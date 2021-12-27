/* -------------------------------------------------------------------------
** EntityComponents.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYCOMPONENTS_HPP_
#define _ENTITYCOMPONENTS_HPP_

#include <boost/python.hpp>

#include "ActorController.hpp"
#include "BaseIds.hpp"
#include "CameraController.hpp"
#include "CodeBehindContainer.hpp"
#include "DynamicsControllerHolder.hpp"
#include "EntityInvalidator.hpp"
#include "EntityMetadata.hpp"
#include "EventController.hpp"
#include "HitboxControllerHolder.hpp"
#include "HudElementController.hpp"
#include "StageControllerHolder.hpp"
#include "StateMachineControllerHolder.hpp"
#include "TileController.hpp"

namespace firemelon
{
	class EntityComponents
	{
	public:
		friend class Entity;
		friend class PhysicsManager;

		EntityComponents(boost::shared_ptr<BaseIds> ids, int entityInstanceId, DebuggerPtr debugger, PhysicsConfigPtr physicsConfig, HitboxManagerPtr hitboxManager);
		virtual ~EntityComponents();

		EntityControllerPtr							createActorController(RendererPtr renderer, AnchorPointManagerPtr anchorPointManager, AnimationManagerPtr animationManager);
		EntityControllerPtr							createCameraController();
		EntityControllerPtr							createEventController();
		EntityControllerPtr							createHudElementController(RendererPtr renderer, AnchorPointManagerPtr anchorPointManager, AnimationManagerPtr animationManager);
		EntityControllerPtr							createParticleController(RendererPtr renderer);
		EntityControllerPtr							createParticleEmitterController();
		EntityControllerPtr							createTileController(RendererPtr renderer);

		bool										getHasDynamicsController();
		
		boost::shared_ptr<CodeBehindContainer>		getCodeBehindContainer();
		DynamicsController*							getDynamicsController();
		EntityControllerPtr							getEntityController();
		boost::shared_ptr<EntityInvalidator>		getEntityInvalidator();
		boost::shared_ptr<EntityMetadata>			getEntityMetadata();
		boost::shared_ptr<HitboxController>			getHitboxController();
		StageControllerPtr							getStageController();
		boost::shared_ptr<Position>					getPosition();
		boost::shared_ptr<Renderable>				getRenderable(int index = 0);
		int											getRenderableCount();
		StateMachineControllerPtr					getStateMachineController();

		void										initialize();

	private:

		void										attachHitboxController(); 
		void										attachRenderable();
		void										attachStageController(AnimationManagerPtr animationManager, RendererPtr renderer);
		void										attachStateMachineController();

		void										cleanup();

		void										prepareController();

		boost::shared_ptr<CodeBehindContainer>		codeBehindContainer_;
		DebuggerPtr									debugger_;
		boost::shared_ptr<DynamicsControllerHolder>	dynamicsControllerHolder_;
		EntityControllerPtr							entityController_;
		boost::shared_ptr<EntityInvalidator>		entityInvalidator_;
		boost::shared_ptr<HitboxControllerHolder>	hitboxControllerHolder_;
		boost::shared_ptr<HitboxManager>			hitboxManager_;
		boost::shared_ptr<BaseIds>					ids_;
		int											instanceId_;
		boost::shared_ptr<EntityMetadata>			metadata_;
		PhysicsConfigPtr							physicsConfig_;
		boost::shared_ptr<Position>					position_;
		//boost::shared_ptr<Renderable>				renderable_;
		StageControllerHolderPtr					stageControllerHolder_;
		StateMachineControllerHolderPtr				stateMachineControllerHolder_;
	};

	typedef boost::shared_ptr<EntityComponents> EntityComponentsPtr;
}

#endif // _PHYSICSOBJECT_HPP_