/* -------------------------------------------------------------------------
** RenderableManager.hpp
**
** The RenderableManager class contains the list of all items that are stored
** in the render grid, and is responsible for updating and rendering them.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _RENDERABLEMANAGER_HPP_
#define _RENDERABLEMANAGER_HPP_

#include <queue>
#include <vector>
#include <cmath>

#include "CameraManager.hpp"
#include "Debugger.hpp"
#include "EntityReplicationState.hpp"
#include "Macros.hpp"
#include "ReplicationStateContainer.hpp"
#include "RoomRenderData.hpp"

namespace firemelon
{
	class RenderableManager
	{
	public:
		friend class GameEngine;
		friend class GameStateManager;
		friend class RoomManager;
		friend class Room;

		RenderableManager(DebuggerPtr debugger);
		virtual ~RenderableManager();
	
		void						addRenderable(int roomIndex, boost::shared_ptr<Renderable> renderable);
		void						removeRenderable(int roomIndex, int id);
		
		void						preInitialize(int roomIndex);
		void						initialize(int roomIndex);
		
		void						clear(int roomIndex);
		
		void						addRoom(RoomId roomId);

		void						addGridLayer(int roomIndex, int layerRows, int layerCols, int gridRows, int gridCols);
		
		int							getLayerCount(int roomIndex);

		void						setInteractiveLayer(int roomIndex, int value);
		int							getInteractiveLayer(int roomIndex);

		boost::shared_ptr<Position>	getLayerPosition(int roomIndex, int layerIndex);

		void						setTileSize(int tileSize);
		void						setGridCellSize(int cellSize);
		void						update(int roomIndex, double time);
		void						render(int roomIndex, double lerp);

		//void						getRelevantEntitiesToCamera(int roomIndex, boost::shared_ptr<CameraController> camera);

	private:
		
		void	cleanup();
		bool	containsCell(int row, int col, int startRow, int endRow, int startCol, int endCol);
		
		DebuggerPtr						debugger_;

		bool							runOnce_;
		int								cellSize_;
		int								tileSize_;
		int								screenWidth_;
		int								screenHeight_;


		AnimationManagerPtr								animationManager_;
		boost::shared_ptr<CameraManager>				cameraManager_;
		boost::shared_ptr<Renderer>						renderer_;
		std::vector<boost::shared_ptr<RoomRenderData>>	roomRenderDataList_;
	};
}

#endif // _RENDERABLEMANAGER_HPP_