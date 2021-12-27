#include "..\..\Headers\EngineCore\CameraScript.hpp"

using namespace firemelon;
using namespace boost::python;

CameraScript::CameraScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

CameraScript::~CameraScript()
{
}

void CameraScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyCentered_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		std::cout << "Error deleting camera " + getPythonInstanceWrapper()->getInstanceVariableName() + " Type: " + getPythonInstanceWrapper()->getInstanceTypeName() << std::endl;
		debugger_->handlePythonError();
	}
}

void CameraScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		// Get the instance for this object.
		PyObj pyInstance = getPythonInstanceWrapper()->getPyInstance();
		PyObj pyInstanceNamespace = getPythonInstanceWrapper()->getPyInstanceNamespace();

		// Store the functions as python objects.
		pyCentered_ = pyInstance.attr("centered");

		// Set the pointers to the c++ firemelon objects
		pyInstanceNamespace["controller"] = ptr(controller_.get());
	}
	catch (error_already_set &)
	{
		std::cout << "Error loading camera " + getPythonInstanceWrapper()->getInstanceTypeName() << ": " << getPythonInstanceWrapper()->getInstanceVariableName() << std::endl;
		debugger_->handlePythonError();
	}
}

void CameraScript::centered()
{
	PythonAcquireGil lock;

	try
	{
		pyCentered_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}