#include "..\..\Headers\EngineCore\RenderableManager.hpp"

using namespace firemelon;

RenderableManager::RenderableManager(DebuggerPtr debugger)
{
	cellSize_ = 0;
	debugger_ = debugger;
	tileSize_ = 0;
	screenWidth_ = 0;
	screenHeight_ = 0;
	runOnce_ = false;
}

RenderableManager::~RenderableManager()
{
}

void RenderableManager::cleanup()
{
	roomRenderDataList_.clear();
}

void RenderableManager::clear(int roomIndex)
{
	roomRenderDataList_[roomIndex]->clear();
}

void RenderableManager::preInitialize(int roomIndex)
{
	roomRenderDataList_[roomIndex]->cellSize_ = cellSize_;
	roomRenderDataList_[roomIndex]->tileSize_ = tileSize_;
	roomRenderDataList_[roomIndex]->screenHeight_ = screenHeight_;
	roomRenderDataList_[roomIndex]->screenWidth_ = screenWidth_;
}

void RenderableManager::initialize(int roomIndex)
{
	roomRenderDataList_[roomIndex]->cameraManager_ = cameraManager_;
}

void RenderableManager::addRoom(RoomId roomId)
{
	boost::shared_ptr<RoomRenderData> roomRenderData = boost::shared_ptr<RoomRenderData>(new RoomRenderData(roomId));
	
	roomRenderDataList_.push_back(roomRenderData);
}

void RenderableManager::update(int roomIndex, double time)
{
	boost::shared_ptr<RoomRenderData> roomRenderData = roomRenderDataList_[roomIndex];

	// Update all static entities that are currently visible in the camera viewport.
	// also update all dynamic entities, regardless of visibility.

	// Rather than looping through every static entity, use the camera to determine which grid cells are in the viewport.
	// Loop through each of those and update them.

	// Build a list of renderables that are currently on screen and need to be updated.
	std::vector<boost::shared_ptr<Renderable>> updateList;

	// Keep track of sprites that were already flagged for an update.
	std::map<int, int> updateMap;

	int size = roomRenderData->dynamicsList_.size();
	
	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<Renderable> renderable = roomRenderData->dynamicsList_[i];

		if (renderable->isVisible_ == true)
		{
			renderable->updateRenderable(time);

			int layerIndex = renderable->mapLayerIndex_;

			int gridRows = 0;
			int gridCols = 0;

			if (layerIndex > -1)
			{
				// Get the grid bounds and calculate which grid cells this renderable intersects.
				gridCols = roomRenderData->mapLayers_[layerIndex]->getGridCols();
				gridRows = roomRenderData->mapLayers_[layerIndex]->getGridRows();

				unsigned int height = renderable->getHeight();

				// Dimensions must be at least 1x1
				if (height <= 0)
				{
					height = 1;
				}

				unsigned int width = renderable->getWidth();

				if (width <= 0)
				{
					width = 1;
				}

				int x = renderable->getX();
				int y = renderable->getY();

				int newStartCol = (int)(x / roomRenderData->cellSize_);

				if (newStartCol >= gridCols)
				{
					newStartCol = gridCols - 1;
				}
				else if (newStartCol < 0)
				{
					newStartCol = 0;
				}

				int newEndCol = (int)(((x + width) - 1) / roomRenderData->cellSize_);

				if (newEndCol >= gridCols)
				{
					newEndCol = gridCols - 1;
				}
				else if (newEndCol < 0)
				{
					newEndCol = 0;
				}

				int newStartRow = (int)(y / roomRenderData->cellSize_);

				if (newStartRow >= gridRows)
				{
					newStartRow = gridRows - 1;
				}
				else if (newStartRow < 0)
				{
					newStartRow = 0;
				}

				int newEndRow = (int)(((y + height) - 1) / roomRenderData->cellSize_);

				if (newEndRow >= gridRows)
				{
					newEndRow = gridRows - 1;
				}
				else if (newEndRow < 0)
				{
					newEndRow = 0;
				}

				if (renderable->id_ == 1156)
				{
					int a = renderable->id_;
					a++;
				}

				int oldStartRow = renderable->getGridCellStartRow();
				int oldEndRow = renderable->getGridCellEndRow();
				int oldStartCol = renderable->getGridCellStartCol();
				int oldEndCol = renderable->getGridCellEndCol();

				renderable->setGridCellStartRow(newStartRow);
				renderable->setGridCellEndRow(newEndRow);
				renderable->setGridCellStartCol(newStartCol);
				renderable->setGridCellEndCol(newEndCol);

				// If the intersecting cells have not changed, there's no need to update the grid.
				if (!(oldStartRow == newStartRow && oldEndRow == newEndRow && oldStartCol == newStartCol && oldEndCol == newEndCol))
				{
					// Do an intersection test of the old grid cells and the new. Any cell that is intersecting can be ignored.
					// Any old cell that is not intersecting must be removed, and any new cell that is not intersecting must be added.				
					int startRow = std::min(oldStartRow, newStartRow);
					int endRow = std::max(oldEndRow, newEndRow);
					int startCol = std::min(oldStartCol, newStartCol);
					int endCol = std::max(oldEndCol, newEndCol);

					for (int row = startRow; row <= endRow; row++)
					{
						for (int col = startCol; col <= endCol; col++)
						{
							bool isNewCell = containsCell(row, col, newStartRow, newEndRow, newStartCol, newEndCol);
							bool isOldCell = containsCell(row, col, oldStartRow, oldEndRow, oldStartCol, oldEndCol);

							// If the current cell being checked is a new cell, but not an old cell, it must be added.
							// If it is a new cell and an old cell, it should be ignored.
							// If it is not a new cell but it is an old cell, it should be removed.
							// If it is neither an old cell nor a new cell, it should be ignored.
							if (isNewCell == true && isOldCell == false)
							{
								roomRenderData->mapLayers_[layerIndex]->addToGrid(row, col, renderable);
							}
							else if (isNewCell == false && isOldCell == true)
							{
								int id = renderable->getRenderableId();

								roomRenderData->mapLayers_[layerIndex]->removeFromGrid(row, col, id);
							}
						}
					}
				}
			}
		}
	}

	// Update all non-grid renderables.
	size = roomRenderData->nonGridSpriteList_.size();
	
	for (int i = 0; i < size; i++) 		
	{
		boost::shared_ptr<Renderable> renderable = roomRenderData->nonGridSpriteList_[i];
		
		renderable->updateRenderable(time);
	}

	// Update all static renderables.
	size = roomRenderData->staticsList_.size();

	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<Renderable> renderable = roomRenderData->staticsList_[i];

		if (renderable->isVisible_ == true)
		{
			renderable->updateRenderable(time);
		}
	}

}

bool RenderableManager::containsCell(int row, int col, int startRow, int endRow, int startCol, int endCol)
{
	if (row < startRow || row > endRow || col < startCol || col > endCol)
	{
		return false;
	}

	return true;
}

void RenderableManager::addRenderable(int roomIndex, boost::shared_ptr<Renderable> renderable)
{
	boost::shared_ptr<RoomRenderData> roomRenderData = roomRenderDataList_[roomIndex];

	if (roomRenderData->renderables_.size() > 0)
	{
		// Add the renderable to the list, based on the render order.
		std::vector<boost::shared_ptr<Renderable>>::iterator itr;
		
		itr = std::lower_bound(roomRenderData->renderables_.begin(), roomRenderData->renderables_.end(), renderable, RenderableRenderOrderCompare()); 
		
		roomRenderData->renderables_.insert(itr, renderable);
	}
	else
	{
		roomRenderData->renderables_.push_back(renderable);
	}

	// If the renderable is dynamic, add it to a list to indicate this.
	// Dynamic renderables will need to be updated regardless of visibility in the camera viewport,
	// because their grid cells may change from frame to frame. If not add it to statics list.
	if (renderable->getIsDynamic() == true)
	{		
		// The dynamics list vector must be sorted.
		std::vector<boost::shared_ptr<Renderable>>::iterator itr;
		
		itr = std::lower_bound(roomRenderData->dynamicsList_.begin(), roomRenderData->dynamicsList_.end(), renderable, RenderableIdCompare()); 
		
		roomRenderData->dynamicsList_.insert(itr, renderable);
	}
	else
	{
		// The statics list vector must be sorted.
		std::vector<boost::shared_ptr<Renderable>>::iterator itr;

		itr = std::lower_bound(roomRenderData->staticsList_.begin(), roomRenderData->staticsList_.end(), renderable, RenderableIdCompare());

		roomRenderData->staticsList_.insert(itr, renderable);
	}
	
	int layer = renderable->mapLayerIndex_;

	// The layer position has to be added before the initialization, but the initialization has to happen
	// before it gets the bounding rect data to determine grid cells. That is why this conditional to check 
	// the layer happens twice.
	if (layer == -1)
	{
		renderable->layerPosition_ = roomRenderData->positionScreen_;
	}
	else if (layer > -1)
	{
		// The renderable needs to store a pointer to its layer's position object.
		renderable->layerPosition_ = roomRenderData->layerPositions_[layer];
	}

	renderable->initializeRenderable();

	// Add this sprite's index to the grid cells it occupies.
	unsigned int height = renderable->getHeight();

	// Dimensions must be at least 1x1
	if (height <= 0)
	{
		height = 1;
	}

	unsigned int width = renderable->getWidth();
	
	if (width <= 0)
	{
		width = 1;
	}

	int x = renderable->getX();
	int y = renderable->getY();

	if (layer == -1)
	{
		roomRenderData->nonGridSpriteList_.push_back(renderable);
	}
	else if (layer > -1)
	{
		// The renderable needs to store a pointer to its layer's position object.
		renderable->visibilityGridBounds_ = roomRenderData->layerVisibleGridCellBounds_[layer];

		int gridCols = roomRenderData->mapLayers_[layer]->getGridCols();
		int gridRows = roomRenderData->mapLayers_[layer]->getGridRows();

		// Now calculate which grid cells that this sprite intersects
		int startCol = (int)(x / roomRenderData->cellSize_);

		if (startCol >= gridCols)
		{
			startCol = gridCols - 1;
		}
		else if (startCol < 0)
		{
			startCol = 0;
		}

		int endCol = (int)(((x + width) - 1) / roomRenderData->cellSize_);
	
		if (endCol >= gridCols)
		{
			endCol = gridCols - 1;
		}
		else if (endCol < 0)
		{
			endCol = 0;
		}

		int startRow = (int)(y / roomRenderData->cellSize_);
	
		if (startRow >= gridRows)
		{
			startRow = gridRows - 1;
		}
		else if (startRow < 0)
		{
			startRow = 0;
		}

		int endRow = (int)(((y + height) - 1) / roomRenderData->cellSize_);
	
		if (endRow >= gridRows)
		{
			endRow = gridRows - 1;
		}
		else if (endRow < 0)
		{
			endRow = 0;
		}
		
		renderable->setGridCellStartRow(startRow);
		renderable->setGridCellEndRow(endRow);
		renderable->setGridCellStartCol(startCol);
		renderable->setGridCellEndCol(endCol);

		// Populate the list of grid cells this sprite is in, and add its index to the grid itself.		
		for (int row = startRow; row <= endRow; row++)	
		{
			for (int col = startCol; col <= endCol; col++)
			{
				roomRenderData->mapLayers_[layer]->addToGrid(row, col, renderable);

			}
		}
	}
}

void RenderableManager::removeRenderable(int roomIndex, int id)
{
	boost::shared_ptr<RoomRenderData> roomRenderData = roomRenderDataList_[roomIndex];

	// Find the renderable with the given ID.
	int size = roomRenderData->renderables_.size();
	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<Renderable> renderable = roomRenderData->renderables_[i];

		int renderableId = renderable->getRenderableId();

		if (renderableId == id)
		{
			int layer = renderable->mapLayerIndex_;

			if (layer > -1)
			{
				int startRow = renderable->getGridCellStartRow();
				int endRow = renderable->getGridCellEndRow();
				int startCol = renderable->getGridCellStartCol();
				int endCol = renderable->getGridCellEndCol();

				// Remove this renderable from the grid cells it occupies.				
				for (int j = startRow; j <= endRow; j++)
				{
					for (int k = startCol; k <= endCol; k++)
					{
						roomRenderData->mapLayers_[layer]->removeFromGrid(j, k, renderableId);
					}
				}
			}

			// If this renderable is in the dynamicsList, remove it from there first.

			// The dynamicsList vector should always be sorted, in order of the renderable's owner ID, 
			// because renderable ID's will only increase as new ones are added.
			
			// Find the lower bound. If the element found matches the value we are looking for, remove it.
			// If not, it was not in the list.
			std::vector<boost::shared_ptr<Renderable>>::iterator itr;
			
			itr = std::lower_bound(roomRenderData->dynamicsList_.begin(), roomRenderData->dynamicsList_.end(), roomRenderData->renderables_[i], RenderableIdCompare()); 

			if (itr != roomRenderData->dynamicsList_.end())
			{
				int lowerBoundPosition = itr - roomRenderData->dynamicsList_.begin();

				boost::shared_ptr<Renderable> foundRenderable = roomRenderData->dynamicsList_[lowerBoundPosition];

				int foundId = foundRenderable->getRenderableId();

				if (renderableId == foundId)
				{
					roomRenderData->dynamicsList_.erase(itr);				
				}
			}

			// The staticsList vector should always be sorted, in order of the renderable's owner ID, 
			// because renderable ID's will only increase as new ones are added.

			// Find the lower bound. If the element found matches the value we are looking for, remove it.
			// If not, it was not in the list.			
			itr = std::lower_bound(roomRenderData->staticsList_.begin(), roomRenderData->staticsList_.end(), roomRenderData->renderables_[i], RenderableIdCompare());

			if (itr != roomRenderData->staticsList_.end())
			{
				int lowerBoundPosition = itr - roomRenderData->staticsList_.begin();

				boost::shared_ptr<Renderable> foundRenderable = roomRenderData->staticsList_[lowerBoundPosition];

				int foundId = foundRenderable->getRenderableId();

				if (renderableId == foundId)
				{
					roomRenderData->staticsList_.erase(itr);
				}
			}

			// Also do the same thing for non grid sprites			
			itr = std::lower_bound(roomRenderData->nonGridSpriteList_.begin(), roomRenderData->nonGridSpriteList_.end(), roomRenderData->renderables_[i], RenderableIdCompare()); 
			
			if (itr != roomRenderData->nonGridSpriteList_.end())
			{
				int lowerBoundPosition = itr - roomRenderData->nonGridSpriteList_.begin();

				boost::shared_ptr<Renderable> foundRenderable = roomRenderData->nonGridSpriteList_[lowerBoundPosition];
				
				int foundId = foundRenderable->getRenderableId();

				if (renderableId == foundId)
				{
					roomRenderData->nonGridSpriteList_.erase(itr);				
				}
			}

			// Now remove the actual renderable from the main list.			
			roomRenderData->renderables_.erase(roomRenderData->renderables_.begin() + i);

			break;
		}
	}
}

void RenderableManager::addGridLayer(int roomIndex, int layerRows, int layerCols, int gridRows, int gridCols)
{
	boost::shared_ptr<RoomRenderData> roomRenderData = roomRenderDataList_[roomIndex];

	boost::shared_ptr<MapLayer> newLayer = boost::shared_ptr<MapLayer>(new MapLayer(layerRows, layerCols, gridRows, gridCols));

	roomRenderData->addLayer(newLayer);

	boost::shared_ptr<Position> layerPosition = boost::shared_ptr<Position>(new Position(0, 0));
	boost::shared_ptr<GridBounds> gridBounds = boost::shared_ptr<GridBounds>(new GridBounds());

	roomRenderData->layerPositions_.push_back(layerPosition);
	roomRenderData->layerVisibleGridCellBounds_.push_back(gridBounds);
}

void RenderableManager::setGridCellSize(int cellSize)
{
	cellSize_ = cellSize;
}

void RenderableManager::setTileSize(int tileSize)
{
	tileSize_ = tileSize;
}
	
int RenderableManager::getInteractiveLayer(int roomIndex)
{
	return roomRenderDataList_[roomIndex]->interactiveLayer_;
}

void RenderableManager::setInteractiveLayer(int roomIndex, int value)
{
	roomRenderDataList_[roomIndex]->interactiveLayer_ = value;
}

int RenderableManager::getLayerCount(int roomIndex)
{
	int size = roomRenderDataList_.size();

	if (roomIndex >= 0 && roomIndex < size)
	{
		boost::shared_ptr<RoomRenderData> roomRenderData = roomRenderDataList_[roomIndex];

		return roomRenderData->layerPositions_.size();
	}
	else
	{
		return 0;
	}
}

boost::shared_ptr<Position> RenderableManager::getLayerPosition(int roomIndex, int layerIndex)
{
	boost::shared_ptr<RoomRenderData> roomRenderData = roomRenderDataList_[roomIndex];

	int size = roomRenderData->layerPositions_.size();
	if (layerIndex > 0 && layerIndex < size)
	{
		return roomRenderData->layerPositions_[layerIndex];
	}
	else
	{
		boost::shared_ptr<Position> p = boost::shared_ptr<Position>(new Position(0, 0));

		return p;
	}
}

void RenderableManager::render(int roomIndex, double lerp)
{
	// Iterate through the render grid for each layer and call the render function for the renderable in each grid cell.
	boost::shared_ptr<RoomRenderData> roomRenderData = roomRenderDataList_[roomIndex];

	int size = roomRenderData->mapLayers_.size();
	for (int layerIndex = 0; layerIndex < size; layerIndex++)
	{
		boost::shared_ptr<MapLayer> layer = roomRenderData->mapLayers_[layerIndex];

		// BUG001: Originally this was moved into the simulation update, because I thought it doesn't need to happen each render.
		// however I later found that I was wrong and it does need to be per render. See other comments for this bug number.
		roomRenderData->populateLayerRenderData(layerIndex, lerp);

		boost::shared_ptr<GridBounds> bounds = roomRenderData->layerVisibleGridCellBounds_[layerIndex];

		int gridStartCellX = bounds->getStartX();
		int gridEndCellX = bounds->getEndX();
		int gridStartCellY = bounds->getStartY();
		int gridEndCellY = bounds->getEndY();

		// Determine which is smaller, the number of occupied grid cells, or the number of grid cells visible on screen.
		int visibleCellCount = (gridEndCellX - gridStartCellX + 1) * (gridEndCellY - gridStartCellY + 1);
		int occupiedCellCount = layer->occupiedCells_.size();

		if (visibleCellCount < occupiedCellCount)
		{
			for (int i = gridStartCellY; i <= gridEndCellY; i++)
			{
				for (int j = gridStartCellX; j <= gridEndCellX; j++)
				{
					int renderableCount = layer->layerData_[i][j].size();

					for (int k = 0; k < renderableCount; k++)
					{
						boost::shared_ptr<Renderable> r = layer->layerData_[i][j][k];

						if (r->isVisible_ == true) // && r->hasRendered_ == false)
						{
							//r->render(lerp);

							r->doRender_ = true;

							//renderCount++;

							//r->hasRendered_ = true;
						}
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < occupiedCellCount; j++)
			{
				GridCell gc = layer->occupiedCells_[j];

				int gridCellX = gc.getColumn();
				int gridCellY = gc.getRow();

				// If this is one of the visisble cells, render all of the renderables in it.
				if (gridCellX >= gridStartCellX && gridCellX <= gridEndCellX && gridCellY >= gridStartCellY && gridCellY <= gridEndCellY)
				{
					int renderableCount = layer->layerData_[gridCellY][gridCellX].size();

					for (int k = 0; k < renderableCount; k++)
					{
						boost::shared_ptr<Renderable> r = layer->layerData_[gridCellY][gridCellX][k];

						if (r->isVisible_ == true) // && r->hasRendered_ == false)
						{
							//r->render(lerp);

							r->doRender_ = true;

							//renderCount++;
							
							//r->hasRendered_ = true;
						}
					}
				}
			}
		}

	//////////	if (visibleCellCount < occupiedCellCount)
	//////////	{
	//////////		for (int i = gridStartCellY; i <= gridEndCellY; i++)
	//////////		{
	//////////			for (int j = gridStartCellX; j <= gridEndCellX; j++)
	//////////			{
	//////////				int renderableCount = layer->layerData_[i][j].size();

	//////////				for (int k = 0; k < renderableCount; k++)
	//////////				{
	//////////					boost::shared_ptr<Renderable> r = layer->layerData_[i][j][k];

	//////////					r->hasRendered_ = false;
	//////////				}
	//////////			}
	//////////		}
	//////////	}
	//////////	else
	//////////	{
	//////////		for (int j = 0; j < occupiedCellCount; j++)
	//////////		{
	//////////			GridCell gc = layer->occupiedCells_[j];

	//////////			int gridCellX = gc.getColumn();
	//////////			int gridCellY = gc.getRow();

	//////////			// If this is one of the visisble cells, render all of the renderables in it.
	//////////			if (gridCellX >= gridStartCellX && gridCellX <= gridEndCellX && gridCellY >= gridStartCellY && gridCellY <= gridEndCellY)
	//////////			{
	//////////				int renderableCount = layer->layerData_[gridCellY][gridCellX].size();

	//////////				for (int k = 0; k < renderableCount; k++)
	//////////				{
	//////////					boost::shared_ptr<Renderable> r = layer->layerData_[gridCellY][gridCellX][k];

	//////////					r->hasRendered_ = false;
	//////////				}
	//////////			}
	//////////		}
	//////////	}
	}

	//std::cout << renderCount << std::endl;


	//// Iterate through the render grid for each layer and call the render function for the renderable in each grid cell.
	//boost::shared_ptr<RoomRenderData> roomRenderData = roomRenderDataList_[roomIndex];

	////roomRenderData->sceneGraph_.clear();

	//int size = roomRenderData->mapLayers_.size();
	//for (int i = 0; i < size; i++)
	//{
	//	roomRenderData->populateLayerRenderData(i, lerp);

	//	boost::shared_ptr<MapLayer> layer = roomRenderData->mapLayers_[i];

	//	int occupiedCellCount = layer->occupiedCells_.size();

	//	boost::shared_ptr<GridBounds> bounds = roomRenderData->layerVisibleGridCellBounds_[i];

	//	int startCol = bounds->getStartX();
	//	int endCol = bounds->getEndX();

	//	int startRow = bounds->getStartY();
	//	int endRow = bounds->getEndY();

	//	for (int j = 0; j < occupiedCellCount; j++)
	//	{
	//		GridCell gc = layer->occupiedCells_[j];

	//		int col = gc.getColumn();
	//		int row = gc.getRow();

	//		// If this is one of the visisble cells, render all of the renderables in it.
	//		if (col >= startCol && col <= endCol && row >= startRow && row <= endRow)
	//		{
	//			int renderableCount = layer->layerData_[row][col].size();

	//			for (int k = 0; k < renderableCount; k++)
	//			{
	//				boost::shared_ptr<Renderable> r = layer->layerData_[row][col][k];
	//				r->doRender_ = r->isVisible_;
	//			}
	//		}
	//	}
	//}

	// Handle any renderables outside of the grid.
	size = roomRenderData->nonGridSpriteList_.size();

	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<Renderable> r = roomRenderData->nonGridSpriteList_[i];

		if (r->isVisible_ == true)
		{
			//r->render(lerp);
			r->doRender_ = true;
		}
	}

	// Loop through the renderables and render all that have been flagged as within the camera's viewport.
	bool renderStageDebugInfo = debugger_->getDebugMode() == true && debugger_->getDebugModeRenderStage() == true;

	size = roomRenderData->renderables_.size();
	
	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<Renderable> r = roomRenderData->renderables_[i];

#if defined(_DEBUG)	

		if (r->getRenderableId() == 44)
		{
			bool debug = true;
		}

#endif

		if (r->doRender_ == true)
		{
			// If rendering debug data, force this to true, for the second render loop.
			r->doRender_ = false || renderStageDebugInfo;

			r->render(lerp);
		}
	}

	if (renderStageDebugInfo == true)
	{
		for (int i = 0; i < size; i++)
		{
			boost::shared_ptr<Renderable> r = roomRenderData->renderables_[i];

			if (r->doRender_ == true)
			{
				// If rendering anchor points, force this to true, for the second render loop.
				r->doRender_ = false;

				r->renderDebugData(lerp);
			}
		}
	}
}
