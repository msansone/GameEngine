#ifndef _AUDIOSOURCECREATIONPROPERTIES_HPP_
#define _AUDIOSOURCECREATIONPROPERTIES_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/shared_ptr.hpp>

#include <string>

#include "BaseIds.hpp"
#include "PythonGil.hpp"
#include "Types.hpp"

namespace firemelon
{
	class FIREMELONAPI AudioSourceProperties
	{
	public:
		friend class Room;
		friend class GameEngine;

		AudioSourceProperties();
		virtual ~AudioSourceProperties();

		AssetId				getAudioId();
		AssetId				getAudioIdPy();
		void				setAudioId(AssetId value);
		void				setAudioIdPy(AssetId value);

		bool				getAutoplay();
		bool				getAutoplayPy();
		void				setAutoplay(bool value);
		void				setAutoplayPy(bool value);

		bool				getLoop();
		bool				getLoopPy();
		void				setLoop(bool value);
		void				setLoopPy(bool value);

		float				getMaxDistance();
		float				getMaxDistancePy();
		void				setMaxDistance(float value);
		void				setMaxDistancePy(float value);

		float				getMinDistance();
		float				getMinDistancePy();
		void				setMinDistance(float value);
		void				setMinDistancePy(float value);

		int					getXPy();
		int					getX();
		void				setXPy(int value);
		void				setX(int value);

		int					getYPy();
		int					getY();
		void				setYPy(int value);
		void				setY(int value);

		int					getLayerPy();
		int					getLayer();
		void				setLayerPy(int value);
		void				setLayer(int value);

		std::string			getNamePy();
		std::string			getName();
		void				setNamePy(std::string value);
		void				setName(std::string value);

		float				getVolume();
		float				getVolumePy();
		void				setVolume(float value);
		void				setVolumePy(float value);

	private:

		AssetId				audioId_;
		bool				autoplay_;
		int					layer_;
		bool				loop_;
		float				maxDistance_;
		float				minDistance_;
		std::string			name_;
		float				volume_;
		int					x_;
		int					y_;
	};

	typedef boost::shared_ptr<AudioSourceProperties> AudioSourcePropertiesPtr;
}

#endif // _AUDIOSOURCECREATIONPROPERTIES_HPP_