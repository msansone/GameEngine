using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FiremelonEditor2
{
    public class NameIdPair : INameIdPair
    {
        public event NameChangedHandler NameChanged;

        private Guid id_;
        private string name_;

        public NameIdPair()
        {
            name_ = string.Empty;
            id_ = Guid.NewGuid();
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return id_;
            }
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            set
            {
                bool hasChanged = false;

                if (value != name_ && name_ != null)
                {
                    hasChanged = true;
                }

                string oldName = name_;

                name_ = value;

                if (hasChanged == true)
                {
                    // Name is changing. Call delegate to notify listeners
                    OnNameChange(new NameChangedEventArgs(oldName, value));
                }
            }
            get
            {
                return name_;
            }
        }

        private void OnNameChange(NameChangedEventArgs e)
        {
            if (NameChanged != null)
            {
                NameChanged(this, e);
            }
        }
    }
}
