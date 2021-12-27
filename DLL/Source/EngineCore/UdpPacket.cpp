//#include "..\..\Headers\EngineCore\UdpPacket.hpp"
//
//using namespace firemelon;
//using namespace boost;
//
//unsigned int UdpPacket::seqCounter_ = 0;
//
//UdpPacket::UdpPacket() :
//	bodyLength_(0)
//{
//	sequence_ = 0;
//	ack_ = 0;
//}
//
//UdpPacket::UdpPacket(UdpPacketType packetType) :
//	bodyLength_(0)
//{
//	packetType_ = packetType;
//	sequence_ = 0;
//	ack_ = 0;
//}
//
//UdpPacket::~UdpPacket()
//{
//}
//
//const char* UdpPacket::getData() const
//{
//	return data_;
//}
//
//char* UdpPacket::getData()
//{
//	return data_;
//}
//
//size_t UdpPacket::getLength() const
//{
//	return HEADER_LENGTH + bodyLength_;
//}
//
//const char* UdpPacket::getBody() const
//{
//	return data_ + HEADER_LENGTH;
//}
//
//char* UdpPacket::getBody()
//{
//	return data_ + HEADER_LENGTH;
//}
//
//size_t UdpPacket::getBodyLength() const
//{
//	return bodyLength_;
//}
//
//void UdpPacket::setBodyLength(size_t newLength)
//{
//	bodyLength_ = newLength;
//
//	if (bodyLength_ > MAX_BODY_LENGTH)
//	{
//		bodyLength_ = MAX_BODY_LENGTH;
//	}
//}
//
//UdpPacketType UdpPacket::getPacketType()
//{
//	return packetType_;
//}
//
//unsigned int UdpPacket::getSequence()
//{
//	return sequence_;
//}
//
//void UdpPacket::setAck(unsigned int value)
//{
//	ack_ = value;
//}
//
//unsigned int UdpPacket::getAck()
//{
//	return ack_;
//}
//
//bool UdpPacket::decodeHeader()
//{
//	memcpy(data_, sdlPacket_->data, sdlPacket_->len);
//
//	char header[HEADER_LENGTH + 1] = "";
//
//	for (int i = 0; i < HEADER_LENGTH; i++)
//	{
//		header[i] = data_[i];
//	}
//
//	unsigned int protocolId = 0;
//	protocolId = ((unsigned char)(header[0]) << 24) | ((unsigned char)(header[1]) << 16) | ((unsigned char)(header[2]) << 8) | (unsigned char)(header[3]);
//
//	if (protocolId != FIREMELON_PROTOCOL_ID)
//	{
//		return false;
//	}
//	
//	sequence_ = ((unsigned char)(header[4]) << 24) | ((unsigned char)(header[5]) << 16) | ((unsigned char)(header[6]) << 8) | (unsigned char)(header[7]);
//	
//	ack_ = ((unsigned char)(header[8]) << 24) | ((unsigned char)(header[9]) << 16) | ((unsigned char)(header[10]) << 8) | (unsigned char)(header[11]);
//
//	packetType_ = (UdpPacketType)header[12];
//
//	bodyLength_ = std::atoi(header + (HEADER_LENGTH - HEADER_BODYSIZE_LENGTH));
//	
//	if (bodyLength_ > MAX_BODY_LENGTH)
//	{
//		bodyLength_ = 0;
//		return false;
//	}
//
//	receivedFromIpAddress = getPacketIpString();
//
//	return true;
//}
//
//std::string UdpPacket::getPacketIpString()
//{
//	std::stringstream ss;
//
//	int host = SDLNet_Read32(&(sdlPacket_->address.host));
//
//	ss << ((host & 0xFF000000) >> 24) << "." << ((host & 0x00FF0000) >> 16) << "." << ((host & 0x0000FF00) >> 8) << "." << (host & 0x000000FF);
//
//	std::string ip = ss.str();
//	
//	return ip;
//}
//
//void UdpPacket::encodeHeader()
//{
//	seqCounter_++;
//	sequence_ = seqCounter_;
//
//	char protocolIdBytes[4];
//	unsigned int protocolId = FIREMELON_PROTOCOL_ID;
//
//	protocolIdBytes[0] = (unsigned char) ((protocolId >> 24) & 0xFF);
//	protocolIdBytes[1] = (unsigned char) ((protocolId >> 16) & 0xFF);
//	protocolIdBytes[2] = (unsigned char) ((protocolId >> 8) & 0xFF);
//	protocolIdBytes[3] = (unsigned char) (protocolId & 0xFF);
//	
//	char sequenceBytes[4];
//
//	sequenceBytes[0] = (unsigned char) ((sequence_ >> 24) & 0xFF);
//	sequenceBytes[1] = (unsigned char) ((sequence_ >> 16) & 0xFF);
//	sequenceBytes[2] = (unsigned char) ((sequence_ >> 8) & 0xFF);
//	sequenceBytes[3] = (unsigned char) (sequence_ & 0xFF);
//	
//	char ackBytes[4];
//
//	ackBytes[0] = (unsigned char) ((ack_ >> 24) & 0xFF);
//	ackBytes[1] = (unsigned char) ((ack_ >> 16) & 0xFF);
//	ackBytes[2] = (unsigned char) ((ack_ >> 8) & 0xFF);
//	ackBytes[3] = (unsigned char) (ack_ & 0xFF);
//
//
//	data_[0] = protocolIdBytes[0];
//	data_[1] = protocolIdBytes[1];
//	data_[2] = protocolIdBytes[2];
//	data_[3] = protocolIdBytes[3];
//	
//	data_[4] = sequenceBytes[0];
//	data_[5] = sequenceBytes[1];
//	data_[6] = sequenceBytes[2];
//	data_[7] = sequenceBytes[3];
//	
//	data_[8] = ackBytes[0];
//	data_[9] = ackBytes[1];
//	data_[10] = ackBytes[2];
//	data_[11] = ackBytes[3];
//
//	data_[12] = (char)packetType_ & 0xFF;
//
//	char bodySizeLength[HEADER_BODYSIZE_LENGTH + 1] = "";
//
//	sprintf_s(bodySizeLength, "%3d", bodyLength_);
//	
//	memcpy(data_ + (HEADER_LENGTH - HEADER_BODYSIZE_LENGTH), bodySizeLength, HEADER_BODYSIZE_LENGTH);
//
//	// Copy to the SDL packet.
//	memcpy(sdlPacket_->data, data_, getLength());
//
//	sdlPacket_->len = getLength();
//
//}
