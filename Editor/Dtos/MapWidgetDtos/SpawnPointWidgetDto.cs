using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class SpawnPointWidgetDto : MapWidgetDto
    {
        public SpawnPointWidgetDto()
        {
            Type = MapWidgetType.SpawnPoint;
        }

        private Guid identity_ = Guid.Empty;
        public Guid Identity
        {
            get { return identity_; }
            set { identity_ = value; }
        }

        private string identityName_ = string.Empty;
        public string IdentityName
        {
            get { return identityName_; }
            set { identityName_ = value; }
        }
    }
}
