#include "..\Headers\FiremelonExUiWidget.hpp"

using namespace boost::python;
using namespace firemelon;

FiremelonExUiWidget::FiremelonExUiWidget(std::string widgetName) : UiWidget(widgetName)
{
	keyDownExists_ = false;
	keyUpExists_ = false;
}

FiremelonExUiWidget::~FiremelonExUiWidget()
{
}

void FiremelonExUiWidget::pythonDataInitialized()
{
	PythonAcquireGil lock;

	std::string pyTypeName = getScriptTypeName();
	
	try
	{
		// Get the instance for this object.
		object pyEntityInstance = getPyInstance();
		object pEntityNamespace = pyEntityInstance.attr("__dict__");
	
		// Import firemelon_ex module to the instance.
		object pyFiremelonExModule((handle<>(PyImport_ImportModule("firemelon_ex"))));
		pEntityNamespace["firemelon_ex"] = pyFiremelonExModule;

		//BoostGameTimer* timer = (BoostGameTimer*)getTimer();

		//pEntityNamespace["timer"] = ptr(timer);

		boost::shared_ptr<InputDeviceManager> inputDeviceManager = getInputDeviceManager();

		if (inputDeviceManager != nullptr)
		{
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
	catch(error_already_set &)
	{
		std::cout<<"Error loading Entity "<< pyTypeName <<std::endl;
		//handlePythonError();
	}
}


void FiremelonExUiWidget::attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice)
{
	keyboardDevice_ = keyboardDevice;
}

void FiremelonExUiWidget::keyDown(SDL_Keycode keyCode)
{
	PythonAcquireGil lock;

	try
	{
		if (keyDownExists_ == true)
		{
			pyKeyDown_(keyCode);
		}
	}
	catch(error_already_set &)
	{
		std::string pyTypeName = this->getScriptTypeName();
		std::cout<<"Error in UI widget key press "<< pyTypeName <<std::endl;
		
		try
		{
			PyObject *ptype, *pvalue, *ptraceback;
			PyErr_Fetch(&ptype, &pvalue, &ptraceback);
	
			//Extract error message
			std::string strErrorMessage = extract<std::string>(pvalue);

			handle<> hType(ptype);
			object extype(hType);

			if (ptraceback != nullptr)
			{
				handle<> hTraceback(ptraceback);
				object traceback(hTraceback);

				//Extract line number (top entry of call stack)
				// if you want to extract another levels of call stack
				// also process traceback.attr("tb_next") recurently
				long lineno = extract<long> (traceback.attr("tb_lineno"));
				std::string filename = extract<std::string>(traceback.attr("tb_frame").attr("f_code").attr("co_filename"));
				std::string funcname = extract<std::string>(traceback.attr("tb_frame").attr("f_code").attr("co_name"));

				std::cout<<"Python error: "<<strErrorMessage<<std::endl
			 			 <<"Line "<<lineno<<std::endl
						 <<"File "<<filename<<std::endl
						 <<"Function "<<funcname<<std::endl;
			}
			else
			{
				std::cout<<"Python error: "<<strErrorMessage<<std::endl
						 <<"File data not available"<<std::endl;
			}
		}
		catch (error_already_set &)
		{
			// For some reason the PyErr_Fetch errors sometimes.
			// Hopefully it doesn't error a second time.
			PyObject *ptype, *pvalue, *ptraceback;
			PyErr_Fetch(&ptype, &pvalue, &ptraceback);
	
			//Extract error message
			std::string strErrorMessage = extract<std::string>(pvalue);

			handle<> hType(ptype);
			object extype(hType);
		
			if (ptraceback != nullptr)
			{
				handle<> hTraceback(ptraceback);
				object traceback(hTraceback);

				//Extract line number (top entry of call stack)
				// if you want to extract another levels of call stack
				// also process traceback.attr("tb_next") recurently
				long lineno = extract<long> (traceback.attr("tb_lineno"));
				std::string filename = extract<std::string>(traceback.attr("tb_frame").attr("f_code").attr("co_filename"));
				std::string funcname = extract<std::string>(traceback.attr("tb_frame").attr("f_code").attr("co_name"));

				std::cout<<"Python error: "<<strErrorMessage<<std::endl
			 			 <<"Line "<<lineno<<std::endl
						 <<"File "<<filename<<std::endl
						 <<"Function "<<funcname<<std::endl;
			}
			else
			{
				std::cout<<"Python error: "<<strErrorMessage<<std::endl
						 <<"File data not available"<<std::endl;
			}
		}

	}
}

void FiremelonExUiWidget::keyUp(SDL_Keycode keyCode)
{
	PythonAcquireGil lock;

	try
	{
		if (keyUpExists_ == true)
		{
			pyKeyUp_(keyCode);
		}
	}
	catch(error_already_set &)
	{
		std::string pyTypeName = this->getScriptTypeName();
		std::cout<<"Error in UI widget key press "<< pyTypeName <<std::endl;
		//handlePythonError();
	}
}

//void FiremelonExUiWidget::preDestroy()
//{
//	//InputDeviceManager* inputDeviceManager = getInputDeviceManager();
//
//	//if (inputDeviceManager != nullptr)
//	//{
//	//	keyboardDevice_->keyDownSignal_.disconnect(boost::bind(&FiremelonExEntity::keyDown, this, _1));
//	//	keyboardDevice_->keyUpSignal_.disconnect(boost::bind(&FiremelonExEntity::keyUp, this, _1));
//	//}
//}
