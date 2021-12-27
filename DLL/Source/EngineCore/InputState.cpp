#include "..\..\Headers\EngineCore\InputState.hpp"

using namespace firemelon;

InputState::InputState()
{
	
}

InputState::~InputState()
{
}

ButtonState InputState::getButtonState(GameButtonId buttonCode)
{
	return buttonStateMap_[buttonCode];
}

void InputState::setButtonState(GameButtonId buttonCode, ButtonState buttonState)
{
	buttonStateMap_[buttonCode] = buttonState;
}