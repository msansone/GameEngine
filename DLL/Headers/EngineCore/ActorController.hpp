/* -------------------------------------------------------------------------
** ActorController.hpp
**
** The ActorController class is is derived from the Base EntityController class.
** It contains all of the functions neccesary for controlling an actor in the
** game.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ACTORCONTROLLER_HPP_
#define _ACTORCONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "EntityController.hpp"
#include "StageRenderable.hpp"

namespace firemelon
{
	class FIREMELONAPI ActorController : public EntityController
	{
	public:

		ActorController();
		virtual ~ActorController();

	private:

		virtual void	createRenderables();

	};
}

#endif // _ACTORCONTROLLER_HPP_