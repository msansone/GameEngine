namespace FiremelonEditor2
{
    public interface IScriptsEditorControl : IAssetsEditorControl
    {
        bool PromptForSaveIfChanged();

        void ResetUi();
    }
}