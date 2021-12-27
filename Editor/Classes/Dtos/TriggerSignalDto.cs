using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class TriggerSignalDto : BaseDto
    {
    }

    public class TriggerSignalDtoProxy : ITriggerSignalDtoProxy
    {
        private IProjectController projectController_;
        private Guid triggerSignalId_;

        public TriggerSignalDtoProxy(IProjectController projectController, Guid triggerSignalId)
        {
            projectController_ = projectController;
            triggerSignalId_ = triggerSignalId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                TriggerSignalDto triggerSignal = projectController_.GetTriggerSignal(triggerSignalId_);

                return triggerSignal.Name;
            }
            set
            {
                bool isValid = true;

                TriggerSignalDto triggerSignal = projectController_.GetTriggerSignal(triggerSignalId_);

                // Validate the name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Trigger Signal name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    for (int i = 0; i < project.TriggerSignals.Count; i++)
                    {
                        TriggerSignalDto currentTriggerSignal = project.TriggerSignals[i];

                        if (value.ToUpper() == currentTriggerSignal.Name.ToUpper() && triggerSignalId_ != currentTriggerSignal.Id)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Trigger Signal name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetTriggerSignalName(triggerSignalId_, value);
                }
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return triggerSignalId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                TriggerSignalDto triggerSignal = projectController_.GetTriggerSignal(triggerSignalId_);

                return triggerSignal.OwnerId;
            }
        }
    }
}
