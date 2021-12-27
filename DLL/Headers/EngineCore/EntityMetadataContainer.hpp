/* -------------------------------------------------------------------------
** EntityMetadataContainer.hpp
** 
** The EntityMetadataContainer class stores metadata for the currently loaded 
** entities. I added this class to assist with spawning entities for items drops. 
** I needed a way to know what classification an entity is, and give it some 
** additional arbitrary data. In this case, the additional data is a percentile
** value, which describes its rarity.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYMETADATACONTAINER_HPP_
#define _ENTITYMETADATACONTAINER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "EntityMetadata.hpp"

#include <vector>

namespace firemelon
{
	class FIREMELONAPI EntityMetadataContainer
	{

	public:

		EntityMetadataContainer();
		virtual ~EntityMetadataContainer();
		
		int				getEntityMetadataCountPy();
		int				getEntityMetadataCount();
		
		EntityMetadata	getEntityMetadataPy(int index);
		EntityMetadata	getEntityMetadata(int index);

		void			addEntityMetadata(EntityTypeId entityTypeId, EntityClassificationId classificationId, std::string tag);

	private:

		std::vector<EntityMetadata> entityMetadataList_;
	};
}

#endif // _ENTITYMETADATACONTAINER_HPP_