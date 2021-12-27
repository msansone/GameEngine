/* -------------------------------------------------------------------------
** TransitionManager.hpp
** 
** The TransitionManager class is responsible for keeping tracking of which
** transition is active, if any, and updating it.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TRANSITIONMANAGER_HPP_
#define _TRANSITIONMANAGER_HPP_

#include "EngineController.hpp"
#include "TransitionContainer.hpp"

namespace firemelon
{
	typedef boost::signals2::signal<void (RoomId roomToShow)> TransitionCompleteSignalRaw;
	typedef boost::shared_ptr<TransitionCompleteSignalRaw> TransitionCompleteSignal;

	class TransitionManager
	{
	public:
		friend class GameEngine;
		friend class Factory;

		TransitionManager();
		virtual ~TransitionManager();

		// Update the syncronous transition.
		void	update(double time);

		// Update the asynchronous transition.
		void	updateAsync(double time);
		
		// Returns whether a synchronous transition is currently happening.
		bool	getIsTransitioningPy();
		bool	getIsTransitioning();

		// Returns whether an asynchronous transition is currently happening.
		bool	getIsTransitioningAsyncPy();
		bool	getIsTransitioningAsync();

		// Activate a synchronous transition. These pause simulation updates, and are used when transitioning between rooms,
		// so the room to show must be passed as a parameter.
		void	activateRoomChangeTransition(TransitionId transitionId, double transitionTime, RoomId roomIdToShow);

		// Activate an asynchronous transition. These do not pause the simulation and can be activated at any time.
		// They could be used for any reason seen fit, such as in cutscenes, or showing a menu.
		void	activateTransitionAsyncPy(TransitionId transitionId, double transitionTime);
		void	activateTransitionAsync(TransitionId transitionId, double transitionTime);

		void	activateTransitionPy(TransitionId transitionId, double transitionTime);
		void	activateTransition(TransitionId transitionId, double transitionTime);

		// add a way for users to activate syncronous transitions, without showing/activating a room.

		// End an asynchronous transition.
		void	endTransitionAsyncPy();
		void	endTransitionAsync();

	private:

		boost::shared_ptr<BaseIds> ids_;

		boost::shared_ptr<TransitionContainer>		transitionContainer_;

		boost::shared_ptr<Transition>				activeTransition_;
		boost::shared_ptr<Transition>				activeTransitionAsync_;

		double										transitionTime_;
		double										transitionTimeAsync_;

		RoomId										roomIdToShow_;
		
		TransitionCompleteSignal					transitionCompleteSignal_;

		boost::shared_ptr<EngineController>			engineController_;
	};
}

#endif // _TRANSITIONMANAGER_HPP_