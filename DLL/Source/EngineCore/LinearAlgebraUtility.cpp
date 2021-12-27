#include "..\..\Headers\EngineCore\LinearAlgebraUtility.hpp"

using namespace firemelon;

LinearAlgebraUtility::LinearAlgebraUtility()
{
}

LinearAlgebraUtility::~LinearAlgebraUtility()
{
}

double LinearAlgebraUtility::dot(Vertex2 &vector1, Vertex2 &vector2)
{
	return (vector1.x * vector2.x) + (vector1.y * vector2.y);
}

double LinearAlgebraUtility::length(Vertex2 &vector)
{
	return sqrt((vector.x * vector.x) + (vector.y * vector.y));
}

void LinearAlgebraUtility::normalize(Vertex2 &vector)
{
	double magnitude = length(vector);

	if (magnitude > 0)
	{
		vector.x /= magnitude;

		vector.y /= magnitude;
	}
}

Vertex2 LinearAlgebraUtility::projection(Vertex2 &vector1, Vertex2 &vector2)
{
	double scalar = dot(vector1, vector2) / dot(vector2, vector2);

	Vertex2 projection;

	projection.x = scalar * vector2.x;

	projection.y = scalar * vector2.y;

	return projection;
}

void LinearAlgebraUtility::rotatePoint(float rotationAngle, Vertex2 pointToRotate, Vertex2 &rotatedPoint, int originDeltaX, int originDeltaY)
{
	// Convert degrees to radians and set the cos and sin values for rotation.
	double pi = 3.1415926535897;

	// The point after being translated to the native origin.
	float translatedToScreenOriginX = 0.0f;
	float translatedToScreenOriginY = 0.0f;

	// The point after being rotated about the origin.
	float rotatedX = 0.0f;
	float rotatedY = 0.0f;

	float radians = (rotationAngle * pi) / 180.0;

	float sinTheta = sin(radians);
	float cosTheta = cos(radians);

	// STEP 1: Translate each value to origin.
	translatedToScreenOriginX = pointToRotate.x;
	translatedToScreenOriginY = pointToRotate.y;

	translatedToScreenOriginX -= originDeltaX;
	translatedToScreenOriginY -= originDeltaY;

	// STEP 2: Do the actual rotation transform about the native origin.
	rotatedX = (translatedToScreenOriginX * cosTheta) - (translatedToScreenOriginY * sinTheta);
	rotatedY = (translatedToScreenOriginX * sinTheta) + (translatedToScreenOriginY * cosTheta);

	// STEP 3: Translate the vertices back to original position.
	rotatedX += originDeltaX;
	rotatedY += originDeltaY;

	// STEP 4: Set the rotated values into the corners objects.
	rotatedPoint.x = rotatedX;
	rotatedPoint.y = rotatedY;
}

void LinearAlgebraUtility::rotatePoints(float rotationAngle, std::vector<Vertex2> pointsToRotate, std::vector<Vertex2> &rotatedPoints, int originDeltaX, int originDeltaY)
{
	// Convert degrees to radians and set the cos and sin values for rotation.
	double pi = 3.1415926535897;

	// The point after being translated to the native origin.
	float translatedToScreenOriginX = 0.0f;
	float translatedToScreenOriginY = 0.0f;

	// The point after being rotated about the origin.
	float rotatedX = 0.0f;
	float rotatedY = 0.0f;

	float radians = (rotationAngle * pi) / 180.0;

	float sinTheta = sin(radians);
	float cosTheta = cos(radians);
	
	for (size_t i = 0; i < pointsToRotate.size(); i++)
	{
		// STEP 1: Translate each value to origin.
		translatedToScreenOriginX = pointsToRotate[i].x;
		translatedToScreenOriginY = pointsToRotate[i].y;

		translatedToScreenOriginX -= originDeltaX;
		translatedToScreenOriginY -= originDeltaY;

		// STEP 2: Do the actual rotation transform about the native origin.
		rotatedX = (translatedToScreenOriginX * cosTheta) - (translatedToScreenOriginY * sinTheta);
		rotatedY = (translatedToScreenOriginX * sinTheta) + (translatedToScreenOriginY * cosTheta);

		// STEP 3: Translate the vertices back to original position.
		rotatedX += originDeltaX;
		rotatedY += originDeltaY;

		// STEP 4: Set the rotated values into the corners objects.
		rotatedPoints[i].x = rotatedX;
		rotatedPoints[i].y = rotatedY;
	}
}

void LinearAlgebraUtility::mirrorPointHorizontally(Vertex2 &pointToMirror, int originDeltaX)
{
	// Shift it so that the translate is done about the native y axis.
	int tempX = pointToMirror.x - originDeltaX;

	// Reverse the sign.
	tempX *= -1;

	// Shift it back now that the transform is done.
	pointToMirror.x = tempX + originDeltaX;
}

void LinearAlgebraUtility::mirrorPointsHorizontally(std::vector<Vertex2> &pointsToMirror, int originDeltaX)
{
	for (size_t j = 0; j < pointsToMirror.size(); j++)
	{
		// Shift it so that the translate is done about the native y axis.
		int tempX = pointsToMirror[j].x - originDeltaX;

		// Reverse the sign.
		tempX *= -1;

		// Shift it back now that the transform is done.
		pointsToMirror[j].x = tempX + originDeltaX;
	}
}