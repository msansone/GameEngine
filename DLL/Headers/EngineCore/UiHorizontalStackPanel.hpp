/* -------------------------------------------------------------------------
** UiHorizontalStackPanel.hpp
**
** The UiHorizontalStackPanel class is a UiPanel that renders its contained
** widgets in a left to right stack.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIHORIZONTALSTACKPANEL_HPP_
#define _UIHORIZONTALSTACKPANEL_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "UiPanel.hpp"

namespace firemelon
{
	class FIREMELONAPI UiHorizontalStackPanel : public UiPanel
	{
	public:

		UiHorizontalStackPanel(std::string panelName);
		virtual ~UiHorizontalStackPanel();

	protected:

	private:

		virtual void	calculateMinimumBoundingSize();

		virtual SizePtr	getFullSize();

		virtual void	locateChildElements();
	};
}

#endif // _UIHORIZONTALSTACKPANEL_HPP_