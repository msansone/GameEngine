/* -------------------------------------------------------------------------
** PhysicsData.hpp
**
** The PhysicsData class stores the physics data variables used by the 
** physics manager. Each room has its own copy.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PHYSICSDATA_HPP_
#define _PHYSICSDATA_HPP_

#include <vector>
#include <map>

#include "CameraManager.hpp"
#include "CollidableGrid.hpp"
#include "CollisionRecord.hpp"
#include "EntityComponents.hpp"

namespace firemelon
{
	class PhysicsData
	{
	public:
		friend class PhysicsManager;
		
		PhysicsData();
		virtual ~PhysicsData();
	
		void	adjustCollisionGrid(int index);
		void	getVisibleGridCells(int &startX, int &endX, int &startY, int &endY);

		void	setCameraManager(boost::shared_ptr<CameraManager> cameraManager);

	private:
		
		std::vector<boost::shared_ptr<EntityComponents>>	entityComponents_;
		
		// A list of entityInstanceId of entities with dynamics controllers.
		// For certain tasks, only dynamic entities need to be processed.
		std::vector<int>									dynamicEntities_;

		// A list of entityInstanceId of entities with dynamics controllers and hitbox controllers.
		// For certain tasks, only dynamic collidable entities need to be processed.
		std::vector<int>									dynamicCollidableEntities_;

		boost::shared_ptr<CameraManager>					cameraManager_;

		boost::shared_ptr<CollidableGrid>					collisionGrid_;
	};
}

#endif // _PHYSICSDATA_HPP_