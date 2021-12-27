#include "..\..\Headers\EngineCore\TileRenderable.hpp"

using namespace firemelon;

TileRenderable::TileRenderable()
{
	height_ = 16;
	width_ = 16;
	regionIndex_ = 0;
	tileSheetId_ = 0;
	tileSheetColumn_ = 0;
	tileSheetRow_ = 0;
	position_ = nullptr;
	layerPosition_ = nullptr;
	updateInterval_ = 1.0 / 4.0;
	updateTimer_ = 0.0;
	useRegion_ = false;
}

TileRenderable::~TileRenderable()
{
}

void TileRenderable::initializeRenderable()
{
	renderer_ = getRenderer();
	position_ = getPosition();
	layerPosition_ = getLayerPosition();
}

void TileRenderable::render(double lerp)
{
	// Calculate the screen location.

	// Tile's don't move so why would their position get LERPed? Was there a reason for this?
	//int spritePositionX = position_->getPreviousX() + (lerp * (position_->getX() - position_->getPreviousX()));
	//int spritePositionY = position_->getPreviousY() + (lerp * (position_->getY() - position_->getPreviousY()));
	
	// To answer the previous question, the camera moves, and so the layerPosition must get LERPed, but not the tile object position.
	int layerPositionX = layerPosition_->getPreviousX() + (lerp * (layerPosition_->getX() - layerPosition_->getPreviousXForRender()));

	int layerPositionY = layerPosition_->getPreviousY() + (lerp * (layerPosition_->getY() - layerPosition_->getPreviousYForRender()));
	
	int drawAtX = layerPositionX + position_->getX();

	int drawAtY = layerPositionY + position_->getY();

	if (useRegion_ == false)
	{
		renderer_->renderSheetCell(drawAtX, drawAtY, tileSheetId_, tileSheetColumn_, tileSheetRow_);
	}
	else
	{
		// TODO: Update here to render the correct region index. findmetodo
		renderer_->renderSheetSection(drawAtX, drawAtY, tileSheetId_, tileSheetRegions_[regionIndex_]);
	}
}

void TileRenderable::setHeight(int height)
{
	height_ = (int)(height * getScaleFactor());
}

void TileRenderable::setWidth(int width)
{
	width_ = (int)(width * getScaleFactor());
}

void TileRenderable::setTileSheetId(int tileSheetId)
{
	tileSheetId_ = tileSheetId;
}

void TileRenderable::setTileSheetRow(int tileSheetRow)
{
	tileSheetRow_ = tileSheetRow;
}

void TileRenderable::setTileSheetColumn(int tileSheetColumn)
{
	tileSheetColumn_ = tileSheetColumn;
}

void TileRenderable::setTileSheetRegions(std::vector<Rect> regions)
{
	tileSheetRegions_ = regions;

	float scaleFactor = getScaleFactor();

	tileSheetRegionsScaled_.clear();

	for (size_t i = 0; i < tileSheetRegions_.size(); i++)
	{
		Rect unscaledRegion = tileSheetRegions_[i];

		Rect scaledRegion;

		scaledRegion.x = unscaledRegion.x;

		scaledRegion.y = unscaledRegion.y;

		scaledRegion.w = (int)(unscaledRegion.w * scaleFactor);

		scaledRegion.h = (int)(unscaledRegion.h * scaleFactor);

		tileSheetRegionsScaled_.push_back(scaledRegion);
	}

	useRegion_ = true;
}

void TileRenderable::setFramesPerSecond(int framesPerSecond)
{
	updateInterval_ = 1.0 / framesPerSecond;
}

void TileRenderable::updateRenderable(double time)
{
	updateTimer_ += time;

	if (updateTimer_ >= updateInterval_)
	{
		updateTimer_ = 0.0;

		regionIndex_++;

		if (regionIndex_ >= tileSheetRegions_.size())
		{
			regionIndex_ = 0;
		}
	}
}

unsigned int TileRenderable::getHeight()
{
	if (useRegion_ == true)
	{
		return tileSheetRegionsScaled_[0].h;
	}
	else
	{
		return height_;
	}
}

unsigned int TileRenderable::getWidth()
{
	if (useRegion_ == true)
	{
		return tileSheetRegionsScaled_[0].w;
	}
	else
	{
		return width_;
	}
}

int TileRenderable::getX()
{
	return position_->getX();
}

int TileRenderable::getY()
{
	return position_->getY();
}

//bool TileRenderable::canReplicate()
//{
//	return false;
//}
//
//void TileRenderable::populateReplicationState(EntityReplicationState* replicationState)
//{
//	// Do nothing. This shouldn't even be required.
//}