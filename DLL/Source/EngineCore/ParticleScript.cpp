#include "..\..\Headers\EngineCore\ParticleScript.hpp"

using namespace firemelon;
using namespace boost::python;

ParticleScript::ParticleScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

ParticleScript::~ParticleScript()
{
}

void ParticleScript::cleanup()
{
	PythonAcquireGil lock;
	
	try
	{
		pyDeactivated_ = boost::python::object();
		pyEmitted_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		std::cout << "Error deleting particle " << getPythonInstanceWrapper()->getInstanceVariableName() << " Type: " << getPythonInstanceWrapper()->getInstanceTypeName() << std::endl;

		debugger_->handlePythonError();
	}		
}

void ParticleScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		// Get the instance for this object.
		PyObj pyInstance = getPythonInstanceWrapper()->getPyInstance();

		PyObj pyInstanceNamespace = getPythonInstanceWrapper()->getPyInstanceNamespace();

		// Store the functions as python objects.
		pyDeactivated_ = pyInstance.attr("deactivated");

		pyEmitted_ = pyInstance.attr("emitted");

		// Set the pointers to the c++ firemelon objects
		pyInstanceNamespace["controller"] = ptr(controller_.get());
		pyInstanceNamespace["metadata"] = ptr(particleMetadata_.get());
	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading entity " + getPythonInstanceWrapper()->getInstanceTypeName() << ": " << getPythonInstanceWrapper ()->getInstanceVariableName() <<std::endl;

		debugger_->handlePythonError();
	}
}

void ParticleScript::deactivated()
{
	PythonAcquireGil lock;

	try
	{
		pyDeactivated_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void ParticleScript::emitted()
{
	PythonAcquireGil lock;

	try
	{
		pyEmitted_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}