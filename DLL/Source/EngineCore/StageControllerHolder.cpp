#include "..\..\Headers\EngineCore\StageControllerHolder.hpp"

using namespace firemelon;

StageControllerHolder::StageControllerHolder()
{
	stageController_ = nullptr;
}

StageControllerHolder::~StageControllerHolder()
{
}

StageControllerPtr StageControllerHolder::getStageController()
{
	return stageController_;
}

void StageControllerHolder::setStageController(StageControllerPtr stageController)
{
	if (stageController_ == nullptr)
	{
		stageController_ = stageController;
	}
}

bool StageControllerHolder::getHasStageController()
{
	return stageController_ != nullptr;
}