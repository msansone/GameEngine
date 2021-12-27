/* -------------------------------------------------------------------------
** IdGenerator.hpp
** 
** The IdGenerator class is used to generate a unique integer ID, which is
** assigned to enties, particles, or anything else that requires one.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _IDGENERATOR_HPP_
#define _IDGENERATOR_HPP_


namespace firemelon
{
	typedef int UniqueId;

	class IdGenerator
	{
	public:

		IdGenerator();
		virtual ~IdGenerator();

		static UniqueId getNextId();

	private:
		
		static UniqueId idIncrementer_;
	};
}

#endif // _IDGENERATOR_HPP_