#include "..\..\Headers\EngineCore\HitboxManager.hpp"

using namespace firemelon;

HitboxManager::HitboxManager()
{
}

HitboxManager::~HitboxManager()
{
	hitboxes_.clear();
}

int HitboxManager::addHitbox(boost::shared_ptr<Hitbox> hitbox)
{
	int index = hitboxes_.size();

	hitboxes_.push_back(hitbox);

	hitbox->id_ = index;

	return index;
}

boost::shared_ptr<Hitbox> HitboxManager::getHitboxPy(int index)
{
	PythonReleaseGil unlocker;

	return getHitbox(index);
}

boost::shared_ptr<Hitbox> HitboxManager::getHitbox(int index)
{
	int size = hitboxes_.size();

	if (index >= 0 && index < size)
	{
		return hitboxes_[index];
	}

	return nullptr;
}