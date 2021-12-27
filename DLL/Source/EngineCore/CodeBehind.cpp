#include "..\..\Headers\EngineCore\CodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

CodeBehind::CodeBehind()
{
}

CodeBehind::~CodeBehind()
{
}

EntityControllerPtr CodeBehind::getEntityController()
{
	return controller_;
}

boost::shared_ptr<HitboxManager> CodeBehind::getHitboxManager()
{
	return hitboxManager_;
}

boost::shared_ptr<InputDeviceManager> CodeBehind::getInputDeviceManager()
{
	return inputDeviceManager_;
}

boost::shared_ptr<EntityMetadata> CodeBehind::getMetadata()
{
	return metadata_;
}

boost::shared_ptr<GameTimer> CodeBehind::getTimer()
{
	return timer_;
}

void CodeBehind::cleanup()
{
	return;
}

void CodeBehind::initialize()
{
	return;
}

boost::shared_ptr<PythonInstanceWrapper> CodeBehind::getPythonInstanceWrapper()
{
	return pythonInstanceWrapper_;
}

void CodeBehind::setClassification(EntityClassification classification)
{
	classification_ = classification;
}
