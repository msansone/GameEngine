#include "..\..\Headers\EngineCore\NameValidator.hpp"

using namespace firemelon;

NameValidator::NameValidator()
{
}

NameValidator::~NameValidator()
{
}
		
void NameValidator::addName(std::string name)
{
	if (findName(name) < 0)
	{
		names_.push_back(name);
	}
}

void NameValidator::removeName(std::string name)
{
	int pos = findName(name);

	if (pos >= 0)
	{
		names_.erase(names_.begin() + pos);
	}
}

bool NameValidator::isNameInUse(std::string name)
{
	if (findName(name) >= 0)
	{
		return true;
	}

	return false;
}

int NameValidator::findName(std::string name)
{
	int size = names_.size();

	for (int i = 0; i < size; i++)
	{
		if (names_[i] == name)
		{
			return i;
		}
	}

	return -1;
}