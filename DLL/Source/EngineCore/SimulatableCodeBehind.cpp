#include "..\..\Headers\EngineCore\SimulatableCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

SimulatableCodeBehind::SimulatableCodeBehind()
{
}

SimulatableCodeBehind::~SimulatableCodeBehind()
{
}


boost::shared_ptr<SimulatableScript> SimulatableCodeBehind::getScript()
{
	return simulatableScript_;
}

void SimulatableCodeBehind::preInitialize()
{
	simulatableScript_ = boost::shared_ptr<SimulatableScript>(new SimulatableScript(debugger_));

	simulatableScript_->dynamicsControllerHolder_ = dynamicsControllerHolder_;

	simulatableScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		simulatableScript_->preInitialize();
	}

	initialize();
}

void SimulatableCodeBehind::frameBegin()
{
	simulatableScript_->frameBegin();
}

void SimulatableCodeBehind::preIntegration()
{
	simulatableScript_->preIntegration();
}

void SimulatableCodeBehind::postIntegration()
{
	simulatableScript_->postIntegration();
}

void SimulatableCodeBehind::start()
{
	simulatableScript_->start();
}

void SimulatableCodeBehind::preCleanup()
{
	cleanup();

	if (classification_ != ENTITY_CLASSIFICATION_TILE)
	{
		simulatableScript_->preCleanup();
	}
}

void SimulatableCodeBehind::cleanup()
{
}

void SimulatableCodeBehind::initialize()
{
}