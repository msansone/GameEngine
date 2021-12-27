#include "..\..\Headers\EngineCore\ParticleEmitterController.hpp"

using namespace firemelon;
using namespace boost::python;

ParticleEmitterController::ParticleEmitterController() :
	getNextParticleSignal_(new boost::signals2::signal<boost::shared_ptr<ParticleEntityCodeBehind>()>),
	getParticleSignal_(new boost::signals2::signal<boost::shared_ptr<ParticleEntityCodeBehind>(int)>),
	getActiveParticleCountSignal_(new boost::signals2::signal<int()>),
	getParticleCountSignal_(new boost::signals2::signal<int()>),
	emitSignal_(new boost::signals2::signal<void()>),
	roomChangedSignal_(new boost::signals2::signal<void(RoomId, SpawnPointId, int, int, TransitionId, double)>),
	removedSignal_(new boost::signals2::signal<void()>),
	setIsAutomaticSignal_(new boost::signals2::signal<void(bool)>)
{
	particleHeight_ = 0;
	particleWidth_ = 0;
}

ParticleEmitterController::~ParticleEmitterController()
{
}

PyObj ParticleEmitterController::getParticlePy(int index)
{
	PythonReleaseGil unlocker;

	boost::shared_ptr<ParticleEntityCodeBehind> p = getParticle(index);

	if (p != nullptr)
	{
		return p->getPythonInstanceWrapper()->getPyInstance();
	}
	else
	{
		// Return None.
		return boost::python::object();
	}
}

boost::shared_ptr<ParticleEntityCodeBehind> ParticleEmitterController::getParticle(int index)
{
	boost::optional<boost::shared_ptr<ParticleEntityCodeBehind>> optionalParticle = (*getParticleSignal_)(index);

	boost::shared_ptr<ParticleEntityCodeBehind> p = optionalParticle.get();
	
	return p;
}

PyObj ParticleEmitterController::getNextParticlePy()
{
	PythonReleaseGil unlocker;

	boost::shared_ptr<ParticleEntityCodeBehind> p = getNextParticle();

	if (p != nullptr)
	{
		return p->getPythonInstanceWrapper()->getPyInstance();
	}
	else
	{
		// Return None.
		return boost::python::object();
	}
}

boost::shared_ptr<ParticleEntityCodeBehind> ParticleEmitterController::getNextParticle()
{
	boost::optional<boost::shared_ptr<ParticleEntityCodeBehind>> optionalParticle = (*getNextParticleSignal_)();

	boost::shared_ptr<ParticleEntityCodeBehind> p = optionalParticle.get();

	return p;
}

int ParticleEmitterController::getActiveParticleCountPy()
{
	PythonReleaseGil unlocker;

	return getActiveParticleCount();
}

int ParticleEmitterController::getActiveParticleCount()
{
	boost::optional<int> optionalInt = (*getActiveParticleCountSignal_)();

	int activeParticleCount = optionalInt.get();
	
	return activeParticleCount;
}

int ParticleEmitterController::getParticleCountPy()
{
	PythonReleaseGil unlocker;

	return getParticleCount();
}

int ParticleEmitterController::getParticleCount()
{
	boost::optional<int> optionalInt = (*getParticleCountSignal_)();

	int particleCount = optionalInt.get();

	return particleCount;
}

void ParticleEmitterController::emitPy()
{
	PythonReleaseGil unlocker;

	emit();
}

void ParticleEmitterController::emit()
{
	(*emitSignal_)();
}

int ParticleEmitterController::getParticleHeightPy()
{
	PythonReleaseGil unlocker;

	return getParticleHeight();
}

int ParticleEmitterController::getParticleHeight()
{
	return particleHeight_;
}

int ParticleEmitterController::getParticleWidthPy()
{
	PythonReleaseGil unlocker;

	return getParticleWidth();
}

int ParticleEmitterController::getParticleWidth()
{
	return particleWidth_;
}

void ParticleEmitterController::roomChanged(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime)
{
	// Signal is connected to the particle emitter code behind.
	(*roomChangedSignal_)(roomId, spawnPoint, offsetX, offsetY, transitionId, transitionTime);
}

void ParticleEmitterController::removed()
{
	// Signal is connected to the particle emitter code behind.
	(*removedSignal_)();
}


void ParticleEmitterController::setIsAutomaticPy(bool value)
{
	PythonReleaseGil unlocker;

	setIsAutomatic(value);
}

void ParticleEmitterController::setIsAutomatic(bool value)
{
	(*setIsAutomaticSignal_)(value);
}