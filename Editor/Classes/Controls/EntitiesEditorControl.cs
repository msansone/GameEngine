using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using System.IO;
    using ProjectDto = ProjectDto_2_2;

    #region Delegates

    public delegate void EntitySelectionChangedHandler(object sender, EntitySelectionChangedEventArgs e);

    #endregion

    #region Enums

    public enum EntityType
    {
        None = 0,
        Actor = 1,
        Event = 2,
        HudElement = 3
    }

    #endregion

    public partial class EntitiesEditorControl : UserControl, IEntitiesEditorControl
    {
        #region Events

        public event EntitySelectionChangedHandler EntitySelectionChanged;

        #endregion

        #region Constructors

        public EntitiesEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            InitializeComponent();

            nameGenerator_ = nameGenerator;
            
            projectController_ = projectController;

            exceptionHandler_ = exceptionHandler;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            scriptGeneratorFactory_ = new ScriptGeneratorFactory();

            utilityFactory_ = new UtilityFactory();

            // Entity root nodes.
            tvEntities.Nodes.Add("ACTORROOT", "Actors");//.Tag = new AssetMenuDto(cmnuActor, null);

            tvEntities.Nodes.Add("EVENTROOT", "Events");///.Tag = new AssetMenuDto(cmnuEvent, null);

            tvEntities.Nodes.Add("HUDELEMENTROOT", "HUD Elements");//.Tag = new AssetMenuDto(cmnuHudElement, null);

            pythonScriptEditorControl_ = firemelonEditorFactory_.NewPythonScriptEditorControl(projectController);

            ((Control)pythonScriptEditorControl_).Dock = DockStyle.Fill;

            scEntities.Panel2.Controls.Add((Control)pythonScriptEditorControl_);

            ((Control)pythonScriptEditorControl_).Visible = false;
            
            stateEditorControl_ = firemelonEditorFactory_.NewStateEditorControl(projectController, exceptionHandler_);

            Control stateEditorControl = (Control)stateEditorControl_;

            stateEditorControl.Visible = false;

            stateEditorControl.Dock = DockStyle.Fill;

            scEntities.Panel2.Controls.Add(stateEditorControl);
            
            ProjectDto project = projectController.GetProjectDto();

            // Build the tree.
            foreach (ActorDto actor in project.Actors)
            {
                TreeNode actorNode = addActorToTree(actor);

                foreach (StateDto state in project.States[actor.Id])
                {
                    TreeNode stateNode = addStateToTree(actorNode, state);

                    foreach (AnimationSlotDto animationSlot in project.AnimationSlots[state.Id])
                    {
                        addAnimationSlotToTree(stateNode, animationSlot);
                    }

                    foreach (HitboxDto hitbox in project.Hitboxes[state.Id])
                    {
                        addHitboxToTree(stateNode, hitbox);
                    }
                }

                foreach (PropertyDto property in project.Properties[actor.Id])
                {
                    addPropertyToTree(actorNode, property);
                }

            }

            foreach (EventDto gameEvent in project.Events)
            {
                TreeNode eventNode = addEventToTree(gameEvent);

                foreach (PropertyDto property in project.Properties[gameEvent.Id])
                {
                    addPropertyToTree(eventNode, property);
                }
            }

            foreach (HudElementDto hudElement in project.HudElements)
            {
                TreeNode hudElementNode = addHudElementToTree(hudElement);

                foreach (StateDto state in project.States[hudElement.Id])
                {
                    TreeNode stateNode = addStateToTree(hudElementNode, state);

                    foreach (AnimationSlotDto animationSlot in project.AnimationSlots[state.Id])
                    {
                        addAnimationSlotToTree(stateNode, animationSlot);
                    }                    
                }

                foreach (PropertyDto property in project.Properties[hudElement.Id])
                {
                    addPropertyToTree(hudElementNode, property);
                }
            }
        }

        #endregion

        #region Private Variables
        
        private IExceptionHandler          exceptionHandler_;

        private IFiremelonEditorFactory    firemelonEditorFactory_;

        private INameGenerator             nameGenerator_;

        private IProjectController         projectController_;

        private IPythonScriptEditorControl pythonScriptEditorControl_;

        private IScriptGeneratorFactory    scriptGeneratorFactory_;

        private Guid                       selectedEntityId_;

        private IStateEditorControl        stateEditorControl_ = null;

        private IUtilityFactory            utilityFactory_;

        #endregion

        #region Public Functions

        public void AddNew()
        {
            switch (getEntityTypeForNode(tvEntities.SelectedNode))
            {
                case EntityType.Actor:

                    addNewActor();

                    break;

                case EntityType.Event:

                    addNewEvent();

                    break;

                case EntityType.HudElement:

                    addNewHudElement();
                    
                    break;
            }
        }

        public void AddAnimationSlot()
        {
            TreeNode stateNode = getSelectedStateTreeNode();

            if (stateNode != null)
            {
                IBaseDtoProxy state = (IBaseDtoProxy)((AssetMenuDto)stateNode.Tag).Asset;

                addAnimationSlot(state.Id);
            }
        }

        public void AddHitbox()
        {
            TreeNode stateNode = getSelectedStateTreeNode();

            if (stateNode != null)
            {
                IBaseDtoProxy state = (IBaseDtoProxy)((AssetMenuDto)stateNode.Tag).Asset;

                addHitbox(state.Id);
            }
        }

        public void AddProperty()
        {
            TreeNode entityNode = getSelectedEntityTreeNode();

            if (getCanAddPropertiesForNode(entityNode) == true)
            {
                IBaseDtoProxy entity = (IBaseDtoProxy)((AssetMenuDto)entityNode.Tag).Asset;

                addProperty(entity.Id);
            }
        }

        public void AddState()
        {
            TreeNode entityNode = getSelectedEntityTreeNode();

            if (getCanAddStatesForNode(entityNode) == true)
            {
                IBaseDtoProxy entity = (IBaseDtoProxy)((AssetMenuDto)entityNode.Tag).Asset;
                
                addState(entity.Id);
            }
        }

        public void Delete()
        {
            switch (getEntityTypeForNode(tvEntities.SelectedNode))
            {
                case EntityType.Actor:
                    
                    deleteActor();

                    break;

                case EntityType.Event:

                    deleteEvent();

                    break;

                case EntityType.HudElement:

                    deleteHudElement();

                    break;
            }
        }

        public void DeleteAnimationSlot()
        {
            IAnimationSlotDtoProxy animationSlot = (IAnimationSlotDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

            deleteAnimationSlot(animationSlot.Id);
        }

        public void DeleteHitbox()
        {
            IHitboxDtoProxy hitbox = (IHitboxDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

            deleteHitbox(hitbox.Id);
        }

        public void DeleteState()
        {
            IStateDtoProxy state = (IStateDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

            deleteState(state.Id);
        }

        #endregion
        
        #region Private Functions

        private void addNewActor()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int actorCount = tvEntities.Nodes["ACTORROOT"].Nodes.Count + 1;

            string name = getNextAvailableName("NewActor");

            ActorDto newActor = projectController_.AddActor(name);

            addActorToTree(newActor);

            if (project.Actors.Count == 1)
            {
                projectController_.SelectActor(0);
            }
        }

        private TreeNode addActorToTree(ActorDto actor)
        {
            IActorDtoProxy actorProxy = firemelonEditorFactory_.NewActorProxy(projectController_, actor.Id);

            // If this actor already has a node on the tree, ignore it.
            foreach (TreeNode node in tvEntities.Nodes["ACTORROOT"].Nodes)
            {
                IActorDtoProxy nodeProxy = (IActorDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == actorProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode actorNode = tvEntities.Nodes["ACTORROOT"].Nodes.Add("ACTOR", actor.Name);
            actorNode.Tag = new AssetMenuDto(cmnuActor, actorProxy);

            actorNode.Nodes.Add("STATEROOT", "States").Tag = new AssetMenuDto(cmnuStateRoot, null);

            actorNode.Nodes.Add("PROPERTYROOT", "Properties").Tag = new AssetMenuDto(cmnuPropertyRoot, null);

            ProjectDto project = projectController_.GetProjectDto();

            ScriptDto script = project.Scripts[actor.Id];

            IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

            actorNode.Nodes.Add("SCRIPT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);

            return actorNode;
        }
        
        private void addNewEvent()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int eventCount = tvEntities.Nodes["EVENTROOT"].Nodes.Count + 1;

            string name = getNextAvailableName("NewEvent");

            EventDto newEvent = projectController_.AddEvent(name);

            addEventToTree(newEvent);

            if (project.Events.Count == 1)
            {
                projectController_.SelectEvent(0);
            }
        }

        private TreeNode addEventToTree(EventDto newEvent)
        {
            IEventDtoProxy eventProxy = firemelonEditorFactory_.NewEventProxy(projectController_, newEvent.Id);

            // If this event already has a node on the tree, ignore it.
            foreach (TreeNode node in tvEntities.Nodes["EVENTROOT"].Nodes)
            {
                IEventDtoProxy nodeProxy = (IEventDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == eventProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode eventNode = tvEntities.Nodes["EVENTROOT"].Nodes.Add("EVENT", newEvent.Name);
            eventNode.Tag = new AssetMenuDto(cmnuEvent, eventProxy);

            eventNode.Nodes.Add("PROPERTYROOT", "Properties").Tag = new AssetMenuDto(cmnuPropertyRoot, null);

            ProjectDto project = projectController_.GetProjectDto();

            ScriptDto script = project.Scripts[newEvent.Id];

            IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

            eventNode.Nodes.Add("SCRIPT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);

            return eventNode;
        }

        private HitboxDto addHitbox(Guid stateId)
        {
            StateDto state = projectController_.GetState(stateId);

            HitboxDto newHitbox = projectController_.AddHitbox(stateId, state.OwnerId);

            TreeNode stateNode = getStateTreeNode(stateId);

            addHitboxToTree(stateNode, newHitbox);

            return newHitbox;
        }

        private void addHitbox(HitboxDto hitbox)
        {
            projectController_.AddHitbox(hitbox);

            TreeNode stateNode = getStateTreeNode(hitbox.OwnerId);

            addHitboxToTree(stateNode, hitbox);            
        }

        private TreeNode addHitboxToTree(TreeNode stateNode, HitboxDto hitbox)
        {
            IHitboxDtoProxy hitboxProxy = firemelonEditorFactory_.NewHitboxProxy(projectController_, hitbox.Id);

            ProjectDto project = projectController_.GetProjectDto();
            
            string nodeName = hitbox.Name;

            string nodeKey = "HITBOX";

            hitbox.Name = nodeName;
            
            TreeNode hitboxRootNode = stateNode.Nodes["HITBOXROOT"].Nodes.Add(nodeKey, nodeName);

            hitboxRootNode.Tag = new AssetMenuDto(cmnuHitbox, hitboxProxy);
            
            return hitboxRootNode;
        }

        private void addNewHudElement()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int hudElementCount = tvEntities.Nodes["HUDELEMENTROOT"].Nodes.Count + 1;

            string name = getNextAvailableName("NewHudElement");

            HudElementDto newHudElement = projectController_.AddHudElement(name);

            addHudElementToTree(newHudElement);

            if (project.HudElements.Count == 1)
            {
                projectController_.SelectHudElement(0);
            }
        }

        private TreeNode addHudElementToTree(HudElementDto hudElement)
        {
            IHudElementDtoProxy hudElementProxy = firemelonEditorFactory_.NewHudElementProxy(projectController_, hudElement.Id);

            // If this HUD element already has a node on the tree, ignore it.
            foreach (TreeNode node in tvEntities.Nodes["HUDELEMENTROOT"].Nodes)
            {
                IHudElementDtoProxy nodeProxy = (IHudElementDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == hudElementProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode hudElementNode = tvEntities.Nodes["HUDELEMENTROOT"].Nodes.Add("HUDELEMENT", hudElement.Name);

            hudElementNode.Tag = new AssetMenuDto(cmnuHudElement, hudElementProxy);

            hudElementNode.Nodes.Add("STATEROOT", "States").Tag = new AssetMenuDto(cmnuStateRoot, null);

            hudElementNode.Nodes.Add("PROPERTYROOT", "Properties").Tag = new AssetMenuDto(cmnuPropertyRoot, null);

            ProjectDto project = projectController_.GetProjectDto();

            ScriptDto script = project.Scripts[hudElement.Id];

            IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

            hudElementNode.Nodes.Add("SCRIPT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);

            return hudElementNode;
        }

        private void addAnimationSlot(Guid stateId)
        {
            AnimationSlotDto newAnimationSlot = projectController_.AddAnimationSlot(stateId);

            newAnimationSlot.Name = getNextAvailableSlotName("Animation Slot ", stateId);

            TreeNode stateNode = getStateTreeNode(stateId);

            addAnimationSlotToTree(stateNode, newAnimationSlot);
        }

        private TreeNode addAnimationSlotToTree(TreeNode stateNode, AnimationSlotDto animationSlot)
        {
            IAnimationSlotDtoProxy animationSlotProxy = firemelonEditorFactory_.NewAnimationSlotProxy(projectController_, animationSlot.Id);

            ProjectDto project = projectController_.GetProjectDto();

            int animationSlotCount = project.AnimationSlots[animationSlotProxy.OwnerId].Count;

            //selectedAnimationSlotIndex_ = animationSlotCount - 1;

            string nodeName = animationSlot.Name;

            string nodeKey = "ANIMATIONSLOT";

            TreeNode animatinSlotRootNode = stateNode.Nodes["ANIMATIONSLOTROOT"].Nodes.Add(nodeKey, nodeName);

            animatinSlotRootNode.Tag = new AssetMenuDto(cmnuAnimationSlot, animationSlotProxy);

            return animatinSlotRootNode;

            //tvState.SelectedNode = node;
        }

        private void addProperty(Guid entityId)
        {
            ProjectDto project = projectController_.GetProjectDto();

            int propertyCount = project.Properties[entityId].Count;

            bool isNameValid = false;
            int counter = 0;

            string currentName = "New Property";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Property " + counter.ToString();
                }

                for (int j = 0; j < propertyCount; j++)
                {
                    string propertyName = project.Properties[entityId][j].Name;

                    if (currentName.ToUpper() == propertyName.ToUpper())
                    {
                        isNameValid = false;
                        break;
                    }
                }

                counter++;
            }

            PropertyDto newProperty = projectController_.AddProperty(entityId, currentName);

            TreeNode entityNode = getEntityTreeNode(entityId);

            addPropertyToTree(entityNode, newProperty);
        }

        private TreeNode addPropertyToTree(TreeNode entityNode, PropertyDto property)
        {
            IPropertyDtoProxy propertyProxy = firemelonEditorFactory_.NewPropertyProxy(projectController_, property.Id);

            // If this state already has a node on the tree, ignore it.
            foreach (TreeNode node in entityNode.Nodes["PROPERTYROOT"].Nodes)
            {
                IPropertyDtoProxy nodeProxy = (IPropertyDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == propertyProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode propertyNode = entityNode.Nodes["PROPERTYROOT"].Nodes.Add("PROPERTY", property.Name);

            propertyNode.Tag = new AssetMenuDto(cmnuProperty, propertyProxy);

            return propertyNode;
        }

        private void addState(Guid entityId)
        {
            ProjectDto project = projectController_.GetProjectDto();

            int stateCount = project.States[entityId].Count;

            bool isNameValid = false;
            int counter = 0;

            string currentName = "New State";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New State " + counter.ToString();
                }

                for (int j = 0; j < stateCount; j++)
                {
                    string stateName = project.States[entityId][j].Name;

                    if (currentName.ToUpper() == stateName.ToUpper())
                    {
                        isNameValid = false;

                        break;
                    }
                }

                counter++;
            }

            StateDto newState = projectController_.AddState(entityId, currentName);

            TreeNode entityNode = getEntityTreeNode(entityId);

            addStateToTree(entityNode, newState);
        }

        private TreeNode addStateToTree(TreeNode entityNode, StateDto state)
        {
            IStateDtoProxy stateProxy = firemelonEditorFactory_.NewStateProxy(projectController_, state.Id);

            // If this state already has a node on the tree, ignore it.
            foreach (TreeNode node in entityNode.Nodes["STATEROOT"].Nodes)
            {
                IStateDtoProxy nodeProxy = (IStateDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == stateProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode stateNode = entityNode.Nodes["STATEROOT"].Nodes.Add("STATE", state.Name);

            stateNode.Tag = new AssetMenuDto(cmnuState, stateProxy);

            TreeNode animationSlotRootNode = stateNode.Nodes.Add("ANIMATIONSLOTROOT", "Animation Slots");

            animationSlotRootNode.Tag = new AssetMenuDto(cmnuAnimationSlotRoot, null);

            if (entityNode.Name == "ACTOR")
            {
                TreeNode hitboxRootNode = stateNode.Nodes.Add("HITBOXROOT", "Hitboxes");

                hitboxRootNode.Tag = new AssetMenuDto(cmnuHitboxRoot, null);
            }

            return stateNode;
        }
        
        private void buildOtherStateList(Guid entityId)
        {
            copyHitboxesFromStateToolStripMenuItem.DropDownItems.Clear();

            ProjectDto project = projectController_.GetProjectDto();

            foreach (StateDto state in project.States[entityId])
            {
                if (state.Id != entityId)
                {
                    ToolStripMenuItem newMenuItem = new ToolStripMenuItem();

                    newMenuItem.Text = state.Name;
                    newMenuItem.Tag = state.Id;
                    newMenuItem.Click += mnuCopyHitboxesFromState_Click;

                    copyHitboxesFromStateToolStripMenuItem.DropDownItems.Add(newMenuItem);

                }
            }
        }

        private void deleteActor()
        {
            TreeNode entityNode = getSelectedEntityTreeNode();

            // Delete the selected actor, and all associated components. 
            if (MessageBox.Show("Delete Actor?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                projectController_.DeleteActor(tvEntities.SelectedNode.Index);

                tvEntities.SelectedNode.Remove();
            }
        }

        private void deleteAnimationSlot(Guid animationSlotId)
        {
            projectController_.DeleteAnimationSlot(animationSlotId);

            tvEntities.SelectedNode.Remove();
        }

        private void deleteEvent()
        {
            // Delete the selected event, and all associated components. 

        }

        private void deleteHitbox(Guid hitboxId)
        {
            projectController_.DeleteHitbox(hitboxId);

            tvEntities.SelectedNode.Remove();
        }

        private void deleteHudElement()
        {
            // Delete the selected HUD element, and all associated components. 

        }

        private void deleteState(Guid stateId)
        {
            projectController_.DeleteState(stateId);

            tvEntities.SelectedNode.Remove();
        }

        private void generateNewScript(IScriptDtoProxy scriptProxy)
        {
            ScriptDto script = projectController_.GetScript(scriptProxy.Id);

            SaveFileDialog saveScript = new SaveFileDialog();

            saveScript.DefaultExt = "py";
            saveScript.AddExtension = true;
            saveScript.RestoreDirectory = true;
            saveScript.Filter = "Python Files|*.py";
            saveScript.FileName = script.Name.ToLower() + ".py";
            saveScript.RestoreDirectory = true;

            string fileName = string.Empty;

            if (saveScript.ShowDialog() == DialogResult.OK)
            {
                fileName = saveScript.FileName;

                IScriptGenerator scriptGenerator = scriptGeneratorFactory_.NewScriptGenerator(script);

                string scriptCode = scriptGenerator.Generate(script);

                File.WriteAllText(fileName, scriptCode);

                scriptProxy.ScriptPath = fileName;
            }
        }

        private TreeNode getEntityTreeNode(Guid entityId)
        {
            foreach (TreeNode rootNode in tvEntities.Nodes)
            {
                foreach (TreeNode node in rootNode.Nodes)
                {
                    IEntityDtoProxy nodeProxy = (IEntityDtoProxy)((AssetMenuDto)node.Tag).Asset;

                    if (nodeProxy.Id == entityId)
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        private TreeNode getStateTreeNode(Guid stateId)
        {
            foreach (TreeNode rootNode in tvEntities.Nodes)
            {
                foreach (TreeNode entityNode in rootNode.Nodes)
                {
                    if (getCanAddStatesForNode(entityNode) == true)
                    {
                        foreach (TreeNode stateNode in entityNode.Nodes["STATEROOT"].Nodes)
                        {
                            IStateDtoProxy nodeProxy = (IStateDtoProxy)((AssetMenuDto)stateNode.Tag).Asset;

                            if (nodeProxy.Id == stateId)
                            {
                                return stateNode;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private bool getCanAddAnimationSlotsForNode(TreeNode entityNode)
        {
            switch (tvEntities.SelectedNode.Name)
            {
                case "STATE":
                case "ANIMATIONSLOTROOT":
                case "ANIMATIONSLOT":
                case "HITBOXROOT":
                case "HITBOX":

                    return true;
            }

            return false;
        }

        private bool getCanAddHitboxesForNode(TreeNode entityNode)
        {
            switch (tvEntities.SelectedNode.Name)
            {
                case "HITBOX":
                case "HITBOXROOT":

                    return true;

                case "STATE":

                    switch (tvEntities.SelectedNode.Parent.Parent.Name)
                    {
                        case "ACTOR":

                            return true;
                    }

                    break;
                    
                case "ANIMATIONSLOT":

                    switch (tvEntities.SelectedNode.Parent.Parent.Parent.Parent.Name)
                    {
                        case "ACTOR":

                            return true;
                    }

                    break;

                case "ANIMATIONSLOTROOT":

                    switch (tvEntities.SelectedNode.Parent.Parent.Parent.Name)
                    {
                        case "ACTOR":

                            return true;
                    }

                    break;
            }

            return false;
        }

        private bool getCanAddPropertiesForNode(TreeNode entityNode)
        {
            switch (tvEntities.SelectedNode.Name)
            {
                case "ACTORROOT":
                case "EVENTROOT":
                case "HUDELEMENTROOT":

                    return false;
            }

            return true;
        }

        // For any given node, return if the add states functionality is allowed..
        private bool getCanAddStatesForNode(TreeNode entityNode)
        {
            switch (entityNode?.Name)
            {
                case "ACTORROOT":
                case "EVENTROOT":
                case "HUDELEMENTROOT":
                case "EVENT":

                    return false;

                case "ACTOR":
                case "HUDELEMENT":
                case "STATEROOT":
                case "STATE":
                case "ANIMATIONSLOT":
                case "ANIMATIONSLOTROOT":
                case "HITBOX":
                case "HITBOXROOT":

                    return true;

                case "PROPERTYROOT":
                case "SCRIPT":

                    switch (entityNode.Parent.Name)
                    {
                        case "ACTOR":
                        case "HUDELEMENT":

                            return true;
                    }

                    return false;

                case "PROPERTY":

                    switch (entityNode.Parent.Parent.Name)
                    {
                        case "ACTOR":
                        case "HUDELEMENT":

                            return true;
                    }

                    return false;
            }

            return false;
        }

        private bool getCanDeleteAnimationSlotForNode(TreeNode entityNode)
        {
            switch (tvEntities.SelectedNode.Name)
            {
                case "ANIMATIONSLOT":

                    return true;
            }

            return false;
        }

        private bool getCanDeleteHitboxForNode(TreeNode entityNode)
        {
            switch (tvEntities.SelectedNode.Name)
            {
                case "HITBOX":
               
                    return true;
            }

            return false;
        }

        private bool getCanDeleteEntityForNode(TreeNode entityNode)
        {
            switch (tvEntities.SelectedNode.Name)
            {
                case "ACTORROOT":
                case "EVENTROOT":
                case "HUDELEMENTROOT":

                    return false;

                case "ACTOR":
                case "EVENT":
                case "HUDELEMENT":
                case "STATEROOT":
                case "STATE":
                case "ANIMATIONSLOT":
                case "ANIMATIONSLOTROOT":
                case "HITBOX":
                case "HITBOXROOT":                    
                case "PROPERTYROOT":
                case "SCRIPT":                    
                case "PROPERTY":
                    
                    return true;                  
            }

            return false;
        }

        private bool getCanDeleteStatesForNode(TreeNode entityNode)
        {
            switch (tvEntities.SelectedNode.Name)
            {
                case "STATE":

                    return true;                
            }

            return false;
        }

        private EntityType getEntityTypeForNode(TreeNode entityNode)
        {
            switch (tvEntities.SelectedNode.Name)
            {
                case "ACTORROOT":
                case "ACTOR":

                    return EntityType.Actor;

                case "HUDELEMENTROOT":
                case "HUDELEMENT":

                    return EntityType.HudElement;

                case "EVENTROOT":
                case "EVENT":

                    return EntityType.Event;

                case "STATEROOT":
                case "PROPERTYROOT":
                case "SCRIPT":

                    switch (tvEntities.SelectedNode.Parent.Name)
                    {
                        case "ACTOR":

                            return EntityType.Actor;

                        case "HUDELEMENT":

                            return EntityType.HudElement;

                        case "EVENT":

                            return EntityType.Event;
                    }

                    break;


                case "STATE":
                case "PROPERTY":

                    switch (tvEntities.SelectedNode.Parent.Parent.Name)
                    {
                        case "ACTOR":

                            return EntityType.Actor;

                        case "HUDELEMENT":

                            return EntityType.HudElement;

                        case "EVENT":

                            return EntityType.Event;
                    }

                    break;

                case "ANIMATIONSLOTROOT":
                case "HITBOXROOT":

                    switch (tvEntities.SelectedNode.Parent.Parent.Parent.Name)
                    {
                        case "ACTOR":

                            return EntityType.Actor;

                        case "HUDELEMENT":

                            return EntityType.HudElement;

                        case "EVENT":

                            return EntityType.Event;
                    }

                    break;

                case "ANIMATIONSLOT":
                case "HITBOX":

                    switch (tvEntities.SelectedNode.Parent.Parent.Parent.Parent.Name)
                    {
                        case "ACTOR":

                            return EntityType.Actor;

                        case "HUDELEMENT":

                            return EntityType.HudElement;

                        case "EVENT":

                            return EntityType.Event;
                    }

                    break;
            }

            return EntityType.None;
        }

        private string getNextAvailableSlotName(string baseName, Guid stateId)
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

                isNameInUse = isSlotNameInUse(currentName, stateId);

                counter++;
            }

            return currentName.Trim();
        }

        private bool isSlotNameInUse(string name, Guid stateId)
        {
            ProjectDto project = projectController_.GetProjectDto();

            foreach (AnimationSlotDto animationSlot in project.AnimationSlots[stateId])
            {
                if (animationSlot.Name.ToUpper() == name.ToUpper())
                {
                    return true;
                }
            }

            return false;
        }

        private TreeNode getSelectedEntityTreeNode()
        {
            TreeNode entityTreeNode = null;

            switch (tvEntities.SelectedNode.Name)
            {
                case "ACTOR":
                case "HUDELEMENT":
                case "EVENT":

                    entityTreeNode = tvEntities.SelectedNode;

                    break;

                case "STATEROOT":
                case "PROPERTYROOT":
                case "SCRIPT":

                    switch (tvEntities.SelectedNode.Parent.Name)
                    {
                        case "ACTOR":
                        case "HUDELEMENT":
                        case "EVENT":

                            entityTreeNode = tvEntities.SelectedNode.Parent;

                            break;
                    }

                    break;

                case "STATE":
                case "PROPERTY":

                    switch (tvEntities.SelectedNode.Parent.Parent.Name)
                    {
                        case "ACTOR":
                        case "HUDELEMENT":
                        case "EVENT":

                            entityTreeNode = tvEntities.SelectedNode.Parent.Parent;

                            break;
                    }

                    break;

                case "ANIMATIONSLOTROOT":
                case "HITBOXROOT":

                    entityTreeNode = tvEntities.SelectedNode.Parent.Parent.Parent;

                    break;

                case "ANIMATIONSLOT":
                case "HITBOX":

                    entityTreeNode = tvEntities.SelectedNode.Parent.Parent.Parent.Parent;

                    break;
            }

            return entityTreeNode;
        }

        private TreeNode getSelectedStateTreeNode()
        {
            TreeNode entityTreeNode = null;

            switch (tvEntities.SelectedNode.Name)
            {
                case "STATE":

                    return tvEntities.SelectedNode;

                case "HITBOX":
                case "ANIMATIONSLOT":

                    return tvEntities.SelectedNode.Parent.Parent;

                case "ANIMATIONSLOTROOT":
                case "HITBOXROOT":

                    return tvEntities.SelectedNode.Parent;
            }

            return entityTreeNode;
        }

        private string getNextAvailableHitboxNameForState(Guid stateId)
        {
            bool isNameValid = false;
            int counter = 1;
            string baseName = "Hitbox ";

            string currentName = string.Empty;

            ProjectDto project = projectController_.GetProjectDto();

            while (isNameValid == false)
            {
                currentName = baseName + counter.ToString();

                bool nameFound = false;

                // Find the first sequentially available name.
                foreach (HitboxDto hitbox in project.Hitboxes[stateId])
                {
                    if (hitbox.Name == currentName)
                    {
                        nameFound = true;

                        break;
                    }
                }

                if (nameFound == false)
                {
                    // This name can be used.
                    isNameValid = true;
                }
                
                counter++;
            }

            return currentName;
        }

        private string getNextAvailableName(string baseName)
        {
            bool isNameValid = false;
            int counter = 0;
            string currentName = baseName;

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = baseName + counter.ToString();
                }

                isNameValid = !nameGenerator_.IsAssetNameInUse(Guid.Empty, projectController_.GetProjectDto(), currentName);

                counter++;
            }

            return currentName.Trim();
        }
        
        private void unloadBitmapResources(IStateDtoProxy stateProxy)
        {
            if (stateEditorControl_.State != null)
            {
                StateDto stateToUnload = projectController_.GetState(stateEditorControl_.State.Id);

                StateDto stateToLoad = projectController_.GetState(stateProxy.Id);

                // A state may have several animations. Unload the bitmap resource used by each of them,
                // *UNLESS* this bitmap resource is used by the current state.
                HashSet<Guid> resourceToUnloadIds = new HashSet<Guid>();

                HashSet<Guid> resourceToLoadIds = new HashSet<Guid>();

                ProjectDto project = projectController_.GetProjectDto();

                foreach (AnimationSlotDto animationSlot in project.AnimationSlots[stateToUnload.Id])
                {
                    if (animationSlot.Animation != Guid.Empty)
                    {
                        AnimationDto animation = projectController_.GetAnimation(animationSlot.Animation);

                        SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(animation.SpriteSheet);

                        resourceToUnloadIds.Add(spriteSheet.BitmapResourceId);
                    }
                }

                foreach (AnimationSlotDto animationSlot in project.AnimationSlots[stateToLoad.Id])
                {
                    if (animationSlot.Animation != Guid.Empty)
                    {
                        AnimationDto animation = projectController_.GetAnimation(animationSlot.Animation);

                        SpriteSheetDto spriteSheet = projectController_.GetSpriteSheet(animation.SpriteSheet);

                        resourceToLoadIds.Add(spriteSheet.BitmapResourceId);
                    }
                }

                foreach (Guid resourceToUnloadId in resourceToUnloadIds)
                {
                    if (resourceToLoadIds.Contains(resourceToUnloadId) == false)
                    {
                        projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.StateEditorControl);
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        private void mnuCopyHitboxesFromState_Click(object sender, EventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

            Guid copyFromStateId = (Guid)(menuItem.Tag);

            TreeNode stateNode = getSelectedStateTreeNode();

            IBaseDtoProxy copyToState = (IBaseDtoProxy)((AssetMenuDto)stateNode.Tag).Asset;
            
            foreach (HitboxDto hitbox in project.Hitboxes[copyFromStateId])
            {
                HitboxDto newHitbox = new HitboxDto();

                newHitbox.OwnerId = copyToState.Id;

                newHitbox.RootOwnerId = copyToState.OwnerId;

                newHitbox.CornerPoint1.X = hitbox.CornerPoint1.X;

                newHitbox.CornerPoint1.Y = hitbox.CornerPoint1.Y;

                newHitbox.CornerPoint2.X = hitbox.CornerPoint2.X;

                newHitbox.CornerPoint2.Y = hitbox.CornerPoint2.Y;

                newHitbox.HitboxRect = new Rectangle(hitbox.HitboxRect.Left, hitbox.HitboxRect.Top, hitbox.HitboxRect.Width, hitbox.HitboxRect.Height);

                newHitbox.Identity = hitbox.Identity;

                newHitbox.IsSolid = hitbox.IsSolid;

                newHitbox.Priority = hitbox.Priority;

                newHitbox.RotationDegrees = hitbox.RotationDegrees;

                string hitboxName = getNextAvailableHitboxNameForState(copyToState.Id);

                newHitbox.Name = hitboxName;

                addHitbox(newHitbox);
            }
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode stateNode = getSelectedStateTreeNode();

            if (stateNode != null)
            {
                IStateDtoProxy state = (IStateDtoProxy)((AssetMenuDto)stateNode.Tag).Asset;

                int currentSlotIndex = tvEntities.SelectedNode.Index;

                projectController_.MoveDownAnimationSlot(state.Id, currentSlotIndex);

                ProjectDto project = projectController_.GetProjectDto();

                int slotCount = project.AnimationSlots[state.Id].Count;

                if (currentSlotIndex < slotCount - 1)
                {
                    TreeNode node = tvEntities.SelectedNode;
                    TreeNode parent = node.Parent;

                    tvEntities.SelectedNode.Remove();

                    parent.Nodes.Insert(node.Index + 1, node);

                    tvEntities.SelectedNode = node;
                }
            }
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode stateNode = getSelectedStateTreeNode();

            if (stateNode != null)
            {
                IStateDtoProxy state = (IStateDtoProxy)((AssetMenuDto)stateNode.Tag).Asset;

                int currentSlotIndex = tvEntities.SelectedNode.Index;

                projectController_.MoveUpAnimationSlot(state.Id, currentSlotIndex);

                if (currentSlotIndex > 0)
                {
                    TreeNode node = tvEntities.SelectedNode;
                    TreeNode parent = node.Parent;

                    tvEntities.SelectedNode.Remove();

                    parent.Nodes.Insert(node.Index - 1, node);

                    tvEntities.SelectedNode = node;
                }
            }
        }

        private void pgProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":

                    // Update the tree view with the new name.
                    string newName = e.ChangedItem.Value.ToString();

                    tvEntities.SelectedNode.Text = newName;

                    break;

                case "STAGEHEIGHT":
                case "STAGEWIDTH":

                    stateEditorControl_.RefreshState(true);

                    break;

                case "LEFT":
                case "TOP":
                case "ANIMATIONORIGINLOCATION":
                case "STAGEORIGINLOCATION":
                case "ROTATIONDEGREES":
                case "HEIGHT":
                case "WIDTH":

                    stateEditorControl_.RefreshState(false);

                    break;
            }

        }

        private void tvEntities_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Regenerate the background when changing entities.
            bool regenerateBackground = false;

            TreeNode selectedEntityNode = getSelectedEntityTreeNode();
            
            if (selectedEntityNode != null)
            {
                selectedEntityId_ = Guid.Empty;
                
                switch (selectedEntityNode.Name)
                {
                    case "ACTOR":

                        IActorDtoProxy actorProxy = (IActorDtoProxy)((AssetMenuDto)selectedEntityNode.Tag).Asset;

                        GlobalVars.listOfStates_ = projectController_.GetStateNames(actorProxy.Id).ToArray();

                        if (selectedEntityId_ != actorProxy.Id)
                        {
                            regenerateBackground = true;

                            selectedEntityId_ = actorProxy.Id;

                            buildOtherStateList(selectedEntityId_);
                        }

                        break;

                    case "EVENT":

                        IEventDtoProxy eventProxy = (IEventDtoProxy)((AssetMenuDto)selectedEntityNode.Tag).Asset;

                        if (selectedEntityId_ != eventProxy.Id)
                        {
                            regenerateBackground = true;

                            selectedEntityId_ = eventProxy.Id;
                        }

                        break;

                    case "HUDELEMENT":

                        IHudElementDtoProxy hudElementProxy = (IHudElementDtoProxy)((AssetMenuDto)selectedEntityNode.Tag).Asset;

                        GlobalVars.listOfStates_ = projectController_.GetStateNames(hudElementProxy.Id).ToArray();

                        if (selectedEntityId_ != hudElementProxy.Id)
                        {
                            regenerateBackground = true;

                            selectedEntityId_ = hudElementProxy.Id;

                            buildOtherStateList(selectedEntityId_);
                        }

                        break;
                }
                            
            }
            
            EntityType selectedEntityType = getEntityTypeForNode(selectedEntityNode);

            bool canAddStates = getCanAddStatesForNode(selectedEntityNode);

            bool canAddProperties = getCanAddPropertiesForNode(selectedEntityNode);

            bool canAddAnimationSlots = getCanAddAnimationSlotsForNode(selectedEntityNode);

            bool canAddHitboxes = getCanAddHitboxesForNode(selectedEntityNode);

            bool canDeleteEntity = getCanDeleteEntityForNode(selectedEntityNode);

            bool canDeleteState = getCanDeleteStatesForNode(selectedEntityNode);

            bool canDeleteAnimationSlot = getCanDeleteAnimationSlotForNode(selectedEntityNode);

            bool canDeleteHitbox = getCanDeleteHitboxForNode(selectedEntityNode);

            OnEntitySelectionChanged(new EntitySelectionChangedEventArgs(selectedEntityType, 
                                                                         canAddStates, 
                                                                         canAddProperties, 
                                                                         canAddAnimationSlots, 
                                                                         canAddHitboxes, 
                                                                         canDeleteEntity, 
                                                                         canDeleteState, 
                                                                         canDeleteAnimationSlot,
                                                                         canDeleteHitbox));
            
            // Set the properties object and control visibilities.

            // Initialize all to default states.
            pgProperties.SelectedObject = null;

            //((Control)stateEditorControl_).Visible = false;

            //scintilla.Visible = false;

            IStateDtoProxy stateProxy;
            
            if (tvEntities.SelectedNode.Name == "ANIMATIONSLOT")
            {
                stateEditorControl_.SelectedAnimationSlotIndex = tvEntities.SelectedNode.Index;
            }
            else
            {
                stateEditorControl_.SelectedAnimationSlotIndex = -1;
            }
            
            switch (tvEntities.SelectedNode.Name)
            {
                case "ACTOR":

                    IActorDtoProxy actorProxy = (IActorDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = actorProxy;

                    ((Control)stateEditorControl_).Visible = false;

                    break;

                case "EVENT":

                    IEventDtoProxy eventProxy = (IEventDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = eventProxy;

                    ((Control)stateEditorControl_).Visible = false;

                    break;

                case "HUDELEMENT":

                    IHudElementDtoProxy hudElementProxy = (IHudElementDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = hudElementProxy;

                    ((Control)stateEditorControl_).Visible = false;

                    break;

                case "STATE":

                    stateProxy = (IStateDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

                    unloadBitmapResources(stateProxy);

                    pgProperties.SelectedObject = stateProxy;
                    
                    stateEditorControl_.State = stateProxy;

                    ((Control)stateEditorControl_).Visible = true;

                    ((Control)pythonScriptEditorControl_).Visible = false;

                    break;

                case "PROPERTY":

                    IPropertyDtoProxy propertyProxy = (IPropertyDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = propertyProxy;

                    break;

                case "ANIMATIONSLOT":
                    
                    stateProxy = (IStateDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Parent.Parent.Tag).Asset;
                    
                    unloadBitmapResources(stateProxy);

                    // Disable any refreshes when changing the state, because they are slow and multiple may occur.
                    // Instead, do just one refresh after the state has changed.
                    stateEditorControl_.LockRefresh();

                    stateEditorControl_.State = stateProxy;
                                        
                    IAnimationSlotDtoProxy animationSlotProxy = (IAnimationSlotDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;
                    
                    pgProperties.SelectedObject = animationSlotProxy;
                    
                    ((Control)stateEditorControl_).Visible = true;

                    ((Control)pythonScriptEditorControl_).Visible = false;

                    stateEditorControl_.UnlockRefresh();

                    break;

                case "ANIMATIONSLOTROOT":
                case "HITBOXROOT":

                    stateProxy = (IStateDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Parent.Tag).Asset;

                    unloadBitmapResources(stateProxy);

                    stateEditorControl_.State = stateProxy;
                    
                    ((Control)stateEditorControl_).Visible = true;

                    ((Control)pythonScriptEditorControl_).Visible = false;

                    break;

                case "HITBOX":

                    stateProxy = (IStateDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Parent.Parent.Tag).Asset;

                    unloadBitmapResources(stateProxy);

                    stateEditorControl_.State = stateProxy;
                    
                    ((Control)stateEditorControl_).Visible = true;

                    ((Control)pythonScriptEditorControl_).Visible = false;

                    IHitboxDtoProxy hitboxProxy = (IHitboxDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = hitboxProxy;
                    
                    break;

                case "SCRIPT":

                    IScriptDtoProxy scriptProxy = (IScriptDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = scriptProxy;
                    
                    ((Control)stateEditorControl_).Visible = false;

                    ((Control)pythonScriptEditorControl_).Visible = true;

                    pythonScriptEditorControl_.Script = scriptProxy;

                    break;

                default:

                    ((Control)stateEditorControl_).Visible = false;

                    ((Control)pythonScriptEditorControl_).Visible = false;

                    break;
            }
            
            if (((Control)stateEditorControl_).Visible == true)
            {
                // This should only be called if the entity, state, or animation slot has changed.
                stateEditorControl_.RefreshState(regenerateBackground);
            }      
        }

        private void tvEntities_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode selectedNode = tvEntities.SelectedNode;

            if (selectedNode?.Name == "SCRIPT")
            {
                DialogResult result = DialogResult.None;

                if (pythonScriptEditorControl_.ChangesMade == true)
                {
                    result = MessageBox.Show("Script contains unsaved changes. Do you want to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);
                }

                if (result == DialogResult.Yes)
                {
                    pythonScriptEditorControl_.Save();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;

                    return;
                }
            }
        }

        private void tvEntities_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (tvEntities.SelectedNode.Tag != null)
                {
                    ContextMenuStrip menu = ((AssetMenuDto)tvEntities.SelectedNode.Tag).Menu;

                    if (menu != null)
                    {
                        menu.Show(tvEntities, e.X, e.Y);
                    }
                }
            }
        }

        private void viewEditScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IScriptDtoProxy scriptProxy = (IScriptDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

            if (String.IsNullOrEmpty(scriptProxy.ScriptPath) == true)
            {
                string msg = "Script file not set. Generate a new script?";
                string caption = "Generate Script?";

                if (MessageBox.Show(msg, caption, MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    generateNewScript(scriptProxy);

                    pgProperties.Refresh();
                }
                else
                {
                    return;
                }
            }

            if (scriptProxy.ScriptPath != string.Empty)
            {
                System.Diagnostics.Process.Start(scriptProxy.ScriptPath);
            }
        }

        #endregion

        #region Event Dispatchers

        protected virtual void OnEntitySelectionChanged(EntitySelectionChangedEventArgs e)
        {
            EntitySelectionChanged(this, e);
        }

        #endregion

    }

    #region Event Args

    public class EntitySelectionChangedEventArgs : System.EventArgs
    {
        #region Constructors

        public EntitySelectionChangedEventArgs(EntityType entityType, 
                                               bool canAddStates, 
                                               bool canAddProperties, 
                                               bool canAddAnimationSlots, 
                                               bool canAddHitboxes, 
                                               bool canDeleteEntity, 
                                               bool canDeleteState,
                                               bool canDeleteAnimationSlot,
                                               bool canDeleteHitbox)
        {
            entityType_ = entityType;
            canAddStates_ = canAddStates;
            canAddProperties_ = canAddProperties;
            canAddAnimationSlots_ = canAddAnimationSlots;
            canAddHitboxes_ = canAddHitboxes;
            canDeleteEntity_ = canDeleteEntity;
            canDeleteState_ = canDeleteState;
            canDeleteAnimationSlot_ = canDeleteAnimationSlot;
            canDeleteHitbox_ = canDeleteHitbox;
        }

        #endregion

        #region Properties

        public EntityType EntityType
        {
            get { return entityType_; }
        }
        private EntityType entityType_;

        public bool CanAddStates
        {
            get { return canAddStates_; }
        }
        private bool canAddStates_;

        public bool CanAddProperties
        {
            get { return canAddProperties_; }
        }
        private bool canAddProperties_;

        public bool CanAddAnimationSlots
        {
            get { return canAddAnimationSlots_; }
        }
        private bool canAddAnimationSlots_;

        public bool CanAddHitboxes
        {
            get { return canAddHitboxes_; }
        }
        private bool canAddHitboxes_;

        public bool CanDeleteAnimationSlot
        {
            get { return canDeleteAnimationSlot_; }
        }
        private bool canDeleteAnimationSlot_;

        public bool CanDeleteHitbox
        {
            get { return canDeleteHitbox_; }
        }
        private bool canDeleteHitbox_;

        public bool CanDeleteEntity
        {
            get { return canDeleteEntity_; }
        }
        private bool canDeleteEntity_;

        public bool CanDeleteState
        {
            get { return canDeleteState_; }
        }
        private bool canDeleteState_;

        #endregion
    }

    #endregion
}
