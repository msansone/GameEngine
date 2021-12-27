using System;

namespace FiremelonEditor2
{
    public class InvalidSlopeRiseException : Exception
    {
        public InvalidSlopeRiseException(int minimumValue, int maximumValue) : base("Slope rise extends beyond the world geometry bounds. The valid range is " + minimumValue + " to " + maximumValue) { }
    }
}
