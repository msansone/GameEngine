using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class InvalidProjectFileVersionException : Exception
    {
        public InvalidProjectFileVersionException(string message) : base(message) { }
    }
}
