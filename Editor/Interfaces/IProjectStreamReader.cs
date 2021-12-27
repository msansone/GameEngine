using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public interface IProjectStreamReader
    {
        BaseProjectDto ReadProjectFromStream(Stream stream);
    }
}
