using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiremelonEditor2
{
    public class Rectangle
    {
        public Rectangle()
        {
            left_ = 0;
            top_ = 0;
            width_ = 0;
            height_ = 0;
        }

        public Rectangle(int left, int top, int width, int height)
        {
            left_ = left;
            top_ = top;
            width_ = width;
            height_ = height;
        }

        private int left_;
        public int Left
        {
            get { return left_; }
            set { left_ = value; }
        }

        private int top_;
        public int Top
        {
            get { return top_; }
            set { top_ = value; }
        }

        private int width_;
        public int Width
        {
            get { return width_; }
            set { width_ = value; }
        }

        private int height_;
        public int Height
        {
            get { return height_; }
            set { height_ = value; }
        }
        
        public int Right
        {
            get { return left_ + width_; }
        }

        public int Bottom
        {
            get { return top_ + height_; }
        }

        public override string ToString()
        {
            return "(" + Left + ", " + Top + ") + (" + Width + " x " + Height + ")";
        }
    }
}