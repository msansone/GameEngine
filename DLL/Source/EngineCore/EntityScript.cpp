#include "..\..\Headers\EngineCore\EntityScript.hpp"

using namespace firemelon;
using namespace boost::python;

EntityScript::EntityScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

EntityScript::~EntityScript()
{
}

void EntityScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyCreated_ = boost::python::object();
		pyDestroyed_ = boost::python::object();
		pyRoomEntered_ = boost::python::object();
		pyRoomExited_ = boost::python::object();
		pyUpdate_ = boost::python::object();	
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void EntityScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		// Get the instance for this object.
		boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper = getPythonInstanceWrapper();
		PyObj pyInstance = pythonInstanceWrapper->getPyInstance();
		PyObj pyInstanceNamespace = pythonInstanceWrapper->getPyInstanceNamespace();

		// Store the functions as python objects.
		pyCreated_ = pyInstance.attr("created");
		pyRoomEntered_ = pyInstance.attr("roomEntered");
		pyRoomExited_ = pyInstance.attr("roomExited");
		pyDestroyed_ = pyInstance.attr("destroyed");
		pyUpdate_ = pyInstance.attr("update");
		

		// Set the entity type value, and defined the entity type names
		pyInstance.attr("metadata") = ptr(metadata_.get());
		pyInstance.attr("roomId") = metadata_->getRoomMetadata()->getRoomId();

		// This is obsolete, it can be accessed from the metadata. After doing a little testing to make sure it's not needed, delete this line.
		//pyInstanceNamespace["entityName"] = metadata_->name_;

		pyInstanceNamespace["controller"] = ptr(controller_.get());
		pyInstanceNamespace["position"] = ptr(position_.get());


		//// If this is an actor or HUD element.
		//if (stageControllerHolder_->getHasStageController() == true)
		//{
		//	pyInstanceNamespace["stageController"] = ptr(stageController_.get());
		//}
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void EntityScript::destroyed()
{
	PythonAcquireGil lock;

	try
	{
		pyDestroyed_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void EntityScript::initializeBegin()
{
	PythonAcquireGil lock;

	try
	{
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void EntityScript::created()
{
	PythonAcquireGil lock;

	try
	{
		PyObj pyInstanceNamespace = getPythonInstanceWrapper()->getPyInstanceNamespace();

		dict properties = extract<dict>(pyInstanceNamespace["_properties"]);

		// Copy the properties to the python object.
		stringmap::iterator itr;

		for (itr = entityProperties_.begin(); itr != entityProperties_.end(); itr++)
		{
			properties[itr->first] = itr->second;
		}

		pyCreated_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void EntityScript::roomEntered(RoomId roomId)
{
	PythonAcquireGil lock;

	try
	{
		pyRoomEntered_(roomId);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void EntityScript::roomExited(RoomId roomId)
{
	PythonAcquireGil lock;

	try
	{
		pyRoomExited_(roomId);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void EntityScript::update(double time)
{
	PythonAcquireGil lock;

	try
	{
		pyUpdate_(time);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}
