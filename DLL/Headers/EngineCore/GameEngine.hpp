/* -------------------------------------------------------------------------
** GameEngine.hpp
**
** The GameEngine class is the main class that creates, initialized and updates
** all the subsystems and components. The user is responsible for creating and
** deleting it, as well as calling the initialize, update, and endUpdate functions.
** It is also used to set any custom configurable properties, such as the fixed
** time step the physics simulation will use.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _GAMEENGINE_HPP_
#define _GAMEENGINE_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/asio.hpp>
#include <boost/python.hpp>

#include "CollisionTester.hpp"
#include "EngineConfig.hpp"
#include "Factory.hpp"
#include "SimulationTracker.hpp"
#include "ThreadManager.hpp"

namespace firemelon
{
	typedef boost::shared_ptr<boost::asio::io_service::work> AsioWork;
	typedef	boost::thread_group ThreadGroup;
	typedef std::vector<EntityCreationParameters> EntityCreationParametersList;

	class FIREMELONAPI GameEngine
	{
	public:

		GameEngine();
		virtual ~GameEngine();
		
		void									endUpdate();
		boost::shared_ptr<AudioPlayer>			getAudioPlayer();
		boost::shared_ptr<EngineController>		getEngineController();
		boost::shared_ptr<GameTimer>			getGameTimer();
		boost::shared_ptr<InputDeviceManager>	getInputDeviceManager();
		boost::shared_ptr<Renderer>				getRenderer();
		boost::shared_ptr<TextManager>			getTextManager();
		boost::shared_ptr<Ui>					getUi();
		bool									initialize();
		void									setFactory(boost::shared_ptr<Factory> factory);
		void									setFixedTimeStep(double fixedTimeStep);
		void									shutdownEngine();
		void									startEngine();
		void									update();
		void									writeDebugLogFile();

	private:
	
		void	render(double time);

		// Local functions
		void	updateLocalGameState(double frameTime);
		void	localPollInput();

		// Worker threads
		void	localRoomLoaderThread( boost::shared_ptr<boost::asio::io_service> io_service );

		// Python functions
		void	intializePythonData();
		void	cleanupPythonData();

		boost::shared_ptr<AnchorPointManager>				anchorPointManager_;
		AnimationManagerPtr									animationManager_;
		boost::shared_ptr<Assets>							assets_;
		boost::shared_ptr<AudioPlayer>						audioPlayer_;
		boost::shared_ptr<GameButtonManager>				buttonManager_;
		boost::shared_ptr<CameraManager>					cameraManager_;
		boost::shared_ptr<CodeBehindFactory>				codeBehindFactory_;
		boost::shared_ptr<Debugger>							debugger_;
		boost::shared_ptr<EngineController>					engineController_;
		boost::shared_ptr<EngineConfig>						engineConfig_;
		boost::shared_ptr<EntityMetadataContainer>			entityMetadataContainer_;
		boost::shared_ptr<Factory>							factory_;
		boost::shared_ptr<FontManager>						fontManager_;
		boost::shared_ptr<HitboxManager>					hitboxManager_;
		boost::shared_ptr<GameTimer>						gameTimer_;
		boost::shared_ptr<BaseIds>							ids_;
		boost::shared_ptr<InputDeviceContainer>				inputDeviceContainer_;
		boost::shared_ptr<LoadingScreenContainer>			loadingScreenContainer_;
		boost::shared_ptr<AudioSourceContainer>				localAudioSourceContainer_;
		boost::shared_ptr<NameValidator>					localAudioSourceNameValidator_;
		boost::shared_ptr<CollisionDispatcher>				localCollisionDispatcher_;
		boost::shared_ptr<NameValidator>					localEntityNameValidator_;
		boost::shared_ptr<GameStateManager>					localGameStateManager_;
		boost::shared_ptr<InputDeviceManager>				localInputDeviceManager_;
		boost::shared_ptr<Messenger>						localMessenger_;
		boost::shared_ptr<NameValidator>					localParticleEmitterNameValidator_;
		boost::shared_ptr<PhysicsManager>					localPhysicsManager_;
		boost::shared_ptr<QueryManager>						localQueryManager_;
		boost::shared_ptr<RoomContainer>					localRoomContainer_;
		boost::shared_ptr<RoomManager>						localRoomManager_;
		boost::shared_ptr<RoomMetadataContainer>			localRoomMetadataContainer_;
		boost::shared_ptr<RenderableManager>				localRenderableManager_;
		boost::shared_ptr<TransitionManager>				localTransitionManager_;
		boost::shared_ptr<PhysicsConfig>					physicsConfig_;
		boost::shared_ptr<UiPanelFactory>					uiPanelFactory_;
		boost::shared_ptr<UiWidgetFactory>					uiWidgetFactory_;
		boost::shared_ptr<QueryFactory>						queryFactory_;
		boost::shared_ptr<QueryParametersFactory>			queryParametersFactory_;
		boost::shared_ptr<QueryResultFactory>				queryResultFactory_;
		boost::shared_ptr<Renderer>							renderer_;
		boost::shared_ptr<RoomLoader>						roomLoader_;
		boost::shared_ptr<SystemMessageDispatcher>			systemMessageDispatcher_;
		boost::shared_ptr<TextManager>						textManager_;
		boost::shared_ptr<ThreadManager>					threadManager_;
		boost::shared_ptr<TransitionContainer>				transitionContainer_;
		boost::shared_ptr<Ui>								ui_;
		boost::shared_ptr<UiPanelContainer>					uiPanelContainer_;
		boost::shared_ptr<UiWidgetContainer>				uiWidgetContainer_;	
				
		double							fixedTimeStep_;
		double							timeAccumulator_;
		//double							lerpTimeAccumulator_;
		double							totalTime_;
		double							timer_;
		double							lerp_;
		
		bool							localRoomLoaderThreadStarted_;
		
		int								roomTransitioningIndex_;
		
		// Used to load files in the client objects.
		IoService						localFileLoaderIoService_;

		AsioWork						localLoaderWork_;
		
		// Python Scripting data
		std::string						scriptName_;
		std::string						scriptTypeName_;
		std::string						scriptVar_;

		boost::python::object			pyMainModule_;
		boost::python::object			pyMainNamespace_;
		boost::python::object			pyEngineInstance_;
		boost::python::object			pyEngineNamespace_;

		boost::python::object			pyEngineStarted_;
		boost::python::object			pyEngineStopped_;
	};
}

#endif // _GAMEENGINE_HPP_