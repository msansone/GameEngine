#include "..\..\Headers\EngineCore\Renderable.hpp"

using namespace firemelon;

int Renderable::idCounter_ = 0;

Renderable::Renderable() :
	renderedSignal_(new boost::signals2::signal<void(int, int)>)
{
	doReplicate_ = false;
	doRender_ = false;
	endRow_ = -1;
	endCol_ = -1;
	id_ = idCounter_;
	idCounter_++;
	isVisible_ = true;
	layerPosition_ = nullptr;
	mapLayerIndex_ = 0;
	renderOrder_ = 0;
	startCol_ = -1;
	startRow_ = -1;
	scaleFactor_ = 1.0f;
	visibilityGridBounds_ = nullptr;

	//renderEffects_ = RenderEffectsPtr(new RenderEffects);

	RotationOperationPtr rotationOperation = boost::shared_ptr<firemelon::RotationOperation>(new RotationOperation(0.0f, 0, 0));

	renderEffects_->addRotation(rotationOperation);
}

Renderable::~Renderable()
{
}

void Renderable::initializeRenderable()
{
}

void Renderable::updateRenderable(double time)
{
}

void Renderable::render(double lerp)
{
}

void Renderable::renderDebugData(double lerp)
{
}

void Renderable::rendered(int x, int y)
{
	(*renderedSignal_)(x, y);
}

int	Renderable::getRenderableId()
{
	return id_;
}

int	Renderable::getGridCellStartRow()
{
	return startRow_;
}

int	Renderable::getGridCellEndRow()
{
	return endRow_;
}

int	Renderable::getGridCellStartCol()
{
	return startCol_;
}

int	Renderable::getGridCellEndCol()
{
	return endCol_;
}

void Renderable::setGridCellStartRow(int value)
{
	startRow_ = value;
}

void Renderable::setGridCellEndRow(int value)
{
	endRow_ = value;
}

void Renderable::setGridCellStartCol(int value)
{
	startCol_ = value;
}

void Renderable::setGridCellEndCol(int value)
{
	endCol_ = value;
}

boost::shared_ptr<Position> Renderable::getLayerPosition()
{
	return layerPosition_;
}

void Renderable::attachDebugger(DebuggerPtr debugger)
{
	debugger_ = debugger;
}

DebuggerPtr Renderable::getDebugger()
{
	return debugger_;
}

RendererPtr Renderable::getRenderer()
{
	return renderer_;
}

void Renderable::attachRenderer(RendererPtr renderer)
{
	renderer_ = renderer;
}

void Renderable::setMapLayer(int value)
{
	mapLayerIndex_ = value;
}

int Renderable::getMapLayer()
{
	return mapLayerIndex_;
}

void Renderable::setRenderOrder(int value)
{
	renderOrder_ = value;
}

int Renderable::getRenderOrderPy()
{
	PythonReleaseGil unlocker;

	return getRenderOrder();
}

int Renderable::getRenderOrder()
{
	return renderOrder_;
}

bool Renderable::getIsDynamic()
{
	return false;
}

bool Renderable::getIsVisiblePy()
{
	PythonReleaseGil unlocker;

	return getIsVisible();
}

bool Renderable::getIsVisible()
{
	// If isVisible is true, then do a secondary check to see if the any of the grid cells are within
	// the visible grid bounds. If it is visible but not in a visible cell, then it should not be rendered.
	if (isVisible_ == true)
	{
		if (visibilityGridBounds_ != nullptr)
		{
			int visibilityGridStartCol = visibilityGridBounds_->getStartX();
			int visibilityGridEndCol = visibilityGridBounds_->getEndX();

			int visibilityGridStartRow = visibilityGridBounds_->getStartY();
			int visibilityGridEndRow = visibilityGridBounds_->getEndY();
		
			if (endCol_ < visibilityGridStartCol ||	startCol_ > visibilityGridEndCol ||
				endRow_ < visibilityGridStartRow ||	startRow_ > visibilityGridEndRow)
			{
				return false;
			}
		}
	}
	
	return isVisible_;
}

void Renderable::setIsVisiblePy(bool isVisisble)
{
	PythonReleaseGil unlocker;

	setIsVisible(isVisisble);
}

void Renderable::setIsVisible(bool isVisisble)
{
	isVisible_ = isVisisble;
}

void Renderable::setPosition(boost::shared_ptr<Position> position)
{
	position_ = position;
}

boost::shared_ptr<Position> Renderable::getPosition()
{
	return position_;
}

RenderEffectsPtr Renderable::getRenderEffects()
{
	return renderEffects_;
}

void Renderable::setScaleFactor(float scaleFactor)
{
	scaleFactor_ = scaleFactor;
}

float Renderable::getScaleFactor()
{
	return scaleFactor_;
}