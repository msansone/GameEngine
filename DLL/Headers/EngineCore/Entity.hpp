/* -------------------------------------------------------------------------
** Entity.hpp
** 
** The Entity class is the base class from which all entities in the game are
** derived. An entity is anything that exists in the game world. Examples include
** actors, events, HUD elements, and the camera. Entities may have any of the following
** subsystem components attached: dynamics controller, state machine controller, or hitbox
** container.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITY_HPP_
#define _ENTITY_HPP_

#include <math.h>
#include <unordered_set>

#include <boost/algorithm/string.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/thread.hpp>

#include "EntityComponents.hpp"
#include "PhysicsConfig.hpp"
#include "StateMachineController.hpp"
#include "TileRenderable.hpp"

namespace firemelon
{
	typedef boost::shared_ptr<boost::signals2::signal<int (int entityInstanceId, std::string name, bool append)>> EntityChangeNameSignal;
	typedef boost::signals2::signal<int (int entityInstanceId, std::string name, bool append)> EntityChangeNameSignalRaw;

	typedef boost::shared_ptr<boost::signals2::signal<void(int entityInstanceId, RoomId moveFromRoomId, ChangeRoomParameters changeRoomParams, ShowRoomParameters showRoomParams)>> EntityChangeRoomSignal;
	typedef boost::signals2::signal<void(int entityInstanceId, RoomId moveFromRoomId, ChangeRoomParameters changeRoomParams, ShowRoomParameters showRoomParams)> EntityChangeRoomSignalRaw;

	typedef boost::shared_ptr<boost::signals2::signal<bool (int entityInstanceId, RoomId roomId)>> EntityRemoveEntitySignal;
	typedef boost::signals2::signal<bool (int entityInstanceId, RoomId roomId)> EntityRemoveEntitySignalRaw;

	enum EntityUpdateType
	{
		NONE = 1,
		PLAYER = 2,
		SIMULATED = 3
	};

	class Entity
	{
	public:
		friend class GameEngine;
		friend class GameStateManager;
		friend class RoomManager;
		friend class Room;

		Entity(boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger, PhysicsConfigPtr physicsConfig, HitboxManagerPtr hitboxManager);
		virtual ~Entity();

		boost::shared_ptr<EntityComponents>	getComponents();

	protected:
		
		//void						attachRenderable();
		void						remove();

	private:
		
		void						cleanup();
		void						validate();
		void						invalidate();
		void						initialize();

		virtual DynamicsController*	createDynamicsController();
		
		void						attachInputDeviceManager(boost::shared_ptr<InputDeviceManager> inputDeviceManager_);
		int							changeName(std::string name, bool append);
		void						changeRoom(ChangeRoomParameters changeRoomParams, ShowRoomParameters showRoomParams);
		
		void						setAttachedCameraId(int entityInstanceId);
		int							getAttachedCameraId();
		
		void						setAttachCamera(bool attachCamera);
		bool						getAttachCamera();

		boost::shared_ptr<EntityComponents>			components_;

		DebuggerPtr					debugger_;
		
		boost::shared_ptr<RoomMetadataContainer>	roomMetadataContainer_;

		boost::shared_ptr<BaseIds>	ids_;
		int							attachedCameraInstanceId_;
		TransitionId				transitionId_;

		int							instanceId_;

		bool						isActive_;
		bool						isInitialized_;
		
		// An entity is invalidated if it is removed or has been extracted.
		// Entities need to be invalidated so that any collision events involving them are not processed.
		bool						isInvalidated_;	

		bool						keepRoomActive_;
		bool						attachCamera_;

		
		EntityChangeNameSignal		changeNameSignal_;
		EntityChangeRoomSignal		entityChangingRoomSignal_;
		EntityRemoveEntitySignal	removeEntitySignal_;
	};
}

#endif // _ENTITY_HPP_