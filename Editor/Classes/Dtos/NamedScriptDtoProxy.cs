using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    public class NamedScriptDtoProxy : IScriptDtoProxy
    {
        IProjectController projectController_;
        Guid scriptId_ = Guid.Empty;

        public NamedScriptDtoProxy(IProjectController projectController, Guid scriptId)
        {
            projectController_ = projectController;
            scriptId_ = scriptId;
        }

        public ScriptDto GetScript()
        {
            ScriptDto script = projectController_.GetScript(scriptId_);

            return script;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                ScriptDto script = projectController_.GetScript(scriptId_);

                return script.Name;
            }
            set
            {
                try
                {
                    projectController_.SetScriptName(scriptId_, value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error Setting Script Name", MessageBoxButtons.OK);
                }
            }
        }

        [CategoryAttribute("Data Source Settings"),
         DescriptionAttribute("The location of the python script file"),
        Editor(typeof(PythonScriptFilePathUiTypeEditor), typeof(UITypeEditor))]
        public string ScriptPath
        {
            get
            {
                ScriptDto script = projectController_.GetScript(scriptId_);

                return script.ScriptPath;
            }
            set
            {
                projectController_.SetScriptPath(scriptId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return scriptId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                ScriptDto script = projectController_.GetScript(scriptId_);

                return script.OwnerId;
            }
        }
    }
}
