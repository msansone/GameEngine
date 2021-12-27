/* -------------------------------------------------------------------------
** StateMachineController.hpp
**
** The StateMachineController class is a component that entities can create if they want
** to have a graphical component. It contains a list of states and animations and
** is responsible for keep tracking of which ones are currently active. It can 
** either store its own list of states and animations, or it can store a entityTemplate 
** and use the states and animations contained within. For entities like tiles that 
** have a lot of instances, and generally don't change, it is more efficient to store 
** the shared tile entityTemplate, rather than create new states/animations for each one.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STATEMACHINECONTROLLER_HPP_
#define _STATEMACHINECONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <list>

#include <boost/signals2.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/shared_ptr.hpp>

#include "Debugger.hpp"
#include "DynamicsController.hpp"
#include "EntityMetadata.hpp"
#include "EntityTemplate.hpp"
#include "GridCell.hpp"
#include "PythonGil.hpp"
#include "StageControllerHolder.hpp"
#include "StateContainer.hpp"

namespace firemelon
{
	class FIREMELONAPI StateMachineController
	{
	public:
		friend class Entity; 
		friend class EntityComponents;
		friend class GameStateManager;;
		friend class HudElementController;
		friend class MapLayer;
		friend class RenderableManager;
		friend class Room;

		StateMachineController();
		virtual ~StateMachineController();
		
		void									setDynamicsController(DynamicsController* dynamicsController);
		DynamicsController*						getDynamicsController();
		
		bool									setStateByIndexPy(int stateIndex);
		bool									setStateByIndex(int stateIndex);
		
		bool									setStateByNamePy(std::string stateName);
		bool									setStateByName(std::string stateName);
		
		int										getCurrentStateIndexPy();
		int										getCurrentStateIndex();
		
		int										getOwnerIdPy();
		int										getOwnerId();

		EntityTypeId							getOwnerEntityTypeId();
		void									setOwnerEntityTypeId(EntityTypeId entityTypeId);

		void									setOwnerId(int id);

		// Methods for dynamically adding states and animations (non-entityTemplate mode).
		boost::shared_ptr<StateMachineState>	addStatePy(std::string name);
		boost::shared_ptr<StateMachineState>	addState(std::string name);


		boost::shared_ptr<StateMachineState>	getStatePy(int index);
		boost::shared_ptr<StateMachineState>	getState(int index);

		boost::shared_ptr<StateMachineState>	getCurrentStatePy();
		boost::shared_ptr<StateMachineState>	getCurrentState();

		boost::shared_ptr<StateMachineState>	getStateByNamePy(std::string name);
		boost::shared_ptr<StateMachineState>	getStateByName(std::string name);
		
		int										getStateIndexFromNamePy(std::string name);
		int										getStateIndexFromName(std::string name);
		
		std::string								getStateNameFromIndexPy(int index);
		std::string								getStateNameFromIndex(int index);
		
		int										getStateCountPy();
		int										getStateCount();
				
	private:

		int										addExistingState(StateMachineStatePtr newStateMachineState);
		
		bool									setStateByNameInternal(std::string stateName, bool forceStageChange);

		// If the owner entity has a dynamics controller attached.
		bool												isDynamic_;

		// The owner entity has been invalidated.
		bool												isInvalidated_;
		
		int													ownerId_;
		EntityTypeId										ownerEntityTypeId_;

		// The map layer index that the owner entity is on.
		int													mapLayer_;
				
		DebuggerPtr											debugger_;

		// The owner's dynamics controller.
		DynamicsController*									dynamicsController_;

		boost::shared_ptr<BaseIds>							ids_;
		
		StageControllerHolderPtr							stageControllerHolder_;

		StateContainerPtr									stateContainer_;
		
		// If the animation should be rendered.
		bool												isSpriteVisible_;		
				
		boost::signals2::signal<void(int oldState, int newState)>		stateChangedSignal;
	};

	typedef boost::shared_ptr<StateMachineController> StateMachineControllerPtr;
}

#endif // _STATEMACHINECONTROLLER_HPP_