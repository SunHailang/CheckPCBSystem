
namespace CheckPCBSystem
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonCamera = new System.Windows.Forms.Button();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.comboBoxResoulution = new System.Windows.Forms.ComboBox();
            this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
            this.btnShootCamera = new System.Windows.Forms.Button();
            this.btnSaveShoot = new System.Windows.Forms.Button();
            this.timerShowShoot = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxImage.Location = new System.Drawing.Point(427, 53);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(360, 219);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 0;
            this.pictureBoxImage.TabStop = false;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(22, 21);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "选择图片";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonCamera
            // 
            this.buttonCamera.Location = new System.Drawing.Point(123, 20);
            this.buttonCamera.Name = "buttonCamera";
            this.buttonCamera.Size = new System.Drawing.Size(75, 23);
            this.buttonCamera.TabIndex = 2;
            this.buttonCamera.Text = "打开相机";
            this.buttonCamera.UseVisualStyleBackColor = true;
            this.buttonCamera.Click += new System.EventHandler(this.buttonCamera_Click);
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(226, 22);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(121, 20);
            this.comboBoxCamera.TabIndex = 3;
            this.comboBoxCamera.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamera_SelectedIndexChanged);
            // 
            // comboBoxResoulution
            // 
            this.comboBoxResoulution.FormattingEnabled = true;
            this.comboBoxResoulution.Location = new System.Drawing.Point(362, 22);
            this.comboBoxResoulution.Name = "comboBoxResoulution";
            this.comboBoxResoulution.Size = new System.Drawing.Size(71, 20);
            this.comboBoxResoulution.TabIndex = 4;
            this.comboBoxResoulution.SelectedIndexChanged += new System.EventHandler(this.comboBoxResoulution_SelectedIndexChanged);
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.Location = new System.Drawing.Point(12, 53);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size(360, 219);
            this.videoSourcePlayer.TabIndex = 5;
            this.videoSourcePlayer.VideoSource = null;
            // 
            // btnShootCamera
            // 
            this.btnShootCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShootCamera.Location = new System.Drawing.Point(712, 20);
            this.btnShootCamera.Name = "btnShootCamera";
            this.btnShootCamera.Size = new System.Drawing.Size(75, 23);
            this.btnShootCamera.TabIndex = 6;
            this.btnShootCamera.Text = "截图";
            this.btnShootCamera.UseVisualStyleBackColor = true;
            this.btnShootCamera.Click += new System.EventHandler(this.btnShootCamera_Click);
            // 
            // btnSaveShoot
            // 
            this.btnSaveShoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveShoot.Location = new System.Drawing.Point(631, 20);
            this.btnSaveShoot.Name = "btnSaveShoot";
            this.btnSaveShoot.Size = new System.Drawing.Size(75, 23);
            this.btnSaveShoot.TabIndex = 7;
            this.btnSaveShoot.Text = "保存截图";
            this.btnSaveShoot.UseVisualStyleBackColor = true;
            this.btnSaveShoot.Click += new System.EventHandler(this.btnSaveShoot_Click);
            // 
            // timerShowShoot
            // 
            this.timerShowShoot.Interval = 33;
            this.timerShowShoot.Tick += new System.EventHandler(this.timerShowShoot_Tick);
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Result,
            this.Position,
            this.Level});
            this.dataGridViewResult.Location = new System.Drawing.Point(12, 363);
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowTemplate.Height = 23;
            this.dataGridViewResult.Size = new System.Drawing.Size(775, 175);
            this.dataGridViewResult.TabIndex = 8;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ID.FillWeight = 50F;
            this.ID.Frozen = true;
            this.ID.HeaderText = "序号";
            this.ID.Name = "ID";
            this.ID.Width = 80;
            // 
            // Result
            // 
            this.Result.FillWeight = 99.49239F;
            this.Result.HeaderText = "结果";
            this.Result.Name = "Result";
            // 
            // Position
            // 
            this.Position.FillWeight = 99.49239F;
            this.Position.HeaderText = "位置";
            this.Position.Name = "Position";
            // 
            // Level
            // 
            this.Level.FillWeight = 99.49239F;
            this.Level.HeaderText = "置信度";
            this.Level.Name = "Level";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 550);
            this.Controls.Add(this.dataGridViewResult);
            this.Controls.Add(this.btnSaveShoot);
            this.Controls.Add(this.btnShootCamera);
            this.Controls.Add(this.videoSourcePlayer);
            this.Controls.Add(this.comboBoxResoulution);
            this.Controls.Add(this.comboBoxCamera);
            this.Controls.Add(this.buttonCamera);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.pictureBoxImage);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "基于深度学习的智能PCB板缺陷检测系统";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonCamera;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.ComboBox comboBoxResoulution;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer;
        private System.Windows.Forms.Button btnShootCamera;
        private System.Windows.Forms.Button btnSaveShoot;
        private System.Windows.Forms.Timer timerShowShoot;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Level;
    }
}