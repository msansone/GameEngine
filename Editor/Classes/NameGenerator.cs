using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class NameGenerator : INameGenerator
    {
        #region Constructors
          
        public NameGenerator()
        {
            dictMapWidgetNameIdMap.Clear();
            dictMapWidgetIdNameMap.Clear();
        }

        #endregion

        #region Private Variables
        
        // A map of widget names to IDs, and the inverse.
        private Dictionary<string, Guid> dictMapWidgetNameIdMap = new Dictionary<string, Guid>();
        private Dictionary<Guid, string> dictMapWidgetIdNameMap = new Dictionary<Guid, string>();

        #endregion

        #region Public Functions

        public void AddMapWidgetName(MapWidgetDto mapWidget)
        {
            dictMapWidgetNameIdMap.Add(mapWidget.Name, mapWidget.Id);
            dictMapWidgetIdNameMap.Add(mapWidget.Id, mapWidget.Name);
        }

        public void ClearMapWidgetNames()
        {
            dictMapWidgetNameIdMap.Clear();
            dictMapWidgetIdNameMap.Clear();
        }

        public string GetNextAvailableAssetName(string baseName, ProjectDto project)
        {
            bool isNameValid = false;
            int counter = 0;
            string currentName = baseName;

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = baseName + " " + counter.ToString();
                }

                isNameValid = !IsAssetNameInUse(Guid.Empty, project, currentName);

                counter++;
            }

            return currentName.Trim();
        }

        public bool IsAssetNameInUse(Guid id, ProjectDto project, string name)
        {
            // Make sure names for scripted objects are not duplicated. Ideally there should be one list of all scripted object names,
            // but for now, check each object type separately.

            for (int i = 0; i < project.Actors.Count; i++)
            {
                if (name.ToUpper() == project.Actors[i].Name.ToUpper() && project.Actors[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.Events.Count; i++)
            {
                if (name.ToUpper() == project.Events[i].Name.ToUpper() && project.Events[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.HudElements.Count; i++)
            {
                if (name.ToUpper() == project.HudElements[i].Name.ToUpper() && project.HudElements[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.UiWidgets.Count; i++)
            {
                if (name.ToUpper() == project.UiWidgets[i].Name.ToUpper() && project.UiWidgets[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.LoadingScreens.Count; i++)
            {
                if (name.ToUpper() == project.LoadingScreens[i].Name.ToUpper() && project.LoadingScreens[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.Transitions.Count; i++)
            {
                if (name.ToUpper() == project.Transitions[i].Name.ToUpper() && project.Transitions[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.Particles.Count; i++)
            {
                if (name.ToUpper() == project.Particles[i].Name.ToUpper() && project.Particles[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.ParticleEmitters.Count; i++)
            {
                if (name.ToUpper() == project.ParticleEmitters[i].Name.ToUpper() && project.ParticleEmitters[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.Rooms.Count; i++)
            {
                if (name.ToUpper() == project.Rooms[i].Name.ToUpper() && project.Rooms[i].Id != id)
                {
                    return true;
                }
            }

            for (int i = 0; i < project.SpawnPoints.Count; i++)
            {
                if (name.ToUpper() == project.SpawnPoints[i].Name.ToUpper() && project.SpawnPoints[i].Id != id)
                {
                    return true;
                }
            }

            foreach (KeyValuePair<Guid, ScriptDto> script in project.Scripts)
            {
                if (name.ToUpper() == script.Value.Name.ToUpper() && script.Value.Id != id)
                {
                    return true;
                }
            }

            if (name.ToUpper() == project.Scripts[Globals.UiManagerId].Name.ToUpper())
            {
                return true;
            }

            if (name.ToUpper() == project.Scripts[Globals.NetworkHandlerId].Name.ToUpper())
            {
                return true;
            }

            if (name.ToUpper() == project.Scripts[Globals.CameraScriptId].Name.ToUpper())
            {
                return true;
            }

            if (name.ToUpper() == project.Scripts[Globals.EngineScriptId].Name.ToUpper())
            {
                return true;
            }

            return false;
        }

        public bool IsMapWidgetNameInUse(string name)
        {
            return dictMapWidgetNameIdMap.ContainsKey(name);
        }

        public void RemoveMapWidgetName(MapWidgetDto mapWidget)
        {
            dictMapWidgetIdNameMap.Remove(mapWidget.Id);
            dictMapWidgetNameIdMap.Remove(mapWidget.Name);
        }

        public void SetNextAvailableMapWidgetName(string baseName, MapWidgetDto mapWidget)
        {
            // Generate a unique name for this instance. Keep trying sequential names until an unused name is found.
            int instanceCounter = 1;

            string uniqueName = baseName + " " + instanceCounter;

            bool foundAvailableName = false;

            while (foundAvailableName == false)
            {
                if (dictMapWidgetNameIdMap.ContainsKey(uniqueName) == false)
                {
                    foundAvailableName = true;
                }
                else
                {
                    instanceCounter++;
                    
                    uniqueName = baseName + " " + instanceCounter;
                }
            }
            
            mapWidget.Name = uniqueName;

            AddMapWidgetName(mapWidget);
        }

        public void UpdateMapWidgetName(MapWidgetDto mapWidget, string name)
        {
            string oldName = mapWidget.Name;

            dictMapWidgetIdNameMap[mapWidget.Id] = name;

            dictMapWidgetNameIdMap.Remove(oldName);
            dictMapWidgetNameIdMap.Add(name, mapWidget.Id);

            mapWidget.Name = name;
        }

        #endregion
    }

    public interface INameGenerator
    {
        void AddMapWidgetName(MapWidgetDto mapWidget);

        void ClearMapWidgetNames();

        string GetNextAvailableAssetName(string baseName, ProjectDto project);

        bool IsAssetNameInUse(Guid id, ProjectDto project, string name);

        bool IsMapWidgetNameInUse(string name);

        void RemoveMapWidgetName(MapWidgetDto mapWidget);

        void SetNextAvailableMapWidgetName(string baseName, MapWidgetDto mapWidget);

        void UpdateMapWidgetName(MapWidgetDto mapWidget, string name);
    }
}
