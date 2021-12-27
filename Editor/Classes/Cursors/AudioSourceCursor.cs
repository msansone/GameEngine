using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace FiremelonEditor2
{
    public class AudioSourceCursor : RoomEditorCursor
    {
        IFiremelonEditorFactory factory_;
        IUtilityFactory utilityFactory_;
        IProjectController projectController_;

        Bitmap bitmap_;

        public AudioSourceCursor(IProjectController projectController)
        {
            factory_ = new FiremelonEditorFactory();
            utilityFactory_ = new UtilityFactory();
            projectController_ = projectController;

            IResourceBitmapReader resourceBitmapReader = factory_.NewResourceBitmapReader();
            IBitmapUtility bitmapUtility = utilityFactory_.NewBitmapUtility();

            bitmap_ = bitmapUtility.ApplyTransparency(resourceBitmapReader.ReadResourceBitmap("audioicon.png"));

            Size = new Size(bitmap_.Width, bitmap_.Height);

            int halfWidth = (Size.Width / 2);
            int halfHeight = (Size.Height / 2);

            Offset = new Point2D(halfWidth, halfHeight);

            LayerNameOffset = new Point(Size.Width, Size.Height);
        }

        public override void Render(Graphics g, int x, int y, int gridOffsetX, int gridOffsetY)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            g.DrawImageUnscaled(bitmap_, new Point(x - Offset.X, y - Offset.Y));
        }        
    }
}
