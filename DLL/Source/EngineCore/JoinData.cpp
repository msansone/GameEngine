#include "..\..\Headers\EngineCore\JoinData.hpp"

using namespace firemelon;
using namespace boost::python;

JoinData::JoinData() : 
	ids_()
{
	std::cout<<"Creating new join data"<<std::endl;
	
	roomId_ = ids_.ROOM_NULL;
	entityTypeId_ = ids_.ENTITY_NULL;

	//entitySerializer_ = nullptr;
}

JoinData::JoinData(const JoinData &obj)
{
	//pyEntitySerializer_ = obj.pyEntitySerializer_;

	entityTypeId_ = obj.entityTypeId_;
	roomId_ = obj.roomId_;
	//entitySerializer_ = obj.entitySerializer_;
}

JoinData::~JoinData()
{
	//pyEntitySerializer_ = boost::python::object();
}

void JoinData::setEntityTypeIdPy(EntityTypeId entityTypeId)
{
	PythonReleaseGil unlocker;

	setEntityTypeId(entityTypeId);
}

void JoinData::setEntityTypeId(EntityTypeId entityTypeId)
{
	entityTypeId_ = entityTypeId;
}

EntityTypeId JoinData::getEntityTypeIdPy()
{
	PythonReleaseGil unlocker;

	return getEntityTypeId();
}

EntityTypeId JoinData::getEntityTypeId()
{
	return entityTypeId_;
}

void JoinData::setSpawnPointIdPy(SpawnPointId spawnPointId)
{
	PythonReleaseGil unlocker;

	setSpawnPointId(spawnPointId);
}

void JoinData::setSpawnPointId(SpawnPointId spawnPointId)
{
	spawnPointId_ = spawnPointId;
}

SpawnPointId JoinData::getSpawnPointIdPy()
{
	PythonReleaseGil unlocker;

	return getSpawnPointId();
}

SpawnPointId JoinData::getSpawnPointId()
{
	return spawnPointId_;
}

void JoinData::setRoomIdPy(RoomId roomId)
{
	PythonReleaseGil unlocker;

	setRoomId(roomId);
}

void JoinData::setRoomId(RoomId roomId)
{
	roomId_ = roomId;
}

RoomId JoinData::getRoomIdPy()
{
	PythonReleaseGil unlocker;

	return getRoomId();
}

RoomId JoinData::getRoomId()
{
	return roomId_;
}
		
//void JoinData::setEntitySerializer(EntitySerializer* entitySerializer)
//{
//	entitySerializer_ = entitySerializer;
//
//	entitySerializer_->preSerialize();
//}
//
//void JoinData::setEntitySerializerPy(boost::python::object entitySerializer)
//{
//	PythonReleaseGil unlocker;
//
//	{
//		PythonAcquireGil lock;
//
//		pyEntitySerializer_ = entitySerializer;
//		entitySerializer_ = extract<EntitySerializer*>(entitySerializer.attr("getThis")());
//			
//		try
//		{		
//			SerializerCorePointer serializer = entitySerializer_->getSerializerCore();
//
//			SerializerCore* serializerRaw = serializer.get();
//
//			//findmeserpyEntitySerializer_.attr("serialize")(boost::ref(serializerRaw));
//	
//			boost::ref(serializerRaw);
//
//			serializerRaw->serialize();
//
//		}
//		catch(error_already_set &)
//		{
//			std::cout<<"Error serializing joining entity."<<std::endl;
//			DebugHelper::handlePythonError();
//		}
//	}
//}
//
//EntitySerializer* JoinData::getEntitySerializer()
//{
//	return entitySerializer_;
//}

void JoinData::clear()
{
	roomId_ = ids_.ROOM_NULL;
	entityTypeId_ = ids_.ENTITY_NULL;
	
	//pyEntitySerializer_ = boost::python::object();
	//entitySerializer_ = nullptr;
}