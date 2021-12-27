/* -------------------------------------------------------------------------
** CodeBehindFactory.hpp
**
** The CodeBehindFactory class is the base class that is used to create
** the codebehind objects. The user can derive a sub class from it to create
** polymorphic codebehind objects in c++.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CODEBEHINDFACTORY_HPP_
#define _CODEBEHINDFACTORY_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/shared_ptr.hpp>

#include "BaseIds.hpp"
#include "EntityCodeBehind.hpp"
#include "CameraEntityCodeBehind.hpp"
#include "CameraSimulatableCodeBehind.hpp"
#include "CollidableCodeBehind.hpp"
#include "InputReceiverCodeBehind.hpp"
#include "MessageableCodeBehind.hpp"
#include "ParticleEmitterEntityCodeBehind.hpp"
#include "ParticleEntityCodeBehind.hpp"
#include "RenderableCodeBehind.hpp"
#include "SimulatableCodeBehind.hpp"
#include "StateMachineCodeBehind.hpp"
#include "TileCollidableCodeBehind.hpp"
#include "TileEntityCodeBehind.hpp"

namespace firemelon
{
	class FIREMELONAPI CodeBehindFactory
	{
	public:
		friend class CodeBehindContainer;
		friend class GameEngine;

		CodeBehindFactory();
		virtual ~CodeBehindFactory();

	protected:

		// This function can be implemented by the user in a derived class, to create a user implemented collidable codebehind object.
		virtual boost::shared_ptr<CollidableCodeBehind>				createCollidableCodeBehind();

		// The base create function is called internally. It ensures that the object will only be allocated once.
		// If the createCollidableCodeBehind is not implemented it will create the default.
		boost::shared_ptr<CollidableCodeBehind>						createCollidableCodeBehindBase(EntityTypeId entityTypeId);

		virtual boost::shared_ptr<EntityCodeBehind>					createEntityCodeBehind();
		virtual boost::shared_ptr<ParticleEmitterEntityCodeBehind>	createParticleEmitterEntityCodeBehind();
		virtual boost::shared_ptr<ParticleEntityCodeBehind>			createParticleEntityCodeBehind();
		boost::shared_ptr<EntityCodeBehind>							createEntityCodeBehindBase(EntityTypeId entityTypeId, EntityClassification classification);

		virtual boost::shared_ptr<InputReceiverCodeBehind>			createInputReceiverCodeBehind();
		boost::shared_ptr<InputReceiverCodeBehind>					createInputReceiverCodeBehindBase();

		virtual boost::shared_ptr<MessageableCodeBehind>			createMessageableCodeBehind();
		boost::shared_ptr<MessageableCodeBehind>					createMessageableCodeBehindBase();

		virtual boost::shared_ptr<RenderableCodeBehind>				createRenderableCodeBehind();
		boost::shared_ptr<RenderableCodeBehind>						createRenderableCodeBehindBase(EntityTypeId entityTypeId);

		virtual boost::shared_ptr<SimulatableCodeBehind>			createSimulatableCodeBehind();
		boost::shared_ptr<SimulatableCodeBehind>					createSimulatableCodeBehindBase(EntityTypeId entityTypeId);

		virtual boost::shared_ptr<StateMachineCodeBehind>			createStateMachineCodeBehind();
		boost::shared_ptr<StateMachineCodeBehind>					createStateMachineCodeBehindBase();

		boost::shared_ptr<BaseIds>									getIds();

	private:

		AnimationManagerPtr			animationManager_;

		boost::shared_ptr<BaseIds>	ids_;

		RendererPtr					renderer_;
	};
}

#endif // _CODEBEHINDFACTORY_HPP_