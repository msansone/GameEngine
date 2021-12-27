#include "..\..\Headers\EngineCore\UdpConnection.hpp"

using namespace firemelon;
using namespace boost;
using namespace boost::asio;
using namespace boost::asio::ip;

UdpConnection::UdpConnection(shared_ptr<io_service> ioService, std::string ip, std::string port) : 
	socket_(new udp::socket(*ioService, ip::udp::endpoint(udp::v4(), 0))),
	resolver_(new udp::resolver(*ioService)),
	query_(new udp::resolver::query(udp::v4(), ip, port)),
	iterator_( new udp::resolver::iterator(resolver_->resolve(*query_)))
{

}

UdpConnection::~UdpConnection()
{

}