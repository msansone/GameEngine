using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    public class DataFileDto : BaseDto
    {
        private string filePath_ = string.Empty;
        public string FilePath
        {
            get { return filePath_; }
            set { filePath_ = value; }
        }

        private string fileFullPath_ = string.Empty;
        public string FileFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(filePath_) == false && string.IsNullOrEmpty(Name) == false && string.IsNullOrEmpty(extension_) == false)
                {
                    string fullPath = string.Empty;

                    if (filePath_.EndsWith("\\") == true)
                    {
                        fullPath = filePath_;
                    }
                    else
                    {
                        fullPath = filePath_ + "\\";
                    }

                    fullPath += (Name + "." + extension_);

                    return fullPath;                        
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string extension_ = string.Empty;
        public string Extension
        {
            get { return extension_; }
            set { extension_ = value; }
        }

        private string fileRelativePath_ = string.Empty;
        public string FileRelativePath
        {
            get { return fileRelativePath_; }
            set { fileRelativePath_ = value; }
        }        
    }

    public class DataFileDtoProxy : IDataFileDtoProxy
    {
        IProjectController projectController_;
        Guid dataFileId_ = Guid.Empty;

        public DataFileDtoProxy(IProjectController projectController, Guid dataFileId)
        {
            projectController_ = projectController;
            dataFileId_ = dataFileId;
        }

        public DataFileDto GetDataFile()
        {
            DataFileDto dataFile = projectController_.GetDataFile(dataFileId_);

            return dataFile;
        }

        // Hide the name for data files, it's pointless for the user to see it.
        [BrowsableAttribute(false)]
        public string Name
        {
            get
            {
                DataFileDto dataFile = projectController_.GetDataFile(dataFileId_);

                return dataFile.Name;
            }
            set
            {
                return;
            }
        }

        [BrowsableAttribute(false)]
        public string FileFullPath
        {
            get
            {
                DataFileDto dataFile = projectController_.GetDataFile(dataFileId_);

                return dataFile.FileFullPath;
            }
        }

        [CategoryAttribute("Data Source Settings"),
        DescriptionAttribute("The location of the data file"),
        Editor(typeof(PythonScriptFilePathUiTypeEditor), typeof(UITypeEditor))]
        public string FilePath
        {
            get
            {
                DataFileDto dataFile = projectController_.GetDataFile(dataFileId_);

                return dataFile.FilePath;
            }
            set
            {
                projectController_.SetDataFilePath(dataFileId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public string Filename
        {
            get
            {
                DataFileDto dataFile = projectController_.GetDataFile(dataFileId_);

                return dataFile.Name;
            }
        }

        [BrowsableAttribute(false)]
        public string Extension
        {
            get
            {
                DataFileDto dataFile = projectController_.GetDataFile(dataFileId_);

                return dataFile.Extension;
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return dataFileId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                DataFileDto dataFile = projectController_.GetDataFile(dataFileId_);

                return dataFile.OwnerId;
            }
        }
    }

    public interface IDataFileDtoProxy : IBaseDtoProxy
    {
        string Extension { get; }
        string Filename { get; }
        string FileFullPath { get; }
        string FilePath { get; set; }

        DataFileDto GetDataFile();
    }
}
