#include "..\..\Headers\EngineCore\CollidableGrid.hpp"

using namespace firemelon;

CollidableGrid::CollidableGrid()
{
}

CollidableGrid::~CollidableGrid()
{
}

void CollidableGrid::init(int cellSize, int rows, int cols)
{
	rows_ = rows;
	cols_ = cols;
	cellSize_ = cellSize;

	grid_.resize(rows);

	for (int i = 0; i < rows; i++)
	{
		grid_[i].resize(cols);
	}
}

void CollidableGrid::addToGrid(int row, int col, int entityInstanceId)
{
	// If this is the first value being inserted in this cell, add it to the list of occupied cells.
	if (grid_[row][col].size() == 0)
	{
		GridCell gc(row, col);
		occupiedCells_.push_back(gc);
	}

	grid_[row][col].push_back(entityInstanceId);
}

void CollidableGrid::removeFromGrid(int row, int col, int entityInstanceId)
{	
	// Loop through the ids in the given grid cell. If it is found, remove it.
	int size = grid_[row][col].size();

	for (int i = size - 1; i >= 0; i--)
	{
		int currentEntityInstanceId = grid_[row][col][i];

		if (currentEntityInstanceId == entityInstanceId)
		{		
			grid_[row][col].erase(grid_[row][col].begin() + i);

			if (grid_[row][col].size() == 0)
			{
				// Find and remove this cell from the occupied cell list.
				int size2 = occupiedCells_.size();
				for (int j = 0; j < size2; j++)
				{
					if (occupiedCells_[j].getColumn() == col && occupiedCells_[j].getRow() == row)
					{						
						occupiedCells_.erase(occupiedCells_.begin() + j);
						break;
					}
				}
			}

			break;
		}
	}
}

int CollidableGrid::getCollidableCount(int row, int col)
{
	return grid_[row][col].size();
}

int CollidableGrid::getCellSize()
{
	return cellSize_;
}

int CollidableGrid::getRows()
{
	return rows_;
}

int CollidableGrid::getCols()
{
	return cols_;
}

void CollidableGrid::clear()
{
	rows_ = 0;

	cols_ = 0;

	grid_.clear();
}