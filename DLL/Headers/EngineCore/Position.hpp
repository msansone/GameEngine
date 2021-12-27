/* -------------------------------------------------------------------------
** Position.hpp
**
** The Position class is a simple wrapper around a x, y coordinate location.
** It is used as an object that is shared amound the various subsystems, so
** they will all know the entity's current position.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _POSITION_HPP_
#define _POSITION_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <iostream>

#include "PythonGil.hpp"
#include "SimulationTracker.hpp"

namespace firemelon
{
	class FIREMELONAPI Position
	{
	public:
		friend class CameraController;
		friend class StageRenderable; // Need for faster execution in performance critical code.

		Position(int x, int y);
		virtual ~Position();

		void	initialize(int x, int y);

		int		getClampedX();
		int		getClampedY();

		int		getXPy();
		int		getX();
		
		int		getYPy();
		int		getY();
	
		void	setXPy(int value);
		void	setX(int value);
		
		void	setYPy(int value);
		void	setY(int value);

		int		getPreviousClampedX();
		int		getPreviousClampedY();
				
		int		getPreviousXPy();
		int		getPreviousX();
		int		getPreviousXForRender();

		int		getPreviousYPy();
		int		getPreviousY();
		int		getPreviousYForRender();

		void	setPreviousXPy(int value);
		void	setPreviousX(int value);

		void	setPreviousYPy(int value);
		void	setPreviousY(int value);

	private:

		int	clampXHi_;

		int	clampXLo_;

		int	clampYHi_;

		int	clampYLo_;

		int x_;
		int y_;

		int previousX_;
		int previousY_;
		int previousXForRender_;
		int previousYForRender_;
	};

	typedef boost::shared_ptr<Position> PositionPtr;
	typedef std::vector<PositionPtr> PositionPtrList;
}

#endif // _POSITION_HPP_