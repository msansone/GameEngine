using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public interface IEntityDtoProxy : IBaseDtoProxy
    {
        string Tag { get; set; }

        // Removed in 2.1
        //string Classification { get; set; }
    }
}
