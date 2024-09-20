using NAudio.Wave;
using System.Drawing;
using System.Drawing.Printing;
using System.Formats.Tar;
using System.Windows.Forms;
using TagLib;
using static System.Formats.Asn1.AsnWriter;

namespace MusicPlayer
{
    public partial class FormMain : Form
    {
        private IWavePlayer? waveOutDevice;
        private AudioFileReader? audioFileReader;

        private int currentMusicIndex = -1;
        private int scrollSpeed = 5; // Adjust this for scrolling speed
        private bool playMusic = false, repeatMusic = false, shuffleMusic = false;
        string[]? musicFilesArr;
        string[]? shuffleMusicFilesArr;

        public FormMain()
        {
            InitializeComponent();
            MyModule.SetRoundedCorners(this, 8, 8, 8, 8);
            LoadMusicFiles();
            MyModule.MakeCircular(PicBoxPlayAndPause);
            CbSortMusic.SelectedIndex = 0;       
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void CbSortMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? currentFilePath = null;

            if (currentMusicIndex > -1)
            {
                currentFilePath = musicFilesArr[currentMusicIndex];
            }

            TblMusicList.Controls.Clear();
            TblMusicList.RowStyles.Clear();
            TblMusicList.ColumnStyles.Clear();
            TblMusicList.RowCount = 1;
            TblMusicList.ColumnCount = 4;

            if (CbSortMusic.SelectedIndex == 0)
            {
                //Array.Sort(musicFilesArr);
                // Sort by artist name using LINQ
                musicFilesArr = musicFilesArr
                    .Select(file => new { FilePath = file, Title = TagLib.File.Create(file).Tag.Title ?? System.IO.Path.GetFileNameWithoutExtension(file) })
                    .OrderBy(x => x.Title)
                    .Select(x => x.FilePath)
                    .ToArray();

            }
            else if (CbSortMusic.SelectedIndex == 1)
            {
                //Array.Reverse(musicFilesArr);
                musicFilesArr = musicFilesArr
                    .Select(file => new { FilePath = file, Title = TagLib.File.Create(file).Tag.Title ?? System.IO.Path.GetFileNameWithoutExtension(file) })
                    .OrderByDescending(x => x.Title)
                    .Select(x => x.FilePath)
                    .ToArray();
            }
            else if (CbSortMusic.SelectedIndex == 2)
            {
                // Sort by artist name using LINQ
                musicFilesArr = musicFilesArr
                    .Select(file => new { FilePath = file, Artist = TagLib.File.Create(file).Tag.FirstPerformer ?? "Unknown Artist" })
                    .OrderBy(x => x.Artist)
                    .Select(x => x.FilePath)
                    .ToArray();

                //List<MusicArtist> sortMusicArtist = new List<MusicArtist>();

                //foreach (var file in musicFilesArr)
                //{
                //    var tagFile = TagLib.File.Create(file);

                //    string artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist";
                //    sortMusicArtist.Add(new MusicArtist(file, artist));
                //}

                //sortMusicArtist.Sort((x, y) => x.GetArtistName().CompareTo(y.GetArtistName()));

                //for(int i = 0; i < sortMusicArtist.Count; i++)
                //{
                //    musicFilesArr[i] = sortMusicArtist[i].GetFilePath();
                //}
            }
            else if (CbSortMusic.SelectedIndex == 3)
            {
                // Sort by artist name using LINQ
                musicFilesArr = musicFilesArr
                    .Select(file => new { FilePath = file, Artist = TagLib.File.Create(file).Tag.FirstPerformer ?? "Unknown Artist" })
                    .OrderByDescending(x => x.Artist)
                    .Select(x => x.FilePath)
                    .ToArray();
            }
            else if (CbSortMusic.SelectedIndex == 4)
            {
                // Sort by duration using LINQ
                musicFilesArr = musicFilesArr
                    .Select(file => new { FilePath = file, Duration = TagLib.File.Create(file).Properties.Duration })
                    .OrderBy(x => x.Duration)
                    .Select(x => x.FilePath)
                    .ToArray();

                //List<MusicDuration> sortMusicDuration = new List<MusicDuration>();

                //foreach (var file in musicFilesArr)
                //{
                //    var tagFile = TagLib.File.Create(file);

                //    TimeSpan duration = tagFile.Properties.Duration;
                //    sortMusicDuration.Add(new MusicDuration(file, duration));
                //}

                //sortMusicDuration.Sort((x, y) => x.GetMusicDuration().CompareTo(y.GetMusicDuration()));

                //for (int i = 0; i < sortMusicDuration.Count; i++)
                //{
                //    musicFilesArr[i] = sortMusicDuration[i].GetFilePath();
                //}
            }
            else if (CbSortMusic.SelectedIndex == 5)
            {
                // Sort by duration using LINQ
                musicFilesArr = musicFilesArr
                    .Select(file => new { FilePath = file, Duration = TagLib.File.Create(file).Properties.Duration })
                    .OrderByDescending(x => x.Duration)
                    .Select(x => x.FilePath)
                    .ToArray();
            }

            if (!shuffleMusic && currentFilePath != null)
            {
                currentMusicIndex = Array.IndexOf(musicFilesArr, currentFilePath);
            }

            CreateMusicListControls();
        }

        private void LoadMusicFiles()
        {
            string musicDirectory = @"C:\Users\Mark Daniel\Music"; // Replace with your music folder path
            musicFilesArr = Directory.GetFiles(musicDirectory, "*.*", SearchOption.AllDirectories)
                                           .Where(file => file.EndsWith(".mp3") || file.EndsWith(".wav"))
                                           .ToArray();

            CreateMusicListControls();
        }

        private void CreateMusicListControls()
        {
            

            int ctrRow = 1;

            Label LblEmpty = new Label
            {
                AutoSize = true,
                BackColor = SystemColors.ControlDarkDark,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Margin = new Padding(0, 1, 0, 1),
                Name = "LblEmpty",
                Text = "",
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label LblTitleHeader = new Label
            {
                AutoSize = true,
                BackColor = SystemColors.ControlDarkDark,
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 0, 30, 0),
                Margin = new Padding(0, 1, 0, 1),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(0, 1),
                Name = "LblTitleHeader",
                Size = new Size(50, 49),
                Text = "TITLE",
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label LblArtistHeader = new Label
            {
                AutoSize = true,
                BackColor = SystemColors.ControlDarkDark,
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 0, 30, 0),
                Margin = new Padding(0, 1, 0, 1),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(50, 1),
                Name = "LblArtistHeader",
                Size = new Size(63, 49),
                Text = "ARTIST",
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label LblDurationHeader = new Label
            {
                AutoSize = true,
                BackColor = SystemColors.ControlDarkDark,
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 0, 5, 0),
                Margin = new Padding(0, 1, 0, 1),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Location = new Point(113, 1),
                Name = "LblDurationHeader",
                Size = new Size(515, 49),
                Text = "DURATION",
                TextAlign = ContentAlignment.MiddleLeft
            };

            TblMusicList.Controls.Add(LblEmpty, 0, 0);
            TblMusicList.Controls.Add(LblTitleHeader, 1, 0);
            TblMusicList.Controls.Add(LblArtistHeader, 2, 0);
            TblMusicList.Controls.Add(LblDurationHeader, 3, 0);

            foreach (string file in musicFilesArr)
            {
                var tagFile = TagLib.File.Create(file);
                string title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(file); // Fallback to file name if no title
                string artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist";
                string album = tagFile.Tag.Album ?? "Unknown Album";
                TimeSpan duration = tagFile.Properties.Duration;

                RowStyle newRowStyle = new RowStyle(SizeType.AutoSize);
                ColumnStyle newColStyle = new ColumnStyle(SizeType.AutoSize);

                TblMusicList.RowStyles.Add(newRowStyle);
                TblMusicList.ColumnStyles.Add(newColStyle);

                PictureBox PicBoxMusicImage = new PictureBox
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Green,
                    Padding = new Padding(0, 0, 30, 0),
                    Margin = new Padding(0, 1, 0, 1),
                    Name = "PicBoxMusicImage",
                    Size = new Size(30, 30),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    BackgroundImage = Image.FromFile("C:/Users/Mark Daniel/Downloads/music.png"),
                    TabStop = false
                };

                Label LblTitle = new Label
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    BackColor = Color.Green,
                    Padding = new Padding(30, 0, 30, 0),
                    Margin = new Padding(0, 1, 0, 1),
                    Name = "LblTitle",
                    Text = title,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                Label LblArtist = new Label
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Green,
                    Padding = new Padding(30, 0, 30, 0),
                    Margin = new Padding(0, 1, 0, 1),
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    Name = "LblArtist",
                    Text = artist,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                Label LblDuration = new Label
                {
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Green,
                    Padding = new Padding(30, 0, 5, 0),
                    Margin = new Padding(0, 1, 0, 1),
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                    Name = "LblDuration",
                    Text = $"{duration:hh\\:mm\\:ss}",
                    TextAlign = ContentAlignment.MiddleLeft
                };

                // Add event
                LblTitle.Click += (sender, args) => PlayMusic(file, title, artist);
                LblArtist.Click += (sender, args) => PlayMusic(file, title, artist);
                LblDuration.Click += (sender, args) => PlayMusic(file, title, artist);

                // Add the controls for each rows and columns
                TblMusicList.Controls.Add(PicBoxMusicImage, 0, ctrRow);
                TblMusicList.Controls.Add(LblTitle, 1, ctrRow);
                TblMusicList.Controls.Add(LblArtist, 2, ctrRow);
                TblMusicList.Controls.Add(LblDuration, 3, ctrRow);
                ctrRow++;
            }

            TblMusicList.RowCount = ctrRow;

            for (int i = 0; i < TblMusicList.RowCount; i++)
            {
                for (int j = 0; j < TblMusicList.ColumnCount; j++)
                {
                    Control control = TblMusicList.GetControlFromPosition(j, i);

                    if (i % 2 == 0)
                    {
                        control.BackColor = Color.Blue;
                    }
                    else
                    {
                        control.BackColor = Color.White;
                    }
                }
            }
        }

        private void LblPlayNextMusic_Click(object sender, EventArgs e)
        {
            if (musicFilesArr != null)
            {
                if (currentMusicIndex < musicFilesArr.Length - 1)
                {
                    currentMusicIndex++;
                    NextAndPreviousMusicHandler();
                }
            }
        }

        private void LblPlayPreviousMusic_Click(object sender, EventArgs e)
        {
            if (currentMusicIndex > 0)
            {
                currentMusicIndex--;
                NextAndPreviousMusicHandler();
            }
        }

        private void PlayMusic(string filePath, string title, string artist)
        {
            if (musicFilesArr != null)
            {
                LblShowPlayTitle.AutoSize = true;
                LblShowPlayArtist.AutoSize = true;
                TimerTitleMarquee.Stop(); // Stop the timer
                TimerArtistMarquee.Stop(); // Stop the timer
                LblShowPlayTitle.Left = 66;
                LblShowPlayArtist.Left = 66;

                if (!shuffleMusic)
                {
                    currentMusicIndex = Array.IndexOf(musicFilesArr, filePath);
                }
                else
                {
                    currentMusicIndex = Array.IndexOf(shuffleMusicFilesArr, filePath);
                }

                if (waveOutDevice != null) // Stop any currently playing music
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

                waveOutDevice = new WaveOutEvent(); // Create new playback device
                audioFileReader = new AudioFileReader(filePath); // Read the audio file
                waveOutDevice.Init(audioFileReader); // Initialize playback with audio file
                waveOutDevice.Play(); // Start playing the music

                LblShowPlayTitle.Text = title;
                LblShowPlayArtist.Text = artist;
                LblMusicLength.Text = audioFileReader.TotalTime.ToString("hh\\:mm\\:ss");

                LblShowPlayTitle.Text = title;
                LblShowPlayArtist.Text = artist;
                LblMusicLength.Text = audioFileReader.TotalTime.ToString("hh\\:mm\\:ss");

                TbSeekMusic.Maximum = (int)audioFileReader.TotalTime.TotalSeconds;

                if (LblShowPlayTitle.Left + LblShowPlayTitle.Width > panel6.Width && LblShowPlayArtist.Left + LblShowPlayArtist.Width < panel6.Width)
                {
                    TimerTitleMarquee.Start(); // Start the timer
                }
                else if (LblShowPlayTitle.Left + LblShowPlayTitle.Width < panel6.Width && LblShowPlayArtist.Left + LblShowPlayArtist.Width > panel6.Width)
                {
                    TimerArtistMarquee.Start(); // Start the timer
                }
                else if (LblShowPlayTitle.Left + LblShowPlayTitle.Width > panel6.Width && LblShowPlayArtist.Left + LblShowPlayArtist.Width > panel6.Width)
                {
                    if (LblShowPlayTitle.Width < LblShowPlayArtist.Width)
                    {
                        LblShowPlayTitle.AutoSize = false;
                        LblShowPlayTitle.Width = LblShowPlayArtist.Width;
                        LblShowPlayTitle.Invalidate();
                    }
                    else if (LblShowPlayArtist.Width < LblShowPlayTitle.Width)
                    {
                        LblShowPlayArtist.AutoSize = false;
                        LblShowPlayArtist.Width = LblShowPlayTitle.Width;
                        LblShowPlayArtist.Invalidate();
                    }

                    TimerTitleMarquee.Start(); // Start the timer
                    TimerArtistMarquee.Start(); // Start the timer
                }

                TimerMusicDuration.Start();
                playMusic = true;
            }
        }

        private void LblPlayAndPause_Click(object sender, EventArgs e)
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                if (playMusic)
                {
                    waveOutDevice.Pause();
                    playMusic = false;
                }
                else
                {
                    waveOutDevice.Play();
                    playMusic = true;
                }
            }
        }

        private void PicBoxShuffleMusic_Click(object sender, EventArgs e)
        {
            if (musicFilesArr != null)
            {
                shuffleMusic = !shuffleMusic;
                label1.Text = shuffleMusic.ToString();

                string currentFilePath = shuffleMusic ? musicFilesArr[currentMusicIndex] : shuffleMusicFilesArr[currentMusicIndex];

                if (shuffleMusic)
                {
                    // Fisher-Yates Shuffle (aka Knuth Shuffle)
                    Random random = new Random();

                    shuffleMusicFilesArr = musicFilesArr.ToArray();

                    for (int i = shuffleMusicFilesArr.Length - 1; i > 0; i--)
                    {
                        int randomIndex = random.Next(0, i + 1);

                        string temp = shuffleMusicFilesArr[i];
                        shuffleMusicFilesArr[i] = shuffleMusicFilesArr[randomIndex];
                        shuffleMusicFilesArr[randomIndex] = temp;
                    }

                    for (int i = 0; i < shuffleMusicFilesArr.Length; i++)
                    {
                        if (currentFilePath == shuffleMusicFilesArr[i])
                        {
                            string temp = shuffleMusicFilesArr[0];
                            shuffleMusicFilesArr[0] = shuffleMusicFilesArr[i];
                            shuffleMusicFilesArr[i] = temp;
                            break;
                        }
                    }

                    currentMusicIndex = 0;
                }
                else
                {
                    currentMusicIndex = Array.IndexOf(musicFilesArr, currentFilePath);
                }
            }
        }

        private void NextAndPreviousMusicHandler()
        {
            if (musicFilesArr != null)
            {
                if (!shuffleMusic)
                {
                    var tagFile = TagLib.File.Create(musicFilesArr[currentMusicIndex]);

                    string title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(musicFilesArr[currentMusicIndex]); // Fallback to file name if no title
                    string artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist";

                    TbSeekMusic.Value = 0;
                    PlayMusic(musicFilesArr[currentMusicIndex], title, artist);
                }
                else
                {
                    var tagFile = TagLib.File.Create(shuffleMusicFilesArr[currentMusicIndex]);

                    string title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(shuffleMusicFilesArr[currentMusicIndex]); // Fallback to file name if no title
                    string artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist";

                    TbSeekMusic.Value = 0;
                    PlayMusic(shuffleMusicFilesArr[currentMusicIndex], title, artist);
                }
            }
        }

        private void PicBoxRepeatMusic_Click(object sender, EventArgs e)
        {
            repeatMusic = !repeatMusic;
        }

        private void TimerTitleMarquee_Tick(object sender, EventArgs e)
        {
            LblShowPlayTitle.Left -= scrollSpeed;

            // Check if the label has gone off-screen and wrap it back
            if (LblShowPlayTitle.Left + LblShowPlayTitle.Width < 0)
            {
                LblShowPlayTitle.Left = panel6.Width;
            }
        }

        private void TimerArtistMarquee_Tick(object sender, EventArgs e)
        {
            LblShowPlayArtist.Left -= scrollSpeed;

            if (LblShowPlayArtist.Left + LblShowPlayArtist.Width < 0)
            {
                LblShowPlayArtist.Left = panel6.Width;
            }
        }

        private void TimerMusicDuration_Tick(object sender, EventArgs e)
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                if (TbSeekMusic.Value < TbSeekMusic.Maximum)
                {
                    // Update seek bar and labels
                    TbSeekMusic.Value = (int)audioFileReader.CurrentTime.TotalSeconds;
                    LblMusicLength.Text = $"{audioFileReader.TotalTime - audioFileReader.CurrentTime:hh\\:mm\\:ss}";
                    LblMusicDurationCtr.Text = $"{audioFileReader.CurrentTime:hh\\:mm\\:ss}";
                }
                else
                {
                    // Stop and clean up resources
                    TbSeekMusic.Value = 0;
                    TimerMusicDuration.Stop();
                    waveOutDevice.Stop();
                    waveOutDevice.Dispose();
                    waveOutDevice = null;

                    // Dispose of audioFileReader
                    if (audioFileReader != null)
                    {
                        audioFileReader.Dispose();
                        audioFileReader = null;
                    }

                    if (!repeatMusic)
                    {
                        currentMusicIndex++;    // Increment to the index of the next track
                    }

                    NextAndPreviousMusicHandler();  // Function call to play the next music
                }
            }
        }

        // next and previeus

        private void TbSeekMusic_ValueChanged()
        {
            if (audioFileReader != null)
            {
                LblMusicLength.Text = $"{audioFileReader.TotalTime - TimeSpan.FromSeconds(TbSeekMusic.Value):hh\\:mm\\:ss}";
                LblMusicDurationCtr.Text = $"{TimeSpan.FromSeconds(TbSeekMusic.Value)}";
            }
        }

        private void TbSeekMusic_MouseUp(object sender, MouseEventArgs e)
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                audioFileReader.CurrentTime = TimeSpan.FromSeconds(TbSeekMusic.Value);

                if (playMusic)
                {
                    waveOutDevice.Play();
                }
            }
        }

        private void TbSeekMusic_MouseDown(object sender, MouseEventArgs e)
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
        }
    }
}
