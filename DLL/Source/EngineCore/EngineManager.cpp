#include "..\..\Headers\EngineCore\EngineManager.hpp"

using namespace firemelon;

EngineManager::EngineManager()
{
	engine_ = new GameEngine();
}

EngineManager::~EngineManager()
{
	if (Debugger::entityCount > 0)
	{
		std::cout << Debugger::entityCount << " entities did not get destroyed." << std::endl;
	}

	if (Debugger::entityControllerCount > 0)
	{
		std::cout << Debugger::entityControllerCount << " entity controllers did not get destroyed." << std::endl;
	}

	if (Debugger::entityComponentCount > 0)
	{
		std::cout << Debugger::entityComponentCount << " entity components did not get destroyed." << std::endl;
	}

	if (Debugger::codeBehindCount > 0)
	{
		std::cout << Debugger::codeBehindCount << " code behinds did not get destroyed." << std::endl;
	}
	
	if (Debugger::collisionDataCount > 0)
	{
		std::cout << Debugger::collisionDataCount << " collision data objects did not get destroyed." << std::endl;
	}

	if (Debugger::messageCount > 0)
	{
		std::cout << Debugger::messageCount << " messages did not get destroyed." << std::endl;
	}
	if (Debugger::queryCount > 0)
	{
		std::cout << Debugger::queryCount << " queries did not get destroyed." << std::endl;
	}
	if (Debugger::renderableTextCount > 0)
	{
		std::cout << Debugger::renderableTextCount << " renderable text objects did not get destroyed." << std::endl;
	}
	if (Debugger::roomCount > 0)
	{
		std::cout << Debugger::roomCount << " rooms did not get destroyed." << std::endl;
	}

	delete engine_;

	engine_ = nullptr;
}

GameEngine* EngineManager::getEngine()
{
	return engine_;
}