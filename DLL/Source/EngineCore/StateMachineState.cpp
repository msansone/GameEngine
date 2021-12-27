#include "..\..\Headers\EngineCore\StateMachineState.hpp"

using namespace firemelon;

StateMachineState::StateMachineState(std::string name)
{
	stateName_ = name;

	myIndex_ = 0;
}

StateMachineState::~StateMachineState()
{
}

int	StateMachineState::getIndex()
{
	return myIndex_;
}

std::string StateMachineState::getNamePy()
{
	PythonReleaseGil unlocker;

	return getName();
}

std::string StateMachineState::getName()
{
	return stateName_;
}