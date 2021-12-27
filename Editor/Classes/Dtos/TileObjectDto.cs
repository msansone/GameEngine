using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class TileObjectDto : BaseDto
    {
        public Guid BitmapResourceId
        {
            get
            {
                return bitmapResourceId_;
            }
            set
            {
                bitmapResourceId_ = value;
            }
        }
        private Guid bitmapResourceId_ = Guid.Empty;

        
        public int Columns
        {
            get
            {
                return columns_;
            }
            set
            {
                columns_ = value;
            }
        }
        private int columns_;


        public int Rows
        {
            get
            {
                return rows_;
            }
            set
            {
                rows_ = value;
            }
        }
        private int rows_;
   

        public int TopLeftCornerColumn
        {
            get
            {
                return topLeftCornerColumn_;
            }
            set
            {
                topLeftCornerColumn_ = value;
            }
        }
        private int topLeftCornerColumn_;


        public int TopLeftCornerRow
        {
            get
            {
                return topLeftCornerRow_;
            }
            set
            {
                topLeftCornerRow_ = value;
            }
        }
        private int topLeftCornerRow_;

        
        public Guid AnimationId
        {
            get
            {
                return animationId_;
            }
            set
            {
                animationId_ = value;
            }
        }
        private Guid animationId_ = Guid.Empty;
    }


    public class TileObjectDtoProxy : ITileObjectDtoProxy
    {
        private IProjectController projectController_;
        private Guid tileObjectId_;

        public TileObjectDtoProxy(IProjectController projectController, Guid tileObjectId)
        {
            projectController_ = projectController;
            tileObjectId_ = tileObjectId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);

                return tileObject.Name;
            }
            set
            {
                bool isValid = true;

                // Validate the tile object name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Tile object name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);
                    
                    for (int i = 0; i < project.TileObjects[tileObject.OwnerId].Count; i++)
                    {
                        if (value.ToUpper() == project.TileObjects[tileObject.OwnerId][i].Name.ToUpper())
                        {
                            isValid = false;
                            break;
                        }
                    }
                    
                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Object name is already in use within this tile sheet.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetTileObjectName(tileObjectId_, value);
                }
            }
        }
        
        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return tileObjectId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);

                return tileObject.OwnerId;
            }
        }

        [TypeConverter(typeof(SceneryAnimationConverter))]
        public string Animation
        {
            get
            {
                TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);

                Guid animationId = tileObject.AnimationId;

                if (animationId == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    SceneryAnimationDto sceneryAniamtion = projectController_.GetSceneryAnimation(animationId);

                    if (sceneryAniamtion == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return sceneryAniamtion.Name;
                    }
                }
            }
            set
            {
                TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);
                
                Guid animationId = projectController_.GetSceneryAnimationIdFromName(tileObject.OwnerId, value);

                projectController_.SetTileObjectAnimationId(tileObjectId_, animationId);
            }
        }

        public int Columns
        {
            get
            {
                TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);

                return tileObject.Columns;
            }
            set
            {
                projectController_.SetTileObjectColumns(tileObjectId_, value);
            }
        }
        
        public int Rows
        {
            get
            {
                TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);

                return tileObject.Rows;
            }
            set
            {
                projectController_.SetTileObjectRows(tileObjectId_, value);
            }
        }

        
        public int TopLeftCornerColumn
        {
            get
            {
                TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);

                return tileObject.TopLeftCornerColumn;
            }
            set
            {
                projectController_.SetTileObjectTopLeftCornerColumn(tileObjectId_, value);
            }
        }
        
        public int TopLeftCornerRow
        {
            get
            {
                TileObjectDto tileObject = projectController_.GetTileObject(tileObjectId_);

                return tileObject.TopLeftCornerRow;
            }
            set
            {
                projectController_.SetTileObjectTopLeftCornerRow(tileObjectId_, value);
            }
        }
    }

    public interface ITileObjectDtoProxy : IBaseDtoProxy
    {
        int Columns { get; set; }

        int Rows { get; set; }

        int TopLeftCornerColumn { get; set; }
       
        int TopLeftCornerRow { get; set; }
    }
}
