/* -------------------------------------------------------------------------
** FiremelonExUi.hpp
** 
** The FiremelonExUi class is derived from the parent Ui class, and is
** used to respond to UI events.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _FIREMELONEXUI_HPP_
#define _FIREMELONEXUI_HPP_

#include <Ui.hpp>

#include <InputDevice.hpp>
#include <Debugger.hpp>

#include "FiremelonExUiWidget.hpp"
#include "FmodAudioPlayer.hpp"
#include "OpenGlRenderer.hpp"
#include "SdlRenderer.hpp"
#include "SdlKeyboardDevice.hpp"

#include <boost/python.hpp>

#include <iostream>
#include <fstream>
#include <map>

class FiremelonExUi : public firemelon::Ui
{
public:
	
	FiremelonExUi();
	virtual ~FiremelonExUi();

	virtual void	initialize();
	
	void			attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice);
	
	void			shutdown();

	void			inputDeviceAdded(int channel);
	void			inputDeviceRemoved(int channel);

protected:
	
private:
	
	void	keyDown(SDL_Keycode keyCode);
	void	keyUp(SDL_Keycode keyCode);
	
	virtual void	cleanup();

	boost::shared_ptr<SdlKeyboardDevice>	keyboardDevice_;

	bool					inputDeviceAddedExists_;
	bool					inputDeviceRemovedExists_;

	boost::python::object	pyInputDeviceAdded_;
	boost::python::object	pyInputDeviceRemoved_;

	bool					keyDownExists_;
	bool					keyUpExists_;

	boost::python::object	pyKeyUp_;
	boost::python::object	pyKeyDown_;
};

#endif // _FIREMELONEXUIMANAGER_HPP_