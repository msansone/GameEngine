#include "..\..\Headers\EngineCore\CameraSimulatableCodeBehind.hpp"

using namespace firemelon;
using namespace boost::python;

CameraSimulatableCodeBehind::CameraSimulatableCodeBehind()
{
}

CameraSimulatableCodeBehind::~CameraSimulatableCodeBehind()
{
	bool debug = true;
}

void CameraSimulatableCodeBehind::initialize()
{
	cameraController_ = boost::static_pointer_cast<CameraController>(getEntityController());
}

void CameraSimulatableCodeBehind::preIntegration()
{
	EntityControllerPtr attachedEntityController = cameraController_->getAttachedEntityController();

	if (attachedEntityController != nullptr)
	{
		DynamicsController* attachedController = attachedEntityController->getDynamicsController();

		if (attachedEntityController->getDynamicsController() != nullptr)
		{
			DynamicsController* myDynamicsController = cameraController_->getDynamicsController();

			float attachedEntityX = attachedController->getPositionX();
			float attachedEntityY = attachedController->getPositionY();

			int attachedEntityMidpointX = (int)(attachedEntityX + ((int)(attachedController->getOwnerStageWidth() / 2)));
			int attachedEntityMidpointY = (int)(attachedEntityY + ((int)(attachedController->getOwnerStageHeight() / 2)));

			// The initial camera position must be set after attaching, but the attached entity's dynamics controller
			// may not have been initialized at the point of attachment. Instead set a flag that indiciates if it must 
			// be centered, and if so do it here.
			if (cameraController_->centerCamera_ == true)
			{
				// I removed this for some reason, but I don't recall why. Presumably something was wrong with it. But now 
				// I found that I need it, because the camera offset from the player is ending up skewed when moving into a new room.
				int cameraX = attachedEntityMidpointX - (cameraController_->getCameraWidth() / 2);

				myDynamicsController->relocatePositionX((float)cameraX);

				int cameraY = attachedEntityMidpointY - (cameraController_->getCameraHeight() / 2);

				myDynamicsController->relocatePositionY((float)cameraY);

				cameraController_->centerCamera_ = false;

				cameraController_->centered();
			}
		}
	}
	
	SimulatableCodeBehind::preIntegration();
}