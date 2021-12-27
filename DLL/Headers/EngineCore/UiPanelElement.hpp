/* -------------------------------------------------------------------------
** UiPanelElement.hpp
**
** The UiPanelElement represents any element that can be contained by a UI
** panel.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIPANELELEMENT_HPP_
#define _UIPANELELEMENT_HPP_

#include <boost/signals2.hpp>

#include "Debugger.hpp"
#include "FontManager.hpp"
#include "Position.hpp"
#include "Renderer.hpp"
#include "Size.hpp"
#include "UiPanelElementDefinition.hpp"

namespace firemelon
{
	typedef boost::shared_ptr<boost::signals2::signal<void(std::string name)>> VisibilityChangedSignal;
	typedef boost::signals2::signal<void(std::string name)> VisibilityChangedSignalRaw;

	// So this is a little tricky and needs to be explained.
	// There are four rectangles that make up a panel element, but some of them can
	// be thought of differnetly depending on the context. This is because the rects
	// have a kind of "thickness". That is, their width isn't 1 pixel. Making it
	// more tricky is that the widths on all sides aren't necessarily the same.
	// For example, a padding region can have a top side whose width is 8 and a bottom
	// side whose width is 16. The padding region surrounds the panel border region.

	// So, given this, the padding region has two rects, the outer boundary, and the 
	// inner boundary. Likewise, the border region also has an outer and inner boundary.
	// Where it gets really crazy is that the padding inner boundary and the border
	// outer boundary are going to be the same rectangle. Now, this gets pretty confusing
	// in terms of readability, because it's very easy to lose track of whether
	// you're dealing with the inner or outer boundary, and which rectangle is which.

	// So, my solution is to create four source rectangles, and have named pointers that
	// point to them, almost like an alias.

	struct UiPanelRects
	{
		// The source rects.
		boost::shared_ptr<Rect>	levelOne;
		boost::shared_ptr<Rect>	levelTwo;
		boost::shared_ptr<Rect>	levelThree;
		boost::shared_ptr<Rect>	levelFour;

		// The outermost rect of a panel element. Point to level one. 
		boost::shared_ptr<Rect>	paddingOuterBoundary;

		// The next innermost rects. Level two.
		boost::shared_ptr<Rect>	paddingInnerBoundary;
		boost::shared_ptr<Rect>	background;
		boost::shared_ptr<Rect>	borderOuterBoundary;

		// The next innermost rects. Level three.
		boost::shared_ptr<Rect>	borderInnerBoundary;
		boost::shared_ptr<Rect>	marginsOuterBoundary;

		// The last innermost rects. Level four.
		boost::shared_ptr<Rect>	marginsInnerBoundary;
	};

	class UiPanelElement
	{
	public:
		friend class GameEngine;
		friend class Ui;
		friend class UiBookPanel;
		friend class UiHorizontalStackPanel;
		friend class UiPanel;
		friend class UiVerticalStackPanel;

		UiPanelElement();
		virtual ~UiPanelElement();

		int							getBorderBottom();
		int							getBorderLeft();
		int							getBorderRight();
		int							getBorderTop();

		virtual bool				getIsFocusable();
		bool						getIsFocusablePy();

		bool						getIsVisible();
		bool						getIsVisiblePy();

		std::string					getName();
		std::string					getNamePy();

		int							getPaddingBottom();

		void						setPaddingBottom(int value);

		int							getPaddingLeft();

		int							getPaddingRight();

		int							getPaddingTop();

		PanelElementType			getPanelElementType();

		boost::shared_ptr<UiPanel>	getParentPanel();
		boost::shared_ptr<UiPanel>	getParentPanelPy();

		boost::shared_ptr<Position>	getPosition();
		boost::shared_ptr<Position>	getPositionPy();

		boost::shared_ptr<Size>		getSize();
		boost::shared_ptr<Size>		getSizePy();

		void						setIsVisible(bool isVisible, bool reset = true);
		void						setIsVisiblePy(bool isVisible);

		void						calculateRectSizes(int contentWidth, int contentHeight, int parentWidth, int parentHeight);

	protected:

		FontManagerPtr			getFontManager();
		RendererPtr				getRenderer();

		int									backgroundExtensionBottom_;

		int									backgroundExtensionLeft_;

		int									backgroundExtensionRight_;

		int									backgroundExtensionTop_;

		RenderEffectsPtr					backgroundRenderEffects_;

		std::string							backgroundSheetName_;

		int									borderBottom_;

		int									borderLeft_;

		int									borderRight_;

		int									borderTop_;

		DebuggerPtr							debugger_;

		PanelElementType					elementType_;

		bool								focusable_;

		bool								hasFocus_;

		bool								hiddenEventCalled_;

		std::string							hiddenHandler_;

		int									indexInParent_;

		bool								isVisible_;
		
		int									marginBottom_;

		int									marginLeft_;

		int									marginRight_;

		int									marginTop_;

		int									paddingLeft_;

		int									paddingTop_;

		int									paddingRight_;

		int									paddingBottom_;

		std::string							name_;

		// Need both because the UI Element can't actually access the panel, otherwise it runs into a circular dependency issue.
		boost::shared_ptr<UiPanelElement>	parentElement_;

		boost::shared_ptr<UiPanel>			parentPanel_;

		boost::shared_ptr<UiPanelRects>		rects_;

		bool								shownEventCalled_;

		std::string							shownHandler_;

	private:

		//virtual void	render() { assert(false); }; // Used to be abstract but had to change because of a bug. See Renderer.hpp (it uses the same workaround) for more details.

		virtual void	calculateMinimumBoundingSize() = 0;

		virtual void	calculateMinimumBoundingSizeBase() = 0;

		virtual void	created();

		virtual SizePtr	getFullSize() = 0;

		virtual void	locateChildElements() = 0;

		virtual void	locateElementsBase() = 0;

		virtual void	renderBase(int parentX, int parentY) = 0;

		virtual void	reset();

		void			setName(std::string name);

		virtual void	update(double time) = 0;

		virtual void	updateBase(double time) = 0;

		boost::shared_ptr<Position>	position_;

		FontManagerPtr				fontManager_;;

		RendererPtr					renderer_;

		boost::shared_ptr<Size>		size_;

		VisibilityChangedSignal		visibilityChangedSignal_;
	};

	typedef boost::shared_ptr<UiPanelElement> UiPanelElementPtr;
}

#endif // _UIPANELELEMENT_HPP_