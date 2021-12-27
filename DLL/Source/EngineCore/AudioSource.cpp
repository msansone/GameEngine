#include "..\..\Headers\EngineCore\AudioSource.hpp"

using namespace firemelon;

AudioSource::AudioSource(std::string name, int x, int y)
{
	BaseIds ids;

	position_ = boost::shared_ptr<Position>(new Position(x, y));

	name_ = name;
	roomId_ = ids.ROOM_NULL;
	roomIndex_ = -1;
	layerIndex_ = -1;
	audioIndex_ = -1;
	loop_ = false;
	autoplay_ = false;
	minDistance_ = 0.0;
	maxDistance_ = 0.0;
	volume_ = 1.0;
}

AudioSource::~AudioSource()
{
}

bool AudioSource::getIsPlaying()
{
	return audioPlayer_->isAudioPlayingByIndex(audioIndex_);
}

bool AudioSource::getIsPlayingPy()
{
	PythonReleaseGil unlocker;

	return getIsPlaying();
}

void AudioSource::playPy()
{
	PythonReleaseGil unlocker;

	play();
}

void AudioSource::play()
{
	audioPlayer_->playAudio(audioIndex_, volume_, loop_, position_->getX(), position_->getY(), minDistance_, maxDistance_);
}

void AudioSource::stopPy()
{
	PythonReleaseGil unlocker;

	stop();
}

void AudioSource::stop()
{
	audioPlayer_->stopAudio(audioIndex_);
}

std::string	AudioSource::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

std::string	AudioSource::getName()
{
	return name_;
}

boost::shared_ptr<Position> AudioSource::getPositionPy()
{
	PythonReleaseGil unlocker;

	return getPosition();
}

boost::shared_ptr<Position> AudioSource::getPosition()
{
	return position_;
}