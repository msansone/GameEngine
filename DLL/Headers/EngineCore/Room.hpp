/* -------------------------------------------------------------------------
** Room.hpp
**
** The Room class stores all of the entities in a room, along with its
** collision and render grids. Rooms are loaded in a background thread. Only
** one room will be displayed at a time, but multiple rooms could be updated.
** 
** Rooms can be serialized to save a snapshot of their current state.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ROOM_HPP_
#define _ROOM_HPP_

#include <boost/asio.hpp>
#include <boost/python.hpp>
#include <boost/python/module.hpp>
#include <boost/thread.hpp>
#include <boost/thread/mutex.hpp>
#include <deque>
#include <string>

#include "AudioSource.hpp"
#include "AudioSourceContainer.hpp"
#include "AudioSourceProperties.hpp"
#include "Assets.hpp"
#include "BaseIds.hpp"
#include "CameraManager.hpp"
#include "CodeBehindContainer.hpp"
#include "CollidableGrid.hpp"
#include "Entity.hpp"
#include "EntityCreationParameters.hpp"
#include "EntityTypeMap.hpp"
#include "NameValidator.hpp"
#include "ParticleEmitterCreationParameters.hpp"
#include "PhysicsManager.hpp"
#include "RoomMetadata.hpp"
#include "SpawnPoint.hpp"
#include "TileController.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	struct MoveEntityParameters
	{
		int						entityInstanceId;
		RoomId					moveFromRoomId;
		ChangeRoomParameters	changeRoomParameters;
		ShowRoomParameters		showRoomParameters;
	};

	typedef std::map<EntityTypeId, std::vector<int>> EntityTypeIdMap;
	typedef boost::shared_ptr<boost::asio::io_service> IoService;
	
	// Signal is called when an entity in this room is moved into another room. It gets passed up the chain to the RoomContainer.
	typedef boost::shared_ptr<boost::signals2::signal<void(MoveEntityParameters params)>> MoveEntitySignal;
	typedef boost::signals2::signal<void(MoveEntityParameters params)> MoveEntitySignalRaw;

	class Room
	{
	public:
		friend class GameEngine;
		friend class GameStateManager;
		friend class RoomManager;
		friend class RoomContainer;
		
		Room();
		virtual ~Room();

		void								loadRoomAsync();
		void								unloadRoomAsync();

		void								setFileName(std::string fileName);
		
		void								beginTransition(double transitionLength);
		bool								getIsTransitioning();
		double								getTransitionTimer();

		bool								getIsLoading();
		void								setIsLoading(bool value);

		bool								getIsLoaded();
		void								setIsLoaded(bool value);

		bool								getIsUnloading();

		int									getPercentLoaded();

		int									getLoadingScreenId();
		void								setLoadingScreenId(int value);

		void								setAssets(boost::shared_ptr<Assets> assets);
		
		int									getIndex();
		
		boost::shared_ptr<RoomMetadata>		getMetadataPy();
		boost::shared_ptr<RoomMetadata>		getMetadata();

		// Create an audio source entity and return its ID.
		AudioSourcePtr						createAudioSourcePy(AudioSourcePropertiesPtr audioSourceProperties);
		AudioSourcePtr						createAudioSource(AudioSourcePropertiesPtr audioSourceProperties);

		// Create an entity and return its ID.
		int									createEntity(EntityCreationParameters params);
		int									createEntityPy(EntityCreationParameters params);

		// Create a camera entity and return its ID.
		int									createCamera(bool isActive);
		int									createCameraPy(bool isActive);

		void								killEntity(int entityInstanceId);
		boost::shared_ptr<Entity>			extractEntity(int entityInstanceId);
		void								insertEntity(boost::shared_ptr<Entity> entity, SpawnPointId spawnPointId, int offsetX, int offsetY);
		
		boost::shared_ptr<Entity>			getEntityById(int entityId);
		boost::shared_ptr<Entity>			getEntityById(EntityTypeId entityType, int entityId);
		
		boost::shared_ptr<Entity>			getEntityByName(std::string entityInstanceName);
		boost::shared_ptr<Entity>			getEntityByName(EntityTypeId entityType, std::string entityInstanceName);
		
		boost::python::object				getEntityPyInstanceById(int entityId);
		boost::python::object				getEntityPyInstanceByIdWithType(EntityTypeId entityType, int entityId);
		boost::python::object				getEntityPyInstanceById(EntityTypeId entityType, int entityId);
		
		boost::python::object				getEntityPyInstanceByName(std::string entityInstanceName);
		boost::python::object				getEntityPyInstanceByName(EntityTypeId entityType, std::string entityInstanceName);
		
		EntityTypeMap						getEntityTypeMapPy();
		EntityTypeMap						getEntityTypeMap();

		int									createParticleEmitter(ParticleEmitterCreationParameters params);
		int									createParticleEmitterPy(ParticleEmitterCreationParameters params);
		
	private:

		struct EntityInsertQueueItem
		{
			boost::shared_ptr<Entity> entity;
			SpawnPointId spawnPointId;
			int offsetX;
			int offsetY;			
		};

		virtual void	roomLoaded();
		virtual void	roomLoading();
		virtual void	roomDisplayed();
		virtual void	roomHidden();
		virtual void	entityCreated(boost::shared_ptr<Entity> entity);
		virtual void	entityEntered(boost::shared_ptr<Entity> entity);
		virtual void	entityExited(boost::shared_ptr<Entity> entity);
		virtual void	update(double time);
		
		void			preRoomShown();
		void			preRoomHidden();
		void			preRoomActivated();
		
		void			initializeEntities();
		void			initializePythonData();
		
		void			processEntityMoveQueue();
		
		int				entityChangingName(int entityInstanceId, std::string name, bool append);
		void			entityChangingRoom(int entityInstanceId, RoomId moveFromRoomId, ChangeRoomParameters changeRoomParams, ShowRoomParameters showRoomParams);
		bool			entityRemoveEntity(int entityInstanceId, RoomId roomId);

		void			cleanup();
		// Adds an entity to the master list and registers it with all necessary subsystems.
		//
		// entity: The entity that be added to this room's master entities list.
		//
		// insertAtEnd: Whether to insert at the end of the master entities list or not. Newly created entities
		//              should be inserted at the end, because they will have a newly generated ID which is
		//              guaranteed to be the highest ID at the time of creation. Inserted entities however, might
		//				be any ID, and need to be inserted in correct sorted order.
		int				addEntity(boost::shared_ptr<Entity> entity, bool insertAtEnd);
		
		void			setActiveCamera(boost::shared_ptr<Entity> camera);
		
		void			setMovedEntityPositions();

		void			setEntityPosition(boost::shared_ptr<Entity> entity, ChangeRoomParameters params);
		
		void			resetAudioListener();
		void			playAudioSources();

		// This is the method that actually removes the entity. This shouldn't be visible to the user, 
		// as actual removals are handled internally by the engine. 
		// The public remove function only flags an entity to be removed, so that it can be removed when it is safe.
		// Allowing the user to remove entities in real time would break the collision system.
		void			removeEntity(int index, bool deleteEntity, bool runDestroy);
		void			removeTileEntity(int index);

		void			removeEntityRenderable(boost::shared_ptr<Entity> entity);

		void			buttonDown(boost::shared_ptr<InputEvent> inputEvent);
		void			buttonUp(boost::shared_ptr<InputEvent> inputEvent);

		void			createWaitingEntities();

		std::deque<EntityInsertQueueItem> entityInsertQueue_;
		
		template <typename QueryFunctor>
		std::vector<int>		findEntity(QueryFunctor compare);

		template <typename QueryFunctor>
		std::vector<int>		findEntity(EntityTypeId entityType, QueryFunctor compare);
		
		boost::shared_ptr<Entity>					activeCamera_;

		// Master list of all game entities, except tiles which don't get updated or removed.
		std::vector<boost::shared_ptr<Entity>>		masterEntityList_;

		std::vector<boost::shared_ptr<Entity>>		masterEntityListTiles_;

		// Parallel list to the master list. It contains of all game entity IDs, for easier searching.
		std::vector<int>							masterEntityIdList_;	

		// List that contains only entities that have a dynamics controller attached.
		std::vector<boost::shared_ptr<Entity>>		dynamicEntityList_;

		// Parallel list to the dynamic entity list. It contains the dynamic entity IDs, for easier searching.
		std::vector<int>							dynamicEntityIdList_;	
	
		// List that contains only entities that DON'T have a dynamics controller attached.
		std::vector<boost::shared_ptr<Entity>>		staticEntityList_;
		
		// Parallel list to the static entity list. It contains the dynamic entity IDs, for easier searching.
		std::vector<int>							staticEntityIdList_;	

		// A map of lists that contain entities of the key type. More optimal to use if you only want to deal
		// with a certain entity type.
		EntityTypeMap								entityTypeMap_;			
		
		// A map of parallel lists to the entityTypeMap lists. It contais entity IDs, for easier searching.
		EntityTypeIdMap								entityTypeIdMap_;		
		
		// Map an entity instance's name to its ID.
		std::map<std::string, int>					entityNameIdMap_;

		// List of spawn points in the room
		std::vector<boost::shared_ptr<SpawnPoint>>	spawnPoints_;

		std::vector<EntityCreationParameters>		entityWaitingList_;

		boost::shared_ptr<RoomMetadata>				roomMetadata_;

		PhysicsConfigPtr							physicsConfig_;

		std::string									fileName_;

		bool										showRoom_;
		bool										isLoaded_;
		bool										isLoading_; 
		bool										isActivated_;
		bool										isInitialized_;
		bool										isShowing_;
		bool										isUpdating_;
		bool										isUnloading_;
		bool										runLoadedEvent_;
		bool										isTransitioning_;
		bool										initialRender_;

		double										transitionTimeTotal_;
		double										transitionTimer_;
		

		boost::shared_ptr<AnchorPointManager>		anchorPointManager_;
		AnimationManagerPtr							animationManager_;
		boost::shared_ptr<Assets>					assets_;
		boost::shared_ptr<AudioPlayer>				audioPlayer_;
		boost::shared_ptr<AudioSourceContainer>		audioSourceContainer_;
		boost::shared_ptr<NameValidator>			audioSourceNameValidator_;
		boost::shared_ptr<CameraManager>			cameraManager_;
		boost::shared_ptr<CodeBehindFactory>		codeBehindFactory_;
		boost::shared_ptr<Debugger>					debugger_;		
		boost::shared_ptr<EngineConfig>				engineConfig_;
		boost::shared_ptr<EngineController>			engineController_;
		boost::shared_ptr<EntityMetadataContainer>	entityMetadataContainer_;
		boost::shared_ptr<NameValidator>			entityNameValidator_;
		boost::shared_ptr<FontManager>				fontManager_;
		boost::shared_ptr<HitboxManager>			hitboxManager_;
		boost::shared_ptr<BaseIds>					ids_;
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		boost::shared_ptr<Messenger>				messenger_;
		boost::shared_ptr<PhysicsManager>			physicsManager_;
		boost::shared_ptr<QueryManager>				queryManager_;
		boost::shared_ptr<QueryParametersFactory>	queryParametersFactory_;
		boost::shared_ptr<QueryResultFactory>		queryResultFactory_;
		boost::shared_ptr<RenderableManager>		renderableManager_;
		boost::shared_ptr<Renderer>					renderer_;
		boost::shared_ptr<RoomMetadataContainer>	roomMetadataContainer_;
		boost::shared_ptr<TextManager>				textManager_;
		boost::shared_ptr<GameTimer>				timer_;
		boost::shared_ptr<Ui>						ui_;

		ButtonSignal								buttonDownSignal_;
		ButtonSignal								buttonUpSignal_;
		MoveEntitySignal							moveEntitySignal_;

		// Scripting data
		std::string									scriptTypeName_;
		std::string									scriptVar_;

		boost::python::object						pyMainModule_;
		boost::python::object						pyMainNamespace_;
		boost::python::object						pyRoomInstance_;
		boost::python::object						pyRoomInstanceNamespace_;
		boost::python::object						pyRoomLoaded_;
		boost::python::object						pyRoomLoading_;
		boost::python::object						pyEntityCreated_;
		boost::python::object						pyEntityEntered_;
		boost::python::object						pyEntityExited_;
		boost::python::object						pyUpdate_;
		boost::python::object						pyRoomDisplayed_;
		boost::python::object						pyRoomHidden_;
	};

	// Search through the entity list based on the criteria provided by a user defined functor object.
	// Return a list of the entity IDs.
	template <typename QueryFunctor>
	std::vector<int> Room::findEntity(QueryFunctor queryFunctor)
	{
		std::vector<int> foundEntityIDs;

		std::vector<boost::shared_ptr<Entity>>::iterator itr;

		itr = masterEntityList_.begin();

		while (itr != masterEntityList_.end())
		{
			itr = std::find_if(itr, masterEntityList_.end(), queryFunctor);

			if (itr != masterEntityList_.end())
			{
				foundEntityIDs.push_back((*itr)->getEntityID());
				itr++;
			}
		}

		return foundEntityIDs;
	}
	
	// Search through a specific entity type list based on the criteria provided by a user defined functor object.
	// Return a list of the entity IDs.
	template <typename QueryFunctor>
	std::vector<int> Room::findEntity(EntityTypeId entityType, QueryFunctor queryFunctor)
	{
		std::vector<int> foundEntityIDs;

		std::vector<boost::shared_ptr<Entity>> entityList = entityTypeMap_.map_[entityType].entityList_;
		std::vector<boost::shared_ptr<Entity>>::iterator itr;

		itr = entityList.begin();

		while (itr != entityList.end())
		{
			itr = std::find_if(itr, entityList.end(), queryFunctor);

			if (itr != entityList.end())
			{
				foundEntityIDs.push_back((*itr)->getEntityInstanceId());
				itr++;
			}
		}

		return foundEntityIDs;
	}
}

#endif // _ROOM_HPP_