using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public interface IStateDtoProxy : IBaseDtoProxy
    {
        int StageWidth { get; set; }
        int StageHeight { get; set; }
        OriginLocation StageOriginLocation { get; set; }
    }
}
