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
    public class PythonScriptFilePathUiTypeEditor : UITypeEditor
    {
        private IScriptGeneratorFactory scriptGeneratorFactory_;

        public PythonScriptFilePathUiTypeEditor()
        {
            scriptGeneratorFactory_ = new ScriptGeneratorFactory();
        }
 
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
                bool generateScript = false;

                if ((string)(value.ToString()) == string.Empty)
                {
                    if (MessageBox.Show("Script not set. Do you want to generate a script?", "Generate Script?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        generateScript = true;
                    }
                }
                
                string fileName = string.Empty;

                if (generateScript == true)
                {
                    IFiremelonEditorFactory factory = new FiremelonEditorFactory();

                    ScriptDto script;

                    if (context.Instance is IRoomDtoProxy)
                    {
                        IRoomDtoProxy roomProxy = (IRoomDtoProxy)context.Instance;

                        script = roomProxy.GetScript();
                    }
                    else
                    {
                        IScriptDtoProxy scriptProxy = (IScriptDtoProxy)context.Instance;

                        script = scriptProxy.GetScript();
                    }

                    IScriptGenerator scriptGenerator = scriptGeneratorFactory_.NewScriptGenerator(script);

                    SaveFileDialog saveScript = new SaveFileDialog();

                    saveScript.DefaultExt = "py";
                    saveScript.AddExtension = true;
                    saveScript.RestoreDirectory = true;
                    saveScript.Filter = "Python Files|*.py";
                    saveScript.FileName = script.Name.ToLower() + ".py";
                    saveScript.RestoreDirectory = true;

                    if (saveScript.ShowDialog() == DialogResult.OK)
                    {
                        fileName = saveScript.FileName;

                        string scriptCode = scriptGenerator.Generate(script);

                        System.IO.File.WriteAllText(fileName, scriptCode);
                    }
                }
                else
                {
                    // get the editor service, just like in windows forms
                    editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                    OpenFileDialog openScript = new OpenFileDialog();

                    openScript.CheckFileExists = true;
                    openScript.CheckPathExists = true;
                    openScript.DefaultExt = "py";
                    openScript.Filter = "Python Files|*.py";
                    openScript.FileName = string.Empty;
                    openScript.Multiselect = false;
                    openScript.RestoreDirectory = true;

                    if (openScript.ShowDialog() == DialogResult.OK)
                    {
                        fileName = openScript.FileName;
                    }
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
