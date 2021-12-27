#include "..\Headers\FiremelonExUi.hpp"

using namespace firemelon;
using namespace boost::python;

FiremelonExUi::FiremelonExUi()
{
	//keyUpExists_ = false;
	//keyDownExists_ = false;
	inputDeviceAddedExists_ = false;
	inputDeviceRemovedExists_ = false;
}

FiremelonExUi::~FiremelonExUi()
{
}

void FiremelonExUi::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		pyInputDeviceAdded_ = boost::python::object();
		pyInputDeviceRemoved_ = boost::python::object();
		pyKeyUp_ = boost::python::object();
		pyKeyDown_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		std::cout << "Error destroying UI manager." << std::endl;
	}

}

void FiremelonExUi::initialize()
{
	PythonAcquireGil lock;

	try
	{
		// Get the instance for this object.
		object pyUiManagerInstance = getPyInstance();
		object pyUiManagerNamespace = pyUiManagerInstance.attr("__dict__");
	
		// Import firemelon_ex module to the instance.
		object pyFiremelonExModule((handle<>(PyImport_ImportModule("firemelon_ex"))));
		pyUiManagerNamespace["firemelon_ex"] = pyFiremelonExModule;

		boost::shared_ptr<OpenGlRenderer> renderer = boost::static_pointer_cast<OpenGlRenderer>(getRenderer());

		pyUiManagerNamespace["renderer"] = ptr(renderer.get());

		keyboardDevice_->keyDownSignal_.connect(boost::bind(&FiremelonExUi::keyDown, this, _1));
		keyboardDevice_->keyUpSignal_.connect(boost::bind(&FiremelonExUi::keyUp, this, _1));

		if (PyObject_HasAttrString(pyUiManagerInstance.ptr(), "inputDeviceAdded") == true)
		{
			inputDeviceAddedExists_ = true;
			pyInputDeviceAdded_ = pyUiManagerInstance.attr("inputDeviceAdded");
		}

		if (PyObject_HasAttrString(pyUiManagerInstance.ptr(), "inputDeviceRemoved") == true)
		{
			inputDeviceRemovedExists_ = true;
			pyInputDeviceRemoved_ = pyUiManagerInstance.attr("inputDeviceRemoved");
		}

		boost::shared_ptr<InputDeviceManager> inputDeviceManager = getInputDeviceManager();

		if (inputDeviceManager != nullptr)
		{
			//keyboardDevice_->keyDownSignal_.connect(boost::bind(&FiremelonExUiManager::keyDown, this, _1));
			//keyboardDevice_->keyUpSignal_.connect(boost::bind(&FiremelonExUiManager::keyUp, this, _1));

			if (PyObject_HasAttrString(pyUiManagerInstance.ptr(), "keyUp") == true)
			{
				keyUpExists_ = true;
				pyKeyUp_ = pyUiManagerInstance.attr("keyUp");
			}

			if (PyObject_HasAttrString(pyUiManagerInstance.ptr(), "keyDown") == true)
			{
				keyDownExists_ = true;
				pyKeyDown_ = pyUiManagerInstance.attr("keyDown");
			}
		}

		//if (PyObject_HasAttrString(pyUiManagerInstance.ptr(), "keyUp") == true)
		//{
		//	keyUpExists_ = true;
		//	pyKeyUp_ = pyUiManagerInstance.attr("keyUp");
		//}

		//if (PyObject_HasAttrString(pyUiManagerInstance.ptr(), "keyDown") == true)
		//{
		//	keyDownExists_ = true;
		//	pyKeyDown_ = pyUiManagerInstance.attr("keyDown");
		//}

	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading UI manager."<<std::endl;		
	}
}


void FiremelonExUi::attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice)
{
	keyboardDevice_ = keyboardDevice;
}

void FiremelonExUi::keyDown(SDL_Keycode keyCode)
{
	boost::shared_ptr<FiremelonExUiWidget> focusedWidget = boost::static_pointer_cast<FiremelonExUiWidget>(getFocusedWidget());

	if (focusedWidget != nullptr)
	{
		focusedWidget->keyDown(keyCode);
	}

	try
	{
		if (keyDownExists_ == true)
		{
			PythonAcquireGil lock;

			pyKeyDown_(keyCode);
		}
	}
	catch(error_already_set &)
	{
		std::cout<<"Error in UI manager key down."<<std::endl;
		//DebugHelper::handlePythonError();
	}
}

void FiremelonExUi::keyUp(SDL_Keycode keyCode)
{
	boost::shared_ptr<FiremelonExUiWidget> focusedWidget = boost::static_pointer_cast<FiremelonExUiWidget>(getFocusedWidget());

	if (focusedWidget != nullptr)
	{
		focusedWidget->keyUp(keyCode);
	}

	try
	{
		if (keyUpExists_ == true)
		{
			PythonAcquireGil lock;

			pyKeyUp_(keyCode);
		}
	}
	catch(error_already_set &)
	{
		std::cout<<"Error in UI manager key up."<<std::endl;
		//DebugHelper::handlePythonError();
	}
}

void FiremelonExUi::shutdown()
{
	keyboardDevice_->keyDownSignal_.disconnect(boost::bind(&FiremelonExUi::keyDown, this, _1));
	keyboardDevice_->keyUpSignal_.disconnect(boost::bind(&FiremelonExUi::keyUp, this, _1));
}

void FiremelonExUi::inputDeviceAdded(int channel)
{	
	try
	{
		if (inputDeviceAddedExists_ == true)
		{
			PythonAcquireGil lock;

			pyInputDeviceAdded_(channel);
		}
	}
	catch(error_already_set &)
	{
		std::cout<<"Error in UI manager inputDeviceAdded function."<<std::endl;
		//handlePythonError();
	}
}

void FiremelonExUi::inputDeviceRemoved(int channel)
{
	try
	{
		if (inputDeviceRemovedExists_ == true)
		{
			PythonAcquireGil lock;

			pyInputDeviceRemoved_(channel);
		}
	}
	catch (error_already_set &)
	{
		std::cout << "Error in UI manager inputDeviceRemoved function." << std::endl;
		//handlePythonError();
	}
}
