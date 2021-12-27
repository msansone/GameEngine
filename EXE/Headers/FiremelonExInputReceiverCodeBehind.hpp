/* -------------------------------------------------------------------------
** FiremelonExInputReceiverCodeBehind.hpp
**
** The FiremelonExInputReceiverCodeBehind class is derived from the 
** InputReceiverCodeBehind class. It is used with the 
** FiremelonExCodeBehindComponentFactory class to create the derived firemelon
** input receiver codebehind components.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _FIREMELONEXINPUTRECEIVERCODEBEHIND_HPP_
#define _FIREMELONEXINPUTRECEIVERCODEBEHIND_HPP_

#include <InputReceiverCodeBehind.hpp>

#include "SdlKeyboardDevice.hpp"

class FiremelonExInputReceiverCodeBehind : public firemelon::InputReceiverCodeBehind
{
public:

	FiremelonExInputReceiverCodeBehind();
	virtual ~FiremelonExInputReceiverCodeBehind();

	void	attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice);
	
private:

	virtual void	cleanup();
	virtual void	initialize();

	void			keyDown(SDL_Keycode keyCode);
	void			keyUp(SDL_Keycode keyCode);

	boost::shared_ptr<SdlKeyboardDevice>	keyboardDevice_;

	bool					keyDownExists_;
	bool					keyUpExists_;

	boost::python::object	pyKeyUp_;
	boost::python::object	pyKeyDown_;

	//static int idCounter_;
	//int id_;
};

#endif // _FIREMELONEXINPUTRECEIVERCODEBEHIND_HPP_