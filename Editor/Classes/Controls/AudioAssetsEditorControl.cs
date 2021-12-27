using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class AudioAssetsEditorControl : UserControl, IAudioAssetsEditorControl
    {
        #region Constructors

        public AudioAssetsEditorControl(IProjectController projectController)
        {
            InitializeComponent();

            projectController_ = projectController;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            ProjectDto project = projectController.GetProjectDto();

            // Initialize audio assets.
            foreach (AudioAssetDto audioAsset in project.AudioAssets)
            {
                lbxAudioAssets.Items.Add(audioAsset.Name);
            }

            audioPlayerControl_ = firemelonEditorFactory_.NewAudioPlayerControl();

            Control audioPlayerControl = (Control)audioPlayerControl_;

            audioPlayerControl.Dock = DockStyle.Fill;

            scAudioAssets.Panel2.Controls.Add(audioPlayerControl);
        }

        #endregion

        #region Private Variables

        IFiremelonEditorFactory firemelonEditorFactory_;

        IProjectController projectController_;

        IAudioPlayerControl audioPlayerControl_;

        #endregion

        #region Public Functions
        
        public void AddNew()
        {
            addNewAudio();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Functions

        private void addNewAudio()
        {
            ProjectDto project = projectController_.GetProjectDto();

            OpenFileDialog openAudioAsset = new OpenFileDialog();

            openAudioAsset.CheckFileExists = true;
            openAudioAsset.CheckPathExists = true;
            openAudioAsset.DefaultExt = "ogg";
            openAudioAsset.Filter = "OGG Files|*.ogg";
            openAudioAsset.FileName = string.Empty;
            openAudioAsset.Multiselect = false;
            openAudioAsset.RestoreDirectory = true;

            if (openAudioAsset.ShowDialog() == DialogResult.OK)
            {
                int audioCount = project.AudioAssets.Count + 1;

                bool isNameValid = false;
                int counter = 0;
                string currentName = "New Audio";

                // Find the first sequentially available name.
                while (isNameValid == false)
                {
                    isNameValid = true;

                    // The current name that is being checked for collision.
                    if (counter > 0)
                    {
                        currentName = "New Audio " + counter.ToString();
                    }

                    for (int j = 0; j < project.AudioAssets.Count; j++)
                    {
                        if (currentName.ToUpper() == project.AudioAssets[j].Name.ToUpper())
                        {
                            isNameValid = false;
                            break;
                        }
                    }

                    counter++;
                }

                AudioAssetDto newAudio = projectController_.AddAudioAsset(openAudioAsset.FileName, currentName);

                // Add audio asset to list box.
                lbxAudioAssets.Items.Add(currentName);
            }
        }

        #endregion

        #region Event Handlers

        private void AudioAssetsEditorControl_VisibleChanged(object sender, System.EventArgs e)
        {
            audioPlayerControl_.Stop();
        }

        private void lbxAudioAssets_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbxAudioAssets.SelectedIndex >= 0 && lbxAudioAssets.SelectedIndex < lbxAudioAssets.Items.Count)
            {
                AudioAssetDto audioAsset = projectController_.GetAudioAssetByName(lbxAudioAssets.Items[lbxAudioAssets.SelectedIndex].ToString());

                IAudioAssetDtoProxy audioAssetProxy = firemelonEditorFactory_.NewAudioAssetProxy(projectController_, audioAsset.Id);

                if (audioPlayerControl_.AudioAsset != null)
                {
                    audioPlayerControl_.Stop();

                    // If the audio asset is different than the currently loaded one, unload the old asset and load the newly selected asset.
                    AudioAssetDto oldAudioAsset = projectController_.GetAudioAsset(audioPlayerControl_.AudioAsset.Id);

                    if (audioAsset.AudioResourceId != oldAudioAsset.AudioResourceId)
                    {
                        projectController_.UnloadAudioResource(oldAudioAsset.AudioResourceId);
                    }
                }

                projectController_.LoadAudio(audioAsset.AudioResourceId);

                pgAudioAsset.SelectedObject = audioAssetProxy;

                audioPlayerControl_.AudioAsset = audioAssetProxy;
            }
        }

        private void pgAudioAsset_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":

                    try
                    {
                        int oldIndex = lbxAudioAssets.Items.IndexOf(e.OldValue.ToString());

                        lbxAudioAssets.Items[oldIndex] = e.ChangedItem.Value.ToString();

                        lbxAudioAssets.Sorted = false;
                        lbxAudioAssets.Sorted = true;
                    }
                    catch (Exception ex)
                    {
                    }

                    break;
            }
        }

        #endregion
    }
}
