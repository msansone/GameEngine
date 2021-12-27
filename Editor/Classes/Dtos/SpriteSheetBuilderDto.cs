using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    #region Enums

    public enum SheetCellVerticalAlignment
    {
        Top = 1,
        Bottom = 2,
        Center = 3
    };

    public enum SheetCellHorizontalAlignment
    {
        Left = 1,
        Right = 2,
        Center = 3
    };

    #endregion

    class SpriteSheetBuilderDto
    {
        public SpriteSheetBuilderDto()
        {

        }

        #region Private Variables
        #endregion

        #region Properties

        [CategoryAttribute("Build Settings"),
         DescriptionAttribute("The number of cell columns")]
        public int Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }
        private int _columns = 0;

        [CategoryAttribute("Build Settings"),
         DescriptionAttribute("The height of the cells")]
        public int CellHeight
        {
            get { return _cellHeight; }
            set { _cellHeight = value; }
        }
        private int _cellHeight = 0;

        [CategoryAttribute("Build Settings"),
         DescriptionAttribute("The width of the cells")]
        public int CellWidth
        {
            get { return _cellWidth; }
            set { _cellWidth = value; }
        }
        private int _cellWidth = 0;


        [BrowsableAttribute(false)]
        public List<SheetImageSourceDto> ImageSourceList
        {
            get { return _imageSourceList; }
            set { _imageSourceList = value; }
        }
        private List<SheetImageSourceDto> _imageSourceList = new List<SheetImageSourceDto>();

        [CategoryAttribute("Build Settings"),
         DescriptionAttribute("The vertical alignment of the cells in the final sheet.")]
        public SheetCellVerticalAlignment VerticalAlignment
        {
            get { return _sheetCellVerticalAlignment; }
            set { _sheetCellVerticalAlignment = value; }
        }
        private SheetCellVerticalAlignment _sheetCellVerticalAlignment = SheetCellVerticalAlignment.Top;


        [CategoryAttribute("Build Settings"),
         DescriptionAttribute("The horizontal alignment of the cells in the final sheet.")]
        public SheetCellHorizontalAlignment HorizontalAlignment
        {
            get { return _sheetCellHorizontalAlignment; }
            set { _sheetCellHorizontalAlignment = value; }
        }
        private SheetCellHorizontalAlignment _sheetCellHorizontalAlignment = SheetCellHorizontalAlignment.Left;


        [CategoryAttribute("Build Settings"),
         DescriptionAttribute("Color mappings to change the palette in the exported image.")
         DisplayNameAttribute("Palette Map")]
        [Editor(typeof(PaletteMapTypeEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(ExpandableObjectConverter))]

        public PaletteMap PaletteMap
        //public Dictionary<Color, Color> PaletteMap
        {
            get { return _paletteMap; }
            set { _paletteMap = value; }
        }
        private PaletteMap _paletteMap = new PaletteMap();
        //private Dictionary<Color, Color> _paletteMap = new Dictionary<Color, Color>();

        #endregion

        #region Public Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion

    }
}
