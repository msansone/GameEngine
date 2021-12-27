//#include "..\..\Headers\EngineCore\EntityReplicationState.hpp"
//
//using namespace firemelon;
//
//EntityReplicationState::EntityReplicationState() : 
//	stateDataBuffer_(new BufferWrapper())
//{	
//	reset();
//}
//
//EntityReplicationState::EntityReplicationState(const EntityReplicationState &obj)
//{
//	// Copy constructor. Make a copy of the shared pointer.
//	stateDataBuffer_ = obj.stateDataBuffer_;
//	entityTypeId_ = obj.entityTypeId_;
//	serverId_ = obj.serverId_;
//	mapLayer_ = obj.mapLayer_;
//	stateIndex_ = obj.stateIndex_;
//	//entitySerializer_ = obj.entitySerializer_;
//	positionX_ = obj.positionX_;
//	positionY_ = obj.positionY_;
//	movementX_ = obj.movementX_;
//	movementY_ = obj.movementY_;
//	velocityX_ = obj.velocityX_;
//	velocityY_ = obj.velocityY_;
//	lookX_ = obj.lookX_;
//	lookY_ = obj.lookY_;
//	accelerationX_ = obj.accelerationX_;
//	accelerationY_ = obj.accelerationY_;
//	attachCamera_ = obj.attachCamera_;
//	acceptInput_ = obj.acceptInput_;
//	ipAddress_ = obj.ipAddress_;
//}
//
//EntityReplicationState::~EntityReplicationState()
//{
//}
//
//void EntityReplicationState::reset()
//{
//	//entityTypeId_ = ids_->ENTITY_NULL;
//	serverId_ = -1;
//	mapLayer_ = -1;
//	stateIndex_ = -1;
//	positionX_ = 0.0f;
//	positionY_ = 0.0f;
//	movementX_ = 0.0f;
//	movementY_ = 0.0f;
//	velocityX_ = 0.0;
//	velocityY_ = 0.0f;
//	accelerationX_ = 0.0f;
//	accelerationY_ = 0.0f;
//	lookX_ = 0.0f;
//	lookY_ = 0.0f;
//
//	entityTypeId_ = -2;
//
//	acceptInput_ = false;
//	attachCamera_ = false;
//	ipAddress_ = "";
//	
//	BufferWrapperPtr newBuffer(new BufferWrapper);
//
//	stateDataBuffer_ = newBuffer;
//}
//
//EntityTypeId EntityReplicationState::getEntityTypeId()
//{
//	return entityTypeId_;
//}
//
//void EntityReplicationState::setEntityTypeId(EntityTypeId value)
//{
//	entityTypeId_ = value;
//}
//
//int EntityReplicationState::getServerId()
//{
//	return serverId_;
//}
//
//void EntityReplicationState::setServerId(int value)
//{
//	serverId_ = value;
//}
//
//int	EntityReplicationState::getMapLayer()
//{
//	return mapLayer_;
//}
//
//void EntityReplicationState::setMapLayer(int value)
//{
//	mapLayer_ = value;
//}
//
//int	EntityReplicationState::getStateIndex()
//{
//	return stateIndex_;
//}
//
//void EntityReplicationState::setStateIndex(int value)
//{
//	stateIndex_ = value;
//}
//
////EntitySerializer* EntityReplicationState::getEntitySerializer()
////{
////	return entitySerializer_;
////}
////
////void EntityReplicationState::setEntitySerializer(EntitySerializer* value)
////{
////	entitySerializer_ = value;
////}
//
//BufferWrapperPtr EntityReplicationState::getEntityDataBuffer()
//{
//	return stateDataBuffer_;
//}
//
//void EntityReplicationState::setEntityDataBuffer(BufferWrapperPtr value)
//{
//	stateDataBuffer_ = value;
//}
//
//float EntityReplicationState::getPositionX()
//{
//	return positionX_;
//}
//
//void EntityReplicationState::setPositionX(float value)
//{
//	positionX_ = value;
//}
//
//float EntityReplicationState::getPositionY()
//{
//	return positionY_;
//}
//
//void EntityReplicationState::setPositionY(float value)
//{
//	positionY_ = value;
//}
//
//float EntityReplicationState::getVelocityX()
//{
//	return velocityX_;
//}
//
//void EntityReplicationState::setVelocityX(float value)
//{
//	velocityX_ = value;
//}
//		
//float EntityReplicationState::getVelocityY()
//{
//	return velocityY_;
//}
//
//void EntityReplicationState::setVelocityY(float value)
//{
//	velocityY_ = value;
//}
//
//float EntityReplicationState::getMovementX()
//{
//	return movementX_;
//}
//
//void EntityReplicationState::setMovementX(float value)
//{
//	movementX_ = value;
//}
//		
//float EntityReplicationState::getMovementY()
//{
//	return movementY_;
//}
//
//void EntityReplicationState::setMovementY(float value)
//{
//	movementY_ = value;
//}
//
//float EntityReplicationState::getAccelerationX()
//{
//	return accelerationX_;
//}
//
//void EntityReplicationState::setAccelerationX(float value)
//{
//	accelerationX_ = value;
//}
//		
//float EntityReplicationState::getAccelerationY()
//{
//	return accelerationX_;
//}
//
//void EntityReplicationState::setAccelerationY(float value)
//{
//	accelerationY_ = value;
//}
//
//float EntityReplicationState::getLookX()
//{
//	return lookX_;
//}
//
//void EntityReplicationState::setLookX(float value)
//{
//	lookX_ = value;
//}
//
//float EntityReplicationState::getLookY()
//{
//	return lookY_;
//}
//
//void EntityReplicationState::setLookY(float value)
//{
//	lookY_ = value;
//}
//
//std::string	EntityReplicationState::getIpAddress()
//{
//	return ipAddress_;
//}
//
//void EntityReplicationState::setIpAddress(std::string value)
//{
//	ipAddress_ = value;
//}
//
//bool EntityReplicationState::getAttachCamera()
//{
//	return attachCamera_;
//}
// 
//void EntityReplicationState::setAttachCamera(bool value)
//{
//	attachCamera_ = value;
//}
//
//bool EntityReplicationState::getAcceptInput()
//{
//	return acceptInput_;
//}
//
//void EntityReplicationState::setAcceptInput(bool value)
//{
//	acceptInput_ = value;
//}