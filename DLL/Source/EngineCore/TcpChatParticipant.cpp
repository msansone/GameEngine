#include "..\..\Headers\EngineCore\TcpChatParticipant.hpp"

using namespace firemelon;
using namespace boost;
using namespace boost::asio;
using namespace boost::asio::ip;

int TcpChatParticipant::participantIdGenerator_ = 0;

TcpChatParticipant::TcpChatParticipant()
{
	participantId_ = participantIdGenerator_;
	participantIdGenerator_++;
}

TcpChatParticipant::~TcpChatParticipant()
{
}

int TcpChatParticipant::getId()
{
	return participantId_;
}
