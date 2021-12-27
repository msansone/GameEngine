#include "..\..\Headers\EngineCore\BasicRenderableText.hpp"

using namespace firemelon;

BasicRenderableText::BasicRenderableText()
{
	font_ = nullptr;
}

BasicRenderableText::~BasicRenderableText()
{

}

void BasicRenderableText::initialize()
{
	boost::shared_ptr<FontManager> fontManager = getFontManager();

	std::string fontName = getFontName();
	
	font_ = fontManager->getFont(fontName);
}
	
void BasicRenderableText::update(double time)
{
}

void BasicRenderableText::render(int x, int y)
{
	ColorRgbaPtr color = getColor();

	std::string text = getDisplayText();
	
	font_->writeText(x, y, text, color->getR(), color->getG(), color->getB(), color->getA());
}
