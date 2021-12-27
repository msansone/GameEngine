/* -------------------------------------------------------------------------
** Vec2.hpp
** 
** The Vec2 class reprents a 2 dimensional vector. It is templated so it can
** represent different types.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _VEC2_HPP_
#define _VEC2_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "PythonGil.hpp"

namespace firemelon
{
	template <class T>
	class FIREMELONAPI Vec2
	{
	public:

		Vec2(T x, T y) { x_ = x; y_ = y; };

		~Vec2() {};
		
		T getXPy() 
		{ 
			PythonReleaseGil unlocker;

			return getX(); 
		};

		T getX() { return x_; };

		void setXPy(T value) 
		{ 
			PythonReleaseGil unlocker;

			setX(value); 
		};

		void setX(T value) { x_ = value; };
		
		T getYPy() 
		{
			PythonReleaseGil unlocker;

			return getY(); 
		};

		T getY() { return y_; };
		
		void setYPy(T value) 
		{ 
			PythonReleaseGil unlocker;
			
			setY(value);
		};

		void setY(T value) 
		{ 
			y_ = value; 
		};

	private:

		T		x_;

		T		y_;
	};

	typedef boost::shared_ptr<Vec2<float>>	Vec2FPtr;
	typedef boost::shared_ptr<Vec2<int>>	Vec2IPtr;
}

#endif // _VEC2_HPP_