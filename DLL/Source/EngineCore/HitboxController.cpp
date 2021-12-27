#include "..\..\Headers\EngineCore\HitboxController.hpp"

using namespace firemelon;

HitboxController::HitboxController()
{
	hasActiveColliders_ = false;

	gridStartRow_ = -1;
	gridEndRow_ = -1;
	gridStartCol_ = -1;
	gridEndCol_ = -1;

	// Default to a position of 0, 0. Overwrite if the owner has a stage.
	stagePosition_ = boost::shared_ptr<Position>(new Position(0, 0));
}

HitboxController::~HitboxController()
{
}

int	HitboxController::getActiveHitboxCount()
{
	return activeHitboxIds_.size();
}

int	HitboxController::getActiveHitboxId(int index)
{
	int size = activeHitboxIds_.size();

	if (index >= 0 && index < size)
	{
		return activeHitboxIds_[index];
	}

	return -1;
}

unsigned char HitboxController::getActiveHitboxEdgeFlags(int index)
{
	int size = activeHitboxIds_.size();

	if (index >= 0 && index < size)
	{
		return activeHitboxEdgeFlags_[index];
	}

	return 0xFF;
}

bool HitboxController::getActiveHitboxCollisionStatus(int hitboxId)
{
	int size = activeHitboxCollisionStatuses_.size();

	for (int i = 0; i < size; i++)
	{
		if (activeHitboxIds_[i] == hitboxId)
		{
			return activeHitboxCollisionStatuses_[i];
		}
	}

	return false;
}

void HitboxController::setActiveHitboxCollisionStatus(int hitboxId, bool status)
{
	int size = activeHitboxCollisionStatuses_.size();

	for (int i = 0; i < size; i++)
	{
		if (activeHitboxIds_[i] == hitboxId)
		{
			activeHitboxCollisionStatuses_[i] = status;
		}
	}
}

void HitboxController::deactivateAllHitboxes()
{
	activeHitboxIds_.clear();
	activeHitboxCollisionStatuses_.clear();
	activeHitboxEdgeFlags_.clear();
}

void HitboxController::activateHitbox(int hitboxId, unsigned char edgeFlags)
{
	if (hitboxId >= 0)
	{
		activeHitboxIds_.push_back(hitboxId);
		activeHitboxEdgeFlags_.push_back(edgeFlags);
		activeHitboxCollisionStatuses_.push_back(false);
	}
}

void HitboxController::deactivateHitbox(int hitboxId)
{
	if (hitboxId >= 0)
	{
		int size = activeHitboxIds_.size();

		for (int i = size - 1; i >= 0; i--)
		{
			if (activeHitboxIds_[i] == hitboxId)
			{
				activeHitboxIds_.erase(activeHitboxIds_.begin() + i);
				activeHitboxEdgeFlags_.erase(activeHitboxEdgeFlags_.begin() + i);
				activeHitboxCollisionStatuses_.erase(activeHitboxCollisionStatuses_.begin() + i);
			}
		}
	}
}

int	HitboxController::getCollisionGridCellStartRow()
{
	return gridStartRow_;
}

int	HitboxController::getCollisionGridCellEndRow()
{
	return gridEndRow_;
}

int	HitboxController::getCollisionGridCellStartCol()
{
	return gridStartCol_;
}

int	HitboxController::getCollisionGridCellEndCol()
{
	return gridEndCol_;
}

void HitboxController::setCollisionGridCellStartRow(int value)
{
	gridStartRow_ = value;
}

void HitboxController::setCollisionGridCellEndRow(int value)
{
	gridEndRow_ = value;
}

void HitboxController::setCollisionGridCellStartCol(int value)
{
	gridStartCol_ = value;
}

void HitboxController::setCollisionGridCellEndCol(int value)
{
	gridEndCol_ = value;
}

void HitboxController::setOwnerPosition(PositionPtr position)
{
	ownerPosition_ = position;
}

PositionPtr HitboxController::getOwnerPosition()
{
	return ownerPosition_;
}

void HitboxController::setStagePosition(PositionPtr position)
{
	stagePosition_ = position;
}

PositionPtr HitboxController::getStagePosition()
{
	return stagePosition_;
}

void HitboxController::setStageHeight(int stageHeight)
{
	stageHeight_ = stageHeight;
}

int	HitboxController::getStageHeight()
{
	return stageHeight_;
}

void HitboxController::setStageWidth(int stageWidth)
{
	stageWidth_ = stageWidth;
}

int	HitboxController::getStageWidth()
{
	return stageWidth_;
}
