/* -------------------------------------------------------------------------
** CameraEntityCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CAMERAENTITYCODEBEHIND_HPP_
#define _CAMERAENTITYCODEBEHIND_HPP_

#include "CameraScript.hpp"
#include "EntityCodeBehind.hpp"

namespace firemelon
{
	class CameraEntityCodeBehind : public EntityCodeBehind
	{
	public:

		CameraEntityCodeBehind();
		virtual ~CameraEntityCodeBehind();

	protected:

	private:

		virtual void	baseCleanup();

		virtual void	baseInitialize();

		virtual void	centered();

		virtual void	roomEntered(RoomId roomId);

		boost::shared_ptr<CameraController>	cameraController_;

		boost::shared_ptr<CameraScript>		cameraScript_;
	};
}

#endif // _CAMERAENTITYCODEBEHIND_HPP_
