using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class TilesetDto
    {
        private Guid tileSheetId_;
        public Guid TileSheetId
        {
            get { return tileSheetId_; }
            set { tileSheetId_ = value; }
        }
    }
}
