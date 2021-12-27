#include "..\..\Headers\EngineCore\CameraController.hpp"

using namespace firemelon;

CameraController::CameraController() :
	centeredSignal_(new boost::signals2::signal<void()>)
{
	cameraRect_.h = 0;
	cameraRect_.w = 0;
	cameraRect_.x = 0;
	cameraRect_.y = 0;
	
	attachedEntityController_ = nullptr;

	centerCamera_ = false;
}

CameraController::~CameraController()
{
	
}

void CameraController::initialize()
{
}

void CameraController::centered()
{
	(*centeredSignal_)();
}

void CameraController::focusOnAttachedEntity()
{
	updateBounds(getMetadata()->getRoomMetadata());

	if (attachedEntityController_ != nullptr)
	{
		DynamicsController* myDynamicsController = getDynamicsController();

		float attachedEntityX = attachedEntityController_->getPosition()->getX();
		float attachedEntityY = attachedEntityController_->getPosition()->getY();

		int attachedEntityMidpointX = (int)(attachedEntityX + ((int)(attachedEntityController_->getMetadata()->getWidth() / 2)));
		int attachedEntityMidpointY = (int)(attachedEntityY + ((int)(attachedEntityController_->getMetadata()->getHeight() / 2)));

		int cameraX = attachedEntityMidpointX - (cameraRect_.w / 2);

		myDynamicsController->relocatePositionX((float)cameraX);

		int cameraY = attachedEntityMidpointY - (cameraRect_.h / 2);

		myDynamicsController->relocatePositionY((float)cameraY);
	}
}

void CameraController::roomChanged(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime)
{
	return;
}

int CameraController::getCameraWidth()
{
	return cameraRect_.w;
}

int CameraController::getCameraHeight()
{
	return cameraRect_.h;
}

void CameraController::setCameraHeight(int height)
{
	cameraRect_.h = height;
}

void CameraController::setCameraWidth(int width)
{
	cameraRect_.w = width;
}

void CameraController::updateBounds(boost::shared_ptr<RoomMetadata> roomMetadata)
{
	int attachedEntityMidpointX = 0;
	int attachedEntityMidpointY = 0;

	if (attachedEntityController_ != nullptr)
	{
		int stageWidth = attachedEntityController_->getDynamicsController()->getOwnerStageWidth();
		int stageHeight = attachedEntityController_->getDynamicsController()->getOwnerStageHeight();

		attachedEntityMidpointX = (int)(stageWidth / 2);
		attachedEntityMidpointY = (int)(stageHeight / 2);
	}

	int mapHeight = 0;
	int mapWidth = 0;

	if (roomMetadata != nullptr)
	{
		mapHeight = roomMetadata->getMapHeight();
		mapWidth = roomMetadata->getMapWidth();
	}

	DynamicsController* myDynamicsController = getDynamicsController();

	myDynamicsController->getPosition()->clampYLo_ = 0;
	myDynamicsController->getPosition()->clampYHi_ = mapHeight - cameraRect_.h;
	myDynamicsController->getPosition()->clampXLo_ = 0;
	myDynamicsController->getPosition()->clampXHi_ = mapWidth - cameraRect_.w;
}

void CameraController::attachToEntityPy(EntityControllerPtr attachedEntityController)
{
	PythonReleaseGil unlocker;

	attachToEntity(attachedEntityController);
}

void CameraController::attachToEntity(EntityControllerPtr attachedEntityController)
{
	// Reset the layerLerp variable on the previously attached entity, if one exists.
	if (attachedEntityController_ != nullptr)
	{
		if (attachedEntityController_->getMetadata()->classification_ == ENTITY_CLASSIFICATION_ACTOR || attachedEntityController_->getMetadata()->classification_ == ENTITY_CLASSIFICATION_HUDELEMENT)
		{
			StageRenderablePtr stageRenderable = boost::static_pointer_cast<StageRenderable>(attachedEntityController_->getRenderable());
			
			if (stageRenderable != nullptr)
			{
				stageRenderable->layerLerp_ = true;
			}
		}
	}

	attachedEntityController_ = attachedEntityController;

	// Set the attachment.
	attachTo(attachedEntityController);

	// Set again in the new attached entity controller.
	if (attachedEntityController_ != nullptr)
	{
		if (attachedEntityController_->getMetadata()->classification_ == ENTITY_CLASSIFICATION_ACTOR || attachedEntityController_->getMetadata()->classification_ == ENTITY_CLASSIFICATION_HUDELEMENT)
		{
			// Attached entity should not calculate layer lerp when rendering.
			StageRenderablePtr stageRenderable = boost::static_pointer_cast<StageRenderable>(attachedEntityController_->getRenderable());
			
			if (stageRenderable != nullptr)
			{
				stageRenderable->layerLerp_ = false;
			}
		}
	}
	
	focusOnAttachedEntity();
}

EntityControllerPtr CameraController::getAttachedEntityController()
{
	return attachedEntityController_;
}