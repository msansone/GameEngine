using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class MissingScriptGenerator : IScriptGenerator
    {
        public string Generate(ScriptDto script)
        {
            System.Windows.Forms.MessageBox.Show("Unable to generate script of type: " + script.ScriptType.ToString() + ". Script generator not implemented.");

            return string.Empty;
        }
    }
}
