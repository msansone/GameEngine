//#include "..\..\Headers\EngineCore\ClientLayer.hpp"
//
//using namespace firemelon;
//
//ClientLayer::ClientLayer()
//{
//	isClientSynchronized = false;
//	clientJoinSuccessful = false;
//	
//	// Update ping every second.
//	clientPingInterval_ = 1.0;
//	clientPingAccumulator_ = 0.0;
//
//	clientPingCounter_ = 0;
//
//	pingTimerId_ = -1;
//	pingPacketId_ = 0;
//	
//	clientPingTimer_ = 0.0;
//
//	waitingForPingResponse_ = false;
//}
//
//ClientLayer::~ClientLayer()
//{
//}
//
//void ClientLayer::initialize()
//{
//	pingTimerId_ = gameTimer_->addTimer();
//}
//
//void ClientLayer::mapLocalIdToServerId(int localId, int serverId)
//{
//	serverToLocalIdMap[localId] = serverId;
//	
//	int mapSize = serverToLocalIdMap.size();
//	int idsSize = BaseIds::getUuidIntegerMap().size();
//
//	if (mapSize == idsSize)
//	{
//		// All local IDs have been mapped to IDs from the server.
//		// The client is now fully initialized and ready to send real data.
//		isClientSynchronized = true;
//
//		DebugHelper::streamLock->lock();
//		std::cout << "[" << boost::this_thread::get_id() << "] Synch complete on client. "<< std::endl;
//		DebugHelper::streamLock->unlock();
//
//	}
//}
//
//void ClientLayer::heartbeatResponse(int packetId)
//{
//	int giveupAfterPackets = 50;
//
//	if (pingPacketId_ == packetId || (packetId > pingPacketId_ + giveupAfterPackets))
//	{
//		double loggedTime = gameTimer_->logTimeEnd(pingTimerId_);
//
//		// Halve the time to get the time to get to the server. Multiply by 1000, to get milliseconds.
//		double pingTime = ((loggedTime / 2) * 1000);
//		
//		clientPingAccumulator_ += pingTime;
//		clientPingCounter_++;
//
//		waitingForPingResponse_ = false;
//	}
//}
