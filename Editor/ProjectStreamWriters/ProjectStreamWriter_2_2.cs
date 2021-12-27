using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    class ProjectStreamWriter_2_2 : IProjectStreamWriter
    {
        // Don't need to include the resources in the stream when saving to the undo/redo stack. 
        // Only need it when saving to disk.
        public ProjectStreamWriter_2_2(ProjectUiStateDto projectUiState, IUriUtility uriUtility)
        {
            projectUiState_ = projectUiState;
            
            uriUtility_ = uriUtility;
        }
        
        private ProjectUiStateDto projectUiState_ = null;
        
        IUriUtility uriUtility_;

        public void WriteProjectToStream(BaseProjectDto project, Stream stream)
        {
            ProjectDto_2_2 project_2_2 = (ProjectDto_2_2)project;
            
            BinaryWriter bw = new BinaryWriter(stream);

            try
            {
                // File type.
                bw.Write("FMPROJ");

                // File major version number. 
                bw.Write(project_2_2.FileVersion.Major);

                // File minor version number.
                // Should be incremented for smallbut breaking fixes to the project_2_2 file.
                bw.Write(project_2_2.FileVersion.Minor);

                // File revision number.
                // Should be incremented for small but nonbreaking fixes to the project_2_2 file.
                bw.Write(project_2_2.FileVersion.Revision);

                bw.Write(project_2_2.ProjectFolderFullPath);
                bw.Write(project_2_2.ProjectFolderRelativePath);
                bw.Write(project_2_2.ProjectName);
                bw.Write(project_2_2.CameraHeight);
                bw.Write(project_2_2.CameraWidth);
                bw.Write(project_2_2.TileSize);

                byte[] guidInitialRoomId = project_2_2.InitialRoomId.ToByteArray();
                bw.Write(guidInitialRoomId);
                
                // Write the bitmap resource data.
                bw.Write(project_2_2.Bitmaps.Count);

                foreach (KeyValuePair<Guid, BitmapResourceDto> bitmapResource in project_2_2.Bitmaps)
                {
                    BitmapResourceDto bitmap = bitmapResource.Value;

                    byte[] guidId = bitmap.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = bitmap.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = bitmap.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(bitmap.Path);
                    bw.Write(bitmap.RelativePath);                    
                }

                // Write the audio resource data.
                bw.Write(project_2_2.AudioData.Count);

                foreach (KeyValuePair<Guid, AudioResourceDto> audioResource in project_2_2.AudioData)
                {
                    AudioResourceDto audio = audioResource.Value;

                    byte[] guidId = audio.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = audio.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = audio.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(audio.Path);
                    bw.Write(audio.RelativePath);                    
                }
               
                // Write the tilesheet data.                
                int tileSheetCount = project_2_2.TileSheets.Count;

                bw.Write(tileSheetCount);

                for (int i = 0; i < tileSheetCount; i++)
                {
                    Guid tileSheetId = project_2_2.TileSheets[i].Id;

                    byte[] guidId = tileSheetId.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.TileSheets[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.TileSheets[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] bitmapId = project_2_2.TileSheets[i].BitmapResourceId.ToByteArray();
                    bw.Write(bitmapId);

                    bw.Write(project_2_2.TileSheets[i].Name);
                    bw.Write(project_2_2.TileSheets[i].TileSize);
                    bw.Write(project_2_2.TileSheets[i].Columns);
                    bw.Write(project_2_2.TileSheets[i].Rows);
                    bw.Write(project_2_2.TileSheets[i].ScaleFactor);
                }

                // Write the tile object data for each tilesheet.                
                for (int i = 0; i < tileSheetCount; i++)
                {
                    Guid tileSheetId = project_2_2.TileSheets[i].Id;

                    int tileObjectCount = project_2_2.TileObjects[tileSheetId].Count;

                    bw.Write(tileObjectCount);

                    // Next, write the object matrices.
                    for (int j = 0; j < tileObjectCount; j++)
                    {
                        Guid tileObjectId = project_2_2.TileObjects[tileSheetId][j].Id;

                        byte[] guidId = tileObjectId.ToByteArray();
                        bw.Write(guidId);

                        Guid tileObjectOwnerId = project_2_2.TileObjects[tileSheetId][j].OwnerId;

                        byte[] guidOwnerId = tileObjectOwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        Guid tileObjectRootOwnerId = project_2_2.TileObjects[tileSheetId][j].RootOwnerId;

                        byte[] guidRootOwnerId = tileObjectRootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        byte[] bitmapId = project_2_2.TileObjects[tileSheetId][j].BitmapResourceId.ToByteArray();
                        bw.Write(bitmapId);

                        string name = project_2_2.TileObjects[tileSheetId][j].Name;

                        bw.Write(name);

                        int cols = project_2_2.TileObjects[tileSheetId][j].Columns;
                        int rows = project_2_2.TileObjects[tileSheetId][j].Rows;

                        int tlcCols = project_2_2.TileObjects[tileSheetId][j].TopLeftCornerColumn;
                        int tlcRows = project_2_2.TileObjects[tileSheetId][j].TopLeftCornerRow;

                        bw.Write(cols);
                        bw.Write(rows);

                        bw.Write(tlcCols);
                        bw.Write(tlcRows);
                        
                        byte[] animationId = project_2_2.TileObjects[tileSheetId][j].AnimationId.ToByteArray();

                        bw.Write(animationId);
                    }
                }


                // Write the tile object data for each tilesheet.                
                for (int i = 0; i < tileSheetCount; i++)
                {
                    Guid tileSheetId = project_2_2.TileSheets[i].Id;
                    
                    int sceneryAnimationCount = project_2_2.SceneryAnimations[tileSheetId].Count;

                    bw.Write(sceneryAnimationCount);

                    // Write the scenery animation data for each tilesheet.                
                    for (int j = 0; j < sceneryAnimationCount; j++)
                    {
                        Guid sceneryAnimationId = project_2_2.SceneryAnimations[tileSheetId][j].Id;
                        
                        byte[] guidId = sceneryAnimationId.ToByteArray();
                        bw.Write(guidId);

                        Guid sceneryAnimationOwnerId = project_2_2.SceneryAnimations[tileSheetId][j].OwnerId;

                        byte[] guidOwnerId = sceneryAnimationOwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        Guid sceneryAnimationRootOwnerId = project_2_2.SceneryAnimations[tileSheetId][j].RootOwnerId;

                        byte[] guidRootOwnerId = sceneryAnimationRootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);
                        
                        string name = project_2_2.SceneryAnimations[tileSheetId][j].Name;

                        bw.Write(name);
                        
                        int framesPerSecond = project_2_2.SceneryAnimations[tileSheetId][j].FramesPerSecond;
                        
                        bw.Write(framesPerSecond);
                    }
                }

                // Write the sprite sheets.                 
                int spriteSheetCount = project_2_2.SpriteSheets.Count;

                bw.Write(spriteSheetCount);

                for (int i = 0; i < spriteSheetCount; i++)
                {
                    byte[] guidId = project_2_2.SpriteSheets[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.SpriteSheets[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.SpriteSheets[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidBitmapId = project_2_2.SpriteSheets[i].BitmapResourceId.ToByteArray();
                    bw.Write(guidBitmapId);

                    bw.Write(project_2_2.SpriteSheets[i].Name);
                    bw.Write(project_2_2.SpriteSheets[i].Columns);
                    bw.Write(project_2_2.SpriteSheets[i].Rows);
                    bw.Write(project_2_2.SpriteSheets[i].CellWidth);
                    bw.Write(project_2_2.SpriteSheets[i].CellHeight);
                    bw.Write(project_2_2.SpriteSheets[i].ScaleFactor);
                    bw.Write(project_2_2.SpriteSheets[i].Padding);
                }

                // Write the audio resources.
                int audioCount = project_2_2.AudioAssets.Count;

                bw.Write(audioCount);

                for (int i = 0; i < audioCount; i++)
                {
                    byte[] guidId = project_2_2.AudioAssets[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.AudioAssets[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.AudioAssets[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidAudioResourceId = project_2_2.AudioAssets[i].AudioResourceId.ToByteArray();
                    bw.Write(guidAudioResourceId);

                    bw.Write(project_2_2.AudioAssets[i].Name);
                    bw.Write(project_2_2.AudioAssets[i].Channel);
                    //bw.Write(project_2_2.AudioAssets[i].Loop);
                    //bw.Write(project_2_2.AudioAssets[i].Volume);
                }

                // Write the loading screen data.
                int loadingScreenCount = project_2_2.LoadingScreens.Count;

                bw.Write(loadingScreenCount);

                for (int i = 0; i < loadingScreenCount; i++)
                {
                    byte[] guidId = project_2_2.LoadingScreens[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.LoadingScreens[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.LoadingScreens[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.LoadingScreens[i].Name);
                }

                // Write the transition data.
                int transitionCount = project_2_2.Transitions.Count;

                bw.Write(transitionCount);

                for (int i = 0; i < transitionCount; i++)
                {
                    byte[] guidId = project_2_2.Transitions[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.Transitions[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.Transitions[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.Transitions[i].Name);
                }

                // Write the particle emitter data.
                int particleEmitterCount = project_2_2.ParticleEmitters.Count;

                bw.Write(particleEmitterCount);

                for (int i = 0; i < particleEmitterCount; i++)
                {
                    byte[] guidId = project_2_2.ParticleEmitters[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.ParticleEmitters[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.ParticleEmitters[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.ParticleEmitters[i].Name);
                }

                // Write the particle data.
                int particleCount = project_2_2.Particles.Count;

                bw.Write(particleCount);

                for (int i = 0; i < particleCount; i++)
                {
                    byte[] guidId = project_2_2.Particles[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.Particles[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.Particles[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.Particles[i].Name);
                }

                // Write the actor data.
                int actorCount = project_2_2.Actors.Count;

                bw.Write(actorCount);

                for (int i = 0; i < actorCount; i++)
                {
                    byte[] guidId = project_2_2.Actors[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.Actors[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.Actors[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidInitialState = project_2_2.Actors[i].InitialStateId.ToByteArray();
                    bw.Write(guidInitialState);

                    bw.Write(project_2_2.Actors[i].Name);
                    bw.Write(project_2_2.Actors[i].StageHeight);
                    bw.Write(project_2_2.Actors[i].StageWidth);
                    bw.Write(project_2_2.Actors[i].StageBackgroundDepth);
                    bw.Write((uint)project_2_2.Actors[i].StageOriginLocation);
                    bw.Write(project_2_2.Actors[i].PivotPoint.X);
                    bw.Write(project_2_2.Actors[i].PivotPoint.Y);
                    bw.Write(project_2_2.Actors[i].Tag);
                    bw.Write(project_2_2.Actors[i].KeepRoomActive);

                    byte[] guidEntityClassificationId = project_2_2.Actors[i].Classification.ToByteArray();
                    bw.Write(guidEntityClassificationId);
                }

                // Write the event data.
                int eventCount = project_2_2.Events.Count;

                bw.Write(eventCount);

                for (int i = 0; i < eventCount; i++)
                {
                    byte[] guidId = project_2_2.Events[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.Events[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.Events[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.Events[i].Name);
                    bw.Write(project_2_2.Events[i].Tag);

                    byte[] guidEntityClassificationId = project_2_2.Events[i].Classification.ToByteArray();
                    bw.Write(guidEntityClassificationId);
                }

                // Write the HUD data.
                int hudElementCount = project_2_2.HudElements.Count;

                bw.Write(hudElementCount);

                for (int i = 0; i < hudElementCount; i++)
                {
                    byte[] guidId = project_2_2.HudElements[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.HudElements[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.HudElements[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidInitialStateId = project_2_2.HudElements[i].InitialStateId.ToByteArray();
                    bw.Write(guidInitialStateId);

                    bw.Write(project_2_2.HudElements[i].Name);
                    bw.Write(project_2_2.HudElements[i].StageHeight);
                    bw.Write(project_2_2.HudElements[i].StageWidth);
                    bw.Write(project_2_2.HudElements[i].StageBackgroundDepth);
                    bw.Write((uint)project_2_2.HudElements[i].StageOriginLocation);
                    bw.Write(project_2_2.HudElements[i].PivotPoint.X);
                    bw.Write(project_2_2.HudElements[i].PivotPoint.Y);
                    bw.Write(project_2_2.HudElements[i].Tag);

                    byte[] guidEntityClassificationId = project_2_2.HudElements[i].Classification.ToByteArray();
                    bw.Write(guidEntityClassificationId);
                }

                // Write the spawn point data.
                int spawnPointCount = project_2_2.SpawnPoints.Count;

                bw.Write(spawnPointCount);

                for (int i = 0; i < spawnPointCount; i++)
                {
                    byte[] guidId = project_2_2.SpawnPoints[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.SpawnPoints[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.SpawnPoints[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.SpawnPoints[i].Name);
                }

                // Write the animation group data.
                List<AnimationGroupDto> animationGroups = project_2_2.AnimationGroups;

                bw.Write(animationGroups.Count);

                for (int i = 0; i < animationGroups.Count; i++)
                {
                    byte[] guidId = animationGroups[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = animationGroups[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = animationGroups[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(animationGroups[i].Name);


                    // Write the animation data.                
                    List<AnimationDto> animations = project_2_2.Animations[animationGroups[i].Id];

                    bw.Write(animations.Count);

                    for (int j = 0; j < animations.Count; j++)
                    {
                        AnimationDto animation = animations[j];

                        byte[] guidAnimationId = animation.Id.ToByteArray();
                        bw.Write(guidAnimationId);

                        byte[] guidAnimationOwnerId = animation.OwnerId.ToByteArray();
                        bw.Write(guidAnimationOwnerId);

                        byte[] guidAnimationRootOwnerId = animation.RootOwnerId.ToByteArray();
                        bw.Write(guidAnimationRootOwnerId);

                        byte[] guidAnimationGroupId = animation.GroupId.ToByteArray();
                        bw.Write(guidAnimationGroupId);

                        byte[] guidSpriteSheetId = animation.SpriteSheet.ToByteArray();
                        bw.Write(guidSpriteSheetId);

                        byte[] guidAlphaMaskSheetId = animation.AlphaMaskSheet.ToByteArray();
                        bw.Write(guidAlphaMaskSheetId);

                        bw.Write(animation.Name);          
                    }
                }

                // Write the state data.
                bw.Write(project_2_2.States.Count);

                foreach (KeyValuePair<Guid, List<StateDto>> stateList in project_2_2.States)
                {
                    List<StateDto> states = stateList.Value;

                    bw.Write(states.Count);

                    for (int i = 0; i < states.Count; i++)
                    {
                        byte[] guidId = states[i].Id.ToByteArray();
                        bw.Write(guidId);

                        byte[] guidOwnerId = states[i].OwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        byte[] guidRootOwnerId = states[i].RootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        bw.Write(states[i].Name);
                    }
                }

                // Write the frame data.
                bw.Write(project_2_2.Frames.Count);

                foreach (KeyValuePair<Guid, List<FrameDto>> frameList in project_2_2.Frames)
                {
                    List<FrameDto> frames = frameList.Value;

                    bw.Write(frames.Count);

                    for (int i = 0; i < frames.Count; i++)
                    {
                        byte[] guidId = frames[i].Id.ToByteArray();
                        bw.Write(guidId);

                        byte[] guidOwnerId = frames[i].OwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        byte[] guidRootOwnerId = frames[i].RootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        bw.Write(frames[i].Name);

                        if (frames[i].SheetCellIndex.HasValue == true)
                        {
                            bw.Write(frames[i].SheetCellIndex.Value);
                        }
                        else
                        {
                            bw.Write(-1);
                        }

                        if (frames[i].AlphaMaskCellIndex.HasValue == true)
                        {
                            bw.Write(frames[i].AlphaMaskCellIndex.Value);
                        }
                        else
                        {
                            bw.Write(-1);
                        }
                    }
                }

                // Write the trigger signal data.              
                List<TriggerSignalDto> triggerSignals = project_2_2.TriggerSignals;

                bw.Write(triggerSignals.Count);

                for (int i = 0; i < triggerSignals.Count; i++)
                {
                    byte[] guidId = triggerSignals[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = triggerSignals[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = triggerSignals[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(triggerSignals[i].Name);
                }

                // Write the frame trigger data.
                bw.Write(project_2_2.FrameTriggers.Count);

                foreach (KeyValuePair<Guid, List<FrameTriggerDto>> frameTriggerList in project_2_2.FrameTriggers)
                {
                    List<FrameTriggerDto> frameTriggers = frameTriggerList.Value;

                    bw.Write(frameTriggers.Count);

                    for (int i = 0; i < frameTriggers.Count; i++)
                    {
                        byte[] guidId = frameTriggers[i].Id.ToByteArray();
                        bw.Write(guidId);

                        byte[] guidOwnerId = frameTriggers[i].OwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        byte[] guidRootOwnerId = frameTriggers[i].RootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        byte[] guidTriggerSignalId = frameTriggers[i].Signal.ToByteArray();
                        bw.Write(guidTriggerSignalId);
                    }
                }

                // Write the action point data.
                bw.Write(project_2_2.ActionPoints.Count);

                foreach (KeyValuePair<Guid, List<ActionPointDto>> actionPointList in project_2_2.ActionPoints)
                {
                    List<ActionPointDto> actionPoints = actionPointList.Value;

                    bw.Write(actionPoints.Count);

                    for (int i = 0; i < actionPoints.Count; i++)
                    {
                        byte[] guidId = actionPoints[i].Id.ToByteArray();
                        bw.Write(guidId);

                        byte[] guidOwnerId = actionPoints[i].OwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        byte[] guidRootOwnerId = actionPoints[i].RootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        string name = actionPoints[i].Name;
                        bw.Write(name);

                        int positionLeft = actionPoints[i].Position.X;
                        bw.Write(positionLeft);

                        int positionTop = actionPoints[i].Position.Y;
                        bw.Write(positionTop);
                    }
                }

                // Write the hitbox identity data.               
                List<HitboxIdentityDto> hitboxIdentities = project_2_2.HitboxIdentities;

                bw.Write(hitboxIdentities.Count);

                for (int i = 0; i < hitboxIdentities.Count; i++)
                {
                    byte[] guidId = hitboxIdentities[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = hitboxIdentities[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = hitboxIdentities[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(hitboxIdentities[i].Name);
                }

                // Write the hitbox data.
                bw.Write(project_2_2.Hitboxes.Count);

                foreach (KeyValuePair<Guid, List<HitboxDto>> hitboxList in project_2_2.Hitboxes)
                {
                    List<HitboxDto> hitboxes = hitboxList.Value;

                    bw.Write(hitboxes.Count);

                    for (int i = 0; i < hitboxes.Count; i++)
                    {
                        byte[] guidId = hitboxes[i].Id.ToByteArray();
                        bw.Write(guidId);

                        byte[] guidOwnerId = hitboxes[i].OwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        byte[] guidRootOwnerId = hitboxes[i].RootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        byte[] guidHitboxIdentityId = hitboxes[i].Identity.ToByteArray();
                        bw.Write(guidHitboxIdentityId);

                        bw.Write(hitboxes[i].HitboxRect.Left);
                        bw.Write(hitboxes[i].HitboxRect.Top);
                        bw.Write(hitboxes[i].HitboxRect.Width);
                        bw.Write(hitboxes[i].HitboxRect.Height);
                        bw.Write(hitboxes[i].IsSolid);
                        bw.Write((int)hitboxes[i].Priority);
                        bw.Write(hitboxes[i].RotationDegrees);
                    }
                }

                // Write the animation slot data.
                bw.Write(project_2_2.AnimationSlots.Count);

                foreach (KeyValuePair<Guid, List<AnimationSlotDto>> animationSlotList in project_2_2.AnimationSlots)
                {
                    List<AnimationSlotDto> animationSlots = animationSlotList.Value;

                    bw.Write(animationSlots.Count);

                    for (int i = 0; i < animationSlots.Count; i++)
                    {
                        byte[] guidId = animationSlots[i].Id.ToByteArray();
                        bw.Write(guidId);

                        byte[] guidOwnerId = animationSlots[i].OwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        byte[] guidRootOwnerId = animationSlots[i].RootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        byte[] guidAnimationId = animationSlots[i].Animation.ToByteArray();
                        bw.Write(guidAnimationId);

                        bw.Write(animationSlots[i].Name);

                        bw.Write(animationSlots[i].Background);

                        bw.Write(animationSlots[i].Position.X);
                        bw.Write(animationSlots[i].Position.Y);

                        bw.Write(animationSlots[i].HueColor.Red);
                        bw.Write(animationSlots[i].HueColor.Green);
                        bw.Write(animationSlots[i].HueColor.Blue);
                        bw.Write(animationSlots[i].HueColor.Alpha);

                        bw.Write(animationSlots[i].BlendColor.Red);
                        bw.Write(animationSlots[i].BlendColor.Green);
                        bw.Write(animationSlots[i].BlendColor.Blue);
                        bw.Write(animationSlots[i].BlendColor.Alpha);

                        bw.Write(animationSlots[i].BlendPercent);

                        bw.Write(animationSlots[i].PivotPoint.X);
                        bw.Write(animationSlots[i].PivotPoint.Y);

                        bw.Write(animationSlots[i].AlphaGradientFrom);
                        bw.Write(animationSlots[i].AlphaGradientTo);
                        bw.Write(animationSlots[i].AlphaGradientRadius);
                        bw.Write(animationSlots[i].AlphaGradientRadialCenter.X);
                        bw.Write(animationSlots[i].AlphaGradientRadialCenter.Y);
                        bw.Write(Convert.ToUInt32(animationSlots[i].AlphaGradientDirection));
                        bw.Write(Convert.ToUInt32(animationSlots[i].OriginLocation));

                        byte[] guidNextStateId = animationSlots[i].NextStateId.ToByteArray();
                        bw.Write(guidNextStateId);
                        
                        bw.Write((int)animationSlots[i].AnimationStyle);

                        bw.Write(animationSlots[i].UpdateInterval);
                        
                        bw.Write(animationSlots[i].OutlineColor.Red);
                        bw.Write(animationSlots[i].OutlineColor.Green);
                        bw.Write(animationSlots[i].OutlineColor.Blue);
                        bw.Write(animationSlots[i].OutlineColor.Alpha);

                    }
                }

                // Write the property data.
                bw.Write(project_2_2.Properties.Count);

                foreach (KeyValuePair<Guid, List<PropertyDto>> propertyList in project_2_2.Properties)
                {
                    List<PropertyDto> properties = propertyList.Value;

                    bw.Write(properties.Count);

                    for (int i = 0; i < properties.Count; i++)
                    {
                        byte[] guidId = properties[i].Id.ToByteArray();
                        bw.Write(guidId);

                        byte[] guidOwnerId = properties[i].OwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        byte[] guidRootOwnerId = properties[i].RootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        bw.Write(properties[i].Name);
                        bw.Write(properties[i].DefaultValue);
                    }
                }

                // Removed in 2.1
                //// Write the query data.
                //int queryCount = project_2_2.Queries.Count;

                //bw.Write(queryCount);

                //for (int i = 0; i < queryCount; i++)
                //{
                //    byte[] guidId = project_2_2.Queries[i].Id.ToByteArray();
                //    bw.Write(guidId);

                //    byte[] guidOwnerId = project_2_2.Queries[i].OwnerId.ToByteArray();
                //    bw.Write(guidOwnerId);

                //    byte[] guidRootOwnerId = project_2_2.Queries[i].RootOwnerId.ToByteArray();
                //    bw.Write(guidRootOwnerId);

                //    bw.Write(project_2_2.Queries[i].Name);                    
                //}

                // Write the game button data.
                int gameButtonCount = project_2_2.GameButtons.Count;

                bw.Write(gameButtonCount);

                for (int i = 0; i < gameButtonCount; i++)
                {
                    byte[] guidId = project_2_2.GameButtons[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.GameButtons[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.GameButtons[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.GameButtons[i].Name);

                    byte[] guidGroupId = project_2_2.GameButtons[i].Group.ToByteArray();
                    bw.Write(guidGroupId);

                    bw.Write(project_2_2.GameButtons[i].Label);
                }

                // Write the game button group data.
                int gameButtonGroupCount = project_2_2.GameButtonGroups.Count;

                bw.Write(gameButtonGroupCount);

                for (int i = 0; i < gameButtonGroupCount; i++)
                {
                    byte[] guidId = project_2_2.GameButtonGroups[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.GameButtonGroups[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.GameButtonGroups[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.GameButtonGroups[i].Name);
                }

                int uiWidgetCount = project_2_2.UiWidgets.Count;

                bw.Write(uiWidgetCount);

                for (int i = 0; i < uiWidgetCount; i++)
                {
                    byte[] guidId = project_2_2.UiWidgets[i].Id.ToByteArray();
                    bw.Write(guidId);

                    bw.Write(project_2_2.UiWidgets[i].Name);
                }

                // Write the script data
                int scriptCount = project_2_2.Scripts.Count;

                bw.Write(scriptCount);

                foreach (KeyValuePair<Guid, ScriptDto> script in project_2_2.Scripts)
                {
                    byte[] guidId = script.Value.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = script.Value.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = script.Value.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(script.Value.Name);
                    bw.Write(Convert.ToUInt32(script.Value.ScriptType));
                    bw.Write(script.Value.ScriptPath);
                    bw.Write(script.Value.ScriptRelativePath);
                }

                // Write the data file data
                int dataFileCount = project_2_2.DataFiles.Count;

                bw.Write(dataFileCount);

                foreach (KeyValuePair<Guid, DataFileDto> dataFile in project_2_2.DataFiles)
                {
                    byte[] guidId = dataFile.Value.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = dataFile.Value.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = dataFile.Value.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(dataFile.Value.Name);
                    bw.Write(dataFile.Value.Extension);
                    bw.Write(dataFile.Value.FilePath);
                    bw.Write(dataFile.Value.FileRelativePath);
                }

                // Write the room data.
                bw.Write(project_2_2.Rooms.Count);

                for (int i = 0; i < project_2_2.Rooms.Count; i++)
                {
                    byte[] guidId = project_2_2.Rooms[i].Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = project_2_2.Rooms[i].OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = project_2_2.Rooms[i].RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(project_2_2.Rooms[i].Name);

                    byte[] guidLoadingScreenId = project_2_2.Rooms[i].LoadingScreenId.ToByteArray();
                    bw.Write(guidLoadingScreenId);

                    byte[] guidTransitionId = project_2_2.Rooms[i].TransitionId.ToByteArray();
                    bw.Write(guidTransitionId);
                }

                // Write the layer data
                bw.Write(project_2_2.Layers.Count);

                foreach (KeyValuePair<Guid, List<LayerDto>> layerList in project_2_2.Layers)
                {
                    Guid roomId = layerList.Key;
                    List<LayerDto> layers = layerList.Value;

                    bw.Write(layers.Count);

                    for (int i = 0; i < layers.Count; i++)
                    {
                        // Write the layers in ordinal order. If the project UI state does not exist, assume they are already in order.
                        int index = projectUiState_?.LayerOrdinalToIndexMap[roomId][i] ?? i;

                        byte[] guidId = layers[index].Id.ToByteArray();
                        bw.Write(guidId);

                        byte[] guidOwnerId = layers[index].OwnerId.ToByteArray();
                        bw.Write(guidOwnerId);

                        byte[] guidRootOwnerId = layers[index].RootOwnerId.ToByteArray();
                        bw.Write(guidRootOwnerId);

                        int cols = layers[index].Cols;
                        int rows = layers[index].Rows;

                        bw.Write(layers[index].Name);
                        bw.Write(cols);
                        bw.Write(rows);
                    }
                }

                // Write the interactive layer data.
                bw.Write(project_2_2.InteractiveLayerIndexes.Count);

                foreach (KeyValuePair<Guid, int> interactiveLayerIndex in project_2_2.InteractiveLayerIndexes)
                {
                    byte[] guidRoomId = interactiveLayerIndex.Key.ToByteArray();
                    bw.Write(guidRoomId);

                    // If the project UI state is null, assume the layers are in order, and no mapping is needed.
                    if (projectUiState_ != null)
                    {
                        bw.Write(projectUiState_.LayerOrdinalToIndexMap[interactiveLayerIndex.Key].IndexOf(interactiveLayerIndex.Value));
                    }
                    else
                    {
                        bw.Write(interactiveLayerIndex.Value);
                    }
                }

                // Add map widgets here.

                // Write the map widget data.
                int spawnPointInstanceCount = project_2_2.MapWidgets[MapWidgetType.SpawnPoint].Count;

                bw.Write(spawnPointInstanceCount);

                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_2_2.MapWidgets[MapWidgetType.SpawnPoint])
                {
                    SpawnPointWidgetDto spawnPoint = (SpawnPointWidgetDto)kvp.Value;

                    byte[] guidId = spawnPoint.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = spawnPoint.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = spawnPoint.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidIdentity = spawnPoint.Identity.ToByteArray();
                    bw.Write(guidIdentity);

                    bw.Write(Convert.ToInt32(spawnPoint.Type));
                    bw.Write(spawnPoint.Name);
                    bw.Write(spawnPoint.BoundingBox.Left);
                    bw.Write(spawnPoint.BoundingBox.Top);
                    bw.Write(spawnPoint.BoundingBox.Width);
                    bw.Write(spawnPoint.BoundingBox.Height);
                    bw.Write(spawnPoint.Position.X);
                    bw.Write(spawnPoint.Position.Y);

                    bw.Write(spawnPoint.IdentityName);
                }

                int particleEmitterInstanceCount = project_2_2.MapWidgets[MapWidgetType.ParticleEmitter].Count;

                bw.Write(particleEmitterInstanceCount);

                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_2_2.MapWidgets[MapWidgetType.ParticleEmitter])
                {
                    ParticleEmitterWidgetDto particleEmitter = (ParticleEmitterWidgetDto)kvp.Value;

                    byte[] guidId = particleEmitter.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = particleEmitter.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = particleEmitter.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidParticleTypeId = particleEmitter.ParticleType.ToByteArray();
                    bw.Write(guidParticleTypeId);

                    byte[] guidParticleEmitterId = particleEmitter.Behavior.ToByteArray();
                    bw.Write(guidParticleEmitterId);

                    byte[] guidSpriteSheetId = particleEmitter.Animation.ToByteArray();
                    bw.Write(guidSpriteSheetId);

                    bw.Write(Convert.ToInt32(particleEmitter.Type));
                    bw.Write(particleEmitter.Name);
                    bw.Write(particleEmitter.BoundingBox.Left);
                    bw.Write(particleEmitter.BoundingBox.Top);
                    bw.Write(particleEmitter.BoundingBox.Width);
                    bw.Write(particleEmitter.BoundingBox.Height);
                    bw.Write(particleEmitter.Position.X);
                    bw.Write(particleEmitter.Position.Y);

                    bw.Write(particleEmitter.ParticleTypeName);
                    bw.Write(particleEmitter.BehaviorName);
                    bw.Write(particleEmitter.AnimationName);
                    bw.Write(particleEmitter.ParticlesPerEmission);
                    bw.Write(particleEmitter.MaxParticles);
                    bw.Write(particleEmitter.Interval);
                    bw.Write(particleEmitter.ParticleLifespan);
                    bw.Write(particleEmitter.Active);
                    bw.Write(particleEmitter.AttachParticles);
                    bw.Write(particleEmitter.AnimationFramesPerSecond);
                }

                int audioSourceInstanceCount = project_2_2.MapWidgets[MapWidgetType.AudioSource].Count;

                bw.Write(audioSourceInstanceCount);

                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_2_2.MapWidgets[MapWidgetType.AudioSource])
                {
                    AudioSourceWidgetDto audioSource = (AudioSourceWidgetDto)kvp.Value;

                    byte[] guidId = audioSource.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = audioSource.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = audioSource.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidAudioId = audioSource.Audio.ToByteArray();
                    bw.Write(guidAudioId);

                    bw.Write(audioSource.Name);
                    bw.Write(audioSource.BoundingBox.Left);
                    bw.Write(audioSource.BoundingBox.Top);
                    bw.Write(audioSource.BoundingBox.Width);
                    bw.Write(audioSource.BoundingBox.Height);
                    bw.Write(audioSource.Position.X);
                    bw.Write(audioSource.Position.Y);

                    bw.Write(audioSource.Autoplay);
                    bw.Write(audioSource.Loop);
                    bw.Write(audioSource.MinDistance);
                    bw.Write(audioSource.MaxDistance);
                    bw.Write(audioSource.Volume);
                    bw.Write(audioSource.AudioName);
                }

                int worldGeometryChunkCount = project_2_2.MapWidgets[MapWidgetType.WorldGeometry].Count;

                bw.Write(worldGeometryChunkCount);

                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_2_2.MapWidgets[MapWidgetType.WorldGeometry])
                {
                    WorldGeometryWidgetDto worldGeometryChunk = (WorldGeometryWidgetDto)kvp.Value;

                    byte[] guidId = worldGeometryChunk.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = worldGeometryChunk.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = worldGeometryChunk.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    bw.Write(worldGeometryChunk.Name);
                    bw.Write(worldGeometryChunk.BoundingBox.Left);
                    bw.Write(worldGeometryChunk.BoundingBox.Top);
                    bw.Write(worldGeometryChunk.BoundingBox.Width);
                    bw.Write(worldGeometryChunk.BoundingBox.Height);
                    bw.Write(worldGeometryChunk.Position.X);
                    bw.Write(worldGeometryChunk.Position.Y);

                    bw.Write(worldGeometryChunk.Corner1.X);
                    bw.Write(worldGeometryChunk.Corner1.Y);
                    bw.Write(worldGeometryChunk.Corner2.X);
                    bw.Write(worldGeometryChunk.Corner2.Y);
                    bw.Write(Convert.ToUInt32(worldGeometryChunk.CollisionStyle));
                    bw.Write(worldGeometryChunk.Edges.UseTopEdge);
                    bw.Write(worldGeometryChunk.Edges.UseRightEdge);
                    bw.Write(worldGeometryChunk.Edges.UseBottomEdge);
                    bw.Write(worldGeometryChunk.Edges.UseLeftEdge);
                    bw.Write(worldGeometryChunk.SlopeRise);
                }

                int tileObjectWidgetCount = project_2_2.MapWidgets[MapWidgetType.TileObject].Count;

                bw.Write(tileObjectWidgetCount);

                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_2_2.MapWidgets[MapWidgetType.TileObject])
                {
                    TileObjectWidgetDto tileObject = (TileObjectWidgetDto)kvp.Value;

                    byte[] guidId = tileObject.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = tileObject.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = tileObject.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidTileObjectId = tileObject.TileObjectId.ToByteArray();
                    bw.Write(guidTileObjectId);

                    bw.Write(tileObject.Name);
                    bw.Write(tileObject.BoundingBox.Left);
                    bw.Write(tileObject.BoundingBox.Top);
                    bw.Write(tileObject.BoundingBox.Width);
                    bw.Write(tileObject.BoundingBox.Height);
                    bw.Write(tileObject.Position.X);
                    bw.Write(tileObject.Position.Y);
                }

                int eventWidgetCount = project_2_2.MapWidgets[MapWidgetType.Event].Count;

                bw.Write(eventWidgetCount);

                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_2_2.MapWidgets[MapWidgetType.Event])
                {
                    EventWidgetDto eventWidget = (EventWidgetDto)kvp.Value;

                    byte[] guidId = eventWidget.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = eventWidget.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = eventWidget.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidEntityId = eventWidget.EntityId.ToByteArray();
                    bw.Write(guidEntityId);

                    bw.Write(eventWidget.Name);
                    bw.Write(eventWidget.BoundingBox.Left);
                    bw.Write(eventWidget.BoundingBox.Top);
                    bw.Write(eventWidget.BoundingBox.Width);
                    bw.Write(eventWidget.BoundingBox.Height);
                    bw.Write(eventWidget.Position.X);
                    bw.Write(eventWidget.Position.Y);
                    bw.Write(eventWidget.AcceptInput);
                }

                int actorWidgetCount = project_2_2.MapWidgets[MapWidgetType.Actor].Count;

                bw.Write(actorWidgetCount);

                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_2_2.MapWidgets[MapWidgetType.Actor])
                {
                    ActorWidgetDto actorWidget = (ActorWidgetDto)kvp.Value;

                    byte[] guidId = actorWidget.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = actorWidget.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = actorWidget.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidEntityId = actorWidget.EntityId.ToByteArray();
                    bw.Write(guidEntityId);

                    bw.Write(actorWidget.Name);
                    bw.Write(actorWidget.BoundingBox.Left);
                    bw.Write(actorWidget.BoundingBox.Top);
                    bw.Write(actorWidget.BoundingBox.Width);
                    bw.Write(actorWidget.BoundingBox.Height);
                    bw.Write(actorWidget.Position.X);
                    bw.Write(actorWidget.Position.Y);
                    bw.Write(actorWidget.RenderOrder);
                    bw.Write(actorWidget.AttachToCamera);
                    bw.Write(actorWidget.AcceptInput);
                }

                int hudElementWidgetCount = project_2_2.MapWidgets[MapWidgetType.HudElement].Count;

                bw.Write(hudElementWidgetCount);

                foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_2_2.MapWidgets[MapWidgetType.HudElement])
                {
                    HudElementWidgetDto hudElementWidget = (HudElementWidgetDto)kvp.Value;

                    byte[] guidId = hudElementWidget.Id.ToByteArray();
                    bw.Write(guidId);

                    byte[] guidOwnerId = hudElementWidget.OwnerId.ToByteArray();
                    bw.Write(guidOwnerId);

                    byte[] guidRootOwnerId = hudElementWidget.RootOwnerId.ToByteArray();
                    bw.Write(guidRootOwnerId);

                    byte[] guidEntityId = hudElementWidget.EntityId.ToByteArray();
                    bw.Write(guidEntityId);

                    bw.Write(hudElementWidget.Name);
                    bw.Write(hudElementWidget.BoundingBox.Left);
                    bw.Write(hudElementWidget.BoundingBox.Top);
                    bw.Write(hudElementWidget.BoundingBox.Width);
                    bw.Write(hudElementWidget.BoundingBox.Height);
                    bw.Write(hudElementWidget.Position.X);
                    bw.Write(hudElementWidget.Position.Y);
                    bw.Write(hudElementWidget.RenderOrder);
                    bw.Write(hudElementWidget.AcceptInput);
                }

                // Write the instance property data.
                bw.Write(project_2_2.MapWidgetProperties.Count);

                foreach (KeyValuePair<Guid, MapWidgetProperties> MapWidgetPropertiesList in project_2_2.MapWidgetProperties)
                {
                    MapWidgetProperties MapWidgetProperties = MapWidgetPropertiesList.Value;

                    int nonreservedPropertyCount = 0;

                    for (int i = 0; i < MapWidgetProperties.Count; i++)
                    {
                        if (MapWidgetProperties[i].Reserved == false)
                        {
                            nonreservedPropertyCount++;
                        }
                    }

                    bw.Write(nonreservedPropertyCount);

                    for (int i = 0; i < MapWidgetProperties.Count; i++)
                    {
                        if (MapWidgetProperties[i].Reserved == false)
                        {
                            byte[] guidId = MapWidgetProperties[i].Id.ToByteArray();
                            bw.Write(guidId);

                            byte[] guidOwnerId = MapWidgetProperties[i].OwnerId.ToByteArray();
                            bw.Write(guidOwnerId);

                            byte[] guidRootOwnerId = MapWidgetProperties[i].RootOwnerId.ToByteArray();
                            bw.Write(guidRootOwnerId);

                            bw.Write(MapWidgetProperties[i].Name);
                            bw.Write(MapWidgetProperties[i].Value.ToString());
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }        
    }
}
