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

#include <set>

#include "CollisionLog.hpp"
#include "Hitbox.hpp"

namespace firemelon
{
	enum CollisionType
	{
		COLLISION_ENTER = 1,
		COLLISION_NORMAL = 2,
		COLLISION_EXIT = 4
	};

	class CollisionLog
	{
	public:
		CollisionLog();
		virtual ~CollisionLog();
		
		HitboxPtr		hitboxCollidee;

		int				hitboxCollideeId;

		int				hitboxCollideeOwnerId;

		HitboxPtr		hitboxCollider;

		int				hitboxColliderId;

		int				hitboxColliderOwnerId;

		Rect			rectCollidee;

		Rect			rectCollider;

		CollisionType	type;

	private:
		
	};

	typedef boost::shared_ptr<CollisionLog> CollisionLogPtr;
	typedef std::vector<CollisionLogPtr> CollisionLogPtrList;
}

#endif // _COLLISIONLOGGER_HPP_