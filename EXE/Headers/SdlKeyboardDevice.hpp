/* -------------------------------------------------------------------------
** SdlKeyboardDevice.hpp
** 
** The SdlKeyboardDevice class is derived from the InputDevice class. It 
** implements the functions necessary to enable a keyboard for input using the
** SDL input library.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SDLKEYBOARDDEVICE_HPP_
#define _SDLKEYBOARDDEVICE_HPP_

#include "SDL.h"

#include <boost/algorithm/string.hpp>
#include <boost/signals2.hpp>

#include <InputDevice.hpp>

class SdlKeyboardDevice : public firemelon::InputDevice
{
public:
	friend class FiremelonExInputReceiverCodeBehind;
	friend class FiremelonExUi;
	friend class FiremelonExUiWidget;

	SdlKeyboardDevice(std::string inputDeviceName);
	virtual ~SdlKeyboardDevice();
	
	virtual	firemelon::ButtonState	getButtonState(firemelon::GameButtonId buttonId);
	virtual	bool					configureButton(firemelon::GameButtonId buttonId);
	virtual bool					initialize();
	virtual std::string				getDeviceButtonName(firemelon::GameButtonId buttonId);
	void							keyDown(SDL_Keycode keyCode);
	void							keyUp(SDL_Keycode keyCode);

private:

	boost::signals2::signal<void (SDL_Keycode keyCode)> keyDownSignal_;
	boost::signals2::signal<void (SDL_Keycode keyCode)> keyUpSignal_;
};

#endif // _SDLKEYBOARDDEVICE_HPP_