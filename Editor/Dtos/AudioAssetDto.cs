using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using DragonOgg.MediaPlayer;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public class AudioAssetDto : BaseDto
    {
        private Guid audioResourceId_;
        public Guid AudioResourceId
        {
            get { return audioResourceId_; }
            set { audioResourceId_ = value; }
        }

        private string channel_ = string.Empty;
        public string Channel
        {
            get { return channel_; }
            set { channel_ = value; }
        }
    }

    public class AudioAssetDtoProxy : IAudioAssetDtoProxy
    {
        private IProjectController projectController_;
        private Guid audioAssetId_;

        public AudioAssetDtoProxy(IProjectController projectController, Guid audioAssetId)
        {
            projectController_ = projectController;
            audioAssetId_ = audioAssetId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                AudioAssetDto audioAsset = projectController_.GetAudioAsset(audioAssetId_);

                return audioAsset.Name;
            }
            set
            {
                bool isValid = true;

                // Validate the tile sheet name.
                if (value == string.Empty)
                {
                    System.Windows.Forms.MessageBox.Show("Audio name cannot be blank.", "Invalid Value", MessageBoxButtons.OK);

                    isValid = false;
                }
                else
                {
                    ProjectDto project = projectController_.GetProjectDto();

                    for (int i = 0; i < project.AudioAssets.Count; i++)
                    {
                        if (value.ToUpper() == project.AudioAssets[i].Name.ToUpper() && project.AudioAssets[i].Id != audioAssetId_)
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Audio name is already in use.", "Name In Use", MessageBoxButtons.OK);
                    }
                }

                if (isValid == true)
                {
                    projectController_.SetAudioAssetName(audioAssetId_, value);
                }
            }
        }

        [CategoryAttribute("Data Source Settings"),
         DescriptionAttribute("The location of the source audio"),
        Editor(typeof(AudioFilePathUiTypeEditor), typeof(UITypeEditor))]
        public string AudioPath
        {
            get
            {
                AudioAssetDto audioAsset = projectController_.GetAudioAsset(audioAssetId_);

                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();
                ProjectDto project = projectController_.GetProjectDto();

                Guid audioResourceId = audioAsset.AudioResourceId;

                // Separate resources dto removed in 2.2 format.
                //AudioResourceDto audio = resources.AudioData[audioResourceId];
                AudioResourceDto audio = project.AudioData[audioResourceId];

                return audio.Path;
            }
            set
            {
                projectController_.SetAudioAssetAudioPath(audioAssetId_, value);
            }
        }

        [CategoryAttribute("Audio Group Settings"),
         DescriptionAttribute("The audio channel"),]
        public string Channel
        {
            get
            {
                AudioAssetDto audioAsset = projectController_.GetAudioAsset(audioAssetId_);

                return audioAsset.Channel;
            }
            set
            {
                projectController_.SetAudioAssetChannel(audioAssetId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public OggFile Audio
        {
            get
            {
                AudioAssetDto audioAsset = projectController_.GetAudioAsset(audioAssetId_);

                // Separate resources dto removed in 2.2 format.
                //ProjectResourcesDto resources = projectController_.GetResources();
                ProjectDto project = projectController_.GetProjectDto();

                Guid audioResourceId = audioAsset.AudioResourceId;

                // Separate resources dto removed in 2.2 format.
                //AudioResourceDto audioResource = resources.AudioData[audioResourceId];
                AudioResourceDto audioResource = project.AudioData[audioResourceId];

                return audioResource.Audio;
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return audioAssetId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                AudioAssetDto audioAsset = projectController_.GetAudioAsset(audioAssetId_);

                return audioAsset.OwnerId;
            }
        }
    }
}
