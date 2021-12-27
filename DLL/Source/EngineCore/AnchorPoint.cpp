#include "..\..\Headers\EngineCore\AnchorPoint.hpp"

using namespace firemelon;

AnchorPoint::AnchorPoint(std::string name, int left, int top)
{
	point_ = boost::shared_ptr<Position>(new Position(left, top));

	initialize(name, left, top);
}

AnchorPoint::~AnchorPoint()
{
}

void AnchorPoint::initialize(std::string name, int left, int top)
{
	point_->initialize(left, top);

	nativePosition_.x = left;

	nativePosition_.y = top;

	name_ = name;
}

std::string	AnchorPoint::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

std::string	AnchorPoint::getName()
{
	return name_;
}


int AnchorPoint::getXPy()
{
	PythonReleaseGil unlocker;

	return getX();
}

int AnchorPoint::getX()
{
	return point_->getX();
}

int AnchorPoint::getYPy()
{
	PythonReleaseGil unlocker;

	return getY();
}

int AnchorPoint::getY()
{
	return point_->getY();
}