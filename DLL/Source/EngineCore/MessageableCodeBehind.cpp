#include "..\..\Headers\EngineCore\MessageableCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

MessageableCodeBehind::MessageableCodeBehind()
{
}

MessageableCodeBehind::~MessageableCodeBehind()
{
}

void MessageableCodeBehind::messageReceived(Message message)
{
	messageableScript_->messageReceived(message);
}

void MessageableCodeBehind::preInitialize()
{
	messageableScript_ = boost::shared_ptr<MessageableScript>(new MessageableScript(debugger_));

	messageableScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		messageableScript_->preInitialize();
	}

	initialize();
}

void MessageableCodeBehind::preCleanup()
{
	cleanup();

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		messageableScript_->preCleanup();
	}
}

void MessageableCodeBehind::cleanup()
{
}