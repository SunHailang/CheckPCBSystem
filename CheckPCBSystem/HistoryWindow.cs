using CheckPCBSystem.UGUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CheckPCBSystem
{

    public partial class HistoryWindow : Form
    {
        private string UserName;

        private const string HistoryPath = "C:\\Users\\long\\Desktop\\predict2";
        private const string SourcePath = "resource";
        private const string TargetPath = "detect";
        private const string TextPath = "text";

        private List<string> RoadDefectList = new List<string>
        {
            "纵向裂缝",
            "横向裂缝",
            "龟裂裂缝",
            "坑洞",
            "修补",
            "未知"
        };

        private int SelectRowIndex = -1;

        private StringBuilder Results = new StringBuilder();

        private List<string> historyFiles = new List<string>();
        private DataTable HistoryFileTable = new DataTable();
        public HistoryWindow()
        {
            InitializeComponent();
        }

        private void HistroyWindow_Load(object sender, EventArgs e)
        {
            SelectRowIndex = -1;

            this.dataGridViewHistroy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewHistroy.CellClick += dataGridViewHistory_CellClick;
            // 初始化DataView
            HistoryFileTable.Columns.Add(new DataColumn("ID", typeof(string)));
            HistoryFileTable.Columns.Add(new DataColumn("File", typeof(string)));
            // 加载历史记录
            string historyFilePath = Path.Combine(HistoryPath, $"{UserName}_history.txt");

            if (!File.Exists(historyFilePath))
            {
                return;
            }
            historyFiles.Clear();
            string[] fileList = File.ReadAllLines(historyFilePath);
            for (int i = 0; i < fileList.Length; i++)
            {
                string[] strs = fileList[i].Split(' ');
                DataRow dr = HistoryFileTable.NewRow();
                dr[0] = $"{i + 1}";
                dr[1] = strs[0];
                historyFiles.Add(strs[0]);
                HistoryFileTable.Rows.Add(dr);
            }
            dataGridViewHistroy.DataSource = HistoryFileTable;
        }

        public void SetAdmin(string user)
        {
            UserName = user;
        }

        private void dataGridViewHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SelectRowIndex == e.RowIndex) return;
            if(e.RowIndex < 0 || e.RowIndex >= historyFiles.Count) return;

            SelectRowIndex = e.RowIndex;

            string file = historyFiles[SelectRowIndex];
            string sourcePath = Path.Combine(HistoryPath, $"{SourcePath}/{file}");
            this.pictureBoxSource.Image = new Bitmap(sourcePath);
            string targetPath = Path.Combine(HistoryPath, $"{TargetPath}/{file}");
            this.pictureBoxTarget.Image = new Bitmap(targetPath);
            string textPath = Path.Combine(HistoryPath, $"{TextPath}/{Path.GetFileNameWithoutExtension(sourcePath)}.txt");
            string[] stream = File.ReadAllLines(textPath);

            Results.Clear();

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
                    ResultData data = new ResultData
                    {
                        Index = i + 1,
                        Data = string.Format("D{0:D2} : {1}", model + 1, RoadDefectList[defectIndex]),
                        StartPos = leftUp,
                        EndPos = rightDown,
                        Level = rate
                    };
                    Results.Append(data.ToString());
                    Results.AppendLine(Environment.NewLine);
                }
            }
            this.textBoxResult.Text = Results.ToString();
        }

        private void HistoryWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.dataGridViewHistroy.CellClick -= dataGridViewHistory_CellClick;
            FormManager.Instance.BackClose();
        }
    }
}
