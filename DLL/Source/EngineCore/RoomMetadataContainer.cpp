#include "..\..\Headers\EngineCore\RoomMetadataContainer.hpp"

using namespace firemelon;

RoomMetadataContainer::RoomMetadataContainer()
{
}

RoomMetadataContainer::~RoomMetadataContainer()
{
}

boost::shared_ptr<RoomMetadata> RoomMetadataContainer::getRoomMetadataPy(RoomId roomId)
{
	PythonReleaseGil unlocker;

	return getRoomMetadata(roomId);
}

boost::shared_ptr<RoomMetadata> RoomMetadataContainer::getRoomMetadata(RoomId roomId)
{
	if (roomIdToMetadataMap_.count(roomId) == 1)
	{
		return roomIdToMetadataMap_[roomId];
	}

	return nullptr;
}

void RoomMetadataContainer::addRoomMetadata(boost::shared_ptr<RoomMetadata> roomMetadata)
{
	roomIdToMetadataMap_[roomMetadata->getRoomId()] = roomMetadata;
}



