using DragonOgg.MediaPlayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class ProjectStreamReader_2_1 : IProjectStreamReader
    {
        public ProjectStreamReader_2_1(IMapWidgetFactory mapWidgetFactory, IProjectController projectController, IBitmapUtility bitmapUtility, IProjectUtility projectUtility, INameUtility nameUtility, IUriUtility uriUtility)
        {
            bitmapUtility_ = bitmapUtility;

            mapWidgetFactory_ = mapWidgetFactory;

            nameUtility_ = nameUtility;

            projectController_ = projectController;

            projectUtility_ = projectUtility;

            uriUtility_ = uriUtility;
        }

        private int bitmapResourceCount_;

        private IBitmapUtility bitmapUtility_;

        private IMapWidgetFactory mapWidgetFactory_;

        private INameUtility nameUtility_;

        private IProjectController projectController_;

        private IProjectUtility projectUtility_;

        private IUriUtility uriUtility_;

        public BaseProjectDto ReadProjectFromStream(Stream stream)
        {
            nameUtility_?.ClearNames();

            ProjectDto_2_1 project = new ProjectDto_2_1();

            BinaryReader br = new BinaryReader(stream);

            stream.Seek(0, SeekOrigin.Begin);

            readHeader(project, br);

            mapWidgetFactory_.TileSize = project.TileSize;

            readResources(project, br);
            
            readTileSheetsAndObjects(project, br);

            readSpriteSheets(project, br);

            readAudioAssets(project, br);

            readLoadingScreens(project, br);

            readTransitions(project, br);

            readParticleEmitters(project, br);

            readParticles(project, br);

            readActors(project, br);

            readEvents(project, br);

            readHudElements(project, br);

            readSpawnPoints(project, br);

            readAnimations(project, br);

            readStates(project, br);
            
            readFrames(project, br);

            readTriggerSignals(project, br);

            readFrameTriggers(project, br);

            readActionPoints(project, br);

            readHitboxIdentities(project, br);

            readHitboxes(project, br);

            readAnimationSlots(project, br);

            readProperties(project, br);

            readGameButtons(project, br);

            readGameButtonGroups(project, br);

            readUiWidgets(project, br);

            readScripts(project, br);

            readDataFiles(project, br);

            readRooms(project, br);

            readLayers(project, br);

            readSpawnPointWidgets(project, br);

            readParticleEmitterWidgets(project, br);

            readAudioSourceWidgets(project, br);

            readWorldGeometryWidgets(project, br);

            readTileObjectWidgets(project, br);

            readEventWidgets(project, br);

            readActorWidgets(project, br);

            readHudElementWidgets(project, br);

            readWidgetProperties(project, br);
            
            setAbsolutePaths(project);
           
            return project;
        }

        #region Private Functions

        private void readHeader(ProjectDto_2_1 project, BinaryReader br)
        {
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

            if (fileVersion != project.FileVersion)
            {
                string message = "The project version found in the file (" + fileVersion.ToString() + ") does not match the project version of the data structure being populated (" + project.FileVersion.Major.ToString() + ").";

                throw new InvalidProjectFileVersionException(message);
            }

            project.ProjectFolderFullPath = br.ReadString();
            project.ProjectFolderRelativePath = br.ReadString();
            project.ProjectName = br.ReadString();
            project.CameraHeight = br.ReadInt32();
            project.CameraWidth = br.ReadInt32();
            project.TileSize = br.ReadInt32();
            
            byte[] guidInitialRoomId = br.ReadBytes(16);
            project.InitialRoomId = new Guid(guidInitialRoomId);
        }

        private void readResources(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the bitmap resource data.
            bitmapResourceCount_ = br.ReadInt32();

            for (int i = 0; i < bitmapResourceCount_; i++)
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

                // Apply transparency
                Bitmap bmpWithTransprency = bitmapUtility_.ApplyTransparency(bmpTemp);

                newBitmapResource.BitmapImageWithTransparency = bmpWithTransprency;

                project.Resources.Bitmaps.Add(newBitmapResource.Id, newBitmapResource);
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

                project.Resources.AudioData.Add(newAudioResource.Id, newAudioResource);
            }
        }

        private void readTileSheetsAndObjects(ProjectDto_2_1 project, BinaryReader br)
        {
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

                if (bitmapResourceCount_ > 0)
                {
                    BitmapResourceDto bitmap = project.Resources.Bitmaps[newTileSheet.BitmapResourceId];

                    int cols = newTileSheet.Columns;
                    int rows = newTileSheet.Rows;
                    int width = newTileSheet.TileSize;
                    int height = newTileSheet.TileSize;
                    float scaleFactor = newTileSheet.ScaleFactor;

                    int scaledWidth = (int)(width / scaleFactor);

                    int scaledHeight = (int)(height / scaleFactor);

                    bitmapUtility_.SplitImageIntoCells(bitmap, cols, rows, scaledWidth, scaledHeight, 0, scaleFactor);
                }

                project.TileSheets.Add(newTileSheet);

                project.TileObjects.Add(newTileSheet.Id, new List<TileObjectDto>());
            }

            // Read the tile object data for each tilesheet.                
            for (int i = 0; i < tileSheetCount; i++)
            {
                TileSheetDto tileSheet = project.TileSheets[i];

                Guid tileSheetId = tileSheet.Id;

                int tileObjectCount = br.ReadInt32();

                for (int j = 0; j < tileObjectCount; j++)
                {
                    byte[] guidId = br.ReadBytes(16);

                    byte[] guidOwnerId = br.ReadBytes(16);

                    byte[] guidRootOwnerId = br.ReadBytes(16);

                    byte[] guidBitmapId = br.ReadBytes(16);

                    string name = br.ReadString();

                    int cols = br.ReadInt32();
                    int rows = br.ReadInt32();

                    int tlcCol = br.ReadInt32();
                    int tlcRow = br.ReadInt32();

                    TileObjectDto newTileObject = new TileObjectDto();

                    newTileObject.Id = new Guid(guidId);

                    newTileObject.OwnerId = new Guid(guidOwnerId);

                    newTileObject.RootOwnerId = new Guid(guidRootOwnerId);

                    newTileObject.BitmapResourceId = new Guid(guidBitmapId);

                    newTileObject.Name = name;
                    newTileObject.Columns = cols;
                    newTileObject.Rows = rows;
                    newTileObject.TopLeftCornerColumn = tlcCol;
                    newTileObject.TopLeftCornerRow = tlcRow;

                    project.TileObjects[tileSheetId].Add(newTileObject);
                }
            }
        }

        private void readSpriteSheets(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the sprite sheets.                 
            int spriteSheetCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                if (bitmapResourceCount_ > 0)
                {
                    BitmapResourceDto bitmap = project.Resources.Bitmaps[newSpriteSheet.BitmapResourceId];

                    bitmapUtility_.SplitImageIntoCells(bitmap, cols, rows, cellWidth, cellHeight, 0, scaleFactor);
                }

                project.SpriteSheets.Add(newSpriteSheet);

                lstNames.Add(newSpriteSheet.Name);

            }

            nameUtility_.AddSpriteSheetNames(lstNames);
        }

        private void readAudioAssets(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the audio assets.
            int audioCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                lstNames.Add(newAudioAsset.Name);

                project.AudioAssets.Add(newAudioAsset);
            }

            nameUtility_.AddAudioAssetNames(lstNames);
        }

        private void readLoadingScreens(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the loading screen data.
            int loadingScreenCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                project.LoadingScreens.Add(newLoadingScreen);

                lstNames.Add(newLoadingScreen.Name);

            }

            nameUtility_.AddLoadingScreenNames(lstNames);
        }

        private void readTransitions(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the transition data.
            int transitionCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                project.Transitions.Add(newTransition);

                lstNames.Add(newTransition.Name);
            }

            nameUtility_.AddTransitionNames(lstNames);
        }

        private void readParticleEmitters(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the particle emitter data.
            int particleEmitterCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                lstNames.Add(newParticleEmitter.Name);

                project.ParticleEmitters.Add(newParticleEmitter);
            }

            nameUtility_.AddParticleEmitterNames(lstNames);
        }

        private void readParticles(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the particle data.
            int particleCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                lstNames.Add(newParticle.Name);

                project.Particles.Add(newParticle);
            }

            nameUtility_.AddParticleEmitterNames(lstNames);
        }

        private void readActors(ProjectDto_2_1 project, BinaryReader br)
        {
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

                project.Actors.Add(newActor);
                
                project.States.Add(newActor.Id, new List<StateDto>());
                project.Properties.Add(newActor.Id, new List<PropertyDto>());
            }
        }

        private void readEvents(ProjectDto_2_1 project, BinaryReader br)
        {
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

                project.Events.Add(newEvent);

                project.Properties.Add(newEvent.Id, new List<PropertyDto>());
            }
        }

        private void readHudElements(ProjectDto_2_1 project, BinaryReader br)
        {
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

                byte[] guidInitialStateIdId = br.ReadBytes(16);
                newHudElement.InitialStateId = new Guid(guidInitialStateIdId);

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

                project.HudElements.Add(newHudElement);
                
                project.States.Add(newHudElement.Id, new List<StateDto>());
                project.Properties.Add(newHudElement.Id, new List<PropertyDto>());
            }
        }

        private void readSpawnPoints(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the spawn point data.
            int spawnPointCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                project.SpawnPoints.Add(newSpawnPoint);

                lstNames.Add(newSpawnPoint.Name);
            }

            nameUtility_.AddSpawnPointNames(lstNames);
        }

        private void readAnimations(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the animation group data.
            int animationGroupCount = br.ReadInt32();

            for (int i = 0; i < animationGroupCount; i++)
            {
                AnimationGroupDto animationGroup = new AnimationGroupDto();

                byte[] guidId = br.ReadBytes(16);
                animationGroup.Id = new Guid(guidId);

                byte[] guidOwnerId = br.ReadBytes(16);
                animationGroup.OwnerId = new Guid(guidOwnerId);

                byte[] guidRootOwnerId = br.ReadBytes(16);
                animationGroup.RootOwnerId = new Guid(guidRootOwnerId);

                animationGroup.Name = br.ReadString();

                // None group already exists.
                if (animationGroup.Id != Guid.Empty)
                {
                    project.AnimationGroups.Add(animationGroup);

                    project.Animations.Add(animationGroup.Id, new List<AnimationDto>());
                }

                // Read the animation data.               
                int animationCount = br.ReadInt32();

                List<string> lstNames = new List<string>();

                for (int j = 0; j < animationCount; j++)
                {
                    AnimationDto newAnimation = new AnimationDto();

                    byte[] guidAnimationId = br.ReadBytes(16);
                    newAnimation.Id = new Guid(guidAnimationId);

                    byte[] guidAnimationOwnerId = br.ReadBytes(16);
                    newAnimation.OwnerId = new Guid(guidAnimationOwnerId);

                    byte[] guidAnimationRootOwnerId = br.ReadBytes(16);
                    newAnimation.RootOwnerId = new Guid(guidAnimationRootOwnerId);

                    byte[] guidAnimationGroupId = br.ReadBytes(16);
                    newAnimation.GroupId = new Guid(guidAnimationGroupId);

                    byte[] guidSpriteSheetId = br.ReadBytes(16);
                    newAnimation.SpriteSheet = new Guid(guidSpriteSheetId);

                    byte[] guidAlphaMaskSheetId = br.ReadBytes(16);
                    newAnimation.AlphaMaskSheet = new Guid(guidAlphaMaskSheetId);

                    newAnimation.Name = br.ReadString();
                    newAnimation.AnimationStyle = (AnimationStyle)br.ReadInt32();
                    newAnimation.UpdateInterval = br.ReadSingle();

                    project.Animations[animationGroup.Id].Add(newAnimation);

                    lstNames.Add(newAnimation.Name);

                    project.Frames.Add(newAnimation.Id, new List<FrameDto>());
                }


                nameUtility_.AddAnimationNames(lstNames);
            }
        }

        private void readStates(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the state data.
            int stateListCount = br.ReadInt32();

            Dictionary<Guid, List<string>> dictNames = new Dictionary<Guid, List<string>>();

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

                    project.States[newState.OwnerId].Add(newState);

                    if (dictNames.ContainsKey(newState.OwnerId) == false)
                    {
                        dictNames.Add(newState.OwnerId, new List<string>());
                    }

                    dictNames[newState.OwnerId].Add(newState.Name);
                                        
                    project.Hitboxes.Add(newState.Id, new List<HitboxDto>());
                    project.AnimationSlots.Add(newState.Id, new List<AnimationSlotDto>());
                }
            }

            foreach (KeyValuePair<Guid, List<string>> kvp in dictNames)
            {
                Guid entityId = kvp.Key;
                List<string> lstNames = kvp.Value;

                nameUtility_.AddStateNames(entityId, lstNames);
            }            
        }

        private void readFrames(ProjectDto_2_1 project, BinaryReader br)
        {
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

                    project.Frames[newFrame.OwnerId].Add(newFrame);

                    project.Hitboxes.Add(newFrame.Id, new List<HitboxDto>());
                    project.FrameTriggers.Add(newFrame.Id, new List<FrameTriggerDto>());
                    project.ActionPoints.Add(newFrame.Id, new List<ActionPointDto>());
                }
            }
        }

        private void readTriggerSignals(ProjectDto_2_1 project, BinaryReader br)
        {
            List<string> lstNames = new List<string>();

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

                project.TriggerSignals.Add(newTriggerSignal);

                lstNames.Add(newTriggerSignal.Name);
            }

            nameUtility_.AddTriggerSignalNames(lstNames);
        }

        private void readFrameTriggers(ProjectDto_2_1 project, BinaryReader br)
        {
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

                    project.FrameTriggers[newFrameTrigger.OwnerId].Add(newFrameTrigger);
                }
            }
        }

        private void readActionPoints(ProjectDto_2_1 project, BinaryReader br)
        {
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

                    if (project.ActionPoints.ContainsKey(newActionPoint.OwnerId) == true)
                    {
                        project.ActionPoints[newActionPoint.OwnerId].Add(newActionPoint);
                    }
                }
            }
        }
        
        private void readHitboxIdentities(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the hitbox identity data.
            int hitboxIdentityCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                project.HitboxIdentities.Add(newHitboxIdentity);

                lstNames.Add(newHitboxIdentity.Name);
            }

            nameUtility_.AddHitboxIdentityNames(lstNames);
        }

        private void readHitboxes(ProjectDto_2_1 project, BinaryReader br)
        {
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

                    project.Hitboxes[newHitbox.OwnerId].Add(newHitbox);
                }
            }
        }

        private void readAnimationSlots(ProjectDto_2_1 project, BinaryReader br)
        {
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

                    uint animationSlotOriginLocation = br.ReadUInt32();
                    newAnimationSlot.OriginLocation = (OriginLocation)animationSlotOriginLocation;

                    project.AnimationSlots[newAnimationSlot.OwnerId].Add(newAnimationSlot);
                }
            }
        }

        private void readProperties(ProjectDto_2_1 project, BinaryReader br)
        {
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

                    project.Properties[newProperty.OwnerId].Add(newProperty);
                }
            }
        }

        private void readGameButtons(ProjectDto_2_1 project, BinaryReader br)
        {
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

                project.GameButtons.Add(newGameButton);
            }
        }

        private void readGameButtonGroups(ProjectDto_2_1 project, BinaryReader br)
        {
            // Read the game button group data.
            int gameButtonGroupCount = br.ReadInt32();

            List<string> lstNames = new List<string>();

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

                lstNames.Add(newGameButtonGroup.Name);

                project.GameButtonGroups.Add(newGameButtonGroup);
            }

            nameUtility_.AddGameButtonGroupNames(lstNames);
        }

        private void readUiWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
            int uiWidgetCount = br.ReadInt32();

            for (int i = 0; i < uiWidgetCount; i++)
            {
                UiWidgetDto newUiWidget = new UiWidgetDto();

                byte[] guidId = br.ReadBytes(16);
                newUiWidget.Id = new Guid(guidId);

                newUiWidget.Name = br.ReadString();

                project.UiWidgets.Add(newUiWidget);
            }
        }

        private void readScripts(ProjectDto_2_1 project, BinaryReader br)
        {
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

                project.Scripts.Add(newScript.OwnerId, newScript);
            }
        }

        private void readDataFiles(ProjectDto_2_1 project, BinaryReader br)
        {
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

                project.DataFiles.Add(newDataFile.OwnerId, newDataFile);
            }
        }

        private void readRooms(ProjectDto_2_1 project, BinaryReader br)
        {
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

                project.Rooms.Add(newRoom);

                project.Layers.Add(newRoom.Id, new List<LayerDto>());
            }
        }

        private void readLayers(ProjectDto_2_1 project, BinaryReader br)
        {
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
                    
                    project.Layers[newLayer.OwnerId].Add(newLayer);

                    project.MapWidgetsByLayer.Add(newLayer.Id, new Dictionary<Guid, MapWidgetDto>());
                }
            }

            // Read the interactive layer data.
            int interactiveLayerIndexesCount = br.ReadInt32();

            for (int i = 0; i < interactiveLayerIndexesCount; i++)
            {
                byte[] guidRoomId = br.ReadBytes(16);
                Guid roomId = new Guid(guidRoomId);

                int interactiveLayerIndex = br.ReadInt32();

                project.InteractiveLayerIndexes.Add(roomId, interactiveLayerIndex);
            }
        }

        private void readSpawnPointWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
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

                // Removed now that 2.1 has been repaced by 2.2
                //// When reading previous versions, only the raw data is required, because it is being upgraded. 
                //// Any extra preparation of the project object is used when interacting with it in the UI, so 
                //// the project utility will not be passed in.                
                //projectUtility_?.AddMapWidgetToGrid(newSpawnPointWidget, project);

                MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                project.MapWidgetProperties.Add(newSpawnPointWidget.Id, properties);

                newSpawnPointWidget.Controller.ResetProperties(properties);

                project.MapWidgets[MapWidgetType.SpawnPoint].Add(newSpawnPointWidget.Id, newSpawnPointWidget);

                project.MapWidgetsByLayer[newSpawnPointWidget.OwnerId].Add(newSpawnPointWidget.Id, newSpawnPointWidget);
            }
        }

        private void readParticleEmitterWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
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

                // Removed now that 2.1 has been replaced by 2.2
                //// When reading previous versions, only the raw data is required, because it is being upgraded. 
                //// Any extra preparation of the project object is used when interacting with it in the UI, so 
                //// the project utility will not be passed in.
                //if (projectUtility_ != null)
                //{
                //    projectUtility_.AddMapWidgetToGrid(newParticleEmitterWidget, (ProjectDto)project);
                //}

                MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                project.MapWidgetProperties.Add(newParticleEmitterWidget.Id, properties);

                newParticleEmitterWidget.Controller.ResetProperties(properties);

                project.MapWidgets[MapWidgetType.ParticleEmitter].Add(newParticleEmitterWidget.Id, newParticleEmitterWidget);

                project.MapWidgetsByLayer[newParticleEmitterWidget.OwnerId].Add(newParticleEmitterWidget.Id, newParticleEmitterWidget);
            }
        }

        private void readAudioSourceWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
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

                // Removed now that 2.1 has been repaced by 2.2
                //// When reading previous versions, only the raw data is required, because it is being upgraded. 
                //// Any extra preparation of the project object is used when interacting with it in the UI, so 
                //// the project utility will not be passed in.
                //if (projectUtility_ != null)
                //{
                //    projectUtility_.AddMapWidgetToGrid(newAudioSourceWidget, (ProjectDto)project);
                //}

                MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                project.MapWidgetProperties.Add(newAudioSourceWidget.Id, properties);

                newAudioSourceWidget.Controller.ResetProperties(properties);

                project.MapWidgets[MapWidgetType.AudioSource].Add(newAudioSourceWidget.Id, newAudioSourceWidget);

                project.MapWidgetsByLayer[newAudioSourceWidget.OwnerId].Add(newAudioSourceWidget.Id, newAudioSourceWidget);
            }
        }

        private void readWorldGeometryWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
            int worldGeometryChunkCount = br.ReadInt32();

            for (int i = 0; i < worldGeometryChunkCount; i++)
            {
                WorldGeometryWidgetDto newWorldGeometryChunkWidget = (WorldGeometryWidgetDto)mapWidgetFactory_.CreateMapWidget(new MapWidgetCreationParametersDto(MapWidgetType.WorldGeometry));

                byte[] guidId = br.ReadBytes(16);
                newWorldGeometryChunkWidget.Id = new Guid(guidId);

                byte[] guidOwnerId = br.ReadBytes(16);
                newWorldGeometryChunkWidget.OwnerId = new Guid(guidOwnerId);

                byte[] guidRootOwnerId = br.ReadBytes(16);
                newWorldGeometryChunkWidget.RootOwnerId = new Guid(guidRootOwnerId);

                newWorldGeometryChunkWidget.Name = br.ReadString();

                newWorldGeometryChunkWidget.Type = MapWidgetType.WorldGeometry;

                int left = br.ReadInt32();
                int top = br.ReadInt32();
                int width = br.ReadInt32();
                int height = br.ReadInt32();

                newWorldGeometryChunkWidget.BoundingBox.Left = left;
                newWorldGeometryChunkWidget.BoundingBox.Top = top;
                newWorldGeometryChunkWidget.BoundingBox.Width = width;
                newWorldGeometryChunkWidget.BoundingBox.Height = height;

                int corner1X = br.ReadInt32();
                int corner1Y = br.ReadInt32();
                int corner2X = br.ReadInt32();
                int corner2Y = br.ReadInt32();

                newWorldGeometryChunkWidget.Corner1.X = corner1X;
                newWorldGeometryChunkWidget.Corner1.Y = corner1Y;
                newWorldGeometryChunkWidget.Corner2.X = corner2X;
                newWorldGeometryChunkWidget.Corner2.Y = corner2Y;

                uint collisionStyle = br.ReadUInt32();
                newWorldGeometryChunkWidget.CollisionStyle = (WorldGeometryCollisionStyle)collisionStyle;

                newWorldGeometryChunkWidget.Edges.UseTopEdge = br.ReadBoolean();
                newWorldGeometryChunkWidget.Edges.UseRightEdge = br.ReadBoolean();
                newWorldGeometryChunkWidget.Edges.UseBottomEdge = br.ReadBoolean();
                newWorldGeometryChunkWidget.Edges.UseLeftEdge = br.ReadBoolean();

                newWorldGeometryChunkWidget.SlopeRise = br.ReadInt32();

                // Removed now that 2.1 has been replaced by 2.2
                //// When reading previous versions, only the raw data is required, because it is being upgraded. 
                //// Any extra preparation of the project object is used when interacting with it in the UI, so 
                //// the project utility will not be passed in.
                //if (projectUtility_ != null)
                //{
                //    projectUtility_.AddMapWidgetToGrid(newWorldGeometryChunkWidget, (ProjectDto_2_1)project);
                //}

                MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                project.MapWidgetProperties.Add(newWorldGeometryChunkWidget.Id, properties);

                newWorldGeometryChunkWidget.Controller.ResetProperties(properties);

                project.MapWidgets[MapWidgetType.WorldGeometry].Add(newWorldGeometryChunkWidget.Id, newWorldGeometryChunkWidget);

                project.MapWidgetsByLayer[newWorldGeometryChunkWidget.OwnerId].Add(newWorldGeometryChunkWidget.Id, newWorldGeometryChunkWidget);
            }
        }

        private void readTileObjectWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
            int tileObjectWidgetCount = br.ReadInt32();

            for (int i = 0; i < tileObjectWidgetCount; i++)
            {
                byte[] guidId = br.ReadBytes(16);
                Guid id = new Guid(guidId);

                byte[] guidOwnerId = br.ReadBytes(16);
                Guid ownerId = new Guid(guidOwnerId);

                byte[] guidRootOwnerId = br.ReadBytes(16);
                Guid rootOwnerId = new Guid(guidRootOwnerId);

                byte[] guidTileObjectId = br.ReadBytes(16);
                Guid tileObjectId = new Guid(guidTileObjectId);
                
                TileObjectWidgetDto newTileObjectWidget = (TileObjectWidgetDto)mapWidgetFactory_.CreateMapWidget(new TileObjectMapWidgetCreationParametersDto(tileObjectId));

                newTileObjectWidget.Id = id;
                newTileObjectWidget.OwnerId = ownerId;
                newTileObjectWidget.RootOwnerId = rootOwnerId;
                newTileObjectWidget.TileObjectId = tileObjectId;

                newTileObjectWidget.Name = br.ReadString();

                newTileObjectWidget.Type = MapWidgetType.TileObject;

                int left = br.ReadInt32();
                int top = br.ReadInt32();
                int width = br.ReadInt32();
                int height = br.ReadInt32();

                newTileObjectWidget.BoundingBox.Left = left;
                newTileObjectWidget.BoundingBox.Top = top;
                newTileObjectWidget.BoundingBox.Width = width;
                newTileObjectWidget.BoundingBox.Height = height;

                if (project.MapWidgetsByLayer.ContainsKey(newTileObjectWidget.OwnerId) == true)
                {
                    // Removed now that 2.1 has been repaced by 2.2
                    //// When reading previous versions, only the raw data is required, because it is being upgraded. 
                    //// Any extra preparation of the project object is used when interacting with it in the UI, so 
                    //// the project utility will not be passed in.
                    //if (projectUtility_ != null)
                    //{
                    //    projectUtility_.AddMapWidgetToGrid(newTileObjectWidget, (ProjectDto)project);
                    //}

                    MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                    project.MapWidgetProperties.Add(newTileObjectWidget.Id, properties);

                    newTileObjectWidget.Controller.ResetProperties(properties);

                    project.MapWidgets[MapWidgetType.TileObject].Add(newTileObjectWidget.Id, newTileObjectWidget);

                    project.MapWidgetsByLayer[newTileObjectWidget.OwnerId].Add(newTileObjectWidget.Id, newTileObjectWidget);
                }
                else
                {
                    // The layer that this tile object was on does not exist. This is because at one point, deleting a layer
                    // did not delete the tile objects that were on it. So the tile objects got saved but the layer didn't.
                    // Now that this is fixed, it should never happen anymore, but I will leave this here as a reminder.
                    System.Diagnostics.Debug.Print("Possible bug. ID 0001.");
                }
            }
        }

        private void readEventWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
            int eventWidgetCount = br.ReadInt32();

            for (int i = 0; i < eventWidgetCount; i++)
            {
                byte[] guidId = br.ReadBytes(16);
                Guid id = new Guid(guidId);

                byte[] guidOwnerId = br.ReadBytes(16);
                Guid ownerId = new Guid(guidOwnerId);

                byte[] guidRootOwnerId = br.ReadBytes(16);
                Guid rootOwnerId = new Guid(guidRootOwnerId);

                byte[] guidEntityId = br.ReadBytes(16);
                Guid entityId = new Guid(guidEntityId);

                EventWidgetDto newEventWidget = (EventWidgetDto)mapWidgetFactory_.CreateMapWidget(new EventMapWidgetCreationParametersDto(entityId));

                newEventWidget.Id = id;
                newEventWidget.OwnerId = ownerId;
                newEventWidget.RootOwnerId = rootOwnerId;
                newEventWidget.EntityId = entityId;

                newEventWidget.Name = br.ReadString();

                newEventWidget.Type = MapWidgetType.Event;

                int left = br.ReadInt32();
                int top = br.ReadInt32();
                int width = br.ReadInt32();
                int height = br.ReadInt32();

                newEventWidget.BoundingBox.Left = left;
                newEventWidget.BoundingBox.Top = top;
                newEventWidget.BoundingBox.Width = width;
                newEventWidget.BoundingBox.Height = height;

                newEventWidget.AcceptInput = br.ReadBoolean();

                // Removed now that 2.1 has been repaced by 2.2
                //// When reading previous versions, only the raw data is required, because it is being upgraded. 
                //// Any extra preparation of the project object is used when interacting with it in the UI, so 
                //// the project utility will not be passed in.
                //if (projectUtility_ != null)
                //{
                //    projectUtility_.AddMapWidgetToGrid(newEventWidget, (ProjectDto)project);
                //}

                MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                project.MapWidgetProperties.Add(newEventWidget.Id, properties);

                newEventWidget.Controller.ResetProperties(properties);

                project.MapWidgets[MapWidgetType.Event].Add(newEventWidget.Id, newEventWidget);

                project.MapWidgetsByLayer[newEventWidget.OwnerId].Add(newEventWidget.Id, newEventWidget);
            }
        }

        private void readActorWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
            int actorWidgetCount = br.ReadInt32();

            for (int i = 0; i < actorWidgetCount; i++)
            {
                byte[] guidId = br.ReadBytes(16);
                Guid id = new Guid(guidId);

                byte[] guidOwnerId = br.ReadBytes(16);
                Guid ownerId = new Guid(guidOwnerId);

                byte[] guidRootOwnerId = br.ReadBytes(16);
                Guid rootOwnerId = new Guid(guidRootOwnerId);

                byte[] guidEntityId = br.ReadBytes(16);
                Guid entityId = new Guid(guidEntityId);

                ActorWidgetDto newActorWidget = (ActorWidgetDto)mapWidgetFactory_.CreateMapWidget(new ActorMapWidgetCreationParametersDto(entityId));

                newActorWidget.Id = id;
                newActorWidget.OwnerId = ownerId;
                newActorWidget.RootOwnerId = rootOwnerId;
                newActorWidget.EntityId = entityId;

                newActorWidget.Name = br.ReadString();

                newActorWidget.Type = MapWidgetType.Actor;

                int left = br.ReadInt32();
                int top = br.ReadInt32();
                int width = br.ReadInt32();
                int height = br.ReadInt32();

                newActorWidget.BoundingBox.Left = left;
                newActorWidget.BoundingBox.Top = top;
                newActorWidget.BoundingBox.Width = width;
                newActorWidget.BoundingBox.Height = height;

                newActorWidget.RenderOrder = br.ReadInt32();
                newActorWidget.AttachToCamera = br.ReadBoolean();
                newActorWidget.AcceptInput = br.ReadBoolean();

                // Removed now that 2.1 has been repaced by 2.2
                //// When reading previous versions, only the raw data is required, because it is being upgraded. 
                //// Any extra preparation of the project object is used when interacting with it in the UI, so 
                //// the project utility will not be passed in.
                //if (projectUtility_ != null)
                //{
                //    projectUtility_.AddMapWidgetToGrid(newActorWidget, (ProjectDto)project);
                //}

                MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                project.MapWidgetProperties.Add(newActorWidget.Id, properties);

                newActorWidget.Controller.ResetProperties(properties);

                project.MapWidgets[MapWidgetType.Actor].Add(newActorWidget.Id, newActorWidget);

                project.MapWidgetsByLayer[newActorWidget.OwnerId].Add(newActorWidget.Id, newActorWidget);
            }
        }

        private void readHudElementWidgets(ProjectDto_2_1 project, BinaryReader br)
        {
            int hudElementWidgetCount = br.ReadInt32();

            for (int i = 0; i < hudElementWidgetCount; i++)
            {
                byte[] guidId = br.ReadBytes(16);
                Guid id = new Guid(guidId);

                byte[] guidOwnerId = br.ReadBytes(16);
                Guid ownerId = new Guid(guidOwnerId);

                byte[] guidRootOwnerId = br.ReadBytes(16);
                Guid rootOwnerId = new Guid(guidRootOwnerId);

                byte[] guidEntityId = br.ReadBytes(16);
                Guid entityId = new Guid(guidEntityId);

                HudElementWidgetDto newHudElementWidget = (HudElementWidgetDto)mapWidgetFactory_.CreateMapWidget(new HudElementMapWidgetCreationParametersDto(entityId));

                newHudElementWidget.Id = id;
                newHudElementWidget.OwnerId = ownerId;
                newHudElementWidget.RootOwnerId = rootOwnerId;
                newHudElementWidget.EntityId = entityId;

                newHudElementWidget.Name = br.ReadString();

                newHudElementWidget.Type = MapWidgetType.HudElement;

                int left = br.ReadInt32();
                int top = br.ReadInt32();
                int width = br.ReadInt32();
                int height = br.ReadInt32();

                newHudElementWidget.BoundingBox.Left = left;
                newHudElementWidget.BoundingBox.Top = top;
                newHudElementWidget.BoundingBox.Width = width;
                newHudElementWidget.BoundingBox.Height = height;

                newHudElementWidget.RenderOrder = br.ReadInt32();

                newHudElementWidget.AcceptInput = br.ReadBoolean();

                // Removed now that 2.1 has been repaced by 2.2
                //// When reading previous versions, only the raw data is required, because it is being upgraded. 
                //// Any extra preparation of the project object is used when interacting with it in the UI, so 
                //// the project utility will not be passed in.
                //if (projectUtility_ != null)
                //{
                //    projectUtility_.AddMapWidgetToRoom(newHudElementWidget, project);
                //}

                MapWidgetProperties properties = new MapWidgetProperties(projectController_);

                project.MapWidgetProperties.Add(newHudElementWidget.Id, properties);

                newHudElementWidget.Controller.ResetProperties(properties);

                project.MapWidgets[MapWidgetType.HudElement].Add(newHudElementWidget.Id, newHudElementWidget);                
            }
        }

        private void readWidgetProperties(ProjectDto_2_1 project, BinaryReader br)
        {
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
                    Guid ownerId = new Guid(guidOwnerId);

                    // Temporarily need to map the owner ID of the event instance to the new ID of the map widget.
                    newProperty.OwnerId = ownerId;

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
                            System.Diagnostics.Debug.Print(newProperty.Name);

                            project.MapWidgetProperties[newProperty.OwnerId].AddProperty(newProperty);

                            break;
                    }
                }
            }
        }

        private void setAbsolutePaths(ProjectDto_2_1 project)
        {
            if (uriUtility_ != null)
            {
                project.ProjectFolderFullPath = uriUtility_.GetFullPath(project.ProjectFolderRelativePath);

                foreach (KeyValuePair<Guid, ScriptDto> script in project.Scripts)
                {
                    script.Value.ScriptPath = uriUtility_.GetFullPath(script.Value.ScriptRelativePath);
                }

                foreach (KeyValuePair<Guid, BitmapResourceDto> bitmap in project.Resources.Bitmaps)
                {
                    bitmap.Value.Path = uriUtility_.GetFullPath(bitmap.Value.RelativePath);
                }

                foreach (KeyValuePair<Guid, AudioResourceDto> audio in project.Resources.AudioData)
                {
                    audio.Value.Path = uriUtility_.GetFullPath(audio.Value.RelativePath);
                }
            }
        }

        #endregion
    }
}
