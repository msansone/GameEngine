using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class BlankScriptGenerator : IScriptGenerator
    {
        public string Generate(ScriptDto script)
        {
            return string.Empty;
        }
    }
}
