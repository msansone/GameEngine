/* -------------------------------------------------------------------------
** BitmapFont.hpp
** 
** The BitmapFont class is a font that is built on top of a sprite sheet. The
** cells in the sprite sheet are mapped to an ASCII character value based on 
** their ordinal position. 
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _BITMAPFONT_HPP_
#define _BITMAPFONT_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <math.h>
#include <string>
#include <vector>

#include "Renderer.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI BitmapFont 
	{ 
	public: 
		friend class TextManager;

		BitmapFont(boost::shared_ptr<Renderer> renderer);
		virtual ~BitmapFont();
	
		// Initialize a font from a bitmap file.	
		void	initializeFont(std::string sheetname, 
							   std::string filename, 
							   int characterHeight, int characterWidth, 
							   int rows, int columns,
							   float scaleFactor);

		// Initialize a font from a sprite sheet.
		void	initializeFont(std::string fontsheetname);

		// Writes text as a single line to the given coordinates.
		void	writeTextPy(int x, int y, std::string text, double r, double g, double b, double a);
		void	writeText(int x, int y, std::string text, double r, double g, double b, double a);

		void	writeTextPy(float x, float y, std::string text, double r, double g, double b, double a);
		void	writeText(float x, float y, std::string text, double r, double g, double b, double a);

		void	writeTextPy(int x, int y, std::string text, boost::shared_ptr<RenderEffects> renderEffects);
		void	writeText(int x, int y, std::string text, boost::shared_ptr<RenderEffects> renderEffects);

		void	writeTextPy(float x, float y, std::string text, boost::shared_ptr<RenderEffects> renderEffects);
		void	writeText(float x, float y, std::string text, boost::shared_ptr<RenderEffects> renderEffects);

		int		getCharacterHeightPy();
		int		getCharacterHeight();

		int		getCharacterWidthPy();
		int		getCharacterWidth();

		int		getFontCharacterHeightPy();
		int		getFontCharacterHeight();

		int		getFontCharacterWidthPy();
		int		getFontCharacterWidth();

	private: 

		boost::shared_ptr<Renderer>	renderer_;

		int							fontSurfaceId_;
	};
}

#endif // _BITMAPFONT_HPP_