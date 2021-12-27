/* -------------------------------------------------------------------------
** CollisionDispatcher.hpp
**
** CollisionDispatcher.hpp is an entity component that maintains a queue of 
** collision records for this entity, and dispatches them.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONDISPATCHER_HPP_
#define _COLLISIONDISPATCHER_HPP_

#include <vector>

#include <boost/python.hpp>

#include "CollisionRecord.hpp"
#include "CollisionTester.hpp"
#include "EntityComponents.hpp"
#include "HitboxManager.hpp"

namespace firemelon
{
	enum CollisionStatus
	{
		// No collision has occurred.
		COLLISION_DID_NOT_OCCUR = 0,
			
		// A collision has occurred, but a collision event hasn't been dispatched to the entities yet.
		COLLISION_OCCURRED_NOT_DISPATCHED = 1,

		// A collision has occurred and a collision event has already been dispatched to the entities during this update frame.
		COLLISION_OCCURRED_DISPATCHED = 2
	};

	struct CollisionRecords
	{
		std::vector<CollisionRecord>	collisionRecords_;
		
		// Collision tracking data. Used so that the correct event (Enter, Current, or Exit) is called.
		std::vector<CollisionRecord>	previousCollisionRecords_;
		
		#ifdef _USESTRINGKEY_
			std::map<std::string, CollisionStatus>	previousCollisionStatusMap_;	
		#else	
			std::map<CollisionRecord, CollisionStatus>	previousCollisionStatusMap_;	
		#endif
	};

	class CollisionDispatcher
	{
	public:
		friend class GameEngine;

		CollisionDispatcher(CollisionTesterPtr collisionTester, boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger);
		virtual ~CollisionDispatcher();
		
		void	addCollisionRecord(CollisionRecord collisionRecord);

		void	dispatch(int roomIndex);

		void	addRoom();
		
		void	prepareFrame(int roomIndex);
		void	removeCollisionRecord(int roomIndex, int ownerId);
		void	clear(int roomIndex);
		void	clearAll(int roomIndex);

	private:
		
		// Functor class used to find a collision record in a vector.
		class MatchesCollisionRecord
		{
		private:
			CollisionRecord collisionRecord_;

		public:
			MatchesCollisionRecord(const CollisionRecord &collisionRecord)
			{
				collisionRecord_ = collisionRecord;
			}
			
			bool operator()(const CollisionRecord &item) const
			{
				if (collisionRecord_.colliderId_ == item.colliderId_ &&
					collisionRecord_.colliderHitboxIdentity_ == item.colliderHitboxIdentity_ &&
					collisionRecord_.collideeId_ == item.collideeId_ &&
					collisionRecord_.collideeHitboxIdentity_ == item.collideeHitboxIdentity_)
				{
					return true;
				}
				
				return false;				
			}
		};
		
		void	collisionEnter(boost::shared_ptr<EntityComponents> requestingEntity,
							   boost::shared_ptr<EntityComponents> respondingEntity,
							   boost::shared_ptr<Hitbox> requestingEntityHitbox,
							   boost::shared_ptr<Hitbox> respondingEntityHitbox,
							   int requestingEntityHitboxId,
							   int respondingEntityHitboxId,
							   Vec2IPtr faceNormal,
							   Vec2IPtr intrusion);

		void	collisionExit(boost::shared_ptr<EntityComponents> requestingEntity,
							  boost::shared_ptr<EntityComponents> respondingEntity,
							  boost::shared_ptr<Hitbox> requestingEntityHitbox,
							  boost::shared_ptr<Hitbox> respondingEntityHitbox,
							  int requestingEntityHitboxId,
							  int respondingEntityHitboxId,
							  Vec2IPtr faceNormal,
							  Vec2IPtr intrusion);

		void	preCollisionExit(CollisionRecord collisionRecord);
		
		void	preCollisionEnter(CollisionRecord collisionRecord);

		// Extract the data from the CollisionRecord object and convert it into the data necessary to call the collision function.
		void	preProcessCollision(CollisionRecord collisionRecord);

		// Process the collision for the responding entity.
		void	processCollision(boost::shared_ptr<EntityComponents> respondingEntity,         boost::shared_ptr<EntityComponents> requestingEntity,
						         boost::shared_ptr<Hitbox>           respondingEntityHitbox,   boost::shared_ptr<Hitbox>           requestingEntityHitbox,
						         int                                 respondingEntityHitboxId, int                                 requestingEntityHitboxId,
						         Vec2IPtr                            faceNormal,               Vec2IPtr                            intrusion);


		CollisionLoggerPtr					collisionLogger_;
		CollisionTesterPtr					collisionTester_;
		DebuggerPtr							debugger_;
		boost::shared_ptr<HitboxManager>	hitboxManager_;
		boost::shared_ptr<BaseIds>			ids_;
		std::vector<CollisionRecords>		roomCollisionRecords_;
	};

	typedef boost::shared_ptr<CollisionDispatcher> CollisionDispatcherPtr;
}

#endif // _COLLISIONDISPATCHER_HPP_