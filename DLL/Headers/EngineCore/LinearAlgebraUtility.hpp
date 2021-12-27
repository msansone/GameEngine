/* -------------------------------------------------------------------------
** LinearAlgebraUtility.hpp
**
** A utility class for performing vector operations and linear transforms.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _LINEARALGEBRAUTILITY_HPP_
#define _LINEARALGEBRAUTILITY_HPP_

#include <vector>

#include <boost/shared_ptr.hpp>

#include "math.h"

#include "Types.hpp"

namespace firemelon
{
	class LinearAlgebraUtility
	{
	public:

		LinearAlgebraUtility();
		virtual ~LinearAlgebraUtility();

		double	dot(Vertex2 &vector1, Vertex2 &vector2);

		double	length(Vertex2 &vector);

		void	normalize(Vertex2 &vector);

		Vertex2 projection(Vertex2 &vector1, Vertex2 &vector2);

		void	rotatePoint(float rotationAngle, Vertex2 pointToRotate, Vertex2 &rotatedPoint, int originDeltaX, int originDeltaY);

		void	rotatePoints(float rotationAngle, std::vector<Vertex2> pointsToRotate, std::vector<Vertex2> &rotatedPoints, int originDeltaX, int originDeltaY);

		void	mirrorPointHorizontally(Vertex2 &pointsToMirror, int originDeltaX);

		void	mirrorPointsHorizontally(std::vector<Vertex2> &pointsToMirror, int originDeltaX);

	private:
		
	};

	typedef boost::shared_ptr<LinearAlgebraUtility> LinearAlgebraUtilityPtr;
	typedef std::vector<LinearAlgebraUtilityPtr> LinearAlgebraUtilityPtrList;
}

#endif // _LINEARALGEBRAUTILITY_HPP_