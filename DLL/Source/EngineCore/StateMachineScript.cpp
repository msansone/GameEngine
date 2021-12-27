#include "..\..\Headers\EngineCore\StateMachineScript.hpp"

using namespace firemelon;
using namespace boost::python;

StateMachineScript::StateMachineScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

StateMachineScript::~StateMachineScript()
{
}

void StateMachineScript::stateChanged(int oldStateIndex, int newStateIndex)
{
	PythonAcquireGil lock;

	try
	{
  		pyStateChanged_(oldStateIndex, newStateIndex);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void StateMachineScript::stateEnded(int stateIndex)
{
	PythonAcquireGil lock;

	try
	{
		pyStateEnded_(stateIndex);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void StateMachineScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		PyObj pyInstance = getPythonInstanceWrapper()->getPyInstance();
		PyObj pyInstanceNamespace = getPythonInstanceWrapper()->getPyInstanceNamespace();
		
		pyStateChanged_ = pyInstance.attr("stateChanged");
		pyStateEnded_ = pyInstance.attr("stateEnded");

		pyInstanceNamespace["stateMachineController"] = ptr(stateMachineController_.get());
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void StateMachineScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{	
		pyStateChanged_ = boost::python::object();
		pyStateEnded_ = boost::python::object();	
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}