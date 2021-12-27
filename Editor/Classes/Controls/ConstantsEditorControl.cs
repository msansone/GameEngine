using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class ConstantsEditorControl : UserControl, IConstantsEditorControl
    {
        #region Constructors

        public ConstantsEditorControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            projectController_.ProjectCreated += new ProjectCreateHandler(this.ConstantsEditorControl_ProjectCreated);

            firemelonEditorFactory_ = new FiremelonEditorFactory();
            
            buildTreeView();
        }

        #endregion

        #region Private Variables

        IFiremelonEditorFactory firemelonEditorFactory_;

        private IProjectController projectController_;

        #endregion
        
        #region Public Functions

        public void AddHitboxIdentity()
        {
            addHitboxIdentity();
        }

        public void AddNew()
        {
        }

        public void AddTriggerSignal()
        {
            addTriggerSignal();
        }

        public void Delete()
        {
        }

        public void DeleteHitboxIdentity()
        {
            throw new NotImplementedException();
        }

        public void DeleteTriggerSignal()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Functions

        private void addHitboxIdentity()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int hitboxIdentityCount = project.HitboxIdentities.Count + 1;

            bool isNameValid = false;

            int counter = 0;

            string currentName = "New Hitbox Identity";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Hitbox Identity " + counter.ToString();
                }

                for (int i = 0; i < project.HitboxIdentities.Count; i++)
                {
                    if (currentName.ToUpper() == project.HitboxIdentities[i].Name.ToUpper())
                    {
                        isNameValid = false;

                        break;
                    }
                }

                counter++;
            }

            HitboxIdentityDto newHitboxIdentity = projectController_.AddHitboxIdentity(currentName);

            addHitboxIdentityToTree(newHitboxIdentity);
        }

        private TreeNode addHitboxIdentityToTree(HitboxIdentityDto hitboxIdentity)
        {
            IHitboxIdentityDtoProxy hitboxIdentityProxy = firemelonEditorFactory_.NewHitboxIdentityProxy(projectController_, hitboxIdentity.Id);

            // If this already has a node on the tree, ignore it.
            foreach (TreeNode node in tvConstants.Nodes["HITBOXIDENTITYROOT"].Nodes)
            {
                IHitboxIdentityDtoProxy nodeProxy = (IHitboxIdentityDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == hitboxIdentity.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode hitboxIdentityNode = tvConstants.Nodes["HITBOXIDENTITYROOT"].Nodes.Add("HITBOXIDENTITY", hitboxIdentity.Name);

            hitboxIdentityNode.Tag = new AssetMenuDto(cmnuHitboxIdentity, hitboxIdentityProxy);

            return hitboxIdentityNode;
        }
        
        private void addTriggerSignal()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int triggerSignalCount = project.TriggerSignals.Count + 1;

            bool isNameValid = false;

            int counter = 0;

            string currentName = "New Trigger Signal";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Trigger Signal " + counter.ToString();
                }

                for (int i = 0; i < project.TriggerSignals.Count; i++)
                {
                    if (currentName.ToUpper() == project.TriggerSignals[i].Name.ToUpper())
                    {
                        isNameValid = false;

                        break;
                    }
                }

                counter++;
            }

            TriggerSignalDto newTriggerSignal = projectController_.AddTriggerSignal(currentName);

            addTriggerSignalToTree(newTriggerSignal);
        }

        private TreeNode addTriggerSignalToTree(TriggerSignalDto triggerSignal)
        {
            ITriggerSignalDtoProxy triggerSignalProxy = firemelonEditorFactory_.NewTriggerSignalProxy(projectController_, triggerSignal.Id);

            // If this already has a node on the tree, ignore it.
            foreach (TreeNode node in tvConstants.Nodes["TRIGGERSIGNALROOT"].Nodes)
            {
                ITriggerSignalDtoProxy nodeProxy = (ITriggerSignalDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == triggerSignalProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode triggerSignalNode = tvConstants.Nodes["TRIGGERSIGNALROOT"].Nodes.Add("TRIGGERSIGNAL", triggerSignal.Name);

            triggerSignalNode.Tag = new AssetMenuDto(cmnuTriggerSignal, triggerSignalProxy);

            return triggerSignalNode;
        }

        
        private void buildTreeView()
        {
            ProjectDto project = projectController_.GetProjectDto();

            tvConstants.Nodes.Clear();

            tvConstants.Nodes.Add("HITBOXIDENTITYROOT", "Hitbox Identities").Tag = new AssetMenuDto(cmnuHitboxIdentityRoot, null);

            tvConstants.Nodes.Add("TRIGGERSIGNALROOT", "Trigger Signals").Tag = new AssetMenuDto(cmnuTriggerSignalRoot, null);

            TreeNode tempNode;

            //tempNode = tvAnimation.Nodes.Add("FRAMEROOT", "Animation Frames");
            
            //tempNode.Tag = new AssetMenuDto(cmnuAnimationRoot, animationProxy_);
            
            foreach (HitboxIdentityDto hitboxIdentity in project.HitboxIdentities)
            {
                tempNode = tvConstants.Nodes["HITBOXIDENTITYROOT"].Nodes.Add("HITBOXIDENTITY", hitboxIdentity.Name);

                IHitboxIdentityDtoProxy hitboxIdentityProxy = firemelonEditorFactory_.NewHitboxIdentityProxy(projectController_, hitboxIdentity.Id);

                tempNode.Tag = new AssetMenuDto(cmnuHitboxIdentity, hitboxIdentityProxy);
            }

            foreach (TriggerSignalDto triggerSignal in project.TriggerSignals)
            {
                tempNode = tvConstants.Nodes["TRIGGERSIGNALROOT"].Nodes.Add("TRIGGERSIGNAL", triggerSignal.Name);

                ITriggerSignalDtoProxy triggerSignalProxy = firemelonEditorFactory_.NewTriggerSignalProxy(projectController_, triggerSignal.Id);

                tempNode.Tag = new AssetMenuDto(cmnuTriggerSignal, triggerSignalProxy);
            }
        }

        #endregion

        #region Event Handlers

        public void ConstantsEditorControl_ProjectCreated(object sender, ProjectCreatedEventArgs e)
        {
            ProjectDto project = projectController_.GetProjectDto();
            ProjectUiStateDto uiState = projectController_.GetUiState();

            int selectedRoomIndex = uiState.SelectedRoomIndex;

            if (e.NewProject == true)
            {
                buildTreeView();                
            }
        }

        private void pgConstants_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":

                    // Update the tree view with the new name.
                    string newName = e.ChangedItem.Value.ToString();

                    tvConstants.SelectedNode.Text = newName;
                    
                    break;
            }
        }

        private void tvConstants_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pgConstants.SelectedObject = null;

            switch (tvConstants.SelectedNode.Name)
            {
                case "HITBOXIDENTITY":

                    IHitboxIdentityDtoProxy hitboxIdentityProxy = (IHitboxIdentityDtoProxy)((AssetMenuDto)tvConstants.SelectedNode.Tag).Asset;

                    pgConstants.SelectedObject = hitboxIdentityProxy;

                    break;

                case "TRIGGERSIGNAL":

                    ITriggerSignalDtoProxy triggerSignalProxy = (ITriggerSignalDtoProxy)((AssetMenuDto)tvConstants.SelectedNode.Tag).Asset;

                    pgConstants.SelectedObject = triggerSignalProxy;

                    break;
            }
        }
        
        #endregion
    }
}
