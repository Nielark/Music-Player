using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class MusicPlayerBase
    {
        public IWavePlayer? waveOutDevice;
        public AudioFileReader? audioFileReader;

        public PlayerUIControllers uiControllers = new PlayerUIControllers();
        public MiniPlayerTrackInfo miniPlayerTrackInfo = new MiniPlayerTrackInfo();

        public List<Music> musicList = new List<Music>();
        public List<Music> temporaryMusicList = new List<Music>();
        public List<Music> playMusicQueue = new List<Music>();
        public List<Music> shuffleMusicList = new List<Music>();

        public bool ShuffleMusic { get; set; } = false;
        public bool IsPlayMusic { get; set; } = false;
        public bool RepeatMusic { get; set; } = false;
        public bool Mute { get; set; } = false;
        public int CurrentMusicIndex { get; set; }

        public void SetPlayerUIControllers(PlayerUIControllers uIControllers)
        {
            uiControllers = uIControllers;
        }
    }
}
