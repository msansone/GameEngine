#include "..\Headers\FiremelonExCodeBehindFactory.hpp"

using namespace firemelon;

FiremelonExCodeBehindFactory::FiremelonExCodeBehindFactory()
{
}

FiremelonExCodeBehindFactory::~FiremelonExCodeBehindFactory()
{
}

//
//CollidableCodeBehind* FiremelonExCodeBehindComponentFactory::createCollidableCodeBehind()
//{
//	CodeBehindComponent* newCodeBehindComponent = nullptr;
//
//	BaseIds* ids = getIds();
//
//	//if (entityType != ids->ENTITY_TILE && 
//	//	entityType != ids->ENTITY_CAMERA && 
//	//	entityType != ids->ENTITY_NULL)
//	//{
//	//	newCodeBehindComponent = new FiremelonExCodeBehindComponent();
//
//	//	((FiremelonExCodeBehindComponent*)newCodeBehindComponent)->attachKeyboardDevice(keyboardDevice_);
//	//}
//
//	return newCodeBehindComponent;
//}

boost::shared_ptr<EntityCodeBehind> FiremelonExCodeBehindFactory::createEntityCodeBehind()
{
	boost::shared_ptr<EntityCodeBehind> newEntityCodeBehind = nullptr;

	newEntityCodeBehind = boost::shared_ptr<EntityCodeBehind>(new FiremelonExEntityCodeBehind());

	return newEntityCodeBehind;
}


boost::shared_ptr<InputReceiverCodeBehind> FiremelonExCodeBehindFactory::createInputReceiverCodeBehind()
{
	boost::shared_ptr<InputReceiverCodeBehind> newInputReceiverCodeBehind = nullptr;
	
	newInputReceiverCodeBehind = boost::shared_ptr<InputReceiverCodeBehind>(new FiremelonExInputReceiverCodeBehind());
	
	boost::static_pointer_cast<FiremelonExInputReceiverCodeBehind>(newInputReceiverCodeBehind)->attachKeyboardDevice(keyboardDevice_);

	return newInputReceiverCodeBehind;
}

//MessageableCodeBehind* FiremelonExCodeBehindComponentFactory::createMessageableCodeBehind()
//{
//
//}
//
//SimulatableCodeBehind* FiremelonExCodeBehindComponentFactory::createSimulatableCodeBehind()
//{
//
//}
//
//StatefulCodeBehind* FiremelonExCodeBehindComponentFactory::createStatefulCodeBehind()
//{
//
//}

void FiremelonExCodeBehindFactory::attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice)
{
	keyboardDevice_ = keyboardDevice;
}