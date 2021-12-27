/* -------------------------------------------------------------------------
** CodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CODEBEHIND_HPP_
#define _CODEBEHIND_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "AnchorPointManager.hpp"
#include "AnimationManager.hpp"
#include "AudioPlayer.hpp"
#include "Debugger.hpp"
#include "EngineConfig.hpp"
#include "EngineController.hpp"
#include "EntityController.hpp"
#include "EntityMetadata.hpp"
#include "FontManager.hpp"
#include "GameTimer.hpp"
#include "HitboxManager.hpp"
#include "InputDeviceManager.hpp"
#include "Messenger.hpp"
#include "PythonInstanceWrapper.hpp"
#include "QueryManager.hpp"
#include "TextManager.hpp"

namespace firemelon
{
	class FIREMELONAPI CodeBehind
	{
	public:
		friend class CodeBehindFactory;
		friend class CodeBehindContainer;
		friend class Room;

		CodeBehind();
		virtual ~CodeBehind();

		EntityControllerPtr			getEntityController();
		boost::shared_ptr<HitboxManager>			getHitboxManager();
		boost::shared_ptr<InputDeviceManager>		getInputDeviceManager();
		boost::shared_ptr<EntityMetadata>			getMetadata();
		boost::shared_ptr<PythonInstanceWrapper>	getPythonInstanceWrapper();
		boost::shared_ptr<GameTimer>				getTimer();

		virtual void								cleanup();
		virtual void								initialize();

		void										setClassification(EntityClassification classification);

	protected:

		// Some code behinds (i.e. tiles) should not have an associated a python instance.
		EntityClassification						classification_;
		DebuggerPtr									debugger_;

	private:

		boost::shared_ptr<AnchorPointManager>		anchorPointManager_;
		AnimationManagerPtr							animationManager_;
		boost::shared_ptr<AudioPlayer>				audioPlayer_;
		EntityControllerPtr			controller_;
		boost::shared_ptr<EngineConfig>				engineConfig_;
		boost::shared_ptr<EngineController>			engineController_;
		boost::shared_ptr<FontManager>				fontManager_;
		boost::shared_ptr<HitboxManager>			hitboxManager_;
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		boost::shared_ptr<Messenger>				messenger_;
		boost::shared_ptr<EntityMetadata>			metadata_;
		boost::shared_ptr<PythonInstanceWrapper>	pythonInstanceWrapper_;
		boost::shared_ptr<QueryManager>				queryManager_;
		boost::shared_ptr<TextManager>				textManager_;
		boost::shared_ptr<GameTimer>				timer_;		
	};
}

#endif // _CODEBEHIND_HPP_
