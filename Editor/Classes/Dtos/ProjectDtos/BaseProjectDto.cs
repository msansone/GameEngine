using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public abstract class BaseProjectDto
    {
        public Version ProjectVersion
        {
            get { return null; }
            set { }

        }

        // Specifies the file version, so it can be loaded correctly.
        static public Version LatestProjectVersion
        {
            get { return latestProjectVersion_; }
        }
        static private Version latestProjectVersion_ = new Version(2, 2, 0, 0);

        public abstract Version FileVersion { get; }
    }
}
