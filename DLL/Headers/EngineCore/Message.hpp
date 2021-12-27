/* -------------------------------------------------------------------------
** Message.hpp
**
** The Message class represents a message object that can be sent from one 
** entity to one or more other entities. It can be sent either to a specific 
** entity ID or list of IDs, or to all entities of a certain type, or a list 
** of entity types. It contains an integer code that the reciever uses to 
** determine how the message should be handled.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _MESSAGE_HPP_
#define _MESSAGE_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <vector>

#include <boost/python.hpp>

#include "BaseIds.hpp"
#include "Debugger.hpp"
#include "MessageContent.hpp"

namespace firemelon
{
	enum MessagePriority
	{
		// Normal priority messages will be sent during the message dispatch phase at the beginning of the frame update.
		MESSAGE_PRIORITY_NORMAL = 0,

		// Immediate priority messages are sent as soon as the entity sends it.
		MESSAGE_PRIORITY_IMMEDIATE = 1
	};

	class FIREMELONAPI Message
	{
	public:

		Message();
		virtual ~Message();

		void					initialize();

		void								setMessageContentPy(boost::python::object pyMessageContent);
		void								setMessageContent(boost::shared_ptr<MessageContent> content);

		boost::shared_ptr<MessageContent>	getMessageContentPy();
		boost::shared_ptr<MessageContent>	getMessageContent();

		int									getMessageCodePy();
		int									getMessageCode();
		
		void								setMessageCodePy(int code);
		void								setMessageCode(int code);
		
		int									getSenderIdPy();
		int									getSenderId();

		void								setSenderIdPy(int id);
		void								setSenderId(int id);

		EntityTypeId						getSenderTypePy();
		EntityTypeId						getSenderType();
		
		void								setSenderTypePy(EntityTypeId type);
		void								setSenderType(EntityTypeId type);
		
		MessagePriority						getPriorityPy();
		MessagePriority						getPriority();
		
		void								setPriorityPy(MessagePriority priority);
		void								setPriority(MessagePriority priority);

		void								addReceiverIdPy(int id);
		void								addReceiverId(int id);
		
		void								addReceiverEntityTypePy(EntityTypeId entityType);
		void								addReceiverEntityType(EntityTypeId entityType);
	
		int									getRoomIdPy();
		int									getRoomId();

		void								setRoomIdPy(int id);
		void								setRoomId(int id);

		int									getReceiverIdCountPy();
		int									getReceiverIdCount();

		int									getReceiverIdPy(int index);
		int									getReceiverId(int index);
		
		int									getReceiverEntityTypeCountPy();
		int									getReceiverEntityTypeCount();

		EntityTypeId						getReceiverEntityTypePy(int index);
		EntityTypeId						getReceiverEntityType(int index);
	
	private:

		boost::shared_ptr<MessageContent>	content_;
		BaseIds								ids_;
		int									messageCode_;
		MessagePriority						priority_;
		std::vector<int>					receiverIdList_;
		std::vector<EntityTypeId>			receiverEntityTypeList_;
		RoomId								roomId_;
		int									senderId_;
		EntityTypeId						senderType_;
	};
}

#endif // _MESSAGE_HPP_