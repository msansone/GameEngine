/* -------------------------------------------------------------------------
** RoomLoader.hpp
**
** The RoomLoader class is the parent class of the room loader class that is
** generated for the project by the editor. The user impements the roomLoading
** and roomUnloading functions, so that data can be transferred between rooms,
** such as the players hitpoints or lives remaining.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ROOMLOADER_HPP_
#define _ROOMLOADER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "AudioPlayer.hpp"
#include "BaseIds.hpp"
#include "Message.hpp"
#include "Messenger.hpp"
#include "QueryManager.hpp"
#include "QueryResult.hpp"
#include "QueryResultFactory.hpp"
#include "QueryParametersFactory.hpp"
#include "QueryContainer.hpp"
#include "TextManager.hpp"
#include "Types.hpp"
#include "Ui.hpp"

namespace firemelon
{
	typedef boost::signals2::signal<void (firemelon::Message message)> messagesignal;

	class FIREMELONAPI RoomLoader
	{
	public:
		friend class GameEngine;
		friend class RoomManager;
		friend class GameStateManager;

		RoomLoader();
		virtual ~RoomLoader();
	
		virtual void	roomLoaded(RoomId roomId, stringmap roomParameters);	
		virtual void	roomUnloading(RoomId roomId, stringmap roomParameters);
		virtual void	userInitialize(){};

		void			postMessage(Message message);

	protected:
	
		boost::shared_ptr<AudioPlayer>				getAudioPlayer();
		boost::shared_ptr<Messenger>				getMessenger();
		boost::shared_ptr<QueryManager>				getQueryManager();
		boost::shared_ptr<QueryParametersFactory>	getQueryParametersFactory();
		boost::shared_ptr<QueryResultFactory>		getQueryResultFactory();
		boost::shared_ptr<TextManager>				getTextManager();

	private:
	
		void	initialize();

		boost::shared_ptr<AudioPlayer>				audioPlayer_;
		DebuggerPtr									debugger_;
		boost::shared_ptr<Messenger>				messenger_;	
		boost::shared_ptr<QueryParametersFactory>	queryParametersFactory_;
		boost::shared_ptr<QueryResultFactory>		queryResultFactory_;
		boost::shared_ptr<QueryManager>				queryManager_;
		boost::shared_ptr<TextManager>				textManager_;

		querysignal				postQuerySignal;
		messagesignal			postMessageSignal;
		
		std::string				scriptName_;
		std::string				scriptTypeName_;
		
		// Scripting data
		boost::python::object			pyMainModule_;
		boost::python::object			pyMainNamespace_;
		boost::python::object			pyRoomLoaderInstance_;
		boost::python::object			pyRoomLoaderNamespace_;
		
		boost::python::object			pyRoomLoaded_;
		boost::python::object			pyRoomUnloading_;
	};
}

#endif // _ROOMLOADER_HPP_