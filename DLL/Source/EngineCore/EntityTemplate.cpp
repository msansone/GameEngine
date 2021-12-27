#include "..\..\Headers\EngineCore\EntityTemplate.hpp"

using namespace firemelon;

EntityTemplate::EntityTemplate()
{
	initialStateIndex_ = 0;
	keepRoomActive_ = false;
	stageMetadata_ = boost::make_shared<StageMetadata>(StageMetadata());
}

EntityTemplate::~EntityTemplate()
{
	stageElements_.clear();

	anchorPoints_.clear();
	
	stateHitboxList_.clear();
}

int EntityTemplate::addStageElements(boost::shared_ptr<StageElements> newStageElements)
{
	newStageElements->renderer_ = renderer_;

	stageElements_.push_back(newStageElements);
	
	stateNameIdMap_[newStageElements->getAssociatedStateName()] = stageElements_.size();

	stateIdNameMap_[stageElements_.size() - 1] = newStageElements->getAssociatedStateName();

	return stageElements_.size() - 1;
}

int EntityTemplate::addStateHitbox(boost::shared_ptr<Hitbox> hitbox)
{
	stateHitboxList_.push_back(hitbox);
	
	return stateHitboxList_.size() - 1;
}

int	EntityTemplate::getStateHitboxCount()
{
	return stateHitboxList_.size();
}

boost::shared_ptr<Hitbox> EntityTemplate::getStateHitbox(int index)
{
	int size = stateHitboxList_.size();
	if (index >= 0 && index < size)
	{
		return stateHitboxList_[index];
	}
	else
	{
		return nullptr;
	}
}

int EntityTemplate::getStateIndexFromName(std::string name)
{
	return stateNameIdMap_[name] - 1;
}

std::string EntityTemplate::getStateNameFromIndex(int index)
{
	return stateIdNameMap_[index];
}

boost::shared_ptr<StageElements> EntityTemplate::getStageElements(int index)
{
	int size = stageElements_.size();

	if (index >= 0 && index < size)
	{
		return stageElements_[index];
	}
	else
	{
		return nullptr;
	}
}

void EntityTemplate::setInitialStateIndex(int initialStateIndex)
{
	initialStateIndex_ = initialStateIndex;
}

int EntityTemplate::getInitialStateIndex()
{
	return initialStateIndex_;
}

int EntityTemplate::getStageElementsCount()
{
	return stageElements_.size();
}

void EntityTemplate::setScriptName(std::string scriptName)
{
	scriptName_ = scriptName;
}

std::string EntityTemplate::getScriptName()
{
	return scriptName_;
}

void EntityTemplate::setTypeName(std::string typeName)
{
	scriptTypeName_ = typeName;
}

std::string	EntityTemplate::getScriptTypeName()
{
	return scriptTypeName_;
}

EntityClassification EntityTemplate::getClassification()
{
	return classification_;
}

void EntityTemplate::setClassification(EntityClassification classification)
{
	classification_ = classification;
}

void EntityTemplate::setKeepRoomActive(bool keepRoomActive)
{
	keepRoomActive_ = keepRoomActive;
}

bool EntityTemplate::getKeepRoomActive()
{
	return keepRoomActive_;
}

StageMetadataPtr EntityTemplate::getStageMetadata()
{
	return stageMetadata_;
}