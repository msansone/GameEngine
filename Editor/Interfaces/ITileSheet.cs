using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiremelonEditor2
{
    public interface ITileSheet
    {
        INameIdPair NameId { get; }

        int CellCount { get; }
    }
}
