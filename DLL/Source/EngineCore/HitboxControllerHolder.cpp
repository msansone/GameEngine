#include "..\..\Headers\EngineCore\HitboxControllerHolder.hpp"

using namespace firemelon;

HitboxControllerHolder::HitboxControllerHolder()
{
	hitboxController_ = nullptr;
}

HitboxControllerHolder::~HitboxControllerHolder()
{
}

boost::shared_ptr<HitboxController> HitboxControllerHolder::getHitboxController()
{
	return hitboxController_;
}

void HitboxControllerHolder::setHitboxController(boost::shared_ptr<HitboxController> hitboxController)
{
	if (hitboxController_ == nullptr)
	{
		hitboxController_ = hitboxController;
	}
}

bool HitboxControllerHolder::getHasHitboxController()
{
	return hitboxController_ != nullptr;
}