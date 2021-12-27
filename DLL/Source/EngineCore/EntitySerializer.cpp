//#include "..\..\Headers\EngineCore\EntitySerializer.hpp"
//
//using namespace firemelon;
//using namespace boost::signals2;
//
//EntitySerializer::EntitySerializer() :
//	serializeEntitySignal_(new boost::signals2::signal<void()>()),
//	deserializeEntitySignal_(new boost::signals2::signal<void()>()),
//	serializer_(new SerializerCore())
//{
//	uuid_ = boost::uuids::random_generator()();
//
//	//std::cout<<"Creating entity data "<<uuid_<<std::endl;
//}
//
//EntitySerializer::EntitySerializer(const EntitySerializer &obj)
//{
//	serializer_ = obj.serializer_;
//	serializeEntitySignal_ = obj.serializeEntitySignal_;
//	deserializeEntitySignal_ = obj.deserializeEntitySignal_;
//
//	uuid_ = boost::uuids::random_generator()();
//
//	//std::cout<<"Creating via copy entity data "<<uuid_<<std::endl;
//}
//
//EntitySerializer::~EntitySerializer()
//{
//	//std::cout<<"Deleting entity data "<<uuid_<<std::endl;
//}
//
//SerializerCorePointer EntitySerializer::getSerializerCore()
//{
//	return serializer_;
//}
//
//EntitySerializer* EntitySerializer::getThisPy()
//{
//	PythonReleaseGil unlocker;
//
//	return getThis();
//}
//
//EntitySerializer* EntitySerializer::getThis()
//{
//	return this;
//}
//
//void EntitySerializer::preSerialize()
//{	
//	serializer_->clear();
//
//	(*serializeEntitySignal_)();
//		
//	serializer_->serialize();	
//}
//
//void EntitySerializer::preDeserialize()
//{
//	serializer_->deserialize();
//		
//	(*deserializeEntitySignal_)();
//}
//
//void EntitySerializer::serialize(SerializerCorePointer serializer)
//{	
//}
//
//void EntitySerializer::deserialize(SerializerCorePointer serializer)
//{
//
//}