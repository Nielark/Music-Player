using Modernial.Controls;
using MusicPlayer.CustomControls;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayer
{
    public partial class MiniPlayer : Form
    {
        private FormMain formMain; // Reference to the main form
        private DataGridView DgvPlayMusicQueue;
        private PlayerControls playerControls;
        //SharedMusicContext context = SharedMusicContext.GetInstance();

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

        public MiniPlayer(FormMain form, DataGridView dgvPlayMusicQueue, PlayerControls playerControls)
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            MyModule.MakeCircular(PicBoxPlayAndPause);      // Make the play or pause button circular
            TbVolume.Maximum = 100;
            this.formMain = form; // Store the reference
            this.DgvPlayMusicQueue = dgvPlayMusicQueue;

            //formMain.PlayAndPauseStateChanged += OnPlayAndPauseStateChanged;
            //formMain.ShuffleMusicStateChanged += OnShuffleMusicStateChanged;
            //formMain.RepeatMusicStateChanged += OnRepeatMusicStateChanged;
            //formMain.MuteStateChanged += OnMuteStateChanged;
            //formMain.UpdateMuteStateChanged += OnUpdateMuteStateChanged;
            //formMain.UpdateVolumeValueChanged += OnUpdateVolumeValueChanged;
            //formMain.UpdateVolumeIconChanged += OnUpdateVolumeIconChanged

            this.playerControls = playerControls;
            this.playerControls.ShuffleMusicStateChanged += OnShuffleMusicStateChanged;
            this.playerControls.PlayAndPauseStateChanged += OnPlayAndPauseStateChanged;
            this.playerControls.RepeatMusicStateChanged += OnRepeatMusicStateChanged;
            //this.playerControls.MuteStateChanged += OnMuteStateChanged;
            //this.playerControls.UpdateMuteStateChanged += OnUpdateMuteStateChanged;
            this.playerControls.UpdateVolumeValueChanged += OnUpdateVolumeValueChanged;

        }

        private void MiniPlayer_Load(object sender, EventArgs e)
        {
            //PicBoxShowPlayPicture.Image = formMain.musicList[formMain.currentMusicIndex].MusicPictureMedium;
            //playerControls.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, formMain.playMusic);
            playerControls.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, playerControls.IsPlayMusic);
            //playerControls.UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, formMain.shuffleMusic);
            playerControls.UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, playerControls.ShuffleMusic);
            //playerControls.UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, formMain.repeatMusic);
            playerControls.UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, playerControls.RepeatMusic);
            //playerControls.UpdateVolumeValueAndUI(TbVolume, formMain.mute);
            playerControls.UpdateVolumeValueAndUI(TbVolume, playerControls.Mute);
            playerControls.UpdateMuteStateUI(TbVolume);
            playerControls.UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);
            //formMain.UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);

            DisplaySongInformation();
            TimerMusicDuration.Start();
            //formMain.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        private void DisplaySongInformation()
        {
            //PicBoxShowPlayPicture.Image = formMain.miniPlayerMusicPicture;
            //LblShowPlayTitle.Text = formMain.miniPlayerTitle;
            //LblShowPlayArtist.Text = formMain.miniPlayerArtist;
            //LblMusicLength.Text = formMain.miniPlayerMusicLen;
            //TbSeekMusic.Maximum = formMain.miniPlayerTrackBarMax;

            PicBoxShowPlayPicture.Image = playerControls.miniPlayerTrackInfo.MiniPlayerMusicPicture;
            LblShowPlayTitle.Text = playerControls.miniPlayerTrackInfo.MiniPlayerTitle;
            LblShowPlayArtist.Text = playerControls.miniPlayerTrackInfo.MiniPlayerArtist;
            //LblMusicLength.Text = playerControls.miniPlayerTrackInfo.MiniPlayerMusicLen;
            TbSeekMusic.Maximum = playerControls.miniPlayerTrackInfo.MiniPlayerTrackBarMax;
        }

        // SHUFFLE MUSIC
        private void PicBoxShuffleMusic_Click(object sender, EventArgs e)
        {
            //formMain.ShuffleMusic();
            //playerControls.SetIsShuffleMusic(formMain.shuffleMusic);
            //playerControls.SetPlayMusicQueue(formMain.playMusicQueue);
            //playerControls.SetCurrentMusicIndex(formMain.currentMusicIndex);
            playerControls.ShuffleMusicHandler(DgvPlayMusicQueue);
            formMain.shuffleMusic = playerControls.ShuffleMusic;
        }

        private void OnShuffleMusicStateChanged(bool isShuffle)
        {
            playerControls.UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, isShuffle);
        }

        // SKIPBACKWARD
        private void PicBoxSkipBackward_Click(object sender, EventArgs e)
        {
            //formMain.SkipBackward();
            //PlayerControls.SkipBackward(formMain.waveOutDevice, formMain.audioFileReader);
            playerControls.SkipBackward();
        }

        // PREVIOUS MUSIC
        private void PicBoxPlayPreviousMusic_Click(object sender, EventArgs e)
        {
            playerControls.PlayPreviousMusic();
            DisplaySongInformation();
            //formMain.ResetAndStopMarqueeSettings(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            //formMain.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        // PLAY AND PAUSE
        private void PicBoxPlayAndPause_Click(object sender, EventArgs e)
        {
            //formMain.TogglePlayAndPause();
            //playerControls.SetIsPlayMusic(formMain.playMusic);
            //playerControls.TogglePlayAndPause(formMain.waveOutDevice, formMain.audioFileReader);
            playerControls.TogglePlayAndPause();
            //formMain.playMusic = playerControls.IsPlayMusic;
        }

        private void OnPlayAndPauseStateChanged(bool isPlaying)
        {
            // Update the UI of MiniPlayer based on the new play/pause state
            //formMain.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, isPlaying);
            playerControls.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, isPlaying);
        }

        // NEXT MUSIC
        private void PicBoxPlayNextMusic_Click(object sender, EventArgs e)
        {
            playerControls.PlayNextMusic();
            DisplaySongInformation();
            //formMain.ResetAndStopMarqueeSettings(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            //formMain.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        // SKIP FORWARD
        private void PicBoxSkipForward_Click(object sender, EventArgs e)
        {
            //formMain.SkipForward();
            //PlayerControls.SkipForward(formMain.waveOutDevice, formMain.audioFileReader);
            playerControls.SkipForward();
        }

        // REPEAT
        private void PicBoxRepeatMusic_Click(object sender, EventArgs e)
        {
            //formMain.RepeatMusic();
            //playerControls.SetIsRepeatMusic(formMain.repeatMusic);
            playerControls.RepeatMusicHandler();
            //formMain.repeatMusic = playerControls.RepeatMusic;
        }

        private void OnRepeatMusicStateChanged(bool isRepeat)
        {
            //formMain.UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, isRepeat);
            playerControls.UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, isRepeat);
        }

        // MINI PLAYER
        private void PicBoxMiniPlayerToggle_Click(object sender, EventArgs e)
        {
            this.Close();
            formMain.ReturnToMainForm();
        }

        private void TimerMusicDuration_Tick(object sender, EventArgs e)
        {
            //formMain.MusicSeekBarHandler(TbSeekMusic, LblMusicLength, LblMusicDurationCtr, TimerMusicDuration);
            playerControls.MusicSeekBarHandler(TbSeekMusic, LblMusicLength, LblMusicDurationCtr, TimerMusicDuration);
            //playerControls.MusicSeekBarHandler();
            DisplaySongInformation();
            //formMain.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            //playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            TimerMusicDuration.Start();
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

        private void PicBoxShowVolumeBar_Click(object sender, EventArgs e)
        {
            // Display the volume bar
            PnlVolumeControl.Visible = true;
        }

        private void PicBoxVolumePicture_Click(object sender, EventArgs e)
        {
            //formMain.VolumeMuteAndUnmuteToggle();
            playerControls.VolumeMuteAndUnmuteToggle();
        }

        private void OnMuteStateChanged(bool isMute)
        {
            //formMain.UpdateVolumeValueAndUI(TbVolume, isMute);
            playerControls.UpdateVolumeValueAndUI(TbVolume, isMute);
        }

        private void PnlVolumeControl_MouseLeave(object sender, EventArgs e)
        {
            playerControls.PnlVolumeControlMousePositionHandler(PnlVolumeControl, PicBoxVolumePicture, TbVolume, LblVolumeValue);
        }

        private void TbVolume_ValueChanged()
        {
            // Update volume and icon based on the current trackbar value
            //formMain.UpdateVolumeValue(TbVolume.Value);
            playerControls.UpdateVolumeValue(TbVolume.Value);
            playerControls.UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);
            //formMain.UpdateVolumeIcon();
        }

        private void OnUpdateVolumeValueChanged(int newVolumeValue)
        {
            TbVolume.Value = newVolumeValue;
            //formMain.UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);
            playerControls.UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);
        }

        //private void OnUpdateVolumeIconChanged()
        //{
        //    formMain.UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);
        //}

        private void TbVolume_MouseUp(object sender, MouseEventArgs e)
        {
            //formMain.UpdateMuteState();
            playerControls.UpdateMuteState();
        }

        private void OnUpdateMuteStateChanged()
        {
            //formMain.UpdateMuteStateUI(TbVolume);
            playerControls.UpdateMuteStateUI(TbVolume);
        }

        private void TimerTitleMarquee_Tick(object sender, EventArgs e)
        {
            //formMain.TitleMarqueeEffectHandler(LblShowPlayTitle, PnlMarquee);
            playerControls.TitleMarqueeEffectHandler(LblShowPlayTitle, PnlMarquee);
        }

        private void TimerArtistMarquee_Tick(object sender, EventArgs e)
        {
            //formMain.ArtistMarqueeEffectHandler(LblShowPlayArtist, PnlMarquee);
            playerControls.ArtistMarqueeEffectHandler(LblShowPlayArtist, PnlMarquee);
        }

        private void TimerShowRightBar_Tick(object sender, EventArgs e)
        {
            if (pnlRightBar.Left > 350)
            {
                pnlRightBar.Left -= 25;
            }
            else
            {
                TimerShowRightBar.Stop();
            }
        }

        private void PicBoxDisplayRightBar_Click(object sender, EventArgs e)
        {
            PicBoxDisplayRightBar.Visible = false;
            TimerShowRightBar.Start();
        }

        private void PnlHideRightBar_Click(object sender, EventArgs e)
        {
            TimerHideRightBar.Start();
        }

        private void TimerHideRightBar_Tick(object sender, EventArgs e)
        {
            if (pnlRightBar.Left < 400)
            {
                pnlRightBar.Left += 25;
            }
            else
            {
                TimerHideRightBar.Stop();
                PicBoxDisplayRightBar.Visible = true;
            }
        }

        private void PnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            MyModule.StartDrag(e);
        }

        private void PnlHeader_MouseMove(object sender, MouseEventArgs e)
        {
            MyModule.MovedDrag(this, e);
        }

        private void PnlHeader_MouseUp(object sender, MouseEventArgs e)
        {
            MyModule.EndDrag();
        }
    }
}
