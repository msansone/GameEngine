using System;
using System.Collections.Generic;

namespace FiremelonEditor2
{
    public interface INameUtility
    {        
        void AddAnimationNames(List<string> name);
        void AddAudioAssetNames(List<string> name);
        void AddGameButtonGroupNames(List<string> name);
        void AddHitboxIdentityNames(List<string> name);
        void AddLoadingScreenNames(List<string> name);
        void AddParticleEmitterNames(List<string> name);
        void AddParticleNames(List<string> name);
        void AddSceneryAnimationNames(Guid entityId, List<string> name);
        void AddSpawnPointNames(List<string> name);
        void AddSpriteSheetNames(List<string> name);
        void AddStateNames(Guid entityId, List<string> name);
        void AddTransitionNames(List<string> name);
        void AddTriggerSignalNames(List<string> name);

        void ClearNames();

        List<string> GetSceneryAnimationNames(Guid tileSheetId);
        List<string> GetStateNames(Guid entityId);

        void RemoveAnimationName(string name);
        void RemoveAudioAssetName(string name);
        void RemoveGameButtonGroupName(string name);
        void RemoveHitboxIdentityName(string name);
        void RemoveLoadingScreenName(string name);
        void RemoveParticleEmitterName(string name);
        void RemoveParticleName(string name);
        void RemoveSceneryAnimationName(Guid tileSheetId, string name);
        void RemoveSpawnPointName(string name);
        void RemoveSpriteSheetName(string name);
        void RemoveStateName(Guid entityId, string name);
        void RemoveTransitionName(string name);
        void RemoveTriggerSignalName(string name);
        
        void UpdateAnimationName(string name, string newName);
        void UpdateAudioAssetName(string name, string newName);
        void UpdateGameButtonGroupName(string name, string newName);
        void UpdateHitboxIdentityName(string name, string newName);
        void UpdateLoadingScreenName(string name, string newName);
        void UpdateParticleEmitterName(string name, string newName);
        void UpdateParticleName(string name, string newName);
        void UpdateSceneryAnimationName(Guid tileSheetId, string name, string newName);
        void UpdateSpawnPointName(string name, string newName);
        void UpdateSpriteSheetName(string name, string newName);
        void UpdateStateName(Guid entityId, string name, string newName);
        void UpdateTransitionName(string name, string newName);
        void UpdateTriggerSignalName(string name, string newName);
    }
}
