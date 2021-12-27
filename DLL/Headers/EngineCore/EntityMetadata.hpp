/* -------------------------------------------------------------------------
** EntityMetadata.hpp
** 
** The EntityMetadata class contains the meta data associated with a single 
** entity, which includes the entity type ID, the entity classification, and
** a string tag value.
**
** It is used so I can generate random items, using the value stored in
** the tag as a weight to make some items more rare. Also, certain 
** classifications are more rare as well (i.e. consumable items will be
** dropped much more often than weapons or armor.)
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYMETADATA_HPP_
#define _ENTITYMETADATA_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "BaseIds.hpp"
#include "IdGenerator.hpp"
#include "RoomMetadata.hpp"
#include "Types.hpp"

#include <vector>

namespace firemelon
{
	class FIREMELONAPI EntityMetadata
	{
	public:
		friend class ParticleRenderable;
		friend class Room;
		friend class RoomContainer;

		EntityMetadata();
		virtual ~EntityMetadata();
		
		UniqueId						entityInstanceId_;
		EntityClassification			classification_;
		
		std::string						name_;
		
		EntityClassificationId			getClassificationIdPy();
		EntityClassificationId			getClassificationId();
		void							setClassificationId(EntityClassificationId value);

		int								getEntityInstanceIdPy();
		int								getEntityInstanceId();
		void							setEntityInstanceId(int value);

		std::string						getEntityInstanceNamePy();
		std::string						getEntityInstanceName();

		EntityTypeId					getEntityTypeIdPy();
		EntityTypeId					getEntityTypeId();
		void							setEntityTypeId(EntityTypeId value);

		int								getMapLayerPy();
		int								getMapLayer();

		boost::shared_ptr<RoomMetadata>	getPreviousRoomMetadataPy();
		boost::shared_ptr<RoomMetadata>	getPreviousRoomMetadata();
		
		boost::shared_ptr<RoomMetadata>	getRoomMetadataPy();
		boost::shared_ptr<RoomMetadata>	getRoomMetadata();

		int								getHeightPy();
		int								getHeight();

		int								getWidthPy();
		int								getWidth();

		std::string						getTagPy();
		std::string						getTag();
		void							setTag(std::string value);

	private:
		
		EntityClassificationId			classificationId_;
		EntityTypeId					entityTypeId_;
		int								layerIndex_;
		boost::shared_ptr<RoomMetadata>	previousRoomMetadata_;
		boost::shared_ptr<RoomMetadata>	roomMetadata_;
		int								height_;
		int								width_;
		std::string						tag_;
	};

	typedef boost::shared_ptr<EntityMetadata> EntityMetadataPtr;
}

#endif // _ENTITYMETADATA_HPP_