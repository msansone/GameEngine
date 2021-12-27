/* -------------------------------------------------------------------------
** SdlApp.hpp
** 
** The SdlApp  class is used to initialize a window using SDL. 
** It implements its own main method and runs the main update loop, initialization, 
** cleanup, and defining the menues.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SDLAPP_HPP_
#define _SDLAPP_HPP_

#include "SDL.h"
#include "SDL_image.h"

#include <assert.h>
#include <string>
#include <sstream>
#include <iostream>
#include <fstream>

//#include <boost/lexical_cast.hpp>
//#include <boost/property_tree/xml_parser.hpp>
//#include <boost/property_tree/ptree.hpp>
//#include <boost/foreach.hpp>
#include <boost/python.hpp>

#include <BitmapFont.hpp>
#include <GameStateManager.hpp>
#include <Position.hpp>

#include "GameApp.hpp"
#include "SdlKeyboardDevice.hpp"
#include "SdlGamepadDevice.hpp"
#include "BoostGameTimer.hpp"
#include "SdlRenderer.hpp"
#include "FmodAudioPlayer.hpp"

class SdlApp : public GameApp
{
public:
	
	SdlApp(firemelon::GameEngine* engine);
	virtual ~SdlApp();

protected:
	
	virtual int										userMain(int argc, char* args[]);
	virtual void									userAppBegin();
	virtual bool									userInitialize();
	virtual void									userShutdown();
	virtual boost::shared_ptr<firemelon::Factory>	createFactory();

private:
	
	bool											begin();
	bool											update();
	bool											end();

	void											inputDeviceRemoved(std::string deviceName);

	SDL_Event										event;	

	boost::shared_ptr<SdlKeyboardDevice>			keyboardDevice_;
};


#endif // _SDLAPP_HPP_