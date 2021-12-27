/* -------------------------------------------------------------------------
** Ui.hpp
**
** This class is the basic UI interface, with which the user interacts with and
** controls the UI.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UI_HPP_
#define _UI_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/algorithm/string.hpp>
#include <boost/assign.hpp>
#include <boost/foreach.hpp>
#include <boost/python.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>
#include <boost/shared_ptr.hpp>

#include <iostream>
#include <vector>

#include "AudioPlayer.hpp"
#include "BaseIds.hpp"
#include "Renderer.hpp"
#include "UiPanelContainer.hpp"
#include "UiPanelFactory.hpp"
#include "UiWidgetContainer.hpp"
#include "UiWidgetFactory.hpp"
#include "UiPanelElementDefinition.hpp"

namespace firemelon
{
	class FIREMELONAPI Ui
	{
	public:
		friend class Assets;
		friend class GameEngine;

		Ui();
		virtual ~Ui();

		void					createUiPanelElementPy(boost::shared_ptr<UiPanel> parent, boost::shared_ptr<UiPanelElementDefinition> itemDef);
		void					createUiPanelElement(boost::shared_ptr<UiPanel> parent, boost::shared_ptr<UiPanelElementDefinition> itemDef);

		void					focusElement(std::string panelName, std::string elementName);
		void					focusElementPy(std::string panelName, std::string elementName);

		void					focusFirstElement(std::string panelName);
		void					focusFirstElementPy(std::string panelName);

		void					focusLastElement(std::string panelName);
		void					focusLastElementPy(std::string panelName);

		void					focusNextElement(std::string panelName);
		void					focusNextElementPy(std::string panelName);

		void					focusPreviousElement(std::string panelName);
		void					focusPreviousElementPy(std::string panelName);

		UiWidgetPtr				getFocusedWidget();
		UiWidgetPtr				getFocusedWidgetPy();

		bool					getIsShowing();

		UiPanelPtr				getPanelByName(std::string panelName);
		UiPanelPtr				getPanelByNamePy(std::string panelName);

		boost::shared_ptr<Size>	getPanelSize(std::string panelName);
		boost::shared_ptr<Size>	getPanelSizePy(std::string panelName);

		boost::python::object	getPyInstance();

		UiWidgetPtr				getWidgetByName(std::string widgetName);
		UiWidgetPtr				getWidgetByNamePy(std::string widgetName);

		void					hidePanel(std::string panelName);
		void					hidePanelPy(std::string panelName);

		void					hideWidget(std::string widgetName);
		void					hideWidgetPy(std::string widgetName);

		void					selectElement();
		void					selectElementPy();

		void					showPanel(std::string panelName);
		void					showPanelPy(std::string panelName);

		void					showWidget(std::string widgetName);
		void					showWidgetPy(std::string widgetName);

		void					writePanelTree();
		void					writePanelTreePy();

	protected:

		AudioPlayerPtr			getAudioPlayer();
		InputDeviceManagerPtr	getInputDeviceManager();
		RendererPtr				getRenderer();

	private:

		enum UiCommandType
		{
			UI_COMMAND_FOCUS = 0,
			UI_COMMAND_SELECT = 1,
			UI_COMMAND_SHOWPANEL = 2,
			UI_COMMAND_HIDEPANEL = 3
		};

		enum FocusType
		{
			FOCUS_NONE = 0,
			FOCUS_BY_NAME = 1,
			FOCUS_FIRST = 2,
			FOCUS_PREVIOUS = 3,
			FOCUS_NEXT = 4,
			FOCUS_LAST = 5
		};

		struct UiCommandArgs
		{
			std::string elementName;
		};

		struct UiFocusArgs : public UiCommandArgs
		{
			FocusType focusType;
			std::string panelName;
		};
		
		struct UiCommand
		{
			UiCommandType type;
			boost::shared_ptr<UiCommandArgs> args;
		};


		virtual void		buttonDown(GameButtonId buttonCode);

		virtual void		buttonUp(GameButtonId buttonCode);

		void				changeInputChannel(InputChannel oldInputChannel, InputChannel newInputChannel);

		UiPanelPtr			constructUiPanel(boost::shared_ptr<UiPanelElementDefinition> itemDef);

		UiPanelElementPtr	constructUiPanelElement(boost::shared_ptr<UiPanelElementDefinition> itemDef);

		UiWidgetPtr			constructUiWidget(boost::shared_ptr<UiPanelElementDefinition> itemDef);

		virtual void		cleanup();

		void				cleanupBase();
		
		void				cleanupPythonData();

		void				elementHidden(UiPanelElementPtr element);

		void				elementShown(UiPanelElementPtr element);

		void				focusElementInternal(std::string panelName, std::string elementName);

		void				focusFirstElementInternal(std::string panelName);

		void				focusLastElementInternal(std::string panelName);

		void				focusNextElementInternal(std::string panelName);

		void				focusPreviousElementInternal(std::string panelName);

		void				hidePanelInternal(std::string panelName);

		// Do anything necessary for initializing the UI.
		virtual void		initialize();

		void				initializeBase();

		// Function that gets called once initialization is complete.
		virtual void		initialized();

		void				initializePythonData();

		void				loadPanels();

		void				loadChildPanelsFromNode(boost::property_tree::ptree panelsNode, UiPanelPtr parent);

		void				panelVisibilityChanged(std::string elementName);
		
		void				preButtonDown(boost::shared_ptr<InputEvent> inputEvent);

		void				preButtonUp(boost::shared_ptr<InputEvent> inputEvent);

		void				render();

		void				showPanelInternal(std::string panelName);

		void				selectElementInternal();

		void				update(double time);

		// Widget events.
		void				widgetButtonDown(UiWidgetPtr widget, GameButtonId buttonId);

		void				widgetButtonUp(UiWidgetPtr widget, GameButtonId buttonId);

		void				widgetGotFocus(UiWidgetPtr widget);

		void				widgetLostFocus(UiWidgetPtr widget);

		void				widgetVisibilityChanged(std::string elementName);

		AudioPlayerPtr								audioPlayer_;

		boost::shared_ptr<Debugger>					debugger_;

		// This is needed because when you change the focus, it will send along the current input state
		// to the newly focused widget, which is not what is expected. For example, if you select the options menu,
		// by pressing the select button, it will then show the menu and immediately select the first item in the list
		// because it now has focus.

		// Also it can be a problem if you select an element and it causes the focus to change, because it would
		// queue up the focus arg, but immediately select the element. It needs to process the commands in the 
		// order they were received.
		std::vector<UiCommand>						queuedCommands_;

		FontManagerPtr								fontManager_;

		boost::shared_ptr<BaseIds>					ids_;

		InputChannel								inputChannel_;

		RendererPtr									renderer_;

		boost::shared_ptr<SystemMessageDispatcher>	systemMessageDispatcher_;

		boost::shared_ptr<UiPanelContainer>			uiPanelContainer_;

		boost::shared_ptr<UiPanelFactory>			uiPanelFactory_;

		boost::shared_ptr<UiWidgetContainer>		uiWidgetContainer_;

		boost::shared_ptr<UiWidgetFactory>			uiWidgetFactory_;
		
		// Engine Subsystem Components
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		boost::shared_ptr<TextManager>				textManager_;

		// Scripting data
		std::string						scriptName_;
		std::string						scriptTypeName_;

		boost::python::object			pyMainModule_;
		boost::python::object			pyMainNamespace_;
		boost::python::object			pyUiInstance_;
		boost::python::object			pyUiNamespace_;

		boost::python::object			pyButtonDown_;
		boost::python::object			pyButtonUp_;
		boost::python::object			pyInitialized_;
	};
}

#endif // _UI_HPP_