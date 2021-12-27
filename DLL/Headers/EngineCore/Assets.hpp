/* -------------------------------------------------------------------------
** Assets.hpp
**
** The Assets class stores all of the game assets.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ASSETS_HPP_
#define _ASSETS_HPP_

#include <boost/filesystem.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp> 
#include <boost/uuid/uuid_io.hpp>

#include "BaseIds.hpp"
#include "Debugger.hpp"
#include "EntityMetadataContainer.hpp"
#include "LoadingScreenContainer.hpp"
#include "RenderableManager.hpp"
#include "TransitionContainer.hpp"
#include "TextManager.hpp"
#include "Ui.hpp"

namespace firemelon
{
	class Assets
	{
	public:

		Assets (
			boost::shared_ptr<RenderableManager> renderableManager,
			boost::shared_ptr<HitboxManager> hitboxManager,
			AnimationManagerPtr animationManager,
			boost::shared_ptr<Ui> ui,
			boost::shared_ptr<InputDeviceManager> inputDeviceManager,
			boost::shared_ptr<Renderer> renderer,
			boost::shared_ptr<AudioPlayer> audioPlayer,
			boost::shared_ptr<EntityMetadataContainer> entityMetadataContainer,
			boost::shared_ptr<AnchorPointManager> anchorPointManager,
			boost::shared_ptr<LoadingScreenContainer> loadingScreenContainer,
			boost::shared_ptr<TransitionContainer> transitionContainer,
			boost::shared_ptr<TextManager> textManager,
			boost::shared_ptr<FontManager> fontManager,
			boost::shared_ptr<BaseIds> ids,
			boost::shared_ptr<Debugger> debugger);

		virtual ~Assets();
		
		void	load();

		boost::shared_ptr<EntityTemplate>					getEntityTemplate(EntityTypeId entityId);
		RoomId												getInitialRoomId();
		int													getTileSize();
		int													getTileWidthPerRenderGridCell();
		int													getTileWidthPerCollisionGridCell();
		int													getCameraWidth();
		int													getCameraHeight();

	private:
		
		int													tileSize_;
		int													cameraHeight_;
		int													cameraWidth_;

		// The number of tiles (NxN) that fit in one cell of the render grid.
		int													tileWidthPerRenderGridCell_;

		// The number of tiles (NxN) that fit in one cell of the collision grid.
		int													tileWidthPerCollisionGridCell_;

		RoomId												initialRoomId_;
		
		std::vector<boost::shared_ptr<EntityTemplate>>		entityTemplates_;

		// Maps the entitytype to an index in the entityTemplate list.
		std::map<AssetId, int>								entityTemplateIdMap_;

		//std::map<CollisionMaskType, int>					hitboxCollisionMaskIdMap_;

		boost::shared_ptr<AnchorPointManager>				anchorPointManager_;
		AnimationManagerPtr									animationManager_;
		boost::shared_ptr<AudioPlayer>						audioPlayer_;
		DebuggerPtr											debugger_;
		boost::shared_ptr<EntityMetadataContainer>			entityMetadataContainer_;
		boost::shared_ptr<FontManager>						fontManager_;
		boost::shared_ptr<HitboxManager>					hitboxManager_;
		boost::shared_ptr<BaseIds>							ids_;
		boost::shared_ptr<InputDeviceManager>				inputDeviceManager_;
		boost::shared_ptr<LoadingScreenContainer>			loadingScreenContainer_;
		boost::shared_ptr<RenderableManager>				renderableManager_;
		boost::shared_ptr<Renderer>							renderer_;
		boost::shared_ptr<TextManager>						textManager_;
		boost::shared_ptr<TransitionContainer>				transitionContainer_;
		boost::shared_ptr<Ui>								ui_;
	};
}

#endif // _ASSETS_HPP_