using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FiremelonEditor2
{
    public delegate void BeforeListSortedHandler(object sender, BeforeListSortedEventArgs e);

    public class SortableBindingList<T> : BindingList<T>
    {
        public event BeforeListSortedHandler BeforeListSorted;

        private IComparer<T> comparer_ = null;
        public IComparer<T> Comparer
        {
            set { comparer_ = value; }
        }

        public void Sort()
        {
            ApplySortCore(null, ListSortDirection.Ascending);
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            List<T> items = this.Items as List<T>;
            
            OnBeforeListSorted(new BeforeListSortedEventArgs());
            
            // Apply and set the sort, if items to sort
            if (items != null && comparer_ != null)
            {
                items.Sort(comparer_);
            }

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            base.RemoveSortCore();
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected void OnBeforeListSorted(BeforeListSortedEventArgs e)
        {
            if (BeforeListSorted != null)
            {
                BeforeListSorted(this, e);
            }
        }
    }
    
    public class BeforeListSortedEventArgs : System.EventArgs
    {
        public BeforeListSortedEventArgs()
        {
        }
    }
}
