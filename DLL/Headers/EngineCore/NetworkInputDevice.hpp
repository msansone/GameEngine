/* -------------------------------------------------------------------------
** NetworkInputDevice.hpp
** 
** The NetworkInputDevice class is derived from the InputDevice class. It 
** acts like any other input device, getting the input state from a packet
** sent by the client.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _NETWORKINPUTDEVICE_HPP_
#define _NETWORKINPUTDEVICE_HPP_

#include <iostream>

#include "InputDevice.hpp"
#include "InputState.hpp"

namespace firemelon
{
	class NetworkInputDevice : public InputDevice
	{
	public:

		NetworkInputDevice(std::string inputDeviceName);
		virtual ~NetworkInputDevice();

		virtual bool					userInitialize();
		virtual	ButtonState				getButtonState(GameButtonId buttonCode);

		boost::shared_ptr<InputState>	getInputState();

	private:

		boost::shared_ptr<InputState>	inputState_;
	};
}

#endif // _NETWORKINPUTDEVICE_HPP_