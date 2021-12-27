#include "..\..\Headers\EngineCore\EntityCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

EntityCodeBehind::EntityCodeBehind()
{
	isRemoved_ = false;
	isInitialized_ = false;
}

EntityCodeBehind::~EntityCodeBehind()
{
	bool debug = true;
}

bool EntityCodeBehind::getIsRemoved()
{
	return isRemoved_;
}

void EntityCodeBehind::setIsRemoved(bool value)
{
	isRemoved_ = value;
}

void EntityCodeBehind::preInitialize()
{
	if (isInitialized_ == false)
	{
		entityScript_ = boost::shared_ptr<EntityScript>(new EntityScript(debugger_));

		entityScript_->controller_ = getEntityController();
		entityScript_->metadata_ = getMetadata();
		entityScript_->position_ = position_;

		entityScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

		if (classification_ != ENTITY_CLASSIFICATION_TILE)
		{
			entityScript_->preInitialize();
		}	

		baseInitialize();

		initialize();

		isInitialized_ = true;
	}
}

void EntityCodeBehind::initialize()
{

}

void EntityCodeBehind::initializeBegin()
{
	entityScript_->initializeBegin();
}

void EntityCodeBehind::created()
{
	entityScript_->created();
}

void EntityCodeBehind::preCleanup()
{
	cleanup();

	baseCleanup();

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		entityScript_->preCleanup();
	}
}

void EntityCodeBehind::cleanup()
{
}

void EntityCodeBehind::preDestroyed()
{
	destroyed();
}

void EntityCodeBehind::destroyed()
{
	entityScript_->destroyed();
}

void EntityCodeBehind::preRoomEntered(RoomId roomId)
{
	try
	{
		PythonAcquireGil lock;

		PyObj pyInstance = entityScript_->getPythonInstanceWrapper()->getPyInstance();

		pyInstance.attr("roomId") = roomId;
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}

	roomEntered(roomId);
}

void EntityCodeBehind::roomEntered(RoomId roomId)
{
	entityScript_->roomEntered(roomId);
}

void EntityCodeBehind::roomExited(RoomId roomId)
{
	entityScript_->roomExited(roomId);
}

void EntityCodeBehind::update(double time)
{
	entityScript_->update(time);
}

bool EntityCodeBehind::baseUpdate(double time)
{
	// No-op.
	return true;
}

void EntityCodeBehind::preUpdate(double time)
{
	if (baseUpdate(time) == true)
	{
		update(time);
	}
}

void EntityCodeBehind::baseInitialize()
{
	// No-op.
}

void EntityCodeBehind::baseCleanup()
{
	// No-op.
}
