#include "..\..\Headers\EngineCore\AnimationManager.hpp"

using namespace firemelon;

AnimationManager::AnimationManager()
{
}

AnimationManager::~AnimationManager()
{
	animations_.clear();
}

int AnimationManager::translateAnimationId(AssetId animationId)
{
	return animationIdMap_[animationId];
}

int AnimationManager::addAnimation(boost::shared_ptr<Animation> animation)
{
	int index = animations_.size();

	animations_.push_back(animation);

	std::string name = animation->getName();
	
	animationNameIdMap_[name] = index + 1;

	return index;
}

boost::shared_ptr<Animation> AnimationManager::getAnimationByNamePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getAnimationByName(name);
}

boost::shared_ptr<Animation> AnimationManager::getAnimationByName(std::string name)
{
	int index = animationNameIdMap_[name];

	auto animation = getAnimationByIndex(index - 1);

	if (animation == nullptr)
	{
		std::cout << "Animation with name " << name << " was not found." << std::endl;
	}

	return animation;
}

int AnimationManager::getAnimationId(std::string name)
{
	std::map<std::string, int>::iterator itr = animationNameIdMap_.find(name);

	if (itr != animationNameIdMap_.end())
	{
		return animationNameIdMap_[name] - 1;
	}

	return -1;
}

boost::shared_ptr<Animation> AnimationManager::getAnimationByIndexPy(int index)
{
	PythonReleaseGil unlocker;

	return getAnimationByIndex(index);
}

boost::shared_ptr<Animation> AnimationManager::getAnimationByIndex(int index)
{
	int size = animations_.size();

	if (index >= 0 && index < size)
	{
		return animations_[index];
	}

	return nullptr;
}