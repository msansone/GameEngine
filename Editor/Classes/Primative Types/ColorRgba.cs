using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    // The ColorRgba class stores a float for the red, green, blue, and alpha components of a color.
    // They each range from 0.0 to 1.0.

    public class ColorRgba
    {
        public ColorRgba(float r, float g, float b, float a)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = a;
        }

        private float r_;
        public float Red
        {
            set
            {
                r_ = value;

                if (r_ < 0.0f)
                {
                    r_ = 0.0f;
                }
                else if (r_ > 1.0f)
                {
                    r_ = 1.0f;
                }
            }
            get
            {
                return r_;
            }
        }
        
        private float g_;
        public float Green
        {
            set
            {
                g_ = value;

                if (g_ < 0.0f)
                {
                    g_ = 0.0f;
                }
                else if (g_ > 1.0f)
                {
                    g_ = 1.0f;
                }
            }
            get
            {
                return g_;
            }
        }


        private float b_;
        public float Blue
        {
            set
            {
                b_ = value;

                if (b_ < 0.0f)
                {
                    b_ = 0.0f;
                }
                else if (b_ > 1.0f)
                {
                    b_ = 1.0f;
                }
            }
            get
            {
                return b_;
            }
        }

        private float a_;
        public float Alpha
        {
            set
            {
                a_ = value;

                if (a_ < 0.0f)
                {
                    a_ = 0.0f;
                }
                else if (a_ > 1.0f)
                {
                    a_ = 1.0f;
                }
            }
            get
            {
                return a_;
            }
        }
    }
}
