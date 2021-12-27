/* -------------------------------------------------------------------------
** Stage.hpp
**
** The Stage class stores the stage properties, as well as all of the stage 
** elements associated with a state machine state. These include animation 
** slots, hitboxes, and anchor points.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STAGE_HPP_
#define _STAGE_HPP_

#include "StageElements.hpp"
#include "StateContainer.hpp"

namespace firemelon
{
	class Stage
	{
	public:
		friend class Assets;
		friend class EntityComponents;
		friend class EntityTemplate;
		friend class Room;
		friend class StageController;
		friend class StageElements;
		friend class StageRenderable;
		friend class StateMachineController;

		Stage();
		virtual ~Stage();

		StageElementsPtr	getStageElementsByIndex(int index);

		// Stage Elements
		StageElementsPtr	getActiveStageElements();

		// Metadata
		StageMetadataPtr	getMetadata();


		// Mirror Horizontal

		bool				getMirrorHorizontallyPy();

		bool				getMirrorHorizontally();

		// Origin
		StageOrigin			getOrigin();

		// Stage Pivot Point

		PositionPtr			getPivotPointPy();

		PositionPtr			getPivotPoint();

		// Rotation Angle

		float				getRotationAnglePy();

		float				getRotationAngle();


		// Mirror Horizontal

		void				setMirrorHorizontallyPy(bool mirrorHorizontally);

		void				setMirrorHorizontally(bool mirrorHorizontally);

		// Rotation Angle

		void				setRotationAnglePy(float rotationAngle);

		void				setRotationAngle(float rotationAngle);

	private:

		// addAnimationSlotInternal assumes x and y coordinates are already in the native TLC origin space.
		void		addAnimationSlotInternal(std::string slotName, int x, int y, ColorRgbaPtr hueColor, ColorRgbaPtr blendColor, float blendPercent, float rotation, double updateInterval, int pivotX, int pivotY, float alphaGradientFrom, float alphaGradientTo, int alphaGradientRadialCenterX, int alphaGradientRadialCenterY, float alphaGradientRadius, AlphaGradientDirection alphaGradientDirection, AnimationSlotOrigin origin);
		
		int			addStageElements(StageElementsPtr stageElements);

		void		applyTransforms(double lerp);

		int			getAnimationSlotIndex(std::string slotName);
		
		void		setHeight(int height);
		
		void		setOrigin(StageOrigin origin);

		void		setWidth(int width);
		
		AnimationSlotPtrList		activeAnimationSlots_;
				
		AnimationManagerPtr			animationManager_;

		StageElementsPtr			currentStageElements_;

		bool						mirrorHorizontally_;

		RendererPtr					renderer_;

		bool						reapplyTransform_;
				
		// The statuses of the hitboxes pointed to by the animation in each slot.
		// This will change as the animation frames change.
		std::vector<std::vector<std::vector<bool>>>	slotHitboxCollisionStatuses_;

		StageElementsPtrList		elements_;

		StageMetadataPtr			metadata_;
				
		EntityMetadataPtr			ownerMetadata_;

	};

	typedef boost::shared_ptr<Stage> StagePtr;
}

#endif // _STAGE_HPP_