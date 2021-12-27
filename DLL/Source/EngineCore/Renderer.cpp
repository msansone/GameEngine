#include "..\..\Headers\EngineCore\Renderer.hpp"

using namespace firemelon;

Renderer::Renderer()
{
	enableFade_ = true;
}

Renderer::~Renderer()
{
	// Clean up all remaining surfaces.
	sheets_.clear();
}

void Renderer::drawPolygonPy(std::vector<Vertex2> vertices, ColorRgbaPtr color)
{
	PythonReleaseGil unlocker;

	drawPolygon(vertices, color);
}

void Renderer::drawRectPy(float x, float y, int width, int height, float red, float green, float blue, float alpha)
{
	PythonReleaseGil unlocker;

	drawRect(x, y, width, height, red, green, blue, alpha);
}

bool Renderer::getEnableFadePy()
{
	PythonReleaseGil unlocker;

	return getEnableFade();
}

bool Renderer::getEnableFade()
{
	return enableFade_;
}

void Renderer::fillRectPy(float x, float y, int width, int height, float red, float green, float blue, float alpha)
{
	PythonReleaseGil unlocker;

	fillRect(x, y, width, height, red, green, blue, alpha);
}

void Renderer::setEnableFadePy(bool enableFade)
{
	PythonReleaseGil unlocker;

	setEnableFade(enableFade);
}

void Renderer::setEnableFade(bool enableFade)
{
	enableFade_ = enableFade;
}

void Renderer::setFadeColor(float red, float green, float blue)
{
	
}

void Renderer::setFadeOpacity(float opacity)
{

}

float Renderer::getFadeOpacity()
{
	return 1.0f;
}

int Renderer::translateSpriteSheetId(AssetId editorSpriteSheetId)
{
	return spritesheetIdMap_[editorSpriteSheetId];
}

void Renderer::renderSheetByIdPy(float x, float y, int sheetId)
{
	PythonReleaseGil unlocker;

	renderSheetById(x, y, sheetId);
}

void Renderer::renderSheetByIdPy(float x, float y, int sheetId, boost::shared_ptr<RenderEffects> renderEffects)
{
	PythonReleaseGil unlocker;

	renderSheetById(x, y, sheetId, renderEffects);
}

void Renderer::renderSheetSectionPy(float x, float y, int sheetId, Rect source)
{
	PythonReleaseGil unlocker;

	renderSheetSection(x, y, sheetId, source);
}
		
void Renderer::renderSheetSectionPy(float x, float y, int sheetId, Rect source, boost::shared_ptr<RenderEffects> renderEffects)
{
	PythonReleaseGil unlocker;

	renderSheetSection(x, y, sheetId, source, renderEffects);
}

void Renderer::renderSheetCellPy(float x,	float y, int sheetId, int sourceX, int sourceY)
{
	PythonReleaseGil unlocker;

	renderSheetCell(x, y, sheetId, sourceX, sourceY);
}

void Renderer::renderSheetCellPy(float x, float y, int sheetId, int sourceX, int sourceY, boost::shared_ptr<RenderEffects> renderEffects)
{
	PythonReleaseGil unlocker;

	renderSheetCell(x, y, sheetId, sourceX, sourceY, renderEffects);
}

void Renderer::saveScreenshot(std::string fileName)
{
	// no-op.
}

void Renderer::setFadeColorPy(float red, float green, float blue)
{
	PythonReleaseGil unlocker;

	setFadeColor(red, green, blue);
}

void Renderer::setFadeOpacityPy(float opacity)
{
	PythonReleaseGil unlocker;

	setFadeOpacity(opacity);
}

float Renderer::getFadeOpacityPy()
{
	PythonReleaseGil unlocker;

	return getFadeOpacity();
}

boost::shared_ptr<SpriteSheet> Renderer::getSheetPy(int sheetId)
{
	PythonReleaseGil unlocker;

	return getSheet(sheetId);
}
		
boost::shared_ptr<SpriteSheet> Renderer::getSheetByNamePy(std::string sheetName)
{
	PythonReleaseGil unlocker;

	return getSheetByName(sheetName);
}
		
int Renderer::getSheetIDByNamePy(std::string sheetName)
{
	PythonReleaseGil unlocker;

	return getSheetIDByName(sheetName);
}

int	Renderer::getScreenHeightPy()
{
	PythonReleaseGil unlocker;

	return getScreenHeight();
}
		
void Renderer::setScreenSizePy(int width, int height)
{
	PythonReleaseGil unlocker;

	setScreenSize(width, height);
}

int Renderer::getScreenWidthPy()
{
	PythonReleaseGil unlocker;

	return getScreenWidth();
}
