#include "..\..\Headers\EngineCore\MessageableScript.hpp"

using namespace firemelon;
using namespace boost::python;

MessageableScript::MessageableScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

MessageableScript::~MessageableScript()
{
}

void MessageableScript::messageReceived(Message message)
{
	PythonAcquireGil lock;

	try
	{
		pyMessageReceived_(message);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void MessageableScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		PyObj pyInstance = getPythonInstanceWrapper()->getPyInstance();
		PyObj pyInstanceNamespace = getPythonInstanceWrapper()->getPyInstanceNamespace();

		pyMessageReceived_ = pyInstance.attr("messageReceived");
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void MessageableScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyMessageReceived_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}
