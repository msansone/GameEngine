#include "..\..\Headers\EngineCore\SpriteData.hpp"

using namespace firemelon;

SpriteData::SpriteData()
{
	interactiveLayer_ = -1;
	cellSize_ = 0;
	tileSize_ = 0;

	// A position object for entities that overlay the map.
	positionScreen_ = new Position(0, 0);
}

SpriteData::~SpriteData()
{
	int size = mapLayers_.size();
	for (int i = 0; i < size; i++)
	{
		if (mapLayers_[i] != nullptr)
		{
			delete mapLayers_[i];
			mapLayers_[i] = nullptr;
		}
	}

	size = layerPositions_.size();
	for (int i = 0; i < size; i++)
	{
		if (layerPositions_[i] != nullptr)
		{
			delete layerPositions_[i];
			layerPositions_[i] = nullptr;
		}
	}
	
	delete positionScreen_;
}

void SpriteData::getVisibleGridCells(int layerIndex, int &startX, int &endX, int &startY, int &endY, int &layerX, int &layerY, double lerp)
{
	MapLayer* mapLayer = mapLayers_[0];
	MapLayer* interactiveMapLayer = mapLayers_[interactiveLayer_];
	MapLayer* workingLayer = mapLayers_[layerIndex];

	// Get the largest layer height and layer width (used for parallax scrolling).
	int maxLayerWidth = mapLayer->getMaxCols() * tileSize_;
	int maxLayerHeight = mapLayer->getMaxRows() * tileSize_;

	int interactiveLayerWidth = interactiveMapLayer->getLayerCols() * tileSize_;
	int interactiveLayerHeight = interactiveMapLayer->getLayerRows() * tileSize_;

	// Calculate which rows and columns of the render grid layer are currently intersecting with the camera viewport.
	Rect cameraViewport;
	cameraViewport.x = camera_->getPosition()->getX();
	cameraViewport.y = camera_->getPosition()->getY();
	cameraViewport.w = camera_->getCameraWidth();
	cameraViewport.h = camera_->getCameraHeight();

	int layerWorldSpaceX = 0;
	int layerWorldSpaceY = 0;
			
	int layerCols = workingLayer->getLayerCols();
	int layerRows = workingLayer->getLayerRows();
	
	int gridCols = workingLayer->getGridCols();
	int gridRows = workingLayer->getGridRows();

	int layerWidth = layerCols * tileSize_;
	int layerHeight = layerRows * tileSize_;
	
	int previousX = camera_->getPosition()->getPreviousX();
	int previousY = camera_->getPosition()->getPreviousY();
							
	int currentX = camera_->getPosition()->getX();
	int currentY = camera_->getPosition()->getY();
							
	int cameraX = previousX + (int)floor((lerp * (currentX - previousX)));
	int cameraY = previousY + (int)floor((lerp * (currentY - previousY)));
	
    // Calculate the position of this layer in parallax, using the size of this layer vs the size 
	// of the interactive layer.
    if (interactiveLayerWidth == cameraViewport.w)
    {
        // If this layer is the same size as the camera, it's always going to located at coordinate 0 in world space.
        // Need to set this explicitly, otherwise it would result in a divide by zero error.
        layerWorldSpaceX = 0;
    }
    else
    {
        // How much bigger or smaller this layer is than the interactive layer
        double temp1 = interactiveLayerWidth - layerWidth;

        // How much bigger the interactive layer is than the camera.
        double temp2 = interactiveLayerWidth - cameraViewport.w;

        double scalingFactor = temp1 / temp2;

        layerWorldSpaceX = (int)(scalingFactor * (double)cameraX);
    }

    if (interactiveLayerHeight == cameraViewport.h)
    {
        layerWorldSpaceY = 0;
    }
    else
    {
        double temp1 = interactiveLayerHeight - layerHeight;
        double temp2 = interactiveLayerHeight - cameraViewport.h;
        double scalingFactor = temp1 / temp2;

        layerWorldSpaceY = (int)(scalingFactor * (double)cameraY);
    }
	
	layerX = layerWorldSpaceX - cameraX;
	layerY = layerWorldSpaceY - cameraY;
	
    // Calculate the subset of grid cells that are visible in the camera.    

	// Shift camera position so that it's coordinates are in the layerspace of the current layer.
	int cameraLayerSpaceX = cameraX - layerWorldSpaceX;
	int cameraLayerSpaceY = cameraY - layerWorldSpaceY;

	int extendedCells = 1;

	// Use this value to determine which cells the camera intersects with.
	int gridStartCellX = (std::abs(cameraLayerSpaceX) / cellSize_) - extendedCells;
	int gridStartCellY = (std::abs(cameraLayerSpaceY) / cellSize_) - extendedCells;
	int gridEndCellX = (std::abs(cameraLayerSpaceX + cameraViewport.w) / cellSize_) + extendedCells;
	int gridEndCellY = (std::abs(cameraLayerSpaceY + cameraViewport.h) / cellSize_) + extendedCells;

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


void SpriteData::clear()
{
	int size = mapLayers_.size();
	for (int i = 0; i < size; i++)
	{
		if (mapLayers_[i] != nullptr)
		{
			delete mapLayers_[i];
			mapLayers_[i] = nullptr;
		}
	}

	mapLayers_.clear();
	
	size = layerPositions_.size();
	for (int i = 0; i < size; i++)
	{
		if (layerPositions_[i] != nullptr)
		{
			delete layerPositions_[i];
			layerPositions_[i] = nullptr;
		}
	}

	layerPositions_.clear();
	dynamicsList_.clear();
	stateMachineControllers_.clear();
	nonGridSpriteList_.clear();
}


void SpriteData::getVisibleGridCells(int layerIndex, int &startX, int &endX, int &startY, int &endY, CameraController* camera)
{
	MapLayer* mapLayer = mapLayers_[0];
	MapLayer* interactiveMapLayer = mapLayers_[interactiveLayer_];
	MapLayer* workingLayer = mapLayers_[layerIndex];

	// Get the largest layer height and layer width (used for parallax scrolling).
	int maxLayerWidth = mapLayer->getMaxCols() * tileSize_;
	int maxLayerHeight = mapLayer->getMaxRows() * tileSize_;

	int interactiveLayerWidth = interactiveMapLayer->getLayerCols() * tileSize_;
	int interactiveLayerHeight = interactiveMapLayer->getLayerRows() * tileSize_;

	// Calculate which rows and columns of the render grid layer are currently intersecting with the camera viewport.
	Rect cameraViewport;
	cameraViewport.x = camera_->getPosition()->getX();
	cameraViewport.y = camera_->getPosition()->getY();
	cameraViewport.w = camera_->getCameraWidth();
	cameraViewport.h = camera_->getCameraHeight();

	int layerWorldSpaceX = 0;
	int layerWorldSpaceY = 0;
			
	int layerCols = workingLayer->getLayerCols();
	int layerRows = workingLayer->getLayerRows();
	
	int gridCols = workingLayer->getGridCols();
	int gridRows = workingLayer->getGridRows();

	int layerWidth = layerCols * tileSize_;
	int layerHeight = layerRows * tileSize_;

	int cameraX = camera->getPosition()->getX();
	int cameraY = camera->getPosition()->getY();

    // Calculate the position of this layer in parallax, using the size of this layer vs the size 
	// of the interactive layer.
    if (interactiveLayerWidth == cameraViewport.w)
    {
        // If this layer is the same size as the camera, it's always going to located at coordinate 0 in world space.
        // Need to set this explicitly, otherwise it would result in a divide by zero error.
        layerWorldSpaceX = 0;
    }
    else
    {
        // How much bigger or smaller this layer is than the interactive layer
        double temp1 = interactiveLayerWidth - layerWidth;

        // How much bigger the interactive layer is than the camera.
        double temp2 = interactiveLayerWidth - cameraViewport.w;

        double scalingFactor = temp1 / temp2;

        layerWorldSpaceX = (int)(scalingFactor * (double)cameraX);
    }

    if (interactiveLayerHeight == cameraViewport.h)
    {
        layerWorldSpaceY = 0;
    }
    else
    {
        double temp1 = interactiveLayerHeight - layerHeight;
        double temp2 = interactiveLayerHeight - cameraViewport.h;
        double scalingFactor = temp1 / temp2;

        layerWorldSpaceY = (int)(scalingFactor * (double)cameraY);
    }
	
    // Calculate the subset of grid cells that are visible in the camera.    

	// Shift camera position so that it's coordinates are in the layerspace of the current layer.
	int cameraLayerSpaceX = cameraX - layerWorldSpaceX;
	int cameraLayerSpaceY = cameraY - layerWorldSpaceY;

	int extendedCells = 1;

	// Use this value to determine which cells the camera intersects with.
	int gridStartCellX = (std::abs(cameraLayerSpaceX) / cellSize_) - extendedCells;
	int gridStartCellY = (std::abs(cameraLayerSpaceY) / cellSize_) - extendedCells;
	int gridEndCellX = (std::abs(cameraLayerSpaceX + cameraViewport.w) / cellSize_) + extendedCells;
	int gridEndCellY = (std::abs(cameraLayerSpaceY + cameraViewport.h) / cellSize_) + extendedCells;

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