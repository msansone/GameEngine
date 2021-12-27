/* -------------------------------------------------------------------------
** CollisionTester.hpp
**
** The CollisionTester class tests for a collision between two colliders.
** It is intended to be used as an abtract base class, with derived child 
** classes for specific collision tests, such as AABB (Axis-aligned bounding
** boxes) or SAT (Separating Axis Theorem) collision tests.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONTESTER_HPP_
#define _COLLISIONTESTER_HPP_

#include <vector>

#include <boost/shared_ptr.hpp>

#include "CollisionTestResult.hpp"
#include "LinearAlgebraUtility.hpp"
#include "Types.hpp"

namespace firemelon
{
	class CollisionTester
	{
	public:

		CollisionTester();
		virtual ~CollisionTester();

		CollisionTestResultPtr	collisionTestAabb(Rect rectCollider, Rect rectCollidee, unsigned char collideeEdgeFlags, bool ignoreContiguousTileEdges);

		CollisionTestResultPtr	collisionTestSat(std::vector<Vertex2> colliderCorners, std::vector<Vertex2> collideeCorners);
		
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

		LinearAlgebraUtilityPtr	linearAlgebraUtility_;
	};

	typedef boost::shared_ptr<CollisionTester> CollisionTesterPtr;
}

#endif // _COLLISIONTESTER_HPP_