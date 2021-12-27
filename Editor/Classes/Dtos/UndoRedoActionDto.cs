using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class UndoRedoActionDto
    {
        private byte[] projectState_;
        public byte[] ProjectState
        {
            get { return projectState_; }
            set { projectState_ = value; }
        }        
    }
}
