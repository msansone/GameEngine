/* -------------------------------------------------------------------------
** StageRenderable.hpp
**
** The StageRenderable class implements the renderable functions for an
** entity's animations.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STAGERENDERABLE_HPP_
#define _STAGERENDERABLE_HPP_

#include <string>
#include <vector>
#include <map>

#include <boost/signals2.hpp>
#include <boost/shared_ptr.hpp>

#include "Position.hpp"
#include "AnimationPlayer.hpp"
#include "AnimationManager.hpp"
#include "EntityMetadata.hpp"
#include "HitboxController.hpp"
#include "HitboxManager.hpp"
#include "Renderable.hpp"
#include "Renderer.hpp"
#include "Stage.hpp"
#include "StageElements.hpp"

namespace firemelon
{
	class StageRenderable : public Renderable
	{
	public:
		friend class ActorController;
		friend class CameraController;
		friend class HudElementController;
		friend class Entity;
		friend class EntityComponents;
		friend class Room;
		friend class StageController;
		friend class StageMetadata;

		StageRenderable();
		virtual ~StageRenderable();

		bool			getIsDynamic();

		int				getXPy();
		int				getX();

		int				getYPy();
		int				getY();

		unsigned int	getHeightPy();
		unsigned int	getHeight();

		unsigned int	getWidthPy();
		unsigned int	getWidth();

		void			render(double lerp);

		void			renderDebugData(double lerp);

		void			updateRenderable(double time);

	private:

		void	initializeRenderable();

		//void	refreshCachedData();
		
		ColorRgbaPtr			anchorPointColor_;

		AnchorPointManagerPtr	anchorPointManager_;

		AnimationManagerPtr		animationManager_;

		HitboxControllerPtr		hitboxController_;
		
		bool					isBackground_;

		// Renderables attached to the camera shouldn't calculate a layer LERP value.
		bool					layerLerp_;

		PositionPtr				layerPosition_;

		PositionPtr				position_;

		// The combined render effects of the stage and slot.
		RenderEffectsPtr		renderEffects_;

		RendererPtr				renderer_;

		ColorRgbaPtr			slotOutlineColor_;

		StagePtr				stage_;

		ColorRgbaPtr			stageOutlineColor_;

		StateContainerPtr		stateContainer_;


#pragma region Signals

		boost::signals2::signal<bool(std::string stateName)>			setStateByNameSignal;

		boost::signals2::signal<void(TriggerSignalId triggerSignalId)>	frameTriggerSignal;
		
		boost::signals2::signal<void(int stateIndex)>					stateEndedSignal;

#pragma endregion

	};

	typedef boost::shared_ptr<StageRenderable> StageRenderablePtr;
}

#endif // _STAGERENDERABLE_HPP_