using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class ProjectResourcesDto
    {
        private BitmapResourceDto collisionTilesBitmap_ = new BitmapResourceDto();
        public BitmapResourceDto CollisionTilesBitmap
        {
            get { return collisionTilesBitmap_; }
        }

        private Dictionary<Guid, BitmapResourceDto> dictBitmaps_ = new Dictionary<Guid, BitmapResourceDto>();
        public Dictionary<Guid, BitmapResourceDto> Bitmaps
        {
            get { return dictBitmaps_; }
        }

        private Dictionary<Guid, AudioResourceDto> dictAudioData_ = new Dictionary<Guid, AudioResourceDto>();
        public Dictionary<Guid, AudioResourceDto> AudioData
        {
            get { return dictAudioData_; }
        }
    }
}
