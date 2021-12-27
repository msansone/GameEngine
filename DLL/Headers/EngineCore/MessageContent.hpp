/* -------------------------------------------------------------------------
** MessageContent.hpp
**
** The MessageContent class is a base class which can be derived from so
** custom data can be sent in a message. It should be subclassed and then
** an instance should be set in the message's Content property.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _MESSAGECONTENT_HPP_
#define _MESSAGECONTENT_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <vector>
#include <boost/python.hpp>

#include "BaseIds.hpp"
#include "Types.hpp"

namespace firemelon
{
	class FIREMELONAPI MessageContent
	{
	public:
		friend class Message;

		MessageContent();
		virtual ~MessageContent();

		PyObj	getPyInstance();

	private:
		
		PyObj	pyInstance_;
	};
}

#endif // _MESSAGECONTENT_HPP_