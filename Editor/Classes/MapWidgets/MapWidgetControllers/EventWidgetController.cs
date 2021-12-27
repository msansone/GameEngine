using System;
using System.Collections.Generic;
using System.Drawing;

namespace FiremelonEditor2
{
    class EventWidgetController : IResizableMapWidgetController
    {
        #region Constructors

        public EventWidgetController(IProjectController projectController, Guid eventId)
        {
            eventId_ = eventId;

            projectController_ = projectController;
        }

        #endregion

        #region Private Variables

        Guid eventId_ = Guid.Empty;

        IProjectController projectController_;

        #endregion

        #region Properties

        public bool GridAligned
        {
            get { return false; }
        }

        public MapWidgetDto MapWidget
        {
            set
            {
                mapWidget_ = value;
                eventMapWidget_ = (EventWidgetDto)mapWidget_;
            }
        }
        MapWidgetDto mapWidget_;
        EventWidgetDto eventMapWidget_;

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
            return new Rectangle(mapWidget_.BoundingBox.Left, mapWidget_.BoundingBox.Top, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);
        }

        public GrabberDirection GetSizeMode(Point cursorLocation)
        {
            var bounds = GetBoundingRect();

            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height);

            List<System.Drawing.Rectangle> lstGrabberRects = generateGrabberRects(drawingRect);

            for (int i = 0; i < lstGrabberRects.Count; i++)
            {
                if (lstGrabberRects[i].Contains(cursorLocation) == true)
                {
                    return (GrabberDirection)i;
                }
            }

            return GrabberDirection.None;
        }

        public void Initialize()
        {
            //tileObject_ = projectController_.GetTileObject(tileObjectId_);

            //tileSheet_ = projectController_.GetTileSheet(tileObject_.OwnerId);

            //ProjectResourcesDto resources = projectController_.GetResources();

            //project_ = projectController_.GetProjectDto();

            //tileObjectBitmap_ = resources.Bitmaps[tileObject_.BitmapResourceId];

            //// The widget bounds wouldn't have been set before the initialize when creating via a UI click. Do it now to account for that.
            //Rectangle bounds = mapWidget_.Controller.GetBoundingRect();

            //mapWidget_.BoundingBox.Left = bounds.Left;
            //mapWidget_.BoundingBox.Top = bounds.Top;
            //mapWidget_.BoundingBox.Width = bounds.Width;
            //mapWidget_.BoundingBox.Height = bounds.Height;

            //((TileObjectWidgetDto)mapWidget_).TileObjectId = tileObjectId_;
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
            //if (tileObjectBitmap_ != null)
            //{
            //    int sourceX = (int)((tileObject_.TopLeftCornerColumn * project_.TileSize) / tileSheet_.ScaleFactor);

            //    int sourceY = (int)((tileObject_.TopLeftCornerRow * project_.TileSize) / tileSheet_.ScaleFactor);

            //    int sourceWidth = (int)((tileObject_.Columns * project_.TileSize) / tileSheet_.ScaleFactor);

            //    int sourceHeight = (int)((tileObject_.Rows * project_.TileSize) / tileSheet_.ScaleFactor);

            //    System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);

            //    int destinationWidth = (int)(sourceWidth * tileSheet_.ScaleFactor);

            //    int destinationHeight = (int)(sourceHeight * tileSheet_.ScaleFactor);

            //    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, destinationWidth, destinationHeight);

            //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            //    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            //    g.DrawImage(tileObjectBitmap_.BitmapImageWithTransparency, destRect, sourceX, sourceY, sourceWidth, sourceHeight, GraphicsUnit.Pixel, null);
            //}

            g.FillRectangle(new SolidBrush(Globals.eventFillColor), x, y, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);
        }

        public void RenderOverlay(Graphics g, Point viewOffset, bool isSelected, bool isSingularSelection, bool showOutline)
        {
            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle(mapWidget_.BoundingBox.Left + viewOffset.X, mapWidget_.BoundingBox.Top + viewOffset.Y, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);
            
            if (isSelected == true)
            {
                g.FillRectangle(new SolidBrush(Globals.actorFillColor), drawingRect);

                if (isSingularSelection == true)
                {
                    // Render the resize grabbers if only one is selected.
                    List<System.Drawing.Rectangle> lstGrabberRects = generateGrabberRects(drawingRect);

                    foreach (System.Drawing.Rectangle r in lstGrabberRects)
                    {
                        int grabberX = r.X;
                        int grabberY = r.Y;
                        int grabberW = r.Width;
                        int grabberH = r.Height;

                        SolidBrush b;
                        Pen p = new Pen(new SolidBrush(Globals.grabberOutlineColor)); ;

                        //if (areMultipleSelected == false && uiState.EditMode == EditMode.Selection)
                        //{
                        b = new SolidBrush(Globals.grabberActiveFillColor);
                        //}
                        //else
                        //{
                        //    b = new SolidBrush(Globals.grabberInactiveFillColor);
                        //}

                        g.FillRectangle(b, grabberX, grabberY, grabberW, grabberH);
                        g.DrawRectangle(p, grabberX, grabberY, grabberW, grabberH);
                    }
                }
            }
        
            if (showOutline == true)
            {
                g.DrawRectangle(new Pen(new SolidBrush(Globals.actorOutlineColor)), drawingRect);
            }
        }

        public void ResetProperties(MapWidgetProperties properties)
        {
            // Remove and re-add.
            properties.RemoveProperty("Name");
            properties.RemoveProperty("PositionX");
            properties.RemoveProperty("PositionY");
            properties.RemoveProperty("BoxHeight");
            properties.RemoveProperty("BoxWidth");
            properties.RemoveProperty("AcceptInput");
            properties.RemoveProperty("RenderOrder");
            properties.RemoveProperty("AttachToCamera");

            properties.RemoveProperty("Name");
            PropertyDto name = new PropertyDto();
            name.Name = "Name";
            name.Value = mapWidget_.Name;
            name.OwnerId = mapWidget_.Id;
            name.Reserved = true;

            properties.AddProperty(name);

            properties.RemoveProperty("PositionX");
            PropertyDto positionX = new PropertyDto();
            positionX.Name = "PositionX";
            positionX.Value = mapWidget_.Position.X;
            positionX.OwnerId = mapWidget_.Id;
            positionX.Reserved = true;

            properties.AddProperty(positionX);

            properties.RemoveProperty("PositionY");
            PropertyDto positionY = new PropertyDto();
            positionY.Name = "PositionY";
            positionY.Value = mapWidget_.Position.Y;
            positionY.OwnerId = mapWidget_.Id;
            positionY.Reserved = true;

            properties.AddProperty(positionY);

            properties.RemoveProperty("AcceptInput");
            PropertyDto acceptInput = new PropertyDto();
            acceptInput.Name = "AcceptInput";
            acceptInput.Value = eventMapWidget_.AcceptInput;
            acceptInput.OwnerId = mapWidget_.Id;
            acceptInput.Reserved = true;

            properties.AddProperty(acceptInput);
            
            properties.RemoveProperty("BoxWidth");
            PropertyDto boxWidth = new PropertyDto();
            boxWidth.Name = "BoxWidth";
            boxWidth.Value = mapWidget_.BoundingBox.Width;
            boxWidth.OwnerId = mapWidget_.Id;
            boxWidth.Reserved = true;

            properties.AddProperty(boxWidth);

            properties.RemoveProperty("BoxHeight");
            PropertyDto boxHeight = new PropertyDto();
            boxHeight.Name = "BoxHeight";
            boxHeight.Value = mapWidget_.BoundingBox.Height;
            boxHeight.OwnerId = mapWidget_.Id;
            boxHeight.Reserved = true;

            properties.AddProperty(boxHeight);
            
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

        #region Protected Functions
        #endregion

        #region Private Functions

        private List<System.Drawing.Rectangle> generateGrabberRects(System.Drawing.Rectangle boundingRect)
        {
            // Gather the entity bounds data.
            int x = boundingRect.Left;
            int y = boundingRect.Top;
            int w = boundingRect.Width;
            int h = boundingRect.Height;

            int size = Globals.grabberSize;

            int midX = w / 2;
            int midY = h / 2;
            int grabberMid = size / 2;

            List<System.Drawing.Rectangle> lstGrabberRects = new List<System.Drawing.Rectangle>();

            // Northwest
            lstGrabberRects.Add(new System.Drawing.Rectangle(x, y, size, size));

            // North
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + midX - grabberMid, y, size, size));

            // Northeast
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + w - size, y, size, size));

            // East
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + w - size, y + midY - grabberMid, size, size));

            // Southeast
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + w - size, y + h - size, size, size));

            // South
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + midX - grabberMid, y + h - size, size, size));

            // Southwest
            lstGrabberRects.Add(new System.Drawing.Rectangle(x, y + h - size, size, size));

            // West
            lstGrabberRects.Add(new System.Drawing.Rectangle(x, y + midY - grabberMid, size, size));

            return lstGrabberRects;
        }

        #endregion

        #region Event Handlers
        #endregion
    }
}
