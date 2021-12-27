#include "..\..\Headers\EngineCore\IdGenerator.hpp"

using namespace firemelon;

int IdGenerator::idIncrementer_ = 0;

IdGenerator::IdGenerator()
{
}

IdGenerator::~IdGenerator()
{

}

UniqueId IdGenerator::getNextId()
{
	return idIncrementer_++;
}