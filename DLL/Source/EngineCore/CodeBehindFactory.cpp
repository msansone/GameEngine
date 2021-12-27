#include "..\..\Headers\EngineCore\CodeBehindFactory.hpp"

using namespace firemelon;
using namespace boost::python;

CodeBehindFactory::CodeBehindFactory()
{
}

CodeBehindFactory::~CodeBehindFactory()
{
}

boost::shared_ptr<CollidableCodeBehind> CodeBehindFactory::createCollidableCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<CollidableCodeBehind> CodeBehindFactory::createCollidableCodeBehindBase(EntityTypeId entityTypeId)
{
	boost::shared_ptr<CollidableCodeBehind> newCollidableCodeBehind = nullptr;

	if (entityTypeId == ids_->ENTITY_TILE)
	{
		newCollidableCodeBehind = boost::shared_ptr<CollidableCodeBehind>(new TileCollidableCodeBehind());
	}
	else
	{
		newCollidableCodeBehind = createCollidableCodeBehind();
	}

	// Default to the basic collidable code behind.
	if (newCollidableCodeBehind == nullptr)
	{
		newCollidableCodeBehind = boost::shared_ptr<CollidableCodeBehind>(new CollidableCodeBehind());
	}

	return newCollidableCodeBehind;
}

boost::shared_ptr<EntityCodeBehind> CodeBehindFactory::createEntityCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<ParticleEmitterEntityCodeBehind> CodeBehindFactory::createParticleEmitterEntityCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<ParticleEntityCodeBehind> CodeBehindFactory::createParticleEntityCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<EntityCodeBehind> CodeBehindFactory::createEntityCodeBehindBase(EntityTypeId entityTypeId, EntityClassification classification)
{
	boost::shared_ptr<EntityCodeBehind> newEntityCodeBehind = nullptr;
	
	// Tiles should not be created polymorphically. They are a special case in that they do not have a python instance associated with them.
	// Also there is really no need at this point for an implementer to change them.
	if (entityTypeId == ids_->ENTITY_TILE)
	{
		newEntityCodeBehind = boost::shared_ptr<TileEntityCodeBehind>(new TileEntityCodeBehind());
	}
	else if (entityTypeId == ids_->ENTITY_CAMERA)
	{
		newEntityCodeBehind = boost::shared_ptr<CameraEntityCodeBehind>(new CameraEntityCodeBehind());
	}
	else if (classification == ENTITY_CLASSIFICATION_PARTICLE)
	{
		newEntityCodeBehind = createParticleEntityCodeBehind();

		if (newEntityCodeBehind == nullptr)
		{
			newEntityCodeBehind = boost::shared_ptr<ParticleEntityCodeBehind>(new ParticleEntityCodeBehind());
		}

		boost::shared_ptr<ParticleEntityCodeBehind> particleCodeBehind = boost::static_pointer_cast<ParticleEntityCodeBehind>(newEntityCodeBehind);

		particleCodeBehind->animationManager_ = animationManager_;
	}
	else if (classification == ENTITY_CLASSIFICATION_PARTICLEEMITTER)
	{
		newEntityCodeBehind = createParticleEmitterEntityCodeBehind();

		if (newEntityCodeBehind == nullptr)
		{
			newEntityCodeBehind = boost::shared_ptr<ParticleEmitterEntityCodeBehind>(new ParticleEmitterEntityCodeBehind());
		}

		boost::shared_ptr<ParticleEmitterEntityCodeBehind> particleEmitterCodeBehind = boost::static_pointer_cast<ParticleEmitterEntityCodeBehind>(newEntityCodeBehind);

		particleEmitterCodeBehind->animationManager_ = animationManager_;
		particleEmitterCodeBehind->renderer_ = renderer_;
	}
	else
	{
		newEntityCodeBehind = createEntityCodeBehind();

		if (newEntityCodeBehind == nullptr)
		{
			newEntityCodeBehind = boost::shared_ptr<EntityCodeBehind>(new EntityCodeBehind());
		}
	}


	return newEntityCodeBehind;
}

boost::shared_ptr<InputReceiverCodeBehind> CodeBehindFactory::createInputReceiverCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<InputReceiverCodeBehind> CodeBehindFactory::createInputReceiverCodeBehindBase()
{
	boost::shared_ptr<InputReceiverCodeBehind> newInputReceiverCodeBehind = createInputReceiverCodeBehind();

	// Default to the basic input receiver code behind.
	if (newInputReceiverCodeBehind == nullptr)
	{
		newInputReceiverCodeBehind = boost::shared_ptr<InputReceiverCodeBehind>(new InputReceiverCodeBehind());
	}

	return newInputReceiverCodeBehind;
}

boost::shared_ptr<MessageableCodeBehind> CodeBehindFactory::createMessageableCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<MessageableCodeBehind> CodeBehindFactory::createMessageableCodeBehindBase()
{
	boost::shared_ptr<MessageableCodeBehind> newMessageableCodeBehind = createMessageableCodeBehind();

	// Default to the basic messageable code behind.
	if (newMessageableCodeBehind == nullptr)
	{
		newMessageableCodeBehind = boost::shared_ptr<MessageableCodeBehind>(new MessageableCodeBehind());
	}

	return newMessageableCodeBehind;
}

boost::shared_ptr<RenderableCodeBehind> CodeBehindFactory::createRenderableCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<RenderableCodeBehind> CodeBehindFactory::createRenderableCodeBehindBase(EntityTypeId entityTypeId)
{
	boost::shared_ptr<RenderableCodeBehind> newRenderableCodeBehind = nullptr;

	newRenderableCodeBehind = createRenderableCodeBehind();

	// Default to the basic renderable code behind.
	if (newRenderableCodeBehind == nullptr)
	{
		newRenderableCodeBehind = boost::shared_ptr<RenderableCodeBehind>(new RenderableCodeBehind());
	}

	return newRenderableCodeBehind;
}

boost::shared_ptr<SimulatableCodeBehind> CodeBehindFactory::createSimulatableCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<SimulatableCodeBehind> CodeBehindFactory::createSimulatableCodeBehindBase(EntityTypeId entityTypeId)
{
	boost::shared_ptr<SimulatableCodeBehind> newSimulatableCodeBehind = nullptr;
	
	if (entityTypeId == ids_->ENTITY_CAMERA)
	{
		newSimulatableCodeBehind = boost::shared_ptr<CameraSimulatableCodeBehind>(new CameraSimulatableCodeBehind());
	}
	else
	{
		newSimulatableCodeBehind = createSimulatableCodeBehind();
	}

	// Default to the basic simulatable code behind.
	if (newSimulatableCodeBehind == nullptr)
	{
		newSimulatableCodeBehind = boost::shared_ptr<SimulatableCodeBehind>(new SimulatableCodeBehind());
	}

	return newSimulatableCodeBehind;
}

boost::shared_ptr<StateMachineCodeBehind> CodeBehindFactory::createStateMachineCodeBehind()
{
	return nullptr;
}

boost::shared_ptr<StateMachineCodeBehind> CodeBehindFactory::createStateMachineCodeBehindBase()
{
	boost::shared_ptr<StateMachineCodeBehind> newStateMachineCodeBehind = createStateMachineCodeBehind();

	// Default to the basic messageable code behind.
	if (newStateMachineCodeBehind == nullptr)
	{
		newStateMachineCodeBehind = boost::shared_ptr<StateMachineCodeBehind>(new StateMachineCodeBehind());
	}

	return newStateMachineCodeBehind;
}

boost::shared_ptr<BaseIds> CodeBehindFactory::getIds()
{
	return ids_;
}