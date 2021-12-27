///* -------------------------------------------------------------------------
//** UiPanelContainer.hpp
//** 
//** The UiPanelContainer class acts is a container which stores the UI panels.
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */

#ifndef _UIPANELCONTAINER_HPP_
#define _UIPANELCONTAINER_HPP_

#include <boost/shared_ptr.hpp>

#include <vector>

#include "UiPanel.hpp"

namespace firemelon
{
	typedef std::vector<UiPanelPtr> UiPanelList;

	class UiPanelContainer
	{
	public:
//		friend class UiManager;
//		friend class GameEngine;
//
//		UiPanelContainer(boost::shared_ptr<FontManager> fontManager);
		UiPanelContainer(DebuggerPtr debugger);
		virtual ~UiPanelContainer();

		void						add(UiPanelPtr panel);

		void						cleanup();

		size_t						size();

		UiPanelPtr	getPanelByName(std::string panelName);

		UiPanelPtr	getPanelByIndex(size_t index);

		UiPanelPtr	getRoot();

		boost::shared_ptr<Size>		getSize();

	private:

		UiPanelList					panels_;

		UiPanelPtr					rootPanel_;
	};
}
#endif // _UIPANELCONTAINER_HPP_