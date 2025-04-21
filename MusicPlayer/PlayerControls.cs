using Modernial.Controls;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TagLib;

namespace MusicPlayer
{
    public class PlayerControls : MusicPlayerBase
    {
        private const int marqueeSpeed = 5;
        public int temporaryVolume = 100;
        public int selectedRowIndex = -1;

        public PlayerControls()
        {
            // Constructor
        }

        public void SetPlayMusicQueue(List<Music> getPlayMusicQueue)
        {
            playMusicQueue = new List<Music>(getPlayMusicQueue);
        }

        public void SetIsShuffleMusic(bool getShuffleMusic)
        {
            ShuffleMusic = getShuffleMusic;
        }

        public void SetIsPlayMusic(bool getPlayMusic)
        {
            IsPlayMusic = getPlayMusic;
        }

        public void SetIsRepeatMusic(bool getRepeatMusic)
        {
            RepeatMusic = getRepeatMusic;
        }

        public void SetCurrentMusicIndex(int getCurrentMusicIndex)
        {
            CurrentMusicIndex = getCurrentMusicIndex;
        }

        // SHUFFLE AND PLAY MUSIC

        public void ShuffleAndPlayMusic(DataGridView DgvPlayMusicQueue, System.Windows.Forms.Timer TimerShowPnlPlayerControls, MusicLibrary musicLibrary)
        {
            ShuffleMusic = true;

            playMusicQueue = new List<Music>(musicLibrary.musicList);      // Copy the current music list to playMusicQueue for shuffling

            FisherYatesShuffle();                               // Shuffle the music list using the Fisher-Yates algorithm
            DgvPlayMusicQueue.DataSource = shuffleMusicList;    // Set shuffleMusicList as the data source and display in data grid view

            // Get the music file information from the first index in the shuffled list
            string filePath = shuffleMusicList[0].File;
            string Title = shuffleMusicList[0].Title;
            string Artist = shuffleMusicList[0].Artist;

            int index = shuffleMusicList.FindIndex(index => index.File == filePath);    // Find the index of the current music in the shuffled list based on file path
            Image musicPicture = shuffleMusicList[index].MusicPictureMedium;            // Get the music image

            PlayMusic(musicPicture, filePath, Title, Artist);   // Function call to play the music

            TimerShowPnlPlayerControls.Start();

            NotifyShuffleMusicStateChanged(ShuffleMusic);
        }

        // SHUFFLE MUSIC

        public delegate void ShuffleMusicStateChangedHandler(bool isShuffled);
        public event ShuffleMusicStateChangedHandler? ShuffleMusicStateChanged;

        public void ShuffleMusicHandler(DataGridView DgvPlayMusicQueue)
        {
            if (playMusicQueue.Count > 0)   // Ensure there are songs in the queue
            {
                string currentFilePath;

                ShuffleMusic = !ShuffleMusic;           // Toggle shuffle mode
                
                // Shuffle or revert to original list
                if (ShuffleMusic)
                {
                    currentFilePath = playMusicQueue[CurrentMusicIndex].File;       // Get the file path of the current playing mp3 from playMusicQueue
                    FisherYatesShuffle(currentFilePath);                            // Shuffle the music list, preserving the current song
                    DgvPlayMusicQueue.DataSource = shuffleMusicList;                // Display shuffled list in the 
                }
                else
                {
                    currentFilePath = shuffleMusicList[CurrentMusicIndex].File;     // Get the file path of the current playing mp3 from shuffleMusicList
                    shuffleMusicList = shuffleMusicList.OrderBy(x => x.Title).ToList();
                    DgvPlayMusicQueue.DataSource = shuffleMusicList;
                    CurrentMusicIndex = playMusicQueue.FindIndex(index => index.File == currentFilePath);   // Find the index of the current song in the unshuffled playMusicQueue

                    // If the song is not found in the unshuffled list, select the first song
                    if (CurrentMusicIndex == -1)
                    {
                        CurrentMusicIndex = 0;
                    }
                }

                NotifyShuffleMusicStateChanged(ShuffleMusic);
            }
        }

        public void NotifyShuffleMusicStateChanged(bool isShuffle)
        {
            ShuffleMusicStateChanged?.Invoke(isShuffle);
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

        public void FisherYatesShuffle(string? currentFilePath = null)
        {
            // Fisher-Yates Shuffle (aka Knuth Shuffle)
            Random random = new Random();

            // Copy the music list from playMusicQueue to shuffleMusicList
            shuffleMusicList = new List<Music>(playMusicQueue);

            // Shuffle the music
            for (int i = shuffleMusicList.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(0, i + 1);                // Generate a random index between 0 and i inclusively

                Music tempMusicList = shuffleMusicList[i];            // Initialize a temporary list to temporary store the current song at index i
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
                        Music tempMusicList = shuffleMusicList[0];    // Initialize a temporary list to temporary store the current song at the first index
                        shuffleMusicList[0] = shuffleMusicList[i];      // Place the current playing song at the first index
                        shuffleMusicList[i] = tempMusicList;            // Place the temporary stored song at the identified index position
                        break;
                    }
                }
            }

            CurrentMusicIndex = 0;  // initialize back to 0 after shuffle
        }

        // SKIP BACKWARD

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

        // PLAY PREVIOUS MUSIC
        public void PlayPreviousMusic()
        {
            var TbSeekMusic = uiControllers.TbSeekMusic;

            // Play previous track
            if (CurrentMusicIndex > 0)
            {
                CurrentMusicIndex--;            // Move to the previous track in the queue
                NextAndPreviousMusicHandler();  // Function call to play the next or previous music
            }
        }

        // PLAY AND PAUSE

        public delegate void PlayAndPauseStateChangedHandler(bool isPlaying);
        public event PlayAndPauseStateChangedHandler? PlayAndPauseStateChanged;

        public void TogglePlayAndPause()
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                // Toggle between playing and pausing the music
                if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    waveOutDevice.Pause();      // Pause the music
                    IsPlayMusic = false;        // Update state to indicate music is paused
                }
                else
                {
                    waveOutDevice.Play();       // Play the music
                    IsPlayMusic = true;         // Update state to indicate music is playing
                }

                // Trigger the event to notify that play/pause state has changed
                NotifyPlayAndPauseStateChanged(IsPlayMusic);
            }
        }

        private void NotifyPlayAndPauseStateChanged(bool playMusic)
        {
            PlayAndPauseStateChanged?.Invoke(playMusic);
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

        // PLAY NEXT MUSIC
        
        public void PlayNextMusic()
        {
            // Play the next track in the queue
            if (CurrentMusicIndex < playMusicQueue.Count - 1)
            {
                CurrentMusicIndex++;            // Move to the next track in the queue
                NextAndPreviousMusicHandler();  // Function call to handle playing the next or previous track
            }
        }

        // SKIP FORWARD

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

        // REPEAT MUSIC

        public delegate void RepeatMusicStateChangedHandler(bool isRepeat);
        public event RepeatMusicStateChangedHandler? RepeatMusicStateChanged;

        public void RepeatMusicHandler()
        {
            RepeatMusic = !RepeatMusic;     // Toggle repeat music

            NotifyRepeatMusicStateChanged(RepeatMusic);
        }

        private void NotifyRepeatMusicStateChanged(bool repeatMusic)
        {
            RepeatMusicStateChanged?.Invoke(repeatMusic);
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

        public void NextAndPreviousMusicHandler()
        {
            var TbSeekMusic = uiControllers.TbSeekMusic!;
            
            if (playMusicQueue.Count > 0)   // Ensure there are songs in the queue
            {
                string filePath, title, artist;
                Image musicPicture;

                // Play the next or previous song, considering shuffle mode
                if (!ShuffleMusic)
                {
                    // Retrieve the music info from the non-shuffled queue
                    musicPicture = playMusicQueue[CurrentMusicIndex].MusicPictureMedium;
                    filePath = playMusicQueue[CurrentMusicIndex].File;
                    title = playMusicQueue[CurrentMusicIndex].Title;
                    artist = playMusicQueue[CurrentMusicIndex].Artist;

                    TbSeekMusic.Value = 0;                              // Reset the track bar to the beginning of the song
                    PlayMusic(musicPicture, filePath, title, artist);   // Function call to play the music
                }
                else
                {
                    // Retrieve the music info from the non-shuffled queue
                    musicPicture = shuffleMusicList[CurrentMusicIndex].MusicPictureMedium;
                    filePath = shuffleMusicList[CurrentMusicIndex].File;
                    title = shuffleMusicList[CurrentMusicIndex].Title;
                    artist = shuffleMusicList[CurrentMusicIndex].Artist;

                    TbSeekMusic.Value = 0;                              // Reset the track bar to the beginning of the song
                    PlayMusic(musicPicture, filePath, title, artist);   // Function call to play the music
                }
            }
        }

        public void PlayMusic(Image musicPicture, string filePath, string title, string artist)
        {
            var PicBoxShowPlayPicture = uiControllers.PicBoxShowPlayPicture!;
            var LblShowPlayTitle = uiControllers.LblShowPlayTitle!;
            var LblShowPlayArtist = uiControllers.LblShowPlayArtist!;
            var LblMusicLength = uiControllers.LblMusicLength!;
            var TbSeekMusic = uiControllers.TbSeekMusic!;
            var TimerMusicDuration = uiControllers.TimerMusicDuration!;
            var PnlMarquee = uiControllers.PnlMarquee!;
            var TbVolume = uiControllers.TbVolume!;
            var TimerTitleMarquee = uiControllers.TimerTitleMarquee!;
            var TimerArtistMarquee = uiControllers.TimerArtistMarquee!;

            if (playMusicQueue != null)
            {
                // Find the index of the song in the queue (shuffled or not) based on file path
                if (!ShuffleMusic)
                {
                    CurrentMusicIndex = playMusicQueue.FindIndex(index => index.File == filePath);
                }
                else
                {
                    CurrentMusicIndex = shuffleMusicList.FindIndex(index => index.File == filePath);
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
                waveOutDevice.Init(audioFileReader);  // Initialize playback with audio file
                waveOutDevice.Play();                               // Start playing the music
                UpdateVolumeValue(TbVolume.Value);                                // Update volume

                // Display music image or a default one if no image is available
                if (musicPicture != null)
                {
                    PicBoxShowPlayPicture.Image = musicPicture;  
                }
                else
                {
                    PicBoxShowPlayPicture.Image = Properties.Resources.default_music_picture_medium;
                }

                // Update UI with the currently playing song information
                LblShowPlayTitle.Text = title;
                LblShowPlayArtist.Text = artist;
                LblMusicLength.Text = audioFileReader.TotalTime.ToString("hh\\:mm\\:ss");

                miniPlayerTrackInfo = new MiniPlayerTrackInfo()
                {
                    MiniPlayerMusicPicture = musicPicture ?? Properties.Resources.default_music_picture_medium,
                    MiniPlayerTitle = title,
                    MiniPlayerArtist = artist,
                    MiniPlayerMusicLen = audioFileReader.TotalTime.ToString("hh\\:mm\\:ss"),
                    MiniPlayerTrackBarMax = (int)audioFileReader.TotalTime.TotalSeconds
                };

                // Set the seek bar maximum value to the total duration of the music in seconds
                TbSeekMusic.Maximum = (int)audioFileReader.TotalTime.TotalSeconds;

                MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);

                TimerMusicDuration.Start();     // Start tracking the music duration in seek bar
                IsPlayMusic = true;               // Update play status
            }
        }

        public void MarqueeEffectHandler(Label LblShowPlayTitle, Label LblShowPlayArtist, System.Windows.Forms.Panel PnlMarquee, System.Windows.Forms.Timer TimerTitleMarquee, System.Windows.Forms.Timer TimerArtistMarquee)
        {
            if (string.IsNullOrEmpty(LblShowPlayTitle.Text) && string.IsNullOrEmpty(LblShowPlayArtist.Text))
            {
                return;
            }

            ResetMarqueeLabels(LblShowPlayTitle, LblShowPlayArtist, TimerTitleMarquee, TimerArtistMarquee);

            bool overflowTitleLabel = LblShowPlayTitle.Right > PnlMarquee.Right;
            bool overflowArtistLabel = LblShowPlayArtist.Right > PnlMarquee.Right;

            // Handle the marquee effect based on label width and panel size
            if (overflowTitleLabel && !overflowArtistLabel)
            {
                StartTitleLabelMarquee(TimerTitleMarquee, TimerArtistMarquee);
            }
            else if (!overflowTitleLabel && overflowArtistLabel)
            {
                StartArtistLabelMarquee(TimerArtistMarquee, TimerTitleMarquee);
            }
            else if (overflowTitleLabel && overflowArtistLabel)
            {
                EqualizeLabelWidth(LblShowPlayTitle, LblShowPlayArtist);
                StartTitleAndArtistLabelMarquee(TimerTitleMarquee, TimerArtistMarquee);
            }
            else
            {
                ResetMarqueeLabels(LblShowPlayTitle, LblShowPlayArtist, TimerTitleMarquee, TimerArtistMarquee);
            }
        }

        private void StartTitleLabelMarquee(System.Windows.Forms.Timer TimerTitleMarquee, System.Windows.Forms.Timer TimerArtistMarquee)
        {
            TimerTitleMarquee.Start();  // Start the title marquee timer
            TimerArtistMarquee.Stop();  // Stop the artist marquee timer
        }

        private void StartArtistLabelMarquee(System.Windows.Forms.Timer TimerArtistMarquee, System.Windows.Forms.Timer TimerTitleMarquee)
        {
            TimerArtistMarquee.Start(); // Start the artist marquee timer
            TimerTitleMarquee.Stop();   // Stop the artist marquee timer
        }

        private void StartTitleAndArtistLabelMarquee(System.Windows.Forms.Timer TimerTitleMarquee, System.Windows.Forms.Timer TimerArtistMarquee)
        {
            // Start both marquees
            TimerTitleMarquee.Start();
            TimerArtistMarquee.Start();
        }

        private void EqualizeLabelWidth(Label LblShowPlayTitle, Label LblShowPlayArtist)
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
        } 

        public void ResetMarqueeLabels(Label LblShowPlayTitle, Label LblShowPlayArtist, System.Windows.Forms.Timer TimerTitleMarquee, System.Windows.Forms.Timer TimerArtistMarquee)
        {
            // Reset the marquee labels
            LblShowPlayTitle.AutoSize = true;
            LblShowPlayArtist.AutoSize = true;
            TimerTitleMarquee.Stop();   // Stop the title marquee timer
            TimerArtistMarquee.Stop();  // Stop the artist marquee timer
            LblShowPlayTitle.Left = 0;  // Reset the title label position
            LblShowPlayArtist.Left = 0; // Reset the artist label position
        }

        public void TitleMarqueeEffectHandler(Label LblShowPlayTitle, System.Windows.Forms.Panel PnlMarquee)
        {
            LblShowPlayTitle.Left -= marqueeSpeed;

            // Check if the label has gone off-screen and wrap it back
            if (LblShowPlayTitle.Right < 0)
            {
                LblShowPlayTitle.Left = PnlMarquee.Width;
            }
        }

        public void ArtistMarqueeEffectHandler(Label LblShowPlayArtist, System.Windows.Forms.Panel PnlMarquee)
        {
            LblShowPlayArtist.Left -= marqueeSpeed;

            // Check if the label has gone off-screen and wrap it back
            if (LblShowPlayArtist.Right < 0)
            {
                LblShowPlayArtist.Left = PnlMarquee.Width;
            }
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
                    if (!RepeatMusic)
                    {
                        CurrentMusicIndex++;    // Go to the next track in the queue
                    }

                    if (CurrentMusicIndex < playMusicQueue.Count)
                        NextAndPreviousMusicHandler();  // Play the next or previous track in the queue
                }
            }
        }

        // MUTE TOGGLE

        public delegate void MuteStateChangedHandler(bool isMute);
        public event MuteStateChangedHandler? MuteStateChanged;

        public void VolumeMuteAndUnmuteToggle()
        {
            //var audioFileReader = uiControllers.audioFileReader;

            if (audioFileReader != null)
            {
                Mute = !Mute;   // Toggle mute status

                NotifyMuteStateChanged(Mute);
            }
        }

        private void NotifyMuteStateChanged(bool isMute)
        {
            MuteStateChanged?.Invoke(isMute);
        }

        public void UpdateVolumeValueAndUI(DungeonTrackBar TbVolume, bool isMute)
        {
            // Check and mute or unmute the volume
            if (isMute)
            {
                temporaryVolume = TbVolume.Value;        // Temporary store the current volume value before muting
                TbVolume.Value = 0;                 // Set the trackbar to 0
            }
            else
            {
                TbVolume.Value = temporaryVolume;        // Restore the previous value when unmuted
            }
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

        // MUTE UPDATE

        public delegate void UpdateMuteStateChangeHandler();
        public event UpdateMuteStateChangeHandler? UpdateMuteStateChanged;

        public void UpdateMuteState()
        {
            NotifyUpdateMuteStateChange();
        }

        private void NotifyUpdateMuteStateChange()
        {
            UpdateMuteStateChanged?.Invoke();
        }

        public void UpdateMuteStateUI(DungeonTrackBar TbVolume)
        {
            if (TbVolume.Value > 0)
            {
                Mute = false;
                temporaryVolume = TbVolume.Value;    // Temporary store the current volume value
            }
            else
            {
                Mute = true;
            }
        }

        // UPDATE VOLUME VALUE

        public delegate void UpdateVolumeValueChangedHandler(int newVolumeValue);
        public event UpdateVolumeValueChangedHandler? UpdateVolumeValueChanged;

        public void UpdateVolumeValue(int newVolumeValue)
        {
            NotifyUpdateVolumeValueChanged(newVolumeValue);
        }

        private void NotifyUpdateVolumeValueChanged(int newVolumeValue)
        {
            UpdateVolumeValueChanged?.Invoke(newVolumeValue);
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

        // MUSIC TRACKBAR

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

        public void MusicSeekBarMouseUpHandler(DungeonTrackBar TbSeekMusic)
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                // Update the current playback time of the track based on the seek bar's value
                audioFileReader.CurrentTime = TimeSpan.FromSeconds(TbSeekMusic.Value);

                // Resume playback if the music is currently playing
                if (IsPlayMusic)
                {           
                    waveOutDevice.Play();
                }
            }
        }

        public void MusicSeekBarMouseDownHandler()
        {
            if (audioFileReader != null && waveOutDevice != null)
            {
                waveOutDevice.Stop();   // Stop the playback when the user presses down on the seek bar
            }
        }

        // More Options

        public void DisplayMusicProperties()
        {

        }

        public void FullScreenOption()
        {

        }
    }
}
