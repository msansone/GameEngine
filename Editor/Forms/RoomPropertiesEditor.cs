using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class RoomPropertiesEditor : Form, IRoomPropertiesEditor
    {
        private IFiremelonEditorFactory firemelonEditorFactory_;
        private IProjectController projectController_;

        public RoomPropertiesEditor()
        {
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();
        }

        public void ShowDialog(IWin32Window owner, IProjectController projectController, Guid roomId)
        {
            projectController_ = projectController;

            IRoomDtoProxy roomProxy = firemelonEditorFactory_.NewRoomProxy(projectController_, roomId);
            
            pgRoom.SelectedObject = roomProxy;

            base.ShowDialog(owner);
        }

    }
}
