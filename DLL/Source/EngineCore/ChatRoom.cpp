#include "..\..\Headers\EngineCore\ChatRoom.hpp"

using namespace firemelon;
using namespace boost::python;

ChatRoom::ChatRoom()
{
}

ChatRoom::~ChatRoom()
{
}

void ChatRoom::cleanup()
{
	PythonAcquireGil lock;

	int size = pyChatListeners_.size();

	for (int j = 0; j < size; j++)
	{
		pyChatListeners_[j] = boost::python::object();
	}

	pyChatListeners_.clear();
}

void ChatRoom::addChatListener(object listener)
{
	pyChatListeners_.push_back(listener);
}

void ChatRoom::addChatLine(std::string chatLine)
{
	newChatLines_.push_back(chatLine);
}

void ChatRoom::dispatchChatLines()
{
	int chatLineCount = newChatLines_.size();

	for (int i = 0; i < chatLineCount; i++)
	{
		std::string chatLine = newChatLines_[i];

		dispatchedChatLines_.push_back(chatLine);

		int size = pyChatListeners_.size();

		for (int j = 0; j < size; j++)
		{
			PythonAcquireGil lock;
			pyChatListeners_[j].attr("chatLineReceived")(chatLine);
		}
	}

	newChatLines_.clear();
}