#include "..\Headers\SdlKeyboardDevice.hpp"

using namespace firemelon;

SdlKeyboardDevice::SdlKeyboardDevice(std::string inputDeviceName) : InputDevice(inputDeviceName)
{
}

SdlKeyboardDevice::~SdlKeyboardDevice()
{
}

// The getButtonState function gets the SDL key state array and return 
// whether the key that the given button code is mapped to is down or up.
// The input device manager uses this function to track the state of each 
// of the game buttons defined in the editor.
ButtonState SdlKeyboardDevice::getButtonState(GameButtonId buttonId)
{
	ButtonState buttonState = BUTTON_STATE_UP;

	if (isBlocked_ == false)
	{
		// Get the SDL KeyState array
		const Uint8* keystates = SDL_GetKeyboardState(nullptr);

		// Check if the key mapped to this button is down.
		int mappedButtonCode = getDeviceButtonCode(buttonId);

		if (keystates[mappedButtonCode] == true)
		{
			buttonState = BUTTON_STATE_DOWN;
		}
	}

	return buttonState;
}

// The configureButton function is used to map a game button defined in 
// the editor to an SDL scan code. Consume the SDL event queue, checking
// for keydown events.
bool SdlKeyboardDevice::configureButton(GameButtonId buttonId)
{  
	bool buttonSet = false;
	SDL_Event event;
	SDL_Scancode* scanCode;

	if (SDL_PollEvent(&event))
	{
		switch(event.type)
		{
			case SDL_KEYDOWN:
				
				if (event.key.repeat == 0)
				{
					scanCode = &event.key.keysym.scancode;

					mapDeviceButtonCode(buttonId, (int)*scanCode);

					buttonSet = true;
				}

				break;

			default:
				break;
		}
	}

	return buttonSet;
}

// The userInitialize function populates the initial game button mapping
// with default values.
bool SdlKeyboardDevice::initialize()
{
	//Ids ids;

	//buttonMap_[ids_->GAMEBUTTON_UP] = SDL_SCANCODE_UP;
	//buttonMap_[ids_->GAMEBUTTON_DOWN] = SDL_SCANCODE_DOWN;
	//buttonMap_[ids_->GAMEBUTTON_LEFT] = SDL_SCANCODE_LEFT;
	//buttonMap_[ids_->GAMEBUTTON_RIGHT] = SDL_SCANCODE_RIGHT;
	//buttonMap_[ids_->GAMEBUTTON_ATTACKSELECT] = SDL_SCANCODE_Z;
	//buttonMap_[ids_->GAMEBUTTON_JUMPBACK] = SDL_SCANCODE_X;
	//buttonMap_[ids_->GAMEBUTTON_START] = SDL_SCANCODE_RETURN;
	//buttonMap_[ids_->GAMEBUTTON_SELECT] = SDL_SCANCODE_BACKSLASH;

	return true;
}

// The getDeviceButtonName function returns a string value representing the SDL 
// scancode that the given game button is mapped to.
std::string SdlKeyboardDevice::getDeviceButtonName(GameButtonId buttonId)
{
	std::string keyName = SDL_GetScancodeName((SDL_Scancode)getDeviceButtonCode(buttonId));
	
	return boost::to_upper_copy(keyName);
}

void SdlKeyboardDevice::keyDown(SDL_Keycode keyCode)
{
	keyDownSignal_(keyCode);
}

void SdlKeyboardDevice::keyUp(SDL_Keycode keyCode)
{
	keyUpSignal_(keyCode);
}