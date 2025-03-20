namespace MusicPlayer
{
    partial class MusicProperties
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
            pnlMain = new Panel();
            pnlProperties = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblTitle = new Label();
            lblFileLocationValue = new Label();
            lblTitleValue = new Label();
            lblFileLocation = new Label();
            lblLength = new Label();
            lblItemTypeValue = new Label();
            lblContributingArtist = new Label();
            lblBitRateValue = new Label();
            lblLengthValue = new Label();
            lblAlbumTitleValue = new Label();
            lblYearValue = new Label();
            lblArtistValue = new Label();
            lblAlbumTitle = new Label();
            lblYear = new Label();
            lblGenre = new Label();
            lblGenreValue = new Label();
            lblBitRate = new Label();
            lblItemType = new Label();
            lblProperties = new Label();
            btnOpenFileLocation = new MusicPlayer.CustomControls.NielarkButton();
            btnBack = new MusicPlayer.CustomControls.NielarkButton();
            pnlMain.SuspendLayout();
            pnlProperties.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(48, 49, 52);
            pnlMain.Controls.Add(pnlProperties);
            pnlMain.Controls.Add(btnOpenFileLocation);
            pnlMain.Controls.Add(btnBack);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(2, 2);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(445, 445);
            pnlMain.TabIndex = 0;
            // 
            // pnlProperties
            // 
            pnlProperties.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlProperties.Controls.Add(tableLayoutPanel1);
            pnlProperties.Controls.Add(lblProperties);
            pnlProperties.Location = new Point(0, 0);
            pnlProperties.Name = "pnlProperties";
            pnlProperties.Size = new Size(445, 360);
            pnlProperties.TabIndex = 40;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblTitle, 0, 0);
            tableLayoutPanel1.Controls.Add(lblTitleValue, 0, 1);
            tableLayoutPanel1.Controls.Add(lblContributingArtist, 0, 3);
            tableLayoutPanel1.Controls.Add(lblArtistValue, 0, 4);
            tableLayoutPanel1.Controls.Add(lblAlbumTitle, 0, 6);
            tableLayoutPanel1.Controls.Add(lblAlbumTitleValue, 0, 7);
            tableLayoutPanel1.Controls.Add(lblYear, 0, 9);
            tableLayoutPanel1.Controls.Add(lblYearValue, 0, 10);
            tableLayoutPanel1.Controls.Add(lblGenre, 0, 12);
            tableLayoutPanel1.Controls.Add(lblGenreValue, 0, 13);
            tableLayoutPanel1.Controls.Add(lblLength, 0, 15);
            tableLayoutPanel1.Controls.Add(lblLengthValue, 0, 16);
            tableLayoutPanel1.Controls.Add(lblBitRate, 0, 18);
            tableLayoutPanel1.Controls.Add(lblBitRateValue, 0, 19);
            tableLayoutPanel1.Controls.Add(lblItemType, 0, 21);
            tableLayoutPanel1.Controls.Add(lblItemTypeValue, 0, 22);
            tableLayoutPanel1.Controls.Add(lblFileLocation, 0, 24);
            tableLayoutPanel1.Controls.Add(lblFileLocationValue, 0, 25);
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel1.Location = new Point(35, 77);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 26;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.Size = new Size(410, 280);
            tableLayoutPanel1.TabIndex = 58;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Left;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = SystemColors.ScrollBar;
            lblTitle.Location = new Point(3, 1);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(35, 17);
            lblTitle.TabIndex = 40;
            lblTitle.Text = "Title:";
            // 
            // lblFileLocationValue
            // 
            lblFileLocationValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblFileLocationValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblFileLocationValue.ForeColor = Color.White;
            lblFileLocationValue.Location = new Point(3, 460);
            lblFileLocationValue.Name = "lblFileLocationValue";
            lblFileLocationValue.Size = new Size(404, 60);
            lblFileLocationValue.TabIndex = 57;
            // 
            // lblTitleValue
            // 
            lblTitleValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTitleValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblTitleValue.ForeColor = Color.White;
            lblTitleValue.Location = new Point(3, 20);
            lblTitleValue.Name = "lblTitleValue";
            lblTitleValue.Size = new Size(404, 40);
            lblTitleValue.TabIndex = 49;
            // 
            // lblFileLocation
            // 
            lblFileLocation.Anchor = AnchorStyles.Left;
            lblFileLocation.AutoSize = true;
            lblFileLocation.Font = new Font("Segoe UI", 9.75F);
            lblFileLocation.ForeColor = SystemColors.ScrollBar;
            lblFileLocation.Location = new Point(3, 441);
            lblFileLocation.Name = "lblFileLocation";
            lblFileLocation.Size = new Size(83, 17);
            lblFileLocation.TabIndex = 48;
            lblFileLocation.Text = "File Location:";
            // 
            // lblLength
            // 
            lblLength.Anchor = AnchorStyles.Left;
            lblLength.AutoSize = true;
            lblLength.Font = new Font("Segoe UI", 9.75F);
            lblLength.ForeColor = SystemColors.ScrollBar;
            lblLength.Location = new Point(3, 291);
            lblLength.Name = "lblLength";
            lblLength.Size = new Size(50, 17);
            lblLength.TabIndex = 43;
            lblLength.Text = "Length:";
            // 
            // lblItemTypeValue
            // 
            lblItemTypeValue.Anchor = AnchorStyles.Left;
            lblItemTypeValue.AutoSize = true;
            lblItemTypeValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblItemTypeValue.ForeColor = Color.White;
            lblItemTypeValue.Location = new Point(3, 411);
            lblItemTypeValue.Name = "lblItemTypeValue";
            lblItemTypeValue.Size = new Size(38, 17);
            lblItemTypeValue.TabIndex = 56;
            lblItemTypeValue.Text = ".mp3";
            // 
            // lblContributingArtist
            // 
            lblContributingArtist.Anchor = AnchorStyles.Left;
            lblContributingArtist.AutoSize = true;
            lblContributingArtist.Font = new Font("Segoe UI", 9.75F);
            lblContributingArtist.ForeColor = SystemColors.ScrollBar;
            lblContributingArtist.Location = new Point(3, 71);
            lblContributingArtist.Name = "lblContributingArtist";
            lblContributingArtist.Size = new Size(117, 17);
            lblContributingArtist.TabIndex = 42;
            lblContributingArtist.Text = "Contributing Artist:";
            // 
            // lblBitRateValue
            // 
            lblBitRateValue.Anchor = AnchorStyles.Left;
            lblBitRateValue.AutoSize = true;
            lblBitRateValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblBitRateValue.ForeColor = Color.White;
            lblBitRateValue.Location = new Point(3, 361);
            lblBitRateValue.Name = "lblBitRateValue";
            lblBitRateValue.Size = new Size(0, 17);
            lblBitRateValue.TabIndex = 55;
            // 
            // lblLengthValue
            // 
            lblLengthValue.Anchor = AnchorStyles.Left;
            lblLengthValue.AutoSize = true;
            lblLengthValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblLengthValue.ForeColor = Color.White;
            lblLengthValue.Location = new Point(3, 311);
            lblLengthValue.Name = "lblLengthValue";
            lblLengthValue.Size = new Size(0, 17);
            lblLengthValue.TabIndex = 54;
            // 
            // lblAlbumTitleValue
            // 
            lblAlbumTitleValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblAlbumTitleValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblAlbumTitleValue.ForeColor = Color.White;
            lblAlbumTitleValue.Location = new Point(3, 140);
            lblAlbumTitleValue.Name = "lblAlbumTitleValue";
            lblAlbumTitleValue.Size = new Size(404, 40);
            lblAlbumTitleValue.TabIndex = 51;
            // 
            // lblYearValue
            // 
            lblYearValue.Anchor = AnchorStyles.Left;
            lblYearValue.AutoSize = true;
            lblYearValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblYearValue.ForeColor = Color.White;
            lblYearValue.Location = new Point(3, 211);
            lblYearValue.Name = "lblYearValue";
            lblYearValue.Size = new Size(0, 17);
            lblYearValue.TabIndex = 52;
            // 
            // lblArtistValue
            // 
            lblArtistValue.Anchor = AnchorStyles.Left;
            lblArtistValue.AutoSize = true;
            lblArtistValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblArtistValue.ForeColor = Color.White;
            lblArtistValue.Location = new Point(3, 91);
            lblArtistValue.Name = "lblArtistValue";
            lblArtistValue.Size = new Size(0, 17);
            lblArtistValue.TabIndex = 50;
            // 
            // lblAlbumTitle
            // 
            lblAlbumTitle.Anchor = AnchorStyles.Left;
            lblAlbumTitle.AutoSize = true;
            lblAlbumTitle.Font = new Font("Segoe UI", 9.75F);
            lblAlbumTitle.ForeColor = SystemColors.ScrollBar;
            lblAlbumTitle.Location = new Point(3, 121);
            lblAlbumTitle.Name = "lblAlbumTitle";
            lblAlbumTitle.Size = new Size(76, 17);
            lblAlbumTitle.TabIndex = 41;
            lblAlbumTitle.Text = "Album Title:";
            // 
            // lblYear
            // 
            lblYear.Anchor = AnchorStyles.Left;
            lblYear.AutoSize = true;
            lblYear.Font = new Font("Segoe UI", 9.75F);
            lblYear.ForeColor = SystemColors.ScrollBar;
            lblYear.Location = new Point(3, 191);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(36, 17);
            lblYear.TabIndex = 44;
            lblYear.Text = "Year:";
            // 
            // lblGenre
            // 
            lblGenre.Anchor = AnchorStyles.Left;
            lblGenre.AutoSize = true;
            lblGenre.Font = new Font("Segoe UI", 9.75F);
            lblGenre.ForeColor = SystemColors.ScrollBar;
            lblGenre.Location = new Point(3, 241);
            lblGenre.Name = "lblGenre";
            lblGenre.Size = new Size(46, 17);
            lblGenre.TabIndex = 45;
            lblGenre.Text = "Genre:";
            // 
            // lblGenreValue
            // 
            lblGenreValue.Anchor = AnchorStyles.Left;
            lblGenreValue.AutoSize = true;
            lblGenreValue.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblGenreValue.ForeColor = Color.White;
            lblGenreValue.Location = new Point(3, 261);
            lblGenreValue.Name = "lblGenreValue";
            lblGenreValue.Size = new Size(0, 17);
            lblGenreValue.TabIndex = 53;
            // 
            // lblBitRate
            // 
            lblBitRate.Anchor = AnchorStyles.Left;
            lblBitRate.AutoSize = true;
            lblBitRate.Font = new Font("Segoe UI", 9.75F);
            lblBitRate.ForeColor = SystemColors.ScrollBar;
            lblBitRate.Location = new Point(3, 341);
            lblBitRate.Name = "lblBitRate";
            lblBitRate.Size = new Size(55, 17);
            lblBitRate.TabIndex = 46;
            lblBitRate.Text = "Bit Rate:";
            // 
            // lblItemType
            // 
            lblItemType.Anchor = AnchorStyles.Left;
            lblItemType.AutoSize = true;
            lblItemType.Font = new Font("Segoe UI", 9.75F);
            lblItemType.ForeColor = SystemColors.ScrollBar;
            lblItemType.Location = new Point(3, 391);
            lblItemType.Name = "lblItemType";
            lblItemType.Size = new Size(67, 17);
            lblItemType.TabIndex = 47;
            lblItemType.Text = "Item Type:";
            // 
            // lblProperties
            // 
            lblProperties.AutoSize = true;
            lblProperties.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProperties.ForeColor = Color.White;
            lblProperties.Location = new Point(35, 35);
            lblProperties.Name = "lblProperties";
            lblProperties.Size = new Size(114, 30);
            lblProperties.TabIndex = 39;
            lblProperties.Text = "Properties";
            // 
            // btnOpenFileLocation
            // 
            btnOpenFileLocation.Anchor = AnchorStyles.Bottom;
            btnOpenFileLocation.BackColor = Color.FromArgb(36, 176, 191);
            btnOpenFileLocation.BackgroundColor = Color.FromArgb(36, 176, 191);
            btnOpenFileLocation.BackgroundImageLayout = ImageLayout.Center;
            btnOpenFileLocation.BorderColor = Color.Empty;
            btnOpenFileLocation.BorderRadius = 6;
            btnOpenFileLocation.BorderSize = 0;
            btnOpenFileLocation.ClickedBackgroundColor = Color.FromArgb(36, 176, 191);
            btnOpenFileLocation.FlatAppearance.BorderSize = 0;
            btnOpenFileLocation.FlatAppearance.MouseDownBackColor = Color.FromArgb(36, 176, 191);
            btnOpenFileLocation.FlatAppearance.MouseOverBackColor = Color.FromArgb(36, 176, 191);
            btnOpenFileLocation.FlatStyle = FlatStyle.Flat;
            btnOpenFileLocation.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnOpenFileLocation.ForeColor = Color.FromArgb(8, 18, 38);
            btnOpenFileLocation.HoverBackgroundColor = Color.Empty;
            btnOpenFileLocation.HoverBorderColor = Color.Empty;
            btnOpenFileLocation.HoverTextColor = Color.White;
            btnOpenFileLocation.Location = new Point(35, 375);
            btnOpenFileLocation.Name = "btnOpenFileLocation";
            btnOpenFileLocation.Size = new Size(150, 35);
            btnOpenFileLocation.TabIndex = 39;
            btnOpenFileLocation.Text = "Open file location";
            btnOpenFileLocation.TextColor = Color.FromArgb(8, 18, 38);
            btnOpenFileLocation.UseVisualStyleBackColor = false;
            btnOpenFileLocation.Click += BtnOpenFileLocation_Click;
            // 
            // btnBack
            // 
            btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnBack.BackColor = Color.FromArgb(36, 176, 191);
            btnBack.BackgroundColor = Color.FromArgb(36, 176, 191);
            btnBack.BackgroundImageLayout = ImageLayout.Center;
            btnBack.BorderColor = Color.Empty;
            btnBack.BorderRadius = 6;
            btnBack.BorderSize = 0;
            btnBack.ClickedBackgroundColor = Color.FromArgb(36, 176, 191);
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatAppearance.MouseDownBackColor = Color.FromArgb(36, 176, 191);
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(36, 176, 191);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBack.ForeColor = Color.FromArgb(8, 18, 38);
            btnBack.HoverBackgroundColor = Color.Empty;
            btnBack.HoverBorderColor = Color.Empty;
            btnBack.HoverTextColor = Color.White;
            btnBack.Location = new Point(257, 375);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(150, 35);
            btnBack.TabIndex = 19;
            btnBack.Text = "Close";
            btnBack.TextColor = Color.FromArgb(8, 18, 38);
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += BtnBack_Click;
            // 
            // MusicProperties
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(36, 176, 191);
            ClientSize = new Size(450, 450);
            Controls.Add(pnlMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MusicProperties";
            Padding = new Padding(2, 2, 3, 3);
            StartPosition = FormStartPosition.Manual;
            Text = "Properties";
            Load += MusicProperties_Load;
            pnlMain.ResumeLayout(false);
            pnlProperties.ResumeLayout(false);
            pnlProperties.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMain;
        private CustomControls.NielarkButton btnBack;
        private CustomControls.NielarkButton btnOpenFileLocation;
        private Panel pnlProperties;
        private Label lblFileLocationValue;
        private Label lblItemTypeValue;
        private Label lblBitRateValue;
        private Label lblLengthValue;
        private Label lblGenreValue;
        private Label lblYearValue;
        private Label lblAlbumTitleValue;
        private Label lblArtistValue;
        private Label lblTitleValue;
        private Label lblFileLocation;
        private Label lblItemType;
        private Label lblBitRate;
        private Label lblGenre;
        private Label lblYear;
        private Label lblLength;
        private Label lblContributingArtist;
        private Label lblAlbumTitle;
        private Label lblTitle;
        private Label lblProperties;
        private TableLayoutPanel tableLayoutPanel1;
    }
}