#include "..\..\Headers\EngineCore\AnchorPointManager.hpp"

using namespace firemelon;

AnchorPointManager::AnchorPointManager()
{
}

AnchorPointManager::~AnchorPointManager()
{
	anchorPoints_.clear();
}

int AnchorPointManager::addAnchorPoint(AnchorPointPtr anchorPoint)
{
	int index = anchorPoints_.size();

	anchorPoints_.push_back(anchorPoint);

	return index;
}

AnchorPointPtr AnchorPointManager::getAnchorPoint(int index)
{
	int size = anchorPoints_.size();

	if (index >= 0 && index < size)
	{
		return anchorPoints_[index];
	}

	return nullptr;
}
