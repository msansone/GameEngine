/* -------------------------------------------------------------------------
** FmodAudioResource.hpp
** 
** The FmodAudioResource class is a wrapper around an FMOD sound, and also
** contains additional data such as volume, channel, and group.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _FMODAUDIORESOURCE_HPP_
#define _FMODAUDIORESOURCE_HPP_

#include <string>

#include <fmod.hpp>
#include <fmod_errors.h>

class FmodAudioResource 
{
public:

	FmodAudioResource();
	virtual ~FmodAudioResource();
	
	void				setSound(FMOD::Sound* sound, char* buffer);
	void				setSound(FMOD::Sound* sound);
	FMOD::Sound*		getSound();

	void				setChannel(FMOD::Channel* channel);
	FMOD::Channel*		getChannel();

	std::string			getGroupName();
	void				setGroupName(std::string group);

	void				setChannelGroup(FMOD::ChannelGroup* channelGroup);
	FMOD::ChannelGroup*	getChannelGroup();

protected:

private:

	FMOD::Sound*		sound_;
	FMOD::Channel*		channel_;
	FMOD::ChannelGroup*	channelGroup_;

	char*				buffer_;
	std::string			groupName_;
};

#endif // _FMODAUDIORESOURCE_HPP_