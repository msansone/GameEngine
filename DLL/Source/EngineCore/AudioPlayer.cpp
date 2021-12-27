#include "..\..\Headers\EngineCore\AudioPlayer.hpp"

using namespace firemelon;

AudioPlayer::AudioPlayer()
{
	listenerPosition_ = nullptr;
	listenerPositionOffset_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(0.0, 0.0));
}

AudioPlayer::~AudioPlayer()
{
}

void AudioPlayer::update()
{
}

void AudioPlayer::playAudioByNamePy(std::string audioName, float volume, bool loop)
{
	PythonReleaseGil unlocker;

	playAudioByName(audioName, volume, loop);
}

void AudioPlayer::playAudioByName(std::string audioName, float volume, bool loop)
{
	if (audioNameIDMap_.count(audioName) > 0)
	{
		int index = audioNameIDMap_[audioName];

		playAudio(index, volume, loop);
	}
}

void AudioPlayer::stopAudioByNamePy(std::string audioName)
{
	PythonReleaseGil unlocker;

	stopAudioByName(audioName);
}

void AudioPlayer::stopAudioByName(std::string audioName)
{
	if (audioNameIDMap_.count(audioName) > 0)
	{
		int index = audioNameIDMap_[audioName];

		stopAudio(index);
	}
}

void AudioPlayer::stopAllAudioPy()
{
	PythonReleaseGil unlocker;

	stopAllAudio();
}

void AudioPlayer::setPauseAllAudioPy(bool isPaused)
{
	PythonReleaseGil unlocker;

	setPauseAllAudio(isPaused);
}

void AudioPlayer::setGroupVolumePy(std::string groupName, float volume)
{
	PythonReleaseGil unlocker;

	setGroupVolume(groupName, volume);
}

void AudioPlayer::setGroupVolume(std::string groupName, float volume)
{
	// No-op.
}

double AudioPlayer::getGroupVolumePy(std::string groupName)
{
	PythonReleaseGil unlocker;

	return getGroupVolume(groupName);
}

double AudioPlayer::getGroupVolume(std::string groupName)
{
	return 0.0;
}

bool AudioPlayer::isAudioPlayingByIndexPy(int audioIndex)
{
	PythonReleaseGil unlocker;

	return isAudioPlayingByIndex(audioIndex);
}

bool AudioPlayer::isAudioPlayingByNamePy(std::string audioName)
{
	PythonReleaseGil unlocker;

	return isAudioPlayingByName(audioName);
}

int	AudioPlayer::translateAudioId(AssetId editorAudioId)
{
	return audioIdMap_[editorAudioId];
}

float AudioPlayer::getListenerX()
{
	if (listenerPosition_ != nullptr)
	{
		return (listenerPosition_->getX() + listenerPositionOffset_->getX());
	}

	return 0.0f;
}

float AudioPlayer::getListenerY()
{
	if (listenerPosition_ != nullptr)
	{
		return (listenerPosition_->getY() + listenerPositionOffset_->getX());
	}

	return 0.0f;
}