#include "..\..\Headers\EngineCore\AlphaMask.hpp"

using namespace firemelon;

AlphaMask::AlphaMask()
{
	sheetCellColumn_ = -1;
	sheetCellRow_ = -1;
	sheetId_ = -1;
}

AlphaMask::~AlphaMask()
{
}

int AlphaMask::getSheetIdPy()
{
	PythonReleaseGil unlocker;

	return getSheetId();
}

int AlphaMask::getSheetId()
{
	return sheetId_;
}

void AlphaMask::setSheetIdPy(int value)
{
	PythonReleaseGil unlocker;

	setSheetId(value);
}

void AlphaMask::setSheetId(int value)
{
	sheetId_ = value;
}

int AlphaMask::getSheetCellColumnPy()
{
	PythonReleaseGil unlocker;

	return getSheetCellColumn();
}

int AlphaMask::getSheetCellColumn()
{
	return sheetCellColumn_;
}

void AlphaMask::setSheetCellColumnPy(int value)
{
	PythonReleaseGil unlocker;

	setSheetCellColumn(value);
}

void AlphaMask::setSheetCellColumn(int value)
{
	sheetCellColumn_ = value;
}

int AlphaMask::getSheetCellRowPy()
{
	PythonReleaseGil unlocker;

	return getSheetCellRow();
}

int AlphaMask::getSheetCellRow()
{
	return sheetCellRow_;
}

void AlphaMask::setSheetCellRowPy(int value)
{
	PythonReleaseGil unlocker;

	setSheetCellRow(value);
}

void AlphaMask::setSheetCellRow(int value)
{
	sheetCellRow_ = value;
}