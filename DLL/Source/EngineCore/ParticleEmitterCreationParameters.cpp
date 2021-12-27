#include "..\..\Headers\EngineCore\ParticleEmitterCreationParameters.hpp"

using namespace firemelon;

ParticleEmitterCreationParameters::ParticleEmitterCreationParameters()
{	
	BaseIds ids;

	x_ = 0;
	y_ = 0;
	layer_ = -1;
	renderOrder_ = 0;
	maxParticles_ = 0;
	particlesPerEmission_ = 0;
	emissionInterval_ = 0.0;
	particleLifespan_ = 0.0;
	roomId_ = ids.ROOM_NULL;
	particleEmitterTypeId_ = ids.PARTICLEEMITTER_NULL;
	particleTypeId_ = ids.PARTICLE_NULL;
	name_ = "";
	animationName_ = "";
	animationFramesPerSecond_ = 60;
	attachParticles_ = false;
	automatic_ = false;
}

ParticleEmitterCreationParameters::~ParticleEmitterCreationParameters()
{
}

int ParticleEmitterCreationParameters::getXPy()
{
	PythonReleaseGil unlocker;

	return getX();
}

int ParticleEmitterCreationParameters::getX()
{
	return x_;
}

void ParticleEmitterCreationParameters::setXPy(int value)
{
	PythonReleaseGil unlocker;

	setX(value);
}

void ParticleEmitterCreationParameters::setX(int value)
{
	x_ = value;
}
		
int ParticleEmitterCreationParameters::getYPy()
{
	PythonReleaseGil unlocker;

	return getY();
}

int ParticleEmitterCreationParameters::getY()
{
	return y_;
}

void ParticleEmitterCreationParameters::setYPy(int value)
{
	PythonReleaseGil unlocker;

	setY(value);
}

void ParticleEmitterCreationParameters::setY(int value)
{
	y_ = value;
}

int ParticleEmitterCreationParameters::getLayerPy()
{
	PythonReleaseGil unlocker;

	return getLayer();
}

int ParticleEmitterCreationParameters::getLayer()
{
	return layer_;
}

void ParticleEmitterCreationParameters::setLayerPy(int value)
{
	PythonReleaseGil unlocker;

	setLayer(value);
}

void ParticleEmitterCreationParameters::setLayer(int value)
{
	layer_ = value;
}

int ParticleEmitterCreationParameters::getRenderOrderPy()
{
	PythonReleaseGil unlocker;

	return getRenderOrder();
}

int ParticleEmitterCreationParameters::getRenderOrder()
{
	return renderOrder_;
}

void ParticleEmitterCreationParameters::setRenderOrderPy(int value)
{
	PythonReleaseGil unlocker;

	setRenderOrder(value);
}

void ParticleEmitterCreationParameters::setRenderOrder(int value)
{
	renderOrder_ = value;
}

std::string ParticleEmitterCreationParameters::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

std::string ParticleEmitterCreationParameters::getName()
{
	return name_;
}

void ParticleEmitterCreationParameters::setNamePy(std::string value)
{
	PythonReleaseGil unlocker;

	setName(value);
}

void ParticleEmitterCreationParameters::setName(std::string value)
{
	name_ = value;
}

std::string ParticleEmitterCreationParameters::getAnimationNamePy()
{
	PythonReleaseGil unlocker;

	return getAnimationName();
}

std::string ParticleEmitterCreationParameters::getAnimationName()
{
	return animationName_;
}

void ParticleEmitterCreationParameters::setAnimationNamePy(std::string value)
{
	PythonReleaseGil unlocker;

	setAnimationName(value);
}

void ParticleEmitterCreationParameters::setAnimationName(std::string value)
{
	animationName_ = value;
}


int ParticleEmitterCreationParameters::getAnimationFramesPerSecondPy()
{
	PythonReleaseGil unlocker;

	return getAnimationFramesPerSecond();
}

int ParticleEmitterCreationParameters::getAnimationFramesPerSecond()
{
	return animationFramesPerSecond_;
}

void ParticleEmitterCreationParameters::setAnimationFramesPerSecondPy(int value)
{
	PythonReleaseGil unlocker;

	setAnimationFramesPerSecond(value);
}

void ParticleEmitterCreationParameters::setAnimationFramesPerSecond(int value)
{
	animationFramesPerSecond_ = value;
}


RoomId ParticleEmitterCreationParameters::getRoomIdPy()
{
	PythonReleaseGil unlocker;

	return getRoomId();
}

RoomId ParticleEmitterCreationParameters::getRoomId()
{
	return roomId_;
}

void ParticleEmitterCreationParameters::setRoomIdPy(RoomId value)
{
	PythonReleaseGil unlocker;

	setRoomId(value);
}

void ParticleEmitterCreationParameters::setRoomId(RoomId value)
{
	roomId_ = value;
}

ParticleEmitterId ParticleEmitterCreationParameters::getParticleEmitterIdPy()
{
	PythonReleaseGil unlocker;

	return getParticleEmitterId();
}

ParticleEmitterId ParticleEmitterCreationParameters::getParticleEmitterId()
{
	return particleEmitterTypeId_;
}

void ParticleEmitterCreationParameters::setParticleEmitterIdPy(ParticleEmitterId value)
{
	PythonReleaseGil unlocker;

	setParticleEmitterId(value);
}

void ParticleEmitterCreationParameters::setParticleEmitterId(ParticleEmitterId value)
{
	particleEmitterTypeId_ = value;
}

ParticleId ParticleEmitterCreationParameters::getParticleIdPy()
{
	PythonReleaseGil unlocker;

	return getParticleId();
}

ParticleId ParticleEmitterCreationParameters::getParticleId()
{
	return particleTypeId_;
}

void ParticleEmitterCreationParameters::setParticleIdPy(ParticleId value)
{
	PythonReleaseGil unlocker;

	setParticleId(value);
}

void ParticleEmitterCreationParameters::setParticleId(ParticleId value)
{
	particleTypeId_ = value;
}

bool ParticleEmitterCreationParameters::getAutomaticPy()
{
	PythonReleaseGil unlocker;

	return getAutomatic();
}

bool ParticleEmitterCreationParameters::getAutomatic()
{
	return automatic_;
}

void ParticleEmitterCreationParameters::setAutomaticPy(bool value)
{
	PythonReleaseGil unlocker;

	setAutomatic(value);
}

void ParticleEmitterCreationParameters::setAutomatic(bool value)
{
	automatic_ = value;
}

bool ParticleEmitterCreationParameters::getAttachParticlesPy()
{
	PythonReleaseGil unlocker;

	return getAttachParticles();
}

bool ParticleEmitterCreationParameters::getAttachParticles()
{
	return attachParticles_;
}

void ParticleEmitterCreationParameters::setAttachParticlesPy(bool value)
{
	PythonReleaseGil unlocker;

	setAttachParticles(value);
}

void ParticleEmitterCreationParameters::setAttachParticles(bool value)
{
	attachParticles_ = value;
}

int ParticleEmitterCreationParameters::getMaxParticlesPy()
{
	PythonReleaseGil unlocker;

	return getMaxParticles();
}

int ParticleEmitterCreationParameters::getMaxParticles()
{
	return maxParticles_;
}

void ParticleEmitterCreationParameters::setMaxParticlesPy(int value)
{
	PythonReleaseGil unlocker;

	setMaxParticles(value);
}

void ParticleEmitterCreationParameters::setMaxParticles(int value)
{
	maxParticles_ = value;
}

int ParticleEmitterCreationParameters::getParticlesPerEmissionPy()
{
	PythonReleaseGil unlocker;

	return getParticlesPerEmission();
}

int ParticleEmitterCreationParameters::getParticlesPerEmission()
{
	return particlesPerEmission_;
}

void ParticleEmitterCreationParameters::setParticlesPerEmissionPy(int value)
{
	PythonReleaseGil unlocker;

	setParticlesPerEmission(value);
}

void ParticleEmitterCreationParameters::setParticlesPerEmission(int value)
{
	particlesPerEmission_ = value;
}

double ParticleEmitterCreationParameters::getIntervalPy()
{
	PythonReleaseGil unlocker;

	return getInterval();
}

double ParticleEmitterCreationParameters::getInterval()
{
	return emissionInterval_;
}

void ParticleEmitterCreationParameters::setIntervalPy(double value)
{
	PythonReleaseGil unlocker;

	setInterval(value);
}

void ParticleEmitterCreationParameters::setInterval(double value)
{
	emissionInterval_ = value;
}

double ParticleEmitterCreationParameters::getParticleLifespanPy()
{
	PythonReleaseGil unlocker;

	return getParticleLifespan();
}

double ParticleEmitterCreationParameters::getParticleLifespan()
{
	return particleLifespan_;
}

void ParticleEmitterCreationParameters::setParticleLifespanPy(double value)
{
	PythonReleaseGil unlocker;

	setParticleLifespan(value);
}

void ParticleEmitterCreationParameters::setParticleLifespan(double value)
{
	particleLifespan_ = value;
}