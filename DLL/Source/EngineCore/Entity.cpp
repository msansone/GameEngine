#include "..\..\Headers\EngineCore\Entity.hpp"

using namespace firemelon;
using namespace boost::python;

Entity::Entity(boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger, PhysicsConfigPtr physicsConfig, HitboxManagerPtr hitboxManager) :
	changeNameSignal_(new EntityChangeNameSignalRaw),
	entityChangingRoomSignal_(new EntityChangeRoomSignalRaw),
	removeEntitySignal_(new EntityRemoveEntitySignalRaw)
{
	instanceId_ = IdGenerator::getNextId();

	components_ = boost::shared_ptr<EntityComponents>(new EntityComponents(ids, instanceId_, debugger, physicsConfig, hitboxManager));
	
	ids_ = ids;
	debugger_ = debugger;
	isInitialized_ = false;
	isActive_ = true;
	isInvalidated_ = false;
	keepRoomActive_ = false;
	attachCamera_ = false;

	attachedCameraInstanceId_ = -1;
	
	Debugger::entityCount++;
}

Entity::~Entity()
{
	//components_->stateData_->pyEntityData_= boost::python::object();

	Debugger::entityCount--;
}

void Entity::cleanup()
{
	if (components_->getEntityController() != nullptr)
	{
		components_->getEntityController()->cleanup();

		components_->getEntityController()->changeNameSignal_->disconnect(boost::bind(&Entity::changeName, this, _1, _2));

		components_->getEntityController()->changeRoomSignal_->disconnect(boost::bind(&Entity::changeRoom, this, _1, _2));

		components_->getEntityController()->removeEntitySignal_->disconnect(boost::bind(&Entity::remove, this));
	}

	if (components_->codeBehindContainer_->getIsStateMachine() == true)
	{
		boost::shared_ptr<StateMachineController> stateMachineController = components_->stateMachineControllerHolder_->getStateMachineController();

		boost::shared_ptr<StateMachineCodeBehind> stateMachineCodeBehind = components_->codeBehindContainer_->getStateMachineCodeBehind();

		stateMachineController->stateChangedSignal.disconnect(boost::bind(&StateMachineCodeBehind::stateChanged, stateMachineCodeBehind, _1, _2));


		StageControllerPtr stageController = components_->stageControllerHolder_->getStageController();

		boost::shared_ptr<RenderableCodeBehind> renderableCodeBehind = components_->codeBehindContainer_->getRenderableCodeBehind();

		StageRenderablePtr stageForegroundRenderable = stageController->getStageForegroundRenderable();
		
		stageForegroundRenderable->setStateByNameSignal.disconnect(boost::bind(&StateMachineController::setStateByName, stateMachineController, _1));

		stageForegroundRenderable->stateEndedSignal.disconnect(boost::bind(&StateMachineCodeBehind::stateEnded, stateMachineCodeBehind, _1));

		stageForegroundRenderable->frameTriggerSignal.disconnect(boost::bind(&RenderableCodeBehind::frameTriggered, renderableCodeBehind, _1));


		StageRenderablePtr stageBackgroundRenderable = stageController->getStageBackgroundRenderable();

		stageBackgroundRenderable->setStateByNameSignal.disconnect(boost::bind(&StateMachineController::setStateByName, stateMachineController, _1));

		stageBackgroundRenderable->stateEndedSignal.disconnect(boost::bind(&StateMachineCodeBehind::stateEnded, stateMachineCodeBehind, _1));

		stageBackgroundRenderable->frameTriggerSignal.disconnect(boost::bind(&RenderableCodeBehind::frameTriggered, renderableCodeBehind, _1));
	}

	components_->cleanup();
}

void Entity::setAttachedCameraId(int cameraInstanceId)
{
	attachedCameraInstanceId_ = cameraInstanceId;
}

int Entity::getAttachedCameraId()
{
	return attachedCameraInstanceId_;
}

void Entity::setAttachCamera(bool attachCamera)
{
	attachCamera_ = attachCamera;
}

bool Entity::getAttachCamera()
{
	return attachCamera_;
}

void Entity::initialize()
{	
	if (isInitialized_ == false)
	{
		boost::shared_ptr<CodeBehindContainer> codeBehindContainer = components_->getCodeBehindContainer();

		boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();
		boost::shared_ptr<StateMachineCodeBehind> stateMachineCodeBehind = codeBehindContainer->getStateMachineCodeBehind();
		boost::shared_ptr<RenderableCodeBehind> renderableCodeBehind = codeBehindContainer->getRenderableCodeBehind();

		EntityControllerPtr controller = components_->getEntityController();

		if (controller != nullptr)
		{
			controller->changeNameSignal_->connect(boost::bind(&Entity::changeName, this, _1, _2));
			controller->changeRoomSignal_->connect(boost::bind(&Entity::changeRoom, this, _1, _2));
			controller->removeEntitySignal_->connect(boost::bind(&Entity::remove, this));
		}
		
		// Initialize the previous position to the current position, to avoid unwanted LERP effects.
		boost::shared_ptr<Position> position = controller->getPosition();
		position->setPreviousX(position->getX());
		position->setPreviousY(position->getY());

		boost::shared_ptr<HitboxController> hc = components_->getHitboxController();

		if (codeBehindContainer->getIsStateMachine() == true)		
		{
			StateMachineControllerPtr stateMachineController = components_->stateMachineControllerHolder_->getStateMachineController();

			stateMachineController->stateChangedSignal.connect(boost::bind(&StateMachineCodeBehind::stateChanged, stateMachineCodeBehind, _1, _2));


			StageControllerPtr stageController = components_->stageControllerHolder_->getStageController();


			StageRenderablePtr stageForegroundRenderable = stageController->getStageForegroundRenderable();

			stageForegroundRenderable->setStateByNameSignal.connect(boost::bind(&StateMachineController::setStateByName, stateMachineController, _1));

			stageForegroundRenderable->stateEndedSignal.connect(boost::bind(&StateMachineCodeBehind::stateEnded, stateMachineCodeBehind, _1));

			stageForegroundRenderable->frameTriggerSignal.connect(boost::bind(&RenderableCodeBehind::frameTriggered, renderableCodeBehind, _1));


			StageRenderablePtr stageBackgroundRenderable = stageController->getStageBackgroundRenderable();

			stageBackgroundRenderable->setStateByNameSignal.connect(boost::bind(&StateMachineController::setStateByName, stateMachineController, _1));

			stageBackgroundRenderable->stateEndedSignal.connect(boost::bind(&StateMachineCodeBehind::stateEnded, stateMachineCodeBehind, _1));

			stageBackgroundRenderable->frameTriggerSignal.connect(boost::bind(&RenderableCodeBehind::frameTriggered, renderableCodeBehind, _1));


			if (hc != nullptr)
			{
				StagePtr stage = components_->getStageController()->getStage();

				hc->setOwnerPosition(stage->getMetadata()->getPosition());
			}
		}
		
		isInitialized_ = true;
		

		if (hc != nullptr)
		{
			hc->setOwnerPosition(components_->getPosition());
		}
	}
}

boost::shared_ptr<EntityComponents> Entity::getComponents()
{
	return components_;
}

int Entity::changeName(std::string name, bool append)
{
	boost::shared_ptr<EntityMetadata> metadata = components_->getEntityMetadata();

	boost::optional<int> optionalResultCode = (*changeNameSignal_)(metadata->entityInstanceId_, name, append);

	int resultCode = optionalResultCode.get();

	return resultCode;
}

void Entity::changeRoom(ChangeRoomParameters changeRoomParams, ShowRoomParameters showRoomParams)
{
	boost::shared_ptr<EntityMetadata> metadata = components_->getEntityMetadata();

	(*entityChangingRoomSignal_)(metadata->getEntityInstanceId(), metadata->getRoomMetadata()->getRoomId(), changeRoomParams, showRoomParams);
}

void Entity::remove()
{
	// If the entity is not in a room that is currently active, it can be deleted immediately. Otherwise it 
	// will have to be flaged for removal, and during the next update frame, instead of being updated by the 
	// entity manager it will be removed from the entity list, and uninitialized.

	// What happens if an entity is removed, and the room is active, so it gets flagged, but then before the
	// room can reach its next update, it becomes de-activated? The entity would not get removed until the
	// next time the room became active again.
	boost::shared_ptr<EntityMetadata> metadata = components_->getEntityMetadata();

	boost::optional<bool> optionalRemoved = (*removeEntitySignal_)(metadata->entityInstanceId_, metadata->getRoomMetadata()->getRoomId());

	bool removed = optionalRemoved.get();

	if (removed == false)
	{
		components_->getCodeBehindContainer()->getEntityCodeBehind()->setIsRemoved(true);

		isActive_ = false;

		invalidate();
	}
}

void Entity::validate()
{
	// Validate that the user has set an entit type.
	//assert(entityTypeId_ != ids_->ENTITY_NULL);

	isInvalidated_ = false;
	
	components_->entityInvalidator_->setIsInvalidated(isInvalidated_);
}

void Entity::invalidate()
{
	isInvalidated_ = true;
	
	components_->entityInvalidator_->setIsInvalidated(isInvalidated_);
}

void Entity::attachInputDeviceManager(boost::shared_ptr<InputDeviceManager> inputDeviceManager)
{
	EntityControllerPtr controller = components_->getEntityController();

	controller->inputDeviceManager_ = inputDeviceManager;
}

DynamicsController* Entity::createDynamicsController()
{
	return new DynamicsController();
}