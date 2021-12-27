/* -------------------------------------------------------------------------
** ProjectDto.cs
**
** The ProjectDto class contains all of the data objects that make up a 
** project file. When serialized/deserialized it can be saved to disk, or
** stored in a stack to save and/or restore the project state.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FiremelonEditor2
{
    // Type shorteners
    using RoomList = List<RoomDto>;
    using LayerLists = Dictionary<Guid, List<LayerDto>>;
    using TileObjectLists = Dictionary<Guid, List<TileObjectDto>>;
    using Indexes = Dictionary<Guid, int>;
    using TileSheetList = List<TileSheetDto>;
    using SpriteSheetList = List<SpriteSheetDto>;
    using AudioList = List<AudioAssetDto>;
    using LoadingScreenList = List<LoadingScreenDto>;
    using TransitionList = List<TransitionDto>;
    using ParticleList = List<ParticleDto>;
    using ParticleEmitterList = List<ParticleEmitterDto>;
    using SpawnPointList = List<SpawnPointDto>;
    using MapWidgetsByType = Dictionary<MapWidgetType, Dictionary<Guid, MapWidgetDto>>;
    using MapWidgetsByLayer = Dictionary<Guid, Dictionary<Guid, MapWidgetDto>>;
    using ActorList = List<ActorDto>;
    using EventList = List<EventDto>;
    using HudElementList = List<HudElementDto>;
    using AnimationList = Dictionary<Guid, List<AnimationDto>>;
    using AnimationGroups = List<AnimationGroupDto>;
    using StateLists = Dictionary<Guid, List<StateDto>>;
    using FrameLists = Dictionary<Guid, List<FrameDto>>;
    using FrameTriggerLists = Dictionary<Guid, List<FrameTriggerDto>>;
    using TriggerSignalList = List<TriggerSignalDto>;
    using ActionPointLists = Dictionary<Guid, List<ActionPointDto>>;
    using HitboxIdenityList = List<HitboxIdentityDto>;
    using HitboxLists = Dictionary<Guid, List<HitboxDto>>;
    using AnimSlotLists = Dictionary<Guid, List<AnimationSlotDto>>;
    using PropertyLists = Dictionary<Guid, List<PropertyDto>>;
    using Scripts = Dictionary<Guid, ScriptDto>;
    using DataFiles = Dictionary<Guid, DataFileDto>;
    using MapWidgetPropertiesDict = Dictionary<Guid, MapWidgetProperties>;
    using GameButtonList = List<GameButtonDto>;
    using GameButtonGroupList = List<GameButtonGroupDto>;
    using UiWidgetList = List<UiWidgetDto>;
    using SceneryAnimationList = Dictionary<Guid, List<SceneryAnimationDto>>;

    public class ProjectDto_2_2 : BaseProjectDto
    {
        public ProjectDto_2_2()
        {
            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                dictMapWidgets_.Add(mapWidgetType, new Dictionary<Guid, MapWidgetDto>());
            }

            AnimationGroupDto noneGroup = new AnimationGroupDto();

            noneGroup.Id = Guid.Empty;

            noneGroup.Name = "None";

            AnimationGroups.Add(noneGroup);

            Animations.Add(Guid.Empty, new List<AnimationDto>());
        }

        // Specifies the file version, so it can be read and written correctly.
        public override Version FileVersion
        {
            get { return fileVersion_; }
        }
        private Version fileVersion_ = new Version(2, 2, 0, 0);


        // Indicates whether the project has been prepared and is safe to be used. This is needed because
        // when loading a project asynchronously, there will be a brief period where the project object is created,
        // but not fully processed yet.
        public bool IsPrepared
        {
            get { return isPrepared_; }
            set { isPrepared_ = value; }
        }
        private bool isPrepared_ = false;

        public string ProjectName
        {
            get { return projectName_; }
            set { projectName_ = value; }
        }
        private string projectName_ = string.Empty;

        public string ProjectFolderFullPath
        {
            get { return projectFolderFullPath_; }
            set { projectFolderFullPath_ = value; }
        }
        private string projectFolderFullPath_ = string.Empty;

        public string ProjectFolderRelativePath
        {
            get { return projectFolderRelativePath_; }
            set { projectFolderRelativePath_ = value; }
        }
        private string projectFolderRelativePath_ = string.Empty;

        public int TileSize
        {
            get { return tileSize_; }
            set { tileSize_ = value; }
        }
        private int tileSize_;

        // The size of the camera viewport, and the game window's resolution.
        public int CameraHeight
        {
            get { return cameraSize_.Height; }
            set { cameraSize_.Height = value; }
        }

        public int CameraWidth
        {
            get { return cameraSize_.Width; }
            set { cameraSize_.Width = value; }
        }
        private Size cameraSize_ = new Size(0, 0);

        // Assets
        public TileSheetList TileSheets
        {
            get { return lstTileSheets_; }
        }
        private TileSheetList lstTileSheets_ = new TileSheetList();

        public TileObjectLists TileObjects
        {
            get { return dictTileObjects_; }
        }
        private TileObjectLists dictTileObjects_ = new TileObjectLists();

        public SpriteSheetList SpriteSheets
        {
            get { return lstSpriteSheets_; }
        }
        private SpriteSheetList lstSpriteSheets_ = new SpriteSheetList();

        public AudioList AudioAssets
        {
            get { return lstAudioAssets_; }
        }
        private AudioList lstAudioAssets_ = new AudioList();

        public LoadingScreenList LoadingScreens
        {
            get { return lstLoadingScreens_; }
        }
        private LoadingScreenList lstLoadingScreens_ = new LoadingScreenList();

        public TransitionList Transitions
        {
            get { return lstTransitions_; }
        }
        private TransitionList lstTransitions_ = new TransitionList();

        public ParticleList Particles
        {
            get { return lstParticles_; }
        }
        private ParticleList lstParticles_ = new ParticleList();

        public ParticleEmitterList ParticleEmitters
        {
            get { return lstParticleEmitters_; }
        }
        private ParticleEmitterList lstParticleEmitters_ = new ParticleEmitterList();

        public SpawnPointList SpawnPoints
        {
            get { return lstSpawnPoints_; }
        }
        private SpawnPointList lstSpawnPoints_ = new SpawnPointList();

        public MapWidgetsByType MapWidgets
        {
            get { return dictMapWidgets_; }
        }
        private MapWidgetsByType dictMapWidgets_ = new MapWidgetsByType();

        public MapWidgetsByLayer MapWidgetsByLayer
        {
            get { return dictMapWidgetsByLayer_; }
        }
        private MapWidgetsByLayer dictMapWidgetsByLayer_ = new MapWidgetsByLayer();

        public ActorList Actors
        {
            get { return lstActors_; }
        }
        private ActorList lstActors_ = new ActorList();

        public EventList Events
        {
            get { return lstEvents_; }
        }
        private EventList lstEvents_ = new EventList();

        public HudElementList HudElements
        {
            get { return lstHudElements_; }
        }
        private HudElementList lstHudElements_ = new HudElementList();

        public AnimationList Animations
        {
            get { return dictAnimations_; }
        }
        private AnimationList dictAnimations_ = new AnimationList();

        public AnimationGroups AnimationGroups
        {
            get { return dictAnimationGroups_; }
        }
        private AnimationGroups dictAnimationGroups_ = new AnimationGroups();

        // Dictionary of states. 
        // The key is the owner stateful entity ID.
        public StateLists States
        {
            get { return dictStates_; }
        }
        private StateLists dictStates_ = new StateLists();

        // Dictionary of animation frames. 
        // The key is the owner animation ID.
        public FrameLists Frames
        {
            get { return dictFrames_; }
        }
        private FrameLists dictFrames_ = new FrameLists();

        // Dictionary of frames triggers. 
        // The key is the owner frame ID.
        public FrameTriggerLists FrameTriggers
        {
            get { return dictFrameTriggers_; }
        }
        private FrameTriggerLists dictFrameTriggers_ = new FrameTriggerLists();

        // Dictionary of frame trigger signals. 
        public TriggerSignalList TriggerSignals
        {
            get { return lstTriggerSignals_; }
        }
        private TriggerSignalList lstTriggerSignals_ = new TriggerSignalList();

        // Dictionary of action points. 
        // The key is the owner frame ID.
        public ActionPointLists ActionPoints
        {
            get { return dictActionPoints_; }
        }
        private ActionPointLists dictActionPoints_ = new ActionPointLists();

        // Dictionary of hitbox identities.
        public HitboxIdenityList HitboxIdentities
        {
            get { return lstHitboxIdentities_; }
        }
        private HitboxIdenityList lstHitboxIdentities_ = new HitboxIdenityList();

        // Dictionary of hitboxes. 
        // The key is the owner animation or state ID.
        public HitboxLists Hitboxes
        {
            get { return dictHitboxes_; }
        }
        private HitboxLists dictHitboxes_ = new HitboxLists();

        // Dictionary of animation slots. 
        // The key is the owner state ID.
        public AnimSlotLists AnimationSlots
        {
            get { return dictAnimationSlots_; }
        }
        private AnimSlotLists dictAnimationSlots_ = new AnimSlotLists();

        // Dictionary of properties. 
        // The key is the owner entity ID.
        public PropertyLists Properties
        {
            get { return dictProperties_; }
        }
        private PropertyLists dictProperties_ = new PropertyLists();


        public SceneryAnimationList SceneryAnimations
        {
            get { return dictSceneryAnimations_; }
        }
        private SceneryAnimationList dictSceneryAnimations_ = new SceneryAnimationList();


        // Dictionary of scripts. 
        // The key is the owner entity ID.
        public Scripts Scripts
        {
            get { return dictScripts_; }
        }
        private Scripts dictScripts_ = new Scripts();

        // Dictionary of data files. 
        // The key is the owner entity ID.
        public DataFiles DataFiles
        {
            get { return dictDataFiles_; }
        }
        private DataFiles dictDataFiles_ = new DataFiles();

        public GameButtonList GameButtons
        {
            get { return lstGameButtons_; }
        }
        private GameButtonList lstGameButtons_ = new GameButtonList();

        public GameButtonGroupList GameButtonGroups
        {
            get { return lstGameButtonGroups_; }
        }
        private GameButtonGroupList lstGameButtonGroups_ = new GameButtonGroupList();

        public UiWidgetList UiWidgets
        {
            get { return lstUiWidgets_; }
        }
        private UiWidgetList lstUiWidgets_ = new UiWidgetList();

        // The rooms contained in this project file.
        public RoomList Rooms
        {
            get { return rooms_; }
        }
        private RoomList rooms_ = new RoomList();

        // The ID of the initial room.
        public Guid InitialRoomId
        {
            get { return initialRoomId_; }
            set { initialRoomId_ = value; }
        }
        private Guid initialRoomId_ = Guid.NewGuid();

        // A dictionary of layers. The key is the room ID.
        public LayerLists Layers
        {
            get { return dictLayers_; }
        }
        private LayerLists dictLayers_ = new LayerLists();

        // A list of lists of interactive layer indexes. Each entry in the 
        // list corresponds to the room with the same index.
        public Indexes InteractiveLayerIndexes
        {
            get { return lstInteractiveLayerIndexList_; }
        }
        private Indexes lstInteractiveLayerIndexList_ = new Indexes();

        // Dictionary of entity instances properties. 
        // The key is the owner entity instance ID.
        public MapWidgetPropertiesDict MapWidgetProperties
        {
            get { return dictMapWidgetProperties_; }
        }
        private MapWidgetPropertiesDict dictMapWidgetProperties_ = new MapWidgetPropertiesDict();
        
        private Dictionary<Guid, BitmapResourceDto> dictBitmaps_ = new Dictionary<Guid, BitmapResourceDto>();
        public Dictionary<Guid, BitmapResourceDto> Bitmaps
        {
            get { return dictBitmaps_; }
        }

        private Dictionary<Guid, AudioResourceDto> dictAudioData_ = new Dictionary<Guid, AudioResourceDto>();
        public Dictionary<Guid, AudioResourceDto> AudioData
        {
            get { return dictAudioData_; }
        }                
    }
}
