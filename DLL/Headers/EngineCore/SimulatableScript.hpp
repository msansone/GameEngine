/* -------------------------------------------------------------------------
** SimulatableScript.hpp
**
** The SimulatableScript is the interface by which the simulation functions
** of a python script are called.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SIMULATABLESCRIPT_HPP_
#define _SIMULATABLESCRIPT_HPP_

#include "BaseIds.hpp"
#include "CodeBehindScript.hpp"
#include "Debugger.hpp"
#include "DynamicsControllerHolder.hpp"
#include "Types.hpp"


namespace firemelon
{
	class SimulatableScript : public CodeBehindScript
	{
	public:
		friend class SimulatableCodeBehind;

		SimulatableScript(DebuggerPtr debugger);
		virtual ~SimulatableScript();

		PyObj getPyDynamicsController();

	protected:

	private:

		virtual void	cleanup();
		void			frameBegin();
		virtual void	initialize();
		void			start();
		void			preIntegration();
		void			postIntegration();		

		boost::shared_ptr<DynamicsControllerHolder>	dynamicsControllerHolder_;

		PyObj										pyCreateDynamicsController_;
		PyObj										pyDynamicsController_;
		PyObj										pyFrameBegin_;
		PyObj										pyPreIntegration_;
		PyObj										pyPostIntegration_;
	};
}

#endif // _SIMULATABLESCRIPT_HPP_
