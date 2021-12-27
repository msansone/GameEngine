namespace FiremelonEditor2
{
    public interface IStateEditorControl
    {
        int SelectedAnimationSlotIndex { get; set; }

        IStateDtoProxy State { get; set; }

        void LockRefresh();

        void RefreshState(bool regenerateBackground);

        void UnlockRefresh();
    }
}
