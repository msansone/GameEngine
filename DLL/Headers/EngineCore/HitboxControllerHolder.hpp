/* -------------------------------------------------------------------------
** HitboxControllerHolder.hpp
**
** The HitboxControllerHolder class is used to store a hitbox controller.
** It is necessary because the creation of a hitbox controller, which is stored
** at the highest level in the EntityComponents class, needs to be created at
** a lower level, in the EntityController class. So rather than storing a
** pointer to a hitbox controller directly in the EntityComponents class
** it stores a pointer to a hitbox controller holder, which contains the
** hitbox controller. This can then be given to the entity controller
** which will use it to create the hitbox controller.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _HITBOXCONTROLLERHOLDER_HPP_
#define _HITBOXCONTROLLERHOLDER_HPP_

#include "HitboxController.hpp"

namespace firemelon
{
	class HitboxControllerHolder
	{
	public:
		friend class EntityController;

		HitboxControllerHolder();
		virtual ~HitboxControllerHolder();

		bool				getHasHitboxController();

		boost::shared_ptr<HitboxController>	getHitboxController();
		void								setHitboxController(boost::shared_ptr<HitboxController> hitboxController);

	private:

		boost::shared_ptr<HitboxController>	hitboxController_;
	};

	typedef boost::shared_ptr<HitboxControllerHolder> HitboxControllerHolderPtr;
}

#endif // _HITBOXCONTROLLERHOLDER_HPP_