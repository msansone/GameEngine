#include "..\..\Headers\EngineCore\ParticleEntityCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

ParticleEntityCodeBehind::ParticleEntityCodeBehind()
{
	isActive_ = false;	
	updateTimer_ = 0.0;
}

ParticleEntityCodeBehind::~ParticleEntityCodeBehind()
{
	bool debug = true;
}

void ParticleEntityCodeBehind::activate()
{
	particleController_->getRenderable()->setIsVisible(true);
	isActive_ = true;
}

void ParticleEntityCodeBehind::baseCleanup()
{
	particleScript_->preCleanup();
}

void ParticleEntityCodeBehind::baseInitialize()
{
	particleScript_ = boost::shared_ptr<ParticleScript>(new ParticleScript(debugger_));

	particleController_ = boost::static_pointer_cast<ParticleController>(getEntityController());
	particleRenderable_ = boost::static_pointer_cast<ParticleRenderable>(getEntityController()->getRenderable());

	particleScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	particleMetadata_ = boost::static_pointer_cast<ParticleMetadata>(getMetadata());

	particleScript_->controller_ = particleController_;
	particleScript_->particleMetadata_ = particleMetadata_;
	
	particleScript_->preInitialize();
}

bool ParticleEntityCodeBehind::baseUpdate(double time)
{
	if (isActive_ == true)
	{
		//particleRenderable_->updateRenderable(time);

		updateTimer_ += time;

		if (updateTimer_ >= particleMetadata_->lifetime_)
		{
			deactivate();
		}
	}

	return isActive_;
}

void ParticleEntityCodeBehind::deactivate()
{
	updateTimer_ = 0.0;

	preDeactivated();

	particleController_->getRenderable()->setIsVisible(false);

	isActive_ = false;
}

void ParticleEntityCodeBehind::emitted()
{
	particleScript_->emitted();
}

void ParticleEntityCodeBehind::preEmitted()
{
	emitted();
}

void ParticleEntityCodeBehind::preDeactivated()
{
	deactivated();
}

void ParticleEntityCodeBehind::deactivated()
{
	particleScript_->deactivated();
}