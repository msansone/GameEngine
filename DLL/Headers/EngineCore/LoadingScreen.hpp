/* -------------------------------------------------------------------------
** LoadingScreen.hpp
** 
** The LoadingScreen class is the base class from which all loading screen
** objects are derived. If a room has a loading screen associated with it, it
** will be displayed if that room is shown and not finished loading yet.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _LOADINGSCREEN_HPP_
#define _LOADINGSCREEN_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>

#include "BaseIds.hpp"
#include "Renderer.hpp"
#include "TextManager.hpp"
#include "Debugger.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI LoadingScreen
	{
	public:
		friend class LoadingScreenContainer;
		friend class Assets;

		LoadingScreen();
		virtual ~LoadingScreen();
		
		LoadingScreenId	getId();
		
		virtual void	initialize();
		virtual void	percentChanged(int percentDone);

	protected:
		
	private:
		
		void	cleanup();
		void	initializePythonData();
		
		LoadingScreenId			id_;

		std::string				scriptName_;
		std::string				scriptTypeName_;
		std::string				scriptVar_;

		DebuggerPtr						debugger_;
		boost::shared_ptr<FontManager>	fontManager_;
		boost::shared_ptr<Renderer>		renderer_;
		boost::shared_ptr<TextManager>	textManager_;

		// Scripting data
		boost::python::object	pyMainModule_;
		boost::python::object	pyMainNamespace_;

		boost::python::object	pyInstance_;
		boost::python::object	pyInstanceNamespace_;
		
		boost::python::object	pyPercentChanged_;
		boost::python::object	pyInitialize_;
	};
}

#endif // _LOADINGSCREEN_HPP_