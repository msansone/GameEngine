/* -------------------------------------------------------------------------
** GameStateManager.hpp
**
** The GameStateManager class, together with the RoomManager class, handles
** the initializing and updating of the entities. Unlike the RoomManager 
** class, it is not available to the user, and is used only for internal engine
** processing. In addition to this, it also contains functions for dispatching
** collision events, messages, and queries.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _GAMESTATEMANAGER_HPP_
#define _GAMESTATEMANAGER_HPP_

#include "QueryFactory.hpp"
#include "Factory.hpp"
#include "Messenger.hpp"
#include "PythonGil.hpp"

#include <queue>

namespace firemelon
{
	class GameStateManager
	{
	public:
		friend class GameEngine;

		GameStateManager(boost::shared_ptr<Ui> ui, boost::shared_ptr<Renderer> renderer,
			boost::shared_ptr<AudioPlayer> audioPlayer, boost::shared_ptr<RoomManager> roomManager, boost::shared_ptr<InputDeviceManager> inputDeviceManager,
			boost::shared_ptr<PhysicsManager> physicsManager, boost::shared_ptr<QueryFactory> queryFactory, boost::shared_ptr<QueryResultFactory> queryResultFactory,
			boost::shared_ptr<QueryParametersFactory> queryParametersFactory, boost::shared_ptr<RenderableManager> renderableManager, boost::shared_ptr<TextManager> textManager,
			boost::shared_ptr<Messenger> messender, boost::shared_ptr<QueryManager> queryManager, AnimationManagerPtr animationManager, boost::shared_ptr<HitboxManager> hitboxManager,
			boost::shared_ptr<AnchorPointManager> anchorPointManager, boost::shared_ptr<EntityMetadataContainer> entityMetadataContainer, boost::shared_ptr<GameTimer> timer,
			boost::shared_ptr<RoomContainer> roomContainer, boost::shared_ptr<FontManager> fontManager, boost::shared_ptr<CollisionDispatcher> collisionDispatcher,
			boost::shared_ptr<EngineController> engineController, DebuggerPtr debugger);

		virtual ~GameStateManager();
		
		void					initialize();
		void					loadRoomByName(std::string roomName, stringmap roomParameters);
		void					loadRoom(RoomId roomId);
		void					showRoom(RoomId roomId);
		void					loadAssets();
		
		void					update(double time);
		void					postUpdate();

		void					render(int roomIndex, double lerp);
		
		void					setFullscreen(bool value);
		void					setRoomLoader(boost::shared_ptr<RoomLoader> roomLoader);
		
		bool					getIsRoomLoaded();

	private:
	
		void			postMessageEvent(Message message);

		void			postQuery(QueryContainer &queryContainer);
		QueryContainer	getQuery(QueryId queryId);
		void			closeQuery(QueryContainer &queryContainer);

		void			dispatchAllMessages();
		void			dispatchSingleMessage(Message message);

		bool									isInitialized_;
		
		double									totalTime_;

		int										timerId_;

		boost::shared_ptr<AnchorPointManager>		anchorPointManager_;
		AnimationManagerPtr							animationManager_;
		boost::shared_ptr<AudioPlayer>				audioPlayer_;
		boost::shared_ptr<CollisionDispatcher>		collisionDispatcher_;
		DebuggerPtr									debugger_;
		boost::shared_ptr<HitboxManager>			hitboxManager_;
		boost::shared_ptr<EngineController>			engineController_;
		boost::shared_ptr<EntityMetadataContainer>	entityMetadataContainer_;
		boost::shared_ptr<FontManager>				fontManager_;
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		boost::shared_ptr<Messenger>				messenger_;
		boost::shared_ptr<PhysicsManager>			physicsManager_;
		boost::shared_ptr<QueryFactory>				queryFactory_;
		boost::shared_ptr<QueryManager>				queryManager_;
		boost::shared_ptr<QueryParametersFactory>	queryParametersFactory_;
		boost::shared_ptr<QueryResultFactory>		queryResultFactory_;
		boost::shared_ptr<Renderer>					renderer_;
		boost::shared_ptr<RoomManager>				roomManager_;
		boost::shared_ptr<RoomContainer>			roomContainer_;
		boost::shared_ptr<RenderableManager>		renderableManager_;
		boost::shared_ptr<TextManager>				textManager_;
		boost::shared_ptr<GameTimer>				timer_;
		boost::shared_ptr<Ui>						ui_;

		std::vector<Message>						messageQueue_;
		std::map<QueryId, boost::shared_ptr<Query>>	activeQueries_;
	};
}

#endif // _GAMESTATEMANAGER_HPP_