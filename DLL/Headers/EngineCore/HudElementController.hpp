/* -------------------------------------------------------------------------
** HudElementController.hpp
**
** The HudElementController class is is derived from the Base EntityController class.
** It contains all of the functions neccesary for controlling a HUD element in the
** game.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _HUDELEMENTCONTROLLER_HPP_
#define _HUDELEMENTCONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "EntityController.hpp"
#include "StageRenderable.hpp"

namespace firemelon
{
	class FIREMELONAPI HudElementController : public EntityController
	{
	public:

		HudElementController();
		virtual ~HudElementController();

	private:

		virtual void	createRenderables();
	};
}

#endif // _HUDELEMENTCONTROLLER_HPP_