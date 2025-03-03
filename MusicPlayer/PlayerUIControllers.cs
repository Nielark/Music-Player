using Modernial.Controls;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class PlayerUIControllers
    {
        // Player UI Controllers
        public PictureBox? PicBoxShowPlayPicture { get; set; }
        public Label? LblShowPlayTitle { get; set; }
        public Label? LblShowPlayArtist { get; set; }
        public Label? LblMusicLength { get; set; }
        public Label? LblMusicDurationCtr { get; set; }
        public DungeonTrackBar? TbSeekMusic { get; set; }
        public DungeonTrackBar? TbVolume { get; set; }
        public System.Windows.Forms.Timer? TimerMusicDuration { get; set; }
        public System.Windows.Forms.Panel? PnlMarquee {  get; set; }
        public System.Windows.Forms.Timer? TimerTitleMarquee { get; set; }
        public System.Windows.Forms.Timer? TimerArtistMarquee { get; set; }
    }
}
