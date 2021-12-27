/* -------------------------------------------------------------------------
** AnimationSlot.hpp
**
** The AnimationSlot class represents a location of an animation on a stage.
** It can store any animation, as well as properties such as position,
** hue color, and rotation, just to name a few. It has points that represent
** the four corners in different representations. This includes:
**
**   - Animation relative (top left, corner, top right corner, bottom right corner,
**     and bottom left corner)
**
**     - These points are useful if you apply a mirroring transform, you may 
**       still want to know the corners, which will not be the same when mirrored.
**       (for example, when mirroring the top left corner across the x axis,
**       that same point (if you imagine it being physically moved) would 
**       actually become the top right corner on the screen after transformation.
**       So it is useful to have named referneces to the corners that will 
**       always match the expectations of the user.
**       It may get a little confusion when a rotation is applied, because it's
**       no longer aligned with the axes. Maybe it should also be set by 
**       assuming a rotation between 0 and 89 degrees?
**       Or maybe this isn't even necessary. Could be opening a can of worms.
**
**  - Ordinal points (1, 2, 3, 4)
**
**    - These points are simply named values that will always be associated
**      with the same point no matter how they are transformed.
** 
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ANIMATIONSLOT_HPP_
#define _ANIMATIONSLOT_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <vector>

#include "AnimationManager.hpp"
#include "AnimationPlayer.hpp"
#include "Position.hpp"
#include "PythonGil.hpp"
#include "Renderer.hpp"
#include "Size.hpp"
#include "StageMetadata.hpp"

namespace firemelon
{
	class FIREMELONAPI AnimationSlot
	{
	public:
		friend class Stage;
		friend class StageController;
		friend class StageElements;
		friend class StageRenderable;

		AnimationSlot(std::string name, int x, int y);
		virtual ~AnimationSlot();

		int					getAnimationIdPy();
		int					getAnimationId();

		AnimationPlayerPtr	getAnimationPlayerPy();
		AnimationPlayerPtr	getAnimationPlayer();

		AnimationStyle		getAnimationStylePy();
		AnimationStyle		getAnimationStyle();

		std::string			getNamePy();
		std::string			getName();

		PositionPtr			getNativePositionPy();
		PositionPtr			getNativePosition();

		RenderEffectsPtr	getRenderEffectsPy();
		RenderEffectsPtr	getRenderEffects();
		
		double				getFramesPerSecondPy();
		double				getFramesPerSecond();
		
	private:

		float				getBottommostPoint();

		float				getLeftmostPoint();

		int					getNextAvailableAnchorPointId();

		int					getNextAvailableHitboxId();

		float				getRightmostPoint();

		float				getTopmostPoint();

		void				setCornerPoints(float scaleFactor);

		int								activeAnchorPointCount_;

		int								activeHitboxCount_;

		// The indexes of the anchor points pointed to by each frame of the animation in the slot.
		std::vector<std::vector<int>>	anchorPointReferences_;

		AnimationPtr					animation_;

		int								animationId_;

		AnimationManagerPtr				animationManager_;

		AnimationPlayerPtr				animationPlayer_;

		// An animation slot contains a list of anchor point IDs that it has created and can use for
		// animation frame anchorpoints when an animation gets assigned to the slot.
		std::vector<int>				availableAnchorPointIds_;

		// An animation slot contains a list of hitbox IDs that it has created and can use for
		// animation frame hitboxes when an animation gets assigned to the slot.
		std::vector<int>				availableHitboxIds_;

		// The indexes of the hitboxes pointed to by each frame of the animation in the slot.
		std::vector<std::vector<int>>	hitboxReferences_;

		bool							isBackground_;

		std::string						name_;

		PositionPtr						nativePosition_;

		std::vector<Vertex2>			nativeCorners_;

		std::string						nextStateName_;

		int								oldAnimationId_;

		AnimationSlotOrigin				origin_;

		RenderEffectsPtr				renderEffects_;

		RendererPtr						renderer_;

		// The size is dependent on the animation currently assigned to the slot.
		SizePtr							size_;

		//// The edgeflags of the hitboxes pointed to by the animation in each slot.
		//// This will change as the animation frames change.
		//std::vector<std::vector<std::vector<unsigned char>>>	hitboxEdgeFlags_;

		//// The statuses of the hitboxes pointed to by the animation in each slot.
		//// This will change as the animation frames change.
		//std::vector<std::vector<std::vector<bool>>>				hitboxCollisionStatuses_;


		std::vector<Vertex2>			transformedCorners_;		
	};

	typedef boost::shared_ptr<AnimationSlot> AnimationSlotPtr;
	typedef std::vector<AnimationSlotPtr> AnimationSlotPtrList;
}

#endif // _ANIMATIONSLOT_HPP_