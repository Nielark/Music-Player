using Modernial.Controls;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayer
{
    public class MusicSearchFilterSorter : MusicPlayerBase
    {
        public bool IsAllMusicList { get; set; }
        public bool IsArtistList { get; set; }

        PlayerControls playerControls;
        MusicListViewManager musicListViewManager;

        public MusicSearchFilterSorter(List<Music> musicList, PlayerControls playerControls, MusicListViewManager musicListViewManager)
        {
            this.musicList = musicList;
            this.playerControls = playerControls;
            this.musicListViewManager = musicListViewManager;
        }


        // SEARCH BOX

        public void Search(TextBox TxtSearch, ComboBox CbFilterAlbum, ComboBox CbFilterArtist, DataGridView DgvMusicList)
        {
            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                // Set filters to default
                CbFilterAlbum.SelectedIndex = 0;
                CbFilterArtist.SelectedIndex = 0;

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
        }

        public List<ArtistInfo> SearchArtistList(TextBox txtSearch)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var artistInfo = musicListViewManager.GetMusicInfo(a => new ArtistInfo(MusicListViewManager.CapitalizeWords(a.Artist), a.MusicPictureMedium));

                // Filter the artist list based on the artist name matching the search text
                // The search is case-insensitive
                return artistInfo
                            .Where(s => s.Name.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)) // Match by artist name
                            .OrderBy(s => s.Name)  // Sort the results alphabetically by artist name
                            .ToList();              // Convert the result to a list
            }
            
            return musicListViewManager.GetMusicInfo(a => new ArtistInfo(MusicListViewManager.CapitalizeWords(a.Artist), a.MusicPictureMedium)); ;
        }

        public List<AlbumInfo> SearchAlbumList(TextBox txtSearch)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var albumInfo = musicListViewManager.GetMusicInfo(a => new AlbumInfo(MusicListViewManager.CapitalizeWords(a.Album), a.Artist, a.MusicPictureMedium, a.Duration));

                return albumInfo
                            .Where(s => s.AlbumName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase) || // Match by title
                                        s.ArtistName.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase)) // Match by artist name
                            .OrderBy(s => s.Name)  // Sort the results alphabetically by artist name
                            .ToList();              // Convert the result to a list
            }

            return musicListViewManager.GetMusicInfo(a => new AlbumInfo(MusicListViewManager.CapitalizeWords(a.Album), a.Artist, a.MusicPictureMedium, a.Duration));
        }


        // SORT COMBO BOX

        public void SortMusicList(ComboBox CbSortMusic, TextBox TxtSearch, DataGridView DgvMusicList)
        {
            string? currentFilePath = null;

            // Store the file path of the currently playing music if it's valid
            if (CurrentMusicIndex > -1) currentFilePath = musicList[CurrentMusicIndex].File;

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

            TxtSearch.Text = string.Empty;          // Clear the search box
            DgvMusicList.DataSource = null;         // Clear the data in data grid view
            DgvMusicList.DataSource = musicList;    // Display music in data grid view   
        }

        public List<ArtistInfo> SortArtistList(ComboBox CbSortMusic)
        {
            // Display artist information
            var artistInfo = musicListViewManager.GetMusicInfo(a => new ArtistInfo(MusicListViewManager.CapitalizeWords(a.Artist), a.MusicPictureMedium));

            // Perform sorting based on the selected index in the combo box
            switch (CbSortMusic.SelectedIndex)
            {
                case 0:
                    artistInfo = artistInfo.OrderBy(x => x.Name).ToList(); // Sort by title in ascending
                    break;
                case 1:
                    artistInfo = artistInfo.OrderByDescending(x => x.Name).ToList(); // Sort by title in descending
                    break;
                default:
                    // Handle invalid selection
                    break;
            }

            return artistInfo;

            //TxtSearch.Text = string.Empty;          // Clear the search box
            //musicListViewManager.DisplayInfo(flowLayoutPanel1, true, artistInfo);
        }

        public List<AlbumInfo> SortAlbumList(ComboBox CbSortMusic)
        {
            var albumInfo = musicListViewManager.GetMusicInfo(a => new AlbumInfo(MusicListViewManager.CapitalizeWords(a.Album), a.Artist, a.MusicPictureMedium, a.Duration));
            
            switch (CbSortMusic.SelectedIndex)
            {
                case 0:
                    albumInfo = albumInfo.OrderBy(x => x.AlbumName).ToList(); // Sort by title in ascending
                    break;
                case 1:
                    albumInfo = albumInfo.OrderByDescending(x => x.AlbumName).ToList(); // Sort by title in descending
                    break;
                case 2:
                    albumInfo = albumInfo.OrderBy(x => x.ArtistName).ToList(); // Sort by artist in ascending
                    break;
                case 3:
                    albumInfo = albumInfo.OrderByDescending(x => x.ArtistName).ToList(); // Sort by artist in descending
                    break;
                default:
                    // Handle invalid selection
                    break;
            }

            return albumInfo;

            //TxtSearch.Text = string.Empty;          // Clear the search box
            //musicListViewManager.DisplayInfo(flowLayoutPanel2, false, albumInfo);
        }

        // FILTER COMBO BOXES

        public void FilterByArtistOrAlbumHandler(ComboBox activeFilterComboBox, ComboBox inactiveFilterComboBox, TextBox TxtSearch, DataGridView DgvMusicList, Func<Music, string> filterSelector)
        {
            musicList = new List<Music>(temporaryMusicList);   // Copy the music list from the tempMusicList to musicList

            // Filter the music by artist name
            if (activeFilterComboBox.SelectedIndex != 0)
            {
                inactiveFilterComboBox.SelectedIndex = 0;    // Set the album filter combo box to default value

                // Filter the music based on the selected artist name in the artist combo box
                musicList = musicList
                        .Where(s => filterSelector(s).Equals(activeFilterComboBox.SelectedValue?.ToString(), StringComparison.OrdinalIgnoreCase))  // Match by the selected artist
                        .ToList();  // Convert the filtered results to a list
            }

            //TxtSearch.Text = string.Empty;          // Clear the search box
            DgvMusicList.DataSource = musicList;    // Set the filtered music as the data source 
        }

        //public List<ArtistInfo> FilterArtist(ComboBox cbFilterAlbum)
        //{
        //    var artistInfo = musicListViewManager.GetMusicInfo(a => new ArtistInfo(MusicListViewManager.CapitalizeWords(a.Artist), a.MusicPictureMedium));

        //    if (cbFilterAlbum.SelectedIndex == 0)
        //    {
        //        return musicListViewManager.GetMusicInfo(a => new ArtistInfo(MusicListViewManager.CapitalizeWords(a.Artist), a.MusicPictureMedium));
        //    }

        //    return artistInfo
        //                .Where(s => s.Name.Equals(cbFilterAlbum.SelectedValue?.ToString(), StringComparison.OrdinalIgnoreCase))  // Match by the selected artist
        //                .ToList();  // Convert the filtered results to a list
        //}

        public List<AlbumInfo> FilterAlbum(ComboBox cbFilterArtist)
        {
            var albumInfo = musicListViewManager.GetMusicInfo(a => new AlbumInfo(MusicListViewManager.CapitalizeWords(a.Album), a.Artist, a.MusicPictureMedium, a.Duration));

            if (cbFilterArtist.SelectedIndex == 0)
            {
                return musicListViewManager.GetMusicInfo(a => new AlbumInfo(MusicListViewManager.CapitalizeWords(a.Album), a.Artist, a.MusicPictureMedium, a.Duration));
            }

            return albumInfo
                        .Where(s => s.ArtistName.Equals(cbFilterArtist.SelectedValue?.ToString(), StringComparison.OrdinalIgnoreCase))  // Match by the selected artist
                        .ToList();  // Convert the filtered results to a list
        }

        public void CleanPlaybackResources()
        {
            // Stop and dispose of the audio playback
            if (playerControls.waveOutDevice != null)
            {
                playerControls.waveOutDevice.Stop();
                playerControls.waveOutDevice.Dispose();
                playerControls.waveOutDevice = null;
            }

            if (playerControls.audioFileReader != null)
            {
                playerControls.audioFileReader.Dispose();
                playerControls.audioFileReader = null;
            }
        }

        public void ResetSeekBarAndDurationLabels(DungeonTrackBar tbSeekMusic, Label lblMusicLength, Label lblMusicDurationCtr)
        {
            // Reset the seek bar and duration labels
            tbSeekMusic.Value = 0;
            lblMusicLength.Text = TimeSpan.FromSeconds(0).ToString("hh\\:mm\\:ss");
            lblMusicDurationCtr.Text = TimeSpan.FromSeconds(0).ToString("hh\\:mm\\:ss");
        }


        public void ClearCurrentMusicInformation(Label lblShowPlayTitle, Label lblShowPlayArtist, PictureBox picBoxShowPlayPicture)
        {
            // Clear the playback information display
            lblShowPlayTitle.Text = string.Empty;
            lblShowPlayArtist.Text = string.Empty;
            picBoxShowPlayPicture.Image = Properties.Resources.default_music_picture_medium;
        }

        public void ClearDataGridView(DataGridView dgvPlayMusicQueue)
        {
            // Clear the data grid view of the play queue
            dgvPlayMusicQueue.DataSource = null;        // clear the data grid view of play list queue
            playMusicQueue.Clear();                     // Clear the play queue list
            playerControls.playMusicQueue.Clear();      // Clear the play queue list in the player controls
        }

        public void ModifyDataGridView(DataGridView dgv, System.Windows.Forms.Panel listPanel)
        {
            // Customize the data grid view when there is a data row displayed
            if (dgv.RowCount > 0)
            {
                // Hide the header title of the column
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
        }
    }
}
