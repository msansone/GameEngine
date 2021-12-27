#include "..\..\Headers\EngineCore\QueryParametersFactory.hpp"

using namespace firemelon;

QueryParametersFactory::QueryParametersFactory()
{
}

QueryParametersFactory::~QueryParametersFactory()
{
}

QueryParameters* QueryParametersFactory::createQueryParametersBase(QueryId queryType)
{
	QueryParameters* newQueryParameters = nullptr;

	newQueryParameters = createQueryParameters(queryType);	

	if (newQueryParameters == nullptr)
	{
		newQueryParameters = new QueryParameters();
	}

	return newQueryParameters;
}

QueryParameters* QueryParametersFactory::createQueryParameters(QueryId queryType)
{
	return nullptr;
}