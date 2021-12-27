/* -------------------------------------------------------------------------
** ParticleRenderable.hpp
**
** The ParticleRenderable class is the renderable component for a particle. 
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PARTICLERENDERABLE_HPP_
#define _PARTICLERENDERABLE_HPP_

#include <boost/signals2.hpp>

#include "Animation.hpp"
#include "AnimationFrame.hpp"
#include "AnimationManager.hpp"
#include "AnimationPlayer.hpp"
#include "ColorRgba.hpp"
#include "LinearAlgebraUtility.hpp"
#include "ParticleMetadata.hpp"
#include "Renderable.hpp"

namespace firemelon
{
	class ParticleRenderable : public Renderable
	{
	public:
		friend class ParticleEntityCodeBehind;

		ParticleRenderable();
		virtual ~ParticleRenderable();

		ColorRgbaPtr	getHueColor();

		unsigned int	getHeight();

		unsigned int	getWidth();

		void			reset();

		void			setAnimation(boost::shared_ptr<Animation> animation, int framesPerSecond);

		void			setMetadata(ParticleMetadataPtr metadata);

	private:

		void			applyRotationTransforms(double lerp);
		void			applyTransforms(double lerp);
		bool			getIsDynamic();
		void			initializeRenderable();
		void			updateRenderable(double time);
		void			render(double lerp);
		int 			getX();
		int 			getY();

		void			setCornerPoints();

		AnimationPtr			animation_;
		AnimationManagerPtr		animationManager_;
		AnimationPlayerPtr		animationPlayer_;
		AnimationFramePtr		currentFrame_;
		int						frameSourceX_;
		int						frameSourceY_;
		int						height_;
		PositionPtr				layerPosition_;
		LinearAlgebraUtilityPtr	linearAlgebraUtility_;
		ParticleMetadataPtr		metadata_;
		std::vector<Vertex2>	nativeCorners_;
		PositionPtr				position_;
		int						previousFrameIndex_;
		RendererPtr				renderer_;
		int						spriteSheetIndex_;
		std::vector<Vertex2>	transformedCorners_;
		int						width_;
	};
}

#endif // _PARTICLERENDERABLE_HPP_