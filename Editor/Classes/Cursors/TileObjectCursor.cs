using System;
using System.Collections.Generic;
using System.Drawing;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class TileObjectCursor : RoomEditorCursor
    {
        public TileObjectCursor(IProjectController projectController)
        {
            offset_ = new Point2D(0, 0);

            projectController_ = projectController;
        }

        #region Private Variables
        
        private Point2D offset_;

        private IProjectController projectController_;
        
        private TileObjectDto tileObject_;

        private TileSheetDto tileSheet_;

        #endregion

        #region Properties

        public override bool GridAligned => true;

        public override Point2D Offset
        {
            get
            {
                offset_.X = projectController_.GetUiState().TileObjectCursorCell[tileObjectId_].X * projectController_.GetProjectDto().TileSize * -1;

                offset_.Y = projectController_.GetUiState().TileObjectCursorCell[tileObjectId_].Y * projectController_.GetProjectDto().TileSize * -1;

                return offset_;
            }
            set
            {
                // No-op.
            }
        }

        public Guid TileObjectId
        {
            get { return tileObjectId_; }
            set
            {
                tileObjectId_ = value;

                tileObject_ = projectController_.GetTileObject(tileObjectId_);

                tileSheet_ = projectController_.GetTileSheet(tileObject_.OwnerId);
            }
        }
        private Guid tileObjectId_ = Guid.Empty;

        #endregion

        #region Public Functions

        public override void Render(Graphics g, int x, int y, int gridOffsetX, int gridOffsetY)
        {
            if (tileObject_ != null)
            {
                ProjectDto project = projectController_.GetProjectDto();
                
                // Separate resources dto removed in 2.2 format.
                //BitmapResourceDto bitmap = resources.Bitmaps[tileObject_.BitmapResourceId];
                BitmapResourceDto bitmap = projectController_.GetBitmapResource(tileObject_.BitmapResourceId, true);

                bitmap.LoadedModules ^= (byte)EditorModule.Cursor;

                // Find the first x and y that align to the grid.
                int tileSize = project.TileSize;

                // Snap to the grid.
                int gridAlignedX = ((int)(x / tileSize) * tileSize);

                int gridAlignedY = ((int)(y / tileSize) * tileSize);

                gridAlignedX += gridOffsetX;

                gridAlignedY += gridOffsetY;

                // Adjust by the cursor offset (the tile object cursor cell).
                gridAlignedX -= Offset.X;

                gridAlignedY -= Offset.Y;

                int sourceX = (int)((tileObject_.TopLeftCornerColumn * project.TileSize) / tileSheet_.ScaleFactor);

                int sourceY = (int)((tileObject_.TopLeftCornerRow * project.TileSize) / tileSheet_.ScaleFactor);

                int sourceWidth = (int)((tileObject_.Columns * project.TileSize) / tileSheet_.ScaleFactor);

                int sourceHeight = (int)((tileObject_.Rows * project.TileSize) / tileSheet_.ScaleFactor);

                System.Drawing.Rectangle sourceRect = new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);

                int destinationWidth = (int)(sourceWidth * tileSheet_.ScaleFactor);

                int destinationHeight = (int)(sourceHeight * tileSheet_.ScaleFactor);

                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(gridAlignedX, gridAlignedY, destinationWidth, destinationHeight);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                
                g.DrawImage(bitmap.BitmapImageWithTransparency, destRect, sourceX, sourceY, sourceWidth, sourceHeight, GraphicsUnit.Pixel, null);

                g.DrawRectangle(new Pen(Globals.actorOutlineColor), destRect);
            }
        }

        #endregion
    }
}
