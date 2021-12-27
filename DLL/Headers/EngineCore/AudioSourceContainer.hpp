/* -------------------------------------------------------------------------
** AudioSourceContainer.hpp
** 
** The AudioSourceContainer class provides a common interface to add,
** remove, and access the audio sources.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _AUDIOSOURCECONTAINER_HPP_
#define _AUDIOSOURCECONTAINER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "AudioSource.hpp"
#include "NameValidator.hpp"

#include <map>
#include <set>
#include <vector>

namespace firemelon
{
	class FIREMELONAPI AudioSourceContainer
	{
	public:
		friend class GameEngine;
		friend class Room;
		friend class RoomContainer;

		AudioSourceContainer();
		virtual ~AudioSourceContainer();

		int		addAudioSource(int roomIndex, boost::shared_ptr<AudioSource> audioSource);
		
		void	attachAudioSourceToEntity(std::string audioSourceName, int entityInstanceId);

		void	detachAudioSourceFromEntity(std::string audioSourceName);
		
		void	removeAudioSourcesAttachedToEntity(int entityInstanceId);
		
		void	changeAudioSourceNamesAttachedToEntity(int entityInstanceId, std::string name);

	private:

		void	addRoom(RoomId roomId);

		void	unloadRoomAudioSources(int roomIndex);

		boost::shared_ptr<NameValidator>							audioSourceNameValidator_;

		// Maps an audio source name to a room index.
		std::map<std::string, int>									audioSourceRoomIndexMap_;

		// The matrix of audio sources. The outer dimension is the room index.
		std::vector<std::vector<boost::shared_ptr<AudioSource>>>	audioSources_;

		// Maps an entity instance ID to a list of audio source names, to keep track of which
		// audio sources are attached to an entity.
		std::map<int, std::set<std::string>>						entityToAudioSourcesMap_;

		// Maps an audio source name to an entity which it is attached to.
		std::map<std::string, int>									audioSourceToEntityMap_;
	};
}

#endif // _AUDIOSOURCECONTAINER_HPP_