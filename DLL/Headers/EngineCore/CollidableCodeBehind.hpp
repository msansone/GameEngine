/* -------------------------------------------------------------------------
** CollidableCodeBehind.hpp
**
** CollidableCodeBehind is the base class which must be derived from to 
** implement the collidable functions for an entity.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLIDABLECODEBEHIND_HPP_
#define _COLLIDABLECODEBEHIND_HPP_

#include "CodeBehind.hpp"
#include "CollidableScript.hpp"
#include "TileCollisionData.hpp"
#include "DynamicsControllerHolder.hpp"
#include "HitboxControllerHolder.hpp"
#include "Types.hpp"
#include <limits.h>

namespace firemelon
{
	class CollidableCodeBehind : public CodeBehind
	{
	public:
		friend class CodeBehindContainer;
		friend class CollisionDispatcher;
		friend class GameStateManager;
		friend class PhysicsManager;

		CollidableCodeBehind();
		virtual ~CollidableCodeBehind();

	private:

		virtual void									collision(boost::shared_ptr<CollisionData> collisionData);

		virtual void									collisionEnter(boost::shared_ptr<CollisionData> collisionData);

		virtual void									collisionExit(boost::shared_ptr<CollisionData> collisionData);
		
		virtual boost::shared_ptr<CollisionData>		createCollisionData();

		virtual void									cleanup();

		virtual boost::shared_ptr<CollisionData>		getCollisionData(int hitboxIndex);

		virtual void									initialize();

		void											prepareFrame();

		void											preCollision(boost::shared_ptr<CollisionData> collisionData);

		void											preCollisionEnter(boost::shared_ptr<CollisionData> collisionData);

		void											preCollisionExit(boost::shared_ptr<CollisionData> collisionData);

		void											preSolidCollision(boost::shared_ptr<CollisionData> collisionData);

		virtual CollisionResolution						resolveCollision(boost::shared_ptr<CollisionData> collisionData);

		// Adjust the position of the entity. Need a better name for this because it's confusing that there is another function called "solidCollision"
		void											solidCollisionResponse(DynamicsController* dynamicsController, boost::shared_ptr<Hitbox> myHitbox, boost::shared_ptr<Hitbox> collidingWithHitbox, Vec2IPtr faceNormal, Vec2IPtr intrusion);

		// Like the collion, collisionEnter, and collisionLeave functions, but for solid collisions only.
		// Update: I don't think I need this because it can just be handled via the regular collision response functions.
		//virtual void									solidCollision(boost::shared_ptr<CollisionData> collisionData);

		void											setHitboxStatuses(HitboxIdentity identity, bool status);

		void											preCleanup();

		void											preInitialize();

		void											setTileSize(int tileSize);

		int												fromAboveCutoff_;
		int												fromBelowCutoff_;
		int												fromLeftCutoff_;
		int												fromRightCutoff_;

		bool											previousSlopeCollision_;
		bool											previousWorldGeometryCollision_;
		bool											slopeCollision_;
		bool											worldGeometryCollision_;
		
		int												tileHeight_;

		boost::shared_ptr<DynamicsControllerHolder>		dynamicsControllerHolder_;
		boost::shared_ptr<HitboxControllerHolder>		hitboxControllerHolder_;

		boost::shared_ptr<Position>						position_;

		boost::shared_ptr<BaseIds>						ids_;

		boost::shared_ptr<CollidableScript>				collidableScript_;
	};
}

#endif // _COLLIDABLECODEBEHIND_HPP_