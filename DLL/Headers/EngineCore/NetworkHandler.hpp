/* -------------------------------------------------------------------------
** NetworkHandler.hpp
**
** The NetworkHandler class contains a script element which the user
** can write to respond to server events, like connected or disconnected,
** as well as custom requests/commands.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _NETWORKHANDLER_HPP_
#define _NETWORKHANDLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>

#include "EngineController.hpp"
#include "MenuController.hpp"
#include "DebugHelper.hpp"
#include "JoinData.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI NetworkHandler
	{
	public:
		friend class GameEngine;
		friend class Assets;

		NetworkHandler();
		virtual ~NetworkHandler();

		void			serverStarted();
		void			connectionOpened();
		void			connectionClosedByClient();
		void			connectionClosedByServer();

		virtual void	userInitialize();

	private:

		void	initializePythonData();
		
		boost::shared_ptr<MenuController>	menuController_;
		boost::shared_ptr<EngineController>	engineController_;
		boost::shared_ptr<JoinData>			joinData_;

		// Scripting data
		std::string				scriptName_;
		std::string				scriptTypeName_;
		
		boost::python::object	pyMainModule_;
		boost::python::object	pyMainNamespace_;
		boost::python::object	pyNetworkHandlerInstance_;
		boost::python::object	pyNetworkHandlerNamespace_;
		
		boost::python::object	pyConnectionClosedByServer_;
		boost::python::object	pyConnectionClosedByClient_;
		boost::python::object	pyConnectionOpened_;
		boost::python::object	pyServerStarted_;		
	};
}

#endif // _NETWORKHANDLER_HPP_