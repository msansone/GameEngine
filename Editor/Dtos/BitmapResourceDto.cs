using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class BitmapResourceDto : ExternalResourceDto
    {
        public BitmapResourceDto()
        {
            spriteSheetImagelist_ = new SpriteSheetImageList();
        }
        
        public Bitmap BitmapImage
        {
            get { return bmpImage_; }

            // No longer used, but needs to be here for old project format stream readers.
            set { bmpImage_ = value; }
        }
        private Bitmap bmpImage_;

        public Bitmap BitmapImageWithTransparency
        {
            get { return bmpImageWithTransparency_; }

            // No longer used, but needs to be here for old project format stream readers.
            set { bmpImageWithTransparency_ = value; }
        }
        private Bitmap bmpImageWithTransparency_;

        public SpriteSheetImageList SpriteSheetImageList
        {
            get
            {
                return spriteSheetImagelist_;
            }

            // No longer used, but needs to be here for old project format stream readers.
            set { spriteSheetImagelist_ = value; }
        }
        private SpriteSheetImageList spriteSheetImagelist_;

        public byte LoadedModules
        {
            get { return loadedModules_; }
            set { loadedModules_ = value; }
        }
        byte loadedModules_ = 0;
    }
}
