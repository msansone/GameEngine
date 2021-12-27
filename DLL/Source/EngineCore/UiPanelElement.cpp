#include "..\..\Headers\EngineCore\UiPanelElement.hpp"

using namespace firemelon;

UiPanelElement::UiPanelElement() : 
	visibilityChangedSignal_(new VisibilityChangedSignalRaw)
{
	backgroundSheetName_ = "";

	backgroundRenderEffects_ = boost::make_shared<RenderEffects>(RenderEffects());

	backgroundExtensionBottom_ = 0;

	backgroundExtensionLeft_ = 0;

	backgroundExtensionRight_ = 0;

	backgroundExtensionTop_ = 0;

	size_ = boost::make_shared<Size>(Size(0, 0));

	position_ = boost::make_shared<Position>(Position(0, 0));

	focusable_ = true;

	hasFocus_ = false;

	borderBottom_ = 0;

	borderLeft_ = 0;

	borderRight_ = 0;

	borderTop_ = 0;

	hiddenEventCalled_ = false;

	hiddenHandler_ = "";

	paddingLeft_ = 0;

	paddingTop_ = 0;

	paddingRight_ = 0;

	paddingBottom_ = 0;

	marginBottom_ = 0;

	marginLeft_ = 0;

	marginRight_ = 0;

	marginTop_ = 0;

	rects_ = boost::make_shared<UiPanelRects>(UiPanelRects());

	rects_->levelOne = boost::make_shared<Rect>(Rect());

	rects_->levelTwo = boost::make_shared<Rect>(Rect());

	rects_->levelThree = boost::make_shared<Rect>(Rect());

	rects_->levelFour = boost::make_shared<Rect>(Rect());

	rects_->paddingOuterBoundary = rects_->levelOne;

	rects_->paddingInnerBoundary = rects_->levelTwo;

	rects_->background = rects_->levelTwo;

	rects_->borderOuterBoundary = rects_->levelTwo;

	rects_->borderInnerBoundary = rects_->levelThree;

	rects_->marginsOuterBoundary = rects_->levelThree;

	rects_->marginsInnerBoundary = rects_->levelFour;

	shownEventCalled_ = false;

	shownHandler_ = "";
}

UiPanelElement::~UiPanelElement()
{
}

boost::shared_ptr<Position> UiPanelElement::getPosition()
{
	return position_;
}

boost::shared_ptr<Position> UiPanelElement::getPositionPy()
{
	PythonReleaseGil unlocker;

	return getPosition();
}

boost::shared_ptr<Size> UiPanelElement::getSize()
{
	return size_;
}

boost::shared_ptr<Size>	UiPanelElement::getSizePy()
{
	PythonReleaseGil unlocker;

	return getSize();
}
std::string	UiPanelElement::getName()
{
	return name_;
}

std::string	UiPanelElement::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

void UiPanelElement::setName(std::string name)
{
	name_ = name;
}

void UiPanelElement::created()
{
}

bool UiPanelElement::getIsVisible()
{
	bool isParentVisible;
	
	if (parentElement_ != nullptr)
	{
		isParentVisible = parentElement_->getIsVisible();
	}
	else
	{
		// Root panel has no parent.
		isParentVisible = true;
	}

	return isVisible_ && isParentVisible;
}

bool UiPanelElement::getIsVisiblePy()
{
	PythonReleaseGil unlocker;

	return getIsVisible();
}

void UiPanelElement::setIsVisible(bool isVisible, bool reset)
{
	bool previousIsVisible = getIsVisible();

	isVisible_ = isVisible;

	bool newIsVisible = getIsVisible();

	if (previousIsVisible != newIsVisible)
	{
		(*visibilityChangedSignal_)(name_);
	}

	if (parentElement_ != nullptr && reset == true)
	{
		parentElement_->reset();
	}
}

void UiPanelElement::setIsVisiblePy(bool isVisible)
{
	PythonReleaseGil unlocker;

	setIsVisible(isVisible);
}

bool UiPanelElement::getIsFocusable()
{
	return focusable_;
}

bool UiPanelElement::getIsFocusablePy()
{
	PythonReleaseGil unlocker;

	return getIsFocusable();
}

boost::shared_ptr<UiPanel> UiPanelElement::getParentPanel()
{
	return parentPanel_;
}

boost::shared_ptr<UiPanel> UiPanelElement::getParentPanelPy()
{
	PythonReleaseGil unlocker;

	return getParentPanel();
}

RendererPtr UiPanelElement::getRenderer()
{
	return renderer_;
}

FontManagerPtr UiPanelElement::getFontManager()
{
	return fontManager_;
}

int UiPanelElement::getPaddingBottom()
{
	return paddingBottom_;
}

void UiPanelElement::setPaddingBottom(int value)
{
	paddingBottom_ = value;
}

int	UiPanelElement::getPaddingLeft()
{
	return paddingLeft_;
}

int	UiPanelElement::getPaddingRight()
{
	return paddingRight_;
}

int	UiPanelElement::getPaddingTop()
{
	return paddingTop_;
}

void UiPanelElement::calculateRectSizes(int contentWidth, int contentHeight, int parentWidth, int parentHeight)
{
	// Size the background.
	rects_->background->w = parentWidth;

	rects_->background->h = parentHeight;

	// Size the border inner and outer boundary rects. Outer boundary is the same as the background rect.
	rects_->borderOuterBoundary->w = rects_->background->w;

	rects_->borderOuterBoundary->h = rects_->background->h;

	rects_->borderInnerBoundary->w = parentWidth - borderLeft_ - borderRight_;

	rects_->borderInnerBoundary->h = parentHeight - borderLeft_ - borderRight_;


	// Padding
	rects_->paddingOuterBoundary->w = contentWidth + paddingLeft_ + paddingRight_ + marginLeft_ + marginRight_ + borderLeft_ + borderRight_;

	rects_->paddingOuterBoundary->h = contentHeight + paddingTop_ + paddingBottom_ + marginTop_ + marginBottom_ + borderTop_ + borderBottom_;

	rects_->paddingInnerBoundary->w = contentWidth + marginLeft_ + marginRight_ + borderLeft_ + borderRight_;

	rects_->paddingInnerBoundary->h = contentHeight + marginTop_ + marginBottom_ + borderTop_ + borderBottom_;


	// Margin
	rects_->marginsOuterBoundary->w = contentWidth + marginLeft_ + marginRight_;

	rects_->marginsOuterBoundary->h = contentHeight + marginTop_ + marginBottom_;

	rects_->marginsInnerBoundary->w = contentWidth;

	rects_->marginsInnerBoundary->h = contentHeight;


	// Size and position should match the content rect.
	size_->setWidth(contentWidth);
	
	size_->setHeight(contentHeight);
}

PanelElementType UiPanelElement::getPanelElementType()
{
	return elementType_;
}

int UiPanelElement::getBorderBottom()
{
	return borderBottom_;
}

int UiPanelElement::getBorderLeft()
{
	return borderLeft_;
}

int UiPanelElement::getBorderRight()
{
	return borderRight_;
}

int UiPanelElement::getBorderTop()
{
	return borderTop_;
}

void UiPanelElement::reset()
{
}