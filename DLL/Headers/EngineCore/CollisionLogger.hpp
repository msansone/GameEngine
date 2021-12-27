/* -------------------------------------------------------------------------
** CollisionLogger.hpp
**
** The CollisionLogger class is a debugging utility that allows the user to
** specify parameters to log collision events, such as entity IDs, hitbox IDs,
** hitbox identities, and collision types.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONLOGGER_HPP_
#define _COLLISIONLOGGER_HPP_

#include <boost/shared_ptr.hpp>

#include <set>

#include "CollisionLog.hpp"

namespace firemelon
{
	class CollisionLogger
	{
	public:
		CollisionLogger();
		virtual ~CollisionLogger();
		
		void	logCollision(CollisionLogPtr collisionLog);

		bool				logCollisions;

		// Log collisions of these types
		unsigned int		types;

		// Log collisions involving these entity IDs.
		std::set<int>		validEntityInstanceIds;

	private:


		// If true, a collision record will be included in the log only if both entity IDs are in the set.
		// If false, a collision record will be included in the log if only one of the entity IDs is in the set.
		bool				matchBothEntityIds;

		// Log collisions involving these hitbox IDs.
		std::set<int>		validHitboxIds;

		// If true, a collision record will be included in the log only if both hitbox IDs are in the set.
		// If false, a collision record will be included in the log if only one of the hitbox IDs is in the set.
		bool				matchBothHitboxIds;

		CollisionLogPtrList	loggedCollisions_;
	};

	typedef boost::shared_ptr<CollisionLogger> CollisionLoggerPtr;
}

#endif // _COLLISIONLOGGER_HPP_