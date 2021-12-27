using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    class AlphaMaskSheetCellUiTypeEditor : UITypeEditor
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

                SheetCellSelectorDialog selectSheetCellDialog = new SheetCellSelectorDialog();
                
                IFrameDtoProxy frame = (IFrameDtoProxy)context.Instance;

                AnimationDto animation = frame.ProjectController.GetAnimation(frame.OwnerId);

                if (animation.AlphaMaskSheet == Guid.Empty)
                {
                    MessageBox.Show("Alpha mask sheet not set in parent animation.");
                    return null;
                }
                else
                {
                    SpriteSheetDto spriteSheet = frame.ProjectController.GetSpriteSheet(animation.AlphaMaskSheet);

                    selectSheetCellDialog.CellIndex = (int?)value;

                    selectSheetCellDialog.ShowDialog(System.Windows.Forms.Application.OpenForms[0], spriteSheet, frame.ProjectController);

                    if (selectSheetCellDialog.CellIndex.HasValue == true)
                    {
                        return selectSheetCellDialog.CellIndex.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                         
            }
            finally
            {
                editorService = null;
            }
        }
    }
}