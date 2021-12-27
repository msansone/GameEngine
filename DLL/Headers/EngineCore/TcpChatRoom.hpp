/* -------------------------------------------------------------------------
** TcpChatRoom.hpp
**
** I'll fill this in later, as it's somewhat expirimental right now.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TCPCHATROOM_HPP_
#define _TCPCHATROOM_HPP_

#include <iostream>
#include <vector>

#include <boost/asio.hpp>
#include <boost/bind.hpp>

#include "TcpChatParticipant.hpp"
#include "TcpChatMessage.hpp"

namespace firemelon
{
	class TcpChatRoom
	{
	public:
		
		TcpChatRoom();
		virtual ~TcpChatRoom();
		
		void	joinChat(TcpChatParticipant* chatParticipant);
		void	leaveChat(TcpChatParticipant* chatParticipant);
		void	deliverMessage(const TcpChatMessage& message);
		void	shutdown();

	private:
		
		std::vector<TcpChatParticipant*> chatParticipants_;
	};
}

#endif // _TCPCHATROOM_HPP_