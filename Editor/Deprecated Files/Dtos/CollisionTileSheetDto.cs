using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class CollisionTileSheetDto : TileSheetDto
    {
        public CollisionTileSheetDto()
        {
            // Populate the list of tiletypes.
            tileTypes_.Add(TileType.SOLIDTILE);
            tileTypes_.Add(TileType.SLOPE_TRANSITION);
            tileTypes_.Add(TileType.ONEWAY_TOP);
            tileTypes_.Add(TileType.NONE);
            tileTypes_.Add(TileType.NONE);
            tileTypes_.Add(TileType.NONE);
            tileTypes_.Add(TileType.SLOPE45_BOTTOMRIGHT_TILE);
            tileTypes_.Add(TileType.SLOPE45_BOTTOMLEFT_TILE);
            tileTypes_.Add(TileType.SLOPE26_HORIZONTAL_BOTTOMLEFT_LARGE_TILE);
            tileTypes_.Add(TileType.SLOPE26_HORIZONTAL_BOTTOMLEFT_SMALL_TILE);
            tileTypes_.Add(TileType.SLOPE26_HORIZONTAL_BOTTOMRIGHT_SMALL_TILE);
            tileTypes_.Add(TileType.SLOPE26_HORIZONTAL_BOTTOMRIGHT_LARGE_TILE);
            tileTypes_.Add(TileType.SLOPE45_TOPRIGHT_TILE);
            tileTypes_.Add(TileType.SLOPE45_TOPLEFT_TILE);
            tileTypes_.Add(TileType.SLOPE26_HORIZONTAL_TOPLEFT_LARGE_TILE);
            tileTypes_.Add(TileType.SLOPE26_HORIZONTAL_TOPLEFT_SMALL_TILE);
            tileTypes_.Add(TileType.SLOPE26_HORIZONTAL_TOPRIGHT_SMALL_TILE);
            tileTypes_.Add(TileType.SLOPE26_HORIZONTAL_TOPRIGHT_LARGE_TILE);
        }
   
        private List<TileType> tileTypes_ = new List<TileType>();
        public List<TileType> TileTypes
        {
            get { return tileTypes_; }
        }
    }
}
