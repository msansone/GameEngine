///* -------------------------------------------------------------------------
//** TcpSession.hpp
//**
//** The TcpSession class stores a participating client's connection data in the
//** TcpServer. It is used to send and receive data from and to the client.
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _TCPSESSION_HPP_
//#define _TCPSESSION_HPP_
//
//#include <iostream>
//#include <deque>
//
//#include <SDL_net.h>
//
//#include "TcpChatRoom.hpp"
//#include "TcpChatParticipant.hpp"
//#include "ThreadManager.hpp"
//
//namespace firemelon
//{
//	class TcpSession : TcpChatParticipant
//	{
//	public:
//		friend class TcpServer;
//		
//		TcpSession(TCPsocket socket, TcpChatRoom* chatRoom);
//		virtual ~TcpSession();
//
//		void			start();
//		void			stop();
//		void			collisionExit();
//
//		virtual void	writeMessage(const TcpChatMessage& message);
//
//		TCPsocket&		getSocket();
//		
//		void			setUserName(std::string value);
//
//	private:
//
//		void	handleReadThread();
//		bool	readHeader();
//		bool	readBody();
//		void	handleWriteThread();
//
//		void	sendChatMessage(std::string message);
//
//		bool						isStarted_;
//		TCPsocket					socket_;
//		SDLNet_SocketSet			socketSet_;
//
//		TcpChatRoom*				chatRoom_;
//		TcpChatMessage				chatMessage_;
//		ThreadManager*				threadManager_;
//
//		std::deque<TcpChatMessage>	messageQueue_;
//
//		std::string					userName_;
//	};
//}
//
//#endif // _TCPSESSION_HPP_