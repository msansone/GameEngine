using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteSheetBuilder
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

    public enum AlphaChannel
    {
        Red = 1,
        Green = 2,
        Blue = 3,
        Grayscale = 4
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
         DescriptionAttribute("The channel of the alpha value in the final alpha mask sheet.")]
        public AlphaChannel AlphaChannel
        {
            get { return _alphaChannel; }
            set { _alphaChannel = value; }
        }
        private AlphaChannel _alphaChannel = AlphaChannel.Red;

        [CategoryAttribute("Build Settings"),
         DescriptionAttribute("The background color of the final sheet.")]
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }
        private Color _backgroundColor = Color.Transparent;

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


        [CategoryAttribute("Build Settings"),
         DescriptionAttribute("The padding between cells")]
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }
        private int _padding = 0;

        #endregion

        #region Public Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion

    }
}
