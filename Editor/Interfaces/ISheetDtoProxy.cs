using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public interface ISheetDtoProxy : IBaseDtoProxy
    {
        int Rows { get; }
        int Columns { get; }
        int CellHeight { get; }
        int CellWidth { get; }
        int Padding { get; }
        float ScaleFactor { get; set; }
        string ImagePath { get; set; }
        Bitmap Image { get; }
        Guid BitmapResourceId { get; }
    }
}
