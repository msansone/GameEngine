using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class SpriteSheetDto : SheetDto
    {
        public SpriteSheetDto()
        {
        }

        private int cellWidth_;
        public int CellWidth
        {
            get { return cellWidth_; }
            set { cellWidth_ = value; }
        }

        private int cellHeight_;
        public int CellHeight
        {
            get { return cellHeight_; }
            set { cellHeight_ = value; }
        }

        private int rows_;
        public int Rows
        {
            get { return rows_; }
            set { rows_ = value; }
        }

        private int cols_;
        public int Columns
        {
            get { return cols_; }
            set { cols_ = value; }
        }
        
        private int padding_ = 0;
        public int Padding
        {
            get { return padding_; }
            set { padding_ = value; }
        }

        private Guid bitmapResourceId_ = Guid.Empty;
        public Guid BitmapResourceId
        {
            get { return bitmapResourceId_; }
            set { bitmapResourceId_ = value; }
        }
    }

    public class SpriteSheetDtoProxy : ISpriteSheetDtoProxy
    {
        private IProjectController projectController_;
        private Guid spriteSheetId_;

        public SpriteSheetDtoProxy(IProjectController projectController, Guid spriteSheetId)
        {
            projectController_ = projectController;
            spriteSheetId_ = spriteSheetId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.Name;
            }
            set
            {
                bool isValid = true;

                // Validate the tile sheet name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Tile sheet name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    // Sprite sheets and tile sheets cannot have the same name.
                    for (int i = 0; i < project.TileSheets.Count; i++)
                    {
                        if (value.ToUpper() == project.TileSheets[i].Name.ToUpper())
                        {
                            isValid = false;
                            break;
                        }
                    }

                    for (int i = 0; i < project.SpriteSheets.Count; i++)
                    {
                        if (value.ToUpper() == project.SpriteSheets[i].Name.ToUpper() && project.SpriteSheets[i].Id != spriteSheetId_)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Sheet name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetSpriteSheetName(spriteSheetId_, value);
                }
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The number of cell rows")]
        public int Rows
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.Rows;
            }
            set
            {
                projectController_.SetSpriteSheetRows(spriteSheetId_, value);
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The number of cell columns")]
        public int Columns
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.Columns;
            }
            set
            {
                projectController_.SetSpriteSheetColumns(spriteSheetId_, value);
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The height of the cells")]
        public int CellHeight
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.CellHeight;
            }
            set
            {
                try
                {
                    projectController_.SetSpriteSheetCellHeight(spriteSheetId_, value);
                }
                catch (InvalidSheetCellSizeException e)
                {
                    MessageBox.Show(e.Message.ToString(), "Invalid Height");
                }
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The width of the cells")]
        public int CellWidth
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.CellWidth;
            }
            set
            {
                try
                {
                    projectController_.SetSpriteSheetCellWidth(spriteSheetId_, value);
                }
                catch (InvalidSheetCellSizeException e)
                {
                    MessageBox.Show(e.Message.ToString(), "Invalid Width");
                }
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The amount of padding between the cells")]
        public int Padding
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.Padding;
            }
            set
            {
                try
                {
                    projectController_.SetSpriteSheetPadding(spriteSheetId_, value);
                }
                catch (InvalidSheetCellSizeException e)
                {
                    MessageBox.Show(e.Message.ToString(), "Invalid Width");
                }
            }
        }

        [CategoryAttribute("Display Settings"),
         DescriptionAttribute("The factor by which the sheet images will be scaled")]
        public float ScaleFactor
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.ScaleFactor;
            }
            set
            {
                projectController_.SetSpriteSheetScaleFactor(spriteSheetId_, value);
            }
        }

        [CategoryAttribute("Data Source Settings"),
         DescriptionAttribute("The location of the source image"),
        Editor(typeof(ImageFilePathUiTypeEditor), typeof(UITypeEditor))]
        public string ImagePath
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();
                ProjectDto project = projectController_.GetProjectDto();

                Guid bitmapResourceId = spriteSheet.BitmapResourceId;

                // Separate resources dto removed in 2.2 format.
                //BitmapResourceDto bitmap = resources.Bitmaps[bitmapResourceId];
                BitmapResourceDto bitmap = project.Bitmaps[bitmapResourceId];

                return bitmap.Path;
            }
            set
            {
                projectController_.SetSpriteSheetImagePath(spriteSheetId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Bitmap Image
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                Guid bitmapResourceId = spriteSheet.BitmapResourceId;
                
                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();
                //BitmapResourceDto bitmapResource = resources.Bitmaps[bitmapResourceId];
                BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(bitmapResourceId, false);

                return bitmapResource.BitmapImage;                
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return spriteSheetId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.OwnerId;
            }
        }
        
        [BrowsableAttribute(false)]
        public Guid BitmapResourceId
        {
            get
            {
                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId_);

                return spriteSheet.BitmapResourceId;
            }
        }
    }

    public interface ISpriteSheetDtoProxy : ISheetDtoProxy
    {
        new int Rows { get; set; }
        new int Columns { get; set; }
        new int CellHeight { get; set; }
        new int CellWidth { get; set; }
    }
}
