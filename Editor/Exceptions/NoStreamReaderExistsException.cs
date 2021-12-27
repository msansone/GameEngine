using System;

namespace FiremelonEditor2
{
    public class NoStreamReaderExistsException : Exception
    {
        public NoStreamReaderExistsException(string message) : base(message) { }
    }
}
