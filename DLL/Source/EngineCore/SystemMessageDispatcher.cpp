#include "..\..\Headers\EngineCore\SystemMessageDispatcher.hpp"

using namespace firemelon;
using namespace boost::python;

SystemMessageDispatcher::SystemMessageDispatcher()
{
}

SystemMessageDispatcher::~SystemMessageDispatcher()
{
}
	

void SystemMessageDispatcher::cleanup()
{
	PythonAcquireGil lock;

	int size = pyListeners_.size();

	for (int j = 0; j < size; j++)
	{
		pyListeners_[j] = boost::python::object();
	}

	pyListeners_.clear();
}

void SystemMessageDispatcher::addListener(object listener)
{
	pyListeners_.push_back(listener);
}

void SystemMessageDispatcher::addSystemMessage(std::string systemMessage)
{
	newMessages_.push_back(systemMessage);
}

void SystemMessageDispatcher::dispatch()
{
	int chatLineCount = newMessages_.size();

	for (int i = 0; i < chatLineCount; i++)
	{
		std::string systemMessage = newMessages_[i];

		dispatchedMessages_.push_back(systemMessage);

		int size = pyListeners_.size();

		for (int j = 0; j < size; j++)
		{
			PythonAcquireGil lock;
			pyListeners_[j].attr("systemMessageReceived")(systemMessage);
		}
	}

	newMessages_.clear();
}