#include "..\Headers\FiremelonExInputReceiverCodeBehind.hpp"

using namespace boost::python;
using namespace firemelon;

//int FiremelonExInputReceiverCodeBehind::idCounter_ = 0;

FiremelonExInputReceiverCodeBehind::FiremelonExInputReceiverCodeBehind()
{
	keyDownExists_ = false;
	keyUpExists_ = false;

	//idCounter_++;
	//id_ = idCounter_;
}

FiremelonExInputReceiverCodeBehind::~FiremelonExInputReceiverCodeBehind()
{
}

void FiremelonExInputReceiverCodeBehind::initialize()
{
	std::string pyTypeName = this->getPythonInstanceWrapper()->getInstanceTypeName();

	try
	{
		PythonAcquireGil lock;

		// Get the instance for this object.
		object pyEntityInstance = this->getPythonInstanceWrapper()->getPyInstance();
		object pEntityNamespace = pyEntityInstance.attr("__dict__");

		// Import firemelon_ex module to the instance.
		object pyFiremelonExModule((handle<>(PyImport_ImportModule("firemelon_ex"))));
		pEntityNamespace["firemelon_ex"] = pyFiremelonExModule;

		boost::shared_ptr<InputDeviceManager> inputDeviceManager = getInputDeviceManager();

		if (inputDeviceManager != nullptr)
		{
			keyboardDevice_->keyDownSignal_.connect(boost::bind(&FiremelonExInputReceiverCodeBehind::keyDown, this, _1));
			keyboardDevice_->keyUpSignal_.connect(boost::bind(&FiremelonExInputReceiverCodeBehind::keyUp, this, _1));

			if (PyObject_HasAttrString(pyEntityInstance.ptr(), "keyUp") == true)
			{
				keyUpExists_ = true;
				pyKeyUp_ = pyEntityInstance.attr("keyUp");
			}

			if (PyObject_HasAttrString(pyEntityInstance.ptr(), "keyDown") == true)
			{
				keyDownExists_ = true;
				pyKeyDown_ = pyEntityInstance.attr("keyDown");
			}
		}
	}
	catch (error_already_set &)
	{
		std::cout << "Error loading Entity " << pyTypeName << std::endl;
		//handlePythonError();
	}
}

void FiremelonExInputReceiverCodeBehind::attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice)
{
	keyboardDevice_ = keyboardDevice;
}

void FiremelonExInputReceiverCodeBehind::keyDown(SDL_Keycode keyCode)
{
	if (keyDownExists_ == true)
	{
		try
		{
			PythonAcquireGil lock;

			std::cout << "Key down" << keyCode << std::endl;

 			pyKeyDown_(keyCode);
		}
		catch (error_already_set &)
		{
			std::cout << "Error in key down." << std::endl;
		}
	}
}

void FiremelonExInputReceiverCodeBehind::keyUp(SDL_Keycode keyCode)
{
	if (keyUpExists_ == true)
	{
		PythonAcquireGil lock;

		pyKeyUp_(keyCode);
	}
}

void FiremelonExInputReceiverCodeBehind::cleanup()
{
	boost::shared_ptr<InputDeviceManager> inputDeviceManager = getInputDeviceManager();

	if (inputDeviceManager != nullptr)
	{
		keyboardDevice_->keyDownSignal_.disconnect(boost::bind(&FiremelonExInputReceiverCodeBehind::keyDown, this, _1));
		keyboardDevice_->keyUpSignal_.disconnect(boost::bind(&FiremelonExInputReceiverCodeBehind::keyUp, this, _1));
	}
}
