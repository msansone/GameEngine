//#include "..\..\Headers\EngineCore\TcpSession.hpp"
//
//using namespace firemelon;
//using namespace boost;
//using namespace boost::asio;
//using namespace boost::asio::ip;
//
//TcpSession::TcpSession(TCPsocket socket, TcpChatRoom* chatRoom)
//{
//	chatRoom_ = chatRoom;
//	userName_ = "User";
//
//	socket_ = socket;
//
//	isStarted_ = false;
//}
//
//TcpSession::~TcpSession()
//{
//}
//
//void TcpSession::setUserName(std::string value)
//{
//	userName_ = value;
//}
//
//void TcpSession::start()
//{
//	socketSet_ = SDLNet_AllocSocketSet(1);
//
//	if (SDLNet_TCP_AddSocket(socketSet_, socket_) == -1)
//	{
//		std::cout << "Error adding TCP socket to socket set." << std::endl;
//		return;
//	}
//
//	isStarted_ = true;
//
//	threadManager_->workerThreads_.create_thread(boost::bind(&TcpSession::handleReadThread, this));
//	threadManager_->workerThreads_.create_thread(boost::bind(&TcpSession::handleWriteThread, this));
//
//	sendChatMessage(userName_ + " has joined the game.");
//
//	chatRoom_->joinChat(this);
//}
//
//void TcpSession::stop()
//{
//	isStarted_ = false;
//
//	SDLNet_TCP_Close(socket_);
//}
//
//TCPsocket& TcpSession::getSocket()
//{
//	return socket_;
//}
//
//void TcpSession::writeMessage(const TcpChatMessage& message)
//{
//	// Queue up the current message to be written.
//    messageQueue_.push_back(message);
//}
//
//void TcpSession::handleReadThread()
//{
//	while (true)
//	{
//		if (isStarted_ == false)
//		{
//			return;
//		}
//
//		SDLNet_CheckSockets(socketSet_, 30000);
//
//		if (SDLNet_SocketReady(socket_) == true)
//		{
//			if (readHeader() == false)
//			{
//				// Exit the thread;
//				return;
//			}
//		}
//	}
//}
//
//bool TcpSession::readHeader()
//{
//	int bytesReceived = SDLNet_TCP_Recv(socket_, chatMessage_.getData(), chatMessage_.HEADER_LENGTH);
//
//	if (bytesReceived > 0)
//	{
//		if (chatMessage_.decodeHeader() == true)
//		{
//			return readBody();
//		}
//		else
//		{
//			return false;
//		}
//	}
//	else
//	{
//		collisionExit();
//
//		return false;
//	}
//}
//
//
//bool TcpSession::readBody()
//{
//	int bytesReceived = SDLNet_TCP_Recv(socket_, chatMessage_.getBody(), chatMessage_.getBodyLength());
//
//	if (bytesReceived > 0)
//	{
//		// The body has been read. Deliver it to the clients in the room and read the next header.		
//
//		// Prepend the username onto the message.
//		std::string mainMessage(chatMessage_.getBody(), chatMessage_.getBodyLength());
//
//		//std::string sendData = userName_ + ": " + mainMessage;
//
//		chatMessage_.setBodyLength(strlen(mainMessage.c_str()));
//
//		memcpy(chatMessage_.getBody(), mainMessage.c_str(), chatMessage_.getBodyLength());
//
//		chatMessage_.encodeHeader();
//
//		chatRoom_->deliverMessage(chatMessage_);
//
//		return true;
//	}
//	else
//	{
//		collisionExit();
//
//		return false;
//	}
//}
//
//void TcpSession::handleWriteThread()
//{
//	while (true)
//	{
//		if (isStarted_ == false)
//		{
//			return;
//		}
//
//		if (messageQueue_.empty() == false)
//		{
//			TcpChatMessage currentMessage = messageQueue_.front();
//
//			SDLNet_TCP_Send(socket_, currentMessage.getData(), currentMessage.getLength());
//
//			messageQueue_.pop_front();
//		}
//    }  	
//}
//
//void TcpSession::collisionExit()
//{
//	chatRoom_->collisionExitChat(this);
//
//	// Build a chat message with the quit status.
//	sendChatMessage(userName_ + " has left the game.");
//
//	stop();
//}
//
//
//void TcpSession::sendChatMessage(std::string message)
//{
//	TcpChatMessage chatMessage;
//
//	chatMessage.setBodyLength(strlen(message.c_str()));
//
//	memcpy(chatMessage.getBody(), message.c_str(), chatMessage.getBodyLength());
//
//	chatMessage.encodeHeader();
//
//	chatRoom_->deliverMessage(chatMessage);
//}