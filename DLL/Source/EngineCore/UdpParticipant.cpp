//#include "..\..\Headers\EngineCore\UdpParticipant.hpp"
//
//using namespace firemelon;
//
//UdpParticipant::UdpParticipant(short clientPort) :
//	bufferWrapper_(new BufferWrapper()),
//	ids_()
//{
//	ipAddress_ = "";
//
//	hasSpawnedEntity_ = false;
//	
//	entityTypeId_ = ids_.ENTITY_NULL;
//	spawnPointId_ = ids_.SPAWNPOINT_NULL;
//	currentRoomId_ = ids_.ROOM_NULL;
//	inputChannel_ = -1;
//	associatedEntityInstanceId_ = -1;
//	idleTimer_ = 0.0;
//
//	camera_ = nullptr;
//	associatedEntityDelayTime_ = nullptr;
//	associatedEntityDelayTimer_ = nullptr;
//	currentTransitionId_ = nullptr;
//
//	isSynchronized_ = false;
//	hasJoined_ = false;
//	isInitialized_ = false;
//
//	clientPort_ = clientPort;
//}
//
//UdpParticipant::~UdpParticipant()
//{
//	if (isInitialized_ == true)
//	{
//		SDLNet_FreePacket(sdlPacket_);
//	}
//}
//
//void UdpParticipant::initialize(std::string ip)
//{
//	if (isInitialized_ == false)
//	{
//		ipAddress_ = ip;
//
//		IPaddress ipAddress;
//		SDLNet_ResolveHost(&ipAddress, ipAddress_.c_str(), clientPort_);
//
//		sdlPacket_ = SDLNet_AllocPacket(UdpPacket::MAX_PACKET_SIZE);
//
//		if (sdlPacket_ == nullptr)
//		{
//			std::cout << "SDLNet_AllocPacket failed : " << SDLNet_GetError() << std::endl;
//			return;
//		}
//
//		sdlPacket_->address.host = ipAddress.host;
//		sdlPacket_->address.port = ipAddress.port;
//
//		std::stringstream ss;
//
//		ss << "IP=" << ((sdlPacket_->address.host & 0xFF000000) >> 24) << "."
//			<< ((sdlPacket_->address.host & 0x00FF0000) >> 16) << "."
//			<< ((sdlPacket_->address.host & 0x0000FF00) >> 8) << "."
//			<< (sdlPacket_->address.host & 0x000000FF);
//
//		std::string s = ss.str();
//
//		std::cout << "Allocating participant packet destined for " << s << std::endl;
//
//		isInitialized_ = true;
//	}
//}
//
//std::string	UdpParticipant::getIpAddress()
//{
//	return ipAddress_;
//}
//
//BufferWrapperPtr UdpParticipant::getEntityDataBuffer()
//{
//	return bufferWrapper_;
//}
//
//bool UdpParticipant::getIsSynchronized()
//{
//	return isSynchronized_;
//}
//
//bool UdpParticipant::getHasJoined()
//{
//	return hasJoined_;
//}
//
//void UdpParticipant::setHasJoined(bool value)
//{
//	hasJoined_ = value;
//}
//
//RoomId	UdpParticipant::getCurrentRoomId()
//{
//	return currentRoomId_;
//}
//
//TransitionId UdpParticipant::getCurrentTransitionId()
//{
//	if (currentTransitionId_ != nullptr)
//	{
//		return (*currentTransitionId_);
//	}
//
//	return ids_.TRANSITION_NULL;
//}
//
//double UdpParticipant::getCurrentTransitionTime()
//{
//	if (associatedEntityDelayTime_ != nullptr && associatedEntityDelayTimer_ != nullptr)
//	{
//		double delayTime = *associatedEntityDelayTime_;
//		double delayTimer = *associatedEntityDelayTimer_;
//
//		double transitionTime = delayTime - delayTimer;
//
//		//if (transitionTime < 0)
//		//{
//		//	std::cout<<"transitionTime ="<<transitionTime<<" delayTime = "<<delayTime<<" delayTimer = "<<delayTimer<<std::endl;
//		//}
//
//		return transitionTime;
//	}
//	
//	return 0.0;
//}
//
//void UdpParticipant::setCurrentRoomId(RoomId currentRoomId)
//{
//	currentRoomId_ = currentRoomId;
//}
//
//void UdpParticipant::setCurrentTransitionId(TransitionId* currentTransitionId)
//{
//	currentTransitionId_ = currentTransitionId;
//}
//
//void UdpParticipant::setEntityTypeId(EntityTypeId entityTypeId)
//{
//	entityTypeId_ = entityTypeId;
//}
//
//EntityTypeId UdpParticipant::getEntityTypeId()
//{
//	return entityTypeId_;
//}
//
//InputChannel UdpParticipant::getInputChannel()
//{
//	return inputChannel_;
//}
//
//void UdpParticipant::setInputChannel(InputChannel inputChannel)
//{
//	inputChannel_ = inputChannel;
//}
//
//SpawnPointId UdpParticipant::getInitialSpawnPointId()
//{
//	return spawnPointId_;
//}
//
//void UdpParticipant::getInitialSpawnPointId(SpawnPointId spawnPointId)
//{
//	spawnPointId_ = spawnPointId;
//}
//
//CameraController* UdpParticipant::getCamera()
//{
//	return camera_;
//}
//
//void UdpParticipant::setCamera(CameraController* camera)
//{
//	camera_ = camera;
//}
//
//
//void UdpParticipant::incrementIdleTimer(double time)
//{
//	idleTimer_ += time;
//}
//
//double UdpParticipant::getIdleTime()
//{
//	return idleTimer_;
//}
//
//int UdpParticipant::getAssociatedEntityInstanceId()
//{
//	return associatedEntityInstanceId_;
//}
//
//void UdpParticipant::setAssociatedEntityInstanceId(int associatedEntityInstanceId)
//{
//	associatedEntityInstanceId_ = associatedEntityInstanceId;
//}
//
//void UdpParticipant::setAssociatedEntityInstanceDelayTimer(double* delayTimer)
//{
//	associatedEntityDelayTimer_ = delayTimer;
//}
//
//void UdpParticipant::setAssociatedEntityInstanceDelayTime(double* delayTime)
//{
//	associatedEntityDelayTime_ = delayTime;
//}