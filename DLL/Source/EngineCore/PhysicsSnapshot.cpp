#include "..\..\Headers\EngineCore\PhysicsSnapshot.hpp"

using namespace firemelon;

PhysicsSnapshot::PhysicsSnapshot() 
{
	acceleration_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(0.0, 0.0));
	velocity_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(0.0, 0.0));
	position_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(0.0, 0.0));
	netForce_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(0.0, 0.0));
	movement_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(0.0, 0.0));
	positionInt_ = boost::shared_ptr<Vec2<int>>(new Vec2<int>(0, 0));
	positionIntDelta_ = boost::shared_ptr<Vec2<int>>(new Vec2<int>(0, 0));

	mass_ = 1.0;
}

PhysicsSnapshot::~PhysicsSnapshot() 
{
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getAccelerationPy()
{
	PythonReleaseGil unlocker;

	return getAcceleration();
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getAcceleration()
{
	return acceleration_;
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getVelocityPy()
{
	PythonReleaseGil unlocker;

	return getVelocity();
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getVelocity()
{
	return velocity_;
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getPositionPy()
{
	PythonReleaseGil unlocker;

	return getPosition();
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getPosition()
{
	return position_;
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getNetForcePy()
{
	PythonReleaseGil unlocker;

	return getNetForce();
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getNetForce()
{
	return netForce_;
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getMovementPy()
{
	PythonReleaseGil unlocker;

	return getMovement();
}

boost::shared_ptr<Vec2<float>> PhysicsSnapshot::getMovement()
{
	return movement_;
}

boost::shared_ptr<Vec2<int>> PhysicsSnapshot::getPositionIntPy()
{
	PythonReleaseGil unlocker;

	return getPositionInt();
}

boost::shared_ptr<Vec2<int>> PhysicsSnapshot::getPositionInt()
{
	return positionInt_;
}

boost::shared_ptr<Vec2<int>> PhysicsSnapshot::getPositionIntDeltaPy()
{
	PythonReleaseGil unlocker;

	return getPositionIntDelta();
}

boost::shared_ptr<Vec2<int>> PhysicsSnapshot::getPositionIntDelta()
{
	return positionIntDelta_;
}

float PhysicsSnapshot::getMassPy()
{
	PythonReleaseGil unlocker;

	return getMass();
}

float PhysicsSnapshot::getMass()
{
	return mass_;
}