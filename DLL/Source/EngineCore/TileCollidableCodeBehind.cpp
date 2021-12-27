#include "..\..\Headers\EngineCore\TileCollidableCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

TileCollidableCodeBehind::TileCollidableCodeBehind()
{
	tileGroupId_ = 0;
}

TileCollidableCodeBehind::~TileCollidableCodeBehind()
{
	cleanup();
}

void TileCollidableCodeBehind::collision(boost::shared_ptr<CollisionData> collisionData)
{
}

void TileCollidableCodeBehind::collisionEnter(boost::shared_ptr<CollisionData> collisionData)
{
}

void TileCollidableCodeBehind::collisionExit(boost::shared_ptr<CollisionData> collisionData)
{
}

boost::shared_ptr<CollisionData> TileCollidableCodeBehind::getCollisionData(int hitboxIndex)
{
	return tileCollisionData_;
}

boost::shared_ptr<CollisionData> TileCollidableCodeBehind::createCollisionData()
{
	return tileCollisionData_;
}

void TileCollidableCodeBehind::cleanup()
{
	PythonAcquireGil lock;

	std::string sCode = "collisionData = None";

	str pyCode(sCode);

	try
	{
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		object pyFiremelonNamespace = pyFiremelonModule.attr("__dict__");

		boost::python::object obj = boost::python::exec(pyCode, pyFiremelonNamespace);

		pyCollisionData_ = boost::python::object();
		tileCollisionData_->pyCollisionData_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void TileCollidableCodeBehind::initialize()
{
	PythonAcquireGil lock;

	std::string sCode = "collisionData = TileCollisionData()";

	str pyCode(sCode);

	try
	{
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));

		object pyFiremelonNamespace = pyFiremelonModule.attr("__dict__");

		boost::python::object obj = boost::python::exec(pyCode, pyFiremelonNamespace);

		// Get the instance for this object.
		pyCollisionData_ = extract<object>(pyFiremelonNamespace["collisionData"]);

		boost::shared_ptr<TileCollisionData>& tileCollisionData = extract<boost::shared_ptr<TileCollisionData>&>(pyCollisionData_);
		
		tileCollisionData->pyCollisionData_ = pyCollisionData_;

		tileCollisionData_ = tileCollisionData;

		pyCollisionData_.attr("tileGroupId") = tileGroupId_;
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

CollisionResolution TileCollidableCodeBehind::resolveCollision(boost::shared_ptr<CollisionData> collisionData)
{
	return COLLISION_RESOLUTION_SOLID_PRIORITY;
}

void TileCollidableCodeBehind::setTileGroupId(unsigned int tileGroupId)
{
	tileGroupId_ = tileGroupId;

	try
	{
		PythonAcquireGil lock;

		pyCollisionData_.attr("tileGroupId") = tileGroupId_;
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}
