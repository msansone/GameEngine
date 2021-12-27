/* -------------------------------------------------------------------------
** RoomMetadata.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ROOMMETADATA_HPP_
#define _ROOMMETADATA_HPP_

#include "BaseIds.hpp"

namespace firemelon
{
	class RoomMetadata
	{
	public:
		friend class Room;
		friend class RoomContainer;

		RoomMetadata();
		virtual ~RoomMetadata();

		int			getMapHeightPy();
		int			getMapHeight();

		int			getMapWidthPy();
		int			getMapWidth();

		RoomId		getRoomIdPy();
		RoomId		getRoomId();

		std::string	getRoomNamePy();
		std::string	getRoomName();

	private:

		firemelon::RoomId	roomId_;
		std::string			roomName_;
		int					myIndex_;
		int					percentLoaded_;
		LoadingScreenId		loadingScreenId_;
		int					mapWidth_;
		int					mapHeight_;
		int					activeEntityRefCount_;

	};
}

#endif // _ROOMMETADATA_HPP_