#include "..\..\Headers\EngineCore\InputReceiverCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

InputReceiverCodeBehind::InputReceiverCodeBehind()
{
	engineController_ = nullptr;
}

InputReceiverCodeBehind::~InputReceiverCodeBehind()
{
}

void InputReceiverCodeBehind::preButtonDown(boost::shared_ptr<InputEvent> inputEvent)
{
	//// If the UI is not visible, the next input event it receives should be ignored.
	//// If it remains invisible, this will have no effect. If it becomes visible
	//// as a result of the entity input response, it will correct a bug where
	//// the input feels like it is incorrectly being pressed twice, because the
	//// same input event will be sent to both this entity and the UI. From the
	//// perspective of the user, the button press that shows the UI should not
	//// also perform whatever response is in the UI item that gets shown.

	//// Likewise, if the UI is showing, don't process entity input events.
	//if (ui_->getIsShowing() == false)
	//{
		//inputEvent->ignoreUiInput_ = true;

	int instanceId = getMetadata()->getEntityInstanceId();

	if (getInputDeviceManager()->isEntityInputEnabled(instanceId) == true)
	{
		InputChannel inputChannel = inputEvent->getChannel();
		GameButtonId buttonId = inputEvent->getButtonId();

		if (inputReceiverScript_->inputChannel_ == inputChannel)
		{
			bool suspendUpdates = false;

			if (engineController_ != nullptr)
			{
				suspendUpdates = engineController_->getIsSimulationStopped();
			}

			if (suspendUpdates == false)
			{
				buttonDown(buttonId);
			}
		}
	}
	//}
}

void InputReceiverCodeBehind::preButtonUp(boost::shared_ptr<InputEvent> inputEvent)
{
	int instanceId = getMetadata()->getEntityInstanceId();

	if (getInputDeviceManager()->isEntityInputEnabled(instanceId) == true)
	{
		InputChannel inputChannel = inputEvent->getChannel();
		GameButtonId buttonId = inputEvent->getButtonId();

		if (inputReceiverScript_->inputChannel_ == inputChannel)
		{
			bool suspendUpdates = false;

			if (engineController_ != nullptr)
			{
				suspendUpdates = engineController_->getIsSimulationStopped();
			}

			if (suspendUpdates == false)
			{
				buttonUp(buttonId);
			}
		}
	}
}

int	InputReceiverCodeBehind::getInputChannel()
{
	return inputReceiverScript_->inputChannel_;
}

void InputReceiverCodeBehind::setInputChannel(InputChannel inputChannel)
{
	inputReceiverScript_->setInputChannel(inputChannel);
}

void InputReceiverCodeBehind::changeInputChannel(InputChannel oldInputChannel, InputChannel newInputChannel)
{
	if (inputReceiverScript_->inputChannel_ == oldInputChannel)
	{
		inputReceiverScript_->setInputChannel(newInputChannel);
	}
}

void InputReceiverCodeBehind::buttonDown(GameButtonId buttonId)
{
	inputReceiverScript_->buttonDown(buttonId);
}

void InputReceiverCodeBehind::buttonUp(GameButtonId buttonId)
{
	inputReceiverScript_->buttonUp(buttonId);
}

void InputReceiverCodeBehind::preInitialize()
{
	inputReceiverScript_ = boost::shared_ptr<InputReceiverScript>(new InputReceiverScript(debugger_));

	getInputDeviceManager()->changeChannelOfListenersSignal_.connect(boost::bind(&InputReceiverCodeBehind::changeInputChannel, this, _1, _2));

	inputReceiverScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	setInputChannel(getInputDeviceManager()->defaultInputChannel_);

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		inputReceiverScript_->preInitialize();
	}

	initialize();
}

void InputReceiverCodeBehind::initialize()
{
}

void InputReceiverCodeBehind::preCleanup()
{
	getInputDeviceManager()->changeChannelOfListenersSignal_.disconnect(boost::bind(&InputReceiverCodeBehind::changeInputChannel, this, _1, _2));

	cleanup();

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		inputReceiverScript_->preCleanup();
	}
}

void InputReceiverCodeBehind::cleanup()
{
}