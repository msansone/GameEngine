namespace FiremelonEditor2
{
    public interface IGameButtonsEditorControl : IAssetsEditorControl
    {
        event GameButtonSelectionChangedHandler GameButtonSelectionChanged;

        void AddGroup();

        void DeleteGroup();
    }
}
