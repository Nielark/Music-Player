using Modernial.Controls;
using NAudio.Wave;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Formats.Tar;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
using TagLib;
using static MusicPlayer.MyModule;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MusicPlayer
{
    public partial class FormMain : Form
    {
        public Image? miniPlayerMusicPicture;
        public string? miniPlayerTitle;
        public string? miniPlayerArtist;
        public string? miniPlayerMusicLen;
        public int miniPlayerTrackBarMax;

        public IWavePlayer? waveOutDevice;
        public AudioFileReader? audioFileReader;

        public int currentMusicIndex = -1;
        public int scrollSpeed = 5; // Adjust this for scrolling speed
        public int tempVolume = 100;  // Store previous volume value when muted
        private int targetTopPosition;
        public bool playMusic = false, repeatMusic = false, shuffleMusic = false, mute = false, fullScreen = false, miniPlayer = false;
        public List<MyMusic> musicList = new List<MyMusic>();
        private List<MyMusic> tempMusicList = new List<MyMusic>();
        private List<string> importMusic = new List<string>();
        public List<MyMusic> playMusicQueue = new List<MyMusic>();
        public List<MyMusic> shuffleMusicList = new List<MyMusic>();

        private Size formSize; //Keep form size when it is minimized and restored.Since the form is resized because it takes into account the size of the title bar and borders.
        private int borderSize = 2;
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
            this.SetStyle(ControlStyles.ResizeRedraw, true); // this is to avoid visual artifacts
            //this.Padding = new Padding(2, 2, 3, 3);
            ApplyRoundedCorners();
            btnHome.BackColor = Color.FromArgb(47, 53, 64);
            btnHome.ForeColor = Color.White;
            MyModule.MakeCircular(PicBoxPlayAndPause);      // Make the play or pause button circular
            previousWidth = this.ClientSize.Width;
            TbVolume.Value = 100;   // Set 100 as the default value for volume
            //CbSortMusic.SelectedIndex = 0;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            formSize = this.ClientSize;
            LoadMusicFiles();   // Loads the music from the device storage
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

            // Display text in the combo box
            CbFilterArtist.Text = "Filter by Artist";
            CbFilterAlbum.Text = "Filter by Album";

            // Subscribe to the event to update the UI when play/pause state changes
            PlayAndPauseStateChanged += OnPlayAndPauseStateChanged;
            ShuffleMusicStateChanged += OnShuffleMusicStateChanged;
            RepeatMusicStateChanged += OnRepeatMusicStateChanged;
            MuteStateChanged += OnMuteStateChanged;
            UpdateMuteStateChanged += OnUpdateMuteStateChanged;
            UpdateVolumeValueChanged += OnUpdateVolumeValueChanged;
            //UpdateVolumeIconChanged += OnUpdateVolumeIconChanged;
        }





        private void PnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void LoadMusicFiles()
        {
            // Define a list of music files to store the music
            List<string> musicFiles = GetMusicFiles();

            // Do something with the collected music files (e.g., populate a DataGridView or ListBox)
            if (musicFiles.Count != 0)
            {
                foreach (string file in musicFiles)
                {
                    try
                    {
                        GetFileMetaData(file);
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the error, skip this file, and continue processing others
                        //Console.WriteLine($"Error processing file {file}: {ex.Message}");
                        label1.Text = $"Error processing file {file}: {ex.Message}";
                    }
                }

                DgvMusicList.DataSource = null;

                tempMusicList = new List<MyMusic>(musicList);   // Copy the music list from musicList to tempMusicList
                DgvMusicList.DataSource = musicList;            // Set musicList as the datasource to display in data grid view           
                ModifyDvgMusicList(DgvMusicList);               // Function call for customizing the data grid 
            }
            else
            {
                MessageBox.Show("No music files found.");
            }
        }

        private List<string> GetMusicFiles()
        {
            // Define a list of music files to stored the music
            List<string> musicFiles = new List<string>();

            if (importMusic != null) musicFiles.AddRange(importMusic);

            // Define a list of directories to scan for music
            List<string> musicDirectories = new List<string>
            {
               Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), // Default Music folder
                @"C:\Users\Mark Daniel\Downloads"   // Downloads folder
                //@"D:\CustomMusicFolder"               // Any other custom folder
            };

            foreach (string directory in musicDirectories)
            {
                if (Directory.Exists(directory))
                {
                    // Get all music files (.mp3 and .wav) from the directory and subdirectories
                    musicFiles.AddRange(Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
                                                      .Where(file => file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                                                      .OrderBy(file => Path.GetFileName(file))
                                                      .ToArray());
                }
            }

            return musicFiles;
        }

        private void GetFileMetaData(string file)
        {
            Image musicPictureSmall, musicPictureMedium;

            var tagFile = TagLib.File.Create(file);

            if (tagFile.Tag.Pictures.Length > 0)
            {
                var bin = (byte[])(tagFile.Tag.Pictures[0].Data.Data);

                using (var ms = new MemoryStream(bin))
                {
                    Image originalImage = Image.FromStream(ms);

                    // Resize the image to the desired dimensions
                    musicPictureSmall = new Bitmap(30, 30);
                    using (Graphics g = Graphics.FromImage(musicPictureSmall))
                    {
                        // Set the interpolation mode for better quality
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(originalImage, 0, 0, 30, 30);
                    }

                    musicPictureMedium = originalImage;
                }
            }
            else
            {
                // If there is no image from the metadata set a default image
                //string albumCoverPath = "C:/Users/Mark Daniel/Downloads/music2.png";
                //musicPictureSmall = Image.FromFile(albumCoverPath);
                musicPictureSmall = Properties.Resources.default_music_picture_small;
                musicPictureMedium = Properties.Resources.default_music_picture_medium;
                //musicPictureMedium = Image.FromFile("C:/Users/Mark Daniel/Downloads/music.png");
            }

            // Get the following information from the file's meta data
            string title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(file);     // Get the song title, or use the file name if the title is not available
            string artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist";                 // Get the artist name, or use "Unknown Artist" if not available   
            string album = tagFile.Tag.Album ?? "Unknown Album";                            // Get the album name, or use "Unknown Album" if not available
            TimeSpan duration = tagFile.Properties.Duration;                                // Get the song's duration

            // Create a new MyMusic object using the gathered metadata and add it to the music list
            MyMusic myMusic = new MyMusic(musicPictureSmall, musicPictureMedium, title, artist, album, duration, file);
            musicList.Add(myMusic); // Add the new music item to the music list
        }

        private void BtnImportMusic_Click(object sender, EventArgs e)
        {
            if (ofdMusic.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofdMusic.FileNames)
                {
                    importMusic.Add(file);
                }

                musicList.Clear();
                LoadMusicFiles();
            }
        }

        private void CbSortMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? currentFilePath = null;

            // Store the file path of the currently playing music if it's valid
            if (currentMusicIndex > -1) currentFilePath = musicList[currentMusicIndex].File;

            // Perform sorting based on the selected index in the combo box
            switch (CbSortMusic.SelectedIndex)
            {
                case 0:
                    musicList = musicList.OrderBy(x => x.Title).ToList(); // Sort by title in ascending
                    break;
                case 1:
                    musicList = musicList.OrderByDescending(x => x.Title).ToList(); // Sort by title in descending
                    break;
                case 2:
                    musicList = musicList.OrderBy(x => x.Artist).ToList(); // Sort by artist in ascending
                    break;
                case 3:
                    musicList = musicList.OrderByDescending(x => x.Artist).ToList(); // Sort by artist in descending
                    break;
                case 4:
                    musicList = musicList.OrderBy(x => x.Duration).ToList(); // Sort by duration in ascending
                    break;
                case 5:
                    musicList = musicList.OrderByDescending(x => x.Duration).ToList(); // Sort by duration in descending
                    break;
                default:
                    // Handle invalid selection
                    break;
            }

            // Restore the current music index if not shuffled and if the file path is valid
            //if (!shuffleMusic && !string.IsNullOrEmpty(currentFilePath))
            //{
            //    currentMusicIndex = musicList.FindIndex(index => index.File == currentFilePath);
            //}

            DgvMusicList.DataSource = null;         // Clear the data in data grid view
            DgvMusicList.DataSource = musicList;    // Display music in data grid view
            ModifyDvgMusicList(DgvMusicList);       // Function call for customizing the music list data grid view     
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                // Filter the music list based on the title or artist matching the search text
                // The search is case-insensitive
                var filteredMusicList = musicList
                                    .Where(s => s.Title.Contains(TxtSearch.Text, StringComparison.OrdinalIgnoreCase) || // Match by title
                                           s.Artist.Contains(TxtSearch.Text, StringComparison.OrdinalIgnoreCase))       // Match by artist
                                    .OrderBy(s => s.Title)  // Sort the results alphabetically by title
                                    .ToList();              // Convert the result to a list

                DgvMusicList.DataSource = filteredMusicList;    // Set the search music as the data source
            }
            else
            {
                DgvMusicList.DataSource = musicList;    // Show all music when search bar is empty
            }

            ModifyDvgMusicList(DgvMusicList);   // Function call for customizing the music list data grid view     
        }

        private void CbFilterArtist_SelectedIndexChanged(object sender, EventArgs e)
        {
            musicList = new List<MyMusic>(tempMusicList);   // Copy the music list from the tempMusicList to musicList

            // Filter the music by artist name
            if (CbFilterArtist.SelectedIndex != 0)
            {
                CbFilterAlbum.SelectedIndex = 0;    // Set the album filter combo box to default value

                // Filter the music based on the selected artist name in the artist combo box
                musicList = musicList
                        .Where(s => s.Artist.Equals(CbFilterArtist.SelectedValue))  // Match by the selected artist
                        .ToList();  // Convert the filtered results to a list
            }

            DgvMusicList.DataSource = musicList;    // Set the filtered music as the data source 

            ModifyDvgMusicList(DgvMusicList);       // Function call for customizing the music list data grid view     
        }

        private void CbFilterAlbum_SelectedIndexChanged(object sender, EventArgs e)
        {
            musicList = new List<MyMusic>(tempMusicList);   // Copy the music list from the tempMusicList to musicList

            // Filter the music by album name
            if (CbFilterAlbum.SelectedIndex != 0)
            {
                CbFilterArtist.SelectedIndex = 0;   // Set the album filter combo box to default value

                // Filter the music base onn the selected album name in the album combo box
                musicList = musicList
                        .Where(s => s.Album.Equals(CbFilterAlbum.SelectedValue))    // Match by the selected artist
                        .ToList();  // Convert the filtered results to a list
            }

            DgvMusicList.DataSource = musicList;    // Set the filtered music as the data source

            ModifyDvgMusicList(DgvMusicList);       // Function call for customizing the music list data grid view  
        }

        private void BtnShuffleAndPlay_Click(object sender, EventArgs e)
        {
            shuffleMusic = true;

            playMusicQueue = new List<MyMusic>(musicList);      // Copy the current music list to playMusicQueue for shuffling

            FisherYatesShuffle();                               // Shuffle the music list using the Fisher-Yates algorithm
            DgvPlayMusicQueue.DataSource = shuffleMusicList;    // Set shuffleMusicList as the data source and display in data grid view

            // Get the music file information from the first index in the shuffled list
            string filePath = shuffleMusicList[0].File;
            string Title = shuffleMusicList[0].Title;
            string Artist = shuffleMusicList[0].Artist;

            int index = shuffleMusicList.FindIndex(index => index.File == filePath);    // Find the index of the current music in the shuffled list based on file path
            Image musicPicture = shuffleMusicList[index].MusicPictureMedium;            // Get the music image

            PlayMusic(musicPicture, filePath, Title, Artist);   // Function call to play the music

            // Adjust and display the player controls panel
            // PnlPlayerControls.Location = new Point(0, 470);
            //PnlPlayerControls.Dock = DockStyle.Bottom;
            TimerShowPnlPlayerControls.Start();
        }

        private void BtnClearQueue_Click(object sender, EventArgs e)
        {
            // Clear the play queue and reset the UI
            if (audioFileReader != null && waveOutDevice != null)
            {
                // Stop and dispose of the audio playback
                waveOutDevice.Stop();
                waveOutDevice.Dispose();
                audioFileReader.Dispose();
                waveOutDevice = null;
                audioFileReader = null;

                // Reset the seek bar and duration labels
                TbSeekMusic.Value = 0;
                LblMusicLength.Text = TimeSpan.FromSeconds(0).ToString("hh\\:mm\\:ss");
                LblMusicDurationCtr.Text = TimeSpan.FromSeconds(0).ToString("hh\\:mm\\:ss");

                // Clear the playback information display
                LblShowPlayTitle.Text = null;
                LblShowPlayArtist.Text = null;
                PicBoxShowPlayPicture.Image = Properties.Resources.default_music_picture_medium;

                // Reset the marquee labels
                MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
                ResetAndStopMarqueeSettings(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            }

            // Clear the data grid view of the play queue
            DgvPlayMusicQueue.DataSource = null;        // clear the data grid view of play list queue
            playMusicQueue.Clear();                     // Clear the play queue list
            ModifyDvgMusicList(DgvPlayMusicQueue);      // Update the DataGridView display of the play list queue
        }

        // SIDE PANEL BUTTONS

        private void BtnHome_Click(object sender, EventArgs e)
        {
            UpdateSideBarButtonUI(btnHome, pnlSelectSign, btnMusicLibrary, btnPlayQueue, btnPlayLists, btnSongs, btnArtists, btnAlbums);
            pnlSubSelectSign.Visible = false;
        }

        private void BtnMusicLibrary_Click(object sender, EventArgs e)
        {
            ModifyDvgMusicList(DgvMusicList);   // Update the DataGridView display of the music list
            UpdateSideBarButtonUI(btnMusicLibrary, pnlSelectSign, btnHome, btnPlayQueue, btnPlayLists, btnSongs, btnArtists, btnAlbums);
            DisplayOrHideSubButtons();

            // Display the filter controls for the music list and hide play list queue controls
            PnlSortControls.Visible = true;
            PnlPlayMusicQueue.Visible = false;

            // Display the music list and hide the play list queue data grid view
            PnlMusicList.Visible = true;
            PnlPlayMusicQueue.Visible = false;
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

            // Display the filter controls for the play list queue and hide music list controls
            PnlPlayQueueControl.Visible = true;
            PnlSortControls.Visible = false;

            // Display the play list queue and hide the music list queue data grid view
            PnlPlayMusicQueue.Visible = true;
            PnlMusicList.Visible = false;
        }

        private void BtnPlayLists_Click(object sender, EventArgs e)
        {
            UpdateSideBarButtonUI(btnPlayLists, pnlSelectSign, btnHome, btnMusicLibrary, btnPlayQueue, btnSongs, btnArtists, btnAlbums);
            pnlSubSelectSign.Visible = false;
        }

        // MUSIC LIBRARY SUB BUTTONS

        private void BtnSongs_Click(object sender, EventArgs e)
        {
            UpdateSideBarButtonUI(btnSongs, pnlSubSelectSign ,btnArtists, btnAlbums);
            UpdateSideBarButtonUI(btnMusicLibrary, pnlSelectSign, btnHome, btnPlayQueue, btnPlayLists, btnArtists, btnAlbums);
            pnlSubSelectSign.Visible = true;
        }

        private void BtnArtists_Click(object sender, EventArgs e)
        {
            UpdateSideBarButtonUI(btnArtists, pnlSubSelectSign, btnSongs, btnAlbums);
            UpdateSideBarButtonUI(btnMusicLibrary, pnlSelectSign, btnHome, btnPlayQueue, btnPlayLists, btnSongs, btnAlbums);
            pnlSubSelectSign.Visible = true;
        }

        private void BtnAlbums_Click(object sender, EventArgs e)
        {
            UpdateSideBarButtonUI(btnAlbums, pnlSubSelectSign, btnSongs, btnArtists);
            UpdateSideBarButtonUI(btnMusicLibrary, pnlSelectSign, btnHome, btnPlayQueue, btnPlayLists, btnSongs, btnArtists);
            pnlSubSelectSign.Visible = true;
        }

        private void UpdateSideBarButtonUI(System.Windows.Forms.Button targetButton, System.Windows.Forms.Panel pnl, params System.Windows.Forms.Button[] Buttons)
        {
            int paddingTop = 8;
            pnl.Top = targetButton.Top + paddingTop;  // Set the position base on the selected button

            //targetButton.ForeColor = Color.White;

            // Loop to set the default appearance of the side bar buttons
            foreach (var btn in Buttons)
            {
                // Reset the side bar button's color if not selected
                btn.BackColor = Color.FromArgb(12, 23, 45);

                // Reset the side bar font's color if not selected
                btn.ForeColor = Color.FromName("ScrollBar");
            }
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

                //dataGridViewMusic.RowTemplate.Height = 40;
                // Dynamically adjust the height of the DataGridView based on the number of rows
                int rowHeight = dataGridViewMusic.RowTemplate.Height; // The height of one row
                int totalHeight = dataGridViewMusic.Rows.Count * rowHeight + dataGridViewMusic.ColumnHeadersHeight;

                // Set DataGridView height dynamically
                dataGridViewMusic.Height = totalHeight > PnlMusicList.Height ? totalHeight : PnlMusicList.Height;
            }
        }

        private void DgvMusicList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Play's the selected music through cell click and populate the playlist queue
            if (DgvMusicList.CurrentRow != null)
            {
                // Filter the music based on the selected artist and album from the combo boxes
                // Selects all the music when both combo boxes are at their default values (index 0)
                playMusicQueue = (CbFilterArtist.SelectedIndex == 0 && CbFilterAlbum.SelectedIndex == 0)
                                    ? new List<MyMusic>(musicList)  // Create a new list with all music
                                    : musicList
                                        .Where(s => s.Artist.Equals(CbFilterArtist.SelectedValue) || s.Album.Equals(CbFilterAlbum.SelectedValue))   // Filter based on selected artist or album
                                        .ToList();  // Convert the filtered results to a list

                // Retrieve every cells data from the selected row
                string? title = DgvMusicList.CurrentRow.Cells[2].Value.ToString();
                string? artist = DgvMusicList.CurrentRow.Cells[3].Value.ToString();
                string? filePath = DgvMusicList.CurrentRow.Cells[6].Value.ToString();

                // Find the index of the selected music using the file path
                int index = playMusicQueue.FindIndex(index => index.File == filePath);
                // Check if the file path was found 
                if (index != -1)
                {
                    Image musicPicture = playMusicQueue[index].MusicPictureMedium;  // Get the mp3 image

                    // Play's the mp3 when all data has value
                    if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(artist))
                    {
                        PlayMusic(musicPicture, filePath, title, artist);
                    }
                }

                if (shuffleMusic)
                {
                    FisherYatesShuffle(filePath);                       // Shuffle the music list
                    DgvPlayMusicQueue.DataSource = shuffleMusicList;    // Set the shuffleMusicList music as the data source 
                }
                else
                {
                    DgvPlayMusicQueue.DataSource = playMusicQueue;      // Set the shuffleMusicList music as the data source (not shuffle)
                }

                // Display the play back controls
                TimerShowPnlPlayerControls.Start();
            }
        }

        private void DgvPlayMusicQueue_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Play's the selected music through cell click 
            if (DgvPlayMusicQueue.CurrentRow != null)
            {
                // Get every cells data of the selected row
                string? title = DgvPlayMusicQueue.CurrentRow.Cells[2].Value.ToString();
                string? artist = DgvPlayMusicQueue.CurrentRow.Cells[3].Value.ToString();
                string? filePath = DgvPlayMusicQueue.CurrentRow.Cells[6].Value.ToString();

                // Find the index of the selected music using the file path
                int index = playMusicQueue.FindIndex(index => index.File == filePath);
                if (index != -1)
                {
                    Image musicPicture = playMusicQueue[index].MusicPictureMedium;  // Get the mp3 image

                    // Play's the music when all necessary data is available
                    if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(artist))
                    {
                        PlayMusic(musicPicture, filePath, title, artist);   // Function call to play the selected music
                    }
                }
            }
        }

        // PLAYBACK CONTROLS

        private void PicBoxShuffleMusic_Click(object sender, EventArgs e)
        {
            ShuffleMusic();
        }

        public delegate void ShuffleMusicStateChangedHandler(bool isShuffled);
        public event ShuffleMusicStateChangedHandler? ShuffleMusicStateChanged;

        private void PicBoxSkipBackward_Click(object sender, EventArgs e)
        {
            SkipBackward();
        }

        private void PicBoxPlayPreviousMusic_Click(object sender, EventArgs e)
        {
            PlayPreviousMusic();
        }


        private void PicBoxPlayAndPause_Click(object sender, EventArgs e)
        {
            TogglePlayAndPause();
        }

        public delegate void PlayAndPauseStateChangedHandler(bool isPlaying);
        public event PlayAndPauseStateChangedHandler? PlayAndPauseStateChanged;

        private void PicBoxPlayNextMusic_Click(object sender, EventArgs e)
        {
            PlayNextMusic();
        }

        private void PicBoxSkipForward_Click(object sender, EventArgs e)
        {
            SkipForward();
        }

        private void PicBoxRepeatMusic_Click(object sender, EventArgs e)
        {
            RepeatMusic();
        }

        public delegate void RepeatMusicStateChangedHandler(bool isRepeat);
        public event RepeatMusicStateChangedHandler? RepeatMusicStateChanged;

        public void ShuffleMusic()
        {
            if (playMusicQueue.Count > 0)   // Ensure there are songs in the queue
            {
                string currentFilePath;

                shuffleMusic = !shuffleMusic;           // Toggle shuffle mode
                //label1.Text = shuffleMusic.ToString();  // Update label to reflect shuffle state

                // Shuffle or revert to original list
                if (shuffleMusic)
                {
                    currentFilePath = playMusicQueue[currentMusicIndex].File;       // Get the file path of the current playing mp3 from playMusicQueue
                    FisherYatesShuffle(currentFilePath);                            // Shuffle the music list, preserving the current song
                    DgvPlayMusicQueue.DataSource = shuffleMusicList;                // Display shuffled list in the 
                }
                else
                {
                    currentFilePath = shuffleMusicList[currentMusicIndex].File;     // Get the file path of the current playing mp3 from shuffleMusicList
                    DgvPlayMusicQueue.DataSource = musicList;                       // Revert to the original (unshuffled) music list in the DataGridView

                    currentMusicIndex = playMusicQueue.FindIndex(index => index.File == currentFilePath);   // Find the index of the current song in the unshuffled playMusicQueue

                    // If the song is not found in the unshuffled list, select the first song
                    if (currentMusicIndex == -1)
                    {
                        currentMusicIndex = 0;
                    }
                }

                ShuffleMusicStateChanged?.Invoke(shuffleMusic);
            }
        }

        private void OnShuffleMusicStateChanged(bool isShuffle)
        {
            UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, isShuffle);
        }

        public void UpdateShuffleUI(PictureBox PicBoxShuffleMusic, System.Windows.Forms.ToolTip toolTipPlayerControl, bool isShuffle)
        {
            // Update the UI of MiniPlayer based on the new play/pause state
            if (isShuffle)
            {
                PicBoxShuffleMusic.Image = Properties.Resources.shuffle_off;    // Change the image into shuffle off icon
                toolTipPlayerControl.SetToolTip(PicBoxShuffleMusic, "Shuffle off");    // Change the tool tip text to shuffle off
            }
            else
            {
                PicBoxShuffleMusic.Image = Properties.Resources.shuffle_on;     // Change the image into shuffle on icon
                toolTipPlayerControl.SetToolTip(PicBoxShuffleMusic, "Shuffle on");    // Change the tool tip text to shuffle on
            }
        }

        public void SkipBackward()
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                waveOutDevice.Pause();  // Pause the music playback

                TimeSpan skipValue = TimeSpan.FromSeconds(50);                  // Define the amount of time to skip backward
                TimeSpan totalValue = audioFileReader.CurrentTime - skipValue;  // Calculate the new time after skipping

                // Check if the new time is greater than 0
                if (totalValue > TimeSpan.Zero)
                {
                    audioFileReader.CurrentTime = totalValue;       // If greater than 0, update the current time to the new value
                }
                else
                {
                    audioFileReader.CurrentTime = TimeSpan.Zero;    // If less than or equal to 0, set the current time to the start of the track
                }

                waveOutDevice.Play();   // Resume music playback
            }
        }

        public void PlayPreviousMusic()
        {
            // Play previous track
            if (currentMusicIndex > 0)
            {
                currentMusicIndex--;            // Move to the previous track in the queue
                NextAndPreviousMusicHandler();  // Function call to play the next or previous music
            }
        }

        public void TogglePlayAndPause()
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                // Toggle between playing and pausing the music
                if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    waveOutDevice.Pause();      // Pause the music
                    playMusic = false;          // Update state to indicate music is paused
                }
                else
                {
                    waveOutDevice.Play();       // Play the music
                    playMusic = true;           // Update state to indicate music is playing
                }

                // Trigger the event to notify that play/pause state has changed
                PlayAndPauseStateChanged?.Invoke(playMusic);
            }
        }

        private void OnPlayAndPauseStateChanged(bool isPlaying)
        {
            UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, isPlaying);
        }

        public void UpdatePlayAndPauseUI(PictureBox PicBoxPlayAndPause, System.Windows.Forms.ToolTip toolTipPlayerControl, bool isPlaying)
        {
            // Update the UI of MiniPlayer based on the new play/pause state
            if (isPlaying)
            {
                PicBoxPlayAndPause.Image = Properties.Resources.pause;          // Change into pause icon
                toolTipPlayerControl.SetToolTip(PicBoxPlayAndPause, "Pause");   // Change the tool tip text to Pause
            }
            else
            {
                PicBoxPlayAndPause.Image = Properties.Resources.play;           // Change the image into play icon
                toolTipPlayerControl.SetToolTip(PicBoxPlayAndPause, "Play");    // Change the tool tip text to Play
            }
        }

        public void PlayNextMusic()
        {
            // Play the next track in the queue
            if (currentMusicIndex < playMusicQueue.Count - 1)
            {
                currentMusicIndex++;            // Move to the next track in the queue
                NextAndPreviousMusicHandler();  // Function call to handle playing the next or previous track
            }
        }

        public void SkipForward()
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                waveOutDevice.Pause();  // Pause the music playback

                TimeSpan skipValue = TimeSpan.FromSeconds(50);                  // Define the amount of time to skip forward
                TimeSpan totalValue = audioFileReader.CurrentTime + skipValue;  // Calculate the new time after skipping

                // Check if the new time exceeds the track's total duration
                if (totalValue < audioFileReader.TotalTime)
                {
                    audioFileReader.CurrentTime = totalValue;   // If not exceeding, update the current time to the new value
                }
                else
                {
                    audioFileReader.CurrentTime = audioFileReader.TotalTime;    // If exceeding, set the current time to the track's total duration
                }

                waveOutDevice.Play();   // Resume music playback
            }
        }

        public void RepeatMusic()
        {
            repeatMusic = !repeatMusic;     // Toggle repeat music

            RepeatMusicStateChanged?.Invoke(repeatMusic);
        }

        private void OnRepeatMusicStateChanged(bool isRepeat)
        {
            UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, isRepeat);
        }

        public void UpdateRepeatUI(PictureBox PicBoxRepeatMusic, System.Windows.Forms.ToolTip toolTipPlayerControl, bool isRepeat)
        {
            if (isRepeat)
            {
                PicBoxRepeatMusic.Image = Properties.Resources.repeat_off;          // Change the image into repeat off icon
                toolTipPlayerControl.SetToolTip(PicBoxRepeatMusic, "Repeat off");   // Change the tool tip text to Repeat off
            }
            else
            {
                PicBoxRepeatMusic.Image = Properties.Resources.repeat;              // Change the image into repeat on icon
                toolTipPlayerControl.SetToolTip(PicBoxRepeatMusic, "Repeat on");    // Change the tool tip text to Repeat on
            }
        }

        private void PicBoxShowVolumeBar_Click(object sender, EventArgs e)
        {
            // Display the volume bar
            PnlVolumeControl.Visible = true;
        }

        private void PicBoxVolumePicture_Click(object sender, EventArgs e)
        {
            VolumeMuteAndUnmuteToggle();
        }

        public delegate void MuteStateChangedHandler(bool isMute);
        public event MuteStateChangedHandler? MuteStateChanged;

        public void VolumeMuteAndUnmuteToggle()
        {
            if (audioFileReader != null)
            {
                mute = !mute;   // Toggle mute status

                MuteStateChanged?.Invoke(mute);
            }
        }

        private void OnMuteStateChanged(bool isMute)
        {
            UpdateVolumeValueAndUI(TbVolume, isMute);
        }

        public void UpdateVolumeValueAndUI(DungeonTrackBar TbVolume, bool isMute)
        {
            // Check and mute or unmute the volume
            if (isMute)
            {
                tempVolume = TbVolume.Value;        // Temporary store the current volume value before muting
                TbVolume.Value = 0;                 // Set the trackbar to 0
            }
            else
            {
                TbVolume.Value = tempVolume;        // Restore the previous value when unmuted
            }
        }

        private void PnlVolumeControl_MouseLeave(object sender, EventArgs e)
        {
            PnlVolumeControlMousePositionHandler(PnlVolumeControl, PicBoxVolumePicture, TbVolume, LblVolumeValue);
        }

        public void PnlVolumeControlMousePositionHandler(System.Windows.Forms.Panel PnlVolumeControl, PictureBox PicBoxVolumePicture, DungeonTrackBar TbVolume, Label LblVolumeValue)
        {
            Point mousePosition = Control.MousePosition;

            // Convert all controls' bounds to screen coordinates
            Rectangle panelBounds = PnlVolumeControl.RectangleToScreen(PnlVolumeControl.ClientRectangle);
            Rectangle pictureBounds = PicBoxVolumePicture.RectangleToScreen(PicBoxVolumePicture.ClientRectangle);
            Rectangle trackBarBounds = TbVolume.RectangleToScreen(TbVolume.ClientRectangle);
            Rectangle labelBounds = LblVolumeValue.RectangleToScreen(LblVolumeValue.ClientRectangle);

            // Check if the mouse is outside all the controls (panel and its children)
            if (!panelBounds.Contains(mousePosition) &&
                !pictureBounds.Contains(mousePosition) &&
                !trackBarBounds.Contains(mousePosition) &&
                !labelBounds.Contains(mousePosition))
            {
                PnlVolumeControl.Visible = false;   // Hide the volume bar
            }
        }

        private void TbVolume_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateMuteState();
        }

        public delegate void UpdateMuteStateChangeHandler();
        public event UpdateMuteStateChangeHandler? UpdateMuteStateChanged;

        public void UpdateMuteState()
        {
            UpdateMuteStateChanged?.Invoke();
        }

        private void OnUpdateMuteStateChanged()
        {
            UpdateMuteStateUI(TbVolume);
        }

        public void UpdateMuteStateUI(DungeonTrackBar TbVolume)
        {
            if (TbVolume.Value > 0)
            {
                mute = false;
                tempVolume = TbVolume.Value;    // Temporary store the current volume value
            }
            else
            {
                mute = true;
            }
        }

        private void TbVolume_ValueChanged()
        {
            // Update volume and icon based on the current trackbar value
            UpdateVolumeValue(TbVolume.Value);
            //UpdateVolumeIcon();
            UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);
        }

        public delegate void UpdateVolumeValueChangedHandler(int newVolumeValue);
        public event UpdateVolumeValueChangedHandler? UpdateVolumeValueChanged;
        //public delegate void UpdateVolumeIconChangedHandler();
        //public event UpdateVolumeIconChangedHandler? UpdateVolumeIconChanged;

        public void UpdateVolumeValue(int newVolumeValue)
        {
            UpdateVolumeValueChanged?.Invoke(newVolumeValue);
        }

        private void OnUpdateVolumeValueChanged(int newVolumeValue)
        {
            TbVolume.Value = newVolumeValue;
            UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);
        }

        public void UpdateVolumeValueAndUI(Label LblVolumeValue, DungeonTrackBar TbVolume)
        {
            if (audioFileReader != null)
            {
                LblVolumeValue.Text = $"{TbVolume.Value}";  // Display the updated volume value based on the volume trackbar
                float volumeValue = TbVolume.Value / 100f;  // Convert the volume trackbar value (0-100) into a float (0.0 - 1.0)  
                audioFileReader.Volume = volumeValue;       // Set the audio file reader's volume to the calculated value
            }
        }

        //public void UpdateVolumeIcon()
        //{
        //    UpdateVolumeIconChanged?.Invoke();
        //}

        //private void OnUpdateVolumeIconChanged()
        //{
        //    UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);
        //}

        public void UpdateVolumeIconUI(DungeonTrackBar TbVolume, PictureBox PicBoxVolumePicture, PictureBox PicBoxShowVolumeBar)
        {
            // Change the volume icon based on the value
            if (TbVolume.Value == 0)
            {
                // Change into mute icon
                PicBoxVolumePicture.Image = Properties.Resources.volume_mute;
                PicBoxShowVolumeBar.Image = Properties.Resources.volume_mute;
            }
            else if (TbVolume.Value > 0 && TbVolume.Value <= 33)
            {
                // Change into low volume icon
                PicBoxVolumePicture.Image = Properties.Resources.volume_low;
                PicBoxShowVolumeBar.Image = Properties.Resources.volume_low;
            }
            else if (TbVolume.Value > 33 && TbVolume.Value <= 66)
            {
                // Change into medium volume icon
                PicBoxVolumePicture.Image = Properties.Resources.volume_medium;
                PicBoxShowVolumeBar.Image = Properties.Resources.volume_medium;
            }
            else
            {
                // Change into high volume icon
                PicBoxVolumePicture.Image = Properties.Resources.volume_high;
                PicBoxShowVolumeBar.Image = Properties.Resources.volume_high;
            }
        }

        public void FisherYatesShuffle(string? currentFilePath = null)
        {
            // Fisher-Yates Shuffle (aka Knuth Shuffle)
            Random random = new Random();

            // Copy the music list from playMusicQueue to shuffleMusicList
            shuffleMusicList = new List<MyMusic>(playMusicQueue);

            // Shuffle the music
            for (int i = shuffleMusicList.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(0, i + 1);                // Generate a random index between 0 and i inclusively

                MyMusic tempMusicList = shuffleMusicList[i];            // Initialize a temporary list to temporary store the current song at index i
                shuffleMusicList[i] = shuffleMusicList[randomIndex];    // Swap the current song with the song at the random index
                shuffleMusicList[randomIndex] = tempMusicList;          // Place the temporarily stored song at the random index
            }

            // If currently playing a song, swap it at the first index
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                for (int i = 0; i < shuffleMusicList.Count; i++)
                {
                    // Check if the file path is in the list and identify the index position
                    if (currentFilePath == shuffleMusicList[i].File)
                    {
                        MyMusic tempMusicList = shuffleMusicList[0];    // Initialize a temporary list to temporary store the current song at the first index
                        shuffleMusicList[0] = shuffleMusicList[i];      // Place the current playing song at the first index
                        shuffleMusicList[i] = tempMusicList;            // Place the temporary stored song at the identified index position
                        break;
                    }
                }
            }

            currentMusicIndex = 0;  // initialize back to 0 after shuffle
        }

        public void NextAndPreviousMusicHandler()
        {
            if (playMusicQueue.Count > 0)   // Ensure there are songs in the queue
            {
                string filePath, title, artist;
                Image musicPicture;

                // Play the next or previous song, considering shuffle mode
                if (!shuffleMusic)
                {
                    // Retrieve the music info from the non-shuffled queue
                    musicPicture = playMusicQueue[currentMusicIndex].MusicPictureMedium;
                    filePath = playMusicQueue[currentMusicIndex].File;
                    title = playMusicQueue[currentMusicIndex].Title;
                    artist = playMusicQueue[currentMusicIndex].Artist;

                    TbSeekMusic.Value = 0;                              // Reset the track bar to the beginning of the song
                    PlayMusic(musicPicture, filePath, title, artist);   // Function call to play the music
                }
                else
                {
                    // Retrieve the music info from the non-shuffled queue
                    musicPicture = shuffleMusicList[currentMusicIndex].MusicPictureMedium;
                    filePath = shuffleMusicList[currentMusicIndex].File;
                    title = shuffleMusicList[currentMusicIndex].Title;
                    artist = shuffleMusicList[currentMusicIndex].Artist;

                    TbSeekMusic.Value = 0;                              // Reset the track bar to the beginning of the song
                    PlayMusic(musicPicture, filePath, title, artist);   // Function call to play the music
                }
            }
        }

        public void PlayMusic(Image musicPicture, string filePath, string title, string artist)
        {
            if (playMusicQueue != null)
            {
                // Reset label settings and stop any running marquee timers
                ResetAndStopMarqueeSettings(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
                //LblShowPlayTitle.AutoSize = true;
                //LblShowPlayArtist.AutoSize = true;
                //TimerTitleMarquee.Stop(); // Stop the timer
                //TimerArtistMarquee.Stop(); // Stop the timer
                //LblShowPlayTitle.Left = 0;
                //LblShowPlayArtist.Left = 0;

                // Find the index of the song in the queue (shuffled or not) based on file path
                if (!shuffleMusic)
                {
                    currentMusicIndex = playMusicQueue.FindIndex(index => index.File == filePath);
                }
                else
                {
                    currentMusicIndex = shuffleMusicList.FindIndex(index => index.File == filePath);
                }

                // Stop and dispose of any currently playing music
                if (waveOutDevice != null)
                {
                    waveOutDevice.Stop();
                    waveOutDevice.Dispose();
                    waveOutDevice = null;
                }

                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                    audioFileReader = null;
                }

                waveOutDevice = new WaveOutEvent();                 // Create new playback device
                audioFileReader = new AudioFileReader(filePath);    // Read the audio file
                waveOutDevice.Init(audioFileReader);                // Initialize playback with audio file
                waveOutDevice.Play();                               // Start playing the music
                UpdateVolumeValue(TbVolume.Value);                             // Update volume

                // Display music image or a default one if no image is available
                if (musicPicture != null)
                {
                    PicBoxShowPlayPicture.Image = musicPicture;
                    miniPlayerMusicPicture = musicPicture;
                }
                else
                {
                    PicBoxShowPlayPicture.Image = Properties.Resources.default_music_picture_medium;
                    //PicBoxShowPlayPicture.ImageLocation = "C:/Users/Mark Daniel/Downloads/music.png";
                    miniPlayerMusicPicture = Properties.Resources.default_music_picture_medium;
                }

                // Update UI with the currently playing song information
                LblShowPlayTitle.Text = title;
                LblShowPlayArtist.Text = artist;
                LblMusicLength.Text = audioFileReader.TotalTime.ToString("hh\\:mm\\:ss");

                miniPlayerTitle = title;
                miniPlayerArtist = artist;
                miniPlayerMusicLen = audioFileReader.TotalTime.ToString("hh\\:mm\\:ss");

                // Set the seek bar maximum value to the total duration of the music in seconds
                TbSeekMusic.Maximum = (int)audioFileReader.TotalTime.TotalSeconds;
                miniPlayerTrackBarMax = (int)audioFileReader.TotalTime.TotalSeconds;

                MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);

                TimerMusicDuration.Start();     // Start tracking the music duration in seek bar
                playMusic = true;               // Update play status
            }
        }

        public void ResetAndStopMarqueeSettings(Label LblShowPlayTitle, Label LblShowPlayArtist, System.Windows.Forms.Panel PnlMarquee, System.Windows.Forms.Timer TimerTitleMarquee, System.Windows.Forms.Timer TimerArtistMarquee)
        {
            // Reset the marquee labels
            //LblShowPlayTitle.AutoSize = true;
            //LblShowPlayArtist.AutoSize = true;
            //TimerTitleMarquee.Stop();   // Stop the title marquee timer
            //TimerArtistMarquee.Stop();  // Stop the artist marquee timer
            //LblShowPlayTitle.Left = 0;  // Reset the title label position
            //LblShowPlayArtist.Left = 0; // Reset the artist label position
        }

        public void MarqueeEffectHandler(Label LblShowPlayTitle, Label LblShowPlayArtist, System.Windows.Forms.Panel PnlMarquee, System.Windows.Forms.Timer TimerTitleMarquee, System.Windows.Forms.Timer TimerArtistMarquee)
        {
            // Handle the marquee effect based on label width and panel size
            if (LblShowPlayTitle.Right > PnlMarquee.Right && LblShowPlayArtist.Right < PnlMarquee.Right)
            {
                TimerTitleMarquee.Start(); // Start the title marquee timer
                TimerArtistMarquee.Stop();
            }
            else if (LblShowPlayTitle.Right < PnlMarquee.Right && LblShowPlayArtist.Right > PnlMarquee.Right)
            {
                TimerArtistMarquee.Start(); // Start the artist marquee timer
                TimerTitleMarquee.Stop();
            }
            else if (LblShowPlayTitle.Right > PnlMarquee.Right && LblShowPlayArtist.Right > PnlMarquee.Right)
            {
                // Ensure both labels have the same width if one is wider
                if (LblShowPlayTitle.Right < LblShowPlayArtist.Right)
                {
                    LblShowPlayTitle.AutoSize = false;                      // Disable auto size
                    LblShowPlayTitle.Width = LblShowPlayArtist.Width;       // Set the width of the title label equal to the artist label
                    LblShowPlayTitle.Invalidate();
                }
                else if (LblShowPlayArtist.Right < LblShowPlayTitle.Right)
                {
                    LblShowPlayArtist.AutoSize = false;                     // Disable auto size
                    LblShowPlayArtist.Width = LblShowPlayTitle.Width;       // Set the width of the artist label equal to the title label
                    LblShowPlayArtist.Invalidate();
                }

                // Start both marquees
                TimerTitleMarquee.Start();
                TimerArtistMarquee.Start();
            }
            else
            {
                // Reset the marquee labels
                LblShowPlayTitle.AutoSize = true;
                LblShowPlayArtist.AutoSize = true;
                TimerTitleMarquee.Stop();   // Stop the title marquee timer
                TimerArtistMarquee.Stop();  // Stop the artist marquee timer
                LblShowPlayTitle.Left = 0;  // Reset the title label position
                LblShowPlayArtist.Left = 0; // Reset the artist label position
            }
        }

        private void TimerTitleMarquee_Tick(object sender, EventArgs e)
        {
            TitleMarqueeEffectHandler(LblShowPlayTitle, PnlMarquee);
        }

        public void TitleMarqueeEffectHandler(Label LblShowPlayTitle, System.Windows.Forms.Panel PnlMarquee)
        {
            LblShowPlayTitle.Left -= scrollSpeed;

            // Check if the label has gone off-screen and wrap it back
            if (LblShowPlayTitle.Right < 0)
            {
                LblShowPlayTitle.Left = PnlMarquee.Width;
            }
        }

        private void TimerArtistMarquee_Tick(object sender, EventArgs e)
        {
            ArtistMarqueeEffectHandler(LblShowPlayArtist, PnlMarquee);
        }

        public void ArtistMarqueeEffectHandler(Label LblShowPlayArtist, System.Windows.Forms.Panel PnlMarquee)
        {
            LblShowPlayArtist.Left -= scrollSpeed;

            // Check if the label has gone off-screen and wrap it back
            if (LblShowPlayArtist.Right < 0)
            {
                LblShowPlayArtist.Left = PnlMarquee.Width;
            }
        }

        private void TimerMusicDuration_Tick(object sender, EventArgs e)
        {
            MusicSeekBarHandler(TbSeekMusic, LblMusicLength, LblMusicDurationCtr, TimerMusicDuration);
        }

        public void MusicSeekBarHandler(DungeonTrackBar TbSeekMusic, Label LblMusicLength, Label LblMusicDurationCtr, System.Windows.Forms.Timer TimerMusicDuration)
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                if (TbSeekMusic.Value < TbSeekMusic.Maximum)
                {
                    // Update seek bar and display the remaining time and current time
                    TbSeekMusic.Value = (int)audioFileReader.CurrentTime.TotalSeconds;
                    LblMusicLength.Text = $"{audioFileReader.TotalTime - audioFileReader.CurrentTime:hh\\:mm\\:ss}";
                    LblMusicDurationCtr.Text = $"{audioFileReader.CurrentTime:hh\\:mm\\:ss}";
                }
                else
                {
                    // Reset seek bar and stop the timer when the track ends
                    TbSeekMusic.Value = 0;
                    TimerMusicDuration.Stop();

                    // Stop the current playback and release resources
                    waveOutDevice.Stop();
                    waveOutDevice.Dispose();
                    waveOutDevice = null;

                    // Dispose of the audio file reader to release its resources
                    if (audioFileReader != null)
                    {
                        audioFileReader.Dispose();
                        audioFileReader = null;
                    }

                    // Move to the next track if repeat mode is disabled
                    if (!repeatMusic)
                    {
                        currentMusicIndex++;    // Go to the next track in the queue
                    }

                    NextAndPreviousMusicHandler();  // Play the next or previous track in the queue
                }
            }
        }

        private void TbSeekMusic_ValueChanged()
        {
            UpdateMusicLengthAndDuration(TbSeekMusic, LblMusicLength, LblMusicDurationCtr);
        }

        public void UpdateMusicLengthAndDuration(DungeonTrackBar TbSeekMusic, Label LblMusicLength, Label LblMusicDurationCtr)
        {
            if (audioFileReader != null)
            {
                // Update the remaining duration based on the seek bar's current value
                LblMusicLength.Text = $"{audioFileReader.TotalTime - TimeSpan.FromSeconds(TbSeekMusic.Value):hh\\:mm\\:ss}";

                // Update the current playback time based on the seek bar's position
                LblMusicDurationCtr.Text = $"{TimeSpan.FromSeconds(TbSeekMusic.Value)}";
            }
        }

        private void TbSeekMusic_MouseUp(object sender, MouseEventArgs e)
        {
            MusicSeekBarMouseUpHandler(TbSeekMusic);
        }

        public void MusicSeekBarMouseUpHandler(DungeonTrackBar TbSeekMusic)
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                // Update the current playback time of the track based on the seek bar's value
                audioFileReader.CurrentTime = TimeSpan.FromSeconds(TbSeekMusic.Value);

                // Resume playback if the music is currently playing
                if (playMusic)
                {
                    waveOutDevice.Play();
                }
            }
        }

        private void TbSeekMusic_MouseDown(object sender, MouseEventArgs e)
        {
            MusicSeekBarMouseDownHandler();
        }

        public void MusicSeekBarMouseDownHandler()
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                waveOutDevice.Stop();   // Stop the playback when the user presses down on the seek bar
            }
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
            //miniPlayer = !miniPlayer;

            //if (miniPlayer)
            //{
            //    PicBoxMiniPlayerToggle.Image = Properties.Resources.mini_player;
            //    toolTipPlayerControl.SetToolTip(PicBoxMiniPlayerToggle, "Minimize player");
            //}
            //else
            //{
            //    PicBoxMiniPlayerToggle.Image = Properties.Resources.exit_mini_player;
            //    toolTipPlayerControl.SetToolTip(PicBoxMiniPlayerToggle, "Exit mini player");
            //}

            Form MiniPlayer = new MiniPlayer(this);
            this.Hide();
            MiniPlayer.Show();
        }

        public void ReturnToMainForm()
        {
            this.Show();  // Show the main form when returning from MiniPlayer
        }

        private void PicBoxMoreOption_Click(object sender, EventArgs e)
        {

        }

        private void DgvMusicList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void PlayBackInfoParentAndChild_MouseMove(object sender, MouseEventArgs e)
        {
            MouseHoverPlayBackInfo();
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
                PnlMusicList.Height -= decrementValue - heightAdjustmentOffset;
                PnlPlayMusicQueue.Height -= decrementValue - heightAdjustmentOffset;
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
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = formSize;
            }
        }

        private void BtnCloseApplication_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void FormMain_Resize(object sender, EventArgs e)
        {
            ApplyRoundedCorners();
            HandleResponsiveLayout();

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
        }

        private void HandleResponsiveLayout()
        {
            if (this.ClientSize.Width > 900)
            {
                // Function call to display Controls
                ShowOrHideDataGridViewColumn(true);
                ShowOrHideShuffleAndRepeatButton(true);
                ShowOrHideFilterControl(true, 161, 45);
                ShowOrHideSideBar(0, PnlSideBar.Width, -30);
                ShowOrHideSkipButton(true, 45, 15);

                picBoxHamburgerMenu.Visible = false;
            }
            else if (this.ClientSize.Width > 860 && this.ClientSize.Width <= 900)
            {
                // Function call to hide controls
                ShowOrHideSkipButton(false, 0, -30);

                // Function call to display Controls
                ShowOrHideDataGridViewColumn(true);
                ShowOrHideShuffleAndRepeatButton(true);
                ShowOrHideFilterControl(true, 161, 45);
                ShowOrHideSideBar(0, PnlSideBar.Width, -30);

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
                ShowOrHideFilterControl(true, 161, 45);

                picBoxHamburgerMenu.Visible = true;
            }
            else if (this.ClientSize.Width > 600 && this.ClientSize.Width <= 660 && this.ClientSize.Width != previousWidth)
            {
                // Function call to hide controls
                ShowOrHideSkipButton(false, 0, -30);
                ShowOrHideSideBar(-PnlSideBar.Width, 0, 15);
                ShowOrHideFilterControl(false, 0, 0);

                // Function call to display Controls
                ShowOrHideDataGridViewColumn(true);
                ShowOrHideShuffleAndRepeatButton(true);

                picBoxHamburgerMenu.Visible = true;
            }
            else if (this.ClientSize.Width <= 600 && this.ClientSize.Width != previousWidth)
            {
                // Function call to hide controls
                ShowOrHideSkipButton(false, 0, -30);
                ShowOrHideSideBar(-PnlSideBar.Width, 0, 15);
                ShowOrHideFilterControl(false, 0, 0);
                ShowOrHideDataGridViewColumn(false);
                ShowOrHideShuffleAndRepeatButton(false);

                picBoxHamburgerMenu.Visible = true;
            }

            previousWidth = this.ClientSize.Width;
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

            PnlHeaderControl.Left = sideBarWidth;
            PnlMusicList.Left = sideBarWidth;
            PnlPlayMusicQueue.Left = sideBarWidth;

            PnlHeaderControl.Width = pnlMain.Width - sideBarWidth;
            PnlMusicList.Width = pnlMain.Width - sideBarWidth;
            PnlPlayMusicQueue.Width = pnlMain.Width - sideBarWidth;
        }

        private void ShowOrHideFilterControl(bool showControl, int filterArtistOffset, int volumeButtonOffset)
        {
            CbFilterAlbum.Visible = showControl;
            CbFilterArtist.Left = CbFilterAlbum.Left - filterArtistOffset;

            PicBoxFullScreenToggle.Visible = showControl;
            PicBoxShowVolumeBar.Left = PicBoxFullScreenToggle.Left - volumeButtonOffset;
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




        // FORM RESIZING

        //protected override void OnPaint(PaintEventArgs e) // you can safely omit this method if you want
        //{
        //    e.Graphics.FillRectangle(Brushes.Green, Top);
        //    e.Graphics.FillRectangle(Brushes.Green, Left);
        //    e.Graphics.FillRectangle(Brushes.Green, Right);
        //    e.Graphics.FillRectangle(Brushes.Green, Bottom);
        //}

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

        private void CheckForAeroSnap()
        {
            //Rectangle screenRect = Screen.PrimaryScreen.Bounds;
            //Rectangle formRect = new Rectangle(this.Location, this.Size);

            //// Snap to the edges
            //if (Math.Abs(formRect.Left - screenRect.Left) < SnapThreshold)
            //{
            //    // Snap to the left
            //    this.Location = new Point(screenRect.Left, this.Location.Y);
            //    this.Size = new Size(screenRect.Width / 2, this.Size.Height);
            //}
            //else if (Math.Abs(formRect.Right - screenRect.Right) < SnapThreshold)
            //{
            //    // Snap to the right
            //    this.Location = new Point(screenRect.Right - this.Width, this.Location.Y);
            //    this.Size = new Size(screenRect.Width / 2, this.Size.Height);
            //}
            //else if (Math.Abs(formRect.Top - screenRect.Top) < SnapThreshold)
            //{
            //    // Snap to the top
            //    this.Location = new Point(this.Location.X, screenRect.Top);
            //    this.Size = new Size(this.Size.Width, screenRect.Height / 2);
            //}
            //else if (Math.Abs(formRect.Bottom - screenRect.Bottom) < SnapThreshold)
            //{
            //    // Snap to the bottom
            //    this.Location = new Point(this.Location.X, screenRect.Bottom - this.Height);
            //    this.Size = new Size(this.Size.Width, screenRect.Height / 2);
            //}
        }

        
    }
}
