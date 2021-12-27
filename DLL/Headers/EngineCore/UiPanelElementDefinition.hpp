/* -------------------------------------------------------------------------
** UiPanelElementDefinition.hpp
**
** The UiPanelElementDefinition class stores the properties used to create a UI element
** (i.e. a panel or widget). It mirrors the properties of the JSON objects
** in the panels file. It's usage is to dynamically create UI elements at
** runtime, as opposed to reading them from the panels JSON file.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIPANELELEMENTDEFINITION_HPP_
#define _UIPANELELEMENTDEFINITION_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <vector>

#include "BaseIds.hpp"

namespace firemelon
{
	enum PanelDecorationOrigin
	{
		PANEL_DECORATION_ORIGIN_TOPLEFT = 0,
		PANEL_DECORATION_ORIGIN_TOPMIDDLE = 1,
		PANEL_DECORATION_ORIGIN_TOPRIGHT = 2,
		PANEL_DECORATION_ORIGIN_CENTERLEFT = 3,
		PANEL_DECORATION_ORIGIN_CENTER = 4,
		PANEL_DECORATION_ORIGIN_CENTERRIGHT = 5,
		PANEL_DECORATION_ORIGIN_BOTTOMLEFT = 6,
		PANEL_DECORATION_ORIGIN_BOTTOMMIDDLE = 7,
		PANEL_DECORATION_ORIGIN_BOTTOMRIGHT = 8
	};

	struct FIREMELONAPI PanelDecoration
	{
		std::string				sheetName;
		int						row;
		int						column;
		float					left;
		float					top;
		PanelDecorationOrigin	origin;
	};

	enum PanelElementType
	{
		PANEL_ELEMENT_PANEL = 0,
		PANEL_ELEMENT_WIDGET = 1
	};

	enum PanelControlFlow
	{
		PANEL_CONTROL_FLOW_BOOK = 0,
		PANEL_CONTROL_FLOW_COLLAGE = 1
	};

	enum PanelPositionStyle
	{
		// Uses an x, y position for the panel position.
		PANEL_POSITION_MANUAL = 0,

		// Position the panel based on the layout properties.
		PANEL_POSITION_AUTO = 1
	};

	enum PanelHorizontalAlignment
	{
		PANEL_HORIZONTAL_ALIGNMENT_LEFT = 0,
		PANEL_HORIZONTAL_ALIGNMENT_RIGHT = 1,
		PANEL_HORIZONTAL_ALIGNMENT_CENTER = 2
	};

	enum PanelVerticalAlignment
	{
		PANEL_VERTICAL_ALIGNMENT_BOTTOM = 0,
		PANEL_VERTICAL_ALIGNMENT_TOP = 1,
		PANEL_VERTICAL_ALIGNMENT_CENTER = 2
	};

	class FIREMELONAPI UiPanelElementDefinition
	{
	public:
		friend class Ui;

		UiPanelElementDefinition();
		virtual ~UiPanelElementDefinition();

		#pragma region Properties

		std::string					getBackgroundSheetName();
		std::string					getBackgroundSheetNamePy();
		void						setBackgroundSheetName(std::string value);
		void						setBackgroundSheetNamePy(std::string value);

		int							getBorderBottom();
		int							getBorderBottomPy();
		void						setBorderBottom(int value);
		void						setBorderBottomPy(int value);

		int							getBorderLeft();
		int							getBorderLeftPy();
		void						setBorderLeft(int value);
		void						setBorderLeftPy(int value);

		int							getBorderRight();
		int							getBorderRightPy();
		void						setBorderRight(int value);
		void						setBorderRightPy(int value);

		int							getBorderTop();
		int							getBorderTopPy();
		void						setBorderTop(int value);
		void						setBorderTopPy(int value);
		
		std::string					getButtonDownHandler();
		std::string					getButtonDownHandlerPy();
		void						setButtonDownHandler(std::string value);
		void						setButtonDownHandlerPy(std::string value);
		
		std::string					getButtonUpHandler();
		std::string					getButtonUpHandlerPy();
		void						setButtonUpHandler(std::string value);
		void						setButtonUpHandlerPy(std::string value);

		std::string					getCaption();
		std::string					getCaptionPy();
		void						setCaption(std::string value);
		void						setCaptionPy(std::string value);

		float						getCaptionColorBlue();
		float						getCaptionColorBluePy();
		void						setCaptionColorBlue(float value);
		void						setCaptionColorBluePy(float value);

		float						getCaptionColorGreen();
		float						getCaptionColorGreenPy();
		void						setCaptionColorGreen(float value);
		void						setCaptionColorGreenPy(float value);

		float						getCaptionColorRed();
		float						getCaptionColorRedPy();
		void						setCaptionColorRed(float value);
		void						setCaptionColorRedPy(float value);

		std::string					getCaptionFont();
		std::string					getCaptionFontPy();
		void						setCaptionFont(std::string value);
		void						setCaptionFontPy(std::string value);

		float						getCaptionPositionLeft();
		float						getCaptionPositionLeftPy();
		void						setCaptionPositionLeft(float value);
		void						setCaptionPositionLeftPy(float value);

		float						getCaptionPositionTop();
		float						getCaptionPositionTopPy();
		void						setCaptionPositionTop(float value);
		void						setCaptionPositionTopPy(float value);

		float						getCaptionScale();
		float						getCaptionScalePy();
		void						setCaptionScale(float value);
		void						setCaptionScalePy(float value);

		PanelControlFlow			getControlFlow();
		PanelControlFlow			getControlFlowPy();
		void						setControlFlow(PanelControlFlow value);
		void						setControlFlowPy(PanelControlFlow value);

		bool						getFillBottom();
		bool						getFillBottomPy();
		void						setFillBottom(bool value);
		void						setFillBottomPy(bool value);

		bool						getFillLeft();
		bool						getFillLeftPy();
		void						setFillLeft(bool value);
		void						setFillLeftPy(bool value);

		bool						getFillRight();
		bool						getFillRightPy();
		void						setFillRight(bool value);
		void						setFillRightPy(bool value);

		bool						getFillTop();
		bool						getFillTopPy();
		void						setFillTop(bool value);
		void						setFillTopPy(bool value);

		bool						getFocusable();
		bool						getFocusablePy();
		void						setFocusable(bool value);
		void						setFocusablePy(bool value);

		bool						getFocusWrap();
		bool						getFocusWrapPy();
		void						setFocusWrap(bool value);
		void						setFocusWrapPy(bool value);

		float						getFrameMarginBottom();
		float						getFrameMarginBottomPy();
		void						setFrameMarginBottom(float value);
		void						setFrameMarginBottomPy(float value);

		float						getFrameMarginLeft();
		float						getFrameMarginLeftPy();
		void						setFrameMarginLeft(float value);
		void						setFrameMarginLeftPy(float value);

		float						getFrameMarginRight();
		float						getFrameMarginRightPy();
		void						setFrameMarginRight(float value);
		void						setFrameMarginRightPy(float value);

		float						getFrameMarginTop();
		float						getFrameMarginTopPy();
		void						setFrameMarginTop(float value);
		void						setFrameMarginTopPy(float value);

		std::string					getGotFocusHandler();
		std::string					getGotFocusHandlerPy();
		void						setGotFocusHandler(std::string value);
		void						setGotFocusHandlerPy(std::string value);

		std::string					getHiddenHandler();
		std::string					getHiddenHandlerPy();
		void						setHiddenHandler(std::string value);
		void						setHiddenHandlerPy(std::string value);

		PanelHorizontalAlignment	getHorizontalAlignment();
		PanelHorizontalAlignment	getHorizontalAlignmentPy();
		void						setHorizontalAlignment(PanelHorizontalAlignment value);
		void						setHorizontalAlignmentPy(PanelHorizontalAlignment value);

		std::string					getLayout();
		std::string					getLayoutPy();
		void						setLayout(std::string value);
		void						setLayoutPy(std::string value);

		std::string					getLostFocusHandler();
		std::string					getLostFocusHandlerPy();
		void						setLostFocusHandler(std::string value);
		void						setLostFocusHandlerPy(std::string value);

		float						getMarginBottom();
		float						getMarginBottomPy();
		void						setMarginBottom(float value);
		void						setMarginBottomPy(float value);

		float						getMarginLeft();
		float						getMarginLeftPy();
		void						setMarginLeft(float value);
		void						setMarginLeftPy(float value);

		float						getMarginRight();
		float						getMarginRightPy();
		void						setMarginRight(float value);
		void						setMarginRightPy(float value);

		float						getMarginTop();
		float						getMarginTopPy();
		void						setMarginTop(float value);
		void						setMarginTopPy(float value);

		std::string					getName();
		std::string					getNamePy();
		void						setName(std::string value);
		void						setNamePy(std::string value);

		int							getPaddingBottom();
		int							getPaddingBottomPy();
		void						setPaddingBottom(int value);
		void						setPaddingBottomPy(int value);

		int							getPaddingLeft();
		int							getPaddingLeftPy();
		void						setPaddingLeft(int value);
		void						setPaddingLeftPy(int value);

		int							getPaddingRight();
		int							getPaddingRightPy();
		void						setPaddingRight(int value);
		void						setPaddingRightPy(int value);

		int							getPaddingTop();
		int							getPaddingTopPy();
		void						setPaddingTop(int value);
		void						setPaddingTopPy(int value);

		PanelElementType			getPanelElementType();
		PanelElementType			getPanelElementTypePy();
		void						setPanelElementType(PanelElementType elementType);
		void						setPanelElementTypePy(PanelElementType elementType);

		std::string					getParams();
		std::string					getParamsPy();
		void						setParams(std::string value);
		void						setParamsPy(std::string value);

		PanelPositionStyle			getPositionStyle();
		PanelPositionStyle			getPositionStylePy();
		void						setPositionStyle(PanelPositionStyle value);
		void						setPositionStylePy(PanelPositionStyle value);

		int							getPositionX();
		int							getPositionXPy();
		void						setPositionX(int value);
		void						setPositionXPy(int value);

		int							getPositionY();
		int							getPositionYPy();
		void						setPositionY(int value);
		void						setPositionYPy(int value);

		std::string					getSelectElementHandler();
		std::string					getSelectElementHandlerPy();
		void						setSelectElementHandler(std::string value);
		void						setSelectElementHandlerPy(std::string value);

		std::string					getShownHandler();
		std::string					getShownHandlerPy();
		void						setShownHandler(std::string value);
		void						setShownHandlerPy(std::string value);

		std::string					getType();
		std::string					getTypePy();
		void						setType(std::string value);
		void						setTypePy(std::string value);

		PanelVerticalAlignment		getVerticalAlignment();
		PanelVerticalAlignment		getVerticalAlignmentPy();
		void						setVerticalAlignment(PanelVerticalAlignment value);
		void						setVerticalAlignmentPy(PanelVerticalAlignment value);

		bool						getVisible();
		bool						getVisiblePy();
		void						setVisible(bool value);
		void						setVisiblePy(bool value);

		#pragma endregion


		#pragma region Functions

		void	addElement(boost::shared_ptr<UiPanelElementDefinition> element);
		void	addElementPy(boost::shared_ptr<UiPanelElementDefinition> element);

		#pragma endregion


	protected:

	private:
		
		std::string						backgroundSheetName_;

		int								borderBottom_;

		std::vector<PanelDecoration>	borderDecorations_;

		int								borderLeft_;

		int								borderRight_;

		int								borderTop_;

		std::string						buttonDownHandler_;

		std::string						buttonUpHandler_;

		std::string						caption_;

		float							captionColorBlue_;

		float							captionColorGreen_;

		float							captionColorRed_;

		std::string						captionFont_;

		float							captionPositionLeft_;

		float							captionPositionTop_;

		float							captionScale_;

		std::vector<boost::shared_ptr<UiPanelElementDefinition>>	childElements_;

		PanelControlFlow				controlFlow_;

		std::vector<PanelDecoration>	decorations_;

		bool							fillBottom_;

		bool							fillLeft_;

		bool							fillRight_;

		bool							fillTop_;

		bool							focusable_;

		bool							focusWrap_;

		float							frameMarginBottom_;

		float							frameMarginLeft_;

		float							frameMarginRight_;

		float							frameMarginTop_;

		std::string						gotFocusHandler_;

		std::string						hiddenHandler_;

		PanelHorizontalAlignment		horizontalAlignment_;

		std::string						layoutName_;

		std::string						lostFocusHandler_;

		int								marginBottom_;

		int								marginLeft_;

		int								marginRight_;

		int								marginTop_;

		std::string						name_;

		int								paddingBottom_;

		int								paddingLeft_;

		int								paddingRight_;

		int								paddingTop_;

		PanelElementType				panelElementType_;

		std::string						params_;

		int								positionX_;

		int								positionY_;

		PanelPositionStyle				positionStyle_;

		std::string						selectElementHandler_;

		std::string						shownHandler_;

		std::string						type_;

		UiWidgetId						uiWidgetId_;

		PanelVerticalAlignment			verticalAlignment_;

		bool							visible_;
	};
}

#endif // _UIPANELELEMENTDEFINITION_HPP_