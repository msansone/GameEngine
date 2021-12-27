#include "..\..\Headers\EngineCore\BitmapFont.hpp"

using namespace firemelon;

BitmapFont::BitmapFont(boost::shared_ptr<Renderer> renderer)
{
	renderer_ = renderer;}

BitmapFont::~BitmapFont()
{
}

void BitmapFont::initializeFont(std::string sheetname, std::string filename, int characterHeight, int characterWidth, int rows, int columns, float scaleFactor)
{
	// Load the image file as a sprite sheet.
	fontSurfaceId_ = renderer_->loadSpriteSheet(sheetname, filename, rows, columns, characterHeight, characterWidth, scaleFactor, true, 0);
}

void BitmapFont::initializeFont(std::string sheetname)
{
	fontSurfaceId_ = renderer_->getSheetIDByName(sheetname);
}

void BitmapFont::writeTextPy(int x, int y, std::string text, double r, double g, double b, double a)
{
	PythonReleaseGil unlocker;

	writeText(x, y, text, r, g, b, a);
}

void BitmapFont::writeTextPy(float x, float y, std::string text, double r, double g, double b, double a)
{
	PythonReleaseGil unlocker;

	writeText(x, y, text, r, g, b, a);
}

void BitmapFont::writeTextPy(int x, int y, std::string text, boost::shared_ptr<RenderEffects> renderEffect)
{
	PythonReleaseGil unlocker;

	writeText(x, y, text, renderEffect);
}

void BitmapFont::writeTextPy(float x, float y, std::string text, boost::shared_ptr<RenderEffects> renderEffect)
{
	PythonReleaseGil unlocker;

	writeText(x, y, text, renderEffect);
}

void BitmapFont::writeText(int x, int y, std::string text, double r, double g, double b, double a)
{
	Rect charRect;

	boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(fontSurfaceId_);

	int rows = sheet->getRows();
	int columns = sheet->getColumns();

	float scaleFactor = sheet->getScaleFactor();

	int fontCharacterHeight = sheet->getCellHeight();
	int fontCharacterWidth = sheet->getCellWidth();

	int currentCharacterX = x;
	int currentCharacterY = y;

	charRect.x = 0;
	charRect.y = 0;
	charRect.w = fontCharacterWidth;
	charRect.h = fontCharacterHeight;

	int scaledCharacterWidth = (int)(fontCharacterWidth * scaleFactor);

	// Loop through each character and render it.
	int size = text.size();
	for (int i = 0; i < size; i++)
	{
		// Convert the character's ASCII value into a location on the source image.
		char currentChar = text[i];

		int sourceCol = (currentChar % columns);
		int sourceRow = (int)(currentChar / columns);

		charRect.x = (sourceCol * fontCharacterWidth);
		charRect.y = (sourceRow * fontCharacterHeight);

		boost::shared_ptr<RenderEffects> renderEffects = boost::shared_ptr<firemelon::RenderEffects>(new RenderEffects);

		renderEffects->getHueColor()->setR(r);
		renderEffects->getHueColor()->setG(g);
		renderEffects->getHueColor()->setB(b);
		renderEffects->getHueColor()->setA(a);

		// Do I need to build the source rect? Can't I just use the sheet row and column?
		renderer_->renderSheetSection(currentCharacterX, currentCharacterY, fontSurfaceId_, charRect, renderEffects);
			
		currentCharacterX += scaledCharacterWidth;
	}
}

void BitmapFont::writeText(float x, float y, std::string text, double r, double g, double b, double a)
{
	Rect charRect;

	boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(fontSurfaceId_);

	int rows = sheet->getRows();
	int columns = sheet->getColumns();

	float scaleFactor = sheet->getScaleFactor();

	int fontCharacterHeight = sheet->getCellHeight();
	int fontCharacterWidth = sheet->getCellWidth();

	int currentCharacterX = x;
	int currentCharacterY = y;

	charRect.x = 0;
	charRect.y = 0;
	charRect.w = fontCharacterWidth;
	charRect.h = fontCharacterHeight;

	int scaledCharacterWidth = (int)(fontCharacterWidth * scaleFactor);

	// Loop through each character and render it.
	int size = text.size();
	for (int i = 0; i < size; i++)
	{
		// Convert the character's ASCII value into a location on the source image.
		char currentChar = text[i];

		int sourceCol = (currentChar % columns);
		int sourceRow = (int)(currentChar / columns);

		charRect.x = (sourceCol * fontCharacterWidth);
		charRect.y = (sourceRow * fontCharacterHeight);

		boost::shared_ptr<RenderEffects> renderEffects = boost::shared_ptr<firemelon::RenderEffects>(new RenderEffects);

		renderEffects->getHueColor()->setR(r);
		renderEffects->getHueColor()->setG(g);
		renderEffects->getHueColor()->setB(b);
		renderEffects->getHueColor()->setA(a);

		// Do I need to build the source rect? Can't I just use the sheet row and column?
		renderer_->renderSheetSection(currentCharacterX, currentCharacterY, fontSurfaceId_, charRect, renderEffects);

		currentCharacterX += scaledCharacterWidth;
	}
}

void BitmapFont::writeText(int x, int y, std::string text, boost::shared_ptr<RenderEffects> renderEffects)
{
	Rect charRect;

	boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(fontSurfaceId_);

	int rows = sheet->getRows();

	int columns = sheet->getColumns();

	float scaleFactor = renderEffects->getScaleFactor() * sheet->getScaleFactor();

	int fontCharacterHeight = sheet->getCellHeight();

	int fontCharacterWidth = sheet->getCellWidth();

	float currentCharacterX = x;

	int currentCharacterY = y;

	charRect.x = 0;
	charRect.y = 0;
	charRect.w = fontCharacterWidth;
	charRect.h = fontCharacterHeight;

	int scaledCharacterWidth = (int)(fontCharacterWidth * scaleFactor);

	// Loop through each character and render it.
	int size = text.size();
	for (int i = 0; i < size; i++)
	{
		// Convert the character's ASCII value into a location on the source image.
		char currentChar = text[i];

		int sourceCol = (currentChar % columns);
		int sourceRow = (int)(currentChar / columns);

		charRect.x = (sourceCol * fontCharacterWidth);
		charRect.y = (sourceRow * fontCharacterHeight);

		 // Do I need to build the source rect? Can't I just use the sheet row and column?
		renderer_->renderSheetSection(floor(currentCharacterX), currentCharacterY, fontSurfaceId_, charRect, renderEffects);
		
		currentCharacterX += scaledCharacterWidth;
	}
}

void BitmapFont::writeText(float x, float y, std::string text, boost::shared_ptr<RenderEffects> renderEffects)
{
	Rect charRect;

	boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(fontSurfaceId_);

	int rows = sheet->getRows();

	int columns = sheet->getColumns();

	float scaleFactor = renderEffects->getScaleFactor();

	float fontCharacterHeight = sheet->getCellHeight();

	float fontCharacterWidth = sheet->getCellWidth();

	float currentCharacterX = x;

	float currentCharacterY = y;

	charRect.x = 0;
	charRect.y = 0;
	charRect.w = fontCharacterWidth;
	charRect.h = fontCharacterHeight;

	int scaledCharacterWidth = (int)(fontCharacterWidth * scaleFactor);

	// Loop through each character and render it.
	int size = text.size();
	for (int i = 0; i < size; i++)
	{
		// Convert the character's ASCII value into a location on the source image.
		char currentChar = text[i];

		int sourceCol = (currentChar % columns);
		int sourceRow = (int)(currentChar / columns);

		charRect.x = (sourceCol * fontCharacterWidth);
		charRect.y = (sourceRow * fontCharacterHeight);

		// Do I need to build the source rect? Can't I just use the sheet row and column?
		renderer_->renderSheetSection(currentCharacterX, currentCharacterY, fontSurfaceId_, charRect, renderEffects);

		currentCharacterX += scaledCharacterWidth;
	}
}

 int BitmapFont::getFontCharacterHeightPy()
 {
	PythonReleaseGil unlocker;

	return getFontCharacterHeight();
 }

 int BitmapFont::getFontCharacterHeight()
 {	 
	 boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(fontSurfaceId_);

	return (int)(sheet->getCellHeight() * sheet->getScaleFactor());
 }
 
 int BitmapFont::getFontCharacterWidthPy()
 {
	PythonReleaseGil unlocker;

	return getFontCharacterWidth();
 }

 int BitmapFont::getFontCharacterWidth()
 {
	 boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(fontSurfaceId_);

	return (int)(sheet->getCellWidth() * sheet->getScaleFactor());
 }

 int BitmapFont::getCharacterHeightPy()
 {
	 PythonReleaseGil unlocker;

	 return getFontCharacterHeight();
 }

 int BitmapFont::getCharacterHeight()
 {
	 boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(fontSurfaceId_);

	 return (int)(sheet->getCellHeight() * sheet->getScaleFactor());
 }

 int BitmapFont::getCharacterWidthPy()
 {
	 PythonReleaseGil unlocker;

	 return getFontCharacterWidth();
 }

 int BitmapFont::getCharacterWidth()
 {
	 boost::shared_ptr<SpriteSheet> sheet = renderer_->getSheet(fontSurfaceId_);

	 return (int)(sheet->getCellWidth() * sheet->getScaleFactor());
 }