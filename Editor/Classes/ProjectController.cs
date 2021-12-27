using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DragonOgg.MediaPlayer;
using System.ComponentModel;
using System.IO.Compression;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    #region Enums

    public enum EditorModule
    {
        Room = 1,
        Cursor = 2,
        TileSheetViewer = 4,
        SpriteSheetViewer = 8,
        AnimationFrameViewer = 16,
        StateEditorControl = 32
    };

    #endregion

    #region Delegates
    public delegate void ProjectCreateHandler(object sender, ProjectCreatedEventArgs e);
    public delegate void ProjectStateChangeHandler(object sender, ProjectStateChangedEventArgs e);

    public delegate void RoomAddHandler(object sender, RoomAddedEventArgs e);
    public delegate void BeforeRoomDeletedHandler(object sender, BeforeRoomDeletedEventArgs e);
    public delegate void RoomSelectHandler(object sender, RoomSelectedEventArgs e);

    public delegate void CameraModeChangedHandler(object sender, CameraModeChangedEventArgs e);
    public delegate void EditModeChangedHandler(object sender, EditModeChangedEventArgs e);
    public delegate void SelectionToggleHandler(object sender, SelectionToggleEventArgs e);

    public delegate void RefreshViewHandler(object sender, RefreshViewEventArgs e);
    public delegate void RefreshPropertiesHandler(object sender, RefreshPropertiesEventArgs e);

    public delegate void TileObjectSelectHandler(object sender, TileObjectSelectedEventArgs e);

    public delegate void LayerSelectHandler(object sender, LayerSelectedEventArgs e);
    public delegate void LayerAddHandler(object sender, LayerAddedEventArgs e);
    public delegate void LayerResizeHandler(object sender, LayerResizedEventArgs e);
    public delegate void BeforeLayerDeleteHandler(object sender, BeforeLayerDeletedEventArgs e);
    public delegate void AfterLayerDeleteHandler(object sender, AfterLayerDeletedEventArgs e);
    public delegate void InteractiveLayerChangeHandler(object sender, InteractiveLayerChangedEventArgs e);
    
    public delegate void ActorAddHandler(object sender, ActorAddedEventArgs e);
    public delegate void EventAddHandler(object sender, EventAddedEventArgs e);
    public delegate void HudElementAddHandler(object sender, HudElementAddedEventArgs e);
    public delegate void SpawnPointAddHandler(object sender, SpawnPointAddedEventArgs e);
    public delegate void TileObjectAddHandler(object sender, TileObjectAddedEventArgs e);
    public delegate void TileObjectDeleteHandler(object sender, TileObjectDeletedEventArgs e);
    public delegate void TileObjectNameChangeHandler(object sender, TileObjectNameChangedEventArgs e);
    
    public delegate void BeforeMapWidgetDeleteHandler(object sender, BeforeMapWidgetDeleteEventArgs e);
    public delegate void BeforeMapWidgetAddHandler(object sender, BeforeMapWidgetAddedEventArgs e);
    public delegate void MapWidgetAddHandler(object sender, MapWidgetAddedEventArgs e);
    
    public delegate void MapWidgetSelectionChangeHandler(object sender, MapWidgetSelectionChangedEventArgs e);
    #endregion 

    public class ProjectController : IProjectController
    {
        #region Events

        public event ProjectCreateHandler ProjectCreated;
        public event ProjectStateChangeHandler ProjectStateChanged;
        public event RoomAddHandler RoomAdded;
        public event RoomSelectHandler RoomSelected;
        public event BeforeRoomDeletedHandler BeforeRoomDeleted;
        public event CameraModeChangedHandler CameraModeChanged;
        public event EditModeChangedHandler EditModeChanged;
        public event SelectionToggleHandler SelectionToggle;
        public event RefreshViewHandler RefreshView;
        public event RefreshPropertiesHandler RefreshProperties;
        public event TileObjectSelectHandler TileObjectSelected;

        public event LayerSelectHandler LayerSelect;
        public event LayerAddHandler LayerAdd;
        public event LayerResizeHandler LayerResize;
        public event BeforeLayerDeleteHandler BeforeLayerDelete;
        public event AfterLayerDeleteHandler AfterLayerDelete;
        public event InteractiveLayerChangeHandler InteractiveLayerChange;
        
        public event ActorAddHandler ActorAdd;
        public event EventAddHandler EventAdd;
        public event HudElementAddHandler HudElementAdd;
        public event SpawnPointAddHandler SpawnPointAdd;
        public event TileObjectAddHandler TileObjectAdd;
        public event TileObjectDeleteHandler TileObjectDelete;        
        public event TileObjectNameChangeHandler TileObjectNameChange;
        public event BeforeMapWidgetDeleteHandler BeforeMapWidgetDelete;
        public event MapWidgetSelectionChangeHandler MapWidgetSelectionChange;
        public event BeforeMapWidgetAddHandler BeforeMapWidgetAdd;
        public event MapWidgetAddHandler MapWidgetAdd;

        #endregion events

        #region Constructors

        public ProjectController(INameValidator nameValidator, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            nameValidator_ = nameValidator;
            nameGenerator_ = nameGenerator;

            projectDto_ = null;
            
            projectUiState_ = new ProjectUiStateDto();
            
            undoStack_ = new Stack<byte[]>();
            redoStack_ = new Stack<byte[]>();
            
            utilityFactory_ = new UtilityFactory();

            bitmapUtility_ = utilityFactory_.NewBitmapUtility();
            nameUtility_ = utilityFactory_.NewNameUtility();
            projectUtility_ = utilityFactory_.NewProjectUtility();
            uriUtility_ = utilityFactory_.NewUriUtility();

            firemelonEditorFactory_ = new FiremelonEditorFactory();
            resourceBitmapReader_ = firemelonEditorFactory_.NewResourceBitmapReader();

            exceptionHandler_ = exceptionHandler;

            ChangesMade = false;

            mapWidgetFactory_ = new MapWidgetFactory(this);
        }

        #endregion

        #region Private Variables

        private ProjectDto projectDto_;
        private ProjectUiStateDto projectUiState_;
        
        // Removed in 2.2, but needs to be here because it's used by upgraders from old versions.    
        private ProjectResourcesDto projectResources_;
        
        private IMapWidgetFactory mapWidgetFactory_;
        private IUtilityFactory utilityFactory_;
        private IFiremelonEditorFactory firemelonEditorFactory_;

        private Stack<byte[]> undoStack_;
        private Stack<byte[]> redoStack_;
        
        private bool isDataChanged_;

        private IBitmapUtility bitmapUtility_;
        private IExceptionHandler exceptionHandler_;
        private INameGenerator nameGenerator_;
        private INameUtility nameUtility_;
        private INameValidator nameValidator_;
        private IProjectUtility projectUtility_;
        private IUriUtility uriUtility_;
        private IResourceBitmapReader resourceBitmapReader_;
                
        #endregion

        #region Public Functions
        
        #region Project Functions

        public ProjectUiStateDto GetUiState()
        {
            return projectUiState_;
        }

        // Resources removed in 2.2
        //public ProjectResourcesDto GetResources()
        //{
        //    return projectResources_;
        //}
        
        public void CreateNewProject(ProjectDto project, ProjectDto assets)
        {
            if (project == null)
            {
                return;
            }

            if (isValidProject(project) == false)
            {
                throw new InvalidProjectFileException("Project File Contains Invalid Parameters.");
            }
            
            projectDto_ = project;
            
            mapWidgetFactory_.TileSize = projectDto_.TileSize;

            if (assets != null)
            {
                copyAssetsToProject(assets);
            }
            
            projectResources_ = new ProjectResourcesDto();

            //// Copy the resources from the project DTO to the independent resources DTO.
            //foreach (KeyValuePair<Guid, BitmapResourceDto> bitmapResource in projectDto_.Resources.Bitmaps)
            //{
            //    projectResources_.Bitmaps.Add(bitmapResource.Key, bitmapResource.Value);
            //}

            //foreach (KeyValuePair<Guid, AudioResourceDto> audioResource in projectDto_.Resources.AudioData)
            //{
            //    projectResources_.AudioData.Add(audioResource.Key, audioResource.Value);
            //}

            projectUiState_ = generateUiStateFromProject(project);

            for (int i = 0; i < project.Rooms.Count; i++)
            {
                RoomDto currentRoom = project.Rooms[i];

                // Ensure there is a layer list and tileset list for this room.
                if (projectDto_.Layers.ContainsKey(currentRoom.Id) == false)
                {
                    projectDto_.Layers.Add(currentRoom.Id, new List<LayerDto>());
                }

                if (projectDto_.InteractiveLayerIndexes.ContainsKey(currentRoom.Id) == false)
                {
                    projectDto_.InteractiveLayerIndexes.Add(currentRoom.Id, 0);
                }
            }
            
            undoStack_.Clear();

            redoStack_.Clear();

            initializeControllers(projectDto_);
        }

        public void FinalizeProject()
        {
            projectDto_.IsPrepared = true;

            OnProjectCreated(new ProjectCreatedEventArgs(true));

            // Select the initial room.
            SelectRoom(0);
        }

        public bool ChangeProjectName(string name)
        {
            if (projectDto_.ProjectName.ToUpper() != name.ToUpper())
            {
                saveProjectState();

                projectDto_.ProjectName = name;

                // Change the script names.
                projectDto_.Scripts[Globals.UiManagerId].Name = name + "Ui";
                projectDto_.Scripts[Globals.NetworkHandlerId].Name = name + "NetworkHandler";

                ChangesMade = true;

                return true;
            }

            return false;
        }
        
        public ProjectDto GetProjectDto()
        {
            return projectDto_;
        }

        public void WriteProjectDtoToStream(Stream stream, bool fullSave)
        {
            // This is a wrapper around the stream writer, and is called by the UI. Create a writer for the most recent version
            // of the project file, and pass in the UI state and resources.

            IProjectStreamWriter projectStreamWriter = firemelonEditorFactory_.NewProjectStreamWriter(ProjectDto.LatestProjectVersion, projectUiState_, projectResources_, uriUtility_, fullSave);

            projectStreamWriter.WriteProjectToStream(projectDto_, stream);
        }
        
        public Version ReadProjectVersionNumberFromStream(Stream stream)
        {
            ProjectDto newProject = new ProjectDto();

            BinaryReader br = new BinaryReader(stream);

            Version projectVersion = new Version(0, 0, 0, 0);

            try
            {
                stream.Seek(0, SeekOrigin.Begin);

                // File type.
                string fileType = br.ReadString();

                if (fileType != "FMPROJ")
                {
                    throw new InvalidProjectFileException("Project file header is invalid.");
                }

                int major = br.ReadInt32();
                int minor = br.ReadInt32();
                int revision = br.ReadInt32();
                
                return new Version(major, minor, 0, revision);
            }
            catch (IOException ex)
            {
                exceptionHandler_.HandleException(ex);
            }
            finally
            {
            }

            return projectVersion;
        }
        
        public ProjectDto ReadProjectDtoFromStream(Stream stream)
        {
            // The stream passed in will be the latest version. Read it into the latest version of the ProjectDto object.

            ProjectDto newProject = new ProjectDto();

            try
            {
                // TO DO: Add a dictinary to cache the readers so I don't need to allocate one every time this is called..


                // Create a reader for this project file version.
                IProjectStreamReader projectStreamReader = firemelonEditorFactory_.NewProjectStreamReader(newProject.FileVersion, mapWidgetFactory_, this, bitmapUtility_, projectUtility_, nameUtility_, uriUtility_);

                newProject = (ProjectDto)projectStreamReader.ReadProjectFromStream(stream);

                // Perform any processing necessary before the project is ready to use. (i.e. populating the widget grid).
            }
            catch (Exception ex)
            {
                exceptionHandler_.HandleException(ex);
            }

            return newProject;
        }

        #endregion

        #region Room Functions

        public void SelectRoom(int roomIndex)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                projectUiState_.SelectedRoomIndex = roomIndex;

                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                projectUiState_.SelectedRoomId = roomId;
                
                OnRoomSelected(new RoomSelectedEventArgs(roomIndex));

                refreshViews();
            }
        }

        public void DeleteRoom(int roomIndex)
        {
            OnBeforeRoomDeleted(new BeforeRoomDeletedEventArgs(roomIndex));

            int selectedRoomIndex = projectUiState_.SelectedRoomIndex;

            saveProjectState();

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            projectDto_.Scripts.Remove(roomId);

            // Remove all map widgets that are in this room.           
            foreach (MapWidgetDto mapWidget in projectUiState_.RoomMapWidgets[roomId])
            {
                deleteMapWidget(mapWidget.Id, false);
            }

            projectDto_.Layers.Remove(roomId);

            projectDto_.InteractiveLayerIndexes.Remove(roomId);

            projectDto_.Rooms.RemoveAt(roomIndex);

            projectUiState_.MaxCols.Remove(roomId);
            projectUiState_.MaxRows.Remove(roomId);

            projectUiState_.SelectedLayerIndex.Remove(roomId);
            projectUiState_.SelectedTileIndex.Remove(roomId);
            projectUiState_.LayerOrdinalToIndexMap.Remove(roomId);
            projectUiState_.LayerVisible.Remove(roomId);
            projectUiState_.MapWidgetMode.Remove(roomId);
            projectUiState_.SelectedMapWidgetType.Remove(roomId);
            projectUiState_.CameraLocation.Remove(roomId);
            projectUiState_.CameraLocationMax.Remove(roomId);
            projectUiState_.CanvasOffset.Remove(roomId);
            projectUiState_.CanvasOffsetMax.Remove(roomId);
            projectUiState_.MaxCols.Remove(roomId);
            projectUiState_.MaxRows.Remove(roomId);
            
            projectUiState_.RoomMapWidgetsByType.Remove(roomId);
            projectUiState_.RoomMapWidgets.Remove(roomId);
 
            if (projectUiState_.SelectedRoomIndex >= projectDto_.Rooms.Count)
            {
                SelectRoom(selectedRoomIndex - 1);
            }
            else
            {
                SelectRoom(selectedRoomIndex);
            }

            //OnRoomDeleted()

            ChangesMade = true;
        }

        public RoomDto AddRoom(string name, string layerName, int layerColumns, int layerRows)
        {
            saveProjectState();

            RoomDto newRoom = addRoom(name, layerName, layerColumns, layerRows);

            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.Room;
            script.OwnerId = newRoom.Id;
            script.Name = newRoom.Name.Replace(" ", "");
            projectDto_.Scripts[newRoom.Id] = script;

            OnRoomAdded(new RoomAddedEventArgs(projectDto_.Rooms.Count - 1));

            return newRoom;
        }

        public int GetRoomIndexFromId(Guid roomId)
        {
            return projectUtility_.GetRoomIndexFromId(roomId, projectDto_);
        }

        public RoomDto GetRoom(Guid roomId)
        {
            return projectUtility_.GetRoom(roomId, projectDto_);
        }

        public bool IsRoom(Guid id)
        {
            foreach (RoomDto room in projectDto_.Rooms)
            {
                if (room.Id == id)
                {
                    return true;
                }
            }

            return false;
        }
        
        public void SetRoomName(Guid roomId, string name)
        {
            int roomIndex = GetRoomIndexFromId(roomId);

            nameValidator_.ValidateAssetName(roomId, projectDto_, name);
            
            saveProjectState();

            ChangesMade = true;

            projectDto_.Rooms[roomIndex].Name = name;
            projectDto_.Scripts[roomId].Name = name;
        }

        public void SetRoomLoadingScreen(Guid roomId, Guid loadingScreenId)
        {
            saveProjectState();

            int roomIndex = GetRoomIndexFromId(roomId);

            ChangesMade = true;

            projectDto_.Rooms[roomIndex].LoadingScreenId = loadingScreenId;
        }

        public void SetRoomTransition(Guid roomId, Guid transitionId)
        {
            saveProjectState();

            int roomIndex = GetRoomIndexFromId(roomId);

            ChangesMade = true;

            projectDto_.Rooms[roomIndex].TransitionId = transitionId;
        }

        #endregion

        #region Layer Functions

        public LayerDto AddLayer(int roomIndex, string name, int columns, int rows)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                saveProjectState();

                LayerDto newLayer = addLayer(roomIndex, name, columns, rows);

                OnLayerAdded(new LayerAddedEventArgs(name, columns, rows));

                refreshViews();

                return newLayer;
            }

            return null;
        }

        public void SelectLayer(int roomIndex, int layerIndex)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            if (layerIndex < 0 || layerIndex >= projectDto_.Layers[roomId].Count)
            {
                return;
            }

            projectUiState_.SelectedLayerIndex[roomId] = layerIndex;

            OnLayerSelect(new LayerSelectedEventArgs(projectDto_.Layers[roomId][layerIndex].Id));

            refreshViews();
        }

        public void SetLayerVisibility(int roomIndex, int layerIndex, bool isVisible)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            if (layerIndex < 0 || layerIndex >= projectDto_.Layers[roomId].Count)
            {
                return;
            }

            Guid layerId = projectDto_.Layers[roomId][layerIndex].Id;

            projectUiState_.LayerVisible[layerId] = isVisible;

            refreshViews();
        }

        public void SetLayerName(int roomIndex, int layerIndex, string name)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            if (layerIndex < 0 || layerIndex >= projectDto_.Layers[roomId].Count)
            {
                return;
            }

            if (name != projectDto_.Layers[roomId][layerIndex].Name)
            {
                saveProjectState();

                projectDto_.Layers[roomId][layerIndex].Name = name;

                ChangesMade = true;

                refreshViews();
            }
        }

        public void SetLayerRows(int roomIndex, int layerIndex, int rows)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            if (layerIndex < 0 || layerIndex >= projectDto_.Layers[roomId].Count)
            {
                return;
            }

            if (rows != projectDto_.Layers[roomId][layerIndex].Rows)
            {
                saveProjectState();

                resizeRows(roomId, layerIndex, rows);

                Guid id = projectDto_.Layers[roomId][layerIndex].Id;
                int columns = projectDto_.Layers[roomId][layerIndex].Cols;

                OnLayerResized(new LayerResizedEventArgs(id, columns, rows));

                ChangesMade = true;

                refreshViews();
            }
        }

        public void SetLayerColumns(int roomIndex, int layerIndex, int columns)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            if (layerIndex < 0 || layerIndex >= projectDto_.Layers[roomId].Count)
            {
                return;
            }

            if (columns != projectDto_.Layers[roomId][layerIndex].Cols)
            {
                saveProjectState();

                resizeColumns(roomId, layerIndex, columns);

                Guid id = projectDto_.Layers[roomId][layerIndex].Id;
                int rows = projectDto_.Layers[roomId][layerIndex].Rows;

                OnLayerResized(new LayerResizedEventArgs(id, columns, rows));

                ChangesMade = true;

                refreshViews();
            }
        }

        public void SetLayerNameRowsColumns(int roomIndex, int layerIndex, string name, int rows, int columns)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            if (layerIndex < 0 || layerIndex >= projectDto_.Layers[roomId].Count)
            {
                return;
            }

            if (rows != projectDto_.Layers[roomId][layerIndex].Rows || 
                columns != projectDto_.Layers[roomId][layerIndex].Cols ||
                name != projectDto_.Layers[roomId][layerIndex].Name)
            {
                saveProjectState();

                projectDto_.Layers[roomId][layerIndex].Name = name;
                resizeColumns(roomId, layerIndex, columns);
                resizeRows(roomId, layerIndex, rows);

                Guid id = projectDto_.Layers[roomId][layerIndex].Id;

                OnLayerResized(new LayerResizedEventArgs(id, columns, rows));

                ChangesMade = true;

                refreshViews();
            }
        }

        public void SetInteractiveLayer(int roomIndex, int layerIndex)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            if (layerIndex < 0 || layerIndex >= projectDto_.Layers[roomId].Count)
            {
                return;
            }

            int oldInteractiveLayer = projectDto_.InteractiveLayerIndexes[roomId];
            int newInteractiveLayer = layerIndex;

            if (oldInteractiveLayer != newInteractiveLayer)
            {
                saveProjectState();

                projectDto_.InteractiveLayerIndexes[roomId] = newInteractiveLayer;

                ChangesMade = true;

                OnInteractiveLayerChanged(new InteractiveLayerChangedEventArgs(oldInteractiveLayer, newInteractiveLayer));

                refreshViews();
            }
        }
        
        public void DeleteLayer(int roomIndex, int layerIndex)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            if (layerIndex < 0 || layerIndex >= projectDto_.Layers[roomId].Count)
            {
                return;
            }

            saveProjectState();

            int indexToDelete = layerIndex;

            int layerCount = projectDto_.Layers[roomId].Count;

            int ordinalToDelete = projectUiState_.LayerOrdinalToIndexMap[roomId].IndexOf(layerIndex);

            Guid idToDelete = projectDto_.Layers[roomId][layerIndex].Id;

            OnBeforeLayerDeleted(new BeforeLayerDeletedEventArgs(idToDelete));

            int interactiveIndexBeforeDelete = projectDto_.InteractiveLayerIndexes[roomId];
            int interactiveOrdinalBeforeDelete = projectUiState_.LayerOrdinalToIndexMap[roomId].IndexOf(interactiveIndexBeforeDelete);
            
            int selectedIndexBeforeDelete = projectUiState_.SelectedLayerIndex[roomId];
            int selectedOrdinalBeforeDelete = projectUiState_.LayerOrdinalToIndexMap[roomId].IndexOf(selectedIndexBeforeDelete);

            int interactiveIndexAfterDelete = interactiveIndexBeforeDelete;
            int interactiveOrdinalAfterDelete = interactiveOrdinalBeforeDelete;

            int selectedIndexAfterDelete = selectedIndexBeforeDelete;
            int selectedOrdinalAfterDelete = selectedOrdinalBeforeDelete;

            if (selectedIndexBeforeDelete == indexToDelete)
            {
                // If the selected layer is being deleted, a new layer will need 
                // to be selected in its place. If there is a layer in the ordinal position
                // directly above it, this should be the new selected layer, so it appears 
                // to "fall down" into the selected slot.
                // If there is not, then the layer in the ordinal position directly below
                // should become the selected layer.

                if (selectedOrdinalBeforeDelete == layerCount - 1)
                {
                    // There are no layers in the ordinal position directly above.
                    // The selected index should then become the index of the layer in the ordinal position below.
                    selectedIndexAfterDelete = projectUiState_.LayerOrdinalToIndexMap[roomId][selectedOrdinalBeforeDelete - 1];

                    // If selectedIndexAfterDelete is greater than the index that is going to get deleted,
                    // adjust it here.
                    if (selectedIndexAfterDelete > indexToDelete)
                    {
                        selectedIndexAfterDelete--;
                    }
                }
                else
                {
                    // There is a layer in the ordinal position directly above. Get its index.
                    selectedIndexAfterDelete = projectUiState_.LayerOrdinalToIndexMap[roomId][selectedOrdinalBeforeDelete + 1];

                    // If selectedIndexAfterDelete is greater than the index that is going to get deleted,
                    // adjust it here.
                    if (selectedIndexAfterDelete > indexToDelete)
                    {
                        selectedIndexAfterDelete--;
                    }
                }
            }
            else
            {
                // Otherwise, a non-selected layer is being deleted. If the layer being 
                // deleted's index is less than the selected index, the selected index
                // will need to be decremented to reflect this change.
                if (indexToDelete < selectedIndexBeforeDelete)
                {
                    selectedIndexAfterDelete--;
                }
            }

            // If the selected index is going to be greater than the list size after the delete takes place,
            // adjust it here.
            if (selectedIndexAfterDelete > layerCount - 2)
            {
                selectedIndexAfterDelete--;
            }

            bool interactiveIndexChanged = false;

            if (interactiveIndexBeforeDelete == indexToDelete)
            {
                // If the interactive layer is being deleted, a new interactive layer will need 
                // to be set in its place. If there is a layer in the ordinal position
                // directly above it, this should be the new interactive layer, so it appears 
                // to "fall down" into the selected slot.
                // If there is not, then the layer in the ordinal position directly below
                // should become the interactive layer.
                if (interactiveOrdinalBeforeDelete == layerCount - 1)
                {
                    // There are no layers in the ordinal position directly above.
                    // The interactive index should then become the index of the layer in the ordinal position below.
                    interactiveIndexAfterDelete = projectUiState_.LayerOrdinalToIndexMap[roomId][interactiveOrdinalBeforeDelete - 1];

                    // If selectedIndexAfterDelete is greater than the index that is going to get deleted,
                    // adjust it here.
                    if (interactiveIndexAfterDelete > indexToDelete)
                    {
                        interactiveIndexAfterDelete--;
                    }
                }
                else
                {
                    // There is a layer in the ordinal position directly above. Get its index.
                    interactiveIndexAfterDelete = projectUiState_.LayerOrdinalToIndexMap[roomId][interactiveOrdinalBeforeDelete + 1];

                    // If selectedIndexAfterDelete is greater than the index that is going to get deleted,
                    // adjust it here.
                    if (interactiveIndexAfterDelete > indexToDelete)
                    {
                        interactiveIndexAfterDelete--;
                    }
                }

                interactiveIndexChanged = true;
            }
            else
            {
                // Otherwise, a non-interactive layer is being deleted. If the layer being 
                // deleted's index is less than the interactive index, the interactive index
                // will need to be decremented to reflect this change.
                if (indexToDelete < interactiveIndexBeforeDelete)
                {
                    interactiveIndexAfterDelete--;
                }
            }

            // If the interactive index is going to be greater than the list size after the delete takes place,
            // adjust it here.
            if (interactiveIndexAfterDelete > layerCount - 2)
            {
                interactiveIndexAfterDelete--;
            }

            projectUiState_.SelectedLayerIndex[roomId] = selectedIndexAfterDelete;
            projectDto_.InteractiveLayerIndexes[roomId] = interactiveIndexAfterDelete;
            
            // Delete any map widgets on this layer.
            foreach (MapWidgetDto mapWidget in projectDto_.MapWidgetsByLayer[idToDelete].Values)
            {
                deleteMapWidget(mapWidget.Id, false, false);
            }

            // Remove the list of widgets on this layer
            projectDto_.MapWidgetsByLayer.Remove(idToDelete);
            
            // Delete the layer.
            projectDto_.Layers[roomId].RemoveAt(indexToDelete);
            projectUiState_.LayerOrdinalToIndexMap[roomId].RemoveAt(ordinalToDelete);

            // The ordinal to index values are now broken, because the indicies above the deleted 
            // layer index in the layer array have changed. Fix it.
            for (int i = 0; i < projectUiState_.LayerOrdinalToIndexMap[roomId].Count; i++)
            {
                if (projectUiState_.LayerOrdinalToIndexMap[roomId][i] > indexToDelete)
                {
                    projectUiState_.LayerOrdinalToIndexMap[roomId][i]--;
                }
            }

            int tempRows = 0;
            int tempCols = 0;

            for (int i = 0; i < layerCount - 1; i++)
            {
                LayerDto currentLayer = projectDto_.Layers[roomId][i];

                if (currentLayer.Rows > tempRows)
                {
                    tempRows = currentLayer.Rows;
                }

                if (currentLayer.Cols > tempCols)
                {
                    tempCols = currentLayer.Cols;
                }
            }

            projectUiState_.MaxCols[roomId] = tempRows;
            projectUiState_.MaxCols[roomId] = tempCols;
                        
            ChangesMade = true;

            if (interactiveIndexChanged == true)
            {
                OnInteractiveLayerChanged(new InteractiveLayerChangedEventArgs(indexToDelete, interactiveIndexAfterDelete));
            }

            OnAfterLayerDeleted(new AfterLayerDeletedEventArgs(idToDelete));

            refreshViews();
        }

        public void MoveLayer(int roomIndex, int fromOrdinal, int toOrdinal)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            // I think don't save the project state, because this is only a UI state change.
            //saveProjectState();

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            int fromLayerIndex = projectUiState_.LayerOrdinalToIndexMap[roomId][fromOrdinal];

            if (toOrdinal < fromOrdinal)
            {
                // Move everything from the from-layer ordinal position to the to-layer ordinal position down one.
                // Put layer being reordered into the to-layer position.
                for (int i = fromOrdinal; i > toOrdinal; i--)
                {
                    projectUiState_.LayerOrdinalToIndexMap[roomId][i] = projectUiState_.LayerOrdinalToIndexMap[roomId][i - 1];
                }
            }
            else if (toOrdinal > fromOrdinal)
            {
                // Move everything from the swap layer position to the drop layer position up one.
                // Put layer being reordered into the drop layer position.
                for (int i = fromOrdinal; i < toOrdinal; i++)
                {
                    projectUiState_.LayerOrdinalToIndexMap[roomId][i] = projectUiState_.LayerOrdinalToIndexMap[roomId][i + 1];
                }
            }

            projectUiState_.LayerOrdinalToIndexMap[roomId][toOrdinal] = fromLayerIndex;

            projectUiState_.SelectedLayerIndex[roomId] = projectUiState_.LayerOrdinalToIndexMap[roomId][toOrdinal];

            ChangesMade = true;

            refreshViews();
        }

        public int GetLayerCount(int roomIndex)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                return projectDto_.Layers[roomId].Count;
            }

            return 0;
        }

        public int GetLayerOrdinalFromIndex(int roomIndex, int layerIndex)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                if (layerIndex >= 0 && layerIndex < projectDto_.Layers[roomId].Count)
                {
                    return projectUiState_.LayerOrdinalToIndexMap[roomId].IndexOf(layerIndex);
                }
            }

            return -1;
        }

        public int GetLayerIndexFromOrdinal(int roomIndex, int layerOrdinal)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                if (layerOrdinal >= 0 && layerOrdinal < projectUiState_.LayerOrdinalToIndexMap[roomId].Count)
                {
                    return projectUiState_.LayerOrdinalToIndexMap[roomId][layerOrdinal];
                }
            }

            return -1;
        }

        public int GetLayerIndexFromId(int roomIndex, Guid layerId)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                for (int i = 0; i < projectDto_.Layers[roomId].Count; i++)
                {
                    if (layerId == projectDto_.Layers[roomId][i].Id)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public int GetInteractiveLayerIndex(int roomIndex)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                return projectDto_.InteractiveLayerIndexes[roomId];
            }

            return -1;
        }
        
        public LayerDto GetLayerByIndex(int roomIndex, int layerIndex)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                return projectDto_.Layers[roomId][layerIndex];
            }

            return null;
        }

        public LayerDto GetLayerById(Guid layerId)
        {
            return projectUtility_.GetLayerById(layerId, projectDto_);
        }

        public LayerDto GetLayerByOrdinal(int roomIndex, int layerOrdinal)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                int layerIndex = projectUiState_.LayerOrdinalToIndexMap[roomId][layerOrdinal];

                return projectDto_.Layers[roomId][layerIndex];
            }

            return null;
        }

        public LayerDto GetInteractiveLayer(int roomIndex)
        {
            int interactiveLayerIndex = GetInteractiveLayerIndex(roomIndex);

            LayerDto interactiveLayer = GetLayerByIndex(roomIndex, interactiveLayerIndex);

            return interactiveLayer;
        }

        #endregion
        
        #region Tile Object Functions

        public TileObjectDto AddTileObject(Guid tileSheetId, TileObjectDto tileObject)
        {
            saveProjectState();

            projectDto_.TileObjects[tileSheetId].Add(tileObject);

            projectUiState_.TileObjectCursorCell.Add(tileObject.Id, new Point2D(0, 0));

            OnTileObjectAdd(new TileObjectAddedEventArgs(tileSheetId, tileObject.Id));

            ChangesMade = true;

            return tileObject;
        }
        

        public void SetTileObjectAnimationId(Guid tileObjectId, Guid animationId)
        {
            saveProjectState();

            TileObjectDto tileObject = GetTileObject(tileObjectId);
            
            tileObject.AnimationId = animationId;
            
            ChangesMade = true;
        }

        public void SetTileObjectName(Guid tileObjectId, string name)
        {
            saveProjectState();

            TileObjectDto tileObject = GetTileObject(tileObjectId);

            string oldName = tileObject.Name;

            tileObject.Name = name;

            OnTileObjectNameChanged(new TileObjectNameChangedEventArgs(tileObject.Id, name, oldName));

            ChangesMade = true;
        }

        public void SetTileObjectColumns(Guid tileObjectId, int columns)
        {
            saveProjectState();

            TileObjectDto tileObject = GetTileObject(tileObjectId);
            
            tileObject.Columns = columns;
            
            ChangesMade = true;
        }

        public void SetTileObjectRows(Guid tileObjectId, int rows)
        {
            saveProjectState();

            TileObjectDto tileObject = GetTileObject(tileObjectId);

            tileObject.Rows = rows;

            ChangesMade = true;
        }

        public void SetTileObjectTopLeftCornerColumn(Guid tileObjectId, int column)
        {
            saveProjectState();

            TileObjectDto tileObject = GetTileObject(tileObjectId);

            tileObject.TopLeftCornerColumn = column;

            ChangesMade = true;
        }

        public void SetTileObjectTopLeftCornerRow(Guid tileObjectId, int row)
        {
            saveProjectState();

            TileObjectDto tileObject = GetTileObject(tileObjectId);

            tileObject.TopLeftCornerRow = row;

            ChangesMade = true;
        }

        public void DeleteTileObject(int tileSheetIndex, int tileObjectIndex)
        {            
            if (tileSheetIndex >= 0 && tileSheetIndex < projectDto_.TileSheets.Count)
            {
                Guid tileSheetId = projectDto_.TileSheets[tileSheetIndex].Id;

                int tileObjectCount = projectDto_.TileObjects[tileSheetId].Count;

                if (tileObjectIndex >= 0 && tileObjectIndex < tileObjectCount)
                {
                    Guid tileObjectId = projectDto_.TileObjects[tileSheetId][tileObjectIndex].Id;

                    saveProjectState();

                    ChangesMade = true;
                    
                    if (projectUiState_.SelectedTileSheetIndex == tileSheetIndex)
                    {
                        int selectedTileObjectIndex = projectUiState_.SelectedTileObjectIndex;

                        // If the selected object index is greater than the deleted object index,
                        // it will need to be decremented, so that the same object will remain selected.
                        if (selectedTileObjectIndex > tileObjectIndex)
                        {
                            projectUiState_.SelectedTileObjectIndex -= 1;

                            selectedTileObjectIndex -= 1;
                        }

                        // If the new selected object index is greater than the (new) object count, it
                        // will need to be decremented.
                        if (selectedTileObjectIndex >= tileObjectCount - 1)
                        {
                            projectUiState_.SelectedTileObjectIndex = tileObjectCount - 2;
                        }
                    }

                    // Delete any map widgets of this type.
                    List<Guid> lstMapWidgetIds = new List<Guid>(projectDto_.MapWidgets[MapWidgetType.TileObject].Keys);

                    foreach (Guid mapWidgetId in lstMapWidgetIds)
                    {
                        TileObjectWidgetDto tileObjectWidget = (TileObjectWidgetDto)projectDto_.MapWidgets[MapWidgetType.TileObject][mapWidgetId];

                        if (tileObjectWidget.TileObjectId == tileObjectId)
                        {
                            deleteMapWidget(tileObjectWidget.Id, true);
                        }
                    }

                    projectDto_.TileObjects[tileSheetId].RemoveAt(tileObjectIndex);

                    OnTileObjectDeleted(new TileObjectDeletedEventArgs(tileSheetIndex, tileObjectIndex));
                }
            }
        }
        
        public void SelectTileObject(Guid tileObjectId)
        {
            if (GetTileObject(tileObjectId) != null)
            {
                projectUiState_.SelectedTileObjectId = tileObjectId;

                TileObjectDto tileObject = GetTileObject(tileObjectId);

                projectUiState_.SelectedTileObjectIndex = GetTileObjectIndexFromId(tileObjectId);

                projectUiState_.SelectedTileSheetId = tileObject.OwnerId;

                projectUiState_.SelectedTileSheetIndex = GetTileSheetIndexFromId(tileObject.OwnerId);

                projectUiState_.SelectedActorId = Guid.Empty;
                projectUiState_.SelectedActorIndex = -1;

                projectUiState_.SelectedEventId = Guid.Empty;
                projectUiState_.SelectedEventIndex = -1;

                projectUiState_.SelectedHudElementId = Guid.Empty;
                projectUiState_.SelectedHudElementIndex = -1;

                projectUiState_.SelectedSpawnPointId = Guid.Empty;
                projectUiState_.SelectedSpawnPointIndex = -1;
                

                //OnTileObjectSelected(new TileObjectSelectedEventArgs(tileSheetIndex, tileObjectIndex));
                OnTileObjectSelected(new TileObjectSelectedEventArgs(tileObjectId));
            }
        }

        public TileObjectDto GetTileObject(int tileSheetIndex, int tileObjectIndex)
        {
            if (tileSheetIndex >= 0 && tileSheetIndex < projectDto_.TileSheets.Count)
            {
                Guid tileSheetId = projectDto_.TileSheets[tileSheetIndex].Id;

                int tileObjectCount = projectDto_.TileObjects[tileSheetId].Count;
                
                if (tileObjectIndex >= 0 && tileObjectIndex < tileObjectCount)
                {
                    return projectDto_.TileObjects[tileSheetId][tileObjectIndex];
                }
            }

            return null;
        }

        public TileObjectDto GetTileObject(Guid tileObjectId)
        {
            return getTileObject(tileObjectId, projectDto_);
        }

        public void SetTileObjectCursorCell(Guid tileObjectId, Point2D cursorCell)
        {
            TileObjectDto tileObject = GetTileObject(tileObjectId);

            if (tileObject == null)
            {
                return;
            }

            // If the cursor cell is within the valid bounds...
            if (cursorCell.X <= 0 && cursorCell.X > tileObject.Columns * -1 && cursorCell.Y <= 0 && cursorCell.Y > tileObject.Rows * -1)
            {
                projectUiState_.TileObjectCursorCell[tileObjectId] = cursorCell;
            }
        }

        #endregion


        #region Scenery Animation Functions

        public SceneryAnimationDto AddSceneryAnimation(Guid tileSheetId, SceneryAnimationDto sceneryAnimation)
        {
            saveProjectState();

            List<string> lstNames = new List<string>(new string[] { sceneryAnimation.Name });

            nameUtility_.AddSceneryAnimationNames(tileSheetId, lstNames);
            
            projectDto_.SceneryAnimations[tileSheetId].Add(sceneryAnimation);
            
            ChangesMade = true;

            return sceneryAnimation;
        }


        public void DeleteSceneryAnimation(int tileSheetIndex, int sceneryAnimationIndex)
        {
            if (tileSheetIndex >= 0 && tileSheetIndex < projectDto_.TileSheets.Count)
            {
                Guid tileSheetId = projectDto_.TileSheets[tileSheetIndex].Id;

                int sceneryAnimationCount = projectDto_.SceneryAnimations[tileSheetId].Count;

                if (sceneryAnimationIndex >= 0 && sceneryAnimationIndex < sceneryAnimationCount)
                {
                    SceneryAnimationDto sceneryAnimation = projectDto_.SceneryAnimations[tileSheetId][sceneryAnimationIndex];

                    Guid sceneryAnimationId = sceneryAnimation.Id;

                    saveProjectState();

                    ChangesMade = true;

                    // Remove the scenery animation name from the list of names.                       
                    nameUtility_.RemoveSceneryAnimationName(tileSheetId, sceneryAnimation.Name);

                    projectDto_.SceneryAnimations[tileSheetId].RemoveAt(sceneryAnimationIndex);                    
                }
            }
        }


        public SceneryAnimationDto GetSceneryAnimation(int tileSheetIndex, int sceneryAnimationIndex)
        {
            if (tileSheetIndex >= 0 && tileSheetIndex < projectDto_.TileSheets.Count)
            {
                Guid tileSheetId = projectDto_.TileSheets[tileSheetIndex].Id;

                int sceneryAnimationCount = projectDto_.SceneryAnimations[tileSheetId].Count;

                if (sceneryAnimationIndex >= 0 && sceneryAnimationIndex < sceneryAnimationCount)
                {
                    return projectDto_.SceneryAnimations[tileSheetId][sceneryAnimationIndex];
                }
            }

            return null;
        }


        public SceneryAnimationDto GetSceneryAnimation(Guid sceneryAnimationId)
        {
            return getSceneryAnimation(sceneryAnimationId, projectDto_);
        }

        public Guid GetSceneryAnimationIdFromName(Guid ownerId, string sceneryAnimationName)
        {
            foreach (SceneryAnimationDto sceneryAnimation in projectDto_.SceneryAnimations[ownerId])
            {
                if (sceneryAnimation.Name.ToUpper() == sceneryAnimationName.ToUpper())
                {
                    return sceneryAnimation.Id;
                }
            }

            return Guid.Empty;
        }

        public List<string> GetSceneryAnimationNames(Guid tileSheetId)
        {
            return nameUtility_.GetSceneryAnimationNames(tileSheetId);
        }

        public void SetSceneryAnimationFramesPerSecond(Guid sceneryAnimationId, int framesPerSecond)
        {
            foreach (KeyValuePair<Guid, List<SceneryAnimationDto>> sceneryAnimationList in projectDto_.SceneryAnimations)
            {
                foreach (SceneryAnimationDto sceneryAnimation in sceneryAnimationList.Value)
                {
                    if (sceneryAnimation.Id == sceneryAnimationId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        sceneryAnimation.FramesPerSecond = framesPerSecond;
                        
                        return;
                    }
                }
            }
        }


        public void SetSceneryAnimationName(Guid sceneryAnimationId, string name)
        {
            saveProjectState();

            SceneryAnimationDto sceneryAnimation = GetSceneryAnimation(sceneryAnimationId);

            string oldName = sceneryAnimation.Name;

            sceneryAnimation.Name = name;

            nameUtility_.RemoveSceneryAnimationName(sceneryAnimation.OwnerId, oldName);
            
            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddSceneryAnimationNames(sceneryAnimation.OwnerId, lstNames);

            ChangesMade = true;
        }


        #endregion
        #region Tile Sheet Functions

        public TileSheetDto AddTileSheet(string imagePath, int tileSize, string name)
        {
            saveProjectState();

            TileSheetDto tileSheet = new TileSheetDto();

            tileSheet.Name = name;
            tileSheet.TileSize = tileSize;

            projectDto_.TileObjects.Add(tileSheet.Id, new List<TileObjectDto>());

            projectDto_.SceneryAnimations.Add(tileSheet.Id, new List<SceneryAnimationDto>());

            setTileSheetImage(tileSheet, imagePath);

            projectDto_.TileSheets.Add(tileSheet);


            //findmelater Do I need to do selected tilesheet index + selected tile object index?
            //projectUiState_.SelectedTileObjectIndex.Add(tileSheet.Id, 0);

            ChangesMade = true;

            return tileSheet;
        }

        public TileSheetDto GetTileSheet(Guid assetId)
        {
            foreach (TileSheetDto tileSheet in projectDto_.TileSheets)
            {
                if (tileSheet.Id == assetId)
                {
                    return tileSheet;
                }
            }

            return null;
        }

        public TileSheetDto GetTileSheetByName(string name)
        {
            foreach (TileSheetDto tileSheet in projectDto_.TileSheets)
            {
                if (tileSheet.Name == name)
                {
                    return tileSheet;
                }
            }

            return null;
        }
        
        public void DeleteTileSheet(int tileSheetIndex)
        {
            if (tileSheetIndex < 0 || tileSheetIndex >= projectDto_.TileSheets.Count)
            {
                return;
            }

            saveProjectState();

            ChangesMade = true;

            Guid tileSheetId = projectDto_.TileSheets[tileSheetIndex].Id;

            projectDto_.TileSheets.RemoveAt(tileSheetIndex);

            // Remove tile objects.
            foreach (TileObjectDto tileObject in projectDto_.TileObjects[tileSheetId])
            {
                projectUiState_.TileObjectCursorCell.Remove(tileObject.Id);
            }

            projectDto_.TileObjects.Remove(tileSheetId);
            
            //findmelater Do I need to do selected tilesheet index + selected tile object index?
            //projectUiState_.SelectedTileObjectIndex.Remove(tileSheetId);

            refreshViews();
        }

        public int GetTileObjectIndexFromId(Guid id)
        {
            TileObjectDto tileObject = GetTileObject(id);

            for (int i = 0; i < projectDto_.TileObjects[tileObject.OwnerId].Count; i++)
            {
                if (projectDto_.TileObjects[tileObject.OwnerId][i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetTileSheetIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.TileSheets.Count; i++)
            {
                if (projectDto_.TileSheets[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }
        
        public void SetTileSheetName(Guid tileSheetId, string name)
        {
            saveProjectState();

            int tileSheetIndex = GetTileSheetIndexFromId(tileSheetId);

            projectDto_.TileSheets[tileSheetIndex].Name = name;

            ChangesMade = true;
        }

        public void SetTileSheetImagePath(Guid tileSheetId, string imagePath)
        {
            if (imagePath != string.Empty)
            {
                saveProjectState();

                int tileSheetIndex = GetTileSheetIndexFromId(tileSheetId);

                TileSheetDto tileSheet = projectDto_.TileSheets[tileSheetIndex];

                ChangesMade = true;

                setTileSheetImage(tileSheet, imagePath);

                generateTileSheetImages(tileSheetIndex);
            }
        }

        public void SetTileSheetScaleFactor(Guid tileSheetId, float scaleFactor)
        {
            saveProjectState();

            int tileSheetIndex = GetTileSheetIndexFromId(tileSheetId);

            ChangesMade = true;

            if (scaleFactor < 0)
            {
                scaleFactor = 0;
            }

            projectDto_.TileSheets[tileSheetIndex].ScaleFactor = scaleFactor;
            
            generateTileSheetImages(tileSheetIndex);
        }

        #endregion
        
        #region Sprite Sheet Functions

        public SpriteSheetDto AddSpriteSheet(string imagePath, string name)
        {
            saveProjectState();

            SpriteSheetDto spriteSheet = new SpriteSheetDto();

            setSpriteSheetImage(spriteSheet, imagePath);

            spriteSheet.Name = name;

            projectDto_.SpriteSheets.Add(spriteSheet);

            List<string> lstNames = new List<string>();

            lstNames.Add(name);

            nameUtility_.AddSpriteSheetNames(lstNames);
            
            ChangesMade = true;

            return spriteSheet;
        }

        public void DeleteSpriteSheet(int spriteSheetIndex)
        {
            if (spriteSheetIndex < 0 || spriteSheetIndex >= projectDto_.SpriteSheets.Count)
            {
                return;
            }

            saveProjectState();

            Guid spriteSheetId = projectDto_.SpriteSheets[spriteSheetIndex].Id;

            // Find any animations that are pointing to this sprite sheet and clear it.           
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                foreach (AnimationDto animation in projectDto_.Animations[animationGroup.Id])
                {
                    if (animation.SpriteSheet == spriteSheetId)
                    {
                        animation.SpriteSheet = Guid.Empty;
                    }
                }
            }

            ChangesMade = true;

            nameUtility_.RemoveSpriteSheetName(projectDto_.SpriteSheets[spriteSheetIndex].Name);

            projectDto_.SpriteSheets.RemoveAt(spriteSheetIndex);
            
            refreshViews();
        }

        public SpriteSheetDto GetSpriteSheet(Guid assetId)
        {
            foreach (SpriteSheetDto spriteSheet in projectDto_.SpriteSheets)
            {
                if (spriteSheet.Id == assetId)
                {
                    return spriteSheet;
                }
            }

            return null;
        }

        public SpriteSheetDto GetSpriteSheetByName(string name)
        {
            foreach (SpriteSheetDto spriteSheet in projectDto_.SpriteSheets)
            {
                if (spriteSheet.Name == name)
                {
                    return spriteSheet;
                }
            }

            return null;
        }

        public int GetSpriteSheetIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.SpriteSheets.Count; i++)
            {
                if (projectDto_.SpriteSheets[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetSpriteSheetIdFromName(string name)
        {
            foreach (SpriteSheetDto spriteSheet in projectDto_.SpriteSheets)
            {
                if (spriteSheet.Name.ToUpper() == name.ToUpper())
                {
                    return spriteSheet.Id;
                }
            }

            return Guid.Empty;
        }

        public string GetSpriteSheetNameFromId(Guid id)
        {
            foreach (SpriteSheetDto spriteSheet in projectDto_.SpriteSheets)
            {
                if (spriteSheet.Id == id)
                {
                    return spriteSheet.Name;
                }
            }

            return string.Empty;
        }

        public void SetSpriteSheetName(Guid spriteSheetId, string name)
        {
            saveProjectState();

            int spriteSheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

            nameUtility_.UpdateSpriteSheetName(projectDto_.SpriteSheets[spriteSheetIndex].Name, name);
            
            projectDto_.SpriteSheets[spriteSheetIndex].Name = name;

            ChangesMade = true;
        }
        
        public void SetSpriteSheetImagePath(Guid spriteSheetId, string imagePath)
        {
            if (imagePath != string.Empty)
            {
                saveProjectState();

                int spriteSheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

                SpriteSheetDto spriteSheet = projectDto_.SpriteSheets[spriteSheetIndex];

                ChangesMade = true;

                setSpriteSheetImage(spriteSheet, imagePath);

                generateSpriteSheetImages(spriteSheetIndex);
            }
        }

        public void SetSpriteSheetRows(Guid spriteSheetId, int rows)
        {
            saveProjectState();

            int spriteSheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

            ChangesMade = true;

            projectDto_.SpriteSheets[spriteSheetIndex].Rows = rows;

            generateSpriteSheetImages(spriteSheetIndex);
        }

        public void SetSpriteSheetColumns(Guid spriteSheetId, int columns)
        {
            saveProjectState();

            int spriteSheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

            ChangesMade = true;

            projectDto_.SpriteSheets[spriteSheetIndex].Columns = columns;

            generateSpriteSheetImages(spriteSheetIndex);
        }

        public void SetSpriteSheetCellWidth(Guid spriteSheetId, int cellWidth)
        {
            saveProjectState();

            int spriteSheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

            ChangesMade = true;

            projectDto_.SpriteSheets[spriteSheetIndex].CellWidth = cellWidth;

            generateSpriteSheetImages(spriteSheetIndex);
        }

        public void SetSpriteSheetCellHeight(Guid spriteSheetId, int cellHeight)
        {
            saveProjectState();

            int spriteSheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

            ChangesMade = true;

            projectDto_.SpriteSheets[spriteSheetIndex].CellHeight = cellHeight;

            generateSpriteSheetImages(spriteSheetIndex);
        }

        public void SetSpriteSheetPadding(Guid spriteSheetId, int padding)
        {
            saveProjectState();

            int spriteSheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

            ChangesMade = true;

            projectDto_.SpriteSheets[spriteSheetIndex].Padding = padding;

            generateSpriteSheetImages(spriteSheetIndex);
        }

        public void SetSpriteSheetScaleFactor(Guid spriteSheetId, float scaleFactor)
        {
            saveProjectState();

            int spriteSheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

            ChangesMade = true;

            if (scaleFactor < 0)
            {
                scaleFactor = 0;
            }

            projectDto_.SpriteSheets[spriteSheetIndex].ScaleFactor = scaleFactor;

            generateSpriteSheetImages(spriteSheetIndex);

            refreshViews();
        }

        #endregion

        #region Audio Asset Functions

        public AudioAssetDto AddAudioAsset(string audioPath, string name)
        {
            saveProjectState();
            
            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddAudioAssetNames(lstNames);
            
            AudioAssetDto audioAsset = new AudioAssetDto();

            setAudioFileData(audioAsset, audioPath);

            audioAsset.Name = name;

            projectDto_.AudioAssets.Add(audioAsset);

            ChangesMade = true;

            return audioAsset;
        }

        public void DeleteAudioAsset(int audioAssetIndex)
        {
            if (audioAssetIndex < 0 || audioAssetIndex >= projectDto_.AudioAssets.Count)
            {
                return;
            }

            saveProjectState();

            nameUtility_.RemoveAudioAssetName(projectDto_.AudioAssets[audioAssetIndex].Name);
                        
            ChangesMade = true;

            projectDto_.AudioAssets.RemoveAt(audioAssetIndex);

            refreshViews();
        }

        public AudioAssetDto GetAudioAsset(Guid assetId)
        {
            foreach (AudioAssetDto audioAsset in projectDto_.AudioAssets)
            {
                if (audioAsset.Id == assetId)
                {
                    return audioAsset;
                }
            }

            return null;
        }

        public AudioAssetDto GetAudioAssetByName(string name)
        {
            foreach (AudioAssetDto audioAsset in projectDto_.AudioAssets)
            {
                if (audioAsset.Name == name)
                {
                    return audioAsset;
                }
            }

            return null;
        }

        public int GetAudioAssetIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.AudioAssets.Count; i++)
            {
                if (projectDto_.AudioAssets[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetAudioAssetIdFromName(string name)
        {
            foreach (AudioAssetDto audioAsset in projectDto_.AudioAssets)
            {
                if (audioAsset.Name.ToUpper() == name.ToUpper())
                {
                    return audioAsset.Id;
                }
            }

            return Guid.Empty;
        }

        public string GetAudioAssetNameFromId(Guid id)
        {
            foreach (AudioAssetDto audioAsset in projectDto_.AudioAssets)
            {
                if (audioAsset.Id == id)
                {
                    return audioAsset.Name;
                }
            }

            return string.Empty;
        }

        public void SetAudioAssetName(Guid audioAssetId, string name)
        {
            saveProjectState();

            int audioAssetIndex = GetAudioAssetIndexFromId(audioAssetId);

            nameUtility_.UpdateAudioAssetName(projectDto_.AudioAssets[audioAssetIndex].Name, name);

            ChangesMade = true;

            projectDto_.AudioAssets[audioAssetIndex].Name = name;
        }
        
        public void SetAudioAssetChannel(Guid audioAssetId, string channel)
        {
            saveProjectState();

            int audioAssetIndex = GetAudioAssetIndexFromId(audioAssetId);

            ChangesMade = true;

            projectDto_.AudioAssets[audioAssetIndex].Channel = channel;
        }
        
        public void SetAudioAssetAudioPath(Guid audioAssetId, string audioPath)
        {
            saveProjectState();

            int audioAssetIndex = GetAudioAssetIndexFromId(audioAssetId);

            AudioAssetDto audioAsset = projectDto_.AudioAssets[audioAssetIndex];

            ChangesMade = true;

            setAudioFileData(audioAsset, audioPath);
        }

        #endregion

        #region Loading Screen Functions

        public LoadingScreenDto AddLoadingScreen(string name)
        {
            saveProjectState();

            LoadingScreenDto loadingScreen = new LoadingScreenDto();

            loadingScreen.Name = name;

            projectDto_.LoadingScreens.Add(loadingScreen);

            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddLoadingScreenNames(lstNames);
            
            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.LoadingScreen;
            script.OwnerId = loadingScreen.Id;
            script.Name = loadingScreen.Name;
            projectDto_.Scripts[loadingScreen.Id] = script;

            ChangesMade = true;

            return loadingScreen;
        }

        public LoadingScreenDto GetLoadingScreen(Guid loadingScreenId)
        {
            foreach (LoadingScreenDto loadingScreen in projectDto_.LoadingScreens)
            {
                if (loadingScreen.Id == loadingScreenId)
                {
                    return loadingScreen;
                }
            }

            return null;
        }

        public LoadingScreenDto GetLoadingScreenByName(string name)
        {
            foreach (LoadingScreenDto loadingScreen in projectDto_.LoadingScreens)
            {
                if (loadingScreen.Name == name)
                {
                    return loadingScreen;
                }
            }

            return null;
        }

        public void SetLoadingScreenName(Guid loadingScreenId, string name)
        {
            int loadingScreenIndex = GetLoadingScreenIndexFromId(loadingScreenId);

            nameValidator_.ValidateAssetName(loadingScreenId, projectDto_, name);
            
            saveProjectState();

            ChangesMade = true;

            nameUtility_.UpdateLoadingScreenName(projectDto_.LoadingScreens[loadingScreenIndex].Name, name);
            
            projectDto_.LoadingScreens[loadingScreenIndex].Name = name;

            projectDto_.Scripts[loadingScreenId].Name = name;
        }

        public int GetLoadingScreenIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.LoadingScreens.Count; i++)
            {
                if (projectDto_.LoadingScreens[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetLoadingScreenIdFromName(string name)
        {
            foreach (LoadingScreenDto loadingScreen in projectDto_.LoadingScreens)
            {
                if (loadingScreen.Name.ToUpper() == name.ToUpper())
                {
                    return loadingScreen.Id;
                }
            }

            return Guid.Empty;
        }

        public void DeleteLoadingScreen(int loadingScreenIndex)
        {
            if (loadingScreenIndex < 0 || loadingScreenIndex >= projectDto_.LoadingScreens.Count)
            {
                return;
            }

            saveProjectState();

            nameUtility_.RemoveLoadingScreenName(projectDto_.LoadingScreens[loadingScreenIndex].Name);
            
            projectDto_.Scripts.Remove(projectDto_.LoadingScreens[loadingScreenIndex].Id);
            
            // Clear loading screen from any room that it is set in.
            foreach (RoomDto room in projectDto_.Rooms)
            {
                if (room.LoadingScreenId == projectDto_.LoadingScreens[loadingScreenIndex].Id)
                {
                    room.LoadingScreenId = Guid.Empty;
                }
            }

            projectDto_.LoadingScreens.RemoveAt(loadingScreenIndex);

            ChangesMade = true;
        }

        #endregion

        #region Transition Functions

        public TransitionDto AddTransition(string name)
        {
            saveProjectState();

            TransitionDto transition = new TransitionDto();

            transition.Name = name;

            projectDto_.Transitions.Add(transition);

            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddTransitionNames(lstNames);
            
            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.Transition;
            script.OwnerId = transition.Id;
            script.Name = transition.Name;
            projectDto_.Scripts[transition.Id] = script;

            ChangesMade = true;

            return transition;
        }

        public TransitionDto GetTransition(Guid transitionId)
        {
            foreach (TransitionDto transition in projectDto_.Transitions)
            {
                if (transition.Id == transitionId)
                {
                    return transition;
                }
            }

            return null;
        }

        public TransitionDto GetTransitionByName(string name)
        {
            foreach (TransitionDto transition in projectDto_.Transitions)
            {
                if (transition.Name == name)
                {
                    return transition;
                }
            }

            return null;
        }

        public void SetTransitionName(Guid transitionId, string name)
        {
            int transitionIndex = GetTransitionIndexFromId(transitionId);

            nameValidator_.ValidateAssetName(transitionId, projectDto_, name);
            
            saveProjectState();

            ChangesMade = true;

            nameUtility_.UpdateTransitionName(projectDto_.Transitions[transitionIndex].Name, name);
            
            projectDto_.Transitions[transitionIndex].Name = name;

            projectDto_.Scripts[transitionId].Name = name;
        }

        public int GetTransitionIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.Transitions.Count; i++)
            {
                if (projectDto_.Transitions[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetTransitionIdFromName(string name)
        {
            foreach (TransitionDto transition in projectDto_.Transitions)
            {
                if (transition.Name.ToUpper() == name.ToUpper())
                {
                    return transition.Id;
                }
            }

            return Guid.Empty;
        }

        public void DeleteTransition(int transitionIndex)
        {
            if (transitionIndex < 0 || transitionIndex >= projectDto_.Transitions.Count)
            {
                return;
            }

            saveProjectState();

            nameUtility_.RemoveTransitionName(projectDto_.Transitions[transitionIndex].Name);
            
            projectDto_.Scripts.Remove(projectDto_.Transitions[transitionIndex].Id);

            // Clear transition from any room that it is set in.
            foreach (RoomDto room in projectDto_.Rooms)
            {
                if (room.TransitionId == projectDto_.Transitions[transitionIndex].Id)
                {
                    room.TransitionId = Guid.Empty;
                }
            }

            projectDto_.Transitions.RemoveAt(transitionIndex);

            ChangesMade = true;
        }

        #endregion

        #region Particle Functions

        public ParticleDto AddParticle(string name)
        {
            saveProjectState();

            ParticleDto particle = new ParticleDto();

            particle.Name = name;

            projectDto_.Particles.Add(particle);
            
            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddParticleNames(lstNames);
            
            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.Particle;
            script.OwnerId = particle.Id;
            script.Name = particle.Name;
            projectDto_.Scripts[particle.Id] = script;

            ChangesMade = true;

            return particle;
        }

        public ParticleDto GetParticle(Guid particleId)
        {
            foreach (ParticleDto particle in projectDto_.Particles)
            {
                if (particle.Id == particleId)
                {
                    return particle;
                }
            }

            return null;
        }

        public void SetParticleName(Guid particleId, string name)
        {
            int particleIndex = GetParticleIndexFromId(particleId);

            nameValidator_.ValidateAssetName(particleId, projectDto_, name);
            
            saveProjectState();

            nameUtility_.UpdateParticleName(projectDto_.Particles[particleIndex].Name, name);

            ChangesMade = true;

            projectDto_.Particles[particleIndex].Name = name;

            projectDto_.Scripts[particleId].Name = name;
        }

        public int GetParticleIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.Particles.Count; i++)
            {
                if (projectDto_.Particles[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetParticleIdFromName(string name)
        {
            foreach (ParticleDto particle in projectDto_.Particles)
            {
                if (particle.Name.ToUpper() == name.ToUpper())
                {
                    return particle.Id;
                }
            }

            return Guid.Empty;
        }

        public string GetParticleNameFromId(Guid id)
        {
            foreach (ParticleDto particle in projectDto_.Particles)
            {
                if (particle.Id == id)
                {
                    return particle.Name;
                }
            }

            return string.Empty;
        }

        public void DeleteParticle(int particleIndex)
        {
            if (particleIndex < 0 || particleIndex >= projectDto_.Particles.Count)
            {
                return;
            }

            saveProjectState();

            nameUtility_.RemoveParticleName(projectDto_.Particles[particleIndex].Name);
            
            projectDto_.Scripts.Remove(projectDto_.Particles[particleIndex].Id);

            projectDto_.Particles.RemoveAt(particleIndex);

            ChangesMade = true;
        }

        #endregion

        #region Particle Emitter Functions

        public ParticleEmitterDto AddParticleEmitter(string name)
        {
            saveProjectState();

            ParticleEmitterDto particleEmitter = new ParticleEmitterDto();

            particleEmitter.Name = name;

            projectDto_.ParticleEmitters.Add(particleEmitter);

            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddParticleEmitterNames(lstNames);

            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.ParticleEmitter;
            script.OwnerId = particleEmitter.Id;
            script.Name = particleEmitter.Name;
            projectDto_.Scripts[particleEmitter.Id] = script;

            ChangesMade = true;

            return particleEmitter;
        }

        public ParticleEmitterDto GetParticleEmitter(Guid particleEmitterId)
        {
            foreach (ParticleEmitterDto particleEmitter in projectDto_.ParticleEmitters)
            {
                if (particleEmitter.Id == particleEmitterId)
                {
                    return particleEmitter;
                }
            }

            return null;
        }

        public void SetParticleEmitterName(Guid particleEmitterId, string name)
        {
            int particleEmitterIndex = GetParticleEmitterIndexFromId(particleEmitterId);

            nameValidator_.ValidateAssetName(particleEmitterId, projectDto_, name);
            
            saveProjectState();

            nameUtility_.UpdateParticleEmitterName(projectDto_.ParticleEmitters[particleEmitterIndex].Name, name);
            
            ChangesMade = true;

            projectDto_.ParticleEmitters[particleEmitterIndex].Name = name;

            projectDto_.Scripts[particleEmitterId].Name = name;
        }

        public int GetParticleEmitterIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.ParticleEmitters.Count; i++)
            {
                if (projectDto_.ParticleEmitters[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetParticleEmitterIdFromName(string name)
        {
            foreach (ParticleEmitterDto particleEmitter in projectDto_.ParticleEmitters)
            {
                if (particleEmitter.Name.ToUpper() == name.ToUpper())
                {
                    return particleEmitter.Id;
                }
            }

            return Guid.Empty;
        }

        public string GetParticleEmitterNameFromId(Guid id)
        {
            foreach (ParticleEmitterDto particleEmitter in projectDto_.ParticleEmitters)
            {
                if (particleEmitter.Id == id)
                {
                    return particleEmitter.Name;
                }
            }

            return string.Empty;
        }

        public void DeleteParticleEmitter(int particleEmitterIndex)
        {
            if (particleEmitterIndex < 0 || particleEmitterIndex >= projectDto_.ParticleEmitters.Count)
            {
                return;
            }

            saveProjectState();

            nameUtility_.RemoveParticleEmitterName(projectDto_.ParticleEmitters[particleEmitterIndex].Name);
            
            projectDto_.Scripts.Remove(projectDto_.ParticleEmitters[particleEmitterIndex].Id);

            projectDto_.ParticleEmitters.RemoveAt(particleEmitterIndex);

            ChangesMade = true;
        }

        #endregion

        #region Entity Functions

        public EntityDto GetEntity(Guid entityId)
        {
            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityId)
                {
                    return actor;
                }
            }

            foreach (EventDto eventEntity in projectDto_.Events)
            {
                if (eventEntity.Id == entityId)
                {
                    return eventEntity;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    return hudElement;
                }
            }

            return null;
        }

        public bool IsEntity(Guid id)
        {
            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == id)
                {
                    return true;
                }
            }

            foreach (EventDto eventEntity in projectDto_.Events)
            {
                if (eventEntity.Id == id)
                {
                    return true;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == id)
                {
                    return true;
                }
            }

            return false;
        }
        
        #endregion

        #region Spawn Point Functions

        public SpawnPointDto AddSpawnPoint(string name)
        {
            saveProjectState();

            SpawnPointDto spawnPoint = new SpawnPointDto();

            spawnPoint.Name = name;

            projectDto_.SpawnPoints.Add(spawnPoint);
            
            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddSpawnPointNames(lstNames);
            
            ChangesMade = true;

            OnSpawnPointAdd(new SpawnPointAddedEventArgs(spawnPoint.Id));

            return spawnPoint;
        }

        public void DeleteSpawnPoint(int spawnPointIndex)
        {
            saveProjectState();

            ChangesMade = true;

            Guid spawnPointId = projectDto_.SpawnPoints[spawnPointIndex].Id;

            nameUtility_.RemoveSpawnPointName(projectDto_.SpawnPoints[spawnPointIndex].Name);

            projectDto_.SpawnPoints.RemoveAt(spawnPointIndex);
            
            return;
        }

        public void SelectSpawnPoint(int spawnPointIndex)
        {
            if (spawnPointIndex >= 0 && spawnPointIndex < projectDto_.SpawnPoints.Count)
            {
                projectUiState_.SelectedSpawnPointIndex = spawnPointIndex;

                Guid spawnPointId = projectDto_.SpawnPoints[spawnPointIndex].Id;

                projectUiState_.SelectedSpawnPointId = spawnPointId;

                projectUiState_.SelectedActorId = Guid.Empty;
                projectUiState_.SelectedActorIndex = -1;

                projectUiState_.SelectedEventId = Guid.Empty;
                projectUiState_.SelectedEventIndex = -1;

                projectUiState_.SelectedTileObjectId = Guid.Empty;
                projectUiState_.SelectedTileObjectIndex = -1;

                projectUiState_.SelectedHudElementId = Guid.Empty;
                projectUiState_.SelectedHudElementIndex = -1;

                projectUiState_.SelectedTileSheetId = Guid.Empty;
                projectUiState_.SelectedTileSheetIndex = -1;

                refreshViews();
            }
        }

        public SpawnPointDto GetSpawnPoint(Guid spawnPointId)
        {
            foreach (SpawnPointDto spawnPoint in projectDto_.SpawnPoints)
            {
                if (spawnPoint.Id == spawnPointId)
                {
                    return spawnPoint;
                }
            }

            return null;
        }

        public int GetSpawnPointIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.SpawnPoints.Count; i++)
            {
                if (projectDto_.SpawnPoints[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetSpawnPointIdFromName(string name)
        {
            foreach (SpawnPointDto spawnPoint in projectDto_.SpawnPoints)
            {
                if (spawnPoint.Name.ToUpper() == name.ToUpper())
                {
                    return spawnPoint.Id;
                }
            }

            return Guid.Empty;
        }

        public string GetSpawnPointNameFromId(Guid id)
        {
            foreach (SpawnPointDto spawnPoint in projectDto_.SpawnPoints)
            {
                if (spawnPoint.Id == id)
                {
                    return spawnPoint.Name;
                }
            }

            return string.Empty;
        }

        public void SetSpawnPointName(Guid spawnPointId, string name)
        {
            int spawnPointIndex = GetSpawnPointIndexFromId(spawnPointId);

            nameValidator_.ValidateAssetName(spawnPointId, projectDto_, name);
            
            saveProjectState();

            nameUtility_.UpdateSpawnPointName(projectDto_.SpawnPoints[spawnPointIndex].Name, name);
            
            projectDto_.SpawnPoints[spawnPointIndex].Name = name;

            // Find any spawn point widgets that are using this as their identity, and update their name.
            foreach (KeyValuePair<Guid, MapWidgetDto> kvp in projectDto_.MapWidgets[MapWidgetType.SpawnPoint])
            {
                SpawnPointWidgetDto spawnPointWidget = (SpawnPointWidgetDto)kvp.Value;

                if (spawnPointWidget.Identity == spawnPointId)
                {
                    spawnPointWidget.IdentityName = name;

                    spawnPointWidget.Controller.ResetProperties(projectDto_.MapWidgetProperties[spawnPointWidget.Id]);
                }
            }

            ChangesMade = true;
        }

        #endregion
        
        #region Map Widget Functions

        public MapWidgetDto AddMapWidget(MapWidgetCreationParametersDto creationParams)
        {
            saveProjectState();

            MapWidgetDto mapWidget = addNewMapWidget(creationParams);

            return mapWidget;
        }

        public List<MapWidgetDto> AddMapWidgets(List<MapWidgetCreationParametersDto> creationParamsList)
        {
            List<MapWidgetDto> lstAddedMapWidgets = new List<MapWidgetDto>();

            bool isStateSaved = false;

            foreach (MapWidgetCreationParametersDto creationParams in creationParamsList)
            {
                if (isStateSaved == false)
                {
                    saveProjectState();

                    isStateSaved = true;
                }

                MapWidgetDto newMapWidget = addNewMapWidget(creationParams);

                ChangesMade = true;

                lstAddedMapWidgets.Add(newMapWidget);
            }

            return lstAddedMapWidgets;
        }

        public void DeleteMapWidgets(List<Guid> ids)
        {
            bool isStateSaved = false;

            foreach (Guid id in ids)
            {
                if (isStateSaved == false)
                {
                    saveProjectState();

                    isStateSaved = true;
                }

                deleteMapWidget(id, true);                
            }
        }

        public MapWidgetDto GetMapWidget(Guid mapWidgetId, MapWidgetType type)
        {
            if (projectDto_.MapWidgets[type].ContainsKey(mapWidgetId) == true)
            {
                return projectDto_.MapWidgets[type][mapWidgetId];
            }

            return null;
        }

        public MapWidgetDto GetMapWidget(Guid mapWidgetId)
        {
            // Loop through the dictionary of map widgets of each widget type.
            foreach (KeyValuePair<MapWidgetType, Dictionary<Guid, MapWidgetDto>> kvp in projectDto_.MapWidgets)
            {
                Dictionary<Guid, MapWidgetDto> mapWidgets = kvp.Value;

                // Loop through each map widget of this type.
                foreach (KeyValuePair<Guid, MapWidgetDto> kvpOfType in mapWidgets)
                {
                    MapWidgetDto mapWidget = kvpOfType.Value;

                    if (mapWidget.Id == mapWidgetId)
                    {
                        return mapWidget;
                    }
                }
            }

            return null;
        }

        public void SetMapWidgetBounds(Guid mapWidgetId, Rectangle bounds)
        {
            saveProjectState();

            MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);
            
            projectUtility_.RemoveMapWidgetFromGrid(mapWidget, projectDto_);
            
            ChangesMade = true;

            mapWidget.BoundingBox.Left = bounds.Left;
            mapWidget.BoundingBox.Top = bounds.Top;
            mapWidget.BoundingBox.Width = bounds.Width;
            mapWidget.BoundingBox.Height = bounds.Height;

            projectUtility_.AddMapWidgetToGrid(mapWidget, projectDto_);

            mapWidget.Controller.ResetProperties(projectDto_.MapWidgetProperties[mapWidgetId]);

            refreshProperties();
        }

        public void SetMapWidgetName(Guid mapWidgetId, MapWidgetType mapWidgetType, string name)
        {
            saveProjectState();

            ChangesMade = true;

            MapWidgetDto mapWidget = projectDto_.MapWidgets[mapWidgetType][mapWidgetId];
            
            nameGenerator_.UpdateMapWidgetName(mapWidget, name);

            projectUiState_.RoomMapWidgets[mapWidget.RootOwnerId].Sort();            
        }

        public void SetMapWidgetPosition(Guid mapWidgetId, MapWidgetType mapWidgetType, Point2D position)
        {
            saveProjectState();

            MapWidgetDto mapWidget = GetMapWidget(mapWidgetId, mapWidgetType);

            projectUtility_.RemoveMapWidgetFromGrid(mapWidget, projectDto_);

            ChangesMade = true;

            mapWidget.Controller.UpdatePosition(position);

            projectUtility_.AddMapWidgetToGrid(mapWidget, projectDto_);

            mapWidget.Controller.ResetProperties(projectDto_.MapWidgetProperties[mapWidgetId]);

            refreshProperties();
        }

        public void SetMapWidgetPosition(Guid mapWidgetId, Point2D position)
        {
            saveProjectState();

            MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);

            projectUtility_.RemoveMapWidgetFromGrid(mapWidget, projectDto_);

            ChangesMade = true;

            mapWidget.Controller.UpdatePosition(position);
            
            projectUtility_.AddMapWidgetToGrid(mapWidget, projectDto_);

            mapWidget.Controller.ResetProperties(projectDto_.MapWidgetProperties[mapWidgetId]);

            refreshProperties();
        }

        public void SetSpawnPointWidgetIdentity(Guid mapWidgetId, Guid identity)
        {
            saveProjectState();

            SpawnPointWidgetDto spawnPointWidget = (SpawnPointWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.SpawnPoint);

            spawnPointWidget.Identity = identity;
            spawnPointWidget.IdentityName = GetSpawnPointNameFromId(identity);

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetName(Guid mapWidgetId, string name)
        {
            // Will thrown an invalid name exception if the name is in use.
            nameValidator_.ValidateMapWidgetName(name);

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            if (particleEmitterWidget != null)
            {
                saveProjectState();

                nameGenerator_.UpdateMapWidgetName(particleEmitterWidget, name);
                
                ChangesMade = true;
                
                projectUiState_.RoomMapWidgets[particleEmitterWidget.RootOwnerId].Sort();
            }
        }

        public void SetParticleEmitterWidgetParticleType(Guid mapWidgetId, Guid particleTypeId)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.ParticleType = particleTypeId;
            particleEmitterWidget.ParticleTypeName = GetParticleNameFromId(particleTypeId);

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetBehavior(Guid mapWidgetId, Guid behaviorId)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.Behavior = behaviorId;
            particleEmitterWidget.BehaviorName = GetParticleEmitterNameFromId(behaviorId);

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetAnimation(Guid mapWidgetId, Guid animationId)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.Animation = animationId;
            particleEmitterWidget.AnimationName = GetAnimationNameFromId(animationId);

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetAnimationFramesPerSecond(Guid mapWidgetId, int animationFramesPerSecond)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.AnimationFramesPerSecond = animationFramesPerSecond;

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetAttachParticles(Guid mapWidgetId, bool attachParticles)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.AttachParticles = attachParticles;

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetInterval(Guid mapWidgetId, double interval)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.Interval = interval;

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetParticleLifespan(Guid mapWidgetId, double particleLifespan)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.ParticleLifespan = particleLifespan;

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetActive(Guid mapWidgetId, bool active)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.Active = active;

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetParticlesPerEmission(Guid mapWidgetId, int particlesPerEmission)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.ParticlesPerEmission = particlesPerEmission;

            ChangesMade = true;
        }

        public void SetParticleEmitterWidgetMaxParticles(Guid mapWidgetId, int maxParticles)
        {
            saveProjectState();

            ParticleEmitterWidgetDto particleEmitterWidget = (ParticleEmitterWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.ParticleEmitter);

            particleEmitterWidget.MaxParticles = maxParticles;

            ChangesMade = true;
        }

        public void SetAudioSourceWidgetName(Guid mapWidgetId, string name)
        {
            // Will thrown an invalid name exception if the name is in use.
            nameValidator_.ValidateMapWidgetName(name);

            AudioSourceWidgetDto audioSourceWidget = (AudioSourceWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.AudioSource);

            if (audioSourceWidget != null)
            {
                saveProjectState();

                nameGenerator_.UpdateMapWidgetName(audioSourceWidget, name);
                
                ChangesMade = true;
                
                projectUiState_.RoomMapWidgets[audioSourceWidget.RootOwnerId].Sort();                
            }
        }

        public void SetAudioSourceWidgetAudio(Guid mapWidgetId, Guid audioId)
        {
            AudioSourceWidgetDto audioSourceWidget = (AudioSourceWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.AudioSource);

            if (audioSourceWidget != null)
            {
                saveProjectState();

                ChangesMade = true;

                audioSourceWidget.Audio = audioId;
                audioSourceWidget.AudioName = GetAudioAssetNameFromId(audioId);        
            }
        }

        public void SetAudioSourceWidgetAutoplay(Guid mapWidgetId, bool autoplay)
        {
            AudioSourceWidgetDto audioSourceWidget = (AudioSourceWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.AudioSource);

            if (audioSourceWidget != null)
            {
                saveProjectState();

                ChangesMade = true;

                audioSourceWidget.Autoplay = autoplay;
            }
        }

        public void SetAudioSourceWidgetLoop(Guid mapWidgetId, bool loop)
        {
            AudioSourceWidgetDto audioSourceWidget = (AudioSourceWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.AudioSource);

            if (audioSourceWidget != null)
            {
                saveProjectState();

                ChangesMade = true;

                audioSourceWidget.Loop = loop;
            }
        }

        public void SetAudioSourceWidgetMinDistance(Guid mapWidgetId, int minDistance)
        {
            AudioSourceWidgetDto audioSourceWidget = (AudioSourceWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.AudioSource);

            if (audioSourceWidget != null)
            {
                saveProjectState();

                ChangesMade = true;

                audioSourceWidget.MinDistance = minDistance;
            }
        }

        public void SetAudioSourceWidgetMaxDistance(Guid mapWidgetId, int maxDistance)
        {
            AudioSourceWidgetDto audioSourceWidget = (AudioSourceWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.AudioSource);

            if (audioSourceWidget != null)
            {
                saveProjectState();

                ChangesMade = true;

                audioSourceWidget.MaxDistance = maxDistance;
            }
        }

        public void SetAudioSourceWidgetVolume(Guid mapWidgetId, float volume)
        {
            AudioSourceWidgetDto audioSourceWidget = (AudioSourceWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.AudioSource);

            if (audioSourceWidget != null)
            {
                saveProjectState();

                ChangesMade = true;

                audioSourceWidget.Volume = volume;
            }
        }

        public void SetWorldGeometryWidgetCorners(Guid mapWidgetId, Point2D corner1, Point2D corner2)
        {
            WorldGeometryWidgetDto worldGeometryWidget = (WorldGeometryWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.WorldGeometry);

            if (worldGeometryWidget != null)
            {
                saveProjectState();

                ChangesMade = true;

                worldGeometryWidget.Corner1.X = corner1.X;
                worldGeometryWidget.Corner1.Y = corner1.Y;

                worldGeometryWidget.Corner2.X = corner2.X;
                worldGeometryWidget.Corner2.Y = corner2.Y;
                

                // Set the default slope rise if this is a slope.
                switch (worldGeometryWidget.CollisionStyle)
                {
                    case WorldGeometryCollisionStyle.Incline:
                    case WorldGeometryCollisionStyle.InvertedDecline:

                        if (worldGeometryWidget.SlopeRise > (Math.Abs(corner1.Y - corner2.Y) + 1))
                        {
                            worldGeometryWidget.SlopeRise = Math.Abs(worldGeometryWidget.Corner1.Y - worldGeometryWidget.Corner2.Y) + 1;
                        }
                        
                        break;

                    case WorldGeometryCollisionStyle.Decline:
                    case WorldGeometryCollisionStyle.InvertedIncline:

                        if (worldGeometryWidget.SlopeRise < -(Math.Abs(corner1.Y - corner2.Y) + 1))
                        {
                            worldGeometryWidget.SlopeRise = -(Math.Abs(worldGeometryWidget.Corner1.Y - worldGeometryWidget.Corner2.Y) + 1);
                        }
                        
                        break;
                }
                
                projectUtility_.RemoveMapWidgetFromGrid(worldGeometryWidget, projectDto_);

                projectUtility_.AddMapWidgetToGrid(worldGeometryWidget, projectDto_);

                worldGeometryWidget.Controller.ResetProperties(projectDto_.MapWidgetProperties[mapWidgetId]);

                refreshViews();
            }
        }

        public void SetWorldGeometryWidgetCollisionStyle(Guid mapWidgetId, WorldGeometryCollisionStyle collisionStyle)
        {
            WorldGeometryWidgetDto worldGeometryWidget = (WorldGeometryWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.WorldGeometry);

            if (worldGeometryWidget != null)
            {
                saveProjectState();

                ChangesMade = true;

                worldGeometryWidget.CollisionStyle = collisionStyle;

                // Set the default slope rise if this is a slope.
                switch (collisionStyle)
                {
                    case WorldGeometryCollisionStyle.Incline:
                    case WorldGeometryCollisionStyle.InvertedDecline:

                        worldGeometryWidget.SlopeRise = Math.Abs(worldGeometryWidget.Corner1.Y - worldGeometryWidget.Corner2.Y) + 1;

                        break;

                    case WorldGeometryCollisionStyle.Decline:
                    case WorldGeometryCollisionStyle.InvertedIncline:

                        worldGeometryWidget.SlopeRise = -(Math.Abs(worldGeometryWidget.Corner1.Y - worldGeometryWidget.Corner2.Y) + 1);

                        break;
                }

                worldGeometryWidget.Controller.ResetProperties(projectDto_.MapWidgetProperties[mapWidgetId]);

                refreshViews();
            }
        }

        public void SetWorldGeometryWidgetSlopeRise(Guid mapWidgetId, int slopeRise)
        {
            WorldGeometryWidgetDto worldGeometryWidget = (WorldGeometryWidgetDto)GetMapWidget(mapWidgetId, MapWidgetType.WorldGeometry);

            if (worldGeometryWidget != null)
            {
                int tileHeight = Math.Abs(worldGeometryWidget.Corner2.Y - worldGeometryWidget.Corner1.Y) + 1;

                switch (worldGeometryWidget.CollisionStyle)
                {
                    case WorldGeometryCollisionStyle.Incline:
                    case WorldGeometryCollisionStyle.InvertedDecline:

                        if (slopeRise > tileHeight || slopeRise < 1)
                        {
                            // Rise extends beyond the world geometry boundary.
                            throw new InvalidSlopeRiseException(1, tileHeight);
                        }

                        break;

                    case WorldGeometryCollisionStyle.Decline:
                    case WorldGeometryCollisionStyle.InvertedIncline:

                        if (slopeRise < -tileHeight || slopeRise > -1)
                        {
                            // Rise extends beyond the world geometry boundary.
                            throw new InvalidSlopeRiseException(-tileHeight, -1);
                        }

                        break;
                }

                saveProjectState();

                ChangesMade = true;

                worldGeometryWidget.SlopeRise = slopeRise;

                refreshViews();
            }
        }
        
        public int GetMapWidgetPropertyIndexFromId(Guid mapWidgetId, Guid propertyId)
        {
            for (int i = 0; i < projectDto_.MapWidgetProperties[mapWidgetId].Count; i++)
            {
                if (projectDto_.MapWidgetProperties[mapWidgetId][i].Id == propertyId)
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetMapWidgetPropertyValue(Guid mapWidgetId, Guid propertyId, Object value)
        {
            MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);

            if (mapWidget != null)
            {
                int propertyIndex = GetMapWidgetPropertyIndexFromId(mapWidgetId, propertyId);

                if (propertyIndex > -1)
                {
                    PropertyDto property = projectDto_.MapWidgetProperties[mapWidgetId][propertyIndex];

                    try
                    {
                        bool cancel = false;

                        mapWidget.Controller.PropertyValueChanged(property.Name, ref value, ref cancel);

                        if (cancel == false)
                        {
                            // All map widgets have a name property. It needs to be validated.
                            if (property.Name == "Name")
                            {
                                try
                                {
                                    nameValidator_.ValidateMapWidgetName(value.ToString());
                                }
                                catch (InvalidNameException ex)                                
                                {
                                    exceptionHandler_.HandleException(ex, "Invalid Name");

                                    return;
                                }
                            }

                            int x = mapWidget.Position.X;
                            int y = mapWidget.Position.Y;
                            int left = mapWidget.BoundingBox.Left;
                            int top = mapWidget.BoundingBox.Top;
                            int width = mapWidget.BoundingBox.Width;
                            int height = mapWidget.BoundingBox.Height;

                            switch (property.Name.ToUpper())
                            {
                                case "NAME":

                                    SetMapWidgetName(mapWidgetId, mapWidget.Type, value.ToString());

                                    break;

                                case "POSITIONX":

                                    x = (int)value;

                                    SetMapWidgetPosition(mapWidgetId, new Point2D(x, y));

                                    break;

                                case "POSITIONY":

                                    y = (int)value;

                                    SetMapWidgetPosition(mapWidgetId, new Point2D(x, y));

                                    break;

                                case "BOXWIDTH":

                                    width = (int)value;

                                    SetMapWidgetBounds(mapWidgetId, new Rectangle(left, top, width, height));

                                    break;

                                case "BOXHEIGHT":

                                    height = (int)value;

                                    SetMapWidgetBounds(mapWidgetId, new Rectangle(left, top, width, height));

                                    break;

                                case "ATTACHTOCAMERA":

                                    ChangesMade = true;

                                    saveProjectState();

                                    ((EntityWidgetDto)mapWidget).AttachToCamera = (bool)value;

                                    break;

                                case "ACCEPTINPUT":

                                    ChangesMade = true;

                                    saveProjectState();

                                    ((EntityWidgetDto)mapWidget).AcceptInput = (bool)value;

                                    break;

                                case "RENDERORDER":

                                    ChangesMade = true;

                                    saveProjectState();

                                    ((EntityWidgetDto)mapWidget).RenderOrder = (int)value;

                                    break;
                            }

                            property.Value = value;                    
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionHandler_.HandleException(ex);
                    }

                    refreshViews();
                }
            }
        }
        
        #endregion

        #region Animation Functions

        public AnimationDto AddAnimation(Guid groupId, string name)
        {
            saveProjectState();

            AnimationDto newAnimation = new AnimationDto();
            newAnimation.Name = name;

            if (projectDto_.Animations.ContainsKey(groupId) == false)
            {
                projectDto_.Animations.Add(groupId, new List<AnimationDto>());
            }

            newAnimation.GroupId = groupId;

            projectDto_.Animations[groupId].Add(newAnimation);

            projectDto_.Frames[newAnimation.Id] = new List<FrameDto>();

            List<string> lstNames = new List<string>();

            lstNames.Add(name);

            nameUtility_.AddAnimationNames(lstNames);
            
            ChangesMade = true;

            return newAnimation;
        }

        public AnimationDto GetAnimation(Guid animationId)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                foreach (AnimationDto animation in projectDto_.Animations[animationGroup.Id])
                {
                    if (animation.Id == animationId)
                    {
                        return animation;
                    }
                }
            }

            return null;
        }

        public int GetAnimationIndexFromId(Guid animationId)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                for (int i = 0; i < projectDto_.Animations[animationGroup.Id].Count; i++)
                {
                    if (projectDto_.Animations[animationGroup.Id][i].Id == animationId)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public void DeleteAnimation(Guid animationId)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                for (int i = 0; i < projectDto_.Animations[animationGroup.Id].Count; i++)
                {
                    if (projectDto_.Animations[animationGroup.Id][i].Id == animationId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        nameUtility_.RemoveAnimationName(projectDto_.Animations[animationGroup.Id][i].Name);

                        // Remove the animation itself.
                        projectDto_.Animations[animationGroup.Id].RemoveAt(i);

                        // Delete the frames in this animation, as well as the hitboxes, action points,
                        // and frame triggers in the frames.
                        foreach (FrameDto frame in projectDto_.Frames[animationId])
                        {
                            projectDto_.Hitboxes.Remove(frame.Id);
                            projectDto_.ActionPoints.Remove(frame.Id);
                            projectDto_.FrameTriggers.Remove(frame.Id);
                        }

                        projectDto_.Frames.Remove(animationId);

                        // Clear this animation from any instances that are pointing to it.
                        foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
                        {
                            foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                            {
                                if (animationSlot.Animation == animationId)
                                {
                                    animationSlot.Animation = Guid.Empty;
                                }
                            }
                        }

                        return;
                    }
                }
            }
        }

        public Guid GetAnimationIdFromName(string name)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                foreach (AnimationDto animation in projectDto_.Animations[animationGroup.Id])
                {
                    if (animation.Name.ToUpper() == name.ToUpper())
                    {
                        return animation.Id;
                    }
                }
            }

            return Guid.Empty;
        }

        public string GetAnimationNameFromId(Guid id)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                foreach (AnimationDto animation in projectDto_.Animations[animationGroup.Id])
                {
                    if (animation.Id == id)
                    {
                        return animation.Name;
                    }
                }
            }

            return string.Empty;
        }

        public void SetAnimationAlphaMaskSheet(Guid animationId, Guid alphaMaskSheetId)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                foreach (AnimationDto animation in projectDto_.Animations[animationGroup.Id])
                {
                    if (animation.Id == animationId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animation.AlphaMaskSheet = alphaMaskSheetId;

                        return;
                    }
                }
            }
        }

        public void SetAnimationName(Guid animationId, string name)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                foreach (AnimationDto animation in projectDto_.Animations[animationGroup.Id])
                {
                    if (animation.Id == animationId)
                    {
                        saveProjectState();

                        nameUtility_.UpdateAnimationName(animation.Name, name);

                        ChangesMade = true;

                        animation.Name = name;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSpriteSheet(Guid animationId, Guid spriteSheetId)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                foreach (AnimationDto animation in projectDto_.Animations[animationGroup.Id])
                {
                    if (animation.Id == animationId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animation.SpriteSheet = spriteSheetId;
                        return;
                    }
                }
            }
        }

        #endregion

        #region Animation Group Functions

        public AnimationGroupDto AddAnimationGroup(string name)
        {
            saveProjectState();

            AnimationGroupDto newAnimationGroup = new AnimationGroupDto();
            newAnimationGroup.Name = name;

            projectDto_.AnimationGroups.Add(newAnimationGroup);

            projectDto_.Animations.Add(newAnimationGroup.Id, new List<AnimationDto>());

            ChangesMade = true;

            return newAnimationGroup;
        }

        public AnimationGroupDto GetAnimationGroup(Guid animationGroupId)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                if (animationGroup.Id == animationGroupId)
                {
                    return animationGroup;
                }
            }

            return null;
        }

        public int GetAnimationGroupIndexFromId(Guid animationGroupId)
        {
            for (int i = 0; i < projectDto_.AnimationGroups.Count; i++)
            {
                if (projectDto_.AnimationGroups[i].Id == animationGroupId)
                {
                    return i;
                }
            }

            return -1;
        }

        public void DeleteAnimationGroup(Guid animationGroupId)
        {
            for (int i = 0; i < projectDto_.AnimationGroups.Count; i++)
            {
                if (projectDto_.AnimationGroups[i].Id == animationGroupId)
                {
                    saveProjectState();

                    ChangesMade = true;
                    
                    // Remove the animation itself.
                    projectDto_.AnimationGroups.RemoveAt(i);
                    
                    return;
                }
            }
        }

        public Guid GetAnimationGroupIdFromName(string name)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                if (animationGroup.Name.ToUpper() == name.ToUpper())
                {
                    return animationGroup.Id;
                }
            }

            return Guid.Empty;
        }

        public string GetAnimationGroupNameFromId(Guid id)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                if (animationGroup.Id == id)
                {
                    return animationGroup.Name;
                }
            }

            return string.Empty;
        }

        public void SetAnimationGroupName(Guid animationGroupId, string name)
        {
            foreach (AnimationGroupDto animationGroup in projectDto_.AnimationGroups)
            {
                if (animationGroup.Id == animationGroupId)
                {
                    saveProjectState();
                    
                    ChangesMade = true;

                    animationGroup.Name = name;

                    return;
                }
            }
        }

        #endregion
        
        #region Stateful Entity Functions

        public StatefulEntityDto GetStatefulEntity(Guid entityId)
        {
            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityId)
                {
                    return actor;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    return hudElement;
                }
            }

            return null;
        }

        public void SetStatefulEntityInitialStateId(Guid entityId, Guid stateId)
        {
            saveProjectState();

            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityId)
                {
                    actor.InitialStateId = stateId;

                    ChangesMade = true;

                    return;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    hudElement.InitialStateId = stateId;

                    ChangesMade = true;

                    return;
                }
            }
        }

        public void SetStatefulEntityPivotPoint(Guid entityId, Point pivotPoint)
        {
            saveProjectState();

            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityId)
                {
                    actor.PivotPoint = pivotPoint;

                    ChangesMade = true;

                    return;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    hudElement.PivotPoint = pivotPoint;

                    ChangesMade = true;

                    return;
                }
            }

        }


        public void SetStatefulEntityStageBackgroundDepth(Guid entityId, int stageBackgroundDepth)
        {
            saveProjectState();

            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityId)
                {
                    if (stageBackgroundDepth < 0)
                    {
                        stageBackgroundDepth *= -1;
                    }

                    actor.StageBackgroundDepth = stageBackgroundDepth;
                    
                    ChangesMade = true;

                    return;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    hudElement.StageBackgroundDepth = stageBackgroundDepth;

                    ChangesMade = true;

                    return;
                }
            }
        }


        public void SetStatefulEntityStageWidth(Guid entityId, int stageWidth)
        {
            saveProjectState();

            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityId)
                {
                    actor.StageWidth = stageWidth;
                    
                    updateActorWidgetBoundingBoxSize(actor.Id, new Size(actor.StageWidth, actor.StageHeight));

                    ChangesMade = true;

                    return;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    hudElement.StageWidth = stageWidth;

                    updateHudElementWidgetBoundingBoxSize(hudElement.Id, new Size(hudElement.StageWidth, hudElement.StageHeight));

                    ChangesMade = true;

                    return;
                }
            }
        }

        public void SetStatefulEntityStageHeight(Guid entityId, int stageHeight)
        {
            saveProjectState();

            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityId)
                {
                    actor.StageHeight = stageHeight;

                    updateActorWidgetBoundingBoxSize(actor.Id, new Size(actor.StageWidth, actor.StageHeight));

                    ChangesMade = true;

                    return;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    hudElement.StageHeight = stageHeight;

                    updateHudElementWidgetBoundingBoxSize(hudElement.Id, new Size(hudElement.StageWidth, hudElement.StageHeight));

                    ChangesMade = true;

                    return;
                }
            }
        }

        public void SetStatefulEntityStageOriginLocation(Guid entityId, OriginLocation stageOriginLocation)
        {
            saveProjectState();

            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityId)
                {
                    ActorDto actorEntity = (ActorDto)GetEntity(entityId);

                    Point stageCenter = new Point(actorEntity.StageWidth / 2, actorEntity.StageHeight / 2);

                    // Get the difference between the current origin transformation that was applied to the bounding box,
                    // and the new one. Use the difference as the adjustment.

                    Point oldStageOriginTransformation;

                    // findme origin transform switch
                    switch (actorEntity.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:

                            oldStageOriginTransformation = new Point(0, 0);

                            break;

                        case OriginLocation.TopMiddle:

                            oldStageOriginTransformation = new Point(stageCenter.X, 0);

                            break;

                        case OriginLocation.TopRight:

                            oldStageOriginTransformation = new Point(actorEntity.StageWidth, 0);

                            break;

                        case OriginLocation.MiddleLeft:

                            oldStageOriginTransformation = new Point(0, stageCenter.Y);

                            break;

                        case OriginLocation.Center:

                            oldStageOriginTransformation = new Point(stageCenter.X, stageCenter.Y);

                            break;

                        case OriginLocation.MiddleRight:

                            oldStageOriginTransformation = new Point(actorEntity.StageWidth, stageCenter.Y);

                            break;

                        case OriginLocation.BottomLeft:

                            oldStageOriginTransformation = new Point(0, actorEntity.StageHeight);

                            break;

                        case OriginLocation.BottomMiddle:

                            oldStageOriginTransformation = new Point(stageCenter.X, actorEntity.StageHeight);

                            break;

                        case OriginLocation.BottomRight:

                            oldStageOriginTransformation = new Point(actorEntity.StageWidth, actorEntity.StageHeight);

                            break;

                        default:

                            // Shouldn't hit this, but just in case, the point needs to be initialized.
                            oldStageOriginTransformation = new Point(0, 0);

                            break;
                    }
                    
                    actor.StageOriginLocation = stageOriginLocation;
                    
                    Point newStageOriginTransformation;

                    switch (actorEntity.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:

                            newStageOriginTransformation = new Point(0, 0);

                            break;

                        case OriginLocation.TopMiddle:

                            newStageOriginTransformation = new Point(stageCenter.X, 0);

                            break;

                        case OriginLocation.TopRight:

                            newStageOriginTransformation = new Point(actorEntity.StageWidth, 0);

                            break;

                        case OriginLocation.MiddleLeft:

                            newStageOriginTransformation = new Point(0, stageCenter.Y);

                            break;

                        case OriginLocation.Center:

                            newStageOriginTransformation = new Point(stageCenter.X, stageCenter.Y);

                            break;

                        case OriginLocation.MiddleRight:

                            newStageOriginTransformation = new Point(actorEntity.StageWidth, stageCenter.Y);

                            break;

                        case OriginLocation.BottomLeft:

                            newStageOriginTransformation = new Point(0, actorEntity.StageHeight);

                            break;

                        case OriginLocation.BottomMiddle:

                            newStageOriginTransformation = new Point(stageCenter.X, actorEntity.StageHeight);

                            break;

                        case OriginLocation.BottomRight:

                            newStageOriginTransformation = new Point(actorEntity.StageWidth, actorEntity.StageHeight);

                            break;

                        default:

                            // Shouldn't hit this, but just in case, the point needs to be initialized.
                            newStageOriginTransformation = new Point(0, 0);

                            break;
                    }

                    Point adjustment = new Point(oldStageOriginTransformation.X - newStageOriginTransformation.X, oldStageOriginTransformation.Y - newStageOriginTransformation.Y);
                    
                    // Adjust the bounding box position.
                    adjustActorWidgetBoundingBoxPosition(actor.Id, adjustment);

                    ChangesMade = true;

                    return;
                }
            }

            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    HudElementDto hudElementEntity = (HudElementDto)GetEntity(entityId);

                    Point stageCenter = new Point(hudElementEntity.StageWidth / 2, hudElementEntity.StageHeight / 2);

                    // Get the difference between the current origin transformation that was applied to the bounding box,
                    // and the new one. Use the difference as the adjustment.

                    Point oldStageOriginTransformation;

                    // findme origin transform switch
                    switch (hudElementEntity.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:

                            oldStageOriginTransformation = new Point(0, 0);

                            break;

                        case OriginLocation.TopMiddle:

                            oldStageOriginTransformation = new Point(stageCenter.X, 0);

                            break;

                        case OriginLocation.TopRight:

                            oldStageOriginTransformation = new Point(hudElementEntity.StageWidth, 0);

                            break;

                        case OriginLocation.MiddleLeft:

                            oldStageOriginTransformation = new Point(0, stageCenter.Y);

                            break;

                        case OriginLocation.Center:

                            oldStageOriginTransformation = new Point(stageCenter.X, stageCenter.Y);

                            break;

                        case OriginLocation.MiddleRight:

                            oldStageOriginTransformation = new Point(hudElementEntity.StageWidth, stageCenter.Y);

                            break;

                        case OriginLocation.BottomLeft:

                            oldStageOriginTransformation = new Point(0, hudElementEntity.StageHeight);

                            break;

                        case OriginLocation.BottomMiddle:

                            oldStageOriginTransformation = new Point(stageCenter.X, hudElementEntity.StageHeight);

                            break;

                        case OriginLocation.BottomRight:

                            oldStageOriginTransformation = new Point(hudElementEntity.StageWidth, hudElementEntity.StageHeight);

                            break;

                        default:

                            // Shouldn't hit this, but just in case, the point needs to be initialized.
                            oldStageOriginTransformation = new Point(0, 0);

                            break;
                    }

                    hudElement.StageOriginLocation = stageOriginLocation;

                    Point newStageOriginTransformation;

                    switch (hudElementEntity.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:

                            newStageOriginTransformation = new Point(0, 0);

                            break;

                        case OriginLocation.TopMiddle:

                            newStageOriginTransformation = new Point(stageCenter.X, 0);

                            break;

                        case OriginLocation.TopRight:

                            newStageOriginTransformation = new Point(hudElementEntity.StageWidth, 0);

                            break;

                        case OriginLocation.MiddleLeft:

                            newStageOriginTransformation = new Point(0, stageCenter.Y);

                            break;

                        case OriginLocation.Center:

                            newStageOriginTransformation = new Point(stageCenter.X, stageCenter.Y);

                            break;

                        case OriginLocation.MiddleRight:

                            newStageOriginTransformation = new Point(hudElementEntity.StageWidth, stageCenter.Y);

                            break;

                        case OriginLocation.BottomLeft:

                            newStageOriginTransformation = new Point(0, hudElementEntity.StageHeight);

                            break;

                        case OriginLocation.BottomMiddle:

                            newStageOriginTransformation = new Point(stageCenter.X, hudElementEntity.StageHeight);

                            break;

                        case OriginLocation.BottomRight:

                            newStageOriginTransformation = new Point(hudElementEntity.StageWidth, hudElementEntity.StageHeight);

                            break;

                        default:

                            // Shouldn't hit this, but just in case, the point needs to be initialized.
                            newStageOriginTransformation = new Point(0, 0);

                            break;
                    }

                    Point adjustment = new Point(oldStageOriginTransformation.X - newStageOriginTransformation.X, oldStageOriginTransformation.Y - newStageOriginTransformation.Y);

                    // Adjust the bounding box position.
                    adjustHudElementWidgetBoundingBoxPosition(hudElement.Id, adjustment);

                    ChangesMade = true;

                    return;
                }
            }
        }

        #endregion

        #region Actor Functions

        public ActorDto AddActor(string name)
        {
            saveProjectState();

            ActorDto actor = new ActorDto();

            actor.Name = name;

            projectDto_.Actors.Add(actor);

            projectDto_.States[actor.Id] = new List<StateDto>();
            projectDto_.Properties[actor.Id] = new List<PropertyDto>();

            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.Entity;
            script.OwnerId = actor.Id;
            script.Name = actor.Name;
            projectDto_.Scripts[actor.Id] = script;
            
            ChangesMade = true;

            OnActorAdd(new ActorAddedEventArgs(actor.Id));

            return actor;
        }

        public ActorDto DuplicateActor(Guid actorIdToDuplicate, string name)
        {
            saveProjectState();

            ActorDto actorToDuplicate = GetActor(actorIdToDuplicate);

            ActorDto actor = new ActorDto();

            actor.Name = name;
            actor.InitialStateId = actorToDuplicate.InitialStateId;
            actor.Classification = actorToDuplicate.Classification;
            actor.KeepRoomActive = actorToDuplicate.KeepRoomActive;
            actor.StageBackgroundDepth = actorToDuplicate.StageBackgroundDepth;
            actor.StageHeight = actorToDuplicate.StageHeight;
            actor.StageWidth = actorToDuplicate.StageWidth;
            actor.Tag = actorToDuplicate.Tag;
            actor.BoundRect.Height = actorToDuplicate.BoundRect.Height;
            actor.BoundRect.Width = actorToDuplicate.BoundRect.Width;
            actor.BoundRect.Top = actorToDuplicate.BoundRect.Top;
            actor.BoundRect.Left = actorToDuplicate.BoundRect.Left;
            actor.PivotPoint = new Point(actorToDuplicate.PivotPoint.X, actorToDuplicate.PivotPoint.Y);

            projectDto_.Actors.Add(actor);
            
            // Copy the states for this actor.
            projectDto_.States[actor.Id] = new List<StateDto>();

            List<string> lstStateNames = new List<string>();

            foreach (StateDto state in projectDto_.States[actorToDuplicate.Id])
            {
                StateDto newState = new StateDto();
                newState.Name = state.Name;
                newState.OwnerId = actor.Id;                

                projectDto_.States[actor.Id].Add(newState);
                
                // Copy the hitboxes for this state.
                projectDto_.Hitboxes[newState.Id] = new List<HitboxDto>();

                foreach (HitboxDto hitbox in projectDto_.Hitboxes[state.Id])
                {
                    HitboxDto newHitbox = new HitboxDto();

                    newHitbox.OwnerId = newState.Id;
                    newHitbox.RootOwnerId = actor.Id;
                    newHitbox.CornerPoint1.X = hitbox.CornerPoint1.X;
                    newHitbox.CornerPoint1.Y = hitbox.CornerPoint1.Y;
                    newHitbox.HitboxRect.Left = hitbox.HitboxRect.Left;
                    newHitbox.HitboxRect.Top = hitbox.HitboxRect.Top;
                    newHitbox.HitboxRect.Width = hitbox.HitboxRect.Width;
                    newHitbox.HitboxRect.Height = hitbox.HitboxRect.Height;
                    newHitbox.Identity = hitbox.Identity;
                    newHitbox.IsSolid = hitbox.IsSolid;
                    newHitbox.Priority = hitbox.Priority;
                    newHitbox.RotationDegrees = hitbox.RotationDegrees;

                    projectDto_.Hitboxes[newState.Id].Add(newHitbox);
                }

                // Copy the animation slots for this state.
                projectDto_.AnimationSlots[newState.Id] = new List<AnimationSlotDto>();

                foreach (AnimationSlotDto animationSlot in projectDto_.AnimationSlots[state.Id])
                {
                    AnimationSlotDto newAnimationSlot = new AnimationSlotDto();

                    newAnimationSlot.Name = animationSlot.Name;
                    newAnimationSlot.OwnerId = newState.Id;
                    newAnimationSlot.RootOwnerId = actor.Id;
                    newAnimationSlot.Background = animationSlot.Background;
                    newAnimationSlot.Animation = animationSlot.Animation;
                    newAnimationSlot.HueColor.Alpha = animationSlot.HueColor.Alpha;
                    newAnimationSlot.HueColor.Red = animationSlot.HueColor.Red;
                    newAnimationSlot.HueColor.Green = animationSlot.HueColor.Green;
                    newAnimationSlot.HueColor.Blue = animationSlot.HueColor.Blue;
                    newAnimationSlot.Position.X = animationSlot.Position.X;
                    newAnimationSlot.Position.Y = animationSlot.Position.Y;

                    projectDto_.AnimationSlots[newState.Id].Add(newAnimationSlot);
                }

                lstStateNames.Add(name);

            }

            nameUtility_.AddStateNames(actor.Id, lstStateNames);

            // Copy the properties for this actor.
            projectDto_.Properties[actor.Id] = new List<PropertyDto>();

            foreach (PropertyDto property in projectDto_.Properties[actorToDuplicate.Id])
            {
                PropertyDto newProperty = new PropertyDto();
                newProperty.Name = property.Name;
                newProperty.DefaultValue = property.DefaultValue;
                newProperty.ReadOnly = property.ReadOnly;
                newProperty.Reserved = property.Reserved;
                newProperty.Value = property.Value;
                newProperty.OwnerId = actor.Id;
                projectDto_.Properties[actor.Id].Add(newProperty);                
            }

            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.Entity;
            script.OwnerId = actor.Id;
            script.Name = actor.Name;
            projectDto_.Scripts[actor.Id] = script;

            ChangesMade = true;

            OnActorAdd(new ActorAddedEventArgs(actor.Id));

            return actor;
        }

        public void SelectActor(int actorIndex)
        {
            if (actorIndex >= 0 && actorIndex < projectDto_.Actors.Count)
            {
                unloadCursorResources();

                projectUiState_.SelectedActorIndex = actorIndex;

                Guid actorId = projectDto_.Actors[actorIndex].Id;

                projectUiState_.SelectedActorId = actorId;

                projectUiState_.SelectedHudElementId = Guid.Empty;
                projectUiState_.SelectedHudElementIndex = -1;

                projectUiState_.SelectedEventId = Guid.Empty;
                projectUiState_.SelectedEventIndex = -1;

                projectUiState_.SelectedTileObjectId = Guid.Empty;
                projectUiState_.SelectedTileObjectIndex = -1;

                projectUiState_.SelectedSpawnPointId = Guid.Empty;
                projectUiState_.SelectedSpawnPointIndex = -1;

                projectUiState_.SelectedTileSheetId = Guid.Empty;
                projectUiState_.SelectedTileSheetIndex = -1;

                refreshViews();
            }
        }

        public void DeleteActor(int actorIndex)
        {
            if (actorIndex < 0 || actorIndex >= projectDto_.Actors.Count)
            {
                return;
            }

            saveProjectState();

            Guid actorId = projectDto_.Actors[actorIndex].Id;

            projectDto_.Scripts.Remove(actorId);

            // Delete animation slots and hitboxes in this state.
            foreach (StateDto state in projectDto_.States[actorId])
            {
                projectDto_.Hitboxes.Remove(state.Id);                
                projectDto_.AnimationSlots.Remove(state.Id);
            }

            projectDto_.States.Remove(actorId);

            projectDto_.Properties.Remove(actorId);

            // Delete any actor map widgets of this type.
            List<Guid> lstMapWidgetIds = new List<Guid>(projectDto_.MapWidgets[MapWidgetType.Actor].Keys);

            foreach (Guid mapWidgetId in lstMapWidgetIds)
            {
                ActorWidgetDto actorWidget = (ActorWidgetDto)projectDto_.MapWidgets[MapWidgetType.Actor][mapWidgetId];

                if (actorWidget.EntityId == actorId)
                {
                    deleteMapWidget(actorWidget.Id, true);                    
                }
            }

            projectDto_.Actors.RemoveAt(actorIndex);

            // If the deleted actor index was less than the deleted index, decrement it.
            if (actorIndex < projectUiState_.SelectedActorIndex)
            {
                projectUiState_.SelectedActorIndex -= 1;
            }

            if (projectUiState_.SelectedActorIndex >= projectDto_.Actors.Count)
            {
                projectUiState_.SelectedActorIndex = projectDto_.Actors.Count - 1;
            }

            // The actor positions may have change. Always re-select just in case, to make
            // sure the correct one is selected.
            if (projectUiState_.SelectedActorIndex > -1)
            {
                SelectActor(projectUiState_.SelectedActorIndex);
            }
            else
            {
                projectUiState_.SelectedActorId = Guid.Empty;
            }

            ChangesMade = true;

            refreshViews();
        }

        public ActorDto GetActor(Guid assetId)
        {
            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == assetId)
                {
                    return actor;
                }
            }

            return null;
        }

        public int GetActorIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.Actors.Count; i++)
            {
                if (projectDto_.Actors[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetActorIdFromIndex(int index)
        {
            for (int i = 0; i < projectDto_.Actors.Count; i++)
            {
                if (i == index)
                {
                    return projectDto_.Actors[i].Id;
                }
            }

            return Guid.Empty;
        }

        public void SetActorName(Guid actorId, string name)
        {
            int actorIndex = GetActorIndexFromId(actorId);

            nameValidator_.ValidateAssetName(actorId, projectDto_, name);
            
            saveProjectState();

            ChangesMade = true;

            projectDto_.Actors[actorIndex].Name = name;

            projectDto_.Scripts[actorId].Name = name;
        }

        public void SetActorTag(Guid actorId, string tag)
        {
            int actorIndex = GetActorIndexFromId(actorId);

            ChangesMade = true;

            projectDto_.Actors[actorIndex].Tag = tag;
        }

        public void SetActorInitialState(Guid actorId, Guid initialStateId)
        {
            saveProjectState();

            int actorIndex = GetActorIndexFromId(actorId);

            ChangesMade = true;

            projectDto_.Actors[actorIndex].InitialStateId = initialStateId;

            refreshViews();
        }

        public void SetActorClassification(Guid actorId, Guid classificationId)
        {
            saveProjectState();

            int actorIndex = GetActorIndexFromId(actorId);

            ChangesMade = true;

            projectDto_.Actors[actorIndex].Classification = classificationId;

            refreshViews();
        }

        public void SetActorKeepRoomActive(Guid actorId, bool keepRoomActive)
        {
            saveProjectState();

            int actorIndex = GetActorIndexFromId(actorId);

            ChangesMade = true;

            projectDto_.Actors[actorIndex].KeepRoomActive = keepRoomActive;

            refreshViews();
        }

        #endregion

        #region Event Functions

        public EventDto AddEvent(string name)
        {
            saveProjectState();

            EventDto eventEntity = new EventDto();

            eventEntity.Name = name;

            projectDto_.Events.Add(eventEntity);

            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.Entity;
            script.OwnerId = eventEntity.Id;
            script.Name = eventEntity.Name;
            projectDto_.Scripts[eventEntity.Id] = script;

            projectDto_.Properties[eventEntity.Id] = new List<PropertyDto>();

            ChangesMade = true;

            OnEventAdd(new EventAddedEventArgs(eventEntity.Id));

            return eventEntity;
        }

        public void SelectEvent(int eventIndex)
        {
            if (eventIndex >= 0 && eventIndex < projectDto_.Events.Count)
            {
                projectUiState_.SelectedEventIndex = eventIndex;

                Guid eventId = projectDto_.Events[eventIndex].Id;

                projectUiState_.SelectedEventId = eventId;

                projectUiState_.SelectedActorId = Guid.Empty;
                projectUiState_.SelectedActorIndex = -1;

                projectUiState_.SelectedHudElementId = Guid.Empty;
                projectUiState_.SelectedHudElementIndex = -1;

                projectUiState_.SelectedTileObjectId = Guid.Empty;
                projectUiState_.SelectedTileObjectIndex = -1;

                projectUiState_.SelectedSpawnPointId = Guid.Empty;
                projectUiState_.SelectedSpawnPointIndex = -1;

                projectUiState_.SelectedTileSheetId = Guid.Empty;
                projectUiState_.SelectedTileSheetIndex = -1;

                refreshViews();
            }
        }

        public void DeleteEvent(int eventIndex)
        {
            if (eventIndex < 0 || eventIndex >= projectDto_.Events.Count)
            {
                return;
            }

            saveProjectState();

            Guid eventId = projectDto_.Events[eventIndex].Id;

            // Delete any event map widgets of this type.
            List<Guid> lstMapWidgetIds = new List<Guid>(projectDto_.MapWidgets[MapWidgetType.Event].Keys);

            foreach (Guid mapWidgetId in lstMapWidgetIds)
            {
                EventWidgetDto eventWidget = (EventWidgetDto)projectDto_.MapWidgets[MapWidgetType.Event][mapWidgetId];

                if (eventWidget.EntityId == eventId)
                {
                    deleteMapWidget(eventWidget.Id, true);
                }
            }

            // Delete the properties in this event.
            projectDto_.Properties.Remove(eventId);

            projectDto_.Scripts.Remove(eventId);

            projectDto_.Events.RemoveAt(eventIndex);

            // If the deleted actor index was less than the deleted index, decrement it.
            if (eventIndex < projectUiState_.SelectedEventIndex)
            {
                projectUiState_.SelectedEventIndex -= 1;
            }

            if (projectUiState_.SelectedEventIndex >= projectDto_.Events.Count)
            {
                projectUiState_.SelectedEventIndex = projectDto_.Events.Count - 1;
            }

            // The actor positions may have change. Always re-select just in case, to make
            // sure the correct one is selected.
            if (projectUiState_.SelectedEventIndex > -1)
            {
                SelectEvent(projectUiState_.SelectedEventIndex);
            }
            else
            {
                projectUiState_.SelectedEventId = Guid.Empty;
            }

            ChangesMade = true;

            refreshViews();
        }

        public EventDto GetEvent(Guid assetId)
        {
            foreach (EventDto eventEntity in projectDto_.Events)
            {
                if (eventEntity.Id == assetId)
                {
                    return eventEntity;
                }
            }

            return null;
        }

        public int GetEventIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.Events.Count; i++)
            {
                if (projectDto_.Events[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetEventName(Guid eventId, string name)
        {
            int eventIndex = GetEventIndexFromId(eventId);

            nameValidator_.ValidateAssetName(eventId, projectDto_, name);
            
            saveProjectState();

            ChangesMade = true;

            projectDto_.Events[eventIndex].Name = name;

            projectDto_.Scripts[eventId].Name = name;
        }

        public void SetEventTag(Guid eventId, string tag)
        {
            int eventIndex = GetEventIndexFromId(eventId);

            ChangesMade = true;

            projectDto_.Events[eventIndex].Tag = tag;
        }

        public void SetEventClassification(Guid eventId, Guid classificationId)
        {
            saveProjectState();

            int eventIndex = GetEventIndexFromId(eventId);

            ChangesMade = true;

            projectDto_.Events[eventIndex].Classification = classificationId;

            refreshViews();
        }

        #endregion

        #region HUD Element Functions

        public HudElementDto AddHudElement(string name)
        {
            saveProjectState();

            HudElementDto hudElement = new HudElementDto();

            hudElement.Name = name;

            projectDto_.HudElements.Add(hudElement);

            projectDto_.States[hudElement.Id] = new List<StateDto>();
            projectDto_.Properties[hudElement.Id] = new List<PropertyDto>();

            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.Entity;
            script.OwnerId = hudElement.Id;
            script.Name = hudElement.Name;
            projectDto_.Scripts[hudElement.Id] = script;
            
            ChangesMade = true;

            OnHudElementAdd(new HudElementAddedEventArgs(hudElement.Id));

            return hudElement;
        }

        public void SelectHudElement(int hudElementIndex)
        {
            if (hudElementIndex >= 0 && hudElementIndex < projectDto_.HudElements.Count)
            {
                projectUiState_.SelectedHudElementIndex = hudElementIndex;

                Guid hudElementId = projectDto_.HudElements[hudElementIndex].Id;

                projectUiState_.SelectedHudElementId = hudElementId;

                projectUiState_.SelectedActorId = Guid.Empty;
                projectUiState_.SelectedActorIndex = -1;

                projectUiState_.SelectedEventId = Guid.Empty;
                projectUiState_.SelectedEventIndex = -1;

                projectUiState_.SelectedTileObjectId = Guid.Empty;
                projectUiState_.SelectedTileObjectIndex = -1;

                projectUiState_.SelectedSpawnPointId = Guid.Empty;
                projectUiState_.SelectedSpawnPointIndex = -1;

                projectUiState_.SelectedTileSheetId = Guid.Empty;
                projectUiState_.SelectedTileSheetIndex = -1;

                refreshViews();
            }
        }

        public void DeleteHudElement(int hudElementIndex)
        {
            if (hudElementIndex < 0 || hudElementIndex >= projectDto_.HudElements.Count)
            {
                return;
            }

            saveProjectState();

            Guid hudElementId = projectDto_.HudElements[hudElementIndex].Id;
            
            projectDto_.Scripts.Remove(hudElementId);

            // Delete animation slots in this state.
            foreach (StateDto state in projectDto_.States[hudElementId])
            {             
                projectDto_.AnimationSlots.Remove(state.Id);
            }

            projectDto_.States.Remove(hudElementId);
            projectDto_.Properties.Remove(hudElementId);

            // Delete any HUD element widgets of this type.
            List<Guid> lstMapWidgetIds = new List<Guid>(projectDto_.MapWidgets[MapWidgetType.HudElement].Keys);

            foreach (Guid mapWidgetId in lstMapWidgetIds)
            {
                HudElementWidgetDto hudElementWidget = (HudElementWidgetDto)projectDto_.MapWidgets[MapWidgetType.HudElement][mapWidgetId];

                if (hudElementWidget.EntityId == hudElementId)
                {
                    deleteMapWidget(hudElementWidget.Id, true);
                }
            }

            projectDto_.HudElements.RemoveAt(hudElementIndex);

            ChangesMade = true;

            refreshViews();
        }

        public HudElementDto GetHudElement(Guid assetId)
        {
            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == assetId)
                {
                    return hudElement;
                }
            }

            return null;
        }

        public int GetHudElementIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.HudElements.Count; i++)
            {
                if (projectDto_.HudElements[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public void SetHudElementName(Guid hudElementId, string name)
        {
            int hudElementIndex = GetHudElementIndexFromId(hudElementId);

            nameValidator_.ValidateAssetName(hudElementId, projectDto_, name);
            
            saveProjectState();

            ChangesMade = true;

            projectDto_.HudElements[hudElementIndex].Name = name;

            projectDto_.Scripts[hudElementId].Name = name;
        }

        public void SetHudElementTag(Guid hudElementId, string tag)
        {
            int hudElementIndex = GetHudElementIndexFromId(hudElementId);

            ChangesMade = true;

            projectDto_.HudElements[hudElementIndex].Tag = tag;
        }

        public void SetHudElementInitialState(Guid hudElementId, Guid initialStateId)
        {
            saveProjectState();

            int hudElementIndex = GetHudElementIndexFromId(hudElementId);

            ChangesMade = true;

            projectDto_.HudElements[hudElementIndex].InitialStateId = initialStateId;

            refreshViews();
        }

        public void SetHudElementClassification(Guid hudElementId, Guid classificationId)
        {
            saveProjectState();

            int hudElementIndex = GetHudElementIndexFromId(hudElementId);

            ChangesMade = true;

            projectDto_.HudElements[hudElementIndex].Classification = classificationId;

            refreshViews();
        }

        #endregion

        #region State Functions
        
        public StateDto AddState(Guid entityId, string name)
        {
            saveProjectState();

            StateDto newState = new StateDto();
            newState.Name = name;
            newState.OwnerId = entityId;
            
            projectDto_.States[entityId].Add(newState);

            if (projectDto_.States[entityId].Count == 1)
            {
                SetStatefulEntityInitialStateId(entityId, newState.Id);
            }

            projectDto_.Hitboxes[newState.Id] = new List<HitboxDto>();
            projectDto_.AnimationSlots[newState.Id] = new List<AnimationSlotDto>();
            
            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddStateNames(entityId, lstNames);

            ChangesMade = true;

            return newState;
        }

        public StateDto GetState(Guid StateId)
        {
            foreach (KeyValuePair<Guid, List<StateDto>> StateList in projectDto_.States)
            {
                foreach (StateDto State in StateList.Value)
                {
                    if (State.Id == StateId)
                    {
                        return State;
                    }
                }
            }

            return null;
        }

        public Guid GetStateOwnerId(Guid StateId)
        {
            for (int i = 0; i < projectDto_.Actors.Count; i++)
            {
                foreach (KeyValuePair<Guid, List<StateDto>> StateList in projectDto_.States)
                {
                    foreach (StateDto State in StateList.Value)
                    {
                        if (State.Id == StateId)
                        {
                            return StateList.Key;
                        }
                    }
                }
            }

            return Guid.Empty;
        }

        public Guid GetStateIdFromName(Guid ownerId, string stateName)
        {
            foreach (StateDto state in projectDto_.States[ownerId])
            {
                if (state.Name.ToUpper() == stateName.ToUpper())
                {
                    return state.Id;
                }
            }

            return Guid.Empty;
        }

        public List<string> GetStateNames(Guid entityId)
        {
            return nameUtility_.GetStateNames(entityId);
        }

        public void DeleteState(Guid stateId)
        {
            foreach (KeyValuePair<Guid, List<StateDto>> stateList in projectDto_.States)
            {
                for (int i = 0; i < stateList.Value.Count; i++)
                {
                    if (stateList.Value[i].Id == stateId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        // Remove the state name from the list of names.                       
                        nameUtility_.RemoveStateName(stateList.Key, stateList.Value[i].Name);

                        stateList.Value.RemoveAt(i);

                        // Delete the animation slots and hitboxes in this State.
                        projectDto_.AnimationSlots.Remove(stateId);
                        projectDto_.Hitboxes.Remove(stateId);

                        return;
                    }
                }
            }
        }

        public void SetStateName(Guid stateId, string name)
        {
            foreach (KeyValuePair<Guid, List<StateDto>> stateList in projectDto_.States)
            {
                foreach (StateDto state in stateList.Value)
                {
                    if (state.Id == stateId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        nameUtility_.RemoveStateName(state.OwnerId, state.Name);

                        state.Name = name;

                        List<string> lstNames = new List<string>(new string[] { name });

                        nameUtility_.AddStateNames(state.OwnerId, lstNames);

                        return;
                    }
                }
            }
        }

        #endregion

        #region Frame Functions

        public FrameDto AddFrame(Guid animationId)
        {
            saveProjectState();

            AnimationDto animation = GetAnimation(animationId);

            FrameDto newFrame = new FrameDto();

            newFrame.OwnerId = animationId;
            newFrame.RootOwnerId = animation.OwnerId;

            projectDto_.Frames[animationId].Add(newFrame);

            projectDto_.Hitboxes[newFrame.Id] = new List<HitboxDto>();
            projectDto_.FrameTriggers[newFrame.Id] = new List<FrameTriggerDto>();
            projectDto_.ActionPoints[newFrame.Id] = new List<ActionPointDto>();

            ChangesMade = true;

            return newFrame;
        }

        public FrameDto AddFrame(Guid animationId, int sheetCellIndex)
        {
            saveProjectState();

            AnimationDto animation = GetAnimation(animationId);

            FrameDto newFrame = new FrameDto();
            newFrame.SheetCellIndex = sheetCellIndex;

            newFrame.OwnerId = animationId;
            newFrame.RootOwnerId = animation.OwnerId;

            projectDto_.Frames[animationId].Add(newFrame);

            projectDto_.Hitboxes[newFrame.Id] = new List<HitboxDto>();
            projectDto_.FrameTriggers[newFrame.Id] = new List<FrameTriggerDto>();
            projectDto_.ActionPoints[newFrame.Id] = new List<ActionPointDto>();

            ChangesMade = true;

            return newFrame;            
        }

        public FrameDto GetFrame(Guid frameId)
        {
            foreach (KeyValuePair<Guid, List<FrameDto>> frameList in projectDto_.Frames)
            {
                foreach (FrameDto frame in frameList.Value)
                {
                    if (frame.Id == frameId)
                    {
                        return frame;
                    }
                }
            }

            return null;
        }

        public void DeleteFrame(Guid frameId)
        {
            foreach (KeyValuePair<Guid, List<FrameDto>> frameList in projectDto_.Frames)
            {
                for (int i = 0; i < frameList.Value.Count; i++)
                {
                    if (frameList.Value[i].Id == frameId)
                    {
                        saveProjectState();

                        frameList.Value.RemoveAt(i);

                        ChangesMade = true;

                        // Delete the hitboxes, actions points, and frame triggers in this frame.
                        projectDto_.Hitboxes.Remove(frameId);
                        projectDto_.ActionPoints.Remove(frameId);
                        projectDto_.FrameTriggers.Remove(frameId);

                        return;
                    }
                }
            }
        }

        public void SetFrameAlphaMaskCellIndex(Guid frameId, int? cellIndex)
        {
            foreach (KeyValuePair<Guid, List<FrameDto>> frameList in projectDto_.Frames)
            {
                foreach (FrameDto frame in frameList.Value)
                {
                    if (frame.Id == frameId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        frame.AlphaMaskCellIndex = cellIndex;

                        return;
                    }
                }
            }
        }

        public void SetFrameSpriteSheetCellIndex(Guid frameId, int? cellIndex)
        {
            foreach (KeyValuePair<Guid, List<FrameDto>> frameList in projectDto_.Frames)
            {
                foreach (FrameDto frame in frameList.Value)
                {
                    if (frame.Id == frameId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        frame.SheetCellIndex = cellIndex;

                        return;
                    }
                }
            }
        }

        #endregion

        #region Frame Trigger Functions

        public FrameTriggerDto AddFrameTrigger(Guid frameId)
        {
            saveProjectState();

            FrameTriggerDto newFrameTrigger = new FrameTriggerDto();

            FrameDto frame = GetFrame(frameId);
            
            newFrameTrigger.OwnerId = frameId;
            newFrameTrigger.RootOwnerId = frame.RootOwnerId;

            projectDto_.FrameTriggers[frameId].Add(newFrameTrigger);

            ChangesMade = true;

            return newFrameTrigger;
        }

        public FrameTriggerDto GetFrameTrigger(Guid frameTriggerId)
        {
            foreach (KeyValuePair<Guid, List<FrameTriggerDto>> frameTriggerList in projectDto_.FrameTriggers)
            {
                foreach (FrameTriggerDto frameTrigger in frameTriggerList.Value)
                {
                    if (frameTrigger.Id == frameTriggerId)
                    {
                        return frameTrigger;
                    }
                }
            }

            return null;
        }

        public void SetTriggerSignal(Guid frameTriggerId, Guid triggerSignalId)
        {
            foreach (KeyValuePair<Guid, List<FrameTriggerDto>> frameTriggerList in projectDto_.FrameTriggers)
            {
                foreach (FrameTriggerDto frameTrigger in frameTriggerList.Value)
                {
                    if (frameTrigger.Id == frameTriggerId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        frameTrigger.Signal = triggerSignalId;

                        return;
                    }
                }
            }
        }

        public void DeleteFrameTrigger(Guid frameTriggerId)
        {
            foreach (KeyValuePair<Guid, List<FrameTriggerDto>> frameTriggerList in projectDto_.FrameTriggers)
            {
                for (int i = 0; i < frameTriggerList.Value.Count; i++)
                {
                    if (frameTriggerList.Value[i].Id == frameTriggerId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        frameTriggerList.Value.RemoveAt(i);

                        return;
                    }
                }
            }
        }

        #endregion

        #region Action Points Functions

        public ActionPointDto AddActionPoint(Guid frameId)
        {
            saveProjectState();

            ActionPointDto newActionPoint = new ActionPointDto();

            FrameDto frame = GetFrame(frameId);

            newActionPoint.OwnerId = frameId;
            newActionPoint.RootOwnerId = frame.RootOwnerId;

            projectDto_.ActionPoints[frameId].Add(newActionPoint);

            ChangesMade = true;

            return newActionPoint;
        }

        public ActionPointDto GetActionPoint(Guid actionPointId)
        {
            foreach (KeyValuePair<Guid, List<ActionPointDto>> actionPointList in projectDto_.ActionPoints)
            {
                foreach (ActionPointDto actionPoint in actionPointList.Value)
                {
                    if (actionPoint.Id == actionPointId)
                    {
                        return actionPoint;
                    }
                }
            }

            return null;
        }

        public void SetActionPointName(Guid actionPointId, string name)
        {
            foreach (KeyValuePair<Guid, List<ActionPointDto>> actionPointList in projectDto_.ActionPoints)
            {
                foreach (ActionPointDto actionPoint in actionPointList.Value)
                {
                    if (actionPoint.Id == actionPointId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        actionPoint.Name = name;
                        return;
                    }
                }
            }
        }

        public void SetActionPointPosition(Guid actionPointId, Point2D position)
        {
            foreach (KeyValuePair<Guid, List<ActionPointDto>> actionPointList in projectDto_.ActionPoints)
            {
                foreach (ActionPointDto actionPoint in actionPointList.Value)
                {
                    if (actionPoint.Id == actionPointId)
                    {
                        saveProjectState();

                        ChangesMade = true;
                        
                        actionPoint.Position.X = position.X;

                        actionPoint.Position.Y = position.Y;

                        return;
                    }
                }
            }
        }

        public void SetActionPointPositionLeft(Guid actionPointId, int left)
        {
            foreach (KeyValuePair<Guid, List<ActionPointDto>> actionPointList in projectDto_.ActionPoints)
            {
                foreach (ActionPointDto actionPoint in actionPointList.Value)
                {
                    if (actionPoint.Id == actionPointId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        actionPoint.Position.X = left;
                        return;
                    }
                }
            }
        }

        public void SetActionPointPositionTop(Guid actionPointId, int top)
        {
            foreach (KeyValuePair<Guid, List<ActionPointDto>> actionPointList in projectDto_.ActionPoints)
            {
                foreach (ActionPointDto actionPoint in actionPointList.Value)
                {
                    if (actionPoint.Id == actionPointId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        actionPoint.Position.Y = top;
                        return;
                    }
                }
            }
        }

        public void DeleteActionPoint(Guid actionPointId)
        {
            foreach (KeyValuePair<Guid, List<ActionPointDto>> actionPointList in projectDto_.ActionPoints)
            {
                for (int i = 0; i < actionPointList.Value.Count; i++)
                {
                    if (actionPointList.Value[i].Id == actionPointId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        actionPointList.Value.RemoveAt(i);

                        return;
                    }
                }
            }
        }

        #endregion

        #region Hitbox Functions

        public HitboxDto AddHitbox(Guid ownerId, Guid rootOwnerId)
        {
            saveProjectState();

            HitboxDto newHitbox = new HitboxDto();

            newHitbox.OwnerId = ownerId;

            newHitbox.RootOwnerId = rootOwnerId;

            projectDto_.Hitboxes[ownerId].Add(newHitbox);

            ChangesMade = true;

            return newHitbox;
        }

        public void AddHitbox(HitboxDto hitbox)
        {
            saveProjectState();
            
            projectDto_.Hitboxes[hitbox.OwnerId].Add(hitbox);

            ChangesMade = true;            
        }

        public HitboxDto GetHitbox(Guid hitboxId)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        return hitbox;
                    }
                }
            }

            return null;
        }

        public void SetHitboxRect(Guid hitboxId, Rectangle hitboxRect)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.HitboxRect = hitboxRect;

                        return;
                    }
                }
            }
        }

        public void SetHitboxRectLeft(Guid hitboxId, int left)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.HitboxRect.Left = left;

                        return;
                    }
                }
            }
        }

        public void SetHitboxRectTop(Guid hitboxId, int top)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.HitboxRect.Top = top;

                        return;
                    }
                }
            }
        }

        public void SetHitboxRectWidth(Guid hitboxId, int width)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.HitboxRect.Width = width;

                        return;
                    }
                }
            }
        }

        public void SetHitboxRectHeight(Guid hitboxId, int height)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.HitboxRect.Height = height;

                        return;
                    }
                }
            }
        }

        public void DeleteHitbox(Guid hitboxId)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                for (int i = 0; i < hitboxList.Value.Count; i++)
                {
                    if (hitboxList.Value[i].Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitboxList.Value.RemoveAt(i);

                        return;
                    }
                }
            }
        }

        public void SetHitboxCornerPoint1(Guid ownerId, int hitboxIndex, int x, int y)
        {
            int hitboxCount =  projectDto_.Hitboxes[ownerId].Count;

            if (hitboxIndex >= 0 && hitboxIndex < hitboxCount)
            {
                projectDto_.Hitboxes[ownerId][hitboxIndex].CornerPoint1.X = x;
                projectDto_.Hitboxes[ownerId][hitboxIndex].CornerPoint1.Y = y;
            }
        }

        public void SetHitboxCornerPoint2(Guid ownerId, int hitboxIndex, int x, int y)
        {
            int hitboxCount = projectDto_.Hitboxes[ownerId].Count;

            if (hitboxIndex >= 0 && hitboxIndex < hitboxCount)
            {
                saveProjectState();

                // Take the two corner points and determine the dimensions and position of 
                // the hitbox rect.
                HitboxDto hitbox = projectDto_.Hitboxes[ownerId][hitboxIndex];

                hitbox.CornerPoint2.X = x;
                hitbox.CornerPoint2.Y = y;

                int pointX1 = hitbox.CornerPoint1.X;
                int pointX2 = hitbox.CornerPoint2.X;

                int left = 0;
                int top = 0;
                int right = 0;
                int bottom = 0;

                if (pointX1 > pointX2)
                {
                    left = pointX2;
                    right = pointX1;
                }
                else
                {
                    left = pointX1;
                    right = pointX2;
                }

                int pointY1 = hitbox.CornerPoint1.Y;
                int pointY2 = hitbox.CornerPoint2.Y;

                if (pointY1 > pointY2)
                {
                    bottom = pointY1;
                    top = pointY2;
                }
                else
                {
                    bottom = pointY2;
                    top = pointY1;
                }

                int width = right - left;
                int height = bottom - top;

                Rectangle rect = new Rectangle(left, top, width, height);

                ChangesMade = true;

                projectDto_.Hitboxes[ownerId][hitboxIndex].HitboxRect = rect;
            }
        }
        public void SetHitboxCornerPoints(Guid ownerId, int hitboxIndex, int x1, int y1, int x2, int y2)
        {
            int hitboxCount = projectDto_.Hitboxes[ownerId].Count;

            if (hitboxIndex >= 0 && hitboxIndex < hitboxCount)
            {
                saveProjectState();

                // Take the two corner points and determine the dimensions and position of 
                // the hitbox rect.
                HitboxDto hitbox = projectDto_.Hitboxes[ownerId][hitboxIndex];

                hitbox.CornerPoint1.X = x1;
                hitbox.CornerPoint1.Y = y1;

                hitbox.CornerPoint2.X = x2;
                hitbox.CornerPoint2.Y = y2;

                int pointX1 = hitbox.CornerPoint1.X;
                int pointX2 = hitbox.CornerPoint2.X;

                int left = 0;
                int top = 0;
                int right = 0;
                int bottom = 0;

                if (pointX1 > pointX2)
                {
                    left = pointX2;
                    right = pointX1;
                }
                else
                {
                    left = pointX1;
                    right = pointX2;
                }

                int pointY1 = hitbox.CornerPoint1.Y;
                int pointY2 = hitbox.CornerPoint2.Y;

                if (pointY1 > pointY2)
                {
                    bottom = pointY1;
                    top = pointY2;
                }
                else
                {
                    bottom = pointY2;
                    top = pointY1;
                }

                int width = right - left;
                int height = bottom - top;

                Rectangle rect = new Rectangle(left, top, width, height);

                ChangesMade = true;

                projectDto_.Hitboxes[ownerId][hitboxIndex].HitboxRect = rect;
            }
        }

        public void SetHitboxIdentity(Guid hitboxId, Guid hitboxIdentityId)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.Identity = hitboxIdentityId;

                        return;
                    }
                }
            }
        }
        
        public void SetHitboxIsSolid(Guid hitboxId, bool isSolid)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.IsSolid = isSolid;

                        return;
                    }
                }
            }
        }

        public void SetHitboxPriority(Guid hitboxId, HitboxPriority priority)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.Priority = priority;

                        return;
                    }
                }
            }
        }
        
        public void SetHitboxRotationDegrees(Guid hitboxId, float rotationDegrees)
        {
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Id == hitboxId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        hitbox.RotationDegrees = rotationDegrees;

                        return;
                    }
                }
            }
        }
        
        #endregion

        #region Animation Slot Functions

        public AnimationSlotDto AddAnimationSlot(Guid stateId)
        {
            saveProjectState();

            StateDto ownerState = GetState(stateId);

            AnimationSlotDto newAnimationSlot = new AnimationSlotDto();

            newAnimationSlot.OwnerId = stateId;

            newAnimationSlot.RootOwnerId = ownerState.OwnerId;

            projectDto_.AnimationSlots[stateId].Add(newAnimationSlot);

            ChangesMade = true;

            return newAnimationSlot;
        }

        public AnimationSlotDto GetAnimationSlot(Guid animationSlotId)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        return animationSlot;
                    }
                }
            }

            return null;
        }

        public void DeleteAnimationSlot(Guid animationSlotId)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                for (int i = 0; i < animationSlotList.Value.Count; i++)
                {
                    if (animationSlotList.Value[i].Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlotList.Value.RemoveAt(i);

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotName(Guid animationSlotId, string name)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        // Make sure no other animation slots in the parent state are using this name.
                        Guid ownerId = animationSlot.OwnerId;

                        foreach (AnimationSlotDto animationSlot2 in projectDto_.AnimationSlots[ownerId])
                        {
                            if (animationSlot2.Id != animationSlotId)
                            {
                                if (animationSlot2.Name == name)
                                {
                                    throw new InvalidNameException("Name is already in use.");
                                }
                            }
                        }

                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.Name = name;
                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotAnimation(Guid animationSlotId, Guid animationId)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.Animation = animationId;
                        return;
                    }
                }
            }
        }


        public void SetAnimationSlotOriginLocation(Guid animationSlotId, OriginLocation originLocation)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.OriginLocation = originLocation;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotPositionLeft(Guid animationSlotId, int positionLeft)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.Position.X = positionLeft;
                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotPositionTop(Guid animationSlotId, int positionTop)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.Position.Y = positionTop;
                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotHueColorRed(Guid animationSlotId, float red)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.HueColor.Red = red;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotHueColorGreen(Guid animationSlotId, float green)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.HueColor.Green = green;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotHueColorBlue(Guid animationSlotId, float blue)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.HueColor.Blue = blue;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotHueColorAlpha(Guid animationSlotId, float alpha)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.HueColor.Alpha = alpha;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotBackgroundFlag(Guid animationSlotId, bool backgroundFlag)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.Background = backgroundFlag;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotBlendColorRed(Guid animationSlotId, float red)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.BlendColor.Red = red;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotBlendColorGreen(Guid animationSlotId, float green)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.BlendColor.Green = green;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotBlendColorBlue(Guid animationSlotId, float blue)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.BlendColor.Blue = blue;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotBlendColorAlpha(Guid animationSlotId, float alpha)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.BlendColor.Alpha = alpha;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotBlendColorPercent(Guid animationSlotId, float percent)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.BlendPercent = percent;

                        return;
                    }
                }
            }
        }


        public void SetAnimationSlotNextStateId(Guid animationSlotId, Guid stateId)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.NextStateId = stateId;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotOutlineColorRed(Guid animationSlotId, float red)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.OutlineColor.Red = red;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotOutlineColorGreen(Guid animationSlotId, float green)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.OutlineColor.Green = green;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotOutlineColorBlue(Guid animationSlotId, float blue)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.OutlineColor.Blue = blue;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotOutlineColorAlpha(Guid animationSlotId, float alpha)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.OutlineColor.Alpha = alpha;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotPivotPoint(Guid animationSlotId, Point pivotPoint)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.PivotPoint = pivotPoint;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotAlphaGradientFrom(Guid animationSlotId, float alphaIntensity)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.AlphaGradientFrom = alphaIntensity;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotAlphaGradientTo(Guid animationSlotId, float alphaIntensity)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.AlphaGradientTo = alphaIntensity;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotAlphaGradientRadius(Guid animationSlotId, float radius)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.AlphaGradientRadius = radius;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotAlphaGradientRadialCenter(Guid animationSlotId, Point radialCenterPoint)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.AlphaGradientRadialCenter = radialCenterPoint;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotAlphaGradientDirection(Guid animationSlotId, GradientDirection direction)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.AlphaGradientDirection = direction;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotStyle(Guid animationSlotId, AnimationStyle animationStyle)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.AnimationStyle = animationStyle;

                        return;
                    }
                }
            }
        }

        public void SetAnimationSlotFramesPerSecond(Guid animationSlotId, int framesPerSecond)
        {
            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in projectDto_.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in animationSlotList.Value)
                {
                    if (animationSlot.Id == animationSlotId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        animationSlot.FramesPerSecond = framesPerSecond;

                        animationSlot.UpdateInterval = 1.0f / (float)framesPerSecond;

                        return;
                    }
                }
            }
        }

        public void MoveUpAnimationSlot(Guid stateId, int index)
        {
            if (projectDto_.AnimationSlots.ContainsKey(stateId))
            {
                if (index > 0)
                {
                    saveProjectState();

                    // Swap the slot with the one above it in the list.
                    projectDto_.AnimationSlots[stateId].Reverse(index - 1, 2);

                    ChangesMade = true;
                }
            }
        }

        public void MoveDownAnimationSlot(Guid stateId, int index)
        {
            if (projectDto_.AnimationSlots.ContainsKey(stateId))
            {
                int slotCount = projectDto_.AnimationSlots[stateId].Count;

                if (index < slotCount - 1)
                {
                    saveProjectState();

                    // Swap the slot with the one below it in the list.
                    projectDto_.AnimationSlots[stateId].Reverse(index, 2);

                    ChangesMade = true;
                }
            }
        }

        #endregion

        public HitboxIdentityDto AddHitboxIdentity(string name)
        {
            saveProjectState();

            HitboxIdentityDto hitboxIdentity = new HitboxIdentityDto();

            hitboxIdentity.Name = name;

            projectDto_.HitboxIdentities.Add(hitboxIdentity);
            
            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddHitboxIdentityNames(lstNames);
            
            ChangesMade = true;

            return hitboxIdentity;
        }

        public HitboxIdentityDto GetHitboxIdentity(Guid hitboxIdentityId)
        {
            foreach (HitboxIdentityDto hitboxIdentity in projectDto_.HitboxIdentities)
            {
                if (hitboxIdentity.Id == hitboxIdentityId)
                {
                    return hitboxIdentity;
                }
            }
            
            return null;
        }

        public void DeleteHitboxIdentity(int hitboxIdentityIndex)
        {
            if (hitboxIdentityIndex < 0 || hitboxIdentityIndex >= projectDto_.HitboxIdentities.Count)
            {
                return;
            }

            saveProjectState();

            Guid hitboxIdentityId = projectDto_.HitboxIdentities[hitboxIdentityIndex].Id;

            // Find any hitboxes that are pointing to this hitbox identity and clear it.
            foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in projectDto_.Hitboxes)
            {
                foreach (HitboxDto hitbox in hitboxList.Value)
                {
                    if (hitbox.Identity == hitboxIdentityId)
                    {
                        hitbox.Identity = Guid.Empty;
                    }
                }
            }

            ChangesMade = true;

            nameUtility_.RemoveHitboxIdentityName(projectDto_.HitboxIdentities[hitboxIdentityIndex].Name);

            projectDto_.HitboxIdentities.RemoveAt(hitboxIdentityIndex);
            
            refreshViews();
        }

        public void SetHitboxIdentityName(Guid hitboxIdentityId, string name)
        {
            foreach (HitboxIdentityDto hitboxIdentity in projectDto_.HitboxIdentities)
            {
                if (hitboxIdentity.Id == hitboxIdentityId)
                {
                    saveProjectState();

                    nameUtility_.UpdateHitboxIdentityName(hitboxIdentity.Name, name);
                    
                    ChangesMade = true;

                    hitboxIdentity.Name = name;

                    return;
                }
            }
        }
        
        public int GetHitboxIdentityIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.HitboxIdentities.Count; i++)
            {
                if (projectDto_.HitboxIdentities[i].Id == id)
                {
                    return i;
                }
            }
            
            return -1;
        }

        public Guid GetHitboxIdentityIdFromName(string name)
        {
            foreach (HitboxIdentityDto hitboxIdentity in projectDto_.HitboxIdentities)
            {
                if (hitboxIdentity.Name.ToUpper() == name.ToUpper())
                {
                    return hitboxIdentity.Id;
                }
            }

            return Guid.Empty;
        }

        public TriggerSignalDto AddTriggerSignal(string name)
        {
            saveProjectState();

            TriggerSignalDto triggerSignal = new TriggerSignalDto();

            triggerSignal.Name = name;

            projectDto_.TriggerSignals.Add(triggerSignal);

            List<string> lstNames = new List<string>();

            lstNames.Add(name);

            nameUtility_.AddTriggerSignalNames(lstNames);

            ChangesMade = true;

            return triggerSignal;
        }

        public TriggerSignalDto GetTriggerSignal(Guid triggerSignalId)
        {
            foreach (TriggerSignalDto triggerSignal in projectDto_.TriggerSignals)
            {
                if (triggerSignal.Id == triggerSignalId)
                {
                    return triggerSignal;
                }
            }
            
            return null;
        }

        public void DeleteTriggerSignal(int triggerSignalIndex)
        {
            if (triggerSignalIndex < 0 || triggerSignalIndex >= projectDto_.TriggerSignals.Count)
            {
                return;
            }

            saveProjectState();

            Guid triggerSignalId = projectDto_.TriggerSignals[triggerSignalIndex].Id;

            // Find any frame triggers that are pointing to this trigger signal and clear it.
            foreach (KeyValuePair<Guid, List<FrameTriggerDto>> frameTriggerList in projectDto_.FrameTriggers)
            {
                foreach (FrameTriggerDto frameTrigger in frameTriggerList.Value)
                {
                    if (frameTrigger.Signal == triggerSignalId)
                    {
                        frameTrigger.Signal = Guid.Empty;
                    }
                }
            }

            nameUtility_.RemoveTriggerSignalName(projectDto_.TriggerSignals[triggerSignalIndex].Name);

            projectDto_.TriggerSignals.RemoveAt(triggerSignalIndex);

            ChangesMade = true;

            refreshViews();
        }

        public void SetTriggerSignalName(Guid triggerSignalId, string name)
        {
            foreach (TriggerSignalDto triggerSignal in projectDto_.TriggerSignals)
            {
                if (triggerSignal.Id == triggerSignalId)
                {
                    saveProjectState();

                    nameUtility_.UpdateTriggerSignalName(triggerSignal.Name, name);
                    
                    ChangesMade = true;

                    triggerSignal.Name = name;

                    return;
                }
            }
        }

        public int GetTriggerSignalIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.TriggerSignals.Count; i++)
            {
                if (projectDto_.TriggerSignals[i].Id == id)
                {
                    return i;
                }
            }
            
            return -1;
        }

        public Guid GetTriggerSignalIdFromName(string name)
        {
            foreach (TriggerSignalDto triggerSignal in projectDto_.TriggerSignals)
            {
                if (triggerSignal.Name.ToUpper() == name.ToUpper())
                {
                    return triggerSignal.Id;
                }
            }

            return Guid.Empty;
        }

        public PropertyDto AddProperty(Guid entityId, string name)
        {
            saveProjectState();

            PropertyDto newProperty = new PropertyDto();
            newProperty.Name = name;
            newProperty.DefaultValue = string.Empty;
            newProperty.OwnerId = entityId;
            projectDto_.Properties[entityId].Add(newProperty);

            // Add the property to any map widgets of this entity type.
            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in projectDto_.MapWidgets[mapWidgetType])
                {
                    MapWidgetDto mapWidget = kvp.Value;


                    if (mapWidget is EntityWidgetDto)
                    {
                        EntityWidgetDto entityWidget = (EntityWidgetDto)mapWidget;

                        if (entityWidget.EntityId == entityId)
                        {
                            Guid mapWidgetId = mapWidget.Id;

                            PropertyDto newInstanceProperty = new PropertyDto();
                            newInstanceProperty.Name = newProperty.Name;
                            newInstanceProperty.Value = newProperty.DefaultValue;
                            newInstanceProperty.OwnerId = mapWidgetId;
                            newInstanceProperty.RootOwnerId = newProperty.Id;
                            newInstanceProperty.UpdateValue = true;

                            MapWidgetProperties properties = (MapWidgetProperties)projectDto_.MapWidgetProperties[mapWidgetId];

                            properties.AddProperty(newInstanceProperty);
                        }
                    }
                }
            }

            ChangesMade = true;

            return newProperty;
        }

        public PropertyDto GetProperty(Guid propertyId)
        {
            foreach (KeyValuePair<Guid, List<PropertyDto>> propertyList in projectDto_.Properties)
            {
                foreach (PropertyDto property in propertyList.Value)
                {
                    if (property.Id == propertyId)
                    {
                        return property;
                    }
                }
            }

            return null;
        }

        public void DeleteProperty(Guid propertyId)
        {
            foreach (KeyValuePair<Guid, List<PropertyDto>> propertyList in projectDto_.Properties)
            {
                for (int i = 0; i < propertyList.Value.Count; i++)
                {
                    if (propertyList.Value[i].Id == propertyId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        propertyList.Value.RemoveAt(i);
                    }
                }
            }
        }

        public void SetPropertyName(Guid propertyId, string name)
        {
            foreach (KeyValuePair<Guid, List<PropertyDto>> propertyList in projectDto_.Properties)
            {
                foreach (PropertyDto property in propertyList.Value)
                {
                    if (property.Id == propertyId)
                    {
                        saveProjectState();

                        string oldName = property.Name;

                        // Add the property to any map widgets of this entity type.
                        foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                        {
                            foreach (KeyValuePair<Guid, MapWidgetDto> kvp in projectDto_.MapWidgets[mapWidgetType])
                            {
                                MapWidgetDto mapWidget = kvp.Value;
                                
                                if (mapWidget is EntityWidgetDto)
                                {
                                    EntityWidgetDto entityWidget = (EntityWidgetDto)mapWidget;

                                    if (entityWidget.EntityId == property.OwnerId)
                                    {
                                        foreach (PropertyDto instanceProperty in projectDto_.MapWidgetProperties[entityWidget.Id])
                                        {
                                            if (instanceProperty.RootOwnerId == property.Id)
                                            {
                                                instanceProperty.Name = name;
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        ChangesMade = true;

                        property.Name = name;

                        return;
                    }
                }
            }
        }

        public void SetPropertyDefaultValue(Guid propertyId, string defaultValue)
        {
            foreach (KeyValuePair<Guid, List<PropertyDto>> propertyList in projectDto_.Properties)
            {
                foreach (PropertyDto property in propertyList.Value)
                {
                    if (property.Id == propertyId)
                    {
                        saveProjectState();

                        ChangesMade = true;

                        property.DefaultValue = defaultValue;

                        // If there are any instance properties for this property, check if
                        // UpdateValue is true, and set the property value if it is.
                        foreach (KeyValuePair<Guid, MapWidgetProperties> MapWidgetProperties in projectDto_.MapWidgetProperties)
                        {
                            MapWidgetProperties properties = MapWidgetProperties.Value;

                            foreach (PropertyDto instanceProperty in properties)
                            {
                                if (instanceProperty.RootOwnerId == property.Id)
                                {
                                    if (instanceProperty.UpdateValue == true)
                                    {
                                        instanceProperty.Value = defaultValue;
                                    }
                                }
                            }
                        }

                        return;
                    }
                }
            }
        }

        public GameButtonDto AddGameButton(string name)
        {
            saveProjectState();

            GameButtonDto gameButton = new GameButtonDto();

            gameButton.Name = name;

            projectDto_.GameButtons.Add(gameButton);

            ChangesMade = true;

            return gameButton;
        }

        public GameButtonDto GetGameButton(Guid gameButtonId)
        {
            foreach (GameButtonDto gameButton in projectDto_.GameButtons)
            {
                if (gameButton.Id == gameButtonId)
                {
                    return gameButton;
                }
            }

            return null;
        }

        public void DeleteGameButton(int gameButtonIndex)
        {
            if (gameButtonIndex < 0 || gameButtonIndex >= projectDto_.GameButtons.Count)
            {
                return;
            }

            saveProjectState();

            ChangesMade = true;

            projectDto_.GameButtons.RemoveAt(gameButtonIndex);

            refreshViews();
        }

        public void SetGameButtonName(Guid gameButtonId, string name)
        {
            int gameButtonIndex = GetGameButtonIndexFromId(gameButtonId);

            saveProjectState();

            ChangesMade = true;

            projectDto_.GameButtons[gameButtonIndex].Name = name;
        }

        public void SetGameButtonGroup(Guid gameButtonId, Guid gameButtonGroupId)
        {
            foreach (GameButtonDto gameButton in projectDto_.GameButtons)
            {
                if (gameButton.Id == gameButtonId)
                {
                    saveProjectState();

                    ChangesMade = true;

                    gameButton.Group = gameButtonGroupId;

                    return;
                }
            }
        }

        public void SetGameButtonLabel(Guid gameButtonId, string label)
        {
            int gameButtonIndex = GetGameButtonIndexFromId(gameButtonId);

            saveProjectState();

            ChangesMade = true;

            projectDto_.GameButtons[gameButtonIndex].Label = label;
        }

        public int GetGameButtonIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.GameButtons.Count; i++)
            {
                if (projectDto_.GameButtons[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public GameButtonGroupDto AddGameButtonGroup(string name)
        {
            saveProjectState();

            GameButtonGroupDto gameButtonGroup = new GameButtonGroupDto();

            gameButtonGroup.Name = name;

            projectDto_.GameButtonGroups.Add(gameButtonGroup);

            List<string> lstNames = new List<string>(new string[] { name });

            nameUtility_.AddGameButtonGroupNames(lstNames);
            
            ChangesMade = true;

            return gameButtonGroup;
        }

        public GameButtonGroupDto GetGameButtonGroup(Guid gameButtonGroupId)
        {
            foreach (GameButtonGroupDto gameButtonGroup in projectDto_.GameButtonGroups)
            {
                if (gameButtonGroup.Id == gameButtonGroupId)
                {
                    return gameButtonGroup;
                }
            }

            return null;
        }

        public void DeleteGameButtonGroup(int gameButtonGroupIndex)
        {
            if (gameButtonGroupIndex < 0 || gameButtonGroupIndex >= projectDto_.GameButtonGroups.Count)
            {
                return;
            }

            saveProjectState();

            Guid gameButtonGroupId = projectDto_.GameButtonGroups[gameButtonGroupIndex].Id;

            // Find any game buttons that are pointing to this game button groups and clear it.
            foreach (GameButtonDto gameButton in projectDto_.GameButtons)
            {
                if (gameButton.Group == gameButtonGroupId)
                {
                    gameButton.Group = Guid.Empty;
                }
            }

            ChangesMade = true;

            nameUtility_.RemoveGameButtonGroupName(projectDto_.GameButtonGroups[gameButtonGroupIndex].Name);

            projectDto_.GameButtonGroups.RemoveAt(gameButtonGroupIndex);
            
            refreshViews();
        }

        public void SetGameButtonGroupName(Guid gameButtonGroupId, string name)
        {
            saveProjectState();

            int gameButtonGroupIndex = GetGameButtonGroupIndexFromId(gameButtonGroupId);
            
            nameUtility_.UpdateGameButtonGroupName(projectDto_.GameButtonGroups[gameButtonGroupIndex].Name, name);
            
            ChangesMade = true;

            projectDto_.GameButtonGroups[gameButtonGroupIndex].Name = name;
        }

        public int GetGameButtonGroupIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.GameButtonGroups.Count; i++)
            {
                if (projectDto_.GameButtonGroups[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public Guid GetGameButtonGroupIdFromName(string name)
        {
            foreach (GameButtonGroupDto gameButtonGroup in projectDto_.GameButtonGroups)
            {
                if (gameButtonGroup.Name.ToUpper() == name.ToUpper())
                {
                    return gameButtonGroup.Id;
                }
            }

            return Guid.Empty;
        }
        
        public ScriptDto GetScript(Guid scriptId)
        {
            foreach (KeyValuePair<Guid, ScriptDto> script in projectDto_.Scripts)
            {
                if (script.Value.Id == scriptId)
                {
                    return script.Value;
                }
            }

            return null;
        }

        public ScriptDto GetScriptByOwnerId(Guid ownerId)
        {
            foreach (KeyValuePair<Guid, ScriptDto> script in projectDto_.Scripts)
            {
                if (script.Value.OwnerId == ownerId)
                {
                    return script.Value;
                }
            }

            return null;
        }
        
        public void DeleteScript(Guid scriptId)
        {
            if (projectDto_.Scripts.ContainsKey(scriptId))
            {
                saveProjectState();
                
                projectDto_.Scripts.Remove(scriptId);

                ChangesMade = true;
            }
        }

        public void SetScriptPath(Guid scriptId, string path)
        {
            foreach (KeyValuePair<Guid, ScriptDto> script in projectDto_.Scripts)
            {
                if (script.Value.Id == scriptId)
                {
                    saveProjectState();

                    ChangesMade = true;

                    script.Value.ScriptPath = path;

                    script.Value.ScriptRelativePath = uriUtility_.GetRelativePath(path);

                    return;
                }
            }
        }

        public void SetScriptName(Guid scriptId, string name)
        {
            nameValidator_.ValidateAssetName(scriptId, projectDto_, name);
            
            saveProjectState();

            ChangesMade = true;

            projectDto_.Scripts[scriptId].Name = name;
        }

        public int ReplaceScriptFolder(string oldValue, string newValue)
        {
            int replacementCount = 0;

            // Only save the project state if a change is made.
            bool projectStateSaved = false;

            string find = "\\" + oldValue + "\\";
            string replace = "\\" + newValue + "\\";

            foreach (ScriptDto script in projectDto_.Scripts.Values)
            {
                if (script.ScriptPath.Contains(find) || script.ScriptRelativePath.Contains(find))
                {
                    if (projectStateSaved == false)
                    {
                        ChangesMade = true;

                        saveProjectState();

                        projectStateSaved = true;
                    }

                    script.ScriptPath = script.ScriptPath.Replace(find, replace);

                    script.ScriptRelativePath = script.ScriptRelativePath.Replace(find, replace);

                    replacementCount++;
                }
            }

            return replacementCount;
        }

        public DataFileDto GetDataFile(Guid dataFileId)
        {
            foreach (KeyValuePair<Guid, DataFileDto> dataFile in projectDto_.DataFiles)
            {
                if (dataFile.Value.Id == dataFileId)
                {
                    return dataFile.Value;
                }
            }

            return null;
        }

        public void SetDataFilePath(Guid dataFileId, string path)
        {
            foreach (KeyValuePair<Guid, DataFileDto> dataFile in projectDto_.DataFiles)
            {
                if (dataFile.Value.Id == dataFileId)
                {
                    saveProjectState();

                    ChangesMade = true;

                    dataFile.Value.FilePath = path;

                    return;
                }
            }
        }

        // Removed in 2.1
        //public int GetMenuBookIndexFromId(Guid id)
        //{
        //    for (int i = 0; i < projectDto_.MenuBooks.Count; i++)
        //    {
        //        if (projectDto_.MenuBooks[i].Id == id)
        //        {
        //            return i;
        //        }
        //    }

        //    return -1;
        //}

        //public int GetMenuPageIndexFromId(Guid id)
        //{
        //    for (int i = 0; i < projectDto_.MenuPages.Count; i++)
        //    {
        //        if (projectDto_.MenuPages[i].Id == id)
        //        {
        //            return i;
        //        }
        //    }

        //    return -1;
        //}

        public int GetUiWidgetIndexFromId(Guid id)
        {
            for (int i = 0; i < projectDto_.UiWidgets.Count; i++)
            {
                if (projectDto_.UiWidgets[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool ChangesMade
        {
            get { return isDataChanged_; }
            set { isDataChanged_ = value; }
        }

        public int GetUndoStackSize()
        {
            return undoStack_.Count;
        }

        public int GetRedoStackSize()
        {
            return redoStack_.Count;
        }
        
        public void Undo()
        {
            // Push the current project state onto the redo stack.
            byte[] currentProjectState = getCompressedProjectState();

            System.Diagnostics.Debug.Print("Adding to redo stack");

            redoStack_.Push(currentProjectState);
            
            // Get the previous project state from the undo stack, and replace the current
            // project state with it.
            byte[] compressedProjectState = undoStack_.Pop();

            byte[] projectState = decompressProjectState(compressedProjectState);

            ProjectDto project = null;

            using (MemoryStream stream = new MemoryStream(projectState))
            {
                project = ReadProjectDtoFromStream(stream);
            }

            initializeControllers(project);

            // The project is always going to be prepared when doing an undo or redo operation, because it is synchronous.
            project.IsPrepared = true;

            // Build a new UI state object for this project.
            ProjectUiStateDto uiState = generateUiStateFromProject(project);

            // Replace the current uiState object with the new one, preserving
            // existing state data.
            replaceUiState(uiState);

            // Replace the current project dto with the restored project dto.
            projectDto_ = project;
            
            GC.Collect();

            // Perform any UI state validation based on the action type.            
            validateUiState();

            // Remove this and move the handler logic into the state changed event.
            OnProjectCreated(new ProjectCreatedEventArgs(false));

            OnProjectStateChanged(new ProjectStateChangedEventArgs());

            refreshViews();

            ChangesMade = true;        
        }

        public void Redo()
        {
            // Push the current project state onto the undo stack.
            byte[] currentProjectState = getCompressedProjectState();
            
            System.Diagnostics.Debug.Print("Adding to undo stack");

            undoStack_.Push(currentProjectState);

            // Get the previous project state from the redo stack, and replace the current
            // project state with it.
            byte[] compressedProjectState = redoStack_.Pop();

            byte[] projectState = decompressProjectState(compressedProjectState);

            ProjectDto project = null;

            using (MemoryStream stream = new MemoryStream(projectState))
            {
                project = ReadProjectDtoFromStream(stream);
            }

            initializeControllers(project);

            // The project is always going to be prepared when doing an undo or redo operation, because it is synchronous.
            project.IsPrepared = true;

            // Build a new UI state object for this project.
            ProjectUiStateDto uiState = generateUiStateFromProject(project);

            // Replace the current uiState object with the new one, preserving
            // existing state data.
            replaceUiState(uiState);

            // Replace the current project dto with the restored project dto.
            projectDto_ = project;
            
            GC.Collect();

            // Perform any UI state validation based on the action type.            
            validateUiState();

            OnProjectCreated(new ProjectCreatedEventArgs(false));

            OnProjectStateChanged(new ProjectStateChangedEventArgs());

            refreshViews();

            ChangesMade = true;
        }
        
        private EntityDto getEntity(Guid entityId, ProjectDto project)
        {
            foreach (ActorDto actor in project.Actors)
            {
                if (actor.Id == entityId)
                {
                    return actor;
                }
            }

            foreach (EventDto eventEntity in project.Events)
            {
                if (eventEntity.Id == entityId)
                {
                    return eventEntity;
                }
            }

            foreach (HudElementDto hudElement in project.HudElements)
            {
                if (hudElement.Id == entityId)
                {
                    return hudElement;
                }
            }

            return null;
        }
        
        public UiWidgetDto AddUiWidget(string name)
        {
            saveProjectState();

            UiWidgetDto newUiWidget = new UiWidgetDto();
            newUiWidget.Name = name;
            projectDto_.UiWidgets.Add(newUiWidget);

            ScriptDto script = new ScriptDto();
            script.ScriptType = ScriptType.UiWidget;
            script.OwnerId = newUiWidget.Id;
            script.Name = "UiWidget" + newUiWidget.Name;
            projectDto_.Scripts[newUiWidget.Id] = script;

            ChangesMade = true;

            return newUiWidget;
        }

        public UiWidgetDto GetUiWidget(Guid uiWidgetId)
        {
            foreach (UiWidgetDto uiWidget in projectDto_.UiWidgets)
            {
                if (uiWidget.Id == uiWidgetId)
                {
                    return uiWidget;
                }
            }

            return null;
        }

        public void DeleteUiWidget(int uiWidgetIndex)
        {
            if (uiWidgetIndex < 0 || uiWidgetIndex >= projectDto_.UiWidgets.Count)
            {
                return;
            }

            saveProjectState();

            projectDto_.Scripts.Remove(projectDto_.UiWidgets[uiWidgetIndex].Id);

            projectDto_.UiWidgets.RemoveAt(uiWidgetIndex);

            ChangesMade = true;

            refreshViews();
        }

        public void SetUiWidgetName(Guid uiWidgetId, string name)
        {
            int uiWidgetIndex = GetUiWidgetIndexFromId(uiWidgetId);

            nameValidator_.ValidateAssetName(uiWidgetId, projectDto_, name);
            
            saveProjectState();

            ChangesMade = true;

            projectDto_.UiWidgets[uiWidgetIndex].Name = name;

            projectDto_.Scripts[uiWidgetId].Name = "UiWidget" + name;
        }

        public bool IsUiWidget(Guid id)
        {
            foreach (UiWidgetDto uiWidget in projectDto_.UiWidgets)
            {
                if (uiWidget.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public ScriptDto AddScript(string name, ScriptType scriptType)
        {
            saveProjectState();

            ScriptDto newScript = new ScriptDto();
            newScript.Name = name;
            newScript.ScriptType = scriptType;
            newScript.OwnerId = newScript.Id;
            newScript.RootOwnerId = newScript.Id;

            projectDto_.Scripts.Add(newScript.Id, newScript);

            ChangesMade = true;

            return newScript;
        }

        public void ClearMapWidgetSelection(int roomIndex)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                MapWidgetSelectorDto selector = projectUiState_.MapWidgetSelector[roomId];

                foreach (Guid mapWidgetId in selector.SelectedMapWidgetIds)
                {
                    projectUiState_.MapWidgetSelected[mapWidgetId] = false;
                }

                selector.SelectedMapWidgetIds.Clear();

                OnSelectionToggle(new SelectionToggleEventArgs(false, false));

                OnMapWidgetSelectionChange(new MapWidgetSelectionChangedEventArgs(roomIndex, false, null));
                
                refreshViews();
            }
        }

        public void AddMapWidgetsToSelection(int roomIndex, List<Guid> mapWidgetIds)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                bool selectionChanged = false;

                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                MapWidgetSelectorDto selector = projectUiState_.MapWidgetSelector[roomId];

                List<Guid> lstFinalSelectionIds = new List<Guid>();

                foreach (Guid mapWidgetId in mapWidgetIds)
                {
                    // If this map widget is already seleted, don't do anything.
                    if (projectUiState_.MapWidgetSelected[mapWidgetId] == false)
                    {
                        MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);

                        // Check if this map widget type can be selected.
                        if (projectUiState_.CanSelectMapWidget[mapWidget.Type] == true)
                        {
                            // If this map widget isn't owned by the room requested, it should be ignored.
                            if (mapWidget.RootOwnerId == roomId)
                            {
                                lstFinalSelectionIds.Add(mapWidgetId);

                                selector.SelectedMapWidgetIds.Add(mapWidgetId);

                                projectUiState_.MapWidgetSelected[mapWidgetId] = true;

                                selectionChanged = true;
                            }
                        }
                    }
                }

                if (selectionChanged == true)
                {
                    OnMapWidgetSelectionChange(new MapWidgetSelectionChangedEventArgs(roomIndex, true, lstFinalSelectionIds));

                    OnSelectionToggle(new SelectionToggleEventArgs(true, false));

                    refreshViews();
                }
            }
        }

        public void RemoveMapWidgetsFromSelection(int roomIndex, List<Guid> mapWidgetIds)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                bool selectionChanged = false;

                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                MapWidgetSelectorDto selector = projectUiState_.MapWidgetSelector[roomId];

                List<Guid> lstFinalSelectionIds = new List<Guid>();

                foreach (Guid mapWidgetId in mapWidgetIds)
                {
                    // If this spawn point instance is already deseleted, don't do anything.
                    if (projectUiState_.MapWidgetSelected[mapWidgetId] == true)
                    {
                        lstFinalSelectionIds.Add(mapWidgetId);

                        MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);

                        // If this map widget isn't owned by the room requested, it should be ignored.
                        if (mapWidget.RootOwnerId == roomId)
                        {
                            selector.SelectedMapWidgetIds.Remove(mapWidgetId);

                            projectUiState_.MapWidgetSelected[mapWidgetId] = false;

                            selectionChanged = true;
                        }
                    }
                }

                if (selectionChanged == true)
                {
                    if (selector.SelectedMapWidgetIds.Count == 0)
                    {
                        OnSelectionToggle(new SelectionToggleEventArgs(false, false));
                    }

                    OnMapWidgetSelectionChange(new MapWidgetSelectionChangedEventArgs(roomIndex, false, lstFinalSelectionIds));

                    refreshViews();
                }
            }
        }

        public void SetMapWidgetSelectorCorner1(int roomIndex, int x, int y)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                if (projectUiState_.MapWidgetSelector[roomId].SelectedLayerId != Guid.Empty)
                {
                    projectUiState_.MapWidgetSelector[roomId].SelectionCorner1.X = x;
                    projectUiState_.MapWidgetSelector[roomId].SelectionCorner1.Y = y;
                    projectUiState_.MapWidgetSelector[roomId].SelectionCorner2.X = x;
                    projectUiState_.MapWidgetSelector[roomId].SelectionCorner2.Y = y;

                    projectUiState_.MapWidgetSelector[roomId].SelectionLeft = x;
                    projectUiState_.MapWidgetSelector[roomId].SelectionTop = y;
                    projectUiState_.MapWidgetSelector[roomId].SelectionRight = x;
                    projectUiState_.MapWidgetSelector[roomId].SelectionBottom = y;

                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x, y, 0, 0);

                    projectUiState_.MapWidgetSelector[roomId].DrawableRect = rect;
                }
            }
        }

        public void SetMapWidgetSelectorCorner2(int roomIndex, int x, int y)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                if (projectUiState_.MapWidgetSelector[roomId].SelectedLayerId != Guid.Empty)
                {
                    int tileSize = projectDto_.TileSize;

                    projectUiState_.MapWidgetSelector[roomId].SelectionCorner2.X = x;
                    projectUiState_.MapWidgetSelector[roomId].SelectionCorner2.Y = y;

                    // Take the two corner points and determine the dimensions and position of 
                    // the drawable rect.
                    int x1 = projectUiState_.MapWidgetSelector[roomId].SelectionCorner1.X;
                    int x2 = projectUiState_.MapWidgetSelector[roomId].SelectionCorner2.X;

                    if (x1 > x2)
                    {
                        projectUiState_.MapWidgetSelector[roomId].SelectionLeft = x2;
                        projectUiState_.MapWidgetSelector[roomId].SelectionRight = x1;
                    }
                    else
                    {
                        projectUiState_.MapWidgetSelector[roomId].SelectionLeft = x1;
                        projectUiState_.MapWidgetSelector[roomId].SelectionRight = x2;
                    }

                    int y1 = projectUiState_.MapWidgetSelector[roomId].SelectionCorner1.Y;
                    int y2 = projectUiState_.MapWidgetSelector[roomId].SelectionCorner2.Y;

                    if (y1 > y2)
                    {
                        projectUiState_.MapWidgetSelector[roomId].SelectionBottom = y1;
                        projectUiState_.MapWidgetSelector[roomId].SelectionTop = y2;
                    }
                    else
                    {
                        projectUiState_.MapWidgetSelector[roomId].SelectionBottom = y2;
                        projectUiState_.MapWidgetSelector[roomId].SelectionTop = y1;
                    }

                    int left = projectUiState_.MapWidgetSelector[roomId].SelectionLeft;
                    int top = projectUiState_.MapWidgetSelector[roomId].SelectionTop;
                    int right = projectUiState_.MapWidgetSelector[roomId].SelectionRight;
                    int bottom = projectUiState_.MapWidgetSelector[roomId].SelectionBottom;

                    int width = right - left;
                    int height = bottom - top;

                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, width, height);

                    projectUiState_.MapWidgetSelector[roomId].DrawableRect = rect;

                    refreshViews();
                }
            }
        }

        public void SetMapWidgetSelectionOn(int roomIndex, bool selectionOn)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                projectUiState_.MapWidgetSelector[roomId].IsSelectionOn = selectionOn;
            }
        }

        public void SetMapWidgetSelectionLayer(int roomIndex, int layerIndex)
        {
            if (roomIndex >= 0 && roomIndex < projectDto_.Rooms.Count)
            {
                Guid roomId = projectDto_.Rooms[roomIndex].Id;

                if (layerIndex >= 0 && layerIndex < projectDto_.Layers[roomId].Count)
                {
                    LayerDto layer = projectDto_.Layers[roomId][layerIndex];

                    projectUiState_.MapWidgetSelector[roomId].SelectedLayerId = layer.Id;
                }
            }
        }

        public void SetProjectFolder(string projectFolder)
        {
            if (projectDto_.ProjectFolderFullPath != projectFolder)
            {
                saveProjectState();

                projectDto_.ProjectFolderFullPath = projectFolder;
                ChangesMade = true;
            }
        }

        public void SetInitialRoomId(Guid initialRoomId)
        {
            if (projectDto_.InitialRoomId != initialRoomId)
            {
                saveProjectState();

                projectDto_.InitialRoomId = initialRoomId;

                ChangesMade = true;
            }
        }

        public void SetCameraMode(CameraMode cameraMode)
        {
            projectUiState_.CameraMode = cameraMode;

            OnCameraModeChanged(new CameraModeChangedEventArgs(cameraMode));
        }

        public void SetCanSelectMapWidget(MapWidgetType mapWidgetType, bool canSelect)
        {
            projectUiState_.CanSelectMapWidget[mapWidgetType] = canSelect;
        }

        public void SetEditMode(EditMode editMode)
        {
            projectUiState_.EditMode = editMode;

            OnEditModeChanged(new EditModeChangedEventArgs(editMode));
        }
                
        public void SetMapWidgetMode(int roomIndex, MapWidgetMode mapWidgetMode)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            projectUiState_.MapWidgetMode[roomId] = mapWidgetMode;

            refreshViews();
        }

        public void SetSelectedMapWidgetType(int roomIndex, MapWidgetType mapWidgetType)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            projectUiState_.SelectedMapWidgetType[roomId] = mapWidgetType;

            refreshViews();
        }

        public void SetSelectedTileIndex(int roomIndex, int tileIndex)
        {
            if (roomIndex < 0 || roomIndex >= projectDto_.Rooms.Count)
            {
                return;
            }

            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            projectUiState_.SelectedTileIndex[roomId] = tileIndex;

            refreshViews();
        }

        public void SetShowCameraOutline(bool value)
        {
            projectUiState_.ShowCameraOutline = value;

            OnRefreshView(new RefreshViewEventArgs());
        }

        public void SetShowGrid(bool value)
        {
            projectUiState_.ShowGrid = value;

            OnRefreshView(new RefreshViewEventArgs()); 
        }

        public void SetShowOutlines(bool value)
        {
            projectUiState_.ShowOutlines = value;

            OnRefreshView(new RefreshViewEventArgs());
        }

        public void ShowWorldGeometry(bool value)
        {
            projectUiState_.ShowWorldGeometry = value;

            OnRefreshView(new RefreshViewEventArgs());
        }
        
        public void ToggleGrid()
        {
            projectUiState_.ShowGrid = !projectUiState_.ShowGrid;
        }

        public void ToggleCameraOutline()
        {
            projectUiState_.ShowCameraOutline = !projectUiState_.ShowCameraOutline;
        }

        public void ToggleOutlines()
        {
            projectUiState_.ShowOutlines = !projectUiState_.ShowOutlines;
        }

        public void ToggleTransparentSelect()
        {
            projectUiState_.TransparentSelect = !projectUiState_.TransparentSelect;
        }

        public void ToggleMouse()
        {
            projectUiState_.ShowMouseOver = !projectUiState_.ShowMouseOver;
        }

        public void HideMouse()
        {
            projectUiState_.ShowMouseOver = false;
        }

        public void ShowMouse()
        {
            projectUiState_.ShowMouseOver = true;
        }


        public BitmapResourceDto GetBitmapResource(Guid resourceId, bool loadFromDisk)
        {
            // Separate resources dto removed in 2.2.
            //foreach (KeyValuePair<Guid, BitmapResourceDto> kvp in projectResources_.Bitmaps)

            if (projectDto_.Bitmaps.ContainsKey(resourceId))
            {
                BitmapResourceDto bitmapResource = projectDto_.Bitmaps[resourceId];

                if (loadFromDisk == true)
                {
                    LoadBitmap(resourceId);
                }

                return bitmapResource;
            }

            return null;
        }

        public void LoadAudio(Guid resourceId)
        {
            if (projectDto_.AudioData.ContainsKey(resourceId) == true)
            {
                AudioResourceDto audioResourceToLoad = projectDto_.AudioData[resourceId];

                if (audioResourceToLoad.AudioData == null)
                {
                    if (File.Exists(audioResourceToLoad.Path) == true)
                    {
                        audioResourceToLoad.AudioData = File.ReadAllBytes(audioResourceToLoad.Path);

                        audioResourceToLoad.Audio = new OggFile(audioResourceToLoad.Path);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Could not load audio resource: " + audioResourceToLoad.Path);
                    }                    
                }
            }
        }

        public void LoadBitmap(Guid resourceId)
        {
            if (projectDto_.Bitmaps.ContainsKey(resourceId) == true)
            {
                BitmapResourceDto bitmapResourceToLoad = projectDto_.Bitmaps[resourceId];

                if (bitmapResourceToLoad.BitmapImage == null)
                {
                    if (File.Exists(bitmapResourceToLoad.Path) == true)
                    {
                        bitmapResourceToLoad.BitmapImage = new Bitmap(bitmapResourceToLoad.Path);

                        Bitmap bitmapWithTransparency = bitmapUtility_.ApplyTransparency(bitmapResourceToLoad.BitmapImage);

                        bitmapResourceToLoad.BitmapImageWithTransparency = bitmapWithTransparency;

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Could not load bitmap resource: " + bitmapResourceToLoad.Path);
                    }
                    
                    // If this resource is associated with a sprite sheet or a tile sheet, populate the image cells list.
                    if (bitmapResourceToLoad.OwnerId != Guid.Empty)
                    {
                        int spriteSheetIndex = GetSpriteSheetIndexFromId(bitmapResourceToLoad.OwnerId);

                        if (spriteSheetIndex >= 0)
                        {
                            generateSpriteSheetImages(spriteSheetIndex);
                        }
                        else
                        {
                            int tileSheetIndex = GetTileSheetIndexFromId(bitmapResourceToLoad.OwnerId);

                            if (tileSheetIndex >= 0)
                            {
                                generateTileSheetImages(tileSheetIndex);
                            }
                        }
                    }
                }
            }
        }

        public void UnloadAudioResource(Guid resourceId)
        {
            if (projectDto_.AudioData.ContainsKey(resourceId) == true)
            {
                AudioResourceDto audioResource = projectDto_.AudioData[resourceId];
                
                if (audioResource.Audio != null)
                {
                    audioResource.Audio.Dispose();

                    audioResource.Audio = null;
                    
                    audioResource.AudioData = null;

                    GC.Collect();
                }
            }
        }

        public void UnloadBitmapResource(Guid resourceId, EditorModule module)
        {
            if (projectDto_.Bitmaps.ContainsKey(resourceId) == true)
            {
                BitmapResourceDto bitmapResource = projectDto_.Bitmaps[resourceId];

                bitmapResource.LoadedModules = (byte)(bitmapResource.LoadedModules & (byte)~module);

                if (bitmapResource.LoadedModules == 0)
                {
                    // No longer used by any modules.
                    if (bitmapResource.BitmapImage != null)
                    {
                        bitmapResource.BitmapImage.Dispose();

                        bitmapResource.BitmapImage = null;

                        bitmapResource.BitmapImageWithTransparency.Dispose();

                        bitmapResource.BitmapImageWithTransparency = null;

                        bitmapResource.SpriteSheetImageList.Clear();

                        GC.Collect();
                    }                    
                }
            }
        }

        public int ReplaceResourceFolder(string oldValue, string newValue)
        {
            int replacementCount = 0;

            // Only save the project state if a change is made.
            bool projectStateSaved = false;

            string find = "\\" + oldValue + "\\";
            string replace = "\\" + newValue + "\\";

            foreach (BitmapResourceDto resource in projectDto_.Bitmaps.Values)
            {
                if (resource.Path.Contains(find) || resource.RelativePath.Contains(find))
                {
                    if (projectStateSaved == false)
                    {
                        ChangesMade = true;

                        saveProjectState();

                        projectStateSaved = true;
                    }

                    resource.Path = resource.Path.Replace(find, replace);

                    resource.RelativePath = resource.RelativePath.Replace(find, replace);

                    replacementCount++;
                }
            }

            foreach (AudioResourceDto resource in projectDto_.AudioData.Values)
            {
                if (resource.Path.Contains(find) || resource.RelativePath.Contains(find))
                {
                    if (projectStateSaved == false)
                    {
                        ChangesMade = true;

                        saveProjectState();

                        projectStateSaved = true;
                    }

                    resource.Path = resource.Path.Replace(find, replace);

                    resource.RelativePath = resource.RelativePath.Replace(find, replace);

                    replacementCount++;
                }
            }

            return replacementCount;
        }

        #endregion

        #region Event Dispatchers

        protected virtual void OnBeforeRoomDeleted(BeforeRoomDeletedEventArgs e)
        {
            BeforeRoomDeleted?.Invoke(this, e);
        }

        protected virtual void OnProjectCreated(ProjectCreatedEventArgs e)
        {
            ProjectCreated?.Invoke(this, e);
        }

        protected virtual void OnProjectStateChanged(ProjectStateChangedEventArgs e)
        {
            ProjectStateChanged?.Invoke(this, e);
        }

        protected virtual void OnRoomAdded(RoomAddedEventArgs e)
        {
            RoomAdded?.Invoke(this, e);
        }

        protected virtual void OnRoomSelected(RoomSelectedEventArgs e)
        {
            RoomSelected?.Invoke(this, e);
        }

        protected virtual void OnLayerSelect(LayerSelectedEventArgs e)
        {
            LayerSelect?.Invoke(this, e);
        }

        protected virtual void OnLayerAdded(LayerAddedEventArgs e)
        {
            LayerAdd?.Invoke(this, e);
        }

        protected virtual void OnLayerResized(LayerResizedEventArgs e)
        {
            LayerResize?.Invoke(this, e);
        }

        protected virtual void OnBeforeLayerDeleted(BeforeLayerDeletedEventArgs e)
        {
            BeforeLayerDelete?.Invoke(this, e);
        }

        protected virtual void OnAfterLayerDeleted(AfterLayerDeletedEventArgs e)
        {
            AfterLayerDelete?.Invoke(this, e);
        }

        protected virtual void OnInteractiveLayerChanged(InteractiveLayerChangedEventArgs e)
        {
            InteractiveLayerChange?.Invoke(this, e);
        }
        
        private void OnCameraModeChanged(CameraModeChangedEventArgs e)
        {
            CameraModeChanged(this, e);
        }

        private void OnEditModeChanged(EditModeChangedEventArgs e)
        {
            EditModeChanged(this, e);
        }

        private void OnSelectionToggle(SelectionToggleEventArgs e)
        {
            SelectionToggle(this, e);
        }

        private void OnRefreshView(RefreshViewEventArgs e)
        {
            RefreshView?.Invoke(this, e);
        }

        private void OnRefreshProperties(RefreshPropertiesEventArgs e)
        {
            RefreshProperties?.Invoke(this, e);
        }

        private void OnTileObjectSelected(TileObjectSelectedEventArgs e)
        {
            TileObjectSelected?.Invoke(this, e);
        }

        private void OnActorAdd(ActorAddedEventArgs e)
        {
            ActorAdd?.Invoke(this, e);
        }

        private void OnEventAdd(EventAddedEventArgs e)
        {
            EventAdd?.Invoke(this, e);
        }

        private void OnHudElementAdd(HudElementAddedEventArgs e)
        {
            HudElementAdd?.Invoke(this, e);
        }

        private void OnSpawnPointAdd(SpawnPointAddedEventArgs e)
        {
            SpawnPointAdd?.Invoke(this, e);
        }

        private void OnTileObjectAdd(TileObjectAddedEventArgs e)
        {
            TileObjectAdd?.Invoke(this, e);
        }

        private void OnTileObjectDeleted(TileObjectDeletedEventArgs e)
        {
            TileObjectDelete?.Invoke(this, e);
        }

        private void OnTileObjectNameChanged(TileObjectNameChangedEventArgs e)
        {
            TileObjectNameChange?.Invoke(this, e);
        }
        
        private void OnBeforeMapWidgetAdd(BeforeMapWidgetAddedEventArgs e)
        {
            BeforeMapWidgetAdd?.Invoke(this, e);
        }

        private void OnMapWidgetAdd(MapWidgetAddedEventArgs e)
        {
            MapWidgetAdd?.Invoke(this, e);
        }
               
        private void OnMapWidgetSelectionChange(MapWidgetSelectionChangedEventArgs e)
        {
            MapWidgetSelectionChange?.Invoke(this, e);
        }

        
        private void OnBeforeMapWidgetDelete(BeforeMapWidgetDeleteEventArgs e)
        {
            BeforeMapWidgetDelete?.Invoke(this, e);
        }
        #endregion

        #region Private Functions

        private RoomDto addRoom(string roomName, string initialLayerName, int initialLayerColumns, int initialLayerRows)
        {
            RoomDto newRoom = new RoomDto();

            newRoom.Name = roomName;

            projectDto_.Rooms.Add(newRoom);
            projectDto_.Layers.Add(newRoom.Id, new List<LayerDto>());
            // Removed in 2.1
            //projectDto_.Tilesets.Add(newRoom.Id, new TilesetDto());
            projectDto_.InteractiveLayerIndexes.Add(newRoom.Id, 0);

            // Initialize the layer data for the room.
            projectUiState_.SelectedLayerIndex.Add(newRoom.Id, 0);
            projectUiState_.SelectedTileIndex.Add(newRoom.Id, -1);
            projectUiState_.LayerOrdinalToIndexMap.Add(newRoom.Id, new List<int>());

            MapWidgetSelectorDto mapWidgetSelector = new MapWidgetSelectorDto();
            mapWidgetSelector.FillColor = Globals.actorFillColor;
            mapWidgetSelector.OutlineColor = Globals.actorOutlineColor;
            projectUiState_.MapWidgetSelector.Add(newRoom.Id, mapWidgetSelector);
            
            projectUiState_.MapWidgetMode.Add(newRoom.Id, MapWidgetMode.Actor);
            projectUiState_.SelectedMapWidgetType.Add(newRoom.Id, MapWidgetType.SpawnPoint);
            projectUiState_.CameraLocation.Add(newRoom.Id, new Point2D(0, 0));
            projectUiState_.CameraLocationMax.Add(newRoom.Id, new Point2D(0, 0));
            projectUiState_.CanvasOffset.Add(newRoom.Id, new Point2D(0, 0));
            projectUiState_.CanvasOffsetMax.Add(newRoom.Id, new Point2D(0, 0));
            projectUiState_.MaxCols.Add(newRoom.Id, initialLayerColumns);
            projectUiState_.MaxRows.Add(newRoom.Id, initialLayerRows);
            
            Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>> mapWidgetsByType = new Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>>();

            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                mapWidgetsByType.Add(mapWidgetType, new SortableBindingList<MapWidgetDto>());
            }
            
            projectUiState_.RoomMapWidgetsByType.Add(newRoom.Id, mapWidgetsByType);

            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                projectUiState_.RoomMapWidgetsByType[newRoom.Id][mapWidgetType].Comparer = new MapWidgetComparer();
            }
            
            SortableBindingList<MapWidgetDto> mapWidgets = new SortableBindingList<MapWidgetDto>();
            projectUiState_.RoomMapWidgets.Add(newRoom.Id, mapWidgets);
            projectUiState_.RoomMapWidgets[newRoom.Id].Comparer = new MapWidgetComparer();

            addLayer(projectDto_.Rooms.Count - 1, initialLayerName, initialLayerColumns, initialLayerRows);

            ChangesMade = true;

            return newRoom;
        }
        
        private void initializeControllers(ProjectDto project)
        {
            // Initialize all widget controllers. 
            // Loop through the dictionary of map widgets of each widget type.
            foreach (KeyValuePair<MapWidgetType, Dictionary<Guid, MapWidgetDto>> kvp in project.MapWidgets)
            {
                Dictionary<Guid, MapWidgetDto> mapWidgets = kvp.Value;

                // Loop through each map widget of this type.
                foreach (KeyValuePair<Guid, MapWidgetDto> kvpOfType in mapWidgets)
                {
                    MapWidgetDto mapWidget = kvpOfType.Value;

                    mapWidget.Controller.Initialize();                    
                }
            }
        }

        private SceneryAnimationDto getSceneryAnimation(Guid sceneryAnimationId, ProjectDto project)
        {
            foreach (KeyValuePair<Guid, List<SceneryAnimationDto>> kvp in project.SceneryAnimations)
            {
                foreach (SceneryAnimationDto sceneryAnimation in kvp.Value)
                {
                    if (sceneryAnimation.Id == sceneryAnimationId)
                    {
                        return sceneryAnimation;
                    }
                }
            }

            return null;
        }

        private TileObjectDto getTileObject(Guid tileObjectId, ProjectDto project)
        {
            foreach (KeyValuePair<Guid, List<TileObjectDto>> kvp in project.TileObjects)
            {
                foreach (TileObjectDto tileObject in kvp.Value)
                {
                    if (tileObject.Id == tileObjectId)
                    {
                        return tileObject;
                    }
                }
            }

            return null;
        }

        private bool isValidProject(ProjectDto project)
        {
            // Projects must have at least one room.
            if (project.Rooms.Count < 1)
            {
                return false;
            }

            // Projects can't have a tile size less than 8.
            if (project.TileSize < Globals.minimumTileSize)
            {
                return false;
            }

            // Camera size must be at least minimum allowed.          
            if (project.CameraHeight < Globals.minimumCameraHeight ||
                project.CameraWidth < Globals.minimumCameraWidth)
            {
                return false;
            }

            // Each room must have at least one layer.
            foreach (RoomDto room in project.Rooms)
            {
                if (project.Layers.ContainsKey(room.Id))
                {
                    if (project.Layers[room.Id].Count < 1)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private LayerDto addLayer(int roomIndex, string layerName, int layerColumns, int layerRows)
        {
            Guid roomId = projectDto_.Rooms[roomIndex].Id;

            LayerDto newLayer = new LayerDto(layerName, layerColumns, layerRows);

            newLayer.OwnerId = roomId;
            newLayer.RootOwnerId = roomId;

            projectDto_.MapWidgetsByLayer.Add(newLayer.Id, new Dictionary<Guid, MapWidgetDto>());

            // If this layer's rows or columns are greater than the current max, set the new value.
            if (layerColumns > projectUiState_.MaxCols[roomId])
            {
                projectUiState_.MaxCols[roomId] = layerColumns;
            }

            if (layerRows > projectUiState_.MaxRows[roomId])
            {
                projectUiState_.MaxRows[roomId] = layerRows;
            }

            // Add an entry to the ordinal/index maps for this layer.
            projectUiState_.LayerOrdinalToIndexMap[roomId].Add(projectDto_.Layers[roomId].Count);

            // Initialize the layer visibility.
            projectUiState_.LayerVisible.Add(newLayer.Id, true);

            projectDto_.Layers[roomId].Add(newLayer);

            ChangesMade = true;

            return newLayer;
        }

        private void resizeRows(Guid roomId, int layerIndex, int rows)
        {
            //findmeupdate  Need to update this when I get a chance. For now do nothing.


            //// Build a list of entities who are in grid cells which will be affected. 
            //// They will need to be removed from the grid, and re-added, because the grid is changing.

            //// When reducing the number of rows, every actor/event/widget from the new last row 
            //// to the old last row could potentially need re-added.

            //// When increasing the number of rows, every actor/event/widget in the last row will
            //// potentially need to be re-added.

            ////findme123
            //HashSet<EntityInstanceDto> lstActorsToRefreshInGrid = new HashSet<EntityInstanceDto>();
            //HashSet<EntityInstanceDto> lstEventsToRefreshInGrid = new HashSet<EntityInstanceDto>();
            //HashSet<MapWidgetDto> lstMapWidgetsToRefreshInGrid = new HashSet<MapWidgetDto>();

            //if (rows < projectDto_.Layers[roomId][layerIndex].Rows)
            //{
            //    for (int r = rows - 1; r < projectDto_.Layers[roomId][layerIndex].Rows; r++)
            //    {
            //        for (int c = 0; c < projectDto_.Layers[roomId][layerIndex].Cols; c++)
            //        {
            //            foreach (Guid actorId in projectDto_.Layers[roomId][layerIndex].ActorIds[r][c])
            //            {
            //                EntityInstanceDto actorInstance = GetEntityInstance(actorId);
            //                lstActorsToRefreshInGrid.Add(actorInstance);
            //            }

            //            foreach (Guid eventId in projectDto_.Layers[roomId][layerIndex].EventIds[r][c])
            //            {
            //                EntityInstanceDto eventInstance = GetEntityInstance(eventId);
            //                lstEventsToRefreshInGrid.Add(eventInstance);
            //            }

            //            foreach (Guid mapWidgetId in projectDto_.Layers[roomId][layerIndex].MapWidgetIds[r][c])
            //            {
            //                MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);
            //                lstMapWidgetsToRefreshInGrid.Add(mapWidget);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    int r = projectDto_.Layers[roomId][layerIndex].Rows - 1;

            //    for (int c = 0; c < projectDto_.Layers[roomId][layerIndex].Cols; c++)
            //    {
            //        foreach (Guid actorId in projectDto_.Layers[roomId][layerIndex].ActorIds[r][c])
            //        {
            //            EntityInstanceDto actorInstance = GetEntityInstance(actorId);
            //            lstActorsToRefreshInGrid.Add(actorInstance);
            //        }

            //        foreach (Guid eventId in projectDto_.Layers[roomId][layerIndex].EventIds[r][c])
            //        {
            //            EntityInstanceDto eventInstance = GetEntityInstance(eventId);
            //            lstEventsToRefreshInGrid.Add(eventInstance);
            //        }

            //        foreach (Guid mapWidgetId in projectDto_.Layers[roomId][layerIndex].MapWidgetIds[r][c])
            //        {
            //            MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);
            //            lstMapWidgetsToRefreshInGrid.Add(mapWidget);
            //        }
            //    }
            //}

            //foreach (EntityInstanceDto actorInstance in lstActorsToRefreshInGrid)
            //{
            //    removeEntityInstanceFromGrid(actorInstance);
            //}

            //foreach (EntityInstanceDto eventInstance in lstEventsToRefreshInGrid)
            //{
            //    removeEntityInstanceFromGrid(eventInstance);
            //}

            //foreach (MapWidgetDto mapWidget in lstMapWidgetsToRefreshInGrid)
            //{
            //    removeMapWidgetFromGrid(mapWidget, projectDto_);
            //}

            //projectDto_.Layers[roomId][layerIndex].Rows = rows;

            //foreach (EntityInstanceDto actorInstance in lstActorsToRefreshInGrid)
            //{
            //    addEntityInstanceToGrid(actorInstance);
            //}

            //foreach (EntityInstanceDto eventInstance in lstEventsToRefreshInGrid)
            //{
            //    addEntityInstanceToGrid(eventInstance);
            //}

            //foreach (MapWidgetDto mapWidget in lstMapWidgetsToRefreshInGrid)
            //{
            //    addMapWidgetToGrid(mapWidget, projectDto_);
            //}

            //// Max rows may not longer be accurate. Refresh it.
            //projectUiState_.MaxRows[roomId] = 0;

            //foreach (LayerDto layer in projectDto_.Layers[roomId])
            //{
            //    // If this layer's rows are greater than the current max, set the new value.
            //    if (layer.Rows > projectUiState_.MaxRows[roomId])
            //    {
            //        projectUiState_.MaxRows[roomId] = layer.Rows;
            //    }
            //}
        }

        private void resizeColumns(Guid roomId, int layerIndex, int columns)
        {
            //findmeupdate  Need to update this when I get a chance. For now do nothing.


            //// Build a list of entities who are in grid cells which will be affected. 
            //// They will need to be removed from the grid, and re-added, because the grid is changing.

            //// When reducing the number of columns, every actor/event/widget from the new last column 
            //// to the old last column could potentially need re-added.

            //// When increasing the number of columns, every actor/event/widget in the last column will
            //// potentially need to be re-added.
            ////findme123
            //HashSet<EntityInstanceDto> lstActorsToRefreshInGrid = new HashSet<EntityInstanceDto>();
            //HashSet<EntityInstanceDto> lstEventsToRefreshInGrid = new HashSet<EntityInstanceDto>();
            //HashSet<MapWidgetDto> lstMapWidgetsToRefreshInGrid = new HashSet<MapWidgetDto>();

            //if (columns < projectDto_.Layers[roomId][layerIndex].Cols)
            //{
            //    for (int r = 0; r < projectDto_.Layers[roomId][layerIndex].Rows; r++)
            //    {
            //        for (int c = columns - 1; c < projectDto_.Layers[roomId][layerIndex].Cols; c++)
            //        {
            //            foreach (Guid actorId in projectDto_.Layers[roomId][layerIndex].ActorIds[r][c])
            //            {
            //                EntityInstanceDto actorInstance = GetEntityInstance(actorId);
            //                lstActorsToRefreshInGrid.Add(actorInstance);
            //            }

            //            foreach (Guid eventId in projectDto_.Layers[roomId][layerIndex].EventIds[r][c])
            //            {
            //                EntityInstanceDto eventInstance = GetEntityInstance(eventId);
            //                lstEventsToRefreshInGrid.Add(eventInstance);
            //            }

            //            foreach (Guid mapWidgetId in projectDto_.Layers[roomId][layerIndex].MapWidgetIds[r][c])
            //            {
            //                MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);
            //                lstMapWidgetsToRefreshInGrid.Add(mapWidget);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    int c = projectDto_.Layers[roomId][layerIndex].Cols - 1;

            //    for (int r = 0; r < projectDto_.Layers[roomId][layerIndex].Rows; r++)
            //    {
            //        foreach (Guid actorId in projectDto_.Layers[roomId][layerIndex].ActorIds[r][c])
            //        {
            //            EntityInstanceDto actorInstance = GetEntityInstance(actorId);
            //            lstActorsToRefreshInGrid.Add(actorInstance);
            //        }

            //        foreach (Guid eventId in projectDto_.Layers[roomId][layerIndex].EventIds[r][c])
            //        {
            //            EntityInstanceDto eventInstance = GetEntityInstance(eventId);
            //            lstEventsToRefreshInGrid.Add(eventInstance);
            //        }

            //        foreach (Guid mapWidgetId in projectDto_.Layers[roomId][layerIndex].MapWidgetIds[r][c])
            //        {
            //            MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);
            //            lstMapWidgetsToRefreshInGrid.Add(mapWidget);
            //        }
            //    }
            //}

            //foreach (EntityInstanceDto actorInstance in lstActorsToRefreshInGrid)
            //{
            //    removeEntityInstanceFromGrid(actorInstance);
            //}

            //foreach (EntityInstanceDto eventInstance in lstEventsToRefreshInGrid)
            //{
            //    removeEntityInstanceFromGrid(eventInstance);
            //}

            //foreach (MapWidgetDto mapWidget in lstMapWidgetsToRefreshInGrid)
            //{
            //    removeMapWidgetFromGrid(mapWidget, projectDto_);
            //}

            //projectDto_.Layers[roomId][layerIndex].Cols = columns;

            //foreach (EntityInstanceDto actorInstance in lstActorsToRefreshInGrid)
            //{
            //    addEntityInstanceToGrid(actorInstance);
            //}

            //foreach (EntityInstanceDto eventInstance in lstEventsToRefreshInGrid)
            //{
            //    addEntityInstanceToGrid(eventInstance);
            //}

            //foreach (MapWidgetDto mapWidget in lstMapWidgetsToRefreshInGrid)
            //{
            //    addMapWidgetToGrid(mapWidget, projectDto_);
            //}

            //// Max columns may not longer be accurate. Refresh it.
            //projectUiState_.MaxCols[roomId] = 0;

            //foreach (LayerDto layer in projectDto_.Layers[roomId])
            //{
            //    // If this layer's columns are greater than the current max, set the new value.
            //    if (layer.Cols > projectUiState_.MaxCols[roomId])
            //    {
            //        projectUiState_.MaxCols[roomId] = layer.Cols;
            //    }
            //}
        }



        private void setTileSheetImage(TileSheetDto tileSheet, string imagePath)
        {
            BitmapResourceDto newBitmapResource = new BitmapResourceDto();
            
            // Bitmap data loading was moved to the resource manager in 2.2
            //newBitmapResource.BitmapImage = new Bitmap(imagePath);

            newBitmapResource.Path = imagePath;

            newBitmapResource.RelativePath = uriUtility_.GetRelativePath(newBitmapResource.Path);

            // Remove the existing bitmap resource, if one exists.
            bool oldImageLoaded = false;

            if (projectDto_.Bitmaps.ContainsKey(tileSheet.BitmapResourceId))
            {
                BitmapResourceDto existingBitmap = projectDto_.Bitmaps[tileSheet.BitmapResourceId];

                if (existingBitmap.BitmapImage != null)
                {
                    oldImageLoaded = true;
                }

                //projectResources_.Bitmaps.Remove(tileSheet.BitmapResourceId);
                projectDto_.Bitmaps.Remove(tileSheet.BitmapResourceId);
            }

            newBitmapResource.OwnerId = tileSheet.Id;

            tileSheet.BitmapResourceId = newBitmapResource.Id;
                        

            // Bitmap data loading was moved to the resource manager in 2.2
            //Bitmap tileSheetWithTransparency = bitmapUtility_.ApplyTransparency(newBitmapResource.BitmapImage);

            //newBitmapResource.BitmapImageWithTransparency = tileSheetWithTransparency;

            // Update any tile objects to use the new ID.
            foreach (TileObjectDto tileObject in projectDto_.TileObjects[tileSheet.Id])
            {
                tileObject.BitmapResourceId = newBitmapResource.Id;
            }

            //projectResources_.Bitmaps.Add(newBitmapResource.Id, newBitmapResource);
            projectDto_.Bitmaps.Add(newBitmapResource.Id, newBitmapResource);

            // If it is replacing a loaded bitmap, load it.
            if (oldImageLoaded == true)
            {
                LoadBitmap(newBitmapResource.Id);
            }

            GC.Collect();
        }

        public void SetTileSheetColumns(Guid tileSheetId, int columns)
        {
            saveProjectState();

            int tileSheetIndex = GetTileSheetIndexFromId(tileSheetId);

            ChangesMade = true;

            projectDto_.TileSheets[tileSheetIndex].Columns = columns;

            generateTileSheetImages(tileSheetIndex);
        }

        public void SetTileSheetRows(Guid tileSheetId, int rows)
        {
            saveProjectState();

            int tileSheetIndex = GetTileSheetIndexFromId(tileSheetId);

            ChangesMade = true;

            projectDto_.TileSheets[tileSheetIndex].Rows = rows;

            generateTileSheetImages(tileSheetIndex);
        }
        
        private void setSpriteSheetImage(SpriteSheetDto spriteSheet, string imagePath)
        {
            // When setting a new image, reset the cell parsing data.
            spriteSheet.CellWidth = 0;
            spriteSheet.CellHeight = 0;
            spriteSheet.Rows = 0;
            spriteSheet.Columns = 0;

            BitmapResourceDto newBitmapResource = new BitmapResourceDto();
            
            // Bitmap data loading was moved to the resource manager in 2.2
            //newBitmapResource.BitmapImage = new Bitmap(imagePath);

            newBitmapResource.Path = imagePath;

            newBitmapResource.RelativePath = uriUtility_.GetRelativePath(newBitmapResource.Path);

            // Bitmap data loading was moved to the resource manager in 2.2
            //Bitmap spriteSheetWithTransparency = bitmapUtility_.ApplyTransparency(newBitmapResource.BitmapImage);

            //newBitmapResource.BitmapImageWithTransparency = spriteSheetWithTransparency;

            // Remove the existing bitmap resource, if one exists.
            //if (projectResources_.Bitmaps.ContainsKey(spriteSheet.BitmapResourceId))
            if (projectDto_.Bitmaps.ContainsKey(spriteSheet.BitmapResourceId))
            {
                //projectResources_.Bitmaps.Remove(spriteSheet.BitmapResourceId);
                projectDto_.Bitmaps.Remove(spriteSheet.BitmapResourceId);
            }

            newBitmapResource.OwnerId = spriteSheet.Id;

            spriteSheet.BitmapResourceId = newBitmapResource.Id;

            //projectResources_.Bitmaps.Add(newBitmapResource.Id, newBitmapResource);
            projectDto_.Bitmaps.Add(newBitmapResource.Id, newBitmapResource);

            GC.Collect();
        }

        private void generateSpriteSheetImages(int spriteSheetIndex)
        {
            SpriteSheetDto spriteSheet = projectDto_.SpriteSheets[spriteSheetIndex];

            int columns = projectDto_.SpriteSheets[spriteSheetIndex].Columns;
            int rows = projectDto_.SpriteSheets[spriteSheetIndex].Rows;
            int cellWidth = projectDto_.SpriteSheets[spriteSheetIndex].CellWidth;
            int cellHeight = projectDto_.SpriteSheets[spriteSheetIndex].CellHeight;
            int cellPadding = projectDto_.SpriteSheets[spriteSheetIndex].Padding;
            float scaleFactor = projectDto_.SpriteSheets[spriteSheetIndex].ScaleFactor;

            // Clear out the ImageList and repopulate it.
            Guid resourceId = spriteSheet.BitmapResourceId;

            BitmapResourceDto bitmapResource = projectDto_.Bitmaps[resourceId];

            bitmapUtility_.SplitImageIntoCells(bitmapResource, columns, rows, cellWidth, cellHeight, cellPadding, scaleFactor);
        }

        private void generateTileSheetImages(int tileSheetIndex)
        {
            TileSheetDto tileSheet = projectDto_.TileSheets[tileSheetIndex];

            int columns = tileSheet.Columns;
            int rows = tileSheet.Rows;
            int cellWidth = tileSheet.TileSize;
            int cellHeight = tileSheet.TileSize;
            float scaleFactor = tileSheet.ScaleFactor;

            // Clear out the ImageList and repopulate it.
            Guid resourceId = tileSheet.BitmapResourceId;

            BitmapResourceDto bitmapResource = projectDto_.Bitmaps[resourceId];

            // The tile size will always be the same, but the bitmap size might need to be scaled.

            int scaledWidth = (int)(cellWidth / scaleFactor);

            int scaledHeight = (int)(cellHeight / scaleFactor);

            bitmapUtility_.SplitImageIntoCells(bitmapResource, columns, rows, scaledWidth, scaledWidth, 0, scaleFactor);
        }

        private void setAudioFileData(AudioAssetDto audioAsset, string audioPath)
        {
            AudioResourceDto newAudioResource = new AudioResourceDto();

            newAudioResource.AudioData = File.ReadAllBytes(audioPath);
            newAudioResource.Audio = new OggFile(audioPath);
            newAudioResource.Path = audioPath;

            audioAsset.AudioResourceId = newAudioResource.Id;

            projectResources_.AudioData.Add(newAudioResource.Id, newAudioResource);
        }

        private MapWidgetDto addNewMapWidget(MapWidgetCreationParametersDto creationParams)
        {
            MapWidgetDto mapWidget = mapWidgetFactory_.CreateMapWidget(creationParams);

            mapWidget.RootOwnerId = creationParams.RoomId;
            mapWidget.OwnerId = creationParams.LayerId;
                        
            nameGenerator_.SetNextAvailableMapWidgetName(creationParams.BaseName, mapWidget);

            projectDto_.MapWidgets[creationParams.Type].Add(mapWidget.Id, mapWidget);

            // HUD Elements set the LayerID field to the room ID, because that is its immediate parent. I know this is indicative
            // of bad design, but for now just ignore because I've got more important things to do. Eventually, it should be changed
            // to a more generic name of MapWidgetsByOwner, and add the RoomID as a key.
            // It may even be safe to remove this entirely, because I don't think I even use it anymore.
            if (projectDto_.MapWidgetsByLayer.ContainsKey(creationParams.LayerId) == true)
            {
                projectDto_.MapWidgetsByLayer[creationParams.LayerId].Add(mapWidget.Id, mapWidget);
            }

            // Why did I do += instead of =? Was there a reason or was this an innocuous error that was overlooked?
            //mapWidget.BoundingBox.Left += creationParams.Bounds.Left;
            //mapWidget.BoundingBox.Top += creationParams.Bounds.Top;
            //mapWidget.BoundingBox.Width += creationParams.Bounds.Width;
            //mapWidget.BoundingBox.Height += creationParams.Bounds.Height;

            // The answer to the above question is that the bounds in the creation params were having the mouse cursor set in the top and left.
            // And so without doing +=, it would always be in the corner. But then a new question is, what is the position offset for?

            // Copy the bounding box and offset the position after using the position offset.
            mapWidget.BoundingBox.Left = creationParams.Bounds.Left;
            mapWidget.BoundingBox.Top = creationParams.Bounds.Top;
            
            // If there is a bounds size set in the creation params, add it. Otherwise set it.
            if (creationParams.Bounds.Width != 0)
            {
                mapWidget.BoundingBox.Width = creationParams.Bounds.Width;
            }
            else
            {
                mapWidget.BoundingBox.Width += creationParams.Bounds.Width;
            }

            if (creationParams.Bounds.Height != 0)
            {
                mapWidget.BoundingBox.Height = creationParams.Bounds.Height;
            }
            else
            {
                mapWidget.BoundingBox.Height += creationParams.Bounds.Height;
            }
            
            if (string.IsNullOrEmpty(creationParams.SerializedDataString) == false)
            {
                mapWidget.Controller.DeserializeFromString(creationParams.SerializedDataString);
            }

            Guid selectedRoomId = projectUiState_.SelectedRoomId;

            mapWidget.BoundingBox.Left += creationParams.PositionOffset.X; 
            mapWidget.BoundingBox.Top += creationParams.PositionOffset.Y;

            mapWidget.Position.X = creationParams.Position.X;
            mapWidget.Position.Y = creationParams.Position.Y;

            mapWidget.Controller.Initialize();

            // Handle any instance specific initializers. There's a more OO way to do this, but I've got more important things to do
            // right now than figure that out.
            if (creationParams.Type == MapWidgetType.WorldGeometry)
            {
                ((WorldGeometryWidgetDto)mapWidget).Corner1.X = ((WorldGeometryMapWidgetCreationParametersDto)creationParams).Corner1.X;
                ((WorldGeometryWidgetDto)mapWidget).Corner1.Y = ((WorldGeometryMapWidgetCreationParametersDto)creationParams).Corner1.Y;
                ((WorldGeometryWidgetDto)mapWidget).Corner2.X = ((WorldGeometryMapWidgetCreationParametersDto)creationParams).Corner2.X;
                ((WorldGeometryWidgetDto)mapWidget).Corner2.Y = ((WorldGeometryMapWidgetCreationParametersDto)creationParams).Corner2.Y;
            }

            projectUtility_.AddMapWidgetToGrid(mapWidget, projectDto_);

            ChangesMade = true;


            MapWidgetProperties mapWidgetProperties = new MapWidgetProperties(this);
            
            foreach (PropertyDto property in creationParams.Properties)
            {
                property.OwnerId = mapWidget.Id;
                mapWidgetProperties.AddProperty(property);
            }
            
            projectDto_.MapWidgetProperties[mapWidget.Id] = mapWidgetProperties;

            mapWidget.Controller.ResetProperties(mapWidgetProperties);

            projectUiState_.MapWidgetSelected[mapWidget.Id] = false;

            projectUiState_.RoomMapWidgetsByType[mapWidget.RootOwnerId][creationParams.Type].Add(mapWidget);
            projectUiState_.RoomMapWidgetsByType[mapWidget.RootOwnerId][creationParams.Type].Sort();

            projectUiState_.RoomMapWidgets[mapWidget.RootOwnerId].Add(mapWidget);
            projectUiState_.RoomMapWidgets[mapWidget.RootOwnerId].Sort();

            refreshProperties();

            OnMapWidgetAdd(new MapWidgetAddedEventArgs(mapWidget.Id, creationParams.Type));

            return mapWidget;
        }

        private void adjustActorWidgetBoundingBoxPosition(Guid entityId, Point boundingBoxPosition)
        {
            // Update the bounding box for any map widgets for the given entity ID.
            foreach (KeyValuePair<Guid, MapWidgetDto> kvp in projectDto_.MapWidgets[MapWidgetType.Actor])
            {
                ActorWidgetDto actorWidget = (ActorWidgetDto)kvp.Value;

                if (actorWidget.EntityId == entityId)
                {
                    projectUtility_.RemoveMapWidgetFromGrid(actorWidget, projectDto_);

                    actorWidget.BoundingBox.Left += boundingBoxPosition.X;

                    actorWidget.BoundingBox.Top += boundingBoxPosition.Y;

                    projectUtility_.AddMapWidgetToGrid(actorWidget, projectDto_);
                }
            }

            refreshViews();
        }

        private void adjustHudElementWidgetBoundingBoxPosition(Guid entityId, Point boundingBoxPosition)
        {
            // Update the bounding box for any map widgets for the given entity ID.
            foreach (KeyValuePair<Guid, MapWidgetDto> kvp in projectDto_.MapWidgets[MapWidgetType.HudElement])
            {
                HudElementWidgetDto hudElementWidget = (HudElementWidgetDto)kvp.Value;

                if (hudElementWidget.EntityId == entityId)
                {
                    hudElementWidget.BoundingBox.Left += boundingBoxPosition.X;

                    hudElementWidget.BoundingBox.Top += boundingBoxPosition.Y;                    
                }
            }

            refreshViews();
        }

        private void deleteMapWidget(Guid id, bool deleteRoomWidget, bool deleteFromLayer = true)
        {
            MapWidgetDto mapWidget = GetMapWidget(id);

            OnBeforeMapWidgetDelete(new BeforeMapWidgetDeleteEventArgs(mapWidget.Type));

            if (deleteRoomWidget == true)
            {
                // Remove from the by type list
                Dictionary<Guid, Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>>> roomMapWidgetsByType = null;

                roomMapWidgetsByType = projectUiState_.RoomMapWidgetsByType;

                // Loop through the list of map widgets for each room.                
                foreach (KeyValuePair<Guid, Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>>> kvp in roomMapWidgetsByType)
                {
                    foreach (KeyValuePair<MapWidgetType, SortableBindingList<MapWidgetDto>> kvp2 in kvp.Value)
                    {
                        for (int j = kvp2.Value.Count - 1; j >= 0; j--)
                        {
                            if (kvp2.Value[j].Id == id)
                            {
                                kvp2.Value.RemoveAt(j);
                            }
                        }
                    }
                }

                // Remove from the typeless list
                Dictionary<Guid, SortableBindingList<MapWidgetDto>> roomMapWidgets = null;

                roomMapWidgets = projectUiState_.RoomMapWidgets;

                // Loop through the list of map widgets for each room.
                foreach (KeyValuePair<Guid, SortableBindingList<MapWidgetDto>> kvp in roomMapWidgets)
                {
                    for (int j = kvp.Value.Count - 1; j >= 0; j--)
                    {
                        if (kvp.Value[j].Id == id)
                        {
                            kvp.Value.RemoveAt(j);
                        }
                    }
                }
            }

            // Remove the properties.
            projectDto_.MapWidgetProperties.Remove(id);

            // Remove the name-ID map values.
            nameGenerator_.RemoveMapWidgetName(mapWidget);

            // Remove from the list of widgets on the layer
            if (deleteFromLayer == true)
            {
                projectDto_.MapWidgetsByLayer[mapWidget.OwnerId].Remove(mapWidget.Id);
            }

            // Remove from the selector.
            bool isSelected = projectUiState_.MapWidgetSelected[mapWidget.Id];

            projectUiState_.MapWidgetSelected.Remove(mapWidget.Id);

            if (isSelected == true)
            {
                Guid roomId = mapWidget.RootOwnerId;

                MapWidgetSelectorDto selector = projectUiState_.MapWidgetSelector[roomId];

                selector.SelectedMapWidgetIds.Remove(id);
            }

            // Remove from the grid.
            projectUtility_.RemoveMapWidgetFromGrid(mapWidget, projectDto_);

            ChangesMade = true;
            
            // Remove the widget.
            projectDto_.MapWidgets[mapWidget.Type].Remove(id);
        }
        
        private void updateActorWidgetBoundingBoxSize(Guid entityId, Size boundingBoxSize)
        {
            // Update the bounding box for any map widgets for the given entity ID.
            foreach (KeyValuePair<Guid, MapWidgetDto> kvp in projectDto_.MapWidgets[MapWidgetType.Actor])
            {
                ActorWidgetDto actorWidget = (ActorWidgetDto)kvp.Value;

                if (actorWidget.EntityId == entityId)
                {
                    projectUtility_.RemoveMapWidgetFromGrid(actorWidget, projectDto_);

                    actorWidget.BoundingBox.Width = boundingBoxSize.Width;
                    actorWidget.BoundingBox.Height = boundingBoxSize.Height;
                    
                    projectUtility_.AddMapWidgetToGrid(actorWidget, projectDto_);
                }
            }

            refreshViews();
        }

        private void updateHudElementWidgetBoundingBoxSize(Guid entityId, Size boundingBoxSize)
        {
            // Update the bounding box for any map widgets for the given entity ID.
            foreach (KeyValuePair<Guid, MapWidgetDto> kvp in projectDto_.MapWidgets[MapWidgetType.HudElement])
            {
                HudElementWidgetDto hudElementWidget = (HudElementWidgetDto)kvp.Value;

                if (hudElementWidget.EntityId == entityId)
                {
                    hudElementWidget.BoundingBox.Width = boundingBoxSize.Width;
                    hudElementWidget.BoundingBox.Height = boundingBoxSize.Height;
                }
            }

            refreshViews();
        }

        private byte[] decompressProjectState(byte[] compressedProjectState)
        {
            byte[] projectState;

            using (var inStream = new MemoryStream(compressedProjectState))
            {
                using (var decompressStream = new GZipStream(inStream, CompressionMode.Decompress))
                {
                    using (var decompressStreamOut = new MemoryStream())
                    {
                        decompressStream.CopyTo(decompressStreamOut);

                        projectState = decompressStreamOut.ToArray();
                    }
                }
            }

            return projectState;
        }
        
        private void unloadCursorResources()
        {
            if (projectUiState_.SelectedActorIndex >= 0)
            {
                ActorDto selectedActor = projectDto_.Actors[projectUiState_.SelectedActorIndex];

                if (projectDto_.States[selectedActor.Id].Count > 0)
                {
                    Guid initialStateId = selectedActor.InitialStateId;

                    if (initialStateId != Guid.Empty)
                    {
                        if (projectDto_.AnimationSlots.ContainsKey(initialStateId) == true)
                        {
                            int animationSlotCount = projectDto_.AnimationSlots[initialStateId].Count;

                            for (int i = 0; i < animationSlotCount; i++)
                            {
                                // Get the index of the animation.
                                Guid animationId = projectDto_.AnimationSlots[initialStateId][i].Animation;

                                AnimationDto animation = GetAnimation(animationId);

                                if (animation != null)
                                {
                                    Guid spriteSheetId = animation.SpriteSheet;

                                    if (spriteSheetId != Guid.Empty)
                                    {
                                        int sheetIndex = GetSpriteSheetIndexFromId(spriteSheetId);

                                        if (sheetIndex > -1)
                                        {
                                            SpriteSheetDto spriteSheet = projectDto_.SpriteSheets[sheetIndex];

                                            Guid resourceId = spriteSheet.BitmapResourceId;

                                            UnloadBitmapResource(resourceId, EditorModule.Cursor);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (projectUiState_.SelectedHudElementIndex >= 0)
            {

            }
        }

        private void validateUiState()
        {
            // Make sure the selected indexes are within valid bounds.
            if (projectUiState_.SelectedRoomIndex >= projectDto_.Rooms.Count)
            {
                int selectedRoomIndex = projectDto_.Rooms.Count - 1;

                projectUiState_.SelectedRoomIndex = selectedRoomIndex;
                projectUiState_.SelectedRoomId = projectDto_.Rooms[selectedRoomIndex].Id;
            }

            if (projectUiState_.SelectedActorIndex >= projectDto_.Actors.Count)
            {
                int selectedActorIndex = projectDto_.Actors.Count - 1;

                projectUiState_.SelectedActorIndex = selectedActorIndex;
                projectUiState_.SelectedActorId = projectDto_.Actors[selectedActorIndex].Id;
            }

            if (projectUiState_.SelectedEventIndex >= projectDto_.Events.Count)
            {
                int selectedEventIndex = projectDto_.Events.Count - 1;

                projectUiState_.SelectedEventIndex = selectedEventIndex;
                projectUiState_.SelectedEventId = projectDto_.Events[selectedEventIndex].Id;
            }

            if (projectUiState_.SelectedHudElementIndex >= projectDto_.HudElements.Count)
            {
                int selectedHudElementIndex = projectDto_.HudElements.Count - 1;

                projectUiState_.SelectedHudElementIndex = selectedHudElementIndex;
                projectUiState_.SelectedHudElementId = projectDto_.HudElements[selectedHudElementIndex].Id;
            }
            
            // Make sure the selected tile sheet is within the bounds.
            if (projectUiState_.SelectedTileSheetIndex >= projectDto_.TileSheets.Count)
            {
                int selectedTileSheetIndex = projectDto_.TileSheets.Count - 1;

                projectUiState_.SelectedTileSheetIndex = selectedTileSheetIndex;
                projectUiState_.SelectedTileSheetId = projectDto_.TileSheets[selectedTileSheetIndex].Id;
            }

            // If there is a selected tile object, make sure it is within the list bounds.
            if (projectUiState_.SelectedTileObjectId != Guid.Empty)
            {
                if (projectUiState_.SelectedTileObjectIndex >= projectDto_.TileObjects[projectUiState_.SelectedTileSheetId].Count)
                {
                    int selectedTileObjectIndex = projectDto_.TileObjects[projectUiState_.SelectedTileSheetId].Count - 1;

                    projectUiState_.SelectedTileObjectIndex = selectedTileObjectIndex;
                    projectUiState_.SelectedTileSheetId = projectDto_.TileObjects[projectUiState_.SelectedTileSheetId][selectedTileObjectIndex].Id;
                }
            }
            
            // Reset the max columns and rows.
            projectUiState_.MaxCols.Clear();
            projectUiState_.MaxRows.Clear();

            foreach (RoomDto room in projectDto_.Rooms)
            {
                if (projectUiState_.SelectedLayerIndex[room.Id] >= projectDto_.Layers[room.Id].Count)
                {
                    projectUiState_.SelectedLayerIndex[room.Id] = projectDto_.Layers[room.Id].Count - 1;
                }

                bool rowsColsInitialized = false;

                foreach (LayerDto layer in projectDto_.Layers[room.Id])
                {
                    if (rowsColsInitialized == false)
                    {
                        projectUiState_.MaxCols.Add(room.Id, layer.Cols);
                        projectUiState_.MaxRows.Add(room.Id, layer.Rows);

                        rowsColsInitialized = true;
                    }
                    else
                    {
                        // If this layer's rows or columns are greater than the current max, set the new value.
                        if (layer.Cols > projectUiState_.MaxCols[room.Id])
                        {
                            projectUiState_.MaxCols[room.Id] = layer.Cols;
                        }

                        if (layer.Rows > projectUiState_.MaxRows[room.Id])
                        {
                            projectUiState_.MaxRows[room.Id] = layer.Rows;
                        }
                    }
                }

                // Make sure that the widget selector does not contain any widget instances that do not exist.
                List<Guid> selectedWidgetIds = new List<Guid>(projectUiState_.MapWidgetSelector[room.Id].SelectedMapWidgetIds);

                foreach (Guid mapWidgetId in selectedWidgetIds)
                {
                    MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);

                    if (projectDto_.MapWidgets[mapWidget.Type].ContainsKey(mapWidgetId) == false)
                    {
                        // The selected map widget doesn't exist.
                        projectUiState_.MapWidgetSelector[room.Id].SelectedMapWidgetIds.Remove(mapWidgetId);
                    }
                }
                
                List<Guid> mapWidgetIds = new List<Guid>(projectUiState_.MapWidgetSelected.Keys);
                foreach (Guid mapWidgetId in mapWidgetIds)
                {
                    MapWidgetDto mapWidget = GetMapWidget(mapWidgetId);

                    if (projectDto_.MapWidgets[mapWidget.Type].ContainsKey(mapWidgetId) == false)
                    {
                        // The selected map widget doesn't exist.
                        projectUiState_.MapWidgetSelected.Remove(mapWidgetId);
                    }
                }
                

                // These lists will be rebuilt later in this function. As long as we're looping
                // through the rooms, clear them now.               
                foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                {
                    projectUiState_.RoomMapWidgetsByType[room.Id][mapWidgetType].Clear();
                }
                
                projectUiState_.RoomMapWidgets[room.Id].Clear();
            }

            //findmelater Do I need to do selected tilesheet index + selected tile object index?
            //foreach (TileSheetDto tileSheet in projectDto_.TileSheets)
            //{
            //    int tileObjectCount = projectDto_.TileObjects[tileSheet.Id].Count;

            //    if (projectUiState_.SelectedTileObjectIndex[tileSheet.Id] >= tileObjectCount)
            //    {
            //        projectUiState_.SelectedTileObjectIndex[tileSheet.Id] = tileObjectCount - 1;
            //    }
            //}

            // The list of map widgets per room may be out of synch. It might contain map widgets that
            // no longer exist, or it might be missing map widget that do exist. Rebuild them.
            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in projectDto_.MapWidgets[mapWidgetType])
                {
                    MapWidgetDto mapWidgetInstance = kvp.Value;

                    Guid roomId = mapWidgetInstance.RootOwnerId;

                    projectUiState_.RoomMapWidgets[roomId].RaiseListChangedEvents = false;
                    projectUiState_.RoomMapWidgets[roomId].Add(mapWidgetInstance);
                    projectUiState_.RoomMapWidgets[roomId].RaiseListChangedEvents = true;

                    projectUiState_.RoomMapWidgetsByType[roomId][mapWidgetType].RaiseListChangedEvents = false;
                    projectUiState_.RoomMapWidgetsByType[roomId][mapWidgetType].Add(mapWidgetInstance);
                    projectUiState_.RoomMapWidgetsByType[roomId][mapWidgetType].RaiseListChangedEvents = true;
                }
            }
        }

        private ProjectUiStateDto generateUiStateFromProject(ProjectDto project)
        {
            ProjectUiStateDto uiState = new ProjectUiStateDto();

            // Initial selected room ID needs to be initialized before 
            // the ProjectCreated event is called.
            uiState.SelectedRoomId = projectDto_.Rooms[0].Id;
            
            nameGenerator_.ClearMapWidgetNames();

            for (int i = 0; i < project.Rooms.Count; i++)
            {
                RoomDto currentRoom = project.Rooms[i];

                // Initialize the layer data for the room.
                uiState.SelectedLayerIndex.Add(currentRoom.Id, 0);
                uiState.SelectedTileIndex.Add(currentRoom.Id, -1);

                uiState.LayerOrdinalToIndexMap.Add(currentRoom.Id, new List<int>());
                
                MapWidgetSelectorDto mapWidgetSelector = new MapWidgetSelectorDto();
                mapWidgetSelector.FillColor = Globals.actorFillColor;
                mapWidgetSelector.OutlineColor = Globals.actorOutlineColor;
                uiState.MapWidgetSelector.Add(currentRoom.Id, mapWidgetSelector);
                
                uiState.MapWidgetMode.Add(currentRoom.Id, MapWidgetMode.Actor);
                uiState.SelectedMapWidgetType.Add(currentRoom.Id, MapWidgetType.SpawnPoint);
                uiState.CameraLocation.Add(currentRoom.Id, new Point2D(0, 0));
                uiState.CameraLocationMax.Add(currentRoom.Id, new Point2D(0, 0));
                uiState.CanvasOffset.Add(currentRoom.Id, new Point2D(0, 0));
                uiState.CanvasOffsetMax.Add(currentRoom.Id, new Point2D(0, 0));
                
                Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>> mapWidgetsByType = new Dictionary<MapWidgetType, SortableBindingList<MapWidgetDto>>();

                foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                {
                    mapWidgetsByType.Add(mapWidgetType, new SortableBindingList<MapWidgetDto>());
                }
                
                uiState.RoomMapWidgetsByType.Add(currentRoom.Id, mapWidgetsByType);

                foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                {
                    uiState.RoomMapWidgetsByType[currentRoom.Id][mapWidgetType].Comparer = new MapWidgetComparer();
                }
                
                SortableBindingList<MapWidgetDto> mapWidgets = new SortableBindingList<MapWidgetDto>();
                uiState.RoomMapWidgets.Add(currentRoom.Id, mapWidgets);
                uiState.RoomMapWidgets[currentRoom.Id].Comparer = new MapWidgetComparer();

                uiState.SelectedLayerIndex[currentRoom.Id] = 0;

                int layerCounter = 0;
                bool rowsColsInitialized = false;

                foreach (LayerDto layer in project.Layers[currentRoom.Id])
                {
                    if (rowsColsInitialized == false)
                    {
                        uiState.MaxCols.Add(currentRoom.Id, layer.Cols);
                        uiState.MaxRows.Add(currentRoom.Id, layer.Rows);

                        rowsColsInitialized = true;
                    }
                    else
                    {
                        // If this layer's rows or columns are greater than the current max, set the new value.
                        if (layer.Cols > uiState.MaxCols[currentRoom.Id])
                        {
                            uiState.MaxCols[currentRoom.Id] = layer.Cols;
                        }

                        if (layer.Rows > uiState.MaxRows[currentRoom.Id])
                        {
                            uiState.MaxRows[currentRoom.Id] = layer.Rows;
                        }
                    }

                    // Add an entry to the ordinal/index maps for this layer.
                    uiState.LayerOrdinalToIndexMap[currentRoom.Id].Add(layerCounter);

                    // Initialize the layer visibility.
                    uiState.LayerVisible.Add(layer.Id, true);

                    layerCounter++;
                }
            }

            foreach (TileSheetDto tileSheet in project.TileSheets)
            {
                foreach (TileObjectDto tileObject in project.TileObjects[tileSheet.Id])
                {
                    uiState.TileObjectCursorCell.Add(tileObject.Id, new Point2D(0, 0));
                }
            }
            
            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project.MapWidgets[mapWidgetType])
                {
                    MapWidgetDto mapWidgetInstance = kvp.Value;

                    uiState.RoomMapWidgets[mapWidgetInstance.RootOwnerId].Add(mapWidgetInstance);

                    uiState.RoomMapWidgetsByType[mapWidgetInstance.RootOwnerId][mapWidgetType].Add(mapWidgetInstance);
                    uiState.MapWidgetSelected[mapWidgetInstance.Id] = false;

                    nameGenerator_.AddMapWidgetName(mapWidgetInstance);
                }
            }
            
            return uiState;
        }

        private void replaceUiState(ProjectUiStateDto uiState)
        {
            // Copy the data from the old ui state to the new one.
            uiState.CameraMode = projectUiState_.CameraMode;
            uiState.EditMode = projectUiState_.EditMode;
            uiState.ShowGrid = projectUiState_.ShowGrid;
            uiState.ShowCameraOutline = projectUiState_.ShowCameraOutline;
            uiState.ShowMouseOver = projectUiState_.ShowMouseOver;
            uiState.ShowOutlines = projectUiState_.ShowOutlines;
            uiState.TransparentSelect = projectUiState_.TransparentSelect;

            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                uiState.CanSelectMapWidget[mapWidgetType] = projectUiState_.CanSelectMapWidget[mapWidgetType];
            }

            uiState.SelectedRoomId = projectUiState_.SelectedRoomId;
            uiState.SelectedRoomIndex = projectUiState_.SelectedRoomIndex;

            uiState.SelectedActorId = projectUiState_.SelectedActorId;
            uiState.SelectedActorIndex = projectUiState_.SelectedActorIndex;

            uiState.SelectedEventId = projectUiState_.SelectedEventId;
            uiState.SelectedEventIndex = projectUiState_.SelectedEventIndex;

            uiState.SelectedHudElementId = projectUiState_.SelectedHudElementId;
            uiState.SelectedHudElementIndex = projectUiState_.SelectedHudElementIndex;

            uiState.SelectedTileSheetId = projectUiState_.SelectedTileSheetId;
            uiState.SelectedTileSheetIndex = projectUiState_.SelectedTileSheetIndex;

            uiState.SelectedTileObjectId = projectUiState_.SelectedTileObjectId;
            uiState.SelectedTileObjectIndex = projectUiState_.SelectedTileObjectIndex;
            
            List<Guid> mapWidgetIds = new List<Guid>(projectUiState_.MapWidgetSelected.Keys);
            foreach (Guid mapWidgetId in mapWidgetIds)
            {
                if (projectUiState_.MapWidgetSelected.ContainsKey(mapWidgetId) == true)
                {
                    uiState.MapWidgetSelected[mapWidgetId] = projectUiState_.MapWidgetSelected[mapWidgetId];
                }
            }

            List<Guid> roomIds = new List<Guid>(projectUiState_.SelectedLayerIndex.Keys);
            foreach (Guid roomId in roomIds)
            {
                if (projectUiState_.SelectedLayerIndex.ContainsKey(roomId) == true)
                {
                    // After generating a new UI state, the layer positions get re-indexed to their default positions,
                    // and the ordinal map now contains in order indexes.
                    int oldIndex = projectUiState_.SelectedLayerIndex[roomId];
                    int newIndex = projectUiState_.LayerOrdinalToIndexMap[roomId].IndexOf(oldIndex);

                    uiState.SelectedLayerIndex[roomId] = newIndex;

                    uiState.SelectedTileIndex[roomId] = projectUiState_.SelectedTileIndex[roomId];
                    uiState.MapWidgetSelector[roomId] = projectUiState_.MapWidgetSelector[roomId];
                    uiState.MapWidgetMode[roomId] = projectUiState_.MapWidgetMode[roomId];
                    uiState.SelectedMapWidgetType[roomId] = projectUiState_.SelectedMapWidgetType[roomId];
                    uiState.CameraLocation[roomId] = projectUiState_.CameraLocation[roomId];
                    uiState.CameraLocationMax[roomId] = projectUiState_.CameraLocationMax[roomId];
                    uiState.CanvasOffset[roomId] = projectUiState_.CanvasOffset[roomId];
                    uiState.CanvasOffsetMax[roomId] = projectUiState_.CanvasOffsetMax[roomId];
                    uiState.MaxCols[roomId] = projectUiState_.MaxCols[roomId];
                    uiState.MaxRows[roomId] = projectUiState_.MaxRows[roomId];
                    
                    foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
                    {
                        uiState.RoomMapWidgetsByType[roomId][mapWidgetType] = projectUiState_.RoomMapWidgetsByType[roomId][mapWidgetType];
                    }

                    uiState.RoomMapWidgets[roomId] = projectUiState_.RoomMapWidgets[roomId];
                }
            }

            List<Guid> layerIds = new List<Guid>(projectUiState_.LayerVisible.Keys);
            foreach (Guid layerId in layerIds)
            {
                if (projectUiState_.LayerVisible.ContainsKey(layerId) == true)
                {
                    uiState.LayerVisible[layerId] = projectUiState_.LayerVisible[layerId];
                }
            }

            List<Guid> tileObjectIds = new List<Guid>(projectUiState_.TileObjectCursorCell.Keys);
            foreach (Guid tileObjectId in tileObjectIds)
            {
                uiState.TileObjectCursorCell[tileObjectId] = projectUiState_.TileObjectCursorCell[tileObjectId];
            }

            projectUiState_ = uiState;
        }
        
        private void saveProjectState()
        {
            //byte[] compressedProjectState = getCompressedProjectState();

            //undoStack_.Push(compressedProjectState);

            //redoStack_.Clear();

            //OnProjectStateChanged(new ProjectStateChangedEventArgs());
        }

        private byte[] getCompressedProjectState()
        {
            byte[] compressedProjectState;

            using (MemoryStream stream = new MemoryStream())
            {
                WriteProjectDtoToStream(stream, false);

                byte[] projectData = stream.ToArray();

                using (var outStream = new MemoryStream())
                {
                    using (var compressedStream = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        using (var dataStream = new MemoryStream(projectData))
                        {
                            dataStream.CopyTo(compressedStream);
                        }
                    }

                    compressedProjectState = outStream.ToArray();
                }
            }

            return compressedProjectState;
        }
        
        private void refreshViews()
        {
            OnRefreshView(new RefreshViewEventArgs());
        }

        private void refreshProperties()
        {
            OnRefreshProperties(new RefreshPropertiesEventArgs());
        }
        
        private void copyAssetsToProject(ProjectDto assets)
        {
            // Copy the bitmap resource data.
            foreach (KeyValuePair<Guid, BitmapResourceDto> kvp in assets.Bitmaps)
            {
                Guid id = kvp.Key;
                BitmapResourceDto bitmapResource = kvp.Value;

                projectDto_.Bitmaps.Add(id, bitmapResource);
            }

            // Copy the audio resource data.
            foreach (KeyValuePair<Guid, AudioResourceDto> kvp in assets.AudioData)
            {
                Guid id = kvp.Key;
                AudioResourceDto audioResource = kvp.Value;

                projectDto_.AudioData.Add(id, audioResource);
            }
            
            if (projectDto_.TileSize == assets.TileSize)
            {
                foreach (TileSheetDto tileSheet in assets.TileSheets)
                {
                    projectDto_.TileSheets.Add(tileSheet);
                }
                
                foreach (KeyValuePair<Guid, List<TileObjectDto>> kvp in assets.TileObjects)
                {
                    projectDto_.TileObjects.Add(kvp.Key, kvp.Value);
                }                
            }

            List<string> lstNames = new List<string>();

            // Copy the sprite sheet data.

            foreach (SpriteSheetDto spriteSheetDto in assets.SpriteSheets)
            {
                projectDto_.SpriteSheets.Add(spriteSheetDto);

                lstNames.Add(spriteSheetDto.Name);
            }

            nameUtility_.AddSpriteSheetNames(lstNames);
            
            lstNames.Clear();

            foreach (AudioAssetDto audioAsset in assets.AudioAssets)
            {
                lstNames.Add(audioAsset.Name);

                projectDto_.AudioAssets.Add(audioAsset);
            }

            nameUtility_.AddAudioAssetNames(lstNames);

            lstNames.Clear();

            foreach (LoadingScreenDto loadingScreen in assets.LoadingScreens)
            {
                projectDto_.LoadingScreens.Add(loadingScreen);

                lstNames.Add(loadingScreen.Name);
            }

            nameUtility_.AddLoadingScreenNames(lstNames);

            lstNames.Clear();

            foreach (TransitionDto transition in assets.Transitions)
            {
                projectDto_.Transitions.Add(transition);

                lstNames.Add(transition.Name);
            }

            nameUtility_.AddTransitionNames(lstNames);

            lstNames.Clear();

            foreach (ParticleEmitterDto particleEmitter in assets.ParticleEmitters)
            {
                lstNames.Add(particleEmitter.Name);

                projectDto_.ParticleEmitters.Add(particleEmitter);
            }

            nameUtility_.AddParticleEmitterNames(lstNames);

            lstNames.Clear();

            foreach (ParticleDto particle in assets.Particles)
            {
                lstNames.Add(particle.Name);
                projectDto_.Particles.Add(particle);
            }

            nameUtility_.AddParticleNames(lstNames);
            
            lstNames.Clear();

            foreach (ActorDto actor in assets.Actors)
            {
                projectDto_.Actors.Add(actor);
                
                projectDto_.States.Add(actor.Id, new List<StateDto>());
                projectDto_.Properties.Add(actor.Id, new List<PropertyDto>());
            }

            foreach (EventDto eventDto in assets.Events)
            {
                projectDto_.Events.Add(eventDto);

                projectDto_.Properties.Add(eventDto.Id, new List<PropertyDto>());
            }

            foreach (HudElementDto hudElement in assets.HudElements)
            {
                projectDto_.HudElements.Add(hudElement);
                
                projectDto_.States.Add(hudElement.Id, new List<StateDto>());
                projectDto_.Properties.Add(hudElement.Id, new List<PropertyDto>());
            }

            foreach (SpawnPointDto spawnPoint in assets.SpawnPoints)
            {
                projectDto_.SpawnPoints.Add(spawnPoint);

                lstNames.Add(spawnPoint.Name);
            }

            nameUtility_.AddSpawnPointNames(lstNames);
            
            lstNames.Clear();

            foreach (AnimationGroupDto animationGroup in assets.AnimationGroups)
            {
                if (animationGroup.Id != Guid.Empty)
                {
                    projectDto_.AnimationGroups.Add(animationGroup);

                    projectDto_.Animations.Add(animationGroup.Id, new List<AnimationDto>());
                }

                foreach (AnimationDto animation in assets.Animations[animationGroup.Id])
                {
                    projectDto_.Animations[animationGroup.Id].Add(animation);

                    lstNames.Add(animation.Name);

                    projectDto_.Frames.Add(animation.Id, new List<FrameDto>());
                }
            }
            
            nameUtility_.AddAnimationNames(lstNames);

            lstNames.Clear();
            
            foreach (KeyValuePair<Guid, List<FrameDto>> kvp in assets.Frames)
            {

                foreach (FrameDto frame in kvp.Value)
                {
                    projectDto_.Frames[frame.OwnerId].Add(frame);

                    projectDto_.Hitboxes.Add(frame.Id, new List<HitboxDto>());
                    projectDto_.FrameTriggers.Add(frame.Id, new List<FrameTriggerDto>());
                    projectDto_.ActionPoints.Add(frame.Id, new List<ActionPointDto>());
                }                
            }          

            Dictionary<Guid, List<string>> dictNames = new Dictionary<Guid, List<string>>();

            foreach (KeyValuePair<Guid, List<StateDto>> kvp in assets.States)
            {
                foreach (StateDto state in kvp.Value)
                {
                    projectDto_.States[state.OwnerId].Add(state);
                    
                    if (dictNames.ContainsKey(state.OwnerId) == false)
                    {
                        dictNames.Add(state.OwnerId, new List<string>());
                    }

                    dictNames[state.OwnerId].Add(state.Name);
                    
                    projectDto_.Hitboxes.Add(state.Id, new List<HitboxDto>());
                    projectDto_.AnimationSlots.Add(state.Id, new List<AnimationSlotDto>());
                }               
            }
            
            foreach (KeyValuePair<Guid, List<string>> kvp in dictNames)
            {
                Guid entityId = kvp.Key;
                List<string> lstStateNames = kvp.Value;

                nameUtility_.AddStateNames(entityId, lstStateNames);
            }

            dictNames.Clear();

            foreach (KeyValuePair<Guid, List<SceneryAnimationDto>> kvp in assets.SceneryAnimations)
            {
                foreach (SceneryAnimationDto sceneryAnimation in kvp.Value)
                {
                    projectDto_.SceneryAnimations[sceneryAnimation.OwnerId].Add(sceneryAnimation);
                    
                    if (dictNames.ContainsKey(sceneryAnimation.OwnerId) == false)
                    {
                        dictNames.Add(sceneryAnimation.OwnerId, new List<string>());
                    }

                    dictNames[sceneryAnimation.OwnerId].Add(sceneryAnimation.Name);                    
                }
            }

            foreach (KeyValuePair<Guid, List<string>> kvp in dictNames)
            {
                Guid tileSheetId = kvp.Key;
                List<string> lstStateNames = kvp.Value;

                nameUtility_.AddSceneryAnimationNames(tileSheetId, lstStateNames);
            }

            foreach (TriggerSignalDto triggerSignal in assets.TriggerSignals)
            {
                projectDto_.TriggerSignals.Add(triggerSignal);
                
                lstNames.Add(triggerSignal.Name);
            }

            nameUtility_.AddTriggerSignalNames(lstNames);

            lstNames.Clear();

            foreach (KeyValuePair<Guid, List<FrameTriggerDto>> kvp in assets.FrameTriggers)
            {
                foreach (FrameTriggerDto frameTrigger in kvp.Value)
                {
                    // Does the name need to be set here?
                    projectDto_.FrameTriggers[frameTrigger.OwnerId].Add(frameTrigger);
                }
            }

            foreach (KeyValuePair<Guid, List<ActionPointDto>> kvp in assets.ActionPoints)
            {
                foreach (ActionPointDto actionPoint in kvp.Value)
                {
                    if (projectDto_.ActionPoints.ContainsKey(actionPoint.OwnerId) == true)
                    {
                        projectDto_.ActionPoints[actionPoint.OwnerId].Add(actionPoint);
                    }
                }
            }

            foreach (HitboxIdentityDto hitboxIdentity in assets.HitboxIdentities)
            {
                projectDto_.HitboxIdentities.Add(hitboxIdentity);

                lstNames.Add(hitboxIdentity.Name);
            }
            
            nameUtility_.AddHitboxIdentityNames(lstNames);

            lstNames.Clear();

            foreach (KeyValuePair<Guid, List<HitboxDto>> kvp in assets.Hitboxes)
            {
                foreach (HitboxDto hitbox in kvp.Value)
                {
                    // Does name need to be set?
                    projectDto_.Hitboxes[hitbox.OwnerId].Add(hitbox);
                }
            }

            foreach (KeyValuePair<Guid, List<AnimationSlotDto>> kvp in assets.AnimationSlots)
            {
                foreach (AnimationSlotDto animationSlot in kvp.Value)
                {
                    projectDto_.AnimationSlots[animationSlot.OwnerId].Add(animationSlot);
                }
            }

            foreach (KeyValuePair<Guid, List<PropertyDto>> kvp in assets.Properties)
            {
                foreach (PropertyDto property in kvp.Value)
                {
                    projectDto_.Properties[property.OwnerId].Add(property);
                }
            }
            
            foreach (GameButtonDto gameButton in assets.GameButtons)
            {
                projectDto_.GameButtons.Add(gameButton);
            }

            foreach (GameButtonGroupDto gameButtonGroup in assets.GameButtonGroups)
            {
                projectDto_.GameButtonGroups.Add(gameButtonGroup);

                lstNames.Add(gameButtonGroup.Name);
            }
            
            nameUtility_.AddGameButtonGroupNames(lstNames);

            lstNames.Clear();

            foreach (UiWidgetDto uiWidget in assets.UiWidgets)
            {
                projectDto_.UiWidgets.Add(uiWidget);
            }

            foreach (KeyValuePair<Guid, ScriptDto> kvp in assets.Scripts)
            {
                ScriptDto script = kvp.Value;
                                
                // If the script is associated with a room, don't add it.
                bool addScript = true;

                foreach (RoomDto room in assets.Rooms)
                {
                    if (room.Id == script.OwnerId)
                    {
                        addScript = false;
                    }
                }

                // Don't copy the UI manager, Engine, or Network Handler scripts.
                if (Globals.NetworkHandlerId == script.OwnerId ||
                    Globals.UiManagerId == script.OwnerId ||
                    Globals.EngineScriptId == script.OwnerId)
                {
                    addScript = false;
                }
                
                if (addScript == true)
                {
                    if (projectDto_.Scripts.ContainsKey(script.OwnerId))
                    {
                        projectDto_.Scripts[script.OwnerId] = script;
                    }
                    else
                    {
                        projectDto_.Scripts.Add(script.OwnerId, script);
                    }
                }
            }            
        }
                
        #endregion
    }

    public interface IProjectController
    {
        #region Events

        event ProjectCreateHandler ProjectCreated;
        event ProjectStateChangeHandler ProjectStateChanged;

        event RoomAddHandler RoomAdded;
        event RoomSelectHandler RoomSelected;
        event BeforeRoomDeletedHandler BeforeRoomDeleted;
        event CameraModeChangedHandler CameraModeChanged;
        event EditModeChangedHandler EditModeChanged;
        event SelectionToggleHandler SelectionToggle;
        event RefreshViewHandler RefreshView;
        event RefreshPropertiesHandler RefreshProperties;
        event TileObjectSelectHandler TileObjectSelected;

        event LayerSelectHandler LayerSelect;
        event LayerAddHandler LayerAdd;
        event LayerResizeHandler LayerResize;
        event BeforeLayerDeleteHandler BeforeLayerDelete;
        event AfterLayerDeleteHandler AfterLayerDelete;
        event InteractiveLayerChangeHandler InteractiveLayerChange;
        
        event ActorAddHandler ActorAdd;
        event EventAddHandler EventAdd;
        event HudElementAddHandler HudElementAdd;
        event SpawnPointAddHandler SpawnPointAdd;
        event TileObjectAddHandler TileObjectAdd;
        event TileObjectDeleteHandler TileObjectDelete;
        event TileObjectNameChangeHandler TileObjectNameChange;

        event BeforeMapWidgetDeleteHandler BeforeMapWidgetDelete;
        event MapWidgetSelectionChangeHandler MapWidgetSelectionChange;
        event BeforeMapWidgetAddHandler BeforeMapWidgetAdd;
        event MapWidgetAddHandler MapWidgetAdd;
        
        #endregion

        #region Properties
        bool ChangesMade { get; set; }
        #endregion

        #region Functions
        // Utility Functions

        void CreateNewProject(ProjectDto project, ProjectDto assets);
        
        // Let any controls know that the project has finished loading and preparing. This is not a part of CreateNewProject
        // because it must be called in the UI thread.
        void FinalizeProject();

        // Project
        ProjectDto GetProjectDto();
        ProjectUiStateDto GetUiState();
        // Resources removed in 2.2
        //ProjectResourcesDto GetResources();

        void WriteProjectDtoToStream(Stream stream, bool fullSave);
        ProjectDto ReadProjectDtoFromStream(Stream stream);
        Version ReadProjectVersionNumberFromStream(Stream stream);

        void SetProjectFolder(string projectFolder);

        void SetInitialRoomId(Guid initialRoomId);

        int GetUndoStackSize();
        int GetRedoStackSize();

        void Undo();
        void Redo();

        bool ChangeProjectName(string name);

        // Rooms
        RoomDto AddRoom(string name, string layerName, int layerColumns, int layerRows);
        RoomDto GetRoom(Guid roomId);
        int GetRoomIndexFromId(Guid roomId);
        void SetRoomName(Guid roomId, string name);
        void SetRoomLoadingScreen(Guid roomId, Guid loadingScreenId);
        void SetRoomTransition(Guid roomId, Guid transitionId);
        void SelectRoom(int roomIndex);
        void DeleteRoom(int roomIndex);
        bool IsRoom(Guid id);

        // Layers
        LayerDto AddLayer(int roomIndex, string name, int columns, int rows);
        void SelectLayer(int roomIndex, int layerIndex);
        void SetLayerVisibility(int roomIndex, int layerIndex, bool isVisible);
        void SetLayerName(int roomIndex, int layerIndex, string layerName);
        void SetLayerRows(int roomIndex, int layerIndex, int rows);
        void SetLayerColumns(int roomIndex, int layerIndex, int columns);
        void SetLayerNameRowsColumns(int roomIndex, int layerIndex, string name, int rows, int columns);
        void SetInteractiveLayer(int roomIndex, int layerIndex);
        void DeleteLayer(int roomIndex, int layerIndex);
        void MoveLayer(int roomIndex, int fromOrdinal, int toOrdinal);
        int GetLayerCount(int roomIndex);
        int GetLayerOrdinalFromIndex(int roomIndex, int layerIndex);
        int GetLayerIndexFromOrdinal(int roomIndex, int layerOrdinal);
        int GetLayerIndexFromId(int roomIndex, Guid layerId);
        int GetInteractiveLayerIndex(int roomIndex);
        LayerDto GetLayerById(Guid layerId);
        LayerDto GetLayerByIndex(int roomIndex, int layerIndex);
        LayerDto GetLayerByOrdinal(int roomIndex, int layerOrdinal);
        LayerDto GetInteractiveLayer(int roomIndex);
        
        // Map Widget Selector
        void ClearMapWidgetSelection(int roomIndex);
        void AddMapWidgetsToSelection(int roomIndex, List<Guid> mapWidgetIds);
        void DeleteMapWidgets(List<Guid> ids);
        void RemoveMapWidgetsFromSelection(int roomIndex, List<Guid> mapWidgetIds);
        void SetMapWidgetSelectionOn(int roomIndex, bool selectionOn);
        void SetMapWidgetSelectorCorner1(int roomIndex, int x, int y);
        void SetMapWidgetSelectorCorner2(int roomIndex, int x, int y);
        void SetMapWidgetSelectionLayer(int roomIndex, int layerIndex);
        
        // Tile Sheets
        TileSheetDto AddTileSheet(string imagePath, int tileSize, string name);
        TileSheetDto GetTileSheet(Guid assetId);
        TileSheetDto GetTileSheetByName(string name);
        void DeleteTileSheet(int tileSheetIndex);
        int GetTileSheetIndexFromId(Guid id);
        void SetTileSheetName(Guid tileSheetId, string name);
        void SetTileSheetImagePath(Guid tileSheetId, string name);
        void SetTileSheetScaleFactor(Guid tileSheetId, float scaleFactor);
        void SetTileSheetColumns(Guid tileSheetId, int columns);
        void SetTileSheetRows(Guid tileSheetId, int rows);
        
        // Tile Sheet Objects
        TileObjectDto AddTileObject(Guid tileSheetId, TileObjectDto tileObject);
        void DeleteTileObject(int tileSheetIndex, int tileObjectIndex);
        //void SelectTileObject(int tileSheetIndex, int tileObjectIndex);
        void SelectTileObject(Guid tileObjectId);
        TileObjectDto GetTileObject(int tileSheetIndex, int tileObjectIndex);
        TileObjectDto GetTileObject(Guid tileObjectId);
        void SetTileObjectAnimationId(Guid tileObjectId, Guid animationId);
        void SetTileObjectName(Guid tileObjectId, string name);
        void SetTileObjectColumns(Guid tileObjectId, int columns);
        void SetTileObjectRows(Guid tileObjectId, int rows);
        void SetTileObjectTopLeftCornerColumn(Guid tileObjectId, int column);
        void SetTileObjectTopLeftCornerRow(Guid tileObjectId, int row);
        void SetTileObjectCursorCell(Guid tileObjectId, Point2D cursorCell);

        // Scenery Animations
        SceneryAnimationDto AddSceneryAnimation(Guid tileSheetId, SceneryAnimationDto sceneryAnimation);
        void DeleteSceneryAnimation(int tileSheetIndex, int sceneryAnimationIndex);
        SceneryAnimationDto GetSceneryAnimation(int tileSheetIndex, int sceneryAnimationIndex);
        SceneryAnimationDto GetSceneryAnimation(Guid sceneryAnimationId);
        Guid GetSceneryAnimationIdFromName(Guid ownerId, string sceneryAnimationName);
        List<string> GetSceneryAnimationNames(Guid tileSheetId);
        void SetSceneryAnimationFramesPerSecond(Guid sceneryAnimationId, int framesPerSecond);
        void SetSceneryAnimationName(Guid sceneryAnimationId, string sceneryAnimationName);

        // Sprite Sheets
        SpriteSheetDto AddSpriteSheet(string imagePath, string name);
        SpriteSheetDto GetSpriteSheet(Guid assetId);
        SpriteSheetDto GetSpriteSheetByName(string name);
        void DeleteSpriteSheet(int spriteSheetIndex);
        int GetSpriteSheetIndexFromId(Guid id);
        Guid GetSpriteSheetIdFromName(string name);
        void SetSpriteSheetName(Guid spriteSheetId, string name);
        void SetSpriteSheetImagePath(Guid spriteSheetId, string imagePath);
        void SetSpriteSheetColumns(Guid spriteSheetId, int columns);
        void SetSpriteSheetRows(Guid spriteSheetId, int rows);
        void SetSpriteSheetCellWidth(Guid spriteSheetId, int cellWidth);
        void SetSpriteSheetCellHeight(Guid spriteSheetId, int cellHeight);
        void SetSpriteSheetPadding(Guid spriteSheetId, int padding);
        void SetSpriteSheetScaleFactor(Guid spriteSheetId, float scaleFactor);
        
        // Audio
        AudioAssetDto AddAudioAsset(string audioPath, string name);
        AudioAssetDto GetAudioAsset(Guid assetId);
        AudioAssetDto GetAudioAssetByName(string name);
        void DeleteAudioAsset(int audioAssetIndex);
        int GetAudioAssetIndexFromId(Guid id);
        Guid GetAudioAssetIdFromName(string name);
        void SetAudioAssetName(Guid audioAssetId, string name);
        void SetAudioAssetChannel(Guid audioAssetId, string channel);
        void SetAudioAssetAudioPath(Guid audioAssetId, string audioPath);

        // Loading screens
        LoadingScreenDto AddLoadingScreen(string name);
        LoadingScreenDto GetLoadingScreen(Guid loadingScreenId);
        LoadingScreenDto GetLoadingScreenByName(string name);
        void SetLoadingScreenName(Guid loadingScreenId, string name);
        void DeleteLoadingScreen(int loadingScreenIndex);
        Guid GetLoadingScreenIdFromName(string name);

        // Transitions
        TransitionDto AddTransition(string name);
        TransitionDto GetTransition(Guid transitionId);
        TransitionDto GetTransitionByName(string name);
        void SetTransitionName(Guid transitionId, string name);
        void DeleteTransition(int transitionIndex);
        Guid GetTransitionIdFromName(string name);

        // Particles
        ParticleDto AddParticle(string name);
        ParticleDto GetParticle(Guid particleId);
        void SetParticleName(Guid particleId, string name);
        void DeleteParticle(int particleIndex);
        Guid GetParticleIdFromName(string name);

        // Particle Emitters
        ParticleEmitterDto AddParticleEmitter(string name);
        ParticleEmitterDto GetParticleEmitter(Guid particleEmitterId);
        void SetParticleEmitterName(Guid particleEmitterId, string name);
        void DeleteParticleEmitter(int particleEmitterIndex);
        Guid GetParticleEmitterIdFromName(string name);

        // Entity
        EntityDto GetEntity(Guid entityId);
        bool IsEntity(Guid id);
        
        // Map Widgets
        MapWidgetDto AddMapWidget(MapWidgetCreationParametersDto creationParams);
        List<MapWidgetDto> AddMapWidgets(List<MapWidgetCreationParametersDto> creationParamsList);
        MapWidgetDto GetMapWidget(Guid mapWidgetId, MapWidgetType type);
        MapWidgetDto GetMapWidget(Guid mapWidgetId);
        int GetMapWidgetPropertyIndexFromId(Guid mapWidgetId, Guid propertyId);
        void SetMapWidgetBounds(Guid mapWidgetId, Rectangle bounds);
        void SetMapWidgetName(Guid mapWidgetId, MapWidgetType mapWidgetType, string name);
        void SetMapWidgetPosition(Guid mapWidgetId, MapWidgetType mapWidgetType, Point2D position);
        void SetMapWidgetPosition(Guid mapWidgetId, Point2D position);
        void SetMapWidgetPropertyValue(Guid mapWidgetId, Guid propertyId, object value);
        
        // Spawn Point Widgets
        void SetSpawnPointWidgetIdentity(Guid mapWidgetId, Guid identity);

        // Particle Emitter Widgets
        void SetParticleEmitterWidgetName(Guid mapWidgetId, string name);
        void SetParticleEmitterWidgetParticleType(Guid mapWidgetId, Guid particleTypeId);
        void SetParticleEmitterWidgetBehavior(Guid mapWidgetId, Guid behaviorId);
        void SetParticleEmitterWidgetAnimation(Guid mapWidgetId, Guid animationId);
        void SetParticleEmitterWidgetAnimationFramesPerSecond(Guid mapWidgetId, int animationFramesPerSecond);
        void SetParticleEmitterWidgetAttachParticles(Guid mapWidgetId, bool attachParticles);
        void SetParticleEmitterWidgetInterval(Guid mapWidgetId, double interval);
        void SetParticleEmitterWidgetParticleLifespan(Guid mapWidgetId, double particleLifespan);
        void SetParticleEmitterWidgetActive(Guid mapWidgetId, bool active);
        void SetParticleEmitterWidgetParticlesPerEmission(Guid mapWidgetId, int particlesPerEmission);
        void SetParticleEmitterWidgetMaxParticles(Guid mapWidgetId, int maxParticles);

        // Audio Source Widgets
        void SetAudioSourceWidgetName(Guid mapWidgetId, string name);
        void SetAudioSourceWidgetAudio(Guid mapWidgetId, Guid audioId);
        void SetAudioSourceWidgetAutoplay(Guid mapWidgetId, bool active);
        void SetAudioSourceWidgetLoop(Guid mapWidgetId, bool active);
        void SetAudioSourceWidgetMinDistance(Guid mapWidgetId, int minDistance);
        void SetAudioSourceWidgetMaxDistance(Guid mapWidgetId, int maxDistance);
        void SetAudioSourceWidgetVolume(Guid mapWidgetId, float volume);

        // World Geometry Widgets
        void SetWorldGeometryWidgetCorners(Guid mapWidgetId, Point2D corner1, Point2D corner2);
        void SetWorldGeometryWidgetCollisionStyle(Guid mapWidgetId, WorldGeometryCollisionStyle collisionStyle);
        void SetWorldGeometryWidgetSlopeRise(Guid mapWidgetId, int slopeRise);        

        // Generic Stateful Entity
        StatefulEntityDto GetStatefulEntity(Guid entityId);
        void SetStatefulEntityInitialStateId(Guid entityId, Guid stateId);
        void SetStatefulEntityStageBackgroundDepth(Guid entityId, int stageBackgroundDepth);
        void SetStatefulEntityStageWidth(Guid entityId, int stageWidth);
        void SetStatefulEntityStageHeight(Guid entityId, int stageHeight);
        void SetStatefulEntityStageOriginLocation(Guid entityId, OriginLocation stageOriginLocation);        
        void SetStatefulEntityPivotPoint(Guid entityId, Point pivotPoint);
        
        // Actors
        ActorDto AddActor(string name);
        ActorDto GetActor(Guid actorId);
        ActorDto DuplicateActor(Guid actorId, string name);
        void SelectActor(int actorIndex);
        void DeleteActor(int actorIndex);
        int GetActorIndexFromId(Guid id);
        Guid GetActorIdFromIndex(int index);
        void SetActorName(Guid actorId, string name);
        void SetActorTag(Guid actorId, string tag);
        void SetActorInitialState(Guid actorId, Guid initialStateId);
        void SetActorClassification(Guid actorId, Guid classificationId);
        void SetActorKeepRoomActive(Guid actorId, bool keepRoomActive);

        // Events
        EventDto AddEvent(string name);
        EventDto GetEvent(Guid eventId);
        void SelectEvent(int eventIndex);
        void DeleteEvent(int eventIndex);
        int GetEventIndexFromId(Guid id);
        void SetEventName(Guid eventId, string name);
        void SetEventTag(Guid eventId, string tag);
        void SetEventClassification(Guid eventId, Guid classificationId);

        // HUD Elements
        HudElementDto AddHudElement(string name);
        HudElementDto GetHudElement(Guid hudElementId);
        void SelectHudElement(int hudElementIndex);
        void DeleteHudElement(int hudElementIndex);
        int GetHudElementIndexFromId(Guid id);
        void SetHudElementName(Guid hudElementId, string name);
        void SetHudElementTag(Guid hudElementId, string tag);
        void SetHudElementInitialState(Guid hudElementId, Guid initialStateId);
        void SetHudElementClassification(Guid hudElementId, Guid classificationId);

        // Spawn Points
        SpawnPointDto AddSpawnPoint(string name);
        SpawnPointDto GetSpawnPoint(Guid spawnPointDtoId);
        void SelectSpawnPoint(int spawnPointIndex);
        void DeleteSpawnPoint(int spawnPointDtoIndex);
        void SetSpawnPointName(Guid spawnPointDtoId, string name);
        Guid GetSpawnPointIdFromName(string name);
        string GetSpawnPointNameFromId(Guid id);
        
        // Animations
        AnimationDto AddAnimation(Guid groupId, string name);
        AnimationDto GetAnimation(Guid animationId);
        Guid GetAnimationIdFromName(string name);
        void DeleteAnimation(Guid animationId);
        int GetAnimationIndexFromId(Guid animationId);
        void SetAnimationAlphaMaskSheet(Guid animationId, Guid alphaMaskSheetId);
        void SetAnimationName(Guid animationId, string name);
        void SetAnimationSpriteSheet(Guid animationId, Guid spriteSheetId);

        // Animation Groups
        AnimationGroupDto AddAnimationGroup(string name);
        AnimationGroupDto GetAnimationGroup(Guid animationGroupId);
        Guid GetAnimationGroupIdFromName(string name);
        void DeleteAnimationGroup(Guid animationGroupId);
        void SetAnimationGroupName(Guid animationGroupId, string name);

        // States
        StateDto AddState(Guid entityId, string name);
        StateDto GetState(Guid stateId);
        Guid GetStateOwnerId(Guid stateId);
        void DeleteState(Guid stateId);
        void SetStateName(Guid stateId, string name);
        Guid GetStateIdFromName(Guid ownerId, string stateName);
        List<string> GetStateNames(Guid entityId);

        // Frames
        FrameDto AddFrame(Guid animationId);
        FrameDto AddFrame(Guid animationId, int sheetCellIndex);
        FrameDto GetFrame(Guid frameId);
        void SetFrameAlphaMaskCellIndex(Guid frameId, int? alphaMaskCellIndex);
        void SetFrameSpriteSheetCellIndex(Guid frameId, int? spriteSheetCellIndex);
        void DeleteFrame(Guid frameId);

        // Frame Triggers
        FrameTriggerDto AddFrameTrigger(Guid frameId);
        FrameTriggerDto GetFrameTrigger(Guid frameTriggerId);
        void SetTriggerSignal(Guid frameTriggerId, Guid triggerSignalId);
        void DeleteFrameTrigger(Guid frameTriggerId);

        // Action Points
        ActionPointDto AddActionPoint(Guid frameId);
        ActionPointDto GetActionPoint(Guid actionPointId);
        void SetActionPointName(Guid actionPointId, string name);
        void SetActionPointPosition(Guid actionPointId, Point2D position);
        void SetActionPointPositionLeft(Guid actionPointId, int left);
        void SetActionPointPositionTop(Guid actionPointId, int top);
        void DeleteActionPoint(Guid actionPointId);

        // Hitboxes
        HitboxDto AddHitbox(Guid ownerId, Guid rootOwnerId);
        void AddHitbox(HitboxDto hitbox);
        HitboxDto GetHitbox(Guid hitboxId);
        void DeleteHitbox(Guid hitboxId);
        void SetHitboxRect(Guid hitboxId, Rectangle hitboxRect);
        void SetHitboxRectLeft(Guid hitboxId, int left);
        void SetHitboxRectTop(Guid hitboxId, int top);
        void SetHitboxRectWidth(Guid hitboxId, int width);
        void SetHitboxRectHeight(Guid hitboxId, int height);
        void SetHitboxCornerPoint1(Guid ownerId, int hitboxIndex, int x, int y);
        void SetHitboxCornerPoint2(Guid ownerId, int hitboxIndex, int x, int y);
        void SetHitboxCornerPoints(Guid ownerId, int hitboxIndex, int x1, int y1, int x2, int y2);
        void SetHitboxIdentity(Guid hitboxId, Guid hitboxIdentityId);
        void SetHitboxIsSolid(Guid hitboxId, bool isSolid);
        void SetHitboxPriority(Guid hitboxId, HitboxPriority priority);
        void SetHitboxRotationDegrees(Guid hitboxId, float rotation);

        // Animation Slots
        AnimationSlotDto AddAnimationSlot(Guid stateId);
        AnimationSlotDto GetAnimationSlot(Guid animationSlotId);
        void DeleteAnimationSlot(Guid animationSlotId);
        void SetAnimationSlotName(Guid animationSlotId, string name);
        void SetAnimationSlotAlphaGradientDirection(Guid animationSlotId, GradientDirection direction);
        void SetAnimationSlotAlphaGradientFrom(Guid animationSlotId, float alphaIntensity);
        void SetAnimationSlotAlphaGradientTo(Guid animationSlotId, float alphaIntensity);
        void SetAnimationSlotAlphaGradientRadialCenter(Guid animationSlotId, Point radialCenterPoint);
        void SetAnimationSlotAlphaGradientRadius(Guid animationSlotId, float radius);
        void SetAnimationSlotAnimation(Guid animationSlotId, Guid animationId);
        void SetAnimationSlotOriginLocation(Guid animationSlotId, OriginLocation originLocation);
        void SetAnimationSlotPositionLeft(Guid animationSlotId, int positionLeft);
        void SetAnimationSlotPositionTop(Guid animationSlotId, int positionTop);
        void SetAnimationSlotBackgroundFlag(Guid animationSlotId, bool backgroundFlag);
        void SetAnimationSlotBlendColorRed(Guid animationSlotId, float red);
        void SetAnimationSlotBlendColorGreen(Guid animationSlotId, float green);
        void SetAnimationSlotBlendColorBlue(Guid animationSlotId, float blue);
        void SetAnimationSlotBlendColorAlpha(Guid animationSlotId, float alpha);
        void SetAnimationSlotBlendColorPercent(Guid animationSlotId, float percent);
        void SetAnimationSlotHueColorRed(Guid animationSlotId, float red);
        void SetAnimationSlotHueColorGreen(Guid animationSlotId, float green);
        void SetAnimationSlotHueColorBlue(Guid animationSlotId, float blue);
        void SetAnimationSlotHueColorAlpha(Guid animationSlotId, float alpha);
        void SetAnimationSlotNextStateId(Guid animationSlotId, Guid stateId);
        void SetAnimationSlotOutlineColorRed(Guid animationSlotId, float red);
        void SetAnimationSlotOutlineColorGreen(Guid animationSlotId, float green);
        void SetAnimationSlotOutlineColorBlue(Guid animationSlotId, float blue);
        void SetAnimationSlotOutlineColorAlpha(Guid animationSlotId, float alpha);
        void SetAnimationSlotPivotPoint(Guid animationSlotId, Point pivotPoint);
        void SetAnimationSlotStyle(Guid animationSlotId, AnimationStyle animationStyle);
        void SetAnimationSlotFramesPerSecond(Guid animationSlotId, int framesPerSecond);

        void MoveUpAnimationSlot(Guid stateId, int index);
        void MoveDownAnimationSlot(Guid stateId, int index);

        // Hitbox Identities
        HitboxIdentityDto AddHitboxIdentity(string name);
        HitboxIdentityDto GetHitboxIdentity(Guid hitboxIdentityId);
        void DeleteHitboxIdentity(int hitboxIdentityIndex);
        void SetHitboxIdentityName(Guid hitboxIdentityId, string name);
        Guid GetHitboxIdentityIdFromName(string name);

        // Trigger Signals
        TriggerSignalDto AddTriggerSignal(string name);
        TriggerSignalDto GetTriggerSignal(Guid triggerSignalId);
        void DeleteTriggerSignal(int triggerSignalIndex);
        void SetTriggerSignalName(Guid hitboxIdentityId, string name);
        Guid GetTriggerSignalIdFromName(string name);

        // Properties
        PropertyDto AddProperty(Guid entityId, string name);
        PropertyDto GetProperty(Guid propertyId);
        void DeleteProperty(Guid propertyId);
        void SetPropertyName(Guid propertyId, string name);
        void SetPropertyDefaultValue(Guid propertyId, string defaultValue);

        // Game Buttons
        GameButtonDto AddGameButton(string name);
        GameButtonDto GetGameButton(Guid gameButtonId);
        void DeleteGameButton(int gameButtonIndex);
        void SetGameButtonName(Guid gameButtonId, string name);
        void SetGameButtonGroup(Guid gameButtonId, Guid gameButtonGroupId);
        void SetGameButtonLabel(Guid gameButtonId, string label);

        // Game Buttons Groups
        GameButtonGroupDto AddGameButtonGroup(string name);
        GameButtonGroupDto GetGameButtonGroup(Guid gameButtonGroupId);
        void DeleteGameButtonGroup(int gameButtonGroupIndex);
        void SetGameButtonGroupName(Guid gameButtonGroupId, string name);
        Guid GetGameButtonGroupIdFromName(string name);
        
        // Scripts
        ScriptDto AddScript(string name, ScriptType scriptType);
        ScriptDto GetScript(Guid scriptId);
        ScriptDto GetScriptByOwnerId(Guid ownerId);
        void DeleteScript(Guid scriptId);
        void SetScriptPath(Guid scriptId, string path);
        void SetScriptName(Guid scriptId, string name);
        int ReplaceScriptFolder(string oldValue, string newValue);

        // Data Files
        DataFileDto GetDataFile(Guid dataFileId);      
        void SetDataFilePath(Guid dataFileId, string path);
                
        // UI Widgets
        UiWidgetDto AddUiWidget(string name);
        UiWidgetDto GetUiWidget(Guid uiWidgetId);
        void DeleteUiWidget(int uiWidgetIndex);
        void SetUiWidgetName(Guid uiWidgetId, string name);
        bool IsUiWidget(Guid id);

        // UI state
        void HideMouse();
        void SetCameraMode(CameraMode cameraMode);
        void SetCanSelectMapWidget(MapWidgetType mapWidgetType, bool canSelect);
        void SetEditMode(EditMode editMode);
        void SetMapWidgetMode(int roomIndex, MapWidgetMode mapWidgetModeMode);
        void SetSelectedMapWidgetType(int roomIndex, MapWidgetType mapWidgetType);
        void SetSelectedTileIndex(int roomIndex, int tileIndex);
        void SetShowCameraOutline(bool value);
        void SetShowGrid(bool value);
        void SetShowOutlines(bool value);
        void ShowWorldGeometry(bool value);
        void ShowMouse();
        void ToggleGrid();
        void ToggleCameraOutline();
        void ToggleOutlines();
        void ToggleTransparentSelect();
        void ToggleMouse();

        // External Resources        
        BitmapResourceDto GetBitmapResource(Guid resourceId, bool loadFromDisk);
        void LoadAudio(Guid resourceId);
        void LoadBitmap(Guid resourceId);
        int ReplaceResourceFolder(string oldValue, string newValue);
        void UnloadAudioResource(Guid resourceId);
        void UnloadBitmapResource(Guid resourceId, EditorModule module);

        #endregion
    }

    #region Event Args
    
    public class ProjectCreatedEventArgs : System.EventArgs
    {
        public ProjectCreatedEventArgs(bool newProject)
        {
            newProject_ = newProject;
        }

        private bool newProject_;
        public bool NewProject
        {
            get { return newProject_; }
        }
    }

    public class ProjectStateChangedEventArgs : System.EventArgs
    {
        public ProjectStateChangedEventArgs()
        {
        }        
    }

    public class RoomAddedEventArgs : System.EventArgs
    {
        public RoomAddedEventArgs(int index)
        {
            index_ = index;
        }

        private int index_;
        public int Index
        {
            get
            {
                return index_;
            }
        }
    }

    public class CameraModeChangedEventArgs : System.EventArgs
    {
        public CameraModeChangedEventArgs(CameraMode mode)
        {
            mode_ = mode;
        }

        private CameraMode mode_;
        public CameraMode CameraMode
        {
            get { return mode_; }
        }
    }

    public class EditModeChangedEventArgs : System.EventArgs
    {
        public EditModeChangedEventArgs(EditMode mode)
        {
            mode_ = mode;
        }

        private EditMode mode_;
        public EditMode EditMode
        {
            get { return mode_; }
        }
    }

    public class SelectionToggleEventArgs : System.EventArgs
    {
        public SelectionToggleEventArgs(bool selectToggle, bool tileSelection)
        {
            selectionOn_ = selectToggle;
            tileSelection_ = tileSelection;
        }

        private bool selectionOn_;
        public bool SelectionOn
        {
            get { return selectionOn_; }
        }

        private bool tileSelection_;
        public bool TileSelection
        {
            get { return tileSelection_; }
        }
    }

    public class CursorChangedEventArgs : System.EventArgs
    {
        public CursorChangedEventArgs(Cursor cursor)
        {
            cursor_ = cursor;
        }

        private Cursor cursor_;
        public Cursor Cursor
        {
            get { return cursor_; }
        }
    }

    public class RefreshViewEventArgs : System.EventArgs
    {
        public RefreshViewEventArgs()
        {
        }
    }

    public class RefreshPropertiesEventArgs : System.EventArgs
    {
        public RefreshPropertiesEventArgs()
        {
        }
    }

    public class LayerSelectedEventArgs : System.EventArgs
    {
        public LayerSelectedEventArgs(Guid layerId)
        {
            layerId_ = layerId;
        }

        private Guid layerId_;
        public Guid LayerID
        {
            get { return layerId_; }
        }
    }

    public class BeforeLayerDeletedEventArgs : System.EventArgs
    {
        public BeforeLayerDeletedEventArgs(Guid layerId)
        {
            layerId_ = layerId;
        }

        private Guid layerId_;
        public Guid LayerID
        {
            get { return layerId_; }
        }
    }

    public class AfterLayerDeletedEventArgs : System.EventArgs
    {
        public AfterLayerDeletedEventArgs(Guid layerId)
        {
            layerId_ = layerId;
        }

        private Guid layerId_;
        public Guid LayerID
        {
            get { return layerId_; }
        }
    }

    public class InteractiveLayerChangedEventArgs : System.EventArgs
    {
        public InteractiveLayerChangedEventArgs(int oldInteractiveIndex, int newInteractiveIndex)
        {
            oldInteractiveIndex_ = oldInteractiveIndex;
            newInteractiveIndex_ = newInteractiveIndex;
        }

        private int oldInteractiveIndex_;
        public int OldInteractiveIndex
        {
            get { return oldInteractiveIndex_; }
        }

        private int newInteractiveIndex_;
        public int NewInteractiveIndex
        {
            get { return newInteractiveIndex_; }
        }
    }
    
    public class RoomSelectedEventArgs : System.EventArgs
    {
        public RoomSelectedEventArgs(int index)
        {
            index_ = index;
        }

        private int index_;
        public int Index
        {
            get { return index_; }
        }
    }

    public class BeforeRoomDeletedEventArgs : System.EventArgs
    {
        public BeforeRoomDeletedEventArgs(int index)
        {
            index_ = index;
        }

        private int index_;
        public int Index
        {
            get { return index_; }
        }
    }
    
    public class BeforeMapWidgetAddedEventArgs : System.EventArgs
    {
        public BeforeMapWidgetAddedEventArgs(Guid id, MapWidgetType type)
        {
            id_ = id;
            type_ = type;
        }
        
        private Guid id_;
        public Guid Id
        {
            get { return id_; }
        }

        private MapWidgetType type_;
        public MapWidgetType Type
        {
            get { return type_; }
        }
    }

    public class MapWidgetAddedEventArgs : System.EventArgs
    {
        public MapWidgetAddedEventArgs(Guid id, MapWidgetType type)
        {
            id_ = id;
            type_ = type;
        }
        
        private Guid id_;
        public Guid Id
        {
            get { return id_; }
        }

        private MapWidgetType type_;
        public MapWidgetType Type
        {
            get { return type_; }
        }
    }
    
    public class BeforeMapWidgetDeleteEventArgs : System.EventArgs
    {
        public BeforeMapWidgetDeleteEventArgs(MapWidgetType type)
        {
            type_ = type;
        }

        private MapWidgetType type_;
        public MapWidgetType Type
        {
            get { return type_; }
        }
    }
    public class ActorAddedEventArgs : System.EventArgs
    {
        public ActorAddedEventArgs(Guid actorId)
        {
            actorId_ = actorId;
        }

        private Guid actorId_;
        public Guid ActorId
        {
            get { return actorId_; }
        }
    }

    public class EventAddedEventArgs : System.EventArgs
    {
        public EventAddedEventArgs(Guid eventId)
        {
            eventId_ = eventId;
        }

        private Guid eventId_;
        public Guid EventId
        {
            get { return eventId_; }
        }
    }

    public class HudElementAddedEventArgs : System.EventArgs
    {
        public HudElementAddedEventArgs(Guid hudElementId)
        {
            hudElementId_ = hudElementId;
        }

        private Guid hudElementId_;
        public Guid HudElementId
        {
            get { return hudElementId_; }
        }
    }

    public class SpawnPointAddedEventArgs : System.EventArgs
    {
        public SpawnPointAddedEventArgs(Guid spawnPointId)
        {
            spawnPointId_ = spawnPointId;
        }

        private Guid spawnPointId_;
        public Guid SpawnPointId
        {
            get { return spawnPointId_; }
        }
    }

    public class TileObjectAddedEventArgs : System.EventArgs
    {
        public TileObjectAddedEventArgs(Guid tileSheetId, Guid tileObjectId)
        {
            tileSheetId_ = tileSheetId;

            tileObjectId_ = tileObjectId;
        }

        public Guid TileObjectId
        {
            get { return tileObjectId_; }
        }
        private Guid tileObjectId_;

        public Guid TileSheetId
        {
            get { return tileSheetId_; }
        }
        private Guid tileSheetId_;
    }

    public class TileObjectDeletedEventArgs : System.EventArgs
    {
        public TileObjectDeletedEventArgs(int tileSheetIndex, int tileObjectIndex)
        {
            tileSheetIndex_ = tileSheetIndex;

            tileObjectIndex_ = tileObjectIndex;
        }

        public int TileObjectIndex
        {
            get { return tileObjectIndex_; }
        }
        private int tileObjectIndex_;

        public int TileSheetIndex
        {
            get { return tileSheetIndex_; }
        }
        private int tileSheetIndex_;
    }

    public class TileObjectNameChangedEventArgs : System.EventArgs
    {
        public TileObjectNameChangedEventArgs(Guid tileObjectId, string name, string oldName)
        {
            name_ = name;

            oldName_ = oldName;

            tileObjectId_ = tileObjectId;
        }

        public Guid TileObjectId
        {
            get { return tileObjectId_; }
        }
        private Guid tileObjectId_;

        public string Name
        {
            get { return name_; }
        }
        private string name_;
        
        public string OldName
        {
            get { return oldName_; }
        }
        private string oldName_;
    }
    
    public class MapWidgetSelectionChangedEventArgs : System.EventArgs
    {
        public MapWidgetSelectionChangedEventArgs(int roomIndex, bool isSelected, List<Guid> mapWidgetIds)
        {
            roomIndex_ = roomIndex;
            isSelected_ = isSelected;
            lstMapWidgetIds = mapWidgetIds;
        }

        private int roomIndex_;
        public int RoomIndex
        {
            get { return roomIndex_; }
        }

        private bool isSelected_;
        public bool IsSelected
        {
            get { return isSelected_; }
        }

        private List<Guid> lstMapWidgetIds;
        public List<Guid> MapWidgetIds
        {
            get { return lstMapWidgetIds; }
        }
    }

    public class TileObjectSelectedEventArgs : System.EventArgs
    {
        // Fields
        private Guid tileObjectId_;

        // Constructor
        public TileObjectSelectedEventArgs(Guid tileObjectId)
        {
            tileObjectId_ = tileObjectId;
        }

        // Properties
        public Guid TileObjectId
        {
            get { return tileObjectId_; }
        }
        
        //// Fields
        //private int tileObjectIndex_;
        //private int tileSheetIndex_;

        //// Constructor
        //public TileObjectSelectedEventArgs(int tileSheetIndex, int tileObjectIndex)
        //{
        //    tileObjectIndex_ = tileObjectIndex;
        //    tileSheetIndex_ = tileSheetIndex;
        //}

        //// Properties
        //public int TileObjectIndex
        //{
        //    get { return tileObjectIndex_; }
        //}

        //public int TileSheetIndex
        //{
        //    get { return tileSheetIndex_; }
        //}
    }
    
    public class MapWidgetComparer : IComparer<MapWidgetDto>
    {
        public int Compare(MapWidgetDto x, MapWidgetDto y)
        {
            if (x.Name == null)
            {
                if (y.Name == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y.Name == null)
                {
                    return 1;
                }
                else
                {
                    return x.Name.CompareTo(y.Name);
                }
            }
        }
    }
    #endregion
}