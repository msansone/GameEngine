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
    public enum ScriptType
    {
        Script = 0,
        Room = 1,
        Entity = 2,
        Query = 3,
        UiWidget = 4,
        LoadingScreen = 5,
        Transition = 6,
        Particle = 7,
        NetworkHandler = 8,
        UiManager = 9,
        ParticleEmitter = 10,
        Engine = 11,
        Panels = 12
    };

    public class ScriptDto : BaseDto
    {
        private string scriptPath_ = string.Empty;
        public string ScriptPath
        {
            get { return scriptPath_; }
            set { scriptPath_ = value; }
        }

        private string scriptRelativePath_ = string.Empty;
        public string ScriptRelativePath
        {
            get { return scriptRelativePath_; }
            set { scriptRelativePath_ = value; }
        }

        private ScriptType scriptType_;
        public ScriptType ScriptType
        {
            get { return scriptType_; }
            set { scriptType_ = value; }
        }
    }
    public class ScriptDtoProxy : IScriptDtoProxy
    {
        IProjectController projectController_;
        Guid scriptId_ = Guid.Empty;

        public ScriptDtoProxy(IProjectController projectController, Guid scriptId)
        {
            projectController_ = projectController;
            scriptId_ = scriptId;
        }

        public ScriptDto GetScript()
        {
            ScriptDto script = projectController_.GetScript(scriptId_);

            return script;
        }

        // Hide the name for scripts, it's pointless.
        [BrowsableAttribute(false)]
        public string Name
        {
            get
            {
                ScriptDto script = projectController_.GetScript(scriptId_);

                return script.Name;
            }
            set
            {
                return;
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

    public interface IScriptDtoProxy : IBaseDtoProxy
    {
        string ScriptPath { get; set; }

        ScriptDto GetScript();
    }
}
