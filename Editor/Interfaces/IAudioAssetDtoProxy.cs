using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonOgg.MediaPlayer;

namespace FiremelonEditor2
{
    public interface IAudioAssetDtoProxy : IBaseDtoProxy
    {
        OggFile Audio { get; }
    }
}
