#include "..\..\Headers\EngineCore\UiWidgetFactory.hpp"

using namespace firemelon;

UiWidgetFactory::UiWidgetFactory()
{
}

UiWidgetFactory::~UiWidgetFactory()
{
}

UiWidgetPtr UiWidgetFactory::createUiWidgetBase(UiWidgetId uiWidgetTypeId, std::string widgetName)
{
	UiWidgetPtr newUiWidget = nullptr;

	newUiWidget = createUiWidget(uiWidgetTypeId, widgetName);

	if (newUiWidget == nullptr)
	{
		newUiWidget = UiWidgetPtr(new UiWidget(widgetName));
	}
	
	newUiWidget->setTypeId(uiWidgetTypeId);

	return newUiWidget;
}

UiWidgetPtr UiWidgetFactory::createUiWidget(UiWidgetId uiWidgetTypeId, std::string widgetName)
{
	return nullptr;
}