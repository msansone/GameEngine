#include "..\..\Headers\EngineCore\EntityCreationParameters.hpp"

using namespace firemelon;

EntityCreationParameters::EntityCreationParameters()
{	
	BaseIds ids;

	x_ = 0;
	y_ = 0;
	w_ = 0;
	h_ = 0;
	layer_ = -1;
	renderOrder_ = 0;
	participantIndex_ = -1;
	roomId_ = ids.ROOM_NULL;
	entityTypeId_ = ids.ENTITY_NULL;
	spawnPointId_ = ids.SPAWNPOINT_NULL;
	inputChannel_ = -1;
	acceptInput_ = false;
	attachCamera_ = false;
	initialStateName_ = "";
	entityName_ = "";
	ipAddress_ = "";
}

EntityCreationParameters::~EntityCreationParameters()
{
}

int EntityCreationParameters::getXPy()
{
	PythonReleaseGil unlocker;

	return getX();
}

int EntityCreationParameters::getX()
{
	return x_;
}

void EntityCreationParameters::setXPy(int value)
{
	PythonReleaseGil unlocker;

	setX(value);
}

void EntityCreationParameters::setX(int value)
{
	x_ = value;
}
		
int EntityCreationParameters::getYPy()
{
	PythonReleaseGil unlocker;

	return getY();
}

int EntityCreationParameters::getY()
{
	return y_;
}

void EntityCreationParameters::setYPy(int value)
{
	PythonReleaseGil unlocker;

	setY(value);
}

void EntityCreationParameters::setY(int value)
{
	y_ = value;
}
		
int EntityCreationParameters::getWPy()
{
	PythonReleaseGil unlocker;

	return getW();
}

int EntityCreationParameters::getW()
{
	return w_;
}

void EntityCreationParameters::setWPy(int value)
{
	PythonReleaseGil unlocker;

	setW(value);
}

void EntityCreationParameters::setW(int value)
{
	w_ = value;
}
		
int EntityCreationParameters::getHPy()
{
	PythonReleaseGil unlocker;

	return getH();
}

int EntityCreationParameters::getH()
{
	return h_;
}

void EntityCreationParameters::setHPy(int value)
{
	PythonReleaseGil unlocker;

	setH(value);
}

void EntityCreationParameters::setH(int value)
{
	h_ = value;
}
		
int EntityCreationParameters::getLayerPy()
{
	PythonReleaseGil unlocker;

	return getLayer();
}

int EntityCreationParameters::getLayer()
{
	return layer_;
}

void EntityCreationParameters::setLayerPy(int value)
{
	PythonReleaseGil unlocker;

	setLayer(value);
}

void EntityCreationParameters::setLayer(int value)
{
	layer_ = value;
}

int EntityCreationParameters::getRenderOrderPy()
{
	PythonReleaseGil unlocker;

	return getRenderOrder();
}

int EntityCreationParameters::getRenderOrder()
{
	return renderOrder_;
}

void EntityCreationParameters::setRenderOrderPy(int value)
{
	PythonReleaseGil unlocker;

	setRenderOrder(value);
}

void EntityCreationParameters::setRenderOrder(int value)
{
	renderOrder_ = value;
}

int EntityCreationParameters::getParticipantIndex()
{
	return participantIndex_;
}

void EntityCreationParameters::setParticipantIndex(int value)
{
	participantIndex_ = value;
}
		
bool EntityCreationParameters::getAcceptInputPy()
{
	PythonReleaseGil unlocker;

	return getAcceptInput();
}

bool EntityCreationParameters::getAcceptInput()
{
	return acceptInput_;
}

void EntityCreationParameters::setAcceptInputPy(bool value)
{
	PythonReleaseGil unlocker;

	setAcceptInput(value);
}

void EntityCreationParameters::setAcceptInput(bool value)
{
	acceptInput_ = value;
}

bool EntityCreationParameters::getAttachCameraPy()
{
	PythonReleaseGil unlocker;

	return getAttachCamera();
}

bool EntityCreationParameters::getAttachCamera()
{
	return attachCamera_;
}

void EntityCreationParameters::setAttachCameraPy(bool value)
{
	PythonReleaseGil unlocker;

	setAttachCamera(value);
}

void EntityCreationParameters::setAttachCamera(bool value)
{
	attachCamera_ = value;
}
		
std::string EntityCreationParameters::getInitialStateNamePy()
{
	PythonReleaseGil unlocker;

	return getInitialStateName();
}

std::string EntityCreationParameters::getInitialStateName()
{
	return initialStateName_;
}

void EntityCreationParameters::setInitialStateNamePy(std::string value)
{
	PythonReleaseGil unlocker;

	setInitialStateName(value);
}

void EntityCreationParameters::setInitialStateName(std::string value)
{
	initialStateName_ = value;
}

std::string EntityCreationParameters::getEntityNamePy()
{
	PythonReleaseGil unlocker;

	return getEntityName();
}

std::string EntityCreationParameters::getEntityName()
{
	return entityName_;
}

void EntityCreationParameters::setEntityNamePy(std::string value)
{
	PythonReleaseGil unlocker;

	setEntityName(value);
}

void EntityCreationParameters::setEntityName(std::string value)
{
	entityName_ = value;
}
		
void EntityCreationParameters::addPropertyPy(std::string key, std::string value)
{
	PythonReleaseGil unlocker;

	addProperty(key, value);
}

void EntityCreationParameters::addProperty(std::string key, std::string value)
{
	properties_[key] = value;
}

RoomId EntityCreationParameters::getRoomIdPy()
{
	PythonReleaseGil unlocker;

	return getRoomId();
}

RoomId EntityCreationParameters::getRoomId()
{
	return roomId_;
}

void EntityCreationParameters::setRoomIdPy(RoomId value)
{
	PythonReleaseGil unlocker;

	setRoomId(value);
}

void EntityCreationParameters::setRoomId(RoomId value)
{
	roomId_ = value;
}
		
SpawnPointId EntityCreationParameters::getSpawnPointIdPy()
{
	PythonReleaseGil unlocker;

	return getSpawnPointId();
}

SpawnPointId EntityCreationParameters::getSpawnPointId()
{
	return spawnPointId_;
}

void EntityCreationParameters::setSpawnPointIdPy(SpawnPointId value)
{
	PythonReleaseGil unlocker;

	setSpawnPointId(value);
}

void EntityCreationParameters::setSpawnPointId(SpawnPointId value)
{
	spawnPointId_ = value;
}
		
EntityTypeId EntityCreationParameters::getEntityTypeIdPy()
{
	PythonReleaseGil unlocker;

	return getEntityTypeId();
}

EntityTypeId EntityCreationParameters::getEntityTypeId()
{
	return entityTypeId_;
}

void EntityCreationParameters::setEntityTypeIdPy(EntityTypeId value)
{
	PythonReleaseGil unlocker;

	setEntityTypeId(value);
}

void EntityCreationParameters::setEntityTypeId(EntityTypeId value)
{
	entityTypeId_ = value;
}

InputChannel EntityCreationParameters::getInputChannelPy()
{
	PythonReleaseGil unlocker;

	return getInputChannel();
}

InputChannel EntityCreationParameters::getInputChannel()
{
	return inputChannel_;
}

void EntityCreationParameters::setInputChannelPy(InputChannel value)
{
	PythonReleaseGil unlocker;

	setInputChannel(value);
}

void EntityCreationParameters::setInputChannel(InputChannel value)
{
	inputChannel_ = value;
}

std::string EntityCreationParameters::getIpAddress()
{
	return ipAddress_;
}

void EntityCreationParameters::setIpAddress(std::string value)
{
	ipAddress_ = value;
}

//BufferWrapperPtr EntityCreationParameters::getEntityDataBuffer()
//{
//	return bufferWrapper_;
//}
//
//void EntityCreationParameters::setEntityDataBuffer(BufferWrapperPtr bufferWrapper)
//{
//	bufferWrapper_ = bufferWrapper;
//}