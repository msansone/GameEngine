/* -------------------------------------------------------------------------
** Factory.hpp
**
** The Factory class is the base class for the generated factory class that the
** user fills in to create components such as the renderer and audio player.
** It also has private functions unavailable to the user that create the engine
** subsystem components such as the physics manager, sprite manager, and input
** device manager.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _FACTORY_HPP_
#define _FACTORY_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "AnchorPointManager.hpp"
#include "AnimationManager.hpp"
#include "CameraManager.hpp"
#include "CodeBehindFactory.hpp"
#include "Debugger.hpp"
#include "DynamicsController.hpp"
#include "EngineController.hpp"
#include "EntityMetadataContainer.hpp"
#include "GameButtonManager.hpp"
#include "GameTimer.hpp"
#include "GameStateManager.hpp"
#include "HitboxManager.hpp"
#include "InputDeviceManager.hpp"
#include "LoadingScreenContainer.hpp"
#include "NotificationManager.hpp"
#include "QueryManager.hpp"
#include "RoomContainer.hpp"
#include "RoomManager.hpp"
#include "RoomMetadataContainer.hpp"
#include "TextManager.hpp"
#include "TransitionManager.hpp"
#include "Ui.hpp"

namespace firemelon
{
	class FIREMELONAPI Factory
	{
	public:
		friend class GameStateManager;
		friend class GameEngine;

		Factory();

		virtual ~Factory();

	private:
		
		DynamicsController*						createDynamicsController();

		boost::shared_ptr<RoomManager>	createRoomManager(AnimationManagerPtr animationManager,
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
														  DebuggerPtr debugger);
		
		boost::shared_ptr<GameStateManager>		createGameStateManager(boost::shared_ptr<Ui> ui, boost::shared_ptr<Renderer> renderer,
																	   boost::shared_ptr<AudioPlayer> audioPlayer, boost::shared_ptr<RoomManager> roomManager, boost::shared_ptr<InputDeviceManager> inputDeviceManager,
																	   boost::shared_ptr<PhysicsManager> physicsManager, boost::shared_ptr<QueryFactory> queryFactory, boost::shared_ptr<QueryResultFactory> queryResultFactory,
																	   boost::shared_ptr<QueryParametersFactory> queryParametersFactory, boost::shared_ptr<RenderableManager> renderableManager, boost::shared_ptr<TextManager> textManager,
																	   boost::shared_ptr<Messenger> messender, boost::shared_ptr<QueryManager> queryManager, AnimationManagerPtr animationManager, boost::shared_ptr<HitboxManager> hitboxManager,
																	   boost::shared_ptr<AnchorPointManager> anchorPointManager, boost::shared_ptr<EntityMetadataContainer> entityMetadataContainer, boost::shared_ptr<GameTimer> timer,
																	   boost::shared_ptr<RoomContainer> roomContainer, boost::shared_ptr<FontManager> fontManager, boost::shared_ptr<CollisionDispatcher> collisionDispatcher,
																	   boost::shared_ptr<EngineController> engineController, DebuggerPtr debugger);
				
		boost::shared_ptr<CameraManager>							createCameraManager();
		boost::shared_ptr<CodeBehindFactory>						createCodeBehindFactory();
		boost::shared_ptr<CollisionDispatcher>						createCollisionDispatcher(CollisionTesterPtr collisionTester, boost::shared_ptr<BaseIds> ids, DebuggerPtr debugger);
		CollisionTesterPtr											createCollisionTester();
		boost::shared_ptr<PhysicsManager>							createPhysicsManager(CollisionDispatcherPtr collisionDispatcher, CollisionTesterPtr collisionTester, boost::shared_ptr<BaseIds> ids,DebuggerPtr debugger);
		boost::shared_ptr<RenderableManager>						createRenderableManager(DebuggerPtr debugger);
		boost::shared_ptr<AudioSourceContainer>						createAudioSourceContainer();
		boost::shared_ptr<TextManager>								createTextManager(RendererPtr renderer,InputDeviceManagerPtr inputDeviceManager, FontManagerPtr fontManager, DebuggerPtr debugger);
		boost::shared_ptr<FontManager>								createFontManager(boost::shared_ptr<Renderer> renderer);
		boost::shared_ptr<Messenger>								createMessenger();
		boost::shared_ptr<QueryManager>								createQueryManager();
		boost::shared_ptr<InputDeviceManager>						createInputDeviceManager(boost::shared_ptr<GameButtonManager> buttonManager, boost::shared_ptr<InputDeviceContainer> inputDeviceContainer);
		boost::shared_ptr<InputDeviceContainer>						createInputDeviceContainer();
		boost::shared_ptr<GameButtonManager>						createGameButtonManager();
		boost::shared_ptr<Renderer>									createRenderer();
		boost::shared_ptr<GameTimer>								createGameTimer();
		boost::shared_ptr<AudioPlayer>								createAudioPlayer();
		boost::shared_ptr<RoomLoader>								createRoomLoader();
		boost::shared_ptr<Ui>										createUi();
		boost::shared_ptr<UiPanelContainer>							createUiPanelContainer(DebuggerPtr debugger);
		boost::shared_ptr<UiWidgetContainer>						createUiWidgetContainer();
		boost::shared_ptr<QueryFactory>								createQueryFactory();
		boost::shared_ptr<QueryResultFactory>						createQueryResultFactory();
		boost::shared_ptr<QueryParametersFactory>					createQueryParametersFactory();
		boost::shared_ptr<UiPanelFactory>							createUiPanelFactory();
		boost::shared_ptr<UiWidgetFactory>							createUiWidgetFactory();
		AnimationManagerPtr											createAnimationManager();
		boost::shared_ptr<HitboxManager>							createHitboxManager();
		boost::shared_ptr<AnchorPointManager>						createAnchorPointManager();
		boost::shared_ptr<EntityMetadataContainer>					createEntityMetadataContainer();
		boost::shared_ptr<RoomContainer>							createRoomContainer();
		boost::shared_ptr<RoomMetadataContainer>					createRoomMetadataContainer();
		boost::shared_ptr<NameValidator>							createNameValidator();
		boost::shared_ptr<LoadingScreenContainer>					createLoadingScreenContainer();
		boost::shared_ptr<TransitionContainer>						createTransitionContainer();
		boost::shared_ptr<TransitionManager>						createTransitionManager(boost::shared_ptr<TransitionContainer> transitionContainer, boost::shared_ptr<EngineController> engineController, boost::shared_ptr<BaseIds> ids);
		boost::shared_ptr<EngineController>							createEngineController(boost::shared_ptr<Renderer> renderer);
		boost::shared_ptr<SystemMessageDispatcher>					createSystemMessageDispatcher();
		boost::shared_ptr<NotificationManager>						createNotificationManager();
		boost::shared_ptr<Debugger>									createDebugger(NotificationManagerPtr notificationManager);
		boost::shared_ptr<PhysicsConfig>							createPhysicsConfig();
		
		virtual boost::shared_ptr<AudioPlayer>						createUserAudioPlayer() = 0;
		virtual boost::shared_ptr<CodeBehindFactory>				createUserCodeBehindFactory();
		virtual DynamicsController*									createUserDynamicsController();
		virtual boost::shared_ptr<NotificationManager>				createUserNotificationManager();
		virtual boost::shared_ptr<Renderer>							createUserRenderer() = 0;
		virtual boost::shared_ptr<GameTimer>						createUserGameTimer() = 0;
		virtual boost::shared_ptr<UiPanelFactory>					createUserUiPanelFactory();
		virtual boost::shared_ptr<UiWidgetFactory>					createUserUiWidgetFactory();
		virtual boost::shared_ptr<QueryFactory>						createUserQueryFactory();
		virtual boost::shared_ptr<QueryResultFactory>				createUserQueryResultFactory();
		virtual boost::shared_ptr<QueryParametersFactory>			createUserQueryParametersFactory();
		virtual boost::shared_ptr<RoomLoader>						createUserRoomLoader();
		virtual boost::shared_ptr<Ui>								createUserUi();
	};
}

#endif // _FACTORY_HPP_