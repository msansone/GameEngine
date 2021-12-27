using System.Collections.Generic;
using System.Drawing;

namespace FiremelonEditor2
{
    public interface ILinearAlgebraUtility
    {
        void RotationTransform(List<PointF> pointsToRotate, List<PointF> rotatedPoints, PointF translationToOrigin, float rotationDegrees);        
    }
}
