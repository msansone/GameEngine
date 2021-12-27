/* -------------------------------------------------------------------------
** GridCell.hpp
**
** The GridCell class is a simple wrapper around a row and column property.
** It is used so that entities can keep track of which cells they are contained
** in, in both the render grid and the collision grid.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _GRIDCELL_HPP_
#define _GRIDCELL_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

namespace firemelon
{
	class FIREMELONAPI GridCell
	{
	public:
		GridCell();
		GridCell(int row, int column);
		virtual ~GridCell();
	
		int		getRow();
		int		getColumn();
		
		void	setRow(int row);
		void	setColumn(int column);

	private:

		int row_;
		int column_;
	};
}

#endif // _GRIDCELL_HPP_