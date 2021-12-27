/* -------------------------------------------------------------------------
** Camera.hpp
**
** The Camera contains the python script functions for the
** camera methods.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CAMERASCRIPT_HPP_
#define _CAMERASCRIPT_HPP_

//#include "BaseIds.hpp"
#include "CodeBehindScript.hpp"
//#include "Debugger.hpp"
//#include "EntityMetadata.hpp"
#include "CameraController.hpp"
//#include "Position.hpp"
//#include "PythonInstanceWrapper.hpp"
//#include "Types.hpp"

namespace firemelon
{
	class CameraScript : public CodeBehindScript
	{
	public:
		friend class CameraEntityCodeBehind;

		CameraScript(DebuggerPtr debugger);
		virtual ~CameraScript();

		void	centered();

	protected:

	private:

		void	cleanup();
		void	initialize();

		boost::shared_ptr<CameraController>	controller_;

		PyObj								pyCentered_;
	};
}

#endif // _CAMERASCRIPT_HPP_
