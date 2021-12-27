#include "..\..\Headers\EngineCore\Debugger.hpp"

using namespace firemelon;
using namespace boost::python;

unsigned int Debugger::codeBehindCount = 0;
unsigned int Debugger::collisionDataCount = 0;
unsigned int Debugger::entityComponentCount = 0;
unsigned int Debugger::entityControllerCount = 0;
unsigned int Debugger::entityCount = 0;
unsigned int Debugger::loadingScreenCount = 0;
unsigned int Debugger::messageCount = 0;
unsigned int Debugger::queryCount = 0;
unsigned int Debugger::renderableTextCount = 0;
unsigned int Debugger::roomCount = 0;
unsigned int Debugger::transitionCount = 0;
unsigned int Debugger::c = 0;
bool Debugger::advanceFrameLog = false;

Debugger::Debugger(boost::shared_ptr<NotificationManager> notificationManager) : 
	streamLock(new boost::mutex())
{
	debugLoggingOn_ = false;
	debugModeOn_ = false;
	debugLogPath_ = "Data\\debug.log";
	debugLog_ = "";

	collisionLogger = boost::make_shared<CollisionLogger>(CollisionLogger());

	renderHitboxes_ = false;
	renderStage_ = false;
	breakOnError_ = false;
	renderDynamicsControllers_ = false;
	panelElementName_ = "";
	notificationManager_ = notificationManager;
	debugLevel = 0;

	collisionLogger->types = COLLISION_ENTER | COLLISION_EXIT | COLLISION_NORMAL;

	collisionLogger->validEntityInstanceIds.emplace(186);

	tracker = 0;

	doubleTracker1 = 0;
	doubleTracker2 = 0;
	doubleTracker3 = 0;
	doubleTracker4 = 0;
	doubleTracker5 = 0;
	doubleTracker6 = 0;
}

Debugger::~Debugger()
{
}

bool Debugger::getIsSingleFrameUpdate()
{
	return advanceFrameLog;
}

bool Debugger::getIsSingleFrameUpdatePy()
{
	PythonReleaseGil unlocker;

	return getIsSingleFrameUpdate();
}

void Debugger::setDebugMode(bool debugModeOn)
{
	debugModeOn_ = debugModeOn;
}

bool Debugger::getDebugMode()
{
	return debugModeOn_;
}

void Debugger::setPanelElementName(std::string name)
{
	panelElementName_ = name;
}

std::string Debugger::getPanelElementName()
{
	return panelElementName_;
}

void Debugger::setDebugLoggingOn(bool debugLoggingOn)
{
	debugLoggingOn_ = debugLoggingOn;
}

bool Debugger::getDebugLoggingOn()
{
	return debugLoggingOn_;
}

void Debugger::setDebugModeRenderHitboxes(bool renderHitboxes)
{
	renderHitboxes_ = renderHitboxes;
}

bool Debugger::getDebugModeRenderHitboxes()
{
	return renderHitboxes_;
}

void Debugger::setDebugModeBreakOnError(bool breakOnError)
{
	breakOnError_ = breakOnError;
}

bool Debugger::getDebugModeBreakOnError()
{
	return breakOnError_;
}

void Debugger::setDebugModeRenderDynamicsControllers(bool renderDynamicsControllers)
{
	renderDynamicsControllers_ = renderDynamicsControllers;
}

bool Debugger::getDebugModeRenderDynamicsControllers()
{
	return renderDynamicsControllers_;
}

void Debugger::setDebugModeRenderStage(bool renderStage)
{
	renderStage_ = renderStage;
}

bool Debugger::getDebugModeRenderStage()
{
	return renderStage_;
}

void Debugger::setLogColisions(bool logCollisions)
{
	collisionLogger->logCollisions = logCollisions;
}

bool Debugger::getLogColisions()
{
	return collisionLogger->logCollisions;
}

void Debugger::setDebugLogPath(std::string debugLogPath)
{
	debugLogPath_ = debugLogPath;
}

std::string Debugger::getDebugLogPath()
{
	return debugLogPath_;
}

void Debugger::appendToLogPy(std::string logMessage)
{
	PythonReleaseGil unlocker;

	appendToLog(logMessage);
}

void Debugger::appendToLog(std::string logMessage)
{
	if (debugModeOn_ == true && debugLoggingOn_ == true || true)
	{
		debugLog_ += "\n" + logMessage + "\n";
	}
}

void Debugger::clearLog()
{
	if (debugModeOn_ == true && debugLoggingOn_ == true)
	{
		debugLog_ = "";
	}
}

void Debugger::writeLogToFile()
{
	if (debugModeOn_ == true && debugLoggingOn_ == true)
	{
		int size = debugLogPath_.size();

		if (size > 0)
		{
			std::ofstream out;
	
			out.open(debugLogPath_.c_str()); 
	
			if (!out)
			{
				// Couldn't open file.
				return;
			}
    
			out<<debugLog_;
	
			out.close();
		}
	}
}


void Debugger::handlePythonError()
{
	try
	{
		PythonAcquireGil lock;

		PyObject *ptype, *pvalue, *ptraceback;
		PyErr_Fetch(&ptype, &pvalue, &ptraceback);

		std::cout << std::endl << "   *** Python Error ***" << std::endl << std::endl;

		if (pvalue != nullptr)
		{
			std::string errorType = boost::python::extract<std::string>(PyObject_Str(PyObject_Type(pvalue)));
			std::string errorDescription = boost::python::extract<std::string>(PyObject_Str(pvalue));
			
			if (errorType == "<class 'SyntaxError'>")
			{

				std::cout << "Fatal Error: " << errorDescription << std::endl;
				std::cout << "Press any key to exit." << std::endl;

				std::string notificationMessage = "Fatal Error: " + errorDescription + ".\nThe engine will now exit.";

				notificationManager_->displayNotification(notificationMessage);

				exit(-1);
			}
			else
			{
				//std::string errorDescription = boost::python::extract<std::string>(pvalue);

				std::string pythonCodeToExecute = "print(\"   Description: \" + str(value))\n";
				pythonCodeToExecute += "print(\"\")";

				object main_module = import("__main__");
				object main_namespace = main_module.attr("__dict__");

				handle<> hValue(pvalue);
				object exvalue(hValue);

				main_namespace["value"] = exvalue;

				str pyCode(pythonCodeToExecute);

				object obj = exec(pyCode, main_namespace);
			}
		}
		else
		{
			std::cout << "    Unable to retrieve python exception description." << std::endl;
		}

		if (ptraceback != nullptr)
		{
			// Write out the stack trace.
			std::string pythonCodeToExecute = "";
			pythonCodeToExecute += "print(\"   Stack Trace:\")\n";
			pythonCodeToExecute += "print(\"\")\n";
			pythonCodeToExecute += "currentTraceback = traceback\n";
			pythonCodeToExecute += "while currentTraceback != None:\n";
			pythonCodeToExecute += "   print(\"\")\n";
			pythonCodeToExecute += "   print(\"      File:        \" + str(currentTraceback.tb_frame.f_code.co_filename))\n";
			pythonCodeToExecute += "   print(\"      Function:    \" + str(currentTraceback.tb_frame.f_code.co_name))\n";
			pythonCodeToExecute += "   print(\"      Line Number: \" + str(currentTraceback.tb_lineno))\n";
			pythonCodeToExecute += "   print(\"\")\n";
			pythonCodeToExecute += "   currentTraceback = currentTraceback.tb_next";

			object main_module = import("__main__");
			object main_namespace = main_module.attr("__dict__");

			handle<> hTraceback(ptraceback);
			object exTraceback(hTraceback);
			
			main_namespace["traceback"] = exTraceback;

			str pyCode(pythonCodeToExecute);

			object obj = exec(pyCode, main_namespace);
		}
		else
		{
			std::cout << "   Unable to retrieve python exception stack trace." << std::endl;
		}

		std::cout << std::endl;

		if (breakOnError_ == true)
		{
			std::string temp;
			std::cin >> temp;
		}

	}
	catch (error_already_set &)
	{
		std::cout << "Unexpected error occurred while handling python exception." << std::endl;
	}
}
