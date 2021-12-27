/* -------------------------------------------------------------------------
** StateMachineScript.hpp
**
** The StateMachineScript is the interface by which the state changing functions
** of a python script are called.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STATEMACHINESCRIPT_HPP_
#define _STATEMACHINESCRIPT_HPP_

#include "CodeBehindScript.hpp"
#include "Debugger.hpp"
#include "StateMachineController.hpp"
#include "Types.hpp"

namespace firemelon
{
	class StateMachineScript : public CodeBehindScript
	{
	public:
		friend class StateMachineCodeBehind;

		StateMachineScript(DebuggerPtr debugger);
		virtual ~StateMachineScript();

		void	stateChanged(int oldStateIndex, int newStateIndex);

		void	stateEnded(int stateIndex);
		
	protected:

	private:

		virtual void	cleanup();
		virtual void	initialize();

		PyObj				pyStateChanged_;

		PyObj				pyStateEnded_;
		
		boost::shared_ptr<StateMachineController>	stateMachineController_;
	};
}

#endif // _STATEMACHINESCRIPT_HPP_
