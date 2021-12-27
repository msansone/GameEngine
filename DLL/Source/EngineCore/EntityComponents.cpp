#include "..\..\Headers\EngineCore\EntityComponents.hpp"

using namespace firemelon;

EntityComponents::EntityComponents(boost::shared_ptr<BaseIds> ids, int entityInstanceId, DebuggerPtr debugger, PhysicsConfigPtr physicsConfig, HitboxManagerPtr hitboxManager)
{
	ids_ = ids;

	instanceId_ = entityInstanceId;

	debugger_ = debugger;

	hitboxManager_ = hitboxManager;

	physicsConfig_ = physicsConfig;

	dynamicsControllerHolder_ = boost::shared_ptr<DynamicsControllerHolder>(new DynamicsControllerHolder(physicsConfig_));

	entityInvalidator_ = boost::shared_ptr<EntityInvalidator>(new EntityInvalidator());

	hitboxControllerHolder_ = boost::shared_ptr<HitboxControllerHolder>(new HitboxControllerHolder());

	position_ = boost::shared_ptr<Position>(new Position(0, 0));

	stageControllerHolder_ = boost::shared_ptr<StageControllerHolder>(new StageControllerHolder());

	stateMachineControllerHolder_ = boost::shared_ptr<StateMachineControllerHolder>(new StateMachineControllerHolder());

	Debugger::entityComponentCount++;
}

EntityComponents::~EntityComponents()
{
	Debugger::entityComponentCount--;
}

void EntityComponents::attachHitboxController()
{
	if (hitboxControllerHolder_->getHasHitboxController() == false)
	{
		boost::shared_ptr<HitboxController> hitboxController = boost::shared_ptr<HitboxController>(new HitboxController());

		hitboxControllerHolder_->setHitboxController(hitboxController);

		hitboxController->setOwnerPosition(position_);

		// If there is a stage, set its position in the hitbox controller.
		if (stageControllerHolder_->getHasStageController() == true)
		{
			StagePtr stage = stageControllerHolder_->getStageController()->getStage();

			StageMetadataPtr stageMetadata = stage->getMetadata();

			hitboxController->setStagePosition(stageMetadata->getPosition());
		}
	}
}

void EntityComponents::attachRenderable()
{
	EntityControllerPtr entityController = getEntityController();

	if (entityController->renderablesCreated_ == false)
	{
		entityController->createRenderables();

		entityController->renderablesCreated_ = true;

		int renderableCount = entityController->getRenderableCount();

		for (int i = 0; i < renderableCount; i++)
		{
			RenderablePtr renderable = entityController->getRenderable(i);

			if (stageControllerHolder_->getHasStageController() == true)
			{
				StageControllerPtr stageController = stageControllerHolder_->getStageController();

				StageRenderablePtr stageRenderable = boost::static_pointer_cast<StageRenderable>(renderable);

				if (stageRenderable->isBackground_ == true)
				{
					stageController->stageBackgroundRenderable_ = stageRenderable;
				}
				else
				{
					stageController->stageForegroundRenderable_ = stageRenderable;
				}
			}

			renderable->attachDebugger(debugger_);

			renderable->setPosition(position_);

			renderable->attachRenderer(entityController->getRenderer());
		}
	}	
}

void EntityComponents::attachStageController(AnimationManagerPtr animationManager, RendererPtr renderer)
{
	if (stageControllerHolder_->getHasStageController() == false)
	{
		StageControllerPtr stageController = StageControllerPtr(new StageController());

		stageControllerHolder_->setStageController(stageController);

		stageController->animationManager_ = animationManager;
		stageController->hitboxControllerHolder_ = hitboxControllerHolder_;
		stageController->hitboxManager_ = hitboxManager_;

		// If there is a hitbox controller, give it the stage position.
		if (hitboxControllerHolder_->getHasHitboxController() == true)
		{
			HitboxControllerPtr hitboxController = hitboxControllerHolder_->getHitboxController();

			StagePtr stage = stageController->getStage();

			StageMetadataPtr stageMetadata = stage->getMetadata();

			hitboxController->setStagePosition(stageMetadata->getPosition());
		}


		stageController->stage_->animationManager_ = animationManager;
		stageController->stage_->renderer_ = renderer;

		//stageController->anchorPointManager_ = getAnchorPointManager();
		//stageController->debugger_ = getDebugger();
		//stageController->setOwnerId(getMetadata()->entityInstanceId_);
		//stageController->setOwnerEntityTypeId(getMetadata()->getEntityTypeId());
		//stageController->setOwnerMetadata(getMetadata());
		//stageController->ids_ = ids_;

		getEntityController()->setStageController(stageController);
	}
}

void EntityComponents::attachStateMachineController()
{
	if (stateMachineControllerHolder_->getHasStateMachineController() == false)
	{
		StateMachineControllerPtr stateMachineController = StateMachineControllerPtr(new StateMachineController);

		stateMachineControllerHolder_->setStateMachineController(stateMachineController);

		// State machine controller can use the stage when one exists.
		stateMachineController->stageControllerHolder_ = stageControllerHolder_;

		getEntityController()->setStateMachineController(stateMachineController);
	}
}

void EntityComponents::initialize()
{
	metadata_ = entityController_->createMetadata();

	metadata_->entityInstanceId_ = instanceId_;

	codeBehindContainer_ = boost::shared_ptr<CodeBehindContainer>(new CodeBehindContainer(debugger_));
	
	codeBehindContainer_->metadata_ = metadata_;
	codeBehindContainer_->dynamicsControllerHolder_ = dynamicsControllerHolder_;
	codeBehindContainer_->hitboxControllerHolder_ = hitboxControllerHolder_;
	codeBehindContainer_->position_ = position_;

	// Q: Why is this done here, and not when the stage controller is created?
	// A: It doesn't appear to have to be done here, as it is already getting set. Keep this code for a while,
	//    until I have used it enough to say it's not actually needed.

	//// Set the stage controller subcomponents, if one exists.
	//if (stageControllerHolder_->getHasStageController() == true)
	//{
	//	stageControllerHolder_->getStageController()->animationManager_ = entityController_->getAnimationManager();
	//}

	metadata_->setEntityTypeId(ids_->ENTITY_NULL);

	prepareController();
}

void EntityComponents::cleanup()
{
	codeBehindContainer_->cleanup();
}

EntityControllerPtr EntityComponents::createActorController(RendererPtr renderer, AnchorPointManagerPtr anchorPointManager, AnimationManagerPtr animationManager)
{
	if (entityController_ == nullptr)
	{
		entityController_ = EntityControllerPtr(new ActorController());

		entityController_->renderer_ = renderer;

		entityController_->anchorPointManager_ = anchorPointManager;

		entityController_->animationManager_ = animationManager;

		attachHitboxController();

		attachStageController(animationManager, renderer);
		
		attachStateMachineController();

		attachRenderable();

		StageControllerPtr stageController = stageControllerHolder_->getStageController();

		int renderableCount = getRenderableCount();

		for (int i = 0; i < renderableCount; i++)
		{
			StageRenderablePtr stageRenderable = boost::static_pointer_cast<StageRenderable>(getRenderable(i));

			stageRenderable->stateContainer_ = stateMachineControllerHolder_->getStateMachineController()->stateContainer_;

			stageRenderable->stage_ = stageController->getStage();

			stageRenderable->hitboxController_ = hitboxControllerHolder_->getHitboxController();
		}
		
		stageController->stateContainer_ = stateMachineControllerHolder_->getStateMachineController()->stateContainer_;

		stageController->renderer_ = renderer;

		// The stage needs a reference to the stateContainer
		entityController_->ids_ = ids_;
	}

	return entityController_;
}

EntityControllerPtr EntityComponents::createCameraController()
{
	if (entityController_ == nullptr)
	{
		entityController_ = EntityControllerPtr(new CameraController());
	
		entityController_->ids_ = ids_;
	}


	return entityController_;
}

EntityControllerPtr EntityComponents::createEventController()
{
	if (entityController_ == nullptr)
	{
		entityController_ = EntityControllerPtr(new EventController());

		attachHitboxController();
	}

	entityController_->ids_ = ids_;

	return entityController_;
}

EntityControllerPtr EntityComponents::createHudElementController(RendererPtr renderer, AnchorPointManagerPtr anchorPointManager, AnimationManagerPtr animationManager)
{
	if (entityController_ == nullptr)
	{
		entityController_ = EntityControllerPtr(new HudElementController());

		entityController_->renderer_ = renderer;

		entityController_->anchorPointManager_ = anchorPointManager;

		entityController_->animationManager_ = animationManager;

		attachStageController(animationManager, renderer);

		attachStateMachineController();

		attachRenderable();

		StageControllerPtr stageController = stageControllerHolder_->getStageController();

		int renderableCount = getRenderableCount();

		for (int i = 0; i < renderableCount; i++)
		{
			StageRenderablePtr stageRenderable = boost::static_pointer_cast<StageRenderable>(getRenderable(i));
			
			stageRenderable->stateContainer_ = stateMachineControllerHolder_->getStateMachineController()->stateContainer_;

			stageRenderable->stage_ = stageController->getStage();
		}

		//left off here, getting multiple stage renderables working, for BG animation pieces in a singular entity.
		stageController->stateContainer_ = stateMachineControllerHolder_->getStateMachineController()->stateContainer_;

		stageController->renderer_ = renderer;

		entityController_->ids_ = ids_;
	}

	return entityController_;
}

EntityControllerPtr EntityComponents::createParticleController(RendererPtr renderer)
{
	if (entityController_ == nullptr)
	{
		entityController_ = EntityControllerPtr(new ParticleController());

		entityController_->renderer_ = renderer;

		attachRenderable();
	}

	entityController_->ids_ = ids_;

	return entityController_;
}

EntityControllerPtr EntityComponents::createParticleEmitterController()
{
	if (entityController_ == nullptr)
	{
		entityController_ = EntityControllerPtr(new ParticleEmitterController());
	}

	entityController_->ids_ = ids_;

	return entityController_;
}

EntityControllerPtr EntityComponents::createTileController(RendererPtr renderer)
{
	if (entityController_ == nullptr)
	{
		entityController_ = EntityControllerPtr(new TileController());

		entityController_->renderer_ = renderer;

		attachHitboxController(); 
		
		if (renderer != nullptr)
		{
			attachRenderable();
		}
	}

	entityController_->ids_ = ids_;

	return entityController_;
}

void EntityComponents::prepareController()
{
	entityController_->dynamicsControllerHolder_ = dynamicsControllerHolder_;
	entityController_->hitboxControllerHolder_ = hitboxControllerHolder_;
	entityController_->position_ = position_;
	entityController_->metadata_ = metadata_;
}

boost::shared_ptr<Position> EntityComponents::getPosition()
{
	return position_;
}

bool EntityComponents::getHasDynamicsController()
{
	return dynamicsControllerHolder_->getHasDynamicsController();
}

DynamicsController*	EntityComponents::getDynamicsController()
{
	return dynamicsControllerHolder_->getDynamicsController();
}

boost::shared_ptr<Renderable> EntityComponents::getRenderable(int index)
{
	return entityController_->getRenderable(index);	
}

int EntityComponents::getRenderableCount()
{
	return entityController_->getRenderableCount();
}

boost::shared_ptr<HitboxController> EntityComponents::getHitboxController()
{
	return hitboxControllerHolder_->getHitboxController();
}

StageControllerPtr EntityComponents::getStageController()
{
	return stageControllerHolder_->getStageController();
}

EntityControllerPtr EntityComponents::getEntityController()
{
	return entityController_;
}

boost::shared_ptr<EntityInvalidator> EntityComponents::getEntityInvalidator()
{
	return entityInvalidator_;
}

boost::shared_ptr<EntityMetadata> EntityComponents::getEntityMetadata()
{
	return metadata_;
}

boost::shared_ptr<CodeBehindContainer> EntityComponents::getCodeBehindContainer()
{
	return codeBehindContainer_;
}

StateMachineControllerPtr EntityComponents::getStateMachineController()
{
	return stateMachineControllerHolder_->getStateMachineController();
}