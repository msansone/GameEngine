/* -------------------------------------------------------------------------
** UiPanel.hpp
**
** The UiPanel stores references to UI Widgets, and is used to set their
** layout style within the panel. Multiple UI Panels
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIPANEL_HPP_
#define _UIPANEL_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/shared_ptr.hpp>

#include <string>
#include <vector>

#include "Position.hpp"
#include "PythonGil.hpp"
#include "UiPanelElement.hpp"

namespace firemelon
{
	class FIREMELONAPI UiPanel : public UiPanelElement
	{
	public:
		typedef std::vector<boost::shared_ptr<UiPanelElement>> PanelElementList;
		typedef std::vector<boost::shared_ptr<UiPanel>> PanelList;

		friend class Ui;
		friend class UiHorizontalStackPanel;
		friend class UiPanelContainer;
		friend class UiVerticalStackPanel;
		friend class UiWidget;

		UiPanel(std::string panelName);
		virtual ~UiPanel();

		std::string							getBackground();
		std::string							getBackgroundPy();

		boost::shared_ptr<UiPanel>			getChildPanelByName(std::string name);
		boost::shared_ptr<UiPanel>			getChildPanelByNamePy(std::string name);

		int									getChildElementCount();
		int									getChildElementCountPy();

		bool								getFillBottom();

		bool								getFillLeft();

		bool								getFillRight();

		bool								getFillTop();

		boost::shared_ptr<UiPanelElement>	getFocusedWidget();

		virtual SizePtr						getFullSize();

		virtual bool						getIsFocusable();

		std::string							getLayoutStyle();
		std::string							getLayoutStylePy();

		PanelHorizontalAlignment			getPanelHorizontalAlignment();
		PanelHorizontalAlignment			getPanelHorizontalAlignmentPy();

		PanelVerticalAlignment				getPanelVerticalAlignment();
		PanelVerticalAlignment				getPanelVerticalAlignmentPy();

		void								setBackground(std::string backgroundSheetName);
		void								setBackgroundPy(std::string backgroundSheetName);

		void								setBackgroundExtensionBottom(int value);

		void								setBackgroundExtensionLeft(int value);

		void								setBackgroundExtensionRight(int value);

		void								setBackgroundExtensionTop(int value);

		void								setLayoutStyle(std::string layoutStyle);
		void								setLayoutStylePy(std::string layoutStyle);

		void								setHorizontalAlignment(PanelHorizontalAlignment alignment);
		void								setHorizontalAlignmentPy(PanelHorizontalAlignment alignment);

		void								setVerticalAlignment(PanelVerticalAlignment alignment);
		void								setVerticalAlignmentPy(PanelVerticalAlignment alignment);

	protected:

		PanelElementList			childElements_;

		PanelList					childPanels_;

		bool						fillBottom_;

		bool						fillLeft_;

		bool						fillRight_;

		bool						fillTop_;

		PanelHorizontalAlignment	horizontalAlign_;

		PanelVerticalAlignment		verticalAlign_;

	private:
		
		virtual void	calculateMinimumBoundingSize();

		virtual void	calculateMinimumBoundingSizeBase();
		
		virtual void	created();

		bool			focusElement(std::string elementName);

		bool			focusElement(int index);

		bool			focusFirstElement();

		bool			focusLastElement();

		bool			focusNextElement();

		bool			focusPreviousElement();

		int				getFirstFocusableChildIndex();

		int				getLastFocusableChildIndex();

		std::string		getNameTree(int depth);

		int				getNextFocusableChildIndex();

		int				getPreviousFocusableChildIndex();

		bool			hasVisibleChildren();

		// Locate the child elements within the panel's main content region.
		virtual void	locateChildElements();

		virtual void	locateElementsBase();

		// It's important to remember that, all rectangle positions in a panel are relative
		// to the parent panel. When it comes time to render, the parent will pass down its
		// position to use as an offset.
		void			renderBackground(int parentX, int parentY);

		void			renderBase(int parentX, int parentY);

		void			renderBorder(int parentX, int parentY);

		virtual void	reset();

		void			setInitialFocus();

		int				transformToScreenSpaceX(int value);

		int				transformToScreenSpaceY(int value);

		void			update(double time);

		void			updateBase(double time);

		std::vector<PanelDecoration>	borderDecorations_;

		std::string						caption_;

		std::string						captionFont_;

		float							captionLeft_;

		RenderEffectsPtr				captionRenderEffects_;

		float							captionTop_;

		std::vector<int>				childElementIndexBackStack_;

		PanelControlFlow				controlFlow_;

		std::vector<PanelDecoration>	decorations_;

		int								focusedChildIndex_;

		int								frameMarginLeft_;

		int								frameMarginRight_;

		int								frameMarginTop_;

		int								frameMarginBottom_;
		
		std::string						layoutStyle_;
		
		PanelPositionStyle				positionStyle_;

		bool							focusWrap_;
	};

	typedef boost::shared_ptr<UiPanel> UiPanelPtr;
}

#endif // _UIPANEL_HPP_