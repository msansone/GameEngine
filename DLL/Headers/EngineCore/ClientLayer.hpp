///* -------------------------------------------------------------------------
//** ClientLayer.hpp
//**
//** The ClientLayer class stores the client side data of the network layer.
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _CLIENTLAYER_HPP_
//#define _CLIENTLAYER_HPP_
//
//#include <map>
//#include <iostream>
//
//#include "ReplicationStateContainer.hpp"
//#include "GameTimer.hpp"
//#include "BaseIds.hpp"
//#include "DebugHelper.hpp"
//
//namespace firemelon
//{
//	typedef boost::signals2::signal<void (RoomId, int, TransitionId, double)> RoomLoadingOnServerSignal;
//	typedef boost::signals2::signal<void (RoomId, TransitionId, double)> RoomAwaitingActivationOnServerSignal;
//	typedef boost::signals2::signal<void (RoomId, TransitionId, double)> EntityListReceivedSignal;
//
//	typedef std::map<int, int> IdMap;
//	
//	class ClientLayer
//	{
//	public:
//		
//		ClientLayer();
//		virtual ~ClientLayer();
//
//		// Map of IDs generated on the server to IDs generated locally.
//		IdMap							serverToLocalIdMap;
//
//		// If the client has finished mapping the IDs from the server to locally generated IDs.
//		bool							isClientSynchronized;
//		
//		bool							clientJoinSuccessful;	
//		
//		bool							waitingForPingResponse_;
//
//		// Accumulate the ping values over time, so that an average can be taken.
//		double							clientPingAccumulator_;
//
//		// Count the number of times ping has been accumulated since the last update.
//		int								clientPingCounter_;
//		
//		unsigned int					pingPacketId_;
//		int								pingTimerId_;
//		
//		// A timer that is incremented until the ping calculation interval has been reached.
//		double							clientPingTimer_;
//
//		// How often to update the ping, in seconds.
//		double							clientPingInterval_;
//		
//		boost::shared_ptr<GameTimer>	gameTimer_;
//
//
//		//boost::shared_ptr<ReplicationStateContainer>	replicationStatesFromServer_;
//		
//		RoomLoadingOnServerSignal				roomLoadingOnServerSignal_;
//		RoomAwaitingActivationOnServerSignal	roomAwaitingActivationOnServerSignal_;
//		EntityListReceivedSignal				entityListReceivedFromServerSignal_;
//		
//		void	initialize();
//
//		void	heartbeatResponse(int packetId);
//
//		void	mapLocalIdToServerId(int localId, int serverId);
//		
//	};
//}
//
//#endif // _CLIENTLAYER_HPP_