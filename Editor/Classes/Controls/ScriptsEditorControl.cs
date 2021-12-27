using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using System.Collections.Generic;
    using System.IO;
    using ProjectDto = ProjectDto_2_2;

    public partial class ScriptsEditorControl : UserControl, IScriptsEditorControl
    {
        #region Constructors

        public ScriptsEditorControl(IProjectController projectController, INameGenerator nameGenerator, bool showAllScripts)
        {
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            nameGenerator_ = nameGenerator;

            projectController_ = projectController;

            projectController_.ProjectCreated += new ProjectCreateHandler(this.ScriptsEditorControl_ProjectCreated);

            showAllScripts_ = showAllScripts;

            if (showAllScripts_ == true)
            {
                scScriptsList.Panel2Collapsed = true;
            }

            populateScriptList();

            pythonScriptEditorControl_ = firemelonEditorFactory_.NewPythonScriptEditorControl(projectController);

            ((Control)pythonScriptEditorControl_).Dock = DockStyle.Fill;

            scScripts.Panel2.Controls.Add((Control)pythonScriptEditorControl_);            
        }

        #endregion
        
        #region Private Variables

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private INameGenerator nameGenerator_;

        private IProjectController projectController_;

        private IPythonScriptEditorControl pythonScriptEditorControl_;

        private int selectedListIndex_ = 0;

        private bool showAllScripts_ = false;

        private bool skipNextSelectedIndexChangedEvent_ = false;

        #endregion

        #region Public Functions

        public void AddNew()
        {
            ProjectDto project = projectController_.GetProjectDto();

            string name = getNextAvailableName("NewScript");

            ScriptDto newScript = projectController_.AddScript(name, ScriptType.Script);

            addScriptToListBox(newScript);
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public bool PromptForSaveIfChanged()
        {
            DialogResult result = DialogResult.None;

            bool cancel = false;

            if (pythonScriptEditorControl_.ChangesMade == true)
            {
                result = MessageBox.Show("Script contains unsaved changes. Do you want to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);
                
                if (result == DialogResult.Yes)
                {
                    pythonScriptEditorControl_.Save();
                }
                else if (result == DialogResult.Cancel)
                {
                    cancel = true;
                }
            }

            return cancel;
        }

        public void ResetUi()
        {
            // Clear the text.
            pythonScriptEditorControl_.Script = null;

            // Clear the listbox selection.
            lbxScripts.SelectedIndex = -1;
        }

        #endregion

        #region Private Functions

        private void addScriptToListBox(ScriptDto script)
        {
            lbxScripts.Items.Add(script);
        }

        private string getNextAvailableName(string baseName)
        {
            bool isNameValid = false;
            int counter = 0;
            string currentName = baseName;

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = baseName + counter.ToString();
                }

                isNameValid = !nameGenerator_.IsAssetNameInUse(Guid.Empty, projectController_.GetProjectDto(), currentName);

                counter++;
            }

            return currentName.Trim();
        }

        private void populateScriptList()
        {
            ProjectDto project = projectController_.GetProjectDto();

            if (project != null)
            {
                ProjectUiStateDto uiState = projectController_.GetUiState();

                int selectedRoomIndex = uiState.SelectedRoomIndex;

                lbxScripts.Items.Clear();

                // Build the tree.
                foreach (KeyValuePair<Guid, ScriptDto> kvp in project.Scripts)
                {
                    // Additional scripts are stored with their id as the key.
                    if (kvp.Key == kvp.Value.Id || showAllScripts_ == true)
                    {
                        lbxScripts.Items.Add(kvp.Value);
                    }
                }
            }
        }
        
        #endregion

        #region Event Handlers

        private void lbxScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (skipNextSelectedIndexChangedEvent_ == true)
            {
                skipNextSelectedIndexChangedEvent_ = false;
            }
            else
            {
                DialogResult result = DialogResult.None;

                bool proceed = true;

                if (pythonScriptEditorControl_.ChangesMade == true)
                {
                    result = MessageBox.Show("Script contains unsaved changes. Do you want to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);
                }

                if (result == DialogResult.Yes)
                {
                    pythonScriptEditorControl_.Save();
                }
                else if (result == DialogResult.Cancel)
                {
                    // Setting the selected item back will trigger this event again. Set a flag to skip it.
                    skipNextSelectedIndexChangedEvent_ = true;

                    lbxScripts.SelectedIndex = selectedListIndex_;
                    
                    proceed = false;
                }

                if (proceed == true)
                {
                    if (lbxScripts.SelectedIndex >= 0 && lbxScripts.SelectedIndex < lbxScripts.Items.Count)
                    {
                        ScriptDto script = (ScriptDto)lbxScripts.Items[lbxScripts.SelectedIndex];

                        IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewNamedScriptProxy(projectController_, script.Id);

                        pgScripts.SelectedObject = scriptProxy;

                        pythonScriptEditorControl_.Script = scriptProxy;
                        
                        selectedListIndex_ = lbxScripts.SelectedIndex;
                    }
                }
            }
        }

        private void pgScripts_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":
                    // Update the list with the new name.
                    
                    lbxScripts.Items[lbxScripts.SelectedIndex] = lbxScripts.Items[lbxScripts.SelectedIndex];

                    lbxScripts.Sorted = false;
                    lbxScripts.Sorted = true;

                    break;
            }
        }

        public void ScriptsEditorControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
        {
            if (e.NewProject == true)
            {
                populateScriptList();
            }
        }

        #endregion
    }
}
