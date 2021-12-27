///* -------------------------------------------------------------------------
//** AudioSource.hpp
//**
//** The AudioSource class is used to play environment audio, where distance 
//** from the source to the listener determines how it is played back.
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */

#ifndef _AUDIOSOURCE_HPP_
#define _AUDIOSOURCE_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "AudioPlayer.hpp"
#include "BaseIds.hpp"
#include "Position.hpp"

#include <string>

namespace firemelon
{	
	class FIREMELONAPI AudioSource
	{
	public:
		friend class AudioSourceContainer;
		friend class EntityController;
		friend class Room;

		AudioSource(std::string name, int x, int y);
		virtual ~AudioSource();

		bool						getIsPlaying();
		bool						getIsPlayingPy();

		void						play();
		void						playPy();

		void						stop();
		void						stopPy();

		std::string					getName();
		std::string					getNamePy();

		boost::shared_ptr<Position>	getPositionPy();
		boost::shared_ptr<Position>	getPosition();

	private:
		
		boost::shared_ptr<AudioPlayer>		audioPlayer_;

		boost::shared_ptr<Position>			position_;

		std::string		name_;
		
		RoomId			roomId_;
		int				roomIndex_;
		int				layerIndex_;
		int				audioIndex_;
		int				minDistance_;
		int				maxDistance_;
		bool			loop_;
		bool			autoplay_;
		double			volume_;
	};

	typedef boost::shared_ptr<AudioSource> AudioSourcePtr;
}

#endif // _AUDIOSOURCE_HPP_