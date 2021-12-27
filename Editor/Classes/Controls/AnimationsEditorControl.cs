using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    #region Delegates

    public delegate void AnimationSelectionChangedHandler(object sender, AnimationSelectionChangedEventArgs e);

    #endregion

    public partial class AnimationsEditorControl : UserControl, IAnimationsEditorControl
    {
        #region Events

        public event AnimationSelectionChangedHandler AnimationSelectionChanged;

        #endregion

        #region Constructors

        public AnimationsEditorControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            firemelonEditorFactory_ = new FiremelonEditorFactory();
            
            ProjectDto project = projectController.GetProjectDto();

            // Initialize animations.
            foreach (AnimationGroupDto animationGroup in project.AnimationGroups)
            {
                TreeNode groupNode = addAnimationGroupToTree(animationGroup);

                foreach (AnimationDto animation in project.Animations[animationGroup.Id])
                {
                    TreeNode animationNode = addAnimationToTree(groupNode, animation);
                    
                    foreach (FrameDto frame in project.Frames[animation.Id])
                    {
                        TreeNode frameNode = addAnimationFrameToTree(animationNode, frame);
                        
                        foreach (HitboxDto hitbox in project.Hitboxes[frame.Id])
                        {
                            addHitboxToTree(frameNode, hitbox);
                        }

                        foreach (FrameTriggerDto frameTrigger in project.FrameTriggers[frame.Id])
                        {
                            addFrameTriggerToTree(frameNode, frameTrigger);
                        }

                        foreach (ActionPointDto actionPoint in project.ActionPoints[frame.Id])
                        {
                            addActionPointToTree(frameNode, actionPoint);
                        }
                    }
                }
            }
            
            animationFrameViewerControl_ = firemelonEditorFactory_.NewAnimationFrameViewerControl(projectController);

            Control animationFrameViewerControl = (Control)animationFrameViewerControl_;

            animationFrameViewerControl.Dock = DockStyle.Fill;

            scAnimations.Panel2.Controls.Add(animationFrameViewerControl);
        }

        #endregion
        
        #region Private Variables

        private IAnimationFrameViewerControl animationFrameViewerControl_;

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private IProjectController projectController_;

        #endregion

        #region Public Functions

        public void AddActionPoint()
        {
            // Add an action point to the selected frame.            
            TreeNode frameNode = null;
            
            switch (tvAnimations.SelectedNode.Name)
            {
                case "ANIMATIONFRAME":

                    frameNode = tvAnimations.SelectedNode;

                    break;

                case "ACTIONPOINTROOT":
                case "FRAMETRIGGERROOT":
                case "HITBOXROOT":

                    frameNode = tvAnimations.SelectedNode.Parent;

                    break;

                case "ACTIONPOINT":
                case "FRAMETRIGGER":
                case "HITBOX":

                    frameNode = tvAnimations.SelectedNode.Parent.Parent;

                    break;
            }

            if (frameNode != null)
            {
                IFrameDtoProxy frameProxy = (IFrameDtoProxy)((AssetMenuDto)frameNode.Tag).Asset;

                addActionPoint(frameProxy.Id);
            }
        }

        public void AddNew()
        {
            TreeNode selectedNode = tvAnimations.SelectedNode;

            Guid animationGroupId = Guid.Empty;

            if (selectedNode != null)
            {
                if (selectedNode.Name == "ANIMATIONGROUP")
                {
                    animationGroupId = projectController_.GetAnimationGroupIdFromName(selectedNode.Text);
                }
                else
                {
                    animationGroupId = projectController_.GetAnimationGroupIdFromName(selectedNode.Parent.Text);
                }
            }
            
            addAnimation(animationGroupId);
        }

        public void AddFrame()
        {
            TreeNode selectedNode = tvAnimations.SelectedNode;

            Guid animationId = Guid.Empty;

            if (selectedNode != null)
            {
                if (selectedNode.Name == "ANIMATION")
                {
                    animationId = projectController_.GetAnimationIdFromName(selectedNode.Text);
                }
                else if(selectedNode.Name == "ANIMATIONFRAME")
                {
                    animationId = projectController_.GetAnimationIdFromName(selectedNode.Parent.Text);
                }
            }

            addAnimationFrame(animationId);
        }

        public void AddFrameTrigger()
        {
            // Add a frame trigger to the selected frame.            
            TreeNode frameNode = null;
            
            switch (tvAnimations.SelectedNode.Name)
            {
                case "ANIMATIONFRAME":

                    frameNode = tvAnimations.SelectedNode;

                    break;

                case "ACTIONPOINTROOT":
                case "FRAMETRIGGERROOT":
                case "HITBOXROOT":

                    frameNode = tvAnimations.SelectedNode.Parent;

                    break;

                case "ACTIONPOINT":
                case "FRAMETRIGGER":
                case "HITBOX":

                    frameNode = tvAnimations.SelectedNode.Parent.Parent;

                    break;
            }

            if (frameNode != null)
            {
                IFrameDtoProxy frameProxy = (IFrameDtoProxy)((AssetMenuDto)frameNode.Tag).Asset;

                addFrameTrigger(frameProxy.Id);
            }
        }

        public void AddGroup()
        {
            addAnimationGroup();
        }

        public void AddHitbox()
        {
            // Add a hitbox to the selected frame.            
            TreeNode frameNode = null;
            
            switch (tvAnimations.SelectedNode.Name)
            {
                case "ANIMATIONFRAME":

                    frameNode = tvAnimations.SelectedNode;
                    
                    break;

                case "ACTIONPOINTROOT":
                case "FRAMETRIGGERROOT":
                case "HITBOXROOT":

                    frameNode = tvAnimations.SelectedNode.Parent;
                    
                    break;

                case "ACTIONPOINT":
                case "FRAMETRIGGER":
                case "HITBOX":

                    frameNode = tvAnimations.SelectedNode.Parent.Parent;
                    
                    break;
            }
            
            if (frameNode != null)
            {
                IFrameDtoProxy frameProxy = (IFrameDtoProxy)((AssetMenuDto)frameNode.Tag).Asset;

                addHitbox(frameProxy.Id);
            }            
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void DeleteFrame()
        {
            IFrameDtoProxy frame = (IFrameDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

            projectController_.DeleteFrame(frame.Id);
            
            tvAnimations.SelectedNode.Remove();
        }

        public void DeleteHitbox()
        {
            IHitboxDtoProxy hitbox = (IHitboxDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

            projectController_.DeleteHitbox(hitbox.Id);

            // If this is the last hitbox node, remove the root node.
            if (tvAnimations.SelectedNode.Parent.Nodes.Count == 1)
            {
                tvAnimations.SelectedNode.Parent.Remove();
            }
            else
            {
                tvAnimations.SelectedNode.Remove();
            }
        }

        #endregion

        #region Private Functions

        private void addActionPoint(Guid frameId)
        {
            FrameDto frame = projectController_.GetFrame(frameId);

            ActionPointDto newActionPoint = projectController_.AddActionPoint(frameId);

            TreeNode frameNode = getFrameTreeNode(frameId);

            addActionPointToTree(frameNode, newActionPoint);
        }

        private void addAnimation(Guid groupId)
        {
            ProjectDto project = projectController_.GetProjectDto();
            
            bool isNameValid = false;
            int counter = 0;

            string currentName = "New Animation";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Animation " + counter.ToString();
                }

                foreach (AnimationGroupDto animationGroup in project.AnimationGroups)
                {
                    int animationsWithinGroupCount = project.Animations[animationGroup.Id].Count;

                    for (int j = 0; j < animationsWithinGroupCount; j++)
                    {
                        string animationName = project.Animations[animationGroup.Id][j].Name;

                        if (currentName.ToUpper() == animationName.ToUpper())
                        {
                            isNameValid = false;
                            break;
                        }
                    }
                }

                counter++;
            }

            AnimationDto newAnimation = projectController_.AddAnimation(groupId, currentName);
            
            TreeNode groupNode = getGroupTreeNode(groupId);

            addAnimationToTree(groupNode, newAnimation);
        }

        private void addAnimationFrame(Guid animationId)
        {
            AnimationDto animation = projectController_.GetAnimation(animationId);

            if (animation != null)
            {
                ProjectDto project = projectController_.GetProjectDto();

                FrameDto frame = projectController_.AddFrame(animationId);

                TreeNode groupNode = getGroupTreeNode(animation.GroupId);

                TreeNode animationNode = getAnimationTreeNode(groupNode, animationId);

                addAnimationFrameToTree(animationNode, frame);
            }
        }
        
        private void addAnimationGroup()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int animationGroupCount = project.AnimationGroups.Count;

            bool isNameValid = false;
            int counter = 0;

            string currentName = "New Animation Group";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Animation Group " + counter.ToString();
                }

                for (int j = 0; j < animationGroupCount; j++)
                {
                    string animationGroupName = project.AnimationGroups[j].Name;

                    if (currentName.ToUpper() == animationGroupName.ToUpper())
                    {
                        isNameValid = false;
                        break;
                    }
                }

                counter++;
            }

            AnimationGroupDto animationGroup = projectController_.AddAnimationGroup(currentName);

            addAnimationGroupToTree(animationGroup);
        }

        private TreeNode addAnimationFrameToTree(TreeNode animationNode, FrameDto frame)
        {
            IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)animationNode.Tag).Asset;

            ProjectDto project = projectController_.GetProjectDto();

            int frameCount = animationNode.Nodes.Count + 1;

            TreeNode animationFrameNode = animationNode.Nodes.Add("ANIMATIONFRAME", "Frame " + frameCount.ToString());
            
            IFrameDtoProxy frameProxy = firemelonEditorFactory_.NewFrameProxy(projectController_, frame.Id);

            animationFrameNode.Tag = new AssetMenuDto(cmnuFrame, frameProxy);

            //tvAnimations.SelectedNode = node;

            return animationFrameNode;
        }

        private TreeNode addAnimationToTree(TreeNode groupNode, AnimationDto animation)
        {
            IAnimationDtoProxy animationProxy = firemelonEditorFactory_.NewAnimationProxy(projectController_, animation.Id);

            // If this animation already has a node on the tree, ignore it.
            foreach (TreeNode node in groupNode.Nodes)
            {
                IAnimationDtoProxy nodeProxy = (IAnimationDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == animationProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode animationNode = groupNode.Nodes.Add("ANIMATION", animation.Name);

            animationNode.Tag = new AssetMenuDto(cmnuAnimation, animationProxy);

            return animationNode;
        }
        
        private TreeNode addAnimationGroupToTree(AnimationGroupDto animationGroup)
        {
            IAnimationGroupDtoProxy animationGroupProxy = firemelonEditorFactory_.NewAnimationGroupProxy(projectController_, animationGroup.Id);

            // If this animation already has a node on the tree, ignore it.
            foreach (TreeNode node in tvAnimations.Nodes)
            {
                IAnimationGroupDtoProxy nodeProxy = (IAnimationGroupDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == animationGroupProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode animationGroupNode = tvAnimations.Nodes.Add("ANIMATIONGROUP", animationGroup.Name);
            
            animationGroupNode.Tag = new AssetMenuDto(cmnuAnimationGroup, animationGroupProxy);

            return animationGroupNode;
        }

        private TreeNode addActionPointToTree(TreeNode frameNode, ActionPointDto actionPoint)
        {
            IActionPointDtoProxy actionPointProxy = firemelonEditorFactory_.NewActionPointProxy(projectController_, actionPoint.Id);

            ProjectDto project = projectController_.GetProjectDto();
            
            //selectedHitboxIndex_ = hitboxCount - 1;

            string nodeName;

            string nodeKey = "ACTIONPOINT";

            TreeNode actionPointRootNode;

            if (frameNode.Nodes.ContainsKey("ACTIONPOINTROOT") == false)
            {
                actionPointRootNode = frameNode.Nodes.Add("ACTIONPOINTROOT", "Action Points");

                actionPointRootNode.Tag = new AssetMenuDto(cmnuActionPointRoot, null);

                nodeName = "Action Point 1";
            }
            else
            {
                actionPointRootNode = frameNode.Nodes["ACTIONPOINTROOT"];

                nodeName = "Action Point " + (actionPointRootNode.Nodes.Count + 1);
            }

            if (string.IsNullOrEmpty(actionPoint.Name))
            {
                actionPoint.Name = nodeName;
            }
           
            TreeNode actionPointNode = actionPointRootNode.Nodes.Add(nodeKey, actionPoint.Name);

            actionPointNode.Tag = new AssetMenuDto(cmnuFrameTrigger, actionPointProxy);

            return actionPointNode;
        }

        private void addFrameTrigger(Guid frameId)
        {
            FrameDto frame = projectController_.GetFrame(frameId);

            FrameTriggerDto newFrameTrigger = projectController_.AddFrameTrigger(frameId);

            TreeNode frameNode = getFrameTreeNode(frameId);

            addFrameTriggerToTree(frameNode, newFrameTrigger);
        }

        private TreeNode addFrameTriggerToTree(TreeNode frameNode, FrameTriggerDto frameTrigger)
        {
            IFrameTriggerDtoProxy frameTriggerProxy = firemelonEditorFactory_.NewFrameTriggerProxy(projectController_, frameTrigger.Id);

            ProjectDto project = projectController_.GetProjectDto();
            
            //selectedHitboxIndex_ = hitboxCount - 1;

            string nodeName;

            string nodeKey = "FRAMETRIGGER";

            TreeNode frameTriggerRootNode;
            
            if (frameNode.Nodes.ContainsKey("FRAMETRIGGERROOT") == false)
            {
                frameTriggerRootNode = frameNode.Nodes.Add("FRAMETRIGGERROOT", "Frame Triggers");

                frameTriggerRootNode.Tag = new AssetMenuDto(cmnuFrameTriggerRoot, null);

                nodeName = "Frame Trigger 1";
            }
            else
            {
                frameTriggerRootNode = frameNode.Nodes["FRAMETRIGGERROOT"];

                nodeName = "Frame Trigger " + (frameTriggerRootNode.Nodes.Count + 1);
            }

            frameTrigger.Name = nodeName;

            TreeNode frameTriggerNode = frameTriggerRootNode.Nodes.Add(nodeKey, nodeName);

            frameTriggerNode.Tag = new AssetMenuDto(cmnuFrameTrigger, frameTriggerProxy);

            return frameTriggerNode;
        }

        private void addHitbox(Guid frameId)
        {
            FrameDto frame = projectController_.GetFrame(frameId);

            HitboxDto newHitbox = projectController_.AddHitbox(frameId, frame.OwnerId);

            TreeNode frameNode = getFrameTreeNode(frameId);

            addHitboxToTree(frameNode, newHitbox);
        }

        private TreeNode addHitboxToTree(TreeNode frameNode, HitboxDto hitbox)
        {
            IHitboxDtoProxy hitboxProxy = firemelonEditorFactory_.NewHitboxProxy(projectController_, hitbox.Id);

            ProjectDto project = projectController_.GetProjectDto();

            int hitboxCount = project.Hitboxes[hitboxProxy.OwnerId].Count;

            //selectedHitboxIndex_ = hitboxCount - 1;

            string nodeName;

            string nodeKey = "HITBOX";

            TreeNode hitboxRootNode;

            if (frameNode.Nodes.ContainsKey("HITBOXROOT") == false)
            {
                hitboxRootNode = frameNode.Nodes.Add("HITBOXROOT", "Hitboxes");

                hitboxRootNode.Tag = new AssetMenuDto(cmnuHitboxRoot, null);

                nodeName = "Hitbox 1";
            }
            else
            {
                hitboxRootNode = frameNode.Nodes["HITBOXROOT"];

                nodeName = "Hitbox " + (hitboxRootNode.Nodes.Count + 1);
            }

            hitbox.Name = nodeName;

            TreeNode hitboxNode = hitboxRootNode.Nodes.Add(nodeKey, nodeName);

            hitboxNode.Tag = new AssetMenuDto(cmnuHitbox, hitboxProxy);
            
            return hitboxNode;
        }

        private TreeNode getAnimationTreeNode(TreeNode groupNode, Guid animationId)
        {
            foreach (TreeNode node in groupNode.Nodes)
            {
                IAnimationDtoProxy nodeProxy = (IAnimationDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == animationId)
                {
                    return node;
                }
            }

            return null;
        }
        
        private TreeNode getGroupTreeNode(Guid groupId)
        {
            foreach (TreeNode node in tvAnimations.Nodes)
            {
                IAnimationGroupDtoProxy nodeProxy = (IAnimationGroupDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == groupId)
                {
                    return node;
                }
            }

            return null;
        }

        private TreeNode getFrameTreeNode(Guid frameId)
        {
            foreach (TreeNode groupNode in tvAnimations.Nodes)
            {
                foreach (TreeNode animationNode in groupNode.Nodes)
                {
                    foreach (TreeNode frameNode in animationNode.Nodes)
                    {
                        IFrameDtoProxy nodeProxy = (IFrameDtoProxy)((AssetMenuDto)frameNode.Tag).Asset;

                        if (nodeProxy.Id == frameId)
                        {
                            return frameNode;
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region Event Handlers

        private void tvAnimations_AfterSelect(object sender, TreeViewEventArgs e)
        {
            animationFrameViewerControl_.ActionPointIndex = -1;

            animationFrameViewerControl_.FrameIndex = -1;

            animationFrameViewerControl_.HitboxIndex = -1;

            if (tvAnimations.SelectedNode.Name == "ANIMATIONGROUP")
            {
                IAnimationGroupDtoProxy animationGroupProxy = (IAnimationGroupDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

                //// If this is the "None" group, make it's name property read only.                
                //PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(AnimationGroupDtoProxy))["Name"];

                //ReadOnlyAttribute attr = (ReadOnlyAttribute)(prop.Attributes[typeof(ReadOnlyAttribute)]);

                //Type attrType = attr.GetType();

                //FieldInfo fi = attrType.GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);

                //if (animationGroupProxy.Id == Guid.Empty)
                //{
                //    fi.SetValue(attr, true);
                //}
                //else
                //{
                //    fi.SetValue(attr, false);
                //}

                pgAnimation.SelectedObject = animationGroupProxy;

                if (tvAnimations.SelectedNode.Nodes.Count > 0)
                {
                    IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Nodes[0].Tag).Asset;
                    
                    if (animationFrameViewerControl_.Animation != null)
                    {
                        SpriteSheetDto spriteSheetToUnload = projectController_.GetSpriteSheet(animationFrameViewerControl_.Animation.SpriteSheetId);

                        SpriteSheetDto spriteSheetToLoad = projectController_.GetSpriteSheet(animationProxy.SpriteSheetId);

                        // Unload the existing tile sheet, if it is changing.
                        Guid resourceToUnloadId = spriteSheetToUnload.BitmapResourceId;

                        Guid resourceToLoadId = Guid.Empty;

                        if (spriteSheetToLoad != null)
                        {
                            resourceToLoadId = spriteSheetToLoad.BitmapResourceId;
                        }

                        if (resourceToUnloadId != resourceToLoadId)
                        {
                            projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.AnimationFrameViewer);
                        }
                    }

                    animationFrameViewerControl_.Animation = animationProxy;
                }
                
                OnAnimationSelectionChanged(new AnimationSelectionChangedEventArgs(false, false, false, false, false, false));
            }
            else if (tvAnimations.SelectedNode.Name == "ANIMATION")
            {
                IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

                //// Set the attributes to read/write.
                //PropertyDescriptor prop = TypeDescriptor.GetProperties(animationProxy.GetType())["Name"];

                //ReadOnlyAttribute attr = (ReadOnlyAttribute)(prop.Attributes[typeof(ReadOnlyAttribute)]);

                //Type attrType = attr.GetType();

                //FieldInfo fi = attrType.GetField("isReadOnly", BindingFlags.NonPublic | BindingFlags.Instance);

                //fi.SetValue(attr, false);                

                pgAnimation.SelectedObject = animationProxy;
                
                if (animationFrameViewerControl_.Animation != null && animationFrameViewerControl_.Animation.SpriteSheetId != Guid.Empty)
                {
                    SpriteSheetDto spriteSheetToUnload = projectController_.GetSpriteSheet(animationFrameViewerControl_.Animation.SpriteSheetId);

                    SpriteSheetDto spriteSheetToLoad = projectController_.GetSpriteSheet(animationProxy.SpriteSheetId);

                    // Unload the existing tile sheet, if it is changing.
                    Guid resourceToUnloadId = spriteSheetToUnload.BitmapResourceId;

                    Guid resourceToLoadId = Guid.Empty;

                    if (spriteSheetToLoad != null)
                    {
                        resourceToLoadId = spriteSheetToLoad.BitmapResourceId;
                    }

                    if (resourceToUnloadId != resourceToLoadId)
                    {
                        projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.AnimationFrameViewer);
                    }
                }

                animationFrameViewerControl_.Animation = animationProxy;
                
                animationFrameViewerControl_.FrameIndex = 0;
                
                OnAnimationSelectionChanged(new AnimationSelectionChangedEventArgs(true, false, false, false, false, false));
            }
            else if (tvAnimations.SelectedNode.Name == "ANIMATIONFRAME")
            {
                IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Parent.Tag).Asset;

                IFrameDtoProxy frameProxy = (IFrameDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

                pgAnimation.SelectedObject = frameProxy;

                if (animationFrameViewerControl_.Animation != null && animationFrameViewerControl_.Animation.SpriteSheetId != Guid.Empty)
                {
                    SpriteSheetDto spriteSheetToUnload = projectController_.GetSpriteSheet(animationFrameViewerControl_.Animation.SpriteSheetId);

                    SpriteSheetDto spriteSheetToLoad = projectController_.GetSpriteSheet(animationProxy.SpriteSheetId);

                    // Unload the existing tile sheet, if it is changing.
                    Guid resourceToUnloadId = spriteSheetToUnload.BitmapResourceId;

                    Guid resourceToLoadId = Guid.Empty;

                    if (spriteSheetToLoad != null)
                    {
                        resourceToLoadId = spriteSheetToLoad.BitmapResourceId;
                    }

                    if (resourceToUnloadId != resourceToLoadId)
                    {
                        projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.AnimationFrameViewer);
                    }
                }

                animationFrameViewerControl_.Animation = animationProxy;

                animationFrameViewerControl_.FrameIndex = tvAnimations.SelectedNode.Index;
                
                OnAnimationSelectionChanged(new AnimationSelectionChangedEventArgs(true, true, true, false, false, false));
            }
            else if (tvAnimations.SelectedNode.Name == "HITBOXROOT" || tvAnimations.SelectedNode.Name == "FRAMETRIGGERROOT" || tvAnimations.SelectedNode.Name == "ACTIONPOINTROOT")
            {
                IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Parent.Parent.Tag).Asset;

                if (animationFrameViewerControl_.Animation != null && animationFrameViewerControl_.Animation.SpriteSheetId != Guid.Empty)
                {
                    SpriteSheetDto spriteSheetToUnload = projectController_.GetSpriteSheet(animationFrameViewerControl_.Animation.SpriteSheetId);

                    SpriteSheetDto spriteSheetToLoad = projectController_.GetSpriteSheet(animationProxy.SpriteSheetId);

                    // Unload the existing tile sheet, if it is changing.
                    Guid resourceToUnloadId = spriteSheetToUnload.BitmapResourceId;

                    Guid resourceToLoadId = Guid.Empty;

                    if (spriteSheetToLoad != null)
                    {
                        resourceToLoadId = spriteSheetToLoad.BitmapResourceId;
                    }

                    if (resourceToUnloadId != resourceToLoadId)
                    {
                        projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.AnimationFrameViewer);
                    }
                }

                animationFrameViewerControl_.Animation = animationProxy;

                OnAnimationSelectionChanged(new AnimationSelectionChangedEventArgs(true, false, true, false, false, false));
                
                animationFrameViewerControl_.FrameIndex = tvAnimations.SelectedNode.Parent.Index;
                
                pgAnimation.SelectedObject = null;
            }
            else if (tvAnimations.SelectedNode.Name == "HITBOX")
            {
                IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Parent.Parent.Parent.Tag).Asset;

                IHitboxDtoProxy hitboxProxy = (IHitboxDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

                pgAnimation.SelectedObject = hitboxProxy;

                if (animationFrameViewerControl_.Animation != null && animationFrameViewerControl_.Animation.SpriteSheetId != Guid.Empty)
                {
                    SpriteSheetDto spriteSheetToUnload = projectController_.GetSpriteSheet(animationFrameViewerControl_.Animation.SpriteSheetId);

                    SpriteSheetDto spriteSheetToLoad = projectController_.GetSpriteSheet(animationProxy.SpriteSheetId);

                    // Unload the existing tile sheet, if it is changing.
                    Guid resourceToUnloadId = spriteSheetToUnload.BitmapResourceId;

                    Guid resourceToLoadId = Guid.Empty;

                    if (spriteSheetToLoad != null)
                    {
                        resourceToLoadId = spriteSheetToLoad.BitmapResourceId;
                    }

                    if (resourceToUnloadId != resourceToLoadId)
                    {
                        projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.AnimationFrameViewer);
                    }
                }

                animationFrameViewerControl_.Animation = animationProxy;
                
                animationFrameViewerControl_.FrameIndex = tvAnimations.SelectedNode.Parent.Parent.Index;

                animationFrameViewerControl_.HitboxIndex = tvAnimations.SelectedNode.Index;

                OnAnimationSelectionChanged(new AnimationSelectionChangedEventArgs(true, false, true, true, false, false));
            }
            else if (tvAnimations.SelectedNode.Name == "FRAMETRIGGER")
            {
                IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Parent.Parent.Parent.Tag).Asset;

                IFrameTriggerDtoProxy frameTriggerProxy = (IFrameTriggerDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

                pgAnimation.SelectedObject = frameTriggerProxy;

                if (animationFrameViewerControl_.Animation != null && animationFrameViewerControl_.Animation.SpriteSheetId != Guid.Empty)
                {
                    SpriteSheetDto spriteSheetToUnload = projectController_.GetSpriteSheet(animationFrameViewerControl_.Animation.SpriteSheetId);

                    SpriteSheetDto spriteSheetToLoad = projectController_.GetSpriteSheet(animationProxy.SpriteSheetId);

                    // Unload the existing tile sheet, if it is changing.
                    Guid resourceToUnloadId = spriteSheetToUnload.BitmapResourceId;

                    Guid resourceToLoadId = Guid.Empty;

                    if (spriteSheetToLoad != null)
                    {
                        resourceToLoadId = spriteSheetToLoad.BitmapResourceId;
                    }

                    if (resourceToUnloadId != resourceToLoadId)
                    {
                        projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.AnimationFrameViewer);
                    }
                }

                animationFrameViewerControl_.Animation = animationProxy;
                
                animationFrameViewerControl_.FrameIndex = tvAnimations.SelectedNode.Parent.Parent.Index;
                
                OnAnimationSelectionChanged(new AnimationSelectionChangedEventArgs(true, false, true, false, true, false));
            }
            else if (tvAnimations.SelectedNode.Name == "ACTIONPOINT")
            {
                IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Parent.Parent.Parent.Tag).Asset;

                IActionPointDtoProxy actionPointProxy = (IActionPointDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

                pgAnimation.SelectedObject = actionPointProxy;

                if (animationFrameViewerControl_.Animation != null && animationFrameViewerControl_.Animation.SpriteSheetId != Guid.Empty)
                {
                    SpriteSheetDto spriteSheetToUnload = projectController_.GetSpriteSheet(animationFrameViewerControl_.Animation.SpriteSheetId);

                    SpriteSheetDto spriteSheetToLoad = projectController_.GetSpriteSheet(animationProxy.SpriteSheetId);

                    // Unload the existing tile sheet, if it is changing.
                    Guid resourceToUnloadId = spriteSheetToUnload.BitmapResourceId;

                    Guid resourceToLoadId = Guid.Empty;

                    if (spriteSheetToLoad != null)
                    {
                        resourceToLoadId = spriteSheetToLoad.BitmapResourceId;
                    }

                    if (resourceToUnloadId != resourceToLoadId)
                    {
                        projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.AnimationFrameViewer);
                    }
                }

                animationFrameViewerControl_.Animation = animationProxy;

                animationFrameViewerControl_.ActionPointIndex = tvAnimations.SelectedNode.Index;

                animationFrameViewerControl_.FrameIndex = tvAnimations.SelectedNode.Parent.Parent.Index;
                
                OnAnimationSelectionChanged(new AnimationSelectionChangedEventArgs(true, false, true, false, false, true));
            }
            else
            {
                pgAnimation.SelectedObject = null;
            }

            animationFrameViewerControl_.RefreshImage();
        }

        private void pgAnimation_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":

                    // Update the tree view with the new name.
                    string newName = e.ChangedItem.Value.ToString();

                    tvAnimations.SelectedNode.Text = newName;

                    // Sorting sorts all child nodes, so this won't work. I only want to sort the top level nodes.
                    // Ultimate I will need a better UI solution than a TreeView.
                    //tvAnimations.Sort();

                    // Sorting may unselect the node. Re-select it.
                    foreach (TreeNode node in tvAnimations.Nodes)
                    {
                        if (node.Text == newName)
                        {
                            tvAnimations.SelectedNode = node;

                            break;
                        }
                    }                   

                    break;

                case "SPRITESHEET":

                    IAnimationDtoProxy animationProxy = (IAnimationDtoProxy)((AssetMenuDto)tvAnimations.SelectedNode.Tag).Asset;

                    animationFrameViewerControl_.Animation = animationProxy;
                    
                    break;

                case "SHEETCELLINDEX":
                    
                    animationFrameViewerControl_.RefreshImage();

                    break;
            }
        }

        #endregion

        #region Event Dispatchers

        protected virtual void OnAnimationSelectionChanged(AnimationSelectionChangedEventArgs e)
        {
            AnimationSelectionChanged(this, e);
        }

        #endregion
    }

    #region Event Args

    public class AnimationSelectionChangedEventArgs : System.EventArgs
    {
        #region Constructors

        public AnimationSelectionChangedEventArgs(bool canAddFrame, bool canDeleteFrame, bool canAddFrameChild, bool canDeleteHitbox, bool canDeleteFrameTrigger, bool canDeleteActionPoint)
        {
            canAddFrame_ = canAddFrame;

            canDeleteFrame_ = canDeleteFrame;

            canAddFrameChild_ = canAddFrameChild;

            canDeleteHitbox_ = canDeleteHitbox;

            canDeleteFrameTrigger_ = canDeleteFrameTrigger;

            canDeleteActionPoint_ = canDeleteActionPoint;
        }

        #endregion

        #region Private Variables

        private bool canAddFrameChild_;

        #endregion

        #region Properties

        public bool CanAddFrame
        {
            get { return canAddFrame_; }
        }
        private bool canAddFrame_;

        public bool CanAddActionPoint
        {
            get { return canAddFrameChild_; }
        }

        public bool CanAddFrameTrigger
        {
            get { return canAddFrameChild_; }
        }

        public bool CanAddHitbox
        {
            get { return canAddFrameChild_; }
        }

        public bool CanDeleteFrame
        {
            get { return canDeleteFrame_; }
        }
        private bool canDeleteFrame_;

        public bool CanDeleteHitbox
        {
            get { return canDeleteHitbox_; }
        }
        bool canDeleteHitbox_ = false;

        public bool CanDeleteFrameTrigger
        {
            get { return canDeleteFrameTrigger_; }
        }
        bool canDeleteFrameTrigger_ = false;

        public bool CanDeleteActionPoint
        {
            get { return canDeleteActionPoint_; }
        }
        bool canDeleteActionPoint_ = false;

        #endregion
    }

    #endregion
}
