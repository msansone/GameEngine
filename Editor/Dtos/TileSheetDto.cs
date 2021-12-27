using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class TileSheetDto : SheetDto
    {
        private int tileSize_;
        public int TileSize
        {
            get { return tileSize_; }
            set { tileSize_ = value; }
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

        private Guid bitmapResourceId_ = Guid.Empty;
        public Guid BitmapResourceId
        {
            get { return bitmapResourceId_; }
            set { bitmapResourceId_ = value; }
        }
    }

    public class TileSheetDtoProxy : ITileSheetDtoProxy
    {
        private IProjectController projectController_;
        private Guid tileSheetId_;

        public TileSheetDtoProxy(IProjectController projectController, Guid tileSheetId)
        {
            projectController_ = projectController;
            tileSheetId_ = tileSheetId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                return tileSheet.Name;
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
                    for (int i = 0; i < project.SpriteSheets.Count; i++)
                    {
                        if (value.ToUpper() == project.SpriteSheets[i].Name.ToUpper())
                        {
                            isValid = false;
                            break;
                        }
                    }

                    for (int i = 0; i < project.TileSheets.Count; i++)
                    {
                        if (value.ToUpper() == project.TileSheets[i].Name.ToUpper() && project.TileSheets[i].Id != tileSheetId_)
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
                    projectController_.SetTileSheetName(tileSheetId_, value);
                }
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The number of cell rows")]
        public int Rows
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                return tileSheet.Rows;
            }
            set
            {
                projectController_.SetTileSheetRows(tileSheetId_, value);
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The number of cell columns")]
        public int Columns
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                return tileSheet.Columns;
            }
            set
            {
                projectController_.SetTileSheetColumns(tileSheetId_, value);
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The height of the cells")]
        public int CellHeight
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                return tileSheet.TileSize;
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The width of the cells")]
        public int CellWidth
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                return tileSheet.TileSize;
            }
        }

        [CategoryAttribute("Parse Settings"),
         DescriptionAttribute("The padding between the cells")
         BrowsableAttribute(false)]
        public int Padding
        {
            get
            {
                return 0;
            }
        }

        [CategoryAttribute("Display Settings"),
         DescriptionAttribute("The factor by which the sheet images will be scaled")]
        public float ScaleFactor
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                return tileSheet.ScaleFactor;
            }
            set
            {
                projectController_.SetTileSheetScaleFactor(tileSheetId_, value);
            }
        }

        [CategoryAttribute("Data Source Settings"),
         DescriptionAttribute("The location of the source image"),
        Editor(typeof(ImageFilePathUiTypeEditor), typeof(UITypeEditor))]
        public string ImagePath
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();
                ProjectDto project = projectController_.GetProjectDto();

                Guid bitmapResourceId = tileSheet.BitmapResourceId;

                // Separate resources dto removed in 2.2 format.
                //BitmapResourceDto bitmap = resources.Bitmaps[bitmapResourceId];
                BitmapResourceDto bitmap = project.Bitmaps[bitmapResourceId];

                return bitmap.Path;
            }
            set
            {
                projectController_.SetTileSheetImagePath(tileSheetId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Bitmap Image
        {
            get
            {
                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();
                //ProjectDto project = projectController_.GetProjectDto();

                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                Guid resourceId = tileSheet.BitmapResourceId;

                // Separate resources dto removed in 2.2 format.
                //BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];
                BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(resourceId, false);

                return bitmapResource.BitmapImage;
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return tileSheetId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                return tileSheet.OwnerId;
            }
        }

        [BrowsableAttribute(false)]
        public Guid BitmapResourceId
        {
            get
            {
                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId_);

                return tileSheet.BitmapResourceId;
            }
        }
    }

    public interface ITileSheetDtoProxy : ISheetDtoProxy
    {
    }
}
