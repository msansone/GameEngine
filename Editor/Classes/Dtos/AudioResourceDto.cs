using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonOgg.MediaPlayer;

namespace FiremelonEditor2
{
    public class AudioResourceDto : ExternalResourceDto
    {
        private byte[] audioData_;
        public byte[] AudioData
        {
            get { return audioData_; }
            set { audioData_ = value; }
        }

        private OggFile audio_;
        public OggFile Audio
        {
            get { return audio_; }
            set { audio_ = value; }
        }        
    }
}
