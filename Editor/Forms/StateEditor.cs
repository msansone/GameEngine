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

    public partial class StateEditor : Form, IStateEditor
    {
        private IStateDtoProxy stateProxy_;
        private IFiremelonEditorFactory firemelonEditorFactory_;
        private IProjectController projectController_;
        private IBackgroundGenerator backgroundGenerator_;
        
        private int selectedAnimationSlotIndex_;
        private int selectedHitboxIndex_;

        private bool isMouseDown_;
        private bool allowAddHitboxes_;

        public StateEditor()
        {
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();
            backgroundGenerator_ = firemelonEditorFactory_.NewBackgroundGenerator();

            vsState.SmallChange = 1;
            vsState.LargeChange = 1;
            hsState.SmallChange = 1;
            hsState.LargeChange = 1;

            tvState.NodeMouseClick += (sender, args) => tvState.SelectedNode = args.Node;
        }

        public void ShowDialog(IWin32Window owner, IProjectController projectController, IStateDtoProxy stateProxy, bool allowAddHitboxes)
        {
            stateProxy_ = stateProxy;
            projectController_ = projectController;
            
            selectedAnimationSlotIndex_ = -1;
            selectedHitboxIndex_ = -1;
            
            isMouseDown_ = false;

            allowAddHitboxes_ = allowAddHitboxes;

            if (allowAddHitboxes_ == true)
            {
                addHitboxToolStripMenuItem.Visible = true;
            }
            else
            {
                addHitboxToolStripMenuItem.Visible = false;
            }

            buildTreeView();

            buildOtherStateList();

            pgState.SelectedObject = stateProxy_;

            base.ShowDialog(owner);
        }

        private void buildOtherStateList()
        {
            copyHitboxesFromStateToolStripMenuItem.DropDownItems.Clear();

            ProjectDto project = projectController_.GetProjectDto();

            foreach (StateDto state in project.States[stateProxy_.OwnerId])
            {
                if (state.Id != stateProxy_.Id)
                {
                    ToolStripMenuItem newMenuItem = new ToolStripMenuItem();

                    newMenuItem.Text = state.Name;
                    newMenuItem.Tag = state.Id;
                    newMenuItem.Click += mnuCopyHitboxesFromState_Click;

                    copyHitboxesFromStateToolStripMenuItem.DropDownItems.Add(newMenuItem);

                }
            }
        }

        private void mnuCopyHitboxesFromState_Click(object sender, EventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

            Guid stateId = (Guid)(menuItem.Tag);
            
            foreach (HitboxDto hitbox in project.Hitboxes[stateId])
            {
                HitboxDto newHitbox = addHitbox();

                newHitbox.CornerPoint1.X = hitbox.CornerPoint1.X;

                newHitbox.CornerPoint1.Y = hitbox.CornerPoint1.Y;

                newHitbox.CornerPoint2.X = hitbox.CornerPoint2.X;

                newHitbox.CornerPoint2.Y = hitbox.CornerPoint2.Y;

                newHitbox.HitboxRect = new Rectangle(hitbox.HitboxRect.Left, hitbox.HitboxRect.Top, hitbox.HitboxRect.Width, hitbox.HitboxRect.Height);

                newHitbox.Identity = hitbox.Identity;

                newHitbox.IsSolid = hitbox.IsSolid;

                newHitbox.Priority = hitbox.Priority;

                newHitbox.RotationDegrees = hitbox.RotationDegrees;
            }
        }

        private void buildTreeView()
        {
            ProjectDto project = projectController_.GetProjectDto();

            tvState.Nodes.Clear();

            TreeNode node = tvState.Nodes.Add("ANIMATIONSLOTROOT", "Animation Slots");
            node.Tag = new AssetMenuDto(cmnuAnimationSlotRoot, null);

            if (allowAddHitboxes_ == true)
            {
                node = tvState.Nodes.Add("HITBOXROOT", "Hitboxes");
                node.Tag = new AssetMenuDto(cmnuHitboxRoot, null);
            }

            foreach (AnimationSlotDto animationSlot in project.AnimationSlots[stateProxy_.Id])
            {
                string nodeName = animationSlot.Name;
                string nodeKey = "ANIMATIONSLOT";

                IAnimationSlotDtoProxy animationSlotProxy = firemelonEditorFactory_.NewAnimationSlotProxy(projectController_, animationSlot.Id);

                node = tvState.Nodes["ANIMATIONSLOTROOT"].Nodes.Add(nodeKey, nodeName);
                node.Tag = new AssetMenuDto(cmnuAnimationSlot, animationSlotProxy);
            }

            int hitboxCounter = 1;

            foreach (HitboxDto hitbox in project.Hitboxes[stateProxy_.Id])
            {
                IHitboxDtoProxy hitboxProxy = firemelonEditorFactory_.NewHitboxProxy(projectController_, hitbox.Id);

                string nodeName = "Hitbox " + hitboxCounter.ToString();
                string nodeKey = "HITBOX";

                tvState.Nodes["HITBOXROOT"]
                       .Nodes.Add(nodeKey, nodeName).Tag = new AssetMenuDto(cmnuHitbox, hitboxProxy);

                hitboxCounter += 1;
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void pnState_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void resize()
        {
            hsState.Top = pnState.ClientSize.Height - hsState.Height;
            hsState.Width = pnState.ClientSize.Width - vsState.Width;

            vsState.Left = pnState.ClientSize.Width - vsState.Width;
            vsState.Height = pnState.ClientSize.Height - hsState.Height;

            pbState.Width = pnState.Width - vsState.Width - 1;
            pbState.Height = pnState.Height - hsState.Height - 1;

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

            pbState.Refresh();
        }

        private void StateEditor_Load(object sender, EventArgs e)
        {
            resize();
        }

        private void pbState_Paint(object sender, PaintEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            
            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();

            Graphics g = e.Graphics;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            int stageWidth = stateProxy_.StageWidth;

            int stageHeight = stateProxy_.StageHeight;

            Point center = new Point(pbState.Width / 2, pbState.Height / 2);

            Point stageCenter = new Point(stageWidth / 2, stageHeight / 2);

            Point renderOrigin = new Point(center.X - stageCenter.X, center.Y - stageCenter.Y);

            if (stageWidth > 0 && stageHeight > 0)
            {
                Bitmap bmpBackground = backgroundGenerator_.GenerateBackground(stageWidth, stageHeight);
                
                g.DrawImageUnscaled(bmpBackground, renderOrigin);
            }
            
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
                            
                            // Separate resources dto removed in 2.2 format.
                            //BitmapResourceDto bitmapResource = resources.Bitmaps[bitmapResourceId];
                            BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(bitmapResourceId, true);

                            if (spriteSheetCellIndex >= 0 && spriteSheetCellIndex < bitmapResource.SpriteSheetImageList.Count)
                            {
                                int cellX = 0;
                                int cellY = 0;

                                switch (stateProxy_.StageOriginLocation)
                                {
                                    case OriginLocation.TopLeft:
                                        cellX = currentAnimationSlot.Position.X + (-1 * hsState.Value);
                                        cellY = currentAnimationSlot.Position.Y + (-1 * vsState.Value);
                                        break;

                                    case OriginLocation.Center:
                                        cellX = currentAnimationSlot.Position.X + (-1 * hsState.Value) + stageCenter.X - (int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2);
                                        cellY = currentAnimationSlot.Position.Y + (-1 * vsState.Value) + stageCenter.Y - (int)((spriteSheet.CellHeight * spriteSheet.ScaleFactor) / 2);
                                        break;

                                    case OriginLocation.TopMiddle:
                                        cellX = currentAnimationSlot.Position.X + (-1 * hsState.Value) + stageCenter.X - (int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2);
                                        cellY = currentAnimationSlot.Position.Y + (-1 * vsState.Value);
                                        break;
                                }


                                for (int j = 0; j < bitmapResource.SpriteSheetImageList.ImageListRows; j++)
                                {
                                    for (int k = 0; k < bitmapResource.SpriteSheetImageList.ImageListCols; k++)
                                    {
                                        // Render each section of the image.
                                        using (Image img = bitmapResource.SpriteSheetImageList.ImageLists[j][k].Images[spriteSheetCellIndex])
                                        {
                                            // I used to multiply by the scale factor here, but this turned out to be incorrect, because
                                            // The subcells have already had the scaling applied.
                                            int subCellX = cellX + (k * (int)(Globals.maxImageListWidth));

                                            int subCellY = cellY + (j * (int)(Globals.maxImageListWidth));

                                            Point pt = new Point(renderOrigin.X + subCellX, renderOrigin.Y + subCellY);

                                            g.DrawImageUnscaled(img, pt);
                                        }

                                        GC.Collect();
                                    }
                                }
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

                System.Drawing.Rectangle drawRect = new System.Drawing.Rectangle();

                switch (stateProxy_.StageOriginLocation)
                {
                    case OriginLocation.TopLeft:
                        drawRect.X = renderOrigin.X + currentHitbox.HitboxRect.Left + (-1 * hsState.Value);
                        drawRect.Y = renderOrigin.Y + currentHitbox.HitboxRect.Top + (-1 * vsState.Value);
                        break;

                    case OriginLocation.Center:
                        drawRect.X = renderOrigin.X + stageCenter.X - (currentHitbox.HitboxRect.Width / 2) + currentHitbox.HitboxRect.Left + (-1 * hsState.Value);
                        drawRect.Y = renderOrigin.Y + stageCenter.Y - (currentHitbox.HitboxRect.Height / 2) + currentHitbox.HitboxRect.Top + (-1 * vsState.Value);
                        break;

                    case OriginLocation.TopMiddle:
                        drawRect.X = renderOrigin.X + stageCenter.X - (currentHitbox.HitboxRect.Width / 2) + currentHitbox.HitboxRect.Left + (-1 * hsState.Value);
                        drawRect.Y = renderOrigin.Y + currentHitbox.HitboxRect.Top + (-1 * vsState.Value);
                        break;
                }

                drawRect.Height = currentHitbox.HitboxRect.Height;
                drawRect.Width = currentHitbox.HitboxRect.Width;

                g.DrawRectangle(new System.Drawing.Pen(new System.Drawing.SolidBrush(Color.Blue)), drawRect);
            }
        }

        private void pgState_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":
                    // Update the tree view with the new name.
                    string newName = e.ChangedItem.Value.ToString();
                    tvState.SelectedNode.Text = newName;

                    break;

                case "STAGEHEIGHT":
                case "STAGEWIDTH":

                    // Resize to update the scrollbars.
                    resize();

                    backgroundGenerator_.Regenerate();

                    break;
            }

            pbState.Refresh();
        }

        private void tvState_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (tvState.SelectedNode.Tag != null)
                {
                    ContextMenuStrip menu = ((AssetMenuDto)tvState.SelectedNode.Tag).Menu;

                    if (menu != null)
                    {
                        menu.Show(tvState, e.X, e.Y);
                    }
                }
            }
        }

        private void addAnimationSlot()
        {
            ProjectDto project = projectController_.GetProjectDto();

            AnimationSlotDto newAnimationSlot = projectController_.AddAnimationSlot(stateProxy_.Id);

            newAnimationSlot.Name = getNextAvailableSlotName("Animation Slot ");

            int animationSlotCount = project.AnimationSlots[stateProxy_.Id].Count;

            selectedAnimationSlotIndex_ = animationSlotCount - 1;

            string nodeName = newAnimationSlot.Name;
            string nodeKey = "ANIMATIONSLOT";

            IAnimationSlotDtoProxy animationSlotProxy = firemelonEditorFactory_.NewAnimationSlotProxy(projectController_, newAnimationSlot.Id);

            TreeNode node = tvState.Nodes["ANIMATIONSLOTROOT"].Nodes.Add(nodeKey, nodeName);

            node.Tag = new AssetMenuDto(cmnuAnimationSlot, animationSlotProxy);

            tvState.SelectedNode = node;
        }

        private void tvState_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvState.SelectedNode.Name == "ANIMATIONSLOT")
            {
                IAnimationSlotDtoProxy animationSlot = (IAnimationSlotDtoProxy)((AssetMenuDto)(tvState.SelectedNode.Tag)).Asset;

                selectedAnimationSlotIndex_ = tvState.SelectedNode.Index;
                selectedHitboxIndex_ = -1;

                pgState.SelectedObject = animationSlot;
            }
            else if (tvState.SelectedNode.Name == "HITBOX")
            {
                selectedAnimationSlotIndex_ = -1;
                selectedHitboxIndex_ = tvState.SelectedNode.Index;

                IHitboxDtoProxy hitbox = (IHitboxDtoProxy)((AssetMenuDto)(tvState.SelectedNode.Tag)).Asset;

                pgState.SelectedObject = hitbox;
            }
            else
            {
                selectedAnimationSlotIndex_ = -1;
                selectedHitboxIndex_ = -1;

                pgState.SelectedObject = stateProxy_;
            }

            pbState.Refresh();
        }

        private void addHitboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addHitbox();
        }

        private HitboxDto addHitbox()
        {
            ProjectDto project = projectController_.GetProjectDto();

            StateDto state = projectController_.GetState(stateProxy_.Id);

            HitboxDto newHitbox = projectController_.AddHitbox(stateProxy_.Id, state.OwnerId);

            // Add a hitbox node to the tree and select it.
            int hitboxCount = project.Hitboxes[stateProxy_.Id].Count;

            selectedHitboxIndex_ = hitboxCount - 1;

            string nodeName = "Hitbox " + hitboxCount.ToString();
            string nodeKey = "HITBOX";

            newHitbox.Name = nodeName;

            IHitboxDtoProxy hitboxProxy = firemelonEditorFactory_.NewHitboxProxy(projectController_, newHitbox.Id);

            TreeNode node = tvState.Nodes["HITBOXROOT"]
                            .Nodes.Add(nodeKey, nodeName);

            node.Tag = new AssetMenuDto(cmnuHitbox, hitboxProxy);

            tvState.SelectedNode = node;

            return newHitbox;
        }
        
        private void pbState_MouseDown(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                isMouseDown_ = true;

                int stageWidth = stateProxy_.StageWidth;
                int stageHeight = stateProxy_.StageHeight;

                Point center = new Point(pbState.Width / 2, pbState.Height / 2);

                Point stageCenter = new Point(stageWidth / 2, stageHeight / 2);

                Point renderOrigin = new Point(center.X - stageCenter.X, center.Y - stageCenter.Y);

                // Set the first corner point of the current hitbox.
                projectController_.SetHitboxCornerPoint1(stateProxy_.Id, selectedHitboxIndex_, e.X - renderOrigin.X, e.Y - renderOrigin.Y);

                pbState.Refresh();
            }             
        }

        private void pbState_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown_ == true && selectedHitboxIndex_ >= 0)
            {
                int stageWidth = stateProxy_.StageWidth;
                int stageHeight = stateProxy_.StageHeight;

                Point center = new Point(pbState.Width / 2, pbState.Height / 2);

                Point stageCenter = new Point(stageWidth / 2, stageHeight / 2);

                Point renderOrigin = new Point(center.X - stageCenter.X, center.Y - stageCenter.Y);

                // Set the second corner point of the current hitbox.
                projectController_.SetHitboxCornerPoint2(stateProxy_.Id, selectedHitboxIndex_, e.X - renderOrigin.X, e.Y - renderOrigin.Y);
                
                // Refresh the property grid.
                pgState.Refresh();

                // Refresh the picture box.
                pbState.Refresh();
            }
        }

        private void pbState_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown_ = false;
        }

        private void vsState_Scroll(object sender, ScrollEventArgs e)
        {
            pbState.Refresh();
        }

        private void hsState_Scroll(object sender, ScrollEventArgs e)
        {
            pbState.Refresh();
        }

        private void pgState_Click(object sender, EventArgs e)
        {

        }

        private void addAnimationSlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addAnimationSlot();
        }

        private void deleteAnimationSlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Animation Slot?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                IBaseDtoProxy slot = (IBaseDtoProxy)((AssetMenuDto)tvState.SelectedNode.Tag).Asset;

                projectController_.DeleteAnimationSlot(slot.Id);

                tvState.SelectedNode.Remove();
            }
        }

        private string getNextAvailableSlotName(string baseName)
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

                isNameInUse = isSlotNameInUse(currentName);

                counter++;
            }

            return currentName.Trim();
        }

        private bool isSlotNameInUse(string name)
        {
            ProjectDto project = projectController_.GetProjectDto();

            foreach (AnimationSlotDto animationSlot in project.AnimationSlots[stateProxy_.Id])
            {
                if (animationSlot.Name.ToUpper() == name.ToUpper())
                {
                    return true;
                }
            }

            return false;
        }

        private void deleteHitboxToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Delete Hitbox?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                IBaseDtoProxy hitbox = (IBaseDtoProxy)((AssetMenuDto)tvState.SelectedNode.Tag).Asset;

                projectController_.DeleteHitbox(hitbox.Id);

                tvState.SelectedNode.Remove();
            }
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentSlotIndex = tvState.SelectedNode.Index;

            projectController_.MoveUpAnimationSlot(stateProxy_.Id, currentSlotIndex);

            if (currentSlotIndex > 0)
            {
                TreeNode node = tvState.SelectedNode;
                TreeNode parent = node.Parent;

                tvState.SelectedNode.Remove();

                parent.Nodes.Insert(node.Index - 1, node);

                tvState.SelectedNode = node;
            }
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentSlotIndex = tvState.SelectedNode.Index;

            projectController_.MoveDownAnimationSlot(stateProxy_.Id, currentSlotIndex);

            ProjectDto project = projectController_.GetProjectDto();

            int slotCount = project.AnimationSlots[stateProxy_.Id].Count;

            if (currentSlotIndex < slotCount - 1)
            {
                TreeNode node = tvState.SelectedNode;
                TreeNode parent = node.Parent;

                tvState.SelectedNode.Remove();

                parent.Nodes.Insert(node.Index + 1, node);

                tvState.SelectedNode = node;
            }
        }

        private void copyHitboxesFromStateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void copyHitboxesFromStateToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {

        }
        
        private void shiftSlotsAndHitboxes(int x, int y)
        {
            ProjectDto project = projectController_.GetProjectDto();
            
            foreach (AnimationSlotDto animationSlot in project.AnimationSlots[stateProxy_.Id])
            {
                animationSlot.Position.X += x;
                animationSlot.Position.Y += y;
            }
            
            foreach (HitboxDto hitbox in project.Hitboxes[stateProxy_.Id])
            {
                hitbox.HitboxRect.Left += x;
                hitbox.HitboxRect.Top += y;
            }
        }
    }

    public interface IStateEditor
    {
        void ShowDialog(IWin32Window owner, IProjectController projectController, IStateDtoProxy stateProxy, bool allowAddHitboxes);
        void Dispose();
    }
}