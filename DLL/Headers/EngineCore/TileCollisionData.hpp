/* -------------------------------------------------------------------------
** TileCollisionData.hpp
** 
** The TileCollisionData class is a subclass of the CollisionData
** class. It contains additional information relevant to a tile collision,
** such as the tile group ID.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TILECOLLISIONDATA_HPP_
#define _TILECOLLISIONDATA_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "CollisionData.hpp"

namespace firemelon
{
	class FIREMELONAPI TileCollisionData : public CollisionData
	{
	public:
		friend class Entity;
		friend class GameStateManager;

		TileCollisionData();
		virtual ~TileCollisionData();

		void			setTileGroupIdPy(unsigned int tileGroupId);
		void			setTileGroupId(unsigned int tileGroupId);

		unsigned int	getTileGroupIdPy();
		unsigned int	getTileGroupId();

	private:

		unsigned int	tileGroupId_;
	};
}

#endif // _TILECOLLISIONDATA_HPP_