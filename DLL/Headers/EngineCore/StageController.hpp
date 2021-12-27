/* -------------------------------------------------------------------------
** StageController.hpp
**
** The StageController class is the interface which the state machine and 
** the end user use to access the stage.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STAGECONTROLLER_HPP_
#define _STAGECONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "AnchorPointManager.hpp"
#include "AnimationManager.hpp"
#include "HitboxControllerHolder.hpp"
#include "Renderer.hpp"
#include "RenderEffects.hpp"
#include "Stage.hpp"
#include "StageRenderable.hpp"

namespace firemelon
{
	class FIREMELONAPI StageController
	{
	public:
		friend class EntityComponents;
		friend class Room;
		friend class StateMachineController;
		
		StageController();

		virtual ~StageController();

		void				addAnimationSlotToStageElementsByIndexPy(int          stageElementsIndex, std::string    slotName,       int          x,            int                 y,
																	 ColorRgbaPtr hueColor,           ColorRgbaPtr   blendColor,     float        blendPercent, float               rotation,
																	 int          framesPerSecond,    int            pivotX,         int          pivotY,       AnimationSlotOrigin origin,
																	 std::string  nextStateName,      AnimationStyle animationStyle, ColorRgbaPtr outlineColor, bool                background);


		void				addAnimationSlotToStageElementsByIndex(int          stageElementsIndex, std::string    slotName,       int          x,            int                 y,
																   ColorRgbaPtr hueColor,           ColorRgbaPtr   blendColor,     float        blendPercent, float               rotation,
																   int          framesPerSecond,    int            pivotX,         int          pivotY,       AnimationSlotOrigin origin,
																   std::string  nextStateName,      AnimationStyle animationStyle, ColorRgbaPtr outlineColor, bool                background);


		void				addAnimationSlotToStageElementsByNamePy(std::string stageElementsName, std::string    slotName,       int          x,            int                 y,
																    ColorRgbaPtr hueColor,         ColorRgbaPtr   blendColor,     float        blendPercent, float               rotation,
																    int          framesPerSecond,  int            pivotX,         int          pivotY,       AnimationSlotOrigin origin,
																    std::string  nextStateName,    AnimationStyle animationStyle, ColorRgbaPtr outlineColor, bool                background);


		void				addAnimationSlotToStageElementsByName(std::string stageElementsName, std::string    slotName,       int          x,            int                 y,
																  ColorRgbaPtr hueColor,         ColorRgbaPtr   blendColor,     float        blendPercent, float               rotation,
																  int          framesPerSecond,  int            pivotX,         int          pivotY,       AnimationSlotOrigin origin,
																  std::string  nextStateName,    AnimationStyle animationStyle, ColorRgbaPtr outlineColor, bool                background);

		// Animation assignments
		
		// Stage Elements Name, Slot Name, Animation Name
		void				assignAnimationByNameToSlotByNamePy(std::string stageElementsName, std::string slotName, std::string animationName);
		void				assignAnimationByNameToSlotByName(std::string stageElementsName, std::string slotName, std::string animationName);

		void				assignAnimationByNameToSlotByNamePy(std::string stageElementsName, std::string slotName, std::string animationName, std::string synchWithSlotName);
		void				assignAnimationByNameToSlotByName(std::string stageElementsName, std::string slotName, std::string animationName, std::string synchWithSlotName);

		// Stage Elements Name, Slot Name, Animation Index
		void				assignAnimationByIdToSlotByNamePy(std::string stageElementsName, std::string slotName, int animationId);
		void				assignAnimationByIdToSlotByName(std::string stageElementsName, std::string slotName, int animationId);

		void				assignAnimationByIdToSlotByNamePy(std::string stageElementsName, std::string slotName, int animationId, std::string synchWithSlotName);
		void				assignAnimationByIdToSlotByName(std::string stageElementsName, std::string slotName, int animationId, std::string synchWithSlotName);

		// Stage Elements Name, Slot Index, Animation Name
		void				assignAnimationByNameToSlotByIndexPy(std::string stageElementsName, int slotIndex, std::string animationName);
		void				assignAnimationByNameToSlotByIndex(std::string stageElementsName, int slotIndex, std::string animationName);

		void				assignAnimationByNameToSlotByIndexPy(std::string stageElementsName, int slotIndex, std::string animationName, std::string synchWithSlotName);
		void				assignAnimationByNameToSlotByIndex(std::string stageElementsName, int slotIndex, std::string animationName, std::string synchWithSlotName);

		// Stage Elements Name, Slot Index, Animation Index
		void				assignAnimationByIdToSlotByIndexPy(std::string stageElementsName, int slotIndex, int animationId);
		void				assignAnimationByIdToSlotByIndex(std::string stageElementsName, int slotIndex, int animationId);

		void				assignAnimationByIdToSlotByIndexPy(std::string stageElementsName, int slotIndex, int animationId, std::string synchWithSlotName);
		void				assignAnimationByIdToSlotByIndex(std::string stageElementsName, int slotIndex, int animationId, std::string synchWithSlotName);
		


		// Stage Elements Index, Slot Name, Animation Name
		void				assignAnimationByNameToSlotByNamePy(int stageElementsIndex, std::string slotName, std::string animationName);
		void				assignAnimationByNameToSlotByName(int stageElementsIndex, std::string slotName, std::string animationName);

		void				assignAnimationByNameToSlotByNamePy(int stageElementsIndex, std::string slotName, std::string animationName, std::string synchWithSlotName);
		void				assignAnimationByNameToSlotByName(int stageElementsIndex, std::string slotName, std::string animationName, std::string synchWithSlotName);

		// Stage Elements Index, Slot Name, Animation Index
		void				assignAnimationByIdToSlotByNamePy(int stageElementsIndex, std::string slotName, int animationId);
		void				assignAnimationByIdToSlotByName(int stageElementsIndex, std::string slotName, int animationId);

		void				assignAnimationByIdToSlotByNamePy(int stageElementsIndex, std::string slotName, int animationId, std::string synchWithSlotName);
		void				assignAnimationByIdToSlotByName(int stageElementsIndex, std::string slotName, int animationId, std::string synchWithSlotName);
		

		// Stage Elements Index, Slot Index, Animation Name
		void				assignAnimationByNameToSlotByIndexPy(int stageElementsIndex, int slotIndex, std::string animationName);
		void				assignAnimationByNameToSlotByIndex(int stageElementsIndex, int slotIndex, std::string animationName);

		void				assignAnimationByNameToSlotByIndexPy(int stageElementsIndex, int slotIndex, std::string animationName, std::string synchWithSlotName);
		void				assignAnimationByNameToSlotByIndex(int stageElementsIndex, int slotIndex, std::string animationName, std::string synchWithSlotName);

		// Stage Elements Index, Slot Index, Animation Index - Add python wrappers when needed.
		void				assignAnimationByIdToSlotByIndexPy(int stageElementsIndex, int slotIndex, int animationId);
		void				assignAnimationByIdToSlotByIndex(int stageElementsIndex, int slotIndex, int animationId);

		void				assignAnimationByIdToSlotByIndexPy(int stageElementsIndex, int slotIndex, int animationId, std::string synchWithSlotName);
		void				assignAnimationByIdToSlotByIndex(int stageElementsIndex, int slotIndex, int animationId, std::string synchWithSlotName);


		// Add a hitbox to a state.
		void				addHitboxReferencePy(int id);
		void				addHitboxReference(int id);

		StageElementsPtr	getCurrentStageElementsPy();
		StageElementsPtr	getCurrentStageElements();

		int					getCurrentStageElementsIndexPy();
		int					getCurrentStageElementsIndex();

		float				getExtentBottomPy();
		float				getExtentBottom();

		float				getExtentLeftPy();
		float				getExtentLeft();

		float				getExtentTopPy();
		float				getExtentTop();

		float				getExtentRightPy();
		float				getExtentRight();

		ColorRgbaPtr		getHueColorPy();
		ColorRgbaPtr		getHueColor();

		bool				getMirrorHorizontallyPy();
		bool				getMirrorHorizontally();

		ColorRgbaPtr		getOutlineColorPy();
		ColorRgbaPtr		getOutlineColor();

		// Pivot Point
		PositionPtr			getPivotPointPy();

		PositionPtr			getPivotPoint();

		// Rotation Angle
		float				getRotationAnglePy();

		float				getRotationAngle();

		StagePtr			getStage();
		
		StageElementsPtr	getStageElementsPy(int index);
		StageElementsPtr	getStageElements(int index);

		StageElementsPtr	getStageElementsByNamePy(std::string name);
		
		StageElementsPtr	getStageElementsByName(std::string name);
		
		int					getStageElementsIndexFromNamePy(std::string name);
		
		int					getStageElementsIndexFromName(std::string name);

		StageRenderablePtr	getStageBackgroundRenderable();

		StageRenderablePtr	getStageForegroundRenderable();

		void				setExtentBottomPy(float extentBottom);
		void				setExtentBottom(float extentBottom);

		void				setExtentLeftPy(float extentLeft);
		void				setExtentLeft(float extentLeft);

		void				setExtentTopPy(float extentTop);
		void				setExtentTop(float extentTop);

		void				setExtentRightPy(float extentRight);
		void				setExtentRight(float extentRight);

		void				setMirrorHorizontallyPy(bool mirrorHorizontally);
		void				setMirrorHorizontally(bool mirrorHorizontally);

		// Rotation Angle

		void				setRotationAnglePy(float rotationAngle);

		void				setRotationAngle(float rotationAngle);

		//  Summary: Synch the animation slots between two different states.
		//
		// Use case: One way I have used this is when the player is in the middle of an aerial attack animation, and then hit the ground and switch to the on ground attack
		//           animation. You wouldn't want the on ground attack animtion to restart, you'd want it to start at the same point that the aerial attack animation was on,
		//           so that it appears to be the same single attack motion.
		void				synchAnimationSlots(std::string synchFromStateName, std::string synchFromSlotName, std::string synchToStateName, std::string synchToSlotName);
		void				synchAnimationSlotsPy(std::string synchFromStateName, std::string synchFromSlotName, std::string synchToStateName, std::string synchToSlotName);
		
		unsigned int		getHeightPy();
		unsigned int		getHeight();

		bool				getInterpolateExtentsPy();
		bool				getInterpolateExtents();

		bool				getInterpolateRotationPy();
		bool				getInterpolateRotation();

		bool				getIsVisiblePy();
		bool				getIsVisible();

		int					getRenderOrderPy();
		int					getRenderOrder();

		unsigned int		getWidthPy();
		unsigned int		getWidth();

		void				setInterpolateExtentsPy(bool interpolateExtents);
		void				setInterpolateExtents(bool interpolateExtents);

		void				setInterpolateRotationPy(bool interpolateRotation);
		void				setInterpolateRotation(bool interpolateRotation);

		void				setIsVisiblePy(bool isVisible);
		void				setIsVisible(bool isVisible);

#pragma endregion

	private:

		bool	activateStageElements(int index);

		int		addExistingStageElements(StageElementsPtr newStageElements);
				
		void	setRenderOrder(int renderOrder);
		
		AnimationManagerPtr			animationManager_;

		HitboxControllerHolderPtr	hitboxControllerHolder_;

		HitboxManagerPtr			hitboxManager_;
		
		RendererPtr					renderer_;

		StagePtr					stage_;

		StageRenderablePtr			stageBackgroundRenderable_;

		StageRenderablePtr			stageForegroundRenderable_;

		StateContainerPtr			stateContainer_;	
	};

	typedef boost::shared_ptr<StageController> StageControllerPtr;
}

#endif // _STAGECONTROLLER_HPP_