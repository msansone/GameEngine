using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace FiremelonEditor2
{
    public enum GradientDirection
    {
		None = 0,
		North = 1,
		South = 2,
		East = 3,
        West = 4,
        Northeast = 5,
        Northwest = 6,
        Southeast = 7,
        Southwest = 8,
        Radial = 9
    };

    public class AnimationSlotDto : BaseDto
    {
        public Guid Animation
        {
            get { return animationId_; }
            set { animationId_ = value; }
        }
        private Guid animationId_ = Guid.Empty;

        public ColorRgba BlendColor
        {
            get
            {
                return blendColor_;
            }
            set
            {
                blendColor_ = value;
            }
        }
        private ColorRgba blendColor_ = new ColorRgba(1.0f, 1.0f, 1.0f, 1.0f);

        public float BlendPercent
        {
            get
            {
                return blendPercent_;
            }
            set
            {
                blendPercent_ = value;

                if (blendPercent_ < 0.0f)
                {
                    blendPercent_ = 0.0f;
                }
                else if (blendPercent_ > 1.0f)
                {
                    blendPercent_ = 1.0f;
                }
            }
        }
        private float blendPercent_ = 0.0f;

        // This is for human entry purposes only. It converts the value into a "seconds per frame"
        // float, which is what it uses in the engine itself. It stores the frames per second
        // integer value as a sort of display only property.
        public int FramesPerSecond
        {
            get { return framesPerSecond_; }
            set { framesPerSecond_ = value; }
        }
        private int framesPerSecond_ = 10;

        public Point PivotPoint
        {
            get
            {
                return pivotPoint_;
            }
            set
            {
                pivotPoint_ = value;
            }
        }
        private Point pivotPoint_ = new Point(0, 0);

        public Point2D Position
        {
            get
            {
                return position_;
            }
            set
            {
                position_ = value;
            }
        }
        private Point2D position_ = new Point2D(0, 0);

        public ColorRgba HueColor
        {
            get
            {
                return hueColor_;
            }
            set
            {
                hueColor_ = value;
            }
        }
        private ColorRgba hueColor_ = new ColorRgba(1.0f, 1.0f, 1.0f, 1.0f);

        public float AlphaGradientFrom
        {
            get
            {
                return alphaGradientFrom_;
            }
            set
            {
                alphaGradientFrom_ = value;
            }
        }
        private float alphaGradientFrom_ = 1.0f;

        public float AlphaGradientTo
        {
            get
            {
                return alphaGradientTo_;
            }
            set
            {
                alphaGradientTo_ = value;
            }
        }
        private float alphaGradientTo_ = 1.0f;

        public float AlphaGradientRadius
        {
            get
            {
                return alphaGradientRadius_;
            }
            set
            {
                alphaGradientRadius_ = value;
            }
        }
        private float alphaGradientRadius_ = 1.0f;

        public Point AlphaGradientRadialCenter
        {
            get
            {
                return alphaGradientRadialCenter_;
            }
            set
            {
                alphaGradientRadialCenter_ = value;
            }
        }
        private Point alphaGradientRadialCenter_ = new Point(0, 0);

        public GradientDirection AlphaGradientDirection
        {
            get
            {
                return alphaGradientDirection_;
            }
            set
            {
                alphaGradientDirection_ = value;
            }
        }
        private GradientDirection alphaGradientDirection_ = GradientDirection.None;
        
        public Guid NextStateId
        {
            get { return nextStateId_; }
            set { nextStateId_ = value; }
        }
        private Guid nextStateId_ = Guid.Empty;
        
        public OriginLocation OriginLocation
        {
            get
            {
                return originLocation_;
            }
            set
            {
                originLocation_ = value;
            }
        }
        private OriginLocation originLocation_ = OriginLocation.TopLeft;
        
        private AnimationStyle animationStyle_ = AnimationStyle.AnimationRepeat;
        public AnimationStyle AnimationStyle
        {
            get { return animationStyle_; }
            set { animationStyle_ = value; }
        }

        public ColorRgba OutlineColor
        {
            get
            {
                return outlineColor_;
            }
            set
            {
                outlineColor_ = value;
            }
        }
        private ColorRgba outlineColor_ = new ColorRgba(1.0f, 1.0f, 1.0f, 1.0f);
        
        public float UpdateInterval
        {
            get { return updateInterval_; }
            set { updateInterval_ = value; }
        }
        private float updateInterval_ = 1.0f / 10;

        public bool Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
            }
        }
        private bool _background;
    }

    public class AnimationSlotDtoProxy : IAnimationSlotDtoProxy
    {
        IProjectController projectController_;
        Guid animationSlotId_;

        public AnimationSlotDtoProxy(IProjectController projectController, Guid animationSlotId)
        {
            projectController_ = projectController;
            animationSlotId_ = animationSlotId;
        }

        [CategoryAttribute("(ID Settings)"),
         DescriptionAttribute("Unique Name String"),
         ParenthesizePropertyName(true)]
        public string Name
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.Name;
            }
            set
            {
                try
                {
                    projectController_.SetAnimationSlotName(animationSlotId_, value);
                }
                catch (InvalidNameException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        [CategoryAttribute("Animation Player"),
         DescriptionAttribute("Animation Player Settings"),
         TypeConverter(typeof(AnimationConverter))]
        public string Animation
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                Guid animationId = animationSlot.Animation;

                if (animationId == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    AnimationDto animation = projectController_.GetAnimation(animationId);

                    return animation.Name;
                }
            }
            set
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                Guid ownerEntityId = animationSlot.RootOwnerId;

                Guid animationId = projectController_.GetAnimationIdFromName(value);

                projectController_.SetAnimationSlotAnimation(animationSlotId_, animationId);
            }
        }

        [CategoryAttribute("Position"),
         DescriptionAttribute("Animation Slot Position")]
        public int Left
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.Position.X;
            }
            set
            {
                projectController_.SetAnimationSlotPositionLeft(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Position"),
         DescriptionAttribute("Animation Slot Position")]
        public int Top
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.Position.Y;
            }
            set
            {
                projectController_.SetAnimationSlotPositionTop(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Hue"),
         DisplayName("Red"),
         DescriptionAttribute("Animation Slot Color Hue")]
        public float HueRed
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.HueColor.Red;
            }
            set
            {
                projectController_.SetAnimationSlotHueColorRed(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Hue"),
         DisplayName("Green"),
         DescriptionAttribute("Animation Slot Color Hue")]
        public float HueGreen
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.HueColor.Green;
            }
            set
            {
                projectController_.SetAnimationSlotHueColorGreen(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Hue"),
         DisplayName("Blue"),
         DescriptionAttribute("Animation Slot Color Hue")]
        public float HueBlue
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.HueColor.Blue;
            }
            set
            {
                projectController_.SetAnimationSlotHueColorBlue(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Hue"),
         DisplayName("Alpha"),
         DescriptionAttribute("Animation Slot Color Hue")]
        public float HueAlpha
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.HueColor.Alpha;
            }
            set
            {
                projectController_.SetAnimationSlotHueColorAlpha(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Blend"),
         DisplayName("Red"),
         DescriptionAttribute("Animation Slot Color Blend")]
        public float BlendRed
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.BlendColor.Red;
            }
            set
            {
                projectController_.SetAnimationSlotBlendColorRed(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Blend"),
         DisplayName("Green"),
         DescriptionAttribute("Animation Slot Color Blend")]
        public float BlendGreen
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.BlendColor.Green;
            }
            set
            {
                projectController_.SetAnimationSlotBlendColorGreen(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Blend"),
         DisplayName("Blue"),
         DescriptionAttribute("Animation Slot Color Blend")]
        public float BlendBlue
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.BlendColor.Blue;
            }
            set
            {
                projectController_.SetAnimationSlotBlendColorBlue(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Blend"),
         DisplayName("Alpha"),
         DescriptionAttribute("Animation Slot Color Blend")]
        public float BlendAlpha
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.BlendColor.Alpha;
            }
            set
            {
                projectController_.SetAnimationSlotBlendColorAlpha(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Color Blend"),
         DisplayName("Blend Percent"),
         DescriptionAttribute("The percentage degree to blend the color")]
        public float BlendPercent
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.BlendPercent;
            }
            set
            {
                projectController_.SetAnimationSlotBlendColorPercent(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Pivot Point"),
         DescriptionAttribute("Animation Slot Pivot Point")]
        public Point PivotPoint
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.PivotPoint;
            }
            set
            {
                projectController_.SetAnimationSlotPivotPoint(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Alpha Gradient"),
         DescriptionAttribute("Animation Slot Alpha Gradient From")]
        public float AlphaGradientFrom
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.AlphaGradientFrom;
            }
            set
            {
                projectController_.SetAnimationSlotAlphaGradientFrom(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Alpha Gradient"),
         DescriptionAttribute("Animation Slot Alpha Gradient To")]
        public float AlphaGradientTo
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.AlphaGradientTo;
            }
            set
            {
                projectController_.SetAnimationSlotAlphaGradientTo(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Alpha Gradient"),
         DescriptionAttribute("Animation Slot Alpha Gradient Radius")]
        public float AlphaGradientRadius
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.AlphaGradientRadius;
            }
            set
            {
                projectController_.SetAnimationSlotAlphaGradientRadius(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Alpha Gradient"),
        DescriptionAttribute("Animation Slot Alpha Gradient Radial Center")]
        public Point RadialGradientCenter
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.AlphaGradientRadialCenter;
            }
            set
            {
                projectController_.SetAnimationSlotAlphaGradientRadialCenter(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Alpha Gradient"),
         DescriptionAttribute("Animation Slot Alpha Gradient Direction")]
        public GradientDirection AlphaGradientDirection
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.AlphaGradientDirection;
            }
            set
            {
                projectController_.SetAnimationSlotAlphaGradientDirection(animationSlotId_, value);
            }
        }


        [CategoryAttribute("Origin"),
         DescriptionAttribute("Animation Slot Origin Location")]
        public OriginLocation AnimationOriginLocation
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.OriginLocation;
            }
            set
            {
                projectController_.SetAnimationSlotOriginLocation(animationSlotId_, value);
            }
        }

        [TypeConverter(typeof(StateConverter))]
        public string NextState
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                Guid nextStateId = animationSlot.NextStateId;

                if (nextStateId == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    StateDto nextState = projectController_.GetState(nextStateId);

                    if (nextState == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return nextState.Name;
                    }
                }
            }
            set
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);
                
                Guid nextStateId = projectController_.GetStateIdFromName(animationSlot.RootOwnerId, value);

                projectController_.SetAnimationSlotNextStateId(animationSlotId_, nextStateId);
            }
        }

        [CategoryAttribute("Animation Player"),
         DescriptionAttribute("Animation Player Settings")]
        public AnimationStyle AnimationStyle
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.AnimationStyle;
            }
            set
            {
                projectController_.SetAnimationSlotStyle(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Animation Player"),
         DescriptionAttribute("Animation Player Settings")]
        public int FramesPerSecond
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.FramesPerSecond;
            }
            set
            {
                projectController_.SetAnimationSlotFramesPerSecond(animationSlotId_, value);
            }
        }


        [CategoryAttribute("Outline Color"),
         DisplayName("Alpha"),
         DescriptionAttribute("Animation Slot Outline Color")]
        public float OutlineColorAlpha
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.OutlineColor.Alpha;
            }
            set
            {
                projectController_.SetAnimationSlotOutlineColorAlpha(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Outline Color"),
         DisplayName("Red"),
         DescriptionAttribute("Animation Slot Outline Color")]
        public float OutlineColorRed
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.OutlineColor.Red;
            }
            set
            {
                projectController_.SetAnimationSlotOutlineColorRed(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Outline Color"),
         DisplayName("Green"),
         DescriptionAttribute("Animation Slot Outline Color")]
        public float OutlineColorGreen
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.OutlineColor.Green;
            }
            set
            {
                projectController_.SetAnimationSlotOutlineColorGreen(animationSlotId_, value);
            }
        }

        [CategoryAttribute("Outline Color"),
         DisplayName("Blue"),
         DescriptionAttribute("Animation Slot Outline Color")]
        public float OutlineColorBlue
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.OutlineColor.Blue;
            }
            set
            {
                projectController_.SetAnimationSlotOutlineColorBlue(animationSlotId_, value);
            }
        }


        public bool Background
        {
            get
            {
                AnimationSlotDto animationSlot = projectController_.GetAnimationSlot(animationSlotId_);

                return animationSlot.Background;
            }
            set
            {
                projectController_.SetAnimationSlotBackgroundFlag(animationSlotId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return animationSlotId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                AnimationSlotDto slot = projectController_.GetAnimationSlot(animationSlotId_);

                return slot.OwnerId;
            }
        }
    }

    public interface IAnimationSlotDtoProxy : IBaseDtoProxy
    {
        OriginLocation AnimationOriginLocation { get; set; }
    }
}
