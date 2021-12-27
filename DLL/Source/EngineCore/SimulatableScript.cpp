#include "..\..\Headers\EngineCore\SimulatableScript.hpp"

using namespace firemelon;
using namespace boost::python;

SimulatableScript::SimulatableScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

SimulatableScript::~SimulatableScript()
{	
}

PyObj SimulatableScript::getPyDynamicsController()
{
	return pyDynamicsController_;
}

void SimulatableScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper = getPythonInstanceWrapper();
		PyObj pyInstance = pythonInstanceWrapper->getPyInstance();
		PyObj pyInstanceNamespace = pythonInstanceWrapper->getPyInstanceNamespace();

		pyCreateDynamicsController_ = pyInstance.attr("createDynamicsController");

		if (dynamicsControllerHolder_->getDynamicsController() == nullptr)
		{
			pyDynamicsController_ = pyCreateDynamicsController_();
			
			DynamicsController* newDynamicsController = extract<DynamicsController*>(pyDynamicsController_.attr("getThis")());

			dynamicsControllerHolder_->setDynamicsController(newDynamicsController);

			pyInstanceNamespace["dynamicsController"] = pyDynamicsController_;
		}

		pyFrameBegin_ = pyInstance.attr("frameBegin");
		pyPreIntegration_ = pyInstance.attr("preIntegration");
		pyPostIntegration_ = pyInstance.attr("postIntegration");
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void SimulatableScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyCreateDynamicsController_ = boost::python::object();
		pyDynamicsController_ = boost::python::object();
		pyFrameBegin_ = boost::python::object();
		pyPreIntegration_ = boost::python::object();
		pyPostIntegration_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void SimulatableScript::frameBegin()
{
	PythonAcquireGil lock;
	
	try
	{
		pyFrameBegin_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void SimulatableScript::start()
{
	PythonAcquireGil lock;

	try
	{
		pyDynamicsController_.attr("initialize")();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void SimulatableScript::preIntegration()
{
	PythonAcquireGil lock;

	try
	{
		pyPreIntegration_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void SimulatableScript::postIntegration()
{
	PythonAcquireGil lock;

	try
	{
		pyPostIntegration_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}