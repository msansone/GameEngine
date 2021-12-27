/* -------------------------------------------------------------------------
** EntityList.hpp
**
** The EntityList class is a wrapper around an std::vector class that is
** defined in boost::python.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYLIST_HPP_
#define _ENTITYLIST_HPP_

#include <boost/python.hpp>

#include <string>
#include <map>
#include "Entity.hpp"

namespace firemelon
{
	class EntityList
	{
	public:

		EntityList();
		virtual ~EntityList();
		
		int sizePy();
		int size();
		
		boost::python::object getValuePy(int index);
		boost::python::object getValue(int index);
		
		std::vector<boost::shared_ptr<Entity>>	entityList_;

	private:

	};
}

#endif // _ENTITYLIST_HPP_