/* -------------------------------------------------------------------------
** FiremelonExFactory.hpp
** 
** The FiremelonExFactory class is generated by the editor. It is derived from 
** the base Factory class and is used to create the engine subsystem objects 
** that the user is responsible for implementing, such as the renderer and the
** audio player. It is also used to create the classes generated by the editor.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _FIREMELONEXFACTORY_HPP_
#define _FIREMELONEXFACTORY_HPP_

#include <boost/shared_ptr.hpp>

#include <Factory.hpp>

#include "BoostGameTimer.hpp"
#include "FiremelonExCodeBehindFactory.hpp"
#include "FiremelonExUiWidgetFactory.hpp"
#include "FiremelonExUi.hpp"
#include "FmodAudioPlayer.hpp"
#include "OpenGlRenderer.hpp"
#include "SdlNotificationManager.hpp"
#include "SdlKeyboardDevice.hpp"
#include "SdlRenderer.hpp"

class FiremelonExFactory : public firemelon::Factory
{
public:

	FiremelonExFactory();
	virtual ~FiremelonExFactory();
	
	void	attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice);

private:

	virtual boost::shared_ptr<firemelon::AudioPlayer>			createUserAudioPlayer();
	virtual boost::shared_ptr<firemelon::CodeBehindFactory>		createUserCodeBehindFactory();
	virtual boost::shared_ptr<firemelon::NotificationManager>	createUserNotificationManager();
	virtual boost::shared_ptr<firemelon::GameTimer>				createUserGameTimer();
	virtual boost::shared_ptr<firemelon::Renderer>				createUserRenderer();
	virtual boost::shared_ptr<firemelon::Ui>					createUserUi();
	virtual boost::shared_ptr<firemelon::UiWidgetFactory>		createUserUiWidgetFactory();

	int	screenWidth_;
	int	screenHeight_;
	int	screenBpp_;

	boost::shared_ptr<SdlKeyboardDevice> keyboardDevice_;
};

#endif // _FIREMELONEXFACTORY_HPP_