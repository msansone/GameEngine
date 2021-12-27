/* -------------------------------------------------------------------------
** UiVerticalStackPanel.hpp
**
** The UiVerticalStackPanel class is a UiPanel that renders its contained
** widgets in a top to bottom stack.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIVERTICALSTACKPANEL_HPP_
#define _UIVERTICALSTACKPANEL_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "UiPanel.hpp"

namespace firemelon
{
	class FIREMELONAPI UiVerticalStackPanel : public UiPanel
	{
	public:

		UiVerticalStackPanel(std::string panelName);
		virtual ~UiVerticalStackPanel();

	protected:

	private:

		virtual void	calculateMinimumBoundingSize();

		virtual SizePtr	getFullSize();

		virtual void	locateChildElements();
	};
}

#endif // _UIVERTICALSTACKPANEL_HPP_