using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace FiremelonEditor2
{
    public class ParticleDto : BaseDto
    {
    }

    public class ParticleDtoProxy : IParticleDtoProxy
    {
        IProjectController projectController_;
        Guid particleId_;

        public ParticleDtoProxy(IProjectController projectController, Guid particleId)
        {
            projectController_ = projectController;
            particleId_ = particleId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                ParticleDto particle = projectController_.GetParticle(particleId_);

                return particle.Name;
            }
            set
            {
                try
                {
                    projectController_.SetParticleName(particleId_, value);
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
                return particleId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                ParticleDto particle = projectController_.GetParticle(particleId_);

                return particle.OwnerId;
            }
        }
    }

    public interface IParticleDtoProxy : IBaseDtoProxy
    {
    }
}
