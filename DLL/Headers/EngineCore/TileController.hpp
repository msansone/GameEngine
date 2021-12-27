/* -------------------------------------------------------------------------
** TileController.hpp
**
** The TileController class is an entity controller subclass defined by the 
** engine. The tile entities make up the basic world geometry and scenery in
** each room. It is used to respond to tile collisions more quickly than if
** it was done in python.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TILECONTROLLER_HPP_
#define _TILECONTROLLER_HPP_

#include "EntityController.hpp"
#include "TileRenderable.hpp"

namespace firemelon
{
	class TileController : public EntityController
	{
	public:
		TileController();
		virtual ~TileController();
		
		virtual void	initialize();
		virtual void	createRenderables();

	private:
		
	};
}

#endif // _TILECONTROLLER_HPP_