/* -------------------------------------------------------------------------
** UdpConnection.hpp
**
** I'll fill this in later, as it's somewhat expirimental right now.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UDPCONNECTION_HPP_
#define _UDPCONNECTION_HPP_

#include <iostream>
#include <boost/shared_ptr.hpp>
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <boost/array.hpp>
#include <boost/signals2.hpp>

#include "BaseIds.hpp"

namespace firemelon
{
	typedef boost::shared_ptr<boost::asio::io_service> IoService;

	class UdpConnection
	{
	public:
		friend class UdpClient;
		
		UdpConnection(IoService ioService, std::string ip, std::string port);
		virtual ~UdpConnection();

	private:
		
		boost::shared_ptr<boost::asio::ip::udp::socket>	socket_;
		boost::shared_ptr<boost::asio::ip::udp::resolver> resolver_;
		boost::shared_ptr<boost::asio::ip::udp::resolver::query> query_;
		boost::shared_ptr<boost::asio::ip::udp::resolver::iterator> iterator_;		
	};
}

#endif // _UDPCONNECTION_HPP_