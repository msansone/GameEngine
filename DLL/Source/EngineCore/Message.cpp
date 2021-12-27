#include "..\..\Headers\EngineCore\Message.hpp"

using namespace firemelon;
using namespace boost::python;

Message::Message() :
	ids_()
{
	priority_ = MESSAGE_PRIORITY_NORMAL;
	senderId_ = -1;	
	senderType_ = ids_.ENTITY_NULL;
	messageCode_ = -1;
	content_ = nullptr;
	Debugger::messageCount++;
}

Message::~Message()
{
	// Need to free the pyInstance object so the smart pointer can de-allocate itself.
	if (content_ != nullptr)
	{
		content_->pyInstance_ = boost::python::object();
	}
	
	Debugger::messageCount--;
}

int Message::getSenderIdPy()
{
	PythonReleaseGil unlocker;

	return getSenderId();
}

int Message::getSenderId()
{
	return senderId_;
}

void Message::setSenderIdPy(int id)
{
	PythonReleaseGil unlocker;

	setSenderId(id);
}

void Message::setSenderId(int id)
{
	senderId_ = id;
}

void Message::setSenderTypePy(EntityTypeId type)
{
	PythonReleaseGil unlocker;

	setSenderType(type);
}

void Message::setSenderType(EntityTypeId type)
{
	senderType_ = type;
}

EntityTypeId Message::getSenderTypePy()
{
	PythonReleaseGil unlocker;

	return getSenderType();
}

EntityTypeId Message::getSenderType()
{
	return senderType_;
}

void Message::addReceiverIdPy(int id)
{
	PythonReleaseGil unlocker;

	addReceiverId(id);
}

void Message::addReceiverId(int id)
{
	receiverIdList_.push_back(id);
}

void Message::addReceiverEntityTypePy(EntityTypeId entityType)
{
	PythonReleaseGil unlocker;

	addReceiverEntityType(entityType);
}

void Message::addReceiverEntityType(EntityTypeId entitType)
{
	receiverEntityTypeList_.push_back(entitType);
}

int Message::getRoomIdPy()
{
	PythonReleaseGil unlocker;

	return getRoomId();
}

int Message::getRoomId()
{
	return roomId_;
}

void Message::setRoomIdPy(int id)
{
	PythonReleaseGil unlocker;

	setRoomId(id);
}

void Message::setRoomId(int id)
{
	roomId_ = id;
}

int Message::getMessageCodePy()
{
	PythonReleaseGil unlocker;

	return getMessageCode();
}

int Message::getMessageCode()
{
	return messageCode_;
}

void Message::setMessageCodePy(int code)
{
	PythonReleaseGil unlocker;

	setMessageCode(code);
}

void Message::setMessageCode(int code)
{
	messageCode_ = code;
}

int Message::getReceiverIdCountPy()
{
	PythonReleaseGil unlocker;

	return getReceiverIdCount();
}

int Message::getReceiverIdCount()
{
	return receiverIdList_.size();
}

int Message::getReceiverIdPy(int index)
{
	PythonReleaseGil unlocker;

	return getReceiverId(index);
}

int Message::getReceiverId(int index)
{
	int size = receiverIdList_.size();

	if (index >= 0 && index < size)
	{
		return receiverIdList_[index];
	}
	else
	{
		return -1;
	}
}

int Message::getReceiverEntityTypeCountPy()
{
	PythonReleaseGil unlocker;

	return getReceiverEntityTypeCount();
}

int Message::getReceiverEntityTypeCount()
{
	return receiverEntityTypeList_.size();
}

EntityTypeId Message::getReceiverEntityTypePy(int index)
{
	PythonReleaseGil unlocker;

	return getReceiverEntityType(index);
}

EntityTypeId Message::getReceiverEntityType(int index)
{
	int size = receiverEntityTypeList_.size();

	if (index >= 0 && index < size)
	{
		return receiverEntityTypeList_[index];
	}
	else
	{
		return ids_.ENTITY_NULL;
	}
}

MessagePriority Message::getPriorityPy()
{
	PythonReleaseGil unlocker;

	return getPriority();
}

MessagePriority Message::getPriority()
{
	return priority_;
}

void Message::setPriorityPy(MessagePriority priority)
{
	PythonReleaseGil unlocker;

	return setPriority(priority);
}

void Message::setPriority(MessagePriority priority)
{
	priority_ = priority;
}


void Message::setMessageContentPy(object pyMessageContent)
{
	PythonReleaseGil unlocker;

	{
		PythonAcquireGil lock;

		boost::shared_ptr<MessageContent>& messageContent = extract<boost::shared_ptr<MessageContent>&>(pyMessageContent);

		messageContent->pyInstance_ = pyMessageContent;

		//messageContent->isPyObject_ = true;

		content_ = messageContent;
	}
}

void Message::setMessageContent(boost::shared_ptr<MessageContent> messageContent)
{
	content_ = messageContent;
}

boost::shared_ptr<MessageContent> Message::getMessageContentPy()
{
	PythonReleaseGil unlocker;

	return getMessageContent();
}

boost::shared_ptr<MessageContent> Message::getMessageContent()
{
	return content_;
}