using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    internal class MyMusic
    {
        public Image AlbumCover { get; set; }
        public string Title {  get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Duration { get; set; }
        public string File { get; set; }

        public MyMusic(Image albumCover, string title, string artist, string album, TimeSpan duration, string file)
        {
            AlbumCover = albumCover;
            Title = title;
            Artist = artist;
            Album = album;
            Duration = duration.ToString("hh\\:mm\\:ss");
            File = file;
        }
    }
}
