using System;
using System.Collections.Generic;

namespace FiremelonEditor2
{
    public class NameUtility : INameUtility
    {
        #region Private Variables

        private Dictionary<Guid, List<string>> dictSceneryAnimationNames_ = new Dictionary<Guid, List<string>>();
        private Dictionary<Guid, List<string>> dictStateNames_ = new Dictionary<Guid, List<string>>();
        private List<string> lstAnimationNames_ = new List<string>();
        private List<string> lstAudioAssetNames_ = new List<string>();
        private List<string> lstGameButtonGroupNames_ = new List<string>();
        private List<string> lstHitboxIdentityNames_ = new List<string>();
        private List<string> lstLoadingScreenNames_ = new List<string>();
        private List<string> lstParticleEmitterNames_ = new List<string>();
        private List<string> lstParticleNames_ = new List<string>();
        private List<string> lstSpawnPointNames_ = new List<string>();
        private List<string> lstSpriteSheetNames_ = new List<string>();
        private List<string> lstTransitionNames_ = new List<string>();
        private List<string> lstTriggerSignalNames_ = new List<string>();

        #endregion

        #region Public Functions

        public void AddAnimationNames(List<string> names)
        {
            lstAnimationNames_.AddRange(names);

            lstAnimationNames_.Sort();

            GlobalVars.listOfAnimations_ = lstAnimationNames_.ToArray();
        }

        public void AddAudioAssetNames(List<string> names)
        {
            lstAudioAssetNames_.AddRange(names);

            lstAudioAssetNames_.Sort();

            GlobalVars.listOfAudioResources_ = lstAudioAssetNames_.ToArray();
        }

        public void AddGameButtonGroupNames(List<string> names)
        {
            lstGameButtonGroupNames_.AddRange(names);

            lstGameButtonGroupNames_.Sort();

            GlobalVars.listOfGameButtonGroups_ = lstGameButtonGroupNames_.ToArray();
        }

        public void AddHitboxIdentityNames(List<string> names)
        {
            lstHitboxIdentityNames_.AddRange(names);

            lstHitboxIdentityNames_.Sort();

            GlobalVars.listOfHitboxIdentities_ = lstHitboxIdentityNames_.ToArray();
        }

        public void AddLoadingScreenNames(List<string> names)
        {
            lstLoadingScreenNames_.AddRange(names);

            lstLoadingScreenNames_.Sort();

            GlobalVars.listOfLoadingScreens_ = lstLoadingScreenNames_.ToArray();
        }

        public void AddParticleEmitterNames(List<string> names)
        {
            lstParticleEmitterNames_.AddRange(names);

            lstParticleEmitterNames_.Sort();

            GlobalVars.listOfParticleEmitters_ = lstParticleEmitterNames_.ToArray();
        }

        public void AddParticleNames(List<string> names)
        {
            lstParticleNames_.AddRange(names);

            lstParticleNames_.Sort();

            GlobalVars.listOfParticles_ = lstParticleNames_.ToArray();
        }
        
        public void AddSceneryAnimationNames(Guid tileSheetId, List<string> names)
        {
            if (dictSceneryAnimationNames_.ContainsKey(tileSheetId) == false)
            {
                dictSceneryAnimationNames_.Add(tileSheetId, new List<string>());
            }

            dictSceneryAnimationNames_[tileSheetId].AddRange(names);

            dictSceneryAnimationNames_[tileSheetId].Sort();
        }

        public void AddSpawnPointNames(List<string> names)
        {
            lstSpawnPointNames_.AddRange(names);

            lstSpawnPointNames_.Sort();

            GlobalVars.listOfSpawnPoints_ = lstSpawnPointNames_.ToArray();
        }

        public void AddSpriteSheetNames(List<string> names)
        {
            lstSpriteSheetNames_.AddRange(names);

            lstSpriteSheetNames_.Sort();
            
            GlobalVars.listOfSpriteSheets_ = lstSpriteSheetNames_.ToArray();
        }

        public void AddStateNames(Guid entityId, List<string> names)
        {
            if (dictStateNames_.ContainsKey(entityId) == false)
            {
                dictStateNames_.Add(entityId, new List<string>());
            }

            dictStateNames_[entityId].AddRange(names);

            dictStateNames_[entityId].Sort();
        }

        public void AddTransitionNames(List<string> names)
        {
            lstTransitionNames_.AddRange(names);

            lstTransitionNames_.Sort();

            GlobalVars.listOfTransitions_ = lstTransitionNames_.ToArray();
        }

        public void AddTriggerSignalNames(List<string> names)
        {
            lstTriggerSignalNames_.AddRange(names);

            lstTriggerSignalNames_.Sort();

            GlobalVars.listOfTriggerSignals_ = lstTriggerSignalNames_.ToArray();
        }
        
        public void ClearNames()
        {
            foreach (KeyValuePair<Guid, List<string>> kvp in dictStateNames_)
            {
                kvp.Value.Clear();
            }

            foreach (KeyValuePair<Guid, List<string>> kvp in dictSceneryAnimationNames_)
            {
                kvp.Value.Clear();
            }

            lstAnimationNames_.Clear();
            lstAudioAssetNames_.Clear();
            lstGameButtonGroupNames_.Clear();
            lstHitboxIdentityNames_.Clear();
            lstLoadingScreenNames_.Clear();
            lstParticleEmitterNames_.Clear();
            lstParticleNames_.Clear();
            lstSpawnPointNames_.Clear();
            lstSpriteSheetNames_.Clear();
            lstTransitionNames_.Clear();
            lstTriggerSignalNames_.Clear();
        }

        public List<string> GetSceneryAnimationNames(Guid tileSheetId)
        {
            if (dictSceneryAnimationNames_.ContainsKey(tileSheetId) == true)
            {
                return dictSceneryAnimationNames_[tileSheetId];
            }
            else
            {
                return new List<string>();
            }
        }

        public List<string> GetStateNames(Guid entityId)
        {
            if (dictStateNames_.ContainsKey(entityId) == true)
            {
                return dictStateNames_[entityId];
            }
            else
            {
                return new List<string>();
            }
        }

        public void RemoveAnimationName(string name)
        {
            int animationIndex = lstAnimationNames_.IndexOf(name);

            lstAnimationNames_.RemoveAt(animationIndex);

            GlobalVars.listOfAnimations_ = lstAnimationNames_.ToArray();
        }

        public void RemoveAudioAssetName(string name)
        {
            int audioAssetIndex = lstAudioAssetNames_.IndexOf(name);

            lstAudioAssetNames_.RemoveAt(audioAssetIndex);

            GlobalVars.listOfAudioResources_ = lstAudioAssetNames_.ToArray();
        }

        public void RemoveGameButtonGroupName(string name)
        {
            int gameButtonGroupIndex = lstGameButtonGroupNames_.IndexOf(name);
                      
            lstGameButtonGroupNames_.RemoveAt(gameButtonGroupIndex);

            GlobalVars.listOfGameButtonGroups_ = lstGameButtonGroupNames_.ToArray();
        }

        public void RemoveHitboxIdentityName(string name)
        {
            int hitboxIdentityIndex = lstHitboxIdentityNames_.IndexOf(name);

            lstHitboxIdentityNames_.RemoveAt(hitboxIdentityIndex);

            GlobalVars.listOfHitboxIdentities_ = lstHitboxIdentityNames_.ToArray();
        }

        public void RemoveLoadingScreenName(string name)
        {
            int loadingScreenIndex = lstLoadingScreenNames_.IndexOf(name);

            lstLoadingScreenNames_.RemoveAt(loadingScreenIndex);

            GlobalVars.listOfLoadingScreens_ = lstLoadingScreenNames_.ToArray();
        }

        public void RemoveParticleEmitterName(string name)
        {
            int particleEmitterIndex = lstParticleEmitterNames_.IndexOf(name);

            lstParticleEmitterNames_.RemoveAt(particleEmitterIndex);

            GlobalVars.listOfParticleEmitters_ = lstParticleEmitterNames_.ToArray();
        }

        public void RemoveParticleName(string name)
        {
            int particleIndex = lstParticleNames_.IndexOf(name);

            lstParticleNames_.RemoveAt(particleIndex);

            GlobalVars.listOfParticles_ = lstParticleNames_.ToArray();
        }

        public void RemoveSceneryAnimationName(Guid tileSheetId, string name)
        {
            int sceneryAnimationNameIndex = dictSceneryAnimationNames_[tileSheetId].IndexOf(name);

            dictSceneryAnimationNames_[tileSheetId].RemoveAt(sceneryAnimationNameIndex);
        }

        public void RemoveSpawnPointName(string name)
        {
            int spawnPointIndex = lstSpawnPointNames_.IndexOf(name);

            lstSpawnPointNames_.RemoveAt(spawnPointIndex);

            GlobalVars.listOfSpawnPoints_ = lstSpawnPointNames_.ToArray();
        }

        public void RemoveSpriteSheetName(string name)
        {
            int spriteSheetIndex = lstSpriteSheetNames_.IndexOf(name);

            lstSpriteSheetNames_.RemoveAt(spriteSheetIndex);

            GlobalVars.listOfSpriteSheets_ = lstSpriteSheetNames_.ToArray();
        }

        public void RemoveStateName(Guid entityId, string name)
        {
            int stateNameIndex = dictStateNames_[entityId].IndexOf(name);

            dictStateNames_[entityId].RemoveAt(stateNameIndex);
        }

        public void RemoveTransitionName(string name)
        {
            int transitionIndex = lstTransitionNames_.IndexOf(name);

            lstTransitionNames_.RemoveAt(transitionIndex);

            GlobalVars.listOfTransitions_ = lstTransitionNames_.ToArray();
        }

        public void RemoveTriggerSignalName(string name)
        {
            int triggerSignalIndex = lstTriggerSignalNames_.IndexOf(name);

            lstTriggerSignalNames_.RemoveAt(triggerSignalIndex);

            GlobalVars.listOfTriggerSignals_ = lstTriggerSignalNames_.ToArray();
        }
        
        public void UpdateAnimationName(string name, string newName)
        {
            int animationIndex = lstAnimationNames_.IndexOf(name);

            lstAnimationNames_[animationIndex] = newName;

            lstAnimationNames_.Sort();

            GlobalVars.listOfAnimations_ = lstAnimationNames_.ToArray();

        }

        public void UpdateAudioAssetName(string name, string newName)
        {
            int audioAssetNameIndex = lstAudioAssetNames_.IndexOf(name);

            lstAudioAssetNames_[audioAssetNameIndex] = newName;

            lstAudioAssetNames_.Sort();

            GlobalVars.listOfAudioResources_ = lstAudioAssetNames_.ToArray();

        }

        public void UpdateGameButtonGroupName(string name, string newName)
        {
            int gameButtonGroupNameIndex = lstGameButtonGroupNames_.IndexOf(name);

            lstGameButtonGroupNames_[gameButtonGroupNameIndex] = newName;

            lstGameButtonGroupNames_.Sort();

            GlobalVars.listOfGameButtonGroups_ = lstGameButtonGroupNames_.ToArray();
        }

        public void UpdateHitboxIdentityName(string name, string newName)
        {
            int hitboxIdentityIndex = lstHitboxIdentityNames_.IndexOf(name);

            lstHitboxIdentityNames_[hitboxIdentityIndex] = newName;

            lstHitboxIdentityNames_.Sort();

            GlobalVars.listOfHitboxIdentities_ = lstHitboxIdentityNames_.ToArray();
        }

        public void UpdateLoadingScreenName(string name, string newName)
        {
            int loadingScreenNameIndex = lstLoadingScreenNames_.IndexOf(name);

            lstLoadingScreenNames_[loadingScreenNameIndex] = newName;

            lstLoadingScreenNames_.Sort();

            GlobalVars.listOfLoadingScreens_ = lstLoadingScreenNames_.ToArray();
        }

        public void UpdateParticleEmitterName(string name, string newName)
        {
            int particleEmitterNameIndex = lstParticleEmitterNames_.IndexOf(name);

            lstParticleEmitterNames_[particleEmitterNameIndex] = newName;

            lstParticleEmitterNames_.Sort();

            GlobalVars.listOfParticleEmitters_ = lstParticleEmitterNames_.ToArray();
        }

        public void UpdateParticleName(string name, string newName)
        {
            int particleNameIndex = lstParticleNames_.IndexOf(name);

            lstParticleNames_[particleNameIndex] = newName;

            lstParticleNames_.Sort();

            GlobalVars.listOfParticles_ = lstParticleNames_.ToArray();

        }

        public void UpdateSpawnPointName(string name, string newName)
        {
            int spawnPointNameIndex = lstSpawnPointNames_.IndexOf(name);

            lstSpawnPointNames_[spawnPointNameIndex] = newName;

            lstSpawnPointNames_.Sort();

            GlobalVars.listOfSpawnPoints_ = lstSpawnPointNames_.ToArray();
        }

        public void UpdateSpriteSheetName(string name, string newName)
        {
            int spriteSheetNameIndex = lstSpriteSheetNames_.IndexOf(name);

            lstSpriteSheetNames_[spriteSheetNameIndex] = newName;

            lstSpriteSheetNames_.Sort();

            GlobalVars.listOfSpriteSheets_ = lstSpriteSheetNames_.ToArray();
        }

        public void UpdateStateName(Guid entityId, string name, string newName)
        {
        }

        public void UpdateSceneryAnimationName(Guid tileSheetId, string name, string newName)
        {
        }

        public void UpdateTransitionName(string name, string newName)
        {
            int transitionNameIndex = lstTransitionNames_.IndexOf(name);

            lstTransitionNames_[transitionNameIndex] = newName;

            lstTransitionNames_.Sort();

            GlobalVars.listOfTransitions_ = lstTransitionNames_.ToArray();
        }

        public void UpdateTriggerSignalName(string name, string newName)
        {
            int triggerSignalIndex = lstTriggerSignalNames_.IndexOf(name);

            lstTriggerSignalNames_[triggerSignalIndex] = newName;

            lstTriggerSignalNames_.Sort();

            GlobalVars.listOfTriggerSignals_ = lstTriggerSignalNames_.ToArray();
        }

        #endregion
    }
}