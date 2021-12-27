/* -------------------------------------------------------------------------
** QueryContainer.hpp
**
** The QueryContainer class is the object that the user creates which encapsulates
** a query. It gets passed the query type to create, and if required, it creates
** the parameters and result object for that query type. It should be created on
** the stack rather than the heap, so that when it goes out of scope its destructor
** is called, it will delete the result and parameters objects if they exist. This
** is so the user doesn't have to call new and delete themself.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _QUERYCONTAINER_HPP_
#define _QUERYCONTAINER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>

#include "BaseIds.hpp"
#include "QueryResult.hpp"
#include "QueryResultFactory.hpp"
#include "QueryParametersFactory.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI QueryContainer
	{
	public:
		friend class GameStateManager;
		friend class Query;	
		
		QueryContainer(QueryId queryType);
		virtual ~QueryContainer();
	
		boost::python::object	getResultPy();
		boost::python::object	getResult();
		
		boost::python::object	getParametersPy();
		boost::python::object	getParameters();
	
		QueryId					getQueryType();

	private:

		int						id_;
		QueryId					queryType_;
		boost::python::object	result_;
		boost::python::object	parameters_;
	};
}

#endif // _QUERYCONTAINER_HPP_