using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class EngineScriptGenerator : IScriptGenerator
    {
        IResourceTextReader resourceTextReader_;

        public EngineScriptGenerator(IResourceTextReader resourceTextReader)
        {
            resourceTextReader_ = resourceTextReader;
        }

        public string Generate(ScriptDto script)
        {
            string scriptCode = resourceTextReader_.ReadResourceText("BaseEngineScript");
            
            return scriptCode;
        }
    }
}
