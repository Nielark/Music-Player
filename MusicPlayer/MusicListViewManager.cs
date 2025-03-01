using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace MusicPlayer
{
    public class MusicListViewManager : MusicPlayerBase
    {
        PlayerControls playerControls = new PlayerControls();
        public ArtistAndAlbumUIControllers ArtistAlbumUIControllers { get; private set; } = new ArtistAndAlbumUIControllers();

        public MusicListViewManager(PlayerControls playerControls, List<Music> musicList)
        {
            this.playerControls = playerControls;
            this.musicList = musicList;
            //ShuffleMusic = this.playerControls.ShuffleMusic;
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
                        playerControls.PlayMusic(musicPicture, filePath, title, artist);
                        //playerControls.MarqueeEffectHandler();
                        //currentMusicIndex = playerControls.CurrentMusicIndex;
                        playerControls.SetCurrentMusicIndex(index);
                    }
                }

                if (ShuffleMusic)
                {
                    playerControls.FisherYatesShuffle(filePath);                       // Shuffle the music list
                    DgvPlayMusicQueue.DataSource = playerControls.shuffleMusicList;    // Set the shuffleMusicList music as the data source 
                }
                else
                {
                    DgvPlayMusicQueue.DataSource = playMusicQueue;      // Set the shuffleMusicList music as the data source (not shuffle)
                }

                // Display the play back controls
                TimerShowPnlPlayerControls.Start();
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
                    Image = info.DisplayImage,
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

                //pnlInfo.Visible = true;

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
            //CurrentMusicIndex = -1; // Set to empty
            //CurrentMusicIndex++;
            CurrentMusicIndex = 0;
            //playerControls.CurrentMusicIndex = CurrentMusicIndex;
            //playerControls.SetCurrentMusicIndex(CurrentMusicIndex);

            // Filter the music based on the selected artist name in the artist combo box
            playMusicQueue = musicList
                    .Where(s => isArtist ? s.Artist.ToLower().Equals(nameIdentifier.ToLower()) : s.Album.ToLower().Equals(nameIdentifier.ToLower()))  // Match by the selected artist
                    .ToList();  // Convert the filtered results to a list

            playerControls.SetPlayMusicQueue(playMusicQueue);

            // Retrieve music information
            string title = playMusicQueue[CurrentMusicIndex].Title;
            string artist = playMusicQueue[CurrentMusicIndex].Artist;
            string filePath = playMusicQueue[CurrentMusicIndex].File;
            Image musicPicture = playMusicQueue[CurrentMusicIndex].MusicPictureMedium;  // Get the mp3 image

            playerControls.PlayMusic(musicPicture, filePath, title, artist);

            ShuffleMusic = playerControls.ShuffleMusic;

            if (ShuffleMusic)
            {
                playerControls.FisherYatesShuffle(filePath);                       // Shuffle the music list
                dgvPlayMusicQueue.DataSource = playerControls.shuffleMusicList;    // Set the shuffleMusicList music as the data source 
            }
            else
            {
                dgvPlayMusicQueue.DataSource = playMusicQueue;      // Set the shuffleMusicList music as the data source (not shuffle)
            }

            // Display the play back controls
            timerShowPnlPlayerControls.Start();
        }

        private (string? albumName, string? artistName, TimeSpan duration, int songCount, Image musicPictureMedium) SelectArtistOrAlbum(string nameIdentifier, bool isArtist)
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

        private void DisplaySelectedArtistOrAlbum(
            (string? albumName, string? artistName, TimeSpan duration, int songCount, Image musicPictureMedium) selected, 
             string nameIdentifier, bool isArtist
            )
        {
            if (selected.songCount > 0)
            {
                ArtistAlbumUIControllers.PicBoxAlbumImage!.Image = selected.musicPictureMedium;

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
                        playerControls.PlayMusic(musicPicture, filePath, title, artist);   // Function call to play the selected music
                    }
                }
            }
        }
    }
}
