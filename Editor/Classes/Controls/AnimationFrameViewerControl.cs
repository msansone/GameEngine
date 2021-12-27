using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class AnimationFrameViewerControl : UserControl, IAnimationFrameViewerControl
    {
        #region Constructors

        public AnimationFrameViewerControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            utilityFactory_ = new UtilityFactory();

            drawingUtility_ = utilityFactory_.NewDrawingUtility();

            backgroundGenerator_ = firemelonEditorFactory_.NewBackgroundGenerator();

            vsAnimation.SmallChange = 1;
            vsAnimation.LargeChange = 1;

            hsAnimation.SmallChange = 1;
            hsAnimation.LargeChange = 1;
            
            hsAnimation.Maximum = 0;
            vsAnimation.Maximum = 0;

            renderOnlyActionPointLocation_ = new Point2D(0, 0);
            renderOnlyHitboxPoint1_ = new Point2D(0, 0);
            renderOnlyHitboxPoint2_ = new Point2D(0, 0);
            
            isDraggingActionPoint_ = false;

            isDrawingHitbox_ = false;

            isMouseDown_ = false;

            //allowAddHitboxes_ = allowAddHitboxes;

            //if (allowAddHitboxes_ == true)
            //{
            //    addHitboxToolStripMenuItem1.Visible = true;
            //}
            //else
            //{
            //    addHitboxToolStripMenuItem1.Visible = false;
            //}

            selectedFrameTriggerIndex_ = -1;

            selectedActionPointIndex_ = -1;       
        }

        #endregion

        #region Private Variables
        
        private IAnimationDtoProxy animationProxy_;

        private IBackgroundGenerator backgroundGenerator_;

        private BitmapResourceDto bitmapResource_;
        
        private IDrawingUtility drawingUtility_;

        private IFiremelonEditorFactory firemelonEditorFactory_;
        
        private bool isDraggingActionPoint_;

        private bool isDrawingHitbox_;

        private bool isMouseDown_;

        private IProjectController projectController_;

        private Point2D renderOnlyActionPointLocation_;

        private Point2D renderOnlyHitboxPoint1_;

        private Point2D renderOnlyHitboxPoint2_;
        
        private int selectedFrameTriggerIndex_;

        private SpriteSheetDto spriteSheet_;
        
        private IUtilityFactory utilityFactory_;

        #endregion

        #region Properties

        public int ActionPointIndex
        {
            get { return selectedActionPointIndex_; }
            set { selectedActionPointIndex_ = value; }
        }
        private int selectedActionPointIndex_ = -1;


        public IAnimationDtoProxy Animation
        {
            get
            {
                return animationProxy_;
            }
            set
            {
                animationProxy_ = value;

                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();
                ProjectDto project = projectController_.GetProjectDto();

                if (animationProxy_.SpriteSheetId != Guid.Empty)
                {
                    spriteSheet_ = projectController_.GetSpriteSheet(animationProxy_.SpriteSheetId);

                    Guid bitmapResourceId = spriteSheet_.BitmapResourceId;

                    // Separate resources dto removed in 2.2 format.
                    //bitmapResource_ = resources.Bitmaps[bitmapResourceId];
                    bitmapResource_ = projectController_.GetBitmapResource(bitmapResourceId, false);
                }

                // This created a weird effect, because the animation and frame are set separately, rather than as one transaction.
                // So there is kind of a blip. Let the caller do the refresh manually. I'm leaving this here so I don't make the same 
                // mistake again later, after I've forgotten this.
                //RefreshImage();

                backgroundGenerator_.Regenerate();
            }
        }

        public int FrameIndex
        {
            get
            {
                return selectedFrameIndex_;
            }
            set
            {
                selectedFrameIndex_ = value;

                // This created a weird effect, because the animation and frame are set separately, rather than as one transaction.
                // So there is kind of a blip. Let the caller do the refresh manually. I'm leaving this here so I don't make the same 
                // mistake again later, after I've forgotten this.
                //RefreshImage();
            }
        }  
        private int selectedFrameIndex_ = -1;

        public int HitboxIndex
        {
            get { return selectedHitboxIndex_; }
            set { selectedHitboxIndex_ = value; }
        }
        private int selectedHitboxIndex_ = -1;

        #endregion

        #region Public Functions

        public void RefreshImage()
        {
            resize();

            pbCurrentFrame.Refresh();
        }

        #endregion

        #region Private Functions

        private void mouseDown(MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            if (animationProxy_.SpriteSheetId != Guid.Empty)
            {   
                int sheetImageCount = bitmapResource_.SpriteSheetImageList.Count;

                int animationIndex = projectController_.GetAnimationIndexFromId(animationProxy_.Id);

                if (selectedFrameIndex_ >= 0 && selectedFrameIndex_ < project.Frames[animationProxy_.Id].Count)
                {
                    FrameDto frame = project.Frames[animationProxy_.Id][selectedFrameIndex_];

                    if (frame.SheetCellIndex.HasValue == true)
                    {
                        int sheetCellIndex = frame.SheetCellIndex.Value;

                        if (sheetCellIndex < sheetImageCount)
                        {
                            if ((ModifierKeys & Keys.Control) == Keys.Control)
                            {
                                isMouseDown_ = true;

                                if (selectedHitboxIndex_ >= 0)
                                {
                                    // Set the first corner point of the current hitbox.
                                    renderOnlyHitboxPoint1_.X = e.X;

                                    renderOnlyHitboxPoint1_.Y = e.Y;

                                    renderOnlyHitboxPoint2_.X = e.X;

                                    renderOnlyHitboxPoint2_.Y = e.Y;

                                    isDrawingHitbox_ = true;
                                }
                                else if (selectedActionPointIndex_ >= 0)
                                {
                                    renderOnlyActionPointLocation_.X = e.X;

                                    renderOnlyActionPointLocation_.Y = e.Y;

                                    isDraggingActionPoint_ = true;
                                }

                                pbCurrentFrame.Refresh();
                            }
                        }
                    }
                }
            }
        }

        private void mouseMove(MouseEventArgs e)
        {
            //findmetodo This is wrong, it shouldn't be updating in the mouse move, only in the mouse up. The mouse
            // up is the final step in the UI transaction. When done in the move it's going to explode the undo stack.
            if (isMouseDown_ == true)
            {
                if (selectedHitboxIndex_ >= 0 && isDrawingHitbox_ == true)
                {
                    renderOnlyHitboxPoint2_.X = e.X;

                    renderOnlyHitboxPoint2_.Y = e.Y;

                    pbCurrentFrame.Refresh();
                }
                else if (selectedActionPointIndex_ >= 0 && isDraggingActionPoint_ == true)
                {
                    renderOnlyActionPointLocation_.X = e.X;

                    renderOnlyActionPointLocation_.Y = e.Y;

                    pbCurrentFrame.Refresh();
                }
            }
        }

        private void mouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                cmnuConfig.Show(Cursor.Position);
            }
            else
            {
                if (isMouseDown_ == true &&
                (selectedHitboxIndex_ >= 0 || selectedActionPointIndex_ >= 0) &&
                selectedFrameIndex_ >= 0)
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    FrameDto frame = project.Frames[animationProxy_.Id][selectedFrameIndex_];

                    if (selectedHitboxIndex_ >= 0)
                    {
                        // Set the second corner point of the current hitbox.

                        projectController_.SetHitboxCornerPoints(frame.Id, selectedHitboxIndex_, renderOnlyHitboxPoint1_.X, renderOnlyHitboxPoint1_.Y, renderOnlyHitboxPoint2_.X, renderOnlyHitboxPoint2_.Y);

                        isDrawingHitbox_ = false;
                    }
                    else if (selectedActionPointIndex_ >= 0)
                    {
                        ActionPointDto actionPoint = project.ActionPoints[frame.Id][selectedActionPointIndex_];

                        // Set the action point position.
                        projectController_.SetActionPointPosition(actionPoint.Id, renderOnlyActionPointLocation_);

                        isDraggingActionPoint_ = false;
                    }

                    pbCurrentFrame.Refresh();

                    //pgAnimation.Refresh();
                }

                isMouseDown_ = false;
            }
        }

        private void paint(Graphics g)
        {
            // Don't draw if no animation is set, or the animation doesn't have a sprite sheet set.
            if (animationProxy_ == null)
            {
                return;
            }

            Guid spriteSheetId = animationProxy_.SpriteSheetId;

            if (spriteSheetId == Guid.Empty)
            {
                return;
            }
                         
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            ProjectDto project = projectController_.GetProjectDto();

            if (bitmapResource_.BitmapImage == null)
            {
                projectController_.LoadBitmap(bitmapResource_.Id);

                bitmapResource_.LoadedModules ^= (byte)EditorModule.AnimationFrameViewer;
            }

            int sheetImageCount = bitmapResource_.SpriteSheetImageList.Count;

            if (selectedFrameIndex_ >= 0)
            {
                   
                int imageCount = bitmapResource_.SpriteSheetImageList.Count;

                if (imageCount > 0)
                {
                    int frameHeight = (int)(spriteSheet_.CellHeight * spriteSheet_.ScaleFactor);

                    int frameWidth = (int)(spriteSheet_.CellWidth * spriteSheet_.ScaleFactor);

                    Bitmap bmpBackground = backgroundGenerator_.GenerateBackground(frameWidth, frameHeight);

                    g.DrawImageUnscaled(bmpBackground, new Point(0, 0));
                }                    
                               
                // Render the current frame of the animation.
                Point pt = new Point(0, 0);

                if (selectedFrameIndex_ < project.Frames[animationProxy_.Id].Count)
                {
                    FrameDto frame = project.Frames[animationProxy_.Id][selectedFrameIndex_];

                    if (frame.SheetCellIndex.HasValue == true)
                    {
                        int sheetCellIndex = frame.SheetCellIndex.Value;

                        if (sheetCellIndex < sheetImageCount)
                        {
                            int cellX = -1 * hsAnimation.Value;
                            int cellY = -1 * vsAnimation.Value;

                            for (int i = 0; i < bitmapResource_.SpriteSheetImageList.ImageListRows; i++)
                            {
                                for (int j = 0; j < bitmapResource_.SpriteSheetImageList.ImageListCols; j++)
                                {
                                    using (Image img = bitmapResource_.SpriteSheetImageList.ImageLists[i][j].Images[sheetCellIndex])
                                    {
                                        int subCellX = cellX + (j * Globals.maxImageListWidth);

                                        int subCellY = cellY + (i * Globals.maxImageListHeight);

                                        g.DrawImageUnscaled(img, new Point(subCellX, subCellY));
                                    }
                                }
                            }
                        }
                    }

                    // Render the hitboxes associated with the current frame.
                    List<HitboxDto> hitboxList = project.Hitboxes[frame.Id];

                    int hitboxCount = hitboxList.Count;

                    for (int i = 0; i < hitboxCount; i++)
                    {
                        HitboxDto currentHitbox = hitboxList[i];

                        System.Drawing.Rectangle drawRect;

                        if (i == selectedHitboxIndex_ && isDrawingHitbox_ == true)
                        {
                            drawRect = drawingUtility_.GetRectFromPoints(renderOnlyHitboxPoint1_, renderOnlyHitboxPoint2_);
                        }
                        else
                        {
                            drawRect = new System.Drawing.Rectangle();
                            drawRect.X = currentHitbox.HitboxRect.Left + (-1 * hsAnimation.Value);
                            drawRect.Y = currentHitbox.HitboxRect.Top + (-1 * vsAnimation.Value);
                            drawRect.Height = currentHitbox.HitboxRect.Height;
                            drawRect.Width = currentHitbox.HitboxRect.Width;
                        }

                        g.DrawRectangle(new System.Drawing.Pen(new System.Drawing.SolidBrush(Color.Blue)), drawRect);
                    }

                    // Render the action points associated with the current frame. 
                    List<ActionPointDto> actionPointList = project.ActionPoints[frame.Id];

                    int actionPointCount = actionPointList.Count;

                    for (int i = 0; i < actionPointCount; i++)
                    {
                        ActionPointDto currentActionPoint = actionPointList[i];

                        System.Drawing.Rectangle drawRect = new System.Drawing.Rectangle();

                        if (i == selectedActionPointIndex_ && isDraggingActionPoint_ == true)
                        {
                            drawRect.X = renderOnlyActionPointLocation_.X + (-1 * hsAnimation.Value);
                            drawRect.Y = renderOnlyActionPointLocation_.Y + (-1 * vsAnimation.Value);
                        }
                        else
                        {
                            drawRect.X = currentActionPoint.Position.X + (-1 * hsAnimation.Value);
                            drawRect.Y = currentActionPoint.Position.Y + (-1 * vsAnimation.Value);                            
                        }

                        drawRect.Height = 4;
                        drawRect.Width = 4;

                        g.FillEllipse(new System.Drawing.SolidBrush(Color.White), drawRect);
                        g.DrawEllipse(new System.Drawing.Pen(new System.Drawing.SolidBrush(Color.Red)), drawRect);
                    }
                }

                GC.Collect();                
            }
        }

        private void resize()
        {
            hsAnimation.Top = this.ClientSize.Height - hsAnimation.Height;
            hsAnimation.Width = this.ClientSize.Width - vsAnimation.Width;

            vsAnimation.Left = this.ClientSize.Width - vsAnimation.Width;
            vsAnimation.Height = this.ClientSize.Height - hsAnimation.Height;

            pbCurrentFrame.Height = this.ClientSize.Height - hsAnimation.Height;
            pbCurrentFrame.Width = this.ClientSize.Width - vsAnimation.Width;

            setCurrentFrameScrollbarValues();

            pbCurrentFrame.Refresh();
        }

        private void setCurrentFrameScrollbarValues()
        {
            if (spriteSheet_ != null)
            {
                int vScrollMax = (int)(spriteSheet_.CellHeight * spriteSheet_.ScaleFactor) - pbCurrentFrame.Height;

                if (vScrollMax > 0)
                {
                    vsAnimation.Maximum = vScrollMax;
                }
                else
                {
                    vsAnimation.Maximum = vsAnimation.Minimum;
                }

                int hScrollMax = (int)(spriteSheet_.CellWidth * spriteSheet_.ScaleFactor) - pbCurrentFrame.Width;

                if (hScrollMax > 0)
                {
                    hsAnimation.Maximum = hScrollMax;
                }
                else
                {
                    hsAnimation.Maximum = hsAnimation.Minimum;
                }
            }
        }

        #endregion

        #region Event Handlers

        private void AnimationFrameViewerControl_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkToolStripMenuItem.Checked = true;

            lightToolStripMenuItem.Checked = false;

            vividToolStripMenuItem.Checked = false;

            backgroundGenerator_.BackgroundColorScheme = BackgroundColorScheme.StandardDark;

            backgroundGenerator_.Regenerate();

            pbCurrentFrame.Refresh();
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkToolStripMenuItem.Checked = false;

            lightToolStripMenuItem.Checked = true;

            vividToolStripMenuItem.Checked = false;

            backgroundGenerator_.BackgroundColorScheme = BackgroundColorScheme.StandardLight;

            backgroundGenerator_.Regenerate();

            pbCurrentFrame.Refresh();
        }

        private void pbCurrentFrame_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown(e);
        }
        
        private void pbCurrentFrame_MouseMove(object sender, MouseEventArgs e)
        {
            mouseMove(e);
        }

        private void pbCurrentFrame_MouseUp(object sender, MouseEventArgs e)
        {
            mouseUp(e);
        }

        private void pbCurrentFrame_Paint(object sender, PaintEventArgs e)
        {
            paint(e.Graphics);
        }

        private void vividToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkToolStripMenuItem.Checked = false;

            lightToolStripMenuItem.Checked = false;

            vividToolStripMenuItem.Checked = true;

            backgroundGenerator_.BackgroundColorScheme = BackgroundColorScheme.Vivid;

            backgroundGenerator_.Regenerate();

            pbCurrentFrame.Refresh();
        }

        #endregion
    }
}
