#include "..\..\Headers\EngineCore\StateMachineController.hpp"

using namespace firemelon;

StateMachineController::StateMachineController()
{
	dynamicsController_ = nullptr;
	mapLayer_ = -1;
	isDynamic_ = false;
	ownerId_ = -1;
	isSpriteVisible_ = true;
	isInvalidated_ = false;
	stateContainer_ = StateContainerPtr(new StateContainer());
}

StateMachineController::~StateMachineController()
{
}

int	StateMachineController::addExistingState(StateMachineStatePtr newStateMachineState)
{
	stateContainer_->addState(newStateMachineState);
		
	return stateContainer_->states_.size() - 1;
}

int StateMachineController::getCurrentStateIndexPy()
{
	PythonReleaseGil unlocker;

	return getCurrentStateIndex();
}

int StateMachineController::getCurrentStateIndex()
{
	return stateContainer_->currentStateIndex_;
}

void StateMachineController::setDynamicsController(DynamicsController* dynamicsController)
{
	dynamicsController_ = dynamicsController;
	isDynamic_ = true;
}

DynamicsController* StateMachineController::getDynamicsController()
{
	return dynamicsController_;
}

bool StateMachineController::setStateByNamePy(std::string stateName)
{
	PythonReleaseGil unlocker;

	return setStateByName(stateName);
}

bool StateMachineController::setStateByName(std::string stateName)
{
	return setStateByNameInternal(stateName, false);
}

bool StateMachineController::setStateByNameInternal(std::string stateName, bool forceStageChange)
{
	bool stateChanged = false;

	if (stateContainer_->stateNameIdMap_.find(stateName) == stateContainer_->stateNameIdMap_.end()) 
	{
		std::cout << "Failed to set state to " << stateName << " because it does not exist in this entity." << std::endl;
	}
	else
	{
		int index = stateContainer_->stateNameIdMap_[stateName] - 1;

		if (index != stateContainer_->currentStateIndex_ || forceStageChange == true)
		{
			stateChanged = setStateByIndex(index);
		}
	}

	return stateChanged;
}

bool StateMachineController::setStateByIndexPy(int stateIndex)
{
	PythonReleaseGil unlocker;

	return setStateByIndex(stateIndex);
}

bool StateMachineController::setStateByIndex(int stateIndex)
{
	bool stateChanged = false;

	if (stateContainer_->currentStateIndex_ != stateIndex)
	{
		int size = stateContainer_->getStateCount();

		if (stateIndex >= 0 && stateIndex < size)
		{
			StageControllerPtr stageController = stageControllerHolder_->getStageController();

			stateChanged = stageController->activateStageElements(stateIndex);

			if (stateContainer_->previousStateIndex_ >= 0 && stateContainer_->currentStateIndex_ >= 0)
			{
				stateChangedSignal(stateContainer_->previousStateIndex_, stateContainer_->currentStateIndex_);
			}
		}
	}

	return stateChanged;
}

void StateMachineController::setOwnerId(int id)
{
	ownerId_ = id;
}

int StateMachineController::getOwnerIdPy()
{
	PythonReleaseGil unlocker;

	return getOwnerId();
}

int StateMachineController::getOwnerId()
{
	return ownerId_;
}

EntityTypeId StateMachineController::getOwnerEntityTypeId()
{
	return ownerEntityTypeId_;
}

void StateMachineController::setOwnerEntityTypeId(EntityTypeId entityTypeId)
{
	ownerEntityTypeId_ = entityTypeId;
}

boost::shared_ptr<StateMachineState> StateMachineController::addStatePy(std::string name)
{
	PythonReleaseGil unlocker;

	return addState(name);
}

boost::shared_ptr<StateMachineState> StateMachineController::addState(std::string name)
{
	std::map<std::string, int>::iterator itr = stateContainer_->stateNameIdMap_.find(name);
	
	boost::shared_ptr<StateMachineState> newState = nullptr;

	if (itr == stateContainer_->stateNameIdMap_.end())
	{
		newState = boost::make_shared<StateMachineState>(StateMachineState(name));

		addExistingState(newState);
	}
	else
	{
		newState = getStateByName(name);
	}

	return newState;
}

boost::shared_ptr<StateMachineState> StateMachineController::getStatePy(int index)
{
	PythonReleaseGil unlocker;

	return getState(index);
}

boost::shared_ptr<StateMachineState> StateMachineController::getState(int index)
{
	return stateContainer_->getStateByIndex(index);
}

boost::shared_ptr<StateMachineState> StateMachineController::getCurrentStatePy()
{
	PythonReleaseGil unlocker;

	return getCurrentState();
}

boost::shared_ptr<StateMachineState> StateMachineController::getCurrentState()
{
	return stateContainer_->getStateByIndex(stateContainer_->currentStateIndex_);
}

boost::shared_ptr<StateMachineState> StateMachineController::getStateByNamePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getStateByName(name);
}

boost::shared_ptr<StateMachineState> StateMachineController::getStateByName(std::string name)
{
	int stateIndex = getStateIndexFromName(name);

	if (stateIndex >= 0)
	{
		return getState(stateIndex);
	}
	
	return nullptr;
}

int	StateMachineController::getStateIndexFromNamePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getStateIndexFromName(name);
}

int	StateMachineController::getStateIndexFromName(std::string name)
{
	return stateContainer_->stateNameIdMap_[name] - 1;
}

std::string	StateMachineController::getStateNameFromIndexPy(int index)
{
	PythonReleaseGil unlocker;

	return getStateNameFromIndex(index);
}

std::string	StateMachineController::getStateNameFromIndex(int index)
{
	return stateContainer_->stateIdNameMap_[index];
}

int	StateMachineController::getStateCountPy()
{
	PythonReleaseGil unlocker;

	return getStateCount();
}

int	StateMachineController::getStateCount()
{
	return stateContainer_->getStateCount();
}