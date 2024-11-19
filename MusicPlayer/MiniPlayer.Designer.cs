namespace MusicPlayer
{
    partial class MiniPlayer
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
            components = new System.ComponentModel.Container();
            fileSystemWatcher1 = new FileSystemWatcher();
            PnlMain = new Panel();
            pnlHeader = new Panel();
            label3 = new Label();
            pnlRightBar = new Panel();
            PnlHideRightBar = new CustomControls.NielarkPictureBox();
            PicBoxMiniPlayerToggle = new CustomControls.NielarkPictureBox();
            PicBoxShowVolumeBar = new CustomControls.NielarkPictureBox();
            PnlVolumeControl = new Panel();
            PicBoxVolumePicture = new CustomControls.NielarkPictureBox();
            LblVolumeValue = new Label();
            TbVolume = new Modernial.Controls.DungeonTrackBar();
            PicBoxDisplayRightBar = new PictureBox();
            PicBoxRepeatMusic = new CustomControls.NielarkPictureBox();
            PicBoxShuffleMusic = new CustomControls.NielarkPictureBox();
            LblMusicLength = new Label();
            LblMusicDurationCtr = new Label();
            PnlPlayBackInfo = new CustomControls.NielarkPanel();
            PicBoxShowPlayPicture = new PictureBox();
            PnlMarquee = new Panel();
            LblShowPlayTitle = new Label();
            LblShowPlayArtist = new Label();
            PicBoxSkipForward = new CustomControls.NielarkPictureBox();
            PicBoxPlayNextMusic = new CustomControls.NielarkPictureBox();
            PicBoxPlayPreviousMusic = new CustomControls.NielarkPictureBox();
            PicBoxSkipBackward = new CustomControls.NielarkPictureBox();
            PicBoxPlayAndPause = new PictureBox();
            TbSeekMusic = new Modernial.Controls.DungeonTrackBar();
            toolTipPlayerControl = new ToolTip(components);
            TimerMusicDuration = new System.Windows.Forms.Timer(components);
            TimerTitleMarquee = new System.Windows.Forms.Timer(components);
            TimerArtistMarquee = new System.Windows.Forms.Timer(components);
            TimerShowRightBar = new System.Windows.Forms.Timer(components);
            TimerHideRightBar = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            PnlMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlRightBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PnlHideRightBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxMiniPlayerToggle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxShowVolumeBar).BeginInit();
            PnlVolumeControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxVolumePicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxDisplayRightBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxRepeatMusic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxShuffleMusic).BeginInit();
            PnlPlayBackInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxShowPlayPicture).BeginInit();
            PnlMarquee.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxSkipForward).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayNextMusic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayPreviousMusic).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxSkipBackward).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayAndPause).BeginInit();
            SuspendLayout();
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // PnlMain
            // 
            PnlMain.BackColor = Color.FromArgb(48, 49, 52);
            PnlMain.BackgroundImageLayout = ImageLayout.Stretch;
            PnlMain.Controls.Add(pnlHeader);
            PnlMain.Controls.Add(pnlRightBar);
            PnlMain.Controls.Add(PnlVolumeControl);
            PnlMain.Controls.Add(PicBoxDisplayRightBar);
            PnlMain.Controls.Add(PicBoxRepeatMusic);
            PnlMain.Controls.Add(PicBoxShuffleMusic);
            PnlMain.Controls.Add(LblMusicLength);
            PnlMain.Controls.Add(LblMusicDurationCtr);
            PnlMain.Controls.Add(PnlPlayBackInfo);
            PnlMain.Controls.Add(PicBoxSkipForward);
            PnlMain.Controls.Add(PicBoxPlayNextMusic);
            PnlMain.Controls.Add(PicBoxPlayPreviousMusic);
            PnlMain.Controls.Add(PicBoxSkipBackward);
            PnlMain.Controls.Add(PicBoxPlayAndPause);
            PnlMain.Controls.Add(TbSeekMusic);
            PnlMain.Dock = DockStyle.Fill;
            PnlMain.Location = new Point(0, 0);
            PnlMain.MinimumSize = new Size(261, 61);
            PnlMain.Name = "PnlMain";
            PnlMain.Size = new Size(400, 200);
            PnlMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(30, 30, 30);
            pnlHeader.Controls.Add(label3);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(400, 30);
            pnlHeader.TabIndex = 43;
            pnlHeader.MouseDown += PnlHeader_MouseDown;
            pnlHeader.MouseMove += PnlHeader_MouseMove;
            pnlHeader.MouseUp += PnlHeader_MouseUp;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(10, 5);
            label3.Name = "label3";
            label3.Size = new Size(101, 21);
            label3.TabIndex = 4;
            label3.Text = "Music Player";
            // 
            // pnlRightBar
            // 
            pnlRightBar.BackColor = Color.FromArgb(12, 23, 45);
            pnlRightBar.Controls.Add(PnlHideRightBar);
            pnlRightBar.Controls.Add(PicBoxMiniPlayerToggle);
            pnlRightBar.Controls.Add(PicBoxShowVolumeBar);
            pnlRightBar.Location = new Point(400, 30);
            pnlRightBar.Name = "pnlRightBar";
            pnlRightBar.Size = new Size(50, 170);
            pnlRightBar.TabIndex = 44;
            // 
            // PnlHideRightBar
            // 
            PnlHideRightBar.BackColor = Color.Transparent;
            PnlHideRightBar.BorderColor = Color.Empty;
            PnlHideRightBar.BorderRadius = 6;
            PnlHideRightBar.BorderSize = 0;
            PnlHideRightBar.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PnlHideRightBar.HoverBorderColor = Color.Empty;
            PnlHideRightBar.Image = Properties.Resources.close;
            PnlHideRightBar.ImagePadding = new Padding(6);
            PnlHideRightBar.Location = new Point(10, 24);
            PnlHideRightBar.Name = "PnlHideRightBar";
            PnlHideRightBar.Padding = new Padding(6);
            PnlHideRightBar.Size = new Size(30, 30);
            PnlHideRightBar.SizeMode = PictureBoxSizeMode.StretchImage;
            PnlHideRightBar.TabIndex = 42;
            PnlHideRightBar.TabStop = false;
            toolTipPlayerControl.SetToolTip(PnlHideRightBar, "Volume bar");
            PnlHideRightBar.Click += PnlHideRightBar_Click;
            // 
            // PicBoxMiniPlayerToggle
            // 
            PicBoxMiniPlayerToggle.BackColor = Color.Transparent;
            PicBoxMiniPlayerToggle.BorderColor = Color.Empty;
            PicBoxMiniPlayerToggle.BorderRadius = 6;
            PicBoxMiniPlayerToggle.BorderSize = 0;
            PicBoxMiniPlayerToggle.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PicBoxMiniPlayerToggle.HoverBorderColor = Color.Empty;
            PicBoxMiniPlayerToggle.Image = Properties.Resources.exit_mini_player;
            PicBoxMiniPlayerToggle.ImagePadding = new Padding(6);
            PicBoxMiniPlayerToggle.Location = new Point(9, 70);
            PicBoxMiniPlayerToggle.Name = "PicBoxMiniPlayerToggle";
            PicBoxMiniPlayerToggle.Padding = new Padding(6);
            PicBoxMiniPlayerToggle.Size = new Size(30, 30);
            PicBoxMiniPlayerToggle.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxMiniPlayerToggle.TabIndex = 36;
            PicBoxMiniPlayerToggle.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxMiniPlayerToggle, "Exit mini player");
            PicBoxMiniPlayerToggle.Click += PicBoxMiniPlayerToggle_Click;
            // 
            // PicBoxShowVolumeBar
            // 
            PicBoxShowVolumeBar.BackColor = Color.Transparent;
            PicBoxShowVolumeBar.BorderColor = Color.Empty;
            PicBoxShowVolumeBar.BorderRadius = 6;
            PicBoxShowVolumeBar.BorderSize = 0;
            PicBoxShowVolumeBar.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PicBoxShowVolumeBar.HoverBorderColor = Color.Empty;
            PicBoxShowVolumeBar.Image = Properties.Resources.volume_high;
            PicBoxShowVolumeBar.ImagePadding = new Padding(6);
            PicBoxShowVolumeBar.Location = new Point(10, 116);
            PicBoxShowVolumeBar.Name = "PicBoxShowVolumeBar";
            PicBoxShowVolumeBar.Padding = new Padding(6);
            PicBoxShowVolumeBar.Size = new Size(30, 30);
            PicBoxShowVolumeBar.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxShowVolumeBar.TabIndex = 41;
            PicBoxShowVolumeBar.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxShowVolumeBar, "Volume bar");
            PicBoxShowVolumeBar.Click += PicBoxShowVolumeBar_Click;
            // 
            // PnlVolumeControl
            // 
            PnlVolumeControl.BackColor = Color.FromArgb(12, 23, 45);
            PnlVolumeControl.Controls.Add(PicBoxVolumePicture);
            PnlVolumeControl.Controls.Add(LblVolumeValue);
            PnlVolumeControl.Controls.Add(TbVolume);
            PnlVolumeControl.Location = new Point(237, 100);
            PnlVolumeControl.Name = "PnlVolumeControl";
            PnlVolumeControl.Size = new Size(150, 40);
            PnlVolumeControl.TabIndex = 42;
            PnlVolumeControl.Visible = false;
            PnlVolumeControl.MouseLeave += PnlVolumeControl_MouseLeave;
            // 
            // PicBoxVolumePicture
            // 
            PicBoxVolumePicture.BackColor = Color.Transparent;
            PicBoxVolumePicture.BorderColor = Color.Empty;
            PicBoxVolumePicture.BorderRadius = 6;
            PicBoxVolumePicture.BorderSize = 0;
            PicBoxVolumePicture.HoverBackgroundColor = Color.LightSlateGray;
            PicBoxVolumePicture.HoverBorderColor = Color.Empty;
            PicBoxVolumePicture.Image = Properties.Resources.volume_high;
            PicBoxVolumePicture.ImagePadding = new Padding(6);
            PicBoxVolumePicture.Location = new Point(3, 5);
            PicBoxVolumePicture.Name = "PicBoxVolumePicture";
            PicBoxVolumePicture.Padding = new Padding(6);
            PicBoxVolumePicture.Size = new Size(30, 30);
            PicBoxVolumePicture.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxVolumePicture.TabIndex = 32;
            PicBoxVolumePicture.TabStop = false;
            PicBoxVolumePicture.Click += PicBoxVolumePicture_Click;
            // 
            // LblVolumeValue
            // 
            LblVolumeValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LblVolumeValue.AutoSize = true;
            LblVolumeValue.ForeColor = Color.White;
            LblVolumeValue.Location = new Point(122, 13);
            LblVolumeValue.Name = "LblVolumeValue";
            LblVolumeValue.Size = new Size(25, 15);
            LblVolumeValue.TabIndex = 19;
            LblVolumeValue.Text = "100";
            // 
            // TbVolume
            // 
            TbVolume.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            TbVolume.BorderColor = Color.FromArgb(200, 200, 200);
            TbVolume.DrawValueString = false;
            TbVolume.EmptyBackColor = Color.FromArgb(221, 221, 221);
            TbVolume.FillBackColor = Color.FromArgb(217, 99, 50);
            TbVolume.JumpToMouse = true;
            TbVolume.Location = new Point(39, 9);
            TbVolume.Maximum = 100;
            TbVolume.Minimum = 0;
            TbVolume.MinimumSize = new Size(47, 22);
            TbVolume.Name = "TbVolume";
            TbVolume.Size = new Size(81, 22);
            TbVolume.TabIndex = 18;
            TbVolume.Text = "dungeonTrackBar1";
            TbVolume.ThumbBackColor = Color.FromArgb(244, 244, 244);
            TbVolume.ThumbBorderColor = Color.FromArgb(180, 180, 180);
            TbVolume.Value = 0;
            TbVolume.ValueDivison = Modernial.Controls.DungeonTrackBar.ValueDivisor.By1;
            TbVolume.ValueToSet = 0F;
            TbVolume.ValueChanged += TbVolume_ValueChanged;
            TbVolume.MouseUp += TbVolume_MouseUp;
            // 
            // PicBoxDisplayRightBar
            // 
            PicBoxDisplayRightBar.Image = Properties.Resources.to_left;
            PicBoxDisplayRightBar.Location = new Point(380, 100);
            PicBoxDisplayRightBar.Name = "PicBoxDisplayRightBar";
            PicBoxDisplayRightBar.Size = new Size(20, 20);
            PicBoxDisplayRightBar.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxDisplayRightBar.TabIndex = 0;
            PicBoxDisplayRightBar.TabStop = false;
            PicBoxDisplayRightBar.Click += PicBoxDisplayRightBar_Click;
            // 
            // PicBoxRepeatMusic
            // 
            PicBoxRepeatMusic.BackColor = Color.Transparent;
            PicBoxRepeatMusic.BorderColor = Color.Empty;
            PicBoxRepeatMusic.BorderRadius = 6;
            PicBoxRepeatMusic.BorderSize = 0;
            PicBoxRepeatMusic.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PicBoxRepeatMusic.HoverBorderColor = Color.Empty;
            PicBoxRepeatMusic.Image = Properties.Resources.repeat;
            PicBoxRepeatMusic.ImagePadding = new Padding(6);
            PicBoxRepeatMusic.Location = new Point(333, 147);
            PicBoxRepeatMusic.Name = "PicBoxRepeatMusic";
            PicBoxRepeatMusic.Padding = new Padding(6);
            PicBoxRepeatMusic.Size = new Size(30, 30);
            PicBoxRepeatMusic.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxRepeatMusic.TabIndex = 40;
            PicBoxRepeatMusic.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxRepeatMusic, "Repeat on");
            PicBoxRepeatMusic.Click += PicBoxRepeatMusic_Click;
            // 
            // PicBoxShuffleMusic
            // 
            PicBoxShuffleMusic.BackColor = Color.Transparent;
            PicBoxShuffleMusic.BorderColor = Color.Empty;
            PicBoxShuffleMusic.BorderRadius = 6;
            PicBoxShuffleMusic.BorderSize = 0;
            PicBoxShuffleMusic.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PicBoxShuffleMusic.HoverBorderColor = Color.Empty;
            PicBoxShuffleMusic.Image = Properties.Resources.shuffle_on;
            PicBoxShuffleMusic.ImagePadding = new Padding(6);
            PicBoxShuffleMusic.Location = new Point(38, 147);
            PicBoxShuffleMusic.Name = "PicBoxShuffleMusic";
            PicBoxShuffleMusic.Padding = new Padding(6);
            PicBoxShuffleMusic.Size = new Size(30, 30);
            PicBoxShuffleMusic.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxShuffleMusic.TabIndex = 39;
            PicBoxShuffleMusic.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxShuffleMusic, "Shuffle on");
            PicBoxShuffleMusic.Click += PicBoxShuffleMusic_Click;
            // 
            // LblMusicLength
            // 
            LblMusicLength.BackColor = Color.Transparent;
            LblMusicLength.ForeColor = Color.White;
            LblMusicLength.Location = new Point(317, 114);
            LblMusicLength.Name = "LblMusicLength";
            LblMusicLength.Size = new Size(55, 15);
            LblMusicLength.TabIndex = 38;
            LblMusicLength.Text = "00:00:00";
            LblMusicLength.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LblMusicDurationCtr
            // 
            LblMusicDurationCtr.BackColor = Color.Transparent;
            LblMusicDurationCtr.ForeColor = Color.White;
            LblMusicDurationCtr.Location = new Point(28, 114);
            LblMusicDurationCtr.Name = "LblMusicDurationCtr";
            LblMusicDurationCtr.Size = new Size(55, 15);
            LblMusicDurationCtr.TabIndex = 37;
            LblMusicDurationCtr.Text = "00:00:00";
            LblMusicDurationCtr.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PnlPlayBackInfo
            // 
            PnlPlayBackInfo.BackColor = Color.Transparent;
            PnlPlayBackInfo.BorderColor = Color.Empty;
            PnlPlayBackInfo.BorderRadius = 3;
            PnlPlayBackInfo.BorderSize = 0;
            PnlPlayBackInfo.ContentPadding = new Padding(0);
            PnlPlayBackInfo.Controls.Add(PicBoxShowPlayPicture);
            PnlPlayBackInfo.Controls.Add(PnlMarquee);
            PnlPlayBackInfo.HoverBackgroundColor = Color.Empty;
            PnlPlayBackInfo.HoverBorderColor = Color.Empty;
            PnlPlayBackInfo.Location = new Point(36, 36);
            PnlPlayBackInfo.Name = "PnlPlayBackInfo";
            PnlPlayBackInfo.Size = new Size(328, 65);
            PnlPlayBackInfo.TabIndex = 35;
            // 
            // PicBoxShowPlayPicture
            // 
            PicBoxShowPlayPicture.BackgroundImageLayout = ImageLayout.Stretch;
            PicBoxShowPlayPicture.Image = Properties.Resources.default_music_picture_medium;
            PicBoxShowPlayPicture.Location = new Point(3, 2);
            PicBoxShowPlayPicture.Name = "PicBoxShowPlayPicture";
            PicBoxShowPlayPicture.Padding = new Padding(4);
            PicBoxShowPlayPicture.Size = new Size(60, 60);
            PicBoxShowPlayPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxShowPlayPicture.TabIndex = 0;
            PicBoxShowPlayPicture.TabStop = false;
            // 
            // PnlMarquee
            // 
            PnlMarquee.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PnlMarquee.Controls.Add(LblShowPlayTitle);
            PnlMarquee.Controls.Add(LblShowPlayArtist);
            PnlMarquee.Location = new Point(68, 2);
            PnlMarquee.Name = "PnlMarquee";
            PnlMarquee.Size = new Size(257, 60);
            PnlMarquee.TabIndex = 18;
            // 
            // LblShowPlayTitle
            // 
            LblShowPlayTitle.AutoSize = true;
            LblShowPlayTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblShowPlayTitle.ForeColor = Color.White;
            LblShowPlayTitle.Location = new Point(0, 9);
            LblShowPlayTitle.Name = "LblShowPlayTitle";
            LblShowPlayTitle.Size = new Size(42, 21);
            LblShowPlayTitle.TabIndex = 1;
            LblShowPlayTitle.Text = "Title";
            LblShowPlayTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LblShowPlayArtist
            // 
            LblShowPlayArtist.AutoSize = true;
            LblShowPlayArtist.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblShowPlayArtist.ForeColor = Color.FromArgb(242, 242, 242);
            LblShowPlayArtist.Location = new Point(0, 32);
            LblShowPlayArtist.Name = "LblShowPlayArtist";
            LblShowPlayArtist.Size = new Size(38, 17);
            LblShowPlayArtist.TabIndex = 2;
            LblShowPlayArtist.Text = "Artist";
            LblShowPlayArtist.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // PicBoxSkipForward
            // 
            PicBoxSkipForward.BackColor = Color.Transparent;
            PicBoxSkipForward.BorderColor = Color.Empty;
            PicBoxSkipForward.BorderRadius = 6;
            PicBoxSkipForward.BorderSize = 0;
            PicBoxSkipForward.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PicBoxSkipForward.HoverBorderColor = Color.Empty;
            PicBoxSkipForward.Image = Properties.Resources.skip_forward;
            PicBoxSkipForward.ImagePadding = new Padding(6);
            PicBoxSkipForward.Location = new Point(288, 147);
            PicBoxSkipForward.Name = "PicBoxSkipForward";
            PicBoxSkipForward.Padding = new Padding(6);
            PicBoxSkipForward.Size = new Size(30, 30);
            PicBoxSkipForward.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxSkipForward.TabIndex = 34;
            PicBoxSkipForward.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxSkipForward, "Skip forward (10 sec)");
            PicBoxSkipForward.Click += PicBoxSkipForward_Click;
            // 
            // PicBoxPlayNextMusic
            // 
            PicBoxPlayNextMusic.BackColor = Color.Transparent;
            PicBoxPlayNextMusic.BorderColor = Color.Empty;
            PicBoxPlayNextMusic.BorderRadius = 6;
            PicBoxPlayNextMusic.BorderSize = 0;
            PicBoxPlayNextMusic.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PicBoxPlayNextMusic.HoverBorderColor = Color.Empty;
            PicBoxPlayNextMusic.Image = Properties.Resources.next;
            PicBoxPlayNextMusic.ImagePadding = new Padding(6);
            PicBoxPlayNextMusic.Location = new Point(243, 147);
            PicBoxPlayNextMusic.Name = "PicBoxPlayNextMusic";
            PicBoxPlayNextMusic.Padding = new Padding(6);
            PicBoxPlayNextMusic.Size = new Size(30, 30);
            PicBoxPlayNextMusic.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxPlayNextMusic.TabIndex = 32;
            PicBoxPlayNextMusic.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxPlayNextMusic, "Next");
            PicBoxPlayNextMusic.Click += PicBoxPlayNextMusic_Click;
            // 
            // PicBoxPlayPreviousMusic
            // 
            PicBoxPlayPreviousMusic.BackColor = Color.Transparent;
            PicBoxPlayPreviousMusic.BorderColor = Color.Empty;
            PicBoxPlayPreviousMusic.BorderRadius = 6;
            PicBoxPlayPreviousMusic.BorderSize = 0;
            PicBoxPlayPreviousMusic.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PicBoxPlayPreviousMusic.HoverBorderColor = Color.Empty;
            PicBoxPlayPreviousMusic.Image = Properties.Resources.previous;
            PicBoxPlayPreviousMusic.ImagePadding = new Padding(6);
            PicBoxPlayPreviousMusic.Location = new Point(129, 147);
            PicBoxPlayPreviousMusic.Name = "PicBoxPlayPreviousMusic";
            PicBoxPlayPreviousMusic.Padding = new Padding(6);
            PicBoxPlayPreviousMusic.Size = new Size(30, 30);
            PicBoxPlayPreviousMusic.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxPlayPreviousMusic.TabIndex = 31;
            PicBoxPlayPreviousMusic.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxPlayPreviousMusic, "Previous");
            PicBoxPlayPreviousMusic.Click += PicBoxPlayPreviousMusic_Click;
            // 
            // PicBoxSkipBackward
            // 
            PicBoxSkipBackward.BackColor = Color.Transparent;
            PicBoxSkipBackward.BorderColor = Color.Empty;
            PicBoxSkipBackward.BorderRadius = 6;
            PicBoxSkipBackward.BorderSize = 0;
            PicBoxSkipBackward.HoverBackgroundColor = Color.FromArgb(89, 89, 89);
            PicBoxSkipBackward.HoverBorderColor = Color.Empty;
            PicBoxSkipBackward.Image = Properties.Resources.skip_backward;
            PicBoxSkipBackward.ImagePadding = new Padding(6);
            PicBoxSkipBackward.Location = new Point(83, 147);
            PicBoxSkipBackward.Name = "PicBoxSkipBackward";
            PicBoxSkipBackward.Padding = new Padding(6);
            PicBoxSkipBackward.Size = new Size(30, 30);
            PicBoxSkipBackward.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBoxSkipBackward.TabIndex = 30;
            PicBoxSkipBackward.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxSkipBackward, "Skip back (10 sec)");
            PicBoxSkipBackward.Click += PicBoxSkipBackward_Click;
            // 
            // PicBoxPlayAndPause
            // 
            PicBoxPlayAndPause.BackColor = Color.FromArgb(36, 176, 191);
            PicBoxPlayAndPause.BackgroundImageLayout = ImageLayout.Stretch;
            PicBoxPlayAndPause.Image = Properties.Resources.pause;
            PicBoxPlayAndPause.Location = new Point(173, 137);
            PicBoxPlayAndPause.Name = "PicBoxPlayAndPause";
            PicBoxPlayAndPause.Padding = new Padding(3, 0, 0, 0);
            PicBoxPlayAndPause.Size = new Size(50, 50);
            PicBoxPlayAndPause.SizeMode = PictureBoxSizeMode.CenterImage;
            PicBoxPlayAndPause.TabIndex = 28;
            PicBoxPlayAndPause.TabStop = false;
            toolTipPlayerControl.SetToolTip(PicBoxPlayAndPause, "Pause");
            PicBoxPlayAndPause.Click += PicBoxPlayAndPause_Click;
            // 
            // TbSeekMusic
            // 
            TbSeekMusic.BorderColor = Color.FromArgb(200, 200, 200);
            TbSeekMusic.DrawValueString = false;
            TbSeekMusic.EmptyBackColor = Color.FromArgb(221, 221, 221);
            TbSeekMusic.FillBackColor = Color.FromArgb(36, 176, 191);
            TbSeekMusic.JumpToMouse = true;
            TbSeekMusic.Location = new Point(85, 110);
            TbSeekMusic.Maximum = 10;
            TbSeekMusic.Minimum = 0;
            TbSeekMusic.MinimumSize = new Size(47, 22);
            TbSeekMusic.Name = "TbSeekMusic";
            TbSeekMusic.Size = new Size(230, 22);
            TbSeekMusic.TabIndex = 7;
            TbSeekMusic.Text = "dungeonTrackBar1";
            TbSeekMusic.ThumbBackColor = Color.FromArgb(36, 176, 191);
            TbSeekMusic.ThumbBorderColor = Color.White;
            TbSeekMusic.Value = 0;
            TbSeekMusic.ValueDivison = Modernial.Controls.DungeonTrackBar.ValueDivisor.By1;
            TbSeekMusic.ValueToSet = 0F;
            TbSeekMusic.ValueChanged += TbSeekMusic_ValueChanged;
            TbSeekMusic.MouseDown += TbSeekMusic_MouseDown;
            TbSeekMusic.MouseUp += TbSeekMusic_MouseUp;
            // 
            // TimerMusicDuration
            // 
            TimerMusicDuration.Tick += TimerMusicDuration_Tick;
            // 
            // TimerTitleMarquee
            // 
            TimerTitleMarquee.Tick += TimerTitleMarquee_Tick;
            // 
            // TimerArtistMarquee
            // 
            TimerArtistMarquee.Tick += TimerArtistMarquee_Tick;
            // 
            // TimerShowRightBar
            // 
            TimerShowRightBar.Tick += TimerShowRightBar_Tick;
            // 
            // TimerHideRightBar
            // 
            TimerHideRightBar.Tick += TimerHideRightBar_Tick;
            // 
            // MiniPlayer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 200);
            Controls.Add(PnlMain);
            FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(1366, 720);
            MinimumSize = new Size(190, 40);
            Name = "MiniPlayer";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "themeForm1";
            Load += MiniPlayer_Load;
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            PnlMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlRightBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PnlHideRightBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxMiniPlayerToggle).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxShowVolumeBar).EndInit();
            PnlVolumeControl.ResumeLayout(false);
            PnlVolumeControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxVolumePicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxDisplayRightBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxRepeatMusic).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxShuffleMusic).EndInit();
            PnlPlayBackInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PicBoxShowPlayPicture).EndInit();
            PnlMarquee.ResumeLayout(false);
            PnlMarquee.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PicBoxSkipForward).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayNextMusic).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayPreviousMusic).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxSkipBackward).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicBoxPlayAndPause).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private FileSystemWatcher fileSystemWatcher1;
        private Panel PnlMain;
        private Modernial.Controls.DungeonTrackBar TbSeekMusic;
        private CustomControls.NielarkPictureBox PicBoxSkipForward;
        private CustomControls.NielarkPictureBox PicBoxPlayNextMusic;
        private CustomControls.NielarkPictureBox PicBoxPlayPreviousMusic;
        private CustomControls.NielarkPictureBox PicBoxSkipBackward;
        private PictureBox PicBoxPlayAndPause;
        private CustomControls.NielarkPanel PnlPlayBackInfo;
        private PictureBox PicBoxShowPlayPicture;
        private Panel PnlMarquee;
        private Label LblShowPlayTitle;
        private Label LblShowPlayArtist;
        private ToolTip toolTipPlayerControl;
        private CustomControls.NielarkPictureBox PicBoxMiniPlayerToggle;
        private System.Windows.Forms.Timer TimerMusicDuration;
        private Label LblMusicDurationCtr;
        private Label LblMusicLength;
        private CustomControls.NielarkPictureBox PicBoxRepeatMusic;
        private CustomControls.NielarkPictureBox PicBoxShuffleMusic;
        private CustomControls.NielarkPictureBox PicBoxShowVolumeBar;
        private Panel PnlVolumeControl;
        private CustomControls.NielarkPictureBox PicBoxVolumePicture;
        private Label LblVolumeValue;
        private Modernial.Controls.DungeonTrackBar TbVolume;
        private System.Windows.Forms.Timer TimerTitleMarquee;
        private System.Windows.Forms.Timer TimerArtistMarquee;
        private Panel pnlHeader;
        private Panel pnlRightBar;
        private System.Windows.Forms.Timer TimerShowRightBar;
        private CustomControls.NielarkPictureBox PnlHideRightBar;
        private System.Windows.Forms.Timer TimerHideRightBar;
        private PictureBox PicBoxDisplayRightBar;
        private Label label3;
    }
}