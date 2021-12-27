/* -------------------------------------------------------------------------
** StringMap.hpp
**
** The StringMap class is a wrapper around an std::map class that is
** defined in boost::python.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STRINGMAP_HPP_
#define _STRINGMAP_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>

#include <string>
#include <map>
#include <list>
#include "Types.hpp"

namespace firemelon
{
	class FIREMELONAPI StringMap
	{
	public:

		StringMap();
		virtual ~StringMap();
		
		int size();
		
		std::string				getValue(std::string key);
		void					setValue(std::string key, std::string value);

		std::list<std::string>	keys();

		stringmap getMap();

	private:
		
		stringmap data_;
	};
}

#endif // _STRINGMAP_HPP_