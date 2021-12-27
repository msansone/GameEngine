/* -------------------------------------------------------------------------
** StateContainer.hpp
**
** The StateContainer class stores the state objects.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STATECONTAINER_HPP_
#define _STATECONTAINER_HPP_

#include "StateMachineState.hpp"

namespace firemelon
{
	class StateContainer
	{
	public:
		friend class StageController;
		friend class StageRenderable;
		friend class StateMachineController;

		StateContainer();
		virtual ~StateContainer();

		//void	addState(boost::shared_ptr<StateMachineState> state);
		
		int						getCurrentStateIndex();

		StateMachineStatePtr	getStateByIndex(int index);

		int						getStateCount();

	private:

		void	addState(StateMachineStatePtr state);

		int							currentStateIndex_;

		int							previousStateIndex_;

		std::map<int, std::string>	stateIdNameMap_;

		std::map<std::string, int>	stateNameIdMap_;
		
		StateMachineStatePtrList	states_;
	};

	typedef boost::shared_ptr<StateContainer> StateContainerPtr;
}

#endif // _STATECONTAINER_HPP_