/* -------------------------------------------------------------------------
** FmodAudioPlayer.hpp
** 
** The FmodAudioPlayer class is derived from the base AudioPlayer class. It 
** uses the FMOD library to implement the virtual functions which load and 
** play audio resources.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _FMODAUDIOPLAYER_HPP_
#define _FMODAUDIOPLAYER_HPP_

#define WIN32

#include <string>
#include <vector>
#include <iostream>

#include <fmod.hpp>
#include <fmod_errors.h>

#include <AudioPlayer.hpp>
#include "FmodAudioResource.hpp"

class FmodAudioPlayer : public firemelon::AudioPlayer
{
public:

	FmodAudioPlayer();
	virtual ~FmodAudioPlayer();
	
	virtual void	initialize();
	virtual void	shutdown();
	virtual void	update();
	
	virtual int		loadAudioResource(std::string audioname, char* buffer, int bufferSize, std::string groupName);
	virtual int		loadAudioResource(std::string audioname, std::string filename, std::string groupName);

	virtual void	playAudio(int audioId, float volume, bool loop);

	virtual void	playAudio(int audioId, float volume, bool loop, double x, double y, int minDistance, int maxDistance);
	
	// Plays the non-looped intro, and then plays the looped portion immediately after the intro finishes.
	// If no intro is provided, it just plays the looped porition.
	void			playMusic(int introAudioId, int loopedAudioId, float volume);

	void			playMusicByName(std::string introAudioName, std::string loopedAudioName, float volume);

	void			playMusicByNamePy(std::string introAudioName, std::string loopedAudioName, float volume);

	virtual void	stopAudio(int audioId);
	
	virtual void	stopAllAudio();
	virtual void	setPauseAllAudio(bool isPaused);

	virtual bool	isAudioPlayingByName(std::string audioName);
	virtual bool	isAudioPlayingByIndex(int audioIndex);
	
	virtual void	setGroupVolume(std::string groupName, float volume);
	virtual double	getGroupVolume(std::string groupName);

protected:

private:

	void FMODErrorCheck(FMOD_RESULT result);

	std::vector<FmodAudioResource*> audioResources_;

	FMOD::ChannelGroup*				channelMusic_;
	FMOD::ChannelGroup*				channelEffects_;
	
	FMOD_VECTOR						listenerPosition_;

	float							pixelsPerMeter_;
	
	FMOD::System*					system_;

};

#endif // _FMODAUDIOPLAYER_HPP_