/* -------------------------------------------------------------------------
** InputReceiverScript.hpp
**
** The InputReceiverScript is the interface by which the input functions
** of a python script are called.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _INPUTRECEIVERSCRIPT_HPP_
#define _INPUTRECEIVERSCRIPT_HPP_

#include "BaseIds.hpp"
#include "CodeBehindScript.hpp"
#include "Debugger.hpp"
#include "InputDeviceManager.hpp"
#include "Types.hpp"


namespace firemelon
{
	class InputReceiverScript : public CodeBehindScript
	{
	public:
		friend class InputReceiverCodeBehind;

		InputReceiverScript(DebuggerPtr debugger);
		virtual ~InputReceiverScript();

		void	buttonUp(GameButtonId buttonCode);
		void	buttonDown(GameButtonId buttonCode);

		void	setInputChannel(InputChannel inputChannel);

	private:

		virtual void	cleanup();
		virtual void	initialize();

		InputChannel			inputChannel_;

		PyObj					pyButtonDown_;
		PyObj					pyButtonUp_;
	};
}

#endif // _INPUTRECEIVERSCRIPT_HPP_
