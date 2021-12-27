#include "..\Headers\SdlSpriteSheet.hpp"

using namespace firemelon;

SdlSpriteSheet::SdlSpriteSheet(std::string name, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor) : SpriteSheet(name, rows, cols, cellHeight, cellWidth, scaleFactor)
{

}

SdlSpriteSheet::~SdlSpriteSheet()
{

}

void SdlSpriteSheet::freeSheet()
{
	SDL_DestroyTexture(surface_);
	surface_ = nullptr;
}

void SdlSpriteSheet::setSurface(SDL_Texture* surface)
{
	surface_ = surface;
}

SDL_Texture* SdlSpriteSheet::getSurface()
{
	return surface_;
}