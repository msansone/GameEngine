using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class ParticleEmitterScriptGenerator : IScriptGenerator
    {
        IResourceTextReader resourceTextReader_;

        public ParticleEmitterScriptGenerator(IResourceTextReader resourceTextReader)
        {
            resourceTextReader_ = resourceTextReader;
        }

        public string Generate(ScriptDto script)
        {
            string scriptCode = resourceTextReader_.ReadResourceText("BaseParticleEmitterScript");

            scriptCode = scriptCode.Replace("%NAME%", script.Name);

            return scriptCode;
        }
    }
}
