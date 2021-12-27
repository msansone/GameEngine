#include "..\..\Headers\EngineCore\InputDevice.hpp"

using namespace firemelon;

int InputDevice::idCounter_ = 0;

InputDevice::InputDevice(std::string inputDeviceName)
{
	ignoreNextInputEvent_ = false;
	isConfiguring_ = false;
	isInitialized_ = false;
	isBlocked_ = false;
	hasInputStateChanged_ = false;

	deviceName_ = inputDeviceName;

	referenceCount_ = 0;

	channel_ = idCounter_;
	idCounter_++;

	inputState_ = boost::shared_ptr<InputState>(new InputState());
}

InputDevice::~InputDevice()
{
}

bool InputDevice::isConfiguring()
{
	return isConfiguring_;
}

bool InputDevice::getHasInputStateChanged()
{
	return hasInputStateChanged_;
}

bool InputDevice::preInitialize()
{
	std::map<GameButtonId, std::string>::iterator itr;

	for (itr = buttonManager_->gameButtonNameIdMap_.begin(); itr != buttonManager_->gameButtonNameIdMap_.end(); itr++)
	{
		inputState_->setButtonState(itr->first, BUTTON_STATE_UP);
	}

	return initialize();
}

void InputDevice::pollInputStatus()
{
	if (isBlocked_ == false)
	{
		// Test the state of each game button.
		std::map<GameButtonId, std::string>::iterator itr;

		bool inputEventOccurred = false;

		for (itr = buttonManager_->gameButtonNameIdMap_.begin(); itr != buttonManager_->gameButtonNameIdMap_.end(); itr++)
		{
			GameButtonId buttonId = itr->first;

			ButtonState buttonState = getButtonState(buttonId);
			
			if (inputState_->getButtonState(buttonId) != buttonState)
			{
				inputEventOccurred = true;

				if (ignoreNextInputEvent_ == false)
				{
					boost::shared_ptr<InputEvent> inputEvent = boost::shared_ptr<InputEvent>(new InputEvent());

					inputEvent->setDeviceName(deviceName_);
					inputEvent->setChannel(channel_);

					inputEvent->setButtonId(buttonId);

					// Button state has changed.
					if (buttonState == BUTTON_STATE_DOWN)
					{
						buttonDownSignal(inputEvent);
					}
					else
					{
						buttonUpSignal(inputEvent);
					}
				}

				inputState_->setButtonState(itr->first, buttonState);				
			}
		}

		if (inputEventOccurred == true && ignoreNextInputEvent_ == true)
		{
			std::cout << "Reseting the ignore next input event flag." << std::endl;
			ignoreNextInputEvent_ = false;
		}
	}

	// Now it is safe to change any button mappings. It isn't safe to do while looping through the
	// buttons, because you don't want to change the current state as you go through it.
	for (size_t i = 0; i < queuedButtonMappings_.size(); i++)
	{
		GameButtonId buttonId = queuedButtonMappings_[i].gameButtonId;

		int deviceButtonId = queuedButtonMappings_[i].deviceButtonId;

		gameButtonIdToDeviceButtonCodeMap_[buttonId] = deviceButtonId;
	}

	queuedButtonMappings_.clear();
}

boost::shared_ptr<GameButtonManager> InputDevice::getGameButtonManager()
{
	return buttonManager_;
}

int InputDevice::getDeviceButtonCode(GameButtonId buttonId)
{
	// First, check if this mapping is in the queue. If it is, return that value.
	for (size_t i = 0; i < queuedButtonMappings_.size(); i++)
	{
		GameButtonId queuedButtonId = queuedButtonMappings_[i].gameButtonId;

		if (queuedButtonId == buttonId)
		{
			return queuedButtonMappings_[i].deviceButtonId;
		}
	}

	// If it got here, it did not find it in the queue. Use the actual map to return the value.
	if (gameButtonIdToDeviceButtonCodeMap_.count(buttonId) > 0)
	{
		return gameButtonIdToDeviceButtonCodeMap_[buttonId];
	}
	else
	{
		return -1;
	}
}

void InputDevice::mapDeviceButtonCode(GameButtonId buttonId, int deviceButtonCode)
{
	// Changing the state of the buttons while in the middle of polling the input state
	// has undesirable results. Instead of setting it immediately, queue it up as an
	// action to be processed after the input polling is complete.

	ButtonMapping mapping;

	mapping.gameButtonId = buttonId;
	
	mapping.deviceButtonId = deviceButtonCode;

	queuedButtonMappings_.push_back(mapping);

	//gameButtonIdToDeviceButtonCodeMap_[buttonId] = deviceButtonCode;
}

void InputDevice::setKeyValue(GameButtonId buttonId, int buttonCode)
{
	gameButtonIdToDeviceButtonCodeMap_[buttonId] = buttonCode;
}

std::string InputDevice::getDeviceName()
{
	return deviceName_;
}

InputChannel InputDevice::getChannel()
{
	return channel_;
}

bool InputDevice::getIsInitialized()
{
	return isInitialized_;
}

void InputDevice::setDeviceName(std::string deviceName)
{
	deviceName_ = deviceName;
}

void InputDevice::setIsBlocked(bool value)
{
	isBlocked_ = value;
}

bool InputDevice::getIsBlocked()
{
	return isBlocked_;
}

std::string InputDevice::getDeviceButtonName(GameButtonId buttonCode)
{
	return "Unnamed Button";
}