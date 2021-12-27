#include "..\..\Headers\EngineCore\EntityTypeMap.hpp"

using namespace firemelon;
using namespace boost::python;

EntityTypeMap::EntityTypeMap()
{
}

EntityTypeMap::~EntityTypeMap()
{
}

int EntityTypeMap::sizePy()
{
	PythonReleaseGil unlocker;

	return size();
}

int EntityTypeMap::size()
{
	return map_.size();
}
		
EntityList EntityTypeMap::getValuePy(EntityTypeId key)
{
	PythonReleaseGil unlocker;

	return getValue(key);
}

EntityList EntityTypeMap::getValue(EntityTypeId key)
{
	return map_[key];
}