#include "..\Headers\FmodAudioPlayer.hpp"

using namespace firemelon;

FmodAudioPlayer::FmodAudioPlayer()
{
	listenerPosition_.x = 0.0;
	listenerPosition_.y = 0.0;
	listenerPosition_.z = 0.0;

	pixelsPerMeter_ = 64.0f;

	system_ = nullptr;
}

FmodAudioPlayer::~FmodAudioPlayer()
{
}
	
void FmodAudioPlayer::initialize()
{
	if (system_ == nullptr)
	{
		FMOD_RESULT result;

		// Create the main system object.
		result = FMOD::System_Create(&system_);

		if (result != FMOD_OK)
		{
			std::cout << "FMOD error! " << result << FMOD_ErrorString(result) << std::endl;

			return;
		}

		// Initialize FMOD.
		result = system_->init(512, FMOD_INIT_NORMAL, 0);
		if (result != FMOD_OK)
		{
			std::cout << "FMOD error! " << result << FMOD_ErrorString(result) << std::endl;

			return;
		}

		// Check version
		unsigned int version;

		result = system_->getVersion(&version);

		if (result != FMOD_OK)
		{
			std::cout << "FMOD error! " << result << FMOD_ErrorString(result) << std::endl;

			return;
		}

		if (version < FMOD_VERSION)
		{
			std::cout << std::hex << "Error! You are using an old version of FMOD " << version << ". This program requires " << FMOD_VERSION << std::endl;
			return;
		}


		//// Get number of sound cards 
		//int numDrivers;
		//FMOD_SPEAKERMODE speakerMode;
		//FMOD_CAPS caps;
		//char name[256];
		//result = system_->getNumDrivers(&numDrivers);

		//FMODErrorCheck(result);
	 //
		//// No sound cards (disable sound)
		//if (numDrivers == 0)
		//{
		//	result = system_->setOutput(FMOD_OUTPUTTYPE_NOSOUND);
		//	FMODErrorCheck(result);
		//}
		//else
		//{
		//	// Get the capabilities of the default (0) sound card
		//	result = system_->getDriverCaps(0, &caps, 0, &speakerMode);
		//	FMODErrorCheck(result);
	 //
		//	// Set the speaker mode to match that in Control Panel
		//	result = system_->setSpeakerMode(speakerMode);
		//	FMODErrorCheck(result);

		//	// Increase buffer size if user has Acceleration slider set to off
		//	if (caps & FMOD_CAPS_HARDWARE_EMULATED)
		//	{
		//		result = system_->setDSPBufferSize(1024, 10);
		//		FMODErrorCheck(result);
		//	}

		//	// Get name of driver
		//	result = system_->getDriverInfo(0, name, 256, 0);
		//	FMODErrorCheck(result);
	 //
		//	// SigmaTel sound devices crackle for some reason if the format is PCM 16-bit.
		//	// PCM floating point output seems to solve it.
		//	if (strstr(name, "SigmaTel"))
		//	{
		//		result = system_->setSoftwareFormat(48000, FMOD_SOUND_FORMAT_PCMFLOAT, 0, 0, FMOD_DSP_RESAMPLER_LINEAR);
		//		FMODErrorCheck(result);
		//	}
		//}

		//// Initialise FMOD
		//result = system_->init(100, FMOD_INIT_NORMAL, 0);

		//// If the selected speaker mode isn't supported by this sound card, switch it back to stereo
		//if (result == FMOD_ERR_OUTPUT_CREATEBUFFER)
		//{
		//	result = system_->setSpeakerMode(FMOD_SPEAKERMODE_STEREO);
		//	FMODErrorCheck(result);
	 //
		//	result = system_->init(100, FMOD_INIT_NORMAL, 0);
		//}
		//FMODErrorCheck(result);

		FMODErrorCheck(system_->createChannelGroup("Music", &channelMusic_));
		FMODErrorCheck(system_->createChannelGroup("Effect", &channelEffects_));

		system_->set3DSettings(1.0, pixelsPerMeter_, 1.0);
	}
}

void FmodAudioPlayer::shutdown()
{
	//int size = audioResources_.size();
	//for (int i = 0; i < size; i++)
	//{
	//	if (audioResources_[i] != nullptr)
	//	{
	//		delete audioResources_[i];
	//		audioResources_[i] = nullptr;
	//	}
	//}

	//system_->release();
}

void FmodAudioPlayer::update()
{
	listenerPosition_.x = getListenerX();
	listenerPosition_.y = getListenerY();
	listenerPosition_.z = 0.0;

	system_->set3DListenerAttributes(0, &listenerPosition_, nullptr, nullptr, nullptr);
    
	system_->update();

	
	//float audiblity = 0.0;
	//audioResources_[0]->getChannel()->getAudibility(&audiblity);

	//std::cout<<audiblity<<std::endl;
}

int FmodAudioPlayer::loadAudioResource(std::string audioname, char* buffer, int bufferSize, std::string groupName)
{
	FMOD::Sound* sound;

	FMOD_CREATESOUNDEXINFO audioInfo;
	memset(&audioInfo, 0, sizeof(FMOD_CREATESOUNDEXINFO));
	audioInfo.cbsize = sizeof(FMOD_CREATESOUNDEXINFO);
	audioInfo.length = static_cast<unsigned int>(bufferSize);
	audioInfo.suggestedsoundtype = FMOD_SOUND_TYPE_OGGVORBIS;

	//system_->setStreamBufferSize(65536, FMOD_TIMEUNIT_RAWBYTES); Do I still need this???

	FMODErrorCheck(system_->createSound(static_cast<const char*>(buffer), FMOD_3D | FMOD_3D_LINEARROLLOFF | FMOD_OPENMEMORY, &audioInfo, &sound));

	int id = audioResources_.size();

	audioNameIDMap_[audioname] = id;

	FmodAudioResource* newAudioResource = new FmodAudioResource();

	newAudioResource->setSound(sound, buffer);
	newAudioResource->setGroupName(groupName);

	if (groupName == "Music")
	{
		newAudioResource->setChannelGroup(channelMusic_);
	}
	else if (groupName == "Effects")
	{
		newAudioResource->setChannelGroup(channelEffects_);
	}
	//newAudioResource->setVolume(volume);
	//newAudioResource->setLoop(loop);

	audioResources_.push_back(newAudioResource);

	return id;
}

int FmodAudioPlayer::loadAudioResource(std::string audioname, std::string filename, std::string groupName)
{
	FMOD::Sound* sound;

	system_->createSound(filename.c_str(), FMOD_3D | FMOD_3D_LINEARROLLOFF, 0, &sound);

	int id = audioResources_.size();

	audioNameIDMap_[audioname] = id;
	
	FmodAudioResource* newAudioResource = new FmodAudioResource();

	newAudioResource->setSound(sound);
	newAudioResource->setGroupName(groupName);

	if (groupName == "Music")
	{
		newAudioResource->setChannelGroup(channelMusic_);
	}
	else if (groupName == "Effects")
	{
		newAudioResource->setChannelGroup(channelEffects_);
	}
	//newAudioResource->setVolume(volume);
	//newAudioResource->setLoop(loop);

	audioResources_.push_back(newAudioResource);

	return id;
}

void FmodAudioPlayer::playAudio(int audioId, float volume, bool loop)
{	
	if (volume > 1.0)
	{
		volume = 1.0;
	}
	else if (volume < 0.0)
	{
		volume = 0.0;
	}

	FMOD::Channel* channel;

	FMOD::Sound* sound = audioResources_[audioId]->getSound();

	FMOD::ChannelGroup* channelGroup = audioResources_[audioId]->getChannelGroup();

	FMODErrorCheck(system_->playSound(sound, channelGroup, false, &channel));

	channel->setVolume(volume);

	if (loop == true)
	{
		channel->setMode(FMOD_LOOP_NORMAL);
		channel->setLoopCount(-1);
	}

	channel->setPaused(false);

	audioResources_[audioId]->setChannel(channel);
}

void FmodAudioPlayer::playAudio(int audioId, float volume, bool loop, double x, double y, int minDistance, int maxDistance)
{	
	if (volume > 1.0)
	{
		volume = 1.0;
	}
	else if (volume < 0.0)
	{
		volume = 0.0;
	}

	FMOD::Channel* channel;
	
	//audioResources_[audioId]->getSound()->set3DMinMaxDistance((float)(minDistance / pixelsPerMeter_), (float)(maxDistance / pixelsPerMeter_));
	audioResources_[audioId]->getSound()->set3DMinMaxDistance((float)minDistance, (float)maxDistance);

	FMOD::Sound* sound = audioResources_[audioId]->getSound();

	FMOD::ChannelGroup* channelGroup = audioResources_[audioId]->getChannelGroup();

	FMODErrorCheck(system_->playSound(sound, channelGroup, false, &channel));

	//FMODErrorCheck(system_->playSound(FMOD_CHANNEL_FREE, audioResources_[audioId]->getSound(), true, &channel));
	//
	//if (audioResources_[audioId]->getGroup() == "Effect")
	//{
	//	channel->setChannelGroup(channelEffects_);
	//}
	//else if (audioResources_[audioId]->getGroup() == "Music")
	//{
	//	channel->setChannelGroup(channelMusic_);
	//}
	
	channel->setVolume(volume);

	if (loop == true)
	{
		channel->setMode(FMOD_LOOP_NORMAL);
		channel->setLoopCount(-1);
	}
	
	FMOD_VECTOR position;
	position.x = x;
	position.y = y;
	position.z = 0.0;
	
	channel->set3DAttributes(&position, nullptr);

	channel->setPaused(false);

	audioResources_[audioId]->setChannel(channel);
}

void FmodAudioPlayer::playMusicByNamePy(std::string introAudioName, std::string loopedAudioName, float volume)
{
	PythonReleaseGil unlocker;

	playMusicByName(introAudioName, loopedAudioName, volume);
}

void FmodAudioPlayer::playMusicByName(std::string introAudioName, std::string loopedAudioName, float volume)
{
	int introIndex = -1;
	
	if (audioNameIDMap_.count(introAudioName) > 0)
	{
		introIndex = audioNameIDMap_[introAudioName];
	}

	int loopedIndex = -1;

	if (audioNameIDMap_.count(loopedAudioName) > 0)
	{
		loopedIndex = audioNameIDMap_[loopedAudioName];
	}

	playMusic(introIndex, loopedIndex, volume);
}

void FmodAudioPlayer::playMusic(int introAudioId, int loopedAudioId, float volume)
{
	if (introAudioId >= 0 && loopedAudioId >= 0)
	{
		// Play the intro immediately and delay the looped audio.		
		if (volume > 1.0)
		{
			volume = 1.0;
		}
		else if (volume < 0.0)
		{
			volume = 0.0;
		}

		FMOD::Channel* introChannel;

		FMOD::Sound* introSound = audioResources_[introAudioId]->getSound();

		FMOD::ChannelGroup* introChannelGroup = audioResources_[introAudioId]->getChannelGroup();

		FMODErrorCheck(system_->playSound(introSound, introChannelGroup, false, &introChannel));

		introChannel->setVolume(volume);
		
		introChannel->setPaused(false);

		audioResources_[introAudioId]->setChannel(introChannel);

		// Now delay the looped sound.

		/*
		https://qa.fmod.com/t/fmod-low-level-playing-sounds-seamlessly-from-multiple-sound-files/12292
		If you’re using the low level API, you can seamlessly queue up sounds with Channel::setDelay.

		You need to start your first sound at a specific clock value, then calculate the length of the sound in output samples. this is outputsamples = sound_length_pcm / sound_samplerate * output_rate.

		Add that outputsample value to your original clock value to tell you when to start the 2nd sample.

		output_rate is 48khz usually for example (use System::getSoftwareFormat) and the sound sample rate is from Sound::getDefaults.

		https://fmod.com/resources/documentation-api?version=1.10&page=/content/generated/FMOD_ChannelControl_SetDelay.html

		dspclock_start:

		DSP clock of the parent channel group to audibly start playing sound at, a value of 0 indicates no delay.

		dspclock_end:

		DSP clock of the parent channel group to audibly stop playing sound at, a value of 0 indicates no delay.

		stopchannels:

		TRUE = stop according to ChannelControl::isPlaying. FALSE = remain 'active' and a new start delay could start playback again at a later time.

		Remarks
		Every channel and channel group has its own DSP Clock. A channel or channel group can be delayed relatively against its parent, with sample accurate positioning. To delay a sound, use the 'parent' channel group DSP clock to reference against when passing values into this function.

		If a parent channel group changes its pitch, the start and stop times will still be correct as the parent clock is rate adjusted by that pitch.
		*/


		// Calculate sound length in output samples.
		int sampleRate;
		FMOD_SPEAKERMODE speakerMode;
		int numRawSpeakers;

		system_->getSoftwareFormat(&sampleRate, &speakerMode, &numRawSpeakers);

		float frequency;
		int priority;

		introSound->getDefaults(&frequency, &priority);

		unsigned int length;

		introSound->getLength(&length, FMOD_TIMEUNIT_PCM);

		unsigned long long outputSamples = length / frequency * sampleRate;

		unsigned long long dspClock;

		unsigned long long parentClock;

		introChannelGroup->getDSPClock(&dspClock, &parentClock);

		FMOD::Channel* loopedChannel;

		FMOD::Sound* loopedSound = audioResources_[loopedAudioId]->getSound();

		FMOD::ChannelGroup* loopedChannelGroup = audioResources_[loopedAudioId]->getChannelGroup();

		FMODErrorCheck(system_->playSound(loopedSound, loopedChannelGroup, false, &loopedChannel));


		loopedChannel->setVolume(volume);

		loopedChannel->setDelay(dspClock + outputSamples, 0);

		loopedChannel->setMode(FMOD_LOOP_NORMAL);
		loopedChannel->setLoopCount(-1);
		
		loopedChannel->setPaused(false);

		audioResources_[loopedAudioId]->setChannel(loopedChannel);
	}
	else if (introAudioId == -1 && loopedAudioId >= 0)
	{
		// Play only the looped portion.
		playAudio(loopedAudioId, volume, true);
	}
}

void FmodAudioPlayer::stopAudio(int audioId)
{	
	FMOD::Channel* channel = audioResources_[audioId]->getChannel();
	channel->stop();	
}

void FmodAudioPlayer::stopAllAudio()
{	
	int size = audioResources_.size();

	for (int i = 0; i < size; i++)
	{
		FMOD::Channel* channel = audioResources_[i]->getChannel();
		channel->stop();
	}
}

void FmodAudioPlayer::setPauseAllAudio(bool isPaused)
{	
	int size = audioResources_.size();

	for (int i = 0; i < size; i++)
	{
		FMOD::Channel* channel = audioResources_[i]->getChannel();

		bool isPlaying = false;
		channel->isPlaying(&isPlaying);
		
		if (isPlaying == true)
		{
			channel->setPaused(isPaused);
		}
	}
}

bool FmodAudioPlayer::isAudioPlayingByName(std::string audioName)
{
	int index = audioNameIDMap_[audioName];

	return isAudioPlayingByIndex(index);
}

bool FmodAudioPlayer::isAudioPlayingByIndex(int audioIndex)
{
	int size = audioResources_.size();

	if (audioIndex < size)
	{
		if (audioResources_[audioIndex]->getChannel() != nullptr)
		{
			bool isPlaying;

			audioResources_[audioIndex]->getChannel()->isPlaying(&isPlaying);

			return isPlaying;
		}
		else
		{
			return false;
		}
	}
	else
	{
		return false;
	}
}

void FmodAudioPlayer::FMODErrorCheck(FMOD_RESULT result)
{
    if (result != FMOD_OK)
    {
        std::cout<<"FMOD error: "<<result <<" - "<<FMOD_ErrorString(result)<<std::endl;        
    }
}

void FmodAudioPlayer::setGroupVolume(std::string groupName, float volume)
{
	if (groupName == "Effect")
	{
		channelEffects_->setVolume(volume);
	}
	else if (groupName == "Music")
	{
		channelMusic_->setVolume(volume);
	}
}

double FmodAudioPlayer::getGroupVolume(std::string groupName)
{
	float volume = 0.0f;

	if (groupName == "Effect")
	{
		channelEffects_->getVolume(&volume);
	}
	else if (groupName == "Music")
	{
		channelMusic_->getVolume(&volume);
	}
	else
	{
		return 0.0;
	}

	return (double)volume;
}
