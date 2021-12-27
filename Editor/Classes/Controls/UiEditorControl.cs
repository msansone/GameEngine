using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class UiEditorControl : UserControl, IUiEditorControl
    {
        private IExceptionHandler exceptionHandler_;

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private INameGenerator nameGenerator_;

        private IProjectController projectController_;

        public UiEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            InitializeComponent();

            exceptionHandler_ = exceptionHandler;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            nameGenerator_ = nameGenerator;

            projectController_ = projectController;

            ProjectDto project = projectController_.GetProjectDto();

            tvUi.Nodes.Add("WIDGETROOT", "UI Widgets").Tag = new AssetMenuDto(cmnuUiWidgetRoot, null);

            ScriptDto uiManagerScript = project.Scripts[Globals.UiManagerId];

            IScriptDtoProxy uiManagerScriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, uiManagerScript.Id);

            tvUi.Nodes.Add("SCRIPTROOT", "Script").Tag = new AssetMenuDto(cmnuScript, uiManagerScriptProxy);

            DataFileDto uiPanelsJsonFile = project.DataFiles[Globals.PanelsJsonFileId];

            IDataFileDtoProxy uiPanelsJsonFileProxy = firemelonEditorFactory_.NewDataFileProxy(projectController_, uiPanelsJsonFile.Id);

            tvUi.Nodes.Add("PANELSROOT", "Panels").Tag = new AssetMenuDto(cmnuDataFile, uiPanelsJsonFileProxy);

            foreach (UiWidgetDto uiWidget in project.UiWidgets)
            {
                addWidgetToTree(uiWidget);
            }
        }

        public void AddNew()
        {
            addWidget();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        #region Private Functions

        private void addWidget()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int menuItemCount = tvUi.Nodes["WIDGETROOT"].Nodes.Count + 1;

            string name = getNextAvailableName("NewWidget");

            UiWidgetDto newWidget = projectController_.AddUiWidget(name);

            addWidgetToTree(newWidget);
        }

        private void addWidgetToTree(UiWidgetDto uiWidget)
        {
            IUiWidgetDtoProxy menuItemProxy = firemelonEditorFactory_.NewUiWidgetProxy(projectController_, firemelonEditorFactory_.NewExceptionHandler(), uiWidget.Id);

            // If this menu item already has a node on the tree, ignore it.
            foreach (TreeNode node in tvUi.Nodes["WIDGETROOT"].Nodes)
            {
                IUiWidgetDtoProxy nodeProxy = (IUiWidgetDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == menuItemProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return;
                }
            }

            TreeNode menuItemNode = tvUi.Nodes["WIDGETROOT"].Nodes.Add("WIDGETROOT", menuItemProxy.Name);

            menuItemNode.Tag = new AssetMenuDto(cmnuUiWidget, menuItemProxy);

            ProjectDto project = projectController_.GetProjectDto();

            ScriptDto script = project.Scripts[uiWidget.Id];

            IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

            menuItemNode.Nodes.Add("SCRIPTROOT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);
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

        #endregion

        #region Event Handlers

        #endregion

        private void tvUi_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            // Initialize all to default states.
            pgUi.SelectedObject = null;
            
            switch (tvUi.SelectedNode.Name)
            {
                case "WIDGETROOT":

                    IUiWidgetDtoProxy uiWidgetProxy = (IUiWidgetDtoProxy)((AssetMenuDto)tvUi.SelectedNode.Tag).Asset;

                    pgUi.SelectedObject = uiWidgetProxy;

                    //tileSheetViewerControl_.Sheet = tileSheetProxy;

                    //((Control)tileSheetViewerControl_).Visible = true;

                    //((Control)tileObjectViewerControl_).Visible = false;

                    break;

                case "SCRIPTROOT":

                    IScriptDtoProxy scriptProxy = (IScriptDtoProxy)((AssetMenuDto)tvUi.SelectedNode.Tag).Asset;

                    pgUi.SelectedObject = scriptProxy;

                    //tileSheetViewerControl_.Sheet = tileSheetProxy;

                    //((Control)tileSheetViewerControl_).Visible = true;

                    //((Control)tileObjectViewerControl_).Visible = false;

                    break;

                case "PANELSROOT":

                    IDataFileDtoProxy dataFileProxy = (IDataFileDtoProxy)((AssetMenuDto)tvUi.SelectedNode.Tag).Asset;
                    
                    //pgUi.SelectedObject = dataFileProxy;

                    //tileObjectViewerControl_.TileSheet = tileSheetProxy;

                    //tileObjectViewerControl_.TileObject = tileSheetObjectProxy;

                    //((Control)tileSheetViewerControl_).Visible = false;

                    //((Control)tileObjectViewerControl_).Visible = true;

                    break;
            }
        }
    }
}