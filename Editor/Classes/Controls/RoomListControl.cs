using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public delegate void MenuOpeningHandler(object sender, MenuOpeningEventArgs e);
    public delegate void MenuClosedHandler(object sender, MenuClosedEventArgs e);

    public partial class RoomListControl : UserControl, IRoomListControl
    {
        private Graphics g;

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private INameValidator nameValidator_;

        private IProjectController projectController_;

        private IRoomPropertiesEditor roomPropertiesEditor_;

        private int rowOffset_;
        private int offset_;
        private int mouseOverRow_;
        private int iconsTop_;
        private int iconsLeft_;
        private int iconSpacing_;

        private bool isMouseOverDelete_;
        private bool isMouseOverInitial_;
        private bool changesMade_;
        private bool isMouseOverButton_;
        private bool isMouseDownButton_;

        //private RoomContainerClearHandler roomContainerClearHandler_;
        
        private INewRoomDialog newRoomDialog_;
        
        public RoomListControl(IProjectController projectController, INameValidator nameValidator)
        {
            InitializeComponent();

            nameValidator_ = nameValidator;

            projectController_ = projectController;

            vScrollBar1.Maximum = 0;
            vScrollBar1.Minimum = 0;
            vScrollBar1.SmallChange = 1;
            vScrollBar1.LargeChange = 1;

            rowOffset_ = 0;
            offset_ = 0;
            mouseOverRow_ = -1;
            iconsTop_ = 5;
            iconsLeft_ = 120;
            iconSpacing_ = 24;

            isMouseOverDelete_ = false;
            isMouseOverInitial_ = false;
            changesMade_ = false;
            isMouseOverButton_ = false;
            isMouseDownButton_ = false;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            roomPropertiesEditor_ = firemelonEditorFactory_.NewRoomPropertiesEditor();

            projectController_.RoomAdded += new RoomAddHandler(this.RoomListControl_RoomAdded);
            projectController_.BeforeRoomDeleted += new BeforeRoomDeletedHandler(this.RoomListControl_BeforeRoomDeleted);

            //roomContainerClearHandler_ = new RoomContainerClearHandler(this.RoomListControl_RoomContainerCleared);

            newRoomDialog_ = firemelonEditorFactory_.NewNewRoomDialog(projectController_, nameValidator_);

            newRoomDialog_.NewRoom += new NewRoomHandler(this.RoomListControl_NewRoom);
        }

        private void RoomListControl_RoomAdded(object sender, RoomAddedEventArgs e)
        {
            resizeRoomList();
        }

        private void RoomListControl_BeforeRoomDeleted(object sender, BeforeRoomDeletedEventArgs e)
        {
            resizeRoomList();
        }

        //private void RoomListControl_RoomContainerCleared(object sender, RoomContainerClearedEventArgs e)
        //{
        //    vScrollBar1.Maximum = 0;
        //    vScrollBar1.Minimum = 0;
        //    vScrollBar1.SmallChange = 1;
        //    vScrollBar1.LargeChange = 1;
        //}

        private void RoomListControl_NewRoom(object sender, NewRoomEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            for (int i = 0; i < project.Rooms.Count; i++)
            {
                RoomDto room = project.Rooms[i];

                if (e.RoomName.ToUpper() == room.Name.ToUpper())
                {
                    e.Cancel = true;
                    break;
                }
            }

            if (e.Cancel == false)
            {
                projectController_.AddRoom(e.RoomName, e.LayerName, e.LayerCols, e.LayerRows);

                updateScrollbar();

                pbRoomList.Refresh();
            }
        }

        private void RoomListControl_Resize(object sender, EventArgs e)
        {
            resizeRoomList();
        }

        private void resizeRoomList()
        {
            ProjectDto project = projectController_.GetProjectDto();

            pnAddNew.Width = this.Width;

            vScrollBar1.Left = this.Width - vScrollBar1.Width;
            vScrollBar1.Height = this.Height - pnAddNew.Height;

            pbRoomList.Width = this.Width - vScrollBar1.Width;
            pbRoomList.Height = this.Height - pnAddNew.Height;

            if (project != null && project.IsPrepared == true)
            {
                // The total number of rows.
                int totalRows = project.Rooms.Count;

                // The number of rows that are fully visible...
                int tempSize = 0;

                if (offset_ > 0)
                {
                    tempSize = pbRoomBG.Height;
                }

                int visibleRows = 0;

                calculateRenderData(ref visibleRows);

                // If there is whitespace and the rowOffset is not zero and the offset is also not zero, drag the rows down to fill the whitespace.
                // Adjust the variables and scrollbar accordingly.       

                bool adjustScrollValues = false;
                int whiteSpaceHeight = 0;

                if (offset_ == 0)
                {
                    whiteSpaceHeight = (this.Height - pnAddNew.Height) - ((totalRows - rowOffset_) * pbRoomBG.Height);

                    if (whiteSpaceHeight > 0 && (rowOffset_ > 0 || (rowOffset_ == 0 && offset_ > 0)))
                    {
                        adjustScrollValues = true;
                    }
                }
                else
                {
                    whiteSpaceHeight = (this.Height - pnAddNew.Height) - ((pbRoomBG.Height - offset_) + ((totalRows - (rowOffset_ + 1)) * pbRoomBG.Height));

                    if (whiteSpaceHeight > 0 && (rowOffset_ > 0 || (rowOffset_ == 0 && offset_ > 0)))
                    {
                        adjustScrollValues = true;
                    }
                }

                if (adjustScrollValues == true)
                {
                    // "Pull down" the rows so that they occupy the whitespace. Adjust offsets and scrollbar accordingly.

                    // Calculate from the whitespace height, how many new rows can fit, and then what the leftover offset will be.
                    int rowAdjust = whiteSpaceHeight / pbRoomBG.Height;

                    int offsetAdjust = whiteSpaceHeight % pbRoomBG.Height;

                    rowOffset_ -= rowAdjust;

                    if (rowOffset_ < 0)
                    {
                        rowOffset_ = 0;
                        offset_ = 0;
                    }
                    else
                    {
                        offset_ -= offsetAdjust;

                        if (offset_ < 0)
                        {
                            if (rowOffset_ == 0)
                            {
                                offset_ = 0;
                            }
                            else
                            {
                                offset_ = pbRoomBG.Height - 1;
                            }

                            if (rowOffset_ > 0)
                            {
                                rowOffset_--;
                            }

                        }
                    }
                }

                int offscreenRows = totalRows - visibleRows;

                if (offscreenRows < 0)
                {
                    offscreenRows = 0;
                }

                vScrollBar1.Maximum = offscreenRows;
            }

            this.Refresh();
        }

        private void RoomListControl_Load(object sender, EventArgs e)
        {
            this.Refresh();
        }

        protected virtual void OnCursorChanged(CursorChangedEventArgs e)
        {
            //CursorChanged(this, e);
        }

        private void pbRoomList_Paint(object sender, PaintEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project != null && project.IsPrepared == true)
            {
                int roomCount = project.Rooms.Count;
                int selectedRoomIndex = uiState.SelectedRoomIndex;
                int layerTop = 0;

                g = e.Graphics;

                int visibleRows = 0;
                calculateRenderData(ref visibleRows);

                int count = 0;
                int start = rowOffset_;
                int end = start + visibleRows + 2;

                if (end > roomCount)
                {
                    end = roomCount;
                }

                for (int i = start; i < end; i++)
                {
                    RoomDto room = project.Rooms[i];

                    layerTop = (count * pbRoomBG.Height) - offset_;

                    if (i == selectedRoomIndex)
                    {
                        g.DrawImage(pbRoomSelectedBG.Image, 0, layerTop);
                    }
                    else
                    {
                        g.DrawImage(pbRoomBG.Image, 0, layerTop);
                    }

                    if (isMouseOverDelete_ == true && mouseOverRow_ == i)
                    {
                        ilIcons.Draw(g, iconsLeft_, layerTop + iconsTop_, 1);
                    }
                    else
                    {
                        ilIcons.Draw(g, iconsLeft_, layerTop + iconsTop_, 0);
                    }

                    if (isMouseOverInitial_ == true && mouseOverRow_ == i)
                    {
                        ilIcons.Draw(g, iconsLeft_ + iconSpacing_, layerTop + iconsTop_, 3);
                    }
                    else
                    {
                        if (project.InitialRoomId == room.Id)
                        {
                            ilIcons.Draw(g, iconsLeft_ + iconSpacing_, layerTop + iconsTop_, 4);
                        }
                        else
                        {
                            ilIcons.Draw(g, iconsLeft_ + iconSpacing_, layerTop + iconsTop_, 2);
                        }
                    }

                    
                    g.DrawRectangle(new Pen(Color.Black), 0, layerTop, pbRoomList.Width - 1, pbRoomBG.Height);

                    g.DrawString(room.Name, new Font("Tahoma", 9), Brushes.Black, new Point(3, layerTop + 6));

                    count++;
                }
            }
        }

        private void pbRoomList_MouseDown(object sender, MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            int clickedRoom = ((e.Y + ((rowOffset_ * pbRoomBG.Height) + offset_)) / pbRoomBG.Height);

            int roomCount = project.Rooms.Count;

            if (clickedRoom < roomCount)
            {
                int visibleRows = 0;

                calculateRenderData(ref visibleRows);

                int highestVisibleRow = 0;
                bool isBottomAligned = false;

                // Get the total height of all visible rows, and the offset if needed, and compare it to the control height, to see if the last row is partially visible or wholly visible.
                if (offset_ == 0)
                {
                    if ((visibleRows * pbRoomBG.Height) < (this.Height - pnAddNew.Height))
                    {
                        // If the last visible row is partially visible...
                        highestVisibleRow = rowOffset_ + visibleRows;
                    }
                    else
                    {
                        highestVisibleRow = rowOffset_ + visibleRows - 1;

                        // If there's no offset and you can fit an exact amount of rows in, it is bottom aligned.
                        if ((this.Height - pnAddNew.Height) % pbRoomBG.Height == 0)
                        {
                            isBottomAligned = true;
                        }
                    }
                }
                else
                {
                    if (((visibleRows * pbRoomBG.Height) + (pbRoomBG.Height - pbRoomBG.Height)) < (this.Height - pnAddNew.Height))
                    {
                        highestVisibleRow = rowOffset_ + visibleRows + 1;
                    }
                    else
                    {
                        isBottomAligned = true;
                        highestVisibleRow = rowOffset_ + visibleRows;
                    }
                }

                if (e.Button == MouseButtons.Left)
                {
                    // Check if any buttons were clicked.
                    int layerTop = ((mouseOverRow_ * pbRoomBG.Height) - offset_) - (rowOffset_ * pbRoomBG.Height);

                    // Delete room.
                    if (isMouseOverDelete_ == true)
                    {
                        if (project.Rooms.Count == 1)
                        {
                            MessageBox.Show("Unable to delete room. At least one room must always exist.", "Unable To Delete", MessageBoxButtons.OK);
                        }
                        else
                        {
                            if (MessageBox.Show("Delete " + project.Rooms[clickedRoom].Name + "?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {                
                                projectController_.DeleteRoom(clickedRoom);

                                updateScrollbar();

                                this.Refresh();
                            }
                        }

                        return;
                    }

                    // Set initial room.
                    if (isMouseOverInitial_ == true)
                    {
                        Guid clickedRoomId = project.Rooms[clickedRoom].Id;

                        projectController_.SetInitialRoomId(clickedRoomId);

                        this.Refresh();
                            
                        return;
                    }

                    if (clickedRoom <= roomCount)
                    {
                        if (offset_ == 0)
                        {
                            if (clickedRoom >= highestVisibleRow)
                            {
                                if (isBottomAligned == false)
                                {
                                    if (vScrollBar1.Value < vScrollBar1.Maximum)
                                    {
                                        vScrollBar1.Value++;
                                    }

                                    offset_ = ((visibleRows + 1) * pbRoomBG.Height) - (this.Height - pnAddNew.Height);

                                    rowOffset_ += offset_ / pbRoomBG.Height;

                                    offset_ = offset_ % pbRoomBG.Height;
                                }
                            }
                        }
                        else
                        {
                            if (clickedRoom == rowOffset_)
                            {
                                offset_ = 0;
                                vScrollBar1.Value--;

                                // Aligning to the bottom may cause number of offscreen rows to change
                                calculateRenderData(ref visibleRows);

                                int offscreenRows = roomCount - visibleRows;

                                if (offscreenRows < 0)
                                {
                                    offscreenRows = 0;
                                }

                                vScrollBar1.Maximum = offscreenRows;

                            }
                            else if (clickedRoom == highestVisibleRow)
                            {
                                // If the highest visible row is offscreen at all...
                                if (isBottomAligned == false)
                                {
                                    offset_ = ((visibleRows + 2) * pbRoomBG.Height) - (this.Height - pnAddNew.Height);

                                    rowOffset_ += offset_ / pbRoomBG.Height;

                                    offset_ = offset_ % pbRoomBG.Height;

                                    vScrollBar1.Value = rowOffset_ + 1;

                                    // Aligning to the bottom may cause number of offscreen rows to change
                                    calculateRenderData(ref visibleRows);

                                    int offscreenRows = roomCount - visibleRows;

                                    if (offscreenRows < 0)
                                    {
                                        offscreenRows = 0;
                                    }

                                    if (vScrollBar1.Value == vScrollBar1.Maximum && offscreenRows > vScrollBar1.Maximum)
                                    {
                                        vScrollBar1.Value -= offscreenRows - vScrollBar1.Maximum;
                                    }

                                    vScrollBar1.Maximum = offscreenRows;
                                }
                            }
                        }

                        if (selectedRoomIndex != clickedRoom)
                        {
                            projectController_.SelectRoom(clickedRoom);
                        }
                    }
                }
            }

            this.Refresh();
        }

        private void updateScrollbar()
        {
            ProjectDto project = projectController_.GetProjectDto();

            if (project != null && project.IsPrepared == true)
            {
                int visibleRows = 0;
                calculateRenderData(ref visibleRows);

                vScrollBar1.Maximum = project.Rooms.Count - visibleRows;
            }
        }

        private void calculateRenderData(ref int visibleRows)
        {
            ProjectDto project = projectController_.GetProjectDto();

            int objectCount = project.Rooms.Count;

            int totalRows = objectCount;

            // The number of rows that are fully visible...
            int temp = 0;
            int tempSize = 0;

            if (offset_ > 0)
            {
                temp = 1;
                tempSize = pbRoomBG.Height;
            }

            visibleRows = 0;

            int size = tempSize - offset_;

            for (int i = rowOffset_ + temp; i < totalRows; i++)
            {
                size += pbRoomBG.Height;

                if (size > (this.Height - pnAddNew.Height))
                {
                    break;
                }

                visibleRows++;
            }

            return;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (e.OldValue != e.NewValue)
                {
                    // Determine the number of rows and columns that are fully visible.
                    // Also determine the total number of rows.
                    ProjectDto project = projectController_.GetProjectDto();

                    int totalRows = project.Rooms.Count;
                    int visibleRows = 0;
                    int offscreenRows = 0;

                    calculateRenderData(ref visibleRows);

                    if (offset_ == 0)
                    {
                        if (e.NewValue == vScrollBar1.Maximum)
                        {
                            offset_ = ((visibleRows + 1) * pbRoomBG.Height) - (this.Height - pnAddNew.Height);

                            rowOffset_ += offset_ / pbRoomBG.Height;

                            offset_ = offset_ % pbRoomBG.Height;


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
                            rowOffset_ = e.NewValue;
                        }
                    }
                    else
                    {
                        if (e.NewValue == 0)
                        {
                            offset_ = 0;
                            rowOffset_ = 0;
                        }
                        else if (e.NewValue == vScrollBar1.Maximum && ((this.Height - pnAddNew.Height) - ((pbRoomBG.Height - offset_) + (visibleRows * pbRoomBG.Height))) != 0)
                        {
                            // There is no row alignment. Need to modify the offset by some amount to bottom align the rows.
                            offset_ += pbRoomBG.Height - ((this.Height - pnAddNew.Height) - ((pbRoomBG.Height - offset_) + (visibleRows * pbRoomBG.Height)));

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
                            rowOffset_ = e.NewValue - 1;
                        }
                    }
                }

                this.Refresh();
            }
            catch (Exception ex)
            {
            }
        }

        private void pbRoomList_MouseMove(object sender, MouseEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();

            if (project == null || project.IsPrepared == false)
            {
                return;
            }

            int roomCount = project.Rooms.Count;

            // Check which object the mouse is over. Loop through the layers until found.
            mouseOverRow_ = ((e.Y + ((rowOffset_ * pbRoomBG.Height) + offset_)) / pbRoomBG.Height);

            if (mouseOverRow_ >= roomCount)
            {
                mouseOverRow_ = roomCount - 1;
            }

            if (mouseOverRow_ < 0)
            {
                mouseOverRow_ = 0;
            }

            int layerTop = ((mouseOverRow_ * pbRoomBG.Height) - offset_) - (rowOffset_ * pbRoomBG.Height);

            // Mouse is within the vertical range of the buttons. Now check the horizontal.
            if (e.Y >= layerTop + iconsTop_ && e.Y <= layerTop + (pbRoomBG.Height - iconsTop_))
            {
                if (e.X >= iconsLeft_ && e.X <= iconsLeft_ + ilIcons.Images[0].Size.Width)
                {
                    isMouseOverDelete_ = true;
                }
                else
                {
                    isMouseOverDelete_ = false;
                }

                if (e.X >= iconsLeft_ + iconSpacing_ && e.X <= iconsLeft_ + iconSpacing_ + ilIcons.Images[0].Size.Width)
                {
                    isMouseOverInitial_ = true;
                }
                else
                {
                    isMouseOverInitial_ = false;
                }
            }
            else
            {
                isMouseOverDelete_ = false;
                isMouseOverInitial_ = false;
            }

            pbRoomList.Refresh();
        }

        private void mnuAddNewRoom_Click(object sender, EventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            newRoomDialog_.CameraHeight = project.CameraHeight;
            newRoomDialog_.CameraWidth = project.CameraWidth;
            newRoomDialog_.TileSize = project.TileSize;
            newRoomDialog_.ShowDialog(this);
        }

        private void vScrollBar1_Scroll_1(object sender, ScrollEventArgs e)
        {
            try
            {
                if (e.OldValue != e.NewValue)
                {
                    // Determine the number of rows that are fully visible.
                    // Also determine the total number of rows.
                    ProjectDto project = projectController_.GetProjectDto();

                    if (project == null || project.IsPrepared == false)
                    {
                        return;
                    }

                    int totalRows = project.Rooms.Count;
                    int visibleRows = 0;
                    int offscreenRows = 0;

                    calculateRenderData(ref visibleRows);

                    if (offset_ == 0)
                    {
                        if (e.NewValue == vScrollBar1.Maximum)
                        {
                            offset_ = ((visibleRows + 1) * pbRoomBG.Height) - (pbRoomList.Height);

                            rowOffset_ += offset_ / pbRoomBG.Height;

                            offset_ = offset_ % pbRoomBG.Height;

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
                            rowOffset_ = e.NewValue;
                        }
                    }
                    else
                    {
                        if (e.NewValue == 0)
                        {
                            offset_ = 0;
                            rowOffset_ = 0;
                        }
                        else if (e.NewValue == vScrollBar1.Maximum && (pbRoomList.Height - ((pbRoomBG.Height - offset_) + (visibleRows * pbRoomBG.Height))) != 0)
                        {
                            // There is no row alignment. Need to modify the offset by some amount to bottom align the tiles.
                            offset_ += pbRoomBG.Height - (pbRoomList.Height - ((pbRoomBG.Height - offset_) + (visibleRows * pbRoomBG.Height)));

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
                            rowOffset_ = e.NewValue - 1;
                        }
                    }
                }

                vScrollBar1.Value = e.NewValue;

                this.Refresh();
            }
            catch (Exception ex)
            {
            }
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
            isMouseDownButton_ = false;

            pbAddButton.Refresh();

            ProjectDto project = projectController_.GetProjectDto();
            newRoomDialog_.CameraHeight = project.CameraHeight;
            newRoomDialog_.CameraWidth = project.CameraWidth;
            newRoomDialog_.TileSize = project.TileSize;
            newRoomDialog_.ShowDialog(this);
        }

        private void pbAddButton_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverButton_ = false;

            pbAddButton.Refresh();
        }

        private void pbRoomList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                mnuRoomProperties.Show(Cursor.Position);
            }
        }

        private void roomPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;

            if (selectedRoomId != Guid.Empty)
            {
                roomPropertiesEditor_.ShowDialog(this, projectController_, selectedRoomId);
            }
        }
    }

    public class MenuOpeningEventArgs : System.EventArgs
    {
        // Fields

        // Constructor
        public MenuOpeningEventArgs()
        {
        }

        // Properties
    }

    public class MenuClosedEventArgs : System.EventArgs
    {
        // Fields

        // Constructor
        public MenuClosedEventArgs()
        {
        }

        // Properties
    }
}
