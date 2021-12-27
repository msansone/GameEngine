#pragma warning( disable : 4273)

#include <EngineManager.hpp>
#include <GameEngine.hpp>

#include "..\Headers\SdlApp.hpp"

using namespace firemelon;

int main(int argc, char* args[])
{
	// Initialize the engine.
	EngineManager* engineManager = CreateEngineManager();
	
	GameApp* app = new SdlApp(engineManager->getEngine());

	int returnValue = app->appMain(argc, args);
	
	//engine_->writeDebugLogFile();
	
	app->shutdownApp();

	delete app;
	
	// Destroy the engine.
	DeleteEngineManager(engineManager);
	
	SDL_Quit();

	//return returnValue;
	return 0;
}