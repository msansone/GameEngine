/* -------------------------------------------------------------------------
** EntityInvalidator.hpp
**
** EntityInvalidator.hpp is an entity component that is used to flag an entity
** as invalid (due to removal, , so that a subsystem, such as collision detection, knows to 
** ignore it.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYINVALIDATOR_HPP_
#define _ENTITYINVALIDATOR_HPP_

namespace firemelon
{
	class EntityInvalidator
	{
	public:
		friend class PhysicsManager;

		EntityInvalidator();
		virtual ~EntityInvalidator();
		
		void	setIsInvalidated(bool isInvalidated);
		bool	getIsInvalidated();

	private:
		
		bool	isInvalidated_;
	};
}

#endif // _ENTITYINVALIDATOR_HPP_