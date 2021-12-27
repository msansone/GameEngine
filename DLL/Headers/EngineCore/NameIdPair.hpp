/* -------------------------------------------------------------------------
** NameIdPair.hpp
**
** The NameIdPair class is used to associate an ID value with a name which can
** be referenced by a python script.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */
#include <string>

#ifndef _NAMEIDPAIR_HPP_
#define _NAMEIDPAIR_HPP_

namespace firemelon
{
	class NameIdPair
	{
	public:
		NameIdPair(int id, std::string name);
		virtual ~NameIdPair();
		
		int			getId();
		std::string	getName();

	private:
		
		int			id_;
		std::string	name_;
	};
}

#endif // _NAMEIDPAIR_HPP_