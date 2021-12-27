/* -------------------------------------------------------------------------
** Query.hpp
**
** The Query class is the parent class from which the child query classes
** that are generated by the editor derive from. The user must implement the
** runQuery function.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _QUERY_HPP_
#define _QUERY_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>

#include "Entity.hpp"
#include "RoomManager.hpp"
#include "RoomContainer.hpp"
#include "QueryContainer.hpp"

namespace firemelon
{
	class FIREMELONAPI Query
	{
	public:
		friend class GameStateManager;

		Query();
		virtual ~Query();
		
		void					initialize();

		virtual void			runQuery(QueryContainer queryContainer);
		QueryResult				getQueryResult();
		QueryParameters			getQueryParameters();

	protected:

		boost::shared_ptr<RoomManager>	getRoomManager();

	private:

		static int				idCounter_;
		int						id_;

		QueryId					queryType_;

		std::string				scriptName_;
		std::string				scriptTypeName_;
		
		DebuggerPtr							debugger_;
		boost::shared_ptr<RoomManager>		roomManager_;
		boost::shared_ptr<RoomContainer>	roomContainer_;

		// Scripting data
		boost::python::object	pyMainModule_;
		boost::python::object	pyMainNamespace_;
		boost::python::object	pyQueryInstance_;
		boost::python::object	pyQueryInstanceNamespace_;
		
		boost::python::object	pyRun_;

		boost::python::object	pyParameters_;
		boost::python::object	pyResult_;
	};
}

#endif // _QUERY_HPP_