#include "..\Headers\GameApp.hpp"

using namespace firemelon;
using namespace boost::python;

// Define all of the firemelon classes to be exposed to Python here.
BOOST_PYTHON_MODULE(firemelon_ex) {

	// Key definitions
	enum_<SDL_Scancode>("Scancode")
		.value("A", SDL_SCANCODE_A)
		.value("B", SDL_SCANCODE_B)
		.value("C", SDL_SCANCODE_C)
		.value("D", SDL_SCANCODE_D)
		.value("E", SDL_SCANCODE_E)
		.value("F", SDL_SCANCODE_F)
		.value("G", SDL_SCANCODE_G)
		.value("H", SDL_SCANCODE_H)
		.value("I", SDL_SCANCODE_I)
		.value("J", SDL_SCANCODE_J)
		.value("K", SDL_SCANCODE_K)
		.value("L", SDL_SCANCODE_L)
		.value("M", SDL_SCANCODE_M)
		.value("N", SDL_SCANCODE_N)
		.value("O", SDL_SCANCODE_O)
		.value("P", SDL_SCANCODE_P)
		.value("Q", SDL_SCANCODE_Q)
		.value("R", SDL_SCANCODE_R)
		.value("S", SDL_SCANCODE_S)
		.value("T", SDL_SCANCODE_T)
		.value("U", SDL_SCANCODE_U)
		.value("V", SDL_SCANCODE_V)
		.value("W", SDL_SCANCODE_W)
		.value("X", SDL_SCANCODE_X)
		.value("Y", SDL_SCANCODE_Y)
		.value("Z", SDL_SCANCODE_Z)
		.value("1", SDL_SCANCODE_1)
		.value("2", SDL_SCANCODE_2)
		.value("3", SDL_SCANCODE_3)
		.value("4", SDL_SCANCODE_4)
		.value("5", SDL_SCANCODE_5)
		.value("6", SDL_SCANCODE_6)
		.value("7", SDL_SCANCODE_7)
		.value("8", SDL_SCANCODE_8)
		.value("9", SDL_SCANCODE_9)
		.value("0", SDL_SCANCODE_0)
		.value("RETURN", SDL_SCANCODE_RETURN)
		.value("ESCAPE", SDL_SCANCODE_ESCAPE)
		.value("BACKSPACE", SDL_SCANCODE_BACKSPACE)
		.value("TAB", SDL_SCANCODE_TAB)
		.value("SPACE", SDL_SCANCODE_SPACE)
		.value("MINUS", SDL_SCANCODE_MINUS)
		.value("EQUALS", SDL_SCANCODE_EQUALS)
		.value("LEFTBRACKET", SDL_SCANCODE_LEFTBRACKET)
		.value("RIGHTBRACKET", SDL_SCANCODE_RIGHTBRACKET)
		.value("BACKSLASH", SDL_SCANCODE_BACKSLASH)
		.value("NONUSHASH", SDL_SCANCODE_NONUSHASH)
		.value("SEMICOLON", SDL_SCANCODE_SEMICOLON)
		.value("APOSTROPHE", SDL_SCANCODE_APOSTROPHE)
		.value("GRAVE", SDL_SCANCODE_GRAVE)
		.value("COMMA", SDL_SCANCODE_COMMA)
		.value("PERIOD", SDL_SCANCODE_PERIOD)
		.value("SLASH", SDL_SCANCODE_SLASH)
		.value("CAPSLOCK", SDL_SCANCODE_CAPSLOCK)
		.value("F1", SDL_SCANCODE_F1)
		.value("F2", SDL_SCANCODE_F2)
		.value("F3", SDL_SCANCODE_F3)
		.value("F4", SDL_SCANCODE_F4)
		.value("F5", SDL_SCANCODE_F5)
		.value("F6", SDL_SCANCODE_F6)
		.value("F7", SDL_SCANCODE_F7)
		.value("F8", SDL_SCANCODE_F8)
		.value("F9", SDL_SCANCODE_F9)
		.value("F10", SDL_SCANCODE_F10)
		.value("F11", SDL_SCANCODE_F11)
		.value("F12", SDL_SCANCODE_F12)
		.value("PRINTSCREEN", SDL_SCANCODE_PRINTSCREEN)
		.value("SCROLLLOCK", SDL_SCANCODE_SCROLLLOCK)
		.value("PAUSE", SDL_SCANCODE_PAUSE)
		.value("INSERT", SDL_SCANCODE_INSERT)
		.value("HOME", SDL_SCANCODE_HOME)
		.value("PAGEUP", SDL_SCANCODE_PAGEUP)
		.value("DELETE", SDL_SCANCODE_DELETE)
		.value("END", SDL_SCANCODE_END)
		.value("PAGEDOWN", SDL_SCANCODE_PAGEDOWN)
		.value("RIGHT", SDL_SCANCODE_RIGHT)
		.value("LEFT", SDL_SCANCODE_LEFT)
		.value("DOWN", SDL_SCANCODE_DOWN)
		.value("UP", SDL_SCANCODE_UP)
		.value("LCTRL", SDL_SCANCODE_LCTRL)
		.value("LSHIFT", SDL_SCANCODE_LSHIFT)
		.value("LALT", SDL_SCANCODE_LALT)
		.value("RCTRL", SDL_SCANCODE_RCTRL)
		.value("RSHIFT", SDL_SCANCODE_RSHIFT)
		.value("RALT", SDL_SCANCODE_RALT);

	enum_<SDL_Keycode>("Keycode")
		.value("SDLK_UNKNOWN", SDLK_UNKNOWN)
		.value("SDLK_BACKSPACE", SDLK_BACKSPACE)
		.value("SDLK_TAB", SDLK_TAB)
		.value("SDLK_RETURN", SDLK_RETURN)
		.value("SDLK_ESCAPE", SDLK_ESCAPE)
		.value("SDLK_SPACE", SDLK_SPACE)
		.value("SDLK_EXCLAIM", SDLK_EXCLAIM)
		.value("SDLK_QUOTEDBL", SDLK_QUOTEDBL)
		.value("SDLK_HASH", SDLK_HASH)
		.value("SDLK_DOLLAR", SDLK_DOLLAR)
		.value("SDLK_PERCENT", SDLK_PERCENT)
		.value("SDLK_AMPERSAND", SDLK_AMPERSAND)
		.value("SDLK_QUOTE", SDLK_QUOTE)
		.value("SDLK_LEFTPAREN", SDLK_LEFTPAREN)
		.value("SDLK_RIGHTPAREN", SDLK_RIGHTPAREN)
		.value("SDLK_ASTERISK", SDLK_ASTERISK)
		.value("SDLK_PLUS", SDLK_PLUS)
		.value("SDLK_COMMA", SDLK_COMMA)
		.value("SDLK_MINUS", SDLK_MINUS)
		.value("SDLK_PERIOD", SDLK_PERIOD)
		.value("SDLK_SLASH", SDLK_SLASH)
		.value("SDLK_0", SDLK_0)
		.value("SDLK_1", SDLK_1)
		.value("SDLK_2", SDLK_2)
		.value("SDLK_3", SDLK_3)
		.value("SDLK_4", SDLK_4)
		.value("SDLK_5", SDLK_5)
		.value("SDLK_6", SDLK_6)
		.value("SDLK_7", SDLK_7)
		.value("SDLK_8", SDLK_8)
		.value("SDLK_9", SDLK_9)
		.value("SDLK_COLON", SDLK_COLON)
		.value("SDLK_SEMICOLON", SDLK_SEMICOLON)
		.value("SDLK_LESS", SDLK_LESS)
		.value("SDLK_EQUALS", SDLK_EQUALS)
		.value("SDLK_GREATER", SDLK_GREATER)
		.value("SDLK_QUESTION", SDLK_QUESTION)
		.value("SDLK_AT", SDLK_AT)
		.value("SDLK_LEFTBRACKET", SDLK_LEFTBRACKET)
		.value("SDLK_BACKSLASH", SDLK_BACKSLASH)
		.value("SDLK_RIGHTBRACKET", SDLK_RIGHTBRACKET)
		.value("SDLK_CARET", SDLK_CARET)
		.value("SDLK_UNDERSCORE", SDLK_UNDERSCORE)
		.value("SDLK_BACKQUOTE", SDLK_BACKQUOTE)
		.value("SDLK_a", SDLK_a)
		.value("SDLK_b", SDLK_b)
		.value("SDLK_c", SDLK_c)
		.value("SDLK_d", SDLK_d)
		.value("SDLK_e", SDLK_e)
		.value("SDLK_f", SDLK_f)
		.value("SDLK_g", SDLK_g)
		.value("SDLK_h", SDLK_h)
		.value("SDLK_i", SDLK_i)
		.value("SDLK_j", SDLK_j)
		.value("SDLK_k", SDLK_k)
		.value("SDLK_l", SDLK_l)
		.value("SDLK_m", SDLK_m)
		.value("SDLK_n", SDLK_n)
		.value("SDLK_o", SDLK_o)
		.value("SDLK_p", SDLK_p)
		.value("SDLK_q", SDLK_q)
		.value("SDLK_r", SDLK_r)
		.value("SDLK_s", SDLK_s)
		.value("SDLK_t", SDLK_t)
		.value("SDLK_u", SDLK_u)
		.value("SDLK_v", SDLK_v)
		.value("SDLK_w", SDLK_w)
		.value("SDLK_x", SDLK_x)
		.value("SDLK_y", SDLK_y)
		.value("SDLK_z", SDLK_z)
		.value("SDLK_DELETE", SDLK_DELETE)
		.value("SDLK_CAPSLOCK", SDLK_CAPSLOCK)
		.value("SDLK_F1", SDLK_F1)
		.value("SDLK_F2", SDLK_F2)
		.value("SDLK_F3", SDLK_F3)
		.value("SDLK_F4", SDLK_F4)
		.value("SDLK_F5", SDLK_F5)
		.value("SDLK_F6", SDLK_F6)
		.value("SDLK_F7", SDLK_F7)
		.value("SDLK_F8", SDLK_F8)
		.value("SDLK_F9", SDLK_F9)
		.value("SDLK_F10", SDLK_F10)
		.value("SDLK_F11", SDLK_F11)
		.value("SDLK_F12", SDLK_F12)
		.value("SDLK_PRINTSCREEN", SDLK_PRINTSCREEN)
		.value("SDLK_SCROLLLOCK", SDLK_SCROLLLOCK)
		.value("SDLK_PAUSE", SDLK_PAUSE)
		.value("SDLK_INSERT", SDLK_INSERT)
		.value("SDLK_HOME", SDLK_HOME)
		.value("SDLK_PAGEUP", SDLK_PAGEUP)
		.value("SDLK_END", SDLK_END)
		.value("SDLK_PAGEDOWN", SDLK_PAGEDOWN)
		.value("SDLK_RIGHT", SDLK_RIGHT)
		.value("SDLK_LEFT", SDLK_LEFT)
		.value("SDLK_DOWN", SDLK_DOWN)
		.value("SDLK_UP", SDLK_UP)
		.value("SDLK_NUMLOCKCLEAR", SDLK_NUMLOCKCLEAR)
		.value("SDLK_KP_DIVIDE", SDLK_KP_DIVIDE)
		.value("SDLK_KP_MULTIPLY", SDLK_KP_MULTIPLY)
		.value("SDLK_KP_MINUS", SDLK_KP_MINUS)
		.value("SDLK_KP_PLUS", SDLK_KP_PLUS)
		.value("SDLK_KP_ENTER", SDLK_KP_ENTER)
		.value("SDLK_KP_1", SDLK_KP_1)
		.value("SDLK_KP_2", SDLK_KP_2)
		.value("SDLK_KP_3", SDLK_KP_3)
		.value("SDLK_KP_4", SDLK_KP_4)
		.value("SDLK_KP_5", SDLK_KP_5)
		.value("SDLK_KP_6", SDLK_KP_6)
		.value("SDLK_KP_7", SDLK_KP_7)
		.value("SDLK_KP_8", SDLK_KP_8)
		.value("SDLK_KP_9", SDLK_KP_9)
		.value("SDLK_KP_0", SDLK_KP_0)
		.value("SDLK_KP_PERIOD", SDLK_KP_PERIOD)
		.value("SDLK_APPLICATION", SDLK_APPLICATION)
		.value("SDLK_POWER", SDLK_POWER)
		.value("SDLK_KP_EQUALS", SDLK_KP_EQUALS)
		.value("SDLK_F13", SDLK_F13)
		.value("SDLK_F14", SDLK_F14)
		.value("SDLK_F15", SDLK_F15)
		.value("SDLK_F16", SDLK_F16)
		.value("SDLK_F17", SDLK_F17)
		.value("SDLK_F18", SDLK_F18)
		.value("SDLK_F19", SDLK_F19)
		.value("SDLK_F20", SDLK_F20)
		.value("SDLK_F21", SDLK_F21)
		.value("SDLK_F22", SDLK_F22)
		.value("SDLK_F23", SDLK_F23)
		.value("SDLK_F24", SDLK_F24)
		.value("SDLK_EXECUTE", SDLK_EXECUTE)
		.value("SDLK_HELP", SDLK_HELP)
		.value("SDLK_MENU", SDLK_MENU)
		.value("SDLK_SELECT", SDLK_SELECT)
		.value("SDLK_STOP", SDLK_STOP)
		.value("SDLK_AGAIN", SDLK_AGAIN)
		.value("SDLK_UNDO", SDLK_UNDO)
		.value("SDLK_CUT", SDLK_CUT)
		.value("SDLK_COPY", SDLK_COPY)
		.value("SDLK_PASTE", SDLK_PASTE)
		.value("SDLK_FIND", SDLK_FIND)
		.value("SDLK_MUTE", SDLK_MUTE)
		.value("SDLK_VOLUMEUP", SDLK_VOLUMEUP)
		.value("SDLK_VOLUMEDOWN", SDLK_VOLUMEDOWN)
		.value("SDLK_KP_COMMA", SDLK_KP_COMMA)
		.value("SDLK_KP_EQUALSAS400", SDLK_KP_EQUALSAS400)
		.value("SDLK_ALTERASE", SDLK_ALTERASE)
		.value("SDLK_SYSREQ", SDLK_SYSREQ)
		.value("SDLK_CANCEL", SDLK_CANCEL)
		.value("SDLK_CLEAR", SDLK_CLEAR)
		.value("SDLK_PRIOR", SDLK_PRIOR)
		.value("SDLK_RETURN2", SDLK_RETURN2)
		.value("SDLK_SEPARATOR", SDLK_SEPARATOR)
		.value("SDLK_OUT", SDLK_OUT)
		.value("SDLK_OPER", SDLK_OPER)
		.value("SDLK_CLEARAGAIN", SDLK_CLEARAGAIN)
		.value("SDLK_CRSEL", SDLK_CRSEL)
		.value("SDLK_EXSEL", SDLK_EXSEL)
		.value("SDLK_KP_00", SDLK_KP_00)
		.value("SDLK_KP_000", SDLK_KP_000)
		.value("SDLK_THOUSANDSSEPARATOR", SDLK_THOUSANDSSEPARATOR)
		.value("SDLK_DECIMALSEPARATOR", SDLK_DECIMALSEPARATOR)
		.value("SDLK_CURRENCYUNIT", SDLK_CURRENCYUNIT)
		.value("SDLK_CURRENCYSUBUNIT", SDLK_CURRENCYSUBUNIT)
		.value("SDLK_KP_LEFTPAREN", SDLK_KP_LEFTPAREN)
		.value("SDLK_KP_RIGHTPAREN", SDLK_KP_RIGHTPAREN)
		.value("SDLK_KP_LEFTBRACE", SDLK_KP_LEFTBRACE)
		.value("SDLK_KP_RIGHTBRACE", SDLK_KP_RIGHTBRACE)
		.value("SDLK_KP_TAB", SDLK_KP_TAB)
		.value("SDLK_KP_BACKSPACE", SDLK_KP_BACKSPACE)
		.value("SDLK_KP_A", SDLK_KP_A)
		.value("SDLK_KP_B", SDLK_KP_B)
		.value("SDLK_KP_C", SDLK_KP_C)
		.value("SDLK_KP_D", SDLK_KP_D)
		.value("SDLK_KP_E", SDLK_KP_E)
		.value("SDLK_KP_F", SDLK_KP_F)
		.value("SDLK_KP_XOR", SDLK_KP_XOR)
		.value("SDLK_KP_POWER", SDLK_KP_POWER)
		.value("SDLK_KP_PERCENT", SDLK_KP_PERCENT)
		.value("SDLK_KP_LESS", SDLK_KP_LESS)
		.value("SDLK_KP_GREATER", SDLK_KP_GREATER)
		.value("SDLK_KP_AMPERSAND", SDLK_KP_AMPERSAND)
		.value("SDLK_KP_DBLAMPERSAND", SDLK_KP_DBLAMPERSAND)
		.value("SDLK_KP_VERTICALBAR", SDLK_KP_VERTICALBAR)
		.value("SDLK_KP_DBLVERTICALBAR", SDLK_KP_DBLVERTICALBAR)
		.value("SDLK_KP_COLON", SDLK_KP_COLON)
		.value("SDLK_KP_HASH", SDLK_KP_HASH)
		.value("SDLK_KP_SPACE", SDLK_KP_SPACE)
		.value("SDLK_KP_AT", SDLK_KP_AT)
		.value("SDLK_KP_EXCLAM", SDLK_KP_EXCLAM)
		.value("SDLK_KP_MEMSTORE", SDLK_KP_MEMSTORE)
		.value("SDLK_KP_MEMRECALL", SDLK_KP_MEMRECALL)
		.value("SDLK_KP_MEMCLEAR", SDLK_KP_MEMCLEAR)
		.value("SDLK_KP_MEMADD", SDLK_KP_MEMADD)
		.value("SDLK_KP_MEMSUBTRACT", SDLK_KP_MEMSUBTRACT)
		.value("SDLK_KP_MEMMULTIPLY", SDLK_KP_MEMMULTIPLY)
		.value("SDLK_KP_MEMDIVIDE", SDLK_KP_MEMDIVIDE)
		.value("SDLK_KP_PLUSMINUS", SDLK_KP_PLUSMINUS)
		.value("SDLK_KP_CLEAR", SDLK_KP_CLEAR)
		.value("SDLK_KP_CLEARENTRY", SDLK_KP_CLEARENTRY)
		.value("SDLK_KP_BINARY", SDLK_KP_BINARY)
		.value("SDLK_KP_OCTAL", SDLK_KP_OCTAL)
		.value("SDLK_KP_DECIMAL", SDLK_KP_DECIMAL)
		.value("SDLK_KP_HEXADECIMAL", SDLK_KP_HEXADECIMAL)
		.value("SDLK_LCTRL", SDLK_LCTRL)
		.value("SDLK_LSHIFT", SDLK_LSHIFT)
		.value("SDLK_LALT", SDLK_LALT)
		.value("SDLK_LGUI", SDLK_LGUI)
		.value("SDLK_RCTRL", SDLK_RCTRL)
		.value("SDLK_RSHIFT", SDLK_RSHIFT)
		.value("SDLK_RALT", SDLK_RALT)
		.value("SDLK_RGUI", SDLK_RGUI)
		.value("SDLK_MODE", SDLK_MODE)
		.value("SDLK_AUDIONEXT", SDLK_AUDIONEXT)
		.value("SDLK_AUDIOPREV", SDLK_AUDIOPREV)
		.value("SDLK_AUDIOSTOP", SDLK_AUDIOSTOP)
		.value("SDLK_AUDIOPLAY", SDLK_AUDIOPLAY)
		.value("SDLK_AUDIOMUTE", SDLK_AUDIOMUTE)
		.value("SDLK_MEDIASELECT", SDLK_MEDIASELECT)
		.value("SDLK_WWW", SDLK_WWW)
		.value("SDLK_MAIL", SDLK_MAIL)
		.value("SDLK_CALCULATOR", SDLK_CALCULATOR)
		.value("SDLK_COMPUTER", SDLK_COMPUTER)
		.value("SDLK_AC_SEARCH", SDLK_AC_SEARCH)
		.value("SDLK_AC_HOME", SDLK_AC_HOME)
		.value("SDLK_AC_BACK", SDLK_AC_BACK)
		.value("SDLK_AC_FORWARD", SDLK_AC_FORWARD)
		.value("SDLK_AC_STOP", SDLK_AC_STOP)
		.value("SDLK_AC_REFRESH", SDLK_AC_REFRESH)
		.value("SDLK_AC_BOOKMARKS", SDLK_AC_BOOKMARKS)
		.value("SDLK_BRIGHTNESSDOWN", SDLK_BRIGHTNESSDOWN)
		.value("SDLK_BRIGHTNESSUP", SDLK_BRIGHTNESSUP)
		.value("SDLK_DISPLAYSWITCH", SDLK_DISPLAYSWITCH)
		.value("SDLK_KBDILLUMTOGGLE", SDLK_KBDILLUMTOGGLE)
		.value("SDLK_KBDILLUMDOWN", SDLK_KBDILLUMDOWN)
		.value("SDLK_KBDILLUMUP", SDLK_KBDILLUMUP)
		.value("SDLK_EJECT", SDLK_EJECT)
		.value("SDLK_SLEEP", SDLK_SLEEP);

		//findme
	//// Gamepad button definitions
	//enum_<GamepadButton>("GamepadButton")
	//	.value("UP", GamepadButtonUp)
	//	.value("RIGHT", GamepadButtonRight)
	//	.value("LEFT", GamepadButtonLeft)
	//	.value("ONE", GamepadButtonOne)
	//	.value("TWO", GamepadButtonTwo)
	//	.value("THREE", GamepadButtonThree)
	//	.value("FOUR", GamepadButtonFour)
	//	.value("L1", GamepadButtonL1)
	//	.value("R1", GamepadButtonR1)
	//	.value("L2", GamepadButtonL2)
	//	.value("R2", GamepadButtonR2)
	//	.value("SELECT", GamepadButtonSelect)
	//	.value("START", GamepadButtonStart)
	//	.value("LEFTANALOG", GamepadButtonLeftAnalog)
	//	.value("RIGHTANALOG", GamepadButtonRightAnalog);
	
	// OpenGL Renderer Python Definition
	class_<OpenGlRenderer, bases<Renderer>>("OpenGlRenderer", no_init)
		.add_property("isFullscreen", &OpenGlRenderer::getIsFullscreenPy, &OpenGlRenderer::setIsFullscreenPy);

	// Boost Timer Python Definition
	class_<BoostGameTimer, bases<GameTimer>>("BoostTimer", no_init)
		.add_property("fps", &BoostGameTimer::getFpsPy)
		.add_property("totalTime", &BoostGameTimer::getTotalTimePy)
		.add_property("deltaTime", &BoostGameTimer::getTimeElapsedPy)
		.add_property("maxDeltaTime", &BoostGameTimer::getMaxTimeElapsedPy)
		.add_property("minDeltaTime", &BoostGameTimer::getMinTimeElapsedPy)
		.add_property("avgDeltaTime", &BoostGameTimer::getAvgTimeElapsedPy)
		.def("getLoggedTime", &BoostGameTimer::getLoggedTimePy)
		.def("getTimerElapsed", &BoostGameTimer::getTimerElapsedPy);


	// FMOD Audio Player Python Definition
	class_<FmodAudioPlayer, bases<AudioPlayer>>("FmodAudioPlayer", no_init)
		.def("playMusic", &FmodAudioPlayer::playMusicByNamePy);
}

/*
** Known bug in VS2015 update 3. Need to explicitly specify conversion to pointer.
** See: https://stackoverflow.com/questions/38261530/unresolved-external-symbols-since-visual-studio-2015-update-3-boost-python-link
*/
namespace boost
{
	template <>
	BoostGameTimer const volatile * get_pointer<class BoostGameTimer const volatile >(
		class BoostGameTimer const volatile *value)
	{
		return value;
	}

	template <>
	FmodAudioPlayer const volatile * get_pointer<class FmodAudioPlayer const volatile >(
		class FmodAudioPlayer const volatile *value)
	{
		return value;
	}

	template <>
	OpenGlRenderer const volatile * get_pointer<class OpenGlRenderer const volatile >(
		class OpenGlRenderer const volatile *value)
	{
		return value;
	}
}

GameApp::GameApp(GameEngine* engine)
{
	engine_ = engine;
}

GameApp::~GameApp()
{
}

int GameApp::appMain(int argc, char* args[])
{
	bool initSuccess = initializeApp();

	if (initSuccess == true)
	{
		userAppBegin();

		// Call the main method for the derived application class.
		userMain(argc, args);
	}
	else
	{
		std::cout<<"Application initialization failed."<<std::endl;
	}

	return 0;
}

bool GameApp::initializeApp()
{
	bool initSuccess = userInitialize();

	if (initSuccess = true)
	{
		factory_ = createFactory();
	
		engine_->setFactory(factory_);
		
		PyImport_AppendInittab("firemelon_ex", &PyInit_firemelon_ex);

		initSuccess = engine_->initialize();

		if (initSuccess == true)
		{
			PythonAcquireGil lock;

			try
			{
				//object main_module = import("__main__");
				//object main_namespace = main_module.attr("__dict__");
				object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
				object pyFiremelonNamespace = pyFiremelonModule.attr("__dict__");

				auto audioPlayer = engine_->getAudioPlayer();

				boost::shared_ptr<FmodAudioPlayer> fmodAudioPlayer = boost::static_pointer_cast<FmodAudioPlayer>(audioPlayer);

				pyFiremelonNamespace["audioPlayer"] = ptr(fmodAudioPlayer.get());

				auto renderer = engine_->getRenderer();

				boost::shared_ptr<OpenGlRenderer> openGlRenderer = boost::static_pointer_cast<OpenGlRenderer>(renderer);

				pyFiremelonNamespace["renderer"] = ptr(openGlRenderer.get());


				auto timer = engine_->getGameTimer();

				boost::shared_ptr<BoostGameTimer> boostGameTimer = boost::static_pointer_cast<BoostGameTimer>(timer);

				pyFiremelonNamespace["timer"] = ptr(boostGameTimer.get());
			}
			catch (error_already_set &)
			{
				std::cout << "Error initializing extended engine features." << std::endl;
			}
		}
	}

	return initSuccess;
}

void GameApp::shutdownApp()
{
	userShutdown();
}