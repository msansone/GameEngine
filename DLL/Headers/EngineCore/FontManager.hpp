/* -------------------------------------------------------------------------
** FontManager.hpp
**
** The FontManager class is used to retrieve bitmap font objects. A BitmapFont
** object is generated from a sprite sheet. This class then caches the object
** for future requests.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */
#ifndef _FONTMANAGER_HPP_
#define _FONTMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <vector>
#include <map>

#include "BitmapFont.hpp"

namespace firemelon
{
	class FIREMELONAPI FontManager
	{
	public:

		FontManager(boost::shared_ptr<Renderer> renderer);
		virtual ~FontManager();
	
		boost::shared_ptr<BitmapFont>	getFontPy(std::string fontSheetName);
		boost::shared_ptr<BitmapFont>	getFont(std::string fontSheetName);

	private:
		
		std::vector<boost::shared_ptr<BitmapFont>>	fonts_;

		// Used so that a BitmapFont object in the fonts_ array only needs to be initialized once.
		std::map<std::string, int>	fontNameIndexMap_;
		
		boost::shared_ptr<Renderer>	renderer_;
	};

	typedef boost::shared_ptr<FontManager> FontManagerPtr;
}

#endif // _FONTMANAGER_HPP_