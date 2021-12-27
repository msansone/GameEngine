#include "..\..\Headers\EngineCore\Hitbox.hpp"

using namespace firemelon;

Hitbox::Hitbox(int left, int top, int height, int width)
{
	baseRotationDegrees_ = 0.0f;

	debugColor_ = boost::make_shared<ColorRgba>(ColorRgba(0.0f, 1.0f, 0.0f, 1.0f));

	edgeFlags_ = 0xFF;

	slope_ = 0.0f;

	id_ = -1;

	isCollisionActive_ = false;

	isSolid_ = false;

	collisionStyle_ = COLLISION_STYLE_SOLID;

	useTopEdge_ = true;
	useRightEdge_ = true;
	useBottomEdge_ = true;
	useLeftEdge_ = true;

	ownerPosition_ = boost::make_shared<Position>(Position(0, 0));

	resize(left, top, height, width);
}

Hitbox::~Hitbox()
{
}

void Hitbox::resize(int left, int top, int height, int width)
{
	collisionRect_.x = left;
	collisionRect_.y = top;
	collisionRect_.h = height;
	collisionRect_.w = width;

	int untranslatedTop = top;
	int untranslatedLeft = left;
	int untranslatedHeight = height;
	int untranslatedWidth = width;

	bottom_ = top + height;
	untranslatedBottom_ = untranslatedTop + untranslatedHeight;

	centerX_ = left + (width / 2);
	untranslatedCenterX_ = untranslatedLeft + (untranslatedWidth / 2);

	centerY_ = top + (height / 2);
	untranslatedCenterY_ = untranslatedTop + (untranslatedHeight / 2);

	untranslatedLeft_ = untranslatedLeft;

	right_ = left + width;
	untranslatedRight_ = untranslatedLeft + untranslatedWidth;

	untranslatedTop_ = untranslatedTop;

	for (int i = 0; i < 4; i++)
	{
		Vertex2 cornerVertex;

		cornerVertex.x = 0;

		cornerVertex.y = 0;

		nativeCorners_.push_back(cornerVertex);

		Vertex2 transformedCornerVertex;

		transformedCornerVertex.x = 0;

		transformedCornerVertex.y = 0;

		transformedCorners_.push_back(transformedCornerVertex);
	}

	nativeCorners_[0].x = left;
	transformedCorners_[0].x = left;

	nativeCorners_[0].y = top;
	transformedCorners_[0].y = top;

	nativeCorners_[1].x = left + width;
	transformedCorners_[1].x = left + width;

	nativeCorners_[1].y = top;
	transformedCorners_[1].y = top;

	nativeCorners_[2].x = left + width;
	transformedCorners_[2].x = left + width;

	nativeCorners_[2].y = top + height;
	transformedCorners_[2].y = top + height;

	nativeCorners_[3].x = left;
	transformedCorners_[3].x = left;

	nativeCorners_[3].y = top + height;
	transformedCorners_[3].y = top + height;
}

float Hitbox::getBottommostCorner()
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

float Hitbox::getLeftmostCorner()
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

float Hitbox::getRightmostCorner()
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

float Hitbox::getTopmostCorner()
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


Rect Hitbox::getCollisionRectPy()
{
	PythonReleaseGil unlocker;

	return getCollisionRect();
}

Rect Hitbox::getCollisionRect()
{
	//return collisionRect_;

	// findmeSAT - This will eventually be removed once SAT collisions are done, because they're no longer axis aligned rects.
	Rect tempCollisionRect;
	
	tempCollisionRect.x = getLeftmostCorner();
	tempCollisionRect.y = getTopmostCorner();
	tempCollisionRect.h = getBottommostCorner() - tempCollisionRect.y;
	tempCollisionRect.w = getRightmostCorner() - tempCollisionRect.x;

	return tempCollisionRect;
}

void Hitbox::setCollisionRectPy(Rect rect)
{
	PythonReleaseGil unlocker;

	setCollisionRect(rect);
}

void Hitbox::setCollisionRect(Rect rect)
{
	collisionRect_ = rect;
}

void Hitbox::setIdentityPy(HitboxIdentity identity)
{
	PythonReleaseGil unlocker;

	setIdentity(identity);
}

void Hitbox::setIdentity(HitboxIdentity indentity)
{
	identity_ = indentity;
}

HitboxIdentity Hitbox::getIdentityPy()
{
	PythonReleaseGil unlocker;

	return getIdentity();
}

HitboxIdentity Hitbox::getIdentity()
{
	return identity_;
}

int Hitbox::getLeftPy()
{
	PythonReleaseGil unlocker;

	return getLeft();
}

int Hitbox::getLeft()
{
	return collisionRect_.x;
}

int Hitbox::getTopPy()
{
	PythonReleaseGil unlocker;

	return getTop();
}

int Hitbox::getTop()
{
	return collisionRect_.y;
}

int Hitbox::getHeightPy()
{
	PythonReleaseGil unlocker;

	return getHeight();
}

int Hitbox::getHeight()
{
 	return collisionRect_.h;
}

int Hitbox::getWidthPy()
{
	PythonReleaseGil unlocker;

	return getWidth();
}

int Hitbox::getWidth()
{
	return collisionRect_.w;
}

PositionPtr Hitbox::getOwnerPositionPy()
{
	PythonReleaseGil unlocker;

	return getOwnerPosition();
}

PositionPtr Hitbox::getOwnerPosition()
{
	return ownerPosition_;
}

void Hitbox::setEdgeFlags(unsigned char value)
{
	edgeFlags_ = value;
}

unsigned char Hitbox::getEdgeFlags()
{
	return edgeFlags_;
}

void Hitbox::setCollisionStatus(bool status)
{
	isCollisionActive_ = status;
}

bool Hitbox::getCollisionStatus()
{
	return isCollisionActive_;
}

void Hitbox::setIsSolid(bool isSolid)
{
	isSolid_ = isSolid;
}

bool Hitbox::getIsSolid()
{
	return isSolid_;
}

void Hitbox::setBaseRotationDegrees(float baseRotationDegrees)
{
	baseRotationDegrees_ = baseRotationDegrees;
}

float Hitbox::getBaseRotationDegrees()
{
	return baseRotationDegrees_;
}


float Hitbox::getSlope()
{
	return slope_;
}

void Hitbox::setSlope(float slope)
{
	slope_ = slope;
}

CollisionStyle Hitbox::getCollisionStylePy()
{
	PythonReleaseGil unlocker;
	
	return getCollisionStyle();
}

CollisionStyle Hitbox::getCollisionStyle()
{
	return collisionStyle_;
}

void Hitbox::setCollisionStyle(CollisionStyle collisionStyle)
{
	collisionStyle_ = collisionStyle;
}

bool Hitbox::getUseTopEdge()
{
	return useTopEdge_;
}

void Hitbox::setUseTopEdge(bool value)
{
	useTopEdge_ = value;
}

bool Hitbox::getUseRightEdge()
{
	return useRightEdge_;
}

void Hitbox::setUseRightEdge(bool value)
{
	useRightEdge_ = value;
}

bool Hitbox::getUseBottomEdge()
{
	return useBottomEdge_;
}

void Hitbox::setUseBottomEdge(bool value)
{
	useBottomEdge_ = value;
}

bool Hitbox::getUseLeftEdge()
{
	return useLeftEdge_;
}

void Hitbox::setUseLeftEdge(bool value)
{
	useLeftEdge_ = value;
}

ColorRgbaPtr Hitbox::getDebugColor()
{
	return debugColor_;
}