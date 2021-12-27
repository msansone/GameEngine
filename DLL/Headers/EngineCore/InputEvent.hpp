/* -------------------------------------------------------------------------
** InputEvent.hpp
**
** The InputEvent class represents a button up or button down input event 
** generated by an input device. It contains the button ID, input device name, 
** and channel  It is propagated along a chain starting from the input device
** to any valid listening objects.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _INPUTEVENT_HPP_
#define _INPUTEVENT_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <iostream>
#include <string>
#include "BaseIds.hpp"

namespace firemelon
{
	class FIREMELONAPI InputEvent
	{
	public:
		friend class InputDeviceManager;
		friend class InputReceiverCodeBehind;
		friend class Room;
		friend class Ui;

		InputEvent();
		virtual ~InputEvent();
	
		void			setDeviceName(std::string deviceName);
		std::string		getDeviceName();
	
		void			setChannel(InputChannel channel);
		InputChannel	getChannel();

		void			setButtonId(GameButtonId buttonCode);
		GameButtonId	getButtonId();

	private:

		bool			blockEntityInput_;

		bool			blockUiInput_;

		GameButtonId	buttonId_;

		InputChannel	channel_;

		std::string		deviceName_;

		bool			ignoreUiInput_;
	};
}

#endif // _INPUTEVENT_HPP_