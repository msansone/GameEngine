using System;
using System.IO;

namespace FiremelonEditor2
{
    public class UriUtility : IUriUtility
    {
        public string GetFullPath(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return string.Empty;
            }
            else
            {
                return Path.GetFullPath(folder);
            }
        }

        public string GetRelativePath(string folder)
        {
            if (String.IsNullOrEmpty(folder) == true)
            {
                return string.Empty;
            }

            if (!folder.EndsWith("\\"))
            {
                // May or may not be a directory.

                bool isDirectory = false;

                // Get the last part of the path.
                int lastSlashIndex = folder.LastIndexOf("\\");

                string fileOrDirectory = folder.Substring(lastSlashIndex, folder.Length - lastSlashIndex);

                if (fileOrDirectory.Contains("."))
                {
                    isDirectory = false;
                }
                else
                {
                    isDirectory = true;
                }

                if (isDirectory == true)
                {
                    // Directory paths must end in a slash                   
                    if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        folder += Path.DirectorySeparatorChar;
                    }
                }
            }


            Uri pathUri = new Uri(folder);

            string cwd = Environment.CurrentDirectory;

            if (!cwd.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                cwd += Path.DirectorySeparatorChar;
            }

            Uri cwdUri = new Uri(cwd);

            string relativePath =  "." + Path.DirectorySeparatorChar + Uri.UnescapeDataString(cwdUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
                       
            return relativePath;
        }
    }
}
