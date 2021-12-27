#include "..\..\Headers\EngineCore\GameEngine.hpp"

using namespace firemelon;
using namespace boost::asio::ip;
using namespace boost::asio;
using namespace boost;
using namespace boost::python;

GameEngine::GameEngine() :
	localFileLoaderIoService_(new boost::asio::io_service),
	localLoaderWork_(new io_service::work( *localFileLoaderIoService_ ))
{
	anchorPointManager_ = nullptr;
	animationManager_ = nullptr;
	assets_ = nullptr;
	audioPlayer_ = nullptr;	
	buttonManager_ = nullptr;
	cameraManager_ = nullptr;
	engineConfig_ = boost::shared_ptr<EngineConfig>(new EngineConfig());
	factory_ = nullptr;
	fontManager_ = nullptr;
	gameTimer_ = nullptr;	
	hitboxManager_ = nullptr;
	ids_ = boost::shared_ptr<BaseIds>(new BaseIds());
	inputDeviceContainer_ = nullptr;
	localAudioSourceContainer_ = nullptr;
	localCollisionDispatcher_ = nullptr;
	localEntityNameValidator_ = nullptr;
	localAudioSourceNameValidator_ = nullptr;
	localGameStateManager_ = nullptr;
	localInputDeviceManager_ = nullptr;
	localMessenger_ = nullptr;
	localParticleEmitterNameValidator_ = nullptr;
	localPhysicsManager_ = nullptr;
	localQueryManager_ = nullptr;
	localRenderableManager_ = nullptr;
	localRoomContainer_ = nullptr;
	localRoomManager_ = nullptr;
	physicsConfig_ = nullptr;
	uiWidgetFactory_  = nullptr;
	queryFactory_ = nullptr;		
	queryResultFactory_ = nullptr;	
	queryParametersFactory_ = nullptr;	
	renderer_ = nullptr;	
	roomLoader_ = nullptr;	
	textManager_ = nullptr;	
	
	timeAccumulator_ = 0.0;
	//lerpTimeAccumulator_ = 0.0;
	lerp_ = 0.0;
	
	// Default it to device ID 0, and allow the user to change it via the engine controller.
	localRoomLoaderThreadStarted_ = false;
	
	roomTransitioningIndex_ = -1;
}

GameEngine::~GameEngine()
{	
	int a = 0;
	a++;
}

void GameEngine::setFactory(boost::shared_ptr<Factory> factory)
{
	factory_ = factory;
}

bool GameEngine::initialize()
{
	// Create the dependencies.	
	if (factory_ != nullptr)
	{
		renderer_ = factory_->createRenderer();
		audioPlayer_ = factory_->createAudioPlayer();	
		gameTimer_ = factory_->createGameTimer();
		fontManager_ = factory_->createFontManager(renderer_);	


		roomLoader_ = factory_->createRoomLoader();	

		queryFactory_ = factory_->createQueryFactory();		
		queryResultFactory_ = factory_->createQueryResultFactory();	
		queryParametersFactory_ = factory_->createQueryParametersFactory();
		uiWidgetFactory_ = factory_->createUiWidgetFactory();
		uiPanelFactory_ = factory_->createUiPanelFactory();
		localQueryManager_ = factory_->createQueryManager();

		physicsConfig_ = factory_->createPhysicsConfig();

		physicsConfig_->getLinearDamp()->setX(-0.1f);
		physicsConfig_->getLinearDamp()->setY(-0.01f);

		// Set the default global linear damp vector

		animationManager_ = factory_->createAnimationManager();
		hitboxManager_ = factory_->createHitboxManager();
		anchorPointManager_ = factory_->createAnchorPointManager();
		entityMetadataContainer_ = factory_->createEntityMetadataContainer();

		codeBehindFactory_ = factory_->createCodeBehindFactory();
		codeBehindFactory_->ids_ = ids_;
		codeBehindFactory_->animationManager_ = animationManager_;
		codeBehindFactory_->renderer_ = renderer_;

		localRoomMetadataContainer_ = factory_->createRoomMetadataContainer();

		localRoomContainer_ = factory_->createRoomContainer();
		localRoomContainer_->ioService_ = localFileLoaderIoService_;
		localRoomContainer_->physicsConfig_ = physicsConfig_;
		localRoomContainer_->roomMetadataContainer_ = localRoomMetadataContainer_;
		
		localAudioSourceNameValidator_ = factory_->createNameValidator();
		localEntityNameValidator_ = factory_->createNameValidator();
		localParticleEmitterNameValidator_ = factory_->createNameValidator();

		systemMessageDispatcher_ = factory_->createSystemMessageDispatcher();
		
		NotificationManagerPtr notificationManager = factory_->createNotificationManager();
			
		debugger_ = factory_->createDebugger(notificationManager);
		
		engineController_ = factory_->createEngineController(renderer_);
		engineController_->systemMessageDispatcher_ = systemMessageDispatcher_;
		engineController_->debugger_ = debugger_;
		engineController_->physicsConfig_ = physicsConfig_;
		

		loadingScreenContainer_ = factory_->createLoadingScreenContainer();
		transitionContainer_ = factory_->createTransitionContainer();
		localTransitionManager_ = factory_->createTransitionManager(transitionContainer_, engineController_, ids_);
		

		uiPanelContainer_ = factory_->createUiPanelContainer(debugger_);

		uiWidgetContainer_ = factory_->createUiWidgetContainer();

		// Create the UI.
		ui_ = factory_->createUi();

		ui_->uiWidgetContainer_ = uiWidgetContainer_;
		ui_->uiPanelContainer_ = uiPanelContainer_;
		ui_->uiWidgetFactory_ = uiWidgetFactory_;
		ui_->uiPanelFactory_ = uiPanelFactory_;
		ui_->ids_ = ids_;
		ui_->renderer_ = renderer_;
		ui_->fontManager_ = fontManager_;
		ui_->systemMessageDispatcher_ = systemMessageDispatcher_;
		ui_->debugger_ = debugger_;
		ui_->audioPlayer_ = audioPlayer_;

		buttonManager_ = factory_->createGameButtonManager();

		inputDeviceContainer_ = factory_->createInputDeviceContainer();

		localInputDeviceManager_ = factory_->createInputDeviceManager(buttonManager_, inputDeviceContainer_);
		
		textManager_ = factory_->createTextManager(renderer_, localInputDeviceManager_, fontManager_, debugger_);
		textManager_->initialize();
		
		CollisionTesterPtr collisionTester = factory_->createCollisionTester();
				
		localCollisionDispatcher_ = factory_->createCollisionDispatcher(collisionTester, ids_, debugger_);
		localCollisionDispatcher_->hitboxManager_ = hitboxManager_;

		localPhysicsManager_ = factory_->createPhysicsManager(localCollisionDispatcher_, collisionTester, ids_, debugger_);
		localPhysicsManager_->renderer_ = renderer_;
		localPhysicsManager_->hitboxManager_ = hitboxManager_;

		localRenderableManager_ = factory_->createRenderableManager(debugger_);
		localRenderableManager_->screenHeight_ = renderer_->getScreenHeight();
		localRenderableManager_->screenWidth_ = renderer_->getScreenWidth();

		cameraManager_ = factory_->createCameraManager();

		localRenderableManager_->cameraManager_ = cameraManager_;
		localPhysicsManager_->cameraManager_ = cameraManager_;

		localAudioSourceContainer_ = factory_->createAudioSourceContainer();
	
		localAudioSourceContainer_->audioSourceNameValidator_ = localAudioSourceNameValidator_;

		localMessenger_ = factory_->createMessenger();

		localRoomManager_ = factory_->createRoomManager(animationManager_,
														assets_,
			                                            audioPlayer_,
														ids_,
														cameraManager_,
														engineConfig_,
														engineController_,
														fontManager_,
														hitboxManager_,
														localInputDeviceManager_,
														localFileLoaderIoService_,
														loadingScreenContainer_,
														localMessenger_,
														physicsConfig_,
														localQueryManager_,
														renderer_,
														localRoomContainer_,
														textManager_,
														gameTimer_,
														localTransitionManager_,
														ui_,
														debugger_);

		localGameStateManager_ = factory_->createGameStateManager(ui_,																	
																  renderer_, 
																  audioPlayer_, 
																  localRoomManager_, 
																  localInputDeviceManager_, 
																  localPhysicsManager_, 
																  queryFactory_, 
																  queryResultFactory_, 
																  queryParametersFactory_, 
																  localRenderableManager_, 
																  textManager_, 
																  localMessenger_, 
																  localQueryManager_, 
																  animationManager_, 
																  hitboxManager_, 
																  anchorPointManager_, 
																  entityMetadataContainer_,
																  gameTimer_,
																  localRoomContainer_,
																  fontManager_,
																  localCollisionDispatcher_,
																  engineController_,
																  debugger_);

		localTransitionManager_->transitionCompleteSignal_->connect(boost::bind(&GameStateManager::showRoom, localGameStateManager_, _1));
	}
	else
	{
		return false;
	}

	intializePythonData();

	assets_ = boost::shared_ptr<Assets>(new Assets(localRenderableManager_, 
												   hitboxManager_, 
												   animationManager_, 
												   ui_,
												   localInputDeviceManager_, 
												   renderer_, 
												   audioPlayer_, 
												   entityMetadataContainer_, 
												   anchorPointManager_,
												   loadingScreenContainer_,
												   transitionContainer_,
												   textManager_,
												   fontManager_,
												   ids_,
												   debugger_));

	threadManager_ = boost::shared_ptr<ThreadManager>(new ThreadManager());

	localRoomContainer_->setAssets(assets_);

	localRoomContainer_->anchorPointManager_ = anchorPointManager_;
	localRoomContainer_->animationManager_ = animationManager_;
	localRoomContainer_->audioPlayer_ = audioPlayer_;
	localRoomContainer_->audioSourceContainer_ = localAudioSourceContainer_;
	localRoomContainer_->cameraManager_ = cameraManager_;
	localRoomContainer_->codeBehindFactory_ = codeBehindFactory_;
	localRoomContainer_->debugger_ = debugger_;
	localRoomContainer_->engineConfig_ = engineConfig_;
	localRoomContainer_->engineController_ = engineController_;
	localRoomContainer_->entityMetadataContainer_ = entityMetadataContainer_;
	localRoomContainer_->entityNameValidator_ = localEntityNameValidator_;
	localRoomContainer_->audioSourceNameValidator_ = localAudioSourceNameValidator_;	
	localRoomContainer_->fontManager_ = fontManager_;
	localRoomContainer_->hitboxManager_ = hitboxManager_;
	localRoomContainer_->ids_ = ids_;
	localRoomContainer_->inputDeviceManager_ = localInputDeviceManager_;
	localRoomContainer_->messenger_ = localMessenger_;
	localRoomContainer_->particleEmitterNameValidator_ = localParticleEmitterNameValidator_;
	localRoomContainer_->physicsManager_ = localPhysicsManager_;
	localRoomContainer_->queryManager_ = localQueryManager_;
	localRoomContainer_->queryParametersFactory_ = queryParametersFactory_;
	localRoomContainer_->queryResultFactory_ = queryResultFactory_;
	localRoomContainer_->renderableManager_ = localRenderableManager_;
	localRoomContainer_->renderer_ = renderer_;
	localRoomContainer_->textManager_ = textManager_;
	localRoomContainer_->timer_ = gameTimer_;
	localRoomContainer_->ui_ = ui_;
	
	localRoomContainer_->initialize();

	localRoomManager_->setAssets(assets_);

	audioPlayer_->initialize();
	
	ui_->inputDeviceManager_ = localInputDeviceManager_;
	ui_->textManager_ = textManager_;
	
	localGameStateManager_->initialize();

	localGameStateManager_->setRoomLoader(roomLoader_);

	localGameStateManager_->loadAssets();	

	uiPanelContainer_->getRoot()->calculateRectSizes(renderer_->getScreenWidth(), renderer_->getScreenHeight(), renderer_->getScreenWidth(), renderer_->getScreenHeight());
	
	//roomLoader_->initialize();
	
	std::cout << "[" << boost::this_thread::get_id() << "] Creating threads... "<< std::endl;
	
	threadManager_->workerThreads_.create_thread(boost::bind( &GameEngine::localRoomLoaderThread, this, localFileLoaderIoService_ ));
	
	int counter = 0;

	while (localRoomLoaderThreadStarted_ == false)
	{
		// Wait.
		if (counter % 20000 == 0)
		{
			std::cout << "Waiting for file loader thread to start..." << std::endl;
		}

		counter++;
	}

	return true;
}

void GameEngine::intializePythonData()
{
	PythonAcquireGil lock;
	
	try
	{
		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = "from _engine import Engine\n";
	
		sCode += "engine = Engine()";
		
		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");
			
		boost::python::str pyCode(sCode);
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);
	
		// Get the instance for this object.
		pyEngineInstance_ = extract<object>(pyMainNamespace_["engine"]);
		pyEngineNamespace_ = pyEngineInstance_.attr("__dict__");
		
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyEngineNamespace_["firemelon"] = pyFiremelonModule;
	
		// Store the functions as python objects.
		pyEngineStarted_ = pyEngineInstance_.attr("engineStarted");
		pyEngineStopped_ = pyEngineInstance_.attr("engineStopped");

	}
	catch(boost::python::error_already_set  &)
	{
		std::cout<<"Error loading engine script."<<std::endl;
		debugger_->handlePythonError();
	}
}

void GameEngine::cleanupPythonData()
{
	try
	{
		PythonAcquireGil lock;

		std::string sCode = "engine = None";

		boost::python::str pyCode(sCode);

		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);
		
		pyMainModule_ = boost::python::object();
		pyMainNamespace_ = boost::python::object();
		pyEngineNamespace_ = boost::python::object();
		pyEngineInstance_ = boost::python::object();
		pyEngineStopped_ = boost::python::object();
		pyEngineStarted_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		std::cout << "Error deleting engine script." << std::endl;
		debugger_->handlePythonError();
	}
}

void GameEngine::startEngine()
{
	// Call the startEngine function. Right now I only have a python function but for completeness I need to
	// do a c++ version like I do with all python function. I usually hate to do this, but I'm swamped with
	// so many tasks that need to be done. findmelater
	{ // Bracket so the lock goes out of scope.
		PythonAcquireGil lock;

		try
		{
			pyEngineStarted_();
		}
		catch (error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}

	ui_->loadPanels();

	ui_->initializeBase();

	gameTimer_->start();
	
	// Load the initial room.
	RoomId initialRoomId = assets_->getInitialRoomId();

	gameTimer_->tick();
	
	localGameStateManager_->showRoom(initialRoomId);
}

void GameEngine::shutdownEngine()
{
	int counter = 0;

	// If any rooms are unloading, wait for that to complete before shutting down.
	while (localRoomContainer_->getIsRoomLoadingOrUnloading() == true)
	{
		// Wait.
		if (counter % 20000 == 0)
		{
			std::cout << "Waiting for room(s) to finish loading or unloading before shutting down engine..." << std::endl;
		}

		counter++;
	}

	std::cout << "Rooms unloaded. Shutting down." << std::endl;


	// Call the engineStopped function. Right now I only have a python function but for completeness I need to
	// do a c++ version like I do with all python function. I usually hate to do this, but I'm swamped with
	// so many tasks that need to be done. findmelater
	{  // Bracket so the lock goes out of scope.
		PythonAcquireGil lock;

		try
		{
			pyEngineStopped_();
		}
		catch (error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}

	audioPlayer_->shutdown();

	localPhysicsManager_->cleanup();

	localRoomContainer_->cleanup();

	localTransitionManager_->transitionCompleteSignal_->disconnect(boost::bind(&GameStateManager::showRoom, localGameStateManager_, _1));
	
	systemMessageDispatcher_->cleanup();
	transitionContainer_->cleanup();
	loadingScreenContainer_->cleanup();
	textManager_->cleanup();
	cameraManager_->setActiveCamera(nullptr);
	localRenderableManager_->cleanup();
	ui_->cleanupBase();
	
	cleanupPythonData();

	localFileLoaderIoService_->stop();

	localRoomManager_->shutdown();

	threadManager_->workerThreads_.join_all();
}

void GameEngine::update()
{
	// Update the timer.
	gameTimer_->tick();

	double frameTime = gameTimer_->getTimeElapsed();
	
	totalTime_ += frameTime;
	
	bool simulationIsStopped = engineController_->getIsSimulationStopped();

	if (simulationIsStopped == false || engineController_->advanceOneFrame_ == true)
	{
		timeAccumulator_ += frameTime;
	}

	bool updateOccurred = false;
	
	Debugger::advanceFrameLog = engineController_->advanceOneFrame_;

	if (engineController_->advanceOneFrame_ == true)
	{
		std::cout << "Advancing frame..." << std::endl;
	}

	int shownRoomId = localRoomContainer_->shownRoomId_;
	int shownRoomIndex = localRoomContainer_->getRoomIndex(shownRoomId);

	// Because the game render is not done in lockstep with the game updates, there is a bug where, when changing rooms,
	// the camera position used to render is still the one from the old room.
	// So there are one or two frames where it looks like the graphics blip, because the new camera position hasn't been set yet.
	// However the showing room ID does get set? So it would seem that if I can update the camera just before the room is shown,
	// it should resolve the bug.
	render(frameTime);

	// Originally I was blocking input to entities when a menu had suspended updates, but this didn't work in the case of when
	// the game map was showing, because the player entity was responsible for showing and hiding the map. So after the map
	// was shown, it no longer received input events which would cause it to hide.

	// For now, I am just tracking the shown menues in the game logic, and handling it case by case. In the future  I will need
	// a more robust solution for input blocking, such as a blacklist and a whitelist for specific buttons, and the ability
	// to block button groups.
	localInputDeviceManager_->setBlockEntityInput(simulationIsStopped);

	if (simulationIsStopped == false || engineController_->advanceOneFrame_ == true)
	{
		// BUG001: Moved from inside GSM. See note in other comments for this bug.

		// Call the RoomLoaded event for any rooms that are flagged as newly loaded.
		int roomCount = localRoomContainer_->getRoomCount();

		for (int i = 0; i < roomCount; i++)
		{
			boost::shared_ptr<Room> room = localRoomContainer_->getRoomByIndex(i);

			if (room->runLoadedEvent_ == true)
			{
				room->createWaitingEntities();

				room->processEntityMoveQueue();

				room->runLoadedEvent_ = false;

				//	room->roomLoaded();
			}
		}

		// Update all the game entities.
		#if defined(_DEBUG)		
			// When compiled in debug mode, the update code takes longer than the fixed-time step, causing
			// a performance degrading feedback loop.
			// Note: The simulation will break with very large delta time values. This should only be used when
			// stopping at a breakpoint is necessary for debugging.
				updateOccurred = true;

				updateLocalGameState(frameTime);
		#else	
			// Update the physics engine in fixed time chunks.
				//timeAccumulator_ += frameTime;

				// Limit to 10 updates, to prevent death spiral.
				int updateCount = 0;

				while (timeAccumulator_ >= fixedTimeStep_ && updateCount < 10)
				{
					updateOccurred = true;

					updateLocalGameState(fixedTimeStep_);

					SimulationTracker::isFirstUpdate = false;

					timeAccumulator_ -= fixedTimeStep_;

					updateCount++;
				}
				
		#endif

		// Turn off the advance frame flag.
		engineController_->advanceOneFrame_ = false;
		
		SimulationTracker::isFirstUpdate = true;
	}

	audioPlayer_->update();

	// Update local.
	localPollInput();
	
	// Finalize the frame.	
	systemMessageDispatcher_->dispatch();

	ui_->update(frameTime);
	
	gameTimer_->frameComplete();
}

void GameEngine::updateLocalGameState(double frameTime)
{
	// If a transition is displaying, don't update the game state.
	if (localTransitionManager_->getIsTransitioning() == false)
	{
		localGameStateManager_->update(frameTime);
		localTransitionManager_->updateAsync(frameTime);
	}
	else
	{
		localTransitionManager_->update(frameTime);
	}	
}

void GameEngine::render(double time)
{
	// Blank out the background.
	renderer_->sceneBegin();

	if (localRoomContainer_->shownRoomId_ != localRoomContainer_->previousShownRoomId_)
	{
		localRoomContainer_->previousShownRoomId_ = localRoomContainer_->shownRoomId_;

		if (debugger_->getDebugMode() == true)
		{
			std::cout << "Rendering room " << localRoomContainer_->shownRoomId_ << std::endl;
		}
	}

	int shownRoomId = localRoomContainer_->shownRoomId_;
	int shownRoomIndex = localRoomContainer_->getRoomIndex(shownRoomId);
	
	boost::shared_ptr<Room> roomToShow = localRoomContainer_->getRoomByIndex(shownRoomIndex);
		
	double lerp = 0.0;

	if (engineController_->getIsSimulationStopped() == false)
	{

		// Don't LERP if it's rendering a transition. This produces an annoying jitter effect on stationary entities.
		if (engineConfig_->getInterpolateFrames() == true && localTransitionManager_->getIsTransitioning() == false)
		{
			lerp_ = timeAccumulator_ / fixedTimeStep_;

			lerp = lerp_;

			// The time accumulator could have some extra time left over that will be consume in the next update.
			// When this happens, the LERP with end up greater than one. Cap it at 1, because the left over time
			// will hold over to the next update, so we don't want to factor it in, otherwise it will be applied
			// twice.
			if (lerp_ > 1.0)
			{
				lerp_ = 1.0;
			}
		}
		else
		{
			lerp_ = 1.0;
		}
	}

	// If running locally, render the game entities.
	localGameStateManager_->render(shownRoomIndex, lerp_);
			
	// If in debug mode, render all active hitbox outlines.
	bool isDebugModeOn = debugger_->getDebugMode();
	
	if (isDebugModeOn == true)
	{	
		bool renderHitboxes = debugger_->getDebugModeRenderHitboxes();
		bool renderDynamicsControllers = debugger_->getDebugModeRenderDynamicsControllers();

		if (renderDynamicsControllers == true)
		{
			localPhysicsManager_->renderDynamicsControllers(shownRoomIndex);
		}

		if (renderHitboxes == true)
		{
			localPhysicsManager_->renderHitboxes(shownRoomIndex);
		}
	}

	ui_->render();

	textManager_->render(shownRoomIndex, lerp_);
}

void GameEngine::endUpdate()
{
	renderer_->sceneComplete();
	
	double frameLimiter = engineConfig_->getFpsLimiter();

	if (frameLimiter > 0.0)
	{
		double waitTime = gameTimer_->getTimeSinceTick();
		// Consume the remaining time to fix it at 60 FPS.
		while (waitTime < frameLimiter)
		{
			// Do nothing.
			waitTime = gameTimer_->getTimeSinceTick();
		}
	}
}

boost::shared_ptr<InputDeviceManager> GameEngine::getInputDeviceManager()
{
	return localInputDeviceManager_;
}

boost::shared_ptr<Ui> GameEngine::getUi()
{
	return ui_;
}

boost::shared_ptr<EngineController> GameEngine::getEngineController()
{
	return engineController_;
}

boost::shared_ptr<GameTimer> GameEngine::getGameTimer()
{
	return gameTimer_;
}

boost::shared_ptr<AudioPlayer> GameEngine::getAudioPlayer()
{
	return audioPlayer_;
}

boost::shared_ptr<Renderer> GameEngine::getRenderer()
{
	return renderer_;
}

boost::shared_ptr<TextManager> GameEngine::getTextManager()
{
	return textManager_;
}

void GameEngine::setFixedTimeStep(double fixedTimeStep)
{
	fixedTimeStep_ = fixedTimeStep;
}


void GameEngine::localRoomLoaderThread( boost::shared_ptr<boost::asio::io_service> io_service )
{	
	debugger_->streamLock->lock();
	std::cout << "[" << boost::this_thread::get_id() << "] Running local file loader thread. "<< std::endl;
	debugger_->streamLock->unlock();

	localRoomLoaderThreadStarted_ = true;
	//std::cout<<"In Worker Thread "<<workerThreadStarted_<<std::endl;
	localFileLoaderIoService_->run();
}


void GameEngine::localPollInput()
{
	localInputDeviceManager_->pollInputStatus();	
}