using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class NetworkHandlerScriptGenerator : IScriptGenerator 
    {
        IResourceTextReader resourceTextReader_;

        public NetworkHandlerScriptGenerator(IResourceTextReader resourceTextReader)
        {
            resourceTextReader_ = resourceTextReader;
        }

        public string Generate(ScriptDto script)
        {
            string scriptCode = resourceTextReader_.ReadResourceText("BaseNetworkHandlerScript");

            scriptCode = scriptCode.Replace("%NAME%", script.Name);

            return scriptCode;
        }
    }
}
