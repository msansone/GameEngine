using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FiremelonEditor2
{
    public enum HitboxPriority
    {
        LOW = 0,
        HIGH = 1
    };

    public class HitboxDto : BaseDto
    {
        public float BottomMostCorner
        {
            get
            {
                float bottomMostCorner = 0;

                for (int i = 0; i < transformedCorners_.Count; i++)
                {
                    if (i == 0)
                    {
                        bottomMostCorner = transformedCorners_[i].Y;
                    }
                    else
                    {
                        if (transformedCorners_[i].Y > bottomMostCorner)
                        {
                            bottomMostCorner = transformedCorners_[i].Y;
                        }
                    }
                }

                return bottomMostCorner;
            }
        }

        private Point2D cornerPoint1_ = new Point2D(0, 0);
        public Point2D CornerPoint1
        {
            get { return cornerPoint1_; }
            set { cornerPoint1_ = value; }
        }

        private Point2D cornerPoint2_ = new Point2D(0, 0);
        public Point2D CornerPoint2
        {
            get { return cornerPoint2_; }
            set { cornerPoint2_ = value; }
        }

        private Rectangle hitboxRect_ = new Rectangle(0, 0, 0, 0);
        public Rectangle HitboxRect
        {
            get { return hitboxRect_; }
            set { hitboxRect_ = value; }
        }

        private Guid identity_ = Guid.Empty;
        public Guid Identity
        {
            get { return identity_; }
            set { identity_ = value; }
        }

        private bool isSolid_ = false;
        public bool IsSolid
        {
            get { return isSolid_; }
            set { isSolid_ = value; }
        }

        public float LeftMostCorner
        {
            get
            {
                float leftmostCorner = 0;

                for (int i = 0; i < transformedCorners_.Count; i++)
                {
                    if (i == 0)
                    {
                        leftmostCorner = transformedCorners_[i].X;
                    }
                    else
                    {
                        if (transformedCorners_[i].X < leftmostCorner)
                        {
                            leftmostCorner = transformedCorners_[i].X;
                        }
                    }
                }

                return leftmostCorner;
            }
        }
        
        private HitboxPriority priority_;
        public HitboxPriority Priority
        {
            get { return priority_; }
            set { priority_ = value; }
        }

        public float RightMostCorner
        {
            get
            {
                float rightMostCorner = 0;

                for (int i = 0; i < transformedCorners_.Count; i++)
                {
                    if (i == 0)
                    {
                        rightMostCorner = transformedCorners_[i].X;
                    }
                    else
                    {
                        if (transformedCorners_[i].X > rightMostCorner)
                        {
                            rightMostCorner = transformedCorners_[i].X;
                        }
                    }
                }

                return rightMostCorner;
            }
        }

        private float rotationDegrees_ = 0.0f;
        public float RotationDegrees
        {
            get { return rotationDegrees_; }
            set {
                rotationDegrees_ = value; }
        }

        private List<PointF> transformedCorners_ = new List<PointF>();
        public List<PointF> TransformedCorners
        {
            get { return transformedCorners_; }
        }

        public float TopMostCorner
        {
            get
            {
                float topMostCorner = 0;

                for (int i = 0; i < transformedCorners_.Count; i++)
                {
                    if (i == 0)
                    {
                        topMostCorner = transformedCorners_[i].Y;
                    }
                    else
                    {
                        if (transformedCorners_[i].Y < topMostCorner)
                        {
                            topMostCorner = transformedCorners_[i].Y;
                        }
                    }
                }

                return topMostCorner;
            }
        }
    }

    public class HitboxDtoProxy : IHitboxDtoProxy
    {
        IProjectController projectController_;

        Guid hitboxId_;
        Guid hitboxIdentityId_ = Guid.Empty;

        public HitboxDtoProxy(IProjectController projectController, Guid hitboxId)
        {
            projectController_ = projectController;
            hitboxId_ = hitboxId;
        }

        [CategoryAttribute("Hitbox"),
         DescriptionAttribute("Hitbox Dimensions")]
        public int Left
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                return hitbox.HitboxRect.Left;
            }
            set
            {
                projectController_.SetHitboxRectLeft(hitboxId_, value);
            }
        }

        [CategoryAttribute("Hitbox"),
         DescriptionAttribute("Hitbox Dimensions")]
        public int Top
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                return hitbox.HitboxRect.Top;
            }
            set
            {
                projectController_.SetHitboxRectTop(hitboxId_, value);
            }
        }

        [CategoryAttribute("Hitbox"),
         DescriptionAttribute("Hitbox Dimensions")]
        public int Width
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                return hitbox.HitboxRect.Width;
            }
            set
            {
                projectController_.SetHitboxRectWidth(hitboxId_, value);
            }
        }

        [CategoryAttribute("Hitbox"),
         DescriptionAttribute("Hitbox Dimensions")]
        public int Height
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                return hitbox.HitboxRect.Height;
            }
            set
            {
                projectController_.SetHitboxRectHeight(hitboxId_, value);
            }
        }

        [BrowsableAttribute(false)]
        public Guid HitboxIdentityId
        {
            get
            {
                return hitboxIdentityId_;
            }
        }

        [TypeConverter(typeof(HitboxIdentityConverter))]
        public string Identity
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                hitboxIdentityId_ = hitbox.Identity;

                if (hitboxIdentityId_ == Guid.Empty)
                {
                    return string.Empty;
                }
                else
                {
                    HitboxIdentityDto hitboxIdentity = projectController_.GetHitboxIdentity(hitboxIdentityId_);

                    return hitboxIdentity.Name;
                }
            }
            set
            {
                Guid hitboxIdentityId = projectController_.GetHitboxIdentityIdFromName(value);

                projectController_.SetHitboxIdentity(hitboxId_, hitboxIdentityId);
            }
        }

        public bool IsSolid
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                return hitbox.IsSolid;
            }
            set
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                projectController_.SetHitboxIsSolid(hitboxId_, value);
            }
        }

        public HitboxPriority Priority
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                return hitbox.Priority;
            }
            set
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                projectController_.SetHitboxPriority(hitboxId_, value);
            }
        }

        public float RotationDegrees
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                return hitbox.RotationDegrees;
            }
            set
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                projectController_.SetHitboxRotationDegrees(hitboxId_, value);
            }
        }
        
        [BrowsableAttribute(false)]
        public Guid Id
        {
            get { return hitboxId_; }
        }

        [BrowsableAttribute(false)]
        public Guid OwnerId
        {
            get
            {
                HitboxDto hitbox = projectController_.GetHitbox(hitboxId_);

                return hitbox.OwnerId;
            }
        }

        [BrowsableAttribute(false)]
        public string Name
        {
            get { return string.Empty; }
            set { /*No-op*/ }
        }
    }
}
