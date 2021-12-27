/* -------------------------------------------------------------------------
** CollisionTestResultAabb.hpp
**
** The CollisionTestResultAabb class stores the result of an AABB collision 
** test and any associated data. It is returned from a function implementing
** the AABB collision test logic.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONTESTRESULTAABB_HPP_
#define _COLLISIONTESTRESULTAABB_HPP_

#include "CollisionTestResult.hpp"
#include "Types.hpp"

namespace firemelon
{
	class CollisionTestResultAabb : public CollisionTestResult
	{
	public:

		CollisionTestResultAabb();
		virtual ~CollisionTestResultAabb();
		
	private:

	};

	typedef boost::shared_ptr<CollisionTestResultAabb> CollisionTestResultAabbPtr;
}

#endif // _COLLISIONTESTRESULTAABB_HPP_