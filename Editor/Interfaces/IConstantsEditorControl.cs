namespace FiremelonEditor2
{
    public interface IConstantsEditorControl : IAssetsEditorControl
    {
        void AddHitboxIdentity();

        void AddTriggerSignal();
        
        void DeleteHitboxIdentity();
        
        void DeleteTriggerSignal();
    }
}
