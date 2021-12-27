#include "..\Headers\FmodAudioResource.hpp"

FmodAudioResource::FmodAudioResource()
{
	sound_ = nullptr;
	channel_ = nullptr;
	channelGroup_ = nullptr;
	buffer_ = nullptr;
	groupName_ = "";
}

FmodAudioResource::~FmodAudioResource()
{
	//sound_->release();

	//if (buffer_ != nullptr)
	//{
	//	delete buffer_;
	//	buffer_ = nullptr;
	//}
}

void FmodAudioResource::setSound(FMOD::Sound* sound, char* buffer)
{
	sound_ = sound;
	buffer_ = buffer;
}

void FmodAudioResource::setSound(FMOD::Sound* sound)
{
	sound_ = sound;
}

FMOD::Sound* FmodAudioResource::getSound()
{
	return sound_;
}

void FmodAudioResource::setChannel(FMOD::Channel* channel)
{
	channel_ = channel;
}

FMOD::ChannelGroup* FmodAudioResource::getChannelGroup()
{
	return channelGroup_;
}

void FmodAudioResource::setChannelGroup(FMOD::ChannelGroup* channelGroup)
{
	channelGroup_ = channelGroup;
}

FMOD::Channel* FmodAudioResource::getChannel()
{
	return channel_;
}

std::string FmodAudioResource::getGroupName()
{
	return groupName_;
}

void FmodAudioResource::setGroupName(std::string groupName)
{
	groupName_ = groupName;
}
