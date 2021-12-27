#include "..\..\Headers\EngineCore\RoomContainer.hpp"

using namespace firemelon;

RoomContainer::RoomContainer() : 
	showRoomSignal_(new ShowRoomSignalRaw)
{
	shownRoomId_ = -1;	
	previousShownRoomId_ = -1;
}

RoomContainer::~RoomContainer()
{
}

void RoomContainer::cleanup()
{
	inputDeviceManager_->buttonDownSignal_.disconnect(boost::bind(&RoomContainer::buttonDown, this, _1));
	inputDeviceManager_->buttonUpSignal_.disconnect(boost::bind(&RoomContainer::buttonUp, this, _1));

	int size = rooms_.size();

	for (int i = 0; i < size; i++)
	{
		rooms_[i]->cleanup();
	}

	rooms_.clear();
}

void RoomContainer::initialize()
{
	inputDeviceManager_->buttonDownSignal_.connect(boost::bind(&RoomContainer::buttonDown, this, _1));
	inputDeviceManager_->buttonUpSignal_.connect(boost::bind(&RoomContainer::buttonUp, this, _1));
}

void RoomContainer::buttonDown(boost::shared_ptr<InputEvent> inputEvent)
{
	int roomIndex = roomIdToIndexMap_[shownRoomId_];

	rooms_[roomIndex]->buttonDown(inputEvent);
}

void RoomContainer::buttonUp(boost::shared_ptr<InputEvent> inputEvent)
{
	int roomIndex = roomIdToIndexMap_[shownRoomId_];

	rooms_[roomIndex]->buttonUp(inputEvent);
}


bool RoomContainer::getIsRoomLoadingOrUnloading()
{
	int size = rooms_.size();

	for (int i = 0; i < size; i++)
	{
		if (rooms_[i]->getIsLoading() || rooms_[i]->getIsUnloading())
		{
			return true;
		}
	}

	return false;
}

void RoomContainer::preloadRooms()
{
	boost::filesystem::path rooms(".\\Data\\Rooms\\");
	boost::filesystem::directory_iterator end_itr;

	// Load the room files
	for (boost::filesystem::directory_iterator itr(rooms); itr != end_itr; ++itr)
    {
		boost::filesystem::path filepath = itr->path();
		boost::filesystem::path extension = itr->path().extension();

        if (extension == ".rm") 
		{
			std::ifstream roomFile;
	
			roomFile.open(filepath.c_str(), std::ios::in | std::ios::binary);

			if (roomFile.is_open())
			{
				int fileMajorVersion = 0;
				roomFile.read((char*)&fileMajorVersion, sizeof(int));

				int fileMinorVersion = 0;
				roomFile.read((char*)&fileMinorVersion, sizeof(int));
		
				int fileRevisionVersion = 0;
				roomFile.read((char*)&fileRevisionVersion, sizeof(int));

				int uuidSize = boost::uuids::uuid::static_size();
			
				int roomNameSize = 0;

				roomFile.read((char*)&roomNameSize, sizeof(int));

				std::string roomName(roomNameSize, '\0');
				roomFile.read(&roomName[0], roomNameSize);
			
				int scriptNameSize = 0;

				roomFile.read((char*)&scriptNameSize, sizeof(int));

				std::string scriptName(scriptNameSize, '\0');
				roomFile.read(&scriptName[0], scriptNameSize);
			
				int pythonVariableNameSize = 0;

				roomFile.read((char*)&pythonVariableNameSize, sizeof(int));

				std::string pythonVariableName(pythonVariableNameSize, '\0');
				roomFile.read(&pythonVariableName[0], pythonVariableNameSize);
			
				// Room ID
				boost::uuids::uuid roomUuid;
				char* uuidBytes = new char[uuidSize];
				roomFile.read(uuidBytes, uuidSize);		
				memcpy(&roomUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				RoomId roomId = BaseIds::getIntegerFromUuid(roomUuid);
				
				boost::uuids::uuid loadingScreenUuid;
				uuidBytes = new char[uuidSize];
				roomFile.read(uuidBytes, uuidSize);		
				memcpy(&loadingScreenUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				LoadingScreenId loadingScreenId = BaseIds::getIntegerFromUuid(loadingScreenUuid);
				
				boost::uuids::uuid transitionUuid;
				uuidBytes = new char[uuidSize];
				roomFile.read(uuidBytes, uuidSize);		
				memcpy(&transitionUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				NameIdPair nameId(roomId, pythonVariableName);

				BaseIds::idNames.push_back(nameId);

				BaseIds::nameIdMap[pythonVariableName] = roomId;
				BaseIds::idNameMap[roomId] = pythonVariableName;

				int mapWidth = 0;
				roomFile.read((char*)&mapWidth, sizeof(int));

				int mapHeight = 0;
				roomFile.read((char*)&mapHeight, sizeof(int));

				roomFile.close();

				boost::shared_ptr<Room> newRoom = boost::shared_ptr<Room>(new Room());
				
				newRoom->moveEntitySignal_->connect(boost::bind(&RoomContainer::moveEntity, this, _1));
				
				newRoom->scriptVar_ = scriptName;
				newRoom->scriptTypeName_ = roomName;

				newRoom->cameraManager_ = cameraManager_;
				newRoom->codeBehindFactory_ = codeBehindFactory_;
				newRoom->roomMetadata_->roomId_ = roomId;
				newRoom->roomMetadata_->roomName_ = pythonVariableName;
				newRoom->roomMetadata_->loadingScreenId_ = loadingScreenId;
				newRoom->roomMetadata_->mapWidth_ = mapWidth;
				newRoom->roomMetadata_->mapHeight_ = mapHeight;
				newRoom->physicsConfig_ = physicsConfig_;

				roomMetadataContainer_->addRoomMetadata(newRoom->roomMetadata_);

				newRoom->renderableManager_ = renderableManager_;
				newRoom->audioSourceContainer_ = audioSourceContainer_;
				newRoom->debugger_ = debugger_;				
				newRoom->roomMetadataContainer_ = roomMetadataContainer_;
				newRoom->ids_ = ids_;
				newRoom->fontManager_ = fontManager_;
				newRoom->engineConfig_ = engineConfig_;
				newRoom->engineController_ = engineController_;
				newRoom->animationManager_ = animationManager_;
				newRoom->hitboxManager_ = hitboxManager_;
				newRoom->anchorPointManager_ = anchorPointManager_;
				newRoom->physicsManager_ = physicsManager_;
				newRoom->textManager_ = textManager_;
				newRoom->messenger_ = messenger_;
				newRoom->queryManager_ = queryManager_;
				newRoom->ui_ = ui_;
				newRoom->audioPlayer_ = audioPlayer_;
				newRoom->renderer_ = renderer_;
				newRoom->timer_ = timer_;
				newRoom->queryResultFactory_ = queryResultFactory_;
				newRoom->queryParametersFactory_ = queryParametersFactory_;
				newRoom->inputDeviceManager_ = inputDeviceManager_;
				newRoom->entityMetadataContainer_ = entityMetadataContainer_;
				newRoom->entityNameValidator_ = entityNameValidator_;
				newRoom->audioSourceNameValidator_ = audioSourceNameValidator_;

				newRoom->setAssets(assets_);

				newRoom->setFileName(filepath.string());

				addRoom(newRoom);

				int roomIndex = rooms_.size() - 1;

				roomIdToIndexMap_[roomId] = roomIndex;

				newRoom->roomMetadata_->myIndex_ = roomIndex;
				
				physicsManager_->addRoom(roomId);
				renderableManager_->addRoom(roomId);
				audioSourceContainer_->addRoom(roomId);

				renderableManager_->preInitialize(roomIndex);

				newRoom->initializePythonData();

				if (textManager_ != nullptr)
				{
					textManager_->addRoom(roomId);
				}
			}
		}
	}
}

int	RoomContainer::getRoomCount()
{
	return rooms_.size();
}

void RoomContainer::moveEntity(MoveEntityParameters params)
{
	// Add the data to a queue, so that it can be moved at the start of the next frame. This is so
	// any operations that have yet to complete won't be interrupted. For example, if this is called in a collision enter
	// event, it will still need to run the main collision event.
	EntityMoveQueueItem item;
	item.entityInstanceId = params.entityInstanceId;
	item.fromRoomId = params.moveFromRoomId;
	item.toRoomId = params.changeRoomParameters.roomId;
	item.spawnPointId = params.changeRoomParameters.spawnPointId;
	item.offsetX = params.changeRoomParameters.offsetX;
	item.offsetY = params.changeRoomParameters.offsetY;
	item.showRoomParameters = params.showRoomParameters;
	item.delayUntilRoomShown = params.changeRoomParameters.delayUntilRoomShown;

	item.key = boost::lexical_cast<std::string>(item.entityInstanceId) + "." +
			   boost::lexical_cast<std::string>(item.fromRoomId) + "." +
			   boost::lexical_cast<std::string>(item.toRoomId);

	if (entityMoveSet_.find(item.key) == entityMoveSet_.end())
	{
		// Update the room metadata immediately.
		boost::shared_ptr<Room> fromRoom = getRoom(params.moveFromRoomId);

		boost::shared_ptr<Entity> entityToMove = fromRoom->getEntityById(params.entityInstanceId);

		boost::shared_ptr<Room> toRoom = getRoom(params.changeRoomParameters.roomId);

		boost::shared_ptr<EntityMetadata> entityMetadata = entityToMove->getComponents()->getEntityMetadata();

		entityMetadata->previousRoomMetadata_ = fromRoom->getMetadata();

		entityMetadata->roomMetadata_ = toRoom->getMetadata();

		entityMoveSet_.insert(item.key);

		entityMoveQueue_.push_back(item);
	}
}

void RoomContainer::moveAllEntities()
{
	moveAllEntities(shownRoomId_);
}

void RoomContainer::moveAllEntities(RoomId showingRoomId)
{
	int size = entityMoveQueue_.size();

	for (int i = size - 1; i >= 0; i--)
	{	
		EntityMoveQueueItem item = entityMoveQueue_[i];

		boost::shared_ptr<Room> fromRoom = getRoom(item.fromRoomId);

		boost::shared_ptr<Room> toRoom = getRoom(item.toRoomId);

		// If the entity is waiting for the room to show, and the room isn't showing yet, ignore it.

		bool removeFromList = true;

		if (toRoom == nullptr)
		{
			std::cout << "Could not move entity into room. Room ID " << item.toRoomId << " not valid." << std::endl;
		}
		else
		{
			bool showRoom = item.showRoomParameters.roomId != ids_->ROOM_NULL;

			if (showRoom == true)
			{
				(*showRoomSignal_)(item.showRoomParameters.roomId, item.showRoomParameters.transitionId, item.showRoomParameters.transitionTime);
			}

			RoomId toRoomId = toRoom->roomMetadata_->roomId_;
			
			// If the room this entity is being moved into is not showing, and it is waiting for the room
			// to be shown before the insert, skip it.
			if (!(toRoomId != showingRoomId && item.delayUntilRoomShown == true))
			{
				boost::shared_ptr<Entity> entityToMove = fromRoom->extractEntity(item.entityInstanceId);

				if (entityToMove != nullptr)
				{
					toRoom->insertEntity(entityToMove, item.spawnPointId, item.offsetX, item.offsetY);
				}
			}
			else
			{
				removeFromList = false;
			}

			// If the room the entity is being moved into is not loaded, load it asynchronously now.
			if (toRoom->getIsLoaded() == false && toRoom->getIsLoading() == false)
			{
				toRoom->setIsLoading(true);

				if (engineController_->getHasQuit() == false)
				{
					ioService_->post(boost::bind(&Room::loadRoomAsync, toRoom));
				}
				else
				{
					std::cout << "Failed to load room because the engine is shutting down." << std::endl;
				}
			}
		}

		if (removeFromList == true)
		{
			entityMoveQueue_.erase(entityMoveQueue_.begin() + i);

			entityMoveSet_.erase(item.key);
		}
	}
}

boost::shared_ptr<Room> RoomContainer::getShownRoomPy()
{
	PythonReleaseGil unlocker;

	return getShownRoom();
}

boost::shared_ptr<Room> RoomContainer::getShownRoom()
{
	return getRoom(shownRoomId_);
}

RoomId RoomContainer::getShownRoomId()
{
	return shownRoomId_;
}

void RoomContainer::setShownRoomId(RoomId roomId)
{
	shownRoomId_ = roomId;

	// Any entities waiting for this room to be shown before being inserted into it
	// should be inserted now.
	moveAllEntities(roomId);

	textManager_->showingRoomId_ = roomId;
}

boost::shared_ptr<Room> RoomContainer::getRoomPy(RoomId roomId)
{
	PythonReleaseGil unlocker;

	return getRoom(roomId);
}

boost::shared_ptr<Room> RoomContainer::getRoom(RoomId roomId)
{
	auto itr = roomIdToIndexMap_.find(roomId);
	
	if (itr == roomIdToIndexMap_.end())
	{
		return nullptr;
	}
	else
	{
		int index = roomIdToIndexMap_[roomId];

		return rooms_[index];
	}
}

boost::shared_ptr<Room> RoomContainer::getRoomByIndex(int roomIndex)
{
	int size = rooms_.size();

	if (roomIndex >= 0 && roomIndex < size)
	{
		return rooms_[roomIndex];
	}

	return nullptr;
}

int RoomContainer::getRoomIndex(RoomId roomId)
{
	std::map<RoomId, int>::iterator itr = roomIdToIndexMap_.find(roomId);
	
	if (itr == roomIdToIndexMap_.end())
	{
		return -1;
	}
	else
	{
		return roomIdToIndexMap_[roomId];
	}
}

void RoomContainer::addRoom(boost::shared_ptr<Room> room)
{
	rooms_.push_back(room);
}

void RoomContainer::setAssets(boost::shared_ptr<Assets> assets)
{
	assets_ = assets;
}