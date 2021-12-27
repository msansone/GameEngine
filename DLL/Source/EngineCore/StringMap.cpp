#include "..\..\Headers\EngineCore\StringMap.hpp"

using namespace firemelon;
using namespace boost::python;

StringMap::StringMap()
{
}

StringMap::~StringMap()
{
}

int StringMap::size()
{
	return data_.size();
}
		
std::string StringMap::getValue(std::string key)
{
	return data_[key];
}

void StringMap::setValue(std::string key, std::string value)
{
	data_[key] = value;
}

stringmap StringMap::getMap()
{
	return data_;
}

std::list<std::string> StringMap::keys()
{
	std::list<std::string> keys;

	return keys;
}