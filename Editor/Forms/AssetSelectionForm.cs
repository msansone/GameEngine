using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public delegate void FormHiddenHandler(object sender, FormHiddenEventArgs e);

    public partial class AssetSelectionForm : Form, IAssetSelectionForm
    {
        public event FormHiddenHandler FormHidden;

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private IAssetSelectionControl assetSelectionControl_;

        private bool isMoving_;
        private bool isResizing_;

        private Point mouseOffset_;

        public AssetSelectionForm(IProjectController projectController, EditorForm owner) 
        {
            InitializeComponent();

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            assetSelectionControl_ = firemelonEditorFactory_.NewAssetSelectionControl(projectController);

            //assetsControl_.MenuOpening += new MenuOpeningHandler(this.AssetSelectionForm_MenuOpening);
            //assetsControl_.MenuClosed += new MenuClosedHandler(this.AssetSelectionForm_MenuClosed);

            //assetsControl_.ComboBoxClosed += new ComboBoxClosedHandler(this.AssetSelectionForm_ComboBoxClosed);

            pnInnerFrame.Controls.Add((System.Windows.Forms.Control)assetSelectionControl_);

            isMoving_ = false;
            isResizing_ = false;

            this.KeyDown += owner.EditorForm_KeyDown;
        }

        /*
        private void AssetSelectionForm_MenuOpening(object sender, MenuOpeningEventArgs e)
        {
            Globals.waitForMenuClose = true;
        }

        private void AssetSelectionForm_MenuClosed(object sender, MenuClosedEventArgs e)
        {
            Globals.waitForMenuClose = false;
        }

        private void AssetSelectionForm_ComboBoxClosed(object sender, ComboBoxClosedEventArgs e)
        {
            //this.Owner.Focus();
        }
        */
        private void AssetSelectionForm_Load(object sender, EventArgs e)
        {
            this.Refresh();
        }

        //protected override bool ShowWithoutActivation
        //{
        //    get { return true; }
        //}

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cparams = base.CreateParams;

        //        cparams.Style = unchecked((int)Constants.WindowStyles.WS_POPUP);

        //        cparams.ExStyle |= (int)Constants.WindowStyles.WS_EX_NOACTIVATE |
        //                           (int)Constants.WindowStyles.WS_EX_TOOLWINDOW;
        //        return cparams;
        //    }
        //}

        //protected override void DefWndProc(ref Message m)
        //{
        //    const int WM_MOUSEACTIVATE = 0x21;
        //    const int MA_NOACTIVATE = 0x0003;

        //    switch (m.Msg)
        //    {
        //        case WM_MOUSEACTIVATE:
        //            m.Result = (IntPtr)MA_NOACTIVATE;
        //            return;
        //    }
        //    base.DefWndProc(ref m);
        //}

        public IAssetSelectionControl AssetSelectionControl
        {
            get
            {
                return assetSelectionControl_;
            }
        }

        public IMapWidgetPropertiesControl PropertiesControl
        {
            set
            {
                assetSelectionControl_.PropertiesControl = value;
            }
        }

        public bool IsClosed
        {
            set
            {
                isClosed_ = value;
            }
            get
            {
                return isClosed_;
            }
        }
        private bool isClosed_ = false;

        protected virtual void OnFormHidden(FormHiddenEventArgs e)
        {
            FormHidden(this, e);
        }

        private void AssetSelectionForm_Resize(object sender, EventArgs e)
        {
            pnFrame.Top = 0;
            pnFrame.Left = 0;
            pnFrame.Width = this.ClientSize.Width;
            pnFrame.Height = this.ClientSize.Height;
        }

        private void AssetSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Don't unload the form, this causes an error when trying to show it again. Just hide it.
            e.Cancel = true;

            OnFormHidden(new FormHiddenEventArgs());
        }

        private void tmrChangeFocus_Tick(object sender, EventArgs e)
        {
            //if (Globals.waitForMenuClose == false)
            //{
            //    tmrChangeFocus.Enabled = false;
            //    this.Owner.Focus();
            //}
        }

        private void AssetSelectionForm_Activated(object sender, EventArgs e)
        {
            //tmrChangeFocus.Enabled = true;
        }

        private void pnFrame_Resize(object sender, EventArgs e)
        {
            pnInnerFrame.Top = 15;
            pnInnerFrame.Left = 5;
            pnInnerFrame.Height = pnFrame.ClientSize.Height - 20;
            pnInnerFrame.Width = pnFrame.ClientSize.Width - 10;

            pnTopLeftGrabber.Top = 0;
            pnTopLeftGrabber.Left = 0;
            pnTopLeftGrabber.Height = 3;
            pnTopLeftGrabber.Width = 3;

            pnTopLeftGrabber3.Top = 0;
            pnTopLeftGrabber3.Left = pnTopLeftGrabber.Width;
            pnTopLeftGrabber3.Height = 3;
            pnTopLeftGrabber3.Width = 6;

            pnTopGrabber.Top = 0;
            pnTopGrabber.Left = pnTopLeftGrabber3.Left + pnTopLeftGrabber3.Width;
            pnTopGrabber.Width = pnFrame.ClientSize.Width - 18;
            pnTopGrabber.Height = 3;

            pnTopRightGrabber3.Top = 0;
            pnTopRightGrabber3.Left = pnTopGrabber.Left + pnTopGrabber.Width;
            pnTopRightGrabber3.Height = 3;
            pnTopRightGrabber3.Width = 6;

            pnTopRightGrabber.Top = 0;
            pnTopRightGrabber.Left = pnTopRightGrabber3.Left + pnTopRightGrabber3.Width;
            pnTopRightGrabber.Height = 3;
            pnTopRightGrabber.Width = 3;

            pnTopLeftGrabber2.Top = pnTopLeftGrabber.Height;
            pnTopLeftGrabber2.Left = 0;
            pnTopLeftGrabber2.Height = 6;
            pnTopLeftGrabber2.Width = 3;

            pnLeftGrabber2.Left = 0;
            pnLeftGrabber2.Top = pnTopLeftGrabber2.Top + pnTopLeftGrabber2.Height;
            pnLeftGrabber2.Height = 6;
            pnLeftGrabber2.Width = 3;

            pnCaption.Left = 3;
            pnCaption.Width = pnTopGrabber.Width + pnTopRightGrabber3.Width + pnTopLeftGrabber3.Width;

            pnTopRightGrabber2.Left = pnCaption.Left + pnCaption.Width;
            pnTopRightGrabber2.Top = pnCaption.Top;
            pnTopRightGrabber2.Height = 6;
            pnTopRightGrabber2.Width = 3;

            pnRightGrabber2.Left = pnCaption.Left + pnCaption.Width;
            pnRightGrabber2.Top = pnTopRightGrabber2.Top + pnTopRightGrabber2.Height;
            pnRightGrabber2.Height = 6;
            pnRightGrabber2.Width = 3;

            pnLeftGrabber.Height = pnInnerFrame.Height;

            pnRightGrabber.Height = pnInnerFrame.Height;
            pnRightGrabber.Left = pnInnerFrame.Left + pnInnerFrame.Width;

            pnBottomLeftGrabber.Top = pnRightGrabber.Top + pnRightGrabber.Height;
            pnBottomLeftGrabber.Left = 0;

            pnBottomGrabber.Top = pnLeftGrabber.Top + pnLeftGrabber.Height;
            pnBottomGrabber.Width = pnInnerFrame.Width;

            pnBottomRightGrabber.Top = pnRightGrabber.Top + pnRightGrabber.Height;
            pnBottomRightGrabber.Left = pnBottomGrabber.Left + pnBottomGrabber.Width;

            lbHide.Left = pnCaption.Width - lbHide.Width - 1;
        }

        private void pnInnerFrame_Resize(object sender, EventArgs e)
        {
            assetSelectionControl_.Top = -1;
            assetSelectionControl_.Left = -1;
            assetSelectionControl_.Height = pnInnerFrame.ClientSize.Height + 2;
            assetSelectionControl_.Width = pnInnerFrame.ClientSize.Width + 2;
        }

        private void pnTopGrabber_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.SizeNS;
        }

        private void pnTopGrabber_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void pnTopLeftGrabber_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
        }

        private void pnTopLeftGrabber_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void pnTopRightGrabber_MouseEnter(object sender, EventArgs e)
        {

            this.Cursor = System.Windows.Forms.Cursors.SizeNESW;
        }

        private void pnTopRightGrabber_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void pnLeftGrabber_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.SizeWE;
        }

        private void pnLeftGrabber_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void pnRightGrabber_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.SizeWE;
        }

        private void pnRightGrabber_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void pnBottomLeftGrabber_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.SizeNESW;
        }

        private void pnBottomLeftGrabber_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void pnBottomGrabber_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.SizeNS;
        }

        private void pnBottomGrabber_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void pnBottomRightGrabber_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
        }

        private void pnBottomRightGrabber_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void lbHide_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbHide_MouseEnter(object sender, EventArgs e)
        {
            lbHide.BackColor = Color.LightGray;
        }

        private void lbHide_MouseLeave(object sender, EventArgs e)
        {
            lbHide.BackColor = pnCaption.BackColor;
        }

        private void lbHide_MouseDown(object sender, MouseEventArgs e)
        {
            lbHide.BackColor = Color.Silver;
        }

        private void pnCaption_MouseDown(object sender, MouseEventArgs e)
        {
            isMoving_ = true;

            mouseOffset_ = new Point(e.X, e.Y);
        }

        private void pnCaption_MouseUp(object sender, MouseEventArgs e)
        {
            isMoving_ = false;
        }

        private void pnCaption_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving_ == true)
            {
                Point pNewLocation = new Point(this.Location.X + (e.X - mouseOffset_.X), this.Location.Y + (e.Y - mouseOffset_.Y));
                this.Location = pNewLocation;
            }
        }

        private void pnCaption_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawString("Assets", new Font("Tahoma", 8), Brushes.Black, new Point(1, -2));

            g.Dispose();
        }

        private void pnBottomRightGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing_ = true;

            int mouseX = System.Windows.Forms.Cursor.Position.X;
            int mouseY = System.Windows.Forms.Cursor.Position.Y;

            int formX = this.Location.X;
            int formY = this.Location.Y;

            int formWidth = this.Size.Width;
            int formHeight = this.Size.Height;

            mouseOffset_ = new Point((formX + formWidth) - mouseX, (formY + formHeight) - mouseY);
        }

        private void pnBottomRightGrabber_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing_ = false;
        }

        private void pnBottomRightGrabber_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing_ == true)
            {
                int mouseX = System.Windows.Forms.Cursor.Position.X;
                int mouseY = System.Windows.Forms.Cursor.Position.Y;

                int newWidth = mouseX - this.Location.X + mouseOffset_.X;

                int minWidth = 30;

                if (newWidth < minWidth)
                {
                    newWidth = minWidth;
                }

                int newHeight = mouseY - this.Location.Y + mouseOffset_.Y;

                int minHeight = 30;

                if (newHeight < minHeight)
                {
                    newHeight = minHeight;
                }

                Size newSize = new Size(newWidth, newHeight);

                this.Size = newSize;

                this.Owner.Refresh();
            }
        }

        private void pnBottomGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing_ = true;

            int mouseY = System.Windows.Forms.Cursor.Position.Y;

            int formY = this.Location.Y;

            int formHeight = this.Size.Height;

            mouseOffset_ = new Point(0, (formY + formHeight) - mouseY);
        }

        private void pnBottomGrabber_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing_ = false;
        }

        private void pnBottomGrabber_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing_ == true)
            {
                int mouseY = System.Windows.Forms.Cursor.Position.Y;

                int newHeight = mouseY - this.Location.Y + mouseOffset_.Y;

                int minHeight = 30;

                if (newHeight < minHeight)
                {
                    newHeight = minHeight;
                }

                Size newSize = new Size(this.Width, newHeight);

                this.Size = newSize;

                this.Owner.Refresh();
            }
        }

        private void pnBottomLeftGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing_ = true;

            Panel pn = (Panel)sender;

            int mouseX = System.Windows.Forms.Cursor.Position.X;
            int mouseY = System.Windows.Forms.Cursor.Position.Y;

            int formX = this.Location.X;
            int formY = this.Location.Y;

            int formHeight = this.Size.Height;

            mouseOffset_ = new Point(mouseX - formX, (formY + formHeight) - mouseY);
        }

        private void pnBottomLeftGrabber_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing_ == true)
            {
                int mouseX = System.Windows.Forms.Cursor.Position.X;
                int mouseY = System.Windows.Forms.Cursor.Position.Y;

                int right = this.Right;

                int newLeft = mouseX - mouseOffset_.X;

                int newWidth = right - newLeft;

                int minWidth = 30;

                if (newWidth < minWidth)
                {
                    newWidth = minWidth;
                    newLeft = right - minWidth;
                }

                int newHeight = mouseY - this.Location.Y + mouseOffset_.Y;

                int minHeight = 30;

                if (newHeight < minHeight)
                {
                    newHeight = minHeight;
                }

                Size newSize = new Size(newWidth, newHeight);
                Point newLocation = new Point(newLeft, this.Top);

                this.Location = newLocation;
                this.Size = newSize;

                this.Owner.Refresh();
            }
        }

        private void pnBottomLeftGrabber_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing_ = false;
        }

        private void pnLeftGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing_ = true;

            Panel pn = (Panel)sender;

            int mouseX = System.Windows.Forms.Cursor.Position.X;

            int formX = this.Location.X;

            mouseOffset_ = new Point(mouseX - formX, 0);
        }

        private void pnLeftGrabber_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing_ = false;
        }

        private void pnLeftGrabber_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing_ == true)
            {
                int mouseX = System.Windows.Forms.Cursor.Position.X;
                int mouseY = System.Windows.Forms.Cursor.Position.Y;

                int right = this.Right;

                int newLeft = mouseX - mouseOffset_.X;

                int newWidth = right - newLeft;

                int minWidth = 30;

                if (newWidth < minWidth)
                {
                    newWidth = minWidth;
                    newLeft = right - minWidth;
                }

                Size newSize = new Size(newWidth, this.Height);
                Point newLocation = new Point(newLeft, this.Top);

                this.Location = newLocation;
                this.Size = newSize;

                this.Owner.Refresh();
            }
        }

        private void pnRightGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing_ = true;

            int mouseX = System.Windows.Forms.Cursor.Position.X;

            int formX = this.Location.X;

            int formWidth = this.Size.Width;

            mouseOffset_ = new Point((formX + formWidth) - mouseX, 0);
        }

        private void pnRightGrabber_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing_ = false;
        }

        private void pnRightGrabber_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing_ == true)
            {
                int mouseX = System.Windows.Forms.Cursor.Position.X;

                int newWidth = mouseX - this.Location.X + mouseOffset_.X;

                int minWidth = 30;

                if (newWidth < minWidth)
                {
                    newWidth = minWidth;
                }

                Size newSize = new Size(newWidth, this.Height);

                this.Size = newSize;

                this.Owner.Refresh();
            }
        }

        private void pnTopGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing_ = true;

            Panel pn = (Panel)sender;

            int mouseY = System.Windows.Forms.Cursor.Position.Y;

            int formY = this.Location.Y;

            mouseOffset_ = new Point(0, mouseY - formY);
        }

        private void pnTopGrabber_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing_ = false;
        }

        private void pnTopGrabber_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing_ == true)
            {
                int mouseX = System.Windows.Forms.Cursor.Position.X;
                int mouseY = System.Windows.Forms.Cursor.Position.Y;

                int bottom = this.Bottom;

                int newTop = mouseY - mouseOffset_.Y;

                int newHeight = bottom - newTop;

                int minHeight = 30;

                if (newHeight < minHeight)
                {
                    newHeight = minHeight;
                    newTop = bottom - minHeight;
                }

                Size newSize = new Size(this.Width, newHeight);
                Point newLocation = new Point(this.Left, newTop);

                this.Location = newLocation;
                this.Size = newSize;

                this.Owner.Refresh();
            }
        }

        private void pnTopLeftGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing_ = true;

            Panel pn = (Panel)sender;

            int mouseX = System.Windows.Forms.Cursor.Position.X;
            int mouseY = System.Windows.Forms.Cursor.Position.Y;

            int formX = this.Location.X;
            int formY = this.Location.Y;

            mouseOffset_ = new Point(mouseX - formX, mouseY - formY);
        }

        private void pnTopLeftGrabber_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing_ = false;
        }

        private void pnTopLeftGrabber_MouseMove(object sender, MouseEventArgs e)
        {

            if (isResizing_ == true)
            {
                int mouseX = System.Windows.Forms.Cursor.Position.X;
                int mouseY = System.Windows.Forms.Cursor.Position.Y;

                int bottom = this.Bottom;
                int right = this.Right;

                int newTop = mouseY - mouseOffset_.Y;
                int newHeight = bottom - newTop;
                int newLeft = mouseX - mouseOffset_.X;
                int newWidth = right - newLeft;

                int minHeight = 30;

                if (newHeight < minHeight)
                {
                    newHeight = minHeight;
                    newTop = bottom - minHeight;
                }

                int minWidth = 30;

                if (newWidth < minWidth)
                {
                    newWidth = minWidth;
                    newLeft = right - minWidth;
                }

                Size newSize = new Size(newWidth, newHeight);
                Point newLocation = new Point(newLeft, newTop);

                this.Location = newLocation;
                this.Size = newSize;

                this.Owner.Refresh();
            }
        }

        private void pnTopRightGrabber_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing_ = true;

            Panel pn = (Panel)sender;

            int mouseX = System.Windows.Forms.Cursor.Position.X;
            int mouseY = System.Windows.Forms.Cursor.Position.Y;

            int formX = this.Location.X;
            int formY = this.Location.Y;

            int formWidth = this.Size.Width;

            mouseOffset_ = new Point((formX + formWidth) - mouseX, mouseY - formY);
        }

        private void pnTopRightGrabber_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing_ = false;
        }

        private void pnTopRightGrabber_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing_ == true)
            {
                int mouseX = System.Windows.Forms.Cursor.Position.X;
                int mouseY = System.Windows.Forms.Cursor.Position.Y;

                int bottom = this.Bottom;

                int newTop = mouseY - mouseOffset_.Y;

                int newHeight = bottom - newTop;

                int newWidth = (mouseX - this.Location.X + mouseOffset_.X);

                int minHeight = 30;

                if (newHeight < minHeight)
                {
                    newHeight = minHeight;
                    newTop = bottom - minHeight;
                }
                int minWidth = 30;

                if (newWidth < minWidth)
                {
                    newWidth = minWidth;
                }

                Size newSize = new Size(newWidth, newHeight);
                Point newLocation = new Point(this.Left, newTop);

                this.Location = newLocation;
                this.Size = newSize;

                this.Owner.Refresh();
            }
        }        
    }

    public class FormHiddenEventArgs : System.EventArgs
    {
        // Field

        // Constructor
        public FormHiddenEventArgs()
        {

        }

        // Properties

    }
}