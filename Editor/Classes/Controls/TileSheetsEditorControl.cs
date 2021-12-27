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

    public partial class TileSheetsEditorControl : UserControl, ITileSheetsEditorControl
    {
        #region Events

        public event TileSheetSelectionChangedHandler TileSheetSelectionChanged;

        #endregion

        #region Constructors

        public TileSheetsEditorControl(IProjectController projectController)
        {
            System.Diagnostics.Debug.Print("Initalizing TileSheetEditorControl with handle " + this.Handle.ToString());

            InitializeComponent();

            projectController_ = projectController;
            
            projectController_.ProjectStateChanged += new ProjectStateChangeHandler(this.TileSheetsEditorControl_ProjectStateChanged);

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            ProjectDto project = projectController.GetProjectDto();
            
            // Initialize tile sheets.
            foreach (TileSheetDto tileSheet in project.TileSheets)
            {
                TreeNode tileSheetNode = addTileSheetToTree(tileSheet);

                foreach (TileObjectDto tileObject in project.TileObjects[tileSheet.Id])
                {
                    addTileSheetObjectToTree(tileSheetNode, tileObject);
                }

                foreach (SceneryAnimationDto sceneryAnimation in project.SceneryAnimations[tileSheet.Id])
                {
                    addSceneryAnimationToTree(tileSheetNode, sceneryAnimation);
                }
            }

            tileSheetViewerControl_ = firemelonEditorFactory_.NewTileSheetViewerControl(projectController);

            tileSheetViewerControl_.TileSheetSelectionChanged += new TileSheetSelectionChangedHandler(this.TileSheetsEditorControl_TileSheetEditorSelectionChanged);
            
            Control tileSheetViewerControl = (Control)tileSheetViewerControl_;

            tileSheetViewerControl.Dock = DockStyle.Fill;

            scTileSheets.Panel2.Controls.Add(tileSheetViewerControl);

            tileObjectViewerControl_ = firemelonEditorFactory_.NewTileObjectViewerControl(projectController);

            Control tileObjectViewerControl = (Control)tileObjectViewerControl_;

            tileObjectViewerControl.Dock = DockStyle.Fill;

            scTileSheets.Panel2.Controls.Add(tileObjectViewerControl);
        }

        #endregion

        #region Private Variables

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private IProjectController projectController_;

        private ITileSheetViewerControl tileSheetViewerControl_;

        private ITileObjectViewerControl tileObjectViewerControl_;

        #endregion

        #region Public Functions

        public void AddNew()
        {
            addNewTileSheet();
        }

        public void AddAnimation()
        {
            addNewAnimation();
        }

        public void AddTileObject()
        {
            addNewTileObject();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void DeleteAnimation()
        {
            deleteAnimation();
        }

        public void DeleteTileObject()
        {
            deleteTileObject();
        }

        #endregion

        #region Private Functions

        private void addNewAnimation()
        {
            if (tvTileSheets.SelectedNode == null)
            {
                MessageBox.Show("Selected Tree Node is null. This is an unresolved bug.");

                System.Diagnostics.Debug.Print("TileSheetEditorControl handle = " + this.Handle.ToString());

                return;
            }

            ProjectDto project = projectController_.GetProjectDto();
            
            TreeNode tileSheetNode = getSelectedTileSheetTreeNode();

            ITileSheetDtoProxy selectedTileSheetProxy = (ITileSheetDtoProxy)((AssetMenuDto)tileSheetNode.Tag).Asset;

            int tileSheetAnimationCount = project.SceneryAnimations[selectedTileSheetProxy.Id].Count + 1;

            bool isNameValid = false;

            int counter = 0;

            string currentName = "New Scenery Animation";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Scenery Animation " + counter.ToString();
                }

                foreach (TileSheetDto tileSheet in project.TileSheets)
                {
                    for (int j = 0; j < project.TileObjects[tileSheet.Id].Count; j++)
                    {
                        if (currentName.ToUpper() == project.TileObjects[tileSheet.Id][j].Name.ToUpper())
                        {
                            isNameValid = false;

                            break;
                        }
                    }

                    if (isNameValid == false)
                    {
                        break;
                    }
                }

                counter++;
            }

            SceneryAnimationDto sceneryAnimation = new SceneryAnimationDto();

            sceneryAnimation.OwnerId = selectedTileSheetProxy.Id;

            sceneryAnimation.Name = currentName;

            projectController_.AddSceneryAnimation(selectedTileSheetProxy.Id, sceneryAnimation);
            
            addSceneryAnimationToTree(tileSheetNode, sceneryAnimation);
        }

        private void addNewTileObject()
        {
            if (tvTileSheets.SelectedNode == null)
            {
                MessageBox.Show("Selected Tree Node is null. This is an unresolved bug.");

                System.Diagnostics.Debug.Print("TileSheetEditorControl handle = " + this.Handle.ToString());

                return;
            }
            
            ProjectDto project = projectController_.GetProjectDto();

            ITileSheetDtoProxy selectedTileSheetProxy = (ITileSheetDtoProxy)((AssetMenuDto)tvTileSheets.SelectedNode.Tag).Asset;

            int tileSheetObjectCount = project.TileObjects[selectedTileSheetProxy.Id].Count + 1;

            bool isNameValid = false;

            int counter = 0;

            string currentName = "New Tile Sheet Object";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Tile Sheet Object " + counter.ToString();
                }

                foreach (TileSheetDto tileSheet in project.TileSheets)
                {
                    for (int j = 0; j < project.TileObjects[tileSheet.Id].Count; j++)
                    {
                        if (currentName.ToUpper() == project.TileObjects[tileSheet.Id][j].Name.ToUpper())
                        {
                            isNameValid = false;

                            break;
                        }
                    }

                    if (isNameValid == false)
                    {
                        break;
                    }
                }

                counter++;
            }
            
            TileObjectDto tileObject = tileSheetViewerControl_.GetSelectionAsObject();

            tileObject.OwnerId = selectedTileSheetProxy.Id;

            tileObject.Name = currentName;

            projectController_.AddTileObject(selectedTileSheetProxy.Id, tileObject);

            TreeNode tileSheetNode = getSelectedTileSheetTreeNode();

            addTileSheetObjectToTree(tileSheetNode, tileObject);
        }

        private void addNewTileSheet()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int tileSize = project.TileSize;

            OpenFileDialog openTileset = new OpenFileDialog();

            openTileset.CheckFileExists = true;
            openTileset.CheckPathExists = true;
            openTileset.DefaultExt = "png";
            openTileset.Filter = "PNG Files|*.png";
            openTileset.FileName = string.Empty;
            openTileset.Multiselect = false;
            openTileset.RestoreDirectory = true;

            if (openTileset.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmpTileSheetImage = new Bitmap(openTileset.FileName);

                if (bmpTileSheetImage.Size.Width < tileSize || bmpTileSheetImage.Size.Height < tileSize)
                {
                    MessageBox.Show("Image dimensions must be at least" + tileSize + "x" + tileSize + ".", "Invalid Tile", MessageBoxButtons.OK);
                }
                else
                {
                    int tilesheetCount = project.TileSheets.Count + 1;

                    bool isNameValid = false;
                    int counter = 0;
                    string currentName = "New Tile Sheet";

                    // Find the first sequentially available name.
                    while (isNameValid == false)
                    {
                        isNameValid = true;

                        // The current name that is being checked for collision.
                        if (counter > 0)
                        {
                            currentName = "New Tile Sheet " + counter.ToString();
                        }

                        // Tile sheets and sprite sheets cannot have name collisions with each other.
                        for (int j = 0; j < project.TileSheets.Count; j++)
                        {
                            if (currentName.ToUpper() == project.TileSheets[j].Name.ToUpper())
                            {
                                isNameValid = false;
                                break;
                            }
                        }

                        for (int j = 0; j < project.SpriteSheets.Count; j++)
                        {
                            if (currentName.ToUpper() == project.SpriteSheets[j].Name.ToUpper())
                            {
                                isNameValid = false;
                                break;
                            }
                        }

                        counter++;
                    }

                    TileSheetDto newTileSheet = projectController_.AddTileSheet(openTileset.FileName, tileSize, currentName);

                    // Add tile sheet to tree.
                    addTileSheetToTree(newTileSheet);
                }
            }
        }

        private void addTileSheetNodes(ProjectDto project)
        {
            foreach (TileSheetDto tileSheet in project.TileSheets)
            {
                TreeNode tileSheetNode = addTileSheetToTree(tileSheet);

                foreach (TileObjectDto tileObject in project.TileObjects[tileSheet.Id])
                {
                    addTileSheetObjectToTree(tileSheetNode, tileObject);
                }
            }
        }

        TreeNode addTileSheetToTree(TileSheetDto tileSheet)
        {
            ITileSheetDtoProxy tileSheetProxy = firemelonEditorFactory_.NewTileSheetProxy(projectController_, tileSheet.Id);

            // If this tile sheet already has a node on the tree, ignore it.
            foreach (TreeNode node in tvTileSheets.Nodes)
            {
                ITileSheetDtoProxy nodeProxy = (ITileSheetDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == tileSheet.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode tileSheetNode = tvTileSheets.Nodes.Add("TILESHEET", tileSheet.Name);

            tileSheetNode.Nodes.Add("ANIMATIONROOT", "Animations");///.Tag = new AssetMenuDto(cmnuEvent, null);

            tileSheetNode.Nodes.Add("SCENERYROOT", "Scenery");///.Tag = new AssetMenuDto(cmnuEvent, null);
            
            tileSheetNode.Tag = new AssetMenuDto(cmnuTileSheet, tileSheetProxy);

            return tileSheetNode;
        }

        TreeNode addTileSheetObjectToTree(TreeNode tileSheetNode, TileObjectDto tileObject)
        {
            ITileObjectDtoProxy tileObjectProxy = firemelonEditorFactory_.NewTileObjectProxy(projectController_, tileObject.Id);

            // If this tile object already has a node on the tree, ignore it.
            foreach (TreeNode node in tileSheetNode.Nodes["SCENERYROOT"].Nodes)
            {
                ITileObjectDtoProxy nodeProxy = (ITileObjectDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == tileObject.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode tileObjectNode = tileSheetNode.Nodes["SCENERYROOT"].Nodes.Add("TILESHEETOBJECT", tileObject.Name);
            
            tileObjectNode.Tag = new AssetMenuDto(cmnuTileObject, tileObjectProxy);

            return tileObjectNode;
        }

        TreeNode addSceneryAnimationToTree(TreeNode tileSheetNode, SceneryAnimationDto sceneryAnimation)
        {
            ISceneryAnimationDtoProxy sceneryAnimationProxy = firemelonEditorFactory_.NewSceneryAnimationProxy(projectController_, sceneryAnimation.Id);

            // If this scenery animation already has a node on the tree, ignore it.
            foreach (TreeNode node in tileSheetNode.Nodes["ANIMATIONROOT"].Nodes)
            {
                ISceneryAnimationDtoProxy nodeProxy = (ISceneryAnimationDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == sceneryAnimation.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode sceneryAnimationNode = tileSheetNode.Nodes["ANIMATIONROOT"].Nodes.Add("SCENERYANIMATION", sceneryAnimation.Name);

            sceneryAnimationNode.Tag = new AssetMenuDto(cmnuSceneryAnimation, sceneryAnimationProxy);

            return sceneryAnimationNode;
        }


        private void deleteAnimation()
        {
            if (tvTileSheets.SelectedNode == null)
            {
                MessageBox.Show("Selected Tree Node is null. This is an unresolved bug.");

                System.Diagnostics.Debug.Print("TileSheetEditorControl handle = " + this.Handle.ToString());

                return;
            }

            projectController_.DeleteSceneryAnimation(getSelectedTileSheetTreeNode().Index, tvTileSheets.SelectedNode.Index);

            tvTileSheets.SelectedNode.Remove();
        }


        private void deleteTileObject()
        {
            if (tvTileSheets.SelectedNode == null)
            {
                MessageBox.Show("Selected Tree Node is null. This is an unresolved bug.");

                System.Diagnostics.Debug.Print("TileSheetEditorControl handle = " + this.Handle.ToString());

                return;
            }

            projectController_.DeleteTileObject(getSelectedTileSheetTreeNode().Index, tvTileSheets.SelectedNode.Index);

            tvTileSheets.SelectedNode.Remove();
        }
        

        private TreeNode getSelectedTileSheetTreeNode()
        {
            TreeNode tileSheetNode = null;

            switch (tvTileSheets.SelectedNode.Name)
            {
                case "ANIMATIONROOT":
                case "SCENERYROOT":

                    tileSheetNode = tvTileSheets.SelectedNode.Parent;
                    
                    break;
                    
                case "TILESHEET":

                    tileSheetNode = tvTileSheets.SelectedNode;

                    break;

                case "TILESHEETOBJECT":
                case "SCENERYANIMATION":

                    tileSheetNode = tvTileSheets.SelectedNode.Parent.Parent;
                    
                    break;                    
            }

            return tileSheetNode;
        }

        private void pruneTileSheetNodes()
        {
            // Remove from the tree any nodes whose corresponding object does not exist
            ProjectDto project = projectController_.GetProjectDto();

            int nodeCount = tvTileSheets.Nodes.Count;

            for (int i = nodeCount - 1; i >= 0; i--)
            {
                TreeNode node = tvTileSheets.Nodes[i];

                ITileSheetDtoProxy nodeProxy = (ITileSheetDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (projectController_.GetTileSheet(nodeProxy.Id) == null)
                {
                    // Tile sheet does not exist if a value of null is returned.
                    node.Remove();
                }
                else
                {
                    int tileObjectNodeCount = node.Nodes.Count;

                    // Check the tile objects for this sheet.
                    for (int j = tileObjectNodeCount - 1; j >= 0; j--)
                    {
                        TreeNode tileObjectNode = node.Nodes[j];

                        ITileObjectDtoProxy tileObjectNodeProxy = (ITileObjectDtoProxy)((AssetMenuDto)tileObjectNode.Tag).Asset;

                        if (projectController_.GetTileObject(tileObjectNodeProxy.Id) == null)
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
            pruneTileSheetNodes();
            
            // Then add any nodes that are missing.
            addTileSheetNodes(project);            
        }

        #endregion

        #region Event Handlers

        private void pgTileSheet_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "IMAGEPATH":
                case "SCALEFACTOR":
                case "ROWS":
                case "COLUMNS":

                    // Scroll bars likely need update and the image needs refreshed.
                    tileSheetViewerControl_.RefreshImage();

                    break;

                case "(NAME)":

                    try
                    {
                        //int oldIndex = lbxTileSheets.Items.IndexOf(e.OldValue.ToString());

                        //lbxTileSheets.Items[oldIndex] = e.ChangedItem.Value.ToString();

                        //lbxTileSheets.Sorted = false;
                        //lbxTileSheets.Sorted = true;

                        tvTileSheets.SelectedNode.Text = e.ChangedItem.Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Test");
                    }

                    break;
            }
        }

        private void TileSheetsEditorControl_Load(object sender, EventArgs e)
        {
            //if (tvTileSheets.Nodes.Count > 0)
            //{
            //    tvTileSheets.SelectedNode = tvTileSheets.Nodes[0];
            //}            
        }
        
        private void TileSheetsEditorControl_ProjectStateChanged(object sender, ProjectStateChangedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            updateTree(project);
        }

        private void TileSheetsEditorControl_TileSheetEditorSelectionChanged(object sender, TileSheetSelectionChangedEventArgs e)
        {
            OnTileSheetSelectionChanged(e);
        }
        
        private void tvTileSheets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //TreeNode selectedEntityNode = getSelectedEntityTreeNode();

            //EntityType selectedEntityType = getEntityTypeForNode(selectedEntityNode);

            // Initialize all to default states.
            pgTileSheet.SelectedObject = null;

            ITileSheetDtoProxy tileSheetProxy = null;

            switch (tvTileSheets.SelectedNode.Name)
            {
                case "TILESHEET":

                    tileSheetProxy = (ITileSheetDtoProxy)((AssetMenuDto)tvTileSheets.SelectedNode.Tag).Asset;

                    pgTileSheet.SelectedObject = tileSheetProxy;

                    GlobalVars.listOfSceneryAnimations_ = projectController_.GetSceneryAnimationNames(tileSheetProxy.Id).ToArray();

                    if (tileSheetViewerControl_.Sheet != null)
                    {
                        TileSheetDto tileSheetToUnload = projectController_.GetTileSheet(tileSheetViewerControl_.Sheet.Id);

                        TileSheetDto tileSheetToLoad = projectController_.GetTileSheet(tileSheetProxy.Id);

                        // Unload the existing tile sheet, if it is changing.
                        Guid resourceToUnloadId = tileSheetToUnload.BitmapResourceId;

                        Guid resourceToLoadId = tileSheetToLoad.BitmapResourceId;

                        if (resourceToUnloadId != resourceToLoadId)
                        {
                            projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.TileSheetViewer);
                        }
                    }

                    tileSheetViewerControl_.Sheet = tileSheetProxy;

                    ((Control)tileSheetViewerControl_).Visible = true;

                    ((Control)tileObjectViewerControl_).Visible = false;

                    break;

                case "TILESHEETOBJECT":

                    tileSheetProxy = (ITileSheetDtoProxy)((AssetMenuDto)tvTileSheets.SelectedNode.Parent.Parent.Tag).Asset;

                    ITileObjectDtoProxy tileSheetObjectProxy = (ITileObjectDtoProxy)((AssetMenuDto)tvTileSheets.SelectedNode.Tag).Asset;

                    pgTileSheet.SelectedObject = tileSheetObjectProxy;

                    GlobalVars.listOfSceneryAnimations_ = projectController_.GetSceneryAnimationNames(tileSheetProxy.Id).ToArray();

                    if (tileSheetViewerControl_.Sheet != null)
                    {
                        TileSheetDto tileSheetToUnload = projectController_.GetTileSheet(tileSheetViewerControl_.Sheet.Id);

                        TileSheetDto tileSheetToLoad = projectController_.GetTileSheet(tileSheetProxy.Id);

                        // Unload the existing tile sheet, if it is changing.
                        Guid resourceToUnloadId = tileSheetToUnload.BitmapResourceId;

                        Guid resourceToLoadId = tileSheetToLoad.BitmapResourceId;

                        if (resourceToUnloadId != resourceToLoadId)
                        {
                            projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.TileSheetViewer);
                        }
                    }

                    tileObjectViewerControl_.TileSheet = tileSheetProxy;

                    tileObjectViewerControl_.TileObject = tileSheetObjectProxy;

                    ((Control)tileSheetViewerControl_).Visible = false;

                    ((Control)tileObjectViewerControl_).Visible = true;

                    OnTileSheetSelectionChanged(new TileSheetSelectionChangedEventArgs(true, false, true, false));

                    break;

                case "SCENERYANIMATION":

                    tileSheetProxy = (ITileSheetDtoProxy)((AssetMenuDto)tvTileSheets.SelectedNode.Parent.Parent.Tag).Asset;

                    ISceneryAnimationDtoProxy sceneryAnimationProxy = (ISceneryAnimationDtoProxy)((AssetMenuDto)tvTileSheets.SelectedNode.Tag).Asset;

                    pgTileSheet.SelectedObject = sceneryAnimationProxy;

                    GlobalVars.listOfSceneryAnimations_ = projectController_.GetSceneryAnimationNames(tileSheetProxy.Id).ToArray();

                    if (tileSheetViewerControl_.Sheet != null)
                    {
                        TileSheetDto tileSheetToUnload = projectController_.GetTileSheet(tileSheetViewerControl_.Sheet.Id);

                        TileSheetDto tileSheetToLoad = projectController_.GetTileSheet(tileSheetProxy.Id);

                        // Unload the existing tile sheet, if it is changing.
                        Guid resourceToUnloadId = tileSheetToUnload.BitmapResourceId;

                        Guid resourceToLoadId = tileSheetToLoad.BitmapResourceId;

                        if (resourceToUnloadId != resourceToLoadId)
                        {
                            projectController_.UnloadBitmapResource(resourceToUnloadId, EditorModule.TileSheetViewer);
                        }
                    }

                    tileObjectViewerControl_.TileSheet = tileSheetProxy;

                    tileObjectViewerControl_.TileObject = null;

                    ((Control)tileSheetViewerControl_).Visible = false;

                    ((Control)tileObjectViewerControl_).Visible = true;

                    OnTileSheetSelectionChanged(new TileSheetSelectionChangedEventArgs(true, false, false, true));

                    break;

                default:

                    OnTileSheetSelectionChanged(new TileSheetSelectionChangedEventArgs(true, false, false, false));

                    break;

            }
        }

        #endregion

        #region Event Dispatchers

        protected virtual void OnTileSheetSelectionChanged(TileSheetSelectionChangedEventArgs e)
        {
            TileSheetSelectionChanged(this, e);
        }

        #endregion
    }
}
