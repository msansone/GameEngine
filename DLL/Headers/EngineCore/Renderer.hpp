/* -------------------------------------------------------------------------
** Renderer.hpp
**
** The Renderer class is the generic parent class that the user should inherit
** from to implement any specific renderers, using the graphics library of their
** choice. It is responsible for loading, storing, and displaying sprite sheet 
** surfaces on the screen. Also it can render filled or outlined rectangles.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _RENDERER_HPP_
#define _RENDERER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "BaseIds.hpp"
#include "Types.hpp"
#include "RenderEffects.hpp"
#include "SpriteSheet.hpp"

namespace firemelon
{
	class FIREMELONAPI Renderer 
	{
	public:
		friend class Assets;

		Renderer();
		virtual ~Renderer();
	
		virtual bool			initializeScreen() { assert(false); return false; }; // = 0; // This should be abstract, but VS2015 breaks boost python for some reason, and this is now needed as a workaround.

		virtual bool			initializeShader() { assert(false); return false; }; // = 0; // This should be abstract, but VS2015 breaks boost python for some reason, and this is now needed as a workaround.

		virtual void			sceneBegin() { assert(false); }; // = 0; // Likewise with this, as well as other methods in this class. (The reason is because if you see in RoomManager.cpp, below the module def, I need to define the get_pointer for all classes used, as a bug workaround. However, there is one case where this workaround fails, and that is for abstract classes.
		
		virtual void			sceneComplete() { assert(false); }; // = 0;

		// Draw/Fill basic gemoetry.
		void					drawPolygonPy(std::vector<Vertex2> vertices, ColorRgbaPtr color);

		virtual void			drawPolygon(std::vector<Vertex2> vertices, ColorRgbaPtr color) { assert(false); };

		void					drawRectPy(float x,   float y,     int   width, int   height, 
									       float red, float green, float blue,  float alpha);

		virtual void			drawRect  (float x,   float y,     int   width, int  height, 
										   float red, float green, float blue,  float alpha) { assert(false); }; // = 0;
		
		void					fillRectPy(float x,   float y,     int   width, int   height, 
									       float red, float green, float blue,  float alpha);

		virtual void			fillRect  (float x,   float y,     int   width, int   height, 
										   float red, float green, float blue,  float alpha) { assert(false); }; // = 0;	
		
		// Load an image from a file into a sprite sheet.
		virtual int				loadSpriteSheet(std::string sheetName,  std::string fileName,  int   rows,        int  cols, 
												int         cellHeight, int         cellWidth, float scaleFactor, bool useTransparencyKey,
												int		    padding) 
								{ assert(false); return -1; }; // = 0;

		// Load an image from a memory stream into a sprite sheet.
		virtual int				loadSpriteSheet(std::string sheetname,  char* buffer,    int   filesize,    int rows,               int cols, 
												int         cellHeight, int   cellWidth, float scaleFactor, bool useTransparencyKey,
												int			padding) 
								{ assert(false); return -1; }; // = 0;

		// Renders the sprite sheet with the given ID at the given x, y location.
		void					renderSheetByIdPy(float x, float y, int sheetId);
		virtual void			renderSheetById(float x, float y, int sheetId) { assert(false); }; // = 0;

		void					renderSheetByIdPy(float x, float y, int sheetId, boost::shared_ptr<RenderEffects> renderEffects);
		virtual void			renderSheetById(float x, float y, int sheetId, boost::shared_ptr<RenderEffects> renderEffects) { assert(false); }; // = 0;
		
		// Renders a subsection of the sprite sheet from the given source rect.
		// This would be used when you have a sheet that has elements that aren't aligned to a grid of rows and columns.
		void					renderSheetSectionPy(float x, float y, int sheetId, Rect source);
		virtual void			renderSheetSection  (float x, float y, int sheetId, Rect source) { assert(false); }; // = 0;

		// Renders a subsection of the sprite sheet from the given source rect with the specified render effects.
		// This would be used when you have a sheet that has elements that aren't aligned to a grid of rows and columns.
		void					renderSheetSectionPy(float x, float y, int sheetId, Rect source, boost::shared_ptr<RenderEffects> renderEffects);
		virtual void			renderSheetSection  (float x, float y, int sheetId, Rect source, boost::shared_ptr<RenderEffects> renderEffects) { assert(false); }; // = 0;
		
		// Renders a cell from the sprite sheet at the given x, y location.
		void					renderSheetCellPy(float x, float y, int sheetId, int sourceX, int sourceY);
		virtual void			renderSheetCell  (float x, float y, int sheetId, int sourceX, int sourceY) { assert(false); }; // = 0;

		// Renders a cell from the sprite sheet at the given x, y location with the specified render effects.
		void					renderSheetCellPy(float x, float y, int sheetId, int sourceX, int sourceY, boost::shared_ptr<RenderEffects> renderEffects);
		virtual void			renderSheetCell  (float x, float y, int sheetId, int sourceX, int sourceY, boost::shared_ptr<RenderEffects> renderEffects) { assert(false); }; // = 0;
		
		// Render a single cell from a sprite sheet into a quad.
		// This would be used when you want to perform all transforms manually.
		virtual void			renderSheetCell(Quad quad, int sheetId, int sourceX, int sourceY, boost::shared_ptr<RenderEffects> renderEffects) { assert(false); }; // = 0;

		virtual void            saveScreenshot(std::string fileName);

		// Fill the screen with a color.
		virtual void			fillScreen(unsigned int color) { assert(false); }; // = 0;

		// Sets whether the fade color will be applied to anything rendered.
		void					setEnableFade(bool enableFade);
		void					setEnableFadePy(bool enableFade);

		bool					getEnableFade();
		bool					getEnableFadePy();

		// Set the RGB values (between 0.0 and 1.0) for the fade out color.
		void					setFadeColorPy(float red, float green, float blue);
		virtual void			setFadeColor(float red, float green, float blue);

		// The opacity value ranges from 0.0 to 1.0.
		void					setFadeOpacityPy(float opacity);
		virtual void			setFadeOpacity(float opacity);

		float					getFadeOpacityPy();
		virtual float			getFadeOpacity();

		SpriteSheetPtr			getSheetPy(int sheetId);
		virtual SpriteSheetPtr	getSheet(int sheetId) { assert(false);  return nullptr;  }; // = 0;
		
		SpriteSheetPtr			getSheetByNamePy(std::string sheetName);
		virtual SpriteSheetPtr	getSheetByName(std::string sheetName) { assert(false); return nullptr;  }; // = 0;
		
		int						getSheetIDByNamePy(std::string sheetName);
		virtual int				getSheetIDByName(std::string sheetName) { assert(false); return -1; }; // = 0;

		int						getScreenHeightPy();
		virtual int				getScreenHeight() { assert(false); return -1; }; // = 0;

		int						getScreenWidthPy();
		virtual int				getScreenWidth() { assert(false); return -1; }; // = 0;

		void					setScreenSizePy(int width, int height);
		virtual void			setScreenSize(int width, int height) { assert(false); }; // = 0;

		int						translateSpriteSheetId(AssetId editorSpriteSheetId);

	protected:
		
		// Method that is called by the engine when all sprite sheets have finished loading.
		virtual void								sheetsLoaded(){};
		
	private:

		bool										enableFade_;

		// List of sprite sheet entitys. Describes a surface.
		std::vector<boost::shared_ptr<SpriteSheet>>	sheets_;	
	
		std::map<std::string, int>					sheetNameIdMap_;

		// Map the sprite sheet ID from the room file to the surface ID in the renderer. 
		// This is because the ID stored in the renderer is not guaranteed to be the 
		// same as the the ID generated by the engine.
		std::map<AssetId, int>						spritesheetIdMap_;
	};

	typedef boost::shared_ptr<Renderer> RendererPtr;
}

#endif // _RENDERER_HPP_