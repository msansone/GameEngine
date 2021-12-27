#include "..\..\Headers\EngineCore\UiWidgetContainer.hpp"

using namespace firemelon;

UiWidgetContainer::UiWidgetContainer()
{
}

UiWidgetContainer::~UiWidgetContainer()
{
}

void UiWidgetContainer::add(UiWidgetPtr widget)
{
	widgets_.push_back(widget);
}

void UiWidgetContainer::cleanup()
{
	for (size_t i = 0; i < widgets_.size(); i++)
	{
		widgets_[i]->cleanupPythonData();			
	}
	
	widgets_.clear();
}

UiWidgetPtr UiWidgetContainer::getWidgetByName(std::string widgetName)
{
	int size = widgets_.size();

	for (int i = 0; i < size; i++)
	{
		if (widgets_[i]->name_ == widgetName)
		{
			return widgets_[i];
		}
	}

	return nullptr;
}

UiWidgetPtr UiWidgetContainer::getWidgetByIndex(size_t index)
{
	if (index < widgets_.size())
	{
		return widgets_[index];
	}

	return nullptr;
}

size_t UiWidgetContainer::size()
{
	return widgets_.size();
}