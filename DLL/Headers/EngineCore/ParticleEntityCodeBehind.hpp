/* -------------------------------------------------------------------------
** ParticleEntityCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PARTICLEENTITYCODEBEHIND_HPP_
#define _PARTICLEENTITYCODEBEHIND_HPP_

#include "EntityCodeBehind.hpp"
#include "ParticleController.hpp"
#include "ParticleMetadata.hpp"
#include "ParticleScript.hpp"

#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp> 

namespace firemelon
{
	class ParticleEntityCodeBehind : public EntityCodeBehind
	{
	public:
		friend class CodeBehindFactory;
		friend class ParticleEmitterEntityCodeBehind;
		friend class Room;

		ParticleEntityCodeBehind();
		virtual ~ParticleEntityCodeBehind();

	protected:

	private:

		void			activate();
		virtual void	baseCleanup();
		virtual void	baseInitialize();
		bool			baseUpdate(double time);
		void			deactivate();
		void			preDeactivated();
		virtual void	deactivated();
		void			preEmitted();
		virtual void	emitted();


		bool									isActive_;
		boost::shared_ptr<ParticleController>	particleController_;
		boost::shared_ptr<ParticleMetadata>		particleMetadata_;
		boost::shared_ptr<ParticleRenderable>	particleRenderable_;
		boost::shared_ptr<ParticleScript>		particleScript_;
		double									updateTimer_;
	};
}

#endif // _PARTICLEENTITYCODEBEHIND_HPP_
