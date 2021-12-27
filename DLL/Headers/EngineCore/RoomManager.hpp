/* -------------------------------------------------------------------------
** RoomManager.hpp
** 
** The RoomManager class is used for loading, showing, and initializing the
** rooms.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ROOMMANAGER_HPP_
#define _ROOMMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

// This must be defined to change the max number of arguments from 15 to 20, for the addAnimationSlot function in StageElements.
// It must be defined before boost/python.hpp is included.
#define BOOST_PYTHON_MAX_ARITY 20

#include <boost/lexical_cast.hpp>
#include <boost/python.hpp>
#include <boost/python/module.hpp>
#include <boost/python/suite/indexing/map_indexing_suite.hpp>

#include "AnimationManager.hpp"
#include "Assets.hpp"
#include "AudioPlayer.hpp"
#include "BaseIds.hpp"
#include "BasicRenderableText.hpp"
#include "EngineController.hpp"
#include "EngineConfig.hpp"
#include "Entity.hpp"
#include "EntityCreationParameters.hpp"
#include "EntityMetadataContainer.hpp"
#include "EntityTypeMap.hpp"
#include "GameTimer.hpp"
#include "HitboxManager.hpp"
#include "InputDeviceManager.hpp"
#include "LoadingScreenContainer.hpp"
#include "NameIdPair.hpp"
#include "ParticleEmitterCreationParameters.hpp"
#include "PhysicsManager.hpp"
#include "PythonInstanceWrapper.hpp"
#include "QueryManager.hpp"
#include "Renderer.hpp"
#include "RoomContainer.hpp"
#include "RoomLoader.hpp"
#include "ScriptingData.hpp"
#include "StageElements.hpp"
#include "TextManager.hpp"
#include "TileCollisionData.hpp"
#include "TransitionManager.hpp"
#include "Ui.hpp"

namespace firemelon
{
	class FIREMELONAPI RoomManager
	{
	public:
		friend class GameStateManager;
		friend class Query;

		RoomManager(AnimationManagerPtr animationManager,
					boost::shared_ptr<Assets> assets,
					boost::shared_ptr<AudioPlayer> audioPlayer,
					boost::shared_ptr<BaseIds> ids,
					boost::shared_ptr<CameraManager> cameraManager,
					boost::shared_ptr<EngineConfig> engineConfig,
					boost::shared_ptr<EngineController> enginerController,
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

		virtual ~RoomManager();
	
		void								setAssets(boost::shared_ptr<Assets> assets);
		boost::shared_ptr<RoomContainer>	getRoomContainer();
		
		void								loadRoomByNamePy(std::string roomName, stringmap roomParameters);
		void								loadRoomByName(std::string roomName, stringmap roomParameters);
		void								loadRoomPy(RoomId roomId, stringmap roomParameters);
		void								loadRoom(RoomId roomId, stringmap roomParameters);
		void								loadRoom2(RoomId roomId);
		
		void			showRoomPy(RoomId roomId, TransitionId transitionId, double transitionTime);
		void			showRoom(RoomId roomId, TransitionId transitionId, double transitionTime);
		void			showRoomByNamePy(std::string roomName, TransitionId transitionId, double transitionTime);
		void			showRoomByName(std::string roomName, TransitionId transitionId, double transitionTime);

		void			unloadRoomPy(RoomId roomId);
		void			unloadRoom(RoomId roomId);

		void			loadAssets();
		
		RoomId			getShownRoomIdPy();
		RoomId			getShownRoomId();

		void			shutdown();

	private:

		void	showRoomImmediate(RoomId roomId);

		void	initialize();
		void	loadQueuedRoom();
		void	moveAllEntities();

		boost::shared_ptr<AnchorPointManager>		anchorPointManager_;
		AnimationManagerPtr							animationManager_;
		boost::shared_ptr<Assets>					assets_;
		boost::shared_ptr<AudioPlayer>				audioPlayer_;
		boost::shared_ptr<CameraController>			camera_;
		boost::shared_ptr<CameraManager>			cameraManager_;
		DebuggerPtr									debugger_;
		boost::shared_ptr<EngineConfig>				engineConfig_;
		boost::shared_ptr<EngineController>			engineController_;
		boost::shared_ptr<EntityMetadataContainer>	entityMetadataContainer_;
		boost::shared_ptr<FontManager>				fontManager_;
		boost::shared_ptr<HitboxManager>			hitboxManager_;
		boost::shared_ptr<BaseIds>					ids_;
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		IoService									ioService_;
		boost::shared_ptr<LoadingScreenContainer>	loadingScreenContainer_;
		boost::shared_ptr<Messenger>				messenger_;
		boost::shared_ptr<PhysicsConfig>			physicsConfig_;
		boost::shared_ptr<PhysicsManager>			physicsManager_;
		boost::shared_ptr<RenderableManager>		renderableManager_;
		boost::shared_ptr<QueryManager>				queryManager_;
		boost::shared_ptr<QueryParametersFactory>	queryParametersFactory_;
		boost::shared_ptr<QueryResultFactory>		queryResultFactory_;
		boost::shared_ptr<Renderer>					renderer_;
		boost::shared_ptr<RoomContainer>			roomContainer_;
		boost::shared_ptr<RoomLoader>				roomLoader_;
		boost::shared_ptr<TextManager>				textManager_;
		boost::shared_ptr<GameTimer>				timer_;
		boost::shared_ptr<TransitionManager>		transitionManager_;
		boost::shared_ptr<Ui>						ui_;

		RoomId							roomToLoad_;
		RoomId							loadedRoom_;

		GameButtonId					continueButton_;
		
		static bool						isPythonInitialized_;
		bool							isInitialized_;
		bool							isFullscreen_;		
		bool							isRoomLoaded_;
		
		// A list of key-value pairs that the user sets, which will be passed as a parameter of 
		// the virtual roomLoaded function.
		stringmap						roomParameters_;
		PyThreadState*					state_;
		boost::python::object			pyDynamicsControllerClass_;
	};

}

#endif // _ROOMMANAGER_HPP_