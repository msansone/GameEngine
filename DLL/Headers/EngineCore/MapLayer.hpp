/* -------------------------------------------------------------------------
** MapLayer.hpp
**
** The MapLayer class represents the render grid for each layer. It contains
** an NxM grid of state machine controllers. Grid cells that are within the camera's
** viewport are looped through and the sprites that they contain are rendered.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _MAPLAYER_HPP_
#define _MAPLAYER_HPP_

#include <vector>
#include <list>

#include "Renderable.hpp"
#include "Types.hpp"
#include "GridCell.hpp"
#include "Macros.hpp"

namespace firemelon
{
	class MapLayer
	{
	public:
		friend class RenderableManager;

		MapLayer(int layerRows, int layerCols, int gridRows, int gridCols);
		virtual ~MapLayer();
	
		void setInteractive(bool value);
		bool getInteractive();

		void addToGrid(int row, int col, boost::shared_ptr<Renderable> renderable);
		void removeFromGrid(int row, int col, int id);

		int getLayerRows();
		int getLayerCols();
		int getGridRows();
		int getGridCols();

	private:

		// The render grid. Each cell stores the state machine controllers that intersect with it.
		std::vector<std::vector<std::vector<boost::shared_ptr<Renderable>>>> layerData_;

		// List of cells that have data in them. No reason to bother with empty cells.
		std::vector<GridCell>	occupiedCells_;	

		int						layerRows_;
		int						layerCols_;
		int						gridRows_;
		int						gridCols_;

		bool					isInteractive_;

		int						tileHW_;

		Rect					layerRect_;
	};
}

#endif // _MAPLAYER_HPP_