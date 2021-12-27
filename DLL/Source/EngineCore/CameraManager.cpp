#include "..\..\Headers\EngineCore\CameraManager.hpp"

using namespace firemelon;

CameraManager::CameraManager()
{
}

CameraManager::~CameraManager()
{
	bool debug = true;
}

boost::shared_ptr<CameraController> CameraManager::getActiveCameraPy()
{
	PythonReleaseGil unlocker;

	return getActiveCamera();
}

boost::shared_ptr<CameraController> CameraManager::getActiveCamera()
{
	return activeCamera_;
}

void CameraManager::setActiveCameraPy(boost::shared_ptr<CameraController> camera)
{
	PythonReleaseGil unlocker;

	setActiveCamera(camera);
}

void CameraManager::setActiveCamera(boost::shared_ptr<CameraController> camera)
{
	activeCamera_ = camera;
}
