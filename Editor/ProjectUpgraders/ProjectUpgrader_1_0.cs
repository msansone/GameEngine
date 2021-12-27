using DragonOgg.MediaPlayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    // Upgrade the 1.0 stream to the next version (2.1).
    class ProjectUpgrader_1_0 : IProjectUpgrader
    {
        public ProjectUpgrader_1_0(IMapWidgetFactory mapWidgetFactory, IProjectController projectController)
        {
            mapWidgetFactory_ = mapWidgetFactory;

            projectController_ = projectController;

            // Read the stream into this project dto object, and then convert it back into a stream to return to the caller.
            projectDto_ = new ProjectDto_2_1();

            // Don't need to pass in a UI state object or resources object when upgrading. It can be assumed that the project
            // object already contains everything it needs. The UI state and resources can only be changed by UI interaction.
            // We do still want to write the resources, so pass true for that. The only time writing resources is ignored
            // is for the undo/redo streams, to save space that is not necessary.
            projectStreamWriter_2_1_ = new ProjectStreamWriter_2_1(null, null, null, true);            
        }

        private IMapWidgetFactory mapWidgetFactory_;

        IProjectController projectController_;
        
        private ProjectDto_2_1 projectDto_;

        private ProjectStreamWriter_2_1 projectStreamWriter_2_1_;

        public void Upgrade(MemoryStream stream_1_0, MemoryStream stream_2_1)
        {
            // Return a 2.1 stream from the given 1.0 stream.
            upgradeFrom_1_0_StreamTo_2_1_Stream(stream_1_0, stream_2_1);
        }
        
        private bool isEntityInstanceActor(Guid entityInstanceId)
        {
            foreach (ActorDto actor in projectDto_.Actors)
            {
                if (actor.Id == entityInstanceId)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isEntityInstanceEvent(Guid entityInstanceId)
        {
            foreach (EventDto eventEntity in projectDto_.Events)
            {
                if (eventEntity.Id == entityInstanceId)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isEntityInstanceHudElement(Guid entityInstanceId)
        {
            foreach (HudElementDto hudElement in projectDto_.HudElements)
            {
                if (hudElement.Id == entityInstanceId)
                {
                    return true;
                }
            }

            return false;
        }

        private void upgradeFrom_1_0_StreamTo_2_1_Stream(MemoryStream stream_1_0, MemoryStream stream_2_1)
        {
            // Populate a 2.1 project DTO object from the 1.0 stream, and write it to a 2.1 stream.
            BinaryReader br = new BinaryReader(stream_1_0);

            try
            {
                stream_1_0.Seek(0, SeekOrigin.Begin);

                // File type.
                string fileType = br.ReadString();

                if (fileType != "FMPROJ")
                {
                    throw new InvalidProjectFileException("Project file header is invalid.");
                }

                int major = br.ReadInt32();
                int minor = br.ReadInt32();
                int revision = br.ReadInt32();

                Version fileVersion = new Version(major, minor, 0, revision);

                // Major version number indicates breaking changes that are unconvertable.
                if (fileVersion.CompareTo(new Version(1, 0, 0, 0)) != 0)
                {
                    string message = "Incorrect file version found when attempting to up-convert. Expected version is 1.0.0.0, but the loaded version is " + fileVersion.ToString(4);

                    throw new InvalidProjectFileVersionException(message);
                }

                projectDto_.ProjectFolderFullPath = br.ReadString();
                projectDto_.ProjectFolderRelativePath = br.ReadString();
                projectDto_.ProjectName = br.ReadString();
                projectDto_.CameraHeight = br.ReadInt32();
                projectDto_.CameraWidth = br.ReadInt32();
                projectDto_.TileSize = br.ReadInt32();

                byte[] guidInitialRoomId = br.ReadBytes(16);
                projectDto_.InitialRoomId = new Guid(guidInitialRoomId);

                // Read the bitmap resource data.
                int bitmapResourceCount = br.ReadInt32();

                for (int i = 0; i < bitmapResourceCount; i++)
                {
                    BitmapResourceDto newBitmapResource = new BitmapResourceDto();

                    byte[] guidId = br.ReadBytes(16);
                    newBitmapResource.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newBitmapResource.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newBitmapResource.RootOwnerId = new Guid(guidRootOwnerId);

                    newBitmapResource.Path = br.ReadString();
                    newBitmapResource.RelativePath = br.ReadString();

                    int bitmapSize = br.ReadInt32();

                    Byte[] bitmapData = new Byte[bitmapSize];

                    bitmapData = br.ReadBytes(bitmapSize);

                    Bitmap bmpTemp;

                    using (MemoryStream bitmapStream = new MemoryStream(bitmapData))
                    {
                        bmpTemp = (Bitmap)Image.FromStream(bitmapStream);
                    }

                    newBitmapResource.BitmapImage = bmpTemp;

                    // Transparency not needed during the upgrade.
                    //// Apply transparency
                    //Bitmap bmpWithTransprency = applyTransparencyToBitmap(bmpTemp);

                    //newBitmapResource.BitmapImageWithTransparency = bmpWithTransprency;

                    projectDto_.Resources.Bitmaps.Add(newBitmapResource.Id, newBitmapResource);
                }

                // Read the audio resource data.
                int audioResourceCount = br.ReadInt32();

                for (int i = 0; i < audioResourceCount; i++)
                {
                    AudioResourceDto newAudioResource = new AudioResourceDto();

                    byte[] guidId = br.ReadBytes(16);
                    newAudioResource.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newAudioResource.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newAudioResource.RootOwnerId = new Guid(guidRootOwnerId);

                    newAudioResource.Path = br.ReadString();
                    newAudioResource.RelativePath = br.ReadString();

                    int audioSize = br.ReadInt32();

                    Byte[] audioData = new Byte[audioSize];

                    audioData = br.ReadBytes(audioSize);

                    newAudioResource.AudioData = audioData;

                    newAudioResource.Audio = new OggFile(audioData);

                    projectDto_.Resources.AudioData.Add(newAudioResource.Id, newAudioResource);
                }

                // Read the tilesheets
                int tileSheetCount = br.ReadInt32();

                for (int i = 0; i < tileSheetCount; i++)
                {
                    TileSheetDto newTileSheet = new TileSheetDto();

                    byte[] guidId = br.ReadBytes(16);
                    newTileSheet.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newTileSheet.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newTileSheet.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidBitmapId = br.ReadBytes(16);
                    newTileSheet.BitmapResourceId = new Guid(guidBitmapId);

                    newTileSheet.Name = br.ReadString();
                    newTileSheet.TileSize = br.ReadInt32();
                    newTileSheet.Columns = br.ReadInt32();
                    newTileSheet.Rows = br.ReadInt32();
                    newTileSheet.ScaleFactor = br.ReadSingle();
                    
                    projectDto_.TileSheets.Add(newTileSheet);
                }

                // Read the tile object data for each tilesheet.                
                for (int i = 0; i < tileSheetCount; i++)
                {
                    TileSheetDto tileSheet = projectDto_.TileSheets[i];

                    Guid tileSheetId = tileSheet.Id;

                    // Tile objects are non-convertible from 1.0 to 2.1
                    projectDto_.TileObjects.Add(tileSheetId, new List<TileObjectDto>());

                    // Removed in 2.1
                    //projectDto_.AdHocTileObjects.Add(tileSheetId, null);

                    int tileObjectCount = br.ReadInt32();

                    // Next, write the object matrices.
                    for (int j = 0; j < tileObjectCount; j++)
                    {
                        byte[] guidId = br.ReadBytes(16);

                        byte[] guidBitmapId = br.ReadBytes(16);

                        string name = br.ReadString();
                        bool isObjectFromTilesheet = br.ReadBoolean();
                        int cols = br.ReadInt32();
                        int rows = br.ReadInt32();

                        // Removd in 2.1
                        //LayerDto newTileObject = new LayerDto(name, cols, rows);

                        //newTileObject.Id = new Guid(guidId);

                        //newTileObject.BitmapResourceId = new Guid(guidBitmapId);

                        //newTileObject.IsObjectFromTilesheet = isObjectFromTilesheet; Removed from the format in version 2.1

                        for (int k = 0; k < rows; k++)
                        {
                            for (int l = 0; l < cols; l++)
                            {
                                int tileId = br.ReadInt32();

                                // Removed in 2.1
                                //if (tileId > -1)
                                //{
                                //    newTileObject.TileCount++;
                                //}
                                //else
                                //{
                                //    bool tileObjectContainsEmptyTile = true;
                                //}
                                //newTileObject.Tiles[k][l].Id = tileId; 
                            }
                        }

                        // Removed in 2.1
                        //projectDto_.TileObjects[tileSheetId].Add(newTileObject);
                    }

                    bool adHocObjectExists = br.ReadBoolean();

                    if (adHocObjectExists == true)
                    {
                        byte[] guidBitmapId = br.ReadBytes(16);

                        string name = br.ReadString();
                        int cols = br.ReadInt32();
                        int rows = br.ReadInt32();

                        LayerDto newTileObject = new LayerDto(name, cols, rows);

                        newTileObject.BitmapResourceId = new Guid(guidBitmapId);

                        for (int k = 0; k < rows; k++)
                        {
                            for (int l = 0; l < cols; l++)
                            {
                                int tileId = br.ReadInt32();

                                // Removed from the project in 2.1
                                //if (tileId > -1)
                                //{
                                //    newTileObject.TileCount++;
                                //}

                                //newTileObject.Tiles[k][l].Id = tileId;
                            }
                        }

                        // Removed in 2.1
                        //projectDto_.AdHocTileObjects[tileSheetId] = newTileObject;
                    }
                }

                // Read the sprite sheets.                 
                int spriteSheetCount = br.ReadInt32();

                for (int i = 0; i < spriteSheetCount; i++)
                {
                    SpriteSheetDto newSpriteSheet = new SpriteSheetDto();

                    byte[] guidId = br.ReadBytes(16);
                    newSpriteSheet.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newSpriteSheet.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newSpriteSheet.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidBitmapId = br.ReadBytes(16);
                    newSpriteSheet.BitmapResourceId = new Guid(guidBitmapId);

                    newSpriteSheet.Name = br.ReadString();

                    int cols = br.ReadInt32();
                    int rows = br.ReadInt32();
                    int cellWidth = br.ReadInt32();
                    int cellHeight = br.ReadInt32();
                    float scaleFactor = br.ReadSingle();

                    newSpriteSheet.Columns = cols;
                    newSpriteSheet.Rows = rows;
                    newSpriteSheet.CellWidth = cellWidth;
                    newSpriteSheet.CellHeight = cellHeight;
                    newSpriteSheet.ScaleFactor = scaleFactor;
                    
                    projectDto_.SpriteSheets.Add(newSpriteSheet);
                }

                // Read the audio resources.
                int audioCount = br.ReadInt32();

                for (int i = 0; i < audioCount; i++)
                {
                    AudioAssetDto newAudioAsset = new AudioAssetDto();

                    byte[] guidId = br.ReadBytes(16);
                    newAudioAsset.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newAudioAsset.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newAudioAsset.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidAudioId = br.ReadBytes(16);
                    newAudioAsset.AudioResourceId = new Guid(guidAudioId);

                    newAudioAsset.Name = br.ReadString();
                    newAudioAsset.Channel = br.ReadString();
                    //newAudioAsset.Loop = br.ReadBoolean();
                    //newAudioAsset.Volume = br.ReadFloat();

                    projectDto_.AudioAssets.Add(newAudioAsset);
                }

                // Read the loading screen data.
                int loadingScreenCount = br.ReadInt32();

                for (int i = 0; i < loadingScreenCount; i++)
                {
                    LoadingScreenDto newLoadingScreen = new LoadingScreenDto();

                    byte[] guidId = br.ReadBytes(16);
                    newLoadingScreen.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newLoadingScreen.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newLoadingScreen.RootOwnerId = new Guid(guidRootOwnerId);

                    newLoadingScreen.Name = br.ReadString();

                    projectDto_.LoadingScreens.Add(newLoadingScreen);
                }

                // Read the transition data.
                int transitionCount = br.ReadInt32();

                for (int i = 0; i < transitionCount; i++)
                {
                    TransitionDto newTransition = new TransitionDto();

                    byte[] guidId = br.ReadBytes(16);
                    newTransition.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newTransition.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newTransition.RootOwnerId = new Guid(guidRootOwnerId);

                    newTransition.Name = br.ReadString();

                    projectDto_.Transitions.Add(newTransition);
                }

                // Read the particle emitter data.
                int particleEmitterCount = br.ReadInt32();

                for (int i = 0; i < particleEmitterCount; i++)
                {
                    ParticleEmitterDto newParticleEmitter = new ParticleEmitterDto();

                    byte[] guidId = br.ReadBytes(16);
                    newParticleEmitter.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newParticleEmitter.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newParticleEmitter.RootOwnerId = new Guid(guidRootOwnerId);

                    newParticleEmitter.Name = br.ReadString();

                    projectDto_.ParticleEmitters.Add(newParticleEmitter);
                }

                // Read the particle data.
                int particleCount = br.ReadInt32();

                for (int i = 0; i < particleCount; i++)
                {
                    ParticleDto newParticle = new ParticleDto();

                    byte[] guidId = br.ReadBytes(16);
                    newParticle.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newParticle.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newParticle.RootOwnerId = new Guid(guidRootOwnerId);

                    newParticle.Name = br.ReadString();

                    projectDto_.Particles.Add(newParticle);
                }

                // Read the actor data.
                int actorCount = br.ReadInt32();

                for (int i = 0; i < actorCount; i++)
                {
                    ActorDto newActor = new ActorDto();

                    byte[] guidId = br.ReadBytes(16);
                    newActor.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newActor.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newActor.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidInitialStateId = br.ReadBytes(16);
                    newActor.InitialStateId = new Guid(guidInitialStateId);

                    newActor.Name = br.ReadString();
                    newActor.StageHeight = br.ReadInt32();
                    newActor.StageWidth = br.ReadInt32();

                    uint stageOriginLocation = br.ReadUInt32();
                    newActor.StageOriginLocation = (OriginLocation)stageOriginLocation;

                    int pivotX = br.ReadInt32();
                    int pivotY = br.ReadInt32();

                    newActor.PivotPoint = new Point(pivotX, pivotY);

                    newActor.Tag = br.ReadString();
                    newActor.KeepRoomActive = br.ReadBoolean();

                    byte[] classificationGuidId = br.ReadBytes(16);
                    newActor.Classification = new Guid(classificationGuidId);

                    projectDto_.Actors.Add(newActor);

                    projectDto_.States.Add(newActor.Id, new List<StateDto>());
                    projectDto_.Properties.Add(newActor.Id, new List<PropertyDto>());
                }

                // Read the event data.
                int eventCount = br.ReadInt32();

                for (int i = 0; i < eventCount; i++)
                {
                    EventDto newEvent = new EventDto();

                    byte[] guidId = br.ReadBytes(16);
                    newEvent.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newEvent.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newEvent.RootOwnerId = new Guid(guidRootOwnerId);

                    newEvent.Name = br.ReadString();
                    newEvent.Tag = br.ReadString();

                    byte[] classificationGuidId = br.ReadBytes(16);
                    newEvent.Classification = new Guid(classificationGuidId);

                    projectDto_.Events.Add(newEvent);

                    projectDto_.Properties.Add(newEvent.Id, new List<PropertyDto>());
                }

                // Read the HUD data.
                int hudElementCount = br.ReadInt32();

                for (int i = 0; i < hudElementCount; i++)
                {
                    HudElementDto newHudElement = new HudElementDto();

                    byte[] guidId = br.ReadBytes(16);
                    newHudElement.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newHudElement.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newHudElement.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidInitialStateId = br.ReadBytes(16);
                    newHudElement.InitialStateId = new Guid(guidInitialStateId);

                    newHudElement.Name = br.ReadString();
                    newHudElement.StageHeight = br.ReadInt32();
                    newHudElement.StageWidth = br.ReadInt32();

                    uint stageOriginLocation = br.ReadUInt32();
                    newHudElement.StageOriginLocation = (OriginLocation)stageOriginLocation;

                    int pivotX = br.ReadInt32();
                    int pivotY = br.ReadInt32();

                    newHudElement.PivotPoint = new Point(pivotX, pivotY);

                    newHudElement.Tag = br.ReadString();

                    byte[] classificationGuidId = br.ReadBytes(16);
                    newHudElement.Classification = new Guid(classificationGuidId);

                    projectDto_.HudElements.Add(newHudElement);

                    projectDto_.States.Add(newHudElement.Id, new List<StateDto>());
                    projectDto_.Properties.Add(newHudElement.Id, new List<PropertyDto>());
                }

                // Read the entity classification data.
                int entityClassificationCount = br.ReadInt32();

                for (int i = 0; i < entityClassificationCount; i++)
                {
                    EntityClassificationDto newEntityClassification = new EntityClassificationDto();

                    byte[] guidId = br.ReadBytes(16);
                    newEntityClassification.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newEntityClassification.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newEntityClassification.RootOwnerId = new Guid(guidRootOwnerId);

                    newEntityClassification.Name = br.ReadString();

                    // Removed in 2.1
                    //projectDto_.EntityClassifications.Add(newEntityClassification);                    
                }

                // Read the spawn point data.
                int spawnPointCount = br.ReadInt32();

                for (int i = 0; i < spawnPointCount; i++)
                {
                    SpawnPointDto newSpawnPoint = new SpawnPointDto();

                    byte[] guidId = br.ReadBytes(16);
                    newSpawnPoint.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newSpawnPoint.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newSpawnPoint.RootOwnerId = new Guid(guidRootOwnerId);

                    newSpawnPoint.Name = br.ReadString();

                    projectDto_.SpawnPoints.Add(newSpawnPoint);
                }

                // Read the animation data.               
                int animationCount = br.ReadInt32();

                for (int j = 0; j < animationCount; j++)
                {
                    AnimationDto newAnimation = new AnimationDto();

                    byte[] guidId = br.ReadBytes(16);
                    newAnimation.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newAnimation.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newAnimation.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidSpriteSheetId = br.ReadBytes(16);
                    newAnimation.SpriteSheet = new Guid(guidSpriteSheetId);

                    byte[] guidAlphaMaskSheetId = br.ReadBytes(16);
                    newAnimation.AlphaMaskSheet = new Guid(guidAlphaMaskSheetId);

                    newAnimation.Name = br.ReadString();
                    newAnimation.AnimationStyle = (AnimationStyle)br.ReadInt32();
                    newAnimation.UpdateInterval = br.ReadSingle();

                    // Version 1.0 did not contain animation groups. Add all animations to group "None" (empty GUID).
                    projectDto_.Animations[Guid.Empty].Add(newAnimation);

                    projectDto_.Frames.Add(newAnimation.Id, new List<FrameDto>());
                }

                // Read the state data.
                int stateListCount = br.ReadInt32();

                for (int i = 0; i < stateListCount; i++)
                {
                    int stateCount = br.ReadInt32();

                    for (int j = 0; j < stateCount; j++)
                    {
                        StateDto newState = new StateDto();

                        byte[] guidId = br.ReadBytes(16);
                        newState.Id = new Guid(guidId);

                        byte[] guidOwnerId = br.ReadBytes(16);
                        newState.OwnerId = new Guid(guidOwnerId);

                        byte[] guidRootOwnerId = br.ReadBytes(16);
                        newState.RootOwnerId = new Guid(guidRootOwnerId);

                        newState.Name = br.ReadString();

                        projectDto_.States[newState.OwnerId].Add(newState);

                        projectDto_.Hitboxes.Add(newState.Id, new List<HitboxDto>());
                        projectDto_.AnimationSlots.Add(newState.Id, new List<AnimationSlotDto>());
                    }
                }

                // Read the frame data.                
                int frameListCount = br.ReadInt32();

                for (int i = 0; i < frameListCount; i++)
                {
                    int frameCount = br.ReadInt32();

                    for (int j = 0; j < frameCount; j++)
                    {
                        FrameDto newFrame = new FrameDto();

                        byte[] guidId = br.ReadBytes(16);
                        newFrame.Id = new Guid(guidId);

                        byte[] guidOwnerId = br.ReadBytes(16);
                        newFrame.OwnerId = new Guid(guidOwnerId);

                        byte[] guidRootOwnerId = br.ReadBytes(16);
                        newFrame.RootOwnerId = new Guid(guidRootOwnerId);

                        newFrame.Name = br.ReadString();

                        int sheetCellIndex = br.ReadInt32();

                        if (sheetCellIndex == -1)
                        {
                            newFrame.SheetCellIndex = null;
                        }
                        else
                        {
                            newFrame.SheetCellIndex = sheetCellIndex;
                        }

                        int alphaMaskIndex = br.ReadInt32();

                        if (alphaMaskIndex == -1)
                        {
                            newFrame.AlphaMaskCellIndex = null;
                        }
                        else
                        {
                            newFrame.AlphaMaskCellIndex = alphaMaskIndex;
                        }

                        projectDto_.Frames[newFrame.OwnerId].Add(newFrame);

                        projectDto_.Hitboxes.Add(newFrame.Id, new List<HitboxDto>());
                        projectDto_.FrameTriggers.Add(newFrame.Id, new List<FrameTriggerDto>());
                        projectDto_.ActionPoints.Add(newFrame.Id, new List<ActionPointDto>());
                    }
                }

                // Read the trigger signal data.
                int triggerSignalCount = br.ReadInt32();

                for (int j = 0; j < triggerSignalCount; j++)
                {
                    TriggerSignalDto newTriggerSignal = new TriggerSignalDto();

                    byte[] guidId = br.ReadBytes(16);
                    newTriggerSignal.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newTriggerSignal.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newTriggerSignal.RootOwnerId = new Guid(guidRootOwnerId);

                    newTriggerSignal.Name = br.ReadString();

                    projectDto_.TriggerSignals.Add(newTriggerSignal);
                }

                // Read the frame trigger data.
                int frameTriggerListCount = br.ReadInt32();

                for (int i = 0; i < frameTriggerListCount; i++)
                {
                    int frameTriggerCount = br.ReadInt32();

                    for (int j = 0; j < frameTriggerCount; j++)
                    {
                        FrameTriggerDto newFrameTrigger = new FrameTriggerDto();

                        byte[] guidId = br.ReadBytes(16);
                        newFrameTrigger.Id = new Guid(guidId);

                        byte[] guidOwnerId = br.ReadBytes(16);
                        newFrameTrigger.OwnerId = new Guid(guidOwnerId);

                        byte[] guidRootOwnerId = br.ReadBytes(16);
                        newFrameTrigger.RootOwnerId = new Guid(guidRootOwnerId);

                        byte[] guidTriggerSignalId = br.ReadBytes(16);
                        newFrameTrigger.Signal = new Guid(guidTriggerSignalId);

                        newFrameTrigger.Name = "Frame Trigger" + (j + 1).ToString();

                        projectDto_.FrameTriggers[newFrameTrigger.OwnerId].Add(newFrameTrigger);
                    }
                }

                // Read the action point data.
                int actionPointListCount = br.ReadInt32();

                for (int i = 0; i < actionPointListCount; i++)
                {
                    int actionPointCount = br.ReadInt32();

                    for (int j = 0; j < actionPointCount; j++)
                    {
                        ActionPointDto newActionPoint = new ActionPointDto();

                        byte[] guidId = br.ReadBytes(16);
                        newActionPoint.Id = new Guid(guidId);

                        byte[] guidOwnerId = br.ReadBytes(16);
                        newActionPoint.OwnerId = new Guid(guidOwnerId);

                        byte[] guidRootOwnerId = br.ReadBytes(16);
                        newActionPoint.RootOwnerId = new Guid(guidRootOwnerId);

                        newActionPoint.Name = br.ReadString();
                        newActionPoint.Position.X = br.ReadInt32();
                        newActionPoint.Position.Y = br.ReadInt32();

                        if (projectDto_.ActionPoints.ContainsKey(newActionPoint.OwnerId) == true)
                        {
                            projectDto_.ActionPoints[newActionPoint.OwnerId].Add(newActionPoint);
                        }
                    }
                }

                // Read the hitbox identity data.
                int hitboxIdentityCount = br.ReadInt32();

                for (int j = 0; j < hitboxIdentityCount; j++)
                {
                    HitboxIdentityDto newHitboxIdentity = new HitboxIdentityDto();

                    byte[] guidId = br.ReadBytes(16);
                    newHitboxIdentity.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newHitboxIdentity.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newHitboxIdentity.RootOwnerId = new Guid(guidRootOwnerId);

                    newHitboxIdentity.Name = br.ReadString();

                    projectDto_.HitboxIdentities.Add(newHitboxIdentity);
                }

                // Read the hitbox data.
                int hitboxListCount = br.ReadInt32();

                for (int i = 0; i < hitboxListCount; i++)
                {
                    int hitboxCount = br.ReadInt32();

                    for (int j = 0; j < hitboxCount; j++)
                    {
                        HitboxDto newHitbox = new HitboxDto();

                        byte[] guidId = br.ReadBytes(16);
                        newHitbox.Id = new Guid(guidId);

                        byte[] guidOwnerId = br.ReadBytes(16);
                        newHitbox.OwnerId = new Guid(guidOwnerId);

                        byte[] guidRootOwnerId = br.ReadBytes(16);
                        newHitbox.RootOwnerId = new Guid(guidRootOwnerId);

                        byte[] guidHitboxIdentityId = br.ReadBytes(16);
                        newHitbox.Identity = new Guid(guidHitboxIdentityId);

                        newHitbox.Name = "Hitbox " + (j + 1).ToString();

                        newHitbox.HitboxRect.Left = br.ReadInt32();
                        newHitbox.HitboxRect.Top = br.ReadInt32();
                        newHitbox.HitboxRect.Width = br.ReadInt32();
                        newHitbox.HitboxRect.Height = br.ReadInt32();

                        newHitbox.IsSolid = br.ReadBoolean();
                        newHitbox.Priority = (HitboxPriority)br.ReadInt32();

                        projectDto_.Hitboxes[newHitbox.OwnerId].Add(newHitbox);
                    }
                }

                // Read the animation slot data.
                int animationSlotListCount = br.ReadInt32();

                for (int i = 0; i < animationSlotListCount; i++)
                {
                    int animationSlotCount = br.ReadInt32();

                    for (int j = 0; j < animationSlotCount; j++)
                    {
                        AnimationSlotDto newAnimationSlot = new AnimationSlotDto();

                        byte[] guidId = br.ReadBytes(16);
                        newAnimationSlot.Id = new Guid(guidId);

                        byte[] guidOwnerId = br.ReadBytes(16);
                        newAnimationSlot.OwnerId = new Guid(guidOwnerId);

                        byte[] guidRootOwnerId = br.ReadBytes(16);
                        newAnimationSlot.RootOwnerId = new Guid(guidRootOwnerId);

                        byte[] guidAnimationId = br.ReadBytes(16);
                        newAnimationSlot.Animation = new Guid(guidAnimationId);

                        newAnimationSlot.Name = br.ReadString();

                        newAnimationSlot.Position.X = br.ReadInt32();
                        newAnimationSlot.Position.Y = br.ReadInt32();

                        newAnimationSlot.HueColor.Red = br.ReadSingle();
                        newAnimationSlot.HueColor.Green = br.ReadSingle();
                        newAnimationSlot.HueColor.Blue = br.ReadSingle();
                        newAnimationSlot.HueColor.Alpha = br.ReadSingle();

                        int pivotX = br.ReadInt32();
                        int pivotY = br.ReadInt32();

                        newAnimationSlot.PivotPoint = new Point(pivotX, pivotY);

                        newAnimationSlot.AlphaGradientFrom = br.ReadSingle();
                        newAnimationSlot.AlphaGradientTo = br.ReadSingle();
                        newAnimationSlot.AlphaGradientRadius = br.ReadSingle();

                        int alphaGradientRadialCenterX = br.ReadInt32();
                        int alphaGradientRadialCenterY = br.ReadInt32();

                        newAnimationSlot.AlphaGradientRadialCenter = new Point(alphaGradientRadialCenterX, alphaGradientRadialCenterY);

                        uint alphaGradientDirection = br.ReadUInt32();
                        newAnimationSlot.AlphaGradientDirection = (GradientDirection)alphaGradientDirection;

                        projectDto_.AnimationSlots[newAnimationSlot.OwnerId].Add(newAnimationSlot);
                    }
                }

                // Read the property data.
                int propertyListCount = br.ReadInt32();

                for (int i = 0; i < propertyListCount; i++)
                {
                    int propertyCount = br.ReadInt32();

                    for (int j = 0; j < propertyCount; j++)
                    {
                        PropertyDto newProperty = new PropertyDto();

                        byte[] guidId = br.ReadBytes(16);
                        newProperty.Id = new Guid(guidId);

                        byte[] guidOwnerId = br.ReadBytes(16);
                        newProperty.OwnerId = new Guid(guidOwnerId);

                        byte[] guidRootOwnerId = br.ReadBytes(16);
                        newProperty.RootOwnerId = new Guid(guidRootOwnerId);

                        newProperty.Name = br.ReadString();
                        newProperty.DefaultValue = br.ReadString();

                        projectDto_.Properties[newProperty.OwnerId].Add(newProperty);
                    }
                }

                // Read the query data.
                int queryCount = br.ReadInt32();

                for (int i = 0; i < queryCount; i++)
                {
                    QueryDto newQuery = new QueryDto();

                    byte[] guidId = br.ReadBytes(16);
                    newQuery.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newQuery.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newQuery.RootOwnerId = new Guid(guidRootOwnerId);

                    newQuery.Name = br.ReadString();
                }

                // Read the game button data.
                int gameButtonCount = br.ReadInt32();

                for (int i = 0; i < gameButtonCount; i++)
                {
                    GameButtonDto newGameButton = new GameButtonDto();

                    byte[] guidId = br.ReadBytes(16);
                    newGameButton.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newGameButton.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newGameButton.RootOwnerId = new Guid(guidRootOwnerId);

                    newGameButton.Name = br.ReadString();

                    byte[] guidGroupId = br.ReadBytes(16);
                    newGameButton.Group = new Guid(guidGroupId);

                    newGameButton.Label = br.ReadString();

                    projectDto_.GameButtons.Add(newGameButton);
                }

                // Read the game button group data.
                int gameButtonGroupCount = br.ReadInt32();

                for (int i = 0; i < gameButtonGroupCount; i++)
                {
                    GameButtonGroupDto newGameButtonGroup = new GameButtonGroupDto();

                    byte[] guidId = br.ReadBytes(16);
                    newGameButtonGroup.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newGameButtonGroup.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newGameButtonGroup.RootOwnerId = new Guid(guidRootOwnerId);

                    newGameButtonGroup.Name = br.ReadString();

                    projectDto_.GameButtonGroups.Add(newGameButtonGroup);
                }

                // Read the UI menu data.
                int menuBookCount = br.ReadInt32();

                for (int i = 0; i < menuBookCount; i++)
                {
                    MenuBookDto newMenuBook = new MenuBookDto();

                    byte[] guidId = br.ReadBytes(16);
                    newMenuBook.Id = new Guid(guidId);

                    newMenuBook.Name = br.ReadString();
                    newMenuBook.SuspendUpdates = br.ReadBoolean();

                    // Removed in 2.1
                    //projectDto_.MenuBooks.Add(newMenuBook);
                }

                int menuPageCount = br.ReadInt32();

                for (int i = 0; i < menuPageCount; i++)
                {
                    MenuPageDto newMenuPage = new MenuPageDto();

                    byte[] guidId = br.ReadBytes(16);
                    newMenuPage.Id = new Guid(guidId);

                    newMenuPage.Name = br.ReadString();

                    // Removed in 2.1
                    //projectDto_.MenuPages.Add(newMenuPage);
                }

                int uiWidgetCount = br.ReadInt32();

                for (int i = 0; i < uiWidgetCount; i++)
                {
                    UiWidgetDto newUiWidget = new UiWidgetDto();

                    byte[] guidId = br.ReadBytes(16);
                    newUiWidget.Id = new Guid(guidId);

                    newUiWidget.Name = br.ReadString();

                    projectDto_.UiWidgets.Add(newUiWidget);
                }

                int scriptCount = br.ReadInt32();

                for (int i = 0; i < scriptCount; i++)
                {
                    ScriptDto newScript = new ScriptDto();

                    byte[] guidId = br.ReadBytes(16);
                    newScript.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newScript.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newScript.RootOwnerId = new Guid(guidRootOwnerId);

                    newScript.Name = br.ReadString();

                    uint scriptType = br.ReadUInt32();
                    newScript.ScriptType = (ScriptType)scriptType;

                    newScript.ScriptPath = br.ReadString();
                    newScript.ScriptRelativePath = br.ReadString();

                    projectDto_.Scripts.Add(newScript.OwnerId, newScript);
                }

                int dataFileCount = br.ReadInt32();

                for (int i = 0; i < dataFileCount; i++)
                {
                    DataFileDto newDataFile = new DataFileDto();

                    byte[] guidId = br.ReadBytes(16);
                    newDataFile.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newDataFile.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newDataFile.RootOwnerId = new Guid(guidRootOwnerId);

                    newDataFile.Name = br.ReadString();
                    newDataFile.Extension = br.ReadString();

                    newDataFile.FilePath = br.ReadString();
                    newDataFile.FileRelativePath = br.ReadString();

                    projectDto_.DataFiles.Add(newDataFile.OwnerId, newDataFile);
                }

                // Read the room data.
                int roomCount = br.ReadInt32();

                for (int i = 0; i < roomCount; i++)
                {
                    RoomDto newRoom = new RoomDto();

                    byte[] guidId = br.ReadBytes(16);
                    newRoom.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newRoom.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newRoom.RootOwnerId = new Guid(guidRootOwnerId);

                    newRoom.Name = br.ReadString();

                    byte[] loadingScreenGuidId = br.ReadBytes(16);
                    newRoom.LoadingScreenId = new Guid(loadingScreenGuidId);

                    byte[] transitionGuidId = br.ReadBytes(16);
                    newRoom.TransitionId = new Guid(transitionGuidId);

                    projectDto_.Rooms.Add(newRoom);

                    projectDto_.Layers.Add(newRoom.Id, new List<LayerDto>());

                    // Removed in 2.1
                    //projectDto_.Tilesets.Add(newRoom.Id, new TilesetDto());
                }

                // Read the layer data
                int layerListCount = br.ReadInt32();

                for (int i = 0; i < layerListCount; i++)
                {
                    int layerCount = br.ReadInt32();

                    for (int j = 0; j < layerCount; j++)
                    {
                        byte[] guidId = br.ReadBytes(16);
                        byte[] guidOwnerId = br.ReadBytes(16);
                        byte[] guidRootOwnerId = br.ReadBytes(16);

                        string name = br.ReadString();
                        int cols = br.ReadInt32();
                        int rows = br.ReadInt32();

                        LayerDto newLayer = new LayerDto(name, cols, rows);
                        newLayer.Id = new Guid(guidId);
                        newLayer.OwnerId = new Guid(guidOwnerId);
                        newLayer.RootOwnerId = new Guid(guidRootOwnerId);

                        for (int k = 0; k < rows; k++)
                        {
                            for (int l = 0; l < cols; l++)
                            {
                                int tileId = br.ReadInt32();

                                // Removed in format 2.1
                                //newLayer.Tiles[k][l].Id = tileId;

                                //if (tileId > -1)
                                //{
                                //    newLayer.TileCount++;
                                //}

                                bool hasTileObjectHint = br.ReadBoolean();

                                if (hasTileObjectHint == true)
                                {
                                    byte[] guidTileObjectHintId = br.ReadBytes(16);

                                    // Removed in format 2.1
                                    //newLayer.Tiles[k][l].TileObjectHint = new Guid(guidTileObjectHintId);
                                }

                                int collisionTileId = br.ReadInt32();

                                // Removed in format 2.1
                                //newLayer.CollisionTileIds[k][l] = collisionTileId;

                                //if (collisionTileId > -1)
                                //{
                                //    newLayer.CollisionTileCount++;
                                //}
                            }
                        }

                        for (int k = 0; k < rows; k++)
                        {
                            for (int l = 0; l < cols; l++)
                            {
                                int actorInstanceIdCount = br.ReadInt32();

                                for (int m = 0; m < actorInstanceIdCount; m++)
                                {
                                    byte[] guidActorInstanceId = br.ReadBytes(16);

                                    // Removed in 2.1
                                    //newLayer.ActorIds[k][l].Add(new Guid(guidActorInstanceId));
                                }
                            }
                        }

                        for (int k = 0; k < rows; k++)
                        {
                            for (int l = 0; l < cols; l++)
                            {
                                int eventInstanceIdCount = br.ReadInt32();

                                for (int m = 0; m < eventInstanceIdCount; m++)
                                {
                                    byte[] guidEventInstanceId = br.ReadBytes(16);

                                    // Removed in 2.1
                                    //newLayer.EventIds[k][l].Add(new Guid(guidEventInstanceId));
                                }
                            }
                        }

                        projectDto_.Layers[newLayer.OwnerId].Add(newLayer);
                    }
                }

                // Read the interactive layer data.
                int interactiveLayerIndexesCount = br.ReadInt32();

                for (int i = 0; i < interactiveLayerIndexesCount; i++)
                {
                    byte[] guidRoomId = br.ReadBytes(16);
                    Guid roomId = new Guid(guidRoomId);

                    int interactiveLayerIndex = br.ReadInt32();

                    projectDto_.InteractiveLayerIndexes.Add(roomId, interactiveLayerIndex);
                }

                // Read the tileset data.
                for (int i = 0; i < roomCount; i++)
                {
                    byte[] guidRoomId = br.ReadBytes(16);
                    Guid roomId = new Guid(guidRoomId);

                    byte[] guidTileSheetId = br.ReadBytes(16);
                    Guid tileSheetId = new Guid(guidTileSheetId);

                    // Removed in 2.1
                    //projectDto_.Tilesets[roomId].TileSheetId = tileSheetId;
                }

                bool findAllTileObjectsInLayers = true;

                if (findAllTileObjectsInLayers == true)
                {
                    foreach (KeyValuePair<Guid, List<LayerDto>> kvp in projectDto_.Layers)
                    {
                        foreach (LayerDto layer in kvp.Value)
                        {
                            for (int k = 0; k < layer.Rows; k++)
                            {
                                for (int l = 0; l < layer.Cols; l++)
                                {
                                    // If this is the top left cell of a tile object, set the hint.

                                    // Removed in 2.1
                                    //layer.Tiles[k][l].TileObjectHint = matchesTileObject(projectDto_, layer, layer.Tiles[k][l].Id, k, l);
                                }
                            }
                        }
                    }
                }

                // Read the map widget data.
                int spawnPointInstanceCount = br.ReadInt32();

                for (int i = 0; i < spawnPointInstanceCount; i++)
                {
                    SpawnPointWidgetDto newSpawnPointWidget = (SpawnPointWidgetDto)mapWidgetFactory_.CreateMapWidget(new MapWidgetCreationParametersDto(MapWidgetType.SpawnPoint));

                    byte[] guidId = br.ReadBytes(16);
                    newSpawnPointWidget.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newSpawnPointWidget.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newSpawnPointWidget.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidIdentity = br.ReadBytes(16);
                    newSpawnPointWidget.Identity = new Guid(guidIdentity);

                    newSpawnPointWidget.Type = (MapWidgetType)br.ReadInt32();
                    newSpawnPointWidget.Name = br.ReadString();

                    int left = br.ReadInt32();
                    int top = br.ReadInt32();
                    int width = br.ReadInt32();
                    int height = br.ReadInt32();

                    newSpawnPointWidget.BoundingBox.Left = left;
                    newSpawnPointWidget.BoundingBox.Top = top;
                    newSpawnPointWidget.BoundingBox.Width = width;
                    newSpawnPointWidget.BoundingBox.Height = height;

                    newSpawnPointWidget.IdentityName = br.ReadString();

                    //addMapWidgetToGrid(newSpawnPointWidget, projectDto_);

                    MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                    projectDto_.MapWidgetProperties.Add(newSpawnPointWidget.Id, properties);

                    newSpawnPointWidget.Controller.ResetProperties(properties);

                    projectDto_.MapWidgets[MapWidgetType.SpawnPoint].Add(newSpawnPointWidget.Id, newSpawnPointWidget);
                }

                int particleEmitterInstanceCount = br.ReadInt32();

                for (int i = 0; i < particleEmitterInstanceCount; i++)
                {
                    ParticleEmitterWidgetDto newParticleEmitterWidget = (ParticleEmitterWidgetDto)mapWidgetFactory_.CreateMapWidget(new MapWidgetCreationParametersDto(MapWidgetType.ParticleEmitter));

                    byte[] guidId = br.ReadBytes(16);
                    newParticleEmitterWidget.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newParticleEmitterWidget.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newParticleEmitterWidget.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidParticleType = br.ReadBytes(16);
                    newParticleEmitterWidget.ParticleType = new Guid(guidParticleType);

                    byte[] guidParticleEmitter = br.ReadBytes(16);
                    newParticleEmitterWidget.Behavior = new Guid(guidParticleEmitter);

                    byte[] guidAnimationId = br.ReadBytes(16);
                    newParticleEmitterWidget.Animation = new Guid(guidAnimationId);

                    newParticleEmitterWidget.Type = (MapWidgetType)br.ReadInt32();
                    newParticleEmitterWidget.Name = br.ReadString();

                    int left = br.ReadInt32();
                    int top = br.ReadInt32();
                    int width = br.ReadInt32();
                    int height = br.ReadInt32();

                    newParticleEmitterWidget.BoundingBox.Left = left;
                    newParticleEmitterWidget.BoundingBox.Top = top;
                    newParticleEmitterWidget.BoundingBox.Width = width;
                    newParticleEmitterWidget.BoundingBox.Height = height;

                    newParticleEmitterWidget.ParticleTypeName = br.ReadString();
                    newParticleEmitterWidget.BehaviorName = br.ReadString();
                    newParticleEmitterWidget.AnimationName = br.ReadString();

                    int particlesPerEmission = br.ReadInt32();
                    newParticleEmitterWidget.ParticlesPerEmission = particlesPerEmission;

                    int maxParticles = br.ReadInt32();
                    newParticleEmitterWidget.MaxParticles = maxParticles;

                    double interval = br.ReadDouble();
                    newParticleEmitterWidget.Interval = interval;

                    double particleLifespan = br.ReadDouble();
                    newParticleEmitterWidget.ParticleLifespan = particleLifespan;

                    bool active = br.ReadBoolean();
                    newParticleEmitterWidget.Active = active;

                    bool attachParticles = br.ReadBoolean();
                    newParticleEmitterWidget.AttachParticles = attachParticles;

                    //addMapWidgetToGrid(newParticleEmitterWidget, projectDto_);

                    MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                    projectDto_.MapWidgetProperties.Add(newParticleEmitterWidget.Id, properties);

                    newParticleEmitterWidget.Controller.ResetProperties(properties);

                    projectDto_.MapWidgets[MapWidgetType.ParticleEmitter].Add(newParticleEmitterWidget.Id, newParticleEmitterWidget);
                }

                int audioSourceInstanceCount = br.ReadInt32();

                for (int i = 0; i < audioSourceInstanceCount; i++)
                {
                    AudioSourceWidgetDto newAudioSourceWidget = (AudioSourceWidgetDto)mapWidgetFactory_.CreateMapWidget(new MapWidgetCreationParametersDto(MapWidgetType.AudioSource));

                    byte[] guidId = br.ReadBytes(16);
                    newAudioSourceWidget.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newAudioSourceWidget.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newAudioSourceWidget.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidAudioId = br.ReadBytes(16);
                    newAudioSourceWidget.Audio = new Guid(guidAudioId);

                    newAudioSourceWidget.Name = br.ReadString();

                    int left = br.ReadInt32();
                    int top = br.ReadInt32();
                    int width = br.ReadInt32();
                    int height = br.ReadInt32();

                    newAudioSourceWidget.BoundingBox.Left = left;
                    newAudioSourceWidget.BoundingBox.Top = top;
                    newAudioSourceWidget.BoundingBox.Width = width;
                    newAudioSourceWidget.BoundingBox.Height = height;

                    bool autoplay = br.ReadBoolean();
                    newAudioSourceWidget.Autoplay = autoplay;

                    bool loop = br.ReadBoolean();
                    newAudioSourceWidget.Loop = loop;

                    int minDistance = br.ReadInt32();
                    newAudioSourceWidget.MinDistance = minDistance;

                    int maxDistance = br.ReadInt32();
                    newAudioSourceWidget.MaxDistance = maxDistance;

                    double volume = br.ReadDouble();
                    newAudioSourceWidget.Volume = (float)volume;

                    string audioName = br.ReadString();
                    newAudioSourceWidget.AudioName = audioName;

                    //addMapWidgetToGrid(newAudioSourceWidget, projectDto_);

                    MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                    projectDto_.MapWidgetProperties.Add(newAudioSourceWidget.Id, properties);

                    newAudioSourceWidget.Controller.ResetProperties(properties);

                    projectDto_.MapWidgets[MapWidgetType.AudioSource].Add(newAudioSourceWidget.Id, newAudioSourceWidget);
                }

                // Read the entity instance data.
                int entityInstanceCount = br.ReadInt32();

                for (int i = 0; i < entityInstanceCount; i++)
                {
                    EntityInstanceDto newEntityInstance = new EntityInstanceDto();

                    byte[] guidId = br.ReadBytes(16);
                    newEntityInstance.Id = new Guid(guidId);

                    byte[] guidOwnerId = br.ReadBytes(16);
                    newEntityInstance.OwnerId = new Guid(guidOwnerId);

                    byte[] guidRootOwnerId = br.ReadBytes(16);
                    newEntityInstance.RootOwnerId = new Guid(guidRootOwnerId);

                    byte[] guidEntityId = br.ReadBytes(16);
                    newEntityInstance.EntityId = new Guid(guidEntityId);

                    bool isActor = isEntityInstanceActor(newEntityInstance.EntityId);
                    bool isEvent = isEntityInstanceEvent(newEntityInstance.EntityId);
                    bool isHudElement = isEntityInstanceHudElement(newEntityInstance.EntityId);

                    string entityInstanceName = br.ReadString();
                    int boundingBoxLeft = br.ReadInt32();
                    int boundingBoxTop = br.ReadInt32();
                    int boundingBoxWidth = br.ReadInt32();
                    int boundingBoxHeight = br.ReadInt32();
                    bool acceptInput = br.ReadBoolean();
                    bool attachToCamera = br.ReadBoolean();
                    int renderOrder = br.ReadInt32();
                    Ownership ownership = (Ownership)br.ReadUInt32();


                    int gridCellCount = br.ReadInt32();

                    for (int j = 0; j < gridCellCount; j++)
                    {
                        int x = br.ReadInt32();
                        int y = br.ReadInt32();
                    }
                    
                    // Convert entity instances to map widgets.
                    if (isActor == true)
                    {
                        ActorWidgetDto newActorWidget = (ActorWidgetDto)mapWidgetFactory_.CreateMapWidget(new ActorMapWidgetCreationParametersDto(newEntityInstance.EntityId));

                        newActorWidget.Id = newEntityInstance.Id;

                        newActorWidget.OwnerId = newEntityInstance.OwnerId;
                        
                        newActorWidget.RootOwnerId = newEntityInstance.RootOwnerId;
                        
                        newActorWidget.EntityId = newEntityInstance.EntityId;

                        newActorWidget.Name = entityInstanceName;
                        newActorWidget.BoundingBox.Left = boundingBoxLeft;
                        newActorWidget.BoundingBox.Top = boundingBoxTop;
                        newActorWidget.BoundingBox.Width = boundingBoxWidth;
                        newActorWidget.BoundingBox.Height = boundingBoxHeight;
                        newActorWidget.AcceptInput = acceptInput;
                        newActorWidget.AttachToCamera = attachToCamera;
                        newActorWidget.RenderOrder = renderOrder;

                        projectDto_.MapWidgets[MapWidgetType.Actor].Add(newActorWidget.Id, newActorWidget);
                    }
                    else if (isEvent == true)
                    {
                        EventWidgetDto newEventWidget = (EventWidgetDto)mapWidgetFactory_.CreateMapWidget(new EventMapWidgetCreationParametersDto(newEntityInstance.EntityId));

                        newEventWidget.Id = newEntityInstance.Id;

                        newEventWidget.OwnerId = newEntityInstance.OwnerId;

                        newEventWidget.RootOwnerId = newEntityInstance.RootOwnerId;

                        newEventWidget.EntityId = newEntityInstance.EntityId;

                        newEventWidget.Name = entityInstanceName;
                        newEventWidget.BoundingBox.Left = boundingBoxLeft;
                        newEventWidget.BoundingBox.Top = boundingBoxTop;
                        newEventWidget.BoundingBox.Width = boundingBoxWidth;
                        newEventWidget.BoundingBox.Height = boundingBoxHeight;
                        newEventWidget.AcceptInput = acceptInput;

                        projectDto_.MapWidgets[MapWidgetType.Event].Add(newEventWidget.Id, newEventWidget);

                    }
                    else if (isHudElement == true)
                    {
                        HudElementWidgetDto newHudElementWidget = (HudElementWidgetDto)mapWidgetFactory_.CreateMapWidget(new HudElementMapWidgetCreationParametersDto(newEntityInstance.EntityId));

                        newHudElementWidget.Id = newEntityInstance.Id;

                        newHudElementWidget.OwnerId = newEntityInstance.OwnerId;

                        newHudElementWidget.RootOwnerId = newEntityInstance.RootOwnerId;

                        newHudElementWidget.EntityId = newEntityInstance.EntityId;

                        newHudElementWidget.Name = entityInstanceName;
                        newHudElementWidget.BoundingBox.Left = boundingBoxLeft;
                        newHudElementWidget.BoundingBox.Top = boundingBoxTop;
                        newHudElementWidget.BoundingBox.Width = boundingBoxWidth;
                        newHudElementWidget.BoundingBox.Height = boundingBoxHeight;
                        newHudElementWidget.AcceptInput = acceptInput;
                        newHudElementWidget.RenderOrder = renderOrder;

                        projectDto_.MapWidgets[MapWidgetType.HudElement].Add(newHudElementWidget.Id, newHudElementWidget);

                    }

                    projectDto_.MapWidgetProperties.Add(newEntityInstance.Id, new MapWidgetProperties(projectController_));                    
                }

                // Read the instance property data.
                int instancePropertyListCount = br.ReadInt32();

                for (int i = 0; i < instancePropertyListCount; i++)
                {
                    int instancePropertyCount = br.ReadInt32();

                    for (int j = 0; j < instancePropertyCount; j++)
                    {
                        PropertyDto newProperty = new PropertyDto();

                        byte[] guidId = br.ReadBytes(16);
                        newProperty.Id = new Guid(guidId);

                        byte[] guidOwnerId = br.ReadBytes(16);
                        newProperty.OwnerId = new Guid(guidOwnerId);

                        byte[] guidRootOwnerId = br.ReadBytes(16);
                        newProperty.RootOwnerId = new Guid(guidRootOwnerId);

                        newProperty.Name = br.ReadString();
                        newProperty.Value = br.ReadString();

                        // Only add user defined properties.
                        switch (newProperty.Name.ToUpper())
                        {
                            case "NAME":
                            case "POSITIONX":
                            case "POSITIONY":
                            case "BOXWIDTH":
                            case "BOXHEIGHT":
                            case "ATTACHTOCAMERA":
                            case "ACCEPTINPUT":
                            case "RENDERORDER":
                            case "OWNERSHIP":
                                // Ignore these.
                                break;

                            default:

                                projectDto_.MapWidgetProperties[newProperty.OwnerId].AddProperty(newProperty);

                                break;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
            // Write the 2.1 project file to a stream_2_1.           
            projectStreamWriter_2_1_.WriteProjectToStream(projectDto_, stream_2_1);            
        }
    }
}
