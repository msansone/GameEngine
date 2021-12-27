/* -------------------------------------------------------------------------
** GameApp.hpp
** 
** The GameApp class is the application's entry point. It is subclassed
** for the different window creation libraries such as freeglut or SDL. The 
** main function will then create one of the subclasses based on which library
** you want to use.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _GAMEAPP_HPP_
#define _GAMEAPP_HPP_

#include <assert.h>

#include <GameEngine.hpp>

#include "FiremelonExFactory.hpp"
#include "SdlGamepadDevice.hpp"

class GameApp
{
public:
	
	GameApp(firemelon::GameEngine* engine);
	virtual ~GameApp();
	
	int		appMain(int argc, char* args[]);
	
	void	shutdownApp();

protected:

	virtual	int										userMain(int argc, char* args[]) = 0;
	virtual void									userAppBegin() = 0;
	virtual bool									userInitialize() = 0;
	virtual void									userShutdown() = 0;

	virtual	boost::shared_ptr<firemelon::Factory>	createFactory() = 0;
	
	bool											initializeApp();

	firemelon::GameEngine*	engine_;	
	boost::shared_ptr<firemelon::Factory>			factory_;
};


#endif // _GAMEAPP_HPP_