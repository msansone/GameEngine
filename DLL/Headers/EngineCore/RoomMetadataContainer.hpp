/* -------------------------------------------------------------------------
** RoomMetadataContainer.hpp
**
** The RoomMetadataContainer class stores the room metadata objects. It can 
** be used to access the metadata for a room from an entity.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ROOMMETADATACONTAINER_HPP_
#define _ROOMMETADATACONTAINER_HPP_

#include <map>

#include "RoomMetadata.hpp"

namespace firemelon
{
	class RoomMetadataContainer
	{
	public:
		friend class RoomContainer;

		RoomMetadataContainer();
		virtual ~RoomMetadataContainer();

		boost::shared_ptr<RoomMetadata>	getRoomMetadataPy(RoomId roomId);
		boost::shared_ptr<RoomMetadata>	getRoomMetadata(RoomId roomId);

	private:

		void	addRoomMetadata(boost::shared_ptr<RoomMetadata> roomMetadata);

		std::map<firemelon::RoomId, boost::shared_ptr<RoomMetadata>>	roomIdToMetadataMap_;
	};
}

#endif // _ROOMMETADATACONTAINER_HPP_