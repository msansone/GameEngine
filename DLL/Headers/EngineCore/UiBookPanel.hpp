/* -------------------------------------------------------------------------
** UiBookPanel.hpp
**
** The UiBookPanel class is a UiPanel that renders its contained
** widgets on top of one another in layers.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIBOOKPANEL_HPP_
#define _UIBOOKPANEL_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "UiPanel.hpp"

namespace firemelon
{
	class FIREMELONAPI UiBookPanel : public UiPanel
	{
	public:

		UiBookPanel(std::string panelName);
		virtual ~UiBookPanel();

	protected:

	private:

		virtual void	calculateMinimumBoundingSize();

		virtual SizePtr	getFullSize();

		//virtual void	locateChildElements();
	};
}

#endif // _UIBOOKPANEL_HPP_