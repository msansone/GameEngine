using System;

namespace FiremelonEditor2
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);

        void HandleException(Exception ex, string caption);
    }
}
