#include "..\..\Headers\EngineCore\TransitionContainer.hpp"

using namespace firemelon;

TransitionContainer::TransitionContainer()
{

}

TransitionContainer::~TransitionContainer()
{

}

void TransitionContainer::cleanup()
{
	int size = transitions_.size();

	for (int i = 0; i < size; i++)
	{
		transitions_[i]->cleanup();
	}

	transitions_.clear();
}

int	TransitionContainer::getTransitionCount()
{
	return transitions_.size();
}

boost::shared_ptr<Transition> TransitionContainer::getTransition(TransitionId transitionId)
{
	std::map<TransitionId, int>::iterator itr = transitionIdToIndexMap_.find(transitionId);
	
	if (itr == transitionIdToIndexMap_.end())
	{
		std::cout << "Transition ID " << transitionId << " does not exist. Did you mean to use TRANSITION_NULL?" << std::endl;
		return nullptr;
	}
	else
	{
		int index = transitionIdToIndexMap_[transitionId];

		return transitions_[index];
	}
}

boost::shared_ptr<Transition> TransitionContainer::getTransitionByIndex(int transitionIndex)
{
	int size = transitions_.size();

	if (transitionIndex >= 0 && transitionIndex < size)
	{
		return transitions_[transitionIndex];
	}

	return nullptr;
}

int TransitionContainer::getTransitionIndex(TransitionId transitionId)
{
	std::map<RoomId, int>::iterator itr = transitionIdToIndexMap_.find(transitionId);
	
	if (itr == transitionIdToIndexMap_.end())
	{
		return -1;
	}
	else
	{
		return transitionIdToIndexMap_[transitionId];
	}
}

void TransitionContainer::addTransition(boost::shared_ptr<Transition> transition)
{
	transition->initializePythonData();

	TransitionId id = transition->getId();

	transitionIdToIndexMap_[id] = transitions_.size();
	
	transition->initialize();

	transitions_.push_back(transition);

}

void TransitionContainer::setAssets(boost::shared_ptr<Assets> assets)
{
	assets_ = assets;
}