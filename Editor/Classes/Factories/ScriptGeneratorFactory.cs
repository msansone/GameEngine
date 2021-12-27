using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class ScriptGeneratorFactory : IScriptGeneratorFactory
    {
        private IFiremelonEditorFactory factory_;

        public ScriptGeneratorFactory()
        {
            factory_ = new FiremelonEditorFactory();
        }

        public IScriptGenerator NewScriptGenerator(ScriptDto script)
        {
            IResourceTextReader resourceTextReader = factory_.NewResourceTextReader();

            switch (script.ScriptType)
            {
                case ScriptType.Script:
                    return new BlankScriptGenerator();

                case ScriptType.Room:
                    return new RoomScriptGenerator(resourceTextReader);

                case ScriptType.Entity:
                    return new EntityScriptGenerator(resourceTextReader);

                case ScriptType.Query:
                    return new QueryScriptGenerator(resourceTextReader);

                case ScriptType.UiWidget:
                    return new UiWidgetScriptGenerator(resourceTextReader);

                case ScriptType.LoadingScreen:
                    return new LoadingScreenScriptGenerator(resourceTextReader);

                case ScriptType.Transition:
                    return new TransitionScriptGenerator(resourceTextReader);

                case ScriptType.Particle:
                    return new ParticleScriptGenerator(resourceTextReader);

                case ScriptType.ParticleEmitter:
                    return new ParticleEmitterScriptGenerator(resourceTextReader);

                case ScriptType.NetworkHandler:
                    return new NetworkHandlerScriptGenerator(resourceTextReader);

                case ScriptType.UiManager:
                    return new UiManagerScriptGenerator(resourceTextReader);

                case ScriptType.Engine:
                    return new EngineScriptGenerator(resourceTextReader);

            }

            return new MissingScriptGenerator();
        }
    }

    public interface IScriptGeneratorFactory
    {
        IScriptGenerator NewScriptGenerator(ScriptDto script);
    }
}
