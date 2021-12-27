/* -------------------------------------------------------------------------
** Debugger.hpp
** 
** The Debugger class is a utility that provides functionality that assist
** with debugging, including logging to a file, and toggling various debugging
** subsystems.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _DEBUGGER_HPP_
#define _DEBUGGER_HPP_

#include "CollisionLogger.hpp"
#include "NotificationManager.hpp"
#include "PythonGil.hpp"

#include <boost/python.hpp>

#include <boost/shared_ptr.hpp>
#include <boost/thread.hpp>
#include <boost/thread/mutex.hpp>
#include <boost/enable_shared_from_this.hpp>

#include <string>
#include <iostream>
#include <fstream>

#include "stdlib.h"

namespace firemelon
{
	class Debugger
	{
	public:
		Debugger(boost::shared_ptr<NotificationManager> notificationManager);
		virtual ~Debugger();

		bool		getIsSingleFrameUpdate();
		bool		getIsSingleFrameUpdatePy();

		void		setDebugMode(bool debugModeOn);
		bool		getDebugMode();

		void		setDebugLoggingOn(bool debugLoggingOn);
		bool		getDebugLoggingOn();
		
		void		setDebugModeRenderHitboxes(bool renderHitboxes);
		bool		getDebugModeRenderHitboxes();

		void		setDebugModeBreakOnError(bool breakOnError);
		bool		getDebugModeBreakOnError();

		void		setDebugModeRenderDynamicsControllers(bool renderDynamicsControllers);
		bool		getDebugModeRenderDynamicsControllers();

		void		setDebugModeRenderStage(bool renderStage);
		bool		getDebugModeRenderStage();

		void		setDebugLogPath(std::string debugLogPath);
		std::string	getDebugLogPath();

		void		setLogColisions(bool logCollisions);
		bool		getLogColisions();

		void		setPanelElementName(std::string name);
		std::string getPanelElementName();

		void		appendToLog(std::string logMessage);
		void		appendToLogPy(std::string logMessage);
		void		clearLog();
		void		writeLogToFile();

		void		handlePythonError();

		int			debugLevel;

		int			tracker;

		double		doubleTracker1;
		double		doubleTracker2;
		double		doubleTracker3;
		double		doubleTracker4;
		double		doubleTracker5;
		double		doubleTracker6;

		boost::shared_ptr<boost::mutex>	streamLock;

		static unsigned int	codeBehindCount;
		static unsigned int	collisionDataCount;
		static unsigned int	entityComponentCount;
		static unsigned int	entityControllerCount;
		static unsigned int	entityCount;
		static unsigned int	renderableTextCount;
		static unsigned int	messageCount;
		static unsigned int	queryCount;
		static unsigned int	loadingScreenCount;
		static unsigned int	transitionCount;
		static unsigned int	roomCount;
		static unsigned int	c;

		static bool advanceFrameLog;

		CollisionLoggerPtr		collisionLogger;

	private:
	
		bool			debugModeOn_;
		bool			debugLoggingOn_;
		bool			renderHitboxes_;
		bool			breakOnError_;
		bool			renderDynamicsControllers_;
		bool			renderStage_;

		std::string	debugLog_;
		std::string	debugLogPath_;
		std::string	panelElementName_;

		NotificationManagerPtr	notificationManager_;
	};

	typedef boost::shared_ptr<Debugger> DebuggerPtr;
}

#endif // _DEBUGGER_HPP_