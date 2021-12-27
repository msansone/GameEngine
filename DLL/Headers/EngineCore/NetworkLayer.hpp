///* -------------------------------------------------------------------------
//** NetworkLayer.hpp
//**
//** 
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _NETWORKLAYER_HPP_
//#define _NETWORKLAYER_HPP_
//
//#include <string>
//
//#include <boost/shared_ptr.hpp>
//
//#include "UdpServer.hpp"
//#include "UdpClient.hpp"
//#include "TcpServer.hpp"
//#include "TcpConnection.hpp"
//
//#include "InputDeviceManager.hpp"
//#include "BaseIds.hpp"
//#include "CameraController.hpp"
//#include "NetworkUtility.hpp"
//#include "ThreadManager.hpp"
//
//namespace firemelon
//{
//	typedef	boost::shared_ptr<UdpServer> UdpServerPtr;
//	typedef boost::shared_ptr<UdpClient> UdpClientPtr;
//	typedef boost::shared_ptr<boost::asio::ip::tcp::resolver::iterator> TcpIterator;
//	typedef boost::shared_ptr<boost::asio::ip::tcp::resolver::query> TcpQuery;
//	typedef boost::shared_ptr<boost::asio::ip::tcp::resolver> TcpResolver;
//	typedef boost::shared_ptr<boost::asio::io_service::work> AsioWork;
//	
//	typedef boost::signals2::signal<void (std::vector<int>, std::vector<int>)> ServerIdMappingSignal;
//	
//	typedef boost::signals2::signal<void ()> ConnectionToHostLostSignal;
//
//	class NetworkLayer
//	{
//	public:
//		friend class GameEngine;
//
//		NetworkLayer();
//		virtual ~NetworkLayer();
//
//		void				initialize();
//		void				beginFrame();
//
//		void				startServer();
//		bool				isServerStarted();
//
//		void				connectToHost(std::string ip);
//		void				client_start();
//		void				client_initiateConnection();
//		void				client_sendInput(InputDevice* inputDevice);
//		void				client_sendHeartbeat();
//		void				client_sendJoin(boost::shared_ptr<JoinData> joinData);
//		bool				isConnectedToHost();
//
//		void				server_sendRoomLoadedPercentToClient(int participantIndex, RoomId roomId, int percentLoaded, TransitionId transitionId, double transitionTime);
//		void				server_sendRoomAwaitingActivationToClient(int participantIndex, RoomId roomId, TransitionId transitionId, double transitionTime);
//		bool				server_sendEntityListToClient(int participantIndex, RoomId roomId, TransitionId transitionId, double transitionTime);
//
//		void				serverTick(double time, bool updateOccurred);
//		void				clientTick(double time, bool updateOccurred);
//
//		void				server_setInputDeviceManager(InputDeviceManager* inputDeviceManager);
//
//		void				shutdown();
//		
//		bool				getSendDataToServer();
//		bool				getSendDataToClient();
//
//		// Participant functions
//		int					getParticipantCount();
//		
//		RoomId				getParticipantRoomId(int participantIndex);
//		EntityTypeId		getParticipantEntityTypeId(int participantIndex);
//		SpawnPointId		getParticipantSpawnPointId(int participantIndex);
//		TransitionId		getParticipantTransitionId(int participantIndex);
//		double				getParticipantTransitionTime(int participantIndex);
//		BufferWrapperPtr	getParticipantEntityDataBuffer(int participantIndex);
//		bool				getIsParticipantSynchronized(int participantIndex);
//		bool				getParticipantHasJoined(int participantIndex);
//		CameraController*	getParticipantCamera(int participantIndex);
//		
//		int					getParticipantAssociatedEntityInstanceId(int participantIndex);
//		void				setAssociatedEntityInstanceId(int participantIndex, int entityInstanceId);		
//		void				setAssociatedEntityInstanceDelayTimer(int participantIndex, double* delayTimer);
//		void				setAssociatedEntityInstanceDelayTime(int participantIndex, double* delayTime);		
//		void				setParticipantTransitionId(int participantIndex, TransitionId* transitionId);
//
//		int					getParticipantInputDeviceId(int participantIndex);
//		void				setParticipantInputDeviceId(int participantIndex, int inputDeviceId);
//
//		std::string			getParticipantIpAddress(int participantIndex);
//
//		void				sendChatString(std::string message);
//
//		ConnectionTimeoutSignal					connectionTimeoutSignal_;
//		ConnectionToHostLostSignal				connectionToHostLostSignal_;
//		ClientJoinedSignal						server_clientJoinedSignal_;
//
//	private:
//		
//		void	client_chatLineReceived(std::string chatLine);	
//		void	client_lostConnectionToHost();	
//
//		void	server_clientConnected(int participantIndex);
//		void	server_clientJoined(int participantIndex);
//		
//		void	linkCameraToParticipant(int participantIndex, CameraController* camera);
//		void	linkRoomToParticipant(int participantIndex, RoomId room);
//
//		// Used to send packets from the server to the clients at a fixed rate.
//		double						serverPacketSendAccumulator_;
//		double						serverPacketSendRate_;
//
//		// Used to send packets from the client to the server at a fixed rate.
//		double						clientPacketSendAccumulator_;
//		double						clientPacketSendRate_;
//		
//		// The timeout value that must be reached before the client decides the server
//		// is no longer available.
//		double						serverConnectionTimeout_;
//
//		// The timeout value that must be reached before the server decides a client
//		// is no longer available.
//		double						clientConnectionTimeout_;
//			
//		bool						sendDataToServer_;
//		bool						sendDataToClient_;
//		bool						connectionOpenCalled_;
//
//		BaseIds*					ids_;
//		NetworkHandler*				networkHandler_;
//		ChatRoom*					chatRoom_;
//		ReplicationStateContainer*	replicationStatesOnServer_;
//		
//		ClientLayer*				clientLayer_;
//		ServerLayer*				serverLayer_;
//
//		NetworkUtility				networkUtility_;
//		
//		TcpServer*					tcpServer_;
//		TcpConnection*				tcpConnection_;
//
//		UdpServerPtr				udpServer_;
//		UdpClientPtr				udpClient_;
//		
//		ThreadManager*				threadManager_;
//
//		short						tcpPort_;
//		short						udpServerPort_;
//		short						udpClientPort_;		
//	};
//}
//
//#endif // _NETWORKLAYER_HPP_