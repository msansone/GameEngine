#include "..\..\Headers\EngineCore\DynamicsController.hpp"

#include <iostream>
#include <sstream>

using namespace firemelon;

DynamicsController::DynamicsController()
{
	ownerStageHeight_ = 0;
	ownerStageWidth_ = 0;
	mass_ = 1.0;
	minVelocity_ = 60.0;
	ownerId_ = -1;
	attachedTo_ = nullptr;
	renderer_ = nullptr;
	
	look_ = boost::shared_ptr<Vec2<float>>(new Vec2<float>(0.0, 0.0));

	physicsSnapshot_ = boost::shared_ptr<PhysicsSnapshot>(new PhysicsSnapshot());
	physicsSnapshotPreviousStep_ = boost::shared_ptr<PhysicsSnapshot>(new PhysicsSnapshot());
	localPhysicsConfig_ = boost::shared_ptr<PhysicsConfig>(new PhysicsConfig());
	
	activePhysicsConfig_ = nullptr;
	globalPhysicsConfig_ = nullptr;

	attachmentAxis_ = AXIS_XY;

	isAffectedByGravity_ = false;

	useLocalPhysicsSettings_ = false;
}

DynamicsController::~DynamicsController()
{
}

void DynamicsController::initializePy()
{
	PythonReleaseGil unlocker;

	initialize();
}

void DynamicsController::initialize()
{
}

void DynamicsController::resetPy()
{
	PythonReleaseGil unlocker;

	reset();
}

void DynamicsController::reset()
{
	boost::shared_ptr<Vec2<float>>	acceleration = physicsSnapshot_->getAcceleration();
	boost::shared_ptr<Vec2<float>>	velocity = physicsSnapshot_->getVelocity();
	boost::shared_ptr<Vec2<float>>	movement = physicsSnapshot_->getMovement();
	boost::shared_ptr<Vec2<float>>	position = physicsSnapshot_->getPosition();
	boost::shared_ptr<Vec2<float>>	netForce = physicsSnapshot_->getNetForce();
	boost::shared_ptr<Vec2<int>>	positionInt = physicsSnapshot_->getPositionInt();
	boost::shared_ptr<Vec2<int>>	delta = physicsSnapshot_->getPositionIntDelta();
	
	boost::shared_ptr<Vec2<float>>	previousAcceleration = physicsSnapshotPreviousStep_->getAcceleration();
	boost::shared_ptr<Vec2<float>>	previousVelocity = physicsSnapshotPreviousStep_->getVelocity();
	boost::shared_ptr<Vec2<float>>	previousMovement = physicsSnapshotPreviousStep_->getMovement();
	boost::shared_ptr<Vec2<float>>	previousPosition = physicsSnapshotPreviousStep_->getPosition();
	boost::shared_ptr<Vec2<int>>	previousPositionInt = physicsSnapshotPreviousStep_->getPositionInt();

	previousAcceleration->setX(0.0);
	previousAcceleration->setY(0.0);

	previousVelocity->setX(0.0);
	previousVelocity->setY(0.0);

	previousMovement->setX(0.0);
	previousMovement->setY(0.0);

	previousPosition->setX(0.0);
	previousPosition->setY(0.0);

	previousPositionInt->setX(0.0);
	previousPositionInt->setY(0.0);
	
	acceleration->setX(0.0);
	acceleration->setY(0.0);

	velocity->setX(0.0);
	velocity->setY(0.0);
	
	position->setX(0.0);
	position->setY(0.0);
	
	positionInt->setX(0);
	positionInt->setY(0);
	
	netForce->setX(0);
	netForce->setY(0);

	movement->setX(0);
	movement->setY(0);

	delta->setX(0);
	delta->setY(0);

	position_->setX(0.0);
	position_->setY(0.0);
	
	position_->setPreviousX(0.0);
	position_->setPreviousY(0.0);
}

Renderer* DynamicsController::getRendererPy()
{
	PythonReleaseGil unlocker;

	return getRenderer().get();
}

boost::shared_ptr<Renderer> DynamicsController::getRenderer()
{
	return renderer_;
}

PhysicsConfig* DynamicsController::getLocalPhysicsConfigPy()
{
	PythonReleaseGil unlocker;

	return getLocalPhysicsConfig().get();
}

boost::shared_ptr<PhysicsConfig> DynamicsController::getLocalPhysicsConfig()
{
	return localPhysicsConfig_;
}

void DynamicsController::setIsAffectedByGravity(bool value)
{
	isAffectedByGravity_ = value;
}

void DynamicsController::setIsAffectedByGravityPy(bool value)
{
	PythonReleaseGil unlocker;

	setIsAffectedByGravity(value);
}

bool DynamicsController::getIsAffectedByGravity()
{
	return isAffectedByGravity_;
}

bool DynamicsController::getIsAffectedByGravityPy()
{
	PythonReleaseGil unlocker;

	return getIsAffectedByGravity();
}

void DynamicsController::integrate(double time)
{
	if (isAffectedByGravity_ == true)
	{
		applyForce(activePhysicsConfig_->getGravity());
	}

	float timeScale = globalPhysicsConfig_->getTimeScale();

	double time2 = time * timeScale;

	// Extract the config data.
	boost::shared_ptr<Vec2<float>>	minimumVelocity = activePhysicsConfig_->getMinimumVelocity();
	boost::shared_ptr<Vec2<float>>	linearDamp = activePhysicsConfig_->getLinearDamp();
	
	// Extract the snapshot data.
	boost::shared_ptr<Vec2<float>>	acceleration = physicsSnapshot_->getAcceleration();
	boost::shared_ptr<Vec2<float>>	velocity = physicsSnapshot_->getVelocity();
	boost::shared_ptr<Vec2<float>>	movement = physicsSnapshot_->getMovement();
	boost::shared_ptr<Vec2<float>>	position = physicsSnapshot_->getPosition();
	boost::shared_ptr<Vec2<float>>	netForce = physicsSnapshot_->getNetForce();
	boost::shared_ptr<Vec2<int>>	positionInt = physicsSnapshot_->getPositionInt();
	boost::shared_ptr<Vec2<int>>	delta = physicsSnapshot_->getPositionIntDelta();
	
	float mass = physicsSnapshot_->getMass();

	// Extract the snapshot data for the previous simulation step, and copy the data over before
	// processing this step.	
	boost::shared_ptr<Vec2<float>>	previousAcceleration = physicsSnapshotPreviousStep_->getAcceleration();
	boost::shared_ptr<Vec2<float>>	previousVelocity = physicsSnapshotPreviousStep_->getVelocity();
	boost::shared_ptr<Vec2<float>>	previousMovement = physicsSnapshotPreviousStep_->getMovement();
	boost::shared_ptr<Vec2<float>>	previousPosition = physicsSnapshotPreviousStep_->getPosition();
	boost::shared_ptr<Vec2<int>>	previousPositionInt = physicsSnapshotPreviousStep_->getPositionInt();

	previousAcceleration->setX(acceleration->getX());
	previousAcceleration->setY(acceleration->getY());
	previousVelocity->setX(velocity->getX());
	previousVelocity->setY(velocity->getY());
	previousMovement->setX(movement->getX());
	previousMovement->setY(movement->getY());
	previousPosition->setX(position->getX());
	previousPosition->setY(position->getY());
	previousPositionInt->setX(positionInt->getX());
	previousPositionInt->setY(positionInt->getY());
	
	int previousX = position->getX();
	int previousY = position->getY();

	// Update the physics snapshot for this step.
	acceleration->setX(netForce->getX() / mass);
	acceleration->setY(netForce->getY() / mass);
	
	velocity->setX(velocity->getX() + (acceleration->getX() * time2));
	velocity->setX(velocity->getX() + (velocity->getX() * (linearDamp->getX() * timeScale)));

	velocity->setY(velocity->getY() + (acceleration->getY() * time2));
	velocity->setY(velocity->getY() + (velocity->getY() * (linearDamp->getY() * timeScale)));

	float positionX = position->getX() + ((velocity->getX() + movement->getX()) * time2);
	float positionY = position->getY() + ((velocity->getY() + movement->getY()) * time2);

	position->setX(positionX);
	position->setY(positionY);
	
	// If the velocity is decreasing and is small enough, just zero it out.
	if (abs(velocity->getX()) < minimumVelocity->getX() && abs(previousVelocity->getX()) > abs(velocity->getX()))
	{
		velocity->setX(0.0);
	}

	if (abs(velocity->getY()) < minimumVelocity->getY() && abs(previousVelocity->getY()) > abs(velocity->getY()))
	{
		velocity->setY(0.0);
	}
	
	positionInt->setX((int)floor(positionX));
	positionInt->setY((int)floor(positionY));

	delta->setX(positionInt->getX() - previousPositionInt->getX());
	delta->setY(positionInt->getY() - previousPositionInt->getY());

	position_->setX(positionInt->getX());
	position_->setY(positionInt->getY());
	
	position_->setPreviousX(previousX);
	position_->setPreviousY(previousY);
}

void DynamicsController::userPostUpdate()
{
}

void DynamicsController::userPreUpdate()
{
}

void DynamicsController::updateAttachments()
{
	// If this dynamics controller is attached to another entity, shift it before
	// shifting the entities that are attached to it.

	if (attachedTo_ != nullptr)
	{
		// If the attachedTo entity is within the valid attachment bounds, then do the update, otherwise ignore.
		// This is to support cameras which can attach to entities, but don't update along the map edges.
		int attachedToX = attachedTo_->getPhysicsSnapshot()->getPositionInt()->getX();
		int positionX = getPhysicsSnapshot()->getPositionInt()->getX();
		int attachedToPreviousX = attachedTo_->getPhysicsSnapshotPrevious()->getPositionInt()->getX();
		
		
		// Can this be replaced with just getting the delta vector directly????
		// Come up with a test and try both to see if they work the same.
		// They might not because the delta is only calculated in the integration step, but not if the position is altered via collision or anything else.		
		int deltaX = attachedToX - attachedToPreviousX;
	
		float newPositionX = physicsSnapshot_->getPosition()->getX() + deltaX;
		
		int newPositionXInt = (int)floor(newPositionX);

		physicsSnapshot_->getPosition()->setX(newPositionX);
		physicsSnapshot_->getPositionInt()->setX(newPositionXInt);
		position_->setX(newPositionXInt);
		
		int attachedToY = attachedTo_->getPhysicsSnapshot()->getPositionInt()->getY();
		int positionY = getPhysicsSnapshot()->getPositionInt()->getY();
		int attachedToPreviousY = attachedTo_->getPhysicsSnapshotPrevious()->getPositionInt()->getY();

		int deltaY = attachedToY - attachedToPreviousY;

		float newPositionY = physicsSnapshot_->getPosition()->getY() + deltaY;

		int newPositionYInt = (int)floor(newPositionY);

		physicsSnapshot_->getPosition()->setY(newPositionY);
		physicsSnapshot_->getPositionInt()->setY(newPositionYInt);
		position_->setY(newPositionYInt);
	}
	
	int size = entitiesAttachedToThis_.size();
	
	for (int i = 0; i < size; i++)
	{
		entitiesAttachedToThis_[i]->updateAttachments();
	}
}

int DynamicsController::getOwnerIdPy()
{
	PythonReleaseGil unlocker;

	return getOwnerId();
}

int DynamicsController::getOwnerId()
{
	return ownerId_;
}

PhysicsSnapshot* DynamicsController::getPhysicsSnapshotPy()
{
	PythonReleaseGil unlocker;

	return getPhysicsSnapshot().get();
}

boost::shared_ptr<PhysicsSnapshot> DynamicsController::getPhysicsSnapshot()
{
	return physicsSnapshot_;
}

PhysicsSnapshot* DynamicsController::getPhysicsSnapshotPreviousPy()
{
	PythonReleaseGil unlocker;

	return getPhysicsSnapshotPrevious().get();
}

boost::shared_ptr<PhysicsSnapshot> DynamicsController::getPhysicsSnapshotPrevious()
{
	return physicsSnapshotPreviousStep_;
}

boost::shared_ptr<Vec2<float>> DynamicsController::getLookPy()
{
	PythonReleaseGil unlocker;

	return getLook();
}

boost::shared_ptr<Vec2<float>> DynamicsController::getLook()
{
	return look_;
}

void DynamicsController::setOwnerId(int id)
{
	ownerId_ = id;

	physicsSnapshot_->ownerId_ = id;
}

void DynamicsController::setPositionXPy(float value)
{
	PythonReleaseGil unlocker;

	setPositionX(value);
}

void DynamicsController::relocatePositionXPy(float value)
{
	PythonReleaseGil unlocker;

	relocatePositionX(value);
}

void DynamicsController::relocatePositionX(float value)
{
	setPositionXInternal(value, true, true);
}

void DynamicsController::setPositionX(float value)
{
	setPositionXInternal(value, false, true);
}

void DynamicsController::setPositionXInternal(float value, bool setPrevious, bool synchAttachments)
{
	//int previousX = position_->getX();
	float previousX = physicsSnapshot_->getPosition()->getX();

	//position_->setX(value);
	position_->setX((int)floor(value));

	physicsSnapshot_->getPosition()->setX(value);
	
	physicsSnapshot_->getPositionInt()->setX((int)floor(value));

	if (setPrevious == true)
	{
		// Set the previous x, so there is no weird graphical artifacts due to 
		// interpolation. Note: It may be the case that this causes unexpected
		// effects elsewhere. Keep this in mind, so that I don't think the
		// solution to this is to remove this code, because it is necessary.
		position_->setPreviousX((int)floor(value));

		physicsSnapshotPreviousStep_->getPosition()->setX(value);

		physicsSnapshotPreviousStep_->getPositionInt()->setX((int)floor(value));
	}

	if (synchAttachments == true)
	{
		// Any attached or previously attached dynamics controllers should be shifted to keep them in synch.	
		int deltaX = (int)floor(value) - (int)floor(previousX);

		int size = entitiesAttachedToThis_.size();

		for (int i = 0; i < size; i++)
		{
			int attachedToX = getPhysicsSnapshot()->getPositionInt()->getX();

			float positionX = entitiesAttachedToThis_[i]->getPhysicsSnapshot()->getPosition()->getX();
			positionX += deltaX;

			entitiesAttachedToThis_[i]->setPositionXInternal(positionX, setPrevious, synchAttachments);
		}
	}
}

float DynamicsController::getPositionXPy()
{
	PythonReleaseGil unlocker;

	return getPositionX();
}

float DynamicsController::getPositionX()
{
	return physicsSnapshot_->getPosition()->getX();
}

void DynamicsController::setPositionYPy(float value)
{
	PythonReleaseGil unlocker;

	setPositionY(value);
}

void DynamicsController::relocatePositionYPy(float value)
{
	PythonReleaseGil unlocker;

	relocatePositionY(value);
}

void DynamicsController::relocatePositionY(float value)
{
	setPositionYInternal(value, true, false);
}

void DynamicsController::setPositionY(float value)
{
	setPositionYInternal(value, false, false);
}

void DynamicsController::setPositionYInternal(float value, bool setPrevious, bool solidCollisionResolution)
{
	//int previousIntY = position_->getY();
	float previousY = physicsSnapshot_->getPosition()->getY();

	position_->setY((int)floor(value));

	//physicsSnapshot_->getPosition()->setY((float)value);
	physicsSnapshot_->getPosition()->setY(value);

	physicsSnapshot_->getPositionInt()->setY((int)floor(value));

	if (setPrevious == true)
	{
		// Set the previous y, so there is no weird graphical artifacts due to 
		// interpolation. Note: It may be the case that this causes unexpected
		// effects elsewhere. Keep this in mind, so that I don't think the
		// solution to this is to remove this code, because it is necessary.	
		position_->setPreviousY((int)floor(value));

		physicsSnapshotPreviousStep_->getPosition()->setY(value);

		physicsSnapshotPreviousStep_->getPositionInt()->setY((int)floor(value));
	}

	// Any attached or previously attached dynamics controllers should be shifted to keep them in synch.
	int deltaY = (int)floor(value) - (int)floor(previousY);

	int size = entitiesAttachedToThis_.size();

	for (int i = 0; i < size; i++)
	{
		int attachedToY = getPhysicsSnapshot()->getPositionInt()->getY();

		float positionY = entitiesAttachedToThis_[i]->getPhysicsSnapshot()->getPosition()->getY();

		positionY += deltaY;

		entitiesAttachedToThis_[i]->setPositionYInternal(positionY, setPrevious, solidCollisionResolution);
	}
}

float DynamicsController::getPositionYPy()
{
	PythonReleaseGil unlocker;

	return getPositionY();
}

float DynamicsController::getPositionY()
{
	return physicsSnapshot_->getPosition()->getY();
}

void DynamicsController::applyForcePy(float forceX, float forceY)
{
	PythonReleaseGil unlocker;

	applyForce(forceX, forceY);
}

void DynamicsController::applyForce(float forceX, float forceY)
{
	// Calculate net force.
	boost::shared_ptr<Vec2<float>> netForce = physicsSnapshot_->getNetForce();
	
	netForce->setX(netForce->getX() + forceX);
	netForce->setY(netForce->getY() + forceY);
}

void DynamicsController::applyForce(boost::shared_ptr<Vec2<float>> force)
{
	// Calculate net force.
	boost::shared_ptr<Vec2<float>> netForce = physicsSnapshot_->getNetForce();

	netForce->setX(netForce->getX() + force->getX());
	netForce->setY(netForce->getY() + force->getY());
}

void DynamicsController::applyImpulsePy(float impulseX, float impulseY)
{
	PythonReleaseGil unlocker;

	applyImpulse(impulseX, impulseY);
}

void DynamicsController::applyImpulse(float impulseX, float impulseY)
{
	// Add impulse to velocity.
	boost::shared_ptr<Vec2<float>> velocity = physicsSnapshot_->getVelocity();

	velocity->setX(velocity->getX() + impulseX);
	velocity->setY(velocity->getY() + impulseY);
}

void DynamicsController::clearForcesXPy()
{
	PythonReleaseGil unlocker;

	clearForcesX();
}

void DynamicsController::clearForcesX()
{
	boost::shared_ptr<Vec2<float>> netForce = physicsSnapshot_->getNetForce();
	
	netForce->setX(0.0);	
}

void DynamicsController::clearForcesYPy()
{
	PythonReleaseGil unlocker;

	clearForcesY();
}

void DynamicsController::clearForcesY()
{
	boost::shared_ptr<Vec2<float>> netForce = physicsSnapshot_->getNetForce();
	
	netForce->setY(0.0);
}

void DynamicsController::setMovementXPy(float value)
{
	PythonReleaseGil unlocker;

	setMovementX(value);
}

void DynamicsController::setMovementX(float value)
{
	physicsSnapshot_->getMovement()->setX(value);
}

void DynamicsController::setMovementYPy(float value)
{
	PythonReleaseGil unlocker;

	setMovementY(value);
}

void DynamicsController::setMovementY(float value)
{
	physicsSnapshot_->getMovement()->setY(value);
}

float DynamicsController::getMovementXPy()
{
	PythonReleaseGil unlocker;

	return getMovementX();
}

float DynamicsController::getMovementX()
{
	return physicsSnapshot_->getMovement()->getX();
}

float DynamicsController::getMovementYPy()
{
	PythonReleaseGil unlocker;

	return getMovementY();
}

float DynamicsController::getMovementY()
{
	return physicsSnapshot_->getMovement()->getY();
}

void DynamicsController::setLookXPy(float value)
{
	PythonReleaseGil unlocker;

	setLookX(value);
}

void DynamicsController::setLookX(float value)
{
	look_->setX(value);
}

void DynamicsController::setLookYPy(float value)
{
	PythonReleaseGil unlocker;

	setLookY(value);
}

void DynamicsController::setLookY(float value)
{
	look_->setY(value);
}

void DynamicsController::setVelocityXPy(float value)
{
	PythonReleaseGil unlocker;

	setVelocityX(value);
}

void DynamicsController::setVelocityX(float value)
{
	physicsSnapshot_->getVelocity()->setX(value);
}

void DynamicsController::setVelocityYPy(float value)
{
	PythonReleaseGil unlocker;

	setVelocityY(value);
}

void DynamicsController::setVelocityY(float value)
{
	physicsSnapshot_->getVelocity()->setY(value);
}

float DynamicsController::getVelocityXPy()
{
	PythonReleaseGil unlocker;

	return getVelocityX();
}

float DynamicsController::getVelocityX()
{
	return physicsSnapshot_->getVelocity()->getX();
}

float DynamicsController::getVelocityYPy()
{
	PythonReleaseGil unlocker;

	return getVelocityY();
}

float DynamicsController::getVelocityY()
{
	return physicsSnapshot_->getVelocity()->getY();
}

void DynamicsController::setAccelerationXPy(float value)
{
	PythonReleaseGil unlocker;

	setAccelerationX(value);
}

void DynamicsController::setAccelerationX(float value)
{
	physicsSnapshot_->getAcceleration()->setX(value);
}

void DynamicsController::setAccelerationYPy(float value)
{
	PythonReleaseGil unlocker;

	setAccelerationY(value);
}

void DynamicsController::setAccelerationY(float value)
{
	physicsSnapshot_->getAcceleration()->setY(value);
}

void DynamicsController::setPosition(boost::shared_ptr<Position> position)
{
	position_ = position;
	
	// Initialize position data.
	setPositionX(position_->getX());
	setPositionY(position_->getY());

	physicsSnapshotPreviousStep_->getPosition()->setX(position_->getX());
	physicsSnapshotPreviousStep_->getPosition()->setY(position_->getY());

	physicsSnapshotPreviousStep_->getPositionInt()->setX((int)floor(position_->getX()));
	physicsSnapshotPreviousStep_->getPositionInt()->setY((int)floor(position_->getY()));
}

boost::shared_ptr<Position> DynamicsController::getPosition()
{
	return position_;
}

void DynamicsController::setAttachedTo(DynamicsController* attachedTo)
{
	attachedTo_ = attachedTo;

	if (attachedTo != nullptr)
	{
		//std::cout << "Attaching " << ownerId_ << " to " << attachedTo->getOwnerId() << std::endl;

		// The entity that is being attached to needs to know that this entity is attaching to it.
		attachedTo_->addAttachedEntity(this);
	}
}

DynamicsController*	DynamicsController::getAttachedTo()
{
	return attachedTo_;
}

void DynamicsController::setAttachmentAxisPy(Axis axis)
{
	PythonReleaseGil unlocker;

	setAttachmentAxis(axis);
}

void DynamicsController::setAttachmentAxis(Axis axis)
{
	attachmentAxis_ = axis;
}

void DynamicsController::addAttachedEntity(DynamicsController* attachedEntity)
{
	// Only allow each attachee to be added once.
	int size = entitiesAttachedToThis_.size();

	for (int i = 0; i < entitiesAttachedToThis_.size(); i++)
	{
		if (entitiesAttachedToThis_[i]->getOwnerId() == attachedEntity->getOwnerId())
		{
			return;
		}
	}

	entitiesAttachedToThis_.push_back(attachedEntity);
}

void DynamicsController::removeAttachedEntity(DynamicsController* attachedEntity)
{
	int size = entitiesAttachedToThis_.size();

	for (int i = 0; i < entitiesAttachedToThis_.size(); i++)
	{
		if (entitiesAttachedToThis_[i]->getOwnerId() == attachedEntity->getOwnerId())
		{
			entitiesAttachedToThis_.erase(entitiesAttachedToThis_.begin() + i);

			break;
		}
	}
}

int	DynamicsController::getAttachedEntityCount()
{
	return entitiesAttachedToThis_.size();
}

DynamicsController*	DynamicsController::getAttachedEntity(int index)
{
	int size = entitiesAttachedToThis_.size();

	if (index >= 0 && index < size)
	{
		return entitiesAttachedToThis_[index];
	}
	else
	{
		return nullptr;
	}
}

void DynamicsController::setOwnerStageHeightPy(int value)
{
	PythonReleaseGil unlocker;

	setOwnerStageHeight(value);
}

void DynamicsController::setOwnerStageHeight(int value)
{
	ownerStageHeight_ = value;
}

int DynamicsController::getOwnerStageHeightPy()
{
	PythonReleaseGil unlocker;

	return getOwnerStageHeight();
}

int DynamicsController::getOwnerStageHeight()
{
	return ownerStageHeight_;
}

void DynamicsController::setOwnerStageWidthPy(int value)
{
	PythonReleaseGil unlocker;

	setOwnerStageWidth(value);
}

void DynamicsController::setOwnerStageWidth(int value)
{
	ownerStageWidth_ = value;
}

int DynamicsController::getOwnerStageWidthPy()
{
	PythonReleaseGil unlocker;

	return getOwnerStageWidth();
}

int DynamicsController::getOwnerStageWidth()
{
	return ownerStageWidth_;
}

void DynamicsController::preRender()
{

}

void DynamicsController::renderPy(int renderX, int renderY)
{
	PythonReleaseGil unlocker;

	render(renderX, renderY);
}

void DynamicsController::render(int renderX, int renderY)
{
	// Useful for debugging. Implement in child classes as needed.
}

DynamicsController* DynamicsController::getThisPy()
{
	PythonReleaseGil unlocker;

	return getThis();
}

DynamicsController* DynamicsController::getThis()
{
	return this;
}

bool DynamicsController::getUseLocalPhysicsSettingsPy()
{
	PythonReleaseGil unlocker;

	return getUseLocalPhysicsSettingsPy();
}

bool DynamicsController::getUseLocalPhysicsSettings()
{
	return useLocalPhysicsSettings_;
}

void DynamicsController::setUseLocalPhysicsSettingsPy(bool value)
{
	PythonReleaseGil unlocker;

	setUseLocalPhysicsSettings(value);
}

void DynamicsController::setUseLocalPhysicsSettings(bool value)
{
	useLocalPhysicsSettings_ = value;

	if (useLocalPhysicsSettings_ == true)
	{
		activePhysicsConfig_ = localPhysicsConfig_;
	}
	else
	{
		activePhysicsConfig_ = globalPhysicsConfig_;
	}
}

void DynamicsController::setGlobalPhysicsConfig(PhysicsConfigPtr physicsConfig)
{
	globalPhysicsConfig_ = physicsConfig;

	activePhysicsConfig_ = physicsConfig;

	// Copy the global settings to the local.
	localPhysicsConfig_->copy(globalPhysicsConfig_.get());
}