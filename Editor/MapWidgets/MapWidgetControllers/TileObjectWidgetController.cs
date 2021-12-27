using System;
using System.Collections.Generic;
using System.Drawing;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class TileObjectWidgetController : IMapWidgetController
    {
        #region Contructors

        public TileObjectWidgetController(IProjectController projectController, Guid tileObjectId)
        {
            projectController_ = projectController;

            size_ = new Size(16, 16);

            tileObjectId_ = tileObjectId;
        }
        
        #endregion

        #region Private Variables

        private IProjectController projectController_;
        
        private Size size_;

        private TileObjectDto tileObject_ = null;

        private BitmapResourceDto tileObjectBitmapResource_ = null;

        private Guid tileObjectId_ = Guid.Empty;

        private TileSheetDto tileSheet_ = null;
        
        #endregion

        #region Properties

        public bool GridAligned
        {
            get { return true; }
        }

        public MapWidgetDto MapWidget
        {
            set
            {
                mapWidget_ = value;
            }
        }
        MapWidgetDto mapWidget_;

        #endregion

        #region Public Functions

        public void DeserializeFromString(string data)
        {
            string[] splitData;
            splitData = data.Split(',');

            mapWidget_.BoundingBox.Left = Convert.ToInt32(splitData[0]);
            mapWidget_.BoundingBox.Top = Convert.ToInt32(splitData[1]);
            mapWidget_.BoundingBox.Width = Convert.ToInt32(splitData[2]);
            mapWidget_.BoundingBox.Height = Convert.ToInt32(splitData[3]);
        }

        public Rectangle GetBoundingRect()
        {
            // The project and tile object may not have been initialized yet if this was called while loading the project. 
            // Don't need to do anything because the data will be loaded from the file.
            ProjectDto project = projectController_.GetProjectDto();

            if (tileObject_ != null && project != null)
            {
                int sourceWidth = (int)((tileObject_.Columns * project.TileSize) / tileSheet_.ScaleFactor);

                int sourceHeight = (int)((tileObject_.Rows * project.TileSize) / tileSheet_.ScaleFactor);

                int destinationWidth = (int)(sourceWidth * tileSheet_.ScaleFactor);

                int destinationHeight = (int)(sourceHeight * tileSheet_.ScaleFactor);

                return new Rectangle(mapWidget_.BoundingBox.Left, mapWidget_.BoundingBox.Top, destinationWidth, destinationHeight);
            }

            return new Rectangle(0, 0, 0, 0);
        }

        public void Initialize()
        {
            tileObject_ = projectController_.GetTileObject(tileObjectId_);

            if (tileObject_ != null)
            {
                tileSheet_ = projectController_.GetTileSheet(tileObject_.OwnerId);

                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();
                ProjectDto project = projectController_.GetProjectDto();

                //// if (resources.Bitmaps.ContainsKey(tileObject_.BitmapResourceId))
                if (project.Bitmaps.ContainsKey(tileObject_.BitmapResourceId))
                {
                    //tileObjectBitmap_ = resources.Bitmaps[tileObject_.BitmapResourceId];
                    tileObjectBitmapResource_ = project.Bitmaps[tileObject_.BitmapResourceId];
                }
                else
                {
                    System.Diagnostics.Debug.Print("Failed to set tile object bitmap for tile object " + tileObject_.Name);
                }
            }
            else
            {
                System.Diagnostics.Debug.Print("Tile object with ID " + tileObjectId_ + " not found");
            }

            // The widget bounds wouldn't have been set before the initialize when creating via a UI click. Do it now to account for that.
            Rectangle bounds = mapWidget_.Controller.GetBoundingRect();

            mapWidget_.BoundingBox.Left = bounds.Left;
            mapWidget_.BoundingBox.Top = bounds.Top;
            mapWidget_.BoundingBox.Width = bounds.Width;
            mapWidget_.BoundingBox.Height = bounds.Height;

            ((TileObjectWidgetDto)mapWidget_).TileObjectId = tileObjectId_;
        }

        public void MouseDown(System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void MouseUp(System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void MouseMove()
        {
        }

        public void PropertyValueChanged(string name, ref object value, ref bool cancel)
        {
        }

        public void RenderBackground(Graphics g, int x, int y)
        {
        }

        public void Render(Graphics g, int x, int y)
        {
            ProjectDto project = projectController_.GetProjectDto();

            if (project.Bitmaps.ContainsKey(tileObject_.BitmapResourceId))
            {
                if (tileObjectBitmapResource_.BitmapImage == null)
                {
                    // Force the bitmap to load from disk.
                    projectController_.LoadBitmap(tileObjectBitmapResource_.Id);

                    tileObjectBitmapResource_.LoadedModules ^= (byte)EditorModule.Room;
                }
                
                int sourceX = (int)((tileObject_.TopLeftCornerColumn * project.TileSize) / tileSheet_.ScaleFactor);

                int sourceY = (int)((tileObject_.TopLeftCornerRow * project.TileSize) / tileSheet_.ScaleFactor);

                int sourceWidth = (int)((tileObject_.Columns * project.TileSize) / tileSheet_.ScaleFactor);

                int sourceHeight = (int)((tileObject_.Rows * project.TileSize) / tileSheet_.ScaleFactor);

                System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);

                int destinationWidth = (int)(sourceWidth * tileSheet_.ScaleFactor);

                int destinationHeight = (int)(sourceHeight * tileSheet_.ScaleFactor);

                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, destinationWidth, destinationHeight);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                g.DrawImage(tileObjectBitmapResource_.BitmapImageWithTransparency, destRect, sourceX, sourceY, sourceWidth, sourceHeight, GraphicsUnit.Pixel, null);                
            }
        }

        public void RenderOverlay(Graphics g, Point viewOffset, bool isSelected, bool isSingularSelection, bool showOutline)
        {
            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle(mapWidget_.BoundingBox.Left + viewOffset.X, mapWidget_.BoundingBox.Top + viewOffset.Y, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);

            if (isSelected == true)
            {
                g.FillRectangle(new SolidBrush(Globals.actorFillColor), drawingRect);

                if (showOutline == true)
                {
                    g.DrawRectangle(new Pen(new SolidBrush(Globals.actorOutlineColor)), drawingRect);
                }
            }
        }
        
        public void ResetProperties(MapWidgetProperties properties)
        {
            // Remove and re-add all properties
            List<string> propertyNames = new List<string>();

            foreach (PropertyDto property in properties)
            {
                propertyNames.Add(property.Name);
            }

            foreach (string propertyName in propertyNames)
            {
                properties.RemoveProperty(propertyName);
            }

            PropertyDto name = new PropertyDto();
            name.Name = "Name";
            name.Value = mapWidget_.Name;
            name.OwnerId = mapWidget_.Id;
            name.ReadOnly = true;
            name.Reserved = true;

            properties.AddProperty(name);

            return;
        }

        public string SerializeToString()
        {
            return mapWidget_.BoundingBox.Left + "," +
                   mapWidget_.BoundingBox.Top + "," +
                   mapWidget_.BoundingBox.Width + "," +
                   mapWidget_.BoundingBox.Height;
        }
        
        public void UpdatePosition(Point2D position)
        {
            mapWidget_.Position.X = position.X;
            mapWidget_.Position.Y = position.Y;

            mapWidget_.BoundingBox.Left = position.X;
            mapWidget_.BoundingBox.Top = position.Y;
        }

        #endregion        
    }
}
