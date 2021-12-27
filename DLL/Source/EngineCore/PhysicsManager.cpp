#include "..\..\Headers\EngineCore\PhysicsManager.hpp"

using namespace firemelon;
using namespace boost::python;

PhysicsManager::PhysicsManager(CollisionDispatcherPtr collisionDispatcher, CollisionTesterPtr collisionTester, boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger)
{
	linearAlgebraUtility_ = boost::make_shared<LinearAlgebraUtility>(LinearAlgebraUtility());

	collisionDispatcher_ = collisionDispatcher;

	collisionTester_ = collisionTester;

	debugger_ = debugger;

	ids_ = ids;

	renderer_ = nullptr;
}

PhysicsManager::~PhysicsManager()
{

}

void PhysicsManager::addRoom(RoomId roomId)
{
	PhysicsData* physicsData = new PhysicsData();

	physicsDataList_.push_back(physicsData);

	collisionDispatcher_->addRoom();
}

void PhysicsManager::addEntityComponents(int roomIndex, boost::shared_ptr<EntityComponents> components)
{
	boost::shared_ptr<HitboxController> hitboxController = components->getHitboxController();
	
	int entityInstanceId = components->getEntityMetadata()->getEntityInstanceId();

	bool isDynamic = components->getHasDynamicsController();
	bool isCollidable = hitboxController != nullptr;

	// Don't add if the components already exist in the list
	bool componentsExistInList = false;

	int size = physicsDataList_[roomIndex]->entityComponents_.size();

	for (int i = 0; i < size; i++)
	{
		if (physicsDataList_[roomIndex]->entityComponents_[i]->getEntityMetadata()->getEntityInstanceId() == entityInstanceId)
		{
			componentsExistInList = true;
			break;
		}
	}

	if (componentsExistInList == false)
	{
		// An entry for this entity components object has not been added yet.
		int index = physicsDataList_[roomIndex]->entityComponents_.size();

		physicsDataList_[roomIndex]->entityComponents_.push_back(components);
	
		if (isDynamic == true)
		{
			// Add a reference to this dynamics entity to all the grid cells it occupies.
			// Also maintain a list in the dynamicsController itself to all the grid cells it's in, so they can quickly be removed later.

			// Store the index to all the components objects that have a dynamics controller.
			physicsDataList_[roomIndex]->dynamicEntities_.push_back(index);

			if (isCollidable == true)
			{
				physicsDataList_[roomIndex]->dynamicCollidableEntities_.push_back(index);
			}
		}
	}
}

void PhysicsManager::removeEntityComponents(int roomIndex, int ownerId)
{
	// Search for the physics object with the given owner ID. (Consider a search friendly data structure here).
	int size = physicsDataList_[roomIndex]->entityComponents_.size();
	for (int i = size - 1; i >= 0; i--)
	{
		boost::shared_ptr<EntityComponents> components = physicsDataList_[roomIndex]->entityComponents_[i];

		if (components->getEntityMetadata()->getEntityInstanceId() == ownerId)
		{			
			// No longer need to have an element in the dynamicEntities list pointing to this.
			// Find the element in the dynamicslist list that is pointing to this components object, and clear it.

			// The dynamicEntities list should always be sorted, as it always only inserts the entityComponents_.size(),
			// and upon items being removed, indexes greater than the removed value are decremented.

			// Find the lower bound. If the element found matches the value we are looking for,
			// Remove it and reindex everything higher.
			std::vector<int>::iterator itr;
			itr = std::lower_bound(physicsDataList_[roomIndex]->dynamicEntities_.begin(), physicsDataList_[roomIndex]->dynamicEntities_.end(), i); 
				
			int lowerBoundPosition = itr - physicsDataList_[roomIndex]->dynamicEntities_.begin();

			bool foundInList = false;
			
			// Adjust all of the items in the list higher than the deleted index.
			int size2 = physicsDataList_[roomIndex]->dynamicEntities_.size();

			if (lowerBoundPosition < size2)
			{
				if (physicsDataList_[roomIndex]->dynamicEntities_[lowerBoundPosition] == i)
				{
					foundInList = true;
				}
			}

			for (int j = 0; j < size2; j++)
			{
				if (physicsDataList_[roomIndex]->dynamicEntities_[j] > i)
				{
					physicsDataList_[roomIndex]->dynamicEntities_[j]--;
				}
			}

			if (foundInList == true)
			{
				physicsDataList_[roomIndex]->dynamicEntities_.erase(itr);
			}

			// Do the same for the dynamic collidables list.
			itr = std::lower_bound(physicsDataList_[roomIndex]->dynamicCollidableEntities_.begin(), physicsDataList_[roomIndex]->dynamicCollidableEntities_.end(), i);

			lowerBoundPosition = itr - physicsDataList_[roomIndex]->dynamicCollidableEntities_.begin();

			foundInList = false;

			// Adjust all of the items in the list higher than the deleted index.
			size2 = physicsDataList_[roomIndex]->dynamicCollidableEntities_.size();

			if (lowerBoundPosition < size2)
			{
				if (physicsDataList_[roomIndex]->dynamicCollidableEntities_[lowerBoundPosition] == i)
				{
					foundInList = true;
				}
			}

			for (int j = 0; j < size2; j++)
			{
				if (physicsDataList_[roomIndex]->dynamicCollidableEntities_[j] > i)
				{
					physicsDataList_[roomIndex]->dynamicCollidableEntities_[j]--;
				}
			}

			if (foundInList == true)
			{
				physicsDataList_[roomIndex]->dynamicCollidableEntities_.erase(itr);
			}

			// Remove the components object from the list.
			physicsDataList_[roomIndex]->entityComponents_.erase(physicsDataList_[roomIndex]->entityComponents_.begin() + i);
			
			physicsDataList_[roomIndex]->adjustCollisionGrid(i);
		
			// If the entity is currently a part of a collision, remove it.
			collisionDispatcher_->removeCollisionRecord(roomIndex, ownerId);
		}
	}
}

void PhysicsManager::clearAllForces(int roomIndex)
{
	int size = physicsDataList_[roomIndex]->dynamicEntities_.size();
	for (int i = 0; i < size; i++)
	{
		int componentsIndex = physicsDataList_[roomIndex]->dynamicEntities_[i];
		boost::shared_ptr<EntityComponents> components = physicsDataList_[roomIndex]->entityComponents_[componentsIndex];
		DynamicsController* dynamicsController = components->getDynamicsController();

		// Dynamics controller may not have been created, if a python error occured in the entity initialize.
		if (dynamicsController != nullptr)
		{
			dynamicsController->clearForcesX();
			dynamicsController->clearForcesY();
		}
	}
}

//void PhysicsManager::clearAllAttachments(int roomIndex)
//{
//	int size = physicsDataList_[roomIndex]->dynamicEntities_.size();
//	for (int i = 0; i < size; i++)
//	{
//		int componentsIndex = physicsDataList_[roomIndex]->dynamicEntities_[i];
//		boost::shared_ptr<EntityComponents> components = physicsDataList_[roomIndex]->entityComponents_[componentsIndex];
//		DynamicsController* dynamicsController = components->getDynamicsController();
//
//		// Dynamics controller may not have been created, if a python error occured in the entity initialize.
//		if (dynamicsController != nullptr)
//		{
//			dynamicsController->clearAttachment();
//		}
//	}
//}

//void PhysicsManager::clearAllPreviousAttachments(int roomIndex)
//{
//	PhysicsData* physicsData = physicsDataList_[roomIndex];
//
//	int size = physicsData->dynamicEntities_.size();
//	for (int i = 0; i < size; i++)
//	{
//		int entityIndex = physicsData->dynamicEntities_[i];
//
//		boost::shared_ptr<EntityComponents> components = physicsData->entityComponents_[entityIndex];
//
//		DynamicsController* dynamicsController = components->getDynamicsController();
//
//		// Dynamics controller may not have been created, if a python error occured in the entity initialize.
//		if (dynamicsController != nullptr)
//		{
//			dynamicsController->clearPreviousAttachments();
//		}
//	}
//}

void PhysicsManager::cleanup()
{
	int size = physicsDataList_.size();

	for (int i = 0; i < size; i++)
	{
		clear(i);
	}
}

void PhysicsManager::clear(int roomIndex)
{
	physicsDataList_[roomIndex]->entityComponents_.clear();

	physicsDataList_[roomIndex]->collisionGrid_->clear();

	physicsDataList_[roomIndex]->dynamicEntities_.clear();
	
	collisionDispatcher_->clearAll(roomIndex);
}

void PhysicsManager::initialize(int roomIndex)
{
	physicsDataList_[roomIndex]->cameraManager_ = cameraManager_;

	// Add the entities to the collision grid.
	int size = physicsDataList_[roomIndex]->entityComponents_.size();
	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<EntityComponents> components = physicsDataList_[roomIndex]->entityComponents_[i];

		boost::shared_ptr<HitboxController> hc = components->getHitboxController();

		if (hc != nullptr)
		{
			// Get a rect that bounds all of the hitboxes in the hitbox container associated with the current object.
			Rect bounds = getAxisAlignedBoundingBox(components);

			if (bounds.area > 1)
			{
				bounds.x += hc->getOwnerPosition()->getX() + hc->getStagePosition()->getX();
				bounds.y += hc->getOwnerPosition()->getY() + hc->getStagePosition()->getY();;

				// Now calculate which grid cells that this object intersects
				int gridCellSize = physicsDataList_[roomIndex]->collisionGrid_->getCellSize();
				int gridCols = physicsDataList_[roomIndex]->collisionGrid_->getCols();
				int gridRows = physicsDataList_[roomIndex]->collisionGrid_->getRows();

				int startCol = (int)(bounds.x / gridCellSize);

				if (startCol >= gridCols)
				{
					startCol = gridCols - 1;
				}
				else if (startCol < 0)
				{
					startCol = 0;
				}

				int endCol = (int)((bounds.x + bounds.w - 1) / gridCellSize);

				if (endCol >= gridCols)
				{
					endCol = gridCols - 1;
				}
				else if (endCol < 0)
				{
					endCol = 0;
				}

				int startRow = (int)(bounds.y / gridCellSize);

				if (startRow >= gridRows)
				{
					startRow = gridRows - 1;
				}
				else if (startRow < 0)
				{
					startRow = 0;
				}

				int endRow = (int)((bounds.y + bounds.h - 1) / gridCellSize);

				if (endRow >= gridRows)
				{
					endRow = gridRows - 1;
				}
				else if (endRow < 0)
				{
					endRow = 0;
				}

				hc->setCollisionGridCellStartRow(startRow);
				hc->setCollisionGridCellEndRow(endRow);
				hc->setCollisionGridCellStartCol(startCol);
				hc->setCollisionGridCellEndCol(endCol);
				hc->hasActiveColliders_ = true;

				// Populate the list of grid cells this object is in, and add its index to the grid itself.
				for (int row = startRow; row <= endRow; row++)
				{
					for (int col = startCol; col <= endCol; col++)
					{
						physicsDataList_[roomIndex]->collisionGrid_->addToGrid(row, col, i);
					}
				}
			}
			else
			{
				hc->hasActiveColliders_ = false;
			}
		}
	}
}

void PhysicsManager::initCollisionGrid(int roomIndex, int cellSize, int rows, int cols)
{
	physicsDataList_[roomIndex]->collisionGrid_->init(cellSize, rows, cols);
}

void PhysicsManager::update(int roomIndex, double time)
{
	int ownerID = -1;
	int collisionID = -1;

	boost::shared_ptr<EntityComponents> components;

	std::vector<int> attachmentRoots;

	// Loop through all of the entity components objects that have a dynamicscontroller and update them.
	// It's possible that an entitycomponents object without a dynamicscontroller has had its 
	// hitboxes changed by changing state or changing animation frame. 
	// However at this time, the only object that this would be is a tile, which do not change
	// state and are not animated, so this does not need to be addressed currently.
	int size = physicsDataList_[roomIndex]->dynamicEntities_.size();

	for (int i = 0; i < size; i++)
	{
		int index = physicsDataList_[roomIndex]->dynamicEntities_[i];

		components = physicsDataList_[roomIndex]->entityComponents_[index];

		DynamicsController* dynamicsController = components->getDynamicsController();
		boost::shared_ptr<HitboxController> hitboxController = components->getHitboxController();
		
		// Dynamics controller may not have been created, if a python error occured in the entity initialize.
		if (dynamicsController != nullptr)
		{
			dynamicsController->integrate(time);

			if (dynamicsController->getAttachedEntityCount() > 0 && dynamicsController->getAttachedTo() == nullptr)
			{
				attachmentRoots.push_back(index);
			}

			if (hitboxController != nullptr)
			{
				// Update the collision grid.
				if (hitboxController->getActiveHitboxCount() > 0)
				{
					// Loop through the gridcells array and remove all of the references to 
					// this entity from the collision grid.
					int oldStartRow = hitboxController->getCollisionGridCellStartRow();
					int oldEndRow = hitboxController->getCollisionGridCellEndRow();
					int oldStartCol = hitboxController->getCollisionGridCellStartCol();
					int oldEndCol = hitboxController->getCollisionGridCellEndCol();

					int newStartRow = -1;
					int newEndRow = -1;
					int newStartCol = -1;
					int newEndCol = -1;

					// Get a rect that bounds all of the currently active hitboxes for this entity.
					Rect bounds = getAxisAlignedBoundingBox(components);

					bool oldHadActiveColliders = hitboxController->hasActiveColliders_;

					if (bounds.area > 1)
					{
						boost::shared_ptr<PhysicsSnapshot> snapshot = dynamicsController->getPhysicsSnapshot();
						bounds.x += snapshot->getPositionInt()->getX();
						bounds.y += snapshot->getPositionInt()->getY();

						int gridCellSize = physicsDataList_[roomIndex]->collisionGrid_->getCellSize();
						int gridCols = physicsDataList_[roomIndex]->collisionGrid_->getCols();
						int gridRows = physicsDataList_[roomIndex]->collisionGrid_->getRows();

						// Now calculate which grid cells this object intersects with.
						newStartCol = (int)(bounds.x / gridCellSize);

						if (newStartCol >= gridCols)
						{
							newStartCol = gridCols - 1;
						}
						else if (newStartCol < 0)
						{
							newStartCol = 0;
						}

						newEndCol = (int)((bounds.x + bounds.w - 1) / gridCellSize);

						if (newEndCol >= gridCols)
						{
							newEndCol = gridCols - 1;
						}
						else if (newEndCol < 0)
						{
							newEndCol = 0;
						}

						newStartRow = (int)(bounds.y / gridCellSize);

						if (newStartRow >= gridRows)
						{
							newStartRow = gridRows - 1;
						}
						else if (newStartRow < 0)
						{
							newStartRow = 0;
						}

						newEndRow = (int)((bounds.y + bounds.h - 1) / gridCellSize);

						if (newEndRow >= gridRows)
						{
							newEndRow = gridRows - 1;
						}
						else if (newEndRow < 0)
						{
							newEndRow = 0;
						}

						hitboxController->setCollisionGridCellStartRow(newStartRow);
						hitboxController->setCollisionGridCellEndRow(newEndRow);
						hitboxController->setCollisionGridCellStartCol(newStartCol);
						hitboxController->setCollisionGridCellEndCol(newEndCol);
						hitboxController->hasActiveColliders_ = true;
					}
					else
					{
						hitboxController->hasActiveColliders_ = false;
					}

					if (hitboxController->hasActiveColliders_ == true && oldHadActiveColliders == false)
					{
						// The previous frame had no active colliders. All grid cells should be added.
						for (int row = newStartRow; row <= newEndRow; row++)
						{
							for (int col = newStartCol; col <= newEndCol; col++)
							{
								physicsDataList_[roomIndex]->collisionGrid_->addToGrid(row, col, index);
							}
						}
					}
					else if (hitboxController->hasActiveColliders_ == false && oldHadActiveColliders == true)
					{
						// The previous frame had active colliders, but the current frame does not. Remove the
						// old grid cells.
						for (int row = oldStartRow; row <= oldEndRow; row++)
						{
							for (int col = oldStartCol; col <= oldEndCol; col++)
							{
								int collidableCount = physicsDataList_[roomIndex]->collisionGrid_->getCollidableCount(row, col);

								for (int l = 0; l < collidableCount; l++)
								{
									if (physicsDataList_[roomIndex]->collisionGrid_->grid_[row][col][l] == index)
									{
										// Delete and break out of the inner loop.							
										physicsDataList_[roomIndex]->collisionGrid_->grid_[row][col].erase(physicsDataList_[roomIndex]->collisionGrid_->grid_[row][col].begin() + l);

										break;
									}
								}
							}
						}
					}
					else if (hitboxController->hasActiveColliders_ == true && oldHadActiveColliders == true)
					{
						// Both the previous frame and the current have active colliders. Iterate over the intersection and determine
						// which grid cells should be added and which should be removed.

						// If the intersecting cells have not changed, there's no need to update the grid.
						if (!(oldStartRow == newStartRow && oldEndRow == newEndRow && oldStartCol == newStartCol && oldEndCol == newEndCol))
						{
							// Do an intersection test of the old grid cells and the new. Any cell that is intersecting can be ignored.
							// Any old cell that is not intersecting must be removed, and any new cell that is not intersecting must be added.

							int startRow = std::min(oldStartRow, newStartRow);
							int endRow = std::max(oldEndRow, newEndRow);
							int startCol = std::min(oldStartCol, newStartCol);
							int endCol = std::max(oldEndCol, newEndCol);

							for (int row = startRow; row <= endRow; row++)
							{
								for (int col = startCol; col <= endCol; col++)
								{
									bool isNewCell = containsCell(row, col, newStartRow, newEndRow, newStartCol, newEndCol);
									bool isOldCell = containsCell(row, col, oldStartRow, oldEndRow, oldStartCol, oldEndCol);

									// If the current cell being checked is a new cell, but not an old cell, it must be added.
									// If it is a new cell and an old cell, it should be ignored.
									// If it is not a new cell but it is an old cell, it should be removed.
									// If it is neither an old cell nor a new cell, it should be ignored.

									if (isNewCell == true && isOldCell == false)
									{
										physicsDataList_[roomIndex]->collisionGrid_->addToGrid(row, col, index);
									}
									else if (isNewCell == false && isOldCell == true)
									{
										int collidableCount = physicsDataList_[roomIndex]->collisionGrid_->getCollidableCount(row, col);

										for (int l = 0; l < collidableCount; l++)
										{
											if (physicsDataList_[roomIndex]->collisionGrid_->grid_[row][col][l] == index)
											{
												// Delete and break out of the inner loop.							
												physicsDataList_[roomIndex]->collisionGrid_->grid_[row][col].erase(physicsDataList_[roomIndex]->collisionGrid_->grid_[row][col].begin() + l);

												break;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Now that the updates are complete, recursively run the attachment updates on the root entities.
	size = attachmentRoots.size();
	
	for (int i = 0; i < size; i++)
	{
		int index = attachmentRoots[i];

		components = physicsDataList_[roomIndex]->entityComponents_[index];
		
		DynamicsController* rootAttachee = components->getDynamicsController();

		rootAttachee->updateAttachments();
	}

	// Loop through all static entities that are not tiles. If their animation frame has changed
	// or their state has changed, update the collision grid.
}

bool PhysicsManager::containsCell(int row, int col, int startRow, int endRow, int startCol, int endCol)
{
	if (row < startRow || row > endRow || col < startCol || col > endCol)
	{
		return false;
	}

	return true;
}

void PhysicsManager::postUpdate(int roomIndex)
{
	// All entities have been updated. Clear the forces and attachments.
	clearAllForces(roomIndex);	
}

void PhysicsManager::collisionDetection(int roomIndex)
{
	// Collider: The current object that is colliding with others.
	EntityComponentsPtr	componentsCollider;
	HitboxControllerPtr hitboxControllerCollider;
	DynamicsController*	dynamicsControllerCollider;
	
	// Collidee: the object that is being collided with.
	EntityComponentsPtr componentsCollidee;
	HitboxControllerPtr	hitboxControllerCollidee;
	DynamicsController* dynamicsControllerCollidee;

	std::vector<int> entitiesToTest;

	collisionDispatcher_->clear(roomIndex);

	// dynamicCollidableEntities_ stores IDs of entityComponents_ that have set a dynamicsController.
	entitiesToTest = physicsDataList_[roomIndex]->dynamicCollidableEntities_;
	
	// Solid collisions must be resolved immediately, and every other collision must be recorded and tested a second time
	// at the end of the loop, because the solid collision resolution may have made it such that the collision is no
	// longer occurring.
	
	int size = entitiesToTest.size();

	for (int i = 0; i < size; i++)
	{
		// Get the collider data.
		int colliderComponentsIndex = entitiesToTest[i];

		componentsCollider			 = physicsDataList_[roomIndex]->entityComponents_[colliderComponentsIndex];

		hitboxControllerCollider	 = componentsCollider->getHitboxController();

		int colliderEntityInstanceId = componentsCollider->getEntityMetadata()->getEntityInstanceId();


		if (hitboxControllerCollider->hasActiveColliders_ == true)
		{
			dynamicsControllerCollider = componentsCollider->getDynamicsController();
		
			// Loop through each collision grid cell that this collidable exists in, and test
			// for collisions with the other collidables that exist in each cell.
			int colliderStartRow = hitboxControllerCollider->getCollisionGridCellStartRow();

			int colliderEndRow	 = hitboxControllerCollider->getCollisionGridCellEndRow();

			int colliderStartCol = hitboxControllerCollider->getCollisionGridCellStartCol();

			int colliderEndCol	 = hitboxControllerCollider->getCollisionGridCellEndCol();

			for (int j = colliderStartRow; j <= colliderEndRow; j++)
			{
				for (int k = colliderStartCol; k <= colliderEndCol; k++)
				{
					int row = j;

					int col = k;
				
					// Iterate through each entity referenced in the current grid cell.
					std::vector<int> collisionGridCellEntityIds = physicsDataList_[roomIndex]->collisionGrid_->grid_[row][col];

					int size = collisionGridCellEntityIds.size();

					for (int l = 0; l < size; l++)
					{
						// Get the collidee data.
						int collideeComponentsIndex = collisionGridCellEntityIds[l];

						componentsCollidee			 = physicsDataList_[roomIndex]->entityComponents_[collideeComponentsIndex];

						int collideeEntityInstanceId = componentsCollidee->getEntityMetadata()->getEntityInstanceId();

						hitboxControllerCollidee	 = componentsCollidee->getHitboxController();

						dynamicsControllerCollidee	 = componentsCollidee->getDynamicsController();

						// Don't want an object to collide with itself.
						if (colliderEntityInstanceId != collideeEntityInstanceId)
						{
							int colliderPositionX = hitboxControllerCollider->getOwnerPosition()->getX() + hitboxControllerCollider->getStagePosition()->getX();

							int colliderPositionY = hitboxControllerCollider->getOwnerPosition()->getY() + hitboxControllerCollider->getStagePosition()->getY();
					
							int collideePositionX = hitboxControllerCollidee->getOwnerPosition()->getX() + hitboxControllerCollidee->getStagePosition()->getX();

							int collideePositionY = hitboxControllerCollidee->getOwnerPosition()->getY() + hitboxControllerCollidee->getStagePosition()->getY();

							// Loop through each of the active hitboxes in the collider.
							int colliderActiveHitboxCount = hitboxControllerCollider->getActiveHitboxCount();

							for (int m = 0; m < colliderActiveHitboxCount; m++)
							{
								// Convert the collider hitbox into world space.
								int colliderHitboxId = hitboxControllerCollider->getActiveHitboxId(m);	

								boost::shared_ptr<Hitbox> colliderHitbox = hitboxManager_->getHitbox(colliderHitboxId);
						
								Rect rectCollider = colliderHitbox->getCollisionRect();
								
								rectCollider.x += colliderPositionX;

								rectCollider.y += colliderPositionY;

								bool colliderIsSolid = colliderHitbox->getIsSolid();

								int colliderHitboxIdentity = colliderHitbox->getIdentity();

								// Loop through each of the active hitboxes in the collidee.
								int collideeActiveHitboxCount = hitboxControllerCollidee->getActiveHitboxCount();

								for (int n = 0; n < collideeActiveHitboxCount; n++)
								{
									int collideeHitboxId = hitboxControllerCollidee->getActiveHitboxId(n);

									boost::shared_ptr<Hitbox> collideeHitbox = hitboxManager_->getHitbox(collideeHitboxId);
									
									unsigned char collideeEdgeFlags = 0x00;

									if (collideeHitbox->getUseTopEdge())
									{
										collideeEdgeFlags |= 0x08;
									}

									if (collideeHitbox->getUseBottomEdge())
									{
										collideeEdgeFlags |= 0x04;
									}

									if (collideeHitbox->getUseLeftEdge())
									{
										collideeEdgeFlags |= 0x02;
									}
									
									if (collideeHitbox->getUseRightEdge())
									{
										collideeEdgeFlags |= 0x01;
									}
									
									// Build temporary rects using the dimensions of the hitboxes, offset by the owner entity positions.
									Rect rectCollidee = collideeHitbox->getCollisionRect();
									
									rectCollidee.x += collideePositionX;
									
									rectCollidee.y += collideePositionY;

									bool collideeIsSolid = collideeHitbox->getIsSolid();

									int collideeHitboxIdenity = collideeHitbox->getIdentity();
									
									// Do the actual collision test now.
									// If either hitbox is rotated (i.e. the y coordinate of the two top corners are different), use SAT collision test.

									bool useSatCollisionTest = false;

									if (collideeHitbox->transformedCorners_[0].y != collideeHitbox->transformedCorners_[1].y || colliderHitbox->transformedCorners_[0].y != colliderHitbox->transformedCorners_[1].y)
									{
										useSatCollisionTest = true;
									}

									CollisionTestResultPtr collisionTestResult;

									CollisionTestResultPtr reverseCollisionTestResult = boost::make_shared<CollisionTestResult>(CollisionTestResult());

									if (useSatCollisionTest == true)
									{
										PositionPtr colliderOwnerPosition = hitboxControllerCollider->getOwnerPosition();

										PositionPtr colliderStagePosition = hitboxControllerCollider->getStagePosition();

										std::vector<Vertex2> colliderCorners;

										for (int j = 0; j < colliderHitbox->transformedCorners_.size(); j++)
										{
											Vertex2 colliderCorner;

											colliderCorner.x = colliderHitbox->transformedCorners_[j].x + colliderOwnerPosition->getX() + colliderStagePosition->getX();

											colliderCorner.y = colliderHitbox->transformedCorners_[j].y + colliderOwnerPosition->getY() + colliderStagePosition->getY();

											colliderCorners.push_back(colliderCorner);
										}

										PositionPtr collideeOwnerPosition = hitboxControllerCollidee->getOwnerPosition();

										PositionPtr collideeStagePosition = hitboxControllerCollidee->getStagePosition();

										std::vector<Vertex2> collideeCorners;

										for (int j = 0; j < colliderCorners.size(); j++)
										{
											Vertex2 collideeCorner;

											collideeCorner.x = collideeHitbox->transformedCorners_[j].x + collideeOwnerPosition->getX() + collideeStagePosition->getX();

											collideeCorner.y = collideeHitbox->transformedCorners_[j].y + collideeOwnerPosition->getY() + collideeStagePosition->getY();

											collideeCorners.push_back(collideeCorner);
										}

										collisionTestResult = collisionTester_->collisionTestSat(colliderCorners, collideeCorners);
									}
									else
									{
										// Basic AABB collision test.
										collisionTestResult = collisionTester_->collisionTestAabb(rectCollider, rectCollidee, collideeEdgeFlags, colliderIsSolid);
									}
									
									if (collisionTestResult->collisionOccurred == true)
									{
										// A collision has occurred. Don't call the user implemented virtual collision functions just yet, because we don't
										// want anything messing with the loop control variables while we're still looping through. A collision that causes
										// a state change could mess with the active hitbox list. Add it to a process queue for later consumption.

										// Note that solid collisions must be resolved immediately, and recorded collisions must be tested again later,
										// because the solid collision may have moved other collisions such that they are no longer colliding.								
										int faceNormalX = collisionTestResult->faceNormal->getX();

										int faceNormalY = collisionTestResult->faceNormal->getY();
										
										// Reverse the face normal and intrusion.
										reverseCollisionTestResult->faceNormal->setX(faceNormalX * -1);

										reverseCollisionTestResult->faceNormal->setY(faceNormalY * -1);

										reverseCollisionTestResult->intrusion->setX(collisionTestResult->intrusion->getX() * -1);
										reverseCollisionTestResult->intrusion->setY(collisionTestResult->intrusion->getY() * -1);

										HitboxIdentity collideeHitboxIdentity = collideeHitbox->getIdentity();
										HitboxIdentity colliderHitboxIdentity = colliderHitbox->getIdentity();
										
										CollisionRecord cr;

										cr.collisionTestResult_ = collisionTestResult;

										cr.reverseCollisionTestResult_ = reverseCollisionTestResult;

										cr.roomIndex_ = roomIndex;

										cr.colliderHitboxId_ = colliderHitboxId;
										cr.collideeHitboxId_ = collideeHitboxId;

										cr.colliderId_ = colliderEntityInstanceId;
										cr.collideeId_ = collideeEntityInstanceId;	

										cr.collideeHitboxIdentity_ = collideeHitboxIdentity;
										cr.colliderHitboxIdentity_ = colliderHitboxIdentity;
									
										cr.collideeComponents_ = componentsCollidee;
										cr.colliderComponents_ = componentsCollider;

										cr.collideeFullRect_ = rectCollidee;
										cr.colliderFullRect_ = rectCollider;

										#ifdef _USESTRINGKEY_
											// Build a collision key that identifies the collision between these two objects.
											// Lower ID should always be first.
											std::string sColliderId = boost::lexical_cast<std::string>(cr.colliderId_);
											std::string sColliderIdentity = boost::lexical_cast<std::string>(cr.colliderIdentity_);

											std::string sCollideeId = boost::lexical_cast<std::string>(cr.collideeId_);
											std::string sCollideeIdentity = boost::lexical_cast<std::string>(cr.collideeIdentity_);
									
											std::string key;

											if (cr.colliderId_ < cr.collideeId_)
											{
												key = sColliderId + "." + sColliderIdentity + "," + sCollideeId + "." + sCollideeIdentity;
											}
											else
											{
												key = sCollideeId + "." + sCollideeIdentity + "," + sColliderId + "." + sColliderIdentity;														
											}

											cr.key_ = key;

										#else

											// Pack the data into the key.


											// Set the primary and secondary data.
											if (cr.colliderId_ < cr.collideeId_)
											{
												unsigned long long int part1 = 0;
												unsigned long long int part2 = 0;
												unsigned long long int part3 = 0;
												unsigned long long int part4 = 0;

												part1 = ((unsigned long long int)cr.colliderId_) << 48;

												part2 = ((unsigned long long int)cr.collideeId_) << 32;

												part3 = (unsigned long long int)cr.colliderHitboxIdentity_ << 16;

												part4 = cr.collideeHitboxIdentity_;

												cr.key_ = part1 | part2 | part3 | part4;
											}
											else
											{
												unsigned long long int part1 = 0;
												unsigned long long int part2 = 0;
												unsigned long long int part3 = 0;
												unsigned long long int part4 = 0;

												part1 = ((unsigned long long int)cr.collideeId_) << 48;
												
												part2 = ((unsigned long long int)cr.colliderId_) << 32;
												
												part3 = (unsigned long long int)cr.collideeHitboxIdentity_ << 16;
												
												part4 = cr.colliderHitboxIdentity_;
												
												cr.key_ = part1 | part2 | part3 | part4;											
											}
										#endif

										// Handle solid collisions immediately. Flag them to run the collision repsonse later, no need to check
										// Other collisions that are valid must be checked again later. only solid collisions can be flagged to ignore this second check.
										if (colliderIsSolid == true && collideeIsSolid == true)
										{
											preSolidCollision(cr);

											cr.ignoreSecondCheck = true;
										}

										collisionDispatcher_->addCollisionRecord(cr);
									}
								}
							}				
						}
					}
				}
			}	
		}
	}

	collisionDispatcher_->dispatch(roomIndex);
}

Rect PhysicsManager::getAxisAlignedBoundingBox(boost::shared_ptr<EntityComponents> components)
{
	// Calculate the axis aligned bounding box relative to the owner's position.

	boost::shared_ptr<Hitbox> hitbox;

	DynamicsController* d = components->getDynamicsController();

	boost::shared_ptr<HitboxController> hc = components->getHitboxController();
	
	int boundingBoxTop = 0;
	int boundingBoxLeft = 0;
	int boundingBoxRight = 0;
	int boundingBoxBottom = 0;
	
	// Get the active hitboxes, if any, and determine an overall bounding rect that contains them.	
	int numActiveHitboxes = hc->getActiveHitboxCount();

	for (int j = 0; j < numActiveHitboxes; j++)
	{
		int activeHitboxId = hc->getActiveHitboxId(j);
		
		// Apply the rotations from the stage and animation slot that the hitboxes are from.
		hitbox = hitboxManager_->getHitbox(activeHitboxId);
		
		int hitboxLeftmostCorner = hitbox->getLeftmostCorner();

		int hitboTopmostCorner = hitbox->getTopmostCorner();

		int hitboxRightmostCorner = hitbox->getRightmostCorner();

		int hitboxBottommostCorner = hitbox->getBottommostCorner();

		if (j == 0)
		{
			// Initialize to the bounds of the first hitbox.
			boundingBoxLeft = hitboxLeftmostCorner;

			boundingBoxTop = hitboTopmostCorner;

			boundingBoxRight = hitboxRightmostCorner;

			boundingBoxBottom = hitboxBottommostCorner;
		}
		else
		{
			// Expand the left, right, top, and bottom as necessary.
			if (hitboxLeftmostCorner < boundingBoxLeft)
			{
				boundingBoxLeft = hitboxLeftmostCorner;
			}

			if (hitboxRightmostCorner > boundingBoxRight)
			{
				boundingBoxRight = hitboxRightmostCorner;
			}

			if (hitboTopmostCorner < boundingBoxTop)
			{
				boundingBoxTop = hitboTopmostCorner;
			}

			if (hitboxBottommostCorner > boundingBoxBottom)
			{
				boundingBoxBottom = hitboxBottommostCorner;
			}
		}
	}

	Rect rectAxisAlignedBoundingBox;
	rectAxisAlignedBoundingBox.x = boundingBoxLeft;
	rectAxisAlignedBoundingBox.w = boundingBoxRight - boundingBoxLeft;
	rectAxisAlignedBoundingBox.y = boundingBoxTop;
	rectAxisAlignedBoundingBox.h = boundingBoxBottom - boundingBoxTop;

	rectAxisAlignedBoundingBox.area = rectAxisAlignedBoundingBox.h * rectAxisAlignedBoundingBox.w;

	return rectAxisAlignedBoundingBox;
}

void PhysicsManager::renderDynamicsControllers(int roomIndex)
{
	boost::shared_ptr<CameraController> camera = cameraManager_->getActiveCamera();

	int size = physicsDataList_[roomIndex]->dynamicEntities_.size();

	for (int i = 0; i < size; i++)
	{
		int index = physicsDataList_[roomIndex]->dynamicEntities_[i];

		boost::shared_ptr<EntityComponents> components = physicsDataList_[roomIndex]->entityComponents_[index];

		DynamicsController* dynamicsController = components->getDynamicsController();


		float cameraX = 0;

		float cameraY = 0;

		//x and y are in world space. they need to be offset by the camera location.
		if (camera != nullptr)
		{
			auto cameraPosition = camera->getPosition();

			cameraX = cameraPosition->getClampedX();

			cameraY = cameraPosition->getClampedY();
		}
		else
		{
			std::cout << "Warning: Camera is null" << std::endl;
		}
		//float dcX = dynamicsController->getPositionX();

		//float dcY = dynamicsController->getPositionY();

		//float x = (float)(dcX - cameraX);

		//float y = (float)(dcY - cameraY);

		{
			PythonAcquireGil lock;
		
			auto simulatableScript = components->getCodeBehindContainer()->getSimulatableCodeBehind()->getScript();

			PyObj pyDynamicsController = simulatableScript->getPyDynamicsController();

			bool isDynamicsControllerAvailable = pyDynamicsController;

			if (isDynamicsControllerAvailable == true)
			{
				boost::python::object pyRender = pyDynamicsController.attr("render");

				bool isRenderAvailable = !pyRender.is_none();

				if (isRenderAvailable == true)
				{
					try
					{
						pyRender((int)cameraX, (int)cameraY);
					}
					catch (error_already_set &)
					{
						debugger_->handlePythonError();
					}
				}
			}
		}

		dynamicsController->render((int)cameraX, (int)cameraY);
	}
}

void PhysicsManager::renderHitboxes(int roomIndex)
{
	boost::shared_ptr<CameraController> camera = cameraManager_->getActiveCamera();

	if (camera != nullptr)
	{
		int cameraX = camera->getPosition()->getClampedX();
		int cameraY = camera->getPosition()->getClampedY();

		int startCellX = 0;
		int startCellY = 0;
		int endCellX = 0;
		int endCellY = 0;

		physicsDataList_[roomIndex]->getVisibleGridCells(startCellX, endCellX, startCellY, endCellY);

		for (int i = startCellY; i <= endCellY; i++)
		{
			for (int j = startCellX; j <= endCellX; j++)
			{
				// Iterate through each object referenced in the current grid cell.			
				int size = physicsDataList_[roomIndex]->collisionGrid_->grid_[i][j].size();

				for (int k = 0; k < size; k++)
				{
					int index = physicsDataList_[roomIndex]->collisionGrid_->grid_[i][j][k];

					boost::shared_ptr<EntityComponents> components = physicsDataList_[roomIndex]->entityComponents_[index];

					int entityInstanceId = components->getEntityMetadata()->getEntityInstanceId();

					boost::shared_ptr<HitboxController> hc = components->getHitboxController();

					DynamicsController* dc = components->getDynamicsController();

					int activeHitboxCount = hc->getActiveHitboxCount();

					int positionX = hc->getOwnerPosition()->getX() + hc->getStagePosition()->getX();
					int positionY = hc->getOwnerPosition()->getY() + hc->getStagePosition()->getY();

					for (int k = 0; k < activeHitboxCount; k++)
					{
						int activeHitboxId = hc->getActiveHitboxId(k);

						boost::shared_ptr<Hitbox> hb = hitboxManager_->getHitbox(activeHitboxId);

						// Render the transformed hitbox rectangle.

						// Build a list of vertices transformed to screen space.
						std::vector<Vertex2> vertices;

						for (size_t i = 0; i < hb->transformedCorners_.size(); i++)
						{
							Vertex2 vertex;

							vertex.x = hb->transformedCorners_[i].x + positionX - cameraX;

							vertex.y = hb->transformedCorners_[i].y + positionY - cameraY;

							vertices.push_back(vertex);
						}

						renderer_->drawPolygon(vertices, hb->getDebugColor());
						
						//int hitboxX = hb->getLeft();
						//int hitboxY = hb->getTop();
						//int hitboxW = hb->getWidth();
						//int hitboxH = hb->getHeight();
						//
						//int transformedHitboxX = hitboxX + hc->getActiveHitboxOffset(k)->getX();

						//int transformedHitboxY = hitboxY + hc->getActiveHitboxOffset(k)->getY();

						//// If the look vector is reversed, mirror any hitboxes across this entity's stage centerpoint.
						//bool flipHorizontally = false;

						//if (dc != nullptr)
						//{
						//	if (dc->getLook()->getX() < 0.0)
						//	{
						//		// Transform so that the center of the stage is the new origin.
						//		// Mirror the x position across the origin.
						//		// Re-transform to restore the original center point.
						//		int centerX = hc->getStageWidth() / 2;

						//		int tempHitboxX = (transformedHitboxX + hitboxW) - centerX;

						//		tempHitboxX *= -1;

						//		transformedHitboxX = tempHitboxX + centerX;
						//	}
						//}


						//int hitboxWorldX = positionX + transformedHitboxX;
						//int hitboxWorldY = positionY + transformedHitboxY;

						////x and y are in world space. they need to be offset by the camera location.
						//float x = (float)(hitboxWorldX - cameraX);
						//float y = (float)(hitboxWorldY - cameraY);
						//int w = hitboxW;
						//int h = hitboxH;

						//bool isColliding = hc->getActiveHitboxCollisionStatus(activeHitboxId);

						//if (isColliding == true)
						//{
						//	renderer_->renderDrawRect(x, y, w, h, 1.0f, 0.0f, 0.0f, 1.0f);
						//}
						//else
						//{
						//	renderer_->renderDrawRect(x, y, w, h, 0.0f, 1.0f, 0.0f, 1.0f);						
						//}
					}
				}
			}
		}
	}
}

void PhysicsManager::preSolidCollision(CollisionRecord collisionRecord)
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
	solidCollision(colliderComponents, collideeComponents, hbCollider, hbCollidee, collisionRecord.colliderHitboxId_, collisionRecord.collideeHitboxId_, collisionRecord.collisionTestResult_->faceNormal, collisionRecord.collisionTestResult_->intrusion);

	// Process the reverse collision for the collider.
	solidCollision(collideeComponents, colliderComponents, hbCollidee, hbCollider, collisionRecord.collideeHitboxId_, collisionRecord.colliderHitboxId_, collisionRecord.reverseCollisionTestResult_->faceNormal, collisionRecord.reverseCollisionTestResult_->intrusion);

	return;
}

void PhysicsManager::solidCollision(boost::shared_ptr<EntityComponents> respondingEntity,	      boost::shared_ptr<EntityComponents> requestingEntity,
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
			// Copy the intrusion and face normal vectors.
			collisionData->faceNormal_->setX(faceNormal->getX());

			collisionData->faceNormal_->setY(faceNormal->getY());

			collisionData->intrusion_->setX(intrusion->getX());

			collisionData->intrusion_->setY(intrusion->getY());

			collisionData->setCollidingEntityController(requestingEntity->getEntityController());
			collisionData->setCollidingEntityStateMachineController(requestingEntity->getStateMachineController());
			collisionData->setCollidingEntityDynamicsController(requestingEntity->getDynamicsController());

			collisionData->pyCollisionData_.attr("collidingEntityType") = requestingEntity->getEntityMetadata()->getEntityTypeId();

			//findmetodo Set the face normal here.
			//collisionData->pyCollisionData_.attr("collisionDirection") = direction;

			// Fill in the "myHitbox" values from the responding hitbox. The rect must be in world space.
			boost::shared_ptr<Hitbox> myHitbox = collisionData->getMyHitbox();




			// Copy the ID
			myHitbox->id_ = respondingEntityHitbox->id_;

			// Copy the collision rect.
			Rect myCollisionRect = respondingEntityHitbox->getCollisionRect();

			myHitbox->setCollisionRect(myCollisionRect);

			// Set the owner position.
			myHitbox->ownerPosition_->setX(respondingEntity->getPosition()->getX());
			myHitbox->ownerPosition_->setY(respondingEntity->getPosition()->getY());

			// Set the hitbox metadata.
			myHitbox->setIdentity(respondingEntityHitbox->getIdentity());

			myHitbox->setIsSolid(respondingEntityHitbox->getIsSolid());

			myHitbox->setCollisionStyle(respondingEntityHitbox->getCollisionStyle());

			myHitbox->setSlope(respondingEntityHitbox->getSlope());

			// Copy over the hitbox corners.
			for (size_t i = 0; i < respondingEntityHitbox->transformedCorners_.size(); i++)
			{
				myHitbox->transformedCorners_[i].x = respondingEntityHitbox->transformedCorners_[i].x;

				myHitbox->transformedCorners_[i].y = respondingEntityHitbox->transformedCorners_[i].y;
			}

			// Set the hitbox property in the python collision data object.
			collisionData->pyCollisionData_.attr("myHitbox") = myHitbox;


			// Now repeat the same process for the hitbox being collided with.
			boost::shared_ptr<Hitbox> collidingHitbox = collisionData->getCollidingHitbox();

			// Copy the hitbox ID
			collidingHitbox->id_ = requestingEntityHitbox->id_;

			// Copy the collision rect.
			Rect collidingCollisionRect = requestingEntityHitbox->getCollisionRect();

			// Set the owner position
			collidingHitbox->ownerPosition_->setX(requestingEntity->getPosition()->getX());
			collidingHitbox->ownerPosition_->setY(requestingEntity->getPosition()->getY());

			collidingHitbox->setCollisionRect(collidingCollisionRect);

			// Set the hitbox metadata.
			collidingHitbox->setIdentity(requestingEntityHitbox->getIdentity());

			collidingHitbox->setIsSolid(requestingEntityHitbox->getIsSolid());

			collidingHitbox->setCollisionStyle(requestingEntityHitbox->getCollisionStyle());

			collidingHitbox->setSlope(requestingEntityHitbox->getSlope());

			// Copy the transformed corners.
			for (size_t i = 0; i < requestingEntityHitbox->transformedCorners_.size(); i++)
			{
				collidingHitbox->transformedCorners_[i].x = requestingEntityHitbox->transformedCorners_[i].x;

				collidingHitbox->transformedCorners_[i].y = requestingEntityHitbox->transformedCorners_[i].y;
			}
		}
		catch (error_already_set &)
		{
			std::cout << "Error setting collision data. Is the getCollisionData function implemented?" << std::endl;
			debugger_->handlePythonError();
		}

		// Use the collisionData object we just constructed to handle the solid collision in the code behind.
		respondingCollidable->preSolidCollision(collisionData);

		// The collision data object for tiles doesn't get generated each collision, it is created at initialization and persists
		// for the entire lifetime of the tile. Don't clear the collision data for these.
		if (requestingEntity->getEntityMetadata()->getEntityTypeId() != ids_->ENTITY_TILE)
		{
			// Done with the collision data. Set the py instance to none.
			collisionData->pyCollisionData_ = boost::python::object();
		}
	}
}