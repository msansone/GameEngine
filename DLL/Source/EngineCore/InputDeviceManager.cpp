#include "..\..\Headers\EngineCore\InputDeviceManager.hpp"

using namespace firemelon;

InputDeviceManager::InputDeviceManager(boost::shared_ptr<GameButtonManager> buttonManager, boost::shared_ptr<InputDeviceContainer> inputDeviceContainer)
{
	buttonManager_ = buttonManager;
	inputDeviceContainer_ = inputDeviceContainer;
	blockEntityInput_ = false;
	disableUi_ = false;
	defaultInputChannel_ = 0;
}

InputDeviceManager::~InputDeviceManager()
{
}

// Takes a user created InputDevice object, initializes it, and adds it
// to a map, which maps a string value to the device object.
void InputDeviceManager::addInputDevice(boost::shared_ptr<InputDevice> inputDevice)
{
	// If this device hasn't been added to the container yet, initialize it and add it here.
	int index = -1;

	if (inputDeviceContainer_->deviceExists(inputDevice->getChannel()) == false)
	{
		boost::shared_ptr<InputDeviceWrapper> newWrapper = boost::shared_ptr<InputDeviceWrapper>(new InputDeviceWrapper());
		newWrapper->device_ = inputDevice;

		inputDevice->buttonManager_ = buttonManager_;
	
		bool initializeSuccess = inputDevice->preInitialize();
		
		inputDevice->isInitialized_ = initializeSuccess;

		if (initializeSuccess == true)
		{
			index = inputDeviceContainer_->addInputDevice(newWrapper);
		}
	}
	else
	{
		int size = inputDeviceContainer_->getInputDeviceCount();

		for (int i = 0; i < size; i++)
		{
			if (inputDeviceContainer_->getInputDevice(i)->getChannel() == inputDevice->getChannel())
			{
				index = i;
				break;
			}
		}
	}

	// At this point the input device is guaranteed to be in the container, located at index.

	// If this device is already contained in this manager, don't add it.
	bool doAdd = true;

	int size = containedInputDeviceIndexes_.size();

	for (int i = 0 ; i < size; i++)
	{
		if (containedInputDeviceIndexes_[i] == index)
		{
			// Already exists.
			doAdd = false;
			break;
		}
	}

	if (doAdd == true)
	{
		// If a device with this name already exists, don't add it.
		std::string deviceName = inputDevice->getDeviceName();

		inputDevice->referenceCount_++;

		int inputDeviceIndex = inputDeviceNameIdMap_[deviceName] - 1;

		if (inputDeviceIndex < 0)
		{
			// Connect the signal from the input device to this manager.
			inputDevice->buttonDownSignal.connect(boost::bind(&InputDeviceManager::preButtonDown, this, _1));
			inputDevice->buttonUpSignal.connect(boost::bind(&InputDeviceManager::preButtonUp, this, _1));

			containedInputDeviceIndexes_.push_back(index);

			inputDeviceNameIdMap_[deviceName] = containedInputDeviceIndexes_.size();
		}
	}
}

void InputDeviceManager::removeInputDeviceByChannel(InputChannel channel, bool deleteFromContainer)
{
	// If the device exists, get its index. Look that index up in the contained input devices list
	// and remove it. 
	int index = -1;

	if (inputDeviceContainer_->deviceExists(channel) == true)
	{
		int size = inputDeviceContainer_->getInputDeviceCount();

		for (int i = 0; i < size; i++)
		{
			if (inputDeviceContainer_->getInputDevice(i)->getChannel() == channel)
			{
				index = i;
				break;
			}
		}
	}

	// If this device is not contained in this manager, don't remove it.
	bool doRemove = false;

	int size = containedInputDeviceIndexes_.size();
	
	int i;

	for (i = 0 ; i < size; i++)
	{
		if (containedInputDeviceIndexes_[i] == index)
		{
			// Exists.
			doRemove = true;
			break;
		}
	}

	if (doRemove == true)
	{
		// If a device with this name already exists, don't add it.
		boost::shared_ptr<InputDeviceWrapper> inputDevice = inputDeviceContainer_->getInputDevice(index);

		std::string deviceName = inputDevice->getDeviceName();
		
		inputDevice->device_->referenceCount_--;

		// Connect the signal from the input device to this manager.
		inputDevice->device_->buttonDownSignal.disconnect(boost::bind(&InputDeviceManager::preButtonDown, this, _1));
		inputDevice->device_->buttonUpSignal.disconnect(boost::bind(&InputDeviceManager::preButtonUp, this, _1));

		inputDeviceNameIdMap_.erase(deviceName);

		containedInputDeviceIndexes_.erase(containedInputDeviceIndexes_.begin() + i);

		if (deleteFromContainer == true)
		{
			if (inputDevice->device_->referenceCount_ == 0)
			{
				inputDeviceContainer_->removeInputDevice(index);
			}
		}
	}
}

boost::shared_ptr<GameButtonManager> InputDeviceManager::getGameButtonManagerPy()
{
	PythonReleaseGil unlocker;

	return getGameButtonManager();
}

boost::shared_ptr<GameButtonManager> InputDeviceManager::getGameButtonManager()
{
	return buttonManager_;
}

// Poll the input status for the currently active input device object.
// This will dispatch any input events to this object. In the future,
// maybe enhance this functionality to handle multiple inputdevice objects
// for multiplayer.
void InputDeviceManager::pollInputStatus()
{
	int size = containedInputDeviceIndexes_.size();

	for (int i = 0; i < size; i++)	
	{
		int index = containedInputDeviceIndexes_[i];

		boost::shared_ptr<InputDevice> device = inputDeviceContainer_->getInputDevice(index)->device_;

		device->pollInputStatus();		
	}
}

int InputDeviceManager::getInputDeviceCountPy()
{
	PythonReleaseGil unlocker;

	return getInputDeviceCount();
}

int InputDeviceManager::getInputDeviceCount()
{
	return containedInputDeviceIndexes_.size();
}

boost::shared_ptr<InputDevice> InputDeviceManager::getInputDeviceByIndex(int index)
{
	int size = containedInputDeviceIndexes_.size();

	if (index >= 0 && index < size)
	{
		int containerIndex = containedInputDeviceIndexes_[index];

		return inputDeviceContainer_->getInputDevice(containerIndex)->device_;
	}
	else
	{
		return nullptr;
	}
}

boost::shared_ptr<InputDevice> InputDeviceManager::getInputDeviceByChannel(InputChannel channel)
{
	int size = containedInputDeviceIndexes_.size();

	for (int i = 0; i < size; i++)
	{
		int containerIndex = containedInputDeviceIndexes_[i];

		boost::shared_ptr<InputDevice> inputDevice = inputDeviceContainer_->getInputDevice(containerIndex)->device_;

		if (inputDevice->getChannel() == channel)
		{
			return inputDevice;
		}
	}

	return nullptr;
}

boost::shared_ptr<InputDeviceWrapper> InputDeviceManager::getInputDeviceWrapperByIndexPy(int index)
{
	PythonReleaseGil unlocker;

	return getInputDeviceWrapperByIndex(index);
}

boost::shared_ptr<InputDeviceWrapper> InputDeviceManager::getInputDeviceWrapperByIndex(int index)
{
	int size = containedInputDeviceIndexes_.size();

	if (index >= 0 && index < size)
	{
		int containerIndex = containedInputDeviceIndexes_[index];

		return inputDeviceContainer_->getInputDevice(containerIndex);
	}
	else
	{
		return nullptr;
	}
}

boost::shared_ptr<InputDeviceWrapper> InputDeviceManager::getInputDeviceWrapperByChannelPy(InputChannel channel)
{
	PythonReleaseGil unlocker;

	return getInputDeviceWrapperByChannel(channel);
}

boost::shared_ptr<InputDeviceWrapper> InputDeviceManager::getInputDeviceWrapperByChannel(InputChannel channel)
{
	int size = containedInputDeviceIndexes_.size();

	for (int i = 0; i < size; i++)
	{
		int containerIndex = containedInputDeviceIndexes_[i];

		boost::shared_ptr<InputDeviceWrapper> inputDeviceWrapper = inputDeviceContainer_->getInputDevice(containerIndex);

		if (inputDeviceWrapper->getChannel() == channel)
		{
			return inputDeviceWrapper;
		}
	}
	
	return nullptr;
}

boost::shared_ptr<InputDevice> InputDeviceManager::getInputDeviceByName(std::string deviceName)
{
	return getInputDeviceByIndex(inputDeviceNameIdMap_[deviceName] - 1);
}


boost::shared_ptr<InputDeviceWrapper> InputDeviceManager::getInputDeviceWrapperByNamePy(std::string deviceName)
{
	PythonReleaseGil unlocker;

	return getInputDeviceWrapperByName(deviceName);
}

boost::shared_ptr<InputDeviceWrapper> InputDeviceManager::getInputDeviceWrapperByName(std::string deviceName)
{
	return getInputDeviceWrapperByIndex(inputDeviceNameIdMap_[deviceName] - 1);
}

// This method is bound to the buttonDown signal in the inputdevice
// objects. After an input event occurs, it will be pre-processed here
// before being dispatched to any listening objects via the 
// buttonDownSignal.
void InputDeviceManager::preButtonDown(boost::shared_ptr<InputEvent> inputEvent)
{
	// Get the input device name that this event originated in.
	std::string inputDeviceName = inputEvent->getDeviceName();
	//InputChannel channel = inputEvent->getChannel();
	//GameButtonId buttonCode = inputEvent->getButtonId();

	inputEvent->blockEntityInput_ = blockEntityInput_;

	inputEvent->blockUiInput_ = disableUi_;

	//// If the device exists and is active, pass the buttonCode to the signal.
	int deviceIndex = inputDeviceNameIdMap_[inputDeviceName] - 1;

	int size = inputDeviceContainer_->getInputDeviceCount();

	if (deviceIndex >= 0 && deviceIndex < size)
	{
		//boost::shared_ptr<InputDevice> inputDevice = inputDeviceContainer_->getInputDevice(deviceIndex)->device_;		
		buttonDownSignal_(inputEvent);		
	}
}

// This method is bound to the buttonUp signal in the inputdevice
// objects. After an input event occurs, it will be pre-processed here
// before being dispatched to any listening objects via the 
// buttonUpSignal.
void InputDeviceManager::preButtonUp(boost::shared_ptr<InputEvent> inputEvent)
{
	// Get the input device name that this event originated in.
	std::string inputDeviceName = inputEvent->getDeviceName();
	//InputChannel channel = inputEvent->getChannel();
	//GameButtonId buttonCode = inputEvent->getButtonId();

	inputEvent->blockEntityInput_ = blockEntityInput_;

	inputEvent->blockUiInput_ = disableUi_;

	// If the device exists and is active, pass along the signal.
	int deviceIndex = inputDeviceNameIdMap_[inputDeviceName] - 1;
	
	int size = inputDeviceContainer_->getInputDeviceCount();

	if (deviceIndex >= 0 && deviceIndex < size)
	{
		//boost::shared_ptr<InputDevice> inputDevice = inputDeviceContainer_->getInputDevice(deviceIndex)->device_;

		buttonUpSignal_(inputEvent);
	}
}

boost::shared_ptr<InputDeviceContainer> InputDeviceManager::getInputDeviceContainer()
{
	return inputDeviceContainer_;
}

void InputDeviceManager::changeInputChannelOfListenersPy(InputChannel oldChannel, InputChannel newChannel)
{
	PythonReleaseGil unlocker;

	changeChannelOfListenersSignal_(oldChannel, newChannel);
}

void InputDeviceManager::changeInputChannelOfListeners(InputChannel oldChannel, InputChannel newChannel)
{
	changeChannelOfListenersSignal_(oldChannel, newChannel);
}

void InputDeviceManager::setBlockEntityInputPy(bool blockEntityInput)
{
	PythonReleaseGil unlocker;

	setBlockEntityInput(blockEntityInput);
}

void InputDeviceManager::setBlockEntityInput(bool blockEntityInput)
{
	blockEntityInput_ = blockEntityInput;
}

void InputDeviceManager::disableUiInputPy()
{
	PythonReleaseGil unlocker;

	disableUiInput();
}

void InputDeviceManager::disableUiInput()
{
	disableUi_ = true;
}

void InputDeviceManager::enableUiInputPy()
{
	PythonReleaseGil unlocker;

	enableUiInput();
}

void InputDeviceManager::enableUiInput()
{
	disableUi_ = false;
}

void InputDeviceManager::setDefaultInputChannelPy(InputChannel channel)
{
	PythonReleaseGil unlocker;

	setDefaultInputChannel(channel);
}

void InputDeviceManager::setDefaultInputChannel(InputChannel channel)
{
	defaultInputChannel_ = channel;
}

bool InputDeviceManager::getBlockEntityInput()
{
	return blockEntityInput_;
}

bool InputDeviceManager::getBlockEntityInputPy()
{
	PythonReleaseGil unlocker;

	return getBlockEntityInput();
}

bool InputDeviceManager::getDisableUiInput()
{
	return disableUi_;
}

bool InputDeviceManager::getDisableUiInputPy()
{
	PythonReleaseGil unlocker;

	return getDisableUiInput();
}

void InputDeviceManager::disableEntityInputPy(int entityInstanceId)
{
	PythonReleaseGil unlocker;

	disableEntityInput(entityInstanceId);
}

void InputDeviceManager::disableEntityInput(int entityInstanceId)
{
	disabledEntities_.emplace(entityInstanceId);

	return;
}

void InputDeviceManager::enableEntityInputPy(int entityInstanceId)
{
	PythonReleaseGil unlocker;

	enableEntityInput(entityInstanceId);
}

void InputDeviceManager::enableEntityInput(int entityInstanceId)
{
	disabledEntities_.erase(entityInstanceId);

	return;
}

bool InputDeviceManager::isEntityInputEnabled(int entityInstanceId)
{
	if (disabledEntities_.find(entityInstanceId) != disabledEntities_.end())
	{
		return false;
	}
	else
	{
		return true;
	}
}