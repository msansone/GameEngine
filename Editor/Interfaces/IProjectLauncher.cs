namespace FiremelonEditor2
{
    public interface IProjectLauncher
    {
        bool ExportScriptsOnly { get; set; }

        bool ShowWarnings { get; set; }

        void Launch();

        void LaunchWithConsole();
    }
}
