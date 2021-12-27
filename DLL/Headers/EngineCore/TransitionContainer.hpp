/* -------------------------------------------------------------------------
** TransitionContainer.hpp
** 
** The TransitionContainer class stores the transitions.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TRANSITIONCONTAINER_HPP_
#define _TRANSITIONCONTAINER_HPP_

#include <vector>
#include <map>

#include "Transition.hpp"
#include "Assets.hpp"

namespace firemelon
{
	class TransitionContainer
	{
	public:

		TransitionContainer();
		virtual ~TransitionContainer();

		void							cleanup();
		int								getTransitionCount();
		boost::shared_ptr<Transition>	getTransition(TransitionId transitionId);
		boost::shared_ptr<Transition>	getTransitionByIndex(int transitionIndex);
		int								getTransitionIndex(TransitionId transitionId);

		void							addTransition(boost::shared_ptr<Transition> transition);

		void							setAssets(boost::shared_ptr<Assets> assets);

	private:

		std::vector<boost::shared_ptr<Transition>>	transitions_;

		std::map<TransitionId, int>					transitionIdToIndexMap_;

		boost::shared_ptr<Assets>					assets_;

	};
}

#endif // _TRANSITIONCONTAINER_HPP_