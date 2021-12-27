/* -------------------------------------------------------------------------
** ActorCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ACTORCODEBEHIND_HPP_
#define _ACTORCODEBEHIND_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "CodeBehind.hpp"

namespace firemelon
{
	class FIREMELONAPI ActorCodeBehind : CodeBehind
	{
	public:
		ActorCodeBehind();
		virtual ~ActorCodeBehind();

	private:

	};
}

#endif // _ACTORCODEBEHIND_HPP_
