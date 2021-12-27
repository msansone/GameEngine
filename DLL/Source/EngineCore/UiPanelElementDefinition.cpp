#include "..\..\Headers\EngineCore\UiPanelElementDefinition.hpp"

using namespace firemelon;

UiPanelElementDefinition::UiPanelElementDefinition()
{
	backgroundSheetName_ = "";

	borderBottom_ = 0;

	borderLeft_ = 0;

	borderRight_ = 0;

	borderTop_ = 0;

	buttonDownHandler_ = "";

	buttonUpHandler_ = "";

	caption_ = "";

	captionColorBlue_ = 1.0f;

	captionColorGreen_ = 1.0f;

	captionColorRed_ = 1.0f;

	captionFont_ = "";

	captionPositionLeft_ = 0.0f;

	captionPositionTop_ = 0.0f;

	captionScale_ = 1.0f;

	controlFlow_ = PANEL_CONTROL_FLOW_COLLAGE;

	fillBottom_ = false;

	fillLeft_ = false;

	fillRight_ = false;

	fillTop_ = false;

	focusable_ = false;

	focusWrap_ = false;

	frameMarginBottom_ = 0.0f;

	frameMarginLeft_ = 0.0f;

	frameMarginRight_ = 0.0f;

	frameMarginTop_ = 0.0f;

	gotFocusHandler_ = "";

	hiddenHandler_ = "";

	layoutName_ = "";

	lostFocusHandler_ = "";

	marginBottom_ = 0;

	marginLeft_ = 0;

	marginRight_ = 0;

	marginTop_ = 0;

	name_ = "";

	paddingBottom_ = 0;

	paddingLeft_ = 0;

	paddingRight_ = 0;

	paddingTop_ = 0;

	panelElementType_ = PANEL_ELEMENT_PANEL;
	
	params_ = "";
	
	positionStyle_ = PANEL_POSITION_MANUAL;
	
	positionX_ = 0;

	positionY_ = 0;

	selectElementHandler_ = "";
	
	shownHandler_ = "";
	
	type_ = "";

	visible_ = false;
}

UiPanelElementDefinition::~UiPanelElementDefinition()
{
}

#pragma region Properties


std::string UiPanelElementDefinition::getBackgroundSheetName()
{
	return backgroundSheetName_;
}

std::string UiPanelElementDefinition::getBackgroundSheetNamePy()
{
	PythonReleaseGil unlocker;

	return getBackgroundSheetName();
}

void UiPanelElementDefinition::setBackgroundSheetName(std::string value)
{
	backgroundSheetName_ = value;
}

void UiPanelElementDefinition::setBackgroundSheetNamePy(std::string value)
{
	PythonReleaseGil unlocker;

	setBackgroundSheetName(value);
}


int UiPanelElementDefinition::getBorderBottom()
{
	return borderBottom_;
}

int	UiPanelElementDefinition::getBorderBottomPy()
{
	PythonReleaseGil unlocker;

	return getBorderBottom();
}

void UiPanelElementDefinition::setBorderBottom(int value)
{
	borderBottom_ = value;
}

void UiPanelElementDefinition::setBorderBottomPy(int value)
{
	PythonReleaseGil unlocker;

	setBorderBottom(value);
}


int	UiPanelElementDefinition::getBorderLeft()
{
	return borderLeft_;
}

int	UiPanelElementDefinition::getBorderLeftPy()
{
	PythonReleaseGil unlocker;

	return getBorderLeft();
}

void UiPanelElementDefinition::setBorderLeft(int value)
{
	borderLeft_ = value;
}

void UiPanelElementDefinition::setBorderLeftPy(int value)
{
	PythonReleaseGil unlocker;

	setBorderLeft(value);
}


int	UiPanelElementDefinition::getBorderRight()
{
	return borderRight_;
}

int	UiPanelElementDefinition::getBorderRightPy()
{
	PythonReleaseGil unlocker;

	return getBorderRight();
}

void UiPanelElementDefinition::setBorderRight(int value)
{
	borderRight_ = value;
}

void UiPanelElementDefinition::setBorderRightPy(int value)
{
	PythonReleaseGil unlocker;

	setBorderRight(value);
}


int	UiPanelElementDefinition::getBorderTop()
{
	return borderTop_;
}

int	UiPanelElementDefinition::getBorderTopPy()
{
	PythonReleaseGil unlocker;

	return getBorderTop();
}

void UiPanelElementDefinition::setBorderTop(int value)
{
	borderTop_ = value;
}

void UiPanelElementDefinition::setBorderTopPy(int value)
{
	PythonReleaseGil unlocker;

	setBorderTop(value);
}


std::string UiPanelElementDefinition::getButtonDownHandler()
{
	return buttonDownHandler_;
}

std::string UiPanelElementDefinition::getButtonDownHandlerPy()
{
	PythonReleaseGil unlocker;

	return getButtonDownHandler();
}

void UiPanelElementDefinition::setButtonDownHandler(std::string value)
{
	buttonDownHandler_ = value;
}

void UiPanelElementDefinition::setButtonDownHandlerPy(std::string value)
{
	PythonReleaseGil unlocker;

	setButtonDownHandler(value);
}


std::string UiPanelElementDefinition::getButtonUpHandler()
{
	return buttonUpHandler_;
}

std::string UiPanelElementDefinition::getButtonUpHandlerPy()
{
	PythonReleaseGil unlocker;

	return getButtonUpHandler();

}

void UiPanelElementDefinition::setButtonUpHandler(std::string value)
{
	buttonUpHandler_ = value;
}

void UiPanelElementDefinition::setButtonUpHandlerPy(std::string value)
{
	PythonReleaseGil unlocker;

	setButtonUpHandler(value);
}


std::string UiPanelElementDefinition::getCaption()
{
	return caption_;
}

std::string UiPanelElementDefinition::getCaptionPy()
{
	PythonReleaseGil unlocker;

	return getCaption();
}

void UiPanelElementDefinition::setCaption(std::string value)
{
	caption_ = value;
}

void UiPanelElementDefinition::setCaptionPy(std::string value)
{
	PythonReleaseGil unlocker;

	setCaption(value);
}


float UiPanelElementDefinition::getCaptionColorBlue()
{
	return captionColorBlue_;
}

float UiPanelElementDefinition::getCaptionColorBluePy()
{
	PythonReleaseGil unlocker;

	return getCaptionColorBlue();
}

void UiPanelElementDefinition::setCaptionColorBlue(float value)
{
	captionColorBlue_ = value;
}

void UiPanelElementDefinition::setCaptionColorBluePy(float value)
{
	PythonReleaseGil unlocker;

	setCaptionColorBlue(value);
}


float UiPanelElementDefinition::getCaptionColorGreen()
{
	return captionColorGreen_;
}

float UiPanelElementDefinition::getCaptionColorGreenPy()
{
	PythonReleaseGil unlocker;

	return getCaptionColorGreen();
}

void UiPanelElementDefinition::setCaptionColorGreen(float value)
{
	captionColorGreen_ = value;
}

void UiPanelElementDefinition::setCaptionColorGreenPy(float value)
{
	PythonReleaseGil unlocker;

	setCaptionColorGreen(value);
}


float UiPanelElementDefinition::getCaptionColorRed()
{
	return captionColorRed_;
}

float UiPanelElementDefinition::getCaptionColorRedPy()
{
	PythonReleaseGil unlocker;

	return getCaptionColorRed();
}

void UiPanelElementDefinition::setCaptionColorRed(float value)
{
	captionColorRed_ = value;
}

void UiPanelElementDefinition::setCaptionColorRedPy(float value)
{
	PythonReleaseGil unlocker;

	setCaptionColorRed(value);
}


std::string UiPanelElementDefinition::getCaptionFont()
{
	return captionFont_;
}

std::string UiPanelElementDefinition::getCaptionFontPy()
{
	PythonReleaseGil unlocker;

	return getCaptionFont();
}

void UiPanelElementDefinition::setCaptionFont(std::string value)
{
	captionFont_ = value;
}

void UiPanelElementDefinition::setCaptionFontPy(std::string value)
{
	PythonReleaseGil unlocker;

	setCaptionFont(value);
}


float UiPanelElementDefinition::getCaptionPositionLeft()
{
	return captionPositionLeft_;
}

float UiPanelElementDefinition::getCaptionPositionLeftPy()
{
	PythonReleaseGil unlocker;

	return getCaptionPositionLeft();
}

void UiPanelElementDefinition::setCaptionPositionLeft(float value)
{
	captionPositionLeft_ = value;
}

void UiPanelElementDefinition::setCaptionPositionLeftPy(float value)
{
	PythonReleaseGil unlocker;

	setCaptionPositionLeft(value);
}


float UiPanelElementDefinition::getCaptionPositionTop()
{
	return captionPositionTop_;
}

float UiPanelElementDefinition::getCaptionPositionTopPy()
{
	PythonReleaseGil unlocker;

	return getCaptionPositionTop();
}

void UiPanelElementDefinition::setCaptionPositionTop(float value)
{
	captionPositionTop_ = value;
}

void UiPanelElementDefinition::setCaptionPositionTopPy(float value)
{
	PythonReleaseGil unlocker;

	setCaptionPositionTop(value);
}


float UiPanelElementDefinition::getCaptionScale()
{
	return captionScale_;
}

float UiPanelElementDefinition::getCaptionScalePy()
{
	PythonReleaseGil unlocker;

	return getCaptionScale();
}

void UiPanelElementDefinition::setCaptionScale(float value)
{
	captionScale_ = value;
}

void UiPanelElementDefinition::setCaptionScalePy(float value)
{
	PythonReleaseGil unlocker;

	setCaptionScale(value);
}


PanelControlFlow UiPanelElementDefinition::getControlFlow()
{
	return controlFlow_;
}

PanelControlFlow UiPanelElementDefinition::getControlFlowPy()
{
	PythonReleaseGil unlocker;

	return getControlFlow();
}

void UiPanelElementDefinition::setControlFlow(PanelControlFlow value)
{
	controlFlow_ = value;
}

void UiPanelElementDefinition::setControlFlowPy(PanelControlFlow value)
{
	PythonReleaseGil unlocker;

	setControlFlow(value);
}


bool UiPanelElementDefinition::getFocusable()
{
	return focusable_;
}

bool UiPanelElementDefinition::getFocusablePy()
{
	PythonReleaseGil unlocker;

	return getFocusable();
}

void UiPanelElementDefinition::setFocusable(bool value)
{
	focusable_ = value;
}

void UiPanelElementDefinition::setFocusablePy(bool value)
{
	PythonReleaseGil unlocker;

	setFocusable(value);
}


bool UiPanelElementDefinition::getFocusWrap()
{
	return focusWrap_;
}

bool UiPanelElementDefinition::getFocusWrapPy()
{
	PythonReleaseGil unlocker;

	return getFocusWrap();
}

void UiPanelElementDefinition::setFocusWrap(bool value)
{
	focusWrap_ = value;
}

void UiPanelElementDefinition::setFocusWrapPy(bool value)
{
	PythonReleaseGil unlocker;

	setFocusWrap(value);
}


float UiPanelElementDefinition::getFrameMarginBottom()
{
	return frameMarginBottom_;
}

float UiPanelElementDefinition::getFrameMarginBottomPy()
{
	PythonReleaseGil unlocker;

	return getFrameMarginBottom();
}

void UiPanelElementDefinition::setFrameMarginBottom(float value)
{
	frameMarginBottom_ = value;
}

void UiPanelElementDefinition::setFrameMarginBottomPy(float value)
{
	PythonReleaseGil unlocker;

	setFrameMarginBottom(value);
}


float UiPanelElementDefinition::getFrameMarginLeft()
{
	return frameMarginLeft_;
}

float UiPanelElementDefinition::getFrameMarginLeftPy()
{
	PythonReleaseGil unlocker;

	return getFrameMarginLeft();
}

void UiPanelElementDefinition::setFrameMarginLeft(float value)
{
	frameMarginLeft_ = value;
}

void UiPanelElementDefinition::setFrameMarginLeftPy(float value)
{
	PythonReleaseGil unlocker;

	setFrameMarginLeft(value);
}


float UiPanelElementDefinition::getFrameMarginRight()
{
	return frameMarginRight_;
}

float UiPanelElementDefinition::getFrameMarginRightPy()
{
	PythonReleaseGil unlocker;

	return getFrameMarginRight();
}

void UiPanelElementDefinition::setFrameMarginRight(float value)
{
	frameMarginRight_ = value;
}

void UiPanelElementDefinition::setFrameMarginRightPy(float value)
{
	PythonReleaseGil unlocker;

	setFrameMarginRight(value);
}


float UiPanelElementDefinition::getFrameMarginTop()
{
	return frameMarginTop_;
}

float UiPanelElementDefinition::getFrameMarginTopPy()
{
	PythonReleaseGil unlocker;

	return getFrameMarginTop();
}

void UiPanelElementDefinition::setFrameMarginTop(float value)
{
	frameMarginTop_ = value;
}

void UiPanelElementDefinition::setFrameMarginTopPy(float value)
{
	PythonReleaseGil unlocker;

	setFrameMarginTop(value);
}


std::string UiPanelElementDefinition::getGotFocusHandler()
{
	return gotFocusHandler_;
}

std::string UiPanelElementDefinition::getGotFocusHandlerPy()
{
	PythonReleaseGil unlocker;

	return getGotFocusHandler();
}

void UiPanelElementDefinition::setGotFocusHandler(std::string value)
{
	gotFocusHandler_ = value;
}

void UiPanelElementDefinition::setGotFocusHandlerPy(std::string value)
{
	PythonReleaseGil unlocker;

	setGotFocusHandler(value);
}


std::string UiPanelElementDefinition::getHiddenHandler()
{
	return hiddenHandler_;
}

std::string UiPanelElementDefinition::getHiddenHandlerPy()
{
	PythonReleaseGil unlocker;

	return getHiddenHandler();
}

void UiPanelElementDefinition::setHiddenHandler(std::string value)
{
	hiddenHandler_ = value;
}

void UiPanelElementDefinition::setHiddenHandlerPy(std::string value)
{
	PythonReleaseGil unlocker;

	setHiddenHandler(value);
}

PanelHorizontalAlignment UiPanelElementDefinition::getHorizontalAlignment()
{
	return horizontalAlignment_;
}

PanelHorizontalAlignment UiPanelElementDefinition::getHorizontalAlignmentPy()
{
	PythonReleaseGil unlocker;

	return getHorizontalAlignment();
}

void UiPanelElementDefinition::setHorizontalAlignment(PanelHorizontalAlignment value)
{
	horizontalAlignment_ = value;
}

void UiPanelElementDefinition::setHorizontalAlignmentPy(PanelHorizontalAlignment value)
{
	PythonReleaseGil unlocker;

	setHorizontalAlignment(value);
}


bool UiPanelElementDefinition::getFillBottom()
{
	return fillBottom_;
}

bool UiPanelElementDefinition::getFillBottomPy()
{
	PythonReleaseGil unlocker;

	return getFillBottom();
}

void UiPanelElementDefinition::setFillBottom(bool value)
{
	fillBottom_ = value;
}

void UiPanelElementDefinition::setFillBottomPy(bool value)
{
	PythonReleaseGil unlocker;
	
	setFillBottom(value);
}

bool UiPanelElementDefinition::getFillLeft()
{
	return fillLeft_;
}

bool UiPanelElementDefinition::getFillLeftPy()
{
	PythonReleaseGil unlocker;

	return getFillLeft();
}

void UiPanelElementDefinition::setFillLeft(bool value)
{
	fillLeft_ = value;
}

void UiPanelElementDefinition::setFillLeftPy(bool value)
{
	PythonReleaseGil unlocker;

	setFillLeft(value);
}

bool UiPanelElementDefinition::getFillRight()
{
	return fillRight_;
}

bool UiPanelElementDefinition::getFillRightPy()
{
	PythonReleaseGil unlocker;

	return getFillRight();
}

void UiPanelElementDefinition::setFillRight(bool value)
{
	fillRight_ = value;
}

void UiPanelElementDefinition::setFillRightPy(bool value)
{
	PythonReleaseGil unlocker;

	setFillRight(value);
}

bool UiPanelElementDefinition::getFillTop()
{
	return fillTop_;
}

bool UiPanelElementDefinition::getFillTopPy()
{
	PythonReleaseGil unlocker;

	return getFillTop();
}

void UiPanelElementDefinition::setFillTop(bool value)
{
	fillTop_ = value;
}

void UiPanelElementDefinition::setFillTopPy(bool value)
{
	PythonReleaseGil unlocker;

	setFillLeft(value);
}

std::string UiPanelElementDefinition::getLayout()
{
	return layoutName_;
}

std::string UiPanelElementDefinition::getLayoutPy()
{
	PythonReleaseGil unlocker;

	return getLayout();
}

void UiPanelElementDefinition::setLayout(std::string value)
{
	layoutName_ = value;
}

void UiPanelElementDefinition::setLayoutPy(std::string value)
{
	PythonReleaseGil unlocker;

	setLayout(value);
}


std::string UiPanelElementDefinition::getLostFocusHandler()
{
	return lostFocusHandler_;
}

std::string UiPanelElementDefinition::getLostFocusHandlerPy()
{
	PythonReleaseGil unlocker;

	return getLostFocusHandler();
}

void UiPanelElementDefinition::setLostFocusHandler(std::string value)
{
	lostFocusHandler_ = value;
}

void UiPanelElementDefinition::setLostFocusHandlerPy(std::string value)
{
	PythonReleaseGil unlocker;

	setLostFocusHandler(value);
}

float UiPanelElementDefinition::getMarginBottom()
{
	return marginBottom_;
}

float UiPanelElementDefinition::getMarginBottomPy()
{
	PythonReleaseGil unlocker;

	return getMarginBottom();
}

void UiPanelElementDefinition::setMarginBottom(float value)
{
	marginBottom_ = value;
}

void UiPanelElementDefinition::setMarginBottomPy(float value)
{
	PythonReleaseGil unlocker;

	setMarginBottom(value);
}

float UiPanelElementDefinition::getMarginLeft()
{
	return marginLeft_;
}

float UiPanelElementDefinition::getMarginLeftPy()
{
	PythonReleaseGil unlocker;

	return getMarginLeft();
}

void UiPanelElementDefinition::setMarginLeft(float value)
{
	marginLeft_ = value;
}

void UiPanelElementDefinition::setMarginLeftPy(float value)
{
	PythonReleaseGil unlocker;

	setMarginLeft(value);
}


float UiPanelElementDefinition::getMarginRight()
{
	return marginRight_;
}

float UiPanelElementDefinition::getMarginRightPy()
{
	PythonReleaseGil unlocker;

	return getMarginRight();
}

void UiPanelElementDefinition::setMarginRight(float value)
{
	marginRight_ = value;
}

void UiPanelElementDefinition::setMarginRightPy(float value)
{
	PythonReleaseGil unlocker;

	setMarginRight(value);
}

float UiPanelElementDefinition::getMarginTop()
{
	return marginTop_;
}

float UiPanelElementDefinition::getMarginTopPy()
{
	PythonReleaseGil unlocker;

	return getMarginTop();
}

void UiPanelElementDefinition::setMarginTop(float value)
{
	marginTop_ = value;
}

void UiPanelElementDefinition::setMarginTopPy(float value)
{
	PythonReleaseGil unlocker;

	setMarginTop(value);
}

std::string UiPanelElementDefinition::getName()
{
	return name_;
}

std::string UiPanelElementDefinition::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

void UiPanelElementDefinition::setName(std::string value)
{
	name_ = value;
}

void UiPanelElementDefinition::setNamePy(std::string value)
{
	PythonReleaseGil unlocker;

	setName(value);
}


int UiPanelElementDefinition::getPaddingBottom()
{
	return paddingBottom_;
}

int	UiPanelElementDefinition::getPaddingBottomPy()
{
	PythonReleaseGil unlocker;

	return getPaddingBottom();
}

void UiPanelElementDefinition::setPaddingBottom(int value)
{
	paddingBottom_ = value;
}

void UiPanelElementDefinition::setPaddingBottomPy(int value)
{
	PythonReleaseGil unlocker;

	setPaddingBottom(value);
}


int	UiPanelElementDefinition::getPaddingLeft()
{
	return paddingLeft_;
}

int	UiPanelElementDefinition::getPaddingLeftPy()
{
	PythonReleaseGil unlocker;

	return getPaddingLeft();
}

void UiPanelElementDefinition::setPaddingLeft(int value)
{
	paddingLeft_ = value;
}

void UiPanelElementDefinition::setPaddingLeftPy(int value)
{
	PythonReleaseGil unlocker;

	setPaddingLeft(value);
}


int	UiPanelElementDefinition::getPaddingRight()
{
	return paddingRight_;
}

int	UiPanelElementDefinition::getPaddingRightPy()
{
	PythonReleaseGil unlocker;

	return getPaddingRight();
}

void UiPanelElementDefinition::setPaddingRight(int value)
{
	paddingRight_ = value;
}

void UiPanelElementDefinition::setPaddingRightPy(int value)
{
	PythonReleaseGil unlocker;

	setPaddingRight(value);
}


int	UiPanelElementDefinition::getPaddingTop()
{
	return paddingTop_;
}

int	UiPanelElementDefinition::getPaddingTopPy()
{
	PythonReleaseGil unlocker;

	return getPaddingTop();
}

void UiPanelElementDefinition::setPaddingTop(int value)
{
	paddingTop_ = value;
}

void UiPanelElementDefinition::setPaddingTopPy(int value)
{
	PythonReleaseGil unlocker;

	setPaddingTop(value);
}


PanelElementType UiPanelElementDefinition::getPanelElementType()
{
	return panelElementType_;
}

PanelElementType UiPanelElementDefinition::getPanelElementTypePy()
{
	PythonReleaseGil unlocker;

	return getPanelElementType();
}

void UiPanelElementDefinition::setPanelElementType(PanelElementType elementType)
{
	panelElementType_ = elementType;
}

void UiPanelElementDefinition::setPanelElementTypePy(PanelElementType elementType)
{
	PythonReleaseGil unlocker;

	setPanelElementType(elementType);
}


std::string UiPanelElementDefinition::getParams()
{
	return params_;
}

std::string UiPanelElementDefinition::getParamsPy()
{
	PythonReleaseGil unlocker;

	return getParams();
}

void UiPanelElementDefinition::setParams(std::string value)
{
	params_ = value;
}

void UiPanelElementDefinition::setParamsPy(std::string value)
{
	PythonReleaseGil unlocker;

	setParams(value);
}


PanelPositionStyle UiPanelElementDefinition::getPositionStyle()
{
	return positionStyle_;
}

PanelPositionStyle UiPanelElementDefinition::getPositionStylePy()
{
	PythonReleaseGil unlocker;

	return getPositionStyle();
}

void UiPanelElementDefinition::setPositionStyle(PanelPositionStyle value)
{
	positionStyle_ = value;
}

void UiPanelElementDefinition::setPositionStylePy(PanelPositionStyle value)
{
	PythonReleaseGil unlocker;

	setPositionStyle(value);
}





int UiPanelElementDefinition::getPositionX()
{
	return positionX_;
}

int UiPanelElementDefinition::getPositionXPy()
{
	PythonReleaseGil unlocker;

	return getPositionX();
}

void UiPanelElementDefinition::setPositionX(int value)
{
	positionX_ = value;
}

void UiPanelElementDefinition::setPositionXPy(int value)
{
	PythonReleaseGil unlocker;

	setPositionX(value);
}

int UiPanelElementDefinition::getPositionY()
{
	return positionY_;
}

int UiPanelElementDefinition::getPositionYPy()
{
	PythonReleaseGil unlocker;

	return getPositionY();
}

void UiPanelElementDefinition::setPositionY(int value)
{
	positionY_ = value;
}

void UiPanelElementDefinition::setPositionYPy(int value)
{
	PythonReleaseGil unlocker;

	setPositionY(value);
}

std::string UiPanelElementDefinition::getSelectElementHandler()
{
	return selectElementHandler_;
}

std::string UiPanelElementDefinition::getSelectElementHandlerPy()
{
	PythonReleaseGil unlocker;

	return getSelectElementHandler();
}

void UiPanelElementDefinition::setSelectElementHandler(std::string value)
{
	selectElementHandler_ = value;
}

void UiPanelElementDefinition::setSelectElementHandlerPy(std::string value)
{
	PythonReleaseGil unlocker;

	setSelectElementHandler(value);
}



std::string UiPanelElementDefinition::getShownHandler()
{
	return shownHandler_;
}

std::string UiPanelElementDefinition::getShownHandlerPy()
{
	PythonReleaseGil unlocker;

	return getShownHandler();
}

void UiPanelElementDefinition::setShownHandler(std::string value)
{
	shownHandler_ = value;
}

void UiPanelElementDefinition::setShownHandlerPy(std::string value)
{
	PythonReleaseGil unlocker;

	setShownHandler(value);
}


std::string UiPanelElementDefinition::getType()
{
	return type_;
}

std::string UiPanelElementDefinition::getTypePy()
{
	PythonReleaseGil unlocker;

	return getType();
}

void UiPanelElementDefinition::setType(std::string value)
{
	type_ = value;
}

void UiPanelElementDefinition::setTypePy(std::string value)
{
	PythonReleaseGil unlocker;

	setType(value);
}


PanelVerticalAlignment UiPanelElementDefinition::getVerticalAlignment()
{
	return verticalAlignment_;
}

PanelVerticalAlignment UiPanelElementDefinition::getVerticalAlignmentPy()
{
	PythonReleaseGil unlocker;

	return getVerticalAlignment();
}

void UiPanelElementDefinition::setVerticalAlignment(PanelVerticalAlignment value)
{
	verticalAlignment_ = value;
}

void UiPanelElementDefinition::setVerticalAlignmentPy(PanelVerticalAlignment value)
{
	PythonReleaseGil unlocker;

	setVerticalAlignment(value);
}


bool UiPanelElementDefinition::getVisible()
{
	return visible_;
}

bool UiPanelElementDefinition::getVisiblePy()
{
	PythonReleaseGil unlocker;

	return getVisible();
}

void UiPanelElementDefinition::setVisible(bool value)
{
	visible_ = value;
}

void UiPanelElementDefinition::setVisiblePy(bool value)
{
	PythonReleaseGil unlocker;

	setVisible(value);
}

#pragma endregion

#pragma region Functions

void UiPanelElementDefinition::addElementPy(boost::shared_ptr<UiPanelElementDefinition> element)
{
	PythonReleaseGil unlocker;

	addElement(element);
}

void UiPanelElementDefinition::addElement(boost::shared_ptr<UiPanelElementDefinition> element)
{
	childElements_.push_back(element);

	return;
}


#pragma endregion