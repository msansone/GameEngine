using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public interface IAnimationDtoProxy : IBaseDtoProxy
    {
        Guid SpriteSheetId { get; }
        Guid AlphaMaskSheetId { get; }
    }
}
