#include "..\..\Headers\EngineCore\StateContainer.hpp"

using namespace firemelon;

StateContainer::StateContainer()
{
	currentStateIndex_ = -1;
	previousStateIndex_ = -1;
}

StateContainer::~StateContainer()
{
}

void StateContainer::addState(StateMachineStatePtr stateMachineState)
{
	std::string name = stateMachineState->getName();

	states_.push_back(stateMachineState);

	std::map<std::string, int>::iterator itr = stateNameIdMap_.find(name);

	bool nameExists = !(itr == stateNameIdMap_.end());
	
	if (nameExists == false)
	{
		stateNameIdMap_[name] = states_.size();

		stateIdNameMap_[states_.size() - 1] = name;
	}
}

int StateContainer::getCurrentStateIndex()
{
	return currentStateIndex_;
}

StateMachineStatePtr StateContainer::getStateByIndex(int index)
{
	int size = states_.size();

	if (index >= 0 && index < size)
	{
		return states_[index];
	}
	else
	{
		return nullptr;
	}
}

int	StateContainer::getStateCount()
{
	return states_.size();
}