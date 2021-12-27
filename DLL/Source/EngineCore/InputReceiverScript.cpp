#include "..\..\Headers\EngineCore\InputReceiverScript.hpp"

using namespace firemelon;
using namespace boost::python;

InputReceiverScript::InputReceiverScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

InputReceiverScript::~InputReceiverScript()
{
}

void InputReceiverScript::buttonDown(GameButtonId buttonCode)
{
	PythonAcquireGil lock;

	try
	{
		pyButtonDown_(buttonCode);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void InputReceiverScript::buttonUp(GameButtonId buttonCode)
{
	PythonAcquireGil lock;

	try
	{
		pyButtonUp_(buttonCode);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void InputReceiverScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		PyObj pyInstance = getPythonInstanceWrapper()->getPyInstance();
		PyObj pyInstanceNamespace = getPythonInstanceWrapper()->getPyInstanceNamespace();

		pyButtonDown_ = pyInstance.attr("buttonDown");
		pyButtonUp_ = pyInstance.attr("buttonUp");

		pyInstanceNamespace["inputChannel"] = inputChannel_;
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void InputReceiverScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyButtonDown_ = boost::python::object();
		pyButtonUp_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void InputReceiverScript::setInputChannel(InputChannel inputChannel)
{
	PythonAcquireGil lock;

	inputChannel_ = inputChannel;

	try
	{
		PyObj pyInstanceNamespace = getPythonInstanceWrapper()->getPyInstanceNamespace();

		pyInstanceNamespace["inputChannel"] = inputChannel_;
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}