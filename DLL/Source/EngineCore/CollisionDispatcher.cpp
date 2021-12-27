#include "..\..\Headers\EngineCore\CollisionDispatcher.hpp"

using namespace firemelon;
using namespace boost::python;

CollisionDispatcher::CollisionDispatcher(CollisionTesterPtr collisionTester, boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger)
{
	collisionLogger_ = debugger->collisionLogger;

	collisionTester_ = collisionTester;

	debugger_ = debugger;

	ids_ = ids;
}

CollisionDispatcher::~CollisionDispatcher()
{
}

void CollisionDispatcher::addRoom()
{
	CollisionRecords roomCollisionRecords;

	roomCollisionRecords_.push_back(roomCollisionRecords);
}

void CollisionDispatcher::prepareFrame(int roomIndex)
{
	CollisionRecords* collisionRecords = &roomCollisionRecords_[roomIndex];

	// Rebuild the previousCollisionMap from the currentCollision list.	
	collisionRecords->previousCollisionRecords_.clear();

	// If the any of the entities were removed as a result of a collision event or update, do not add them to this list.
	int size = collisionRecords->collisionRecords_.size();
	for (int i = 0 ; i < size; i++)
	{
		CollisionRecord cr = collisionRecords->collisionRecords_[i];

		if (cr.collideeComponents_->getEntityInvalidator()->getIsInvalidated() == false && cr.colliderComponents_->getEntityInvalidator()->getIsInvalidated() == false)
		{
			#ifdef _USESTRINGKEY_
				collisionRecords->previousCollisionStatusMap_[cr.key_] = COLLISION_OCCURRED_NOT_DISPATCHED;
			#else
				collisionRecords->previousCollisionStatusMap_[cr] = COLLISION_OCCURRED_NOT_DISPATCHED;
			#endif

			collisionRecords->previousCollisionRecords_.push_back(cr);
		}
	}
}

void CollisionDispatcher::clear(int roomIndex)
{
	roomCollisionRecords_[roomIndex].collisionRecords_.clear();
}

void CollisionDispatcher::clearAll(int roomIndex)
{
	CollisionRecords* collisionRecords = &roomCollisionRecords_[roomIndex];

	collisionRecords->collisionRecords_.clear();
	collisionRecords->previousCollisionRecords_.clear();
	collisionRecords->previousCollisionStatusMap_.clear();
}

void CollisionDispatcher::addCollisionRecord(CollisionRecord collisionRecord)
{
	int roomIndex = collisionRecord.roomIndex_;

	std::vector<CollisionRecord>* collisionRecordList;
	
	collisionRecordList = &roomCollisionRecords_[roomIndex].collisionRecords_;

	// Insert the collision record if a match is not found.
	if(std::find_if(collisionRecordList->begin(), collisionRecordList->end(), MatchesCollisionRecord(collisionRecord)) == collisionRecordList->end())
	{														
		collisionRecordList->push_back(collisionRecord);
	}
}

void CollisionDispatcher::removeCollisionRecord(int roomIndex, int ownerId)
{
	CollisionRecords* collisionRecords = &roomCollisionRecords_[roomIndex];

	int size = collisionRecords->collisionRecords_.size();
	
	for (int i = size - 1; i >= 0; i--)
	{
		CollisionRecord cr = collisionRecords->collisionRecords_[i];

		if (cr.colliderId_ == ownerId || cr.collideeId_ == ownerId)
		{
			 collisionRecords->collisionRecords_.erase(collisionRecords->collisionRecords_.begin() + i);
		}
	}

	size = collisionRecords->previousCollisionRecords_.size();
	
	for (int i = size - 1; i >= 0; i--)
	{
		CollisionRecord cr = collisionRecords->previousCollisionRecords_[i];

		if (cr.colliderId_ == ownerId || cr.collideeId_ == ownerId)
		{
			collisionRecords->previousCollisionRecords_.erase(collisionRecords->previousCollisionRecords_.begin() + i);
		}
	}	
}

void CollisionDispatcher::dispatch(int roomIndex)
{
	CollisionRecords* collisionRecordStruct = &roomCollisionRecords_[roomIndex];
	
	std::vector<CollisionRecord> collisionRecords;
	std::vector<CollisionRecord> previousCollisionRecords;
		
	#ifdef _USESTRINGKEY_
		std::map<std::string, CollisionStatus> currentCollisionStatusMap;
		std::map<std::string, CollisionStatus>* previousCollisionStatusMap;
	#else
		std::map<CollisionRecord, CollisionStatus> currentCollisionStatusMap;
		std::map<CollisionRecord, CollisionStatus>* previousCollisionStatusMap;
	#endif

	collisionRecords = collisionRecordStruct->collisionRecords_;
	previousCollisionRecords = collisionRecordStruct->previousCollisionRecords_;
	previousCollisionStatusMap = &(collisionRecordStruct->previousCollisionStatusMap_);

	int size = collisionRecords.size();
	
	for (int i = 0; i < size; i++)
	{
		CollisionRecord cr = collisionRecords[i];

		// Objects may have been removed as a result of the collision event.
		// Don't process collisions for removed entities.
		
		bool colliderInvalidated = cr.colliderComponents_->getEntityInvalidator()->getIsInvalidated();

		bool collideeInvalidated = cr.collideeComponents_->getEntityInvalidator()->getIsInvalidated();

		if (colliderInvalidated == false && collideeInvalidated == false)
		{

			// Remove for now. This doesn't work quite right without massive reworking. Is it worth it?

			// See D:\Development\Cpp\Firemelon_1_2_0\Documentation\20210804_215442.jpg for the reason why it doesn't work right.

			////////// Rebuild the collision data to run a secondary collision test. A solid collision response could have made a previously valid collision
			////////// no longer valid.
			////////HitboxPtr collideeHitbox = hitboxManager_->getHitbox(cr.collideeHitboxId_);

			////////HitboxPtr colliderHitbox = hitboxManager_->getHitbox(cr.colliderHitboxId_);

			////////HitboxControllerPtr hitboxControllerCollidee = cr.collideeComponents_->getHitboxController();

			////////HitboxControllerPtr hitboxControllerCollider = cr.colliderComponents_->getHitboxController();

			////////int colliderPositionX = hitboxControllerCollider->getOwnerPosition()->getX() + hitboxControllerCollider->getStagePosition()->getX();

			////////int colliderPositionY = hitboxControllerCollider->getOwnerPosition()->getY() + hitboxControllerCollider->getStagePosition()->getY();

			////////Rect rectCollider = colliderHitbox->getCollisionRect();

			////////rectCollider.x += colliderPositionX;

			////////rectCollider.y += colliderPositionY;

			////////int collideePositionX = hitboxControllerCollidee->getOwnerPosition()->getX() + hitboxControllerCollidee->getStagePosition()->getX();

			////////int collideePositionY = hitboxControllerCollidee->getOwnerPosition()->getY() + hitboxControllerCollidee->getStagePosition()->getY();

			////////unsigned char collideeEdgeFlags = 0x00;

			////////if (collideeHitbox->getUseTopEdge())
			////////{
			////////	collideeEdgeFlags |= 0x08;
			////////}

			////////if (collideeHitbox->getUseBottomEdge())
			////////{
			////////	collideeEdgeFlags |= 0x04;
			////////}

			////////if (collideeHitbox->getUseLeftEdge())
			////////{
			////////	collideeEdgeFlags |= 0x02;
			////////}

			////////if (collideeHitbox->getUseRightEdge())
			////////{
			////////	collideeEdgeFlags |= 0x01;
			////////}

			////////// Build temporary rects using the dimensions of the hitboxes, offset by the owner entity positions.
			////////Rect rectCollidee = collideeHitbox->getCollisionRect();

			////////rectCollidee.x += collideePositionX;

			////////rectCollidee.y += collideePositionY;

			////////bool colliderIsSolid = colliderHitbox->getIsSolid();

			////////bool useSatCollisionTest = false;

			////////if (collideeHitbox->transformedCorners_[0].y != collideeHitbox->transformedCorners_[1].y || colliderHitbox->transformedCorners_[0].y != colliderHitbox->transformedCorners_[1].y)
			////////{
			////////	useSatCollisionTest = true;
			////////}

			////////CollisionTestResultPtr collisionTestResult;

			////////CollisionTestResultPtr reverseCollisionTestResult = boost::make_shared<CollisionTestResult>(CollisionTestResult());

			////////if (useSatCollisionTest == true)
			////////{
			////////	PositionPtr colliderOwnerPosition = hitboxControllerCollider->getOwnerPosition();

			////////	PositionPtr colliderStagePosition = hitboxControllerCollider->getStagePosition();

			////////	std::vector<Vertex2> colliderCorners;

			////////	for (int j = 0; j < colliderHitbox->transformedCorners_.size(); j++)
			////////	{
			////////		Vertex2 colliderCorner;

			////////		colliderCorner.x = colliderHitbox->transformedCorners_[j].x + colliderOwnerPosition->getX() + colliderStagePosition->getX();

			////////		colliderCorner.y = colliderHitbox->transformedCorners_[j].y + colliderOwnerPosition->getY() + colliderStagePosition->getY();

			////////		colliderCorners.push_back(colliderCorner);
			////////	}

			////////	PositionPtr collideeOwnerPosition = hitboxControllerCollidee->getOwnerPosition();

			////////	PositionPtr collideeStagePosition = hitboxControllerCollidee->getStagePosition();

			////////	std::vector<Vertex2> collideeCorners;

			////////	for (int j = 0; j < colliderCorners.size(); j++)
			////////	{
			////////		Vertex2 collideeCorner;

			////////		collideeCorner.x = collideeHitbox->transformedCorners_[j].x + collideeOwnerPosition->getX() + collideeStagePosition->getX();

			////////		collideeCorner.y = collideeHitbox->transformedCorners_[j].y + collideeOwnerPosition->getY() + collideeStagePosition->getY();

			////////		collideeCorners.push_back(collideeCorner);
			////////	}

			////////	collisionTestResult = collisionTester_->collisionTestSat(colliderCorners, collideeCorners);
			////////}
			////////else
			////////{
			////////	// Basic AABB collision test.
			////////	collisionTestResult = collisionTester_->collisionTestAabb(rectCollider, rectCollidee, collideeEdgeFlags, colliderIsSolid);
			////////}

			//if (collisionTestResult->collisionOccurred == true)
			if (true)
			{
				// Add this collision to the current collision map, to be used later in this function.
			
				#ifdef _USESTRINGKEY_
					currentCollisionStatusMap[cr.key_] = COLLISION_OCCURRED_NOT_DISPATCHED;
				
					CollisionStatus status = (*previousCollisionStatusMap)[cr.key_];
				#else
					currentCollisionStatusMap[cr] = COLLISION_OCCURRED_NOT_DISPATCHED;
				
					CollisionStatus statusFromPreviousFrame = (*previousCollisionStatusMap)[cr];
				#endif

				// If this collision is in the list of collisions from the previous frame, only call the collision signal,
				// otherwise call collisionEnterSignal as well.
						
				if (statusFromPreviousFrame == COLLISION_OCCURRED_NOT_DISPATCHED)
				{
					// Log the collision
					CollisionLogPtr collisionLog = boost::make_shared<CollisionLog>(CollisionLog());

					// Fill in the collision data.
					collisionLog->type = COLLISION_NORMAL;

					HitboxPtr hitboxCollidee = hitboxManager_->getHitbox(cr.collideeHitboxId_);

					HitboxPtr hitboxCollider = hitboxManager_->getHitbox(cr.colliderHitboxId_);

					collisionLog->hitboxCollidee = hitboxCollidee;

					collisionLog->hitboxCollideeId = cr.collideeHitboxId_;

					collisionLog->hitboxCollideeOwnerId = cr.collideeId_;

					collisionLog->hitboxCollider = hitboxCollider;

					collisionLog->hitboxColliderId = cr.colliderHitboxId_;

					collisionLog->hitboxColliderOwnerId = cr.colliderId_;

					collisionLog->rectCollidee = cr.collideeFullRect_;

					collisionLog->rectCollider = cr.colliderFullRect_;

					collisionLogger_->logCollision(collisionLog);

					preProcessCollision(cr);
				}
				else if (statusFromPreviousFrame == COLLISION_DID_NOT_OCCUR)
				{
					// Log the collision
					CollisionLogPtr collisionLogEnter = boost::make_shared<CollisionLog>(CollisionLog());

					// Fill in the collision data.
					collisionLogEnter->type = COLLISION_ENTER;

					HitboxPtr hitboxCollidee = hitboxManager_->getHitbox(cr.collideeHitboxId_);

					HitboxPtr hitboxCollider = hitboxManager_->getHitbox(cr.colliderHitboxId_);

					collisionLogEnter->hitboxCollidee = hitboxCollidee;

					collisionLogEnter->hitboxCollideeId = cr.collideeHitboxId_;

					collisionLogEnter->hitboxCollideeOwnerId = cr.collideeId_;
				
					collisionLogEnter->hitboxCollider = hitboxCollider;

					collisionLogEnter->hitboxColliderId = cr.colliderHitboxId_;

					collisionLogEnter->hitboxColliderOwnerId = cr.colliderId_;

					collisionLogEnter->rectCollidee = cr.collideeFullRect_;

					collisionLogEnter->rectCollider = cr.colliderFullRect_;

					collisionLogger_->logCollision(collisionLogEnter);

					// Log the collision
					CollisionLogPtr collisionLog = boost::make_shared<CollisionLog>(CollisionLog());

					// Fill in the collision data.
					collisionLog->type = COLLISION_NORMAL;

					hitboxCollidee = hitboxManager_->getHitbox(cr.collideeHitboxId_);
				
					hitboxCollider = hitboxManager_->getHitbox(cr.colliderHitboxId_);

					collisionLog->hitboxCollidee = hitboxCollidee;

					collisionLog->hitboxCollideeId = cr.collideeHitboxId_;

					collisionLog->hitboxCollideeOwnerId = cr.collideeId_;

					collisionLog->hitboxCollider = hitboxCollider;

					collisionLog->hitboxColliderId = cr.colliderHitboxId_;

					collisionLog->hitboxColliderOwnerId = cr.colliderId_;

					collisionLog->rectCollidee = cr.collideeFullRect_;

					collisionLog->rectCollider = cr.colliderFullRect_;

					collisionLogger_->logCollision(collisionLog);

					preCollisionEnter(cr);

					preProcessCollision(cr);
				
					#ifdef _USESTRINGKEY_
						(*previousCollisionStatusMap)[cr.key_] = COLLISION_OCCURRED_NOT_DISPATCHED;
					#else
						(*previousCollisionStatusMap)[cr] = COLLISION_OCCURRED_NOT_DISPATCHED;
					#endif
				}		
			}
			else
			{
				// Collision that was previously valid is no longer valid. Override the status to be NONE.
				bool debug = true;
				
				#ifdef _USESTRINGKEY_
					currentCollisionStatusMap[cr.key_] = COLLISION_DID_NOT_OCCUR;
				#else
					currentCollisionStatusMap[cr] = COLLISION_DID_NOT_OCCUR;
				#endif
			}
		}
	}
	
	// If any collisions from the previous list did not happen in this frame, call the collisionExit signal.	
	previousCollisionStatusMap->clear();

	for (size_t i = 0 ; i < previousCollisionRecords.size(); i++)
	{
		CollisionRecord cr = previousCollisionRecords[i];
		
		// Entities may have been removed as a result of the collision event. Don't process collisions for removed entities.
		
		if (cr.collideeComponents_->getEntityInvalidator()->getIsInvalidated() == false && cr.colliderComponents_->getEntityInvalidator()->getIsInvalidated() == false)
		{
			#ifdef _USESTRINGKEY_
				if (currentCollisionStatusMap[cr.key_] == COLLISION_DID_NOT_OCCUR)
				{				
					preCollisionExit(cr);

					currentCollisionStatusMap[cr.key_] = COLLISION_OCCURRED_DISPATCHED;
				}
			#else
				if (currentCollisionStatusMap[cr] == COLLISION_DID_NOT_OCCUR)
				{
					// Log the collision
					CollisionLogPtr collisionLogExit = boost::make_shared<CollisionLog>(CollisionLog());

					// Fill in the collision data.
					collisionLogExit->type = COLLISION_EXIT;

					HitboxPtr hitboxCollidee = hitboxManager_->getHitbox(cr.collideeHitboxId_);

					HitboxPtr hitboxCollider = hitboxManager_->getHitbox(cr.colliderHitboxId_);

					collisionLogExit->hitboxCollidee = hitboxCollidee;

					collisionLogExit->hitboxCollideeId = cr.collideeHitboxId_;

					collisionLogExit->hitboxCollideeOwnerId = cr.collideeId_;

					collisionLogExit->hitboxCollider = hitboxCollider;

					collisionLogExit->hitboxColliderId = cr.colliderHitboxId_;

					collisionLogExit->hitboxColliderOwnerId = cr.colliderId_;

					collisionLogExit->rectCollidee = cr.collideeFullRect_;

					collisionLogExit->rectCollider = cr.colliderFullRect_;

					collisionLogger_->logCollision(collisionLogExit);

					preCollisionExit(cr);

					currentCollisionStatusMap[cr] = COLLISION_OCCURRED_DISPATCHED;
				}
			#endif
		}
	}
}

void CollisionDispatcher::preProcessCollision(CollisionRecord collisionRecord)
{
	boost::shared_ptr<EntityComponents> colliderComponents = collisionRecord.colliderComponents_;

	boost::shared_ptr<EntityComponents> collideeComponents = collisionRecord.collideeComponents_;
	
	if (collideeComponents == nullptr || colliderComponents == nullptr)
	{
		return;
	}

	// Get the two hitboxes that are colliding.
	boost::shared_ptr<Hitbox> hbCollidee = hitboxManager_->getHitbox(collisionRecord.collideeHitboxId_);

	boost::shared_ptr<Hitbox> hbCollider = hitboxManager_->getHitbox(collisionRecord.colliderHitboxId_);

	// Process the collision for the collidee.
	processCollision(colliderComponents, collideeComponents, hbCollider, hbCollidee, collisionRecord.colliderHitboxId_, collisionRecord.collideeHitboxId_, collisionRecord.reverseCollisionTestResult_->faceNormal, collisionRecord.reverseCollisionTestResult_->intrusion);

	// Process the reverse collision for the collider.
	processCollision(collideeComponents, colliderComponents, hbCollidee, hbCollider, collisionRecord.collideeHitboxId_, collisionRecord.colliderHitboxId_, collisionRecord.collisionTestResult_->faceNormal, collisionRecord.collisionTestResult_->intrusion);
}

void CollisionDispatcher::processCollision(boost::shared_ptr<EntityComponents> respondingEntity,	     boost::shared_ptr<EntityComponents> requestingEntity,
										   boost::shared_ptr<Hitbox>           respondingEntityHitbox,   boost::shared_ptr<Hitbox>           requestingEntityHitbox,
										   int                                 respondingEntityHitboxId, int                                 requestingEntityHitboxId,
	                                       Vec2IPtr                            faceNormal,               Vec2IPtr                            intrusion)
{
	// Tile's dont need to respond to collisions.
	EntityTypeId entityTypeId = respondingEntity->getEntityMetadata()->getEntityTypeId();

	boost::shared_ptr<CollidableCodeBehind> respondingCollidable = respondingEntity->getCodeBehindContainer()->getCollidableCodeBehind();

	boost::shared_ptr<CollidableCodeBehind> requestingCollidable = requestingEntity->getCodeBehindContainer()->getCollidableCodeBehind();

	if (entityTypeId != ids_->ENTITY_TILE)
	{
		PythonAcquireGil lock;

		boost::shared_ptr<CollisionData> collisionData = requestingCollidable->getCollisionData(requestingEntityHitboxId);

		try
		{
			collisionData->intrusion_->setX(intrusion->getX());

			collisionData->intrusion_->setY(intrusion->getY());
			
			collisionData->setCollidingEntityController(requestingEntity->getEntityController());
			collisionData->setCollidingEntityStateMachineController(requestingEntity->getStateMachineController());
			collisionData->setCollidingEntityDynamicsController(requestingEntity->getDynamicsController());

			collisionData->pyCollisionData_.attr("collidingEntityType") = requestingEntity->getEntityMetadata()->getEntityTypeId();
			
			//findmetodo Set the face normal here.
			//collisionData->pyCollisionData_.attr("collisionDirection") = direction;
		
			boost::shared_ptr<Hitbox> myHitbox = collisionData->getMyHitbox();
		
			myHitbox->ownerPosition_->setX(respondingEntity->getPosition()->getX());
			myHitbox->ownerPosition_->setY(respondingEntity->getPosition()->getY());

			myHitbox->id_ = respondingEntityHitbox->id_;

			myHitbox->setCollisionRect(respondingEntityHitbox->getCollisionRect());

			myHitbox->setIdentity(respondingEntityHitbox->getIdentity());
			myHitbox->setIsSolid(respondingEntityHitbox->getIsSolid());
			myHitbox->setCollisionStyle(respondingEntityHitbox->getCollisionStyle());
			myHitbox->setSlope(respondingEntityHitbox->getSlope());

			for (size_t i = 0; i < respondingEntityHitbox->transformedCorners_.size(); i++)
			{
				myHitbox->transformedCorners_[i].x = respondingEntityHitbox->transformedCorners_[i].x;

				myHitbox->transformedCorners_[i].y = respondingEntityHitbox->transformedCorners_[i].y;
			}
			
			collisionData->pyCollisionData_.attr("myHitbox") = myHitbox;

			boost::shared_ptr<Hitbox> collidingHitbox = collisionData->getCollidingHitbox();
		
						
			collidingHitbox->id_ = requestingEntityHitbox->id_;
			
			collidingHitbox->ownerPosition_->setX(requestingEntity->getPosition()->getX());
			collidingHitbox->ownerPosition_->setY(requestingEntity->getPosition()->getY());

			collidingHitbox->setCollisionRect(requestingEntityHitbox->getCollisionRect());
			collidingHitbox->setIdentity(requestingEntityHitbox->getIdentity());
			collidingHitbox->setIsSolid(requestingEntityHitbox->getIsSolid());
			collidingHitbox->setCollisionStyle(requestingEntityHitbox->getCollisionStyle());
			collidingHitbox->setSlope(requestingEntityHitbox->getSlope());

			for (size_t i = 0; i < requestingEntityHitbox->transformedCorners_.size(); i++)
			{
				collidingHitbox->transformedCorners_[i].x = requestingEntityHitbox->transformedCorners_[i].x;

				collidingHitbox->transformedCorners_[i].y = requestingEntityHitbox->transformedCorners_[i].y;
			}
		}
		catch(error_already_set &)
		{
			std::cout<<"Error setting collision data. Is the getCollisionData function implemented?"<<std::endl;
			debugger_->handlePythonError();
		}

		respondingCollidable->preCollision(collisionData);
		
		// The collision data object for tiles doesn't get generated each collision, it is created at initialization and persists
		// for the entire lifetime of the tile. Don't clear the collision data for these.
		if (requestingEntity->getEntityMetadata()->getEntityTypeId() != ids_->ENTITY_TILE)
		{
			// Done with the collision data. Set the py instance to none.
			collisionData->pyCollisionData_ = boost::python::object();
		}
	}
}

void CollisionDispatcher::preCollisionEnter(CollisionRecord collisionRecord)
{
	boost::shared_ptr<EntityComponents> colliderComponents = collisionRecord.colliderComponents_;
	boost::shared_ptr<EntityComponents> collideeComponents = collisionRecord.collideeComponents_;
	
	if (collideeComponents == nullptr || colliderComponents == nullptr)
	{
		return;
	}

	boost::shared_ptr<Hitbox> hbCollidee = hitboxManager_->getHitbox(collisionRecord.collideeHitboxId_);
	boost::shared_ptr<Hitbox> hbCollider = hitboxManager_->getHitbox(collisionRecord.colliderHitboxId_);
	
	collisionEnter(collideeComponents, colliderComponents, hbCollidee, hbCollider, collisionRecord.collideeHitboxId_, collisionRecord.colliderHitboxId_, collisionRecord.collisionTestResult_->faceNormal, collisionRecord.collisionTestResult_->intrusion);
	collisionEnter(colliderComponents, collideeComponents, hbCollider, hbCollidee, collisionRecord.colliderHitboxId_, collisionRecord.collideeHitboxId_, collisionRecord.reverseCollisionTestResult_->faceNormal, collisionRecord.reverseCollisionTestResult_->intrusion);
}

void CollisionDispatcher::collisionEnter(boost::shared_ptr<EntityComponents> requestingEntity, boost::shared_ptr<EntityComponents> respondingEntity, boost::shared_ptr<Hitbox> requestingEntityHitbox, boost::shared_ptr<Hitbox> respondingEntityHitbox, int requestingEntityHitboxId, int respondingEntityHitboxId, Vec2IPtr faceNormal, Vec2IPtr intrusion)
{
	// Tile's dont need to respond to collisions.
	EntityTypeId entityTypeId = respondingEntity->getEntityMetadata()->getEntityTypeId();

	boost::shared_ptr<CollidableCodeBehind> respondingCollidable = respondingEntity->getCodeBehindContainer()->getCollidableCodeBehind();
	boost::shared_ptr<CollidableCodeBehind> requestingCollidable = requestingEntity->getCodeBehindContainer()->getCollidableCodeBehind();

	if (entityTypeId != ids_->ENTITY_TILE)
	{
		PythonAcquireGil lock;

		boost::shared_ptr<CollisionData> collisionData = requestingCollidable->getCollisionData(requestingEntityHitboxId);

		try
		{
			// Prepare the collision data.
			collisionData->setCollidingEntityController(requestingEntity->getEntityController());

			collisionData->setCollidingEntityStateMachineController(requestingEntity->getStateMachineController());

			collisionData->setCollidingEntityDynamicsController(requestingEntity->getDynamicsController());

			collisionData->pyCollisionData_.attr("collidingEntityType") = requestingEntity->getEntityMetadata()->getEntityTypeId();
			
			//findmetodo Set the face normal here.
			//collisionData->pyCollisionData_.attr("collisionDirection") = direction;
		
			boost::shared_ptr<Hitbox> myHitbox = collisionData->getMyHitbox();
		
			int transformedX = respondingEntityHitbox->getLeft();

			Rect collisionRect1;
			collisionRect1.x = respondingEntity->getPosition()->getX() + transformedX;
			collisionRect1.y = respondingEntity->getPosition()->getY() + respondingEntityHitbox->getTop();		
			collisionRect1.w = respondingEntityHitbox->getWidth();
			collisionRect1.h = respondingEntityHitbox->getHeight();

			myHitbox->setCollisionRect(collisionRect1);
			myHitbox->setIdentity(respondingEntityHitbox->getIdentity());
			myHitbox->setIsSolid(respondingEntityHitbox->getIsSolid());
			myHitbox->setCollisionStyle(respondingEntityHitbox->getCollisionStyle());
			myHitbox->setSlope(respondingEntityHitbox->getSlope());
			
			collisionData->pyCollisionData_.attr("myHitbox") = myHitbox;

			boost::shared_ptr<Hitbox> collidingHitbox = collisionData->getCollidingHitbox();
		
			transformedX = requestingEntityHitbox->getLeft();

			Rect collisionRect2;
			collisionRect2.x = requestingEntity->getPosition()->getX() + transformedX;
			collisionRect2.y = requestingEntity->getPosition()->getY() + requestingEntityHitbox->getTop();		
			collisionRect2.w = requestingEntityHitbox->getWidth();
			collisionRect2.h = requestingEntityHitbox->getHeight();
		
			collidingHitbox->setCollisionRect(collisionRect2);
			collidingHitbox->setIdentity(requestingEntityHitbox->getIdentity());
			collidingHitbox->setIsSolid(requestingEntityHitbox->getIsSolid());
			collidingHitbox->setCollisionStyle(requestingEntityHitbox->getCollisionStyle());
			collidingHitbox->setSlope(requestingEntityHitbox->getSlope());			
		}
		catch(error_already_set &)
		{
			std::cout<<"Error setting collision data. Is the getCollisionData function implemented?"<<std::endl;
			debugger_->handlePythonError();
		}

		respondingCollidable->preCollisionEnter(collisionData);

		// The collision data object for tiles doesn't get generated each collision, it is created at initialization and persists
		// for the entire lifetime of the tile. Don't clear the collision data for these.
		if (requestingEntity->getEntityMetadata()->getEntityTypeId() != ids_->ENTITY_TILE)
		{
			// Done with the collision data. Set the py instance to none.
			collisionData->pyCollisionData_ = boost::python::object();
		}
	}
	else
	{
		if (debugger_->getDebugMode() == true && debugger_->getDebugModeRenderHitboxes() == true)
		{
			respondingCollidable->setHitboxStatuses(respondingEntityHitbox->getIdentity(), true);
		}
	}
}

void CollisionDispatcher::preCollisionExit(CollisionRecord collisionRecord)
{
	boost::shared_ptr<EntityComponents> colliderComponents = collisionRecord.colliderComponents_;
	boost::shared_ptr<EntityComponents> collideeComponents = collisionRecord.collideeComponents_;
	
	if (collideeComponents == nullptr || colliderComponents == nullptr)
	{
		return;
	}

	boost::shared_ptr<Hitbox> hbCollidee = hitboxManager_->getHitbox(collisionRecord.collideeHitboxId_);
	boost::shared_ptr<Hitbox> hbCollider = hitboxManager_->getHitbox(collisionRecord.colliderHitboxId_);
		
	collisionExit(collideeComponents, colliderComponents, hbCollidee, hbCollider, collisionRecord.collideeHitboxId_, collisionRecord.colliderHitboxId_, collisionRecord.collisionTestResult_->faceNormal, collisionRecord.collisionTestResult_->intrusion);
	collisionExit(colliderComponents, collideeComponents, hbCollider, hbCollidee, collisionRecord.colliderHitboxId_, collisionRecord.collideeHitboxId_, collisionRecord.reverseCollisionTestResult_->faceNormal, collisionRecord.reverseCollisionTestResult_->intrusion);
}

void CollisionDispatcher::collisionExit(boost::shared_ptr<EntityComponents> requestingEntity, boost::shared_ptr<EntityComponents> respondingEntity, boost::shared_ptr<Hitbox> requestingEntityHitbox, boost::shared_ptr<Hitbox> respondingEntityHitbox, int requestingEntityHitboxId, int respondingEntityHitboxId,	Vec2IPtr faceNormal, Vec2IPtr intrusion)
{
	// Tile's dont need to respond to collisions.
	EntityTypeId entityTypeId = respondingEntity->getEntityMetadata()->getEntityTypeId();

	boost::shared_ptr<CollidableCodeBehind> respondingCollidable = respondingEntity->getCodeBehindContainer()->getCollidableCodeBehind();
	boost::shared_ptr<CollidableCodeBehind> requestingCollidable = requestingEntity->getCodeBehindContainer()->getCollidableCodeBehind();

	if (entityTypeId != ids_->ENTITY_TILE)
	{
		PythonAcquireGil lock;

		boost::shared_ptr<CollisionData> collisionData = requestingCollidable->getCollisionData(requestingEntityHitboxId);
		
		try
		{
			// Prepare the collision data.
			collisionData->setCollidingEntityController(requestingEntity->getEntityController());
			collisionData->setCollidingEntityStateMachineController(requestingEntity->getStateMachineController());
			collisionData->setCollidingEntityDynamicsController(requestingEntity->getDynamicsController());

			collisionData->pyCollisionData_.attr("collidingEntityType") = requestingEntity->getEntityMetadata()->getEntityTypeId();
			
			//findmetodo Set the face normal here.
			//collisionData->pyCollisionData_.attr("collisionDirection") = direction;
		
			boost::shared_ptr<Hitbox> myHitbox = collisionData->getMyHitbox();
		
			int transformedX = respondingEntityHitbox->getLeft();
					
			Rect collisionRect1;
			collisionRect1.x = respondingEntity->getPosition()->getX() + transformedX;
			collisionRect1.y = respondingEntity->getPosition()->getY() + respondingEntityHitbox->getTop();		
			collisionRect1.w = respondingEntityHitbox->getWidth();
			collisionRect1.h = respondingEntityHitbox->getHeight();

			myHitbox->setCollisionRect(collisionRect1);
			myHitbox->setIdentity(respondingEntityHitbox->getIdentity());
			myHitbox->setIsSolid(respondingEntityHitbox->getIsSolid());
			myHitbox->setCollisionStyle(respondingEntityHitbox->getCollisionStyle());
			myHitbox->setSlope(respondingEntityHitbox->getSlope());
			
			collisionData->pyCollisionData_.attr("myHitbox") = myHitbox;

			boost::shared_ptr<Hitbox> collidingHitbox = collisionData->getCollidingHitbox();
		
			transformedX = requestingEntityHitbox->getLeft();

			Rect collisionRect2;
			collisionRect2.x = requestingEntity->getPosition()->getX() + transformedX;
			collisionRect2.y = requestingEntity->getPosition()->getY() + requestingEntityHitbox->getTop();		
			collisionRect2.w = requestingEntityHitbox->getWidth();
			collisionRect2.h = requestingEntityHitbox->getHeight();
		
			collidingHitbox->setCollisionRect(collisionRect2);
			collidingHitbox->setIdentity(requestingEntityHitbox->getIdentity());
			collidingHitbox->setIsSolid(requestingEntityHitbox->getIsSolid());
			collidingHitbox->setCollisionStyle(requestingEntityHitbox->getCollisionStyle());
			collidingHitbox->setSlope(requestingEntityHitbox->getSlope());			
		}
		catch(error_already_set &)
		{
			std::cout<<"Error setting collision data. Is the getCollisionData function implemented?"<<std::endl;
			debugger_->handlePythonError();
		}

		respondingCollidable->preCollisionExit(collisionData);
		
		// The collision data object for tiles doesn't get generated each collision, it is created at initialization and persists
		// for the entire lifetime of the tile. Don't clear the collision data for these.
		if (requestingEntity->getEntityMetadata()->getEntityTypeId() != ids_->ENTITY_TILE)
		{
			collisionData->pyCollisionData_ = boost::python::object();
		}
	}
	else
	{
		if (debugger_->getDebugMode() == true && debugger_->getDebugModeRenderHitboxes() == true)
		{
			respondingCollidable->setHitboxStatuses(respondingEntityHitbox->getIdentity(), false);
		}
	}
}