using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    internal class MusicDuration
    {
        private string filePath;
        private TimeSpan musicDuration;

        public MusicDuration (string filePath, TimeSpan musicDuration)
        {
            this.filePath = filePath;
            this.musicDuration = musicDuration;
        }

        public TimeSpan GetMusicDuration()
        {
            return musicDuration;
        }

        public string GetFilePath()
        {
            return filePath;
        }
    }
}
