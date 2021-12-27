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
    public partial class ScriptsForm : Form, IScriptsForm
    {
        #region Constructors

        public ScriptsForm(IProjectController projectController, INameGenerator nameGenerator)
        {
            InitializeComponent();

            IFiremelonEditorFactory firemelonEditorFactory = new FiremelonEditorFactory();

            scriptsEditorControl_ = firemelonEditorFactory.NewScriptsEditorControl(projectController, nameGenerator, true);
            
            ((Control)scriptsEditorControl_).Dock = DockStyle.Fill;

            tpScriptEditor.Controls.Add(((Control)scriptsEditorControl_));

            scriptsManagerControl_ = firemelonEditorFactory.NewScriptManagerControl(projectController);

            ((Control)scriptsManagerControl_).Dock = DockStyle.Fill;

            tpScriptManager.Controls.Add(((Control)scriptsManagerControl_));
        }

        #endregion

        #region Private Variables

        IScriptsEditorControl scriptsEditorControl_;

        IScriptManagerControl scriptsManagerControl_;

        #endregion

        #region Public Functions

        new public void ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);
        }

        #endregion

        #region Event Handlers

        private void ScriptsForm_Shown(object sender, EventArgs e)
        {
        }
        
        private void ScriptsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            bool cancel = scriptsEditorControl_.PromptForSaveIfChanged();

            if (cancel == false)
            {
                this.Hide();

                scriptsEditorControl_.ResetUi();
            }           
        }

        #endregion
    }

    public interface IScriptsForm
    {
        // Derived from base.
        int Width { get; set; }
        int Height { get; set; }
        int Bottom { get; }
        int Left { get; set; }
        int Top { get; set; }
        bool TopLevel { get; set; }

        void Show(IWin32Window owner);
        void ShowDialog(IWin32Window owner);
        void Hide();
        void Close();
    }
}
