/* -------------------------------------------------------------------------
** DynamicsControllerHolder.hpp
** 
** The DynamicsControllerHolder class is used to store a dynamics controller.
** It is necessary because the creation of a dynamics controller, which is stored
** at the highest level in the EntityComponents class, needs to be created at
** a lower level, in the EntityController class. So rather than storing a
** pointer to a dynamics controller directly in the EntityComponents class
** it stores a pointer to a dynamics controller holder, which contains the
** dynamics controller. This can then be given to the entity controller
** which will use it to create the dynamics controller.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _DYNAMICSCONTROLLERHOLDER_HPP_
#define _DYNAMICSCONTROLLERHOLDER_HPP_

#include "DynamicsController.hpp"

namespace firemelon
{
	class DynamicsControllerHolder
	{
	public:
		friend class EntityController;

		DynamicsControllerHolder(PhysicsConfigPtr physicsConfig);
		virtual ~DynamicsControllerHolder();
		
		bool				getHasDynamicsController();

		DynamicsController* getDynamicsController();
		void				setDynamicsController(DynamicsController* dynamicsController);

	private:
	
		// The creation of the actual dynamics controller object should be delayed.
		// Instead of creating it right away, set a flag that indicates it should be
		// created later.
		bool				hasDynamicsController_;

		DynamicsController* dynamicsController_;

		PhysicsConfigPtr	physicsConfig_;
	};
}

#endif // _DYNAMICSCONTROLLERHOLDER_HPP_