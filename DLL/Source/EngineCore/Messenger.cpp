#include "..\..\Headers\EngineCore\Messenger.hpp"

using namespace firemelon;

Messenger::Messenger()
{
}

Messenger::~Messenger()
{
}

void Messenger::sendMessagePy(Message message)
{
	PythonReleaseGil unlocker;

	sendMessage(message);
}

void Messenger::sendMessage(Message message)
{
	postMessageSignal(message);
}