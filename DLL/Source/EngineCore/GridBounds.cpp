#include "..\..\Headers\EngineCore\GridBounds.hpp"

using namespace firemelon;

GridBounds::GridBounds()
{
	startX_ = 0;
	startY_ = 0;
	endX_ = 0;
	endY_ = 0;
}

GridBounds::~GridBounds()
{
}

void GridBounds::setStartX(unsigned int value)
{
	startX_ = value;
}

unsigned int GridBounds::getStartX()
{
	return startX_;
}

void GridBounds::setStartY(unsigned int value)
{
	startY_ = value;
}

unsigned int GridBounds::getStartY()
{
	return startY_;
}

void GridBounds::setEndX(unsigned int value)
{
	endX_ = value;
}

unsigned int GridBounds::getEndX()
{
	return endX_;
}

void GridBounds::setEndY(unsigned int value)
{
	endY_ = value;
}

unsigned int GridBounds::getEndY()
{
	return endY_;
}