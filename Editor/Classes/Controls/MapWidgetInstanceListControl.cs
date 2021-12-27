using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class MapWidgetInstanceListControl : UserControl, IMapWidgetInstanceListControl
    {
        public MapWidgetInstanceListControl(IProjectController projectController, IMapWidgetPropertiesControl propertiesControl)
        {
            InitializeComponent();

            projectController_ = projectController;

            projectController_.MapWidgetSelectionChange += new MapWidgetSelectionChangeHandler(this.MapWidgetInstanceListControl_MapWidgetSelectionChanged);
            projectController_.ProjectCreated += new ProjectCreateHandler(this.MapWidgetInstanceListControl_ProjectCreated);
            projectController_.ProjectStateChanged += new ProjectStateChangeHandler(this.MapWidgetInstanceListControl_ProjectStateChanged);
            projectController_.RoomSelected += new RoomSelectHandler(this.MapWidgetInstanceListControl_RoomSelected);

            propertiesControl_ = propertiesControl;

            listChangedEventHandler_ = new ListChangedEventHandler(this.MapWidgetInstanceListControl_ListChanged);
            beforeListSortedEventHandler_ = new BeforeListSortedHandler(this.MapWidgetInstanceListControl_BeforeListSorted);

            suppressSelectedIndexChanged_ = false;
            suppresSetSelectedListItem_ = false;
            //suppressMapWidgetSelection_ = false;
        }

        private IProjectController projectController_;

        private IMapWidgetPropertiesControl propertiesControl_;

        private ListChangedEventHandler listChangedEventHandler_;

        private BeforeListSortedHandler beforeListSortedEventHandler_;

        private bool suppresSetSelectedListItem_;

        private bool suppressSelectedIndexChanged_;

        #region Event Handlers

        private void lbxInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressSelectedIndexChanged_ == false)
            {
                ProjectUiStateDto uiState = projectController_.GetUiState();

                suppresSetSelectedListItem_ = true;
                
                int selectedRoomIndex = uiState.SelectedRoomIndex;

                // Don't run the node selection code after clearing. This causes the selected
                // node to be set to null.
                //suppressNodeSelection_ = true;

                projectController_.ClearMapWidgetSelection(selectedRoomIndex);

                List<Guid> lstSelectedMapWidgetIds = new List<Guid>();

                foreach (MapWidgetDto mapWidget in lbxInstances.SelectedItems)
                {
                    lstSelectedMapWidgetIds.Add(mapWidget.Id);
                }

                projectController_.AddMapWidgetsToSelection(selectedRoomIndex, lstSelectedMapWidgetIds);

                suppresSetSelectedListItem_ = false;
                //suppressNodeSelection_ = false;
            }
        }

        public void MapWidgetInstanceListControl_BeforeListSorted(object sender, BeforeListSortedEventArgs e)
        {
        }

        public void MapWidgetInstanceListControl_ListChanged(object sender, ListChangedEventArgs e)
        {
            // The listbox selection is going to be messed up. Clear it and re-select
            // the correct items.
            ProjectUiStateDto uiState = projectController_.GetUiState();
            int selectedRoomIndex = uiState.SelectedRoomIndex;
            
            //setSelectedMapWidgetListItems(selectedRoomIndex);
            
            //suppressMapWidgetSelection_ = false;
        }

        public void MapWidgetInstanceListControl_MapWidgetSelectionChanged(object sender, MapWidgetSelectionChangedEventArgs e)
        {
            setSelectedMapWidgetListItems(e.RoomIndex);
        }

        public void MapWidgetInstanceListControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
        {

            // Binding the list boxes for some reason selects the first item. I don't want it to do this.

            // Ignore the selected index changed event, and set the selected index back to -1 after binding.
            suppressSelectedIndexChanged_ = true;

            bindListBoxes();

            // Re-select the selected widgets in this room.
            ProjectUiStateDto uiState = projectController_.GetUiState();

            setSelectedMapWidgetListItems(uiState.SelectedRoomIndex);

            suppressSelectedIndexChanged_ = false;
        }

        public void MapWidgetInstanceListControl_ProjectStateChanged(object sender, ProjectStateChangedEventArgs e)
        {
            // Do I need to rebind the listboxes when the project state changes?? Seems like overkill, because the project state changes
            // for everything, not just widget additions or deletions.
            //bindListBoxes();
        }

        private void MapWidgetInstanceListControl_RoomSelected(object sender, RoomSelectedEventArgs e)
        {
            suppressSelectedIndexChanged_ = true;

            bindListBoxes();

            // Re-select the selected widgets in this room.
            ProjectUiStateDto uiState = projectController_.GetUiState();

            setSelectedMapWidgetListItems(uiState.SelectedRoomIndex);


            suppressSelectedIndexChanged_ = false;
        }

        #endregion

        #region Private Functions

        private void bindListBoxes()
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;
            
            //suppressMapWidgetSelection_ = true;
            uiState.RoomMapWidgets[selectedRoomId].ResetBindings();
            lbxInstances.DisplayMember = "Name";
            lbxInstances.ValueMember = "Name";

            //suppressMapWidgetSelection_ = false;
            lbxInstances.DataSource = uiState.RoomMapWidgets[selectedRoomId];
            uiState.RoomMapWidgets[selectedRoomId].ListChanged -= listChangedEventHandler_;
            uiState.RoomMapWidgets[selectedRoomId].ListChanged += listChangedEventHandler_;

            uiState.RoomMapWidgets[selectedRoomId].BeforeListSorted -= beforeListSortedEventHandler_;
            uiState.RoomMapWidgets[selectedRoomId].BeforeListSorted += beforeListSortedEventHandler_;
        }

        private void setSelectedMapWidgetListItems(int roomIndex)
        {
            // Setting the selected node will fire the SelectedIndexChanged event of 
            // which then calls the select method of the projectController, which leads to this function
            // being called again, resulting in an infinite loop.

            // Set a boolean flag to supress the  here, and restore it at the end of this function.

            //suppressMapWidgetSelection_ = true;
            suppressSelectedIndexChanged_ = true;

            //if (suppressNodeSelection_ == false)
            //{
            //    lbMapWidgets.SelectedIndex = -1;
            //}

            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            Guid selectedRoomId = uiState.SelectedRoomId;

            Guid roomId = project.Rooms[roomIndex].Id;

            if (roomId == selectedRoomId)
            {
                List<MapWidgetProperties> lstSelectedWidgetProperties = new List<MapWidgetProperties>();
                
                MapWidgetSelectorDto selector = uiState.MapWidgetSelector[roomId];

                if (selector.SelectedMapWidgetIds.Count == 0)
                {
                    if (suppresSetSelectedListItem_ == false)
                    {
                        lbxInstances.SelectedIndex = -1;
                    }
                }
                else
                {
                    foreach (Guid mapWidgetId in selector.SelectedMapWidgetIds)
                    {
                        MapWidgetProperties properties = (MapWidgetProperties)project.MapWidgetProperties[mapWidgetId];

                        lstSelectedWidgetProperties.Add(properties);

                        if (suppresSetSelectedListItem_ == false)
                        {
                            for (int i = 0; i < lbxInstances.Items.Count; i++)
                            {
                                if (((MapWidgetDto)lbxInstances.Items[i]).Id == mapWidgetId)
                                {
                                    lbxInstances.SetSelected(i, true);
                                }
                            }
                        }
                    }
                }

                propertiesControl_.SelectedObjects = lstSelectedWidgetProperties.ToArray();
            }

            //suppressMapWidgetSelection_ = false;
            suppressSelectedIndexChanged_ = false;
        }

        #endregion

    }
}
