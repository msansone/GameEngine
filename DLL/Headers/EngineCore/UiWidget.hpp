/* -------------------------------------------------------------------------
** UiWidget.hpp
**
** The UiWidget is an item that is contained by one or more panels, and is
** the element which the user interacts with visually.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _UIWIDGET_HPP_
#define _UIWIDGET_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>

#include <string>
#include <vector>

#include "Debugger.hpp"
#include "InputDeviceManager.hpp"
#include "SystemMessageDispatcher.hpp"
#include "Types.hpp"
#include "UiPanel.hpp"
#include "UiPanelElement.hpp"

namespace firemelon
{
	class FIREMELONAPI UiWidget : public UiPanelElement
	{
	public:
		friend class Ui;
		friend class UiWidgetContainer;
		friend class UiWidgetFactory;

		UiWidget(std::string widgetName);
		virtual ~UiWidget();

		virtual bool			getIsFocusable();

		boost::python::object	getPyInstance();

	protected:

		std::string				getScriptVar();
		std::string				getScriptName();
		std::string				getScriptTypeName();

		InputDeviceManagerPtr	getInputDeviceManager();

	private:

		virtual void	buttonDown(GameButtonId buttonCode);

		virtual void	buttonUp(GameButtonId buttonCode);

		virtual void	calculateMinimumBoundingSize();

		virtual void	calculateMinimumBoundingSizeBase();

		virtual void	calculateSize();

		void			cleanupPythonData();

		virtual void	created();

		virtual SizePtr	getFullSize();

		virtual void	initialize();

		void			initializePythonData();

		virtual void	locateChildElements();

		virtual void	locateElementsBase();

		virtual void	pythonDataInitialized();

		void			renderBase(int parentX, int parentY);

		virtual void	readParameters(std::string parameters);

		virtual void	render();

		// It's important to remember that, all rectangle positions in a panel are relative
		// to the parent panel. When it comes time to render, the parent will pass down its
		// position to use as an offset.
		void			renderBackground(int parentX, int parentY);

		virtual void	reset();

		void			setTypeName(std::string typeName);

		void			setTypeId(UiWidgetId typeId);

		virtual void	update(double time);

		void			updateBase(double time);

		// Private variables.

		std::string	buttonDownHandler_;

		std::string	buttonUpHandler_;

		std::string	gotFocusHandler_;

		bool		isInitialized_;

		bool		isEnabled_;

		static int	idCounter_;

		int			id_;

		std::string	lostFocusHandler_;

		std::string	selectElementHandler_;

		std::string	typeName_;

		UiWidgetId	typeId_;

		// Core components
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		boost::shared_ptr<SystemMessageDispatcher>	systemMessageDispatcher_;
		

		// Scripting data
		std::string						scriptName_;
		std::string						scriptVar_;
		std::string						scriptTypeName_;

		boost::python::object			pyMainModule_;
		boost::python::object			pyMainNamespace_;
		boost::python::object			pyUiWidgetInstance_;
		boost::python::object			pyUiWidgetNamespace_;

		boost::python::object			pyButtonUp_;
		boost::python::object			pyButtonDown_;
		boost::python::object			pyCalculateSize_;
		boost::python::object			pyEnter_;
		boost::python::object			pyInitialize_;
		boost::python::object			pyLeave_;
		boost::python::object			pyReadParameters_;
		boost::python::object			pyRender_;
		boost::python::object			pyUpdate_;
	};

	typedef boost::shared_ptr<UiWidget> UiWidgetPtr;
}

#endif // _UIWIDGET_HPP_