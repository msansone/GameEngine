using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public interface IBaseDtoProxy
    {
        string Name { get; set; }

        Guid Id { get; }

        Guid OwnerId { get; }
    }
}
