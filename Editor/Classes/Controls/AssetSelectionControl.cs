using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class AssetSelectionControl : UserControl, IAssetSelectionControl 
    {
        #region Private Variables

        private Graphics g;

        private IFiremelonEditorFactory firemelonEditorFactory_;
        private IScriptGeneratorFactory scriptGeneratorFactory_;
        private IProjectController projectController_;
        private IMapWidgetPropertiesControl propertiesControl_;

        private ListChangedEventHandler listChangedEventHandler_;
        private BeforeListSortedHandler beforeListSortedEventHandler_;
        private PropertyValueChangedEventHandler propertyValueChangedEventHandler_;

        private Dictionary<string, MapWidgetMode> dictMapWidgetMode_;
        private Dictionary<string, MapWidgetType> dictMapWidgetType_;

        #endregion

        #region Events

        #endregion

        #region Constructors

        public AssetSelectionControl(IProjectController projectController)
        {
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();
            scriptGeneratorFactory_ = new ScriptGeneratorFactory();

            projectController_ = projectController;
            
            listChangedEventHandler_ = new ListChangedEventHandler(this.AssetsControl3_ListChanged);
            beforeListSortedEventHandler_ = new BeforeListSortedHandler(this.AssetsControl3_BeforeListSorted);

            dictMapWidgetMode_ = new Dictionary<string, MapWidgetMode>();

            dictMapWidgetMode_.Add("ACTOR", MapWidgetMode.Actor);
            dictMapWidgetMode_.Add("EVENT", MapWidgetMode.Event);
            dictMapWidgetMode_.Add("HUDELEMENT", MapWidgetMode.HudElement);
            dictMapWidgetMode_.Add("SPAWNPOINT", MapWidgetMode.SpawnPoint);
            dictMapWidgetMode_.Add("PARTICLEEMITTER", MapWidgetMode.ParticleEmitter);
            dictMapWidgetMode_.Add("AUDIOSOURCE", MapWidgetMode.AudioSource);
            dictMapWidgetMode_.Add("WORLDGEOMETRY", MapWidgetMode.WorldGeometry);
            dictMapWidgetMode_.Add("TILESHEETOBJECT", MapWidgetMode.TileObject);

            dictMapWidgetType_ = new Dictionary<string, MapWidgetType>();
            
            dictMapWidgetType_.Add("SPAWNPOINT", MapWidgetType.SpawnPoint);
            dictMapWidgetType_.Add("PARTICLEEMITTER", MapWidgetType.ParticleEmitter);
            dictMapWidgetType_.Add("AUDIOSOURCE", MapWidgetType.AudioSource);
            dictMapWidgetType_.Add("WORLDGEOMETRY", MapWidgetType.WorldGeometry);
            dictMapWidgetType_.Add("TILESHEETOBJECT", MapWidgetType.TileObject);

            //roomContainerClearedHandler_ = new RoomContainerClearHandler(this.AssetsControl3_RoomContainerCleared);
            //assetContainerClearHandler_ = new AssetContainerClearHandler(this.AssetsControl3_AssetsContainerCleared);

            projectController_.ProjectCreated += new ProjectCreateHandler(this.AssetSelectionControl_ProjectCreated);
            projectController_.ProjectStateChanged += new ProjectStateChangeHandler(this.AssetSelectionControl_ProjectStateChanged);

            projectController_.RoomSelected += new RoomSelectHandler(this.AssetSelectionControl_RoomSelected);
            projectController_.RefreshView += new RefreshViewHandler(this.AssetsControl3_RefreshView);
            projectController_.RefreshProperties += new RefreshPropertiesHandler(this.AssetsControl3_RefreshProperties);

            projectController_.ActorAdd += new ActorAddHandler(this.AssetSelectionControl_ActorAdded);
            projectController_.EventAdd += new EventAddHandler(this.AssetSelectionControl_EventAdded);
            projectController_.HudElementAdd += new HudElementAddHandler(this.AssetSelectionControl_HudElementAdded);
            projectController_.SpawnPointAdd += new SpawnPointAddHandler(this.AssetSelectionControl_SpawnPointAdded);
            projectController_.TileObjectAdd += new TileObjectAddHandler(this.AssetSelectionControl_TileObjectAdded);
            projectController_.TileObjectDelete += new TileObjectDeleteHandler(this.AssetSelectionControl_TileObjectDeleted);
            projectController_.TileObjectNameChange += new TileObjectNameChangeHandler(this.AssetSelectionControl_TileObjectNameChanged);

            projectController_.BeforeMapWidgetDelete += new BeforeMapWidgetDeleteHandler(this.AssetsControl3_BeforeMapWidgetDelete);
            projectController_.BeforeMapWidgetAdd += new BeforeMapWidgetAddHandler(this.AssetsControl3_BeforeMapWidgetAdded);
            projectController_.MapWidgetAdd += new MapWidgetAddHandler(this.AssetsControl3_MapWidgetAdded);
            
            // Fixes weird behavior where right clicking doesn't actually select the node.
            tvAssets.NodeMouseClick += (sender, args) => tvAssets.SelectedNode = args.Node;                    
        }

        #endregion

        #region Properties

        public bool IsTileObjectSelected
        {
            get
            {
                return tvAssets.SelectedNode.Name == "TILESHEETOBJECT";
            }
        }

        public IMapWidgetPropertiesControl PropertiesControl
        {
            set
            {
                propertiesControl_ = value;

                propertiesControl_.PropertyValueChanged -= propertyValueChangedEventHandler_;
                propertiesControl_.PropertyValueChanged += propertyValueChangedEventHandler_;
            }
        }

        #endregion

        #region Public Functions

        #endregion

        #region Protected Functions
        
        #endregion

        #region Private Functions

        private void buildMainTree()
        {
            ProjectDto project = projectController_.GetProjectDto();

            tvAssets.Nodes.Clear();

            // Root nodes.
            TreeNode tileSheetRootNode = tvAssets.Nodes.Add("TILESHEETROOT", "Tile Sheets");
            
            // Add the tile sheets to the tree.
            foreach (TileSheetDto tileSheet in project.TileSheets)
            {
                TreeNode tileSheetNode = addTileSheetToTree(tileSheetRootNode, tileSheet);

                tileSheetNode.Tag = tileSheet.Id;

                // Add the tile sheet objects to the tree.
                foreach (TileObjectDto tileObject in project.TileObjects[tileSheet.Id])
                {
                    TreeNode tileObjectNode = tileSheetNode.Nodes.Add("TILESHEETOBJECT", tileObject.Name);

                    tileObjectNode.Tag = tileObject.Id;
                }
            }
            
            TreeNode entityRootNode = tvAssets.Nodes.Add("ENTITYROOT", "Entities");

            TreeNode actorRootNode = entityRootNode.Nodes.Add("ACTORROOT", "Actors");
            
            // Add the actors to the tree.
            foreach (ActorDto actor in project.Actors)
            {
                TreeNode actorNode = actorRootNode.Nodes.Add("ACTOR", actor.Name);

                actorNode.Tag = actor.Id;
            }
            
            TreeNode eventRootNode = entityRootNode.Nodes.Add("EVENTROOT", "Events");

            // Add the events to the tree.
            foreach (EventDto currentEvent in project.Events)
            {
                TreeNode eventNode = eventRootNode.Nodes.Add("EVENT", currentEvent.Name);

                eventNode.Tag = currentEvent.Id;
            }

            TreeNode hudElementRootNode = entityRootNode.Nodes.Add("HUDELEMENTROOT", "HUD Elements");

            // Add the HUD Elements to the tree.
            foreach (HudElementDto hudElement in project.HudElements)
            {
                TreeNode hudElementNode = hudElementRootNode.Nodes.Add("HUDELEMENT", hudElement.Name);

                hudElementNode.Tag = hudElement.Id;
            }

            TreeNode audioSourceRootNode = tvAssets.Nodes.Add("AUDIOSOURCE", "Audio Sources");

            TreeNode spawnPointRootNode = tvAssets.Nodes.Add("SPAWNPOINT", "Spawn Point");

            TreeNode particleEmitterRoot = tvAssets.Nodes.Add("PARTICLEEMITTER", "Particle Emitter");

            TreeNode worldGeometryRoot = tvAssets.Nodes.Add("WORLDGEOMETRY", "World Geometry");            
        }

        private TreeNode getNode(TreeNode rootNode, Guid id)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                if ((Guid)node.Tag == id)
                {
                    return node;
                }
            }

            return null;
        }

        private void addActorNodes(ProjectDto project)
        {
            TreeNode actorRootNode = tvAssets.Nodes["ENTITYROOT"].Nodes["ACTORROOT"];

            foreach (ActorDto actor in project.Actors)
            {
                addActorToTree(actorRootNode, actor);
            }
        }

        private TreeNode addActorToTree(TreeNode rootNode, ActorDto actor)
        {
            // If this actor already has a node on the tree, use that one. Otherwise create a new one.
            TreeNode node = getNode(rootNode, actor.Id);

            if (node == null)
            {
                TreeNode actorNode = rootNode.Nodes.Add("ACTOR", actor.Name);

                actorNode.Tag = actor.Id;

                return actorNode;
            }
            else
            {
                node.Text = actor.Name;

                return node;
            }
        }
        
        private void addEventNodes(ProjectDto project)
        {
            TreeNode eventRootNode = tvAssets.Nodes["ENTITYROOT"].Nodes["EVENTROOT"];

            foreach (EventDto currentEvent in project.Events)
            {
                addEventToTree(eventRootNode, currentEvent);
            }
        }

        private TreeNode addEventToTree(TreeNode rootNode, EventDto eventToAdd)
        {
            // If this actor already has a node on the tree, use that one. Otherwise create a new one.
            TreeNode node = getNode(rootNode, eventToAdd.Id);

            if (node == null)
            {
                TreeNode eventNode = rootNode.Nodes.Add("EVENT", eventToAdd.Name);

                eventNode.Tag = eventToAdd.Id;

                return eventNode;
            }
            else
            {
                node.Text = eventToAdd.Name;

                return node;
            }
        }
        
        private void addHudElementNodes(ProjectDto project)
        {
            TreeNode hudElementRootNode = tvAssets.Nodes["ENTITYROOT"].Nodes["HUDELEMENTROOT"];

            foreach (HudElementDto hudElement in project.HudElements)
            {
                addHudElementToTree(hudElementRootNode, hudElement);
            }
        }

        private TreeNode addHudElementToTree(TreeNode rootNode, HudElementDto hudElement)
        {
            // If this actor already has a node on the tree, use that one. Otherwise create a new one.
            TreeNode node = getNode(rootNode, hudElement.Id);

            if (node == null)
            {
                TreeNode hudElementNode = rootNode.Nodes.Add("HUDELEMENT", hudElement.Name);

                hudElementNode.Tag = hudElement.Id;

                return hudElementNode;
            }
            else
            {
                node.Text = hudElement.Name;

                return node;
            }
        }

        private TreeNode addTileObjectToTree(TreeNode rootNode, TileObjectDto tileObject)
        {
            // If this tile object already has a node on the tree, ignore it.
            foreach (TreeNode node in rootNode.Nodes)
            {
                if ((Guid)node.Tag == tileObject.Id)
                {
                    node.Text = tileObject.Name;

                    return node;
                }
            }

            TreeNode tileObjectNode = rootNode.Nodes.Add("TILEOBJECT", tileObject.Name);

            tileObjectNode.Tag = tileObject.Id;

            return tileObjectNode;
        }

        private void addTileSheetNodes(ProjectDto project)
        {
            TreeNode tileSheetRootNode = tvAssets.Nodes["TILESHEETROOT"];
            
            foreach (TileSheetDto tileSheet in project.TileSheets)
            {
                TreeNode tileSheetNode = addTileSheetToTree(tileSheetRootNode, tileSheet);

                foreach (TileObjectDto tileObject in project.TileObjects[tileSheet.Id])
                {
                    addTileObjectToTree(tileSheetNode, tileObject);
                }
            }
        }

        private TreeNode addTileSheetToTree(TreeNode rootNode, TileSheetDto tileSheet)
        {
            // If this tile sheet already has a node on the tree, ignore it.
            foreach (TreeNode node in rootNode.Nodes)
            {
                if ((Guid)node.Tag == tileSheet.Id)
                {
                    node.Text = tileSheet.Name;

                    return node;
                }
            }

            TreeNode tileObjectNode = rootNode.Nodes.Add("TILESHEET", tileSheet.Name);

            tileObjectNode.Tag = tileSheet.Id;

            return tileObjectNode;
        }

        private TreeNode getTileSheetNode(Guid tileSheetId)
        {
            foreach (TreeNode node in tvAssets.Nodes["TILESHEETROOT"].Nodes)
            {
                if ((Guid)node.Tag == tileSheetId)
                {
                    return node;
                }
            }

            return null;
        }

        private void pruneActorNodes(ProjectDto project)
        {
            int nodeCount = tvAssets.Nodes["ENTITYROOT"].Nodes["ACTORROOT"].Nodes.Count;

            for (int i = nodeCount - 1; i >= 0; i--)
            {
                TreeNode node = tvAssets.Nodes["ENTITYROOT"].Nodes["ACTORROOT"].Nodes[i];

                if (projectController_.GetActor((Guid)node.Tag) == null)
                {
                    // Actor does not exist if a value of null is returned.
                    node.Remove();
                }
            }
        }

        private void pruneEventNodes(ProjectDto project)
        {
            int nodeCount = tvAssets.Nodes["ENTITYROOT"].Nodes["EVENTROOT"].Nodes.Count;

            for (int i = nodeCount - 1; i >= 0; i--)
            {
                TreeNode node = tvAssets.Nodes["ENTITYROOT"].Nodes["EVENTROOT"].Nodes[i];

                if (projectController_.GetEvent((Guid)node.Tag) == null)
                {
                    // Event does not exist if a value of null is returned.
                    node.Remove();
                }
            }
        }

        private void pruneHudElementNodes(ProjectDto project)
        {
            int nodeCount = tvAssets.Nodes["ENTITYROOT"].Nodes["HUDELEMENTROOT"].Nodes.Count;

            for (int i = nodeCount - 1; i >= 0; i--)
            {
                TreeNode node = tvAssets.Nodes["ENTITYROOT"].Nodes["HUDELEMENTROOT"].Nodes[i];

                if (projectController_.GetHudElement((Guid)node.Tag) == null)
                {
                    // HUD element does not exist if a value of null is returned.
                    node.Remove();
                }
            }
        }

        private void pruneTileSheetNodes(ProjectDto project)
        {
            TreeNode tileSheetRootNode = tvAssets.Nodes["TILESHEETROOT"];

            int tileSheetNodeCount = tileSheetRootNode.Nodes.Count;

            for (int i = tileSheetNodeCount - 1; i >= 0; i--)
            {
                TreeNode tileSheetNode = tileSheetRootNode.Nodes[i];

                TileSheetDto tileSheet = projectController_.GetTileSheet((Guid)tileSheetNode.Tag);

                if (tileSheet == null)
                {
                    // Tile sheet does not exist if a value of null is returned.
                    tileSheetNode.Remove();
                }
                else
                {
                    int tileObjectNodeCount = tileSheetNode.Nodes.Count;

                    // Check the tile objects for this sheet.
                    for (int j = tileObjectNodeCount - 1; j >= 0; j--)
                    {
                        TreeNode tileObjectNode = tileSheetNode.Nodes[j];

                        TileObjectDto tileObject = projectController_.GetTileObject((Guid)tileObjectNode.Tag);

                        if (tileObject == null)
                        {
                            // Tile sheet does not exist if a value of null is returned.
                            tileObjectNode.Remove();
                        }
                    }
                }
            }
        }

        private void updateTree(ProjectDto project)
        {
            // First remove any nodes whose corresponding object does not exist.
            pruneTileSheetNodes(project);

            pruneActorNodes(project);

            pruneEventNodes(project);

            pruneHudElementNodes(project);

            // Then add any nodes that are missing.
            addTileSheetNodes(project);

            addActorNodes(project);

            addEventNodes(project);

            addHudElementNodes(project);
        }
        
        #endregion

        #region Event Handlers

        private void AssetsSelectionControl_Load(object sender, EventArgs e)
        {
            Application.EnableVisualStyles();
        }

        public void AssetSelectionControl_TileObjectAdded(object sender, TileObjectAddedEventArgs e)
        {
            TreeNode tileSheetNode = getTileSheetNode(e.TileSheetId);

            if (tileSheetNode != null)
            {
                ProjectDto project = projectController_.GetProjectDto();

                foreach (TileObjectDto tileObject in project.TileObjects[e.TileSheetId])
                {
                    if (tileObject.Id == e.TileObjectId)
                    {
                        TreeNode tileObjectNode = tileSheetNode.Nodes.Add("TILESHEETOBJECT", tileObject.Name);

                        tileObjectNode.Tag = tileObject.Id;
                    }
                }
            }
        }

        public void AssetSelectionControl_TileObjectDeleted(object sender, TileObjectDeletedEventArgs e)
        {
            tvAssets.Nodes["TILESHEETROOT"].Nodes[e.TileSheetIndex].Nodes[e.TileObjectIndex].Remove();
        }

        public void AssetSelectionControl_TileObjectNameChanged(object sender, TileObjectNameChangedEventArgs e)
        {
            TileObjectDto tileObject = projectController_.GetTileObject(e.TileObjectId);

            TreeNode tileSheetNode = getTileSheetNode(tileObject.OwnerId);

            if (tileSheetNode != null)
            {
                foreach (TreeNode tileObjectNode in tileSheetNode.Nodes)
                {
                    if (tileObjectNode.Text == e.OldName)
                    {
                        tileObjectNode.Text = e.Name;

                        break;
                    }
                }
            }
        }

        private void tvAssets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }
            
            switch (tvAssets.SelectedNode.Name)
            {
                case "ACTOR":
                    
                    projectController_.SelectActor(tvAssets.SelectedNode.Index);

                    projectController_.SetMapWidgetMode(uiState.SelectedRoomIndex, MapWidgetMode.Actor);

                    projectController_.ShowMouse();

                    break;

                case "EVENT":

                    projectController_.SelectEvent(tvAssets.SelectedNode.Index);

                    projectController_.SetMapWidgetMode(uiState.SelectedRoomIndex, MapWidgetMode.Event);

                    projectController_.ShowMouse();

                    break;

                case "HUDELEMENT":

                    projectController_.SelectHudElement(tvAssets.SelectedNode.Index);

                    projectController_.SetMapWidgetMode(uiState.SelectedRoomIndex, MapWidgetMode.HudElement);

                    projectController_.ShowMouse();

                    break;

                case "TILESHEETOBJECT":
                    
                    projectController_.SetMapWidgetMode(uiState.SelectedRoomIndex, MapWidgetMode.TileObject);

                    projectController_.SetSelectedMapWidgetType(uiState.SelectedRoomIndex, MapWidgetType.TileObject);

                    projectController_.HideMouse();

                    // Get the tile sheet ID and object ID.
                    Guid tileObjectId = (Guid)tvAssets.SelectedNode.Tag;

                    projectController_.SelectTileObject(tileObjectId);

                    break;

                default:

                    if (dictMapWidgetMode_.ContainsKey(tvAssets.SelectedNode.Name))
                    {
                        MapWidgetMode mapWidgetMode = dictMapWidgetMode_[tvAssets.SelectedNode.Name];

                        MapWidgetType mapWidgetType = dictMapWidgetType_[tvAssets.SelectedNode.Name];

                        projectController_.SetMapWidgetMode(uiState.SelectedRoomIndex, mapWidgetMode);

                        projectController_.SetSelectedMapWidgetType(uiState.SelectedRoomIndex, mapWidgetType);

                        projectController_.HideMouse();
                    }
                    else
                    {
                        projectController_.SetMapWidgetMode(uiState.SelectedRoomIndex, MapWidgetMode.None);

                        projectController_.SetSelectedMapWidgetType(uiState.SelectedRoomIndex, MapWidgetType.None);
                    }

                    break;
            }            
        }
        
        #endregion

        #region Potentially no longer used functions. Need to verify each one.        
                
        private void addAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //addAnimation();
        }
        
        public void AssetSelectionControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            if (e.NewProject == true)
            {
                // Create the initial nodes for the tree.
                buildMainTree();

                propertiesControl_.SelectedObject = null;

                tvAssets.SelectedNode = tvAssets.Nodes["TILESHEETROOT"];
            
                //projectController_.ClearMapWidgetSelection(selectedRoomIndex);
            }          
        }

        public void AssetSelectionControl_ProjectStateChanged(object sender, ProjectStateChangedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            updateTree(project);
        }
                
        public void AssetSelectionControl_RoomSelected(object sender, RoomSelectedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid roomId = project.Rooms[e.Index].Id;            
        }

        public void AssetsControl3_RefreshView(object sender, RefreshViewEventArgs e)
        {
            refreshView();
        }

        private void refreshView()
        {
        }

        public void AssetsControl3_BeforeListSorted(object sender, BeforeListSortedEventArgs e)
        {
        }
        
        public void AssetsControl3_BeforeMapWidgetDelete(object sender, BeforeMapWidgetDeleteEventArgs e)
        {
        }

        public void AssetsControl3_ListChanged(object sender, ListChangedEventArgs e)
        {         
        }

        public void AssetsControl3_RefreshProperties(object sender, RefreshPropertiesEventArgs e)
        {
            propertiesControl_.Refresh();
        }

        public void AssetSelectionControl_ActorAdded(object sender, ActorAddedEventArgs e)
        {
        }

        public void AssetSelectionControl_EventAdded(object sender, EventAddedEventArgs e)
        {
        }

        public void AssetSelectionControl_HudElementAdded(object sender, HudElementAddedEventArgs e)
        {
        }

        public void AssetSelectionControl_SpawnPointAdded(object sender, SpawnPointAddedEventArgs e)
        {
        }
        
        public void AssetsControl3_BeforeMapWidgetAdded(object sender, BeforeMapWidgetAddedEventArgs e)
        {
        }

        public void AssetsControl3_MapWidgetAdded(object sender, MapWidgetAddedEventArgs e)
        {
        }
        
        private void generateScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //// Generate a skeleton script for the selected entity, query, or menu item.
            //IScriptDtoProxy scriptProxy = (IScriptDtoProxy)((AssetMenuDto)tvAssets.SelectedNode.Tag).Asset;

            //generateNewScript(scriptProxy);

            //propertyGridForm_.PropertyGrid.Refresh();
        }

        private void generateNewScript(IScriptDtoProxy scriptProxy)
        {
            //ScriptDto script = projectController_.GetScript(scriptProxy.Id);

            //SaveFileDialog saveScript = new SaveFileDialog();

            //saveScript.DefaultExt = "py";
            //saveScript.AddExtension = true;
            //saveScript.RestoreDirectory = true;
            //saveScript.Filter = "Python Files|*.py";
            //saveScript.FileName = script.Name.ToLower() + ".py";
            //saveScript.RestoreDirectory = true;

            //string fileName = string.Empty;

            //if (saveScript.ShowDialog() == DialogResult.OK)
            //{
            //    fileName = saveScript.FileName;

            //    IScriptGenerator scriptGenerator = scriptGeneratorFactory_.NewScriptGenerator(script);

            //    string scriptCode = scriptGenerator.Generate(script);

            //    File.WriteAllText(fileName, scriptCode);

            //    scriptProxy.ScriptPath = fileName;
            //}
        }
        
        private void viewEditScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //IScriptDtoProxy scriptProxy = (IScriptDtoProxy)((AssetMenuDto)tvAssets.SelectedNode.Tag).Asset;

            //if (String.IsNullOrEmpty(scriptProxy.ScriptPath) == true)
            //{
            //    string msg = "Script file not set. Generate a new script?";
            //    string caption = "Generate Script?";

            //    if (MessageBox.Show(msg, caption, MessageBoxButtons.OKCancel) == DialogResult.OK)
            //    {
            //        generateNewScript(scriptProxy);

            //        propertyGridForm_.PropertyGrid.Refresh();
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            //if (scriptProxy.ScriptPath != string.Empty)
            //{
            //    System.Diagnostics.Process.Start(scriptProxy.ScriptPath);
            //}
        }

        private void addTriggerSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addTriggerSignal(tvAssets.SelectedNode.Parent);
        }

        private void addTriggerSignal(TreeNode entityNode)
        {
            //ProjectDto project = projectController_.GetProjectDto();

            //int triggerSignalCount = project.TriggerSignals.Count;

            //bool isNameValid = false;
            //int counter = 0;
            //string currentName = "New Trigger Signal";

            //// Find the first sequentially available name.
            //while (isNameValid == false)
            //{
            //    isNameValid = true;

            //    // The current name that is being checked for collision.
            //    if (counter > 0)
            //    {
            //        currentName = "New Trigger Signal " + counter.ToString();
            //    }

            //    for (int i = 0; i < triggerSignalCount; i++)
            //    {
            //        if (currentName.ToUpper() == project.TriggerSignals[i].Name.ToUpper())
            //        {
            //            isNameValid = false;
            //            break;
            //        }
            //    }

            //    counter++;
            //}

            //TriggerSignalDto newTriggerSignal = projectController_.AddTriggerSignal(currentName);

            //addTriggerSignalToTree(newTriggerSignal);
        }
        
        private void deleteTriggerSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Delete Trigger Signal?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    IBaseDtoProxy entity = (IBaseDtoProxy)((AssetMenuDto)tvAssets.SelectedNode.Parent.Parent.Tag).Asset;

            //    projectController_.DeleteTriggerSignal(tvAssets.SelectedNode.Index);

            //    tvAssets.SelectedNode.Remove();
            //}
        }

        private void addMenuBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Removed in 2.1
            //addMenuBook();
        }
        
        private void deleteMenuBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Removed in 2.1
            //if (MessageBox.Show("Delete Menu Book?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    projectController_.DeleteMenuBook(tvAssets.SelectedNode.Index);

            //    tvAssets.SelectedNode.Remove();
            //}
        }

        private void addMenuPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Removed in 2.1
            //addMenuPage();
        }
        
        private void deleteMenuPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Removed in 2.1
            //if (MessageBox.Show("Delete Menu Page?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    projectController_.DeleteMenuPage(tvAssets.SelectedNode.Index);

            //    tvAssets.SelectedNode.Remove();
            //}
        }

        private void addWidgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addWidget();
        }

        private void addWidget()
        {
            //ProjectDto project = projectController_.GetProjectDto();

            //int menuItemCount = tvAssets.Nodes["UIROOT"].Nodes["WIDGETROOT"].Nodes.Count + 1;

            //string name = getNextAvailableName("NewWidget");

            //UiWidgetDto newWidget = projectController_.AddUiWidget(name);

            //addWidgetToTree(newWidget);
        }

        private void deleteWidgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete UI Widget?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                projectController_.DeleteUiWidget(tvAssets.SelectedNode.Index);
                
                tvAssets.SelectedNode.Remove();
            }
        }

        private void addScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addAdditionalScript();
        }

        private void addAdditionalScript()
        {
            ProjectDto project = projectController_.GetProjectDto();

            string name = getNextAvailableName("NewScript");

            ScriptDto newScript = projectController_.AddScript(name, ScriptType.Script);

            addScriptToTree(newScript);
        }

        private void addScriptToTree(ScriptDto script)
        {
            //IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewNamedScriptProxy(projectController_, script.Id);

            //// If this script already has a node on the tree, ignore it.
            //foreach (TreeNode node in tvAssets.Nodes["OTHERROOT"].Nodes["ADDITIONALSCRIPTROOT"].Nodes)
            //{
            //    IScriptDtoProxy nodeProxy = (IScriptDtoProxy)((AssetMenuDto)node.Tag).Asset;

            //    if (nodeProxy.Id == scriptProxy.Id)
            //    {
            //        node.Text = nodeProxy.Name;

            //        return;
            //    }
            //}

            //TreeNode scriptNode = tvAssets.Nodes["OTHERROOT"].Nodes["ADDITIONALSCRIPTROOT"].Nodes.Add("SCRIPT", script.Name);

            //scriptNode.Tag = new AssetMenuDto(cmnuScript, scriptProxy);

            //ProjectDto project = projectController_.GetProjectDto();
        }

        private void cmnuScript_Opening(object sender, CancelEventArgs e)
        {
            IScriptDtoProxy scriptProxy = (IScriptDtoProxy)((AssetMenuDto)tvAssets.SelectedNode.Tag).Asset;

            // If this is an additional script, not associated with any other asset type, allow for deletion.
            // Also disable generation, as it would just be blank.
            if (scriptProxy.Id == scriptProxy.OwnerId)
            {
                generateScriptToolStripMenuItem.Visible = false;
                deleteScriptToolStripMenuItem.Visible = true;
            }
            else
            {
                generateScriptToolStripMenuItem.Visible = true;
                deleteScriptToolStripMenuItem.Visible = false;
            }
        }

        private void deleteScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Script?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                IScriptDtoProxy scriptProxy = (IScriptDtoProxy)((AssetMenuDto)tvAssets.SelectedNode.Tag).Asset;

                projectController_.DeleteScript(scriptProxy.Id);

                tvAssets.SelectedNode.Remove();
            }
        }

        private string getNextAvailableName(string baseName)
        {
            bool isNameValid = false;
            int counter = 0;
            string currentName = baseName;

            //// Find the first sequentially available name.
            //while (isNameValid == false)
            //{
            //    // The current name that is being checked for collision.
            //    if (counter > 0)
            //    {
            //        currentName = baseName + counter.ToString();
            //    }

            //    isNameValid = !projectController_.IsNameInUse(Guid.Empty, currentName);
               
            //    counter++;
            //}

            return currentName.Trim();
        }

        private void addEntityClassificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Removed in 2.1
            //addEntityClassification();
        }
        
        private void deleteEntityClassificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Removed in 2.1
            //if (MessageBox.Show("Delete Entity Classification?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    projectController_.DeleteEntityClassification(tvAssets.SelectedNode.Index);

            //    tvAssets.SelectedNode.Remove();
            //}
        }

        private void addLoadingScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addLoadingScreen();
        }

        private void addLoadingScreen()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int loadingScreenCount = tvAssets.Nodes["LOADINGSCREENSROOT"].Nodes.Count + 1;

            string name = getNextAvailableName("NewLoadingScreen");

            LoadingScreenDto newLoadingScreen = projectController_.AddLoadingScreen(name);            
        }
        
        private void deleteLoadingScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Loading Screen?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                projectController_.DeleteLoadingScreen(tvAssets.SelectedNode.Index);

                tvAssets.SelectedNode.Remove();
            }
        }

        private void addTransitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addTransition();
        }

        private void addTransition()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int transitionCount = tvAssets.Nodes["TRANSITIONROOT"].Nodes.Count + 1;

            string name = getNextAvailableName("NewTransition");

            TransitionDto newTransition = projectController_.AddTransition(name);

            addTransitionToTree(newTransition);
        }
        
        private TreeNode addTransitionToTree(TransitionDto transition)
        {
            //ITransitionDtoProxy transitionProxy = firemelonEditorFactory_.NewTransitionProxy(projectController_, transition.Id);

            //// If this transition already has a node on the tree, ignore it.
            //foreach (TreeNode node in tvAssets.Nodes["TRANSITIONROOT"].Nodes)
            //{
            //    ITransitionDtoProxy nodeProxy = (ITransitionDtoProxy)((AssetMenuDto)node.Tag).Asset;

            //    if (nodeProxy.Id == transitionProxy.Id)
            //    {
            //        node.Text = nodeProxy.Name;

            //        return node;
            //    }
            //}

            //TreeNode transitionNode = tvAssets.Nodes["TRANSITIONROOT"].Nodes.Add("TRANSITION", transition.Name);

            //transitionNode.Tag = new AssetMenuDto(cmnuTransition, transitionProxy);

            //ProjectDto project = projectController_.GetProjectDto();

            //ScriptDto script = project.Scripts[transition.Id];

            //IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

            //transitionNode.Nodes.Add("SCRIPTROOT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);

            //return transitionNode;
            return null;
        }

        private void deleteTransitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Transition?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                projectController_.DeleteTransition(tvAssets.SelectedNode.Index);

                tvAssets.SelectedNode.Remove();
            }
        }

        private void addSpawnPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addSpawnPoint();
        }

        private void addSpawnPoint()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int spawnPointCount = tvAssets.Nodes["SPAWNPOINTROOT"].Nodes.Count + 1;

            string name = getNextAvailableName("NewSpawnPoint");

            SpawnPointDto newSpawnPoint = projectController_.AddSpawnPoint(name);

            addSpawnPointToTree(newSpawnPoint);
        }

        private TreeNode addSpawnPointToTree(SpawnPointDto newSpawnPoint)
        {
            //ISpawnPointDtoProxy spawnPointProxy = firemelonEditorFactory_.NewSpawnPointProxy(projectController_, newSpawnPoint.Id);

            //// If this spawn point already has a node on the tree, ignore it.
            //foreach (TreeNode node in tvAssets.Nodes["SPAWNPOINTROOT"].Nodes)
            //{
            //    ISpawnPointDtoProxy nodeProxy = (ISpawnPointDtoProxy)((AssetMenuDto)node.Tag).Asset;

            //    if (nodeProxy.Id == spawnPointProxy.Id)
            //    {
            //        node.Text = nodeProxy.Name;

            //        return node;
            //    }
            //}

            //TreeNode spawnPointNode = tvAssets.Nodes["SPAWNPOINTROOT"].Nodes.Add("SPAWNPOINT", newSpawnPoint.Name);
            //spawnPointNode.Tag = new AssetMenuDto(cmnuSpawnPoint, spawnPointProxy);

            //return spawnPointNode;
            return null;
        }

        private void deleteSpawnPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Spawn Point?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                projectController_.DeleteSpawnPoint(tvAssets.SelectedNode.Index);

                tvAssets.SelectedNode.Remove();
            }
        }

        private void actorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //tvAssets.Visible = false;

            //defineAssetsToolStripMenuItem.Checked = false;
            //actorsToolStripMenuItem.Checked = true;
            //eventsToolStripMenuItem.Checked = false;
            //hudElementsToolStripMenuItem.Checked = false;
            //spawnPointsToolStripMenuItem.Checked = false;
            //particleEmitterToolStripMenuItem.Checked = false;
            //audioSourceToolStripMenuItem.Checked = false;

            //scActors.Visible = actorsToolStripMenuItem.Checked;
            //scEvents.Visible = eventsToolStripMenuItem.Checked;
            //scHudElements.Visible = hudElementsToolStripMenuItem.Checked;
            //lbMapWidgets.Visible = spawnPointsToolStripMenuItem.Checked || particleEmitterToolStripMenuItem.Checked || audioSourceToolStripMenuItem.Checked;

            //ProjectUiStateDto uiState = projectController_.GetUiState();
            //int selectedRoomIndex = uiState.SelectedRoomIndex;

            //projectController_.SetDrawMode(selectedRoomIndex, DrawMode.DrawMapWidgets);

            //projectController_.SetMapWidgetMode(selectedRoomIndex, MapWidgetMode.Actor);
            //resizeActorList();             
        }

        private void eventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //tvAssets.Visible = false;

            //defineAssetsToolStripMenuItem.Checked = false;
            //actorsToolStripMenuItem.Checked = false;
            //eventsToolStripMenuItem.Checked = true;
            //hudElementsToolStripMenuItem.Checked = false;
            //spawnPointsToolStripMenuItem.Checked = false;
            //particleEmitterToolStripMenuItem.Checked = false;
            //audioSourceToolStripMenuItem.Checked = false;

            //scActors.Visible = actorsToolStripMenuItem.Checked;
            //scEvents.Visible = eventsToolStripMenuItem.Checked;
            //scHudElements.Visible = hudElementsToolStripMenuItem.Checked;
            //lbMapWidgets.Visible = spawnPointsToolStripMenuItem.Checked || particleEmitterToolStripMenuItem.Checked || audioSourceToolStripMenuItem.Checked;

            //ProjectUiStateDto uiState = projectController_.GetUiState();
            //int selectedRoomIndex = uiState.SelectedRoomIndex;

            //projectController_.SetDrawMode(selectedRoomIndex, DrawMode.DrawMapWidgets);

            //projectController_.SetMapWidgetMode(selectedRoomIndex, MapWidgetMode.Event);
            //resizeEventList();     
        }

        private void hudElementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //tvAssets.Visible = false;

            //defineAssetsToolStripMenuItem.Checked = false;
            //actorsToolStripMenuItem.Checked = false;
            //eventsToolStripMenuItem.Checked = false;
            //hudElementsToolStripMenuItem.Checked = true;
            //spawnPointsToolStripMenuItem.Checked = false;
            //particleEmitterToolStripMenuItem.Checked = false;
            //audioSourceToolStripMenuItem.Checked = false;
            
            //scActors.Visible = actorsToolStripMenuItem.Checked;
            //scEvents.Visible = eventsToolStripMenuItem.Checked;
            //scHudElements.Visible = hudElementsToolStripMenuItem.Checked;
            //lbMapWidgets.Visible = spawnPointsToolStripMenuItem.Checked || particleEmitterToolStripMenuItem.Checked || audioSourceToolStripMenuItem.Checked;

            //ProjectUiStateDto uiState = projectController_.GetUiState();
            //int selectedRoomIndex = uiState.SelectedRoomIndex;

            //projectController_.SetDrawMode(selectedRoomIndex, DrawMode.DrawMapWidgets);

            //projectController_.SetMapWidgetMode(selectedRoomIndex, MapWidgetMode.HudElement);
            //resizeHudElementList();     
        }

        private void defineAssetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //defineAssetsToolStripMenuItem.Checked = true;
            //actorsToolStripMenuItem.Checked = false;
            //eventsToolStripMenuItem.Checked = false;
            //hudElementsToolStripMenuItem.Checked = false;
            //spawnPointsToolStripMenuItem.Checked = false;
            //particleEmitterToolStripMenuItem.Checked = false;
            //audioSourceToolStripMenuItem.Checked = false;

            //scActors.Visible = false;
            //scEvents.Visible = false;
            //scHudElements.Visible = false;
            //lbMapWidgets.Visible = false;

            //tvAssets.Visible = true;
        }
        
        private void spawnPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //tvAssets.Visible = false;

            //defineAssetsToolStripMenuItem.Checked = false;
            //actorsToolStripMenuItem.Checked = false;
            //eventsToolStripMenuItem.Checked = false;
            //hudElementsToolStripMenuItem.Checked = false;
            //particleEmitterToolStripMenuItem.Checked = false;
            //spawnPointsToolStripMenuItem.Checked = true;
            //audioSourceToolStripMenuItem.Checked = false;

            //scActors.Visible = actorsToolStripMenuItem.Checked;
            //scEvents.Visible = eventsToolStripMenuItem.Checked;
            //scHudElements.Visible = hudElementsToolStripMenuItem.Checked;
            //lbMapWidgets.Visible = spawnPointsToolStripMenuItem.Checked || particleEmitterToolStripMenuItem.Checked || audioSourceToolStripMenuItem.Checked;

            //ProjectUiStateDto uiState = projectController_.GetUiState();
            //int selectedRoomIndex = uiState.SelectedRoomIndex;

            //projectController_.SetDrawMode(selectedRoomIndex, DrawMode.DrawMapWidgets);
            
            //projectController_.SetMapWidgetMode(selectedRoomIndex, MapWidgetMode.SpawnPoint);
            //projectController_.SetSelectedMapWidgetType(selectedRoomIndex, MapWidgetType.SpawnPoint);   
        }

        private void lbMapWidgets_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (suppressMapWidgetSelection_ == false)
            //{
            //    ProjectUiStateDto uiState = projectController_.GetUiState();

            //    int selectedRoomIndex = uiState.SelectedRoomIndex;

            //    // Don't run the node selection code after clearing. This causes the selected
            //    // node to be set to null.
            //    suppressNodeSelection_ = true;

            //    projectController_.ClearMapWidgetSelection(selectedRoomIndex);

            //    List<Guid> lstSelectedMapWidgetIds = new List<Guid>();

            //    foreach (MapWidgetDto mapWidget in lbMapWidgets.SelectedItems)
            //    {
            //        lstSelectedMapWidgetIds.Add(mapWidget.Id);
            //    }

            //    projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstSelectedMapWidgetIds);

            //    suppressNodeSelection_ = false;
            //}
        }

        private void cmnuAddParticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addParticle();
        }

        private void addParticle()
        {
            //ProjectDto project = projectController_.GetProjectDto();

            //int particleCount = tvAssets.Nodes["PARTICLESYSTEMROOT"].Nodes["PARTICLEROOT"].Nodes.Count + 1;

            //string name = getNextAvailableName("NewParticle");

            //ParticleDto newParticle = projectController_.AddParticle(name);

            //addParticleToTree(newParticle);
        }

        //private TreeNode addParticleToTree(ParticleDto particle)
        //{
        //    IParticleDtoProxy particleProxy = firemelonEditorFactory_.NewParticleProxy(projectController_, particle.Id);

        //    // If this particle already has a node on the tree, ignore it.
        //    foreach (TreeNode node in tvAssets.Nodes["PARTICLESYSTEMROOT"].Nodes["PARTICLEROOT"].Nodes)
        //    {
        //        IParticleDtoProxy nodeProxy = (IParticleDtoProxy)((AssetMenuDto)node.Tag).Asset;

        //        if (nodeProxy.Id == particleProxy.Id)
        //        {
        //            node.Text = nodeProxy.Name;

        //            return node;
        //        }
        //    }

        //    TreeNode particleNode = tvAssets.Nodes["PARTICLESYSTEMROOT"].Nodes["PARTICLEROOT"].Nodes.Add("PARTICLE", particle.Name);
        //    particleNode.Tag = new AssetMenuDto(cmnuParticle, particleProxy);

        //    ProjectDto project = projectController_.GetProjectDto();

        //    ScriptDto script = project.Scripts[particle.Id];

        //    IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

        //    particleNode.Nodes.Add("SCRIPTROOT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);

        //    return particleNode;
        //}

        private void deleteParticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Delete Particle?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    projectController_.DeleteParticle(tvAssets.SelectedNode.Index);

            //    tvAssets.SelectedNode.Remove();
            //}
        }

        private void particleEmitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //tvAssets.Visible = false;

            //defineAssetsToolStripMenuItem.Checked = false;
            //actorsToolStripMenuItem.Checked = false;
            //eventsToolStripMenuItem.Checked = false;
            //hudElementsToolStripMenuItem.Checked = false;
            //spawnPointsToolStripMenuItem.Checked = false;
            //particleEmitterToolStripMenuItem.Checked = true;
            //audioSourceToolStripMenuItem.Checked = false;

            //scActors.Visible = actorsToolStripMenuItem.Checked;
            //scEvents.Visible = eventsToolStripMenuItem.Checked;
            //scHudElements.Visible = hudElementsToolStripMenuItem.Checked;
            //lbMapWidgets.Visible = spawnPointsToolStripMenuItem.Checked || particleEmitterToolStripMenuItem.Checked || audioSourceToolStripMenuItem.Checked;

            //ProjectUiStateDto uiState = projectController_.GetUiState();
            //int selectedRoomIndex = uiState.SelectedRoomIndex;

            //projectController_.SetDrawMode(selectedRoomIndex, DrawMode.DrawMapWidgets);

            //projectController_.SetMapWidgetMode(selectedRoomIndex, MapWidgetMode.ParticleEmitter);
            //projectController_.SetSelectedMapWidgetType(selectedRoomIndex, MapWidgetType.ParticleEmitter);   
        }

        private void addParticleEmitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addParticleEmitter();
        }

        private void addParticleEmitter()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int particleEmitterCount = tvAssets.Nodes["PARTICLESYSTEMROOT"].Nodes["PARTICLEEMITTERROOT"].Nodes.Count + 1;

            string name = getNextAvailableName("NewParticleEmitter");

            ParticleEmitterDto newParticleEmitter = projectController_.AddParticleEmitter(name);

            addParticleEmitterToTree(newParticleEmitter);
        }

        private TreeNode addParticleEmitterToTree(ParticleEmitterDto particleEmitter)
        {
            //IParticleEmitterDtoProxy particleEmitterProxy = firemelonEditorFactory_.NewParticleEmitterProxy(projectController_, particleEmitter.Id);

            //// If this particle emitter already has a node on the tree, ignore it.
            //foreach (TreeNode node in tvAssets.Nodes["PARTICLESYSTEMROOT"].Nodes["PARTICLEEMITTERROOT"].Nodes)
            //{
            //    IParticleEmitterDtoProxy nodeProxy = (IParticleEmitterDtoProxy)((AssetMenuDto)node.Tag).Asset;

            //    if (nodeProxy.Id == particleEmitterProxy.Id)
            //    {
            //        node.Text = nodeProxy.Name;

            //        return node;
            //    }
            //}

            //TreeNode particleEmitterNode = tvAssets.Nodes["PARTICLESYSTEMROOT"].Nodes["PARTICLEEMITTERROOT"].Nodes.Add("PARTICLEEMITTER", particleEmitter.Name);
            //particleEmitterNode.Tag = new AssetMenuDto(cmnuParticleEmitter, particleEmitterProxy);

            //ProjectDto project = projectController_.GetProjectDto();

            //ScriptDto script = project.Scripts[particleEmitter.Id];

            //IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

            //particleEmitterNode.Nodes.Add("SCRIPTROOT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);

            //return particleEmitterNode;
            return null;
        }

        private void deleteParticleEmitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Particle Emitter?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                projectController_.DeleteParticleEmitter(tvAssets.SelectedNode.Index);

                tvAssets.SelectedNode.Remove();
            }
        }

        private void audioSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //tvAssets.Visible = false;

            //defineAssetsToolStripMenuItem.Checked = false;
            //actorsToolStripMenuItem.Checked = false;
            //eventsToolStripMenuItem.Checked = false;
            //hudElementsToolStripMenuItem.Checked = false;
            //spawnPointsToolStripMenuItem.Checked = false;
            //particleEmitterToolStripMenuItem.Checked = false;
            //audioSourceToolStripMenuItem.Checked = true;

            //scActors.Visible = actorsToolStripMenuItem.Checked;
            //scEvents.Visible = eventsToolStripMenuItem.Checked;
            //scHudElements.Visible = hudElementsToolStripMenuItem.Checked;
            //lbMapWidgets.Visible = spawnPointsToolStripMenuItem.Checked || particleEmitterToolStripMenuItem.Checked || audioSourceToolStripMenuItem.Checked;

            //ProjectUiStateDto uiState = projectController_.GetUiState();
            //int selectedRoomIndex = uiState.SelectedRoomIndex;

            //projectController_.SetDrawMode(selectedRoomIndex, DrawMode.DrawMapWidgets);

            //projectController_.SetMapWidgetMode(selectedRoomIndex, MapWidgetMode.AudioSource);
            //projectController_.SetSelectedMapWidgetType(selectedRoomIndex, MapWidgetType.AudioSource);   
        }

        private void addGameButtonGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addGameButtonGroup();
        }

        private void addGameButtonGroup()
        {
            //ProjectDto project = projectController_.GetProjectDto();

            //int gameButtonGroupCount = project.GameButtonGroups.Count;

            //bool isNameValid = false;
            //int counter = 0;
            //string currentName = "New Button Group";

            //// Find the first sequentially available name.
            //while (isNameValid == false)
            //{
            //    isNameValid = true;

            //    // The current name that is being checked for collision.
            //    if (counter > 0)
            //    {
            //        currentName = "New Button Group " + counter.ToString();
            //    }

            //    for (int i = 0; i < gameButtonGroupCount; i++)
            //    {
            //        if (currentName.ToUpper() == project.GameButtonGroups[i].Name.ToUpper())
            //        {
            //            isNameValid = false;
            //            break;
            //        }
            //    }

            //    counter++;
            //}

            //GameButtonGroupDto newGameButtonGroup = projectController_.AddGameButtonGroup(currentName);

            //addGameButtonGroupToTree(newGameButtonGroup);
        }

        private TreeNode addGameButtonGroupToTree(GameButtonGroupDto gameButtonGroup)
        {
            //IGameButtonGroupDtoProxy gameButtonGroupProxy = firemelonEditorFactory_.NewGameButtonGroupProxy(projectController_, gameButtonGroup.Id);

            //// If this button group already has a node on the tree, ignore it.
            //foreach (TreeNode node in tvAssets.Nodes["GAMEBUTTONROOT"].Nodes["BUTTONGROUPROOT"].Nodes)
            //{
            //    IGameButtonGroupDtoProxy nodeProxy = (IGameButtonGroupDtoProxy)((AssetMenuDto)node.Tag).Asset;

            //    if (nodeProxy.Id == gameButtonGroupProxy.Id)
            //    {
            //        node.Text = nodeProxy.Name;

            //        return node;
            //    }
            //}

            //TreeNode gameButtonGroupNode = tvAssets.Nodes["GAMEBUTTONROOT"]
            //                             .Nodes["BUTTONGROUPROOT"]
            //                             .Nodes.Add("BUTTONGROUP", gameButtonGroup.Name);

            //gameButtonGroupNode.Tag = new AssetMenuDto(cmnuGameButtonGroup, gameButtonGroupProxy);

            //return gameButtonGroupNode;
            return null;
        }

        private void deleteGameButtonGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Delete Button Group?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    projectController_.DeleteGameButtonGroup(tvAssets.SelectedNode.Index);

            //    tvAssets.SelectedNode.Remove();
            //}
        }

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("Duplicate Actor?", "Confirm Duplicate", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    Guid actorId = projectController_.GetActorIdFromIndex(tvAssets.SelectedNode.Index);

            //    int actorCount = tvAssets.Nodes["ENTITYROOT"].Nodes["ACTORROOT"].Nodes.Count + 1;

            //    string name = getNextAvailableName("NewActor");

            //    ActorDto duplicatedActor = projectController_.DuplicateActor(actorId, name);
                
            //    TreeNode actorNode = addActorToTree(duplicatedActor);

            //    ProjectDto project = projectController_.GetProjectDto();

            //    if (project.States.ContainsKey(duplicatedActor.Id) == true)
            //    {
            //        foreach (StateDto state in project.States[duplicatedActor.Id])
            //        {
            //            addStateToTree(actorNode, state);
            //        }
            //    }

            //    if (project.Properties.ContainsKey(duplicatedActor.Id) == true)
            //    {
            //        foreach (PropertyDto property in project.Properties[duplicatedActor.Id])
            //        {
            //            addPropertyToTree(actorNode, property);
            //        }
            //    }
            //}
        }
                
        private void viewEditDataFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDataFileDtoProxy dataFileProxy = (IDataFileDtoProxy)((AssetMenuDto)tvAssets.SelectedNode.Tag).Asset;

            if (String.IsNullOrEmpty(dataFileProxy.FileFullPath) == true)
            {
                // No file is being pointed to.
            }
            else
            {
                if (File.Exists(dataFileProxy.FileFullPath) == false)
                {
                    using (File.Create(dataFileProxy.FileFullPath));
                }

                if (dataFileProxy.FilePath != string.Empty)
                {
                    System.Diagnostics.Process.Start(dataFileProxy.FileFullPath);
                }
            }
        }

        #endregion
    }
}