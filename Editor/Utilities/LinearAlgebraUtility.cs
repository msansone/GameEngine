using System;
using System.Collections.Generic;
using System.Drawing;

namespace FiremelonEditor2
{
    public class LinearAlgebraUtility : ILinearAlgebraUtility
    {
        #region Constructors

        public LinearAlgebraUtility()
        {

        }

        #endregion

        #region Private Variables
        #endregion

        #region Properties
        #endregion

        #region Public Functions

        public void RotationTransform(List<PointF> pointsToRotate, List<PointF> rotatedPoints, PointF translationToOrigin, float rotationDegrees)
        {
            // The point after being translated to the native origin.
            float translatedToScreenOriginX = 0.0f;
            float translatedToScreenOriginY = 0.0f;

            // The point after being rotated about the origin.
            float rotatedX = 0.0f;
            float rotatedY = 0.0f;

            // Convert degrees to radians and set the cos and sin values for rotation.
            float radians = (float)((rotationDegrees * Math.PI) / 180.0);

            float sinTheta = (float)Math.Sin(radians);
            float cosTheta = (float)Math.Cos(radians);

            for (int i = 0; i < pointsToRotate.Count; i++)
            {
                // STEP 1: Translate each value to origin.
                translatedToScreenOriginX = pointsToRotate[i].X;
                translatedToScreenOriginY = pointsToRotate[i].Y;

                translatedToScreenOriginX -= translationToOrigin.X;
                translatedToScreenOriginY -= translationToOrigin.Y;

                // STEP 2: Do the actual rotation transform about the native origin.
                rotatedX = (translatedToScreenOriginX * cosTheta) - (translatedToScreenOriginY * sinTheta);
                rotatedY = (translatedToScreenOriginX * sinTheta) + (translatedToScreenOriginY * cosTheta);

                // STEP 3: Translate the vertices back to original position.
                rotatedX += translationToOrigin.X;
                rotatedY += translationToOrigin.Y;

                // STEP 4: Set the rotated values into the corners objects.
                rotatedPoints.Add(new PointF(rotatedX, rotatedY));
            }
        }

        #endregion

        #region Protected Functions
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion
    }
}
