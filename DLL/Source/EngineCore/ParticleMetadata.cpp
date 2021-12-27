#include "..\..\Headers\EngineCore\ParticleMetadata.hpp"

using namespace firemelon;

ParticleMetadata::ParticleMetadata()
{
	lifetime_ = 1.0;

	origin_ = PARTICLE_ORIGIN_CENTER;

	reapplyTransform_ = true;
}

ParticleMetadata::~ParticleMetadata()
{
}

double ParticleMetadata::getLifetimePy()
{
	PythonReleaseGil unlocker;

	return getLifetime();
}

double ParticleMetadata::getLifetime()
{
	return lifetime_;
}