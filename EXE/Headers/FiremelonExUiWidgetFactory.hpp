#ifndef _FIREMELONEXUIWIDGETFACTORY_HPP_
#define _FIREMELONEXUIWIDGETFACTORY_HPP_

#include <UiWidgetFactory.hpp>

#include "FiremelonExUiWidget.hpp"
#include "SdlKeyboardDevice.hpp"

class FiremelonExUiWidgetFactory : public firemelon::UiWidgetFactory
{
public:
	FiremelonExUiWidgetFactory();
	virtual ~FiremelonExUiWidgetFactory();

	boost::shared_ptr<firemelon::UiWidget> createUiWidget(firemelon::UiWidgetId widgetId, std::string widgetName);
	
	void	attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice);

private:

	boost::shared_ptr<SdlKeyboardDevice> keyboardDevice_;
};

#endif // _FIREMELONEXUIWIDGETFACTORY_HPP_