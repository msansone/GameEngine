using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace FiremelonEditor2
{
    public class TransitionDto : BaseDto
    {
    }

    class TransitionDtoProxy : ITransitionDtoProxy
    {
        IProjectController projectController_;
        Guid transitionId_;

        public TransitionDtoProxy(IProjectController projectController, Guid transitionId)
        {
            projectController_ = projectController;
            transitionId_ = transitionId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                TransitionDto transition = projectController_.GetTransition(transitionId_);

                return transition.Name;
            }
            set
            {
                try
                {
                    projectController_.SetTransitionName(transitionId_, value);
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
                return transitionId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                TransitionDto transition = projectController_.GetTransition(transitionId_);

                return transition.OwnerId;
            }
        }

        [CategoryAttribute("Data Source Settings"),
         DescriptionAttribute("The location of the python script file"),
         Editor(typeof(PythonScriptFilePathUiTypeEditor), typeof(UITypeEditor))]
        public string Script
        {
            get
            {
                ScriptDto script = projectController_.GetScriptByOwnerId(transitionId_);

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
                ScriptDto script = projectController_.GetScriptByOwnerId(transitionId_);

                projectController_.SetScriptPath(script.Id, value);
            }
        }
    }

    public interface ITransitionDtoProxy : IBaseDtoProxy
    {
    }
}
