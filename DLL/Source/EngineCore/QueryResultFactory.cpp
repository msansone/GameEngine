#include "..\..\Headers\EngineCore\QueryResultFactory.hpp"

using namespace firemelon;

QueryResultFactory::QueryResultFactory()
{
}

QueryResultFactory::~QueryResultFactory()
{
}


QueryResult* QueryResultFactory::createQueryResultBase(QueryId queryType)
{
	QueryResult* newQueryResult = nullptr;

	newQueryResult = createQueryResult(queryType);	

	if (newQueryResult == nullptr)
	{
		newQueryResult = new QueryResult();
	}

	return newQueryResult;
}

QueryResult* QueryResultFactory::createQueryResult(QueryId queryType)
{
	return nullptr;
}