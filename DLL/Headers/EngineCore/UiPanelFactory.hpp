/* -------------------------------------------------------------------------
** UiPanelFactory.hpp
**
** The UiPanelFactory class is used to create the UI panel objects. The user
** should derive their own subclass if they are implementing UI panels in C++.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIPANELFACTORY_HPP_
#define _UIPANELFACTORY_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "BaseIds.hpp"
#include "UiBookPanel.hpp"
#include "UiHorizontalStackPanel.hpp"
#include "UiVerticalStackPanel.hpp"

namespace firemelon
{
	class FIREMELONAPI UiPanelFactory
	{
	public:

		UiPanelFactory();
		virtual ~UiPanelFactory();

		virtual UiPanelPtr createUiPanel(std::string layoutName, std::string panelName);

		UiPanelPtr createUiPanelBase(std::string layoutName, std::string panelName);

	private:
	};
}

#endif // _UIPANELFACTORY_HPP_