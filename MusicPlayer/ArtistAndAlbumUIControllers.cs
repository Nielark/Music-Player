using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class ArtistAndAlbumUIControllers
    {
        public Panel? PnlInfo { get; set; }
        public Panel? PnlHeaderControl { get; set; }
        public Panel? PnlMusicLibrary { get; set; }
        public Panel? PnlPlayMusicQueue { get; set; }
        public DataGridView? DgvPlayMusicQueue { get; set; }
        public System.Windows.Forms.Timer? TimerShowPnlPlayerControls { get; set; }

        public PictureBox? PicBoxAlbumImage { get; set; }
        public Label? LblAlbumName { get; set; }
        public Label? LblAlbumArtist { get; set; }
        public Label? LblAlbumNumbers { get; set; }

        public ToolTip? ToolTipPlayerControl { get; set; } = new ToolTip();
    }
}
