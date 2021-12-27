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

    public delegate void TileSheetSelectionChangedHandler(object sender, TileSheetSelectionChangedEventArgs e);

    public partial class TileSheetViewerControl : UserControl, ITileSheetViewerControl
    {
        public event TileSheetSelectionChangedHandler TileSheetSelectionChanged;

        public TileSheetViewerControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            backgroundGenerator_ = firemelonEditorFactory_.NewBackgroundGenerator();

            rowOffset_ = 0;
            colOffset_ = 0;
            rowPixelOffset_ = 0;
            colPixelOffset_ = 0;
            
            isMouseDownTiles_ = false;

            selectionCorner1TileId_ = -2;
            selectionCorner2TileId_ = -2;

            vsImage.SmallChange = 1;
            vsImage.LargeChange = 1;

            hsImage.SmallChange = 1;
            hsImage.LargeChange = 1;
        }

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private IProjectController projectController_ = null;

        private ISheetDtoProxy sheetDtoProxy_ = null;

        // Background generator.
        private IBackgroundGenerator backgroundGenerator_;

        // Scrolling data.
        private int rowOffset_;
        private int colOffset_;
        private int rowPixelOffset_;
        private int colPixelOffset_;

        // Tile selection drag.
        private bool isMouseDownTiles_;

        private int selectionCorner1TileId_;

        private int selectionCorner2TileId_;
        
        #region Properties

        public ISheetDtoProxy Sheet
        {
            get
            {
                return sheetDtoProxy_;
            }
            set
            {
                sheetDtoProxy_ = value;

                // Turn off the selection.
                selectionCorner1TileId_ = -2;
                selectionCorner2TileId_ = -2;

                OnTileSheetSelectionChanged(new TileSheetSelectionChangedEventArgs(false, false, false, false));

                RefreshImage();
            }
        }
        
        #endregion

        #region Public Functions

        public TileObjectDto GetSelectionAsObject()
        {
            ProjectDto project = projectController_.GetProjectDto();

            TileObjectDto tileObject = new TileObjectDto();

            int tileSize = project.TileSize;
                        
            int imageCols = 0;

            TileSheetDto tileSheet = null;

            if (sheetDtoProxy_ != null)
            {
                Guid tileSheetId = sheetDtoProxy_.Id; // project.Tilesets[selectedRoomId].TileSheetId;

                if (tileSheetId != Guid.Empty)
                {
                    tileSheet = projectController_.GetTileSheet(tileSheetId);

                    Guid resourceId = tileSheet.BitmapResourceId;

                    tileObject.BitmapResourceId = resourceId;

                    imageCols = tileSheet.Columns;

                    int cornerTile1X = (int)(((selectionCorner1TileId_ % imageCols) * tileSize) / project.TileSize);
                    int cornerTile1Y = (int)((Convert.ToInt32(Math.Floor(Convert.ToDouble(selectionCorner1TileId_ / imageCols))) * tileSize) / project.TileSize);

                    int cornerTile2X = (int)(((selectionCorner2TileId_ % imageCols) * tileSize) / project.TileSize);
                    int cornerTile2Y = (int)((Convert.ToInt32(Math.Floor(Convert.ToDouble(selectionCorner2TileId_ / imageCols))) * tileSize) / project.TileSize);

                    if (cornerTile1X < cornerTile2X)
                    {
                        tileObject.TopLeftCornerColumn = cornerTile1X;

                        tileObject.Columns = cornerTile2X - cornerTile1X + 1;
                    }
                    else
                    {
                        tileObject.TopLeftCornerColumn = cornerTile2X;

                        tileObject.Columns = cornerTile1X - cornerTile2X + 1;
                    }

                    if (cornerTile1Y < cornerTile2Y)
                    {
                        tileObject.TopLeftCornerRow = cornerTile1Y;

                        tileObject.Rows = cornerTile2Y - cornerTile1Y + 1;
                    }
                    else
                    {
                        tileObject.TopLeftCornerRow = cornerTile2Y;

                        tileObject.Rows = cornerTile1Y - cornerTile2Y + 1;
                    }

                }
            }
            
            return tileObject;
        }

        public void RefreshImage()
        {
            resize();

            pbImage.Refresh();
        }

        #endregion

        #region Private Functions

        private void calculateRenderData(ref int visibleCols, ref int visibleRows)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int temp = 0;
            int tempSize = 0;
            int tileSize = project.TileSize;

            int imageCols = 0;
            int imageRows = 0;
            
            Guid tileSheetId = sheetDtoProxy_.Id;

            TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

            imageCols = tileSheet.Columns;
            imageRows = tileSheet.Rows;
           
            if (colPixelOffset_ > 0)
            {
                temp = 1;
                tempSize = tileSize;
            }

            visibleCols = 0;

            int size = tempSize - colPixelOffset_;

            for (int i = colOffset_ + temp; i < imageCols; i++)
            {
                size += tileSize;

                if (size > pbImage.Width)
                {
                    break;
                }

                visibleCols++;
            }

            // The number of rows that are fully visible...
            temp = 0;
            tempSize = 0;

            if (rowPixelOffset_ > 0)
            {
                temp = 1;
                tempSize = tileSize;
            }

            visibleRows = 0;

            size = tempSize - rowPixelOffset_;

            for (int i = rowOffset_ + temp; i < imageRows; i++)
            {
                size += tileSize;

                if (size > pbImage.Height)
                {
                    break;
                }

                visibleRows++;
            }

            return;
        }

        private void resize()
        {
            hsImage.Top = this.ClientSize.Height - hsImage.Height;
            hsImage.Width = this.ClientSize.Width - vsImage.Width;

            vsImage.Left = this.ClientSize.Width - vsImage.Width;
            vsImage.Height = this.ClientSize.Height - hsImage.Height;

            pbImage.Width = this.Width - vsImage.Width - 1;
            pbImage.Height = this.Height - hsImage.Height - 1;

            if (sheetDtoProxy_ != null)
            {
                //int vScrollMax = ((int)(sheetDtoProxy_.Image.Height * sheetDtoProxy_.ScaleFactor)) - pbImage.Height;

                //if (vScrollMax > 0)
                //{
                //    vsImage.Maximum = vScrollMax;
                //}
                //else
                //{
                //    vsImage.Maximum = vsImage.Minimum;
                //}

                //int hScrollMax = ((int)(sheetDtoProxy_.Image.Width * sheetDtoProxy_.ScaleFactor)) - pbImage.Width;

                //if (hScrollMax > 0)
                //{
                //    hsImage.Maximum = hScrollMax;
                //}
                //else
                //{
                //    hsImage.Maximum = hsImage.Minimum;
                //}

                try
                {
                    backgroundGenerator_.Regenerate();

                    ProjectDto project = projectController_.GetProjectDto();
                    ProjectUiStateDto uiState = projectController_.GetUiState();

                    int tileSize = project.TileSize;

                    int selectedRoomIndex = uiState.SelectedRoomIndex;
                    Guid selectedRoomId = uiState.SelectedRoomId;
                    
                    bool isOkay = false;

                    int imageRows = 0;
                    int imageCols = 0;

                    if (sheetDtoProxy_ != null)
                    { 
                        Guid tileSheetId = sheetDtoProxy_.Id;

                        if (tileSheetId != Guid.Empty)
                        {
                            TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

                            imageRows = tileSheet.Rows;
                            imageCols = tileSheet.Columns;

                            isOkay = true;
                        }
                    }

                    if (isOkay == true)
                    {
                        int visibleCols = 0;
                        int visibleRows = 0;

                        calculateRenderData(ref visibleCols, ref visibleRows);

                        // If there is whitespace and the rowOffset is not zero and the offset is also not zero, drag the tiles down to fill the whitespace.
                        // Adjust the variables and scrollbar accordingly.       

                        bool adjustScrollValues = false;
                        int whiteSpaceHeight = 0;

                        if (rowPixelOffset_ == 0)
                        {
                            whiteSpaceHeight = pbImage.Height - ((imageRows - rowOffset_) * tileSize);

                            if (whiteSpaceHeight > 0 && (rowOffset_ > 0 || (rowOffset_ == 0 && rowPixelOffset_ > 0)))
                            {
                                adjustScrollValues = true;
                            }
                        }
                        else
                        {
                            whiteSpaceHeight = pbImage.Height - ((tileSize - rowPixelOffset_) + ((imageRows - (rowOffset_ + 1)) * tileSize));

                            if (whiteSpaceHeight > 0 && (rowOffset_ > 0 || (rowOffset_ == 0 && rowPixelOffset_ > 0)))
                            {
                                adjustScrollValues = true;
                            }
                        }

                        if (adjustScrollValues == true)
                        {
                            // "Pull down" the tiles so that they occupy the whitespace. Adjust offsets and scrollbar accordingly.

                            // Calculate from the whitespace height, how many new rows can fit, and then what the leftover offset will be.
                            int rowAdjust = whiteSpaceHeight / tileSize;

                            int offsetAdjust = whiteSpaceHeight % tileSize;

                            rowOffset_ -= rowAdjust;

                            if (rowOffset_ < 0)
                            {
                                rowOffset_ = 0;
                                rowPixelOffset_ = 0;
                            }
                            else
                            {
                                rowPixelOffset_ -= offsetAdjust;

                                if (rowPixelOffset_ < 0)
                                {
                                    if (rowOffset_ == 0)
                                    {
                                        rowPixelOffset_ = 0;
                                    }
                                    else
                                    {
                                        rowPixelOffset_ = tileSize - 1;
                                    }

                                    if (rowOffset_ > 0)
                                    {
                                        rowOffset_--;
                                    }
                                }
                            }
                        }

                        // Do the same for columns.
                        adjustScrollValues = false;
                        int whiteSpaceWidth = 0;

                        if (colPixelOffset_ == 0)
                        {
                            whiteSpaceWidth = (pbImage.Width) - ((imageCols - colOffset_) * tileSize);

                            if (whiteSpaceWidth > 0 && (colOffset_ > 0 || (colOffset_ == 0 && colPixelOffset_ > 0)))
                            {
                                adjustScrollValues = true;
                            }
                        }
                        else
                        {
                            whiteSpaceWidth = pbImage.Width - ((tileSize - colPixelOffset_) + ((imageCols - (colOffset_ + 1)) * tileSize));

                            if (whiteSpaceWidth > 0 && (colOffset_ > 0 || (colOffset_ == 0 && colPixelOffset_ > 0)))
                            {
                                adjustScrollValues = true;
                            }
                        }

                        if (adjustScrollValues == true)
                        {
                            // "Pull down" the tiles so that they occupy the whitespace. Adjust offsets and scrollbar accordingly.

                            // Calculate from the whitespace width, how many new cols can fit, and then what the leftover offset will be.
                            int colAdjust = whiteSpaceWidth / tileSize;

                            int offsetAdjust = whiteSpaceWidth % tileSize;

                            colOffset_ -= colAdjust;

                            if (colOffset_ < 0)
                            {
                                colOffset_ = 0;
                                colPixelOffset_ = 0;
                            }
                            else
                            {
                                colPixelOffset_ -= offsetAdjust;

                                if (colPixelOffset_ < 0)
                                {
                                    if (colOffset_ == 0)
                                    {
                                        colPixelOffset_ = 0;
                                    }
                                    else
                                    {
                                        colPixelOffset_ = tileSize - 1;
                                    }

                                    if (colOffset_ > 0)
                                    {
                                        colOffset_--;
                                    }
                                }
                            }
                        }

                        calculateRenderData(ref visibleCols, ref visibleRows);

                        int offscreenCols = imageCols - visibleCols;

                        if (offscreenCols < 0)
                        {
                            offscreenCols = 0;
                        }

                        int offscreenRows = imageRows - visibleRows;

                        if (offscreenRows < 0)
                        {
                            offscreenRows = 0;
                        }

                        hsImage.Maximum = offscreenCols;
                        vsImage.Maximum = offscreenRows;
                    }
                    else
                    {
                        //hScrollBar1.Value = 0;
                        //vScrollBar1.Value = 0;
                        hsImage.Maximum = 0;
                        vsImage.Maximum = 0;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            pbImage.Refresh();
        }

        private void paint(Graphics g)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();
            
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            if (project != null && project.IsPrepared == true)
            {
                backgroundGenerator_.GenerateBackground(pbImage.Width, pbImage.Height);
                
                g.DrawImageUnscaled(backgroundGenerator_.BackgroundImage, new Point(0, 0));

                int tileSize = project.TileSize;

                bool isOkay = false;

                Bitmap bitmapToRender = null;

                Point renderPoint = new Point(0, 0);

                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int selectedTile = -1;
                int imageCols = 0;
                int tileCount = 0;

                TileSheetDto tileSheet = null;

                if (sheetDtoProxy_ != null)
                {
                    Guid tileSheetId = sheetDtoProxy_.Id; // project.Tilesets[selectedRoomId].TileSheetId;

                    if (tileSheetId != Guid.Empty)
                    {
                        tileSheet = projectController_.GetTileSheet(tileSheetId);

                        Guid resourceId = tileSheet.BitmapResourceId;

                        // Separate resources dto removed in 2.2 format.
                        //BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];
                        BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(resourceId, true);

                        bitmapResource.LoadedModules ^= (byte)EditorModule.TileSheetViewer;

                        tileCount = bitmapResource.SpriteSheetImageList.Count;

                        imageCols = tileSheet.Columns;

                        selectedTile = uiState.SelectedTileIndex[selectedRoomId];

                        bitmapToRender = bitmapResource.BitmapImageWithTransparency;

                        isOkay = true;
                    }

                    if (isOkay == true)
                    {
                        int x = -1 * ((colOffset_ * tileSize) + colPixelOffset_);
                        int y = -1 * ((rowOffset_ * tileSize) + rowPixelOffset_);

                        renderPoint = new Point(x, y);

                        int sourceWidth = bitmapToRender.Width;

                        int sourceHeight = bitmapToRender.Height;

                        float scaleFactor = 1.0f;

                        if (tileSheet != null)
                        {
                            scaleFactor = tileSheet.ScaleFactor;
                        }

                        // Scale the destination by the scaling factor.
                        int destinationWidth = (int)(sourceWidth * scaleFactor);

                        int destinationHeight = (int)(sourceHeight * scaleFactor);

                        System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(renderPoint.X, renderPoint.Y, destinationWidth, destinationHeight);

                        g.DrawImage(bitmapToRender, destRect, 0, 0, bitmapToRender.Width, bitmapToRender.Height, GraphicsUnit.Pixel, null);

                        //g.DrawImageUnscaled(bitmapToRender, renderPoint);

                        int visibleRows = 0;
                        int visibleCols = 0;

                        calculateRenderData(ref visibleCols, ref visibleRows);

                        Pen p = new Pen(Color.Black);
                        Pen pSelect = new Pen(Color.Orange, 2.0f);

                        // Draw the rectangle around the selected tile.

                        //if (selectedTile >= 0)
                        //{
                        //    if (selectedTile <= tileCount)
                        //    {
                        //        int tileX = ((selectedTile % imageCols) * tileSize) - (colOffset_ * tileSize) - colPixelOffset_;
                        //        int tileY = (Convert.ToInt32(Math.Floor(Convert.ToDouble(selectedTile / imageCols))) * tileSize) + (rowOffset_ * -tileSize) - rowPixelOffset_;

                        //        g.DrawRectangle(pSelect, tileX, tileY, tileSize, tileSize);
                        //    }
                        //}

                        // If there is a selection rectangle active, draw the selector corners.
                        if (selectionCorner1TileId_ != -2 && selectionCorner2TileId_ != -2)
                        {
                            int cornerTile1X = ((selectionCorner1TileId_ % imageCols) * tileSize) - (colOffset_ * tileSize) - colPixelOffset_;
                            int cornerTile1Y = (Convert.ToInt32(Math.Floor(Convert.ToDouble(selectionCorner1TileId_ / imageCols))) * tileSize) + (rowOffset_ * -tileSize) - rowPixelOffset_;

                            int cornerTile2X = ((selectionCorner2TileId_ % imageCols) * tileSize) - (colOffset_ * tileSize) - colPixelOffset_;
                            int cornerTile2Y = (Convert.ToInt32(Math.Floor(Convert.ToDouble(selectionCorner2TileId_ / imageCols))) * tileSize) + (rowOffset_ * -tileSize) - rowPixelOffset_;

                            int selectorTop = 0;
                            int selectorLeft = 0;
                            int selectorWidth = 0;
                            int selectorHeight = 0;

                            // Determine the selector bounds.
                            if (cornerTile1X < cornerTile2X)
                            {
                                selectorLeft = cornerTile1X;
                                selectorWidth = (cornerTile2X - cornerTile1X) + tileSize;
                            }
                            else
                            {
                                selectorLeft = cornerTile2X;
                                selectorWidth = (cornerTile1X - cornerTile2X) + tileSize;
                            }

                            if (cornerTile1Y < cornerTile2Y)
                            {
                                selectorTop = cornerTile1Y;
                                selectorHeight = (cornerTile2Y - cornerTile1Y) + tileSize;
                            }
                            else
                            {
                                selectorTop = cornerTile2Y;
                                selectorHeight = (cornerTile1Y - cornerTile2Y) + tileSize;
                            }

                            g.DrawRectangle(pSelect, selectorLeft, selectorTop, selectorWidth, selectorHeight);

                            p.Dispose();
                        }
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        private void vsImage_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                ProjectDto project = projectController_.GetProjectDto();
                ProjectUiStateDto uiState = projectController_.GetUiState();

                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int tileSize = project.TileSize;

                if (sheetDtoProxy_ != null)
                {
                    Guid tileSheetId = sheetDtoProxy_.Id;

                    if (tileSheetId != Guid.Empty)
                    {
                        TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

                        int imageRows = tileSheet.Rows;

                        if (e.OldValue != e.NewValue)
                        {
                            int visibleCols = 0;
                            int visibleRows = 0;

                            calculateRenderData(ref visibleCols, ref visibleRows);

                            if (rowPixelOffset_ == 0)
                            {
                                if (e.NewValue == vsImage.Maximum)
                                {
                                    rowPixelOffset_ = ((visibleRows + 1) * tileSize) - pbImage.Height;

                                    rowOffset_ += rowPixelOffset_ / tileSize;

                                    rowPixelOffset_ = rowPixelOffset_ % tileSize;

                                    calculateRenderData(ref visibleCols, ref visibleRows);

                                    int offscreenRows = imageRows - visibleRows;

                                    if (offscreenRows < 0)
                                    {
                                        offscreenRows = 0;
                                    }

                                    e.NewValue = offscreenRows;

                                    vsImage.Maximum = offscreenRows;
                                }
                                else
                                {
                                    rowOffset_ = e.NewValue;
                                }
                            }
                            else
                            {
                                if (e.NewValue == 0)
                                {
                                    rowPixelOffset_ = 0;
                                    rowOffset_ = 0;
                                }
                                else if (e.NewValue == vsImage.Maximum && (pbImage.Height - ((tileSize - rowPixelOffset_) + (visibleRows * tileSize))) != 0)
                                {
                                    // There is no row alignment. Need to modify the offset by some amount to bottom align the tiles.
                                    rowPixelOffset_ += tileSize - (pbImage.Height - ((tileSize - rowPixelOffset_) + (visibleRows * tileSize)));

                                    calculateRenderData(ref visibleCols, ref visibleRows);

                                    int offscreenRows = imageRows - visibleRows;

                                    if (offscreenRows < 0)
                                    {
                                        offscreenRows = 0;
                                    }

                                    e.NewValue = offscreenRows;

                                    vsImage.Maximum = offscreenRows;
                                }
                                else
                                {
                                    rowOffset_ = e.NewValue - 1;
                                }
                            }
                        }
                    }
                }

                pbImage.Refresh();
            }
            catch (Exception ex)
            {
            }
        }

        private void hsImage_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                ProjectDto project = projectController_.GetProjectDto();
                ProjectUiStateDto uiState = projectController_.GetUiState();

                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int tileSize = project.TileSize;

                if (sheetDtoProxy_ != null)
                {
                    Guid tileSheetId = sheetDtoProxy_.Id;

                    if (tileSheetId != Guid.Empty)
                    {
                        TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

                        if (e.OldValue != e.NewValue)
                        {
                            int imageCols = tileSheet.Columns;

                            int visibleCols = 0;
                            int visibleRows = 0;

                            calculateRenderData(ref visibleCols, ref visibleRows);

                            if (colPixelOffset_ == 0)
                            {
                                if (e.NewValue == hsImage.Maximum)
                                {
                                    colPixelOffset_ = ((visibleCols + 1) * tileSize) - pbImage.Width;

                                    colOffset_ += colPixelOffset_ / tileSize;

                                    colPixelOffset_ = colPixelOffset_ % tileSize;

                                    calculateRenderData(ref visibleCols, ref visibleRows);

                                    int offscreenCols = imageCols - visibleCols;

                                    if (offscreenCols < 0)
                                    {
                                        offscreenCols = 0;
                                    }

                                    e.NewValue = offscreenCols;

                                    hsImage.Maximum = offscreenCols;
                                }
                                else
                                {
                                    colOffset_ = e.NewValue;
                                }
                            }
                            else
                            {
                                if (e.NewValue == 0)
                                {
                                    colPixelOffset_ = 0;
                                    colOffset_ = 0;
                                }
                                else if (e.NewValue == hsImage.Maximum && (pbImage.Width - ((tileSize - colPixelOffset_) + (visibleCols * tileSize))) != 0)
                                {
                                    // There is no row alignment. Need to modify the offset by some amount to bottom align the tiles.
                                    colPixelOffset_ += tileSize - (pbImage.Width - ((tileSize - colPixelOffset_) + (visibleCols * tileSize)));

                                    calculateRenderData(ref visibleCols, ref visibleRows);

                                    int offscreenCols = imageCols - visibleCols;

                                    if (offscreenCols < 0)
                                    {
                                        offscreenCols = 0;
                                    }

                                    e.NewValue = offscreenCols;

                                    hsImage.Maximum = offscreenCols;
                                }
                                else
                                {
                                    colOffset_ = e.NewValue - 1;
                                }
                            }
                        }
                    }
                }

                pbImage.Refresh();
            }
            catch (Exception ex)
            {
            }
        }

        private void SheetViewerControl_Resize(object sender, EventArgs e)
        {
            resize();
        }
        
        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDownTiles_ = true;

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();
            //ProjectResourcesDto resources = projectController_.GetResources();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int tileSize = project.TileSize;

            bool isOkay = false;

            int imageCols = 0;
            int imageRows = 0;
            int tileCount = 0;
            
            if (sheetDtoProxy_ != null)
            {
                Guid tileSheetId = sheetDtoProxy_.Id;

                if (tileSheetId != Guid.Empty)
                {
                    TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

                    Guid resourceId = tileSheet.BitmapResourceId;

                    // Separate resources dto removed in 2.2 format.
                    //BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];
                    BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(resourceId, true);

                    bitmapResource.LoadedModules ^= (byte)EditorModule.TileSheetViewer;

                    imageCols = tileSheet.Columns;
                    imageRows = tileSheet.Rows;

                    if (bitmapResource.SpriteSheetImageList.Count > 0)
                    {
                        tileCount = bitmapResource.SpriteSheetImageList.Count;
                        isOkay = true;
                    }
                }

                if (isOkay == true)
                {
                    int visiblerows = 0;
                    int visiblecols = 0;

                    calculateRenderData(ref visiblecols, ref visiblerows);

                    int clickedRow = ((e.Y + ((rowOffset_ * tileSize) + rowPixelOffset_)) / tileSize);
                    int clickedCol = ((e.X + ((colOffset_ * tileSize) + colPixelOffset_)) / tileSize);
                   
                    int highestvisiblerow = 0;

                    bool isbottomaligned = false;

                    if (clickedRow >= 0 && clickedCol >= 0 && clickedRow < imageRows && clickedCol < imageCols)
                    {
                        int clickedTile = (clickedRow * imageCols) + clickedCol;

                        selectionCorner1TileId_ = clickedTile;
                        selectionCorner2TileId_ = clickedTile;

                        OnTileSheetSelectionChanged(new TileSheetSelectionChangedEventArgs(true, true, false, false));

                        // get the total Height of all visible rows, and the offset if needed, and compare it to the control Height, to see if the last row is partially visible or wholly visible.
                        if (rowPixelOffset_ == 0)
                        {
                            if ((visiblerows * tileSize) < pbImage.Height)
                            {
                                // if the last visible row is partially visible...
                                highestvisiblerow = rowOffset_ + visiblerows;
                            }
                            else
                            {
                                highestvisiblerow = rowOffset_ + visiblerows - 1;

                                // if there's no offset and you can fit an exact amount of tiles in, it is bottom aligned.
                                if (pbImage.Height % tileSize == 0)
                                {
                                    isbottomaligned = true;
                                }
                            }
                        }
                        else
                        {
                            if (((visiblerows * tileSize) + (tileSize - rowPixelOffset_)) < pbImage.Height)
                            {
                                highestvisiblerow = rowOffset_ + visiblerows + 1;
                            }
                            else
                            {
                                isbottomaligned = true;
                                highestvisiblerow = rowOffset_ + visiblerows;
                            }
                        }

                        int highestVisibleCol = 0;
                        bool isRightAligned = false;

                        // get the total Width of all visible cols, and the offset if needed, and compare it to the control Width, to see if the last col is partially visible or wholly visible.
                        if (colPixelOffset_ == 0)
                        {
                            if ((visiblecols * tileSize) < pbImage.Width)
                            {
                                // if the last visible row is partially visible...
                                highestVisibleCol = colOffset_ + visiblecols;
                            }
                            else
                            {
                                highestVisibleCol = colOffset_ + visiblecols - 1;

                                // if there's no offset and you can fit an exact amount of tiles in, it is bottom aligned.
                                if (pbImage.Width % tileSize == 0)
                                {
                                    isRightAligned = true;
                                }
                            }
                        }
                        else
                        {
                            if (((visiblecols * tileSize) + (tileSize - colPixelOffset_)) < pbImage.Width)
                            {
                                highestVisibleCol = colOffset_ + visiblecols + 1;
                            }
                            else
                            {
                                isRightAligned = true;
                                highestVisibleCol = colOffset_ + visiblecols;
                            }
                        }

                        if (e.Button == MouseButtons.Left || e.Button == System.Windows.Forms.MouseButtons.Right)
                        {
                            if (clickedTile <= tileCount - 1)
                            {
                                if (rowPixelOffset_ == 0)
                                {
                                    if (clickedRow >= highestvisiblerow)
                                    {
                                        if (isbottomaligned == false)
                                        {
                                            if (vsImage.Value < vsImage.Maximum)
                                            {
                                                vsImage.Value++;
                                            }

                                            rowPixelOffset_ = ((visiblerows + 1) * tileSize) - pbImage.Height;
                                        }
                                    }
                                    else if (clickedRow == rowOffset_ && isbottomaligned == false)
                                    {
                                        if (rowPixelOffset_ > 0)
                                        {
                                            rowPixelOffset_ = 0;

                                            if (vsImage.Value != vsImage.Minimum)
                                            {
                                                vsImage.Value--;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (clickedRow == rowOffset_)
                                    {
                                        rowPixelOffset_ = 0;

                                        vsImage.Value--;

                                        // aligning to the bottom may cause number of offscreen rows to change
                                        calculateRenderData(ref visiblecols, ref visiblerows);

                                        int offscreenrows = imageRows - visiblerows;

                                        if (offscreenrows < 0)
                                        {
                                            offscreenrows = 0;
                                        }

                                        vsImage.Maximum = offscreenrows;

                                    }
                                    else if (clickedRow == highestvisiblerow)
                                    {
                                        // if the highest visible row is offscreen at all...
                                        if (isbottomaligned == false)
                                        {
                                            rowPixelOffset_ = ((visiblerows + 2) * tileSize) - pbImage.Height;

                                            rowOffset_ += rowPixelOffset_ / tileSize;

                                            rowPixelOffset_ = rowPixelOffset_ % tileSize;

                                            vsImage.Value = rowOffset_ + 1;

                                            // aligning to the bottom may cause number of offscreen rows to change
                                            calculateRenderData(ref visiblecols, ref visiblerows);

                                            int offscreenrows = imageRows - visiblerows;

                                            if (offscreenrows < 0)
                                            {
                                                offscreenrows = 0;
                                            }

                                            if (vsImage.Value == vsImage.Maximum && offscreenrows > vsImage.Maximum)
                                            {
                                                vsImage.Value -= offscreenrows - vsImage.Maximum;
                                            }

                                            vsImage.Maximum = offscreenrows;
                                        }
                                    }
                                }

                                if (colPixelOffset_ == 0)
                                {
                                    if (clickedCol >= highestVisibleCol)
                                    {
                                        if (isRightAligned == false)
                                        {
                                            if (hsImage.Value < hsImage.Maximum)
                                            {
                                                hsImage.Value++;
                                            }

                                            colPixelOffset_ = ((visiblecols + 1) * tileSize) - pbImage.Width;
                                        }
                                    }
                                    else if (clickedCol == colOffset_ && isRightAligned == false)
                                    {
                                        if (colPixelOffset_ > 0)
                                        {
                                            colPixelOffset_ = 0;

                                            if (hsImage.Value != hsImage.Minimum)
                                            {
                                                hsImage.Value--;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (clickedCol == colOffset_)
                                    {
                                        colPixelOffset_ = 0;

                                        hsImage.Value--;

                                        // aligning to the bottom may cause number of offscreen cols to change
                                        calculateRenderData(ref visiblecols, ref visiblerows);

                                        int offscreencols = imageCols - visiblecols;

                                        if (offscreencols < 0)
                                        {
                                            offscreencols = 0;
                                        }

                                        hsImage.Maximum = offscreencols;
                                    }
                                    else if (clickedCol == highestVisibleCol)
                                    {
                                        // if the highest visible col is offscreen at all...
                                        if (isRightAligned == false)
                                        {
                                            colPixelOffset_ = ((visiblecols + 2) * tileSize) - pbImage.Width;

                                            colOffset_ += colPixelOffset_ / tileSize;

                                            colPixelOffset_ = colPixelOffset_ % tileSize;

                                            hsImage.Value = colOffset_ + 1;

                                            // aligning to the bottom may cause number of offscreen cols to change
                                            calculateRenderData(ref visiblecols, ref visiblerows);

                                            int offscreencols = imageCols - visiblecols;

                                            if (offscreencols < 0)
                                            {
                                                offscreencols = 0;
                                            }

                                            if (hsImage.Value == hsImage.Maximum && offscreencols > hsImage.Maximum)
                                            {
                                                hsImage.Value -= offscreencols - hsImage.Maximum;
                                            }

                                            hsImage.Maximum = offscreencols;
                                        }
                                    }
                                }
                            }
                        }

                        this.Refresh();
                    }
                }
            }
        }

        private void pbImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDownTiles_ == true)
            {
                // Dragging a selection rectangle.                
                ProjectDto project = projectController_.GetProjectDto();
                ProjectUiStateDto uiState = projectController_.GetUiState();

                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();

                int selectedRoomIndex = uiState.SelectedRoomIndex;
                Guid selectedRoomId = uiState.SelectedRoomId;

                int tileSize = project.TileSize;

                bool isOkay = false;

                int imageCols = 0;
                int imageRows = 0;
                int tileCount = 0;

                if (sheetDtoProxy_ != null)
                {
                    Guid tileSheetId = sheetDtoProxy_.Id;

                    if (tileSheetId != Guid.Empty)
                    {
                        TileSheetDto tileSheet = projectController_.GetTileSheet(tileSheetId);

                        Guid resourceId = tileSheet.BitmapResourceId;

                        // Separate resources dto removed in 2.2 format.
                        //BitmapResourceDto bitmapResource = resources.Bitmaps[resourceId];
                        BitmapResourceDto bitmapResource = projectController_.GetBitmapResource(resourceId, true);

                        bitmapResource.LoadedModules ^= (byte)EditorModule.TileSheetViewer;

                        imageCols = tileSheet.Columns;
                        imageRows = tileSheet.Rows;

                        if (bitmapResource.SpriteSheetImageList.Count > 0)
                        {
                            tileCount = bitmapResource.SpriteSheetImageList.Count;
                            isOkay = true;
                        }
                    }

                    if (isOkay == true)
                    {
                        int visiblerows = 0;
                        int visiblecols = 0;

                        calculateRenderData(ref visiblecols, ref visiblerows);

                        int clickedRow = ((e.Y + ((rowOffset_ * tileSize) + rowPixelOffset_)) / tileSize);
                        int clickedCol = ((e.X + ((colOffset_ * tileSize) + colPixelOffset_)) / tileSize);

                        if (clickedRow >= 0 && clickedCol >= 0 && clickedRow < imageRows && clickedCol < imageCols)
                        {
                            int clickedTile = (clickedRow * imageCols) + clickedCol;

                            selectionCorner2TileId_ = clickedTile;

                            if (selectionCorner1TileId_ != selectionCorner2TileId_)
                            {
                                // Activate the add object button.

                            }

                            pbImage.Refresh();
                        }
                    }
                }
            }
        }

        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDownTiles_ = false;
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            paint(e.Graphics);
        }

        #endregion

        protected virtual void OnTileSheetSelectionChanged(TileSheetSelectionChangedEventArgs e)
        {
            TileSheetSelectionChanged(this, e);
        }
    }

    public class TileSheetSelectionChangedEventArgs : System.EventArgs
    {
        // Constructor
        public TileSheetSelectionChangedEventArgs(bool isSelected, bool canAddObject, bool canDeleteObject, bool canDeleteAnimation)
        {
            canAddObject_ = canAddObject;

            canDeleteObject_ = canDeleteObject;

            canDeleteAnimation_ = canDeleteAnimation;

            isSelected_ = isSelected;
        }

        // Properties
        public bool CanAddObject
        {
            get { return canAddObject_; }
        }
        private bool canAddObject_;


        public bool CanDeleteAnimation
        {
            get { return canDeleteAnimation_; }
            set { canDeleteAnimation_ = value; }
        }
        private bool canDeleteAnimation_;


        public bool CanDeleteObject
        {
            get { return canDeleteObject_; }
            set { canDeleteObject_ = value; }
        }
        private bool canDeleteObject_;

        public bool IsSelected
        {
            get { return isSelected_; }
        }
        private bool isSelected_;
    }
}
