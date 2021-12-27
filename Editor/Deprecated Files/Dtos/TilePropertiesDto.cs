using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class TilePropertiesDto
    {
        private TileType tileType_ = TileType.SOLIDTILE;
        public TileType TileType
        {
            get
            {
                return tileType_;
            }
            set
            {
                tileType_ = value;
            }
        }
    }
}
