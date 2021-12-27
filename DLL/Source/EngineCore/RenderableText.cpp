#include "..\..\Headers\EngineCore\RenderableText.hpp"

using namespace firemelon;
using namespace boost::python;

int RenderableText::idCounter_ = 0;

RenderableText::RenderableText() :
	renderableTextDestroySignal_(new RenderableTextDestroySignalRaw())
{
	displayText_ = "";
	fontName_ = "";
	x_ = 0;
	y_ = 0;
	previousX_ = 0;
	previousY_ = 0;
	layerIndex_ = -1;
	roomIndex_ = -1;
	interpolatePosition_ = false;
	fontManager_ = nullptr;
	isPyObject_ = false;
	isRemoved_ = false;

	id_ = idCounter_;
	idCounter_++;

	color_ = ColorRgbaPtr(new ColorRgba(1.0f, 1.0f, 1.0f, 1.0f));

	implementsPyAdded_ = false;
	implementsPyButtonDown_ = false;
	implementsPyButtonUp_ = false;
	implementsPyInitialize_ = false;
	implementsPyUpdate_= false;
	implementsPyRemoved_ = false;
	implementsPyRender_= false;

	controller_ = boost::shared_ptr<RenderableTextController>(new RenderableTextController());
}

RenderableText::~RenderableText()
{
	// Let the text manager know this renderable text is being destroyed, so it can remove it.
	// If it wasn't added, this signal won't be set and nothing will be called.	
	(*renderableTextDestroySignal_)(roomIndex_, id_);
}

void RenderableText::cleanup()
{
	controller_->renderableTextRemoveSignal_->disconnect(boost::bind(&RenderableText::remove, this));

	pyInitialize_ = boost::python::object();
	pyUpdate_ = boost::python::object();
	pyRender_ = boost::python::object();
}

void RenderableText::preInitialize()
{
	controller_->isActive_ = true;

	controller_->renderableTextRemoveSignal_->connect(boost::bind(&RenderableText::remove, this));

	previousX_ = x_;
	previousY_ = y_;

	initialize();
}

void RenderableText::initialize()
{
	PythonAcquireGil lock;

	if (implementsPyInitialize_ == true)
	{
		try
		{
			pyInitialize_();
		}
		catch (error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}
}

void RenderableText::preButtonDown(GameButtonId buttonId)
{
	buttonDown(buttonId);
}

void RenderableText::buttonDown(GameButtonId buttonId)
{
	PythonAcquireGil lock;

	if (implementsPyButtonDown_ == true)
	{
		try
		{
			pyButtonDown_(buttonId);
		}
		catch (error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}
}

void RenderableText::preButtonUp(GameButtonId buttonId)
{
	buttonUp(buttonId);
}

void RenderableText::buttonUp(GameButtonId buttonId)
{
	PythonAcquireGil lock;

	if (implementsPyButtonUp_ == true)
	{
		try
		{
			pyButtonUp_(buttonId);
		}
		catch (error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}
}

void RenderableText::preUpdate(double time)
{
	update(time);
}

void RenderableText::update(double time)
{
	PythonAcquireGil lock;

	if (implementsPyUpdate_ == true)
	{
		try
		{
			pyUpdate_(time);
		}
		catch(error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}
}

void RenderableText::render(int x, int y)
{
	PythonAcquireGil lock;

	if (implementsPyRender_ == true)
	{
		try
		{
			pyRender_(x, y);
		}
		catch(error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}
}

boost::shared_ptr<FontManager> RenderableText::getFontManager()
{
	return fontManager_;
}

ColorRgbaPtr RenderableText::getColorPy()
{
	PythonReleaseGil unlocker;

	return getColor();
}

ColorRgbaPtr RenderableText::getColor()
{
	return color_;
}

std::string RenderableText::getDisplayTextPy()
{
	PythonReleaseGil unlocker;

	return getDisplayText();
}

std::string RenderableText::getDisplayText()
{
	return displayText_;
}

void RenderableText::setDisplayTextPy(std::string value)
{
	PythonReleaseGil unlocker;

	setDisplayText(value);
}

void RenderableText::setDisplayText(std::string value)
{
	displayText_ = value;
}

std::string RenderableText::getFontNamePy()
{
	PythonReleaseGil unlocker;

	return getFontName();
}

std::string RenderableText::getFontName()
{
	return fontName_;
}

void RenderableText::setFontNamePy(std::string value)
{
	PythonReleaseGil unlocker;

	setFontName(value);
}

void RenderableText::setFontName(std::string value)
{
	fontName_ = value;
}

int	RenderableText::getXPy()
{
	PythonReleaseGil unlocker;

	return getX();
}

int	RenderableText::getX()
{
	return x_;
}

void RenderableText::setXPy(int value)
{
	PythonReleaseGil unlocker;

	setX(value);
}

void RenderableText::setX(int value)
{
	x_ = value;
}
		
int	RenderableText::getYPy()
{
	PythonReleaseGil unlocker;

	return getY();
}

int	RenderableText::getY()
{
	return y_;
}

void RenderableText::setYPy(int value)
{
	PythonReleaseGil unlocker;

	setY(value);
}

void RenderableText::setY(int value)
{
	y_ = value;
}
		
int	RenderableText::getLayerIndexPy()
{
	PythonReleaseGil unlocker;

	return getLayerIndex();
}

int	RenderableText::getLayerIndex()
{
	return layerIndex_;
}

void RenderableText::setLayerIndexPy(int value)
{
	PythonReleaseGil unlocker;

	setLayerIndex(value);
}

void RenderableText::setLayerIndex(int value)
{
	layerIndex_ = value;
}

bool RenderableText::getInterpolatePositionPy()
{
	PythonReleaseGil unlocker;

	return getInterpolatePosition();
}

bool RenderableText::getInterpolatePosition()
{
	return interpolatePosition_;
}

void RenderableText::setInterpolatePositionPy(bool value)
{
	PythonReleaseGil unlocker;

	setInterpolatePosition(value);
}

void RenderableText::setInterpolatePosition(bool value)
{
	interpolatePosition_ = value;
}

void RenderableText::remove()
{
	isRemoved_ = true;
	controller_->isActive_ = false;

	PythonAcquireGil lock;

	if (implementsPyRemoved_ == true)
	{
		try
		{
			pyRemoved_();
		}
		catch (error_already_set &)
		{
			debugger_->handlePythonError();
		}
	}
}
