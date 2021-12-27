using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class ParticleEmitterWidgetDto : MapWidgetDto
    {
        public ParticleEmitterWidgetDto()
        {
            Type = MapWidgetType.ParticleEmitter;
        }

        private Guid particleType_ = Guid.Empty;
        public Guid ParticleType
        {
            get { return particleType_; }
            set { particleType_ = value; }
        }

        private string particleTypeName_ = string.Empty;
        public string ParticleTypeName
        {
            get { return particleTypeName_; }
            set { particleTypeName_ = value; }
        }

        private Guid behaviorId_ = Guid.Empty;
        public Guid Behavior
        {
            get { return behaviorId_; }
            set { behaviorId_ = value; }
        }

        private string behaviorName_ = string.Empty;
        public string BehaviorName
        {
            get { return behaviorName_; }
            set { behaviorName_ = value; }
        }

        private Guid animationId_ = Guid.Empty;
        public Guid Animation
        {
            get { return animationId_; }
            set { animationId_ = value; }
        }

        private int animationFramesPerSecond_ = 60;
        public int AnimationFramesPerSecond
        {
            get { return animationFramesPerSecond_; }
            set { animationFramesPerSecond_ = value; }
        }

        private string animationName_ = string.Empty;
        public string AnimationName
        {
            get { return animationName_; }
            set { animationName_ = value; }
        }

        private bool attachParticles_ = false;
        public bool AttachParticles
        {
            get { return attachParticles_; }
            set { attachParticles_ = value; }
        }

        private int particlesPerEmission_ = 10;
        public int ParticlesPerEmission
        {
            get { return particlesPerEmission_; }
            set { particlesPerEmission_ = value; }
        }

        private int maxParticles_ = 10;
        public int MaxParticles
        {
            get { return maxParticles_; }
            set { maxParticles_ = value; }
        }

        private double interval_ = 1.0;
        public double Interval
        {
            get { return interval_; }
            set { interval_ = value; }
        }

        private double particleLifespan_ = 1.0;
        public double ParticleLifespan
        {
            get { return particleLifespan_; }
            set { particleLifespan_ = value; }
        }

        private bool isActive_ = false;
        public bool Active
        {
            get { return isActive_; }
            set { isActive_ = value; }
        }
    }
}
