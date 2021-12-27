using System.Collections.Generic;
using System.Drawing;

namespace FiremelonEditor2
{
    public class WorldGeometryCursor : RoomEditorCursor
    {
        IProjectController projectController_;
        
        public WorldGeometryCursor(IProjectController projectController)
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

            int halfWidth = (Size.Width / 2);
            int halfHeight = (Size.Height / 2);

            int quarterWidth = (Size.Width / 4);
            int quarterHeight = (Size.Height / 4);

            int eighthWidth = (Size.Width / 8);
            int eighthHeight = (Size.Height / 8);

            List<Point> lstPoints = new List<Point>();

            lstPoints.Add(new Point(x - eighthWidth, y - halfHeight));
            lstPoints.Add(new Point(x + eighthWidth, y - halfHeight));
            lstPoints.Add(new Point(x + eighthWidth, y - eighthWidth));
            lstPoints.Add(new Point(x + halfWidth, y - eighthWidth));            
            lstPoints.Add(new Point(x + halfWidth, y + eighthWidth));
            lstPoints.Add(new Point(x + eighthWidth, y + eighthWidth));
            lstPoints.Add(new Point(x + eighthWidth, y + halfHeight));
            lstPoints.Add(new Point(x - eighthWidth, y + halfHeight));
            lstPoints.Add(new Point(x - eighthWidth, y + eighthWidth));
            lstPoints.Add(new Point(x - halfWidth, y + eighthWidth));           
            lstPoints.Add(new Point(x - halfWidth, y - eighthWidth));
            lstPoints.Add(new Point(x - eighthWidth, y - eighthWidth));

            g.FillPolygon(Globals.bWorldGeometryCursorFill, lstPoints.ToArray());
            g.DrawPolygon(Globals.pWorldGeometryCursorOutline, lstPoints.ToArray());            
        }        
    }
}
