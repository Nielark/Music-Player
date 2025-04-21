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
        private DataGridView dgvPlayMusicQueue;
        private PlayerControls playerControls;

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

            // Set up appearance
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            MyModule.MakeCircular(PicBoxPlayAndPause);      // Make the play or pause button circular
            TbVolume.Maximum = 100;

            // Store references passed from the MainForm
            this.formMain = form; // Store the reference
            this.dgvPlayMusicQueue = dgvPlayMusicQueue;
            this.playerControls = playerControls;

            // Wire up events from the playerControls that are UI-related
            this.playerControls.ShuffleMusicStateChanged += OnShuffleMusicStateChanged;
            this.playerControls.PlayAndPauseStateChanged += OnPlayAndPauseStateChanged;
            this.playerControls.RepeatMusicStateChanged += OnRepeatMusicStateChanged;
            this.playerControls.UpdateVolumeValueChanged += OnUpdateVolumeValueChanged;
        }

        private void MiniPlayer_Load(object sender, EventArgs e)
        {
            // Update the UI based on current playback state
            playerControls.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, playerControls.IsPlayMusic);
            playerControls.UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, playerControls.ShuffleMusic);
            playerControls.UpdateRepeatUI(PicBoxRepeatMusic, toolTipPlayerControl, playerControls.RepeatMusic);
            playerControls.UpdateVolumeValueAndUI(TbVolume, playerControls.Mute);
            playerControls.UpdateMuteStateUI(TbVolume);
            playerControls.UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);

            // Load current song information and start related timers
            DisplaySongInformation();
            TimerMusicDuration.Start();
            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        private void DisplaySongInformation()
        {
            PicBoxShowPlayPicture.Image = playerControls.miniPlayerTrackInfo.MiniPlayerMusicPicture;
            LblShowPlayTitle.Text = playerControls.miniPlayerTrackInfo.MiniPlayerTitle;
            LblShowPlayArtist.Text = playerControls.miniPlayerTrackInfo.MiniPlayerArtist;
            TbSeekMusic.Maximum = playerControls.miniPlayerTrackInfo.MiniPlayerTrackBarMax;
        }

        // SHUFFLE MUSIC
        private void PicBoxShuffleMusic_Click(object sender, EventArgs e)
        {
            playerControls.ShuffleMusicHandler(dgvPlayMusicQueue);
        }

        private void OnShuffleMusicStateChanged(bool isShuffle)
        {
            playerControls.UpdateShuffleUI(PicBoxShuffleMusic, toolTipPlayerControl, isShuffle);
        }

        // SKIPBACKWARD
        private void PicBoxSkipBackward_Click(object sender, EventArgs e)
        {
            playerControls.SkipBackward();
        }

        // PREVIOUS MUSIC
        private void PicBoxPlayPreviousMusic_Click(object sender, EventArgs e)
        {
            playerControls.PlayPreviousMusic();
            DisplaySongInformation();
            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        // PLAY AND PAUSE
        private void PicBoxPlayAndPause_Click(object sender, EventArgs e)
        {
            playerControls.TogglePlayAndPause();
        }

        private void OnPlayAndPauseStateChanged(bool isPlaying)
        {
            playerControls.UpdatePlayAndPauseUI(PicBoxPlayAndPause, toolTipPlayerControl, isPlaying);
        }

        // NEXT MUSIC
        private void PicBoxPlayNextMusic_Click(object sender, EventArgs e)
        {
            playerControls.PlayNextMusic();
            DisplaySongInformation();
            playerControls.MarqueeEffectHandler(LblShowPlayTitle, LblShowPlayArtist, PnlMarquee, TimerTitleMarquee, TimerArtistMarquee);
        }

        // SKIP FORWARD
        private void PicBoxSkipForward_Click(object sender, EventArgs e)
        {
            playerControls.SkipForward();
        }

        // REPEAT
        private void PicBoxRepeatMusic_Click(object sender, EventArgs e)
        {
            playerControls.RepeatMusicHandler();
        }

        private void OnRepeatMusicStateChanged(bool isRepeat)
        {
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
            playerControls.MusicSeekBarHandler(TbSeekMusic, LblMusicLength, LblMusicDurationCtr, TimerMusicDuration);
            DisplaySongInformation();
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

        private void TbVolume_ValueChanged()
        {
            playerControls.UpdateVolumeValue(TbVolume.Value);
            playerControls.UpdateVolumeIconUI(TbVolume, PicBoxVolumePicture, PicBoxShowVolumeBar);
        }

        private void OnUpdateVolumeValueChanged(int newVolumeValue)
        {
            TbVolume.Value = newVolumeValue;
            playerControls.UpdateVolumeValueAndUI(LblVolumeValue, TbVolume);
        }

        private void TbVolume_MouseUp(object sender, MouseEventArgs e)
        {
            playerControls.UpdateMuteState();
        }

        private void OnUpdateMuteStateChanged()
        {
            playerControls.UpdateMuteStateUI(TbVolume);
        }

        private void TimerTitleMarquee_Tick(object sender, EventArgs e)
        {
            playerControls.TitleMarqueeEffectHandler(LblShowPlayTitle, PnlMarquee);
        }

        private void TimerArtistMarquee_Tick(object sender, EventArgs e)
        {
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
