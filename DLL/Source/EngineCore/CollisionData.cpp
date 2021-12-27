#include "..\..\Headers\EngineCore\CollisionData.hpp"

using namespace firemelon;

CollisionData::CollisionData()
{
	myHitbox_ = boost::make_shared<Hitbox>(Hitbox(0, 0, 0, 0));
	
	collidingHitbox_ = boost::make_shared<Hitbox>(Hitbox(0, 0, 0, 0));

	solidCollision_ = false;

	faceNormal_ = boost::make_shared<Vec2<int>>(Vec2<int>(0, 0));

	intrusion_ = boost::make_shared<Vec2<int>>(Vec2<int>(0, 0));
	
	Debugger::collisionDataCount++;
}

CollisionData::~CollisionData()
{
	Debugger::collisionDataCount--;
}

Vec2IPtr CollisionData::getIntrusionPy()
{
	PythonReleaseGil unlocker;

	return getIntrusion();
}

Vec2IPtr CollisionData::getIntrusion()
{
	return intrusion_;
}

Vec2IPtr CollisionData::getFaceNormalPy()
{
	PythonReleaseGil unlocker;

	return getFaceNormal();
}

Vec2IPtr CollisionData::getFaceNormal()
{
	return faceNormal_;
}

EntityTypeId CollisionData::getCollidingEntityTypePy()
{
	PythonReleaseGil unlocker;

	return getCollidingEntityType();
}

EntityTypeId CollisionData::getCollidingEntityType()
{
	return collidingEntityType_;
}

void CollisionData::setCollidingEntityTypePy(EntityTypeId value)
{
	PythonReleaseGil unlocker;

	setCollidingEntityType(value);
}

void CollisionData::setCollidingEntityType(EntityTypeId value)
{
	collidingEntityType_ = value;
}

void CollisionData::setValuePy(std::string name, std::string value)
{
	PythonReleaseGil unlocker;

	setValue(name, value);
}

void CollisionData::setValue(std::string name, std::string value)
{
	values_[name] = value;
}

std::string CollisionData::getValuePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getValue(name);
}

std::string CollisionData::getValue(std::string name)
{
	return values_[name];
}

void CollisionData::setCollidingEntityControllerPy(EntityControllerPtr value)
{
	PythonReleaseGil unlocker;

	setCollidingEntityController(value);
}

void CollisionData::setCollidingEntityController(EntityControllerPtr value)
{
	collidingEntityController_ = value;
}

EntityControllerPtr CollisionData::getCollidingEntityControllerPy()
{
	PythonReleaseGil unlocker;

	return getCollidingEntityController();
}

EntityControllerPtr CollisionData::getCollidingEntityController()
{
	return collidingEntityController_;
}

//void CollisionData::setCollidingEntityStateMachineControllerPy(boost::shared_ptr<StateMachineController> value)
//{
//	PythonReleaseGil unlocker;
//
//	setCollidingEntityStateMachineController(value);
//}

void CollisionData::setCollidingEntityStateMachineController(boost::shared_ptr<StateMachineController> value)
{
	collidingEntityStateMachineController_ = value;
}

void CollisionData::setCollidingEntityDynamicsController(DynamicsController* value)
{
	collidingEntityDynamicsController_ = value;
}

boost::shared_ptr<StateMachineController> CollisionData::getCollidingEntityStateMachineControllerPy()
{
	PythonReleaseGil unlocker;

	return getCollidingEntityStateMachineController();
}

boost::shared_ptr<StateMachineController> CollisionData::getCollidingEntityStateMachineController()
{
	return collidingEntityStateMachineController_;
}

DynamicsController* CollisionData::getCollidingEntityDynamicsControllerPy()
{
	PythonReleaseGil unlocker;

	return getCollidingEntityDynamicsController();
}

DynamicsController* CollisionData::getCollidingEntityDynamicsController()
{
	return collidingEntityDynamicsController_;
}

boost::shared_ptr<Hitbox> CollisionData::getMyHitboxPy()
{
	PythonReleaseGil unlocker;

	return getMyHitbox();
}

boost::shared_ptr<Hitbox> CollisionData::getMyHitbox()
{
	return myHitbox_;
}

boost::shared_ptr<Hitbox> CollisionData::getCollidingHitboxPy()
{
	PythonReleaseGil unlocker;

	return getCollidingHitbox();
}

boost::shared_ptr<Hitbox> CollisionData::getCollidingHitbox()
{
	return collidingHitbox_;
}

void CollisionData::setSolidCollision(bool value)
{
	solidCollision_ = value;
}

bool CollisionData::getSolidCollisionPy()
{
	PythonReleaseGil unlocker;

	return getSolidCollision();
}

bool CollisionData::getSolidCollision()
{
	return solidCollision_;
}

boost::python::object CollisionData::getPythonInstance()
{
	return pyCollisionData_;
}