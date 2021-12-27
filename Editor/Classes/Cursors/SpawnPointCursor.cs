using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public class SpawnPointCursor : RoomEditorCursor
    {
        IProjectController projectController_;

        public SpawnPointCursor(IProjectController projectController)
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
            
            g.DrawEllipse(Globals.pSpawnPoint, x - Offset.X, y - Offset.Y, Size.Width, Size.Height);
            g.DrawLine(Globals.pSpawnPoint, x - Offset.X, y, x + Offset.Y, y);
            g.DrawLine(Globals.pSpawnPoint, x, y - Offset.Y, x, y + Offset.Y);
        }

    }
}
