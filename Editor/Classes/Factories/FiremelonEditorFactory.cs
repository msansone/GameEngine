using SpriteSheetBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    class FiremelonEditorFactory : IFiremelonEditorFactory
    {
        public IAnimationsEditorControl NewAnimationsEditorControl(IProjectController projectController)
        {
            return new AnimationsEditorControl(projectController);
        }

        public IAnimationFrameViewerControl NewAnimationFrameViewerControl(IProjectController projectController)
        {
            return new AnimationFrameViewerControl(projectController);
        }

        public IAudioAssetsEditorControl NewAudioAssetsEditorControl(IProjectController projectController)
        {
            return new AudioAssetsEditorControl(projectController);
        }

        public IAudioPlayerControl NewAudioPlayerControl()
        {
            return new AudioPlayerControl();
        }

        public IConstantsEditorControl NewConstantsEditorControl(IProjectController projectController)
        {
            return new ConstantsEditorControl(projectController);
        }

        public IEntitiesEditorControl NewEntitiesEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            return new EntitiesEditorControl(projectController, nameGenerator, exceptionHandler);
        }

        public IExceptionHandler NewExceptionHandler()
        {
            return new ExceptionHandler();
        }

        public IGameButtonsEditorControl NewGameButtonsEditorControl(IProjectController projectController)
        {
            return new GameButtonsEditorControl(projectController);
        }

        public ILoadingScreensEditorControl NewLoadingScreensEditorControl(IProjectController projectController, INameGenerator nameGenerator)
        {
            return new LoadingScreensEditorControl(projectController, nameGenerator);
        }

        public IMapWidgetInstanceListControl NewMapWidgetInstanceListControl(IProjectController projectController, IMapWidgetPropertiesControl propertiesControl)
        {
            return new MapWidgetInstanceListControl(projectController, propertiesControl);
        }

        public IMapWidgetPropertiesControl NewMapWidgetPropertiesControl(IProjectController projectController)
        {
            return new MapWidgetPropertiesControl(projectController);
        }

        public IParticleEmittersEditorControl NewParticleEmittersEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            return new ParticleEmittersEditorControl(projectController, nameGenerator, exceptionHandler);
        }

        public IResourceManagerControl NewResourceManagerControl(IProjectController projectController)
        {
            return new ResourceManagerControl(projectController);
        }

        public IScriptManagerControl NewScriptManagerControl(IProjectController projectController)
        {
            return new ScriptManagerControl(projectController);
        }

        public IScriptsEditorControl NewScriptsEditorControl(IProjectController projectController, INameGenerator nameGenerator, bool showAllScripts)
        {
            return new ScriptsEditorControl(projectController, nameGenerator, showAllScripts);
        }

        public IPythonScriptEditorControl NewPythonScriptEditorControl(IProjectController projectController)
        {
            return new PythonScriptEditorControl(projectController);
        }

        public IStateEditorControl NewStateEditorControl(IProjectController projectController, IExceptionHandler exceptionHandler)
        {
            return new StateEditorControl(projectController, exceptionHandler);
        }

        public ITransitionsEditorControl NewTransitionsEditorControl(IProjectController projectController, INameGenerator nameGenerator)
        {
            return new TransitionsEditorControl(projectController, nameGenerator);
        }

        public IUiEditorControl NewUiEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            return new UiEditorControl(projectController, nameGenerator, exceptionHandler);
        }

        public IChangeProjectNameDialog NewChangeProjectNameDialog()
        {
            return new ChangeProjectNameForm();
        }

        public IMapWidgetFactory NewMapWidgetFactory(IProjectController projectController)
        {
            return new MapWidgetFactory(projectController);
        }

        public IProjectExporter NewProjectExporter(IProjectController projectController, IWin32Window owner, bool exportScriptsOnly)
        {
            return new ProjectExporter(projectController, NewWarningsForm(owner), exportScriptsOnly);
        }

        public IProjectLauncher NewProjectLauncher(IProjectController projectController, IWin32Window owner)
        {
            return new ProjectLauncher(projectController, owner);
        }

        public INewProjectDialog NewNewProjectDialog(IProjectController projectController)
        {
            return new NewProjectDialog(projectController);
        }

        public INewRoomDialog NewNewRoomDialog(IProjectController projectController, INameValidator nameValidator)
        {
            return new NewRoomDialog(projectController, nameValidator);
        }

        public INewLayerDialog NewNewLayerDialog()
        {
            return new NewLayerDialog();
        }

        public IEditLayerDialog NewEditLayerDialog()
        {
            return new EditLayerDialog();
        }

        public IEnterNameDialog NewEnterNameDialog()
        {
            return new EnterNameForm();
        }

        public IStringReplacementDialog NewStringReplacementDialog()
        {
            return new StringReplacementDialog();
        }
                
        public IWarningsForm NewWarningsForm(IWin32Window owner)
        {
            return new WarningsForm(owner);
        }

        public IProjectStreamReader NewProjectStreamReader(Version fileVersion, IMapWidgetFactory mapWidgetFactory, IProjectController projectController, IBitmapUtility bitmapUtility, IProjectUtility projectUtility, INameUtility nameUtility, IUriUtility uriUtility)
        {
            IBitmapUtility tempBitmapUtility = null;
            IProjectUtility tempProjectUtility = null;
            INameUtility tempNameUtility = null;
            IUriUtility tempUriUtility = null;

            // Only pass in the utilities if this is the latest version of the project file. Old versions are only used for upgrading, so any preparation functions can be ignored.
            if (fileVersion.CompareTo(ProjectDto.LatestProjectVersion) == 0)
            {
                tempBitmapUtility = bitmapUtility;
                tempProjectUtility = projectUtility;
                tempNameUtility = nameUtility;
                tempUriUtility = uriUtility;
            }

            if (fileVersion.CompareTo(new Version(2, 1, 0, 0)) == 0)
            {
                // Upgrade to the next project file.               
                return new ProjectStreamReader_2_1(mapWidgetFactory, projectController, tempBitmapUtility, tempProjectUtility, tempNameUtility, tempUriUtility);
            }
            else if (fileVersion.CompareTo(new Version(2, 2, 0, 0)) == 0)
            {
                // Upgrade to the next project file.               
                return new ProjectStreamReader_2_2(mapWidgetFactory, projectController, tempBitmapUtility, tempProjectUtility, tempNameUtility, tempUriUtility);
            }

            // No project upgrader exists for the given version.

            throw new NoStreamReaderExistsException("Project stream reader for version " + fileVersion.ToString() + " does not exist");
        }

        public IProjectStreamWriter NewProjectStreamWriter(Version fileVersion, ProjectUiStateDto projectUiState, ProjectResourcesDto projectResources, IUriUtility uriUtility, bool writeResources)
        {
            // Upgrade to the next project file.    
            if (fileVersion.CompareTo(new Version(2, 1, 0, 0)) == 0)
            {           
                return new ProjectStreamWriter_2_1(projectUiState, projectResources, uriUtility, writeResources);
            }
            if (fileVersion.CompareTo(new Version(2, 2, 0, 0)) == 0)
            {
                return new ProjectStreamWriter_2_2(projectUiState, uriUtility);
            }

            // No project upgrader exists for the given version.

            throw new NoUpgraderExistsException("Project stream writer for version " + fileVersion.ToString() + " does not exist");
        }

        public IProjectUpgrader NewProjectUpgrader(Version fileVersion, IProjectController projectController)
        {
            // Upgrade to the next project file. 
            if (fileVersion.CompareTo(new Version(1, 0, 0, 0)) == 0)
            {              
                return new ProjectUpgrader_1_0(NewMapWidgetFactory(projectController), projectController);
            }
            else if (fileVersion.CompareTo(new Version(2, 1, 0, 0)) == 0)
            {             
                return new ProjectUpgrader_2_1(NewMapWidgetFactory(projectController), projectController);
            }

            // No project upgrader exists for the given version.
            throw new NoUpgraderExistsException("Project upgrader for version " + fileVersion.ToString() + " does not exist");            
        }

        public IRoomEditorControl NewRoomEditorControl(IProjectController projectController)
        {
            return new RoomEditorControl(projectController);
        }

        public ISheetUtility NewSheetUtility()
        {
            return new SheetUtility();
        }

        public ISpriteSheetsEditorControl NewSpriteSheetsEditorControl(IProjectController projectController)
        {
            return new SpriteSheetsEditorControl(projectController);
        }

        public ITileSheetsEditorControl NewTileSheetsEditorControl(IProjectController projectController)
        {
            return new TileSheetsEditorControl(projectController);
        }

        public IRoomListForm NewRoomListForm(IProjectController projectController, INameValidator nameValidator)
        {
            return new RoomListForm(projectController, nameValidator);
        }

        public IRoomListControl NewRoomListControl(IProjectController projectController, INameValidator nameValidator)
        {
            return new RoomListControl(projectController, nameValidator);
        }
        
        public IPropertyGridForm NewPropertyGridForm(IProjectController projectController)
        {
            return new PropertyGridForm(projectController);
        }
        
        public ILayerListForm NewLayerListForm(IProjectController projectController)
        {
            return new LayerListForm(projectController);
        }

        public ILayerListControl NewLayerListControl(IProjectController projectController)
        {
            return new LayerListControl(projectController);
        }

        public ISheetViewerControl NewSpriteSheetViewerControl(IProjectController projectController)
        {
            ISheetViewerControl sheetViewerControl = new SpriteSheetViewerControl(projectController);
            
            return sheetViewerControl;
        }

        public ITileObjectViewerControl NewTileObjectViewerControl(IProjectController projectController)
        {
            return new TileObjectViewerControl(projectController);
        }

        public ITileSheetViewerControl NewTileSheetViewerControl(IProjectController projectController)
        {
            return new TileSheetViewerControl(projectController);
        }

        public IAssetSelectionForm NewAssetSelectionForm(IProjectController projectController, EditorForm owner)
        {
            return new AssetSelectionForm(projectController, owner);
        }

        public IAssetSelectionControl NewAssetSelectionControl(IProjectController projectController)
        {
            return new AssetSelectionControl(projectController);
        }
        
        public IProjectController NewProjectController(INameValidator nameValidator, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            return new ProjectController(nameValidator, nameGenerator, exceptionHandler);
        }

        public IBackgroundGenerator NewBackgroundGenerator()
        {
            return new CheckeredBackgroundGenerator();
        }

        public IOverlayGenerator NewOverlayGenerator()
        {
            return new OverlayGenerator();
        }

        public IResourceTextReader NewResourceTextReader()
        {
            return new ResourceTextReader();
        }

        public IResourceBitmapReader NewResourceBitmapReader()
        {
            return new ResourceBitmapReader();
        }
        
        public IRoomEditorCursor NewAudioSourceCursor(IProjectController projectController)
        {
            return new AudioSourceCursor(projectController);
        }

        public IRoomEditorCursor NewParticleEmitterCursor(IProjectController projectController)
        {
            return new ParticleEmitterCursor(projectController);
        }

        public IRoomEditorCursor NewSpawnPointCursor(IProjectController projectController)
        {
            return new SpawnPointCursor(projectController);
        }

        public IRoomEditorCursor NewTileObjectCursor(IProjectController projectController)
        {
            return new TileObjectCursor(projectController);
        }

        public IRoomEditorCursor NewWorldGeometryCursor(IProjectController projectController)
        {
            return new WorldGeometryCursor(projectController);
        }
        
        public IMapWidgetController NewSpawnPointWidgetController(IProjectController projectController)
        {
            return new SpawnPointWidgetController(projectController);
        }

        public IMapWidgetController NewParticleEmitterWidgetController(IProjectController projectController)
        {
            return new ParticleEmitterWidgetController(projectController);
        }

        public IAssetEditor NewAssetEditor()
        {
            return new AssetEditor();
        }

        public ITileSheetEditor NewTileSheetEditor()
        {
            return new TileSheetEditor();
        }

        public ISpriteSheetEditor NewSpriteSheetEditor()
        {
            return new SpriteSheetEditor();
        }
        
        public IAudioAssetEditor NewAudioAssetEditor()
        {
            return new AudioAssetEditor();
        }

        public IAnimationEditor NewAnimationEditor()
        {
            return new AnimationEditor();
        }

        public IStateEditor NewStateEditor()
        {
            return new StateEditor();
        }
        
        public IRoomPropertiesEditor NewRoomPropertiesEditor()
        {
            return new RoomPropertiesEditor();
        }

        public IPopoutForm NewPopoutForm(Control control, string caption)
        {
            IPopoutForm newPopoutForm = new PopoutForm();

            newPopoutForm.Text = caption;

            newPopoutForm.ChildControl = control;

            return newPopoutForm;
        }

        public IProgressForm NewProgressForm(IProjectController projectController)
        {
            ProgressForm newProgressForm = new ProgressForm();
            
            return newProgressForm;
        }

        public IResourcesForm NewResourcesForm(IProjectController projectController)
        {
            return new ResourcesForm(projectController);
        }
        
        public IScriptsForm NewScriptsForm(IProjectController projectController, INameGenerator nameGenerator)
        {
            return new ScriptsForm(projectController, nameGenerator);
        }

        public ISpriteSheetBuilderDialog NewSheetBuilderForm()
        {
            return new SpriteSheetBuilderDialog();
        }
        
        public ITileObjectDtoProxy NewTileObjectProxy(IProjectController projectController, Guid tileObjectId)
        {
            return new TileObjectDtoProxy(projectController, tileObjectId);
        }

        public ISceneryAnimationDtoProxy NewSceneryAnimationProxy(IProjectController projectController, Guid sceneryAnimationId)
        {
            return new SceneryAnimationDtoProxy(projectController, sceneryAnimationId);
        }

        public ITileSheetDtoProxy NewTileSheetProxy(IProjectController projectController, Guid tileSheetId)
        {
            return new TileSheetDtoProxy(projectController, tileSheetId);
        }

        public ISpriteSheetDtoProxy NewSpriteSheetProxy(IProjectController projectController, Guid spriteSheetId)
        {
            return new SpriteSheetDtoProxy(projectController, spriteSheetId);
        }

        public IAudioAssetDtoProxy NewAudioAssetProxy(IProjectController projectController, Guid audioAssetId)
        {
            return new AudioAssetDtoProxy(projectController, audioAssetId);
        }
        
        // Removed in 2.1
        //public IEntityClassificationDtoProxy NewEntityClassificationProxy(IProjectController projectController, Guid entityClassificationId)
        //{
        //    return new EntityClassificationDtoProxy(projectController, entityClassificationId);
        //}

        public ISpawnPointDtoProxy NewSpawnPointProxy(IProjectController projectController, Guid spawnPointId)
        {
            return new SpawnPointDtoProxy(projectController, spawnPointId);
        }

        public IRoomDtoProxy NewRoomProxy(IProjectController projectController, Guid roomId)
        {
            return new RoomDtoProxy(projectController, roomId);
        }

        public IActorDtoProxy NewActorProxy(IProjectController projectController, Guid actorId)
        {
            return new ActorDtoProxy(projectController, actorId);
        }

        public IEventDtoProxy NewEventProxy(IProjectController projectController, Guid eventId)
        {
            return new EventDtoProxy(projectController, eventId);
        }

        public IHudElementDtoProxy NewHudElementProxy(IProjectController projectController, Guid hudElementId)
        {
            return new HudElementDtoProxy(projectController, hudElementId);
        }

        public IAnimationDtoProxy NewAnimationProxy(IProjectController projectController, Guid animationId)
        {
            return new AnimationDtoProxy(projectController, animationId);
        }

        public IAnimationGroupDtoProxy NewAnimationGroupProxy(IProjectController projectController, Guid animationGroupId)
        {
            return new AnimationGroupDtoProxy(projectController, animationGroupId);
        }

        public IStateDtoProxy NewStateProxy(IProjectController projectController, Guid stateId)
        {
            return new StateDtoProxy(projectController, stateId);
        }

        public IFrameDtoProxy NewFrameProxy(IProjectController projectController, Guid frameId)
        {
            return new FrameDtoProxy(projectController, frameId);
        }

        public IHitboxDtoProxy NewHitboxProxy(IProjectController projectController, Guid hitboxId)
        {
            return new HitboxDtoProxy(projectController, hitboxId);
        }

        public IAnimationSlotDtoProxy NewAnimationSlotProxy(IProjectController projectController, Guid animationSlotId)
        {
            return new AnimationSlotDtoProxy(projectController, animationSlotId);
        }

        public IHitboxIdentityDtoProxy NewHitboxIdentityProxy(IProjectController projectController, Guid hitboxIdentityId)
        {
            return new HitboxIdentityDtoProxy(projectController, hitboxIdentityId);
        }

        public IPropertyDtoProxy NewPropertyProxy(IProjectController projectController, Guid propertyId)
        {
            return new PropertyDtoProxy(projectController, propertyId);
        }

        public IGameButtonDtoProxy NewGameButtonProxy(IProjectController projectController, Guid gameButtonId)
        {
            return new GameButtonDtoProxy(projectController, gameButtonId);
        }

        public IGameButtonGroupDtoProxy NewGameButtonGroupProxy(IProjectController projectController, Guid gameButtonGroupId)
        {
            return new GameButtonGroupDtoProxy(projectController, gameButtonGroupId);
        }

        // Removed in 2.1
        //public IQueryDtoProxy NewQueryProxy(IProjectController projectController, Guid queryId)
        //{
        //    return new QueryDtoProxy(projectController, queryId);
        //}

        public IScriptDtoProxy NewScriptProxy(IProjectController projectController, Guid scriptId)
        {
            return new ScriptDtoProxy(projectController, scriptId);
        }

        public IScriptDtoProxy NewNamedScriptProxy(IProjectController projectController, Guid scriptId)
        {
            return new NamedScriptDtoProxy(projectController, scriptId);
        }

        public IDataFileDtoProxy NewDataFileProxy(IProjectController projectController, Guid dataFileId)
        {
            return new DataFileDtoProxy(projectController, dataFileId);
        }

        public IFrameTriggerDtoProxy NewFrameTriggerProxy(IProjectController projectController, Guid frameTriggerId)
        {
            return new FrameTriggerDtoProxy(projectController, frameTriggerId);
        }

        public ITriggerSignalDtoProxy NewTriggerSignalProxy(IProjectController projectController, Guid triggerSignalId)
        {
            return new TriggerSignalDtoProxy(projectController, triggerSignalId);
        }

        public IActionPointDtoProxy NewActionPointProxy(IProjectController projectController, Guid actionPointId)
        {
            return new ActionPointDtoProxy(projectController, actionPointId);
        }
        
        public IUiWidgetDtoProxy NewUiWidgetProxy(IProjectController projectController, IExceptionHandler exceptionHandler, Guid uiWidgetId)
        {
            return new UiWidgetDtoProxy(projectController, uiWidgetId, exceptionHandler);
        }

        public ILoadingScreenDtoProxy NewLoadingScreenProxy(IProjectController projectController, Guid loadingScreenId)
        {
            return new LoadingScreenDtoProxy(projectController, loadingScreenId);
        }

        public ITransitionDtoProxy NewTransitionProxy(IProjectController projectController, Guid transitionId)
        {
            return new TransitionDtoProxy(projectController, transitionId);
        }

        public IParticleDtoProxy NewParticleProxy(IProjectController projectController, Guid particleId)
        {
            return new ParticleDtoProxy(projectController, particleId);
        }

        public IParticleEmitterDtoProxy NewParticleEmitterProxy(IProjectController projectController, Guid particleEmitterId)
        {
            return new ParticleEmitterDtoProxy(projectController, particleEmitterId);
        }
    }

    public interface IFiremelonEditorFactory
    {
        IBackgroundGenerator NewBackgroundGenerator();
        IProjectController NewProjectController(INameValidator nameValidator, INameGenerator nameGenerator, IExceptionHandler exceptionHandler);
        IExceptionHandler NewExceptionHandler();
        IOverlayGenerator NewOverlayGenerator();
        IProjectExporter NewProjectExporter(IProjectController projectController, IWin32Window owner, bool exportScriptsOnly);
        IProjectLauncher NewProjectLauncher(IProjectController projectController, IWin32Window owner);
        IProjectStreamReader NewProjectStreamReader(Version fileVersion, IMapWidgetFactory mapWidgetFactory, IProjectController projectController, IBitmapUtility bitmapUtility, IProjectUtility projectUtility, INameUtility nameUtility, IUriUtility tempUriUtility);
        IProjectStreamWriter NewProjectStreamWriter(Version fileVersion, ProjectUiStateDto projectUiState, ProjectResourcesDto projectResources, IUriUtility uriUtility, bool writeResources);
        IProjectUpgrader NewProjectUpgrader(Version fileVersion, IProjectController projectController);
        IResourceTextReader NewResourceTextReader();
        IResourceBitmapReader NewResourceBitmapReader();
        ISheetUtility NewSheetUtility();

        // Factories
        IMapWidgetFactory NewMapWidgetFactory(IProjectController projectController);

        // Cursors
        IRoomEditorCursor NewAudioSourceCursor(IProjectController projectController);
        IRoomEditorCursor NewParticleEmitterCursor(IProjectController projectController);
        IRoomEditorCursor NewSpawnPointCursor(IProjectController projectController);
        IRoomEditorCursor NewTileObjectCursor(IProjectController projectController);
        IRoomEditorCursor NewWorldGeometryCursor(IProjectController projectController);
        

        // Map Widget Controllers
        IMapWidgetController NewSpawnPointWidgetController(IProjectController projectController);
        IMapWidgetController NewParticleEmitterWidgetController(IProjectController projectController);

        // Dialogs
        IChangeProjectNameDialog NewChangeProjectNameDialog();
        IEditLayerDialog NewEditLayerDialog();
        IEnterNameDialog NewEnterNameDialog();
        INewProjectDialog NewNewProjectDialog(IProjectController projectController);
        INewRoomDialog NewNewRoomDialog(IProjectController projectController, INameValidator nameValidator);
        INewLayerDialog NewNewLayerDialog();
        IStringReplacementDialog NewStringReplacementDialog();

        // Controls        
        IAnimationsEditorControl NewAnimationsEditorControl(IProjectController projectController);
        IAnimationFrameViewerControl NewAnimationFrameViewerControl(IProjectController projectController);
        IAssetSelectionControl NewAssetSelectionControl(IProjectController projectController);
        IAudioAssetsEditorControl NewAudioAssetsEditorControl(IProjectController projectController);
        IAudioPlayerControl NewAudioPlayerControl();
        IConstantsEditorControl NewConstantsEditorControl(IProjectController projectController);
        IEntitiesEditorControl NewEntitiesEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler);
        IGameButtonsEditorControl NewGameButtonsEditorControl(IProjectController projectController);
        ILayerListControl NewLayerListControl(IProjectController projectController);
        ILoadingScreensEditorControl NewLoadingScreensEditorControl(IProjectController projectController, INameGenerator nameGenerator);
        IMapWidgetInstanceListControl NewMapWidgetInstanceListControl(IProjectController projectController, IMapWidgetPropertiesControl propertiesControl);
        IMapWidgetPropertiesControl NewMapWidgetPropertiesControl(IProjectController projectController);
        IParticleEmittersEditorControl NewParticleEmittersEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler);
        IPythonScriptEditorControl NewPythonScriptEditorControl(IProjectController projectController);
        IResourceManagerControl NewResourceManagerControl(IProjectController projectController);        
        IRoomEditorControl NewRoomEditorControl(IProjectController projectController);
        IRoomListControl NewRoomListControl(IProjectController projectController, INameValidator nameValidator);
        IScriptManagerControl NewScriptManagerControl(IProjectController projectController);
        IScriptsEditorControl NewScriptsEditorControl(IProjectController projectController, INameGenerator nameGenerator, bool showAllScripts);
        ISheetViewerControl NewSpriteSheetViewerControl(IProjectController projectController);
        ITileObjectViewerControl NewTileObjectViewerControl(IProjectController projectController);
        ITileSheetViewerControl NewTileSheetViewerControl(IProjectController projectController);
        ISpriteSheetsEditorControl NewSpriteSheetsEditorControl(IProjectController projectController);
        IStateEditorControl NewStateEditorControl(IProjectController projectController, IExceptionHandler exceptionHandler);
        ITileSheetsEditorControl NewTileSheetsEditorControl(IProjectController projectController);
        ITransitionsEditorControl NewTransitionsEditorControl(IProjectController projectController, INameGenerator nameGenerator);
        IUiEditorControl NewUiEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler);

        // Pop out forms
        IRoomListForm NewRoomListForm(IProjectController projectController, INameValidator nameValidator);
        ILayerListForm NewLayerListForm(IProjectController projectController);
        IAssetSelectionForm NewAssetSelectionForm(IProjectController projectController, EditorForm owner);
        IPropertyGridForm NewPropertyGridForm(IProjectController projectController);

        // Editor forms
        IAssetEditor NewAssetEditor();
        ITileSheetEditor NewTileSheetEditor();
        ISpriteSheetEditor NewSpriteSheetEditor();
        IAudioAssetEditor NewAudioAssetEditor();
        IAnimationEditor NewAnimationEditor();
        IStateEditor NewStateEditor();
        IRoomPropertiesEditor NewRoomPropertiesEditor();

        // Other forms
        IWarningsForm NewWarningsForm(IWin32Window owner);
        IPopoutForm NewPopoutForm(Control control, string caption);
        IProgressForm NewProgressForm(IProjectController projectController);
        IResourcesForm NewResourcesForm(IProjectController projectController);
        IScriptsForm NewScriptsForm(IProjectController projectController, INameGenerator nameGenerator);
        ISpriteSheetBuilderDialog NewSheetBuilderForm();

        // Dto Proxies
        ITileObjectDtoProxy NewTileObjectProxy(IProjectController projectController, Guid tileObjectId);
        ISceneryAnimationDtoProxy NewSceneryAnimationProxy(IProjectController projectController, Guid sceneryAnimationId);
        ITileSheetDtoProxy NewTileSheetProxy(IProjectController projectController, Guid tileSheetId);
        ISpriteSheetDtoProxy NewSpriteSheetProxy(IProjectController projectController, Guid spriteSheetId);
        IAudioAssetDtoProxy NewAudioAssetProxy(IProjectController projectController, Guid audioAssetId);
        // Removed in 2.1
        //IEntityClassificationDtoProxy NewEntityClassificationProxy(IProjectController projectController, Guid entityClassificationId);
        ISpawnPointDtoProxy NewSpawnPointProxy(IProjectController projectController, Guid spawnPointId);
        IRoomDtoProxy NewRoomProxy(IProjectController projectController, Guid roomId);
        IActorDtoProxy NewActorProxy(IProjectController projectController, Guid actorId);
        IEventDtoProxy NewEventProxy(IProjectController projectController, Guid eventId);
        IHudElementDtoProxy NewHudElementProxy(IProjectController projectController, Guid hudElementId);
        IAnimationDtoProxy NewAnimationProxy(IProjectController projectController, Guid animationId);
        IAnimationGroupDtoProxy NewAnimationGroupProxy(IProjectController projectController, Guid animationGroupId);
        IStateDtoProxy NewStateProxy(IProjectController projectController, Guid stateId);
        IFrameDtoProxy NewFrameProxy(IProjectController projectController, Guid frameId);
        IHitboxDtoProxy NewHitboxProxy(IProjectController projectController, Guid hitboxId);
        IAnimationSlotDtoProxy NewAnimationSlotProxy(IProjectController projectController, Guid animationSlotId);
        IPropertyDtoProxy NewPropertyProxy(IProjectController projectController, Guid propertyId);
        IHitboxIdentityDtoProxy NewHitboxIdentityProxy(IProjectController projectController, Guid hitboxIdentityId);
        IGameButtonDtoProxy NewGameButtonProxy(IProjectController projectController, Guid gameButtonId);
        IGameButtonGroupDtoProxy NewGameButtonGroupProxy(IProjectController projectController, Guid gameButtonGroupId);
        // Removed in 2.1
        //IQueryDtoProxy NewQueryProxy(IProjectController projectController, Guid queryId);
        IScriptDtoProxy NewScriptProxy(IProjectController projectController, Guid scriptId);
        IScriptDtoProxy NewNamedScriptProxy(IProjectController projectController, Guid scriptId);
        IDataFileDtoProxy NewDataFileProxy(IProjectController projectController, Guid dataFileId);
        IFrameTriggerDtoProxy NewFrameTriggerProxy(IProjectController projectController, Guid frameTriggerId);
        ITriggerSignalDtoProxy NewTriggerSignalProxy(IProjectController projectController, Guid triggerSignalId);
        IActionPointDtoProxy NewActionPointProxy(IProjectController projectController, Guid actionPointId);
        // Removed in 2.1
        //IMenuBookDtoProxy NewMenuBookProxy(IProjectController projectController, Guid menuBookId);
        //IMenuPageDtoProxy NewMenuPageProxy(IProjectController projectController, Guid menuPageId);
        IUiWidgetDtoProxy NewUiWidgetProxy(IProjectController projectController, IExceptionHandler exceptionHandler, Guid uiWidgetId);
        ILoadingScreenDtoProxy NewLoadingScreenProxy(IProjectController projectController, Guid loadingScreenId);
        ITransitionDtoProxy NewTransitionProxy(IProjectController projectController, Guid transitionId);
        IParticleDtoProxy NewParticleProxy(IProjectController projectController, Guid particleId);
        IParticleEmitterDtoProxy NewParticleEmitterProxy(IProjectController projectController, Guid particleEmitterId);        
    }
}
