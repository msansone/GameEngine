/* -------------------------------------------------------------------------
** InputDeviceManager.hpp
**
** The InputDeviceManager class contains a list of user provided input device
** objects. It polls the currently selected input device's button states
** and dispatches input events to any entities that are listening.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _INPUTDEVICEMANAGER_HPP_
#define _INPUTDEVICEMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <set>

#include "InputDeviceContainer.hpp"
#include "InputDevice.hpp"
#include "InputDeviceWrapper.hpp"

namespace firemelon
{
	typedef boost::signals2::signal<void (InputChannel oldChannel, InputChannel newChannel)> ChangeChannelSignal;
	typedef boost::signals2::signal<void(boost::shared_ptr<InputEvent> inputEvent)> ButtonSignal;

	class FIREMELONAPI InputDeviceManager
	{
	public:
		friend class Entity;
		friend class GameEngine;
		friend class InputReceiverCodeBehind;
		friend class RoomManager;
		friend class RoomContainer;
		friend class TextManager;
		friend class Ui;

		InputDeviceManager(boost::shared_ptr<GameButtonManager> buttonManager, boost::shared_ptr<InputDeviceContainer> inputDeviceContainer);
		virtual ~InputDeviceManager();
	
		void									addInputDevice(boost::shared_ptr<InputDevice> inputDevice);

		void									changeInputChannelOfListenersPy(InputChannel oldChannel, InputChannel newChannel);

		void									changeInputChannelOfListeners(InputChannel oldChannel, InputChannel newChannel);

		void									disableEntityInputPy(int entityInstanceId);

		void									disableEntityInput(int entityInstanceId);

		void									disableUiInput();

		void									disableUiInputPy();

		void									enableEntityInputPy(int entityInstanceId);

		void									enableEntityInput(int entityInstanceId);

		void									enableUiInput();

		void									enableUiInputPy();

		bool									getBlockEntityInput();
		bool									getBlockEntityInputPy();

		bool									getDisableUiInput();
		bool									getDisableUiInputPy();

		boost::shared_ptr<GameButtonManager>	getGameButtonManagerPy();
		boost::shared_ptr<GameButtonManager>	getGameButtonManager();

		boost::shared_ptr<InputDevice>			getInputDeviceByChannel(InputChannel channel);
		boost::shared_ptr<InputDevice>			getInputDeviceByIndex(int index);
		boost::shared_ptr<InputDevice>			getInputDeviceByName(std::string deviceName);

		boost::shared_ptr<InputDeviceContainer>	getInputDeviceContainer();

		int										getInputDeviceCountPy();
		int										getInputDeviceCount();

		boost::shared_ptr<InputDeviceWrapper>	getInputDeviceWrapperByChannelPy(InputChannel channel);
		boost::shared_ptr<InputDeviceWrapper>	getInputDeviceWrapperByChannel(InputChannel channel);

		boost::shared_ptr<InputDeviceWrapper>	getInputDeviceWrapperByIndexPy(int index);
		boost::shared_ptr<InputDeviceWrapper>	getInputDeviceWrapperByIndex(int index);

		boost::shared_ptr<InputDeviceWrapper>	getInputDeviceWrapperByNamePy(std::string deviceName);
		boost::shared_ptr<InputDeviceWrapper>	getInputDeviceWrapperByName(std::string deviceName);
		
		bool									isEntityInputEnabled(int entityInstanceId);

		// Polls the button states in the currently selected device.
		void									pollInputStatus();

		void									removeInputDeviceByChannel(InputChannel channel, bool deleteFromContainer = false);

		void									setBlockEntityInput(bool blockEntityInput);
		void									setBlockEntityInputPy(bool blockEntityInput);
		
		void									setDefaultInputChannelPy(InputChannel channel);

		void									setDefaultInputChannel(InputChannel channel);

	private:

		// Pre-process input events before dispatching.
		void preButtonDown(boost::shared_ptr<InputEvent> inputEvent);

		// Pre-process input events before dispatching.
		void preButtonUp(boost::shared_ptr<InputEvent> inputEvent);

		bool									blockEntityInput_;

		ButtonSignal							buttonDownSignal_;
		boost::shared_ptr<GameButtonManager>	buttonManager_;
		ButtonSignal							buttonUpSignal_;
		ChangeChannelSignal						changeChannelOfListenersSignal_;

		// Stores the indexes into the inputDeviceContainer of all the input devices that are active in this manager.
		std::vector<int>						containedInputDeviceIndexes_;

		InputChannel							defaultInputChannel_;
		
		std::set<int>							disabledEntities_;

		bool									disableUi_;

		boost::shared_ptr<InputDeviceContainer>	inputDeviceContainer_;

		// Map an input device name to an index in the input devices vector.
		std::map<std::string, int>				inputDeviceNameIdMap_;
	};

	typedef boost::shared_ptr<InputDeviceManager> InputDeviceManagerPtr;
}

#endif // _INPUTDEVICEMANAGER_HPP_