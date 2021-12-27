/* -------------------------------------------------------------------------
** PhysicsState.hpp
**
** The PhysicsSnapshot class stores all of the data necessary for a snapshot
** of an object in a physics simulation. (Position, Velocity, etc.)
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PHYSICSSNAPSHOT_HPP_
#define _PHYSICSSNAPSHOT_HPP_

#include "Vec2.hpp"

namespace firemelon
{
	class PhysicsSnapshot
	{
	public:
		friend class DynamicsController;

		PhysicsSnapshot();
		virtual ~PhysicsSnapshot();
	
		boost::shared_ptr<Vec2<float>>	getAccelerationPy();
		boost::shared_ptr<Vec2<float>>	getAcceleration();
		boost::shared_ptr<Vec2<float>>	getVelocityPy();
		boost::shared_ptr<Vec2<float>>	getVelocity();
		boost::shared_ptr<Vec2<float>>	getPositionPy();
		boost::shared_ptr<Vec2<float>>	getPosition();
		boost::shared_ptr<Vec2<float>>	getNetForcePy();
		boost::shared_ptr<Vec2<float>>	getNetForce();
		boost::shared_ptr<Vec2<float>>	getMovementPy();
		boost::shared_ptr<Vec2<float>>	getMovement();
		boost::shared_ptr<Vec2<int>>	getPositionIntPy();
		boost::shared_ptr<Vec2<int>>	getPositionInt();
		boost::shared_ptr<Vec2<int>>	getPositionIntDeltaPy();
		boost::shared_ptr<Vec2<int>>	getPositionIntDelta();
		
		
		float							getMassPy();
		float							getMass();

	private:
		
		boost::shared_ptr<Vec2<float>>	acceleration_;
		boost::shared_ptr<Vec2<float>>	velocity_;
		boost::shared_ptr<Vec2<float>>	position_;
		boost::shared_ptr<Vec2<float>>	netForce_;
		boost::shared_ptr<Vec2<float>>	movement_;
		
		boost::shared_ptr<Vec2<int>>	positionInt_;
		boost::shared_ptr<Vec2<int>>	positionIntDelta_;

		float							mass_;

		int								ownerId_;
	};
}

#endif // _PHYSICSSNAPSHOT_HPP_