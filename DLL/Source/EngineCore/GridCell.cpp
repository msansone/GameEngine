#include "..\..\Headers\EngineCore\GridCell.hpp"

using namespace firemelon;

GridCell::GridCell(int row, int column)
{
	row_ = row;
	column_ = column;
}

GridCell::GridCell()
{
	row_ = 0;
	column_ = 0;
}

GridCell::~GridCell()
{
}

int GridCell::getRow()
{
	return row_;
}

int GridCell::getColumn()
{
	return column_;
}

void GridCell::setRow(int row)
{
	row_ = row;
}

void GridCell::setColumn(int column)
{
	column_ = column;
}