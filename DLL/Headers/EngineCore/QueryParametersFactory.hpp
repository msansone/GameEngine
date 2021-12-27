/* -------------------------------------------------------------------------
** QueryParametersFactory.hpp
**
** The QueryParametersFactory class is the parent class of the query parameters
** factory class that is generated by the editor. It is used internally by the 
** query container to create the user defined query parameters objects.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */


#ifndef _QUERYPARAMETERSFACTORY_HPP_
#define _QUERYPARAMETERSFACTORY_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "QueryParameters.hpp"
#include "BaseIds.hpp"

namespace firemelon
{
	class FIREMELONAPI QueryParametersFactory
	{
	public:
		QueryParametersFactory();
		virtual ~QueryParametersFactory();

		virtual QueryParameters* createQueryParameters(QueryId queryType);
		
		QueryParameters* createQueryParametersBase(QueryId queryType);
	private:

	};
}

#endif // _QUERYPARAMETERSFACTORY_HPP_