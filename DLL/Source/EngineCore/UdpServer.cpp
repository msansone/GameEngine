//#include "..\..\Headers\EngineCore\UdpServer.hpp"
//
//using namespace firemelon;
//using namespace boost;
//using namespace boost::asio;
//using namespace boost::asio::ip;
//
//UdpServer::UdpServer()
//{
//	isStarted_ = false;
//	clientPort_ = 0;
//}
//
//UdpServer::~UdpServer()
//{
//	int size = participants_.size();
//
//	for (int i = 0; i < size; i++)
//	{
//		if (participants_[i] != nullptr)
//		{
//			delete participants_[i];
//		}
//	}
//
//	participants_.clear();
//}
//
//void UdpServer::start(short serverPort, short clientPort)
//{
//	if (isStarted_ == false)
//	{
//		threadManager_->workerThreads_.create_thread(boost::bind(&UdpServer::handleReceiveThread, this));
//
//		isStarted_ = true;
//
//		clientPort_ = clientPort;
//
//		socket_ = SDLNet_UDP_Open(serverPort);
//
//		if (socket_ == nullptr)
//		{
//			std::cout << "Unable to open UDP socket on port " << serverPort << "." << std::endl;
//		}
//
//		udpPacket_.sdlPacket_ = SDLNet_AllocPacket(UdpPacket::MAX_PACKET_SIZE);
//
//		if (udpPacket_.sdlPacket_ == nullptr)
//		{
//			std::cout << "SDLNet_AllocPacket failed on server : " << SDLNet_GetError() << std::endl;
//			return;
//		}
//	}
//}
//
//void UdpServer::stop()
//{
//	if (isStarted_ == true)
//	{
//		isStarted_ = false;
//
//		SDLNet_UDP_Close(socket_);
//
//		SDLNet_FreePacket(udpPacket_.sdlPacket_);
//	}
//}
//
//void UdpServer::handleReceiveThread()
//{
//	while (true)
//	{
//		// Receieve UDP packets while server is started.
//		if (isStarted_ == false)
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
//int UdpServer::getParticipantCount()
//{
//	return participants_.size();
//}
//
//UdpParticipant* UdpServer::getParticipant(int index)
//{
//	int size = participants_.size();
//
//	if (index >= 0 && index < size)
//	{
//		return participants_[index];
//	}
//
//	return nullptr;
//}
//
//void UdpServer::sendToClient(int index, UdpPacket packet)
//{
//	int size = participants_.size();
//
//	if (index >= 0 && index < size)
//	{
//		UdpParticipant* participant = participants_[index];
//
//		packet.sdlPacket_ = participant->sdlPacket_;
//
//		packet.encodeHeader();
//
//		int packetsSent = SDLNet_UDP_Send(socket_, -1, packet.sdlPacket_);
//
//		if (packetsSent == 0)
//		{
//			std::cout << "SDLNet_UDP_Send failed : " << SDLNet_GetError() << std::endl;
//		}		
//	}
//}
//
//void UdpServer::setInputDeviceManager(InputDeviceManager* inputDeviceManager)
//{
//	inputDeviceManager_ = inputDeviceManager;
//}
//
//bool UdpServer::isParticipantConnected(std::string ipAddress)
//{
//	int clientCount = participants_.size();
//
//	for (int i = 0; i < clientCount; i++)
//	{
//		if (participants_[i]->ipAddress_ == ipAddress)
//		{
//			return true;
//		}
//	}
//
//	return false;
//}
//
//bool UdpServer::getIsStarted()
//{
//	return isStarted_;
//}
//
//int UdpServer::getParticipantIndex(std::string ipAddress)
//{	
//	int clientCount = participants_.size();
//
//	for (int i = 0; i < clientCount; i++)
//	{
//		if (participants_[i]->ipAddress_ == ipAddress)
//		{
//			return i;
//		}
//	}
//
//	return -1;
//}
//
//
//void UdpServer::connectionTimeout(int index)
//{
//	std::cout<<"Getting Client "<<index<<std::endl;
//
//	UdpParticipant* client = participants_[index];
//	
//	std::cout<<"Deleting Client "<<index<<std::endl;
//
//	delete client;
//	
//	std::cout<<"Removing client from list"<<std::endl;
//
//	participants_.erase(participants_.begin() + index);
//}
//
//void UdpServer::processPacket()
//{
//	std::string ipAddress = udpPacket_.receivedFromIpAddress;
//
//	int participantIndex = getParticipantIndex(ipAddress);
//
//	bool isClientConnected = isParticipantConnected(ipAddress);
//
//	UdpPacketType packetType = udpPacket_.getPacketType();
//
//	switch (packetType)
//	{
//		case UDP_PACKET_INPUT:
//		{
//			if (isClientConnected == true)
//			{
//				std::vector<GameButtonId> buttonIds;
//				std::vector<ButtonState> buttonStates;
//
//				char* body = udpPacket_.getBody();
//
//				int bodyLength = udpPacket_.getBodyLength();
//
//				for (int i = 0 ; i < bodyLength; i += 5)
//				{
//					GameButtonId buttonId = networkUtility_.buildInteger(body[i + 3], body[i + 2], body[i + 1], body[i]);
//					
//					ButtonState buttonState = (ButtonState)body[i + 4];
//
//					buttonIds.push_back(buttonId);
//					buttonStates.push_back(buttonState);
//				}
//			
//				serverLayer_->inputReceivedFromClientSignal_(buttonIds, buttonStates, ipAddress);				
//			}
//
//			break;
//		}
//		case UDP_PACKET_CONNECT:
//
//			participantIndex = registerParticipant(ipAddress);
//
//			sendIdListToClient(participantIndex);
//
//			break;
//		
//		case UDP_PACKET_JOIN:
//		{
//			UdpParticipant* connection = participants_[participantIndex];
//				
//			// A join packet can not be sent until the client has synchronized. This is because
//			// it sends IDs from the client to the server, and it needs to know how to translate 
//			// them correctly.
//			connection->isSynchronized_ = true;
//
//			if (connection->getHasJoined() == false)
//			{
//				char* body = udpPacket_.getBody();
//
//				EntityTypeId spawnEntityId = networkUtility_.buildInteger(body[0], body[1], body[2], body[3]);
//				RoomId roomId = networkUtility_.buildInteger(body[4], body[5], body[6], body[7]);				
//				SpawnPointId spawnPointId = networkUtility_.buildInteger(body[8], body[9], body[10], body[11]);
//				
//				// Read the entity state data serialized array here.
//				int dataLength = networkUtility_.buildInteger(body[12], body[13], body[14], body[15]);
//				
//				BufferWrapperPtr bufferWrapper = connection->getEntityDataBuffer();
//
//				bufferWrapper->allocateBuffer(dataLength);
//
//				for (int i = 0; i < dataLength; i++)
//				{
//					bufferWrapper->setChar(i, body[12 + i]);
//				}
//
//				connection->setEntityTypeId(spawnEntityId);
//				connection->setCurrentRoomId(roomId);
//				connection->getInitialSpawnPointId(spawnPointId);
//			}
//
//			clientJoinedSignal_(participantIndex);
//			
//			break;
//		}
//		case UDP_PACKET_HEARTBEAT:
//				
//			if (isClientConnected == true)
//			{
//				// A heartbeat was received from the client. Reset the heartbeat counter for it to 0.				
//				UdpParticipant* connection = participants_[participantIndex];
//
//				connection->idleTimer_ = 0.0;
//
//				// Send a response to the client, so it can calculate ping.
//					
//				UdpPacket udpPacketHeartbeatReply(UDP_PACKET_HEARTBEAT);
//				
//				// Acknowledge the sequence.
//				udpPacketHeartbeatReply.setAck(udpPacket_.getSequence());
//
//				udpPacketHeartbeatReply.setBodyLength(0);
//
//				sendToClient(participantIndex, udpPacketHeartbeatReply);
//			}
//
//			break;
//	}
//}
//
//int UdpServer::registerParticipant(std::string ipAddress)
//{
//	bool isClientConnected = isParticipantConnected(ipAddress);
//	
//	int participantIndex = -1;
//
//	// If the client is not currently connected, create a new connection object associated with it.
//	if (isClientConnected == false)
//	{
//		DebugHelper::streamLock->lock();
//		std::cout << "[" << boost::this_thread::get_id() << "] Client at "<<ipAddress<<" has connected to the server."<< std::endl;
//		DebugHelper::streamLock->unlock();
//
//		int participantCount = participants_.size();
//					
//		participantIndex = participantCount;
//					
//		UdpParticipant* newParticipant = new UdpParticipant(clientPort_);
//				
//		newParticipant->initialize(ipAddress);
//
//		participants_.push_back(newParticipant);
//				
//		clientConnectedSignal_(participantCount);
//	}
//	else
//	{
//		participantIndex = getParticipantIndex(ipAddress);
//	}
//
//	return participantIndex;
//}
//
//void UdpServer::sendIdListToClient(int connectionIndex)
//{
//	DebugHelper::streamLock->lock();
//	std::cout << "[" << boost::this_thread::get_id() << "] Sending IDs to client. "<< std::endl;
//	DebugHelper::streamLock->unlock();
//
//	// Send the server IDs. This will serve as the acknowledgement that the client was accepted.
//	// Once the client has successfully mapped all of the IDs it can begin.
//
//	// Send the client the list of IDs from the server, so it can translate from its local IDs.
//	UuidIntegerMap uuidIntMap = BaseIds::getUuidIntegerMap();
//
//	int integerSize = sizeof(int);
//
//	int uuidSize = boost::uuids::uuid::static_size();
//				
//	int idCount = uuidIntMap.size();
//
//	// The number of packets to send all IDs to the client.
//	int totalSize = idCount * (uuidSize + integerSize);
//
//	int packetsNeeded = totalSize / UdpPacket::MAX_BODY_LENGTH;
//	
//	if (totalSize % UdpPacket::MAX_BODY_LENGTH > 0)
//	{
//		packetsNeeded++;
//	}
//
//	int idsPerPacket = UdpPacket::MAX_BODY_LENGTH / (uuidSize + integerSize);
//				
//	// Initialize the iterator to the beginning of the list.
//	std::map<boost::uuids::uuid, int>::const_iterator it;
//		
//	it = uuidIntMap.begin();
//			
//	for (int i = 0; i < packetsNeeded; i++)
//	{
//		UdpPacket udpPacketServerIds(UDP_PACKET_SERVER_ID_LIST);
//
//		char* serverIdsBody = udpPacketServerIds.getBody();
//
//		int counter = 0;
//		for (; it != uuidIntMap.end(); ++it)
//		{
//			int startIndex = counter * (uuidSize + integerSize);
//
//			// Insert the UUID and the ID into the array.
//			boost::uuids::uuid uuid = it->first;
//
//			for (int i = 0; i < uuidSize; i++)
//			{
//				serverIdsBody[startIndex + i] = uuid.data[i];
//			}
//			
//			int id = it->second;
//						
//			serverIdsBody[startIndex + uuidSize] = (char)((id>>24) & 0xFF);
//			serverIdsBody[startIndex + uuidSize + 1] = (char)((id>>16) & 0xFF);
//			serverIdsBody[startIndex + uuidSize + 2] = (char)((id>>8) & 0xFF);
//			serverIdsBody[startIndex + uuidSize + 3] = (char)(id & 0xFF);
//			
//			counter++;
//
//			if (counter >= idsPerPacket)
//			{
//				// The max amount of ids that will fit has been reached.
//				break;
//			}
//		}
//		
//		if (it != uuidIntMap.end())
//		{
//			++it;
//		}
//					
//		udpPacketServerIds.setBodyLength(counter * (uuidSize + integerSize));
//					
//		sendToClient(connectionIndex, udpPacketServerIds);
//	}
//}