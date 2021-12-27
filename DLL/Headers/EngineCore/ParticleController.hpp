/* -------------------------------------------------------------------------
** ParticleController.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PARTICLECONTROLLER_HPP_
#define _PARTICLECONTROLLER_HPP_

#include <boost/python.hpp>
#include <boost/signals2.hpp>

#include "EntityController.hpp"
#include "ParticleMetadata.hpp"
#include "ParticleRenderable.hpp"

namespace firemelon
{
	class ParticleController : public EntityController
	{
	public:
		friend class ParticleEmitterEntityCodeBehind;
		friend class ParticleEntityCodeBehind;
		friend class Room;

		ParticleController();
		virtual ~ParticleController();

		void			deactivate();
		void			deactivatePy();

		ColorRgbaPtr	getHueColor();
		ColorRgbaPtr	getHueColorPy();

		ParticleOrigin	getOrigin();
		ParticleOrigin	getOriginPy();

		RenderEffectsPtr getRenderEffects();
		RenderEffectsPtr getRenderEffectsPy();

		float			getRotationAnglePy();
		float			getRotationAngle();

		void			setRotationAngle(float rotationAngle);
		void			setRotationAnglePy(float rotationAngle);

		void			setAnimation(std::string animationName, int framesPerSecond);
		void			setAnimationPy(std::string animationName, int framesPerSecond);

		void			setOrigin(ParticleOrigin origin);
		void			setOriginPy(ParticleOrigin origin);

	protected:


	private:

		virtual void								cleanup();
		virtual boost::shared_ptr<EntityMetadata>	createMetadata();
		virtual void								createRenderables();
		
		bool										deactivate_;

		boost::shared_ptr<ParticleMetadata>			particleMetadata_;

		boost::shared_ptr<ParticleRenderable>		particleRenderable_;

		boost::shared_ptr<Renderable>				renderable_;

	};
}

#endif // _PARTICLECONTROLLER_HPP_