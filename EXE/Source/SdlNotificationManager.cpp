#include "..\Headers\SdlNotificationManager.hpp"

using namespace firemelon;


SdlNotificationManager::SdlNotificationManager()
{
}

SdlNotificationManager::~SdlNotificationManager()
{
}

void SdlNotificationManager::displayNotification(std::string message)
{
	const SDL_MessageBoxButtonData buttons[] = {
		{ /* .flags, .buttonid, .text */        0, 0, "Ok" }
	};

	const SDL_MessageBoxColorScheme colorScheme = {
		{ /* .colors (.r, .g, .b) */
		  /* [SDL_MESSAGEBOX_COLOR_BACKGROUND] */
			{ 255,   0,   0 },
			/* [SDL_MESSAGEBOX_COLOR_TEXT] */
			{ 0, 255,   0 },
			/* [SDL_MESSAGEBOX_COLOR_BUTTON_BORDER] */
			{ 255, 255,   0 },
			/* [SDL_MESSAGEBOX_COLOR_BUTTON_BACKGROUND] */
			{ 0,   0, 255 },
			/* [SDL_MESSAGEBOX_COLOR_BUTTON_SELECTED] */
			{ 255,   0, 255 }
		}
	};

	const SDL_MessageBoxData messageBoxData = {
		SDL_MESSAGEBOX_INFORMATION, /* .flags */
		NULL, /* .window */
		"Error", /* .title */
		message.c_str(), /* .message */
		SDL_arraysize(buttons), /* .numbuttons */
		buttons, /* .buttons */
		&colorScheme /* .colorScheme */
	};

	int buttonId;

	if (SDL_ShowMessageBox(&messageBoxData, &buttonId) < 0)
	{
		SDL_Log("error displaying message box");
	}

}