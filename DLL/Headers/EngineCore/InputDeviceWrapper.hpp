/* -------------------------------------------------------------------------
** InputDeviceWrapper.hpp
**
** The InputDeviceWrapper class is the a wrapper around the InputDevice class.
** It is needed because input devices are defined by the user, and thus
** Python scripts do not be able to use them correctly. Instead
** this wrapper class can be defined, which will store a reference to the
** input device instance, and act as an intermediary between the device and
** the a python object.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _INPUTDEVICEWRAPPER_HPP_
#define _INPUTDEVICEWRAPPER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "InputDevice.hpp"
#include "GameButtonManager.hpp"

namespace firemelon
{
	class FIREMELONAPI InputDeviceWrapper
	{
	public:
		friend class InputDeviceManager;
		friend class InputDeviceContainer;

		InputDeviceWrapper();
		virtual ~InputDeviceWrapper();
	
		bool							isConfiguringPy();
		bool							isConfiguring();
		
		ButtonState						getButtonStatePy(GameButtonId buttonCode);
		ButtonState						getButtonState(GameButtonId buttonCode);
		
		bool							configureButtonPy(GameButtonId buttonCode);
		bool							configureButton(GameButtonId buttonCode);
		
		// Return a string literal for the key mapped to the given button.
		std::string						getDeviceButtonNamePy(GameButtonId buttonCode);
		std::string						getDeviceButtonName(GameButtonId buttonCode);
		
		int								getDeviceButtonCodePy(GameButtonId buttonCode);
		int								getDeviceButtonCode(GameButtonId buttonCode);

		void							mapDeviceButtonCodePy(GameButtonId buttonCode, int deviceButtonCode);
		void							mapDeviceButtonCode(GameButtonId buttonCode, int deviceButtonCode);

		void							setKeyValuePy(GameButtonId buttonCode, int keyValue);
		void							setKeyValue(GameButtonId buttonCode, int keyValue);
		
		std::string						getDeviceNamePy();
		std::string						getDeviceName();
		
		InputChannel					getChannelPy();
		InputChannel					getChannel();
		
		bool							getIsInitializedPy();
		bool							getIsInitialized();
		
		void							setIsBlockedPy(bool value);
		void							setIsBlocked(bool value);
		
		bool							getIsBlockedPy();
		bool							getIsBlocked();

		boost::shared_ptr<InputDevice>	getDevice();

	private:
	
		boost::shared_ptr<InputDevice>	device_;
	};
}

#endif //_INPUTDEVICE_HPP_