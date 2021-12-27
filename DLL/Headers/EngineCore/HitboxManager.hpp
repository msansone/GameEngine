/* -------------------------------------------------------------------------
** HitboxManager.hpp
** 
** The HitboxManager class provides a single location to store and access
** all hitboxes.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _HITBOXMANAGER_HPP_
#define _HITBOXMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <vector>
#include <map>

#include "Hitbox.hpp"

namespace firemelon
{
	class FIREMELONAPI HitboxManager
	{
	public:	

		HitboxManager();
		virtual ~HitboxManager();
	
		int							addHitbox(boost::shared_ptr<Hitbox> hitbox);
		
		boost::shared_ptr<Hitbox>	getHitboxPy(int index);
		boost::shared_ptr<Hitbox>	getHitbox(int index);

	private:
		
		std::vector<boost::shared_ptr<Hitbox>>	hitboxes_;
	};

	typedef boost::shared_ptr<HitboxManager> HitboxManagerPtr;
}

#endif // _HITBOXMANAGER_HPP_