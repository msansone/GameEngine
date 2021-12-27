///* -------------------------------------------------------------------------
//** UdpClient.hpp
//**
//** The UdpClient class
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _UDPCLIENT_HPP_
//#define _UDPCLIENT_HPP_
//
//
//#include <queue>
//#include <iostream>
//
//#include <SDL_net.h>
//#include <boost/signals2.hpp>
//
//#include "ThreadManager.hpp"
//#include "ClientLayer.hpp"
//#include "BaseIds.hpp"
//#include "UdpPacket.hpp"
//#include "UdpConnection.hpp"
//#include "EntityReplicationState.hpp"
//#include "Types.hpp"
//
//namespace firemelon
//{
//	typedef boost::signals2::signal<void (int)> HeartbeatResponseSignal;
//	
//	class UdpClient
//	{
//	public:
//		friend class NetworkLayer;
//		
//		UdpClient();
//		virtual ~UdpClient();
//		
//		bool	getIsConnected();		
//		void	connect(std::string ip, short clientPort, short serverPort);
//		void	closeConnection();
//
//		void	sendData(UdpPacket udpPacket);
//
//		void	handleReceiveThread();
//		
//		void	incrementIdleTimer(double time);
//		double	getIdleTime();
//
//	private:
//
//		void	processPacket();
//
//		ThreadManager*	threadManager_;
//
//		ClientLayer*	clientLayer_;
//
//		bool			isConnected_;
//		
//		double			idleTimer_;
//
//		UDPsocket		socket_;
//		UdpPacket		udpPacket_;
//
//		NetworkUtility	networkUtility_;
//	};
//}
//
//#endif // _UDPCLIENT_HPP_