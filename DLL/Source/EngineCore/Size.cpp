#include "..\..\Headers\EngineCore\Size.hpp"

using namespace firemelon;

Size::Size(int width, int height)
{
	height_ = height;
	width_ = width;
}

Size::~Size()
{
}

int Size::getHeightPy()
{
	PythonReleaseGil unlocker;

	return getHeight();
}

int Size::getHeight()
{
	return height_;
}

int Size::getWidthPy()
{
	PythonReleaseGil unlocker;

	return getWidth();
}

int Size::getWidth()
{
	return width_;
}

void Size::setWidthPy(int width)
{
	PythonReleaseGil unlocker;

	setWidth(width);
}

void Size::setWidth(int value)
{
	width_ = value;
}

void Size::setHeightPy(int height)
{
	PythonReleaseGil unlocker;

	setHeight(height);
}

void Size::setHeight(int height)
{
	height_ = height;
}