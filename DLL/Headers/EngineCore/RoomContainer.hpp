/* -------------------------------------------------------------------------
** RoomContainer.hpp
** 
** The RoomContainer class stores the rooms.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ROOMCONTAINER_HPP_
#define _ROOMCONTAINER_HPP_

#include <vector>
#include <queue>
#include <set>

#include "Room.hpp"

namespace firemelon
{
	typedef boost::shared_ptr<boost::signals2::signal<void(RoomId roomId, TransitionId transitionId, double transitionTime)>> ShowRoomSignal;
	typedef boost::signals2::signal<void(RoomId roomId, TransitionId transitionId, double transitionTime)> ShowRoomSignalRaw;

	class RoomContainer
	{
	public:
		friend class GameEngine;
		friend class RoomManager;

		RoomContainer();
		virtual ~RoomContainer();

		void					addRoom(boost::shared_ptr<Room> room);
		
		bool					getIsRoomLoadingOrUnloading();

		int						getRoomCount();
		boost::shared_ptr<Room>	getRoomPy(RoomId roomId);
		boost::shared_ptr<Room>	getRoom(RoomId roomId);
		boost::shared_ptr<Room>	getRoomByIndex(int roomIndex);
		int						getRoomIndex(RoomId roomId);
		
		boost::shared_ptr<Room>	getShownRoomPy();
		boost::shared_ptr<Room>	getShownRoom();
		RoomId					getShownRoomId();
		
		void					moveAllEntities();
		void					moveAllEntities(RoomId showingRoomId);
		void					moveEntity(MoveEntityParameters params);

		void					preloadRooms();
		
		void					setAssets(boost::shared_ptr<Assets> assets);

		void					setShownRoomId(RoomId roomId);

	private:

		struct EntityMoveQueueItem
		{
			int entityInstanceId;
			int fromRoomId;
			int toRoomId;
			SpawnPointId spawnPointId;
			int offsetX;
			int offsetY;
			std::string key;			
			TransitionId transitionId;
			ShowRoomParameters showRoomParameters;
			bool delayUntilRoomShown;
		};

		void				buttonDown(boost::shared_ptr<InputEvent> inputEvent);
		void				buttonUp(boost::shared_ptr<InputEvent> inputEvent);
		void				cleanup();
		void				initialize();
		void				linkCameraToParticipant(int participantIndex, boost::shared_ptr<CameraController> camera);
		void				linkRoomToParticipant(int participantIndex, RoomId room);

		std::set<std::string>						entityMoveSet_;
		std::vector<EntityMoveQueueItem>			entityMoveQueue_;
		std::vector<boost::shared_ptr<Room>>		rooms_;
		
		int											shownRoomId_;
		int											previousShownRoomId_;
		
		std::map<firemelon::RoomId, int>			roomIdToIndexMap_;

		boost::shared_ptr<AnchorPointManager>		anchorPointManager_;
		AnimationManagerPtr							animationManager_;
		boost::shared_ptr<Assets>					assets_;
		boost::shared_ptr<AudioPlayer>				audioPlayer_;
		boost::shared_ptr<AudioSourceContainer>		audioSourceContainer_;
		boost::shared_ptr<NameValidator>			audioSourceNameValidator_;
		boost::shared_ptr<CameraManager>			cameraManager_;
		boost::shared_ptr<CodeBehindFactory>		codeBehindFactory_;
		boost::shared_ptr<Debugger>					debugger_;
		boost::shared_ptr<EngineConfig>				engineConfig_;
		boost::shared_ptr<EngineController>			engineController_;
		boost::shared_ptr<EntityMetadataContainer>	entityMetadataContainer_;
		boost::shared_ptr<NameValidator>			entityNameValidator_;
		boost::shared_ptr<HitboxManager>			hitboxManager_;
		boost::shared_ptr<FontManager>				fontManager_;
		boost::shared_ptr<BaseIds>					ids_;
		IoService									ioService_;
		boost::shared_ptr<InputDeviceManager>		inputDeviceManager_;
		boost::shared_ptr<Messenger>				messenger_;
		boost::shared_ptr<NameValidator>			particleEmitterNameValidator_;
		PhysicsConfigPtr							physicsConfig_;
		boost::shared_ptr<PhysicsManager>			physicsManager_;
		boost::shared_ptr<QueryManager>				queryManager_;
		boost::shared_ptr<QueryParametersFactory>	queryParametersFactory_;
		boost::shared_ptr<QueryResultFactory>		queryResultFactory_;
		boost::shared_ptr<RenderableManager>		renderableManager_;
		boost::shared_ptr<Renderer>					renderer_;
		boost::shared_ptr<RoomMetadataContainer>	roomMetadataContainer_;
		boost::shared_ptr<TextManager>				textManager_;
		boost::shared_ptr<GameTimer>				timer_;
		boost::shared_ptr<Ui>						ui_;

		// Signals
		ShowRoomSignal 								showRoomSignal_;
	};
}

#endif // _ROOMCONTAINER_HPP_