#include "..\..\Headers\EngineCore\CollisionTester.hpp"

using namespace firemelon;

CollisionTester::CollisionTester()
{
	linearAlgebraUtility_ = boost::make_shared<LinearAlgebraUtility>(LinearAlgebraUtility());
}

CollisionTester::~CollisionTester()
{

}

CollisionTestResultPtr CollisionTester::collisionTestSat(std::vector<Vertex2> colliderCorners, std::vector<Vertex2> collideeCorners)
{
	bool separatingAxisExists = false;

	CollisionTestResultPtr collisionTestResult = boost::make_shared<CollisionTestResult>(CollisionTestResult());

	// Build a list of axes using the hitbox corners to get their face normals.
	Vertex2 xAxis1;
	Vertex2 yAxis1;

	Vertex2 xAxis2;
	Vertex2 yAxis2;

	std::vector<Vertex2> axes;

	// x axis 1
	xAxis1.x = colliderCorners[1].x - colliderCorners[0].x;

	xAxis1.y = colliderCorners[1].y - colliderCorners[0].y;

	linearAlgebraUtility_->normalize(xAxis1);

	axes.push_back(xAxis1);

	// y axis 1
	yAxis1.x = colliderCorners[2].x - colliderCorners[1].x;

	yAxis1.y = colliderCorners[2].y - colliderCorners[1].y;

	linearAlgebraUtility_->normalize(yAxis1);

	axes.push_back(yAxis1);

	// x axis 2
	xAxis2.x = collideeCorners[1].x - collideeCorners[0].x;

	xAxis2.y = collideeCorners[1].y - collideeCorners[0].y;

	linearAlgebraUtility_->normalize(xAxis2);

	axes.push_back(xAxis2);

	// y axis 2
	yAxis2.x = collideeCorners[2].x - collideeCorners[1].x;

	yAxis2.y = collideeCorners[2].y - collideeCorners[1].y;

	linearAlgebraUtility_->normalize(yAxis2);

	axes.push_back(yAxis2);

	double minimumOverlap = 0;

	int minimumCollisionAxisIndex = -1;

	for (int i = 0; i < axes.size(); i++)
	{
		Vertex2 axis = axes[i];

		double colliderMinimum = 0;

		double colliderMaximum = 0;

		double collideeMinimum = 0;

		double collideeMaximum = 0;

		for (int j = 0; j < colliderCorners.size(); j++)
		{
			double dot = linearAlgebraUtility_->dot(colliderCorners[j], axis);

			if (j == 0)
			{
				colliderMinimum = dot;

				colliderMaximum = dot;
			}
			else
			{
				if (dot < colliderMinimum)
				{
					colliderMinimum = dot;
				}

				if (dot > colliderMaximum)
				{
					colliderMaximum = dot;
				}
			}
		}


		for (int j = 0; j < collideeCorners.size(); j++)
		{
			double dot = linearAlgebraUtility_->dot(collideeCorners[j], axis);

			if (j == 0)
			{
				collideeMinimum = dot;

				collideeMaximum = dot;
			}
			else
			{
				if (dot < collideeMinimum)
				{
					collideeMinimum = dot;
				}

				if (dot > collideeMaximum)
				{
					collideeMaximum = dot;
				}
			}
		}

		// Check if there is a separating axis.
		if (colliderMinimum > collideeMaximum || colliderMaximum < collideeMinimum)
		{
			collisionTestResult->collisionOccurred = false;

			return collisionTestResult;
		}
		else
		{
			// Get the overlap, and store the axis index, to get the intrusion vector.
			double overlap = std::min(colliderMaximum, collideeMaximum) - std::max(colliderMinimum, collideeMinimum);

			if (minimumCollisionAxisIndex == -1)
			{
				// No overlap has been set yet, so just initialize it here.
				minimumCollisionAxisIndex = i;

				minimumOverlap = overlap;
			}
			else
			{
				// An overlap exists. If this one is smaller, use it as the overlap.
				if (overlap < minimumOverlap)
				{
					minimumCollisionAxisIndex = i;

					minimumOverlap = overlap;
				}
			}
		}
	}

	// If the code has made it here, no separating axis can be found, therefore a collision must have occurred.
	collisionTestResult->collisionOccurred = true;

	collisionTestResult->intrusion->setX(axes[minimumCollisionAxisIndex].x * minimumOverlap);

	collisionTestResult->intrusion->setY(axes[minimumCollisionAxisIndex].y * minimumOverlap);

	return collisionTestResult;
}


CollisionTestResultPtr CollisionTester::collisionTestAabb(Rect rectCollider, Rect rectCollidee, unsigned char collideeEdgeFlags, bool solidCollision)
{
	CollisionTestResultPtr collisionTestResult = boost::make_shared<CollisionTestResult>(CollisionTestResult());

	int left1, left2;
	int right1, right2;
	int top1, top2;
	int bottom1, bottom2;

	left1 = rectCollider.x;
	left2 = rectCollidee.x;
	right1 = rectCollider.x + rectCollider.w - 1;
	right2 = rectCollidee.x + rectCollidee.w - 1;
	top1 = rectCollider.y;
	top2 = rectCollidee.y;
	bottom1 = rectCollider.y + rectCollider.h - 1;
	bottom2 = rectCollidee.y + rectCollidee.h - 1;

	bool collision = true;

	if (bottom1 < top2)
	{
		collision = false;
	}

	if (top1 > bottom2)
	{
		collision = false;
	}

	if (right1 < left2)
	{
		collision = false;
	}

	if (left1 > right2)
	{
		collision = false;
	}

	collisionTestResult->collisionOccurred = collision;

	if (collision == true)
	{
		// Test the depth of the collision to find from which direction is the smallest.
		// If an edge is flagged as internal, it should be ignored.
		int above = bottom1 - top2;
		int below = top1 - bottom2;
		int left = right1 - left2;
		int right = left1 - right2;

		HitboxEdgeDepth hedAbove;
		hedAbove.depth = above;
		hedAbove.edge = HITBOX_EDGE_TOP;

		HitboxEdgeDepth hedBelow;
		hedBelow.depth = below;
		hedBelow.edge = HITBOX_EDGE_BOTTOM;

		HitboxEdgeDepth hedLeft;
		hedLeft.depth = left;
		hedLeft.edge = HITBOX_EDGE_LEFT;

		HitboxEdgeDepth hedRight;
		hedRight.depth = right;
		hedRight.edge = HITBOX_EDGE_RIGHT;

		bool ignoreTopEdge = false;
		bool ignoreBottomEdge = false;
		bool ignoreRightEdge = false;
		bool ignoreLeftEdge = false;

		// Check which edges have neighbors.

		// If the edgeflag is not set, ignore it.
		if ((collideeEdgeFlags & 0x08) == 0)
		{
			ignoreTopEdge = true;
		}

		if ((collideeEdgeFlags & 0x04) == 0)
		{
			ignoreBottomEdge = true;
		}

		if ((collideeEdgeFlags & 0x02) == 0)
		{
			ignoreLeftEdge = true;
		}

		if ((collideeEdgeFlags & 0x01) == 0)
		{
			ignoreRightEdge = true;
		}

		// Make all values positive.
		if (hedRight.depth < 0)
		{
			hedRight.depth *= -1;
		}

		if (hedLeft.depth < 0)
		{
			hedLeft.depth *= -1;
		}

		if (hedAbove.depth < 0)
		{
			hedAbove.depth *= -1;
		}

		if (hedBelow.depth < 0)
		{
			hedBelow.depth *= -1;
		}

		// Find which one is the minimum.
		// If the minimum is flagged to be ignored, use the second smallest.
		// Continue until unignored minimum is found.

		std::vector<HitboxEdgeDepth> vals;
		vals.push_back(hedRight);
		vals.push_back(hedLeft);
		vals.push_back(hedBelow);
		vals.push_back(hedAbove);

		std::sort(vals.begin(), vals.begin() + 4, compare_hitboxEdgeDepth);

		// vals is now a sorted array of hitbox edge depths. Loop through and check if each element is to be ignored.

		/*
		For solid collisions, only test two levels deep otherwise the collisions will behave incorrectly.

		Consider the following example, solid hitbox 4 collides with 1, 2, and 3 in a given frame.
		Hitbox 2's top edge is contiguous with hitbox 1's bottom edge, and hitbox 2's right edge
		is contiguous with hitbox 3's left edge. So when the collision between hitbox 4 and 2, it
		would return a directional result of FROM_LEFT, even though the desired result would be
		COLLISION_NONE.

		However if a non-solid collision was occurring, you would never want it to return
		COLLISION_NONE, because a collision is most certainly occurring.

		,.,,,,,,,,,
		,         ,
		....,..       ,
		.   , .  4    ,
		.  1, .       ,
		....,........ ,
		.   ,,,,,,,,,,,
		.  2  .  3  .
		.............

		*/

		int depth = 4;

		if (solidCollision == true)
		{
			depth = 2;
		}

		bool found = false;

		for (int i = 0; i < depth; i++)
		{
			switch (vals[i].edge)
			{
			case HITBOX_EDGE_TOP:

				if (ignoreTopEdge == false)
				{
					// Found the result.
					//direction = COLLISION_RESULT_DIRECTION_FROM_ABOVE;
					found = true;

					collisionTestResult->intrusion->setX(0);

					collisionTestResult->intrusion->setY(-1 * vals[i].depth);

					collisionTestResult->faceNormal->setX(0);

					collisionTestResult->faceNormal->setY(-1);
				}

				break;

			case HITBOX_EDGE_BOTTOM:

				if (ignoreBottomEdge == false)
				{
					// Found the result.
					//direction = COLLISION_RESULT_DIRECTION_FROM_BELOW;
					found = true;

					collisionTestResult->intrusion->setX(0);

					collisionTestResult->intrusion->setY(vals[i].depth);

					collisionTestResult->faceNormal->setX(0);

					collisionTestResult->faceNormal->setY(1);
				}

				break;

			case HITBOX_EDGE_RIGHT:

				if (ignoreRightEdge == false)
				{
					// Found the result.
					//direction = COLLISION_RESULT_DIRECTION_FROM_RIGHT;
					found = true;

					collisionTestResult->intrusion->setX(vals[i].depth);

					collisionTestResult->intrusion->setY(0);

					collisionTestResult->faceNormal->setX(1);

					collisionTestResult->faceNormal->setY(0);
				}

				break;

			case HITBOX_EDGE_LEFT:

				if (ignoreLeftEdge == false)
				{
					// Found the result.
					//direction = COLLISION_RESULT_DIRECTION_FROM_LEFT;
					found = true;

					collisionTestResult->intrusion->setX(-1 * vals[i].depth);

					collisionTestResult->intrusion->setY(0);

					collisionTestResult->faceNormal->setX(-1);

					collisionTestResult->faceNormal->setY(0);
				}

				break;
			}

			if (found == true)
			{
				break;
			}
		}
	}

	return collisionTestResult;
}