using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    class AssetMenuDto
    {
        private ContextMenuStrip menu_;

        private Object asset_;

        private bool alternateMode_;

        public AssetMenuDto(ContextMenuStrip menu, Object asset)
        {
            menu_ = menu;
            asset_ = asset;
        }

        public ContextMenuStrip Menu
        {
            get
            {
                return menu_;
            }
        }

        public Object Asset
        {
            get
            {
                return asset_;
            }
        }
    }
}
