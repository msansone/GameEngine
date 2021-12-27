#include "..\..\Headers\EngineCore\ParticleEmitterEntityCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

ParticleEmitterEntityCodeBehind::ParticleEmitterEntityCodeBehind()
{
	automatic_ = false;
	attachParticles_ = false;
	interval_ = 5.0;
	maxParticles_ = 10;
	particleLifespan_ = 1.0;
	particlesPerEmission_ = 0;
	updateTimer_ = 0.0;
}

ParticleEmitterEntityCodeBehind::~ParticleEmitterEntityCodeBehind()
{
	bool debug = true;
}

void ParticleEmitterEntityCodeBehind::particleEmitted(boost::shared_ptr<ParticleEntityCodeBehind> p)
{
	particleEmitterScript_->particleEmitted(p);
}

void ParticleEmitterEntityCodeBehind::cleanup()
{
	while (inactiveParticles_.empty() == false)
	{
		inactiveParticles_.pop();
	}

	activeParticles_.clear();
}

boost::shared_ptr<ParticleEntityCodeBehind> ParticleEmitterEntityCodeBehind::getNextParticle()
{
	boost::shared_ptr<ParticleEntityCodeBehind> p = nullptr;

	if (inactiveParticles_.empty() == false)
	{
		// Get the next particle from the inactive list.
		p = inactiveParticles_.front();
	}

	return p;
}

boost::shared_ptr<ParticleEntityCodeBehind> ParticleEmitterEntityCodeBehind::getParticle(int index)
{
	boost::shared_ptr<ParticleEntityCodeBehind> p = nullptr;

	if (index >= 0 && index < particles_.size())
	{
		// Get the next particle from the inactive list.
		p = particles_[index];
	}

	return p;
}

int ParticleEmitterEntityCodeBehind::getActiveParticleCount()
{
	return activeParticles_.size();
}

int ParticleEmitterEntityCodeBehind::getParticleCount()
{
	return particles_.size();
}

void ParticleEmitterEntityCodeBehind::emit()
{
	for (int i = 0; i < particlesPerEmission_; i++)
	{
		boost::shared_ptr<ParticleEntityCodeBehind> p = activateAndGetParticle();

		if (p != nullptr)
		{
			emitParticle(p);
		}
	}
}

void ParticleEmitterEntityCodeBehind::emitParticle(boost::shared_ptr<ParticleEntityCodeBehind> p)
{
	// Do any pre-initialization necessary for the particle.
	boost::shared_ptr<ParticleController> particleController = boost::static_pointer_cast<ParticleController>(p->getEntityController());
	boost::shared_ptr<ParticleRenderable> particleRenderable = boost::static_pointer_cast<ParticleRenderable>(particleController->getRenderable());
	DynamicsController* dynamicsController = particleController->getDynamicsController();

	dynamicsController->reset();

	particleRenderable->reset();

	boost::shared_ptr<Position> position = particleEmitterController_->getPosition();

	dynamicsController->setPositionX((float)position->getX());
	dynamicsController->setPositionY((float)position->getY());

	dynamicsController->physicsSnapshotPreviousStep_->getPosition()->setX((float)position->getX());
	dynamicsController->physicsSnapshotPreviousStep_->getPosition()->setY((float)position->getX());

	particleController->getPosition()->setPreviousX((int)position->getX());
	particleController->getPosition()->setPreviousY((int)position->getY());

	p->preEmitted();

	particleEmitted(p);
}

boost::shared_ptr<ParticleEntityCodeBehind> ParticleEmitterEntityCodeBehind::activateAndGetParticle()
{
	boost::shared_ptr<ParticleEntityCodeBehind> p = nullptr;

	if (inactiveParticles_.empty() == false)
	{
		// Get the particle from the inactive list and remove it from said list.
		p = inactiveParticles_.front();

		p->activate();

		inactiveParticles_.pop();

		activeParticles_.push_back(p);
	}

	return p;
}

bool ParticleEmitterEntityCodeBehind::baseUpdate(double time)
{	
	// Iterate through the active particles, and if they have become deactivated, move them to the inactive queue.
	int activeParticleCount = activeParticles_.size();
	
	for (int i = activeParticleCount - 1; i >= 0 ; i--)
	{	
		// If the deactivate flag is set, force particle deactivation.
		if (activeParticles_[i]->particleController_->deactivate_ == true)
		{
			// Reset the deactivate flag to false.
			activeParticles_[i]->particleController_->deactivate_ = false;

			activeParticles_[i]->deactivate();
		}

		if (activeParticles_[i]->isActive_ == false)
		{
			// Move to inactive list.
			boost::shared_ptr<ParticleEntityCodeBehind> p = activeParticles_[i];
				
			inactiveParticles_.push(p);
	
			activeParticles_.erase(activeParticles_.begin() + i);
		}
	}
	
	// If in automatic mode, update the timer, and emit if the emission interval has been reached
	if (automatic_ == true)
	{
		updateTimer_ += time;

		if (updateTimer_ >= interval_)
		{
			updateTimer_ = 0.0;

			emit();
		}
	}

	return true;
}

void ParticleEmitterEntityCodeBehind::baseCleanup()
{
	particleEmitterController_->getNextParticleSignal_->disconnect(boost::bind(&ParticleEmitterEntityCodeBehind::getNextParticle, this));
	particleEmitterController_->getParticleSignal_->disconnect(boost::bind(&ParticleEmitterEntityCodeBehind::getParticle, this, _1));
	particleEmitterController_->getActiveParticleCountSignal_->disconnect(boost::bind(&ParticleEmitterEntityCodeBehind::getActiveParticleCount, this));
	particleEmitterController_->getParticleCountSignal_->disconnect(boost::bind(&ParticleEmitterEntityCodeBehind::getParticleCount, this));
	particleEmitterController_->emitSignal_->disconnect(boost::bind(&ParticleEmitterEntityCodeBehind::emit, this));
	particleEmitterController_->removedSignal_->disconnect(boost::bind(&ParticleEmitterEntityCodeBehind::removed, this));
	particleEmitterController_->roomChangedSignal_->disconnect(boost::bind(&ParticleEmitterEntityCodeBehind::roomChanged, this, _1, _2, _3, _4, _5, _6));
	particleEmitterController_->setIsAutomaticSignal_->disconnect(boost::bind(&ParticleEmitterEntityCodeBehind::setIsAutomatic, this, _1));

	particles_.clear();

	particleEmitterScript_->preCleanup();
}

void ParticleEmitterEntityCodeBehind::baseInitialize()
{
	particleEmitterScript_ = boost::shared_ptr<ParticleEmitterScript>(new ParticleEmitterScript(debugger_));

	particleEmitterController_ = boost::static_pointer_cast<ParticleEmitterController>(getEntityController());

	particleEmitterController_->getNextParticleSignal_->connect(boost::bind(&ParticleEmitterEntityCodeBehind::getNextParticle, this));
	particleEmitterController_->getParticleSignal_->connect(boost::bind(&ParticleEmitterEntityCodeBehind::getParticle, this, _1));
	particleEmitterController_->getActiveParticleCountSignal_->connect(boost::bind(&ParticleEmitterEntityCodeBehind::getActiveParticleCount, this));
	particleEmitterController_->getParticleCountSignal_->connect(boost::bind(&ParticleEmitterEntityCodeBehind::getParticleCount, this));
	particleEmitterController_->emitSignal_->connect(boost::bind(&ParticleEmitterEntityCodeBehind::emit, this));
	particleEmitterController_->removedSignal_->connect(boost::bind(&ParticleEmitterEntityCodeBehind::removed, this));
	particleEmitterController_->roomChangedSignal_->connect(boost::bind(&ParticleEmitterEntityCodeBehind::roomChanged, this, _1, _2, _3, _4, _5, _6));
	particleEmitterController_->setIsAutomaticSignal_->connect(boost::bind(&ParticleEmitterEntityCodeBehind::setIsAutomatic, this, _1));
	
	particleEmitterScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	particleEmitterScript_->controller_ = particleEmitterController_;

	particleEmitterScript_->preInitialize();
}

void ParticleEmitterEntityCodeBehind::initializeParticles()
{
	// This must be done on its own, after the dynamics controller has been initialzied, so the particles
	// can be attached if necessary.

	// Initialize the particles.
	int particleCount = particles_.size();

	for (int i = 0; i < particleCount; i++)
	{
		boost::shared_ptr<ParticleEntityCodeBehind> particle = particles_[i];

		particle->particleRenderable_->setIsVisible(false);

		// If the particles are attached to this emitter, set the attachment here.
		if (attachParticles_ == true)
		{
			particle->getEntityController()->attachTo(particleEmitterController_);
		}

		inactiveParticles_.push(particle);
	}
}

void ParticleEmitterEntityCodeBehind::roomChanged(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime)
{
	int size = particles_.size();
	for (int i = 0; i < size; i++)
	{
		//ChangeRoomParameters changeRoomParams;

		//changeRoomParams.roomId = roomId;
		//changeRoomParams.offsetX = offsetX;
		//changeRoomParams.offsetY = offsetY;
		//changeRoomParams.spawnPointId = spawnPoint;

		//ShowRoomParameters showRoomParams;
		//showRoomParams.roomId = getIds()->ROOM_NULL;

		particles_[i]->getEntityController()->changeRoom(roomId, spawnPoint, offsetX, offsetY, transitionId, transitionTime, false);
	}
}

void ParticleEmitterEntityCodeBehind::removed()
{
	int size = particles_.size();
	for (int i = 0; i < size; i++)
	{
		particles_[i]->getEntityController()->remove();
	}
}

void ParticleEmitterEntityCodeBehind::setIsAutomatic(bool value)
{
	automatic_ = value;
}