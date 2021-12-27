/* -------------------------------------------------------------------------
** StateMachineControllerHolder.hpp
**
** The StateMachineControllerHolder class is used to store a state machine
** controller. It is necessary because the creation of a state machine 
** controller, which is stored at the highest level in the EntityComponents 
** class, needs to be created at a lower level, in the EntityController class. 
** So rather than storing a pointer to a stage controller directly in the 
** EntityComponents class it stores a pointer to a stage controller holder, 
** which contains the stage controller. This can then be given to the entity 
** controller which will use it to create the stage controller.
**
** Basically, the "holder" classes exist so that the order the various things
** they hold are created in does not matter. The holders can be shared among
** all of them, even if the held object is not instantiated yet.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STATEMACHINECONTROLLERHOLDER_HPP_
#define _STATEMACHINECONTROLLERHOLDER_HPP_

#include "StateMachineController.hpp"

namespace firemelon
{
	class StateMachineControllerHolder
	{
	public:
		friend class EntityController;

		StateMachineControllerHolder();
		virtual ~StateMachineControllerHolder();

		bool						getHasStateMachineController();

		StateMachineControllerPtr	getStateMachineController();
		void						setStateMachineController(StateMachineControllerPtr stateMachineController);

	private:

		StateMachineControllerPtr	stateMachineController_;
	};

	typedef boost::shared_ptr<StateMachineControllerHolder> StateMachineControllerHolderPtr;
}

#endif // _STATEMACHINECONTROLLERHOLDER_HPP_