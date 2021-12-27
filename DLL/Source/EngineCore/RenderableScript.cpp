#include "..\..\Headers\EngineCore\RenderableScript.hpp"

using namespace firemelon;
using namespace boost::python;

RenderableScript::RenderableScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

RenderableScript::~RenderableScript()
{
}

void RenderableScript::frameTriggered(TriggerSignalId frameTriggerSignal)
{
	PythonAcquireGil lock;

	try
	{
		pyFrameTriggered_(frameTriggerSignal);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}


void RenderableScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper = getPythonInstanceWrapper();

		PyObj pyInstance = pythonInstanceWrapper->getPyInstance();

		PyObj pyInstanceNamespace = pythonInstanceWrapper->getPyInstanceNamespace();

		pyFrameTriggered_ = pyInstance.attr("frameTriggered");

		pyRendered_ = pyInstance.attr("rendered");

		if (stageController_ != nullptr)
		{
			pyInstanceNamespace["stageController"] = ptr(stageController_.get());
		}
		else
		{
			bool debug = true;
			// No-op.
		}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void RenderableScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyFrameTriggered_ = boost::python::object();
		pyRendered_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void RenderableScript::rendered(int x, int y)
{
	PythonAcquireGil lock;

	try
	{
		pyRendered_(x, y);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}
