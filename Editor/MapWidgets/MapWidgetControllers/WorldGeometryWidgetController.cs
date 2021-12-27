using System;
using System.Collections.Generic;
using System.Drawing;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class WorldGeometryWidgetController : IResizableMapWidgetController
    {
        IProjectController projectController_;
        MapWidgetDto mapWidget_;
        WorldGeometryWidgetDto worldGeometryMapWidget_;
        Size size_;
        int tileSize_;
        
        public WorldGeometryWidgetController(IProjectController projectController, int tileSize)
        {
            projectController_ = projectController;
            size_ = new Size(16, 16);

            tileSize_ = tileSize;
        }
        
        public bool GridAligned
        {
            get { return true;  }
        }

        public MapWidgetDto MapWidget
        {
            set
            {
                mapWidget_ = value;
                worldGeometryMapWidget_ = (WorldGeometryWidgetDto)mapWidget_;
            }
        }

        public void Initialize()
        {

        }

        public void RenderBackground(Graphics g, int x, int y)
        {
        }

        public void Render(Graphics g, int x, int y)
        {
            if (projectController_.GetUiState().ShowWorldGeometry == true)
            {
                Point bottomLeft = new Point(x, y + mapWidget_.BoundingBox.Height);

                Point topRight = new Point(x + mapWidget_.BoundingBox.Width, y);

                Point bottomRight = new Point(x + mapWidget_.BoundingBox.Width, y + mapWidget_.BoundingBox.Height);

                Point topLeft = new Point(x, y);

                int slopeRise = ((WorldGeometryWidgetDto)mapWidget_).SlopeRise;

                Point inclineTopRight = new Point(x + mapWidget_.BoundingBox.Width, y + mapWidget_.BoundingBox.Height - (slopeRise * tileSize_));

                Point inclineTopLeft = new Point(x, y + mapWidget_.BoundingBox.Height + (slopeRise * tileSize_));

                Point inclineBottomRight = new Point(x + mapWidget_.BoundingBox.Width, y - (slopeRise * tileSize_));

                Point inclineBottomLeft = new Point(x, y + (slopeRise * tileSize_));
                
                switch (worldGeometryMapWidget_.CollisionStyle)
                {
                    case WorldGeometryCollisionStyle.Solid:

                        System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle(x, y, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);

                        g.DrawRectangle(Globals.pWorldGeometryWidgetOutline, drawingRect);

                        g.FillRectangle(Globals.bWorldGeometryWidgetFill, drawingRect);

                        break;

                    case WorldGeometryCollisionStyle.Incline:

                        Point[] inclineTrianglePolyPoints = { bottomLeft, inclineTopRight, bottomRight };

                        g.DrawPolygon(Globals.pWorldGeometryWidgetOutline, inclineTrianglePolyPoints);

                        g.FillPolygon(Globals.bWorldGeometryWidgetFill, inclineTrianglePolyPoints);
                        
                        break;

                    case WorldGeometryCollisionStyle.InvertedIncline:
                        
                        Point[] invertedInclineTrianglePolyPoints = { topLeft, topRight, inclineBottomRight };

                        g.DrawPolygon(Globals.pWorldGeometryWidgetOutline, invertedInclineTrianglePolyPoints);

                        g.FillPolygon(Globals.bWorldGeometryWidgetFill, invertedInclineTrianglePolyPoints);

                        break;

                    case WorldGeometryCollisionStyle.Decline:

                        Point[] declineTrianglePolyPoints = { inclineTopLeft, bottomLeft, bottomRight };

                        g.DrawPolygon(Globals.pWorldGeometryWidgetOutline, declineTrianglePolyPoints);

                        g.FillPolygon(Globals.bWorldGeometryWidgetFill, declineTrianglePolyPoints);

                        break;

                    case WorldGeometryCollisionStyle.InvertedDecline:

                        Point[] invertedDeclineTrianglePolyPoints = { topLeft, topRight, inclineBottomLeft };

                        g.DrawPolygon(Globals.pWorldGeometryWidgetOutline, invertedDeclineTrianglePolyPoints);

                        g.FillPolygon(Globals.bWorldGeometryWidgetFill, invertedDeclineTrianglePolyPoints);

                        break;

                    case WorldGeometryCollisionStyle.SnapToBottom:

                        System.Drawing.Rectangle snapToBottomRect = new System.Drawing.Rectangle(x, y, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);

                        g.DrawRectangle(Globals.pWorldGeometryWidgetOutline, snapToBottomRect);

                        g.FillRectangle(Globals.bWorldGeometryWidgetFill, snapToBottomRect);
                        
                        int oneThirdWidth = mapWidget_.BoundingBox.Width / 3;

                        int twoThirdsWidth = (mapWidget_.BoundingBox.Width / 3) * 2;

                        int oneHalfWidth = mapWidget_.BoundingBox.Width / 2;

                        int oneThirdHeight = mapWidget_.BoundingBox.Height / 3;

                        //int twoThirdsHeight = (mapWidget_.BoundingBox.Height / 3) * 2;

                        int oneHalfHeight = mapWidget_.BoundingBox.Height / 2;

                        Point arrow1 = new Point(x + oneThirdWidth, y);
                        Point arrow2 = new Point(x + twoThirdsWidth, y);
                        Point arrow3 = new Point(arrow2.X, y + oneHalfHeight);
                        Point arrow4 = new Point(x + mapWidget_.BoundingBox.Width - 4, arrow3.Y);
                        Point arrow5 = new Point(x + oneHalfWidth, y + mapWidget_.BoundingBox.Height - 4);
                        Point arrow6 = new Point(x + 4, arrow4.Y);
                        Point arrow7 = new Point(arrow1.X, arrow6.Y);

                        Point[] snapDownArrowPolyPoints = { arrow1, arrow2, arrow3, arrow4, arrow5, arrow6, arrow7 };

                        g.DrawPolygon(Globals.pSnapArrow, snapDownArrowPolyPoints);

                        g.FillPolygon(Globals.bSnapArrow, snapDownArrowPolyPoints);

                        break;
                }
            }            
        }

        public void RenderOverlay(Graphics g, Point viewOffset, bool isSelected, bool isSingularSelection, bool showOutline)
        {
            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle(mapWidget_.BoundingBox.Left + viewOffset.X, mapWidget_.BoundingBox.Top + viewOffset.Y, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);
            
            //if (showOutline == true)
            //{
            //    g.DrawRectangle(new Pen(new SolidBrush(Globals.actorOutlineColor)), drawingRect);
            //}

            // Fill in selected with a transparent color.
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
        }

        public void UpdatePosition(Point2D position)
        {
            mapWidget_.Position.X = position.X;
            mapWidget_.Position.Y = position.Y;

            ProjectDto project = projectController_.GetProjectDto();

            int width = mapWidget_.BoundingBox.Width;
            int height = mapWidget_.BoundingBox.Height;

            ((WorldGeometryWidgetDto)mapWidget_).Corner1.X = position.X / project.TileSize;
            ((WorldGeometryWidgetDto)mapWidget_).Corner1.Y = position.Y / project.TileSize;
            ((WorldGeometryWidgetDto)mapWidget_).Corner2.X = ((WorldGeometryWidgetDto)mapWidget_).Corner1.X + (width / project.TileSize) - 1;
            ((WorldGeometryWidgetDto)mapWidget_).Corner2.Y = ((WorldGeometryWidgetDto)mapWidget_).Corner1.Y + (height / project.TileSize) - 1;
        }

        public Rectangle GetBoundingRect()
        {
            // Take the two corner points and determine the dimensions and position of the drawable rect.            
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            if (worldGeometryMapWidget_.Corner1.X > worldGeometryMapWidget_.Corner2.X)
            {
                left = worldGeometryMapWidget_.Corner2.X * tileSize_;
                right = (worldGeometryMapWidget_.Corner1.X * tileSize_) + tileSize_;
            }
            else
            {
                left = worldGeometryMapWidget_.Corner1.X * tileSize_;
                right = (worldGeometryMapWidget_.Corner2.X * tileSize_) + tileSize_;
            }

            if (worldGeometryMapWidget_.Corner1.Y > worldGeometryMapWidget_.Corner2.Y)
            {
                top = worldGeometryMapWidget_.Corner2.Y * tileSize_;
                bottom = (worldGeometryMapWidget_.Corner1.Y * tileSize_) + tileSize_;
            }
            else
            {
                top = worldGeometryMapWidget_.Corner1.Y * tileSize_;
                bottom = (worldGeometryMapWidget_.Corner2.Y * tileSize_) + tileSize_;
            }

            int width = right - left;
            int height = bottom - top;

            Rectangle rect = new Rectangle(left, top, width, height);

            return rect;
        }

        public string SerializeToString()
        {
            return mapWidget_.BoundingBox.Left + "," +
                   mapWidget_.BoundingBox.Top + "," +
                   mapWidget_.BoundingBox.Width + "," +
                   mapWidget_.BoundingBox.Height;
        }

        public void DeserializeFromString(string data)
        {
            string[] splitData;
            splitData = data.Split(',');

            mapWidget_.BoundingBox.Left = Convert.ToInt32(splitData[0]);
            mapWidget_.BoundingBox.Top = Convert.ToInt32(splitData[1]);
            mapWidget_.BoundingBox.Width = Convert.ToInt32(splitData[2]);
            mapWidget_.BoundingBox.Height = Convert.ToInt32(splitData[3]);
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

            PropertyDto cell1X = new PropertyDto();
            cell1X.Name = "Corner One X";
            cell1X.Value = ((WorldGeometryWidgetDto)mapWidget_).Corner1.X;
            cell1X.OwnerId = mapWidget_.Id;
            cell1X.Reserved = true;

            properties.AddProperty(cell1X);

            PropertyDto cell1Y = new PropertyDto();
            cell1Y.Name = "Corner One Y";
            cell1Y.Value = ((WorldGeometryWidgetDto)mapWidget_).Corner1.Y;
            cell1Y.OwnerId = mapWidget_.Id;
            cell1Y.Reserved = true;

            properties.AddProperty(cell1Y);

            PropertyDto cell2X = new PropertyDto();
            cell2X.Name = "Corner Two X";
            cell2X.Value = ((WorldGeometryWidgetDto)mapWidget_).Corner2.X;
            cell2X.OwnerId = mapWidget_.Id;
            cell2X.Reserved = true;

            properties.AddProperty(cell2X);

            PropertyDto cell2Y = new PropertyDto();
            cell2Y.Name = "Corner Two Y";
            cell2Y.Value = ((WorldGeometryWidgetDto)mapWidget_).Corner2.Y;
            cell2Y.OwnerId = mapWidget_.Id;
            cell2Y.Reserved = true;

            properties.AddProperty(cell2Y);
            
            PropertyDto collisionStyle = new PropertyDto();
            collisionStyle.Name = "Collision Style";
            collisionStyle.Value = ((WorldGeometryWidgetDto)mapWidget_).CollisionStyle;
            collisionStyle.OwnerId = mapWidget_.Id;
            collisionStyle.Reserved = true;

            properties.AddProperty(collisionStyle);

            PropertyDto edges = new PropertyDto();
            edges.Name = "Edges";
            edges.Value = ((WorldGeometryWidgetDto)mapWidget_).Edges;
            edges.OwnerId = mapWidget_.Id;
            edges.ReadOnly = true;
            edges.Reserved = true;

            properties.AddProperty(edges);

            PropertyDto slopeRise = new PropertyDto();
            slopeRise.Name = "Slope Rise";
            slopeRise.Value = ((WorldGeometryWidgetDto)mapWidget_).SlopeRise;
            slopeRise.OwnerId = mapWidget_.Id;
            slopeRise.Reserved = true;

            properties.AddProperty(slopeRise);
            
            return;
        }

        public void PropertyValueChanged(string name, ref object value, ref bool cancel)
        {
            int corner1X;
            int corner1Y;

            int corner2X;
            int corner2Y;

            switch (name.ToUpper())
            {
                case "CORNER ONE X":

                    corner1X = (int)value;
                    corner1Y = ((WorldGeometryWidgetDto)mapWidget_).Corner1.Y;

                    corner2X = ((WorldGeometryWidgetDto)mapWidget_).Corner2.X;
                    corner2Y = ((WorldGeometryWidgetDto)mapWidget_).Corner2.Y;

                    projectController_.SetWorldGeometryWidgetCorners(mapWidget_.Id, new Point2D(corner1X, corner1Y), new Point2D(corner2X, corner2Y));

                    break;

                case "CORNER ONE Y":

                    corner1X = ((WorldGeometryWidgetDto)mapWidget_).Corner1.X;
                    corner1Y = (int)value;

                    corner2X = ((WorldGeometryWidgetDto)mapWidget_).Corner2.X;
                    corner2Y = ((WorldGeometryWidgetDto)mapWidget_).Corner2.Y;


                    projectController_.SetWorldGeometryWidgetCorners(mapWidget_.Id, new Point2D(corner1X, corner1Y), new Point2D(corner2X, corner2Y));

                    break;

                case "CORNER TWO X":

                    corner1X = ((WorldGeometryWidgetDto)mapWidget_).Corner1.X;
                    corner1Y = ((WorldGeometryWidgetDto)mapWidget_).Corner1.Y;

                    corner2X = (int)value;
                    corner2Y = ((WorldGeometryWidgetDto)mapWidget_).Corner2.Y;

                    projectController_.SetWorldGeometryWidgetCorners(mapWidget_.Id, new Point2D(corner1X, corner1Y), new Point2D(corner2X, corner2Y));

                    break;

                case "CORNER TWO Y":

                    corner1X = ((WorldGeometryWidgetDto)mapWidget_).Corner1.X;
                    corner1Y = ((WorldGeometryWidgetDto)mapWidget_).Corner1.Y;

                    corner2X = ((WorldGeometryWidgetDto)mapWidget_).Corner2.X;
                    corner2Y = (int)value; 
                    
                    projectController_.SetWorldGeometryWidgetCorners(mapWidget_.Id, new Point2D(corner1X, corner1Y), new Point2D(corner2X, corner2Y));

                    break;

                case "COLLISION STYLE":
                    
                    projectController_.SetWorldGeometryWidgetCollisionStyle(mapWidget_.Id, (WorldGeometryCollisionStyle)value);

                    break;


                case "SLOPE RISE":

                    projectController_.SetWorldGeometryWidgetSlopeRise(mapWidget_.Id, (int)value);

                    break;                    
            }
        }

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
    }
}
