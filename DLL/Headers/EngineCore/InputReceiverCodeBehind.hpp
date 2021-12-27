/* -------------------------------------------------------------------------
** InputReceiverCodeBehind.hpp
**
** InputReceiverCodeBehind is the base class which must be derived from to
** implement the input functions for an entity.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _INPUTRECEIVERCODEBEHIND_HPP_
#define _INPUTRECEIVERCODEBEHIND_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "BaseIds.hpp"
#include "CodeBehind.hpp"
#include "EngineController.hpp"
#include "InputEvent.hpp"
#include "InputReceiverScript.hpp"
#include "PythonInstanceWrapper.hpp"
#include "Types.hpp"
#include "Ui.hpp"

namespace firemelon
{
	class FIREMELONAPI InputReceiverCodeBehind : public CodeBehind
	{
	public:
		friend class CodeBehindContainer;
		friend class GameStateManager;
		friend class Room;

		InputReceiverCodeBehind();
		virtual ~InputReceiverCodeBehind();

		void			setInputChannel(InputChannel inputChannel);
		InputChannel	getInputChannel();

	private:


		virtual void	buttonDown(GameButtonId buttonId);
		virtual void	buttonUp(GameButtonId buttonId);
		void			changeInputChannel(InputChannel oldInputChannel, InputChannel newInputChannel);
		virtual void	cleanup();
		virtual void	initialize();

		void			preButtonDown(boost::shared_ptr<InputEvent> inputEvent);
		void			preButtonUp(boost::shared_ptr<InputEvent> inputEvent);
		
		void			preCleanup();
		void			preInitialize();

		boost::shared_ptr<EngineController>		engineController_;
		boost::shared_ptr<InputReceiverScript>	inputReceiverScript_;
		boost::shared_ptr<Ui>					ui_;
	};
}

#endif // _INPUTRECEIVERCODEBEHIND_HPP_