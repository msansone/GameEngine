/* -------------------------------------------------------------------------
** EngineController.hpp
**
** The EngineController class is used to expose engine functions and settings
** to the user.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENGINECONTROLLER_HPP_
#define _ENGINECONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/shared_ptr.hpp>
#include <boost/python.hpp>
#include <boost/signals2.hpp>
#include <boost/algorithm/string.hpp>

#include <string>

#include "BaseIds.hpp"
#include "Debugger.hpp"
#include "PhysicsConfig.hpp"
#include "Renderer.hpp"
#include "SystemMessageDispatcher.hpp"
#include "Types.hpp"

namespace firemelon
{
	class FIREMELONAPI EngineController
	{
	public:
		friend class Factory;
		friend class GameEngine;
		friend class GameStateManager;

		EngineController();
		virtual ~EngineController();


		void						advanceSimulationFrame();
		void						advanceSimulationFramePy();

		void						exit();
		void						exitPy();
		
		bool						getHasQuit();

		RoomId						getIdFromNamePy(std::string name);
		RoomId						getIdFromName(std::string name);

		bool						getIsSimulationStopped();

		std::string					getNameFromIdPy(FiremelonId id);
		std::string					getNameFromId(FiremelonId id);
		
		void						runCommandPy(std::string command);
		void						runCommand(std::string command);

		void						saveScreenshotPy(std::string fileName);
		void						saveScreenshot(std::string fileName);

		void						setSimulationTimeScalePy(float timeScale);
		void						setSimulationTimeScale(float timeScale);

		float						getSimulationTimeScalePy();
		float						getSimulationTimeScale();
		
		void						startSimulation();
		void						startSimulationPy();

		void						stopSimulation();
		void						stopSimulationPy();
		
	private:
		
		// When the simulation is paused, move to the next frame.
		bool						advanceOneFrame_;

		DebuggerPtr					debugger_;

		bool						hasQuit_;

		bool						isSimulationStopped_;
		
		PhysicsConfigPtr			physicsConfig_;

		RendererPtr					renderer_;

		SystemMessageDispatcherPtr	systemMessageDispatcher_;
	};
}

#endif // _ENGINECONTROLLER_HPP_