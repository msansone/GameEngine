#include "..\..\Headers\EngineCore\UiHorizontalStackPanel.hpp"

using namespace firemelon;

UiHorizontalStackPanel::UiHorizontalStackPanel(std::string panelName) : UiPanel(panelName)
{
}

UiHorizontalStackPanel::~UiHorizontalStackPanel()
{
}

void UiHorizontalStackPanel::calculateMinimumBoundingSize()
{
	// The width will be the cumulative width of the all child elements.
	// The height will be the height of the child element with the greatest height.

	int width = 0;

	int height = 0;

	int maxHeight = 0;

	for (size_t i = 0; i < childElements_.size(); i++)
	{
		if (childElements_[i]->isVisible_ == true)
		{
			if (childElements_[i]->rects_->paddingOuterBoundary->h > maxHeight)
			{
				maxHeight = childElements_[i]->rects_->paddingOuterBoundary->h;
			}

			width += childElements_[i]->rects_->paddingOuterBoundary->w;
		}
	}

	height = maxHeight;

	calculateRectSizes(width, height, 0, 0);
}

void UiHorizontalStackPanel::locateChildElements()
{	
	// Set the position of the elements contained by this panel.
	auto myRects = rects_;

	int panelWidth = myRects->marginsInnerBoundary->w;

	int panelHeight = myRects->marginsInnerBoundary->h;

	int panelCenterX = panelWidth / 2;

	int panelCenterY = panelHeight / 2;

	int xAccumulator = 0;

	for (size_t i = 0; i < childElements_.size(); i++)
	{
		auto childElement = childElements_[i];

		if (childElement->isVisible_ == true)
		{
			auto childRects = childElement->rects_;

			int childWidth = 0;

			int childHeight = 0;

			int childX = 0;

			int childY = 0;

			UiPanelPtr childPanel = nullptr;

			switch (childElement->getPanelElementType())
			{
			case PANEL_ELEMENT_PANEL:

				childPanel = boost::static_pointer_cast<UiPanel>(childElement);

				childWidth = childRects->paddingOuterBoundary->w;

				childHeight = childRects->paddingOuterBoundary->h;

				switch (verticalAlign_)
				{
				case PANEL_VERTICAL_ALIGNMENT_TOP:

					childY = 0;

					break;

				case PANEL_VERTICAL_ALIGNMENT_CENTER:

					childY = panelCenterY - (childHeight / 2);

					break;

				case PANEL_VERTICAL_ALIGNMENT_BOTTOM:

					childY = panelHeight - childHeight;

					break;
				}

				// Set rect size and position.

				// The outermost box of the child is the padding rect.
				// Position it in the top left corner of the parent panel's border inner boundary, offset by
				// the parent panel's margin.
				childElement->rects_->paddingOuterBoundary->x = xAccumulator;

				childElement->rects_->paddingOuterBoundary->y = childY;

				//childElement->rects_->paddingOuterBoundary->w = myRects->borderInnerBoundary->w - marginLeft_ - marginRight_;

				//childElement->rects_->paddingOuterBoundary->h = myRects->borderInnerBoundary->h - marginTop_ - marginBottom_;


				// Padding inner boundary rect is next. Offset the outer boundary position the padding values.
				childElement->rects_->paddingInnerBoundary->x = childElement->rects_->paddingOuterBoundary->x + childElement->paddingLeft_;

				childElement->rects_->paddingInnerBoundary->y = childElement->rects_->paddingOuterBoundary->y + childElement->paddingTop_;

				//childElement->rects_->paddingInnerBoundary->w = childElement->rects_->paddingOuterBoundary->w - childElement->paddingLeft_ - childElement->paddingRight_;

				//childElement->rects_->paddingInnerBoundary->h = childElement->rects_->paddingOuterBoundary->h - childElement->paddingTop_ - childElement->paddingBottom_;


				// The padding inner boundary is the same as the border outer boundary. Offset the border outer
				// boundary by the border values to get the border inner boundary position and size.
				childElement->rects_->borderInnerBoundary->x = childElement->rects_->borderOuterBoundary->x + childElement->borderLeft_;

				childElement->rects_->borderInnerBoundary->y = childElement->rects_->borderOuterBoundary->y + childElement->borderTop_;

				// An an extension value to fill the background as far left as it can go. (That is, to what would be x=0)
				if (childPanel->getFillLeft() == true)
				{
					childPanel->setBackgroundExtensionLeft(childElement->rects_->borderOuterBoundary->x);
				}

				//childElement->rects_->borderInnerBoundary->w = childElement->rects_->borderOuterBoundary->w - childElement->borderLeft_ - childElement->borderRight_;

				//childElement->rects_->borderInnerBoundary->h = childElement->rects_->borderOuterBoundary->h - childElement->borderTop_ - childElement->borderBottom_;


				// The border inner boundary is the same as the margin outer boundary. Offset the margin outer
				// boundary by the margin values to get the margin inner boundary position and size.
				childElement->rects_->marginsInnerBoundary->x = childElement->rects_->marginsOuterBoundary->x + childElement->marginLeft_;

				childElement->rects_->marginsInnerBoundary->y = childElement->rects_->marginsOuterBoundary->y + childElement->marginTop_;

				//childElement->rects_->marginsInnerBoundary->w = childElement->rects_->marginsOuterBoundary->w - childElement->marginLeft_ - childElement->marginRight_;

				//childElement->rects_->marginsInnerBoundary->h = childElement->rects_->marginsOuterBoundary->h - childElement->marginTop_ - childElement->marginBottom_;

				// The margin inner boundary is the same as the content rect. No need to position or size the content rect.

				break;

			case PANEL_ELEMENT_WIDGET:

				// The outermost box of the child is the padding rect.
				// Position it in the top left corner of the parent panel's bounder inner boundary, offset by
				// the parent panel's margin.
				childX = xAccumulator;

				childWidth = childRects->paddingOuterBoundary->w;

				childHeight = childRects->paddingOuterBoundary->h;

				switch (verticalAlign_)
				{
				case PANEL_VERTICAL_ALIGNMENT_TOP:

					childY = 0;

					break;

				case PANEL_VERTICAL_ALIGNMENT_CENTER:

					childY = panelCenterY - (childHeight / 2);

					// Position the y value such that the padding inner region is at childY

					break;

				case PANEL_VERTICAL_ALIGNMENT_BOTTOM:

					childY = panelHeight - childHeight;

					break;
				}

				childElement->rects_->paddingOuterBoundary->x = childX;

				childElement->rects_->paddingOuterBoundary->y = childY;

				// Padding inner boundary rect is next. Offset the outer boundary position the padding values.
				childElement->rects_->paddingInnerBoundary->x = childElement->rects_->paddingOuterBoundary->x + childElement->paddingLeft_;

				childElement->rects_->paddingInnerBoundary->y = childElement->rects_->paddingOuterBoundary->y + childElement->paddingTop_;

				break;
			}

			// Add the height to the x position accumulator variable.
			xAccumulator += childRects->paddingOuterBoundary->w;
		}
	}
}

boost::shared_ptr<Size> UiHorizontalStackPanel::getFullSize()
{
	boost::shared_ptr<Size> size = boost::make_shared<Size>(Size(0, 0));

	// Get the size for each child.
	for (size_t i = 0; i < childElements_.size(); i++)
	{
		auto childElement = childElements_[i];

		auto childSize = childElement->getFullSize();

		int childWidth = childSize->getWidth() + childElement->paddingLeft_ + childElement->paddingRight_ + childElement->marginLeft_ + childElement->marginRight_ + childElement->borderLeft_ + childElement->borderRight_;

		int childHeight = childSize->getHeight() + childElement->paddingTop_ + childElement->paddingBottom_ + childElement->marginTop_ + childElement->marginBottom_ + childElement->borderTop_ + childElement->borderBottom_;

		size->setWidth(size->getWidth() + childWidth);

		if (childHeight > size->getHeight())
		{
			size->setHeight(childHeight);
		}
	}

	return size;
}