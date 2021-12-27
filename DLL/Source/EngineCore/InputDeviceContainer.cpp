#include "..\..\Headers\EngineCore\InputDeviceContainer.hpp"

using namespace firemelon;

InputDeviceContainer::InputDeviceContainer()
{
}

InputDeviceContainer::~InputDeviceContainer()
{
	inputDevices_.clear();
}

int InputDeviceContainer::getInputDeviceCount()
{
	return inputDevices_.size();
}

boost::shared_ptr<InputDeviceWrapper> InputDeviceContainer::getInputDevice(int index)
{
	int size = getInputDeviceCount();

	if (index >= 0 && index < size)
	{
		return inputDevices_[index];
	}

	return nullptr;
}

int InputDeviceContainer::addInputDevice(boost::shared_ptr<InputDeviceWrapper> inputDevice)
{
	// If an input device with this channel already exists, don't add it.
	InputChannel channel = inputDevice->getChannel();

	int size = inputDevices_.size();

	for (int i = 0; i < size; i++)
	{
		if (inputDevices_[i]->getChannel() == channel)
		{
			return i;
		}
	}

	inputDevices_.push_back(inputDevice);

	return inputDevices_.size() - 1;
}

void InputDeviceContainer::removeInputDevice(int index)
{
	int size = inputDevices_.size();

	if (index >= 0 && index < size)
	{
		inputDevices_.erase(inputDevices_.begin() + index);
	}
}

bool InputDeviceContainer::deviceExists(InputChannel channel)
{
	int size = inputDevices_.size();

	for (int i = 0; i < size; i++)
	{
		if (inputDevices_[i]->getChannel() == channel)
		{
			return true;
		}
	}

	return false;
}