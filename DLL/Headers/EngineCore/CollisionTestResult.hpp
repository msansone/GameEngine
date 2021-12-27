/* -------------------------------------------------------------------------
** CollisionTestResult.hpp
**
** The CollisionTestResult class stores the result of a collision test and
** any associated data. It is intended to be used as an abtract base class,
** with derived child classes for specific collision tests, such as AABB
** (Axis-aligned bounding boxes) or SAT (Separating Axis Theorem) collision
** tests.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONTESTRESULT_HPP_
#define _COLLISIONTESTRESULT_HPP_

#include <boost/shared_ptr.hpp>

#include "Vec2.hpp"

namespace firemelon
{
	class CollisionTestResult
	{
	public:

		CollisionTestResult();
		virtual ~CollisionTestResult();

		bool		collisionOccurred;

		Vec2IPtr	faceNormal;

		Vec2IPtr	intrusion;

	private:

	};

	typedef boost::shared_ptr<CollisionTestResult> CollisionTestResultPtr;
}

#endif // _COLLISIONTESTRESULT_HPP_