using System;
using System.Collections.Generic;

namespace FiremelonEditor2
{
    public enum EditMode
    {
        Draw = 1,
        Selection = 2,
        Grab = 3
    };
    
    public enum MapWidgetMode
    {
        None = 0,
        Actor = 1,
        Event = 2,
        HudElement = 3,
        SpawnPoint = 4,
        ParticleEmitter = 5,
        AudioSource = 6,
        WorldGeometry = 7,
        TileObject = 8
    };

    /* Notes:
       
        - Remember that when adding to the UI state, changes must be made in generateUiStateFromProject and replaceUiState.

    */
    public class ProjectUiStateDto
    {
        public ProjectUiStateDto()
        {
            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                CanSelectMapWidget.Add(mapWidgetType, true);
            }
        }        

        public int SelectedRoomIndex
        {
            get { return selectedRoomIndex_; }
            set { selectedRoomIndex_ = value; }
        }
        private int selectedRoomIndex_;

        public Guid SelectedRoomId
        {
            get { return selectedRoomId_; }
            set { selectedRoomId_ = value; }
        }
        private Guid selectedRoomId_;

        public Guid SelectedActorId
        {
            get { return selectedActorId_; }
            set { selectedActorId_ = value; }
        }
        private Guid selectedActorId_ = Guid.Empty;

        public int SelectedActorIndex
        {
            get { return selectedActorIndex_; }
            set { selectedActorIndex_ = value; }
        }
        private int selectedActorIndex_ = -1;

        public Guid SelectedEventId
        {
            get { return selectedEventId_; }
            set { selectedEventId_ = value; }
        }
        private Guid selectedEventId_ = Guid.Empty;

        public int SelectedEventIndex
        {
            get { return selectedEventIndex_; }
            set { selectedEventIndex_ = value; }
        }
        private int selectedEventIndex_ = -1;

        public Guid SelectedHudElementId
        {
            get { return selectedHudElementId_; }
            set { selectedHudElementId_ = value; }
        }
        private Guid selectedHudElementId_ = Guid.Empty;

        public int SelectedHudElementIndex
        {
            get { return selectedHudElementIndex_; }
            set { selectedHudElementIndex_ = value; }
        }
        private int selectedHudElementIndex_ = -1;

        public Guid SelectedSpawnPointId
        {
            get { return selectedSpawnPointId_; }
            set { selectedSpawnPointId_ = value; }
        }
        private Guid selectedSpawnPointId_ = Guid.Empty;

        public int SelectedSpawnPointIndex
        {
            get { return selectedSpawnPointIndex_; }
            set { selectedSpawnPointIndex_ = value; }
        }
        private int selectedSpawnPointIndex_ = -1;
        
        public int SelectedTileSheetIndex
        {
            get { return selectedTileSheetIndex_; }
            set { selectedTileSheetIndex_ = value; }
        }
        private int selectedTileSheetIndex_ = -1;

        public Guid SelectedTileSheetId
        {
            get { return selectedTileSheetId_; }
            set { selectedTileSheetId_ = value; }
        }
        private Guid selectedTileSheetId_ = Guid.Empty;

        public int SelectedTileObjectIndex
        {
            get { return selectedTileObjectIndex_; }
            set { selectedTileObjectIndex_ = value; }
        }
        private int selectedTileObjectIndex_ = -1;

        public Guid SelectedTileObjectId
        {
            get { return selectedTileObjectId_; }
            set { selectedTileObjectId_ = value; }
        }
        private Guid selectedTileObjectId_ = Guid.Empty;

        public bool ShowWarnings
        {
            get { return showWarnings_; }
            set { showWarnings_ = value; }
        }
        private bool showWarnings_ = true;

        public Dictionary<Guid, int> SelectedLayerIndex
        {
            get { return dictSelectedLayerIndex_; }
        }
        private Dictionary<Guid, int> dictSelectedLayerIndex_ = new Dictionary<Guid, int>();

        public Dictionary<Guid, int> SelectedTileIndex
        {
            get { return dictSelectedTileIndex_; }
        }
        private Dictionary<Guid, int> dictSelectedTileIndex_ = new Dictionary<Guid, int>();
        
        
        // Stores an index into the LayerDto list. The index at which this is stored
        // is the layer ordinal. 
        public Dictionary<Guid, List<int>> LayerOrdinalToIndexMap
        {
            get { return dictLayerOrdinalToIndexMap_; }
        }
        private Dictionary<Guid, List<int>> dictLayerOrdinalToIndexMap_ = new Dictionary<Guid, List<int>>();

        public Dictionary<Guid, bool> LayerVisible
        {
            get { return dictLayerVisible_; }
        }
        private Dictionary<Guid, bool> dictLayerVisible_ = new Dictionary<Guid, bool>();

        public Dictionary<Guid, int> MaxCols
        {
            get { return dictMaxCols_; }
        }
        private Dictionary<Guid, int> dictMaxCols_ = new Dictionary<Guid, int>();

        public Dictionary<Guid, int> MaxRows
        {
            get { return dictMaxRows_; }
        }
        private Dictionary<Guid, int> dictMaxRows_ = new Dictionary<Guid, int>();

        public Dictionary<MapWidgetType, bool> CanSelectMapWidget
        {
            get { return canSelectMapWidget_; }           
        }
        Dictionary<MapWidgetType, bool> canSelectMapWidget_ = new Dictionary<MapWidgetType, bool>();
        
        public Dictionary<Guid, Point2D> CameraLocation
        {
            get { return dictCameraLocation_; }
        }
        private Dictionary<Guid, Point2D> dictCameraLocation_ = new Dictionary<Guid, Point2D>();

        public Dictionary<Guid, Point2D> CameraLocationMax
        {
            get { return dictCameraLocationMax_; }
        }
        private Dictionary<Guid, Point2D> dictCameraLocationMax_ = new Dictionary<Guid, Point2D>();

        public Dictionary<Guid, Point2D> CanvasOffset
        {
            get { return dictCanvasOffset_; }
        }
        private Dictionary<Guid, Point2D> dictCanvasOffset_ = new Dictionary<Guid, Point2D>();

        public Dictionary<Guid, Point2D> CanvasOffsetMax
        {
            get { return dictCanvasOffsetMax_; }
        }
        private Dictionary<Guid, Point2D> dictCanvasOffsetMax_ = new Dictionary<Guid, Point2D>();

        public Dictionary<Guid, Point2D> TileObjectCursorCell
        {
            get { return dictTileObjectCursorCell_; }
        }
        private Dictionary<Guid, Point2D> dictTileObjectCursorCell_ = new Dictionary<Guid, Point2D>();
                
        public Dictionary<Guid, MapWidgetSelectorDto> MapWidgetSelector
        {
            get { return dictMapWidgetSelector_; }
        }
        private Dictionary<Guid, MapWidgetSelectorDto> dictMapWidgetSelector_ = new Dictionary<Guid, MapWidgetSelectorDto>();
        
        public  Dictionary<Guid, bool> MapWidgetSelected
        {
            get { return dictMapWidgetSelected_; }
        }
        private Dictionary<Guid, bool> dictMapWidgetSelected_ = new Dictionary<Guid, bool>();
        
        public Dictionary<Guid, Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>>> RoomMapWidgetsByType
        {
            get { return dictRoomMapWidgetsByType_; }
        }
        private Dictionary<Guid, Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>>> dictRoomMapWidgetsByType_ = new Dictionary<Guid, Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>>>();
        
        public Dictionary<Guid, SortableBindingList<MapWidgetDto>> RoomMapWidgets
        {
            get { return dictRoomMapWidgets_; }
        }
        private Dictionary<Guid,SortableBindingList<MapWidgetDto>> dictRoomMapWidgets_ = new Dictionary<Guid, SortableBindingList<MapWidgetDto>>();

        public CameraMode CameraMode
        {
            get { return cameraMode_; }
            set { cameraMode_ = value; }
        }
        private CameraMode cameraMode_ = CameraMode.CameraLocked;

        public EditMode EditMode
        {
            get { return editMode_; }
            set { editMode_ = value; }
        }
        private EditMode editMode_ = EditMode.Draw;
        
        public Dictionary<Guid, MapWidgetMode> MapWidgetMode
        {
            get { return dictMapWidgetMode_; }
        }
        private Dictionary<Guid, MapWidgetMode> dictMapWidgetMode_ = new Dictionary<Guid, MapWidgetMode>();

        public Dictionary<Guid, MapWidgetType> SelectedMapWidgetType
        {
            get { return dictSelectedMapWidgetType_; }
        }
        private Dictionary<Guid, MapWidgetType> dictSelectedMapWidgetType_ = new Dictionary<Guid, MapWidgetType>();

        public bool ShowGrid
        {
            get { return showGrid_; }
            set { showGrid_ = value; }
        }
        private bool showGrid_ = true;

        public bool ShowCameraOutline
        {
            get { return showCameraOutline_; }
            set { showCameraOutline_ = value; }
        }
        private bool showCameraOutline_ = true;

        public bool ShowOutlines
        {
            get { return showOutlines_; }
            set { showOutlines_ = value; }
        }
        private bool showOutlines_ = true;

        public bool ShowWorldGeometry
        {
            get { return showWorldGeometry_; }
            set { showWorldGeometry_ = value; }
        }
        private bool showWorldGeometry_ = true;

        public bool TransparentSelect
        {
            get { return transparentSelect_; }
            set { transparentSelect_ = value; }
        }
        private bool transparentSelect_ = true;

        public bool ShowMouseOver
        {
            get { return showMouseOver_; }
            set { showMouseOver_ = value; }
        }
        private bool showMouseOver_ = true;

        // Used to track which external resources are currently loaded by different forms.
        public HashSet<Guid> BitmapsLoadedForRoom
        {
            get { return bitmapsLoadedForRoom_; }
            set { bitmapsLoadedForRoom_ = value; }
        }
        private HashSet<Guid> bitmapsLoadedForRoom_ = new HashSet<Guid>();                
    }
}
