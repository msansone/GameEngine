/* -------------------------------------------------------------------------
** SdlGamepadDevice.hpp
** 
** The SdlGamepadDevice class is derived from the InputDevice class. It 
** implements the functions necessary to enable a gamepad for input using the
** SDL input library.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SDLGAMEPADDEVICE_HPP_
#define _SDLGAMEPADDEVICE_HPP_

#include "SDL.h"

#include <map>
#include <typeinfo>

#include <boost/lexical_cast.hpp>
#include <boost/signals2.hpp>

#include <InputDevice.hpp>

class SdlGamepadDevice : public firemelon::InputDevice
{
public:
	friend class SdlApp;

	SdlGamepadDevice(SDL_JoystickID joystickId, SDL_GameController* controller, SDL_Joystick* joystick);
	virtual ~SdlGamepadDevice();
	
	virtual	firemelon::ButtonState	getButtonState(firemelon::GameButtonId buttonId);
	virtual bool					initialize();
	virtual std::string				getDeviceButtonName(firemelon::GameButtonId buttonId);
	virtual	bool					configureButton(firemelon::GameButtonId buttonId);

private:

	// Create an enumeration for analog stick directions and triggers.
	enum AnalogStickDirections
	{
		ANALOG_STICK_LEFT_UP = 16,
		ANALOG_STICK_LEFT_DOWN = 17,
		ANALOG_STICK_LEFT_LEFT = 18,
		ANALOG_STICK_LEFT_RIGHT = 19,
		ANALOG_STICK_RIGHT_UP = 20,
		ANALOG_STICK_RIGHT_DOWN = 21,
		ANALOG_STICK_RIGHT_LEFT = 22,
		ANALOG_STICK_RIGHT_RIGHT = 23,
		LEFT_TRIGGER = 24,
		RIGHT_TRIGGER = 25
	};

	std::map<int, std::string>	controllerButtonToNameMap_;

	SDL_GameController*			controller_;
	SDL_Joystick*				joystick_;
	SDL_JoystickID				joystickId_;

	boost::signals2::signal<void(std::string deviceName)>	deviceRemovedSignal_;
};

#endif // _SDLGAMEPADDEVICE_HPP_