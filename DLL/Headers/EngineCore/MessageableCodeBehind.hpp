/* -------------------------------------------------------------------------
** MessageableCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _MESSAGEABLECODEBEHIND_HPP_
#define _MESSAGEABLECODEBEHIND_HPP_

#include "CodeBehind.hpp"
#include "Message.hpp"
#include "MessageableScript.hpp"
#include "PythonInstanceWrapper.hpp"
#include "Types.hpp"

namespace firemelon
{
	class MessageableCodeBehind : public CodeBehind
	{
	public:
		friend class CodeBehindContainer;
		friend class GameStateManager;
		friend class Room;

		MessageableCodeBehind();
		virtual ~MessageableCodeBehind();

	private:

		virtual void	cleanup();

		virtual void	messageReceived(Message message);

		void			preCleanup();
		void			preInitialize();

		boost::shared_ptr<MessageableScript>	messageableScript_;
	};
}

#endif // _MESSAGEABLECODEBEHIND_HPP_