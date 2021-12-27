using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class EntityDto : BaseDto
    {
        private Rectangle boundingRect_ = new Rectangle(0, 0, 0, 0);
        public Rectangle BoundRect
        {
            get { return boundingRect_; }
        }

        private String tag_ = string.Empty;
        public String Tag
        {
            get { return tag_; }
            set { tag_ = value; }
        }


        private Guid classification_ = Guid.Empty;
        public Guid Classification
        {
            get { return classification_; }
            set { classification_ = value; }
        }
    }
}
