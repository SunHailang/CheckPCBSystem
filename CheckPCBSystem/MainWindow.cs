using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckPCBSystem
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 所有相机设备
        private FilterInfoCollection videoDevices;
        // 相机设备
        private VideoCaptureDevice videoDevice;
        // 摄像头分辨率
        private VideoCapabilities[] videoCapabilities;

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // 初始化相机
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count != 0)
            {
                for (int i = 0; i < videoDevices.Count; i++)
                {
                    FilterInfo camera = videoDevices[i];
                    this.comboBoxCamera.Items.Add(camera.Name);
                }

                this.comboBoxCamera.SelectedIndex = 0;
                videoDevice = new VideoCaptureDevice(videoDevices[this.comboBoxCamera.SelectedIndex].MonikerString);
            }
            else
            {
                this.comboBoxCamera.Items.Add("没有相机设备");
            }
            // 初始化结果表
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择图片...";
            dialog.Multiselect = false;
            dialog.Filter = "图片文件 (*.jpg)|*.jpg|All files (*.*)|*.*";// "*.jpg|*.jpeg|*.png";
            dialog.ShowDialog();
            var selects = dialog.FileNames;
            if (selects != null && selects.Length > 0)
            {
                Bitmap map = new Bitmap(selects[0]);
                pictureBoxImage.Image = map;
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
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
            if (videoDevice != null)
            {
                videoSourcePlayer.Start();

                this.timerShowShoot.Enabled = true;
            }
        }

        private void comboBoxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (videoDevices.Count > 0)
            {
                videoDevice = new VideoCaptureDevice(videoDevices[this.comboBoxCamera.SelectedIndex].MonikerString);
                videoSourcePlayer.VideoSource = videoDevice;
                GetCameraResolution(videoDevice);
            }
        }

        private void GetCameraResolution(VideoCaptureDevice device)
        {
            comboBoxResoulution.Items.Clear();
            // 设备的摄像头分辨率组
            videoCapabilities = device.VideoCapabilities;
            for (int i = 0; i < videoCapabilities.Length; i++)
            {
                var capability = videoCapabilities[i];
                comboBoxResoulution.Items.Add($"{capability.FrameSize.Width}*{capability.FrameSize.Height}");
            }
            comboBoxResoulution.SelectedIndex = 0;
        }

        private void comboBoxResoulution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (videoDevice != null)
            {
                videoDevice.VideoResolution = videoCapabilities[comboBoxResoulution.SelectedIndex];
            }
        }

        private void btnShootCamera_Click(object sender, EventArgs e)
        {
            if (this.videoDevice != null)
            {
                var map = this.videoSourcePlayer.GetCurrentVideoFrame();
                this.pictureBoxImage.Image = map;
            }
        }

        private void btnSaveShoot_Click(object sender, EventArgs e)
        {
            if (this.pictureBoxImage.Image != null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "图片(*.jpg)|*.jpg";
                dialog.Multiselect = false;
                dialog.ShowDialog();
                if (dialog.FileNames != null && dialog.FileNames.Length > 0)
                {
                    this.pictureBoxImage.Image.Save(dialog.FileNames[0]);
                }
            }
        }

        private void timerShowShoot_Tick(object sender, EventArgs e)
        {
            if (this.videoDevice != null)
            {
                var map = this.videoSourcePlayer.GetCurrentVideoFrame();
                this.pictureBoxImage.Image = map;
            }
        }
    }
}
