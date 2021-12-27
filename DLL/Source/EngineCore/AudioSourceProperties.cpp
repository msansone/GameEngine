#include "..\..\Headers\EngineCore\AudioSourceProperties.hpp"

using namespace firemelon;

AudioSourceProperties::AudioSourceProperties()
{
	BaseIds ids;

	x_ = 0;
	y_ = 0;
	layer_ = -1;
	audioId_ = ids.ASSET_NULL;
	name_ = "";
	autoplay_ = false;
	loop_ = false;
	maxDistance_ = 0.0;
	minDistance_ = 0.0;
	volume_ = 1.0;
}

AudioSourceProperties::~AudioSourceProperties()
{
}

int AudioSourceProperties::getXPy()
{
	PythonReleaseGil unlocker;

	return getX();
}

int AudioSourceProperties::getX()
{
	return x_;
}

void AudioSourceProperties::setXPy(int value)
{
	PythonReleaseGil unlocker;

	setX(value);
}

void AudioSourceProperties::setX(int value)
{
	x_ = value;
}

int AudioSourceProperties::getYPy()
{
	PythonReleaseGil unlocker;

	return getY();
}

int AudioSourceProperties::getY()
{
	return y_;
}

void AudioSourceProperties::setYPy(int value)
{
	PythonReleaseGil unlocker;

	setY(value);
}

void AudioSourceProperties::setY(int value)
{
	y_ = value;
}

int AudioSourceProperties::getLayerPy()
{
	PythonReleaseGil unlocker;

	return getLayer();
}

int AudioSourceProperties::getLayer()
{
	return layer_;
}

void AudioSourceProperties::setLayerPy(int value)
{
	PythonReleaseGil unlocker;

	setLayer(value);
}

void AudioSourceProperties::setLayer(int value)
{
	layer_ = value;
}

std::string AudioSourceProperties::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

std::string AudioSourceProperties::getName()
{
	return name_;
}

void AudioSourceProperties::setNamePy(std::string value)
{
	PythonReleaseGil unlocker;

	setName(value);
}

void AudioSourceProperties::setName(std::string value)
{
	name_ = value;
}

AssetId AudioSourceProperties::getAudioIdPy()
{
	PythonReleaseGil unlocker;

	return getAudioId();
}

AssetId AudioSourceProperties::getAudioId()
{
	return audioId_;
}

void AudioSourceProperties::setAudioIdPy(AssetId value)
{
	PythonReleaseGil unlocker;

	setAudioId(value);
}

void AudioSourceProperties::setAudioId(AssetId value)
{
	audioId_ = value;
}

bool AudioSourceProperties::getAutoplayPy()
{
	PythonReleaseGil unlocker;

	return getAutoplay();
}

bool AudioSourceProperties::getAutoplay()
{
	return autoplay_;
}

void AudioSourceProperties::setAutoplayPy(bool value)
{
	PythonReleaseGil unlocker;

	setAutoplay(value);
}

void AudioSourceProperties::setAutoplay(bool value)
{
	autoplay_ = value;
}

bool AudioSourceProperties::getLoopPy()
{
	PythonReleaseGil unlocker;

	return getLoop();
}

bool AudioSourceProperties::getLoop()
{
	return loop_;
}

void AudioSourceProperties::setLoopPy(bool value)
{
	PythonReleaseGil unlocker;

	setLoop(value);
}

void AudioSourceProperties::setLoop(bool value)
{
	loop_ = value;
}

float AudioSourceProperties::getMaxDistancePy()
{
	PythonReleaseGil unlocker;

	return getMaxDistance();
}

float AudioSourceProperties::getMaxDistance()
{
	return maxDistance_;
}

void AudioSourceProperties::setMaxDistancePy(float value)
{
	PythonReleaseGil unlocker;

	setMaxDistance(value);
}

void AudioSourceProperties::setMaxDistance(float value)
{
	maxDistance_ = value;
}

float AudioSourceProperties::getMinDistancePy()
{
	PythonReleaseGil unlocker;

	return getMinDistance();
}

float AudioSourceProperties::getMinDistance()
{
	return minDistance_;
}

void AudioSourceProperties::setMinDistancePy(float value)
{
	PythonReleaseGil unlocker;

	setMinDistance(value);
}

void AudioSourceProperties::setMinDistance(float value)
{
	minDistance_ = value;
}

float AudioSourceProperties::getVolumePy()
{
	PythonReleaseGil unlocker;

	return getVolume();
}

float AudioSourceProperties::getVolume()
{
	return volume_;
}

void AudioSourceProperties::setVolumePy(float value)
{
	PythonReleaseGil unlocker;

	setVolume(value);
}

void AudioSourceProperties::setVolume(float value)
{
	volume_ = value;
}