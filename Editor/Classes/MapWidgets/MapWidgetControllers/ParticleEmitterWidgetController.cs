using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public class ParticleEmitterWidgetController : IMapWidgetController 
    {
        IProjectController projectController_;
        MapWidgetDto mapWidget_;
        Size size_;

        public ParticleEmitterWidgetController(IProjectController projectController)
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

        public void RenderBackground(Graphics g, int x, int y)
        {
        }

        public void Render(Graphics g, int x, int y)
        {
            Point[] trianglePoints = {new Point(x, y + size_.Height),
                                      new Point(x + (size_.Width / 2), y),
                                      new Point(x + size_.Width, y + size_.Height)
                                     };

            g.FillPolygon(Globals.bParticleEmitter, trianglePoints);
            g.DrawPolygon(Globals.pParticleEmitter, trianglePoints);

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


            PropertyDto particlesPerEmission = new PropertyDto();
            particlesPerEmission.Name = "ParticlesPerEmission";
            particlesPerEmission.Value = ((ParticleEmitterWidgetDto)mapWidget_).ParticlesPerEmission;
            particlesPerEmission.OwnerId = mapWidget_.Id;
            particlesPerEmission.Reserved = true;

            properties.AddProperty(particlesPerEmission);


            PropertyDto maxParticles = new PropertyDto();
            maxParticles.Name = "MaxParticles";
            maxParticles.Value = ((ParticleEmitterWidgetDto)mapWidget_).MaxParticles;
            maxParticles.OwnerId = mapWidget_.Id;
            maxParticles.Reserved = true;

            properties.AddProperty(maxParticles);


            PropertyDto interval = new PropertyDto();
            interval.Name = "Interval";
            interval.Value = ((ParticleEmitterWidgetDto)mapWidget_).Interval;
            interval.OwnerId = mapWidget_.Id;
            interval.Reserved = true;

            properties.AddProperty(interval);


            PropertyDto particleLifespan = new PropertyDto();
            particleLifespan.Name = "ParticleLifespan";
            particleLifespan.Value = ((ParticleEmitterWidgetDto)mapWidget_).ParticleLifespan;
            particleLifespan.OwnerId = mapWidget_.Id;
            particleLifespan.Reserved = true;

            properties.AddProperty(particleLifespan);


            PropertyDto active = new PropertyDto();
            active.Name = "Active";
            active.Value = ((ParticleEmitterWidgetDto)mapWidget_).Active;
            active.OwnerId = mapWidget_.Id;
            active.Reserved = true;

            properties.AddProperty(active);


            PropertyDto particleEmitterParticleType = new PropertyDto();
            particleEmitterParticleType.Name = "ParticleType";

            particleEmitterParticleType.Value = ((ParticleEmitterWidgetDto)mapWidget_).ParticleTypeName;
            particleEmitterParticleType.OwnerId = mapWidget_.Id;
            particleEmitterParticleType.Reserved = true;

            particleEmitterParticleType.TypeConverter = typeof(ParticleConverter);

            properties.AddProperty(particleEmitterParticleType);


            PropertyDto particleEmitterBehavior = new PropertyDto();
            particleEmitterBehavior.Name = "Behavior";

            particleEmitterBehavior.Value = ((ParticleEmitterWidgetDto)mapWidget_).BehaviorName;
            particleEmitterBehavior.OwnerId = mapWidget_.Id;
            particleEmitterBehavior.Reserved = true;

            particleEmitterBehavior.TypeConverter = typeof(ParticleEmitterConverter);

            properties.AddProperty(particleEmitterBehavior);


            PropertyDto particleEmitterAnimation = new PropertyDto();
            particleEmitterAnimation.Name = "Animation";

            particleEmitterAnimation.Value = ((ParticleEmitterWidgetDto)mapWidget_).AnimationName;
            particleEmitterAnimation.OwnerId = mapWidget_.Id;
            particleEmitterAnimation.Reserved = true;

            particleEmitterAnimation.TypeConverter = typeof(AnimationConverter);

            properties.AddProperty(particleEmitterAnimation);


            PropertyDto attachParticles = new PropertyDto();
            attachParticles.Name = "AttachParticles";
            attachParticles.Value = ((ParticleEmitterWidgetDto)mapWidget_).AttachParticles;
            attachParticles.OwnerId = mapWidget_.Id;
            attachParticles.Reserved = true;

            properties.AddProperty(attachParticles);

            PropertyDto animationFramesPerSecond = new PropertyDto();
            animationFramesPerSecond.Name = "AnimationFramesPerSecond";
            animationFramesPerSecond.Value = ((ParticleEmitterWidgetDto)mapWidget_).AnimationFramesPerSecond;
            animationFramesPerSecond.OwnerId = mapWidget_.Id;
            animationFramesPerSecond.Reserved = true;

            properties.AddProperty(animationFramesPerSecond);

            return;
        }

        public void PropertyValueChanged(string name, ref object value, ref bool cancel)
        {
            switch (name.ToUpper())
            {
                case "NAME":

                    // Map widget name property is now handled automatically because it is shared among all map widget types.

                    //string particleEmitterName = (string)value;

                    //try
                    //{
                    //    projectController_.SetParticleEmitterWidgetName(mapWidget_.Id, particleEmitterName);
                    //}
                    //catch (InvalidNameException ex)
                    //{
                    //    System.Windows.Forms.MessageBox.Show(ex.Message.ToString(), "Invalid Name", System.Windows.Forms.MessageBoxButtons.OK);
                    //    cancel = true;
                    //}

                    break;

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

                case "PARTICLETYPE":
                    
                    Guid particleId = projectController_.GetParticleIdFromName((string)value);

                    projectController_.SetParticleEmitterWidgetParticleType(mapWidget_.Id, particleId);

                    break;

                case "BEHAVIOR":

                    Guid particleEmitterId = projectController_.GetParticleEmitterIdFromName((string)value);

                    projectController_.SetParticleEmitterWidgetBehavior(mapWidget_.Id, particleEmitterId);

                    break;

                case "ANIMATION":

                    Guid animationtId = projectController_.GetAnimationIdFromName((string)value);

                    projectController_.SetParticleEmitterWidgetAnimation(mapWidget_.Id, animationtId);

                    break;

                case "ATTACHPARTICLES":

                    bool attachParticles = (bool)value;

                    projectController_.SetParticleEmitterWidgetAttachParticles(mapWidget_.Id, attachParticles);

                    break;

                case "INTERVAL":
                    
                    double interval = (double)value;

                    projectController_.SetParticleEmitterWidgetInterval(mapWidget_.Id, interval);

                    break;

                case "PARTICLELIFESPAN":

                    double particleLifespan = (double)value;

                    projectController_.SetParticleEmitterWidgetParticleLifespan(mapWidget_.Id, particleLifespan);

                    break;

                case "ACTIVE":

                    bool active = (bool)value;

                    projectController_.SetParticleEmitterWidgetActive(mapWidget_.Id, active);

                    break;

                case "PARTICLESPEREMISSION":

                    int particlesPerEmission = (int)value;

                    projectController_.SetParticleEmitterWidgetParticlesPerEmission(mapWidget_.Id, particlesPerEmission);

                    break;

                case "MAXPARTICLES":

                    int maxParticles = (int)value;

                    projectController_.SetParticleEmitterWidgetMaxParticles(mapWidget_.Id, maxParticles);

                    break;

                case "ANIMATIONFRAMESPERSECOND":

                    int framesPerSecond = (int)value;

                    projectController_.SetParticleEmitterWidgetAnimationFramesPerSecond(mapWidget_.Id, framesPerSecond);

                    break;
            }
        }
    }
}
