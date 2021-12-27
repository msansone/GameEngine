using System.IO;

namespace FiremelonEditor2
{
    public interface IProjectUpgrader
    {
        void Upgrade(MemoryStream stream, MemoryStream upgradedStream);
    }
}
