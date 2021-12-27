using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FiremelonEditor2
{
    public class ResourceTextReader : IResourceTextReader 
    {
        public string ReadResourceText(string resourceName)
        {
            string resourceData = string.Empty;

            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

                Stream stream = assembly.GetManifestResourceStream("FiremelonEditor2." + resourceName + ".txt");
                StreamReader reader = new StreamReader(stream);

                resourceData = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error getting resource - " + e.ToString(), "Error Getting Resource");
            }

            return resourceData;
        }
    }

    public interface IResourceTextReader
    {
        string ReadResourceText(string resourceName);
    }
}
