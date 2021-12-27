using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace FiremelonEditor2
{
    public class ResourceBitmapReader : IResourceBitmapReader
    {
        public Bitmap ReadResourceBitmap(string resourceName)
        {
            Bitmap bitmap = null;

            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

                Stream stream = assembly.GetManifestResourceStream("FiremelonEditor2." + resourceName);
                
                bitmap = new Bitmap(stream);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Error getting resource - " + e.ToString(), "Error Getting Resource");
            }

            return bitmap;
        }
    }

    public interface IResourceBitmapReader
    {
        Bitmap ReadResourceBitmap(string resourceName);
    }
}
