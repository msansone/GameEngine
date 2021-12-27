/* -------------------------------------------------------------------------
** Size.hpp
**
** The Size class represents the size of a rectangular region, using height
** and width.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SIZE_HPP_
#define _SIZE_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/shared_ptr.hpp>

#include <iostream>

#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI Size
	{
	public:
		friend class GameEngine;

		Size(int width, int height);
		virtual ~Size();

		int		getHeightPy();
		int		getHeight();

		int		getWidthPy();
		int		getWidth();

		void	setHeightPy(int height);
		void	setHeight(int height);

		void	setWidthPy(int width);
		void	setWidth(int width);

	private:

		int height_;

		int width_;

	};

	typedef boost::shared_ptr<Size> SizePtr;
}

#endif // _SIZE_HPP_