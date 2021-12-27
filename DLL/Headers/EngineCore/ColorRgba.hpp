/* -------------------------------------------------------------------------
** ColorRgba.hpp
**
** The ColorRgba class represents a color, using red, green, blue, and alpha
** components, stored as floats which range from 0.0 to 1.0.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLORRGBA_HPP_
#define _COLORRGBA_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI ColorRgba
	{
	public:
		ColorRgba(float r, float g, float b, float a);
		virtual ~ColorRgba();
		
		static	boost::shared_ptr<ColorRgba> createPy(float r, float g, float b, float a);

		float	getRPy();
		float	getR();
		void	setRPy(float value);
		void	setR(float value);
		
		float	getGPy();
		float	getG();
		void	setGPy(float value);
		void	setG(float value);
		
		float	getBPy();
		float	getB();
		void	setBPy(float value);
		void	setB(float value);
		
		float	getAPy();
		float	getA();
		void	setAPy(float value);
		void	setA(float value);
		
	private:
		
		float r_;
		float g_;
		float b_;
		float a_;
	};

	typedef boost::shared_ptr<ColorRgba>	ColorRgbaPtr;
	typedef std::vector<ColorRgbaPtr>		ColorRgbaPtrList;
}

#endif // _COLORRGBA_HPP_