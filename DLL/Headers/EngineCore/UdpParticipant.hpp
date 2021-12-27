///* -------------------------------------------------------------------------
//** UdpParticipant.hpp
//**
//** The UdpParticipant class represnts a client that has connected to the 
//** server and is a participant in the game. It stores the client's remote 
//** endpoint object, along with other information needed to track the client,
//** such as the entity ID that is associated with it. The UdpServer class
//** creates a new UdpParticipant object every time it receives a connection 
//** packet from a client that is not currently a participant.
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _UDPPARTICIPANT_HPP_
//#define _UDPPARTICIPANT_HPP_
//
//#include <iostream>
//#include <boost/signals2.hpp>
//
//#include "BaseIds.hpp"
//#include "CameraController.hpp"
//
//namespace firemelon
//{
//	class UdpParticipant
//	{
//	public:
//		friend class UdpServer;
//		
//		UdpParticipant(short clientPort);
//		virtual ~UdpParticipant();
//		
//		void				initialize(std::string ip);
//
//		std::string			getIpAddress();
//		
//		bool				getIsSynchronized();
//		
//		void				setHasJoined(bool value);
//		bool				getHasJoined();
//
//		RoomId				getCurrentRoomId();		
//		void				setCurrentRoomId(RoomId currentRoomId);
//		
//		TransitionId		getCurrentTransitionId();		
//		void				setCurrentTransitionId(TransitionId* currentTransitionId);
//		
//		double				getCurrentTransitionTime();		
//		void				setAssociatedEntityInstanceDelayTimer(double* delayTimer);
//		void				setAssociatedEntityInstanceDelayTime(double* delayTime);
//
//		InputChannel		getInputChannel();
//		void				setInputChannel(InputChannel inputChannel);
//		
//		SpawnPointId		getInitialSpawnPointId();
//		void				getInitialSpawnPointId(SpawnPointId spawnPointId);
//
//		EntityTypeId		getEntityTypeId();
//		void				setEntityTypeId(EntityTypeId entityTypeId);
//
//		void				incrementIdleTimer(double time);
//		double				getIdleTime();
//		
//		int					getAssociatedEntityInstanceId();
//		void				setAssociatedEntityInstanceId(int associatedEntityInstanceId);
//		
//		boost::shared_ptr<CameraController>	getCamera();
//		void								setCamera(boost::shared_ptr<CameraController> camera);
//
//		BufferWrapperPtr	getEntityDataBuffer();
//		
//	private:
//		
//		std::string			ipAddress_;
//
//		UDPpacket*			sdlPacket_;
//
//		BufferWrapperPtr	bufferWrapper_;
//
//		BaseIds				ids_;
//
//		boost::shared_ptr<CameraController>	camera_;
//		
//		EntityTypeId		entityTypeId_;
//		SpawnPointId		spawnPointId_;
//		RoomId				currentRoomId_;
//		TransitionId*		currentTransitionId_;
//		double*				associatedEntityDelayTimer_;
//		double*				associatedEntityDelayTime_;
//
//		InputChannel		inputChannel_;
//
//		int					associatedEntityInstanceId_;
//
//		bool				hasSpawnedEntity_;
//
//		bool				isSynchronized_;
//		
//		bool				hasJoined_;
//
//		bool				isInitialized_;
//
//		short				clientPort_;
//
//		double				idleTimer_;
//	};
//}
//
//#endif // _UDPPARTICIPANT_HPP_