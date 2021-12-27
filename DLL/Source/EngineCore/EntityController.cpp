#include "..\..\Headers\EngineCore\EntityController.hpp"

using namespace firemelon;
using namespace boost::python;


EntityController::EntityController() :
	changeNameSignal_(new ChangeNameSignalRaw),
	changeRoomSignal_(new ChangeRoomSignalRaw),
	removeEntitySignal_(new RemoveEntitySignalRaw)
{
	animationManager_ = nullptr;
	attachedTo_ = nullptr;
	audioPlayer_ = nullptr;
	inputDeviceManager_ = nullptr;
	renderer_ = nullptr;
	stageController_ = nullptr;
	stateMachineController_ = nullptr;
	textManager_ = nullptr;

	renderablesCreated_ = false;

	Debugger::entityControllerCount++;
}

EntityController::~EntityController()
{
	Debugger::entityControllerCount--;
}

void EntityController::attachAudioSourcePy(AudioSourcePtr audioSource)
{
	PythonReleaseGil unlocker;

	attachAudioSource(audioSource);
}

void EntityController::attachAudioSource(AudioSourcePtr audioSource)
{
	if (audioSource != nullptr)
	{
		std::string audioSourceNameToAttach = audioSource->getName();

		audioSourceContainer_->attachAudioSourceToEntity(audioSourceNameToAttach, metadata_->getEntityInstanceId());
	}
}

void EntityController::detachAudioSourcePy(AudioSourcePtr audioSource)
{
	PythonReleaseGil unlocker;

	detachAudioSource(audioSource);
}

void EntityController::detachAudioSource(AudioSourcePtr audioSource)
{
	if (audioSource != nullptr)
	{
		std::string audioSourceNameToDetach = audioSource->getName();

		audioSourceContainer_->detachAudioSourceFromEntity(audioSourceNameToDetach);
	}
}

boost::shared_ptr<EntityMetadata> EntityController::getMetadata()
{
	return metadata_;
}

void EntityController::setPosition(int x, int y)
{
	if (dynamicsControllerHolder_->getDynamicsController() != nullptr)
	{
		dynamicsControllerHolder_->getDynamicsController()->setPositionX(x);
		dynamicsControllerHolder_->getDynamicsController()->setPositionY(y);
	}
	else
	{
		position_->setX(x);
		position_->setY(y);
	}
}

boost::shared_ptr<AnchorPointManager> EntityController::getAnchorPointManager()
{
	return anchorPointManager_;
}

AnimationManagerPtr EntityController::getAnimationManager()
{
	return animationManager_;
}

DebuggerPtr EntityController::getDebugger()
{
	return debugger_;
}

boost::shared_ptr<InputDeviceManager> EntityController::getInputDeviceManager()
{
	return inputDeviceManager_;
}

boost::shared_ptr<HitboxManager> EntityController::getHitboxManager()
{
	return hitboxManager_;
}

boost::shared_ptr<Renderer> EntityController::getRenderer()
{
	return renderer_;
}

boost::shared_ptr<RoomMetadataContainer> EntityController::getRoomMetadataContainer()
{
	return roomMetadataContainer_;
}

boost::shared_ptr<GameTimer> EntityController::getTimer()
{
	return timer_;
}

DynamicsController* EntityController::getDynamicsController()
{
	return dynamicsControllerHolder_->getDynamicsController();
}

boost::shared_ptr<AudioPlayer> EntityController::getAudioPlayer()
{
	return audioPlayer_;
}

boost::shared_ptr<TextManager> EntityController::getTextManager()
{
	return textManager_;
}

boost::shared_ptr<Messenger> EntityController::getMessenger()
{
	return messenger_;
}

boost::shared_ptr<QueryManager> EntityController::getQueryManager()
{
	return queryManager_;
}

boost::shared_ptr<QueryResultFactory> EntityController::getQueryResultFactory()
{
	return queryResultFactory_;
}

boost::shared_ptr<QueryParametersFactory> EntityController::getQueryParametersFactory()
{
	return queryParametersFactory_;
}

void EntityController::changeRoomPy(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime, bool showRoomAfterMove)
{
	PythonReleaseGil unlocker;

	changeRoom(roomId, spawnPoint, offsetX, offsetY, transitionId, transitionTime, showRoomAfterMove);
}

void EntityController::changeRoom(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime, bool showRoomAfterMove)
{
	if (roomId != metadata_->getRoomMetadata()->getRoomId())
	{
		ShowRoomParameters showRoomParams;

		if (showRoomAfterMove == true)
		{
			showRoomParams.roomId = roomId;
		}
		else
		{
			showRoomParams.roomId = ids_->ROOM_NULL;
		}

		showRoomParams.transitionId = transitionId;
		showRoomParams.transitionTime = transitionTime;

		ChangeRoomParameters changeRoomParams;
		changeRoomParams.roomId = roomId;
		changeRoomParams.spawnPointId = spawnPoint;
		changeRoomParams.offsetX = offsetX;
		changeRoomParams.offsetY = offsetY;
		changeRoomParams.delayUntilRoomShown = showRoomAfterMove;

		(*changeRoomSignal_)(changeRoomParams, showRoomParams);

		// Need to change room for all attached entities as well.	
		int attachedEntityCount = attachedEntities_.size();

		for (int i = 0; i < attachedEntityCount; i++)
		{
			// Change the offset so that it remains attached in the same spot.
			int attachmentOffsetX = attachedEntities_[i]->getPosition()->getX() - getPosition()->getX();
			int attachmentOffsetY = attachedEntities_[i]->getPosition()->getY() - getPosition()->getY();

			attachedEntities_[i]->changeRoom(roomId, spawnPoint, offsetX + attachmentOffsetX, offsetY + attachmentOffsetY, transitionId, transitionTime, showRoomAfterMove);
		}

		roomChanged(roomId, spawnPoint, offsetX, offsetY, transitionId, transitionTime);
	}
}

int EntityController::changeNamePy(std::string name, bool append)
{
	PythonReleaseGil unlocker;

	return changeName(name, append);
}

int EntityController::changeName(std::string name, bool append)
{
	boost::optional<int> optionalResultCode = (*changeNameSignal_)(name, append);
	
	int resultCode = optionalResultCode.get();

	if (append == true)
	{
		// Need to change name for all attached entities as well.	
		int attachedEntityCount = attachedEntities_.size();

		for (int i = 0; i < attachedEntityCount; i++)
		{
			attachedEntities_[i]->changeName(name, append);
		}

		audioSourceContainer_->changeAudioSourceNamesAttachedToEntity(metadata_->getEntityInstanceId(), name);
	}
	
	return resultCode;
}

void EntityController::roomChanged(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime)
{
	return;
}

void EntityController::removePy()
{
	PythonReleaseGil unlocker;

	remove();
}

void EntityController::remove()
{
	(*removeEntitySignal_)();

	audioSourceContainer_->removeAudioSourcesAttachedToEntity(metadata_->getEntityInstanceId());
	
	// Need to remove all attached entities as well.
	// Start from the end of the list, as it will be shrinking from recursive detachment.
	int attachedEntityCount = attachedEntities_.size();

	for (int i = attachedEntityCount - 1; i >= 0; i--)
	{
		attachedEntities_[i]->remove();
	}

	removed();

	detach();
}

void EntityController::removed()
{
	// No-op.
	return;
}

boost::shared_ptr<Position> EntityController::getPosition()
{
	return position_;
}

bool EntityController::cyclicalAttachmentExists(int entityInstanceId)
{
	if (metadata_->entityInstanceId_ == entityInstanceId)
	{
		return true;
	}
	else
	{
		// Go through the attached entities. If the owner ID is found for any, return true.
		bool cyclicalAttachmentFound = false;

		int size = attachedEntities_.size();

		for (int i = 0; i < size; i++)
		{
			boost::shared_ptr<EntityController> currentEntityController = attachedEntities_[i];

			cyclicalAttachmentFound = attachedEntities_[i]->cyclicalAttachmentExists(entityInstanceId);

			if (cyclicalAttachmentFound == true)
			{
				break;
			}
		}

		return cyclicalAttachmentFound;
	}
}

void EntityController::attachToPy(boost::shared_ptr<EntityController> attachedTo)
 {
	PythonReleaseGil unlocker;

	attachTo(attachedTo);
}

void EntityController::attachTo(boost::shared_ptr<EntityController> attachedTo)
{
	// Attach this entity controller to another one. When the attached to entity controller moves,
	// This one will get shifted by its delta change.
	// An entity can be attached to only one other entity, while many entities can be attached to a single entity entity.	
	if (attachedTo != nullptr)
	{
		// Make sure that this will not create a cyclical attachment dependency. 
		// If it does, log it to the debug file and don't set it.

		int attachedToEntityControllerId = attachedTo->getMetadata()->getEntityInstanceId();

		//std::cout << "Attaching " << metadata_->getEntityInstanceId() << " to " << attachedToEntityControllerId << std::endl;

		if (cyclicalAttachmentExists(attachedToEntityControllerId) == false)
		{
			if (attachedTo_ != nullptr)
			{
				// Already attached to an entity. Detach from it.
				detach();
			}

			attachedTo_ = attachedTo;

			// The entity that is being attached to needs to know that this entity is attaching to it.
			attachedTo_->addAttachedEntity(shared_from_this());

			// Set attachment in dynamics controller, if one exists.
			DynamicsController* attachedToDynamicsController = attachedTo->getDynamicsController();

			if (attachedToDynamicsController != nullptr)
			{
				dynamicsControllerHolder_->getDynamicsController()->setAttachedTo(attachedToDynamicsController);

				// Set attached entity in dynamics controller.
				attachedTo_->getDynamicsController()->addAttachedEntity(this->getDynamicsController());
			}
		}
		else
		{
			std::string err = "Attachment failed due to cyclical linkage. Attempted to attach ID " +
				boost::lexical_cast<std::string>(metadata_->getEntityInstanceId()) + " to " +
				boost::lexical_cast<std::string>(attachedToEntityControllerId);

			debugger_->appendToLog(err);
		}
	}
	else
	{
		std::string err = "Attachment failed due to null attacher. Attempted to attach ID " +
			boost::lexical_cast<std::string>(metadata_->getEntityInstanceId()) + ".";

		debugger_->appendToLog(err);
	}
}

void EntityController::detachPy()
{
	PythonReleaseGil unlocker;

	detach();
}

void EntityController::detach()
{
	if (attachedTo_ != nullptr)
	{
		// The entity that is being attached to needs to know that this entity is attaching to it.
		attachedTo_->removeAttachedEntity(shared_from_this());

		// Removed attached entity in dynamics controller.
		attachedTo_->getDynamicsController()->removeAttachedEntity(this->getDynamicsController());

		attachedTo_ = nullptr;

		dynamicsControllerHolder_->getDynamicsController()->setAttachedTo(nullptr);
	}
}

boost::shared_ptr<EntityController> EntityController::getAttachedToPy()
{
	PythonReleaseGil unlocker;

	return getAttachedTo();
}

boost::shared_ptr<EntityController> EntityController::getAttachedTo()
{
	return attachedTo_;
}

void EntityController::setAttachmentAxisPy(Axis axis)
{
	PythonReleaseGil unlocker;

	setAttachmentAxis(axis);
}

void EntityController::setAttachmentAxis(Axis axis)
{
	DynamicsController* dynamicsController = getDynamicsController();

	if (dynamicsController != nullptr)
	{
		dynamicsController->setAttachmentAxis(axis);
	}
}

void EntityController::addAttachedEntityPy(boost::shared_ptr<EntityController> attachedEntity)
{
	PythonReleaseGil unlocker;

	addAttachedEntity(attachedEntity);
}

void EntityController::addAttachedEntity(boost::shared_ptr<EntityController> attachedEntity)
{
	// Only allow each attachee to be added once.
	int size = attachedEntities_.size();

	for (int i = 0; i < size; i++)
	{
		if (attachedEntities_[i]->getMetadata()->getEntityInstanceId() == attachedEntity->getMetadata()->getEntityInstanceId())
		{
			return;
		}
	}

	attachedEntities_.push_back(attachedEntity);
}

void EntityController::removeAttachedEntity(boost::shared_ptr<EntityController> attachedEntity)
{
	int size = attachedEntities_.size();

	for (int i = 0; i < size; i++)
	{
		if (attachedEntities_[i]->getMetadata()->getEntityInstanceId() == attachedEntity->getMetadata()->getEntityInstanceId())
		{
			attachedEntities_.erase(attachedEntities_.begin() + i);

			break;
		}
	}
}

int	EntityController::getAttachedEntityCountPy()
{
	PythonReleaseGil unlocker;

	return getAttachedEntityCount();
}

int	EntityController::getAttachedEntityCount()
{
	return attachedEntities_.size();
}

boost::shared_ptr<EntityController> EntityController::getAttachedEntityPy(int index)
{
	PythonReleaseGil unlocker;

	return getAttachedEntity(index);
}

boost::shared_ptr<EntityController> EntityController::getAttachedEntity(int index)
{
	int size = attachedEntities_.size();

	if (index >= 0 && index < size)
	{
		return attachedEntities_[index];
	}
	else
	{
		return nullptr;
	}
}

void EntityController::createRenderables()
{
}

boost::shared_ptr<Renderable> EntityController::getRenderable(int index)
{
	if (index >= 0 && index < renderables_.size())
	{
		return renderables_[index];
	}

	std::cout << "Renderable with index " << index << " does not exist" << std::endl;

	return nullptr;
}

int EntityController::getRenderableCount()
{
	return renderables_.size();
}

void EntityController::addRenderable(boost::shared_ptr<Renderable> renderable)
{
	renderables_.push_back(renderable);
}

StageControllerPtr EntityController::getStageController()
{
	return stageController_;
}

void EntityController::setStageController(StageControllerPtr stageController)
{
	stageController_ = stageController;
}

StateMachineControllerPtr EntityController::getStateMachineController()
{
	return stateMachineController_;
}

void EntityController::setStateMachineController(StateMachineControllerPtr stateMachineController)
{
	stateMachineController_ = stateMachineController;
}

void EntityController::attachDynamicsController()
{
	dynamicsControllerHolder_->hasDynamicsController_ = true;
}

boost::shared_ptr<EntityMetadata> EntityController::createMetadata()
{
	return boost::shared_ptr<EntityMetadata>(new EntityMetadata());
}

void EntityController::cleanup()
{
	return;
}