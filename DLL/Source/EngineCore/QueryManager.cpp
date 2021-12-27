#include "..\..\Headers\EngineCore\QueryManager.hpp"

using namespace firemelon;

QueryManager::QueryManager()
{
}

QueryManager::~QueryManager()
{
}

void QueryManager::runQueryPy(QueryContainer query)
{
	PythonReleaseGil unlocker;

	runQuery(query);
}

void QueryManager::runQuery(QueryContainer query)
{
	postQuerySignal(query);
}

QueryContainer QueryManager::getQueryPy(QueryId queryId)
{
	PythonReleaseGil unlocker;

	return getQuery(queryId);
}

QueryContainer QueryManager::getQuery(QueryId queryId)
{
	boost::optional<QueryContainer> oi = getQuerySignal(queryId);
	
	QueryContainer qc = oi.get();

	return qc;
}

void QueryManager::closeQueryPy(QueryContainer query)
{
	PythonReleaseGil unlocker;

	closeQuery(query);
}

void QueryManager::closeQuery(QueryContainer query)
{
	closeQuerySignal(query);
}
