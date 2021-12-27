using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FiremelonEditor2
{
    internal class GlobalVars
    {
        internal static string[] listOfSpriteSheets_;
        internal static string[] listOfHitboxIdentities_;
        internal static string[] listOfTriggerSignals_;
        internal static string[] listOfAnimations_;
        internal static string[] listOfSceneryAnimations_;
        internal static string[] listOfStates_;
        internal static string[] listOfSpawnPoints_;
        internal static string[] listOfLoadingScreens_;
        internal static string[] listOfTransitions_;
        internal static string[] listOfParticles_;
        internal static string[] listOfParticleEmitters_;
        internal static string[] listOfAudioResources_;
        internal static string[] listOfGameButtonGroups_;
    }

    class SpriteSheetConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(GlobalVars.listOfSpriteSheets_);
        }
    }
}
