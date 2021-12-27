#include "..\..\Headers\EngineCore\TcpChatRoom.hpp"

using namespace firemelon;
using namespace boost;
using namespace boost::asio;
using namespace boost::asio::ip;

TcpChatRoom::TcpChatRoom()
{
}

TcpChatRoom::~TcpChatRoom()
{
}

void TcpChatRoom::joinChat(TcpChatParticipant* chatParticipant)
{
	chatParticipants_.push_back(chatParticipant);
}

void TcpChatRoom::leaveChat(TcpChatParticipant* chatParticipant)
{
	int size = chatParticipants_.size();

	for (int i = 0; i < size; i++)
	{
		if (chatParticipants_[i]->getId() == chatParticipant->getId())
		{
			chatParticipants_.erase(chatParticipants_.begin() + i);
			break;
		}
	}
}

void  TcpChatRoom::deliverMessage(const TcpChatMessage& message)
{
	int size = chatParticipants_.size();

	for (int i = 0; i < size; i++)
	{
		chatParticipants_[i]->writeMessage(message);
	}
}

void  TcpChatRoom::shutdown()
{
	int size = chatParticipants_.size();

	for (int i = 0; i < size; i++)
	{
		leaveChat(chatParticipants_[i]);
	}
}