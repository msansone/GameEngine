#include "..\..\Headers\EngineCore\StageElements.hpp"

using namespace firemelon;

StageElements::StageElements(std::string associatedStateName)
{
	associatedStateName_ = associatedStateName;

	bottomBoundary_ = 0;

	leftBoundary_ = 0;

	linearAlgebraUtility_ = boost::make_shared<LinearAlgebraUtility>(LinearAlgebraUtility());

	reapplyTransform_ = true;

	returnedAnchorPoint_ = AnchorPointPtr(new AnchorPoint("Anchor Point Not Found", 0, 0));

	rightBoundary_ = 0;

	singleFrame_ = false;

	stageRenderEffects_ = RenderEffectsPtr(new RenderEffects());

	topBoundary_ = 0;
}

StageElements::~StageElements()
{
	animationSlots_.clear();
}

void StageElements::addHitboxReferencePy(int id)
{
	PythonReleaseGil unlocker;

	addHitboxReference(id);
}

void StageElements::addHitboxReference(int id)
{
	hitboxReferences_.push_back(id);

	//hitboxEdgeFlags_.push_back(0xFF);

	//hitboxCollisionStatuses_.push_back(false);
}


// Internal slot add assumes the x and y are already in the native TLC position.
//void StageElements::addAnimationSlotInternal(std::string                 slotName,                   int                 x,                          int         y,
//											 ColorRgbaPtr                hueColor,                   ColorRgbaPtr        blendColor,                 float       blendPercent,
//											 float                       rotation,                   int                 framesPerSecond,            int         pivotX,
//	                                         int                         pivotY,                     float               alphaGradientFrom,          float       alphaGradientTo,
//											 int                         alphaGradientRadialCenterX, int                 alphaGradientRadialCenterY, float       alphaGradientRadius,
//											 AlphaGradientDirection      alphaGradientDirection,     AnimationSlotOrigin origin,                     std::string nextStateName,
//	                                         AnimationStyle              animationStyle)

AnimationSlotPtr StageElements::addAnimationSlotInternal(std::string                 slotName,                   int                 x,                          int         y,
														 ColorRgbaPtr                hueColor,                   ColorRgbaPtr        blendColor,                 float       blendPercent,
														 float                       rotation,                   int                 framesPerSecond,            int         pivotX,
														 int                         pivotY,                     AnimationSlotOrigin origin,                     std::string nextStateName,
														 AnimationStyle              animationStyle,			 ColorRgbaPtr        outlineColor,				 bool        background)
{
	int index = getAnimationSlotIndex(slotName);

	if (index >= 0)
	{
		std::cout << "Slot with name " << slotName << " could not be added because that name is already in use." << std::endl;

		return nullptr;
	}

	AnimationSlotPtr newAnimationSlot = AnimationSlotPtr(new AnimationSlot(slotName, x, y));

	newAnimationSlot->animationPlayer_->setAnimationStyle(animationStyle);

	newAnimationSlot->animationPlayer_->setFramesPerSecond(framesPerSecond);

	newAnimationSlot->origin_ = origin;

	newAnimationSlot->isBackground_ = background;

	newAnimationSlot->nextStateName_ = nextStateName;
	
	RenderEffectsPtr renderEffects = newAnimationSlot->renderEffects_;

	renderEffects->getHueColor()->setR(hueColor->getR());
	renderEffects->getHueColor()->setG(hueColor->getG());
	renderEffects->getHueColor()->setB(hueColor->getB());
	renderEffects->getHueColor()->setA(hueColor->getA());

	renderEffects->getOutlineColor()->setR(outlineColor->getR());
	renderEffects->getOutlineColor()->setG(outlineColor->getG());
	renderEffects->getOutlineColor()->setB(outlineColor->getB());
	renderEffects->getOutlineColor()->setA(outlineColor->getA());

	renderEffects->getBlendColor()->setR(blendColor->getR());
	renderEffects->getBlendColor()->setG(blendColor->getG());
	renderEffects->getBlendColor()->setB(blendColor->getB());
	renderEffects->getBlendColor()->setA(blendColor->getA());

	renderEffects->setBlendPercent(blendPercent);

	//renderEffects->setAlphaGradientDirection(alphaGradientDirection);

	//renderEffects->setAlphaGradientFrom(alphaGradientFrom);

	//renderEffects->setAlphaGradientTo(alphaGradientTo);

	//// Translate the  radial center point, which is relative to the center of this sprite, to screen space coordinates.
	//renderEffects->getAlphaGradientRadialCenterPoint()->setX(alphaGradientRadialCenterX);

	//renderEffects->getAlphaGradientRadialCenterPoint()->setY(alphaGradientRadialCenterY);

	//renderEffects->setAlphaGradientRadius(alphaGradientRadius);

	RotationOperationPtr slotRotationOperation = renderEffects->getRotationOperation(0);

	slotRotationOperation->setAngle(rotation);

	slotRotationOperation->getPivotPoint()->setX(pivotX);

	slotRotationOperation->getPivotPoint()->setY(pivotY);

	animationSlots_.push_back(newAnimationSlot);

	return newAnimationSlot;
}

void StageElements::applyMirrorHorizontallyTransform()
{
	int stageWidth = stageMetadata_->getWidth();

	int stageCenterX = stageWidth / 2;

	int size = animationSlots_.size();

	for (int i = 0; i < size; i++)
	{
		AnimationSlotPtr animationSlot = animationSlots_[i];

		// Mirror the transformed corners.
		linearAlgebraUtility_->mirrorPointsHorizontally(animationSlot->transformedCorners_, stageCenterX);

		// Also need to mirror any hitboxes that are in this animation's frames.
		int animationId = animationSlot->getAnimationId();

		if (animationId >= 0)
		{
			AnimationPtr animation = animationManager_->getAnimationByIndex(animationId);

			int frameCount = animation->getFrameCount();

			for (int j = 0; j < frameCount; j++)
			{
				AnimationFramePtr frame = animation->getFrame(j);

				int hitboxCount = animationSlot->hitboxReferences_[j].size();

				for (int k = 0; k < hitboxCount; k++)
				{
					int hitboxId = animationSlot->hitboxReferences_[j][k];

					HitboxPtr hitbox = hitboxManager_->getHitbox(hitboxId);

					linearAlgebraUtility_->mirrorPointsHorizontally(hitbox->transformedCorners_, stageCenterX);
				}
				
				int anchorPointCount = animationSlot->anchorPointReferences_[j].size();

				// BUG TODO: Anchor points can't be stored in the frame. Like hitboxes, they need to exist in the slot, otherwise
				// entities using the same animation will step on each others toes.
				for (int k = 0; k < anchorPointCount; k++)
				{
					int anchorPointId = animationSlot->anchorPointReferences_[j][k];

					AnchorPointPtr anchorPoint = anchorPointManager_->getAnchorPoint(anchorPointId);
					
					linearAlgebraUtility_->mirrorPointHorizontally(anchorPoint->transformedPosition_, stageCenterX);
				}
			}
		}
	}


	// Mirror stage hitboxes
	int stageHitboxCount = getHitboxReferenceCount();

	for (int i = 0; i < stageHitboxCount; i++)
	{
		int hitboxId = getHitboxReference(i);

		HitboxPtr hitbox = hitboxManager_->getHitbox(hitboxId);
		
		linearAlgebraUtility_->mirrorPointsHorizontally(hitbox->transformedCorners_, stageCenterX);
	}
}

void StageElements::applyRotationTransforms(double lerp)
{
	PositionPtr stagePivotPoint = stageMetadata_->getRotationOperation()->getPivotPoint();

	float stageRotationAngle = stageMetadata_->getRotationOperation()->getAngle();

	float previousStageRotationAngle = stageMetadata_->getRotationOperation()->previousAngle_;

	float previousPreviousStageRotationAngle = stageMetadata_->getRotationOperation()->previousPreviousAngle_;

	// Interpolate the rotation angle.
	if (stageMetadata_->renderEffects_->getInterpolateRotation() == true)
	{
		stageRotationAngle = interpolateAngle(stageRotationAngle, previousStageRotationAngle, previousPreviousStageRotationAngle, lerp);
	}

	// Determine the value by which all the vertices must be shifted to the screen origin.
	float stageTranslationVectorX = stagePivotPoint->getX();

	float stageTranslationVectorY = stagePivotPoint->getY();

	int size = animationSlots_.size();

	for (int i = 0; i < size; i++)
	{
		AnimationSlotPtr animationSlot = animationSlots_[i];

		// Rotate the animation around the slot's pivot point.
		float slotRotationAngle = animationSlot->getRenderEffects()->getRotationOperation(0)->getAngle();

		float previousSlotRotationAngle = animationSlot->getRenderEffects()->getRotationOperation(0)->previousAngle_;

		float previousPreviousSlotRotationAngle = animationSlot->getRenderEffects()->getRotationOperation(0)->previousPreviousAngle_;

		// Interpolate the rotation angle.
		if (stageMetadata_->renderEffects_->getInterpolateRotation() == true)
		{
			slotRotationAngle = interpolateAngle(slotRotationAngle, previousSlotRotationAngle, previousPreviousSlotRotationAngle, lerp);
		}

		auto pivotPoint = animationSlot->getRenderEffects()->getRotationOperation(0)->getPivotPoint();

		int slotNativePositionX = animationSlot->getNativePosition()->getX();

		int slotNativePositionY = animationSlot->getNativePosition()->getY();

		// Determine the value by which all the vertices must be shifted to the screen origin.
		float slotTranslationVectorX = slotNativePositionX + pivotPoint->getX();

		float slotTranslationVectorY = slotNativePositionY + pivotPoint->getY();

		linearAlgebraUtility_->rotatePoints(slotRotationAngle, animationSlot->nativeCorners_, animationSlot->transformedCorners_, slotTranslationVectorX, slotTranslationVectorY);

		// Also need to rotate any hitboxes and anchor points that are in this animation's frames.
		int animationId = animationSlot->getAnimationId();

		if (animationId >= 0)
		{
			AnimationPtr animation = animationManager_->getAnimationByIndex(animationId);

			int frameCount = animation->getFrameCount();

			for (int j = 0; j < frameCount; j++)
			{
				AnimationFramePtr frame = animation->getFrame(j);

				int hitboxCount = animationSlot->hitboxReferences_[j].size();

				for (int k = 0; k < hitboxCount; k++)
				{
					int hitboxId = animationSlot->hitboxReferences_[j][k];

					HitboxPtr hitbox = hitboxManager_->getHitbox(hitboxId);

					float hitboxBaseRotation = hitbox->getBaseRotationDegrees();

					// The hitbox coordinates are stored as relative to the animation, but I need to rotate them relative to the stage.
					// Create a new array of points in stage space.
					std::vector<Vertex2> hitboxCornersStageSpace;

					for (int i = 0; i < 4; i++)
					{
						Vertex2 hitboxNativeCorner;

						hitboxNativeCorner.x = animationSlot->nativeCorners_[0].x + hitbox->nativeCorners_[i].x;

						hitboxNativeCorner.y = animationSlot->nativeCorners_[0].y + hitbox->nativeCorners_[i].y;

						hitboxCornersStageSpace.push_back(hitboxNativeCorner);
					}

					// First rotate in line with the animation.
					linearAlgebraUtility_->rotatePoints(slotRotationAngle, hitboxCornersStageSpace, hitbox->transformedCorners_, slotTranslationVectorX, slotTranslationVectorY);

					// Then apply the stage rotation to the hitbox.
					linearAlgebraUtility_->rotatePoints(stageRotationAngle, hitbox->transformedCorners_, hitbox->transformedCorners_, stageTranslationVectorX, stageTranslationVectorY);
				}


				int anchorPointCount = animationSlot->anchorPointReferences_[j].size();

				for (int k = 0; k < anchorPointCount; k++)
				{
					int anchorPointId = animationSlot->anchorPointReferences_[j][k];

					AnchorPointPtr anchorPoint = anchorPointManager_->getAnchorPoint(anchorPointId);
					
					// The anchor point coordinate is stored as relative to the animation, but I need to rotate them relative to the stage.

					// Create a new point in stage space.
					Vertex2 anchorPointStageSpace;

					Vertex2 anchorPointNative;

					anchorPointNative.x = animationSlot->nativeCorners_[0].x + anchorPoint->nativePosition_.x;

					anchorPointNative.y = animationSlot->nativeCorners_[0].y + anchorPoint->nativePosition_.y;
					
					// First rotate in line with the animation.
					linearAlgebraUtility_->rotatePoint(slotRotationAngle, anchorPointNative, anchorPoint->transformedPosition_, slotTranslationVectorX, slotTranslationVectorY);

					// Then apply the stage rotation to the point.
					linearAlgebraUtility_->rotatePoint(stageRotationAngle, anchorPoint->transformedPosition_, anchorPoint->transformedPosition_, stageTranslationVectorX, stageTranslationVectorY);
				}
			}

			// Apply the stage rotation to the animation.
			linearAlgebraUtility_->rotatePoints(stageRotationAngle, animationSlot->transformedCorners_, animationSlot->transformedCorners_, stageTranslationVectorX, stageTranslationVectorY);
		}
	}

	// Rotate stage hitboxes
	int stageHitboxCount = getHitboxReferenceCount();

	for (int i = 0; i < stageHitboxCount; i++)
	{
		int hitboxId = getHitboxReference(i);

		HitboxPtr hitbox = hitboxManager_->getHitbox(hitboxId);

		float hitboxBaseRotation = hitbox->getBaseRotationDegrees();

		linearAlgebraUtility_->rotatePoints(stageRotationAngle + hitboxBaseRotation, hitbox->nativeCorners_, hitbox->transformedCorners_, stageTranslationVectorX, stageTranslationVectorY);
	}
}

void StageElements::applyTransforms(bool mirrorHorizontally, double lerp)
{
	applyRotationTransforms(lerp);

	if (mirrorHorizontally == true)
	{
		applyMirrorHorizontallyTransform();
	}

	setRenderableBoundary();
}
//
//void StageElements::assignAnimationByNameToSlotByNamePy(std::string slotName, std::string animationName)
//{
//	PythonReleaseGil unlocker;
//
//	assignAnimationByNameToSlotByName(slotName, animationName);
//}
//
//void StageElements::assignAnimationByNameToSlotByName(std::string slotName, std::string animationName)
//{
//	// When assigning an animation to a slot, it may need to apply the transforms so that the new animation matches the current stage transforms.
//	// I think the correct way to do this is to make this function callable via the stage controller, so that the stage apply transforms 
//	// function can be called after the animation is assigned.
//	// All end user interactions with the stage should be done via the controller, because that is the highest level in the heirarchy and all
//	// stage functions will be available for any processing that must be done.
//	
//	// Scripts will need to be updated
//	int index = animationManager_->getAnimationId(animationName);
//
//	assignAnimationByIdToSlotByName(slotName, index);
//}
//
//void StageElements::assignAnimationByNameToSlotByNamePy(std::string slotName, std::string animationName, std::string synchWithSlotName)
//{
//	PythonReleaseGil unlocker;
//
//	assignAnimationByNameToSlotByName(slotName, animationName, synchWithSlotName);
//}
//
//void StageElements::assignAnimationByNameToSlotByName(std::string slotName, std::string animationName, std::string synchWithSlotName)
//{
//	int index = animationManager_->getAnimationId(animationName);
//
//	assignAnimationByIdToSlotByName(slotName, index, synchWithSlotName);
//}
//
//void StageElements::assignAnimationByNameToSlotByIndexPy(int slotIndex, std::string animationName)
//{
//	PythonReleaseGil unlocker;
//
//	assignAnimationByNameToSlotByIndex(slotIndex, animationName);
//}
//
//void StageElements::assignAnimationByNameToSlotByIndex(int slotIndex, std::string animationName)
//{
//	int index = animationManager_->getAnimationId(animationName);
//
//	assignAnimationByIdToSlotByIndex(slotIndex, index);
//}
//
//void StageElements::assignAnimationByNameToSlotByIndex(int slotIndex, std::string animationName, std::string synchWithSlotName)
//{
//	int index = animationManager_->getAnimationId(animationName);
//
//	assignAnimationByIdToSlotByIndex(slotIndex, index, synchWithSlotName);
//}
//
//void StageElements::assignAnimationByIdToSlotByName(std::string slotName, int animationId)
//{
//	int slotIndex = getAnimationSlotIndex(slotName);
//
//	assignAnimationByIdToSlotByIndex(slotIndex, animationId);
//}
//
//void StageElements::assignAnimationByIdToSlotByName(std::string slotName, int animationId, std::string synchWithSlotName)
//{
//	int slotIndex = getAnimationSlotIndex(slotName);
//
//	assignAnimationByIdToSlotByIndex(slotIndex, animationId, synchWithSlotName);
//}
//
//void StageElements::assignAnimationByIdToSlotByIndex(int slotIndex, int animationId, std::string synchWithSlotName)
//{
//	int size = animationSlots_.size();
//
//	if (slotIndex >= 0 && slotIndex < size)
//	{
//		int synchWithSlotIndex = getAnimationSlotIndex(synchWithSlotName);
//
//		if (synchWithSlotIndex >= 0)
//		{
//			AnimationPlayerPtr synchWithAnimationPlayer = animationSlots_[synchWithSlotIndex]->animationPlayer_;
//
//			animationSlots_[slotIndex]->animationPlayer_->synch(synchWithAnimationPlayer);
//		}
//
//		assignAnimationByIdToSlotByIndex(slotIndex, animationId);
//	}
//}
//
//void StageElements::assignAnimationByIdToSlotByIndex(int slotIndex, int animationId)
//{
//	assignAnimationByIdToSlotByIndexInternal(slotIndex, animationId);
//}

void StageElements::assignAnimationByIdToSlotByNameInternal(std::string slotName, int animationId, bool copyHitboxes, std::string synchWithSlotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	assignAnimationByIdToSlotByIndexInternal(slotIndex, animationId, copyHitboxes, synchWithSlotName);
}

void StageElements::assignAnimationByIdToSlotByNameInternal(std::string slotName, int animationId, bool copyHitboxes)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	assignAnimationByIdToSlotByIndexInternal(slotIndex, animationId, copyHitboxes);
}

void StageElements::assignAnimationByIdToSlotByIndexInternal(int slotIndex, int animationId, bool copyHitboxes, std::string synchWithSlotName)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		int synchWithSlotIndex = getAnimationSlotIndex(synchWithSlotName);

		if (synchWithSlotIndex >= 0)
		{
			AnimationPlayerPtr synchWithAnimationPlayer = animationSlots_[synchWithSlotIndex]->animationPlayer_;

			animationSlots_[slotIndex]->animationPlayer_->synch(synchWithAnimationPlayer);
		}

		assignAnimationByIdToSlotByIndexInternal(slotIndex, animationId, copyHitboxes);
	}
}

void StageElements::assignAnimationByIdToSlotByIndexInternal(int slotIndex, int animationId, bool copyHitboxes)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		AnimationSlotPtr animationSlot = animationSlots_[slotIndex];

		// POTENTIAL BUG: Assigning an animation to a slot when one already exists, and is currently visible will
		//                probably need to deactivate any hitboxes in that frame.


		animationSlot->oldAnimationId_ = animationSlot->animationId_;

		animationSlot->animationId_ = animationId;

		animationSlot->animation_ = nullptr;

		if (animationId >= 0)
		{
			boost::shared_ptr<Animation> newAnimation = animationManager_->getAnimationByIndex(animationId);

			animationSlot->animation_ = newAnimation;

			int frameCount = newAnimation->getFrameCount();

			int newSheetId = newAnimation->getSpriteSheetId();

			animationSlot->size_->setWidth(0);

			animationSlot->size_->setHeight(0);

			float scaleFactor = 1.0f;

			if (newSheetId >= 0)
			{
				auto newSpriteSheet = renderer_->getSheet(newSheetId);

				animationSlot->size_->setWidth(newSpriteSheet->getCellWidth() * newSpriteSheet->getScaleFactor());

				animationSlot->size_->setHeight(newSpriteSheet->getCellHeight() * newSpriteSheet->getScaleFactor());

				scaleFactor = newSpriteSheet->getScaleFactor();
			}

#if defined(_DEBUG)	

			if (ownerMetadata_ != nullptr && ownerMetadata_->getEntityTypeId() == 695 && animationSlot->animation_->getName() == "StonemasonIdle")
			{
				bool debug = true;
			}

#endif
			animationSlot->setCornerPoints(scaleFactor);

			// An instance of an animation (i.e. an animation that has been assigned to a slot) needs to
			// have its own copy of the animation's hitboxes, otherwise any entity that has this animation
			// assigned to it will be working against the same hitbox object, and thus stepping all over
			// each other.

			// Make copies of the hitboxes here, to be used in the slot, and remove any hitboxes that were 
			// left over from the prevoiusly assigned animation, if any exist.

			// The same is true for anchor points.

			if (copyHitboxes == true)
			{
				animationSlot->hitboxReferences_.clear();

				animationSlot->hitboxReferences_.resize(frameCount);

				// Restart the available hitbox tracker at 0.
				animationSlot->activeHitboxCount_ = 0;



				animationSlot->anchorPointReferences_.clear();

				animationSlot->anchorPointReferences_.resize(frameCount);

				// Restart the available anchor point tracker at 0.
				animationSlot->activeAnchorPointCount_ = 0;


				//slotHitboxEdgeFlags_[slotIndex].clear();

				//slotHitboxEdgeFlags_[slotIndex].resize(frameCount);

				//slotHitboxCollisionStatuses_[slotIndex].clear();

				//slotHitboxCollisionStatuses_[slotIndex].resize(frameCount);

				for (int i = 0; i < frameCount; i++)
				{
					boost::shared_ptr<AnimationFrame> f = newAnimation->getFrame(i);

					int hitboxCount = f->getHitboxCount();

					for (int j = 0; j < hitboxCount; j++)
					{
						int hitboxId = f->getHitboxReference(j);

						boost::shared_ptr<Hitbox> sourceHitbox = hitboxManager_->getHitbox(hitboxId);

						// If a hitbox is available for use, get it. Otherwise create a new one.
						int newHitboxId = animationSlot->getNextAvailableHitboxId();
						
						boost::shared_ptr<Hitbox> newHitbox;

						if (newHitboxId == -1)
						{
							newHitbox = boost::shared_ptr<Hitbox>(new Hitbox(sourceHitbox->getLeft(), sourceHitbox->getTop(), sourceHitbox->getHeight(), sourceHitbox->getWidth()));

							newHitboxId = hitboxManager_->addHitbox(newHitbox);

							animationSlot->availableHitboxIds_.push_back(newHitboxId);

							animationSlot->activeHitboxCount_ = animationSlot->availableHitboxIds_.size();
						}
						else
						{
							newHitbox = hitboxManager_->getHitbox(newHitboxId);

							newHitbox->resize(sourceHitbox->getLeft(), sourceHitbox->getTop(), sourceHitbox->getHeight(), sourceHitbox->getWidth());
						}

						// Set the hitbox hitbox properties based on the hitbox data from the animation frame definition.
						newHitbox->setIdentity(sourceHitbox->getIdentity());

						newHitbox->setIsSolid(sourceHitbox->getIsSolid());

						newHitbox->setBaseRotationDegrees(sourceHitbox->getBaseRotationDegrees());

						newHitbox->stageMetadata_ = stageMetadata_;

						animationSlot->hitboxReferences_[i].push_back(newHitboxId);

						//slotHitboxEdgeFlags_[slotIndex][i].push_back(0xFF);

						//slotHitboxCollisionStatuses_[slotIndex][i].push_back(false);
					}

					int anchorPointCount = f->getAnchorPointCount();

					for (int j = 0; j < anchorPointCount; j++)
					{
						int anchorPointId = f->getAnchorPointReference(j);

						AnchorPointPtr sourceAnchorPoint = anchorPointManager_->getAnchorPoint(anchorPointId);

						// If an anchor point is available for use, get it. Otherwise create a new one.
						int newAnchorPointId = animationSlot->getNextAvailableAnchorPointId();

						AnchorPointPtr newAnchorPoint;

						if (newAnchorPointId == -1)
						{
							newAnchorPoint = AnchorPointPtr(new AnchorPoint(sourceAnchorPoint->getName(), sourceAnchorPoint->getX(), sourceAnchorPoint->getY()));

							newAnchorPointId = anchorPointManager_->addAnchorPoint(newAnchorPoint);

							animationSlot->availableAnchorPointIds_.push_back(newAnchorPointId);

							animationSlot->activeAnchorPointCount_ = animationSlot->availableAnchorPointIds_.size();
						}
						else
						{
							newAnchorPoint = anchorPointManager_->getAnchorPoint(newAnchorPointId);

							newAnchorPoint->initialize(sourceAnchorPoint->getName(), sourceAnchorPoint->getX(), sourceAnchorPoint->getY());
						}

						animationSlot->anchorPointReferences_[i].push_back(newAnchorPointId);
					}
				}
			}
		}
		else
		{
			// Clear corner points.			
		}

		setRenderableBoundary();
	}
}

float StageElements::interpolateAngle(float angle, float previousAngle, float previousPreviousAngle, double lerp)
{
	// Determine if the angle has wrapped around.

	//// If previousAngle < angle, 
	//bool clockwiseChange = (previousPreviousAngle - previousAngle) > 1;

	//if (previousAngle < angle) previousPreviousAngle > previousAngle && )
	//{
	//	// Wrapped around clockwise. Subtract 360 from the angle.
	//	std::cout << "Switch 1" << std::endl;
	//	std::cout << previousPreviousAngle << " " << previousAngle << " " << angle << std::endl;
	//	angle -= 360;
	//}
	//else if (previousPreviousAngle < previousAngle && previousAngle > angle)
	//{
	//	// Wrapped around counter clockwise. Add 360 to the angle.
	//	std::cout << "Switch 2" << std::endl;
	//	std::cout << previousPreviousAngle << " " << previousAngle << " " << angle << std::endl;
	//	angle += 360;
	//}
	
	// Need a better way. For now, just return the angle.
	return angle;

	float interpolatedAngle = previousAngle + (lerp * (angle - previousAngle));

	if (interpolatedAngle != 0)
	{
		std::cout << interpolatedAngle << std::endl;
	}

	return interpolatedAngle;
}

AnchorPointPtr StageElements::getAnchorPointByIndexFromSlotByIndex(int slotIndex, int anchorPointIndex)
{
	int animationId = getAnimationIdByIndex(slotIndex);

	if (animationId > -1)
	{
		AnimationSlotPtr animationSlot = getAnimationSlotByIndex(slotIndex);

		AnimationPlayerPtr animationPlayer = animationSlot->getAnimationPlayer();

		boost::shared_ptr<Animation> currentAnimation = animationManager_->getAnimationByIndex(animationId);

		int frameIndex = animationPlayer->getCurrentFrame();

		if (frameIndex > -1)
		{
			if (anchorPointIndex <= animationSlot->anchorPointReferences_[frameIndex].size())
			{
				int anchorPointId = animationSlot->anchorPointReferences_[frameIndex][anchorPointIndex];

				AnchorPointPtr ap = anchorPointManager_->getAnchorPoint(anchorPointId);

				returnedAnchorPoint_->name_ = ap->getName();

				returnedAnchorPoint_->point_->setX(ap->transformedPosition_.x + stageMetadata_->getPosition()->getX());

				returnedAnchorPoint_->point_->setY(ap->transformedPosition_.y + stageMetadata_->getPosition()->getY());
			}

			return returnedAnchorPoint_;
		}
	}
	else
	{
		if (debugger_->getDebugMode() == true)
		{
			std::cout << "Error: No animation in slot " << slotIndex << std::endl;
		}
	}

	returnedAnchorPoint_->name_ = "Anchor Point Not Found";
	returnedAnchorPoint_->point_->setX(0);
	returnedAnchorPoint_->point_->setY(0);

	return returnedAnchorPoint_;
}

AnchorPointPtr StageElements::getAnchorPointByIndexFromSlotByName(std::string slotName, int anchorPointIndex)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnchorPointByIndexFromSlotByIndex(slotIndex, anchorPointIndex);
}

AnchorPointPtr StageElements::getAnchorPointByNameFromSlotByIndex(int slotIndex, std::string anchorPointName)
{
	int animationId = getAnimationIdByIndex(slotIndex);

	if (animationId > -1)
	{
		AnimationSlotPtr animationSlot = getAnimationSlotByIndex(slotIndex);

		AnimationPlayerPtr animationPlayer = animationSlot->getAnimationPlayer();

		int frameIndex = animationPlayer->getCurrentFrame();
		
		int anchorPointCount = animationSlot->anchorPointReferences_[frameIndex].size();

		for (int i = 0; i < anchorPointCount; i++)
		{
			int anchorPointId = animationSlot->anchorPointReferences_[frameIndex][i];

			AnchorPointPtr ap = anchorPointManager_->getAnchorPoint(anchorPointId);

			if (ap->getName() == anchorPointName)
			{
				returnedAnchorPoint_->name_ = ap->getName();

				returnedAnchorPoint_->point_->setX(ap->transformedPosition_.x + stageMetadata_->getPosition()->getX());

				returnedAnchorPoint_->point_->setY(ap->transformedPosition_.y + stageMetadata_->getPosition()->getY());

				return returnedAnchorPoint_;
			}
		}
	}
	else
	{
		if (debugger_->getDebugMode() == true)
		{
			std::cout << "Error: No animation in slot " << slotIndex << std::endl;
		}
	}

	returnedAnchorPoint_->name_ = "Anchor Point Not Found";
	returnedAnchorPoint_->point_->setX(0);
	returnedAnchorPoint_->point_->setY(0);

	return returnedAnchorPoint_;
}

AnchorPointPtr StageElements::getAnchorPointByNameFromSlotByNamePy(std::string slotName, std::string anchorPointName)
{
	PythonReleaseGil unlocker;

	return getAnchorPointByNameFromSlotByName(slotName, anchorPointName);
}

AnchorPointPtr StageElements::getAnchorPointByNameFromSlotByName(std::string slotName, std::string anchorPointName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnchorPointByNameFromSlotByIndex(slotIndex, anchorPointName);
}

int StageElements::getAnimationIdByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationIdByName(slotName);
}

int StageElements::getAnimationIdByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationIdByIndex(slotIndex);
}

int StageElements::getAnimationIdByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationIdByIndex(slotIndex);
}

int StageElements::getAnimationIdByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->animationId_;
	}
	else
	{
		return -1;
	}
}

AnimationPlayerPtr StageElements::getAnimationPlayerByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationPlayerByName(slotName);
}

AnimationPlayerPtr StageElements::getAnimationPlayerByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationPlayerByIndex(slotIndex);
}

AnimationPlayerPtr StageElements::getAnimationPlayerByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationPlayerByIndex(slotIndex);
}

AnimationPlayerPtr StageElements::getAnimationPlayerByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->animationPlayer_;
	}
	else
	{
		return nullptr;
	}
}

AnimationSlotPtr StageElements::getAnimationSlotByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotByName(slotName);
}

AnimationSlotPtr StageElements::getAnimationSlotByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotByIndex(slotIndex);
}

AnimationSlotPtr StageElements::getAnimationSlotByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotByIndex(slotIndex);
}

AnimationSlotPtr StageElements::getAnimationSlotByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex];
	}
	else
	{
		return nullptr;
	}
}

AlphaGradientDirection StageElements::getAnimationSlotAlphaGradientDirectionByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotAlphaGradientDirectionByName(slotName);
}

AlphaGradientDirection StageElements::getAnimationSlotAlphaGradientDirectionByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotAlphaGradientDirectionByIndex(slotIndex);
}

AlphaGradientDirection StageElements::getAnimationSlotAlphaGradientDirectionByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getAlphaGradientDirection();
	}
	else
	{
		return ALPHA_GRADIENT_NONE;
	}
}

float StageElements::getAnimationSlotAlphaGradientFromByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotAlphaGradientFromByName(slotName);
}

float StageElements::getAnimationSlotAlphaGradientFromByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotAlphaGradientFromByIndex(slotIndex);
}

float StageElements::getAnimationSlotAlphaGradientFromByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getAlphaGradientFrom();
	}
	else
	{
		return 0.0;
	}
}

float StageElements::getAnimationSlotAlphaGradientRadiusByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotAlphaGradientRadiusByName(slotName);
}

float StageElements::getAnimationSlotAlphaGradientRadiusByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotAlphaGradientRadiusByIndex(slotIndex);
}

int	StageElements::getAnimationSlotAlphaGradientRadialCenterXByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotAlphaGradientRadialCenterXByName(slotName);
}

int	StageElements::getAnimationSlotAlphaGradientRadialCenterXByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotAlphaGradientRadialCenterXByIndex(slotIndex);
}

int	StageElements::getAnimationSlotAlphaGradientRadialCenterXByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getAlphaGradientRadialCenterPoint()->getX();
	}
	else
	{
		return -1;
	}
}

int	StageElements::getAnimationSlotAlphaGradientRadialCenterYByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotAlphaGradientRadialCenterYByName(slotName);
}

int	StageElements::getAnimationSlotAlphaGradientRadialCenterYByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotAlphaGradientRadialCenterYByIndex(slotIndex);
}

int	StageElements::getAnimationSlotAlphaGradientRadialCenterYByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getAlphaGradientRadialCenterPoint()->getY();
	}
	else
	{
		return -1;
	}
}

float StageElements::getAnimationSlotAlphaGradientRadiusByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getAlphaGradientRadius();
	}
	else
	{
		return 0.0;
	}
}

float StageElements::getAnimationSlotAlphaGradientToByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotAlphaGradientToByName(slotName);
}

float StageElements::getAnimationSlotAlphaGradientToByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotAlphaGradientToByIndex(slotIndex);
}

float StageElements::getAnimationSlotAlphaGradientToByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getAlphaGradientTo();
	}
	else
	{
		return 0.0;
	}
}

AnimationStyle StageElements::getAnimationSlotAnimationStyleByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotAnimationStyleByName(slotName);
}

AnimationStyle StageElements::getAnimationSlotAnimationStyleByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotAnimationStyleByIndex(slotIndex);
}

AnimationStyle StageElements::getAnimationSlotAnimationStyleByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->animationPlayer_->animationStyle_;
	}
	else
	{
		return ANIMATION_STYLE_REPEAT;
	}
}

int StageElements::getAnimationSlotCountPy()
{
	PythonReleaseGil unlocker;

	return getAnimationSlotCount();
}

int StageElements::getAnimationSlotCount()
{
	return animationSlots_.size();
}

int StageElements::getAnimationSlotFramesPerSecondByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotFramesPerSecondByName(slotName);
}

int StageElements::getAnimationSlotFramesPerSecondByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotFramesPerSecondByIndex(slotIndex);
}

int StageElements::getAnimationSlotFramesPerSecondByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->animationPlayer_->getFramesPerSecond();
	}
	else
	{
		return 60;
	}
}

int	StageElements::getAnimationSlotIndexPy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotIndex(slotName);
}

int StageElements::getAnimationSlotIndex(std::string slotName)
{
	int size = animationSlots_.size();

	for (int i = 0; i < size; i++)
	{
		if (animationSlots_[i]->name_ == slotName)
		{
			return i;
		}
	}

	return -1;
}

std::string	StageElements::getAnimationSlotNamePy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotName(slotIndex);
}

std::string	StageElements::getAnimationSlotName(int index)
{
	int size = animationSlots_.size();

	if (index >= 0 && index < size)
	{
		return animationSlots_[index]->name_;
	}
	else
	{
		return "";
	}
}


// Next State Name
std::string	StageElements::getAnimationSlotNextStateNameByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotNextStateNameByIndex(slotIndex);
}

std::string	StageElements::getAnimationSlotNextStateNameByIndex(int index)
{
	int size = animationSlots_.size();

	if (index >= 0 && index < size)
	{
		return animationSlots_[index]->nextStateName_;
	}
	else
	{
		return "";
	}
}

AnimationSlotOrigin StageElements::getAnimationSlotOriginByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->origin_;
	}
	else
	{
		return ANIMATION_SLOT_ORIGIN_TOP_LEFT;
	}
}

int	StageElements::getAnimationSlotPivotPointXByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotPivotPointXByName(slotName);
}

int	StageElements::getAnimationSlotPivotPointXByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotPivotPointXByIndex(slotIndex);
}

int	StageElements::getAnimationSlotPivotPointXByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getRotationOperation(0)->getPivotPoint()->getX();
	}
	else
	{
		return -1;
	}
}

int	StageElements::getAnimationSlotPivotPointYByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotPivotPointYByName(slotName);
}

int	StageElements::getAnimationSlotPivotPointYByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotPivotPointYByIndex(slotIndex);
}

int	StageElements::getAnimationSlotPivotPointYByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getRotationOperation(0)->getPivotPoint()->getY();
	}
	else
	{
		return -1;
	}
}

int	StageElements::getAnimationSlotPositionYByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotPositionYByName(slotName);
}

int	StageElements::getAnimationSlotPositionYByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotPositionYByIndex(slotIndex);
}

int	StageElements::getAnimationSlotPositionYByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotPositionYByIndex(slotIndex);
}

int	StageElements::getAnimationSlotPositionYByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		AnimationSlotPtr animationSlot = animationSlots_[slotIndex];

		int y = animationSlot->nativePosition_->getY();

		int stageOffsetY = 0;

		int animationOffsetY = 0;

		switch (stageMetadata_->getOrigin())
		{
		case STAGE_ORIGIN_CENTER:
		case STAGE_ORIGIN_BOTTOM_MIDDLE:

			int stageHeight = stageMetadata_->getHeight();

			stageOffsetY = (int)(stageHeight / 2);

			break;
		}

		switch (animationSlot->origin_)
		{
		case ANIMATION_SLOT_ORIGIN_CENTER:
		case ANIMATION_SLOT_ORIGIN_BOTTOM_MIDDLE:

			if (animationSlot->animation_ != nullptr)
			{
				auto sheet = renderer_->getSheet(animationSlot->animation_->getSpriteSheetId());

				if (sheet != nullptr)
				{
					int cellHeight = sheet->getCellHeight();

					float scaleFactor = sheet->getScaleFactor();

					animationOffsetY = (int)((cellHeight * scaleFactor) / 2);
				}
			}

			break;
		}

		int transformedY = y - stageOffsetY + animationOffsetY;

		return transformedY;
	}
	else
	{
		return -1;
	}
}

RenderEffectsPtr StageElements::getAnimationSlotRenderEffectsByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotRenderEffectsByIndex(slotIndex);
}

RenderEffectsPtr StageElements::getAnimationSlotRenderEffectsByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_;
	}
	else
	{
		return nullptr;
	}
}

ColorRgbaPtr StageElements::getAnimationSlotBlendColorByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotBlendColorByName(slotName);
}

ColorRgbaPtr StageElements::getAnimationSlotBlendColorByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotBlendColorByIndex(slotIndex);
}

ColorRgbaPtr StageElements::getAnimationSlotBlendColorByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotBlendColorByIndex(slotIndex);
}

ColorRgbaPtr StageElements::getAnimationSlotBlendColorByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getBlendColor();
	}
	else
	{
		return nullptr;
	}
}

float StageElements::getAnimationSlotBlendPercentByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotBlendPercentByName(slotName);
}

float StageElements::getAnimationSlotBlendPercentByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotBlendPercentByIndex(slotIndex);
}

float StageElements::getAnimationSlotBlendPercentByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotBlendPercentByIndex(slotIndex);
}

float StageElements::getAnimationSlotBlendPercentByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getBlendPercent();
	}
	else
	{
		return 0.0f;
	}
}

ColorRgbaPtr StageElements::getAnimationSlotHueColorByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotHueColorByName(slotName);
}

ColorRgbaPtr StageElements::getAnimationSlotHueColorByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotHueColorByIndex(slotIndex);
}

ColorRgbaPtr StageElements::getAnimationSlotHueColorByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotHueColorByIndex(slotIndex);
}

ColorRgbaPtr StageElements::getAnimationSlotHueColorByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getHueColor();
	}
	else
	{
		return nullptr;
	}
}

ColorRgbaPtr StageElements::getAnimationSlotOutlineColorByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotOutlineColorByName(slotName);
}

ColorRgbaPtr StageElements::getAnimationSlotOutlineColorByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotOutlineColorByIndex(slotIndex);
}

ColorRgbaPtr StageElements::getAnimationSlotOutlineColorByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotOutlineColorByIndex(slotIndex);
}

ColorRgbaPtr StageElements::getAnimationSlotOutlineColorByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getOutlineColor();
	}
	else
	{
		return nullptr;
	}
}

float StageElements::getAnimationSlotRotationByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotRotationByName(slotName);
}

float StageElements::getAnimationSlotRotationByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotRotationByIndex(slotIndex);
}

float StageElements::getAnimationSlotRotationByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->renderEffects_->getRotationOperation(0)->getAngle();
	}
	else
	{
		return 0.0;
	}
}

int	StageElements::getAnimationSlotPositionXByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotPositionXByName(slotName);
}

int	StageElements::getAnimationSlotPositionXByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return getAnimationSlotPositionXByIndex(slotIndex);
}

int	StageElements::getAnimationSlotPositionXByIndexPy(int slotIndex)
{
	PythonReleaseGil unlocker;

	return getAnimationSlotPositionXByIndex(slotIndex);
}

int	StageElements::getAnimationSlotPositionXByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		AnimationSlotPtr animationSlot = animationSlots_[slotIndex];

		int x = animationSlot->nativePosition_->getX();

		int stageOffsetX = 0;

		int animationOffsetX = 0;

		switch (stageMetadata_->getOrigin())
		{
		case STAGE_ORIGIN_CENTER:
		case STAGE_ORIGIN_TOP_MIDDLE:
		case STAGE_ORIGIN_BOTTOM_MIDDLE:

			int stageWidth = stageMetadata_->getWidth();

			stageOffsetX = (int)(stageWidth / 2);

			break;

		}

		switch (animationSlot->origin_)
		{
		case ANIMATION_SLOT_ORIGIN_CENTER:
		case ANIMATION_SLOT_ORIGIN_TOP_MIDDLE:
		case ANIMATION_SLOT_ORIGIN_BOTTOM_MIDDLE:

			if (animationSlot->animation_ != nullptr)
			{
				auto sheet = renderer_->getSheet(animationSlot->animation_->getSpriteSheetId());

				if (sheet != nullptr)
				{
					int cellWidth = sheet->getCellWidth();

					float scaleFactor = sheet->getScaleFactor();

					animationOffsetX = (int)((cellWidth * scaleFactor) / 2);
				}
			}

			break;
		}

		int transformedX = x - stageOffsetX + animationOffsetX;

		return transformedX;
	}
	else
	{
		return -1;
	}
}

//bool StageElements::getHitboxCollisionStatus(int index)
//{
//	int size = hitboxCollisionStatuses_.size();
//
//	if (index >= 0 && index < size)
//	{
//		return hitboxCollisionStatuses_[index];
//	}
//
//	return false;
//}
//
//unsigned char StageElements::getHitboxEdgeFlags(int index)
//{
//	int size = hitboxEdgeFlags_.size();
//
//	if (index >= 0 && index < size)
//	{
//		return hitboxEdgeFlags_[index];
//	}
//
//	return 0;
//}

int StageElements::getHitboxReferenceCountPy()
{
	PythonReleaseGil unlocker;

	return getHitboxReferenceCount();
}

int StageElements::getHitboxReferenceCount()
{
	return hitboxReferences_.size();
}

int StageElements::getHitboxReferencePy(int index)
{
	PythonReleaseGil unlocker;

	return getHitboxReference(index);
}

int StageElements::getHitboxReference(int index)
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

ColorRgbaPtr StageElements::getHueColorPy()
{
	PythonReleaseGil unlocker;

	return getHueColor();
}

ColorRgbaPtr StageElements::getHueColor()
{
	return stageRenderEffects_->getHueColor();
}

bool StageElements::getAnimationSlotBackgroundByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->isBackground_;
	}
	else
	{
		return false;
	}
}

int	StageElements::getNativeAnimationSlotPositionXByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		int x = animationSlots_[slotIndex]->nativePosition_->getX();

		return x;
	}
	else
	{
		return -1;
	}
}

bool StageElements::getSingleFrame()
{
	return singleFrame_;
}

int	StageElements::getNativeAnimationSlotPositionYByIndex(int slotIndex)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		return animationSlots_[slotIndex]->nativePosition_->getY();
	}
	else
	{
		return -1;
	}
}
//
//bool StageElements::getSlotHitboxCollisionStatusByName(std::string slotName, int frameIndex, int hitboxIndex)
//{
//	int slotIndex = getAnimationSlotIndex(slotName);
//
//	return getSlotHitboxCollisionStatusByIndex(slotIndex, frameIndex, hitboxIndex);
//}
//
//bool StageElements::getSlotHitboxCollisionStatusByIndex(int slotIndex, int frameIndex, int hitboxIndex)
//{
//	int slotCount = slotHitboxCollisionStatuses_.size();
//
//	if (slotIndex >= 0 && slotIndex < slotCount)
//	{
//		int frameCount = slotHitboxCollisionStatuses_[slotIndex].size();
//
//		if (frameIndex >= 0 && frameIndex < frameCount)
//		{
//			int hitboxReferenceCount = slotHitboxCollisionStatuses_[slotIndex][frameIndex].size();
//
//			if (hitboxIndex >= 0 && hitboxIndex < hitboxReferenceCount)
//			{
//				return slotHitboxCollisionStatuses_[slotIndex][frameIndex][hitboxIndex];
//			}
//		}
//	}
//
//	return false;
//}

//unsigned char StageElements::getSlotHitboxEdgeFlagsByName(std::string slotName, int frameIndex, int hitboxIndex)
//{
//	int slotIndex = getAnimationSlotIndex(slotName);
//
//	return getSlotHitboxEdgeFlagsByIndex(slotIndex, frameIndex, hitboxIndex);
//}
//
//unsigned char StageElements::getSlotHitboxEdgeFlagsByIndex(int slotIndex, int frameIndex, int hitboxIndex)
//{
//	int slotCount = slotHitboxEdgeFlags_.size();
//
//	if (slotIndex >= 0 && slotIndex < slotCount)
//	{
//		int frameCount = slotHitboxEdgeFlags_[slotIndex].size();
//
//		if (frameIndex >= 0 && frameIndex < frameCount)
//		{
//			int hitboxReferenceCount = slotHitboxEdgeFlags_[slotIndex][frameIndex].size();
//
//			if (hitboxIndex >= 0 && hitboxIndex < hitboxReferenceCount)
//			{
//				return slotHitboxEdgeFlags_[slotIndex][frameIndex][hitboxIndex];
//			}
//		}
//	}
//
//	return -1;
//}

//int	StageElements::getSlotHitboxReferenceByNamePy(std::string slotName, int frameIndex, int hitboxIndex)
//{
//	PythonReleaseGil unlocker;
//
//	return getSlotHitboxReferenceByName(slotName, frameIndex, hitboxIndex);
//}
//
//int	StageElements::getSlotHitboxReferenceByName(std::string slotName, int frameIndex, int hitboxIndex)
//{
//	int slotIndex = getAnimationSlotIndex(slotName);
//
//	return getSlotHitboxReferenceByIndex(slotIndex, frameIndex, hitboxIndex);
//}
//
//int	StageElements::getSlotHitboxReferenceByIndexPy(int slotIndex, int frameIndex, int hitboxIndex)
//{
//	PythonReleaseGil unlocker;
//
//	return getSlotHitboxReferenceByIndex(slotIndex, frameIndex, hitboxIndex);
//}
//
//int	StageElements::getSlotHitboxReferenceByIndex(int slotIndex, int frameIndex, int hitboxIndex)
//{
//	int slotCount = slotHitboxReferences_.size();
//
//	if (slotIndex >= 0 && slotIndex < slotCount)
//	{
//		int frameCount = slotHitboxReferences_[slotIndex].size();
//
//		if (frameIndex >= 0 && frameIndex < frameCount)
//		{
//			int hitboxCount = slotHitboxReferences_[slotIndex][frameIndex].size();
//
//			if (hitboxIndex >= 0 && hitboxIndex < hitboxCount)
//			{
//				return slotHitboxReferences_[slotIndex][frameIndex][hitboxIndex];
//			}
//		}
//	}
//
//	return -1;
//}

//int StageElements::getSlotHitboxReferenceCountByName(std::string slotName)
//{
//	int slotIndex = getAnimationSlotIndex(slotName);
//
//	return getSlotHitboxReferenceCountByIndex(slotIndex);
//}
//
//int StageElements::getSlotHitboxReferenceCountByIndex(int slotIndex)
//{
//	int size = animationSlots_.size();
//
//	if (slotIndex >= 0 && slotIndex < size)
//	{
//		return slotHitboxReferences_[slotIndex].size();
//	}
//
//	return 0;
//}

void StageElements::removeAnimationSlotByNamePy(std::string slotName)
{
	PythonReleaseGil unlocker;

	return removeAnimationSlotByName(slotName);
}

void StageElements::removeAnimationSlotByName(std::string slotName)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	removeAnimationSlotByIndex(slotIndex);
}

void StageElements::removeAnimationSlotByIndex(int slotIndex)
{
	animationSlots_.erase(animationSlots_.begin() + slotIndex);
}

void StageElements::removeHitboxReferenceByIndexPy(int index)
{
	PythonReleaseGil unlocker;

	return removeHitboxReferenceByIndex(index);
}

void StageElements::removeHitboxReferenceByIndex(int index)
{
	int size = hitboxReferences_.size();

	if (index > -1 && index < size)
	{
		hitboxReferences_.erase(hitboxReferences_.begin() + index);

		//hitboxEdgeFlags_.erase(hitboxEdgeFlags_.begin() + index);

		//hitboxCollisionStatuses_.erase(hitboxCollisionStatuses_.begin() + index);
	}
}

void StageElements::setAnimationSlotAlphaGradientRadiusByNamePy(std::string slotName, float radius)
{
	PythonReleaseGil unlocker;

	setAnimationSlotAlphaGradientRadiusByName(slotName, radius);
}

void StageElements::setAnimationSlotAlphaGradientRadiusByName(std::string slotName, float radius)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotAlphaGradientRadiusByIndex(slotIndex, radius);
}

void StageElements::setAnimationSlotAlphaGradientRadiusByIndex(int slotIndex, float radius)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setAlphaGradientRadius(radius);
	}
}

void StageElements::setAnimationSlotPositionByNamePy(std::string slotName, int x, int y)
{
	PythonReleaseGil unlocker;

	setAnimationSlotPositionByName(slotName, x, y);
}

void StageElements::setAnimationSlotPositionByName(std::string slotName, int x, int y)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotPositionByIndex(slotIndex, x, y);
}

void StageElements::setAnimationSlotPositionByIndex(int slotIndex, int x, int y)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		AnimationSlotPtr animationSlot = animationSlots_[slotIndex];

		int stageOffsetX = 0;

		int animationOffsetX = 0;

		switch (stageMetadata_->getOrigin())
		{
		case STAGE_ORIGIN_CENTER:
		case STAGE_ORIGIN_TOP_MIDDLE:
		case STAGE_ORIGIN_BOTTOM_MIDDLE:

			int stageWidth = stageMetadata_->getWidth();

			stageOffsetX = (int)(stageWidth / 2);

			break;

		}

		switch (animationSlot->origin_)
		{
		case ANIMATION_SLOT_ORIGIN_CENTER:
		case ANIMATION_SLOT_ORIGIN_TOP_MIDDLE:
		case ANIMATION_SLOT_ORIGIN_BOTTOM_MIDDLE:

			if (animationSlot->animation_ != nullptr)
			{
				auto sheet = renderer_->getSheet(animationSlot->animation_->getSpriteSheetId());

				if (sheet != nullptr)
				{
					int cellWidth = sheet->getCellWidth();

					float scaleFactor = sheet->getScaleFactor();

					animationOffsetX = (int)((cellWidth * scaleFactor) / 2);
				}
			}

			break;
		}

		int transformedX = x + stageOffsetX - animationOffsetX;

		int stageOffsetY = 0;

		int animationOffsetY = 0;

		switch (stageMetadata_->getOrigin())
		{
		case STAGE_ORIGIN_CENTER:
		case STAGE_ORIGIN_BOTTOM_MIDDLE:

			int stageHeight = stageMetadata_->getHeight();

			stageOffsetY = (int)(stageHeight / 2);

			break;
		}

		switch (animationSlot->origin_)
		{
		case ANIMATION_SLOT_ORIGIN_CENTER:
		case ANIMATION_SLOT_ORIGIN_BOTTOM_MIDDLE:

			if (animationSlot->animation_ != nullptr)
			{
				auto sheet = renderer_->getSheet(animationSlot->animation_->getSpriteSheetId());

				if (sheet != nullptr)
				{
					int cellHeight = sheet->getCellHeight();

					float scaleFactor = sheet->getScaleFactor();

					animationOffsetY = (int)((cellHeight * scaleFactor) / 2);
				}
			}

			break;
		}

		int transformedY = y + stageOffsetY - animationOffsetY;

		animationSlots_[slotIndex]->nativePosition_->setX(transformedX);

		animationSlots_[slotIndex]->nativePosition_->setY(transformedY);
	}
}

void StageElements::setAnimationSlotBlendColorByNamePy(std::string slotName, float r, float g, float b, float a)
{
	PythonReleaseGil unlocker;

	setAnimationSlotBlendColorByName(slotName, r, g, b, a);
}

void StageElements::setAnimationSlotBlendColorByName(std::string slotName, float r, float g, float b, float a)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotBlendColorByIndex(slotIndex, r, g, b, a);
}

void StageElements::setAnimationSlotBlendColorByIndexPy(int slotIndex, float r, float g, float b, float a)
{
	PythonReleaseGil unlocker;

	setAnimationSlotBlendColorByIndex(slotIndex, r, g, b, a);
}

void StageElements::setAnimationSlotBlendColorByIndex(int slotIndex, float r, float g, float b, float a)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->getBlendColor()->setR(r);
		animationSlots_[slotIndex]->renderEffects_->getBlendColor()->setG(g);
		animationSlots_[slotIndex]->renderEffects_->getBlendColor()->setB(b);
		animationSlots_[slotIndex]->renderEffects_->getBlendColor()->setA(a);
	}
}

void StageElements::setAnimationSlotBlendPercentByNamePy(std::string slotName, float percent)
{
	PythonReleaseGil unlocker;

	setAnimationSlotBlendPercentByName(slotName, percent);
}

void StageElements::setAnimationSlotBlendPercentByName(std::string slotName, float percent)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotBlendPercentByIndex(slotIndex, percent);
}

void StageElements::setAnimationSlotBlendPercentByIndexPy(int slotIndex, float percent)
{
	PythonReleaseGil unlocker;

	setAnimationSlotBlendPercentByIndex(slotIndex, percent);
}

void StageElements::setAnimationSlotBlendPercentByIndex(int slotIndex, float percent)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setBlendPercent(percent);
	}
}

void StageElements::setAnimationSlotExtentLeftByNamePy(std::string slotName, float extentLeft)
{
	PythonReleaseGil unlocker;

	setAnimationSlotExtentLeftByName(slotName, extentLeft);
}

void StageElements::setAnimationSlotExtentLeftByName(std::string slotName, float extentLeft)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	setAnimationSlotExtentLeftByIndex(slotIndex, extentLeft);
}

void StageElements::setAnimationSlotExtentLeftByIndex(int slotIndex, float extentLeft)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setExtentLeft(extentLeft);
	}
}

void StageElements::setAnimationSlotExtentTopByNamePy(std::string slotName, float extentTop)
{
	PythonReleaseGil unlocker;

	setAnimationSlotExtentTopByName(slotName, extentTop);
}

void StageElements::setAnimationSlotExtentTopByName(std::string slotName, float extentTop)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	setAnimationSlotExtentTopByIndex(slotIndex, extentTop);
}

void StageElements::setAnimationSlotExtentTopByIndex(int slotIndex, float extentTop)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setExtentTop(extentTop);
	}
}

void StageElements::setAnimationSlotExtentRightByNamePy(std::string slotName, float extentRight)
{
	PythonReleaseGil unlocker;

	setAnimationSlotExtentRightByName(slotName, extentRight);
}

void StageElements::setAnimationSlotExtentRightByName(std::string slotName, float extentRight)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	setAnimationSlotExtentRightByIndex(slotIndex, extentRight);
}

void StageElements::setAnimationSlotExtentRightByIndex(int slotIndex, float extentRight)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setExtentRight(extentRight);
	}
}

void StageElements::setAnimationSlotExtentBottomByNamePy(std::string slotName, float extentBottom)
{
	PythonReleaseGil unlocker;

	setAnimationSlotExtentBottomByName(slotName, extentBottom);
}

void StageElements::setAnimationSlotExtentBottomByName(std::string slotName, float extentBottom)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	setAnimationSlotExtentBottomByIndex(slotIndex, extentBottom);
}

void StageElements::setAnimationSlotExtentBottomByIndex(int slotIndex, float extentBottom)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setExtentBottom(extentBottom);
	}
}

void StageElements::setAnimationSlotHueColorByNamePy(std::string slotName, float r, float g, float b, float a)
{
	PythonReleaseGil unlocker;

	setAnimationSlotHueColorByName(slotName, r, g, b, a);
}

void StageElements::setAnimationSlotHueColorByName(std::string slotName, float r, float g, float b, float a)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotHueColorByIndex(slotIndex, r, g, b, a);
}

void StageElements::setAnimationSlotHueColorByIndexPy(int slotIndex, float r, float g, float b, float a)
{
	PythonReleaseGil unlocker;

	setAnimationSlotHueColorByIndex(slotIndex, r, g, b, a);
}

void StageElements::setAnimationSlotHueColorByIndex(int slotIndex, float r, float g, float b, float a)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->getHueColor()->setR(r);
		animationSlots_[slotIndex]->renderEffects_->getHueColor()->setG(g);
		animationSlots_[slotIndex]->renderEffects_->getHueColor()->setB(b);
		animationSlots_[slotIndex]->renderEffects_->getHueColor()->setA(a);
	}
}

void StageElements::setAnimationSlotFramesPerSecondByNamePy(std::string slotName, int framesPerSecond)
{
	PythonReleaseGil unlocker;

	setAnimationSlotFramesPerSecondByName(slotName, framesPerSecond);
}

void StageElements::setAnimationSlotFramesPerSecondByName(std::string slotName, int framesPerSecond)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotFramesPerSecondByIndex(slotIndex, framesPerSecond);
}

void StageElements::setAnimationSlotFramesPerSecondByIndex(int slotIndex, int framesPerSecond)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->animationPlayer_->setFramesPerSecond(framesPerSecond);
	}
}


void StageElements::setAnimationSlotOutlineColorByNamePy(std::string slotName, float r, float g, float b, float a)
{
	PythonReleaseGil unlocker;

	setAnimationSlotOutlineColorByName(slotName, r, g, b, a);
}

void StageElements::setAnimationSlotOutlineColorByName(std::string slotName, float r, float g, float b, float a)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotOutlineColorByIndex(slotIndex, r, g, b, a);
}

void StageElements::setAnimationSlotOutlineColorByIndexPy(int slotIndex, float r, float g, float b, float a)
{
	PythonReleaseGil unlocker;

	setAnimationSlotOutlineColorByIndex(slotIndex, r, g, b, a);
}

void StageElements::setAnimationSlotOutlineColorByIndex(int slotIndex, float r, float g, float b, float a)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->getOutlineColor()->setR(r);
		animationSlots_[slotIndex]->renderEffects_->getOutlineColor()->setG(g);
		animationSlots_[slotIndex]->renderEffects_->getOutlineColor()->setB(b);
		animationSlots_[slotIndex]->renderEffects_->getOutlineColor()->setA(a);
	}
}


std::string StageElements::getAssociatedStateNamePy()
{
	PythonReleaseGil unlocker;

	return getAssociatedStateName();
}

std::string StageElements::getAssociatedStateName()
{
	return associatedStateName_;
}

boost::shared_ptr<Position> StageElements::getPivotPointPy()
{
	PythonReleaseGil unlocker;

	return getPivotPoint();
}

boost::shared_ptr<Position> StageElements::getPivotPoint()
{
	return stageMetadata_->getRotationOperation()->getPivotPoint();
}

float StageElements::getRotationAnglePy()
{
	PythonReleaseGil unlocker;

	return getRotationAngle();
}

float StageElements::getRotationAngle()
{
	return stageMetadata_->getRotationOperation()->getAngle();
}

void StageElements::setAnimationSlotAlphaGradientDirectionByNamePy(std::string slotName, AlphaGradientDirection alphaGradientDirection)
{
	PythonReleaseGil unlocker;

	setAnimationSlotAlphaGradientDirectionByName(slotName, alphaGradientDirection);
}

void StageElements::setAnimationSlotAlphaGradientDirectionByName(std::string slotName, AlphaGradientDirection alphaGradientDirection)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotAlphaGradientDirectionByIndex(slotIndex, alphaGradientDirection);
}

void StageElements::setAnimationSlotAlphaGradientDirectionByIndex(int slotIndex, AlphaGradientDirection alphaGradientDirection)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setAlphaGradientDirection(alphaGradientDirection);
	}
}

void StageElements::setAnimationSlotAlphaGradientFromByNamePy(std::string slotName, float alphaGradientFrom)
{
	PythonReleaseGil unlocker;

	setAnimationSlotAlphaGradientFromByName(slotName, alphaGradientFrom);
}

void StageElements::setAnimationSlotAlphaGradientFromByName(std::string slotName, float alphaGradientFrom)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotAlphaGradientFromByIndex(slotIndex, alphaGradientFrom);
}

void StageElements::setAnimationSlotAlphaGradientFromByIndex(int slotIndex, float alphaGradientFrom)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setAlphaGradientFrom(alphaGradientFrom);
	}
}


void StageElements::setAnimationSlotAlphaGradientToByNamePy(std::string slotName, float alphaGradientTo)
{
	PythonReleaseGil unlocker;

	setAnimationSlotAlphaGradientToByName(slotName, alphaGradientTo);
}

void StageElements::setAnimationSlotAlphaGradientToByName(std::string slotName, float alphaGradientTo)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotAlphaGradientToByIndex(slotIndex, alphaGradientTo);
}

void StageElements::setAnimationSlotAlphaGradientToByIndex(int slotIndex, float alphaGradientTo)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->renderEffects_->setAlphaGradientTo(alphaGradientTo);
	}
}

void StageElements::setAnimationSlotAnimationStyleByNamePy(std::string slotName, AnimationStyle animationStyle)
{
	PythonReleaseGil unlocker;

	setAnimationSlotAnimationStyleByName(slotName, animationStyle);
}

void StageElements::setAnimationSlotAnimationStyleByName(std::string slotName, AnimationStyle animationStyle)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotAnimationStyleByIndex(slotIndex, animationStyle);
}

void StageElements::setAnimationSlotAnimationStyleByIndex(int slotIndex, AnimationStyle animationStyle)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		animationSlots_[slotIndex]->animationPlayer_->animationStyle_ = animationStyle;
	}
}

void StageElements::setAnimationSlotPositionXByNamePy(std::string slotName, int x)
{
	PythonReleaseGil unlocker;

	setAnimationSlotPositionXByName(slotName, x);
}

void StageElements::setAnimationSlotPositionXByName(std::string slotName, int x)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	setAnimationSlotPositionXByIndex(slotIndex, x);
}

void StageElements::setAnimationSlotPositionXByIndexPy(int slotIndex, int x)
{
	PythonReleaseGil unlocker;

	setAnimationSlotPositionXByIndex(slotIndex, x);
}

void StageElements::setAnimationSlotPositionXByIndex(int slotIndex, int x)
{
	// Set the animation slot position. Need to take into account the origin, because
	// the position is stored in the engine as native TLC coordinates, but as far as
	// the user is concerned, the origin is whatever system they specified.
	// So if for example the origin is center, it will need to take half the width
	// as a translation offset to convert to native TLC coordinates

	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		AnimationSlotPtr animationSlot = animationSlots_[slotIndex];

		int stageOffsetX = 0;

		int animationOffsetX = 0;

		switch (stageMetadata_->getOrigin())
		{
		case STAGE_ORIGIN_CENTER:
		case STAGE_ORIGIN_TOP_MIDDLE:
		case STAGE_ORIGIN_BOTTOM_MIDDLE:

			int stageWidth = stageMetadata_->getWidth();

			stageOffsetX = (int)(stageWidth / 2);

			break;

		}

		switch (animationSlot->origin_)
		{
		case ANIMATION_SLOT_ORIGIN_CENTER:
		case ANIMATION_SLOT_ORIGIN_TOP_MIDDLE:
		case ANIMATION_SLOT_ORIGIN_BOTTOM_MIDDLE:

			if (animationSlot->animation_ != nullptr)
			{
				auto sheet = renderer_->getSheet(animationSlot->animation_->getSpriteSheetId());

				if (sheet != nullptr)
				{
					int cellWidth = sheet->getCellWidth();

					float scaleFactor = sheet->getScaleFactor();

					animationOffsetX = (int)((cellWidth * scaleFactor) / 2);
				}
			}

			break;
		}

		int transformedX = x + stageOffsetX - animationOffsetX;


		animationSlots_[slotIndex]->nativePosition_->setX(transformedX);
	}
}

void StageElements::setAnimationSlotPositionYByNamePy(std::string slotName, int y)
{
	PythonReleaseGil unlocker;

	setAnimationSlotPositionYByName(slotName, y);
}

void StageElements::setAnimationSlotPositionYByName(std::string slotName, int y)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	setAnimationSlotPositionYByIndex(slotIndex, y);
}

void StageElements::setAnimationSlotPositionYByIndexPy(int slotIndex, int y)
{
	PythonReleaseGil unlocker;

	setAnimationSlotPositionYByIndex(slotIndex, y);
}

void StageElements::setAnimationSlotPositionYByIndex(int slotIndex, int y)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		AnimationSlotPtr animationSlot = animationSlots_[slotIndex];

		int y = animationSlot->nativePosition_->getY();

		int stageOffsetY = 0;

		int animationOffsetY = 0;

		switch (stageMetadata_->getOrigin())
		{
		case STAGE_ORIGIN_CENTER:
		case STAGE_ORIGIN_BOTTOM_MIDDLE:

			int stageHeight = stageMetadata_->getHeight();

			stageOffsetY = (int)(stageHeight / 2);

			break;
		}

		switch (animationSlot->origin_)
		{
		case ANIMATION_SLOT_ORIGIN_CENTER:
		case ANIMATION_SLOT_ORIGIN_BOTTOM_MIDDLE:

			if (animationSlot->animation_ != nullptr)
			{
				auto sheet = renderer_->getSheet(animationSlot->animation_->getSpriteSheetId());

				if (sheet != nullptr)
				{
					int cellHeight = sheet->getCellHeight();

					float scaleFactor = sheet->getScaleFactor();

					animationOffsetY = (int)((cellHeight * scaleFactor) / 2);
				}
			}

			break;
		}

		int transformedY = y + stageOffsetY - animationOffsetY;

		animationSlots_[slotIndex]->nativePosition_->setY(transformedY);
	}
}

void StageElements::setAnimationSlotRotationByNamePy(std::string slotName, float rotation)
{
	PythonReleaseGil unlocker;

	setAnimationSlotRotationByName(slotName, rotation);
}

void StageElements::setAnimationSlotRotationByName(std::string slotName, float rotation)
{
	int slotIndex = getAnimationSlotIndex(slotName);

	return setAnimationSlotRotationByIndex(slotIndex, rotation);
}

void StageElements::setAnimationSlotRotationByIndex(int slotIndex, float rotation)
{
	int size = animationSlots_.size();

	if (slotIndex >= 0 && slotIndex < size)
	{
		if (animationSlots_[slotIndex]->renderEffects_->getRotationOperation(0)->getAngle() != rotation)
		{
			reapplyTransform_ = true;

			animationSlots_[slotIndex]->renderEffects_->getRotationOperation(0)->setAngle(rotation);
		}
	}
}

void StageElements::setRenderableBoundary()
{
	int slotCount = animationSlots_.size();

	bool boundsInitialized = false;

	// Loop through the slots and set the extents.
	for (int i = 0; i < slotCount; i++)
	{
		// The first slot with a valid animation should initialize all values.
		if (animationSlots_[i]->animation_ != nullptr)
		{
			int sheetId = animationSlots_[i]->animation_->getSpriteSheetId();

			if (sheetId >= 0)
			{
				int animationLeft = animationSlots_[i]->getLeftmostPoint();

				int animationTop = animationSlots_[i]->getTopmostPoint();

				int animationRight = animationSlots_[i]->getRightmostPoint();

				int animationBottom = animationSlots_[i]->getBottommostPoint();

				if (boundsInitialized == false)
				{
					leftBoundary_ = animationLeft;

					topBoundary_ = animationTop;

					rightBoundary_ = animationRight;

					bottomBoundary_ = animationBottom;

					boundsInitialized = true;
				}
				else
				{
					if (animationLeft < leftBoundary_)
					{
						leftBoundary_ = animationLeft;
					}

					if (animationTop < topBoundary_)
					{
						topBoundary_ = animationTop;
					}

					if (animationRight > rightBoundary_)
					{
						rightBoundary_ = animationRight;
					}

					if (animationBottom > bottomBoundary_)
					{
						bottomBoundary_ = animationBottom;
					}
				}
			}
		}
	}
}

//void StageElements::setHitboxCollisionStatus(int index, bool status)
//{
//	int size = hitboxCollisionStatuses_.size();
//
//	if (index >= 0 && index < size)
//	{
//		hitboxCollisionStatuses_[index] = status;
//	}
//}
//
//void StageElements::setHitboxEdgeFlags(int index, unsigned char edgeFlags)
//{
//	int size = hitboxEdgeFlags_.size();
//
//	if (index >= 0 && index < size)
//	{
//		hitboxEdgeFlags_[index] = edgeFlags;
//	}
//}
//
//void StageElements::setSlotHitboxCollisionStatusByName(std::string slotName, int frameIndex, int hitboxIndex, bool status)
//{
//	int slotIndex = getAnimationSlotIndex(slotName);
//
//	setSlotHitboxCollisionStatusByIndex(slotIndex, frameIndex, hitboxIndex, status);
//}
//
//void StageElements::setSlotHitboxCollisionStatusByIndex(int slotIndex, int frameIndex, int hitboxIndex, bool status)
//{
//	int slotCount = slotHitboxCollisionStatuses_.size();
//
//	if (slotIndex >= 0 && slotIndex < slotCount)
//	{
//		int frameCount = slotHitboxCollisionStatuses_[slotIndex].size();
//
//		if (frameIndex >= 0 && frameIndex < frameCount)
//		{
//			int hitboxReferenceCount = slotHitboxCollisionStatuses_[slotIndex][frameIndex].size();
//
//			if (hitboxIndex >= 0 && hitboxIndex < hitboxReferenceCount)
//			{
//				slotHitboxCollisionStatuses_[slotIndex][frameIndex][hitboxIndex] = status;
//			}
//		}
//	}
//}
//
//void StageElements::setSlotHitboxEdgeFlagsByName(std::string slotName, int frameIndex, int hitboxIndex, unsigned char edgeFlags)
//{
//	int slotIndex = getAnimationSlotIndex(slotName);
//
//	setSlotHitboxEdgeFlagsByIndex(slotIndex, frameIndex, hitboxIndex, edgeFlags);
//}
//
//void StageElements::setSlotHitboxEdgeFlagsByIndex(int slotIndex, int frameIndex, int hitboxIndex, unsigned char edgeFlags)
//{
//	int slotCount = slotHitboxEdgeFlags_.size();
//
//	if (slotIndex >= 0 && slotIndex < slotCount)
//	{
//		int frameCount = slotHitboxEdgeFlags_[slotIndex].size();
//
//		if (frameIndex >= 0 && frameIndex < frameCount)
//		{
//			int hitboxReferenceCount = slotHitboxEdgeFlags_[slotIndex][frameIndex].size();
//
//			if (hitboxIndex >= 0 && hitboxIndex < hitboxReferenceCount)
//			{
//				slotHitboxEdgeFlags_[slotIndex][frameIndex][hitboxIndex] = edgeFlags;
//			}
//		}
//	}
//}

void StageElements::setRotationAnglePy(float rotationAngle)
{
	PythonReleaseGil unlocker;

	setRotationAngle(rotationAngle);
}

void StageElements::setRotationAngle(float rotationAngle)
{
	stageMetadata_->getRotationOperation()->setAngle(rotationAngle);
}

//void StageElements::setSingleFramePy(bool singleFrame)
//{
//	PythonReleaseGil unlocker;
//
//	return setSingleFrame(singleFrame);
//}

void StageElements::setSingleFrame(bool singleFrame)
{
	singleFrame_ = singleFrame;
}
