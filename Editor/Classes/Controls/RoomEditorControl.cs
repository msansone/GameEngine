using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    #region Enums

    public enum CameraMode 
    { 
        CameraLocked = 1, 
        CameraFree   = 2,
    };

    #endregion

    #region Delegates
    #endregion

    public class RoomEditorControl : UserControl, IRoomEditorControl
    {
        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomEditorControl));
            this.pbTiles = new System.Windows.Forms.PictureBox();
            this.hsCanvasOffset = new System.Windows.Forms.HScrollBar();
            this.vsCanvasOffset = new System.Windows.Forms.VScrollBar();
            this.hsCameraPosition = new System.Windows.Forms.HScrollBar();
            this.vsCameraPosition = new System.Windows.Forms.VScrollBar();
            this.tmrScroll = new System.Windows.Forms.Timer(this.components);
            this.ilOverlay = new System.Windows.Forms.ImageList(this.components);
            this.pnlCorner = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbTiles)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTiles
            // 
            this.pbTiles.BackColor = System.Drawing.Color.Black;
            this.pbTiles.Location = new System.Drawing.Point(194, 162);
            this.pbTiles.Name = "pbTiles";
            this.pbTiles.Size = new System.Drawing.Size(414, 284);
            this.pbTiles.TabIndex = 0;
            this.pbTiles.TabStop = false;
            this.pbTiles.Click += new System.EventHandler(this.pbTiles_Click);
            this.pbTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTiles_Paint);
            this.pbTiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTiles_MouseDown);
            this.pbTiles.MouseEnter += new System.EventHandler(this.pbTiles_MouseEnter);
            this.pbTiles.MouseLeave += new System.EventHandler(this.pbTiles_MouseLeave);
            this.pbTiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTiles_MouseMove);
            this.pbTiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbTiles_MouseUp);
            // 
            // hsCanvasOffset
            // 
            this.hsCanvasOffset.Enabled = false;
            this.hsCanvasOffset.LargeChange = 1;
            this.hsCanvasOffset.Location = new System.Drawing.Point(0, 584);
            this.hsCanvasOffset.Maximum = 1;
            this.hsCanvasOffset.Minimum = 1;
            this.hsCanvasOffset.Name = "hsCanvasOffset";
            this.hsCanvasOffset.Size = new System.Drawing.Size(392, 16);
            this.hsCanvasOffset.TabIndex = 1;
            this.hsCanvasOffset.Value = 1;
            this.hsCanvasOffset.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsCanvasOffset_Scroll);
            // 
            // vsCanvasOffset
            // 
            this.vsCanvasOffset.Enabled = false;
            this.vsCanvasOffset.LargeChange = 1;
            this.vsCanvasOffset.Location = new System.Drawing.Point(784, 0);
            this.vsCanvasOffset.Maximum = 1;
            this.vsCanvasOffset.Minimum = 1;
            this.vsCanvasOffset.Name = "vsCanvasOffset";
            this.vsCanvasOffset.Size = new System.Drawing.Size(16, 292);
            this.vsCanvasOffset.TabIndex = 2;
            this.vsCanvasOffset.Value = 1;
            this.vsCanvasOffset.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsCanvasOffset_Scroll);
            // 
            // hsCameraPosition
            // 
            this.hsCameraPosition.Enabled = false;
            this.hsCameraPosition.LargeChange = 1;
            this.hsCameraPosition.Location = new System.Drawing.Point(392, 584);
            this.hsCameraPosition.Name = "hsCameraPosition";
            this.hsCameraPosition.Size = new System.Drawing.Size(392, 16);
            this.hsCameraPosition.TabIndex = 3;
            this.hsCameraPosition.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsCameraPosition_Scroll);
            // 
            // vsCameraPosition
            // 
            this.vsCameraPosition.Enabled = false;
            this.vsCameraPosition.LargeChange = 1;
            this.vsCameraPosition.Location = new System.Drawing.Point(784, 292);
            this.vsCameraPosition.Name = "vsCameraPosition";
            this.vsCameraPosition.Size = new System.Drawing.Size(16, 292);
            this.vsCameraPosition.TabIndex = 4;
            this.vsCameraPosition.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsCameraPosition_Scroll);
            // 
            // tmrScroll
            // 
            this.tmrScroll.Interval = 50;
            this.tmrScroll.Tick += new System.EventHandler(this.tmrScroll_Tick);
            // 
            // ilOverlay
            // 
            this.ilOverlay.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilOverlay.ImageStream")));
            this.ilOverlay.TransparentColor = System.Drawing.Color.Fuchsia;
            this.ilOverlay.Images.SetKeyName(0, "Overlay20.bmp");
            this.ilOverlay.Images.SetKeyName(1, "Overlay21.bmp");
            this.ilOverlay.Images.SetKeyName(2, "Overlay22.bmp");
            this.ilOverlay.Images.SetKeyName(3, "Overlay23.bmp");
            this.ilOverlay.Images.SetKeyName(4, "Overlay24.bmp");
            this.ilOverlay.Images.SetKeyName(5, "Overlay25.bmp");
            this.ilOverlay.Images.SetKeyName(6, "Overlay26.bmp");
            this.ilOverlay.Images.SetKeyName(7, "Overlay27.bmp");
            // 
            // pnlCorner
            // 
            this.pnlCorner.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pnlCorner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCorner.Location = new System.Drawing.Point(784, 584);
            this.pnlCorner.Name = "pnlCorner";
            this.pnlCorner.Size = new System.Drawing.Size(13, 13);
            this.pnlCorner.TabIndex = 5;
            // 
            // RoomEditorControl
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.pnlCorner);
            this.Controls.Add(this.vsCameraPosition);
            this.Controls.Add(this.hsCameraPosition);
            this.Controls.Add(this.vsCanvasOffset);
            this.Controls.Add(this.hsCanvasOffset);
            this.Controls.Add(this.pbTiles);
            this.DoubleBuffered = true;
            this.Name = "RoomEditorControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.RoomEditorControl_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RoomEditorControl_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.RoomEditorControl_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RoomEditorControl_MouseMove);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.RoomEditorControl_PreviewKeyDown);
            this.Resize += new System.EventHandler(this.RoomEditorControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbTiles)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #region Events

        new public event CursorChangedHandler CursorChanged;

        #endregion

        #region Private Variables

        private System.Windows.Forms.PictureBox pbTiles;
		private System.Windows.Forms.HScrollBar hsCanvasOffset;
        private System.Windows.Forms.VScrollBar vsCanvasOffset;
        private System.ComponentModel.IContainer components;
        private HScrollBar hsCameraPosition;
        private VScrollBar vsCameraPosition;
        private Timer tmrScroll;
        private ImageList ilOverlay;
        private Panel pnlCorner;
        
        private Graphics g;

        private IFiremelonEditorFactory firemelonEditorFactory_;
        
        private IBackgroundGenerator backgroundGenerator_;

        private IDrawingUtility drawingUtility_;

        // Variables related to rendering the canvas background.
        private Size szBounds_;
        private Size szBuffer_;
        private Bitmap bmpBlankTile_;

        private IProjectController projectController_;

        private Guid mouseOverActorInstanceId_;
        private Guid mouseOverEventInstanceId_;
        private Guid mouseOverHudElementInstanceId_;

        private Guid mouseOverMapWidgetId_;
        
        private IRoomEditorCursor audioSourceCursor_;
        private IRoomEditorCursor particleEmitterCursor_;
        private IRoomEditorCursor spawnPointCursor_;
        private IRoomEditorCursor worldGeometryCursor_;
        private IRoomEditorCursor tileObjectCursor_;

        private IMapWidgetFactory mapWidgetFactory_;

        private IUtilityFactory utilityFactory_;

        private Point2D actorDragStart_;
        private Point2D eventDragStart_;
        private Point2D grabberOffset_;
        private Point2D hudElementDragStart_;
        private Point2D keyboardDragOffset_;
        private Point2D mapWidgetDragStart_;
        private Point2D mapWidgetDragSquareStart_;
        private Point2D newEventPoint1_;
        private Point2D newEventPoint2_;

        private Point2D resizingCornerVertical_;
        private Point2D resizingCornerHorizontal_;

        private WorldGeometryWidgetDto displayOnlyWorldGeometryChunk_;
        private EventWidgetDto displayOnlyEvent_;

        private int oldX_, oldY_;
        private int squareMouseOverX_, squareMouseOverY_;
        private int mouseCursorX_, mouseCursorY_;
        
        private Rectangle resizeNewBounds_;

        private bool isDraggingMapWidgetSelection_;
        private bool isDrawingMapWidgetSelection_;            
        private bool isDrawingNewEvent_;
        private bool isDrawingNewWorldGeometry_;
        private bool isResizingMapWidget_;
        private bool isScrollTimerStarted_;

        private GrabberDirection mapWidgetResizeDirection_ = GrabberDirection.None;        

        private bool isUpArrowKeyDown_;
        private bool isDownArrowKeyDown_;
        private bool isRightArrowKeyDown_;
        private bool isLeftArrowKeyDown_;

        private bool keyboardMovingObjects_;
        private bool mouseLeftDown_;
        private bool mouseRightDown_;
        private bool mouseMiddleDown_;
        private Cursor previousCursor_;
        private bool showCursor_;
        
        private Dictionary<Point, TileObjectDto> tileObjectsAdded_;
        private Dictionary<MapWidgetMode, IRoomEditorCursor> mapWidgetCursor_;

        #endregion

        #region Constructors

        public RoomEditorControl(IProjectController projectController)
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            utilityFactory_ = new UtilityFactory();

            backgroundGenerator_ = firemelonEditorFactory_.NewBackgroundGenerator();

            drawingUtility_ = utilityFactory_.NewDrawingUtility();

            mapWidgetCursor_ = new Dictionary<MapWidgetMode, IRoomEditorCursor>();
            
            mapWidgetFactory_ = firemelonEditorFactory_.NewMapWidgetFactory(projectController);
            
            projectController_ = projectController;

            projectController_.CameraModeChanged += new CameraModeChangedHandler(this.RoomEditorControl_CameraModeChanged);

            projectController_.RefreshView += new RefreshViewHandler(this.RoomEditorControl_RefreshView);
            projectController_.ProjectCreated += new ProjectCreateHandler(this.RoomEditorControl_ProjectCreated);
            projectController_.RoomAdded += new RoomAddHandler(this.RoomEditorControl_RoomAdded);
            projectController_.BeforeRoomDeleted += new BeforeRoomDeletedHandler(this.RoomEditorControl_BeforeRoomDeleted);
            projectController_.RoomSelected += new RoomSelectHandler(this.RoomEditorControl_RoomSelected);
            projectController_.LayerAdd += new LayerAddHandler(this.RoomEditorControl_LayerAdded);
            projectController_.LayerResize += new LayerResizeHandler(this.RoomEditorControl_LayerResized);
            projectController_.AfterLayerDelete += new AfterLayerDeleteHandler(this.RoomEditorControl_AfterLayerDeleted);
            projectController_.InteractiveLayerChange += new InteractiveLayerChangeHandler(this.RoomEditorControl_InteractiveLayerChanged);
            projectController_.TileObjectSelected += new TileObjectSelectHandler(this.RoomEditorControl_TileObjectSelected);

            mouseOverActorInstanceId_ = Guid.Empty;
            mouseOverEventInstanceId_ = Guid.Empty;
            mouseOverHudElementInstanceId_ = Guid.Empty;
            mouseOverMapWidgetId_ = Guid.Empty;
            
            actorDragStart_ = new Point2D(0, 0);
            eventDragStart_ = new Point2D(0, 0);
            hudElementDragStart_ = new Point2D(0, 0);
            mapWidgetDragStart_ = new Point2D(0, 0);
            mapWidgetDragSquareStart_ = new Point2D(0, 0);
            grabberOffset_ = new Point2D(0, 0);
            keyboardDragOffset_ = new Point2D(0, 0);
            newEventPoint1_ = new Point2D(0, 0);
            newEventPoint2_ = new Point2D(0, 0);
            
            mouseLeftDown_ = false;
            mouseRightDown_ = false;
            mouseMiddleDown_ = false;
            isDrawingMapWidgetSelection_ = false;
            isDraggingMapWidgetSelection_ = false;
            isDrawingNewEvent_ = false;
            isResizingMapWidget_ = false;
            isDrawingNewWorldGeometry_ = false;
            isUpArrowKeyDown_ = false;
            isDownArrowKeyDown_ = false;
            isRightArrowKeyDown_ = false;
            isLeftArrowKeyDown_ = false;
            keyboardMovingObjects_ = false;

            previousCursor_ = Cursors.Arrow;

            resizeNewBounds_ = new Rectangle(0, 0, 0, 0);
            
            szBuffer_ = new Size(200, 200);
            szBounds_ = new Size(0, 0);

            tileObjectsAdded_ = new Dictionary<Point, TileObjectDto>();
            
            vsCameraPosition.Minimum = 0;
            vsCameraPosition.Maximum = 0;
            vsCameraPosition.SmallChange = 1;
            vsCameraPosition.LargeChange = 1;

            hsCameraPosition.Minimum = 0;
            hsCameraPosition.Maximum = 0;
            hsCameraPosition.SmallChange = 1;
            hsCameraPosition.LargeChange = 1;

            vsCanvasOffset.Minimum = 0;
            vsCanvasOffset.Maximum = 0;
            vsCanvasOffset.SmallChange = 1;
            vsCanvasOffset.LargeChange = 1;

            hsCanvasOffset.Minimum = 0;
            hsCanvasOffset.Maximum = 0;
            hsCanvasOffset.SmallChange = 1;
            hsCanvasOffset.LargeChange = 1;

            vsCanvasOffset.Value = vsCanvasOffset.Minimum;
            vsCameraPosition.Value = vsCameraPosition.Minimum;
            hsCanvasOffset.Value = hsCanvasOffset.Minimum;
            hsCameraPosition.Value = hsCameraPosition.Minimum;            
        }

        #endregion

        #region Properties

        public bool IsMouseOver
        {
            get
            {
                return showCursor_;
            }
        }

        public IProjectController ProjectController
        {
            set
            {
                projectController_ = value;                
            }
        }

        #endregion
        
        #region Public Functions

        public void Clear()
        {
            //isInitialized_ = false;
        }
        
        new public void Dispose()
        {
            base.Dispose();
        }

        public void Initialize()
        {
        }
        
        new public void Refresh()
        {
            base.Refresh();
        }

        public void RefreshScrollbars()
        {
            // The resize code will set the scrollbars.
            resizeControls();
        }
        
        #endregion

        #region Protected Functions

        protected virtual void OnCursorChanged(CursorChangedEventArgs e)
        {
            // Don't call the event if the cursor isn't actually changing.
            if (previousCursor_ != e.Cursor)
            {
                previousCursor_ = e.Cursor;

                CursorChanged?.Invoke(this, e);
            }
        }

        #endregion

        #region Private Functions

        private MapWidgetCreationParametersDto createMapWidgetCreationParameters(MapWidgetType mapWidgetType)
        {
            MapWidgetCreationParametersDto returnValue;

            ProjectUiStateDto uiState = projectController_.GetUiState();

            // Build the data needed to create the selected map widget.
            switch (mapWidgetType)
            {
                case MapWidgetType.TileObject:


                    TileObjectDto tileObject = projectController_.GetTileObject(uiState.SelectedTileObjectId);

                    returnValue = new TileObjectMapWidgetCreationParametersDto(tileObject.Id, tileObject.Name);

                    break;

                case MapWidgetType.Actor:

                    returnValue = new ActorMapWidgetCreationParametersDto(uiState.SelectedActorId);

                    break;

                case MapWidgetType.HudElement:

                    returnValue = new HudElementMapWidgetCreationParametersDto(uiState.SelectedHudElementId);

                    break;

                default:

                    returnValue = new MapWidgetCreationParametersDto(mapWidgetType);

                    break;
            }

            return returnValue;
        }

        private Bitmap generateBackground()
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;

            int tileSize = project.TileSize;

            int maxCols = uiState.MaxCols[selectedRoomId];
            int maxRows = uiState.MaxRows[selectedRoomId];

            // Create a background bitmap the size of the canvas, plus the extra buffer region for smooth resizing.

            // The two colors to toggle between.
            SolidBrush bBackground1 = new SolidBrush(Color.LightGray);
            SolidBrush bBackground2 = new SolidBrush(Color.White);

            // The size of the little background squares.
            int smallBoxSize = 8;

            // Keep track of the row, so that the squares can be checkerboarded.
            int currentBackgroundRow = 0;

            // Determine the size of the background. Start by calculating the size of the largest layer.
            // Then, if it is larger than the canvas, clip it down to just the canvas size.
        
            int largestLayerHeight = maxRows * tileSize;
            int largestLayerWidth = maxCols * tileSize;

            if (largestLayerHeight >= pbTiles.Height)
            {
                szBounds_.Height = pbTiles.Height;
            }
            else
            {
                szBounds_.Height = largestLayerHeight;
            }

            if (largestLayerWidth >= pbTiles.Width)
            {
                szBounds_.Width = pbTiles.Width;
            }
            else
            {
                szBounds_.Width = largestLayerWidth;
            }

            Bitmap bmpBG = new Bitmap(szBounds_.Width + szBuffer_.Width, szBounds_.Height + szBuffer_.Height);

            Graphics gBackground = Graphics.FromImage(bmpBG);

            // First blank out the entire background with black.
            gBackground.FillRectangle(Brushes.Black, this.ClientRectangle);

            // Toggle between white and gray.
            bool isToggled = true;

            Size szFullBG = new Size(szBounds_.Width + szBuffer_.Width, szBounds_.Height + szBuffer_.Height);

            // If the largest layer height or width  is smaller than the extra buffer region, don't render the
            // background squares into it.
            if (szFullBG.Height > largestLayerHeight)
            {
                szFullBG.Height = largestLayerHeight;
            }

            if (szFullBG.Width > largestLayerWidth)
            {
                szFullBG.Width = largestLayerWidth;
            }

            // Render the squares into the bitmap. If the buffer region goes beyond the largest layer it is drawing squares where it should be black.

            for (int j = 0; j < szFullBG.Height; j += smallBoxSize)
            {
                for (int i = 0; i < szFullBG.Width; i += smallBoxSize)
                {
                    if (isToggled)
                    {
                        gBackground.FillRectangle(bBackground1, i, j, smallBoxSize, smallBoxSize);
                    }
                    else
                    {
                        gBackground.FillRectangle(bBackground2, i, j, smallBoxSize, smallBoxSize);
                    }

                    isToggled = !isToggled;
                }

                currentBackgroundRow++;

                // Change toggled value for checkerboard pattern.
                isToggled = (currentBackgroundRow % 2 == 0);
            }
            
            gBackground.Dispose();

            return bmpBG;
        }

        private List<System.Drawing.Rectangle> generateGrabberRects(Rectangle boundingRect)
        {
            // Gather the entity bounds data.
            int x = boundingRect.Left;
            int y = boundingRect.Top;
            int w = boundingRect.Width;
            int h = boundingRect.Height;

            int size = Globals.grabberSize;

            int midX = w / 2;
            int midY = h / 2;
            int grabberMid = size / 2;

            List<System.Drawing.Rectangle> lstGrabberRects = new List<System.Drawing.Rectangle>();

            // Northwest
            lstGrabberRects.Add(new System.Drawing.Rectangle(x, y, size, size));

            // North
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + midX - grabberMid, y, size, size));

            // Northeast
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + w - size, y, size, size));

            // East
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + w - size, y + midY - grabberMid, size, size));

            // Southeast
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + w - size, y + h - size, size, size));

            // South
            lstGrabberRects.Add(new System.Drawing.Rectangle(x + midX - grabberMid, y + h - size, size, size));

            // Southwest
            lstGrabberRects.Add(new System.Drawing.Rectangle(x, y + h - size, size, size));

            // West
            lstGrabberRects.Add(new System.Drawing.Rectangle(x, y + midY - grabberMid, size, size));

            return lstGrabberRects;
        }

        private void getVisibleTileRange(int layerOrdinal, ref int startTileX, ref int startTileY, ref int visibleTilesX, ref int visibleTilesY)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int roomIndex = uiState.SelectedRoomIndex;

            int tileSize = project.TileSize;

            int layerIndex = projectController_.GetLayerIndexFromOrdinal(roomIndex, layerOrdinal);

            LayerDto layer = projectController_.GetLayerByIndex(roomIndex, layerIndex);

            // Get the location of the layer in canvas space.
            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(layerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            int layerWidth = layer.Cols * tileSize;
            int layerHeight = layer.Rows * tileSize;

            // First, check if the layer is visible on the x axis.
            if (((canvasLocation.X + (layer.Cols * tileSize)) > 0) && canvasLocation.X < pbTiles.Width)
            {
                // Second, check along the y axis.
                if (((canvasLocation.Y + (layer.Rows * tileSize)) > 0) && canvasLocation.Y < pbTiles.Height)
                {
                    // This layer is visible so calculate which tile to start with, and how many tiles to draw.
                    if (canvasLocation.X < 0)
                    {
                        // Calculate which tile to start drawing at.
                        if (canvasLocation.X < pbTiles.Width)
                        {
                            startTileX = 0;
                        }
                        else
                        {
                            startTileX = Math.Abs(canvasLocation.X) / tileSize;
                        }

                        if ((layerWidth + canvasLocation.X) > pbTiles.Width)
                        {
                            // Draw the amount that fit in the current space left over.
                            visibleTilesX = (pbTiles.Width - canvasLocation.X) / tileSize;

                            if (((pbTiles.Width - canvasLocation.X) % tileSize) != 0)
                            {
                                visibleTilesX++;
                            }

                            if (visibleTilesX > layer.Cols)
                            {
                                visibleTilesX = layer.Cols;
                            }
                        }
                        else
                        {
                            // Need to calculate how many tiles are visible
                            visibleTilesX = layer.Cols - startTileX;
                        }
                    }
                    else
                    {
                        // Start tile is going to be the first one. Calculate how many more are visible.
                        startTileX = 0;

                        visibleTilesX = (pbTiles.Width - canvasLocation.X) / tileSize;

                        if (((pbTiles.Width - canvasLocation.X) % tileSize) != 0)
                        {
                            visibleTilesX++;
                        }

                        if (visibleTilesX > layer.Cols)
                        {
                            visibleTilesX = layer.Cols;
                        }
                    }

                    if (canvasLocation.Y > 0)
                    {
                        // Calculate which tile to start drawing at.

                        if (canvasLocation.Y < pbTiles.Height)
                        {
                            startTileY = 0;
                        }
                        else
                        {
                            startTileY = Math.Abs(canvasLocation.Y) / tileSize;
                        }

                        if ((layerHeight + canvasLocation.Y) > pbTiles.Height)
                        {
                            // Draw the amount that fit in the current space left over.
                            visibleTilesY = (pbTiles.Height - canvasLocation.Y) / tileSize;

                            if (((pbTiles.Height - canvasLocation.Y) % tileSize) != 0)
                            {
                                visibleTilesY++;
                            }

                            if (visibleTilesY > layer.Rows)
                            {
                                visibleTilesY = layer.Rows;
                            }
                        }
                        else
                        {
                            // Need to calculate how many tiles are visible
                            visibleTilesY = layer.Rows - startTileY;
                        }
                    }
                    else
                    {
                        // Start tile is going to be the first one. Calculate how many more are visible.
                        startTileY = 0;

                        visibleTilesY = (pbTiles.Height - canvasLocation.Y) / tileSize;

                        if (((pbTiles.Height - canvasLocation.Y) % tileSize) != 0)
                        {
                            visibleTilesY++;
                        }

                        if (visibleTilesY > layer.Rows)
                        {
                            visibleTilesY = layer.Rows;
                        }
                    }
                }
                else
                {
                    // This layer is not on the screen. Set the start tiles so that it will get skipped.
                    startTileX = layer.Cols;
                    startTileY = layer.Rows;
                }
            }
            else
            {
                // This layer is not on the screen. Set the start tiles so that it will get skipped.
                startTileX = layer.Cols;
                startTileY = layer.Rows;
            }
        }

        private void locateCamera()
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int interactiveLayerIndex = project.InteractiveLayerIndexes[selectedRoomId];

            LayerDto interactiveLayer = project.Layers[selectedRoomId][interactiveLayerIndex];

            int tileSize = project.TileSize;
            int cameraWidth = project.CameraWidth;
            int cameraHeight = project.CameraHeight;

            int maxCols = uiState.MaxCols[selectedRoomId];
            int maxRows = uiState.MaxRows[selectedRoomId];

            int cameraX = uiState.CameraLocation[selectedRoomId].X;
            int cameraY = uiState.CameraLocation[selectedRoomId].Y;

            // Check if the camera position is beyond that of the map, and move it back if it is.
            if (cameraX + cameraWidth > maxCols * tileSize)
            {
                uiState.CameraLocation[selectedRoomId].X = (maxCols * tileSize) - cameraWidth;
            }
            else if (cameraX < 0)
            {
                uiState.CameraLocation[selectedRoomId].X = 0;
            }

            if (cameraY + cameraHeight > maxRows * tileSize)
            {
                uiState.CameraLocation[selectedRoomId].Y = (maxRows * tileSize) - cameraHeight;
            }
            else if (cameraY < 0)
            {
                uiState.CameraLocation[selectedRoomId].Y = 0;
            }

            // If the interactive layer was changed, update the scrollbars.
            int newVMax = vsCameraPosition.Minimum + ((interactiveLayer.Rows * tileSize) - cameraHeight);
            int newHMax = hsCameraPosition.Minimum + ((interactiveLayer.Cols * tileSize) - cameraWidth);

            // If a scrollbar value is out of range, change it so that it aligns to the edge of the map.
            if (vsCameraPosition.Value > newVMax)
            {
                vsCameraPosition.Value = newVMax;
                uiState.CameraLocation[selectedRoomId].Y = newVMax;
            }

            if (hsCameraPosition.Value > newHMax)
            {
                hsCameraPosition.Value = newHMax;
                uiState.CameraLocation[selectedRoomId].X = newHMax;
            }

            uiState.CameraLocationMax[selectedRoomId].X = newHMax;
            uiState.CameraLocationMax[selectedRoomId].Y = newVMax;

            hsCameraPosition.Maximum = newHMax;
            vsCameraPosition.Maximum = newVMax;

            if (uiState.CameraMode == CameraMode.CameraLocked)
            {
                // Interactive layer may have changed, which would throw off where the viewport is rendered.
                // Make an adjustment to the canvas offset here to correctly align it with the viewport.
                Point cameraMap = new Point(0, 0);

                cameraX = uiState.CameraLocation[selectedRoomId].X;
                cameraY = uiState.CameraLocation[selectedRoomId].Y;

                cameraMap = translateWorldToMap(new Point(cameraX, cameraY));

                int deltaX = cameraMap.X - uiState.CanvasOffset[selectedRoomId].X;
                int deltaY = cameraMap.Y - uiState.CanvasOffset[selectedRoomId].Y;

                uiState.CanvasOffset[selectedRoomId].X += deltaX;
                uiState.CanvasOffset[selectedRoomId].Y += deltaY;

                hsCanvasOffset.Value = uiState.CanvasOffset[selectedRoomId].X + hsCanvasOffset.Minimum;
                vsCanvasOffset.Value = uiState.CanvasOffset[selectedRoomId].Y + vsCanvasOffset.Minimum;
            }
        }

        private void mouseDownActorDraw(System.Windows.Forms.MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project != null && project.IsPrepared == true)
            {
                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int tileSize = project.TileSize;

                int cameraX = uiState.CameraLocation[selectedRoomId].X;
                int cameraY = uiState.CameraLocation[selectedRoomId].Y;
                int cameraWidth = project.CameraWidth;
                int cameraHeight = project.CameraHeight;

                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

                LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

                int interactiveLayerIndex = projectController_.GetInteractiveLayerIndex(selectedRoomIndex);
                int interactiveLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, interactiveLayerIndex);

                // Need to get the layer offset so the mouse position is done correctly.
                Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
                Point mapLocation = translateWorldToMap(worldLocation);
                Point canvasLocation = translateToCanvas(mapLocation);

                int rows = selectedLayer.Rows;
                int cols = selectedLayer.Cols;

                bool isVisible = uiState.LayerVisible[selectedLayer.Id];

                if (squareMouseOverY_ >= 0 && squareMouseOverX_ >= 0 && squareMouseOverY_ < rows && squareMouseOverX_ < cols && isVisible == true)
                {
                    // Place the actor onto the current layer.
                    Guid selectedActorId = uiState.SelectedActorId;

                    if (selectedActorId != Guid.Empty)
                    {
                        ActorDto actorEntity = (ActorDto)projectController_.GetEntity(selectedActorId);

                        Point stageCenter = new Point(actorEntity.StageWidth / 2, actorEntity.StageHeight / 2);

                        Size stageOriginTransformation;

                        switch (actorEntity.StageOriginLocation)
                        {
                            case OriginLocation.TopLeft:

                                stageOriginTransformation = new Size(0, 0);

                                break;

                            case OriginLocation.TopMiddle:

                                stageOriginTransformation = new Size(stageCenter.X, 0);

                                break;

                            case OriginLocation.TopRight:

                                stageOriginTransformation = new Size(actorEntity.StageWidth, 0);

                                break;

                            case OriginLocation.MiddleLeft:

                                stageOriginTransformation = new Size(0, stageCenter.Y);

                                break;

                            case OriginLocation.Center:

                                stageOriginTransformation = new Size(stageCenter.X, stageCenter.Y);

                                break;

                            case OriginLocation.MiddleRight:

                                stageOriginTransformation = new Size(actorEntity.StageWidth, stageCenter.Y);

                                break;

                            case OriginLocation.BottomLeft:

                                stageOriginTransformation = new Size(0, actorEntity.StageHeight);

                                break;

                            case OriginLocation.BottomMiddle:

                                stageOriginTransformation = new Size(stageCenter.X, actorEntity.StageHeight);

                                break;

                            case OriginLocation.BottomRight:

                                stageOriginTransformation = new Size(actorEntity.StageWidth, actorEntity.StageHeight);

                                break;

                            default:

                                stageOriginTransformation = new Size(0, 0);

                                break;
                        }

                        Rectangle bounds = new Rectangle(mouseCursorX_ - stageOriginTransformation.Width, mouseCursorY_ - stageOriginTransformation.Height, actorEntity.BoundRect.Width, actorEntity.BoundRect.Height);

                        ActorMapWidgetCreationParametersDto creationParams = (ActorMapWidgetCreationParametersDto)createMapWidgetCreationParameters(MapWidgetType.Actor);

                        creationParams.LayerId = selectedLayer.Id;
                        creationParams.RoomId = selectedRoomId;
                        creationParams.Bounds.Left = bounds.Left;
                        creationParams.Bounds.Top = bounds.Top;
                        creationParams.Bounds.Width = bounds.Width;
                        creationParams.Bounds.Height = bounds.Height;

                        creationParams.Position.X = mouseCursorX_;
                        creationParams.Position.Y = mouseCursorY_;
                        
                        for (int i = 0; i < project.Properties[selectedActorId].Count; i++)
                        {
                            PropertyDto newInstanceProperty = new PropertyDto();
                            newInstanceProperty.Name = project.Properties[selectedActorId][i].Name;
                            newInstanceProperty.Value = project.Properties[selectedActorId][i].DefaultValue;
                            newInstanceProperty.RootOwnerId = project.Properties[selectedActorId][i].Id;

                            creationParams.Properties.Add(newInstanceProperty);
                        }
                        
                        ActorWidgetDto actorWidget = (ActorWidgetDto)projectController_.AddMapWidget(creationParams);
                                                
                        List<Guid> lstSelectedMapWidgetIds = new List<Guid>();
                        lstSelectedMapWidgetIds.Add(actorWidget.Id);

                        projectController_.ClearMapWidgetSelection(selectedRoomIndex);
                        projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstSelectedMapWidgetIds);
                    }
                }
            }
        }
        
        private void mouseDownEventDraw(System.Windows.Forms.MouseEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            isDrawingNewEvent_ = true;

            Point cursorLocation = new Point(mouseCursorX_, mouseCursorY_);
            newEventPoint1_.X = cursorLocation.X;
            newEventPoint1_.Y = cursorLocation.Y;

            newEventPoint2_.X = cursorLocation.X;
            newEventPoint2_.Y = cursorLocation.Y;
        }

        private void mouseDownGenericMapWidgetDraw(System.Windows.Forms.MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project != null && project.IsPrepared == true)
            {
                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int tileSize = project.TileSize;

                int cameraX = uiState.CameraLocation[selectedRoomId].X;
                int cameraY = uiState.CameraLocation[selectedRoomId].Y;
                int cameraWidth = project.CameraWidth;
                int cameraHeight = project.CameraHeight;

                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

                LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

                int interactiveLayerIndex = projectController_.GetInteractiveLayerIndex(selectedRoomIndex);
                int interactiveLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, interactiveLayerIndex);

                // Need to get the layer offset so the mouse position is done correctly.
                Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
                Point mapLocation = translateWorldToMap(worldLocation);
                Point canvasLocation = translateToCanvas(mapLocation);

                int rows = selectedLayer.Rows;
                int cols = selectedLayer.Cols;

                bool isVisible = uiState.LayerVisible[selectedLayer.Id];

                if (squareMouseOverY_ >= 0 && squareMouseOverX_ >= 0 && squareMouseOverY_ < rows && squareMouseOverX_ < cols && isVisible == true)
                {
                    // Place the map widget onto the current layer.
                    MapWidgetType selectedMapWidgetType = uiState.SelectedMapWidgetType[selectedRoomId];

                    MapWidgetMode mapWidgetMode = uiState.MapWidgetMode[selectedRoomId];

                    IRoomEditorCursor cursor = mapWidgetCursor_[mapWidgetMode];
                    
                    MapWidgetCreationParametersDto creationParams = createMapWidgetCreationParameters(selectedMapWidgetType);

                    creationParams.RoomId = selectedRoomId;
                    creationParams.LayerId = selectedLayer.Id;

                    if (cursor.GridAligned == true)
                    {
                        int gridAlignedX = (int)(mouseCursorX_ / tileSize) * tileSize;
                        int gridAlignedY = (int)(mouseCursorY_ / tileSize) * tileSize;

                        creationParams.PositionOffset.X = gridAlignedX - cursor.Offset.X;
                        creationParams.PositionOffset.Y = gridAlignedY - cursor.Offset.Y;
                    }
                    else
                    {
                        creationParams.PositionOffset.X = mouseCursorX_ - cursor.Offset.X;
                        creationParams.PositionOffset.Y = mouseCursorY_ - cursor.Offset.Y;
                    }

                    creationParams.Position.X = mouseCursorX_;
                    creationParams.Position.Y = mouseCursorY_;

                    MapWidgetDto mapWidgetInstance = projectController_.AddMapWidget(creationParams);

                    List<Guid> lstSelectedMapWidgetInstances = new List<Guid>();
                    lstSelectedMapWidgetInstances.Add(mapWidgetInstance.Id);

                    projectController_.ClearMapWidgetSelection(selectedRoomIndex);
                    projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstSelectedMapWidgetInstances);
                }
            }
        }

        private void mouseDownHudElementDraw(System.Windows.Forms.MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project != null && project.IsPrepared == true)
            {
                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int tileSize = project.TileSize;

                int cameraX = uiState.CameraLocation[selectedRoomId].X;
                int cameraY = uiState.CameraLocation[selectedRoomId].Y;
                int cameraWidth = project.CameraWidth;
                int cameraHeight = project.CameraHeight;

                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

                LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

                int interactiveLayerIndex = projectController_.GetInteractiveLayerIndex(selectedRoomIndex);
                int interactiveLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, interactiveLayerIndex);

                // Need to get the layer offset so the mouse position is done correctly.
                Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
                Point mapLocation = translateWorldToMap(worldLocation);
                Point canvasLocation = translateToCanvas(mapLocation);

                int rows = selectedLayer.Rows;
                int cols = selectedLayer.Cols;

                bool isVisible = uiState.LayerVisible[selectedLayer.Id];

                if (squareMouseOverY_ >= 0 && squareMouseOverX_ >= 0 && squareMouseOverY_ < rows && squareMouseOverX_ < cols && isVisible == true)
                {
                    // Place the HUD element onto the current layer.
                    Guid selectedHudElementId = uiState.SelectedHudElementId;

                    if (selectedHudElementId != Guid.Empty)
                    {

                        HudElementDto hudElementEntity = (HudElementDto)projectController_.GetEntity(selectedHudElementId);

                        Point stageCenter = new Point(hudElementEntity.StageWidth / 2, hudElementEntity.StageHeight / 2);

                        Size stageOriginTransformation;

                        switch (hudElementEntity.StageOriginLocation)
                        {
                            case OriginLocation.TopLeft:

                                stageOriginTransformation = new Size(0, 0);

                                break;

                            case OriginLocation.TopMiddle:

                                stageOriginTransformation = new Size(stageCenter.X, 0);

                                break;

                            case OriginLocation.TopRight:

                                stageOriginTransformation = new Size(hudElementEntity.StageWidth, 0);

                                break;

                            case OriginLocation.MiddleLeft:

                                stageOriginTransformation = new Size(0, stageCenter.Y);

                                break;

                            case OriginLocation.Center:

                                stageOriginTransformation = new Size(stageCenter.X, stageCenter.Y);

                                break;

                            case OriginLocation.MiddleRight:

                                stageOriginTransformation = new Size(hudElementEntity.StageWidth, stageCenter.Y);

                                break;


                            case OriginLocation.BottomLeft:

                                stageOriginTransformation = new Size(0, hudElementEntity.StageHeight);

                                break;

                            case OriginLocation.BottomMiddle:

                                stageOriginTransformation = new Size(stageCenter.X, hudElementEntity.StageHeight);

                                break;

                            case OriginLocation.BottomRight:

                                stageOriginTransformation = new Size(hudElementEntity.StageWidth, hudElementEntity.StageHeight);

                                break;

                            default:

                                stageOriginTransformation = new Size(0, 0);

                                break;
                        }

                        Rectangle bounds = new Rectangle(mouseCursorX_ - stageOriginTransformation.Width, mouseCursorY_ - stageOriginTransformation.Height, hudElementEntity.BoundRect.Width, hudElementEntity.BoundRect.Height);
                        
                        HudElementMapWidgetCreationParametersDto creationParams = (HudElementMapWidgetCreationParametersDto)createMapWidgetCreationParameters(MapWidgetType.HudElement);

                        creationParams.LayerId = selectedRoomId; // HUD Elements aren't on layers. The immediate parent is the room.
                        creationParams.RoomId = selectedRoomId;
                        creationParams.Bounds.Left = bounds.Left;
                        creationParams.Bounds.Top = bounds.Top;
                        creationParams.Bounds.Width = bounds.Width;
                        creationParams.Bounds.Height = bounds.Height;

                        for (int i = 0; i < project.Properties[selectedHudElementId].Count; i++)
                        {
                            PropertyDto newInstanceProperty = new PropertyDto();
                            newInstanceProperty.Name = project.Properties[selectedHudElementId][i].Name;
                            newInstanceProperty.Value = project.Properties[selectedHudElementId][i].DefaultValue;
                            newInstanceProperty.RootOwnerId = project.Properties[selectedHudElementId][i].Id;

                            creationParams.Properties.Add(newInstanceProperty);
                        }

                        HudElementWidgetDto hudElementWidget = (HudElementWidgetDto)projectController_.AddMapWidget(creationParams);

                        List<Guid> lstSelectedMapWidgetIds = new List<Guid>();
                        lstSelectedMapWidgetIds.Add(hudElementWidget.Id);

                        projectController_.ClearMapWidgetSelection(selectedRoomIndex);
                        projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstSelectedMapWidgetIds);
                    }
                }
            }
        }

        private void mouseDownSelection(System.Windows.Forms.MouseEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            
            // Start the scroll if the mouse has left.
            if (isScrollTimerStarted_ == false)
            {
                isScrollTimerStarted_ = true;

                tmrScroll.Start();
            }

            if (mouseOverMapWidgetId_ != Guid.Empty)
            {
                if (uiState.MapWidgetSelected[mouseOverMapWidgetId_] == true)
                {
                    // If the widget is selected already and the control key isn't 
                    // down, this is a drag. If the control key is down, remove it from
                    // the selection.
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        List<Guid> lstMapWidgetSelection = new List<Guid>();
                        lstMapWidgetSelection.Add(mouseOverMapWidgetId_);

                        projectController_.RemoveMapWidgetsFromSelection(selectedRoomIndex, lstMapWidgetSelection);
                    }
                    else
                    {
                        // If the control key isn't down, and the widget is selected, check if the mouse is over a grabber.
                        MapWidgetDto mouseOverMapWidget = projectController_.GetMapWidget(mouseOverMapWidgetId_);

                        if (mouseOverMapWidget.Controller is IResizableMapWidgetController)
                        {
                            IResizableMapWidgetController mouseOverMapWidgetController = (IResizableMapWidgetController)mouseOverMapWidget.Controller;

                            Point cursorLocation = new Point(mouseCursorX_, mouseCursorY_);

                            mapWidgetResizeDirection_ = mouseOverMapWidgetController.GetSizeMode(cursorLocation);

                            if (mapWidgetResizeDirection_ != GrabberDirection.None)
                            {
                                isResizingMapWidget_ = true;

                                // Update the display only version with the data from the selected widget.
                                if (mouseOverMapWidget is WorldGeometryWidgetDto)
                                {
                                    displayOnlyWorldGeometryChunk_ = (WorldGeometryWidgetDto)mapWidgetFactory_.CreateMapWidget(new MapWidgetCreationParametersDto(MapWidgetType.WorldGeometry));

                                    displayOnlyWorldGeometryChunk_.Id = mouseOverMapWidget.Id;

                                    displayOnlyWorldGeometryChunk_.Name = "Display Only World Geometry";

                                    displayOnlyWorldGeometryChunk_.Corner1.X = ((WorldGeometryWidgetDto)mouseOverMapWidget).Corner1.X;
                                    displayOnlyWorldGeometryChunk_.Corner1.Y = ((WorldGeometryWidgetDto)mouseOverMapWidget).Corner1.Y;
                                    displayOnlyWorldGeometryChunk_.Corner2.X = ((WorldGeometryWidgetDto)mouseOverMapWidget).Corner2.X;
                                    displayOnlyWorldGeometryChunk_.Corner2.Y = ((WorldGeometryWidgetDto)mouseOverMapWidget).Corner2.Y;
                                    displayOnlyWorldGeometryChunk_.CollisionStyle = ((WorldGeometryWidgetDto)mouseOverMapWidget).CollisionStyle;
                                    displayOnlyWorldGeometryChunk_.SlopeRise = ((WorldGeometryWidgetDto)mouseOverMapWidget).SlopeRise;
                                    
                                    switch (mapWidgetResizeDirection_)
                                    { 
                                    case GrabberDirection.North:

                                        // Use the top most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.Y < displayOnlyWorldGeometryChunk_.Corner2.Y)
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }
                                       
                                        break;

                                    case GrabberDirection.South:

                                        // Use the bottom most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.Y > displayOnlyWorldGeometryChunk_.Corner2.Y)
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }
                                        
                                        break;

                                    case GrabberDirection.West:

                                        // Use the left most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.X < displayOnlyWorldGeometryChunk_.Corner2.X)
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        break;

                                    case GrabberDirection.East:

                                        // Use the right most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.X > displayOnlyWorldGeometryChunk_.Corner2.X)
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        break;

                                    case GrabberDirection.NorthEast:

                                        // Use the right most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.X > displayOnlyWorldGeometryChunk_.Corner2.X)
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        // Use the top most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.Y < displayOnlyWorldGeometryChunk_.Corner2.Y)
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        break;

                                    case GrabberDirection.SouthWest:

                                        // Use the left most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.X < displayOnlyWorldGeometryChunk_.Corner2.X)
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }


                                        // Use the bottom most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.Y > displayOnlyWorldGeometryChunk_.Corner2.Y)
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        break;

                                    case GrabberDirection.NorthWest:

                                        // Use the left most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.X < displayOnlyWorldGeometryChunk_.Corner2.X)
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        // Use the top most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.Y < displayOnlyWorldGeometryChunk_.Corner2.Y)
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        break;

                                    case GrabberDirection.SouthEast:

                                        // Use the right most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.X > displayOnlyWorldGeometryChunk_.Corner2.X)
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerHorizontal_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        // Use the bottom most corner.
                                        if (displayOnlyWorldGeometryChunk_.Corner1.Y > displayOnlyWorldGeometryChunk_.Corner2.Y)
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner1;
                                        }
                                        else
                                        {
                                            resizingCornerVertical_ = displayOnlyWorldGeometryChunk_.Corner2;
                                        }

                                        break;
                                    }

                                    displayOnlyWorldGeometryChunk_.Controller.GetBoundingRect();

                                }
                                else if (mouseOverMapWidget is EventWidgetDto)
                                {
                                    displayOnlyEvent_ = (EventWidgetDto)mapWidgetFactory_.CreateMapWidget(new EventMapWidgetCreationParametersDto(Guid.Empty));

                                    displayOnlyEvent_.Id = mouseOverMapWidget.Id;

                                    displayOnlyEvent_.BoundingBox.Left = mouseOverMapWidget.BoundingBox.Left;
                                    displayOnlyEvent_.BoundingBox.Top = mouseOverMapWidget.BoundingBox.Top;
                                    displayOnlyEvent_.BoundingBox.Width = mouseOverMapWidget.BoundingBox.Width;
                                    displayOnlyEvent_.BoundingBox.Height = mouseOverMapWidget.BoundingBox.Height;                                    
                                }
                            }
                            else
                            {
                                isResizingMapWidget_ = false;
                            }
                        }
                    }
                }
                else
                {
                    // If the entity is not selected, add it to the selection.
                    if (Control.ModifierKeys != Keys.Control)
                    {
                        projectController_.ClearMapWidgetSelection(selectedRoomIndex);
                    }

                    List<Guid> lstMapWidgetIds = new List<Guid>();
                    lstMapWidgetIds.Add(mouseOverMapWidgetId_);
                    projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstMapWidgetIds);
                }

                if (isResizingMapWidget_ == false)
                {
                    isDraggingMapWidgetSelection_ = true;

                    mapWidgetDragStart_.X = mouseCursorX_;
                    mapWidgetDragStart_.Y = mouseCursorY_;

                    mapWidgetDragSquareStart_.X = squareMouseOverX_;
                    mapWidgetDragSquareStart_.Y = squareMouseOverY_;
                }
            }
            else
            {
                // If the mouse is not over a map widget, start dragging a selection rect.
                isDrawingMapWidgetSelection_ = true;

                projectController_.SetMapWidgetSelectionLayer(selectedRoomIndex, selectedLayerIndex);

                projectController_.SetMapWidgetSelectorCorner1(selectedRoomIndex, mouseCursorX_, mouseCursorY_);
            }
        }
        
        private void mouseDownWorldGeometryDraw(System.Windows.Forms.MouseEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            isDrawingNewWorldGeometry_ = true;
            
            displayOnlyWorldGeometryChunk_ = (WorldGeometryWidgetDto)mapWidgetFactory_.CreateMapWidget(new MapWidgetCreationParametersDto(MapWidgetType.WorldGeometry));
            
            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            LayerDto layer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);
            Guid selectedLayerId = layer.Id;

            displayOnlyWorldGeometryChunk_.OwnerId = selectedLayerId;

            displayOnlyWorldGeometryChunk_.RootOwnerId = selectedRoomId;

            displayOnlyWorldGeometryChunk_.Corner1.X = squareMouseOverX_;
            displayOnlyWorldGeometryChunk_.Corner1.Y = squareMouseOverY_;

            displayOnlyWorldGeometryChunk_.Corner2.X = squareMouseOverX_;
            displayOnlyWorldGeometryChunk_.Corner2.Y = squareMouseOverY_;
        }

        private void mouseMoveSelection(System.Windows.Forms.MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;
            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            // Get the location of the layer in canvas space.
            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            Point cursorLocation = new Point(mouseCursorX_, mouseCursorY_);
            
            if (isResizingMapWidget_ == true)
            {
                MapWidgetDto mouseOverMapWidget = projectController_.GetMapWidget(mouseOverMapWidgetId_);

                // The un-sized rect.
                Rectangle rect = mouseOverMapWidget.BoundingBox;

                int originalLeft =0;
                int originalTop = 0;
                int originalBottom = 0;
                int originalRight = 0;

                if (mouseOverMapWidget is EventWidgetDto)
                {
                    // Calculate the resized rect's dimensions.
                    resizeNewBounds_.Left = rect.Left;
                    resizeNewBounds_.Top = rect.Top;
                    resizeNewBounds_.Width = rect.Width;
                    resizeNewBounds_.Height = rect.Height;

                    originalBottom = rect.Top + rect.Height;
                    originalRight = rect.Left + rect.Width;
                }
                else if (mouseOverMapWidget is WorldGeometryWidgetDto)
                {
                    originalLeft = displayOnlyWorldGeometryChunk_.Corner1.X;
                    originalTop = displayOnlyWorldGeometryChunk_.Corner1.Y;
                    originalRight = displayOnlyWorldGeometryChunk_.Corner2.X;
                    originalBottom = displayOnlyWorldGeometryChunk_.Corner2.Y;
                }

                // Update the Cell based on the sizing mode.
                switch (mapWidgetResizeDirection_)
                {
                case GrabberDirection.North:

                    // Update Cell1 Y.
                    if (mouseOverMapWidget is WorldGeometryWidgetDto)
                    {
                        if (resizingCornerVertical_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerVertical_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerVertical_.Y = squareMouseOverY_;
                        }
                    }
                    else if (mouseOverMapWidget is EventWidgetDto)
                    {
                        resizeNewBounds_.Top = mouseCursorY_ - grabberOffset_.Y;
                        resizeNewBounds_.Height = originalBottom - resizeNewBounds_.Top;

                        if (resizeNewBounds_.Height < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Top = rect.Top + rect.Height - Globals.minimumEventSize;
                            resizeNewBounds_.Height = Globals.minimumEventSize;
                        }

                        displayOnlyEvent_.BoundingBox = resizeNewBounds_;
                    }

                    break;

                case GrabberDirection.South:

                    // Update Cell2 Y.
                    if (mouseOverMapWidget is WorldGeometryWidgetDto)
                    {
                        if (resizingCornerVertical_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerVertical_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerVertical_.Y = squareMouseOverY_;
                        }
                    }
                    else if (mouseOverMapWidget is EventWidgetDto)
                    {
                        resizeNewBounds_.Height = mouseCursorY_ - rect.Top;

                        if (resizeNewBounds_.Height < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Height = Globals.minimumEventSize;
                        }

                        displayOnlyEvent_.BoundingBox = resizeNewBounds_;
                    }

                    break;

                case GrabberDirection.West:

                    // Update Cell1 X.
                    if (mouseOverMapWidget is WorldGeometryWidgetDto)
                    {
                        if (resizingCornerHorizontal_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerHorizontal_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerHorizontal_.X = squareMouseOverX_;
                        }
                    }
                    else if (mouseOverMapWidget is EventWidgetDto)
                    {
                        resizeNewBounds_.Left = mouseCursorX_ - grabberOffset_.X;
                        resizeNewBounds_.Width = originalRight - resizeNewBounds_.Left;

                        if (resizeNewBounds_.Width < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Left = rect.Left + rect.Width - Globals.minimumEventSize;
                            resizeNewBounds_.Width = Globals.minimumEventSize;
                        }

                        displayOnlyEvent_.BoundingBox = resizeNewBounds_;
                    }

                    break;

                case GrabberDirection.East:

                    // Update Cell2 X.
                    if (mouseOverMapWidget is WorldGeometryWidgetDto)
                    {
                        if (resizingCornerHorizontal_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerHorizontal_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerHorizontal_.X = squareMouseOverX_;
                        }
                    }
                    else if (mouseOverMapWidget is EventWidgetDto)
                    {
                        resizeNewBounds_.Width = mouseCursorX_ - rect.Left;

                        if (resizeNewBounds_.Width < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Width = Globals.minimumEventSize;
                        }

                        displayOnlyEvent_.BoundingBox = resizeNewBounds_;
                    }

                    break;

                case GrabberDirection.NorthEast:

                    // Update Cell2 X and Cell1 Y.
                    if (mouseOverMapWidget is WorldGeometryWidgetDto)
                    {
                        if (resizingCornerHorizontal_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerHorizontal_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerHorizontal_.X = squareMouseOverX_;
                        }

                        if (resizingCornerVertical_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerVertical_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerVertical_.Y = squareMouseOverY_;
                        }
                    }
                    else if (mouseOverMapWidget is EventWidgetDto)
                    {
                        resizeNewBounds_.Top = mouseCursorY_ - grabberOffset_.Y;
                        resizeNewBounds_.Height = originalBottom - resizeNewBounds_.Top;

                        if (resizeNewBounds_.Height < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Top = rect.Top + rect.Height - Globals.minimumEventSize;
                            resizeNewBounds_.Height = Globals.minimumEventSize;
                        }

                        resizeNewBounds_.Width = mouseCursorX_ - rect.Left;

                        if (resizeNewBounds_.Width < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Width = Globals.minimumEventSize;
                        }

                        displayOnlyEvent_.BoundingBox = resizeNewBounds_;
                    }

                    break;

                case GrabberDirection.SouthWest:

                    // Update Cell1 X and Cell2 Y.
                    if (mouseOverMapWidget is WorldGeometryWidgetDto)
                    {

                        if (resizingCornerHorizontal_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerHorizontal_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerHorizontal_.X = squareMouseOverX_;
                        }

                        if (resizingCornerVertical_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerVertical_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerVertical_.Y = squareMouseOverY_;
                        }
                    }
                    else if (mouseOverMapWidget is EventWidgetDto)
                    {
                        resizeNewBounds_.Height = mouseCursorY_ - rect.Top;

                        if (resizeNewBounds_.Height < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Height = Globals.minimumEventSize;
                        }

                        resizeNewBounds_.Left = mouseCursorX_ - grabberOffset_.X;
                        resizeNewBounds_.Width = originalRight - resizeNewBounds_.Left;

                        if (resizeNewBounds_.Width < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Left = rect.Left + rect.Width - Globals.minimumEventSize;
                            resizeNewBounds_.Width = Globals.minimumEventSize;
                        }

                        displayOnlyEvent_.BoundingBox = resizeNewBounds_;
                    }

                    break;

                case GrabberDirection.NorthWest:

                    // Update Cell1 X and Cell1 Y.
                    if (mouseOverMapWidget is WorldGeometryWidgetDto)
                    {
                        if (resizingCornerHorizontal_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerHorizontal_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerHorizontal_.X = squareMouseOverX_;
                        }

                        if (resizingCornerVertical_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerVertical_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerVertical_.Y = squareMouseOverY_;
                        }
                    }
                    else if (mouseOverMapWidget is EventWidgetDto)
                    {
                        resizeNewBounds_.Top = mouseCursorY_ - grabberOffset_.Y;
                        resizeNewBounds_.Height = originalBottom - resizeNewBounds_.Top;

                        if (resizeNewBounds_.Height < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Top = rect.Top + rect.Height - Globals.minimumEventSize;
                            resizeNewBounds_.Height = Globals.minimumEventSize;
                        }

                        resizeNewBounds_.Left = mouseCursorX_ - grabberOffset_.X;
                        resizeNewBounds_.Width = originalRight - resizeNewBounds_.Left;

                        if (resizeNewBounds_.Width < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Left = rect.Left + rect.Width - Globals.minimumEventSize;
                            resizeNewBounds_.Width = Globals.minimumEventSize;
                        }

                        displayOnlyEvent_.BoundingBox = resizeNewBounds_;
                    }

                    break;

                case GrabberDirection.SouthEast:

                    // Update Cell2 X and Cell2 Y.
                    if (mouseOverMapWidget is WorldGeometryWidgetDto)
                    {
                        if (resizingCornerHorizontal_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerHorizontal_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerHorizontal_.X = squareMouseOverX_;
                        }

                        if (resizingCornerVertical_ == null)
                        {
                            System.Diagnostics.Debug.Print("Warning: resizingCornerVertical_ is null, but this should never happen. Investigate.");
                        }
                        else
                        {
                            resizingCornerVertical_.Y = squareMouseOverY_;
                        }
                    }
                    else if (mouseOverMapWidget is EventWidgetDto)
                    {
                        resizeNewBounds_.Width = mouseCursorX_ - rect.Left;

                        if (resizeNewBounds_.Width < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Width = Globals.minimumEventSize;
                        }

                        resizeNewBounds_.Height = mouseCursorY_ - rect.Top;

                        if (resizeNewBounds_.Height < Globals.minimumEventSize)
                        {
                            resizeNewBounds_.Height = Globals.minimumEventSize;
                        }

                        displayOnlyEvent_.BoundingBox = resizeNewBounds_;
                    }

                    break;
                }

                if (mouseOverMapWidget is WorldGeometryWidgetDto)
                {
                    WorldGeometryWidgetDto mouseOverWorldGeometryMapWidget = (WorldGeometryWidgetDto)mouseOverMapWidget;

                    // Set the default slope rise if this is a slope.
                    switch (displayOnlyWorldGeometryChunk_.CollisionStyle)
                    {
                    case WorldGeometryCollisionStyle.Incline:
                    case WorldGeometryCollisionStyle.InvertedDecline:

                        if (mouseOverWorldGeometryMapWidget.SlopeRise >= (Math.Abs(displayOnlyWorldGeometryChunk_.Corner1.Y - displayOnlyWorldGeometryChunk_.Corner2.Y) + 1))
                        {
                            displayOnlyWorldGeometryChunk_.SlopeRise = Math.Abs(displayOnlyWorldGeometryChunk_.Corner1.Y - displayOnlyWorldGeometryChunk_.Corner2.Y) + 1;
                        }

                        break;

                    case WorldGeometryCollisionStyle.Decline:
                    case WorldGeometryCollisionStyle.InvertedIncline:

                        if (mouseOverWorldGeometryMapWidget.SlopeRise <= -(Math.Abs(displayOnlyWorldGeometryChunk_.Corner1.Y - displayOnlyWorldGeometryChunk_.Corner2.Y) + 1))
                        {
                            displayOnlyWorldGeometryChunk_.SlopeRise = -(Math.Abs(displayOnlyWorldGeometryChunk_.Corner1.Y - displayOnlyWorldGeometryChunk_.Corner2.Y) + 1);
                        }

                        break;
                    }
                }
            }
            else if (isDraggingMapWidgetSelection_ == false)
            {
                if (isDrawingMapWidgetSelection_ == false)
                {
                    // Determine which map widget the mouse is over.

                    mouseOverMapWidgetId_ = Guid.Empty;

                    bool isMouseOverMapWidget = false;

                    // Loop through the map widget ID's in the current grid cell the mouse is over.
                    // Test if the mouse is over them.

                    if (squareMouseOverX_ < 0 || squareMouseOverX_ >= project.Layers[selectedRoomId][selectedLayerIndex].Cols)
                    {
                        return;
                    }

                    if (squareMouseOverY_ < 0 || squareMouseOverY_ >= project.Layers[selectedRoomId][selectedLayerIndex].Rows)
                    {
                        return;
                    }

                    MapWidgetDto mouseOverMapWidget = null;

                    // Check HUD Elements first.
                    List<Guid> hudElementMapWidgetIdList = project.Rooms[selectedRoomIndex].MapWidgetIds;

                    foreach (Guid mapWidgetId in hudElementMapWidgetIdList)
                    {
                        MapWidgetDto mapWidget = projectController_.GetMapWidget(mapWidgetId);

                        int left = mapWidget.BoundingBox.Left;
                        int width = mapWidget.BoundingBox.Width;
                        int top = mapWidget.BoundingBox.Top;
                        int height = mapWidget.BoundingBox.Height;

                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, width, height);

                        if (rect.Contains(pbTiles.PointToClient(Cursor.Position)) == true)
                        {
                            if (uiState.CanSelectMapWidget[mapWidget.Type] == true)
                            {
                                mouseOverMapWidgetId_ = mapWidgetId;

                                mouseOverMapWidget = mapWidget;

                                isMouseOverMapWidget = true;
                            }

                            break;
                        }
                    }

                    if (isMouseOverMapWidget == false)
                    {
                        List<Guid> mapWidgetIdList = project.Layers[selectedRoomId][selectedLayerIndex].MapWidgetIds[squareMouseOverY_][squareMouseOverX_];
                        
                        foreach (Guid mapWidgetId in mapWidgetIdList)
                        {
                            MapWidgetDto mapWidget = projectController_.GetMapWidget(mapWidgetId);

                            int left = mapWidget.BoundingBox.Left;
                            int width = mapWidget.BoundingBox.Width;
                            int top = mapWidget.BoundingBox.Top;
                            int height = mapWidget.BoundingBox.Height;

                            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, width, height);

                            if (rect.Contains(cursorLocation) == true)
                            {
                                if (uiState.CanSelectMapWidget[mapWidget.Type] == true)
                                {
                                    mouseOverMapWidgetId_ = mapWidgetId;

                                    mouseOverMapWidget = mapWidget;

                                    isMouseOverMapWidget = true;

                                    break;
                                }
                            }
                        }
                    }

                    if (isMouseOverMapWidget == true)
                    {
                        if (mouseOverMapWidget.Controller is IResizableMapWidgetController == false)
                        {
                            OnCursorChanged(new CursorChangedEventArgs(Cursors.Arrow));
                        }
                        else
                        {
                            IResizableMapWidgetController resizableMapWidgetController = (IResizableMapWidgetController)mouseOverMapWidget.Controller;

                            bool isSelected = false;

                            if (uiState.MapWidgetSelected.ContainsKey(mouseOverMapWidget.Id) == true)
                            {
                                isSelected = uiState.MapWidgetSelected[mouseOverMapWidget.Id];
                            }

                            if (isSelected == true)
                            {
                                switch (resizableMapWidgetController.GetSizeMode(cursorLocation))
                                {
                                    case GrabberDirection.North:
                                    case GrabberDirection.South:

                                        OnCursorChanged(new CursorChangedEventArgs(Cursors.SizeNS));

                                        break;

                                    case GrabberDirection.West:
                                    case GrabberDirection.East:

                                        OnCursorChanged(new CursorChangedEventArgs(Cursors.SizeWE));

                                        break;

                                    case GrabberDirection.NorthEast:
                                    case GrabberDirection.SouthWest:

                                        OnCursorChanged(new CursorChangedEventArgs(Cursors.SizeNESW));

                                        break;

                                    case GrabberDirection.NorthWest:
                                    case GrabberDirection.SouthEast:

                                        OnCursorChanged(new CursorChangedEventArgs(Cursors.SizeNWSE));

                                        break;

                                    case GrabberDirection.None:

                                        OnCursorChanged(new CursorChangedEventArgs(Cursors.Arrow));

                                        break;
                                }
                            }
                            else
                            {
                                OnCursorChanged(new CursorChangedEventArgs(Cursors.Arrow));
                            }
                        }
                    }
                    else
                    {
                        OnCursorChanged(new CursorChangedEventArgs(Cursors.Cross));
                    }
                }
                else
                {
                    projectController_.SetMapWidgetSelectionOn(selectedRoomIndex, true);
                    projectController_.SetMapWidgetSelectorCorner2(selectedRoomIndex, mouseCursorX_, mouseCursorY_);
                }
            }
        }


        private void mouseMoveEventDraw(System.Windows.Forms.MouseEventArgs e)
        {
            if (isDrawingNewEvent_ == true)
            {
                Point cursorLocation = new Point(0, 0);
                cursorLocation = new Point(mouseCursorX_, mouseCursorY_);

                newEventPoint2_.X = cursorLocation.X;
                newEventPoint2_.Y = cursorLocation.Y;

                pbTiles.Refresh();
            }
        }


        private void mouseMoveWorldGeometryDraw(System.Windows.Forms.MouseEventArgs e)
        {
            if (isDrawingNewWorldGeometry_ == true)
            {
                ProjectDto project = projectController_.GetProjectDto();

                displayOnlyWorldGeometryChunk_.Corner2.X = squareMouseOverX_;
                displayOnlyWorldGeometryChunk_.Corner2.Y = squareMouseOverY_;
                
                pbTiles.Refresh();
            }
        }

        private void mouseUpEventDraw(System.Windows.Forms.MouseEventArgs e)
        {
            isDrawingNewEvent_ = false;

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            Guid selectedEventId = uiState.SelectedEventId;

            if (selectedEventId == Guid.Empty)
            {
                // No event is currently selected.
                return;
            }

            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            LayerDto layer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);
            Guid selectedLayerId = layer.Id;

            Rectangle bounds = new Rectangle(0, 0, 0, 0);

            System.Drawing.Rectangle rect = drawingUtility_.GetRectFromPoints(newEventPoint1_, newEventPoint2_);

            bounds.Left = rect.Left;
            bounds.Top = rect.Top;
            bounds.Width = rect.Width;
            bounds.Height = rect.Height;

            if (bounds.Width >= Globals.minimumEventSize && bounds.Height >= Globals.minimumEventSize)
            {
                EventMapWidgetCreationParametersDto creationParams = new EventMapWidgetCreationParametersDto(selectedEventId, selectedRoomId, selectedLayerId, bounds);

                for (int i = 0; i < project.Properties[selectedEventId].Count; i++)
                {
                    PropertyDto newInstanceProperty = new PropertyDto();
                    newInstanceProperty.Name = project.Properties[selectedEventId][i].Name;
                    newInstanceProperty.Value = project.Properties[selectedEventId][i].DefaultValue;
                    newInstanceProperty.RootOwnerId = project.Properties[selectedEventId][i].Id;

                    creationParams.Properties.Add(newInstanceProperty);
                }

                MapWidgetDto eventMapWidget = projectController_.AddMapWidget(creationParams); 

                List<Guid> lstSelectedEventInstanceIds = new List<Guid>();
                lstSelectedEventInstanceIds.Add(eventMapWidget.Id);

                projectController_.ClearMapWidgetSelection(selectedRoomIndex);
                projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstSelectedEventInstanceIds);
            }

            newEventPoint1_.X = 0;
            newEventPoint1_.Y = 0;

            newEventPoint2_.X = 0;
            newEventPoint2_.Y = 0;

            pbTiles.Refresh();
        }

        private void mouseUpSelection(System.Windows.Forms.MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int tileSize = project.TileSize;
            
            if (isDrawingMapWidgetSelection_ == true)
            {
                if (Control.ModifierKeys != Keys.Control)
                {
                    // If the control key is down add to the selection.
                    // Otherwise, clear it first to start a new selection.
                    projectController_.ClearMapWidgetSelection(selectedRoomIndex);
                }

                // Loop through the layer grid cells the selector intersects with.
                // Test if any of the map widgets in these cells intersect with the
                // selector rect. Select them if they do.

                MapWidgetSelectorDto selector = uiState.MapWidgetSelector[selectedRoomId];

                Guid selectedLayerId = selector.SelectedLayerId;

                LayerDto selectorLayer = projectController_.GetLayerById(selectedLayerId);

                int selectionStartX = selector.SelectionLeft / tileSize;

                if (selectionStartX < 0)
                {
                    selectionStartX = 0;
                }

                if (selectionStartX >= selectorLayer.Cols)
                {
                    selectionStartX = selectorLayer.Cols - 1;
                }

                int selectionStartY = selector.SelectionTop / tileSize;

                if (selectionStartY < 0)
                {
                    selectionStartY = 0;
                }

                if (selectionStartY >= selectorLayer.Rows)
                {
                    selectionStartY = selectorLayer.Rows - 1;
                }

                int selectionEndX = selector.SelectionRight / tileSize;

                if (selectionEndX < 0)
                {
                    selectionEndX = 0;
                }

                if (selectionEndX >= selectorLayer.Cols)
                {
                    selectionEndX = selectorLayer.Cols - 1;
                }

                int selectionEndY = selector.SelectionBottom / tileSize;

                if (selectionEndY < 0)
                {
                    selectionEndY = 0;
                }

                if (selectionEndY >= selectorLayer.Rows)
                {
                    selectionEndY = selectorLayer.Rows - 1;
                }

                List<Guid> lstMapWidgetIds = new List<Guid>();

                for (int i = selectionStartY; i <= selectionEndY; i++)
                {
                    for (int j = selectionStartX; j <= selectionEndX; j++)
                    {
                        for (int k = 0; k < selectorLayer.MapWidgetIds[i][j].Count; k++)
                        {
                            Guid mapWidgetId = selectorLayer.MapWidgetIds[i][j][k];

                            if (uiState.MapWidgetSelected[mapWidgetId] == false)
                            {
                                MapWidgetDto mapWidget = projectController_.GetMapWidget(mapWidgetId);

                                // Get a bounding rectangle for this actor.
                                int x = mapWidget.BoundingBox.Left;
                                int y = mapWidget.BoundingBox.Top;
                                int h = mapWidget.BoundingBox.Height;
                                int w = mapWidget.BoundingBox.Width;

                                System.Drawing.Rectangle widgetRect = new System.Drawing.Rectangle(x, y, w, h);
                                System.Drawing.Rectangle selectorRect = selector.DrawableRect;

                                // If the selector rect has an area of zero, it should still select.
                                // Force it to have an area of 1.
                                if (selectorRect.Height * selectorRect.Width == 0)
                                {
                                    selectorRect.Height = 1;
                                    selectorRect.Width = 1;
                                }

                                System.Drawing.Rectangle resultRect = System.Drawing.Rectangle.Intersect(widgetRect, selectorRect);

                                if (resultRect.IsEmpty == false)
                                {
                                    lstMapWidgetIds.Add(mapWidgetId);
                                }
                            }
                        }
                    }
                }

                if (lstMapWidgetIds.Count > 0)
                {
                    projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstMapWidgetIds);
                }

                projectController_.SetMapWidgetSelectionOn(selectedRoomIndex, false);

                isDrawingMapWidgetSelection_ = false;
            }
            else if (isDraggingMapWidgetSelection_ == true)
            {
                MapWidgetSelectorDto selector = uiState.MapWidgetSelector[selectedRoomId];
                
                foreach (Guid mapWidgetId in selector.SelectedMapWidgetIds)
                {
                    MapWidgetDto mapWidget = projectController_.GetMapWidget(mapWidgetId);

                    int x = mapWidget.Position.X;
                    int y = mapWidget.Position.Y;

                    // The mouse cursor
                    int dragDeltaX = 0;
                    int dragDeltaY = 0;

                    if (mapWidget.Controller.GridAligned == false)
                    {
                        dragDeltaX = mouseCursorX_ - mapWidgetDragStart_.X;
                        dragDeltaY = mouseCursorY_ - mapWidgetDragStart_.Y;
                    }
                    else
                    {
                        dragDeltaX = (squareMouseOverX_ - mapWidgetDragSquareStart_.X) * tileSize;
                        dragDeltaY = (squareMouseOverY_ - mapWidgetDragSquareStart_.Y) * tileSize;
                    }

                    if (dragDeltaX != 0 || dragDeltaY != 0)
                    {
                        projectController_.SetMapWidgetPosition(mapWidgetId, mapWidget.Type, new Point2D(dragDeltaX + x, dragDeltaY + y));
                    }
                }

                isDraggingMapWidgetSelection_ = false;
            }
            else if (isResizingMapWidget_ == true)
            {
                isResizingMapWidget_ = false;

                // Update the map widget from the display only object.

                MapWidgetDto mouseOverMapWidget = projectController_.GetMapWidget(mouseOverMapWidgetId_);

                if (mouseOverMapWidget is WorldGeometryWidgetDto)
                {
                    projectController_.SetWorldGeometryWidgetCorners(displayOnlyWorldGeometryChunk_.Id, displayOnlyWorldGeometryChunk_.Corner1, displayOnlyWorldGeometryChunk_.Corner2);
                }
                else if (mouseOverMapWidget is EventWidgetDto)
                {
                    projectController_.SetMapWidgetBounds(mouseOverMapWidget.Id, displayOnlyEvent_.BoundingBox);
                }
            }
        }

        private void mouseUpWorldGeometryDraw(System.Windows.Forms.MouseEventArgs e)
        {
            if (isDrawingNewWorldGeometry_ == true)
            {
                 isDrawingNewWorldGeometry_ = false;

                ProjectUiStateDto uiState = projectController_.GetUiState();
                
                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];

                LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);
                
                // Place the world geometry chunk widget onto the current layer.
                MapWidgetType selectedMapWidgetType = uiState.SelectedMapWidgetType[selectedRoomId];

                // Copy the creation params from the drawing widget.
                WorldGeometryMapWidgetCreationParametersDto creationParams = new WorldGeometryMapWidgetCreationParametersDto(displayOnlyWorldGeometryChunk_);
                
                MapWidgetDto mapWidgetInstance = projectController_.AddMapWidget(creationParams);

                List<Guid> lstSelectedMapWidgetInstances = new List<Guid>();
                lstSelectedMapWidgetInstances.Add(mapWidgetInstance.Id);

                projectController_.ClearMapWidgetSelection(selectedRoomIndex);
                projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstSelectedMapWidgetInstances);
                
                pbTiles.Refresh();
            }
        }

        private void renderActorCursor(Graphics g)
        {
            if (showCursor_ == false)
            {
                return;
            }

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();
            
            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            int selectedActorIndex = uiState.SelectedActorIndex;
            Guid selectedActorId = uiState.SelectedActorId;

            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            Point cursorLocation = new Point(mouseCursorX_ + canvasLocation.X, mouseCursorY_ + canvasLocation.Y);
            //cursorLocation = translateToCanvas(new Point(mouseCursorX_, mouseCursorY_));
            
            // Render the currently selected actor, if there is one.
            if (selectedActorIndex > -1)
            {
                ActorDto selectedActor = project.Actors[selectedActorIndex];

                if (project.States[selectedActorId].Count > 0)
                {
                    Guid initialStateId = selectedActor.InitialStateId;

                    if (initialStateId != Guid.Empty)
                    {
                        if (project.AnimationSlots.ContainsKey(initialStateId) == true)
                        {
                            int animationSlotCount = project.AnimationSlots[initialStateId].Count;

                            for (int i = 0; i < animationSlotCount; i++)
                            {
                                // Get the index of the animation.
                                Guid animationId = project.AnimationSlots[initialStateId][i].Animation;

                                AnimationDto animation = projectController_.GetAnimation(animationId);

                                if (animation != null)
                                {
                                    Guid spriteSheetId = animation.SpriteSheet;

                                    if (spriteSheetId != Guid.Empty)
                                    {
                                        int sheetIndex = projectController_.GetSpriteSheetIndexFromId(spriteSheetId);

                                        if (sheetIndex > -1)
                                        {
                                            SpriteSheetDto spriteSheet = project.SpriteSheets[sheetIndex];

                                            Guid resourceId = spriteSheet.BitmapResourceId;
                                            
                                            // Separate resources dto removed in 2.2 format.
                                            //BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];
                                            BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(resourceId, true);
                                            
                                            bitmapResource.LoadedModules = (byte)(bitmapResource.LoadedModules ^ (byte)EditorModule.Cursor);

                                            uiState.BitmapsLoadedForRoom.Add(resourceId);

                                            if (project.Frames[animationId].Count > 0)
                                            {
                                                if (project.Frames[animationId][0].SheetCellIndex.HasValue == true)
                                                {
                                                    int cellIndex = project.Frames[animationId][0].SheetCellIndex.Value;

                                                    if (cellIndex > -1)
                                                    {
                                                        float[][] ptsArray ={ new float[] {1, 0, 0, 0,    0},
                                                                              new float[] {0, 1, 0, 0,    0},
                                                                              new float[] {0, 0, 1, 0,    0},
                                                                              new float[] {0, 0, 0, 0.7f, 0},
                                                                              new float[] {0, 0, 0, 0,    1}};

                                                        ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                                                        ImageAttributes imgAttributes = new ImageAttributes();
                                                        imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                                                        int cellWidth = (int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor);

                                                        int cellHeight = (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor);

                                                        Bitmap bmpTemp = new Bitmap(cellWidth, cellHeight);

                                                        Graphics gTmp = Graphics.FromImage(bmpTemp);

                                                        for (int j = 0; j < bitmapResource.SpriteSheetImageList.ImageListRows; j++)
                                                        {
                                                            for (int k = 0; k < bitmapResource.SpriteSheetImageList.ImageListCols; k++)
                                                            {
                                                                int subCellX = k * Globals.maxImageListWidth;
                                                                int subCellY = j * Globals.maxImageListWidth;

                                                                bitmapResource.SpriteSheetImageList.ImageLists[j][k].Draw(gTmp, subCellX, subCellY, cellIndex);
                                                            }
                                                        }

                                                        // Determine the animation position based on the stage origin point and the animation slot origin point.

                                                        // Half of the size of the stage, used for centering.
                                                        Size stageHalfSize = new Size(selectedActor.StageWidth / 2, selectedActor.StageHeight / 2);

                                                        Point2D stageOriginPointInNativeStageSpace = new Point2D(0, 0);

                                                        switch (selectedActor.StageOriginLocation)
                                                        {
                                                            case OriginLocation.TopLeft:

                                                                // The native stage origin is top left, so no change is necessary.

                                                                stageOriginPointInNativeStageSpace.X = 0;

                                                                stageOriginPointInNativeStageSpace.Y = 0;

                                                                break;

                                                            case OriginLocation.TopMiddle:

                                                                stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                                                                stageOriginPointInNativeStageSpace.Y = 0;

                                                                break;


                                                            case OriginLocation.TopRight:

                                                                // The native stage origin is top left, so no change is necessary.

                                                                stageOriginPointInNativeStageSpace.X = selectedActor.StageWidth;

                                                                stageOriginPointInNativeStageSpace.Y = 0;

                                                                break;

                                                            case OriginLocation.MiddleLeft:

                                                                // Calculate the center point of the stage in the native stage space.

                                                                stageOriginPointInNativeStageSpace.X = 0;

                                                                stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                                                                break;

                                                            case OriginLocation.Center:

                                                                // Calculate the center point of the stage in the native stage space.

                                                                stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                                                                stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                                                                break;

                                                            case OriginLocation.MiddleRight:

                                                                // Calculate the center point of the stage in the native stage space.

                                                                stageOriginPointInNativeStageSpace.X = selectedActor.StageWidth;

                                                                stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                                                                break;

                                                            case OriginLocation.BottomLeft:

                                                                stageOriginPointInNativeStageSpace.X = 0;

                                                                stageOriginPointInNativeStageSpace.Y = selectedActor.StageHeight;

                                                                break;

                                                            case OriginLocation.BottomMiddle:

                                                                stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                                                                stageOriginPointInNativeStageSpace.Y = selectedActor.StageHeight;

                                                                break;

                                                            case OriginLocation.BottomRight:

                                                                stageOriginPointInNativeStageSpace.X = selectedActor.StageWidth;

                                                                stageOriginPointInNativeStageSpace.Y = selectedActor.StageHeight;

                                                                break;
                                                        }

                                                        int animationFrameXInStageSpace = 0;

                                                        int animationFrameYInStageSpace = 0;

                                                        Size animationFrameSize = new Size((int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor), (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor));

                                                        Size animationFrameHalfSize = new Size((int)(animationFrameSize.Width / 2), (int)(animationFrameSize.Height / 2));

                                                        // Native animation space uses the default origin of the top left of the animation frame.
                                                        Point2D animationOriginPointInNativeAnimationSpace = new Point2D(0, 0);

                                                        switch (project.AnimationSlots[initialStateId][i].OriginLocation)
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

                                                        switch (selectedActor.StageOriginLocation)
                                                        {
                                                            case OriginLocation.TopLeft:

                                                                // The default stage origin is top left, so we can just use the animation slot position without any coordinate space transformation.

                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;

                                                            case OriginLocation.TopMiddle:

                                                                // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                                // Y-axis is unchanged.
                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;

                                                            case OriginLocation.TopRight:
                                                                
                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + selectedActor.StageWidth - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;

                                                            case OriginLocation.MiddleLeft:

                                                                // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;

                                                            case OriginLocation.Center:

                                                                // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;

                                                            case OriginLocation.MiddleRight:

                                                                // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + selectedActor.StageWidth - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;

                                                            case OriginLocation.BottomLeft:

                                                                // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                                // Y-axis is unchanged.
                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + selectedActor.StageHeight - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;

                                                            case OriginLocation.BottomMiddle:

                                                                // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                                // Y-axis is unchanged.
                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + selectedActor.StageHeight - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;

                                                            case OriginLocation.BottomRight:

                                                                // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                                // Y-axis is unchanged.
                                                                animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + selectedActor.StageWidth - animationOriginPointInNativeAnimationSpace.X;

                                                                animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + selectedActor.StageHeight - animationOriginPointInNativeAnimationSpace.Y;

                                                                break;
                                                        }

                                                        int x = cursorLocation.X + animationFrameXInStageSpace - stageOriginPointInNativeStageSpace.X;
                                                        int y = cursorLocation.Y + animationFrameYInStageSpace - stageOriginPointInNativeStageSpace.Y;
                                                                                         
                                                        int w = bmpTemp.Width;
                                                        int h = bmpTemp.Height;

                                                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, w, h);
                                                        g.DrawImage(bmpTemp, destRect, 0, 0, bmpTemp.Width, bmpTemp.Height, GraphicsUnit.Pixel, imgAttributes);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void renderActorCursorOutline(Graphics g)
        {
            if (showCursor_ == false)
            {
                return;
            }

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedActorIndex = uiState.SelectedActorIndex;
            Guid selectedActorId = uiState.SelectedActorId;

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            Point cursorLocation = new Point(mouseCursorX_ + canvasLocation.X, mouseCursorY_ + canvasLocation.Y);

            // Render the currently selected actor, if there is one.
            if (selectedActorIndex > -1)
            {
                // For now, just render whatever the first state is.
                ActorDto selectedActor = project.Actors[selectedActorIndex];

                if (uiState.ShowOutlines == true)
                {
                    // Half of the size of the stage, used for centering.
                    Size stageHalfSize = new Size(selectedActor.StageWidth / 2, selectedActor.StageHeight / 2);

                    Point2D stageOriginPointInNativeStageSpace = new Point2D(0, 0);

                    switch (selectedActor.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:

                            // The native stage origin is top left, so no change is necessary.

                            stageOriginPointInNativeStageSpace.X = 0;

                            stageOriginPointInNativeStageSpace.Y = 0;

                            break;

                        case OriginLocation.TopMiddle:

                            stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                            stageOriginPointInNativeStageSpace.Y = 0;

                            break;

                        case OriginLocation.TopRight:

                            stageOriginPointInNativeStageSpace.X = selectedActor.StageWidth;

                            stageOriginPointInNativeStageSpace.Y = 0;

                            break;

                        case OriginLocation.MiddleLeft:

                            // Calculate the center point of the stage in the native stage space.

                            stageOriginPointInNativeStageSpace.X = 0;

                            stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                            break;

                        case OriginLocation.Center:

                            // Calculate the center point of the stage in the native stage space.

                            stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                            stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                            break;

                        case OriginLocation.MiddleRight:

                            // Calculate the center point of the stage in the native stage space.

                            stageOriginPointInNativeStageSpace.X = selectedActor.StageWidth;

                            stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                            break;


                        case OriginLocation.BottomLeft:

                            stageOriginPointInNativeStageSpace.X = 0;

                            stageOriginPointInNativeStageSpace.Y = selectedActor.StageHeight;

                            break;

                        case OriginLocation.BottomMiddle:

                            stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                            stageOriginPointInNativeStageSpace.Y = selectedActor.StageHeight;

                            break;

                        case OriginLocation.BottomRight:

                            stageOriginPointInNativeStageSpace.X = selectedActor.StageWidth;

                            stageOriginPointInNativeStageSpace.Y = selectedActor.StageHeight;

                            break;
                    }

                    int x = cursorLocation.X - stageOriginPointInNativeStageSpace.X;
                    int y = cursorLocation.Y - stageOriginPointInNativeStageSpace.Y;

                    int w = selectedActor.StageWidth;
                    int h = selectedActor.StageHeight;

                    Color c = Globals.actorOutlineColor;

                    g.DrawRectangle(new Pen(new SolidBrush(c)), x, y, w, h);
                }
            }
        }

        private void renderCursor(Graphics g, IRoomEditorCursor cursor)
        {
            if (cursor != null && showCursor_ == true)
            {
                ProjectDto project = projectController_.GetProjectDto();
                ProjectUiStateDto uiState = projectController_.GetUiState();

                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

                Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
                Point mapLocation = translateWorldToMap(worldLocation);
                Point canvasLocation = translateToCanvas(mapLocation);

                Point cursorLocation = new Point(mouseCursorX_ + canvasLocation.X, mouseCursorY_ + canvasLocation.Y);

                Point gridAlignedTo = new Point((Math.Abs(canvasLocation.X) % project.TileSize * -1), (Math.Abs(canvasLocation.Y) % project.TileSize * -1));

                cursor.Render(g, cursorLocation.X - gridAlignedTo.X, cursorLocation.Y - gridAlignedTo.Y, gridAlignedTo.X, gridAlignedTo.Y);
            }
        }

        private void renderEventCursor(Graphics g)
        {
            if (isDrawingNewEvent_ == false)
            {
                return;
            }

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedEventIndex = uiState.SelectedEventIndex;
            Guid selectedEventId = uiState.SelectedEventId;

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            Point tempEventPoint1 = new Point(newEventPoint1_.X + canvasLocation.X, newEventPoint1_.Y + canvasLocation.Y);
            Point tempEventPoint2 = new Point(newEventPoint2_.X + canvasLocation.X, newEventPoint2_.Y + canvasLocation.Y);
            
            // Render the currently selected event, if there is one.
            if (selectedEventIndex > -1)
            {
                Point2D p1 = new Point2D(tempEventPoint1.X, tempEventPoint1.Y);
                Point2D p2 = new Point2D(tempEventPoint2.X, tempEventPoint2.Y);

                System.Drawing.Rectangle rect = drawingUtility_.GetRectFromPoints(p1, p2);

                g.DrawRectangle(new Pen(Globals.eventOutlineColor), rect.Left, rect.Top, rect.Width, rect.Height);

                if (rect.Width >= Globals.minimumEventSize && rect.Height >= Globals.minimumEventSize)
                {
                    g.FillRectangle(new SolidBrush(Globals.eventFillColor), rect.Left, rect.Top, rect.Width, rect.Height);

                    EventDto currentEvent = project.Events[selectedEventIndex];

                    Font f = new Font(FontFamily.GenericSansSerif, 8.0f);

                    SizeF stringWidth = g.MeasureString(currentEvent.Name, f);

                    g.FillRectangle(new SolidBrush(Color.Black), rect.Left, rect.Top - stringWidth.ToSize().Height, stringWidth.ToSize().Width, stringWidth.ToSize().Height);

                    g.DrawString(currentEvent.Name, f, new SolidBrush(Color.White), (float)rect.Left, (float)rect.Top - stringWidth.ToSize().Height);
                }
            }
        }
        
        private void renderGrid(Graphics g, int roomIndex, int layerIndex)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int tileSize = project.TileSize;

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;
            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

            // Get the location of the layer in canvas space.
            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            int cols = selectedLayer.Cols;
            int rows = selectedLayer.Rows;

            int layerWidth = cols * tileSize;
            int layerHeight = rows * tileSize;

            // Draw the layer outline.
            Pen p2 = new Pen(Color.Gray);
            p2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            
            //g.DrawRectangle(p2, canvasLocation.X, canvasLocation.Y, cols * tileSize, rows * tileSize);

            if (uiState.ShowGrid == true)
            {
                for (int i = 0; i < rows; i++)
                {
                    g.DrawLine(p2, canvasLocation.X, (i * tileSize) + canvasLocation.Y, layerWidth + canvasLocation.X, (i * tileSize) + canvasLocation.Y);
                }

                for (int i = 0; i < cols; i++)
                {
                    g.DrawLine(p2, (i * tileSize) + canvasLocation.X, canvasLocation.Y, (i * tileSize) + canvasLocation.X, layerHeight + canvasLocation.Y);
                }
            }
        }
        
        private void renderHudElementCursor(Graphics g)
        {
            if (showCursor_ == false)
            {
                return;
            }

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();

            int selectedHudElementIndex = uiState.SelectedHudElementIndex;
            Guid selectedHudElementId = uiState.SelectedHudElementId;

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int cameraX = uiState.CameraLocation[selectedRoomId].X;
            int cameraY = uiState.CameraLocation[selectedRoomId].Y;

            Point viewportLocation = new Point(0, 0);
            viewportLocation = translateWorldToMap(new Point(cameraX, cameraY));
            viewportLocation = translateToCanvas(viewportLocation);

            int interactiveLayerIndex = projectController_.GetInteractiveLayerIndex(selectedRoomIndex);
            int interactiveLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, interactiveLayerIndex);

            // Need to get the layer offset so the mouse position is done correctly.
            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(interactiveLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            Point cursorLocation = new Point(mouseCursorX_ + canvasLocation.X, mouseCursorY_ + canvasLocation.Y);

            // Render the currently selected hud element, if there is one.
            if (selectedHudElementIndex > -1)
            {
                // For now, just render whatever the first state is.
                HudElementDto selectedHudElement = project.HudElements[selectedHudElementIndex];

                if (project.States[selectedHudElementId].Count > 0)
                {
                    Guid initialStateId = selectedHudElement.InitialStateId;

                    if (initialStateId != Guid.Empty)
                    {
                        int animationSlotCount = project.AnimationSlots[initialStateId].Count;

                        for (int i = 0; i < animationSlotCount; i++)
                        {
                            // Get the index of the animation.
                            Guid animationId = project.AnimationSlots[initialStateId][i].Animation;

                            AnimationDto animation = projectController_.GetAnimation(animationId);

                            if (animation != null)
                            {
                                Guid spriteSheetId = animation.SpriteSheet;

                                if (spriteSheetId != Guid.Empty)
                                {
                                    int sheetIndex = projectController_.GetSpriteSheetIndexFromId(spriteSheetId);

                                    if (sheetIndex > -1)
                                    {
                                        SpriteSheetDto spriteSheet = project.SpriteSheets[sheetIndex];

                                        Guid resourceId = spriteSheet.BitmapResourceId;

                                        // Separate resources dto removed in 2.2 format.
                                        //BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];
                                        BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(resourceId, true);

                                        bitmapResource.LoadedModules ^= (byte)EditorModule.Cursor;

                                        if (project.Frames[animationId].Count > 0)
                                        {
                                            if (project.Frames[animationId][0].SheetCellIndex.HasValue == true)
                                            {
                                                int cellIndex = project.Frames[animationId][0].SheetCellIndex.Value;

                                                if (cellIndex > -1)
                                                {
                                                    float[][] ptsArray ={ new float[] {1, 0, 0, 0,    0},
                                                                          new float[] {0, 1, 0, 0,    0},
                                                                          new float[] {0, 0, 1, 0,    0},
                                                                          new float[] {0, 0, 0, 0.7f, 0},
                                                                          new float[] {0, 0, 0, 0,    1}};

                                                    ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                                                    ImageAttributes imgAttributes = new ImageAttributes();
                                                    imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                                                    int cellWidth = (int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor);
                                                    int cellHeight = (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor);

                                                    Bitmap bmpTemp = new Bitmap(cellWidth, cellHeight);
                                                    Graphics gTmp = Graphics.FromImage(bmpTemp);

                                                    for (int j = 0; j < bitmapResource.SpriteSheetImageList.ImageListRows; j++)
                                                    {
                                                        for (int k = 0; k < bitmapResource.SpriteSheetImageList.ImageListCols; k++)
                                                        {
                                                            int subCellX = k * Globals.maxImageListWidth;
                                                            int subCellY = j * Globals.maxImageListWidth;

                                                            bitmapResource.SpriteSheetImageList.ImageLists[j][k].Draw(gTmp, subCellX, subCellY, cellIndex);
                                                        }
                                                    }

                                                    // Determine the animation position based on the stage origin point and the animation slot origin point.

                                                    // Half of the size of the stage, used for centering.
                                                    Size stageHalfSize = new Size(selectedHudElement.StageWidth / 2, selectedHudElement.StageHeight / 2);

                                                    Point2D stageOriginPointInNativeStageSpace = new Point2D(0, 0);

                                                    switch (selectedHudElement.StageOriginLocation)
                                                    {
                                                        case OriginLocation.TopLeft:

                                                            // The native stage origin is top left, so no change is necessary.

                                                            stageOriginPointInNativeStageSpace.X = 0;

                                                            stageOriginPointInNativeStageSpace.Y = 0;

                                                            break;

                                                        case OriginLocation.TopMiddle:

                                                            stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                                                            stageOriginPointInNativeStageSpace.Y = 0;

                                                            break;

                                                        case OriginLocation.TopRight:

                                                            stageOriginPointInNativeStageSpace.X = selectedHudElement.StageWidth;

                                                            stageOriginPointInNativeStageSpace.Y = 0;

                                                            break;

                                                        case OriginLocation.MiddleLeft:

                                                            // Calculate the center point of the stage in the native stage space.

                                                            stageOriginPointInNativeStageSpace.X = 0;

                                                            stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                                                            break;

                                                        case OriginLocation.Center:

                                                            // Calculate the center point of the stage in the native stage space.

                                                            stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                                                            stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                                                            break;

                                                        case OriginLocation.MiddleRight:

                                                            // Calculate the center point of the stage in the native stage space.

                                                            stageOriginPointInNativeStageSpace.X = selectedHudElement.StageWidth;

                                                            stageOriginPointInNativeStageSpace.Y = stageHalfSize.Height;

                                                            break;

                                                        case OriginLocation.BottomLeft:

                                                            stageOriginPointInNativeStageSpace.X = 0;

                                                            stageOriginPointInNativeStageSpace.Y = selectedHudElement.StageHeight;

                                                            break;

                                                        case OriginLocation.BottomMiddle:

                                                            stageOriginPointInNativeStageSpace.X = stageHalfSize.Width;

                                                            stageOriginPointInNativeStageSpace.Y = selectedHudElement.StageHeight;

                                                            break;

                                                        case OriginLocation.BottomRight:

                                                            stageOriginPointInNativeStageSpace.X = selectedHudElement.StageWidth;

                                                            stageOriginPointInNativeStageSpace.Y = selectedHudElement.StageHeight;

                                                            break;
                                                    }
                                                    
                                                    int animationFrameXInStageSpace = 0;

                                                    int animationFrameYInStageSpace = 0;

                                                    Size animationFrameSize = new Size((int)(spriteSheet.CellWidth * spriteSheet.ScaleFactor), (int)(spriteSheet.CellHeight * spriteSheet.ScaleFactor));

                                                    Size animationFrameHalfSize = new Size((int)(animationFrameSize.Width / 2), (int)(animationFrameSize.Height / 2));

                                                    // Native animation space uses the default origin of the top left of the animation frame.
                                                    Point2D animationOriginPointInNativeAnimationSpace = new Point2D(0, 0);

                                                    switch (project.AnimationSlots[initialStateId][i].OriginLocation)
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

                                                            animationOriginPointInNativeAnimationSpace.X = animationFrameHalfSize.Width;

                                                            animationOriginPointInNativeAnimationSpace.Y = animationFrameSize.Width;

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

                                                    switch (selectedHudElement.StageOriginLocation)
                                                    {
                                                        case OriginLocation.TopLeft:

                                                            // The default stage origin is top left, so we can just use the animation slot position without any coordinate space transformation.

                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;

                                                        case OriginLocation.TopMiddle:

                                                            // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                            // Y-axis is unchanged.
                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;

                                                        case OriginLocation.TopRight:

                                                            // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                            // Y-axis is unchanged.
                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + selectedHudElement.StageWidth - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;

                                                        case OriginLocation.MiddleLeft:

                                                            // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;

                                                        case OriginLocation.Center:

                                                            // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;

                                                        case OriginLocation.MiddleRight:

                                                            // Do a coordinate space transformation by adding half the stage size to get to the center, and then subtract half of the frame size.
                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + selectedHudElement.StageWidth - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + stageHalfSize.Height - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;

                                                        case OriginLocation.BottomLeft:

                                                            // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                            // Y-axis is unchanged.
                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + selectedHudElement.StageHeight - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;

                                                        case OriginLocation.BottomMiddle:

                                                            // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                            // Y-axis is unchanged.
                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + stageHalfSize.Width - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + selectedHudElement.StageHeight - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;

                                                        case OriginLocation.BottomRight:

                                                            // Do a coordinate space transformation by adding half the stage size to get to the center for the x axis, and then subtract half of the frame size.
                                                            // Y-axis is unchanged.
                                                            animationFrameXInStageSpace = project.AnimationSlots[initialStateId][i].Position.X + selectedHudElement.StageWidth - animationOriginPointInNativeAnimationSpace.X;

                                                            animationFrameYInStageSpace = project.AnimationSlots[initialStateId][i].Position.Y + selectedHudElement.StageHeight - animationOriginPointInNativeAnimationSpace.Y;

                                                            break;
                                                    }
                                                    
                                                    int x = cursorLocation.X + animationFrameXInStageSpace;
                                                    int y = cursorLocation.Y + animationFrameYInStageSpace;

                                                    System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, bmpTemp.Width, bmpTemp.Height);

                                                    g.DrawImage(bmpTemp, destRect, 0, 0, bmpTemp.Width, bmpTemp.Height, GraphicsUnit.Pixel, imgAttributes);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private void renderHudElementCursorOutline(Graphics g)
        {
            if (showCursor_ == false)
            {
                return;
            }

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedHudElementIndex = uiState.SelectedHudElementIndex;
            Guid selectedHudElementId = uiState.SelectedHudElementId;

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int cameraX = uiState.CameraLocation[selectedRoomId].X;
            int cameraY = uiState.CameraLocation[selectedRoomId].Y;

            Point viewportLocation = new Point(0, 0);
            viewportLocation = translateWorldToMap(new Point(cameraX, cameraY));
            viewportLocation = translateToCanvas(viewportLocation);

            int interactiveLayerIndex = projectController_.GetInteractiveLayerIndex(selectedRoomIndex);
            int interactiveLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, interactiveLayerIndex);

            // Need to get the layer offset so the mouse position is done correctly.
            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(interactiveLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            Point cursorLocation = new Point(mouseCursorX_ + canvasLocation.X, mouseCursorY_ + canvasLocation.Y);

            // Render the currently selected hud element, if there is one.
            if (selectedHudElementIndex > -1)
            {
                // For now, just render whatever the first state is.
                HudElementDto selectedHudElement = project.HudElements[selectedHudElementIndex];

                if (uiState.ShowOutlines == true)
                {
                    int x = cursorLocation.X;
                    int y = cursorLocation.Y;

                    int w = selectedHudElement.StageWidth;
                    int h = selectedHudElement.StageHeight;

                    Color c = Globals.actorOutlineColor;

                    g.DrawRectangle(new Pen(new SolidBrush(c)), x, y, w, h);
                }
            }
        }

        private void renderMapWidgets(Graphics g, int layerOrdinal)
        {
            Dictionary<Guid, bool> dictRenderedList = new Dictionary<Guid, bool>();

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int tileSize = project.TileSize;

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;
            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            int dragDeltaX = 0;
            int dragDeltaY = 0;

            int dragSquareDeltaX = 0;
            int dragSquareDeltaY = 0;

            if (isDraggingMapWidgetSelection_ == true)
            {
                dragDeltaX = mouseCursorX_ - mapWidgetDragStart_.X;
                dragDeltaY = mouseCursorY_ - mapWidgetDragStart_.Y;

                dragSquareDeltaX = squareMouseOverX_ - mapWidgetDragSquareStart_.X;
                dragSquareDeltaY = squareMouseOverY_ - mapWidgetDragSquareStart_.Y;
            }

            if (layerOrdinal < 0)
            {
                // Render HUD element widgets.
                foreach (HudElementWidgetDto hudElement in project.MapWidgets[MapWidgetType.HudElement].Values)
                {
                    if (hudElement.RootOwnerId == selectedRoomId)
                    {                                
                        // If the widget is selected and being dragged, render it with an offset 
                        // of the current mouse location minus the location it was at when the drag started.
                        Point2D selectionOffset = new Point2D(0, 0);
                        Point2D keyMoveOffset = new Point2D(0, 0);

                        // The paint may occur in the middle of a delete, so it's possible this value may have been removed.
                        bool isSelected = false;

                        if (uiState.MapWidgetSelected.ContainsKey(hudElement.Id) == true)
                        {
                            isSelected = uiState.MapWidgetSelected[hudElement.Id];

                            if (isSelected == true)
                            {
                                selectionOffset.X = dragDeltaX;
                                selectionOffset.Y = dragDeltaY;
                               
                                keyMoveOffset.X = keyboardDragOffset_.X;
                                keyMoveOffset.Y = keyboardDragOffset_.Y;
                            }
                        }

                        int x = hudElement.BoundingBox.Left + keyMoveOffset.X + selectionOffset.X;
                        int y = hudElement.BoundingBox.Top + keyMoveOffset.Y + selectionOffset.Y;

                        hudElement.Controller.Render(g, x, y);
                    }
                }
            }
            else
            {
                int layerIndex = projectController_.GetLayerIndexFromOrdinal(selectedRoomIndex, layerOrdinal);

                LayerDto layer = projectController_.GetLayerByIndex(selectedRoomIndex, layerIndex);

                // Get the location of the layer in canvas space.
                Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(layerOrdinal);
                Point mapLocation = translateWorldToMap(worldLocation);
                Point canvasLocation = translateToCanvas(mapLocation);

                int layerStartTileX = 0;
                int layerStartTileY = 0;

                int layerVisibleX = 0;
                int layerVisibleY = 0;

                getVisibleTileRange(layerOrdinal, ref layerStartTileX, ref layerStartTileY, ref layerVisibleX, ref layerVisibleY);

                int layerActiveStartTileX = 0;
                int layerActiveStartTileY = 0;
                int layerActiveVisibleX = 0;
                int layerActiveVisibleY = 0;

                if (layerOrdinal == selectedLayerOrdinal)
                {
                    layerActiveStartTileX = layerStartTileX;
                    layerActiveStartTileY = layerStartTileY;

                    layerActiveVisibleX = layerVisibleX;
                    layerActiveVisibleY = layerVisibleY;
                }

                for (int i = layerStartTileY; i < layerStartTileY + layerVisibleY; i++)
                {
                    for (int j = layerStartTileX; j < layerStartTileX + layerVisibleX; j++)
                    {
                        for (int n = 0; n < layer.MapWidgetIds[i][j].Count; n++)
                        {
                            Guid currentMapWidgetInstanceId = layer.MapWidgetIds[i][j][n];

                            if (dictRenderedList.ContainsKey(currentMapWidgetInstanceId) == false)
                            {
                                dictRenderedList.Add(currentMapWidgetInstanceId, true);

                                MapWidgetDto currentMapWidget = projectController_.GetMapWidget(currentMapWidgetInstanceId);

                                // If the widget is selected and being dragged, render it with an offset 
                                // of the current mouse location minus the location it was at when the drag started.
                                Point2D selectionOffset = new Point2D(0, 0);
                                Point2D keyMoveOffset = new Point2D(0, 0);

                                // The paint may occur in the middle of a delete, so it's possible this value may have been removed.
                                bool isSelected = false;

                                if (uiState.MapWidgetSelected.ContainsKey(currentMapWidgetInstanceId) == true)
                                {
                                    isSelected = uiState.MapWidgetSelected[currentMapWidgetInstanceId];

                                    if (isSelected == true)
                                    {
                                        if (currentMapWidget.Controller.GridAligned == false)
                                        {
                                            selectionOffset.X = dragDeltaX;
                                            selectionOffset.Y = dragDeltaY;
                                        }
                                        else
                                        {
                                            selectionOffset.X = dragSquareDeltaX * project.TileSize;
                                            selectionOffset.Y = dragSquareDeltaY * project.TileSize;
                                        }

                                        keyMoveOffset.X = keyboardDragOffset_.X;
                                        keyMoveOffset.Y = keyboardDragOffset_.Y;
                                    }
                                }

                                int layerOffsetX = canvasLocation.X + keyMoveOffset.X + selectionOffset.X;
                                int layerOffsetY = canvasLocation.Y + keyMoveOffset.Y + selectionOffset.Y;

                                if (isResizingMapWidget_ == true && isSelected == true)
                                {
                                    // Render the "display only" version of the widget.
                                    if (currentMapWidget is WorldGeometryWidgetDto)
                                    {
                                        int x = displayOnlyWorldGeometryChunk_.BoundingBox.Left + layerOffsetX;
                                        int y = displayOnlyWorldGeometryChunk_.BoundingBox.Top + layerOffsetY;

                                        displayOnlyWorldGeometryChunk_.Controller.Render(g, x, y);
                                    }
                                    else if (currentMapWidget is EventWidgetDto)
                                    {
                                        int x = displayOnlyEvent_.BoundingBox.Left + layerOffsetX;
                                        int y = displayOnlyEvent_.BoundingBox.Top + layerOffsetY;

                                        displayOnlyEvent_.Controller.Render(g, x, y);
                                    }
                                }
                                else
                                {
                                    int x = currentMapWidget.BoundingBox.Left + layerOffsetX;
                                    int y = currentMapWidget.BoundingBox.Top + layerOffsetY;

                                    currentMapWidget.Controller.Render(g, x, y);
                                }
                            }
                        }
                    }
                }
            }
        }
                
        private void renderMapWidgetOutlines(Graphics g)
        {
            Dictionary<Guid, bool> dictRenderedList = new Dictionary<Guid, bool>();

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();
            
            int tileSize = project.TileSize;

            Guid selectedRoomId = uiState.SelectedRoomId;
            int selectedRoomIndex = uiState.SelectedRoomIndex;
            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            int layerIndex = projectController_.GetLayerIndexFromOrdinal(selectedRoomIndex, selectedLayerOrdinal);
            
            LayerDto layer = projectController_.GetLayerByIndex(selectedRoomIndex, layerIndex);

            MapWidgetSelectorDto selector = uiState.MapWidgetSelector[selectedRoomId];

            bool isSingularSelection = false;

            if (selector.SelectedMapWidgetIds.Count == 1)
            {
                isSingularSelection = true;
            }

            // Get the location of the layer in canvas space.
            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            int dragDeltaX = 0;
            int dragDeltaY = 0;

            int dragSquareDeltaX = 0;
            int dragSquareDeltaY = 0;

            if (isDraggingMapWidgetSelection_ == true)
            {
                dragDeltaX = mouseCursorX_ - mapWidgetDragStart_.X;
                dragDeltaY = mouseCursorY_ - mapWidgetDragStart_.Y;

                dragSquareDeltaX = squareMouseOverX_ - mapWidgetDragSquareStart_.X;
                dragSquareDeltaY = squareMouseOverY_ - mapWidgetDragSquareStart_.Y;
            }

            int layerStartTileX = 0;
            int layerStartTileY = 0;

            int layerVisibleX = 0;
            int layerVisibleY = 0;

            getVisibleTileRange(selectedLayerOrdinal, ref layerStartTileX, ref layerStartTileY, ref layerVisibleX, ref layerVisibleY);

            for (int i = layerStartTileY; i < layerStartTileY + layerVisibleY; i++)
            {
                for (int j = layerStartTileX; j < layerStartTileX + layerVisibleX; j++)
                {
                    for (int n = 0; n < layer.MapWidgetIds[i][j].Count; n++)
                    {
                        Guid currentMapWidgetId = layer.MapWidgetIds[i][j][n];

                        if (dictRenderedList.ContainsKey(currentMapWidgetId) == false)
                        {
                            dictRenderedList.Add(currentMapWidgetId, true);

                            // If the actor instance is selected and being dragged, render it with an offset 
                            // of the current mouse location minus the location it was at when the drag started.
                            Point2D selectionOffset = new Point2D(0, 0);
                            Point2D keyMoveOffset = new Point2D(0, 0);

                            bool isInstanceSelected = false;

                            if (uiState.MapWidgetSelected.ContainsKey(currentMapWidgetId) == true)
                            {
                                isInstanceSelected = uiState.MapWidgetSelected[currentMapWidgetId];
                            }

                            MapWidgetDto currentMapWidgetInstance = projectController_.GetMapWidget(currentMapWidgetId);

                            if (isInstanceSelected == true)
                            {
                                if (currentMapWidgetInstance.Controller.GridAligned == false)
                                {
                                    selectionOffset.X = dragDeltaX;
                                    selectionOffset.Y = dragDeltaY;
                                }
                                else
                                {
                                    selectionOffset.X = dragSquareDeltaX * project.TileSize;
                                    selectionOffset.Y = dragSquareDeltaY * project.TileSize;
                                }

                                keyMoveOffset.X = keyboardDragOffset_.X;
                                keyMoveOffset.Y = keyboardDragOffset_.Y;
                            }

                            if (isResizingMapWidget_ == true && isInstanceSelected == true)
                            {
                                MapWidgetDto mouseOverMapWidget = projectController_.GetMapWidget(mouseOverMapWidgetId_);

                                // Render the "display only" version of the widget.
                                if (mouseOverMapWidget is WorldGeometryWidgetDto)
                                {
                                    int x = canvasLocation.X + keyMoveOffset.X + selectionOffset.X;
                                    int y = canvasLocation.Y + keyMoveOffset.Y + selectionOffset.Y;
                                    
                                    displayOnlyWorldGeometryChunk_.Controller.RenderOverlay(g, new Point(x, y), isInstanceSelected, isSingularSelection, uiState.ShowOutlines);
                                }
                                else if (mouseOverMapWidget is EventWidgetDto)
                                {
                                    int x = canvasLocation.X + keyMoveOffset.X + selectionOffset.X;
                                    int y = canvasLocation.Y + keyMoveOffset.Y + selectionOffset.Y;
                                    
                                    displayOnlyEvent_.Controller.RenderOverlay(g, new Point(x, y), isInstanceSelected, isSingularSelection, uiState.ShowOutlines);
                                }
                            }
                            else
                            {
                                int x = canvasLocation.X + keyMoveOffset.X + selectionOffset.X;
                                int y = canvasLocation.Y + keyMoveOffset.Y + selectionOffset.Y;
                                
                                currentMapWidgetInstance.Controller.RenderOverlay(g, new Point(x, y), isInstanceSelected, isSingularSelection, uiState.ShowOutlines);
                            }
                        }
                    }
                }
            }

            // Render HUD element widgets outlines no matter what.
            foreach (HudElementWidgetDto hudElement in project.MapWidgets[MapWidgetType.HudElement].Values)
            {
                if (hudElement.RootOwnerId == selectedRoomId)
                {
                    // If the actor instance is selected and being dragged, render it with an offset 
                    // of the current mouse location minus the location it was at when the drag started.
                    Point2D selectionOffset = new Point2D(0, 0);
                    Point2D keyMoveOffset = new Point2D(0, 0);

                    bool isInstanceSelected = false;

                    if (uiState.MapWidgetSelected.ContainsKey(hudElement.Id) == true)
                    {
                        isInstanceSelected = uiState.MapWidgetSelected[hudElement.Id];
                    }

                    MapWidgetDto currentMapWidgetInstance = projectController_.GetMapWidget(hudElement.Id);

                    if (isInstanceSelected == true)
                    {
                        selectionOffset.X = dragDeltaX;
                        selectionOffset.Y = dragDeltaY;
                       
                        keyMoveOffset.X = keyboardDragOffset_.X;
                        keyMoveOffset.Y = keyboardDragOffset_.Y;
                    }

                    int x = hudElement.BoundingBox.Left + keyMoveOffset.X + selectionOffset.X;
                    int y = hudElement.BoundingBox.Top + keyMoveOffset.Y + selectionOffset.Y;
                    
                    hudElement.Controller.RenderOverlay(g, new Point(x, y), uiState.MapWidgetSelected[hudElement.Id], isSingularSelection, uiState.ShowOutlines);
                }
            }
        }

        private void renderSelector(Graphics g, MapWidgetSelectorDto selector)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            if (selector.IsSelectionOn == true)
            {
                MapWidgetMode mapWidgetMode = uiState.MapWidgetMode[selectedRoomId];

                System.Drawing.Rectangle selectionRect = selector.DrawableRect;

                string layerName = string.Empty;

                int selectorX = 0;
                int selectorY = 0;

                //if (mapWidgetMode == MapWidgetMode.HudElement)
                //{
                //    layerName = "HUD Elements";

                //    Point rectLocation = new Point(selectionRect.Left, selectionRect.Top);

                //    int cameraX = uiState.CameraLocation[selectedRoomId].X;
                //    int cameraY = uiState.CameraLocation[selectedRoomId].Y;

                //    Point viewportLocation = new Point(0, 0);
                //    viewportLocation = translateWorldToMap(new Point(cameraX, cameraY));
                //    viewportLocation = translateToCanvas(viewportLocation);

                //    int interactiveLayerIndex = projectController_.GetInteractiveLayerIndex(selectedRoomIndex);
                //    int interactiveLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, interactiveLayerIndex);

                //    // Need to get the layer offset so the mouse position is done correctly.
                //    Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(interactiveLayerOrdinal);
                //    Point mapLocation = translateWorldToMap(worldLocation);
                //    Point canvasLocation = translateToCanvas(mapLocation);

                //    selectorX = rectLocation.X + viewportLocation.X;
                //    selectorY = rectLocation.Y + viewportLocation.Y;
                //}
                //else
                //{
                    Guid selectorLayerId = selector.SelectedLayerId;

                    if (selectorLayerId != Guid.Empty)
                    {
                        int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                        int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

                        // Get the location of the layer in canvas space.
                        Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
                        Point mapLocation = translateWorldToMap(worldLocation);
                        Point canvasLocation = translateToCanvas(mapLocation);

                        int selectorLayerIndex = projectController_.GetLayerIndexFromId(selectedRoomIndex, selectorLayerId);

                        if (selectorLayerIndex > -1)
                        {
                            int selectorLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectorLayerIndex);

                            LayerDto currentLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectorLayerIndex);

                            layerName = currentLayer.Name;

                            if (uiState.LayerVisible[currentLayer.Id])
                            {
                                // Convert selection rectangle to canvas space.
                                Point rectLocation = new Point(selectionRect.Left, selectionRect.Top);

                                worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectorLayerOrdinal);
                                mapLocation = translateWorldToMap(worldLocation);
                                canvasLocation = translateToCanvas(mapLocation);

                                selectorX = canvasLocation.X + rectLocation.X;
                                selectorY = canvasLocation.Y + rectLocation.Y;
                            }
                        }
                    }
                //}

                Pen p = new Pen(new SolidBrush(selector.OutlineColor), 2.0f);
                Brush b = new SolidBrush(selector.FillColor);

                g.DrawRectangle(p, selectorX, selectorY, selectionRect.Width, selectionRect.Height);

                if (uiState.TransparentSelect == true)
                {
                    g.FillRectangle(b, selectorX, selectorY, selectionRect.Width, selectionRect.Height);
                }

                // Render the name of the layer that the selector is on, to avoid confusion.
                // Don't render if the selector is offscreen though.
                if (selectorX < pbTiles.Width && selectorX + selectionRect.Width > 0 && selectorY < pbTiles.Height && selectorY + selectionRect.Height > 0)
                {
                    int layerNameX = selectorX;

                    if (layerNameX < 0)
                    {
                        layerNameX = 0;
                    }

                    int layerNameY = selectorY;

                    if (layerNameY < 0)
                    {
                        layerNameY = 0;
                    }

                    Font f = new Font(FontFamily.GenericSansSerif, 8.0f);
                    g.DrawString(layerName, f, new SolidBrush(Color.White), (float)layerNameX, (float)layerNameY);
                }
            }
        }
        
        private void renderWorldGeometryCursor(Graphics g)
        {
            if (showCursor_ == false)
            {
                return;
            }

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();
            
            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;
            
            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
            Point mapLocation = translateWorldToMap(worldLocation);
            Point canvasLocation = translateToCanvas(mapLocation);

            if (isDrawingNewWorldGeometry_ == true)
            {
                int cell1X = displayOnlyWorldGeometryChunk_.Corner1.X * project.TileSize;
                int cell1Y = displayOnlyWorldGeometryChunk_.Corner1.Y * project.TileSize;

                int cell2X = displayOnlyWorldGeometryChunk_.Corner2.X * project.TileSize;
                int cell2Y = displayOnlyWorldGeometryChunk_.Corner2.Y * project.TileSize;

                Point tempWorldGeometryPoint1 = new Point(cell1X + canvasLocation.X, cell1Y + canvasLocation.Y);
                Point tempWorldGeometryPoint2 = new Point(cell2X + canvasLocation.X, cell2Y + canvasLocation.Y);

                Point2D p1 = new Point2D(tempWorldGeometryPoint1.X, tempWorldGeometryPoint1.Y);
                Point2D p2 = new Point2D(tempWorldGeometryPoint2.X, tempWorldGeometryPoint2.Y);

                System.Drawing.Rectangle rect = drawingUtility_.GetRectFromPoints(p1, p2);
                
                int x = rect.Left;
                int y =  rect.Top;

                displayOnlyWorldGeometryChunk_.Controller.Render(g, x, y);

                //g.DrawRectangle(Globals.pWorldGeometryCursorOutline, rect.Left, rect.Top, rect.Width + project.TileSize, rect.Height + project.TileSize);

                //g.FillRectangle(new SolidBrush(Color.Black), rect.Left, rect.Top, rect.Width + project.TileSize, rect.Height + project.TileSize);
            }

            Point cursorLocation = new Point(mouseCursorX_ + canvasLocation.X, mouseCursorY_ + canvasLocation.Y);

            worldGeometryCursor_.Render(g, cursorLocation.X, cursorLocation.Y, Math.Abs(canvasLocation.X) % project.TileSize * -1, Math.Abs(canvasLocation.Y) % project.TileSize * -1);
        }

        private void resetCursor()
        {
            // This method is needed because after adding a tile, or creating a new map, the current tile flashes in the position it was
            // before the mouse was moved from over the map to over the toolbar/menu. This resets the mouse position to off the map.
            squareMouseOverX_ = -100;
            squareMouseOverY_ = -100;
        }

        private void resizeControls()
        {
            // Position the scroll bars.
            hsCanvasOffset.Top = this.Height - hsCanvasOffset.Height;
            vsCanvasOffset.Left = this.Width - vsCanvasOffset.Width;

            hsCanvasOffset.Width = (this.Width - vsCameraPosition.Width) / 2;
            vsCanvasOffset.Height = (this.Height - hsCameraPosition.Height) / 2;

            vsCameraPosition.Visible = true;
            hsCameraPosition.Visible = true;

            hsCameraPosition.Top = this.Height - hsCameraPosition.Height;
            hsCameraPosition.Left = hsCanvasOffset.Right;
            vsCameraPosition.Top = vsCanvasOffset.Bottom;
            vsCameraPosition.Left = this.Width - vsCameraPosition.Width;

            hsCameraPosition.Width = hsCanvasOffset.Width;

            if (this.Width % 2 == 1)
            {
                hsCameraPosition.Width++;
            }
            
            vsCameraPosition.Height = vsCanvasOffset.Height;

            if (this.Height % 2 == 1)
            {
                vsCameraPosition.Height++;
            }

            pnlCorner.Left = hsCameraPosition.Right;
            pnlCorner.Top = vsCameraPosition.Bottom;

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int placementRegionWidth = this.ClientSize.Width - vsCanvasOffset.Width;
            int placementRegionHeight = this.ClientSize.Height - hsCanvasOffset.Height;

            // Default is to fill the whole canvas placement region.
            int canvasLeft = 0;
            int canvasTop = 0;
            int canvasWidth = placementRegionWidth;
            int canvasHeight = placementRegionHeight;
            
            pbTiles.Left = canvasLeft;
            pbTiles.Top = canvasTop;
            pbTiles.Width = canvasWidth;
            pbTiles.Height = canvasHeight;

            if (project != null && project.IsPrepared == true)
            {
                if (uiState.CameraMode == CameraMode.CameraLocked)
                {
                    // Center the canvas in the region.
                    canvasWidth = project.CameraWidth;
                    canvasHeight = project.CameraHeight;

                    canvasLeft = (placementRegionWidth / 2) - (project.CameraWidth / 2);
                    canvasTop = (placementRegionHeight / 2) - (canvasHeight / 2);


                    pbTiles.Left = canvasLeft;
                    pbTiles.Top = canvasTop;
                    pbTiles.Width = canvasWidth;
                    pbTiles.Height = canvasHeight;
                }
                
                int selectedRoomIndex = uiState.SelectedRoomIndex;
                int tileSize = project.TileSize;

                for (int i = 0; i < project.Rooms.Count; i++)
                {
                    Guid roomId = project.Rooms[i].Id;

                    int maxCols = uiState.MaxCols[roomId];
                    int maxRows = uiState.MaxRows[roomId];

                    int widthWithOffset, heightWithOffset;
                    int layerWidth, layerHeight;

                    try
                    {
                        widthWithOffset = uiState.CanvasOffset[roomId].X + pbTiles.Width;
                        heightWithOffset = uiState.CanvasOffset[roomId].Y + pbTiles.Height;

                        layerWidth = maxCols * tileSize;
                        layerHeight = maxRows * tileSize;

                        // If the map is being resized and the space that is being revealed on the right extends beyond the border of the map, move 
                        // the offset to give the effect that the layer is being "pulled" outward, revealing the portion of the map that would have
                        // been extending beyond the lefthand border.
                        if (widthWithOffset > layerWidth)
                        {
                            if (i == selectedRoomIndex)
                            {
                                if (uiState.CanvasOffset[roomId].X - (widthWithOffset - layerWidth) < 0)
                                {
                                    hsCanvasOffset.Value = hsCanvasOffset.Minimum;
                                }
                                else
                                {
                                    hsCanvasOffset.Value -= widthWithOffset - layerWidth;
                                }

                                uiState.CanvasOffset[roomId].X = hsCanvasOffset.Value - hsCanvasOffset.Minimum;
                            }
                            else
                            {
                                if (uiState.CanvasOffset[roomId].X - (widthWithOffset - layerWidth) < 0)
                                {
                                    uiState.CanvasOffset[roomId].X = hsCanvasOffset.Minimum;
                                }
                                else
                                {
                                    uiState.CanvasOffset[roomId].X -= (widthWithOffset - layerWidth);
                                }

                                uiState.CanvasOffset[roomId].X -= hsCanvasOffset.Minimum;
                            }
                        }

                        if (heightWithOffset > layerHeight)
                        {
                            if (i == selectedRoomIndex)
                            {
                                if (uiState.CanvasOffset[roomId].Y - (heightWithOffset - layerHeight) < 0)
                                {
                                    vsCanvasOffset.Value = vsCanvasOffset.Minimum;
                                }
                                else
                                {
                                    vsCanvasOffset.Value -= heightWithOffset - layerHeight;
                                }

                                uiState.CanvasOffset[roomId].Y = vsCanvasOffset.Value - vsCanvasOffset.Minimum;
                            }
                            else
                            {
                                if (uiState.CanvasOffset[roomId].Y - (heightWithOffset - layerHeight) < 0)
                                {
                                    uiState.CanvasOffset[roomId].Y = vsCanvasOffset.Minimum;
                                }
                                else
                                {
                                    uiState.CanvasOffset[roomId].Y -= (heightWithOffset - layerHeight);
                                }

                                uiState.CanvasOffset[roomId].Y -= vsCanvasOffset.Minimum;
                            }
                        }

                        // Change the maximum value of the scrollbar if more of the map is being hidden.
                        int newVMax, newHMax;

                        if (layerHeight <= pbTiles.Height)
                        {
                            newVMax = vsCanvasOffset.Minimum;
                        }
                        else
                        {
                            newVMax = (layerHeight - pbTiles.Height) + 1;
                        }

                        if (layerWidth <= pbTiles.Width)
                        {
                            newHMax = hsCanvasOffset.Minimum;
                        }
                        else
                        {
                            newHMax = (layerWidth - pbTiles.Width) + 1;
                        }

                        uiState.CanvasOffsetMax[roomId].X = newHMax;
                        uiState.CanvasOffsetMax[roomId].Y = newVMax;

                        if (i == selectedRoomIndex)
                        {
                            vsCanvasOffset.Maximum = newVMax;
                            hsCanvasOffset.Maximum = newHMax;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            
            // If the canvas is sized beyond the current background buffer region, 
            // it will need to be regenerated.
            if ((pbTiles.Height > (szBounds_.Height + szBuffer_.Height) || pbTiles.Width > (szBounds_.Width + szBuffer_.Width)) ||
                (pbTiles.Height < (szBounds_.Height - szBuffer_.Height) || pbTiles.Width < (szBounds_.Width - szBuffer_.Width)))
            {
                backgroundGenerator_.Regenerate();
            }

            pbTiles.Refresh();  
        }
        
        private Point layerPositionInWorldSpaceRelativeToCamera(int layerOrdinal)
        {
            // Given a camera position in world space, return the given layer's worldspace position.
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int tileSize = project.TileSize;
            int cameraWidth = project.CameraWidth;
            int cameraHeight = project.CameraHeight;

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            Point worldSpace = new Point();

            // Gather the layer data necessary to calculate the layer position.
            LayerDto layer = projectController_.GetLayerByOrdinal(selectedRoomIndex, layerOrdinal);

            LayerDto interactiveLayer = projectController_.GetInteractiveLayer(selectedRoomIndex);

            int interactiveLayerWidth = interactiveLayer.Cols * tileSize;
            int interactiveLayerHeight = interactiveLayer.Rows * tileSize;

            // Calculate the position of this layer in parallax, using the size of this layer vs the size of the interactive layer.
            if (interactiveLayerWidth == cameraWidth)
            {
                // If this layer is the same size as the camera, it's always going to located at coordinate 0 in world space.
                // Need to set this explicitly, otherwise it would result in a divide by zero error.
                worldSpace.X = 0;
            }
            else
            {
                // How much bigger or smaller this layer is than the interactive layer
                double temp1 = interactiveLayerWidth - (layer.Cols * tileSize);

                // How much bigger the interactive layer is than the camera.
                double temp2 = interactiveLayerWidth - cameraWidth;

                double scalingFactor = temp1 / temp2;

                worldSpace.X = Convert.ToInt32(scalingFactor * Convert.ToDouble(uiState.CameraLocation[selectedRoomId].X));
            }

            if (interactiveLayerHeight == cameraHeight)
            {
                worldSpace.Y = 0;
            }
            else
            {
                double temp1 = interactiveLayerHeight - (layer.Rows * tileSize);
                double temp2 = interactiveLayerHeight - cameraHeight;
                double scalingFactor = temp1 / temp2;

                worldSpace.Y = Convert.ToInt32(scalingFactor * Convert.ToDouble(uiState.CameraLocation[selectedRoomId].Y));
            }
            
            return worldSpace;
        }

        private void setCursor()
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            // Raise an event to change the mouse back to what it should be.
            switch (uiState.EditMode)
            {
                case EditMode.Draw:
                    OnCursorChanged(new CursorChangedEventArgs(Cursors.Arrow));
                    break;
                case EditMode.Selection:
                    OnCursorChanged(new CursorChangedEventArgs(Cursors.Cross));
                    break;
                case EditMode.Grab:
                    OnCursorChanged(new CursorChangedEventArgs(Cursors.SizeAll));
                    break;
            }
        }
        
        private Point translateMapToWorld(Point source)
        {
            // Takes a worldspace point as input. Subtract the world space of the largest layer rows/cols to get the map space coordinate.
            Point worldSpace = new Point(0, 0);
            Point largestLayerWorldspace = new Point(0, 0);

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int tileSize = project.TileSize;
            int cameraWidth = project.CameraWidth;
            int cameraHeight = project.CameraHeight;

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int interactiveLayerIndex = projectController_.GetInteractiveLayerIndex(selectedRoomIndex);

            LayerDto interactiveLayer = projectController_.GetLayerByIndex(selectedRoomIndex, interactiveLayerIndex);

            int interactiveLayerWidth = interactiveLayer.Cols * tileSize;
            int interactiveLayerHeight = interactiveLayer.Rows * tileSize;

            // Max cols returns the number of columns in the widest layer for the given room.
            int maxCols = uiState.MaxCols[selectedRoomId];
            int widestLayerWidth = maxCols * tileSize;

            // Max rows returns the number of rows in the tallest layer for the given room.
            int maxRows = uiState.MaxRows[selectedRoomId];
            int highestLayerHeight = maxRows * tileSize;

            // Calculate the position of this layer in parallax, using the size of this layer vs the size of the largest layer.
            if (interactiveLayerWidth == project.CameraWidth)
            {
                largestLayerWorldspace.X = 0;
            }
            else
            {
                // The size of the x dimension of the widest layer that is not overlapped by the interactive layer.
                double temp1 = Convert.ToDouble(interactiveLayerWidth - widestLayerWidth);
                
                // The size of the x dimension of the interactive layer that is not occupied by the camera.
                double temp2 = Convert.ToDouble(interactiveLayerWidth - cameraWidth);

                double cameraX = Convert.ToDouble(uiState.CameraLocation[selectedRoomId].X);

                largestLayerWorldspace.X = Convert.ToInt32((temp1 / temp2) * cameraX);
            }

            if (interactiveLayerHeight == project.CameraHeight)
            {
                largestLayerWorldspace.Y = 0;
            }
            else
            {
                double temp1 = Convert.ToDouble(interactiveLayerHeight - highestLayerHeight);
                double temp2 = Convert.ToDouble(interactiveLayerHeight - cameraHeight);
                double cameraY = Convert.ToDouble(uiState.CameraLocation[selectedRoomId].Y);

                largestLayerWorldspace.Y = Convert.ToInt32((temp1 / temp2) * cameraY);
            }

            worldSpace.X = source.X + largestLayerWorldspace.X;
            worldSpace.Y = source.Y + largestLayerWorldspace.Y;

            return worldSpace;
        }

        private Point translateToCanvas(Point source)
        {
            //ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;

            // This method takes into account the first scrollbar values that actually scroll the canvas
            // rather than the second scrollbars that move the camera viewport around.
            Point canvasSpace = new Point();

            canvasSpace.X = Math.Abs(source.X) - uiState.CanvasOffset[selectedRoomId].X;
            canvasSpace.Y = Math.Abs(source.Y) - uiState.CanvasOffset[selectedRoomId].Y;

            return canvasSpace;
        }
        
        private Point translateWorldToMap(Point source)
        {
            // Map space refers coordinates relative to the largest layer.

            // Takes a worldspace point as input. 
            // Subtract the world space of the largest layer rows/cols to get the map space coordinate.            
            Point mapSpace = new Point(0, 0);

            Point largestLayerWorldspace = new Point(0, 0);

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int tileSize = project.TileSize;
            int cameraWidth = project.CameraWidth;
            int cameraHeight = project.CameraHeight;

            int maxCols = uiState.MaxCols[selectedRoomId];
            int maxRows = uiState.MaxRows[selectedRoomId];

            LayerDto interactiveLayer = projectController_.GetInteractiveLayer(selectedRoomIndex);

            int layerInteractiveWidth = interactiveLayer.Cols * tileSize;
            int layerInteractiveHeight = interactiveLayer.Rows * tileSize;
               
            // Calculate the position of this layer in parallax, using the size of this layer vs the size of the largest layer.
            if (layerInteractiveWidth == cameraWidth)
            {
                largestLayerWorldspace.X = 0;
            }
            else
            {
                double temp1 = layerInteractiveWidth - (maxCols * tileSize);
                double temp2 = layerInteractiveWidth - cameraWidth;
                double scalingFactor = temp1 / temp2;

                double cameraX = Convert.ToDouble(uiState.CameraLocation[selectedRoomId].X);

                largestLayerWorldspace.X = Convert.ToInt32(scalingFactor * cameraX);
            }

            if (layerInteractiveHeight == cameraHeight)
            {
                largestLayerWorldspace.Y = 0;
            }
            else
            {
                double temp1 = layerInteractiveHeight - (maxRows * tileSize);
                double temp2 = layerInteractiveHeight - cameraHeight;
                double scalingFactor = temp1 / temp2;

                double cameraY = Convert.ToDouble(uiState.CameraLocation[selectedRoomId].Y);

                largestLayerWorldspace.Y = Convert.ToInt32(scalingFactor * cameraY);
            }

            mapSpace.X = source.X - largestLayerWorldspace.X;
            mapSpace.Y = source.Y - largestLayerWorldspace.Y;
            
            return mapSpace;
        }

        #endregion

        #region Event Handlers

        private void hsCameraPosition_Scroll(object sender, ScrollEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int cameraX = e.NewValue - hsCameraPosition.Minimum;
            int cameraY = uiState.CameraLocation[selectedRoomId].Y;

            uiState.CameraLocation[selectedRoomId].X = cameraX;

            switch (uiState.CameraMode)
            {
                case CameraMode.CameraLocked:
                    // Move the canvas offset to line up. Camera map space position minus the canvas offset.
                    Point cameraMap = new Point(0, 0);
                    cameraMap = translateWorldToMap(new Point(cameraX, cameraY));

                    int delta = cameraMap.X - uiState.CanvasOffset[selectedRoomId].X;

                    hsCanvasOffset.Value += delta;

                    uiState.CanvasOffset[selectedRoomId].X = hsCanvasOffset.Value - hsCanvasOffset.Minimum;

                    break;

                case CameraMode.CameraFree:

                    break;

                default:
                    break;
            }

            this.Refresh();
        }        
        
        private void hsCanvasOffset_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;

            uiState.CanvasOffset[selectedRoomId].X = e.NewValue - hsCanvasOffset.Minimum;

            this.Refresh();
        }
        
        private void pbTiles_Click(object sender, EventArgs e)
        {

        }

        private void pbTiles_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseLeftDown_ = true;
                    break;

                case MouseButtons.Right:
                    mouseRightDown_ = true;
                    break;

                case MouseButtons.Middle:
                    mouseMiddleDown_ = true;
                    break;
            }

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;
            
            MapWidgetMode mapWidgetMode = uiState.MapWidgetMode[selectedRoomId];

            if (mouseMiddleDown_ == true)
            {
                    EditMode editMode = uiState.EditMode;

                    editMode++;

                    // This should work, as long as the enumeration never defines more than 1000 items, 
                    // and never defines an item whose name is less than 3 characters long.
                    if (editMode.ToString().Length < 3)
                    {
                        editMode = EditMode.Draw;
                    }

                    projectController_.SetEditMode(editMode);

                    this.Refresh();

                    return;
            }

            switch (uiState.EditMode)
            {
                case EditMode.Selection:
                    
                    mouseDownSelection(e);

                    break;

                case EditMode.Draw:

                    switch (mapWidgetMode)
                    {
                        case MapWidgetMode.Actor:

                            mouseDownActorDraw(e);

                            break;

                        case MapWidgetMode.Event:

                            mouseDownEventDraw(e);

                            break;

                        case MapWidgetMode.HudElement:

                            mouseDownHudElementDraw(e);

                            break;

                        case MapWidgetMode.WorldGeometry:

                            mouseDownWorldGeometryDraw(e);

                            break;

                        case MapWidgetMode.None:

                            break;

                        default:

                            mouseDownGenericMapWidgetDraw(e);

                            break;
                    }

                    break;
            }

            pbTiles.Refresh();
        }

        private void pbTiles_MouseEnter(object sender, System.EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (uiState.ShowMouseOver == false && uiState.EditMode == EditMode.Draw)
            {
                Cursor.Hide();                
            }
            else
            {
                switch (uiState.EditMode)
                {
                    case EditMode.Draw:
                        OnCursorChanged(new CursorChangedEventArgs(Cursors.Arrow));
                        break;
                    case EditMode.Selection:
                        OnCursorChanged(new CursorChangedEventArgs(Cursors.Cross));
                        break;
                    case EditMode.Grab:
                        OnCursorChanged(new CursorChangedEventArgs(Cursors.SizeAll));
                        break;
                }
            }

            showCursor_ = true;
            
            this.Refresh();
        }

        private void pbTiles_MouseLeave(object sender, System.EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (uiState.ShowMouseOver == false || uiState.EditMode != EditMode.Draw)
            {
                Cursor.Show();
            }

            resetCursor();

            OnCursorChanged(new CursorChangedEventArgs(Cursors.Arrow));

            showCursor_ = false;

            this.Refresh();
        }

        private void pbTiles_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X == mouseCursorX_ && e.Y == mouseCursorY_)
            {
                // For some reason, refreshing the property grid in the actor list control causes the mousemove event to keep
                // firing, even when the mouse is not moving. This in turn causes issues with the tmrScroll tick event not firing,
                // also for reasons unknown. If the mouse position didn't actually change, just return. This resolves the problem.
                return;
            }
            
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project != null && project.IsPrepared == true)
            {
                Guid selectedRoomId = uiState.SelectedRoomId;
                int selectedRoomIndex = uiState.SelectedRoomIndex;
                int tileSize = project.TileSize;
                int cameraWidth = project.CameraWidth;
                int cameraHeight = project.CameraHeight;
                
                MapWidgetMode mapWidgetMode = uiState.MapWidgetMode[selectedRoomId];
                
                Point worldLocation;
                Point mapLocation;
                Point canvasLocation;

                LayerDto selectedLayer;

                int selectedLayerIndex;
                int selectedLayerOrdinal;

                // Need to get the layer offset so the mouse position is done correctly.
               
                //// When drawing HUD elements, always use the interactive layer rather than the selcted layer.
                //if (mapWidgetMode == MapWidgetMode.HudElement)
                //{
                //    int interactiveLayerIndex = project.InteractiveLayerIndexes[selectedRoomId];
                //    int interactiveLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, interactiveLayerIndex);

                //    selectedLayerIndex = interactiveLayerIndex;
                //    selectedLayerOrdinal = interactiveLayerOrdinal;

                //    selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, interactiveLayerIndex);

                //    worldLocation = layerPositionInWorldSpaceRelativeToCamera(interactiveLayerOrdinal);
                //    mapLocation = translateWorldToMap(worldLocation);
                //    canvasLocation = translateToCanvas(mapLocation);
                //}
                //else
                //{
                    selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                    selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

                    selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

                    worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
                    mapLocation = translateWorldToMap(worldLocation);
                    canvasLocation = translateToCanvas(mapLocation);
                //}

                int rows = selectedLayer.Rows;
                int cols = selectedLayer.Cols;

                bool isVisible = uiState.LayerVisible[selectedLayer.Id];

                mouseCursorX_ = e.X - canvasLocation.X;

                //mouseDragCursorX_ = mouseCursorX_;

                int clickedCol = mouseCursorX_ / tileSize;

                // Handle rounding error for negative values.
                if (mouseCursorX_ < 0)
                {
                    clickedCol = -1;
                }

                mouseCursorY_ = e.Y - canvasLocation.Y;

                //mouseDragCursorY_ = mouseCursorY_;

                int clickedRow = mouseCursorY_ / tileSize;

                // Handle rounding error for negative values.
                if (mouseCursorY_ < 0)
                {
                    clickedRow = -1;
                }

                squareMouseOverX_ = clickedCol;
                squareMouseOverY_ = clickedRow;

                switch (uiState.EditMode)
                {
                    case EditMode.Selection:
                        
                        mouseMoveSelection(e);

                        break;

                    case EditMode.Draw:

                        switch (mapWidgetMode)
                        {
                            case MapWidgetMode.Event:

                                mouseMoveEventDraw(e);

                                break;

                            case MapWidgetMode.WorldGeometry:

                                mouseMoveWorldGeometryDraw(e);

                                break;
                        }

                        break;

                    case EditMode.Grab:

                        int interactiveLayerIndex = project.InteractiveLayerIndexes[selectedRoomId];

                        LayerDto interactiveLayer = project.Layers[selectedRoomId][interactiveLayerIndex];

                        int maxCols, maxRows;

                        maxRows = interactiveLayer.Rows;
                        maxCols = interactiveLayer.Cols;

                        if (mouseLeftDown_ == true || (mouseRightDown_ == true && uiState.CameraMode == CameraMode.CameraLocked))
                        {
                            int cameraX = uiState.CameraLocation[selectedRoomId].X;
                            int newCameraX = cameraX - (oldX_ - e.X);

                            // Keep the camera within the layer bounds.
                            if (newCameraX < 0)
                            {
                                newCameraX = 0;
                            }

                            if (newCameraX > (maxCols * tileSize) - cameraWidth)
                            {
                                newCameraX = (maxCols * tileSize) - cameraWidth;
                            }

                            uiState.CameraLocation[selectedRoomId].X = newCameraX;

                            int cameraY = uiState.CameraLocation[selectedRoomId].Y;
                            int newCameraY = cameraY - (oldY_ - e.Y);

                            // Keep the camera within the layer bounds.
                            if (newCameraY < 0)
                            {
                                newCameraY = 0;
                            }

                            if (newCameraY > (maxRows * tileSize) - cameraHeight)
                            {
                                newCameraY = (maxRows * tileSize) - cameraHeight;
                            }

                            uiState.CameraLocation[selectedRoomId].Y = newCameraY;

                            hsCameraPosition.Value = newCameraX + hsCameraPosition.Minimum;
                            vsCameraPosition.Value = newCameraY + vsCameraPosition.Minimum;
                            
                            // Convert camera world space to canvas space
                            // If the camera is offscreen, scroll the offset so that it is on screen.
                            switch (uiState.CameraMode)
                            {
                                case CameraMode.CameraLocked:

                                    // Move the canvas offset to line up. Camera map space position minus the canvas offset.
                                    Point cameraMap = new Point(0, 0);
                                    cameraMap = translateWorldToMap(new Point(newCameraX, newCameraY));

                                    hsCanvasOffset.Value += (((cameraMap.X - uiState.CanvasOffset[selectedRoomId].X) + cameraWidth)) - pbTiles.Width;
                                    uiState.CanvasOffset[selectedRoomId].X = hsCanvasOffset.Value - hsCanvasOffset.Minimum;

                                    vsCanvasOffset.Value += (((cameraMap.Y - uiState.CanvasOffset[selectedRoomId].Y) + cameraHeight)) - pbTiles.Height;
                                    uiState.CanvasOffset[selectedRoomId].Y = vsCanvasOffset.Value - vsCanvasOffset.Minimum;
                                    break;

                                default:
                                    break;
                            }

                        }
                        else if (mouseRightDown_ == true && uiState.CameraMode == CameraMode.CameraFree)
                        {
                            int newCanvasOffsetX = uiState.CanvasOffset[selectedRoomId].X - (oldX_ - e.X);
                            
                            // Keep the canvas within the proper bounds.
                            if (newCanvasOffsetX < 0)
                            {
                                newCanvasOffsetX = 0;
                            }

                            if (newCanvasOffsetX > hsCanvasOffset.Maximum - hsCanvasOffset.Minimum)
                            {
                                newCanvasOffsetX = hsCanvasOffset.Maximum - hsCanvasOffset.Minimum;
                            }

                            uiState.CanvasOffset[selectedRoomId].X = newCanvasOffsetX;

                            int newCanvasOffsetY = uiState.CanvasOffset[selectedRoomId].Y - (oldY_ - e.Y);
                            
                            if (newCanvasOffsetY < 0)
                            {
                                newCanvasOffsetY = 0;
                            }

                            if (newCanvasOffsetY > vsCanvasOffset.Maximum - vsCanvasOffset.Minimum)
                            {
                                newCanvasOffsetY = vsCanvasOffset.Maximum - vsCanvasOffset.Minimum;
                            }
                            
                            uiState.CanvasOffset[selectedRoomId].Y = newCanvasOffsetY;
                            
                            hsCanvasOffset.Value = newCanvasOffsetX + hsCanvasOffset.Minimum;
                            vsCanvasOffset.Value = newCanvasOffsetY + vsCanvasOffset.Minimum;
                        }

                        break;
                }
                                
                oldX_ = e.X;
                oldY_ = e.Y;

                pbTiles.Refresh();
            }
        }
        
        private void pbTiles_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    mouseRightDown_ = false;
                    break;

                case MouseButtons.Left:
                    mouseLeftDown_ = false;
                    break;

                case MouseButtons.Middle:
                    mouseMiddleDown_ = false;
                    break;
            }

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;
            
            MapWidgetMode mapWidgetMode = uiState.MapWidgetMode[selectedRoomId];

            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];

            LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

            tmrScroll.Stop();
            isScrollTimerStarted_ = false;
            
            switch (uiState.EditMode)
            {
                case EditMode.Selection:

                    mouseUpSelection(e);

                    break;

                case EditMode.Draw:
                    
                    switch (mapWidgetMode)
                    {
                        case MapWidgetMode.Event:

                            mouseUpEventDraw(e);

                            break;

                        case MapWidgetMode.WorldGeometry:

                            mouseUpWorldGeometryDraw(e);

                            break;
                    }

                    break;
            }

            this.Refresh();
        }
        
        private void pbTiles_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (this.DesignMode == false && projectController_ != null)
            {
                ProjectDto project = projectController_.GetProjectDto();

                if (project == null || project.IsPrepared == false)
                {
                    return;
                }

                ProjectUiStateDto uiState = projectController_.GetUiState();
            
                int tileSize = project.TileSize;

                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                
                MapWidgetMode mapWidgetMode = uiState.MapWidgetMode[selectedRoomId];

                LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

                g = e.Graphics;

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                // Determine the size of the background. Start by calculating the size of the largest layer.
                // Then, if it is larger than the canvas, clip it down to just the canvas size.
                int maxCols = uiState.MaxCols[selectedRoomId];
                int maxRows = uiState.MaxRows[selectedRoomId];

                int largestLayerHeight = maxRows * tileSize;
                int largestLayerWidth = maxCols * tileSize;

                if (largestLayerHeight >= pbTiles.Height)
                {
                    szBounds_.Height = pbTiles.Height;
                }
                else
                {
                    szBounds_.Height = largestLayerHeight;
                }

                if (largestLayerWidth >= pbTiles.Width)
                {
                    szBounds_.Width = pbTiles.Width;
                }
                else
                {
                    szBounds_.Width = largestLayerWidth;
                }


                Size szFullBG = new Size(szBounds_.Width + szBuffer_.Width, szBounds_.Height + szBuffer_.Height);

                // If the largest layer height or width is smaller than the extra buffer region, don't render to it.
                if (szFullBG.Height > largestLayerHeight)
                {
                    szFullBG.Height = largestLayerHeight;
                }

                if (szFullBG.Width > largestLayerWidth)
                {
                    szFullBG.Width = largestLayerWidth;
                }

                backgroundGenerator_.GenerateBackground(szFullBG.Width, szFullBG.Height);

                g.DrawImageUnscaled(backgroundGenerator_.BackgroundImage, new Point(0, 0));

                // Render the layers.
                int layerCount = projectController_.GetLayerCount(selectedRoomIndex);

                for (int k = 0; k < layerCount; k++)
                {
                    int layerIndex = projectController_.GetLayerIndexFromOrdinal(selectedRoomIndex, k);

                    Guid layerId = project.Layers[selectedRoomId][layerIndex].Id;

                    if (uiState.LayerVisible[layerId] == true)
                    {
                        renderMapWidgets(g, k);                        
                    }
                }
                
                // Render the map widgets not in a layer (HUD elements).
                renderMapWidgets(g, -1);

                //renderHudElements(g);

                // Render the grid for the selected layer.
                if (uiState.LayerVisible[selectedLayer.Id] == true)
                {
                    renderGrid(g, selectedRoomIndex, selectedLayerIndex);
                }

                // Render the camera viewport
                if (uiState.ShowCameraOutline == true)
                {
                    Point viewportLocation = new Point(0, 0);

                    int cameraX = uiState.CameraLocation[selectedRoomId].X;
                    int cameraY = uiState.CameraLocation[selectedRoomId].Y;
                    int cameraW = project.CameraWidth;
                    int cameraH = project.CameraHeight;

                    viewportLocation = translateWorldToMap(new Point(cameraX, cameraY));
                    viewportLocation = translateToCanvas(viewportLocation);

                    Color c = Globals.cameraViewportColor;
                    g.DrawRectangle(new Pen(c, 2.0f), viewportLocation.X, viewportLocation.Y, cameraW, cameraH);
                }

                // Render the cursor.
                if (uiState.EditMode == EditMode.Draw)
                {
                    // Add map widgets here.
                    switch (mapWidgetMode)
                    {
                        case MapWidgetMode.Actor:  
                                                      
                            renderActorCursor(g);

                            break;

                        case MapWidgetMode.Event:    
                                                    
                            renderEventCursor(g);

                            break;

                        case MapWidgetMode.HudElement:

                            renderHudElementCursor(g);

                            break;

                        case MapWidgetMode.WorldGeometry:

                            renderWorldGeometryCursor(g);

                            break;

                        default:

                            if (mapWidgetCursor_.ContainsKey(mapWidgetMode))
                            {
                                IRoomEditorCursor cursor = mapWidgetCursor_[mapWidgetMode];

                                renderCursor(g, cursor);
                            }

                            break;
                    }
                }

                // Render outlines on the selected layer. Having the outlines render after the grid
                // makes it easier to see where they are.
                renderMapWidgetOutlines(g);

                // Render the cursor outline.
                if (uiState.EditMode == EditMode.Draw)
                {
                    if (mapWidgetMode == MapWidgetMode.Actor)
                    {
                        renderActorCursorOutline(g);
                    }
                    else if (mapWidgetMode == MapWidgetMode.HudElement)
                    {
                        renderHudElementCursorOutline(g);
                    }
                }

                //// Render the outline of the camera. Convert the world space to canvas space.
                //g.DrawRectangle(pViewport, viewportLocation.X, viewportLocation.Y, rcRooms_.CameraWidth, rcRooms_.CameraHeight);

                // Render the selection rectangle                
                renderSelector(g, uiState.MapWidgetSelector[selectedRoomId]);

                // Render the name of the selected layer.

                Font f = new Font(FontFamily.GenericSansSerif, 8.0f);

                SizeF stringWidth = g.MeasureString(selectedLayer.Name, f);

                Point relativePoint = pbTiles.PointToClient(Cursor.Position);
                
                Point labelOffset = new Point(0, 0);

                if (mapWidgetCursor_.ContainsKey(mapWidgetMode))
                {
                    IRoomEditorCursor cursor = mapWidgetCursor_[mapWidgetMode];

                    labelOffset.X = cursor.LayerNameOffset.X;

                    labelOffset.Y = cursor.LayerNameOffset.Y;
                }

                g.FillRectangle(new SolidBrush(Color.FromArgb(196, 0, 0, 0)), relativePoint.X + labelOffset.X, relativePoint.Y - stringWidth.ToSize().Height - labelOffset.Y, stringWidth.ToSize().Width, stringWidth.ToSize().Height);
                
                g.DrawString(selectedLayer.Name, f, new SolidBrush(Color.White), (float)relativePoint.X + labelOffset.X, (float)relativePoint.Y - stringWidth.ToSize().Height - labelOffset.Y);
            }
        }

        private void RoomEditorControl_AfterLayerDeleted(object sender, AfterLayerDeletedEventArgs e)
        {
            resizeControls();
        }

        private void RoomEditorControl_BeforeRoomDeleted(object sender, BeforeRoomDeletedEventArgs e)
        {
            resizeControls();
        }

        private void RoomEditorControl_CameraModeChanged(object sender, CameraModeChangedEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            resizeControls();

            switch (e.CameraMode)
            {
                case CameraMode.CameraFree:
                    hsCanvasOffset.Enabled = true;
                    vsCanvasOffset.Enabled = true;
                    break;

                case CameraMode.CameraLocked:
                    hsCanvasOffset.Enabled = false;
                    vsCanvasOffset.Enabled = false;

                    // Set the scrollbars so that the camera view is centered.
                    // Canvas offset needs to be equal to the camera offset
                    int cameraX = uiState.CameraLocation[selectedRoomId].X;
                    int cameraY = uiState.CameraLocation[selectedRoomId].Y;

                    uiState.CanvasOffset[selectedRoomId].X = cameraX;
                    uiState.CanvasOffset[selectedRoomId].Y = cameraY;

                    // Move the canvas offset to line up. Camera map space position minus the canvas offset.
                    Point cameraMap = new Point(0, 0);
                    cameraMap = translateWorldToMap(new Point(cameraX, cameraY));

                    int deltaX = cameraMap.X - uiState.CanvasOffset[selectedRoomId].X;
                    int deltaY = cameraMap.Y - uiState.CanvasOffset[selectedRoomId].Y;

                    uiState.CanvasOffset[selectedRoomId].X += deltaX;
                    uiState.CanvasOffset[selectedRoomId].Y += deltaY;

                    hsCanvasOffset.Value = uiState.CanvasOffset[selectedRoomId].X + hsCanvasOffset.Minimum;
                    vsCanvasOffset.Value = uiState.CanvasOffset[selectedRoomId].Y + vsCanvasOffset.Minimum;

                    break;

                default:
                    break;
            }

            pbTiles.Refresh();
        }

        private void RoomEditorControl_InteractiveLayerChanged(object sender, InteractiveLayerChangedEventArgs e)
        {
            locateCamera();

            backgroundGenerator_.Regenerate();

            pbTiles.Refresh();
        }

        private void RoomEditorControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    isUpArrowKeyDown_ = true;
                    break;

                case Keys.Down:
                    isDownArrowKeyDown_ = true;
                    break;

                case Keys.Left:
                    isLeftArrowKeyDown_ = true;
                    break;

                case Keys.Right:
                    isRightArrowKeyDown_ = true;
                    break;
                
                default:
                    e.SuppressKeyPress = true;
                    break;
            }

            if (isLeftArrowKeyDown_ == true)
            {
                keyboardDragOffset_.X -= 1;
            }
            else if (isRightArrowKeyDown_ == true)
            {
                keyboardDragOffset_.X += 1;
            }

            if (isDownArrowKeyDown_ == true)
            {
                keyboardDragOffset_.Y += 1;
            }
            else if (isUpArrowKeyDown_ == true)
            {
                keyboardDragOffset_.Y -= 1;
            }

            if (isUpArrowKeyDown_ == true || isDownArrowKeyDown_ == true ||
                isLeftArrowKeyDown_ == true || isRightArrowKeyDown_ == true)
            {
                keyboardMovingObjects_ = true;
            }

            pbTiles.Refresh();
        }
        
        private void RoomEditorControl_KeyUp(object sender, KeyEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            if (selectedRoomId != Guid.Empty)
            {
                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];

                // If objects are being moved with the arrow keys, keep track of them so it can be undone/redone
                switch (e.KeyCode)
                {
                    case Keys.Up:

                        isUpArrowKeyDown_ = false;

                        break;

                    case Keys.Down:

                        isDownArrowKeyDown_ = false;

                        break;

                    case Keys.Left:

                        isLeftArrowKeyDown_ = false;

                        break;

                    case Keys.Right:

                        isRightArrowKeyDown_ = false;

                        break;
                }

                if (isUpArrowKeyDown_ == false && isDownArrowKeyDown_ == false &&
                    isLeftArrowKeyDown_ == false && isRightArrowKeyDown_ == false)
                {
                    // Update the selected entity instances with the new position.
                    
                    MapWidgetSelectorDto mapWidgetSelector = uiState.MapWidgetSelector[selectedRoomId];
                    
                    foreach (Guid instanceId in mapWidgetSelector.SelectedMapWidgetIds)
                    {
                        MapWidgetDto mapWidgetInstance = projectController_.GetMapWidget(instanceId);

                        int x = mapWidgetInstance.BoundingBox.Left;
                        int y = mapWidgetInstance.BoundingBox.Top;

                        if (keyboardDragOffset_.X != 0 || keyboardDragOffset_.Y != 0)
                        {
                            projectController_.SetMapWidgetPosition(instanceId, new Point2D(keyboardDragOffset_.X + x, keyboardDragOffset_.Y + y));
                        }                        
                    }
                    
                    keyboardDragOffset_.X = 0;
                    keyboardDragOffset_.Y = 0;

                    pbTiles.Refresh();

                    keyboardMovingObjects_ = false;
                }
            }
        }
        
        private void RoomEditorControl_LayerAdded(object sender, LayerAddedEventArgs e)
        {
            resizeControls();
        }

        private void RoomEditorControl_LayerResized(object sender, LayerResizedEventArgs e)
        {
            resizeControls();
            locateCamera();
        }

        private void RoomEditorControl_Load(object sender, EventArgs e)
        {

        }
        
        private void RoomEditorControl_MouseMove(object sender, MouseEventArgs e)
        {
            pbTiles.Refresh();
        }

        private void RoomEditorControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
        }
        
        private void RoomEditorControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int tileSize = project.TileSize;
            int cameraWidth = project.CameraWidth;
            int cameraHeight = project.CameraHeight;

            //actorCursor_ = firemelonEditorFactory_.NewActorCursor(projectController_);
            audioSourceCursor_ = firemelonEditorFactory_.NewAudioSourceCursor(projectController_);
            particleEmitterCursor_ = firemelonEditorFactory_.NewParticleEmitterCursor(projectController_);
            spawnPointCursor_ = firemelonEditorFactory_.NewSpawnPointCursor(projectController_);
            worldGeometryCursor_ = firemelonEditorFactory_.NewWorldGeometryCursor(projectController_);
            tileObjectCursor_ = firemelonEditorFactory_.NewTileObjectCursor(projectController_);

            mapWidgetCursor_.Clear();


           // mapWidgetCursor_.Add(MapWidgetMode.Actor, actorCursor_);
            mapWidgetCursor_.Add(MapWidgetMode.AudioSource, audioSourceCursor_);
            mapWidgetCursor_.Add(MapWidgetMode.ParticleEmitter, particleEmitterCursor_);
            mapWidgetCursor_.Add(MapWidgetMode.SpawnPoint, spawnPointCursor_);
            mapWidgetCursor_.Add(MapWidgetMode.WorldGeometry, worldGeometryCursor_);
            mapWidgetCursor_.Add(MapWidgetMode.TileObject, tileObjectCursor_);
            
            // Create a blank tile to be used as the cursor when no tile is selected. This may be no longer needed.

            // Reset the blank tile image.
            if (bmpBlankTile_ != null)
            {
                bmpBlankTile_.Dispose();
                bmpBlankTile_ = null;
            }
            
            mapWidgetFactory_.TileSize = project.TileSize;

            bmpBlankTile_ = new Bitmap(tileSize, tileSize);
            Graphics g = Graphics.FromImage(bmpBlankTile_);
            g.Clear(Color.Black);

            backgroundGenerator_.Regenerate();

            hsCameraPosition.Enabled = true;
            vsCameraPosition.Enabled = true;

            for (int i = 0; i < project.Rooms.Count; i++)
            {
                LayerDto interactiveLayer = projectController_.GetInteractiveLayer(i);

                Guid roomId = project.Rooms[i].Id;

                int hsCameraLocationMax = (interactiveLayer.Cols * tileSize) - cameraWidth;
                int vsCameraLocationMax = (interactiveLayer.Rows * tileSize) - cameraHeight;

                uiState.CameraLocationMax[roomId].X = hsCameraLocationMax;
                uiState.CameraLocationMax[roomId].Y = vsCameraLocationMax;

                if (i == selectedRoomIndex)
                {
                    hsCameraPosition.Maximum = hsCameraLocationMax;
                    vsCameraPosition.Maximum = vsCameraLocationMax;
                }

                if (uiState.CameraLocation[roomId].X > hsCameraLocationMax)
                {
                    uiState.CameraLocation[roomId].X = hsCameraLocationMax;
                }

                if (uiState.CameraLocation[roomId].Y > vsCameraLocationMax)
                {
                    uiState.CameraLocation[roomId].Y = vsCameraLocationMax;
                }
            }

            // The resize code will set the canvas offset scrollbar values,
            resizeControls();

            switch (uiState.CameraMode)
            {
                case CameraMode.CameraFree:
                    hsCanvasOffset.Enabled = true;
                    vsCanvasOffset.Enabled = true;
                    break;

                case CameraMode.CameraLocked:
                    hsCanvasOffset.Enabled = false;
                    vsCanvasOffset.Enabled = false;
                    break;

                default:
                    break;
            }

            pbTiles.Refresh();
		}
        
        private void RoomEditorControl_RefreshView(object sender, RefreshViewEventArgs e)
        {
            pbTiles.Refresh();
        }
        
        private void RoomEditorControl_Resize(object sender, System.EventArgs e)
        {
            resizeControls();
        }

        private void RoomEditorControl_RoomAdded(object sender, RoomAddedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int roomIndex = e.Index;
            Guid roomId = project.Rooms[roomIndex].Id;

            int tileSize = project.TileSize;
            int cameraWidth = project.CameraWidth;
            int cameraHeight = project.CameraHeight;

            LayerDto interactiveLayer = projectController_.GetInteractiveLayer(roomIndex);

            int hMax = (interactiveLayer.Cols * tileSize) - cameraWidth;
            int vMax = (interactiveLayer.Rows * tileSize) - cameraHeight;

            uiState.CameraLocationMax[roomId].X = hMax;
            uiState.CameraLocationMax[roomId].Y = vMax;

            if (uiState.CameraLocation[roomId].X > hMax)
            {
                uiState.CameraLocation[roomId].X = hMax;
            }

            if (uiState.CameraLocation[roomId].Y > vMax)
            {
                uiState.CameraLocation[roomId].Y = vMax;
            }

            // Change the maximum value of the scrollbar if more of the map is being hidden.
            int newVMax, newHMax;

            int layerWidth = uiState.MaxCols[roomId] * tileSize;
            int layerHeight = uiState.MaxRows[roomId] * tileSize;

            if (layerHeight <= pbTiles.Height)
            {
                newVMax = vsCanvasOffset.Minimum;
            }
            else
            {
                newVMax = (layerHeight - pbTiles.Height) + 1;
            }

            if (layerWidth <= pbTiles.Width)
            {
                newHMax = hsCanvasOffset.Minimum;
            }
            else
            {
                newHMax = (layerWidth - pbTiles.Width) + 1;
            }

            uiState.CanvasOffsetMax[roomId].X = newHMax;
            uiState.CanvasOffsetMax[roomId].Y = newVMax;

        }
        
        private void RoomEditorControl_RoomSelected(object sender, RoomSelectedEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;

            backgroundGenerator_.Regenerate();

            hsCameraPosition.Maximum = uiState.CameraLocationMax[selectedRoomId].X;
            vsCameraPosition.Maximum = uiState.CameraLocationMax[selectedRoomId].Y;

            hsCameraPosition.Value = uiState.CameraLocation[selectedRoomId].X;
            vsCameraPosition.Value = uiState.CameraLocation[selectedRoomId].Y;

            hsCanvasOffset.Maximum = uiState.CanvasOffsetMax[selectedRoomId].X;
            vsCanvasOffset.Maximum = uiState.CanvasOffsetMax[selectedRoomId].Y;

            hsCanvasOffset.Value = uiState.CanvasOffset[selectedRoomId].X;
            vsCanvasOffset.Value = uiState.CanvasOffset[selectedRoomId].Y;

            pbTiles.Refresh();
        }
        
        private void RoomEditorControl_TileObjectSelected(object sender, TileObjectSelectedEventArgs e)
        {
            ((TileObjectCursor)tileObjectCursor_).TileObjectId = e.TileObjectId;
        }

        private void tmrScroll_Tick(object sender, EventArgs e)
        {
            Point cursorPosition = Cursor.Position;

            Point canvasPosition = this.PointToScreen(pbTiles.Location);

            Point controlPosition = this.PointToScreen(this.Location);

            bool isOutsideRect = false;

            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            CameraMode cameraMode = uiState.CameraMode;
            
            int left = Math.Max(canvasPosition.X, controlPosition.X);
            int right = Math.Min(canvasPosition.X + pbTiles.Width, controlPosition.X + this.Width);
            int width = right - left;
            
            int top = Math.Max(canvasPosition.Y, controlPosition.Y);
            int bottom = Math.Min(canvasPosition.Y + pbTiles.Height, controlPosition.Y + this.Height);
            int height = bottom - top;

            System.Drawing.Rectangle finalRect = new System.Drawing.Rectangle(left, top, width, height);

            int pixelsPerScrollSpeed = 5;
            int scrollAmountPerTick = 5;

            int maxScrollSpeed = 10 * scrollAmountPerTick;

            if (finalRect.Contains(cursorPosition) == false)
            {
                isOutsideRect = true;
            }

            if (isOutsideRect == true)
            {
                bool scrollRight = cursorPosition.X > right;
                bool scrollLeft = cursorPosition.X < left;
                bool scrollUp = cursorPosition.Y < top;
                bool scrollDown = cursorPosition.Y > bottom;

                int scrollChange = 0;

                if (uiState.EditMode == EditMode.Selection)
                {
                    Guid selectedRoomId = uiState.SelectedRoomId;

                    if (scrollRight == true)
                    {
                        // Scroll by 'scrollAmountPerTick' for every 'pixelsPerScrollSpeed' the mouse cursor is from the right border, with a maximum value of 'maxScrollSpeed'.
                        scrollChange = Math.Min((int)((cursorPosition.X - right) / pixelsPerScrollSpeed), maxScrollSpeed) * scrollAmountPerTick;

                        if (cameraMode == CameraMode.CameraLocked)
                        {
                            if (hsCameraPosition.Value + scrollChange > hsCameraPosition.Maximum)
                            {
                                scrollChange = hsCameraPosition.Maximum - hsCameraPosition.Value;
                            }
                            
                            hsCameraPosition.Value += scrollChange;
                            uiState.CameraLocation[selectedRoomId].X += scrollChange;
                        }

                        if (hsCanvasOffset.Value + scrollChange > hsCanvasOffset.Maximum)
                        {
                            scrollChange = hsCanvasOffset.Maximum - hsCanvasOffset.Value;
                        }

                        hsCanvasOffset.Value += scrollChange;
                        uiState.CanvasOffset[selectedRoomId].X += scrollChange;                    
                    }
                    else if (scrollLeft == true)
                    {
                        // Scroll by 'scrollAmountPerTick' for every 'pixelsPerScrollSpeed' the mouse cursor is from the left border, with a maximum value of 'maxScrollSpeed'.
                        scrollChange = Math.Min((int)((left - cursorPosition.X) / pixelsPerScrollSpeed), maxScrollSpeed) * scrollAmountPerTick;

                        if (cameraMode == CameraMode.CameraLocked)
                        {
                            if (hsCameraPosition.Value - scrollChange < hsCameraPosition.Minimum)
                            {
                                scrollChange = hsCameraPosition.Value;
                            }

                            hsCameraPosition.Value -= scrollChange;
                            uiState.CameraLocation[selectedRoomId].X -= scrollChange;
                        }

                        if (hsCanvasOffset.Value - scrollChange < hsCanvasOffset.Minimum)
                        {
                            scrollChange = hsCanvasOffset.Value;
                        }

                        hsCanvasOffset.Value -= scrollChange;
                        uiState.CanvasOffset[selectedRoomId].X -= scrollChange;
                    }


                    if (scrollDown == true)
                    {
                        // Scroll by 'scrollAmountPerTick' for every 'pixelsPerScrollSpeed' the mouse cursor is from the bottom border, with a maximum value of 'maxScrollSpeed'.
                        scrollChange = Math.Min((int)((cursorPosition.Y - bottom) / pixelsPerScrollSpeed), maxScrollSpeed) * scrollAmountPerTick;

                        if (cameraMode == CameraMode.CameraLocked)
                        {
                            if (vsCameraPosition.Value + scrollChange > vsCameraPosition.Maximum)
                            {
                                scrollChange = vsCameraPosition.Maximum - vsCameraPosition.Value;
                            }

                            vsCameraPosition.Value += scrollChange;
                            uiState.CameraLocation[selectedRoomId].Y += scrollChange;
                        }

                        if (vsCanvasOffset.Value + scrollChange > vsCanvasOffset.Maximum)
                        {
                            scrollChange = vsCanvasOffset.Maximum - vsCanvasOffset.Value;
                        }

                        vsCanvasOffset.Value += scrollChange;
                        uiState.CanvasOffset[selectedRoomId].Y += scrollChange;
                    }
                    else if (scrollUp == true)
                    {
                        // Scroll by 'scrollAmountPerTick' for every 'pixelsPerScrollSpeed' the mouse cursor is from the top border, with a maximum value of 'maxScrollSpeed'.
                        scrollChange = Math.Min((int)((top - cursorPosition.Y) / pixelsPerScrollSpeed), maxScrollSpeed) * scrollAmountPerTick;

                        if (cameraMode == CameraMode.CameraLocked)
                        {
                            if (vsCameraPosition.Value - scrollChange < vsCameraPosition.Minimum)
                            {
                                scrollChange = vsCameraPosition.Value;
                            }

                            vsCameraPosition.Value -= scrollChange;
                            uiState.CameraLocation[selectedRoomId].Y -= scrollChange;
                        }

                        if (vsCanvasOffset.Value - scrollChange < vsCanvasOffset.Minimum)
                        {
                            scrollChange = vsCanvasOffset.Value;
                        }

                        vsCanvasOffset.Value -= scrollChange;
                        uiState.CanvasOffset[selectedRoomId].Y -= scrollChange;
                    }

                    int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
                    int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

                    // Get the location of the layer in canvas space.
                    Point worldLocation = layerPositionInWorldSpaceRelativeToCamera(selectedLayerOrdinal);
                    Point mapLocation = translateWorldToMap(worldLocation);
                    Point canvasLocation = translateToCanvas(mapLocation);

                    // Get the mouse cursor point for the canvas, and then translate it to layer space for the selected layer.
                    Point cursorPointOnCanvas = new Point(pbTiles.PointToClient(Cursor.Position).X, pbTiles.PointToClient(Cursor.Position).Y);

                    mouseCursorX_ = cursorPointOnCanvas.X - canvasLocation.X;
                    mouseCursorY_ = cursorPointOnCanvas.Y - canvasLocation.Y;

                    projectController_.SetMapWidgetSelectorCorner2(selectedRoomIndex, mouseCursorX_, mouseCursorY_);
                }

                this.Refresh();
            }
        }

        private void vsCanvasOffset_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;

            uiState.CanvasOffset[selectedRoomId].Y = e.NewValue - vsCanvasOffset.Minimum;

            this.Refresh();
        }

        private void vsCameraPosition_Scroll(object sender, ScrollEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int cameraX = uiState.CameraLocation[selectedRoomId].X;
            int cameraY = e.NewValue - vsCameraPosition.Minimum;

            uiState.CameraLocation[selectedRoomId].Y = cameraY;

            switch (uiState.CameraMode)
            {
                case CameraMode.CameraLocked:
                    // Move the canvas offset to line up. Camera map space position minus the canvas offset.
                    Point cameraMap = new Point(0, 0);
                    cameraMap = translateWorldToMap(new Point(cameraX, cameraY));

                    int delta = cameraMap.Y - uiState.CanvasOffset[selectedRoomId].Y;

                    vsCanvasOffset.Value += delta;

                    uiState.CanvasOffset[selectedRoomId].Y = vsCanvasOffset.Value - vsCanvasOffset.Minimum;

                    break;

                case CameraMode.CameraFree:

                    break;

                default:
                    break;
            }

            this.Refresh();
        }
        
        #endregion
    }
}