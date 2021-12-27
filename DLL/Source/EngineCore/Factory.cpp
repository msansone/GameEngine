#include "..\..\Headers\EngineCore\Factory.hpp"

using namespace firemelon;

Factory::Factory()
{

}

Factory::~Factory()
{
}

DynamicsController* Factory::createDynamicsController()
{
	DynamicsController* d = createUserDynamicsController();
	
	return d;
}

boost::shared_ptr<RoomManager> Factory::createRoomManager(
										AnimationManagerPtr animationManager,
										boost::shared_ptr<Assets> assets,
										boost::shared_ptr<AudioPlayer> audioPlayer,
										boost::shared_ptr<BaseIds> ids,
										boost::shared_ptr<CameraManager> cameraManager,
										boost::shared_ptr<EngineConfig> engineConfig,
										boost::shared_ptr<EngineController> engineController,
										boost::shared_ptr<FontManager> fontManager,
										boost::shared_ptr<HitboxManager> hitboxManager,
										boost::shared_ptr<InputDeviceManager> inputDeviceManager,
										IoService ioService,
										boost::shared_ptr<LoadingScreenContainer> loadingScreenContainer,
										boost::shared_ptr<Messenger> messenger,
										boost::shared_ptr<PhysicsConfig> physicsConfig,
										boost::shared_ptr<QueryManager> queryManager,
										boost::shared_ptr<Renderer> renderer,
										boost::shared_ptr<RoomContainer> roomContainer,
										boost::shared_ptr<TextManager> textManager,
										boost::shared_ptr<GameTimer> timer,
										boost::shared_ptr<TransitionManager> transitionManager,
										boost::shared_ptr<Ui> ui,
										DebuggerPtr debugger)
{
	return boost::shared_ptr<RoomManager>(new RoomManager(animationManager,
														  assets,
														  audioPlayer,
														  ids,
														  cameraManager,
														  engineConfig,
														  engineController,
														  fontManager,
														  hitboxManager,
														  inputDeviceManager,
				  	 									  ioService,
														  loadingScreenContainer,
														  messenger,
														  physicsConfig,
														  queryManager,
														  renderer,
														  roomContainer,
														  textManager,
														  timer,
														  transitionManager,														  
														  ui,
														  debugger));
}

boost::shared_ptr<GameStateManager> Factory::createGameStateManager(
												  boost::shared_ptr<Ui> ui, boost::shared_ptr<Renderer> renderer,
												  boost::shared_ptr<AudioPlayer> audioPlayer, boost::shared_ptr<RoomManager> roomManager, boost::shared_ptr<InputDeviceManager> inputDeviceManager,
												  boost::shared_ptr<PhysicsManager> physicsManager, boost::shared_ptr<QueryFactory> queryFactory, boost::shared_ptr<QueryResultFactory> queryResultFactory,
												  boost::shared_ptr<QueryParametersFactory> queryParametersFactory, boost::shared_ptr<RenderableManager> renderableManager, boost::shared_ptr<TextManager> textManager,
												  boost::shared_ptr<Messenger> messenger, boost::shared_ptr<QueryManager> queryManager, AnimationManagerPtr animationManager, boost::shared_ptr<HitboxManager> hitboxManager,
												  boost::shared_ptr<AnchorPointManager> anchorPointManager, boost::shared_ptr<EntityMetadataContainer> entityMetadataContainer, boost::shared_ptr<GameTimer> timer,
												  boost::shared_ptr<RoomContainer> roomContainer, boost::shared_ptr<FontManager> fontManager, boost::shared_ptr<CollisionDispatcher> collisionDispatcher,
												  boost::shared_ptr<EngineController> engineController, DebuggerPtr debugger)
{
	return boost::shared_ptr<GameStateManager>(new GameStateManager(ui,
								 			   renderer, 
											   audioPlayer, 
											   roomManager, 
											   inputDeviceManager, 								
											   physicsManager, 
											   queryFactory, 
											   queryResultFactory, 
											   queryParametersFactory, 
											   renderableManager, 
											   textManager, 
											   messenger, 
											   queryManager, 
											   animationManager, 
											   hitboxManager, 
											   anchorPointManager, 
											   entityMetadataContainer,
											   timer,
											   roomContainer,
											   fontManager,
											   collisionDispatcher,
											   engineController,
											   debugger));
}

boost::shared_ptr<CameraManager> Factory::createCameraManager()
{
	return boost::shared_ptr<CameraManager>(new CameraManager());
}

boost::shared_ptr<CollisionDispatcher> Factory::createCollisionDispatcher(CollisionTesterPtr collisionTester, boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger)
{
	return boost::shared_ptr<CollisionDispatcher>(new CollisionDispatcher(collisionTester, ids, debugger));
}

CollisionTesterPtr Factory::createCollisionTester()
{
	return boost::make_shared<CollisionTester>(CollisionTester());
}

boost::shared_ptr<PhysicsManager> Factory::createPhysicsManager(CollisionDispatcherPtr collisionDispatcher, CollisionTesterPtr collisionTester, boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger)
{
	return boost::shared_ptr<PhysicsManager>(new PhysicsManager(collisionDispatcher, collisionTester, ids, debugger));
}

boost::shared_ptr<RenderableManager> Factory::createRenderableManager(DebuggerPtr debugger)
{
	return boost::shared_ptr<RenderableManager>(new RenderableManager(debugger));
}

boost::shared_ptr<AudioSourceContainer> Factory::createAudioSourceContainer()
{
	return boost::shared_ptr<AudioSourceContainer>(new AudioSourceContainer());
}

boost::shared_ptr<TextManager> Factory::createTextManager(RendererPtr renderer,InputDeviceManagerPtr inputDeviceManager, FontManagerPtr fontManager, DebuggerPtr debugger)
{
	return boost::shared_ptr<TextManager>(new TextManager(renderer, inputDeviceManager, fontManager, debugger));
}

boost::shared_ptr<FontManager> Factory::createFontManager(boost::shared_ptr<Renderer> renderer)
{
	return boost::shared_ptr<FontManager>(new FontManager(renderer));
}

boost::shared_ptr<Messenger> Factory::createMessenger()
{
	return boost::shared_ptr<Messenger>(new Messenger());
}

boost::shared_ptr<QueryManager> Factory::createQueryManager()
{
	return boost::shared_ptr<QueryManager>(new QueryManager());
}

boost::shared_ptr<InputDeviceManager> Factory::createInputDeviceManager(boost::shared_ptr<GameButtonManager> buttonManager, boost::shared_ptr<InputDeviceContainer> inputDeviceContainer)
{
	return boost::shared_ptr<InputDeviceManager>(new InputDeviceManager(buttonManager, inputDeviceContainer));
}

boost::shared_ptr<InputDeviceContainer> Factory::createInputDeviceContainer()
{
	return boost::shared_ptr<InputDeviceContainer>(new InputDeviceContainer());
}

boost::shared_ptr<GameButtonManager> Factory::createGameButtonManager()
{
	return boost::shared_ptr<GameButtonManager>(new GameButtonManager());
}

boost::shared_ptr<Renderer> Factory::createRenderer()
{
	return createUserRenderer();
}

DynamicsController* Factory::createUserDynamicsController()
{
	return new DynamicsController();
}

boost::shared_ptr<GameTimer> Factory::createGameTimer()
{
	return createUserGameTimer();
}

boost::shared_ptr<AudioPlayer> Factory::createAudioPlayer()
{
	return createUserAudioPlayer();
}

boost::shared_ptr<RoomLoader> Factory::createRoomLoader()
{
	return createUserRoomLoader();
}

boost::shared_ptr<Ui> Factory::createUi()
{
	return createUserUi();
}

boost::shared_ptr<PhysicsConfig> Factory::createPhysicsConfig()
{
	return boost::shared_ptr<PhysicsConfig>(new PhysicsConfig());
}

boost::shared_ptr<UiPanelContainer> Factory::createUiPanelContainer(DebuggerPtr debugger)
{
	return boost::shared_ptr<UiPanelContainer>(new UiPanelContainer(debugger));
}

boost::shared_ptr<UiWidgetContainer> Factory::createUiWidgetContainer()
{
	return boost::shared_ptr<UiWidgetContainer>(new UiWidgetContainer());
}

boost::shared_ptr<CodeBehindFactory> Factory::createCodeBehindFactory()
{
	return createUserCodeBehindFactory();
}

boost::shared_ptr<QueryFactory> Factory::createQueryFactory()
{
	return createUserQueryFactory();
}

boost::shared_ptr<QueryResultFactory> Factory::createQueryResultFactory()
{
	return createUserQueryResultFactory();
}

boost::shared_ptr<QueryParametersFactory> Factory::createQueryParametersFactory()
{
	return createUserQueryParametersFactory();
}

boost::shared_ptr<UiWidgetFactory> Factory::createUiWidgetFactory()
{
	return createUserUiWidgetFactory();
}

boost::shared_ptr<UiPanelFactory> Factory::createUiPanelFactory()
{
	return createUserUiPanelFactory();
}

boost::shared_ptr<RoomLoader> Factory::createUserRoomLoader()
{
	return boost::shared_ptr<RoomLoader>(new RoomLoader());
}

boost::shared_ptr<Ui> Factory::createUserUi()
{
	return boost::shared_ptr<Ui>(new Ui());
}

boost::shared_ptr<CodeBehindFactory> Factory::createUserCodeBehindFactory()
{
	return boost::shared_ptr<CodeBehindFactory>(new CodeBehindFactory());
}

boost::shared_ptr<QueryFactory> Factory::createUserQueryFactory()
{
	return boost::shared_ptr<QueryFactory>(new QueryFactory());
}

boost::shared_ptr<QueryResultFactory> Factory::createUserQueryResultFactory()
{
	return boost::shared_ptr<QueryResultFactory>(new QueryResultFactory());
}

boost::shared_ptr<QueryParametersFactory> Factory::createUserQueryParametersFactory()
{
	return boost::shared_ptr<QueryParametersFactory>(new QueryParametersFactory());
}

boost::shared_ptr<UiWidgetFactory> Factory::createUserUiWidgetFactory()
{
	return boost::shared_ptr<UiWidgetFactory>(new UiWidgetFactory());
}

boost::shared_ptr<UiPanelFactory> Factory::createUserUiPanelFactory()
{
	return boost::shared_ptr<UiPanelFactory>(new UiPanelFactory());
}

AnimationManagerPtr Factory::createAnimationManager()
{
	return AnimationManagerPtr(new AnimationManager());
}

boost::shared_ptr<HitboxManager> Factory::createHitboxManager()
{
	return boost::shared_ptr<HitboxManager>(new HitboxManager());
}

boost::shared_ptr<AnchorPointManager> Factory::createAnchorPointManager()
{
	return boost::shared_ptr<AnchorPointManager>(new AnchorPointManager());
}

boost::shared_ptr<EntityMetadataContainer> Factory::createEntityMetadataContainer()
{
	return boost::shared_ptr<EntityMetadataContainer>(new EntityMetadataContainer());
}

boost::shared_ptr<RoomContainer> Factory::createRoomContainer()
{
	return boost::shared_ptr<RoomContainer>(new RoomContainer());
}

boost::shared_ptr<RoomMetadataContainer> Factory::createRoomMetadataContainer()
{
	return boost::shared_ptr<RoomMetadataContainer>(new RoomMetadataContainer());
}

boost::shared_ptr<NameValidator> Factory::createNameValidator()
{
	return boost::shared_ptr<NameValidator>(new NameValidator());
}

boost::shared_ptr<LoadingScreenContainer> Factory::createLoadingScreenContainer()
{
	return boost::shared_ptr<LoadingScreenContainer>(new LoadingScreenContainer());
}

boost::shared_ptr<TransitionContainer> Factory::createTransitionContainer()
{
	return boost::shared_ptr<TransitionContainer>(new TransitionContainer());
}

boost::shared_ptr<TransitionManager> Factory::createTransitionManager(boost::shared_ptr<TransitionContainer> transitionContainer, boost::shared_ptr<EngineController> engineController, boost::shared_ptr<BaseIds> ids)
{
	boost::shared_ptr<TransitionManager> transitionManager = boost::shared_ptr<TransitionManager>(new TransitionManager());

	transitionManager->engineController_ = engineController;
	transitionManager->ids_ = ids;
	transitionManager->transitionContainer_ = transitionContainer;

	return transitionManager;
}

boost::shared_ptr<EngineController> Factory::createEngineController(boost::shared_ptr<Renderer> renderer)
{
	boost::shared_ptr<EngineController> engineController = boost::shared_ptr<EngineController>(new EngineController());

	engineController->renderer_ = renderer;

	return engineController;
}

boost::shared_ptr<SystemMessageDispatcher> Factory::createSystemMessageDispatcher()
{
	return boost::shared_ptr<SystemMessageDispatcher>(new SystemMessageDispatcher());
}

NotificationManagerPtr Factory::createNotificationManager()
{
	return createUserNotificationManager();
}

NotificationManagerPtr Factory::createUserNotificationManager()
{
	return boost::make_shared<NotificationManager>(NotificationManager());
}

boost::shared_ptr<Debugger> Factory::createDebugger(NotificationManagerPtr notificationManager)
{
	return boost::make_shared<Debugger>(Debugger(notificationManager));
}
