#include "..\..\Headers\EngineCore\QueryFactory.hpp"

using namespace firemelon;

QueryFactory::QueryFactory()
{
}

QueryFactory::~QueryFactory()
{
}

boost::shared_ptr<Query> QueryFactory::createQueryBase(QueryId queryType)
{
	boost::shared_ptr<Query> newQuery = nullptr;

	newQuery = createQuery(queryType);	

	if (newQuery == nullptr)
	{
		newQuery = boost::shared_ptr<Query>(new Query());
	}

	return newQuery;
}

boost::shared_ptr<Query> QueryFactory::createQuery(QueryId queryType)
{
	return nullptr;
}