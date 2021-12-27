#include "..\..\Headers\EngineCore\FontManager.hpp"

using namespace firemelon;

FontManager::FontManager(boost::shared_ptr<Renderer> renderer)
{
	renderer_ = renderer;
}

FontManager::~FontManager()
{
	fonts_.clear();
}

boost::shared_ptr<BitmapFont> FontManager::getFontPy(std::string fontSheetName)
{
	PythonReleaseGil unlocker;

	return getFont(fontSheetName);
}

boost::shared_ptr<BitmapFont> FontManager::getFont(std::string fontSheetName)
{
	// If the font for this text has not yet been initialized, then initialize it here.
	std::map<std::string, int>::iterator it = fontNameIndexMap_.find(fontSheetName);
	
	int charWidth = 0;
	int charHeight = 0;

	if (it == fontNameIndexMap_.end())
	{
		fontNameIndexMap_[fontSheetName] = fonts_.size();
		
		boost::shared_ptr<BitmapFont> newFont = boost::shared_ptr<BitmapFont>(new BitmapFont(renderer_));

		newFont->initializeFont(fontSheetName);

		fonts_.push_back(newFont);

		return newFont;
	}
	else
	{
		int index = fontNameIndexMap_[fontSheetName];

		return fonts_[index];
	}
}
