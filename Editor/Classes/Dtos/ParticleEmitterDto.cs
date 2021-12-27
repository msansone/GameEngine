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
    public class ParticleEmitterDto : BaseDto
    {
    }

    public class ParticleEmitterDtoProxy : IParticleEmitterDtoProxy
    {
        IProjectController projectController_;
        Guid particleEmitterId_;

        public ParticleEmitterDtoProxy(IProjectController projectController, Guid particleEmitterId)
        {
            projectController_ = projectController;
            particleEmitterId_ = particleEmitterId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                ParticleEmitterDto particleEmitter = projectController_.GetParticleEmitter(particleEmitterId_);

                return particleEmitter.Name;
            }
            set
            {
                try
                {
                    projectController_.SetParticleEmitterName(particleEmitterId_, value);
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
                return particleEmitterId_;
            }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                ParticleEmitterDto particleEmitter = projectController_.GetParticleEmitter(particleEmitterId_);

                return particleEmitter.OwnerId;
            }
        }
    }

    public interface IParticleEmitterDtoProxy : IBaseDtoProxy
    {
    }
}
