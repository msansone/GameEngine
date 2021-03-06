/* -------------------------------------------------------------------------
** QueryResult.hpp
**
** The QueryResult class is the parent class that the query result
** classes generated by the editor inherit from. The QueryContainer will
** create and delete it.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _QUERYRESULT_HPP_
#define _QUERYRESULT_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

namespace firemelon
{
	class FIREMELONAPI QueryResult
	{
	public:

		QueryResult();
		virtual ~QueryResult();
	
	private:

	};
}

#endif // _QUERYRESULT_HPP_