using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class UiWidgetDto : BaseDto
    {
    }

    public class UiWidgetDtoProxy : IUiWidgetDtoProxy
    {
        IExceptionHandler exceptionHandler_;
        private IProjectController projectController_;
        private Guid uiWidgetId_;

        public UiWidgetDtoProxy(IProjectController projectController, Guid uiWidgetId, IExceptionHandler exceptionHandler)
        {
            exceptionHandler_ = exceptionHandler;
            projectController_ = projectController;
            uiWidgetId_ = uiWidgetId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                UiWidgetDto uiWidget = projectController_.GetUiWidget(uiWidgetId_);

                return uiWidget.Name;
            }
            set
            {
                try
                {
                    projectController_.SetUiWidgetName(uiWidgetId_, value);
                }
                catch (InvalidNameException ex)
                {
                    exceptionHandler_.HandleException(ex, "Invalid Name");
                }
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return uiWidgetId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                UiWidgetDto uiWidget = projectController_.GetUiWidget(uiWidgetId_);

                return uiWidget.OwnerId;
            }
        }
    }
}
