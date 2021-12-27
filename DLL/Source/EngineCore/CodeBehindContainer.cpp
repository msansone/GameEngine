#include "..\..\Headers\EngineCore\CodeBehindContainer.hpp"

using namespace firemelon;
using namespace boost::python;

CodeBehindContainer::CodeBehindContainer(DebuggerPtr debugger)
{
	debugger_ = debugger;

	isCollidable_ = false;
	isInitialized_ = false;
	isInputReceiver_ = false;
	isMessageable_ = false;
	isRenderable_ = false;
	isSimulatable_ = false;
	isStateMachine_ = false;

	pythonInstanceWrapper_ = boost::shared_ptr<PythonInstanceWrapper>(new PythonInstanceWrapper());

	pythonInstanceWrapper_->debugger_ = debugger;
}

CodeBehindContainer::~CodeBehindContainer()
{
	if (this->metadata_->classification_ == ENTITY_CLASSIFICATION_CAMERA)
	{
		bool debug = true;
	}
}

boost::shared_ptr<PythonInstanceWrapper> CodeBehindContainer::getPythonInstanceWrapper()
{
	return pythonInstanceWrapper_;
}

void CodeBehindContainer::createCodeBehinds(EntityTypeId entityTypeId, EntityClassification classification)
{
	entityTypeId_ = entityTypeId;
	classification_ = classification;

	entityCodeBehind_ = codeBehindFactory_->createEntityCodeBehindBase(entityTypeId, classification);

	entityCodeBehind_->setClassification(classification);

	entityCodeBehind_->controller_ = controller_;
	entityCodeBehind_->debugger_ = debugger_;
	entityCodeBehind_->metadata_ = metadata_;
	entityCodeBehind_->position_ = position_;
	entityCodeBehind_->pythonInstanceWrapper_ = pythonInstanceWrapper_;

	entityCodeBehind_->timer_ = timer_;

	if (isCollidable_ == true)
	{
		collidableCodeBehind_ = codeBehindFactory_->createCollidableCodeBehindBase(entityTypeId);

		collidableCodeBehind_->setClassification(classification);
		collidableCodeBehind_->setTileSize(tileSize_);
		collidableCodeBehind_->metadata_ = metadata_;

		collidableCodeBehind_->pythonInstanceWrapper_ = pythonInstanceWrapper_;

		collidableCodeBehind_->debugger_ = debugger_;
		collidableCodeBehind_->hitboxManager_ = hitboxManager_;
		collidableCodeBehind_->dynamicsControllerHolder_ = dynamicsControllerHolder_;
		collidableCodeBehind_->hitboxControllerHolder_ = hitboxControllerHolder_;
		collidableCodeBehind_->position_ = position_;
		collidableCodeBehind_->ids_ = ids_;
		collidableCodeBehind_->timer_ = timer_;
	}

	if (isInputReceiver_ == true)
	{
		inputReceiverCodeBehind_ = codeBehindFactory_->createInputReceiverCodeBehindBase();

		inputReceiverCodeBehind_->setClassification(classification);

		inputReceiverCodeBehind_->debugger_ = debugger_;

		inputReceiverCodeBehind_->metadata_ = metadata_;

		inputReceiverCodeBehind_->pythonInstanceWrapper_ = pythonInstanceWrapper_;

		inputReceiverCodeBehind_->inputDeviceManager_ = inputDeviceManager_;

		inputReceiverCodeBehind_->ui_ = ui_;

		inputReceiverCodeBehind_->timer_ = timer_;
	}

	if (isMessageable_ == true)
	{
		messageableCodeBehind_ = codeBehindFactory_->createMessageableCodeBehindBase();

		messageableCodeBehind_->setClassification(classification);

		messageableCodeBehind_->debugger_ = debugger_;

		messageableCodeBehind_->metadata_ = metadata_;

		messageableCodeBehind_->pythonInstanceWrapper_ = pythonInstanceWrapper_;

		messageableCodeBehind_->timer_ = timer_;
	}

	if (isStateMachine_ == true)
	{
		stateMachineCodeBehind_ = codeBehindFactory_->createStateMachineCodeBehindBase();

		stateMachineCodeBehind_->setClassification(classification);

		stateMachineCodeBehind_->debugger_ = debugger_;

		stateMachineCodeBehind_->metadata_ = metadata_;

		stateMachineCodeBehind_->stateMachineController_ = stateMachineController_;

		stateMachineCodeBehind_->pythonInstanceWrapper_ = pythonInstanceWrapper_;

		stateMachineCodeBehind_->timer_ = timer_;
	}

	if (isSimulatable_ == true)
	{
		simulatableCodeBehind_ = codeBehindFactory_->createSimulatableCodeBehindBase(entityTypeId);

		simulatableCodeBehind_->controller_ = controller_;
		simulatableCodeBehind_->debugger_ = debugger_;
		simulatableCodeBehind_->dynamicsControllerHolder_ = dynamicsControllerHolder_;
		simulatableCodeBehind_->pythonInstanceWrapper_ = pythonInstanceWrapper_;
		simulatableCodeBehind_->setClassification(classification);
		simulatableCodeBehind_->timer_ = timer_;
	}


	if (isRenderable_ == true)
	{
		renderableCodeBehind_ = codeBehindFactory_->createRenderableCodeBehindBase(entityTypeId);

		renderableCodeBehind_->controller_ = controller_;
		renderableCodeBehind_->debugger_ = debugger_;
		renderableCodeBehind_->pythonInstanceWrapper_ = pythonInstanceWrapper_;
		renderableCodeBehind_->renderable_ = controller_->getRenderable();
		renderableCodeBehind_->stageController_ = stageController_;
		renderableCodeBehind_->setClassification(classification);
		renderableCodeBehind_->timer_ = timer_;
	}
}


void CodeBehindContainer::initialize()
{
	if (isInitialized_ == false)
	{
		isInitialized_ = true;

		if (classification_ != ENTITY_CLASSIFICATION_TILE)
		{
			pythonInstanceWrapper_->initialize();
		}

		entityCodeBehind_->preInitialize();

		if (isCollidable_ == true)
		{
			collidableCodeBehind_->preInitialize();
		}

		if (isInputReceiver_ == true)
		{
			inputReceiverCodeBehind_->preInitialize();
		}

		if (isMessageable_ == true)
		{
			messageableCodeBehind_->preInitialize();
		}

		if (isRenderable_ == true)
		{
			renderableCodeBehind_->preInitialize();
		}
		
		if (isStateMachine_ == true)
		{
			stateMachineCodeBehind_->preInitialize();
		}

		if (isSimulatable_ == true)
		{
			simulatableCodeBehind_->preInitialize();

			// The dynamics controller will have been created inside the simulatable code behind.
			DynamicsController* dynamicsController = dynamicsControllerHolder_->getDynamicsController();

			if (dynamicsController != nullptr)
			{
				dynamicsController->setPosition(position_);

				dynamicsController->setOwnerId(metadata_->entityInstanceId_);

				dynamicsController->renderer_ = renderer_;

				
				if (stateMachineCodeBehind_ != nullptr)
				{
					if (stateMachineCodeBehind_->stateMachineController_ != nullptr)
					{
						stateMachineCodeBehind_->stateMachineController_->setDynamicsController(dynamicsController);
					}
				}
			}
		}
	}
}

bool CodeBehindContainer::getIsCollidable()
{
	return collidableCodeBehind_ != nullptr;
}

bool CodeBehindContainer::getIsInputReceiver()
{
	return inputReceiverCodeBehind_ != nullptr;
}

bool CodeBehindContainer::getIsMessageable()
{
	return messageableCodeBehind_ != nullptr;
}

bool CodeBehindContainer::getIsSimulatable()
{
	return simulatableCodeBehind_ != nullptr;
}

bool CodeBehindContainer::getIsStateMachine()
{
	return stateMachineCodeBehind_ != nullptr;
}

boost::shared_ptr<EntityCodeBehind> CodeBehindContainer::getEntityCodeBehind()
{
	return entityCodeBehind_;
}

boost::shared_ptr<CollidableCodeBehind> CodeBehindContainer::getCollidableCodeBehind()
{
	return collidableCodeBehind_;
}

boost::shared_ptr<InputReceiverCodeBehind> CodeBehindContainer::getInputReceiverCodeBehind()
{
	return inputReceiverCodeBehind_;
}

boost::shared_ptr<MessageableCodeBehind> CodeBehindContainer::getMessageableCodeBehind()
{
	return messageableCodeBehind_;
}

boost::shared_ptr<RenderableCodeBehind> CodeBehindContainer::getRenderableCodeBehind()
{
	return renderableCodeBehind_;
}

boost::shared_ptr<SimulatableCodeBehind> CodeBehindContainer::getSimulatableCodeBehind()
{
	return simulatableCodeBehind_;
}

boost::shared_ptr<StateMachineCodeBehind> CodeBehindContainer::getStateMachineCodeBehind()
{
	return stateMachineCodeBehind_;
}

void CodeBehindContainer::setIsCollidable()
{
	isCollidable_ = true;
}

void CodeBehindContainer::setIsInputReceiver()
{
	isInputReceiver_ = true;
}

void CodeBehindContainer::setIsMessageable()
{
	isMessageable_ = true;
}

void CodeBehindContainer::setIsRenderable()
{
	isRenderable_ = true;
}

void CodeBehindContainer::setIsSimulatable()
{
	isSimulatable_ = true;
}

void CodeBehindContainer::setIsStateMachine()
{
	isStateMachine_ = true;
}

void CodeBehindContainer::cleanup()
{
	entityCodeBehind_->preCleanup();

	if (isCollidable_ == true)
	{
		collidableCodeBehind_->preCleanup();
	}

	if (isInputReceiver_ == true)
	{
		inputReceiverCodeBehind_->preCleanup();
	}

	if (isMessageable_ == true)
	{
		messageableCodeBehind_->preCleanup();
	}

	if (isRenderable_ == true)
	{
		renderableCodeBehind_->preCleanup();
	}

	if (isStateMachine_ == true)
	{
		stateMachineCodeBehind_->preCleanup();
	}

	if (isSimulatable_ == true)
	{
		simulatableCodeBehind_->preCleanup();
	}

	pythonInstanceWrapper_->cleanup();
}