#include "..\Headers\SdlRenderer.hpp"

using namespace firemelon;

SdlRenderer::SdlRenderer(int screenWidth, int screenHeight) : Renderer()
{
	window_ = NULL;
	
	screenWidth_ = screenWidth;
	screenHeight_ = screenHeight;
	isFullscreen_ = false;
}

SdlRenderer::~SdlRenderer()
{
	//Destroy the SDL rendering objects.
	SDL_DestroyRenderer(sdlRenderer_);
    SDL_DestroyWindow(window_);

    IMG_Quit();
}

int SdlRenderer::loadSpriteSheet(std::string sheetname, std::string filename, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor, bool useTransparencyKey, int padding)
{	
	// Load an sprite sheet from an external image file.

	//The final texture
    SDL_Texture* newTexture = NULL;

    //Load image at specified path
    SDL_Surface* loadedSurface =  IMG_Load(filename.c_str());
	
	if (loadedSurface == NULL)
    {
        std::cout<<"Image load failed for image "<<filename<<" with error "<<IMG_GetError()<<std::endl;

		return -1;
    }
	else
	{
		if (useTransparencyKey == true)
		{
			//Set the transparency key 
			Uint32 colorkey_ = SDL_MapRGB(loadedSurface->format, 0xFF, 0, 0xFF);
			SDL_SetColorKey(loadedSurface, SDL_TRUE, colorkey_);
		}

		//Create texture from surface pixels
        newTexture = SDL_CreateTextureFromSurface(sdlRenderer_, loadedSurface);

		//Free the old image 
		SDL_FreeSurface(loadedSurface);

		if (newTexture  == NULL)
		{
			std::cout<<"Unable to create texture from image "<<filename<< ". Error "<<SDL_GetError()<<std::endl;

			return -1;
		}
		else
		{
			boost::shared_ptr<SdlSpriteSheet> newSheet = boost::shared_ptr<SdlSpriteSheet>(new SdlSpriteSheet(sheetname, rows, cols, cellHeight, cellWidth, 1.0f));

			newSheet->setSurface(newTexture);

			sheets_.push_back(newSheet);

			sheetNameIDMap_[sheetname] = sheets_.size() - 1;

		}
	
		return sheets_.size() - 1;
	}
}

int SdlRenderer::loadSpriteSheet(std::string sheetname, char* buffer, int filesize, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor, bool useTransparencyKey, int padding)
{
	//Load the buffer into a surface using RWops
	SDL_RWops* rw = SDL_RWFromMem(buffer, filesize);

	if (rw == NULL)
	{
		return -1;
	}

	SDL_Surface* surfaceTemp = IMG_Load_RW(rw, 1);

	if (surfaceTemp == NULL)
	{
		// Couldn't load surface.
		std::string err = SDL_GetError();
		return -1;
	}

	if (useTransparencyKey == true)
	{
		//Set the transparency key 
		Uint32 colorkey_ = SDL_MapRGB(surfaceTemp->format, 0xFF, 0, 0xFF);
		SDL_SetColorKey(surfaceTemp, SDL_TRUE, colorkey_);
	}

	//Create texture from surface pixels
    SDL_Texture* newTexture = SDL_CreateTextureFromSurface(sdlRenderer_, surfaceTemp);

	//Free the old image 
	SDL_FreeSurface(surfaceTemp);
	
	if (newTexture == NULL)
	{
		// Couldn't load surface.
		std::cout<<"Unable to create texture from surface. Error "<<SDL_GetError()<<std::endl;

		return -1;
	}

	boost::shared_ptr<SdlSpriteSheet> newSheet = boost::shared_ptr<SdlSpriteSheet>(new SdlSpriteSheet(sheetname, rows, cols, cellHeight, cellWidth, 1.0f));

	newSheet->setSurface(newTexture);

	sheets_.push_back(newSheet);
	
	sheetNameIDMap_[sheetname] = sheets_.size() - 1;

	// Return the index of this sheet, which will be used later to render it.
	return sheets_.size() - 1;
}

void SdlRenderer::renderSheetById(float x, float y, int sheetID, bool flipHorizontally,
	float red,
	float green,
	float blue,
	float alpha,
	float rotationAngle)
{
	renderSheetById(x, y, sheetID, flipHorizontally);
}

void SdlRenderer::renderSheetById(float x, float y, int sheetID, bool flipHorizontally)
{
	// Temporary rectangle to hold the offsets 
	SDL_Rect offset; 
	offset.x = x; 
	offset.y = y;

	//Blit the surface 
	boost::shared_ptr<SdlSpriteSheet> sdlSheet = boost::static_pointer_cast<SdlSpriteSheet>(sheets_[sheetID]);
	SDL_Texture* texture = sdlSheet->getSurface();
	
	offset.w = sdlSheet->getCellWidth();
	offset.h = sdlSheet->getCellHeight();

	SDL_RenderCopy(sdlRenderer_, texture, nullptr, &offset);

	//SDL_BlitSurface(s, nullptr, screen_, &offset);
}

void SdlRenderer::renderSheet(float x, float y, boost::shared_ptr<SpriteSheet> sheet, bool flipHorizontally,
	float red,
	float green,
	float blue,
	float alpha,
	float rotationAngle)
{
	renderSheet(x, y, sheet, flipHorizontally);
}

void SdlRenderer::renderSheet(float x, float y, boost::shared_ptr<SpriteSheet> sheet, bool flipHorizontally)
{
	// Temporary rectangle to hold the offsets 
	SDL_Rect offset; 
	offset.x = x; 
	offset.y = y;

	boost::shared_ptr<SdlSpriteSheet> sdlSheet = boost::static_pointer_cast<SdlSpriteSheet>(sheet);
	
	SDL_Texture* texture = sdlSheet->getSurface();
	
	offset.w = sdlSheet->getCellWidth();
	offset.h = sdlSheet->getCellHeight();

	//Blit the surface 
	SDL_RenderCopy(sdlRenderer_, texture, nullptr, &offset);

	//SDL_BlitSurface(sdlSheet->getSurface(), nullptr, screen_, &offset);
}

void SdlRenderer::renderSheetByIdFromSourceRect(float x, float y, int sheetID, Rect source,	bool flipHorizontally,
	float red,
	float green,
	float blue,
	float alpha,
	float rotationAngle)
{
	renderSheetByIdFromSourceRect(x, y, sheetID, source, flipHorizontally);
}

void SdlRenderer::renderSheetByIdFromSourceRect(float x, float y, int sheetID, Rect source, bool flipHorizontally)
{
	// Temporary rectangle to hold the offsets 
	SDL_Rect offset; 
	offset.x = x; 
	offset.y = y;

	//Blit the surface
	boost::shared_ptr<SdlSpriteSheet> sdlSheet = boost::static_pointer_cast<SdlSpriteSheet>(sheets_[sheetID]);
	SDL_Texture* texture = sdlSheet->getSurface();

	SDL_Rect sdlSource;

	sdlSource.x = source.x;
	sdlSource.y = source.y;
	sdlSource.w = source.w;
	sdlSource.h = source.h;
	
	offset.w = source.w;
	offset.h = source.h;

	SDL_RenderCopy(sdlRenderer_, texture, &sdlSource, &offset);
	//SDL_BlitSurface(s, &sdlSource, screen_, &offset);
}

void SdlRenderer::renderSheetByIdFromSourceCell(float x, float y, int sheetID, int sourceX, int sourceY, bool flipHorizontally,
	float red,
	float green,
	float blue,
	float alpha,
	float rotationAngle)
{
	renderSheetByIdFromSourceCell(x, y, sheetID, sourceX, sourceY, flipHorizontally);
}

void SdlRenderer::renderSheetByIdFromSourceCell(float x, float y, int sheetID, int sourceX, int sourceY, bool flipHorizontally)
{
	// Temporary rectangle to hold the offsets 
	SDL_Rect offset; 
	offset.x = x; 
	offset.y = y;

	// Use the sprite sheet data to calculate the source rect.
	int rows = sheets_[sheetID]->getRows();
	int cols = sheets_[sheetID]->getColumns();

	int cellHeight = sheets_[sheetID]->getCellHeight();
	int cellWidth = sheets_[sheetID]->getCellWidth();

	SDL_Rect source;

	int srcX = sourceX;

	if (srcX > cols)
	{
		srcX = cols;
	}

	int srcY = sourceY;

	if (srcY > rows)
	{
		srcY = rows;
	}

	source.x = srcX * cellWidth;
	source.y = srcY * cellHeight;
	source.h = cellHeight;
	source.w = cellWidth;

	offset.w = source.w;
	offset.h = source.h;

	//Blit the surface
	boost::shared_ptr<SdlSpriteSheet> sdlSheet = boost::static_pointer_cast<SdlSpriteSheet>(sheets_[sheetID]);
	SDL_Texture* texture = sdlSheet->getSurface();
	
	SDL_RenderCopy(sdlRenderer_, texture, &source, &offset);
	//SDL_BlitSurface(s, &source, screen_, &offset);
}

void SdlRenderer::renderDrawRect(float x, float y, int width, int height, unsigned int color)
{
	SDL_Rect rect;
	rect.x = x;
	rect.y = y;
	rect.w = width;
	rect.h = height;

	SdlColor c = convertIntToColor(color);

	SDL_SetRenderDrawColor(sdlRenderer_, c.r, c.g, c.b, c.a);

	SDL_RenderDrawRect(sdlRenderer_, &rect);
}

void SdlRenderer::renderFillRect(float x, float y, int width, int height, unsigned int color)
{
	SDL_Rect rect;
	rect.x = x;
	rect.y = y;
	rect.w = width;
	rect.h = height;

	SdlColor c = convertIntToColor(color);

	SDL_SetRenderDrawColor(sdlRenderer_, c.r, c.g, c.b, c.a);

	SDL_RenderFillRect(sdlRenderer_, &rect);
}

boost::shared_ptr<SpriteSheet> SdlRenderer::getSheet(int sheetID)
{
	int size = sheets_.size();
	if (sheetID >= 0 && sheetID < size)
	{
		return sheets_[sheetID];
	}

	return nullptr;
}

void SdlRenderer::fillScreen(unsigned int color)
{
	SDL_FillRect(screen_, 0, color);
}

void SdlRenderer::sceneBegin()
{
	// Clear the screen.
	SDL_SetRenderDrawColor(sdlRenderer_, 0x00, 0x00, 0x00, 0x00);
	
	SDL_RenderClear(sdlRenderer_);
}

void SdlRenderer::sceneComplete()
{
	SDL_RenderPresent(sdlRenderer_);

	//screen_ = SDL_GetWindowSurface(window_);

	//SDL_UpdateWindowSurface(window_);
}

bool SdlRenderer::initializeScreen()
{
	//Create the SDL window
	window_ = SDL_CreateWindow("Firemelon Engine", 
							   SDL_WINDOWPOS_CENTERED, 
							   SDL_WINDOWPOS_CENTERED, 
							   screenWidth_, 
							   screenHeight_, 
							   SDL_WINDOW_SHOWN);

    if (window_ == NULL)
    {
		std::cout<<"Window creation failed with error: "<<SDL_GetError()<<std::endl;
    }

	SDL_ShowCursor(0);
	
	screen_ = SDL_GetWindowSurface(window_);
	
	if (screen_ == nullptr)
	{
		return false;
	}
	
	// Initialize PNG images,
	int imgFlags = IMG_INIT_PNG;

	bool success = IMG_Init(imgFlags) & imgFlags;

    if (success == false)
    {
        std::cout<<"SDL_image initialize failed with SDL_image Error: "<<IMG_GetError()<<std::endl;

		return -1;
    }

	//Create the renderer.
	sdlRenderer_ = SDL_CreateRenderer(window_, -1, SDL_RENDERER_ACCELERATED);

    if (sdlRenderer_ == nullptr)
    {
        std::cout<<"Renderer creation failed with error: "<<SDL_GetError()<<std::endl;
		
		return false;
    }
    else
    {
        //Initialize renderer color
        SDL_SetRenderDrawColor(sdlRenderer_, 0x00, 0x00, 0x00, 0x00);
    }

	return true;
}

int SdlRenderer::getSheetIDByName(std::string sheetname)
{
	return sheetNameIDMap_[sheetname];
}

boost::shared_ptr<SpriteSheet> SdlRenderer::getSheetByName(std::string sheetname)
{
	int sheetIndex = sheetNameIDMap_[sheetname];

	return sheets_[sheetIndex];
}

SDL_Surface* SdlRenderer::getScreen()
{
	return screen_;
}

SdlRenderer::SdlColor SdlRenderer::convertIntToColor(unsigned int color)
{
	SdlRenderer::SdlColor retColor;
	retColor.r = (color & 0xFF000000)>>24;
	retColor.g = (color & 0x00FF0000)>>16;
	retColor.b = (color & 0x0000FF00)>>8;
	retColor.a = (color & 0x000000FF);

	return retColor;
}

void SdlRenderer::setScreenSize(int width, int height)
{
	screenWidth_ = width;
	screenHeight_ = height;
}

int SdlRenderer::getScreenHeight()
{
	return screenHeight_;
}

int SdlRenderer::getScreenWidth()
{
	return screenWidth_;
}

void SdlRenderer::setIsFullscreen(bool value)
{
	isFullscreen_ = value;

	if (isFullscreen_ == true)
	{
		SDL_SetWindowFullscreen(window_, SDL_WINDOW_FULLSCREEN);
	}
	else
	{
		SDL_SetWindowFullscreen(window_, 0);
	}
}

bool SdlRenderer::getIsFullscreen()
{
	return isFullscreen_;
}
