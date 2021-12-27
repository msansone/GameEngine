/* -------------------------------------------------------------------------
** Messenger.hpp
** 
** The Messenger class acts as a layer between the Entity class and the
** GameStateManager class, which is used to send messages from one entity to
** one or more other entities.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _MESSENGER_HPP_
#define _MESSENGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/signals2.hpp>

#include "Message.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	typedef boost::signals2::signal<void (firemelon::Message message)> messagesignal;

	class FIREMELONAPI Messenger 
	{
	public:
		friend class GameStateManager;

		Messenger();
		virtual ~Messenger();
		
		void sendMessagePy(Message message);
		void sendMessage(Message message);

	private:
		
		messagesignal postMessageSignal;	
	};
}
#endif // _MESSENGER_HPP_