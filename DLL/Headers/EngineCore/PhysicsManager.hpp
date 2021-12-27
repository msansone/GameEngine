/* -------------------------------------------------------------------------
** PhysicsManager.hpp
**
** The PhysicsManager class contains references to all of the dynamics controllers
** and hitbox containers created by the entities. It is responsible for updating 
** the integration step for the dynamics controllers, and processing collision 
** detection with the hitbox containers.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */
#ifndef _PHYSICSMANAGER_HPP_
#define _PHYSICSMANAGER_HPP_

#include <algorithm>
#include <iostream>
#include <vector>
#include <list>
#include <map>
#include <set>

#include <boost/signals2.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/python.hpp>


#include "CameraManager.hpp"
#include "CollisionDispatcher.hpp"
#include "Debugger.hpp"
#include "EntityComponents.hpp"
#include "Macros.hpp"
#include "MapLayer.hpp"
#include "PhysicsData.hpp"
#include "Renderer.hpp"
#include "Types.hpp"

namespace firemelon
{
	class PhysicsManager
	{
	public:
		friend class GameEngine;
		friend class GameStateManager;
		friend class RoomManager;
		friend class Room;

		PhysicsManager(CollisionDispatcherPtr collisionDispatcher, CollisionTesterPtr collisionTester, boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger);
		virtual ~PhysicsManager();
	
		void	addEntityComponents(int roomIndex, boost::shared_ptr<EntityComponents> components);
		void	removeEntityComponents(int roomIndex, int entityId);

		void	clear(int roomIndex);
		void	initialize(int roomIndex);
		void	update(int roomIndex, double time);
		void	postUpdate(int roomIndex);
	
		void	addRoom(RoomId roomId);
		
		void	initCollisionGrid(RoomId roomId, int cellSize, int rows, int cols);

	private:
		
		// The HitboxEdge enum values represent the bounding edges of a collision rectangle.
		// It is used when edges are flagged as invalid.
		enum HitboxEdge
		{
			HITBOX_EDGE_TOP = 0,
			HITBOX_EDGE_BOTTOM = 1,
			HITBOX_EDGE_RIGHT = 2,
			HITBOX_EDGE_LEFT = 3
		};
	
		// The HitboxEdgeDepth structure measures the distance from the colliding rectangle's edge
		// to the collided with rectangles opposite edge (i.e. the bottom of rectangle 1 vs. the top of rectangle 2).
		// This is used to determine the direction from which the collision occurred. The smallest
		// distance is the direction to use.
		struct HitboxEdgeDepth
		{
			int depth;
			HitboxEdge edge;
		};
		
		// Compare hitbox edge depth values to determine which one is the smallest, to find the 
		// collision direction.
		static bool compare_hitboxEdgeDepth(HitboxEdgeDepth& a, HitboxEdgeDepth& b) 
		{
			return (a.depth < b.depth);
		};

		void						cleanup();

		bool						containsCell(int row, int col, int startRow, int endRow, int startCol, int endCol);

		void						clearAllForces(int roomIndex);

		void						collisionDetection(int roomIndex);

		void						renderHitboxes(int roomIndex);

		void						renderDynamicsControllers(int roomIndex);

		void						preSolidCollision(CollisionRecord cr);

		void						solidCollision(boost::shared_ptr<EntityComponents> respondingEntity,	     boost::shared_ptr<EntityComponents> requestingEntity,
												   boost::shared_ptr<Hitbox>           respondingEntityHitbox,   boost::shared_ptr<Hitbox>           requestingEntityHitbox,
												   int                                 respondingEntityHitboxId, int                                 requestingEntityHitboxId,
												   Vec2IPtr                            faceNormal,               Vec2IPtr                            intrusion);

		// Calculates the smallest bounding rectangle for all currently active hitboxes.
		// This is used to determine which cells of the collision grid the entity is in.
		Rect						getAxisAlignedBoundingBox(boost::shared_ptr<EntityComponents> components);

		boost::shared_ptr<CameraManager>			cameraManager_;

		boost::shared_ptr<CollisionDispatcher>		collisionDispatcher_;

		CollisionTesterPtr							collisionTester_;
		
		DebuggerPtr									debugger_;

		boost::shared_ptr<HitboxManager>			hitboxManager_;

		boost::shared_ptr<BaseIds>                  ids_;

		LinearAlgebraUtilityPtr						linearAlgebraUtility_;

		std::vector<PhysicsData*>					physicsDataList_;

		boost::shared_ptr<Renderer>					renderer_;
	};
}

#endif // _PHYSICSMANAGER_HPP_