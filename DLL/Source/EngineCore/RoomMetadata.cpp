#include "..\..\Headers\EngineCore\RoomMetadata.hpp"

using namespace firemelon;

RoomMetadata::RoomMetadata()
{
	mapHeight_ = 0;
	mapWidth_ = 0;
}

RoomMetadata::~RoomMetadata()
{
}


RoomId RoomMetadata::getRoomIdPy()
{
	PythonReleaseGil unlocker;

	return getRoomId();
}

RoomId RoomMetadata::getRoomId()
{
	return roomId_;
}

std::string	RoomMetadata::getRoomNamePy()
{
	PythonReleaseGil unlocker;

	return getRoomName();
}

std::string	RoomMetadata::getRoomName()
{
	return roomName_;
}

int RoomMetadata::getMapHeightPy()
{
	PythonReleaseGil unlocker;

	return getMapHeight();
}

int RoomMetadata::getMapHeight()
{
	return mapHeight_;
}

int RoomMetadata::getMapWidthPy()
{
	PythonReleaseGil unlocker;

	return getMapWidth();
}

int RoomMetadata::getMapWidth()
{
	return mapWidth_;
}