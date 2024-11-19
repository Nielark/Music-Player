using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class MyMusic
    {
        public Image MusicPictureSmall { get; set; }
        public Image MusicPictureMedium { get; set; }
        public string Title {  get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Duration { get; set; }
        public string File { get; set; }

        public MyMusic(Image musicPictureSmall, Image musicPictureMedium, string title, string artist, string album, TimeSpan duration, string file)
        {
            MusicPictureSmall = musicPictureSmall;
            MusicPictureMedium = musicPictureMedium;
            Title = title;
            Artist = artist;
            Album = album;
            Duration = duration.ToString("hh\\:mm\\:ss");
            File = file;
        }
    }
}
