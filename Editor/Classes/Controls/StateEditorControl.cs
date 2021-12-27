using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


/*
    There are three relative coordinate spaces, canvas space, stage space, and animation space.

    Canvas space origin is always the top left.

    Stage space and animation space can each specify their own origin point. For rendering purposes
    the default will be assumed to be top left for each, and adjustments will be made based on the
    user specified origin point.

    Canvas
    _____________________________________________________
    |                                                     |
    |             Stage                                   |
    |             __________________________              |
    |            |                          |             |
    |            |      Animation           |             |
    |            |      _____________       |             |
    |            |     |             |      |             |
    |            |     |             |      |             |
    |            |     |_____________|      |             |
    |            |                          |             |
    |            |__________________________|             |
    |                                                     |
    |                                                     |
    |_____________________________________________________|

*/


namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class StateEditorControl : UserControl, IStateEditorControl
    {
        #region Constructors

        public StateEditorControl(IProjectController projectController, IExceptionHandler exceptionHandler)
        {
            InitializeComponent();

            IUtilityFactory utilityFactory = new UtilityFactory();

            projectController_ = projectController;

            exceptionHandler_ = exceptionHandler;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            backgroundGenerator_ = firemelonEditorFactory_.NewBackgroundGenerator();
            
            bmpStageImage_ = null;

            linearAlgebraUtility_ = utilityFactory.NewLinearAlgebraUtility();

            lockRefresh_ = false;

            overlayGenerator_ = firemelonEditorFactory_.NewOverlayGenerator();

            // Initialize to true, to ensure the bitmap gets generated.
            regenerateBitmap_ = true;

            scrollableRegion_ = new Rectangle(0, 0, 0, 0);
            
            vsState.SmallChange = 1;
            vsState.LargeChange = 1;
            hsState.SmallChange = 1;
            hsState.LargeChange = 1;
        }

        #endregion

        #region Private Variables

        private IBackgroundGenerator    backgroundGenerator_;
        
        private Bitmap                  bmpStageBackground_;

        private Bitmap                  bmpStageImage_;

        private Bitmap                  bmpStageOverlay_;

        private IExceptionHandler       exceptionHandler_;
        
        private IFiremelonEditorFactory firemelonEditorFactory_;
        
        private ILinearAlgebraUtility   linearAlgebraUtility_;

        private bool                    lockRefresh_;

        private IOverlayGenerator       overlayGenerator_;

        private IProjectController      projectController_;

        private bool                    regenerateBitmap_;

        // The scrollbars can be based on the farthest extents of the stage elements, or of the background, whichever
        // extends farther. Generate a scrollable region rect that is the minimum bounding box of both the stage and 
        // stage elements.
        private Rectangle               scrollableRegion_;

        private Point2D                 stageElementsOffset_ = new Point2D(0, 0);

        private Point2D                 stageOriginPointInNativeStageSpace_ = new Point2D(0, 0);

        #endregion

        #region Properties

        public int SelectedAnimationSlotIndex
        {
            get { return selectedAnimationSlotIndex_; }
            set { selectedAnimationSlotIndex_ = value; }
        }
        private int selectedAnimationSlotIndex_;

        public IStateDtoProxy State
        {
            get
            {
                return stateProxy_;
            }
            set
            {
                stateProxy_ = value;

                resize(false);
            }
        }
        private IStateDtoProxy stateProxy_;

        #endregion

        #region Public Functions
        
        public void LockRefresh()
        {
            lockRefresh_ = true;
        }

        public void RefreshState(bool regenerateBackground)
        {
            calculateStageOriginInNativeSpace();

            if (regenerateBackground == true)
            {
                backgroundGenerator_.Regenerate();
                
                overlayGenerator_.Regenerate();
            }

            regenerateBitmap_ = true;
            
            // Resize to update the scrollbars.
            if (this.Visible == true)
            {
                resize(false);
                
                pbState.Refresh();
            }
        }

        public void UnlockRefresh()
        {
            lockRefresh_ = false;
        }

        #endregion

        #region Private Functions

        // This needs to be called whenever a new state is set, the stage size is changed, or the origin enum value is changed.
        // Calling it in the RefreshState method will handle all of these.
        private void calculateStageOriginInNativeSpace()
        {
            if (stateProxy_ != null)
            {
                int stageWidth = stateProxy_.StageWidth;

                int stageHeight = stateProxy_.StageHeight;

                // Half of the size of the stage, used for centering.
                Size stageHalfSize = new Size(stageWidth / 2, stageHeight / 2);

                switch (stateProxy_.StageOriginLocation)
                {
                    case OriginLocation.TopLeft:

                        // The native stage origin is top left, so no change is necessary.

                        stageOriginPointInNativeStageSpace_.X = 0;

                        stageOriginPointInNativeStageSpace_.Y = 0;

                        break;

                    case OriginLocation.TopMiddle:

                        // Calculate the center point of the x axis stage in the native stage space.

                        stageOriginPointInNativeStageSpace_.X = stageHalfSize.Width;

                        stageOriginPointInNativeStageSpace_.Y = 0;

                        break;

                    case OriginLocation.TopRight:

                        // Calculate the center point of the x axis stage in the native stage space.

                        stageOriginPointInNativeStageSpace_.X = stageWidth;

                        stageOriginPointInNativeStageSpace_.Y = 0;

                        break;

                    case OriginLocation.MiddleLeft:

                        // Calculate the center point of the stage in the native stage space.

                        stageOriginPointInNativeStageSpace_.X = 0;

                        stageOriginPointInNativeStageSpace_.Y = stageHalfSize.Height;

                        break;

                    case OriginLocation.Center:

                        // Calculate the center point of the stage in the native stage space.

                        stageOriginPointInNativeStageSpace_.X = stageHalfSize.Width;

                        stageOriginPointInNativeStageSpace_.Y = stageHalfSize.Height;

                        break;

                    case OriginLocation.MiddleRight:

                        // Calculate the center point of the stage in the native stage space.

                        stageOriginPointInNativeStageSpace_.X = stageWidth;

                        stageOriginPointInNativeStageSpace_.Y = stageHalfSize.Height;

                        break;

                    case OriginLocation.BottomLeft:

                        // Calculate the center point of the x axis stage in the native stage space.

                        stageOriginPointInNativeStageSpace_.X = 0;

                        stageOriginPointInNativeStageSpace_.Y = stageHeight;

                        break;

                    case OriginLocation.BottomMiddle:

                        // Calculate the center point of the x axis stage in the native stage space.

                        stageOriginPointInNativeStageSpace_.X = stageHalfSize.Width;

                        stageOriginPointInNativeStageSpace_.Y = stageHeight;

                        break;

                    case OriginLocation.BottomRight:

                        // Calculate the center point of the x axis stage in the native stage space.

                        stageOriginPointInNativeStageSpace_.X = stageWidth;

                        stageOriginPointInNativeStageSpace_.Y = stageHeight;

                        break;
                }
            }
        }

        private Point2D getAnimationFrameInStageSpace(AnimationSlotDto currentAnimationSlot, SpriteSheetDto spriteSheet)
        {
            int stageWidth = stateProxy_.StageWidth;

            int stageHeight = stateProxy_.StageHeight;

            // Half of the size of the stage, used for centering.
            Size stageHalfSize = new Size(stageWidth / 2, stageHeight / 2);

            Point2D animationFrameInStageSpace = new Point2D(0, 0);
            
            Point2D animationOriginPointInNativeAnimationSpace = getAnimationOriginPointInNativeAnimationSpace(currentAnimationSlot, spriteSheet);

            switch (stateProxy_.StageOriginLocation)
            {
                case OriginLocation.TopLeft:

                    // The default stage origin is top left, so we can just use the animation slot position without any coordinate space transformation.

                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                    break;

                case OriginLocation.TopMiddle:

                    // Get the center of the x axis and the native y axis position.
                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                    break;

                case OriginLocation.TopRight:

                    // Get the center of the x axis and the native y axis position.
                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X + stageWidth - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                    break;
                    
                case OriginLocation.MiddleLeft:

                    // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                    break;

                case OriginLocation.Center:

                    // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                    break;

                case OriginLocation.MiddleRight:

                    // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X + stageWidth - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                    break;

                case OriginLocation.BottomLeft:

                    // Get the center of the x axis and the native y axis position.
                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y + stageHeight - animationOriginPointInNativeAnimationSpace.Y;

                    break;

                case OriginLocation.BottomMiddle:

                    // Get the center of the x axis and the native y axis position.
                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y + stageHeight - animationOriginPointInNativeAnimationSpace.Y;

                    break;

                case OriginLocation.BottomRight:

                    // Get the center of the x axis and the native y axis position.
                    animationFrameInStageSpace.X = currentAnimationSlot.Position.X + stageWidth - animationOriginPointInNativeAnimationSpace.X;

                    animationFrameInStageSpace.Y = currentAnimationSlot.Position.Y + stageHeight - animationOriginPointInNativeAnimationSpace.Y;

                    break;
            }

            return animationFrameInStageSpace;
        }

        private Point2D getAnimationOriginPointInNativeAnimationSpace(AnimationSlotDto currentAnimationSlot, SpriteSheetDto spriteSheet)
        {
            Size animationFrameSize = new Size((int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor), (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor));

            Size animationFrameHalfSize = new Size((int)((animationFrameSize.Width) / 2), (int)((animationFrameSize.Height) / 2));

            // Native animation space uses the default origin of the top left of the animation frame.
            Point2D animationOriginPointInNativeAnimationSpace = new Point2D(0, 0);

            switch (currentAnimationSlot.OriginLocation)
            {
                case OriginLocation.TopLeft:

                    animationOriginPointInNativeAnimationSpace.X = 0;

                    animationOriginPointInNativeAnimationSpace.Y = 0;

                    break;

                case OriginLocation.TopMiddle:

                    animationOriginPointInNativeAnimationSpace.X = animationFrameHalfSize.Width;

                    animationOriginPointInNativeAnimationSpace.Y = 0;

                    break;

                case OriginLocation.TopRight:

                    animationOriginPointInNativeAnimationSpace.X = animationFrameSize.Width;

                    animationOriginPointInNativeAnimationSpace.Y = 0;

                    break;

                case OriginLocation.MiddleLeft:

                    animationOriginPointInNativeAnimationSpace.X = 0;

                    animationOriginPointInNativeAnimationSpace.Y = animationFrameHalfSize.Height;

                    break;

                case OriginLocation.Center:

                    animationOriginPointInNativeAnimationSpace.X = animationFrameHalfSize.Width;

                    animationOriginPointInNativeAnimationSpace.Y = animationFrameHalfSize.Height;

                    break;

                case OriginLocation.MiddleRight:

                    animationOriginPointInNativeAnimationSpace.X = animationFrameSize.Width;

                    animationOriginPointInNativeAnimationSpace.Y = animationFrameHalfSize.Height;

                    break;

                case OriginLocation.BottomLeft:

                    animationOriginPointInNativeAnimationSpace.X = 0;

                    animationOriginPointInNativeAnimationSpace.Y = animationFrameSize.Height;

                    break;

                case OriginLocation.BottomMiddle:

                    animationOriginPointInNativeAnimationSpace.X = animationFrameHalfSize.Width;

                    animationOriginPointInNativeAnimationSpace.Y = animationFrameSize.Height;

                    break;

                case OriginLocation.BottomRight:

                    animationOriginPointInNativeAnimationSpace.X = animationFrameSize.Width;

                    animationOriginPointInNativeAnimationSpace.Y = animationFrameSize.Height;

                    break;
            }

            return animationOriginPointInNativeAnimationSpace;
        }

        // Find the farthest left, right, top, and bottom points on the stage for any renderable elements (animations, hitboxes).
        private Rectangle getStageElementsBoundingBox()
        {
            int left   = 0;
            int right  = 0;
            int top    = 0;
            int bottom = 0;

            ProjectDto project = projectController_.GetProjectDto();

            // First check the animation positions.
            int animationCount = project.AnimationSlots[stateProxy_.Id].Count;

            for (int i = 0; i < animationCount; i++)
            {
                AnimationSlotDto currentAnimationSlot = project.AnimationSlots[stateProxy_.Id][i];

                Guid animationId = currentAnimationSlot.Animation;

                if (animationId != Guid.Empty)
                {
                    AnimationDto currentAnimation = projectController_.GetAnimation(animationId);

                    Guid spriteSheetId = currentAnimation.SpriteSheet;

                    if (spriteSheetId != Guid.Empty && project.Frames[currentAnimation.Id].Count > 0)
                    {
                        if (project.Frames[currentAnimation.Id][0].SheetCellIndex.HasValue == true)
                        {
                            int spriteSheetCellIndex = project.Frames[currentAnimation.Id][0].SheetCellIndex.Value;

                            SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId);

                            Guid bitmapResourceId = spriteSheet.BitmapResourceId;
                            
                            BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(bitmapResourceId, true);

                            if (spriteSheetCellIndex >= 0 && spriteSheetCellIndex < bitmapResource.SpriteSheetImageList.Count)
                            {
                                // Get the positions.
                                Point2D animationFrameInStageSpace = getAnimationFrameInStageSpace(currentAnimationSlot, spriteSheet);
                                
                                if (i == 0)
                                {
                                    left = animationFrameInStageSpace.X;

                                    top = animationFrameInStageSpace.Y;

                                    right = left + (int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor);

                                    bottom = top + (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor);
                                }
                                else
                                {
                                    if (animationFrameInStageSpace.X < left)
                                    {
                                        left = animationFrameInStageSpace.X;
                                    }

                                    if (animationFrameInStageSpace.Y < top)
                                    {
                                        top = animationFrameInStageSpace.Y;
                                    }

                                    if (animationFrameInStageSpace.X + (int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor) > right)
                                    {
                                        right = animationFrameInStageSpace.X + (int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor);
                                    }

                                    if (animationFrameInStageSpace.Y + (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor) > bottom)
                                    {
                                        bottom = animationFrameInStageSpace.Y + (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            // Then check the hitbox positions.
            List<HitboxDto> hitboxList = project.Hitboxes[stateProxy_.Id];

            int hitboxCount = hitboxList.Count;

            int stageWidth = stateProxy_.StageWidth;

            int stageHeight = stateProxy_.StageHeight;

            Size stageHalfSize = new Size(stageWidth / 2, stageHeight / 2);

            for (int i = 0; i < hitboxCount; i++)
            {
                HitboxDto currentHitbox = hitboxList[i];

                RectangleF hitboxRect = new RectangleF();

                // Convert the hitbox to a rect in TLC native space.
                switch (stateProxy_.StageOriginLocation)
                {
                    case OriginLocation.TopLeft:
                        hitboxRect.X = currentHitbox.HitboxRect.Left;
                        hitboxRect.Y = currentHitbox.HitboxRect.Top;
                        break;

                    case OriginLocation.Center:
                        hitboxRect.X = stageHalfSize.Width - (currentHitbox.HitboxRect.Width / 2) + currentHitbox.HitboxRect.Left;
                        hitboxRect.Y = stageHalfSize.Height - (currentHitbox.HitboxRect.Height / 2) + currentHitbox.HitboxRect.Top;
                        break;

                    case OriginLocation.TopMiddle:
                        hitboxRect.X = stageHalfSize.Width - (currentHitbox.HitboxRect.Width / 2) + currentHitbox.HitboxRect.Left;
                        hitboxRect.Y = currentHitbox.HitboxRect.Top;
                        break;

                    case OriginLocation.BottomMiddle:
                        hitboxRect.X = stageHalfSize.Width - (currentHitbox.HitboxRect.Width / 2) + currentHitbox.HitboxRect.Left;
                        hitboxRect.Y = stageHeight + currentHitbox.HitboxRect.Top;
                        break;
                }

                hitboxRect.Height = currentHitbox.HitboxRect.Height;

                hitboxRect.Width = currentHitbox.HitboxRect.Width;

                if (currentHitbox.RotationDegrees == 0.0f)
                {
                    // This will be populated by the rotation transform method.
                    currentHitbox.TransformedCorners.Clear();

                    currentHitbox.TransformedCorners.Add(new PointF(hitboxRect.Left, hitboxRect.Top));
                    currentHitbox.TransformedCorners.Add(new PointF(hitboxRect.Left + hitboxRect.Width, hitboxRect.Top));
                    currentHitbox.TransformedCorners.Add(new PointF(hitboxRect.Left + hitboxRect.Width, hitboxRect.Top + hitboxRect.Height));
                    currentHitbox.TransformedCorners.Add(new PointF(hitboxRect.Left, hitboxRect.Top + hitboxRect.Height));
                }
                else
                {
                    // Build a list of corner points and rotate them around the stage origin.
                    List<PointF> cornerPoints = new List<PointF>();

                    cornerPoints.Add(new PointF(hitboxRect.Left, hitboxRect.Top));
                    cornerPoints.Add(new PointF(hitboxRect.Left + hitboxRect.Width, hitboxRect.Top));
                    cornerPoints.Add(new PointF(hitboxRect.Left + hitboxRect.Width, hitboxRect.Top + hitboxRect.Height));
                    cornerPoints.Add(new PointF(hitboxRect.Left, hitboxRect.Top + hitboxRect.Height));

                    PointF translationToNative = new PointF(0, 0);

                    // Convert the hitbox to a rect in TLC native space.
                    switch (stateProxy_.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:
                            translationToNative = new PointF(0, 0);
                            break;

                        case OriginLocation.TopMiddle:
                            translationToNative = new PointF(stageHalfSize.Width, 0);
                            break;

                        case OriginLocation.TopRight:
                            translationToNative = new PointF(stageWidth, 0);
                            break;

                        case OriginLocation.MiddleLeft:
                            translationToNative = new PointF(0, stageHalfSize.Height);
                            break;
                            
                        case OriginLocation.Center:
                            translationToNative = new PointF(stageHalfSize.Width, stageHalfSize.Height);
                            break;

                        case OriginLocation.MiddleRight:
                            translationToNative = new PointF(stageWidth, stageHalfSize.Height);
                            break;

                        case OriginLocation.BottomLeft:
                            translationToNative = new PointF(0, stageHeight);
                            break;

                        case OriginLocation.BottomMiddle:
                            translationToNative = new PointF(stageHalfSize.Width, stageHeight);
                            break;

                        case OriginLocation.BottomRight:
                            translationToNative = new PointF(stageWidth, stageHeight);
                            break;
                    }

                    // This will be populated by the rotation transform method.
                    currentHitbox.TransformedCorners.Clear();

                    linearAlgebraUtility_.RotationTransform(cornerPoints, currentHitbox.TransformedCorners, translationToNative, currentHitbox.RotationDegrees);
                }
                
                if (currentHitbox.LeftMostCorner < left)
                {
                    left = (int)Math.Floor(currentHitbox.LeftMostCorner);
                }

                if (currentHitbox.TopMostCorner < top)
                {
                    top = (int)Math.Floor(currentHitbox.TopMostCorner);
                }

                if (currentHitbox.RightMostCorner > right)
                {
                    right = (int)Math.Ceiling(currentHitbox.RightMostCorner);
                }

                if (currentHitbox.BottomMostCorner > bottom)
                {
                    bottom = (int)Math.Ceiling(currentHitbox.BottomMostCorner);
                }
            }

            // BUG001: When drawing a rectangle along the left and top borders, the line gets get off. 
            // Adjust by 1 to account for this, effectively creating a padding region, and be 
            // sure to render everything else at a shifted position by 1 as well. This will 
            // ensure that every border line gets rendered correctly.
            
            left -= 1;

            top -= 1;

            return new Rectangle(left, top, right - left, bottom - top);
        }

        private void paint(Graphics g)
        {
            if (lockRefresh_ == false)
            {
                Point2D paddingOffset = new Point2D(1, 1); // See BUG001 for how the padding offset is used and why it was necessary.
                
                int stageWidth = stateProxy_.StageWidth;

                int stageHeight = stateProxy_.StageHeight;

                // Half of the size of the stage, used for centering.
                Size stageHalfSize = new Size(stageWidth / 2, stageHeight / 2);

                // The center point of the canvas, in canvas space.
                Point canvasCenterInCanvasSpace = new Point(pbState.Width / 2, pbState.Height / 2);

                // The left point of the stage, in canvas space. If the canvas is small such that the left extends
                // off canvas, it should be positioned to align to the left instead of centered.
                int stageLeftInCanvasSpace = canvasCenterInCanvasSpace.X - stageHalfSize.Width;

                if (stageLeftInCanvasSpace < 0)
                {
                    stageLeftInCanvasSpace = 0;
                }

                // The top point of the stage, in canvas space. If the canvas is small such that the top extends
                // off canvas, it should be positioned to align to the top instead of centered.
                int stageTopInCanvasSpace = canvasCenterInCanvasSpace.Y - stageHalfSize.Height;

                if (stageTopInCanvasSpace < 0)
                {
                    stageTopInCanvasSpace = 0;
                }

                Point stageTopLeftInCanvasSpace = new Point(stageLeftInCanvasSpace, stageTopInCanvasSpace);
                
                // Render the stage background at the default origin point.
                if (stageWidth > 0 && stageHeight > 0)
                {
                    bmpStageBackground_ = backgroundGenerator_.GenerateBackground(stageWidth, stageHeight);
                    
                    try
                    {
                        g.DrawImageUnscaled(bmpStageBackground_, new Point(stageTopLeftInCanvasSpace.X + (-1 * hsState.Value), stageTopLeftInCanvasSpace.Y + (-1 * vsState.Value)));
                    }
                    catch (Exception ex)
                    {
                        exceptionHandler_.HandleException(ex);
                    }

                }

                // If the stage elements were not modified, there's no need to regenerate the bitmap of them. It can just be rendered as is.
                if (regenerateBitmap_ == true)
                {
                    regenerateBitmap_ = false;

                    // Get the bounding box of the stage elements.
                    Rectangle stageElementsBoundingBox = getStageElementsBoundingBox();

                    // Build the scrollable region that minimally bounds both the stage and its elements.
                    int scrollableLeft = Math.Min(0, stageElementsBoundingBox.Left);

                    int scrollableTop = Math.Min(0, stageElementsBoundingBox.Top);

                    int scrollableWidth = Math.Max(stageWidth, stageElementsBoundingBox.Right) - scrollableLeft;

                    int scrollableHeight = Math.Max(stageHeight, stageElementsBoundingBox.Bottom) - scrollableTop;

                    scrollableRegion_ = new Rectangle(scrollableLeft, scrollableTop, scrollableWidth, scrollableHeight);

                    if (stageElementsBoundingBox.Width * stageElementsBoundingBox.Height > 0)
                    {
                        ProjectDto project = projectController_.GetProjectDto();

                        // Separate resources dto removed in 2.2 format.
                        //ProjectResourcesDto resources = projectController_.GetResources();

                        if (bmpStageImage_ != null)
                        {
                            bmpStageImage_.Dispose();

                            bmpStageImage_ = null;

                            GC.Collect();
                        }

                        // Add the padding offset to the size, to include the padding on the other ends of the drawing area.
                        bmpStageImage_ = new Bitmap(stageElementsBoundingBox.Width + paddingOffset.X, stageElementsBoundingBox.Height + paddingOffset.Y);

                        // The stage elements are in stage space coordinates, which are different that the coordinate space of the bitmap they are getting rendered to.
                        // To account for this, take the top and left of the stage element bounding box and use it as an offset when painting the bitmap.                                     
                        stageElementsOffset_.X = stageElementsBoundingBox.Left;
                        stageElementsOffset_.Y = stageElementsBoundingBox.Top;

                        Graphics gBitmap = Graphics.FromImage(bmpStageImage_);

                        gBitmap.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                        gBitmap.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                        // Render the first frame of all associated animations.
                        int animationCount = project.AnimationSlots[stateProxy_.Id].Count;

                        for (int i = 0; i < animationCount; i++)
                        {
                            AnimationSlotDto currentAnimationSlot = project.AnimationSlots[stateProxy_.Id][i];

                            Guid animationId = currentAnimationSlot.Animation;

                            if (animationId != Guid.Empty)
                            {
                                AnimationDto currentAnimation = projectController_.GetAnimation(animationId);

                                Guid spriteSheetId = currentAnimation.SpriteSheet;

                                if (spriteSheetId != Guid.Empty && project.Frames[currentAnimation.Id].Count > 0)
                                {
                                    if (project.Frames[currentAnimation.Id][0].SheetCellIndex.HasValue == true)
                                    {
                                        int spriteSheetCellIndex = project.Frames[currentAnimation.Id][0].SheetCellIndex.Value;

                                        SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(spriteSheetId);

                                        Guid bitmapResourceId = spriteSheet.BitmapResourceId;

                                        BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(bitmapResourceId, true);

                                        bitmapResource.LoadedModules ^= (byte)EditorModule.StateEditorControl;

                                        if (spriteSheetCellIndex >= 0 && spriteSheetCellIndex < bitmapResource.SpriteSheetImageList.Count)
                                        {
                                            // Get the positions.
                                            Point2D animationFrameInStageSpace = getAnimationFrameInStageSpace(currentAnimationSlot, spriteSheet);

                                            Point2D animationOriginPointInNativeAnimationSpace = getAnimationOriginPointInNativeAnimationSpace(currentAnimationSlot, spriteSheet);

                                            // Do the actual painting.

                                            // Because of the limitations of the ImageList control, larger images have to be broken into sub cells.
                                            // Iterate over the subcells and render them 
                                            for (int j = 0; j < bitmapResource.SpriteSheetImageList.ImageListRows; j++)
                                            {
                                                for (int k = 0; k < bitmapResource.SpriteSheetImageList.ImageListCols; k++)
                                                {
                                                    try
                                                    {
                                                        // Render each section of the image.
                                                        using (Image img = bitmapResource.SpriteSheetImageList.ImageLists[j][k].Images[spriteSheetCellIndex])
                                                        {
                                                            // I used to multiply by the scale factor here, but this turned out to be incorrect, because
                                                            // The subcells have already had the scaling applied.

                                                            // I also used to add the padding offset in, but this made it render 1 pixel off, so this must be already added in at some point.

                                                            //int subCellXInStageSpace = animationFrameInStageSpace.X + (k * (int)(Globals.maxImageListWidth)) + paddingOffset.X - stageElementsOffset_.X;
                                                            int subCellXInStageSpace = animationFrameInStageSpace.X + (k * (int)(Globals.maxImageListWidth)) - stageElementsOffset_.X;

                                                            //int subCellYInStageSpace = animationFrameInStageSpace.Y + (j * (int)(Globals.maxImageListWidth)) + paddingOffset.Y - stageElementsOffset_.Y;
                                                            int subCellYInStageSpace = animationFrameInStageSpace.Y + (j * (int)(Globals.maxImageListWidth)) - stageElementsOffset_.Y;

                                                            // Transform into canvas space by adding the stage top left value.
                                                            Point pt = new Point(subCellXInStageSpace, subCellYInStageSpace);

                                                            gBitmap.DrawImageUnscaled(img, pt);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        MessageBox.Show(ex.Message.ToString());
                                                    }
                                                }
                                            }

                                            // Render the outline of the selected slot.
                                            if (i == selectedAnimationSlotIndex_)
                                            {
                                                Point slotOutline = new Point(animationFrameInStageSpace.X - stageElementsOffset_.X,
                                                                              animationFrameInStageSpace.Y - stageElementsOffset_.Y);

                                                Size slotSize = new Size((int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor), (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor));

                                                gBitmap.DrawRectangle(Globals.pAnimationSlotOutline, new System.Drawing.Rectangle(slotOutline, slotSize));

                                                // The origin indicator is an X surrounded by an ellipse (or rather a circle more accurately).
                                                int xSize = 8;
                                                
                                                int ellipseX = animationFrameInStageSpace.X + animationOriginPointInNativeAnimationSpace.X - xSize - stageElementsOffset_.X;

                                                int ellipseY = animationFrameInStageSpace.Y + animationOriginPointInNativeAnimationSpace.Y - xSize - stageElementsOffset_.Y;

                                                gBitmap.DrawEllipse(Globals.pAnimationSlotOriginPoint, new RectangleF(ellipseX, ellipseY, xSize * 2, xSize * 2));

                                                // Hardcode sqrt(2). This was calculated using the the pythagorean theorem. c = sqrt(a^2 + b^2) where a==b.
                                                int xExtent = (int)(xSize / 1.41421356f);

                                                int originIndicatorX1 = animationFrameInStageSpace.X + animationOriginPointInNativeAnimationSpace.X - xExtent - stageElementsOffset_.X;

                                                int originIndicatorY1 = animationFrameInStageSpace.Y + animationOriginPointInNativeAnimationSpace.Y - xExtent - stageElementsOffset_.Y;

                                                int originIndicatorX2 = animationFrameInStageSpace.X + animationOriginPointInNativeAnimationSpace.X + xExtent - stageElementsOffset_.X;

                                                int originIndicatorY2 = animationFrameInStageSpace.Y + animationOriginPointInNativeAnimationSpace.Y + xExtent - stageElementsOffset_.Y;

                                                gBitmap.DrawLine(Globals.pAnimationSlotOriginPoint, new Point(originIndicatorX1, originIndicatorY1), new Point(originIndicatorX2, originIndicatorY2));

                                                gBitmap.DrawLine(Globals.pAnimationSlotOriginPoint, new Point(originIndicatorX1, originIndicatorY2), new Point(originIndicatorX2, originIndicatorY1));
                                            }

                                            GC.Collect();
                                        }
                                    }
                                }
                            }
                        }

                        List<HitboxDto> hitboxList = project.Hitboxes[stateProxy_.Id];

                        int hitboxCount = hitboxList.Count;

                        for (int i = 0; i < hitboxCount; i++)
                        {
                             HitboxDto currentHitbox = hitboxList[i];

                            // Convert the draw rect to corner coordinate list, and rotate it if necessary. Otherwise render a rect.
                            if (currentHitbox.RotationDegrees != 0.0f)
                            {
                                // Draw a polygon. Build the corners relative to the stage bitmap.
                                List<PointF> corners = new List<PointF>();

                                for (int j = 0; j < currentHitbox.TransformedCorners.Count; j++)
                                {
                                    float x = currentHitbox.TransformedCorners[j].X - stageElementsOffset_.X ;

                                    float y = currentHitbox.TransformedCorners[j].Y - stageElementsOffset_.Y;

                                    corners.Add(new PointF(x, y));

                                    if (j > 0)
                                    {
                                        // Add it twice to connect lines of the poly.
                                        corners.Add(new PointF(x, y));
                                    }
                                }

                                // Connect back to the first point.
                                corners.Add(new PointF(corners[0].X, corners[0].Y));


                                gBitmap.DrawPolygon(new System.Drawing.Pen(new System.Drawing.SolidBrush(Color.Blue)), corners.ToArray());

                            }
                            else
                            {
                                System.Drawing.Rectangle drawRect = new System.Drawing.Rectangle();

                                switch (stateProxy_.StageOriginLocation)
                                {
                                    case OriginLocation.TopLeft:
                                        drawRect.X = currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;

                                    case OriginLocation.TopMiddle:
                                        drawRect.X = stageHalfSize.Width - (currentHitbox.HitboxRect.Width / 2) + currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;

                                    case OriginLocation.TopRight:
                                        drawRect.X = stageWidth - currentHitbox.HitboxRect.Width + currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;

                                    case OriginLocation.MiddleLeft:
                                        drawRect.X = currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = stageHalfSize.Height - (currentHitbox.HitboxRect.Height / 2) + currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;

                                    case OriginLocation.Center:
                                        drawRect.X = stageHalfSize.Width - (currentHitbox.HitboxRect.Width / 2) + currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = stageHalfSize.Height - (currentHitbox.HitboxRect.Height / 2) + currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;

                                    case OriginLocation.MiddleRight:
                                        drawRect.X = stageWidth - currentHitbox.HitboxRect.Width + currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = stageHalfSize.Height - (currentHitbox.HitboxRect.Height / 2) + currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;

                                    case OriginLocation.BottomLeft:
                                        drawRect.X = currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = stageHeight + currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;

                                    case OriginLocation.BottomMiddle:
                                        drawRect.X = stageHalfSize.Width - (currentHitbox.HitboxRect.Width / 2) + currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = stageHeight + currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;

                                    case OriginLocation.BottomRight:
                                        drawRect.X = stageWidth - currentHitbox.HitboxRect.Width + currentHitbox.HitboxRect.Left - stageElementsOffset_.X;
                                        drawRect.Y = stageHeight + currentHitbox.HitboxRect.Top - stageElementsOffset_.Y;
                                        break;
                                }

                                drawRect.Height = currentHitbox.HitboxRect.Height;
                                drawRect.Width = currentHitbox.HitboxRect.Width;

                                gBitmap.DrawRectangle(new System.Drawing.Pen(new System.Drawing.SolidBrush(Color.Blue)), drawRect);
                            }
                        }

                        gBitmap.Dispose();
                    }
                }

                if (bmpStageImage_ != null)
                {
                    // Remember to remove the padding offset to ensure that the stage elements bitmap aligns correctly with the stage
                    // Update - the animation frames are 1 pixel off. I think I don't want to remove the padding?
                    //int drawAtX = stageTopLeftInCanvasSpace.X - paddingOffset.X + stageElementsOffset_.X + (-1 * hsState.Value);
                    int drawAtX = stageTopLeftInCanvasSpace.X + stageElementsOffset_.X + (-1 * hsState.Value);

                    //int drawAtY = stageTopLeftInCanvasSpace.Y - paddingOffset.Y + stageElementsOffset_.Y + (-1 * vsState.Value);
                    int drawAtY = stageTopLeftInCanvasSpace.Y + stageElementsOffset_.Y + (-1 * vsState.Value);

                    g.DrawImage(bmpStageImage_, drawAtX, drawAtY);
                }

                // Draw the overlay
                if (stageWidth > 0 && stageHeight > 0)
                {
                    bmpStageOverlay_ = overlayGenerator_.GenerateOverlay(stageWidth, stageHeight, stageOriginPointInNativeStageSpace_);

                    try
                    {
                        g.DrawImageUnscaled(bmpStageOverlay_, new Point(stageTopLeftInCanvasSpace.X + (-1 * hsState.Value), stageTopLeftInCanvasSpace.Y + (-1 * vsState.Value)));
                    }
                    catch (Exception ex)
                    {
                        exceptionHandler_.HandleException(ex);
                    }

                }
            }
        }

        private void resize(bool refresh)
        {
            hsState.Top = this.ClientSize.Height - hsState.Height;
            hsState.Width = this.ClientSize.Width - vsState.Width;

            vsState.Left = this.ClientSize.Width - vsState.Width;
            vsState.Height = this.ClientSize.Height - hsState.Height;

            pbState.Width = this.Width - vsState.Width - 1;
            pbState.Height = this.Height - hsState.Height - 1;

            int vScrollMax = stateProxy_.StageHeight - pbState.Height;

            if (vScrollMax > 0)
            {
                vsState.Maximum = vScrollMax;
            }
            else
            {
                vsState.Maximum = vsState.Minimum;
            }

            int hScrollMax = stateProxy_.StageWidth - pbState.Width;

            if (hScrollMax > 0)
            {
                hsState.Maximum = hScrollMax;
            }
            else
            {
                hsState.Maximum = hsState.Minimum;
            }

            if (refresh == true)
            {
                pbState.Refresh();
            }
        }

        #endregion

        #region Event Handlers

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkToolStripMenuItem.Checked = true;

            lightToolStripMenuItem.Checked = false;

            vividToolStripMenuItem.Checked = false;

            backgroundGenerator_.BackgroundColorScheme = BackgroundColorScheme.StandardDark;

            backgroundGenerator_.Regenerate();

            pbState.Refresh();
        }

        private void hsState_Scroll(object sender, ScrollEventArgs e)
        {
            pbState.Refresh();
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkToolStripMenuItem.Checked = false;

            lightToolStripMenuItem.Checked = true;

            vividToolStripMenuItem.Checked = false;

            backgroundGenerator_.BackgroundColorScheme = BackgroundColorScheme.StandardLight;

            backgroundGenerator_.Regenerate();

            pbState.Refresh();
        }

        private void pbState_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmnuConfig.Show(Cursor.Position);
            }
        }

        private void pbState_Paint(object sender, PaintEventArgs e)
        {
            paint(e.Graphics);
        }
        
        private void StateEditorControl_Resize(object sender, System.EventArgs e)
        {
            resize(true);
        }

        private void vividToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkToolStripMenuItem.Checked = false;

            lightToolStripMenuItem.Checked = false;

            vividToolStripMenuItem.Checked = true;

            backgroundGenerator_.BackgroundColorScheme = BackgroundColorScheme.Vivid;

            backgroundGenerator_.Regenerate();

            pbState.Refresh();
        }

        private void vsState_Scroll(object sender, ScrollEventArgs e)
        {
            pbState.Refresh();
        }

        #endregion

    }
}
