#include "..\..\Headers\EngineCore\RoomRenderData.hpp"

using namespace firemelon;

RoomRenderData::RoomRenderData(RoomId roomId)
{
	interactiveLayer_ = -1;
	cellSize_ = 0;
	tileSize_ = 0;
	screenWidth_ = 0;
	screenHeight_ = 0;

	maxLayerRows_ = 0;
	maxLayerCols_ = 0;

	roomId_ = roomId;

	previousCameraX_ = 0;
	previousCameraY_ = 0;

	currentCameraX_ = 0;
	currentCameraY_ = 0;

	// A position object for entities that overlay the map.
	positionScreen_ = boost::shared_ptr<Position>(new Position(0, 0));
}

RoomRenderData::~RoomRenderData()
{
}

void RoomRenderData::cleanup()
{
	layerVisibleGridCellBounds_.clear();
}

void RoomRenderData::addLayer(boost::shared_ptr<MapLayer> layer)
{
	mapLayers_.push_back(layer);

	int rows = layer->getLayerRows();

	if (rows > maxLayerRows_)
	{
		maxLayerRows_ = rows;
	}

	int cols = layer->getLayerCols();

	if (cols > maxLayerCols_)
	{
		maxLayerCols_ = cols;
	}

}

void RoomRenderData::populateLayerRenderData(int layerIndex, double lerp)
{
	boost::shared_ptr<MapLayer> mapLayer = mapLayers_[0];
	boost::shared_ptr<MapLayer> interactiveMapLayer = mapLayers_[interactiveLayer_];
	boost::shared_ptr<MapLayer> workingLayer = mapLayers_[layerIndex];

	// Get the largest layer height and layer width (used for parallax scrolling).
	int maxLayerWidth = maxLayerCols_ * tileSize_;
	int maxLayerHeight = maxLayerRows_ * tileSize_;

	int interactiveLayerWidth = interactiveMapLayer->getLayerCols() * tileSize_;
	int interactiveLayerHeight = interactiveMapLayer->getLayerRows() * tileSize_;

	// Calculate which rows and columns of the render grid layer are currently intersecting with the camera viewport.	
	boost::shared_ptr<CameraController> camera = cameraManager_->getActiveCamera();

	if (camera != nullptr)
	{
		if (camera->getMetadata()->getRoomMetadata()->getRoomId() == roomId_)
		{
			// Only update the camera viewport if the active camera matches the room being rendered.
			cameraViewport_.x = camera->getPosition()->getClampedX();
			cameraViewport_.y = camera->getPosition()->getClampedY();

			cameraViewport_.w = camera->getCameraWidth();
			cameraViewport_.h = camera->getCameraHeight();
		}
	}
	else
	{
		cameraViewport_.x = 0;
		cameraViewport_.y = 0;
		cameraViewport_.w = screenWidth_;
		cameraViewport_.h = screenHeight_;
	}

	int layerWorldSpaceX = 0;
	int layerWorldSpaceY = 0;

	int layerCols = workingLayer->getLayerCols();
	int layerRows = workingLayer->getLayerRows();

	int gridCols = workingLayer->getGridCols();
	int gridRows = workingLayer->getGridRows();

	int layerWidth = layerCols * tileSize_;
	int layerHeight = layerRows * tileSize_;

	if (camera != nullptr)
	{
		if (camera->getMetadata()->getRoomMetadata()->getRoomId() == roomId_)
		{
			// Only update the previous position data if the active camera matches the room being rendered.
			previousCameraX_ = camera->getPosition()->getPreviousClampedX();
			previousCameraY_ = camera->getPosition()->getPreviousClampedY();

			currentCameraX_ = camera->getPosition()->getClampedX();
			currentCameraY_ = camera->getPosition()->getClampedY();
		}
	}
	else
	{
		previousCameraX_ = 0;
		previousCameraY_ = 0;

		currentCameraX_ = 0;
		currentCameraY_ = 0;
	}

	// Setting the actual positoin in the world using a LERP is wrong. LERPING is only for rendering.
	// However, this function gets called every RENDER, not every update, as it is populating RENDER DATA.
	// So, maybe it is correct to set the layer offset using a lerp? Or, should it be lerping the camera position in the renderable render functions?
	
	//int cameraX = previousCameraX_ + (int)floor((lerp * (currentCameraX_ - previousCameraX_)));
	//int cameraY = previousCameraY_ + (int)floor((lerp * (currentCameraY_ - previousCameraY_)));
	int cameraX = currentCameraX_;
	int cameraY = currentCameraY_;

    // Calculate the position of this layer in parallax, using the size of this layer vs the size 
	// of the interactive layer.
    if (interactiveLayerWidth == cameraViewport_.w)
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
        double temp2 = interactiveLayerWidth - cameraViewport_.w;

        double scalingFactor = temp1 / temp2;

        layerWorldSpaceX = (int)(scalingFactor * (double)cameraX);
    }

    if (interactiveLayerHeight == cameraViewport_.h)
    {
        layerWorldSpaceY = 0;
    }
    else
    {
        double temp1 = interactiveLayerHeight - layerHeight;
        double temp2 = interactiveLayerHeight - cameraViewport_.h;
        double scalingFactor = temp1 / temp2;

        layerWorldSpaceY = (int)(scalingFactor * (double)cameraY);
    }
	
	int previousX = layerPositions_[layerIndex]->getX();
	int previousY = layerPositions_[layerIndex]->getY();

	layerPositions_[layerIndex]->setPreviousX(previousX);
	layerPositions_[layerIndex]->setPreviousY(previousY);
	
	layerPositions_[layerIndex]->setX(layerWorldSpaceX - cameraX);
	layerPositions_[layerIndex]->setY(layerWorldSpaceY - cameraY);
	
    // Calculate the subset of grid cells that are visible in the camera.    

	// Shift camera position so that it's coordinates are in the layerspace of the current layer.
	int cameraLayerSpaceX = cameraX - layerWorldSpaceX;
	int cameraLayerSpaceY = cameraY - layerWorldSpaceY;
	
	// Use this value to determine which cells the camera intersects with.
	int gridStartCellX = (std::abs(cameraLayerSpaceX) / cellSize_);
	int gridStartCellY = (std::abs(cameraLayerSpaceY) / cellSize_);
	int gridEndCellX = (std::abs(cameraLayerSpaceX + cameraViewport_.w) / cellSize_);
	int gridEndCellY = (std::abs(cameraLayerSpaceY + cameraViewport_.h) / cellSize_);

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

	layerVisibleGridCellBounds_[layerIndex]->setStartX(gridStartCellX);
	layerVisibleGridCellBounds_[layerIndex]->setEndX(gridEndCellX);
	layerVisibleGridCellBounds_[layerIndex]->setStartY(gridStartCellY);
	layerVisibleGridCellBounds_[layerIndex]->setEndY(gridEndCellY);
}

void RoomRenderData::clear()
{
	dynamicsList_.clear();
	staticsList_.clear();
	layerVisibleGridCellBounds_.clear();
	layerPositions_.clear();
	mapLayers_.clear();	
	nonGridSpriteList_.clear();
	renderables_.clear();
}


void RoomRenderData::getVisibleGridCells(int layerIndex, int &startX, int &endX, int &startY, int &endY, boost::shared_ptr<CameraController> camera)
{
	boost::shared_ptr<MapLayer> mapLayer = mapLayers_[0];
	boost::shared_ptr<MapLayer> interactiveMapLayer = mapLayers_[interactiveLayer_];
	boost::shared_ptr<MapLayer> workingLayer = mapLayers_[layerIndex];

	// Get the largest layer height and layer width (used for parallax scrolling).
	int maxLayerWidth = maxLayerCols_ * tileSize_;
	int maxLayerHeight = maxLayerRows_ * tileSize_;

	int interactiveLayerWidth = interactiveMapLayer->getLayerCols() * tileSize_;
	int interactiveLayerHeight = interactiveMapLayer->getLayerRows() * tileSize_;

	boost::shared_ptr<CameraController> activeCamera = cameraManager_->getActiveCamera();

	// Calculate which rows and columns of the render grid layer are currently intersecting with the camera viewport.
	Rect cameraViewport;

	cameraViewport.x = activeCamera->getPosition()->getClampedX();
	cameraViewport.y = activeCamera->getPosition()->getClampedY();
	cameraViewport.w = activeCamera->getCameraWidth();
	cameraViewport.h = activeCamera->getCameraHeight();

	int layerWorldSpaceX = 0;
	int layerWorldSpaceY = 0;
			
	int layerCols = workingLayer->getLayerCols();
	int layerRows = workingLayer->getLayerRows();
	
	int gridCols = workingLayer->getGridCols();
	int gridRows = workingLayer->getGridRows();

	int layerWidth = layerCols * tileSize_;
	int layerHeight = layerRows * tileSize_;

	int cameraX = camera->getPosition()->getClampedX();
	int cameraY = camera->getPosition()->getClampedY();

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