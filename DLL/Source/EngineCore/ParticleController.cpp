#include "..\..\Headers\EngineCore\ParticleController.hpp"

using namespace firemelon;
using namespace boost::python;

ParticleController::ParticleController()
{
	deactivate_ = false;

	particleRenderable_ = nullptr;

	renderable_ = nullptr;
}

ParticleController::~ParticleController()
{
}

void ParticleController::createRenderables()
{
	particleRenderable_ = boost::shared_ptr<ParticleRenderable>(new ParticleRenderable());

	renderable_ = boost::static_pointer_cast<Renderable>(particleRenderable_);

	addRenderable(renderable_);
}

float ParticleController::getRotationAnglePy()
{
	PythonReleaseGil unlocker;

	return getRotationAngle();
}

float ParticleController::getRotationAngle()
{
	return renderable_->getRenderEffects()->getRotationOperation(0)->getAngle();
}

void ParticleController::setRotationAnglePy(float rotationAngle)
{
	PythonReleaseGil unlocker;

	setRotationAngle(rotationAngle);
}

void ParticleController::setRotationAngle(float rotationAngle)
{
	renderable_->getRenderEffects()->getRotationOperation(0)->setAngle(rotationAngle);

	particleMetadata_->reapplyTransform_ = true;
}

void ParticleController::setAnimationPy(std::string animationName, int framesPerSecond)
{
	PythonReleaseGil unlocker;

	setAnimation(animationName, framesPerSecond);
}

void ParticleController::setAnimation(std::string animationName, int framesPerSecond)
{
	AnimationManagerPtr animationManager = getAnimationManager();

	AssetId animationId = animationManager->getAnimationId(animationName);

	boost::shared_ptr<Animation> animation = animationManager->getAnimationByIndex(animationId);

	if (animation != nullptr)
	{
		particleMetadata_->reapplyTransform_ = true;

		particleRenderable_->setAnimation(animation, framesPerSecond);
	}
}

ColorRgbaPtr ParticleController::getHueColorPy()
{
	PythonReleaseGil unlocker;

	return getHueColor();
}

ColorRgbaPtr ParticleController::getHueColor()
{
	return particleRenderable_->getHueColor();
}

boost::shared_ptr<EntityMetadata> ParticleController::createMetadata()
{
	particleMetadata_ = boost::shared_ptr<ParticleMetadata>(new ParticleMetadata());

	return particleMetadata_;
}

void ParticleController::deactivatePy()
{
	PythonReleaseGil unlocker;

	deactivate();
}

void ParticleController::deactivate()
{
	deactivate_ = true;
}

void ParticleController::cleanup()
{
}

boost::shared_ptr<RenderEffects> ParticleController::getRenderEffectsPy()
{
	PythonReleaseGil unlocker;

	return getRenderEffects();
}

boost::shared_ptr<RenderEffects> ParticleController::getRenderEffects()
{
	return renderable_->getRenderEffects();
}

ParticleOrigin ParticleController::getOriginPy()
{
	PythonReleaseGil unlocker;

	return getOrigin();
}

ParticleOrigin ParticleController::getOrigin()
{
	return particleMetadata_->origin_;
}

void ParticleController::setOriginPy(ParticleOrigin origin)
{
	PythonReleaseGil unlocker;

	return setOrigin(origin);
}

void ParticleController::setOrigin(ParticleOrigin origin)
{
	particleMetadata_->origin_ = origin;
}