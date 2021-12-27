using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public interface IProjectStreamWriter
    {
        void WriteProjectToStream(BaseProjectDto project, Stream stream);
    }
}
