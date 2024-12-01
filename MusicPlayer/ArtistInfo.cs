using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class ArtistInfo : MusicInfo
    {
        public string ArtistName {  get; set; }
        public Image ArtistImage { get; set; }

        public string Name => ArtistName;
        public Image DisplayImage => ArtistImage;


        public ArtistInfo(string artistName, Image artistImage) 
        {
            ArtistName = artistName;
            ArtistImage = artistImage;
        }

        // Override Equals and GetHashCode for proper distinct filtering
        public override bool Equals(object? obj)
        {
            if (obj is ArtistInfo other)
            {
                return ArtistName == other.ArtistName; // Compare by ArtistName only (or add more conditions if needed)
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ArtistName.GetHashCode(); // Use ArtistName's hash code (or a combination of properties)
        }
    }
}
