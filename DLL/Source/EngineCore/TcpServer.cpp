//#include "..\..\Headers\EngineCore\TcpServer.hpp"
//
//using namespace firemelon;
//using namespace boost;
//using namespace boost::asio;
//using namespace boost::asio::ip;
//
//TcpServer::TcpServer(short port)
//{
//	port_ = port;
//	chatRoom_ = new TcpChatRoom();
//
//	isStarted_ = false;
//}
//
//TcpServer::~TcpServer()
//{
//	delete chatRoom_;
//}
//
//void TcpServer::initialize()
//{
//	threadManager_->workerThreads_.create_thread(boost::bind(&TcpServer::handleAcceptThread, this));
//}
//
//void TcpServer::start()
//{
//	startAccept();
//}
//
//void TcpServer::startAccept()
//{
//	IPaddress ipAddress;
//	int success = SDLNet_ResolveHost(&ipAddress, nullptr, port_);
//
//	if (success == -1)
//	{
//		std::cout << "Failed to open port " << port_ << std::endl;
//		return;
//	}
//
//	socket_ = SDLNet_TCP_Open(&ipAddress);
//
//	if (socket_ == nullptr)
//	{
//		std::cout << "Failed to open port "<< port_ << " for listening" << std::endl;
//	}
//
//	isStarted_ = true;
//}
//
//void TcpServer::handleAcceptThread()
//{
//	// Listen for a new chat participant (session).
//	// Continuously loop while trying to accept connections.
//	while (true)
//	{
//		if (isStarted_ == true)
//		{
//			TCPsocket socket = SDLNet_TCP_Accept(socket_);
//
//			if (socket != nullptr)
//			{
//				std::cout << "TCP Connection Accepted." << std::endl;
//
//				// New connection accepted.
//				TcpSession* newSession = new TcpSession(socket, chatRoom_);
//
//				newSession->threadManager_ = threadManager_;
//
//				newSession->start();
//
//				// Get the entering user's IP.
//				IPaddress *remoteIpAddress;
//
//				remoteIpAddress = SDLNet_TCP_GetPeerAddress(socket);
//
//				int ip = SDLNet_Read32(&(remoteIpAddress->host));
//
//				std::cout << " Accepting TCP connection from "
//					<< ((ip & 0xFF000000) >> 24) << "."
//					<< ((ip & 0x00FF0000) >> 16) << "."
//					<< ((ip & 0x0000FF00) >> 8) << "."
//					<< (ip & 0x000000FF) << std::endl;
//			}
//		}
//		else
//		{
//			return;
//		}
//	}
//}
//
//void TcpServer::shutdown()
//{
//	isStarted_ = false;
//
//	chatRoom_->shutdown();
//
//	SDLNet_TCP_Close(socket_);
//}