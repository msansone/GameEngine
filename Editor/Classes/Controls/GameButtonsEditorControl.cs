using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    #region Delegates

    public delegate void GameButtonSelectionChangedHandler(object sender, GameButtonSelectionChangedEventArgs e);

    #endregion

    public partial class GameButtonsEditorControl : UserControl, IGameButtonsEditorControl
    {
        #region Events

        public event GameButtonSelectionChangedHandler GameButtonSelectionChanged;

        #endregion

        #region Constructors

        public GameButtonsEditorControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            // Entity root nodes.
            tvGameButtons.Nodes.Add("BUTTONROOT", "Buttons").Tag = new AssetMenuDto(cmnuGameButtonRoot, null);

            tvGameButtons.Nodes.Add("BUTTONGROUPROOT", "Button Groups").Tag = new AssetMenuDto(cmnuGameButtonGroupRoot, null);
            
            // Build the tree.
            ProjectDto project = projectController.GetProjectDto();

            foreach (GameButtonDto gameButton in project.GameButtons)
            {
                TreeNode gameButtonNode = addGameButtonToTree(gameButton);
            }

            foreach (GameButtonGroupDto gameButtonGroup in project.GameButtonGroups)
            {
                TreeNode gameButtonGroupNode = addGameButtonGroupToTree(gameButtonGroup);
            }
        }

        #region Private Variables

        IFiremelonEditorFactory firemelonEditorFactory_;

        #endregion

        IProjectController projectController_;

        #endregion

        #region Public Functions

        public void AddGroup()
        {
            addGameButtonGroup();
        }

        public void AddNew()
        {
            addGameButton();
        }

        public void Delete()
        {
            deleteGameButton();
        }

        public void DeleteGroup()
        {
            deleteGameButtonGroup();
        }

        #endregion

        #region Private Functions

        private void addGameButtonGroup()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int gameButtonGroupCount = project.GameButtonGroups.Count + 1; // tvAssets.Nodes["GAMEBUTTONROOT"].Nodes["BUTTONROOT"].Nodes.Count + 1;

            bool isNameValid = false;

            int counter = 0;

            string currentName = "New Game Button Group";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Game Button Group " + counter.ToString();
                }

                for (int i = 0; i < project.GameButtonGroups.Count; i++)
                {
                    if (currentName.ToUpper() == project.GameButtonGroups[i].Name.ToUpper())
                    {
                        isNameValid = false;

                        break;
                    }
                }

                counter++;
            }

            GameButtonGroupDto newGameButtonGroup = projectController_.AddGameButtonGroup(currentName);

            addGameButtonGroupToTree(newGameButtonGroup);
        }

        private TreeNode addGameButtonGroupToTree(GameButtonGroupDto gameButtonGroup)
        {
            IGameButtonGroupDtoProxy gameButtonGroupProxy = firemelonEditorFactory_.NewGameButtonGroupProxy(projectController_, gameButtonGroup.Id);

            // If this already has a node on the tree, ignore it.
            foreach (TreeNode node in tvGameButtons.Nodes["BUTTONGROUPROOT"].Nodes)
            {
                IGameButtonGroupDtoProxy nodeProxy = (IGameButtonGroupDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == gameButtonGroupProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode buttonGroupNode = tvGameButtons.Nodes["BUTTONGROUPROOT"].Nodes.Add("BUTTONGROUP", gameButtonGroup.Name);

            buttonGroupNode.Tag = new AssetMenuDto(cmnuGameButtonGroup, gameButtonGroupProxy);

            return buttonGroupNode;
        }

        private void addGameButton()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int gameButtonCount = project.GameButtons.Count + 1; // tvAssets.Nodes["GAMEBUTTONROOT"].Nodes["BUTTONROOT"].Nodes.Count + 1;

            bool isNameValid = false;

            int counter = 0;

            string currentName = "New Game Button";

            // Find the first sequentially available name.
            while (isNameValid == false)
            {
                isNameValid = true;

                // The current name that is being checked for collision.
                if (counter > 0)
                {
                    currentName = "New Game Button " + counter.ToString();
                }

                for (int i = 0; i < project.GameButtons.Count; i++)
                {
                    if (currentName.ToUpper() == project.GameButtons[i].Name.ToUpper())
                    {
                        isNameValid = false;

                        break;
                    }
                }

                counter++;
            }

            GameButtonDto newGameButton = projectController_.AddGameButton(currentName);

            addGameButtonToTree(newGameButton);
        }

        private TreeNode addGameButtonToTree(GameButtonDto gameButton)
        {
            IGameButtonDtoProxy gameButtonProxy = firemelonEditorFactory_.NewGameButtonProxy(projectController_, gameButton.Id);

            // If this already has a node on the tree, ignore it.
            foreach (TreeNode node in tvGameButtons.Nodes["BUTTONROOT"].Nodes)
            {
                IGameButtonDtoProxy nodeProxy = (IGameButtonDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == gameButtonProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode buttonNode = tvGameButtons.Nodes["BUTTONROOT"].Nodes.Add("BUTTON", gameButton.Name);

            buttonNode.Tag = new AssetMenuDto(cmnuGameButton, gameButtonProxy);

            return buttonNode;
        }

        private void deleteGameButton()
        {
            // Delete the selected game button. 
            if (MessageBox.Show("Delete Game Button?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                projectController_.DeleteGameButton(tvGameButtons.SelectedNode.Index);

                tvGameButtons.SelectedNode.Remove();
            }
        }

        private void deleteGameButtonGroup()
        {
            // Delete the selected game button group.
            if (MessageBox.Show("Delete Game Button Group?", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                projectController_.DeleteGameButtonGroup(tvGameButtons.SelectedNode.Index);

                tvGameButtons.SelectedNode.Remove();
            }
        }

        #endregion

        #region Event Handlers

        private void pgGameButtons_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":

                    // Update the tree view with the new name.
                    string newName = e.ChangedItem.Value.ToString();

                    tvGameButtons.SelectedNode.Text = newName;

                    // how to sort only child nodes? tvGameButtons.Nodes["BUTTONROOT"].Sor

                    break;
            }
        }

        private void tvGameButtons_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pgGameButtons.SelectedObject = null;

            switch (tvGameButtons.SelectedNode.Name)
            {
                case "BUTTON":

                    IGameButtonDtoProxy gameButtonProxy = (IGameButtonDtoProxy)((AssetMenuDto)tvGameButtons.SelectedNode.Tag).Asset;

                    pgGameButtons.SelectedObject = gameButtonProxy;

                    break;

                case "BUTTONGROUP":

                    IGameButtonGroupDtoProxy gameButtonGroupProxy = (IGameButtonGroupDtoProxy)((AssetMenuDto)tvGameButtons.SelectedNode.Tag).Asset;

                    pgGameButtons.SelectedObject = gameButtonGroupProxy;

                    break;
            }


            bool canAddGameButton = true;

            bool canAddGameButtonGroup = true;

            bool canDeleteGameButton = tvGameButtons.SelectedNode.Name == "BUTTON";

            bool canDeleteGameButtonGroup = tvGameButtons.SelectedNode.Name == "BUTTONGROUP";
            
            OnGameButtonSelectionChanged(new GameButtonSelectionChangedEventArgs(canAddGameButton, canAddGameButtonGroup, canDeleteGameButton, canDeleteGameButtonGroup));
        }

        #endregion


        #region Event Dispatchers

        protected virtual void OnGameButtonSelectionChanged(GameButtonSelectionChangedEventArgs e)
        {
            GameButtonSelectionChanged(this, e);
        }

        #endregion
    }

    #region Event Args

    public class GameButtonSelectionChangedEventArgs : System.EventArgs
    {
        #region Constructors

        public GameButtonSelectionChangedEventArgs(bool canAddGameButton,
                                                   bool canAddButtonGroup,
                                                   bool canDeleteGameButton,
                                                   bool canDeleteButtonGroup)
        {
            canAddGameButton_ = canAddGameButton;
            canDeleteGameButton_ = canDeleteGameButton;
            canAddButtonGroup_ = canAddButtonGroup;
            canDeleteButtonGroup_ = canDeleteButtonGroup;
        }

        #endregion

        #region Properties
        
        public bool CanAddGameButton
        {
            get { return canAddGameButton_; }
        }
        private bool canAddGameButton_;

        public bool CanAddButtonGroup
        {
            get { return canAddButtonGroup_; }
        }
        private bool canAddButtonGroup_;
        
        public bool CanDeleteGameButton
        {
            get { return canDeleteGameButton_; }
        }
        private bool canDeleteGameButton_;

        public bool CanDeleteButtonGroup
        {
            get { return canDeleteButtonGroup_; }
        }
        private bool canDeleteButtonGroup_;
        
        #endregion
    }

    #endregion
}