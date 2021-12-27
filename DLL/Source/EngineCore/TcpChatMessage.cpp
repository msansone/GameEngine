#include "..\..\Headers\EngineCore\TcpChatMessage.hpp"

using namespace firemelon;
using namespace boost;
using namespace boost::asio;
using namespace boost::asio::ip;

TcpChatMessage::TcpChatMessage() :
	bodyLength_(0)
{
}

TcpChatMessage::~TcpChatMessage()
{
}

const char* TcpChatMessage::getData() const
{
	return data_;
}

char* TcpChatMessage::getData()
{
	return data_;
}

size_t TcpChatMessage::getLength() const
{
	return HEADER_LENGTH + bodyLength_;
}

const char* TcpChatMessage::getBody() const
{
	return data_ + HEADER_LENGTH;
}

char* TcpChatMessage::getBody()
{
	return data_ + HEADER_LENGTH;
}

size_t TcpChatMessage::getBodyLength() const
{
	return bodyLength_;
}

void TcpChatMessage::setBodyLength(size_t newLength)
{
	bodyLength_ = newLength;

	if (bodyLength_ > MAX_BODY_LENGTH)
	{
		bodyLength_ = MAX_BODY_LENGTH;
	}
}

bool TcpChatMessage::decodeHeader()
{
	char header[HEADER_LENGTH + 1] = "";

	strncat_s(header, data_, HEADER_LENGTH);

	bodyLength_ = std::atoi(header);
	
	if (bodyLength_ > MAX_BODY_LENGTH)
	{
		bodyLength_ = 0;
		return false;
	}

	return true;
}

void TcpChatMessage::encodeHeader()
{
	char header[HEADER_LENGTH + 1] = "";

	sprintf_s(header, "%4d", bodyLength_);

	memcpy(data_, header, HEADER_LENGTH);
}
