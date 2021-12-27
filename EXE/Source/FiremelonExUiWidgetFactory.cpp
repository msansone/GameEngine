#include "..\Headers\FiremelonExUiWidgetFactory.hpp"

using namespace firemelon;

FiremelonExUiWidgetFactory::FiremelonExUiWidgetFactory()
{
}

FiremelonExUiWidgetFactory::~FiremelonExUiWidgetFactory()
{
}

boost::shared_ptr<UiWidget> FiremelonExUiWidgetFactory::createUiWidget(UiWidgetId uiWidgetId, std::string widgetName)
{
	boost::shared_ptr<UiWidget> newUiWidget = nullptr;
	
	newUiWidget = boost::shared_ptr<UiWidget>(new FiremelonExUiWidget(widgetName));

	boost::static_pointer_cast<FiremelonExUiWidget>(newUiWidget)->attachKeyboardDevice(keyboardDevice_);
	
	return newUiWidget;
}

void FiremelonExUiWidgetFactory::attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice)
{
	keyboardDevice_ = keyboardDevice;
}