#include "..\Headers\OpenGlSpriteSheet.hpp"

using namespace firemelon;

OpenGlSpriteSheet::OpenGlSpriteSheet(std::string name, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor) : SpriteSheet(name, rows, cols, cellHeight, cellWidth, scaleFactor)
{
	scaleFactor_ = scaleFactor;

	sheetOffsetX_ = 0;

	sheetOffsetY_ = 0;

	sheetWidth_ = 0;

	sheetHeight_ = 0;

	padding_ = 0;
}

OpenGlSpriteSheet::~OpenGlSpriteSheet()
{
	
}

void OpenGlSpriteSheet::freeSheet()
{

}

void OpenGlSpriteSheet::setSheetOffsetX(int value)
{
	sheetOffsetX_ = value;
}

int	OpenGlSpriteSheet::getSheetOffsetX()
{
	return sheetOffsetX_;
}

void OpenGlSpriteSheet::setSheetOffsetY(int value)
{
	sheetOffsetY_ = value;
}

int OpenGlSpriteSheet::getSheetOffsetY()
{
	return sheetOffsetY_;
}

void OpenGlSpriteSheet::setSheetWidth(int value)
{
	sheetWidth_ = value;
}

int	OpenGlSpriteSheet::getSheetWidth()
{
	return sheetWidth_;
}

void OpenGlSpriteSheet::setSheetHeight(int value)
{
	sheetHeight_ = value;
}

int OpenGlSpriteSheet::getSheetHeight()
{
	return sheetHeight_;
}