/* -------------------------------------------------------------------------
** ParticleScript.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PARTICLESCRIPT_HPP_
#define _PARTICLESCRIPT_HPP_

#include "BaseIds.hpp"
#include "CodeBehindScript.hpp"
#include "Debugger.hpp"
#include "ParticleMetadata.hpp"
#include "ParticleController.hpp"
#include "Position.hpp"
#include "PythonInstanceWrapper.hpp"
#include "Types.hpp"

namespace firemelon
{
	class ParticleScript : public CodeBehindScript
	{
	public:
		friend class ParticleEntityCodeBehind;
		friend class Room;

		ParticleScript(DebuggerPtr debugger);
		virtual ~ParticleScript();

	protected:

	private:

		void	cleanup();
		void	initialize();

		void	deactivated();
		void	emitted();
		void	rendered(int x, int y);

		boost::shared_ptr<ParticleController>	controller_;
		boost::shared_ptr<ParticleMetadata>		particleMetadata_;
		PyObj									pyDeactivated_;
		PyObj									pyEmitted_;
	};
}

#endif // _PARTICLESCRIPT_HPP_
