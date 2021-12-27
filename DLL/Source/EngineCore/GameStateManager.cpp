#include "..\..\Headers\EngineCore\GameStateManager.hpp"

using namespace firemelon;
using namespace boost::python;

GameStateManager::GameStateManager(boost::shared_ptr<Ui> ui, boost::shared_ptr<Renderer> renderer,
								   boost::shared_ptr<AudioPlayer> audioPlayer, boost::shared_ptr<RoomManager> roomManager, boost::shared_ptr<InputDeviceManager> inputDeviceManager,
								   boost::shared_ptr<PhysicsManager> physicsManager, boost::shared_ptr<QueryFactory> queryFactory, boost::shared_ptr<QueryResultFactory> queryResultFactory,
								   boost::shared_ptr<QueryParametersFactory> queryParametersFactory, boost::shared_ptr<RenderableManager> renderableManager, boost::shared_ptr<TextManager> textManager,
								   boost::shared_ptr<Messenger> messenger, boost::shared_ptr<QueryManager> queryManager, AnimationManagerPtr animationManager, boost::shared_ptr<HitboxManager> hitboxManager,
								   boost::shared_ptr<AnchorPointManager> anchorPointManager, boost::shared_ptr<EntityMetadataContainer> entityMetadataContainer, boost::shared_ptr<GameTimer> timer,
								   boost::shared_ptr<RoomContainer> roomContainer, boost::shared_ptr<FontManager> fontManager, boost::shared_ptr<CollisionDispatcher> collisionDispatcher,
	                               boost::shared_ptr<EngineController> engineController, DebuggerPtr debugger)
{	
	isInitialized_ = false;
	totalTime_ = 0.0;
	
	ui_ = ui;
	renderer_ = renderer;
	audioPlayer_ = audioPlayer;
	roomManager_ = roomManager;
	inputDeviceManager_ = inputDeviceManager;
	physicsManager_ = physicsManager;
	queryFactory_ = queryFactory; 
	queryResultFactory_ = queryResultFactory;
	queryParametersFactory_ = queryParametersFactory;
	renderableManager_ = renderableManager;
	textManager_ = textManager;
	messenger_ = messenger; 
	queryManager_ = queryManager;
	animationManager_ = animationManager;
	hitboxManager_ = hitboxManager;
	anchorPointManager_ = anchorPointManager;
	entityMetadataContainer_ = entityMetadataContainer;
	timer_ = timer;
	roomContainer_ = roomContainer;
	fontManager_ = fontManager;
	collisionDispatcher_ = collisionDispatcher;
	engineController_ = engineController;
	debugger_ = debugger;
}

GameStateManager::~GameStateManager()
{
	bool debug = true;
}

void GameStateManager::initialize()
{
	if (isInitialized_ == false)
	{
		roomManager_->animationManager_ = animationManager_;
		roomManager_->hitboxManager_ = hitboxManager_;
		roomManager_->anchorPointManager_ = anchorPointManager_;
		roomManager_->physicsManager_ = physicsManager_;
		roomManager_->audioPlayer_ = audioPlayer_;
		roomManager_->renderer_ = renderer_;
		roomManager_->ui_ = ui_;
		roomManager_->queryResultFactory_ = queryResultFactory_;
		roomManager_->queryParametersFactory_ = queryParametersFactory_;
		roomManager_->inputDeviceManager_ = inputDeviceManager_;
		roomManager_->renderableManager_ = renderableManager_;
		roomManager_->fontManager_ = fontManager_;
		roomManager_->messenger_ = messenger_;
		roomManager_->queryManager_ = queryManager_;
		roomManager_->timer_ = timer_;
		roomManager_->entityMetadataContainer_ = entityMetadataContainer_;

		roomManager_->initialize();

		messenger_->postMessageSignal.connect(boost::bind(&GameStateManager::postMessageEvent, this, _1));

		queryManager_->postQuerySignal.connect(boost::bind(&GameStateManager::postQuery, this, _1));
		queryManager_->getQuerySignal.connect(boost::bind<QueryContainer>(&GameStateManager::getQuery, this, _1));
		queryManager_->closeQuerySignal.connect(boost::bind(&GameStateManager::closeQuery, this, _1));

		timerId_ = timer_->addTimer();

		isInitialized_ = true;
	}
}

void GameStateManager::loadRoomByName(std::string roomName, stringmap roomParameters)
{
	roomManager_->loadRoomByName(roomName, roomParameters);
}

//void GameStateManager::loadRoom(RoomId roomId, stringmap roomParameters)
void GameStateManager::loadRoom(RoomId roomId)
{
	//roomManager_->loadRoom(roomId, roomParameters);
	roomManager_->loadRoom2(roomId);
}

void GameStateManager::showRoom(RoomId roomId)
{
	roomManager_->showRoomImmediate(roomId);
}

void GameStateManager::loadAssets()
{
	roomManager_->loadAssets();
}

void GameStateManager::render(int roomIndex, double lerp)
{
	roomManager_->renderableManager_->render(roomIndex, lerp);
}

void GameStateManager::update(double time)
{
	boost::thread::id threadId = boost::this_thread::get_id();

	RoomId shownRoomId = roomManager_->roomContainer_->getShownRoomId();

	int shownRoomIndex = roomManager_->roomContainer_->getRoomIndex(shownRoomId);

	//// BUG0001: Moved outside of the GSM the update function to fix a bug where the room renders incorrectly for one single frame
	//// when changing rooms. 
	/*int size = renderableManager_->roomRenderDataList_[shownRoomIndex]->mapLayers_.size();
	for (int i = 0; i < size; i++)
	{
		renderableManager_->roomRenderDataList_[shownRoomIndex]->populateLayerRenderData(i, 1.0);
	}*/

	if (engineController_->advanceOneFrame_ == true)
	{
		std::cout << "Updating local game state..." << std::endl;
	}

	totalTime_ += time;

	roomManager_->moveAllEntities();

	std::priority_queue<int> removalQueue;

	// First, dispatch messages from the previous frame to the entities
	dispatchAllMessages();


	/*
	BUG0001: I had to move this out of the GSM and into the engine itself, because of a bug. What was happening is I was swapping to the assassin,
	who had not been created yet and was in a room that was not yet loaded. So, the room load would be triggered, and the assassin was added
	to the waiting to be created entity list. Because the entities in this list were created as a part of the simulation update, there was
	a brief frame or two where the room was rendering incorrectly, because it could was not populating the room render data correctly,
	because the new active camera was not yet created.

	// Call the RoomLoaded event for any rooms that are flagged as newly loaded.
	int roomCount = roomManager_->roomContainer_->getRoomCount();
		
	for (int i = 0; i < roomCount; i++)
	{
		boost::shared_ptr<Room> room = roomManager_->roomContainer_->getRoomByIndex(i);
			
		if (room->runLoadedEvent_ == true)
		{
			room->createWaitingEntities();

			room->processEntityMoveQueue();

			room->runLoadedEvent_ = false;
				
		//	room->roomLoaded();
		}
	}
	*/

	// Changing this to only update the shown room. I tried to have it update multiple rooms when it was going to
	// have online play, so players could be in more than one room, but I'm scrapping that for now because it
	// adds far too much work.

	boost::shared_ptr<Room> shownRoom = roomManager_->roomContainer_->getShownRoom();
			
	// If the room is unloading, don't update.
	if (shownRoom->isUnloading_ == false)
	{
		shownRoom->isUpdating_ = true;

		shownRoom->update(time);

		// Don't update the text manager if the simulation is currently suspending updates.
		// This was changed on 8/11/2020, when I moved the suspend updates into the GameEngine update function.
		// Given that this block used to be surrounded by "if simulationIsStopped == false", I don't see how this ever could have mattered. 
		// Maybe it was itself a relic of old code structure?
		//if (simulationIsStopped == true)
		//{
		roomManager_->textManager_->preUpdate(shownRoomIndex);
		//}
			
		// Update sprites and animations.
		roomManager_->renderableManager_->update(shownRoomIndex, time);

		// Run all of the entities' user defined frame begin functions.
		int size = shownRoom->masterEntityList_.size();

		for (int j = 0; j < size; j++)
		{
			boost::shared_ptr<Entity> entity = shownRoom->masterEntityList_[j];

			boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entity->getComponents()->getCodeBehindContainer();
			boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();

			if (entityCodeBehind->getIsRemoved() == true)
			{
				removalQueue.push(j);
			}
			else
			{
				if (codeBehindContainer->getIsSimulatable() == true)
				{
					codeBehindContainer->getSimulatableCodeBehind()->frameBegin();
				}
			}
		}

		// At this point, all entities that are going to be removed are known. It is now safe to build the list
		// of previously collided entities in the physics manager. It will be guaranteed that no removed entities
		// will end up in the list.
		collisionDispatcher_->prepareFrame(shownRoomIndex);

		// Remove all entities that are flagged for removal.		
		while (removalQueue.empty() == false)
		{
			int entityIndex = removalQueue.top();

			shownRoom->removeEntity(entityIndex, true, false);

			removalQueue.pop();
		}

		// Run all of the entities' user defined pre-integration update functions.
		// These must be called before the physics integration step.
		size = shownRoom->masterEntityList_.size();

		for (int j = 0; j < size; j++)
		{
			boost::shared_ptr<Entity> entity = shownRoom->masterEntityList_[j];

			boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entity->getComponents()->getCodeBehindContainer();

			boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();

			if (entityCodeBehind->getIsRemoved() == true)
			{
				removalQueue.push(j);
			}
			else
			{
				entityCodeBehind->preUpdate(time);

				if (codeBehindContainer->getIsCollidable() == true)
				{
					codeBehindContainer->getCollidableCodeBehind()->prepareFrame(); //formerly internalPreUpdate
				}

				if (codeBehindContainer->getIsSimulatable() == true)
				{
					codeBehindContainer->getSimulatableCodeBehind()->preIntegration();
				}
			}
		}

		// Update the physics manager (integration step)
		physicsManager_->update(shownRoomIndex, time);

		// Run the collision detection routine.
		physicsManager_->collisionDetection(shownRoomIndex);

		// Run all of the entities' user defined update complete functions.
		size = shownRoom->masterEntityList_.size();
		for (int j = 0; j < size; j++)
		{
			boost::shared_ptr<Entity> entity = shownRoom->masterEntityList_[j];

			boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entity->getComponents()->getCodeBehindContainer();
			boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();

			if (entityCodeBehind->getIsRemoved() == true)
			{
				removalQueue.push(j);
			}
			else
			{
				if (codeBehindContainer->getIsSimulatable() == true)
				{
					codeBehindContainer->getSimulatableCodeBehind()->postIntegration();
				}
			}
		}

		// Don't update the text manager if the ui is currently suspending updates.
		
		// Same as above in this functoin. This was changed on 8/11/2020, when I moved the suspend updates into the GameEngine update function.
		// Given that this block used to be surrounded by "if simulationIsStopped == false", I don't see how this ever could have mattered. 
		// Maybe it was itself a relic of old code structure? Leave it here until I feel comfortable that it is indeed useless.
		//if (simulationIsStopped == false)
		//{
		roomManager_->textManager_->update(shownRoomIndex, time);
		//}
			
		// Note: This used to be contained in a separate postUpdate function, but this was problematic because it needs to 
		// occur in synch with the game state updated, and due to consuming the time in fixed timesteps, sometimes that
		// didn't occur if two timesteps were consumed in one frame, because the postUpdate was only called once at the 
		// end of the frame.
		// A bug manifested where gravity was applying twice if this loop ran twice, because it wasn't getting cleared.
		// Why was it originally moved outside of this loop to begin with? Did it actually address some issue, or was it a
		// vestigial artifact of a previous structure? Remember this if some issue arises in the future where the solution 
		// appears to be moving it to the very end of the game update loop, because that is wrong.
		physicsManager_->postUpdate(shownRoomIndex);

		shownRoom->isUpdating_ = false;
	}
}

void GameStateManager::postUpdate()
{
}

void GameStateManager::postMessageEvent(Message message)
{
	// If this message does not have immediate priority, add it to the message queue
	// Process and dispatch messages as part of the update.
	MessagePriority priority = message.getPriority();

	if (priority == MESSAGE_PRIORITY_IMMEDIATE)
	{
		dispatchSingleMessage(message);
	}
	else if (priority == MESSAGE_PRIORITY_NORMAL)
	{
		messageQueue_.push_back(message);
	}
}

QueryContainer GameStateManager::getQuery(QueryId queryId)
{
	boost::shared_ptr<Query> currentQuery = queryFactory_->createQueryBase(queryId);

	currentQuery->queryType_ = queryId;

	ScriptingData scriptingData = BaseIds::idScriptDataMap[queryId];
	
	currentQuery->scriptTypeName_ = scriptingData.getScriptTypeName();
	currentQuery->scriptName_ = scriptingData.getFileName();
	
	currentQuery->debugger_ = debugger_;
	currentQuery->roomManager_ = roomManager_;
	currentQuery->roomContainer_ = roomContainer_;

	currentQuery->initialize();
	
	activeQueries_[currentQuery->id_] = currentQuery;

	QueryContainer queryContainer(queryId);
	
	queryContainer.id_ = currentQuery->id_;
	queryContainer.parameters_ = currentQuery->pyParameters_;
	queryContainer.result_ = currentQuery->pyResult_;

	return queryContainer;
}

void GameStateManager::postQuery(QueryContainer &queryContainer)
{
	// Get the query from the active queries map.
	boost::shared_ptr<Query> query = activeQueries_[queryContainer.id_];

	if (query != nullptr)
	{
		query->runQuery(queryContainer);
	}
}

void GameStateManager::closeQuery(QueryContainer &queryContainer)
{
	// If the query exists in the active queries map, remove it and free the memory.
	std::map<QueryId, boost::shared_ptr<Query>>::iterator itr;

	itr = activeQueries_.find(queryContainer.id_);

	if (itr != activeQueries_.end())
	{
		boost::shared_ptr<Query> query = activeQueries_[queryContainer.id_];

		activeQueries_.erase(queryContainer.id_);
	}
}

void GameStateManager::setRoomLoader(boost::shared_ptr<RoomLoader> roomLoader)
{
	roomManager_->roomLoader_ = roomLoader;
	
	roomManager_->roomLoader_->debugger_ = debugger_;
	roomManager_->roomLoader_->queryResultFactory_ = queryResultFactory_;
	roomManager_->roomLoader_->queryParametersFactory_ = queryParametersFactory_;
	roomManager_->roomLoader_->textManager_ = roomManager_->textManager_;
	roomManager_->roomLoader_->queryManager_ = roomManager_->queryManager_;
	roomManager_->roomLoader_->audioPlayer_ = roomManager_->audioPlayer_;
	roomManager_->roomLoader_->messenger_ = roomManager_->messenger_;

	// Connect the query system signal.
	roomManager_->roomLoader_->postQuerySignal.connect(boost::bind(&GameStateManager::postQuery, this, _1));
	roomManager_->roomLoader_->postMessageSignal.connect(boost::bind(&GameStateManager::postMessageEvent, this, _1));
}

void GameStateManager::setFullscreen(bool value)
{
	roomManager_->isFullscreen_ = value;
}

void GameStateManager::dispatchAllMessages()
{
	int size = messageQueue_.size();
	for (int i = 0; i < size; i++)
	{
		dispatchSingleMessage(messageQueue_[i]);
	}

	// All messages have been dispatched for this frame. Empty the queue.
	messageQueue_.clear();
}

void GameStateManager::dispatchSingleMessage(Message message)
{
	RoomId roomId = message.getRoomId();

	boost::shared_ptr<Room> room = roomManager_->roomContainer_->getRoom(roomId);

	if (room != nullptr)
	{
		// First, dispatch the message to all of the receiving entity types.
		int size = message.getReceiverEntityTypeCount();
		for (int i = 0; i < size; i++)
		{
			EntityTypeId type = message.getReceiverEntityType(i);

			std::vector<boost::shared_ptr<Entity>> entityList = room->entityTypeMap_.map_[type].entityList_;

			int entityListSize = entityList.size();
			for (int j = 0; j < entityListSize; j++)
			{
				if (entityList[j]->getComponents()->getCodeBehindContainer()->getIsMessageable() == true)
				{
					entityList[j]->getComponents()->getCodeBehindContainer()->getMessageableCodeBehind()->messageReceived(message);
				}
			}
		}

		// Now, dispatch to any individual receivers, via their ID.
		size = message.getReceiverIdCount();
		for (int i = 0; i < size; i++)
		{
			std::vector<int>::iterator itr;

			itr = find(room->masterEntityIdList_.begin(), room->masterEntityIdList_.end(), message.getReceiverId(i));

			if (itr != room->masterEntityIdList_.end())
			{
				if (room->masterEntityList_[*itr]->getComponents()->getCodeBehindContainer()->getIsMessageable() == true)
				{
					room->masterEntityList_[*itr]->getComponents()->getCodeBehindContainer()->getMessageableCodeBehind()->messageReceived(message);
				}
			}
		}
	}
	else
	{
		std::cout << "Unable to deliver message. Room " << roomId << " not found." << std::endl;
	}

}

bool GameStateManager::getIsRoomLoaded()
{
	return roomManager_->isRoomLoaded_;
}