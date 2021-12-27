/* -------------------------------------------------------------------------
** InputDevice.hpp
**
** The InputDevice class is the generic base class that the user should derive
** from to create input devices. It contains a map which keeps track of the
** game button states, which it gets from the virtual getButtonState function.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _INPUTDEVICE_HPP_
#define _INPUTDEVICE_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <iostream>
#include <string>
#include <map>

#include <boost/signals2.hpp>

#include "GameButtonManager.hpp"
#include "InputEvent.hpp"
#include "InputState.hpp"

namespace firemelon
{
	typedef std::map<GameButtonId, int> ButtonMap;
	
	class FIREMELONAPI InputDevice
	{
	public:
		friend class InputDeviceContainer;
		friend class InputDeviceManager;
		friend class InputDeviceWrapper;

		InputDevice(std::string inputDeviceName);
		virtual ~InputDevice();
	
		bool				isConfiguring();

		bool				preInitialize();

		// Return a string literal for the key mapped to the given button.
		virtual std::string getDeviceButtonName(GameButtonId buttonCode);

		int					getDeviceButtonCode(GameButtonId buttonId);
		void				mapDeviceButtonCode(GameButtonId buttonId, int deviceButtonCode);
		void				setKeyValue(GameButtonId buttonId, int keyValue);
		
		std::string			getDeviceName();
		bool				getIsInitialized();

		bool				getHasInputStateChanged();

		void				setIsBlocked(bool value);
		bool				getIsBlocked();

		InputChannel		getChannel();
		
	protected:
		
		boost::shared_ptr<GameButtonManager>	getGameButtonManager();

		void		setDeviceName(std::string deviceName);

		bool		isBlocked_;

	private:

		struct ButtonMapping
		{
			GameButtonId gameButtonId;
			int deviceButtonId;
		};

		virtual	ButtonState	getButtonState(GameButtonId buttonCode) = 0;

		virtual	bool		configureButton(GameButtonId buttonCode) = 0;

		virtual bool		initialize() = 0;

		void				pollInputStatus();
	
		boost::signals2::signal<void (boost::shared_ptr<InputEvent> args)> buttonDownSignal;
		boost::signals2::signal<void (boost::shared_ptr<InputEvent> args)> buttonUpSignal;
	
		boost::shared_ptr<GameButtonManager>	buttonManager_;

		ButtonMap								gameButtonIdToDeviceButtonCodeMap_;

		bool									ignoreNextInputEvent_;

		bool									isConfiguring_;

		bool									isInitialized_;
		
		bool									hasInputStateChanged_;

		std::string								deviceName_;

		InputChannel							channel_;
		
		int										referenceCount_;

		std::vector<ButtonMapping>				queuedButtonMappings_;

		// Map each game button to an up or down state value.
		boost::shared_ptr<InputState>			inputState_;
		
		// Auto-incrementing static variable used to assign each input device a unique ID.
		static int								idCounter_;
	};
}

#endif //_INPUTDEVICE_HPP_