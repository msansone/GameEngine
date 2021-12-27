/* -------------------------------------------------------------------------
** TcpChatMessage.hpp
**
** I'll fill this in later, as it's somewhat expirimental right now.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TCPCHATMESSAGE_HPP_
#define _TCPCHATMESSAGE_HPP_

#include <iostream>

#include <cstdio>
#include <cstdlib>
#include <cstring>

#include <boost/asio.hpp>
#include <boost/bind.hpp>

namespace firemelon
{
	class TcpChatMessage
	{
	public:
		enum { HEADER_LENGTH = 4 };
		enum { MAX_BODY_LENGTH = 512 };
		
		TcpChatMessage();
		virtual ~TcpChatMessage();
		
		const char* getData() const;
  
		char*		getData();

		size_t		getLength() const;

		const char* getBody() const;

		char*		getBody();

		size_t		getBodyLength() const;

		void		setBodyLength(size_t newLength);
  
		bool		decodeHeader();

		void		encodeHeader();

	private:
		
		char	data_[HEADER_LENGTH + MAX_BODY_LENGTH];
		size_t	bodyLength_;
	};
}

#endif // _TCPCHATMESSAGE_HPP_