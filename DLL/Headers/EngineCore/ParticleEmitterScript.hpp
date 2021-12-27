/* -------------------------------------------------------------------------
** ParticleEmitterScript.hpp
**
** The ParticleEmitterScript contains the python script functions for the
** particle emitter methods.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PARTICLEEMITTERSCRIPT_HPP_
#define _PARTICLEEMITTERSCRIPT_HPP_

#include "BaseIds.hpp"
#include "CodeBehindScript.hpp"
#include "Debugger.hpp"
#include "EntityMetadata.hpp"
#include "ParticleEmitterController.hpp"
#include "Position.hpp"
#include "PythonInstanceWrapper.hpp"
#include "Types.hpp"

namespace firemelon
{
	class ParticleEmitterScript : public CodeBehindScript
	{
	public:
		friend class ParticleEmitterEntityCodeBehind;
		friend class Room;

		ParticleEmitterScript(DebuggerPtr debugger);
		virtual ~ParticleEmitterScript();
		
	protected:

	private:

		void	cleanup();
		void	initialize();

		void	particleEmitted(boost::shared_ptr<ParticleEntityCodeBehind> p);

		boost::shared_ptr<ParticleEmitterController>	controller_;
		
		PyObj											pyParticleEmitted_;
	};
}

#endif // _PARTICLEEMITTERSCRIPT_HPP_
