/* -------------------------------------------------------------------------
** ParticleEmitterEntityCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PARTICLEEMITTERENTITYCODEBEHIND_HPP_
#define _PARTICLEEMITTERENTITYCODEBEHIND_HPP_

#include "AnimationManager.hpp"
#include "EntityCodeBehind.hpp"
#include "ParticleEmitterController.hpp"
#include "ParticleEmitterScript.hpp"
#include "ParticleEntityCodeBehind.hpp"
#include "Renderer.hpp"

#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp> 

namespace firemelon
{
	class ParticleEmitterEntityCodeBehind : public EntityCodeBehind
	{
	public:
		friend class CodeBehindFactory;
		friend class Room;

		ParticleEmitterEntityCodeBehind();
		virtual ~ParticleEmitterEntityCodeBehind();

	protected:

	private:

		boost::shared_ptr<ParticleEntityCodeBehind>	activateAndGetParticle();
		virtual void								baseCleanup();
		virtual void								baseInitialize();
		virtual bool								baseUpdate(double time);
		void										cleanup();
		void										emit();
		void										emitParticle(boost::shared_ptr<ParticleEntityCodeBehind> p);
		boost::shared_ptr<ParticleEntityCodeBehind>	getNextParticle();
		boost::shared_ptr<ParticleEntityCodeBehind>	getParticle(int);
		int											getActiveParticleCount();
		int											getParticleCount();
		void										initializeParticles();
		virtual void								particleEmitted(boost::shared_ptr<ParticleEntityCodeBehind> p);
		void										removed();
		void										roomChanged(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime);
		void										setIsAutomatic(bool value);

		std::vector<boost::shared_ptr<ParticleEntityCodeBehind>>	activeParticles_;
		AnimationManagerPtr											animationManager_;
		bool														attachParticles_;
		bool														automatic_;
		DynamicsController*											dynamicsController_;
		std::queue<boost::shared_ptr<ParticleEntityCodeBehind>>		inactiveParticles_;
		double														interval_;
		int															maxParticles_;
		AssetId														particleAnimationId_;
		boost::shared_ptr<ParticleEmitterController>				particleEmitterController_;
		boost::shared_ptr<ParticleEmitterScript>					particleEmitterScript_;
		double														particleLifespan_;
		ParticleId													particleTypeId_;
		std::vector<boost::shared_ptr<ParticleEntityCodeBehind>>	particles_;
		int															particlesPerEmission_;
		boost::shared_ptr<Renderer>									renderer_;
		double														updateTimer_;
	};
}

#endif // _PARTICLEEMITTERENTITYCODEBEHIND_HPP_
