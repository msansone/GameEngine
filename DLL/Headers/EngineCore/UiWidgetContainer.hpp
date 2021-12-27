///* -------------------------------------------------------------------------
//** UiWidgetContainer.hpp
//** 
//** The UiWidgetContainer class acts is a container which stores the UI widgets.
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */

#ifndef _UIWIDGETCONTAINER_HPP_
#define _UIWIDGETCONTAINER_HPP_

#include <boost/shared_ptr.hpp>

#include <vector>

#include "UiWidget.hpp"

namespace firemelon
{
	typedef std::vector<UiWidgetPtr> UiWidgetList;

	class UiWidgetContainer
	{
	public:

		UiWidgetContainer();
		virtual ~UiWidgetContainer();

		void						add(UiWidgetPtr widget);

		void						cleanup();

		size_t						size();

		UiWidgetPtr	getWidgetByName(std::string widgetName);

		UiWidgetPtr	getWidgetByIndex(size_t index);

	private:

		UiWidgetList	widgets_;
	};
}
#endif // _UIWIDGETCONTAINER_HPP_