
namespace FiremelonEditor2
{
    public class ExternalResourceDto : BaseDto
    {
        public ExternalResourceDto()
        {
        }
        
        public string Path
        {
            get { return path_; }
            set { path_ = value; }
        }
        private string path_;

        public string RelativePath
        {
            get { return relativePath_; }
            set { relativePath_ = value; }
        }
        private string relativePath_;
    }
}
