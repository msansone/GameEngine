#include "..\..\Headers\EngineCore\CameraEntityCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

CameraEntityCodeBehind::CameraEntityCodeBehind()
{
}

CameraEntityCodeBehind::~CameraEntityCodeBehind()
{

}

void CameraEntityCodeBehind::baseInitialize()
{
	cameraScript_ = boost::shared_ptr<CameraScript>(new CameraScript(debugger_));

	cameraController_ = boost::static_pointer_cast<CameraController>(getEntityController());

	cameraController_->centeredSignal_->connect(boost::bind(&CameraEntityCodeBehind::centered, this));

	cameraScript_->setPythonInstanceWrapper(getPythonInstanceWrapper());

	cameraScript_->controller_ = cameraController_;

	cameraScript_->preInitialize();
}

void CameraEntityCodeBehind::baseCleanup()
{
	cameraController_->centeredSignal_->disconnect(boost::bind(&CameraEntityCodeBehind::centered, this));

	cameraScript_->preCleanup();
}

void CameraEntityCodeBehind::centered()
{
	cameraScript_->centered();
}

void CameraEntityCodeBehind::roomEntered(RoomId roomId)
{
	// Doing it here is wrong. The room is "entered" before the transition completes. It should wait until the room is shown.
	//cameraController_->updateBounds(getMetadata()->getRoomMetadata());

	//// Need to refocus on the entity when entering a new room.
	//cameraController_->centerCamera_ = true;

	//cameraController_->focusOnAttachedEntity();
}