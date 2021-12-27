/* -------------------------------------------------------------------------
** QueryManager.hpp
** 
** The QueryManager class acts as a layer between the Entity class and the
** GameStateManager class, which is used to run queries from an entity and return
** the results to it.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _QUERYMANAGER_HPP_
#define _QUERYMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/signals2.hpp>

#include "QueryContainer.hpp"

namespace firemelon
{
	typedef boost::signals2::signal<void (firemelon::QueryContainer &queryContainer)> querysignal;
	typedef boost::signals2::signal<QueryContainer (QueryId queryId)> getquerysignal;

	class FIREMELONAPI QueryManager 
	{
	public:
		friend class GameStateManager;

		QueryManager();
		virtual ~QueryManager();
		
		QueryContainer getQueryPy(QueryId queryId);
		QueryContainer getQuery(QueryId queryId);
		
		void runQueryPy(QueryContainer query);
		void runQuery(QueryContainer query);

		void closeQueryPy(QueryContainer query);
		void closeQuery(QueryContainer query);

	private:
		
		querysignal postQuerySignal;
		getquerysignal getQuerySignal;	
		querysignal closeQuerySignal;	
	};
}
#endif // _QUERYMANAGER_HPP_