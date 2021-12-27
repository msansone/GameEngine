/* -------------------------------------------------------------------------
** ChatRoom.hpp
**
** The ChatRoom class stores the strings sent by clients or status messages
** from the server, and alerts connected listeners when a new string has been
** added.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CHATROOM_HPP_
#define _CHATROOM_HPP_

#include <string>
#include <vector>
#include <iostream>

#include <boost/signals2.hpp>
#include <boost/python.hpp>

#include "DebugHelper.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class ChatRoom
	{
	public:
		ChatRoom();
		virtual ~ChatRoom();

		void	addChatListener(boost::python::object listener);
		void	addChatLine(std::string chatLine); 
		void	cleanup();
		void	dispatchChatLines();

	private:
		
		std::vector<std::string>			newChatLines_;
		std::vector<std::string>			dispatchedChatLines_;
		std::vector<boost::python::object>	pyChatListeners_;
	};
}

#endif // _CHATROOM_HPP_