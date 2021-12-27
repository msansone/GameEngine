///* -------------------------------------------------------------------------
//** UdpServer.hpp
//**
//** I'll fill this in later, as it's somewhat expirimental right now.
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _UDPSERVER_HPP_
//#define _UDPSERVER_HPP_
//
//
//#include <iostream>
//#include <string>
//#include <vector>
//
//#include <SDL_net.h>
//
//#include "ThreadManager.hpp"
//#include "ServerLayer.hpp"
//#include "UdpParticipant.hpp"
//#include "UdpPacket.hpp"
//#include "BaseIds.hpp"
//#include "InputDeviceManager.hpp"
//#include "NetworkInputDevice.hpp"
//#include "DebugHelper.hpp"
//
//namespace firemelon
//{
//	typedef boost::signals2::signal<void (int)> ClientConnectedSignal;
//	typedef boost::signals2::signal<void (int)> ClientJoinedSignal;
//	typedef boost::signals2::signal<void (int)> ConnectionTimeoutSignal;
//
//	typedef std::vector<UdpParticipant*> ParticipantList;
//
//	class UdpServer
//	{
//	public:
//		friend class NetworkLayer;
//
//		UdpServer();
//		virtual ~UdpServer();
//
//		void			start(short serverPort, short clientPort);
//		void			stop();
//		
//		bool			getIsStarted();
//		
//		int				getParticipantCount();
//		UdpParticipant*	getParticipant(int index);
//		void			sendToClient(int index, UdpPacket packet);
//		void			connectionTimeout(int index);
//
//		void			setInputDeviceManager(InputDeviceManager* inputDeviceManager);
//
//	private:
//		
//
//		void	handleReceiveThread();
//
//		bool	isParticipantConnected(std::string ipAddress);
//		int		registerParticipant(std::string ipAddress);
//		int		getParticipantIndex(std::string ipAddress);
//		void	sendIdListToClient(int connectionIndex);
//
//		void	processPacket();
//		
//		ServerLayer*			serverLayer_;
//
//		bool					isStarted_;
//
//		short					clientPort_;
//
//		ThreadManager*			threadManager_;
//
//		ClientConnectedSignal	clientConnectedSignal_;
//		ClientJoinedSignal		clientJoinedSignal_;
//		ConnectionTimeoutSignal	connectionTimeoutSignal_;
//
//		UDPsocket				socket_;
//		ParticipantList			participants_;
//		UdpPacket				udpPacket_;
//
//		InputDeviceManager*		inputDeviceManager_;
//		NetworkUtility			networkUtility_;
//	};
//}
//
//#endif // _UDPSERVER_HPP_