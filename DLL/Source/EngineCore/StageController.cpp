#include "..\..\Headers\EngineCore\StageController.hpp"

using namespace firemelon;

StageController::StageController()
{
	stage_ = StagePtr(new Stage());

	stageBackgroundRenderable_ = nullptr;
	
	stageForegroundRenderable_ = nullptr;
}

StageController::~StageController()
{
}

bool StageController::activateStageElements(int index)
{
	if (stateContainer_->currentStateIndex_ != index)
	{
		StageElementsPtr stageElements = stage_->getStageElementsByIndex(index);

		// Map the active hitbox identities to a status, to carry it over to the new state.
		std::map<HitboxIdentity, bool> hitboxStatusMap;

		HitboxControllerPtr hitboxController = hitboxControllerHolder_->getHitboxController();

		if (stage_->currentStageElements_ != nullptr && hitboxControllerHolder_->getHasHitboxController() == true)
		{
			int hitboxReferenceCount = stage_->currentStageElements_->getHitboxReferenceCount();

			for (int i = 0; i < hitboxReferenceCount; i++)
			{
				int hitboxId = stage_->currentStageElements_->getHitboxReference(i);

				HitboxIdentity identity = hitboxManager_->getHitbox(hitboxId)->getIdentity();

				hitboxStatusMap[identity] = hitboxController->getActiveHitboxCollisionStatus(hitboxId);
			}
		}

		// Deactivate the hitboxes in this state before changing to the new state.
		if (hitboxControllerHolder_->getHasHitboxController() == true)
		{
			hitboxController->deactivateAllHitboxes();
		}

		stateContainer_->previousStateIndex_ = stateContainer_->currentStateIndex_;

		stateContainer_->currentStateIndex_ = index;

		stage_->currentStageElements_ = stage_->elements_[index];

		// Activate stage hitboxes.
		int hitboxReferenceCount = stage_->currentStageElements_->getHitboxReferenceCount();

		for (int i = 0; i < hitboxReferenceCount; i++)
		{
			int hitboxId = stage_->currentStageElements_->getHitboxReference(i);

			HitboxIdentity identity = hitboxManager_->getHitbox(hitboxId)->getIdentity();

			hitboxController->activateHitbox(hitboxId);

			// If there is a status to carry over from the old state, set it now.
			std::map<HitboxIdentity, bool>::iterator itr = hitboxStatusMap.find(identity);

			if (itr != hitboxStatusMap.end())
			{
				bool mappedStatus = hitboxStatusMap[identity];

				hitboxController->setActiveHitboxCollisionStatus(hitboxId, mappedStatus);
			}
		}

		// Clear out the current list of animation slots, and repopulate based on the current state.
		stage_->activeAnimationSlots_.clear();

		// Reset the frame counter for the animations in this state, and activate any slot hitboxes.
		int animationSlotCount = stage_->currentStageElements_->getAnimationSlotCount();

		for (int i = 0; i < animationSlotCount; i++)
		{
			AnimationSlotPtr animationSlot = stage_->currentStageElements_->animationSlots_[i];

			int animationId = animationSlot->getAnimationId();

			if (animationId >= 0)
			{
				AnimationPlayerPtr animationPlayer = animationSlot->getAnimationPlayer();

				boost::shared_ptr<Animation> animation = animationManager_->getAnimationByIndex(animationId);

				animationPlayer->reset();
				
				// Activate the hitboxes in this frame and call the frame triggers.

				int currentFrameIndex = animationPlayer->getCurrentFrame();

				if (currentFrameIndex > -1)
				{
					size_t frameHitboxCount = animationSlot->hitboxReferences_[currentFrameIndex].size();

					for (size_t j = 0; j < frameHitboxCount; j++)
					{
						int hitboxId = animationSlot->hitboxReferences_[currentFrameIndex][j];

						HitboxIdentity identity = hitboxManager_->getHitbox(hitboxId)->getIdentity();

						hitboxController->activateHitbox(hitboxId);

						// If there is a status to carry over from the old state, set it now.
						std::map<HitboxIdentity, bool>::iterator itr = hitboxStatusMap.find(identity);

						if (itr != hitboxStatusMap.end())
						{
							bool mappedStatus = hitboxStatusMap[identity];

							hitboxController->setActiveHitboxCollisionStatus(hitboxId, mappedStatus);
						}
					}
				}
				
				AnimationFramePtr f = animation->getFrame(currentFrameIndex);

				if (f != nullptr)
				{
					int frameTriggerSignalCount = f->getTriggerSignalCount();

					for (int j = 0; j < frameTriggerSignalCount; j++)
					{
						TriggerSignalId triggerSignalId = f->getTriggerSignal(j);

						if (animationSlot->isBackground_ == true)
						{
							stageBackgroundRenderable_->frameTriggerSignal(triggerSignalId);
						}
						else
						{
							stageForegroundRenderable_->frameTriggerSignal(triggerSignalId);
						}
					}
				}
			}

			stage_->activeAnimationSlots_.push_back(animationSlot);

			stage_->reapplyTransform_ = true;
		}

		return true;
	}

	return false;
}

void StageController::addAnimationSlotToStageElementsByIndexPy(

	int          stageElementsIndex, std::string    slotName,       int          x,            int                 y,
	ColorRgbaPtr hueColor,           ColorRgbaPtr   blendColor,     float        blendPercent, float               rotation,
	int          framesPerSecond,    int            pivotX,         int          pivotY,       AnimationSlotOrigin origin,
	std::string  nextStateName,      AnimationStyle animationStyle, ColorRgbaPtr outlineColor, bool                background)

{
	PythonReleaseGil unlocker;

	addAnimationSlotToStageElementsByIndex(stageElementsIndex, slotName,       x,
		                                   y,                  hueColor,       blendColor,                 
		                                   blendPercent,       rotation,       framesPerSecond,            
		                                   pivotX,             pivotY,         origin,                     
		                                   nextStateName,      animationStyle, outlineColor,
										   background);
}

void StageController::addAnimationSlotToStageElementsByIndex(int          stageElementsIndex, std::string    slotName,       int          x,            int                 y,
														     ColorRgbaPtr hueColor,           ColorRgbaPtr   blendColor,     float        blendPercent, float               rotation,
														     int          framesPerSecond,    int            pivotX,         int          pivotY,       AnimationSlotOrigin origin,
														     std::string  nextStateName,      AnimationStyle animationStyle, ColorRgbaPtr outlineColor, bool                background)
{
	StageElementsPtr stageElements = getStageElements(stageElementsIndex);

	// Convert x and y to TLC based on the owner stage origin.

	StageMetadataPtr stageMetadata = stage_->getMetadata();

	switch (stageMetadata->getOrigin())
	{
	case STAGE_ORIGIN_CENTER:

		x = x + (int)(stageMetadata->getWidth() / 2);

		y = y + (int)(stageMetadata->getHeight() / 2);

		break;

	case STAGE_ORIGIN_TOP_MIDDLE:

		x = x + (int)(stageMetadata->getWidth() / 2);

		y = y;

		break;

	case STAGE_ORIGIN_BOTTOM_MIDDLE:

		x = x + (int)(stageMetadata->getWidth() / 2);

		y = y + stageMetadata->getHeight();

		break;
	}

	AnimationSlotPtr animationSlot = stageElements->addAnimationSlotInternal(slotName,                   x,                          y,
																			 hueColor,                   blendColor,                 blendPercent,
																			 rotation,                   framesPerSecond,            pivotX,
																			 pivotY,                     origin,                     nextStateName,
																			 animationStyle,			 outlineColor,               background);

	// Add this slot to the active slots if these are the active stage elements.
	if (stateContainer_->currentStateIndex_ == stageElementsIndex && animationSlot != nullptr)
	{
		stage_->activeAnimationSlots_.push_back(animationSlot);
	}
}

void StageController::addAnimationSlotToStageElementsByNamePy(std::string stageElementsName,
													 		  std::string slotName,
															  int x, int y,
															  ColorRgbaPtr hueColor, ColorRgbaPtr blendColor, float blendPercent,
															  float rotation,
															  int framesPerSecond,
															  int pivotX, int pivotY,
															  AnimationSlotOrigin origin,
															  std::string nextStateName,
															  AnimationStyle animationStyle, 
														      ColorRgbaPtr outlineColor, 
															  bool background)
{
	PythonReleaseGil unlocker;

	addAnimationSlotToStageElementsByName(stageElementsName, slotName,       x,
		                                  y,                 hueColor,       blendColor,                 
		                                  blendPercent,      rotation,       framesPerSecond,            
		                                  pivotX,            pivotY,         origin,                     
		                                  nextStateName,     animationStyle, outlineColor,
		                                  background);
}

void StageController::addAnimationSlotToStageElementsByName(std::string stageElementsName,
													 		std::string slotName,
															int x, int y,
															ColorRgbaPtr hueColor, ColorRgbaPtr blendColor, float blendPercent,
															float rotation,
															int framesPerSecond,
															int pivotX, int pivotY,
															AnimationSlotOrigin origin,
															std::string nextStateName,
															AnimationStyle animationStyle, 
															ColorRgbaPtr outlineColor, 
															bool background)
{
	int index = getStageElementsIndexFromName(stageElementsName);

	addAnimationSlotToStageElementsByIndex(index,         slotName,       x,                          
		                                   y,             hueColor,       blendColor,                 
		                                   blendPercent,  rotation,       framesPerSecond,            
		                                   pivotX,        pivotY,         origin,                     
		                                   nextStateName, animationStyle, outlineColor, 
		                                   background);
}

int	StageController::addExistingStageElements(StageElementsPtr newStageElements)
{
	// left off changing the add animatoin slot by moving it into the stage controller.
	// after this is ready, update the entity scripts
	// then, add in any of the necessary extra processing for handling transform application 
	// and adding the slot to the active stage slots if necessary
	newStageElements->renderer_ = renderer_;

	stage_->elements_.push_back(newStageElements);

	return stage_->elements_.size() - 1;
}


// ***  string string (fall through to string int method)
// ***  Stage Elements Name, Slot Name, Animation Name
// *** 
void StageController::assignAnimationByNameToSlotByNamePy(std::string stageElementsName, std::string slotName, std::string animationName)
{
	PythonReleaseGil unlocker;

	assignAnimationByNameToSlotByName(stageElementsName, slotName, animationName);
}


void StageController::assignAnimationByNameToSlotByName(std::string stageElementsName, std::string slotName, std::string animationName)
{
	int index = animationManager_->getAnimationId(animationName);

	// Call the string int method
	assignAnimationByIdToSlotByName(stageElementsName, slotName, index);
}


void StageController::assignAnimationByNameToSlotByNamePy(std::string stageElementsName, std::string slotName, std::string animationName, std::string synchWithSlotName)
{
	PythonReleaseGil unlocker;

	assignAnimationByNameToSlotByName(stageElementsName, slotName, animationName, synchWithSlotName);
}


void StageController::assignAnimationByNameToSlotByName(std::string stageElementsName, std::string slotName, std::string animationName, std::string synchWithSlotName)
{
	int animationIndex = animationManager_->getAnimationId(animationName);

	// Call the string int method
	assignAnimationByIdToSlotByName(stageElementsName, slotName, animationIndex, synchWithSlotName);
}


// *** string int
// *** Stage Elements Name, Slot Name, Animation Index ***
// ***
void StageController::assignAnimationByIdToSlotByNamePy(std::string stageElementsName, std::string slotName, int animationId)
{
	PythonReleaseGil unlocker;

	assignAnimationByIdToSlotByName(stageElementsName, slotName, animationId);
}

void StageController::assignAnimationByIdToSlotByName(std::string stageElementsName, std::string slotName, int animationId)
{
	StageElementsPtr stageElements = getStageElementsByName(stageElementsName);

	// Call the internal string int method.
	stageElements->assignAnimationByIdToSlotByNameInternal(slotName, animationId, true);

	// Re-apply transformations, so the modified animation slot is transformed correctly.
	stage_->reapplyTransform_ = true;
}

void StageController::assignAnimationByIdToSlotByNamePy(std::string stageElementsName, std::string slotName, int animationId, std::string synchWithSlotName)
{
	PythonReleaseGil unlocker;

	assignAnimationByIdToSlotByName(stageElementsName, slotName, animationId, synchWithSlotName);
}

void StageController::assignAnimationByIdToSlotByName(std::string stageElementsName, std::string slotName, int animationId, std::string synchWithSlotName)
{
	StageElementsPtr stageElements = getStageElementsByName(stageElementsName);

	// Call the internal string int method.
	stageElements->assignAnimationByIdToSlotByNameInternal(slotName, animationId, true, synchWithSlotName);

	// Re-apply transformations, so the modified animation slot is transformed correctly.
	stage_->reapplyTransform_ = true;
}


// ***  int string (fall through to int int)
// ***  Stage Elements Index, Slot Index, Animation Name
// *** 
void StageController::assignAnimationByNameToSlotByIndexPy(std::string stageElementsName, int slotIndex, std::string animationName)
{
	PythonReleaseGil unlocker;

	assignAnimationByNameToSlotByIndex(stageElementsName, slotIndex, animationName);
}

void StageController::assignAnimationByNameToSlotByIndex(std::string stageElementsName, int slotIndex, std::string animationName)
{
	int index = animationManager_->getAnimationId(animationName);

	// Call the int int version
	assignAnimationByIdToSlotByIndex(stageElementsName, slotIndex, index);
}

void StageController::assignAnimationByNameToSlotByIndexPy(std::string stageElementsName, int slotIndex, std::string animationName, std::string synchWithSlotName)
{
	PythonReleaseGil unlocker;

	assignAnimationByNameToSlotByIndex(stageElementsName, slotIndex, animationName, synchWithSlotName);
}

void StageController::assignAnimationByNameToSlotByIndex(std::string stageElementsName, int slotIndex, std::string animationName, std::string synchWithSlotName)
{
	int animationIndex = animationManager_->getAnimationId(animationName);

	// Call the int int version
	assignAnimationByIdToSlotByIndex(stageElementsName, slotIndex, animationIndex, synchWithSlotName);
}

// *** int int
// *** Stage Elements Index, Slot Index, Animation Index
// *** 
void StageController::assignAnimationByIdToSlotByIndexPy(std::string stageElementsName, int slotIndex, int animationId)
{
	PythonReleaseGil unlocker;

	assignAnimationByIdToSlotByIndex(stageElementsName, slotIndex, animationId);
}

void StageController::assignAnimationByIdToSlotByIndex(std::string stageElementsName, int slotIndex, int animationId)
{
	StageElementsPtr stageElements = getStageElementsByName(stageElementsName);

	// Call the internal int int method.
	stageElements->assignAnimationByIdToSlotByIndexInternal(slotIndex, animationId, true);

	// Re-apply transformations, so the modified animation slot is transformed correctly.
	stage_->reapplyTransform_ = true;
}

void StageController::assignAnimationByIdToSlotByIndexPy(std::string stageElementsName, int slotIndex, int animationId, std::string synchWithSlotName)
{
	PythonReleaseGil unlocker;

	assignAnimationByIdToSlotByIndex(stageElementsName, slotIndex, animationId, synchWithSlotName);
}

void StageController::assignAnimationByIdToSlotByIndex(std::string stageElementsName, int slotIndex, int animationId, std::string synchWithSlotName)
{
	StageElementsPtr stageElements = getStageElementsByName(stageElementsName);

	// Call the internal int int method.
	stageElements->assignAnimationByIdToSlotByIndexInternal(slotIndex, animationId, true, synchWithSlotName);

	// Re-apply transformations, so the modified animation slot is transformed correctly.
	stage_->reapplyTransform_ = true;
}


// ***  string string (fall through to string int)
// ***  Stage Elements Index, Slot Name, Animation Name
// *** 
void StageController::assignAnimationByNameToSlotByNamePy(int stageElementsIndex, std::string slotName, std::string animationName)
{
	PythonReleaseGil unlocker;

	assignAnimationByNameToSlotByName(stageElementsIndex, slotName, animationName);
}

void StageController::assignAnimationByNameToSlotByName(int stageElementsIndex, std::string slotName, std::string animationName)
{
	int index = animationManager_->getAnimationId(animationName);

	// Call the string int version
	assignAnimationByIdToSlotByName(stageElementsIndex, slotName, index);
}

void StageController::assignAnimationByNameToSlotByNamePy(int stageElementsIndex, std::string slotName, std::string animationName, std::string synchWithSlotName)
{
	PythonReleaseGil unlocker;

	assignAnimationByNameToSlotByName(stageElementsIndex, slotName, animationName, synchWithSlotName);
}

void StageController::assignAnimationByNameToSlotByName(int stageElementsIndex, std::string slotName, std::string animationName, std::string synchWithSlotName)
{
	int animationIndex = animationManager_->getAnimationId(animationName);
	
	// Call the string int version
	assignAnimationByIdToSlotByName(stageElementsIndex, slotName, animationIndex, synchWithSlotName);
}


// *** string int
// *** Stage Elements Index, Slot Name, Animation Index
// *** 
void StageController::assignAnimationByIdToSlotByNamePy(int stageElementsIndex, std::string slotName, int animationId)
{
	PythonReleaseGil unlocker;

	assignAnimationByIdToSlotByName(stageElementsIndex, slotName, animationId);
}

void StageController::assignAnimationByIdToSlotByName(int stageElementsIndex, std::string slotName, int animationId)
{
	StageElementsPtr stageElements = getStageElements(stageElementsIndex);

	// Call the internal string int method.
	stageElements->assignAnimationByIdToSlotByNameInternal(slotName, animationId, true);

	// Re-apply transformations, so the modified animation slot is transformed correctly.
	stage_->reapplyTransform_ = true;
}

void StageController::assignAnimationByIdToSlotByNamePy(int stageElementsIndex, std::string slotName, int animationId, std::string synchWithSlotName)
{
	PythonReleaseGil unlocker;

	assignAnimationByIdToSlotByName(stageElementsIndex, slotName, animationId, synchWithSlotName);
}

void StageController::assignAnimationByIdToSlotByName(int stageElementsIndex, std::string slotName, int animationId, std::string synchWithSlotName)
{
	StageElementsPtr stageElements = getStageElements(stageElementsIndex);

	// Call the internal string int method.
	stageElements->assignAnimationByIdToSlotByNameInternal(slotName, animationId, true, synchWithSlotName);

	// Re-apply transformations, so the modified animation slot is transformed correctly.
	stage_->reapplyTransform_ = true;
}



// ***  int string (fall through to int int)
// ***  Stage Elements Index, Slot Index, Animation Name
// *** 
void StageController::assignAnimationByNameToSlotByIndexPy(int stageElementsIndex, int slotIndex, std::string animationName)
{
	PythonReleaseGil unlocker;

	assignAnimationByNameToSlotByIndex(stageElementsIndex, slotIndex, animationName);
}

void StageController::assignAnimationByNameToSlotByIndex(int stageElementsIndex, int slotIndex, std::string animationName)
{
	int index = animationManager_->getAnimationId(animationName);

	// Call the int int version
	assignAnimationByIdToSlotByIndex(stageElementsIndex, slotIndex, index);
}

void StageController::assignAnimationByNameToSlotByIndexPy(int stageElementsIndex, int slotIndex, std::string animationName, std::string synchWithSlotName)
{
	PythonReleaseGil unlocker;

	assignAnimationByNameToSlotByIndex(stageElementsIndex, slotIndex, animationName, synchWithSlotName);
}

void StageController::assignAnimationByNameToSlotByIndex(int stageElementsIndex, int slotIndex, std::string animationName, std::string synchWithSlotName)
{
	int animationIndex = animationManager_->getAnimationId(animationName);

	// Call the int int version
	assignAnimationByIdToSlotByIndex(stageElementsIndex, slotIndex, animationIndex, synchWithSlotName);
}

// *** int int
// *** Stage Elements Index, Slot Index, Animation Index
// *** 
void StageController::assignAnimationByIdToSlotByIndexPy(int stageElementsIndex, int slotIndex, int animationId)
{
	PythonReleaseGil unlocker;

	assignAnimationByIdToSlotByIndex(stageElementsIndex, slotIndex, animationId);
}

void StageController::assignAnimationByIdToSlotByIndex(int stageElementsIndex, int slotIndex, int animationId)
{
	StageElementsPtr stageElements = getStageElements(stageElementsIndex);

	// Call the internal int int method.
	stageElements->assignAnimationByIdToSlotByIndexInternal(slotIndex, animationId, true);

	// Re-apply transformations, so the modified animation slot is transformed correctly.
	stage_->reapplyTransform_ = true;
}

void StageController::assignAnimationByIdToSlotByIndexPy(int stageElementsIndex, int slotIndex, int animationId, std::string synchWithSlotName)
{
	PythonReleaseGil unlocker;

	assignAnimationByIdToSlotByIndex(stageElementsIndex, slotIndex, animationId, synchWithSlotName);
}

void StageController::assignAnimationByIdToSlotByIndex(int stageElementsIndex, int slotIndex, int animationId, std::string synchWithSlotName)
{
	StageElementsPtr stageElements = getStageElements(stageElementsIndex);

	// Call the internal int int method.
	stageElements->assignAnimationByIdToSlotByIndexInternal(slotIndex, animationId, true, synchWithSlotName);

	// Re-apply transformations, so the modified animation slot is transformed correctly.
	stage_->reapplyTransform_ = true;
}


StagePtr StageController::getStage()
{
	return stage_;
}

StageRenderablePtr StageController::getStageBackgroundRenderable()
{
	return stageBackgroundRenderable_;
}

StageRenderablePtr StageController::getStageForegroundRenderable()
{
	return stageForegroundRenderable_;
}

StageElementsPtr StageController::getStageElementsPy(int index)
{
	PythonReleaseGil unlocker;

	return getStageElements(index);
}

StageElementsPtr StageController::getStageElements(int index)
{
	if (index >= 0 && index < stage_->elements_.size())
	{
		return stage_->elements_[index];
	}

	return nullptr;
}

StageElementsPtr StageController::getCurrentStageElementsPy()
{
	PythonReleaseGil unlocker;

	return getCurrentStageElements();
}

StageElementsPtr StageController::getCurrentStageElements()
{
	return stage_->getStageElementsByIndex(stateContainer_->currentStateIndex_);
}

int StageController::getCurrentStageElementsIndexPy()
{
	PythonReleaseGil unlocker;

	return getCurrentStageElementsIndex();
}

int StageController::getCurrentStageElementsIndex()
{
	return stateContainer_->currentStateIndex_;
}

ColorRgbaPtr StageController::getHueColorPy()
{
	PythonReleaseGil unlocker;

	return getHueColor();
}

ColorRgbaPtr StageController::getHueColor()
{
	return stage_->metadata_->renderEffects_->getHueColor();
}

ColorRgbaPtr StageController::getOutlineColorPy()
{
	PythonReleaseGil unlocker;

	return getOutlineColor();
}

ColorRgbaPtr StageController::getOutlineColor()
{
	return stage_->metadata_->renderEffects_->getOutlineColor();
}

boost::shared_ptr<Position> StageController::getPivotPointPy()
{
	PythonReleaseGil unlocker;

	return getPivotPoint();
}

boost::shared_ptr<Position> StageController::getPivotPoint()
{
	return stage_->metadata_->getRotationOperation()->getPivotPoint();
}

float StageController::getRotationAnglePy()
{
	PythonReleaseGil unlocker;

	return getRotationAngle();
}

float StageController::getRotationAngle()
{
	return stage_->getRotationAngle();
}

StageElementsPtr StageController::getStageElementsByNamePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getStageElementsByName(name);
}

StageElementsPtr StageController::getStageElementsByName(std::string name)
{
	int stateIndex = getStageElementsIndexFromName(name);

	if (stateIndex >= 0)
	{
		return getStageElements(stateIndex);
	}

	return nullptr;
}

int	StageController::getStageElementsIndexFromNamePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getStageElementsIndexFromName(name);
}

int	StageController::getStageElementsIndexFromName(std::string name)
{
	return stateContainer_->stateNameIdMap_[name] - 1;
}

void StageController::setRotationAnglePy(float rotationAngle)
{
	PythonReleaseGil unlocker;

	setRotationAngle(rotationAngle);
}

void StageController::setRotationAngle(float rotationAngle)
{
	stage_->setRotationAngle(rotationAngle);
}

void StageController::synchAnimationSlotsPy(std::string synchFromStateName, std::string synchFromSlotName, std::string synchToStateName, std::string synchToSlotName)
{
	PythonReleaseGil unlocker;

	synchAnimationSlots(synchFromStateName, synchFromSlotName, synchToStateName, synchToSlotName);
}

void StageController::synchAnimationSlots(std::string synchFromStateName, std::string synchFromSlotName, std::string synchToStateName, std::string synchToSlotName)
{
	StageElementsPtr synchFromStageElements = getStageElementsByName(synchFromStateName);

	StageElementsPtr synchToStageElements = getStageElementsByName(synchToStateName);

	if (synchFromStageElements != nullptr && synchToStageElements != nullptr)
	{
		StageElementsPtr currentStageElements = getCurrentStageElements();
		
		AnimationSlotPtr synchFromSlot = synchFromStageElements->getAnimationSlotByName(synchFromSlotName);

		AnimationSlotPtr synchToSlot = synchToStageElements->getAnimationSlotByName(synchToSlotName);

		if (synchFromSlot != nullptr && synchToSlot != nullptr)
		{
			AnimationPlayerPtr synchFromAnimationPlayer = synchFromSlot->getAnimationPlayer();

			AnimationPlayerPtr synchToAnimationPlayer = synchToSlot->getAnimationPlayer();

			// Deactivate any hitboxes from the current frame before synching.
			int previousFrameIndex = synchToAnimationPlayer->getCurrentFrame();

			int animationId = synchToSlot->getAnimationId();

			if (animationId >= 0)
			{
				boost::shared_ptr<Animation> animation = animationManager_->getAnimationByIndex(animationId);
				
				// If the synch to state isn't the current state, there's no need to do anything with hitboxes, because they won't be active.
				// The only active hitboxes will be the ones in the currently active state.
				if (currentStageElements->getAssociatedStateName() == synchToStageElements->getAssociatedStateName())
				{
					boost::shared_ptr<AnimationFrame> frame = animation->getFrame(previousFrameIndex);

					int hitboxCount = synchToSlot->hitboxReferences_[previousFrameIndex].size();

					for (int j = 0; j < hitboxCount; j++)
					{
						int hitboxId = synchToSlot->hitboxReferences_[previousFrameIndex][j];

						hitboxControllerHolder_->getHitboxController()->deactivateHitbox(hitboxId);
					}
				}
			}

			synchToAnimationPlayer->synch(synchFromAnimationPlayer);

			// Do I need to change the hitboxes for the animation frames?? I probably do.

			// Fire any frame triggers if necessary. 
			// Note: It is possible that there could be situations where you don't want the frame trigger to fire, such as if both animations
			// being synched have the same trigger, and you don't want it to fire twice. I hate doing this, but I'll have to figure out what
			// to do about then when it comes up. (Maybe check if it exists in both?)

			int currentFrameIndex = synchToAnimationPlayer->getCurrentFrame();

			animationId = synchToSlot->getAnimationId();

			if (animationId >= 0)
			{
				boost::shared_ptr<Animation> animation = animationManager_->getAnimationByIndex(animationId);

				// Activate the hitboxes in this frame and call the frame triggers.
				// Note: ONLY DO THIS IF THE STATE WHICH IS BEING SYNCHED TO IS ACTIVE! Inactive states will not activate hitboxes.
				if (currentStageElements->getAssociatedStateName() == synchToStageElements->getAssociatedStateName())
				{
					boost::shared_ptr<AnimationFrame> frame = animation->getFrame(currentFrameIndex);

					if (frame != nullptr)
					{
						int hitboxCount = synchFromSlot->hitboxReferences_[currentFrameIndex].size();

						for (int j = 0; j < hitboxCount; j++)
						{
							int hitboxId = synchFromSlot->hitboxReferences_[currentFrameIndex][j];

							hitboxControllerHolder_->getHitboxController()->activateHitbox(hitboxId, 0xFF);
						}

						int frameTriggerSignalCount = frame->getTriggerSignalCount();

						for (int j = 0; j < frameTriggerSignalCount; j++)
						{
							TriggerSignalId triggerSignalId = frame->getTriggerSignal(j);

							if (synchToSlot->isBackground_ == true)
							{
								stageBackgroundRenderable_->frameTriggerSignal(triggerSignalId);
							}
							else
							{
								stageForegroundRenderable_->frameTriggerSignal(triggerSignalId);
							}

						}
					}
				}
			}
		}
	}
}



float StageController::getExtentBottomPy()
{
	PythonReleaseGil unlocker;

	return getExtentBottom();
}

float StageController::getExtentBottom()
{
	float extentBottom = stage_->metadata_->renderEffects_->getExtentBottom();

	return extentBottom;
}

float StageController::getExtentLeftPy()
{
	PythonReleaseGil unlocker;

	return getExtentLeft();
}

float StageController::getExtentLeft()
{
	return  stage_->metadata_->renderEffects_->getExtentLeft();
}

float StageController::getExtentTopPy()
{
	PythonReleaseGil unlocker;

	return getExtentTop();
}

float StageController::getExtentTop()
{
	float extentTop = stage_->metadata_->renderEffects_->getExtentTop();

	return extentTop;
}

float StageController::getExtentRightPy()
{
	PythonReleaseGil unlocker;

	return getExtentRight();
}

float StageController::getExtentRight()
{
	float extentRight = stage_->metadata_->renderEffects_->getExtentRight();
	
	return extentRight;
}

bool StageController::getMirrorHorizontallyPy()
{
	PythonReleaseGil unlocker;

	return getMirrorHorizontally();
}

bool StageController::getMirrorHorizontally()
{
	return stage_->getMirrorHorizontally();
}

void StageController::setMirrorHorizontallyPy(bool mirrorHorizontally)
{
	PythonReleaseGil unlocker;

	setMirrorHorizontally(mirrorHorizontally);
}

void StageController::setMirrorHorizontally(bool mirrorHorizontally)
{
	stage_->setMirrorHorizontally(mirrorHorizontally);
}


void StageController::setExtentBottomPy(float extentBottom)
{
	PythonReleaseGil unlocker;

	setExtentBottom(extentBottom);
}

void StageController::setExtentBottom(float extentBottom)
{
	stage_->metadata_->renderEffects_->setExtentBottom(extentBottom);
}

void StageController::setExtentLeftPy(float extentLeft)
{
	PythonReleaseGil unlocker;

	setExtentLeft(extentLeft);
}

void StageController::setExtentLeft(float extentLeft)
{
	stage_->metadata_->renderEffects_->setExtentLeft(extentLeft);
}

void StageController::setExtentTopPy(float extentTop)
{
	PythonReleaseGil unlocker;

	setExtentTop(extentTop);
}

void StageController::setExtentTop(float extentTop)
{
	stage_->metadata_->renderEffects_->setExtentTop(extentTop);
}

void StageController::setExtentRightPy(float extentRight)
{
	PythonReleaseGil unlocker;

	setExtentRight(extentRight);
}

void StageController::setExtentRight(float extentRight)
{
	stage_->metadata_->renderEffects_->setExtentRight(extentRight);
}

unsigned int StageController::getHeightPy()
{
	PythonReleaseGil unlocker;

	return getHeight();
}

unsigned int StageController::getHeight()
{
	return stage_->metadata_->getHeight();
}

bool StageController::getIsVisiblePy()
{
	PythonReleaseGil unlocker;

	return getIsVisible();
}

bool StageController::getIsVisible()
{
	// Not sure the best way to handle this. For now, just use the foreground.
	return stageForegroundRenderable_->getIsVisible();
}

int StageController::getRenderOrderPy()
{
	PythonReleaseGil unlocker;

	return getRenderOrder();
}

int StageController::getRenderOrder()
{
	// Use the foreground, as the background will always be rendered behind it.
	// The foreground is the "main" layer.
	return stageForegroundRenderable_->getRenderOrder();
}

unsigned int StageController::getWidthPy()
{
	PythonReleaseGil unlocker;

	return getWidth();
}

unsigned int StageController::getWidth()
{
	return stage_->metadata_->getWidth();
}

bool StageController::getInterpolateExtentsPy()
{
	PythonReleaseGil unlocker;

	return getInterpolateExtents();
}

bool StageController::getInterpolateExtents()
{
	return stage_->metadata_->renderEffects_->getInterpolateExtents();
}

void StageController::setInterpolateExtentsPy(bool interpolateExtents)
{
	PythonReleaseGil unlocker;

	setInterpolateExtents(interpolateExtents);
}

void StageController::setInterpolateExtents(bool interpolateExtents)
{
	stage_->metadata_->renderEffects_->setInterpolateExtents(interpolateExtents);
}

bool StageController::getInterpolateRotationPy()
{
	PythonReleaseGil unlocker;

	return getInterpolateRotation();
}

bool StageController::getInterpolateRotation()
{
	return stage_->metadata_->renderEffects_->getInterpolateRotation();
}

void StageController::setInterpolateRotationPy(bool interpolateRotation)
{
	PythonReleaseGil unlocker;

	setInterpolateRotation(interpolateRotation);
}

void StageController::setInterpolateRotation(bool interpolateRotation)
{
	stage_->metadata_->renderEffects_->setInterpolateRotation(interpolateRotation);
}

void StageController::setIsVisiblePy(bool isVisisble)
{
	PythonReleaseGil unlocker;

	setIsVisible(isVisisble);
}

void StageController::setIsVisible(bool isVisisble)
{
	stageForegroundRenderable_->setIsVisible(isVisisble);
}

void StageController::setRenderOrder(int renderOrder)
{
	stageForegroundRenderable_->setRenderOrder(renderOrder);

	// TODO: Allow user to set how far back the background goes.
	stageBackgroundRenderable_->setRenderOrder(renderOrder - stageBackgroundRenderable_->stage_->metadata_->backgroundDepth_);
}