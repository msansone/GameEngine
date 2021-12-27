#include "..\..\Headers\EngineCore\Animation.hpp"

using namespace firemelon;

Animation::Animation()
{
	spriteSheetId_ = -1;
	alphaMaskSheetId_ = -1;
}

Animation::~Animation()
{
	frames_.clear();
}

void Animation::setName(std::string name)
{
	name_ = name;
}

std::string Animation::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

std::string Animation::getName()
{
	return name_;
}


void Animation::setAlphaMaskSheetIdPy(int id)
{
	PythonReleaseGil unlocker;

	setAlphaMaskSheetId(id);
}

void Animation::setAlphaMaskSheetId(int id)
{
	alphaMaskSheetId_ = id;
}

int Animation::getAlphaMaskSheetIdPy()
{
	PythonReleaseGil unlocker;

	return getAlphaMaskSheetId();
}

int Animation::getAlphaMaskSheetId()
{
	return alphaMaskSheetId_;
}

void Animation::setSpriteSheetIdPy(int id)
{
	PythonReleaseGil unlocker;

	setSpriteSheetId(id);
}

void Animation::setSpriteSheetId(int id)
{
	spriteSheetId_ = id;
}

int Animation::getSpriteSheetIdPy()
{
	PythonReleaseGil unlocker;

	return getSpriteSheetId();
}

int Animation::getSpriteSheetId()
{
	return spriteSheetId_;
}

void Animation::addFramePy(boost::shared_ptr<AnimationFrame> newFrame)
{
	PythonReleaseGil unlocker;

	addFrame(newFrame);
}

void Animation::addFrame(boost::shared_ptr<AnimationFrame> newFrame)
{
	// Set anchor point manager here, for frames that were created programatically?

	frames_.push_back(newFrame);
}

int Animation::getFrameCountPy()
{
	PythonReleaseGil unlocker;

	return getFrameCount();
}

int Animation::getFrameCount()
{
	return frames_.size();
}

boost::shared_ptr<AnimationFrame> Animation::getFramePy(int index)
{
	PythonReleaseGil unlocker;

	return getFrame(index);
}

boost::shared_ptr<AnimationFrame> Animation::getFrame(int index)
{
	int size = frames_.size();
	
	if (index >= 0 && index < size)
	{
		return frames_[index];
	}
	else
	{
		return nullptr;
	}
}