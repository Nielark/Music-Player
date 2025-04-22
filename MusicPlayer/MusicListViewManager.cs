using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace MusicPlayer
{
    public class MusicListViewManager : MusicPlayerBase
    {
        PlayerControls playerControls = new PlayerControls();
        public ArtistAndAlbumUIControllers ArtistAlbumUIControllers { get; private set; } = new ArtistAndAlbumUIControllers();

        public MusicListViewManager(PlayerControls playerControls)
        {
            this.playerControls = playerControls;
            //this.musicList = musicList;
        }

        public void SetMusicList(List<Music> musicList)
        {
            this.musicList = musicList;
        }

        public void SetArtistAndAlbumUIControllers(ArtistAndAlbumUIControllers artistAndAlbumUIController)
        {
            this.ArtistAlbumUIControllers = artistAndAlbumUIController;
        }

        // DATA GRID VIEW OF MUSIC LIST

        public void MusicListView(DataGridView DgvMusicList, DataGridView DgvPlayMusicQueue, ComboBox CbFilterArtist, ComboBox CbFilterAlbum, System.Windows.Forms.Timer TimerShowPnlPlayerControls)
        {
            ShuffleMusic = this.playerControls.ShuffleMusic;

            // Play's the selected music through cell click and populate the playlist queue
            if (DgvMusicList.CurrentRow != null)
            {
                // Filter the music based on the selected artist and album from the combo boxes
                // Selects all the music when both combo boxes are at their default values (index 0)
                playMusicQueue = (CbFilterArtist.SelectedIndex == 0 && CbFilterAlbum.SelectedIndex == 0)
                                    ? new List<Music>(musicList)  // Create a new list with all music
                                    : musicList
                                        .Where(s => s.Artist.Equals(CbFilterArtist.SelectedValue) || s.Album.Equals(CbFilterAlbum.SelectedValue))   // Filter based on selected artist or album
                                        .ToList();  // Convert the filtered results to a list


                playerControls.SetPlayMusicQueue(playMusicQueue);
                playerControls.TogglePlayAndPause();

                var (title, artist, filePath) = RetrieveSelectedRowData(DgvMusicList);
                FindAndPlayMusic(title, artist, filePath);
                UpdateDataGridViewList(DgvPlayMusicQueue, filePath);

                TimerShowPnlPlayerControls.Start();
            }
        }

        private (string?, string?, string?) RetrieveSelectedRowData(DataGridView dgvMusicList)
        {
            // Retrieve every cells data from the selected row
            string? title = dgvMusicList.CurrentRow.Cells[2].Value.ToString();
            string? artist = dgvMusicList.CurrentRow.Cells[3].Value.ToString();
            string? filePath = dgvMusicList.CurrentRow.Cells[6].Value.ToString();

            return (title, artist, filePath);
        }

        public void FindAndPlayMusic(string? title, string? artist, string? filePath)
        {
            // Find the index of the selected music using the file path
            int index = playMusicQueue.FindIndex(index => index.File == filePath);

            // Check if the file path was found 
            if (index != -1)
            {
                Image musicPicture = playMusicQueue[index].MusicPictureMedium;  // Get the mp3 image

                // Play's the mp3 when all data has value
                if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(artist))
                {
                    playerControls.PlayMusic(musicPicture, filePath, title, artist);
                    playerControls.SetCurrentMusicIndex(index);
                }
            }
        }

        public void UpdateDataGridViewList(DataGridView dgvPlayMusicQueue, string? filePath)
        {
            if (ShuffleMusic)
            {
                playerControls.FisherYatesShuffle(filePath);                       // Shuffle the music list
                dgvPlayMusicQueue.DataSource = playerControls.shuffleMusicList;    // Set the shuffleMusicList music as the data source 
            }
            else
            {
                dgvPlayMusicQueue.DataSource = playMusicQueue;      // Set the shuffleMusicList music as the data source (not shuffle)
            }
        }

        // ARTISTS OR ALBUM LIST

        public void DisplayInfo<T>(FlowLayoutPanel targetFlowLayoutPanel, bool isArtist, List<T> musicInfo) where T : MusicInfo
        {
            foreach (var info in musicInfo)
            {
                // Create a new Label for each iteration
                Panel pnlCover = new Panel
                {
                    //BackColor = Color.Red,
                    //Name = "pnlArtistCover",
                    //Size = new Size(150, 200),
                    //Margin = new Padding(5)

                    BackColor = Color.FromArgb(8, 18, 38),
                    //BorderStyle = BorderStyle.FixedSingle,
                    BorderStyle = BorderStyle.None,
                    //Location = new Point(18, 3),
                    Name = "pnlCover",
                    Size = new Size(150, 200),
                    Margin = new Padding(5),
                    //TabIndex = 1,
                };

                PictureBox picBoxImage = new PictureBox
                {
                    //BackColor = Color.Silver,
                    Image = isArtist? Properties.Resources.artist : info.DisplayImage,
                    Location = new Point(10, 10),
                    Name = "picBoxImage",
                    Size = new Size(130, 130),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    //TabIndex = 0,
                    //TabStop = false
                };

                PictureBox picBoxPlayButton = new PictureBox
                {
                    Anchor = AnchorStyles.None,
                    BackColor = Color.FromArgb(36, 176, 191),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Image = Properties.Resources.play,
                    Location = new Point(99, 99),
                    Name = "picBoxPlayButton",
                    Padding = new Padding(3, 0, 0, 0),
                    Size = new Size(35, 35),
                    SizeMode = PictureBoxSizeMode.CenterImage,
                    //TabIndex = 7,
                    //TabStop = false
                    //SetToolTip(pictureBox3, "Pause")
                };

                Label lblName = new Label
                {
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    ForeColor = Color.White,
                    Location = new Point(10, 150),
                    Name = "lblName",
                    Size = new Size(130, 20),
                    MaximumSize = new Size(130, 20),
                    //TabIndex = 6,
                    Text = isArtist ? info.Name : ((AlbumInfo)(object)info).AlbumName,
                    AutoEllipsis = true,
                };


                Label? lblArtistName = null;
                if (!isArtist && info is AlbumInfo albumInfo)
                {
                    lblArtistName = new Label
                    {
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0),
                        ForeColor = Color.White,
                        Location = new Point(10, 170),
                        Name = "LblArtistName",
                        Size = new Size(130, 20),
                        MaximumSize = new Size(130, 20),
                        //TabIndex = 6,
                        //Text = ((AlbumInfo)(object)info).ArtistName,
                        Text = albumInfo.ArtistName,
                        AutoEllipsis = true,
                    };
                }

                // Add the controls to te form
                targetFlowLayoutPanel.Controls.Add(pnlCover);
                pnlCover.Controls.Add(picBoxImage);
                pnlCover.Controls.Add(picBoxPlayButton);
                pnlCover.Controls.Add(lblName);

                // Add the artist name label if album button was clicked
                if (!isArtist && lblArtistName != null)
                {
                    pnlCover.Controls.Add(lblArtistName);
                    ShowToolTip(lblArtistName, pnlCover, picBoxImage, picBoxPlayButton, lblName, lblArtistName);
                }

                ShowToolTip(lblName, pnlCover, picBoxImage, picBoxPlayButton, lblName); // Display the tool tip

                // Create rounded corners and circular controls
                MyModule.MakeCircular(picBoxPlayButton);
                MyModule.SetRoundedCorners(pnlCover, 10, 10, 10, 10);
                MyModule.SetRoundedCorners(picBoxImage, 10, 10, 10, 10);
                picBoxPlayButton.BringToFront();

                // Attach event handlers for mouse enter and leave
                AttachMouseEvents(pnlCover, picBoxPlayButton, lblName.Text, isArtist);
            }
        }

        private void ShowToolTip(Label name, params Control[] targetControls)
        {
            // Check if the text needs a tooltip
            using (Graphics g = name.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(name.Text, name.Font);

                // Set a tooltip if the text size exceeds the label's width
                if (textSize.Width > name.Width)
                {
                    foreach (Control control in targetControls)
                    {
                        ArtistAlbumUIControllers.ToolTipPlayerControl!.SetToolTip(control, name.Text);
                    }
                }
            }
        }

        private void AttachMouseEvents(Panel pnlCover, PictureBox playButton, string nameIdentifier, bool isArtist)
        {   
            HoverEffect(pnlCover, Color.Silver, Color.FromArgb(8, 18, 38));

            playButton.MouseClick += (sender, e) =>
            {
                PlayMusicByArtistsOrAlbum(nameIdentifier, isArtist, ArtistAlbumUIControllers.DgvPlayMusicQueue!, ArtistAlbumUIControllers.TimerShowPnlPlayerControls!);
            };

            pnlCover.MouseClick += (sender, e) =>
            {
                var selected = SelectArtistOrAlbum(nameIdentifier, isArtist);
                DisplaySelectedArtistOrAlbum(selected, nameIdentifier, isArtist);

                if (isArtist)
                {
                    DisplayArtistOrAlbumTracks(isArtist, ArtistAlbumUIControllers.DgvArtistTracks!, ArtistAlbumUIControllers.PnlArtistTrack!, ArtistAlbumUIControllers.PnlAlbumTrack!);
                }
                else
                {
                    DisplayArtistOrAlbumTracks(isArtist, ArtistAlbumUIControllers.DgvAlbumTracks!, ArtistAlbumUIControllers.PnlAlbumTrack!, ArtistAlbumUIControllers.PnlArtistTrack!);
                }

                Panel[] showPanels = { ArtistAlbumUIControllers.PnlInfo! };
                Panel[] hidePanels = { ArtistAlbumUIControllers.PnlHeaderControl!, ArtistAlbumUIControllers.PnlMusicLibrary!, ArtistAlbumUIControllers.PnlPlayMusicQueue! };
                PanelVisibilityHandler(showPanels, hidePanels);
            };
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

        private void PlayMusicByArtistsOrAlbum(string nameIdentifier, bool isArtist, DataGridView dgvPlayMusicQueue, System.Windows.Forms.Timer timerShowPnlPlayerControls)
        {
            CurrentMusicIndex = 0;

            // Filter the music based on the selected artist name in the artist combo box
            playMusicQueue = musicList
                    .Where(s => isArtist ? s.Artist.ToLower().Equals(nameIdentifier.ToLower()) : s.Album.ToLower().Equals(nameIdentifier.ToLower()))  // Match by the selected artist
                    .ToList();  // Convert the filtered results to a list

            playerControls.SetPlayMusicQueue(playMusicQueue);
            playerControls.TogglePlayAndPause();

            var (title, artist, filePath, musicPicture) = RetrieveMusicInformation();

            playerControls.PlayMusic(musicPicture, filePath, title, artist);

            ShuffleMusic = playerControls.ShuffleMusic;

            UpdateDataGridViewList(dgvPlayMusicQueue, filePath);

            // Display the play back controls
            timerShowPnlPlayerControls.Start();
        }

        private (string, string, string, Image) RetrieveMusicInformation()
        {
            // Retrieve music information
            string title = playMusicQueue[CurrentMusicIndex].Title;
            string artist = playMusicQueue[CurrentMusicIndex].Artist;
            string filePath = playMusicQueue[CurrentMusicIndex].File;
            Image musicPicture = playMusicQueue[CurrentMusicIndex].MusicPictureMedium;  // Get the mp3 image

            return (title, artist, filePath, musicPicture);
        }

        public (string? albumName, string? artistName, TimeSpan duration, int songCount, Image musicPictureMedium) SelectArtistOrAlbum(string nameIdentifier, bool isArtist)
        {
            var select = musicList
                            .Where(s => isArtist ? s.Artist.Equals(nameIdentifier, StringComparison.OrdinalIgnoreCase) : s.Album.Equals(nameIdentifier, StringComparison.OrdinalIgnoreCase))
                            .GroupBy(s => isArtist ? s.Artist : s.Album)
                            .Select(group => new
                            {
                                AlbumName = isArtist ? null : group.Key,
                                ArtistName = isArtist ? group.Key : group.First().Artist,  // Assuming all songs in the album have the same artist
                                Duration = group.Aggregate(TimeSpan.Zero, (total, song) => total + TimeSpan.Parse(song.Duration)), // Parse and sum durations
                                SongCount = group.Count(),  // Count the total number of songs in the album
                                Image = group.First().MusicPictureMedium,   // Assuming all songs in the album have the same image
                            })
                            .ToList();

            return (select[0].AlbumName, select[0].ArtistName, select[0].Duration, select[0].SongCount, select[0].Image);
        }

        public void DisplaySelectedArtistOrAlbum(
            (string? albumName, string? artistName, TimeSpan duration, int songCount, Image musicPictureMedium) selected, 
             string nameIdentifier, bool isArtist
            )
        {
            if (selected.songCount > 0)
            {
                ArtistAlbumUIControllers.PicBoxAlbumImage!.Image = selected.musicPictureMedium;
                ArtistAlbumUIControllers.LblAlbumName!.Tag = isArtist;

                if (isArtist)
                {
                    int totalAlbums = musicList
                                .Where(w => w.Artist.Equals(nameIdentifier, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.Album)
                                .Distinct()
                                .Count();

                    ArtistAlbumUIControllers.LblAlbumNumbers!.Visible = false;
                    ArtistAlbumUIControllers.LblAlbumName!.Text = selected.artistName;
                    ArtistAlbumUIControllers.LblAlbumArtist!.Text = $"{totalAlbums} albums  |  {selected.songCount} songs  |  {selected.duration:hh\\:mm\\:ss} run time";
                    ArtistAlbumUIControllers.LblAlbumNumbers!.Text = selected.albumName;
                }
                else
                {

                    ArtistAlbumUIControllers.LblAlbumNumbers!.Visible = true;
                    ArtistAlbumUIControllers.LblAlbumName!.Text = selected.albumName;
                    ArtistAlbumUIControllers.LblAlbumArtist!.Text = selected.artistName;
                    ArtistAlbumUIControllers.LblAlbumNumbers!.Text = $"{selected.songCount} songs  |  {selected.duration:hh\\:mm\\:ss} run time";
                }
            }
        }

        //private List<Music> FilterAlbumTracks(string? nameIdentifier, bool isArtist)
        private List<Music> FilterTrackHandler(bool isArtist)
        {
            var albumName = ArtistAlbumUIControllers.LblAlbumName!.Text;
            var artistName = ArtistAlbumUIControllers.LblAlbumName!.Text;

            // Filters the tracks that belongs to a specific album
            var albumTracks = musicList
                .Where(w => isArtist
                    ? w.Artist.Equals(artistName, StringComparison.OrdinalIgnoreCase)
                    : w.Album.Equals(albumName, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => isArtist
                    ? o.Album
                    : o.Artist)
                .ToList();

            return albumTracks;
        }

        //private void DisplayAlbumTracks(string albumName)
        public void DisplayArtistOrAlbumTracks(bool isArtist, DataGridView dgv, Panel showPnl, Panel hidePnl)
        {
            dgv.DataSource = null;

            var tracks = FilterTrackHandler(isArtist);

            dgv.DataSource = tracks;
            ModifyDataGridView(dgv, showPnl);
            showPnl.Visible = true;
            hidePnl.Visible = false;
        }

        public void PlayArtistTrack(DataGridView dgvArtistTracks, DataGridView dgvPlayMusicQueue, System.Windows.Forms.Timer TimerShowPnlPlayerControls)
        {
            ShuffleMusic = this.playerControls.ShuffleMusic;

            // Play's the selected music through cell click and populate the playlist queue
            if (dgvArtistTracks.CurrentRow != null)
            {
                playMusicQueue = FilterTrackHandler(true);

                playerControls.SetPlayMusicQueue(playMusicQueue);
                playerControls.TogglePlayAndPause();

                var (title, artist, filePath) = RetrieveSelectedRowData(dgvArtistTracks);
                FindAndPlayMusic(title, artist, filePath);
                UpdateDataGridViewList(dgvPlayMusicQueue, filePath);

                TimerShowPnlPlayerControls.Start();
            }
        }

        public void PlayAlbumTrack(DataGridView dgvAlbumTracks, DataGridView dgvPlayMusicQueue, System.Windows.Forms.Timer TimerShowPnlPlayerControls)
        {
            ShuffleMusic = this.playerControls.ShuffleMusic;

            // Play's the selected music through cell click and populate the playlist queue
            if (dgvAlbumTracks.CurrentRow != null)
            {
                playMusicQueue = FilterTrackHandler(false);

                playerControls.SetPlayMusicQueue(playMusicQueue);
                playerControls.TogglePlayAndPause();

                var (title, artist, filePath) = RetrieveSelectedRowData(dgvAlbumTracks);
                FindAndPlayMusic(title, artist, filePath);
                UpdateDataGridViewList(dgvPlayMusicQueue, filePath);

                TimerShowPnlPlayerControls.Start();
            }
        }

        public void ModifyDataGridView(DataGridView dgv, System.Windows.Forms.Panel listPanel)
        {
            // Customize the data grid view when there is a data row displayed
            if (dgv.RowCount > 0)
            {
                // Hide the header title of the column
                dgv.Columns["Artist"].Visible = false;
                //dgv.Columns["Album"].Visible = false;
                dgv.Columns["File"].Visible = false;
                dgv.Columns["musicPictureMedium"].Visible = false;
                dgv.Columns[0].HeaderText = "Image";  // Set the header title of the first column

                // Customize the sizing of columns
                dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns["Artist"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns["Album"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns["Duration"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Dynamically adjust the height of the DataGridView based on the number of rows
            int rowHeight = dgv.RowTemplate.Height; // The height of one row
            int totalHeight = dgv.Rows.Count * rowHeight + dgv.ColumnHeadersHeight;

            // Set DataGridView height dynamically
            dgv.Height = totalHeight > listPanel.Height ? totalHeight : listPanel.Height;

            listPanel.VerticalScroll.Value = 0;
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

        public List<T> GetMusicInfo<T>(Func<Music, T> selector) where T : MusicInfo
        {
            var musicInfo = musicList
                            .Select(selector)
                            .Distinct()
                            .ToList();

            return musicInfo;
        }

        // Helper method to capitalize the first letter of each word
        public static string CapitalizeWords(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        // DATA GRID VIEW OF MUSIC QUEUE

        public void MusicQueueView(DataGridView DgvPlayMusicQueue)
        {
            // Play's the selected music through cell click 
            if (DgvPlayMusicQueue.CurrentRow != null)
            {
                var (title, artist, filePath) = RetrieveSelectedRowData(DgvPlayMusicQueue);

                // Find the index of the selected music using the file path
                int index = playMusicQueue.FindIndex(index => index.File == filePath);
                if (index != -1)
                {
                    Image musicPicture = playMusicQueue[index].MusicPictureMedium;  // Get the mp3 image

                    // Play's the music when all necessary data is available
                    if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(artist))
                    {
                        playerControls.PlayMusic(musicPicture, filePath, title, artist);   // Function call to play the selected music
                    }
                }
            }
        }

        // ALBUM TRACK CONTROLS
        // PLAY ALL
        public void PlayAllArtistOrAlbumTracks(Label albumName, DataGridView dgvPlayMusicQueue, System.Windows.Forms.Timer timerShowPnlPlayerControls)
        {
            if (ArtistAlbumUIControllers.LblAlbumName?.Tag != null)
            {
                CurrentMusicIndex = 0;
                bool isArtist = (bool)ArtistAlbumUIControllers.LblAlbumName.Tag;

                // Filters the tracks that belongs to a specific album
                playMusicQueue = FilterTrackHandler(isArtist);


                playerControls.SetPlayMusicQueue(playMusicQueue);
                playerControls.TogglePlayAndPause();

                var (title, artist, filePath, musicPicture) = RetrieveMusicInformation();

                playerControls.PlayMusic(musicPicture, filePath, title, artist);

                ShuffleMusic = playerControls.ShuffleMusic;

                UpdateDataGridViewList(dgvPlayMusicQueue, filePath);

                // Display the play back controls
                timerShowPnlPlayerControls.Start();
            }
        }

        // SHUFFLE & PLAY
        public void ShuffleAndPlayArtistsOrAlbumTracks(Label albumName, DataGridView dgvPlayMusicQueue, System.Windows.Forms.Timer timerShowPnlPlayerControls)
        {
            playerControls.ShuffleMusic = true;

            if (ArtistAlbumUIControllers.LblAlbumName?.Tag != null)
            {
                bool isArtist = (bool)ArtistAlbumUIControllers.LblAlbumName.Tag;
                // Filters the tracks that belongs to a specific album
                playerControls.playMusicQueue = FilterTrackHandler(isArtist);

                playerControls.FisherYatesShuffle();                // Shuffle the music list using the Fisher-Yates algorithm
                dgvPlayMusicQueue.DataSource = playerControls.shuffleMusicList;    // Set shuffleMusicList as the data source and display in data grid view

                // Get the music file information from the first index in the shuffled list
                string filePath = playerControls.shuffleMusicList[0].File;
                string Title = playerControls.shuffleMusicList[0].Title;
                string Artist = playerControls.shuffleMusicList[0].Artist;

                int index = playerControls.shuffleMusicList.FindIndex(index => index.File == filePath);    // Find the index of the current music in the shuffled list based on file path
                Image musicPicture = playerControls.shuffleMusicList[index].MusicPictureMedium;            // Get the music image

                playerControls.PlayMusic(musicPicture, filePath, Title, Artist);   // Function call to play the music

                timerShowPnlPlayerControls.Start();
            }

            playerControls.NotifyShuffleMusicStateChanged(playerControls.ShuffleMusic);
        }

        public void BackToArtistOrAlbumList()
        {
            if (ArtistAlbumUIControllers.LblAlbumName?.Tag != null)
            {
                bool isArtist = (bool)ArtistAlbumUIControllers.LblAlbumName.Tag;
                Panel[] showPanels, hidePanels;

                if (isArtist)
                {
                    showPanels = new Panel[] { ArtistAlbumUIControllers.PnlHeaderControl!,  ArtistAlbumUIControllers.PnlMusicLibrary!, ArtistAlbumUIControllers.PnlArtist! };
                    hidePanels = new Panel[] { ArtistAlbumUIControllers.PnlInfo!, ArtistAlbumUIControllers.PnlArtistTrack! };
                }
                else
                {
                    showPanels = new Panel[] { ArtistAlbumUIControllers.PnlHeaderControl!, ArtistAlbumUIControllers.PnlMusicLibrary!, ArtistAlbumUIControllers.PnlAlbum! };
                    hidePanels = new Panel[] { ArtistAlbumUIControllers.PnlInfo!, ArtistAlbumUIControllers.PnlAlbumTrack! };
                }

                PanelVisibilityHandler(showPanels, hidePanels);
            }
        }

        public void GetSelectedRowIndex(DataGridView dgv, ContextMenuStrip cms, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            dgv.ClearSelection();  // Clear current selection
            dgv.Rows[e.RowIndex].Selected = true;  // Select the row that was right-clicked
            e.ContextMenuStrip = cms;
            playerControls.selectedRowIndex = e.RowIndex;
        }
    }
}
