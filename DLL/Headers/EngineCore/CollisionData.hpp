/* -------------------------------------------------------------------------
** CollisionData.hpp
** 
** The CollisionData class contains information necessary to process a 
** collision, such as the colliding object IDs, their hitbox identities, and 
** their dynamics controller objects. Subclasses can be implemented for when 
** specific results are necessary.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLISIONDATA_HPP_
#define _COLLISIONDATA_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "BaseIds.hpp"
#include "Debugger.hpp"
#include "EntityController.hpp"
#include "Hitbox.hpp"
#include "StateMachineController.hpp"
#include "Types.hpp"

#include <boost/python.hpp>

namespace firemelon
{
	enum CollisionResolution
	{
		COLLISION_RESOLUTION_SOLID_PRIORITY = 1,
		COLLISION_RESOLUTION_SOLID_YIELD = 2,
		COLLISION_RESOLUTION_PERMEABLE = 3
	};

	class FIREMELONAPI CollisionData
	{
	public:
		friend class CollidableScript;
		friend class CollisionDispatcher;
		friend class Entity;
		friend class GameStateManager;
		friend class PhysicsManager;
		friend class TileCollidableCodeBehind;

		CollisionData();
		virtual ~CollisionData();
	
		boost::shared_ptr<Hitbox>	getMyHitboxPy();
		boost::shared_ptr<Hitbox>	getMyHitbox();

		boost::shared_ptr<Hitbox>	getCollidingHitboxPy();
		boost::shared_ptr<Hitbox>	getCollidingHitbox();
				
		void						setCollidingEntityControllerPy(EntityControllerPtr value);
		void						setCollidingEntityController(EntityControllerPtr value);
		
		EntityControllerPtr			getCollidingEntityControllerPy();
		EntityControllerPtr			getCollidingEntityController();
		
		//void						setCollidingEntityStateMachineControllerPy(boost::shared_ptr<StateMachineController> value);
		
		StateMachineControllerPtr	getCollidingEntityStateMachineControllerPy();
		StateMachineControllerPtr	getCollidingEntityStateMachineController();

		DynamicsController*			getCollidingEntityDynamicsControllerPy();
		DynamicsController*			getCollidingEntityDynamicsController();

		void						setCollidingEntityTypePy(EntityTypeId value);
		void						setCollidingEntityType(EntityTypeId value);
		
		EntityTypeId				getCollidingEntityTypePy();
		EntityTypeId				getCollidingEntityType();

		boost::python::object		getPythonInstance();

		void						setSolidCollision(bool value);
		bool						getSolidCollisionPy();
		bool						getSolidCollision();

		// Setter/Getter for a key-value pair of strings.
		void						setValuePy(std::string name, std::string value);
		void						setValue(std::string name, std::string value);
		
		std::string					getValue(std::string name);
		std::string					getValuePy(std::string name);

		Vec2IPtr					getIntrusionPy();
		Vec2IPtr					getIntrusion();

		Vec2IPtr					getFaceNormalPy();
		Vec2IPtr					getFaceNormal();

	private:

		void	setCollidingEntityDynamicsController(DynamicsController* value);
		void	setCollidingEntityStateMachineController(boost::shared_ptr<StateMachineController> value);

		// The hitbox that is being collided with in the owner entity.
		boost::shared_ptr<Hitbox>	myHitbox_;

		// The hitbox that is colliding with it.
		boost::shared_ptr<Hitbox>	collidingHitbox_;

		// Colliding entity data.
		EntityTypeId				collidingEntityType_;
		EntityControllerPtr			collidingEntityController_;
		StateMachineControllerPtr	collidingEntityStateMachineController_;
		DynamicsController*			collidingEntityDynamicsController_;
		
		// Collision data.		
		Vec2IPtr					faceNormal_;

		Vec2IPtr					intrusion_;

		// If a solid collision was processed for this collision.
		bool						solidCollision_;

		stringmap					values_;

		boost::python::object		pyCollisionData_;
		//bool						isPythonResponse_;
	};

	typedef boost::shared_ptr<CollisionData> CollisionDataPtr;
}

#endif // _COLLISIONDATA_HPP_