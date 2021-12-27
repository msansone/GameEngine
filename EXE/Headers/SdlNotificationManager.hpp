/* -------------------------------------------------------------------------
** SdlNotificationManager.hpp
**
** The SdlNotificationManager class is derived from the NotificationManager 
** class. It is used to show notifications dispatched by the core engine. 
** This implementation will use SDL to show them as a message box.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SDLNOTIFICATIONMANAGER_HPP_
#define _SDLNOTIFICATIONMANAGER_HPP_

#include <NotificationManager.hpp>

#include "SDL.h"

#include <string>

class SdlNotificationManager : public firemelon::NotificationManager
{
public:

	SdlNotificationManager();
	virtual ~SdlNotificationManager();

	virtual void	displayNotification(std::string message);

private:

};

#endif // _SDLNOTIFICATIONMANAGER_HPP_