#include "..\..\Headers\EngineCore\CollidableScript.hpp"

using namespace firemelon;
using namespace boost::python;

CollidableScript::CollidableScript(DebuggerPtr debugger) : CodeBehindScript(debugger)
{
}

CollidableScript::~CollidableScript()
{
}

void CollidableScript::collision(boost::shared_ptr<CollisionData> collision)
{
	PythonAcquireGil lock;

	try
	{
		pyCollision_(collision);
	}
	catch (error_already_set &)
	{
		std::cout << "Error handling collision in entity " << getPythonInstanceWrapper()->getInstanceTypeName() << std::endl;
		debugger_->handlePythonError();
	}
}

void CollidableScript::collisionEnter(boost::shared_ptr<CollisionData> collision)
{
	PythonAcquireGil lock;

	try
	{
		pyCollisionEnter_(collision);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void CollidableScript::collisionExit(boost::shared_ptr<CollisionData> collision)
{
	PythonAcquireGil lock;

	try
	{
		pyCollisionExit_(collision);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

CollisionResolution	CollidableScript::resolveCollision(boost::shared_ptr<CollisionData> collision)
{
	PythonAcquireGil lock;

	CollisionResolution resolution = COLLISION_RESOLUTION_PERMEABLE;

	try
	{
		PyObj result = pyResolveCollision_(collision);

		resolution = extract<CollisionResolution>(result);
	}
	catch (error_already_set &)
	{
		std::cout << "Error handling collision resolution in entity " << getPythonInstanceWrapper()->getInstanceTypeName() << std::endl;
		debugger_->handlePythonError();
	}

	return resolution;
}

boost::shared_ptr<CollisionData> CollidableScript::getCollisionData(int hitboxIndex)
{
	PythonAcquireGil lock;

	boost::shared_ptr<CollisionData >collisionData;

	try
	{
		boost::shared_ptr<Hitbox> h = hitboxManager_->getHitbox(hitboxIndex);

		PyObj pyObject = pyGetCollisionData_(h);

		boost::shared_ptr<CollisionData>& collision = extract<boost::shared_ptr<CollisionData>&>(pyObject);

		collision->pyCollisionData_ = pyObject;

		collisionData = collision;
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}

	return collisionData;
}
void CollidableScript::initialize()
{
	PythonAcquireGil lock;

	try
	{
		PyObj pyInstance = getPythonInstanceWrapper()->getPyInstance();

		pyCollision_ = pyInstance.attr("collision");
		pyCollisionEnter_ = pyInstance.attr("collisionEnter");
		pyCollisionExit_ = pyInstance.attr("collisionExit");
		pyGetCollisionData_ = pyInstance.attr("getCollisionData");
		pyResolveCollision_ = pyInstance.attr("resolveCollision");
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void CollidableScript::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyCollision_ = boost::python::object();
		pyCollisionEnter_ = boost::python::object();
		pyCollisionExit_ = boost::python::object();
		pyGetCollisionData_ = boost::python::object();
		pyResolveCollision_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}