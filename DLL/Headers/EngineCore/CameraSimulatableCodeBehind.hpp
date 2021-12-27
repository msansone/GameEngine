/* -------------------------------------------------------------------------
** CameraSimulatableCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CAMERASIMULATABLECODEBEHIND_HPP_
#define _CAMERASIMULATABLECODEBEHIND_HPP_

#include "CameraController.hpp"
#include "SimulatableCodeBehind.hpp"

namespace firemelon
{
	class CameraSimulatableCodeBehind : public SimulatableCodeBehind
	{
	public:

		CameraSimulatableCodeBehind();
		virtual ~CameraSimulatableCodeBehind();

	protected:

	private:

		virtual void	initialize();

		virtual void	preIntegration();

		boost::shared_ptr<CameraController>	cameraController_;
	};
}

#endif // _CAMERASIMULATABLECODEBEHIND_HPP_
