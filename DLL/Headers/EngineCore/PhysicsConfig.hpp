/* -------------------------------------------------------------------------
** PhysicsConfig.hpp
** 
** The PhysicsConfig class stores the physics configuration data, such as
** linear damp, and minimum velocity, and provides an interface to modify 
** these values.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PHYSICSCONFIG_HPP_
#define _PHYSICSCONFIG_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/shared_ptr.hpp>

#include "Vec2.hpp"

namespace firemelon
{
	class FIREMELONAPI PhysicsConfig
	{
	public:	
		friend class DynamicsController;

		PhysicsConfig();
		virtual ~PhysicsConfig();

		boost::shared_ptr<Vec2<float>>	getGravity();
		boost::shared_ptr<Vec2<float>>	getGravityPy();

		boost::shared_ptr<Vec2<float>>	getLinearDamp();
		boost::shared_ptr<Vec2<float>>	getLinearDampPy();

		boost::shared_ptr<Vec2<float>>	getMinimumVelocity();
		boost::shared_ptr<Vec2<float>>	getMinimumVelocityPy();

		float							getTimeScale();
		float							getTimeScalePy();

		void							setTimeScale(float value);
		void							setTimeScalePy(float value);

	private:

		void	copy(PhysicsConfig* physicsConfig);

		boost::shared_ptr<Vec2<float>>	gravity_;

		boost::shared_ptr<Vec2<float>>	linearDamp_;

		boost::shared_ptr<Vec2<float>>	minimumVelocity_;

		float							timeScale_;
	};

	typedef boost::shared_ptr<PhysicsConfig> PhysicsConfigPtr;
}

#endif // _PHYSICSCONFIG_HPP_