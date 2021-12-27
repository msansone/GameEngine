using System.Windows.Forms;

namespace FiremelonEditor2
{
    public interface IMapWidgetPropertiesControl
    {
        event PropertyValueChangedEventHandler PropertyValueChanged;

        object SelectedObject { get;  set; }
        object[] SelectedObjects { get; set; }

        void Refresh();
    }
}
