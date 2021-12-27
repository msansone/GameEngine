/* -------------------------------------------------------------------------
** StateMachineCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STATEMACHINECODEBEHIND_HPP_
#define _STATEMACHINECODEBEHIND_HPP_

#include "CodeBehind.hpp"
#include "StateMachineController.hpp"
#include "StateMachineScript.hpp"
#include "Types.hpp"

namespace firemelon
{
	class StateMachineCodeBehind : public CodeBehind
	{
	public:
		friend class CodeBehindContainer;
		friend class Entity;

		StateMachineCodeBehind();
		virtual ~StateMachineCodeBehind();

	private:

		virtual void	cleanup();
		virtual void	initialize();
		void			preCleanup();
		void			preInitialize();
		virtual void	stateChanged(int oldState, int newState);
		virtual void	stateEnded(int stateIndex);
		
		boost::shared_ptr<StateMachineController>	stateMachineController_;

		boost::shared_ptr<StateMachineScript>		stateMachineScript_;
	};
}

#endif // _STATEMACHINECODEBEHIND_HPP_