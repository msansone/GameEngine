#include "..\..\Headers\EngineCore\AnimationSlot.hpp"

using namespace firemelon;

AnimationSlot::AnimationSlot(std::string name, int x, int y)
{
	activeAnchorPointCount_ = 0;

	activeHitboxCount_ = 0;

	animationId_ = -1;

	animationPlayer_ = boost::shared_ptr<AnimationPlayer>(new AnimationPlayer(60, ANIMATION_STYLE_REPEAT));
	
	size_ = boost::shared_ptr<Size>(new Size(0, 0));

	name_ = name;
	
	nativePosition_ = boost::shared_ptr<Position>(new Position(x, y));

	for (int i = 0; i < 4; i++)
	{
		Vertex2 vertex;

		vertex.x = x;

		vertex.y = y;

		nativeCorners_.push_back(vertex);

		Vertex2 vertexTransformed;

		vertexTransformed.x = x;

		vertexTransformed.y = y;

		transformedCorners_.push_back(vertexTransformed);
	}
	
	isBackground_ = false;

	nextStateName_ = "";
		
	origin_ = ANIMATION_SLOT_ORIGIN_TOP_LEFT;

	renderEffects_ = RenderEffectsPtr(new RenderEffects);

	RotationOperationPtr rotationOperation = boost::shared_ptr<firemelon::RotationOperation>(new RotationOperation(0.0f, 0, 0));

	renderEffects_->addRotation(rotationOperation);
	
	// Initialize the hitbox references for this slot.
	std::vector<int> hitboxReferences;
	hitboxReferences_.push_back(hitboxReferences);

	//std::vector<std::vector<unsigned char>> slotHitboxEdgeFlags;
	//slotHitboxEdgeFlags_.push_back(slotHitboxEdgeFlags);

	//std::vector<std::vector<bool>> slotHitboxCollisionStatuses;
	//slotHitboxCollisionStatuses_.push_back(slotHitboxCollisionStatuses);


	// Initialize the anchor point references for this slot.
	std::vector<int> anchorPointReferences;
	anchorPointReferences_.push_back(anchorPointReferences);

}

AnimationSlot::~AnimationSlot()
{
}

int	AnimationSlot::getAnimationIdPy()
{
	PythonReleaseGil unlocker;

	return getAnimationId();
}

int AnimationSlot::getAnimationId()
{
	return animationId_;
}

AnimationPlayerPtr AnimationSlot::getAnimationPlayerPy()
{
	PythonReleaseGil unlocker;

	return getAnimationPlayer();
}

AnimationPlayerPtr AnimationSlot::getAnimationPlayer()
{
	return animationPlayer_;
}

std::string	AnimationSlot::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

std::string	AnimationSlot::getName()
{
	return name_;
}

PositionPtr	AnimationSlot::getNativePositionPy()
{
	PythonReleaseGil unlocker;

	return getNativePosition();
}

PositionPtr	AnimationSlot::getNativePosition()
{
	return nativePosition_;
}

int AnimationSlot::getNextAvailableAnchorPointId()
{
	size_t availableAnchorPointCount = availableAnchorPointIds_.size();

	if (activeAnchorPointCount_ < availableAnchorPointCount)
	{
		activeAnchorPointCount_++;

		return availableAnchorPointIds_[activeAnchorPointCount_ - 1];
	}

	return -1;
}


int AnimationSlot::getNextAvailableHitboxId()
{
	size_t availableHitboxCount = availableHitboxIds_.size();

	if (activeHitboxCount_ < availableHitboxCount)
	{
		activeHitboxCount_++;

		return availableHitboxIds_[activeHitboxCount_ - 1];
	}

	return -1;
}

RenderEffectsPtr AnimationSlot::getRenderEffectsPy()
{
	PythonReleaseGil unlocker;

	return getRenderEffects();
}

RenderEffectsPtr AnimationSlot::getRenderEffects()
{
	return renderEffects_;
}

float AnimationSlot::getBottommostPoint()
{
	float bottommost = transformedCorners_[0].y;

	for (int i = 1; i < 4; i++)
	{
		if (transformedCorners_[i].y > bottommost)
		{
			bottommost = transformedCorners_[i].y;
		}
	}

	return bottommost;
}

float AnimationSlot::getLeftmostPoint()
{
	float leftmost = transformedCorners_[0].x;

	for (int i = 1; i < 4; i++)
	{
		if (transformedCorners_[i].x < leftmost)
		{
			leftmost = transformedCorners_[i].x;
		}
	}

	return leftmost;
}

float AnimationSlot::getRightmostPoint()
{
	float rightmost = transformedCorners_[0].x;

	for (int i = 1; i < 4; i++)
	{
		if (transformedCorners_[i].x > rightmost)
		{
			rightmost = transformedCorners_[i].x;
		}
	}
	
	return rightmost;
}

float AnimationSlot::getTopmostPoint()
{
	float topmost = transformedCorners_[0].y;

	for (int i = 1; i < 4; i++)
	{
		if (transformedCorners_[i].y < topmost)
		{
			topmost = transformedCorners_[i].y;
		}
	}

	return topmost;
}

void AnimationSlot::setCornerPoints(float scaleFactor)
{
	// Potential subtle bug: adding the border padding could affect transforms in subtle ways. For example rotation around a pivot point will be off slightly.
	// Not sure what the best solution is yet. Add an offset to the pivot point? (As a separate variable, not baked into the pivot point data itself)

	// Set the corner points, (clockwise order, starting with top left).

	// 0------1
	// -      -
	// -      -
	// 3------2

	scaleFactor *= renderEffects_->getScaleFactor();

	int nativeSlotX = nativePosition_->getX();

	int nativeSlotY = nativePosition_->getY();

	int cornerLeft = 0;
	
	int cornerTop = 0;

	int width = size_->getWidth();

	int height = size_->getHeight();

	int halfWidth = width / 2;
	
	int halfHeight = height / 2;

	// Set the animation corner points in native TLC space.
	switch (origin_)
	{
	case ANIMATION_SLOT_ORIGIN_TOP_LEFT:

		// This is native space.
		cornerLeft = nativeSlotX;

		cornerTop = nativeSlotY;

		break;

	case ANIMATION_SLOT_ORIGIN_TOP_MIDDLE:

		cornerLeft = nativeSlotX - halfWidth;

		cornerTop = nativeSlotY;

		break;

	case ANIMATION_SLOT_ORIGIN_TOP_RIGHT:

		cornerLeft = nativeSlotX - width;

		cornerTop = nativeSlotY;

		break;

	case ANIMATION_SLOT_ORIGIN_MIDDLE_LEFT:

		cornerLeft = nativeSlotX;

		cornerTop = nativeSlotY - halfHeight;

		break;

	case ANIMATION_SLOT_ORIGIN_CENTER:

		cornerLeft = nativeSlotX - halfWidth;

		cornerTop = nativeSlotY - halfHeight;

		break;

	case ANIMATION_SLOT_ORIGIN_MIDDLE_RIGHT:

		cornerLeft = nativeSlotX - width;

		cornerTop = nativeSlotY - halfHeight;

		break;

	case ANIMATION_SLOT_ORIGIN_BOTTOM_LEFT:

		cornerLeft = nativeSlotX;

		cornerTop = nativeSlotY - height;

		break;

	case ANIMATION_SLOT_ORIGIN_BOTTOM_MIDDLE:

		cornerLeft = nativeSlotX - halfWidth;

		cornerTop = nativeSlotY - height;
	
		break;
	

	case ANIMATION_SLOT_ORIGIN_BOTTOM_RIGHT:

		cornerLeft = nativeSlotX - width;

		cornerTop = nativeSlotY - height;

		break;

	default:

		break;
	}

	// Adjust for the padding.

	nativeCorners_[0].x = cornerLeft - scaleFactor;
	nativeCorners_[0].y = cornerTop - scaleFactor;

	nativeCorners_[1].x = cornerLeft + width + scaleFactor;
	nativeCorners_[1].y = cornerTop - scaleFactor;

	nativeCorners_[2].x = cornerLeft + width + scaleFactor;
	nativeCorners_[2].y = cornerTop + height + scaleFactor;

	nativeCorners_[3].x = cornerLeft - scaleFactor;
	nativeCorners_[3].y = cornerTop + height + scaleFactor;
	
	transformedCorners_[0].x = nativeCorners_[0].x;
	transformedCorners_[0].y = nativeCorners_[0].y;

	transformedCorners_[1].x = nativeCorners_[1].x;
	transformedCorners_[1].y = nativeCorners_[1].y;

	transformedCorners_[2].x = nativeCorners_[2].x;
	transformedCorners_[2].y = nativeCorners_[2].y;

	transformedCorners_[3].x = nativeCorners_[3].x;
	transformedCorners_[3].y = nativeCorners_[3].y;
}