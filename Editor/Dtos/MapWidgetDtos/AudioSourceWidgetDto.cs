using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class AudioSourceWidgetDto : MapWidgetDto
    {
        public AudioSourceWidgetDto()
        {
            Type = MapWidgetType.AudioSource;
        }

        private Guid audioId_ = Guid.Empty;
        public Guid Audio
        {
            get { return audioId_; }
            set { audioId_ = value; }
        }

        private string audioName_ = string.Empty;
        public string AudioName
        {
            get { return audioName_; }
            set { audioName_ = value; }
        }

        private bool loop_ = false;
        public bool Loop
        {
            get { return loop_; }
            set { loop_ = value; }
        }

        private bool autoplay_ = false;
        public bool Autoplay
        {
            get { return autoplay_; }
            set { autoplay_ = value; }
        }

        private int minDistance_ = 0;
        public int MinDistance
        {
            get { return minDistance_; }
            set { minDistance_ = value; }
        }

        private int maxDistance_ = 0;
        public int MaxDistance
        {
            get { return maxDistance_; }
            set { maxDistance_ = value; }
        }

        private float volume_ = 0.0f;
        public float Volume
        {
            get { return volume_; }
            set { volume_ = value; }
        }
    }
}
