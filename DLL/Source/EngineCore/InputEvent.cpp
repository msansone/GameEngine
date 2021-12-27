#include "..\..\Headers\EngineCore\InputEvent.hpp"

using namespace firemelon;

InputEvent::InputEvent()
{
	//std::cout << "Created input event" << std::endl;
	blockEntityInput_ = false;
	blockUiInput_ = false;
	ignoreUiInput_ = false;
}

InputEvent::~InputEvent()
{
	//std::cout << "Destroyed input event" << std::endl;
}

void InputEvent::setDeviceName(std::string deviceName)
{
	deviceName_ = deviceName;
}

std::string InputEvent::getDeviceName()
{
	return deviceName_;
}

void InputEvent::setChannel(InputChannel channel)
{
	channel_ = channel;
}

InputChannel InputEvent::getChannel()
{
	return channel_;
}

void InputEvent::setButtonId(GameButtonId buttonId)
{
	buttonId_ = buttonId;
}

GameButtonId InputEvent::getButtonId()
{
	return buttonId_;
}