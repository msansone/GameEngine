/* -------------------------------------------------------------------------
** EngineConfig.hpp
** 
** The EngineConfig class stores the engine configuration data, and provides
** an interface to modify it.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENGINECONFIG_HPP_
#define _ENGINECONFIG_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI EngineConfig
	{
	public:	

		EngineConfig();
		virtual ~EngineConfig();
	
		double	getFpsLimiterPy();
		double	getFpsLimiter();
		void	setFpsLimiterPy(double value);
		void	setFpsLimiter(double value);
		
		bool	getInterpolateFramesPy();
		bool	getInterpolateFrames();
		void	setInterpolateFramesPy(bool value);
		void	setInterpolateFrames(bool value);
		
	private:
	
		double	fpsLimiter_;

		bool	interpolateFrames_;
	};
}

#endif // _ENGINECONFIG_HPP_