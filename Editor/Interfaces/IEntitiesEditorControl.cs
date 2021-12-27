namespace FiremelonEditor2
{
    public interface IEntitiesEditorControl : IAssetsEditorControl
    {
        event EntitySelectionChangedHandler EntitySelectionChanged;

        void AddAnimationSlot();

        void AddHitbox();

        void AddProperty();

        void AddState();

        void DeleteAnimationSlot();

        void DeleteHitbox();

        void DeleteState();
    }
}
