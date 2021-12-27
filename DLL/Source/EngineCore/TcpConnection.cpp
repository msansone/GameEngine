//#include "..\..\Headers\EngineCore\TcpConnection.hpp"
//
//using namespace firemelon;
//using namespace boost;
//using namespace boost::asio;
//using namespace boost::asio::ip;
//
//TcpConnection::TcpConnection(std::string ip, short port)
//{
//	isConnected_ = false;
//	ip_ = ip;
//	port_ = port;
//}
//
//TcpConnection::~TcpConnection()
//{
//
//}
//
//void TcpConnection::writeChatMessage(std::string message)
//{
//	write(message);
//}
//
//void TcpConnection::openConnection()
//{
//	socketSet_ = SDLNet_AllocSocketSet(1);
//
//	if (SDLNet_TCP_AddSocket(socketSet_, socket_) == -1)
//	{
//		std::cout << "Error adding TCP socket to socket set." << std::endl;
//		return;
//	}
//
//	IPaddress ipAddress;
//	SDLNet_ResolveHost(&ipAddress, ip_.c_str(), port_);
//
//	std::cout << "Connecting to " << ip_ << std::endl;
//
//	socket_ = SDLNet_TCP_Open(&ipAddress);
//
//	if (socket_ != nullptr)
//	{
//		isConnected_ = true;
//
//		std::string status = "Connection to host established.";
//
//		chatLineReceivedSignal_(status);
//
//		threadManager_->workerThreads_.create_thread(boost::bind(&TcpConnection::handleWriteThread, this));
//		threadManager_->workerThreads_.create_thread(boost::bind(&TcpConnection::handleReadThread, this));
//	}
//}
//
//void TcpConnection::connectionLost()
//{
//	closeConnection();
//
//	std::string status = "Connection to host lost.";
//
//	chatLineReceivedSignal_(status);
//
//	connectionLostSignal_();
//}
//
//void TcpConnection::closeConnection()
//{
//	if (isConnected_ == true)
//	{
//		isConnected_ = false;
//
//		SDLNet_TCP_Close(socket_);
//	}
//}
//
//void TcpConnection::readHeader()
//{
//	if (isConnected_ == true)
//	{
//		int byteCount = SDLNet_TCP_Recv(socket_, readMessage_.getData(), readMessage_.HEADER_LENGTH);
//
//		if (byteCount > 0)
//		{
//			if (readMessage_.decodeHeader() == true)
//			{
//				readBody();
//			}
//		}
//		else
//		{
//			connectionLost();
//		}
//	}
//}
//
//void TcpConnection::readBody()
//{
//	if (isConnected_ == true)
//	{
//		int byteCount = SDLNet_TCP_Recv(socket_, readMessage_.getBody(), readMessage_.getBodyLength());
//
//		if (byteCount > 0)
//		{			
//			std::string chatLine(readMessage_.getBody(), readMessage_.getBodyLength());
//
//			chatLineReceivedSignal_(chatLine);
//		}
//		else
//		{
//			connectionLost();
//		}
//	}
//}
//
//void TcpConnection::write(std::string message)
//{
//	if (isConnected_ == false)
//	{
//		std::string err = "Unable to send message. Not connected to host.";
//
//		chatLineReceivedSignal_(err);
//
//		return;
//	}
//
//	bool isWriting = !pendingWriteQueue_.empty();
//
//	TcpChatMessage chatMessage;
//
//	//if (socket_.is_open() == true)
//	//{
//		//tcp::endpoint localEndpoint = socket_.local_endpoint();
//		//address localAddress = localEndpoint.address();
//
//		//std::string ipAddress = localAddress.to_string();
//
//		std::string sendData = message;
//	
//		chatMessage.setBodyLength(strlen(sendData.c_str()));
//
//		memcpy(chatMessage.getBody(), sendData.c_str(), chatMessage.getBodyLength());
//
//		chatMessage.encodeHeader();
//
//		pendingWriteQueue_.push_back(chatMessage);
//
//		if (isWriting == false)
//		{
//
//		}
//	//}
//}
//
//void TcpConnection::handleWriteThread()
//{
//	while (true)
//	{
//		if (isConnected_ == false)
//		{
//			// End the thread if not connected.
//			return;
//		}
//
//		if (!pendingWriteQueue_.empty())
//		{
//			TcpChatMessage currentMessage = pendingWriteQueue_.front();
//
//			pendingWriteQueue_.pop_front();
//
//			SDLNet_TCP_Send(socket_, currentMessage.getData(), currentMessage.getLength());
//		}
//	}
//}
//
//void TcpConnection::handleReadThread()
//{
//	while (true)
//	{
//		if (isConnected_ == false)
//		{
//			// End the thread if not connected.
//			return;
//		}
//
//		readHeader();	
//	}
//}