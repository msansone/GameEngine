/* -------------------------------------------------------------------------
** NetworkedEntityMetadata.hpp
** 
** The NetworkedEntityMetadata class contains the meta data associated with 
** a networked entity, such as the IP address associated with it.
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _NETWORKEDENTITYMETADATA_HPP_
#define _NETWORKEDENTITYMETADATA_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>

namespace firemelon
{
	class FIREMELONAPI NetworkedEntityMetadata
	{
	public:

		NetworkedEntityMetadata();
		virtual ~NetworkedEntityMetadata();
	
		std::string	ipAddress_;

	private:

	};
}

#endif // _NETWORKEDENTITYMETADATA_HPP_