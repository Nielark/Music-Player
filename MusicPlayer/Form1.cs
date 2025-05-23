using Modernial.Controls;
using MusicPlayer.CustomControls;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Formats.Tar;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;
using TagLib;
using static MusicPlayer.MyModule;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Panel = System.Windows.Forms.Panel;

namespace MusicPlayer
{
    public partial class FormMain : Form
    {
        private int targetTopPosition;
        public bool fullScreen = false;
        private System.Windows.Forms.Button lastClickedButton; // Track the last clicked button

        MusicLibrary musicLibrary = new MusicLibrary();
        public List<Music> musicList;
        private List<Music> tempMusicList;

        private PlayerControls playerControls;
        private MusicListViewManager musicListViewManager;
        private MusicSearchFilterSorter musicSearchFilterSorter;

        private Size formSize; //Keep form size when it is minimized and restored.Since the form is resized because it takes into account the size of the title bar and borders.
        //private int borderSize = 2;
        private int previousWidth;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public FormMain()
        {
            InitializeComponent();

            // Set form style for smooth redrawing
            this.SetStyle(ControlStyles.ResizeRedraw, true); // this is to avoid visual artifacts
            //this.Padding = new Padding(2, 2, 3, 3);
            ApplyRoundedCorners();

            // Configure initial appearance for controls
            btnHome.BackColor = Color.FromArgb(47, 53, 64);
            btnHome.ForeColor = Color.White;
            MyModule.MakeCircular(PicBoxPlayAndPause);      // Make the play or pause button circular
            previousWidth = this.ClientSize.Width;
            TbVolume.Value = 100;   // Set 100 as the default value for volume

            // Instantiate PlayerControls and assign UI controllers
            playerControls = new PlayerControls();
            var uiControllers = new PlayerUIControllers
            {
                PicBoxShowPlayPicture = this.PicBoxShowPlayPicture,
                LblShowPlayTitle = this.LblShowPlayTitle,
                LblShowPlayArtist = this.LblShowPlayArtist,
                LblMusicLength = this.LblMusicLength,
                LblMusicDurationCtr = this.LblMusicDurationCtr,
                TbSeekMusic = this.TbSeekMusic,
                TbVolume = this.TbVolume,
                TimerMusicDuration = this.TimerMusicDuration,
                PnlMarquee = this.PnlMarquee,
                TimerArtistMarquee = this.TimerArtistMarquee,
                TimerTitleMarquee = this.TimerTitleMarquee
            };
            playerControls.SetPlayerUIControllers(uiControllers);

            // Instantiate controllers and assign UI controllers
            musicListViewManager = new MusicListViewManager(playerControls);
            var artistAndAlbumUIControllers = new ArtistAndAlbumUIControllers
            {
                PnlInfo = this.pnlInfo,
                PnlHeaderControl = this.PnlHeaderControl,
                PnlMusicLibrary = this.pnlMusicLibrary,
                PnlPlayMusicQueue = this.PnlPlayMusicQueue,
                DgvPlayMusicQueue = this.DgvPlayMusicQueue,
                TimerShowPnlPlayerControls = this.TimerShowPnlPlayerControls,

                PicBoxAlbumImage = this.picBoxAlbumImage,
                LblAlbumName = this.lblAlbumName,
                LblAlbumArtist = this.lblAlbumArtist,
                LblAlbumNumbers = this.lblAlbumNumbers,

                ToolTipPlayerControl = this.toolTipPlayerControl,

                PnlArtist = this.pnlArtists,
                PnlAlbum = this.pnlAlbum,
                PnlAlbumTrack = this.pnlAlbumTrack,
                DgvAlbumTracks = this.dgvAlbumTracks,
                PnlArtistTrack = this.pnlArtistTrack,
                DgvArtistTracks = this.dgvArtistTracks
            };
            musicListViewManager.SetArtistAndAlbumUIControllers(artistAndAlbumUIControllers);

            // Wire up events that don't require dynamic data
            playerControls.ShuffleMusicStateChanged += OnShuffleMusicStateChanged;
            playerControls.PlayAndPauseStateChanged += OnPlayAndPauseStateChanged;
            playerControls.RepeatMusicStateChanged += OnRepeatMusicStateChanged;
            playerControls.MuteStateChanged += OnMuteStateChanged;
            playerControls.UpdateMuteStateChanged += OnUpdateMuteStateChanged;
            playerControls.UpdateVolumeValueChanged += OnUpdateVolumeValueChanged;

            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //playerControls.playMusicQueue = LoadMusicQueue();
            //if (playerControls.playMusicQueue.Count > 0)
            //{
            //    // Resume the last played song
            //    playerControls.PlayMusic(playerControls.playMusicQueue[0].MusicPictureMedium, playerControls.playMusicQueue[0].File, playerControls.playMusicQueue[0].Title, playerControls.playMusicQueue[0].Artist);
            //    DgvPlayMusicQueue.DataSource = playerControls.playMusicQueue;
            //    TimerShowPnlPlayerControls.Start();
            //}

            formSize = this.ClientSize;

            // Load music files (this populates musicLibrary.musicList)
            musicLibrary.LoadMusicFiles(DgvMusicList);

            // Copy the loaded music to your local lists
            musicList = new List<Music>(musicLibrary.musicList);
            tempMusicList = new List<Music>(musicList);

            // Initialize view managers with the loaded music list
            MusicPlayerBase musicPlayerBase = new MusicPlayerBase();
            musicListViewManager.SetMusicList(musicLibrary.musicList);
            musicSearchFilterSorter = new MusicSearchFilterSorter(musicLibrary.musicList, playerControls, musicListViewManager);
            musicSearchFilterSorter.temporaryMusicList = new List<Music>(musicLibrary.musicList);

            // Display artist and album information
            var artistInfo = musicListViewManager.GetMusicInfo(a => new ArtistInfo(MusicListViewManager.CapitalizeWords(a.Artist), a.MusicPictureMedium));
            musicListViewManager.DisplayInfo(flowLayoutPanel1, true, artistInfo);

            var albumInfo = musicListViewManager.GetMusicInfo(a => new AlbumInfo(MusicListViewManager.CapitalizeWords(a.Album), a.Artist, a.MusicPictureMedium, a.Duration));
            musicListViewManager.DisplayInfo(flowLayoutPanel2, false, albumInfo);

            // for artist
            //CbSortMusic.Items.Clear();
            //CbSortMusic.Items.AddRange(new string[] { "A - Z", "Z - A" });

            // for album
            //CbSortMusic.Items.Clear();
            //CbSortMusic.Items.AddRange(new string[] { "A - Z", "Z - A", "Artist (A - Z)", "Artist (Z - A)" });

            // Set the ComboBox data source for filtering by artist
            CbFilterArtist.DataSource = musicList
                                        .Select(s => s.Artist)          // Select the Artist property from each item in the music list
                                        .Distinct()                     // Remove duplicates artist names
                                        .OrderBy(Artist => Artist)      // Sort the artis names alphabetically 
                                        .Prepend("Display All")         // Add "Display All" option at the beginning of the list
                                        .ToList();                      // Convert the result to a List and set it as the data source

            // Set the ComboBox data source for filtering by album
            CbFilterAlbum.DataSource = musicList
                                        .Select(s => s.Album)           // Select the Album property from each item in the musicList
                                        .Distinct()                     // Remove duplicate album names
                                        .OrderBy(Album => Album)        // Sort the album names alphabetically
                                        .Prepend("Display All")         // Add "Display All" option at the beginning of the list
                                        .ToList();                      // Convert the result to a List and set it as the data source

            // Set default text for the ComboBoxes
            CbFilterArtist.Text = "Filter by Artist";
            CbFilterAlbum.Text = "Filter by Album";

            // Set up AutoComplete for the search textbox
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();   // Assuming `musicList` is a List<MyMusic> with Title and Artist 
            autoCompleteCollection.AddRange(musicList.Select(x => x.Title).ToArray());  // Add Titles  to the AutoCompleteStringCollection
            autoCompleteCollection.AddRange(musicList.Select(x => x.Artist).ToArray()); // Add Artists to the AutoCompleteStringCollection

            // Configure the TextBox for auto-completion
            TxtSearch.AutoCompleteCustomSource = autoCompleteCollection;
            TxtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void PnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void BtnImportMusic_Click(object sender, EventArgs e)
        {
            CbFilterAlbum.SelectedIndex = 0;
            CbFilterArtist.SelectedIndex = 0;

            // Display text in the combo box
            CbFilterArtist.Text = "Filter by Artist";
            CbFilterAlbum.Text = "Filter by Album";

            musicLibrary.ImportMusicFiles(ofdMusic, DgvMusicList);
            musicList = new List<Music>(musicLibrary.musicList);
            tempMusicList = new List<Music>(musicList);   // Copy the music list from musicList to tempMusicList
            musicListViewManager = new MusicListViewManager(playerControls);
            musicListViewManager.SetMusicList(musicLibrary.musicList);

            musicSearchFilterSorter = new MusicSearchFilterSorter(musicLibrary.musicList, playerControls, musicListViewManager);
            musicSearchFilterSorter.temporaryMusicList = new List<Music>(musicLibrary.musicList);
            musicSearchFilterSorter.ModifyDataGridView(DgvMusicList, PnlMusicList);
        }

        private void CbSortMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!musicSearchFilterSorter.IsAllMusicList)
            {
                if (musicSearchFilterSorter.IsArtistList)
                {
                    var artistInfo = musicSearchFilterSorter.SortArtistList(CbSortMusic);
                    flowLayoutPanel1.Controls.Clear();
                    musicListViewManager.DisplayInfo(flowLayoutPanel1, true, artistInfo);
                }
                else
                {
                    var albumInfo = musicSearchFilterSorter.SortAlbumList(CbSortMusic);
                    flowLayoutPanel2.Controls.Clear();
                    musicListViewManager.DisplayInfo(flowLayoutPanel2, false, albumInfo);
                }
            }
            else
            {
                musicSearchFilterSorter.SortMusicList(CbSortMusic, TxtSearch, DgvMusicList);
                musicSearchFilterSorter.ModifyDataGridView(DgvMusicList, PnlMusicList);
            }

            TxtSearch.Text = string.Empty;          // Clear the search box
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!musicSearchFilterSorter.IsAllMusicList)
            {
                if (musicSearchFilterSorter.IsArtistList)
                {
                    var artistInfo = musicSearchFilterSorter.SearchArtistList(TxtSearch);
                    flowLayoutPanel1.Controls.Clear();
                    musicListViewManager.DisplayInfo(flowLayoutPanel1, true, artistInfo);
                }
                else
                {
                    var albumInfo = musicSearchFilterSorter.SearchAlbumList(TxtSearch);
                    flowLayoutPanel2.Controls.Clear();
                    musicListViewManager.DisplayInfo(flowLayoutPanel2, false, albumInfo);
                }
            }
            else
            {
                musicSearchFilterSorter.Search(TxtSearch, CbFilterAlbum, CbFilterArtist, DgvMusicList);
                musicSearchFilterSorter.ModifyDataGridView(DgvMusicList, PnlMusicList);
            }
        }

        private void CbFilterArtist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!musicSearchFilterSorter.IsAllMusicList && !musicSearchFilterSorter.IsArtistList)
            {
                var albumInfo = musicSearchFilterSorter.FilterAlbum(CbFilterArtist);
                flowLayoutPanel2.Controls.Clear();
                musicListViewManager.DisplayInfo(flowLayoutPanel2, false, albumInfo);
            }
            else
            {
                musicSearchFilterSorter.FilterByArtistOrAlbumHandler(CbFilterArtist, CbFilterAlbum, TxtSearch, DgvMusicList, s => s.Artist);
                musicSearchFilterSorter.ModifyDataGridView(DgvMusicList, PnlMusicList);
            }

            TxtSearch.Text = string.Empty;
        }

        private void CbFilterAlbum_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (musicSearchFilterSorter.IsAllMusicList)
            {
                musicSearchFilterSorter.FilterByArtistOrAlbumHandler(CbFilterAlbum, CbFilterArtist, TxtSearch, DgvMusicList, s => s.Album);
                musicSearchFilterSorter.ModifyDataGridView(DgvMusicList, PnlMusicList);
            }

            TxtSearch.Text = string.Empty;
        }

        private void BtnShuffleAndPlay_Click(object sender, EventArgs e)
        {
            playerControls.ShuffleAndPlayMusic(DgvPlayMusicQueue, TimerShowPnlPlayerControls, musicLibrary);
            //playerControls.ShuffleMusicHandler(DgvPlayMusicQueue);
        }

        private void BtnClearQueue_Click(object sender, EventArgs e)
        {
            if (playerControls.playMusicQueue.Count <= 0)
                return;

            var audioFileReader = playerControls.audioFileReader;
            var waveOutDevice = playerControls.waveOutDevice;

            TimerMusicDuration.Stop();

            if (audioFileReader != null && waveOutDevice != null)
            {
                musicSearchFilterSorter.CleanPlaybackResources();
                musicSearchFilterSorter.ResetSeekBarAndDurationLabels(TbSeekMusic, LblMusicLength, LblMusicDurationCtr);
                musicSearchFilterSorter.ClearCurrentMusicInformation(LblShowPlayTitle, LblShowPlayArtist, PicBoxShowPlayPicture);

                playerControls.ResetMarqueeLabels(LblShowPlayTitle, LblShowPlayArtist, TimerTitleMarquee, TimerArtistMarquee);
            }

            musicSearchFilterSorter.ClearDataGridView(DgvPlayMusicQueue);
            musicSearchFilterSorter.ModifyDataGridView(DgvPlayMusicQueue, PnlPlayMusicQueue);
        }

        // SIDE PANEL BUTTONS

        private void BtnHome_Click(object sender, EventArgs e)
        {
            UpdateSideBarButtonUI(btnHome, pnlSelectSign, btnMusicLibrary, btnPlayQueue, btnPlayLists, btnSongs, btnArtists, btnAlbums);
            pnlSubSelectSign.Visible = false;
        }

        private void BtnMusicLibrary_Click(object sender, EventArgs e)
        {
            // Set up AutoComplete for the search textbox
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();   // Assuming `musicList` is a List<MyMusic> with Title and Artist 
            autoCompleteCollection.AddRange(musicList.Select(x => x.Title).ToArray());  // Add Titles  to the AutoCompleteStringCollection
            autoCompleteCollection.AddRange(musicList.Select(x => x.Artist).ToArray()); // Add Artists to the AutoCompleteStringCollection

            // Configure the TextBox for auto-completion
            TxtSearch.AutoCompleteCustomSource = autoCompleteCollection;
            TxtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;

            musicSearchFilterSorter.IsAllMusicList = true;
            musicSearchFilterSorter.IsArtistList = false;

            CbSortMusic.Text = "Sort by";
            CbSortMusic.Items.Clear();
            CbSortMusic.Items.AddRange(new string[] { "A - Z", "Z - A", "Artist (Asc)", "Artist (Desc)", "Duration (Asc)", "Duration (Desc)" });
            CbFilterArtist.Left = CbFilterAlbum.Left - CbFilterArtist.Width - 21;
            CbFilterArtist.Visible = true;
            CbFilterAlbum.Visible = true;

            ModifyDvgMusicList(DgvMusicList);   // Update the DataGridView display of the music list
            UpdateSideBarButtonUI(btnMusicLibrary, pnlSelectSign, btnHome, btnPlayQueue, btnPlayLists, btnSongs, btnArtists, btnAlbums);
            UpdateSideBarButtonUI(btnSongs, pnlSubSelectSign, btnArtists, btnAlbums);
            DisplayOrHideSubButtons();

            Panel[] showPanels = { PnlHeaderControl, PnlSortControls, pnlMusicLibrary, PnlMusicList };
            Panel[] hidePanels = { PnlPlayQueueControl, PnlPlayMusicQueue, pnlArtists, pnlAlbum, pnlInfo, pnlArtistTrack, pnlAlbumTrack, pnlPlaylist };
            PanelVisibilityHandler(showPanels, hidePanels);
        }

        private void DisplayOrHideSubButtons()
        {
            bool isVisible = !btnSongs.Visible && !btnArtists.Visible && !btnAlbums.Visible;
            pnlSubSelectSign.Visible = isVisible;

            ToggleButtonVisibility(isVisible, btnSongs, btnArtists, btnAlbums);

            if (isVisible)
            {
                AdjustButtonPosition(btnAlbums, btnPlayQueue, btnPlayLists);
            }
            else
            {
                AdjustButtonPosition(btnMusicLibrary, btnPlayQueue, btnPlayLists);
            }
        }

        private void ToggleButtonVisibility(bool isVisible, params System.Windows.Forms.Button[] buttons)
        {
            foreach (var btn in buttons)
            {
                btn.Visible = isVisible;
            }
        }

        private void AdjustButtonPosition(System.Windows.Forms.Button referenceBtn, params System.Windows.Forms.Button[] buttons)
        {
            int spacing = 8;

            foreach (var btn in buttons)
            {
                btn.Top = referenceBtn.Bottom + spacing;
                referenceBtn = btn;
            }
        }

        private void BtnPlayQueue_Click(object sender, EventArgs e)
        {
            ModifyDvgMusicList(DgvPlayMusicQueue);  // Update the DataGridView display of the music list queue
            UpdateSideBarButtonUI(btnPlayQueue, pnlSelectSign, btnHome, btnMusicLibrary, btnPlayLists, btnSongs, btnArtists, btnAlbums);
            pnlSubSelectSign.Visible = false;

            Panel[] showPanels = { PnlHeaderControl, PnlPlayQueueControl, PnlPlayMusicQueue };
            Panel[] hidePanels = { PnlSortControls, pnlMusicLibrary, pnlArtists, pnlAlbum, pnlInfo, pnlArtistTrack, pnlAlbumTrack, pnlPlaylist };
            PanelVisibilityHandler(showPanels, hidePanels);

            lblShowLibraryTitle.Text = "Songs";
        }

        private void BtnPlayLists_Click(object sender, EventArgs e)
        {
            UpdateSideBarButtonUI(btnPlayLists, pnlSelectSign, btnHome, btnMusicLibrary, btnPlayQueue, btnSongs, btnArtists, btnAlbums);
            pnlSubSelectSign.Visible = false;

            CbFilterArtist.Visible = false;
            CbFilterAlbum.Visible = false;

            Panel[] showPanels = { PnlHeaderControl, PnlSortControls, pnlPlaylist };
            Panel[] hidePanels = { pnlMusicLibrary, PnlPlayQueueControl, PnlPlayMusicQueue, pnlArtists, pnlAlbum, pnlInfo, pnlArtistTrack, pnlAlbumTrack };
            PanelVisibilityHandler(showPanels, hidePanels);
        }

        // MUSIC LIBRARY SUB BUTTONS

        private void BtnSongs_Click(object sender, EventArgs e)
        {
            // Set up AutoComplete for the search textbox
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();   // Assuming `musicList` is a List<MyMusic> with Title and Artist 
            autoCompleteCollection.AddRange(musicList.Select(x => x.Title).ToArray());  // Add Titles  to the AutoCompleteStringCollection
            autoCompleteCollection.AddRange(musicList.Select(x => x.Artist).ToArray()); // Add Artists to the AutoCompleteStringCollection

            // Configure the TextBox for auto-completion
            TxtSearch.AutoCompleteCustomSource = autoCompleteCollection;
            TxtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;

            musicSearchFilterSorter.IsAllMusicList = true;
            musicSearchFilterSorter.IsArtistList = false;

            CbSortMusic.Text = "Sort by";
            CbSortMusic.Items.Clear();
            CbSortMusic.Items.AddRange(new string[] { "A - Z", "Z - A", "Artist (Asc)", "Artist (Desc)", "Duration (Asc)", "Duration (Desc)" });
            CbFilterArtist.Left = CbFilterAlbum.Left - CbFilterArtist.Width - 21;
            CbFilterArtist.Visible = true;
            CbFilterAlbum.Visible = true;

            UpdateSideBarButtonUI(btnSongs, pnlSubSelectSign, btnArtists, btnAlbums);
            UpdateSideBarButtonUI(btnMusicLibrary, pnlSelectSign, btnHome, btnPlayQueue, btnPlayLists);
            pnlSubSelectSign.Visible = true;

            Panel[] showPanels = { PnlHeaderControl, PnlSortControls, pnlMusicLibrary, PnlMusicList };
            Panel[] hidePanels = { PnlPlayQueueControl, PnlPlayMusicQueue, pnlArtists, pnlAlbum, pnlInfo, pnlArtistTrack, pnlAlbumTrack, pnlPlaylist };
            PanelVisibilityHandler(showPanels, hidePanels);

            lblShowLibraryTitle.Text = "Songs";
        }

        private void BtnArtists_Click(object sender, EventArgs e)
        {
            var artistInfo = musicListViewManager.GetMusicInfo(a => new ArtistInfo(MusicListViewManager.CapitalizeWords(a.Artist), a.MusicPictureMedium));
            var albumInfo = musicListViewManager.GetMusicInfo(a => new AlbumInfo(MusicListViewManager.CapitalizeWords(a.Album), a.Artist, a.MusicPictureMedium, a.Duration));

            // Set up AutoComplete for the search textbox
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();   // Assuming `musicList` is a List<MyMusic> with Title and Artist 
            autoCompleteCollection.AddRange(artistInfo.Select(s => s.Name).ToArray());  // Add Titles  to the AutoCompleteStringCollection

            musicSearchFilterSorter.IsAllMusicList = false;
            musicSearchFilterSorter.IsArtistList = true;

            CbSortMusic.Text = "Sort by";
            CbSortMusic.Items.Clear();
            CbSortMusic.Items.AddRange(new string[] { "A - Z", "Z - A" });
            CbFilterArtist.Left = CbFilterArtist.Width + 21;
            CbFilterArtist.Visible = false;
            CbFilterAlbum.Visible = false;

            UpdateSideBarButtonUI(btnArtists, pnlSubSelectSign, btnSongs, btnAlbums);
            UpdateSideBarButtonUI(btnMusicLibrary, pnlSelectSign, btnHome, btnPlayQueue, btnPlayLists);
            pnlSubSelectSign.Visible = true;

            Panel[] showPanels = { PnlHeaderControl, PnlSortControls, pnlMusicLibrary, pnlArtists };
            Panel[] hidePanels = { PnlPlayQueueControl, PnlPlayMusicQueue, PnlMusicList, pnlAlbum, pnlInfo, pnlArtistTrack, pnlAlbumTrack, pnlPlaylist };
            PanelVisibilityHandler(showPanels, hidePanels);

            lblShowLibraryTitle.Text = "Artists";

            MyModule.MakeCircular(pictureBox3);
        }

        private void PanelVisibilityHandler(Panel[] showPanels, Panel[] hidePanels)
        {
            foreach (var showPanel in showPanels)
            {
                showPanel.Visible = true;
            }

            foreach (var hidePanel in hidePanels)
            {
                hidePanel.Visible = false;
            }
        }

        private void HoverEffect(Panel parentControl, Color mouseEnterColor, Color mouseLeaveColor)
        {
            parentControl.MouseEnter += (sender, e) => parentControl.BackColor = mouseEnterColor;
            parentControl.MouseLeave += (sender, e) => parentControl.BackColor = mouseLeaveColor;

            // Apply the same events to all child controls
            foreach (Control child in parentControl.Controls)
            {
                child.MouseEnter += (sender, e) => parentControl.BackColor = mouseEnterColor;
                child.MouseLeave += (sender, e) =>
                {
                    // Check if the mouse is still within the panel
                    Point mousePosition = parentControl.PointToClient(Control.MousePosition);
                    if (!parentControl.ClientRectangle.Contains(mousePosition))
                    {
                        parentControl.BackColor = mouseLeaveColor;
                    }
                };
            }
        }

        private void BtnAlbums_Click(object sender, EventArgs e)
        {
            var artistInfo = musicListViewManager.GetMusicInfo(a => new ArtistInfo(MusicListViewManager.CapitalizeWords(a.Artist), a.MusicPictureMedium));
            var albumInfo = musicListViewManager.GetMusicInfo(a => new AlbumInfo(MusicListViewManager.CapitalizeWords(a.Album), a.Artist, a.MusicPictureMedium, a.Duration));

            // Set up AutoComplete for the search textbox
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();   // Assuming `musicList` is a List<MyMusic> with Title and Artist 
            autoCompleteCollection.AddRange(artistInfo.Select(s => s.Name).ToArray());  // Add Titles  to the AutoCompleteStringCollection
            autoCompleteCollection.AddRange(albumInfo.Select(s => s.AlbumName).ToArray()); // Add Artists to the AutoCompleteStringCollection

            // Configure the TextBox for auto-completion
            TxtSearch.AutoCompleteCustomSource = autoCompleteCollection;
            TxtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;

            musicSearchFilterSorter.IsAllMusicList = false;
            musicSearchFilterSorter.IsArtistList = false;

            CbSortMusic.Text = "Sort by";
            CbSortMusic.Items.Clear();
            CbSortMusic.Items.AddRange(new string[] { "A - Z", "Z - A", "Artist (A - Z)", "Artist (Z - A)" });
            CbFilterArtist.Left = CbFilterAlbum.Left;
            CbFilterArtist.Visible = true;
            CbFilterAlbum.Visible = false;

            UpdateSideBarButtonUI(btnAlbums, pnlSubSelectSign, btnSongs, btnArtists);
            UpdateSideBarButtonUI(btnMusicLibrary, pnlSelectSign, btnHome, btnPlayQueue, btnPlayLists);
            pnlSubSelectSign.Visible = true;

            Panel[] showPanels = { PnlHeaderControl, PnlSortControls, pnlMusicLibrary, pnlAlbum };
            Panel[] hidePanels = { PnlPlayQueueControl, PnlPlayMusicQueue, PnlMusicList, pnlArtists, pnlInfo, pnlArtistTrack, pnlAlbumTrack, pnlPlaylist };
            PanelVisibilityHandler(showPanels, hidePanels);

            lblShowLibraryTitle.Text = "Albums";
        }

        private void UpdateSideBarButtonUI(System.Windows.Forms.Button targetButton, System.Windows.Forms.Panel pnl, params System.Windows.Forms.Button[] Buttons)
        {
            int paddingTop = 8;
            pnl.Top = targetButton.Top + paddingTop;  // Set the position base on the selected button

            targetButton.BackColor = Color.FromArgb(47, 53, 64); // Selected background color
            targetButton.ForeColor = Color.White;   // Change the font color of the selected button

            // Update the isClicked state of the new target button
            if (targetButton is NielarkButton nielarkButton)
            {
                nielarkButton.SetClickedState(true);
            }

            // Loop to set the default appearance of the side bar buttons
            foreach (var btn in Buttons)
            {
                // Reset the side bar button's color if not selected
                btn.BackColor = Color.FromArgb(12, 23, 45);

                // Reset the side bar font's color if not selected
                btn.ForeColor = Color.FromName("ScrollBar");

                if (btn is NielarkButton otherNielarkButton)
                {
                    otherNielarkButton.SetClickedState(false);
                }
            }

            // Update the last clicked button reference
            lastClickedButton = targetButton;
        }

        private void ModifyDvgMusicList(DataGridView dataGridViewMusic)
        {
            // Customize the data grid view when there is a data row displayed
            if (dataGridViewMusic.RowCount > 0)
            {
                // Hide the header title of the column
                dataGridViewMusic.Columns["File"].Visible = false;
                dataGridViewMusic.Columns["musicPictureMedium"].Visible = false;
                dataGridViewMusic.Columns[0].HeaderText = "Image";  // Set the header title of the first column

                // Customize the sizing of columns
                dataGridViewMusic.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridViewMusic.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewMusic.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewMusic.Columns["Artist"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewMusic.Columns["Album"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewMusic.Columns["Duration"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Dynamically adjust the height of the DataGridView based on the number of rows
                int rowHeight = dataGridViewMusic.RowTemplate.Height; // The height of one row
                int totalHeight = dataGridViewMusic.Rows.Count * rowHeight + dataGridViewMusic.ColumnHeadersHeight;

                // Set DataGridView height dynamically
                dataGridViewMusic.Height = totalHeight > PnlMusicList.Height ? totalHeight : PnlMusicList.Height;
            }
        }

        private void DgvMusicList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                musicListViewManager.MusicListView(DgvMusicList, DgvPlayMusicQueue, CbFilterArtist, CbFilterAlbum, TimerShowPnlPlayerControls);
            }
        }

        private void DgvMusicList_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            //DgvMusicList.ClearSelection();  // Clear current selection
            //DgvMusicList.Rows[e.RowIndex].Selected = true;  // Select the row that was right-clicked
            //e.ContextMenuStrip = contextMenuStrip1;
            //playerControls.selectedRowIndex = e.RowIndex;

            musicListViewManager.GetSelectedRowIndex(DgvMusicList, contextMenuStrip1, e);
            contextMenuStrip1.Tag = DgvMusicList;

            // Hide and show the context menu items based on the selected DataGridView
            showArtistToolStripMenuItem.Visible = true;
            showAlbumToolStripMenuItem.Visible = true;
        }

        private void DgvArtistTracks_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            musicListViewManager.GetSelectedRowIndex(dgvArtistTracks, contextMenuStrip1, e);
            contextMenuStrip1.Tag = dgvArtistTracks;

            // Hide and show the context menu items based on the selected DataGridView
            showArtistToolStripMenuItem.Visible = false;
            showAlbumToolStripMenuItem.Visible = true;
        }

        private void DgvAlbumTracks_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            musicListViewManager.GetSelectedRowIndex(dgvAlbumTracks, contextMenuStrip1, e);
            contextMenuStrip1.Tag = dgvAlbumTracks;

            // Hide and show the context menu items based on the selected DataGridView
            showArtistToolStripMenuItem.Visible = true;
            showAlbumToolStripMenuItem.Visible = false;
        }

        private void PlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the context menu strip has a tag (i.e., if a DataGridView is associated with it)
            if (contextMenuStrip1.Tag == null)
            {
                MessageBox.Show("No music selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridView dgv = (DataGridView)contextMenuStrip1.Tag;  // Get the DataGridView from the context menu strip
            
            var selectedRowIndex = playerControls.selectedRowIndex;

            string? title = dgv.Rows[selectedRowIndex].Cells[2].Value.ToString();
            string? artist = dgv.Rows[selectedRowIndex].Cells[3].Value.ToString();
            string? getFilePath = dgv.Rows[selectedRowIndex].Cells[6].Value.ToString();

            musicListViewManager.playMusicQueue = musicList
                                .Where(s => s.File.Equals(getFilePath))
                                .ToList();

            playerControls.SetPlayMusicQueue(musicListViewManager.playMusicQueue);
            playerControls.TogglePlayAndPause();

            musicListViewManager.FindAndPlayMusic(title, artist, getFilePath);
            musicListViewManager.UpdateDataGridViewList(DgvPlayMusicQueue, getFilePath);

            TimerShowPnlPlayerControls.Start();
        }

        private void ShowArtistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the context menu strip has a tag (i.e., if a DataGridView is associated with it)
            if (contextMenuStrip1.Tag == null)
            {
                MessageBox.Show("No music selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridView dgv = (DataGridView)contextMenuStrip1.Tag;  // Get the DataGridView from the context menu strip

            var nameIdentifier = dgv.Rows[playerControls.selectedRowIndex].Cells[3].Value.ToString();  // Get the artist name from the selected row

            // Check if the artist name is null or empty
            if (nameIdentifier == null)
            {
                MessageBox.Show("Artist not found! Please check if the artist exists.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selected = musicListViewManager.SelectArtistOrAlbum(nameIdentifier, true);      // Select the artist name from the music list
            musicListViewManager.DisplaySelectedArtistOrAlbum(selected, nameIdentifier, true);  // Display the selected artist name

            musicListViewManager.DisplayArtistOrAlbumTracks(true, dgvArtistTracks, pnlArtistTrack, pnlAlbumTrack);  // Display the selected artist tracks

            // Hide and show the panels
            Panel[] showPanels = { pnlInfo };
            Panel[] hidePanels = { PnlHeaderControl, pnlMusicLibrary, PnlPlayMusicQueue };
            PanelVisibilityHandler(showPanels, hidePanels);
        }

        private void ShowAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the context menu strip has a tag (i.e., if a DataGridView is associated with it)
            if (contextMenuStrip1.Tag == null)
            {
                MessageBox.Show("No music selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridView dgv = (DataGridView)contextMenuStrip1.Tag;  // Get the DataGridView from the context menu strip

            var nameIdentifier = dgv.Rows[playerControls.selectedRowIndex].Cells[4].Value.ToString();  // Get the artist name from the selected row

            // Check if the artist name is null or empty
            if (nameIdentifier == null)
            {
                MessageBox.Show("Album not found! Please check if the album exists.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selected = musicListViewManager.SelectArtistOrAlbum(nameIdentifier, false);      // Select the artist name from the music list
            musicListViewManager.DisplaySelectedArtistOrAlbum(selected, nameIdentifier, false);  // Display the selected artist name

            musicListViewManager.DisplayArtistOrAlbumTracks(false, dgvAlbumTracks, pnlAlbumTrack, pnlArtistTrack); // Display the selected album tracks

            // Hide and show the panels
            Panel[] showPanels = { pnlInfo };
            Panel[] hidePanels = { PnlHeaderControl, pnlMusicLibrary, PnlPlayMusicQueue };
            PanelVisibilityHandler(showPanels, hidePanels);
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the context menu strip has a tag (i.e., if a DataGridView is associated with it)
            if (contextMenuStrip1.Tag == null)
            {
                MessageBox.Show("No music selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridView dgv = (DataGridView)contextMenuStrip1.Tag;  // Get the DataGridView from the context menu strip

            // Open the Music Properties form
            Form MusicProperties = new MusicProperties(this, playerControls, dgv);
            MusicProperties.ShowDialog();
        }

        private void PlayQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if the context menu strip has a tag (i.e., if a DataGridView is associated with it)
            if (contextMenuStrip1.Tag == null)
            {
                MessageBox.Show("No music selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridView dgv = (DataGridView)contextMenuStrip1.Tag;  // Get the DataGridView from the context menu strip

            var selectedRowIndex = playerControls.selectedRowIndex;

            string? title = dgv.Rows[selectedRowIndex].Cells[2].Value.ToString();
            string? artist = dgv.Rows[selectedRowIndex].Cells[3].Value.ToString();
            string? getFilePath = dgv.Rows[selectedRowIndex].Cells[6].Value.ToString();

            var playMusicQueue = musicList
                                .Where(s => s.File.Equals(getFilePath))
                                .ToList();

            musicListViewManager.playMusicQueue.AddRange(playMusicQueue);
            playerControls.SetPlayMusicQueue(musicListViewManager.playMusicQueue);
            musicListViewManager.playMusicQueue = playerControls.playMusicQueue;
            musicListViewManager.UpdateDataGridViewList(DgvPlayMusicQueue, getFilePath);
        }

        private void DgvPlayMusicQueue_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            musicListViewManager.MusicQueueView(DgvPlayMusicQueue);
        }

        private void DgvArtistsTracks_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            musicListViewManager.PlayArtistTrack(dgvArtistTracks, DgvPlayMusicQueue, TimerShowPnlPlayerControls);
        }

        private void DgvAlbumTracks_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            musicListViewManager.PlayAlbumTrack(dgvAlbumTracks, DgvPlayMusicQueue, TimerShowPnlPlayerControls);
        }

        // PLAYER CONTROLS

        private void PicBoxShuffleMusic_Click(object sender, EventArgs e)
        {
            playerControls.ShuffleMusicHandler(DgvPlayMusicQueue);
        }

        private void PicBoxSkipBackward_Click(object sender, EventArgs e)
        {
            playerControls.SkipBackward();
        }

        private void PicBoxPlayPreviousMusic_Click(object sender, EventArgs e)
        {
            playerControls.PlayPreviousMusic();
        }

        private void PicBoxPlayAndPause_Click(object sender, EventArgs e)
        {
            playerControls.TogglePlayAndPause();
        }

        private void PicBoxPlayNextMusic_Click(object sender, EventArgs e)
        {
            playerControls.PlayNextMusic();
        }

        private void PicBoxSkipForward_Click(object sender, EventArgs e)
        {
            playerControls.SkipForward();
        }

        private void PicBoxRepeatMusic_Click(object sender, EventArgs e)
        {
            playerControls.RepeatMusicHandler();
        }

        private void OnShuffleMusicStateChanged(bool isShuffle)
        {
            playerControls.UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, isShuffle);
        }

        private void OnPlayAndPauseStateChanged(bool isPlaying)
        {
            playerControls.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, isPlaying);
        }

        private void OnRepeatMusicStateChanged(bool isRepeat)
        {
            playerControls.UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, isRepeat);
        }

        private void PicBoxShowVolumeBar_Click(object sender, EventArgs e)
        {
            // Display the volume bar
            PnlVolumeControl.Visible = true;
        }

        private void PicBoxVolumePicture_Click(object sender, EventArgs e)
        {
            playerControls.VolumeMuteAndUnmuteToggle();
        }

        private void OnMuteStateChanged(bool isMute)
        {
            playerControls.UpdateVolumeValueAndUI(TbVolume, isMute);
        }

        private void PnlVolumeControl_MouseLeave(object sender, EventArgs e)
        {
            playerControls.PnlVolumeControlMousePositionHandler(PnlVolumeControl, PicBoxVolumePicture, TbVolume, LblVolumeValue);
        }

        private void TbVolume_MouseUp(object sender, MouseEventArgs e)
        {
            playerControls.UpdateMuteState();
        }

        private void OnUpdateMuteStateChanged()
        {
            playerControls.UpdateMuteStateUI(TbVolume);
        }

        private void TbVolume_ValueChanged()
        {
            // Update volume and icon based on the current trackbar value
            playerControls.UpdateVolumeValue(TbVolume.Value);
            playerControls.UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);
        }

        private void OnUpdateVolumeValueChanged(int newVolumeValue)
        {
            TbVolume.Value = newVolumeValue;
            playerControls.UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);
        }

        private void TimerTitleMarquee_Tick(object sender, EventArgs e)
        {
            playerControls.TitleMarqueeEffectHandler(LblShowPlayTitle, PnlMarquee);
        }

        private void TimerArtistMarquee_Tick(object sender, EventArgs e)
        {
            playerControls.ArtistMarqueeEffectHandler(LblShowPlayArtist, PnlMarquee);
        }

        private void TimerMusicDuration_Tick(object sender, EventArgs e)
        {
            playerControls.MusicSeekBarHandler(TbSeekMusic, LblMusicLength, LblMusicDurationCtr, TimerMusicDuration);
        }

        private void TbSeekMusic_ValueChanged()
        {
            playerControls.UpdateMusicLengthAndDuration(TbSeekMusic, LblMusicLength, LblMusicDurationCtr);
        }

        private void TbSeekMusic_MouseUp(object sender, MouseEventArgs e)
        {
            playerControls.MusicSeekBarMouseUpHandler(TbSeekMusic);
        }

        private void TbSeekMusic_MouseDown(object sender, MouseEventArgs e)
        {
            playerControls.MusicSeekBarMouseDownHandler();
        }

        private void PicBoxFullScreenToggle_Click(object sender, EventArgs e)
        {
            fullScreen = !fullScreen;

            if (fullScreen)
            {
                PicBoxFullScreenToggle.Image = Properties.Resources.full_screen;
                toolTipPlayerControl.SetToolTip(PicBoxFullScreenToggle, "Exit Full Screen");
            }
            else
            {
                PicBoxFullScreenToggle.Image = Properties.Resources.exit_full_screen;
                toolTipPlayerControl.SetToolTip(PicBoxFullScreenToggle, "Full Screen");
            }
        }

        private void PicBoxMiniPlayerToggle_Click(object sender, EventArgs e)
        {
            Form MiniPlayer = new MiniPlayer(this, DgvPlayMusicQueue, playerControls);
            this.Hide();
            MiniPlayer.Show();
        }

        public void ReturnToMainForm()
        {
            this.Show();  // Show the main form when returning from MiniPlayer
        }

        private void PlayBackInfoParentAndChild_MouseMove(object sender, MouseEventArgs e)
        {
            HoverEffect(PnlPlayBackInfo, Color.FromArgb(89, 89, 89), Color.Transparent);
        }

        private void PnlPlayBackInfo_MouseLeave(object sender, EventArgs e)
        {
            PnlPlayBackInfo.BackColor = Color.Transparent;  // Reset background color when not hovering
        }

        private void MouseHoverPlayBackInfo()
        {
            Point mousePosition = PnlPlayBackInfo.PointToClient(Control.MousePosition);  // Get mouse position relative to the panel

            Rectangle panelBounds = PnlMarquee.Bounds;
            Rectangle pictureBounds = PicBoxShowPlayPicture.Bounds;
            Rectangle titleBounds = LblShowPlayTitle.Bounds;
            Rectangle artistBounds = LblShowPlayArtist.Bounds;
            Rectangle marqueeBounds = PnlMarquee.Bounds;

            // Check if the mouse is over any of the controls (relative to the panel)
            if (panelBounds.Contains(mousePosition) ||
                pictureBounds.Contains(mousePosition) ||
                titleBounds.Contains(mousePosition) ||
                artistBounds.Contains(mousePosition) ||
                marqueeBounds.Contains(mousePosition))
            {
                PnlPlayBackInfo.BackColor = Color.FromArgb(89, 89, 89);  // Set hover color
            }
            else
            {
                PnlPlayBackInfo.BackColor = Color.Transparent;  // Reset background color when not hovering
            }
        }

        private void TimerShowPnlPlayerControls_Tick(object sender, EventArgs e)
        {
            int currentTopPosition = PnlPlayerControls.Top;         // Get the current top position of the PnlPlayerControls
            int decrementValue = 50, heightAdjustmentOffset = 0;

            // Check if the panel's current top position is greater than the target position
            if (currentTopPosition > targetTopPosition && PnlPlayerControls.Bottom != pnlMain.Height)
            {
                // If the next move will overshoot the target, adjust the decrement value
                if (currentTopPosition - decrementValue < targetTopPosition)
                {
                    decrementValue = currentTopPosition - targetTopPosition;    // Adjust the decrement value so that the panel lands exactly at the target position
                    heightAdjustmentOffset = 5;     // Adjust the height with a small offset to prevent visual inconsistencies
                }

                PnlPlayerControls.Top -= decrementValue;    // Move the panel upwards

                // Adjust the heights of the associated panels accordingly
                pnlMusicLibrary.Height -= decrementValue - heightAdjustmentOffset;
                PnlPlayMusicQueue.Height -= decrementValue - heightAdjustmentOffset;
                pnlArtistTrack.Height -= decrementValue - heightAdjustmentOffset;
                pnlAlbumTrack.Height -= decrementValue - heightAdjustmentOffset;
            }
            else
            {
                TimerShowPnlPlayerControls.Stop();  // Stop the timer once the panel reaches the target position
            }
        }

        //private void PnlHeader_MouseDown(object sender, MouseEventArgs e)
        //{
        //    MyModule.StartDrag(e);
        //}

        private void PnlHeader_MouseMove(object sender, MouseEventArgs e)
        {
            //MyModule.MovedDrag(this, e);
        }

        private void PnlHeader_MouseUp(object sender, MouseEventArgs e)
        {
            //MyModule.EndDrag();
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            //formSize = this.ClientSize;
            this.WindowState = FormWindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            MaximizeWindow();
        }

        private void MaximizeWindow()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                formSize = this.ClientSize;
                this.WindowState = FormWindowState.Maximized;
                //btnMaximize.Image = Properties.Resources.minimize_window;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = formSize;
            }

            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        private void BtnCloseApplication_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void FormMain_Resize(object sender, EventArgs e)
        {
            ApplyRoundedCorners();
            HandleResponsiveLayout();
            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);

            // Update the width of the PnlPlayBackInfo with respect to form size controls
            PnlPlayBackInfo.Width = this.ClientSize.Width <= 600 ? PicBoxPlayPreviousMusic.Left - 15 : PicBoxShuffleMusic.Left - 15;

            //PnlPlayBackInfo.Width = PicBoxShuffleMusic.Left - 15;   // Update the width of the PnlPlayBackInfo with respect to form size

            label1.Text = $"Panel Width: {this.ClientSize.Width} \n Playbackinfo Width: {PnlPlayBackInfo.Width} \n space: {CbFilterArtist.Left - BtnShuffleAndPlay.Right}, size: {PnlPlayBackInfo.Left}";

            targetTopPosition = PnlPlayerControls.Top - 105;    // Update the target position value for accurate displaying position of PnlPlayerControls
        }

        private void ApplyRoundedCorners()
        {
            // Maintain the rounded corners of the following panels when resizing the form
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            MyModule.SetRoundedCorners(pnlMain, 10, 10, 10, 10);
            MyModule.SetRoundedCorners(PnlSideBar, 10, 0, 0, 10);
            MyModule.SetRoundedCorners(PnlHeader, 10, 10, 0, 0);
            MyModule.SetRoundedCorners(PnlPlayMusicQueue, 0, 0, 10, 0);
            MyModule.SetRoundedCorners(PnlPlayerControls, 0, 0, 10, 0);
            MyModule.SetRoundedCorners(PnlPlayerControls, 0, 0, 10, 0);
            MyModule.SetRoundedCorners(picBoxAlbumImage, 10, 10, 10, 10);
            MyModule.SetRoundedCorners(pnlMoreOption, 10, 10, 10, 10);
        }

        private void HandleResponsiveLayout()
        {
            if (this.ClientSize.Width > 900)
            {
                // Function call to display Controls
                ShowOrHideDataGridViewColumn(true);
                ShowOrHideShuffleAndRepeatButton(true);
                ShowOrHideFilterControl(true, 161, 45, 272);
                ShowOrHideSideBar(0, PnlSideBar.Width, -30);
                ShowOrHideSkipButton(true, 45, 15);

                ArtistOrAlbumLabelFontResize(20.25F, 15.75F, 12);
                ArtistAlbumBtnSizeAndPositionHandler(130);
                ArtistAlbumBtnBackgroundHandler(false);

                picBoxHamburgerMenu.Visible = false;
            }
            else if (this.ClientSize.Width > 860 && this.ClientSize.Width <= 900)
            {
                // Function call to hide controls
                ShowOrHideSkipButton(false, 0, -30);

                // Function call to display Controls
                ShowOrHideDataGridViewColumn(true);
                ShowOrHideShuffleAndRepeatButton(true);
                ShowOrHideFilterControl(true, 161, 45, 272);
                ShowOrHideSideBar(0, PnlSideBar.Width, -30);

                ArtistOrAlbumLabelFontResize(20.25F, 15.75F, 12);
                ArtistAlbumBtnSizeAndPositionHandler(130);
                ArtistAlbumBtnBackgroundHandler(false);

                picBoxHamburgerMenu.Visible = false;
            }
            else if (this.ClientSize.Width > 660 && this.ClientSize.Width <= 860 && this.ClientSize.Width != previousWidth) // 500
            {
                // Function call to hide controls
                ShowOrHideSkipButton(false, 0, -30);
                ShowOrHideSideBar(-PnlSideBar.Width, 0, 15);

                // Function call to display Controls
                ShowOrHideDataGridViewColumn(true);
                ShowOrHideShuffleAndRepeatButton(true);
                ShowOrHideFilterControl(true, 161, 45, 272);

                picBoxHamburgerMenu.Visible = true;

                ArtistOrAlbumLabelFontResize(20.25F, 15.75F, 12);
                ArtistAlbumBtnSizeAndPositionHandler(130);
                ArtistAlbumBtnBackgroundHandler(false);
            }
            else if (this.ClientSize.Width > 600 && this.ClientSize.Width <= 660 && this.ClientSize.Width != previousWidth)
            {
                // Function call to hide controls
                ShowOrHideSkipButton(false, 0, -30);
                ShowOrHideSideBar(-PnlSideBar.Width, 0, 15);
                ShowOrHideFilterControl(false, 0, 0, 111);

                // Function call to display Controls
                ShowOrHideDataGridViewColumn(true);
                ShowOrHideShuffleAndRepeatButton(true);

                ArtistOrAlbumLabelFontResize(15.25F, 10.75F, 9);
                ArtistAlbumBtnSizeAndPositionHandler(100);
                ArtistAlbumBtnBackgroundHandler(true);

                picBoxHamburgerMenu.Visible = true;
            }
            else if (this.ClientSize.Width <= 600 && this.ClientSize.Width != previousWidth)
            {
                // Function call to hide controls
                ShowOrHideSkipButton(false, 0, -30);
                ShowOrHideSideBar(-PnlSideBar.Width, 0, 15);
                ShowOrHideFilterControl(false, 0, 0, 111);
                ShowOrHideDataGridViewColumn(false);
                ShowOrHideShuffleAndRepeatButton(false);

                ArtistOrAlbumLabelFontResize(15.25F, 10.75F, 9);
                ArtistAlbumBtnSizeAndPositionHandler(60);
                ArtistAlbumBtnBackgroundHandler(true);

                picBoxHamburgerMenu.Visible = true;
            }

            previousWidth = this.ClientSize.Width;
            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        private void ShowOrHideSkipButton(bool showSkipButton, int shuffleOffset, int repeatOffset)
        {
            PicBoxSkipForward.Visible = showSkipButton;
            PicBoxSkipBackward.Visible = showSkipButton;

            PicBoxShuffleMusic.Left = PicBoxSkipBackward.Left - shuffleOffset;
            PicBoxRepeatMusic.Left = PicBoxSkipForward.Right + repeatOffset;
        }

        private void ShowOrHideSideBar(int sideBarLeftPosition, int sideBarWidth, int headerTitleOffset)
        {
            // Displaying hamburger menu and repositioning header title
            lblHeaderTitle.Left = picBoxHamburgerMenu.Right + headerTitleOffset;


            PnlSideBar.Left = sideBarLeftPosition;

            //PnlHeaderControl.Left = sideBarWidth;
            //pnlMusicLibrary.Left = sideBarWidth;
            //PnlPlayMusicQueue.Left = sideBarWidth;
            pnlContent.Left = sideBarWidth;

            //PnlHeaderControl.Width = pnlMain.Width - sideBarWidth;
            //pnlMusicLibrary.Width = pnlMain.Width - sideBarWidth;
            //PnlPlayMusicQueue.Width = pnlMain.Width - sideBarWidth;
            pnlContent.Width = pnlMain.Width - sideBarWidth;
        }

        private void ShowOrHideFilterControl(bool showControl, int filterArtistOffset, int volumeButtonOffset, int searchBarWidth)
        {
            CbFilterAlbum.Visible = showControl;
            CbFilterArtist.Left = CbFilterAlbum.Left - filterArtistOffset;

            PicBoxFullScreenToggle.Visible = showControl;
            PicBoxShowVolumeBar.Left = PicBoxFullScreenToggle.Left - volumeButtonOffset;

            picBoxSearchIcon.Left = CbFilterArtist.Left;
            TxtSearch.Left = picBoxSearchIcon.Right;
            TxtSearch.Width = searchBarWidth; // 172 maximum size
        }

        private void ShowOrHideDataGridViewColumn(bool showColumn)
        {
            if (DgvMusicList.RowCount > 0)
            {
                DgvMusicList.Columns[0].Visible = showColumn;         // Image Column
                DgvMusicList.Columns["Album"].Visible = showColumn;
            }

            if (DgvPlayMusicQueue.RowCount > 0)
            {
                DgvPlayMusicQueue.Columns[0].Visible = showColumn;        // Image Column
                DgvPlayMusicQueue.Columns["Album"].Visible = showColumn;
            }
        }

        private void ShowOrHideShuffleAndRepeatButton(bool showButton)
        {
            PicBoxShuffleMusic.Visible = showButton;
            PicBoxRepeatMusic.Visible = showButton;
        }

        private void ArtistOrAlbumLabelFontResize(float albumNameFontSize, float albumArtistFontSize, float albumNumbersFontSize)
        {
            lblAlbumName.Font = new Font("Segoe UI", albumNameFontSize, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAlbumArtist.Font = new Font("Segoe UI Semibold", albumArtistFontSize, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAlbumNumbers.Font = new Font("Segoe UI", albumNumbersFontSize, FontStyle.Regular, GraphicsUnit.Point, 0);
        }

        private void ArtistAlbumBtnSizeAndPositionHandler(int btnWidth)
        {
            btnAlbumPlay.Size = new Size(btnWidth, 35);
            btnAlbumShuffleAndPlay.Size = new Size(btnWidth, 35);
            btnBack.Size = new Size(btnWidth, 35);

            //btnAlbumPlay.Left -= btnWidth;
            btnAlbumShuffleAndPlay.Left = btnAlbumPlay.Right + 10;
            btnBack.Left = btnAlbumShuffleAndPlay.Right + 10;
        }

        private void ArtistAlbumBtnBackgroundHandler(bool isShowBackground)
        {
            if (isShowBackground)
            {
                btnAlbumPlay.Text = string.Empty;
                btnAlbumShuffleAndPlay.Text = string.Empty;
                btnBack.Text = string.Empty;

                btnAlbumPlay.BackgroundImage = Properties.Resources.play_inactive;
                btnAlbumShuffleAndPlay.BackgroundImage = Properties.Resources.shuffle_inactive;
                btnBack.BackgroundImage = Properties.Resources.back_inactive;
            }
            else
            {
                btnAlbumPlay.Text = "Play All";
                btnAlbumShuffleAndPlay.Text = "Shuffle and Play";
                btnBack.Text = "Back";

                btnAlbumPlay.BackgroundImage = null;
                btnAlbumShuffleAndPlay.BackgroundImage = null;
                btnBack.BackgroundImage = null;
            }
        }

        private void PicBoxHamburgerMenu_Click(object sender, EventArgs e)
        {
            // Display and hide the sidebar
            if (PnlSideBar.Left < 0)
            {
                PnlSideBar.Left = 0;
                picBoxHamburgerMenu.Image = Properties.Resources.close;
            }
            else
            {
                PnlSideBar.Left = -PnlSideBar.Width;
                picBoxHamburgerMenu.Image = Properties.Resources.hamburger_menu;
            }
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    isDragging = true; // Start dragging
            //    dragCursorPoint = Cursor.Position;
            //    dragFormPoint = this.Location;
            //}
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
        {
            //if (isDragging)
            //{
            //    // Calculate new position
            //    Point newCursorPoint = Cursor.Position;
            //    int deltaX = newCursorPoint.X - dragCursorPoint.X;
            //    int deltaY = newCursorPoint.Y - dragCursorPoint.Y;
            //    this.Location = new Point(dragFormPoint.X + deltaX, dragFormPoint.Y + deltaY);

            //    // Check for Aero Snap
            //    CheckForAeroSnap();
            //}
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            //isDragging = false; // Stop dragging
        }

        private void BtnAlbumPlay_Click(object sender, EventArgs e)
        {
            musicListViewManager.PlayAllArtistOrAlbumTracks(lblAlbumName, DgvPlayMusicQueue, TimerShowPnlPlayerControls);
        }

        private void BtnAlbumShuffleAndPlay_Click(object sender, EventArgs e)
        {
            musicListViewManager.ShuffleAndPlayArtistsOrAlbumTracks(lblAlbumName, DgvPlayMusicQueue, TimerShowPnlPlayerControls);
            //playerControls.ShuffleMusicHandler(DgvPlayMusicQueue);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            musicListViewManager.BackToArtistOrAlbumList();
        }

        private void BtnAlbumPlay_MouseHover(object sender, EventArgs e)
        {
            if (this.ClientSize.Width <= 660)
            {
                btnAlbumPlay.BackgroundImage = Properties.Resources.play_active;
            }
        }

        private void BtnAlbumPlay_MouseLeave(object sender, EventArgs e)
        {
            if (this.ClientSize.Width <= 660)
            {
                btnAlbumPlay.BackgroundImage = Properties.Resources.play_inactive;
            }
        }

        private void BtnAlbumShuffleAndPlay_MouseHover(object sender, EventArgs e)
        {
            if (this.ClientSize.Width <= 660)
            {
                btnAlbumShuffleAndPlay.BackgroundImage = Properties.Resources.shuffle_active;
            }
        }

        private void BtnAlbumShuffleAndPlay_MouseLeave(object sender, EventArgs e)
        {
            if (this.ClientSize.Width <= 660)
            {
                btnAlbumShuffleAndPlay.BackgroundImage = Properties.Resources.shuffle_inactive;
            }
        }

        private void BtnBack_MouseHover(object sender, EventArgs e)
        {
            if (this.ClientSize.Width <= 660)
            {
                btnBack.BackgroundImage = Properties.Resources.back_active;
            }
        }

        private void BtnBack_MouseLeave(object sender, EventArgs e)
        {
            if (this.ClientSize.Width <= 660)
            {
                btnBack.BackgroundImage = Properties.Resources.back_inactive;
            }
        }

        // MORE OPTION BUTTONS
        private void PicBoxMoreOption_Click_1(object sender, EventArgs e)
        {
            pnlMoreOption.Visible = true;
        }

        private void PnlMoreOption_MouseLeave(object sender, EventArgs e)
        {
            Point mousePosition = Control.MousePosition;

            // Convert all controls' bounds to screen coordinates
            Rectangle panelBounds = pnlMoreOption.RectangleToScreen(pnlMoreOption.ClientRectangle);
            Rectangle btnPropertiesBounds = btnProperties.RectangleToScreen(btnProperties.ClientRectangle);
            Rectangle btnShuffleBounds = btnShuffle.RectangleToScreen(btnShuffle.ClientRectangle);
            Rectangle btnSkipBackwardBounds = btnSkipBackward.RectangleToScreen(btnSkipBackward.ClientRectangle);
            Rectangle btnSkipForwardBounds = btnSkipForward.RectangleToScreen(btnSkipForward.ClientRectangle);
            Rectangle btnRepeatBounds = btnRepeat.RectangleToScreen(btnRepeat.ClientRectangle);
            Rectangle btnFullScreenBounds = btnFullScreen.RectangleToScreen(btnFullScreen.ClientRectangle);

            // Check if the mouse is outside all the controls (panel and its children)
            if (!panelBounds.Contains(mousePosition) &&
                !btnPropertiesBounds.Contains(mousePosition) &&
                !btnShuffleBounds.Contains(mousePosition) &&
                !btnSkipBackwardBounds.Contains(mousePosition) &&
                !btnSkipForwardBounds.Contains(mousePosition) &&
                !btnRepeatBounds.Contains(mousePosition) &&
                !btnFullScreenBounds.Contains(mousePosition))
            {
                pnlMoreOption.Visible = false;   // Hide the More Option Panel
            }
        }

        private void BtnProperties_Click(object sender, EventArgs e)
        {
            pnlMoreOption.Visible = false;   // Hide the More Option Panel

            // Open the Music Properties form
            Form MusicProperties = new MusicProperties(this, playerControls);
            MusicProperties.ShowDialog();
        }

        private void BtnShuffle_Click(object sender, EventArgs e)
        {
            playerControls.ShuffleMusicHandler(DgvPlayMusicQueue);
        }

        private void BtnSkipBackward_Click(object sender, EventArgs e)
        {
            playerControls.SkipBackward();
        }

        private void BtnSkipForward_Click(object sender, EventArgs e)
        {
            playerControls.SkipForward();
        }

        private void BtnRepeat_Click(object sender, EventArgs e)
        {
            playerControls.RepeatMusicHandler();
        }

        private void BtnFullScreen_Click(object sender, EventArgs e)
        {

        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                playerControls.TogglePlayAndPause();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //var musicQueueSerializable = playerControls.playMusicQueue.Select(m => new
            //{
            //    MusicPictureSmall = ImageToBase64(m.MusicPictureSmall),  // Convert Image to Base64
            //    MusicPictureMedium = ImageToBase64(m.MusicPictureMedium),
            //    Title = m.Title,
            //    Artist = m.Artist,
            //    Album = m.Album,
            //    Duration = m.Duration.ToString(), // Convert TimeSpan to string
            //    File = m.File
            //}).ToList();



            //string filePath = "music_queue.json";
            //string json = JsonSerializer.Serialize(musicQueueSerializable, new JsonSerializerOptions { WriteIndented = true });
            //System.IO.File.WriteAllText(filePath, json);

            //ClearAllPictureBoxes(); // Free memory by clearing PictureBoxes

        }

        private void ClearAllPictureBoxes()
        {
            foreach (Form form in Application.OpenForms)
            {
                foreach (Control control in form.Controls)
                {
                    SetPictureBoxNull(control);
                }
            }
        }

        private void SetPictureBoxNull(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose(); // Dispose safely
                        pictureBox.Image = null;
                    }
                }
                else if (control.HasChildren)
                {
                    SetPictureBoxNull(control); // Recursively check child controls
                }
            }
        }

        private List<Music> LoadMusicQueue()
        {
            string filePath = "music_queue.json";

            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);

                // Deserialize into dynamic objects with Base64 strings
                var deserializedQueue = JsonSerializer.Deserialize<List<Music>>(json) ?? new List<Music>();

                // Convert Base64 to Image before assigning to `Music`
                return deserializedQueue.Select(m => new Music(
                    Base64ToImage(m.MusicPictureSmall.ToString()),  // Convert Base64 back to Image
                    Base64ToImage(m.MusicPictureMedium.ToString()),
                    m.Title.ToString(),
                    m.Artist.ToString(),
                    m.Album.ToString(),
                    TimeSpan.Parse(m.Duration.ToString()),  // Convert string to TimeSpan
                    m.File.ToString()
                )).ToList();
            }

            return new List<Music>(); // Return empty list if file doesn't exist
        }

        private string ImageToBase64(Image image)
        {
            if (image == null) return string.Empty;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Bitmap bmp = new Bitmap(image)) // Clone the image to avoid GDI+ errors
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting image to Base64: {ex.Message}");
                return string.Empty;
            }
        }



        private Image Base64ToImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String)) return null;

            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Image img = Image.FromStream(ms))
                {
                    return new Bitmap(img); // Clone to avoid memory lock issues
                }
            }
        }

        // FORM RESIZING AND AERO SNAP

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int _ = 10; // you can rename this variable if you like

        // Snap threshold in pixels
        private const int SnapThreshold = 20;
        private const int ResizeBorderThickness = 10; // Border thickness for resizing

        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height); } }

        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }


        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }

        
    }
}
