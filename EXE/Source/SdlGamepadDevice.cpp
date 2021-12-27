#include "..\Headers\SdlGamepadDevice.hpp"

using namespace firemelon;

SdlGamepadDevice::SdlGamepadDevice(SDL_JoystickID joystickId, SDL_GameController* controller, SDL_Joystick* joystick) : InputDevice("")
{
	controller_ = controller;
	joystick_ = joystick;
	joystickId_ = joystickId;
}

SdlGamepadDevice::~SdlGamepadDevice()
{
	//Close the joystick 
	if (controller_ != nullptr)
	{
		SDL_GameControllerClose(controller_);
		//Do I need this? SDL_JoystickClose(joystick_);
	}
}

ButtonState SdlGamepadDevice::getButtonState(GameButtonId buttonId)
{
	int xMoveLeft = SDL_GameControllerGetAxis(controller_, SDL_CONTROLLER_AXIS_LEFTX);
	int yMoveLeft = SDL_GameControllerGetAxis(controller_, SDL_CONTROLLER_AXIS_LEFTY);
	int xMoveRight = SDL_GameControllerGetAxis(controller_, SDL_CONTROLLER_AXIS_RIGHTX);
	int yMoveRight = SDL_GameControllerGetAxis(controller_, SDL_CONTROLLER_AXIS_RIGHTY);
	int leftTrigger = SDL_GameControllerGetAxis(controller_, SDL_CONTROLLER_AXIS_TRIGGERLEFT);
	int rightTrigger = SDL_GameControllerGetAxis(controller_, SDL_CONTROLLER_AXIS_TRIGGERRIGHT);
	
	ButtonState buttonState = BUTTON_STATE_UP;

	int mappedButtonCode = getDeviceButtonCode(buttonId);

	// Check if the gamepad button mapped to this button is pressed.
	// The analog sticks are a special case, as they are joysticks, not buttons.

	switch (mappedButtonCode)
	{
		case ANALOG_STICK_LEFT_UP:
				
			if (yMoveLeft < -3200)
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case ANALOG_STICK_RIGHT_UP:

			if (yMoveRight < -3200)
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case ANALOG_STICK_LEFT_DOWN:
				
			if (yMoveLeft > 3200) 
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case ANALOG_STICK_RIGHT_DOWN:

			if (yMoveRight > 3200)
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case ANALOG_STICK_LEFT_LEFT:
				
			if (xMoveLeft < -3200) 
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case ANALOG_STICK_RIGHT_LEFT:

			if (xMoveRight < -3200)
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case ANALOG_STICK_LEFT_RIGHT:
				
			if (xMoveLeft > 3200) 
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case ANALOG_STICK_RIGHT_RIGHT:

			if (xMoveRight > 3200)
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case LEFT_TRIGGER:

			if (leftTrigger > 3200)
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		case RIGHT_TRIGGER:

			if (rightTrigger > 3200)
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

		default:

			if (SDL_GameControllerGetButton(controller_, (SDL_GameControllerButton)mappedButtonCode))
			{
				buttonState = BUTTON_STATE_DOWN;
			}

			break;

	}

	return buttonState;
}

bool SdlGamepadDevice::initialize()
{
	std::string deviceName = SDL_GameControllerName(controller_);

	std::cout << "Initializing game controller device: " << deviceName << std::endl;

	setDeviceName(deviceName);

	controllerButtonToNameMap_[ANALOG_STICK_LEFT_UP] = "LEFT ANALOG UP";
	controllerButtonToNameMap_[ANALOG_STICK_LEFT_DOWN] = "LEFT ANALOG DOWN";
	controllerButtonToNameMap_[ANALOG_STICK_LEFT_LEFT] = "LEFT ANALOG LEFT";
	controllerButtonToNameMap_[ANALOG_STICK_LEFT_RIGHT] = "LEFT ANALOG RIGHT";
	controllerButtonToNameMap_[ANALOG_STICK_RIGHT_UP] = "RIGHT ANALOG UP";
	controllerButtonToNameMap_[ANALOG_STICK_RIGHT_DOWN] = "RIGHT ANALOG DOWN";
	controllerButtonToNameMap_[ANALOG_STICK_RIGHT_LEFT] = "RIGHT ANALOG LEFT";
	controllerButtonToNameMap_[ANALOG_STICK_RIGHT_RIGHT] = "RIGHT ANALOG RIGHT";
	controllerButtonToNameMap_[LEFT_TRIGGER] = "LEFT TRIGGER";
	controllerButtonToNameMap_[RIGHT_TRIGGER] = "RIGHT TRIGGER";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_A] = "A";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_B] = "B";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_X] = "X";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_Y] = "Y";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_BACK] = "BACK";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_GUIDE] = "GUIDE";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_START] = "START";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_LEFTSTICK] = "LEFT STICK";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_RIGHTSTICK] = "RIGHT STICK";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_LEFTSHOULDER] = "LEFT SHOULDER";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_RIGHTSHOULDER] = "RIGHT SHOULDER";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_DPAD_UP] = "UP";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_DPAD_DOWN] = "DOWN";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_DPAD_LEFT] = "LEFT";
	controllerButtonToNameMap_[SDL_CONTROLLER_BUTTON_DPAD_RIGHT] = "RIGHT";

	return true;
}

std::string SdlGamepadDevice::getDeviceButtonName(GameButtonId buttonId)
{

	std::string keyName = "";
	
	SDL_GameControllerButton mappedButton = (SDL_GameControllerButton)getDeviceButtonCode(buttonId);

	switch (mappedButton)
	{
		case ANALOG_STICK_LEFT_UP:
			keyName = "LEFT ANALOG UP";
			break;

		case ANALOG_STICK_LEFT_DOWN:
			keyName = "LEFT ANALOG DOWN";
			break;

		case ANALOG_STICK_LEFT_LEFT:
			keyName = "LEFT ANALOG LEFT";
			break;

		case ANALOG_STICK_LEFT_RIGHT:
			keyName = "LEFT ANALOG RIGHT";
			break;

		case ANALOG_STICK_RIGHT_UP:
			keyName = "RIGHT ANALOG UP";
			break;

		case ANALOG_STICK_RIGHT_DOWN:
			keyName = "RIGHT ANALOG DOWN";
			break;

		case ANALOG_STICK_RIGHT_LEFT:
			keyName = "RIGHT ANALOG LEFT";
			break;

		case ANALOG_STICK_RIGHT_RIGHT:
			keyName = "RIGHT ANALOG RIGHT";
			break;

		case LEFT_TRIGGER:
			keyName = "LEFT TRIGGER";
			break;

		case RIGHT_TRIGGER:
			keyName = "RIGHT TRIGGER";
			break;

		case SDL_CONTROLLER_BUTTON_A:
			keyName = "A";
			break;

		case SDL_CONTROLLER_BUTTON_B:
			keyName = "B";
			break;

		case SDL_CONTROLLER_BUTTON_X:
			keyName = "X";
			break;

		case SDL_CONTROLLER_BUTTON_Y:
			keyName = "Y";
			break;

		case SDL_CONTROLLER_BUTTON_BACK:
			keyName = "BACK";
			break;

		case SDL_CONTROLLER_BUTTON_GUIDE:
			keyName = "GUIDE";
			break;

		case SDL_CONTROLLER_BUTTON_START:
			keyName = "START";
			break;

		case SDL_CONTROLLER_BUTTON_LEFTSTICK:
			keyName = "LEFT STICK";
			break;

		case SDL_CONTROLLER_BUTTON_RIGHTSTICK:
			keyName = "RIGHT STICK";
			break;

		case SDL_CONTROLLER_BUTTON_LEFTSHOULDER:
			keyName = "LEFT SHOULDER";
			break;

		case SDL_CONTROLLER_BUTTON_RIGHTSHOULDER:
			keyName = "RIGHT SHOULDER";
			break;

		case SDL_CONTROLLER_BUTTON_DPAD_UP:
			keyName = "UP";
			break;

		case SDL_CONTROLLER_BUTTON_DPAD_DOWN:
			keyName = "DOWN";
			break;

		case SDL_CONTROLLER_BUTTON_DPAD_LEFT:
			keyName = "LEFT";
			break;

		case SDL_CONTROLLER_BUTTON_DPAD_RIGHT:
			keyName = "RIGHT";
			break;
	}

	return keyName;
}

bool SdlGamepadDevice::configureButton(GameButtonId buttonId)
{
	bool buttonSet = false;
	SDL_Event event;
	int keyCode = -5;
	int joystickId = -1;

	if (SDL_PollEvent(&event))
	{
		switch(event.type)
		{
			case SDL_CONTROLLERDEVICEREMOVED:
				
				// What happens if the input device is removed while it is configuring??
				buttonSet = true;

				deviceRemovedSignal_(getDeviceName());

				break;

			case SDL_CONTROLLERBUTTONDOWN:
				joystickId = event.cbutton.which;

				if (joystickId == joystickId_)
				{
					keyCode = event.cbutton.button;
					mapDeviceButtonCode(buttonId, keyCode);
					buttonSet = true;
				}

				break;

			case SDL_CONTROLLERAXISMOTION:

				joystickId = event.caxis.which;

				if (joystickId == joystickId_)
				{
					if (event.caxis.value < -3200)
					{
						if (event.caxis.axis == SDL_CONTROLLER_AXIS_LEFTX)
						{
							keyCode = ANALOG_STICK_LEFT_LEFT;
						}
						else if (event.caxis.axis == SDL_CONTROLLER_AXIS_LEFTY)
						{
							keyCode = ANALOG_STICK_LEFT_UP;
						}
						else if (event.caxis.axis == SDL_CONTROLLER_AXIS_RIGHTX)
						{
							keyCode = ANALOG_STICK_RIGHT_LEFT;
						}
						else if (event.caxis.axis == SDL_CONTROLLER_AXIS_RIGHTY)
						{
							keyCode = ANALOG_STICK_RIGHT_UP;
						}
					}
					else if (event.caxis.value > 3200)
					{
						if (event.caxis.axis == SDL_CONTROLLER_AXIS_LEFTX)
						{
							keyCode = ANALOG_STICK_LEFT_RIGHT;
						}
						else if (event.caxis.axis == SDL_CONTROLLER_AXIS_LEFTY)
						{
							keyCode = ANALOG_STICK_LEFT_DOWN;
						}
						if (event.caxis.axis == SDL_CONTROLLER_AXIS_RIGHTX)
						{
							keyCode = ANALOG_STICK_RIGHT_RIGHT;
						}
						else if (event.caxis.axis == SDL_CONTROLLER_AXIS_RIGHTY)
						{
							keyCode = ANALOG_STICK_RIGHT_DOWN;
						}
						else if (event.caxis.axis == SDL_CONTROLLER_AXIS_TRIGGERLEFT)
						{
							keyCode = LEFT_TRIGGER;
						}

						else if (event.caxis.axis == SDL_CONTROLLER_AXIS_TRIGGERRIGHT)
						{
							keyCode = RIGHT_TRIGGER;
						}						
					}

					if (keyCode > -5)
					{
						mapDeviceButtonCode(buttonId, keyCode);
						buttonSet = true;
					}
				}

				break;

			default:
				break;
		}
	}

	return buttonSet;
}