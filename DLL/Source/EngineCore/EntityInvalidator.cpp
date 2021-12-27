#include "..\..\Headers\EngineCore\EntityInvalidator.hpp"

using namespace firemelon;

EntityInvalidator::EntityInvalidator() 
{
	isInvalidated_ = false;
}

EntityInvalidator::~EntityInvalidator()
{
}

void EntityInvalidator::setIsInvalidated(bool isInvalid)
{
	isInvalidated_ = isInvalid;
}

bool EntityInvalidator::getIsInvalidated()
{
	return isInvalidated_;
}