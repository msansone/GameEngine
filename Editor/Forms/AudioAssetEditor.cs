using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using DragonOgg.MediaPlayer;

namespace FiremelonEditor2
{
    public partial class AudioAssetEditor : Form, IAudioAssetEditor
    {
        private IAudioAssetDtoProxy audioAssetProxy_;

        private OggPlayerFBN player_;

        public AudioAssetEditor()
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

        public void ShowDialog(IWin32Window owner, IAudioAssetDtoProxy audioAssetProxy)
        {
            audioAssetProxy_ = audioAssetProxy;

            player_.SetCurrentFile(audioAssetProxy_.Audio);

            pgAudioAsset.SelectedObject = audioAssetProxy_;

            base.ShowDialog(owner);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {            
            player_.Play();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            player_.Pause();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            player_.Stop();
        }

        private void AudioAssetEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            player_.Stop();
        }

        private void pnAudioAsset_Resize(object sender, EventArgs e)
        {
            resize();
        }

        private void resize()
        {
            pnPlayer.Left = (pnAudioAsset.Width / 2) - (pnPlayer.Width / 2);
            pnPlayer.Top = (pnAudioAsset.Height / 2) - (pnPlayer.Height / 2);
        }

        private void pnAudioAsset_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPlay_MouseEnter(object sender, EventArgs e)
        {
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playiconover));
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playicon));
        }

        private void btnPlay_MouseDown(object sender, MouseEventArgs e)
        {
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playicondown));
        }

        private void btnPlay_MouseUp(object sender, MouseEventArgs e)
        {
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.playicon));
        }

        private void btnPause_MouseDown(object sender, MouseEventArgs e)
        {
            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseicondown));
        }

        private void btnPause_MouseLeave(object sender, EventArgs e)
        {
            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseicon));
        }

        private void btnPause_MouseEnter(object sender, EventArgs e)
        {
            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseiconover));
        }

        private void btnPause_MouseUp(object sender, MouseEventArgs e)
        {
            btnPause.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.pauseicon));
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

        private void AudioAssetEditor_Load(object sender, EventArgs e)
        {
            resize();
        }

        private void pgAudioAsset_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label.ToUpper())
            {
                case "AUDIOPATH":

                    // Need to reset the player's current file.
                    player_.Stop();

                    player_.SetCurrentFile(audioAssetProxy_.Audio);

                    break;
            }
        }

        private void pgAudioAsset_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void pnPlayer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
