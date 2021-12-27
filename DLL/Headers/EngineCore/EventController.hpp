/* -------------------------------------------------------------------------
** EventController.hpp
**
** The EventController class is is derived from the Base EntityController class.
** It contains all of the functions neccesary for controlling an event in the
** game.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _EVENTCONTROLLER_HPP_
#define _EVENTCONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "EntityController.hpp"

namespace firemelon
{
	class FIREMELONAPI EventController : public EntityController
	{
	public:

		EventController();
		virtual ~EventController();

	private:

	};
}

#endif // _EVENTCONTROLLER_HPP_