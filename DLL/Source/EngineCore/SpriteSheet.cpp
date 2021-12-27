#include "..\..\Headers\EngineCore\SpriteSheet.hpp"

using namespace firemelon;

int SpriteSheet::idCounter_ = 0;

SpriteSheet::SpriteSheet(std::string name, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor)
{
	id_ = idCounter_;	
	idCounter_++;

	name_ = name;
	rows_ = rows;
	cols_ = cols;
	cellHeight_ = cellHeight;
	cellWidth_ = cellWidth;
	cellCount_ = 0;
	scaleFactor_ = scaleFactor;
}

SpriteSheet::~SpriteSheet()
{
	freeSheet();
}

int SpriteSheet::getIdPy()
{
	PythonReleaseGil unlocker;

	return getId();
}

int SpriteSheet::getId()
{
	return id_;
}

void SpriteSheet::setPadding(int value)
{
	padding_ = value;
}

int SpriteSheet::getPadding()
{
	return padding_;
}

void SpriteSheet::freeSheet()
{
}

void SpriteSheet::setColumnsPy(int columns)
{
	PythonReleaseGil unlocker;

	setColumns(columns);
}

void SpriteSheet::setColumns(int columns)
{
	cols_ = columns;
}

int SpriteSheet::getColumnsPy()
{
	PythonReleaseGil unlocker;

	return getColumns();
}

int SpriteSheet::getColumns()
{
	return cols_;
}

void SpriteSheet::setRowsPy(int rows)
{
	PythonReleaseGil unlocker;

	setRows(rows);
}

void SpriteSheet::setRows(int rows)
{
	rows_ = rows;
}

int SpriteSheet::getRowsPy()
{
	PythonReleaseGil unlocker;

	return getRows();
}

int SpriteSheet::getRows()
{
	return rows_;
}

void SpriteSheet::setCellHeightPy(int cellHeight)
{
	PythonReleaseGil unlocker;

	setCellHeight(cellHeight);
}

void SpriteSheet::setCellHeight(int cellHeight)
{
	cellHeight_ = cellHeight;
}

int SpriteSheet::getCellHeightPy()
{
	PythonReleaseGil unlocker;

	return getCellHeight();
}

int SpriteSheet::getCellHeight()
{
	return cellHeight_;
}

void SpriteSheet::setCellWidthPy(int cellWidth)
{
	PythonReleaseGil unlocker;

	setCellWidth(cellWidth);
}

void SpriteSheet::setCellWidth(int cellWidth)
{
	cellWidth_ = cellWidth;
}

int SpriteSheet::getCellWidthPy()
{
	PythonReleaseGil unlocker;

	return getCellWidth();
}

int SpriteSheet::getCellWidth()
{
	return cellWidth_;
}

float SpriteSheet::getScaleFactorPy()
{
	PythonReleaseGil unlocker;

	return getScaleFactor();
}

float SpriteSheet::getScaleFactor()
{
	return scaleFactor_;
}

std::string	SpriteSheet::getSheetName()
{
	return name_;
}