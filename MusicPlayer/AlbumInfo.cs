using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class AlbumInfo : MusicInfo
    {
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public Image AlbumImage { get; set; }
        public string Duration { get; set; }

        public string Name => AlbumName;
        public Image DisplayImage => AlbumImage;

        public AlbumInfo(string albumName, string artistName, Image albumImage, string duration)
        {
            AlbumName = albumName;
            ArtistName = artistName;
            AlbumImage = albumImage;
            Duration = duration;
        }

        // Override Equals and GetHashCode for proper distinct filtering
        public override bool Equals(object? obj)
        {
            if (obj is AlbumInfo other)
            {
                return AlbumName == other.AlbumName; // Compare by ArtistName only (or add more conditions if needed)
            }
            return false;
        }

        public override int GetHashCode()
        {
            return AlbumName.GetHashCode(); // Use ArtistName's hash code (or a combination of properties)
        }
    }
}
