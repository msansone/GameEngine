/* -------------------------------------------------------------------------
** Transition.hpp
** 
** The Transition class is the base class from which all transition
** objects are derived. If a room has a transition associated with it, it
** will be displayed if that room is shown.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TRANSITION_HPP_
#define _TRANSITION_HPP_

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
	class FIREMELONAPI Transition
	{
	public:
		friend class TransitionManager;
		friend class TransitionContainer;
		friend class Assets;

		Transition();
		virtual ~Transition();
		
		TransitionId			getId();
		
		bool					getIsComplete();

	protected:
		
	private:
	
		virtual void					begin();
		void							cleanup();
		virtual void					complete();

		// Force the transition to end, rather than letting it complete.
		void							end();

		virtual void					initialize();
		void							initializePythonData();
		void							run(double time, double totalTime, bool allowCompletion);
		virtual void					update(double time);

		double							timeElapsed_;
		double							totalTime_;
		bool							isComplete_;

		LoadingScreenId					id_;

		std::string						scriptName_;
		std::string						scriptTypeName_;
		
		DebuggerPtr						debugger_;
		RendererPtr						renderer_;
		boost::shared_ptr<TextManager>	textManager_;

		// Scripting data
		boost::python::object			pyMainModule_;
		boost::python::object			pyMainNamespace_;

		boost::python::object			pyInstance_;
		boost::python::object			pyInstanceNamespace_;
		
		boost::python::object			pyInitialize_;
		boost::python::object			pyBegin_;
		boost::python::object			pyUpdate_;
		boost::python::object			pyComplete_;
	};
}

#endif // _TRANSITION_HPP_