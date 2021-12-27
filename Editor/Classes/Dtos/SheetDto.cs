using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class SheetDto : BaseDto
    {
        public Bitmap Image { get; }

        private float scaleFactor_ = 1;
        public float ScaleFactor
        {
            get { return scaleFactor_; }
            set { scaleFactor_ = value; }
        }
    }
}
