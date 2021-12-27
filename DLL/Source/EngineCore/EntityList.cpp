#include "..\..\Headers\EngineCore\EntityList.hpp"

using namespace firemelon;
using namespace boost::python;

EntityList::EntityList()
{
}

EntityList::~EntityList()
{
}

int EntityList::sizePy()
{
	PythonReleaseGil unlocker;

	return size();
}

int EntityList::size()
{
	return entityList_.size();
}
		
object EntityList::getValuePy(int index)
{
	PythonReleaseGil unlocker;

	return getValue(index);
}

object EntityList::getValue(int index)
{
	int size = entityList_.size();

	if (index >= 0 && index < size)
	{
		return entityList_[index]->getComponents()->getCodeBehindContainer()->getPythonInstanceWrapper()->getPyInstance();
	}
	else
	{
		std::cout << "Failed to get entity at index " << index << std::endl;

		return boost::python::object();
	}
}