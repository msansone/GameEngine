#include "..\..\Headers\EngineCore\Stage.hpp"

using namespace firemelon;

Stage::Stage()
{
	metadata_ = boost::make_shared<StageMetadata>(StageMetadata());

	mirrorHorizontally_ = false;

	reapplyTransform_ = true;
}

Stage::~Stage()
{
	activeAnimationSlots_.clear();
}

void Stage::applyTransforms(double lerp)
{
	// First apply scaling. Currently not available.
	//applyScalingTransform();

	// First, rotate all of the animation slots and hitboxes.
	int stageElementsCount = elements_.size();

	for (int i = 0; i < stageElementsCount; i++)
	{
		elements_[i]->applyTransforms(mirrorHorizontally_, lerp);
	}	
}

StageElementsPtr Stage::getStageElementsByIndex(int index)
{
	if (index >= 0 && index < elements_.size())
	{
		return elements_[index];
	}
	else
	{
		return nullptr;
	}
}

StageElementsPtr Stage::getActiveStageElements()
{
	//stageIdTo
	return nullptr;
}

StageMetadataPtr Stage::getMetadata()
{
	return metadata_;
}

int Stage::getAnimationSlotIndex(std::string slotName)
{
	int size = activeAnimationSlots_.size();

	for (int i = 0; i < size; i++)
	{
		if (activeAnimationSlots_[i]->name_ == slotName)
		{
			return i;
		}
	}

	return -1;
}


StageOrigin Stage::getOrigin()
{
	return metadata_->getOrigin();
}

boost::shared_ptr<Position> Stage::getPivotPointPy()
{
	PythonReleaseGil unlocker;

	return getPivotPoint();
}

boost::shared_ptr<Position> Stage::getPivotPoint()
{
	return metadata_->getRotationOperation()->getPivotPoint();
}

float Stage::getRotationAnglePy()
{
	PythonReleaseGil unlocker;

	return getRotationAngle();
}

float Stage::getRotationAngle()
{
	return metadata_->getRotationOperation()->getAngle();
}

void Stage::setHeight(int height)
{
	return metadata_->setHeight(height);
}

void Stage::setOrigin(StageOrigin origin)
{
	metadata_->setOrigin(origin);
}

void Stage::setRotationAnglePy(float rotationAngle)
{
	PythonReleaseGil unlocker;

	setRotationAngle(rotationAngle);
}

void Stage::setRotationAngle(float rotationAngle)
{
	if (metadata_->getRotationOperation()->getAngle() != rotationAngle)
	{
		reapplyTransform_ = true;

		metadata_->getRotationOperation()->setAngle(rotationAngle);
	}
}

void Stage::setWidth(int width)
{
	return metadata_->setWidth(width);
}


bool Stage::getMirrorHorizontallyPy()
{
	PythonReleaseGil unlocker;

	return getMirrorHorizontally();
}

bool Stage::getMirrorHorizontally()
{
	return mirrorHorizontally_;
}


void Stage::setMirrorHorizontallyPy(bool mirrorHorizontally)
{
	PythonReleaseGil unlocker;

	setMirrorHorizontally(mirrorHorizontally);
}

void Stage::setMirrorHorizontally(bool mirrorHorizontally)
{
	if (mirrorHorizontally_ != mirrorHorizontally)
	{
		reapplyTransform_ = true;
	}

	mirrorHorizontally_ = mirrorHorizontally;
}