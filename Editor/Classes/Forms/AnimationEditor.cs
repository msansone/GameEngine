/* -------------------------------------------------------------------------
** AnimationEditor.cs
**
** The AnimationEditor form provides an interface to view and edit the
** the properties and child objects of an animation. After choosing a
** sprite sheet to point to, the frames from that sheet will be displayed
** and can be selected as frames in the animation. Each frame can also contain
** hitboxes and triggers.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class AnimationEditor : Form, IAnimationEditor
    {
        private IAnimationDtoProxy animationProxy_;
        private IFiremelonEditorFactory firemelonEditorFactory_;
        private IProjectController projectController_;
        private IBackgroundGenerator backgroundGenerator_;

        SpriteSheetDto spriteSheet_;
        BitmapResourceDto bitmapResource_;

        // Used for background rendering.
        private Size szBounds_;
        private Size szBuffer_;
        private Bitmap bmpBackground_;
        private bool generateBackgroundImage_;

        private bool isMouseDown_;

        // Only animations owned by actors can define hitboxes.
        private bool allowAddHitboxes_;

        private int selectedFrameIndex_;
        private int selectedHitboxIndex_;
        private int selectedFrameTriggerIndex_;
        private int selectedActionPointIndex_;

        public AnimationEditor()
        {
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            backgroundGenerator_ = firemelonEditorFactory_.NewBackgroundGenerator();

            szBuffer_ = new Size(200, 200);
            szBounds_ = new Size(0, 0);

            hsAnimation.SmallChange = 1;
            hsAnimation.LargeChange = 1;
            vsAnimation.SmallChange = 1;
            vsAnimation.LargeChange = 1;
            hsAnimation.Maximum = 0;
            vsAnimation.Maximum = 0;
            
            // Tree right click behaves strangely without this.
            tvAnimation.NodeMouseClick += (sender, args) => tvAnimation.SelectedNode = args.Node;
        }

        public void ShowDialog(IWin32Window owner, IProjectController projectController, IAnimationDtoProxy animationProxy, bool allowAddHitboxes)
        {
            animationProxy_ = animationProxy;
            projectController_ = projectController;

            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();
            ProjectDto project = projectController_.GetProjectDto();

            Guid spriteSheetId = animationProxy_.SpriteSheetId;

            if (spriteSheetId != Guid.Empty)
            {
                spriteSheet_ = projectController_.GetSpriteSheet(spriteSheetId);

                Guid bitmapResourceId = spriteSheet_.BitmapResourceId;

                // Separate resources dto removed in 2.2 format.
                //bitmapResource_ = resources.Bitmaps[bitmapResourceId];
                bitmapResource_ = projectController_.GetBitmapResource(bitmapResourceId, true);

                // findmebitmap Is this still used? If so, I need to free the bitmap resource when the dialog is closed.
            }

            generateBackgroundImage_ = true;

            isMouseDown_ = false;

            allowAddHitboxes_ = allowAddHitboxes;

            if (allowAddHitboxes_ == true)
            {
                addHitboxToolStripMenuItem1.Visible = true;
            }
            else
            {
                addHitboxToolStripMenuItem1.Visible = false;
            }

            selectedFrameIndex_ = -1;
            selectedHitboxIndex_ = -1;
            selectedFrameTriggerIndex_ = -1;
            selectedActionPointIndex_ = -1;

            // These will be regenerated as needed, so there's no point creating them yet.
            bmpBackground_ = null;
            
            buildTreeView();
            
            pgAnimation.SelectedObject = animationProxy_;

            base.ShowDialog(owner);
        }
        
        private void AnimationEditor_Load(object sender, EventArgs e)
        {
            resizeCurrentFramePanel();
        }

        private void buildTreeView()
        {
            ProjectDto project = projectController_.GetProjectDto();

            tvAnimation.Nodes.Clear();

            TreeNode tempNode;

            tempNode = tvAnimation.Nodes.Add("FRAMEROOT", "Animation Frames");
            
            tempNode.Tag = new AssetMenuDto(cmnuAnimationRoot, animationProxy_);

            int frameCounter = 1;

            foreach (FrameDto frame in project.Frames[animationProxy_.Id])
            {
                tempNode = tvAnimation.Nodes["FRAMEROOT"].Nodes.Add("FRAME", "Frame " + frameCounter.ToString());

                IFrameDtoProxy frameProxy = firemelonEditorFactory_.NewFrameProxy(projectController_, frame.Id);

                tempNode.Tag = new AssetMenuDto(cmnuFrame, frameProxy);

                int hitboxCounter = 1;

                foreach (HitboxDto hitbox in project.Hitboxes[frame.Id])
                {
                    IHitboxDtoProxy hitboxProxy = firemelonEditorFactory_.NewHitboxProxy(projectController_, hitbox.Id);

                    string nodeName = "Hitbox " + hitboxCounter.ToString();
                    string nodeKey = "HITBOX";

                    TreeNode node;

                    if (hitboxCounter == 1)
                    {
                        node = tvAnimation.Nodes["FRAMEROOT"]
                               .Nodes[frameCounter - 1]
                               .Nodes.Add("HITBOXROOT", "Hitboxes");

                        node.Tag = new AssetMenuDto(cmnuHitboxRoot, null);
                    }
                    else
                    {
                        node = tvAnimation.Nodes["FRAMEROOT"]
                               .Nodes[frameCounter - 1]
                               .Nodes["HITBOXROOT"];
                    }

                    node.Nodes.Add(nodeKey, nodeName).Tag = new AssetMenuDto(cmnuHitbox, hitboxProxy);

                    hitboxCounter += 1;
                }

                int frameTriggerCounter = 1;

                foreach (FrameTriggerDto frameTrigger in project.FrameTriggers[frame.Id])
                {
                    IFrameTriggerDtoProxy frameTriggerProxy = firemelonEditorFactory_.NewFrameTriggerProxy(projectController_, frameTrigger.Id);

                    string nodeName = "Frame Trigger " + frameTriggerCounter.ToString();
                    string nodeKey = "FRAMETRIGGER";

                    TreeNode node;

                    if (frameTriggerCounter == 1)
                    {
                        node = tvAnimation.Nodes["FRAMEROOT"]
                               .Nodes[frameCounter - 1]
                               .Nodes.Add("FRAMETRIGGERROOT", "Frame Triggers");

                        node.Tag = new AssetMenuDto(cmnuFrameTriggerRoot, null);
                    }
                    else
                    {
                        node = tvAnimation.Nodes["FRAMEROOT"]
                               .Nodes[frameCounter - 1]
                               .Nodes["FRAMETRIGGERROOT"];
                    }

                    node.Nodes.Add(nodeKey, nodeName).Tag = new AssetMenuDto(cmnuFrameTrigger, frameTriggerProxy);

                    frameTriggerCounter += 1;
                }

                int actionPointCounter = 1;

                foreach (ActionPointDto actionPoint in project.ActionPoints[frame.Id])
                {
                    IActionPointDtoProxy actionPointProxy = firemelonEditorFactory_.NewActionPointProxy(projectController_, actionPoint.Id);

                    string nodeName = actionPoint.Name;
                    string nodeKey = "ACTIONPOINT";

                    TreeNode node;

                    if (actionPointCounter == 1)
                    {
                        node = tvAnimation.Nodes["FRAMEROOT"]
                               .Nodes[frameCounter - 1]
                               .Nodes.Add("ACTIONPOINTROOT", "Action Points");

                        node.Tag = new AssetMenuDto(cmnuActionPointRoot, null);
                    }
                    else
                    {
                        node = tvAnimation.Nodes["FRAMEROOT"]
                               .Nodes[frameCounter - 1]
                               .Nodes["ACTIONPOINTROOT"];
                    }

                    node.Nodes.Add(nodeKey, nodeName).Tag = new AssetMenuDto(cmnuActionPoint, actionPointProxy);

                    actionPointCounter += 1;
                }

                frameCounter += 1;
            }
        }

        private void resizeCurrentFramePanel()
        {
            hsAnimation.Top = pnAnimation.ClientSize.Height - hsAnimation.Height;
            hsAnimation.Width = pnAnimation.ClientSize.Width - vsAnimation.Width;

            vsAnimation.Left = pnAnimation.ClientSize.Width - vsAnimation.Width;
            vsAnimation.Height = pnAnimation.ClientSize.Height - hsAnimation.Height;

            pbCurrentFrame.Height = pnAnimation.ClientSize.Height - hsAnimation.Height;
            pbCurrentFrame.Width = pnAnimation.ClientSize.Width - vsAnimation.Width;

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

        private void AnimationEditor_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void resize()
        {
            resizeCurrentFramePanel();
        }

        private void pgAnimation_Click(object sender, EventArgs e)
        {

        }

        private void pgAnimation_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":

                    if (tvAnimation.SelectedNode.Name.ToUpper() == "ACTIONPOINT")
                    {
                        tvAnimation.SelectedNode.Text = e.ChangedItem.Value.ToString();
                    }

                    break;

                case "SPRITESHEET":

                    // Reset the sprite sheet cell images.                                        
                    spriteSheet_ = projectController_.GetSpriteSheet(animationProxy_.SpriteSheetId);

                    // Separate resources dto removed in 2.2 format.
                    //ProjectResourcesDto resources = projectController_.GetResources();

                    ProjectDto project = projectController_.GetProjectDto();

                    Guid bitmapResourceId = spriteSheet_.BitmapResourceId;

                    // Separate resources dto removed in 2.2 format.
                    //bitmapResource_ = resources.Bitmaps[bitmapResourceId];
                    bitmapResource_ = projectController_ .GetBitmapResource(bitmapResourceId, false);
                    
                    //findmebitmap remember to unload this resource when closing the form (unless it is used by the room). What if this gets overwritten so more than one bitmap got loaned/rendered?
                    // need some sort of way to track where bitmaps were loaded.

                    generateBackgroundImage_ = true;
                    
                    resizeCurrentFramePanel();
                    
                    break;
            }

            pbCurrentFrame.Refresh();
        }
        
        private void tvAnimation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvAnimation.SelectedNode.Name == "FRAME")
            {
                selectedFrameIndex_ = tvAnimation.SelectedNode.Index;
                selectedHitboxIndex_ = -1;

                IFrameDtoProxy frameProxy = (IFrameDtoProxy)((AssetMenuDto)(tvAnimation.SelectedNode.Tag)).Asset;

                pgAnimation.SelectedObject = frameProxy;
            }
            else if (tvAnimation.SelectedNode.Name == "HITBOXROOT" ||
                     tvAnimation.SelectedNode.Name == "FRAMETRIGGERROOT" ||
                     tvAnimation.SelectedNode.Name == "ACTIONPOINTROOT")
            {
                selectedFrameIndex_ = tvAnimation.SelectedNode.Parent.Index;
                selectedHitboxIndex_ = -1;
                selectedFrameTriggerIndex_ = -1;
                selectedActionPointIndex_ = -1;
                
                pgAnimation.SelectedObject = animationProxy_;
            }
            else if (tvAnimation.SelectedNode.Name == "HITBOX")
            {
                selectedFrameIndex_ = tvAnimation.SelectedNode.Parent.Parent.Index;
                selectedHitboxIndex_ = tvAnimation.SelectedNode.Index;
                selectedFrameTriggerIndex_ = -1;
                selectedActionPointIndex_ = -1;

                IHitboxDtoProxy hitboxProxy = (IHitboxDtoProxy)((AssetMenuDto)(tvAnimation.SelectedNode.Tag)).Asset;

                pgAnimation.SelectedObject = hitboxProxy;
            }
            else if (tvAnimation.SelectedNode.Name == "FRAMETRIGGER")
            {
                selectedFrameIndex_ = tvAnimation.SelectedNode.Parent.Parent.Index;
                selectedHitboxIndex_ = -1;
                selectedFrameTriggerIndex_ = tvAnimation.SelectedNode.Index;
                selectedActionPointIndex_ = -1;

                IFrameTriggerDtoProxy frameTriggerProxy = (IFrameTriggerDtoProxy)((AssetMenuDto)(tvAnimation.SelectedNode.Tag)).Asset;

                pgAnimation.SelectedObject = frameTriggerProxy;
            }
            else if (tvAnimation.SelectedNode.Name == "ACTIONPOINT")
            {
                selectedFrameIndex_ = tvAnimation.SelectedNode.Parent.Parent.Index;
                selectedHitboxIndex_ = -1;
                selectedFrameTriggerIndex_ = -1;
                selectedActionPointIndex_ = tvAnimation.SelectedNode.Index;

                IActionPointDtoProxy actionPointProxy = (IActionPointDtoProxy)((AssetMenuDto)(tvAnimation.SelectedNode.Tag)).Asset;

                pgAnimation.SelectedObject = actionPointProxy;
            }
            else
            {
                selectedFrameIndex_ = -1;
                selectedFrameTriggerIndex_ = -1;
                selectedActionPointIndex_ = -1;
                selectedHitboxIndex_ = -1;

                pgAnimation.SelectedObject = animationProxy_;
            }

            pbCurrentFrame.Refresh();
        }

        private void pbCurrentFrame_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            ProjectDto project = projectController_.GetProjectDto();

            if (animationProxy_.SpriteSheetId != Guid.Empty)
            {
                int sheetImageCount = bitmapResource_.SpriteSheetImageList.Count;

                if (selectedFrameIndex_ >= 0)
                {
                    if (generateBackgroundImage_ == true || bmpBackground_ == null)
                    {
                        // Dispose of the old background object.
                        if (bmpBackground_ != null)
                        {
                            bmpBackground_.Dispose();
                            bmpBackground_ = null;
                        }
                        
                        int imageCount = bitmapResource_.SpriteSheetImageList.Count;

                        if (imageCount > 0)
                        {
                            int frameHeight = (int)(spriteSheet_.CellHeight * spriteSheet_.ScaleFactor);

                            int frameWidth = (int)(spriteSheet_.CellWidth * spriteSheet_.ScaleFactor);

                            bmpBackground_ = backgroundGenerator_.GenerateBackground(frameWidth, frameHeight);
                        }
                        
                        generateBackgroundImage_ = false;                        
                    }

                    g.DrawImageUnscaled(bmpBackground_, new Point(0, 0));

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
                    }

                    GC.Collect();

                    if (selectedFrameIndex_ > -1)
                    {
                        // Render the hitboxes associated with the current frame.
                        FrameDto frame = project.Frames[animationProxy_.Id][selectedFrameIndex_];

                        List<HitboxDto> hitboxList = project.Hitboxes[frame.Id];
                        
                        int hitboxCount = hitboxList.Count;

                        for (int i = 0; i < hitboxCount; i++)
                        {
                            HitboxDto currentHitbox = hitboxList[i];

                            System.Drawing.Rectangle drawRect = new System.Drawing.Rectangle();
                            drawRect.X = currentHitbox.HitboxRect.Left + (-1 * hsAnimation.Value);
                            drawRect.Y = currentHitbox.HitboxRect.Top + (-1 * vsAnimation.Value);
                            drawRect.Height = currentHitbox.HitboxRect.Height;
                            drawRect.Width = currentHitbox.HitboxRect.Width;

                            g.DrawRectangle(new System.Drawing.Pen(new System.Drawing.SolidBrush(Color.Blue)), drawRect);                            
                        }
                    
                        // Render the action points associated with the current frame. 
                        List<ActionPointDto> actionPointList = project.ActionPoints[frame.Id];

                        int actionPointCount = actionPointList.Count;

                        for (int i = 0; i < actionPointCount; i++)
                        {
                            ActionPointDto currentActionPoint = actionPointList[i];

                            System.Drawing.Rectangle drawRect = new System.Drawing.Rectangle();
                            drawRect.X = currentActionPoint.Position.X + (-1 * hsAnimation.Value);
                            drawRect.Y = currentActionPoint.Position.Y + (-1 * vsAnimation.Value);
                            drawRect.Height = 4;
                            drawRect.Width = 4;

                            g.FillEllipse(new System.Drawing.SolidBrush(Color.White), drawRect);
                            g.DrawEllipse(new System.Drawing.Pen(new System.Drawing.SolidBrush(Color.Red)), drawRect);
                        }
                    }  
                }
            }
        }

        private void pbCurrentFrame_MouseDown(object sender, MouseEventArgs e)
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
                                    projectController_.SetHitboxCornerPoint1(frame.Id, selectedHitboxIndex_, e.X, e.Y);
                                }

                                pbCurrentFrame.Refresh();
                            }
                        }
                    }
                }
            }
        }

        private void pbCurrentFrame_MouseMove(object sender, MouseEventArgs e)
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
                    projectController_.SetHitboxCornerPoint2(frame.Id, selectedHitboxIndex_, e.X, e.Y);
                }
                else if (selectedActionPointIndex_ >= 0)
                {
                    ActionPointDto actionPoint = project.ActionPoints[frame.Id][selectedActionPointIndex_];

                    // Set the action point position.
                    projectController_.SetActionPointPositionLeft(actionPoint.Id, e.X);
                    projectController_.SetActionPointPositionTop(actionPoint.Id, e.Y);
                }

                pbCurrentFrame.Refresh();

                pgAnimation.Refresh();
            }
        }

        private void pbCurrentFrame_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown_ = false;
        }

        private void addHitboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addHitbox();
        }

        private void addHitbox()
        {
            ProjectDto project = projectController_.GetProjectDto();

            FrameDto frame = project.Frames[animationProxy_.Id][selectedFrameIndex_];

            HitboxDto newHitbox = projectController_.AddHitbox(frame.Id, frame.RootOwnerId);

            // Add a hitbox node to the tree and select it.
            int hitboxCount = project.Hitboxes[frame.Id].Count;

            selectedHitboxIndex_ = hitboxCount - 1;

            string nodeName = "Hitbox " + hitboxCount.ToString();
            string nodeKey = "HITBOX";

            newHitbox.Name = nodeName;

            TreeNode node;

            if (hitboxCount == 1)
            {
                node = tvAnimation.SelectedNode.Nodes.Add("HITBOXROOT", "Hitboxes");

                node.Tag = new AssetMenuDto(cmnuHitboxRoot, null);
            }
            
            IHitboxDtoProxy hitboxProxy = firemelonEditorFactory_.NewHitboxProxy(projectController_, newHitbox.Id);

            node = tvAnimation.Nodes["FRAMEROOT"]
                  .Nodes[selectedFrameIndex_]
                  .Nodes["HITBOXROOT"]
                  .Nodes.Add(nodeKey, nodeName);

            node.Tag = new AssetMenuDto(cmnuHitbox, hitboxProxy);

            tvAnimation.SelectedNode = node;
        }

        private void addFrameTrigger()
        {
            ProjectDto project = projectController_.GetProjectDto();

            FrameDto frame = project.Frames[animationProxy_.Id][selectedFrameIndex_];

            FrameTriggerDto newFrameTrigger = projectController_.AddFrameTrigger(frame.Id);

            // Add a hitbox node to the tree and select it.
            int frameTriggerCount = project.FrameTriggers[frame.Id].Count;

            selectedFrameTriggerIndex_ = frameTriggerCount - 1;

            string nodeName = "Frame Trigger " + frameTriggerCount.ToString();
            string nodeKey = "FRAMETRIGGER";

            TreeNode node;

            if (frameTriggerCount == 1)
            {
                node = tvAnimation.SelectedNode.Nodes.Add("FRAMETRIGGERROOT", "Frame Triggers");

                node.Tag = new AssetMenuDto(cmnuFrameTriggerRoot, null);
            }

            IFrameTriggerDtoProxy frameTriggerProxy = firemelonEditorFactory_.NewFrameTriggerProxy(projectController_, newFrameTrigger.Id);

            node = tvAnimation.Nodes["FRAMEROOT"]
                  .Nodes[selectedFrameIndex_]
                  .Nodes["FRAMETRIGGERROOT"]
                  .Nodes.Add(nodeKey, nodeName);

            node.Tag = new AssetMenuDto(cmnuFrameTrigger, frameTriggerProxy);

            tvAnimation.SelectedNode = node;
        }

        private void tvAnimation_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (tvAnimation.SelectedNode.Tag != null)
                {
                    ContextMenuStrip menu = ((AssetMenuDto)tvAnimation.SelectedNode.Tag).Menu;

                    if (menu != null)
                    {
                        menu.Show(tvAnimation, e.X, e.Y);
                    }
                }
            }
        }

        private void deleteHitboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHitboxDtoProxy hitbox = (IHitboxDtoProxy)((AssetMenuDto)tvAnimation.SelectedNode.Tag).Asset;

            projectController_.DeleteHitbox(hitbox.Id);

            // If this is the last hitbox node, remove the root node.
            if (tvAnimation.SelectedNode.Parent.Nodes.Count == 1)
            {
                tvAnimation.SelectedNode.Parent.Remove();
            }
            else
            {
                tvAnimation.SelectedNode.Remove();
            }
        }

        private void deleteFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFrameDtoProxy frame = (IFrameDtoProxy)((AssetMenuDto)tvAnimation.SelectedNode.Tag).Asset;

            projectController_.DeleteFrame(frame.Id);

            tvAnimation.SelectedNode.Remove();
        }

        private void addFrameTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFrameTrigger();
        }

        private void deleteFrameTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFrameTriggerDtoProxy frameTrigger = (IFrameTriggerDtoProxy)((AssetMenuDto)tvAnimation.SelectedNode.Tag).Asset;

            projectController_.DeleteFrameTrigger(frameTrigger.Id);

            // If this is the last frame trigger node, remove the root node.
            if (tvAnimation.SelectedNode.Parent.Nodes.Count == 1)
            {
                tvAnimation.SelectedNode.Parent.Remove();
            }
            else
            {
                tvAnimation.SelectedNode.Remove();
            }
        }

        private void addActionPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addActionPoint();
        }

        private void addActionPoint()
        {
            ProjectDto project = projectController_.GetProjectDto();

            FrameDto frame = project.Frames[animationProxy_.Id][selectedFrameIndex_];

            ActionPointDto newActionPoint = projectController_.AddActionPoint(frame.Id);

            newActionPoint.Name = getNextAvailableActionPointName("Action Point ");

            // Add a hitbox node to the tree and select it.
            int actionPointCount = project.ActionPoints[frame.Id].Count;

            selectedActionPointIndex_ = actionPointCount - 1;

            string nodeName = newActionPoint.Name;
            string nodeKey = "ACTIONPOINT";

            TreeNode node;

            if (actionPointCount == 1)
            {
                node = tvAnimation.SelectedNode.Nodes.Add("ACTIONPOINTROOT", "Action Points");

                node.Tag = new AssetMenuDto(cmnuActionPointRoot, null);
            }

            IActionPointDtoProxy actionPointProxy = firemelonEditorFactory_.NewActionPointProxy(projectController_, newActionPoint.Id);

            node = tvAnimation.Nodes["FRAMEROOT"]
                  .Nodes[selectedFrameIndex_]
                  .Nodes["ACTIONPOINTROOT"]
                  .Nodes.Add(nodeKey, nodeName);

            node.Tag = new AssetMenuDto(cmnuActionPoint, actionPointProxy);

            tvAnimation.SelectedNode = node;
        }
        
        private void addActionPointToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            addActionPoint();
        }

        private string getNextAvailableActionPointName(string baseName)
        {
            bool isNameInUse = true;
            int counter = 0;
            string currentName = baseName;

            // Find the first sequentially available name.
            while (isNameInUse == true)
            {
                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = baseName + counter.ToString();
                }

                isNameInUse = isActionPointNameInUse(currentName);

                counter++;
            }

            return currentName.Trim();
        }

        private bool isActionPointNameInUse(string name)
        {
            ProjectDto project = projectController_.GetProjectDto();

            FrameDto currentFrame = project.Frames[animationProxy_.Id][selectedFrameIndex_];

            foreach (ActionPointDto actionPoint in project.ActionPoints[currentFrame.Id])
            {
                if (actionPoint.Name.ToUpper() == name.ToUpper())
                {
                    return true;
                }
            }

            return false;
        }

        private void deleteActionPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IActionPointDtoProxy actionPoint = (IActionPointDtoProxy)((AssetMenuDto)tvAnimation.SelectedNode.Tag).Asset;

            projectController_.DeleteActionPoint(actionPoint.Id);

            // If this is the last  action point node, remove the root node.
            if (tvAnimation.SelectedNode.Parent.Nodes.Count == 1)
            {
                tvAnimation.SelectedNode.Parent.Remove();
            }
            else
            {
                tvAnimation.SelectedNode.Remove();
            }
        }

        private void vsAnimation_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void hsAnimation_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void addFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            FrameDto frame = projectController_.AddFrame(animationProxy_.Id);

            // Add frame to tree
            int frameCount = project.Frames[animationProxy_.Id].Count;

            TreeNode node = tvAnimation.Nodes["FRAMEROOT"].Nodes.Add("FRAME", "Frame " + frameCount.ToString());

            IFrameDtoProxy frameProxy = firemelonEditorFactory_.NewFrameProxy(projectController_, frame.Id);

            node.Tag = new AssetMenuDto(cmnuFrame, frameProxy);

            tvAnimation.SelectedNode = node;
        }
    }
}