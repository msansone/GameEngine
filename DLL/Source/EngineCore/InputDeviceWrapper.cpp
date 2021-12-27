#include "..\..\Headers\EngineCore\InputDeviceWrapper.hpp"

using namespace firemelon;

InputDeviceWrapper::InputDeviceWrapper()
{
}

InputDeviceWrapper::~InputDeviceWrapper()
{
}

bool InputDeviceWrapper::isConfiguringPy()
{
	PythonReleaseGil unlocker;

	return isConfiguring();
}

bool InputDeviceWrapper::isConfiguring()
{
	return device_->isConfiguring();
}

ButtonState	InputDeviceWrapper::getButtonStatePy(GameButtonId buttonCode)
{
	PythonReleaseGil unlocker;

	return getButtonState(buttonCode);
}

ButtonState	InputDeviceWrapper::getButtonState(GameButtonId buttonCode)
{
	return device_->getButtonState(buttonCode);
}

bool InputDeviceWrapper::configureButtonPy(GameButtonId buttonCode)
{
	PythonReleaseGil unlocker;

	return configureButton(buttonCode);
}

bool InputDeviceWrapper::configureButton(GameButtonId buttonCode)
{
	bool buttonConfigured = device_->configureButton(buttonCode);

	device_->isConfiguring_ = !buttonConfigured;

	if (buttonConfigured == true)
	{
		std::cout << "The button was configured. Ignore the next input polling where an event occurs." << std::endl;
		device_->ignoreNextInputEvent_ = true;
	}


	return buttonConfigured;
}
		
std::string InputDeviceWrapper::getDeviceButtonNamePy(GameButtonId buttonCode)
{
	PythonReleaseGil unlocker;

	return getDeviceButtonName(buttonCode);
}
		
std::string InputDeviceWrapper::getDeviceButtonName(GameButtonId buttonCode)
{
	return device_->getDeviceButtonName(buttonCode);
}

int InputDeviceWrapper::getDeviceButtonCodePy(GameButtonId buttonCode)
{
	PythonReleaseGil unlocker;

	return getDeviceButtonCode(buttonCode);
}

int InputDeviceWrapper::getDeviceButtonCode(GameButtonId buttonCode)
{
	return device_->getDeviceButtonCode(buttonCode);
}

void InputDeviceWrapper::mapDeviceButtonCodePy(GameButtonId buttonCode, int deviceButtonCode)
{
	PythonReleaseGil unlocker;

	mapDeviceButtonCode(buttonCode, deviceButtonCode);
}

void InputDeviceWrapper::mapDeviceButtonCode(GameButtonId buttonCode, int deviceButtonCode)
{
	device_->setKeyValue(buttonCode, deviceButtonCode);
}

void InputDeviceWrapper::setKeyValuePy(GameButtonId buttonCode, int keyValue)
{
	PythonReleaseGil unlocker;

	setKeyValue(buttonCode, keyValue);
}

void InputDeviceWrapper::setKeyValue(GameButtonId buttonCode, int keyValue)
{
	device_->setKeyValue(buttonCode, keyValue);
}

InputChannel InputDeviceWrapper::getChannelPy()
{
	PythonReleaseGil unlocker;

	return getChannel();
}

InputChannel InputDeviceWrapper::getChannel()
{
	return device_->getChannel();
}

std::string InputDeviceWrapper::getDeviceNamePy()
{
	PythonReleaseGil unlocker;

	return getDeviceName();
}

std::string InputDeviceWrapper::getDeviceName()
{
	return device_->getDeviceName();
}

bool InputDeviceWrapper::getIsInitializedPy()
{
	PythonReleaseGil unlocker;

	return getIsInitialized();
}

bool InputDeviceWrapper::getIsInitialized()
{
	return device_->getIsInitialized();
}

void InputDeviceWrapper::setIsBlockedPy(bool value)
{
	PythonReleaseGil unlocker;

	setIsBlocked(value);
}

void InputDeviceWrapper::setIsBlocked(bool value)
{
	device_->setIsBlocked(value);
}

bool InputDeviceWrapper::getIsBlockedPy()
{
	PythonReleaseGil unlocker;

	return getIsBlocked();
}

bool InputDeviceWrapper::getIsBlocked()
{
	return device_->getIsBlocked();
}

boost::shared_ptr<InputDevice> InputDeviceWrapper::getDevice()
{
	return device_;
}