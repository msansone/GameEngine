/* -------------------------------------------------------------------------
** SpriteData.hpp
**
** The SpriteData class stores the sprite data variables used by the 
** sprite manager. Each room has its own copy.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SPRITEDATA_HPP_
#define _SPRITEDATA_HPP_

#include <vector>

#include "StateMachineController.hpp"
#include "MapLayer.hpp"
#include "CameraController.hpp"

namespace firemelon
{
	class SpriteData
	{
	public:
		friend class RenderableManager;
		
		SpriteData();
		virtual ~SpriteData();
	
		void	clear();

	private:
		
		void	getVisibleGridCells(int layerIndex, int &startX, int &endX, int &startY, int &endY, int &layerX, int &layerY, double lerp);
		
		void	getVisibleGridCells(int layerIndex, int &startX, int &endX, int &startY, int &endY, CameraController* camera);

		int								cellSize_;
		int								tileSize_;

		CameraController*				camera_;

		int								interactiveLayer_;
		
		// A list of all the state machine controllers added from the game entities.
		std::vector<StateMachineController*>	stateMachineControllers_;

		// A list of state machine controllers, whose owner has a dynamics controller.
		std::vector<StateMachineController*>	dynamicsList_;
		
		// Stores any state machine controllers that exist outside the grid. Currently only HUD elements.
		std::vector<StateMachineController*>	nonGridSpriteList_;

		// Space partition grids, used so that only sprites in the camera's viewport are rendered.
		std::vector<MapLayer*>			mapLayers_;			

		// The positions of the layers, relative to the camera.
		std::vector<Position*>			layerPositions_;
		Position*						positionScreen_;
	};
}

#endif // _SPRITEDATA_HPP_