using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayer
{
    public class MusicLibrary
    {
        public List<Music> musicList;

        public MusicLibrary() 
        {
            musicList = new List<Music>();
        }

        public void LoadMusicFiles(DataGridView DgvMusicList)
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
                        MessageBox.Show($"Error processing file {file}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                DisplayMusic(DgvMusicList);
            }
            else
            {
                MessageBox.Show("No music files found.");
            }
        }

        private void DisplayMusic(DataGridView DgvMusicList)
        {
            DgvMusicList.DataSource = null;
            musicList = musicList.OrderBy(x => x.Title).ToList();
            DgvMusicList.DataSource = musicList;    // Set musicList as the datasource to display in data grid view           
        }

        private List<string> GetMusicFiles()
        {
            // Define a list of music files to stored the music
            List<string> musicFiles = new List<string>();

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
                musicPictureSmall = Properties.Resources.default_music_picture_small;
                musicPictureMedium = Properties.Resources.default_music_picture_medium;
            }

            // Get the following information from the file's meta data
            string title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(file);     // Get the song title, or use the file name if the title is not available
            string artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist";                 // Get the artist name, or use "Unknown Artist" if not available   
            string album = tagFile.Tag.Album ?? "Unknown Album";                            // Get the album name, or use "Unknown Album" if not available
            TimeSpan duration = tagFile.Properties.Duration;                                // Get the song's duration

            // Create a new MyMusic object using the gathered metadata and add it to the music list
            Music myMusic = new Music(musicPictureSmall, musicPictureMedium, title, artist, album, duration, file);
            musicList.Add(myMusic); // Add the new music item to the music list
        }

        public void ImportMusicFiles(OpenFileDialog ofdMusic, DataGridView DgvMusicList)
        {
            if (ofdMusic.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofdMusic.FileNames)
                {
                    GetFileMetaData(file);
                }

                DisplayMusic(DgvMusicList);
            }
        }
    }
}
