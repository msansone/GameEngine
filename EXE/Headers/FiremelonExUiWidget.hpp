/* -------------------------------------------------------------------------
** FiremelonExUiWidget.hpp
** 
** The FiremelonExUiWidget class is derived from the UiWidget class. It is used 
** with the FiremelonExUiWidgetFactory class to create the derived firemelon_ex 
** UI widgets, which set the boost timer variable in the associated python object
** instance, via the entity virtual userinitialize function.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _FIREMELONEXUIWIDGET_HPP_
#define _FIREMELONEXUIWIDGET_HPP_

#include <UiWidget.hpp>

#include "BoostGameTimer.hpp"
#include "SdlKeyboardDevice.hpp"

class FiremelonExUiWidget : public firemelon::UiWidget
{
public:
	friend class FiremelonExUi;

	FiremelonExUiWidget(std::string widgetName);
	virtual ~FiremelonExUiWidget();
	
	void	attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice);
	
	virtual void	pythonDataInitialized();
	
private:

	//virtual void	preDestroy();

	void					keyDown(SDL_Keycode keyCode);
	void					keyUp(SDL_Keycode keyCode);

	boost::shared_ptr<SdlKeyboardDevice>	keyboardDevice_;
	
	bool					keyDownExists_;
	bool					keyUpExists_;

	boost::python::object	pyKeyUp_;
	boost::python::object	pyKeyDown_;
};

#endif // _FIREMELONEXUIWIDGET_HPP_