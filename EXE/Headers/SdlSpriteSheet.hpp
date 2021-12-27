/* -------------------------------------------------------------------------
** SdlSpriteSheet.hpp
** 
** The SdlSpriteSheet class is derived from the SpriteSheet class. It is a
** wrapper around an SDL surface to be displayed on the screen. The renderer
** stores a list of sprite sheets that it initializes.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SDLSPRITESHEET_HPP_
#define _SDLSPRITESHEET_HPP_

#include <SpriteSheet.hpp>

#include "SDL.h"

#include <iostream>
#include <string>
#include <vector>

class SdlSpriteSheet : public firemelon::SpriteSheet
{
public:
	SdlSpriteSheet(std::string name, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor);
	virtual ~SdlSpriteSheet();

	virtual void	freeSheet();

	void			setSurface(SDL_Texture* surface);
	SDL_Texture*	getSurface();

private:
	
	SDL_Texture*	surface_;
};

#endif // _SDLSPRITESHEET_HPP_