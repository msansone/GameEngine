/* -------------------------------------------------------------------------
** SimulatableCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SIMULATABLECODEBEHIND_HPP_
#define _SIMULATABLECODEBEHIND_HPP_

#include "CodeBehind.hpp"
#include "DynamicsControllerHolder.hpp"
#include "SimulatableScript.hpp"
#include "Types.hpp"

namespace firemelon
{
	class SimulatableCodeBehind : public CodeBehind
	{
	public:
		friend class CodeBehindContainer;
		friend class GameStateManager;

		SimulatableCodeBehind();
		virtual ~SimulatableCodeBehind();

		boost::shared_ptr<SimulatableScript>	getScript();

		void									preCleanup();
		void									preInitialize();
		void									start();
	
	protected:

		virtual void	cleanup();
		virtual void	frameBegin();
		virtual void	initialize();
		virtual void	preIntegration();
		virtual void	postIntegration();

	private:

		boost::shared_ptr<DynamicsControllerHolder>	dynamicsControllerHolder_;
		boost::shared_ptr<SimulatableScript>		simulatableScript_;
	};
}

#endif // _SIMULATABLECODEBEHIND_HPP_