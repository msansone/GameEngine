#include "..\..\Headers\EngineCore\EngineConfig.hpp"

using namespace firemelon;

EngineConfig::EngineConfig()
{
	fpsLimiter_ = 0.0;
	interpolateFrames_ = true;	
}

EngineConfig::~EngineConfig()
{
}

double EngineConfig::getFpsLimiterPy()
{
	PythonReleaseGil unlocker;

	return getFpsLimiter();
}

double EngineConfig::getFpsLimiter()
{
	return fpsLimiter_;
}

void EngineConfig::setFpsLimiterPy(double value)
{
	PythonReleaseGil unlocker;

	setFpsLimiter(value);
}

void EngineConfig::setFpsLimiter(double value)
{
	if (value > 0.0)
	{
		fpsLimiter_ = 1.0 / value;
	}
	else
	{
		fpsLimiter_ = 0.0;
	}
}

bool EngineConfig::getInterpolateFramesPy()
{
	PythonReleaseGil unlocker;

	return getInterpolateFrames();
}

bool EngineConfig::getInterpolateFrames()
{
	return interpolateFrames_;
}

void EngineConfig::setInterpolateFramesPy(bool value)
{
	PythonReleaseGil unlocker;

	setInterpolateFrames(value);
}

void EngineConfig::setInterpolateFrames(bool value)
{
	interpolateFrames_ = value;
}