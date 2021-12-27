/* -------------------------------------------------------------------------
** CameraController.hpp
** 
** The CameraController class is the entity controller for the camera entity.
** It will always be located at index 0 of the master entity list. It contains a Rect
** structure representing the viewport that the game world is seen through. Eventually 
** this should be made polymorphic so the user can implement their own camera system.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CAMERACONTROLLER_HPP_
#define _CAMERACONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <vector>

#include "EntityController.hpp"
#include "MapLayer.hpp"
#include "Position.hpp"
#include "StateMachineController.hpp"

namespace firemelon
{
	class FIREMELONAPI CameraController : public EntityController
	{
	public:
		friend class CameraEntityCodeBehind;
		friend class CameraSimulatableCodeBehind;

		CameraController();
		virtual ~CameraController();

		int									getCameraWidth();
		int									getCameraHeight();
	
		void								setCameraHeight(int height);
		void								setCameraWidth(int width);

		void								updateBounds(boost::shared_ptr<RoomMetadata> roomMetadata);

		void								attachToEntity(EntityControllerPtr attachedEntityController);
		void								attachToEntityPy(EntityControllerPtr attachedEntityController);

		void								focusOnAttachedEntity();

		EntityControllerPtr	getAttachedEntityController();

		void								initialize();

	private:

		void			centered();

		virtual void	roomChanged(RoomId roomId, SpawnPointId spawnPoint, int offsetX, int offsetY, TransitionId transitionId, double transitionTime);

		EntityControllerPtr					attachedEntityController_;

		Rect												cameraRect_;

		bool												centerCamera_;

		boost::shared_ptr<boost::signals2::signal<void()>>	centeredSignal_;
	};
}

#endif // _CAMERACONTROLLER_HPP_