#include "..\..\Headers\EngineCore\ParticleRenderable.hpp"

using namespace firemelon;

ParticleRenderable::ParticleRenderable()
{
	spriteSheetIndex_ = -1;
	previousFrameIndex_ = -1;
	frameSourceX_ = -1;
	frameSourceY_ = -1;
	height_ = 0;
	width_ = 0;

	animationPlayer_ = boost::shared_ptr<AnimationPlayer>(new AnimationPlayer(60, ANIMATION_STYLE_REPEAT));

	linearAlgebraUtility_ = boost::make_shared<LinearAlgebraUtility>(LinearAlgebraUtility());

	currentFrame_ = nullptr;
	
	layerPosition_ = nullptr;
	
	for (int i = 0; i < 4; i++)
	{
		Vertex2 vertex;

		vertex.x = 0;

		vertex.y = 0;

		nativeCorners_.push_back(vertex);

		Vertex2 vertexTransformed;

		vertexTransformed.x = 0;

		vertexTransformed.y = 0;

		transformedCorners_.push_back(vertexTransformed);
	}
}

ParticleRenderable::~ParticleRenderable()
{
}

void ParticleRenderable::initializeRenderable()
{
	renderer_ = getRenderer();
	position_ = getPosition();
	layerPosition_ = getLayerPosition();

	//if (dynamicsController_ != nullptr)
	//{
	//	dynamicsController_->setPosition(position_);
	//	dynamicsController_->initialize();
	//	dynamicsController_->setOwnerId(particleId_);
	//}
	//else
	//{
	//	std::cout << "DynamicsController not initialized in particle renderable " << getRenderableId() << "." << std::endl;
	//}
}

void ParticleRenderable::render(double lerp)
{
	if (animation_ != nullptr)
	{
		if (metadata_->reapplyTransform_ == true)
		{
			metadata_->reapplyTransform_ = false;

			applyTransforms(lerp);
		}

		int currentFrameIndex = animationPlayer_->getCurrentFrame();

		if (currentFrameIndex > -1)
		{
			if (currentFrameIndex != previousFrameIndex_)
			{
				// Get the frame data.
				currentFrame_ = animation_->getFrame(currentFrameIndex);
				previousFrameIndex_ = currentFrameIndex;

				// Determine the source rectangle position.
				frameSourceX_ = currentFrame_->getSpriteSheetCellColumn();
				frameSourceY_ = currentFrame_->getSpriteSheetCellRow();
			}
		}
		else
		{
			previousFrameIndex_ = -1;
			currentFrame_ = nullptr;
		}
	}

	// Calculate the screen location.			
	int previousX = position_->getPreviousXForRender();
	int previousY = position_->getPreviousYForRender();

	int currentX = position_->getX();
	int currentY = position_->getY();

	int spritePositionX = previousX + (lerp * (currentX - previousX));
	int spritePositionY = previousY + (lerp * (currentY - previousY));

	int previousLayerX = layerPosition_->getPreviousXForRender();
	int previousLayerY = layerPosition_->getPreviousYForRender();

	int currentLayerX = layerPosition_->getX();
	int currentLayerY = layerPosition_->getY();

	int layerPositionX = previousLayerX + (lerp * (currentLayerX - previousLayerX));
	int layerPositionY = previousLayerY + (lerp * (currentLayerY - previousLayerY));

	int drawAtX = layerPositionX + spritePositionX;
	int drawAtY = layerPositionY + spritePositionY;

	if (currentFrame_ != nullptr)
	{
		Quad quad;

		quad.vertices[0].x = drawAtX + transformedCorners_[0].x;
		quad.vertices[0].y = drawAtY + transformedCorners_[0].y;

		quad.vertices[1].x = drawAtX + transformedCorners_[1].x;
		quad.vertices[1].y = drawAtY + transformedCorners_[1].y;

		quad.vertices[2].x = drawAtX + transformedCorners_[2].x;
		quad.vertices[2].y = drawAtY + transformedCorners_[2].y;

		quad.vertices[3].x = drawAtX + transformedCorners_[3].x;
		quad.vertices[3].y = drawAtY + transformedCorners_[3].y;

		renderer_->renderSheetCell(quad, spriteSheetIndex_, frameSourceX_, frameSourceY_, getRenderEffects());
	}

	rendered(drawAtX, drawAtY);
}

unsigned int ParticleRenderable::getHeight()
{
	return height_;
}

unsigned int ParticleRenderable::getWidth()
{
	return width_;
}

int ParticleRenderable::getX()
{
	return position_->getX();
}

int ParticleRenderable::getY()
{
	return position_->getY();
}

void ParticleRenderable::reset()
{
	animationPlayer_->reset();
}

void ParticleRenderable::updateRenderable(double time)
{
	if (animation_ != nullptr)
	{
		animationPlayer_->updateAnimation(animation_, time);
	}
}

void ParticleRenderable::setAnimation(boost::shared_ptr<Animation> animation, int framesPerSecond)
{
	boost::shared_ptr<Renderer> renderer = getRenderer();

	animation_ = animation;

	animationPlayer_->setFramesPerSecond(framesPerSecond);

	spriteSheetIndex_ = animation_->getSpriteSheetId();

	boost::shared_ptr<SpriteSheet> sheet = renderer->getSheet(spriteSheetIndex_);

	height_ = ((int)(sheet->getCellHeight() * sheet->getScaleFactor()));
	width_ = ((int)(sheet->getCellHeight() * sheet->getScaleFactor()));

	metadata_->height_ = height_;
	metadata_->width_ = width_;
	
	setCornerPoints();
}


void ParticleRenderable::setMetadata(ParticleMetadataPtr metadata)
{
	metadata_ = metadata;
}

bool ParticleRenderable::getIsDynamic()
{
	return true;
}

ColorRgbaPtr ParticleRenderable::getHueColor()
{
	return getRenderEffects()->getHueColor();
}

void ParticleRenderable::applyTransforms(double lerp)
{
	// Initialize the native corners, and apply scaling.
	setCornerPoints();

	applyRotationTransforms(lerp);
}

void ParticleRenderable::applyRotationTransforms(double lerp)
{
	// No pivot point at the moment. This will need to change.
	//PositionPtr stagePivotPoint = stageMetadata_->getRotationOperation()->getPivotPoint();

	float rotationAngle = getRenderEffects()->getRotationOperation(0)->getAngle();

	// Haven't figured out the best way to interpolate yet, given that angles wrap around at 360.
	//float previousStageRotationAngle = stageMetadata_->getRotationOperation()->previousAngle_;

	//float previousPreviousStageRotationAngle = stageMetadata_->getRotationOperation()->previousPreviousAngle_;

	//// Interpolate the rotation angle.
	//if (stageMetadata_->renderEffects_->getInterpolateRotation() == true)
	//{
	//	stageRotationAngle = interpolateAngle(stageRotationAngle, previousStageRotationAngle, previousPreviousStageRotationAngle, lerp);
	//}

	// Determine the value by which all the vertices must be shifted to the screen origin. Currently none because there is no pivot point. 
	float translationVectorX = 0; // stagePivotPoint->getX();

	float translationVectorY = 0; // stagePivotPoint->getY();

	linearAlgebraUtility_->rotatePoints(rotationAngle, nativeCorners_, transformedCorners_, translationVectorX, translationVectorY);
}


void ParticleRenderable::setCornerPoints()
{
	// Potential issue: Does this need to add the border padding like the AnimationSlot object does in its setCornerPoints function?

	// Set the corner points, (clockwise order, starting with top left).

	// 0------1
	// -      -
	// -      -
	// 3------2

	int cornerLeft = 0;

	int cornerTop = 0;

	int width = width_ * renderEffects_->getScaleFactor();

	int height = height_ * renderEffects_->getScaleFactor();
	
	int halfWidth = width / 2;

	int halfHeight = height / 2;

	// Set the animation corner points in native TLC space.
	switch (metadata_->origin_)
	{
	case PARTICLE_ORIGIN_TOP_LEFT:

		// This is native space.
		cornerLeft = 0;

		cornerTop = 0;

		break;

	case PARTICLE_ORIGIN_TOP_MIDDLE:

		cornerLeft = 0 - halfWidth;

		cornerTop = 0;

		break;

	case PARTICLE_ORIGIN_TOP_RIGHT:

		cornerLeft = 0 - width;

		cornerTop = 0;

		break;

	case PARTICLE_ORIGIN_MIDDLE_LEFT:

		cornerLeft = 0;

		cornerTop = 0 - halfHeight;

		break;

	case PARTICLE_ORIGIN_CENTER:

		cornerLeft = 0 - halfWidth;

		cornerTop = 0 - halfHeight;

		break;

	case PARTICLE_ORIGIN_MIDDLE_RIGHT:

		cornerLeft = 0 - width;

		cornerTop = 0 - halfHeight;

		break;

	case PARTICLE_ORIGIN_BOTTOM_LEFT:

		cornerLeft = 0;

		cornerTop = 0 - height;

		break;

	case PARTICLE_ORIGIN_BOTTOM_MIDDLE:

		cornerLeft = 0 - halfWidth;

		cornerTop = 0 - height;

		break;


	case PARTICLE_ORIGIN_BOTTOM_RIGHT:

		cornerLeft = 0 - width;

		cornerTop = 0 - height;

		break;

	default:

		break;
	}

	nativeCorners_[0].x = cornerLeft;
	nativeCorners_[0].y = cornerTop;

	nativeCorners_[1].x = cornerLeft + width;
	nativeCorners_[1].y = cornerTop;

	nativeCorners_[2].x = cornerLeft + width;
	nativeCorners_[2].y = cornerTop + height;

	nativeCorners_[3].x = cornerLeft;
	nativeCorners_[3].y = cornerTop + height;


	transformedCorners_[0].x = nativeCorners_[0].x;
	transformedCorners_[0].y = nativeCorners_[0].y;

	transformedCorners_[1].x = nativeCorners_[1].x;
	transformedCorners_[1].y = nativeCorners_[1].y;

	transformedCorners_[2].x = nativeCorners_[2].x;
	transformedCorners_[2].y = nativeCorners_[2].y;

	transformedCorners_[3].x = nativeCorners_[3].x;
	transformedCorners_[3].y = nativeCorners_[3].y;
}