#include "..\..\Headers\EngineCore\Position.hpp"

using namespace firemelon;

Position::Position(int x, int y)
{
	clampXLo_ = INT_MIN;

	clampXHi_ = INT_MAX;

	clampYLo_ = INT_MIN;

	clampYHi_ = INT_MAX;

	initialize(x, y);
}

Position::~Position()
{
}

void Position::initialize(int x, int y)
{
	x_ = x;
	y_ = y;

	previousX_ = x;
	previousY_ = y;

	previousXForRender_ = x;
	previousYForRender_ = y;
}

int Position::getXPy()
{
	PythonReleaseGil unlocker;

	return getX();
}

int Position::getX()
{
	return x_;
}

int Position::getClampedX()
{
	if (x_ < clampXLo_)
	{
		return clampXLo_;
	}
	else if (x_ > clampXHi_)
	{
		return clampXHi_;
	}

	return x_;
}

int Position::getYPy()
{
	PythonReleaseGil unlocker;

	return getY();
}

int Position::getY()
{
	return y_;
}

int Position::getClampedY()
{
	if (y_ < clampYLo_)
	{
		return clampYLo_;
	}
	else if (y_ > clampYHi_)
	{
		return clampYHi_;
	}

	return y_;
}

void Position::setXPy(int value)
{
	PythonReleaseGil unlocker;

	setX(value);
}

void Position::setX(int value)
{
	x_ = value;
}

void Position::setYPy(int value)
{
	PythonReleaseGil unlocker;

	setY(value);
}

void Position::setY(int value)
{
	y_ = value;
}

int Position::getPreviousXPy()
{
	PythonReleaseGil unlocker;

	return getPreviousX();
}

int Position::getPreviousX()
{
	return previousX_;
}

int Position::getPreviousXForRender()
{
	return previousXForRender_;
}

int Position::getPreviousYPy()
{
	PythonReleaseGil unlocker;

	return getPreviousY();
}

int Position::getPreviousY()
{
	return previousY_;
}

int Position::getPreviousYForRender()
{
	return previousYForRender_;
}

int Position::getPreviousClampedX()
{
	if (previousX_ < clampXLo_)
	{
		return clampXLo_;
	}
	else if (previousX_ > clampXHi_)
	{
		return clampXHi_;
	}

	return previousX_;
}

int Position::getPreviousClampedY()
{
	if (previousY_ < clampYLo_)
	{
		return clampYLo_;
	}
	else if (previousY_ > clampYHi_)
	{
		return clampYHi_;
	}

	return previousY_;
}

//int Position::getPreviousClampedYForRender()
//{
//	if (previousYForRender_ < clampYLo_)
//	{
//		return clampYLo_;
//	}
//	else if (previousYForRender_ > clampYHi_)
//	{
//		return clampYHi_;
//	}
//
//	return previousYForRender_;
//}

void Position::setPreviousXPy(int value)
{
	PythonReleaseGil unlocker;

	return setPreviousX(value);
}

void Position::setPreviousX(int value)
{
	previousX_ = value;

	if (SimulationTracker::isFirstUpdate == true)
	{
		previousXForRender_ = value;
	}
}

void Position::setPreviousYPy(int value)
{
	PythonReleaseGil unlocker;

	return setPreviousY(value);
}

void Position::setPreviousY(int value)
{
	previousY_ = value;

	if (SimulationTracker::isFirstUpdate == true)
	{
		previousYForRender_ = value;
	}
}