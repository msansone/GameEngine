/* -------------------------------------------------------------------------
** ParticleEmitterTypeMap.hpp
**
** The ParticleEmitterTypeMap class is a wrapper around an std::map class that is
** defined in boost::python.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

/* -------------------------------------------------------------------------
** EntityTypeMap.hpp
**
** The EntityTypeMap class is a wrapper around an std::map class that is
** defined in boost::python.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYTYPEMAP_HPP_
#define _ENTITYTYPEMAP_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>

#include <string>
#include <map>
#include "BaseIds.hpp"
#include "Entity.hpp"
#include "EntityList.hpp"

namespace firemelon
{
	class FIREMELONAPI EntityTypeMap
	{
	public:

		EntityTypeMap();
		virtual ~EntityTypeMap();

		int sizePy();
		int size();

		EntityList getValuePy(EntityTypeId key);
		EntityList getValue(EntityTypeId key);

		std::map<EntityTypeId, EntityList> map_;

	private:

	};
}

#endif // _ENTITYTYPEMAP_HPP_