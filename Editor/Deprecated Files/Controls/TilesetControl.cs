using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace FiremelonEditor2
{
    // The tile type is determined by concatenating the bits of the following enumeration values.

    public enum TileType
    {
        SOLIDTILE = 0,									// 0b000000
        SLOPE_TRANSITION = 1,							// 0b000001
        NONE = 2,							            // 0b000010
        ONEWAY_TOP = 3,			    		            // 0b000011
        SLOPE45_BOTTOMLEFT_TILE = 32,					// 0b100000
        SLOPE45_TOPLEFT_TILE = 36,						// 0b100100
        SLOPE45_BOTTOMRIGHT_TILE = 40,					// 0b101000
        SLOPE45_TOPRIGHT_TILE = 44,						// 0b101100
        SLOPE26_HORIZONTAL_BOTTOMLEFT_SMALL_TILE = 48,	// 0b110000
        SLOPE26_HORIZONTAL_TOPLEFT_SMALL_TILE = 52,		// 0b110100
        SLOPE26_HORIZONTAL_BOTTOMRIGHT_SMALL_TILE = 56,	// 0b111000
        SLOPE26_HORIZONTAL_TOPRIGHT_SMALL_TILE = 60,	// 0b111100
        SLOPE26_HORIZONTAL_BOTTOMLEFT_LARGE_TILE = 50,	// 0b110010
        SLOPE26_HORIZONTAL_TOPLEFT_LARGE_TILE = 54,		// 0b110110
        SLOPE26_HORIZONTAL_BOTTOMRIGHT_LARGE_TILE = 58,	// 0b111010
        SLOPE26_HORIZONTAL_TOPRIGHT_LARGE_TILE = 62,	// 0b111110
        SLOPE26_VERTICAL_BOTTOMLEFT_SMALL_TILE = 49,	// 0b110001
        SLOPE26_VERTICAL_TOPLEFT_SMALL_TILE = 53,		// 0b110101
        SLOPE26_VERTICAL_BOTTOMRIGHT_SMALL_TILE = 57,	// 0b111001
        SLOPE26_VERTICAL_TOPRIGHT_SMALL_TILE = 61,		// 0b111101
        SLOPE26_VERTICAL_BOTTOMLEFT_LARGE_TILE = 51,	// 0b110011
        SLOPE26_VERTICAL_TOPLEFT_LARGE_TILE = 55,		// 0b110111
        SLOPE26_VERTICAL_BOTTOMRIGHT_LARGE_TILE = 59,	// 0b111011
        SLOPE26_VERTICAL_TOPRIGHT_LARGE_TILE = 63		// 0b111111
    };

    // Bit 5-6: Indicates which type of geometry.
    // Bits 3-4: Stores the slope origin corner. Not used if not a slope.
    // Bit 2: Stores the slope size. Only used for 26 degree slopes.
    // Bit 1: Stores the slope orientation. Only used for 26 degree slopes.

    // The first bit indicates if this is a slope or not.
    // The second bit indicates the type of slope.
    public enum TileGeometry
    {
        SOLID = 0,    // 0b00
        SLOPE_45 = 2, // 0b10
        SLOPE_26 = 3  // 0b11
    };

    // The first bit indicates whether it is the bottom or top corner. 0 = Bottom, 1 = Top
    // The second bit indicates whether it is the right or left corner. 0 = Left, 1 = Right
    public enum TileSlopeOriginCorner
    {
        BOTTOMLEFT = 0,  //0b00
        TOPLEFT = 1,     //0b01
        BOTTOMRIGHT = 2, //0b10
        TOPRIGHT = 3     //0b11
    };

    public enum TileSlopeSize
    {
        SMALL = 0,
        LARGE = 1
    };

    public enum TileSlopeOrientation
    {
        HORIZONTAL = 0,
        VERTICAL = 1
    };

    //public class TilesetControl : System.Windows.Forms.UserControl, ITilesetControl
    //{
    //    private System.Windows.Forms.PictureBox pbTiles;
    //    private System.Windows.Forms.VScrollBar vScrollBar1;
    //    private System.ComponentModel.IContainer components;

    //    private Graphics g;

    //    private IFiremelonEditorFactory firemelonEditorFactory_;
    //    private IUtilityFactory utilityFactory_;
    //    private IProjectController projectController_;

    //    private IBackgroundGenerator backgroundGenerator_;
    //    private Bitmap bmpBackground_;
    //    private bool generateBackground_;
        
    //    // Scrolling data.
    //    private int rowOffset_;
    //    private int colOffset_;
    //    private int rowPixelOffset_;
    //    private int colPixelOffset_;

    //    private bool isMouseOverButton_;
    //    private bool isMouseDownButton_;

    //    private bool isMouseOverTileModeButton_;
    //    private bool isMouseDownTileModeButton_;

    //    private bool isMouseOverCreateObjectButton_;
    //    private bool isMouseDownCreateObjectButton_;

    //    // Tile selection drag.
    //    private bool isMouseDownTiles_;

    //    private int selectionCorner1TileId_;
    //    private int selectionCorner2TileId_;

    //    private IEnterNameDialog enterNameDialog_;

    //    private Panel pnAddNew;
    //    private Button btnPopOut;
    //    private HScrollBar hScrollBar1;
    //    private ContextMenuStrip mnuPopup;
    //    private ToolStripMenuItem selectImageToolStripMenuItem;
    //    private PictureBox pbEmptySpace;
    //    private PictureBox pbMenuButton;
    //    private PictureBox pbMenuNormal;
    //    private PictureBox pbMenuOver;
    //    private PictureBox pbMenuDown;

    //    private Panel pnTiles;

    //    private PictureBox pbTileModeNormal;
    //    private PictureBox pbTileModeOver;
    //    private PictureBox pbTileModeDown;
    //    private PictureBox pbTileModeNormal2;
    //    private PictureBox pbTileModeOver2;
    //    private PictureBox pbTileModeDown2;
    //    private PictureBox pbCreateObject;
    //    private PictureBox pbCreateTileObjectNormal;
    //    private PictureBox pbCreateTileObjectOver;
    //    private PictureBox pbCreateTileObjectDown;
    //    private PictureBox pbTileMode;
        
    //    public TilesetControl(IProjectController projectController)
    //    {
    //        // This call is required by the Windows.Forms Form Designer.
    //        InitializeComponent();

    //        projectController_ = projectController;

    //        firemelonEditorFactory_ = new FiremelonEditorFactory();

    //        backgroundGenerator_ = firemelonEditorFactory_.NewBackgroundGenerator();
            
    //        projectController_.RefreshView += new RefreshViewHandler(this.TilesetControl_RefreshView);
    //        projectController_.RoomSelected += new RoomSelectHandler(this.TilesetControl_RoomSelected);
    //        projectController_.ProjectCreated += new ProjectCreateHandler(this.TilesetControl_ProjectCreated);
    //        projectController_.CollisionModeChange += new CollisionModeChangedHandler(this.TilesetControl_CollisionModeChanged);

    //        enterNameDialog_ = firemelonEditorFactory_.NewEnterNameDialog();
    //        enterNameDialog_.NameEntered += new NameEnteredHandler(this.TilesetControl_ObjectNameEntered);

    //        rowOffset_ = 0;
    //        colOffset_ = 0;
    //        rowPixelOffset_ = 0;
    //        colPixelOffset_ = 0;

    //        isMouseOverButton_ = false;
    //        isMouseDownButton_ = false;

    //        isMouseOverTileModeButton_ = false;
    //        isMouseDownTileModeButton_ = false;

    //        isMouseDownTiles_ = false;

    //        selectionCorner1TileId_ = -1;
    //        selectionCorner2TileId_ = -2;

    //        generateBackground_ = true;

    //        bmpBackground_ = null;

    //        vScrollBar1.Maximum = 0;
    //        vScrollBar1.Minimum = 0;
    //        vScrollBar1.SmallChange = 1;
    //        vScrollBar1.LargeChange = 1;

    //        hScrollBar1.Maximum = 0;
    //        hScrollBar1.Minimum = 0;
    //        hScrollBar1.SmallChange = 1;
    //        hScrollBar1.LargeChange = 1;

    //        pbEmptySpace.BackColor = this.BackColor;

    //        utilityFactory_ = new UtilityFactory();
    //    }

    //    /// <summary> 
    //    /// Clean up any resources being used.
    //    /// </summary>
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (g != null)
    //        {
    //            g.Dispose();
    //        }

    //        if (disposing)
    //        {
    //            if (components != null)
    //            {
    //                components.Dispose();
    //            }
    //        }
    //        base.Dispose(disposing);
    //    }

    //    #region Component Designer generated code
    //    /// <summary> 
    //    /// Required method for Designer support - do not modify 
    //    /// the contents of this method with the code editor.
    //    /// </summary>
    //    private void InitializeComponent()
    //    {
    //        this.components = new System.ComponentModel.Container();
    //        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TilesetControl));
    //        this.pbTiles = new System.Windows.Forms.PictureBox();
    //        this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
    //        this.pnAddNew = new System.Windows.Forms.Panel();
    //        this.pbCreateObject = new System.Windows.Forms.PictureBox();
    //        this.pbTileMode = new System.Windows.Forms.PictureBox();
    //        this.pbMenuButton = new System.Windows.Forms.PictureBox();
    //        this.btnPopOut = new System.Windows.Forms.Button();
    //        this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
    //        this.mnuPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
    //        this.selectImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
    //        this.pbEmptySpace = new System.Windows.Forms.PictureBox();
    //        this.pbMenuNormal = new System.Windows.Forms.PictureBox();
    //        this.pbMenuOver = new System.Windows.Forms.PictureBox();
    //        this.pbMenuDown = new System.Windows.Forms.PictureBox();
    //        this.pnTiles = new System.Windows.Forms.Panel();
    //        this.pbTileModeNormal = new System.Windows.Forms.PictureBox();
    //        this.pbTileModeOver = new System.Windows.Forms.PictureBox();
    //        this.pbTileModeDown = new System.Windows.Forms.PictureBox();
    //        this.pbTileModeNormal2 = new System.Windows.Forms.PictureBox();
    //        this.pbTileModeOver2 = new System.Windows.Forms.PictureBox();
    //        this.pbTileModeDown2 = new System.Windows.Forms.PictureBox();
    //        this.pbCreateTileObjectNormal = new System.Windows.Forms.PictureBox();
    //        this.pbCreateTileObjectOver = new System.Windows.Forms.PictureBox();
    //        this.pbCreateTileObjectDown = new System.Windows.Forms.PictureBox();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTiles)).BeginInit();
    //        this.pnAddNew.SuspendLayout();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbCreateObject)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileMode)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbMenuButton)).BeginInit();
    //        this.mnuPopup.SuspendLayout();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbEmptySpace)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbMenuNormal)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbMenuOver)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbMenuDown)).BeginInit();
    //        this.pnTiles.SuspendLayout();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeNormal)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeOver)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeDown)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeNormal2)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeOver2)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeDown2)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbCreateTileObjectNormal)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbCreateTileObjectOver)).BeginInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbCreateTileObjectDown)).BeginInit();
    //        this.SuspendLayout();
    //        // 
    //        // pbTiles
    //        // 
    //        this.pbTiles.BackColor = System.Drawing.Color.White;
    //        this.pbTiles.Location = new System.Drawing.Point(0, 0);
    //        this.pbTiles.Name = "pbTiles";
    //        this.pbTiles.Size = new System.Drawing.Size(257, 422);
    //        this.pbTiles.TabIndex = 0;
    //        this.pbTiles.TabStop = false;
    //        this.pbTiles.Click += new System.EventHandler(this.pbTiles_Click);
    //        this.pbTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTiles_Paint);
    //        this.pbTiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTiles_MouseDown);
    //        this.pbTiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTiles_MouseMove);
    //        this.pbTiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbTiles_MouseUp);
    //        // 
    //        // vScrollBar1
    //        // 
    //        this.vScrollBar1.Location = new System.Drawing.Point(257, 0);
    //        this.vScrollBar1.Name = "vScrollBar1";
    //        this.vScrollBar1.Size = new System.Drawing.Size(16, 422);
    //        this.vScrollBar1.TabIndex = 1;
    //        this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
    //        // 
    //        // pnAddNew
    //        // 
    //        this.pnAddNew.BackColor = System.Drawing.Color.White;
    //        this.pnAddNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnAddNew.BackgroundImage")));
    //        this.pnAddNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
    //        this.pnAddNew.Controls.Add(this.pbCreateObject);
    //        this.pnAddNew.Controls.Add(this.pbTileMode);
    //        this.pnAddNew.Controls.Add(this.pbMenuButton);
    //        this.pnAddNew.Controls.Add(this.btnPopOut);
    //        this.pnAddNew.Location = new System.Drawing.Point(0, 0);
    //        this.pnAddNew.Name = "pnAddNew";
    //        this.pnAddNew.Size = new System.Drawing.Size(273, 22);
    //        this.pnAddNew.TabIndex = 5;
    //        // 
    //        // pbCreateObject
    //        // 
    //        this.pbCreateObject.Location = new System.Drawing.Point(48, 0);
    //        this.pbCreateObject.Name = "pbCreateObject";
    //        this.pbCreateObject.Size = new System.Drawing.Size(21, 20);
    //        this.pbCreateObject.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbCreateObject.TabIndex = 30;
    //        this.pbCreateObject.TabStop = false;
    //        this.pbCreateObject.Click += new System.EventHandler(this.pbCreateObject_Click);
    //        this.pbCreateObject.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCreateObject_Paint);
    //        this.pbCreateObject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCreateObject_MouseDown);
    //        this.pbCreateObject.MouseLeave += new System.EventHandler(this.pbCreateObject_MouseLeave);
    //        this.pbCreateObject.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbCreateObject_MouseMove);
    //        this.pbCreateObject.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbCreateObject_MouseUp);
    //        // 
    //        // pbTileMode
    //        // 
    //        this.pbTileMode.Location = new System.Drawing.Point(24, 0);
    //        this.pbTileMode.Name = "pbTileMode";
    //        this.pbTileMode.Size = new System.Drawing.Size(21, 20);
    //        this.pbTileMode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbTileMode.TabIndex = 29;
    //        this.pbTileMode.TabStop = false;
    //        this.pbTileMode.Click += new System.EventHandler(this.pbTileMode_Click);
    //        this.pbTileMode.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTileMode_Paint);
    //        this.pbTileMode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbTileMode_MouseDown);
    //        this.pbTileMode.MouseLeave += new System.EventHandler(this.pbTileMode_MouseLeave);
    //        this.pbTileMode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbTileMode_MouseMove);
    //        this.pbTileMode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbTileMode_MouseUp);
    //        // 
    //        // pbMenuButton
    //        // 
    //        this.pbMenuButton.Location = new System.Drawing.Point(0, 0);
    //        this.pbMenuButton.Name = "pbMenuButton";
    //        this.pbMenuButton.Size = new System.Drawing.Size(21, 20);
    //        this.pbMenuButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbMenuButton.TabIndex = 28;
    //        this.pbMenuButton.TabStop = false;
    //        this.pbMenuButton.Click += new System.EventHandler(this.pbMenuButton_Click);
    //        this.pbMenuButton.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMenuButton_Paint);
    //        this.pbMenuButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMenuButton_MouseDown);
    //        this.pbMenuButton.MouseLeave += new System.EventHandler(this.pbMenuButton_MouseLeave);
    //        this.pbMenuButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMenuButton_MouseMove);
    //        this.pbMenuButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbMenuButton_MouseUp);
    //        // 
    //        // btnPopOut
    //        // 
    //        this.btnPopOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
    //        this.btnPopOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
    //        this.btnPopOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
    //        this.btnPopOut.Image = ((System.Drawing.Image)(resources.GetObject("btnPopOut.Image")));
    //        this.btnPopOut.Location = new System.Drawing.Point(206, -2);
    //        this.btnPopOut.Name = "btnPopOut";
    //        this.btnPopOut.Size = new System.Drawing.Size(20, 20);
    //        this.btnPopOut.TabIndex = 7;
    //        this.btnPopOut.UseVisualStyleBackColor = false;
    //        this.btnPopOut.Visible = false;
    //        // 
    //        // hScrollBar1
    //        // 
    //        this.hScrollBar1.Location = new System.Drawing.Point(0, 422);
    //        this.hScrollBar1.Name = "hScrollBar1";
    //        this.hScrollBar1.Size = new System.Drawing.Size(257, 16);
    //        this.hScrollBar1.TabIndex = 7;
    //        this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
    //        // 
    //        // mnuPopup
    //        // 
    //        this.mnuPopup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
    //        this.selectImageToolStripMenuItem});
    //        this.mnuPopup.Name = "mnuPopup";
    //        this.mnuPopup.Size = new System.Drawing.Size(142, 26);
    //        // 
    //        // selectImageToolStripMenuItem
    //        // 
    //        this.selectImageToolStripMenuItem.Enabled = false;
    //        this.selectImageToolStripMenuItem.Name = "selectImageToolStripMenuItem";
    //        this.selectImageToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
    //        this.selectImageToolStripMenuItem.Text = "Select Image";
    //        this.selectImageToolStripMenuItem.Visible = false;
    //        this.selectImageToolStripMenuItem.Click += new System.EventHandler(this.selectImageToolStripMenuItem_Click);
    //        // 
    //        // pbEmptySpace
    //        // 
    //        this.pbEmptySpace.Location = new System.Drawing.Point(257, 423);
    //        this.pbEmptySpace.Name = "pbEmptySpace";
    //        this.pbEmptySpace.Size = new System.Drawing.Size(49, 30);
    //        this.pbEmptySpace.TabIndex = 8;
    //        this.pbEmptySpace.TabStop = false;
    //        // 
    //        // pbMenuNormal
    //        // 
    //        this.pbMenuNormal.Image = ((System.Drawing.Image)(resources.GetObject("pbMenuNormal.Image")));
    //        this.pbMenuNormal.Location = new System.Drawing.Point(316, 22);
    //        this.pbMenuNormal.Name = "pbMenuNormal";
    //        this.pbMenuNormal.Size = new System.Drawing.Size(21, 20);
    //        this.pbMenuNormal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbMenuNormal.TabIndex = 31;
    //        this.pbMenuNormal.TabStop = false;
    //        this.pbMenuNormal.Visible = false;
    //        // 
    //        // pbMenuOver
    //        // 
    //        this.pbMenuOver.Image = ((System.Drawing.Image)(resources.GetObject("pbMenuOver.Image")));
    //        this.pbMenuOver.Location = new System.Drawing.Point(316, 48);
    //        this.pbMenuOver.Name = "pbMenuOver";
    //        this.pbMenuOver.Size = new System.Drawing.Size(21, 20);
    //        this.pbMenuOver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbMenuOver.TabIndex = 29;
    //        this.pbMenuOver.TabStop = false;
    //        this.pbMenuOver.Visible = false;
    //        // 
    //        // pbMenuDown
    //        // 
    //        this.pbMenuDown.Image = ((System.Drawing.Image)(resources.GetObject("pbMenuDown.Image")));
    //        this.pbMenuDown.Location = new System.Drawing.Point(316, 74);
    //        this.pbMenuDown.Name = "pbMenuDown";
    //        this.pbMenuDown.Size = new System.Drawing.Size(21, 20);
    //        this.pbMenuDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbMenuDown.TabIndex = 30;
    //        this.pbMenuDown.TabStop = false;
    //        this.pbMenuDown.Visible = false;
    //        // 
    //        // pnTiles
    //        // 
    //        this.pnTiles.Controls.Add(this.pbTiles);
    //        this.pnTiles.Controls.Add(this.vScrollBar1);
    //        this.pnTiles.Controls.Add(this.hScrollBar1);
    //        this.pnTiles.Controls.Add(this.pbEmptySpace);
    //        this.pnTiles.Location = new System.Drawing.Point(0, 22);
    //        this.pnTiles.Name = "pnTiles";
    //        this.pnTiles.Size = new System.Drawing.Size(310, 458);
    //        this.pnTiles.TabIndex = 32;
    //        // 
    //        // pbTileModeNormal
    //        // 
    //        this.pbTileModeNormal.Image = global::FiremelonEditor2.Properties.Resources.TileModeNormal;
    //        this.pbTileModeNormal.Location = new System.Drawing.Point(316, 100);
    //        this.pbTileModeNormal.Name = "pbTileModeNormal";
    //        this.pbTileModeNormal.Size = new System.Drawing.Size(21, 20);
    //        this.pbTileModeNormal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbTileModeNormal.TabIndex = 35;
    //        this.pbTileModeNormal.TabStop = false;
    //        this.pbTileModeNormal.Visible = false;
    //        // 
    //        // pbTileModeOver
    //        // 
    //        this.pbTileModeOver.Image = global::FiremelonEditor2.Properties.Resources.TileModeOver;
    //        this.pbTileModeOver.Location = new System.Drawing.Point(316, 126);
    //        this.pbTileModeOver.Name = "pbTileModeOver";
    //        this.pbTileModeOver.Size = new System.Drawing.Size(21, 20);
    //        this.pbTileModeOver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbTileModeOver.TabIndex = 33;
    //        this.pbTileModeOver.TabStop = false;
    //        this.pbTileModeOver.Visible = false;
    //        // 
    //        // pbTileModeDown
    //        // 
    //        this.pbTileModeDown.Image = global::FiremelonEditor2.Properties.Resources.TileModeDown;
    //        this.pbTileModeDown.Location = new System.Drawing.Point(316, 152);
    //        this.pbTileModeDown.Name = "pbTileModeDown";
    //        this.pbTileModeDown.Size = new System.Drawing.Size(21, 20);
    //        this.pbTileModeDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbTileModeDown.TabIndex = 34;
    //        this.pbTileModeDown.TabStop = false;
    //        this.pbTileModeDown.Visible = false;
    //        // 
    //        // pbTileModeNormal2
    //        // 
    //        this.pbTileModeNormal2.Image = global::FiremelonEditor2.Properties.Resources.TileMode2Normal;
    //        this.pbTileModeNormal2.Location = new System.Drawing.Point(316, 178);
    //        this.pbTileModeNormal2.Name = "pbTileModeNormal2";
    //        this.pbTileModeNormal2.Size = new System.Drawing.Size(21, 20);
    //        this.pbTileModeNormal2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbTileModeNormal2.TabIndex = 38;
    //        this.pbTileModeNormal2.TabStop = false;
    //        this.pbTileModeNormal2.Visible = false;
    //        // 
    //        // pbTileModeOver2
    //        // 
    //        this.pbTileModeOver2.Image = global::FiremelonEditor2.Properties.Resources.TileMode2Over;
    //        this.pbTileModeOver2.Location = new System.Drawing.Point(316, 204);
    //        this.pbTileModeOver2.Name = "pbTileModeOver2";
    //        this.pbTileModeOver2.Size = new System.Drawing.Size(21, 20);
    //        this.pbTileModeOver2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbTileModeOver2.TabIndex = 36;
    //        this.pbTileModeOver2.TabStop = false;
    //        this.pbTileModeOver2.Visible = false;
    //        // 
    //        // pbTileModeDown2
    //        // 
    //        this.pbTileModeDown2.Image = global::FiremelonEditor2.Properties.Resources.TileMode2Down;
    //        this.pbTileModeDown2.Location = new System.Drawing.Point(316, 230);
    //        this.pbTileModeDown2.Name = "pbTileModeDown2";
    //        this.pbTileModeDown2.Size = new System.Drawing.Size(21, 20);
    //        this.pbTileModeDown2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbTileModeDown2.TabIndex = 37;
    //        this.pbTileModeDown2.TabStop = false;
    //        this.pbTileModeDown2.Visible = false;
    //        // 
    //        // pbCreateTileObjectNormal
    //        // 
    //        this.pbCreateTileObjectNormal.Image = ((System.Drawing.Image)(resources.GetObject("pbCreateTileObjectNormal.Image")));
    //        this.pbCreateTileObjectNormal.Location = new System.Drawing.Point(316, 256);
    //        this.pbCreateTileObjectNormal.Name = "pbCreateTileObjectNormal";
    //        this.pbCreateTileObjectNormal.Size = new System.Drawing.Size(21, 20);
    //        this.pbCreateTileObjectNormal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbCreateTileObjectNormal.TabIndex = 41;
    //        this.pbCreateTileObjectNormal.TabStop = false;
    //        this.pbCreateTileObjectNormal.Visible = false;
    //        // 
    //        // pbCreateTileObjectOver
    //        // 
    //        this.pbCreateTileObjectOver.Image = ((System.Drawing.Image)(resources.GetObject("pbCreateTileObjectOver.Image")));
    //        this.pbCreateTileObjectOver.Location = new System.Drawing.Point(316, 282);
    //        this.pbCreateTileObjectOver.Name = "pbCreateTileObjectOver";
    //        this.pbCreateTileObjectOver.Size = new System.Drawing.Size(21, 20);
    //        this.pbCreateTileObjectOver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbCreateTileObjectOver.TabIndex = 39;
    //        this.pbCreateTileObjectOver.TabStop = false;
    //        this.pbCreateTileObjectOver.Visible = false;
    //        // 
    //        // pbCreateTileObjectDown
    //        // 
    //        this.pbCreateTileObjectDown.Image = ((System.Drawing.Image)(resources.GetObject("pbCreateTileObjectDown.Image")));
    //        this.pbCreateTileObjectDown.Location = new System.Drawing.Point(316, 308);
    //        this.pbCreateTileObjectDown.Name = "pbCreateTileObjectDown";
    //        this.pbCreateTileObjectDown.Size = new System.Drawing.Size(21, 20);
    //        this.pbCreateTileObjectDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
    //        this.pbCreateTileObjectDown.TabIndex = 40;
    //        this.pbCreateTileObjectDown.TabStop = false;
    //        this.pbCreateTileObjectDown.Visible = false;
    //        // 
    //        // TilesetControl
    //        // 
    //        this.BackColor = System.Drawing.SystemColors.Control;
    //        this.Controls.Add(this.pbCreateTileObjectNormal);
    //        this.Controls.Add(this.pbCreateTileObjectOver);
    //        this.Controls.Add(this.pbCreateTileObjectDown);
    //        this.Controls.Add(this.pbTileModeNormal2);
    //        this.Controls.Add(this.pbTileModeOver2);
    //        this.Controls.Add(this.pbTileModeDown2);
    //        this.Controls.Add(this.pbTileModeNormal);
    //        this.Controls.Add(this.pbTileModeOver);
    //        this.Controls.Add(this.pbTileModeDown);
    //        this.Controls.Add(this.pnTiles);
    //        this.Controls.Add(this.pbMenuNormal);
    //        this.Controls.Add(this.pbMenuOver);
    //        this.Controls.Add(this.pbMenuDown);
    //        this.Controls.Add(this.pnAddNew);
    //        this.DoubleBuffered = true;
    //        this.Name = "TilesetControl";
    //        this.Size = new System.Drawing.Size(344, 649);
    //        this.Load += new System.EventHandler(this.TilesetControl_Load);
    //        this.Resize += new System.EventHandler(this.TilesetControl_Resize);
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTiles)).EndInit();
    //        this.pnAddNew.ResumeLayout(false);
    //        this.pnAddNew.PerformLayout();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbCreateObject)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileMode)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbMenuButton)).EndInit();
    //        this.mnuPopup.ResumeLayout(false);
    //        ((System.ComponentModel.ISupportInitialize)(this.pbEmptySpace)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbMenuNormal)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbMenuOver)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbMenuDown)).EndInit();
    //        this.pnTiles.ResumeLayout(false);
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeNormal)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeOver)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeDown)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeNormal2)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeOver2)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbTileModeDown2)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbCreateTileObjectNormal)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbCreateTileObjectOver)).EndInit();
    //        ((System.ComponentModel.ISupportInitialize)(this.pbCreateTileObjectDown)).EndInit();
    //        this.ResumeLayout(false);
    //        this.PerformLayout();

    //    }
    //    #endregion

    //    private void TilesetControl_Resize(object sender, System.EventArgs e)
    //    {
    //        resizeTileset();
    //    }

    //    private void resizeTileset()
    //    {
    //        try
    //        {
    //            generateBackground_ = true;

    //            ProjectDto project = projectController_.GetProjectDto();
    //            ProjectUiStateDto uiState = projectController_.GetUiState();

    //            int tileSize = project.TileSize;

    //            int selectedRoomIndex = uiState.SelectedRoomIndex;
    //            Guid selectedRoomId = uiState.SelectedRoomId;

    //            vScrollBar1.Left = this.Width - vScrollBar1.Width;
    //            vScrollBar1.Height = this.Height - pnAddNew.Height - hScrollBar1.Height;

    //            pbEmptySpace.Left = vScrollBar1.Left;
    //            pbEmptySpace.Top = vScrollBar1.Bottom;

    //            pbTiles.Height = this.Height - pnAddNew.Height - hScrollBar1.Height;
    //            pbTiles.Width = this.Width - vScrollBar1.Width;

    //            pnTiles.Height = this.Height - pnAddNew.Height;
    //            pnTiles.Width = this.Width;

    //            hScrollBar1.Top = pbTiles.Bottom;
    //            hScrollBar1.Width = pbTiles.Width;

    //            pnAddNew.Width = this.Width;

    //            btnPopOut.Left = this.Width - 26;

    //            bool isOkay = false;

    //            int imageRows = 0;
    //            int imageCols = 0;

    //            if (uiState.CollisionMode[selectedRoomId] == true)
    //            {
    //                imageRows = project.CollisionTileSheet.Rows;
    //                imageCols = project.CollisionTileSheet.Columns;

    //                isOkay = true;
    //            }
    //            else
    //            {
    //                Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

    //                if (tileSheetId != Guid.Empty)
    //                {
    //                    TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

    //                    imageRows = tileSheet.Rows;
    //                    imageCols = tileSheet.Columns;

    //                    isOkay = true;
    //                }
    //            }

    //            if (isOkay == true)
    //            {
    //                int visibleCols = 0;
    //                int visibleRows = 0;

    //                calculateRenderData(ref visibleCols, ref visibleRows);

    //                // If there is whitespace and the rowOffset is not zero and the offset is also not zero, drag the tiles down to fill the whitespace.
    //                // Adjust the variables and scrollbar accordingly.       

    //                bool adjustScrollValues = false;
    //                int whiteSpaceHeight = 0;

    //                if (rowPixelOffset_ == 0)
    //                {
    //                    whiteSpaceHeight = pbTiles.Height - ((imageRows - rowOffset_) * tileSize);

    //                    if (whiteSpaceHeight > 0 && (rowOffset_ > 0 || (rowOffset_ == 0 && rowPixelOffset_ > 0)))
    //                    {
    //                        adjustScrollValues = true;
    //                    }
    //                }
    //                else
    //                {
    //                    whiteSpaceHeight = pbTiles.Height - ((tileSize - rowPixelOffset_) + ((imageRows - (rowOffset_ + 1)) * tileSize));

    //                    if (whiteSpaceHeight > 0 && (rowOffset_ > 0 || (rowOffset_ == 0 && rowPixelOffset_ > 0)))
    //                    {
    //                        adjustScrollValues = true;
    //                    }
    //                }

    //                if (adjustScrollValues == true)
    //                {
    //                    // "Pull down" the tiles so that they occupy the whitespace. Adjust offsets and scrollbar accordingly.

    //                    // Calculate from the whitespace height, how many new rows can fit, and then what the leftover offset will be.
    //                    int rowAdjust = whiteSpaceHeight / tileSize;

    //                    int offsetAdjust = whiteSpaceHeight % tileSize;

    //                    rowOffset_ -= rowAdjust;

    //                    if (rowOffset_ < 0)
    //                    {
    //                        rowOffset_ = 0;
    //                        rowPixelOffset_ = 0;
    //                    }
    //                    else
    //                    {
    //                        rowPixelOffset_ -= offsetAdjust;

    //                        if (rowPixelOffset_ < 0)
    //                        {
    //                            if (rowOffset_ == 0)
    //                            {
    //                                rowPixelOffset_ = 0;
    //                            }
    //                            else
    //                            {
    //                                rowPixelOffset_ = tileSize - 1;
    //                            }

    //                            if (rowOffset_ > 0)
    //                            {
    //                                rowOffset_--;
    //                            }
    //                        }
    //                    }
    //                }

    //                // Do the same for columns.
    //                adjustScrollValues = false;
    //                int whiteSpaceWidth = 0;

    //                if (colPixelOffset_ == 0)
    //                {
    //                    whiteSpaceWidth = (pbTiles.Width) - ((imageCols - colOffset_) * tileSize);

    //                    if (whiteSpaceWidth > 0 && (colOffset_ > 0 || (colOffset_ == 0 && colPixelOffset_ > 0)))
    //                    {
    //                        adjustScrollValues = true;
    //                    }
    //                }
    //                else
    //                {
    //                    whiteSpaceWidth = pbTiles.Width - ((tileSize - colPixelOffset_) + ((imageCols - (colOffset_ + 1)) * tileSize));

    //                    if (whiteSpaceWidth > 0 && (colOffset_ > 0 || (colOffset_ == 0 && colPixelOffset_ > 0)))
    //                    {
    //                        adjustScrollValues = true;
    //                    }
    //                }

    //                if (adjustScrollValues == true)
    //                {
    //                    // "Pull down" the tiles so that they occupy the whitespace. Adjust offsets and scrollbar accordingly.

    //                    // Calculate from the whitespace width, how many new cols can fit, and then what the leftover offset will be.
    //                    int colAdjust = whiteSpaceWidth / tileSize;

    //                    int offsetAdjust = whiteSpaceWidth % tileSize;

    //                    colOffset_ -= colAdjust;

    //                    if (colOffset_ < 0)
    //                    {
    //                        colOffset_ = 0;
    //                        colPixelOffset_ = 0;
    //                    }
    //                    else
    //                    {
    //                        colPixelOffset_ -= offsetAdjust;

    //                        if (colPixelOffset_ < 0)
    //                        {
    //                            if (colOffset_ == 0)
    //                            {
    //                                colPixelOffset_ = 0;
    //                            }
    //                            else
    //                            {
    //                                colPixelOffset_ = tileSize - 1;
    //                            }

    //                            if (colOffset_ > 0)
    //                            {
    //                                colOffset_--;
    //                            }
    //                        }
    //                    }
    //                }

    //                calculateRenderData(ref visibleCols, ref visibleRows);

    //                int offscreenCols = imageCols - visibleCols;

    //                if (offscreenCols < 0)
    //                {
    //                    offscreenCols = 0;
    //                }

    //                int offscreenRows = imageRows - visibleRows;

    //                if (offscreenRows < 0)
    //                {
    //                    offscreenRows = 0;
    //                }

    //                hScrollBar1.Maximum = offscreenCols;
    //                vScrollBar1.Maximum = offscreenRows;
    //            }
    //            else
    //            {
    //                //hScrollBar1.Value = 0;
    //                //vScrollBar1.Value = 0;
    //                hScrollBar1.Maximum = 0;
    //                vScrollBar1.Maximum = 0;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }

    //    private void pbTiles_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
    //    {
    //        ProjectDto project = projectController_.GetProjectDto();
    //        ProjectUiStateDto uiState = projectController_.GetUiState();
    //        ProjectResourcesDto resources = projectController_.GetResources();

    //        Graphics g = e.Graphics;

    //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

    //        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

    //        if (project != null && project.IsPrepared == true)
    //        {
    //            if (generateBackground_ == true || bmpBackground_ == null)
    //            {
    //                // Dispose of the old background object.
    //                if (bmpBackground_ != null)
    //                {
    //                    bmpBackground_.Dispose();
    //                    bmpBackground_ = null;
    //                }

    //                bmpBackground_ = backgroundGenerator_.GenerateBackground(pbTiles.Width, pbTiles.Height);

    //                generateBackground_ = false;
    //            }

    //            g.DrawImageUnscaled(bmpBackground_, new Point(0, 0));

    //            int tileSize = project.TileSize;

    //            bool isOkay = false;

    //            Bitmap bitmapToRender = null;

    //            Point renderPoint = new Point(0, 0);

    //            int selectedRoomIndex = uiState.SelectedRoomIndex;
    //            Guid selectedRoomId = uiState.SelectedRoomId;

    //            int selectedTile = -1;
    //            int imageCols = 0;
    //            int tileCount = 0;

    //            TileSheetDto tileSheet = null;

    //            if (uiState.CollisionMode[selectedRoomId] == true)
    //            {
    //                BitmapResourceDto bitmapResource = resources.CollisionTilesBitmap;

    //                tileSheet = project.CollisionTileSheet;

    //                tileCount = bitmapResource.SpriteSheetImageList.Count;
    //                imageCols = project.CollisionTileSheet.Columns;
    //                selectedTile = uiState.SelectedCollisionTileIndex[selectedRoomId];
                    
    //                bitmapToRender = bitmapResource.BitmapImageWithTransparency;
                    
    //                isOkay = true;
    //            }
    //            else
    //            {
    //                Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

    //                if (tileSheetId != Guid.Empty)
    //                {
    //                    tileSheet = projectController_.GetTileSheet(tileSheetId);

    //                    Guid resourceId = tileSheet.BitmapResourceId;

    //                    BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];

    //                    tileCount = bitmapResource.SpriteSheetImageList.Count;
    //                    imageCols = tileSheet.Columns;
    //                    selectedTile = uiState.SelectedTileIndex[selectedRoomId];
                        
    //                    bitmapToRender = bitmapResource.BitmapImageWithTransparency;

    //                    isOkay = true;
    //                }
    //            }

    //            if (isOkay == true)
    //            {
    //                int x = -1 * ((colOffset_ * tileSize) + colPixelOffset_);
    //                int y = -1 * ((rowOffset_ * tileSize) + rowPixelOffset_);

    //                renderPoint = new Point(x, y);
                    
    //                int sourceWidth = bitmapToRender.Width;

    //                int sourceHeight = bitmapToRender.Height;

    //                float scaleFactor = 1.0f;

    //                if (tileSheet != null)
    //                {
    //                    scaleFactor = tileSheet.ScaleFactor;
    //                }

    //                // Scale the destination by the scaling factor.
    //                int destinationWidth = (int)(sourceWidth * scaleFactor);

    //                int destinationHeight = (int)(sourceHeight * scaleFactor);

    //                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(renderPoint.X, renderPoint.Y, destinationWidth, destinationHeight);

    //                g.DrawImage(bitmapToRender, destRect, 0, 0, bitmapToRender.Width, bitmapToRender.Height, GraphicsUnit.Pixel, null);
                    
    //                //g.DrawImageUnscaled(bitmapToRender, renderPoint);

    //                int visibleRows = 0;
    //                int visibleCols = 0;

    //                calculateRenderData(ref visibleCols, ref visibleRows);

    //                Pen p = new Pen(Color.Black);
    //                Pen pSelect = new Pen(Color.Orange, 2.0f);
                    
    //                // Draw the rectangle around the selected tile.
    //                if (uiState.TilesetActive[selectedRoomId] == true)
    //                {
    //                    if (selectedTile >= 0)
    //                    {
    //                        if (selectedTile <= tileCount)
    //                        {
    //                            int tileX = ((selectedTile % imageCols) * tileSize) - (colOffset_ * tileSize) - colPixelOffset_;
    //                            int tileY = (Convert.ToInt32(Math.Floor(Convert.ToDouble(selectedTile / imageCols))) * tileSize) + (rowOffset_ * -tileSize) - rowPixelOffset_;

    //                            g.DrawRectangle(pSelect, tileX, tileY, tileSize, tileSize);
    //                        }
    //                    }
                        
    //                    // If there is a selection rectangle active, draw the selector corners.
    //                    if (selectionCorner1TileId_ != selectionCorner2TileId_)
    //                    {
    //                        int cornerTile1X = ((selectionCorner1TileId_ % imageCols) * tileSize) - (colOffset_ * tileSize) - colPixelOffset_;
    //                        int cornerTile1Y = (Convert.ToInt32(Math.Floor(Convert.ToDouble(selectionCorner1TileId_ / imageCols))) * tileSize) + (rowOffset_ * -tileSize) - rowPixelOffset_;

    //                        int cornerTile2X = ((selectionCorner2TileId_ % imageCols) * tileSize) - (colOffset_ * tileSize) - colPixelOffset_;
    //                        int cornerTile2Y = (Convert.ToInt32(Math.Floor(Convert.ToDouble(selectionCorner2TileId_ / imageCols))) * tileSize) + (rowOffset_ * -tileSize) - rowPixelOffset_;

    //                        int selectorTop = 0;
    //                        int selectorLeft = 0;
    //                        int selectorWidth = 0;
    //                        int selectorHeight = 0;

    //                        // Determine the selector bounds.
    //                        if (cornerTile1X < cornerTile2X)
    //                        {
    //                            selectorLeft = cornerTile1X;
    //                            selectorWidth = (cornerTile2X - cornerTile1X) + tileSize;
    //                        }
    //                        else
    //                        {
    //                            selectorLeft = cornerTile2X;
    //                            selectorWidth = (cornerTile1X - cornerTile2X) + tileSize;
    //                        }

    //                        if (cornerTile1Y < cornerTile2Y)
    //                        {
    //                            selectorTop = cornerTile1Y;
    //                            selectorHeight = (cornerTile2Y - cornerTile1Y) + tileSize;
    //                        }
    //                        else
    //                        {
    //                            selectorTop = cornerTile2Y;
    //                            selectorHeight = (cornerTile1Y - cornerTile2Y) + tileSize;
    //                        }
                            
    //                        g.DrawRectangle(pSelect, selectorLeft, selectorTop, selectorWidth, selectorHeight);
    //                    }


    //                }

    //                p.Dispose();
    //            }
    //        }
    //    }

    //    public void TilesetControl_RefreshView(object sender, RefreshViewEventArgs e)
    //    {
    //        pbTiles.Refresh();
    //    }

    //    public void TilesetControl_RoomSelected(object sender, RoomSelectedEventArgs e)
    //    {
    //        pbTiles.Refresh();

    //        resizeTileset();
    //    }

    //    private void TilesetControl_ObjectNameEntered(object sender, NameEnteredEventArgs e)
    //    {
    //        ProjectDto project = projectController_.GetProjectDto();
            
    //        string tileObjectName = e.Name;

    //        ProjectUiStateDto uiState = projectController_.GetUiState();

    //        int selectedRoomIndex = uiState.SelectedRoomIndex;
    //        Guid selectedRoomId = uiState.SelectedRoomId;

    //        Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

    //        if (tileSheetId != Guid.Empty)
    //        {
    //            TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

    //            int imageCols = tileSheet.Columns;

    //            int left = 0;
    //            int top = 0;
    //            int width = 0;
    //            int height = 0;

    //            getSelectorRectData(selectionCorner1TileId_, selectionCorner2TileId_, imageCols, ref left, ref top, ref width, ref height);

    //            Rectangle tileObjectRect = new Rectangle(left, top, width, height);

    //            bool success = projectController_.CreateObjectFromTileSheet(selectedRoomIndex, tileObjectName, tileObjectRect);

    //            if (success == false)
    //            {
    //                e.Cancel = true;
    //            }
    //        }
    //    }

    //    public void TilesetControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
    //    {
    //    }

    //    public void TilesetControl_CollisionModeChanged(object sender, CollisionModeChangedEventArgs e)
    //    {
    //        resizeTileset();
    //    }

    //    private void calculateRenderData(ref int visibleCols, ref int visibleRows)
    //    {
    //        ProjectDto project = projectController_.GetProjectDto();
    //        ProjectUiStateDto uiState = projectController_.GetUiState();

    //        int selectedRoomIndex = uiState.SelectedRoomIndex;
    //        Guid selectedRoomId = uiState.SelectedRoomId;

    //        int temp = 0;
    //        int tempSize = 0;
    //        int tileSize = project.TileSize;

    //        int imageCols = 0;
    //        int imageRows = 0;

    //        if (uiState.CollisionMode[selectedRoomId] == true)
    //        {
    //            imageCols = project.CollisionTileSheet.Columns;
    //            imageRows = project.CollisionTileSheet.Rows;
    //        }
    //        else
    //        {
    //            Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;
    //            TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

    //            imageCols = tileSheet.Columns;
    //            imageRows = tileSheet.Rows;
    //        }

    //        if (colPixelOffset_ > 0)
    //        {
    //            temp = 1;
    //            tempSize = tileSize;
    //        }

    //        visibleCols = 0;

    //        int size = tempSize - colPixelOffset_;

    //        for (int i = colOffset_ + temp; i < imageCols; i++)
    //        {
    //            size += tileSize;

    //            if (size > pbTiles.Width)
    //            {
    //                break;
    //            }

    //            visibleCols++;
    //        }

    //        // The number of rows that are fully visible...
    //        temp = 0;
    //        tempSize = 0;

    //        if (rowPixelOffset_ > 0)
    //        {
    //            temp = 1;
    //            tempSize = tileSize;
    //        }

    //        visibleRows = 0;

    //        size = tempSize - rowPixelOffset_;

    //        for (int i = rowOffset_ + temp; i < imageRows; i++)
    //        {
    //            size += tileSize;

    //            if (size > pbTiles.Height)
    //            {
    //                break;
    //            }

    //            visibleRows++;
    //        }

    //        return;
    //    }

    //    // Scroll through the tiles.
    //    private void vScrollBar1_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
    //    {
    //        try
    //        {
    //            ProjectDto project = projectController_.GetProjectDto();
    //            ProjectUiStateDto uiState = projectController_.GetUiState();

    //            int selectedRoomIndex = uiState.SelectedRoomIndex;
    //            Guid selectedRoomId = uiState.SelectedRoomId;

    //            int tileSize = project.TileSize;

    //            Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

    //            if (tileSheetId != Guid.Empty)
    //            {
    //                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

    //                int imageRows = tileSheet.Rows;

    //                if (e.OldValue != e.NewValue)
    //                {
    //                    int visibleCols = 0;
    //                    int visibleRows = 0;

    //                    calculateRenderData(ref visibleCols, ref visibleRows);

    //                    if (rowPixelOffset_ == 0)
    //                    {
    //                        if (e.NewValue == vScrollBar1.Maximum)
    //                        {
    //                            rowPixelOffset_ = ((visibleRows + 1) * tileSize) - pbTiles.Height;

    //                            rowOffset_ += rowPixelOffset_ / tileSize;

    //                            rowPixelOffset_ = rowPixelOffset_ % tileSize;

    //                            calculateRenderData(ref visibleCols, ref visibleRows);

    //                            int offscreenRows = imageRows - visibleRows;

    //                            if (offscreenRows < 0)
    //                            {
    //                                offscreenRows = 0;
    //                            }

    //                            e.NewValue = offscreenRows;

    //                            vScrollBar1.Maximum = offscreenRows;
    //                        }
    //                        else
    //                        {
    //                            rowOffset_ = e.NewValue;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (e.NewValue == 0)
    //                        {
    //                            rowPixelOffset_ = 0;
    //                            rowOffset_ = 0;
    //                        }
    //                        else if (e.NewValue == vScrollBar1.Maximum && (pbTiles.Height - ((tileSize - rowPixelOffset_) + (visibleRows * tileSize))) != 0)
    //                        {
    //                            // There is no row alignment. Need to modify the offset by some amount to bottom align the tiles.
    //                            rowPixelOffset_ += tileSize - (pbTiles.Height - ((tileSize - rowPixelOffset_) + (visibleRows * tileSize)));

    //                            calculateRenderData(ref visibleCols, ref visibleRows);

    //                            int offscreenRows = imageRows - visibleRows;

    //                            if (offscreenRows < 0)
    //                            {
    //                                offscreenRows = 0;
    //                            }

    //                            e.NewValue = offscreenRows;

    //                            vScrollBar1.Maximum = offscreenRows;
    //                        }
    //                        else
    //                        {
    //                            rowOffset_ = e.NewValue - 1;
    //                        }
    //                    }
    //                }
    //            }
    //            this.Refresh();
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }

    //    private void hScrollBar1_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
    //    {
    //        try
    //        {
    //            ProjectDto project = projectController_.GetProjectDto();
    //            ProjectUiStateDto uiState = projectController_.GetUiState();

    //            int selectedRoomIndex = uiState.SelectedRoomIndex;
    //            Guid selectedRoomId = uiState.SelectedRoomId;

    //            int tileSize = project.TileSize;

    //            Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

    //            if (tileSheetId != Guid.Empty)
    //            {
    //                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

    //                if (e.OldValue != e.NewValue)
    //                {
    //                    int imageCols = tileSheet.Columns;

    //                    int visibleCols = 0;
    //                    int visibleRows = 0;

    //                    calculateRenderData(ref visibleCols, ref visibleRows);

    //                    if (colPixelOffset_ == 0)
    //                    {
    //                        if (e.NewValue == hScrollBar1.Maximum)
    //                        {
    //                            colPixelOffset_ = ((visibleCols + 1) * tileSize) - pbTiles.Width;

    //                            colOffset_ += colPixelOffset_ / tileSize;

    //                            colPixelOffset_ = colPixelOffset_ % tileSize;

    //                            calculateRenderData(ref visibleCols, ref visibleRows);

    //                            int offscreenCols = imageCols - visibleCols;

    //                            if (offscreenCols < 0)
    //                            {
    //                                offscreenCols = 0;
    //                            }

    //                            e.NewValue = offscreenCols;

    //                            hScrollBar1.Maximum = offscreenCols;
    //                        }
    //                        else
    //                        {
    //                            colOffset_ = e.NewValue;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (e.NewValue == 0)
    //                        {
    //                            colPixelOffset_ = 0;
    //                            colOffset_ = 0;
    //                        }
    //                        else if (e.NewValue == hScrollBar1.Maximum && (pbTiles.Width - ((tileSize - colPixelOffset_) + (visibleCols * tileSize))) != 0)
    //                        {
    //                            // There is no row alignment. Need to modify the offset by some amount to bottom align the tiles.
    //                            colPixelOffset_ += tileSize - (pbTiles.Width - ((tileSize - colPixelOffset_) + (visibleCols * tileSize)));

    //                            calculateRenderData(ref visibleCols, ref visibleRows);

    //                            int offscreenCols = imageCols - visibleCols;

    //                            if (offscreenCols < 0)
    //                            {
    //                                offscreenCols = 0;
    //                            }

    //                            e.NewValue = offscreenCols;

    //                            hScrollBar1.Maximum = offscreenCols;
    //                        }
    //                        else
    //                        {
    //                            colOffset_ = e.NewValue - 1;
    //                        }
    //                    }
    //                }
    //            }

    //            this.Refresh();
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }

    //    // Clicking on the control selects a tile.
    //    private void pbTiles_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
    //    {
    //        isMouseDownTiles_ = true;
            
    //        ProjectDto project = projectController_.GetProjectDto();
    //        ProjectUiStateDto uiState = projectController_.GetUiState();
    //        ProjectResourcesDto resources = projectController_.GetResources();

    //        int selectedRoomIndex = uiState.SelectedRoomIndex;
    //        Guid selectedRoomId = uiState.SelectedRoomId;

    //        int tileSize = project.TileSize;

    //        bool isOkay = false;

    //        int imageCols = 0;
    //        int imageRows = 0;
    //        int tileCount = 0;

    //        if (uiState.CollisionMode[selectedRoomId] == true)
    //        {
    //            BitmapResourceDto bitmapResource = resources.CollisionTilesBitmap;
                
    //            imageCols = project.CollisionTileSheet.Columns;
    //            imageRows = project.CollisionTileSheet.Rows;
    //            tileCount = bitmapResource.SpriteSheetImageList.Count;

    //            isOkay = true;
    //        }
    //        else
    //        {
    //            Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

    //            if (tileSheetId != Guid.Empty)
    //            {
    //                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

    //                Guid resourceId = tileSheet.BitmapResourceId;

    //                BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];

    //                imageCols = tileSheet.Columns;
    //                imageRows = tileSheet.Rows;

    //                if (bitmapResource.SpriteSheetImageList.Count > 0)
    //                {
    //                    tileCount = bitmapResource.SpriteSheetImageList.Count;
    //                    isOkay = true;
    //                }
    //            }
    //        }

    //        if (isOkay == true)
    //        { 
    //            int visiblerows = 0;
    //            int visiblecols = 0;

    //            calculateRenderData(ref visiblecols, ref visiblerows);

    //            int clickedRow = ((e.Y + ((rowOffset_ * tileSize) + rowPixelOffset_)) / tileSize);
    //            int clickedCol = ((e.X + ((colOffset_ * tileSize) + colPixelOffset_)) / tileSize);
    //            int clickedTile = (clickedRow * imageCols) + clickedCol;

    //            selectionCorner1TileId_ = clickedTile;
    //            selectionCorner2TileId_ = clickedTile;
                
    //            int highestvisiblerow = 0;
    //            bool isbottomaligned = false;

    //            if (clickedRow < imageRows && clickedCol < imageCols)
    //            {
    //                // get the total Height of all visible rows, and the offset if needed, and compare it to the control Height, to see if the last row is partially visible or wholly visible.
    //                if (rowPixelOffset_ == 0)
    //                {
    //                    if ((visiblerows * tileSize) < pbTiles.Height)
    //                    {
    //                        // if the last visible row is partially visible...
    //                        highestvisiblerow = rowOffset_ + visiblerows;
    //                    }
    //                    else
    //                    {
    //                        highestvisiblerow = rowOffset_ + visiblerows - 1;

    //                        // if there's no offset and you can fit an exact amount of tiles in, it is bottom aligned.
    //                        if (pbTiles.Height % tileSize == 0)
    //                        {
    //                            isbottomaligned = true;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (((visiblerows * tileSize) + (tileSize - rowPixelOffset_)) < pbTiles.Height)
    //                    {
    //                        highestvisiblerow = rowOffset_ + visiblerows + 1;
    //                    }
    //                    else
    //                    {
    //                        isbottomaligned = true;
    //                        highestvisiblerow = rowOffset_ + visiblerows;
    //                    }
    //                }

    //                int highestVisibleCol = 0;
    //                bool isRightAligned = false;

    //                // get the total Width of all visible cols, and the offset if needed, and compare it to the control Width, to see if the last col is partially visible or wholly visible.
    //                if (colPixelOffset_ == 0)
    //                {
    //                    if ((visiblecols * tileSize) < pbTiles.Width)
    //                    {
    //                        // if the last visible row is partially visible...
    //                        highestVisibleCol = colOffset_ + visiblecols;
    //                    }
    //                    else
    //                    {
    //                        highestVisibleCol = colOffset_ + visiblecols - 1;

    //                        // if there's no offset and you can fit an exact amount of tiles in, it is bottom aligned.
    //                        if (pbTiles.Width % tileSize == 0)
    //                        {
    //                            isRightAligned = true;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (((visiblecols * tileSize) + (tileSize - colPixelOffset_)) < pbTiles.Width)
    //                    {
    //                        highestVisibleCol = colOffset_ + visiblecols + 1;
    //                    }
    //                    else
    //                    {
    //                        isRightAligned = true;
    //                        highestVisibleCol = colOffset_ + visiblecols;
    //                    }
    //                }

    //                if (e.Button == MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right)
    //                {
    //                    if (clickedTile <= tileCount - 1)
    //                    {
    //                        if (rowPixelOffset_ == 0)
    //                        {
    //                            if (clickedRow >= highestvisiblerow)
    //                            {
    //                                if (isbottomaligned == false)
    //                                {
    //                                    if (vScrollBar1.Value < vScrollBar1.Maximum)
    //                                    {
    //                                        vScrollBar1.Value++;
    //                                    }

    //                                    rowPixelOffset_ = ((visiblerows + 1) * tileSize) - pbTiles.Height;
    //                                }
    //                            }
    //                            else if (clickedRow == rowOffset_ && isbottomaligned == false)
    //                            {
    //                                rowPixelOffset_ = 0;
    //                                if (vScrollBar1.Value != vScrollBar1.Minimum)
    //                                {
    //                                    vScrollBar1.Value--;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (clickedRow == rowOffset_)
    //                            {
    //                                rowPixelOffset_ = 0;
    //                                vScrollBar1.Value--;

    //                                // aligning to the bottom may cause number of offscreen rows to change
    //                                calculateRenderData(ref visiblecols, ref visiblerows);

    //                                int offscreenrows = imageRows - visiblerows;

    //                                if (offscreenrows < 0)
    //                                {
    //                                    offscreenrows = 0;
    //                                }

    //                                vScrollBar1.Maximum = offscreenrows;

    //                            }
    //                            else if (clickedRow == highestvisiblerow)
    //                            {
    //                                // if the highest visible row is offscreen at all...
    //                                if (isbottomaligned == false)
    //                                {
    //                                    rowPixelOffset_ = ((visiblerows + 2) * tileSize) - pbTiles.Height;

    //                                    rowOffset_ += rowPixelOffset_ / tileSize;

    //                                    rowPixelOffset_ = rowPixelOffset_ % tileSize;

    //                                    vScrollBar1.Value = rowOffset_ + 1;

    //                                    // aligning to the bottom may cause number of offscreen rows to change
    //                                    calculateRenderData(ref visiblecols, ref visiblerows);

    //                                    int offscreenrows = imageRows - visiblerows;

    //                                    if (offscreenrows < 0)
    //                                    {
    //                                        offscreenrows = 0;
    //                                    }

    //                                    if (vScrollBar1.Value == vScrollBar1.Maximum && offscreenrows > vScrollBar1.Maximum)
    //                                    {
    //                                        vScrollBar1.Value -= offscreenrows - vScrollBar1.Maximum;
    //                                    }

    //                                    vScrollBar1.Maximum = offscreenrows;
    //                                }
    //                            }
    //                        }

    //                        if (colPixelOffset_ == 0)
    //                        {
    //                            if (clickedCol >= highestVisibleCol)
    //                            {
    //                                if (isRightAligned == false)
    //                                {
    //                                    if (hScrollBar1.Value < hScrollBar1.Maximum)
    //                                    {
    //                                        hScrollBar1.Value++;
    //                                    }

    //                                    colPixelOffset_ = ((visiblecols + 1) * tileSize) - pbTiles.Width;
    //                                }
    //                            }
    //                            else if (clickedCol == colOffset_ && isRightAligned == false)
    //                            {
    //                                colPixelOffset_ = 0;
    //                                if (hScrollBar1.Value != hScrollBar1.Minimum)
    //                                {
    //                                    hScrollBar1.Value--;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (clickedCol == colOffset_)
    //                            {
    //                                colPixelOffset_ = 0;
    //                                hScrollBar1.Value--;

    //                                // aligning to the bottom may cause number of offscreen cols to change
    //                                calculateRenderData(ref visiblecols, ref visiblerows);

    //                                int offscreencols = imageCols - visiblecols;

    //                                if (offscreencols < 0)
    //                                {
    //                                    offscreencols = 0;
    //                                }

    //                                hScrollBar1.Maximum = offscreencols;
    //                            }
    //                            else if (clickedCol == highestVisibleCol)
    //                            {
    //                                // if the highest visible col is offscreen at all...
    //                                if (isRightAligned == false)
    //                                {
    //                                    colPixelOffset_ = ((visiblecols + 2) * tileSize) - pbTiles.Width;

    //                                    colOffset_ += colPixelOffset_ / tileSize;

    //                                    colPixelOffset_ = colPixelOffset_ % tileSize;

    //                                    hScrollBar1.Value = colOffset_ + 1;

    //                                    // aligning to the bottom may cause number of offscreen cols to change
    //                                    calculateRenderData(ref visiblecols, ref visiblerows);

    //                                    int offscreencols = imageCols - visiblecols;

    //                                    if (offscreencols < 0)
    //                                    {
    //                                        offscreencols = 0;
    //                                    }

    //                                    if (hScrollBar1.Value == hScrollBar1.Maximum && offscreencols > hScrollBar1.Maximum)
    //                                    {
    //                                        hScrollBar1.Value -= offscreencols - hScrollBar1.Maximum;
    //                                    }

    //                                    hScrollBar1.Maximum = offscreencols;
    //                                }
    //                            }
    //                        }

    //                        if (uiState.CollisionMode[selectedRoomId] == true)
    //                        {
    //                            projectController_.SetSelectedCollisionTileIndex(selectedRoomIndex, clickedTile);
    //                        }
    //                        else
    //                        {
    //                            projectController_.SetSelectedTileIndex(selectedRoomIndex, clickedTile);
    //                        }

    //                        projectController_.SetDrawMode(selectedRoomIndex, DrawMode.DrawSingleTile);
    //                    }
    //                }
               
    //                this.Refresh();
    //            }
    //        }
    //    }

    //    private void TilesetControl_Load(object sender, EventArgs e)
    //    {
    //    }

    //    private void selectImageToolStripMenuItem_Click(object sender, EventArgs e)
    //    {
    //    }

    //    private void btnPopupMenu_Click(object sender, EventArgs e)
    //    {
    //        ProjectDto project = projectController_.GetProjectDto();

    //        mnuPopup.Items.Clear();

    //        if (project.TileSheets.Count == 0)
    //        {

    //            ToolStripMenuItem newMenuItem = new ToolStripMenuItem();

    //            newMenuItem.Text = "Add tile sheets in the asset editor";
    //            newMenuItem.Enabled = false;
    //            mnuPopup.Items.Add(newMenuItem);
    //        }

    //        for (int i = 0; i < project.TileSheets.Count; i++)
    //        {
    //            ToolStripMenuItem newMenuItem = new ToolStripMenuItem();

    //            newMenuItem.Text = project.TileSheets[i].Name;
    //            newMenuItem.Tag = i;
    //            newMenuItem.Click += mnuPopupItem_Click;
    //            mnuPopup.Items.Add(newMenuItem);
    //        }

    //        mnuPopup.Show(System.Windows.Forms.Cursor.Position);
    //    }

    //    private void mnuPopupItem_Click(object sender, EventArgs e)
    //    {
    //        ProjectDto project = projectController_.GetProjectDto();
    //        ProjectUiStateDto uiState = projectController_.GetUiState();

    //        int selectedRoomIndex = uiState.SelectedRoomIndex;

    //        ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

    //        int tileSheetIndex = Convert.ToInt32(menuItem.Tag);

    //        projectController_.SetTileSheet(selectedRoomIndex, tileSheetIndex);

    //        resizeTileset();

    //        pbTiles.Refresh();
    //    }

    //    private void pbMenuButton_Paint(object sender, PaintEventArgs e)
    //    {
    //        Graphics g = e.Graphics;

    //        if (isMouseDownButton_ == true)
    //        {
    //            g.DrawImageUnscaled(pbMenuDown.Image, 0, 0);
    //        }
    //        else if (isMouseOverButton_ == true)
    //        {
    //            g.DrawImageUnscaled(pbMenuOver.Image, 0, 0);
    //        }
    //        else
    //        {
    //            g.DrawImageUnscaled(pbMenuNormal.Image, 0, 0);
    //        }
    //    }

    //    private void pbMenuButton_MouseDown(object sender, MouseEventArgs e)
    //    {
    //        isMouseDownButton_ = true;

    //        pbMenuButton.Refresh();
    //    }

    //    private void pbMenuButton_MouseMove(object sender, MouseEventArgs e)
    //    {
    //        isMouseOverButton_ = true;

    //        pbMenuButton.Refresh();
    //    }

    //    private void pbMenuButton_MouseUp(object sender, MouseEventArgs e)
    //    {
    //        ProjectDto project = projectController_.GetProjectDto();

    //        isMouseDownButton_ = false;

    //        pbMenuButton.Refresh();

    //        mnuPopup.Items.Clear();

    //        if (project.TileSheets.Count == 0)
    //        {
    //            ToolStripMenuItem newMenuItem = new ToolStripMenuItem();

    //            newMenuItem.Text = "Add tile sheets in the asset editor";
    //            newMenuItem.Enabled = false;
    //            mnuPopup.Items.Add(newMenuItem);
    //        }

    //        for (int i = 0; i < project.TileSheets.Count; i++)
    //        {
    //            ToolStripMenuItem newMenuItem = new ToolStripMenuItem();

    //            newMenuItem.Text = project.TileSheets[i].Name;
    //            newMenuItem.Tag = i;
    //            newMenuItem.Click += mnuPopupItem_Click;
    //            mnuPopup.Items.Add(newMenuItem);
    //        }

    //        mnuPopup.Show(System.Windows.Forms.Cursor.Position);
    //    }

    //    private void pbMenuButton_MouseLeave(object sender, EventArgs e)
    //    {
    //        isMouseOverButton_ = false;

    //        pbMenuButton.Refresh();
    //    }

    //    private void pbMenuButton_Click(object sender, EventArgs e)
    //    {

    //    }

    //    private void pbTiles_MouseUp(object sender, MouseEventArgs e)
    //    {
    //        isMouseDownTiles_ = false;            
    //    }

    //    private void pbTiles_Click(object sender, EventArgs e)
    //    {

    //    }
        
    //    private void pbTileMode_Click(object sender, EventArgs e)
    //    {
    //    }

    //    private void pbTileMode_Paint(object sender, PaintEventArgs e)
    //    {
    //        Graphics g = e.Graphics;
    //        ProjectUiStateDto uiState = projectController_.GetUiState();

    //        Guid selectedRoomId = uiState.SelectedRoomId;

    //        if (uiState.CollisionMode[selectedRoomId] == true)
    //        {
    //            if (isMouseDownTileModeButton_ == true)
    //            {
    //                g.DrawImageUnscaled(pbTileModeDown.Image, 0, 0);
    //            }
    //            else if (isMouseOverTileModeButton_ == true)
    //            {
    //                g.DrawImageUnscaled(pbTileModeOver.Image, 0, 0);
    //            }
    //            else
    //            {
    //                g.DrawImageUnscaled(pbTileModeNormal.Image, 0, 0);
    //            }
    //        }
    //        else
    //        {
    //            if (isMouseDownTileModeButton_ == true)
    //            {
    //                g.DrawImageUnscaled(pbTileModeDown2.Image, 0, 0);
    //            }
    //            else if (isMouseOverTileModeButton_ == true)
    //            {
    //                g.DrawImageUnscaled(pbTileModeOver2.Image, 0, 0);
    //            }
    //            else
    //            {
    //                g.DrawImageUnscaled(pbTileModeNormal2.Image, 0, 0);
    //            }
    //        }
    //    }

    //    private void pbTileMode_MouseDown(object sender, MouseEventArgs e)
    //    {
    //        isMouseDownTileModeButton_ = true;

    //        pbTileMode.Refresh();
    //    }

    //    private void pbTileMode_MouseLeave(object sender, EventArgs e)
    //    {
    //        isMouseOverTileModeButton_ = false;

    //        pbTileMode.Refresh();
    //    }

    //    private void pbTileMode_MouseMove(object sender, MouseEventArgs e)
    //    {
    //        isMouseOverTileModeButton_ = true;

    //        pbTileMode.Refresh();
    //    }

    //    private void pbTileMode_MouseUp(object sender, MouseEventArgs e)
    //    {
    //        ProjectDto project = projectController_.GetProjectDto();
    //        ProjectUiStateDto uiState = projectController_.GetUiState();
    //        Guid selectedRoomId = uiState.SelectedRoomId;
    //        int selectedRoomIndex = uiState.SelectedRoomIndex;

    //        isMouseDownTileModeButton_ = false;

    //        projectController_.SetCollisionMode(selectedRoomIndex, !uiState.CollisionMode[selectedRoomId]);
    //    }

    //    private void pbTiles_MouseMove(object sender, MouseEventArgs e)
    //    {
    //        if (isMouseDownTiles_ == true)
    //        {
    //            // Dragging a selection rectangle.                
    //            ProjectDto project = projectController_.GetProjectDto();
    //            ProjectUiStateDto uiState = projectController_.GetUiState();
    //            ProjectResourcesDto resources = projectController_.GetResources();

    //            int selectedRoomIndex = uiState.SelectedRoomIndex;
    //            Guid selectedRoomId = uiState.SelectedRoomId;

    //            int tileSize = project.TileSize;

    //            bool isOkay = false;

    //            int imageCols = 0;
    //            int imageRows = 0;
    //            int tileCount = 0;

    //            Guid tileSheetId = project.Tilesets[selectedRoomId].TileSheetId;

    //            if (tileSheetId != Guid.Empty)
    //            {
    //                TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

    //                Guid resourceId = tileSheet.BitmapResourceId;

    //                BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];

    //                imageCols = tileSheet.Columns;
    //                imageRows = tileSheet.Rows;

    //                if (bitmapResource.SpriteSheetImageList.Count > 0)
    //                {
    //                    tileCount = bitmapResource.SpriteSheetImageList.Count;
    //                    isOkay = true;
    //                }
    //            }

    //            if (isOkay == true)
    //            {
    //                int visiblerows = 0;
    //                int visiblecols = 0;

    //                calculateRenderData(ref visiblecols, ref visiblerows);

    //                int clickedRow = ((e.Y + ((rowOffset_ * tileSize) + rowPixelOffset_)) / tileSize);
    //                int clickedCol = ((e.X + ((colOffset_ * tileSize) + colPixelOffset_)) / tileSize);
    //                int clickedTile = (clickedRow * imageCols) + clickedCol;

    //                selectionCorner2TileId_ = clickedTile;

    //                if (selectionCorner1TileId_ != selectionCorner2TileId_)
    //                {
    //                    // Activate the add object button.

    //                }

    //                pbTiles.Refresh();
    //            }
    //        }
    //    }

    //    private void pbCreateObject_Click(object sender, EventArgs e)
    //    {

    //    }

    //    private void pbCreateObject_Paint(object sender, PaintEventArgs e)
    //    {
    //        Graphics g = e.Graphics;
            
    //        if (isMouseDownCreateObjectButton_ == true)
    //        {
    //            g.DrawImageUnscaled(pbCreateTileObjectDown.Image, 0, 0);
    //        }
    //        else if (isMouseOverCreateObjectButton_ == true)
    //        {
    //            g.DrawImageUnscaled(pbCreateTileObjectOver.Image, 0, 0);
    //        }
    //        else
    //        {
    //            g.DrawImageUnscaled(pbCreateTileObjectNormal.Image, 0, 0);
    //        }
    //    }

    //    private void pbCreateObject_MouseDown(object sender, MouseEventArgs e)
    //    {
    //        isMouseDownCreateObjectButton_ = true;

    //        pbCreateObject.Refresh();
    //    }

    //    private void pbCreateObject_MouseLeave(object sender, EventArgs e)
    //    {
    //        isMouseDownCreateObjectButton_ = false;

    //        pbMenuButton.Refresh();
    //    }

    //    private void pbCreateObject_MouseMove(object sender, MouseEventArgs e)
    //    {
    //        isMouseOverCreateObjectButton_ = true;

    //        pbMenuButton.Refresh();
    //    }

    //    private void pbCreateObject_MouseUp(object sender, MouseEventArgs e)
    //    {
    //        // Prompt the user for an object name.
    //        enterNameDialog_.ShowDialog(this);
    //    }

    //    private void getSelectorRectData(int tileId1, int tileId2, int imageCols, ref int left, ref int top, ref int width, ref int height)
    //    {
    //        int cornerTile1X = tileId1 % imageCols;
    //        int cornerTile1Y = Convert.ToInt32(Math.Floor(Convert.ToDouble(tileId1 / imageCols)));

    //        int cornerTile2X = tileId2 % imageCols;
    //        int cornerTile2Y = Convert.ToInt32(Math.Floor(Convert.ToDouble(tileId2 / imageCols)));
            
    //        // Determine the selector bounds.
    //        if (cornerTile1X < cornerTile2X)
    //        {
    //            left = cornerTile1X;
    //            width = (cornerTile2X - cornerTile1X) + 1;
    //        }
    //        else
    //        {
    //            left = cornerTile2X;
    //            width = (cornerTile1X - cornerTile2X) + 1;
    //        }

    //        if (cornerTile1Y < cornerTile2Y)
    //        {
    //            top = cornerTile1Y;
    //            height = (cornerTile2Y - cornerTile1Y) + 1;
    //        }
    //        else
    //        {
    //            top = cornerTile2Y;
    //            height = (cornerTile1Y - cornerTile2Y) + 1;
    //        }
    //    }
    //}

    //public interface ITilesetControl
    //{
    //    // Derived from base.
    //    bool Visible { get; set; }
    //    bool Enabled { get; set; }
    //    int Width { get; set; }
    //    int Height { get; set; }
    //    int Left { get; set; }
    //    int Top { get; set; }
        
    //    void Dispose();
    //    void Refresh();
    //}
}
