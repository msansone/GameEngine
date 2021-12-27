#include "..\..\Headers\EngineCore\AnimationFrame.hpp"

using namespace firemelon;

AnimationFrame::AnimationFrame(int spriteSheetCellColumn, int spriteSheetCellRow)
{
	alphaMaskSheetCellColumn_ = -1;
	alphaMaskSheetCellRow_ = -1;

	spriteSheetCellColumn_ = spriteSheetCellColumn;
	spriteSheetCellRow_ = spriteSheetCellRow;
}

AnimationFrame::~AnimationFrame()
{
}

void AnimationFrame::addHitboxReferencePy(int id)
{
	PythonReleaseGil unlocker;

	addHitboxReference(id);
}

void AnimationFrame::addHitboxReference(int id)
{
	hitboxReferences_.push_back(id);
}

void AnimationFrame::addAnchorPointReferencePy(int id)
{
	PythonReleaseGil unlocker;

	addAnchorPointReference(id);
}

void AnimationFrame::addAnchorPointReference(int id)
{
	anchorPointReferences_.push_back(id);
}

void AnimationFrame::addTriggerSignalPy(TriggerSignalId triggerSignalId)
{
	PythonReleaseGil unlocker;

	addTriggerSignal(triggerSignalId);
}

void AnimationFrame::addTriggerSignal(TriggerSignalId triggerSignalId)
{
	triggerSignals_.push_back(triggerSignalId);
}

int AnimationFrame::getSpriteSheetCellColumnPy()
{
	PythonReleaseGil unlocker;

	return getSpriteSheetCellColumn();
}

int AnimationFrame::getSpriteSheetCellColumn()
{
	return spriteSheetCellColumn_;
}

int AnimationFrame::getSpriteSheetCellRowPy()
{
	PythonReleaseGil unlocker;

	return getSpriteSheetCellRow();
}

int AnimationFrame::getSpriteSheetCellRow()
{
	return spriteSheetCellRow_;
}

int AnimationFrame::getHitboxCountPy()
{
	PythonReleaseGil unlocker;

	return getHitboxCount();
}

int AnimationFrame::getHitboxCount()
{
	return hitboxReferences_.size();
}

int AnimationFrame::getHitboxReferencePy(int index)
{
	PythonReleaseGil unlocker;

	return getHitboxReference(index);
}

int AnimationFrame::getHitboxReference(int index)
{
	int size = hitboxReferences_.size();

	if (index >= 0 && index < size)
	{
		return hitboxReferences_[index];
	}
	else
	{
		return -1;
	}
}

int AnimationFrame::getAnchorPointCountPy()
{
	PythonReleaseGil unlocker;

	return getAnchorPointCount();
}

int AnimationFrame::getAnchorPointCount()
{
	return anchorPointReferences_.size();
}

int AnimationFrame::getAnchorPointReferencePy(int index)
{
	PythonReleaseGil unlocker;

	return getAnchorPointReference(index);
}

int AnimationFrame::getAnchorPointReference(int index)
{
	int size = anchorPointReferences_.size();

	if (index >= 0 && index < size)
	{
		return anchorPointReferences_[index];
	}
	else
	{
		return -1;
	}
}

TriggerSignalId AnimationFrame::getTriggerSignalCountPy()
{
	PythonReleaseGil unlocker;

	return getTriggerSignalCount();
}

TriggerSignalId AnimationFrame::getTriggerSignalCount()
{
	return triggerSignals_.size();
}

TriggerSignalId AnimationFrame::getTriggerSignalPy(int index)
{
	PythonReleaseGil unlocker;

	return getTriggerSignal(index);
}

TriggerSignalId AnimationFrame::getTriggerSignal(int index)
{
	int size = triggerSignals_.size();

	if (index >= 0 && index < size)
	{
		return triggerSignals_[index];
	}
	else
	{
		return -1;
	}
}

AnchorPointPtr AnimationFrame::getAnchorPointPy(int index)
{
	PythonReleaseGil unlocker;

	return getAnchorPoint(index);
}

AnchorPointPtr AnimationFrame::getAnchorPoint(int index)
{
	int size = anchorPointReferences_.size();

	if (index >= 0 && index < size)
	{
		int anchorPointId = anchorPointReferences_[index];

		AnchorPointPtr ap = anchorPointManager_->getAnchorPoint(anchorPointId);

		return ap;
	}
	else
	{
		return nullptr;
	}
}


void AnimationFrame::setAlphaMaskSheetCellColumn(int value)
{
	alphaMaskSheetCellColumn_ = value;
}

void AnimationFrame::setAlphaMaskSheetCellRow(int value)
{
	alphaMaskSheetCellRow_ = value;
}