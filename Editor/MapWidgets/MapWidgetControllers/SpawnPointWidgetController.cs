using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public class SpawnPointWidgetController : IMapWidgetController 
    {
        IProjectController projectController_;
        MapWidgetDto mapWidget_;
        Size size_;

        public SpawnPointWidgetController(IProjectController projectController)
        {
            projectController_ = projectController; 
            size_ = new Size(16, 16);
        }
        
        public bool GridAligned
        {
            get { return false; }
        }

        public MapWidgetDto MapWidget
        {
            set
            {
                mapWidget_ = value;
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
            int halfWidth = (size_.Width / 2);
            int halfHeight = (size_.Height / 2);

            g.DrawEllipse(Globals.pSpawnPoint, x, y, size_.Width, size_.Height);
            g.DrawLine(Globals.pSpawnPoint, x, y + halfHeight, x + size_.Width, y + halfHeight);
            g.DrawLine(Globals.pSpawnPoint, x + halfWidth, y, x + halfWidth, y + size_.Height);
        }

        public void RenderOverlay(Graphics g, Point viewOffset, bool isSelected, bool isSingularSelection, bool showOutline)
        {
            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle(mapWidget_.BoundingBox.Left + viewOffset.X, mapWidget_.BoundingBox.Top + viewOffset.Y, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);
            
            if (showOutline == true)
            {
                g.DrawRectangle(new Pen(new SolidBrush(Globals.actorOutlineColor)), drawingRect.Left, drawingRect.Top, drawingRect.Width, drawingRect.Height);
            }

            // Fill in selected with a transparent color.
            if (isSelected == true)
            {
                g.FillRectangle(new SolidBrush(Globals.actorFillColor), drawingRect.Left, drawingRect.Top, drawingRect.Width, drawingRect.Height);
            }
        }

        public Rectangle GetBoundingRect()
        {
            int halfWidth = (size_.Width / 2);
            int halfHeight = (size_.Height / 2);

            return new Rectangle(-halfWidth, -halfHeight, size_.Width, size_.Height);
        }

        public void UpdatePosition(Point2D position)
        {
            // Get the delta vector between the position and the bounding box corner.
            // Use this to ensure the two remain in synch during the position update.
            Point2D delta = new Point2D(mapWidget_.Position.X - mapWidget_.BoundingBox.Left, mapWidget_.Position.Y - mapWidget_.BoundingBox.Top);

            mapWidget_.Position.X = position.X;
            mapWidget_.Position.Y = position.Y;

            mapWidget_.BoundingBox.Left = position.X - delta.X;
            mapWidget_.BoundingBox.Top = position.Y - delta.Y;
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

            PropertyDto positionX = new PropertyDto();
            positionX.Name = "PositionX";
            positionX.Value = mapWidget_.Position.X;
            positionX.OwnerId = mapWidget_.Id;
            positionX.Reserved = true;

            properties.AddProperty(positionX);

            PropertyDto positionY = new PropertyDto();
            positionY.Name = "PositionY";
            positionY.Value = mapWidget_.Position.Y;
            positionY.OwnerId = mapWidget_.Id;
            positionY.Reserved = true;

            properties.AddProperty(positionY);

            PropertyDto spawnPointId = new PropertyDto();
            spawnPointId.Name = "Identity";

            spawnPointId.Value = ((SpawnPointWidgetDto)mapWidget_).IdentityName;
            spawnPointId.OwnerId = mapWidget_.Id;
            spawnPointId.Reserved = true;

            spawnPointId.TypeConverter = typeof(SpawnPointConverter);


            properties.AddProperty(spawnPointId);

            return;
        }

        public void PropertyValueChanged(string name, ref object value, ref bool cancel)
        {
            switch (name.ToUpper())
            {
                case "POSITIONX":

                    int x = (int)value;
                    int y = mapWidget_.Position.Y;

                    projectController_.SetMapWidgetPosition(mapWidget_.Id, mapWidget_.Type, new Point2D(x, y));

                    break;

                case "POSITIONY":

                    x = mapWidget_.Position.X;
                    y = (int)value;

                    projectController_.SetMapWidgetPosition(mapWidget_.Id, mapWidget_.Type, new Point2D(x, y));

                    break;

                case "IDENTITY":

                    Guid spawnPointId = projectController_.GetSpawnPointIdFromName((string)value);

                    projectController_.SetSpawnPointWidgetIdentity(mapWidget_.Id, spawnPointId);

                    break;
            }
        }
    }
}
