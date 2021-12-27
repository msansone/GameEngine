namespace FiremelonEditor2
{
    public interface IPythonScriptEditorControl
    {
        bool ChangesMade { get; }

        IScriptDtoProxy Script { get; set; }
        
        void Save();
    }
}
