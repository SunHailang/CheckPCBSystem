using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CheckPCBSystem
{
    /// <summary>
    /// 邮件发送服务：检测完成后自动发送包含检测结果的通知邮件。
    /// </summary>
    internal static class EmailService
    {
        private const int DefaultSmtpTimeoutMs = 15000;

        /// <summary>
        /// 异步发送检测结果邮件。
        /// 邮件发送失败时仅记录错误，不会影响主界面正常使用。
        /// </summary>
        /// <param name="userName">执行检测的用户名</param>
        /// <param name="imageFileName">被检测的图片文件名</param>
        /// <param name="results">检测结果列表</param>
        public static void SendDetectionEmailAsync(string userName, string imageFileName, List<ResultData> results)
        {
            string enabledStr = ConfigurationManager.AppSettings["Email.Enabled"] ?? "false";
            bool enabled;
            if (!bool.TryParse(enabledStr, out enabled) || !enabled)
            {
                return;
            }

            // 在后台线程发送，避免阻塞 UI
            Task.Run(() =>
            {
                try
                {
                    SendEmail(userName, imageFileName, results);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"EmailService Error: {ex}");
                }
            });
        }

        private static void SendEmail(string userName, string imageFileName, List<ResultData> results)
        {
            string smtpHost = ConfigurationManager.AppSettings["Email.SmtpHost"] ?? string.Empty;
            string portStr = ConfigurationManager.AppSettings["Email.SmtpPort"] ?? "465";
            string sslStr = ConfigurationManager.AppSettings["Email.SmtpSsl"] ?? "true";
            string senderAddress = ConfigurationManager.AppSettings["Email.SenderAddress"] ?? string.Empty;
            string senderPassword = ConfigurationManager.AppSettings["Email.SenderPassword"] ?? string.Empty;
            string recipientAddress = ConfigurationManager.AppSettings["Email.RecipientAddress"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(senderAddress)
                || string.IsNullOrWhiteSpace(recipientAddress))
            {
                Console.WriteLine("EmailService: 邮件配置不完整，跳过发送。");
                return;
            }

            int port;
            if (!int.TryParse(portStr, out port) || port <= 0)
            {
                Console.WriteLine($"EmailService: 无效的 SMTP 端口配置 \"{portStr}\"，使用默认端口 465。");
                port = 465;
            }

            bool useSsl;
            if (!bool.TryParse(sslStr, out useSsl))
            {
                Console.WriteLine($"EmailService: 无效的 SSL 配置 \"{sslStr}\"，默认启用 SSL。");
                useSsl = true;
            }

            string subject = $"【PCB检测系统】检测完成通知 - {imageFileName}";
            string body = BuildEmailBody(userName, imageFileName, results);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(senderAddress);

                // 支持多收件人（英文分号分隔）
                foreach (string addr in recipientAddress.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = addr.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        mail.To.Add(new MailAddress(trimmed));
                    }
                }

                if (mail.To.Count == 0)
                {
                    Console.WriteLine("EmailService: 未设置有效的收件人地址，跳过发送。");
                    return;
                }

                mail.Subject = subject;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient(smtpHost, port))
                {
                    client.EnableSsl = useSsl;
                    client.Credentials = new NetworkCredential(senderAddress, senderPassword);
                    client.Timeout = DefaultSmtpTimeoutMs;
                    client.Send(mail);
                }
            }

            Console.WriteLine($"EmailService: 检测结果邮件已发送至 {recipientAddress}。");
        }

        private static string BuildEmailBody(string userName, string imageFileName, List<ResultData> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><body style=\"font-family:Microsoft YaHei,Arial,sans-serif;\">");
            sb.AppendLine("<h2 style=\"color:#2c3e50;\">PCB 检测系统 - 检测结果通知</h2>");
            sb.AppendLine("<hr/>");

            sb.AppendLine("<table style=\"border-collapse:collapse;\">");
            sb.AppendLine($"<tr><td style=\"padding:4px 8px;\"><b>检测用户：</b></td><td style=\"padding:4px 8px;\">{HtmlEncode(userName)}</td></tr>");
            sb.AppendLine($"<tr><td style=\"padding:4px 8px;\"><b>检测图片：</b></td><td style=\"padding:4px 8px;\">{HtmlEncode(imageFileName)}</td></tr>");
            sb.AppendLine($"<tr><td style=\"padding:4px 8px;\"><b>检测时间：</b></td><td style=\"padding:4px 8px;\">{DateTime.Now:yyyy-MM-dd HH:mm:ss}</td></tr>");
            sb.AppendLine($"<tr><td style=\"padding:4px 8px;\"><b>缺陷数量：</b></td><td style=\"padding:4px 8px;\">{results.Count}</td></tr>");
            sb.AppendLine("</table>");

            if (results.Count > 0)
            {
                sb.AppendLine("<h3 style=\"color:#e74c3c;\">检测到的缺陷列表</h3>");
                sb.AppendLine("<table border=\"1\" cellpadding=\"6\" cellspacing=\"0\" style=\"border-collapse:collapse;\">");
                sb.AppendLine("<tr style=\"background:#2c3e50;color:#fff;\">");
                sb.AppendLine("<th>ID</th><th>缺陷类型</th><th>起始坐标</th><th>结束坐标</th><th>置信度</th>");
                sb.AppendLine("</tr>");

                for (int i = 0; i < results.Count; i++)
                {
                    ResultData r = results[i];
                    string rowBg = (i % 2 == 0) ? "#f9f9f9" : "#ffffff";
                    sb.AppendLine($"<tr style=\"background:{rowBg};\">");
                    sb.AppendLine($"<td align=\"center\">{r.Index}</td>");
                    sb.AppendLine($"<td>{HtmlEncode(r.Data)}</td>");
                    sb.AppendLine($"<td>{HtmlEncode(r.StartPos.ToString())}</td>");
                    sb.AppendLine($"<td>{HtmlEncode(r.EndPos.ToString())}</td>");
                    sb.AppendLine($"<td align=\"center\">{r.Level:P1}</td>");
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</table>");
            }
            else
            {
                sb.AppendLine("<p style=\"color:#27ae60;\">本次检测未发现缺陷。</p>");
            }

            sb.AppendLine("<hr/>");
            sb.AppendLine("<p style=\"color:#7f8c8d;font-size:12px;\">此邮件由 PCB 检测系统自动发送，请勿直接回复。</p>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

        private static string HtmlEncode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }
    }
}
