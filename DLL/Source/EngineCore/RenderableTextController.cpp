#include "..\..\Headers\EngineCore\RenderableTextController.hpp"

using namespace firemelon;
using namespace boost::python;

RenderableTextController::RenderableTextController() :
	renderableTextRemoveSignal_(new RenderableTextRemoveSignalRaw())
{
	isActive_ = false;
}

RenderableTextController::~RenderableTextController()
{

}

bool RenderableTextController::getIsActivePy()
{
	PythonReleaseGil unlocker;

	return getIsActive();
}

bool RenderableTextController::getIsActive()
{
	return isActive_;
}

void RenderableTextController::removePy()
{
	PythonReleaseGil unlocker;

	remove();
}

void RenderableTextController::remove()
{
	(*renderableTextRemoveSignal_)();
}