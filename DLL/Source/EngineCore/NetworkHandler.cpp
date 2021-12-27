#include "..\..\Headers\EngineCore\NetworkHandler.hpp"

using namespace firemelon;
using namespace boost::python;

NetworkHandler::NetworkHandler()
{
}

NetworkHandler::~NetworkHandler()
{
	
}

void NetworkHandler::initializePythonData()
{
	PythonAcquireGil lock;

	try
	{
		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = "from ";

		std::string scriptVar = "networkHandler";

		sCode += scriptName_ + " import " + scriptTypeName_ + "\n";
		sCode += scriptVar + " = " + scriptTypeName_ + "()";
	
		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");
	
		str pyCode(sCode);	
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Get the instance for this object.
		pyNetworkHandlerInstance_ = extract<object>(pyMainNamespace_[scriptVar]);
		pyNetworkHandlerNamespace_ = pyNetworkHandlerInstance_.attr("__dict__");
	
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyNetworkHandlerNamespace_["firemelon"] = pyFiremelonModule;
		
		// Store the functions as python objects.
		pyConnectionClosedByServer_ = pyNetworkHandlerInstance_.attr("connectionClosedByServer");
		pyConnectionClosedByClient_ = pyNetworkHandlerInstance_.attr("connectionClosedByClient");
		pyConnectionOpened_ = pyNetworkHandlerInstance_.attr("connectionOpened");
		pyServerStarted_ = pyNetworkHandlerInstance_.attr("serverStarted");

		pyNetworkHandlerNamespace_["menuController"] = ptr(menuController_.get());
		pyNetworkHandlerNamespace_["engineController"] = ptr(engineController_.get());
	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading network handler " + scriptTypeName_<<std::endl;
		DebugHelper::handlePythonError();
	}

	userInitialize();
}

void NetworkHandler::connectionOpened()
{
	PythonAcquireGil lock;

	try
	{
		pyConnectionOpened_();
	}
	catch(error_already_set &)
	{
		DebugHelper::handlePythonError();
	}
}

void NetworkHandler::serverStarted()
{
	PythonAcquireGil lock;

	try
	{
		pyServerStarted_();
	}
	catch (error_already_set &)
	{
		DebugHelper::handlePythonError();
	}
}

void NetworkHandler::connectionClosedByServer()
{
	PythonAcquireGil lock;

	try
	{
		pyConnectionClosedByServer_();
	}
	catch(error_already_set &)
	{
		DebugHelper::handlePythonError();
	}
}

void NetworkHandler::connectionClosedByClient()
{
	PythonAcquireGil lock;

	try
	{
		pyConnectionClosedByClient_();
	}
	catch(error_already_set &)
	{
		DebugHelper::handlePythonError();
	}
}

void NetworkHandler::userInitialize()
{
	return;
}