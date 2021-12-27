#include "..\..\Headers\EngineCore\UiPanelContainer.hpp"

using namespace firemelon;

UiPanelContainer::UiPanelContainer(DebuggerPtr debugger)
{
	//shared_ptr<X> p1{ new X{ 2 } }; // bad
	//auto p = make_shared<X>(2);    // good

	rootPanel_ = boost::make_shared<UiPanel>("Root");

	rootPanel_->setIsVisible(true);

	rootPanel_->debugger_ = debugger;

	panels_.push_back(rootPanel_);
}

UiPanelContainer::~UiPanelContainer()
{
}

void UiPanelContainer::add(UiPanelPtr panel)
{
	panels_.push_back(panel);
}

void UiPanelContainer::cleanup()
{
	panels_.clear();
}

UiPanelPtr UiPanelContainer::getPanelByName(std::string panelName)
{
	int size = panels_.size();
	
	for (int i = 0; i < size; i++)
	{
		if (panels_[i]->getName() == panelName)
		{
			return panels_[i];
		}
	}

	return nullptr;
}

UiPanelPtr UiPanelContainer::getPanelByIndex(size_t index)
{
	if (index < panels_.size())
	{
		return panels_[index];
	}

	return nullptr;
}

UiPanelPtr UiPanelContainer::getRoot()
{
	return rootPanel_;
}


size_t UiPanelContainer::size()
{
	return panels_.size();
}