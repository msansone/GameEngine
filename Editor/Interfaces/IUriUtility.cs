namespace FiremelonEditor2
{
    public interface IUriUtility
    {
        string GetFullPath(string folder);

        string GetRelativePath(string folder);
    }
}
