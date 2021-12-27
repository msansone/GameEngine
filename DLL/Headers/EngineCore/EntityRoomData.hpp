/* -------------------------------------------------------------------------
** EntityRoomData.hpp
** 
** The EntityRoomData class stored info such as which room the player is in,
** and which layer it is on.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYROOMDATA_HPP_
#define _ENTITYROOMDATA_HPP_

#include "BaseIds.hpp"

namespace firemelon
{
	class EntityRoomData
	{
	public:

		EntityRoomData();
		virtual ~EntityRoomData();

		int		roomIndex;
		RoomId	roomId;

		int		layerIndex;
	};
}

#endif //_ENTITYROOMDATA_HPP_