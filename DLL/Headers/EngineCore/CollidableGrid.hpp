/* -------------------------------------------------------------------------
** CollidableGrid.hpp
**
** The CollidableGrid class is used to partition the world into grid cells.
** Used so that only entities in close proximity to one another (i.e. in the 
** same grid cells) are tested against each other for collisions.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLIDABLEGRID_HPP_
#define _COLLIDABLEGRID_HPP_

#include <vector>

#include "GridCell.hpp"

namespace firemelon
{
	class CollidableGrid
	{
	public:
		friend class PhysicsManager;
		friend class PhysicsData;

		CollidableGrid();
		virtual ~CollidableGrid();
		
		void init(int cellSize, int rows, int cols);
		void addToGrid(int row, int col, int entityInstanceId);
		void removeFromGrid(int row, int col, int entityInstanceId);
		
		int getCollidableCount(int row, int col);
		int getCellSize();
		int getRows();
		int getCols();

		void clear();

	private:

		// The collidable grid. Each cell stores the collidables that intersect with it.
		std::vector<std::vector<std::vector<int>>> grid_;

		// List of cells that have data in them. No reason to bother with empty cells.
		std::vector<GridCell>	occupiedCells_;	

		int						rows_;
		int						cols_;

		int						cellSize_;
	};
}

#endif // _COLLIDABLEGRID_HPP_