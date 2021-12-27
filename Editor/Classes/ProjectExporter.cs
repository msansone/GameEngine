using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class ProjectExporter : IProjectExporter
    {
        #region Constructors

        public ProjectExporter(IProjectController projectController, IWarningsForm warningsForm, bool exportScriptsOnly)
        {
            scriptFolder_ = string.Empty; ;
            scriptCoreFolder_ = string.Empty; ;
            shaderFolder_ = string.Empty; ;
            exportPath_ = string.Empty;
            project_ = null;
            projectController_ = projectController;
            warningsForm_ = warningsForm;
            exportVersion_ = new Version(1, 2, 0, 0);
            exportScriptsOnly_ = exportScriptsOnly;
            
            scriptGeneratorFactory_ = new ScriptGeneratorFactory();

            dictAnimations_ = new Dictionary<Guid, List<Rectangle>>();
        }

        #endregion

        #region Private Variables

        private string scriptFolder_;
        private string scriptCoreFolder_;
        private string shaderFolder_;
        private string exportPath_;
        private bool exportScriptsOnly_;
        private Version exportVersion_;
        private ProjectDto project_;
        private IProjectController projectController_;
        private IWarningsForm warningsForm_;
        private IScriptGeneratorFactory scriptGeneratorFactory_;
        private Dictionary<Guid, List<Rectangle>> dictAnimations_;

        #endregion
        
        #region Public Functions

        public void ExportProject(string exportPath, bool showWarnings)
        {
            exportPath_ = exportPath;

            project_ = projectController_.GetProjectDto();

            scriptFolder_ = exportPath_ + "\\Source\\Scripts\\";
            scriptCoreFolder_ = scriptFolder_ + "\\firemelon_core\\";
            shaderFolder_ = exportPath_ + "\\Source\\Shaders\\";

            // This will get rebuilt.
            dictAnimations_.Clear();

            if (exportScriptsOnly_ == false)
            {
                createAllDirectories();

                // The export version should match the firemelon dll version which the assets were built for.            

                // First write the engine init file
                writeHeader();

                // Next write the meta data file.
                writeMetadataFile();

                writeBitmapResourceFiles();

                writeAudioResourceFiles();

                writeTileSheetAssetFiles();

                writeSpriteSheetAssetFiles();

                writeAudioAssetFiles();

                writeActorAssetFiles();

                writeEventAssetFiles();

                writeHudElementAssetFiles();

                writeUiWidgetAssetFiles();

                writeLoadingScreenAssetFiles();

                writeTransitionAssetFiles();

                writeParticleEmitterAssetFiles();

                writeParticleAssetFiles();

                writeRoomData();

                copyScriptFiles();

                copyDataFiles();

                copyBinaryResources();
            }
            else
            {
                // Delete the scripts folder, so no unused files end up left in it.
                if (Directory.Exists(scriptFolder_) == true)
                {
                    Directory.Delete(scriptFolder_, true);
                }
                
                createDirectoryIfNotExist(scriptFolder_);
                createDirectoryIfNotExist(scriptCoreFolder_);

                exportBinaryResource("__init__.py", scriptCoreFolder_);
                exportBinaryResource("entity.py", scriptCoreFolder_);
                exportBinaryResource("entityserializer.py", scriptCoreFolder_);
                exportBinaryResource("uiwidget.py", scriptCoreFolder_);
                exportBinaryResource("loadingscreen.py", scriptCoreFolder_);
                exportBinaryResource("transition.py", scriptCoreFolder_);
                exportBinaryResource("particle.py", scriptCoreFolder_);
                exportBinaryResource("particleemitter.py", scriptCoreFolder_);
                exportBinaryResource("room.py", scriptCoreFolder_);
                exportBinaryResource("query.py", scriptCoreFolder_);
                exportBinaryResource("ui.py", scriptCoreFolder_);
                exportBinaryResource("networkhandler.py", scriptCoreFolder_);

                copyScriptFiles();
            }

            project_ = null;

            GC.Collect();

            if (showWarnings)
            {
                showWarningsDialog();
            }
            
            return;
        }

        #endregion
                
        #region Private Functions

        private void copyBinaryResources()
        {
            exportBinaryResource("boost_python-vc140-mt-1_64.dll", exportPath_);
            exportBinaryResource("boost_python3-vc140-mt-1_64.dll", exportPath_);
            exportBinaryResource("DevIL.dll", exportPath_);
            exportBinaryResource("FiremelonCore.dll", exportPath_);
            exportBinaryResource("Firemelon.exe", exportPath_);
            exportBinaryResource("Firemelon_Console.exe", exportPath_);
            exportBinaryResource("fmod.dll", exportPath_);
            exportBinaryResource("freeglut.dll", exportPath_);
            exportBinaryResource("glew32.dll", exportPath_);
            exportBinaryResource("ILU.dll", exportPath_);
            exportBinaryResource("ILUT.dll", exportPath_);
            exportBinaryResource("jpeg.dll", exportPath_);
            exportBinaryResource("libjpeg-9.dll", exportPath_);
            exportBinaryResource("libpng12-0.dll", exportPath_);
            exportBinaryResource("libpng16-16.dll", exportPath_);
            exportBinaryResource("libtiff-3.dll", exportPath_);
            exportBinaryResource("libtiff-5.dll", exportPath_);
            exportBinaryResource("libwebp-4.dll", exportPath_);
            exportBinaryResource("python36.dll", exportPath_);
            exportBinaryResource("python3.dll", exportPath_);
            exportBinaryResource("SDL2.dll", exportPath_);
            exportBinaryResource("SDL2_image.dll", exportPath_);
            exportBinaryResource("SDL2_net.dll", exportPath_);

            exportBinaryResource("__init__.py", scriptCoreFolder_);
            exportBinaryResource("entity.py", scriptCoreFolder_);
            exportBinaryResource("entityserializer.py", scriptCoreFolder_);
            exportBinaryResource("uiwidget.py", scriptCoreFolder_);
            exportBinaryResource("loadingscreen.py", scriptCoreFolder_);
            exportBinaryResource("transition.py", scriptCoreFolder_);
            exportBinaryResource("particle.py", scriptCoreFolder_);
            exportBinaryResource("particleemitter.py", scriptCoreFolder_);
            exportBinaryResource("room.py", scriptCoreFolder_);
            exportBinaryResource("query.py", scriptCoreFolder_);
            exportBinaryResource("ui.py", scriptCoreFolder_);
            exportBinaryResource("networkhandler.py", scriptCoreFolder_);

            exportBinaryResource("vertex.glsl", shaderFolder_);
            exportBinaryResource("fragment.glsl", shaderFolder_);
            exportBinaryResource("fragment_texture.glsl", shaderFolder_);
        }

        private void copyDataFiles()
        {
            // Copy the panels file to the project_ directory.
            DataFileDto jsonFile = project_.DataFiles[Globals.PanelsJsonFileId];

            if (File.Exists(jsonFile.FileFullPath) == true)
            {
                File.Copy(jsonFile.FileFullPath, exportPath_ + "\\panels.json", true);
            }
            else
            {
                using (File.Create(jsonFile.FileFullPath)) ;
            }

            // Lastly, copy any data files to their destination (TO DO!!!)
            //foreach (KeyValuePair<Guid, ScriptDto> kvp in project_.Scripts)
            //{
            //    ScriptDto script = kvp.Value;

            //    string scriptDestPath = scriptFolder + "_" + script.Name.ToLower() + ".py";

            //    IScriptGenerator scriptGenerator = scriptGeneratorFactory_.NewScriptGenerator(script);

            //    try
            //    {
            //        if (string.IsNullOrEmpty(script.ScriptPath) == true)
            //        {
            //            string scriptCode = scriptGenerator.Generate(script);

            //            File.WriteAllText(scriptDestPath, scriptCode);
            //        }
            //        else
            //        {
            //            if (File.Exists(script.ScriptPath) == true)
            //            {
            //                File.Copy(script.ScriptPath, scriptDestPath, true);
            //            }
            //            else
            //            {
            //                System.Windows.Forms.MessageBox.Show("Script file " + script.ScriptPath + " does not exist.");
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        System.Windows.Forms.MessageBox.Show(ex.ToString());
            //    }
            //}
        }

        private void copyScriptFiles()
        {
            // Copy the script files to the project_ directory.
            foreach (KeyValuePair<Guid, ScriptDto> kvp in project_.Scripts)
            {
                ScriptDto script = kvp.Value;

                string scriptDestPath = scriptFolder_ + "_" + script.Name.ToLower() + ".py";

                IScriptGenerator scriptGenerator = scriptGeneratorFactory_.NewScriptGenerator(script);

                try
                {
                    if (string.IsNullOrEmpty(script.ScriptPath) == true)
                    {
                        string scriptCode = scriptGenerator.Generate(script);

                        File.WriteAllText(scriptDestPath, scriptCode);
                    }
                    else
                    {
                        if (File.Exists(script.ScriptPath) == true)
                        {
                            File.Copy(script.ScriptPath, scriptDestPath, true);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Script file " + script.ScriptPath + " does not exist.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
            }
        }

        private void createAllDirectories()
        {
            // Delete the export folder, so no unused files end up left in it.
            if (Directory.Exists(exportPath_) == true)
            {
                Directory.Delete(exportPath_, true);
            }

            createDirectoryIfNotExist(exportPath_ + "\\Data\\");
            createDirectoryIfNotExist(exportPath_ + "\\Data\\");
            createDirectoryIfNotExist(exportPath_ + "\\Data\\Rooms\\");
            createDirectoryIfNotExist(exportPath_ + "\\Data\\Assets\\");
            createDirectoryIfNotExist(exportPath_ + "\\Source\\");
            createDirectoryIfNotExist(shaderFolder_);
            createDirectoryIfNotExist(scriptFolder_);
            createDirectoryIfNotExist(scriptCoreFolder_);
        }

        private void createDirectoryIfNotExist(string path)
        {
            bool directoryExists = System.IO.Directory.Exists(path);

            if (directoryExists == false)
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        private void formatGuidForBoost(byte[] guidBytes)
        {
            // C# formats GUID bytes differently than boost UUID does, due to endian-ness.
            // Boost stores a guid as an array of 16 bytes, where as C# stores it as a 32 bit integer,
            // followed by two 16 bit integers, followed by 8 bytes.
            // See: http://stackoverflow.com/questions/9195551/why-does-guid-tobytearray-order-the-bytes-the-way-it-does

            byte temp;

            // 1 through 3 stores a 32 bit integer.
            temp = guidBytes[0];
            guidBytes[0] = guidBytes[3];
            guidBytes[3] = temp;

            temp = guidBytes[1];
            guidBytes[1] = guidBytes[2];
            guidBytes[2] = temp;

            // 4 and 5 store a 16 bit integer.
            temp = guidBytes[4];
            guidBytes[4] = guidBytes[5];
            guidBytes[5] = temp;

            // 6 and 7 store a 16 bit integer.
            temp = guidBytes[6];
            guidBytes[6] = guidBytes[7];
            guidBytes[7] = temp;

            // The final 8 bytes are stored as single bytes, thus unaffected by endian-ness.
        }

        private void exportBinaryResource(string resourceName, string exportPath_)
        {
            byte[] resourceData;

            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

                Stream stream = assembly.GetManifestResourceStream("FiremelonEditor2.Resources." + resourceName);

                using (var streamReader = new MemoryStream())
                {
                    stream.CopyTo(streamReader);
                    resourceData = streamReader.ToArray();
                }

                File.WriteAllBytes(exportPath_ + "//" + resourceName, resourceData);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error getting resource - " + e.ToString(), "Error Getting Resource");
            }
        }

        private void showWarningsDialog()
        {
            ProjectUiStateDto uiState = projectController_.GetUiState();

            if (warningsForm_.WarningCount > 0 && uiState.ShowWarnings == true)
            {
                warningsForm_.ShowDialog();
            }
        }

        private void writeActorAssetFiles()
        {
            Byte[] stringBytes;

            // Write the actor data.      
            foreach (ActorDto actor in project_.Actors)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + actor.Id.ToString() + ".ae");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(actor.Name);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    // Script name will be the lowercase actor name with an underscore in front.
                    stringBytes = System.Text.Encoding.ASCII.GetBytes("_" + actor.Name.ToLower());
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    // Python ID name will be the uppercase actor name with an "ENTITY_" in front.                  
                    stringBytes = System.Text.Encoding.ASCII.GetBytes("ENTITY_" + actor.Name.ToUpper().Replace(" ", ""));
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = actor.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    bw.Write(actor.StageWidth);
                    bw.Write(actor.StageHeight);
                    bw.Write((uint)actor.StageOriginLocation);

                    bw.Write(actor.StageBackgroundDepth);

                    int pivotPointX = actor.PivotPoint.X;
                    int pivotPointY = actor.PivotPoint.Y;

                    // Convert the pivot point to native TLC space.
                    switch (actor.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:
                            // Do nothing, already in TLC space.
                            break;

                        case OriginLocation.TopMiddle:

                            pivotPointX += actor.StageWidth / 2;

                            break;

                        case OriginLocation.TopRight:

                            pivotPointX += actor.StageWidth;

                            break;

                        case OriginLocation.MiddleLeft:

                            pivotPointY += actor.StageHeight / 2;

                            break;

                        case OriginLocation.Center:

                            pivotPointX += actor.StageWidth / 2;

                            pivotPointY += actor.StageHeight / 2;

                            break;

                        case OriginLocation.MiddleRight:

                            pivotPointX += actor.StageWidth;

                            pivotPointY += actor.StageHeight / 2;

                            break;

                        case OriginLocation.BottomLeft:

                            pivotPointY += actor.StageHeight;

                            break;

                        case OriginLocation.BottomMiddle:

                            pivotPointX += actor.StageWidth / 2;

                            pivotPointY += actor.StageHeight;

                            break;

                        case OriginLocation.BottomRight:

                            pivotPointX += actor.StageWidth;

                            pivotPointY += actor.StageHeight;

                            break;
                    }

                    bw.Write(pivotPointX);
                    bw.Write(pivotPointY);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(actor.Tag);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidClassificationId = actor.Classification.ToByteArray();
                    formatGuidForBoost(guidClassificationId);
                    bw.Write(guidClassificationId);

                    bw.Write(actor.KeepRoomActive);

                    List<StateDto> states = project_.States[actor.Id];

                    int stateCount = states.Count;

                    bw.Write(stateCount);

                    foreach (StateDto state in states)
                    {
                        stringBytes = System.Text.Encoding.ASCII.GetBytes(state.Name);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);

                        if (state.Id == actor.InitialStateId)
                        {
                            bw.Write(true);
                        }
                        else
                        {
                            bw.Write(false);
                        }

                        List<HitboxDto> hitboxes = project_.Hitboxes[state.Id];
                        int hitboxCount = hitboxes.Count;
                        
                        bw.Write(hitboxCount);

                        foreach (HitboxDto hitbox in hitboxes)
                        {
                            int hitboxLeft = hitbox.HitboxRect.Left;
                            int hitboxTop = hitbox.HitboxRect.Top;

                            // Convert the hitbox positions to TLC mode, if they are not already using it.
                            switch (actor.StageOriginLocation)
                            {
                                case OriginLocation.Center:

                                    hitboxLeft = (actor.StageWidth / 2) - (hitbox.HitboxRect.Width / 2) + hitbox.HitboxRect.Left;

                                    hitboxTop = (actor.StageHeight / 2) - (hitbox.HitboxRect.Height / 2) + hitbox.HitboxRect.Top;

                                    break;

                                case OriginLocation.TopMiddle:

                                    hitboxLeft = (actor.StageWidth / 2) - (hitbox.HitboxRect.Width / 2) + hitbox.HitboxRect.Left;
                                    
                                    break;

                                case OriginLocation.BottomMiddle:

                                    hitboxLeft = (actor.StageWidth / 2) - (hitbox.HitboxRect.Width / 2) + hitbox.HitboxRect.Left;

                                    hitboxTop = actor.StageHeight + hitbox.HitboxRect.Top;

                                    break;
                            }

                            bw.Write(hitboxTop);
                            bw.Write(hitboxLeft);
                            bw.Write(hitbox.HitboxRect.Height);
                            bw.Write(hitbox.HitboxRect.Width);

                            byte[] guidHitboxIdentityId = hitbox.Identity.ToByteArray();
                            formatGuidForBoost(guidHitboxIdentityId);
                            bw.Write(guidHitboxIdentityId);

                            if (hitbox.Identity == Guid.Empty)
                            {
                                warningsForm_.AddWarningMessage("Hitbox " + hitbox.Name + " identity not set", "Actor " + actor.Name, "State " + state.Name);
                            }

                            bw.Write((uint)hitbox.Priority);
                            bw.Write(hitbox.IsSolid);
                            bw.Write(hitbox.RotationDegrees);
                        }

                        List<AnimationSlotDto> animationSlots = project_.AnimationSlots[state.Id];

                        int animationSlotCount = animationSlots.Count;

                        bw.Write(animationSlotCount);

                        foreach (AnimationSlotDto animationSlot in animationSlots)
                        {
                            stringBytes = System.Text.Encoding.ASCII.GetBytes(animationSlot.Name);
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);

                            byte[] guidAnimationId = animationSlot.Animation.ToByteArray();
                            formatGuidForBoost(guidAnimationId);
                            bw.Write(guidAnimationId);

                            bw.Write(animationSlot.Background);

                            int animationSlotPositionX = animationSlot.Position.X;
                            int animationSlotPositionY = animationSlot.Position.Y;
                            
                            AnimationDto animation = projectController_.GetAnimation(animationSlot.Animation);
                            
                            // Convert the positions to TLC mode, if they are not already using it.
                                    
                            // Changed it to no longer use the animation slot origin, as that now gets handled by the engine. This is 
                            // because the engine needs to know the slot's native position because the animation could change. So it's
                            // easier to just store the position without the translation for the slot origin, and then when the 
                            // animation assignment happens in the engine, do the translation there.

                            switch (actor.StageOriginLocation)
                            {
                                case OriginLocation.TopLeft:

                                    animationSlotPositionX = animationSlot.Position.X;

                                    animationSlotPositionY = animationSlot.Position.Y;

                                    break;

                                case OriginLocation.TopMiddle:

                                    animationSlotPositionX = (actor.StageWidth / 2) + animationSlot.Position.X;

                                    animationSlotPositionY = animationSlot.Position.Y;

                                    break;

                                case OriginLocation.TopRight:

                                    animationSlotPositionX = actor.StageWidth + animationSlot.Position.X;

                                    animationSlotPositionY = animationSlot.Position.Y;

                                    break;

                                case OriginLocation.MiddleLeft:

                                    animationSlotPositionX = animationSlot.Position.X;

                                    animationSlotPositionY = (actor.StageHeight / 2) + animationSlot.Position.Y;
                                            
                                    break;

                                case OriginLocation.Center:

                                    animationSlotPositionX = (actor.StageWidth / 2) + animationSlot.Position.X;

                                    animationSlotPositionY = (actor.StageHeight / 2) + animationSlot.Position.Y;
                                                                                           
                                    break;

                                case OriginLocation.MiddleRight:

                                    animationSlotPositionX = actor.StageWidth + animationSlot.Position.X;

                                    animationSlotPositionY = (actor.StageHeight / 2) + animationSlot.Position.Y;

                                    break;

                                case OriginLocation.BottomLeft:
                                            
                                    animationSlotPositionX = animationSlot.Position.X;

                                    animationSlotPositionY = actor.StageHeight + animationSlot.Position.Y;

                                    break;

                                case OriginLocation.BottomMiddle:

                                    animationSlotPositionX = (actor.StageWidth / 2) + animationSlot.Position.X;

                                    animationSlotPositionY = actor.StageHeight + animationSlot.Position.Y;

                                    break;

                                case OriginLocation.BottomRight:

                                    animationSlotPositionX = actor.StageWidth + animationSlot.Position.X;

                                    animationSlotPositionY = actor.StageHeight + animationSlot.Position.Y;

                                    break;
                            }

                            bw.Write(animationSlotPositionX);
                            bw.Write(animationSlotPositionY);

                            bw.Write(animationSlot.HueColor.Red);
                            bw.Write(animationSlot.HueColor.Green);
                            bw.Write(animationSlot.HueColor.Blue);
                            bw.Write(animationSlot.HueColor.Alpha);

                            bw.Write(animationSlot.BlendColor.Red);
                            bw.Write(animationSlot.BlendColor.Green);
                            bw.Write(animationSlot.BlendColor.Blue);
                            bw.Write(animationSlot.BlendColor.Alpha);

                            bw.Write(animationSlot.BlendPercent);

                            bw.Write(animationSlot.PivotPoint.X);
                            bw.Write(animationSlot.PivotPoint.Y);

                            bw.Write(animationSlot.AlphaGradientFrom);
                            bw.Write(animationSlot.AlphaGradientTo);
                            bw.Write(animationSlot.AlphaGradientRadius);
                            bw.Write(animationSlot.AlphaGradientRadialCenter.X);
                            bw.Write(animationSlot.AlphaGradientRadialCenter.Y);
                            bw.Write((uint)(animationSlot.AlphaGradientDirection));
                            bw.Write((uint)(animationSlot.OriginLocation));

                            StateDto nextState = projectController_.GetState(animationSlot.NextStateId);

                            if (nextState != null)
                            {
                                stringBytes = System.Text.Encoding.ASCII.GetBytes(nextState.Name);
                                bw.Write(stringBytes.Length);
                                bw.Write(stringBytes);
                            }
                            else
                            {
                                bw.Write(0);
                            }
                            
                            bw.Write(animationSlot.FramesPerSecond);
                            bw.Write((int)animationSlot.AnimationStyle);

                            bw.Write(animationSlot.OutlineColor.Red);
                            bw.Write(animationSlot.OutlineColor.Green);
                            bw.Write(animationSlot.OutlineColor.Blue);
                            bw.Write(animationSlot.OutlineColor.Alpha);
                        }
                    }

                    List<PropertyDto> properties = project_.Properties[actor.Id];

                    int propertyCount = properties.Count;

                    bw.Write(propertyCount);

                    foreach (PropertyDto property in properties)
                    {
                        stringBytes = System.Text.Encoding.ASCII.GetBytes(property.Name);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);

                        stringBytes = System.Text.Encoding.ASCII.GetBytes(property.DefaultValue);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);
                    }
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeAnimationMetadata(BinaryWriter bw)
        {
            Byte[] stringBytes;

            // Write the animation data.
            int animationCount = 0;

            foreach (AnimationGroupDto animationGroup in project_.AnimationGroups)
            {
                animationCount += project_.Animations[animationGroup.Id].Count;
            }

            bw.Write(animationCount);

            foreach (AnimationGroupDto animationGroup in project_.AnimationGroups)
            {
                foreach (AnimationDto animation in project_.Animations[animationGroup.Id])
                {
                    stringBytes = System.Text.Encoding.ASCII.GetBytes(animation.Name);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidAnimationId = animation.Id.ToByteArray();
                    formatGuidForBoost(guidAnimationId);
                    bw.Write(guidAnimationId);

                    byte[] guidSpriteSheetId = animation.SpriteSheet.ToByteArray();
                    formatGuidForBoost(guidSpriteSheetId);
                    bw.Write(guidSpriteSheetId);

                    byte[] guidAlphaMaskSheetId = animation.AlphaMaskSheet.ToByteArray();
                    formatGuidForBoost(guidAlphaMaskSheetId);
                    bw.Write(guidAlphaMaskSheetId);

                    List<FrameDto> frames = project_.Frames[animation.Id];

                    int frameCount = frames.Count;

                    bw.Write(frameCount);

                    for (int i = 0; i < frameCount; i++)
                    {
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

                        List<HitboxDto> hitboxes = project_.Hitboxes[frames[i].Id];
                        int hitboxCount = hitboxes.Count;

                        bw.Write(hitboxCount);

                        foreach (HitboxDto hitbox in hitboxes)
                        {
                            bw.Write(hitbox.HitboxRect.Top);
                            bw.Write(hitbox.HitboxRect.Left);
                            bw.Write(hitbox.HitboxRect.Height);
                            bw.Write(hitbox.HitboxRect.Width);

                            byte[] guidHitboxIdentityId = hitbox.Identity.ToByteArray();
                            formatGuidForBoost(guidHitboxIdentityId);
                            bw.Write(guidHitboxIdentityId);

                            if (hitbox.Identity == Guid.Empty)
                            {
                                warningsForm_.AddWarningMessage("Hitbox " + hitbox.Name + " identity not set", "Animation " + animation.Name, "Frame " + i);
                            }

                            bw.Write((uint)hitbox.Priority);
                            bw.Write(hitbox.IsSolid);
                            bw.Write(hitbox.RotationDegrees);
                        }

                        List<ActionPointDto> actionPoints = project_.ActionPoints[frames[i].Id];
                        int actionPointCount = actionPoints.Count;

                        bw.Write(actionPointCount);

                        foreach (ActionPointDto actionPoint in actionPoints)
                        {
                            stringBytes = System.Text.Encoding.ASCII.GetBytes(actionPoint.Name);
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);

                            bw.Write(actionPoint.Position.X);
                            bw.Write(actionPoint.Position.Y);
                        }

                        List<FrameTriggerDto> frameTriggers = project_.FrameTriggers[frames[i].Id];
                        int frameTriggerCount = frameTriggers.Count;

                        bw.Write(frameTriggerCount);

                        foreach (FrameTriggerDto frameTrigger in frameTriggers)
                        {

                            byte[] guidSignalId = frameTrigger.Signal.ToByteArray();
                            formatGuidForBoost(guidSignalId);
                            bw.Write(guidSignalId);
                        }
                    }
                }
            }
        }

        private void writeAudioAssetFiles()
        {
            Byte[] stringBytes;

            // Write the audio data.      
            foreach (AudioAssetDto audioAsset in project_.AudioAssets)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + audioAsset.Id.ToString() + ".aa");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(audioAsset.Name);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = audioAsset.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    byte[] guidAudioResourceId = audioAsset.AudioResourceId.ToByteArray();
                    formatGuidForBoost(guidAudioResourceId);
                    bw.Write(guidAudioResourceId);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(audioAsset.Channel);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    //bw.Write(audioAsset.Loop);
                    //bw.Write(audioAsset.Volume);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeAudioResourceFiles()
        {
            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();

            foreach (AudioAssetDto audioAsset in project_.AudioAssets)
            {
                Guid audioId = audioAsset.AudioResourceId;

                // Separate resources dto removed in 2.2 format.
                //AudioResourceDto audio = resources.AudioData[audioId];
                AudioResourceDto audio = project_.AudioData[audioId];

                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + audio.Id.ToString() + ".ar");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    byte[] guidId = audio.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    projectController_.LoadAudio(audio.Id);

                    // Audio size.
                    bw.Write(audio.AudioData.Length);

                    // Audio bytes.
                    bw.Write(audio.AudioData);

                    projectController_.UnloadAudioResource(audio.Id);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeAudioAssetMetadata(BinaryWriter bw)
        {
            Byte[] stringBytes;

            // Write the audio resource data.
            int audioCount = project_.AudioAssets.Count;

            bw.Write(audioCount);

            foreach (AudioAssetDto audio in project_.AudioAssets)
            {
                string audioScriptName = "AUDIO_" + audio.Name.ToUpper();
                audioScriptName = audioScriptName.Replace(" ", "");
                audioScriptName = audioScriptName.Replace("/", "");

                stringBytes = System.Text.Encoding.ASCII.GetBytes(audioScriptName);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                stringBytes = System.Text.Encoding.ASCII.GetBytes(audio.Name);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                byte[] guidAudioId = audio.Id.ToByteArray();
                formatGuidForBoost(guidAudioId);
                bw.Write(guidAudioId);
            }
        }

        private void writeBitmapResourceFiles()
        {
            // Separate resources dto removed in 2.2 format.
            //ProjectResourcesDto resources = projectController_.GetResources();

            Bitmap bmpTemp;
            Byte[] bytes = new Byte[0];
            MemoryStream stream = new MemoryStream();

            foreach (TileSheetDto tileSheet in project_.TileSheets)
            {
                Guid bitmapId = tileSheet.BitmapResourceId;
                BitmapResourceDto bitmap = project_.Bitmaps[bitmapId];

                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + bitmap.Id.ToString() + ".br");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    byte[] guidId = bitmap.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    // If the bitmap is not loaded, Load the bitmap from disk, then dispose of it.                   
                    projectController_.LoadBitmap(bitmap.Id);

                    // Create a second bitmap that has a 1 pixel border with full transparency, and copy the asset bitmap
                    // into the center so that there is an alpha outline. This is to save GPU effort later, because I won't
                    // have to test for edge cases in the sprite outline shader.
                                        
                    if (bitmap.BitmapImage != null)
                    {
                        bmpTemp = new Bitmap(bitmap.BitmapImage.Size.Width + 2, bitmap.BitmapImage.Size.Height + 2);

                        Graphics gBitmapWithAlphaBorder = Graphics.FromImage(bmpTemp);

                        gBitmapWithAlphaBorder.FillRectangle(new SolidBrush(Color.Transparent), new System.Drawing.Rectangle(0, 0, bmpTemp.Size.Width, bmpTemp.Size.Height));
                        
                        // The original bitmap.
                        RectangleF sourceRect = new RectangleF(0, 0, bitmap.BitmapImage.Size.Width, bitmap.BitmapImage.Size.Height);

                        // The target location. Shift by 2 to get the border.
                        RectangleF destRect = new RectangleF(1, 1, bitmap.BitmapImage.Size.Width, bitmap.BitmapImage.Size.Height);

                        gBitmapWithAlphaBorder.DrawImage(bitmap.BitmapImage, destRect, sourceRect, GraphicsUnit.Pixel);

                        // Clear the memory stream and save the new bitmap to the stream.
                        byte[] buffer = stream.GetBuffer();
                        Array.Clear(buffer, 0, buffer.Length);
                        stream.Position = 0;
                        stream.SetLength(0);

                        bmpTemp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    }

                    bytes = stream.ToArray();

                    // Image size.
                    bw.Write(bytes.Length);

                    // Image bytes.
                    bw.Write(bytes);

                    projectController_.UnloadBitmapResource(bitmap.Id, 0);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }

            foreach (SpriteSheetDto spriteSheet in project_.SpriteSheets)
            {
                Guid bitmapId = spriteSheet.BitmapResourceId;
                BitmapResourceDto bitmap = project_.Bitmaps[bitmapId];

                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + bitmap.Id.ToString() + ".br");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    byte[] guidId = bitmap.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    // If the bitmap is not loaded, Load the bitmap from disk, then dispose of it.                   
                    projectController_.LoadBitmap(bitmap.Id);

                    // Create a second bitmap that has a 1 pixel border with full transparency, and copy the asset bitmap
                    // into the center so that there is an alpha outline. This is to save GPU effort later, because I won't
                    // have to test for edge cases in the sprite outline shader.

                    if (bitmap.BitmapImage != null)
                    {
                        bmpTemp = new Bitmap(bitmap.BitmapImage.Size.Width + 2, bitmap.BitmapImage.Size.Height + 2);

                        Graphics gBitmapWithAlphaBorder = Graphics.FromImage(bmpTemp);

                        gBitmapWithAlphaBorder.FillRectangle(new SolidBrush(Color.Transparent), new System.Drawing.Rectangle(0, 0, bmpTemp.Size.Width, bmpTemp.Size.Height));

                        // The original bitmap.
                        RectangleF sourceRect = new RectangleF(0, 0, bitmap.BitmapImage.Size.Width, bitmap.BitmapImage.Size.Height);

                        // The target location. Shift by 2 to get the border.
                        RectangleF destRect = new RectangleF(1, 1, bitmap.BitmapImage.Size.Width, bitmap.BitmapImage.Size.Height);

                        gBitmapWithAlphaBorder.DrawImage(bitmap.BitmapImage, destRect, sourceRect, GraphicsUnit.Pixel);

                        // Clear the memory stream.
                        byte[] buffer = stream.GetBuffer();
                        Array.Clear(buffer, 0, buffer.Length);
                        stream.Position = 0;
                        stream.SetLength(0);

                        bmpTemp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    }

                    bytes = stream.ToArray();

                    // Image size.
                    bw.Write(bytes.Length);

                    // Image bytes.
                    bw.Write(bytes);

                    projectController_.UnloadBitmapResource(bitmap.Id, 0);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeEventAssetFiles()
        {
            Byte[] stringBytes;

            // Write the event data.
            List<EventDto> events = project_.Events;

            foreach (EventDto eventEntity in events)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + eventEntity.Id.ToString() + ".ee");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(eventEntity.Name);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    // Script name will be the lowercase event name with an underscore in front.
                    stringBytes = System.Text.Encoding.ASCII.GetBytes("_" + eventEntity.Name.ToLower());
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    // Python ID name will be the uppercase event name with an "ENTITY_" in front.                  
                    stringBytes = System.Text.Encoding.ASCII.GetBytes("ENTITY_" + eventEntity.Name.ToUpper().Replace(" ", ""));
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = eventEntity.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(eventEntity.Tag);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidClassificationId = eventEntity.Classification.ToByteArray();
                    formatGuidForBoost(guidClassificationId);
                    bw.Write(guidClassificationId);

                    List<PropertyDto> properties = project_.Properties[eventEntity.Id];

                    int propertyCount = properties.Count;

                    bw.Write(propertyCount);

                    foreach (PropertyDto property in properties)
                    {
                        stringBytes = System.Text.Encoding.ASCII.GetBytes(property.Name);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);

                        stringBytes = System.Text.Encoding.ASCII.GetBytes(property.DefaultValue);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);
                    }
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeGameButtonMetadata(BinaryWriter bw)
        {
            Byte[] stringBytes;

            // Write the game button data.    
            int gameButtonCount = project_.GameButtons.Count;

            bw.Write(gameButtonCount);

            foreach (GameButtonDto gameButton in project_.GameButtons)
            {
                string gameButtonScriptName = "GAMEBUTTON_" + gameButton.Name.ToUpper();
                gameButtonScriptName = gameButtonScriptName.Replace(" ", "");
                gameButtonScriptName = gameButtonScriptName.Replace("/", "");

                stringBytes = System.Text.Encoding.ASCII.GetBytes(gameButtonScriptName);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                stringBytes = System.Text.Encoding.ASCII.GetBytes(gameButton.Name);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                stringBytes = System.Text.Encoding.ASCII.GetBytes(gameButton.Label);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                byte[] guidId = gameButton.Id.ToByteArray();
                formatGuidForBoost(guidId);
                bw.Write(guidId);

                byte[] guidGroupId = gameButton.Group.ToByteArray();
                formatGuidForBoost(guidGroupId);
                bw.Write(guidGroupId);
            }
        }

        private void writeGameButtonGroupMetadata(BinaryWriter bw)
        {
            Byte[] stringBytes;

            // Write the game button group data.    
            int gameButtonGroupCount = project_.GameButtonGroups.Count;

            bw.Write(gameButtonGroupCount);

            foreach (GameButtonGroupDto gameButtonGroup in project_.GameButtonGroups)
            {
                string gameButtonGroupScriptName = "GAMEBUTTONGROUP_" + gameButtonGroup.Name.ToUpper();
                gameButtonGroupScriptName = gameButtonGroupScriptName.Replace(" ", "");
                gameButtonGroupScriptName = gameButtonGroupScriptName.Replace("/", "");

                stringBytes = System.Text.Encoding.ASCII.GetBytes(gameButtonGroupScriptName);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                stringBytes = System.Text.Encoding.ASCII.GetBytes(gameButtonGroup.Name);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                byte[] guidId = gameButtonGroup.Id.ToByteArray();
                formatGuidForBoost(guidId);
                bw.Write(guidId);
            }
        }

        private void writeHitboxIdentityMetadata(BinaryWriter bw)
        {
            Byte[] stringBytes;

            // Write the hitbox identity data.
            int hitboxIdentityCount = project_.HitboxIdentities.Count;

            bw.Write(hitboxIdentityCount);

            foreach (HitboxIdentityDto hitboxIdentity in project_.HitboxIdentities)
            {
                string hitboxIdentityName = hitboxIdentity.Name.ToUpper();
                hitboxIdentityName = hitboxIdentityName.Replace(" ", "");

                stringBytes = System.Text.Encoding.ASCII.GetBytes("HITBOX_" + hitboxIdentityName);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                byte[] hitboxGuidId = hitboxIdentity.Id.ToByteArray();
                formatGuidForBoost(hitboxGuidId);
                bw.Write(hitboxGuidId);
            }
        }

        private void writeHudElementAssetFiles()
        {
            Byte[] stringBytes;

            // Write the HUD element data.
            List<HudElementDto> hudElements = project_.HudElements;

            foreach (HudElementDto hudElement in hudElements)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + hudElement.Id.ToString() + ".he");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(hudElement.Name);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    // Script name will be the lowercase HUD element name with an underscore in front.
                    stringBytes = System.Text.Encoding.ASCII.GetBytes("_" + hudElement.Name.ToLower());
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    // Python ID name will be the uppercase hud element name with an "ENTITY_" in front.                  
                    stringBytes = System.Text.Encoding.ASCII.GetBytes("ENTITY_" + hudElement.Name.ToUpper().Replace(" ", ""));
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = hudElement.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    bw.Write(hudElement.StageWidth);
                    bw.Write(hudElement.StageHeight);
                    bw.Write((uint)hudElement.StageOriginLocation);

                    bw.Write(hudElement.StageBackgroundDepth);

                    int pivotPointX = hudElement.PivotPoint.X;
                    int pivotPointY = hudElement.PivotPoint.Y;

                    // Convert the pivot point to native TLC space.
                    switch (hudElement.StageOriginLocation)
                    {
                        case OriginLocation.TopLeft:
                            // Do nothing, already in TLC space.
                            break;

                        case OriginLocation.TopMiddle:

                            pivotPointX += hudElement.StageWidth / 2;

                            break;

                        case OriginLocation.TopRight:

                            pivotPointX += hudElement.StageWidth;

                            break;

                        case OriginLocation.MiddleLeft:

                            pivotPointY += hudElement.StageHeight / 2;

                            break;

                        case OriginLocation.Center:

                            pivotPointX += hudElement.StageWidth / 2;

                            pivotPointY += hudElement.StageHeight / 2;

                            break;

                        case OriginLocation.MiddleRight:

                            pivotPointX += hudElement.StageWidth;

                            pivotPointY += hudElement.StageHeight / 2;

                            break;

                        case OriginLocation.BottomLeft:

                            pivotPointY += hudElement.StageHeight;

                            break;

                        case OriginLocation.BottomMiddle:

                            pivotPointX += hudElement.StageWidth / 2;

                            pivotPointY += hudElement.StageHeight;

                            break;

                        case OriginLocation.BottomRight:

                            pivotPointX += hudElement.StageWidth;

                            pivotPointY += hudElement.StageHeight;

                            break;
                    }

                    bw.Write(pivotPointX);
                    bw.Write(pivotPointY);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(hudElement.Tag);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidClassificationId = hudElement.Classification.ToByteArray();
                    formatGuidForBoost(guidClassificationId);
                    bw.Write(guidClassificationId);

                    List<StateDto> states = project_.States[hudElement.Id];

                    int stateCount = states.Count;

                    bw.Write(stateCount);

                    foreach (StateDto state in states)
                    {
                        stringBytes = System.Text.Encoding.ASCII.GetBytes(state.Name);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);

                        if (state.Id == hudElement.InitialStateId)
                        {
                            bw.Write(true);
                        }
                        else
                        {
                            bw.Write(false);
                        }

                        List<AnimationSlotDto> animationSlots = project_.AnimationSlots[state.Id];

                        int animationSlotCount = animationSlots.Count;

                        bw.Write(animationSlotCount);

                        foreach (AnimationSlotDto animationSlot in animationSlots)
                        {
                            stringBytes = System.Text.Encoding.ASCII.GetBytes(animationSlot.Name);
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);

                            byte[] guidAnimationId = animationSlot.Animation.ToByteArray();
                            formatGuidForBoost(guidAnimationId);
                            bw.Write(guidAnimationId);
                            
                            bw.Write(animationSlot.Background);
                            
                            int animationSlotPositionX = 0;
                            int animationSlotPositionY = 0;

                            AnimationDto animation = projectController_.GetAnimation(animationSlot.Animation);

                            if (animation != null)
                            {
                                int spriteSheetIndex = projectController_.GetSpriteSheetIndexFromId(animation.SpriteSheet);

                                if (spriteSheetIndex >= 0 && spriteSheetIndex < project_.SpriteSheets.Count)
                                {
                                    SpriteSheetDto spriteSheet = project_.SpriteSheets[spriteSheetIndex];

                                    // Convert the animation slot positions to TLC mode, if they are not already using it.
                                    switch (hudElement.StageOriginLocation)
                                    {
                                        case OriginLocation.Center:

                                            switch (animationSlot.OriginLocation)
                                            {
                                                case OriginLocation.TopLeft:

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.Center:

                                                    // Changed it so the engine moves the slot position to TLC if the animation is centered.

                                                    //animationSlotPositionX = (hudElement.StageWidth / 2) - (int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2) + animationSlot.Position.X;

                                                    //animationSlotPositionY = (hudElement.StageHeight / 2) - (int)((spriteSheet.CellHeight * spriteSheet.ScaleFactor) / 2) + animationSlot.Position.Y;

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.TopMiddle:
                                                    
                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.BottomMiddle:

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                    break;
                                            }

                                            break;

                                        case OriginLocation.TopLeft:

                                            switch (animationSlot.OriginLocation)
                                            {
                                                case OriginLocation.TopLeft:

                                                    animationSlotPositionX = animationSlot.Position.X;

                                                    animationSlotPositionY = animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.Center:

                                                    // Changed it so the engine moves the slot position to TLC if the animation is centered.

                                                    //animationSlotPositionX = 0 - (int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2) + animationSlot.Position.X;

                                                    //animationSlotPositionY = 0 - (int)((spriteSheet.CellHeight * spriteSheet.ScaleFactor) / 2) + animationSlot.Position.Y;

                                                    animationSlotPositionX = animationSlot.Position.X;

                                                    animationSlotPositionY = animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.TopMiddle:
                                                    
                                                    animationSlotPositionX = animationSlot.Position.X;

                                                    animationSlotPositionY = animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.BottomMiddle:

                                                    animationSlotPositionX = animationSlot.Position.X;

                                                    animationSlotPositionY = animationSlot.Position.Y;

                                                    break;
                                            }

                                            break;

                                        case OriginLocation.TopMiddle:
                                            
                                            switch (animationSlot.OriginLocation)
                                            {
                                                case OriginLocation.TopLeft:

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.Center:

                                                    // Changed it so the engine moves the slot position to TLC if the animation is centered.

                                                    //animationSlotPositionX = 0 - (int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2) + animationSlot.Position.X;

                                                    //animationSlotPositionY = 0 - (int)((spriteSheet.CellHeight * spriteSheet.ScaleFactor) / 2) + animationSlot.Position.Y;

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.TopMiddle:

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.BottomMiddle:

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = animationSlot.Position.Y;

                                                    break;
                                            }

                                            break;

                                        case OriginLocation.BottomMiddle:

                                            switch (animationSlot.OriginLocation)
                                            {
                                                case OriginLocation.TopLeft:

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = hudElement.StageHeight + animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.Center:

                                                    // Changed it so the engine moves the slot position to TLC if the animation is centered.

                                                    //animationSlotPositionX = 0 - (int)((spriteSheet.CellWidth * spriteSheet.ScaleFactor) / 2) + animationSlot.Position.X;

                                                    //animationSlotPositionY = 0 - (int)((spriteSheet.CellHeight * spriteSheet.ScaleFactor) / 2) + animationSlot.Position.Y;

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = hudElement.StageHeight + animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.TopMiddle:

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = hudElement.StageHeight + animationSlot.Position.Y;

                                                    break;

                                                case OriginLocation.BottomMiddle:

                                                    animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                    animationSlotPositionY = hudElement.StageHeight + animationSlot.Position.Y;

                                                    break;
                                            }

                                            break;
                                    }
                                }
                            }
                            else
                            {
                                // Convert the animation slot positions to TLC mode, if they are not already using it.
                                switch (hudElement.StageOriginLocation)
                                {
                                    case OriginLocation.Center:

                                        switch (animationSlot.OriginLocation)
                                        {
                                            case OriginLocation.TopLeft:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.Center:

                                                // Changed it so the engine moves the slot position to TLC if the animation is centered.

                                                //// No animation is assigned, so the height and width should be considered to be zero. I know I
                                                //// don't actually need to include the 0 in the code, but I find it to be easier to read when
                                                //// trying to think through the logic.
                                                //animationSlotPositionX = (hudElement.StageWidth / 2) - 0 + animationSlot.Position.X;

                                                //animationSlotPositionY = (hudElement.StageHeight / 2) - 0 + animationSlot.Position.Y;

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.TopMiddle:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.BottomMiddle:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                break;
                                        }

                                        break;

                                    case OriginLocation.TopMiddle:
                                        
                                        switch (animationSlot.OriginLocation)
                                        {
                                            case OriginLocation.TopLeft:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                //animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.Center:

                                                // Changed it so the engine moves the slot position to TLC if the animation is centered.

                                                //// No animation is assigned, so the height and width should be considered to be zero. I know I
                                                //// don't actually need to include the 0 in the code, but I find it to be easier to read when
                                                //// trying to think through the logic.
                                                //animationSlotPositionX = (hudElement.StageWidth / 2) - 0 + animationSlot.Position.X;

                                                //animationSlotPositionY = (hudElement.StageHeight / 2) - 0 + animationSlot.Position.Y;

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                //animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.TopMiddle:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                //animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.BottomMiddle:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                //animationSlotPositionY = (hudElement.StageHeight / 2) + animationSlot.Position.Y;

                                                break;
                                        }

                                        break;

                                    case OriginLocation.BottomMiddle:

                                        switch (animationSlot.OriginLocation)
                                        {
                                            case OriginLocation.TopLeft:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                animationSlotPositionY = hudElement.StageHeight + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.Center:

                                                // Changed it so the engine moves the slot position to TLC if the animation is centered.

                                                //// No animation is assigned, so the height and width should be considered to be zero. I know I
                                                //// don't actually need to include the 0 in the code, but I find it to be easier to read when
                                                //// trying to think through the logic.
                                                //animationSlotPositionX = (hudElement.StageWidth / 2) - 0 + animationSlot.Position.X;

                                                //animationSlotPositionY = (hudElement.StageHeight / 2) - 0 + animationSlot.Position.Y;

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                animationSlotPositionY = hudElement.StageHeight + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.TopMiddle:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                animationSlotPositionY = hudElement.StageHeight + animationSlot.Position.Y;

                                                break;

                                            case OriginLocation.BottomMiddle:

                                                animationSlotPositionX = (hudElement.StageWidth / 2) + animationSlot.Position.X;

                                                animationSlotPositionY = hudElement.StageHeight + animationSlot.Position.Y;

                                                break;
                                        }

                                        break;
                                }                                
                            }

                            bw.Write(animationSlotPositionX);
                            bw.Write(animationSlotPositionY);

                            bw.Write(animationSlot.HueColor.Red);
                            bw.Write(animationSlot.HueColor.Green);
                            bw.Write(animationSlot.HueColor.Blue);
                            bw.Write(animationSlot.HueColor.Alpha);

                            bw.Write(animationSlot.BlendColor.Red);
                            bw.Write(animationSlot.BlendColor.Green);
                            bw.Write(animationSlot.BlendColor.Blue);
                            bw.Write(animationSlot.BlendColor.Alpha);

                            bw.Write(animationSlot.BlendPercent);

                            bw.Write(animationSlot.PivotPoint.X);
                            bw.Write(animationSlot.PivotPoint.Y);

                            bw.Write(animationSlot.AlphaGradientFrom);
                            bw.Write(animationSlot.AlphaGradientTo);
                            bw.Write(animationSlot.AlphaGradientRadius);
                            bw.Write(animationSlot.AlphaGradientRadialCenter.X);
                            bw.Write(animationSlot.AlphaGradientRadialCenter.Y);
                            bw.Write((uint)(animationSlot.AlphaGradientDirection));
                            bw.Write((uint)(animationSlot.OriginLocation));

                            StateDto nextState = projectController_.GetState(animationSlot.NextStateId);

                            if (nextState != null)
                            {
                                stringBytes = System.Text.Encoding.ASCII.GetBytes(nextState.Name);
                                bw.Write(stringBytes.Length);
                                bw.Write(stringBytes);
                            }
                            else
                            {
                                bw.Write(0);
                            }

                            bw.Write(animationSlot.FramesPerSecond);
                            bw.Write((int)animationSlot.AnimationStyle);

                            bw.Write(animationSlot.OutlineColor.Red);
                            bw.Write(animationSlot.OutlineColor.Green);
                            bw.Write(animationSlot.OutlineColor.Blue);
                            bw.Write(animationSlot.OutlineColor.Alpha);
                        }
                    }

                    List<PropertyDto> properties = project_.Properties[hudElement.Id];

                    int propertyCount = properties.Count;

                    bw.Write(propertyCount);

                    foreach (PropertyDto property in properties)
                    {
                        stringBytes = System.Text.Encoding.ASCII.GetBytes(property.Name);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);

                        stringBytes = System.Text.Encoding.ASCII.GetBytes(property.DefaultValue);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);
                    }
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeHeader()
        {
            FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\engine.init");
            BinaryWriter bw = new BinaryWriter(fs);

            try
            {
                bw.Write(exportVersion_.Major);
                bw.Write(exportVersion_.Minor);
                bw.Write(exportVersion_.Revision);
                bw.Write(project_.CameraHeight);
                bw.Write(project_.CameraWidth);
                bw.Write(project_.TileSize);
            }
            catch (IOException ex)
            {
                throw ex;
            }
            finally
            {
                bw.Close();
                fs.Close();
            }
        }

        private void writeLoadingScreenAssetFiles()
        {
            Byte[] stringBytes;

            // Write the loading screen data.
            List<LoadingScreenDto> loadingScreens = project_.LoadingScreens;

            foreach (LoadingScreenDto loadingScreen in loadingScreens)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + loadingScreen.Id.ToString() + ".ls");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    string loadingScreenName = loadingScreen.Name.ToUpper();
                    loadingScreenName = loadingScreenName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes("LOADINGSCREEN_" + loadingScreenName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string loadingScreenFileName = "_" + loadingScreen.Name.ToLower();
                    loadingScreenFileName = loadingScreenFileName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(loadingScreenFileName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string loadingScreenClassName = loadingScreen.Name;
                    loadingScreenClassName = loadingScreenClassName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(loadingScreenClassName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = loadingScreen.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeMetadataFile()
        {
            Byte[] stringBytes;

            FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\meta.data");
            BinaryWriter bw = new BinaryWriter(fs);

            try
            {
                bw.Write(exportVersion_.Major);
                bw.Write(exportVersion_.Minor);
                bw.Write(exportVersion_.Revision);

                byte[] guidInitialRoomId = project_.InitialRoomId.ToByteArray();
                formatGuidForBoost(guidInitialRoomId);
                bw.Write(guidInitialRoomId);

                ScriptDto uiManagerScript = project_.Scripts[Globals.UiManagerId];

                stringBytes = System.Text.Encoding.ASCII.GetBytes("_" + uiManagerScript.Name.ToLower());
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                string uiManagerClassName = project_.ProjectName + "Ui";
                uiManagerClassName = uiManagerClassName.Replace(" ", "");

                stringBytes = System.Text.Encoding.ASCII.GetBytes(uiManagerClassName);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                ScriptDto networkHandlerScript = project_.Scripts[Globals.NetworkHandlerId];

                stringBytes = System.Text.Encoding.ASCII.GetBytes("_" + networkHandlerScript.Name.ToLower());
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                string networkHandlerClassName = project_.ProjectName + "NetworkHandler";
                networkHandlerClassName = networkHandlerClassName.Replace(" ", "");

                stringBytes = System.Text.Encoding.ASCII.GetBytes(networkHandlerClassName);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                writeHitboxIdentityMetadata(bw);

                writeTriggerSignalMetadata(bw);

                writeAnimationMetadata(bw);

                writeAudioAssetMetadata(bw);

                writeGameButtonMetadata(bw);

                writeGameButtonGroupMetadata(bw);

                writeSpawnPointMetadata(bw);                
            }
            catch (IOException ex)
            {
                throw ex;
            }
            finally
            {
                bw.Close();
                fs.Close();
            }
        }

        private void writeParticleAssetFiles()
        {
            Byte[] stringBytes;

            // Write the particle data.
            List<ParticleDto> particles = project_.Particles;

            foreach (ParticleDto particle in particles)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + particle.Id.ToString() + ".pt");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    string particleName = particle.Name.ToUpper();
                    particleName = particleName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes("PARTICLE_" + particleName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string particleFileName = "_" + particle.Name.ToLower();
                    particleFileName = particleFileName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(particleFileName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string particleClassName = particle.Name;
                    particleClassName = particleClassName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(particleClassName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = particle.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeParticleEmitterAssetFiles()
        {
            Byte[] stringBytes;

            // Write the particle emitter data.
            List<ParticleEmitterDto> particleEmitters = project_.ParticleEmitters;

            foreach (ParticleEmitterDto particleEmitter in particleEmitters)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + particleEmitter.Id.ToString() + ".pe");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    string particleEmitterName = particleEmitter.Name.ToUpper();
                    particleEmitterName = particleEmitterName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes("PARTICLEEMITTER_" + particleEmitterName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string particleEmitterFileName = "_" + particleEmitter.Name.ToLower();
                    particleEmitterFileName = particleEmitterFileName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(particleEmitterFileName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string particleEmitterClassName = particleEmitter.Name;
                    particleEmitterClassName = particleEmitterClassName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(particleEmitterClassName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = particleEmitter.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }
        
        private void writeRoomData()
        {
            Byte[] stringBytes;
            
            // Add Map Widgets Here

            // Build a list of map widgets in each layer.
            var mapWidgetInstanceLists = new Dictionary<MapWidgetType, Dictionary<Guid, List<MapWidgetDto>>>();

            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                mapWidgetInstanceLists.Add(mapWidgetType, new Dictionary<Guid, List<MapWidgetDto>>());
            }

            // Create the map widget instance lists for each map widget type for each layer in each room.
            foreach (RoomDto room in project_.Rooms)
            {
                // Create a new list for the room, for HUD elements.
                mapWidgetInstanceLists[MapWidgetType.HudElement].Add(room.Id, new List<MapWidgetDto>());

                foreach (LayerDto layer in project_.Layers[room.Id])
                {
                    foreach (KeyValuePair<MapWidgetType, Dictionary<Guid, List<MapWidgetDto>>> kvp in mapWidgetInstanceLists)
                    {
                        // Create a new list for this layer. HUD Elements are in the room itself, not layers.
                        if (kvp.Key != MapWidgetType.HudElement)
                        {
                            Dictionary<Guid, List<MapWidgetDto>> dictMapWidgetsForLayer = kvp.Value;

                            dictMapWidgetsForLayer.Add(layer.Id, new List<MapWidgetDto>());
                        }
                    }
                }
            }
            
            // Add Map Widgets Here
            foreach (MapWidgetType mapWidgetType in Enum.GetValues(typeof(MapWidgetType)))
            {
                if (project_.MapWidgets.ContainsKey(mapWidgetType) == true)
                {
                    foreach (KeyValuePair<Guid, MapWidgetDto> kvp in project_.MapWidgets[mapWidgetType])
                    {
                        MapWidgetDto widget = kvp.Value;
                        
                        mapWidgetInstanceLists[mapWidgetType][widget.OwnerId].Add(widget);
                    }
                }
            }

            for (int i = 0; i < project_.Rooms.Count; i++)
            {
                RoomDto room = project_.Rooms[i];

                string folder = exportPath_ + "\\Data\\Rooms\\";
                string filename = room.Id.ToString() + ".rm";

                FileStream fs = System.IO.File.Create(folder + filename);
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    string roomName = room.Name;
                    roomName = roomName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(roomName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    // Script name will be the lowercase room name with an underscore in front.
                    stringBytes = System.Text.Encoding.ASCII.GetBytes("_" + roomName.ToLower());
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);


                    // Write the python name ID and room GUID.
                    stringBytes = System.Text.Encoding.ASCII.GetBytes("ROOM_" + roomName.ToUpper());
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = room.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    // Write the header data.
                    Guid loadingScreenId = room.LoadingScreenId;
                    byte[] guidLoadingScreenId = loadingScreenId.ToByteArray();
                    formatGuidForBoost(guidLoadingScreenId);
                    bw.Write(guidLoadingScreenId);

                    if (loadingScreenId == Guid.Empty)
                    {
                        warningsForm_.AddWarningMessage("Loading screen not set.", "Room " + roomName, string.Empty);
                    }

                    Guid transitionId = room.TransitionId;
                    byte[] guidTransitionId = transitionId.ToByteArray();
                    formatGuidForBoost(guidTransitionId);
                    bw.Write(guidTransitionId);

                    if (transitionId == Guid.Empty)
                    {
                        warningsForm_.AddWarningMessage("Transition not set.", "Room " + roomName, string.Empty);
                    }

                    // Get the room's height and width based on the interactive layer.
                    int interactiveLayerIndex = project_.InteractiveLayerIndexes[room.Id];

                    LayerDto interactiveLayer = projectController_.GetLayerByIndex(i, interactiveLayerIndex);

                    int mapWidth = interactiveLayer.Cols * project_.TileSize;
                    bw.Write(mapWidth);

                    int mapHeight = interactiveLayer.Rows * project_.TileSize;
                    bw.Write(mapHeight);
                    
                    int hudElementInstanceCount = mapWidgetInstanceLists[MapWidgetType.HudElement][room.Id].Count;

                    // Add Map Widgets Here
                    int totalActorInstanceCount = 0;
                    int totalEventInstanceCount = 0;
                    int totalSpawnPointCount = 0;
                    int totalParticleEmitterCount = 0;
                    int totalParticleCount = 0;
                    int totalAudioSourceCount = 0;
                    int totalTileObjectCount = 0;
                    int totalWorldGeometryCount = 0;

                    for (int j = 0; j < project_.Layers[room.Id].Count; j++)
                    {
                        LayerDto currentLayer = projectController_.GetLayerByOrdinal(i, j);

                        int layerOrdinal = projectController_.GetLayerOrdinalFromIndex(i, j);

                        int layerIndex = projectController_.GetLayerIndexFromOrdinal(i, j);

                        totalActorInstanceCount += mapWidgetInstanceLists[MapWidgetType.Actor][currentLayer.Id].Count;

                        totalEventInstanceCount += mapWidgetInstanceLists[MapWidgetType.Event][currentLayer.Id].Count;

                        totalSpawnPointCount += mapWidgetInstanceLists[MapWidgetType.SpawnPoint][currentLayer.Id].Count;

                        totalParticleEmitterCount += mapWidgetInstanceLists[MapWidgetType.ParticleEmitter][currentLayer.Id].Count;

                        for (int k = 0; k < mapWidgetInstanceLists[MapWidgetType.ParticleEmitter][currentLayer.Id].Count; k++)
                        {
                            totalParticleCount += ((ParticleEmitterWidgetDto)(mapWidgetInstanceLists[MapWidgetType.ParticleEmitter][currentLayer.Id][k])).MaxParticles;
                        }

                        totalAudioSourceCount += mapWidgetInstanceLists[MapWidgetType.AudioSource][currentLayer.Id].Count;

                        totalTileObjectCount += mapWidgetInstanceLists[MapWidgetType.TileObject][currentLayer.Id].Count;

                        // Only export world geometry on the interactive layer.
                        if (currentLayer.Id == interactiveLayer.Id)
                        {
                            totalWorldGeometryCount += mapWidgetInstanceLists[MapWidgetType.WorldGeometry][currentLayer.Id].Count;
                        }
                    }

                    // Do a shallow pass through the layers to get the total number of elements
                    int totalElementcount = hudElementInstanceCount +
                                            totalActorInstanceCount +
                                            totalEventInstanceCount +
                                            totalSpawnPointCount +
                                            totalParticleEmitterCount +
                                            totalParticleCount +
                                            totalAudioSourceCount +
                                            totalTileObjectCount +
                                            totalWorldGeometryCount;

                    bw.Write(totalElementcount);

                    // Next, write the HUD element data.

                    bw.Write(hudElementInstanceCount);

                    //for (int j = 0; j < hudElementInstanceCount; j++)
                    foreach (HudElementWidgetDto hudElementInstance in mapWidgetInstanceLists[MapWidgetType.HudElement][room.Id])
                    {
                        //HudElementWidgetDto hudElementInstance = mapWidgetInstanceLists[MapWidgetType.HudElement][room.Id][j];

                        byte[] guidEntityId = hudElementInstance.EntityId.ToByteArray();
                        formatGuidForBoost(guidEntityId);
                        bw.Write(guidEntityId);

                        bw.Write(hudElementInstance.Position.X);
                        bw.Write(hudElementInstance.Position.Y);
                        bw.Write(hudElementInstance.AcceptInput);
                        bw.Write(hudElementInstance.RenderOrder);
                        //bw.Write((uint)hudElementInstance.Ownership);

                        string instanceName = hudElementInstance.Name;
                        stringBytes = System.Text.Encoding.ASCII.GetBytes(instanceName);
                        bw.Write(stringBytes.Length);
                        bw.Write(stringBytes);

                        bw.Write(project_.MapWidgetProperties[hudElementInstance.Id].Count);

                        for (int k = 0; k < project_.MapWidgetProperties[hudElementInstance.Id].Count; k++)
                        {
                            PropertyDto instanceProperty = project_.MapWidgetProperties[hudElementInstance.Id][k];

                            stringBytes = System.Text.Encoding.ASCII.GetBytes(instanceProperty.Name);
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);

                            stringBytes = System.Text.Encoding.ASCII.GetBytes(instanceProperty.Value.ToString());
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);
                        }
                    }

                    // Next, write the layer data.
                    bw.Write(project_.Layers[room.Id].Count);

                    int interactiveLayerOrdinal = projectController_.GetLayerOrdinalFromIndex(i, interactiveLayerIndex);

                    bw.Write(interactiveLayerOrdinal);

                    for (int j = 0; j < project_.Layers[room.Id].Count; j++)
                    {
                        LayerDto currentLayer = projectController_.GetLayerByOrdinal(i, j);
                        
                        bw.Write(currentLayer.Rows);
                        bw.Write(currentLayer.Cols);
                        
                        int actorInstanceCount = mapWidgetInstanceLists[MapWidgetType.Actor][currentLayer.Id].Count;

                        bw.Write(actorInstanceCount);
                        
                        foreach (ActorWidgetDto actorInstance in mapWidgetInstanceLists[MapWidgetType.Actor][currentLayer.Id])
                        {
                            byte[] guidEntityId = actorInstance.EntityId.ToByteArray();
                            formatGuidForBoost(guidEntityId);
                            bw.Write(guidEntityId);

                            bw.Write(actorInstance.Position.X);
                            bw.Write(actorInstance.Position.Y);
                            bw.Write(actorInstance.AcceptInput);
                            bw.Write(actorInstance.AttachToCamera);
                            bw.Write(actorInstance.RenderOrder);
                            
                            string instanceName = actorInstance.Name;
                            stringBytes = System.Text.Encoding.ASCII.GetBytes(instanceName);
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);

                            bw.Write(project_.MapWidgetProperties[actorInstance.Id].Count);

                            for (int n = 0; n < project_.MapWidgetProperties[actorInstance.Id].Count; n++)
                            {
                                PropertyDto property = project_.MapWidgetProperties[actorInstance.Id][n];

                                stringBytes = System.Text.Encoding.ASCII.GetBytes(property.Name);
                                bw.Write(stringBytes.Length);
                                bw.Write(stringBytes);

                                stringBytes = System.Text.Encoding.ASCII.GetBytes(property.Value.ToString());
                                bw.Write(stringBytes.Length);
                                bw.Write(stringBytes);
                            }
                        }

                        int eventInstanceCount = mapWidgetInstanceLists[MapWidgetType.Event][currentLayer.Id].Count;

                        bw.Write(eventInstanceCount);
                        
                        foreach (EventWidgetDto eventInstance in mapWidgetInstanceLists[MapWidgetType.Event][currentLayer.Id])
                        {
                            byte[] guidEntityId = eventInstance.EntityId.ToByteArray();
                            formatGuidForBoost(guidEntityId);
                            bw.Write(guidEntityId);

                            bw.Write(Convert.ToInt32(eventInstance.BoundingBox.Left));
                            bw.Write(Convert.ToInt32(eventInstance.BoundingBox.Top));
                            bw.Write(Convert.ToInt32(eventInstance.BoundingBox.Width));
                            bw.Write(Convert.ToInt32(eventInstance.BoundingBox.Height));
                            bw.Write(eventInstance.AcceptInput);
                            //bw.Write((uint)eventInstance.Ownership);

                            string instanceName = eventInstance.Name;
                            stringBytes = System.Text.Encoding.ASCII.GetBytes(instanceName);
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);

                            bw.Write(project_.MapWidgetProperties[eventInstance.Id].Count);

                            for (int n = 0; n < project_.MapWidgetProperties[eventInstance.Id].Count; n++)
                            {
                                PropertyDto property = project_.MapWidgetProperties[eventInstance.Id][n];

                                stringBytes = System.Text.Encoding.ASCII.GetBytes(property.Name);
                                bw.Write(stringBytes.Length);
                                bw.Write(stringBytes);

                                stringBytes = System.Text.Encoding.ASCII.GetBytes(property.Value.ToString());
                                bw.Write(stringBytes.Length);
                                bw.Write(stringBytes);
                            }
                        }

                        // Export the spawn point data.
                        int spawnPointCount = mapWidgetInstanceLists[MapWidgetType.SpawnPoint][currentLayer.Id].Count;

                        bw.Write(spawnPointCount);

                        foreach (MapWidgetDto mapWidget in mapWidgetInstanceLists[MapWidgetType.SpawnPoint][currentLayer.Id])
                        {
                            SpawnPointWidgetDto spawnPoint = (SpawnPointWidgetDto)mapWidget;

                            byte[] guidIdentityId = spawnPoint.Identity.ToByteArray();
                            formatGuidForBoost(guidIdentityId);
                            bw.Write(guidIdentityId);

                            if (spawnPoint.Identity == Guid.Empty)
                            {
                                warningsForm_.AddWarningMessage("Spawn point " + spawnPoint.Name + " identity not set", "Room " + roomName, "Layer " + currentLayer.Name);
                            }

                            bw.Write(Convert.ToInt32(spawnPoint.Position.X));
                            bw.Write(Convert.ToInt32(spawnPoint.Position.Y));
                        }

                        // Export the particle emitter data.
                        int particleEmitterCount = mapWidgetInstanceLists[MapWidgetType.ParticleEmitter][currentLayer.Id].Count;

                        bw.Write(particleEmitterCount);

                        foreach (MapWidgetDto mapWidget in mapWidgetInstanceLists[MapWidgetType.ParticleEmitter][currentLayer.Id])
                        {
                            ParticleEmitterWidgetDto particleEmitter = (ParticleEmitterWidgetDto)mapWidget;

                            if (particleEmitter.ParticleType == Guid.Empty)
                            {
                                warningsForm_.AddWarningMessage("Particle emitter " + particleEmitter.Name + " particle type not set", "Room " + roomName, "Layer " + currentLayer.Name);
                            }

                            if (particleEmitter.Behavior == Guid.Empty)
                            {
                                warningsForm_.AddWarningMessage("Particle emitter " + particleEmitter.Name + " behavior not set", "Room " + roomName, "Layer " + currentLayer.Name);
                            }

                            byte[] guidParticleTypeId = particleEmitter.ParticleType.ToByteArray();
                            formatGuidForBoost(guidParticleTypeId);
                            bw.Write(guidParticleTypeId);

                            byte[] guidParticleEmitterId = particleEmitter.Behavior.ToByteArray();
                            formatGuidForBoost(guidParticleEmitterId);
                            bw.Write(guidParticleEmitterId);
                            
                            byte[] guidAnimationId = particleEmitter.Animation.ToByteArray();
                            formatGuidForBoost(guidAnimationId);
                            bw.Write(guidAnimationId);

                            string instanceName = particleEmitter.Name;
                            stringBytes = System.Text.Encoding.ASCII.GetBytes(instanceName);
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);

                            bw.Write(particleEmitter.ParticlesPerEmission);
                            bw.Write(particleEmitter.MaxParticles);
                            bw.Write(particleEmitter.Interval);
                            bw.Write(particleEmitter.ParticleLifespan);
                            bw.Write(particleEmitter.Active);
                            bw.Write(particleEmitter.AttachParticles);

                            bw.Write(Convert.ToInt32(particleEmitter.BoundingBox.Left));
                            bw.Write(Convert.ToInt32(particleEmitter.BoundingBox.Top));

                            bw.Write(particleEmitter.AnimationFramesPerSecond);
                        }

                        // Export the audio source data.
                        int audioSourceCount = mapWidgetInstanceLists[MapWidgetType.AudioSource][currentLayer.Id].Count;

                        bw.Write(audioSourceCount);

                        foreach (MapWidgetDto mapWidget in mapWidgetInstanceLists[MapWidgetType.AudioSource][currentLayer.Id])
                        {
                            AudioSourceWidgetDto audioSource = (AudioSourceWidgetDto)mapWidget;

                            if (audioSource.Audio == Guid.Empty)
                            {
                                warningsForm_.AddWarningMessage("Audio source " + audioSource.Name + " audio not set", "Room " + roomName, "Layer " + currentLayer.Name);
                            }

                            byte[] guidAudioId = audioSource.Audio.ToByteArray();
                            formatGuidForBoost(guidAudioId);
                            bw.Write(guidAudioId);

                            string instanceName = audioSource.Name;
                            stringBytes = System.Text.Encoding.ASCII.GetBytes(instanceName);
                            bw.Write(stringBytes.Length);
                            bw.Write(stringBytes);

                            bw.Write(audioSource.Autoplay);
                            bw.Write(audioSource.Loop);
                            bw.Write(audioSource.MinDistance);
                            bw.Write(audioSource.MaxDistance);
                            bw.Write(audioSource.Volume);

                            bw.Write(Convert.ToInt32(audioSource.BoundingBox.Left));
                            bw.Write(Convert.ToInt32(audioSource.BoundingBox.Top));
                        }

                        // Export the tile object data.
                        int tileObjectCount = mapWidgetInstanceLists[MapWidgetType.TileObject][currentLayer.Id].Count;

                        bw.Write(tileObjectCount);

                        foreach (MapWidgetDto mapWidget in mapWidgetInstanceLists[MapWidgetType.TileObject][currentLayer.Id])
                        {
                            TileObjectWidgetDto tileObjectWidget = (TileObjectWidgetDto)mapWidget;

                            TileObjectDto tileObject = projectController_.GetTileObject(tileObjectWidget.TileObjectId);

                            TileSheetDto tileSheet = projectController_.GetTileSheet(tileObject.OwnerId);

                            int tileSize = project_.TileSize;
                            
                            float scaleFactor = tileSheet.ScaleFactor;

                            List<Rectangle> sourceRects = new List<Rectangle>();

                            int framesPerSecond = 1;

                            if (tileObject.AnimationId != Guid.Empty)
                            {
                                SceneryAnimationDto sceneryAnimation = projectController_.GetSceneryAnimation(tileObject.AnimationId);

                                framesPerSecond = sceneryAnimation.FramesPerSecond;

                                // If the animation data is not built yet, build it here.
                                if (dictAnimations_.ContainsKey(tileObject.AnimationId) == false)
                                {
                                    dictAnimations_.Add(tileObject.AnimationId, new List<Rectangle>());

                                    // Loop through all tile objects, and if they have this animation ID, add it to the list.
                                    foreach (TileObjectDto currentTileObject in project_.TileObjects[tileSheet.Id])
                                    {
                                        if (currentTileObject.AnimationId == tileObject.AnimationId)
                                        {
                                            Rectangle sourceRect = new Rectangle();

                                            sourceRect.Left = (int)((currentTileObject.TopLeftCornerColumn * tileSize) / scaleFactor);

                                            sourceRect.Top = (int)((currentTileObject.TopLeftCornerRow * tileSize) / scaleFactor);

                                            sourceRect.Width = (int)((currentTileObject.Columns * tileSize) / scaleFactor);

                                            sourceRect.Height = (int)((currentTileObject.Rows * tileSize) / scaleFactor);

                                            dictAnimations_[tileObject.AnimationId].Add(sourceRect);
                                        }
                                    }
                                }

                                sourceRects = dictAnimations_[tileObject.AnimationId];
                            }
                            else
                            {
                                Rectangle sourceRect = new Rectangle();

                                sourceRect.Left = (int)((tileObject.TopLeftCornerColumn * tileSize) / scaleFactor);

                                sourceRect.Top = (int)((tileObject.TopLeftCornerRow * tileSize) / scaleFactor);

                                sourceRect.Width = (int)((tileObject.Columns * tileSize) / scaleFactor);

                                sourceRect.Height = (int)((tileObject.Rows * tileSize) / scaleFactor);

                                sourceRects.Add(sourceRect);
                            }


                            byte[] guidTileSheetId = tileSheet.Id.ToByteArray();
                            formatGuidForBoost(guidTileSheetId);
                            bw.Write(guidTileSheetId);

                            bw.Write(framesPerSecond);

                            bw.Write(sourceRects.Count);

                            for (int k = 0; k < sourceRects.Count; k++)
                            {
                                // Export the region in the native position and size.
                                bw.Write(sourceRects[k].Left);

                                bw.Write(sourceRects[k].Top);

                                bw.Write(sourceRects[k].Width);

                                bw.Write(sourceRects[k].Height);
                            }
                            
                            bw.Write(tileObjectWidget.BoundingBox.Left);

                            bw.Write(tileObjectWidget.BoundingBox.Top);
                        }

                        // Export the world geometry data.
                        if (currentLayer.Id == interactiveLayer.Id)
                        {
                            int worldGeometryCount = mapWidgetInstanceLists[MapWidgetType.WorldGeometry][currentLayer.Id].Count;

                            bw.Write(worldGeometryCount);

                            foreach (MapWidgetDto mapWidget in mapWidgetInstanceLists[MapWidgetType.WorldGeometry][currentLayer.Id])
                            {
                                bw.Write(mapWidget.BoundingBox.Left);

                                bw.Write(mapWidget.BoundingBox.Top);

                                bw.Write(mapWidget.BoundingBox.Width);

                                bw.Write(mapWidget.BoundingBox.Height);

                                WorldGeometryCollisionStyle collisionStyle = ((WorldGeometryWidgetDto)mapWidget).CollisionStyle;

                                bw.Write((uint)collisionStyle);
                                
                                System.Drawing.Point topLeft = new System.Drawing.Point(mapWidget.BoundingBox.Left, mapWidget.BoundingBox.Top);
                                System.Drawing.Point bottomLeft = new System.Drawing.Point(mapWidget.BoundingBox.Left, mapWidget.BoundingBox.Top + mapWidget.BoundingBox.Height);
                                System.Drawing.Point topRight = new System.Drawing.Point(mapWidget.BoundingBox.Left + mapWidget.BoundingBox.Width, mapWidget.BoundingBox.Top);
                                System.Drawing.Point bottomRight = new System.Drawing.Point(mapWidget.BoundingBox.Left + mapWidget.BoundingBox.Width, mapWidget.BoundingBox.Top + mapWidget.BoundingBox.Height);

                                float rise = (float)((WorldGeometryWidgetDto)mapWidget).SlopeRise;
                                float run = (float)(mapWidget.BoundingBox.Width / project_.TileSize);

                                float slope = 0;
                                
                                switch (collisionStyle)
                                {
                                    case WorldGeometryCollisionStyle.Incline:
                                    case WorldGeometryCollisionStyle.InvertedIncline:
                                    case WorldGeometryCollisionStyle.Decline:
                                    case WorldGeometryCollisionStyle.InvertedDecline:

                                        slope = rise / run;

                                        break;
                                }

                                bw.Write(slope);

                                bw.Write(((WorldGeometryWidgetDto)mapWidget).Edges.UseTopEdge);

                                bw.Write(((WorldGeometryWidgetDto)mapWidget).Edges.UseRightEdge);

                                bw.Write(((WorldGeometryWidgetDto)mapWidget).Edges.UseBottomEdge);

                                bw.Write(((WorldGeometryWidgetDto)mapWidget).Edges.UseLeftEdge);                                
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }
        
        private void writeSpawnPointMetadata(BinaryWriter bw)
        {
            Byte[] stringBytes;

            // Write the spawn point data.    
            int spawnPointCount = project_.SpawnPoints.Count;

            bw.Write(spawnPointCount);

            foreach (SpawnPointDto spawnPoint in project_.SpawnPoints)
            {
                string spawnPointScriptName = "SPAWNPOINT_" + spawnPoint.Name.ToUpper();
                spawnPointScriptName = spawnPointScriptName.Replace(" ", "");
                spawnPointScriptName = spawnPointScriptName.Replace("/", "");

                stringBytes = System.Text.Encoding.ASCII.GetBytes(spawnPointScriptName);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                stringBytes = System.Text.Encoding.ASCII.GetBytes(spawnPoint.Name);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                byte[] guidId = spawnPoint.Id.ToByteArray();
                formatGuidForBoost(guidId);
                bw.Write(guidId);
            }
        }

        private void writeSpriteSheetAssetFiles()
        {
            Byte[] stringBytes;

            // Write the sprite sheet data.      
            foreach (SpriteSheetDto spriteSheet in project_.SpriteSheets)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + spriteSheet.Id.ToString() + ".sa");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(spriteSheet.Name);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = spriteSheet.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    byte[] guidBitmapId = spriteSheet.BitmapResourceId.ToByteArray();
                    formatGuidForBoost(guidBitmapId);
                    bw.Write(guidBitmapId);

                    bw.Write(spriteSheet.CellHeight);
                    bw.Write(spriteSheet.CellWidth);
                    bw.Write(spriteSheet.Rows);
                    bw.Write(spriteSheet.Columns);
                    bw.Write(spriteSheet.ScaleFactor);
                    bw.Write(spriteSheet.Padding);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeTileSheetAssetFiles()
        {
            Byte[] stringBytes;

            // Write the tile sheet data.      
            foreach (TileSheetDto tileSheet in project_.TileSheets)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + tileSheet.Id.ToString() + ".ta");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    // C# BinaryWriter writes strings in 7-bit encoded format.
                    // See: http://stackoverflow.com/questions/1550560/encoding-an-integer-in-7-bit-format-of-c-sharp-binaryreader-readstring
                    // Rather than writing a decoder in the c++ end, just write the length of each string as a 4 byte int, and 
                    // save it as a byte array. It works just as well and is easier to do.
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(tileSheet.Name);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = tileSheet.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);

                    byte[] guidBitmapId = tileSheet.BitmapResourceId.ToByteArray();
                    formatGuidForBoost(guidBitmapId);
                    bw.Write(guidBitmapId);

                    // Export the tile graphics in their native size (ex: 16x16 tiles being scaled 3x to fit a 48x48 collision region).
                    // Export the scaling factor so they can be scaled back up when rendered.
                    int scaledWidth = (int)(tileSheet.TileSize / tileSheet.ScaleFactor);

                    int scaledHeight = (int)(tileSheet.TileSize / tileSheet.ScaleFactor);

                    bw.Write(tileSheet.Rows);

                    bw.Write(tileSheet.Columns);

                    bw.Write(scaledWidth);

                    bw.Write(scaledHeight);

                    bw.Write(tileSheet.ScaleFactor);                    
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeTransitionAssetFiles()
        {
            Byte[] stringBytes;

            // Write the transition data.
            List<TransitionDto> transitions = project_.Transitions;

            foreach (TransitionDto transition in transitions)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + transition.Id.ToString() + ".tr");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    string transitionName = transition.Name.ToUpper();
                    transitionName = transitionName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes("TRANSITION_" + transitionName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string transitionFileName = "_" + transition.Name.ToLower();
                    transitionFileName = transitionFileName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(transitionFileName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string transitionClassName = transition.Name;
                    transitionClassName = transitionClassName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(transitionClassName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = transition.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void writeTriggerSignalMetadata(BinaryWriter bw)
        {
            Byte[] stringBytes;

            // Write the trigger signal data.
            int triggerSignalCount = project_.TriggerSignals.Count;

            bw.Write(triggerSignalCount);

            foreach (TriggerSignalDto triggerSignal in project_.TriggerSignals)
            {
                string triggerSignalName = triggerSignal.Name.ToUpper();
                triggerSignalName = triggerSignalName.Replace(" ", "");

                stringBytes = System.Text.Encoding.ASCII.GetBytes("TRIGGERSIGNAL_" + triggerSignalName);
                bw.Write(stringBytes.Length);
                bw.Write(stringBytes);

                byte[] triggerSignalGuidId = triggerSignal.Id.ToByteArray();
                formatGuidForBoost(triggerSignalGuidId);
                bw.Write(triggerSignalGuidId);
            }
        }

        private void writeUiWidgetAssetFiles()
        {
            Byte[] stringBytes;

            // Write the menu items data.
            List<UiWidgetDto> uiWidgets = project_.UiWidgets;

            foreach (UiWidgetDto uiWidget in uiWidgets)
            {
                FileStream fs = System.IO.File.Create(exportPath_ + "\\Data\\Assets\\" + uiWidget.Id.ToString() + ".mi");
                BinaryWriter bw = new BinaryWriter(fs);

                try
                {
                    bw.Write(exportVersion_.Major);
                    bw.Write(exportVersion_.Minor);
                    bw.Write(exportVersion_.Revision);

                    string uiWidgetName = uiWidget.Name.ToUpper();
                    uiWidgetName = uiWidgetName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes("UIWIDGET_" + uiWidgetName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string uiWidgetFileName = "_uiwidget" + uiWidget.Name.ToLower();
                    uiWidgetFileName = uiWidgetFileName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(uiWidgetFileName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    string uiWidgetClassName = "UiWidget" + uiWidget.Name;
                    uiWidgetClassName = uiWidgetClassName.Replace(" ", "");

                    stringBytes = System.Text.Encoding.ASCII.GetBytes(uiWidgetClassName);
                    bw.Write(stringBytes.Length);
                    bw.Write(stringBytes);

                    byte[] guidId = uiWidget.Id.ToByteArray();
                    formatGuidForBoost(guidId);
                    bw.Write(guidId);
                }
                catch (IOException ex)
                {
                    throw ex;
                }
                finally
                {
                    bw.Close();
                    fs.Close();
                }
            }
        }

        #endregion
    }
}