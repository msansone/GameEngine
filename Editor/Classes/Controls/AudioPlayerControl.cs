
using DragonOgg.MediaPlayer;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public partial class AudioPlayerControl : UserControl, IAudioPlayerControl
    {
        #region Constructors
        
        public AudioPlayerControl()
        {
            InitializeComponent();
            
            player_ = new OggPlayerFBN();

            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playicon));
            btnPlay.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);

            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseicon));
            btnPause.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);

            btnStop.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.stopicon));
            btnStop.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
        }

        #endregion

        #region Private Variables

        private OggPlayerFBN player_;

        #endregion

        #region Properties

        public IAudioAssetDtoProxy AudioAsset
        {
            get { return audioAssetDtoProxy_; }
            set
            {
                audioAssetDtoProxy_ = value;

                player_.SetCurrentFile(audioAssetDtoProxy_.Audio);
            }
        }
        private IAudioAssetDtoProxy audioAssetDtoProxy_ = null;

        #endregion

        #region Public Functions

        public void Stop()
        {
            player_.Stop();
        }

        #endregion

        #region Private Functions

        private void resize()
        {
            pnPlayer.Left = (this.ClientSize.Width / 2) - (pnPlayer.Width / 2);
            pnPlayer.Top = (this.ClientSize.Height / 2) - (pnPlayer.Height / 2);
        }

        #endregion

        #region Event Handlers

        private void AudioPlayerControl_Load(object sender, EventArgs e)
        {
            resize();
        }

        private void AudioPlayerControl_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            player_.Pause();
        }

        private void btnPause_MouseDown(object sender, MouseEventArgs e)
        {
            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseicondown));
        }

        private void btnPause_MouseEnter(object sender, EventArgs e)
        {
            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseiconover));
        }

        private void btnPause_MouseLeave(object sender, EventArgs e)
        {
            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseicon));
        }

        private void btnPause_MouseUp(object sender, MouseEventArgs e)
        {
            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseicon));
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            player_.Play();
        }

        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playicondown));
        }

        private void btnPlay_MouseEnter(object sender, EventArgs e)
        {
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playiconover));
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playicon));
        }

        private void btnPlay_MouseUp(object sender, MouseEventArgs e)
        {
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playicon));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            player_.Stop();
        }

        private void btnStop_MouseDown(object sender, MouseEventArgs e)
        {
            btnStop.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.stopicondown));
        }

        private void btnStop_MouseEnter(object sender, EventArgs e)
        {
            btnStop.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.stopiconover));
        }

        private void btnStop_MouseLeave(object sender, EventArgs e)
        {
            btnStop.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.stopicon));
        }

        private void btnStop_MouseUp(object sender, MouseEventArgs e)
        {
            btnStop.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.stopicon));
        }

        #endregion        
    }
}
