#include "..\..\Headers\EngineCore\StageRenderable.hpp"

using namespace firemelon;

StageRenderable::StageRenderable()
{
	renderEffects_ = RenderEffectsPtr(new RenderEffects());

	boost::shared_ptr<firemelon::RotationOperation> rotationOperation1 = boost::shared_ptr<firemelon::RotationOperation>(new RotationOperation(0.0, 0, 0));
	boost::shared_ptr<firemelon::RotationOperation> rotationOperation2 = boost::shared_ptr<firemelon::RotationOperation>(new RotationOperation(0.0, 0, 0));

	renderEffects_->addRotation(rotationOperation1);
	renderEffects_->addRotation(rotationOperation2);

	renderEffects_->debug_ = true;

	slotOutlineColor_ = boost::make_shared<ColorRgba>(ColorRgba(1.0f, 0.647f, 0.0f, 1.0f));

	stageOutlineColor_ = boost::make_shared<ColorRgba>(ColorRgba(1.0f, 0.0f, 1.0f, 1.0f));

	anchorPointColor_ = boost::make_shared<ColorRgba>(ColorRgba(1.0f, 1.0f, 0.0f, 1.0f));

	layerLerp_ = true;

	isBackground_ = false;

	position_ = nullptr;
}

StageRenderable::~StageRenderable()
{
}

bool StageRenderable::getIsDynamic()
{
	return true;
}


int StageRenderable::getXPy()
{
	PythonReleaseGil unlocker;

	return getX();
}

int StageRenderable::getX()
{
	if (position_ != nullptr)
	{
		int x = position_->getX();

		if (stateContainer_->currentStateIndex_ >= 0)
		{
			x += stage_->currentStageElements_->leftBoundary_;
		}

		return x;
	}
	else
	{
		return 0;
	}
}

int StageRenderable::getYPy()
{
	PythonReleaseGil unlocker;

	return getY();
}

int StageRenderable::getY()
{
	if (position_ != nullptr)
	{
		int y = position_->getY();

		if (stateContainer_->currentStateIndex_ >= 0)
		{
			y += stage_->currentStageElements_->topBoundary_;
		}

		return y;
	}
	else
	{
		return 0;
	}
}

unsigned int StageRenderable::getHeightPy()
{
	PythonReleaseGil unlocker;

	return getHeight();
}

unsigned int StageRenderable::getHeight()
{
	// The animation slot that is farthest down, plus the height of the animation in it is the bottom extent.
	// The animation slot that is farthest up is the top extent.
	// The height is the difference between them.
	
	int stateIndex = stateContainer_->getCurrentStateIndex();

	StageElementsPtr stageElements = stage_->getStageElementsByIndex(stateIndex);

	if (stageElements != nullptr)
	{
		return stageElements->bottomBoundary_ - stageElements->topBoundary_;
	}
	else
	{
		return 0;
	}

}

unsigned int StageRenderable::getWidthPy()
{
	PythonReleaseGil unlocker;

	return getWidth();
}

unsigned int StageRenderable::getWidth()
{
	// The animation slot that is farthest right, plus the width of the animation in it is the right extent.
	// The animation slot that is farthest left is the left extent.
	// The width is the difference between them.
	if (stateContainer_->currentStateIndex_ >= 0)
	{
		return stage_->currentStageElements_->rightBoundary_ - stage_->currentStageElements_->leftBoundary_;
	}
	else
	{
		return 0;
	}
}

void StageRenderable::initializeRenderable()
{
	renderer_ = getRenderer();
	position_ = getPosition();
	layerPosition_ = getLayerPosition();
}


void StageRenderable::render(double lerp)
{
	if (getIsVisible() == true && stage_->currentStageElements_ != nullptr)
	{
		// Calculate the screen location. Start by LERPing the position. Offset by each animation position.

		// Note - At some point things started getting jittery, and apparently it is because it is already
		// factoring in the LERP value in the layer position, because the camera position is LERP'd and this is used
		// to determine the layer position. I'm not sure when or why this became a problem because it had been working
		// just fine for a while.

		// Update, I stopped LERPing the camera position in the layerPosition value, because LERPing needs to be done only 
		// when rendering, not when setting actual stateful data.

		#if defined(_DEBUG)	

		//if (stage_->ownerMetadata_->getEntityTypeId() == 691)
		//{
		//	//energy whisk
		//	std::cout << "Render whisk" << std::endl;
		//	bool debug = true;
		//}
		//else if (stage_->ownerMetadata_->getEntityTypeId() == 722)
		//{
		//	// Assassin
		//	std::cout << "Render Assassin" << std::endl;
		//	bool debug = true;
		//}
		//else if (stage_->ownerMetadata_->getEntityTypeId() == 732)
		//{
		//	// Archangel
		//	std::cout << "Render Archangel" << std::endl;
		//	bool debug = true;
		//}

		#endif


		// First, apply any transformations, if necessary.

		// Note that this logic also exists in the updaterenderable function, because rendering and updates don't necessarily occur
		// in lock step with one another. It's possible a render may occur first, and animation transforms need to be 
		// applied. It's also possible that an entity is offscreen, and therefore will not be rendered, so in this case
		// it would call the transform applies in the update.
		if (stage_->reapplyTransform_ == true || stage_->currentStageElements_->reapplyTransform_ == true)
		{
			stage_->reapplyTransform_ = false;

			stage_->currentStageElements_->reapplyTransform_ = false;

			stage_->applyTransforms(lerp);
		}

		renderEffects_->setMirrorHorizontal(stage_->mirrorHorizontally_);

		int previousLayerX = layerPosition_->previousXForRender_;
		int previousLayerY = layerPosition_->previousYForRender_;

		int currentLayerX = layerPosition_->getX();
		int currentLayerY = layerPosition_->getY();

		int layerPositionX = previousLayerX + (lerp * (currentLayerX - previousLayerX));
		int layerPositionY = previousLayerY + (lerp * (currentLayerY - previousLayerY));

		int spritePositionX = position_->previousX_ + (lerp * (position_->x_ - position_->previousXForRender_));
		int spritePositionY = position_->previousY_ + (lerp * (position_->y_ - position_->previousYForRender_));

		// Remember to include the stage position offset, for sprites whose origins are not TLC.
		int drawAtX = layerPositionX + spritePositionX + stage_->metadata_->getPosition()->getX();
		int drawAtY = layerPositionY + spritePositionY + stage_->metadata_->getPosition()->getY();



		// Set the extents. Use the stage extent values to set each of the animation extents.

		// Stage extents start from the farthest animations in each direction.

		// If an animation already has an extent set, which is beyond the stage extent in that direction, it 
		// will take precedence.

		int stageLeftmostBound = 0;
		int stageTopmostBound = 0;
		int stageRightmostBound = 0;
		int stageBottommostBound = 0;

		// Loop through the animations to find the stage bounds in TLC space.
		int size = stage_->activeAnimationSlots_.size();

		for (int i = 0; i < size; i++)
		{
			AnimationSlotPtr animationSlot = stage_->activeAnimationSlots_[i];

			if (i == 0)
			{
				stageLeftmostBound = animationSlot->nativeCorners_[0].x;

				stageTopmostBound = animationSlot->nativeCorners_[0].y;

				stageRightmostBound = animationSlot->nativeCorners_[2].x;

				stageBottommostBound = animationSlot->nativeCorners_[2].y;
			}
			else
			{
				if (animationSlot->nativeCorners_[0].x < stageLeftmostBound)
				{
					stageLeftmostBound = animationSlot->nativeCorners_[0].x;
				}

				if (animationSlot->nativeCorners_[0].y < stageTopmostBound)
				{
					stageTopmostBound = animationSlot->nativeCorners_[0].y;
				}

				if (animationSlot->nativeCorners_[2].x > stageRightmostBound)
				{
					stageRightmostBound = animationSlot->nativeCorners_[2].x;
				}

				if (animationSlot->nativeCorners_[2].y > stageBottommostBound)
				{
					stageBottommostBound = animationSlot->nativeCorners_[2].y;
				}
			}

		}

		int stageExtentLeftLine = stageLeftmostBound;
		int stageExtentTopLine = stageTopmostBound;
		int stageExtentRightLine = stageRightmostBound;
		int stageExtentBottomLine = stageBottommostBound;

		int stageTotalHeight = stageBottommostBound - stageTopmostBound;
		int stageTotalWidth = stageRightmostBound - stageLeftmostBound;

		bool interpolateExtents = stage_->metadata_->renderEffects_->getInterpolateExtents();

		// Left defaults to -1.0
		float extentLeft = stage_->metadata_->renderEffects_->getExtentLeft();

		if (interpolateExtents == true)
		{
			float previousExtentLeft = stage_->metadata_->renderEffects_->previousExtentLeft_;

			extentLeft = previousExtentLeft + (lerp * (extentLeft - previousExtentLeft));
		}

		if (extentLeft > -1.0)
		{
			// Convert the extent value from range -1 to 1 to range 0 to 1. Add 1 to get a range of 0 to 2, and then divide by 2.
			// i.e. an extent of 0 (in the middle) will be 1 / 2 = 0.5

			float percent = (extentLeft + 1) / 2;

			stageExtentLeftLine = stageLeftmostBound + (stageTotalWidth * percent);
		}

		// Top defaults to -1.0
		float extentTop = stage_->metadata_->renderEffects_->getExtentTop();

		if (interpolateExtents == true)
		{
			float previousExtentTop = stage_->metadata_->renderEffects_->previousExtentTop_;

			extentTop = previousExtentTop + (lerp * (extentTop - previousExtentTop));
		}

		if (extentTop > -1.0)
		{
			// Convert the extent value from range -1 to 1 to range 0 to 1. Add 1 to get a range of 0 to 2, and then divide by 2.
			// i.e. an extent of 0 (in the middle) will be 1 / 2 = 0.5

			float percent = (extentTop + 1) / 2;

			stageExtentTopLine = stageTopmostBound + (stageTotalHeight * percent);
		}


		// Right defaults to 1.0
		float extentRight = stage_->metadata_->renderEffects_->getExtentRight();

		if (interpolateExtents == true)
		{
			float previousExtentRight = stage_->metadata_->renderEffects_->previousExtentRight_;

			extentRight = previousExtentRight + (lerp * (extentRight - previousExtentRight));
		}

		if (extentRight < 1.0)
		{
			// Convert the extent value from range -1 to 1 to range 0 to 1. Add 1 to get a range of 0 to 2, and then divide by 2.
			// i.e. an extent of 0 (in the middle) will be 1 / 2 = 0.5

			// The percent is how far along from left to right the extent line is, so to get the stage extent right line,
			// start at the right bound and subtract the total width times 1 minus the percent. So, if the percent is 0.75
			// it would be 0.25 away from the right.
			float percent = 1 - ((extentRight + 1) / 2);

			// Move the right line left
			stageExtentRightLine = stageRightmostBound - (stageTotalWidth * percent);
		}

		// Bottom defaults to 1.0
		float extentBottom = stage_->metadata_->renderEffects_->getExtentBottom();
		
		if (interpolateExtents == true)
		{
			float previousExtentBottom = stage_->metadata_->renderEffects_->previousExtentBottom_;

			extentBottom = previousExtentBottom + (lerp * (extentBottom - previousExtentBottom));
		}

		if (extentBottom < 1.0)
		{
			// Convert the extent value from range -1 to 1 to range 0 to 1. Add 1 to get a range of 0 to 2, and then divide by 2.
			// i.e. an extent of 0 (in the middle) will be 1 / 2 = 0.5
			
			// The percent is how far along from top to bottom the extent line is, so to get the stage extent bottom line,
			// start at the bottom bound and subtract the total height times 1 minus the percent. So, if the percent is 0.75
			// it would be 0.25 away from the bottom.
			float percent = 1 - ((extentBottom + 1) / 2);

			// Move the bottom line up
			stageExtentBottomLine = stageBottommostBound - (stageTotalHeight * percent);
		}

		size = stage_->activeAnimationSlots_.size();
		
		for (int i = 0; i < size; i++)
		{
			AnimationSlotPtr animationSlot = stage_->activeAnimationSlots_[i];
			
			// If this is a background slot in the background stage, or a foreground slot in the foreground stage.
			if (animationSlot->isBackground_ == isBackground_)
			{
				boost::shared_ptr<AnimationPlayer> animationPlayer = animationSlot->getAnimationPlayer();

				AnimationPtr animation = animationSlot->animation_;

				if (animationPlayer->isVisible_ == true && animation != nullptr)
				{
					// Get the sheet ID, current frame, and relative position of the animation.
					int sheetId = animationSlot->animation_->spriteSheetId_;

					boost::shared_ptr<SpriteSheet> spriteSheet = renderer_->getSheet(sheetId);

					int currentFrame = animationPlayer->currentFrame_;

					// Combine the slot render effects with the stage render effects. Blend colors.
					boost::shared_ptr<RenderEffects> slotRenderEffects = animationSlot->getRenderEffects();

					ColorRgbaPtr slotHueColor = slotRenderEffects->getHueColor();

					float slotHueRed = slotHueColor->getR();

					float slotHueGreen = slotHueColor->getG();

					float slotHueBlue = slotHueColor->getB();

					float slotHueAlpha = slotHueColor->getA();


					ColorRgbaPtr stageHueColor = stage_->metadata_->renderEffects_->getHueColor();

					float stageHueRed = stageHueColor->getR();

					float stageHueGreen = stageHueColor->getG();

					float stageHueBlue = stageHueColor->getB();

					float stageHueAlpha = stageHueColor->getA();


					ColorRgbaPtr combinedHueColor = renderEffects_->getHueColor();

					combinedHueColor->setR(stageHueRed * slotHueRed);

					combinedHueColor->setG(stageHueGreen * slotHueGreen);

					combinedHueColor->setB(stageHueBlue * slotHueBlue);

					combinedHueColor->setA(stageHueAlpha * slotHueAlpha);

					// Right now, stage does not support an overall blend color.
					// I'm not sure how to handle that, because the blending algorithm depends on
					// the user. For example, right now I am using the GLSL mix function, but in theory
					// it could be anything, and you would want them to match.
					ColorRgbaPtr slotBlendColor = slotRenderEffects->getBlendColor();

					float slotBlendRed = slotBlendColor->getR();

					float slotBlendGreen = slotBlendColor->getG();

					float slotBlendBlue = slotBlendColor->getB();

					float slotBlendAlpha = slotBlendColor->getA();


					ColorRgbaPtr blendColor = renderEffects_->getBlendColor();

					blendColor->setR(slotBlendRed);

					blendColor->setG(slotBlendGreen);

					blendColor->setB(slotBlendBlue);

					blendColor->setA(slotBlendAlpha);


					float slotBlendPercent = slotRenderEffects->getBlendPercent();

					renderEffects_->setBlendPercent(slotBlendPercent);



					ColorRgbaPtr stageOutlineColor = stage_->metadata_->renderEffects_->getOutlineColor();

					float stageOutlineRed = stageOutlineColor->getR();

					float stageOutlineGreen = stageOutlineColor->getG();

					float stageOutlineBlue = stageOutlineColor->getB();

					float stageOutlineAlpha = stageOutlineColor->getA();


					ColorRgbaPtr slotOutlineColor = slotRenderEffects->getOutlineColor();

					float slotOutlineRed = slotOutlineColor->getR();

					float slotOutlineGreen = slotOutlineColor->getG();

					float slotOutlineBlue = slotOutlineColor->getB();

					float slotOutlineAlpha = slotOutlineColor->getA();


					ColorRgbaPtr combinedOutlineColor = renderEffects_->getOutlineColor();

					combinedOutlineColor->setR(stageOutlineRed * slotOutlineRed);

					combinedOutlineColor->setG(stageOutlineGreen * slotOutlineGreen);

					combinedOutlineColor->setB(stageOutlineBlue * slotOutlineBlue);

					combinedOutlineColor->setA(stageOutlineAlpha * slotOutlineAlpha);


					int size = animation->frames_.size();

					if (currentFrame >= 0 && currentFrame < size)
					{
						// Get the frame data.
						boost::shared_ptr<AnimationFrame> frame = animation->frames_[currentFrame];

						int col = frame->spriteSheetCellColumn_;
						int row = frame->spriteSheetCellRow_;

						int slotLeft = animationSlot->nativeCorners_[0].x;
						int slotTop = animationSlot->nativeCorners_[0].y;
						int slotRight = animationSlot->nativeCorners_[2].x;
						int slotBottom = animationSlot->nativeCorners_[2].y;

						int slotHeight = slotBottom - slotTop;
						int slotWidth = slotRight - slotLeft;

						// Calculate the top line of the animation when the slot extent is applied. If the stage
						// top line goes beyond it, calculate a new extent value to use instead. Otherwise use
						// the slot extent.
						float topExtent = -1;

						int slotTopLine = slotTop;

						float slotExtentTop = slotRenderEffects->getExtentTop();

						if (interpolateExtents == true)
						{
							float previousExtentTop = slotRenderEffects->previousExtentTop_;

							slotExtentTop = previousExtentTop + (lerp * (slotExtentTop - previousExtentTop));
						}

						if (slotExtentTop > -1.0f)
						{
							float percent = (extentTop + 1) / 2;

							slotTopLine = slotTop - (stageTotalHeight * percent);
						}

						// Check if the slot top line is above the stage top line.
						// Use this to determine which top line to use, and use it to calculate a new extent if necessary, to match the desired top line.
						if (slotTopLine < stageExtentTopLine)
						{
							// Calculate what the slot extent will need to be for its top line to match the stage top line.

							// Get the percent for how far deep the stage top line is in the slot.
							float percent = (float)(stageExtentTopLine - slotTop) / (float)slotHeight;

							// Convert the percent BACK to an extent value in the range of -1 to 1, using the inverse of the function to get the percent.
							topExtent = (2 * percent) - 1;
						}
						else
						{
							topExtent = slotExtentTop;
						}

						renderEffects_->setExtentTop(topExtent);

						// Calculate the bottom line of the animation when the slot extent is applied. If the stage
						// bottom line goes beyond it, calculate a new extent value to use instead. Otherwise use
						// the slot extent.
						float bottomExtent = 1;

						int slotBottomLine = slotBottom;

						float slotExtentBottom = slotRenderEffects->getExtentBottom();

						if (interpolateExtents == true)
						{
							float previousExtentBottom = slotRenderEffects->previousExtentBottom_;

							slotExtentBottom = previousExtentBottom + (lerp * (slotExtentBottom - previousExtentBottom));
						}

						if (slotExtentBottom < 1.0f)
						{
							float percent = (extentBottom + 1) / 2;

							slotBottomLine = slotBottom - (stageTotalHeight * percent);
						}

						// Check if the slot bottom line is below the stage bottom line.
						// Use this to determine which bottom line to use, and use it to calculate a new extent if necessary, to match the desired bottom line.
						if (slotBottomLine > stageExtentBottomLine)
						{
							// Calculate what the slot extent will need to be for its bottom line to match the stage bottom line.

							// Get the percent for how far deep the stage bottom line is in the slot.
							float percent = (float)(stageExtentBottomLine - slotTop) / (float)slotHeight;

							// Convert the percent BACK to an extent value in the range of -1 to 1, using the inverse of the function to get the percent.
							bottomExtent = (2 * percent) - 1;
						}
						else
						{
							bottomExtent = slotExtentBottom;
						}

						renderEffects_->setExtentBottom(bottomExtent);


						// Calculate the left line of the animation when the slot extent is applied. If the stage
						// left line goes beyond it, calculate a new extent value to use instead. Otherwise use
						// the slot extent.
						float leftExtent = -1;

						int slotLeftLine = slotLeft;

						float slotExtentLeft = slotRenderEffects->getExtentLeft();

						if (interpolateExtents == true)
						{
							float previousExtentLeft = slotRenderEffects->previousExtentLeft_;

							slotExtentLeft = previousExtentLeft + (lerp * (slotExtentLeft - previousExtentLeft));
						}

						if (slotExtentLeft > -1.0f)
						{
							float percent = (extentLeft + 1) / 2;

							slotLeftLine = slotLeft - (stageTotalWidth * percent);
						}

						// Check if the slot left line is before the stage left line.
						// Use this to determine which left line to use, and use it to calculate a new extent if necessary, to match the desired left line.
						if (slotLeftLine < stageExtentLeftLine)
						{
							// Calculate what the slot extent will need to be for its left line to match the stage left line.

							// Get the percent for how far deep the stage left line is in the slot.
							float percent = (float)(stageExtentLeftLine - slotLeft) / (float)slotWidth;

							// Convert the percent BACK to an extent value in the range of -1 to 1, using the inverse of the function to get the percent.
							leftExtent = (2 * percent) - 1;
						}
						else
						{
							leftExtent = slotExtentLeft;
						}

						renderEffects_->setExtentLeft(leftExtent);

						// Calculate the right line of the animation when the slot extent is applied. If the stage
						// right line goes beyond it, calculate a new extent value to use instead. Otherwise use
						// the slot extent.
						float rightExtent = 1;

						int slotRightLine = slotRight;

						float slotExtentRight = slotRenderEffects->getExtentRight();

						if (interpolateExtents == true)
						{
							float previousExtentRight = slotRenderEffects->previousExtentRight_;

							slotExtentRight = previousExtentRight + (lerp * (slotExtentRight - previousExtentRight));
						}

						if (slotExtentRight < 1.0f)
						{
							float percent = (extentRight + 1) / 2;

							slotRightLine = slotRight - (stageTotalWidth * percent);
						}

						// Check if the slot right line is below the stage right line.
						// Use this to determine which right line to use, and use it to calculate a new extent if necessary, to match the desired right line.
						if (slotRightLine > stageExtentRightLine)
						{
							// Calculate what the slot extent will need to be for its right line to match the stage right line.

							// Get the percent for how far deep the stage right line is in the slot.
							float percent = (float)(stageExtentRightLine - slotLeft) / (float)slotWidth;

							// Convert the percent BACK to an extent value in the range of -1 to 1, using the inverse of the function to get the percent.
							rightExtent = (2 * percent) - 1;
						}
						else
						{
							rightExtent = slotExtentRight;
						}


						renderEffects_->setExtentRight(rightExtent);


						Quad quad;

						quad.vertices[0].x = drawAtX + animationSlot->transformedCorners_[0].x;
						quad.vertices[0].y = drawAtY + animationSlot->transformedCorners_[0].y;

						quad.vertices[1].x = drawAtX + animationSlot->transformedCorners_[1].x;
						quad.vertices[1].y = drawAtY + animationSlot->transformedCorners_[1].y;

						quad.vertices[2].x = drawAtX + animationSlot->transformedCorners_[2].x;
						quad.vertices[2].y = drawAtY + animationSlot->transformedCorners_[2].y;

						quad.vertices[3].x = drawAtX + animationSlot->transformedCorners_[3].x;
						quad.vertices[3].y = drawAtY + animationSlot->transformedCorners_[3].y;

						renderer_->renderSheetCell(quad, sheetId, col, row, renderEffects_);
					}
				}
			}
		}

		rendered(spritePositionX, spritePositionY);
	}
}

void StageRenderable::renderDebugData(double lerp)
{
	if (getIsVisible() == true)
	{	
		// Calculate the screen location. Start by LERPing the position. Offset by each animation position.

		// Note - At some point things started getting jittery, and apparently it is because it is already
		// factoring in the LERP value in the layer position, because the camera position is LERP'd and this is used
		// to determine the layer position. I'm not sure when or why this became a problem because it had been working
		// just fine for a while.

		// Update, I stopped LERPing the camera position in the layerPosition value, because LERPing needs to be done only 
		// when rendering, not when setting actual stateful data.

#if defined(_DEBUG)	

		if (stage_->ownerMetadata_->getEntityTypeId() == 703)
		{
			bool debug = true;
		}

#endif

		int previousLayerX = layerPosition_->previousXForRender_;
		int previousLayerY = layerPosition_->previousYForRender_;

		int currentLayerX = layerPosition_->getX();
		int currentLayerY = layerPosition_->getY();

		int layerPositionX = previousLayerX + (lerp * (currentLayerX - previousLayerX));
		int layerPositionY = previousLayerY + (lerp * (currentLayerY - previousLayerY));

		int spritePositionX = position_->previousX_ + (lerp * (position_->x_ - position_->previousXForRender_));
		int spritePositionY = position_->previousY_ + (lerp * (position_->y_ - position_->previousYForRender_));

		int drawAtX = layerPositionX + spritePositionX + stage_->metadata_->getPosition()->getX();
		int drawAtY = layerPositionY + spritePositionY + stage_->metadata_->getPosition()->getY();


		// Build a list of vertices transformed to screen space.
		std::vector<Vertex2> vertices;

		// Corner 0 vertex
		Vertex2 corner0;

		corner0.x = drawAtX;

		corner0.y = drawAtY;

		vertices.push_back(corner0);


		// Corner 1 vertex
		Vertex2 corner1;

		corner1.x = drawAtX + stage_->metadata_->getWidth();

		corner1.y = drawAtY;

		vertices.push_back(corner1);


		// Corner 2 vertex
		Vertex2 corner2;

		corner2.x = drawAtX + stage_->metadata_->getWidth();

		corner2.y = drawAtY + stage_->metadata_->getHeight();

		vertices.push_back(corner2);


		// Corner 3 vertex
		Vertex2 corner3;

		corner3.x = drawAtX;

		corner3.y = drawAtY + stage_->metadata_->getHeight();

		vertices.push_back(corner3);
		
		renderer_->drawPolygon(vertices, stageOutlineColor_);

		// Render the origin.
		renderer_->drawRect(drawAtX - stage_->metadata_->getPosition()->getX() - 2, drawAtY - stage_->metadata_->getPosition()->getY() - 2, 5, 5, 1.0f, 1.0f, 1.0f, 1.0f);

		// Loop through each animation in this stage controller for the current state.
		// First get the current state.
		if (stage_->currentStageElements_ != nullptr)
		{

			int size = stage_->activeAnimationSlots_.size();

			for (int i = 0; i < size; i++)
			{
				AnimationSlotPtr animationSlot = stage_->activeAnimationSlots_[i];

				boost::shared_ptr<AnimationPlayer> animationPlayer = animationSlot->getAnimationPlayer();

				AnimationPtr animation = animationSlot->animation_;

				if (animationPlayer->isVisible_ == true && animation != nullptr)
				{
					// Get the sheet ID, current frame, and relative position of the animation.
					int sheetId = animationSlot->animation_->spriteSheetId_;

					boost::shared_ptr<SpriteSheet> spriteSheet = renderer_->getSheet(sheetId);

					int currentFrame = animationPlayer->currentFrame_;

					std::vector<Vertex2> slotOutlineVertices;

					for (size_t j = 0; j < animationSlot->transformedCorners_.size(); j++)
					{
						Vertex2 slotOutlineVertex;

						slotOutlineVertex.x = drawAtX + animationSlot->transformedCorners_[j].x;

						slotOutlineVertex.y = drawAtY + animationSlot->transformedCorners_[j].y;

						slotOutlineVertices.push_back(slotOutlineVertex);

						int animationId = animationSlot->getAnimationId();

						if (animationId >= 0)
						{
							AnimationPtr animation = animationManager_->getAnimationByIndex(animationId);

							int currentFrameIndex = animationPlayer->getCurrentFrame();

							if (currentFrameIndex >= 0)
							{
								int anchorPointCount = animationSlot->anchorPointReferences_[currentFrameIndex].size();

								for (int k = 0; k < anchorPointCount; k++)
								{
									int anchorPointId = animationSlot->anchorPointReferences_[currentFrameIndex][k];

									AnchorPointPtr anchorPoint = anchorPointManager_->getAnchorPoint(anchorPointId);

									renderer_->drawRect(drawAtX + anchorPoint->transformedPosition_.x - 2, drawAtY + anchorPoint->transformedPosition_.y - 2, 5, 5, 1.0f, 1.0f, 1.0f, 1.0f);
								}
							}
						}
					}

					renderer_->drawPolygon(slotOutlineVertices, slotOutlineColor_);
				}
			}
		}
	}
}

void StageRenderable::updateRenderable(double time)
{
	// If this is a background stage renderable, don't update. The update will happen in the foreground renderable.
	if (stateContainer_->currentStateIndex_ >= 0 && isBackground_ == false)
	{
		// Apply any transforms if necessary. This is to ensure the hitboxes are positioned and oriented correctly.
		// Note that this logic also exists in the render function, because rendering and updates don't necessarily occur
		// in lock step with one another. It's possible a render may occur first, and animation transforms need to be 
		// applied. It's also possible that an entity is offscreen, and therefore will not be rendered, so in this case
		// it would call the transform applies in the update.
		if (stage_->reapplyTransform_ == true || stage_->currentStageElements_->reapplyTransform_ == true)
		{
			stage_->reapplyTransform_ = false;

			stage_->currentStageElements_->reapplyTransform_ = false;

			// Use a lerp value of 1.0, because it should only be hitting this function if the renderable is off screen, so lerp is irrelevant.
			stage_->applyTransforms(1.0);
		}

		// BUG002: Changing the state will change the active animation slots, which this loop is iterating through.
		// This presents a problem, in that it will potentially go out of the array bounds, or operate on
		// the wrong animation slot.
		//
		// Additionally, it is also possible that the user can change the state in the frameTrigger event code, which
		// can be fired also within this loop. 
		
		// In order to address this problem, make a copy of the active slots into a separate list to loop through.		
		int animationSlotCount = stage_->activeAnimationSlots_.size();

		AnimationSlotPtrList slotsToIterateOver;

		for (int i = 0; i < animationSlotCount; i++)
		{
			slotsToIterateOver.push_back(stage_->activeAnimationSlots_[i]);
		}

		for (int i = 0; i < animationSlotCount; i++)
		{
			AnimationSlotPtr animationSlot = slotsToIterateOver[i];
			
			boost::shared_ptr<AnimationPlayer> animationPlayer = animationSlot->getAnimationPlayer();

			int animationId = animationSlot->getAnimationId();

			if (animationId >= 0)
			{
				boost::shared_ptr<Animation> animation = animationManager_->getAnimationByIndex(animationId);

				// Get the current frame.
				int currentFrameIndex = animationPlayer->getCurrentFrame();

				boost::shared_ptr<AnimationFrame> f = animation->getFrame(currentFrameIndex);

				bool frameChanged = animationPlayer->updateAnimation(animation, time);

				if (frameChanged == true && f != nullptr)
				{
					int hitboxCount = animationSlot->hitboxReferences_[currentFrameIndex].size();

					for (int j = 0; j < hitboxCount; j++)
					{
						int hitboxId = animationSlot->hitboxReferences_[currentFrameIndex][j];

						hitboxController_->deactivateHitbox(hitboxId);
					}
				}

				currentFrameIndex = animationPlayer->getCurrentFrame();

				int frameCount = animation->getFrameCount();

				AnimationStyle style = animationPlayer->getAnimationStyle();

				if (currentFrameIndex >= frameCount)
				{
					if (style == ANIMATION_STYLE_SINGLE_END_STATE)
					{
						// If there is a next state set, switch to it.
						
						if (animationSlot->nextStateName_ != "")
						{
							boost::optional<bool> optionalResult = setStateByNameSignal(animationSlot->nextStateName_);
							
							bool resultCode = optionalResult.get();

							if (resultCode == false)
							{
								// The state didn't change for some reason. This would result in a bug where the current frame keeps increasing.
								// Force it to stay on the previous frame and display a warning message.
								animationPlayer->currentFrame_ = animationPlayer->previousFrame_;

								std::cout << "Warning: Animation ended with a style of END_STATE, but the stage was not able to be changed. The animation will remain on the final frame instead." << std::endl;
							}
						}
						
						stateEndedSignal(stateContainer_->currentStateIndex_);
					}
				}
				else
				{
					if (frameChanged == true)
					{
						// Activate the hitboxes in this frame and call the frame triggers.
						AnimationFramePtr f = animation->getFrame(currentFrameIndex);

						if (f != nullptr)
						{
							int hitboxCount = animationSlot->hitboxReferences_[currentFrameIndex].size();

							for (int j = 0; j < hitboxCount; j++)
							{
								int hitboxId = animationSlot->hitboxReferences_[currentFrameIndex][j];

								//unsigned char edgeFlags = currentState_->getHitboxEdgeFlags(j);
								//HitboxStatus status = currentState_->getHitboxStatus(j);

								hitboxController_->activateHitbox(hitboxId, 0xFF);
							}

							int frameTriggerSignalCount = f->getTriggerSignalCount();

							for (int j = 0; j < frameTriggerSignalCount; j++)
							{
								TriggerSignalId triggerSignalId = f->getTriggerSignal(j);

								frameTriggerSignal(triggerSignalId);
							}
						}
					}
				}
			}
		}
	}
}






















//// OLD RENDER FUNCTION, BEFORE SWITCHING TO RENDERING TO A QUAD RATHER THAN A 2D POINT

//void StageRenderable::render(double lerp)
//{
//	if (getIsVisible() == true && stage_->currentStageElements_ != nullptr)
//	{
//		// Calculate the screen location. Start by LERPing the position. Offset by each animation position.
//
//		// Note - At some point things started getting jittery, and apparently it is because it is already
//		// factoring in the LERP value in the layer position, because the camera position is LERP'd and this is used
//		// to determine the layer position. I'm not sure when or why this became a problem because it had been working
//		// just fine for a while.
//
//		// Update, I stopped LERPing the camera position in the layerPosition value, because LERPing needs to be done only 
//		// when rendering, not when setting actual stateful data.
//
//		// First, apply any transormations, if necessary.
//		if (stage_->reapplyTransform_ == true || stage_->currentStageElements_->reapplyTransform_ == true)
//		{
//			stage_->reapplyTransform_ = false;
//
//			stage_->currentStageElements_->reapplyTransform_ = false;
//
//			stage_->applyTransforms();
//		}
//
//		int previousLayerX = layerPosition_->previousXForRender_;
//		int previousLayerY = layerPosition_->previousYForRender_;
//
//		int currentLayerX = layerPosition_->getX();
//		int currentLayerY = layerPosition_->getY();
//
//		int layerPositionX = previousLayerX + (lerp * (currentLayerX - previousLayerX));
//		int layerPositionY = previousLayerY + (lerp * (currentLayerY - previousLayerY));
//
//		int spritePositionX = position_->previousX_ + (lerp * (position_->x_ - position_->previousXForRender_));
//		int spritePositionY = position_->previousY_ + (lerp * (position_->y_ - position_->previousYForRender_));
//
//		int drawAtX = layerPositionX + spritePositionX;
//		int drawAtY = layerPositionY + spritePositionY;
//
//		int size = stage_->activeAnimationSlots_.size();
//
//		bool output = false;
//
//		for (int i = 0; i < size; i++)
//		{
//			AnimationSlotPtr animationSlot = stage_->activeAnimationSlots_[i];
//
//			boost::shared_ptr<AnimationPlayer> animationPlayer = animationSlot->getAnimationPlayer();
//
//			AnimationPtr animation = animationSlot->animation_;
//
//			if (animationPlayer->isVisible_ == true && animation != nullptr)
//			{
//				// Get the sheet ID, current frame, and relative position of the animation.
//				int sheetId = animationSlot->animation_->spriteSheetId_;
//
//				boost::shared_ptr<SpriteSheet> spriteSheet = renderer_->getSheet(sheetId);
//
//				float scaleFactor = spriteSheet->getScaleFactor();
//
//				int slotAnimationWidth = (int)(spriteSheet->getCellWidth() * scaleFactor);
//
//				int slotAnimationHeight = (int)(spriteSheet->getCellHeight() * scaleFactor);
//
//				float stageMidpointX = (stage_->metadata_->getWidth() / 2);
//
//				float stageMidpointY = (stage_->metadata_->getHeight() / 2);
//
//				int currentFrame = animationPlayer->currentFrame_;
//
//				//// Default offset is (0, 0) for a top left origin. 
//				//int offsetX = 0;
//
//				//int offsetY = 0;
//
//				//switch (stageOrigin_)
//				//{
//				//case STAGE_ORIGIN_CENTER:
//				//	
//				//	offsetX = stageMidpointX - (slotAnimationWidth / 2);
//
//				//	offsetY = stageMidpointY - (slotAnimationHeight / 2);
//				//	
//				//	break;
//
//				//}
//
//				int animationPositionX = stage_->getNativeTransformedAnimationSlotXByIndex(i);
//
//				int animationPositionY = stage_->getNativeTransformedAnimationSlotYByIndex(i);
//
//				//animationPositionX += offsetX;
//
//				//animationPositionY += offsetY;
//
//				float slotAnimationMidpointX = animationPositionX + (slotAnimationWidth / 2);
//
//				float slotAnimationMidpointY = animationPositionY + (slotAnimationHeight / 2);
//
//				// Combine the slot render effects with the stage render effects. Blend colors and append rotation operation.
//				boost::shared_ptr<RenderEffects> slotRenderEffects = animationSlot->getRenderEffects();
//
//				ColorRgbaPtr slotHueColor = slotRenderEffects->getHueColor();
//
//				float slotHueRed = slotHueColor->getR();
//
//				float slotHueGreen = slotHueColor->getG();
//
//				float slotHueBlue = slotHueColor->getB();
//
//				float slotHueAlpha = slotHueColor->getA();
//
//				ColorRgbaPtr stageHueColor = stage_->metadata_->renderEffects_->getHueColor();
//
//				float stageHueRed = stageHueColor->getR();
//
//				float stageHueGreen = stageHueColor->getG();
//
//				float stageHueBlue = stageHueColor->getB();
//
//				float stageHueAlpha = stageHueColor->getA();
//
//				ColorRgbaPtr combinedHueColor = renderEffects_->getHueColor();
//
//				combinedHueColor->setR(stageHueRed * slotHueRed);
//
//				combinedHueColor->setG(stageHueGreen * slotHueGreen);
//
//				combinedHueColor->setB(stageHueBlue * slotHueBlue);
//
//				combinedHueColor->setA(stageHueAlpha * slotHueAlpha);
//
//				// Right now, stage does not support an overall blend color.
//				// I'm not sure how to handle that, because the blending algorithm depends on
//				// the user. For example, right now I am using the GLSL mix function, but in theory
//				// it could be anything, and you would want them to match.
//				ColorRgbaPtr slotBlendColor = slotRenderEffects->getBlendColor();
//
//				float slotBlendRed = slotBlendColor->getR();
//
//				float slotBlendGreen = slotBlendColor->getG();
//
//				float slotBlendBlue = slotBlendColor->getB();
//
//				float slotBlendAlpha = slotBlendColor->getA();
//
//				ColorRgbaPtr blendColor = renderEffects_->getBlendColor();
//
//				blendColor->setR(slotBlendRed);
//
//				blendColor->setG(slotBlendGreen);
//
//				blendColor->setB(slotBlendBlue);
//
//				blendColor->setA(slotBlendAlpha);
//
//				float slotBlendPercent = slotRenderEffects->getBlendPercent();
//
//				renderEffects_->setBlendPercent(slotBlendPercent);
//
//				// The combined render effects will have two rotation operations, one to rotate the animation around the slot pivot point, and one to
//				// rotate it around the stage pivot point. Set the pivot point and rotation values here.
//
//				RotationOperationPtr slotRotationOperation = slotRenderEffects->getRotationOperation(0);
//
//				boost::shared_ptr<Position> slotPivotPoint = slotRotationOperation->getPivotPoint();
//
//				RotationOperationPtr stageRotationOperation = stage_->metadata_->getRotationOperation();
//
//				boost::shared_ptr<Position> stagePivotPoint = stageRotationOperation->getPivotPoint();
//
//				// Set the slot rotation angle to the blended
//				renderEffects_->getRotationOperation(0)->setAngle(slotRotationOperation->getAngle());
//
//				// The slot pivot point is in a coordinate space whose origin is at the center of the animation. That is, point (0, 0) is
//				// located at the center of the animation.
//				renderEffects_->getRotationOperation(0)->getPivotPoint()->setX(slotPivotPoint->getX());
//
//				renderEffects_->getRotationOperation(0)->getPivotPoint()->setY(slotPivotPoint->getY());
//
//				renderEffects_->getRotationOperation(1)->setAngle(stageRotationOperation->getAngle());
//
//				// The stage pivot point is in a coordinate space whose origin is at the center of the stage. That is, point (0, 0) is
//				// located at the center of the stage. This must be converted to the same coordinate space as the slot pivot point.
//
//				float coordinateSpaceChangeDeltaX = stageMidpointX - slotAnimationMidpointX;
//
//				float coordinateSpaceChangeDeltaY = stageMidpointY - slotAnimationMidpointY;
//
//				int stagePivotX = stagePivotPoint->getX() + coordinateSpaceChangeDeltaX;
//
//				int stagePivotY = stagePivotPoint->getY() + coordinateSpaceChangeDeltaY;
//
//				int size = animation->frames_.size();
//
//				if (currentFrame >= 0 && currentFrame < size)
//				{
//					// Get the frame data.
//					boost::shared_ptr<AnimationFrame> frame = animation->frames_[currentFrame];
//
//					// If the sprite's associated look vector is looking left, flip the image horizontally.
//					bool flipHorizontally = false;
//
//					//if (dynamicsController_ != nullptr)
//					//{
//					//	if (dynamicsController_->look_->x_ < 0.0)
//					//	{
//					//		flipHorizontally = true;
//					//	}
//					//}
//
//					if (flipHorizontally == true)
//					{
//						// Mirror the pivot points if necessary. Pivot points are in a different coordinate space where the origin is the center.
//						// Therefore, mirroring is as simple as multiplying by -1.
//						renderEffects_->getRotationOperation(0)->getPivotPoint()->setX(slotPivotPoint->getX() * -1);
//
//						stagePivotX *= -1;
//					}
//
//					renderEffects_->getRotationOperation(1)->getPivotPoint()->setX(stagePivotX);
//					renderEffects_->getRotationOperation(1)->getPivotPoint()->setY(stagePivotY);
//
//					// Factor in the animation position.
//					int finalDrawAtX = drawAtX + animationPositionX;
//					int finalDrawAtY = drawAtY + animationPositionY;
//
//					// In terms of rendering, mirroring flips the animation itself. The animation slot position itself is changed dynamically in the state machine logic.
//					// Thus setting this value in the stage and slot render effects is irrelevant.
//					renderEffects_->setMirrorHorizontal(flipHorizontally);
//
//					int col = frame->spriteSheetCellColumn_;
//					int row = frame->spriteSheetCellRow_;
//
//					renderEffects_->setExtentLeft(slotRenderEffects->getExtentLeft());
//					renderEffects_->setExtentTop(slotRenderEffects->getExtentTop());
//					renderEffects_->setExtentRight(slotRenderEffects->getExtentRight());
//					renderEffects_->setExtentBottom(slotRenderEffects->getExtentBottom());
//
//					renderEffects_->setAlphaGradientDirection(slotRenderEffects->getAlphaGradientDirection());
//					renderEffects_->setAlphaGradientFrom(slotRenderEffects->getAlphaGradientFrom());
//					renderEffects_->setAlphaGradientTo(slotRenderEffects->getAlphaGradientTo());
//
//					// When this animation frame is rendered, the fade origin point must be transformed from relative to the center of the stage,
//					// to relative to the animation. 
//					// Do I need to subtract the animation offset?
//					int originPointXRelativeToAnimation = slotRenderEffects->getAlphaGradientRadialCenterPoint()->getX() + stageMidpointX;
//					int originPointYRelativeToAnimation = slotRenderEffects->getAlphaGradientRadialCenterPoint()->getY() + stageMidpointY;
//
//					renderEffects_->getAlphaGradientRadialCenterPoint()->setX(originPointXRelativeToAnimation);
//					renderEffects_->getAlphaGradientRadialCenterPoint()->setY(originPointYRelativeToAnimation);
//
//					float alphaGradientRadius = slotRenderEffects->getAlphaGradientRadius();
//
//					renderEffects_->setAlphaGradientRadius(alphaGradientRadius);
//
//					boost::shared_ptr<AlphaMask> alphaMask = renderEffects_->getAlphaMask();
//
//					alphaMask->setSheetId(animation->getAlphaMaskSheetId());
//					alphaMask->setSheetCellColumn(frame->alphaMaskSheetCellColumn_);
//					alphaMask->setSheetCellRow(frame->alphaMaskSheetCellRow_);
//
//					renderer_->renderSheetCell(finalDrawAtX, finalDrawAtY, sheetId, col, row, renderEffects_);
//				}
//			}
//		}
//
//		rendered(spritePositionX, spritePositionY);
//	}
//}
