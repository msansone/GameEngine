/* -------------------------------------------------------------------------
** StageControllerHolder.hpp
**
** The StageControllerHolder class is used to store a stage controller.
** It is necessary because the creation of a stage controller, which is stored
** at the highest level in the EntityComponents class, needs to be created at
** a lower level, in the EntityController class. So rather than storing a
** pointer to a stage controller directly in the EntityComponents class
** it stores a pointer to a stage controller holder, which contains the
** stage controller. This can then be given to the entity controller
** which will use it to create the stage controller.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STAGECONTROLLERHOLDER_HPP_
#define _STAGECONTROLLERHOLDER_HPP_

#include "StageController.hpp"

namespace firemelon
{
	class StageControllerHolder
	{
	public:
		friend class EntityController;

		StageControllerHolder();
		virtual ~StageControllerHolder();

		bool				getHasStageController();

		StageControllerPtr	getStageController();
		void				setStageController(StageControllerPtr stageController);

	private:

		StageControllerPtr	stageController_;
	};

	typedef boost::shared_ptr<StageControllerHolder> StageControllerHolderPtr;
}

#endif // _STAGECONTROLLERHOLDER_HPP_