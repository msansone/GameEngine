/* -------------------------------------------------------------------------
** RoomRenderData.hpp
**
** The RoomRenderData class stores the data necessary to render a room.
** This includes the list of renderable items, the camera, and the render grid.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ROOMRENDERDATA_HPP_
#define _ROOMRENDERDATA_HPP_

#include <vector>

#include "CameraController.hpp"
#include "CameraManager.hpp"
#include "GridBounds.hpp"
#include "MapLayer.hpp"
#include "Renderable.hpp"

namespace firemelon
{
	class RoomRenderData
	{
	public:
		friend class GameStateManager;
		friend class RenderableManager;
		
		RoomRenderData(RoomId roomId);
		virtual ~RoomRenderData();
	
		void	clear();

		void	populateLayerRenderData(int layerIndex, double lerp);

		// Space partition grids, used so that only sprites in the camera's viewport are rendered.
		std::vector<boost::shared_ptr<MapLayer>>	mapLayers_;

	private:
		
		void	cleanup();
				
		void	getVisibleGridCells(int layerIndex, int &startX, int &endX, int &startY, int &endY, boost::shared_ptr<CameraController> camera);

		void	addLayer(boost::shared_ptr<MapLayer> layer);

		int									cellSize_;
		int									tileSize_;

		int									screenWidth_;
		int									screenHeight_;

		int									maxLayerRows_;
		int									maxLayerCols_;

		boost::shared_ptr<CameraManager>	cameraManager_;

		int									interactiveLayer_;
		
		// A list of all the renderables.
		std::vector<boost::shared_ptr<Renderable>>	renderables_;
		
		// A list of renderables who may move into different render grid cells, or have an animation that needs updating.
		std::vector<boost::shared_ptr<Renderable>>	dynamicsList_;

		// A list of renderables that will never move into different render grid cells, but may have an animation that needs updating.
		std::vector<boost::shared_ptr<Renderable>>	staticsList_;
		
		// Stores any sprites that exist outside the grid. Currently only HUD elements.
		std::vector<boost::shared_ptr<Renderable>>	nonGridSpriteList_;

		// A list of all the renderables which gets rebuilt every frame.
		std::vector<boost::shared_ptr<Renderable>>	sceneGraph_;


		// The positions of the layers, relative to the camera.
		std::vector<boost::shared_ptr<Position>>	layerPositions_;
		std::vector<boost::shared_ptr<GridBounds>>	layerVisibleGridCellBounds_;
		boost::shared_ptr<Position>					positionScreen_;

		RoomId										roomId_;

		Rect										cameraViewport_;

		int											previousCameraX_;
		int											previousCameraY_;

		int											currentCameraX_;
		int											currentCameraY_;
	};
}

#endif // _ROOMRENDERDATA_HPP_