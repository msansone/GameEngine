using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    public class SpawnPointDto : BaseDto
    {
        public int Size
        {
            get
            {
                return 16;
            }
        }
    }

    class SpawnPointDtoProxy : ISpawnPointDtoProxy
    {
        IProjectController projectController_;
        Guid spawnPointId_;

        public SpawnPointDtoProxy(IProjectController projectController, Guid spawnPointId)
        {
            projectController_ = projectController;
            spawnPointId_ = spawnPointId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                SpawnPointDto spawnPoint = projectController_.GetSpawnPoint(spawnPointId_);

                return spawnPoint.Name;
            }
            set
            {
                try
                {
                    projectController_.SetSpawnPointName(spawnPointId_, value);
                }
                catch (InvalidNameException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get
            {
                return spawnPointId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                SpawnPointDto spawnPoint = projectController_.GetSpawnPoint(spawnPointId_);

                return spawnPoint.OwnerId;
            }
        }
    }
}
