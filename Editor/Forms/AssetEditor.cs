using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class AssetEditor : Form, IAssetEditor
    {
        #region Private Variables

        private IAnimationsEditorControl animationsEditorControl_;

        private IAudioAssetsEditorControl audioAssetsEditorControl_;
        
        private IConstantsEditorControl constantsEditorControl_;

        private IEntitiesEditorControl entitiesEditorControl_;

        private IExceptionHandler exceptionHandler_;

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private IGameButtonsEditorControl gameButtonEditorControl_;

        private ILoadingScreensEditorControl loadingScreensEditorControl_;

        private INameGenerator nameGenerator_;

        private INameValidator nameValidator_;

        private IParticleEmittersEditorControl particleEmittersEditorControl_;

        private IProjectController projectController_;
        
        private IScriptsEditorControl scriptsEditorControl_;

        private ISpriteSheetsEditorControl spriteSheetsEditorControl_;

        private ITileSheetsEditorControl tileSheetsEditorControl_;

        private ITransitionsEditorControl transitionsEditorControl_;

        private IUiEditorControl uiEditorControl_;

        private IAssetsEditorControl visibleAssetEditor_;

        #endregion

        public AssetEditor()
        {
            InitializeComponent();
        }

        public void ShowDialog(IWin32Window owner, IProjectController projectController, INameValidator nameValidator, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            exceptionHandler_ = exceptionHandler;

            nameGenerator_ = nameGenerator;

            nameValidator_ = nameValidator;
            
            projectController_ = projectController;

            projectController_.ProjectStateChanged += new ProjectStateChangeHandler(this.AssetEditor_ProjectStateChanged);

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            createEditorControls();

            base.ShowDialog(owner);
        }

        private void createEditorControls()
        {
            // Tile sheets
            tileSheetsEditorControl_ = firemelonEditorFactory_.NewTileSheetsEditorControl(projectController_);

            tileSheetsEditorControl_.TileSheetSelectionChanged += new TileSheetSelectionChangedHandler(this.AssetsEditor_TileSheetEditorSelectionChanged);

            rtTilesheets.Tag = tileSheetsEditorControl_;

            ((Control)tileSheetsEditorControl_).Dock = DockStyle.Fill;

            ((Control)tileSheetsEditorControl_).Visible = true;

            visibleAssetEditor_ = tileSheetsEditorControl_;

            pnControls.Controls.Add((Control)tileSheetsEditorControl_);


            // Sprite Sheets
            spriteSheetsEditorControl_ = firemelonEditorFactory_.NewSpriteSheetsEditorControl(projectController_);

            rtSpriteSheets.Tag = spriteSheetsEditorControl_;

            ((Control)spriteSheetsEditorControl_).Dock = DockStyle.Fill;

            ((Control)spriteSheetsEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)spriteSheetsEditorControl_);


            // Audio Assets
            audioAssetsEditorControl_ = firemelonEditorFactory_.NewAudioAssetsEditorControl(projectController_);

            rtAudioAssets.Tag = audioAssetsEditorControl_;

            ((Control)audioAssetsEditorControl_).Dock = DockStyle.Fill;

            ((Control)audioAssetsEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)audioAssetsEditorControl_);


            // Animations
            animationsEditorControl_ = firemelonEditorFactory_.NewAnimationsEditorControl(projectController_);

            rtAnimations.Tag = animationsEditorControl_;

            animationsEditorControl_.AnimationSelectionChanged += new AnimationSelectionChangedHandler(this.AssetsEditor_AnimationSelectionChanged);

            ((Control)animationsEditorControl_).Dock = DockStyle.Fill;

            ((Control)animationsEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)animationsEditorControl_);


            // Entites
            entitiesEditorControl_ = firemelonEditorFactory_.NewEntitiesEditorControl(projectController_, nameGenerator_, exceptionHandler_);

            rtEntities.Tag = entitiesEditorControl_;
            
            entitiesEditorControl_.EntitySelectionChanged += new EntitySelectionChangedHandler(this.AssetsEditor_EntitySelectionChanged);

            ((Control)entitiesEditorControl_).Dock = DockStyle.Fill;

            ((Control)entitiesEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)entitiesEditorControl_);


            // Partile Emitters
            particleEmittersEditorControl_ = firemelonEditorFactory_.NewParticleEmittersEditorControl(projectController_, nameGenerator_, exceptionHandler_);

            rtParticleEmitters.Tag = particleEmittersEditorControl_;

            ((Control)particleEmittersEditorControl_).Dock = DockStyle.Fill;

            ((Control)particleEmittersEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)particleEmittersEditorControl_);


            // Scripts
            constantsEditorControl_ = firemelonEditorFactory_.NewConstantsEditorControl(projectController_);

            rtConstants.Tag = constantsEditorControl_;

            ((Control)constantsEditorControl_).Dock = DockStyle.Fill;

            ((Control)constantsEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)constantsEditorControl_);


            // Buttons
            gameButtonEditorControl_ = firemelonEditorFactory_.NewGameButtonsEditorControl(projectController_);

            rtButtons.Tag = gameButtonEditorControl_;

            gameButtonEditorControl_.GameButtonSelectionChanged += new GameButtonSelectionChangedHandler(this.AssetsEditor_GameButtonSelectionChanged);

            ((Control)gameButtonEditorControl_).Dock = DockStyle.Fill;

            ((Control)gameButtonEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)gameButtonEditorControl_);


            // Loading Screens
            loadingScreensEditorControl_ = firemelonEditorFactory_.NewLoadingScreensEditorControl(projectController_, nameGenerator_);

            rtLoadingScreens.Tag = loadingScreensEditorControl_;

            ((Control)loadingScreensEditorControl_).Dock = DockStyle.Fill;

            ((Control)loadingScreensEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)loadingScreensEditorControl_);


            // Transitions
            transitionsEditorControl_ = firemelonEditorFactory_.NewTransitionsEditorControl(projectController_, nameGenerator_);

            rtTransitions.Tag = transitionsEditorControl_;

            ((Control)transitionsEditorControl_).Dock = DockStyle.Fill;

            ((Control)transitionsEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)transitionsEditorControl_);


            // Ui
            uiEditorControl_ = firemelonEditorFactory_.NewUiEditorControl(projectController_, nameGenerator_, exceptionHandler_);

            rtUi.Tag = uiEditorControl_;

            ((Control)uiEditorControl_).Dock = DockStyle.Fill;

            ((Control)uiEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)uiEditorControl_);


            // Scripts
            scriptsEditorControl_ = firemelonEditorFactory_.NewScriptsEditorControl(projectController_, nameGenerator_, false);

            rtScripts.Tag = scriptsEditorControl_;

            ((Control)scriptsEditorControl_).Dock = DockStyle.Fill;

            ((Control)scriptsEditorControl_).Visible = false;

            pnControls.Controls.Add((Control)scriptsEditorControl_);
        }

        #region Event Handlers

        private void AssetsEditor_AnimationSelectionChanged(object sender, AnimationSelectionChangedEventArgs e)
        {
            rbtnAddFrames.Enabled = e.CanAddFrame;
            rbtnAddAnimationFrameHitbox.Enabled = e.CanAddHitbox;
            rbtnAddFrameTrigger.Enabled = e.CanAddFrameTrigger;
            rbtnAddActionPoint.Enabled = e.CanAddActionPoint;

            rbtnDeleteFrame.Enabled = e.CanDeleteFrame;
            rbtnDeleteFrameHitbox.Enabled = e.CanDeleteHitbox;
        }

        private void AssetsEditor_GameButtonSelectionChanged(object sender, GameButtonSelectionChangedEventArgs e)
        {
            rbtnAddGameButton.Enabled = e.CanAddGameButton;

            rbtnAddGameButtonGroup.Enabled = e.CanAddButtonGroup;
            
            rbtnDeleteButton.Enabled = e.CanDeleteGameButton;

            rbtnDeleteButtonGroup.Enabled = e.CanDeleteButtonGroup;
        }

        private void AssetsEditor_EntitySelectionChanged(object sender, EntitySelectionChangedEventArgs e)
        {
            if (e.EntityType == EntityType.None)
            {
                rbtnAddNewEntity.Enabled = false;
            }
            else
            {
                rbtnAddNewEntity.Enabled = true;
            }

            rbtnAddState.Enabled = e.CanAddStates;
            
            rbtnAddNewEntityProperty.Enabled = e.CanAddProperties;

            rbtnAddAnimationSlot.Enabled = e.CanAddAnimationSlots;

            rbtnAddEntityHitbox.Enabled = e.CanAddHitboxes;

            rbtnDeleteEntity.Enabled = e.CanDeleteEntity;

            rbtnDeleteState.Enabled = e.CanDeleteState;

            rbtnDeleteAnimationSlot.Enabled = e.CanDeleteAnimationSlot;

            rbtnDeleteEntityHitbox.Enabled = e.CanDeleteHitbox;
        }

        private void AssetEditor_ProjectStateChanged(object sender, ProjectStateChangedEventArgs e)
        {
            if (projectController_.GetUndoStackSize() > 0)
            {
                rbtnUndo.Enabled = true;
            }
            else
            {
                rbtnUndo.Enabled = false;
            }

            if (projectController_.GetRedoStackSize() > 0)
            {
                rbtnRedo.Enabled = true;
            }
            else
            {
                rbtnRedo.Enabled = false;
            }
        }

        private void AssetsEditor_TileSheetEditorSelectionChanged(object sender, TileSheetSelectionChangedEventArgs e)
        {
            rbtnAddTileObject.Enabled = e.CanAddObject;
            rbtnDeleteTileObject.Enabled = e.CanDeleteObject;
            rbtnDeleteSceneryAnimation.Enabled = e.CanDeleteAnimation;
        }
        
        private void rbnToolbar_ActiveTabChanged(object sender, EventArgs e)
        {
            IAssetsEditorControl assetEditor = (IAssetsEditorControl)rbnToolbar.ActiveTab.Tag;

            if (assetEditor != null)
            {
                System.Diagnostics.Debug.Print("Active tab changed to " + ((Control)assetEditor).Name);

                if (visibleAssetEditor_ != null)
                {
                    ((Control)visibleAssetEditor_).Visible = false;
                }

                visibleAssetEditor_ = assetEditor;

                if (visibleAssetEditor_ != null)
                {
                    System.Diagnostics.Debug.Print("Setting control " + ((Control)assetEditor).Name + " to visible");
                    ((Control)visibleAssetEditor_).Visible = true;
                }
                else
                {
                    MessageBox.Show("No asset editor has been assigned to the selected tab.");
                }
            }
        }

        private void rbtnAddActionPoint_Click(object sender, EventArgs e)
        {
            animationsEditorControl_.AddActionPoint();
        }

        private void rbtnAddAnimation_Click(object sender, EventArgs e)
        {
            animationsEditorControl_.AddNew();
        }

        private void rbtnAddAnimationFrameHitbox_Click(object sender, EventArgs e)
        {
            animationsEditorControl_.AddHitbox();
        }

        private void rbtnAddAnimationGroup_Click(object sender, EventArgs e)
        {
            animationsEditorControl_.AddGroup();
        }

        private void rbtnAddAnimationSlot_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.AddAnimationSlot();
        }

        private void rbtnAddAudioAsset_Click(object sender, EventArgs e)
        {
            audioAssetsEditorControl_.AddNew();
        }

        private void rbtnAddEntityHitbox_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.AddHitbox();
        }

        private void rbtnAddFrames_Click(object sender, EventArgs e)
        {
            animationsEditorControl_.AddFrame();
        }

        private void rbtnAddFrameTrigger_Click(object sender, EventArgs e)
        {
            animationsEditorControl_.AddFrameTrigger();
        }

        private void rbtnAddGameButton_Click(object sender, EventArgs e)
        {
            gameButtonEditorControl_.AddNew();
        }

        private void rbtnAddGameButtonGroup_Click(object sender, EventArgs e)
        {
            gameButtonEditorControl_.AddGroup();
        }

        private void rbtnAddHitboxIdentity_Click(object sender, EventArgs e)
        {
            constantsEditorControl_.AddHitboxIdentity();
        }

        private void rbtnAddLoadingScreen_Click(object sender, EventArgs e)
        {
            loadingScreensEditorControl_.AddNew();
        }
        
        private void rbtnAddNewParticle_Click(object sender, EventArgs e)
        {
            particleEmittersEditorControl_.AddNewParticle();
        }

        private void rbtnAddParticleEmitter_Click(object sender, EventArgs e)
        {
            particleEmittersEditorControl_.AddNew();
        }

        private void rbtnAddNewEntity_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.AddNew();
        }

        private void rbtnAddNewEntityProperty_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.AddProperty();
        }

        private void rbtnAddNewTileSheet_Click(object sender, EventArgs e)
        {
            tileSheetsEditorControl_.AddNew();
        }

        private void rbtnAddScript_Click(object sender, EventArgs e)
        {
            scriptsEditorControl_.AddNew();
        }

        private void rbtnAddSceneryAnimation_Click(object sender, EventArgs e)
        {
            tileSheetsEditorControl_.AddAnimation();
        }

        private void rbtnAddSpriteSheet_Click(object sender, EventArgs e)
        {
            spriteSheetsEditorControl_.AddNew();
        }

        private void rbtnAddState_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.AddState();
        }

        private void rbtnAddTileObject_Click(object sender, EventArgs e)
        {
            tileSheetsEditorControl_.AddTileObject();
        }

        private void rbtnAddTransition_Click(object sender, EventArgs e)
        {
            transitionsEditorControl_.AddNew();
        }
        
        private void rbtnAddTriggerSignal_Click(object sender, EventArgs e)
        {
            constantsEditorControl_.AddTriggerSignal();
        }

        private void rbtnAddUiWidget_Click(object sender, EventArgs e)
        {
            uiEditorControl_.AddNew();
        }

        private void rbtnDeleteAnimationSlot_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.DeleteAnimationSlot();
        }

        private void rbtnDeleteButton_Click(object sender, EventArgs e)
        {
            gameButtonEditorControl_.Delete();
        }

        private void rbtnDeleteButtonGroup_Click(object sender, EventArgs e)
        {
            gameButtonEditorControl_.DeleteGroup();
        }

        private void rbtnDeleteEntity_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.Delete();
        }

        private void rbtnDeleteFrame_Click(object sender, EventArgs e)
        {
            animationsEditorControl_.DeleteFrame();
        }

        private void rbtnDeleteFrameHitbox_Click(object sender, EventArgs e)
        {
            animationsEditorControl_.DeleteHitbox();         
        }

        private void rbtnDeleteEntityHitbox_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.DeleteHitbox();
        }

        private void rbtnDeleteHitboxIdentity_Click(object sender, EventArgs e)
        {
            constantsEditorControl_.DeleteHitboxIdentity();
        }

        private void rbtnDeleteSceneryAnimation_Click(object sender, EventArgs e)
        {
            tileSheetsEditorControl_.DeleteAnimation();
        }

        private void rbtnDeleteState_Click(object sender, EventArgs e)
        {
            entitiesEditorControl_.DeleteState();
        }

        private void rbtnDeleteTileObject_Click(object sender, EventArgs e)
        {
            tileSheetsEditorControl_.DeleteTileObject();
        }

        private void rbtnDeleteTriggerSignal_Click(object sender, EventArgs e)
        {
            constantsEditorControl_.DeleteTriggerSignal();
        }

        private void rbtnRedo_Click(object sender, EventArgs e)
        {
            projectController_.Redo();
        }
        
        private void rbtnUndo_Click(object sender, EventArgs e)
        {
            projectController_.Undo();
        }

        #endregion
    }
}