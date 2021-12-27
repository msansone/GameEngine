using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class TransitionsEditorControl : UserControl, ITransitionsEditorControl
    {
        private IFiremelonEditorFactory firemelonEditorFactory_;
        
        private IProjectController projectController_;

        private INameGenerator nameGenerator_;

        public TransitionsEditorControl(IProjectController projectController, INameGenerator nameGenerator)
        {
            InitializeComponent();

            projectController_ = projectController;
            
            firemelonEditorFactory_ = new FiremelonEditorFactory();
            
            nameGenerator_ = nameGenerator;
        }

        public void AddNew()
        {
            addTransition();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        #region Private Functions

        private void addTransition()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int transitionCount = project.Transitions.Count + 1;

            string name = nameGenerator_.GetNextAvailableAssetName("New Transition", project);

            TransitionDto newTransition = projectController_.AddTransition(name);
            
            lbxTransitions.Items.Add(newTransition.Name);
        }

        #endregion

        private void lbxTransitions_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbxTransitions.SelectedIndex >= 0 && lbxTransitions.SelectedIndex < lbxTransitions.Items.Count)
            {
                TransitionDto transition = projectController_.GetTransitionByName(lbxTransitions.Items[lbxTransitions.SelectedIndex].ToString());

                ITransitionDtoProxy transitionProxy = firemelonEditorFactory_.NewTransitionProxy(projectController_, transition.Id);

                pgTransitions.SelectedObject = transitionProxy;
            }
        }
    }
}
