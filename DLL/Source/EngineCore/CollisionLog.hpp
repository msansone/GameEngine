/* -------------------------------------------------------------------------
** CollisionLog.hpp
**
** The CollisionLogr class stores the data about a collision that gets saved
** to the collision logger.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONLOG_HPP_
#define _COLLISIONLOG_HPP_

#include <boost/shared_ptr.hpp>

#include "CollisionLog.hpp"

namespace firemelon
{
	class CollisionLog
	{
	public:
		CollisionLog();
		virtual ~CollisionLog();
		
	private:
		
	};

	typedef boost::shared_ptr<CollisionLog> CollisionLog;
}

#endif // _COLLISIONLOGGER_HPP_