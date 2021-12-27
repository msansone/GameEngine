using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public enum LayerButtons
    {
        Visible = 1,
        Delete = 2,
        Interactive = 3
    };

    public delegate void CursorChangedHandler(object sender, CursorChangedEventArgs e);

    public partial class LayerListControl : UserControl, ILayerListControl
    {   
        private Graphics g;
        
        private const int blinkTimes_ = 10;

        new public event CursorChangedHandler CursorChanged;
        
        private Point ptMouseOver_;
        private List<int> lstOffsets_;
        private List<int> lstRowOffsets_;
        private List<int> lstScrollbarValues_;

        private int mouseOverLayer_;
        private int mouseOverIndex_;
        private int toLayerOrdinal_;
        private int fromLayerOrdinal_;
        private int scrollRate_;
        private int scrollCounter_;
        private int iconsTop_;
        private int iconsLeft_;

        private bool mouseOverVisible_;
        private bool mouseOverDelete_;
        private bool mouseOverInteractive_;
        private bool mouseOverEdit_;
        private bool mouseOverTopHalf_;
        private bool mouseDown_;
        private bool mouseDragging_;
        private bool scrollUp_;
        private bool scrollDown_;
        private bool isMouseOverButton_;
        private bool isMouseDownButton_;

        private INewLayerDialog newLayerDialog_;
        private IEditLayerDialog editLayerDialog_;

        private IProjectController projectController_;

        public LayerListControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            projectController_.ProjectCreated += LayerListControl_ProjectCreated;
            projectController_.RoomAdded += LayerListControl_RoomAdded;
            projectController_.BeforeRoomDeleted += LayerListControl_BeforeRoomDeleted;
            projectController_.RoomSelected += LayerListControl_RoomSelected;
            projectController_.RefreshView += LayerListControl_RefreshView;
            
            lstOffsets_ = new List<int>();
            lstRowOffsets_ = new List<int>();
            lstScrollbarValues_ = new List<int>();

            ptMouseOver_ = new Point(0, 0);

            mouseOverLayer_ = 0;
            toLayerOrdinal_ = 0;
            mouseOverIndex_ = 0;
            scrollRate_ = 0;
            scrollCounter_ = 1;
            iconsTop_ = 25;
            iconsLeft_ = 10;

            mouseOverTopHalf_ = false;
            mouseDown_ = false;
            mouseDragging_ = false;
            mouseOverVisible_ = false;
            mouseOverDelete_ = false;
            mouseOverInteractive_ = false;
            mouseOverEdit_ = false;
            scrollDown_ = false;
            scrollUp_ = false;
            isMouseOverButton_ = false;
            isMouseDownButton_ = false;

            vScrollBar1.Maximum = 0;
            vScrollBar1.Minimum = 0;
            vScrollBar1.SmallChange = 1;
            vScrollBar1.LargeChange = 1;

            IFiremelonEditorFactory firemelonEditorFactory = new FiremelonEditorFactory();

            newLayerDialog_ = firemelonEditorFactory.NewNewLayerDialog();
            newLayerDialog_.LayerAdded += new LayerAddedHandler(this.LayerListControl_LayerAdded);

            editLayerDialog_ = firemelonEditorFactory.NewEditLayerDialog();
            editLayerDialog_.LayerEdited += new LayerEditedHandler(this.LayerListControl_LayerEdited);
        }

        void LayerListControl_RefreshView(object sender, RefreshViewEventArgs e)
        {
            pbLayers.Refresh();
        }

        void LayerListControl_RoomAdded(object sender, RoomAddedEventArgs e)
        {
            lstOffsets_.Add(0);
            lstRowOffsets_.Add(0);
            lstScrollbarValues_.Add(0);
        }

        void LayerListControl_BeforeRoomDeleted(object sender, BeforeRoomDeletedEventArgs e)
        {
            lstOffsets_.RemoveAt(e.Index);
            lstRowOffsets_.RemoveAt(e.Index);
            lstScrollbarValues_.RemoveAt(e.Index);
        }
        
        void LayerListControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
        {
            lstOffsets_.Clear();
            lstRowOffsets_.Clear();
            lstScrollbarValues_.Clear();

            ProjectDto project = projectController_.GetProjectDto();

            foreach (RoomDto room in project.Rooms)
            {
                lstScrollbarValues_.Add(0);
                lstOffsets_.Add(0);
                lstRowOffsets_.Add(0);
            }
        }

        protected virtual void OnCursorChanged(CursorChangedEventArgs e)
        {
            CursorChanged(this, e);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                ProjectDto project = projectController_.GetProjectDto();
                ProjectUiStateDto uiState = projectController_.GetUiState();

                Guid selectedRoomId = uiState.SelectedRoomId;
                int selectedRoomIndex = uiState.SelectedRoomIndex;

                if (e.OldValue != e.NewValue)
                {
                    // Determine the number of rows and columns that are fully visible.
                    // Also determine the total number of rows.                
                    int totalRows = project.Layers[selectedRoomId].Count;
                    int visibleRows = 0;
                    int offscreenRows = 0;

                    calculateRenderData(ref visibleRows);

                    if (lstOffsets_[selectedRoomIndex] == 0)
                    {
                        if (e.NewValue == vScrollBar1.Maximum)
                        {
                            lstOffsets_[selectedRoomIndex] = ((visibleRows + 1) * pbLayerBG.Height) - (pbLayers.Height);

                            lstRowOffsets_[selectedRoomIndex] += lstOffsets_[selectedRoomIndex] / pbLayerBG.Height;

                            lstOffsets_[selectedRoomIndex] = lstOffsets_[selectedRoomIndex] % pbLayerBG.Height;

                            calculateRenderData(ref visibleRows);

                            offscreenRows = totalRows - visibleRows;

                            if (offscreenRows < 0)
                            {
                                offscreenRows = 0;
                            }

                            e.NewValue = offscreenRows;

                            vScrollBar1.Maximum = offscreenRows;
                        }
                        else
                        {
                            lstRowOffsets_[selectedRoomIndex] = e.NewValue;
                        }
                    }
                    else
                    {
                        if (e.NewValue == 0)
                        {
                            lstOffsets_[selectedRoomIndex] = 0;
                            lstRowOffsets_[selectedRoomIndex] = 0;
                        }
                        else if (e.NewValue == vScrollBar1.Maximum && (pbLayers.Height - ((pbLayerBG.Height - lstOffsets_[selectedRoomIndex]) + (visibleRows * pbLayerBG.Height))) != 0)
                        {
                            // There is no row alignment. Need to modify the offset by some amount to bottom align the tiles.
                            lstOffsets_[selectedRoomIndex] += pbLayerBG.Height - (pbLayers.Height - ((pbLayerBG.Height - lstOffsets_[selectedRoomIndex]) + (visibleRows * pbLayerBG.Height)));

                            calculateRenderData(ref visibleRows);

                            offscreenRows = totalRows - visibleRows;

                            if (offscreenRows < 0)
                            {
                                offscreenRows = 0;
                            }

                            e.NewValue = offscreenRows;

                            vScrollBar1.Maximum = offscreenRows;
                        }
                        else
                        {
                            lstRowOffsets_[selectedRoomIndex] = e.NewValue - 1;
                        }
                    }
                }

                lstScrollbarValues_[selectedRoomIndex] = e.NewValue;

                pbLayers.Refresh();
            }
            catch (Exception ex)
            {
            }
        }
        
        private int calculateLayerTop(int layerIndex)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;
            int selectedRoomIndex = uiState.SelectedRoomIndex;

            int layerTop = 0;

            for (int i = 0; i < project.Layers[selectedRoomId].Count; i++)
            {
                int currentLayerIndex = projectController_.GetLayerIndexFromOrdinal(selectedRoomIndex, i);

                if (currentLayerIndex == layerIndex)
                {
                    layerTop = ((i - lstRowOffsets_[selectedRoomIndex]) * pbLayerBG.Height) - lstOffsets_[selectedRoomIndex];
                    break;
                }
            }

            return layerTop;
        }

        private void calculateRenderData(ref int visibleRows)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            Guid selectedRoomId = uiState.SelectedRoomId;
            int selectedRoomIndex = uiState.SelectedRoomIndex;

            int totalRows = project.Layers[selectedRoomId].Count;

            // The number of rows that are fully visible...
            int temp = 0;
            int tempSize = 0;

            if (lstOffsets_[selectedRoomIndex] > 0)
            {
                temp = 1;
                tempSize = pbLayerBG.Height;
            }

            visibleRows = 0;

            int size = tempSize - lstOffsets_[selectedRoomIndex];

            for (int i = lstRowOffsets_[selectedRoomIndex] + temp; i < totalRows; i++)
            {
                size += pbLayerBG.Height;

                if (size > pbLayers.Height)
                {
                    break;
                }

                visibleRows++;
            }

            return;
        }

        private void LayerListControl_RoomSelected(object sender, RoomSelectedEventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            resizeLayerList(false);

            if (lstScrollbarValues_[selectedRoomIndex] > vScrollBar1.Maximum)
            {
                vScrollBar1.Value = vScrollBar1.Maximum;
                lstScrollbarValues_[selectedRoomIndex] = vScrollBar1.Value;
            }
            else
            {
                vScrollBar1.Value = lstScrollbarValues_[selectedRoomIndex];
            }

            pbLayers.Refresh();
        }

        //private void LayerListControl_RefreshView(object sender, RefreshViewEventArgs e)
        //{
        //    resizeLayerList();
        //}

        //private void LayerListControl_RoomContainerCleared(object sender, RoomContainerClearedEventArgs e)
        //{
        //    vScrollBar1.Maximum = 0;
        //    vScrollBar1.Minimum = 0;
        //    vScrollBar1.SmallChange = 1;
        //    vScrollBar1.LargeChange = 1;
        //}
        
        private void LayerListControl_LayerAdded(object sender, LayerAddedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            for (int i = 0; i < project.Layers[selectedRoomId].Count; i++)
            {
                LayerDto currentLayer = project.Layers[selectedRoomId][i];

                if (e.LayerName.ToUpper() == currentLayer.Name.ToUpper())
                {
                    e.Cancel = true;
                    break;
                }
            }

            if (e.Cancel == false)
            {
                projectController_.AddLayer(selectedRoomIndex, e.LayerName, e.LayerCols, e.LayerRows);

                // Update the scrollbar if necessary.
                int visibleRows = 0;
                calculateRenderData(ref visibleRows);

                vScrollBar1.Maximum = project.Layers[selectedRoomId].Count - visibleRows;

                this.Refresh();
            }
        }

        private void LayerListControl_LayerEdited(object sender, LayerEditedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            for (int i = 0; i < project.Layers[selectedRoomId].Count; i++)
            {
                LayerDto currentLayer = project.Layers[selectedRoomId][i];

                if (e.LayerName.ToUpper() == currentLayer.Name.ToUpper() && e.LayerId != currentLayer.Id)
                {
                    e.Cancel = true;
                    break;
                }
            }

            if (e.Cancel == false)
            {
                int layerIndex = -1;

                for (int i = 0; i < project.Layers[selectedRoomId].Count; i++)
                {
                    if (project.Layers[selectedRoomId][i].Id == e.LayerId)
                    {
                        layerIndex = i;
                    }
                }

                projectController_.SetLayerNameRowsColumns(selectedRoomIndex, layerIndex, e.LayerName, e.LayerRows, e.LayerCols);

                this.Refresh();                
            }
        }

        private void LayerListControl_Resize(object sender, EventArgs e)
        {
            resizeLayerList();
        }

        private void resizeLayerList(bool saveScrollValue = true)
        {
            try
            {
                ProjectDto project = projectController_.GetProjectDto();
                ProjectUiStateDto uiState = projectController_.GetUiState();

                pnAddNew.Width = this.Width;

                pbLayers.Width = this.Width - vScrollBar1.Width;
                pbLayers.Height = this.Height - pnAddNew.Height;

                vScrollBar1.Height = this.Height - pnAddNew.Height;
                vScrollBar1.Left = pnAddNew.Right - vScrollBar1.Width;

                int roomCount = project.Rooms.Count;

                int selectedRoomIndex = uiState.SelectedRoomIndex;

                // A resize affects the state variables of all rooms.
                for (int i = 0; i < roomCount; i++)
                {
                    Guid currentRoomId = project.Rooms[i].Id;

                    // The total number of rows.
                    int totalRows = project.Layers[currentRoomId].Count;

                    // The number of rows that are fully visible...
                    int tempSize = 0;

                    if (lstOffsets_[i] > 0)
                    {
                        tempSize = pbLayerBG.Height;
                    }

                    int visibleRows = 0;

                    calculateRenderData(ref visibleRows);

                    // If there is whitespace and the rowOffset is not zero and the offset is also not zero, drag the tiles down to fill the whitespace.
                    // Adjust the variables and scrollbar accordingly.       
                    bool adjustScrollValues = false;
                    int whiteSpaceHeight = 0;

                    if (lstOffsets_[i] == 0)
                    {
                        whiteSpaceHeight = pbLayers.Height - ((totalRows - lstRowOffsets_[i]) * pbLayerBG.Height);

                        if (whiteSpaceHeight > 0 && (lstRowOffsets_[i] > 0 || (lstRowOffsets_[i] == 0 && lstOffsets_[i] > 0)))
                        {
                            adjustScrollValues = true;
                        }
                    }
                    else
                    {
                        whiteSpaceHeight = pbLayers.Height - ((pbLayerBG.Height - lstOffsets_[i]) + ((totalRows - (lstRowOffsets_[i] + 1)) * pbLayerBG.Height));

                        if (whiteSpaceHeight > 0 && (lstRowOffsets_[i] > 0 || (lstRowOffsets_[i] == 0 && lstOffsets_[i] > 0)))
                        {
                            adjustScrollValues = true;
                        }
                    }

                    if (adjustScrollValues == true)
                    {
                        // "Pull down" the tiles so that they occupy the whitespace. Adjust offsets and scrollbar accordingly.

                        // Calculate from the whitespace height, how many new rows can fit, and then what the leftover offset will be.
                        int rowAdjust = whiteSpaceHeight / pbLayerBG.Height;

                        int offsetAdjust = whiteSpaceHeight % pbLayerBG.Height;

                        lstRowOffsets_[i] -= rowAdjust;

                        if (lstRowOffsets_[i] < 0)
                        {
                            lstRowOffsets_[i] = 0;
                            lstOffsets_[i] = 0;
                        }
                        else
                        {
                            lstOffsets_[i] -= offsetAdjust;

                            if (lstOffsets_[i] < 0)
                            {
                                if (lstRowOffsets_[i] == 0)
                                {
                                    lstOffsets_[i] = 0;
                                }
                                else
                                {
                                    lstOffsets_[i] = pbLayerBG.Height - 1;
                                }

                                if (lstRowOffsets_[i] > 0)
                                {
                                    lstRowOffsets_[i]--;
                                }

                            }
                        }
                    }

                    int offscreenRows = totalRows - visibleRows;

                    if (offscreenRows < 0)
                    {
                        offscreenRows = 0;
                    }

                    if (selectedRoomIndex == i)
                    {
                        vScrollBar1.Maximum = offscreenRows;
                    }

                    if (saveScrollValue == true)
                    {
                        lstScrollbarValues_[i] = vScrollBar1.Value;
                    }
                }

                pbLayers.Refresh();
            }
            catch (Exception ex)
            {
            }
        }

        private void tmrScroll_Tick(object sender, EventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int newValue = 0;

            scrollCounter_++;

            if (scrollCounter_ == 3)
            {
                scrollCounter_ = 4;
            }
            else if (scrollCounter_ == 5)
            {
                scrollCounter_ = 1;
            }

            if (scrollCounter_ % scrollRate_ == 0)
            {
                if (scrollDown_ == true)
                {
                    // Adjust the scroll bar to move up one.
                    newValue = vScrollBar1.Value + 1;

                    if (newValue >= vScrollBar1.Maximum)
                    {
                        newValue = vScrollBar1.Maximum;
                    }

                    vScrollBar1.Value = newValue;

                    lstScrollbarValues_[selectedRoomIndex] = newValue;

                    try
                    {
                        // Determine the number of rows and columns that are fully visible.
                        // Also determine the total number of rows.
                        int totalRows = project.Layers[selectedRoomId].Count;
                        int visibleRows = 0;
                        int offscreenRows = 0;

                        calculateRenderData(ref visibleRows);

                        if (lstOffsets_[selectedRoomIndex] == 0)
                        {
                            if (newValue == vScrollBar1.Maximum)
                            {
                                lstOffsets_[selectedRoomIndex] = ((visibleRows + 1) * pbLayerBG.Height) - (pbLayers.Height);

                                lstRowOffsets_[selectedRoomIndex] += lstOffsets_[selectedRoomIndex] / pbLayerBG.Height;

                                lstOffsets_[selectedRoomIndex] = lstOffsets_[selectedRoomIndex] % pbLayerBG.Height;

                                calculateRenderData(ref visibleRows);

                                offscreenRows = totalRows - visibleRows;

                                if (offscreenRows < 0)
                                {
                                    offscreenRows = 0;
                                }

                                newValue = offscreenRows;

                                vScrollBar1.Maximum = offscreenRows;
                            }
                            else
                            {
                                lstRowOffsets_[selectedRoomIndex] = newValue;
                            }
                        }
                        else
                        {
                            if (newValue == 0)
                            {
                                lstOffsets_[selectedRoomIndex] = 0;
                                lstRowOffsets_[selectedRoomIndex] = 0;
                            }
                            else if (newValue == vScrollBar1.Maximum && (pbLayers.Height - ((pbLayerBG.Height - lstOffsets_[selectedRoomIndex]) + (visibleRows * pbLayerBG.Height))) != 0)
                            {
                                // There is no row alignment. Need to modify the offset by some amount to bottom align the tiles.
                                lstOffsets_[selectedRoomIndex] += pbLayerBG.Height - (pbLayers.Height - ((pbLayerBG.Height - lstOffsets_[selectedRoomIndex]) + (visibleRows * pbLayerBG.Height)));

                                calculateRenderData(ref visibleRows);

                                offscreenRows = totalRows - visibleRows;

                                if (offscreenRows < 0)
                                {
                                    offscreenRows = 0;
                                }

                                newValue = offscreenRows;

                                vScrollBar1.Maximum = offscreenRows;
                            }
                            else
                            {
                                lstRowOffsets_[selectedRoomIndex] = newValue - 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (scrollUp_ == true)
                {
                    // Adjust the scroll bar to move up one.
                    newValue = vScrollBar1.Value - 1;

                    if (newValue <= vScrollBar1.Minimum)
                    {
                        newValue = vScrollBar1.Minimum;
                    }

                    vScrollBar1.Value = newValue;

                    lstScrollbarValues_[selectedRoomIndex] = newValue;

                    try
                    {
                        // Determine the number of rows and columns that are fully visible.
                        // Also determine the total number of rows.
                        int totalRows = project.Layers[selectedRoomId].Count;
                        int visibleRows = 0;
                        int offscreenRows = 0;

                        calculateRenderData(ref visibleRows);

                        if (lstOffsets_[selectedRoomIndex] == 0)
                        {
                            if (newValue == vScrollBar1.Maximum)
                            {
                                lstOffsets_[selectedRoomIndex] = ((visibleRows + 1) * pbLayerBG.Height) - (pbLayers.Height);

                                lstRowOffsets_[selectedRoomIndex] += lstOffsets_[selectedRoomIndex] / pbLayerBG.Height;

                                lstOffsets_[selectedRoomIndex] = lstOffsets_[selectedRoomIndex] % pbLayerBG.Height;


                                calculateRenderData(ref visibleRows);

                                offscreenRows = totalRows - visibleRows;

                                if (offscreenRows < 0)
                                {
                                    offscreenRows = 0;
                                }

                                newValue = offscreenRows;

                                vScrollBar1.Maximum = offscreenRows;
                            }
                            else
                            {
                                lstRowOffsets_[selectedRoomIndex] = newValue;
                            }
                        }
                        else
                        {
                            if (newValue == 0)
                            {
                                lstOffsets_[selectedRoomIndex] = 0;
                                lstRowOffsets_[selectedRoomIndex] = 0;
                            }
                            else if (newValue == vScrollBar1.Maximum && (pbLayers.Height - ((pbLayerBG.Height - lstOffsets_[selectedRoomIndex]) + (visibleRows * pbLayerBG.Height))) != 0)
                            {
                                // There is no row alignment. Need to modify the offset by some amount to bottom align the tiles.
                                lstOffsets_[selectedRoomIndex] += pbLayerBG.Height - (pbLayers.Height - ((pbLayerBG.Height - lstOffsets_[selectedRoomIndex]) + (visibleRows * pbLayerBG.Height)));

                                calculateRenderData(ref visibleRows);

                                offscreenRows = totalRows - visibleRows;

                                if (offscreenRows < 0)
                                {
                                    offscreenRows = 0;
                                }

                                newValue = offscreenRows;

                                vScrollBar1.Maximum = offscreenRows;
                            }
                            else
                            {
                                lstRowOffsets_[selectedRoomIndex] = newValue - 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            this.Refresh();
        }       

        private void pbLayers_Paint(object sender, PaintEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            if (project != null && project.IsPrepared == true)
            {   
                string layerName;
                string layerRows;
                string layerColumns;
                string rendertext;

                int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];

                int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);
                
                int layerTop = 0;

                g = e.Graphics;

                int visibleRows = 0;
                calculateRenderData(ref visibleRows);

                int count = 0;
                int start = lstRowOffsets_[selectedRoomIndex];
                int end = start + visibleRows + 2;

                int layerCount = project.Layers[selectedRoomId].Count;

                if (end > layerCount)
                {
                    end = layerCount;
                }

                // Render all the layers currently visible in the control.
                for (int i = start; i < end; i++)
                {
                    int currentLayerIndex = projectController_.GetLayerIndexFromOrdinal(selectedRoomIndex, i);
                    LayerDto currentLayer = projectController_.GetLayerByIndex(selectedRoomIndex, currentLayerIndex); 

                    layerTop = (count * pbLayerBG.Height) - lstOffsets_[selectedRoomIndex];

                    if (i == selectedLayerOrdinal)
                    {
                        for (int j = 0; j < pbLayers.Width; j += pbLayerSelectedBG.Width)
                        {
                            g.DrawImage(pbLayerSelectedBG.Image, j, layerTop);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < pbLayers.Width; j += pbLayerBG.Width)
                        {
                            g.DrawImage(pbLayerBG.Image, j, layerTop);
                        }
                    }

                    if (uiState.LayerVisible[currentLayer.Id] == true)
                    {
                        if (mouseOverVisible_ == true && mouseOverLayer_ == currentLayerIndex)
                        {
                            ilIcons.Draw(g, 10, layerTop + iconsTop_, 2);
                        }
                        else
                        {
                            ilIcons.Draw(g, 10, layerTop + iconsTop_, 0);
                        }
                    }
                    else
                    {
                        if (mouseOverVisible_ == true && mouseOverLayer_ == currentLayerIndex)
                        {
                            ilIcons.Draw(g, 10, layerTop + iconsTop_, 3);
                        }
                        else
                        {
                            ilIcons.Draw(g, 10, layerTop + iconsTop_, 1);
                        }
                    }

                    if (mouseOverDelete_ == true && mouseOverLayer_ == currentLayerIndex)
                    {
                        ilIcons.Draw(g, 36, layerTop + iconsTop_, 5);
                    }
                    else
                    {
                        ilIcons.Draw(g, 36, layerTop + iconsTop_, 4);
                    }


                    if (mouseOverInteractive_ == true && mouseOverLayer_ == currentLayerIndex)
                    {
                        ilIcons.Draw(g, 62, layerTop + iconsTop_, 7);
                    }
                    else
                    {
                        if (project.InteractiveLayerIndexes[selectedRoomId] == currentLayerIndex)
                        {
                            ilIcons.Draw(g, 62, layerTop + iconsTop_, 8);
                        }
                        else
                        {
                            ilIcons.Draw(g, 62, layerTop + iconsTop_, 6);
                        }
                    }

                    if (mouseOverEdit_ == true && mouseOverLayer_ == currentLayerIndex)
                    {
                        ilIcons.Draw(g, 88, layerTop + iconsTop_, 10);
                    }
                    else
                    {
                        ilIcons.Draw(g, 88, layerTop + iconsTop_, 9);                        
                    }

                    if (mouseDragging_)
                    {
                        int layerMouseOverTop = calculateLayerTop(mouseOverLayer_);

                        // Draw the transparent layer.
                        float[][] ptsArray ={ new float[] {1, 0, 0, 0,    0},
                                              new float[] {0, 1, 0, 0,    0},
                                              new float[] {0, 0, 1, 0,    0},
                                              new float[] {0, 0, 0, 0.7f, 0},
                                              new float[] {0, 0, 0, 0,    1}};

                        ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                        ImageAttributes imgAttributes = new ImageAttributes();
                        imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                        Bitmap bmpTemp = new Bitmap(pbLayers.Width, pbLayerSelectedBG.Height);
                        Graphics gTmp = Graphics.FromImage(bmpTemp);

                        for (int j = 0; j < pbLayers.Width; j += pbLayerSelectedBG.Width)
                        {
                            pbLayerSelectedBG.DrawToBitmap(bmpTemp, new System.Drawing.Rectangle(new Point(j, 0), new Size(pbLayerSelectedBG.Width, pbLayerSelectedBG.Height)));
                        }

                        int dragLayerTop = ptMouseOver_.Y - (pbLayerSelectedBG.Height / 2);
                        g.DrawImage(bmpTemp, new System.Drawing.Rectangle(0, dragLayerTop, bmpTemp.Width, bmpTemp.Height), 0, 0, bmpTemp.Width, bmpTemp.Height, GraphicsUnit.Pixel, imgAttributes);

                        // Draw the layer name and size.
                        LayerDto selectedLayer = projectController_.GetLayerByIndex(selectedRoomIndex, selectedLayerIndex);

                        layerName = selectedLayer.Name;
                        layerRows = selectedLayer.Rows.ToString();
                        layerColumns = selectedLayer.Cols.ToString();
                        rendertext = layerName + "  (" + layerRows + "x" + layerColumns + ")";

                        g.DrawString(rendertext, new Font("Tahoma", 9), Brushes.Black, new Point(3, dragLayerTop + 6));

                        if (uiState.LayerVisible[selectedLayer.Id] == true)
                        {
                            ilIcons.Draw(g, 10, dragLayerTop + iconsTop_, 0);
                        }
                        else
                        {
                            ilIcons.Draw(g, 10, dragLayerTop + iconsTop_, 1);
                        }

                        ilIcons.Draw(g, 36, dragLayerTop + iconsTop_, 4);
                        ilIcons.Draw(g, 62, dragLayerTop + iconsTop_, 6);

                        // Draw a cursor, displaying where the selected layer will be dropped.
                        if (mouseOverTopHalf_ == true)
                        {
                            g.DrawLine(new Pen(Color.Red, 6.0f), 0, layerMouseOverTop, this.Width, layerMouseOverTop);
                        }
                        else
                        {
                            g.DrawLine(new Pen(Color.Red, 6.0f), 0, layerMouseOverTop + pbLayerBG.Height, this.Width, layerMouseOverTop + pbLayerBG.Height);
                        }
                    }

                    g.DrawRectangle(new Pen(Color.Black), 0, layerTop, pbLayers.Width - 1, pbLayerBG.Height);

                    layerName = currentLayer.Name;
                    layerRows = currentLayer.Rows.ToString();
                    layerColumns = currentLayer.Cols.ToString();

                    rendertext = layerName + "  (" + layerRows + "x" + layerColumns + ")";

                    g.DrawString(rendertext, new Font("Tahoma", 9), Brushes.Black, new Point(3, layerTop + 6));

                    count++;
                }
            }
             
        }

        private void pbLayers_Click(object sender, EventArgs e)
        {

        }

        private void pbLayers_MouseDown(object sender, MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            int clickedIndex = -1;
            int clickedOrdinal = -1;

            int layerTop = 0;

            int layerCount = project.Layers[selectedRoomId].Count;

            // Loop through the layers until one is found located where the mouse has just clicked.
            for (int i = 0; i < layerCount; i++)
            {
                int currentLayerIndex = projectController_.GetLayerIndexFromOrdinal(selectedRoomIndex, i);

                layerTop = calculateLayerTop(currentLayerIndex);

                if (e.Y >= layerTop && e.Y < layerTop + pbLayerBG.Height)
                {
                    fromLayerOrdinal_ = i;
                    clickedOrdinal = i;
                    clickedIndex = currentLayerIndex;
                    break;
                }
            }

            if (clickedIndex > -1)
            {
                LayerDto clickedLayer = project.Layers[selectedRoomId][clickedIndex];

                int visibleRows = 0;

                calculateRenderData(ref visibleRows);

                int highestVisibleRow = 0;
                bool isBottomAligned = false;

                // Get the total height of all visible rows, and the offset if needed, and compare it to the control height, to see if the last row is partially visible or wholly visible.
                if (lstOffsets_[selectedRoomIndex] == 0)
                {
                    if ((visibleRows * pbLayerBG.Height) < pbLayers.Height)
                    {
                        // If the last visible row is partially visible...
                        highestVisibleRow = lstRowOffsets_[selectedRoomIndex] + visibleRows;
                    }
                    else
                    {
                        highestVisibleRow = lstRowOffsets_[selectedRoomIndex] + visibleRows - 1;

                        // If there's no offset and you can fit an exact amount of tiles in, it is bottom aligned.
                        if (pbLayers.Height % pbLayerBG.Height == 0)
                        {
                            isBottomAligned = true;
                        }
                    }
                }
                else
                {
                    if (((visibleRows * pbLayerBG.Height) + (pbLayerBG.Height - pbLayerBG.Height)) < (this.Height - pnAddNew.Height))
                    {
                        highestVisibleRow = lstRowOffsets_[selectedRoomIndex] + visibleRows + 1;
                    }
                    else
                    {
                        isBottomAligned = true;
                        highestVisibleRow = lstRowOffsets_[selectedRoomIndex] + visibleRows;
                    }
                }

                layerTop = calculateLayerTop(clickedIndex);

                // Check if any buttons were clicked.
                if ((e.Y >= layerTop + iconsTop_ && e.Y <= layerTop + 41) && (e.X >= 10 && e.X <= 26))
                {
                    bool isVisible = !uiState.LayerVisible[clickedLayer.Id];

                    projectController_.SetLayerVisibility(selectedRoomIndex, clickedIndex, isVisible);
                }
                else if ((e.Y >= layerTop + iconsTop_ && e.Y <= layerTop + 41) && (e.X >= 36 && e.X <= 52))
                {
                    if (layerCount == 1)
                    {
                        MessageBox.Show("Unable to delete layer. At least one layer must always exist.", "Unable To Delete", MessageBoxButtons.OK);
                    }
                    else
                    {
                        // Delete the clicked layer.
                        if (MessageBox.Show("Delete " + clickedLayer.Name + "?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            ////   The following code doesn't work. It's not a huge deal, so maybe come back to it later
                            ////   when I can take another look at it.

                            ////   To recreate the problem:
                            ////   1. Create a new map, 800x600.
                            ////   1. Add 3 new layers. 
                            ////   2. Scroll to the bottom so there is an offset.
                            ////   3. Delete the bottom layer.
                            ////   4. There is now whitespace that needs to be filled in.
                            ////   5. This should be true for all list popups.


                            ////// If the highest ordinal layer was deleted, and there was an offset, there may be
                            ////// unnecessary whitespace. Make sure this doesn't happen here.
                            ////int rowOffset = lstRowOffsets_[selectedRoomIndex];
                            ////int offset = lstOffsets_[selectedRoomIndex];
                            ////int layerCount = rcRooms_.SelectedRoom.LayerList.Count;

                            ////if (offset > 0 && clickedOrdinal == layerCount - 1)
                            ////{
                            ////    int rowHeight = pbLayerBG.Height;
                            ////    int listHeight = pbLayers.Height;
                            ////    int totalHeight = layerCount * rowHeight;

                            ////    // "Pull down" the rows until the white space is filled in. 
                            ////    // Shouldn't be more than one row offset adjustment.

                            ////    int newOffset = offset + (rowHeight - (totalHeight - (listHeight + (rowOffset * rowHeight) + offset)));

                            ////    lstRowOffsets_[selectedRoomIndex] = rowOffset - 1;
                            ////    lstOffsets_[selectedRoomIndex] = newOffset;

                            ////    resizeLayerList();
                            ////}

                            /*
                            ObjectSelection os;
                            int layerID = rcRooms_.SelectedRoom.LayerList.Layers[clickedIndex].LayerID;

                            // Loop through each object instance on this layer and explicitly remove it.
                            List<ObjectSelection> lstRemovedActorInstances = new List<ObjectSelection>();

                            int actorInstanceCount = rcRooms_.AssetContainer.ActorInstanceList.Count;

                            for (int i = actorInstanceCount - 1; i >= 0; i--)
                            {
                                os = rcRooms_.AssetContainer.ActorInstanceList[i];

                                if (os.LayerID == layerID)
                                {
                                    lstRemovedActorInstances.Add(os);
                                    rcRooms_.AssetContainer.removeActorInstanceReference(os.InstanceNameID.Id, true);
                                }
                            }

                            List<ObjectSelection> lstRemovedEventInstances = new List<ObjectSelection>();
                            int eventInstanceCount = rcRooms_.AssetContainer.EventInstanceList.Count;

                            for (int i = eventInstanceCount - 1; i >= 0; i--)
                            {
                                os = rcRooms_.AssetContainer.EventInstanceList[i];

                                if (os.LayerID == layerID)
                                {
                                    lstRemovedEventInstances.Add(os);
                                    rcRooms_.AssetContainer.removeEventInstanceReference(os.InstanceNameID.Id, true);
                                }
                            }

                            m.StateData["RemovedActorInstances"] = lstRemovedActorInstances;
                            m.StateData["RemovedEventInstances"] = lstRemovedEventInstances;

                            undoStack_.Push(m);

                            OnActionTaken(new ActionTakenEventArgs());
                            */
                            projectController_.DeleteLayer(selectedRoomIndex, clickedIndex);

                            // Update the scrollbar if necessary.
                            calculateRenderData(ref visibleRows);

                            vScrollBar1.Maximum = project.Layers[selectedRoomId].Count - visibleRows;

                            this.Refresh();
                        }
                    }
                }
                else if ((e.Y >= layerTop + iconsTop_ && e.Y <= layerTop + 41) && (e.X >= 62 && e.X <= 78))
                {
                    // Changing the interactive layer.
                    projectController_.SetInteractiveLayer(selectedRoomIndex, clickedIndex);
                }
                else if ((e.Y >= layerTop + iconsTop_ && e.Y <= layerTop + 41) && (e.X >= 88 && e.X <= 104))
                {
                    // A layer can't be smaller than the camera size.
                    int cameraHeight = project.CameraHeight;
                    int cameraWidth = project.CameraWidth;
                    int tileSize = project.TileSize;

                    editLayerDialog_.CurrentName = clickedLayer.Name;
                    editLayerDialog_.LayerId = clickedLayer.Id;
                    editLayerDialog_.CurrentCols = clickedLayer.Cols;
                    editLayerDialog_.CurrentRows = clickedLayer.Rows;
                    editLayerDialog_.MinRows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cameraHeight) / Convert.ToDouble(tileSize)));
                    editLayerDialog_.MinCols = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cameraWidth) / Convert.ToDouble(tileSize)));
                    editLayerDialog_.ShowDialog(this);
                }
                else
                {
                    if (clickedIndex <= layerCount)
                    {
                        if (lstOffsets_[selectedRoomIndex] == 0)
                        {
                            if (clickedOrdinal >= highestVisibleRow)
                            {
                                if (isBottomAligned == false)
                                {
                                    if (vScrollBar1.Value < vScrollBar1.Maximum)
                                    {
                                        vScrollBar1.Value++;

                                        lstScrollbarValues_[selectedRoomIndex] = vScrollBar1.Value;
                                    }

                                    lstOffsets_[selectedRoomIndex] = ((visibleRows + 1) * pbLayerBG.Height) - pbLayers.Height;

                                    lstRowOffsets_[selectedRoomIndex] += lstOffsets_[selectedRoomIndex] / pbLayerBG.Height;

                                    lstOffsets_[selectedRoomIndex] = lstOffsets_[selectedRoomIndex] % pbLayerBG.Height;
                                }
                            }
                        }
                        else
                        {
                            if (clickedOrdinal == lstRowOffsets_[selectedRoomIndex])
                            {
                                lstOffsets_[selectedRoomIndex] = 0;

                                vScrollBar1.Value--;

                                lstScrollbarValues_[selectedRoomIndex] = vScrollBar1.Value;

                                // Aligning to the bottom may cause number of offscreen rows to change
                                calculateRenderData(ref visibleRows);

                                int offscreenRows = layerCount - visibleRows;

                                if (offscreenRows < 0)
                                {
                                    offscreenRows = 0;
                                }

                                vScrollBar1.Maximum = offscreenRows;

                            }
                            else if (clickedOrdinal == highestVisibleRow)
                            {
                                // If the highest visible row is offscreen at all...
                                if (isBottomAligned == false)
                                {
                                    lstOffsets_[selectedRoomIndex] = ((visibleRows + 2) * pbLayerBG.Height) - pbLayers.Height;

                                    lstRowOffsets_[selectedRoomIndex] += lstOffsets_[selectedRoomIndex] / pbLayerBG.Height;

                                    lstOffsets_[selectedRoomIndex] = lstOffsets_[selectedRoomIndex] % pbLayerBG.Height;

                                    vScrollBar1.Value = lstRowOffsets_[selectedRoomIndex] + 1;

                                    lstScrollbarValues_[selectedRoomIndex] = vScrollBar1.Value;

                                    // Aligning to the bottom may cause number of offscreen rows to change
                                    calculateRenderData(ref visibleRows);

                                    int offscreenRows = layerCount - visibleRows;

                                    if (offscreenRows < 0)
                                    {
                                        offscreenRows = 0;
                                    }

                                    if (vScrollBar1.Value == vScrollBar1.Maximum && offscreenRows > vScrollBar1.Maximum)
                                    {
                                        vScrollBar1.Value -= offscreenRows - vScrollBar1.Maximum;

                                        lstScrollbarValues_[selectedRoomIndex] = vScrollBar1.Value;
                                    }

                                    vScrollBar1.Maximum = offscreenRows;
                                }
                            }
                        }

                        projectController_.SelectLayer(selectedRoomIndex, clickedIndex);
                    }
                    
                    mouseDown_ = true;
                }
            }

            this.Refresh();
        }

        private void pbLayers_MouseMove(object sender, MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            int selectedRoomIndex = uiState.SelectedRoomIndex;
            Guid selectedRoomId = uiState.SelectedRoomId;

            if (mouseDown_ == true)
            {
                mouseDragging_ = true;
                ptMouseOver_.X = e.X;
                ptMouseOver_.Y = e.Y;
            }

            bool foundLayer = false;
            int layerTop = 0;
            int mouseOverLayerTop = 0;

            int layerCount = project.Layers[selectedRoomId].Count;

            // Loop through the layers until one is found located where the mouse has just clicked.
            for (int i = 0; i < layerCount; i++)
            {
                int currentLayerIndex = projectController_.GetLayerIndexFromOrdinal(selectedRoomIndex, i);

                layerTop = calculateLayerTop(currentLayerIndex);

                if (e.Y >= layerTop && e.Y < layerTop + pbLayerBG.Height)
                {
                    mouseOverLayer_ = currentLayerIndex;
                    mouseOverIndex_ = i;
                    foundLayer = true;
                    break;
                }
            }

            if (foundLayer == false)
            {
                // No need to do anything.
                return;
            }

            mouseOverLayerTop = calculateLayerTop(mouseOverLayer_);

            int selectedLayerIndex = uiState.SelectedLayerIndex[selectedRoomId];
            int selectedLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(selectedRoomIndex, selectedLayerIndex);

            if ((e.Y - mouseOverLayerTop) < (pbLayerBG.Height / 2))
            {
                toLayerOrdinal_ = mouseOverIndex_ - 1;

                if (toLayerOrdinal_ < selectedLayerOrdinal)
                {
                    toLayerOrdinal_++;
                }

                mouseOverTopHalf_ = true;
            }
            else
            {
                toLayerOrdinal_ = mouseOverIndex_;

                if (toLayerOrdinal_ < selectedLayerOrdinal)
                {
                    toLayerOrdinal_++;
                }

                mouseOverTopHalf_ = false;
            }

            // Check if mouse is over buttons
            if ((e.Y >= mouseOverLayerTop + iconsTop_ && e.Y <= mouseOverLayerTop + 41) && (e.X >= 10 && e.X <= 26))
            {
                mouseOverVisible_ = true;
            }
            else
            {
                mouseOverVisible_ = false;
            }

            if ((e.Y >= mouseOverLayerTop + iconsTop_ && e.Y <= mouseOverLayerTop + 41) && (e.X >= 36 && e.X <= 52))
            {
                mouseOverDelete_ = true;
            }
            else
            {
                mouseOverDelete_ = false;
            }

            if ((e.Y >= mouseOverLayerTop + iconsTop_ && e.Y <= mouseOverLayerTop + 41) && (e.X >= 62 && e.X <= 78))
            {
                mouseOverInteractive_ = true;
            }
            else
            {
                mouseOverInteractive_ = false;
            }

            if ((e.Y >= mouseOverLayerTop + iconsTop_ && e.Y <= mouseOverLayerTop + 41) && (e.X >= 88 && e.X <= 104))
            {
                mouseOverEdit_ = true;
            }
            else
            {
                mouseOverEdit_ = false;
            }

            int dropPosition = 0;

            // Draw a cursor, displaying where the selected layer will be dropped.
            if (mouseOverTopHalf_ == true)
            {
                dropPosition = mouseOverLayerTop;
            }
            else
            {
                dropPosition = mouseOverLayerTop + pbLayerBG.Height;
            }

            if (mouseDown_ == true)
            {
                // If the drop position is located offscreen, scroll in the appropriate direction.
                bool mouseOffscreen = false;

                scrollUp_ = false;
                scrollDown_ = false;

                if (dropPosition > pbLayers.Height)
                {
                    if ((dropPosition - pbLayers.Height) < 50)
                    {
                        scrollRate_ = 4;
                    }
                    else if ((dropPosition - pbLayers.Height) >= 50 && (dropPosition - pbLayers.Height) < 150)
                    {
                        scrollRate_ = 2;
                    }
                    else
                    {
                        scrollRate_ = 1;
                    }

                    scrollDown_ = true;
                    mouseOffscreen = true;
                }
                else if (dropPosition < 0)
                {
                    if (-dropPosition < 50)
                    {
                        scrollRate_ = 4;
                    }
                    else if (-dropPosition >= 50 && -dropPosition < 150)
                    {
                        scrollRate_ = 2;
                    }
                    else
                    {
                        scrollRate_ = 1;
                    }

                    scrollUp_ = true;
                    mouseOffscreen = true;
                }

                if (mouseOffscreen == false)
                {
                    tmrScroll.Stop();
                }
                else
                {
                    tmrScroll.Start();
                }
            }

            pbLayers.Refresh();
        }

        private void pbLayers_MouseUp(object sender, MouseEventArgs e)
        {
            tmrScroll.Stop();

            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            if (mouseDragging_ == true)
            {
                projectController_.MoveLayer(selectedRoomIndex, fromLayerOrdinal_, toLayerOrdinal_);
            }

            mouseDown_ = false;
            mouseDragging_ = false;

            this.Refresh();
        }

        private void pbLayers_MouseLeave(object sender, EventArgs e)
        {
            mouseOverDelete_ = false;
            mouseOverInteractive_ = false;
            mouseOverVisible_ = false;
            mouseOverEdit_ = false;

            pbLayers.Refresh();
        }

        private void pbAddButton_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (isMouseDownButton_ == true)
            {
                g.DrawImageUnscaled(pbAddDown.Image, 0, 0);
            }
            else if (isMouseOverButton_ == true)
            {
                g.DrawImageUnscaled(pbAddOver.Image, 0, 0);
            }
            else
            {
                g.DrawImageUnscaled(pbAddNormal.Image, 0, 0);
            }
        }

        private void pbAddButton_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDownButton_ = true;

            pbAddButton.Refresh();
        }

        private void pbAddButton_MouseMove(object sender, MouseEventArgs e)
        {
            isMouseOverButton_ = true;

            pbAddButton.Refresh();
        }

        private void pbAddButton_MouseUp(object sender, MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            isMouseDownButton_ = false;

            pbAddButton.Refresh();

            // A layer can't be smaller than the camera size.
            int cameraHeight = project.CameraHeight;
            int cameraWidth = project.CameraWidth;
            int tileSize = project.TileSize;

            newLayerDialog_.MinRows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cameraHeight) / Convert.ToDouble(tileSize)));
            newLayerDialog_.MinCols = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cameraWidth) / Convert.ToDouble(tileSize)));
            newLayerDialog_.ShowDialog(this);
        }

        private void pbAddButton_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverButton_ = false;

            pbAddButton.Refresh();
        }
    }


    public interface ILayerListControl
    {
        event CursorChangedHandler CursorChanged;
        
        // Derived from base.
        bool Enabled { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        int Left { get; set; }
        int Top { get; set; }
        int TabIndex { get; set; }
        string Name { get; set; }
        System.Drawing.Size Size { get; set; }
        System.Drawing.Point Location { get; set; }
        System.Drawing.Font Font { get; set; }

        void Dispose();
        void Refresh();
    }

    public class LayerButtonClickedEventArgs : System.EventArgs
    {
        // Fields
        private LayerButtons button_;

        // Constructor
        public LayerButtonClickedEventArgs(LayerButtons button)
        {
            button_ = button;
        }

        // Properties
        public LayerButtons LayerButton
        {
            get { return button_; }
        }
    }
}