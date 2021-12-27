/* -------------------------------------------------------------------------
** ParticleEmitterController.hpp
**
** The ParticleEmitterController class is an intermediary interface between a c++
** particle emitter object, and the python object instance it contains. It is
** used to call the c++ functions from inside python script.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PARTICLEEMITTERCONTROLLER_HPP_
#define _PARTICLEEMITTERCONTROLLER_HPP_

#include <boost/python.hpp>
#include <boost/signals2.hpp>

#include "EntityController.hpp"
#include "ParticleEntityCodeBehind.hpp"
#include "PythonGil.hpp"

#include <queue>

namespace firemelon
{
	class ParticleEmitterController : public EntityController
	{
	public:
		friend class ParticleEmitterEntityCodeBehind;
		friend class Room;

		ParticleEmitterController();
		virtual ~ParticleEmitterController();

		PyObj										getNextParticlePy();
		boost::shared_ptr<ParticleEntityCodeBehind>	getNextParticle();

		PyObj										getParticlePy(int index);
		boost::shared_ptr<ParticleEntityCodeBehind>	getParticle(int index);

		int							getActiveParticleCountPy();
		int							getActiveParticleCount();

		int							getParticleCountPy();
		int							getParticleCount();

		// The dimensions of the particle renderable.
		int							getParticleHeightPy();
		int							getParticleHeight();

		int							getParticleWidthPy();
		int							getParticleWidth();

		void						emitPy();
		void						emit();

		void						setIsAutomaticPy(bool value);
		void						setIsAutomatic(bool value);

	protected:
		
		
	private:

		virtual void	removed();
		virtual void	roomChanged(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime);

		boost::shared_ptr<boost::signals2::signal<int()>>														getActiveParticleCountSignal_;
		boost::shared_ptr<boost::signals2::signal<int()>>														getParticleCountSignal_;
		boost::shared_ptr<boost::signals2::signal<boost::shared_ptr<ParticleEntityCodeBehind>()>>				getNextParticleSignal_;
		boost::shared_ptr<boost::signals2::signal<boost::shared_ptr<ParticleEntityCodeBehind>(int)>>			getParticleSignal_;
		boost::shared_ptr<boost::signals2::signal<void()>>														emitSignal_;
		boost::shared_ptr<boost::signals2::signal<void(RoomId, SpawnPointId, int, int, TransitionId, double)>>	roomChangedSignal_;
		boost::shared_ptr<boost::signals2::signal<void()>>														removedSignal_;
		boost::shared_ptr<boost::signals2::signal<void(bool)>>													setIsAutomaticSignal_;
		
		int particleHeight_;
		int particleWidth_;
	};
}

#endif // _PARTICLEEMITTERCONTROLLER_HPP_