#include "..\..\Headers\EngineCore\CollidableCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

CollidableCodeBehind::CollidableCodeBehind()
{
	tileHeight_ = 16;
	previousSlopeCollision_ = false;
	previousWorldGeometryCollision_ = false;
	worldGeometryCollision_ = false;
	
	fromAboveCutoff_ = INT_MAX;
	fromBelowCutoff_ = 0;
	fromLeftCutoff_ = INT_MAX;
	fromRightCutoff_ = 0;
}

CollidableCodeBehind::~CollidableCodeBehind()
{
}

void CollidableCodeBehind::setTileSize(int tileSize)
{
	tileHeight_ = tileSize;
}

void CollidableCodeBehind::prepareFrame()
{
	// Start the update frame with the isTileCollision_ variable set to false.
	// Then, in the collision code, it may get set to true.
	// Before doing this though, store the previous value from the previous update frame.
	previousSlopeCollision_ = slopeCollision_;
	previousWorldGeometryCollision_ = worldGeometryCollision_;

	slopeCollision_ = false;
	worldGeometryCollision_ = false;

	fromAboveCutoff_ = INT_MAX;
	fromBelowCutoff_ = 0;
	fromLeftCutoff_ = INT_MAX;
	fromRightCutoff_ = 0;

	// Also start the update frame with the below cutoff at the maximum signed integer value. (Should I change it to unsigned?)

	// Clear the collision statuses of all active hitboxes.
	auto hitboxController = hitboxControllerHolder_->getHitboxController();

	int activeHitboxCount = hitboxController->getActiveHitboxCount();

	for (int i = 0; i < activeHitboxCount; i++)
	{
		int hitboxId = hitboxController->getActiveHitboxId(i);

		hitboxController->setActiveHitboxCollisionStatus(hitboxId, false);		
	}
}

boost::shared_ptr<CollisionData> CollidableCodeBehind::createCollisionData()
{
	return boost::shared_ptr<CollisionData>(new CollisionData());
}

void CollidableCodeBehind::preCollision(boost::shared_ptr<CollisionData> collisionData)
{
	bool collisionValid = false;

	// Any other active hitbox with the same identity should share the status.
	boost::shared_ptr<Hitbox> myHitbox = collisionData->getMyHitbox();

	HitboxIdentity myHitboxIdentity = myHitbox->getIdentity();
	boost::shared_ptr<Hitbox> collidingWithHitbox = collisionData->getCollidingHitbox();
	HitboxIdentity collidingWithIdentity = collidingWithHitbox->getIdentity();

	int myPositionY = position_->getY();
	int myPositionX = position_->getX();

	float slopeAngle = collidingWithHitbox->getSlope();

	CollisionStyle collisionStyle = collidingWithHitbox->getCollisionStyle();

	bool isSolidTileCollision = myHitbox->getIsSolid() == true && collidingWithIdentity == ids_->HITBOX_WORLDGEOMETRY;

	bool isOneWayCollision = false;

	bool collisionFromAbove = (collisionData->getFaceNormal()->getY() == -1 && collisionData->getFaceNormal()->getX() == 0);

	if (collisionStyle == COLLISION_STYLE_ONE_WAY_TOP && collisionFromAbove == true)
	{
		isOneWayCollision = true;
	}

	bool isSlopeCollision = false;

	// Test the bit which indicates if this is a slope or not.
	if (collisionStyle == COLLISION_STYLE_SOLID)
	{
		collisionValid = true;
	}
	else if (collisionStyle == COLLISION_STYLE_INCLINE || collisionStyle == COLLISION_STYLE_DECLINE)
	{
		// Get the slope and y intercept for this tile type.		
		double slope = collidingWithHitbox->getSlope();

		int yIntercept = 0;

		if (collisionStyle == COLLISION_STYLE_DECLINE)
		{
			yIntercept = collidingWithHitbox->getHeight();
		}

		int x = 0;

		int myHitboxXWorldSpace = myHitbox->getOwnerPosition()->getX() + myHitbox->getLeft();

		int collidingWithHitboxXWorldSapce = collidingWithHitbox->getOwnerPosition()->getX() + collidingWithHitbox->getLeft();
		
		if (collisionStyle == COLLISION_STYLE_INCLINE)
		{
			x = (myHitboxXWorldSpace + myHitbox->getWidth()) - collidingWithHitboxXWorldSapce;
		}
		else if (collisionStyle == COLLISION_STYLE_DECLINE)
		{
			x = myHitboxXWorldSpace - collidingWithHitboxXWorldSapce;
		}

		//// Calculate the y value for the given x.
		int y = (slope * x) + yIntercept;

		//if (y > tileHeight_)
		//{
		//	y = tileHeight_;
		//}

		// Get the y position in world space of my hitbox's bottom and the colliding hitbox's slope line.
		int collidingWithHitboxY = collidingWithHitbox->getOwnerPosition()->getY() + collidingWithHitbox->getTop();

		int slopeLineY = collidingWithHitboxY + (collidingWithHitbox->getHeight() - y);

		int myHitboxBottom = myHitbox->getOwnerPosition()->getY() + myHitbox->getTop() + myHitbox->getHeight();

		bool isCollidingBelowSlopeLine = (myHitboxBottom >= slopeLineY);
		//bool handleCollision = (myHitboxBottom >= collidingWithHitboxY);

		if (isCollidingBelowSlopeLine == true)
		{
			isSlopeCollision = true;

			collisionValid = true;

			// TODO do I need to set the face normal here in place of the direction? Or is that no longer relevant at all?

			// The direction should always be from above for this tile type.
			//collisionData->setCollisionDirection(COLLISION_RESULT_DIRECTION_FROM_ABOVE);
		}
	}

	boost::shared_ptr<HitboxController> hitboxController = hitboxControllerHolder_->getHitboxController();

	collisionData->setSolidCollision(isSlopeCollision || isSolidTileCollision || isOneWayCollision);

	if (collisionValid == true)
	{
		collision(collisionData);
	}
}


void CollidableCodeBehind::preSolidCollision(boost::shared_ptr<CollisionData> collisionData)
{
	// For world geometry collisions, there is a buffer region, so that continuous collisions can be tracked
	// across frames, so that the player can snap to the ground for slopes.

	// There are three different methods of handling collisions.
	//
	// 1. Standard solid collision.
	//
	// 2. Slope collisions.
	//
	// 3. World geometry collisions.

	collisionData->setSolidCollision(false);

	bool isSolidCollision = false;

	bool isSlopeCollision = false;

	bool isSnapCollision = false;

	bool isWorldGeometryCollision = false;

	// Get the hitboxes that are colliding.
	boost::shared_ptr<Hitbox> myHitbox = collisionData->getMyHitbox();

	//findmedebug
	if (myHitbox->getIdentity() == 170)
	{
		bool debug = true;
	}

	boost::shared_ptr<Hitbox> collidingWithHitbox = collisionData->getCollidingHitbox();

	// Get the identities of the hitboxes.
	HitboxIdentity myIdentity = myHitbox->getIdentity();

	HitboxIdentity collidingWithIdentity = collidingWithHitbox->getIdentity();

	// The collision style indicates if it's solid, a slope, or one way.
	CollisionStyle collisionStyle = collidingWithHitbox->getCollisionStyle();

	// It's a world geometry collision if my hitbox is solid, and it is colliding with an identity of HITBOX_WORLDGEOMETRY
	isWorldGeometryCollision = myHitbox->getIsSolid() == true && collidingWithIdentity == ids_->HITBOX_WORLDGEOMETRY;

	int myHitboxBottom = myHitbox->getOwnerPosition()->getY() + myHitbox->getTop() + myHitbox->getHeight();

	// POSSIBLE BUG: I don't see it using isSolidCollision anywhere. There may be a problem with solid collisions that are not world geometry. If this happens, 
	// remember to revisit this.

	switch (collisionStyle)
	{
	case COLLISION_STYLE_SOLID:

		isSolidCollision = true;

		break;

	case COLLISION_STYLE_INCLINE:
	case COLLISION_STYLE_DECLINE:

		isSlopeCollision = true;

		break;

	case COLLISION_STYLE_SNAP_TO_BOTTOM:

		isSnapCollision = true;
	}

	bool mustResolveCollision = false;

	if (isWorldGeometryCollision == true && isSlopeCollision == false && isSnapCollision == false)
	{
		//collisionData->setSolidCollision(isSlopeCollision || isSolidTileCollision || isOneWayCollision);
		collisionData->setSolidCollision(true);

		worldGeometryCollision_ = true;

		mustResolveCollision = true;

		hitboxControllerHolder_->getHitboxController()->setActiveHitboxCollisionStatus(myHitbox->id_, true);
	}
	else if (isSlopeCollision == true && isSnapCollision == false)
	{
		// Determine whether this collision should be handled. 
		// Handle if there was a previous world geometry collision, and moving down (i.e. snap to slope), or 
		// if my hitbox is colliding with the collidable region (i.e. below the slope line).

		double slope = collidingWithHitbox->getSlope();

		int yIntercept = 0;

		if (collisionStyle == COLLISION_STYLE_DECLINE)
		{
			yIntercept = collidingWithHitbox->getHeight();
		}

		// Calculate the x value of the right side of my hitbox, relative to the colliding hitbox bounds.
		int x = 0;

		int myHitboxXWorldSpace = myHitbox->getOwnerPosition()->getX() + myHitbox->getLeft();

		int collidingWithHitboxXWorldSapce = collidingWithHitbox->getOwnerPosition()->getX() + collidingWithHitbox->getLeft();

		if (collisionStyle == COLLISION_STYLE_INCLINE)
		{
			x = (myHitboxXWorldSpace + myHitbox->getWidth()) - collidingWithHitboxXWorldSapce;
		}
		else if (collisionStyle == COLLISION_STYLE_DECLINE)
		{
			x = myHitboxXWorldSpace - collidingWithHitboxXWorldSapce;
		}

		// Calculate the y value for the calculated x value.
		int y = (slope * x) + yIntercept;

		int collidingHitboxRight = collidingWithHitbox->getWidth();

		// Get the maximum height of the slope. This will be different for declines than inclines. Inverted slopes are not yet fully supported.
		int slopeMaxHeight = (abs(slope) * collidingHitboxRight);

		if (y > slopeMaxHeight)
		{
			y = slopeMaxHeight;
		}

		// Get the y position in world space of my hitbox's bottom and the colliding hitbox's slope line.
		int collidingWithHitboxY = collidingWithHitbox->getOwnerPosition()->getY() + collidingWithHitbox->getTop();

		int slopeLineY = collidingWithHitboxY + (collidingWithHitbox->getHeight() - y);

		// Determine if this entity is moving down.
		boost::shared_ptr<PhysicsSnapshot> snapshot = dynamicsControllerHolder_->getDynamicsController()->getPhysicsSnapshot();

		bool movingDownward = (snapshot->getVelocity()->getY() > 0 || snapshot->getMovement()->getY() > 0);

		bool isCollidingBelowSlopeLine = (myHitboxBottom >= slopeLineY);

		bool snapToSlope = previousWorldGeometryCollision_ == true && myHitbox->getIsSolid() == true && movingDownward == true;

		bool handleCollision = isCollidingBelowSlopeLine || snapToSlope;

		if (handleCollision == true)
		{
			mustResolveCollision = false;

			// If this is a solid collision, respond.
			bool isSolid = myHitbox->getIsSolid();

			collisionData->setSolidCollision(isSolid);

			// The direction should always be from above for this tile type.			
			collisionData->getFaceNormal()->setX(0);
			collisionData->getFaceNormal()->setY(-1);
				
			if (isSolid == true)
			{
				boost::shared_ptr<PhysicsSnapshot> snapshot = dynamicsControllerHolder_->getDynamicsController()->getPhysicsSnapshot();

				if (snapshot->getVelocity()->getY() > 0.0)
				{
					snapshot->getMovement()->setY(0.0);
					snapshot->getVelocity()->setY(0.0);
				}

				int myPositionY = position_->getY();

				int hitboxOffsetFromMyTop = (myHitbox->getOwnerPosition()->getY() + myHitbox->getTop()) - myPositionY;

				int newPositionY = (collidingWithHitboxY + (collidingWithHitbox->getHeight() - y)) - myHitbox->getHeight() - hitboxOffsetFromMyTop + 1;

				// Bug: When the player is at the bottom of the map, and the camera is attached to them, the player moves down and permeates the slope tile.
				// However, the camera, going beyond the allowable bounds, does not move with them. Then, when the collision is resolved (here), the player is moved back up, outside
				// of the collidable slope. The camera CAN move up because it is within the bounds. This results in the camera shifting upward.

				// I've finally figured this bug out, and it's a tricky one. I'll try to explain it as best as I can, because I certaintly won't remember it later.
				// Remember that the slope tiles have extra height, so that when walking down them, it is able to snap the player to the floor,
				// otherwise you would get a weird falling effect. So, think about what happens when colliding with TWO slope boxes. It will snap
				// the player down to the lower one, and then back up to the higher one, effectively cancelling out the first snap (just like Iron Man in Avengers Endgame.)
				// Now, when the seam between the two is at just the right height where the player enters the region where the attached camera is outside
				// of its bounds, the camera will not shift down on the first snap, but then the second snap puts the player back into the region
				// where the camera can move freely, and so the camera moves up a little bit, causing an effect where the camera appears to fly away.

				// I plan to solve this with a change where the camera stores a "theoretical position" and an "actual position." The theoretical position is
				// where the camera would be in theory if it was unrestricted by boundaries. The actual position is where it is based on the boundaries. So for example,
				// If the camera cannot move beyond the y boundary of 5000, and the player it is attached to moved to y position 6000, the theoretial position would be
				// somewhere between 5000 and 6000 (based on how it was attached), but the actual position would get clamped and return 5000.

				dynamicsControllerHolder_->getDynamicsController()->setPositionYInternal(newPositionY, false, true);

				fromAboveCutoff_ = newPositionY + hitboxOffsetFromMyTop - 1 + myHitbox->getHeight();

				worldGeometryCollision_ = true;

				slopeCollision_ = true;
			}
			else
			{
				bool debug = true;

				// This collision is below the cutoff.
			}

			hitboxControllerHolder_->getHitboxController()->setActiveHitboxCollisionStatus(myHitbox->id_, true);
		}
	}
	else if (isSnapCollision == true)
	{
		// If the player is moving down, and is the previous frame was a slope collision, and it is not above the cutoff, snap down to the bottom.
		// (Remember to add more snap directions as they become necessary).
		bool isSolid = myHitbox->getIsSolid();

		if (isSolid == true)
		{
			auto dynamicsController = dynamicsControllerHolder_->getDynamicsController();

			boost::shared_ptr<PhysicsSnapshot> snapshot = dynamicsController->getPhysicsSnapshot();

			bool movingDownward = (snapshot->getVelocity()->getY() > 0 || snapshot->getMovement()->getY() > 0);

			if (movingDownward == true && previousSlopeCollision_ == true)
			{
				if (collidingWithHitbox->getTop() < fromAboveCutoff_)
				{
					int collidingWithHitboxBottom = (collidingWithHitbox->getOwnerPosition()->getY() + collidingWithHitbox->getTop()) + collidingWithHitbox->getHeight();

					int myPositionY = position_->getY();

					int hitboxOffsetFromMyTop = (myHitbox->getOwnerPosition()->getY() + myHitbox->getTop()) - myPositionY;

					int newPositionY = collidingWithHitboxBottom - (hitboxOffsetFromMyTop + myHitbox->getHeight()) + 1;

					collisionData->setSolidCollision(isSolid);

					// The direction should always be from above for this tile type.
					//collisionData->setCollisionDirection(COLLISION_RESULT_DIRECTION_FROM_ABOVE);

					dynamicsController->setPositionY(newPositionY);

					// Set a new from above cutoff value. Solid collisions from above that are below this cutoff don't need to be handled, because they would already be effectively
					// handled by this response moving this entity.
					fromAboveCutoff_ = newPositionY + hitboxOffsetFromMyTop - 1 + myHitbox->getHeight();

					hitboxControllerHolder_->getHitboxController()->setActiveHitboxCollisionStatus(myHitbox->id_, true);
				}
			}
		}

	}
	else
	{
		hitboxControllerHolder_->getHitboxController()->setActiveHitboxCollisionStatus(myHitbox->id_, true);

		collisionData->setSolidCollision(false);

		mustResolveCollision = true;
	}

	if (mustResolveCollision == true)
	{
		// The resolve collision function returns a collision resolution value. It indicates which of the colliding entities has precendence
		// in solid body collision.
		CollisionResolution resolution;

		resolution = resolveCollision(collisionData);

		// If both hitboxes are solid, use the resolution value to determine how to respond.
		if (myHitbox->getIsSolid() == true && collidingWithHitbox->getIsSolid() == true)
		{
			// If the collision resolution is to take priority, but it is colliding with a hitbox, change it to yield.
			// Tiles will always take priority,
			if (resolution == COLLISION_RESOLUTION_SOLID_PRIORITY)
			{
				// If colliding with a tile, the tile will always take priority.
				if (collisionData->getCollidingEntityType() == ids_->ENTITY_TILE)
				{
					resolution = COLLISION_RESOLUTION_SOLID_YIELD;
				}
			}

			if (resolution == COLLISION_RESOLUTION_SOLID_PRIORITY)
			{
				solidCollisionResponse(collisionData->getCollidingEntityController()->getDynamicsController(), collidingWithHitbox, myHitbox, collisionData->getFaceNormal(), collisionData->getIntrusion());
			}
			else if (resolution == COLLISION_RESOLUTION_SOLID_YIELD)
			{
				DynamicsController* dynamicController = dynamicsControllerHolder_->getDynamicsController();

				//// If this entity is colliding with a one way collision, test the direction to see if the collision should be processed.
				//if (isOneWayCollision == true)
				//{
				//	solidCollision(direction, dynamicController, myHitbox, collidingWithHitbox, tileCollision, offsetY);
				//}
				//else if (isSolidTileCollision == true)
				//{
				//	solidCollision(direction, dynamicController, myHitbox, collidingWithHitbox, tileCollision, offsetY);
				//}

				collisionData->setSolidCollision(true);

				solidCollisionResponse(dynamicController, myHitbox, collidingWithHitbox, collisionData->getFaceNormal(), collisionData->getIntrusion());
			}
			else
			{
				// Permeate. Do nothing.
			}
		}
	}
}


void CollidableCodeBehind::solidCollisionResponse(DynamicsController* dynamicsController, boost::shared_ptr<Hitbox> myHitbox, boost::shared_ptr<Hitbox> collidingWithHitbox, Vec2IPtr faceNormal, Vec2IPtr intrusion)
{
	boost::shared_ptr<PhysicsSnapshot> snapshot = dynamicsController->getPhysicsSnapshot();

	int myTop = snapshot->getPositionInt()->getY();

	int myLeft = snapshot->getPositionInt()->getX();

	int newPositionY = myTop + intrusion->getY();

	dynamicsController->setPositionY(newPositionY);

	int newPositionX = myLeft + intrusion->getX();

	dynamicsController->setPositionX(newPositionX);

	// If a collision occured along the x-axis, kill the movement.
	if (faceNormal->getX() != 0)
	{
		if (snapshot->getVelocity()->getX() != 0.0)
		{
			snapshot->getMovement()->setX(0.0);
			snapshot->getVelocity()->setX(0.0);
		}
	}

	// If a collision occured along the y-axis, kill the movement.
	if (faceNormal->getY() != 0)
	{
		if (snapshot->getVelocity()->getY() != 0.0)
		{
			snapshot->getMovement()->setY(0.0);
			snapshot->getVelocity()->setY(0.0);
		}
	}

	//if (faceNormal->getX() == 0 && faceNormal->getY() == -1)
	//{
	//	// If this is below the from above cutoff, it doesn't need to be handled.
	//	if (collidingWithHitbox->getTop() < fromAboveCutoff_)
	//	{
	//		if (snapshot->getVelocity()->getY() > 0.0)
	//		{
	//			snapshot->getMovement()->setY(0.0);
	//			snapshot->getVelocity()->setY(0.0);
	//		}

	//		//int adjustPositionYAmount = collidingWithHitbox->getTop() - myHitbox->getTop();

	//		//int hitboxOffsetFromMyTop = myHitbox->getTop() - myTop;
	//		//
	//		//int newPositionY = collidingWithHitbox->getTop() - myHitbox->getHeight() - hitboxOffsetFromMyTop + 1;

	//		//newPositionY = myTop - adjustPositionYAmount;
	//		
	//		// Set a new from above cutoff value. Solid collisions from above that are below this cutoff don't need to be handled, because they would already be effectively
	//		// handled by this response moving this entity.
	//		
	//		//fromAboveCutoff_ = newPositionY + hitboxOffsetFromMyTop - 1 + myHitbox->getHeight();
	//		fromAboveCutoff_ = collidingWithHitbox->getTop();
	//	}
	//	else
	//	{
	//		bool debug = true;

	//		// This collision is below the cutoff.
	//	}
	//}
	//else if (faceNormal->getX() == 0 && faceNormal->getY() == 1)
	//{
	//	// If this is below the from above cutoff, it doesn't need to be handled.
 //  		if (collidingWithHitbox->getTop() + collidingWithHitbox->getHeight() > fromBelowCutoff_)
	//	{
	//		if (snapshot->getVelocity()->getY() < 0.0)
	//		{
	//			snapshot->getMovement()->setY(0.0);
	//			snapshot->getVelocity()->setY(0.0);
	//		}

	//		//int newPositionY = collidingWithHitbox->getTop() + collidingWithHitbox->getHeight() - (myHitbox->getTop() - myTop) - 1;

	//		//dynamicsController->setPositionY(newPositionY);

	//		// Set a new from below cutoff value. Solid collisions from below that are above this cutoff don't need to be handled, because they would already be effectively
	//		// handled by this response moving this entity.
	//		fromBelowCutoff_ = newPositionY;
	//	}
	//}
	//else if (faceNormal->getX() == -1 && faceNormal->getY() == 0)
	//{
	//	if (snapshot->getVelocity()->getX() > 0.0)
	//	{
	//		snapshot->getMovement()->setX(0.0);
	//		snapshot->getVelocity()->setX(0.0);
	//	}

	//	//int newPositionX = collidingWithHitbox->getLeft() - myHitbox->getWidth() - (myHitbox->getLeft() - myLeft) + 1;

	//	//dynamicsController->setPositionX(newPositionX);

	//	// Set a new from left cutoff value. Solid collisions from left that are above this cutoff don't need to be handled, because they would already be effectively
	//	// handled by this response moving this entity.
	//	fromLeftCutoff_ = newPositionX + myHitbox->getWidth();
	//}
	//else if (faceNormal->getX() == 1 && faceNormal->getY() == 0)
	//{
	//	if (snapshot->getVelocity()->getX() < 0.0)
	//	{
	//		snapshot->getMovement()->setX(0.0);
	//		snapshot->getVelocity()->setX(0.0);
	//	}

	//	//int newPositionX = collidingWithHitbox->getLeft() + collidingWithHitbox->getWidth() - (myHitbox->getLeft() - myLeft) - 1;

	//	//dynamicsController->setPositionX(newPositionX);

	//	// Set a new from right cutoff value. Solid collisions from right that are below this cutoff don't need to be handled, because they would already be effectively
	//	// handled by this response moving this entity.
	//	fromRightCutoff_ = newPositionX;
	//}
}

void CollidableCodeBehind::setHitboxStatuses(HitboxIdentity identity, bool status)
{
	boost::shared_ptr<HitboxManager> hitboxManager = getHitboxManager();

	boost::shared_ptr<HitboxController> hitboxController = hitboxControllerHolder_->getHitboxController();

	int activeHitboxCount = hitboxController->getActiveHitboxCount();

	for (int i = 0; i < activeHitboxCount; i++)
	{
		int hitboxId = hitboxController->getActiveHitboxId(i);

		boost::shared_ptr<Hitbox> currentHitbox = hitboxManager->getHitbox(hitboxId);

		HitboxIdentity currentHitboxIdentity = currentHitbox->getIdentity();

		if (identity == currentHitboxIdentity)
		{
			hitboxController->setActiveHitboxCollisionStatus(hitboxId, status);
		}
	}
}

void CollidableCodeBehind::preCollisionEnter(boost::shared_ptr<CollisionData> collisionData)
{
	bool collisionValid = false;

	// Any other active hitbox with the same identity should share the status.
	boost::shared_ptr<Hitbox> myHitbox = collisionData->getMyHitbox();

	HitboxIdentity myHitboxIdentity = myHitbox->getIdentity();
	boost::shared_ptr<Hitbox> collidingWithHitbox = collisionData->getCollidingHitbox();
	HitboxIdentity collidingWithIdentity = collidingWithHitbox->getIdentity();

	int myPositionY = position_->getY();
	int myPositionX = position_->getX();

	float slopeAngle = collidingWithHitbox->getSlope();

	CollisionStyle collisionStyle = collidingWithHitbox->getCollisionStyle();

	bool isSolidTileCollision = myHitbox->getIsSolid() == true && collidingWithIdentity == ids_->HITBOX_WORLDGEOMETRY;

	bool isOneWayCollision = false;

	bool collisionFromAbove = (collisionData->getFaceNormal()->getY() == -1 && collisionData->getFaceNormal()->getX() == 0);

	if (collisionStyle == COLLISION_STYLE_ONE_WAY_TOP && collisionFromAbove == true)
	{
		isOneWayCollision = true;
	}

	bool isSlopeCollision = false;
	
	// Test the bit which indicates if this is a slope or not.
	if (collisionStyle == COLLISION_STYLE_SOLID)
	{
		collisionValid = true;
	}
	else
	{
		// Get the slope and y intercept for this tile type.		
		double slope = collidingWithHitbox->getSlope();

		int yIntercept = 0;

		//findmeupdate
		//// 45 degree slope.
		//if ((collidingWithCollisionMask & SLOPE45) == 0)
		//{
		//	slope = 1.0;
		//}
		//else
		//{
		//	slope = 0.5;

		//	if ((collidingWithCollisionMask & LARGE) > 0)
		//	{
		//		yIntercept = tileHeight_ / 2;
		//	}
		//}

		int x = 0;

		//// Switch the slope direction if it starts on the left side.
		//// Get the x value.
		//int result = collidingWithCollisionMask & RIGHTCORNER;
		//if (result == 0)
		//{
		//	slope *= -1;

		//	x = myHitbox->getLeft() - (collidingWithHitbox->getLeft() + collidingWithHitbox->getWidth());
		//}
		//else
		//{
			x = (myHitbox->getLeft() + myHitbox->getWidth()) - collidingWithHitbox->getLeft();
		//}

		//// Calculate the y value for the given x.
		int y = (slope * x) + yIntercept;

		//if (y > tileHeight_)
		//{
		//	y = tileHeight_;
		//}

		int collidingHitboxTop = collidingWithHitbox->getTop() + (collidingWithHitbox->getHeight() - y);
		int myHitboxBottom = myHitbox->getTop() + myHitbox->getHeight();

		bool handleCollision = (myHitboxBottom >= collidingHitboxTop);

		if (handleCollision == true)
		{
			isSlopeCollision = true;

			collisionValid = true;

			// TODO do I need to set the face normal here in place of the direction? Or is that no longer relevant at all?

			// The direction should always be from above for this tile type.
			//collisionData->setCollisionDirection(COLLISION_RESULT_DIRECTION_FROM_ABOVE);
		}
	}

	boost::shared_ptr<HitboxController> hitboxController = hitboxControllerHolder_->getHitboxController();

	collisionData->setSolidCollision(isSlopeCollision || isSolidTileCollision || isOneWayCollision);

	if (collisionValid == true)
	{
		collisionEnter(collisionData);
	}
}

void CollidableCodeBehind::preCollisionExit(boost::shared_ptr<CollisionData> collisionData)
{
	bool collisionValid = false;
	
	
	// Any other active hitbox with the same identity should share the status.
	boost::shared_ptr<Hitbox> myHitbox = collisionData->getMyHitbox();

	boost::shared_ptr<HitboxController> hitboxController = hitboxControllerHolder_->getHitboxController();

	HitboxIdentity myHitboxIdentity = myHitbox->getIdentity();
	boost::shared_ptr<Hitbox> collidingWithHitbox = collisionData->getCollidingHitbox();
	HitboxIdentity collidingWithIdentity = collidingWithHitbox->getIdentity();

	int myPositionY = position_->getY();
	int myPositionX = position_->getX();

	//unsigned char collidingWithCollisionMask = collidingWithHitbox->getCollisionMaskType();
	float slopeAngle = collidingWithHitbox->getSlope();

	CollisionStyle collisionStyle = collidingWithHitbox->getCollisionStyle();

	bool isSolidTileCollision = myHitbox->getIsSolid() == true && collidingWithIdentity == ids_->HITBOX_WORLDGEOMETRY;

	bool isOneWayCollision = false;

	bool collisionFromAbove = (collisionData->getFaceNormal()->getY() == -1 && collisionData->getFaceNormal()->getX() == 0);

	if (collisionStyle == COLLISION_STYLE_ONE_WAY_TOP && collisionFromAbove == true)
	{
		isOneWayCollision = true;
	}

	bool isSlopeCollision = false;

	if (collisionStyle == COLLISION_STYLE_SOLID)
	{
		collisionValid = true;
	}
	else
	{
		// Get the slope and y intercept for this tile type.
		double slope = collidingWithHitbox->getSlope();

		int yIntercept = 0;

		//// 45 degree slope.
		//if ((collidingWithCollisionMask & SLOPE45) == 0)
		//{
		//	slope = 1.0;
		//}
		//else
		//{
		//	slope = 0.5;

		//	if ((collidingWithCollisionMask & LARGE) > 0)
		//	{
		//		yIntercept = tileHeight_ / 2;
		//	}
		//}

		int x = 0;

		//// Switch the slope direction if it starts on the left side.
		//// Get the x value.
		//int result = collidingWithCollisionMask & RIGHTCORNER;
		//if (result == 0)
		//{
		//	slope *= -1;

		//	x = myHitbox->getLeft() - (collidingWithHitbox->getLeft() + collidingWithHitbox->getWidth());
		//}
		//else
		//{
			x = (myHitbox->getLeft() + myHitbox->getWidth()) - collidingWithHitbox->getLeft();
		//}

		// Calculate the y value for the given x.
		int y = (slope * x) + yIntercept;

		//if (y > tileHeight_)
		//{
		//	y = tileHeight_;
		//}

		int collidingHitboxTop = collidingWithHitbox->getTop() + (collidingWithHitbox->getHeight() - y);
		int myHitboxBottom = myHitbox->getTop() + myHitbox->getHeight();

		bool handleCollision = (myHitboxBottom < collidingHitboxTop);

		if (handleCollision == true)
		{
			isSlopeCollision = true;

			collisionValid = true;

			//todo Is this irrelevant now or do I need to set the face normal?

			// The direction should always be from above for this tile type.
			//collisionData->setCollisionDirection(COLLISION_RESULT_DIRECTION_FROM_ABOVE);
		}
	}

	collisionData->setSolidCollision(isSlopeCollision || isSolidTileCollision || isOneWayCollision);

	if (collisionValid == true)
	{
		collisionExit(collisionData);
	}
}

void CollidableCodeBehind::collision(boost::shared_ptr<CollisionData> collisionData)
{
	collidableScript_->collision(collisionData);
}

void CollidableCodeBehind::collisionEnter(boost::shared_ptr<CollisionData> collisionData)
{
	collidableScript_->collisionEnter(collisionData);
}

void CollidableCodeBehind::collisionExit(boost::shared_ptr<CollisionData> collisionData)
{
	collidableScript_->collisionExit(collisionData);
}

CollisionResolution CollidableCodeBehind::resolveCollision(boost::shared_ptr<CollisionData> collisionData)
{
	return collidableScript_->resolveCollision(collisionData);
}

boost::shared_ptr<CollisionData> CollidableCodeBehind::getCollisionData(int hitboxId)
{
	return collidableScript_->getCollisionData(hitboxId);
}

void CollidableCodeBehind::preInitialize()
{
	collidableScript_ = boost::shared_ptr<CollidableScript>(new CollidableScript(debugger_));

	boost::shared_ptr<HitboxManager> hitboxManager = getHitboxManager();

	collidableScript_->hitboxManager_ = hitboxManager; 
	collidableScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		collidableScript_->preInitialize();
	}

	initialize();
}

void CollidableCodeBehind::initialize()
{
}

void CollidableCodeBehind::preCleanup()
{
	cleanup();

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		collidableScript_->preCleanup();
	}
}

void CollidableCodeBehind::cleanup()
{
}