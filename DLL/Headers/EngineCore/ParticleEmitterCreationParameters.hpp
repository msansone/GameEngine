#ifndef _PARTICLEEMITTERCREATIONPARAMETERS_HPP_
#define _PARTICLEEMITTERCREATIONPARAMETERS_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include "Types.hpp"
#include "BaseIds.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI ParticleEmitterCreationParameters
	{
	public:
		friend class Room;
		friend class GameEngine;

		ParticleEmitterCreationParameters();
		virtual ~ParticleEmitterCreationParameters();
		
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
		
		RoomId				getRoomIdPy();
		RoomId				getRoomId();
		void				setRoomIdPy(RoomId value);
		void				setRoomId(RoomId value);
		
		ParticleEmitterId	getParticleEmitterIdPy();
		ParticleEmitterId	getParticleEmitterId();
		void				setParticleEmitterIdPy(ParticleEmitterId value);
		void				setParticleEmitterId(ParticleEmitterId value);

		ParticleId			getParticleIdPy();
		ParticleId			getParticleId();
		void				setParticleIdPy(ParticleId value);
		void				setParticleId(ParticleId value);

		int					getRenderOrderPy();
		int					getRenderOrder();
		void				setRenderOrderPy(int value);
		void				setRenderOrder(int value);

		int					getMaxParticlesPy();
		int					getMaxParticles();
		void				setMaxParticlesPy(int value);
		void				setMaxParticles(int value);

		int					getParticlesPerEmissionPy();
		int					getParticlesPerEmission();
		void				setParticlesPerEmissionPy(int value);
		void				setParticlesPerEmission(int value);

		double				getIntervalPy();
		double				getInterval();
		void				setIntervalPy(double value);
		void				setInterval(double value);

		double				getParticleLifespanPy();
		double				getParticleLifespan();
		void				setParticleLifespanPy(double value);
		void				setParticleLifespan(double value);

		bool				getAttachParticlesPy();
		bool				getAttachParticles();
		void				setAttachParticlesPy(bool value);
		void				setAttachParticles(bool value);

		bool				getAutomaticPy();
		bool				getAutomatic();
		void				setAutomaticPy(bool value);
		void				setAutomatic(bool value);

		std::string			getNamePy();
		std::string			getName();
		void				setNamePy(std::string value);
		void				setName(std::string value);

		std::string			getAnimationNamePy();
		std::string			getAnimationName();
		void				setAnimationNamePy(std::string value);
		void				setAnimationName(std::string value);

		int					getAnimationFramesPerSecondPy();
		int					getAnimationFramesPerSecond();
		void				setAnimationFramesPerSecondPy(int value);
		void				setAnimationFramesPerSecond(int value);

	private:
	
		int					x_;
		int					y_;
		int					layer_;
		int					renderOrder_;
		int					maxParticles_;
		int					particlesPerEmission_;

		bool				attachParticles_;
		bool				automatic_;

		double				emissionInterval_;
		double				particleLifespan_;

		RoomId				roomId_;
		ParticleEmitterId	particleEmitterTypeId_;
		ParticleId			particleTypeId_;

		std::string			name_;
		std::string			animationName_;
		int					animationFramesPerSecond_;
	};
}

#endif // _PARTICLEEMITTERCREATIONPARAMETERS_HPP_