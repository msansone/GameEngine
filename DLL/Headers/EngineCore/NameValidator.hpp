/* -------------------------------------------------------------------------
** NameValidator.hpp
**
** The NameValidator class stores all of the entity instance names. It 
** is used to verify that when an entity instance is created, the name is not
** already in use by any other entities. It is necessary because entities
** can exist in other rooms, so there needs to be a collection of names that
** is shared between all rooms.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _NAMEVALIDATOR_HPP_
#define _NAMEVALIDATOR_HPP_

#include <iostream>
#include <string>
#include <vector>

namespace firemelon
{
	class NameValidator
	{
	public:

		NameValidator();
		virtual ~NameValidator();
		
		void	addName(std::string name);
		void	removeName(std::string name);

		bool	isNameInUse(std::string name);

	private:
		
		int	findName(std::string name);

		std::vector<std::string>	names_;

	};
}

#endif // _NAMEVALIDATOR_HPP_