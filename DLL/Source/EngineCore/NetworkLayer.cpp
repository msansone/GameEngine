//#include "..\..\Headers\EngineCore\NetworkLayer.hpp"
//
//using namespace firemelon;
//using namespace boost::asio::ip;
//using namespace boost::asio;
//using namespace boost;
//
////NetworkLayer::NetworkLayer() : 
////	serverIoService_(new boost::asio::io_service),
////	clientIoService_(new boost::asio::io_service),
////	serverWork_(new io_service::work(*serverIoService_)),
////	clientWork_(new io_service::work(*clientIoService_)),
////	networkUtility_()
//
//NetworkLayer::NetworkLayer() : 
//	udpServer_(new UdpServer),
//	udpClient_(new UdpClient),
//	networkUtility_()
//{
//	// Send 30 packets from the server to the clients per second.
//	serverPacketSendAccumulator_ = 0.0;
//	serverPacketSendRate_ = 1 / 30.0;
//	
//	// Send 10 packets from the client to the server per second.
//	clientPacketSendAccumulator_ = 0.0;
//	clientPacketSendRate_ = 1 / 10.0;
//	
//	serverConnectionTimeout_ = 30.0;
//	clientConnectionTimeout_ = 30.0;
//	
//	udpServerPort_ = 10000;
//	udpClientPort_ = 10001;
//	tcpPort_ = 10002;
//	
//	tcpServer_ = new TcpServer(tcpPort_);
//	tcpConnection_ = nullptr;
//	
//	sendDataToServer_ = true;
//	sendDataToClient_ = false;
//	connectionOpenCalled_ = false;
//}
//
//NetworkLayer::~NetworkLayer()
//{
//
//}
//
//void NetworkLayer::beginFrame()
//{
//	sendDataToServer_ = false;
//	sendDataToClient_ = false;
//}
//
//void NetworkLayer::initialize()
//{
//	if (SDLNet_Init() == -1)
//	{
//		std::cout<<"SDLNet_Init: "<<SDLNet_GetError()<<std::endl;
//		return;
//	}
//
//	clientLayer_->initialize();
//	serverLayer_->initialize();
//	
//	udpClient_->clientLayer_ = clientLayer_;
//	udpClient_->threadManager_ = threadManager_;
//
//	udpServer_->serverLayer_ = serverLayer_;
//	udpServer_->threadManager_ = threadManager_;
//	
//	tcpServer_->threadManager_ = threadManager_;
//	tcpServer_->initialize();
//}
//
//void NetworkLayer::shutdown()
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			udpServer_->stop();
//		}
//	}
//
//	tcpServer_->shutdown();
//
//	if (udpClient_->getIsConnected() == true)
//	{
//		udpClient_->closeConnection();
//	}
//	
//	if (tcpConnection_ != nullptr)
//	{
//		tcpConnection_->closeConnection();
//	}
//	
//	SDLNet_Quit();
//}
//
//void NetworkLayer::client_start()
//{
//}
//
//bool NetworkLayer::getSendDataToServer()
//{
//	return sendDataToServer_;
//}
//
//bool NetworkLayer::getSendDataToClient()
//{
//	return sendDataToClient_;
//}
//
//void NetworkLayer::client_initiateConnection()
//{	
//	if (sendDataToServer_ == true)
//	{
//		//findme2
//		//streamLock_->lock();
//		//std::cout << "[" << boost::this_thread::get_id() << "] Requesting connection to server. "<< std::endl;
//		//streamLock_->unlock();
//
//		// The connect packet lets the server know the client is connecting and tells it which
//		// room the client player entity should be spawned in.
//		UdpPacket udpConnectPacket(UDP_PACKET_CONNECT);
//
//		udpConnectPacket.setBodyLength(0);
//	
//		udpClient_->sendData(udpConnectPacket);	
//	}
//}
//
//void NetworkLayer::client_sendInput(InputDevice* inputDevice)
//{
//	if (udpClient_->getIsConnected() == true)
//	{
//		// If the state of the input has changed since the last frame, send the update to the server.
//		if (inputDevice->getHasInputStateChanged() == true)
//		{
//			UdpPacket udpInputPacket = inputDevice->getInputPacket();
//
//			udpClient_->sendData(udpInputPacket);
//		}
//	}
//}
//
//void NetworkLayer::client_sendJoin(boost::shared_ptr<JoinData> joinData)
//{
//	if (sendDataToServer_ == true)
//	{
//		EntityTypeId spawnEntityId = joinData->getEntityTypeId();
//		RoomId joinRoomId = joinData->getRoomId();
//		SpawnPointId joinSpawnPointId = joinData->getSpawnPointId();
//
//		// If the join data isn't set, join hasn't been called yet.
//		if (spawnEntityId != ids_->ENTITY_NULL && joinRoomId != ids_->ROOM_NULL)
//		{
//
//			// The connect packet lets the server know the client is connecting and tells it which
//			// room the client player entity should be spawned in.
//			UdpPacket udpJoinPacket(UDP_PACKET_JOIN);
//
//			char* body = udpJoinPacket.getBody();
//
//			body[0] = networkUtility_.getLittleEndianByteFromInt(spawnEntityId, 0);
//			body[1] = networkUtility_.getLittleEndianByteFromInt(spawnEntityId, 1);
//			body[2] = networkUtility_.getLittleEndianByteFromInt(spawnEntityId, 2);
//			body[3] = networkUtility_.getLittleEndianByteFromInt(spawnEntityId, 3);
//	
//			body[4] = networkUtility_.getLittleEndianByteFromInt(joinRoomId, 0);
//			body[5] = networkUtility_.getLittleEndianByteFromInt(joinRoomId, 1);
//			body[6] = networkUtility_.getLittleEndianByteFromInt(joinRoomId, 2);
//			body[7] = networkUtility_.getLittleEndianByteFromInt(joinRoomId, 3);
//			
//			body[8] = networkUtility_.getLittleEndianByteFromInt(joinSpawnPointId, 0);
//			body[9] = networkUtility_.getLittleEndianByteFromInt(joinSpawnPointId, 1);
//			body[10] = networkUtility_.getLittleEndianByteFromInt(joinSpawnPointId, 2);
//			body[11] = networkUtility_.getLittleEndianByteFromInt(joinSpawnPointId, 3);
//	
//			// Add the entity state data into the body.
//
//			EntitySerializer* entitySerializer = joinData->getEntitySerializer();
//
//			SerializerCorePointer serializer = entitySerializer->getSerializerCore();
//
//			BufferWrapperPtr buffer = serializer->getBufferWrapper();
//
//			int entityDataBufferSize = buffer->getBufferSize();
//			char* entityDataBuffer = buffer->getBuffer();
//
//			for (int i = 0; i < entityDataBufferSize; i++)
//			{
//				body[12 + i] = entityDataBuffer[i];
//			}
//
//			//findme what happens if the size is greater than max packet size? need to send multiple joins.
//
//			udpJoinPacket.setBodyLength(12 + entityDataBufferSize);
//	
//			udpClient_->sendData(udpJoinPacket);
//		}
//		else
//		{
//			// If this code is being hit, the client has been synchronized. 
//			// The join should happen in the connection opened function of 
//			// the network handler. This function should only be called once,
//			// after the synchronization has taken place.
//			if (connectionOpenCalled_ == false)
//			{
//				networkHandler_->connectionOpened();
//
//				connectionOpenCalled_ = true;
//			}
//		}
//	}
//}
//
//void NetworkLayer::client_sendHeartbeat()
//{
//	if (sendDataToServer_ == true)
//	{
//		if (udpClient_->getIsConnected() == true)
//		{
//			// The connect packet lets the server know the client is connecting and tells it which
//			// room the client player entity should be spawned in.
//			UdpPacket udpHeartbeatPacket(UDP_PACKET_HEARTBEAT);
//
//			udpHeartbeatPacket.setBodyLength(0);
//	
//			// Don't log the time until the response for this packet was received.
//			if (clientLayer_->waitingForPingResponse_ == false)
//			{
//				clientLayer_->pingPacketId_ = udpHeartbeatPacket.getSequence();
//		
//				clientLayer_->gameTimer_->logTimeStart(clientLayer_->pingTimerId_);
//	
//				clientLayer_->waitingForPingResponse_ = true;
//			}
//
//			udpClient_->sendData(udpHeartbeatPacket);
//		}
//	}
//}
//
//int NetworkLayer::getParticipantCount()
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			return udpServer_->getParticipantCount();
//		}
//	}
//
//	return 0;
//}
//
//bool NetworkLayer::getIsParticipantSynchronized(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getIsSynchronized();
//			}
//		}
//	}
//
//	return false;
//}
//
//bool NetworkLayer::getParticipantHasJoined(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getHasJoined();
//			}
//		}
//	}
//
//	return false;
//}
//
//std::string	NetworkLayer::getParticipantIpAddress(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getIpAddress();
//			}
//		}
//	}
//
//	return "";
//}
//
//int NetworkLayer::getParticipantRoomId(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getCurrentRoomId();
//			}
//	
//		}
//	}
//
//	return ids_->ROOM_NULL;
//}
//
//TransitionId NetworkLayer::getParticipantTransitionId(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getCurrentTransitionId();
//			}
//	
//		}
//	}
//
//	return ids_->TRANSITION_NULL;
//}
//
//double NetworkLayer::getParticipantTransitionTime(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getCurrentTransitionTime();
//			}
//		}
//	}
//
//	return 0.0;
//}
//
//EntityTypeId NetworkLayer::getParticipantEntityTypeId(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getEntityTypeId();
//			}
//		}
//	}
//
//	return ids_->ENTITY_NULL;
//}
//
//SpawnPointId NetworkLayer::getParticipantSpawnPointId(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getInitialSpawnPointId();
//			}
//		}
//	}
//
//	return ids_->SPAWNPOINT_NULL;
//}
//
//BufferWrapperPtr  NetworkLayer::getParticipantEntityDataBuffer(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getEntityDataBuffer();
//			}
//		}
//	}
//	
//	return nullptr;
//}
//
//CameraController* NetworkLayer::getParticipantCamera(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getCamera();
//			}
//		}
//	}
//
//	return nullptr;
//}
//
//int NetworkLayer::getParticipantAssociatedEntityInstanceId(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getAssociatedEntityInstanceId();
//			}
//		}
//	}
//
//	return -1;
//}
//
//void NetworkLayer::setAssociatedEntityInstanceId(int participantIndex, int entityInstanceId)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->setAssociatedEntityInstanceId(entityInstanceId);
//			}
//		}
//	}
//}
//
//void NetworkLayer::setAssociatedEntityInstanceDelayTimer(int participantIndex, double* delayTimer)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->setAssociatedEntityInstanceDelayTimer(delayTimer);
//			}
//		}
//	}
//}
//
//void NetworkLayer::setAssociatedEntityInstanceDelayTime(int participantIndex, double* delayTime)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->setAssociatedEntityInstanceDelayTime(delayTime);
//			}
//		}
//	}
//}
//
//void NetworkLayer::setParticipantTransitionId(int participantIndex, TransitionId* transitionId)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->setCurrentTransitionId(transitionId);
//			}
//		}
//	}
//}
//
//int NetworkLayer::getParticipantInputDeviceId(int participantIndex)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->getInputChannel();
//			}
//		}
//	}
//
//	return -1;
//}
//
//void NetworkLayer::setParticipantInputDeviceId(int participantIndex, int inputDeviceId)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				return participant->setInputChannel(inputDeviceId);
//			}
//		}
//	}
//}
//
//	
//void NetworkLayer::server_sendRoomAwaitingActivationToClient(int participantIndex, RoomId roomId, TransitionId transitionId, double transitionTime)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			// Send the game state to the clients.
//			int participantCount = udpServer_->getParticipantCount();
//
//			if (participantIndex >= 0 && participantIndex < participantCount)
//			{		
//				UdpPacket udpLoadingRoomPacket(UDP_PACKET_AWAITING_ACTIVATION);
//
//				char* body = udpLoadingRoomPacket.getBody();
//						
//				body[0] = networkUtility_.getLittleEndianByteFromInt(roomId, 0);
//				body[1] = networkUtility_.getLittleEndianByteFromInt(roomId, 1);
//				body[2] = networkUtility_.getLittleEndianByteFromInt(roomId, 2);
//				body[3] = networkUtility_.getLittleEndianByteFromInt(roomId, 3);
//
//				body[4] = networkUtility_.getLittleEndianByteFromInt(transitionId, 0);
//				body[5] = networkUtility_.getLittleEndianByteFromInt(transitionId, 1);
//				body[6] = networkUtility_.getLittleEndianByteFromInt(transitionId, 2);
//				body[7] = networkUtility_.getLittleEndianByteFromInt(transitionId, 3);
//
//				body[8] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 0);
//				body[9] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 1);
//				body[10] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 2);
//				body[11] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 3);
//				body[12] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 4);
//				body[13] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 5);
//				body[14] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 6);
//				body[15] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 7);
//
//				udpLoadingRoomPacket.setBodyLength(16);
//						
//				udpServer_->sendToClient(participantIndex, udpLoadingRoomPacket);	
//			}
//		}
//	}
//}
//
//void NetworkLayer::server_sendRoomLoadedPercentToClient(int participantIndex, RoomId roomId, int percentLoaded, TransitionId transitionId, double transitionTime)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			// Send the game state to the clients.
//			int participantCount = udpServer_->getParticipantCount();
//
//			if (participantIndex >= 0 && participantIndex < participantCount)
//			{		
//				UdpPacket udpLoadingRoomPacket(UDP_PACKET_LOADING);
//
//				char* body = udpLoadingRoomPacket.getBody();
//						
//				body[0] = networkUtility_.getLittleEndianByteFromInt(roomId, 0);
//				body[1] = networkUtility_.getLittleEndianByteFromInt(roomId, 1);
//				body[2] = networkUtility_.getLittleEndianByteFromInt(roomId, 2);
//				body[3] = networkUtility_.getLittleEndianByteFromInt(roomId, 3);
//						
//				body[4] = networkUtility_.getLittleEndianByteFromInt(percentLoaded, 0);
//				body[5] = networkUtility_.getLittleEndianByteFromInt(percentLoaded, 1);
//				body[6] = networkUtility_.getLittleEndianByteFromInt(percentLoaded, 2);
//				body[7] = networkUtility_.getLittleEndianByteFromInt(percentLoaded, 3);
//				
//				body[8] = networkUtility_.getLittleEndianByteFromInt(transitionId, 0);
//				body[9] = networkUtility_.getLittleEndianByteFromInt(transitionId, 1);
//				body[10] = networkUtility_.getLittleEndianByteFromInt(transitionId, 2);
//				body[11] = networkUtility_.getLittleEndianByteFromInt(transitionId, 3);
//
//				body[12] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 0);
//				body[13] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 1);
//				body[14] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 2);
//				body[15] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 3);
//				body[16] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 4);
//				body[17] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 5);
//				body[18] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 6);
//				body[19] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 7);
//
//				udpLoadingRoomPacket.setBodyLength(20);
//				
//				udpServer_->sendToClient(participantIndex, udpLoadingRoomPacket);	
//			}
//		}
//	}
//}
//
//bool NetworkLayer::server_sendEntityListToClient(int participantIndex, RoomId roomId, TransitionId transitionId, double transitionTime)
//{
//	if (sendDataToClient_ == true && udpServer_ != nullptr)
//	{
//		UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//		if (participant == nullptr)
//		{
//			return false;
//		}
//		
//		//pack in as many entities will fit in a single packet.
//		// what happens if an entity is too big? it will need to be split across mutliple packets
//		// so this is where the reliability thing comes into play. 
//		
//		int entityCount = replicationStatesOnServer_->size();
//	
//		int validEntityCounter = 0;
//
//		// Add as many entities as will fit into the packet. Entity size will vary by entity.
//
//		// 4 bytes have already been set in the body, for the room ID.
//		int headerSize = 20;
//		int currentIndex = headerSize;
//		int baseDataSize = 49;
//
//		UdpPacket udpEntityListPacket(UDP_PACKET_ENTITY_LIST);
//
//		char* body = udpEntityListPacket.getBody();
//				
//		body[0] = networkUtility_.getLittleEndianByteFromInt(roomId, 0);
//		body[1] = networkUtility_.getLittleEndianByteFromInt(roomId, 1);
//		body[2] = networkUtility_.getLittleEndianByteFromInt(roomId, 2);
//		body[3] = networkUtility_.getLittleEndianByteFromInt(roomId, 3);
//	
//		// Bytes 4-7 contain the entity count and get set after the loop.
//
//		body[8] = networkUtility_.getLittleEndianByteFromInt(transitionId, 0);
//		body[9] = networkUtility_.getLittleEndianByteFromInt(transitionId, 1);
//		body[10] = networkUtility_.getLittleEndianByteFromInt(transitionId, 2);
//		body[11] = networkUtility_.getLittleEndianByteFromInt(transitionId, 3);
//
//		body[12] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 0);
//		body[13] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 1);
//		body[14] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 2);
//		body[15] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 3);
//		body[16] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 4);
//		body[17] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 5);
//		body[18] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 6);
//		body[19] = networkUtility_.getLittleEndianByteFromDouble(transitionTime, 7);
//
//		for (int j = 0; j < entityCount; j++)
//		{
//			// Eventually I will want to only send some entities to specific clients, but for now, send everything.
//		
//			EntityReplicationState* replicationState = (*replicationStatesOnServer_)[j];
//			
//			EntitySerializer* entitySerializer = replicationState->getEntitySerializer();
//
//			SerializerCorePointer serializer = entitySerializer->getSerializerCore();
//
//			int entityStateDataSize = serializer->getBufferWrapper()->getBufferSize();
//			int entityTotalDataSize = baseDataSize + entityStateDataSize;
//
//			// If this entity won't fit in this packet, send it off and start building a new packet.
//			if (currentIndex + entityTotalDataSize <= UdpPacket::MAX_BODY_LENGTH)
//			{
//				validEntityCounter++;
//
//				bytemap toBytes;
//
//				int entityTypeId = replicationState->getEntityTypeId();				
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(entityTypeId, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(entityTypeId, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(entityTypeId, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(entityTypeId, 3);
//
//				int serverId = replicationState->getServerId();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(serverId, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(serverId, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(serverId, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(serverId, 3);
//					
//				int mapLayer = replicationState->getMapLayer();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(mapLayer, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(mapLayer, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(mapLayer, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(mapLayer, 3);
//					
//				int stateIndex = replicationState->getStateIndex();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(stateIndex, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(stateIndex, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(stateIndex, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromInt(stateIndex, 3);
//			
//				float positionX = replicationState->getPositionX();
//
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(positionX, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(positionX, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(positionX, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(positionX, 3);
//
//				float positionY = replicationState->getPositionY();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(positionY, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(positionY, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(positionY, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(positionY, 3);
//				
//				float velocityX = replicationState->getVelocityX();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(velocityX, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(velocityX, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(velocityX, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(velocityX, 3);
//
//				float velocityY = replicationState->getVelocityY();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(velocityY, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(velocityY, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(velocityY, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(velocityY, 3);
//			
//				float movementX = replicationState->getMovementX();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(movementX, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(movementX, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(movementX, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(movementX, 3);
//
//				float movementY = replicationState->getMovementY();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(movementY, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(movementY, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(movementY, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(movementY, 3);
//			
//				float lookX = replicationState->getLookX();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(lookX, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(lookX, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(lookX, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(lookX, 3);	
//
//				float lookY = replicationState->getLookY();
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(lookY, 0);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(lookY, 1);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(lookY, 2);
//				body[currentIndex++] = networkUtility_.getLittleEndianByteFromFloat(lookY, 3);	
//
//				unsigned char attachCamera = 0;
//
//				// If this entity is associated with this client...
//				if (participant->getIpAddress() == replicationState->getIpAddress())
//				{
//					attachCamera = 0x80;
//				}
//			
//				unsigned char acceptInput = 0;
//
//				// If this entity is associated with this client...
//				if (participant->getIpAddress() == replicationState->getIpAddress())
//				{
//					acceptInput = 0x40;
//				}
//
//				unsigned char bitFlags = attachCamera | acceptInput;
//
//				// Use a one byte bitflag for boolean values. (Attach Camera, attach input)
//				body[currentIndex++] = (char)bitFlags;
//
//				entitySerializer->preSerialize();
//
//				char* entityData = serializer->getBufferWrapper()->getBuffer();
//
//				for (int i = 0; i < entityStateDataSize; i++)
//				{
//					body[currentIndex++] = entityData[i];
//				}		
//			}
//			else
//			{
//				// Note: Make sure this gets tested. Right now, there aren't enough entities to cause it to
//				// hit this block. maybe add two entities with a lot of state data?
//
//				// Set the entity count in the header block.			
//				body[4] = networkUtility_.getLittleEndianByteFromInt(validEntityCounter, 0);
//				body[5] = networkUtility_.getLittleEndianByteFromInt(validEntityCounter, 1);
//				body[6] = networkUtility_.getLittleEndianByteFromInt(validEntityCounter, 2);
//				body[7] = networkUtility_.getLittleEndianByteFromInt(validEntityCounter, 3);
//
//				// Max packet size was reached. Send this packet off and start a new one.				
//				udpEntityListPacket.setBodyLength(currentIndex);
//
//				udpServer_->sendToClient(participantIndex, udpEntityListPacket);
//
//
//				// Create the new packet and reset the index counter.
//				udpEntityListPacket = UdpPacket(UDP_PACKET_ENTITY_LIST);
//
//				char* body = udpEntityListPacket.getBody();
//				
//				body[0] = networkUtility_.getLittleEndianByteFromInt(roomId, 0);
//				body[1] = networkUtility_.getLittleEndianByteFromInt(roomId, 1);
//				body[2] = networkUtility_.getLittleEndianByteFromInt(roomId, 2);
//				body[3] = networkUtility_.getLittleEndianByteFromInt(roomId, 3);
//			
//				currentIndex = headerSize - 1;
//
//				validEntityCounter = 0;
//			}			
//		}
//	
//		// Set the entity count in the header block.		
//		body[4] = networkUtility_.getLittleEndianByteFromInt(validEntityCounter, 0);
//		body[5] = networkUtility_.getLittleEndianByteFromInt(validEntityCounter, 1);
//		body[6] = networkUtility_.getLittleEndianByteFromInt(validEntityCounter, 2);
//		body[7] = networkUtility_.getLittleEndianByteFromInt(validEntityCounter, 3);
//		
//		udpEntityListPacket.setBodyLength(currentIndex);
//
//		udpServer_->sendToClient(participantIndex, udpEntityListPacket);
//
//		return true;
//	}
//
//	return false;
//}
//
//bool NetworkLayer::isConnectedToHost()
//{
//	return udpClient_->getIsConnected();
//}
//
//bool NetworkLayer::isServerStarted()
//{
//	if (udpServer_ != nullptr)
//	{
//		return udpServer_->getIsStarted();
//	}
//
//	return false;
//}
//
//void NetworkLayer::connectToHost(std::string ip)
//{
//	std::cout << "Connect to host" << std::endl;
//
//	if (udpClient_->getIsConnected())
//	{
//		udpClient_->closeConnection();
//	}
//	
//	udpClient_->connect(ip, udpClientPort_, udpServerPort_);
//	
//	if (tcpConnection_ != nullptr)
//	{
//		tcpConnection_->closeConnection();
//
//		tcpConnection_->chatLineReceivedSignal_.disconnect(boost::bind(&NetworkLayer::client_chatLineReceived, this, _1));
//		tcpConnection_->connectionLostSignal_.disconnect(boost::bind(&NetworkLayer::client_lostConnectionToHost, this));
//		
//		delete tcpConnection_;
//	}
//	
//	tcpConnection_ = new TcpConnection(ip, tcpPort_);
//	
//	tcpConnection_->chatLineReceivedSignal_.connect(boost::bind(&NetworkLayer::client_chatLineReceived, this, _1));
//	tcpConnection_->connectionLostSignal_.connect(boost::bind(&NetworkLayer::client_lostConnectionToHost, this));
//
//	tcpConnection_->threadManager_ = threadManager_;
//
//	tcpConnection_->openConnection();
//}
//
//void NetworkLayer::client_chatLineReceived(std::string chatLine)
//{
//	chatRoom_->addChatLine(chatLine);
//}
//
//void NetworkLayer::startServer()
//{
//	// Stop the server if it's already been started.
//	if (udpServer_->getIsStarted() == true)
//	{
//		udpServer_->stop();
//	
//		// Disconnect the signal before connecting it again.
//		udpServer_->clientConnectedSignal_.disconnect(boost::bind(&NetworkLayer::server_clientConnected, this, _1));
//		udpServer_->clientJoinedSignal_.disconnect(boost::bind(&NetworkLayer::server_clientJoined, this, _1));
//	}
//	
//	udpServer_->clientConnectedSignal_.connect(boost::bind(&NetworkLayer::server_clientConnected, this, _1));
//	udpServer_->clientJoinedSignal_.connect(boost::bind(&NetworkLayer::server_clientJoined, this, _1));
//
//	udpServer_->start(udpServerPort_, udpClientPort_);
//
//	networkHandler_->serverStarted();
//}
//
//void NetworkLayer::serverTick(double time, bool updateOccurred)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (udpServer_->getIsStarted() == true)
//		{
//			// Update the idle timers in each of the currently connected clients.
//			// Drop the participant if it's been idle for too long.
//			int clientCount = udpServer_->getParticipantCount();
//
//			for (int i = clientCount - 1; i >= 0; i--)
//			{
//				UdpParticipant* participant = udpServer_->getParticipant(i);
//
//				participant->incrementIdleTimer(time);
//
//				if (participant->getIdleTime() > clientConnectionTimeout_)
//				{
//					connectionTimeoutSignal_(i);
//
//					udpServer_->connectionTimeout(i);
//
//					tcpServer_->shutdown();
//				}
//			}
//		
//			// Send packets at a fixed rate. Accumulate time until enough time has passed to send the next packet.
//			serverPacketSendAccumulator_ += time;
//
//			if (serverPacketSendAccumulator_ > serverPacketSendRate_)
//			{		
//				// Only trigger a send if an update has occured. This is because some of the state data gets reset after an update, and
//				// if it didn't have the chance to reset it during the update, it will send bad data.
//				if (updateOccurred == true)
//				{		
//					// Reset the accumulator, don't run another update until enough time has elapsed
//					serverPacketSendAccumulator_ -= serverPacketSendRate_;
//					//serverPacketSendAccumulator_ = 0.0;
//	
//					sendDataToClient_ = true;
//				}
//			}
//		}
//	}
//}
//
//void NetworkLayer::clientTick(double time, bool updateOccurred)
//{
//	if (udpClient_->getIsConnected() == true)
//	{
//		// Send packets at a fixed rate. Accumulate time until enough time has passed to send the next packet.
//		clientPacketSendAccumulator_ += time;
//		
//		if (clientPacketSendAccumulator_ > clientPacketSendRate_)
//		{	
//			// Only trigger a send if an update has occured. This is because some of the state data (what data?) gets reset after an update, and
//			// if it didn't have the chance to reset it during the update, it will send bad data (bad how?).
//			if (updateOccurred == true)
//			{			
//				// Reset the accumulator, don't run another update until enough time has elapsed
//				clientPacketSendAccumulator_ -= clientPacketSendRate_;
//				//clientPacketSendAccumulator_ = 0.0;
//	
//				sendDataToServer_ = true;
//			}
//		}
//
//		//udpClient_->incrementIdleTimer(time);
//
//		if (udpClient_->getIdleTime() > serverConnectionTimeout_)
//		{
//			udpClient_->closeConnection();
//			
//			connectionToHostLostSignal_();
//
//			connectionOpenCalled_ = false;
//		
//			tcpConnection_->closeConnection();
//
//			networkHandler_->connectionClosedByServer();
//		}
//
//		clientLayer_->clientPingTimer_ += time;
//		
//		if (clientLayer_->clientPingTimer_ > clientLayer_->clientPingInterval_)
//		{	
//			double averagePingTime = clientLayer_->clientPingAccumulator_ / clientLayer_->clientPingCounter_;
//
//			int pingTimeMs = (unsigned int)averagePingTime;
//
//			clientLayer_->gameTimer_->pingTime_ = pingTimeMs;
//
//			clientLayer_->clientPingTimer_ = 0.0;
//			clientLayer_->clientPingCounter_ = 0;
//			clientLayer_->clientPingAccumulator_ = 0.0;
//		}
//	}
//}
//
//void NetworkLayer::server_clientConnected(int participantIndex)
//{
//	std::string ipAddress = getParticipantIpAddress(participantIndex);
//	
//	std::cout<<"Connection initialized for client "<<ipAddress<<"."<<std::endl;
//}
//
//void NetworkLayer::server_clientJoined(int participantIndex)
//{
//	UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//	
//	if (participant->getHasJoined() == false)
//	{
//		participant->setHasJoined(true);
//
//		server_clientJoinedSignal_(participantIndex);
//	}
//}
//
//void NetworkLayer::client_lostConnectionToHost()
//{
//	networkHandler_->connectionClosedByServer();
//}
//
//void NetworkLayer::server_setInputDeviceManager(InputDeviceManager* inputDeviceManager)
//{
//	if (udpServer_ != nullptr)
//	{
//		udpServer_->setInputDeviceManager(inputDeviceManager);
//	}
//}
//
//void NetworkLayer::linkCameraToParticipant(int participantIndex, CameraController* camera)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (participantIndex >= 0)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{
//				participant->setCamera(camera);
//			}
//		}
//	}
//}
//
//void NetworkLayer::linkRoomToParticipant(int participantIndex, RoomId roomId)
//{
//	if (udpServer_ != nullptr)
//	{
//		if (participantIndex >= 0)
//		{
//			UdpParticipant* participant = udpServer_->getParticipant(participantIndex);
//
//			if (participant != nullptr)
//			{					
//				participant->setCurrentRoomId(roomId);
//			}
//		}
//	}
//}
//
//
//void NetworkLayer::sendChatString(std::string message)
//{
//	if (tcpConnection_ != nullptr)
//	{
//		tcpConnection_->writeChatMessage(message);	
//	}
//	else
//	{
//		chatRoom_->addChatLine("Not connected to server.");
//	}
//}