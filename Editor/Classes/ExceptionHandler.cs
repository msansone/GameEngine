using System;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    class ExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception ex)
        {
            MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK);
        }

        public void HandleException(Exception ex, string caption)
        {
            MessageBox.Show(ex.Message.ToString(), caption, MessageBoxButtons.OK);
        }
    }
}
