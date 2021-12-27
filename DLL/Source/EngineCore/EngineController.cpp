#include "..\..\Headers\EngineCore\EngineController.hpp"

using namespace firemelon;
using namespace boost;
using namespace boost::python;
using namespace boost::signals2;

EngineController::EngineController() 	
{
	advanceOneFrame_ = false;

	hasQuit_ = false;
	
	isSimulationStopped_ = false;
}

EngineController::~EngineController()
{
}

void EngineController::exitPy()
{
	PythonReleaseGil unlocker;

	exit();
}

void EngineController::exit()
{
	hasQuit_ = true;
}

bool EngineController::getHasQuit()
{
	return hasQuit_;
}

void EngineController::runCommandPy(std::string command)
{
	PythonReleaseGil unlocker;

	runCommand(command);
}

void EngineController::runCommand(std::string command)
{
	// Echo the message.	
	systemMessageDispatcher_->addSystemMessage(command);

	std::string commandName = "";

	std::string commandParam = "";

	if (command[0] == '/')
	{
		int equalsIndex = command.find('=');

		if (equalsIndex == -1)
		{
			commandName = command.substr(1, command.size() - 1);
		}
		else
		{
			commandName = command.substr(1, equalsIndex - 1);

			commandParam = command.substr(equalsIndex + 1, command.size() - equalsIndex);
		}
	}
	else
	{
		commandName = "";
	}

	boost::to_upper(commandName);

	boost::algorithm::trim_right(commandName);

	boost::algorithm::trim(commandParam);

	if (commandName == "TOGGLEDEBUG")
	{
		// Toggle debug mode.
		bool isDebugModeOn = debugger_->getDebugMode();

		isDebugModeOn = !isDebugModeOn;

		if (isDebugModeOn == true)
		{
			systemMessageDispatcher_->addSystemMessage("Debug mode on");
		}
		else
		{
			systemMessageDispatcher_->addSystemMessage("Debug mode off");
		}

		debugger_->setDebugMode(isDebugModeOn);
	}
	else if (commandName == "TOGGLEHITBOX")
	{
		bool renderHitboxes = debugger_->getDebugModeRenderHitboxes();

		renderHitboxes = !renderHitboxes;

		if (renderHitboxes == true)
		{
			systemMessageDispatcher_->addSystemMessage("Render hitboxes on");
		}
		else
		{
			systemMessageDispatcher_->addSystemMessage("Render hitboxes off");
		}

		debugger_->setDebugModeRenderHitboxes(renderHitboxes);
	}
	else if (commandName == "TOGGLESTAGE")
	{
		bool renderStage = debugger_->getDebugModeRenderStage();

		renderStage = !renderStage;

		if (renderStage == true)
		{
			systemMessageDispatcher_->addSystemMessage("Render stage on");
		}
		else
		{
			systemMessageDispatcher_->addSystemMessage("Render stage off");
		}

		debugger_->setDebugModeRenderStage(renderStage);
	}
	else if (commandName == "TOGGLELOGCOLLISIONS")
	{
		bool logCollisions = debugger_->getLogColisions();

		logCollisions = !logCollisions;

		if (logCollisions == true)
		{
			systemMessageDispatcher_->addSystemMessage("Logging collisions on");
		}
		else
		{
			systemMessageDispatcher_->addSystemMessage("Logging collisions off");
		}

		debugger_->setLogColisions(logCollisions);
	}
	else if (commandName == "TOGGLEBREAKONERROR")
	{
		bool breakOnError = debugger_->getDebugModeBreakOnError();

		breakOnError = !breakOnError;

		if (breakOnError == true)
		{
			systemMessageDispatcher_->addSystemMessage("Break on error on");
		}
		else
		{
			systemMessageDispatcher_->addSystemMessage("Break on error off");
		}

		debugger_->setDebugModeBreakOnError(breakOnError);
	}
	else if (commandName == "TOGGLELOGGING")
	{
		bool isLoggingOn = debugger_->getDebugLoggingOn();

		isLoggingOn = !isLoggingOn;
		
		if (isLoggingOn == true)
		{
			systemMessageDispatcher_->addSystemMessage("Debug logging on");
		}
		else
		{
			systemMessageDispatcher_->addSystemMessage("Debug logging off");
		}

		debugger_->setDebugLoggingOn(isLoggingOn);
	}
	else if (commandName == "LOGPATH")
	{
		systemMessageDispatcher_->addSystemMessage("Debug log path set to " + commandParam);
		
		debugger_->setDebugLogPath(commandParam);
	}
	else if (commandName == "UIELEMENT")
	{
		systemMessageDispatcher_->addSystemMessage("Panel element set to " + commandParam);

		debugger_->setPanelElementName(commandParam);
	}
	else if (commandName == "WRITELOG")
	{
		debugger_->writeLogToFile();
	}
	else if (commandName == "TOGGLEDYNAMICSRENDER")
	{
		bool isDynamicsControllerRenderingOn = debugger_->getDebugModeRenderDynamicsControllers();

		isDynamicsControllerRenderingOn = !isDynamicsControllerRenderingOn;
		
		if (isDynamicsControllerRenderingOn == true)
		{
			systemMessageDispatcher_->addSystemMessage("Dynamics controller render on");
		}
		else
		{
			systemMessageDispatcher_->addSystemMessage("Dynamics controller render off");
		}

		debugger_->setDebugModeRenderDynamicsControllers(isDynamicsControllerRenderingOn);
	}
}

void EngineController::saveScreenshotPy(std::string fileName)
{
	PythonReleaseGil unlocker;

	saveScreenshot(fileName);
}

void EngineController::saveScreenshot(std::string fileName)
{
	renderer_->saveScreenshot(fileName);
}

RoomId EngineController::getIdFromNamePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getIdFromName(name);
}

RoomId EngineController::getIdFromName(std::string name)
{
	return BaseIds::nameIdMap[name];
}

std::string EngineController::getNameFromIdPy(FiremelonId id)
{
	PythonReleaseGil unlocker;

	return getNameFromId(id);
}

std::string EngineController::getNameFromId(FiremelonId id)
{
	return BaseIds::idNameMap[id];
}

void EngineController::startSimulation()
{
	isSimulationStopped_ = false;
}

void EngineController::startSimulationPy()
{
	PythonReleaseGil unlocker;

	startSimulation();
}

void EngineController::stopSimulation()
{
	isSimulationStopped_ = true;
}

void EngineController::stopSimulationPy()
{
	PythonReleaseGil unlocker;

	stopSimulation();
}

bool EngineController::getIsSimulationStopped()
{
	return isSimulationStopped_;
}

void EngineController::advanceSimulationFrame()
{
	advanceOneFrame_ = true;
}

void EngineController::advanceSimulationFramePy()
{
	PythonReleaseGil unlocker;

	advanceSimulationFrame();
}

void EngineController::setSimulationTimeScalePy(float timeScale)
{
	PythonReleaseGil unlocker;

	setSimulationTimeScale(timeScale);
}

void EngineController::setSimulationTimeScale(float timeScale)
{
	physicsConfig_->setTimeScale(timeScale);
}

float EngineController::getSimulationTimeScalePy()
{
	PythonReleaseGil unlocker;

	return getSimulationTimeScale();
}

float EngineController::getSimulationTimeScale()
{
	return physicsConfig_->getTimeScale();
}