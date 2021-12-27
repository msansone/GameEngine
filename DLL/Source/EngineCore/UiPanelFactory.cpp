#include "..\..\Headers\EngineCore\UiPanelFactory.hpp"

using namespace firemelon;

UiPanelFactory::UiPanelFactory()
{
}

UiPanelFactory::~UiPanelFactory()
{
}

UiPanelPtr UiPanelFactory::createUiPanelBase(std::string layoutName, std::string panelName)
{
	UiPanelPtr newUiPanel = nullptr;

	if (layoutName == "VerticalStack")
	{
		newUiPanel = UiPanelPtr(new UiVerticalStackPanel(panelName));
	}
	else if (layoutName == "HorizontalStack")
	{
		newUiPanel = UiPanelPtr(new UiHorizontalStackPanel(panelName));
	}
	else if (layoutName == "Book")
	{
		newUiPanel = UiPanelPtr(new UiBookPanel(panelName));
	}
	else
	{
		newUiPanel = createUiPanel(layoutName, panelName);
	}

	// Default to vertical stack if an unrecognized layout is specified.
	if (newUiPanel == nullptr)
	{
		std::cout << "Panel layout " << layoutName << " not recognized. Defaulting to VerticalStack" << std::endl;
		newUiPanel = UiPanelPtr(new UiVerticalStackPanel(panelName));
	}

	newUiPanel->setLayoutStyle(layoutName);
	
	return newUiPanel;
}

UiPanelPtr UiPanelFactory::createUiPanel(std::string layoutName, std::string PanelName)
{
	return nullptr;
}