//#include "..\..\Headers\EngineCore\UdpClient.hpp"
//
//using namespace firemelon;
//using namespace boost;
//
//UdpClient::UdpClient()
//{
//	isConnected_ = false;
//
//	idleTimer_ = 0.0;
//}
//
//UdpClient::~UdpClient()
//{
//	closeConnection();
//}
//
//bool UdpClient::getIsConnected()
//{
//	return isConnected_;
//}
//
//void UdpClient::connect(std::string ip, short clientPort, short serverPort)
//{
//	if (isConnected_ == false)
//	{
//		threadManager_->workerThreads_.create_thread(boost::bind(&UdpClient::handleReceiveThread, this));
//
//		socket_ = SDLNet_UDP_Open(clientPort);
//
//		if (socket_ == nullptr)
//		{
//			std::cout << "Unable to open UDP socket on port " << clientPort << ": " << SDLNet_GetError() << std::endl;
//		}
//
//		IPaddress serverIpAddress;
//		SDLNet_ResolveHost(&serverIpAddress, ip.c_str(), serverPort);
//
//		udpPacket_.sdlPacket_ = SDLNet_AllocPacket(UdpPacket::MAX_PACKET_SIZE);
//
//		if (udpPacket_.sdlPacket_ == nullptr)
//		{
//			std::cout << "SDLNet_AllocPacket failed : " << SDLNet_GetError() << std::endl;
//			return;
//		}
//
//		udpPacket_.sdlPacket_->address.host = serverIpAddress.host;
//		udpPacket_.sdlPacket_->address.port = serverIpAddress.port;
//
//		isConnected_ = true;
//	}
//}
//
//void UdpClient::closeConnection()
//{
//	if (isConnected_ == true)
//	{
//		SDLNet_UDP_Close(socket_);
//
//		SDLNet_FreePacket(udpPacket_.sdlPacket_);
//
//		idleTimer_ = 0.0;
//		isConnected_ = false;
//	}
//}
//
//void UdpClient::incrementIdleTimer(double time)
//{
//	idleTimer_ += time;
//}
//
//double UdpClient::getIdleTime()
//{
//	return idleTimer_;
//}
//
//void UdpClient::sendData(UdpPacket udpPacket)
//{
//	udpPacket.sdlPacket_ = udpPacket_.sdlPacket_;
//
//	udpPacket.encodeHeader();
//
//	int packetsSent = SDLNet_UDP_Send(socket_, -1, udpPacket_.sdlPacket_);
//
//	if (packetsSent == 0)
//	{
//		std::cout << "SDLNet_UDP_Send failed : " << SDLNet_GetError() << std::endl;		
//	}
//}
//
//void UdpClient::handleReceiveThread()
//{
//	while (true)
//	{
//		// Receieve UDP packets while the client is connected is started.
//		if (isConnected_ == false)
//		{
//			return;
//		}
//
//		int result = SDLNet_UDP_Recv(socket_, udpPacket_.sdlPacket_);
//
//		if (result == 1)
//		{
//			// Packet received.			
//			if (udpPacket_.decodeHeader() == true)
//			{
//				processPacket();
//			}
//		}
//		else if (result == -1)
//		{
//			std::cout << "Error receiving UDP packet" << std::endl;
//		}
//	}
//}
//
//void UdpClient::processPacket()
//{
//	idleTimer_ = 0.0;
//		
//	UdpPacketType packetType = udpPacket_.getPacketType();		
//	int bodyLength = udpPacket_.getBodyLength();
//	char* body = udpPacket_.getBody();
//
//	switch (packetType)
//	{
//		case UDP_PACKET_SERVER_ID_LIST: // Read a batch of IDs from the server, and map them to client IDs.
//		{
//			DebugHelper::streamLock->lock();
//			std::cout << "[" << boost::this_thread::get_id() << "] Synching IDs on client. "<<std::endl;
//			DebugHelper::streamLock->unlock();
//			
//			if (clientLayer_->isClientSynchronized == false)
//			{
//				int uuidSize = boost::uuids::uuid::static_size();
//				
//				// Combined size of a UUID and an integer ID.
//				int combinedSize = sizeof(int) + uuidSize;
//			
//				for (int i = 0; i < bodyLength; i += combinedSize)
//				{
//					// Read the UUID from the packet body.
//					boost::uuids::uuid uuid = networkUtility_.buildUuid(body, i);			
//						
//					// Read the server generated integer ID from the packet body.
//					int serverId = networkUtility_.buildInteger(body, i + uuidSize);
//
//					// Get the locally generated integer ID.
//					int localId = BaseIds::getIntegerFromUuid(uuid);
//
//					clientLayer_->mapLocalIdToServerId(localId, serverId);
//				}
//
//				DebugHelper::streamLock->lock();
//				std::cout << "[" << boost::this_thread::get_id() << "] ID batch processed on client. "<<std::endl;
//				DebugHelper::streamLock->unlock();
//			}
//			
//			break;
//		}		
//		case UDP_PACKET_HEARTBEAT:
//		{
//			// Received a response heart beat from the server.		
//			clientLayer_->heartbeatResponse(udpPacket_.getAck());
//
//			break;
//		}
//		case UDP_PACKET_LOADING:
//		{
//			// If the client is receiving room loading packets, then it must have joined.
//			clientLayer_->clientJoinSuccessful = true;
//
//			RoomId roomId = networkUtility_.buildInteger(body[0], body[1], body[2], body[3]);
//			int percentLoaded = networkUtility_.buildInteger(body[4], body[5], body[6], body[7]);
//			TransitionId transitionId = networkUtility_.buildInteger(body[8], body[9], body[10], body[11]);
//			double transitionTime = networkUtility_.buildDouble(body[12], body[13], body[14], body[15], body[16], body[17], body[18], body[19]);
//
//			clientLayer_->roomLoadingOnServerSignal_(roomId, percentLoaded, transitionId, transitionTime);	
//
//			break;
//		}
//		case UDP_PACKET_AWAITING_ACTIVATION:
//		{
//			// If the client is receiving room loading packets, then it must have joined.
//			clientLayer_->clientJoinSuccessful = true;
//
//			RoomId roomId = networkUtility_.buildInteger(body[0], body[1], body[2], body[3]);
//			TransitionId transitionId = networkUtility_.buildInteger(body[4], body[5], body[6], body[7]);
//			double transitionTime = networkUtility_.buildDouble(body[8], body[9], body[10], body[11], body[12], body[13], body[14], body[15]);
//
//			clientLayer_->roomAwaitingActivationOnServerSignal_(roomId, transitionId, transitionTime);	
//
//			break;
//		}
//		case UDP_PACKET_ENTITY_LIST:
//		{
//			//DebugHelper::streamLock->lock();
//			//std::cout << "[" << boost::this_thread::get_id() << "] Unpacking replication states on client. "<< std::endl;
//			//DebugHelper::streamLock->unlock();
//
//			// If the client is receiving entities, it must have joined.
//			clientLayer_->clientJoinSuccessful = true;
//
//			RoomId roomId = networkUtility_.buildInteger(body[0], body[1], body[2], body[3]);
//			int entityCount = networkUtility_.buildInteger(body[4], body[5], body[6], body[7]);
//			TransitionId transitionId = networkUtility_.buildInteger(body[8], body[9], body[10], body[11]);
//			double transitionTime = networkUtility_.buildDouble(body[12], body[13], body[14], body[15], body[16], body[17], body[18], body[19]);
//
//			int bodyLength = udpPacket_.getBodyLength();
//				
//			int headerSize = 20;
//			int currentIndex = headerSize;
//			int baseDataSize = 49;
//
//			// Populate the entity replication state container on the client.
//			clientLayer_->replicationStatesFromServer_->clear();
//
//			for (int i = 0; i < entityCount; i++)
//			{
//				EntityReplicationState* replicationState = clientLayer_->replicationStatesFromServer_->addNewReplicationState();
//
//				// Extract the entity type.
//
//				EntityTypeId entityTypeId = networkUtility_.buildInteger(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//
//				replicationState->setEntityTypeId(entityTypeId);
//					
//				// Extract the server ID.
//				int serverId = networkUtility_.buildInteger(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//
//				currentIndex += 4;
//
//				replicationState->setServerId(serverId);
//
//				// Extract the map layer.
//				int mapLayer = networkUtility_.buildInteger(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//
//				currentIndex += 4;
//
//				replicationState->setMapLayer(mapLayer);
//
//				// Extract the state index.
//				int stateIndex = networkUtility_.buildInteger(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//
//				currentIndex += 4;
//
//				replicationState->setStateIndex(stateIndex);
//					
//				// Extract the x position.
//				float positionX = networkUtility_.buildFloat(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//
//				replicationState->setPositionX(positionX);
//
//				// Extract the y position.
//				float positionY = networkUtility_.buildFloat(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//
//				replicationState->setPositionY(positionY);
//					
//				// Extract the x velocity.
//				float velocityX = networkUtility_.buildFloat(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//
//				replicationState->setVelocityX(velocityX);
//					
//				// Extract the y velocity.
//				float velocityY = networkUtility_.buildFloat(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//
//				replicationState->setVelocityY(velocityY);
//					
//				// Extract the x movement.
//				float movementX = networkUtility_.buildFloat(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//
//				replicationState->setMovementX(movementX);
//					
//				// Extract the y movement.
//				float movementY = networkUtility_.buildFloat(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//
//				replicationState->setMovementY(movementY);
//					
//				// Extract the look vector x.
//				float lookX = networkUtility_.buildFloat(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//	
//				replicationState->setLookX(lookX);
//					
//				// Extract the look vector y.
//				float lookY = networkUtility_.buildFloat(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//				
//				currentIndex += 4;
//	
//				replicationState->setLookY(lookY);
//
//				unsigned char bitFlags = (unsigned char)(body[currentIndex++]);
//
//				if (bitFlags & 0x80)
//				{
//					replicationState->setAttachCamera(true);
//				}
//	
//				if (bitFlags & 0x40)
//				{
//					replicationState->setAcceptInput(true);
//				}
//
//				// Read the entity state data size.					
//				int entityStateDataSize = networkUtility_.buildInteger(body[currentIndex], body[currentIndex + 1], body[currentIndex + 2], body[currentIndex + 3]);
//
//				BufferWrapperPtr buffer = replicationState->getEntityDataBuffer();
//
//				buffer->allocateBuffer(entityStateDataSize);
//
//				// Read the entity state data, and set it in the replication state.
//				for (int j = 0; j < entityStateDataSize; j++)
//				{
//					buffer->setChar(j, body[currentIndex++]);
//				}
//			}
//				
//			clientLayer_->entityListReceivedFromServerSignal_(roomId, transitionId, transitionTime);
//				
//			break;
//		}
//	}
//}