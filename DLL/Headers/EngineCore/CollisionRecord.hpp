/* -------------------------------------------------------------------------
** CollisionRecord.hpp
** 
** The CollisionRecord class stores data that describes a collision between two 
** entities. It is used to keep track of collisions that have occurred during
** the collision detection phase.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONRECORD_HPP_
#define _COLLISIONRECORD_HPP_

#include <string>

#include "CollisionTestResult.hpp"
#include "EntityComponents.hpp"
#include "Hitbox.hpp"
#include "BaseIds.hpp"
#include "Types.hpp"
#include "Macros.hpp"

namespace firemelon
{
	class CollisionRecord
	{
	public:
		friend class CollisionDispatcher;
		friend class GameStateManager;
		friend class PhysicsManager;

		CollisionRecord();
		virtual ~CollisionRecord();
		
		#ifndef _USESTRINGKEY_
			friend bool operator<(const CollisionRecord& lhs, const CollisionRecord& rhs)
			{
				// Need to overload the < operator to be used in an std::map. std::map uses < rather than ==
				// because it is sorted.
				// Equivalence is defined as !(a<b) && !(b<a), i.e. neither is less than the other.

				return lhs.key_ < rhs.key_;
				
				//if (lhs.primaryId_ < rhs.primaryId_)
				//{
				//	return true;
				//}
				//else if (lhs.primaryId_ > rhs.primaryId_)
				//{
				//	return false;
				//}
			
				//// First level test is equivalent. Check the second level.			
				//if (lhs.primaryIdentity_ < rhs.primaryIdentity_)
				//{
				//	return true;
				//}
				//else if (lhs.primaryIdentity_ > rhs.primaryIdentity_)
				//{
				//	return false;
				//}

				//// Second level test is equivalent. Check the third level.			
				//if (lhs.secondaryId_ < rhs.secondaryId_)
				//{
				//	return true;
				//}
				//else if (lhs.secondaryId_ > rhs.secondaryId_)
				//{
				//	return false;
				//}
			
				//// Third level test is equivalent. Check the fourth level.			
				//if (lhs.secondaryIdentity_ < rhs.secondaryIdentity_)
				//{
				//	return true;
				//}
				//else if (lhs.secondaryIdentity_ > rhs.secondaryIdentity_)
				//{
				//	return false;
				//}

				//// These collision records are identical.
				//return false;
			};
		#endif

	private:
		
		// The room that this collision occurred in.
		RoomId								roomIndex_;
		
		int									colliderHitboxId_;
		int									collideeHitboxId_;

		Rect								colliderFullRect_;
		Rect								collideeFullRect_;

		unsigned short						colliderId_;
		unsigned short						collideeId_;

		boost::shared_ptr<EntityComponents>	collideeComponents_;
		boost::shared_ptr<EntityComponents>	colliderComponents_;

		HitboxIdentity						collideeHitboxIdentity_;
		HitboxIdentity						colliderHitboxIdentity_;

		CollisionTestResultPtr				collisionTestResult_;
		CollisionTestResultPtr				reverseCollisionTestResult_;

		bool								ignoreSecondCheck;
		

		#ifdef _USESTRINGKEY_
			// A unique key to identify this collision.
			std::string						key_;
		#else
			//// The primary and secondary fields are used for comparison of two collision records,
			//// because the collider and collidee IDs order is irrelevant.
			//// i. e. a record with {Collider ID = 1, Collidee ID = 2} is equivalent to {Collider ID = 2, Collidee ID = 1}
			//// because in both cases it describes a collision between the same entities.

			//// The primary ID will be the smaller of the two IDs.
			//int								primaryId_;
		
			//// The primary identity is the identity associated with the primary ID.
			//HitboxIdentity					primaryIdentity_;
		
			//// The secondary ID will be the larger of the two IDs.
			//int								secondaryId_;

			//// The secondary identity is the identity associated with the primary ID.
			//HitboxIdentity					secondaryIdentity_;

			unsigned long long int			key_;
		#endif
	};
}

#endif // _COLLISIONRECORD_HPP_