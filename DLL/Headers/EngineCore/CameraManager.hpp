/* -------------------------------------------------------------------------
** CameraManager.hpp
**
** The CameraManager class is used to set the currently active camera.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CAMERAMANAGER_HPP_
#define _CAMERAMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "CameraController.hpp"

namespace firemelon
{
	class FIREMELONAPI CameraManager
	{
	public:

		CameraManager();
		virtual ~CameraManager();

		boost::shared_ptr<CameraController>	getActiveCamera();
		boost::shared_ptr<CameraController>	getActiveCameraPy();

		void								setActiveCamera(boost::shared_ptr<CameraController> camera);
		void								setActiveCameraPy(boost::shared_ptr<CameraController> camera);

	private:

		boost::shared_ptr<CameraController>	activeCamera_;
	};
}

#endif // _CAMERAMANAGER_HPP_
