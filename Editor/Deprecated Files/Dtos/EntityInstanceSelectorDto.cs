using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FiremelonEditor2
{
    public class EntityInstanceSelectorDto
    {
        private Guid selectedLayerId = Guid.Empty;
        public Guid SelectedLayerId
        {
            get { return selectedLayerId; }
            set { selectedLayerId = value; }
        }

        private HashSet<Guid> selectedInstanceIds_ = new HashSet<Guid>();
        public HashSet<Guid> SelectedInstanceIds
        {
            get { return selectedInstanceIds_; }
        }

        private Point2D selectionCorner1_ = new Point2D(0, 0);
        public Point2D SelectionCorner1
        {
            get { return selectionCorner1_; }
        }

        private Point2D selectionCorner2_ = new Point2D(0, 0);
        public Point2D SelectionCorner2
        {
            get { return selectionCorner2_; }
        }

        private bool isSelectionOn_ = false;
        public bool IsSelectionOn
        {
            get { return isSelectionOn_; }
            set { isSelectionOn_ = value; }
        }

        private int selectionLeft_;
        public int SelectionLeft
        {
            get { return selectionLeft_; }
            set { selectionLeft_ = value; }
        }

        private int selectionRight_;
        public int SelectionRight
        {
            get { return selectionRight_; }
            set { selectionRight_ = value; }
        }

        private int selectionTop_;
        public int SelectionTop
        {
            get { return selectionTop_; }
            set { selectionTop_ = value; }
        }

        private int selectionBottom_;
        public int SelectionBottom
        {
            get { return selectionBottom_; }
            set { selectionBottom_ = value; }
        }

        private System.Drawing.Rectangle rectDrawable_;
        public System.Drawing.Rectangle DrawableRect
        {
            get { return rectDrawable_; }
            set { rectDrawable_ = value; }
        }

        private Color outlineColor_ = Color.Black;
        public Color OutlineColor
        {
            get { return outlineColor_; }
            set { outlineColor_ = value; }
        }

        private Color fillColor_ = Color.White;
        public Color FillColor
        {
            get { return fillColor_; }
            set { fillColor_ = value; }
        }
    }
}
