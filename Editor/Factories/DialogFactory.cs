namespace FiremelonEditor2
{
    class DialogFactory : IDialogFactory
    {
        #region Constructors

        public DialogFactory()
        {

        }

        #endregion

        #region Private Variables
        #endregion

        #region Properties
        #endregion

        #region Public Functions

        public IFindReplaceDialog NewFindReplaceDialog()
        {
            return new FindReplaceDialog();
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion
    }
}
