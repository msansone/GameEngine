using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class LoadingScreenDto : BaseDto
    {
    }

    class LoadingScreenDtoProxy : ILoadingScreenDtoProxy
    {
        IProjectController projectController_;
        Guid loadingScreenId_;

        public LoadingScreenDtoProxy(IProjectController projectController, Guid loadingScreenId)
        {
            projectController_ = projectController;
            loadingScreenId_ = loadingScreenId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                LoadingScreenDto loadingScreen = projectController_.GetLoadingScreen(loadingScreenId_);

                return loadingScreen.Name;
            }
            set
            {
                try
                {
                    projectController_.SetLoadingScreenName(loadingScreenId_, value);
                }
                catch (InvalidNameException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return loadingScreenId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                LoadingScreenDto loadingScreen = projectController_.GetLoadingScreen(loadingScreenId_);

                return loadingScreen.OwnerId;
            }
        }

        [CategoryAttribute("Data Source Settings"),
         DescriptionAttribute("The location of the python script file"),
         Editor(typeof(PythonScriptFilePathUiTypeEditor), typeof(UITypeEditor))]
        public string Script
        {
            get
            {
                ScriptDto script = projectController_.GetScriptByOwnerId(loadingScreenId_);

                if (script == null)
                {
                    return string.Empty;
                }
                else
                {
                    return script.ScriptPath;
                }
            }
            set
            {
                ScriptDto script = projectController_.GetScriptByOwnerId(loadingScreenId_);

                projectController_.SetScriptPath(script.Id, value);
            }
        }
    }

    public interface ILoadingScreenDtoProxy : IBaseDtoProxy
    {
    }
}
