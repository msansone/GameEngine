/* -------------------------------------------------------------------------
** InputState.hpp
** 
** The InputState class stores the state of the game buttons. It is used
** so that the state of an input device can be replicated across the network.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _INPUTSTATE_HPP_
#define _INPUTSTATE_HPP_

#include <map>

#include "BaseIds.hpp"

namespace firemelon
{
	enum ButtonState
	{
		// The button is currently down.
		BUTTON_STATE_DOWN = 0,

		// The button is currently up.
		BUTTON_STATE_UP = 1
	};
	
	typedef std::map<GameButtonId, ButtonState> ButtonIdStateMap;

	class InputState
	{
	public:
		friend class InputDevice;

		InputState();
		virtual ~InputState();
		
		ButtonState getButtonState(GameButtonId buttonCode);

		void		setButtonState(GameButtonId buttonCode, ButtonState buttonState);

	private:
		
		ButtonIdStateMap	buttonStateMap_;
	};
}

#endif // _INPUTSTATE_HPP_