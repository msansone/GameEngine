/* -------------------------------------------------------------------------
** GridBounds.hpp
**
** The GridBounds class stores the upper and lower bounds of a two dimensional
** grid. Used to determine which cells are visible.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _GRIDBOUNDS_HPP_
#define _GRIDBOUNDS_HPP_

namespace firemelon
{
	class GridBounds
	{
	public:
		
		GridBounds();
		virtual ~GridBounds();
	
		void			setStartX(unsigned int value);
		unsigned int	getStartX();

		void			setStartY(unsigned int value);
		unsigned int	getStartY();

		void			setEndX(unsigned int value);
		unsigned int	getEndX();

		void			setEndY(unsigned int value);
		unsigned int	getEndY();

	private:
		
		unsigned int startX_;
		unsigned int startY_;
		unsigned int endX_;
		unsigned int endY_;
	};
}

#endif // _GRIDBOUNDS_HPP_