/* -------------------------------------------------------------------------
** SdlRenderer.hpp
** 
** The SdlRenderer class is derived from the Renderer class. It implements the
** functions for initializing the screen and displaying surfaces on it using
** the SDL library.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SDLRENDERER_HPP_
#define _SDLRENDERER_HPP_

#include <vector>
#include <map>

#include "SDL.h"
#include "SDL_image.h"

#include <Renderer.hpp>
#include "SdlSpriteSheet.hpp"

class SdlRenderer : public firemelon::Renderer
{
public:

	struct SdlColor
	{
		Uint8 r;
		Uint8 g;
		Uint8 b;
		Uint8 a;
	};

	SdlRenderer(int screenWidth, int screenHeight);
	virtual ~SdlRenderer();
	
	// Set up the screen.
	virtual bool					initializeScreen();

	// Load an image from a file into a surface. Returns the surface ID.
	virtual int						loadSpriteSheet(std::string sheetname, std::string filename, int rows, 
													int cols, int cellHeight, int cellWidth, 
													float scaleFactor, bool useTransparencyKey, int padding);

	// Load an image from a memory stream into a surface. Returns the surface ID.
	virtual int						loadSpriteSheet(std::string sheetname, char* buffer, int filesize, 
													int rows, int cols, int cellHeight, int cellWidth, 
													float scaleFactor, bool useTransparencyKey, int padding);
	
	// Renders the sheet with the given ID.
	virtual void					renderSheetById(float x, float y, int sheetID, bool flipHorizontally);
	virtual void					renderSheetById(float x, float y, int sheetID, bool flipHorizontally,
		float red,
		float green,
		float blue,
		float alpha,
		float rotationAngle);
	
	// Renders the sheet passed in.
	virtual void					renderSheet(float x, float y, boost::shared_ptr<firemelon::SpriteSheet> sheet,
		bool flipHorizontally,
		float red,
		float green,
		float blue,
		float alpha,
		float rotationAngle);

	virtual void					renderSheet(float x, float y, boost::shared_ptr<firemelon::SpriteSheet> sheet, bool flipHorizontally);
	
	// Renders a subsection of the sheet from the given source rect.
	virtual void					renderSheetByIdFromSourceRect(float x, float y, int sheetID, firemelon::Rect source,
		bool flipHorizontally,
		float red,
		float green,
		float blue,
		float alpha,
		float rotationAngle);

	virtual void					renderSheetByIdFromSourceRect(float x, float y, int sheetID, firemelon::Rect source, bool flipHorizontally);

	// Renders a sheet cell from the surface at the given x, y location.
	virtual void					renderSheetByIdFromSourceCell(float x, float y, int sheetID, int sourceX, int sourceY, bool flipHorizontally);
	virtual void					renderSheetByIdFromSourceCell(float x, float y, int sheetID, int sourceX, int sourceY,
		bool flipHorizontally,
		float red,
		float green,
		float blue,
		float alpha,
		float rotationAngle);

	virtual void					renderDrawRect(float x, float y, int width, int height_, unsigned int color);
	virtual void					renderFillRect(float x, float y, int width, int height_, unsigned int color);

	// Fill the screen with a color.
	virtual void					fillScreen(unsigned int color);
	virtual void					sceneBegin();
	virtual void					sceneComplete();
	SdlColor						convertIntToColor(unsigned int color);
	
	virtual boost::shared_ptr<firemelon::SpriteSheet>	getSheet(int sheetID);
	virtual boost::shared_ptr<firemelon::SpriteSheet>	getSheetByName(std::string sheetname);
	virtual int						getSheetIDByName(std::string sheetname);

	SDL_Surface*					getScreen();
	
	void							setIsFullscreen(bool value);
	bool							getIsFullscreen();
	
	virtual void					setScreenSize(int width, int height);
	virtual int						getScreenHeight();

	virtual int						getScreenWidth();

protected:

private:

	std::vector<boost::shared_ptr<firemelon::SpriteSheet>>	sheets_;
	
	std::map<std::string, int>				sheetNameIDMap_;

	SDL_Window*								window_;
	SDL_Surface*							screen_;
	SDL_Renderer*							sdlRenderer_;
	
	int										screenWidth_;
	int										screenHeight_;
	bool									isFullscreen_;
};


#endif // _SDLRENDERER_HPP_