using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiremelonEditor2
{
    public delegate void NameChangedHandler(object sender, NameChangedEventArgs e);

    public interface INameIdPair
    {
        event NameChangedHandler NameChanged;

        Guid Id { get; }

        string Name { get; set; }
    }

    public class NameChangedEventArgs : System.EventArgs
    {
        // Fields
        private string oldName_;
        private string newName_;

        // Constructor
        public NameChangedEventArgs(string oldName, string newName)
        {
            oldName_ = oldName;
            newName_ = newName;
        }

        // Properties
        public string OldName
        {
            set
            {
                oldName_ = value;
            }
            get
            {
                return oldName_;
            }
        }

        public string NewName
        {
            set
            {
                newName_ = value;
            }
            get
            {
                return newName_;
            }
        }
    }
}
