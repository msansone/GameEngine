#include "..\..\Headers\EngineCore\NetworkInputDevice.hpp"

using namespace firemelon;

NetworkInputDevice::NetworkInputDevice(std::string ipAddress) : InputDevice(ipAddress)
{
	inputState_ = boost::shared_ptr<InputState>(new InputState());
}

NetworkInputDevice::~NetworkInputDevice()
{
}

ButtonState NetworkInputDevice::getButtonState(GameButtonId buttonCode)
{
	ButtonState buttonState = inputState_->getButtonState(buttonCode);

	return buttonState;
}


boost::shared_ptr<InputState> NetworkInputDevice::getInputState()
{
	return inputState_;
}

bool NetworkInputDevice::userInitialize()
{
	boost::shared_ptr<GameButtonManager> buttonManager = getGameButtonManager();

	// Set the input state to all buttons up.
	int gameButtonCount = buttonManager->getGameButtonCount();

	for (int i = 0; i < gameButtonCount; i++)
	{
		inputState_->setButtonState(buttonManager->getGameButtonId(i), BUTTON_STATE_UP); 
	}

	return true;
}