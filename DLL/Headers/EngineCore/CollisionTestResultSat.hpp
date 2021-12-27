/* -------------------------------------------------------------------------
** CollisionTestResultSat.hpp
**
** The CollisionTestResultAabb class stores the result of an SAT collision
** test and any associated data. It is returned from a function implementing
** the SAT collision test logic.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONTESTRESULTSAT_HPP_
#define _COLLISIONTESTRESULTSAT_HPP_

#include "CollisionTestResult.hpp"

namespace firemelon
{
	class CollisionTestResultSat : public CollisionTestResult
	{
	public:

		CollisionTestResultSat();
		virtual ~CollisionTestResultSat();

	private:

	};

	typedef boost::shared_ptr<CollisionTestResultSat> CollisionTestResultSatPtr;
}

#endif // _COLLISIONTESTRESULTSAT_HPP_