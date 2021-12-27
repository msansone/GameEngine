#include "..\..\Headers\EngineCore\GameTimer.hpp"

using namespace firemelon;

GameTimer::GameTimer()
{
	pingTime_ = 0;
}

GameTimer::~GameTimer()
{

}

double GameTimer::getTimeElapsedPy()
{
	PythonReleaseGil unlocker;

	return getTimeElapsed();
}

unsigned int GameTimer::getPingTimePy()
{
	PythonReleaseGil unlocker;

	return getPingTime();
}

unsigned int GameTimer::getPingTime()
{
	return pingTime_;
}

int GameTimer::addTimerPy()
{
	PythonReleaseGil unlocker;

	return addTimer();
}

int GameTimer::addTimer() 
{ 
	return -1; 
}

void GameTimer::logTimeStartPy(int timerId)
{
	PythonReleaseGil unlocker;

	logTimeStart(timerId);
}

void GameTimer::logTimeStart(int timerId) 
{
	// No-op.
}

double GameTimer::logTimeEndPy(int timerId) 
{
	PythonReleaseGil unlocker;

	return logTimeEnd(timerId);
}

double GameTimer::logTimeEnd(int timerId) 
{ 
	return 0.0; 
}

double GameTimer::getLoggedTimePy(int timerId) 
{
	PythonReleaseGil unlocker;

	return getLoggedTime(timerId);
}

double GameTimer::getLoggedTime(int timerId) 
{ 
	return 0.0; 
}

double GameTimer::getTimeSinceTick() 
{ 
	return 0.0; 
}

double GameTimer::getTimerElapsedPy(int timerId)
{
	PythonReleaseGil unlocker;

	return getTimerElapsed(timerId);
}

double GameTimer::getTimerElapsed(int timerId)
{
	return 0.0;
}