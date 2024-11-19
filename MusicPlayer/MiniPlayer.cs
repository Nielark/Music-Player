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

        public MiniPlayer(FormMain form)
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            MyModule.MakeCircular(PicBoxPlayAndPause);      // Make the play or pause button circular
            TbVolume.Maximum = 100;
            this.formMain = form; // Store the reference

            formMain.PlayAndPauseStateChanged += OnPlayAndPauseStateChanged;
            formMain.ShuffleMusicStateChanged += OnShuffleMusicStateChanged;
            formMain.RepeatMusicStateChanged += OnRepeatMusicStateChanged;
            //formMain.MuteStateChanged += OnMuteStateChanged;
            //formMain.UpdateMuteStateChanged += OnUpdateMuteStateChanged;
            formMain.UpdateVolumeValueChanged += OnUpdateVolumeValueChanged;
            //formMain.UpdateVolumeIconChanged += OnUpdateVolumeIconChanged;
        }

        private void MiniPlayer_Load(object sender, EventArgs e)
        {
            PicBoxShowPlayPicture.Image = formMain.musicList[formMain.currentMusicIndex].MusicPictureMedium;

            formMain.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, formMain.playMusic);
            formMain.UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, formMain.shuffleMusic);
            formMain.UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, formMain.repeatMusic);
            formMain.UpdateVolumeValueAndUI(TbVolume, formMain.mute);
            formMain.UpdateMuteStateUI(TbVolume);
            formMain.UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);
            //formMain.UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);

            DisplaySongInformation();
            TimerMusicDuration.Start();
            formMain.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        private void DisplaySongInformation()
        {
            PicBoxShowPlayPicture.Image = formMain.miniPlayerMusicPicture;
            LblShowPlayTitle.Text = formMain.miniPlayerTitle;
            LblShowPlayArtist.Text = formMain.miniPlayerArtist;
            //LblMusicLength.Text = formMain.miniPlayerMusicLen;
            TbSeekMusic.Maximum = formMain.miniPlayerTrackBarMax;
        }

        private void PicBoxShuffleMusic_Click(object sender, EventArgs e)
        {
            formMain.ShuffleMusic();
        }

        private void OnShuffleMusicStateChanged(bool isShuffle)
        {
            formMain.UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, isShuffle);
        }

        private void PicBoxSkipBackward_Click(object sender, EventArgs e)
        {
            formMain.SkipBackward();
        }

        private void PicBoxPlayPreviousMusic_Click(object sender, EventArgs e)
        {
            formMain.PlayPreviousMusic();
            DisplaySongInformation();
            //formMain.ResetAndStopMarqueeSettings(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            formMain.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        private void PicBoxPlayAndPause_Click(object sender, EventArgs e)
        {
            formMain.TogglePlayAndPause();
        }

        private void OnPlayAndPauseStateChanged(bool isPlaying)
        {
            // Update the UI of MiniPlayer based on the new play/pause state
            formMain.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, isPlaying);
        }

        private void PicBoxPlayNextMusic_Click(object sender, EventArgs e)
        {
            formMain.PlayNextMusic();
            DisplaySongInformation();
            //formMain.ResetAndStopMarqueeSettings(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            formMain.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        private void PicBoxSkipForward_Click(object sender, EventArgs e)
        {
            formMain.SkipForward();
        }

        private void PicBoxRepeatMusic_Click(object sender, EventArgs e)
        {
            formMain.RepeatMusic();
        }

        private void OnRepeatMusicStateChanged(bool isRepeat)
        {
            formMain.UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, isRepeat);
        }

        private void PicBoxMiniPlayerToggle_Click(object sender, EventArgs e)
        {
            this.Close();
            formMain.ReturnToMainForm();
        }

        private void TimerMusicDuration_Tick(object sender, EventArgs e)
        {
            formMain.MusicSeekBarHandler(TbSeekMusic, LblMusicLength, LblMusicDurationCtr, TimerMusicDuration);
            DisplaySongInformation();
            formMain.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
            TimerMusicDuration.Start();
        }

        private void TbSeekMusic_ValueChanged()
        {
            formMain.UpdateMusicLengthAndDuration(TbSeekMusic, LblMusicLength, LblMusicDurationCtr);
        }

        private void TbSeekMusic_MouseUp(object sender, MouseEventArgs e)
        {
            formMain.MusicSeekBarMouseUpHandler(TbSeekMusic);
        }

        private void TbSeekMusic_MouseDown(object sender, MouseEventArgs e)
        {
            formMain.MusicSeekBarMouseDownHandler();
        }

        private void PicBoxShowVolumeBar_Click(object sender, EventArgs e)
        {
            // Display the volume bar
            PnlVolumeControl.Visible = true;
        }

        private void PicBoxVolumePicture_Click(object sender, EventArgs e)
        {
            formMain.VolumeMuteAndUnmuteToggle();
        }

        private void OnMuteStateChanged(bool isMute)
        {
            formMain.UpdateVolumeValueAndUI(TbVolume, isMute);
        }

        private void PnlVolumeControl_MouseLeave(object sender, EventArgs e)
        {
            formMain.PnlVolumeControlMousePositionHandler(PnlVolumeControl, PicBoxVolumePicture, TbVolume, LblVolumeValue);
        }

        private void TbVolume_ValueChanged()
        {
            // Update volume and icon based on the current trackbar value
            formMain.UpdateVolumeValue(TbVolume.Value);
            formMain.UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);
            //formMain.UpdateVolumeIcon();
        }

        private void OnUpdateVolumeValueChanged(int newVolumeValue)
        {
            TbVolume.Value = newVolumeValue;
            formMain.UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);
        }

        //private void OnUpdateVolumeIconChanged()
        //{
        //    formMain.UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);
        //}

        private void TbVolume_MouseUp(object sender, MouseEventArgs e)
        {
            formMain.UpdateMuteState();
        }

        private void OnUpdateMuteStateChanged()
        {
            formMain.UpdateMuteStateUI(TbVolume);
        }

        private void TimerTitleMarquee_Tick(object sender, EventArgs e)
        {
            formMain.TitleMarqueeEffectHandler(LblShowPlayTitle, PnlMarquee);
        }

        private void TimerArtistMarquee_Tick(object sender, EventArgs e)
        {
            formMain.ArtistMarqueeEffectHandler(LblShowPlayArtist, PnlMarquee);
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
