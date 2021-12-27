/* -------------------------------------------------------------------------
** TcpChatParticipant.hpp
**
** I'll fill this in later, as it's somewhat expirimental right now.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TCPCHATPARTICIPANT_HPP_
#define _TCPCHATPARTICIPANT_HPP_

#include <iostream>

#include <boost/asio.hpp>
#include <boost/bind.hpp>

#include "TcpChatMessage.hpp"

namespace firemelon
{
	class TcpChatParticipant
	{
	public:
		
		TcpChatParticipant();
		virtual ~TcpChatParticipant();
		
		virtual void writeMessage(const TcpChatMessage& message) = 0;

		int			getId();

	private:
		
		static int	participantIdGenerator_;
		int			participantId_;
	};
}

#endif // _TCPCHATPARTICIPANT_HPP_