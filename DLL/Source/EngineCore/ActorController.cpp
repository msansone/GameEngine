#include "..\..\Headers\EngineCore\ActorController.hpp"

using namespace firemelon;
using namespace boost::python;

ActorController::ActorController()
{
}

ActorController::~ActorController()
{
}

void ActorController::createRenderables()
{
	StageRenderablePtr stageRenderableForeground = StageRenderablePtr(new StageRenderable());

	stageRenderableForeground->isBackground_ = false;

	boost::shared_ptr<Renderable> renderableForeground = boost::static_pointer_cast<Renderable>(stageRenderableForeground);

	stageRenderableForeground->anchorPointManager_ = getAnchorPointManager();
	stageRenderableForeground->animationManager_ = getAnimationManager();

	addRenderable(renderableForeground);

	StageRenderablePtr stageRenderableBackground = StageRenderablePtr(new StageRenderable());

	stageRenderableBackground->isBackground_ = true;

	boost::shared_ptr<Renderable> renderableBackground = boost::static_pointer_cast<Renderable>(stageRenderableBackground);

	stageRenderableBackground->anchorPointManager_ = getAnchorPointManager();
	stageRenderableBackground->animationManager_ = getAnimationManager();

	addRenderable(renderableBackground);
}