using System;
using System.Collections.Generic;
using System.Drawing;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    class HudElementWidgetController : IMapWidgetController
    {
        #region Constructors

        public HudElementWidgetController(IProjectController projectController, Guid hudElementId)
        {
            hudElementId_ = hudElementId;

            bitmapResources_ = new List<BitmapResourceDto>();

            projectController_ = projectController;

            renderOffset_ = new List<Point2D>();

            sheetCellIndex_ = new List<int>();
        }

        #endregion

        #region Private Variables


        List<BitmapResourceDto> bitmapResources_;
        
        Guid                    hudElementId_ = Guid.Empty;

        IProjectController      projectController_;

        List<Point2D>           renderOffset_;

        List<int>               sheetCellIndex_;

        // The offset to the stage origin.
        Size                    stageOriginTransformation_ = new Size(0, 0);

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
                hudElementMapWidget_ = (HudElementWidgetDto)mapWidget_;
            }
        }
        MapWidgetDto mapWidget_;
        HudElementWidgetDto hudElementMapWidget_;

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

        public void Initialize()
        {
            ProjectDto project = projectController_.GetProjectDto();
            
            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();

            HudElementDto hudElement = projectController_.GetHudElement(hudElementId_);

            if (hudElement.InitialStateId != Guid.Empty)
            {
                StateDto initialState = projectController_.GetState(hudElement.InitialStateId);

                if (initialState != null)
                {
                    int stageWidth = hudElement.StageWidth;

                    int stageHeight = hudElement.StageHeight;

                    Point stageCenter = new Point(stageWidth / 2, stageHeight / 2);

                    switch (hudElement.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:

                            stageOriginTransformation_ = new Size(0, 0);

                            break;

                        case OriginLocation.Center:

                            stageOriginTransformation_ = new Size(stageCenter.X, stageCenter.Y);

                            break;

                        case OriginLocation.TopMiddle:

                            stageOriginTransformation_ = new Size(stageCenter.X, 0);

                            break;

                        case OriginLocation.BottomMiddle:

                            stageOriginTransformation_ = new Size(stageCenter.X, stageHeight);

                            break;
                    }

                    int animationSlotCount = project.AnimationSlots[initialState.Id].Count;

                    // Loop through each animation slot in the state.
                    for (int o = 0; o < animationSlotCount; o++)
                    {
                        AnimationSlotDto currentAnimationSlot = project.AnimationSlots[initialState.Id][o];

                        Guid currentAnimationId = currentAnimationSlot.Animation;

                        // If the animation slot is pointing to a valid animation...
                        if (currentAnimationId != Guid.Empty)
                        {
                            AnimationDto currentAnimation = projectController_.GetAnimation(currentAnimationId);

                            Guid spriteSheetId = currentAnimation.SpriteSheet;

                            // If the animation has at least one frame...
                            if (project.Frames[currentAnimationId].Count > 0 && spriteSheetId != Guid.Empty)
                            {
                                SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId);

                                Guid resourceId = spriteSheet.BitmapResourceId;

                                // Separate resources dto removed in 2.2 format.
                                //BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];
                                BitmapResourceDto bitmapResource = project.Bitmaps[resourceId];

                                // Get the index of the sprite sheet, and the cell of the first frame.
                                if (project.Frames[currentAnimationId][0].SheetCellIndex.HasValue == true)
                                {
                                    int sheetCellIndex = project.Frames[currentAnimationId][0].SheetCellIndex.Value;

                                    if (project.Frames[currentAnimationId][0].SheetCellIndex.Value > -1)
                                    {
                                        try
                                        {
                                            Point2D renderOffset = new Point2D(0, 0);

                                            Size animationSlotTransformation = new Size(0, 0);

                                            switch (currentAnimationSlot.OriginLocation)
                                            {
                                                case OriginLocation.TopLeft:

                                                    animationSlotTransformation = new Size(0, 0);

                                                    break;

                                                case OriginLocation.Center:

                                                    animationSlotTransformation = new Size((int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2), (int)((spriteSheet.CellHeight * spriteSheet.ScaleFactor) / 2));

                                                    break;

                                                case OriginLocation.TopMiddle:

                                                    animationSlotTransformation = new Size((int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2), 0);

                                                    break;

                                                case OriginLocation.BottomMiddle:

                                                    animationSlotTransformation = new Size((int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2), (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor));

                                                    break;
                                            }

                                            // Note: It used to include the stage origin transformatoin, but this was removed when I made a change so that the actor 
                                            // "origin position point" was taken into account for rendering in the world editor (i.e. the mouse cursor was always
                                            // at the stage origin.) And in doing this, I realized that when rendering, I was just subtracting off the origin transform
                                            // again, and rather than have it cancel out that way, I could just not include it to begin with.

                                            //renderOffset.X = currentAnimationSlot.Position.X + stageOriginTransformation.Width - animationSlotTransformation.Width;

                                            //renderOffset.Y = currentAnimationSlot.Position.Y + stageOriginTransformation.Height - animationSlotTransformation.Height;

                                            renderOffset.X = currentAnimationSlot.Position.X - animationSlotTransformation.Width;

                                            renderOffset.Y = currentAnimationSlot.Position.Y - animationSlotTransformation.Height;

                                            bitmapResources_.Add(bitmapResource);

                                            sheetCellIndex_.Add(sheetCellIndex);

                                            renderOffset_.Add(renderOffset);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
            for (int i = 0; i < bitmapResources_.Count; i++)
            {
                int renderAtX = x + renderOffset_[i].X;
                int renderAtY = y + renderOffset_[i].Y;

                if (bitmapResources_[i] != null)
                {
                    if (bitmapResources_[i].BitmapImage == null)
                    {
                        projectController_.LoadBitmap(bitmapResources_[i].Id);

                        bitmapResources_[i].LoadedModules ^= (byte)EditorModule.Room;
                    }

                    if (sheetCellIndex_[i] > -1 && sheetCellIndex_[i] < bitmapResources_[i].SpriteSheetImageList.Count)
                    {
                        for (int u = 0; u < bitmapResources_[i].SpriteSheetImageList.ImageListRows; u++)
                        {
                            for (int v = 0; v < bitmapResources_[i].SpriteSheetImageList.ImageListCols; v++)
                            {
                                int subCellX = renderAtX + (v * Globals.maxImageListWidth);
                                int subCellY = renderAtY + (u * Globals.maxImageListWidth);

                                bitmapResources_[i].SpriteSheetImageList.ImageLists[u][v].Draw(g, subCellX, subCellY, sheetCellIndex_[i]);
                            }
                        }
                    }
                }
            }
        }

        public void RenderOverlay(Graphics g, Point viewOffset, bool isSelected, bool isSingularSelection, bool showOutline)
        {
            System.Drawing.Rectangle drawingRect = new System.Drawing.Rectangle(mapWidget_.BoundingBox.Left + viewOffset.X, mapWidget_.BoundingBox.Top + viewOffset.Y, mapWidget_.BoundingBox.Width, mapWidget_.BoundingBox.Height);

            if (isSelected == true)
            {
                g.FillRectangle(new SolidBrush(Globals.actorFillColor), drawingRect);
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
            properties.RemoveProperty("AcceptInput");
            properties.RemoveProperty("RenderOrder");

            // Name
            properties.RemoveProperty("Name");
            PropertyDto name = new PropertyDto();
            name.Name = "Name";
            name.Value = mapWidget_.Name;
            name.OwnerId = mapWidget_.Id;
            name.Reserved = true;

            properties.AddProperty(name);

            // Position X
            properties.RemoveProperty("PositionX");
            PropertyDto positionX = new PropertyDto();
            positionX.Name = "PositionX";
            positionX.Value = mapWidget_.Position.X;
            positionX.OwnerId = mapWidget_.Id;
            positionX.Reserved = true;

            properties.AddProperty(positionX);

            // Position Y
            properties.RemoveProperty("PositionY");
            PropertyDto positionY = new PropertyDto();
            positionY.Name = "PositionY";
            positionY.Value = mapWidget_.Position.Y;
            positionY.OwnerId = mapWidget_.Id;
            positionY.Reserved = true;

            properties.AddProperty(positionY);

            // Accept Input
            properties.RemoveProperty("AcceptInput");
            PropertyDto acceptInput = new PropertyDto();
            acceptInput.Name = "AcceptInput";
            acceptInput.Value = hudElementMapWidget_.AcceptInput;
            acceptInput.OwnerId = mapWidget_.Id;
            acceptInput.Reserved = true;

            properties.AddProperty(acceptInput);

            // Render Order
            properties.RemoveProperty("RenderOrder");
            PropertyDto renderOrder = new PropertyDto();
            renderOrder.Name = "RenderOrder";
            renderOrder.Value = hudElementMapWidget_.RenderOrder;
            renderOrder.OwnerId = mapWidget_.Id;
            renderOrder.Reserved = true;

            properties.AddProperty(renderOrder);
            
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
            // Get the delta vector between the position and the bounding box corner.
            // Use this to ensure the two remain in synch during the position update.
            Point2D delta = new Point2D(mapWidget_.Position.X - mapWidget_.BoundingBox.Left, mapWidget_.Position.Y - mapWidget_.BoundingBox.Top);

            mapWidget_.Position.X = position.X;
            mapWidget_.Position.Y = position.Y;

            mapWidget_.BoundingBox.Left = position.X - delta.X;
            mapWidget_.BoundingBox.Top = position.Y - delta.Y;
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion
    }
}
