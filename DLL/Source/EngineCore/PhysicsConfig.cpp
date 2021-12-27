#include "..\..\Headers\EngineCore\PhysicsConfig.hpp"

using namespace firemelon;

PhysicsConfig::PhysicsConfig() 
{
	gravity_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(0.0f, 0.0f));
	linearDamp_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(-0.01f, -0.01f));
	minimumVelocity_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(60.0f, 60.0f));
	timeScale_ = 1.0f;
}

PhysicsConfig::~PhysicsConfig() 
{
}

boost::shared_ptr<Vec2<float>> PhysicsConfig::getGravity()
{
	return gravity_;
}

boost::shared_ptr<Vec2<float>> PhysicsConfig::getGravityPy()
{
	PythonReleaseGil unlocker;

	return getGravity();
}

boost::shared_ptr<Vec2<float>> PhysicsConfig::getLinearDamp()
{
	return linearDamp_;
}

boost::shared_ptr<Vec2<float>> PhysicsConfig::getLinearDampPy()
{
	PythonReleaseGil unlocker;

	return getLinearDamp();
}

boost::shared_ptr<Vec2<float>> PhysicsConfig::getMinimumVelocity()
{
	return minimumVelocity_;
}

boost::shared_ptr<Vec2<float>> PhysicsConfig::getMinimumVelocityPy()
{
	PythonReleaseGil unlocker;

	return getMinimumVelocity();
}

float PhysicsConfig::getTimeScale()
{
	return timeScale_;
}

float PhysicsConfig::getTimeScalePy()
{
	PythonReleaseGil unlocker;

	return getTimeScale();
}

void PhysicsConfig::setTimeScale(float value)
{
	timeScale_ = value;
}

void PhysicsConfig::setTimeScalePy(float value)
{
	PythonReleaseGil unlocker;

	return setTimeScale(value);
}


void PhysicsConfig::copy(PhysicsConfig* physicsConfig)
{
	gravity_->setX(physicsConfig->getGravity()->getX());
	gravity_->setY(physicsConfig->getGravity()->getY());
	linearDamp_->setX(physicsConfig->getLinearDamp()->getX());
	linearDamp_->setX(physicsConfig->getLinearDamp()->getY());
	minimumVelocity_->setX(physicsConfig->getMinimumVelocity()->getX());
	minimumVelocity_->setY(physicsConfig->getMinimumVelocity()->getX());
	timeScale_ = physicsConfig->getTimeScale();
}