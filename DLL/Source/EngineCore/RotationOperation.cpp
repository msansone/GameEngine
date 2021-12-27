#include "..\..\Headers\EngineCore\RotationOperation.hpp"

using namespace firemelon;

RotationOperation::RotationOperation(float angle, int pivotPointX, int pivotPointY)
{
	pivotPoint_ = boost::shared_ptr<Position>(new Position(pivotPointX, pivotPointY));

	angle_ = clampAngle(angle);

	previousAngle_ = angle;

	previousPreviousAngle_ = previousAngle_;
}

RotationOperation::~RotationOperation()
{
}

boost::shared_ptr<Position> RotationOperation::getPivotPointPy()
{
	PythonReleaseGil unlocker;

	return getPivotPoint();
}

boost::shared_ptr<Position> RotationOperation::getPivotPoint()
{
	return pivotPoint_;
}

float RotationOperation::getAnglePy()
{
	PythonReleaseGil unlocker;

	return getAngle();
}

float RotationOperation::getAngle()
{
	return angle_;
}

void RotationOperation::setAnglePy(float angle)
{
	PythonReleaseGil unlocker;

	setAngle(angle);
}

void RotationOperation::setAngle(float angle)
{
	previousPreviousAngle_ = previousAngle_;

	previousAngle_ = angle_;

	// Keep angle in range of 0 to 360.
	angle_ = clampAngle(angle);
}

float RotationOperation::clampAngle(float angle)
{
	// Convert angle into 0-359 range.
	float clampedAngle = fmod(angle, 360.0f);

	// Handle negative angles
	clampedAngle = fmod(clampedAngle + 360.0f, 360.0f);

	return clampedAngle;
}
