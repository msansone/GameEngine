/* -------------------------------------------------------------------------
** SpawnPoint.hpp
** 
** The SpawnPoint class is a named point in a room where entities can spawn
** at upon entry.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SPAWNPOINT_HPP_
#define _SPAWNPOINT_HPP_

#include "BaseIds.hpp"

namespace firemelon
{
	class SpawnPoint
	{
	public:

		SpawnPoint();
		virtual ~SpawnPoint();
		
		void			setId(SpawnPointId id);
		SpawnPointId	getId();

		void	setX(int x);
		int		getX();
		
		void	setY(int y);
		int		getY();

		void	setLayer(int layer);
		int		getLayer();

	protected:
		
	private:
	
		SpawnPointId id_;

		int x_;
		int y_;
		int layer_;
	};
}

#endif // _SPAWNPOINT_HPP_