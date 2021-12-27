///* -------------------------------------------------------------------------
//** TcpConnection.hpp
//**
//** The TcpConnection class represents a connection on a client to a TcpServer.
//** It stores the socket which is used to send and receive data.
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _TCPCONNECTION_HPP_
//#define _TCPCONNECTION_HPP_
//
//#include <iostream>
//#include <deque>
//
//#include <SDL_net.h>
//#include <boost/signals2.hpp>
//
//#include "TcpChatMessage.hpp"
//#include "ThreadManager.hpp"
//
//namespace firemelon
//{
//	class TcpConnection
//	{
//	public:
//		friend class NetworkLayer;
//		
//		TcpConnection(std::string ip, short port);
//		virtual ~TcpConnection();
//
//		void writeChatMessage(std::string message);
//
//		void openConnection();
//		void closeConnection();
//		void connectionLost();
//
//	private:
//		
//		void write(std::string message);
//
//		void handleWriteThread();
//		void handleReadThread();
//		
//		void readHeader();
//		void readBody();
//
//		std::string									ip_;
//
//		short										port_;
//
//		TCPsocket									socket_;
//		SDLNet_SocketSet							socketSet_;
//
//		TcpChatMessage								readMessage_;
//
//		ThreadManager*								threadManager_;
//
//		std::deque<TcpChatMessage>					pendingWriteQueue_;
//
//		boost::signals2::signal<void(std::string)>	chatLineReceivedSignal_;
//		boost::signals2::signal<void()>				connectionLostSignal_;
//		
//		bool										isConnected_;
//	};
//}
//
//#endif // _TCPCONNECTION_HPP_