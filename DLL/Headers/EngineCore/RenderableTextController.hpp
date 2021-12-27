/* -------------------------------------------------------------------------
** RenderableTextController.hpp
**
** The RenderableTextController class is an intermediary interface between a c++
** renderable text object, and the python object instance it contains. It is
** used to call the c++ functions from inside python script.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */


#ifndef _RENDERABLETEXTCONTROLLER_HPP_
#define _RENDERABLETEXTCONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "Debugger.hpp"
#include "PythonGil.hpp"

#include <boost/signals2.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/make_shared.hpp>

namespace firemelon
{
	typedef boost::shared_ptr<boost::signals2::signal<void ()>> RenderableTextRemoveSignal;
	typedef boost::signals2::signal<void ()> RenderableTextRemoveSignalRaw;
	
	class FIREMELONAPI RenderableTextController
	{
	public:
		friend class RenderableText;

		RenderableTextController();
		virtual ~RenderableTextController();

		bool	getIsActivePy();
		bool	getIsActive();

		void	removePy();
		void	remove();

	protected:
		
	private:

		bool						isActive_;

		RenderableTextRemoveSignal	renderableTextRemoveSignal_;
	};
}

#endif // _RENDERABLETEXTCONTROLLER_HPP_