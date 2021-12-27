namespace FiremelonEditor2
{
    public interface IAnimationsEditorControl : IAssetsEditorControl
    {
        event AnimationSelectionChangedHandler AnimationSelectionChanged;

        void AddActionPoint();

        void AddGroup();

        void AddFrame();

        void AddFrameTrigger();

        void AddHitbox();

        void DeleteFrame();

        void DeleteHitbox();
    }
}