#include "..\Headers\BoostGameTimer.hpp"

using namespace firemelon;

BoostGameTimer::BoostGameTimer()
{
	timeScale_ = 1.0f;
	lockFps_ = 0.0f;
	minFrameTime_ = 0.0f;
	fpsTimeCount_ = 0.0f;
	deltaTime_ = 0.0;
	totalTime_ = 0.0;
	minimumDeltaTime_ = 0.0;
	maximumDeltaTime_ = 0.0;
	averageDeltaTime_ = 0.0;
	
	minimumTimer_ = 0.0;
	maximumTimer_ = 0.0;
	averageTimer_ = 0.0;

	fps_ = 0;
	fpsFrameCount_ = 0;
	frameCount_ = 0;

	writeFps_ = false;
}

BoostGameTimer::~BoostGameTimer()
{
}

double BoostGameTimer::getMinTimeElapsedPy()
{
	PythonReleaseGil unlocker;

	return getMinTimeElapsed();
}

double BoostGameTimer::getMinTimeElapsed()
{
	return minimumDeltaTime_;
}

double BoostGameTimer::getMaxTimeElapsedPy()
{
	PythonReleaseGil unlocker;

	return getMaxTimeElapsed();
}

double BoostGameTimer::getMaxTimeElapsed()
{
	return maximumDeltaTime_;
}

double BoostGameTimer::getAvgTimeElapsedPy()
{
	PythonReleaseGil unlocker;

	return getAvgTimeElapsed();
}

double BoostGameTimer::getAvgTimeElapsed()
{
	return averageDeltaTime_;
}

int BoostGameTimer::addTimer()
{
	logStartTime_.push_back(clock.now());
	loggedTimes_.push_back(0.0);
	return logStartTime_.size() - 1;
}

void BoostGameTimer::start()
{
	currentTime_ = clock.now();
	previousTime_ = clock.now();
}

double BoostGameTimer::tick()
{
	previousTime_ = currentTime_;

	frameCount_++;
	fpsFrameCount_++;
	
	currentTime_ = clock.now();

	durationDouble time_span = currentTime_ - previousTime_;

	//typedef boost::chrono::seconds sec;

	//sec d = boost::chrono::duration_cast<sec>(time_span);

	double deltaTime = time_span.count() * timeScale_;
	
	//fps_ = 1.0f / deltaTime_;

	// If the timespan is large, ignore it.
	if (deltaTime < 0.2)
	{
		deltaTime_ = deltaTime;

		totalTime_ += deltaTime_;
		fpsTimeCount_ += deltaTime_;

		averageTimer_ += deltaTime_;

		if (deltaTime_ < minimumTimer_)
		{
			minimumTimer_ = deltaTime_;
		}

		if (deltaTime_ > maximumTimer_)
		{
			maximumTimer_ = deltaTime_;
		}
		// Count the number of frames displayed each second.
		if (fpsTimeCount_ > 1.0f)
		{
			averageDeltaTime_ = averageTimer_ / fpsFrameCount_;
			averageTimer_ = 0;

			fps_ = fpsFrameCount_;

			if (writeFps_ == true)
			{
				std::cout << fps_ << std::endl;
			}

			fpsFrameCount_ = 0;
			fpsTimeCount_ = 0.0f;

			maximumDeltaTime_ = maximumTimer_;
			maximumTimer_ = 0.0;

			minimumDeltaTime_ = minimumTimer_;
			minimumTimer_ = 10000.0;
		}
	}

	return deltaTime_;
}

double BoostGameTimer::getTimeSinceTick()
{
	hiResDoubleTimePoint timePoint = clock.now();

	durationDouble time_span = timePoint - currentTime_;

	typedef boost::chrono::seconds sec;

	sec d = boost::chrono::duration_cast<sec>(time_span);

	double time = time_span.count() * timeScale_;

	return time;
}

void BoostGameTimer::frameComplete()
{
	
}

double BoostGameTimer::getTimeElapsedPy()
{
	PythonReleaseGil unlocker;

	return getTimeElapsed();
}

double BoostGameTimer::getTimeElapsed()
{
	return deltaTime_;
}

void BoostGameTimer::setLockFps(float lockFps)
{
	lockFps_ = lockFps;
	
	minFrameTime_ = 0.0f;

	if (lockFps_ < 0.0f)
	{
		lockFps_ = 0.0f;
	}
	else if (lockFps_ > 0.0f)
	{		
		minFrameTime_ = 1 / lockFps_;
	}
}

void BoostGameTimer::setTimeScale(float timeScale)
{
	timeScale_ = timeScale;
}

int	BoostGameTimer::getFpsPy()
{
	PythonReleaseGil unlocker;

	return getFps();
}

int	BoostGameTimer::getFps()
{
	return fps_;
}

double BoostGameTimer::getTotalTimePy()
{
	PythonReleaseGil unlocker;

	return getTotalTime();
}

double BoostGameTimer::getTotalTime()
{
	return totalTime_;
}

void BoostGameTimer::logTimeStart(int timerId)
{
	int size = logStartTime_.size();

	if (timerId >= 0 && timerId < size)
	{
		logStartTime_[timerId] = clock.now();
	}
}

double BoostGameTimer::logTimeEnd(int timerId)
{	
	int size = logStartTime_.size();

	if (timerId >= 0 && timerId < size)
	{
		durationDouble time_span =  clock.now() - logStartTime_[timerId];

		loggedTimes_[timerId] =  time_span.count();

		return time_span.count();
	}
	else
	{
		return 0.0;
	}
}

double BoostGameTimer::getLoggedTimePy(int timerId)
{
	PythonReleaseGil unlocker;

	return getLoggedTime(timerId);
}

double BoostGameTimer::getLoggedTime(int timerId)
{	
	int size = loggedTimes_.size();

	if (timerId >= 0 && timerId < size)
	{
		return loggedTimes_[timerId];
	}
}

double BoostGameTimer::getTimerElapsedPy(int timerId)
{
	PythonReleaseGil unlocker;

	return getTimerElapsed(timerId);
}

double BoostGameTimer::getTimerElapsed(int timerId)
{
	int size = logStartTime_.size();

	if (timerId >= 0 && timerId < size)
	{
		durationDouble time_span = clock.now() - logStartTime_[timerId];
		
		return time_span.count();
	}
	else
	{
		return 0.0;
	}
}