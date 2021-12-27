/* -------------------------------------------------------------------------
** StageElements.hpp
**
** The StageElements class stores a group of animation slots, hitboxes, and 
** anchor points that are associated with a particular state.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STAGEELEMENTS_HPP_
#define _STAGEELEMENTS_HPP_

#include <iostream>
#include <vector>

#include <boost/shared_ptr.hpp>

#include "AnimationSlot.hpp"
#include "Debugger.hpp"
#include "EntityMetadata.hpp"
#include "HitboxManager.hpp"
#include "LinearAlgebraUtility.hpp"
#include "RenderEffects.hpp"
#include "StageMetadata.hpp"

namespace firemelon
{
	class StageElements
	{
	public:
		friend class Assets;
		friend class EntityTemplate;
		friend class Room;
		friend class Stage;
		friend class StageController;
		friend class StageRenderable;
		friend class StateMachineController;

		StageElements(std::string associatedStateName);
		virtual ~StageElements();

		int						addAnchorPoint(std::string name, boost::shared_ptr<Position> anchorPoint);

		
		// Add stage hitboxes
		void					addHitboxReferencePy(int id);
		void					addHitboxReference(int id);

		// Add slot hitboxes

		void					addSlotHitboxReferenceByNamePy(std::string slotName, int hitboxId);
		void					addSlotHitboxReferenceByName(std::string slotName, int hitboxId);

		void					addSlotHitboxReferenceByIndexPy(int slotIndex, int hitboxId);
		void					addSlotHitboxReferenceByIndex(int slotIndex, int hitboxId);

		// Anchor Points
		AnchorPointPtr			getAnchorPointByNameFromSlotByNamePy(std::string slotName, std::string anchorPointName);
		AnchorPointPtr			getAnchorPointByNameFromSlotByName(std::string slotName, std::string anchorPointName);

		AnchorPointPtr			getAnchorPointByNameFromSlotByIndex(int slotIndex, std::string anchorPointName);

		AnchorPointPtr			getAnchorPointByIndexFromSlotByName(std::string slotName, int anchorPointIndex);

		AnchorPointPtr			getAnchorPointByIndexFromSlotByIndex(int slotIndex, int anchorPointIndex);


		// Animation ID

		int						getAnimationIdByNamePy(std::string slotName);
		int						getAnimationIdByName(std::string slotName);

		int						getAnimationIdByIndexPy(int slotIndex);
		int						getAnimationIdByIndex(int slotIndex);

		// Animation Player

		AnimationPlayerPtr		getAnimationPlayerByName(std::string slotName);
		AnimationPlayerPtr		getAnimationPlayerByNamePy(std::string slotName);

		AnimationPlayerPtr		getAnimationPlayerByIndex(int slotIndex);
		AnimationPlayerPtr		getAnimationPlayerByIndexPy(int slotIndex);

		// Animation Slot
		AnimationSlotPtr		getAnimationSlotByNamePy(std::string slotName);
		AnimationSlotPtr		getAnimationSlotByName(std::string slotName);

		AnimationSlotPtr		getAnimationSlotByIndexPy(int slotIndex);
		AnimationSlotPtr		getAnimationSlotByIndex(int slotIndex);

		// Count
		int						getAnimationSlotCountPy();
		int						getAnimationSlotCount();

		// Index

		int						getAnimationSlotIndexPy(std::string slotName);
		int						getAnimationSlotIndex(std::string slotName);

		// Name

		std::string				getAnimationSlotNamePy(int slotIndex);
		std::string				getAnimationSlotName(int slotIndex);

		// Next State Name
		std::string				getAnimationSlotNextStateNameByIndexPy(int slotIndex);
		std::string				getAnimationSlotNextStateNameByIndex(int slotIndex);
		
		// Pivot Point

		int						getAnimationSlotPivotPointXByNamePy(std::string slotName);
		int						getAnimationSlotPivotPointXByName(std::string slotName);

		int						getAnimationSlotPivotPointXByIndex(int slotIndex);

		int						getAnimationSlotPivotPointYByNamePy(std::string slotName);
		int						getAnimationSlotPivotPointYByName(std::string slotName);

		int						getAnimationSlotPivotPointYByIndex(int slotIndex);

		// Position

		int						getAnimationSlotPositionXByNamePy(std::string slotName);
		int						getAnimationSlotPositionXByName(std::string slotName);

		int						getAnimationSlotPositionXByIndexPy(int slotIndex);
		int						getAnimationSlotPositionXByIndex(int slotIndex);

		int						getAnimationSlotPositionYByNamePy(std::string slotName);
		int						getAnimationSlotPositionYByName(std::string slotName);

		int						getAnimationSlotPositionYByIndexPy(int slotIndex);
		int						getAnimationSlotPositionYByIndex(int slotIndex);

		// Hue Color

		ColorRgbaPtr			getAnimationSlotHueColorByNamePy(std::string slotName);
		ColorRgbaPtr			getAnimationSlotHueColorByName(std::string slotName);

		ColorRgbaPtr			getAnimationSlotHueColorByIndexPy(int slotIndex);
		ColorRgbaPtr			getAnimationSlotHueColorByIndex(int slotIndex);

		// Outline Color

		ColorRgbaPtr			getAnimationSlotOutlineColorByNamePy(std::string slotName);
		ColorRgbaPtr			getAnimationSlotOutlineColorByName(std::string slotName);

		ColorRgbaPtr			getAnimationSlotOutlineColorByIndexPy(int slotIndex);
		ColorRgbaPtr			getAnimationSlotOutlineColorByIndex(int slotIndex);

		// Blend Color

		ColorRgbaPtr			getAnimationSlotBlendColorByNamePy(std::string slotName);
		ColorRgbaPtr			getAnimationSlotBlendColorByName(std::string slotName);

		ColorRgbaPtr			getAnimationSlotBlendColorByIndexPy(int slotIndex);
		ColorRgbaPtr			getAnimationSlotBlendColorByIndex(int slotIndex);

		// Blend Percent

		float					getAnimationSlotBlendPercentByNamePy(std::string slotName);
		float					getAnimationSlotBlendPercentByName(std::string slotName);

		float					getAnimationSlotBlendPercentByIndexPy(int slotIndex);
		float					getAnimationSlotBlendPercentByIndex(int slotIndex);

		// Alpha Gradient Direction

		AlphaGradientDirection	getAnimationSlotAlphaGradientDirectionByNamePy(std::string slotName);
		AlphaGradientDirection	getAnimationSlotAlphaGradientDirectionByName(std::string slotName);
		AlphaGradientDirection	getAnimationSlotAlphaGradientDirectionByIndex(int slotIndex);

		// Alpha Gradient From

		float					getAnimationSlotAlphaGradientFromByNamePy(std::string slotName);
		float					getAnimationSlotAlphaGradientFromByName(std::string slotName);
		float					getAnimationSlotAlphaGradientFromByIndex(int slotIndex);

		// Alpha Gradient To

		float					getAnimationSlotAlphaGradientToByNamePy(std::string slotName);
		float					getAnimationSlotAlphaGradientToByName(std::string slotName);
		float					getAnimationSlotAlphaGradientToByIndex(int slotIndex);

		// Alpha Gradient Radius Center Point X

		int						getAnimationSlotAlphaGradientRadialCenterXByNamePy(std::string slotName);
		int						getAnimationSlotAlphaGradientRadialCenterXByName(std::string slotName);
		int						getAnimationSlotAlphaGradientRadialCenterXByIndex(int slotIndex);

		// Alpha Gradient Radius Center Point Y

		int						getAnimationSlotAlphaGradientRadialCenterYByNamePy(std::string slotName);
		int						getAnimationSlotAlphaGradientRadialCenterYByName(std::string slotName);
		int						getAnimationSlotAlphaGradientRadialCenterYByIndex(int slotIndex);

		// Alpha Gradient Radius

		float					getAnimationSlotAlphaGradientRadiusByNamePy(std::string slotName);
		float					getAnimationSlotAlphaGradientRadiusByName(std::string slotName);
		float					getAnimationSlotAlphaGradientRadiusByIndex(int slotIndex);
		
		// Frames Per Second

		int						getAnimationSlotFramesPerSecondByNamePy(std::string slotName);
		int						getAnimationSlotFramesPerSecondByName(std::string slotName);

		int						getAnimationSlotFramesPerSecondByIndex(int slotIndex);

		// Animation Style

		AnimationStyle			getAnimationSlotAnimationStyleByNamePy(std::string slotName);
		AnimationStyle			getAnimationSlotAnimationStyleByName(std::string slotName);
		AnimationStyle			getAnimationSlotAnimationStyleByIndex(int slotIndex);

		// Associated State Name

		std::string				getAssociatedStateNamePy();
		std::string				getAssociatedStateName();

		// Render effects

		RenderEffectsPtr		getAnimationSlotRenderEffectsByName(std::string slotName);
		RenderEffectsPtr		getAnimationSlotRenderEffectsByIndex(int slotIndex);

		// Rotation

		float					getAnimationSlotRotationByNamePy(std::string slotName);
		float					getAnimationSlotRotationByName(std::string slotName);
		float					getAnimationSlotRotationByIndex(int slotIndex);

		// Slot Origin

		AnimationSlotOrigin		getAnimationSlotOriginByIndex(int slotIndex);
		
		// Hitbox Edge Flags

		unsigned char			getHitboxEdgeFlags(int index);

		// Stage Hitbox References

		int						getHitboxReferenceCountPy();
		int						getHitboxReferenceCount();

		int						getHitboxReferencePy(int index);
		int						getHitboxReference(int index);

		// Hue Color

		ColorRgbaPtr			getHueColorPy();
		ColorRgbaPtr			getHueColor();

		// Stage Pivot Point

		PositionPtr				getPivotPointPy();

		PositionPtr				getPivotPoint();

		// RotationAngle

		float					getRotationAnglePy();

		float					getRotationAngle();

		//// Slot Hitbox References

		//int						getSlotHitboxReferenceCountByName(std::string slotName);
		//int						getSlotHitboxReferenceCountByIndex(int slotIndex);

		//int						getSlotHitboxReferenceByNamePy(std::string slotName, int frameIndex, int hitboxIndex);
		//int						getSlotHitboxReferenceByName(std::string slotName, int frameIndex, int hitboxIndex);

		//int						getSlotHitboxReferenceByIndexPy(int slotIndex, int frameIndex, int hitboxIndex);
		//int						getSlotHitboxReferenceByIndex(int slotIndex, int frameIndex, int hitboxIndex);

		//// Slot Hitbox Collision Status

		//bool					getSlotHitboxCollisionStatusByName(std::string slotName, int frameIndex, int hitboxIndex);
		//bool					getSlotHitboxCollisionStatusByIndex(int slotIndex, int frameIndex, int hitboxIndex);

		//// Slot Hitbox Edge Flags

		//unsigned char			getSlotHitboxEdgeFlagsByName(std::string slotName, int frameIndex, int hitboxIndex);
		//unsigned char			getSlotHitboxEdgeFlagsByIndex(int slotIndex, int frameIndex, int hitboxIndex);

		// Removals

		void					removeAnimationSlotByNamePy(std::string slotName);
		void					removeAnimationSlotByName(std::string slotName);

		void					removeAnimationSlotByIndex(int slotIndex);


		void					removeHitboxReferenceByIndexPy(int index);
		void					removeHitboxReferenceByIndex(int index);


		// TODO - The stage should really only be interacted with via the controller.
		// These setter functions should be made private and have wrappers added in the controller, and the python versions should be removed.
		// Also, the alpha functions were removed in favor of alpha textures.

		// Alpha Gradient Direction

		void					setAnimationSlotAlphaGradientDirectionByNamePy(std::string slotName, AlphaGradientDirection alphaGradientDirection);
		void					setAnimationSlotAlphaGradientDirectionByName(std::string slotName, AlphaGradientDirection alphaGradientDirection);
		void					setAnimationSlotAlphaGradientDirectionByIndex(int slotIndex, AlphaGradientDirection alphaGradientDirection);

		// Alpha Gradient From

		void					setAnimationSlotAlphaGradientFromByNamePy(std::string slotName, float alphaGradientFrom);
		void					setAnimationSlotAlphaGradientFromByName(std::string slotName, float alphaGradientFrom);
		void					setAnimationSlotAlphaGradientFromByIndex(int slotIndex, float alphaGradientFrom);

		// Alpha Gradient Radius

		void					setAnimationSlotAlphaGradientRadiusByNamePy(std::string slotName, float radius);
		void					setAnimationSlotAlphaGradientRadiusByName(std::string slotName, float radius);
		void					setAnimationSlotAlphaGradientRadiusByIndex(int slotIndex, float radius);

		// Alpha Gradient To

		void					setAnimationSlotAlphaGradientToByNamePy(std::string slotName, float alphaGradientTo);
		void					setAnimationSlotAlphaGradientToByName(std::string slotName, float alphaGradientTo);
		void					setAnimationSlotAlphaGradientToByIndex(int slotIndex, float alphaGradientTo);

		// Animation Style

		void					setAnimationSlotAnimationStyleByNamePy(std::string slotName, AnimationStyle animationStyle);
		void					setAnimationSlotAnimationStyleByName(std::string slotName, AnimationStyle animationStyle);
		void					setAnimationSlotAnimationStyleByIndex(int slotIndex, AnimationStyle animationStyle);
				
		// Blend Color

		void					setAnimationSlotBlendColorByNamePy(std::string slotName, float r, float g, float b, float a);
		void					setAnimationSlotBlendColorByName(std::string slotName, float r, float g, float b, float a);

		void					setAnimationSlotBlendColorByIndexPy(int slotIndex, float r, float g, float b, float a);
		void					setAnimationSlotBlendColorByIndex(int slotIndex, float r, float g, float b, float a);

		// Blend Percent

		void					setAnimationSlotBlendPercentByNamePy(std::string slotName, float percent);
		void					setAnimationSlotBlendPercentByName(std::string slotName, float percent);

		void					setAnimationSlotBlendPercentByIndexPy(int slotIndex, float percent);
		void					setAnimationSlotBlendPercentByIndex(int slotIndex, float percent);

		// Extent Left

		void					setAnimationSlotExtentLeftByNamePy(std::string slotName, float extentLeft);
		void					setAnimationSlotExtentLeftByName(std::string slotName, float extentLeft);
		void					setAnimationSlotExtentLeftByIndex(int slotIndex, float extentLeft);

		// Extent Top

		void					setAnimationSlotExtentTopByNamePy(std::string slotName, float extentTop);
		void					setAnimationSlotExtentTopByName(std::string slotName, float extentTop);
		void					setAnimationSlotExtentTopByIndex(int slotIndex, float extentTop);

		// Extent Right

		void					setAnimationSlotExtentRightByNamePy(std::string slotName, float extentRight);
		void					setAnimationSlotExtentRightByName(std::string slotName, float extentRight);
		void					setAnimationSlotExtentRightByIndex(int slotIndex, float extentRight);

		// Extent Bottom

		void					setAnimationSlotExtentBottomByNamePy(std::string slotName, float extentBottom);
		void					setAnimationSlotExtentBottomByName(std::string slotName, float extentBottom);
		void					setAnimationSlotExtentBottomByIndex(int slotIndex, float extentBottom);

		// Frames Per Second

		void					setAnimationSlotFramesPerSecondByNamePy(std::string slotName, int framesPerSecond);
		void					setAnimationSlotFramesPerSecondByName(std::string slotName, int framesPerSecond);

		void					setAnimationSlotFramesPerSecondByIndex(int slotIndex, int framesPerSecond);

		// Outline Color

		void					setAnimationSlotOutlineColorByNamePy(std::string slotName, float r, float g, float b, float a);
		void					setAnimationSlotOutlineColorByName(std::string slotName, float r, float g, float b, float a);

		void					setAnimationSlotOutlineColorByIndexPy(int slotIndex, float r, float g, float b, float a);
		void					setAnimationSlotOutlineColorByIndex(int slotIndex, float r, float g, float b, float a);

		// Hue Color

		void					setAnimationSlotHueColorByNamePy(std::string slotName, float r, float g, float b, float a);
		void					setAnimationSlotHueColorByName(std::string slotName, float r, float g, float b, float a);

		void					setAnimationSlotHueColorByIndexPy(int slotIndex, float r, float g, float b, float a);
		void					setAnimationSlotHueColorByIndex(int slotIndex, float r, float g, float b, float a);

		// Position

		void					setAnimationSlotPositionXByNamePy(std::string slotName, int x);
		void					setAnimationSlotPositionXByName(std::string slotName, int x);

		void					setAnimationSlotPositionXByIndexPy(int slotIndex, int x);
		void					setAnimationSlotPositionXByIndex(int slotIndex, int x);

		void					setAnimationSlotPositionYByNamePy(std::string slotName, int y);
		void					setAnimationSlotPositionYByName(std::string slotName, int y);

		void					setAnimationSlotPositionYByIndexPy(int slotIndex, int y);
		void					setAnimationSlotPositionYByIndex(int slotIndex, int y);

		void					setAnimationSlotPositionByNamePy(std::string slotName, int x, int y);
		void					setAnimationSlotPositionByName(std::string slotName, int x, int y);

		void					setAnimationSlotPositionByIndex(int slotIndex, int x, int y);

		// Rotation

		void					setAnimationSlotRotationByNamePy(std::string slotName, float rotation);
		void					setAnimationSlotRotationByName(std::string slotName, float rotation);
		void					setAnimationSlotRotationByIndex(int slotIndex, float rotation);

		//// Hitbox Collision Status

		//void					setHitboxCollisionStatus(int index, bool status);

		//// Hitbox Edge Flags

		//void					setHitboxEdgeFlags(int index, unsigned char edgeFlags);

		// Rotation Angle

		void					setRotationAnglePy(float rotationAngle);

		void					setRotationAngle(float rotationAngle);

		//// Slot Hitbox Collision Status

		//void					setSlotHitboxCollisionStatusByName(std::string slotName, int frameIndex, int hitboxIndex, bool status);
		//void					setSlotHitboxCollisionStatusByIndex(int slotIndex, int frameIndex, int hitboxIndex, bool status);

		//// Slot Hitbox Edge Flags

		//void					setSlotHitboxEdgeFlagsByName(std::string slotName, int frameIndex, int hitboxIndex, unsigned char edgeFlags);
		//void					setSlotHitboxEdgeFlagsByIndex(int slotIndex, int frameIndex, int hitboxIndex, unsigned char edgeFlags);

	private:
		
		// addAnimationSlotInternal assumes x and y coordinates are already in the native TLC origin space.
		//void	addAnimationSlotInternal(std::string            slotName,                   int                 x,                          int         y, 
		//	                             ColorRgbaPtr           hueColor,                   ColorRgbaPtr        blendColor,                 float       blendPercent, 
		//	                             float                  rotation,                   int                 framesPerSecond,            int         pivotX,
		//	                             int                    pivotY,                     float               alphaGradientFrom,          float       alphaGradientTo, 
		//	                             int                    alphaGradientRadialCenterX, int                 alphaGradientRadialCenterY, float       alphaGradientRadius, 
		//	                             AlphaGradientDirection alphaGradientDirection,     AnimationSlotOrigin origin,                     std::string nextStateName,

		//	                             AnimationStyle         animationStyle);

		AnimationSlotPtr	addAnimationSlotInternal(std::string            slotName,                   int                 x,                          int         y,
													 ColorRgbaPtr           hueColor,                   ColorRgbaPtr        blendColor,                 float       blendPercent, 
													 float                  rotation,                   int                 framesPerSecond,            int         pivotX,
													 int                    pivotY,                     AnimationSlotOrigin origin,                     std::string nextStateName,
													 AnimationStyle         animationStyle,             ColorRgbaPtr        outlineColor,               bool        background);

		void				applyMirrorHorizontallyTransform();

		void				applyRotationTransforms(double lerp);

		void				applyTransforms(bool mirrorHorizontally, double lerp);

		void				assignAnimationByIdToSlotByIndexInternal(int slotIndex, int animationId, bool copyHitboxes);

		void				assignAnimationByIdToSlotByIndexInternal(int slotIndex, int animationId, bool copyHitboxes, std::string synchWithSlotName);

		void				assignAnimationByIdToSlotByNameInternal(std::string slotName, int animationId, bool copyHitboxes);

		void				assignAnimationByIdToSlotByNameInternal(std::string slotName, int animationId, bool copyHitboxes, std::string synchWithSlotName);

		bool				getHitboxCollisionStatus(int index);

		bool				getAnimationSlotBackgroundByIndex(int slotIndex);

		int					getNativeAnimationSlotPositionXByIndex(int slotIndex);

		int					getNativeAnimationSlotPositionYByIndex(int slotIndex);

		bool				getSingleFrame();

		float				interpolateAngle(float angle, float previousAngle, float previousPreviousAngle, double lerp);

		//void				setSingleFramePy(bool singleFrame);
		void				setRenderableBoundary();

		void				setSingleFrame(bool singleFrame);

		AnchorPointManagerPtr									anchorPointManager_;

		AnimationManagerPtr										animationManager_;

		AnimationSlotPtrList									animationSlots_;

		std::string												associatedStateName_;

		DebuggerPtr												debugger_;

		// The farthest left animation.
		int														leftBoundary_;

		// The farthest right animation, plus the width of the animation it holds.
		int														rightBoundary_;

		// The farthest up animation.
		int														topBoundary_;

		// The farthest down animation, plus the height of the animation it holds.
		int														bottomBoundary_;

		// The indexes of the hitboxes pointed to by this state.
		std::vector<int>										hitboxReferences_;

		//// The edgeflags of the hitboxes pointed to by this state.
		//std::vector<unsigned char>								hitboxEdgeFlags_;

		//// The statuses of the hitboxes pointed to by this state.
		//std::vector<bool>										hitboxCollisionStatuses_;

		boost::shared_ptr<HitboxManager>						hitboxManager_;

		LinearAlgebraUtilityPtr									linearAlgebraUtility_;

		EntityMetadataPtr										ownerMetadata_;

		boost::shared_ptr<Renderer>								renderer_;

		bool													reapplyTransform_;

		// Set to 0, 0. Used as a return value when getting an anchor point that doesn't exist.
		AnchorPointPtr											returnedAnchorPoint_;

		// Should be set if all of the animations in this state only have one frame.
		// This is used for optimization purposes, because single frame animations don't need to be 
		// updated, unless their style is set to SINGLE_END_STATE which is a special case.
		bool													singleFrame_;

		StageMetadataPtr										stageMetadata_;

		// The render effects for the stage.
		RenderEffectsPtr										stageRenderEffects_;
	};

	typedef boost::shared_ptr<StageElements> StageElementsPtr;
	typedef std::vector<StageElementsPtr> StageElementsPtrList;
}

#endif // _STAGEELEMENTS_HPP_