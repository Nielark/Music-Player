using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    internal class MusicArtist
    {
        private string filePath;
        private string artistName;

        public MusicArtist(string filePath, string artistName)
        {
            this.filePath = filePath;
            this.artistName = artistName;
        }

        public string GetArtistName()
        {
            return artistName;
        }

        public string GetFilePath()
        {
            return filePath;
        }
    }
}
