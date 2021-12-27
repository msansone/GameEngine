using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class AudioFilePathUiTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null)
            {
                return base.GetEditStyle(context);
            }

            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService;

            if (context == null || context.Instance == null || provider == null)
            {
                return value;
            }

            try
            {
                // get the editor service, just like in windows forms
                editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                OpenFileDialog openAudioAsset = new OpenFileDialog();

                openAudioAsset.CheckFileExists = true;
                openAudioAsset.CheckPathExists = true;
                openAudioAsset.DefaultExt = "ogg";
                openAudioAsset.Filter = "OGG Files|*.ogg";
                openAudioAsset.FileName = string.Empty;
                openAudioAsset.Multiselect = false;
                openAudioAsset.RestoreDirectory = true;

                string fileName = string.Empty;

                if (openAudioAsset.ShowDialog() == DialogResult.OK)
                {
                    fileName = openAudioAsset.FileName;
                }

                return fileName;
            }
            finally
            {
                editorService = null;
            }
        }
    }
}
