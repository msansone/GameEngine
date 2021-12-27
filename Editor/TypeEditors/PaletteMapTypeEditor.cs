﻿using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    class PaletteMapTypeEditor : UITypeEditor
    {
        #region Constructors

        public PaletteMapTypeEditor()
        {

        }

        #endregion

        #region Private Variables
        #endregion

        #region Properties
        #endregion

        #region Public Functions

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            System.Diagnostics.Debug.Print(context.Instance.GetType().ToString());

            if (context == null || context.Instance == null || provider == null)
            {
                return value;
            }

            if (editorService != null)
            {
                PaletteMapControl selectionControl = new PaletteMapControl((PaletteMap)value, editorService);

                editorService.DropDownControl(selectionControl);

                return value;
            }

            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion
    }
}