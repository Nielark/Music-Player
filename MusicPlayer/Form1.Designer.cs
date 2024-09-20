namespace MusicPlayer
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            PnlHeader = new Panel();
            panel2 = new Panel();
            panel5 = new Panel();
            TblMusicList = new TableLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            panel1 = new Panel();
            pictureBox4 = new PictureBox();
            textBox2 = new TextBox();
            label5 = new Label();
            CbSortMusic = new Modernial.Controls.PoisonComboBox();
            PnlControls = new Panel();
            PicBoxShuffleMusic = new PictureBox();
            label1 = new Label();
            PicBoxRepeatMusic = new PictureBox();
            PicBoxPlayNextMusic = new PictureBox();
            PicboxPlayPreviousMusic = new PictureBox();
            PicBoxPlayAndPause = new PictureBox();
            TbSeekMusic = new Modernial.Controls.DungeonTrackBar();
            LblMusicLength = new Label();
            LblMusicDurationCtr = new Label();
            panel6 = new Panel();
            pictureBox1 = new PictureBox();
            LblShowPlayTitle = new Label();
            LblShowPlayArtist = new Label();
            PnlSideBar = new Panel();
            TimerTitleMarquee = new System.Windows.Forms.Timer(components);
            TimerArtistMarquee = new System.Windows.Forms.Timer(components);
            TimerMusicDuration = new System.Windows.Forms.Timer(components);
            panel2.SuspendLayout();
            panel5.SuspendLayout();
            TblMusicList.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            PnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxShuffleMusic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxRepeatMusic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayNextMusic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicboxPlayPreviousMusic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayAndPause).BeginInit();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // PnlHeader
            // 
            PnlHeader.BackColor = SystemColors.ButtonHighlight;
            PnlHeader.Dock = DockStyle.Top;
            PnlHeader.Location = new Point(0, 0);
            PnlHeader.Name = "PnlHeader";
            PnlHeader.Size = new Size(900, 30);
            PnlHeader.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(PnlControls);
            panel2.Controls.Add(PnlSideBar);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 30);
            panel2.Name = "panel2";
            panel2.Size = new Size(900, 570);
            panel2.TabIndex = 1;
            // 
            // panel5
            // 
            panel5.AutoScroll = true;
            panel5.BackColor = Color.RosyBrown;
            panel5.Controls.Add(TblMusicList);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(200, 47);
            panel5.Name = "panel5";
            panel5.Size = new Size(700, 423);
            panel5.TabIndex = 3;
            // 
            // TblMusicList
            // 
            TblMusicList.AutoSize = true;
            TblMusicList.ColumnCount = 4;
            TblMusicList.ColumnStyles.Add(new ColumnStyle());
            TblMusicList.ColumnStyles.Add(new ColumnStyle());
            TblMusicList.ColumnStyles.Add(new ColumnStyle());
            TblMusicList.ColumnStyles.Add(new ColumnStyle());
            TblMusicList.Controls.Add(label2, 1, 0);
            TblMusicList.Controls.Add(label3, 2, 0);
            TblMusicList.Controls.Add(label4, 3, 0);
            TblMusicList.Location = new Point(36, 10);
            TblMusicList.Name = "TblMusicList";
            TblMusicList.RowCount = 1;
            TblMusicList.RowStyles.Add(new RowStyle());
            TblMusicList.Size = new Size(386, 23);
            TblMusicList.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.ControlDarkDark;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(0, 1);
            label2.Margin = new Padding(0, 1, 0, 1);
            label2.Name = "label2";
            label2.Padding = new Padding(30, 0, 30, 0);
            label2.Size = new Size(110, 21);
            label2.TabIndex = 4;
            label2.Text = "TITLE";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.ControlDarkDark;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(110, 1);
            label3.Margin = new Padding(0, 1, 0, 1);
            label3.Name = "label3";
            label3.Padding = new Padding(30, 0, 30, 0);
            label3.Size = new Size(123, 21);
            label3.TabIndex = 5;
            label3.Text = "ARTIST";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = SystemColors.ControlDarkDark;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(233, 1);
            label4.Margin = new Padding(0, 1, 0, 1);
            label4.Name = "label4";
            label4.Padding = new Padding(30, 0, 15, 0);
            label4.Size = new Size(153, 21);
            label4.TabIndex = 6;
            label4.Text = "DURATION";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox4);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(CbSortMusic);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(200, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(700, 47);
            panel1.TabIndex = 13;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = SystemColors.Highlight;
            pictureBox4.BackgroundImageLayout = ImageLayout.None;
            pictureBox4.BorderStyle = BorderStyle.FixedSingle;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(187, 6);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(29, 29);
            pictureBox4.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox4.TabIndex = 5;
            pictureBox4.TabStop = false;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(216, 6);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Search";
            textBox2.Size = new Size(220, 29);
            textBox2.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(470, 10);
            label5.Name = "label5";
            label5.Size = new Size(63, 21);
            label5.TabIndex = 12;
            label5.Text = "Sort By:";
            // 
            // CbSortMusic
            // 
            CbSortMusic.DropDownHeight = 109;
            CbSortMusic.FormattingEnabled = true;
            CbSortMusic.IntegralHeight = false;
            CbSortMusic.ItemHeight = 23;
            CbSortMusic.Items.AddRange(new object[] { "A - Z ", "Z - A", "Artist (Asc)", "Artist (Desc)", "Duration (Asc)", "Duration (Desc)" });
            CbSortMusic.Location = new Point(539, 6);
            CbSortMusic.Name = "CbSortMusic";
            CbSortMusic.Size = new Size(140, 29);
            CbSortMusic.TabIndex = 11;
            CbSortMusic.UseSelectable = true;
            CbSortMusic.SelectedIndexChanged += CbSortMusic_SelectedIndexChanged;
            // 
            // PnlControls
            // 
            PnlControls.BackColor = SystemColors.ActiveCaption;
            PnlControls.Controls.Add(PicBoxShuffleMusic);
            PnlControls.Controls.Add(label1);
            PnlControls.Controls.Add(PicBoxRepeatMusic);
            PnlControls.Controls.Add(PicBoxPlayNextMusic);
            PnlControls.Controls.Add(PicboxPlayPreviousMusic);
            PnlControls.Controls.Add(PicBoxPlayAndPause);
            PnlControls.Controls.Add(TbSeekMusic);
            PnlControls.Controls.Add(LblMusicLength);
            PnlControls.Controls.Add(LblMusicDurationCtr);
            PnlControls.Controls.Add(panel6);
            PnlControls.Dock = DockStyle.Bottom;
            PnlControls.Location = new Point(200, 470);
            PnlControls.Name = "PnlControls";
            PnlControls.Size = new Size(700, 100);
            PnlControls.TabIndex = 1;
            // 
            // PicBoxShuffleMusic
            // 
            PicBoxShuffleMusic.Image = (Image)resources.GetObject("PicBoxShuffleMusic.Image");
            PicBoxShuffleMusic.Location = new Point(246, 49);
            PicBoxShuffleMusic.Name = "PicBoxShuffleMusic";
            PicBoxShuffleMusic.Size = new Size(20, 20);
            PicBoxShuffleMusic.SizeMode = PictureBoxSizeMode.CenterImage;
            PicBoxShuffleMusic.TabIndex = 11;
            PicBoxShuffleMusic.TabStop = false;
            PicBoxShuffleMusic.Click += PicBoxShuffleMusic_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(515, 58);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 10;
            label1.Text = "label1";
            // 
            // PicBoxRepeatMusic
            // 
            PicBoxRepeatMusic.Image = (Image)resources.GetObject("PicBoxRepeatMusic.Image");
            PicBoxRepeatMusic.Location = new Point(435, 49);
            PicBoxRepeatMusic.Name = "PicBoxRepeatMusic";
            PicBoxRepeatMusic.Size = new Size(20, 20);
            PicBoxRepeatMusic.SizeMode = PictureBoxSizeMode.CenterImage;
            PicBoxRepeatMusic.TabIndex = 9;
            PicBoxRepeatMusic.TabStop = false;
            PicBoxRepeatMusic.Click += PicBoxRepeatMusic_Click;
            // 
            // PicBoxPlayNextMusic
            // 
            PicBoxPlayNextMusic.Image = (Image)resources.GetObject("PicBoxPlayNextMusic.Image");
            PicBoxPlayNextMusic.Location = new Point(395, 49);
            PicBoxPlayNextMusic.Name = "PicBoxPlayNextMusic";
            PicBoxPlayNextMusic.Size = new Size(20, 20);
            PicBoxPlayNextMusic.SizeMode = PictureBoxSizeMode.CenterImage;
            PicBoxPlayNextMusic.TabIndex = 8;
            PicBoxPlayNextMusic.TabStop = false;
            PicBoxPlayNextMusic.Click += LblPlayNextMusic_Click;
            // 
            // PicboxPlayPreviousMusic
            // 
            PicboxPlayPreviousMusic.Image = (Image)resources.GetObject("PicboxPlayPreviousMusic.Image");
            PicboxPlayPreviousMusic.Location = new Point(286, 49);
            PicboxPlayPreviousMusic.Name = "PicboxPlayPreviousMusic";
            PicboxPlayPreviousMusic.Size = new Size(20, 20);
            PicboxPlayPreviousMusic.SizeMode = PictureBoxSizeMode.CenterImage;
            PicboxPlayPreviousMusic.TabIndex = 7;
            PicboxPlayPreviousMusic.TabStop = false;
            PicboxPlayPreviousMusic.Click += LblPlayPreviousMusic_Click;
            // 
            // PicBoxPlayAndPause
            // 
            PicBoxPlayAndPause.BackColor = Color.FromArgb(192, 255, 192);
            PicBoxPlayAndPause.BackgroundImageLayout = ImageLayout.Stretch;
            PicBoxPlayAndPause.Image = (Image)resources.GetObject("PicBoxPlayAndPause.Image");
            PicBoxPlayAndPause.Location = new Point(325, 34);
            PicBoxPlayAndPause.Name = "PicBoxPlayAndPause";
            PicBoxPlayAndPause.Padding = new Padding(3, 0, 0, 0);
            PicBoxPlayAndPause.Size = new Size(50, 50);
            PicBoxPlayAndPause.SizeMode = PictureBoxSizeMode.CenterImage;
            PicBoxPlayAndPause.TabIndex = 3;
            PicBoxPlayAndPause.TabStop = false;
            PicBoxPlayAndPause.Click += LblPlayAndPause_Click;
            // 
            // TbSeekMusic
            // 
            TbSeekMusic.BorderColor = Color.FromArgb(200, 200, 200);
            TbSeekMusic.DrawValueString = false;
            TbSeekMusic.EmptyBackColor = Color.FromArgb(221, 221, 221);
            TbSeekMusic.FillBackColor = Color.FromArgb(217, 99, 50);
            TbSeekMusic.JumpToMouse = true;
            TbSeekMusic.Location = new Point(60, 6);
            TbSeekMusic.Maximum = 10;
            TbSeekMusic.Minimum = 0;
            TbSeekMusic.MinimumSize = new Size(47, 22);
            TbSeekMusic.Name = "TbSeekMusic";
            TbSeekMusic.Size = new Size(580, 22);
            TbSeekMusic.TabIndex = 6;
            TbSeekMusic.Text = "dungeonTrackBar1";
            TbSeekMusic.ThumbBackColor = Color.FromArgb(244, 244, 244);
            TbSeekMusic.ThumbBorderColor = Color.FromArgb(180, 180, 180);
            TbSeekMusic.Value = 0;
            TbSeekMusic.ValueDivison = Modernial.Controls.DungeonTrackBar.ValueDivisor.By1;
            TbSeekMusic.ValueToSet = 0F;
            TbSeekMusic.ValueChanged += TbSeekMusic_ValueChanged;
            TbSeekMusic.MouseDown += TbSeekMusic_MouseDown;
            TbSeekMusic.MouseUp += TbSeekMusic_MouseUp;
            // 
            // LblMusicLength
            // 
            LblMusicLength.BackColor = Color.Transparent;
            LblMusicLength.Location = new Point(639, 10);
            LblMusicLength.Name = "LblMusicLength";
            LblMusicLength.Size = new Size(55, 15);
            LblMusicLength.TabIndex = 6;
            LblMusicLength.Text = "00:00:00";
            LblMusicLength.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LblMusicDurationCtr
            // 
            LblMusicDurationCtr.BackColor = Color.Transparent;
            LblMusicDurationCtr.Location = new Point(6, 10);
            LblMusicDurationCtr.Name = "LblMusicDurationCtr";
            LblMusicDurationCtr.Size = new Size(55, 15);
            LblMusicDurationCtr.TabIndex = 5;
            LblMusicDurationCtr.Text = "00:00:00";
            LblMusicDurationCtr.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            panel6.Controls.Add(pictureBox1);
            panel6.Controls.Add(LblShowPlayTitle);
            panel6.Controls.Add(LblShowPlayArtist);
            panel6.Location = new Point(6, 34);
            panel6.Name = "panel6";
            panel6.Size = new Size(200, 60);
            panel6.TabIndex = 4;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(60, 60);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // LblShowPlayTitle
            // 
            LblShowPlayTitle.AutoSize = true;
            LblShowPlayTitle.BorderStyle = BorderStyle.FixedSingle;
            LblShowPlayTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblShowPlayTitle.Location = new Point(66, 12);
            LblShowPlayTitle.Name = "LblShowPlayTitle";
            LblShowPlayTitle.Size = new Size(56, 23);
            LblShowPlayTitle.TabIndex = 1;
            LblShowPlayTitle.Text = "label5";
            LblShowPlayTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LblShowPlayArtist
            // 
            LblShowPlayArtist.AutoSize = true;
            LblShowPlayArtist.BorderStyle = BorderStyle.FixedSingle;
            LblShowPlayArtist.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblShowPlayArtist.Location = new Point(66, 35);
            LblShowPlayArtist.Name = "LblShowPlayArtist";
            LblShowPlayArtist.Size = new Size(45, 19);
            LblShowPlayArtist.TabIndex = 2;
            LblShowPlayArtist.Text = "label6";
            LblShowPlayArtist.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // PnlSideBar
            // 
            PnlSideBar.BackColor = SystemColors.ButtonShadow;
            PnlSideBar.Dock = DockStyle.Left;
            PnlSideBar.Location = new Point(0, 0);
            PnlSideBar.Name = "PnlSideBar";
            PnlSideBar.Size = new Size(200, 570);
            PnlSideBar.TabIndex = 0;
            // 
            // TimerTitleMarquee
            // 
            TimerTitleMarquee.Tick += TimerTitleMarquee_Tick;
            // 
            // TimerArtistMarquee
            // 
            TimerArtistMarquee.Tick += TimerArtistMarquee_Tick;
            // 
            // TimerMusicDuration
            // 
            TimerMusicDuration.Interval = 1000;
            TimerMusicDuration.Tick += TimerMusicDuration_Tick;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(panel2);
            Controls.Add(PnlHeader);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += FormMain_Load;
            panel2.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            TblMusicList.ResumeLayout(false);
            TblMusicList.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            PnlControls.ResumeLayout(false);
            PnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxShuffleMusic).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxRepeatMusic).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayNextMusic).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicboxPlayPreviousMusic).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayAndPause).EndInit();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel PnlHeader;
        private Panel panel2;
        private Panel PnlSideBar;
        private Panel PnlControls;
        private TableLayoutPanel TblMusicList;
        private Label label2;
        private Label label3;
        private Panel panel5;
        private Label LblShowPlayArtist;
        private Label LblShowPlayTitle;
        private PictureBox pictureBox1;
        private Label label4;
        private PictureBox PicBoxPlayAndPause;
        private System.Windows.Forms.Timer TimerTitleMarquee;
        private Panel panel6;
        private System.Windows.Forms.Timer TimerArtistMarquee;
        private Label LblMusicLength;
        private Label LblMusicDurationCtr;
        private System.Windows.Forms.Timer TimerMusicDuration;
        private Modernial.Controls.DungeonTrackBar TbSeekMusic;
        private PictureBox PicBoxPlayNextMusic;
        private PictureBox PicboxPlayPreviousMusic;
        private PictureBox PicBoxRepeatMusic;
        private Label label1;
        private PictureBox PicBoxShuffleMusic;
        private Modernial.Controls.PoisonComboBox CbSortMusic;
        private Label label5;
        private Panel panel1;
        private Modernial.Controls.PoisonTextBox TxtSearch;
        private PictureBox pictureBox4;
        private TextBox textBox2;
    }
}
