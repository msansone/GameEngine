/* -------------------------------------------------------------------------
** EntityController.hpp
**
** The EntityController class is used to allow entities to call entity 
** functions from inside python scripts. There must be a corresponding python 
** function in the base python entity class for each method, which will call 
** the entityController object which serves as a sort of proxy.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYCONTROLLER_HPP_
#define _ENTITYCONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS


#include <boost/python.hpp>
#include <boost/signals2.hpp>
#include <boost/enable_shared_from_this.hpp>

#include "AnchorPointManager.hpp"
#include "AnimationManager.hpp"
#include "AudioSourceContainer.hpp"
#include "BaseIds.hpp"
#include "Debugger.hpp"
#include "DynamicsControllerHolder.hpp"
#include "EngineConfig.hpp"
#include "EngineController.hpp"
#include "EntityMetadata.hpp"
#include "GameTimer.hpp"
#include "HitboxControllerHolder.hpp"
#include "InputDeviceManager.hpp"
#include "Messenger.hpp"
#include "Position.hpp"
#include "PythonGil.hpp"
#include "QueryContainer.hpp"
#include "QueryManager.hpp"
#include "QueryParametersFactory.hpp"
#include "QueryResult.hpp"
#include "QueryResultFactory.hpp"
#include "Renderable.hpp"
#include "Renderer.hpp"
#include "RoomMetadataContainer.hpp"
#include "StageController.hpp"
#include "StateMachineController.hpp"
#include "TextManager.hpp"
#include "Types.hpp"
#include "Ui.hpp"

namespace firemelon
{
	struct ShowRoomParameters
	{
		// The room to show.
		RoomId			roomId;

		// The transition to use.
		TransitionId	transitionId;

		// The length of the transition, in seconds.
		double			transitionTime;
	};

	struct ChangeRoomParameters
	{
		RoomId			roomId;
		SpawnPointId	spawnPointId;
		int				offsetX;
		int				offsetY;
		bool			delayUntilRoomShown;
	};

	typedef boost::shared_ptr<boost::signals2::signal<int (std::string name, bool append)>> ChangeNameSignal;
	typedef boost::signals2::signal<int (std::string name, bool append)> ChangeNameSignalRaw;

	typedef boost::shared_ptr<boost::signals2::signal<void (ChangeRoomParameters changeRoomParams, ShowRoomParameters showRoomParams)>> ChangeRoomSignal;
	typedef boost::signals2::signal<void (ChangeRoomParameters changeRoomParams, ShowRoomParameters showRoomParams)> ChangeRoomSignalRaw;
	
	typedef boost::shared_ptr<boost::signals2::signal<void ()>> RemoveEntitySignal;
	typedef boost::signals2::signal<void ()> RemoveEntitySignalRaw;
	
	class FIREMELONAPI EntityController : public boost::enable_shared_from_this<EntityController>
	{
	public:
		friend class Entity;
		friend class EntityComponents;
		friend class Room;
		friend class GameStateManager;
		friend class CollisionDispatcher;
		
		EntityController();
		virtual ~EntityController();

		void								attachAudioSourcePy(AudioSourcePtr audioSource);
		void								attachAudioSource(AudioSourcePtr audioSource);

		void								detachAudioSourcePy(AudioSourcePtr audioSource);
		void								detachAudioSource(AudioSourcePtr audioSource);

		// Components
		boost::shared_ptr<EntityMetadata>	getMetadata();
		boost::shared_ptr<Position>			getPosition();
		DynamicsController*					getDynamicsController();
		
		void								setPosition(int x, int y);	
		
		void								changeRoomPy(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime, bool showRoomAfterMove);
		void								changeRoom(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime, bool showRoomAfterMove);

		int									changeNamePy(std::string name, bool append);
		int									changeName(std::string name, bool append);
		
		void								removePy();
		void								remove();

		// Attach this entity to another entity.
		void								attachToPy(boost::shared_ptr<EntityController> attachedTo);
		void								attachTo(boost::shared_ptr<EntityController> attachedTo);

		// Detach this entity from the entity it is attached to.
		void								detachPy();
		void								detach();

		boost::shared_ptr<EntityController>	getAttachedToPy();
		boost::shared_ptr<EntityController>	getAttachedTo();

		void								setAttachmentAxisPy(Axis axis);
		void								setAttachmentAxis(Axis axis);

		int									getAttachedEntityCountPy();
		int									getAttachedEntityCount();

		void								addAttachedEntityPy(boost::shared_ptr<EntityController> attachedEntity);
		void								addAttachedEntity(boost::shared_ptr<EntityController> attachedEntity);

		boost::shared_ptr<EntityController>	getAttachedEntityPy(int index);
		boost::shared_ptr<EntityController>	getAttachedEntity(int index);

		void								removeAttachedEntity(boost::shared_ptr<EntityController> attachedEntity);

		boost::shared_ptr<Renderable>		getRenderable(int index = 0);
		
		int									getRenderableCount();

		StateMachineControllerPtr			getStateMachineController();

		StageControllerPtr					getStageController();

	protected:
		
		void										attachDynamicsController();
		
		virtual void								createRenderables();
		
		boost::shared_ptr<AnchorPointManager>		getAnchorPointManager();
		AnimationManagerPtr							getAnimationManager();
		DebuggerPtr									getDebugger();
		boost::shared_ptr<AudioPlayer>				getAudioPlayer();
		boost::shared_ptr<HitboxManager>			getHitboxManager();
		boost::shared_ptr<InputDeviceManager>		getInputDeviceManager();
		boost::shared_ptr<Messenger>				getMessenger();
		boost::shared_ptr<QueryManager>				getQueryManager();
		boost::shared_ptr<QueryParametersFactory>	getQueryParametersFactory();
		boost::shared_ptr<QueryResultFactory>		getQueryResultFactory();
		boost::shared_ptr<Renderer>					getRenderer();
		boost::shared_ptr<RoomMetadataContainer>	getRoomMetadataContainer();
		boost::shared_ptr<TextManager>				getTextManager();
		boost::shared_ptr<GameTimer>				getTimer();

		void										addRenderable(boost::shared_ptr<Renderable> renderable);
		void										setStageController(StageControllerPtr stageController);
		void										setStateMachineController(StateMachineControllerPtr stateMachineController);

		boost::shared_ptr<HitboxControllerHolder>	hitboxControllerHolder_;
		boost::shared_ptr<BaseIds>					ids_;
		bool										renderablesCreated_;

	private:

		virtual void								cleanup();
		virtual boost::shared_ptr<EntityMetadata>	createMetadata();
		bool										cyclicalAttachmentExists(int ownerId);
		virtual void								removed();
		virtual void								roomChanged(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime);

		ChangeNameSignal		changeNameSignal_;
		ChangeRoomSignal		changeRoomSignal_;
		RemoveEntitySignal 		removeEntitySignal_;
				

		boost::shared_ptr<AnchorPointManager>		anchorPointManager_;
		AnimationManagerPtr							animationManager_;
		boost::shared_ptr<AudioPlayer>				audioPlayer_;
		boost::shared_ptr<AudioSourceContainer>		audioSourceContainer_;
		DebuggerPtr									debugger_;
		boost::shared_ptr<DynamicsControllerHolder>	dynamicsControllerHolder_;
		boost::shared_ptr<EngineConfig>				engineConfig_;
		boost::shared_ptr<EngineController>			engineController_;
		boost::shared_ptr<FontManager>				fontManager_;
		boost::shared_ptr<HitboxManager>			hitboxManager_;
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		boost::shared_ptr<Messenger>				messenger_;
		boost::shared_ptr<EntityMetadata>			metadata_;
		boost::shared_ptr<Position>					position_;
		boost::shared_ptr<QueryManager>				queryManager_;
		boost::shared_ptr<QueryParametersFactory>	queryParametersFactory_;
		boost::shared_ptr<QueryResultFactory>		queryResultFactory_;
		std::vector<boost::shared_ptr<Renderable>>	renderables_;
		boost::shared_ptr<Renderer>					renderer_;
		boost::shared_ptr<RoomMetadataContainer>	roomMetadataContainer_;
		StageControllerPtr							stageController_;
		StateMachineControllerPtr					stateMachineController_;
		boost::shared_ptr<TextManager>				textManager_;
		boost::shared_ptr<GameTimer>				timer_;
		
		// Attachment data members.

		// The entity controller object that this entity controller is attached to.
		// Attaching keeps the position in synch with the attached entity controller.
		// You can only be attached to one at a time.
		boost::shared_ptr<EntityController>					attachedTo_;

		// List of entity controllers that are currently attached to this one.
		std::vector<boost::shared_ptr<EntityController>>	attachedEntities_;
	};

	typedef boost::shared_ptr<EntityController> EntityControllerPtr;
}

#endif // _ENTITYCONTROLLER_HPP_