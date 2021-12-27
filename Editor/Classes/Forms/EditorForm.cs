using SpriteSheetBuilder;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace FiremelonEditor2
{
    using WeifenLuo.WinFormsUI.Docking;
    using ProjectDto = ProjectDto_2_2;

    /// <summary>
    /// Summary description for EditorForm.nrbn
    /// nrbn
    /// </summary>
    /// 

    public class EditorForm : System.Windows.Forms.Form
    {
        #region Private Variables

        #region Auto-generated controls

        public System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem mnuEdit;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem mnuView;
        private System.Windows.Forms.MenuItem menuItem25;
        private System.Windows.Forms.MenuItem menuItem27;
        private System.Windows.Forms.MenuItem mnuShowgrid;
        private System.Windows.Forms.MenuItem mnuNewProject;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.MenuItem mnuTransparentSelect;
        private System.Windows.Forms.MenuItem mnuUndo;
        private System.Windows.Forms.MenuItem mnuRedo;
        private System.Windows.Forms.MenuItem mnuCut;
        private System.Windows.Forms.MenuItem mnuCopy;
        private System.Windows.Forms.MenuItem mnuPaste;
        private System.Windows.Forms.MenuItem mnuDelete;
        private System.Windows.Forms.MenuItem mnuContents;
        private System.Windows.Forms.MenuItem mnuAbout;
        private System.Windows.Forms.MenuItem mnuFill;
        private System.Windows.Forms.MenuItem mnuOpenProject;
        private System.Windows.Forms.MenuItem mnuSaveProject;
        private System.Windows.Forms.MenuItem mnuSaveProjectAs;

        private MenuItem mnuExit;
        private MenuItem mnuTools;
        private MenuItem mnuToolsDraw;
        private MenuItem mnuToolsSelect;
        private MenuItem mnuToolsGrab;
        private MenuItem mnuViewCameraLock;
        private StatusStrip ssStatus;
        private MenuItem mnuToolsResetPopouts;
        private MenuItem mnuLayersPopout;
        private MenuItem mnuAssetsPopout;
        private MenuItem mnuShowOutlines;
        private MenuItem mnuRoomsPopout;
        private MenuItem mnuConfig;
        private MenuItem mnuSelectFolder;
        private FolderBrowserDialog folderBrowserDialog1;
        private MenuItem mnuExportAndRun;
        private ToolStripStatusLabel tsslbCameraMode;
        private ToolStripStatusLabel tsslbProjectFolder;
        private MenuItem mnuExport;        
        private MenuItem mnuRunWithConsole;
        private MenuItem mnuPropertyGridPopout;
        private ToolStripStatusLabel tsslbTileSelection;
        private MenuItem mnuShowCameraOutline;
        private MenuItem menuItem1;
        private MenuItem mnuShowWarnings;
        private BackgroundWorker bgWorkLoadFile;
        private BackgroundWorker bgWorkPrepareFile;
        private MenuItem mnuChangeProjectName;
        private MenuItem mnuManageScripts;
        private MenuItem menuItem2;
        private MenuItem mnuSpriteSheetBuilder;
        private MenuItem mnuRecentProjects;
        private MenuItem menuItem3;
        private Ribbon rbnToolbar;
        private RibbonTab rbtEdit;
        private RibbonPanel rpnSelect;
        private RibbonPanel rpnlUndoRedo;
        private RibbonTab rtabTools;
        private RibbonTab rtabView;
        private RibbonButton rbtnUndo;
        private RibbonButton rbtnRedo;
        private RibbonTab rtabConfig;
        private RibbonButton rbtnCopy;
        private RibbonButton rbtnPaste;
        private RibbonButton rbtnCut;
        private RibbonButton rbtnDelete;
        private RibbonPanel rpnlMode;
        private RibbonButton rbtnDraw;
        private RibbonButton rbtnSelect;
        private RibbonButton rbtnScroll;
        private RibbonPanel rpnlPopouts;
        private RibbonButton rbtnRooms;
        private RibbonPanel rpnlPopups;
        private RibbonButton rbtnAssets;
        private RibbonButton rbtnScripts;
        private RibbonButton rbtnSpriteSheetBuilder;
        private RibbonPanel rpnlShowHide;
        private RibbonPanel rpnlProjectSettings;
        private RibbonButton rbtnSelectProjectFolder;
        private RibbonButton rbtnChangeName;
        private RibbonOrbMenuItem romiOpen;
        private RibbonOrbMenuItem romiSave;
        private RibbonOrbMenuItem romiSaveAs;
        private RibbonSeparator ribbonSeparator1;
        private RibbonOrbMenuItem romiRunConsole;
        private RibbonOrbMenuItem romiRun;
        private Timer tmrPostLoad;
        private RibbonOrbMenuItem romiNew;
        private RibbonCheckBox rbtnShowGrid;
        private RibbonCheckBox rcbShowOutlines;
        private RibbonCheckBox rcbCameraOutline;
        private RibbonCheckBox rcbShowWorldGeometry;
        private RibbonCheckBox rcbFillSelectedItems;
        private RibbonCheckBox rcbLockCamera;

        #endregion

        private IFiremelonEditorFactory firemelonEditorFactory_;
        private IUtilityFactory utilityFactory_;

        private IProjectController projectController_;
        private IProjectLauncher projectLauncher_;
        
        private IAssetEditor assetEditor_;
        //private IAssetSelectionForm assetSelectionForm_;
        private IPopoutForm assetSelectionForm_;
        private IChangeProjectNameDialog changeProjectNameDialog_;
        private IExceptionHandler exceptionHandler_;
        private IPopoutForm instancesForm_;
        //private ILayerListForm layerListForm_;
        private IPopoutForm layerListForm_;
        private INameGenerator nameGenerator_;
        private INameValidator nameValidator_;
        private INewProjectDialog newProjectDialog_;
        private IProgressForm progressForm_;
        private IPopoutForm propertyGridForm_;
        //private IRoomListForm roomListForm_;        
        private IResourcesForm resourcesForm_;
        private IPopoutForm roomListForm_;
        private IScriptsForm scriptsForm_;
        private ISpriteSheetBuilderDialog sheetBuilderForm_;

        private string projectFileName_;
        
        private RibbonCheckBox rcbSelectActors;
        private RibbonCheckBox rcbSelectEvents;
        private RibbonCheckBox rcbSelectHudElements;
        private RibbonCheckBox rcbTileObjects;
        private RibbonCheckBox rcbWorldGeometry;
        private RibbonCheckBox rcbSpawnPoints;
        private RibbonCheckBox rcbSelectAudioSources;
        private RibbonCheckBox rcbParticleEmitters;
        private WeifenLuo.WinFormsUI.Docking.DockPanel pnlMain;
        private Dictionary<string, string> recentlyOpenedProjects_ = new Dictionary<string, string>();
        private RibbonButton rbtnLayers;
        private RibbonButton rbtnAssetSelector;
        private RibbonButton rbtnInstances;
        private RibbonButton rbtnResources;
        private RibbonButton ribbonButton1;
        private RibbonCheckBox rcbShowWarnings;
        private RibbonButton rbtnProperties;
        
        #endregion

        #region Constructors

        public EditorForm()
        {
            Application.EnableVisualStyles();
            
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();


            firemelonEditorFactory_ = new FiremelonEditorFactory();

            utilityFactory_ = new UtilityFactory();
            
            nameGenerator_ = utilityFactory_.NewNameGenerator();
            nameValidator_ = utilityFactory_.NewNameValidator(nameGenerator_);

            exceptionHandler_ = firemelonEditorFactory_.NewExceptionHandler();

            projectController_ = firemelonEditorFactory_.NewProjectController(nameValidator_, nameGenerator_, exceptionHandler_);
            projectController_.CameraModeChanged += new CameraModeChangedHandler(this.EditorForm_CameraModeChanged);
            projectController_.EditModeChanged += new EditModeChangedHandler(this.EditorForm_EditModeChanged);
            projectController_.ProjectStateChanged += new ProjectStateChangeHandler(this.EditorForm_ProjectStateChanged);
            projectController_.SelectionToggle += new SelectionToggleHandler(this.EditorForm_SelectionToggled);
            projectController_.RoomSelected += new RoomSelectHandler(this.EditorForm_RoomSelected);

            projectLauncher_ = firemelonEditorFactory_.NewProjectLauncher(projectController_, this);

            rbnToolbar.Expanded = false;

            newProjectDialog_ = firemelonEditorFactory_.NewNewProjectDialog(projectController_);
            
            projectFileName_ = string.Empty;

            IRoomEditorControl roomEditorControl = firemelonEditorFactory_.NewRoomEditorControl(projectController_);

            IPopoutForm roomEditorForm = firemelonEditorFactory_.NewPopoutForm((Control)roomEditorControl, "Editor");

            roomEditorForm.Show(pnlMain, DockState.Document);

            ((DockContent)roomEditorForm).DockHandler.AllowEndUserDocking = false;
            ((DockContent)roomEditorForm).DockHandler.CloseButtonVisible = false;
            ((DockContent)roomEditorForm).DockAreas = DockAreas.Document;

            // Create the pop-out forms.
            ILayerListControl layerListControl = firemelonEditorFactory_.NewLayerListControl(projectController_);
            layerListForm_ = firemelonEditorFactory_.NewPopoutForm((Control)layerListControl, "Layers");
            layerListForm_.TopLevel = true;
            //layerListForm_.FormHidden += new FormHiddenHandler(this.EditorForm_LayerFormHidden);
            layerListForm_.Show(pnlMain, DockState.DockRight);
            

            IAssetSelectionControl assetSelectionControl = firemelonEditorFactory_.NewAssetSelectionControl(projectController_);

            //assetSelectionForm_ = firemelonEditorFactory_.NewAssetSelectionForm(projectController_, this);
            assetSelectionForm_ = firemelonEditorFactory_.NewPopoutForm((Control)assetSelectionControl, "Assets");
            assetSelectionForm_.TopLevel = true;
            //assetSelectionForm_.FormHidden += new FormHiddenHandler(this.EditorForm_AssetSelectionFormHidden);

            assetSelectionForm_.Show(pnlMain, DockState.DockLeft);

            assetEditor_ = firemelonEditorFactory_.NewAssetEditor();

            //roomListForm_ = firemelonEditorFactory_.NewRoomListForm(projectController_, nameValidator_);
            IRoomListControl roomListControl = firemelonEditorFactory_.NewRoomListControl(projectController_, nameValidator_);

            roomListForm_ = firemelonEditorFactory_.NewPopoutForm((Control)roomListControl, "Rooms");
            roomListForm_.TopLevel = true;
            //roomListForm_.FormHidden += new FormHiddenHandler(this.EditorForm_RoomsFormHidden);

            roomListForm_.Show(layerListForm_.Pane, DockAlignment.Top, 0.5);


            IMapWidgetPropertiesControl propertiesControl = firemelonEditorFactory_.NewMapWidgetPropertiesControl(projectController_);
            propertyGridForm_ = firemelonEditorFactory_.NewPopoutForm((Control)propertiesControl, "Properties");
            propertyGridForm_.TopLevel = true;

            
            propertyGridForm_.Show(assetSelectionForm_.Pane, DockAlignment.Bottom, 0.66);
            
            //propertyGridForm_.Dockwui .FloatPane, DockState.DockBottom);

            assetSelectionControl.PropertiesControl = propertiesControl;

            roomEditorControl.CursorChanged += new CursorChangedHandler(this.EditorForm_CursorChanged);

            progressForm_ = firemelonEditorFactory_.NewProgressForm(projectController_);

            instancesForm_ = firemelonEditorFactory_.NewPopoutForm((Control)firemelonEditorFactory_.NewMapWidgetInstanceListControl(projectController_, propertiesControl), "Instances");
            instancesForm_.TopLevel = true;
            //instancePropertiesForm_.FormHidden += new FormHiddenHandler(this.EditorForm_WidgetInstanceListFormHidden);
            //instancePropertiesForm_.Text = "Instances";

            instancesForm_.Show(propertyGridForm_.Pane, DockAlignment.Top, 0.5);

            scriptsForm_ = firemelonEditorFactory_.NewScriptsForm(projectController_, nameGenerator_);

            resourcesForm_ = firemelonEditorFactory_.NewResourcesForm(projectController_);

            sheetBuilderForm_ = firemelonEditorFactory_.NewSheetBuilderForm();

            changeProjectNameDialog_ = firemelonEditorFactory_.NewChangeProjectNameDialog();
            changeProjectNameDialog_.ProjectNameChanged += new ProjectNameChangedHandler(this.EditorForm_ProjectNameChanged);

            mnuLayersPopout.Enabled = false;

            tmrPostLoad.Enabled = true;

            pnlMain.DockLeftPortion = 250.0;
            pnlMain.DockRightPortion = 200.0;
        }

        #endregion

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuNewProject = new System.Windows.Forms.MenuItem();
            this.mnuOpenProject = new System.Windows.Forms.MenuItem();
            this.mnuSaveProject = new System.Windows.Forms.MenuItem();
            this.mnuSaveProjectAs = new System.Windows.Forms.MenuItem();
            this.mnuExport = new System.Windows.Forms.MenuItem();
            this.mnuExportAndRun = new System.Windows.Forms.MenuItem();
            this.mnuRunWithConsole = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.mnuRecentProjects = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuUndo = new System.Windows.Forms.MenuItem();
            this.mnuRedo = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.mnuCut = new System.Windows.Forms.MenuItem();
            this.mnuCopy = new System.Windows.Forms.MenuItem();
            this.mnuPaste = new System.Windows.Forms.MenuItem();
            this.mnuDelete = new System.Windows.Forms.MenuItem();
            this.mnuFill = new System.Windows.Forms.MenuItem();
            this.mnuTools = new System.Windows.Forms.MenuItem();
            this.mnuToolsDraw = new System.Windows.Forms.MenuItem();
            this.mnuToolsSelect = new System.Windows.Forms.MenuItem();
            this.mnuToolsGrab = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuToolsResetPopouts = new System.Windows.Forms.MenuItem();
            this.mnuRoomsPopout = new System.Windows.Forms.MenuItem();
            this.mnuLayersPopout = new System.Windows.Forms.MenuItem();
            this.mnuAssetsPopout = new System.Windows.Forms.MenuItem();
            this.mnuPropertyGridPopout = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuManageScripts = new System.Windows.Forms.MenuItem();
            this.mnuSpriteSheetBuilder = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuShowgrid = new System.Windows.Forms.MenuItem();
            this.mnuShowOutlines = new System.Windows.Forms.MenuItem();
            this.mnuTransparentSelect = new System.Windows.Forms.MenuItem();
            this.mnuViewCameraLock = new System.Windows.Forms.MenuItem();
            this.mnuShowCameraOutline = new System.Windows.Forms.MenuItem();
            this.mnuConfig = new System.Windows.Forms.MenuItem();
            this.mnuSelectFolder = new System.Windows.Forms.MenuItem();
            this.mnuChangeProjectName = new System.Windows.Forms.MenuItem();
            this.mnuShowWarnings = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.mnuContents = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tsslbCameraMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslbProjectFolder = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslbTileSelection = new System.Windows.Forms.ToolStripStatusLabel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.bgWorkLoadFile = new System.ComponentModel.BackgroundWorker();
            this.bgWorkPrepareFile = new System.ComponentModel.BackgroundWorker();
            this.rbnToolbar = new System.Windows.Forms.Ribbon();
            this.romiNew = new System.Windows.Forms.RibbonOrbMenuItem();
            this.romiOpen = new System.Windows.Forms.RibbonOrbMenuItem();
            this.romiSave = new System.Windows.Forms.RibbonOrbMenuItem();
            this.romiSaveAs = new System.Windows.Forms.RibbonOrbMenuItem();
            this.ribbonSeparator1 = new System.Windows.Forms.RibbonSeparator();
            this.romiRun = new System.Windows.Forms.RibbonOrbMenuItem();
            this.romiRunConsole = new System.Windows.Forms.RibbonOrbMenuItem();
            this.rtabTools = new System.Windows.Forms.RibbonTab();
            this.rpnlMode = new System.Windows.Forms.RibbonPanel();
            this.rbtnDraw = new System.Windows.Forms.RibbonButton();
            this.rbtnSelect = new System.Windows.Forms.RibbonButton();
            this.rbtnScroll = new System.Windows.Forms.RibbonButton();
            this.rpnlPopouts = new System.Windows.Forms.RibbonPanel();
            this.rbtnRooms = new System.Windows.Forms.RibbonButton();
            this.rbtnLayers = new System.Windows.Forms.RibbonButton();
            this.rbtnAssetSelector = new System.Windows.Forms.RibbonButton();
            this.rbtnInstances = new System.Windows.Forms.RibbonButton();
            this.rbtnProperties = new System.Windows.Forms.RibbonButton();
            this.rpnlPopups = new System.Windows.Forms.RibbonPanel();
            this.rbtnAssets = new System.Windows.Forms.RibbonButton();
            this.rbtnScripts = new System.Windows.Forms.RibbonButton();
            this.rbtnResources = new System.Windows.Forms.RibbonButton();
            this.rbtnSpriteSheetBuilder = new System.Windows.Forms.RibbonButton();
            this.rbtEdit = new System.Windows.Forms.RibbonTab();
            this.rpnSelect = new System.Windows.Forms.RibbonPanel();
            this.rbtnCopy = new System.Windows.Forms.RibbonButton();
            this.rbtnPaste = new System.Windows.Forms.RibbonButton();
            this.rbtnCut = new System.Windows.Forms.RibbonButton();
            this.rbtnDelete = new System.Windows.Forms.RibbonButton();
            this.rcbSelectActors = new System.Windows.Forms.RibbonCheckBox();
            this.rcbSelectEvents = new System.Windows.Forms.RibbonCheckBox();
            this.rcbSelectHudElements = new System.Windows.Forms.RibbonCheckBox();
            this.rcbTileObjects = new System.Windows.Forms.RibbonCheckBox();
            this.rcbWorldGeometry = new System.Windows.Forms.RibbonCheckBox();
            this.rcbSpawnPoints = new System.Windows.Forms.RibbonCheckBox();
            this.rcbSelectAudioSources = new System.Windows.Forms.RibbonCheckBox();
            this.rcbParticleEmitters = new System.Windows.Forms.RibbonCheckBox();
            this.rpnlUndoRedo = new System.Windows.Forms.RibbonPanel();
            this.rbtnUndo = new System.Windows.Forms.RibbonButton();
            this.rbtnRedo = new System.Windows.Forms.RibbonButton();
            this.rtabView = new System.Windows.Forms.RibbonTab();
            this.rpnlShowHide = new System.Windows.Forms.RibbonPanel();
            this.rbtnShowGrid = new System.Windows.Forms.RibbonCheckBox();
            this.rcbShowOutlines = new System.Windows.Forms.RibbonCheckBox();
            this.rcbCameraOutline = new System.Windows.Forms.RibbonCheckBox();
            this.rcbShowWorldGeometry = new System.Windows.Forms.RibbonCheckBox();
            this.rcbFillSelectedItems = new System.Windows.Forms.RibbonCheckBox();
            this.rcbLockCamera = new System.Windows.Forms.RibbonCheckBox();
            this.rtabConfig = new System.Windows.Forms.RibbonTab();
            this.rpnlProjectSettings = new System.Windows.Forms.RibbonPanel();
            this.rbtnSelectProjectFolder = new System.Windows.Forms.RibbonButton();
            this.rbtnChangeName = new System.Windows.Forms.RibbonButton();
            this.tmrPostLoad = new System.Windows.Forms.Timer(this.components);
            this.pnlMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.ribbonButton1 = new System.Windows.Forms.RibbonButton();
            this.rcbShowWarnings = new System.Windows.Forms.RibbonCheckBox();
            this.ssStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // ilIcons
            // 
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.Magenta;
            this.ilIcons.Images.SetKeyName(0, "");
            this.ilIcons.Images.SetKeyName(1, "");
            this.ilIcons.Images.SetKeyName(2, "");
            this.ilIcons.Images.SetKeyName(3, "");
            this.ilIcons.Images.SetKeyName(4, "cubist.bmp");
            this.ilIcons.Images.SetKeyName(5, "cubist2.bmp");
            this.ilIcons.Images.SetKeyName(6, "L1.bmp");
            this.ilIcons.Images.SetKeyName(7, "L2.bmp");
            this.ilIcons.Images.SetKeyName(8, "L3.bmp");
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuTools,
            this.mnuView,
            this.mnuConfig,
            this.menuItem25});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNewProject,
            this.mnuOpenProject,
            this.mnuSaveProject,
            this.mnuSaveProjectAs,
            this.mnuExport,
            this.mnuExportAndRun,
            this.mnuRunWithConsole,
            this.menuItem10,
            this.mnuRecentProjects,
            this.menuItem3,
            this.mnuExit});
            this.mnuFile.Text = "File";
            this.mnuFile.Visible = false;
            // 
            // mnuNewProject
            // 
            this.mnuNewProject.Index = 0;
            this.mnuNewProject.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.mnuNewProject.ShowShortcut = false;
            this.mnuNewProject.Text = "New Project";
            this.mnuNewProject.Click += new System.EventHandler(this.mnuNewProject_Click);
            // 
            // mnuOpenProject
            // 
            this.mnuOpenProject.Index = 1;
            this.mnuOpenProject.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuOpenProject.ShowShortcut = false;
            this.mnuOpenProject.Text = "Open Project";
            this.mnuOpenProject.Click += new System.EventHandler(this.mnuOpenProject_Click);
            // 
            // mnuSaveProject
            // 
            this.mnuSaveProject.Enabled = false;
            this.mnuSaveProject.Index = 2;
            this.mnuSaveProject.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuSaveProject.ShowShortcut = false;
            this.mnuSaveProject.Text = "Save Project";
            this.mnuSaveProject.Click += new System.EventHandler(this.mnuSaveProject_Click);
            // 
            // mnuSaveProjectAs
            // 
            this.mnuSaveProjectAs.Enabled = false;
            this.mnuSaveProjectAs.Index = 3;
            this.mnuSaveProjectAs.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
            this.mnuSaveProjectAs.ShowShortcut = false;
            this.mnuSaveProjectAs.Text = "Save Project As";
            this.mnuSaveProjectAs.Click += new System.EventHandler(this.mnuSaveProjectAs_Click);
            // 
            // mnuExport
            // 
            this.mnuExport.Enabled = false;
            this.mnuExport.Index = 4;
            this.mnuExport.Text = "Export";
            this.mnuExport.Visible = false;
            this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click);
            // 
            // mnuExportAndRun
            // 
            this.mnuExportAndRun.Enabled = false;
            this.mnuExportAndRun.Index = 5;
            this.mnuExportAndRun.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.mnuExportAndRun.Text = "Run";
            this.mnuExportAndRun.Click += new System.EventHandler(this.mnuExportAndRun_Click);
            // 
            // mnuRunWithConsole
            // 
            this.mnuRunWithConsole.Enabled = false;
            this.mnuRunWithConsole.Index = 6;
            this.mnuRunWithConsole.Shortcut = System.Windows.Forms.Shortcut.CtrlF5;
            this.mnuRunWithConsole.Text = "Run With Console";
            this.mnuRunWithConsole.Click += new System.EventHandler(this.mnuRunWithConsole_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 7;
            this.menuItem10.Text = "-";
            // 
            // mnuRecentProjects
            // 
            this.mnuRecentProjects.Index = 8;
            this.mnuRecentProjects.Text = "Recent Projects";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 9;
            this.menuItem3.Text = "-";
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 10;
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Enabled = false;
            this.mnuEdit.Index = 1;
            this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuUndo,
            this.mnuRedo,
            this.menuItem15,
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste,
            this.mnuDelete,
            this.mnuFill});
            this.mnuEdit.Text = "Edit";
            this.mnuEdit.Visible = false;
            this.mnuEdit.Popup += new System.EventHandler(this.mnuEdit_Popup);
            // 
            // mnuUndo
            // 
            this.mnuUndo.Enabled = false;
            this.mnuUndo.Index = 0;
            this.mnuUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.mnuUndo.Text = "Undo";
            this.mnuUndo.Click += new System.EventHandler(this.mnuUndo_Click);
            // 
            // mnuRedo
            // 
            this.mnuRedo.Enabled = false;
            this.mnuRedo.Index = 1;
            this.mnuRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.mnuRedo.Text = "Redo";
            this.mnuRedo.Click += new System.EventHandler(this.mnuRedo_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 2;
            this.menuItem15.Text = "-";
            // 
            // mnuCut
            // 
            this.mnuCut.Enabled = false;
            this.mnuCut.Index = 3;
            this.mnuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.mnuCut.Text = "Cut";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Enabled = false;
            this.mnuCopy.Index = 4;
            this.mnuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuCopy.Text = "Copy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Enabled = false;
            this.mnuPaste.Index = 5;
            this.mnuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.mnuPaste.Text = "Paste";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Enabled = false;
            this.mnuDelete.Index = 6;
            this.mnuDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.mnuDelete.Text = "Delete";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // mnuFill
            // 
            this.mnuFill.Enabled = false;
            this.mnuFill.Index = 7;
            this.mnuFill.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.mnuFill.Text = "Fill";
            this.mnuFill.Click += new System.EventHandler(this.mnuFill_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.Enabled = false;
            this.mnuTools.Index = 2;
            this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuToolsDraw,
            this.mnuToolsSelect,
            this.mnuToolsGrab,
            this.menuItem1,
            this.mnuToolsResetPopouts,
            this.mnuRoomsPopout,
            this.mnuLayersPopout,
            this.mnuAssetsPopout,
            this.mnuPropertyGridPopout,
            this.menuItem2,
            this.mnuManageScripts,
            this.mnuSpriteSheetBuilder});
            this.mnuTools.Text = "Tools";
            this.mnuTools.Visible = false;
            // 
            // mnuToolsDraw
            // 
            this.mnuToolsDraw.Checked = true;
            this.mnuToolsDraw.Index = 0;
            this.mnuToolsDraw.Text = "Draw";
            this.mnuToolsDraw.Click += new System.EventHandler(this.mnuToolsDraw_Click);
            // 
            // mnuToolsSelect
            // 
            this.mnuToolsSelect.Index = 1;
            this.mnuToolsSelect.Text = "Select";
            this.mnuToolsSelect.Click += new System.EventHandler(this.mnuToolsSelect_Click);
            // 
            // mnuToolsGrab
            // 
            this.mnuToolsGrab.Index = 2;
            this.mnuToolsGrab.Text = "Grab";
            this.mnuToolsGrab.Click += new System.EventHandler(this.mnuToolsGrab_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // mnuToolsResetPopouts
            // 
            this.mnuToolsResetPopouts.Index = 4;
            this.mnuToolsResetPopouts.Text = "Anchor Popout Windows";
            this.mnuToolsResetPopouts.Click += new System.EventHandler(this.mnuToolsResetPopouts_Click);
            // 
            // mnuRoomsPopout
            // 
            this.mnuRoomsPopout.Index = 5;
            this.mnuRoomsPopout.Text = "Rooms";
            this.mnuRoomsPopout.Click += new System.EventHandler(this.mnuRoomsPopout_Click);
            // 
            // mnuLayersPopout
            // 
            this.mnuLayersPopout.Index = 6;
            this.mnuLayersPopout.Text = "Layers";
            this.mnuLayersPopout.Click += new System.EventHandler(this.mnuLayersPopout_Click);
            // 
            // mnuAssetsPopout
            // 
            this.mnuAssetsPopout.Index = 7;
            this.mnuAssetsPopout.Text = "Assets";
            this.mnuAssetsPopout.Click += new System.EventHandler(this.mnuAssetsPopout_Click);
            // 
            // mnuPropertyGridPopout
            // 
            this.mnuPropertyGridPopout.Index = 8;
            this.mnuPropertyGridPopout.Text = "Properties";
            this.mnuPropertyGridPopout.Click += new System.EventHandler(this.mnuPropertyGridPopout_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 9;
            this.menuItem2.Text = "-";
            // 
            // mnuManageScripts
            // 
            this.mnuManageScripts.Index = 10;
            this.mnuManageScripts.Text = "Manage Scripts";
            this.mnuManageScripts.Click += new System.EventHandler(this.mnuManageScripts_Click);
            // 
            // mnuSpriteSheetBuilder
            // 
            this.mnuSpriteSheetBuilder.Index = 11;
            this.mnuSpriteSheetBuilder.Text = "Sprite Sheet Builder";
            this.mnuSpriteSheetBuilder.Click += new System.EventHandler(this.mnuSpriteSheetBuilder_Click);
            // 
            // mnuView
            // 
            this.mnuView.Enabled = false;
            this.mnuView.Index = 3;
            this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuShowgrid,
            this.mnuShowOutlines,
            this.mnuTransparentSelect,
            this.mnuViewCameraLock,
            this.mnuShowCameraOutline});
            this.mnuView.Text = "View";
            this.mnuView.Visible = false;
            // 
            // mnuShowgrid
            // 
            this.mnuShowgrid.Checked = true;
            this.mnuShowgrid.Index = 0;
            this.mnuShowgrid.Text = "Show Grid";
            this.mnuShowgrid.Click += new System.EventHandler(this.mnuShowgrid_Click);
            // 
            // mnuShowOutlines
            // 
            this.mnuShowOutlines.Checked = true;
            this.mnuShowOutlines.Index = 1;
            this.mnuShowOutlines.Text = "Show Outlines";
            this.mnuShowOutlines.Click += new System.EventHandler(this.mnuShowOutlines_Click);
            // 
            // mnuTransparentSelect
            // 
            this.mnuTransparentSelect.Checked = true;
            this.mnuTransparentSelect.Index = 2;
            this.mnuTransparentSelect.Text = "Transparent Selection";
            this.mnuTransparentSelect.Click += new System.EventHandler(this.mnuTransparentSelect_Click);
            // 
            // mnuViewCameraLock
            // 
            this.mnuViewCameraLock.Checked = true;
            this.mnuViewCameraLock.Index = 3;
            this.mnuViewCameraLock.Text = "Locked Camera";
            this.mnuViewCameraLock.Click += new System.EventHandler(this.mnuViewCameraLock_Click);
            // 
            // mnuShowCameraOutline
            // 
            this.mnuShowCameraOutline.Checked = true;
            this.mnuShowCameraOutline.Index = 4;
            this.mnuShowCameraOutline.Text = "Show Camera Outline";
            this.mnuShowCameraOutline.Click += new System.EventHandler(this.mnuShowCameraOutline_Click);
            // 
            // mnuConfig
            // 
            this.mnuConfig.Enabled = false;
            this.mnuConfig.Index = 4;
            this.mnuConfig.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSelectFolder,
            this.mnuChangeProjectName,
            this.mnuShowWarnings});
            this.mnuConfig.Text = "Config";
            this.mnuConfig.Visible = false;
            // 
            // mnuSelectFolder
            // 
            this.mnuSelectFolder.Index = 0;
            this.mnuSelectFolder.Text = "Select Project Folder";
            this.mnuSelectFolder.Click += new System.EventHandler(this.mnuSelectFolder_Click);
            // 
            // mnuChangeProjectName
            // 
            this.mnuChangeProjectName.Index = 1;
            this.mnuChangeProjectName.Text = "Change Project Name";
            this.mnuChangeProjectName.Click += new System.EventHandler(this.mnuChangeProjectName_Click);
            // 
            // mnuShowWarnings
            // 
            this.mnuShowWarnings.Index = 2;
            this.mnuShowWarnings.Text = "Show Warnings";
            this.mnuShowWarnings.Click += new System.EventHandler(this.mnuShowWarnings_Click);
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 5;
            this.menuItem25.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuContents,
            this.menuItem27,
            this.mnuAbout});
            this.menuItem25.Text = "Help";
            this.menuItem25.Visible = false;
            // 
            // mnuContents
            // 
            this.mnuContents.Enabled = false;
            this.mnuContents.Index = 0;
            this.mnuContents.Text = "Contents";
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 1;
            this.menuItem27.Text = "-";
            this.menuItem27.Visible = false;
            // 
            // mnuAbout
            // 
            this.mnuAbout.Enabled = false;
            this.mnuAbout.Index = 2;
            this.mnuAbout.Text = "About";
            this.mnuAbout.Visible = false;
            // 
            // ssStatus
            // 
            this.ssStatus.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslbCameraMode,
            this.tsslbProjectFolder,
            this.tsslbTileSelection});
            this.ssStatus.Location = new System.Drawing.Point(0, 498);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(852, 25);
            this.ssStatus.TabIndex = 19;
            // 
            // tsslbCameraMode
            // 
            this.tsslbCameraMode.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsslbCameraMode.Name = "tsslbCameraMode";
            this.tsslbCameraMode.Size = new System.Drawing.Size(140, 20);
            this.tsslbCameraMode.Text = "Camera Mode: Locked";
            // 
            // tsslbProjectFolder
            // 
            this.tsslbProjectFolder.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsslbProjectFolder.Name = "tsslbProjectFolder";
            this.tsslbProjectFolder.Size = new System.Drawing.Size(629, 20);
            this.tsslbProjectFolder.Spring = true;
            this.tsslbProjectFolder.Text = "Project Folder:";
            this.tsslbProjectFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslbTileSelection
            // 
            this.tsslbTileSelection.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsslbTileSelection.Name = "tsslbTileSelection";
            this.tsslbTileSelection.Size = new System.Drawing.Size(68, 20);
            this.tsslbTileSelection.Text = "(0, 0) 0x0";
            this.tsslbTileSelection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bgWorkLoadFile
            // 
            this.bgWorkLoadFile.WorkerReportsProgress = true;
            this.bgWorkLoadFile.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkLoadFile_DoWork);
            this.bgWorkLoadFile.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkLoadFile_ProgressChanged);
            this.bgWorkLoadFile.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkLoadFile_RunWorkerCompleted);
            // 
            // bgWorkPrepareFile
            // 
            this.bgWorkPrepareFile.WorkerReportsProgress = true;
            this.bgWorkPrepareFile.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkPrepareFile_DoWork);
            this.bgWorkPrepareFile.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkPrepareFile_ProgressChanged);
            this.bgWorkPrepareFile.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkPrepareFile_RunWorkerCompleted);
            // 
            // rbnToolbar
            // 
            this.rbnToolbar.CaptionBarVisible = false;
            this.rbnToolbar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbnToolbar.Location = new System.Drawing.Point(0, 0);
            this.rbnToolbar.Minimized = false;
            this.rbnToolbar.Name = "rbnToolbar";
            // 
            // 
            // 
            this.rbnToolbar.OrbDropDown.BorderRoundness = 8;
            this.rbnToolbar.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.rbnToolbar.OrbDropDown.MenuItems.Add(this.romiNew);
            this.rbnToolbar.OrbDropDown.MenuItems.Add(this.romiOpen);
            this.rbnToolbar.OrbDropDown.MenuItems.Add(this.romiSave);
            this.rbnToolbar.OrbDropDown.MenuItems.Add(this.romiSaveAs);
            this.rbnToolbar.OrbDropDown.MenuItems.Add(this.ribbonSeparator1);
            this.rbnToolbar.OrbDropDown.MenuItems.Add(this.romiRun);
            this.rbnToolbar.OrbDropDown.MenuItems.Add(this.romiRunConsole);
            this.rbnToolbar.OrbDropDown.Name = "";
            this.rbnToolbar.OrbDropDown.Size = new System.Drawing.Size(527, 339);
            this.rbnToolbar.OrbDropDown.TabIndex = 0;
            this.rbnToolbar.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2013;
            this.rbnToolbar.OrbText = "File";
            this.rbnToolbar.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.rbnToolbar.Size = new System.Drawing.Size(852, 115);
            this.rbnToolbar.TabIndex = 20;
            this.rbnToolbar.Tabs.Add(this.rtabTools);
            this.rbnToolbar.Tabs.Add(this.rbtEdit);
            this.rbnToolbar.Tabs.Add(this.rtabView);
            this.rbnToolbar.Tabs.Add(this.rtabConfig);
            this.rbnToolbar.TabSpacing = 4;
            this.rbnToolbar.Text = "rbnToolbar";
            this.rbnToolbar.ThemeColor = System.Windows.Forms.RibbonTheme.Blue_2010;
            this.rbnToolbar.ExpandedChanged += new System.EventHandler(this.rbnToolbar_ExpandedChanged);
            // 
            // romiNew
            // 
            this.romiNew.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.romiNew.Image = ((System.Drawing.Image)(resources.GetObject("romiNew.Image")));
            this.romiNew.LargeImage = ((System.Drawing.Image)(resources.GetObject("romiNew.LargeImage")));
            this.romiNew.Name = "romiNew";
            this.romiNew.SmallImage = ((System.Drawing.Image)(resources.GetObject("romiNew.SmallImage")));
            this.romiNew.Text = "New Project";
            this.romiNew.Click += new System.EventHandler(this.romiNew_Click);
            // 
            // romiOpen
            // 
            this.romiOpen.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.romiOpen.Image = ((System.Drawing.Image)(resources.GetObject("romiOpen.Image")));
            this.romiOpen.LargeImage = ((System.Drawing.Image)(resources.GetObject("romiOpen.LargeImage")));
            this.romiOpen.Name = "romiOpen";
            this.romiOpen.SmallImage = ((System.Drawing.Image)(resources.GetObject("romiOpen.SmallImage")));
            this.romiOpen.Text = "Open Project";
            this.romiOpen.Click += new System.EventHandler(this.romiOpen_Click);
            // 
            // romiSave
            // 
            this.romiSave.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.romiSave.Enabled = false;
            this.romiSave.Image = ((System.Drawing.Image)(resources.GetObject("romiSave.Image")));
            this.romiSave.LargeImage = ((System.Drawing.Image)(resources.GetObject("romiSave.LargeImage")));
            this.romiSave.Name = "romiSave";
            this.romiSave.SmallImage = ((System.Drawing.Image)(resources.GetObject("romiSave.SmallImage")));
            this.romiSave.Text = "Save Project";
            this.romiSave.Click += new System.EventHandler(this.romiSave_Click);
            // 
            // romiSaveAs
            // 
            this.romiSaveAs.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.romiSaveAs.Enabled = false;
            this.romiSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("romiSaveAs.Image")));
            this.romiSaveAs.LargeImage = ((System.Drawing.Image)(resources.GetObject("romiSaveAs.LargeImage")));
            this.romiSaveAs.Name = "romiSaveAs";
            this.romiSaveAs.SmallImage = ((System.Drawing.Image)(resources.GetObject("romiSaveAs.SmallImage")));
            this.romiSaveAs.Text = "Save Project As";
            this.romiSaveAs.Click += new System.EventHandler(this.romiSaveAs_Click);
            // 
            // ribbonSeparator1
            // 
            this.ribbonSeparator1.Name = "ribbonSeparator1";
            // 
            // romiRun
            // 
            this.romiRun.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.romiRun.Enabled = false;
            this.romiRun.Image = ((System.Drawing.Image)(resources.GetObject("romiRun.Image")));
            this.romiRun.LargeImage = ((System.Drawing.Image)(resources.GetObject("romiRun.LargeImage")));
            this.romiRun.Name = "romiRun";
            this.romiRun.SmallImage = ((System.Drawing.Image)(resources.GetObject("romiRun.SmallImage")));
            this.romiRun.Text = "Run";
            this.romiRun.Click += new System.EventHandler(this.romiRun_Click);
            // 
            // romiRunConsole
            // 
            this.romiRunConsole.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.romiRunConsole.Enabled = false;
            this.romiRunConsole.Image = ((System.Drawing.Image)(resources.GetObject("romiRunConsole.Image")));
            this.romiRunConsole.LargeImage = ((System.Drawing.Image)(resources.GetObject("romiRunConsole.LargeImage")));
            this.romiRunConsole.Name = "romiRunConsole";
            this.romiRunConsole.SmallImage = ((System.Drawing.Image)(resources.GetObject("romiRunConsole.SmallImage")));
            this.romiRunConsole.Text = "Run (Console)";
            this.romiRunConsole.Click += new System.EventHandler(this.romiRunConsole_Click);
            // 
            // rtabTools
            // 
            this.rtabTools.Name = "rtabTools";
            this.rtabTools.Panels.Add(this.rpnlMode);
            this.rtabTools.Panels.Add(this.rpnlPopouts);
            this.rtabTools.Panels.Add(this.rpnlPopups);
            this.rtabTools.Text = "Tools";
            // 
            // rpnlMode
            // 
            this.rpnlMode.ButtonMoreVisible = false;
            this.rpnlMode.Items.Add(this.rbtnDraw);
            this.rpnlMode.Items.Add(this.rbtnSelect);
            this.rpnlMode.Items.Add(this.rbtnScroll);
            this.rpnlMode.Name = "rpnlMode";
            this.rpnlMode.Text = "Mode";
            // 
            // rbtnDraw
            // 
            this.rbtnDraw.Checked = true;
            this.rbtnDraw.CheckedGroup = "Mode";
            this.rbtnDraw.CheckOnClick = true;
            this.rbtnDraw.Enabled = false;
            this.rbtnDraw.Image = ((System.Drawing.Image)(resources.GetObject("rbtnDraw.Image")));
            this.rbtnDraw.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnDraw.LargeImage")));
            this.rbtnDraw.Name = "rbtnDraw";
            this.rbtnDraw.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnDraw.SmallImage")));
            this.rbtnDraw.Text = "Draw";
            this.rbtnDraw.Click += new System.EventHandler(this.rbtnDraw_Click);
            // 
            // rbtnSelect
            // 
            this.rbtnSelect.CheckedGroup = "Mode";
            this.rbtnSelect.CheckOnClick = true;
            this.rbtnSelect.Enabled = false;
            this.rbtnSelect.Image = ((System.Drawing.Image)(resources.GetObject("rbtnSelect.Image")));
            this.rbtnSelect.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnSelect.LargeImage")));
            this.rbtnSelect.Name = "rbtnSelect";
            this.rbtnSelect.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnSelect.SmallImage")));
            this.rbtnSelect.Text = "Select";
            this.rbtnSelect.Click += new System.EventHandler(this.rbtnSelect_Click);
            // 
            // rbtnScroll
            // 
            this.rbtnScroll.CheckedGroup = "Mode";
            this.rbtnScroll.CheckOnClick = true;
            this.rbtnScroll.Enabled = false;
            this.rbtnScroll.Image = ((System.Drawing.Image)(resources.GetObject("rbtnScroll.Image")));
            this.rbtnScroll.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnScroll.LargeImage")));
            this.rbtnScroll.Name = "rbtnScroll";
            this.rbtnScroll.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnScroll.SmallImage")));
            this.rbtnScroll.Text = "Scroll";
            this.rbtnScroll.Click += new System.EventHandler(this.rbtnScroll_Click);
            // 
            // rpnlPopouts
            // 
            this.rpnlPopouts.ButtonMoreVisible = false;
            this.rpnlPopouts.Items.Add(this.rbtnRooms);
            this.rpnlPopouts.Items.Add(this.rbtnLayers);
            this.rpnlPopouts.Items.Add(this.rbtnAssetSelector);
            this.rpnlPopouts.Items.Add(this.rbtnInstances);
            this.rpnlPopouts.Items.Add(this.rbtnProperties);
            this.rpnlPopouts.Name = "rpnlPopouts";
            this.rpnlPopouts.Text = "Popout Forms";
            // 
            // rbtnRooms
            // 
            this.rbtnRooms.Enabled = false;
            this.rbtnRooms.Image = ((System.Drawing.Image)(resources.GetObject("rbtnRooms.Image")));
            this.rbtnRooms.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnRooms.LargeImage")));
            this.rbtnRooms.Name = "rbtnRooms";
            this.rbtnRooms.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnRooms.SmallImage")));
            this.rbtnRooms.Text = "Rooms";
            this.rbtnRooms.Click += new System.EventHandler(this.rbtnRooms_Click);
            // 
            // rbtnLayers
            // 
            this.rbtnLayers.Enabled = false;
            this.rbtnLayers.Image = ((System.Drawing.Image)(resources.GetObject("rbtnLayers.Image")));
            this.rbtnLayers.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnLayers.LargeImage")));
            this.rbtnLayers.Name = "rbtnLayers";
            this.rbtnLayers.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnLayers.SmallImage")));
            this.rbtnLayers.Text = "Layers";
            this.rbtnLayers.Click += new System.EventHandler(this.rbtnLayers_Click);
            // 
            // rbtnAssetSelector
            // 
            this.rbtnAssetSelector.Enabled = false;
            this.rbtnAssetSelector.Image = ((System.Drawing.Image)(resources.GetObject("rbtnAssetSelector.Image")));
            this.rbtnAssetSelector.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnAssetSelector.LargeImage")));
            this.rbtnAssetSelector.Name = "rbtnAssetSelector";
            this.rbtnAssetSelector.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnAssetSelector.SmallImage")));
            this.rbtnAssetSelector.Text = "Asset Selector";
            this.rbtnAssetSelector.Click += new System.EventHandler(this.rbtnAssetSelector_Click);
            // 
            // rbtnInstances
            // 
            this.rbtnInstances.Enabled = false;
            this.rbtnInstances.Image = ((System.Drawing.Image)(resources.GetObject("rbtnInstances.Image")));
            this.rbtnInstances.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnInstances.LargeImage")));
            this.rbtnInstances.Name = "rbtnInstances";
            this.rbtnInstances.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnInstances.SmallImage")));
            this.rbtnInstances.Text = "Instances";
            this.rbtnInstances.Click += new System.EventHandler(this.rbtnInstances_Click);
            // 
            // rbtnProperties
            // 
            this.rbtnProperties.Enabled = false;
            this.rbtnProperties.Image = ((System.Drawing.Image)(resources.GetObject("rbtnProperties.Image")));
            this.rbtnProperties.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnProperties.LargeImage")));
            this.rbtnProperties.Name = "rbtnProperties";
            this.rbtnProperties.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnProperties.SmallImage")));
            this.rbtnProperties.Text = "Properties";
            this.rbtnProperties.Click += new System.EventHandler(this.rbtnProperties_Click);
            // 
            // rpnlPopups
            // 
            this.rpnlPopups.ButtonMoreVisible = false;
            this.rpnlPopups.Items.Add(this.rbtnAssets);
            this.rpnlPopups.Items.Add(this.rbtnScripts);
            this.rpnlPopups.Items.Add(this.rbtnResources);
            this.rpnlPopups.Items.Add(this.rbtnSpriteSheetBuilder);
            this.rpnlPopups.Name = "rpnlPopups";
            this.rpnlPopups.Text = "Popup Forms";
            // 
            // rbtnAssets
            // 
            this.rbtnAssets.Enabled = false;
            this.rbtnAssets.Image = ((System.Drawing.Image)(resources.GetObject("rbtnAssets.Image")));
            this.rbtnAssets.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnAssets.LargeImage")));
            this.rbtnAssets.Name = "rbtnAssets";
            this.rbtnAssets.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnAssets.SmallImage")));
            this.rbtnAssets.Text = "Assets";
            this.rbtnAssets.Click += new System.EventHandler(this.rbtnAssets_Click);
            // 
            // rbtnScripts
            // 
            this.rbtnScripts.Enabled = false;
            this.rbtnScripts.Image = ((System.Drawing.Image)(resources.GetObject("rbtnScripts.Image")));
            this.rbtnScripts.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnScripts.LargeImage")));
            this.rbtnScripts.Name = "rbtnScripts";
            this.rbtnScripts.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnScripts.SmallImage")));
            this.rbtnScripts.Text = "Scripts";
            this.rbtnScripts.Click += new System.EventHandler(this.rbtnScripts_Click);
            // 
            // rbtnResources
            // 
            this.rbtnResources.Enabled = false;
            this.rbtnResources.Image = ((System.Drawing.Image)(resources.GetObject("rbtnResources.Image")));
            this.rbtnResources.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnResources.LargeImage")));
            this.rbtnResources.Name = "rbtnResources";
            this.rbtnResources.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnResources.SmallImage")));
            this.rbtnResources.Text = "Resources";
            this.rbtnResources.Click += new System.EventHandler(this.rbtnResources_Click);
            // 
            // rbtnSpriteSheetBuilder
            // 
            this.rbtnSpriteSheetBuilder.Image = ((System.Drawing.Image)(resources.GetObject("rbtnSpriteSheetBuilder.Image")));
            this.rbtnSpriteSheetBuilder.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnSpriteSheetBuilder.LargeImage")));
            this.rbtnSpriteSheetBuilder.MinimumSize = new System.Drawing.Size(70, 0);
            this.rbtnSpriteSheetBuilder.Name = "rbtnSpriteSheetBuilder";
            this.rbtnSpriteSheetBuilder.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnSpriteSheetBuilder.SmallImage")));
            this.rbtnSpriteSheetBuilder.Text = "Sprite Sheet Builder";
            this.rbtnSpriteSheetBuilder.Click += new System.EventHandler(this.rbtnSpriteSheetBuilder_Click);
            // 
            // rbtEdit
            // 
            this.rbtEdit.Name = "rbtEdit";
            this.rbtEdit.Panels.Add(this.rpnSelect);
            this.rbtEdit.Panels.Add(this.rpnlUndoRedo);
            this.rbtEdit.Text = "Edit";
            // 
            // rpnSelect
            // 
            this.rpnSelect.ButtonMoreVisible = false;
            this.rpnSelect.Items.Add(this.rbtnCopy);
            this.rpnSelect.Items.Add(this.rbtnPaste);
            this.rpnSelect.Items.Add(this.rbtnCut);
            this.rpnSelect.Items.Add(this.rbtnDelete);
            this.rpnSelect.Items.Add(this.rcbSelectActors);
            this.rpnSelect.Items.Add(this.rcbSelectEvents);
            this.rpnSelect.Items.Add(this.rcbSelectHudElements);
            this.rpnSelect.Items.Add(this.rcbTileObjects);
            this.rpnSelect.Items.Add(this.rcbWorldGeometry);
            this.rpnSelect.Items.Add(this.rcbSpawnPoints);
            this.rpnSelect.Items.Add(this.rcbSelectAudioSources);
            this.rpnSelect.Items.Add(this.rcbParticleEmitters);
            this.rpnSelect.Name = "rpnSelect";
            this.rpnSelect.Text = "Selection";
            // 
            // rbtnCopy
            // 
            this.rbtnCopy.Image = ((System.Drawing.Image)(resources.GetObject("rbtnCopy.Image")));
            this.rbtnCopy.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnCopy.LargeImage")));
            this.rbtnCopy.Name = "rbtnCopy";
            this.rbtnCopy.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnCopy.SmallImage")));
            this.rbtnCopy.Text = "Copy";
            // 
            // rbtnPaste
            // 
            this.rbtnPaste.Image = ((System.Drawing.Image)(resources.GetObject("rbtnPaste.Image")));
            this.rbtnPaste.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnPaste.LargeImage")));
            this.rbtnPaste.Name = "rbtnPaste";
            this.rbtnPaste.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnPaste.SmallImage")));
            this.rbtnPaste.Text = "Paste";
            // 
            // rbtnCut
            // 
            this.rbtnCut.Image = ((System.Drawing.Image)(resources.GetObject("rbtnCut.Image")));
            this.rbtnCut.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnCut.LargeImage")));
            this.rbtnCut.Name = "rbtnCut";
            this.rbtnCut.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnCut.SmallImage")));
            this.rbtnCut.Text = "Cut";
            // 
            // rbtnDelete
            // 
            this.rbtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("rbtnDelete.Image")));
            this.rbtnDelete.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnDelete.LargeImage")));
            this.rbtnDelete.Name = "rbtnDelete";
            this.rbtnDelete.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnDelete.SmallImage")));
            this.rbtnDelete.Text = "Delete";
            // 
            // rcbSelectActors
            // 
            this.rcbSelectActors.Checked = true;
            this.rcbSelectActors.Name = "rcbSelectActors";
            this.rcbSelectActors.Text = "Actors";
            this.rcbSelectActors.CheckBoxCheckChanged += new System.EventHandler(this.rcbSelectActors_CheckBoxCheckChanged);
            // 
            // rcbSelectEvents
            // 
            this.rcbSelectEvents.Checked = true;
            this.rcbSelectEvents.Name = "rcbSelectEvents";
            this.rcbSelectEvents.Text = "Events";
            this.rcbSelectEvents.CheckBoxCheckChanged += new System.EventHandler(this.rcbSelectEvents_CheckBoxCheckChanged);
            // 
            // rcbSelectHudElements
            // 
            this.rcbSelectHudElements.Checked = true;
            this.rcbSelectHudElements.Name = "rcbSelectHudElements";
            this.rcbSelectHudElements.Text = "HUD Elements";
            this.rcbSelectHudElements.CheckBoxCheckChanged += new System.EventHandler(this.rcbSelectHudElements_CheckBoxCheckChanged);
            // 
            // rcbTileObjects
            // 
            this.rcbTileObjects.Checked = true;
            this.rcbTileObjects.Name = "rcbTileObjects";
            this.rcbTileObjects.Text = "Tile Objects";
            this.rcbTileObjects.CheckBoxCheckChanged += new System.EventHandler(this.rcbTileObjects_CheckBoxCheckChanged);
            // 
            // rcbWorldGeometry
            // 
            this.rcbWorldGeometry.Checked = true;
            this.rcbWorldGeometry.Name = "rcbWorldGeometry";
            this.rcbWorldGeometry.Text = "World Geometry";
            this.rcbWorldGeometry.CheckBoxCheckChanged += new System.EventHandler(this.rcbWorldGeometry_CheckBoxCheckChanged);
            // 
            // rcbSpawnPoints
            // 
            this.rcbSpawnPoints.Checked = true;
            this.rcbSpawnPoints.Name = "rcbSpawnPoints";
            this.rcbSpawnPoints.Text = "Spawn Points";
            this.rcbSpawnPoints.CheckBoxCheckChanged += new System.EventHandler(this.rcbSpawnPoints_CheckBoxCheckChanged);
            // 
            // rcbSelectAudioSources
            // 
            this.rcbSelectAudioSources.Checked = true;
            this.rcbSelectAudioSources.Name = "rcbSelectAudioSources";
            this.rcbSelectAudioSources.Text = "Audio Sources";
            this.rcbSelectAudioSources.CheckBoxCheckChanged += new System.EventHandler(this.rcbSelectAudioSources_CheckBoxCheckChanged);
            // 
            // rcbParticleEmitters
            // 
            this.rcbParticleEmitters.Checked = true;
            this.rcbParticleEmitters.Name = "rcbParticleEmitters";
            this.rcbParticleEmitters.Text = "Particle Emitters";
            this.rcbParticleEmitters.CheckBoxCheckChanged += new System.EventHandler(this.rcbParticleEmitters_CheckBoxCheckChanged);
            // 
            // rpnlUndoRedo
            // 
            this.rpnlUndoRedo.ButtonMoreVisible = false;
            this.rpnlUndoRedo.Items.Add(this.rbtnUndo);
            this.rpnlUndoRedo.Items.Add(this.rbtnRedo);
            this.rpnlUndoRedo.Name = "rpnlUndoRedo";
            this.rpnlUndoRedo.Text = "";
            // 
            // rbtnUndo
            // 
            this.rbtnUndo.Enabled = false;
            this.rbtnUndo.Image = ((System.Drawing.Image)(resources.GetObject("rbtnUndo.Image")));
            this.rbtnUndo.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnUndo.LargeImage")));
            this.rbtnUndo.Name = "rbtnUndo";
            this.rbtnUndo.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnUndo.SmallImage")));
            this.rbtnUndo.Text = "Undo";
            this.rbtnUndo.Click += new System.EventHandler(this.rtbnUndo_Click);
            // 
            // rbtnRedo
            // 
            this.rbtnRedo.Enabled = false;
            this.rbtnRedo.Image = ((System.Drawing.Image)(resources.GetObject("rbtnRedo.Image")));
            this.rbtnRedo.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnRedo.LargeImage")));
            this.rbtnRedo.Name = "rbtnRedo";
            this.rbtnRedo.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnRedo.SmallImage")));
            this.rbtnRedo.Text = "Redo";
            this.rbtnRedo.Click += new System.EventHandler(this.rbtnRedo_Click);
            // 
            // rtabView
            // 
            this.rtabView.Name = "rtabView";
            this.rtabView.Panels.Add(this.rpnlShowHide);
            this.rtabView.Text = "View";
            // 
            // rpnlShowHide
            // 
            this.rpnlShowHide.ButtonMoreVisible = false;
            this.rpnlShowHide.Items.Add(this.rbtnShowGrid);
            this.rpnlShowHide.Items.Add(this.rcbShowOutlines);
            this.rpnlShowHide.Items.Add(this.rcbCameraOutline);
            this.rpnlShowHide.Items.Add(this.rcbShowWorldGeometry);
            this.rpnlShowHide.Items.Add(this.rcbFillSelectedItems);
            this.rpnlShowHide.Items.Add(this.rcbLockCamera);
            this.rpnlShowHide.Name = "rpnlShowHide";
            this.rpnlShowHide.Text = "";
            // 
            // rbtnShowGrid
            // 
            this.rbtnShowGrid.Checked = true;
            this.rbtnShowGrid.Name = "rbtnShowGrid";
            this.rbtnShowGrid.Text = "Show Grid";
            this.rbtnShowGrid.CheckBoxCheckChanged += new System.EventHandler(this.rbtnShowGrid_CheckBoxCheckChanged);
            // 
            // rcbShowOutlines
            // 
            this.rcbShowOutlines.Name = "rcbShowOutlines";
            this.rcbShowOutlines.Text = "Show Outlines";
            this.rcbShowOutlines.CheckBoxCheckChanged += new System.EventHandler(this.rcbShowOutlines_CheckBoxCheckChanged);
            // 
            // rcbCameraOutline
            // 
            this.rcbCameraOutline.Checked = true;
            this.rcbCameraOutline.LabelWidth = 120;
            this.rcbCameraOutline.Name = "rcbCameraOutline";
            this.rcbCameraOutline.Text = "Show Camera Outline";
            this.rcbCameraOutline.CheckBoxCheckChanged += new System.EventHandler(this.rcbCameraOutline_CheckBoxCheckChanged);
            // 
            // rcbShowWorldGeometry
            // 
            this.rcbShowWorldGeometry.Checked = true;
            this.rcbShowWorldGeometry.Name = "rcbShowWorldGeometry";
            this.rcbShowWorldGeometry.Text = "Show World Geometry";
            this.rcbShowWorldGeometry.CheckBoxCheckChanged += new System.EventHandler(this.rcbShowWorldGeometry_CheckBoxCheckChanged);
            // 
            // rcbFillSelectedItems
            // 
            this.rcbFillSelectedItems.Checked = true;
            this.rcbFillSelectedItems.Enabled = false;
            this.rcbFillSelectedItems.Name = "rcbFillSelectedItems";
            this.rcbFillSelectedItems.Text = "Fill Selected Items";
            // 
            // rcbLockCamera
            // 
            this.rcbLockCamera.Checked = true;
            this.rcbLockCamera.Name = "rcbLockCamera";
            this.rcbLockCamera.Text = "In Game View";
            this.rcbLockCamera.CheckBoxCheckChanged += new System.EventHandler(this.rcbLockCamera_CheckBoxCheckChanged);
            // 
            // rtabConfig
            // 
            this.rtabConfig.Name = "rtabConfig";
            this.rtabConfig.Panels.Add(this.rpnlProjectSettings);
            this.rtabConfig.Text = "Config";
            // 
            // rpnlProjectSettings
            // 
            this.rpnlProjectSettings.ButtonMoreVisible = false;
            this.rpnlProjectSettings.Items.Add(this.rbtnSelectProjectFolder);
            this.rpnlProjectSettings.Items.Add(this.rbtnChangeName);
            this.rpnlProjectSettings.Items.Add(this.rcbShowWarnings);
            this.rpnlProjectSettings.Name = "rpnlProjectSettings";
            this.rpnlProjectSettings.Text = "";
            // 
            // rbtnSelectProjectFolder
            // 
            this.rbtnSelectProjectFolder.Image = ((System.Drawing.Image)(resources.GetObject("rbtnSelectProjectFolder.Image")));
            this.rbtnSelectProjectFolder.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnSelectProjectFolder.LargeImage")));
            this.rbtnSelectProjectFolder.MinimumSize = new System.Drawing.Size(40, 0);
            this.rbtnSelectProjectFolder.Name = "rbtnSelectProjectFolder";
            this.rbtnSelectProjectFolder.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnSelectProjectFolder.SmallImage")));
            this.rbtnSelectProjectFolder.Text = "Select Folder";
            this.rbtnSelectProjectFolder.Click += new System.EventHandler(this.rbtnSelectProjectFolder_Click);
            // 
            // rbtnChangeName
            // 
            this.rbtnChangeName.Image = ((System.Drawing.Image)(resources.GetObject("rbtnChangeName.Image")));
            this.rbtnChangeName.LargeImage = ((System.Drawing.Image)(resources.GetObject("rbtnChangeName.LargeImage")));
            this.rbtnChangeName.Name = "rbtnChangeName";
            this.rbtnChangeName.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnChangeName.SmallImage")));
            this.rbtnChangeName.Text = "Change Name";
            // 
            // tmrPostLoad
            // 
            this.tmrPostLoad.Interval = 3000;
            this.tmrPostLoad.Tick += new System.EventHandler(this.tmrPostLoad_Tick);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.pnlMain.Location = new System.Drawing.Point(0, 115);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(852, 383);
            this.pnlMain.TabIndex = 22;
            this.pnlMain.ActiveDocumentChanged += new System.EventHandler(this.pnlMain_ActiveDocumentChanged);
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.Enabled = false;
            this.ribbonButton1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.Image")));
            this.ribbonButton1.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.LargeImage")));
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
            this.ribbonButton1.Text = "Scripts";
            // 
            // rcbShowWarnings
            // 
            this.rcbShowWarnings.Name = "rcbShowWarnings";
            this.rcbShowWarnings.Text = "Show Warnings";
            this.rcbShowWarnings.CheckBoxCheckChanged += new System.EventHandler(this.rcbShowWarnings_CheckBoxCheckChanged);
            // 
            // EditorForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(852, 523);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.rbnToolbar);
            this.Controls.Add(this.ssStatus);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "EditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Firemelon Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditorForm_FormClosing);
            this.Load += new System.EventHandler(this.EditorForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditorForm_KeyDown);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.EditorForm_MouseWheel);
            this.Resize += new System.EventHandler(this.EditorForm_Resize);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new EditorForm());
        }

        #region Public Functions

        public bool ProcessKeyFromChildForm(ref Message msg)
        {
            Message messageCopy = msg;
            messageCopy.HWnd = this.Handle; // We need to assign our own Handle, otherwise the message is rejected!

            bool result = ProcessKeyMessage(ref messageCopy);

            return result;
        }

        #endregion

        #region Private Functions

        private void showPopoutForms(bool locateForms = false)
        {
            //if (mnuLayersPopout.Checked == false && layerListForm_.IsClosed == false)
            //{
            //    layerListForm_.Show(this);
            //    mnuLayersPopout.Checked = true;
            //}

            //if (mnuAssetsPopout.Checked == false && assetSelectionForm_.IsClosed == false)
            //{
            //    assetSelectionForm_.Show(this);
            //    mnuAssetsPopout.Checked = true;
            //}

            //if (mnuRoomsPopout.Checked == false && roomListForm_.IsClosed == false)
            //{
            //    roomListForm_.Show(this);
            //    mnuRoomsPopout.Checked = true;
            //}

            //if (mnuPropertyGridPopout.Checked == false && propertyGridForm_.IsClosed == false)
            //{
            //    propertyGridForm_.Show(this);
            //    mnuPropertyGridPopout.Checked = true;
            //}

            //if (instancePropertiesForm_.IsClosed == false)
            //{
            //    instancePropertiesForm_.Show(this);
            //}
            
            //// If this is the first time the popout windows are being shown, position them in a nice orderly manner.
            //if (locateForms == true)
            //{
            //    locatePopoutForms();
            //}
        }
        
        private void locatePopoutForms()
        {
            //int popoutWidthLeft = 200;

            //int popoutWidthRight = 230;
            
            //assetSelectionForm_.Top = this.Top;
            //assetSelectionForm_.Width = popoutWidthRight;
            //assetSelectionForm_.Left = this.Left - assetSelectionForm_.Width;
            //assetSelectionForm_.Height = this.Height / 3;
            
            //instancePropertiesForm_.Top = assetSelectionForm_.Bottom;
            //instancePropertiesForm_.Width = assetSelectionForm_.Width;
            //instancePropertiesForm_.Left = assetSelectionForm_.Left;
            //instancePropertiesForm_.Height = this.Height / 3;
            
            //propertyGridForm_.Top = instancePropertiesForm_.Bottom;
            //propertyGridForm_.Width = instancePropertiesForm_.Width;
            //propertyGridForm_.Left = instancePropertiesForm_.Left;
            //propertyGridForm_.Height = this.Height - (assetSelectionForm_.Height + instancePropertiesForm_.Height);
            
            //roomListForm_.Top = this.Top;
            //roomListForm_.Width = popoutWidthLeft;
            //roomListForm_.Left = this.Right;
            //roomListForm_.Height = this.Height / 2;

            //layerListForm_.Top = roomListForm_.Bottom;
            //layerListForm_.Width = roomListForm_.Width;
            //layerListForm_.Left = roomListForm_.Left;
            //layerListForm_.Height = this.Height  - roomListForm_.Height;            
        }
        
        private void newProject()
        {
            bool continueSave = false;
            bool continueAction = true;

            if (projectController_.ChangesMade == true)
            {
                DialogResult res = MessageBox.Show("Changes have been made to the current project. Do you wish to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);

                if (res == DialogResult.Yes)
                {
                    continueSave = true;
                    continueAction = true;
                }
                else if (res == DialogResult.No)
                {
                    continueSave = false;
                    continueAction = true;
                }
                else if (res == DialogResult.Cancel)
                {
                    continueSave = false;
                    continueAction = false;
                }
            }

            if (continueSave == true)
            {
                saveProject(false);
            }

            if (continueAction == true)
            {
                newProjectDialog_.ShowDialog(this);
            }

            if (mnuViewCameraLock.Checked == true)
            {
                resizeToGame();
            }

            ProjectDto project = projectController_.GetProjectDto();

            if (project != null && project.IsPrepared == true)
            {
                projectFileName_ = string.Empty;

                this.Text = "Firemelon Editor : " + project.ProjectName;

                tsslbProjectFolder.Text = "Project Folder: " + project.ProjectFolderFullPath;

                setMenuEnabled();

                mnuLayersPopout.Enabled = true;
                
                rcbTileObjects.Checked = false;

                projectController_.SetCanSelectMapWidget(MapWidgetType.TileObject, false);

                showPopoutForms(true);
            }

            this.CenterToScreen();

            locatePopoutForms();
        }

        private void saveProject(bool showDialog)
        {
            try
            {
                ProjectDto project = projectController_.GetProjectDto();

                string defaultName = String.Empty;

                // Default to the project name.
                if (String.IsNullOrEmpty(projectFileName_) == true)
                {
                    defaultName = project.ProjectName + ".fmproj";

                    showDialog = true;
                }
                else
                {
                    defaultName = projectFileName_;
                }

                if (showDialog == true)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();

                    saveDialog.DefaultExt = "fmproj";
                    saveDialog.AddExtension = true;
                    saveDialog.RestoreDirectory = true;
                    saveDialog.Filter = "Firemelon Project Files (*.fmproj)|*.fmproj";
                    saveDialog.FileName = Path.GetFileName(defaultName);
                    saveDialog.InitialDirectory = project.ProjectFolderFullPath;

                    // Bring up the save dialog if it has not been saved yet, or if save as was clicked.
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        projectFileName_ = saveDialog.FileName;

                        // Set the current working directory to the location of the
                        // project file, so absolute paths can be converted to relative 
                        // during the save operation.
                        string projectFileFullPath = Path.GetDirectoryName(projectFileName_);
                        Directory.SetCurrentDirectory(projectFileFullPath);

                        addRecentProject(project.ProjectName, projectFileName_);
                    }
                    else
                    {
                        return;
                    }
                }

                MemoryStream stream = new MemoryStream();

                try
                {
                    projectController_.WriteProjectDtoToStream(stream, true);

                    using (var fileStream = File.Open(projectFileName_, FileMode.Create))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }

                    projectController_.ChangesMade = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error Saving File", MessageBoxButtons.OK);
                }
                finally
                {
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void loadProject()
        {
            if (projectController_.ChangesMade == true)
            {
                DialogResult res = MessageBox.Show("Changes have been made to the current project. Do you want to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);

                if (res == DialogResult.Yes)
                {
                    saveProject(false);
                }
                else if (res == DialogResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.CheckFileExists = true;
            openDialog.CheckPathExists = true;
            openDialog.DefaultExt = "fmproj";
            openDialog.Filter = "Firemelon Project Files (*.fmproj)|*.fmproj";
            openDialog.Multiselect = false;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                loadProjectFile(openDialog.FileName);              
            }
        }

        private void loadProjectFile(string projectFileFullPath)
        {
            projectFileName_ = projectFileFullPath;

            progressForm_.Show(this);

            progressForm_.SetStatus("Loading Project");

            progressForm_.CenterToScreen();

            mnuNewProject.Enabled = false;
            mnuOpenProject.Enabled = false;

            romiNew.Enabled = false;
            romiOpen.Enabled = false;

            bgWorkLoadFile.RunWorkerAsync();
        }

        private ProjectDto loadProjectDtoFromFile(string filename)
        {
            ProjectDto project = null;

            FileStream fileStream = null;
            MemoryStream streamFromFile = new MemoryStream();

            MemoryStream upgradedStream = new MemoryStream();

            MemoryStream finalStream = new MemoryStream();

            try
            {
                // Set the current working directory to the location of the
                // project file, so relative paths can be converted to absolute 
                // during the load operation.
                string projectFileFullPath = Path.GetDirectoryName(projectFileName_);

                Directory.SetCurrentDirectory(projectFileFullPath);

                fileStream = new FileStream(projectFileName_, FileMode.Open);

                fileStream.CopyTo(streamFromFile);

                finalStream = streamFromFile;

                // Every project file begins with the version number.
                Version projectVersionNumber = projectController_.ReadProjectVersionNumberFromStream(streamFromFile);

                while (projectVersionNumber < ProjectDto.LatestProjectVersion)
                {
                    IProjectUpgrader projectUpgrader = firemelonEditorFactory_.NewProjectUpgrader(projectVersionNumber, projectController_);

                    // Upgrade the project stream into the next version up.
                    projectUpgrader.Upgrade(streamFromFile, upgradedStream);

                    projectVersionNumber = projectController_.ReadProjectVersionNumberFromStream(upgradedStream);

                    // Replace the old stream object with the upgraded stream.
                    finalStream = upgradedStream;
                }

                project = projectController_.ReadProjectDtoFromStream(finalStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed to load project file " + filename);
            }
            finally
            {
                finalStream.Close();
                fileStream.Close();
            }

            return project;
        }
        
        private void deleteSelectedMapWidgets(int roomIndex)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid roomId = project.Rooms[roomIndex].Id;

            MapWidgetSelectorDto selector = uiState.MapWidgetSelector[roomId];

            List<Guid> selectedMapWidgetIds = selector.SelectedMapWidgetIds.ToList<Guid>();

            projectController_.DeleteMapWidgets(selectedMapWidgetIds);

            projectController_.ClearMapWidgetSelection(roomIndex);
        }

        private void copySelectedMapWidgets(int roomIndex)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();
            ProjectDto project = projectController_.GetProjectDto();

            Guid roomId = project.Rooms[roomIndex].Id;

            MapWidgetSelectorDto selector = uiState.MapWidgetSelector[roomId];

            int leftMost = 0;
            int topMost = 0;

            bool isInitialized = false;

            foreach (Guid mapWidgetId in selector.SelectedMapWidgetIds)
            {
                MapWidgetDto mapWidget = projectController_.GetMapWidget(mapWidgetId);

                if (isInitialized == false)
                {
                    leftMost = mapWidget.BoundingBox.Left;
                    topMost = mapWidget.BoundingBox.Top;

                    isInitialized = true;
                }
                else
                {
                    if (mapWidget.BoundingBox.Left < leftMost)
                    {
                        leftMost = mapWidget.BoundingBox.Left;
                    }

                    if (mapWidget.BoundingBox.Top < topMost)
                    {
                        topMost = mapWidget.BoundingBox.Top;
                    }
                }
            }

            string clipboardData = string.Empty;

            int selectedMapWidgetCount = selector.SelectedMapWidgetIds.Count;

            if (selectedMapWidgetCount > 0)
            {
                // Build the stream of copied objects and put it on the clipboard.
                clipboardData = "MAPWIDGET," + 
                                selectedMapWidgetCount.ToString() + "," +
                                leftMost.ToString() + "," +
                                topMost.ToString() + ",";

                foreach (Guid mapWidgetId in selector.SelectedMapWidgetIds)
                {
                    MapWidgetDto mapWidget = projectController_.GetMapWidget(mapWidgetId);

                    MapWidgetType type = mapWidget.Type;

                    int type_int = Convert.ToInt32(type);

                    string data = mapWidget.Controller.SerializeToString();

                    clipboardData += type_int.ToString() + "," +
                                     data.Length.ToString() + "," +
                                     data + ",";
                }

                clipboardData = clipboardData.Substring(0, clipboardData.Length - 1);

                Clipboard.SetDataObject(clipboardData);
            }
        }
        
        private void pasteMapWidgets()
        {
            try
            {
                ProjectUiStateDto uiState = projectController_.GetUiState();

                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                projectController_.ClearMapWidgetSelection(selectedRoomIndex);

                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

                int cameraX = uiState.CameraLocation[selectedRoomId].X;
                int cameraY = uiState.CameraLocation[selectedRoomId].Y;

                string clipbardData;
                char delim = ',';               
                bool proceed = true;

                int cursorPos = 0;

                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                {
                    clipbardData = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();

                    try
                    {
                        string pasteType = clipbardData.Substring(cursorPos, clipbardData.IndexOf(delim));

                        cursorPos = clipbardData.IndexOf(delim, cursorPos);

                        if (pasteType != "MAPWIDGET")
                        {
                            proceed = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        proceed = false;
                    }

                    if (proceed == true)
                    {
                        // Get the substring from the current cursor position to the next delimiter character.
                        
                        int mapWidgetCount = Convert.ToInt32(getNextPasteToken(clipbardData, delim, ref cursorPos));

                        int offsetX = Convert.ToInt32(getNextPasteToken(clipbardData, delim, ref cursorPos));
                        int offsetY = Convert.ToInt32(getNextPasteToken(clipbardData, delim, ref cursorPos));

                        List<MapWidgetCreationParametersDto> lstCreationParams = new List<MapWidgetCreationParametersDto>();

                        for (int i = 0; i < mapWidgetCount; i++)
                        {
                            MapWidgetType mapWidgetType = (MapWidgetType)(Convert.ToInt32(getNextPasteToken(clipbardData, delim, ref cursorPos)));

                            MapWidgetCreationParametersDto creationParameters = new MapWidgetCreationParametersDto(mapWidgetType);

                            int serializedDataLength = Convert.ToInt32(getNextPasteToken(clipbardData, delim, ref cursorPos));
                            
                            string serializedData = getNextPasteToken(clipbardData, serializedDataLength, ref cursorPos);

                            creationParameters.PositionOffset.X = cameraX - offsetX;
                            creationParameters.PositionOffset.Y = cameraY - offsetY;
                            creationParameters.SerializedDataString = serializedData;

                            creationParameters.RoomId = selectedRoomId;
                            creationParameters.LayerId = selectedLayer.Id;
                            creationParameters.Type = mapWidgetType;

                            lstCreationParams.Add(creationParameters);
                        }

                        List<MapWidgetDto> lstAddedMapWidgets = projectController_.AddMapWidgets(lstCreationParams);

                        List<Guid> lstAddedMapWidgetIds = new List<Guid>();

                        foreach (MapWidgetDto mapWidget in lstAddedMapWidgets)
                        {
                            lstAddedMapWidgetIds.Add(mapWidget.Id);
                        }

                        projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstAddedMapWidgetIds);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private string getNextPasteToken(string data, char delimiter, ref int cursorPos)
        {
            int tokenLength = data.IndexOf(delimiter, cursorPos + 1) - (cursorPos + 1);

            string token = data.Substring(cursorPos + 1, tokenLength);

            cursorPos += (tokenLength + 1);

            return token;
        }

        private string getNextPasteToken(string data, int tokenLength, ref int cursorPos)
        {
            string token = data.Substring(cursorPos + 1, tokenLength);

            cursorPos += (tokenLength + 1);

            return token;
        }
                
        private void toggleGrid()
        {
            mnuShowgrid.Checked = !mnuShowgrid.Checked;

            projectController_.ToggleGrid();
        }

        private void toggleCameraOutline()
        {
            mnuShowCameraOutline.Checked = !mnuShowCameraOutline.Checked;

            projectController_.ToggleCameraOutline();
        }      

        private void resizeToGame()
        {
            // This isn't really necessary anymore since I changed the canvas to be centered inside the room editor control.
            //if (this.WindowState == FormWindowState.Maximized)
            //{
            //    this.WindowState = FormWindowState.Normal;
            //}

            //ProjectDto project = projectController_.GetProjectDto();

            //int width = 839;
            //int height = 639;

            //if (project != null)
            //{
            //    if (project.CameraHeight == 0)
            //    {
            //        this.ClientSize = new Size(width, height);
            //    }
            //    else
            //    {
            //        width = project.CameraWidth + 16;
            //        height = project.CameraHeight + 136;

            //        this.ClientSize = new Size(width, height);
            //    }
            //}
            //else
            //{
            //    this.ClientSize = new Size(width, height);
            //}
        }
                
        private void loadRecentProjectsList()
        {
            string recentlyOpenedFilesListRaw = Properties.Settings.Default["RecentlyOpenedFiles"].ToString();

            char delimiter = (char)(255);
            char subdelimiter = (char)(254);

            foreach (string filedatalist in recentlyOpenedFilesListRaw.Split(delimiter))
            {
                string[] fileData = filedatalist.Split(subdelimiter);

                string projectName = fileData[0];

                string projectFilePath = fileData[1];

                addRecentProject(projectName, projectFilePath);
            }
        }

        private void addRecentProject(string projectName, string projectFilePath)
        {
            if (recentlyOpenedProjects_.ContainsKey(projectFilePath) == false)
            {
                if (File.Exists(projectFilePath) == true)
                {
                    recentlyOpenedProjects_.Add(projectFilePath, projectName);

                    MenuItem mnuRecentProject = new MenuItem();

                    mnuRecentProject.Text = projectName;

                    mnuRecentProject.Tag = projectFilePath;

                    mnuRecentProject.Click += new System.EventHandler(this.mnuOpenRecentFile_Click);

                    mnuRecentProjects.MenuItems.Add(mnuRecentProject);

                    RibbonOrbRecentItem roriRecebtProject = new RibbonOrbRecentItem();

                    roriRecebtProject.Text = projectFilePath; //projectName;

                    roriRecebtProject.Tag = projectFilePath;

                    roriRecebtProject.Click += new System.EventHandler(this.mnuOpenRecentFile_Click);

                    rbnToolbar.OrbDropDown.RecentItems.Add(roriRecebtProject);                    
                }
            }
        }
        
        private void saveRecentProjectsList()
        {
            string recentlyOpenedFilesList = string.Empty;

            char delimiter = (char)(255);
            char subdelimiter = (char)(254);

            foreach (KeyValuePair<string, string> kvp in recentlyOpenedProjects_)
            {
                if (recentlyOpenedFilesList != string.Empty)
                {
                    recentlyOpenedFilesList += delimiter;
                }

                recentlyOpenedFilesList += (kvp.Value + subdelimiter + kvp.Key);
            }

            Properties.Settings.Default["RecentlyOpenedFiles"] = recentlyOpenedFilesList;

            Properties.Settings.Default.Save();
        }
        
        private void setMenuEnabled()
        {
            mnuNewProject.Enabled = true;
            mnuOpenProject.Enabled = true;
            mnuEdit.Enabled = true;
            mnuConfig.Enabled = true;
            mnuTools.Enabled = true;
            mnuView.Enabled = true;

            mnuSaveProject.Enabled = true;
            mnuSaveProjectAs.Enabled = true;
            mnuExport.Enabled = true;
            mnuExportAndRun.Enabled = true;
            mnuRunWithConsole.Enabled = true;
            mnuLayersPopout.Enabled = true;

            romiNew.Enabled = true;
            romiOpen.Enabled = true;
            romiRun.Enabled = true;
            romiRunConsole.Enabled = true;
            romiSave.Enabled = true;
            romiSaveAs.Enabled = true;
            rbtnAssets.Enabled = true;
            rbtnRooms.Enabled = true;
            rbtnLayers.Enabled = true;
            rbtnInstances.Enabled = true;
            rbtnProperties.Enabled = true;
            rbtnAssets.Enabled = true;
            rbtnScripts.Enabled = true;
            rbtnResources.Enabled = true;
            rbtnDraw.Enabled = true;
            rbtnSelect.Enabled = true;
            rbtnScroll.Enabled = true;
        }
        
        private void selectProjectFolder()
        {
            ProjectDto project = projectController_.GetProjectDto();

            string oldProjectFolder = project.ProjectFolderFullPath;
            string newProjectFolder = string.Empty;

            folderBrowserDialog1.SelectedPath = oldProjectFolder;

            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                newProjectFolder = folderBrowserDialog1.SelectedPath;

                if (newProjectFolder.Substring(newProjectFolder.Length - 1) != "\\")
                {
                    newProjectFolder += "\\";
                }

                projectController_.SetProjectFolder(newProjectFolder);

                tsslbProjectFolder.Text = "Project Folder: " + newProjectFolder;
            }
        }

        #endregion

        #region Event Handlers

        private void EditorForm_Load(object sender, System.EventArgs e)
        {
            loadRecentProjectsList();

            resizeToGame();

            // Center the form. For some reason the StartPosition property isn't working correctly.
            this.Top = (Screen.PrimaryScreen.Bounds.Height / 2) - (this.Height / 2);
            this.Left = (Screen.PrimaryScreen.Bounds.Width / 2) - (this.Width / 2);

            // Force the resize event.
            this.Height = this.Height + 1;
            this.Height = this.Height - 1;

            this.Refresh();
        }

        private void mnuOpenRecentFile_Click(object sender, System.EventArgs e)
        {
            string projectFileNameToOpen = ((RibbonOrbRecentItem)sender).Tag.ToString();

            if (projectFileName_ != projectFileNameToOpen)
            {
                loadProjectFile(projectFileNameToOpen);
            }
        }

        private void EditorForm_ProjectStateChanged(object sender, ProjectStateChangedEventArgs e)
        {
            if (projectController_.GetUndoStackSize() > 0)
            {
                rbtnUndo.Enabled = true;
            }
            else
            {
                rbtnUndo.Enabled = false;
            }

            if (projectController_.GetRedoStackSize() > 0)
            {
                rbtnRedo.Enabled = true;
            }
            else
            {
                rbtnRedo.Enabled = false;
            }
        }

        private void EditorForm_SelectionToggled(object sender, SelectionToggleEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            Guid selectedRoomId = uiState.SelectedRoomId;
            
            tsslbTileSelection.Text = string.Empty;

            if (uiState.EditMode == EditMode.Selection)
            {
                if (e.SelectionOn == true)
                {
                    // Enable menu items.
                    mnuCopy.Enabled = true;
                    mnuPaste.Enabled = true;
                    mnuCut.Enabled = true;
                    mnuDelete.Enabled = true;                    
                }
                else
                {
                    // Enable menu items.
                    mnuCopy.Enabled = false;
                    mnuCut.Enabled = false;
                    mnuDelete.Enabled = false;
                    mnuFill.Enabled = false;

                    if (e.TileSelection == true)
                    {
                        mnuPaste.Enabled = false;
                    }
                    else
                    {
                        // Selector should allow pasting even if the selection rect is not shown.
                        mnuPaste.Enabled = true;
                    }
                }
            }
        }

        private void EditorForm_RoomSelected(object sender, RoomSelectedEventArgs e)
        {
            // The selected room has changed. Reset the menu options based on the selected room's UI state.
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            Guid selectedRoomId = uiState.SelectedRoomId;
            
            tsslbTileSelection.Text = string.Empty;

            // If the room is in selection mode...
            if (uiState.EditMode == EditMode.Selection)
            {
                mnuCopy.Enabled = true;
                mnuPaste.Enabled = true;
                mnuCut.Enabled = true;
                mnuDelete.Enabled = true;
                mnuPaste.Enabled = true;
                mnuFill.Enabled = false;
            }
            else
            {
                // Disable menu items.
                mnuCopy.Enabled = false;
                mnuCut.Enabled = false;
                mnuDelete.Enabled = false;
                mnuFill.Enabled = false;
                mnuPaste.Enabled = false;
            }
        }
        
        //private void EditorForm_MapWidgetSelectionChanged(object sender, MapWidgetSelectionChangedEventArgs e)
        //{
        //    setSelectedMapWidgetListItems(e.RoomIndex);
        //}
                            
        private void EditorForm_CameraModeChanged(object sender, CameraModeChangedEventArgs e)
        {
        }

        private void EditorForm_EditModeChanged(object sender, EditModeChangedEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;
            
            MapWidgetMode mapWidgetMode = uiState.MapWidgetMode[selectedRoomId];

            switch (e.EditMode)
            {
                case EditMode.Draw:
                    mnuToolsDraw.Checked = true;
                    mnuToolsSelect.Checked = false;
                    mnuToolsGrab.Checked = false;

                    mnuCopy.Enabled = false;
                    mnuCut.Enabled = false;
                    mnuPaste.Enabled = false;
                    mnuDelete.Enabled = false;
                    mnuFill.Enabled = false;

                    this.Cursor = Cursors.Arrow;

                    break;

                case EditMode.Selection:
                    mnuToolsDraw.Checked = false;
                    mnuToolsSelect.Checked = true;
                    mnuToolsGrab.Checked = false;

                    this.Cursor = Cursors.Cross;

                    bool selectionOn = uiState.MapWidgetSelector[selectedRoomId].SelectedMapWidgetIds.Count > 0;
                                        
                    mnuCopy.Enabled = selectionOn;
                    mnuCut.Enabled = selectionOn;
                    mnuPaste.Enabled = true;
                    mnuDelete.Enabled = selectionOn;
                    mnuFill.Enabled = false;                

                    break;

                case EditMode.Grab:

                    mnuToolsDraw.Checked = false;
                    mnuToolsSelect.Checked = false;
                    mnuToolsGrab.Checked = true;

                    mnuCopy.Enabled = false;
                    mnuCut.Enabled = false;
                    mnuPaste.Enabled = false;
                    mnuDelete.Enabled = false;
                    mnuFill.Enabled = false;

                    this.Cursor = Cursors.SizeAll;

                    break;

                default:

                    break;
            }
        }

        private void EditorForm_ProjectNameChanged(object sender, ProjectNameChangedEventArgs e)
        {
            if (projectController_.ChangeProjectName(e.ProjectName) == true)
            {
                ProjectDto project = projectController_.GetProjectDto();

                this.Text = "Firemelon Editor : " + project.ProjectName;
            }
        }

        private void EditorForm_CursorChanged(object sender, CursorChangedEventArgs e)
        {
            this.Cursor = e.Cursor;
        }

        private void EditorForm_LayerFormHidden(object sender, FormHiddenEventArgs e)
        {
            //if (mnuLayersPopout.Checked == true)
            //{
            //    layerListForm_.Hide();
            //    layerListForm_.IsClosed = true;
            //    mnuLayersPopout.Checked = false;
            //}
        }

        private void EditorForm_AssetSelectionFormHidden(object sender, FormHiddenEventArgs e)
        {
            //if (mnuAssetsPopout.Checked == true)
            //{
            //    assetSelectionForm_.Hide();
            //    assetSelectionForm_.IsClosed = true;
            //    mnuAssetsPopout.Checked = false;
            //}
        }

        private void EditorForm_WidgetInstanceListFormHidden(object sender, FormHiddenEventArgs e)
        {
            //if (mnuAssetsPopout.Checked == true)
            //{
            //instancePropertiesForm_.Hide();
            //instancePropertiesForm_.IsClosed = true;
            //    mnuAssetsPopout.Checked = false;
            //}
        }

        private void EditorForm_RoomsFormHidden(object sender, FormHiddenEventArgs e)
        {
            //if (mnuRoomsPopout.Checked == true)
            //{
            //    roomListForm_.Hide();
            //    roomListForm_.IsClosed = true;
            //    mnuRoomsPopout.Checked = false;
            //}
        }

        private void EditorForm_PropertyGridFormHidden(object sender, FormHiddenEventArgs e)
        {
            //if (mnuPropertyGridPopout.Checked == true)
            //{
            //    propertyGridForm_.Hide();
            //    propertyGridForm_.IsClosed = true;
            //    mnuPropertyGridPopout.Checked = false;
            //}
        }

        private void EditorForm_Resize(object sender, System.EventArgs e)
        {
            ////findmetodo fix the resizing mode
            //findmeroomchange
            //roomEditorControl_.Left = 0;
            //roomEditorControl_.Top = 0;
            //roomEditorControl_.Height = pnlMain.ClientSize.Height; // - ssStatus.Height;
            //roomEditorControl_.Width = pnlMain.ClientSize.Width;

            this.Refresh();
        }

        private void mnuNewProject_Click(object sender, System.EventArgs e)
        {
            newProject();
        }

        private void mnuSaveProject_Click(object sender, EventArgs e)
        {
            saveProject(false);
        }

        private void mnuSaveProjectAs_Click(object sender, EventArgs e)
        {
            saveProject(true);
        }

        private void mnuCut_Click(object sender, System.EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            
            copySelectedMapWidgets(selectedRoomIndex);

            deleteSelectedMapWidgets(selectedRoomIndex);
        }

        private void mnuFill_Click(object sender, System.EventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            Guid selectedRoomId = uiState.SelectedRoomId;            
        }

        private void mnuResizeToGame_Click(object sender, System.EventArgs e)
        {
            resizeToGame();
        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool continueSave = false;
            bool continueAction = true;

            e.Cancel = false;

            ProjectDto project = projectController_.GetProjectDto();

            if (project != null)
            {
                if (projectController_.ChangesMade == true)
                {
                    DialogResult res = MessageBox.Show("Changes have been made to the current project. Do you wish to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);

                    if (res == DialogResult.Yes)
                    {
                        continueSave = true;
                        continueAction = true;
                    }
                    else if (res == DialogResult.No)
                    {
                        continueSave = false;
                        continueAction = true;
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        continueSave = false;
                        continueAction = false;
                    }
                }

                if (continueSave == true)
                {
                    saveProject(false);
                }

                if (continueAction == false)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    saveRecentProjectsList();

                    //findmeroomchangeroomEditorControl_.Dispose();

                    layerListForm_.ChildControl.Dispose();
                    assetSelectionForm_.ChildControl.Dispose();
                    roomListForm_.ChildControl.Dispose();
                    instancesForm_.ChildControl.Dispose();

                    layerListForm_.Close();
                    assetSelectionForm_.Close();
                    roomListForm_.Close();
                    instancesForm_.Close();
                }
            }
            else
            {
                //findmeroomchangeroomEditorControl_.Dispose();

                layerListForm_.ChildControl.Dispose();
                assetSelectionForm_.ChildControl.Dispose();
                roomListForm_.ChildControl.Dispose();
                instancesForm_.ChildControl.Dispose();

                layerListForm_.Close();
                assetSelectionForm_.Close();
                roomListForm_.Close();
                instancesForm_.Close();
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuToolsDraw_Click(object sender, EventArgs e)
        {
            projectController_.SetEditMode(EditMode.Draw);
        }

        private void mnuToolsSelect_Click(object sender, EventArgs e)
        {
            projectController_.SetEditMode(EditMode.Selection);
        }

        private void mnuToolsGrab_Click(object sender, EventArgs e)
        {
            projectController_.SetEditMode(EditMode.Grab);
        }

        private void mnuViewCameraLock_Click(object sender, EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (mnuViewCameraLock.Checked == true)
            {
                // The camera is currently in locked mode. Toggle it to free mode.
                tsslbCameraMode.Text = "Camera Mode: Free";

                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.MaximizeBox = true;

                projectController_.SetCameraMode(CameraMode.CameraFree);
            }
            else
            {
                tsslbCameraMode.Text = "Camera Mode: Locked";

                resizeToGame();

                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;

                projectController_.SetCameraMode(uiState.CameraMode = CameraMode.CameraLocked);
            }

            mnuViewCameraLock.Checked = !mnuViewCameraLock.Checked;
        }

        private void mnuToolsResetPopouts_Click(object sender, EventArgs e)
        {
            locatePopoutForms();
        }

        private void mnuLayersPopout_Click(object sender, EventArgs e)
        {
            //if (mnuLayersPopout.Checked == true)
            //{
            //    layerListForm_.Hide();
            //}
            //else
            //{
            //    layerListForm_.Show(this);
            //}

            //layerListForm_.IsClosed = mnuLayersPopout.Checked;

            //mnuLayersPopout.Checked = !mnuLayersPopout.Checked;
        }

        private void mnuAssetsPopout_Click(object sender, EventArgs e)
        {
            //if (mnuAssetsPopout.Checked == true)
            //{
            //    assetSelectionForm_.Hide();
            //}
            //else
            //{
            //    assetSelectionForm_.Show(this);
            //}

            //assetSelectionForm_.IsClosed = mnuAssetsPopout.Checked;

            //mnuAssetsPopout.Checked = !mnuAssetsPopout.Checked;
        }

        private void mnuRoomsPopout_Click(object sender, EventArgs e)
        {
            //if (mnuRoomsPopout.Checked == true)
            //{
            //    roomListForm_.Hide();
            //}
            //else
            //{
            //    roomListForm_.Show(this);
            //}

            //roomListForm_.IsClosed = mnuRoomsPopout.Checked;

            //mnuRoomsPopout.Checked = !mnuRoomsPopout.Checked;
        }

        public void EditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();
            ProjectDto project = projectController_.GetProjectDto();

            if (project != null)
            {
                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                EditMode editMode = uiState.EditMode;
                //findmeroomchange
                //if (assetSelectionForm_.AssetSelectionControl.IsTileObjectSelected == true && roomEditorControl_.IsMouseOver == true)                    
                //{
                //    e.Handled = true;

                //    Guid selectedTileObjectId = uiState.SelectedTileObjectId;

                //    Point2D tileObjectCursorCell = uiState.TileObjectCursorCell[selectedTileObjectId];

                //    switch (e.KeyCode)
                //    {
                //        case Keys.Down:

                //            if (editMode == EditMode.Draw)
                //            {
                //                projectController_.SetTileObjectCursorCell(uiState.SelectedTileObjectId, new Point2D(tileObjectCursorCell.X, tileObjectCursorCell.Y + 1));

                //                roomEditorControl_.Refresh();
                //            }

                //            break;

                //        case Keys.Up:

                //            if (editMode == EditMode.Draw)
                //            {
                //                projectController_.SetTileObjectCursorCell(uiState.SelectedTileObjectId, new Point2D(tileObjectCursorCell.X, tileObjectCursorCell.Y - 1));

                //                roomEditorControl_.Refresh();
                //            }
                //            break;

                //        case Keys.Left:

                //            if (editMode == EditMode.Draw)
                //            {
                //                projectController_.SetTileObjectCursorCell(uiState.SelectedTileObjectId, new Point2D(tileObjectCursorCell.X - 1, tileObjectCursorCell.Y));

                //                roomEditorControl_.Refresh();
                //            }

                //            break;

                //        case Keys.Right:

                //            if (editMode == EditMode.Draw)
                //            {
                //                projectController_.SetTileObjectCursorCell(uiState.SelectedTileObjectId, new Point2D(tileObjectCursorCell.X + 1, tileObjectCursorCell.Y));

                //                roomEditorControl_.Refresh();
                //            }

                //            break;

                //        default:
                //            break;
                //    }
                //}
            }
        }

        private void EditorForm_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Removed because this behavior is undersirable. It's too easy to accidentally change tile, and
            // it's not likely that the other tiles you will be using will be next in the list anyway.

            //ProjectDto project = projectController_.GetProjectDto();
            //ProjectUiStateDto uiState = projectController_.GetUiState();

            //if (project == null || project.IsPrepared == false)
            //{
            //    return;
            //}

            //int selectedRoomIndex = uiState.SelectedRoomIndex;
            //Guid selectedRoomId = uiState.SelectedRoomId;

            //if (uiState.TileObjectsActive[selectedRoomId] == true)
            //{
            //    Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

            //    int tileSheetIndex = projectController_.GetTileSheetIndexFromId(tileSheetId);

            //    int objectCount = project.TileObjects[tileSheetId].Count;

            //    bool useAdHoc = false;

            //    if (project.AdHocTileObjects[tileSheetId] != null)
            //    {
            //        // Add one to the count, to account for the ad hod object.
            //        objectCount += 1;
            //        useAdHoc = true;
            //    }

            //    int currentObject = uiState.SelectedTileObjectIndex[tileSheetId];

            //    if (e.Delta > 0)
            //    {
            //        currentObject--;
            //    }
            //    else
            //    {
            //        currentObject++;
            //    }

            //    if (currentObject >= 0 && currentObject < objectCount)
            //    {
            //        projectController_.SelectTileObject(tileSheetIndex, currentObject);
            //    }
            //}
            //else if (uiState.TilesetActive[selectedRoomId] == true)
            //{
            //    Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

            //    int tileSheetIndex = projectController_.GetTileSheetIndexFromId(tileSheetId);

            //    if (tileSheetId != Guid.Empty)
            //    {
            //        int currentTile = uiState.SelectedTileIndex[selectedRoomId];

            //        int tileColumns = project.TileSheets[tileSheetIndex].Columns;
            //        int tileRows = project.TileSheets[tileSheetIndex].Rows;

            //        int tileCount = tileColumns * tileRows;

            //        if (e.Delta > 0)
            //        {
            //            currentTile--;
            //        }
            //        else
            //        {
            //            currentTile++;
            //        }

            //        if (currentTile >= 0 && currentTile <= tileCount)
            //        {
            //            projectController_.SetSelectedTileIndex(selectedRoomIndex, currentTile);
            //        }
            //    }
            //}
            //else if (uiState.EntitiesActive[selectedRoomId] == true)
            //{
            //    //int actorCount = rcRooms_.AssetContainer.ActorCount;

            //    //int currentActor = rcRooms_.AssetContainer.SelectedActorIndex;

            //    //if (e.Delta > 0)
            //    //{
            //    //    currentActor--;
            //    //}
            //    //else
            //    //{
            //    //    currentActor++;
            //    //}

            //    //if (currentActor <= actorCount && currentActor > 0)
            //    //{
            //    //    rcRooms_.AssetContainer.selectActor(currentActor);
            //    //}
            //    //else
            //    //{
            //    //    // Out of range. Undo the last increment made.
            //    //    if (e.Delta > 0)
            //    //    {
            //    //        currentActor++;
            //    //    }
            //    //    else
            //    //    {
            //    //        currentActor--;
            //    //    }
            //    //}
            //}

            //this.Refresh();
        }

        private void mnuShowOutlines_Click(object sender, EventArgs e)
        {
            projectController_.ToggleOutlines();
            mnuShowOutlines.Checked = !mnuShowOutlines.Checked;
        }

        private void mnuSelectFolder_Click(object sender, EventArgs e)
        {
            selectProjectFolder();
        }

        private void mnuExport_Click(object sender, EventArgs e)
        {
         
        }

        private void mnuUndo_Click(object sender, EventArgs e)
        {
            projectController_.Undo();
        }

        private void rbtnRedo_Click(object sender, EventArgs e)
        {
            projectController_.Redo();
        }

        private void mnuRedo_Click(object sender, EventArgs e)
        {
            projectController_.Redo();
        }
        
        private void mnuExportAndRun_Click(object sender, EventArgs e)
        {
            projectLauncher_.Launch();
        }

        private void mnuEdit_Popup(object sender, EventArgs e)
        {
            if (projectController_.GetUndoStackSize() > 0)
            {
                mnuUndo.Enabled = true;
            }
            else
            {
                mnuUndo.Enabled = false;
            }

            if (projectController_.GetRedoStackSize() > 0)
            {
                mnuRedo.Enabled = true;
            }
            else
            {
                mnuRedo.Enabled = false;
            }

            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;            
        }
        
        private void rbtnAssets_Click(object sender, EventArgs e)
        {
            assetEditor_.ShowDialog(this, projectController_, nameValidator_, nameGenerator_, exceptionHandler_);
        }

        private void rbtnGrid_Click(object sender, EventArgs e)
        {
            projectController_.ToggleGrid();
        }

        private void rbtnScripts_Click(object sender, EventArgs e)
        {
            scriptsForm_.ShowDialog(this);
        }

        private void rbtnSpriteSheetBuilder_Click(object sender, EventArgs e)
        {
            sheetBuilderForm_.ShowDialog(this);
        }

        private void rtbnUndo_Click(object sender, EventArgs e)
        {
            projectController_.Undo();
        }

        private void romiSaveAs_Click(object sender, EventArgs e)
        {
            saveProject(true);
        }

        private void romiSave_Click(object sender, EventArgs e)
        {
            saveProject(false);
        }

        private void romiNew_Click(object sender, EventArgs e)
        {
            newProject();
        }


        private void rbtnOpen_Click(object sender, EventArgs e)
        {

        }

        private void rbtnOpen_DropDownItemClicked(object sender, RibbonItemEventArgs e)
        {

        }

        private void rbtnNewProject_Click(object sender, EventArgs e)
        {

        }

        private void romiOpen_Click(object sender, EventArgs e)
        {
            loadProject();
        }

        private void romiRun_Click(object sender, EventArgs e)
        {
            projectLauncher_.Launch();
        }

        private void romiRunConsole_Click(object sender, EventArgs e)
        {
            projectLauncher_.LaunchWithConsole();
        }

        private void rbnToolbar_ExpandedChanged(object sender, EventArgs e)
        {
        }

        private void rbtnDraw_Click(object sender, EventArgs e)
        {
            projectController_.SetEditMode(EditMode.Draw);
        }

        private void rbtnSelect_Click(object sender, EventArgs e)
        {
            projectController_.SetEditMode(EditMode.Selection);
        }

        private void rbtnScroll_Click(object sender, EventArgs e)
        {
            projectController_.SetEditMode(EditMode.Grab);
        }

        private void tmrPostLoad_Tick(object sender, EventArgs e)
        {
            return;
        }
        
        private void mnuRunWithConsole_Click(object sender, EventArgs e)
        {
            projectLauncher_.LaunchWithConsole();
        }

        private void mnuPropertyGridPopout_Click(object sender, EventArgs e)
        {
            //if (mnuPropertyGridPopout.Checked == true)
            //{
            //    propertyGridForm_.Hide();
            //}
            //else
            //{
            //    propertyGridForm_.Show(this);
            //}

            //roomListForm_.IsClosed = mnuPropertyGridPopout.Checked;

            //mnuPropertyGridPopout.Checked = !mnuPropertyGridPopout.Checked;
        }

        private void mnuShowCameraOutline_Click(object sender, EventArgs e)
        {
            toggleCameraOutline();
        }
        
        private void mnuShowWarnings_Click(object sender, EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            uiState.ShowWarnings = !uiState.ShowWarnings;

            mnuShowWarnings.Checked = uiState.ShowWarnings;
        }

        private void bgWorkLoadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            // Load the project file.
            ProjectDto project = loadProjectDtoFromFile(projectFileName_);

            e.Result = project;
        }

        private void bgWorkLoadFile_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgWorkLoadFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The project has loaded.
            ProjectDto project = (ProjectDto)e.Result;


            if (project != null)
            {
                progressForm_.SetStatus("Preparing Project");

                bgWorkPrepareFile.RunWorkerAsync(project);
            }
            else
            {
                projectFileName_ = string.Empty;

                progressForm_.Hide();
            }
        }

        private void bgWorkPrepareFile_DoWork(object sender, DoWorkEventArgs e)
        {
            ProjectDto project = (ProjectDto)e.Argument;

            projectController_.CreateNewProject(project, null);

            e.Result = project;
        }

        private void bgWorkPrepareFile_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgWorkPrepareFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The project has been prepared.

            // Lets any controls know that the project has been loaded and prepared.
            projectController_.FinalizeProject();

            ProjectDto project = (ProjectDto)e.Result;

            progressForm_.Hide();

            addRecentProject(project.ProjectName, projectFileName_);

            setMenuEnabled();
            
            if (mnuViewCameraLock.Checked == true)
            {
                resizeToGame();
            }

            this.Text = "Firemelon Editor : " + project.ProjectName;

            tsslbProjectFolder.Text = "Project Folder: " + project.ProjectFolderFullPath;

            rcbTileObjects.Checked = false;

            projectController_.SetCanSelectMapWidget(MapWidgetType.TileObject, false);

            this.Refresh();

            this.Focus();

            this.CenterToScreen();

            //foreach (RoomDto room in project.Rooms)
            //{
            //    IRoomEditorControl roomEditorControl = firemelonEditorFactory_.NewRoomEditorControl(projectController_);

            //    IPopoutForm roomEditorForm = firemelonEditorFactory_.NewPopoutForm((Control)roomEditorControl, room.Name);
                
            //    roomEditorForm.Show(pnlMain, DockState.Document);
            //}

            showPopoutForms();

            locatePopoutForms();

        }

        private void mnuChangeProjectName_Click(object sender, EventArgs e)
        {
            changeProjectNameDialog_.ShowDialog(this);
        }

        private void mnuManageScripts_Click(object sender, EventArgs e)
        {
            scriptsForm_.Show(this);
        }

        private void mnuSpriteSheetBuilder_Click(object sender, EventArgs e)
        {
            sheetBuilderForm_.Show(this);
        }


        private void mnuOpenProject_Click(object sender, EventArgs e)
        {
            loadProject();
        }

        private void mnuShowgrid_Click(object sender, System.EventArgs e)
        {
            toggleGrid();
        }

        private void mnuTransparentSelect_Click(object sender, System.EventArgs e)
        {
            projectController_.ToggleTransparentSelect();

            mnuTransparentSelect.Checked = !mnuTransparentSelect.Checked;
        }

        private void mnuDelete_Click(object sender, System.EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
           
            deleteSelectedMapWidgets(selectedRoomIndex);
        }
        
        private void mnuCopy_Click(object sender, System.EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            
            copySelectedMapWidgets(selectedRoomIndex);            
        }

        private void mnuPaste_Click(object sender, System.EventArgs e)
        {
            pasteMapWidgets();
        }

        private void rbtnShowGrid_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetShowGrid(rbtnShowGrid.Checked);
        }

        private void rbtnSelectProjectFolder_Click(object sender, EventArgs e)
        {
            selectProjectFolder();
        }

        private void rcbCameraOutline_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetShowCameraOutline(rcbCameraOutline.Checked);
        }

        private void rcbSelectActors_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetCanSelectMapWidget(MapWidgetType.Actor, rcbSelectActors.Checked);
        }

        private void rcbSelectAudioSources_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetCanSelectMapWidget(MapWidgetType.AudioSource, rcbSelectAudioSources.Checked);
        }

        private void rcbLockCamera_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (rcbLockCamera.Checked == true)
            {
                projectController_.SetCameraMode(uiState.CameraMode = CameraMode.CameraLocked);
            }
            else
            {
                // The camera is currently in locked mode. Toggle it to free mode.              
                projectController_.SetCameraMode(CameraMode.CameraFree);
            }
        }

        private void rcbSelectEvents_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetCanSelectMapWidget(MapWidgetType.Event, rcbSelectEvents.Checked);
        }

        private void rcbSelectHudElements_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetCanSelectMapWidget(MapWidgetType.HudElement, rcbSelectHudElements.Checked);
        }

        private void rcbShowOutlines_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetShowOutlines(rcbShowOutlines.Checked);
        }

        private void rcbShowWorldGeometry_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.ShowWorldGeometry(rcbShowWorldGeometry.Checked);
        }

        private void rcbParticleEmitters_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetCanSelectMapWidget(MapWidgetType.ParticleEmitter, rcbParticleEmitters.Checked);
        }

        private void rcbSpawnPoints_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetCanSelectMapWidget(MapWidgetType.SpawnPoint, rcbSpawnPoints.Checked);
        }

        private void rcbTileObjects_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetCanSelectMapWidget(MapWidgetType.TileObject, rcbTileObjects.Checked);
        }

        private void rcbWorldGeometry_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectController_.SetCanSelectMapWidget(MapWidgetType.WorldGeometry, rcbWorldGeometry.Checked);
        }

        private void pnlMain_ActiveDocumentChanged(object sender, EventArgs e)
        {

        }

        private void rbtnAssetSelector_Click(object sender, EventArgs e)
        {
            ((DockContent)assetSelectionForm_).DockState = DockState.DockLeft;
        }

        private void rbtnInstances_Click(object sender, EventArgs e)
        {
            ((DockContent)instancesForm_).DockState = DockState.DockLeft;
        }

        private void rbtnLayers_Click(object sender, EventArgs e)
        {
            ((DockContent)layerListForm_).DockState = DockState.DockRight;
        }

        private void rbtnProperties_Click(object sender, EventArgs e)
        {
            ((DockContent)propertyGridForm_).DockState = DockState.DockLeft;
        }

        private void rbtnResources_Click(object sender, EventArgs e)
        {
            resourcesForm_.ShowDialog(this);
        }

        private void rbtnRooms_Click(object sender, EventArgs e)
        {
            ((DockContent)roomListForm_).DockState = DockState.DockRight;
        }

        private void rcbShowWarnings_CheckBoxCheckChanged(object sender, EventArgs e)
        {
            projectLauncher_.ShowWarnings = rcbShowWarnings.Checked;
        }

        #endregion

    }
}
