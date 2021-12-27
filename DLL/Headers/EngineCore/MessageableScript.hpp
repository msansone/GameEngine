/* -------------------------------------------------------------------------
** MessageableScript.hpp
**
** The MEssageableScript is the interface by which the messenger functions of
** a python script are called.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _MESSAGEABLESCRIPT_HPP_
#define _MESSAGEABLESCRIPT_HPP_

#include "CodeBehindScript.hpp"
#include "Debugger.hpp"
#include "Message.hpp"
#include "Types.hpp"

namespace firemelon
{
	class MessageableScript : public CodeBehindScript
	{
	public:
		friend class MessageableCodeBehind;

		MessageableScript(DebuggerPtr debugger);
		virtual ~MessageableScript();

		void messageReceived(Message message);

	protected:

	private:

		virtual void	cleanup();
		virtual void	initialize();

		PyObj	pyMessageReceived_;
	};
}

#endif // _MESSAGEABLESCRIPT_HPP_
