#include "..\..\Headers\EngineCore\NameIdPair.hpp"

using namespace firemelon;

NameIdPair::NameIdPair(int id, std::string name)
{
	id_ = id;
	name_ = name;
}

NameIdPair::~NameIdPair()
{
}

int NameIdPair::getId()
{
	return id_;
}

std::string	NameIdPair::getName()
{
	return name_;
}