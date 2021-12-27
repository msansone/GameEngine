#include "..\..\Headers\EngineCore\MapLayer.hpp"

using namespace firemelon;

MapLayer::MapLayer(int layerRows, int layerCols, int gridRows, int gridCols)
{
	layerCols_ = layerCols;
	layerRows_ = layerRows;

	gridRows_ = gridRows;
	gridCols_ = gridCols;

	layerData_.resize(gridRows_);

	for (int i = 0; i < gridRows_; i++)
	{
		layerData_[i].resize(gridCols_);
	}

	isInteractive_ = false;
}

MapLayer::~MapLayer()
{
}

void MapLayer::setInteractive(bool value)
{
	isInteractive_ = value;
}

bool MapLayer::getInteractive()
{
	return isInteractive_;
}

void MapLayer::addToGrid(int row, int col, boost::shared_ptr<Renderable> renderable)
{
	// If this is the first value being inserted in this cell, add it to the list of occupied cells.
	if (layerData_[row][col].size() == 0)
	{
		GridCell gc(row, col);
		
		occupiedCells_.push_back(gc);
	}
	
	layerData_[row][col].push_back(renderable);

}

void MapLayer::removeFromGrid(int row, int col, int id)
{
	// Loop through the state machine controllers in the given grid cell. If a state machine controller
	// is found with the given ID, remove it.
	int size = layerData_[row][col].size();

	for (int i = size - 1; i >= 0; i--)
	{
		boost::shared_ptr<Renderable> r = layerData_[row][col][i];

		if (r->getRenderableId() == id)
		{		
			layerData_[row][col].erase(layerData_[row][col].begin() + i);
			
			if (layerData_[row][col].size() == 0)
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

int MapLayer::getLayerRows()
{
	return layerRows_;
}

int MapLayer::getLayerCols()
{
	return layerCols_;
}

int MapLayer::getGridRows()
{
	return gridRows_;
}

int MapLayer::getGridCols()
{
	return gridCols_;
}