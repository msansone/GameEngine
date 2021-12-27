#include "..\..\Headers\EngineCore\UiBookPanel.hpp"

using namespace firemelon;

UiBookPanel::UiBookPanel(std::string panelName) : UiPanel(panelName)
{
}

UiBookPanel::~UiBookPanel()
{
}

void UiBookPanel::calculateMinimumBoundingSize()
{
	// The width and height will be the size of the visible child, because only one child can be visible at a time.

	int width = 0;

	int height = 0;

	for (size_t i = 0; i < childElements_.size(); i++)
	{
		if (childElements_[i]->getIsVisible() == true)
		{
			width = childElements_[i]->rects_->paddingOuterBoundary->w;
		
			height = childElements_[i]->rects_->paddingOuterBoundary->h;

			break;
		}
	}
	
	calculateRectSizes(width, height, 0, 0);
}

boost::shared_ptr<Size> UiBookPanel::getFullSize()
{
	boost::shared_ptr<Size> size = boost::make_shared<Size>(Size(0, 0));

	// Get the size of the largest child. (Not sure if this is the desired result)
	for (size_t i = 0; i < childElements_.size(); i++)
	{
		auto childElement = childElements_[i];

		auto childSize = childElement->getFullSize();

		int childWidth = childSize->getWidth() + childElement->paddingLeft_ + childElement->paddingRight_ + childElement->marginLeft_ + childElement->marginRight_ + childElement->borderLeft_ + childElement->borderRight_;

		int childHeight = childSize->getHeight() + childElement->paddingTop_ + childElement->paddingBottom_ + childElement->marginTop_ + childElement->marginBottom_ + childElement->borderTop_ + childElement->borderBottom_;

		if (childHeight > size->getHeight())
		{
			size->setHeight(childHeight);
		}

		if (childWidth > size->getWidth())
		{
			size->setWidth(childWidth);
		}
	}

	return size;
}