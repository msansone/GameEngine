#include "..\Headers\SdlApp.hpp"

using namespace firemelon;
using namespace boost::python;

SdlApp::SdlApp(GameEngine* engine) : GameApp(engine)
{
}

SdlApp::~SdlApp ()
{

}

void SdlApp::userAppBegin()
{
	boost::shared_ptr<InputDeviceManager> inputDeviceManager = engine_->getInputDeviceManager();
	boost::shared_ptr<FiremelonExUi> ui = boost::static_pointer_cast<FiremelonExUi>(engine_->getUi());

	inputDeviceManager->addInputDevice(keyboardDevice_);
	
	//SDL_GameController *pad = SDL_GameControllerOpen( id );
	//SDL_Joystick *joy = SDL_GameControllerGetJoystick( pad );
	//int instanceID = SDL_JoystickInstanceID( joy );

	//std::cout << "Initializing Input Devices" << std::endl;

	//int joystickCount = SDL_NumJoysticks();

	//std::cout << joystickCount << " gamepads detected." << std::endl;

	//for (int i = 0; i < joystickCount; i++)
	//{		
	//	SdlGamepadDevice* gamepadDevice = new SdlGamepadDevice(i);
	//	inputDeviceManager->addInputDevice(gamepadDevice);
	//}

	engine_->startEngine();
}

bool SdlApp::userInitialize()
{		
	char fullpath[_MAX_PATH];
	if (_fullpath(fullpath, ".//", _MAX_PATH) != NULL)
	{
		std::cout<<fullpath<<std::endl;
	}

	// Start SDL 
	std::cout<<"Initializing SDL"<<std::endl;

	int sdlErrorCode = SDL_Init(SDL_INIT_EVERYTHING);

	if (sdlErrorCode < 0) 
	{ 
		std::cout<<"SDL Initialization failed."<<std::endl
				 <<"Error: "<<SDL_GetError()<<std::endl;

		return false; 
	}
	
	std::string deviceName = "KEYBOARD";
	keyboardDevice_ = boost::shared_ptr<SdlKeyboardDevice>(new SdlKeyboardDevice(deviceName));

	return true;
}

void SdlApp::userShutdown()
{
	boost::shared_ptr<InputDeviceManager> inputDeviceManager = engine_->getInputDeviceManager();

	int inputDeviceCount = inputDeviceManager->getInputDeviceCount();

	for (int i = 0; i < inputDeviceCount; i++)
	{
		boost::shared_ptr<InputDevice> inputDevice = inputDeviceManager->getInputDeviceByIndex(i);

		if (inputDevice->getDeviceName() != "KEYBOARD")
		{
			boost::shared_ptr<SdlGamepadDevice> gamepadDevice = boost::static_pointer_cast<SdlGamepadDevice>(inputDevice);

			gamepadDevice->deviceRemovedSignal_.disconnect(boost::bind(&SdlApp::inputDeviceRemoved, this, _1));
		}
	}

	engine_->shutdownEngine();
}

int SdlApp::userMain(int argc, char* args[])
{
	if (begin() == false)
	{
		return 0;
	}
	
	boost::shared_ptr<EngineController> engineController = engine_->getEngineController();

	boost::shared_ptr<FiremelonExUi> ui = boost::static_pointer_cast<FiremelonExUi>(engine_->getUi());


	//While the user hasn't quit, run the main loop.
	bool hasQuit = false;

	while(hasQuit == false) 
	{
		hasQuit = engineController->getHasQuit();
		
		if (update() == false)
		{
			engineController->exit();
		}		

		// If an input device is configuring a button, don't consume messages here. It could interupt it.
		bool isConfiguring = false;

		boost::shared_ptr<InputDeviceManager> inputDeviceManager = engine_->getInputDeviceManager();

		int inputDeviceCount = inputDeviceManager->getInputDeviceCount();

		for (int i = 0; i < inputDeviceCount; i++)
		{
			boost::shared_ptr<InputDevice> inputDevice = inputDeviceManager->getInputDeviceByIndex(i);
			
			if (inputDevice->isConfiguring() == true)
			{
				isConfiguring = true;
				break;
			}
		}

		if (isConfiguring == false)
		{
			boost::shared_ptr<BoostGameTimer> gameTimer = boost::static_pointer_cast<BoostGameTimer>(engine_->getGameTimer());

			// While there's an event to handle...
			while(SDL_PollEvent(&event)) 
			{			
				switch (event.type)
				{
					case SDL_CONTROLLERDEVICEADDED:

						if (SDL_IsGameController(event.cdevice.which))
						{
							SDL_GameController* controller = SDL_GameControllerOpen(event.cdevice.which);
							SDL_Joystick* j = SDL_GameControllerGetJoystick(controller);
							SDL_JoystickID joyId = SDL_JoystickInstanceID(j);

							boost::shared_ptr<SdlGamepadDevice> gamepadDevice = boost::shared_ptr<SdlGamepadDevice>(new SdlGamepadDevice(joyId, controller, j));
							
							gamepadDevice->deviceRemovedSignal_.connect(boost::bind(&SdlApp::inputDeviceRemoved, this, _1));

							inputDeviceManager->addInputDevice(gamepadDevice);
							
							std::cout << "Controller device " << gamepadDevice->getDeviceName() << " added." << std::endl;

							ui->inputDeviceAdded(gamepadDevice->getChannel());
						}

						break;

					case SDL_CONTROLLERDEVICEREMOVED:
					{
						int inputDeviceCount = inputDeviceManager->getInputDeviceCount();

						for (int i = 0; i < inputDeviceCount; i++)
						{
							boost::shared_ptr<InputDevice> inputDevice = inputDeviceManager->getInputDeviceByIndex(i);

							std::string deviceTypeName = typeid(*inputDevice).name();
							std::string gamepadTypeName = typeid(SdlGamepadDevice).name();

							if (deviceTypeName == gamepadTypeName)
							{
								boost::shared_ptr<SdlGamepadDevice> gamepadDevice = boost::static_pointer_cast<SdlGamepadDevice>(inputDevice);

								if (event.cdevice.which == gamepadDevice->joystickId_)
								{
									std::cout << "Controller device " << inputDevice->getDeviceName() << " removed." << std::endl;

									boost::shared_ptr<SdlGamepadDevice> gamepadDevice = boost::static_pointer_cast<SdlGamepadDevice>(inputDevice);

									gamepadDevice->deviceRemovedSignal_.disconnect(boost::bind(&SdlApp::inputDeviceRemoved, this, _1));

									ui->inputDeviceRemoved(inputDevice->getChannel());

									inputDeviceManager->removeInputDeviceByChannel(inputDevice->getChannel());

									break;
								}
							}
						}

						break;
					}
					case SDL_QUIT:
					{
						engineController->exit();

						break;
					}
					case SDL_KEYDOWN: 

						if(event.key.keysym.mod & KMOD_SHIFT)
						{
							if (event.key.keysym.sym >= 'a' && event.key.keysym.sym <= 'z')
							{
								if (event.key.keysym.mod & KMOD_CAPS)
								{
									keyboardDevice_->keyDown(event.key.keysym.sym);
								}
								else
								{
									keyboardDevice_->keyDown((SDL_Keycode)((int)event.key.keysym.sym - 32));
								}
							}
							else if (event.key.keysym.sym == '1')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'!');
							}
							else if (event.key.keysym.sym == '2')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'@');
							}
							else if (event.key.keysym.sym == '3')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'#');
							}
							else if (event.key.keysym.sym == '4')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'$');
							}
							else if (event.key.keysym.sym == '5')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'%');
							}
							else if (event.key.keysym.sym == '6')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'^');
							}
							else if (event.key.keysym.sym == '7')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'&');
							}
							else if (event.key.keysym.sym == '8')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'*');
							}
							else if (event.key.keysym.sym == '9')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'(');
							}
							else if (event.key.keysym.sym == '0')
							{
								keyboardDevice_->keyDown((SDL_Keycode)')');
							}
							else if (event.key.keysym.sym == '-')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'_');
							}
							else if (event.key.keysym.sym == '=')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'+');
							}
							else if (event.key.keysym.sym == '[')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'{');
							}
							else if (event.key.keysym.sym == ']')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'}');
							}
							else if (event.key.keysym.sym == '\\')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'|');
							}
							else if (event.key.keysym.sym == ';')
							{
								keyboardDevice_->keyDown((SDL_Keycode)':');
							}
							else if (event.key.keysym.sym == '\'')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'"');
							}
							else if (event.key.keysym.sym == ',')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'<');
							}
							else if (event.key.keysym.sym == '.')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'>');
							}
							else if (event.key.keysym.sym == '/')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'?');
							}
							else if (event.key.keysym.sym == '`')
							{
								keyboardDevice_->keyDown((SDL_Keycode)'~');
							}
							else
							{
								keyboardDevice_->keyDown(event.key.keysym.sym);
							}
						}
						else if (event.key.keysym.mod & KMOD_CAPS)
						{
							if (event.key.keysym.sym >= 'a' && event.key.keysym.sym <= 'z')
							{
								keyboardDevice_->keyDown((SDL_Keycode)((int)event.key.keysym.sym - 32));
							}
							else
							{
								keyboardDevice_->keyDown(event.key.keysym.sym);
							}
						}
						else
						{
							// Change keypad events to the non-keypad value.
							switch (event.key.keysym.sym)
							{
								case SDLK_KP_1:
									keyboardDevice_->keyDown(SDLK_1);
									break;

								case SDLK_KP_2:
									keyboardDevice_->keyDown(SDLK_2);
									break;

								case SDLK_KP_3:
									keyboardDevice_->keyDown(SDLK_3);
									break;

								case SDLK_KP_4:
									keyboardDevice_->keyDown(SDLK_4);
									break;

								case SDLK_KP_5:
									keyboardDevice_->keyDown(SDLK_5);
									break;

								case SDLK_KP_6:
									keyboardDevice_->keyDown(SDLK_6);
									break;

								case SDLK_KP_7:
									keyboardDevice_->keyDown(SDLK_7);
									break;

								case SDLK_KP_8:
									keyboardDevice_->keyDown(SDLK_8);
									break;

								case SDLK_KP_9:
									keyboardDevice_->keyDown(SDLK_9);
									break;

								case SDLK_KP_0:
									keyboardDevice_->keyDown(SDLK_0);
									break;

								case SDLK_KP_ENTER:								
									keyboardDevice_->keyDown(SDLK_RETURN);
									break;

								case SDLK_KP_PERIOD:								
									keyboardDevice_->keyDown(SDLK_PERIOD);
									break;
								
								case SDLK_KP_DIVIDE:								
									keyboardDevice_->keyDown(SDLK_SLASH);
									break;
								
								case SDLK_KP_MULTIPLY:								
									keyboardDevice_->keyDown(SDLK_ASTERISK);
									break;
								
								case SDLK_KP_MINUS:								
									keyboardDevice_->keyDown(SDLK_MINUS);
									break;
								
								case SDLK_KP_PLUS:								
									keyboardDevice_->keyDown(SDLK_PLUS);
									break;

								default:
									keyboardDevice_->keyDown(event.key.keysym.sym);
									break;
							}
						}

						break;

					case SDL_KEYUP: 

						keyboardDevice_->keyUp(event.key.keysym.sym);

						break;

					default:

						break;
				}
			} 
		}
	}

	boost::static_pointer_cast<FiremelonExUi>(ui)->shutdown();

	end();

	return 0;
}

boost::shared_ptr<Factory> SdlApp::createFactory()
{
	boost::shared_ptr<Factory> factory = boost::shared_ptr<Factory>(new FiremelonExFactory());

	boost::static_pointer_cast<FiremelonExFactory>(factory)->attachKeyboardDevice(keyboardDevice_);

	return factory;
}

bool SdlApp::update()	
{	
	engine_->update();

	engine_->endUpdate();
	
	return true; 
}

bool SdlApp::begin()
{
	boost::shared_ptr<FiremelonExUi> ui = boost::static_pointer_cast<FiremelonExUi>(engine_->getUi());
	boost::shared_ptr<FmodAudioPlayer> audioPlayer = boost::static_pointer_cast<FmodAudioPlayer>(engine_->getAudioPlayer());
	//SdlRenderer* renderer = (SdlRenderer*)engine_->getRenderer();
	boost::shared_ptr<OpenGlRenderer> renderer = boost::static_pointer_cast<OpenGlRenderer>(engine_->getRenderer());
	boost::shared_ptr<InputDeviceManager> inputDeviceManager = engine_->getInputDeviceManager();
	
	engine_->setFixedTimeStep(0.01);
	
	std::cout<<"SDL Initialized"<<std::endl;

	return true; 
}

bool SdlApp::end()		
{ 
	std::cout<<"Exiting application"<<std::endl;

	return true; 
}


void SdlApp::inputDeviceRemoved(std::string deviceName)
{
	boost::shared_ptr<InputDeviceManager> inputDeviceManager = engine_->getInputDeviceManager();

	boost::shared_ptr<InputDevice> inputDevice = inputDeviceManager->getInputDeviceByName(deviceName);

	boost::shared_ptr<SdlGamepadDevice> gamepadDevice = boost::static_pointer_cast<SdlGamepadDevice>(inputDevice);

	std::cout << "Controller device " << inputDevice->getDeviceName() << " removed." << std::endl;

	gamepadDevice->deviceRemovedSignal_.disconnect(boost::bind(&SdlApp::inputDeviceRemoved, this, _1));

	boost::shared_ptr<FiremelonExUi> ui = boost::static_pointer_cast<FiremelonExUi>(engine_->getUi());

	ui->inputDeviceRemoved(inputDevice->getChannel());

	inputDeviceManager->removeInputDeviceByChannel(inputDevice->getChannel());
}