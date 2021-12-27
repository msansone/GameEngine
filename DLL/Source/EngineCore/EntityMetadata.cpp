#include "..\..\Headers\EngineCore\EntityMetadata.hpp"

using namespace firemelon;

EntityMetadata::EntityMetadata()
{
	entityTypeId_ = -1;
	layerIndex_ = -1;
	
	height_ = 0;
	width_ = 0;

	name_ = "";
}

EntityMetadata::~EntityMetadata()
{
}

int EntityMetadata::getEntityInstanceIdPy()
{
	PythonReleaseGil unlocker;

	return getEntityInstanceId();
}

int EntityMetadata::getEntityInstanceId()
{
	return entityInstanceId_;
}

void EntityMetadata::setEntityInstanceId(int value)
{
	entityInstanceId_ = value;
}

EntityTypeId EntityMetadata::getEntityTypeIdPy()
{
	PythonReleaseGil unlocker;

	return getEntityTypeId();
}

EntityTypeId EntityMetadata::getEntityTypeId()
{
	return entityTypeId_;
}

void EntityMetadata::setEntityTypeId(EntityTypeId value)
{
	entityTypeId_ = value;
}
		
EntityClassificationId EntityMetadata::getClassificationIdPy()
{
	PythonReleaseGil unlocker;

	return getClassificationId();
}

EntityClassificationId EntityMetadata::getClassificationId()
{
	return classificationId_;
}

void EntityMetadata::setClassificationId(EntityClassificationId value)
{
	classificationId_ = value;
}

int	EntityMetadata::getHeightPy()
{
	PythonReleaseGil unlocker;

	return getHeight();
}

int	EntityMetadata::getHeight()
{
	return height_;
}

int	EntityMetadata::getWidthPy()
{
	PythonReleaseGil unlocker;

	return getWidth();
}

int	EntityMetadata::getWidth()
{
	return width_;
}

std::string EntityMetadata::getTagPy()
{
	PythonReleaseGil unlocker;

	return getTag();
}

std::string EntityMetadata::getTag()
{
	return tag_;
}

void EntityMetadata::setTag(std::string value)
{
	tag_ = value;
}

boost::shared_ptr<RoomMetadata> EntityMetadata::getRoomMetadataPy()
{
	PythonReleaseGil unlocker;

	return getRoomMetadata();
}

boost::shared_ptr<RoomMetadata> EntityMetadata::getRoomMetadata()
{
	return roomMetadata_;
}

boost::shared_ptr<RoomMetadata> EntityMetadata::getPreviousRoomMetadataPy()
{
	PythonReleaseGil unlocker;

	return getPreviousRoomMetadata();
}

boost::shared_ptr<RoomMetadata> EntityMetadata::getPreviousRoomMetadata()
{
	return previousRoomMetadata_;
}

std::string EntityMetadata::getEntityInstanceNamePy()
{
	PythonReleaseGil unlocker;

	return getEntityInstanceName();
}

std::string	EntityMetadata::getEntityInstanceName()
{
	return name_;
}

int	EntityMetadata::getMapLayerPy()
{
	PythonReleaseGil unlocker;

	return getMapLayer();
}

int EntityMetadata::getMapLayer()
{
	return layerIndex_;
}