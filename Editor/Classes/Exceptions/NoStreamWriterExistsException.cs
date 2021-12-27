using System;

namespace FiremelonEditor2
{
    public class NoStreamWriterExistsException : Exception
    {
        public NoStreamWriterExistsException(string message) : base(message) { }
    }
}
