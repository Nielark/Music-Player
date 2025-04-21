using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    internal class SharedMusicContext
    {
        // Singleton instance
        private static SharedMusicContext _instance;

        public IWavePlayer WaveOutDevice { get; set; }

        public AudioFileReader AudioFileReader { get; set; }

        // List to hold music data
        public List<Music> MusicList { get; set; }

        // Current song index
        public int CurrentMusicIndex { get; set; } = -1;

        // Volume, seek position, etc.
        public int Volume { get; set; }

        public TimeSpan SeekPosition { get; set; }

        public bool PlayMusic { get; set; } = false;


        // Private constructor to prevent instantiation
        private SharedMusicContext()
        {
            MusicList = new List<Music>();
        }

        // Method to get the singleton instance
        public static SharedMusicContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SharedMusicContext();
            }
            return _instance;
        }
    }
}
