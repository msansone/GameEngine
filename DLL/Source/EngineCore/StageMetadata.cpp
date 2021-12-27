#include "..\..\Headers\EngineCore\StageMetadata.hpp"

using namespace firemelon;

StageMetadata::StageMetadata()
{
	angularVelocity_ = 0.0f;

	height_ = 0;

	origin_ = STAGE_ORIGIN_TOP_LEFT;

	width_ = 0;

	position_ = boost::shared_ptr<Position>(new Position(0, 0));

	renderEffects_ = boost::make_shared<RenderEffects>(RenderEffects());

	RotationOperationPtr rotationOperation = boost::make_shared<RotationOperation>(RotationOperation(0.0, 0, 0));

	renderEffects_->addRotation(rotationOperation);
}

StageMetadata::~StageMetadata()
{
}

int StageMetadata::getHeight()
{
	return height_;
}

void StageMetadata::setHeight(int height)
{
	height_ = height;
}

StageOrigin StageMetadata::getOrigin()
{
	return origin_;
}

void StageMetadata::setOrigin(StageOrigin origin)
{
	origin_ = origin;
}

PositionPtr StageMetadata::getPosition()
{
	return position_;
}

RotationOperationPtr StageMetadata::getRotationOperation()
{
	return renderEffects_->getRotationOperation(0);
}

int StageMetadata::getWidth()
{
	return width_;
}

void StageMetadata::setWidth(int width)
{
	width_ = width;
}


int StageMetadata::getBackgroundDepth()
{
	return backgroundDepth_;
}

void StageMetadata::setBackgroundDepth(int backgroundDepth)
{
	backgroundDepth_ = backgroundDepth;
}