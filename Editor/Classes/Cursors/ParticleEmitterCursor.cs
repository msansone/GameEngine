using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public class ParticleEmitterCursor : RoomEditorCursor
    {
        IProjectController projectController_;
        
        public ParticleEmitterCursor(IProjectController projectController)
        {
            projectController_ = projectController;
            
            int halfWidth = (Size.Width / 2);
            int halfHeight = (Size.Height / 2);

            Offset = new Point2D(halfWidth, halfHeight);

            LayerNameOffset = new Point(Size.Width, Size.Height);
        }

        public override void Render(Graphics g, int x, int y, int gridOffsetX, int gridOffsetY)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();
            
            Point[] trianglePoints = {new Point(x - Offset.X, y + Offset.Y),
                                      new Point(x, y - Offset.Y),
                                      new Point(x + Offset.X, y + Offset.Y)
                                     };

            g.FillPolygon(Globals.bParticleEmitter, trianglePoints);
            g.DrawPolygon(Globals.pParticleEmitter, trianglePoints);

            //Font f = new Font(FontFamily.GenericSansSerif, 8.0f);
            //g.DrawString("Particle Emitter", f, new SolidBrush(Color.White), (float)x, (float)y + halfHeight);
        }        
    }
}
