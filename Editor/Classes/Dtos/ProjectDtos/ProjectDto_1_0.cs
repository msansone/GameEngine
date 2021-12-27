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
    using Layers = Dictionary<Guid, LayerDto>;
    using Indexes = Dictionary<Guid, int>;
    using Tilesets = Dictionary<Guid, TilesetDto>;
    using TileSheetList = List<TileSheetDto>;
    using TilePropertiesLists = Dictionary<Guid, List<TilePropertiesDto>>;
    using SpriteSheetList = List<SpriteSheetDto>;
    using AudioList = List<AudioAssetDto>;
    using LoadingScreenList = List<LoadingScreenDto>;
    using TransitionList = List<TransitionDto>;
    using ParticleList = List<ParticleDto>;
    using ParticleEmitterList = List<ParticleEmitterDto>;
    using EntityClassificationList = List<EntityClassificationDto>;
    using SpawnPointList = List<SpawnPointDto>;
    using MapWidgetsByType = Dictionary<MapWidgetType, Dictionary<Guid, MapWidgetDto>>;
    using ActorList = List<ActorDto>;
    using EventList = List<EventDto>;
    using HudElementList = List<HudElementDto>;
    using AnimationList = List<AnimationDto>;
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
    using EntityInstances = Dictionary<Guid, EntityInstanceDto>;
    using MapWidgetPropertiesDict = Dictionary<Guid, MapWidgetProperties>;
    using GameButtonList = List<GameButtonDto>;
    using GameButtonGroupList = List<GameButtonGroupDto>;
    using QueryList = List<QueryDto>;
    using MenuBookList = List<MenuBookDto>;
    using MenuPageList = List<MenuPageDto>;
    using UiWidgetList = List<UiWidgetDto>;

    public class ProjectDto_1_0 : BaseProjectDto
    {
        private MapWidgetsByType dictMapWidgets = new MapWidgetsByType();

        public ProjectDto_1_0()
        {
            dictMapWidgets.Add(MapWidgetType.SpawnPoint, new Dictionary<Guid, MapWidgetDto>());
            dictMapWidgets.Add(MapWidgetType.ParticleEmitter, new Dictionary<Guid, MapWidgetDto>());
            dictMapWidgets.Add(MapWidgetType.AudioSource, new Dictionary<Guid, MapWidgetDto>());
        }

        // Specifies the file version, so it can be loaded correctly.
        public override Version FileVersion
        {
            get { return fileVersion_; }
        }
        private Version fileVersion_ = new Version(1, 0, 0, 0);

        // Indicates whether the project has been prepared and is safe to be used. This is needed because
        // when loading a project asynchronously, there will be a brief period where the project object is created,
        // but not fully processed yet.
        private bool isPrepared_ = false;
        public bool IsPrepared
        {
            get { return isPrepared_; }
            set { isPrepared_ = value; }
        }

        // A list of tilesets. Each entry in the outer list corresponds to 
        // the room with the same index.
        private Tilesets lstTilesets_ = new Tilesets();
        public Tilesets Tilesets
        {
            get { return lstTilesets_; }
        }

        private int tileSize_;
        public int TileSize
        {
            get { return tileSize_; }
            set { tileSize_ = value; }
        }

        private string projectName_ = string.Empty;
        public string ProjectName
        {
            get { return projectName_; }
            set { projectName_ = value; }
        }

        private string projectFolderFullPath_ = string.Empty;
        public string ProjectFolderFullPath
        {
            get { return projectFolderFullPath_; }
            set { projectFolderFullPath_ = value; }
        }

        private string projectFolderRelativePath_ = string.Empty;
        public string ProjectFolderRelativePath
        {
            get { return projectFolderRelativePath_; }
            set { projectFolderRelativePath_ = value; }
        }

        // The size of the camera viewport, and the game window's resolution.
        private Size cameraSize_ = new Size(0, 0);
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

        // The collision tilesheet exists separate from the user defined tilesheets.
        // It will be initialized to either the 16x16 or 32x32 tiles, based on the project settings.
        private Guid collisionTileSheetId_ = Guid.NewGuid();
        public Guid CollisionTileSheetId
        {
            get { return collisionTileSheetId_; }
        }

        private CollisionTileSheetDto collisionTileSheet_;
        public CollisionTileSheetDto CollisionTileSheet
        {
            set { collisionTileSheet_ = value; }
            get { return collisionTileSheet_; }
        }

        // Assets
        private TileSheetList lstTileSheets_ = new TileSheetList();
        public TileSheetList TileSheets
        {
            get { return lstTileSheets_; }
        }

        // Each tile sheet has a list of layers that represent tile objects
        // assocated with it.
        private LayerLists dictTileObjects_ = new LayerLists();
        public LayerLists TileObjects
        {
            get { return dictTileObjects_; }
        }

        // Each tile sheet has a layer that represents the ad hoc tile objects.
        private Layers dictAdHocTileObjects = new Layers();
        public Layers AdHocTileObjects
        {
            get { return dictAdHocTileObjects; }
        }

        private SpriteSheetList lstSpriteSheets_ = new SpriteSheetList();
        public SpriteSheetList SpriteSheets
        {
            get { return lstSpriteSheets_; }
        }

        private AudioList lstAudioAssets_ = new AudioList();
        public AudioList AudioAssets
        {
            get { return lstAudioAssets_; }
        }

        private LoadingScreenList lstLoadingScreens_ = new LoadingScreenList();
        public LoadingScreenList LoadingScreens
        {
            get { return lstLoadingScreens_; }
        }

        private TransitionList lstTransitions_ = new TransitionList();
        public TransitionList Transitions
        {
            get { return lstTransitions_; }
        }

        private ParticleList lstParticles_ = new ParticleList();
        public ParticleList Particles
        {
            get { return lstParticles_; }
        }

        private ParticleEmitterList lstParticleEmitters_ = new ParticleEmitterList();
        public ParticleEmitterList ParticleEmitters
        {
            get { return lstParticleEmitters_; }
        }

        private EntityClassificationList lstEntityClassifications_ = new EntityClassificationList();
        public EntityClassificationList EntityClassifications
        {
            get { return lstEntityClassifications_; }
        }

        private SpawnPointList lstSpawnPoints_ = new SpawnPointList();
        public SpawnPointList SpawnPoints
        {
            get { return lstSpawnPoints_; }
        }

        public MapWidgetsByType MapWidgets
        {
            get { return dictMapWidgets; }
        }

        private ActorList lstActors_ = new ActorList();
        public ActorList Actors
        {
            get { return lstActors_; }
        }

        private EventList lstEvents_ = new EventList();
        public EventList Events
        {
            get { return lstEvents_; }
        }

        private HudElementList lstHudElements_ = new HudElementList();
        public HudElementList HudElements
        {
            get { return lstHudElements_; }
        }

        private AnimationList lstAnimations_ = new AnimationList();
        public AnimationList Animations
        {
            get { return lstAnimations_; }
        }

        // Dictionary of states. 
        // The key is the owner actor or HUD element ID.
        private StateLists dictStates_ = new StateLists();
        public StateLists States
        {
            get { return dictStates_; }
        }

        // Dictionary of animation frames. 
        // The key is the owner animation ID.
        private FrameLists dictFrames_ = new FrameLists();
        public FrameLists Frames
        {
            get { return dictFrames_; }
        }

        // Dictionary of frames triggers. 
        // The key is the owner frame ID.
        private FrameTriggerLists dictFrameTriggers_ = new FrameTriggerLists();
        public FrameTriggerLists FrameTriggers
        {
            get { return dictFrameTriggers_; }
        }

        // Dictionary of frame trigger signals. 
        private TriggerSignalList lstTriggerSignals_ = new TriggerSignalList();
        public TriggerSignalList TriggerSignals
        {
            get { return lstTriggerSignals_; }
        }

        // Dictionary of action points. 
        // The key is the owner frame ID.
        private ActionPointLists dictActionPoints_ = new ActionPointLists();
        public ActionPointLists ActionPoints
        {
            get { return dictActionPoints_; }
        }

        // Dictionary of hitbox identities.
        private HitboxIdenityList lstHitboxIdentities_ = new HitboxIdenityList();
        public HitboxIdenityList HitboxIdentities
        {
            get { return lstHitboxIdentities_; }
        }

        // Dictionary of hitboxes. 
        // The key is the owner animation or state ID.
        private HitboxLists dictHitboxes_ = new HitboxLists();
        public HitboxLists Hitboxes
        {
            get { return dictHitboxes_; }
        }

        // Dictionary of animation slots. 
        // The key is the owner state ID.
        private AnimSlotLists dictAnimationSlots_ = new AnimSlotLists();
        public AnimSlotLists AnimationSlots
        {
            get { return dictAnimationSlots_; }
        }

        // Dictionary of properties. 
        // The key is the owner entity ID.
        private PropertyLists dictProperties_ = new PropertyLists();
        public PropertyLists Properties
        {
            get { return dictProperties_; }
        }

        // Dictionary of scripts. 
        // The key is the owner entity ID.
        private Scripts dictScripts_ = new Scripts();
        public Scripts Scripts
        {
            get { return dictScripts_; }
        }

        // Dictionary of data files. 
        // The key is the owner entity ID.
        private DataFiles dictDataFiles_ = new DataFiles();
        public DataFiles DataFiles
        {
            get { return dictDataFiles_; }
        }

        private GameButtonList lstGameButtons_ = new GameButtonList();
        public GameButtonList GameButtons
        {
            get { return lstGameButtons_; }
        }

        private GameButtonGroupList lstGameButtonGroups_ = new GameButtonGroupList();
        public GameButtonGroupList GameButtonGroups
        {
            get { return lstGameButtonGroups_; }
        }

        private QueryList lstQueries_ = new QueryList();
        public QueryList Queries
        {
            get { return lstQueries_; }
        }

        private MenuBookList lstMenuBooks_ = new MenuBookList();
        public MenuBookList MenuBooks
        {
            get { return lstMenuBooks_; }
        }

        private MenuPageList lstMenuPages_ = new MenuPageList();
        public MenuPageList MenuPages
        {
            get { return lstMenuPages_; }
        }

        private UiWidgetList lstUiWidgets_ = new UiWidgetList();
        public UiWidgetList UiWidgets
        {
            get { return lstUiWidgets_; }
        }

        // The rooms contained in this project file.
        private RoomList rooms_ = new RoomList();
        public RoomList Rooms
        {
            get { return rooms_; }
        }

        // The ID of the initial room.
        private Guid initialRoomId_ = Guid.NewGuid();
        public Guid InitialRoomId
        {
            get { return initialRoomId_; }
            set { initialRoomId_ = value; }
        }

        // A dictionary of layers. The key is the room ID.
        private LayerLists dictLayers_ = new LayerLists();
        public LayerLists Layers
        {
            get { return dictLayers_; }
        }

        // A list of lists of interactive layer indexes. Each entry in the 
        // list corresponds to the room with the same index.
        private Indexes lstInteractiveLayerIndexList_ = new Indexes();
        public Indexes InteractiveLayerIndexes
        {
            get { return lstInteractiveLayerIndexList_; }
        }

        // Dictionary of entity instances. 
        // The key is the entity instance ID.
        private EntityInstances dictEntityInstances_ = new EntityInstances();
        public EntityInstances EntityInstances
        {
            get { return dictEntityInstances_; }
        }

        // Dictionary of entity instances properties. 
        // The key is the owner entity instance ID.
        private MapWidgetPropertiesDict dictMapWidgetProperties_ = new MapWidgetPropertiesDict();
        public MapWidgetPropertiesDict MapWidgetProperties
        {
            get { return dictMapWidgetProperties_; }
        }

        // The resources DTO which stores bitmap and audio data when saving to disk.
        private ProjectResourcesDto resources_ = new ProjectResourcesDto();
        public ProjectResourcesDto Resources
        {
            get { return resources_; }
        }
    }
}
