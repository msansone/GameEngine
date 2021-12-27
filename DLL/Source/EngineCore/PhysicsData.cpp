#include "..\..\Headers\EngineCore\PhysicsData.hpp"

using namespace firemelon;
using namespace boost::python;

PhysicsData::PhysicsData()
{
	collisionGrid_ = boost::shared_ptr<CollidableGrid>(new CollidableGrid());
}

PhysicsData::~PhysicsData()
{
}

void PhysicsData::setCameraManager(boost::shared_ptr<CameraManager> cameraManager)
{
	cameraManager_ = cameraManager;
}

void PhysicsData::getVisibleGridCells(int &startX, int &endX, int &startY, int &endY)
{
	// Calculate which rows and columns of the collision grid layer are currently intersecting with the camera viewport.
	boost::shared_ptr<CameraController> activeCamera = cameraManager_->getActiveCamera();

	Rect cameraViewport;
	cameraViewport.x = activeCamera->getPosition()->getClampedX();
	cameraViewport.y = activeCamera->getPosition()->getClampedY();
	cameraViewport.w = activeCamera->getCameraWidth();
	cameraViewport.h = activeCamera->getCameraHeight();

	int gridCellSize = collisionGrid_->getCellSize();
	int gridCols = collisionGrid_->getCols();
	int gridRows = collisionGrid_->getRows();

	// Use this value to determine which cells the camera intersects with.
	// Add 1 more to the result just to be sure it doesn't cut off.
	int gridStartCellX = (std::abs(cameraViewport.x) / gridCellSize) - 1;
	int gridStartCellY = (std::abs(cameraViewport.y) / gridCellSize) - 1;
	int gridEndCellX = (std::abs(cameraViewport.x + cameraViewport.w) / gridCellSize) + 1;
	int gridEndCellY = (std::abs(cameraViewport.y + cameraViewport.h) / gridCellSize) + 1;

	if (gridStartCellX < 0)
	{
		gridStartCellX = 0;
	}

	if (gridStartCellY < 0)
	{
		gridStartCellY = 0;
	}

	if (gridEndCellX >= gridCols)
	{
		gridEndCellX = gridCols - 1;
	}

	if (gridEndCellY >= gridRows)
	{
		gridEndCellY = gridRows - 1;
	}

	startX = gridStartCellX;
	endX = gridEndCellX;
	startY = gridStartCellY;
	endY = gridEndCellY;
}

void PhysicsData::adjustCollisionGrid(int index)
{
	int gridCols = collisionGrid_->getCols();
	int gridRows = collisionGrid_->getRows();

	// Search the collision grid. If the grid element is equal to the passed in index, remove it.
	for (int i = 0; i < gridRows; i++)
	{
		for (int j = 0; j < gridCols; j++)
		{
			// Iterate through each object referenced in the current grid cell.
			std::vector<int> deletionList;

			int size = collisionGrid_->grid_[i][j].size();

			for (int k = 0; k < size; k++)
			{
				int value = collisionGrid_->grid_[i][j][k];

				if (value == index)
				{
					deletionList.push_back(k);
				}
				else if(value > index)
				{
					collisionGrid_->grid_[i][j][k] -= 1;
				}
			}
			
			size = deletionList.size();
			for (int k = 0 ; k < size; k++)
			{
				int index = deletionList[k];
				collisionGrid_->grid_[i][j].erase(collisionGrid_->grid_[i][j].begin() + index);
			}
		}
	}
}
