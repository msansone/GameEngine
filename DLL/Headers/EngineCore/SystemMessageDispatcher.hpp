/* -------------------------------------------------------------------------
** SystemMessageDispatcher.hpp
**
** The SystemMessageDispatcher class stores the system notification strings
** sent by the engine and alerts connected listeners when a new message has
** been added. It is up to the user to implement listners and display the 
** messages however they see fit.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SYSTEMMESSAGEDISPATCHER_HPP_
#define _SYSTEMMESSAGEDISPATCHER_HPP_

#include <string>
#include <vector>
#include <iostream>

#include <boost/signals2.hpp>
#include <boost/python.hpp>

#include "Debugger.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class SystemMessageDispatcher
	{
	public:
		SystemMessageDispatcher();
		virtual ~SystemMessageDispatcher();

		void	addListener(boost::python::object listener);
		void	addSystemMessage(std::string systemMessage);
		void	cleanup();
		void	dispatch();

	private:
		
		std::vector<std::string>			newMessages_;
		std::vector<std::string>			dispatchedMessages_;
		std::vector<boost::python::object>	pyListeners_;
	};

	typedef boost::shared_ptr<SystemMessageDispatcher> SystemMessageDispatcherPtr;
}

#endif // _SYSTEMMESSAGEDISPATCHER_HPP_