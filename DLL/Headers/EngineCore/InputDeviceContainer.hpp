/* -------------------------------------------------------------------------
** InputDeviceContainer.hpp
** 
** The InputDeviceContainer class provides a single object which stores all
** input devices. It will be shared among all input device managers. This was
** added because copying input devices from one input device manager to
** another, they would, in the destructor, try to delete memory that had already
** been deleted in another manager.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _INPUTDEVICECONTAINER_HPP_
#define _INPUTDEVICECONTAINER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "InputDeviceWrapper.hpp"

#include <vector>

namespace firemelon
{
	typedef std::vector<boost::shared_ptr<InputDeviceWrapper>> InputDeviceList;

	class FIREMELONAPI InputDeviceContainer
	{
	public:

		InputDeviceContainer();
		virtual ~InputDeviceContainer();

		int										getInputDeviceCount();
		boost::shared_ptr<InputDeviceWrapper>	getInputDevice(int index);

		int										addInputDevice(boost::shared_ptr<InputDeviceWrapper> inputDevice);
		void									removeInputDevice(int index);

		bool									deviceExists(InputChannel channel);

	private:
		
		InputDeviceList							inputDevices_;
	};
}

#endif // _INPUTDEVICECONTAINER_HPP_