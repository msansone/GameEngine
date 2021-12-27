///* -------------------------------------------------------------------------
//** TcpServer.hpp
//**
//** The TcpServer class is used to start the server, and accept/manage connections
//** from clients.
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _TCPSERVER_HPP_
//#define _TCPSERVER_HPP_
//
//#include <iostream>
//
//#include <boost/asio.hpp>
//#include <boost/bind.hpp>
//
//#include "TcpSession.hpp"
//
//#include "ThreadManager.hpp"
//
//namespace firemelon
//{
//	typedef	boost::thread_group ThreadGroup;
//
//	class TcpServer
//	{
//	public:
//		friend class NetworkLayer;
//
//		TcpServer(short port);
//		virtual ~TcpServer();
//
//		void initialize();
//		void start();
//		void shutdown();
//
//		void startAccept();
//		void handleAcceptThread();
//
//	private:
//		
//		bool			isStarted_;
//
//		TCPsocket		socket_;
//		short			port_;
//		TcpChatRoom*	chatRoom_;
//
//		ThreadManager*	threadManager_;
//	};
//}
//
//#endif // _TCPSERVER_HPP_