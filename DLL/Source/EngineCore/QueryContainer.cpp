#include "..\..\Headers\EngineCore\QueryContainer.hpp"

using namespace firemelon;
using namespace boost::python;

QueryContainer::QueryContainer(QueryId queryType)
{
	queryType_ = queryType;
}

QueryContainer::~QueryContainer()
{
	// Signal the entity manager that this query is no longer needed.
	


	//if (result_ != nullptr)
	//{
	//	delete result_;
	//	result_ = nullptr;
	//}

	//if (parameters_ != nullptr)
	//{
	//	delete parameters_;
	//	parameters_ = nullptr;
	//}
}

object QueryContainer::getResultPy()
{
	PythonReleaseGil unlocker;

	return getResult();
}

object QueryContainer::getResult()
{
	return result_;
}

object QueryContainer::getParametersPy()
{
	PythonReleaseGil unlocker;

	return getParameters();
}

object QueryContainer::getParameters()
{
	return parameters_;
}

QueryId QueryContainer::getQueryType()
{
	return queryType_;
}