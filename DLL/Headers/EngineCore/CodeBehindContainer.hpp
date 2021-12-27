//* -------------------------------------------------------------------------
//** CodeBehindContainer.hpp
//**
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */

#ifndef _CODEBEHINDCONTAINER_HPP_
#define _CODEBEHINDCONTAINER_HPP_

#include "BaseIds.hpp"
#include "CodeBehindFactory.hpp"
#include "DynamicsControllerHolder.hpp"
#include "EntityMetadata.hpp"
#include "GameTimer.hpp"
#include "HitboxManager.hpp"
#include "InputDeviceManager.hpp"
#include "Position.hpp"
#include "PythonInstanceWrapper.hpp"
#include "RenderableCodeBehind.hpp"
#include "Renderer.hpp"
#include "StateMachineController.hpp"

namespace firemelon
{
	class CodeBehindContainer
	{
	public:
		friend class EntityComponents;
		friend class Room;

		CodeBehindContainer(DebuggerPtr debugger);
		virtual ~CodeBehindContainer();

		void										cleanup();
		void										createCodeBehinds(EntityTypeId entityTypeId, EntityClassification classification);
		boost::shared_ptr<EntityCodeBehind>			getEntityCodeBehind();
		boost::shared_ptr<CollidableCodeBehind>		getCollidableCodeBehind();
		boost::shared_ptr<InputReceiverCodeBehind>	getInputReceiverCodeBehind();
		bool										getIsCollidable();
		bool										getIsInputReceiver();
		bool										getIsMessageable();
		bool										getIsSimulatable();
		bool										getIsStateMachine();
		boost::shared_ptr<MessageableCodeBehind>	getMessageableCodeBehind();
		boost::shared_ptr<PythonInstanceWrapper>	getPythonInstanceWrapper();
		boost::shared_ptr<RenderableCodeBehind>		getRenderableCodeBehind();
		boost::shared_ptr<SimulatableCodeBehind>	getSimulatableCodeBehind();
		boost::shared_ptr<StateMachineCodeBehind>	getStateMachineCodeBehind();
		void										initialize();
		void										setIsCollidable();
		void										setIsInputReceiver();
		void										setIsMessageable();
		void										setIsRenderable();
		void										setIsSimulatable();
		void										setIsStateMachine();

	protected:

	private:

		EntityClassification						classification_;
		boost::shared_ptr<CodeBehindFactory>		codeBehindFactory_;
		boost::shared_ptr<CollidableCodeBehind>		collidableCodeBehind_;
		EntityControllerPtr			controller_;
		DebuggerPtr									debugger_;
		boost::shared_ptr<DynamicsControllerHolder>	dynamicsControllerHolder_;
		EntityTypeId								entityTypeId_;
		boost::shared_ptr<HitboxControllerHolder>	hitboxControllerHolder_;
		boost::shared_ptr<EntityCodeBehind>			entityCodeBehind_;
		boost::shared_ptr<HitboxManager>			hitboxManager_;
		boost::shared_ptr<BaseIds>					ids_;
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		boost::shared_ptr<InputReceiverCodeBehind>	inputReceiverCodeBehind_;
		bool										isCollidable_;
		bool										isInitialized_;
		bool										isInputReceiver_;
		bool										isMessageable_;
		bool										isRenderable_;
		bool										isSimulatable_;
		bool										isStateMachine_;
		boost::shared_ptr<MessageableCodeBehind>	messageableCodeBehind_;
		EntityMetadataPtr							metadata_;
		PositionPtr									position_;
		boost::shared_ptr<PythonInstanceWrapper>	pythonInstanceWrapper_;
		boost::shared_ptr<RenderableCodeBehind>		renderableCodeBehind_;
		boost::shared_ptr<Renderer>					renderer_;
		boost::shared_ptr<SimulatableCodeBehind>	simulatableCodeBehind_;
		StageControllerPtr							stageController_;
		boost::shared_ptr<StateMachineController>	stateMachineController_;
		boost::shared_ptr<StateMachineCodeBehind>	stateMachineCodeBehind_;
		int											tileSize_;
		boost::shared_ptr<GameTimer>				timer_;
		boost::shared_ptr<Ui>						ui_;
	};
}

#endif // _CODEBEHINDCONTAINER_HPP_
