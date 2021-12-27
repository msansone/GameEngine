#include "..\..\Headers\EngineCore\UiPanel.hpp"

using namespace firemelon;

UiPanel::UiPanel(std::string panelName)
{
	caption_ = "";
	
	captionFont_ = "";

	captionLeft_ = 0.0;

	captionRenderEffects_ = boost::make_shared<RenderEffects>(RenderEffects());

	captionTop_ = 0.0;

	fillBottom_ = false;

	fillLeft_ = false;

	fillRight_ = false;

	fillTop_ = false;
	
	name_ = panelName;

	horizontalAlign_ = PANEL_HORIZONTAL_ALIGNMENT_CENTER;

	verticalAlign_ = PANEL_VERTICAL_ALIGNMENT_CENTER;

	isVisible_ = false;
	
	layoutStyle_ = "";

	frameMarginLeft_ = 0;

	frameMarginRight_ = 0;

	frameMarginTop_ = 0;

	frameMarginBottom_ = 0;

	positionStyle_ = PANEL_POSITION_AUTO;

	focusedChildIndex_ = 0;

	elementType_ = PANEL_ELEMENT_PANEL;

	focusWrap_ = false;

	controlFlow_ = PANEL_CONTROL_FLOW_COLLAGE;
}

UiPanel::~UiPanel()
{
}

bool UiPanel::getIsFocusable()
{
	// A panel element is focusable if 
	// 1) Its focusable property is set to true
	// 2) Its visible property is set to true, and it is not contained in a book panel
	// 3) and it has at least one focusable child
	bool isVisible = isVisible_;

	if (parentPanel_->controlFlow_ == PANEL_CONTROL_FLOW_BOOK)
	{
		// If this is contained by a book, force it to be true, because
		// unfocused pages will always not be visible.
		isVisible = true;
	}

	if (focusable_ == true && isVisible == true)
	{
		for (size_t i = 0; i < childElements_.size(); i++)
		{
			if (childElements_[i]->getIsFocusable() == true)
			{
				return true;
			}
		}
	}

	return false;
}

std::string UiPanel::getLayoutStyle()
{
	return layoutStyle_;
}

std::string UiPanel::getLayoutStylePy()
{
	PythonReleaseGil unlocker;

	return getLayoutStyle();
}

void UiPanel::setLayoutStyle(std::string layoutStyle)
{
	layoutStyle_ = layoutStyle;
}

void UiPanel::setLayoutStylePy(std::string layoutStyle)
{
	PythonReleaseGil unlocker;

	setLayoutStyle(layoutStyle);
}

PanelHorizontalAlignment UiPanel::getPanelHorizontalAlignment()
{
	return horizontalAlign_;
}

PanelHorizontalAlignment UiPanel::getPanelHorizontalAlignmentPy()
{
	PythonReleaseGil unlocker;

	return getPanelHorizontalAlignment();
}

PanelVerticalAlignment UiPanel::getPanelVerticalAlignment()
{
	return verticalAlign_;
}

PanelVerticalAlignment UiPanel::getPanelVerticalAlignmentPy()
{
	PythonReleaseGil unlocker;

	return getPanelVerticalAlignment();
}

void UiPanel::setHorizontalAlignment(PanelHorizontalAlignment alignment)
{
	horizontalAlign_ = alignment;
}

void UiPanel::setHorizontalAlignmentPy(PanelHorizontalAlignment alignment)
{
	PythonReleaseGil unlocker;

	setHorizontalAlignment(alignment);
}

void UiPanel::setVerticalAlignment(PanelVerticalAlignment alignment)
{
	verticalAlign_ = alignment;
}

void UiPanel::setVerticalAlignmentPy(PanelVerticalAlignment alignment)
{
	PythonReleaseGil unlocker;

	setVerticalAlignment(alignment);
}

void UiPanel::locateElementsBase()
{
	if (isVisible_ == true)
	{
		locateChildElements();

		for (size_t i = 0; i < childElements_.size(); i++)
		{
			childElements_[i]->locateElementsBase();
		}
	}
}

void UiPanel::locateChildElements()
{	
	auto myRects = rects_;

	// Only the root should be calling this function. Every other panel should have a specific panel type set.
	for (size_t i = 0; i < childElements_.size(); i++)
	{
		// Size each of the children to fill the entire content region.
		auto childElement = childElements_[i];

		auto childRects = childElement->rects_;

		switch (positionStyle_)
		{
		case PANEL_POSITION_AUTO:

			// The outermost box of the child is the padding rect.
			// Position it in the top left corner of the parent panel's bounder inner boundary, offset by
			// the parent panel's margin.
			childRects->paddingOuterBoundary->x = 0;

			childRects->paddingOuterBoundary->y = 0;

			childRects->paddingOuterBoundary->w = myRects->borderInnerBoundary->w - marginLeft_ - marginRight_;

			childRects->paddingOuterBoundary->h = myRects->borderInnerBoundary->h - marginTop_ - marginBottom_;


			// Padding inner boundary rect is next. Offset the outer boundary position the padding values.
			childRects->paddingInnerBoundary->x = childRects->paddingOuterBoundary->x + childElement->paddingLeft_;

			childRects->paddingInnerBoundary->y = childRects->paddingOuterBoundary->y + childElement->paddingTop_;

			childRects->paddingInnerBoundary->w = childRects->paddingOuterBoundary->w - childElement->paddingLeft_ - childElement->paddingRight_;

			childRects->paddingInnerBoundary->h = childRects->paddingOuterBoundary->h - childElement->paddingTop_ - childElement->paddingBottom_;


			// The padding inner boundary is the same as the border outer boundary. Offset the border outer
			// boundary by the border values to get the border inner boundary position and size.
			childRects->borderInnerBoundary->x = childRects->borderOuterBoundary->x + childElement->borderLeft_;

			childRects->borderInnerBoundary->y = childRects->borderOuterBoundary->y + childElement->borderTop_;

			childRects->borderInnerBoundary->w = childRects->borderOuterBoundary->w - childElement->borderLeft_ - childElement->borderRight_;

			childRects->borderInnerBoundary->h = childRects->borderOuterBoundary->h - childElement->borderTop_ - childElement->borderBottom_;


			// The border inner boundary is the same as the margin outer boundary. Offset the margin outer
			// boundary by the margin values to get the margin inner boundary position and size.
			childRects->marginsInnerBoundary->x = childRects->marginsOuterBoundary->x + childElement->marginLeft_;

			childRects->marginsInnerBoundary->y = childRects->marginsOuterBoundary->y + childElement->marginTop_;

			childRects->marginsInnerBoundary->w = childRects->marginsOuterBoundary->w - childElement->marginLeft_ - childElement->marginRight_;

			childRects->marginsInnerBoundary->h = childRects->marginsOuterBoundary->h - childElement->marginTop_ - childElement->marginBottom_;

			//childRects->content->x = childRects->marginsInnerBoundary->x;

			//childRects->content->y = childRects->marginsInnerBoundary->y;

			break;

		case PANEL_POSITION_MANUAL:

			childRects->paddingOuterBoundary->x = childElement->position_->getX();

			childRects->paddingOuterBoundary->y = childElement->position_->getY();

			// Padding inner boundary rect is next. Offset the outer boundary position the padding values.
			childRects->paddingInnerBoundary->x = childRects->paddingOuterBoundary->x + childElement->paddingLeft_;

			childRects->paddingInnerBoundary->y = childRects->paddingOuterBoundary->y + childElement->paddingTop_;

			// The padding inner boundary is the same as the border outer boundary. Offset the border outer
			// boundary by the border values to get the border inner boundary position and size.
			childRects->borderInnerBoundary->x = childRects->borderOuterBoundary->x + childElement->borderLeft_;

			childRects->borderInnerBoundary->y = childRects->borderOuterBoundary->y + childElement->borderTop_;

			// The border inner boundary is the same as the margin outer boundary. Offset the margin outer
			// boundary by the margin values to get the margin inner boundary position and size.
			childRects->marginsInnerBoundary->x = childRects->marginsOuterBoundary->x + childElement->marginLeft_;

			childRects->marginsInnerBoundary->y = childRects->marginsOuterBoundary->y + childElement->marginTop_;

			break;

		}
	}
}

void UiPanel::calculateMinimumBoundingSize()
{
}

void UiPanel::calculateMinimumBoundingSizeBase()
{
	if (isVisible_ == true)
	{
		// Ensure that all children have had their sizes calculated before calculating the panel size.
		for (size_t i = 0; i < childElements_.size(); i++)
		{
			childElements_[i]->calculateMinimumBoundingSizeBase();
		}

		calculateMinimumBoundingSize();
	}
}

void UiPanel::update(double time)
{
}

boost::shared_ptr<UiPanelElement> UiPanel::getFocusedWidget()
{
	if (childElements_.size() > 0 && focusedChildIndex_ >= 0 && focusedChildIndex_ < childElements_.size())
	{
		boost::shared_ptr<UiPanelElement> childElement = childElements_[focusedChildIndex_];

		if (childElement->elementType_ == PANEL_ELEMENT_WIDGET)
		{
			return childElement;
		}
		else
		{
			UiPanelPtr childPanel = boost::static_pointer_cast<UiPanel>(childElement);

			return childPanel->getFocusedWidget();
		}
	}
	
	return nullptr;
}

void UiPanel::updateBase(double time)
{
	shownEventCalled_ = false;
	hiddenEventCalled_ = false;

	if (isVisible_ == true)
	{
		if (parentPanel_ != nullptr)
		{
			hasFocus_ = (indexInParent_ == parentPanel_->focusedChildIndex_) && parentPanel_->hasFocus_;;
		}
		else
		{
			// This is the root, which always has focus.
			hasFocus_ = true;
		}

		// Update the children.
		for (size_t i = 0; i < childElements_.size(); i++)
		{
			childElements_[i]->updateBase(time);
		}

		if (hasFocus_ == true)
		{
			update(time);
		}
	}
}

void UiPanel::renderBase(int parentX, int parentY)
{
	if (isVisible_ == true)
	{
		renderBackground(parentX, parentY);

		// Render the children.
		for (size_t i = 0; i < childElements_.size(); i++)
		{
			childElements_[i]->renderBase(parentX + rects_->marginsInnerBoundary->x, parentY + +rects_->marginsInnerBoundary->y);
		}

		// When to render the decorations? Maybe I should have BG decorations and Border decorations.
		renderBorder(parentX, parentY);

		if (debugger_->getDebugMode() == true && (debugger_->getPanelElementName() == getName() || debugger_->getPanelElementName() == "all"))
		{
			RendererPtr renderer = getRenderer();

			if (renderer != nullptr)
			{
				renderer->drawRect(rects_->paddingOuterBoundary->x + parentX, rects_->paddingOuterBoundary->y + parentY, rects_->paddingOuterBoundary->w, rects_->paddingOuterBoundary->h, 1.0, 0.0, 0.0, 1.0);
				
				renderer->drawRect(rects_->borderOuterBoundary->x + parentX, rects_->borderOuterBoundary->y + parentY, rects_->borderOuterBoundary->w, rects_->borderOuterBoundary->h, 0.0, 1.0, 0.0, 1.0);

				renderer->drawRect(rects_->marginsOuterBoundary->x + parentX, rects_->marginsOuterBoundary->y + parentY, rects_->marginsOuterBoundary->w, rects_->marginsOuterBoundary->h, 0.0, 0.0, 1.0, 1.0);

				renderer->drawRect(rects_->marginsInnerBoundary->x + parentX, rects_->marginsInnerBoundary->y + parentY, rects_->marginsInnerBoundary->w, rects_->marginsInnerBoundary->h, 1.0, 0.0, 1.0, 1.0);

				//renderer->drawRect(rects_->content->x + parentX, rects_->content->y + parentY, rects_->content->w, rects_->content->h, 1.0, 0.6, 0.2, 1.0);
			}
		}
	}
}

void UiPanel::setInitialFocus()
{
	int firstFocusableChildIndex = getFirstFocusableChildIndex();
	
	if (firstFocusableChildIndex >= 0 && firstFocusableChildIndex < childElements_.size())
	{
		int oldFocusedChildIndex = focusedChildIndex_;

		focusedChildIndex_ = firstFocusableChildIndex;

		if (childElements_[focusedChildIndex_]->getPanelElementType() == PANEL_ELEMENT_PANEL)
		{
			UiPanelPtr childPanel = boost::static_pointer_cast<UiPanel>(childElements_[focusedChildIndex_]);

			if (controlFlow_ == PANEL_CONTROL_FLOW_BOOK)
			{
				childElements_[oldFocusedChildIndex]->setIsVisible(false, false);

				childElements_[focusedChildIndex_]->setIsVisible(true, false);
			}

			childPanel->setInitialFocus();
		}
	}
}

void UiPanel::reset()
{
	if (focusedChildIndex_ >= 0 && focusedChildIndex_ < childElements_.size())
	{
		// The focused child element's visibility may have changed. 
		if (childElements_[focusedChildIndex_]->isVisible_ == false)
		{
			// Try focusing the previous element first. If that fails, try focusing the next element.
			if (focusPreviousElement() == false)
			{
				focusNextElement();
			}
		}
	}
	else if (focusedChildIndex_ == -1)
	{
		focusFirstElement();
	}
}

bool UiPanel::focusElement(int index)
{
	if (index >= 0 && index < childElements_.size())
	{
		std::string elementName = childElements_[index]->getName();

		return focusElement(elementName);
	}
	else
	{
		std::cout << "Failed to focus element because the given index was outside the bounds of the child element array." << std::endl;

		return false;
	}
}

bool UiPanel::focusElement(std::string elementName)
{
	bool elementFocused = false;

	if (childElements_.size() > 0)
	{
		bool isOkayToProceed = true;

		if (focusedChildIndex_ >= 0 && focusedChildIndex_ < childElements_.size())
		{
			if (childElements_[focusedChildIndex_]->name_ == elementName)
			{
				isOkayToProceed = false;
			}			
		}

		if (isOkayToProceed == true)
		{
			for (size_t i = 0; i < childElements_.size(); i++)
			{
				if (childElements_[i]->name_ == elementName)
				{
					if (childElements_[i]->getIsFocusable() == true)
					{
						if (controlFlow_ == PANEL_CONTROL_FLOW_BOOK)
						{
							auto elementGettingFocus = childElements_[i];

							// If this panel is using book style control flow, when a child becomes active, it becomes visible,
							// and the others become invisible.
							elementGettingFocus->setIsVisible(true, false);

							// Put the previously focused element's index on the back stack.
							childElementIndexBackStack_.push_back(focusedChildIndex_);
							
							if (elementGettingFocus->getPanelElementType() == PANEL_ELEMENT_PANEL)
							{
								// If the focus of the children for the element getting focus has not been initalized yet, it could
								// still have the intial values, which may be invalid based on the focusable properties.

								// Check if the newly focused element's own focused child element is itself also focusable.						
								// If it is, make sure the first focusable element is given focus.
								// If it is not, move to the next child element, and try that one. Continue until one is found.
							
								auto panelGettingFocus = boost::static_pointer_cast<UiPanel>(elementGettingFocus);

								panelGettingFocus->setInitialFocus();
							}
						}

						focusedChildIndex_ = i;

						elementFocused = true;
					}
					else
					{
						std::cout << "Unable to set focus to element " << childElements_[i]->getName() << " either because it is not focusable or not visible." << std::endl;
					}
				}
				else
				{
					// Potential bug. If focusing a non-focusable element is attempted, it will still hide the other pages.					
					if (controlFlow_ == PANEL_CONTROL_FLOW_BOOK)
					{
						childElements_[i]->setIsVisible(false, false);
					}
				}
			}
		}
	}

	return elementFocused;
}

bool UiPanel::focusFirstElement()
{
	bool elementFocused = false;

	int firstFocusableChildIndex = getFirstFocusableChildIndex();

	if (focusedChildIndex_ != firstFocusableChildIndex)
	{
		int previouslyFocusedElementIndex = focusedChildIndex_;

		focusedChildIndex_ = firstFocusableChildIndex;

		// For now leave this alone. Might need to come back to it later.
		//////// This code may need to be copied to the focusLastElement functions.
		//////if (childElements_[focusedChildIndex_]->getIsFocusable() == true)
		//////{
		//////	// If the element getting focus is a book
		//////	if (controlFlow_ == PANEL_CONTROL_FLOW_BOOK)
		//////	{
		//////		auto elementGettingFocus = childElements_[focusedChildIndex_];

		//////		// If this panel is using book style control flow, when a child becomes active, it becomes visible,
		//////		// and the others become invisible.
		//////		elementGettingFocus->setIsVisible(true, false);

		//////		if (previouslyFocusedElementIndex < childElements_.size())
		//////		{
		//////			auto elementLosingFocus = childElements_[previouslyFocusedElementIndex];

		//////			// If this panel is using book style control flow, when a child becomes active, it becomes visible,
		//////			// and the others become invisible.
		//////			elementLosingFocus->setIsVisible(false, false);
		//////		}
		//////		
		//////		if (elementGettingFocus->getPanelElementType() == PANEL_ELEMENT_PANEL)
		//////		{
		//////			// If the focus of the children for the element getting focus has not been initalized yet, it could
		//////			// still have the intial values, which may be invalid based on the focusable properties.

		//////			// Check if the newly focused element's own focused child element is itself also focusable.						
		//////			// If it is, make sure the first focusable element is given focus.
		//////			// If it is not, move to the next child element, and try that one. Continue until one is found.

		//////			auto panelGettingFocus = boost::static_pointer_cast<UiPanel>(elementGettingFocus);

		//////			panelGettingFocus->setInitialFocus();
		//////		}
		//////	}
		//////}

		elementFocused = true;

		if (controlFlow_ == PANEL_CONTROL_FLOW_BOOK)
		{
			childElementIndexBackStack_.clear();
		}
	}

	return elementFocused;
}

bool UiPanel::focusLastElement()
{
	bool elementFocused = false;

	int lastFocusableChildIndex = getLastFocusableChildIndex();

	if (focusedChildIndex_ != lastFocusableChildIndex)
	{
		focusedChildIndex_ = lastFocusableChildIndex;
	}

	return elementFocused;
}

bool UiPanel::focusNextElement()
{
	bool elementFocused = false;

	if (controlFlow_ == PANEL_CONTROL_FLOW_BOOK)
	{
		// Book panels are a special case because every page in a book is "focusable", in that
		// a page is displayed by being given focus. So even if it only displays information.
		// and contains no interactive components, it needs to be able to change the focused
		// child index.

		if (focusedChildIndex_ + 1 < childElements_.size())
		{
			auto elementLosingFocus = childElements_[focusedChildIndex_];

			auto elementGettingFocus = childElements_[focusedChildIndex_ + 1];

			// If this panel is using book style control flow, when a child becomes active, it becomes visible,
			// and the others become invisible.
			elementGettingFocus->setIsVisible(true, false);

			elementLosingFocus->setIsVisible(false, false);

			// Put the previously focused element's index on the back stack.
			childElementIndexBackStack_.push_back(focusedChildIndex_);

			if (elementGettingFocus->getPanelElementType() == PANEL_ELEMENT_PANEL)
			{
				// If the focus of the children for the element getting focus has not been initalized yet, it could
				// still have the intial values, which may be invalid based on the focusable properties.

				// Check if the newly focused element's own focused child element is itself also focusable.						
				// If it is, make sure the first focusable element is given focus.
				// If it is not, move to the next child element, and try that one. Continue until one is found.

				auto panelGettingFocus = boost::static_pointer_cast<UiPanel>(elementGettingFocus);

				panelGettingFocus->setInitialFocus();
			}

			focusedChildIndex_++;

			elementFocused = true;
		}
		else
		{
			elementFocused = false;
		}
	}
	else
	{
		int previousFocusedChildIndex = focusedChildIndex_;

		int nextFocusableChildIndex = getNextFocusableChildIndex();

		int lastFocusableChildIndex = getLastFocusableChildIndex();

		if (nextFocusableChildIndex > lastFocusableChildIndex)
		{
			// The focus has reached the end. Decide whether to wrap or not.
			if (focusWrap_ == false)
			{
				focusedChildIndex_ = lastFocusableChildIndex;
			}
			else
			{
				focusedChildIndex_ = getFirstFocusableChildIndex();
			}
		}
		else
		{
			focusedChildIndex_ = nextFocusableChildIndex;
		}

		if (previousFocusedChildIndex != focusedChildIndex_)
		{
			elementFocused = true;
		}
	}

	return elementFocused;
}

bool UiPanel::focusPreviousElement()
{
	bool elementFocused = false;

	int previousFocusedChildIndex = focusedChildIndex_;

	if (controlFlow_ == PANEL_CONTROL_FLOW_BOOK)
	{
		if (childElementIndexBackStack_.size() > 0)
		{
			childElements_[focusedChildIndex_]->setIsVisible(false, false);

			focusedChildIndex_ = childElementIndexBackStack_.back();

			childElementIndexBackStack_.pop_back();

			childElements_[focusedChildIndex_]->setIsVisible(true, false);

			if (childElements_[focusedChildIndex_]->getPanelElementType() == PANEL_ELEMENT_PANEL)
			{
				// If the focus of the children for the element getting focus has not been initalized yet, it could
				// still have the intial values, which may be invalid based on the focusable properties.

				// Check if the newly focused element's own focused child element is itself also focusable.						
				// If it is, make sure the first focusable element is given focus.
				// If it is not, move to the next child element, and try that one. Continue until one is found.

				auto panelGettingFocus = boost::static_pointer_cast<UiPanel>(childElements_[focusedChildIndex_]);

				panelGettingFocus->setInitialFocus();
			}
		}
	}
	else
	{
		int previousFocusableChildIndex = getPreviousFocusableChildIndex();

		int firstFocusableChildIndex = getFirstFocusableChildIndex();

		int lastFocusableChildIndex = getLastFocusableChildIndex();

		if (previousFocusableChildIndex < 0)
		{
			if (focusWrap_ == false)
			{
				focusedChildIndex_ = firstFocusableChildIndex;
			}
			else
			{
				focusedChildIndex_ = lastFocusableChildIndex;
			}
		}
		else
		{
			focusedChildIndex_ = previousFocusableChildIndex;
		}
	}

	if (previousFocusedChildIndex != focusedChildIndex_)
	{
		elementFocused = true;
	}

	return elementFocused;
}

void UiPanel::created()
{
	// Panels default to invisible.
	isVisible_ = false;
}

void UiPanel::renderBackground(int parentX, int parentY)
{
	// Fill in the background.
	int left = rects_->background->x + parentX - backgroundExtensionLeft_;

	int right = left + rects_->background->w + backgroundExtensionLeft_ + backgroundExtensionRight_;

	int top = rects_->background->y + parentY - backgroundExtensionTop_;

	int bottom = top + rects_->background->h + backgroundExtensionTop_ + backgroundExtensionBottom_;

	if (backgroundSheetName_ != "")
	{
		// Render the background
		int backgroundSurfaceId = renderer_->getSheetIDByName(backgroundSheetName_);

		boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(backgroundSurfaceId);

		int backgroundCellHeight = (sheet->getCellHeight() * sheet->getScaleFactor());

		int backgroundCellWidth = (sheet->getCellWidth() * sheet->getScaleFactor());
		
		int marginLeft = frameMarginLeft_;

		int marginRight = frameMarginRight_;

		int marginTop = frameMarginTop_;

		int marginBottom = frameMarginBottom_;

		// Fill in the background
		for (int i = top; i < bottom; i += backgroundCellHeight)
		{
			for (int j = left; j < right; j += backgroundCellWidth)
			{
				backgroundRenderEffects_->setExtentRight(1.0f);
				backgroundRenderEffects_->setExtentBottom(1.0f);

				if (j + backgroundCellWidth > right)
				{
					// The background is going to extend too far. Set the extent accordingly.
					int extraWidth = (j + backgroundCellWidth) - right;

					int dispayWidth = backgroundCellWidth - extraWidth;

					float extentPercent = ((float)dispayWidth / (float)backgroundCellWidth) * 100.0f;

					// Transform the percent to a range of - 50 to 50, and the divide by 50 to transform it 
					// into a range - 1 to 1, which is what the extent uses.
					float extentRight = ((float)(extentPercent - 50.0f) / 50.0f);

					backgroundRenderEffects_->setExtentRight(extentRight);
				}

				if (i + backgroundCellHeight > bottom)
				{
					// The background is going to extend too far. Set the extent accordingly.
					int extraHeight = (i + backgroundCellHeight) - bottom;

					int dispayHeight = backgroundCellHeight - extraHeight;

					float extentPercent = ((float)dispayHeight / (float)backgroundCellHeight) * 100.0f;

					// Transform the percent to a range of - 50 to 50, and the divide by 50 to transform it 
					// into a range - 1 to 1, which is what the extent uses.
					float extentBottom = ((float)(extentPercent - 50.0f) / 50.0f);

					backgroundRenderEffects_->setExtentBottom(extentBottom);

				}

				renderer_->renderSheetCell(j, i, backgroundSurfaceId, 1, 1, backgroundRenderEffects_);
			}
		}
	}


	// Render all background decorations.
	for (size_t i = 0; i < decorations_.size(); i++)
	{
		PanelDecoration decoration = decorations_[i];

		int sheetId = renderer_->getSheetIDByName(decoration.sheetName);

		if (sheetId >= 0)
		{
			boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(sheetId);

			int column = decoration.column;

			int row = decoration.row;

			int decorationLeft = (int)((decoration.left + 1) * (rects_->background->w / 2));

			int decorationTop = (int)((decoration.top + 1) * (rects_->background->h / 2));

			int offsetX = 0;

			int offsetY = 0;

			switch (decoration.origin)
			{
			case PANEL_DECORATION_ORIGIN_TOPMIDDLE:

				offsetX = (int)(((sheet->getCellWidth() * sheet->getScaleFactor()) / 2));

				break;

			case PANEL_DECORATION_ORIGIN_TOPRIGHT:

				offsetX = (int)((sheet->getCellWidth() * sheet->getScaleFactor()));

				break;

			case PANEL_DECORATION_ORIGIN_CENTERLEFT:

				offsetY = (int)(((sheet->getCellHeight() * sheet->getScaleFactor()) / 2));

				break;

			case PANEL_DECORATION_ORIGIN_CENTER:

				offsetX = (int)(((sheet->getCellWidth() * sheet->getScaleFactor()) / 2));

				offsetY = (int)(((sheet->getCellHeight() * sheet->getScaleFactor()) / 2));

				break;

			case PANEL_DECORATION_ORIGIN_CENTERRIGHT:

				offsetX = (int)((sheet->getCellWidth() * sheet->getScaleFactor()));

				offsetY = (int)(((sheet->getCellHeight() * sheet->getScaleFactor()) / 2));

				break;

			case PANEL_DECORATION_ORIGIN_BOTTOMLEFT:

				offsetY = (int)((sheet->getCellHeight() * sheet->getScaleFactor()));

				break;

			case PANEL_DECORATION_ORIGIN_BOTTOMMIDDLE:

				offsetX = (int)(((sheet->getCellWidth() * sheet->getScaleFactor()) / 2));

				offsetY = (int)((sheet->getCellHeight() * sheet->getScaleFactor()));

				break;

			case PANEL_DECORATION_ORIGIN_BOTTOMRIGHT:

				offsetX = (int)((sheet->getCellWidth() * sheet->getScaleFactor()));

				offsetY = (int)((sheet->getCellHeight() * sheet->getScaleFactor()));

				break;

			}


			int x = left + decorationLeft - offsetX;

			int y = top + decorationTop - offsetY;

			renderer_->renderSheetCell(x, y, sheetId, column, row);
		}
	}

	// Render the panel caption.
	if (caption_ != "" && captionFont_ != "")
	{
		auto font = fontManager_->getFont(captionFont_);

		int width = (int)(font->getCharacterWidth() * captionRenderEffects_->getScaleFactor()) * caption_.size();

		int height = (int)(font->getCharacterHeight() * captionRenderEffects_->getScaleFactor());

		int captionLeft = (int)((captionLeft_ + 1) * (rects_->background->w / 2));

		int captionTop = (int)((captionTop_ + 1) * (rects_->background->h / 2));

		int captionCenterX = (int)(width / 2);

		int captionCenterY = (int)(height / 2);

		int x = left + captionLeft - captionCenterX;

		int y = top + captionTop - captionCenterY;

		font->writeText(x, y, caption_, captionRenderEffects_);
	}
}

void UiPanel::renderBorder(int parentX, int parentY)
{
	// Draw a border around the background.
	int left = rects_->background->x + parentX - backgroundExtensionLeft_;

	int right = left + rects_->background->w + backgroundExtensionLeft_ + backgroundExtensionRight_;

	int top = rects_->background->y + parentY - backgroundExtensionTop_;

	int bottom = top + rects_->background->h + backgroundExtensionTop_ + backgroundExtensionBottom_;

	if (backgroundSheetName_ != "")
	{
		// Render the background
		int backgroundSurfaceId = renderer_->getSheetIDByName(backgroundSheetName_);

		boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(backgroundSurfaceId);

		int backgroundCellHeight = (sheet->getCellHeight() * sheet->getScaleFactor());

		int backgroundCellWidth = (sheet->getCellWidth() * sheet->getScaleFactor());

		int marginLeft = frameMarginLeft_;

		int marginRight = frameMarginRight_;

		int marginTop = frameMarginTop_;

		int marginBottom = frameMarginBottom_;

		backgroundRenderEffects_->setExtentRight(1.0f);
		backgroundRenderEffects_->setExtentBottom(1.0f);

		// Fill in the top and bottom borders.
		for (int j = left + marginLeft; j < right - marginRight; j += backgroundCellWidth)
		{
			if (j + backgroundCellWidth > right - marginRight)
			{
				// The background is going to extend too far. Set the extent accordingly.
				int extraWidth = (j + backgroundCellWidth) - (right - marginRight);

				int dispayWidth = backgroundCellWidth - extraWidth;

				float extentPercent = ((float)dispayWidth / (float)backgroundCellWidth) * 100.0f;

				// Transform the percent to a range of - 50 to 50, and the divide by 50 to transform it 
				// into a range - 1 to 1, which is what the extent uses.
				float extentRight = ((float)(extentPercent - 50.0f) / 50.0f);

				backgroundRenderEffects_->setExtentRight(extentRight);
			}

			renderer_->renderSheetCell(j, top, backgroundSurfaceId, 1, 0, backgroundRenderEffects_);

			renderer_->renderSheetCell(j, bottom - backgroundCellHeight, backgroundSurfaceId, 1, 2, backgroundRenderEffects_);
		}

		backgroundRenderEffects_->setExtentRight(1.0f);

		// Then, fill in the left and right borders.
		for (int i = top + marginTop; i < bottom - marginBottom; i += backgroundCellHeight)
		{
			if (i + backgroundCellHeight > bottom - marginBottom)
			{
				// The background is going to extend too far. Set the extent accordingly.
				int extraHeight = (i + backgroundCellHeight) - (bottom - marginBottom);

				int dispayHeight = backgroundCellHeight - extraHeight;

				float extentPercent = ((float)dispayHeight / (float)backgroundCellHeight) * 100.0f;

				// Transform the percent to a range of - 50 to 50, and the divide by 50 to transform it 
				// into a range - 1 to 1, which is what the extent uses.
				float extentBottom = ((float)(extentPercent - 50.0f) / 50.0f);

				backgroundRenderEffects_->setExtentBottom(extentBottom);
			}

			// Render the left border tile.
			renderer_->renderSheetCell(left, i, backgroundSurfaceId, 0, 1, backgroundRenderEffects_);

			// Render the right border tile.
			renderer_->renderSheetCell(right - backgroundCellWidth, i, backgroundSurfaceId, 2, 1, backgroundRenderEffects_);
		}

		// Render the top left corner.
		renderer_->renderSheetCell(left, top, backgroundSurfaceId, 0, 0);

		// Render the top right corner.
		renderer_->renderSheetCell(right - backgroundCellHeight, top, backgroundSurfaceId, 2, 0);

		// Render the bottom right corner.
		renderer_->renderSheetCell(left, bottom - backgroundCellHeight, backgroundSurfaceId, 0, 2);

		// Render the bottom left corner.
		renderer_->renderSheetCell(right - backgroundCellHeight, bottom - backgroundCellHeight, backgroundSurfaceId, 2, 2);

		// Render all border decorations.
		for (size_t i = 0; i < borderDecorations_.size(); i++)
		{
			PanelDecoration decoration = borderDecorations_[i];

			int sheetId = renderer_->getSheetIDByName(decoration.sheetName);

			if (sheetId >= 0)
			{
				boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(sheetId);

				int column = decoration.column;

				int row = decoration.row;

				int decorationLeft = (int)((decoration.left + 1) * (rects_->background->w / 2));

				int decorationTop = (int)((decoration.top + 1) * (rects_->background->h / 2));

				int offsetX = 0;

				int offsetY = 0;
				
				switch (decoration.origin)
				{
				case PANEL_DECORATION_ORIGIN_TOPMIDDLE:

					offsetX = (int)(((sheet->getCellWidth() * sheet->getScaleFactor()) / 2));

					break;

				case PANEL_DECORATION_ORIGIN_TOPRIGHT:

					offsetX = (int)((sheet->getCellWidth() * sheet->getScaleFactor()));

					break;

				case PANEL_DECORATION_ORIGIN_CENTERLEFT:

					offsetY = (int)(((sheet->getCellHeight() * sheet->getScaleFactor()) / 2));

					break;

				case PANEL_DECORATION_ORIGIN_CENTER:

					offsetX = (int)(((sheet->getCellWidth() * sheet->getScaleFactor()) / 2));

					offsetY = (int)(((sheet->getCellHeight() * sheet->getScaleFactor()) / 2));

					break;

				case PANEL_DECORATION_ORIGIN_CENTERRIGHT:

					offsetX = (int)((sheet->getCellWidth() * sheet->getScaleFactor()));

					offsetY = (int)(((sheet->getCellHeight() * sheet->getScaleFactor()) / 2));

					break;

				case PANEL_DECORATION_ORIGIN_BOTTOMLEFT:

					offsetY = (int)((sheet->getCellHeight() * sheet->getScaleFactor()));

					break;

				case PANEL_DECORATION_ORIGIN_BOTTOMMIDDLE:

					offsetX = (int)(((sheet->getCellWidth() * sheet->getScaleFactor()) / 2));

					offsetY = (int)((sheet->getCellHeight() * sheet->getScaleFactor()));

					break;

				case PANEL_DECORATION_ORIGIN_BOTTOMRIGHT:

					offsetX = (int)((sheet->getCellWidth() * sheet->getScaleFactor()));

					offsetY = (int)((sheet->getCellHeight() * sheet->getScaleFactor()));

					break;

				}


				int x = left + decorationLeft - offsetX;

				int y = top + decorationTop - offsetY;

				renderer_->renderSheetCell(x, y, sheetId, column, row);
			}
		}
	}
}

bool UiPanel::getFillBottom()
{
	return fillBottom_;
}

bool UiPanel::getFillLeft()
{
	return fillLeft_;
}

bool UiPanel::getFillRight()
{
	return fillRight_;
}

bool UiPanel::getFillTop()
{
	return fillTop_;
}

std::string UiPanel::getBackground()
{
	return backgroundSheetName_;
}

std::string UiPanel::getBackgroundPy()
{
	PythonReleaseGil unlocker;

	return getBackground();
}

void UiPanel::setBackground(std::string backgroundSheetName)
{
	backgroundSheetName_ = backgroundSheetName;
}

void UiPanel::setBackgroundPy(std::string backgroundSheetName)
{
	PythonReleaseGil unlocker;

	setBackground(backgroundSheetName);
}

void UiPanel::setBackgroundExtensionBottom(int value)
{
	backgroundExtensionBottom_ = value;
}

void UiPanel::setBackgroundExtensionLeft(int value)
{
	backgroundExtensionLeft_ = value;
}

void UiPanel::setBackgroundExtensionRight(int value)
{
	backgroundExtensionRight_ = value;
}

void UiPanel::setBackgroundExtensionTop(int value)
{
	backgroundExtensionTop_ = value;
}

bool UiPanel::hasVisibleChildren()
{
	for (size_t i = 0; i < childElements_.size(); i++)
	{
		if (childElements_[i]->isVisible_ == true)
		{
			return true;
		}
	}

	return false;
}

boost::shared_ptr<UiPanel> UiPanel::getChildPanelByName(std::string name)
{
	for (size_t i = 0; i < childPanels_.size(); i++)
	{
		if (childPanels_[i]->name_ == name)
		{
			return childPanels_[i];
		}
	}

	return nullptr;
}

boost::shared_ptr<UiPanel> UiPanel::getChildPanelByNamePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getChildPanelByName(name);
}

int UiPanel::getChildElementCount()
{
	return childElements_.size();
}

int UiPanel::getChildElementCountPy()
{
	PythonReleaseGil unlocker;

	return getChildElementCount();
}

int UiPanel::transformToScreenSpaceX(int value)
{
	// Recursively add on the parent's position.
	if (parentPanel_ != nullptr)
	{ 
		return parentPanel_->transformToScreenSpaceX(value + parentPanel_->rects_->borderInnerBoundary->x);
	}
	else
	{
		return value;
	}
}

int UiPanel::transformToScreenSpaceY(int value)
{
	// Recursively add on the parent's position.
	if (parentPanel_ != nullptr)
	{
		return parentPanel_->transformToScreenSpaceY(value + parentPanel_->rects_->borderInnerBoundary->y);
	}
	else
	{
		return value;
	}
}

std::string UiPanel::getNameTree(int depth)
{
	std::string value = getName();

	value += " Size (" + boost::lexical_cast<std::string>(getSize()->getWidth()) + "x" +
						 boost::lexical_cast<std::string>(getSize()->getHeight()) + ")" + 
		
			 " Padding (" + boost::lexical_cast<std::string>(paddingLeft_) + ", " +
							boost::lexical_cast<std::string>(paddingTop_) + ", " +
							boost::lexical_cast<std::string>(paddingRight_) + ", " +
							boost::lexical_cast<std::string>(paddingBottom_) + ") " + 

			 " Margins (" + boost::lexical_cast<std::string>(marginLeft_) + ", " +
							boost::lexical_cast<std::string>(marginTop_) + ", " +
							boost::lexical_cast<std::string>(marginRight_) + ", " +
							boost::lexical_cast<std::string>(marginBottom_) + ")" +

			 " Border (" + boost::lexical_cast<std::string>(borderLeft_) + ", " +
						   boost::lexical_cast<std::string>(borderTop_) + ", " +
						   boost::lexical_cast<std::string>(borderRight_) + ", " +
						   boost::lexical_cast<std::string>(borderBottom_) + ")";

	std::string indentation = "";

	for (int i = 0; i < depth; i++)
	{
		indentation += "  ";
	}

	for (size_t i = 0; i < childElements_.size(); i++)
	{
		if (childElements_[i]->getPanelElementType() == PANEL_ELEMENT_PANEL)
		{
			UiPanelPtr childPanel = boost::static_pointer_cast<UiPanel>(childElements_[i]);

			value += childPanel->getNameTree(depth + 1);
				
		}
		else
		{
			value += "\n" + indentation + "  Widget: " +  childElements_[i]->getName() +

										  "  Size (" + boost::lexical_cast<std::string>(childElements_[i]->getSize()->getWidth()) + "x" +
													   boost::lexical_cast<std::string>(childElements_[i]->getSize()->getHeight()) + ")" +

										  "  Padding (" + boost::lexical_cast<std::string>(childElements_[i]->paddingLeft_) + ", " +
													      boost::lexical_cast<std::string>(childElements_[i]->paddingTop_) + ", " +
														  boost::lexical_cast<std::string>(childElements_[i]->paddingRight_) + ", " +
														  boost::lexical_cast<std::string>(childElements_[i]->paddingBottom_) + ")";
		}
	}

	return "\n" + indentation + value;
}


int UiPanel::getNextFocusableChildIndex()
{
	int index = focusedChildIndex_ + 1;

	while (index < childElements_.size())
	{
		if (childElements_[index]->getIsFocusable() == true && childElements_[index]->isVisible_)
		{
			return index;
		}

		index++;
	}

	// None found. Return the size (EOF), so that it will know it is at the end.
	return childElements_.size();
}


int UiPanel::getPreviousFocusableChildIndex()
{
	int index = focusedChildIndex_ - 1;

	while (index >= 0)
	{
		if (childElements_[index]->getIsFocusable() == true && childElements_[index]->isVisible_)
		{
			return index;
		}

		index--;
	}

	// None found. Return -1 (BOF).
	return -1;
}


int UiPanel::getLastFocusableChildIndex()
{
	// Start at the end of the list and iterate backwards.
	// Return the index of the first element found that is focusable.
	if (childElements_.size() > 0)
	{
		int index = childElements_.size() - 1;

		while (index >= 0)
		{
			if (childElements_[index]->getIsFocusable() == true && childElements_[index]->isVisible_)
			{
				return index;
			}

			index--;
		}
	}

	// It's possible there are no focusable elements. In this case, return -1;
	return -1;
}

int UiPanel::getFirstFocusableChildIndex()
{	
	// Start at the beginning of the list and iterate forwards.
	// Return the index of the first element found that is focusable.
	if (childElements_.size() > 0)
	{
		int index = 0;

		while (index < childElements_.size())
		{
			if (childElements_[index]->getIsFocusable() == true && childElements_[index]->isVisible_)
			{
				return index;
			}

			index++;
		}
	}

	// It's possible there are no focusable elements. In this case, return -1;
	return -1;
}

boost::shared_ptr<Size> UiPanel::getFullSize()
{
	boost::shared_ptr<Size> size = boost::make_shared<Size>(Size(0, 0));

	return size;
}