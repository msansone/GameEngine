#include "..\..\Headers\EngineCore\ServerState.hpp"

using namespace firemelon;

ServerState::ServerState(boost::shared_ptr<BaseIds> ids)
{
	isRoomLoading = false;
	isRoomActivated = false;
	
	currentRoomId = ids->ROOM_NULL;
	previousRoomId = ids->ROOM_NULL;
		
	currentTransitionId = ids->TRANSITION_NULL;
	previousTransitionId = ids->TRANSITION_NULL;

	transitionTime = 0.0;

	percentRoomLoaded = 0;
}

ServerState::~ServerState()
{
}
