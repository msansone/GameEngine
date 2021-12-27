#include "..\..\Headers\EngineCore\TileController.hpp"

using namespace firemelon;
using namespace boost::python;

TileController::TileController()
{
}

TileController::~TileController()
{
}

void TileController::initialize()
{
}

void TileController::createRenderables()
{
	addRenderable(boost::shared_ptr<Renderable>(new TileRenderable()));
}