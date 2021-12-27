/* -------------------------------------------------------------------------
** ServerState.hpp
**
** The ServerState class exists on the client, and stores variables which
** it receives from the server to track its state, such as isLoading, and
** currentRoomId.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SERVERSTATE_HPP_
#define _SERVERSTATE_HPP_

#include "ReplicationStateContainer.hpp"
#include "BaseIds.hpp"

namespace firemelon
{
	class ServerState
	{
	public:

		ServerState(boost::shared_ptr<BaseIds> ids);
		virtual ~ServerState();

		bool						isRoomLoading;
		bool						isRoomActivated;
		
		RoomId						currentRoomId;
		RoomId						previousRoomId;
		
		TransitionId				previousTransitionId;
		TransitionId				currentTransitionId;

		double						transitionTime;

		int							percentRoomLoaded;

	private:

	};
}

#endif // _SERVERSTATE_HPP_