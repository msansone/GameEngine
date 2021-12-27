#include "..\..\Headers\EngineCore\Room.hpp"

using namespace firemelon;
using namespace boost::asio;
using namespace boost::python;

Room::Room() :
	moveEntitySignal_(new MoveEntitySignalRaw())
{
	roomMetadata_ = boost::shared_ptr<RoomMetadata>(new RoomMetadata());

	showRoom_ = false;
	isLoaded_ = false;
	isLoading_ = false; 
	isInitialized_ = false;
	runLoadedEvent_ = false;
	isTransitioning_ = false;
	isShowing_ = false;
	isUpdating_ = false;
	isUnloading_ = false;

	roomMetadata_->roomId_ = -1;
	roomMetadata_->percentLoaded_ = 0;
	roomMetadata_->loadingScreenId_ = -1;
	roomMetadata_->mapWidth_ = 0;
	roomMetadata_->mapHeight_ = 0;
	
	initialRender_ = false;

	activeCamera_ = nullptr;

	transitionTimer_ = 0.0;
	transitionTimeTotal_ = 0.0;

	fileName_ = "";
}

Room::~Room()
{
	RoomId roomId = roomMetadata_->getRoomId();
}

void Room::cleanup()
{
	int size = masterEntityList_.size();

	for (int i = 0; i < size; i++)
	{
		masterEntityList_[i]->cleanup();
	}

	masterEntityList_.clear();

	size = masterEntityListTiles_.size();

	for (int i = 0; i < size; i++)
	{
		masterEntityListTiles_[i]->cleanup();
	}

	masterEntityListTiles_.clear();

	spawnPoints_.clear();

	audioSourceContainer_->unloadRoomAudioSources(roomMetadata_->myIndex_);

	try
	{
		PythonAcquireGil lock;

		pyRoomInstanceNamespace_ = boost::python::object();
		pyRoomInstance_ = boost::python::object();
		pyRoomLoaded_ = boost::python::object();
		pyRoomLoading_ = boost::python::object();
		pyRoomDisplayed_ = boost::python::object();
		pyRoomHidden_ = boost::python::object();
		pyEntityCreated_ = boost::python::object();
		pyEntityEntered_ = boost::python::object();
		pyEntityExited_ = boost::python::object();
		pyUpdate_ = boost::python::object();
		pyMainModule_ = boost::python::object();
		pyMainNamespace_ = boost::python::object();

		std::string sCode = scriptVar_ + " = None";

		str pyCode(sCode);

		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);
	}
	catch (error_already_set &)
	{
		std::cout << "Error deleting room " + scriptVar_ << std::endl;
		debugger_->handlePythonError();
	}
}

void Room::update(double time)
{
	PythonAcquireGil lock;

	try
	{
		pyUpdate_(time);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Room::roomLoaded()
{
	PythonAcquireGil lock;

	try
	{
		pyRoomLoaded_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Room::roomLoading()
{
	PythonAcquireGil lock;

	try
	{
		pyRoomLoading_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Room::roomDisplayed()
{
	PythonAcquireGil lock;

	try
	{
		pyRoomDisplayed_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Room::roomHidden()
{
	PythonAcquireGil lock;

	try
	{
		pyRoomHidden_();
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Room::preRoomShown()
{
	if (debugger_->getDebugMode() == true)
	{
		std::cout << "Room " << getMetadata()->getRoomId() << " shown." << std::endl;
	}

	isShowing_ = true;

	setMovedEntityPositions();

	// Reset the camera focus.
	boost::shared_ptr<CameraController> activeCamera = cameraManager_->getActiveCamera();
	
	if (activeCamera != nullptr)
	{
		activeCamera->focusOnAttachedEntity();
	}

	playAudioSources();

	// At this moment the room actually begins to be rendered and updated.
	// The timer needs to tick as if this a complete frame occurred, otherwise there could be a huge
	// delta time value due to waiting for the room to finish loading/activating/etc.
	timer_->tick();

	initialRender_ = false;

	resetAudioListener();

	roomDisplayed();
}

void Room::preRoomActivated()
{
	//// If the room is already showing when it gets activated set the audio listener and play the audio sources.
	//if (isShowing_ == true)
	//{
	//	resetAudioListener();
	//	playAudioSources();

	//	// At this moment the room actually begins to be rendered and updated.
	//	// The timer needs to tick as if this a complete frame occurred, otherwise there could be a huge
	//	// delta time value due to waiting for the room to finish loading/activating/etc.
	//	timer_->tick();

	//	initialRender_ = false;
	//}

	//isActivated_ = true;
	//
	//roomActivated();
}

void Room::preRoomHidden()
{
	// Stop all audio sources in this room.
	int size = audioSourceContainer_->audioSources_[roomMetadata_->myIndex_].size();

	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<AudioSource> audioSource = audioSourceContainer_->audioSources_[roomMetadata_->myIndex_][i];

		audioSource->stop();
	}
		
	isShowing_ = false;

	roomHidden();
}

void Room::entityEntered(boost::shared_ptr<Entity> entity)
{
	PythonAcquireGil lock;

	try
	{
		boost::shared_ptr<EntityComponents> components = entity->getComponents();
		boost::shared_ptr<CodeBehindContainer> codeBehindContainer = components->getCodeBehindContainer();
		boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper = codeBehindContainer->getPythonInstanceWrapper();

		pyEntityEntered_(pythonInstanceWrapper->getPyInstance());
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Room::entityCreated(boost::shared_ptr<Entity> entity)
{
	PythonAcquireGil lock;

	try
	{
		boost::shared_ptr<EntityComponents> components = entity->getComponents();
		boost::shared_ptr<CodeBehindContainer> codeBehindContainer = components->getCodeBehindContainer();
		boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper = codeBehindContainer->getPythonInstanceWrapper();

		pyEntityCreated_(pythonInstanceWrapper->getPyInstance());
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Room::entityExited(boost::shared_ptr<Entity> entity)
{
	PythonAcquireGil lock;

	try
	{
		boost::shared_ptr<EntityComponents> components = entity->getComponents();
		boost::shared_ptr<CodeBehindContainer> codeBehindContainer = components->getCodeBehindContainer();
		boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper = codeBehindContainer->getPythonInstanceWrapper();

		pyEntityExited_(pythonInstanceWrapper->getPyInstance());
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void Room::loadRoomAsync()
{
	bool displayStatus = false;

	if (engineController_->getHasQuit() == true)
	{
		return;
	}

	roomLoading();

	debugger_->streamLock->lock();
	std::cout << "[" << boost::this_thread::get_id() << "] Loading Room "<<fileName_<<" locally."<<std::endl;
	debugger_->streamLock->unlock();
	
	//PythonAcquireGil lock;
	int tileSize = assets_->getTileSize();

	// Open the map file and load in all the sprites and camera information.
	bool isAnimated = false;
	int tileFrames = 0;

	// Open the file for reading.
	std::ifstream roomfile;

	roomfile.open(fileName_.c_str(), std::ios::in | std::ios::binary);

	if (roomfile.is_open())
	{
		int fileMajorVersion = 0;
		roomfile.read((char*)&fileMajorVersion, sizeof(int));

		int fileMinorVersion = 0;
		roomfile.read((char*)&fileMinorVersion, sizeof(int));
		
		int fileRevisionVersion = 0;
		roomfile.read((char*)&fileRevisionVersion, sizeof(int));

		int uuidSize = boost::uuids::uuid::static_size();
		
		int roomNameSize = 0;

		roomfile.read((char*)&roomNameSize, sizeof(int));

		std::string roomName(roomNameSize, '\0');
		roomfile.read(&roomName[0], roomNameSize);
			
		int scriptNameSize = 0;

		roomfile.read((char*)&scriptNameSize, sizeof(int));

		std::string scriptName(scriptNameSize, '\0');
		roomfile.read(&scriptName[0], scriptNameSize);
			
		int pythonVariableNameSize = 0;

		roomfile.read((char*)&pythonVariableNameSize, sizeof(int));

		std::string pythonVariableName(pythonVariableNameSize, '\0');
		roomfile.read(&pythonVariableName[0], pythonVariableNameSize);
		
		// Room ID. This was already preloaded.
		boost::uuids::uuid roomUuid;
		char* uuidBytes = new char[uuidSize];
		roomfile.read(uuidBytes, uuidSize);		
		memcpy(&roomUuid, uuidBytes, uuidSize);
		delete uuidBytes;
		
		// Loading Screen ID this room is pointing to. This was already preloaded.
		boost::uuids::uuid loadingScreenUuid;
		uuidBytes = new char[uuidSize];
		roomfile.read(uuidBytes, uuidSize);		
		memcpy(&loadingScreenUuid, uuidBytes, uuidSize);
		delete uuidBytes;

		// Transition ID this room is pointing to. This was already preloaded.
		boost::uuids::uuid transitionUuid;
		uuidBytes = new char[uuidSize];
		roomfile.read(uuidBytes, uuidSize);		
		memcpy(&transitionUuid, uuidBytes, uuidSize);
		delete uuidBytes;

		// Height and width needed when preloading. They can be ignored here.
		int mapWidth = 0;
		roomfile.read((char*)&mapWidth, sizeof(int));

		int mapHeight = 0;
		roomfile.read((char*)&mapHeight, sizeof(int));

		// Read the total number of elements to load, to be used for calculating
		// the percent loaded
		int itemsToLoadCount = 0;
		int itemsLoadedCounter = 0;

		int actorsLoadedCounter = 0;
		int eventsLoadedCounter = 0;
		int hudElementsLoadedCounter = 0;
		int audioSourceLoadedCounter = 0;
		int particleEmittersLoadedCounter = 0;
		int particlesLoadedCounter = 0;
		int spawnPointsLoadedCounter = 0;
		int tileObjectsLoadedCounter = 0;
		int worldGeometryLoadedCounter = 0;

		roomfile.read((char*)&itemsToLoadCount, sizeof(int));

		if (displayStatus == true)
		{
			std::cout << "Loading HUD elements" << std::endl;
		}

		// load any HUD element instances.
		int hudElementInstanceCount = 0;
		roomfile.read((char*)&hudElementInstanceCount, sizeof(int));
		
		for (int i = 0; i < hudElementInstanceCount; i++)
		{
			boost::uuids::uuid hudElementUuid;
			uuidBytes = new char[uuidSize];
			roomfile.read(uuidBytes, uuidSize);
			memcpy(&hudElementUuid, uuidBytes, uuidSize);
			delete uuidBytes;

			boost::shared_ptr<Entity> newHudElement = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
			EntityControllerPtr controller = newHudElement->getComponents()->createHudElementController(renderer_, anchorPointManager_, animationManager_);

			controller->anchorPointManager_ = anchorPointManager_;
			controller->hitboxManager_ = hitboxManager_;
			controller->timer_ = timer_;
			controller->audioPlayer_ = audioPlayer_;
			controller->audioSourceContainer_ = audioSourceContainer_;
			controller->textManager_ = textManager_;
			controller->fontManager_ = fontManager_;
			controller->engineConfig_ = engineConfig_;
			controller->debugger_ = debugger_;
			controller->engineController_ = engineController_;
			controller->messenger_ = messenger_;
			controller->queryManager_ = queryManager_;
			controller->roomMetadataContainer_ = roomMetadataContainer_;

			newHudElement->getComponents()->initialize();

			EntityTypeId hudElementId = BaseIds::getIntegerFromUuid(hudElementUuid);

			boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(hudElementId);

			boost::shared_ptr<EntityMetadata> metadata = newHudElement->getComponents()->getEntityMetadata();

			int initialStateIndex = entityTemplate->getInitialStateIndex();

			metadata->setEntityTypeId(hudElementId);
			metadata->roomMetadata_ = roomMetadata_;
			metadata->previousRoomMetadata_ = roomMetadata_;
			metadata->classification_ = entityTemplate->getClassification();

			int hudElementPositionX = 0;
			int hudElementPositionY = 0;
			roomfile.read((char*)&hudElementPositionY, sizeof(int));
			roomfile.read((char*)&hudElementPositionX, sizeof(int));

			bool acceptInput = false;
			roomfile.read((char*)&acceptInput, sizeof(bool));

			int renderOrder = 0;
			roomfile.read((char*)&renderOrder, sizeof(int));

			int entityNameSize = 0;

			roomfile.read((char*)&entityNameSize, sizeof(int));

			std::string entityName(entityNameSize, '\0');
			roomfile.read(&entityName[0], entityNameSize);

			if (acceptInput == true)
			{
				newHudElement->attachInputDeviceManager(inputDeviceManager_);
			}

			boost::shared_ptr<CodeBehindContainer> codeBehindContainer = newHudElement->getComponents()->getCodeBehindContainer();

			codeBehindContainer->getPythonInstanceWrapper()->setScriptName(entityTemplate->getScriptName());
			codeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName(entityTemplate->getScriptTypeName());
			codeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(entityName);

			codeBehindContainer->controller_ = controller;
			codeBehindContainer->hitboxManager_ = hitboxManager_;
			codeBehindContainer->ids_ = ids_;
			codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
			codeBehindContainer->tileSize_ = tileSize;
			codeBehindContainer->timer_ = timer_;
			codeBehindContainer->renderer_ = renderer_;
			codeBehindContainer->ui_ = ui_;

			codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

			codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;
			codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
			codeBehindContainer->stateMachineController_ = newHudElement->getComponents()->getStateMachineController();
			codeBehindContainer->stageController_ = newHudElement->getComponents()->getStageController();

			codeBehindContainer->setIsInputReceiver();
			codeBehindContainer->setIsMessageable();
			codeBehindContainer->setIsRenderable();
			codeBehindContainer->setIsStateMachine();

			codeBehindContainer->createCodeBehinds(hudElementId, ENTITY_CLASSIFICATION_HUDELEMENT);
			codeBehindContainer->initialize();

			boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();

			entityCodeBehind->initializeBegin();

			newHudElement->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
			newHudElement->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
			newHudElement->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));

			newHudElement->validate();

			metadata->name_ = entityName;

			entityNameIdMap_[entityName] = metadata->entityInstanceId_;

			// Load the HUD element instance properties.
			int propertyCount = 0;
			roomfile.read((char*)&propertyCount, sizeof(int));

			for (int j = 0; j < propertyCount; j++)
			{
				int propertyNameSize = 0;

				roomfile.read((char*)&propertyNameSize, sizeof(int));

				std::string propertyName(propertyNameSize, '\0');
				roomfile.read(&propertyName[0], propertyNameSize);

				int propertyValueSize = 0;

				roomfile.read((char*)&propertyValueSize, sizeof(int));

				std::string propertyValue(propertyValueSize, '\0');
				roomfile.read(&propertyValue[0], propertyValueSize);

				codeBehindContainer->getEntityCodeBehind()->entityScript_->entityProperties_[propertyName] = propertyValue;
			}

			controller->setPosition(hudElementPositionX, hudElementPositionY);

			metadata->layerIndex_ = -1;
			newHudElement->transitionId_ = ids_->TRANSITION_NULL;

			//newHudElement->initialize();

			// Look up the entityTemplate associated with this entity ID and set it in the state machine controller.

			int renderableCount = newHudElement->components_->getRenderableCount();


			StageControllerPtr stageController = newHudElement->components_->getStageController();

  			StagePtr stage = stageController->getStage();

			stage->ownerMetadata_ = metadata;

			StateMachineControllerPtr stateMachineController = newHudElement->components_->getStateMachineController();

			StageMetadataPtr stageMetadata = stage->getMetadata();
			
			// Copy states and animations over to the entity.	
			StageMetadataPtr stageMetadataFromTemplate = entityTemplate->getStageMetadata();

			stage->setHeight(stageMetadataFromTemplate->getHeight());
			stage->setWidth(stageMetadataFromTemplate->getWidth());
			stage->setOrigin(stageMetadataFromTemplate->getOrigin());

			stageMetadata->getPosition()->setX(stageMetadataFromTemplate->getPosition()->getX());
			stageMetadata->getPosition()->setY(stageMetadataFromTemplate->getPosition()->getY());

			stageMetadata->getRotationOperation()->getPivotPoint()->setX(stageMetadataFromTemplate->getRotationOperation()->getPivotPoint()->getX());
			stageMetadata->getRotationOperation()->getPivotPoint()->setY(stageMetadataFromTemplate->getRotationOperation()->getPivotPoint()->getY());
			
			metadata->height_ = stageMetadataFromTemplate->getHeight();
			metadata->width_ = stageMetadataFromTemplate->getWidth();
			

			for (int i = 0; i < renderableCount; i++)
			{
				StageRenderablePtr stageRenderable = boost::static_pointer_cast<StageRenderable>(newHudElement->components_->getRenderable(i));

				// Render order will be different for each renderable.
				if (stageRenderable->isBackground_ == true)
				{
					stageRenderable->setRenderOrder(renderOrder - stageRenderable->stage_->metadata_->getBackgroundDepth());
				}
				else
				{
					stageRenderable->setRenderOrder(renderOrder);
				}

				stageRenderable->setMapLayer(metadata->layerIndex_);
			}

			int size = entityTemplate->getStageElementsCount();

			for (int m = 0; m < size; m++)
			{
				boost::shared_ptr<StageElements> stageElements = entityTemplate->getStageElements(m);

				std::string stateName = entityTemplate->getStateNameFromIndex(m);

				std::string typeName = codeBehindContainer->getPythonInstanceWrapper()->getInstanceTypeName();

				StageElementsPtr newStageElements = StageElementsPtr(new StageElements(stateName));

				StateMachineStatePtr newState = StateMachineStatePtr(new StateMachineState(stateName));

				newStageElements->stageMetadata_ = stageMetadata;

				newStageElements->ownerMetadata_ = metadata;

				newStageElements->animationManager_ = animationManager_;

				newStageElements->anchorPointManager_ = anchorPointManager_;

				newStageElements->debugger_ = debugger_;
				
				newStageElements->renderer_ = renderer_;

				newStageElements->hitboxManager_ = hitboxManager_;

				newStageElements->ownerMetadata_ = metadata;

				// Copy state data from the current entityTemplate state to the new state.
				int animationSlotCount = stageElements->getAnimationSlotCount();
								
				bool singleFrame = stageElements->getSingleFrame();

				newStageElements->setSingleFrame(singleFrame);

				for (int k = 0; k < animationSlotCount; k++)
				{
					std::string slotName = stageElements->getAnimationSlotName(k);

					int animationId = stageElements->getAnimationIdByIndex(k);

					int framesPerSecond = stageElements->getAnimationSlotFramesPerSecondByIndex(k);
					
					bool background = stageElements->getAnimationSlotBackgroundByIndex(k);

					int positionX = stageElements->getNativeAnimationSlotPositionXByIndex(k);

					int positionY = stageElements->getNativeAnimationSlotPositionYByIndex(k);
					
					ColorRgbaPtr hueColor = stageElements->getAnimationSlotHueColorByIndex(k);
					
					ColorRgbaPtr blendColor = stageElements->getAnimationSlotBlendColorByIndex(k);
					
					float blendPercent = stageElements->getAnimationSlotBlendPercentByIndex(k);

					ColorRgbaPtr outlineColor = stageElements->getAnimationSlotOutlineColorByIndex(k);

					float rotation = stageElements->getAnimationSlotRotationByIndex(k);

					int pivotX = stageElements->getAnimationSlotPivotPointXByIndex(k);

					int pivotY = stageElements->getAnimationSlotPivotPointYByIndex(k);

					float alphaGradientFrom = stageElements->getAnimationSlotAlphaGradientFromByIndex(k);

					float alphaGradientTo = stageElements->getAnimationSlotAlphaGradientToByIndex(k);

					int alphaGradientRadialCenterX = stageElements->getAnimationSlotAlphaGradientRadialCenterXByIndex(k);

					int alphaGradientRadialCenterY = stageElements->getAnimationSlotAlphaGradientRadialCenterYByIndex(k);

					float alphaGradientRadius = stageElements->getAnimationSlotAlphaGradientRadiusByIndex(k);

					AlphaGradientDirection alphaGradientDirection = stageElements->getAnimationSlotAlphaGradientDirectionByIndex(k);

					AnimationSlotOrigin animationSlotOrigin = stageElements->getAnimationSlotOriginByIndex(k);

					std::string animationSlotNextStateName = stageElements->getAnimationSlotNextStateNameByIndex(k);

					AnimationStyle animationStyle = stageElements->getAnimationSlotAnimationStyleByIndex(k);

					//newStageElements->addAnimationSlotInternal(slotName,
					//				   						   positionX,                  positionY,
					//	                                       hueColor,				   blendColor,                 blendPercent,
					//								           rotation,
					//								           framesPerSecond,
					//								           pivotX,                     pivotY,
					//								           alphaGradientFrom,          alphaGradientTo,
					//								           alphaGradientRadialCenterX, alphaGradientRadialCenterY, alphaGradientRadius,
					//								           alphaGradientDirection,
					//	  							           animationSlotOrigin,
					//	                                       animationSlotNextStateName,
					//	                                       animationStyle);

					newStageElements->addAnimationSlotInternal(slotName,
									   						   positionX,                  positionY,
						                                       hueColor,				   blendColor,                 blendPercent,
													           rotation,
													           framesPerSecond,
													           pivotX,                     pivotY,
						  							           animationSlotOrigin,
						                                       animationSlotNextStateName,
						                                       animationStyle,
															   outlineColor,
															   background);

					newStageElements->assignAnimationByIdToSlotByIndexInternal(k, animationId, true);
				}

				stageController->addExistingStageElements(newStageElements);
				
				stateMachineController->addExistingState(newState);
			}

			addEntity(newHudElement, true);
			
			stateMachineController->setStateByIndex(initialStateIndex);
							
			if (newHudElement->components_->getDynamicsController() != nullptr)
			{
				newHudElement->components_->getDynamicsController()->setOwnerStageHeight(entityTemplate->getStageMetadata()->getHeight());
				newHudElement->components_->getDynamicsController()->setOwnerStageWidth(entityTemplate->getStageMetadata()->getWidth());
			}
			
			newHudElement->initialize();

			// I used to call this here, but it became a problem because it needs for the room to be fully loaded, otherwise
			// the implementation might try to do things that won't work, like accessing or creating other entities.
			//entityCodeBehind->created();

			itemsLoadedCounter++;
			hudElementsLoadedCounter++;

			roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
		}
		
		int tileWidthPerCollisionGridCell = assets_->getTileWidthPerCollisionGridCell();
		int tileWidthPerRenderGridCell = assets_->getTileWidthPerRenderGridCell();

		// Loop through each layer and create the tiles & other entities in each cell.
		int layerCount = 0;
		roomfile.read((char*)&layerCount, sizeof(int));

		int interactiveIndex = 0;
		roomfile.read((char*)&interactiveIndex, sizeof(int));
		
		renderableManager_->setInteractiveLayer(roomMetadata_->myIndex_, interactiveIndex);
		
		for (int i = 0; i < layerCount; i++)
		{
			if (displayStatus == true)
			{
				std::cout << "Loading layer " << i << std::endl;
			}

			int layerRows = 0;
			int layerCols = 0;
			roomfile.read((char*)&layerRows, sizeof(int));
			roomfile.read((char*)&layerCols, sizeof(int));
			
			if (interactiveIndex == i)
			{
				// Setup the grid in the physics controller and the sprite manager.
					
				// Adjust the number of collision grid cells to the minimum needed for the map size.
				double gridRows2 = ceil((double)layerRows / (double)tileWidthPerCollisionGridCell);
				double gridCols2 = ceil((double)layerCols / (double)tileWidthPerCollisionGridCell);

				int collisionGridCellWidth = tileSize * tileWidthPerCollisionGridCell;

				physicsManager_->initCollisionGrid(roomMetadata_->myIndex_, collisionGridCellWidth, (int)gridRows2, (int)gridCols2);
			}
				
			// Adjust the number of render grid cells to the minimum needed for the map size.
			double gridRows = ceil((double)layerRows / (double)tileWidthPerRenderGridCell);
			double gridCols = ceil((double)layerCols / (double)tileWidthPerRenderGridCell);

			renderableManager_->addGridLayer(roomMetadata_->myIndex_, layerRows, layerCols, (int)gridRows, (int)gridCols);
			
			int counter = 0;

			if (displayStatus == true)
			{
				std::cout << "Loading actors" << std::endl;
			}

			// Load any actors that exist in this cell.
			int actorInstanceCount = 0;
			roomfile.read((char*)&actorInstanceCount, sizeof(int));

			for (int l = 0; l < actorInstanceCount; l++)
			{
				boost::uuids::uuid actorUuid;
				uuidBytes = new char[uuidSize];
				roomfile.read(uuidBytes, uuidSize);		
				memcpy(&actorUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				EntityTypeId actorId = BaseIds::getIntegerFromUuid(actorUuid);

				boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(actorId);

				boost::shared_ptr<Entity> newActor = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
				EntityControllerPtr controller = newActor->getComponents()->createActorController(renderer_, anchorPointManager_, animationManager_);

				newActor->getComponents()->initialize();

				int initialStateIndex = entityTemplate->getInitialStateIndex();

				int actorPositionX = 0;
				int actorPositionY = 0;
				roomfile.read((char*)&actorPositionX, sizeof(int));
				roomfile.read((char*)&actorPositionY, sizeof(int));
				
				bool acceptInput = false;
				bool attachToCamera = false;
				int renderOrder = 0;

				roomfile.read((char*)&acceptInput, sizeof(bool));
				roomfile.read((char*)&attachToCamera, sizeof(bool));
				roomfile.read((char*)&renderOrder, sizeof(int));

				boost::shared_ptr<EntityMetadata> metadata = newActor->getComponents()->getEntityMetadata();

				metadata->setEntityTypeId(actorId);
				metadata->roomMetadata_ = roomMetadata_;
				metadata->previousRoomMetadata_ = roomMetadata_;
				metadata->classification_ = entityTemplate->getClassification();

				int entityNameSize = 0;

				roomfile.read((char*)&entityNameSize, sizeof(int));

				std::string entityName(entityNameSize, '\0');
				roomfile.read(&entityName[0], entityNameSize);

				metadata->name_ = entityName;

				controller->anchorPointManager_ = anchorPointManager_;
				controller->hitboxManager_ = hitboxManager_;
				controller->timer_ = timer_;
				controller->audioPlayer_ = audioPlayer_;
				controller->audioSourceContainer_ = audioSourceContainer_;
				controller->textManager_ = textManager_;
				controller->fontManager_ = fontManager_;
				controller->engineConfig_ = engineConfig_;
				controller->debugger_ = debugger_;
				controller->engineController_ = engineController_;
				controller->messenger_ = messenger_;
				controller->queryManager_ = queryManager_;
				controller->roomMetadataContainer_ = roomMetadataContainer_;

				if (acceptInput == true)
				{
					newActor->attachInputDeviceManager(inputDeviceManager_);
				}
								
				boost::shared_ptr<CodeBehindContainer> codeBehindContainer = newActor->getComponents()->getCodeBehindContainer();

				codeBehindContainer->getPythonInstanceWrapper()->setScriptName(entityTemplate->getScriptName());
				codeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName(entityTemplate->getScriptTypeName());
				codeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(entityName);

				codeBehindContainer->controller_ = controller;
				codeBehindContainer->hitboxManager_ = hitboxManager_;
				codeBehindContainer->ids_ = ids_;
				codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
				codeBehindContainer->tileSize_ = tileSize;
				codeBehindContainer->timer_ = timer_;
				codeBehindContainer->renderer_ = renderer_;
				codeBehindContainer->ui_ = ui_;

				codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;
				codeBehindContainer->stateMachineController_ = newActor->getComponents()->getStateMachineController();
				codeBehindContainer->stageController_ = newActor->getComponents()->getStageController();
				
				codeBehindContainer->setIsCollidable();
				codeBehindContainer->setIsInputReceiver();
				codeBehindContainer->setIsMessageable();
				codeBehindContainer->setIsRenderable();
				codeBehindContainer->setIsSimulatable();
				codeBehindContainer->setIsStateMachine();

				codeBehindContainer->createCodeBehinds(actorId, ENTITY_CLASSIFICATION_ACTOR);
				codeBehindContainer->initialize();

				boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();

				entityCodeBehind->initializeBegin();

				newActor->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
				newActor->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
				newActor->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));
				
				newActor->validate();
				
				entityNameIdMap_[entityName] = metadata->entityInstanceId_;
				
				if (attachToCamera == true)
				{
					// Create a camera entity to attach to this entity.
					boost::shared_ptr<Entity> camera = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
					boost::shared_ptr<CameraController> cameraController =  boost::static_pointer_cast<CameraController>(camera->getComponents()->getEntityController());
					camera->getComponents()->initialize();

					boost::shared_ptr<EntityMetadata> cameraMetadata = camera->getComponents()->getEntityMetadata();

					cameraMetadata->name_ = "Camera_" + entityName;

					cameraMetadata->setEntityTypeId(ids_->ENTITY_CAMERA);
					cameraMetadata->classification_ = ENTITY_CLASSIFICATION_CAMERA;
					cameraMetadata->roomMetadata_ = roomMetadata_;
					cameraMetadata->previousRoomMetadata_ = roomMetadata_;

					cameraController->attachDynamicsController();

					int cameraHeight = assets_->getCameraHeight();
					int cameraWidth = assets_->getCameraWidth();

					// Need to call the functions so it sets the midpoint.
					cameraController->setCameraHeight(cameraHeight);
					cameraController->setCameraWidth(cameraWidth);

					camera->initialize();

					addEntity(camera, true);

					cameraController->attachToEntity(controller);
				}

				// Load the sprite instance properties.
				int propertyCount = 0;
				roomfile.read((char*)&propertyCount, sizeof(int));

				for (int m = 0; m < propertyCount; m++)
				{
					int propertyNameSize = 0;

					roomfile.read((char*)&propertyNameSize, sizeof(int));

					std::string propertyName(propertyNameSize, '\0');
					roomfile.read(&propertyName[0], propertyNameSize);
							
					int propertyValueSize = 0;

					roomfile.read((char*)&propertyValueSize, sizeof(int));

					std::string propertyValue(propertyValueSize, '\0');
					roomfile.read(&propertyValue[0], propertyValueSize);

					codeBehindContainer->getEntityCodeBehind()->entityScript_->entityProperties_[propertyName] = propertyValue;
				}

				controller->setPosition(actorPositionX, actorPositionY);
					
				metadata->layerIndex_ = i;
				newActor->transitionId_ = ids_->TRANSITION_NULL;

				controller->attachDynamicsController();
				
				//newActor->initialize();
				
				// Position needs to be set twice, because it needs to know the position before initialization,
				// and also, after initialization it is used to set the dynamics controller position if one is set.
				//newActor->setPosition(actorPositionX, actorPositionY);
					
				// Look up the sprite entityTemplate associated with this sprite ID and set it in the state machine controller.
				StateMachineControllerPtr stateMachineController = newActor->components_->getStateMachineController();

				StageControllerPtr stageController = newActor->components_->getStageController();

				StagePtr stage = stageController->getStage();

				StageMetadataPtr stageMetadata = stage->getMetadata();

				stage->ownerMetadata_ = metadata;

				boost::shared_ptr<HitboxController> hc = newActor->components_->getHitboxController();


				// Instead of setting entityTemplate, copy states and animations over to the entity.
				StageMetadataPtr stageMetadataFromTemplate = entityTemplate->getStageMetadata();

				stage->setHeight(stageMetadataFromTemplate->getHeight());
				stage->setWidth(stageMetadataFromTemplate->getWidth());
				stage->setOrigin(stageMetadataFromTemplate->getOrigin());

				stageMetadata->getPosition()->setX(stageMetadataFromTemplate->getPosition()->getX());
				stageMetadata->getPosition()->setY(stageMetadataFromTemplate->getPosition()->getY());

				stage->getPivotPoint()->setX(stageMetadataFromTemplate->getRotationOperation()->getPivotPoint()->getX());
				stage->getPivotPoint()->setY(stageMetadataFromTemplate->getRotationOperation()->getPivotPoint()->getY());

				metadata->height_ = entityTemplate->getStageMetadata()->getHeight();
				metadata->width_ = entityTemplate->getStageMetadata()->getWidth();

				hc->setStageHeight(entityTemplate->getStageMetadata()->getHeight());
				hc->setStageWidth(entityTemplate->getStageMetadata()->getWidth());

				int renderableCount = newActor->components_->getRenderableCount();
				
				for (int i = 0; i < renderableCount; i++)
				{
					StageRenderablePtr stageRenderable = boost::static_pointer_cast<StageRenderable>(newActor->components_->getRenderable(i));

					if (stageRenderable->isBackground_ == true)
					{
						stageRenderable->setRenderOrder(renderOrder - stageRenderable->stage_->metadata_->getBackgroundDepth());
					}
					else
					{
						stageRenderable->setRenderOrder(renderOrder);
					}

					stageRenderable->setMapLayer(metadata->layerIndex_);
				}

				int size = entityTemplate->getStageElementsCount();

				for (int m = 0; m < size; m++)
				{
					StageElementsPtr stageElementsFromTemplate = entityTemplate->getStageElements(m);

					std::string stateName = entityTemplate->getStateNameFromIndex(m);

					std::string typeName = codeBehindContainer->getPythonInstanceWrapper()->getInstanceTypeName();

					StageElementsPtr newStageElements = StageElementsPtr(new StageElements(stateName));

					StateMachineStatePtr newState = StateMachineStatePtr(new StateMachineState(stateName));

					newStageElements->stageMetadata_ = stage->getMetadata();
					
					newStageElements->ownerMetadata_ = metadata;

					newStageElements->animationManager_ = animationManager_;

					newStageElements->anchorPointManager_ = anchorPointManager_;

					newStageElements->debugger_ = debugger_;

					newStageElements->renderer_ = renderer_;

					newStageElements->hitboxManager_ = hitboxManager_;

					newStageElements->ownerMetadata_ = metadata;

					// Get the hitboxes in this state, and make copies of them. This is so they can be resized, or
					// otherwise changed, on a per instance basis.
					int hitboxReferenceCount = stageElementsFromTemplate->getHitboxReferenceCount();

					for (int k = 0; k < hitboxReferenceCount; k++)
					{
						int hitboxId = stageElementsFromTemplate->getHitboxReference(k);

						// Make a copy of the hitbox.
						boost::shared_ptr<Hitbox> sourceHitbox = hitboxManager_->getHitbox(hitboxId);

						boost::shared_ptr<Hitbox> newHitbox = boost::shared_ptr<Hitbox>(new Hitbox(sourceHitbox->getLeft(), sourceHitbox->getTop(), sourceHitbox->getHeight(), sourceHitbox->getWidth()));

						newHitbox->setIdentity(sourceHitbox->getIdentity());

						newHitbox->setIsSolid(sourceHitbox->getIsSolid());

						newHitbox->setBaseRotationDegrees(sourceHitbox->getBaseRotationDegrees());

						int newHitboxId = hitboxManager_->addHitbox(newHitbox);

						newStageElements->addHitboxReference(newHitboxId);

						newHitbox->stageMetadata_ = stage->metadata_;
					}

					int animationSlotCount = stageElementsFromTemplate->getAnimationSlotCount();
								
					bool singleFrame = stageElementsFromTemplate->getSingleFrame();

					newStageElements->setSingleFrame(singleFrame);

					for (int k = 0; k < animationSlotCount; k++)
					{
						std::string slotName = stageElementsFromTemplate->getAnimationSlotName(k);

						int animationId = stageElementsFromTemplate->getAnimationIdByIndex(k);

						int framesPerSecond = stageElementsFromTemplate->getAnimationSlotFramesPerSecondByIndex(k);

						bool background = stageElementsFromTemplate->getAnimationSlotBackgroundByIndex(k);

						int positionX = stageElementsFromTemplate->getNativeAnimationSlotPositionXByIndex(k);
						int positionY = stageElementsFromTemplate->getNativeAnimationSlotPositionYByIndex(k);

						ColorRgbaPtr hueColor = stageElementsFromTemplate->getAnimationSlotHueColorByIndex(k);

						float hueRed = hueColor->getR();
						float hueGreen = hueColor->getG();
						float hueBlue = hueColor->getB();
						float hueAlpha = hueColor->getA();

						ColorRgbaPtr blendColor = stageElementsFromTemplate->getAnimationSlotBlendColorByIndex(k);

						float blendRed = blendColor->getR();
						float blendGreen = blendColor->getG();
						float blendBlue = blendColor->getB();
						float blendAlpha = blendColor->getA();

						float blendPercent = stageElementsFromTemplate->getAnimationSlotBlendPercentByIndex(k);

						ColorRgbaPtr outlineColor = stageElementsFromTemplate->getAnimationSlotOutlineColorByIndex(k);

						float outlineRed = outlineColor->getR();
						float outlineGreen = outlineColor->getG();
						float outlineBlue = outlineColor->getB();
						float outlineAlpha = outlineColor->getA();

						float rotation = stageElementsFromTemplate->getAnimationSlotRotationByIndex(k);

						int pivotX = stageElementsFromTemplate->getAnimationSlotPivotPointXByIndex(k);
						int pivotY = stageElementsFromTemplate->getAnimationSlotPivotPointYByIndex(k);

						float alphaGradientFrom = stageElementsFromTemplate->getAnimationSlotAlphaGradientFromByIndex(k);
						float alphaGradientTo = stageElementsFromTemplate->getAnimationSlotAlphaGradientToByIndex(k);
						
						int alphaGradientRadialCenterX = stageElementsFromTemplate->getAnimationSlotAlphaGradientRadialCenterXByIndex(k);
						int alphaGradientRadialCenterY = stageElementsFromTemplate->getAnimationSlotAlphaGradientRadialCenterYByIndex(k);
						float alphaGradientRadius = stageElementsFromTemplate->getAnimationSlotAlphaGradientRadiusByIndex(k);

						AlphaGradientDirection alphaGradientDirection = stageElementsFromTemplate->getAnimationSlotAlphaGradientDirectionByIndex(k);

						AnimationSlotOrigin animationSlotOrigin = stageElementsFromTemplate->getAnimationSlotOriginByIndex(k);

						std::string animationSlotNextStateName = stageElementsFromTemplate->getAnimationSlotNextStateNameByIndex(k);
						
						AnimationStyle animationSlotAnimationStyle = stageElementsFromTemplate->getAnimationSlotAnimationStyleByIndex(k);

						//newStageElements->addAnimationSlotInternal(slotName,
						//										   positionX, positionY,
						//										   hueColor, blendColor, blendPercent,
						//										   rotation,
						//	                                       framesPerSecond,
						//										   pivotX, pivotY,
						//										   alphaGradientFrom, alphaGradientTo,
						//										   alphaGradientRadialCenterX, alphaGradientRadialCenterY, alphaGradientRadius, 
						//										   alphaGradientDirection,
						//										   animationSlotOrigin,
						//	                                       animationSlotNextStateName,
						//										   animationSlotAnimationStyle);

						newStageElements->addAnimationSlotInternal(slotName,
																   positionX, positionY,
																   hueColor, blendColor, blendPercent,
																   rotation,
							                                       framesPerSecond,
																   pivotX, pivotY,
																   animationSlotOrigin,
							                                       animationSlotNextStateName,
																   animationSlotAnimationStyle,
																   outlineColor,
																   background);

						newStageElements->assignAnimationByIdToSlotByIndexInternal(k, animationId, true);
					}

					stageController->addExistingStageElements(newStageElements);

					stateMachineController->addExistingState(newState);
				}

				addEntity(newActor, true);
				
				newActor->initialize();

				if (stateMachineController != nullptr)
				{
					stateMachineController->setStateByIndex(initialStateIndex);
				}

				if (newActor->components_->getDynamicsController() != nullptr)
				{
					EntityTypeId entityTypeId = newActor->getComponents()->getEntityMetadata()->getEntityTypeId();

					boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(entityTypeId);
			
					DynamicsController* dc = newActor->components_->getDynamicsController();
			
					dc->setOwnerStageHeight(entityTemplate->getStageMetadata()->getHeight());

					dc->setOwnerStageWidth(entityTemplate->getStageMetadata()->getWidth());

					// Need to know the layer position for visual debug rendering.
					boost::shared_ptr<Position> p = renderableManager_->getLayerPosition(roomMetadata_->myIndex_, i);

					dc->layerPosition_ = p;
				}

				codeBehindContainer->getSimulatableCodeBehind()->start();
				
				// I used to call this here, but it became a problem because it needs for the room to be fully loaded, otherwise
				// the implementation might try to do things that won't work, like accessing or creating other entities.
				//entityCodeBehind->created();

				itemsLoadedCounter++;

				actorsLoadedCounter++;

				roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
			}

			if (displayStatus == true)
			{
				std::cout << "Loading events" << std::endl;
			}

			// Load any events that exist in this cell.
			int eventInstanceCount = 0;
			roomfile.read((char*)&eventInstanceCount, sizeof(int));

			for (int l = 0; l < eventInstanceCount; l++)
			{
				boost::uuids::uuid eventUuid;
				uuidBytes = new char[uuidSize];
				roomfile.read(uuidBytes, uuidSize);		
				memcpy(&eventUuid, uuidBytes, uuidSize);
				delete uuidBytes;
					
				EntityTypeId eventId = BaseIds::getIntegerFromUuid(eventUuid);

				boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(eventId);
					
				int eventPositionX = 0;
				int eventPositionY = 0;
				int eventWidth = 0;
				int eventHeight = 0;
				bool acceptInput = false;
				
				roomfile.read((char*)&eventPositionX, sizeof(int));
				roomfile.read((char*)&eventPositionY, sizeof(int));
				roomfile.read((char*)&eventWidth, sizeof(int));
				roomfile.read((char*)&eventHeight, sizeof(int));							
				roomfile.read((char*)&acceptInput, sizeof(bool));
						
				int entityNameSize = 0;

				roomfile.read((char*)&entityNameSize, sizeof(int));

				std::string entityName(entityNameSize, '\0');
				roomfile.read(&entityName[0], entityNameSize); 
				
				boost::shared_ptr<Entity> newEvent = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
				EntityControllerPtr controller = newEvent->getComponents()->createEventController();
				newEvent->getComponents()->initialize();

				boost::shared_ptr<EntityMetadata> metadata = newEvent->getComponents()->getEntityMetadata();
				
				metadata->setEntityTypeId(eventId);
				metadata->roomMetadata_ = roomMetadata_;
				metadata->previousRoomMetadata_ = roomMetadata_;
				metadata->classification_ = entityTemplate->getClassification();

				metadata->height_ = eventHeight;
				metadata->width_ = eventWidth;

				boost::shared_ptr<CodeBehindContainer> codeBehindContainer = newEvent->getComponents()->getCodeBehindContainer();

				codeBehindContainer->getPythonInstanceWrapper()->setScriptName(entityTemplate->getScriptName());
				codeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName(entityTemplate->getScriptTypeName());
				codeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(entityName);

				codeBehindContainer->controller_ = controller;
				codeBehindContainer->hitboxManager_ = hitboxManager_;
				codeBehindContainer->ids_ = ids_;
				codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
				codeBehindContainer->tileSize_ = tileSize;
				codeBehindContainer->timer_ = timer_;
				codeBehindContainer->renderer_ = renderer_;
				codeBehindContainer->ui_ = ui_;

				codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

				codeBehindContainer->setIsCollidable();
				codeBehindContainer->setIsInputReceiver();
				codeBehindContainer->setIsMessageable();

				codeBehindContainer->createCodeBehinds(eventId, ENTITY_CLASSIFICATION_EVENT);
				codeBehindContainer->initialize();
			
				boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();
				
				entityCodeBehind->initializeBegin();

				newEvent->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
				newEvent->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
				newEvent->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));
							
				if (acceptInput == true)
				{
					newEvent->attachInputDeviceManager(inputDeviceManager_);
				}

				newEvent->validate();

				metadata->name_ = entityName;
				
				entityNameIdMap_[entityName] = metadata->entityInstanceId_;
				
				entityCodeBehind->timer_ = timer_;
				entityCodeBehind->animationManager_ = animationManager_;
				entityCodeBehind->anchorPointManager_ = anchorPointManager_;
				entityCodeBehind->hitboxManager_ = hitboxManager_;
				entityCodeBehind->audioPlayer_ = audioPlayer_;
				entityCodeBehind->textManager_ = textManager_;
				entityCodeBehind->fontManager_ = fontManager_;
				entityCodeBehind->engineConfig_ = engineConfig_;
				entityCodeBehind->engineController_ = engineController_;
				entityCodeBehind->messenger_ = messenger_;
				entityCodeBehind->queryManager_ = queryManager_;
				
				// Load the event instance properties.
				int propertyCount = 0;
				roomfile.read((char*)&propertyCount, sizeof(int));

				for (int m = 0; m < propertyCount; m++)
				{
					int propertyNameSize = 0;

					roomfile.read((char*)&propertyNameSize, sizeof(int));

					std::string propertyName(propertyNameSize, '\0');
					roomfile.read(&propertyName[0], propertyNameSize);
							
					int propertyValueSize = 0;

					roomfile.read((char*)&propertyValueSize, sizeof(int));

					std::string propertyValue(propertyValueSize, '\0');
					roomfile.read(&propertyValue[0], propertyValueSize);

					codeBehindContainer->getEntityCodeBehind()->entityScript_->entityProperties_[propertyName] = propertyValue;
				}

				metadata->layerIndex_ = i;
				newEvent->transitionId_ = ids_->TRANSITION_NULL;

				//newEvent->attachHitboxController(); Moved inside components. Get initialized when controller is created.

				//newEvent->initialize();

				controller->setPosition(eventPositionX, eventPositionY);		

				addEntity(newEvent, true);
				
				newEvent->initialize();

				if (i == interactiveIndex)
				{
					boost::shared_ptr<HitboxController> hc = newEvent->components_->getHitboxController();

					boost::shared_ptr<Hitbox> h = boost::shared_ptr<Hitbox>(new Hitbox(0, 0, eventHeight, eventWidth));
					h->setIdentity(ids_->HITBOX_EVENT);

					h->setEdgeFlags(0xFF);

					int hitboxId = hitboxManager_->addHitbox(h);

					hc->activateHitbox(hitboxId);
				}

				// I used to call this here, but it became a problem because it needs for the room to be fully loaded, otherwise
				// the implementation might try to do things that won't work, like accessing or creating other entities.
				//entityCodeBehind->created();

				itemsLoadedCounter++;

				eventsLoadedCounter++;

				roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
			}

			if (displayStatus == true)
			{
				std::cout << "Loading spawn points" << std::endl;
			}

			// Load any spawn points that exist in this cell.
			int spawnPointCount = 0;
			roomfile.read((char*)&spawnPointCount, sizeof(int));
				
			for (int l = 0; l < spawnPointCount; l++)
			{				
				boost::shared_ptr<SpawnPoint> newSpawnPoint = boost::shared_ptr<SpawnPoint>(new SpawnPoint());

				boost::uuids::uuid spawnPointUuid;
				uuidBytes = new char[uuidSize];
				roomfile.read(uuidBytes, uuidSize);		
				memcpy(&spawnPointUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				SpawnPointId spawnPointId = ids_->SPAWNPOINT_NULL;

				if (spawnPointUuid != boost::uuids::nil_uuid())
				{
					spawnPointId = BaseIds::getIntegerFromUuid(spawnPointUuid);
				}
				
				int spawnPointX = 0;
				int spawnPointY = 0;

				roomfile.read((char*)&spawnPointX, sizeof(int));
				roomfile.read((char*)&spawnPointY, sizeof(int));
				
				newSpawnPoint->setId(spawnPointId);
				newSpawnPoint->setX(spawnPointX);
				newSpawnPoint->setY(spawnPointY);
				newSpawnPoint->setLayer(i);

				spawnPoints_.push_back(newSpawnPoint);

				itemsLoadedCounter++;

				spawnPointsLoadedCounter++;

				roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
			}

			if (displayStatus == true)
			{
				std::cout << "Loading particle emitters" << std::endl;
			}

			// Load any particle emitters that exist in this cell.
			int particleEmitterCount = 0;
			roomfile.read((char*)&particleEmitterCount, sizeof(int));
				
			for (int l = 0; l < particleEmitterCount; l++)
			{
				boost::shared_ptr<Entity> newParticleEmitter = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
				
				boost::uuids::uuid particleTypeUuid;
				uuidBytes = new char[uuidSize];
				roomfile.read(uuidBytes, uuidSize);		
				memcpy(&particleTypeUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				boost::uuids::uuid particleEmitterUuid;
				uuidBytes = new char[uuidSize];
				roomfile.read(uuidBytes, uuidSize);		
				memcpy(&particleEmitterUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				boost::uuids::uuid animationUuid;
				uuidBytes = new char[uuidSize];
				roomfile.read(uuidBytes, uuidSize);		
				memcpy(&animationUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				int particleEmitterNameSize = 0;

				roomfile.read((char*)&particleEmitterNameSize, sizeof(int));

				std::string particleEmitterName(particleEmitterNameSize, '\0');
				roomfile.read(&particleEmitterName[0], particleEmitterNameSize);
				
				int particlesPerEmission = 0;
				roomfile.read((char*)&particlesPerEmission, sizeof(int));

				int maxParticles = 0;
				roomfile.read((char*)&maxParticles, sizeof(int));
				
				double interval = 0;
				roomfile.read((char*)&interval, sizeof(double));

				double particleLifespan = 0;
				roomfile.read((char*)&particleLifespan, sizeof(double));

				bool automatic = false;
				roomfile.read((char*)&automatic, sizeof(bool));

				bool attachParticles = false;
				roomfile.read((char*)&attachParticles, sizeof(bool));

				int particleEmitterX = 0;
				roomfile.read((char*)&particleEmitterX, sizeof(int));

				int particleEmitterY = 0;
				roomfile.read((char*)&particleEmitterY, sizeof(int));
				
				int animationFramesPerSecond = 0;
				roomfile.read((char*)&animationFramesPerSecond, sizeof(int));
				
									
				//ParticleEmitterId particleEmitterId = ids_->PARTICLEEMITTER_NULL;				
				EntityTypeId particleEmitterId = ids_->PARTICLEEMITTER_NULL;
				ParticleId particleTypeId = BaseIds::getIntegerFromUuid(particleTypeUuid);

				if (particleEmitterUuid != boost::uuids::nil_uuid())
				{
					particleEmitterId = BaseIds::getIntegerFromUuid(particleEmitterUuid);
					
					AssetId animationId;
					
					if (animationUuid != boost::uuids::nil_uuid())
					{
						animationId = BaseIds::getIntegerFromUuid(animationUuid);
					}
					else
					{
						animationId = -1;
					}
									
					EntityControllerPtr controller = newParticleEmitter->getComponents()->createParticleEmitterController();
					newParticleEmitter->getComponents()->initialize();

					boost::shared_ptr<EntityMetadata> metadata = newParticleEmitter->getComponents()->getEntityMetadata();
					boost::shared_ptr<ParticleEmitterController> particleEmitterController = boost::static_pointer_cast<ParticleEmitterController>(controller);
					boost::shared_ptr<CodeBehindContainer> codeBehindContainer = newParticleEmitter->getComponents()->getCodeBehindContainer();

					controller->attachDynamicsController();

					particleEmitterController->renderer_ = renderer_;

					metadata->setEntityTypeId(particleEmitterId);
					metadata->roomMetadata_ = roomMetadata_;
					metadata->previousRoomMetadata_ = roomMetadata_;
					metadata->classification_ = ENTITY_CLASSIFICATION_PARTICLEEMITTER;
					metadata->layerIndex_ = i;

					codeBehindContainer->getPythonInstanceWrapper()->setScriptName(BaseIds::idScriptDataMap[particleEmitterId].getFileName());
					codeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName(BaseIds::idScriptDataMap[particleEmitterId].getScriptTypeName());
					codeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(particleEmitterName);

					codeBehindContainer->controller_ = controller;
					codeBehindContainer->hitboxManager_ = hitboxManager_;
					codeBehindContainer->ids_ = ids_;
					codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
					codeBehindContainer->tileSize_ = tileSize;
					codeBehindContainer->timer_ = timer_;
					codeBehindContainer->renderer_ = renderer_;
					codeBehindContainer->ui_ = ui_;

					codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

					codeBehindContainer->setIsSimulatable();

					codeBehindContainer->createCodeBehinds(particleEmitterId, ENTITY_CLASSIFICATION_PARTICLEEMITTER);

					boost::shared_ptr<ParticleEmitterEntityCodeBehind> particleEmitterCodeBehind = boost::static_pointer_cast<ParticleEmitterEntityCodeBehind>(codeBehindContainer->getEntityCodeBehind());

					particleEmitterCodeBehind->particleTypeId_ = particleTypeId;

					particleEmitterCodeBehind->interval_ = interval;
					particleEmitterCodeBehind->updateTimer_ = interval;
					particleEmitterCodeBehind->particlesPerEmission_ = particlesPerEmission;
					particleEmitterCodeBehind->particleLifespan_ = particleLifespan;
					particleEmitterCodeBehind->maxParticles_ = maxParticles;

					if (animationId >= 0)
					{
						particleEmitterCodeBehind->particleAnimationId_ = animationManager_->translateAnimationId(animationId);
					}

					particleEmitterCodeBehind->automatic_ = automatic;
					particleEmitterCodeBehind->attachParticles_ = attachParticles;

					metadata->name_ = particleEmitterName;
						
					controller->setPosition(particleEmitterX, particleEmitterY);

					newParticleEmitter->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
					newParticleEmitter->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
					newParticleEmitter->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));

					//newParticleEmitter->position_->setPreviousX(particleEmitterX);
					//newParticleEmitter->position_->setX(particleEmitterX);

					//newParticleEmitter->position_->setPreviousY(particleEmitterY);
					//newParticleEmitter->position_->setY(particleEmitterY);
					
					if (particleTypeUuid != boost::uuids::nil_uuid())
					{
						if (displayStatus == true)
						{
							std::cout << "Loading particles" << std::endl;
						}

						particleTypeId = BaseIds::getIntegerFromUuid(particleTypeUuid);

						// Create the particle entities this emitter will contain.
						for (int m = 0; m < maxParticles; m++)
						{
							boost::shared_ptr<Entity> newParticle = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));

							// Generate a UUID to use for the particle python variable name.
							boost::uuids::uuid uuid = boost::uuids::random_generator()();

							// Names in python can't begin with a number. prefix it with an underscore.
							std::string particleName = "_" + boost::lexical_cast<std::string>(uuid);

							// The controller must be created first, as it is used to create some of the other components.
							EntityControllerPtr controller2 = newParticle->getComponents()->createParticleController(renderer_);
							newParticle->getComponents()->initialize();

							controller2->animationManager_ = animationManager_;

							boost::shared_ptr<ParticleMetadata> particleMetadata = boost::static_pointer_cast<ParticleMetadata>(newParticle->getComponents()->getEntityMetadata());
							boost::shared_ptr<ParticleController> particleController = boost::static_pointer_cast<ParticleController>(controller2);
							boost::shared_ptr<CodeBehindContainer> particleCodeBehindContainer = newParticle->getComponents()->getCodeBehindContainer();

							particleController->renderer_ = renderer_;

							particleController->attachDynamicsController();

							//newParticle->attachRenderable();

							boost::shared_ptr<ParticleRenderable> particleRenderable = boost::static_pointer_cast<ParticleRenderable>(particleController->getRenderable());
							
							particleMetadata->setEntityTypeId(particleTypeId);
							particleMetadata->roomMetadata_ = roomMetadata_;
							particleMetadata->previousRoomMetadata_ = roomMetadata_;
							particleMetadata->classification_ = ENTITY_CLASSIFICATION_PARTICLE;
							particleMetadata->layerIndex_ = i;
									
							particleRenderable->setMapLayer(i);
							particleRenderable->setMetadata(particleMetadata);
							particleRenderable->setRenderOrder(((i - interactiveIndex) * 10) + 1);

							particleCodeBehindContainer->getPythonInstanceWrapper()->setScriptName(BaseIds::idScriptDataMap[particleTypeId].getFileName());
							particleCodeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName(BaseIds::idScriptDataMap[particleTypeId].getScriptTypeName());
							particleCodeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(particleName);

							particleCodeBehindContainer->controller_ = particleController;
							particleCodeBehindContainer->hitboxManager_ = hitboxManager_;
							particleCodeBehindContainer->ids_ = ids_;
							particleCodeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
							particleCodeBehindContainer->tileSize_ = tileSize;
							particleCodeBehindContainer->timer_ = timer_;
							particleCodeBehindContainer->renderer_ = renderer_;
							particleCodeBehindContainer->ui_ = ui_;

							particleCodeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

							particleCodeBehindContainer->setIsSimulatable();

							particleCodeBehindContainer->createCodeBehinds(particleTypeId, ENTITY_CLASSIFICATION_PARTICLE);
							particleCodeBehindContainer->initialize();

							boost::shared_ptr<ParticleEntityCodeBehind> particleCodeBehind = boost::static_pointer_cast<ParticleEntityCodeBehind>(particleCodeBehindContainer->getEntityCodeBehind());

							particleMetadata->name_ = particleName;

							//particleCodeBehind->particleTypeId_ = particleTypeId;

							particleController->setPosition(particleEmitterX, particleEmitterY);

							newParticle->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
							newParticle->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
							newParticle->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));

							//particleCodeBehind->isVisible_ = false;

							particleMetadata->lifetime_ = particleLifespan;

							if (particleEmitterCodeBehind->particleAnimationId_ >= 0)
							{
								particleRenderable->setAnimation(animationManager_->getAnimationByIndex(particleEmitterCodeBehind->particleAnimationId_), animationFramesPerSecond);
							}

							particleEmitterCodeBehind->particles_.push_back(particleCodeBehind);

							addEntity(newParticle, true);

							newParticle->initialize();

							particleCodeBehindContainer->getSimulatableCodeBehind()->start();

							particleCodeBehind->created();
									
							particleCodeBehind->preRoomEntered(roomMetadata_->roomId_);

							entityEntered(newParticle);

							itemsLoadedCounter++;

							particlesLoadedCounter++;

							roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
						}
					}

					// The emitter code behind must be initialized after the particles it contains.
					codeBehindContainer->initialize();
					
					entityNameIdMap_[particleEmitterName] = metadata->entityInstanceId_;

					particleEmitterCodeBehind->initializeParticles();

					addEntity(newParticleEmitter, false);

					codeBehindContainer->getSimulatableCodeBehind()->start();

					newParticleEmitter->initialize();

					particleEmitterCodeBehind->created();

					particleEmitterCodeBehind->preRoomEntered(roomMetadata_->roomId_);

					entityEntered(newParticleEmitter);
				}
			
				itemsLoadedCounter++;

				particleEmittersLoadedCounter++;

				roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
			}

			if (displayStatus == true)
			{
				std::cout << "Loading audio sources" << std::endl;
			}

			// Load any audio sources that exist in this cell.
			int audioSourceCount = 0;
			roomfile.read((char*)&audioSourceCount, sizeof(int));
				
			for (int l = 0; l < audioSourceCount; l++)
			{		
				boost::uuids::uuid audioAssetUuid;
				uuidBytes = new char[uuidSize];
				roomfile.read(uuidBytes, uuidSize);		
				memcpy(&audioAssetUuid, uuidBytes, uuidSize);
				delete uuidBytes;
				
				int instanceNameSize = 0;

				roomfile.read((char*)&instanceNameSize, sizeof(int));

				std::string instanceName(instanceNameSize, '\0');
				roomfile.read(&instanceName[0], instanceNameSize); 
				
				bool autoplay = false;
				roomfile.read((char*)&autoplay, sizeof(bool));

				bool loop = false;
				roomfile.read((char*)&loop, sizeof(bool));
				
				int minDistance = 0;
				roomfile.read((char*)&minDistance, sizeof(int));

				int maxDistance = 0;
				roomfile.read((char*)&maxDistance, sizeof(int));
				
				float volume = 0;
				roomfile.read((char*)&volume, sizeof(volume));
				
				int audioSourceX = 0;
				roomfile.read((char*)&audioSourceX, sizeof(int));

				int audioSourceY = 0;
				roomfile.read((char*)&audioSourceY, sizeof(int));
				
				AssetId audioId = BaseIds::getIntegerFromUuid(audioAssetUuid);

				int audioIndex = audioPlayer_->translateAudioId(audioId);
						
				boost::shared_ptr<AudioSource> newAudioSource = boost::shared_ptr<AudioSource>(new AudioSource(instanceName, audioSourceX, audioSourceY));

				newAudioSource->audioPlayer_ = audioPlayer_;
				newAudioSource->audioIndex_ = audioIndex;
				newAudioSource->roomIndex_ = roomMetadata_->myIndex_;
				newAudioSource->roomId_ = roomMetadata_->roomId_;
				newAudioSource->layerIndex_ = i;
				newAudioSource->loop_ = loop;
				newAudioSource->autoplay_ = autoplay;
				newAudioSource->minDistance_ = minDistance;
				newAudioSource->maxDistance_ = maxDistance;
				newAudioSource->volume_ = volume;
				
				audioSourceContainer_->addAudioSource(roomMetadata_->myIndex_, newAudioSource);

				itemsLoadedCounter++;

				audioSourceLoadedCounter++;

				roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
			}

			// Load tile objects.
			int tileObjectCount = 0;
			roomfile.read((char*)&tileObjectCount, sizeof(int));

			for (int l = 0; l < tileObjectCount; l++)
			{
				boost::uuids::uuid tileSheetUuid;
				uuidBytes = new char[uuidSize];
				roomfile.read(uuidBytes, uuidSize);
				memcpy(&tileSheetUuid, uuidBytes, uuidSize);
				delete uuidBytes;

				AssetId tileSheetId = BaseIds::getIntegerFromUuid(tileSheetUuid);

				int translatedTileSheetId = renderer_->translateSpriteSheetId(tileSheetId);

				boost::shared_ptr<SpriteSheet> tileSheet = renderer_->getSheet(translatedTileSheetId);

				// Animated scenery has multiple regions.
				int framesPerSecond = 0;

				roomfile.read((char*)&framesPerSecond, sizeof(int));

				int regionCount = 0;

 				roomfile.read((char*)&regionCount, sizeof(int));

				std::vector<Rect> sheetRegions;

				for (int m = 0; m < regionCount; m++)
				{
					int regionLeft = 0;
					roomfile.read((char*)&regionLeft, sizeof(int));

					int regionTop = 0;
					roomfile.read((char*)&regionTop, sizeof(int));

					int regionWidth = 0;
					roomfile.read((char*)&regionWidth, sizeof(int));

					int regionHeight = 0;
					roomfile.read((char*)&regionHeight, sizeof(int));

					Rect sheetRegion;

					sheetRegion.x = regionLeft;

					sheetRegion.y = regionTop;

					sheetRegion.w = regionWidth;

					sheetRegion.h = regionHeight;

					sheetRegions.push_back(sheetRegion);
				}

				int positionX = 0;
				roomfile.read((char*)&positionX, sizeof(int));

				int positionY = 0;
				roomfile.read((char*)&positionY, sizeof(int));

				// Create a tile entity, and add it's ID to the grid(s).
				boost::shared_ptr<Entity> newTile = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
				EntityControllerPtr controller = newTile->getComponents()->createTileController(renderer_);
				newTile->getComponents()->initialize();

				boost::shared_ptr<EntityMetadata> metadata = newTile->getComponents()->getEntityMetadata();

				metadata->setEntityTypeId(ids_->ENTITY_TILE);
							
				metadata->roomMetadata_ = roomMetadata_;
				metadata->previousRoomMetadata_ = roomMetadata_;

				boost::shared_ptr<CodeBehindContainer> codeBehindContainer = newTile->getComponents()->getCodeBehindContainer();

				codeBehindContainer->controller_ = controller;
				codeBehindContainer->hitboxManager_ = hitboxManager_;
				codeBehindContainer->ids_ = ids_;
				codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
				codeBehindContainer->tileSize_ = tileSize;
				codeBehindContainer->timer_ = timer_;
				codeBehindContainer->renderer_ = renderer_;
				codeBehindContainer->ui_ = ui_;

				codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;
				codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;

				codeBehindContainer->setIsCollidable();
							
				codeBehindContainer->createCodeBehinds(ids_->ENTITY_TILE, ENTITY_CLASSIFICATION_TILE);
				codeBehindContainer->initialize();

				metadata->classification_ = ENTITY_CLASSIFICATION_TILE;

				newTile->validate();
							
				controller->renderer_ = renderer_;
				controller->animationManager_ = animationManager_;
				controller->anchorPointManager_ = anchorPointManager_;
				controller->hitboxManager_ = hitboxManager_;
				controller->timer_ = timer_;
				controller->textManager_ = textManager_;
				controller->fontManager_ = fontManager_;
				controller->engineConfig_ = engineConfig_;
				controller->debugger_ = debugger_;
				controller->engineController_ = engineController_;
				controller->messenger_ = messenger_;
				controller->queryManager_ = queryManager_;
							
				metadata->layerIndex_ = i;
				newTile->transitionId_ = ids_->TRANSITION_NULL;

				//newTile->attachRenderable();

				//newTile->initialize();

				//int tileX = k * tileSize;
				//int tileY = j * tileSize;

				controller->setPosition(positionX, positionY);
								
				boost::shared_ptr<TileRenderable> r = boost::static_pointer_cast<TileRenderable>(newTile->components_->getRenderable());

				r->setHeight(tileSize);
							
				r->setWidth(tileSize);
							
				r->getPosition()->setPreviousX(positionX);
							
				r->getPosition()->setPreviousY(positionY);

				r->setTileSheetId(translatedTileSheetId);

				r->setScaleFactor(tileSheet->getScaleFactor());
				
				r->setTileSheetRegions(sheetRegions);

				r->setFramesPerSecond(framesPerSecond);
				
				// Render order is calculated in increments of 10 based on the layer. The interactive
				// layer is order 0. Everything below is negative, everything above is positive.
				r->setRenderOrder((i - interactiveIndex) * 10);

				r->setMapLayer(metadata->layerIndex_);

				addEntity(newTile, true);
				
				newTile->initialize();

				itemsLoadedCounter++;

				tileObjectsLoadedCounter++;

				roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
			}

			if (i == interactiveIndex)
			{
				// Load world geometry
				int worldGeometryCount = 0;
				roomfile.read((char*)&worldGeometryCount, sizeof(int));

				for (int l = 0; l < worldGeometryCount; l++)
				{
					int x = 0;
					roomfile.read((char*)&x, sizeof(int));

					int y = 0;
					roomfile.read((char*)&y, sizeof(int));

					int width = 0;
					roomfile.read((char*)&width, sizeof(int));

					int height = 0;
					roomfile.read((char*)&height, sizeof(int));

					unsigned int collisionStyleUint = 0;
					roomfile.read((char*)&collisionStyleUint, sizeof(unsigned int));

					CollisionStyle collisionStyle = (CollisionStyle)collisionStyleUint;

					float slope = 0;
					roomfile.read((char*)&slope, sizeof(slope));

					// findmeremove, I'm not sure I want to do this, I may have a better solution.
					// For slopes, extend the top by one tile height. This will need to be more generalized to handle all directions eventually.
					//switch (collisionStyle)
					//{
					//	case COLLISION_STYLE_INCLINE:
					//	case COLLISION_STYLE_DECLINE:

							//y -= tileSize;

							//height += tileSize;

					//		break;
					//}
					
					bool useTopEdge = true;
					roomfile.read((char*)&useTopEdge, sizeof(bool));

					bool useRightEdge = true;
					roomfile.read((char*)&useRightEdge, sizeof(bool));

					bool useBottomEdge = true;
					roomfile.read((char*)&useBottomEdge, sizeof(bool));

					bool useLeftEdge = true;
					roomfile.read((char*)&useLeftEdge, sizeof(bool));

					// Create a tile entity, and add it's ID to the grid(s).
					boost::shared_ptr<Entity> newTile = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
					boost::shared_ptr<TileController> controller = boost::static_pointer_cast<TileController>(newTile->getComponents()->createTileController(nullptr));
					newTile->getComponents()->initialize();

					boost::shared_ptr<EntityMetadata> metadata = newTile->getComponents()->getEntityMetadata();

					metadata->setEntityTypeId(ids_->ENTITY_TILE);
									
					metadata->roomMetadata_ = roomMetadata_;
					metadata->previousRoomMetadata_ = roomMetadata_;

					boost::shared_ptr<CodeBehindContainer> codeBehindContainer = newTile->getComponents()->getCodeBehindContainer();

					codeBehindContainer->controller_ = controller;
					codeBehindContainer->hitboxManager_ = hitboxManager_;
					codeBehindContainer->ids_ = ids_;
					codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
					codeBehindContainer->tileSize_ = tileSize;
					codeBehindContainer->timer_ = timer_;
					codeBehindContainer->renderer_ = renderer_;
					codeBehindContainer->ui_ = ui_;

					codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

					codeBehindContainer->setIsCollidable();

					codeBehindContainer->createCodeBehinds(ids_->ENTITY_TILE, ENTITY_CLASSIFICATION_TILE);
					codeBehindContainer->initialize();

					boost::shared_ptr<TileCollidableCodeBehind> collidableCodeBehind = boost::static_pointer_cast<TileCollidableCodeBehind>(codeBehindContainer->getCollidableCodeBehind());
									
					collidableCodeBehind->setTileGroupId(l + 1);
									
					metadata->classification_ = ENTITY_CLASSIFICATION_TILE;
									
					newTile->validate();

					controller->renderer_ = renderer_;
					controller->animationManager_ = animationManager_;
					controller->anchorPointManager_ = anchorPointManager_;
					controller->hitboxManager_ = hitboxManager_;
					controller->timer_ = timer_;
					controller->textManager_ = textManager_;
					controller->fontManager_ = fontManager_;
					controller->engineConfig_ = engineConfig_;
					controller->debugger_ = debugger_;
					controller->engineController_ = engineController_;
					controller->messenger_ = messenger_;
					controller->queryManager_ = queryManager_;
									
					metadata->layerIndex_ = i;
					newTile->transitionId_ = ids_->TRANSITION_NULL;

					//newTile->attachHitboxController(); Moved into components. Created when controller is created.

					controller->setPosition(x, y);

					boost::shared_ptr<HitboxController> hitboxController = newTile->getComponents()->getHitboxController();

					metadata->height_ = height;
					metadata->width_ = width;

					hitboxController->setStageHeight(height);
					hitboxController->setStageWidth(width);

					boost::shared_ptr<Hitbox> worldGeometryHitbox = boost::shared_ptr<Hitbox>(new Hitbox(0, 0, height, width));

					worldGeometryHitbox->setIdentity(ids_->HITBOX_WORLDGEOMETRY);

					worldGeometryHitbox->setIsSolid(true);

					worldGeometryHitbox->setCollisionStyle(collisionStyle);

					worldGeometryHitbox->setSlope(slope);

					worldGeometryHitbox->setUseTopEdge(useTopEdge);

					worldGeometryHitbox->setUseRightEdge(useRightEdge);

					worldGeometryHitbox->setUseBottomEdge(useBottomEdge);

					worldGeometryHitbox->setUseLeftEdge(useLeftEdge);

					int hitboxId = hitboxManager_->addHitbox(worldGeometryHitbox);
					
					unsigned char edgeFlags = 0xFF;

					hitboxController->activateHitbox(hitboxId, edgeFlags);
									
					addEntity(newTile, true);

					newTile->initialize();

					itemsLoadedCounter++;

					worldGeometryLoadedCounter++;

					roomMetadata_->percentLoaded_ = (int)(((double)itemsLoadedCounter / (double)itemsToLoadCount) * 100);
				}
			}

			// Point the text manager to this layer's position entity.
			boost::shared_ptr<Position> p = renderableManager_->getLayerPosition(roomMetadata_->myIndex_, i);

			if (textManager_ != nullptr)
			{
				textManager_->addLayerPosition(roomMetadata_->myIndex_, p);
			}
		}

		physicsManager_->initialize(roomMetadata_->myIndex_);
		renderableManager_->initialize(roomMetadata_->myIndex_);

		roomfile.close();

		if (roomMetadata_->percentLoaded_ != 100)
		{
			debugger_->streamLock->lock();
			std::cout << "Potential bug: The room has finished loading but the percent loaded value is less than 100" << std::endl;
			debugger_->streamLock->unlock();
		}
	}

	debugger_->streamLock->lock();
	std::cout << "[" << boost::this_thread::get_id() << "] Room loaded "<<fileName_<<" locally."<<std::endl;
	debugger_->streamLock->unlock();

	// Call the created event for the entities in this room.
	for (size_t i = 0; i < masterEntityList_.size(); i++)
	{
		auto entity = masterEntityList_[i];

		boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entity->getComponents()->getCodeBehindContainer();

		boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();

		entityCodeBehind->created();
	}

	roomLoaded();

	roomMetadata_->percentLoaded_ = 100;

	// These must be set after the roomLoaded function, so that nothing can access the room
	// until the function has returned.
	isLoaded_ = true;

	isLoading_ = false;

	runLoadedEvent_ = true;
}

void Room::setMovedEntityPositions()
{
	// For any entities which are waiting to be moved into the room, set their positions now.
	int size = entityInsertQueue_.size();

	for (int i = 0; i < size; i++)
	{
		EntityInsertQueueItem item = entityInsertQueue_[i];

		// Pre-set the position the entity will be in when it enters the room, if the room is already loaded.
		// This is so that the camera will be positioned correctly if there are a couple frames of rendering before
		// the actual insert happens (the rendering and game updates are not necessarily in synch with one another).
		ChangeRoomParameters params;

		params.spawnPointId = item.spawnPointId;
		params.offsetX = item.offsetX;
		params.offsetY = item.offsetY;

		setEntityPosition(item.entity, params);
	}
}

void Room::unloadRoomAsync()
{
	std::string dontOptimizeAwayLoop = "..";

	// If the room is not currently processing an update, unload it.
	while (isUpdating_ == true)
	{
		dontOptimizeAwayLoop = "";
	}
	
	isUnloading_ = true;
	
	isLoaded_ = false;

	roomMetadata_->percentLoaded_ = 0;

	bool displayStatus = false;

	if (engineController_->getHasQuit() == true)
	{
		return;
	}

	debugger_->streamLock->lock();
	std::cout << "[" << boost::this_thread::get_id() << "] Unloading Room " << roomMetadata_->roomId_ << " locally." << dontOptimizeAwayLoop << std::endl;
	debugger_->streamLock->unlock();
	
	int size = masterEntityListTiles_.size();

	for (int i = size - 1; i >= 0; i--)
	{
		removeTileEntity(i);
	}

	size = masterEntityList_.size();

	for (int i = size - 1; i >= 0; i--)
	{
		removeEntity(i, true, false);
	}

	spawnPoints_.clear();

	audioSourceContainer_->unloadRoomAudioSources(roomMetadata_->myIndex_);

/*
	if (displayStatus == true)
	{
		std::cout << "Unloading HUD elements" << std::endl;
	}

	if (displayStatus == true)
	{
		std::cout << "Unloading tiles" << std::endl;
	}

	if (displayStatus == true)
	{
		std::cout << "Unloading actors" << std::endl;
	}

	if (displayStatus == true)
	{
		std::cout << "Loading events" << std::endl;
	}

	if (displayStatus == true)
	{
		std::cout << "Loading spawn points" << std::endl;
	}

	if (displayStatus == true)
	{
		std::cout << "Loading particle emitters" << std::endl;
	}

	if (displayStatus == true)
	{
		std::cout << "Loading particles" << std::endl;
	}

	if (displayStatus == true)
	{
		std::cout << "Loading audio sources" << std::endl;
	}*/

	// Do the physics an renderable managers need to be unloaded?

	isUnloading_ = false;

	debugger_->streamLock->lock();
	std::cout << "[" << boost::this_thread::get_id() << "] Room unloaded " << fileName_ << " locally." << std::endl;
	debugger_->streamLock->unlock();	
}

int	Room::getPercentLoaded()
{
	return roomMetadata_->percentLoaded_;
}

int	Room::getLoadingScreenId()
{
	return roomMetadata_->loadingScreenId_;
}

void Room::setLoadingScreenId(int value)
{
	roomMetadata_->loadingScreenId_ = value;
}

void Room::initializeEntities()
{
	if (isInitialized_ == false)
	{
		bool displayThreadData = true;

		if (displayThreadData == true)
		{
			boost::thread::id threadId = boost::this_thread::get_id();

			debugger_->streamLock->lock();
			std::cout << "[" << boost::this_thread::get_id() << "] Initializing room "<< roomMetadata_->roomId_<<" on client"<<std::endl;
			debugger_->streamLock->unlock();			
		}

		//int size = cameraList_.size();
		//for (int i = 0; i < size; i++)
		//{
		//	cameraList_[i]->initialize();
		//}

		//size = masterEntityList_.size();
		//for (int i = 0; i < size; i++)
		//{
		//	Entity* currentEntity = masterEntityList_[i];

		//	// Entity may have been created from the initialized of another entity.
		//	if (currentEntity->isInitialized_ == false)
		//	{						
		//		currentEntity->initialize();

		//		if (currentEntity->components_->getDynamicsController() != nullptr)
		//		{
		//			EntityTypeId entityTypeId = currentEntity->getComponents()->getEntityMetadata()->getEntityTypeId();

		//			boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(entityTypeId);
		//	
		//			DynamicsController* dc = currentEntity->components_->getDynamicsController();
		//	
		//			if (dc != nullptr)
		//			{
		//				dc->setOwnerStageHeight(entityTemplate->getStageHeight());
		//				dc->setOwnerStageWidth(entityTemplate->getStageWidth());

		//				// Need to know the layer position for visual debug rendering.
		//				Position* p = renderableManager_->getLayerPosition(roomMetadata_->myIndex_, i);

		//				dc->layerPosition_ = p;
		//			}
		//		}
		//	}
		//}
		
		//particleEmitterContainer_->initialize(roomMetadata_->myIndex_);findmdebug
		physicsManager_->initialize(roomMetadata_->myIndex_);
		renderableManager_->initialize(roomMetadata_->myIndex_);

		isInitialized_ = true;

		if (displayThreadData == true)
		{
			boost::thread::id threadId = boost::this_thread::get_id();

			debugger_->streamLock->lock();
			std::cout << "[" << boost::this_thread::get_id() << "] Room "<< roomMetadata_->roomId_<<" initialized on client"<<std::endl;
			debugger_->streamLock->unlock();
		}

		timer_->tick();
	}
}

void Room::initializePythonData()
{
	PythonAcquireGil lock;

	try
	{
		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = "from ";

		sCode += scriptVar_ + " import " + scriptTypeName_ + "\n";
		sCode += scriptVar_ + " = " + scriptTypeName_ + "()";
	
		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");
	
		str pyCode(sCode);	
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Get the instance for this object.
		pyRoomInstance_ = extract<object>(pyMainNamespace_[scriptVar_]);
		pyRoomInstanceNamespace_ = pyRoomInstance_.attr("__dict__");
	
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyRoomInstanceNamespace_["firemelon"] = pyFiremelonModule;

		pyRoomLoaded_ = pyRoomInstance_.attr("roomLoaded");
		pyRoomLoading_ = pyRoomInstance_.attr("roomLoading");
		pyRoomDisplayed_ = pyRoomInstance_.attr("roomDisplayed");
		pyRoomHidden_ = pyRoomInstance_.attr("roomHidden");
		pyEntityEntered_ = pyRoomInstance_.attr("entityEntered");
		pyEntityCreated_ = pyRoomInstance_.attr("entityCreated");
		pyEntityExited_ = pyRoomInstance_.attr("entityExited");
		pyUpdate_ = pyRoomInstance_.attr("update");

		pyRoomInstanceNamespace_["metadata"] = ptr(roomMetadata_.get());
		pyRoomInstanceNamespace_["controller"] = ptr(this);
	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading room script data " + scriptTypeName_<<std::endl;
		debugger_->handlePythonError();
	}
}

void Room::setFileName(std::string fileName)
{
	fileName_ = fileName;
}

bool Room::getIsLoading()
{
	return isLoading_;
}

void Room::setIsLoading(bool value)
{
	isLoading_ = value;
}

bool Room::getIsUnloading()
{
	return isUnloading_;
}

bool Room::getIsLoaded()
{
	return isLoaded_;
}

void Room::setIsLoaded(bool value)
{
	isLoaded_ = value;
}

int Room::getIndex()
{
	return roomMetadata_->myIndex_;
}

void Room::setAssets(boost::shared_ptr<Assets> assets)
{
	assets_ = assets;
}

EntityTypeMap Room::getEntityTypeMapPy()
{
	PythonReleaseGil unlocker;

	return getEntityTypeMap();
}

EntityTypeMap Room::getEntityTypeMap()
{
	return entityTypeMap_;
}

boost::shared_ptr<Entity> Room::getEntityById(int entityId)
{
	// Find the entity with the given ID in the ID list, and use that index to return the entity in the master entity list.

	std::vector<int>::iterator itr;

	int size = masterEntityIdList_.size();
	if (entityId < size)
	{
		// If there are more elements in the list than the entity ID, then the
		// entire list does not need to be searched. It's impossible for the entity ID to be
		// located at an index greater than itself, because insertions are only done at the end.
		itr = std::lower_bound(masterEntityIdList_.begin(), masterEntityIdList_.begin() + entityId, entityId); 
	}
	else
	{
		itr = std::lower_bound(masterEntityIdList_.begin(), masterEntityIdList_.end(), entityId); 
	}
		
	int lowerBoundPosition = itr - masterEntityIdList_.begin();

	size = masterEntityIdList_.size();

	if (lowerBoundPosition < size)
	{
		if (masterEntityIdList_[lowerBoundPosition] == entityId)
		{
			return masterEntityList_[lowerBoundPosition];
		}
		else
		{
			return nullptr;
		}
	}
	else
	{
		return nullptr;
	}
}

boost::shared_ptr<Entity> Room::getEntityById(EntityTypeId entityType, int entityId)
{
	// Find the entity with the given ID in the ID list, and use that index to return the entity in the master entity list.

	std::vector<int>::iterator itr;
	
	std::vector<boost::shared_ptr<Entity>> entityList = entityTypeMap_.map_[entityType].entityList_;
	std::vector<int> entityIdList = entityTypeIdMap_[entityType];

	int size = entityIdList.size();

	if (entityId < size)
	{
		// If there are more elements in the list than the entity ID, then the
		// entire list does not need to be searched. It's impossible for the entity ID to be
		// located at an index greater than itself, because insertions are only done at the end.
		itr = std::lower_bound(entityIdList.begin(), entityIdList.begin() + entityId, entityId);
	}
	else
	{
		itr = std::lower_bound(entityIdList.begin(), entityIdList.end(), entityId);
	}
		
	int lowerBoundPosition = itr - entityIdList.begin();

	size = entityIdList.size();

	if (lowerBoundPosition < size)
	{
		if (entityIdList[lowerBoundPosition] == entityId)
		{
			return entityList[lowerBoundPosition];
		}
		else
		{
			return nullptr;
		}
	}
	else
	{
		return nullptr;
	}
}

boost::shared_ptr<Entity> Room::getEntityByName(std::string entityInstanceName)
{
	// Map the given name to an ID and get it.

	int entityId = entityNameIdMap_[entityInstanceName];

	return getEntityById(entityId);
}

boost::shared_ptr<Entity> Room::getEntityByName(EntityTypeId entityType, std::string entityInstanceName)
{
	// Map the given name to an ID and get it.

	std::map<std::string, int>::iterator it = entityNameIdMap_.find(entityInstanceName);
	
	int charWidth = 0;
	int charHeight = 0;

	if (it == entityNameIdMap_.end())
	{
		std::cout<<"Error: Entity \"" + entityInstanceName + "\" was not found."<<std::endl;

		return nullptr;
	}
	else
	{
		int entityId = entityNameIdMap_[entityInstanceName];

		return getEntityById(entityType, entityId);
	}
}

object Room::getEntityPyInstanceById(int entityId)
{
	// Find the entity with the given ID in the ID list, and use that index to return the entity in the master entity list.
	
	//PythonAcquireGil lock;

	if (entityId >= 0)
	{
		std::vector<int>::iterator itr;

		int size = masterEntityIdList_.size();
		if (entityId < size)
		{
			// If there are more elements in the list than the entity ID, then the
			// entire list does not need to be searched. It's impossible for the entity ID to be
			// located at an index greater than itself, because insertions are only done at the end.
			itr = std::lower_bound(masterEntityIdList_.begin(), masterEntityIdList_.begin() + entityId, entityId);
		}
		else
		{
			itr = std::lower_bound(masterEntityIdList_.begin(), masterEntityIdList_.end(), entityId);
		}

		int lowerBoundPosition = itr - masterEntityIdList_.begin();

		size = masterEntityIdList_.size();

		if (lowerBoundPosition < size)
		{
			if (masterEntityIdList_[lowerBoundPosition] == entityId)
			{
				boost::shared_ptr<Entity> entity = masterEntityList_[lowerBoundPosition];
				boost::shared_ptr<EntityComponents> components = entity->getComponents();
				boost::shared_ptr<CodeBehindContainer> codeBehindContainer = components->getCodeBehindContainer();
				boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper = codeBehindContainer->getPythonInstanceWrapper();

				return pythonInstanceWrapper->getPyInstance();
			}
			else
			{
				return object();
			}
		}
		else
		{
			return object();
		}
	}
	
	return object();
}

object Room::getEntityPyInstanceById(EntityTypeId entityType, int entityId)
{
	//PythonAcquireGil lock;

	// Find the entity with the given ID in the ID list, and use that index to return the entity in the master entity list.

	std::vector<int>::iterator itr;
	
	std::vector<boost::shared_ptr<Entity>> entityList = entityTypeMap_.map_[entityType].entityList_;
	std::vector<int> entityIDList = entityTypeIdMap_[entityType];

	int size = entityIDList.size();

	if (entityId < size)
	{
		// If there are more elements in the list than the entity ID, then the
		// entire list does not need to be searched. It's impossible for the entity ID to be
		// located at an index greater than itself, because insertions are only done at the end.
		itr = std::lower_bound(entityIDList.begin(), entityIDList.begin() + entityId, entityId); 
	}
	else
	{
		itr = std::lower_bound(entityIDList.begin(), entityIDList.end(), entityId); 
	}
		
	int lowerBoundPosition = itr - entityIDList.begin();

	size = entityIDList.size();

	if (lowerBoundPosition < size)
	{
		if (entityIDList[lowerBoundPosition] == entityId)
		{
			return entityList[lowerBoundPosition]->getComponents()->getCodeBehindContainer()->getPythonInstanceWrapper()->getPyInstance();
		}
		else
		{
			return object();
		}
	}
	else
	{
		return object();
	}
}

object Room::getEntityPyInstanceByName(std::string entityInstanceName)
{
	// Map the given name to an ID and get it.

	int entityId = entityNameIdMap_[entityInstanceName];

	return getEntityPyInstanceById(entityId);
}

object Room::getEntityPyInstanceByName(EntityTypeId entityType, std::string entityInstanceName)
{
	// Map the given name to an ID and get it.

	std::map<std::string, int>::iterator it = entityNameIdMap_.find(entityInstanceName);
	
	int charWidth = 0;
	int charHeight = 0;

	if (it == entityNameIdMap_.end())
	{
		std::cout<<"Error: Entity \"" + entityInstanceName + "\" was not found."<<std::endl;

		return object();
	}
	else
	{
		int entityId = entityNameIdMap_[entityInstanceName];

		return getEntityPyInstanceById(entityType, entityId);
	}
}

boost::shared_ptr<Entity> Room::extractEntity(int entityInstanceId)
{
	boost::shared_ptr<Entity> extractionEntity = getEntityById(entityInstanceId);

	if (extractionEntity != nullptr)
	{	
		int size = masterEntityList_.size();
		for (int j = 0; j < size; j++)
		{
			boost::shared_ptr<Entity> entity = masterEntityList_[j];

			EntityControllerPtr controller = entity->getComponents()->getEntityController();
			boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entity->getComponents()->getCodeBehindContainer();
			boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();
			boost::shared_ptr<EntityMetadata> metadata = entity->getComponents()->getEntityMetadata();

			if (metadata->entityInstanceId_ == entityInstanceId)
			{
				entityCodeBehind->roomExited(roomMetadata_->roomId_);

				removeEntity(j, false, false);

				break;
			}
		}

		return extractionEntity;
	}
	else
	{
		return nullptr;
	}
}

void Room::insertEntity(boost::shared_ptr<Entity> entity, SpawnPointId spawnPointId, int offsetX, int offsetY)
{	
	if (isLoaded_ == false)
	{
		if (debugger_->getDebugMode() == true)
		{
			std::cout << "Entity " << entity->getComponents()->getEntityMetadata()->name_ << " inserted into room " << getMetadata()->getRoomId() << " but it has not been loaded yet." << std::endl
				<< "Adding it to a wait queue, to be inserted upon room load." << std::endl;
		}

		// If the room is not yet loaded, queue this entity to be inserted after the room load is complete.
		EntityInsertQueueItem item;
		item.entity = entity;
		item.spawnPointId = spawnPointId;
		item.offsetX = offsetX;
		item.offsetY = offsetY;
		
		entityInsertQueue_.push_back(item); 
	}
	else
	{
		if (debugger_->getDebugMode() == true)
		{
			std::cout << "Entity " << entity->getComponents()->getEntityMetadata()->name_ << " inserted into room " << getMetadata()->getRoomId() << std::endl;
		}

		ChangeRoomParameters params;

		params.spawnPointId = spawnPointId;
		params.offsetX = offsetX;
		params.offsetY = offsetY;

		setEntityPosition(entity, params);
		
		boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entity->getComponents()->getCodeBehindContainer();
		boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();
		boost::shared_ptr<EntityMetadata> metadata = entity->getComponents()->getEntityMetadata();
		
		int spawnPointLayerIndex = 0;

		int size = spawnPoints_.size();

		for (int i = 0; i < size; i++)
		{
			if (spawnPoints_[i]->getId() == spawnPointId)
			{
				spawnPointLayerIndex = spawnPoints_[i]->getLayer();

				break;
			}
		}

		metadata->layerIndex_ = spawnPointLayerIndex;

		metadata->roomMetadata_ = roomMetadata_;

		boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(metadata->getEntityTypeId());
		
		int renderableCount = entity->getComponents()->getRenderableCount();

		for (int i = 0; i < renderableCount; i++)
		{
			boost::shared_ptr<Renderable> renderable = entity->getComponents()->getRenderable(i);

			if (renderable != nullptr)
			{
				renderable->setMapLayer(metadata->layerIndex_);
			}
		}
		
		boost::shared_ptr<HitboxController> hc = entity->getComponents()->getHitboxController();

		if (hc != nullptr)
		{
			// Default the collision grid data to bounds to -1 to 0, so that it doesn't use data from the previous room it was in.
			hc->setCollisionGridCellStartRow(0);
			hc->setCollisionGridCellEndRow(-1);
			hc->setCollisionGridCellStartCol(0);
			hc->setCollisionGridCellEndCol(-1);
		}

		entity->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
		entity->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
		entity->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));

		addEntity(entity, false);

		////// Do I need to do anything in initialize? I don't think so. I hope not.
		//////o->initialize();

		if (entity->components_->getDynamicsController() != nullptr)
		{
			boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(metadata->getEntityTypeId());
				
			DynamicsController* dc = entity->components_->getDynamicsController();
				
			if (dc != nullptr)
			{
				// Need to know the layer position for visual debug rendering.
				int layerIndex = entity->getComponents()->getEntityMetadata()->layerIndex_;

				boost::shared_ptr<Position> p = renderableManager_->getLayerPosition(roomMetadata_->myIndex_, layerIndex);

				dc->layerPosition_ = p;
			}
		}
		
		resetAudioListener();

		entityNameIdMap_[metadata->name_] = metadata->entityInstanceId_;

		entityCodeBehind->preRoomEntered(roomMetadata_->roomId_);

		entityEntered(entity);
	}
}

void Room::killEntity(int entityInstanceId)
{
	if (entityInstanceId >= 0)
	{
		// Find and flag the entity as removed.
		boost::shared_ptr<Entity> temp = getEntityById(entityInstanceId);

		if (temp != nullptr)
		{
			temp->remove();
		}
	}
}

void Room::setActiveCamera(boost::shared_ptr<Entity> camera)
{

}

int Room::addEntity(boost::shared_ptr<Entity> entity, bool insertAtEnd)
{
	// Setup the entity, add it to lists, and wire up all the required connections with engine subsystems.

	EntityControllerPtr controller = entity->getComponents()->getEntityController();
	boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entity->getComponents()->getCodeBehindContainer();
	boost::shared_ptr<EntityMetadata> metadata = entity->getComponents()->getEntityMetadata();

	boost::shared_ptr<InputReceiverCodeBehind> inputReceiverCodeBehind = codeBehindContainer->getInputReceiverCodeBehind();

	if (inputReceiverCodeBehind != nullptr)
	{
		// Connect the room's button signals to this entity.		
		buttonDownSignal_.connect(boost::bind(&InputReceiverCodeBehind::preButtonDown, inputReceiverCodeBehind, _1));
		buttonUpSignal_.connect(boost::bind(&InputReceiverCodeBehind::preButtonUp, inputReceiverCodeBehind, _1));
	}

	controller->queryParametersFactory_ = queryParametersFactory_;
	controller->queryResultFactory_ = queryResultFactory_;
	controller->renderer_ = renderer_;
	controller->timer_ = timer_;
	controller->inputDeviceManager_ = inputDeviceManager_;
	controller->audioPlayer_ = audioPlayer_;
	controller->audioSourceContainer_ = audioSourceContainer_;
	controller->textManager_ = textManager_;
	controller->fontManager_ = fontManager_;
	controller->engineConfig_ = engineConfig_;
	controller->debugger_ = debugger_;
	controller->engineController_ = engineController_;
	controller->messenger_ = messenger_;
	controller->queryManager_ = queryManager_;
	controller->queryResultFactory_ = queryResultFactory_;
	controller->queryParametersFactory_ = queryParametersFactory_;
	controller->roomMetadataContainer_ = roomMetadataContainer_;
						
	entityTypeMap_.map_[metadata->getEntityTypeId()].entityList_.push_back(entity);
	entityTypeIdMap_[metadata->getEntityTypeId()].push_back(metadata->entityInstanceId_);
	
	std::string entityName = entity->components_->getEntityMetadata()->name_;

	if (entityNameValidator_->isNameInUse(entityName) == false)
	{
		//entityNameValidator_->addName(entityName);
	}

	int insertedAtIndex = -1;

	if (entity->getComponents()->getEntityMetadata()->classification_ == ENTITY_CLASSIFICATION_TILE)
	{
		masterEntityListTiles_.push_back(entity);
	}
	else
	{
		// Master list stores all entities.
		if (insertAtEnd == true)
		{
			insertedAtIndex = masterEntityList_.size();

			masterEntityList_.push_back(entity);
			masterEntityIdList_.push_back(entity->instanceId_);
		}
		else
		{
			// Find the	index of the next highest entity ID, to determine where to insert.
			std::vector<int>::iterator itr;
			itr = std::lower_bound(masterEntityIdList_.begin(), masterEntityIdList_.end(), metadata->entityInstanceId_);

			int lowerBoundPosition = itr - masterEntityIdList_.begin();

			masterEntityIdList_.insert(itr, metadata->entityInstanceId_);

			masterEntityList_.insert(masterEntityList_.begin() + lowerBoundPosition, entity);
		}
	}

	bool isDynamic = entity->getComponents()->getHasDynamicsController();
	bool isRenderable = (entity->getComponents()->getRenderable() != nullptr);
	
	if (isDynamic == true)
	{
		// Dynamic entity list. These are entities that can move around.
		// Stores a list of indexes to the master list.		
		std::vector<int>::iterator itr;
		itr = std::lower_bound(dynamicEntityIdList_.begin(), dynamicEntityIdList_.end(), metadata->entityInstanceId_);

		int lowerBoundPosition = itr - dynamicEntityIdList_.begin();

		dynamicEntityIdList_.insert(dynamicEntityIdList_.begin() + lowerBoundPosition, metadata->entityInstanceId_);
		dynamicEntityList_.insert(dynamicEntityList_.begin() + lowerBoundPosition, entity);
	}
	else
	{
		// Static entity list. These don't move, like world geometry.
		// Stores a list of indexes to the master list.
		staticEntityIdList_.push_back(metadata->entityInstanceId_);
		staticEntityList_.push_back(entity);
	}

	// Collisions should only be processed for entities that exist on the interactive layer.	
	int entityLayer = metadata->layerIndex_;
	int interactiveLayer = renderableManager_->getInteractiveLayer(roomMetadata_->myIndex_);

	if (entityLayer == interactiveLayer || isDynamic == true)
	{		
		physicsManager_->addEntityComponents(roomMetadata_->myIndex_, entity->components_);
	}

	if (isRenderable == true)
	{
		int renderableCount = entity->components_->getRenderableCount();

		for (int i = 0; i < renderableCount; i++)
		{
			boost::shared_ptr<Renderable> renderable = entity->components_->getRenderable(i);

			renderableManager_->addRenderable(roomMetadata_->myIndex_, renderable);
		}
	}
	
	return insertedAtIndex;
}

void Room::removeEntity(int index, bool deleteEntity, bool runDestroy)
{
	// Removes an entity in the master list at the given index, and unregisters it from subsystems.

	boost::shared_ptr<Entity> entityToRemove = masterEntityList_[index];

	entityExited(entityToRemove);

	// If deleting a camera: If it's the active camera, the active camera should be cleared.
	if (deleteEntity == true)
	{
		if (entityToRemove->getComponents()->getEntityMetadata()->getEntityTypeId() == ids_->ENTITY_CAMERA)
		{
			auto activeCamera = cameraManager_->getActiveCamera();

			if (activeCamera != nullptr)
			{
				int entityToRemoveId = entityToRemove->getComponents()->getEntityMetadata()->getEntityInstanceId();
				int activeCameraId = activeCamera->getMetadata()->getEntityInstanceId();

				if (activeCameraId == entityToRemoveId)
				{
					cameraManager_->setActiveCamera(nullptr);
				}
			}
		}
	}

	// Flag entity as no longer valid, so collision events involving it will not be processed.
	entityToRemove->invalidate();

	EntityControllerPtr controller = entityToRemove->getComponents()->getEntityController();
	boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entityToRemove->getComponents()->getCodeBehindContainer();
	boost::shared_ptr<InputReceiverCodeBehind> inputReceiverCodeBehind = codeBehindContainer->getInputReceiverCodeBehind();
	boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();

	if (controller->inputDeviceManager_ != nullptr)
	{
		buttonDownSignal_.disconnect(boost::bind(&InputReceiverCodeBehind::preButtonDown, inputReceiverCodeBehind, _1));
		buttonUpSignal_.disconnect(boost::bind(&InputReceiverCodeBehind::preButtonUp, inputReceiverCodeBehind, _1));
	}

	bool isDynamic = entityToRemove->components_->getDynamicsController() != nullptr;

	boost::shared_ptr<Renderable> renderable = entityToRemove->components_->getRenderable();

	int entityInstanceId = entityToRemove->getComponents()->getEntityMetadata()->entityInstanceId_;
	
	EntityTypeId entityId = entityToRemove->getComponents()->getEntityMetadata()->getEntityTypeId();
	std::string entityName = entityToRemove->getComponents()->getEntityMetadata()->name_;

	if (isDynamic == true)
	{
		// The dynamicEntityIDList vector should always be sorted, as it always only inserts increasing entity IDs at the end.

		// Find the lower bound. If the element found matches the value we are looking for,
		// remove it and reindex everything higher.
		std::vector<int>::iterator itr;

		itr = std::lower_bound(dynamicEntityIdList_.begin(), dynamicEntityIdList_.end(), entityInstanceId); 
				
		int lowerBoundPosition = itr - dynamicEntityIdList_.begin();

		if (dynamicEntityIdList_[lowerBoundPosition] == entityInstanceId)
		{
			// Maintain parallelism.
			dynamicEntityIdList_.erase(dynamicEntityIdList_.begin() + lowerBoundPosition);
			dynamicEntityList_.erase(dynamicEntityList_.begin() + lowerBoundPosition);
		}	
	}
	else
	{
		// The staticEntityIDList vector should always be sorted, as it always only inserts increasing entity IDs at the end.

		// Find the lower bound. If the element found matches the value we are looking for,
		// remove it and reindex everything higher.
		std::vector<int>::iterator itr;

		itr = std::lower_bound(staticEntityIdList_.begin(), staticEntityIdList_.end(), index); 
				
		int lowerBoundPosition = itr - staticEntityIdList_.begin();

		if (staticEntityIdList_[lowerBoundPosition] == index)
		{
			// Maintain paralellism.
			staticEntityIdList_.erase(staticEntityIdList_.begin() + lowerBoundPosition);
			staticEntityList_.erase(staticEntityList_.begin() + lowerBoundPosition);
		}
	}

	// Also remove from the entity type map.
	std::vector<int>::iterator itr;
	itr = std::lower_bound(entityTypeIdMap_[entityId].begin(), entityTypeIdMap_[entityId].end(), entityInstanceId);
		
	int lowerBoundPosition = itr - entityTypeIdMap_[entityId].begin();

	int size = entityTypeIdMap_[entityId].size();

	if (lowerBoundPosition < size)
	{
		// If the index was found exactly, remove it.
		if (entityTypeIdMap_[entityId][lowerBoundPosition] == entityInstanceId)
		{
			entityTypeIdMap_[entityId].erase(entityTypeIdMap_[entityId].begin() + lowerBoundPosition);
			entityTypeMap_.map_[entityId].entityList_.erase(entityTypeMap_.map_[entityId].entityList_.begin() + lowerBoundPosition);
		}
	}

	// Remove the entity's hitbox container from the physics manager.
	int layerIndex = entityToRemove->getComponents()->getEntityMetadata()->layerIndex_;

	if (layerIndex == renderableManager_->getInteractiveLayer(roomMetadata_->myIndex_) || isDynamic == true)
	{		
		physicsManager_->removeEntityComponents(roomMetadata_->myIndex_, entityInstanceId);
	}

	// Remove the entity's renderables from the renderable manager.
	if (renderable != nullptr)
	{
		removeEntityRenderable(entityToRemove);	
	}
	
	if (deleteEntity == true && runDestroy == true)
	{
		// if a room is unloading dont destroy
		entityCodeBehind->destroyed();
	}

	// Remove the entity from the master entities list.
	masterEntityList_.erase(masterEntityList_.begin() + index);
	masterEntityIdList_.erase(masterEntityIdList_.begin() + index);
	
	// Remove the entity from the nameID map and the name validator.
	entityNameIdMap_.erase(entityName);
	
	if (deleteEntity == true)
	{
		entityNameValidator_->removeName(entityName);

		// shared pointer will automatically be removed if the entity was not extracted.
		//delete entityToRemove;

		entityToRemove->cleanup();
	}
	else
	{
		// If the entity is being deleted, these signal disconnections happen in the cleanup function.
		entityToRemove->changeNameSignal_->disconnect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
		entityToRemove->entityChangingRoomSignal_->disconnect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
		entityToRemove->removeEntitySignal_->disconnect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));
	}

}

void Room::removeEntityRenderable(boost::shared_ptr<Entity> entity)
{
	int renderableCount = entity->components_->getRenderableCount();

	for (int i = 0; i < renderableCount; i++)
	{
		int renderableId = entity->components_->getRenderable(i)->getRenderableId();

		renderableManager_->removeRenderable(roomMetadata_->myIndex_, renderableId);
	}
}

void Room::removeTileEntity(int index)
{
	// Removes an entity in the master list at the given index, and unregisters it from subsystems.

	boost::shared_ptr<Entity> entityToRemove = masterEntityListTiles_[index];

	EntityControllerPtr controller = entityToRemove->getComponents()->getEntityController();
	boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entityToRemove->getComponents()->getCodeBehindContainer();
	boost::shared_ptr<InputReceiverCodeBehind> inputReceiverCodeBehind = codeBehindContainer->getInputReceiverCodeBehind();
	boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();

	boost::shared_ptr<Renderable> renderable = entityToRemove->components_->getRenderable();

	int entityInstanceId = entityToRemove->getComponents()->getEntityMetadata()->entityInstanceId_;

	EntityTypeId entityId = entityToRemove->getComponents()->getEntityMetadata()->getEntityTypeId();
	std::string entityName = entityToRemove->getComponents()->getEntityMetadata()->name_;

	// The staticEntityIDList vector should always be sorted, as it always only inserts increasing entity IDs at the end.
	// Find the lower bound. If the element found matches the value we are looking for,
	// remove it and reindex everything higher.
	std::vector<int>::iterator itr;

	itr = std::lower_bound(staticEntityIdList_.begin(), staticEntityIdList_.end(), index);

	int lowerBoundPosition = itr - staticEntityIdList_.begin();

	if (staticEntityIdList_[lowerBoundPosition] == index)
	{
		// Maintain paralellism.
		staticEntityIdList_.erase(staticEntityIdList_.begin() + lowerBoundPosition);
		staticEntityList_.erase(staticEntityList_.begin() + lowerBoundPosition);
	}

	// Also remove from the entity type map.
	std::vector<int> entityIdList = entityTypeIdMap_[entityId];
	std::vector<boost::shared_ptr<Entity>> entityList = entityTypeMap_.map_[entityId].entityList_;

	itr = std::lower_bound(entityIdList.begin(), entityIdList.end(), entityInstanceId);

	lowerBoundPosition = itr - entityIdList.begin();

	int size = entityIdList.size();

	if (lowerBoundPosition < size)
	{
		// If the index was found exactly, remove it.
		if (entityIdList[lowerBoundPosition] == entityInstanceId)
		{
			entityIdList.erase(entityIdList.begin() + lowerBoundPosition);
			entityList.erase(entityList.begin() + lowerBoundPosition);
		}
	}

	// Remove the entity's hitbox container from the physics manager.
	int layerIndex = entityToRemove->getComponents()->getEntityMetadata()->layerIndex_;

	if (layerIndex == renderableManager_->getInteractiveLayer(roomMetadata_->myIndex_))
	{
		physicsManager_->removeEntityComponents(roomMetadata_->myIndex_, entityInstanceId);
	}

	// Remove the entity's renderable from the renderable manager.
	if (renderable != nullptr)
	{
		int renderableId = entityToRemove->components_->getRenderable()->getRenderableId();

		renderableManager_->removeRenderable(roomMetadata_->myIndex_, renderableId);
	}

	// Remove the entity from the master entities list.
	masterEntityListTiles_.erase(masterEntityListTiles_.begin() + index);

	// Remove the entity from the nameID map and the name validator.
	entityNameIdMap_.erase(entityName);

	entityNameValidator_->removeName(entityName);

	entityToRemove->cleanup();
}

AudioSourcePtr Room::createAudioSourcePy(AudioSourcePropertiesPtr audioSourceProperties)
{
	PythonReleaseGil unlocker;

	return createAudioSource(audioSourceProperties);
}

AudioSourcePtr Room::createAudioSource(AudioSourcePropertiesPtr audioSourceProperties)
{
	std::string audioSourceName = audioSourceProperties->getName();

	if (audioSourceNameValidator_->isNameInUse(audioSourceName) == false)
	{
		audioSourceNameValidator_->addName(audioSourceName);

		int audioAssetIndex = audioPlayer_->translateAudioId(audioSourceProperties->getAudioId());

		int x = audioSourceProperties->getX();
		int y = audioSourceProperties->getY();
		int layerIndex = audioSourceProperties->getLayer();

		bool autoplay = audioSourceProperties->getAutoplay();
		bool loop = audioSourceProperties->getLoop();
		float maxDistance = audioSourceProperties->getMaxDistance();
		float minDistance = audioSourceProperties->getMinDistance();
		float volume = audioSourceProperties->getVolume();

		boost::shared_ptr<AudioSource> newAudioSource = boost::shared_ptr<AudioSource>(new AudioSource(audioSourceName, x, y));

		newAudioSource->audioPlayer_ = audioPlayer_;
		newAudioSource->audioIndex_ = audioAssetIndex;
		newAudioSource->roomIndex_ = roomMetadata_->myIndex_;
		newAudioSource->roomId_ = roomMetadata_->roomId_;
		newAudioSource->layerIndex_ = layerIndex;
		newAudioSource->loop_ = loop;
		newAudioSource->autoplay_ = autoplay;
		newAudioSource->minDistance_ = minDistance;
		newAudioSource->maxDistance_ = maxDistance;
		newAudioSource->volume_ = volume;

		audioSourceContainer_->addAudioSource(roomMetadata_->myIndex_, newAudioSource);

		return newAudioSource;
	}
	else
	{
		std::cout << "Audio Source not created because name \"" << audioSourceName << "\" is already in use." << std::endl;

		return nullptr;
	}
}

int Room::createCameraPy(bool isActive)
{
	PythonReleaseGil unlocker;

	return createCamera(isActive);
}

int Room::createCamera(bool isActive)
{
	int tileSize = assets_->getTileSize();

	// Create a camera entity to attach to this entity.
	boost::shared_ptr<Entity> camera = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
	boost::shared_ptr<CameraController> cameraController = boost::static_pointer_cast<CameraController>(camera->getComponents()->createCameraController());
	camera->getComponents()->initialize();

	camera->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
	camera->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
	camera->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));

	cameraController->attachDynamicsController();

	boost::shared_ptr<EntityMetadata> cameraMetadata = camera->getComponents()->getEntityMetadata();

	cameraMetadata->name_ = "Camera_" + boost::lexical_cast<std::string>(cameraMetadata->entityInstanceId_);
	cameraMetadata->roomMetadata_ = roomMetadata_;
	cameraMetadata->previousRoomMetadata_ = roomMetadata_;
	cameraMetadata->setEntityTypeId(ids_->ENTITY_CAMERA);
	cameraMetadata->classification_ = ENTITY_CLASSIFICATION_CAMERA;
	
	boost::shared_ptr<CodeBehindContainer> codeBehindContainer = camera->getComponents()->getCodeBehindContainer();

	codeBehindContainer->getPythonInstanceWrapper()->setScriptName("_camera");
	codeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName("Camera");
	codeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(cameraMetadata->name_);

	codeBehindContainer->controller_ = cameraController;
	codeBehindContainer->hitboxManager_ = hitboxManager_;
	codeBehindContainer->ids_ = ids_;
	codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
	codeBehindContainer->tileSize_ = tileSize;
	codeBehindContainer->timer_ = timer_;
	codeBehindContainer->renderer_ = renderer_;
	codeBehindContainer->ui_ = ui_;

	codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

	codeBehindContainer->setIsSimulatable();

	codeBehindContainer->createCodeBehinds(ids_->ENTITY_CAMERA, ENTITY_CLASSIFICATION_CAMERA);
	codeBehindContainer->initialize();


	boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();
	
	entityCodeBehind->initializeBegin();

	int cameraHeight = assets_->getCameraHeight();
	int cameraWidth = assets_->getCameraWidth();

	// Need to call the functions so it sets the midpoint.
	cameraController->setCameraHeight(cameraHeight);
	cameraController->setCameraWidth(cameraWidth);

	camera->initialize();

	cameraController->updateBounds(getMetadata());

	addEntity(camera, true);

	codeBehindContainer->getSimulatableCodeBehind()->start();

	boost::shared_ptr<CameraController> activeCamera = cameraManager_->getActiveCamera();

	if (activeCamera == nullptr || isActive == true)
	{
		cameraManager_->setActiveCamera(cameraController);
	}

	renderableManager_->initialize(roomMetadata_->myIndex_);

	int entityInstanceId = cameraMetadata->getEntityInstanceId();

	return entityInstanceId;
}

int Room::createEntityPy(EntityCreationParameters params)
{
	PythonReleaseGil unlocker;

	int instanceId = createEntity(params);

	return instanceId;
}

int Room::createEntity(EntityCreationParameters params)
{
	EntityTypeId entityId = params.getEntityTypeId();

	// Don't allow creation of special entities, camera and tile.
	if (entityId != ids_->ENTITY_TILE && entityId != ids_->ENTITY_CAMERA)
	{
		std::string entityName = params.getEntityName();

		// If the room is loading, entities cannot be created in it yet.
		if (isLoading_ == true && roomMetadata_->percentLoaded_ < 100)
		{
			std::cout << "Failed to create entity " << entityName << ", because the room has not finished loading. Adding it to a list of entities to be created upon room load." << std::endl;

			entityWaitingList_.push_back(params);
			
			return -1;
		}

		// If no entity name was given, the user presumably doesn't care. Generate a guid and use that as the name.
		if (entityName == "")
		{
			boost::uuids::uuid uuid = boost::uuids::random_generator()();

			// Names in python can't begin with a number. prefix it with an underscore.
			entityName = "_" + boost::lexical_cast<std::string>(uuid);

			std::replace(entityName.begin(), entityName.end(), '-', '_');
		}


		if (entityNameValidator_->isNameInUse(entityName) == true)		
		{
			std::cout<<"Entity Type ID "<<entityId<<" could not be created. The name "<<entityName<<" is already in use."<<std::endl;

			return -1;
		}
		else
		{
			entityNameValidator_->addName(entityName);

			boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(entityId);

			boost::shared_ptr<Entity> newEntity = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));

			int tileSize = assets_->getTileSize();

			EntityControllerPtr controller = nullptr;

			EntityComponentsPtr components = newEntity->getComponents();

			switch (entityTemplate->getClassification())
			{
			case ENTITY_CLASSIFICATION_ACTOR:

				controller = components->createActorController(renderer_, anchorPointManager_, animationManager_);

				break;

			case ENTITY_CLASSIFICATION_EVENT:

				controller = components->createEventController();

				break;

			case ENTITY_CLASSIFICATION_HUDELEMENT:

				controller = components->createHudElementController(renderer_, anchorPointManager_, animationManager_);

				break;

			case ENTITY_CLASSIFICATION_PARTICLEEMITTER:

				// particle emitters are a special case.
				//controller = components->createParticleEmitterController();

				break;
			}

			components->initialize();

			boost::shared_ptr<EntityMetadata> metadata = components->getEntityMetadata();
			metadata->setEntityTypeId(entityId);
			metadata->roomMetadata_ = roomMetadata_;
			metadata->previousRoomMetadata_ = roomMetadata_;
			metadata->classification_ = entityTemplate->getClassification();

			boost::shared_ptr<CodeBehindContainer> codeBehindContainer = components->getCodeBehindContainer();

			controller->anchorPointManager_ = anchorPointManager_;
			controller->hitboxManager_ = hitboxManager_;
			controller->timer_ = timer_;
			controller->audioPlayer_ = audioPlayer_;
			controller->audioSourceContainer_ = audioSourceContainer_;
			controller->textManager_ = textManager_;
			controller->fontManager_ = fontManager_;
			controller->engineConfig_ = engineConfig_;
			controller->debugger_ = debugger_;
			controller->engineController_ = engineController_;
			controller->messenger_ = messenger_;
			controller->queryManager_ = queryManager_;
			controller->roomMetadataContainer_ = roomMetadataContainer_;

			codeBehindContainer->getPythonInstanceWrapper()->setScriptName(entityTemplate->getScriptName());
			codeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName(entityTemplate->getScriptTypeName());
			codeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(entityName);

			codeBehindContainer->controller_ = controller;
			codeBehindContainer->hitboxManager_ = hitboxManager_;
			codeBehindContainer->ids_ = ids_;
			codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
			codeBehindContainer->tileSize_ = tileSize;
			codeBehindContainer->timer_ = timer_;
			codeBehindContainer->renderer_ = renderer_;
			codeBehindContainer->ui_ = ui_;

			codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

			StateMachineControllerPtr stateMachineController;
			StageControllerPtr stageController;
			StageMetadataPtr stageMetadata;
			StagePtr stage;

			int renderableCount = 0;

			switch (metadata->classification_)
			{
			case ENTITY_CLASSIFICATION_ACTOR:

				stateMachineController = components->getStateMachineController();

				stageController = components->getStageController();

				renderableCount = components->getRenderableCount();
	
				stage = stageController->getStage();

				stageMetadata = stage->getMetadata();

				stage->ownerMetadata_ = metadata;

				codeBehindContainer->stateMachineController_ = stateMachineController;
				codeBehindContainer->stageController_ = stageController;
				codeBehindContainer->setIsCollidable();
				codeBehindContainer->setIsInputReceiver();
				codeBehindContainer->setIsMessageable();
				codeBehindContainer->setIsRenderable();
				codeBehindContainer->setIsSimulatable();
				codeBehindContainer->setIsStateMachine();

				break;

			case ENTITY_CLASSIFICATION_EVENT:

				codeBehindContainer->setIsCollidable();
				codeBehindContainer->setIsInputReceiver();
				codeBehindContainer->setIsMessageable();

				break;

			case ENTITY_CLASSIFICATION_HUDELEMENT:

				stateMachineController = components->getStateMachineController();

				stageController = components->getStageController();

				renderableCount = components->getRenderableCount();

				stage = stageController->getStage();

				stageMetadata = stage->getMetadata();

				stage->ownerMetadata_ = metadata;

				codeBehindContainer->stateMachineController_ = stateMachineController;
				codeBehindContainer->stageController_ = stageController;
				codeBehindContainer->setIsInputReceiver();
				codeBehindContainer->setIsMessageable();
				codeBehindContainer->setIsRenderable();
				codeBehindContainer->setIsStateMachine();

				break;

			case ENTITY_CLASSIFICATION_PARTICLEEMITTER:

				//codeBehindContainer->setIsInputReceiver();
				//codeBehindContainer->setIsMessageable();
				codeBehindContainer->setIsSimulatable();

				break;
			}

			codeBehindContainer->createCodeBehinds(entityId, metadata->classification_);
			codeBehindContainer->initialize();

			boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();
			boost::shared_ptr<InputReceiverCodeBehind> inputReceiverCodeBehind = codeBehindContainer->getInputReceiverCodeBehind();

			entityCodeBehind->initializeBegin();

			newEntity->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
			newEntity->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
			newEntity->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));

			InputChannel inputChannel = params.getInputChannel();

			if (inputChannel > -1 && inputReceiverCodeBehind != nullptr)
			{
				//std::cout << "Setting input channel = " << inputChannel << std::endl;
				inputReceiverCodeBehind->setInputChannel(inputChannel);
			}

			newEntity->validate();
			
			metadata->name_ = entityName;

			entityNameIdMap_[entityName] = metadata->entityInstanceId_;

			newEntity->transitionId_ = ids_->TRANSITION_NULL;

			if (inputReceiverCodeBehind != nullptr)
			{
				if (params.getAcceptInput() == true)
				{
					newEntity->attachInputDeviceManager(inputDeviceManager_);
					inputReceiverCodeBehind->setInputChannel(params.getInputChannel());
				}
				else
				{
					// Don't use any input device;
					inputReceiverCodeBehind->setInputChannel(-1);
				}
			}

			// Load the sprite instance properties.
			stringmap::iterator itr;
			stringmap map = params.properties_;

			for(itr = map.begin(); itr != map.end(); itr++) 
			{
				std::string propertyName = (*itr).first;
				std::string propertyValue = (*itr).second;

				codeBehindContainer->getEntityCodeBehind()->entityScript_->entityProperties_[propertyName] = propertyValue;
			}
			
			if (stage != nullptr)
			{
				// Instead of setting entityTemplate, copy states and animations over to the entity.	
				StageMetadataPtr stageMetadataFromTemplate = entityTemplate->getStageMetadata();

				stageMetadata->setHeight(stageMetadataFromTemplate->getHeight());
				stageMetadata->setWidth(stageMetadataFromTemplate->getWidth());
				stageMetadata->setOrigin(stageMetadataFromTemplate->getOrigin());
				stageMetadata->setBackgroundDepth(stageMetadataFromTemplate->getBackgroundDepth());

				stageMetadata->getRotationOperation()->getPivotPoint()->setX(stageMetadataFromTemplate->getRotationOperation()->getPivotPoint()->getX());
				stageMetadata->getRotationOperation()->getPivotPoint()->setY(stageMetadataFromTemplate->getRotationOperation()->getPivotPoint()->getY());

				stageMetadata->getPosition()->setX(stageMetadataFromTemplate->getPosition()->getX());
				stageMetadata->getPosition()->setY(stageMetadataFromTemplate->getPosition()->getY());

				metadata->height_ = entityTemplate->getStageMetadata()->getHeight();
				metadata->width_ = entityTemplate->getStageMetadata()->getWidth();				
			}
			else
			{
				// Events can have any width or height. The user is able to specify them.
				metadata->width_ = params.getW();

				metadata->height_ = params.getH();
			}

			// Position the entity.
			if (params.getSpawnPointId() != ids_->SPAWNPOINT_NULL)
			{
				// Find the spawn point with the given ID.
				int spawnPointX = 0;
				int spawnPointY = 0;
				int spawnPointLayerIndex = 0;

				int size = spawnPoints_.size();

				for (int i = 0; i < size; i++)
				{
					if (spawnPoints_[i]->getId() == params.getSpawnPointId())
					{
						spawnPointX = spawnPoints_[i]->getX();
						spawnPointY = spawnPoints_[i]->getY();
						spawnPointLayerIndex = spawnPoints_[i]->getLayer();
						break;
					}
				}

				controller->setPosition(spawnPointX, spawnPointY);
				controller->getPosition()->setPreviousX(spawnPointX);
				controller->getPosition()->setPreviousY(spawnPointY);

				// Set the position in the dynamics controller, and have it set the previous position as well.
				controller->getDynamicsController()->setPositionXInternal(spawnPointX, true, false);
				controller->getDynamicsController()->setPositionYInternal(spawnPointY, true, false);

				metadata->layerIndex_ = spawnPointLayerIndex;
			}
			else
			{
				int x = params.getX();

				int y = params.getY();

				controller->setPosition(x, y);

				// Set the position in the dynamics controller, and have it set the previous position as well.
				controller->getDynamicsController()->setPositionXInternal(x, true, false);
				controller->getDynamicsController()->setPositionYInternal(y, true, false);

				controller->getPosition()->setPreviousX(x);
				controller->getPosition()->setPreviousY(y);
				
				int layerIndex = params.getLayer();

				if (layerIndex < 0)
				{
					// If the layer is not set, assume it's the interactive layer.
					metadata->layerIndex_ = renderableManager_->getInteractiveLayer(roomMetadata_->myIndex_);				
				}
				else
				{				
					metadata->layerIndex_ = layerIndex;
				}
			}
						
			boost::shared_ptr<HitboxController> hc = newEntity->components_->getHitboxController();

			if (metadata->classification_ == ENTITY_CLASSIFICATION_ACTOR ||
				metadata->classification_ == ENTITY_CLASSIFICATION_CAMERA ||
				metadata->classification_ == ENTITY_CLASSIFICATION_PARTICLE ||
				metadata->classification_ == ENTITY_CLASSIFICATION_PARTICLEEMITTER)
			{
				controller->attachDynamicsController();
			}
			
			for (int i = 0; i < renderableCount; i++)
			{
				StageRenderablePtr stageRenderable;

				stageRenderable = boost::static_pointer_cast<StageRenderable>(components->getRenderable(i));

				stageRenderable->setMapLayer(metadata->layerIndex_);

				if (stageRenderable->isBackground_ == true)
				{
					stageRenderable->setRenderOrder(params.getRenderOrder() - stage->metadata_->getBackgroundDepth());
				}
				else
				{
					stageRenderable->setRenderOrder(params.getRenderOrder());
				}
			}

			hc->setStageHeight(entityTemplate->getStageMetadata()->getHeight());
			hc->setStageWidth(entityTemplate->getStageMetadata()->getWidth());

			int size = entityTemplate->getStageElementsCount();

			for (int m = 0; m < size; m++)
			{
				StageElementsPtr stageElementsFromTemplate = entityTemplate->getStageElements(m);

				std::string stateName = entityTemplate->getStateNameFromIndex(m);

				std::string typeName = codeBehindContainer->getPythonInstanceWrapper()->getInstanceTypeName();
				
				StageElementsPtr newStageElements = StageElementsPtr(new StageElements(stateName));

				StateMachineStatePtr newState = StateMachineStatePtr(new StateMachineState(stateName));
				
				newStageElements->stageMetadata_ = stageMetadata;
				
				newStageElements->ownerMetadata_ = metadata;

				newStageElements->animationManager_ = animationManager_;

				newStageElements->anchorPointManager_ = anchorPointManager_;

				newStageElements->debugger_ = debugger_;
				
				newStageElements->renderer_ = renderer_;

				newStageElements->hitboxManager_ = hitboxManager_;

				newStageElements->ownerMetadata_ = metadata;

				// Copy state data from the current entityTemplate state to the new state.
				// Get the hitboxes in this state, and make copies of them. This is so they can be resized, or
				// otherwise changed, on a per instance basis.
				int hitboxReferenceCount = stageElementsFromTemplate->getHitboxReferenceCount();

				for (int k = 0; k < hitboxReferenceCount; k++)
				{
					int hitboxId = stageElementsFromTemplate->getHitboxReference(k);

					// Make a copy of the hitbox.
					boost::shared_ptr<Hitbox> sourceHitbox = hitboxManager_->getHitbox(hitboxId);

					boost::shared_ptr<Hitbox> newHitbox = boost::shared_ptr<Hitbox>(new Hitbox(sourceHitbox->getLeft(), sourceHitbox->getTop(), sourceHitbox->getHeight(), sourceHitbox->getWidth()));

					newHitbox->setIdentity(sourceHitbox->getIdentity());

					newHitbox->setIsSolid(sourceHitbox->getIsSolid());

					newHitbox->setBaseRotationDegrees(sourceHitbox->getBaseRotationDegrees());

					int newHitboxId = hitboxManager_->addHitbox(newHitbox);

					newStageElements->addHitboxReference(newHitboxId);

					newHitbox->stageMetadata_ = stageMetadata;
				}
				
				int animationSlotCount = stageElementsFromTemplate->getAnimationSlotCount();
								
				bool singleFrame = newStageElements->getSingleFrame();

				newStageElements->setSingleFrame(singleFrame);

				for (int k = 0; k < animationSlotCount; k++)
				{
					std::string slotName = stageElementsFromTemplate->getAnimationSlotName(k);

					int animationId = stageElementsFromTemplate->getAnimationIdByIndex(k);

					int framesPerSecond = stageElementsFromTemplate->getAnimationSlotFramesPerSecondByIndex(k);

					bool background = stageElementsFromTemplate->getAnimationSlotBackgroundByIndex(k);

					int positionX = stageElementsFromTemplate->getNativeAnimationSlotPositionXByIndex(k);

					int positionY = stageElementsFromTemplate->getNativeAnimationSlotPositionYByIndex(k);

					ColorRgbaPtr hueColor = stageElementsFromTemplate->getAnimationSlotHueColorByIndex(k);

					float hueRed = hueColor->getR();
					float hueGreen = hueColor->getG();
					float hueBlue = hueColor->getB();
					float hueAlpha = hueColor->getA();

					ColorRgbaPtr blendColor = stageElementsFromTemplate->getAnimationSlotBlendColorByIndex(k);

					float blendRed = blendColor->getR();
					float blendGreen = blendColor->getG();
					float blendBlue = blendColor->getB();
					float blendAlpha = blendColor->getA();

					float blendPercent = stageElementsFromTemplate->getAnimationSlotBlendPercentByIndex(k);

					ColorRgbaPtr outlineColor = stageElementsFromTemplate->getAnimationSlotOutlineColorByIndex(k);

					float outlineRed = outlineColor->getR();
					float outlineGreen = outlineColor->getG();
					float outlineBlue = outlineColor->getB();
					float outlineAlpha = outlineColor->getA();

					float rotation = stageElementsFromTemplate->getAnimationSlotRotationByIndex(k);

					int pivotX = stageElementsFromTemplate->getAnimationSlotPivotPointXByIndex(k);

					int pivotY = stageElementsFromTemplate->getAnimationSlotPivotPointYByIndex(k);

					float alphaGradientFrom = stageElementsFromTemplate->getAnimationSlotAlphaGradientFromByIndex(k);

					float alphaGradientTo = stageElementsFromTemplate->getAnimationSlotAlphaGradientToByIndex(k);

					int alphaGradientRadialCenterX = stageElementsFromTemplate->getAnimationSlotAlphaGradientRadialCenterXByIndex(k);

					int alphaGradientRadialCenterY = stageElementsFromTemplate->getAnimationSlotAlphaGradientRadialCenterYByIndex(k);

					float alphaGradientRadius = stageElementsFromTemplate->getAnimationSlotAlphaGradientRadiusByIndex(k);

					AlphaGradientDirection alphaGradientDirection = stageElementsFromTemplate->getAnimationSlotAlphaGradientDirectionByIndex(k);
					
					AnimationSlotOrigin animationSlotOrigin = stageElementsFromTemplate->getAnimationSlotOriginByIndex(k);

					std::string animationSlotNextStateName = stageElementsFromTemplate->getAnimationSlotNextStateNameByIndex(k);

					AnimationStyle animationSlotAnimationStyle = stageElementsFromTemplate->getAnimationSlotAnimationStyleByIndex(k);
					
					newStageElements->addAnimationSlotInternal(slotName,
															   positionX, positionY, 
															   hueColor, blendColor, blendPercent,
															   rotation, 
						                                       framesPerSecond,
															   pivotX, pivotY, 
															   animationSlotOrigin,
						                                       animationSlotNextStateName,
						                                       animationSlotAnimationStyle,
															   outlineColor,
															   background);

					newStageElements->assignAnimationByIdToSlotByIndexInternal(k, animationId, true);
				}

				stageController->addExistingStageElements(newStageElements);

				stateMachineController->addExistingState(newState);
			}
							
			if (metadata->classification_ == ENTITY_CLASSIFICATION_EVENT)
			{
				boost::shared_ptr<Hitbox> newHitbox = boost::shared_ptr<Hitbox>(new Hitbox(params.getX(), params.getY(),  params.getH(), params.getW()));
				newHitbox->setEdgeFlags(0xFF); // Don't need to use edgeflags for non-tile entities.
				newHitbox->setIdentity(ids_->HITBOX_EVENT);

				int hitboxId = hitboxManager_->addHitbox(newHitbox);

				std::string typeName = codeBehindContainer->getPythonInstanceWrapper()->getInstanceTypeName();
				
				// Events don't have a stage. Is this necessary? findmetodo
				StageElementsPtr newEventStageElements = StageElementsPtr(new StageElements("Event"));

				newEventStageElements->ownerMetadata_ = metadata;

				newEventStageElements->renderer_ = renderer_;

				newEventStageElements->hitboxManager_ = hitboxManager_;

				newEventStageElements->animationManager_ = animationManager_;

				newEventStageElements->anchorPointManager_ = anchorPointManager_;

				newEventStageElements->debugger_ = debugger_;

				newEventStageElements->ownerMetadata_ = metadata;

				newEventStageElements->addHitboxReference(hitboxId);

				stageController->addExistingStageElements(newEventStageElements);

				stateMachineController->setStateByIndex(0);

				hc->activateHitbox(hitboxId);
			}
			else
			{
				int initialStateIndex = entityTemplate->getInitialStateIndex();
				
				if (params.getInitialStateName() == "")
				{
					stateMachineController->setStateByIndex(initialStateIndex);
				}
				else
				{
					stateMachineController->setStateByNameInternal(params.getInitialStateName(), true);
				}
			}
			
			addEntity(newEntity, true);

			newEntity->initialize();

			if (codeBehindContainer->getIsSimulatable() == true)
			{
				codeBehindContainer->getSimulatableCodeBehind()->start();
			}

			if (newEntity->components_->getDynamicsController() != nullptr)
			{
				boost::shared_ptr<EntityTemplate> entityTemplate = assets_->getEntityTemplate(metadata->getEntityTypeId());
			
				DynamicsController* dc = newEntity->components_->getDynamicsController();
			
				if (dc != nullptr)
				{
					dc->setOwnerStageHeight(entityTemplate->getStageMetadata()->getHeight());

					dc->setOwnerStageWidth(entityTemplate->getStageMetadata()->getWidth());

					// Need to know the layer position for visual debug rendering.
					boost::shared_ptr<Position> p = renderableManager_->getLayerPosition(roomMetadata_->myIndex_, metadata->layerIndex_);

					dc->layerPosition_ = p;
				}
			}
			
			resetAudioListener();

			entityCodeBehind->created();

			entityCodeBehind->preRoomEntered(roomMetadata_->roomId_);

			entityCreated(newEntity);

			entityEntered(newEntity);

			int entityInstanceId = metadata->getEntityInstanceId();
		
			return entityInstanceId;
		}
	}
	else
	{
		std::cout<<"Entity Type ID "<<entityId<<" could not be created."<<std::endl;
		return -1;
	}
}

int Room::createParticleEmitterPy(ParticleEmitterCreationParameters params)
{
	PythonReleaseGil unlocker;

	int instanceId = createParticleEmitter(params);

	return instanceId;
}

int Room::createParticleEmitter(ParticleEmitterCreationParameters params)
{
	std::string name = params.getName();

	// If the room is loading, entities cannot be created in it yet.
	if (isLoading_ == true && roomMetadata_->percentLoaded_ < 100)
	{
		std::cout << "Failed to create particle emitter " << name << ", because the room has not finished loading" << std::endl;

		return -1;
	}

	ParticleEmitterId particleEmitterId = params.getParticleEmitterId();

	// If no entity name was given, the user presumably doesn't care. Generate a guid and use that as the name.
	if (name == "")
	{
		boost::uuids::uuid uuid = boost::uuids::random_generator()();

		// Names in python can't begin with a number. prefix it with an underscore.
		name = "_" + boost::lexical_cast<std::string>(uuid);
	}


	if (entityNameValidator_->isNameInUse(name) == true)
	{
		std::cout << "Particle Emitter Type ID " << particleEmitterId << " could not be created. The name " << name << " is already in use." << std::endl;

		return -1;
	}
	else
	{
		entityNameValidator_->addName(name);

		boost::shared_ptr<Entity> newParticleEmitter = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));
		EntityControllerPtr controller = newParticleEmitter->getComponents()->createParticleEmitterController();
		newParticleEmitter->getComponents()->initialize();

		int tileSize = assets_->getTileSize();

		int particlesPerEmission = params.getParticlesPerEmission();

		int maxParticles = params.getMaxParticles();

		int mapLayerIndex = params.getLayer();

		int renderOrder = params.getRenderOrder();

		double interval = params.getInterval();

		double particleLifespan = params.getParticleLifespan();

		bool automatic = params.getAutomatic();

		bool attachParticles = params.getAttachParticles();

		int particleEmitterX = params.getX();

		int particleEmitterY = params.getY();

		ParticleId particleTypeId = params.getParticleId();

		std::string animationName = params.getAnimationName();

		AssetId animationId = animationManager_->getAnimationId(animationName);

		int animationFramesPerSecond = params.getAnimationFramesPerSecond();

		boost::shared_ptr<EntityMetadata> metadata = newParticleEmitter->getComponents()->getEntityMetadata();
		boost::shared_ptr<ParticleEmitterController> particleEmitterController = boost::static_pointer_cast<ParticleEmitterController>(controller);
		boost::shared_ptr<CodeBehindContainer> codeBehindContainer = newParticleEmitter->getComponents()->getCodeBehindContainer();

		controller->renderer_ = renderer_;
		controller->animationManager_ = animationManager_;
		controller->anchorPointManager_ = anchorPointManager_;
		controller->hitboxManager_ = hitboxManager_;
		controller->timer_ = timer_;
		controller->audioPlayer_ = audioPlayer_;
		controller->audioSourceContainer_ = audioSourceContainer_;
		controller->textManager_ = textManager_;
		controller->fontManager_ = fontManager_;
		controller->engineConfig_ = engineConfig_;
		controller->debugger_ = debugger_;
		controller->engineController_ = engineController_;
		controller->messenger_ = messenger_;
		controller->queryManager_ = queryManager_;
		controller->roomMetadataContainer_ = roomMetadataContainer_;

		controller->attachDynamicsController();

		particleEmitterController->renderer_ = renderer_;

		metadata->setEntityTypeId(particleEmitterId);
		metadata->roomMetadata_ = roomMetadata_;
		metadata->previousRoomMetadata_ = roomMetadata_;
		metadata->classification_ = ENTITY_CLASSIFICATION_PARTICLEEMITTER;
		metadata->layerIndex_ = mapLayerIndex;

		codeBehindContainer->getPythonInstanceWrapper()->setScriptName(BaseIds::idScriptDataMap[particleEmitterId].getFileName());
		codeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName(BaseIds::idScriptDataMap[particleEmitterId].getScriptTypeName());
		codeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(name);

		codeBehindContainer->controller_ = controller;
		codeBehindContainer->hitboxManager_ = hitboxManager_;
		codeBehindContainer->ids_ = ids_;
		codeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
		codeBehindContainer->tileSize_ = tileSize;
		codeBehindContainer->timer_ = timer_;
		codeBehindContainer->renderer_ = renderer_;
		codeBehindContainer->ui_ = ui_;

		codeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

		codeBehindContainer->setIsSimulatable();

		codeBehindContainer->createCodeBehinds(particleEmitterId, ENTITY_CLASSIFICATION_PARTICLEEMITTER);
		codeBehindContainer->initialize();

		boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();
		boost::shared_ptr<ParticleEmitterEntityCodeBehind> particleEmitterCodeBehind = boost::static_pointer_cast<ParticleEmitterEntityCodeBehind>(codeBehindContainer->getEntityCodeBehind());

		particleEmitterCodeBehind->particleTypeId_ = particleTypeId;

		particleEmitterCodeBehind->interval_ = interval;
		particleEmitterCodeBehind->updateTimer_ = interval;
		particleEmitterCodeBehind->particlesPerEmission_ = particlesPerEmission;
		particleEmitterCodeBehind->particleLifespan_ = particleLifespan;
		particleEmitterCodeBehind->maxParticles_ = maxParticles;

		particleEmitterCodeBehind->particleAnimationId_ = animationId;

		particleEmitterCodeBehind->automatic_ = automatic;
		particleEmitterCodeBehind->attachParticles_ = attachParticles;

		entityCodeBehind->initializeBegin();

		newParticleEmitter->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
		newParticleEmitter->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
		newParticleEmitter->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));

		metadata->name_ = name;

		entityNameIdMap_[name] = metadata->entityInstanceId_;

		controller->setPosition(particleEmitterX, particleEmitterY);
		controller->getPosition()->setPreviousX(particleEmitterX);
		controller->getPosition()->setPreviousY(particleEmitterY);

		// Set the position in the dynamics controller, and have it set the previous position as well.
		controller->getDynamicsController()->setPositionXInternal(particleEmitterX, true, false);
		controller->getDynamicsController()->setPositionYInternal(particleEmitterY, true, false);

		std::vector<boost::shared_ptr<Entity>> particlesToAdd;

		// Create the particle entities this emitter will contain.
		for (int m = 0; m < maxParticles; m++)
		{
			boost::shared_ptr<Entity> newParticle = boost::shared_ptr<Entity>(new Entity(ids_, debugger_, physicsConfig_, hitboxManager_));

			// Generate a UUID to use for the particle python variable name.
			boost::uuids::uuid uuid = boost::uuids::random_generator()();

			// Names in python can't begin with a number. prefix it with an underscore.
			std::string particleName = "_" + boost::lexical_cast<std::string>(uuid);

			// The controller must be created first, as it is used to create some of the other components.
			EntityControllerPtr controller2 = newParticle->getComponents()->createParticleController(renderer_);
			newParticle->getComponents()->initialize();

			controller2->animationManager_ = animationManager_;

			boost::shared_ptr<ParticleController> particleController = boost::static_pointer_cast<ParticleController>(controller2);
			newParticle->getComponents()->initialize();

			boost::shared_ptr<ParticleMetadata> particleMetadata = boost::static_pointer_cast<ParticleMetadata>(newParticle->getComponents()->getEntityMetadata());
			boost::shared_ptr<CodeBehindContainer> particleCodeBehindContainer = newParticle->getComponents()->getCodeBehindContainer();

			particleController->renderer_ = renderer_;

			particleController->attachDynamicsController();

			//newParticle->attachRenderable();

			boost::shared_ptr<ParticleRenderable> particleRenderable = boost::static_pointer_cast<ParticleRenderable>(particleController->getRenderable());

			particleMetadata->setEntityTypeId(particleTypeId);
			particleMetadata->roomMetadata_ = roomMetadata_;
			particleMetadata->previousRoomMetadata_ = roomMetadata_;
			particleMetadata->classification_ = ENTITY_CLASSIFICATION_PARTICLE;
			particleMetadata->layerIndex_ = mapLayerIndex;

			//particleRenderable->setMapLayer(mapLayerIndex);
			particleRenderable->setMetadata(particleMetadata);

			particleCodeBehindContainer->getPythonInstanceWrapper()->setScriptName(BaseIds::idScriptDataMap[particleTypeId].getFileName());
			particleCodeBehindContainer->getPythonInstanceWrapper()->setInstanceTypeName(BaseIds::idScriptDataMap[particleTypeId].getScriptTypeName());
			particleCodeBehindContainer->getPythonInstanceWrapper()->setInstanceVariableName(particleName);

			particleCodeBehindContainer->controller_ = particleController;
			particleCodeBehindContainer->hitboxManager_ = hitboxManager_;
			particleCodeBehindContainer->ids_ = ids_;
			particleCodeBehindContainer->inputDeviceManager_ = inputDeviceManager_;
			particleCodeBehindContainer->tileSize_ = tileSize;
			particleCodeBehindContainer->timer_ = timer_;
			particleCodeBehindContainer->renderer_ = renderer_;
			particleCodeBehindContainer->ui_ = ui_;

			particleCodeBehindContainer->codeBehindFactory_ = codeBehindFactory_;

			particleCodeBehindContainer->setIsSimulatable();
			particleCodeBehindContainer->setIsRenderable();

			particleCodeBehindContainer->createCodeBehinds(particleTypeId, ENTITY_CLASSIFICATION_PARTICLE);
			particleCodeBehindContainer->initialize();

			boost::shared_ptr<ParticleEntityCodeBehind> particleCodeBehind = boost::static_pointer_cast<ParticleEntityCodeBehind>(particleCodeBehindContainer->getEntityCodeBehind());

			particleMetadata->name_ = particleName;

			//particleCodeBehind->particleTypeId_ = particleTypeId;

			particleController->setPosition(particleEmitterX, particleEmitterY);

			// Set the position in the dynamics controller, and have it set the previous position as well.
			particleController->getDynamicsController()->setPositionXInternal(particleEmitterX, true, false);
			particleController->getDynamicsController()->setPositionYInternal(particleEmitterY, true, false);

			newParticle->changeNameSignal_->connect(boost::bind(&Room::entityChangingName, this, _1, _2, _3));
			newParticle->entityChangingRoomSignal_->connect(boost::bind(&Room::entityChangingRoom, this, _1, _2, _3, _4));
			newParticle->removeEntitySignal_->connect(boost::bind(&Room::entityRemoveEntity, this, _1, _2));

			//particleCodeBehind->isVisible_ = false;

			particleMetadata->lifetime_ = particleLifespan;

			particleRenderable->setRenderOrder(renderOrder);

			if (particleEmitterCodeBehind->particleAnimationId_ >= 0)
			{
				particleRenderable->setAnimation(animationManager_->getAnimationByIndex(particleEmitterCodeBehind->particleAnimationId_), animationFramesPerSecond);
			}

			particleEmitterController->particleHeight_ = particleRenderable->getHeight();
			particleEmitterController->particleWidth_ = particleRenderable->getWidth();

			particleEmitterCodeBehind->particles_.push_back(particleCodeBehind);

			// Need to add the particles AFTER the emitter.
			particlesToAdd.push_back(newParticle);
		}
		
		// The emitter code behind must be initialized after the particles it contains.
		codeBehindContainer->initialize();

		particleEmitterCodeBehind->initializeParticles();

		addEntity(newParticleEmitter, true);

		codeBehindContainer->getSimulatableCodeBehind()->start();

		newParticleEmitter->initialize();

		entityCodeBehind->created();

		entityCodeBehind->preRoomEntered(roomMetadata_->roomId_);

		entityCreated(newParticleEmitter);

		entityEntered(newParticleEmitter);

		int size = particlesToAdd.size();

		for (int i = 0; i < size; i++)
		{
			boost::shared_ptr<Entity> particle = particlesToAdd[i];

			addEntity(particle, true);


			particle->initialize();

			boost::shared_ptr<CodeBehindContainer> particleCodeBehindContainer = particle->getComponents()->getCodeBehindContainer();
			boost::shared_ptr<ParticleEntityCodeBehind> particleCodeBehind = boost::static_pointer_cast<ParticleEntityCodeBehind>(particleCodeBehindContainer->getEntityCodeBehind());

			particleCodeBehindContainer->getSimulatableCodeBehind()->start();

			particleCodeBehind->created();

			particleCodeBehind->preRoomEntered(roomMetadata_->roomId_);

			entityCreated(particle);

			entityEntered(particle);
		}

		int entityInstanceId = metadata->getEntityInstanceId();

		return entityInstanceId;
	}
}

//void Room::removeParticleEmitterByIdPy(int particleEmitterId)
//{
//	PythonReleaseGil unlocker;
//
//	removeParticleEmitterById(particleEmitterId);
//}
//
//void Room::removeParticleEmitterById(int particleEmitterId)
//{
//	if (particleEmitterId >= 0)
//	{
//		// Find and flag the particle emitter as removed.
//		boost::shared_ptr<ParticleEmitterController> temp = getParticleEmitterById(particleEmitterId);
//
//		if (temp != nullptr)
//		{
//			temp->remove();
//		}
//	}
//}

void Room::buttonDown(boost::shared_ptr<InputEvent> inputEvent)
{
	if (inputEvent->blockEntityInput_ == false)
	{
		if (isShowing_ == true)
		{
			buttonDownSignal_(inputEvent);
		}	
	}
}

void Room::buttonUp(boost::shared_ptr<InputEvent> inputEvent)
{
	if (inputEvent->blockEntityInput_ == false)
	{
		if (isShowing_ == true)
		{
			buttonUpSignal_(inputEvent);
		}
	}
}

boost::shared_ptr<RoomMetadata> Room::getMetadataPy()
{
	PythonReleaseGil unlocker;

	return getMetadata();
}

boost::shared_ptr<RoomMetadata> Room::getMetadata()
{
	return roomMetadata_;
}

int Room::entityChangingName(int entityInstanceId, std::string name, bool append)
{
	boost::shared_ptr<Entity> entityToBeChanged = getEntityById(entityInstanceId);

	boost::shared_ptr<EntityMetadata> metadata = entityToBeChanged->getComponents()->getEntityMetadata();

	std::string oldEntityName = metadata->getEntityInstanceName();

	std::string newName;

	if (append == true)
	{
		// Append the name value to the current name.
		newName = oldEntityName + name;
	}
	else
	{
		// Overwrite the current name with the name value.
		newName = name;
	}

	// If the name is already in use, return -1. Otherwise, change the entity name.
	if (entityNameValidator_->isNameInUse(newName))
	{
		std::cout << "Entity name could not be changed. The name " << name << " is already in use." << std::endl;

		return -1;
	}
	else
	{
		// Update the name validator.
		entityNameValidator_->removeName(oldEntityName);

		entityNameValidator_->addName(newName);

		// Change the python instance variable name to a temp value, so it can also be reused, and it will not cause any interference when deallocating it.	
		boost::shared_ptr<EntityComponents> components = entityToBeChanged->getComponents();

		boost::shared_ptr<CodeBehindContainer> codeBehindContainer = components->getCodeBehindContainer();

		codeBehindContainer->getPythonInstanceWrapper()->renameInstanceVariableName(newName);

		metadata->name_ = newName;

		entityNameIdMap_[newName] = metadata->entityInstanceId_;

		entityNameIdMap_.erase(oldEntityName);

		return 0;
	}
}

void Room::entityChangingRoom(int entityInstanceId, RoomId moveFromRoomId, ChangeRoomParameters changeRoomParams, ShowRoomParameters showRoomParams)
{
	MoveEntityParameters moveParams;
	moveParams.entityInstanceId = entityInstanceId;
	moveParams.moveFromRoomId = moveFromRoomId;
	moveParams.changeRoomParameters = changeRoomParams;
	moveParams.showRoomParameters = showRoomParams;

	(*moveEntitySignal_)(moveParams);
}

bool Room::entityRemoveEntity(int entityInstanceId, RoomId roomId)
{
	// This entity will be effecitevly considered deleted, so we need to change the entity name to some temporary value, so it can be reused by other entities.
	boost::shared_ptr<Entity> entityToBeRemoved = getEntityById(entityInstanceId);

	// Create a temp GUID to use as a name.
	boost::uuids::uuid uuid = boost::uuids::random_generator()();

	// Names in python can't begin with a number. prefix it with an underscore.
	std::string newEntityName = "_" + boost::lexical_cast<std::string>(uuid);

	std::replace(newEntityName.begin(), newEntityName.end(), '-', '_');

	boost::shared_ptr<EntityMetadata> metadata = entityToBeRemoved->getComponents()->getEntityMetadata();

	std::string oldEntityName = metadata->getEntityInstanceName();

	// Remove the name from the name validator, so the name can be reused.
	entityNameValidator_->removeName(oldEntityName);

	entityNameValidator_->addName(newEntityName);

	// Change the python instance variable name to a temp value, so it can also be reused, and it will not cause any interference when deallocating it.	
	boost::shared_ptr<EntityComponents> components = entityToBeRemoved->getComponents();

	boost::shared_ptr<CodeBehindContainer> codeBehindContainer = components->getCodeBehindContainer();

	codeBehindContainer->getPythonInstanceWrapper()->renameInstanceVariableName(newEntityName);

	metadata->name_ = newEntityName;

	entityNameIdMap_[newEntityName] = metadata->entityInstanceId_;

	entityNameIdMap_.erase(oldEntityName);

	return false;
}

void Room::processEntityMoveQueue()
{
	// Insert any entities which have been queued up.
	while (entityInsertQueue_.empty() == false)
	{
		EntityInsertQueueItem item = entityInsertQueue_.front();

		entityInsertQueue_.pop_front();

		insertEntity(item.entity, item.spawnPointId, item.offsetX, item.offsetY);
	}
}

void Room::beginTransition(double transitionLength)
{
	transitionTimeTotal_ = transitionLength;
	isTransitioning_ = true;
}

bool Room::getIsTransitioning()
{
	return isTransitioning_;
}

double Room::getTransitionTimer()
{
	return transitionTimer_;
}

void Room::playAudioSources()
{
	// Start all autoplay audio sources in this room.
	int size = audioSourceContainer_->audioSources_[roomMetadata_->myIndex_].size();

	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<AudioSource> audioSource = audioSourceContainer_->audioSources_[roomMetadata_->myIndex_][i];

		if (audioSource->autoplay_ == true)
		{
			audioSource->play();
		}
	}
}

void Room::setEntityPosition(boost::shared_ptr<Entity> entity, ChangeRoomParameters params)
{
	// Find the spawn point with the given ID.
	int spawnPointX = 0;
	int spawnPointY = 0;
	int spawnPointLayerIndex = 0;

	int size = spawnPoints_.size();

	for (int i = 0; i < size; i++)
	{
		if (spawnPoints_[i]->getId() == params.spawnPointId)
		{
			spawnPointX = spawnPoints_[i]->getX();
			spawnPointY = spawnPoints_[i]->getY();
			spawnPointLayerIndex = spawnPoints_[i]->getLayer();
			break;
		}
	}

	EntityControllerPtr controller = entity->getComponents()->getEntityController();
	boost::shared_ptr<CodeBehindContainer> codeBehindContainer = entity->getComponents()->getCodeBehindContainer();
	boost::shared_ptr<EntityCodeBehind> entityCodeBehind = codeBehindContainer->getEntityCodeBehind();
	boost::shared_ptr<EntityMetadata> metadata = entity->getComponents()->getEntityMetadata();

	entity->validate();

	int insertAtX = spawnPointX + params.offsetX;
	int insertAtY = spawnPointY + params.offsetY;

	// Before setting the position, fake the previous positions, otherwise it will try to do an attachment synch
	// using the position from the previous room the entity was in, if it was moved from a room, and
	// will be put in a super wrong position.
	
	controller->getPosition()->setPreviousX(insertAtX);
	controller->getPosition()->setPreviousY(insertAtY);

	// ...Actually, it turns out the way to fix this particular issue is to set the physics snapshot position,
	// but I'll leave the previous two lines how they are just in case.

	controller->getDynamicsController()->getPhysicsSnapshot()->getPosition()->setX(insertAtX);
	controller->getDynamicsController()->getPhysicsSnapshot()->getPosition()->setY(insertAtY);

	controller->setPosition(insertAtX, insertAtY);

	metadata->layerIndex_ = spawnPointLayerIndex;
}

void Room::resetAudioListener()
{
	if (isShowing_ == true)
	{
		//need to attach listener in editor?
		DynamicsController* dynamicsController = nullptr; // Camerafix attachedToCameraEntity_->getComponents()->getDynamicsController();
		
		if (dynamicsController != nullptr)
		{
			audioPlayer_->listenerPosition_ = dynamicsController->getPhysicsSnapshot()->getPosition();
			audioPlayer_->listenerPositionOffset_->setX((float)(dynamicsController->getOwnerStageWidth() / 2));
			audioPlayer_->listenerPositionOffset_->setY((float)(dynamicsController->getOwnerStageHeight() / 2));
		}
		
	}
}

void Room::createWaitingEntities()
{
	int size = entityWaitingList_.size();

	for (int i = 0; i < size; i++)
	{
		EntityCreationParameters params = entityWaitingList_[i];

		createEntity(params);
	}

	entityWaitingList_.clear();
}
//boost::shared_ptr<ParticleEmitter> Room::getParticleEmitterById(int particleEmitterId)
//{
//	// Find the particle emitter with the given ID in the ID list, and use that index to return the particle emitter in the master particle emitter list.
//	int size = particleEmitterContainer_->particleEmitters_[roomMetadata_->myIndex_].size();
//
//	for (int i = 0; i < size; i++)
//	{
//		int id = particleEmitterContainer_->particleEmitters_[roomMetadata_->myIndex_][i]->particleEmitterId_;
//
//		if (id == particleEmitterId)
//		{
//			return  particleEmitterContainer_->particleEmitters_[roomMetadata_->myIndex_][i];
//		}
//	}	
//	
//	return nullptr;
//}

//boost::shared_ptr<ParticleEmitter> Room::getParticleEmitterById(ParticleEmitterId particleEmitterType, int particleEmitterId)
//{
//	// Find the particle emitter with the given ID in the ID list, and use that index to return the particle emitter in the master particle emitter list.
//
//	int size = particleEmitterContainer_->particleEmitters_[roomMetadata_->myIndex_].size();
//
//	for (int i = 0; i < size; i++)
//	{
//		int id = particleEmitterContainer_->particleEmitters_[roomMetadata_->myIndex_][i]->particleEmitterId_;
//
//		if (id == particleEmitterId)
//		{
//			return  particleEmitterContainer_->particleEmitters_[roomMetadata_->myIndex_][i];
//		}
//	}
//
//	return nullptr;
//}
//
//boost::shared_ptr<ParticleEmitter> Room::getParticleEmitterByName(std::string particleEmitterInstanceName)
//{
//	// Map the given name to an ID and get it.
//
//	int particleEmitterId = particleEmitterContainer_->particleEmitterNameIdMap_[particleEmitterInstanceName];
//
//	return getParticleEmitterById(particleEmitterId);
//}
//
//boost::shared_ptr<ParticleEmitter> Room::getParticleEmitterByName(ParticleEmitterId particleEmitterType, std::string particleEmitterInstanceName)
//{
//	// Map the given name to an ID and get it.
//
//	auto it = particleEmitterContainer_->particleEmitterNameIdMap_.find(particleEmitterInstanceName);
//
//	//int charWidth = 0;
//	//int charHeight = 0;
//
//	if (it == particleEmitterContainer_->particleEmitterNameIdMap_.end())
//	{
//		std::cout << "Error: ParticleEmitter \"" + particleEmitterInstanceName + "\" was not found." << std::endl;
//
//		return nullptr;
//	}
//	else
//	{
//		int particleEmitterId = particleEmitterContainer_->particleEmitterNameIdMap_[particleEmitterInstanceName];
//
//		return getParticleEmitterById(particleEmitterType, particleEmitterId);
//	}
//}
//
//object Room::getParticleEmitterPyInstanceById(int particleEmitterId)
//{
//	// Find the particleEmitter with the given ID in the ID list, and use that index to return the particle emitter in the master particle emitter list.
//
//	//PythonAcquireGil lock;
//
//	std::vector<int>::iterator itr;
//
//	int size = particleEmitterContainer_->particleEmitterIds_[roomMetadata_->myIndex_].size();
//	if (particleEmitterId < size)
//	{
//		// If there are more elements in the list than the particle emitter ID, then the
//		// entire list does not need to be searched. It's impossible for the particle emitter ID to be
//		// located at an index greater than itself, because insertions are only done at the end.
//		itr = std::lower_bound(particleEmitterContainer_->particleEmitterIds_[roomMetadata_->myIndex_].begin(), particleEmitterContainer_->particleEmitterIds_[roomMetadata_->myIndex_].begin() + particleEmitterId, particleEmitterId);
//	}
//	else
//	{
//		itr = std::lower_bound(particleEmitterContainer_->particleEmitterIds_[roomMetadata_->myIndex_].begin(), particleEmitterContainer_->particleEmitterIds_[roomMetadata_->myIndex_].end(), particleEmitterId);
//	}
//
//	int lowerBoundPosition = itr - particleEmitterContainer_->particleEmitterIds_[roomMetadata_->myIndex_].begin();
//
//	size = particleEmitterContainer_->particleEmitterIds_[roomMetadata_->myIndex_].size();
//
//	if (lowerBoundPosition < size)
//	{
//		if (particleEmitterContainer_->particleEmitterIds_[roomMetadata_->myIndex_][lowerBoundPosition] == particleEmitterId)
//		{
//			return particleEmitterContainer_->particleEmitters_[roomMetadata_->myIndex_][lowerBoundPosition]->getPyInstance();
//		}
//		else
//		{
//			return object();
//		}
//	}
//	else
//	{
//		return object();
//	}
//}
//
//object Room::getParticleEmitterPyInstanceById(ParticleEmitterId particleEmitterType, int particleEmitterId)
//{
//	//PythonAcquireGil lock;
//
//	// Find the particle emitter with the given ID in the ID list, and use that index to return the particle emitter in the master particle emitter list.
//
//	std::vector<int>::iterator itr;
//
//	std::vector<boost::shared_ptr<ParticleEmitter>> particleEmitterList = particleEmitterContainer_->particleEmitterTypeMap_.map_[particleEmitterType].particleEmitterList_;
//	std::vector<int> particleEmitterIdList = particleEmitterContainer_->particleEmitterTypeIdMap_[particleEmitterType];
//
//	int size = particleEmitterContainer_->particleEmitterIds_.size();
//
//	if (particleEmitterId < size)
//	{
//		// If there are more elements in the list than the particle emitter ID, then the
//		// entire list does not need to be searched. It's impossible for the particle emitter ID to be
//		// located at an index greater than itself, because insertions are only done at the end.
//		itr = std::lower_bound(particleEmitterIdList.begin(), particleEmitterIdList.begin() + particleEmitterId, particleEmitterId);
//	}
//	else
//	{
//		itr = std::lower_bound(particleEmitterIdList.begin(), particleEmitterIdList.end(), particleEmitterId);
//	}
//
//	int lowerBoundPosition = itr - particleEmitterIdList.begin();
//
//	size = particleEmitterIdList.size();
//
//	if (lowerBoundPosition < size)
//	{
//		if (particleEmitterIdList[lowerBoundPosition] == particleEmitterId)
//		{
//			return particleEmitterList[lowerBoundPosition]->getPyInstance();
//		}
//		else
//		{
//			return object();
//		}
//	}
//	else
//	{
//		return object();
//	}
//}
//
//object Room::getParticleEmitterPyInstanceByName(std::string particleEmitterInstanceName)
//{
//	// Map the given name to an ID and get it.
//
//	int particleEmitterId = particleEmitterContainer_->particleEmitterNameIdMap_[particleEmitterInstanceName];
//
//	return getParticleEmitterPyInstanceById(particleEmitterId);
//}
//
//object Room::getParticleEmitterPyInstanceByName(ParticleEmitterId particleEmitterType, std::string particleEmitterInstanceName)
//{
//	// Map the given name to an ID and get it.
//
//	auto it = particleEmitterContainer_->particleEmitterNameIdMap_.find(particleEmitterInstanceName);
//
//	if (it == particleEmitterContainer_->particleEmitterNameIdMap_.end())
//	{
//		std::cout << "Error: ParticleEmitter \"" + particleEmitterInstanceName + "\" was not found." << std::endl;
//
//		return object();
//	}
//	else
//	{
//		int particleEmitterId = particleEmitterContainer_->particleEmitterNameIdMap_[particleEmitterInstanceName];
//
//		return getParticleEmitterPyInstanceById(particleEmitterType, particleEmitterId);
//	}
//}
