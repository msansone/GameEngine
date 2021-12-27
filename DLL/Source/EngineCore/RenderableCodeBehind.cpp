#include "..\..\Headers\EngineCore\RenderableCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

RenderableCodeBehind::RenderableCodeBehind()
{
}

RenderableCodeBehind::~RenderableCodeBehind()
{
}

void RenderableCodeBehind::frameTriggered(TriggerSignalId frameTriggerSignal)
{
	renderableScript_->frameTriggered(frameTriggerSignal);
}

void RenderableCodeBehind::preInitialize()
{
	renderableScript_ = boost::make_shared<RenderableScript>(RenderableScript(debugger_));

	renderableScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	renderableScript_->stageController_ = stageController_;

	renderable_->renderedSignal_->connect(boost::bind(&RenderableCodeBehind::rendered, this, _1, _2));

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		renderableScript_->preInitialize();
	}

	initialize();
}

void RenderableCodeBehind::rendered(int x, int y)
{
	renderableScript_->rendered(x, y);
}

void RenderableCodeBehind::preCleanup()
{
	renderable_->renderedSignal_->disconnect(boost::bind(&RenderableCodeBehind::rendered, this, _1, _2));

	cleanup();

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		renderableScript_->preCleanup();
	}
}

void RenderableCodeBehind::cleanup()
{
}

void RenderableCodeBehind::initialize()
{
}