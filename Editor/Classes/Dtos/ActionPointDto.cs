using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class ActionPointDto : BaseDto
    {
        private Point2D position_ = new Point2D(0, 0);
        public Point2D Position
        {
            get
            {
                return position_;
            }
            set
            {
                position_ = value;
            }
        }
    }

    public class ActionPointDtoProxy : IActionPointDtoProxy
    {
        IProjectController projectController_;
        Guid actionPointId_;

        public ActionPointDtoProxy(IProjectController projectController, Guid actionPointId)
        {
            projectController_ = projectController;
            actionPointId_ = actionPointId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                ActionPointDto actionPoint = projectController_.GetActionPoint(actionPointId_);

                return actionPoint.Name;
            }
            set
            {
                try
                {
                    projectController_.SetActionPointName(actionPointId_, value);
                }
                catch (InvalidNameException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        [CategoryAttribute("Position"),
         DescriptionAttribute("Action Point Position")]
        public int Left
        {
            get
            {
                ActionPointDto actionPoint = projectController_.GetActionPoint(actionPointId_);

                return actionPoint.Position.X;
            }
            set
            {
                projectController_.SetActionPointPositionLeft(actionPointId_, value);
            }
        }

        [CategoryAttribute("Position"),
         DescriptionAttribute("Action Point Position")]
        public int Top
        {
            get
            {
                ActionPointDto actionPoint = projectController_.GetActionPoint(actionPointId_);

                return actionPoint.Position.Y;
            }
            set
            {
                projectController_.SetActionPointPositionTop(actionPointId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return actionPointId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                ActionPointDto actionPoint = projectController_.GetActionPoint(actionPointId_);

                return actionPoint.OwnerId;
            }
        }
    }
}
