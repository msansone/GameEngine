using System;

namespace FiremelonEditor2
{
    public class NoUpgraderExistsException : Exception
    {
        public NoUpgraderExistsException(string message) : base(message) { }
    }
}
