#include "..\..\Headers\EngineCore\AudioSourceContainer.hpp"

using namespace firemelon;

AudioSourceContainer::AudioSourceContainer()
{
}

AudioSourceContainer::~AudioSourceContainer()
{
	int roomCount = audioSources_.size();

	for (int i = 0; i < roomCount; i++)
	{
		audioSources_[i].clear();
	}

	audioSourceRoomIndexMap_.clear();

	audioSources_.clear();
}

int AudioSourceContainer::addAudioSource(int roomIndex, boost::shared_ptr<AudioSource> audioSource)
{
	int roomCount = audioSources_.size();

	if (roomIndex >= 0 && roomIndex < roomCount)
	{
		audioSources_[roomIndex].push_back(audioSource);

		audioSourceRoomIndexMap_[audioSource->getName()] = roomIndex;

		return audioSources_.size();
	}

	return -1;
}

void AudioSourceContainer::attachAudioSourceToEntity(std::string audioSourceName, int entityInstanceId)
{
	// If this audio source is already attached to a different entity, detach it.
	detachAudioSourceFromEntity(audioSourceName);

	// Set the audio source attached to the given entity;
	entityToAudioSourcesMap_[entityInstanceId].insert(audioSourceName);

	audioSourceToEntityMap_[audioSourceName] = entityInstanceId;
}

void AudioSourceContainer::detachAudioSourceFromEntity(std::string audioSourceName)
{
	int attachedToEntityInstanceId = audioSourceToEntityMap_[audioSourceName];

	// Remove the audio name from the list of audio sources linked to this entity ID.
	entityToAudioSourcesMap_[attachedToEntityInstanceId].erase(audioSourceName);

	// Remove the mapping from this audio source name to an entity ID.
	audioSourceToEntityMap_.erase(audioSourceName);
}

void AudioSourceContainer::removeAudioSourcesAttachedToEntity(int entityInstanceId)
{
	std::set<std::string>::iterator it;

	for (it = entityToAudioSourcesMap_[entityInstanceId].begin(); it != entityToAudioSourcesMap_[entityInstanceId].end(); ++it)
	{
		std::string audioToRemoveName = *it;

		int audioRoomIndex = audioSourceRoomIndexMap_[audioToRemoveName];

		int size2 = audioSources_[audioRoomIndex].size();

		for (int j = 0; j < size2; j++)
		{
			if (audioSources_[audioRoomIndex][j]->getName() == audioToRemoveName)
			{
				audioSourceNameValidator_->removeName(audioToRemoveName);

				audioSourceRoomIndexMap_.erase(audioToRemoveName);

				audioSources_[audioRoomIndex].erase(audioSources_[audioRoomIndex].begin() + j);

				audioSourceToEntityMap_.erase(audioToRemoveName);

				break;
			}
		}
	}

	entityToAudioSourcesMap_[entityInstanceId].clear();
}

void AudioSourceContainer::changeAudioSourceNamesAttachedToEntity(int entityInstanceId, std::string name)
{
	std::set<std::string>::iterator it;

	std::vector<std::string> namesToAdd;
	std::vector<std::string> namesToRemove;

	for (it = entityToAudioSourcesMap_[entityInstanceId].begin(); it != entityToAudioSourcesMap_[entityInstanceId].end(); ++it)
	{
		std::string audioOldName = *it;

		int audioRoomIndex = audioSourceRoomIndexMap_[audioOldName];

		int size2 = audioSources_[audioRoomIndex].size();

		for (int j = 0; j < size2; j++)
		{
			if (audioSources_[audioRoomIndex][j]->getName() == audioOldName)
			{
				std::string audioNewName = audioOldName + name;

				if (audioSourceNameValidator_->isNameInUse(audioNewName) == false)
				{
					audioSourceNameValidator_->removeName(audioOldName);

					audioSourceNameValidator_->addName(audioNewName);

					// Change the name in the actual audio source.
					audioSources_[audioRoomIndex][j]->name_ = audioNewName;
					
					// Update the audio source name to room index map.
					int roomIndex = audioSourceRoomIndexMap_[audioOldName];

					audioSourceRoomIndexMap_.erase(audioOldName);

					audioSourceRoomIndexMap_[audioNewName] = roomIndex;

					// Need to store the names to add and remove from the list currently being iterated through
					// otherwise it will break it.
					namesToRemove.push_back(audioOldName);

					namesToAdd.push_back(audioNewName);

					// Update the map of the audio source name to entity which it is attached to.					
					audioSourceToEntityMap_.erase(audioOldName);

					audioSourceToEntityMap_[audioNewName] = entityInstanceId;
				}

				break;
			}
		}
	}
	
	// Now it is safe to update the set of audio sources attached to the given entity from the temp lists.
	int size = namesToAdd.size();

	for (int i = 0; i < size; i++)
	{
		entityToAudioSourcesMap_[entityInstanceId].erase(namesToRemove[i]);

		entityToAudioSourcesMap_[entityInstanceId].emplace(namesToAdd[i]);
	}

}

void AudioSourceContainer::addRoom(RoomId roomId)
{
	std::vector<boost::shared_ptr<AudioSource>> audioSourceList;

	audioSources_.push_back(audioSourceList);
}

void AudioSourceContainer::unloadRoomAudioSources(int roomIndex)
{
	int roomCount = audioSources_.size();

	if (roomIndex >= 0 && roomIndex < roomCount)
	{
		int size = audioSources_[roomIndex].size();

		for (int i = 0; i < size; i++)
		{
			std::string audioSourceName = audioSources_[roomIndex][i]->getName();

			audioSourceNameValidator_->removeName(audioSourceName);

			audioSourceRoomIndexMap_.erase(audioSourceName);
		}

		audioSources_[roomIndex].clear();
	}
}