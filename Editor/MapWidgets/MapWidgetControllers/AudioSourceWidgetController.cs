using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace FiremelonEditor2
{
    public class AudioSourceWidgetController : IMapWidgetController 
    {
        IBitmapUtility bitmapUtility_;
        IFiremelonEditorFactory factory_;
        IProjectController projectController_;
        IUtilityFactory utilityFactory_;
        MapWidgetDto mapWidget_;
        Size size_;
        Bitmap bitmap_;

        public AudioSourceWidgetController(IProjectController projectController)
        {
            factory_ = new FiremelonEditorFactory();

            projectController_ = projectController;

            utilityFactory_ = new UtilityFactory();

            bitmapUtility_ = utilityFactory_.NewBitmapUtility();

            IResourceBitmapReader resourceBitmapReader = factory_.NewResourceBitmapReader();

            bitmap_ = bitmapUtility_.ApplyTransparency(resourceBitmapReader.ReadResourceBitmap("audioicon.png"));

            size_ = new Size(bitmap_.Width, bitmap_.Height);
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

            g.DrawImageUnscaled(bitmap_, new Point(x, y));
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
                
                int centerPointX = drawingRect.Left + (drawingRect.Width / 2);
                int centerPointY = drawingRect.Top + (drawingRect.Height / 2);

                int minDistance = ((AudioSourceWidgetDto)mapWidget_).MinDistance;
                int maxDistance = ((AudioSourceWidgetDto)mapWidget_).MaxDistance;

                g.DrawEllipse(new Pen(new SolidBrush(Globals.audioMinimumDistanceColor)),
                              centerPointX - minDistance,
                              centerPointY - minDistance,
                              minDistance * 2,
                              minDistance * 2);


                g.DrawEllipse(new Pen(new SolidBrush(Globals.audioMaximumDistanceColor)),
                              centerPointX - maxDistance,
                              centerPointY - maxDistance,
                              maxDistance * 2,
                              maxDistance * 2);  
            }
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

        public Rectangle GetBoundingRect()
        {
            int halfWidth = (size_.Width / 2);
            int halfHeight = (size_.Height / 2);

            return new Rectangle(-halfWidth, -halfHeight, size_.Width, size_.Height);
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
            name.ReadOnly = false;
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


            PropertyDto loop = new PropertyDto();
            loop.Name = "Loop";
            loop.Value = ((AudioSourceWidgetDto)mapWidget_).Loop;
            loop.OwnerId = mapWidget_.Id;
            loop.Reserved = true;

            properties.AddProperty(loop);


            PropertyDto autoPlay = new PropertyDto();
            autoPlay.Name = "AutoPlay";
            autoPlay.Value = ((AudioSourceWidgetDto)mapWidget_).Autoplay;
            autoPlay.OwnerId = mapWidget_.Id;
            autoPlay.Reserved = true;

            properties.AddProperty(autoPlay);


            PropertyDto audio= new PropertyDto();
            audio.Name = "Audio";

            audio.Value = ((AudioSourceWidgetDto)mapWidget_).AudioName;
            audio.OwnerId = mapWidget_.Id;
            audio.Reserved = true;

            audio.TypeConverter = typeof(AudioConverter);

            properties.AddProperty(audio);


            PropertyDto minDistance = new PropertyDto();
            minDistance.Name = "MinDistance";

            minDistance.Value = ((AudioSourceWidgetDto)mapWidget_).MinDistance;
            minDistance.OwnerId = mapWidget_.Id;
            minDistance.Reserved = true;

            properties.AddProperty(minDistance);


            PropertyDto maxDistance = new PropertyDto();
            maxDistance.Name = "MaxDistance";

            maxDistance.Value = ((AudioSourceWidgetDto)mapWidget_).MaxDistance;
            maxDistance.OwnerId = mapWidget_.Id;
            maxDistance.Reserved = true;

            properties.AddProperty(maxDistance);


            PropertyDto volume = new PropertyDto();
            volume.Name = "Volume";

            volume.Value = ((AudioSourceWidgetDto)mapWidget_).Volume;
            volume.OwnerId = mapWidget_.Id;
            volume.Reserved = true;

            properties.AddProperty(volume);

            return;
        }

        public void PropertyValueChanged(string name, ref object value, ref bool cancel)
        {
            switch (name.ToUpper())
            {
                case "NAME":

                    string particleEmitterName = (string)value;

                    try
                    {
                        projectController_.SetAudioSourceWidgetName(mapWidget_.Id, particleEmitterName);
                    }
                    catch (InvalidNameException ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message.ToString(), "Invalid Name", System.Windows.Forms.MessageBoxButtons.OK);
                        cancel = true;
                    }

                    break;

                case "POSITIONX":

                    int x = (int)value;
                    int y = mapWidget_.BoundingBox.Top;

                    projectController_.SetMapWidgetPosition(mapWidget_.Id, mapWidget_.Type, new Point2D(x, y));

                    break;

                case "POSITIONY":

                    x = mapWidget_.BoundingBox.Left;
                    y = (int)value;

                    projectController_.SetMapWidgetPosition(mapWidget_.Id, mapWidget_.Type, new Point2D(x, y));

                    break;

                case "AUDIO":

                    Guid audioId = projectController_.GetAudioAssetIdFromName((string)value);

                    projectController_.SetAudioSourceWidgetAudio(mapWidget_.Id, audioId);

                    break;

                case "AUTOPLAY":

                    bool autoplay = (bool)value;

                    projectController_.SetAudioSourceWidgetAutoplay(mapWidget_.Id, autoplay);

                    break;

                case "LOOP":

                    bool loop = (bool)value;

                    projectController_.SetAudioSourceWidgetLoop(mapWidget_.Id, loop);

                    break;

                case "MINDISTANCE":

                    int minDistance = (int)value;

                    projectController_.SetAudioSourceWidgetMinDistance(mapWidget_.Id, minDistance);

                    break;

                case "MAXDISTANCE":

                    int maxDistance = (int)value;

                    projectController_.SetAudioSourceWidgetMaxDistance(mapWidget_.Id, maxDistance);

                    break;

                case "VOLUME":

                    float volume = (float)value;

                    if (volume < 0.0)
                    {
                        volume = 0.0f;
                    }
                    else if (volume > 1.0)
                    {
                        volume = 1.0f;
                    }

                    value = volume;

                    projectController_.SetAudioSourceWidgetVolume(mapWidget_.Id, volume);

                    break;
            }
        }
    }
}
