﻿
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
            this.pictureBoxTarget = new System.Windows.Forms.PictureBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonCamera = new System.Windows.Forms.Button();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.comboBoxResoulution = new System.Windows.Forms.ComboBox();
            this.btnSaveShoot = new System.Windows.Forms.Button();
            this.timerShowShoot = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
            this.pictureBoxSource = new System.Windows.Forms.PictureBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.menuStripMainWindow = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModifyFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerDelay = new System.Windows.Forms.Timer(this.components);
            this.linkLabelUser = new System.Windows.Forms.LinkLabel();
            this.comboBoxModel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).BeginInit();
            this.menuStripMainWindow.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxTarget
            // 
            this.pictureBoxTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxTarget.Location = new System.Drawing.Point(552, 67);
            this.pictureBoxTarget.Name = "pictureBoxTarget";
            this.pictureBoxTarget.Size = new System.Drawing.Size(300, 220);
            this.pictureBoxTarget.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTarget.TabIndex = 0;
            this.pictureBoxTarget.TabStop = false;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(22, 35);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "选择图片";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonCamera
            // 
            this.buttonCamera.Location = new System.Drawing.Point(123, 34);
            this.buttonCamera.Name = "buttonCamera";
            this.buttonCamera.Size = new System.Drawing.Size(75, 23);
            this.buttonCamera.TabIndex = 2;
            this.buttonCamera.Text = "打开相机";
            this.buttonCamera.UseVisualStyleBackColor = true;
            this.buttonCamera.Visible = false;
            this.buttonCamera.Click += new System.EventHandler(this.buttonCamera_Click);
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(226, 36);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(121, 20);
            this.comboBoxCamera.TabIndex = 3;
            this.comboBoxCamera.Visible = false;
            this.comboBoxCamera.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamera_SelectedIndexChanged);
            // 
            // comboBoxResoulution
            // 
            this.comboBoxResoulution.FormattingEnabled = true;
            this.comboBoxResoulution.Location = new System.Drawing.Point(362, 36);
            this.comboBoxResoulution.Name = "comboBoxResoulution";
            this.comboBoxResoulution.Size = new System.Drawing.Size(71, 20);
            this.comboBoxResoulution.TabIndex = 4;
            this.comboBoxResoulution.Visible = false;
            this.comboBoxResoulution.SelectedIndexChanged += new System.EventHandler(this.comboBoxResoulution_SelectedIndexChanged);
            // 
            // btnSaveShoot
            // 
            this.btnSaveShoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveShoot.Location = new System.Drawing.Point(777, 34);
            this.btnSaveShoot.Name = "btnSaveShoot";
            this.btnSaveShoot.Size = new System.Drawing.Size(75, 23);
            this.btnSaveShoot.TabIndex = 7;
            this.btnSaveShoot.Text = "保存截图";
            this.btnSaveShoot.UseVisualStyleBackColor = true;
            this.btnSaveShoot.Visible = false;
            this.btnSaveShoot.Click += new System.EventHandler(this.btnSaveShoot_Click);
            // 
            // timerShowShoot
            // 
            this.timerShowShoot.Interval = 33;
            this.timerShowShoot.Tick += new System.EventHandler(this.timerShowShoot_Tick);
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Location = new System.Drawing.Point(12, 335);
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowTemplate.Height = 23;
            this.dataGridViewResult.Size = new System.Drawing.Size(840, 251);
            this.dataGridViewResult.TabIndex = 8;
            // 
            // videoSourcePlayer
            // 
            this.videoSourcePlayer.Location = new System.Drawing.Point(12, 302);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new System.Drawing.Size(360, 219);
            this.videoSourcePlayer.TabIndex = 5;
            this.videoSourcePlayer.VideoSource = null;
            this.videoSourcePlayer.Visible = false;
            // 
            // pictureBoxSource
            // 
            this.pictureBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxSource.Location = new System.Drawing.Point(123, 67);
            this.pictureBoxSource.Name = "pictureBoxSource";
            this.pictureBoxSource.Size = new System.Drawing.Size(300, 220);
            this.pictureBoxSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSource.TabIndex = 0;
            this.pictureBoxSource.TabStop = false;
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(403, 302);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "检测图片";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // menuStripMainWindow
            // 
            this.menuStripMainWindow.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripMainWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.ViewToolStripMenuItem,
            this.ToolsToolStripMenuItem,
            this.AboutToolStripMenuItem});
            this.menuStripMainWindow.Location = new System.Drawing.Point(0, 0);
            this.menuStripMainWindow.Name = "menuStripMainWindow";
            this.menuStripMainWindow.Size = new System.Drawing.Size(867, 25);
            this.menuStripMainWindow.TabIndex = 9;
            this.menuStripMainWindow.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ModifyFileToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.FileToolStripMenuItem.Text = "文件";
            // 
            // ModifyFileToolStripMenuItem
            // 
            this.ModifyFileToolStripMenuItem.Name = "ModifyFileToolStripMenuItem";
            this.ModifyFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ModifyFileToolStripMenuItem.Text = "修改文件名";
            this.ModifyFileToolStripMenuItem.Visible = false;
            this.ModifyFileToolStripMenuItem.Click += new System.EventHandler(this.ModifyFileToolStripMenuItem_Click);
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.ViewToolStripMenuItem.Text = "视图";
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.historyToolStripMenuItem});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.ToolsToolStripMenuItem.Text = "工具";
            // 
            // historyToolStripMenuItem
            // 
            this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            this.historyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.historyToolStripMenuItem.Text = "查看历史记录";
            this.historyToolStripMenuItem.Click += new System.EventHandler(this.historyToolStripMenuItem_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.AboutToolStripMenuItem.Text = "关于";
            // 
            // timerDelay
            // 
            this.timerDelay.Tick += new System.EventHandler(this.timerDelay_Tick);
            // 
            // linkLabelUser
            // 
            this.linkLabelUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelUser.AutoSize = true;
            this.linkLabelUser.Location = new System.Drawing.Point(811, 40);
            this.linkLabelUser.Name = "linkLabelUser";
            this.linkLabelUser.Size = new System.Drawing.Size(41, 12);
            this.linkLabelUser.TabIndex = 10;
            this.linkLabelUser.TabStop = true;
            this.linkLabelUser.Text = "用户：";
            this.linkLabelUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxModel
            // 
            this.comboBoxModel.FormattingEnabled = true;
            this.comboBoxModel.Location = new System.Drawing.Point(123, 37);
            this.comboBoxModel.Name = "comboBoxModel";
            this.comboBoxModel.Size = new System.Drawing.Size(121, 20);
            this.comboBoxModel.TabIndex = 11;
            this.comboBoxModel.SelectedIndexChanged += new System.EventHandler(this.comboBoxModel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "源图片：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(481, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "检测结果：";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 598);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxModel);
            this.Controls.Add(this.linkLabelUser);
            this.Controls.Add(this.dataGridViewResult);
            this.Controls.Add(this.btnSaveShoot);
            this.Controls.Add(this.videoSourcePlayer);
            this.Controls.Add(this.comboBoxResoulution);
            this.Controls.Add(this.comboBoxCamera);
            this.Controls.Add(this.buttonCamera);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.pictureBoxSource);
            this.Controls.Add(this.pictureBoxTarget);
            this.Controls.Add(this.menuStripMainWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStripMainWindow;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "基于深度学习的道路缺陷检测系统";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).EndInit();
            this.menuStripMainWindow.ResumeLayout(false);
            this.menuStripMainWindow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxTarget;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonCamera;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.ComboBox comboBoxResoulution;
        private System.Windows.Forms.Button btnSaveShoot;
        private System.Windows.Forms.Timer timerShowShoot;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer;
        private System.Windows.Forms.PictureBox pictureBoxSource;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.MenuStrip menuStripMainWindow;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ModifyFileToolStripMenuItem;
        private System.Windows.Forms.Timer timerDelay;
        private System.Windows.Forms.LinkLabel linkLabelUser;
        private System.Windows.Forms.ComboBox comboBoxModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}