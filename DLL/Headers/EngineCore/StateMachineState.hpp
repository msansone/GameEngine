/* -------------------------------------------------------------------------
** StateMachineState.hpp
**
** The StateMachineState class represents a single state that a state machine
** can be in, such as running, jumping, etc. It contains references to all of
** the animations that should be displayed (as well as their position, relative
** to the entity), and also references to all of the hitboxes that are active 
** while the sprite is in this state.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STATEMACHINESTATE_HPP_
#define _STATEMACHINESTATE_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <vector>
#include <map>

#include <boost/signals2.hpp>
#include <boost/shared_ptr.hpp>

#include "Position.hpp"
#include "AnimationPlayer.hpp"
#include "EntityMetadata.hpp"
#include "Renderer.hpp"
#include "StageMetadata.hpp"

namespace firemelon
{
	class FIREMELONAPI StateMachineState
	{
	public:
		friend class StateMachineController;

		StateMachineState(std::string name);
		virtual ~StateMachineState();

		// Get the index that this state is at in the array that holds it.
		int					getIndex();

		std::string			getNamePy();
		std::string			getName();
		
	private:
		
		void	setOwnerMetadata(boost::shared_ptr<EntityMetadata> ownerMetadata);
		
		int														myIndex_;
		
		RendererPtr												renderer_;

		// The name of this state.
		std::string												stateName_;		
	};

	typedef boost::shared_ptr<StateMachineState> StateMachineStatePtr;
	typedef std::vector<StateMachineStatePtr> StateMachineStatePtrList;
}

#endif // _STATEMACHINESTATE_HPP_