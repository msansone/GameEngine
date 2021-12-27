#include "..\..\Headers\EngineCore\DynamicsControllerHolder.hpp"

using namespace firemelon;

DynamicsControllerHolder::DynamicsControllerHolder(PhysicsConfigPtr physicsConfig)
{
	dynamicsController_ = nullptr;
	hasDynamicsController_ = false;
	physicsConfig_ = physicsConfig;
}

DynamicsControllerHolder::~DynamicsControllerHolder()
{
}

DynamicsController* DynamicsControllerHolder::getDynamicsController()
{
	return dynamicsController_;
}

void DynamicsControllerHolder::setDynamicsController(DynamicsController* dynamicsController)
{
	if (dynamicsController_ == nullptr)
	{
		dynamicsController_ = dynamicsController;

		dynamicsController_->setGlobalPhysicsConfig(physicsConfig_);
	}
}

bool DynamicsControllerHolder::getHasDynamicsController()
{
	return hasDynamicsController_;
}