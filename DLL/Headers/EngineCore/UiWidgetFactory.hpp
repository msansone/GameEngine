/* -------------------------------------------------------------------------
** UiWidgetFactory.hpp
**
** The UiWidgetFactory class is used to create the UI widget objects. The user
** should derive their own subclass if they are implementing UI widgets in C++.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIWIDGETFACTORY_HPP_
#define _UIWIDGETFACTORY_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "BaseIds.hpp"
#include "UiWidget.hpp"

namespace firemelon
{
	class FIREMELONAPI UiWidgetFactory
	{
	public:
		UiWidgetFactory();
		virtual ~UiWidgetFactory();
		
		virtual UiWidgetPtr createUiWidget(UiWidgetId uiWidgetTypeId, std::string widgetName);

		UiWidgetPtr createUiWidgetBase(UiWidgetId uiWidgetTypeId, std::string widgetName);

	private:
	};
}

#endif // _UIWIDGETFACTORY_HPP_