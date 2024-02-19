using AForge.Controls;
using AForge.Video.DirectShow;
using CheckPCBSystem.UGUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CheckPCBSystem
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();


            VideoSourcePlayer.CheckForIllegalCrossThreadCalls = false;
        }

        // 所有相机设备
        private FilterInfoCollection videoDevices;
        // 相机设备
        private VideoCaptureDevice videoDevice;
        // 摄像头分辨率
        private VideoCapabilities[] videoCapabilities;

        private bool CameraIsRunning = false;

        private string CheckImagePath;

        private List<ResultData> resultDataList = new List<ResultData>();
        private DataTable dt = new DataTable();

        private string UserName;

        public void SetAdmin(string user)
        {
            UserName = user;
        }

        private List<string> RoadDefectList = new List<string>
        {
            "纵向裂缝",
            "横向裂缝",
            "龟裂裂缝",
            "坑洞",
            "修补",
            "未知"
        };

        private List<string> ModelList = new List<string>
        {
            "YOLOv8-EBS.pt",
            "YOLOv8m.pt",
            "YOLOv7.pt",
            "YOLOv5.pt"
        };
        private int SelectModelIndex = 0;
        private void MainWindow_Load(object sender, EventArgs e)
        {
            // 初始化模型
            this.comboBoxModel.Items.Clear();
            for (int i = 0; i < ModelList.Count; i++)
            {
                this.comboBoxModel.Items.Add(ModelList[i]);
            }
            SelectModelIndex = 0;
            this.comboBoxModel.SelectedIndex = SelectModelIndex;
            // 初始化用户名
            this.linkLabelUser.Text = $"用户：{UserName}";
            int posWidth = this.linkLabelUser.Width - 41;
            this.linkLabelUser.Location = new Point(811 - posWidth, this.linkLabelUser.Location.Y);
            // 初始化相机
            this.videoSourcePlayer.NewFrame += VideoSourcePlayerNewFrame;
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count != 0)
            {
                for (int i = 0; i < videoDevices.Count; i++)
                {
                    FilterInfo camera = videoDevices[i];
                    //this.comboBoxCamera.Items.Add(camera.Name);

                    ToolStripItem item = this.OpenCameraToolStripMenuItem.DropDownItems.Add(camera.Name);
                    item.Click += ToolStripItem_ItemClick;
                }

                //this.comboBoxCamera.SelectedIndex = 0;
                //videoDevice = new VideoCaptureDevice(videoDevices[this.comboBoxCamera.SelectedIndex].MonikerString);

                CameraIsRunning = false;
            }
            else
            {
                //this.comboBoxCamera.Items.Add("没有相机设备");
            }
            // 初始化结果表
            dt.Columns.Add(new DataColumn("ID"));
            dt.Columns.Add(new DataColumn("Result"));
            dt.Columns.Add(new DataColumn("Position"));
            dt.Columns.Add(new DataColumn("Level"));
        }

        private void ToolStripItem_ItemClick(object obj, EventArgs e)
        {
            string objName = obj.ToString();
            int videoIndex = -1;
            for (int i = 0; i < videoDevices.Count; i++)
            {
                if (videoDevices[i].Name == objName)
                {
                    videoIndex = i; 
                    break;
                }
            }
            if (videoIndex >= 0)
            {
                videoDevice = new VideoCaptureDevice(videoDevices[videoIndex].MonikerString);
            }else
            {
                videoDevice = null;
            }
            if (videoDevice != null)
            {
                // 多线程 报错
                return;
                if (!CameraIsRunning)
                {
                    videoSourcePlayer.Start();

                    this.timerShowShoot.Enabled = true;

                    CameraIsRunning = true;
                }
                else
                {
                    videoSourcePlayer.WaitForStop();

                    this.timerShowShoot.Enabled = false;

                    CameraIsRunning = false;

                    this.pictureBoxTarget.Image = null;
                }
            }
        }

        private void VideoSourcePlayerNewFrame(object sender, ref Bitmap image)
        {
            image.RotateFlip(RotateFlipType.Rotate180FlipY);
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.videoSourcePlayer.NewFrame -= VideoSourcePlayerNewFrame;
                this.Dispose();
                this.Close();
                Environment.Exit(Environment.ExitCode);
            }
            catch (Exception error)
            {
                Console.WriteLine($"MainWindow_FormClosed Error:{error.Message}");
            }
        }



        private void buttonCamera_Click(object sender, EventArgs e)
        {

        }

        public delegate void MyInvoke();
        private void comboBoxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (videoDevices.Count > 0)
            {
                MyInvoke mi = new MyInvoke(SetVideoSource);
                this.BeginInvoke(mi);
            }
        }

        private void SetVideoSource()
        {
            //videoDevice = new VideoCaptureDevice(videoDevices[this.comboBoxCamera.SelectedIndex].MonikerString);
            //this.videoSourcePlayer.VideoSource = videoDevice;
            //GetCameraResolution(videoDevice);
        }

        private void GetCameraResolution(VideoCaptureDevice device)
        {
            // 设备的摄像头分辨率组
            videoCapabilities = device.VideoCapabilities;
            for (int i = 0; i < videoCapabilities.Length; i++)
            {
                var capability = videoCapabilities[i];
                //comboBoxResoulution.Items.Add($"{capability.FrameSize.Width}*{capability.FrameSize.Height}");
            }
            //comboBoxResoulution.SelectedIndex = 0;
        }


        private void btnSaveShoot_Click(object sender, EventArgs e)
        {
            if (this.pictureBoxTarget.Image != null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "图片(*.jpg)|*.jpg";
                dialog.Multiselect = false;
                dialog.ShowDialog();
                if (dialog.FileNames != null && dialog.FileNames.Length > 0)
                {
                    this.pictureBoxTarget.Image.Save(dialog.FileNames[0]);
                }
            }
        }

        private void timerShowShoot_Tick(object sender, EventArgs e)
        {
            if (this.videoDevice != null)
            {
                var map = this.videoSourcePlayer.GetCurrentVideoFrame();
                if (map != null)
                {
                    this.pictureBoxTarget.Image = map;
                }
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (this.pictureBoxSource.Image != null)
            {
                //Random ro = new Random();
                //int timeDu = ro.Next(0, 1000);
                this.timerDelay.Interval = 10;// timeDu;
                this.timerDelay.Enabled = true;
            }
        }
        private void RefreshResultDataView()
        {
            dt.Rows.Clear();
            for (int i = 0; i < resultDataList.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = resultDataList[i].Index;
                dr[1] = resultDataList[i].Data;
                dr[2] = $"{resultDataList[i].StartPos},{resultDataList[i].EndPos}";
                dr[3] = resultDataList[i].Level;
                dt.Rows.Add(dr);
            }
            dataGridViewResult.DataSource = dt;
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 打开HistoryWindow
            var history = new HistoryWindow();
            history.SetAdmin(UserName);
            FormManager.Instance.OpenForm(history);
        }

        private void ModifyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string root = "C:\\Users\\long\\Desktop\\predict2";
            string source = Path.Combine(root, "resource");
            string target = Path.Combine(root, "detect");
            string text = Path.Combine(root, "text");

            DirectoryInfo sourceDir = new DirectoryInfo(source);
            foreach (FileInfo file in sourceDir.GetFiles())
            {
                string path = Path.GetFileName(file.FullName);
                string[] paths = path.Split('_');
                if (paths.Length >= 3)
                {
                    string targetPath = Path.Combine(Path.GetDirectoryName(file.FullName), paths[2]);
                    File.Move(file.FullName, targetPath);
                }
            }

            DirectoryInfo targetDir = new DirectoryInfo(target);
            foreach (FileInfo file in targetDir.GetFiles())
            {
                string path = Path.GetFileName(file.FullName);
                string[] paths = path.Split('_');
                if (paths.Length >= 3)
                {
                    string targetPath = Path.Combine(Path.GetDirectoryName(file.FullName), paths[2]);
                    File.Move(file.FullName, targetPath);
                }
            }

            DirectoryInfo textDir = new DirectoryInfo(text);
            foreach (FileInfo file in textDir.GetFiles())
            {
                string path = Path.GetFileName(file.FullName);
                string[] paths = path.Split('_');
                if (paths.Length >= 3)
                {
                    string targetPath = Path.Combine(Path.GetDirectoryName(file.FullName), paths[2]);
                    File.Move(file.FullName, targetPath);
                }
            }

        }

        private void timerDelay_Tick(object sender, EventArgs e)
        {
            // 拿 源数据  检测  得到 目标数据
            if (!File.Exists(CheckImagePath))
            {
                return;
            }
            string dicitName = Path.GetDirectoryName(CheckImagePath);
            string fileName = Path.GetFileName(CheckImagePath);
            string fileNameExt = Path.GetFileNameWithoutExtension(CheckImagePath);

            string targetImage = Path.Combine(dicitName, $"../detect/{fileName}");
            string targetTextPath = Path.Combine(dicitName, $"../text/{fileNameExt}.txt");

            using (Bitmap target = new Bitmap(targetImage))
            {
                this.pictureBoxTarget.Image = (Bitmap)target.Clone();
            }

            if (!File.Exists(targetTextPath))
            {
                return;
            }

            // 添加历史记录
            string historyPath = Path.Combine(dicitName, $"../{UserName}_history.txt");
            if (!File.Exists(historyPath))
            {
                using (FileStream fs = File.Create(historyPath))
                {

                }
            }
            bool hasExist = false;
            string[] historyList = File.ReadAllLines(historyPath);
            foreach (string line in historyList)
            {
                string[] files = line.Split(' ');
                if (files.Length > 0 && files[0] == fileName)
                {
                    hasExist = true;
                    break;
                }
            }
            if (!hasExist)
            {
                using (StreamWriter sw = File.AppendText(historyPath))
                {
                    sw.WriteLine($"{fileName} {dicitName}");
                }
            }

            resultDataList.Clear();
            string[] stream = File.ReadAllLines(targetTextPath);
            for (int i = 0; i < stream.Length; i++)
            {
                string[] strs = stream[i].Split(' ');
                if (strs.Length >= 6)
                {
                    int model = Convert.ToInt32(strs[0]);
                    float rate = Convert.ToSingle(strs[1]);

                    Tuple<int, int> leftUp = new Tuple<int, int>(Convert.ToInt32(strs[2]), Convert.ToInt32(strs[3]));
                    Tuple<int, int> rightDown = new Tuple<int, int>(Convert.ToInt32(strs[4]), Convert.ToInt32(strs[5]));
                    int defectIndex = model < 0 || model >= RoadDefectList.Count ? RoadDefectList.Count - 1 : model;
                    resultDataList.Add(new ResultData
                    {
                        Index = i + 1,
                        Data = string.Format("D{0:D2} : {1}", model + 1, RoadDefectList[defectIndex]),
                        StartPos = leftUp,
                        EndPos = rightDown,
                        Level = rate
                    }); ;
                }
            }
            // 刷新Result
            RefreshResultDataView();

            this.timerDelay.Enabled = false;
        }

        private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectModelIndex = comboBoxModel.SelectedIndex;
        }

        private void SelectImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择图片...";
            dialog.Multiselect = false;
            dialog.Filter = "图片文件 (*.jpg)|*.jpg|All files (*.*)|*.*";// "*.jpg|*.jpeg|*.png";
            dialog.ShowDialog();
            var selects = dialog.FileNames;
            if (selects != null && selects.Length > 0)
            {
                CheckImagePath = selects[0];
                using (Bitmap map = new Bitmap(selects[0]))
                {
                    this.pictureBoxSource.Image = (Bitmap)map.Clone();
                }
            }
        }

        private void SelectVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择视频...";
            dialog.Multiselect = false;
            dialog.Filter = "视频文件 (*.mp4)|*.mp4|All files (*.*)|*.*";// "*.jpg|*.jpeg|*.png";
            dialog.ShowDialog();
            var selects = dialog.FileNames;
            if (selects != null && selects.Length > 0)
            {
                //CheckImagePath = selects[0];
                //using (Bitmap map = new Bitmap(selects[0]))
                //{
                //    this.pictureBoxSource.Image = (Bitmap)map.Clone();
                //}
            }
        }

        private void vlcControlPlay_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDir = new FileInfo(currentAssembly.Location).DirectoryName;
            if(currentDir == null)
            {
                return;
            }
            if(IntPtr.Size == 4)
            {
                e.VlcLibDirectory = new DirectoryInfo(Path.GetFullPath(@".\libvlc\win-x86"));
            }
            else
            {
                e.VlcLibDirectory = new DirectoryInfo(Path.GetFullPath(@".\libvlc\win-x64"));
            }
        }
    }
}
