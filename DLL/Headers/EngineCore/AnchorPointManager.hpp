/* -------------------------------------------------------------------------
** AnchorPointManager.hpp
** 
** The AnchorPointManager class provides a single location to store and access
** all anchor points.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ANCHORPOINTMANAGER_HPP_
#define _ANCHORPOINTMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <vector>
#include <map>

#include "AnchorPoint.hpp"

namespace firemelon
{
	class FIREMELONAPI AnchorPointManager
	{
	public:

		AnchorPointManager();
		virtual ~AnchorPointManager();
	
		int				addAnchorPoint(AnchorPointPtr anchorPoint);
		AnchorPointPtr	getAnchorPoint(int index);

	private:
		
		std::vector<AnchorPointPtr>	anchorPoints_;
	};

	typedef boost::shared_ptr<AnchorPointManager> AnchorPointManagerPtr;
}

#endif // _ANCHORPOINTMANAGER_HPP_