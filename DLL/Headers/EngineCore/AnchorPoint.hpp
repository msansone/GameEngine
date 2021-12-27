/* -------------------------------------------------------------------------
** AnchorPoint.hpp
**
** The AnchorPoint class stores a name and associated 2D point. Anchor points
** are contained in frames and are used for various purposes, such as spawning
** new entities at their location.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ANCHORPOINT_HPP_
#define _ANCHORPOINT_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>

#include "Position.hpp"
#include "PythonGil.hpp"
#include "Types.hpp"

namespace firemelon
{	
	class FIREMELONAPI AnchorPoint
	{
	public:
		friend class StageElements;
		friend class StageRenderable;

		AnchorPoint(std::string name, int left, int top);
		virtual ~AnchorPoint();
		
		std::string	getNamePy();
		std::string	getName();
		
		int			getXPy();
		int			getX();

		int			getYPy();
		int			getY();

	private:

		void	initialize(std::string name, int left, int top);

		Vertex2						nativePosition_;

		Vertex2						transformedPosition_;

		boost::shared_ptr<Position>	point_;

		std::string					name_;
	};

	typedef boost::shared_ptr<AnchorPoint>	AnchorPointPtr;	
}

#endif // _ANCHORPOINT_HPP_