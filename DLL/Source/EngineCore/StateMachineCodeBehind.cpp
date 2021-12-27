#include "..\..\Headers\EngineCore\StateMachineCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

StateMachineCodeBehind::StateMachineCodeBehind()
{
}

StateMachineCodeBehind::~StateMachineCodeBehind()
{
}

void StateMachineCodeBehind::stateChanged(int oldStateIndex, int newStateIndex)
{
	stateMachineScript_->stateChanged(oldStateIndex, newStateIndex);
}

void StateMachineCodeBehind::stateEnded(int stateIndex)
{
	stateMachineScript_->stateEnded(stateIndex);
}

void StateMachineCodeBehind::preInitialize()
{
	stateMachineScript_ = boost::shared_ptr<StateMachineScript>(new StateMachineScript(debugger_));

	stateMachineScript_->stateMachineController_ = stateMachineController_;

	stateMachineScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		stateMachineScript_->preInitialize();
	}

	initialize();
}

void StateMachineCodeBehind::preCleanup()
{
	cleanup();

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		stateMachineScript_->preCleanup();
	}
}

void StateMachineCodeBehind::cleanup()
{
}

void StateMachineCodeBehind::initialize()
{
}