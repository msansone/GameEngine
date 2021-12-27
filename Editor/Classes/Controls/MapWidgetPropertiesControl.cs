using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class MapWidgetPropertiesControl : UserControl, IMapWidgetPropertiesControl
    {
        public MapWidgetPropertiesControl(IProjectController projectController)
        {
            InitializeComponent();


            projectController.RefreshView += new RefreshViewHandler(this.MapWidgetPropertiesControl_RefreshView);
        }

        #region Events

        public event PropertyValueChangedEventHandler PropertyValueChanged;

        #endregion
        
        #region Properties

        public object SelectedObject
        {
            get
            {
                return pgProperties.SelectedObject;
            }
            set
            {
                pgProperties.SelectedObject = value;
            }
        }

        public object[] SelectedObjects
        {
            get
            {
                return pgProperties.SelectedObjects;
            }
            set
            {
                pgProperties.SelectedObjects = value;
            }
        }

        #endregion

        #region Public Functions

        public override void Refresh()
        {
            pgProperties.Refresh();
        }

        #endregion

        #region Event Handlers

        private void pgProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyValueChanged?.Invoke(s, e);
        }

        private void MapWidgetPropertiesControl_RefreshView(object sender, RefreshViewEventArgs e)
        {
            pgProperties.Refresh();
        }

        #endregion

    }
}
