using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public interface IActorDtoProxy : IStatefulEntityDtoProxy
    {
        bool KeepRoomActive { get; set; }
    }
}
