#include "..\Headers\FiremelonExFactory.hpp"

using namespace firemelon;

FiremelonExFactory::FiremelonExFactory()
{
	screenWidth_ = 640;
	screenHeight_ = 480;
	screenBpp_ = 32;
}

FiremelonExFactory::~FiremelonExFactory()
{
}

boost::shared_ptr<Renderer> FiremelonExFactory::createUserRenderer()
{
	return boost::shared_ptr<OpenGlRenderer>(new OpenGlRenderer(screenWidth_, screenHeight_));
	//return  boost::shared_ptr<SdlRenderer>(new SdlRenderer(screenWidth_, screenHeight_));
}

boost::shared_ptr<GameTimer> FiremelonExFactory::createUserGameTimer()
{
	return boost::shared_ptr<GameTimer>(new BoostGameTimer());
}

boost::shared_ptr<AudioPlayer> FiremelonExFactory::createUserAudioPlayer()
{
	return boost::shared_ptr<AudioPlayer>(new FmodAudioPlayer());
}

boost::shared_ptr<NotificationManager> FiremelonExFactory::createUserNotificationManager()
{
	return boost::shared_ptr<NotificationManager>(new SdlNotificationManager());
}

boost::shared_ptr<Ui> FiremelonExFactory::createUserUi()
{
	boost::shared_ptr<Ui> ui = boost::shared_ptr<Ui>(new FiremelonExUi());

	boost::static_pointer_cast<FiremelonExUi>(ui)->attachKeyboardDevice(keyboardDevice_);

	return ui;
}

boost::shared_ptr<CodeBehindFactory> FiremelonExFactory::createUserCodeBehindFactory()
{
	boost::shared_ptr<CodeBehindFactory> codeBehindFactory = boost::shared_ptr<CodeBehindFactory>(new FiremelonExCodeBehindFactory());

	boost::static_pointer_cast<FiremelonExCodeBehindFactory>(codeBehindFactory)->attachKeyboardDevice(keyboardDevice_);

	return codeBehindFactory;
}

boost::shared_ptr<UiWidgetFactory> FiremelonExFactory::createUserUiWidgetFactory()
{
	boost::shared_ptr<UiWidgetFactory> uiWidgetFactory = boost::shared_ptr<UiWidgetFactory>(new FiremelonExUiWidgetFactory());

	boost::static_pointer_cast<FiremelonExUiWidgetFactory>(uiWidgetFactory)->attachKeyboardDevice(keyboardDevice_);

	return uiWidgetFactory;
}

void FiremelonExFactory::attachKeyboardDevice(boost::shared_ptr<SdlKeyboardDevice> keyboardDevice)
{
	keyboardDevice_ = keyboardDevice;
}
