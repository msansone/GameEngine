﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FiremelonEditor2
{
    public partial class RoomEditorForm : DockContent, IRoomEditorForm
    {
        public RoomEditorForm()
        {
            InitializeComponent();
        }
    }
}
