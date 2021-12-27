#include "..\..\Headers\EngineCore\StateMachineControllerHolder.hpp"

using namespace firemelon;

StateMachineControllerHolder::StateMachineControllerHolder()
{
	stateMachineController_ = nullptr;
}

StateMachineControllerHolder::~StateMachineControllerHolder()
{
}

StateMachineControllerPtr StateMachineControllerHolder::getStateMachineController()
{
	return stateMachineController_;
}

void StateMachineControllerHolder::setStateMachineController(StateMachineControllerPtr stateMachineController)
{
	if (stateMachineController_ == nullptr)
	{
		stateMachineController_ = stateMachineController;
	}
}

bool StateMachineControllerHolder::getHasStateMachineController()
{
	return stateMachineController_ != nullptr;
}