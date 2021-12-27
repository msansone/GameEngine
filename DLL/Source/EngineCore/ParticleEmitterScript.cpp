#include "..\..\Headers\EngineCore\ParticleEmitterScript.hpp"

using namespace firemelon;
using namespace boost::python;

ParticleEmitterScript::ParticleEmitterScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

ParticleEmitterScript::~ParticleEmitterScript()
{
}

void ParticleEmitterScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyParticleEmitted_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		std::cout << "Error deleting particle emitter " + getPythonInstanceWrapper()->getInstanceVariableName() + " Type: " + getPythonInstanceWrapper()->getInstanceTypeName() << std::endl;
		debugger_->handlePythonError();
	}
}

void ParticleEmitterScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		// Get the instance for this object.
		PyObj pyInstance = getPythonInstanceWrapper()->getPyInstance();
		PyObj pyInstanceNamespace = getPythonInstanceWrapper()->getPyInstanceNamespace();
	
		// Store the functions as python objects.
		pyParticleEmitted_ = pyInstance.attr("particleEmitted");
	
		// Set the pointers to the c++ firemelon objects
		pyInstanceNamespace["controller"] = ptr(controller_.get());
		//pyInstanceNamespace_["particleEmitterId"] = particleEmitterId_;
	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading particle emitter " + getPythonInstanceWrapper()->getInstanceTypeName() <<": "<< getPythonInstanceWrapper()->getInstanceVariableName() << std::endl;
		debugger_->handlePythonError();
	}
}

void ParticleEmitterScript::particleEmitted(boost::shared_ptr<ParticleEntityCodeBehind> p)
{
	PythonAcquireGil lock;

	try
	{
		//pyParticleEmitted_(ptr(p.get()));
		pyParticleEmitted_(p->getPythonInstanceWrapper()->getPyInstance());
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}