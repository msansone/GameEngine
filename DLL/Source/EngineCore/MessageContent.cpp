#include "..\..\Headers\EngineCore\MessageContent.hpp"

using namespace firemelon;
using namespace boost::python;

MessageContent::MessageContent()
{
	//std::cout << "Message created" << std::endl;
}

MessageContent::~MessageContent()
{
	//std::cout << "Message destroyed" << std::endl;
}

PyObj MessageContent::getPyInstance()
{
	return pyInstance_;
}